# Done: UI↔JSON Fixes (2026-06-07) — commit `966a3d7a`

## Fixes applied

### 1. Mixed line endings (RoomTemplateJsonExporter.cs)
- All `sb.AppendLine(...)` replaced with `sb.Append(... + "\n")` — output is LF-only.
- `WriteIfChanged`: both existing file content and new JSON normalized to LF before compare (`.Replace("\r\n","\n").Replace("\r","\n")`).
- **Result:** double-export = 0 written / 26 skipped on both runs. No git noise.

### 2. Drag-paint undo spam (UnifiedMapDesigner.cs)
- Added `_dragUndoGroupIndex` field.
- On mouse-down: `Undo.IncrementCurrentGroup()` + `Undo.SetCurrentGroupName("Room Schematic Stroke")` + capture group index.
- On mouse-up: `Undo.CollapseUndoOperations(_dragUndoGroupIndex)`.
- **Result:** entire drag stroke = one Ctrl+Z undo step.

### 3. v2 props-key-missing wipe (RoomJsonImporter.cs)
- Changed `data.hasPropsArray = true` → `data.hasPropsArray = room.props != null`.
- Missing "props" key in v2 JSON → `room.props == null` (JsonUtility default) → `hasPropsArray=false` → existing `template.props` preserved.
- Added `TestOnly_V2JsonHasPropsKey(string singleRoomJson)` public bridge for test access.

### 4. New tests (RoomJsonImporterPropsTests.cs)
3 EditMode tests in `Assets/Editor/Rooms/`:
- `V2Json_WithPropsKey_HasPropsArrayTrue` — PASS
- `V2Json_WithoutPropsKey_HasPropsArrayFalse` — PASS
- `V1Json_NoPropsKey_HasPropsArrayFalse` — PASS

## Verification
- Compile: 0 errors
- All EditMode tests: **65/65 PASS** (was 62 before)
- Double-export: Run 1 written=0 skipped=26 | Run 2 written=0 skipped=26
