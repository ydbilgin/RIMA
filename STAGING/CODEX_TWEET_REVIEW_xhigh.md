# Technical Report - PowerPlay Data-First Rendering Lessons for RIMA Phase 1.5

Date: 2026-05-17
Scope read: `TWEET_REVIEW_BRIEFING.md`, `tweet_ss.png`, `X.mp4`, `X_frames/frame_01.png` through `frame_12.png`, and the current Brush V1 / Phase 1A Unity code paths named below.

## 1. Video summary

Independent observation from `X.mp4` and the extracted frames:

- The PowerPlay editor is a Unity-based, grid-authored, 3D terrain editor. The visible terrain is not a Unity 2D Tilemap surface: it has real elevation, cliff sides, ramps, water borders, and blended ground materials.
- Painting appears grid driven. A yellow rectangular grid overlay appears during brush placement, and the left toolbar contains terrain/brush operations such as terrain paint, ramp, erase/delete, scroll/document, and biome/water-like tools.
- Terrain visuals are material-driven and continuous. Sand, dirt, grass/forest, water, and dark ground states blend without per-cell visual seams. The author's later note about missing triplanar mapping is consistent with the visible cliff texture stretching and the intended future fix.
- Dense vegetation is rendered in high volume around terrain borders. It is visually too dense to be practical as one active gameplay `GameObject` per tree if editor responsiveness is the goal.
- Units and buildings are visibly distinct from terrain decoration. Frames 10-12 show selected tanks/mechs with HP bars and turret/building-like structures; these are likely the retained gameplay `GameObject` population.
- The editor UI has separate unit/building palettes, hotkey color slots, a minimap, and bottom command/hotbar panels. The optimization being discussed is not just rendering; it also protects editor interaction latency as the scene grows.

The strongest technical lesson is not "make RIMA 3D." It is "author into data, render visual-only content from batched derived data, and reserve scene objects for things with behavior, collision, selection, or runtime state."

## 2. ChatGPT interpretation verdict

Verdict: mostly correct, with a few important precision fixes.

Correct:

- The core interpretation of the tweet is accurate: removing `GameObject` overhead from everything except units/buildings means the scene hierarchy is no longer the source of truth for static visual terrain/detail.
- The RIMA translation is directionally right: `RoomData` / room template data should own map state; visual-only content should be serialized placements or generated/baked visual layers; gameplay entities should remain `GameObject`-backed.
- Option B, "Hybrid Tilemap + Chunked Sprite/Mesh Layers," fits RIMA better than a full 3D or DOTS/ECS pivot.
- The specific anti-pattern list is valid for RIMA: moss decals, grime, dirt patches, cracks, pebbles, dust, floor patches, rubble, ambient glows, and visual shadows should not become one `GameObject` each.

Corrections / uncertainty:

- The video proves a 3D mesh/material terrain path for PowerPlay, not a Tilemap path. For RIMA, Tilemap remains valid for structural 2D layers because RIMA's locked constraints are PPU=32, 32x32 base terrain, and Wang16 for elevation/feature edges only.
- "Dozens of trees are not individual GameObjects" is very likely given the tweet, but it is not visually provable from frames alone. They could be instanced meshes, static batched renderers, or a custom chunk renderer. The report should treat the implementation as inferred, not directly observed.
- Texture splatting is a plausible read of the terrain, but the only confirmed author detail is "grid based terrain editor" in Unity and the explicit note that triplanar mapping is still missing.
- "GameObjects only for units/buildings" should not be copied literally to RIMA. RIMA also needs `GameObject`s for player, enemies, NPCs, doors, chests, destructibles, combat interactables, major collision/trigger volumes, and tool hosts.

Missing from the prior interpretation:

- Authoring undo/redo needs to operate on data, not object creation/destruction, or the editor will keep paying hierarchy cost during painting.
- The renderer must solve both CPU hierarchy cost and GPU batching. Removing `GameObject`s is not enough if each decal still becomes an unbatched sprite/material draw.
- Dirty-chunk rebuild is the key operational unit. Full-room mesh rebuilds after every brush dab will only move the bottleneck.
- RIMA needs a migration bridge because the current test suite and Brush V1 result contract report spawned objects.

## 3. RIMA applicability matrix

