# TASK: Rooms tab edit toolbar + SO→JSON auto-export + import props-fix (R4 spec)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç
Implement the council-approved UI↔JSON room editing per `STAGING/R4_DECISION_2026-06-07.md` §1 (READ IT FULLY — it is the spec incl. schema v2, Y-flip rule, time bombs). Unity is OPEN.

## Work items
1. **JSON exporter** (new editor class): deterministic RoomTemplateSO → schema-v2 JSON writer. Per-room file `STAGING/rooms_json/<roomId>.json`. Diff-check before write (skip identical payload). Y-flip: `jsonY = height-1-gridY`. walkable rows: '#'=walkable '.'=void, row 0 = TOP (north). Named exitSlots from `door_NW_01/door_N_01/door_NE_01` sockets; spawn; props (propDefinitionGuid + name comment if cheap, x/y/variant/flipX); enemySpawns; version:2; unknown-key-tolerant shape.
2. **Auto-export hook:** every Rooms-tab edit → Undo.RecordObject + SetDirty + ~1s debounce → export ACTIVE template's JSON. Window close + "Save Assets" → flush all dirty. Also a manual "Export All JSON" menu/button for the 25 templates (initial population).
3. **Rooms tab edit toolbar** (UnifiedMapDesigner.cs, schematic preview area ~333-403): mode buttons `[Paint Walkable] [Paint Void] [Set Entry] [Set NW] [Set N] [Set NE]` — click (and drag for paint modes) on the existing 2D schematic → reverse-map rect→cell (TilePreviewRect inverse) → mutate walkableGrid / playerSpawn / slot socket with Undo + dirty + debounce-export. Set-slot modes enforce/auto-set direction=North + isExit + socketId convention; run the existing slot validator after each slot move and show MUST violations inline (red text), don't block.
4. **Prop palette (Tier-A.2, include if it stays lean):** small foldout listing PropRegistry entries; selected prop + click cell = add PropPlacementData (Undo'lu); RMB on a propped cell = remove. Skip if it balloons — note SKIP in DONE.
5. **Importer props-fix (CRITICAL, Flash finding):** RoomJsonImporter currently wipes `template.props` on every import (~line 400, `new List<PropPlacementData>()`). Change to PRESERVE existing props when incoming JSON has no props array; when it does, replace only then. Also teach importer to read schema-v2 files (named exitSlots/spawn/props) while keeping v1 (grid markers P/e/B/C + doors[]) working.
6. **Round-trip test:** EditMode test — export a template → reimport the JSON → assert walkable/spawn/slots/props identical (Y-flip correctness). Plus existing tests stay green.
7. **Verify:** compile clean · Export All JSON produces 25 files · edit a cell in Rooms tab → JSON file updates within ~1s · round-trip test green · smoke 26/26 unchanged.
8. **Commit** (ydbilgin, English, no Co-Authored-By): `feat(designer): rooms tab edit toolbar + JSON round-trip export`. Don't commit unrelated files. CODEX_DONE summary with file:line map.

## Constraints (hard)
- Do NOT touch IsoRoomBuilder/RoomRunDirector runtime (slot consumption shipped separately).
- Do NOT introduce ScriptedImporter/file-watcher (council RED).
- `STAGING/rooms_json/` only for JSON output (outside Unity watcher).
- Chamber_CharSelect: exportable but never auto-modified.
- If gate-slot implementation (parallel task) has NOT landed in your checkout (no door_NW_01 constants/resolver), BLOCKED — say so instead of inventing.
