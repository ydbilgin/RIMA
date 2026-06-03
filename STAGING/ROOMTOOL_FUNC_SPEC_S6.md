# ROOMTOOL FUNCTIONALITY SPEC — Select → Place → Auto-Connect (S6)

Status: DESIGN (no code written). Grounded in the actual files listed under "Code read" below.
Vision (user, translated): "Townscaper but 2D, simpler — I select and place a piece, and pieces connect to each other."
Locked: tool lives in BOTH surfaces sharing ONE `RoomData` asset — (a) in-game F2 overlay (Play, pause, place into live top-down view) AND (b) Unity Editor window. Connect behavior = BOTH click-single Wang auto-connect (Townscaper) AND press-hold-drag connected run (Conor-Dart). Placeholders OK; the TOOL is the deliverable.

## Code read (ground truth)
- `Assets/Scripts/RoomPainter/RoomData.cs` — `floorCells`/`cliffCells` (`TileCellRecord`), `wallSegments` (`WallSegment`), `propPlacements` (`PropPlacement`). ScriptableObject. No per-cell wall occupancy grid today; walls are stored as from→to **runs**.
- `Assets/Scripts/RoomPainter/RoomPlacementTypes.cs` — `WallPiece { prefab, sprite, cornerSprite, footprint, displayName }`, `WallSegment { kind, fromCell, toCell, piece, height }`, `SegmentKind { SolidWall, VoidEdge, Entrance, BrokenGap, Anchor }`. ONLY `sprite` + optional `cornerSprite` — no T/end/cross variant slots.
- `Assets/Scripts/DevTools/WallRunBuilder.cs` — `BuildRun(grid, from, to, piece, parent, occupied)` rasterizes a Bresenham line, steps by `footprint.x`, **instantiates only** (never writes RoomData). Corner = swaps to `piece.cornerSprite` when incoming dir ≠ outgoing dir. `PlaceOne` for single. Sorting forced to `Entities`/order 0/pivot.
- `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` — F2 overlay. Paints Floor/Cliff into live **Tilemaps** (`SetTile`), Prop mode calls `WallRunBuilder.BuildRun` into `[DragPlace_Props]`. **Reads/writes NO RoomData.** No `UnityEditor` at runtime (wrapped `#if UNITY_EDITOR || DEVELOPMENT_BUILD`; only an `#if UNITY_EDITOR` fallback folder scan).
- `Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs` — `Compose(room)` → clears `[RoomPreview_Generated]`, rebuilds Floor/Cliff/Walls/Props groups from RoomData lists. Walls via `WallRunBuilder.BuildRun`. Editor-only (`AssetDatabase`/`PrefabUtility`).
- `Assets/Editor/RoomPainter/RoomPainterScenePlacer.cs` — SceneView click+drag placement of cliff/parallax (per-cell `_paintedCells` dedup, R-rotate). Instantiates only; does not write RoomData lists (separate from composer path).
- `Assets/Editor/RoomPainter/Authoring/RoomDataAuthoringController.cs` — RoomData library at `Assets/Data/Rooms/*.asset` via `AssetDatabase`. `SaveActiveRoom` → `SetDirty` + `SaveAssets`.

## Core gap
Walls are stored/built as **directional runs** with at most a single corner swap. There is no per-cell occupancy model, so a wall cell cannot know its 4 neighbors and therefore cannot auto-orient into straight / corner / T / cross / end. And the two surfaces are **disjoint**: Editor reads RoomData; F2 overlay writes nothing back. We unify both around ONE per-cell occupancy grid + ONE Wang resolver.

---

## 1. Wang CLICK auto-connect — `WangResolver` (SHARED, runtime-safe)

New file: `Assets/Scripts/RoomPainter/WangResolver.cs` (namespace `RIMA.RoomPainter`, in the runtime asmdef so BOTH Editor placer and F2 overlay reference it — **no `UnityEditor` usage**).

### Occupancy source
The resolver is **pure** — it never touches Unity assets. It reads occupancy from a delegate so the same code serves Editor (RoomData lists) and runtime (the cell set rebuilt from RoomData):

