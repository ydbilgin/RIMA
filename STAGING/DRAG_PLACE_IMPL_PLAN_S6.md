# DRAG-PLACE + RuinedKeepComposer — Implementation Plan (S6)

**Author:** Opus 4.8 (senior gameplay/tools engineer, reviewer). **Writer != reviewer** — this is implementable by a Sonnet/Codex writer.
**Goal:** Add Conor-Dart-style hold-drag continuous placement to RIMA's F2 in-Play overlay (`InPlayMapPaintOverlay.cs`), in BOTH a Tile mode and a new Wall/Prop sprite mode, AND a deterministic `RuinedKeepComposer` that rebuilds the connected wall-run room in one call. The drag-place wall mode and the composer SHARE one core method.

**Grounding facts (verified in repo):**
- F2 overlay paints `TileBase` onto `Floor`/`Cliff` `Tilemap`s via `PaintAtMouse(Vector2 screenPos, bool set)` (lines 120-140) called from `Update()` (lines 90-116). Single-cell per frame; NO interpolation, NO undo, NO grid-crossing throttle.
- Walls are **GameObject sprites**, NOT tiles → tile painting can't make a wall run. New mode required.
- `RuntimeAssetRegistry` (`Assets/Scripts/Live/RuntimeAssetRegistry.cs`) is the live asset source. `RegistryEntry` carries `sprite`, `tile`, `prefab`, `tag` (e.g. "wall","prop"), `layer` (`RoomPainter.RoomLayer`), `kind`. Filter via `GetByTag("wall")` / `GetByLayer(RoomPainter.RoomLayer.Wall)`.
- `RoomPainter.RoomLayer` (`Assets/Scripts/RoomPainter/RoomLayer.cs`): `Floor=0,Edge=1,Cliff=2,Wall=3,Props=4,...`.
- Room look LOCK (`STAGING/RUINED_KEEP_ROOM_LOOK_LOCK_S6.md`): walls = bottom-center pivot, sprite Sort Point=Pivot, Custom-Axis (0,1,0) Y-sort, SortingGroup for multi-sprite assemblies. Wall kit module sizes @64PPU: 128×192/128×160 walls, 64×128 pillars, 160×192 arch gate. Kit sprites staged in `STAGING/ruinedkeep_wallkit/` (`RUINED_KEEP_WALLKIT_IMAGEGEN_CX_TASK.md`).
- `DEPTH_AND_WALLRUN_RECIPE_S6.md` does NOT yet exist — the composer's segment data shape is DESIGNED below so the recipe author fills the values into a serialized asset, no code change.

---

## 0. Shared core method (the convergence point)

Both the wall drag-place mode and the composer call ONE method to lay a connected run of wall sprites between two cells. Put it in a NEW static helper so neither owns it:

```csharp
// Assets/Scripts/DevTools/WallRunBuilder.cs  (NEW — runtime, under #if UNITY_EDITOR || DEVELOPMENT_BUILD)
namespace RIMA.DevTools
{
    public static class WallRunBuilder
    {
        /// Lay a connected run of wall sprites from cell A to cell B along the
        /// grid line, auto-spaced by the prefab/sprite footprint (in cells),
        /// bottom-center pivot, parented under 'parent', on sorting layer
        /// "Entities" (Custom-Axis Y-sort, Sort Point = Pivot). Auto-orients the
        /// piece at direction changes (corner variant) when 'orientFn' supplies one.
        /// Returns every GameObject it instantiated (for the undo/stroke record).
        /// Idempotent per cell: never double-places on a cell already in 'occupied'.
        public static List<GameObject> BuildRun(
            Grid grid,
            Vector3Int fromCell,
            Vector3Int toCell,
            WallPiece piece,             // sprite/prefab + footprint(cells) + corner variant
            Transform parent,
            HashSet<Vector3Int> occupied); // cells already filled this run; mutated
    }

    [System.Serializable]
    public struct WallPiece            // one palette/segment entry
    {
        public GameObject prefab;      // preferred; instantiated if non-null
        public Sprite sprite;          // fallback: build a bare GO+SpriteRenderer
        public Sprite cornerSprite;    // optional corner/turn variant
        public Vector2Int footprint;   // cells the piece occupies (default 1x1)
        public string displayName;
    }
}
```

