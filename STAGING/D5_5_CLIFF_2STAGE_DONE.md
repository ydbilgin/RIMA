# D5.5 Cliff 2-Stage Separation — DONE

**Date:** 2026-05-27  
**Compile status:** 0 errors, 0 warnings  
**Orphans deleted:** 0 (ManualPaintedCells was empty; scene scan confirmed 0 orphan cliff tiles)

## Changed Files

| File | Change |
|---|---|
| `Assets/Scripts/Environment/CliffAutoPlacer.cs` | +`ValidateManualPainted()` method (~30 LOC); called at top of `Regenerate()` |
| `Assets/Editor/RoomPainter/SceneAuthoring/DecorCliffPainter.cs` | NEW — free-form Shift+Click painter for DecorCliffTilemap (~80 LOC) |
| `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` | +`DrawDecorCliffSection()` with tile count, Clear button, Create helper (+60 LOC) |
| `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` | +`DecorCliffPainter.OnSceneGui` call in Cliff mode; +Shift-mode statusbar label; +`using UnityEngine.Tilemaps` |
| `Assets/Scenes/Test/PlayableArena_Test01.unity` | Added `DecorCliffTilemap` GameObject under `Floor` Grid |

## DecorCliffTilemap Scene Object

- **Parent:** Floor (Grid)
- **Components:** Tilemap + TilemapRenderer
- **sortingLayerName:** Decor_Cliff (value=12)
- **sortingOrder:** 50
- **Material:** Sprite-Lit-Default (copied from CliffTilemap)
- **Collider:** NONE (decor, hitbox=false per spec)

## Architecture Summary

### Algorithmic cliff (CliffTilemap)
- Controlled by `CliffAutoPlacer.Regenerate()`
- Only floor-edge cells (S/SE/SW neighbor void)
- `ValidateManualPainted()` now prunes any orphan whitelist entries before placement
- `ManualOverrideCells` (blacklist) + `ManualPaintedCells` (whitelist) still work

### Decor cliff (DecorCliffTilemap)
- Controlled by `DecorCliffPainter.OnSceneGui()`
- Activated in Cliff mode + Shift hold
- **Shift+Click** → paint free-form (no floor check)
- **Shift+Alt+Click** → erase
- **Regenerate (C)** does NOT touch this tilemap
- Count + Clear button in Inspector "Decor Cliff" sub-section

## Verify Results

- `PlayableArena_Test01` scene saved with DecorCliffTilemap
- 0 compiler errors
- Orphan scan: 283 cliff tiles all algorithmic (0 orphans)
- `ValidateManualPainted()` guard active for future Regenerate calls
- Inspector Cliff Mode: "Decor Cliff (Free-form)" section visible with tile count + Clear button
- Statusbar: cyan "Free-form Decor mode" label when Shift held in Cliff mode

## Migration Log

See `STAGING/D5_5_orphan_cleanup_log.md` — 0 cells deleted.

## D2-D5 Features Preserved

- CliffAutoPlacer core algorithm: unchanged
- ManualOverrideCells (Alt+click erase): unchanged
- ManualPaintedCells (normal cliff paint): unchanged + ValidateManualPainted guard added
- CliffHoverIndicator: unchanged
- Cliff mode UI (variant dropdown, regen, erase counts): extended only
- D4 ColliderShapeSwapper: untouched
- D3 mode tabs: untouched
