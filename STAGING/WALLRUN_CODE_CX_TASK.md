# CX CODE TASK — WallRunBuilder + RuinedKeepComposer + drag-place upgrade (S6)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the 3 files listed (4) BLOCKED if unclear.
NLM ACCESS: if you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read only: the files named below.

## CONTEXT
RIMA = Unity 2D top-down 3/4 action-roguelite (URP, PPU64, Custom-Axis (0,1,0) Y-sort). We are adding a connected-wall-run room builder + a Conor-Dart hold-drag live editor. The FULL design is already locked in two spec files — read them, they contain the exact code shapes:
- `STAGING/DRAG_PLACE_IMPL_PLAN_S6.md`  (THE spec — §0 shared core, §1 tile drag, §2 Prop mode, §3 composer, §4 risks). Implement exactly this.
- `STAGING/RUINED_KEEP_BUILD_PLAN_S6.md` (ordered build plan; you own STEPS 4, 5-shell, 10, 11).

ALSO READ before writing:
- `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`  (the file you EDIT — current single-click tile paint)
- `Assets/Scripts/Live/RuntimeAssetRegistry.cs`  (registry API: `Entries`, `Get(guid)`, `GetSprite(guid)`, `GetByTag(tag)`; `RegistryEntry` fields `sprite/tile/prefab/tag/layer/kind/displayName`) — for the Prop palette source.

## DELIVERABLES (exactly 3 files)