`BuildRun` internals: Bresenham/`GridLine(fromCell,toCell)` → step by `footprint.x` along the dominant axis → for each step cell not in `occupied`, instantiate (prefab via `Object.Instantiate`, else new GO + SpriteRenderer with `sprite`), set world pos = `grid.GetCellCenterWorld(cell)` snapped so the **bottom-center pivot** sits on the cell's ground line, set `SpriteRenderer.sortingLayerName="Entities"`, `sortingOrder=0`, `spriteSortPoint=Pivot`, add the cell to `occupied`, append GO to the returned list. Corner detection: when the incoming and outgoing step directions differ and `cornerSprite != null`, swap to the corner variant.

---

## 1. TILE drag-place — exact edits to `InPlayMapPaintOverlay.cs`

Upgrade the existing single-click tile paint into hold-drag. All edits are in this one file.

**New state fields (add near lines 48-68, the `// ── State ──` block):**
```csharp
private bool   _dragging;
private Vector3Int _lastCell;            // last cell painted this stroke
private bool   _hasLastCell;
private Vector3Int _strokeAnchor;        // first cell, for Shift axis-lock
// One drag = one undo entry. In-memory stroke stack (Tilemap has no native play-mode undo).
private struct TileEdit { public Vector3Int cell; public TileBase before; }
private readonly List<TileEdit> _currentStroke = new List<TileEdit>();
private readonly Stack<List<TileEdit>> _undo = new Stack<List<TileEdit>>();
```

**Rewrite `Update()` (lines 90-116) input section** so LMB/RMB become a drag lifecycle (keep the F2 toggle + palette-rect guard exactly as-is):
- On `mouse.leftButton.wasPressedThisFrame` (and not over `_paletteRect`): start stroke — `_dragging=true; _currentStroke.Clear(); _hasLastCell=false; _strokeAnchor = CellUnderMouse();`
- While `_dragging && mouse.leftButton.isPressed`: compute `Vector3Int cell = CellUnderMouse();` If `_hasLastCell && cell==_lastCell` → return (grid-crossing throttle: only act when entering a NEW cell). If Shift held → snap `cell` to `_strokeAnchor` axis-lock (clamp the smaller of |dx|,|dy| to 0). Then **interpolate**: `foreach (Vector3Int c in GridLine(_hasLastCell ? _lastCell : cell, cell)) PaintCell(c, set:true);` Set `_lastCell=cell; _hasLastCell=true;`
- On `mouse.leftButton.wasReleasedThisFrame`: commit stroke — if `_currentStroke.Count>0` push a COPY onto `_undo`; `_dragging=false`.
- **RMB = cancel/erase.** While dragging, `mouse.rightButton.wasPressedThisFrame` → roll back the in-progress stroke (re-apply each `TileEdit.before`) and clear it = cancel. When NOT dragging, RMB drag erases using the same line+throttle path with `set:false` (erase strokes are also undoable).
- Ctrl+Z (`keyboard.ctrlKey.isPressed && keyboard.zKey.wasPressedThisFrame`) → pop `_undo`, re-apply each `before` in reverse.

**New helper methods (replace `PaintAtMouse`, lines 120-140):**
```csharp
private Vector3Int CellUnderMouse() { /* the ScreenToWorldPoint + WorldToCell from old PaintAtMouse, lines 126-128 */ }

private void PaintCell(Vector3Int cell, bool set) {
    Tilemap target = ActiveTilemap(); if (target == null) return;
    TileBase before = target.GetTile(cell);
    TileBase next = set ? SelectedTile() : null;
    if (before == next) return;                    // dedupe no-op
    _currentStroke.Add(new TileEdit{ cell=cell, before=before });
    target.SetTile(cell, next);
}

// Bresenham line between two cells (XY plane, z ignored). Yields each cell incl. endpoints.
private static IEnumerable<Vector3Int> GridLine(Vector3Int a, Vector3Int b) { /* standard int Bresenham */ }
```
`SelectedTile()` / `ActiveTilemap()` (lines 142-148) stay unchanged. `_eraseMode` toggle stays — when on, LMB strokes call `PaintCell(c,false)`.

