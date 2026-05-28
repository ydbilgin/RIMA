# D5: DirectionalCliffTile Full Wire + Cliff Mode UI — DONE

Date: 2026-05-27
Agent: Sonnet (claude-sonnet-4-6)

## Compile Status
**0 errors / 0 warnings** — verified via `read_console` after `refresh_unity scope=all mode=force`.

## Changed Files

### 1. `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset`
- `spritesS[]` expanded from 1 → 5 sprites (YAML direct edit, no import-time race)
- Index 0: `cliff_S` (GUID `2a3d49363e3628c4292a7d2c6f575c9e`)
- Index 1: `cliff_S_new1` (GUID `0fb53d1598117804fb280f73aa7bffee`)
- Index 2: `cliff_S_new2` (GUID `c83d4e0fd9bbbc84980f65f902822fef`)
- Index 3: `cliff_S_new3` (GUID `7483b62a84244a9458965f196815a0aa`)
- Index 4: `cliff_S_new4` (GUID `4733d595eb91f4041a3b124836117587`)
- Verified: `execute_code` returns `spritesS.Length = 5` with all 5 sprite names

### 2. `Assets/Editor/RoomPainter/SceneAuthoring/CliffHoverIndicator.cs` (NEW ~80 LOC)
- `[InitializeOnLoad]` static class in `RIMA.RoomPainter.Editor` namespace
- Subscribes to `SceneView.duringSceneGui`
- Reads `CliffAutoPlacer.ManualOverrideCells` / `ManualPaintedCells` / `cliffTilemap.HasTile()`
- Draws diamond-shaped cell outline: red=erased, green=painted, grey=auto
- Active property wired from `RimaRoomPainterWindow` on mode change / enable / disable

### 3. `Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs` (EXTEND ~45 LOC)
- `MouseDown e.button == 0 && e.alt` → `ApplyCliffErase()` (before existing paint MouseDown)
- `ApplyCliffErase()`: finds CliffAutoPlacer, calls `SetTile(cell, null)`, `AddManualOverride()`, `RemoveManualPainted()`, with `Undo.RegisterCompleteObjectUndo`
- `CliffEraseCounter` static class added inside namespace (`RIMA.MapDesigner.VisualEditor`): `Count`, `Increment()`, `Reset()`

### 4. `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (EXTEND ~35 LOC)
- **C hotkey** (Cliff mode only): `HandleModeHotkeys()` → `TriggerCliffRegenerate()`
- `TriggerCliffRegenerate()`: `FindAnyObjectByType<CliffAutoPlacer>().Regenerate()`, stores `_lastRegenCount`
- **Statusbar**: when Cliff mode — shows "Erased: N" (from `CliffAutoPlacer.ManualOverrideCells.Count`) + "Cliff regenerated: N tiles" (from `_lastRegenCount` after C key)
- `CliffHoverIndicator.Active` synced on `SetMode()`, `ApplyModeDefaults()`, `OnEnable()`, `OnDisable()`

### 5. `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (EXTEND ~85 LOC)
- `DrawCliffModeBody()` replaced with full D5 content:
  - Loads `DirectionalCliffTile_Hades.asset`, reads `spritesS[]`
  - Popup dropdown with sprite name labels + AssetPreview thumbnail
  - "Manual painted: N" and "Manual erased: N" live labels (from CliffAutoPlacer)
  - "Clear Manual Painted" button with confirmation dialog → `ClearManualPainted()`
  - "Clear Manual Override" button with confirmation dialog → `ClearManualOverrides()`
  - "Regenerate (C)" button → `placer.Regenerate()` with Debug.Log count

## Verify Checklist

- [x] `refresh_unity scope=all mode=force` → 0 errors / 0 warnings
- [x] `DirectionalCliffTile_Hades.asset` Inspector → `spritesS[]` 5 sprites (`execute_code` verified)
- [ ] PlayMode: cliff cells show variant rotation (deterministic hash ÷ 5)
- [ ] RoomPainter → Cliff mode (hotkey 2)
- [ ] SceneView cliff cell hover → diamond outline (red/green/grey by state)
- [ ] Alt+Click cliff cell → tile deleted, Erased counter increments in statusbar
- [ ] C key → Regenerate fires, "Cliff regenerated: N tiles" appears in statusbar
- [ ] Inspector Cliff section: 5-entry dropdown with thumbnail, painted/erased counts, 3 buttons functional

## YASAK Compliance
- mounting_*.prefab → NOT touched (D5 scope excluded)
- N/E/W/NE/NW/SE/SW direction arrays → NOT changed (only spritesS[] expanded)
- CliffAutoPlacer algorithm → NOT changed (only consumed AddManualOverride/RemoveManualPainted/Regenerate public API)
- D3 mode tabs → NOT broken
- D4 Prefab Mode collider authoring → NOT touched
- Input.GetKey* → NOT used (Event.current.keyCode pattern used throughout, consistent with existing code)

## Notes
- `CliffHoverIndicator.Active` is set on every mode change (SetMode + ApplyModeDefaults) and cleared on window close, preventing ghost callbacks.
- `CliffEraseCounter` is in `RIMA.MapDesigner.VisualEditor` assembly; the RoomPainter statusbar reads `ManualOverrideCells.Count` from CliffAutoPlacer directly (avoids cross-assembly dependency between `RIMA.RoomPainter.Editor` and `RIMA.MapDesigner.Editor`).
- Asset YAML was edited directly (GUIDs obtained via `execute_code` TryGetGUIDAndLocalFileIdentifier before edit) — no AssetDatabase API race condition.