```csharp
public enum WangShape { Single, End, Straight, Corner, T, Cross }

public readonly struct WangResult
{
    public readonly WangShape shape;
    public readonly float rotationDegrees; // 0/90/180/270, CCW from canonical
    public readonly int neighborMask;      // bit0=+Y(N) bit1=+X(E) bit2=-Y(S) bit3=-X(W)
}

public static class WangResolver
{
    // 4-neighbor (N,E,S,W) occupancy → shape + rotation. 8-neighbor reserved for floor edges.
    public static WangResult Resolve4(Vector3Int cell, System.Func<Vector3Int, bool> isOccupied);

    // Floor edge variant: 8-neighbor mask → which edge/corner trim a floor cell needs.
    public static int EdgeMask8(Vector3Int cell, System.Func<Vector3Int, bool> isOccupied);
}
```

### 4-neighbor bitmask → shape/rotation table (the Wang rule)
Mask bits: `N=1, E=2, S=4, W=8`. `popcount` decides the family; rotation aligns the canonical sprite (canonical Straight = horizontal E–W; canonical Corner = connects S+E; canonical T = connects W+E+S; canonical End = connects S only):

| popcount | mask example | shape | rotationDeg |
|---|---|---|---|
| 0 | 0000 | Single | 0 |
| 1 | N(1)/E(2)/S(4)/W(8) | End | S→0, W→90, N→180, E→270 |
| 2 straight | E+W(10), N+S(5) | Straight | E+W→0, N+S→90 |
| 2 bent | S+E(6),S+W(12),N+W(9),N+E(3) | Corner | S+E→0, S+W→90, N+W→180, N+E→270 |
| 3 | W+E+S(14),N+S+W(13),N+E+S(7),N+E+W(11) | T | open side picks rot: openN→0, openE→90, openS→180, openW→270 |
| 4 | 1111 | Cross | 0 |

The resolver returns shape+rotation; the **caller** maps shape→`WangPiece` sprite/prefab and applies `Quaternion.Euler(0,0,rot)`. Rotation lets us ship FEWER sprites (one Corner rotated 4×) — but corners that must read "outer face down" may need distinct art (flag below).

### Connecting pieces the resolver needs (the new art set, per wall family)
- **Single** (isolated post / pillar cap)
- **End** (run terminator / wall stub) ×1 (rotated for 4 dirs)
- **Straight** (the existing flat run sprite) ×1 (rotated for H/V)
- **Corner** (L) ×1 (rotated for 4 dirs) — `cornerSprite` already exists; promote it
- **T** (3-way junction) ×1 (rotated for 4 dirs) — NEW
- **Cross** (+ 4-way) ×1 — NEW

Floor auto-edge (8-neighbor) is a SECOND, lighter Wang set (edge strip + outer/inner corner trims) consumed only by the Floor layer; Phase-2, flagged below.

---

## 2. Drag-array (Conor-Dart) — make `WallRunBuilder.BuildRun` WRITE RoomData

Today `BuildRun` instantiates only. Change so a drag persists and auto-connects:

1. Add `WallRunBuilder.AppendRunToRoom(RoomData room, Vector3Int from, Vector3Int to, WallPiece piece)` that:
   - rasterizes the same Bresenham line (reuse `GridLine`),
   - for each stepped cell, adds/updates an entry in a new **`RoomData.wallCells`** per-cell list (see §4) keyed by `cell`, skipping cells already occupied (dedup, matches current `occupied` HashSet),
   - does NOT pick a sprite per cell — occupancy only.
2. After writing, call a shared **`WangRebuild.ReorientWallCells(room, dirtyCells)`** that runs `WangResolver.Resolve4` on each newly-written cell **and its 4 neighbors** (endpoints + interior corners auto-fix), storing the resolved `shape`+`rotation` back into each `WallCell`.
3. Keep the existing `BuildRun(...)→List<GameObject>` for the **live preview/instantiation** during a drag, but have both surfaces call `AppendRunToRoom` on commit so the asset is the source of truth. Visuals are always a projection of `wallCells` (re-composed), never the authority.