**GUI (`OnGUI`, lines 228-264):** add a hint line `"Hold-drag paint · Shift straight · RMB cancel/erase · Ctrl+Z undo"`. No other GUI change for tile mode.

---

## 2. WALL/PROP drag-place MODE — new layer option in the overlay

Add a third mode to the F2 overlay so dragging lays a connected run of **wall sprites** (not tiles).

**Enum change (line 46):** `private enum PaintLayer { Floor, Cliff, Prop }` and add a `"Prop"` toggle to the layer row (line 240-242).

**Prop palette (parallel to the tile palette, lines 60-61):**
```csharp
private readonly List<WallPiece> _propPalette = new List<WallPiece>();
private int _propSelected;
private Transform _propParent;   // "[DragPlace_Props]" GO under the active room (or scene root)
private readonly HashSet<Vector3Int> _propOccupied = new HashSet<Vector3Int>();
private readonly Stack<List<GameObject>> _propUndo = new Stack<List<GameObject>>();
private List<GameObject> _propStroke;
```

**Palette sourcing (extend `RebuildPalette`, lines 188-212):** when in Prop mode, populate `_propPalette` from the registry:
`foreach (RegistryEntry e in registry.GetByTag("wall") ∪ GetByTag("prop"))` → build a `WallPiece{ prefab=e.prefab, sprite=e.sprite, footprint = FootprintFromSpriteSize(e.sprite), displayName=e.displayName }`. Footprint derived from sprite px / 64 PPU rounded up (128px→2 cells, 64px→1 cell). Corner variant: optional, by name convention (`*_corner` registry entry matched to the base). If the registry has no wall/prop entries (kit not yet baked), FALLBACK: load `STAGING/ruinedkeep_wallkit/*.png` is editor-only — so the fallback is a small hardcoded `Resources.LoadAll<Sprite>("Live/WallKit")` if present, else show `"<no wall/prop assets baked — Refresh after bake>"`.

**Drag behaviour (in the rewritten `Update`):** identical lifecycle to tile mode, but the per-stroke action calls the shared core:
- On press: `_propStroke = new List<GameObject>(); _propOccupied.Clear();`
- Per grid-crossing: `_propStroke.AddRange(WallRunBuilder.BuildRun(_grid, _lastCell, cell, _propPalette[_propSelected], PropParent(), _propOccupied));` (Shift axis-lock applies to `cell` before the call → straight wall runs.)
- On release: if `_propStroke.Count>0` push to `_propUndo`.
- RMB while dragging = cancel: `Destroy` every GO in `_propStroke`, clear. Ctrl+Z = pop `_propUndo`, `Destroy` each GO.
- Erase mode in Prop: RMB-drag over a cell raycasts the prop layer (`Physics2D.OverlapPoint` on a small "DragPlaceProp" layer OR a simple proximity test against `_propParent` children) and destroys the nearest wall GO (recorded for undo).

