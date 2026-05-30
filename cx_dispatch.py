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
import json
import urllib.request
import urllib.error
from datetime import datetime, timezone

# Windows console is cp1254 here; print(result) crashes on chars like U+2192 (→).
# Force utf-8 with replacement so dispatch never dies just printing a result.
try:
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")
except Exception:
    pass

CX_CMD = r"C:\Users\ydbil\AppData\Roaming\npm\cx.cmd"
DEFAULT_PRIORITY = ["yekta", "laurethayday", "laurethgame", "yasinderyabilgin"]  # fallback order; cx_profiles.local.json overrides
CODEX_TASK_FILE = "CODEX_TASK.md"
CODEX_DONE_FILE = "CODEX_DONE.md"
LOCK_DIR = ".cx_dispatch_locks"
LOCK_STALE_SECS = 7200  # 2 hours — auto-release ghost locks

# Variables that trigger the Conda shell hook inside cmd.exe.
# Stripping them from the child env prevents the
# "CONDA_SHLVL was unexpected at this time" parse error.
_CONDA_VARS = {
    "CONDA_SHLVL", "CONDA_DEFAULT_ENV", "CONDA_PREFIX",
    "CONDA_EXE", "CONDA_PYTHON_EXE", "CONDA_PROMPT_MODIFIER",
    "_CE_CONDA", "_CE_M",
}


def _load_profile_config():
    """Active/passive + priority for cx profiles — editable WITHOUT touching code.
    Reads cx_profiles.local.json (cwd): {"disabled": ["yekta"], "priority": ["yekta","laurethgame"]}
      - disabled: profiles to skip (passive). Bench a temp account (e.g. 'yekta') by listing it here.
      - priority: preferred order; unlisted logged-in profiles follow, oldest-refresh first.
    Missing/invalid file -> no disabled, DEFAULT_PRIORITY order. Every logged-in profile is
    auto-discovered from `cx accounts` (add new ones with `cx add` — no code edit needed)."""
    try:
        with open("cx_profiles.local.json", encoding="utf-8") as f:
            cfg = json.load(f)
        disabled = set(cfg.get("disabled", []))
        priority = list(cfg.get("priority", [])) or list(DEFAULT_PRIORITY)
        return disabled, priority
    except Exception:
        return set(), list(DEFAULT_PRIORITY)


def _ordered_eligible(profiles):
    """Auto-discovered candidates: every logged-in profile NOT disabled, ordered by
    (priority index, oldest LastRefresh). Replaces the old hardcoded allowlist."""
    disabled, priority = _load_profile_config()
    elig = [p for p in profiles if p["logged_in"] and p["name"] not in disabled]
    far = datetime.max.replace(tzinfo=timezone.utc)

    def keyf(p):
        try:
            pi = priority.index(p["name"])
        except ValueError:
            pi = len(priority)
        return (pi, p["last_refresh"] or far)

    elig.sort(key=keyf)
    return elig


def _is_profile_locked(profile):
    """Check if profile has an active lock (another dispatch using it)."""
    lock_path = os.path.join(LOCK_DIR, f"{profile}.lock")
    if not os.path.exists(lock_path):
        return False
    try:
        age = datetime.now(timezone.utc).timestamp() - os.path.getmtime(lock_path)
        if age > LOCK_STALE_SECS:
            os.remove(lock_path)
            return False
        return True
    except (FileNotFoundError, OSError):
        return False


def _acquire_profile_lock(profile):
    os.makedirs(LOCK_DIR, exist_ok=True)
    lock_path = os.path.join(LOCK_DIR, f"{profile}.lock")
    with open(lock_path, "w") as f:
        f.write(str(os.getpid()))
    return lock_path


def _release_profile_lock(lock_path):
    try:
        os.remove(lock_path)
    except (FileNotFoundError, OSError):
        pass


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
    # Auto-discovered: first unlocked logged-in profile (minus disabled), by priority+refresh.
    for p in _ordered_eligible(profiles):
        if not _is_profile_locked(p["name"]):
            return p["name"]
    return None