| PowerPlay lesson | RIMA action | Applicability |
|---|---|---|
| Grid-authored terrain can render from derived data instead of many objects | Keep `RoomData` / `RoomTemplateSO` as source of truth; render from serialized grids and placement lists | Copy directly |
| Units/buildings stay as `GameObject`s | Keep Player, Enemies, NPCs, interactables, doors, chests, destructibles, major collision volumes as `GameObject`s | Copy with RIMA-specific categories |
| Dense forest/decoration should not be scene hierarchy | Move L2b/L4/L5/L6 visual-only placements to serialized data plus chunked rendering | Copy directly |
| 3D terrain mesh/material blending | Translate to 2D Tilemap + macro patch + chunked sprite/quad layers; do not convert RIMA to full 3D | Adapt |
| Triplanar mapping for cliffs | Not relevant to current 2D locked constraints, except as an analogy for natural wall/elevation texture continuity | Do not copy literally |
| RTS-scale unit/building palettes | RIMA only needs editor-scale palette ergonomics for rooms, props, biomes, and brush presets | Adapt |
| Everything except units/buildings removed from objects | Too strict for RIMA because collision props, doors, destructibles, and triggers need components | Do not copy literally |

## 4. Phase 1.5 architecture proposal

### Current pressure points

- `BrushExecutorResult` exposes `List<GameObject> spawnedObjects`, coupling brush success reporting to hierarchy creation (`Assets/Scripts/MapDesigner/Brush/Executors/IBrushExecutor.cs:20`).
- `FreeformDecalExecutor` calls `DecorativeExecutorUtility.PlaceSingle`, which resolves a sprite and creates a `GameObject` for every L4/L5/L6 decal (`FreeformDecalExecutor.cs:19`, `:54`, `:224`, `:268`).
- `ScatterAlongStrokeExecutor` samples along the stroke and calls the same `PlaceAt` path for every scatter point (`ScatterAlongStrokeExecutor.cs:52`, `:58`, `:68`).
- `CompositeStrokeExecutor` aggregates `spawnedObjects`, so composite brushes inherit the same object-first model (`BrushExecutorRouter.cs:214`, `:243`, `:245`).
- Erase tools delete child `GameObject`s from named roots, so erasing is also hierarchy-first (`EraseByLayerExecutor.cs:18`, `:38`; `EraseAllDecorativeExecutor.cs:27`, `:35`).
- Current Phase 1A SO contracts are renderer-agnostic and therefore compatible with a pivot: terrain, patch atlas, prop, and visual profile SOs store data/sprites/settings, not renderers (`Assets/Scripts/Rima/MapDesigner/SO/*.cs`).

### Proposed new data structures

Add a runtime-safe data layer under `Assets/Scripts/MapDesigner/Visuals/`:

```csharp
[Serializable]
public struct VisualPlacement
{
    public string placementId;
    public TargetLayer layer;
    public string assetGuid;
    public string variantId;
    public Vector2 worldPosition;
    public Vector2Int cell;
    public Vector2 scale;
    public float rotationDegrees;
    public Color32 tint;
    public int sortingOrderOffset;
    public int seed;
    public VisualPlacementFlags flags;
}
```

```csharp
[Serializable]
public sealed class VisualLayerData
{
    public TargetLayer layer;
    public int schemaVersion = 1;
    public List<VisualPlacement> placements = new List<VisualPlacement>();
}
```

```csharp
[CreateAssetMenu(menuName = "RIMA/Room/RoomVisualData")]
public sealed class RoomVisualDataSO : ScriptableObject
{
    public string roomId;
    public RectInt bounds;
    public int chunkSize = 8;
    public List<VisualLayerData> layers = new List<VisualLayerData>();
}
```

Add a registry/binding asset:

- `VisualAssetRegistrySO`: maps `assetGuid` / `variantId` to sprite, source atlas, material, default pivot, default layer, and allowed transforms.
- It can initially wrap existing `AssetPoolSO` and Phase 1A `PatchAtlasSO` data so asset growth does not stop.

### Proposed services/classes

- `IVisualPlacementStore`
  - `Add(VisualPlacement placement)`
  - `RemoveInRadius(TargetLayer layer, Vector2 worldPosition, float radius)`
  - `ClearLayer(TargetLayer layer)`
  - `ClearDecorative()`
  - `QueryLayer(TargetLayer layer)`
  - `MarkDirty(VisualChunkKey key)`

- `EditorVisualPlacementStore`
  - Stores placements in `RoomVisualDataSO` or a `RoomTemplateSO.visualData` reference.
  - Uses `Undo.RecordObject(roomVisualData, "...")` instead of `Undo.RegisterCreatedObjectUndo`.
  - Marks dirty chunks after each add/remove.