**Parenting / sorting:** `PropParent()` finds-or-creates a `[DragPlace_Props]` child under the active room instance (from `HandleRoomLoaded`'s `roomInstance`, else scene root). Every instantiated wall = sorting layer `"Entities"`, order 0, Sort Point = Pivot, bottom-center pivot — set inside `WallRunBuilder.BuildRun` so the composer gets it for free. Multi-sprite pieces get a `SortingGroup` (per look-LOCK).

---

## 3. RuinedKeepComposer — one-call deterministic room rebuild

A small component, callable from a ContextMenu (Editor) AND at runtime, that reads a serialized perimeter-segment plan and builds the whole connected room via the SAME `WallRunBuilder.BuildRun` core.

```csharp
// Assets/Scripts/DevTools/RuinedKeepComposer.cs  (NEW)
namespace RIMA.DevTools
{
    public enum SegmentKind { SolidWall, VoidEdge, Entrance, BrokenGap }  // matches look-LOCK §"PLACEMENT ALGORITHM"

    [System.Serializable]
    public struct WallSegment
    {
        public SegmentKind kind;
        public Vector3Int  fromCell;
        public Vector3Int  toCell;
        public WallPiece   piece;     // sprite/prefab + footprint + corner variant
        public float       height;    // visual height hint (px) for art selection / future
    }

    public sealed class RuinedKeepComposer : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private List<WallSegment> _segments = new List<WallSegment>();  // authored by the recipe

        [ContextMenu("Compose Ruined Keep")]
        public void Compose()  // Editor right-click
        {
            ClearPrevious();                                   // destroy "[RuinedKeep_Walls]" child if present
            Transform parent = NewParent("[RuinedKeep_Walls]");
            var occupied = new HashSet<Vector3Int>();
            foreach (WallSegment s in _segments)
            {
                if (s.kind == SegmentKind.Entrance || s.kind == SegmentKind.VoidEdge) continue; // gaps stay open
                WallRunBuilder.BuildRun(_grid, s.fromCell, s.toCell, s.piece, parent, occupied);
            }
        }
    }
}
```
- **Data shape is the deliverable for the recipe author:** the `DEPTH_AND_WALLRUN_RECIPE_S6.md` author fills the `_segments` list (in the Inspector or a ScriptableObject) with the perimeter plan: N-back SolidWall run + corner buttresses, E/W mixed SolidWall + BrokenGap, S-front VoidEdge/low-parapet, N-center Entrance (arch gate). No code change to add a room — only data.
- `Compose()` is runtime-callable (a public method) so it can be triggered from the F2 overlay ("Compose room" button) or a boot hook; the `[ContextMenu]` gives the edit-mode authoring path.
- Shares `WallRunBuilder.BuildRun` → the composer and the drag tool produce byte-identical wall placement (same spacing, pivot, sorting).

---

## 4. File list, complexity, risks

**NEW files:**
- `Assets/Scripts/DevTools/WallRunBuilder.cs` — shared core + `WallPiece` struct. ~120 LOC. **Medium.**
- `Assets/Scripts/DevTools/RuinedKeepComposer.cs` — composer + `WallSegment`/`SegmentKind`. ~100 LOC. **Low-Medium.**

**EDITED files:**
- `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` — drag lifecycle in `Update` (90-116), replace `PaintAtMouse` (120-140) with `CellUnderMouse`/`PaintCell`/`GridLine`, add stroke-undo stack, add `Prop` enum/palette/parenting, GUI hints. ~200 LOC delta. **Medium-High** (most of the work; keep the F2 toggle + `_paletteRect` guard + registry palette path intact).

**Estimated total:** ~1 focused day for a competent writer. Tile drag-place first (self-contained, demoable), then `WallRunBuilder`, then wire Prop mode, then composer.

**Risks:**
1. **IMGUI ghost preview is hard.** OnGUI can't easily draw a world-space tinted ghost of the full drag path. MITIGATION: ship V1 with NO ghost (commit-on-cross is immediate, undo is the safety net); add a cheap ghost later via `Debug.DrawLine`/`GL` lines or a pooled translucent SpriteRenderer that follows the cursor cell. Do not block V1 on ghost.
2. **Play-mode instantiate vs edit-mode.** Composer runs in BOTH. In play mode use `Object.Instantiate`/`Destroy`; in edit mode (ContextMenu) the writer must guard with `#if UNITY_EDITOR` and use `Undo.RegisterCreatedObjectUndo` / `DestroyImmediate` so edit-mode authoring is undoable and doesn't leak. `WallRunBuilder` itself stays play-safe (`Instantiate`); the composer wraps it for edit-mode.
3. **Prior MCP/play crash note** (memory [[feedback-compile-in-unity-autonomous]]): heavy play-mode instantiation under MCP control has crashed before. MITIGATION: cap `BuildRun` per-stroke instantiations (e.g. ≤64 GO), batch under one parent, no per-GO `Resources.Load` in the hot loop (resolve sprites once at palette build). Composer clears its previous parent before rebuild to avoid GO pile-up across repeated calls.
4. **Footprint/spacing wrong → walls overlap or gap.** `FootprintFromSpriteSize` must match the actual baked kit sizes (128px=2 cells). Verify against `STAGING/ruinedkeep_wallkit/` once baked; until then the registry tag path may be empty (graceful "no assets" message, not a crash).
5. **Y-sort regression.** Walls MUST go on the single `"Entities"` layer with Sort Point=Pivot or they break Custom-Axis sorting (memory [[feedback-depth-sort-custom-axis-not-manual-ysort-s6]]). Centralizing this in `BuildRun` is the guard — do NOT set sortingOrder manually elsewhere.
