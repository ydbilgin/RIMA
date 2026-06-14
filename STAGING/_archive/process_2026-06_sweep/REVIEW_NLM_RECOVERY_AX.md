# AX RESEARCH — NLM (notebooklm-mcp-cli) keeps breaking — find a DURABLE fix

ACTIVE RULES: (1) think (2) min, no speculation (3) surgical (4) flag if unclear.
You are a RESEARCHER. Respond INLINE in your AGY_DONE file. Do NOT edit project files. Web research + tool inspection OK.

## Problem
RIMA uses `notebooklm-mcp-cli` (invoked as `uvx --from notebooklm-mcp-cli nlm notebook query <id> "<q>"`) to query the
canonical design notebook (id `30ddffa5-292f-4248-8e77-68074af901be`). It **keeps breaking** — auth/login expires
repeatedly. Prior "fix" (from project memory `reference_nlm_auth_recovery_manual_cookie`): FULL RESET = rename the
`.notebooklm-mcp-cli` config dir (so `doctor` shows Profiles:none) → user runs `! nlm login` for fresh OAuth (Chrome,
~49 cookies). That WORKED once but it **broke AGAIN** now ("eskisi gibi çalışmıyor" — same as before).

`nlm login --clear` does NOT fix it (it only clears chrome-profiles, leaves cookies.json + auth.json STALE → relogin loop).

## Research questions (answer concretely)
1. **WHY does it keep expiring?** Is it a Google OAuth refresh-token expiry, a cookie TTL, the CLI not persisting the
   refresh token, or NotebookLM session invalidation? What's the actual root cause of the recurring break?
2. **Is there a DURABLE fix** so it stops needing a full reset every few days? e.g.:
   - a config/env flag to persist the refresh token,
   - a different auth mode (service account / API key / app password) the CLI supports,
   - pinning a CLI version that doesn't have this bug,
   - a cron/scheduled `nlm login --refresh` keepalive,
   - or an ALTERNATIVE to notebooklm-mcp-cli entirely (official NotebookLM API? a different MCP server? a local cache of the canon so we don't depend on live NLM during a session?).
3. **Exact recovery steps** for RIGHT NOW (the durable one if found, else the cleanest reset). Include the precise
   commands and where the config/cookie/auth files live on Windows.
4. **Fallback architecture recommendation:** should RIMA reduce its hard dependency on live NLM (e.g. snapshot the
   canonical notebook to local markdown that agents read directly, and only re-sync NLM occasionally)? Pros/cons for a
   solo-dev autonomous workflow.

Check the notebooklm-mcp-cli docs / GitHub issues / PyPI for the known auth-expiry behavior. Be specific and actionable.