- `ChunkedVisualLayerRenderer`
  - One renderer host per room/layer, not one object per placement.
  - Builds meshes/quads per chunk from `VisualPlacement` data.
  - Uses one material per atlas/layer where possible. If multiple atlases are needed, split per atlas inside the chunk, not per placement.

- `VisualChunkMeshBuilder`
  - Converts sprite UVs/pivot/size/rotation/tint into mesh buffers.
  - Supports sorting layer/order by layer and coarse sublayer, not arbitrary per-object renderer state.

- `BrushVisualPlacementFactory`
  - Extracts the deterministic sprite/variant/position/scale/rotation/tint logic now embedded in `DecorativeExecutorUtility.PlaceAt`.
  - Returns `VisualPlacement`; does not instantiate anything.

- `VisualLayerBakePreview`
  - Editor-only component that rebuilds dirty chunks after a brush stroke for immediate author feedback.

### Brush code changes

1. Extend `BrushStroke` with a non-serialized visual sink, or introduce a `BrushExecutionContext` passed by `BrushExecutorRouter`.

Preferred longer-term shape:

```csharp
public readonly struct BrushExecutionContext
{
    public readonly BrushStroke Stroke;
    public readonly IVisualPlacementStore VisualStore;
    public readonly IBrushRenderInvalidator RenderInvalidator;
}
```

If avoiding a broad interface change in the first step, inject the visual store into `BrushExecutorRouter` and use a small adapter for decorative executors.

2. Replace `DecorativeExecutorUtility.PlaceSingle` with two paths:

- `CreatePlacement(...)`: deterministic data creation.
- `PlaceSingleLegacyGameObject(...)`: temporary compatibility path.

3. Add an output mode:

```csharp
public enum DecorativeOutputMode
{
    LegacyGameObject,
    DataOnly,
    DualWriteForMigration
}
```

Start in `DualWriteForMigration` for tests and visual comparison. Switch production to `DataOnly` once chunk rendering parity is verified.

4. Change `BrushExecutorResult` additively:

```csharp
public int visualPlacementCount;
public List<VisualPlacement> visualPlacements;
```

Keep `spawnedCount` temporarily so current tests do not break. For data-only decorative strokes, `spawnedCount` can mean "placements affected" until tests are updated, but new tests should assert `visualPlacementCount`.

5. Update layer routing:

- L1 base floor: Tilemap.
- L2 floor variation: Tilemap.
- L2b macro painterly patches: chunked visual layer.
- L3 elevation/wall feature edges: Tilemap for Wang16 edge tiles where possible; if wall sprites are larger overlays, use chunked renderer. Avoid one `GameObject` per wall segment for dense wall dressing.
- L4 organic transition patches: chunked visual layer.
- L5 detail decals/scatter: chunked visual layer.
- L6 accents/glows/visual shadows: chunked visual layer unless they are gameplay interactables.
- Gameplay props: keep `PropPlacementData` plus `PropRuntimeSpawner` for props with collision/destruction/interaction. Non-interactive props can later use a `VisualPropPlacement` path.

6. Update erasers:

- `EraseByLayerExecutor` removes placements from `IVisualPlacementStore` and marks dirty chunks instead of searching `GameObject` roots.
- `EraseAllDecorativeExecutor` clears visual layers L2b/L4/L5/L6 in data and rebuilds/bakes chunk renderers.
- Legacy root deletion remains only as a migration cleanup command.

7. Update save/load:

- Add `RoomTemplateSO.visualData` or a parallel room-id-linked `RoomVisualDataSO`.
- Keep `RoomTemplateSO.props` for behavior props. It is already data-first for prop placement (`RoomTemplateSO.cs:26`, `PropPlacementData.cs`), even though runtime prop spawning creates objects by design.
- Room authoring save should serialize visual placement data, not child decorative objects.

### Migration steps

1. Add data structures, store, registry, and tests without changing live brush behavior.
2. Add dual-write mode to L4/L5/L6 only. Confirm the generated placement count matches legacy spawned count.
3. Implement chunked preview renderer for L5 first because detail decals have the highest object-count pressure and the least gameplay coupling.
4. Move L4 and L6 to data-only after visual parity.
5. Move L3 wall overlays away from per-segment objects only after the structural Tilemap/chunk decision is validated.
6. Add an editor migration command: "Convert Decorative Children To VisualData" that scans `TransitionBrushLayer`, `DetailDecalLayer`, and `AccentLayer`, records placements, and optionally disables/deletes legacy children.
7. Update tests from object-count assertions to placement-count and chunk-renderer assertions.

