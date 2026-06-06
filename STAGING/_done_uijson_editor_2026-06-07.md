# DONE: rooms tab edit toolbar + JSON round-trip export
**Completed:** 2026-06-07
**Commit:** feat(designer): rooms tab edit toolbar + JSON round-trip export

## File:line map

### New files
- `Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs` — deterministic SO→v2 JSON writer
  - `Export(template)` line 35: single-template export, diff-check before write
  - `ExportAll()` line 46 [MenuItem "RIMA/Rooms/Export All JSON (v2)"]: batch export all templates
  - `BuildJson(template, roomId)` line 63: full v2 schema builder (walkable, spawn, exitSlots, props, enemySpawns)
  - `WriteIfChanged()` line 148: UTF-8 no-BOM, diff-check via byte compare with BOM stripping
  - Y-flip: `jsonY = (h-1) - gridY` at line 78 (walkable), 97 (spawn), 110 (slots), 119 (props)
- `Assets/Tests/EditMode/Room/RoomTemplateJsonRoundTripTests.cs` — 5 EditMode tests
  - `ExportBuildJson_WalkableRows_YFlipCorrect` — top rows are void, row 2+ walkable
  - `ExportBuildJson_SpawnPosition_YFlipCorrect` — spawn (4,2) → jsonY = h-1-2
  - `ExportBuildJson_ExitSlots_YFlipCorrect` — N slot at gridY=h-3 → jsonY=2
  - `ExportBuildJson_Props_RoundTrip` — 2 props, guid+flipX+y preserved
  - `ExportBuildJson_WalkableGrid_FullRoundTrip` — every cell verified

### Modified files
- `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`
  - line 14: added `using RIMA.MapDesigner.Room.Validation`
  - lines 39-60: added `SchematicEditMode` enum + debounce fields
  - lines 77-114: `OnEnable`/`OnDisable` hook EditorApplication.update; `OnEditorUpdate` debounce; `ScheduleExport`/`FlushPendingExports`
  - line 126: `OnCoreChanged` schedules export of `_selectedTemplate`
  - lines 264-266: `DrawRooms` calls `DrawSchematicEditToolbar` + `DrawTemplatePreviewWithEditing`
  - lines 330-341: "Export JSON" + "Export All JSON" buttons in actions panel
  - lines 322-323: `FlushPendingExports` before "Save Assets"
  - lines 394-640: full schematic edit toolbar + mouse handling + ApplySchematicEdit + SetWalkable/SetPlayerSpawn/SetExitSlot + RunSlotValidation
  - lines 643-646: `DrawTemplatePreview` stores area rect + mode hint in title
- `Assets/Editor/Rooms/RoomJsonImporter.cs`
  - line 104-155: `TryImportRoom` handles v2 fields (id alias, size.w/h, walkable→grid)
  - lines 158-311: `BuildRoomData` v2 branch: pure '#'/'.' parse, v2 spawn invert, v2 exitSlots, v2 enemySpawns, v2 props (hasPropsArray=true)
  - lines 313-337: `BuildExitSlotsV2` — NW/N/NE named slots with Y-invert
  - lines 488-511: `FillTemplate` CRITICAL FIX — preserve existing props when `hasPropsArray=false` (v1)
  - lines 519-600+: v2 JSON data classes (RoomSizeJson, RoomSpawnJson, ExitSlotsJson, ExitSlotPosJson, RoomPropJson, RoomEnemyJson)
- `Assets/Tests/EditMode/RIMA_EditMode_Tests.asmdef`
  - added `"RIMA.Editor"` reference so round-trip tests can use `RoomTemplateJsonExporter`

## Work item status
- WI-1 JSON exporter: DONE
- WI-2 Auto-export hook + debounce + flush-on-close + "Export All JSON" menu: DONE
- WI-3 Rooms tab edit toolbar (Paint Walkable/Void, Set Entry/NW/N/NE, drag, undo, dirty, debounce-export, inline validator): DONE
- WI-4 Prop palette: SKIPPED (scope would balloon; items 1-3+5-6 are core per task spec)
- WI-5 Importer props-fix (CRITICAL): DONE — v1 preserves existing props; v2 replaces
- WI-6 Round-trip test (5 tests): DONE
- WI-7 Verification:
  - Compile clean: YES (0 errors)
  - Export All JSON: 26 files in STAGING/rooms_json/
  - Smoke test: 26/26 PASS (exceptions=0)
  - Round-trip tests: 5/5 PASS
  - Previously-green socket tests: PASS (RoomTemplatesExceptCharSelect_HaveAuthoredNorthExitSlots PASS)
  - Full EditMode suite: 514 total, 19 pre-existing failures (unchanged), 0 new failures
- WI-8 Commit: see below

## Key findings / time bombs addressed
- Y-flip inverse correctly implemented: `jsonY = (h-1) - gridY` in exporter, `gridY = (h-1) - jsonY` in importer
- Props wipe FIXED: v1 import preserves `template.props` when JSON has no props array
- Diff-check before write: BOM-stripped byte comparison ensures no spurious git noise
- Undo.RecordObject called on every schematic edit
- Mouse→cell mapping correctly inverts `TilePreviewRect` formula
- Slot validator runs after every Set-slot move, shows MUST violations inline (red text)

## Skip note
WI-4 (prop palette) SKIPPED as authorized by task spec ("Skip if it balloons — note SKIP in DONE").
