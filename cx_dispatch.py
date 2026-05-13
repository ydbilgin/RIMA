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


def get_profiles():
    result = subprocess.run(
        ["cmd", "/c", CX_CMD, "accounts"],
        capture_output=True, text=True, timeout=30,
    )
    return _parse_accounts(result.stdout)


def _parse_accounts(output):
    profiles = []
    for line in output.strip().split("\n")[2:]:
        if not line.strip():
            continue
        parts = re.split(r"\s{2,}", line.strip())
        if not parts:
            continue
        name = parts[0]
        logged_in = "logged in" in line and "not logged in" not in line
        last_refresh = None
        if logged_in and len(parts) >= 6:
            try:
                last_refresh = datetime.fromisoformat(parts[-1].strip().replace("Z", "+00:00"))
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

    cmd = [
        "cmd", "/c", CX_CMD, "run", profile, "exec",
        "--skip-git-repo-check",
        "--color", "never",
        "--dangerously-bypass-approvals-and-sandbox",
        "--config", f"model_reasoning_effort={effort}",
        f"Read {task_file} and execute every step using shell commands. Do not describe — actually run them.",
    ]
    result = subprocess.run(cmd, stdin=subprocess.DEVNULL, capture_output=True, text=True, encoding="utf-8", errors="replace", timeout=1200)
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
