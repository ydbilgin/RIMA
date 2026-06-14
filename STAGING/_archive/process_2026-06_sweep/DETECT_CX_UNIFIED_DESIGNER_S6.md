ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# TASK: DETECTION ONLY — Unified Designer technical inventory + cliff-generate root-cause

**THIS IS A FINDINGS-ONLY TASK. DO NOT WRITE OR EDIT ANY CODE. DO NOT TOUCH SCENES.**
**Output = one report appended to CODEX_DONE.md. No implementation. If you start editing code you have failed the task.**

## Context
RIMA has TOO MANY overlapping level/room editor windows. The user wants them
consolidated into ONE dual-surface (in-game F2 + Unity Editor) tool with room
select+edit, organized AssetPack, clean separated layers, and a real in-game-tool
UX. There is ALREADY a locked direction + working code (Townscaper-2D RoomTool,
M1+M2 done). Read `STAGING/UNIFIED_DESIGNER_TASK_S6.md` for the full spec — it is
the map of what to inventory. Also read `STAGING/ROOMTOOL_IMPROVEMENT_PLAN_S6.md`
and `STAGING/ROOMTOOL_VERIFY_REPORT_S6.md`.

## What to detect & report (be concrete: file:line, class names, what calls what)

### A. DESIGNER INVENTORY (the scatter)
For EACH editor window / authoring entry point under `Assets/Editor/` (MapDesigner,
RoomPainter, LiveTool, TileImport; ignore `_archive_S73~` except to note it's dead):
- Menu path + window class + file.
- What it does (1-2 lines), what data it reads/writes (RoomData? scene tilemaps? SO?).
- OVERLAP: which other window does the same/similar job.
- Status: LIVE / partially-wired / dead / duplicate.
- Verdict: KEEP-as-core / FOLD-into-core / DELETE / archive.
Flag the obvious redundancies (two TileImportWizard copies, multiple tile-palette
openers, AssetPackBrowser vs palette, BrushWindow vs RoomPainter vs VisualMapEditor).

### B. SHARED-DATA REALITY CHECK
- Confirm which windows actually use the M1 shared RoomData pipeline
  (`WangResolver`/`RoomDataMutator`/`RoomDataJson`/`RoomData.cs`) vs which write
  scene tilemaps directly (the ones that DON'T share data = the consolidation risk).
- Does `InPlayMapPaintOverlay` (in-game) and `RimaRoomPainterWindow` (editor)
  genuinely share the same RoomData? Cite the load/save calls.

### C. CLIFF-GENERATE ROOT CAUSE (priority)
- Find the "Generate Cliff" button/menu the user means. Candidates:
  `VisualEditorScenePainter.cs`, `DecorCliffPainter.cs`, RoomPainter Cliff mode,
  `CliffAutoPlacer`/`CliffAutoPlacerEditor`. Identify the EXACT entry point.
- Trace why it no-ops: is `CliffAutoPlacer.IsReady` false (missing floor/cliff
  tilemap or cliffTile ref)? Wrong tilemap target? Old/dead code path? Layer/
  sort issue so cliffs generate but are invisible?
- State the MINIMAL wiring/fix that would make cliff-generation logical again
  (floor → auto cliff ring under floor, layers separate). DESCRIBE the fix; DO NOT apply it.

### D. CONSOLIDATION ARCHITECTURE RECOMMENDATION
- Propose the target architecture: one core authoring controller (shared by both
  surfaces) + thin Editor-window view + thin in-game-overlay view, both driving the
  same RoomData + AssetPack. Name the existing classes that become the core vs the
  views vs what gets deleted.
- Identify the riskiest coupling to untangle (e.g. Editor-only AssetDatabase calls in
  a path that needs to run at runtime — `RoomDataComposer` is Editor-only per verify report).
- List the concrete build steps to get from today → consolidated tool, mapped onto the
  existing M3/M4 milestones. Order by dependency.

### E. ASSET-PACK / ROOM ORGANIZATION
- Where do room .assets live today? Where do tile/prop assets live? Is there a single
  AssetPack SO or hardcoded `Resources/...` lookups (cite them)?
- Recommend the folder + SO structure so "asset-pack, rooms, everything is organized".

## Output format
Append to CODEX_DONE.md a structured report under headers A/B/C/D/E with concrete
file:line evidence. End with a one-paragraph "BOTTOM LINE: keep X, fold Y, delete Z,
cliff breaks because W". NO code diffs. NO file edits.
