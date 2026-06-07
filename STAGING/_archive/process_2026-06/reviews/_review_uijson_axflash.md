# Static Code Review: UI↔JSON Room Editor (Commit `c97ddfa1`)

This review evaluates the implementation of the UI↔JSON room editing system under commit `c97ddfa1` against the specifications in [R4_DECISION_2026-06-07.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/STAGING/R4_DECISION_2026-06-07.md) §1 and [TASK_uijson_editor_2026-06-07.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/STAGING/TASK_uijson_editor_2026-06-07.md).

---

## Focus Areas Evaluation

### 1. Y-Flip Correctness (Both Directions)
* **Exporter Logic:** Located in [RoomTemplateJsonExporter.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs). The exporter maps 2D grid coordinates to JSON rows via `int gridY = (h - 1) - jsonRow` ([L72](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L72)). Spawn positions, exit slots, props, and enemy spawns are all correctly localized relative to `bounds.xMin`/`yMin` and Y-flipped:
  * Spawn: `spawnJsonY = (h - 1) - spawnLocalY` ([L89](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L89))
  * Slots: `jy = (h - 1) - ly` ([L108](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L108))
  * Props: `pjy = (h - 1) - ply` ([L125](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L125))
  * Enemies: `ejy = (h - 1) - ely` ([L147](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L147))
