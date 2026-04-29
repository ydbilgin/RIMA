STATUS: DONE

COMPLETED:
- Audited the 4 original memory files.
- Split multi-topic `feedback_workflow.md` into focused memory files.
- Shortened memory files so each topic file stays under 25 lines.
- Renamed the old underscore-prefixed memory folder to `MEMORY`.
- Renamed the old underscore-prefixed staging folder to `STAGING`.
- Renamed the old underscore-prefixed archive folder to `ARCHIVE`.
- Updated allowed markdown references to `MEMORY`, `STAGING`, and `ARCHIVE`.
- Verified remaining old underscore references are outside task scope or explicitly forbidden (`CODEX_TASKS.md`, README/Tools docs).

SPLIT FILES:
- `MEMORY/feedback_workflow.md` was split and removed.
- `MEMORY/feedback_agent_delegation.md`
- `MEMORY/feedback_encoding.md`
- `MEMORY/feedback_git_attribution.md`
- `MEMORY/feedback_temp_files.md`

RENAMED FOLDERS:
- old underscore-prefixed memory folder -> `MEMORY/`
- old underscore-prefixed staging folder -> `STAGING/`
- old underscore-prefixed archive folder -> `ARCHIVE/`

REF FILES UPDATED:
- `AGENTS.md`
- `ANTIGRAVITY.md`
- `CLAUDE.md`
- `CODEX.md`
- `CURRENT_STATUS.md`
- `OTHER_AGENTS.md`
- `TASARIM/MASTER_KARAR_BELGESI.md`
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
- `STAGING/PLAYTEST_REPORT.md`
- `STAGING/PROMPTS_S43/PIXELLAB_CFR_V3_PROMPTS.md`
- `ARCHIVE/**/*.md` files that contained old underscore references

ERRORS:
- NONE

NOTES:
- `CODEX_TASKS.md` was not cleared because the task explicitly listed it under FORBIDDEN: "Do not change CODEX_TASKS.md".
- Git commit was limited to task-scoped paths to avoid staging unrelated dirty work already present in the worktree.

NEXT_SIGNAL: "Memory and folder cleanup done -- Claude review"