def _fetch_live_limits(profile):
    """Query the Codex backend usage endpoint for live rate-limit data.

    Same endpoint Codex CLI's interactive TUI uses. Returns
    (primary_pct, secondary_pct) or None on any failure (auth revoked,
    network down, schema drift). Caller treats None as "skip this profile".
    """
    auth_path = os.path.expandvars(rf"%USERPROFILE%\.codex-profiles\{profile}\auth.json")
    cap_path = os.path.expandvars(rf"%USERPROFILE%\.codex-profiles\{profile}\cap_sid")
    try:
        with open(auth_path) as f:
            token = json.load(f)["tokens"]["access_token"]
    except Exception:
        return None

    cap = ""
    if os.path.exists(cap_path):
        try:
            with open(cap_path) as f:
                cap = f.read().strip()
        except Exception:
            pass

    headers = {
        "Authorization": f"Bearer {token}",
        "User-Agent": "codex_cli_rs/0.0.0",
        "Accept": "application/json",
        "Origin": "https://chatgpt.com",
        "Referer": "https://chatgpt.com/",
    }
    if cap:
        headers["Cookie"] = f"cap_sid={cap}"

    req = urllib.request.Request(
        "https://chatgpt.com/backend-api/codex/usage", headers=headers
    )
    try:
        with urllib.request.urlopen(req, timeout=8) as r:
            data = json.loads(r.read().decode())
    except Exception:
        return None

    rl = data.get("rate_limit", {})
    if not rl.get("allowed", True) or rl.get("limit_reached", False):
        return None
    pri = (rl.get("primary_window") or {}).get("used_percent")
    sec = (rl.get("secondary_window") or {}).get("used_percent")
    if pri is None or sec is None:
        return None
    return (pri, sec)


def select_profile_quota_aware(profiles, primary_cap=85, secondary_cap=90):
    """Pick the logged-in profile with the most remaining quota.

    - Calls the live usage endpoint per profile.
    - Skips profiles that return auth/network errors (None) or are over caps.
    - Sorts surviving candidates by weekly% then primary% (lowest first).
    - Returns None if no profile is usable — caller should fall back to
      LastRefresh-based selection.
    """
    scored = []
    for p in _ordered_eligible(profiles):
        if _is_profile_locked(p["name"]):
            print(f"  Skipping {p['name']} (locked by parallel dispatch)", file=sys.stderr)
            continue
        lim = _fetch_live_limits(p["name"])
        if lim is None:
            continue
        pri, sec = lim
        if pri >= primary_cap or sec >= secondary_cap:
            continue
        scored.append((p["name"], pri, sec))
    if not scored:
        return None
    scored.sort(key=lambda x: (x[2], x[1]))
    return scored[0][0]


def dispatch(profile, task_content, effort, task_file_path=None, timeout=1200):
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
    lock_path = _acquire_profile_lock(profile)
    try:
        result = _ps_run(cx_args, timeout=timeout)
    finally:
        _release_profile_lock(lock_path)
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
    parser.add_argument("--timeout", type=int, default=1200, help="Subprocess timeout in seconds (default 1200=20min)")
    args = parser.parse_args()

    with open(args.task_file, encoding="utf-8") as f:
        task_content = f.read()

    if args.profile:
        profile = args.profile
    else:
        profiles = get_profiles()
        profile = select_profile_quota_aware(profiles)
        if profile:
            print(f"PROFILE_SELECTED (quota-aware): {profile}", file=sys.stderr)
        else:
            profile = select_profile(profiles)
            if not profile:
                print("ERROR: no logged-in profile", file=sys.stderr)
                sys.exit(1)
            print(f"PROFILE_SELECTED (LastRefresh fallback): {profile}", file=sys.stderr)

    result = dispatch(profile, task_content, args.effort, timeout=args.timeout)

    if result:
        print(result)

    # Failure detection is anchored to a STATUS: line, NOT a naive substring
    # scan. A rich result legitimately containing the words "BLOCKED"/"FAILED"
    # somewhere in its body (e.g. flagging one sub-point as unverified) used to
    # trip the old `kw in result` check and exit 1 on a fully-successful run.
    # Now: empty result = failure; a STATUS: line decides; otherwise non-empty
    # output = success.
    status_match = re.search(r"^\s*STATUS:\s*(.+)$", result, re.MULTILINE | re.IGNORECASE)
    if not result.strip():
        failed = True
    elif status_match:
        failed = any(kw in status_match.group(1).upper() for kw in ("BLOCKED", "FAILED", "PARTIAL"))
    else:
        failed = False
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()
