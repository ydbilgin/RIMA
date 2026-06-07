# Task: notebooklm-mcp-cli login failure — GitHub issues + community evaluation

ACTIVE RULES: (1) think before answering (2) minimal, evidence-grounded (3) cite exact issue/PR URLs and quote relevant lines (4) if unresolved, say BLOCKED + best-available workaround.
NLM ACCESS: N/A — NLM is the SUBJECT of this research and is currently broken; do NOT try to query it. Direct-read only this file.
RESPOND INLINE in your final answer (which the dispatcher captures into AGY_DONE_*.md). Do NOT write to scratch / external files.

## The exact symptom (verbatim from the user's terminal, today)
The user runs:
```
nlm login --clear
```
and gets:
```
Authenticating profile: default (None)
✗ Authentication Error
  Authentication expired. Run 'nlm login' in your terminal to re-authenticate.
  MCP users: the server should auto-detect the new credentials; if not, call the refresh_auth tool.
→ Run nlm login to re-authenticate
```
So `--clear` itself FAILS instead of clearing + reprompting. Historically the user logged in via a **dedicated/isolated browser window** (CDP-driven OAuth that captures Google session cookies). That login flow now errors. This has recurred multiple times across sessions (the auth does not durably hold).

## What a prior research pass (ax) already found + DID (so don't repeat it — verify/extend it)
- Root causes claimed: (1) hardcoded Linux/Mac **User-Agent** in API calls vs the Windows-Chrome login session → Google revokes the session; (2) modern Chrome stores cookies at `Default/Network/Cookies`, but the CLI only checked `Default/Cookies` → headless refresh disabled; (3) a retry loop reloaded stale cookies instead of refreshing.
- It **patched the installed package** (`utils/cdp.py`, `core/base.py`, `core/auth.py`) and told the user to run `nlm login --clear`. The user did → STILL the error above. So either the patch is incomplete, was overwritten by `uvx` re-resolving the package, or the real cause is upstream.
- NOTE: the tool is invoked via `uvx --from notebooklm-mcp-cli nlm ...`, so **uvx may run a fresh/cached copy that does NOT include the local patches** — flag this if relevant.

## YOUR JOB — evaluate the upstream repo + community, then give a concrete fix
1. Identify the **actual** GitHub repo behind the PyPI package `notebooklm-mcp-cli` (candidate: `jacob-bd/notebooklm-mcp-cli` — VERIFY the real owner/name; check PyPI project page "Homepage"/"Source" links). Give the canonical repo URL.
2. Read the **issue tracker** (open + closed) and any discussions/PRs. Look specifically for:
   - "Authentication expired" / "login --clear" failing / login loop / session not persisting / cookies expiring quickly
   - the dedicated-browser / CDP login flow breaking
   - Windows-specific auth problems
   - `refresh_auth` tool guidance
   How many users report this? Is it acknowledged by the maintainer? Any fix merged, release, or pinned workaround?
3. Check the **latest version + changelog/releases** — is there a newer version that fixes auth? What version does `uvx --from notebooklm-mcp-cli` resolve to vs latest? Should the user **pin a version** or upgrade?
4. Determine the **correct current login procedure** for this tool on Windows (exact command sequence, whether Chrome must be fully closed, profile flags, where cookies/auth.json land, how to verify with `login --check`).
5. Assess the **architectural fallback** the user keeps hitting: since this auth is fragile and recurring, is the realistic answer to (a) pin a known-good version, (b) switch auth method, (c) reduce dependence by snapshotting the canonical design notebook to local Markdown? Give a recommendation.

## Deliverable (inline, concise, ranked)
- **Repo URL** (verified) + latest version + whether uvx is running stale code.
- **Issue findings:** 3-6 most relevant issues/PRs with URLs + 1-line each on what they say. Is this a known widespread bug?
- **THE FIX:** the exact command(s) the user should run RIGHT NOW to get logged in again (ranked: most-likely-to-work first), and why each should work.
- **Durable fix:** what stops the recurrence (version pin / config / alt path) + the fallback recommendation.
Keep it tight and actionable — the user is blocked and wants to know "is it just me, and what do I type."
