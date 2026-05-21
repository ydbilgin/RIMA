# STAGING Index

`STAGING/` is a temporary production and review workspace. Root-level files should stay limited to active loose drafts, source assets, and one-off research outputs. Recurring dispatch, report, plan, and inventory files live in the folders below.

## Folders

- `concepts/` - visual outputs and generated concept images. Keep this folder as-is.
- `_codex_tasks/` - Codex task dispatch files.
- `_codex_tasks/overnight/` - overnight Codex task dispatch files.
- `_codex_done/` - Codex completion reports.
- `_codex_done/overnight/` - overnight Codex completion reports.
- `_antigravity/prompts/` - Antigravity prompt files.
- `_antigravity/done/` - Antigravity completion reports.
- `_research/nlm/` - NotebookLM canonical batch outputs.
- `_research/pixellab/` - PixelLab research and inventory outputs.
- `_plans/progression/` - progression plan drafts and final versions.
- `_plans/door/` - door design specs and related plans.
- `_inventories/` - morning inventories, asset audits, showcase room reports, and production batch reports.
- `_archive/` - existing archive area. Keep existing organization intact.

## Root Rules

- Do not delete staging records during cleanup; move them with `git mv`.
- Do not reorganize `concepts/` during document cleanup passes.
- Do not reorganize `_archive/` unless the orchestrator explicitly requests it.
- Add new recurring report categories as subfolders instead of returning to a crowded root.