Net: drag writes cells → Wang reorients endpoints/corners → recompose draws the correctly-oriented pieces. Single-click (§1) is just a 1-cell run through the same path.

---

## 3. Shared RoomData across BOTH surfaces

Problem: Editor uses `AssetDatabase` (compile-time `UnityEditor`); the F2 overlay must compile in a `DEVELOPMENT_BUILD` with NO `UnityEditor`. So they cannot share the `.asset` load path directly at runtime.

### Decision: ONE `.asset` is canonical; runtime uses a JSON sidecar bridge.
- **Canonical store (Editor):** `Assets/Data/Rooms/<roomId>.asset` (unchanged, `RoomDataAuthoringController`).
- **Runtime mirror:** alongside each room, a JSON sidecar `Assets/Data/Rooms/<roomId>.room.json` (copied into a Resources/StreamingAssets-readable location at build; in Editor Play it lives next to the asset and we read via path). Add a pure serializer `RoomDataJson` (runtime asmdef) using `JsonUtility` over a `[Serializable] RoomDataDTO` (the cell lists only — no `UnityEngine.Object` refs; assets referenced by `assetGuidOrName` string, which RoomData already uses).

### Flow
- **Editor save** (`SaveActiveRoom`): after `SetDirty/SaveAssets`, ALSO write `<roomId>.room.json` via `RoomDataJson.Write(room, path)`. (One extra line; keeps the asset authoritative and the JSON a derived mirror.)
- **In-game F2 load:** `InPlayMapPaintOverlay` resolves the active room id from `RoomLoader.OnRoomLoaded(RoomConfig config, ...)` (it already subscribes), reads `<roomId>.room.json`, and builds an in-memory `RoomData` (`ScriptableObject.CreateInstance` is runtime-legal). Place-in-game mutates this in-memory RoomData (§1/§2).
- **In-game F2 save:** write the in-memory RoomData back to `<roomId>.room.json`. In the Editor Play session ALSO `#if UNITY_EDITOR` push it into the `.asset` (`EditorUtility.CopySerialized` + `SetDirty`) so opening the Editor window shows the same map immediately. In a standalone `DEVELOPMENT_BUILD`, only the JSON updates (no AssetDatabase) — re-import on next Editor launch reconciles.
- **Editor window load:** unchanged (`AssetDatabase` `.asset`). Because save writes both, "place in-game then open Editor" = same map.

Path constant (shared, runtime-safe): `RoomDataPaths.JsonFor(roomId)` → `Assets/Data/Rooms/<roomId>.room.json` (Editor) / `Application.streamingAssetsPath/Rooms/<roomId>.room.json` (player). Single helper consumed by both surfaces.

---

## 4. Concrete change list per file

**`RoomData.cs`** (add per-cell wall model — runs stay for back-compat/migration):
- Add `[Serializable] struct WallCell { Vector3Int cell; SegmentKind kind; WangShape shape; float rotation; string pieceId; float height; }`.
- Add `public List<WallCell> wallCells = new();` + `EnsureLists()` null-guard.
- Add `Dictionary<Vector3Int,int>` runtime index (non-serialized) built on demand for O(1) occupancy in the resolver delegate.
- Keep `wallSegments` (runs) — migrate by expanding each run into `wallCells` once via `WallRunBuilder.AppendRunToRoom`.

**`RoomPlacementTypes.cs`**:
- Extend `WallPiece` with shape-variant sprites: `Sprite straightSprite, cornerSprite, tSprite, crossSprite, endSprite, singleSprite;` (cornerSprite already present). Add `string pieceId` (stable key for `WallCell.pieceId`).
- (Optional) `WangPieceSet` ScriptableObject grouping the 6 variants per wall theme, so a palette entry = one set.

**`WangResolver.cs`** (NEW, runtime): pure `Resolve4` / `EdgeMask8` per §1. No Unity asset refs.

