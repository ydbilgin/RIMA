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
# Anchor the (project-local) PRIORITY config to THIS script's folder so `--list`
# etc. report correct state regardless of the caller's cwd (e.g. run from ~).
_CONFIG_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "cx_profiles.local.json")
# DISABLED state lives in the GLOBAL cx settings (single source of truth shared
# with `cx list` / `cx limits` / `cx enable|disable`). Priority stays project-local.
_PROFILE_ROOT = os.path.expandvars(r"%USERPROFILE%\.codex-profiles")
_GLOBAL_SETTINGS_PATH = os.path.join(_PROFILE_ROOT, ".cx-settings.json")
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


def _load_global_settings():
    """Read the global ~/.codex-profiles/.cx-settings.json (utf-8-sig tolerates the
    BOM PowerShell writes). {} on any failure."""
    try:
        with open(_GLOBAL_SETTINGS_PATH, encoding="utf-8-sig") as f:
            return json.load(f)
    except Exception:
        return {}


def _global_disabled():
    """The global 'disabled' bench list (lowercased) — same one `cx list`/`cx limits`
    show and `cx enable|disable` edit."""
    data = _load_global_settings()
    return {str(x).strip().lower() for x in (data.get("disabled") or []) if str(x).strip()}


def _set_global_disabled(name, disabled):
    """Add/remove a profile in the GLOBAL .cx-settings.json disabled list, preserving
    every other key (statusline, etc.). Returns the resulting sorted list."""
    data = _load_global_settings()
    cur = {str(x).strip().lower() for x in (data.get("disabled") or []) if str(x).strip()}
    name = name.strip().lower()
    if disabled:
        cur.add(name)
    else:
        cur.discard(name)
    data["disabled"] = sorted(cur)
    os.makedirs(_PROFILE_ROOT, exist_ok=True)
    with open(_GLOBAL_SETTINGS_PATH, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2)
    return data["disabled"]


def _clear_local_disabled(name):
    """Legacy: older versions stored disabled in cx_profiles.local.json. Drop NAME from
    there too so `--enable` works regardless of where it was benched. No-op if absent."""
    try:
        with open(_CONFIG_PATH, encoding="utf-8") as f:
            cfg = json.load(f)
    except Exception:
        return
    name = name.strip().lower()
    kept = [x for x in cfg.get("disabled", []) if str(x).strip().lower() != name]
    if kept != cfg.get("disabled", []):
        cfg["disabled"] = kept
        with open(_CONFIG_PATH, "w", encoding="utf-8") as f:
            json.dump(cfg, f, indent=2)


def _load_profile_config():
    """(disabled, priority) for cx profiles — editable WITHOUT touching code.
      - disabled: GLOBAL .cx-settings.json 'disabled' list (single source of truth,
        also honored by `cx list`/`cx limits`) UNIONed with any legacy local entries.
        Manage via `cx enable|disable <name>` or `cx_dispatch.py --enable|--disable`.
      - priority: project-local cx_profiles.local.json {"priority": [...]} — preferred
        dispatch order; unlisted logged-in profiles follow, oldest-refresh first.
    Every logged-in profile is auto-discovered from `cx accounts` (add via `cx add`)."""
    local_disabled, priority = set(), list(DEFAULT_PRIORITY)
    try:
        with open(_CONFIG_PATH, encoding="utf-8") as f:
            cfg = json.load(f)
        local_disabled = {str(x).strip().lower() for x in cfg.get("disabled", []) if str(x).strip()}
        priority = list(cfg.get("priority", [])) or list(DEFAULT_PRIORITY)
    except Exception:
        pass
    return (_global_disabled() | local_disabled), priority


def _ordered_eligible(profiles):
    """Auto-discovered candidates: every logged-in profile NOT disabled, ordered by
    (priority index, oldest LastRefresh). Replaces the old hardcoded allowlist."""
    disabled, priority = _load_profile_config()
    elig = [p for p in profiles if p["logged_in"] and p["name"].lower() not in disabled]
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

    # Suppress the powershell.exe console window FLASH on Windows. CREATE_NO_WINDOW
    # stops the conhost window from being allocated; STARTUPINFO SW_HIDE is a
    # belt-and-suspenders hidden-window state. capture_output pipes stdout/stderr,
    # so no visible console is needed.
    _run_kwargs = {}
    if sys.platform == "win32":
        _si = subprocess.STARTUPINFO()
        _si.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        _si.wShowWindow = 0  # SW_HIDE
        _run_kwargs["startupinfo"] = _si
        _run_kwargs["creationflags"] = subprocess.CREATE_NO_WINDOW

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
        **_run_kwargs,
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