## 5. Phase 1A trajectory verdict

Verdict: hybrid pivot now.

Continue Phase 1A SO work because it is renderer-agnostic and directly supports the new path. Do not continue expanding production content through the current GameObject-per-decal brush path. The correct move is to freeze high-density decorative production, add Phase 1.5 data-first rendering as a thin vertical slice, and then resume asset/library growth through the new placement store. Waiting until 20-30 MVP rooms are authored will turn a known architecture issue into migration debt.

## 6. Benchmark plan

Benchmark scene:

- Room size: 40x25 cells.
- Base: L1/L2 Tilemaps populated.
- Visual-only data: exactly 1000 placements distributed across L4/L5/L6.
- Legacy baseline: 1000 spawned decorative child `GameObject`s using current `FreeformDecalExecutor` / `ScatterAlongStrokeExecutor` path.
- New path: 1000 `VisualPlacement` entries rendered through chunked layers with chunk size 8x8 cells.
- Camera: same orthographic 2D room view used by MapDesigner.
- Assets: one SpriteAtlas/material per layer where possible; repeat with 3 atlases to test worst-case batch splits.

Measurements:

- Hierarchy count: total objects, objects under decorative roots, renderer component count.
- Paint stroke time: stopwatch around one 100-placement stroke and one 1000-placement auto-dress operation.
- Room bake time: time to serialize data and rebuild all dirty chunks.
- Runtime load: time from room template selection to visible room.
- GC alloc: `GC.GetAllocatedBytesForCurrentThread()` around paint, erase, bake, and load.
- Frame time: ProfilerRecorder or FrameTimingManager in editor preview and PlayMode.
- Draw calls / batches: Unity rendering stats for legacy vs chunked.
- Memory: managed memory delta plus mesh/texture object count.
- Undo/redo time: 100 add operations, 100 erase operations, then undo/redo all.

Pass targets:

- 1000 visual-only placements create no more than one host object per room/layer/chunk group, not one object per placement.
- 1000-placement bake completes under 250 ms in editor on the target workstation.
- Single stroke dirty rebuild completes under 16 ms for affected chunks, or defers rendering until stroke end if needed.
- Undo/redo records data-object changes, not 1000 created/destroyed objects.
- Full EditMode suite remains green; add focused Phase 1.5 tests before switching default output mode.

## 7. Risks + tradeoffs

1. Renderer parity risk: SpriteRenderer behavior is forgiving; mesh/quads require correct pivot, sorting, tint, flipping, rotation, alpha, and atlas UV handling. Mitigation: dual-write visual comparison during migration.
2. Undo complexity: Data-first undo is cleaner long term but requires precise `Undo.RecordObject` boundaries. Poor grouping can corrupt authoring data or create huge undo records.
3. Sorting limitations: Per-placement `sortingOrder` is easy with SpriteRenderer but expensive in chunk meshes. RIMA should restrict decorative sorting to layer plus small suborder bands.
4. Atlas/material fragmentation: Chunk rendering only helps if assets share atlases/materials. Asset import rules must enforce atlas grouping for L2b/L4/L5/L6.
5. Erase/query behavior: Radius erase becomes a data query problem. It needs spatial indexing or chunk-local lists once placement counts exceed a few thousand.
6. Existing tests assume objects: Current Brush V1 tests may assert spawned counts or child objects. Keep additive result fields and dual-write until tests are migrated.
7. Authoring inspectability loss: Individual child objects are easy to click in the hierarchy. Replace that with a placement inspector, hover picking, and selected-placement handles.
8. Prop classification risk: Some props are decorative now but may later need collision/destruction. Use explicit prop flags (`visualOnly`, `hasCollision`, `destructible`, `interactable`) so migration is reversible per asset.

## 8. Final recommendation

Adopt the data-first rule in Phase 1.5 before scaling content: `GameObject`s are for behavior, collision, triggers, selection, and runtime state; visual-only brush output is serialized placement data rendered by chunked visual layers. Keep Phase 1A SO contracts and the locked 2D constraints intact, route L1/L2 and structural L3 through Tilemap where appropriate, move L2b/L4/L5/L6 into chunked sprite/mesh renderers, and use dual-write only as a short migration bridge. This is the smallest pivot that preserves RIMA's current direction while preventing the Brush V1 hierarchy from becoming the project's next performance ceiling.
