"""cx_dispatch.py — Profile-aware Codex dispatcher for RIMA.

Usage:
  python cx_dispatch.py --task-file path/to/task.md [--effort medium] [--profile laurethayday]

Selects optimal cx profile (oldest LastRefresh), writes task to CODEX_TASK.md,
dispatches via cx exec, waits for CODEX_DONE.md, prints result and exits.
Exit 0 = DONE, Exit 1 = FAILED/BLOCKED/no result.
"""

import subprocess
import sys
import os
import re
import argparse
from datetime import datetime, timezone

CX_CMD = r"C:\Users\ydbil\AppData\Roaming\npm\cx.cmd"
PROFILE_ORDER = ["laurethayday", "laurethgame", "yasinderyabilgin"]
CODEX_TASK_FILE = "CODEX_TASK.md"
CODEX_DONE_FILE = "CODEX_DONE.md"

# Variables that trigger the Conda shell hook inside cmd.exe.
# Stripping them from the child env prevents the
# "CONDA_SHLVL was unexpected at this time" parse error.
_CONDA_VARS = {
    "CONDA_SHLVL", "CONDA_DEFAULT_ENV", "CONDA_PREFIX",
    "CONDA_EXE", "CONDA_PYTHON_EXE", "CONDA_PROMPT_MODIFIER",
    "_CE_CONDA", "_CE_M",
}


def _clean_env():
    """Return a copy of os.environ with all Conda hook variables removed."""
    env = os.environ.copy()
    for var in _CONDA_VARS:
        env.pop(var, None)
    return env


def _ps_run(cx_args, timeout=30):
    """Invoke cx.cmd via PowerShell to avoid cmd.exe Conda hook failures.

    PowerShell is used because:
    - ``cmd /c cx.cmd`` triggers ``CONDA_SHLVL was unexpected at this time``
      which crashes the cx.cmd batch file before cx ever runs.
    - The same invocation via PowerShell ``& 'cx.cmd' args`` works correctly
      (confirmed in-session).
    - ``-NoProfile`` skips the PS user profile (which may also source Conda
      hooks) and keeps startup fast.
    """
    # Build the PowerShell -Command string: & 'cx.cmd' arg1 arg2 ...
    # Single-quote each argument to handle spaces; escape embedded single
    # quotes by doubling them.
    def ps_quote(s):
        return "'" + s.replace("'", "''") + "'"

    ps_cmd = "& " + ps_quote(CX_CMD) + " " + " ".join(ps_quote(a) for a in cx_args)

    return subprocess.run(
        [
            "powershell",
            "-NoProfile",
            "-NonInteractive",
            "-Command",
            ps_cmd,
        ],
        stdin=subprocess.DEVNULL,
        capture_output=True,
        text=True,
        encoding="utf-8",
        errors="replace",
        timeout=timeout,
        env=_clean_env(),
    )


def get_profiles():
    result = _ps_run(["accounts"], timeout=30)
    return _parse_accounts(result.stdout)


def _parse_accounts(output):
    # cx accounts emits a fixed-width column table. Variable-width Name/Profile
    # columns can collide with single-space-separated neighbors, so regex split
    # on whitespace is unreliable. Anchor on the trailing ISO-8601 timestamp
    # (the LastRefresh column) and treat the rest as best-effort.
    profiles = []
    iso_re = re.compile(r"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z?)\s*$")
    for line in output.strip().split("\n")[2:]:
        stripped = line.strip()
        if not stripped or stripped.startswith("-"):
            continue
        name = stripped.split(None, 1)[0]
        logged_in = "logged in" in line and "not logged in" not in line
        last_refresh = None
        m = iso_re.search(stripped)
        if logged_in and m:
            try:
                last_refresh = datetime.fromisoformat(m.group(1).replace("Z", "+00:00"))
            except ValueError:
                pass
        profiles.append({"name": name, "logged_in": logged_in, "last_refresh": last_refresh})
    return profiles


def select_profile(profiles):
    candidates = [
        p for p in profiles
        if p["logged_in"] and p["last_refresh"] and p["name"] in PROFILE_ORDER
    ]
    if candidates:
        candidates.sort(key=lambda p: p["last_refresh"])
        return candidates[0]["name"]
    for name in PROFILE_ORDER:
        for p in profiles:
            if p["name"] == name and p["logged_in"]:
                return p["name"]
    return None


def dispatch(profile, task_content, effort, task_file_path=None):
    # Use profile-specific task/done files to avoid parallel dispatch race condition
    safe_profile = re.sub(r"[^a-zA-Z0-9_-]", "_", profile)
    task_file = f"CODEX_TASK_{safe_profile}.md"
    done_file = f"CODEX_DONE_{safe_profile}.md"

    wrapper = f"ALWAYS WRITE YOUR RESULT SUMMARY TO {done_file} AS THE VERY LAST STEP."
    task_content = f"{wrapper}\n\n{task_content}\n\n---\n{wrapper}"

    task_path = os.path.abspath(task_file)
    with open(task_path, "w", encoding="utf-8") as f:
        f.write(task_content)
    open(done_file, "w").close()

    cx_args = [
        "run", profile, "exec",
        "--skip-git-repo-check",
        "--color", "never",
        "--dangerously-bypass-approvals-and-sandbox",
        "--config", f"model_reasoning_effort={effort}",
        f"Read {task_file} and execute every step using shell commands. Do not describe — actually run them.",
    ]
    result = _ps_run(cx_args, timeout=1200)
    if result.stdout:
        print(result.stdout, file=sys.stderr)

    # Prefer profile-specific DONE file, fallback to shared CODEX_DONE.md
    for df in [done_file, CODEX_DONE_FILE]:
        try:
            done = open(df, encoding="utf-8").read().strip()
            if done:
                # Append to shared CODEX_DONE.md for history
                if df != CODEX_DONE_FILE:
                    with open(CODEX_DONE_FILE, "a", encoding="utf-8") as f:
                        f.write("\n" + done)
                return done
        except FileNotFoundError:
            pass

    # Fallback: extract STATUS block from cx stdout
    match = re.search(r"(STATUS:.*)", result.stdout, re.DOTALL)
    if match:
        summary = match.group(1).strip()
        with open(CODEX_DONE_FILE, "w", encoding="utf-8") as f:
            f.write(summary)
        return summary

    # Last resort: if cx exited 0, Codex completed but forgot to write CODEX_DONE.md
    if result.returncode == 0:
        summary = "STATUS: COMPLETED (inferred — Codex exit 0, CODEX_DONE.md not written)\nCheck git log for changes."
        with open(CODEX_DONE_FILE, "w", encoding="utf-8") as f:
            f.write(summary)
        return summary

    return ""


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--task-file", required=True)
    parser.add_argument("--effort", default="medium", choices=["low", "medium", "high", "xhigh"])
    parser.add_argument("--profile", default=None)
    args = parser.parse_args()

    with open(args.task_file, encoding="utf-8") as f:
        task_content = f.read()

    if args.profile:
        profile = args.profile
    else:
        profiles = get_profiles()
        profile = select_profile(profiles)
        if not profile:
            print("ERROR: no logged-in profile", file=sys.stderr)
            sys.exit(1)
        print(f"PROFILE_SELECTED: {profile}", file=sys.stderr)

    result = dispatch(profile, task_content, args.effort)

    if result:
        print(result)

    failed = not result or any(kw in result for kw in ("BLOCKED", "FAILED", "STATUS: PARTIAL"))
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()
