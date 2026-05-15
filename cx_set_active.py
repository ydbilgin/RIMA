"""cx_set_active.py — Rotate the profile that codex plugin uses.

Plugin (codex-companion.mjs) reads ~/.codex/auth.json. cx_dispatch.py rotates
via $env:CODEX_HOME but plugin never sees that env var, so it would always
log in as whichever account auth.json currently belongs to.

This script copies the chosen profile's auth.json over ~/.codex/auth.json,
so the next plugin invocation runs as that account.

Usage:
  python cx_set_active.py                # auto: oldest LastRefresh profile
  python cx_set_active.py laurethgame    # explicit profile
  python cx_set_active.py --show         # report current active account only
"""

import json
import os
import shutil
import subprocess
import sys
from pathlib import Path

from cx_dispatch import CX_CMD, _parse_accounts, PROFILE_ORDER

HOME = Path(os.environ["USERPROFILE"])
CODEX_HOME = HOME / ".codex"
PROFILE_ROOT = HOME / ".codex-profiles"
ACTIVE_AUTH = CODEX_HOME / "auth.json"
ACTIVE_MARKER = CODEX_HOME / ".cx-active-profile"


def list_profiles():
    res = subprocess.run(
        ["cmd", "/c", CX_CMD, "accounts"],
        capture_output=True, text=True, timeout=30,
    )
    return _parse_accounts(res.stdout)


def current_email():
    if not ACTIVE_AUTH.exists():
        return None
    try:
        data = json.loads(ACTIVE_AUTH.read_text(encoding="utf-8"))
        # Decode the JWT payload (middle segment, base64url).
        import base64
        idt = data.get("tokens", {}).get("id_token", "")
        if not idt:
            return None
        payload = idt.split(".")[1]
        payload += "=" * (-len(payload) % 4)
        decoded = json.loads(base64.urlsafe_b64decode(payload))
        return decoded.get("email")
    except Exception as e:
        return f"<parse error: {e}>"


def current_marker():
    if ACTIVE_MARKER.exists():
        return ACTIVE_MARKER.read_text(encoding="utf-8").strip()
    return None


def pick_oldest(profiles):
    candidates = [
        p for p in profiles
        if p["logged_in"] and p["last_refresh"] and p["name"] in PROFILE_ORDER
    ]
    if not candidates:
        return None
    candidates.sort(key=lambda p: p["last_refresh"])
    return candidates[0]["name"]


def switch(profile_name):
    src = PROFILE_ROOT / profile_name / "auth.json"
    if not src.exists():
        sys.exit(f"FAIL: source auth not found: {src}")
    CODEX_HOME.mkdir(exist_ok=True)
    if ACTIVE_AUTH.exists():
        backup = CODEX_HOME / "auth.json.prev"
        shutil.copy2(ACTIVE_AUTH, backup)
    shutil.copy2(src, ACTIVE_AUTH)
    ACTIVE_MARKER.write_text(profile_name, encoding="utf-8")
    return profile_name


def main():
    args = sys.argv[1:]
    if "--show" in args:
        print(f"active marker : {current_marker() or '(none)'}")
        print(f"active email  : {current_email() or '(none)'}")
        return

    profiles = list_profiles()
    if args and not args[0].startswith("-"):
        target = args[0]
        if target not in PROFILE_ORDER:
            sys.exit(f"FAIL: unknown profile '{target}'. Known: {PROFILE_ORDER}")
        if not any(p["name"] == target and p["logged_in"] for p in profiles):
            sys.exit(f"FAIL: profile '{target}' not logged in.")
    else:
        target = pick_oldest(profiles)
        if not target:
            sys.exit("FAIL: no logged-in profile with LastRefresh found.")

    prev_marker = current_marker()
    prev_email = current_email()
    switch(target)
    print(f"PROFILE_ROTATED:")
    print(f"  prev: {prev_marker or '(unmarked)'} ({prev_email})")
    print(f"  now : {target} ({current_email()})")


if __name__ == "__main__":
    main()
