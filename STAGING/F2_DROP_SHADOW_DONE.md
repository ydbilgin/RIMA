# F2 Drop Shadow — DONE

Date: 2026-05-27
Status: COMPLETE — 0 err / 0 warn

## Delivered

### New Scripts
- `Assets/Scripts/Environment/CliffDropShadowGenerator.cs`
  - Static singleton, DontSave cache
  - 32×16 px Texture2D: top=alpha 0.6, bottom=alpha 0 (linear gradient)
  - Pivot top-centre so shadow hangs below cliff cell world position
  - `InvalidateCache()` called on each Regenerate to avoid stale sprites across play mode transitions
  - Pattern: GroundBlobShadow.cs texture/sprite generation

- `Assets/Scripts/Environment/CliffDropShadowPlacer.cs`
  - `[ExecuteAlways]` — works in edit mode and play mode
  - Fields: `cliffAutoPlacer` (ref), `shadowTilemap` (ref)
  - `Regenerate()` [ContextMenu]: clears shadow tilemap, creates runtime Tile wrapping procedural sprite, mirrors every CliffTilemap cell into CliffDropShadowTilemap
  - `OnEnable` + `OnValidate` (editor deferred) auto-refresh
  - Pattern: CliffAutoPlacer.Regenerate mirror approach

### Scene Wire (PlayableArena_Test01.unity)
- `CliffDropShadowTilemap` GO created as child of `Floor` Grid
  - Components: Tilemap, TilemapRenderer
  - TilemapRenderer: sortingLayerName=`Decor_Cliff`, sortingOrder=`-20`
- `CliffDropShadowPlacer` component added to `CliffRing` (CliffAutoPlacer host GO)
  - `cliffAutoPlacer` → CliffRing (self)
  - `shadowTilemap` → CliffDropShadowTilemap

## Sorting Z-order (verified)
| Layer          | Order | Object                |
|----------------|-------|-----------------------|
| Decor_Cliff    | -1    | CliffTilemap (cliff base) |
| Decor_Cliff    | -20   | CliffDropShadowTilemap    |
| (floor default)| 0     | FloorTilemap              |

Shadow (-20) is behind cliff base (-1) — correct.

## Invariants respected
- CliffAutoPlacer.cs: NOT modified
- DecorCliffTilemap: NOT modified
- DirectionalCliffTile: NOT modified
- refresh_unity scope=all mode=force: DONE post-.cs creation

## Verify checklist
- [x] 0 compile errors
- [x] 0 compile warnings
- [ ] PlayMode: cliff cells show soft gradient shadow below (user verify)
- [ ] Manual: Right-click CliffRing → "Regenerate Shadow Tilemap" re-runs sync

## Next
Codex xhigh review pass (F2 PASS gate per task spec).