_LIMITS_COLS = [
    "Profile", "Enabled", "Plan", "5h %", "5h reset in", "5h reset at",
    "Week %", "Week reset in", "Week reset at", "Status",
]


def _parse_limits(output):
    """Parse the `cx limits` fixed-width table -> {profile_name: {...}}.

    Values can contain spaces ('5h 0m', 'Sun 18:32', 'no auth.json'), so the
    table is sliced by the column offset of each header label (computed from the
    live header line) rather than split on whitespace. '%' columns coerce to int
    (None for '-'). 'status_ok' is True only when Status starts with 'OK' (i.e.
    NOT blocked / re-auth / no-auth). Returns {} if the header can't be located."""
    lines = output.replace("\r", "").strip("\n").split("\n")
    header_idx = next(
        (i for i, ln in enumerate(lines)
         if "Profile" in ln and "Status" in ln and "Week" in ln),
        None,
    )
    if header_idx is None:
        return {}
    header = lines[header_idx]
    starts, search_from = [], 0
    for c in _LIMITS_COLS:
        idx = header.find(c, search_from)
        if idx < 0:
            return {}
        starts.append(idx)
        search_from = idx + len(c)
    bounds = list(zip(starts, starts[1:] + [None]))

    def to_int(v):
        return int(v) if v.isdigit() else None

    table = {}
    for ln in lines[header_idx + 1:]:
        s = ln.strip()
        if not s or s.startswith("-"):
            continue
        vals = [ln[a:b].strip() if b is not None else ln[a:].strip() for a, b in bounds]
        rec = dict(zip(_LIMITS_COLS, vals))
        name = rec["Profile"]
        if not name:
            continue
        status = rec["Status"]
        table[name] = {
            "plan": rec["Plan"],
            "h5_pct": to_int(rec["5h %"]),
            "week_pct": to_int(rec["Week %"]),
            "week_reset_at": rec["Week reset at"],
            "status": status,
            "status_ok": status.strip().upper().startswith("OK"),
        }
    return table


def _limits_table():
    """Run `cx limits` and parse it into a per-profile dict; {} on any failure.

    Timeout is generous (60s): `cx limits` does one network round-trip per profile
    sequentially, behind nested PowerShell startup — 30s occasionally truncated it,
    which silently dropped quota-aware selection back to the LastRefresh fallback."""
    try:
        result = _ps_run(["limits"], timeout=60)
    except Exception:
        return {}
    return _parse_limits(result.stdout or "")


def _short_status(s):
    """Collapse a `cx limits` Status cell to a compact tag for the --list table."""
    if not s:
        return "-"
    u = s.upper()
    if u.startswith("OK"):
        return "OK"
    if "BLOCKED" in u:
        return "BLOCKED"
    if "RE-AUTH" in u:
        return "RE-AUTH"
    if "NO AUTH" in u:
        return "NO-AUTH"
    return s[:11]