* **Importer Logic:** Located in [RoomJsonImporter.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/Rooms/RoomJsonImporter.cs). The inverse mapping uses `tileY = ToTemplateY(inputY, room.height)` where `ToTemplateY` is `height - 1 - inputY` ([L593](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/Rooms/RoomJsonImporter.cs#L593)). This maps JSON row `0` (top/north) back to grid Y `h-1` (top/north). Spawn coordinates, named exit slots, props, and enemies are flipped back via `gridY = (room.height - 1) - jsonY`.
* **Testing Sufficiency:** The EditMode tests in [RoomTemplateJsonRoundTripTests.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Tests/EditMode/Room/RoomTemplateJsonRoundTripTests.cs) check the raw JSON string structure directly (e.g., asserting string row `0` is void while checking specific values like `Assert.AreEqual(h - 1 - 2, sy)` for Y-flipped spawn). This ensures that any double-flip bugs (where exporter and importer carry the same inverse error) are caught.
* **Verdict:** **PASS**

### 2. Diff-Check & Write Mechanics
* **BOM Handling:** Strip-on-read and write-without-BOM are implemented cleanly via `StripBom` ([L182-187](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L182-L187)) and `Utf8NoBom` encoding parameter in [RoomTemplateJsonExporter.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs).
* **Line-Endings (Brimming Git Noise):** 
  > [!WARNING]
  > The exporter builds the JSON string using `sb.AppendLine()` (which writes platform-dependent endings, `\r\n` on Windows) mixed with `string.Join(",\n", ...)` ([L111](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L111), [L132](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L132), [L151](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L151)).
  >
  > On Windows, this creates a mixed-line-ending string in memory. If the checked-out file is normalized by Git or another IDE to either consistent LF or consistent CRLF, the raw string comparison `existing == json` ([L176](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L176)) will always evaluate to `false` even if the payload is identical. This results in the exporter repeatedly writing files and generating Git noise.
* **Verdict:** **PASS-WITH-NOTES** (Save is functional, but the diff-check is prone to false-positives on Windows).

### 3. Debounce Loop & Editor Subscriptions
* **Event Handlers:** In [UnifiedMapDesigner.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs), the `EditorApplication.update` delegate is registered in `OnEnable` ([L77](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L77)) and correctly unregistered in `OnDisable` ([L84](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L84)).
* **Resource Leak/Multiple-Subscribe:** The unregistration in `OnDisable` guarantees no double-subscription leaks when the editor window is closed/reopened or recompiled.
* **Safety Flush:** `FlushPendingExports()` is correctly called on `OnDisable` ([L85](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L85)) and before manual asset saves ([L323](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L323)), ensuring dirty templates write out before window close.
* **Verdict:** **PASS**

### 4. Undo Correctness
* **Mutation Recording:** `Undo.RecordObject(template, ...)` is invoked correctly in `ApplySchematicEdit` ([L511](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L511)) *before* any properties (walkable grid, spawn, exit slots) are modified.
* **Drag-Paint Grouping:** 
  > [!WARNING]
  > There is no Undo group collapsing (e.g. `Undo.CollapseUndoOperations`) during drag painting. A single mouse drag stroke over 50 cells will result in 50 separate undo items, requiring 50 sequential `Ctrl+Z` keypresses to undo a single brush stroke.
* **Verdict:** **PASS-WITH-NOTES** (Functional but has poor UX for undoing brush strokes).

### 5. Importer Props Preservation
* **v1 Behavior:** If `version < 2`, `data.hasPropsArray` is `false`. The existing `template.props` is preserved untouched ([L517](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/Rooms/RoomJsonImporter.cs#L517)), solving the critical wipe issue.
* **v2 Behavior:** If `version >= 2`, `data.hasPropsArray` is set to `true` and props are fully replaced.
* **Edge Case (Omitted Key in v2):**
  > [!CAUTION]
  > In v2 branch, `data.hasPropsArray` is set to `true` unconditionally ([L289](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/Rooms/RoomJsonImporter.cs#L289)) even if the `"props"` key is completely missing in the JSON. This results in the importer wiping out existing props (overwriting with an empty list) if a v2 JSON without a `"props"` key is imported.
* **Verdict:** **PASS-WITH-NOTES** (Dangerous edge case that could lead to accidental prop deletion).

### 6. Set-Slot Auto-Enforcement
* **Socket ID Enforcement:** `SetExitSlot` correctly retrieves the target `socketId` via `RoomTemplateSO.ExitSlotId(slotIndex)`. It searches the existing sockets for a match and updates the position/direction (North) in-place ([L576-590](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L576-L590)) instead of creating duplicates.
* **Overlapping Sockets:** If a user assigns two exit slots (e.g., N and NW) to the exact same cell, the editor allows it, but runs `RunSlotValidation` immediately and displays a red warning inline ([L410-416](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L410-L416)).
* **Verdict:** **PASS**

### 7. Schematic Mouse-to-Cell Inverse Mapping
* **Offset, Zoom & Scroll:** Since the right-hand panel has no zoom/scroll view, offset calculation is straightforward.
* **Border Calculations:** `cell` is a floored float, ensuring integer division is clean. `SchematicMouseToCell` ([L477](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L477)) reverses `TilePreviewRect` perfectly. Outer boundaries are clamped via `Mathf.Clamp` to prevent index-out-of-bounds exceptions.
* **Verdict:** **PASS**

---

## Summary of Findings

| Severity | File : Line | Description / Proposed Fix |
| :--- | :--- | :--- |
| **Minor** | [RoomTemplateJsonExporter.cs:111](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/Map/RoomTemplateJsonExporter.cs#L111) | **Mixed Line Endings:** Mixing `sb.AppendLine` (CRLF on Windows) with `string.Join(",\n", ...)` (LF) results in mixed line endings in memory, breaking diff-checks on Windows. <br>*Fix:* Standardize on `\n` line endings by replacing `sb.AppendLine(...)` with `sb.Append(line + "\n")`. |
| **Minor** | [UnifiedMapDesigner.cs:511](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs#L511) | **Undo Spam during Drag-Paint:** Lacks collapse operation. Every cell painted during drag-paint registers a separate undo. <br>*Fix:* Open an undo group on mouse-down, collapse it on mouse-up. |
| **Warning** | [RoomJsonImporter.cs:289](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/Rooms/RoomJsonImporter.cs#L289) | **Props Wipe on Omitted Key:** v2 imports unconditionally overwrite props even if `"props"` key is missing from JSON. <br>*Fix:* Only set `data.hasPropsArray = true` if `room.props != null`. |

---

## Verdict
**PASS-WITH-NOTES**
