# Painter Paint Regression Fix

## Files changed

- `Assets/Editor/RimaUnifiedPainterWindow.cs`

## Fixes

- `RimaUnifiedPainterWindow.cs:752-760`
  - Guarded `targetParent = targetTilemap.transform.parent`.
  - Grid and Tilemap parents are no longer stored as `targetParent` during auto-init.

- `RimaUnifiedPainterWindow.cs:3211-3237`
  - `PeekTargetParent()` now rejects a non-null `targetParent` when it is Grid/Tilemap-like.
  - Added `IsGridOrTilemapTransform()` helper so stale serialized Grid/Tilemap target parents fall through to the existing fallback route.

- `RimaUnifiedPainterWindow.cs:3525-3531`
  - `PaintWallWithConnections()` now resolves `baseParent` through `GetTargetParent()`, then routes wall placement through `GetOrCreateGroupParent(baseParent, selectedPrefab.name, PaletteCategory.Wall)`.
  - Auto-connected walls now use the same canonical group parent path as regular prefab paint.

## Verification

- Unity script refresh/compile requested through MCP.
- `read_console` after compile: 0 errors.
- Opened `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`.
- Opened `RIMA/Tools/Unified Painter`.
- Wall palette scan result: `pilot_a_wall_*` count = 3.
- Auto-init guard reflection check:
  - `Floor_Tilemap` parent was `Grid`.
  - auto-assigned `targetParent` stayed `null`.
  - stale `targetParent = Grid` was rejected by `PeekTargetParent()` and fell back to `Props_Root`.
- Paint-path test:
  - With `targetParent = Walls_Root`, `PaintWallWithConnections()` placed `pilot_a_wall_face_EW` under `IsoShowcaseRoom_S95_Root/Walls_Root/Walls/pilot_a_wall_face_EW`.
  - Test-created object and empty test group were cleaned up.
- Final `read_console`: 0 errors.

## Notes

- No scene files were saved.
- No CollisionRulesSO, sorting-layer scripts, or unrelated painter logic were edited.