### FILE 1 (NEW) `Assets/Scripts/DevTools/WallRunBuilder.cs`
Implement per `DRAG_PLACE_IMPL_PLAN_S6.md` §0 verbatim:
- `public static class WallRunBuilder` with `BuildRun(Grid grid, Vector3Int fromCell, Vector3Int toCell, WallPiece piece, Transform parent, HashSet<Vector3Int> occupied) -> List<GameObject>`.
- `[System.Serializable] public struct WallPiece { GameObject prefab; Sprite sprite; Sprite cornerSprite; Vector2Int footprint; string displayName; }`.
- Internals: integer Bresenham `GridLine(from,to)` over XY; step by `footprint.x` (default 1) along the dominant axis; for each cell NOT in `occupied`: instantiate (prefab via `Object.Instantiate` if non-null, else `new GameObject` + `SpriteRenderer` with `piece.sprite`); world pos = `grid.GetCellCenterWorld(cell)`; **shift down by half a cell so the BOTTOM-CENTER pivot of the sprite sits on the cell ground line** (sprites are imported bottom-center pivot; for a bare-GO fallback, set `sr.sprite` and position pivot accordingly — if the imported sprite already has bottom-center pivot, `GetCellCenterWorld` minus 0.5*cellSize.y on Y is the foot); set `sr.sortingLayerName="Entities"`, `sr.sortingOrder=0`, `sr.spriteSortPoint=SpriteSortPoint.Pivot`; add cell to `occupied`; append GO to return list. Corner swap: when incoming≠outgoing step direction and `piece.cornerSprite!=null`, use the corner sprite for that GO. **Cap total instantiations per call at 64** (risk #3 — guard the loop). Namespace `RIMA.DevTools`. Wrap in `#if UNITY_EDITOR || DEVELOPMENT_BUILD` (matches the overlay; must compile without UnityEditor in the DEVELOPMENT_BUILD half — use only runtime APIs in this file).

### FILE 2 (NEW) `Assets/Scripts/DevTools/RuinedKeepComposer.cs`
Implement per `DRAG_PLACE_IMPL_PLAN_S6.md` §3:
- `public enum SegmentKind { SolidWall, VoidEdge, Entrance, BrokenGap, Anchor }` (NOTE: add `Anchor` to §3's enum — for single-place hero pieces: corner_buttress / pillar / arch).
- `[System.Serializable] public struct WallSegment { SegmentKind kind; Vector3Int fromCell; Vector3Int toCell; WallPiece piece; float height; }`.
- `public sealed class RuinedKeepComposer : MonoBehaviour` with `[SerializeField] Grid _grid;` `[SerializeField] List<WallSegment> _segments;` `[SerializeField] Transform _parent;`.
- `[ContextMenu("Compose Ruined Keep")] public void Compose()`: clear previous children of `_parent` (if `_parent` null, find-or-create a `"RuinedKeep_Walls"` GO); `var occupied = new HashSet<Vector3Int>();` `foreach (WallSegment s in _segments)`: if `kind==SolidWall` → `WallRunBuilder.BuildRun(_grid, s.fromCell, s.toCell, s.piece, parent, occupied)`; if `kind==Anchor || kind==Entrance` → single `Object.Instantiate`/bare-GO of `s.piece` at `_grid.GetCellCenterWorld(s.fromCell)` (foot-shifted, Entities layer, Pivot sort — reuse a small shared `PlaceOne(...)` helper, or expose `WallRunBuilder.PlaceOne(grid,cell,piece,parent)` and call it from both); if `VoidEdge||BrokenGap` → skip (gaps stay open).
- Edit-mode safety (risk #2): guard create/destroy with `#if UNITY_EDITOR` using `Undo.RegisterCreatedObjectUndo` / `DestroyImmediate` when `!Application.isPlaying`; else `Object.Instantiate`/`Destroy`. Keep `WallRunBuilder` itself play-safe (`Instantiate` only); the composer wraps edit-mode.
- Namespace `RIMA.DevTools`. The `_segments` list stays EMPTY (the orchestrator authors it later); `Compose()` must no-op gracefully (log a warning) if `_segments` empty or `_grid` null.

### FILE 3 (EDIT) `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs`
Upgrade per `DRAG_PLACE_IMPL_PLAN_S6.md` §1 (tile drag) + §2 (Prop mode):
- Add the drag-lifecycle state fields (§1) + rewrite the `Update()` input section (lines ~90-116) into a press/drag/release lifecycle with grid-crossing throttle, Bresenham interpolation, Shift axis-lock, RMB cancel/erase, Ctrl+Z undo. Replace `PaintAtMouse` (lines ~120-140) with `CellUnderMouse()` / `PaintCell(cell,set)` / `GridLine(a,b)` + the in-memory stroke-undo stack (`TileEdit` struct, `_currentStroke`, `_undo`).
- Add `Prop` to `enum PaintLayer` (line 46) + a `"Prop"` layer toggle in `OnGUI` (lines ~240-242). Prop palette (`_propPalette` of `WallPiece`) sourced in `RebuildPalette` from `registry.GetByTag("wall")` + `GetByTag("prop")` (dedupe), footprint via `FootprintFromSpriteSize(sprite)` = `Mathf.CeilToInt(sprite.rect.width/64f) x ...height`. In Prop mode the per-grid-crossing action calls `WallRunBuilder.BuildRun(_grid, _lastCell, cell, _propPalette[_propSelected], PropParent(), _propOccupied)`; release pushes the stroke to `_propUndo`; RMB-while-dragging cancels (Destroy stroke GOs); Ctrl+Z pops `_propUndo` and Destroys. `PropParent()` = find-or-create `"[DragPlace_Props]"`.
- Add a **"Compose Ruined Keep"** GUILayout.Button in `OnGUI` that finds-or-adds a `RuinedKeepComposer` and calls `Compose()`.
- Add the GUI hint line `"Hold-drag · Shift straight · RMB cancel/erase · Ctrl+Z undo"`.
- KEEP intact: the F2 toggle, `_paletteRect` over-GUI guard, `RuntimeInitializeOnLoadMethod` bootstrap, the existing registry/scan palette path for tiles, `ActiveTilemap()`/`SelectedTile()`. The whole file stays under `#if UNITY_EDITOR || DEVELOPMENT_BUILD`.

## ACCEPTANCE (verify before reporting DONE)
- `dotnet build` the RIMA.Runtime project AND ensure the 3 files compile (no UnityEditor calls in WallRunBuilder; composer's UnityEditor calls are `#if UNITY_EDITOR` guarded).
- Report: the 3 file paths, the exact `BuildRun` + `WallPiece` + `WallSegment` signatures you produced, and any deviation from the spec with reason. Do NOT touch any other file. Do NOT commit.
- If a spec detail is ambiguous, pick the minimal interpretation and note it under a "DEVIATIONS" heading — do not invent extra features (no ghost preview in V1 per risk #1).
