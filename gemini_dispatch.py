#!/usr/bin/env python3
"""gemini_dispatch.py — Profile-aware Gemini CLI dispatcher for RIMA.

Mirror of cx_dispatch.py for Codex, but routed through gchange (Gemini Auth
Manager v2.3). Picks oldest-used account (or honors --profile), switches via
gchange, runs gemini -m <model> -p <prompt>, records last-used time per
account, and returns the exit code.

Usage:
  python gemini_dispatch.py --prompt "..."
  python gemini_dispatch.py --prompt-file STAGING/some_task.md
  python gemini_dispatch.py --prompt "..." --model gemini-3.1-pro-preview
  python gemini_dispatch.py --prompt "..." --profile laurethayday@gmail.com
  python gemini_dispatch.py --prompt "..." --rotate
  python gemini_dispatch.py --prompt "..." --output STAGING/out.md

Defaults:
  --model    gemini-3.1-pro-preview
  --profile  auto-pick (oldest LastUsed)

Exit codes:
  0  success
  1  switch failed / gemini failed / arg error
"""

import argparse
import json
import os
import subprocess
import sys
import time
from pathlib import Path

GEMINI_DIR = Path(os.path.expanduser("~/.gemini"))
PROFILES_DIR = GEMINI_DIR / "auth_profiles"
ACCOUNTS_JSON = GEMINI_DIR / "google_accounts.json"
HISTORY_FILE = GEMINI_DIR / "dispatch_history.json"
GCHANGE_PY = GEMINI_DIR / "gemini_cli_auth_manager.py"

DEFAULT_MODEL = "gemini-3.1-pro-preview"

# Strip ANSI escape codes from gchange output so log files stay readable.
ANSI_RE = None
try:
    import re
    ANSI_RE = re.compile(r"\x1B\[[0-9;]*[mGKHF]")
except Exception:
    pass


def _strip_ansi(text):
    if ANSI_RE is None or not text:
        return text
    return ANSI_RE.sub("", text)


def list_accounts():
    """Return ordered list of all known account emails (active first)."""
    if ACCOUNTS_JSON.exists():
        try:
            with open(ACCOUNTS_JSON, encoding="utf-8") as f:
                data = json.load(f)
            active = data.get("active")
            old = data.get("old", [])
            if active:
                return [active] + [a for a in old if a != active]
            return list(old)
        except Exception:
            pass
    # Fallback: scan profile dirs
    if PROFILES_DIR.exists():
        return sorted(d.name for d in PROFILES_DIR.iterdir() if d.is_dir())
    return []


def load_history():
    if HISTORY_FILE.exists():
        try:
            with open(HISTORY_FILE, encoding="utf-8") as f:
                return json.load(f)
        except Exception:
            return {}
    return {}


def save_history(h):
    try:
        HISTORY_FILE.write_text(json.dumps(h, indent=2), encoding="utf-8")
    except Exception as e:
        print(f"[gemini_dispatch] Warning: history save failed: {e}", file=sys.stderr)


def pick_oldest(accounts):
    """Pick the account with the oldest last-used timestamp (or never used)."""
    h = load_history()
    never_used = [a for a in accounts if a not in h]
    if never_used:
        return never_used[0]
    return min(accounts, key=lambda a: h.get(a, 0))


def get_active_account():
    """Read currently active account email from gchange's accounts file."""
    if not ACCOUNTS_JSON.exists():
        return None
    try:
        with open(ACCOUNTS_JSON, encoding="utf-8") as f:
            return json.load(f).get("active")
    except Exception:
        return None


def switch_account(email):
    """Switch active gchange account. Returns True on success."""
    if not GCHANGE_PY.exists():
        print(f"[gemini_dispatch] ERROR: gchange manager not found: {GCHANGE_PY}", file=sys.stderr)
        return False
    try:
        result = subprocess.run(
            [sys.executable, str(GCHANGE_PY), email],
            capture_output=True,
            text=True,
            timeout=30,
        )
        if result.returncode != 0:
            print("[gemini_dispatch] gchange switch failed:", file=sys.stderr)
            print(_strip_ansi(result.stdout or "") + _strip_ansi(result.stderr or ""), file=sys.stderr)
            return False
        return True
    except subprocess.TimeoutExpired:
        print("[gemini_dispatch] gchange switch timed out (30s)", file=sys.stderr)
        return False


