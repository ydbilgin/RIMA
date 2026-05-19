"""cx_limits.py — Live rate-limit display for all Codex profiles.

Usage:
  python STAGING/cx_limits.py

Calls the same backend endpoint Codex CLI's interactive TUI uses
(https://chatgpt.com/backend-api/codex/usage) per profile, using each
profile's auth.json bearer token + cap_sid cookie. Read-only, no dispatch.

Output: aligned table — Profile / Plan / 5h % / 5h reset / Week % / Week reset / Status.
401 token_revoked profiles flagged with "RE-AUTH (cx login <profile>)".
"""

import json
import os
import urllib.request
import urllib.error
from datetime import datetime

PROFILES = ["laurethayday", "laurethgame", "yasinderyabilgin"]
ENDPOINT = "https://chatgpt.com/backend-api/codex/usage"


def fetch(profile):
    auth_path = os.path.expandvars(rf"%USERPROFILE%\.codex-profiles\{profile}\auth.json")
    cap_path = os.path.expandvars(rf"%USERPROFILE%\.codex-profiles\{profile}\cap_sid")
    try:
        with open(auth_path) as f:
            token = json.load(f)["tokens"]["access_token"]
    except FileNotFoundError:
        return {"_err": "no auth.json"}
    except Exception as e:
        return {"_err": f"auth read: {e}"}

    cap = ""
    if os.path.exists(cap_path):
        with open(cap_path) as f:
            cap = f.read().strip()

    headers = {
        "Authorization": f"Bearer {token}",
        "User-Agent": "codex_cli_rs/0.0.0",
        "Accept": "application/json",
        "Origin": "https://chatgpt.com",
        "Referer": "https://chatgpt.com/",
    }
    if cap:
        headers["Cookie"] = f"cap_sid={cap}"

    req = urllib.request.Request(ENDPOINT, headers=headers)
    try:
        with urllib.request.urlopen(req, timeout=10) as r:
            return json.loads(r.read().decode())
    except urllib.error.HTTPError as e:
        body = ""
        try:
            body = e.read().decode()[:120]
        except Exception:
            pass
        return {"_err": f"HTTP {e.code}", "_body": body}
    except Exception as e:
        return {"_err": f"{type(e).__name__}: {e}"}


def reset_str(secs):
    if secs is None:
        return "?"
    h, rem = divmod(int(secs), 3600)
    m = rem // 60
    return f"{h // 24}d {h % 24}h" if h >= 24 else f"{h}h {m}m"


def reset_at_str(unix_ts):
    if unix_ts is None:
        return "?"
    try:
        dt = datetime.fromtimestamp(int(unix_ts))
        now = datetime.now()
        if dt.date() == now.date():
            return dt.strftime("%H:%M")
        elif (dt - now).days < 7:
            return dt.strftime("%a %H:%M")
        return dt.strftime("%m-%d %H:%M")
    except Exception:
        return "?"


def main():
    cols = ("Profile", "Plan", "5h %", "5h reset in", "5h reset at", "Week %", "Week reset in", "Week reset at", "Status")
    widths = (18, 6, 6, 13, 14, 7, 13, 14, 30)
    header = " ".join(c.ljust(w) for c, w in zip(cols, widths))
    print(header)
    print("-" * len(header))

    for p in PROFILES:
        r = fetch(p)
        if "_err" in r:
            err = r["_err"]
            status = err
            if "401" in err:
                status = f"RE-AUTH (cx login {p})"
            cells = (p, "-", "-", "-", "-", "-", "-", "-", status)
            print(" ".join(str(c).ljust(w) for c, w in zip(cells, widths)))
            continue

        rl = r.get("rate_limit", {})
        pri = rl.get("primary_window", {}) or {}
        sec = rl.get("secondary_window", {}) or {}
        allowed = rl.get("allowed", "?")
        plan = r.get("plan_type", "?")
        status = "OK" if allowed and not rl.get("limit_reached", False) else "BLOCKED"

        cells = (
            p,
            plan,
            f"{pri.get('used_percent', '?')}",
            reset_str(pri.get("reset_after_seconds")),
            reset_at_str(pri.get("reset_at")),
            f"{sec.get('used_percent', '?')}",
            reset_str(sec.get("reset_after_seconds")),
            reset_at_str(sec.get("reset_at")),
            status,
        )
        print(" ".join(str(c).ljust(w) for c, w in zip(cells, widths)))


if __name__ == "__main__":
    main()