**`WangRebuild.cs`** (NEW, runtime): `ReorientWallCells(RoomData, IEnumerable<Vector3Int> dirty)` — runs resolver on dirty ∪ neighbors, writes shape+rotation into `wallCells`. Shared by both surfaces.

**`RoomDataJson.cs` + `RoomDataPaths.cs`** (NEW, runtime): DTO + `JsonUtility` read/write + path helper (§3).

**`WallRunBuilder.cs`**:
- Add `AppendRunToRoom(RoomData, from, to, piece)` (writes `wallCells`, dedup) + call `WangRebuild.ReorientWallCells`.
- In `PlaceOne`/`BuildRun` instantiation, pick the sprite by `WallCell.shape` (map shape→variant on `WallPiece`) and apply `rotation`, replacing the current incoming≠outgoing single-corner heuristic.

**`RoomDataComposer.cs`** (`ComposeWallSegments`):
- Add a `ComposeWallCells(room.wallCells, grid, wallParent)` pass that instantiates per-cell using shape+rotation. Run the old `wallSegments` path only for unmigrated rooms.

**`RoomPainterScenePlacer.cs`** (Editor click/drag):
- On wall-layer click/drag, call `WallRunBuilder.AppendRunToRoom` on the **active RoomData** (route through `RoomDataAuthoringController.ActiveRoom`) then recompose, instead of instantiating loose GameObjects. Single click = 1-cell run = Wang neighbor re-eval.

**`InPlayMapPaintOverlay.cs`**:
- Replace the `[DragPlace_Props]` loose-instantiation path with: mutate in-memory RoomData via `AppendRunToRoom`, then recompose the wall group (re-use a runtime-safe subset of compose — sprite-only, no AssetDatabase). Floor/Cliff Tilemap painting can ALSO mirror into `floorCells`/`cliffCells` so it persists (Phase-2; today tiles are live-only).
- Load/save JSON per §3 on room load and on a "Save" GUI button.

**`RoomDataAuthoringController.cs`** (`SaveActiveRoom`):
- After `SaveAssets`, add `RoomDataJson.Write(ActiveRoom, RoomDataPaths.JsonFor(ActiveRoom.roomId))`.

---

## 5. NEW sprite pieces (sprite-gen follow-up — FLAG)
Current wall art = flat run segment (+ one corner). Wang needs, per wall theme (placeholder grey boxes acceptable now):
- **End** cap, **T** junction, **Cross** (+) junction — DO NOT EXIST, must be generated.
- **Single** post/pillar — may reuse an existing pillar.
- **Corner** — `cornerSprite` exists; verify it reads correctly at all 4 rotations, else author 4 distinct corners (flag: rotation may mirror lighting wrong on a top-down 3/4 wall — author distinct if so).
- **Floor edge set** (Phase-2): edge strip + inner/outer corner trims for 8-neighbor floor auto-edge.
PixelLab follow-up: generate `wall_<theme>_{straight,corner,t,cross,end,single}` at PPU64, pivot at base (Custom-Axis Y-sort), transparent. Log as placeholders in `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` per project rule.

## Biggest risk
The Editor↔runtime RoomData bridge. `AssetDatabase` is Editor-only and the F2 overlay must compile under `DEVELOPMENT_BUILD` without `UnityEditor`. If we let the overlay touch the `.asset` directly it won't compile for a build; if the JSON mirror drifts from the `.asset` the two surfaces silently disagree. Mitigation: `.asset` is the single authority, JSON is a derived mirror written on EVERY Editor save, and in-Editor Play pushes overlay edits straight back into the `.asset` via `EditorUtility.CopySerialized` (under `#if UNITY_EDITOR`) so divergence only exists in a real build, where the next Editor open reconciles.

## Phasing
- P1: `WangResolver` + `WallCell` model + `WallRunBuilder.AppendRunToRoom` + composer per-cell pass + Editor placer rewire (Editor surface fully Wang, single .asset). Verify in Editor window first (no runtime bridge yet).
- P2: JSON bridge + F2 overlay rewire to in-memory RoomData (both surfaces share). Floor auto-edge (8-neighbor). New T/Cross/End sprites.
