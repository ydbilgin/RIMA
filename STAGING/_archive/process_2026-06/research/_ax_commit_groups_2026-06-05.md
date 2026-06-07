ACTIVE RULES: (1) surgical — only the listed files (2) no code changes, git only (3) BLOCKED if unclear.

# Amaç (Purpose)
Group the uncommitted changes in `F:\Antigravity Projeler\2d roguelite\RIMA` into logical git commits.

# Hard rules
- Commit author = the repo's configured identity (ydbilgin). Do NOT add any "Co-Authored-By: Claude" or AI trailer.
- Commit messages in English, conventional-commit style.
- Do NOT push.
- Do NOT touch `Assets/_Recovery/` (leave untracked, do not add, do not delete).

# Commits (in this order)

## 1. chore(tooling): switch to global `cx dispatch`, retire local dispatcher
Files: `.claude/PROJECT_RULES.md` · deleted `cx_dispatch.py` (stage the deletion) · `CURRENT_STATUS.md` · `cx_profiles.local.json` · `STAGING/cx_task_globalize_dispatch_FINISH_2026-06-05.md`
Body (1-2 lines): dispatch is now the global `cx dispatch` subcommand (CodexAuthManager 6dbe8d18); local cx_dispatch.py removed, rules/status updated.

## 2. docs(staging): session artifacts 2026-06-04/05 (council outputs + cx task specs + Sodaman decision)
Files: ALL untracked `STAGING/*.md` from `git status` EXCEPT the FINISH file already committed in #1. (That is: `STAGING/SODAMAN_LEARNINGS_DECISION_2026-06-04.md`, all `STAGING/_ax_*.md`, all `STAGING/_council_*.md`, all `STAGING/cx_task_*_2026-06-04.md`.)

## 3. chore(characters): drop stale eski_anchors metadata.json files
Files: the 10 deleted `Characters/eski_anchors/*/metadata.json` (stage deletions).

## 4. TMP font asset — INSPECT FIRST, then decide
Run `git diff --stat "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF - Fallback.asset"`.
- If the diff is only dynamic-atlas/glyph-table data (incidental noise from a glyph test): `git restore` it, do NOT commit. Report that you restored it.
- If anything else changed: leave it uncommitted and report the diff stat. Do NOT guess.

# Output
Print: each commit hash + message + file count, and the decision taken for the TMP asset. Verify with `git status --short` at the end (expected leftovers: only `Assets/_Recovery/*` and possibly the TMP asset).