def select_profile_quota_aware(profiles, week_cap=95, primary_cap=95):
    """Pick the enabled, logged-in profile with the most remaining WEEKLY quota.

    Data comes from `cx limits` (the same table the user sees), which is reliable
    — unlike the raw usage endpoint, which returned None here. Skips DISABLED
    (via _ordered_eligible), locked, BLOCKED/RE-AUTH/no-auth, and over-cap
    profiles. Sorts survivors by weekly% then 5h% (lowest first) to spread load.
    Returns None -> caller falls back to LastRefresh selection."""
    limits = _limits_table()
    if not limits:
        return None
    scored = []
    for p in _ordered_eligible(profiles):
        name = p["name"]
        if _is_profile_locked(name):
            print(f"  Skipping {name} (locked by parallel dispatch)", file=sys.stderr)
            continue
        rec = limits.get(name)
        if not rec or not rec["status_ok"]:
            continue
        wk, h5 = rec["week_pct"], rec["h5_pct"]
        if wk is None or wk >= week_cap:
            continue
        if h5 is not None and h5 >= primary_cap:
            continue
        scored.append((name, wk, h5 or 0))
    if not scored:
        return None
    scored.sort(key=lambda x: (x[1], x[2]))
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
    parser = argparse.ArgumentParser(
        description="Dispatch a task to Codex (cx) on an auto-selected profile, or manage which profiles are enabled/disabled.",
        epilog="Examples:\n"
               "  python cx_dispatch.py --list\n"
               "  python cx_dispatch.py --disable yekta\n"
               "  python cx_dispatch.py --enable yekta\n"
               "  python cx_dispatch.py --task-file STAGING/task.md --effort high",
        formatter_class=argparse.RawDescriptionHelpFormatter,
    )
    parser.add_argument("--task-file", default=None, help="Markdown task file to dispatch to Codex.")
    parser.add_argument("--effort", default="medium", choices=["low", "medium", "high", "xhigh"])
    parser.add_argument("--profile", default=None, help="Force a specific cx profile (skips auto-selection).")
    parser.add_argument("--timeout", type=int, default=1200, help="Subprocess timeout in seconds (default 1200=20min)")
    parser.add_argument("--list", action="store_true", dest="list_profiles",
                        help="List discovered cx profiles with enabled/DISABLED status + priority rank + last refresh, then exit.")
    parser.add_argument("--disable", default=None, metavar="PROFILE",
                        help="Mark a profile DISABLED (stays registered in `cx accounts`, but dispatch skips it), then exit.")
    parser.add_argument("--enable", default=None, metavar="PROFILE",
                        help="Re-ENABLE a previously disabled profile, then exit.")
    args = parser.parse_args()

    # --- Profile management actions (no dispatch). Writes the GLOBAL bench list so
    #     `cx list` / `cx limits` / `cx enable|disable` all stay in sync. ---
    if args.disable or args.enable:
        target = (args.disable or args.enable).strip().lower()
        try:
            known = {p["name"].lower() for p in get_profiles()}
            if target not in known:
                print(f"WARNING: '{target}' is not a currently-known profile {sorted(known)}. Proceeding anyway.", file=sys.stderr)
        except Exception:
            pass
        if args.disable:
            now = _set_global_disabled(target, True)
            print(f"DISABLED '{target}' (global ~/.codex-profiles/.cx-settings.json). "
                  f"Dispatch skips it and it shows DISABLED in `cx list`/`cx limits`. Disabled now: {now}")
        else:
            _set_global_disabled(target, False)
            _clear_local_disabled(target)  # drop legacy local entry too
            print(f"ENABLED '{target}'.")
        return

    if args.list_profiles:
        disabled, priority = _load_profile_config()
        profiles = get_profiles()
        limits = _limits_table()
        print(f"{'PROFILE':<20}{'STATE':<10}{'PRI':<5}{'5H%':<6}{'WEEK%':<7}{'CX_STATUS':<10}WEEK_RESET")
        for p in profiles:
            name = p["name"]
            lname = name.lower()
            state = "DISABLED" if lname in disabled else ("enabled" if p.get("logged_in") else "logged-out")
            try:
                rank = str(priority.index(lname))
            except ValueError:
                rank = "-"
            rec = limits.get(name, {})
            h5 = rec.get("h5_pct")
            wk = rec.get("week_pct")
            h5s = str(h5) if h5 is not None else "-"
            wks = str(wk) if wk is not None else "-"
            cxs = _short_status(rec.get("status"))
            wreset = rec.get("week_reset_at") or "-"
            print(f"{name:<20}{state:<10}{rank:<5}{h5s:<6}{wks:<7}{cxs:<10}{wreset}")
        if disabled:
            print(f"\nDisabled (skipped by dispatch): {sorted(disabled)}")
        print("Dispatch picks the enabled, non-DISABLED profile with CX_STATUS=OK and the lowest WEEK%.")
        return

    if not args.task_file:
        parser.error("--task-file is required (or use --list / --disable / --enable)")

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
