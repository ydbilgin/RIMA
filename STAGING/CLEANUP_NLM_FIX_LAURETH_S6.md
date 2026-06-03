# TASK — Token diet (CURRENT_STATUS slim) + NLM login fix + PR review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
  (NLM may be DOWN because of the login bug you are fixing in Part 1. If a query errors, do NOT block — proceed conservatively: ARCHIVE, never delete, and note in your report that NLM was unreachable.)
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / project MEMORY files.

Profile: laurethayday. Effort: high. Language: English (report + commit messages).

## Amaç (Goal)
The user is burning far too many tokens every session even when nothing is happening, because the auto-loaded context files are bloated. Root cause: `CURRENT_STATUS.md` is 125 KB / ~595 lines (read at every session start) with ~25 stacked historical blocks — only the TOP block is current. Secondary: the user's `nlm login` is broken (Chrome shows a "preferences cannot be read" warning) and two upstream PRs need reconciling. Do BOTH parts. **Do Part 1 first** (it may restore NLM, which Part 2 uses as a safety net).

**CRITICAL SAFETY (this user has lost work before — be conservative):**
- ARCHIVE, NEVER DELETE. Every "removed" block/file is MOVED into an `_archive/` folder, byte-for-byte. Nothing is lost — canonical detail also lives in NLM.
- Surgical: touch ONLY the files named below. Do NOT modify `.claude/PROJECT_RULES.md`, `RULES.md`, `AGENTS.md`, `CLAUDE.md` (rules files — leave intact). Do NOT touch `C:\Users\ydbil\.claude\...` (Claude's own memory — orchestrator handles that).
- Do NOT touch `CODEX_DONE.md` / `CODEX_DONE_laurethayday.md` as cleanup targets (orchestrator already archived the old one; just write your final report there as the dispatch protocol requires).
- After each file edit: do NOT run Unity. This is pure docs + tooling. Verify by line-count / diff, not by build.

---

## PART 1 — NLM login fix + PR reconcile (do this FIRST)

Context (already verified by orchestrator, don't re-derive):
- `nlm` is installed as a **uv tool** at `C:\Users\ydbil\AppData\Roaming\uv\tools\notebooklm-mcp-cli` (version 0.6.13 = latest released).
- `PR #211` ("Fix nlm login crash on expired auth") is **OPEN / unmerged**. We previously applied its diff as a LOCAL patch to the venv. PR #211 makes `nlm login` not crash when auth is expired.
- `PR #212` ("fail loudly on stale auth instead of silent false-success") is **OPEN / unmerged**. It changes `refresh_auth()` / `studio_create()` / `studio_status()` in the MCP layer (NOT the browser login flow). Refs #170, #203, #210.
- The user thinks PR #212 was the one we applied — it was actually #211. Reconcile and clarify.

### 1A. PRIMARY — fix the Chrome "preferences cannot be read" warning on `nlm login`
Symptom (verbatim from user, Turkish): when they run `nlm login`, the Chrome window that opens shows:
  "Tercihleriniz okunamıyor. Bazı özellikler kullanılamayabilir. Tercihlerde yapılan değişiklikler kaydedilmeyecektir."
  (EN: "Your preferences cannot be read. Some features may be unavailable, and changes to preferences will not be saved.")
This is a Chromium message that appears when the browser is launched against a `user-data-dir` / profile that is **locked (another Chrome holding it), corrupt, or read-only.**

Do:
1. Find how `nlm login` launches the browser. Grep the installed package (the uv tool venv above, and `C:\Users\ydbil\.notebooklm-mcp-cli` for stored profile/cookies/auth dirs) for the login/auth/oauth/browser launch code (Playwright? `webbrowser`? a spawned Chrome with `--user-data-dir`?). Identify the exact profile / user-data-dir it uses.
2. Diagnose why that dir triggers the warning (stale `SingletonLock`/`lockfile`, leftover `Default/Preferences`, perms, or a dir shared with the user's real Chrome).
3. Apply the MINIMAL, REVERSIBLE fix. Prefer the least invasive that works, e.g.: clear/recreate the dedicated login profile dir, remove stale lock files, or make the launcher use a fresh disposable temp `--user-data-dir` per login. Do not require the user to close their real Chrome if avoidable.
4. Preserve the existing PR #211 local patch — do not regress it. If the venv was upgraded and the patch is gone, re-apply it.
5. Verify auth still works after the fix using a non-interactive check (e.g. `uvx --from notebooklm-mcp-cli nlm` doctor / `--check` if such a flag exists, or inspect that the OAuth flow can complete). The interactive browser step itself is the user's to run — give them a one-line `nlm login` retry instruction in your report.

### 1B. SECONDARY — PR #211 / #212 reconcile (lower priority, time-box it)
1. Check current status of `gh pr view 211` and `gh pr view 212` (repo `jacob-bd/notebooklm-mcp-cli`) — have they merged since? Is there a release newer than 0.6.13?
2. Document where the PR #211 local patch lives in the venv (file + lines) so future `uv tool upgrade` re-applies are scriptable.
3. Decide on PR #212: RIMA uses `nlm notebook query` (NOT `studio_create`), so #212's value here is MODERATE (auth-honesty for studio tools we don't use). Recommend: apply #212's diff locally ONLY if it's low-risk and touches the same `check_auth()` seam without breaking query; otherwise SKIP and state the one-line reason. If you apply it, make it reversible and note the patched files.
4. Optional, if cheap: a tiny `nlm_patch_apply.ps1` / note that re-applies the chosen local patches after a `uv tool upgrade` (durable against upgrades). Keep it minimal.

---

## PART 2 — Token diet: slim CURRENT_STATUS.md (THE big win)

### 2A. CURRENT_STATUS.md  (125 KB → target < 10 KB)  — DETERMINISTIC, do exactly this
The file has ~25 sections that each start with `## `. The FIRST one (line ~3:
`## 🆕🔧🎯 MAP DESIGNER REGRESYON KURTARMA + İSO FLOOR ÇÖZÜLDÜ ...`) is the ONLY current state. Everything from the SECOND `## ` header (line ~32: `## 🆕🆕... DEMO MAP PIPELINE ...`) to the END of the file is historical session-log.

Steps:
1. Create `STAGING/_archive/CURRENT_STATUS_archive_2026-06-01.md`. Header it:
   `# CURRENT_STATUS — archived history blocks (moved 2026-06-01 for token diet). Canonical detail = NLM notebook 30ddffa5-292f-4248-8e77-68074af901be.`
   Then append, BYTE-FOR-BYTE, every section from the 2nd `## ` header through EOF.
2. Rewrite `CURRENT_STATUS.md` to contain ONLY:
   - the file's `# CURRENT_STATUS` title line (if present),
   - the FIRST block verbatim (the Map Designer regression / iso floor block, through its routing/Memory line, i.e. everything BEFORE the 2nd `## ` header),
   - then a new compact trailer section:
     ```
     ## 📚 Arşiv (token diet 2026-06-01)
     Eski session-log blokları arşivlendi: `STAGING/_archive/CURRENT_STATUS_archive_2026-06-01.md`.
     Kanonik tasarım detayı = NLM notebook 30ddffa5-292f-4248-8e77-68074af901be.
     Arşivlenen bloklar (başlık + tarih):
     - <one bullet per archived block: its `## ` title>
     ```
3. VERIFY (report the numbers): `(lines kept) + (lines archived) == original 595` (±a few for the new trailer). New `CURRENT_STATUS.md` must be < 10 KB and still open cleanly with the top block 100% intact. Zero content from the top block may change.

### 2B. Project `MEMORY/` light tidy  (OPTIONAL, conservative, NOT blocking)
Only if Part 1 + 2A are solid and you have budget:
- Read `MEMORY/INDEX.md`. Identify topic files that belong to **fully-closed, superseded sprints** — filename or INDEX text clearly says `S101`..`S114` / "KAPANIŞ" / "archive" / a sprint older than the current S6 work.
- MOVE those files into `MEMORY/_archive/` and move their INDEX lines into an "## Archived" pointer section at the bottom of `MEMORY/INDEX.md`.
- KEEP every S6 file and every HARD-RULE / evergreen feedback file. When unsure → KEEP (query NLM if it helps decide). Conservative.
- If this is risky or ambiguous, SKIP it and say so. 2A is the real win.

---

## Deliverable / report (write to CODEX_DONE_laurethayday.md, last step)
End your report with a `STATUS:` line (`STATUS: COMPLETED` or `STATUS: BLOCKED — reason`). Include:
- Part 1A: root cause of the Chrome warning + exact fix applied + files touched + how the user should retry `nlm login`.
- Part 1B: #211/#212 status, where the local patch lives, your #212 decision + reason.
- Part 2A: before/after byte+line counts for CURRENT_STATUS.md, archive path, conservation check (kept+archived==original).
- Part 2B: what (if anything) you archived, or why you skipped.
- Do NOT commit. Leave changes in the working tree for orchestrator (Opus) review (writer ≠ reviewer).