def rotate_next():
    """Use 'gchange next' to advance to next account."""
    if not GCHANGE_PY.exists():
        return False
    try:
        result = subprocess.run(
            [sys.executable, str(GCHANGE_PY), "next"],
            capture_output=True,
            text=True,
            timeout=30,
        )
        return result.returncode == 0
    except subprocess.TimeoutExpired:
        return False


def run_gemini(prompt, model, output_file=None, timeout=600):
    """Invoke gemini CLI with prompt + model. Returns exit code."""
    cmd = ["gemini", "-m", model, "--yolo", "-p", prompt]
    try:
        if output_file:
            with open(output_file, "w", encoding="utf-8") as f:
                result = subprocess.run(
                    cmd,
                    stdout=f,
                    stderr=subprocess.STDOUT,
                    timeout=timeout,
                )
        else:
            result = subprocess.run(cmd, timeout=timeout)
        return result.returncode
    except subprocess.TimeoutExpired:
        print(f"[gemini_dispatch] gemini timeout after {timeout}s", file=sys.stderr)
        return 124


def main():
    parser = argparse.ArgumentParser(description=__doc__.split("\n\n")[0])
    parser.add_argument("--prompt", help="Prompt text (or use --prompt-file)")
    parser.add_argument("--prompt-file", help="Read prompt from file")
    parser.add_argument("--model", default=DEFAULT_MODEL, help=f"Gemini model (default {DEFAULT_MODEL})")
    parser.add_argument("--profile", help="Specific account email (default: auto-pick oldest LastUsed)")
    parser.add_argument("--output", help="Write Gemini output to file (else stdout)")
    parser.add_argument("--rotate", action="store_true", help="Use 'gchange next' instead of pick-oldest")
    parser.add_argument("--timeout", type=int, default=600, help="Gemini call timeout in seconds (default 600)")
    parser.add_argument("--dry-run", action="store_true", help="Show which account would be used, do not call Gemini")
    args = parser.parse_args()

    # Resolve prompt
    if args.prompt_file:
        try:
            with open(args.prompt_file, encoding="utf-8") as f:
                prompt = f.read()
        except Exception as e:
            print(f"[gemini_dispatch] ERROR reading --prompt-file: {e}", file=sys.stderr)
            sys.exit(1)
    elif args.prompt:
        prompt = args.prompt
    else:
        print("[gemini_dispatch] ERROR: either --prompt or --prompt-file required", file=sys.stderr)
        sys.exit(1)

    # Account selection
    accounts = list_accounts()
    if not accounts:
        print("[gemini_dispatch] ERROR: no accounts found in ~/.gemini/auth_profiles/", file=sys.stderr)
        sys.exit(1)

    target = None
    if args.profile:
        if args.profile not in accounts:
            print(f"[gemini_dispatch] ERROR: profile '{args.profile}' not in accounts: {accounts}", file=sys.stderr)
            sys.exit(1)
        target = args.profile
    elif args.rotate:
        print("[gemini_dispatch] Rotating via gchange next ...", file=sys.stderr)
        if not rotate_next():
            print("[gemini_dispatch] ERROR: rotate next failed", file=sys.stderr)
            sys.exit(1)
        target = get_active_account()
    else:
        target = pick_oldest(accounts)

    current = get_active_account()
    if target and target != current:
        print(f"[gemini_dispatch] Switching {current} -> {target}", file=sys.stderr)
        if not switch_account(target):
            sys.exit(1)
    else:
        print(f"[gemini_dispatch] Using already-active account: {target}", file=sys.stderr)

    if args.dry_run:
        print(f"[gemini_dispatch] DRY RUN. Would call gemini -m {args.model} on account {target}", file=sys.stderr)
        sys.exit(0)

    # Record use
    h = load_history()
    if target:
        h[target] = time.time()
    save_history(h)

    # Run Gemini
    print(f"[gemini_dispatch] Running model={args.model}, output={args.output or 'stdout'}", file=sys.stderr)
    rc = run_gemini(prompt, args.model, args.output, args.timeout)
    sys.exit(rc)


if __name__ == "__main__":
    main()
