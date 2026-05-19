# RIMA Map Composer - Multi-Projection Architecture

## 0. Position

RIMA should commit now to a projection-neutral Map Composer contract, but not to shipping four production renderers before V1. The V1 shipping renderer remains low top-down 2D. The work that must happen now is narrower and more valuable: preserve `RoomData` as the source of truth, stop baking visual-only decoration into per-object scene hierarchies, and make every brush write semantic cell-space data that can be rendered by a swappable projection adapter.

Evidence from the current code supports this split. `RoomData` already stores size, seed, vertex terrain, cell terrain, walkability, wall edges, encounters, patch/decal/accent atlases, scatter, natural features, and feature smoothing profile (`Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs:17-33`). The generator builds this from `RoomRecipe` and keeps render-facing assets as references on the room data (`Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs:83-99`). Serialization already flattens room grids and stores wall/encounter data (`Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs:347-387`). That is enough to feed several renderers. The weak spot is not data. The weak spot is that current visual layers still lean on Tilemap and SpriteRenderer placement assumptions.

Local PowerPlay validation: I ran `ffprobe` on `STAGING/X.mp4`; the file is 1920x1080, 60 fps, about 95.0 seconds, with 5695 video frames and 12 extracted PNG frames in `STAGING/X_frames`. The prior Codex frame review correctly treats PowerPlay as real 3D terrain with elevation/cliffs/material blend, not a 2D Tilemap surface (`STAGING/CODEX_TWEET_REVIEW_xhigh.md:8-10`). It also warns that dense decoration batching is inferred, not directly provable from frames alone (`STAGING/CODEX_TWEET_REVIEW_xhigh.md:32-33`). So the HD-2D lesson is: borrow data-first/chunk rendering, not the entire 3D stack blindly.

## 1. Multi-projection renderer abstraction

The renderer boundary should be a package-level interface, not a RIMA gameplay class. It consumes `RoomData`, a visual profile, asset registries, and optional projection settings. It produces a render output root, structural surfaces, visual chunk surfaces, collider/navigation artifacts, and debug handles. The current `MapLayerOrchestrator` already shows the shape of this pipeline: floor base/variation, wall overlay, transition brush, detail decal, and accent toggles (`Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs:13-21`), then ordered paint passes (`Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs:47-84`). That class should become a low-top-down renderer implementation, not the global architecture.

The renderers must not own room generation. They translate the same cell data into projection-specific output:

- Top-down: square Tilemap plus chunked sprite/quads.
- Low top-down: square Tilemap plus Y-sorted sprite/quads and wall caps.
- Isometric: diamond cell projection plus iso-specific sorting and iso-authored sprite directions.
- HD-2D: 3D mesh terrain plus 2D sprite quads/billboards.

```csharp
namespace MapComposer.Rendering
{
    public enum RoomProjection { TopDown, LowTopDown, Isometric, HD2D }

    public sealed class RoomRenderContext
    {
        public Transform Parent;
        public MaterialLibrary Materials;
        public VisualAssetRegistry Assets;
        public LightingProfile Lighting;
        public int Seed;
        public bool EditorPreview;
        public RectInt DirtyCells;
    }

    public sealed class RoomRenderOutput
    {
        public GameObject Root;
        public List<UnityEngine.Object> StructuralSurfaces = new();
        public List<UnityEngine.Object> VisualChunkSurfaces = new();
        public List<Collider> Colliders = new();
        public Bounds Bounds;
        public RoomProjection Projection;
    }

    public interface IRoomRenderer
    {
        RoomProjection Projection { get; }
        bool Supports(RoomData room, RoomVisualProfile profile);
        RoomRenderOutput Render(RoomData room, RoomVisualProfile profile, RoomRenderContext context);
        void RebuildDirty(RoomRenderOutput output, RoomData room, RectInt dirtyCells);
        Vector3 CellToWorld(Vector2 cell, int elevation = 0);
        Vector2Int WorldToCell(Vector3 world);
        int SortKey(Vector2 cell, int layer, int subOrder);
    }

    public sealed class TopDownRenderer : IRoomRenderer
    {
        public RoomProjection Projection => RoomProjection.TopDown;
        private Tilemap floor;
        private ChunkedSpriteLayer visuals;

        public bool Supports(RoomData room, RoomVisualProfile profile)
        {
            return room.vertexGrid != null && room.walkable != null;
        }

        public RoomRenderOutput Render(RoomData room, RoomVisualProfile profile, RoomRenderContext context)
        {
            var output = CreateOutputRoot("TopDownRoom", context);
            floor = CreateTilemap(output.Root, "L1_L2_Floor");
            PaintBaseFloor(room, profile, context);
            PaintFeatureEdges(room, profile, context);
            visuals = new ChunkedSpriteLayer(output.Root.transform, 8, Projection);
            visuals.Rebuild(room.VisualPlacements, context.Assets, fullRoom: true);
            Build2DColliders(output, room);
            output.StructuralSurfaces.Add(floor);
            output.VisualChunkSurfaces.AddRange(visuals.Meshes);
            output.Bounds = BoundsFromRoom(room);
            return output;
        }

        public void RebuildDirty(RoomRenderOutput output, RoomData room, RectInt dirtyCells)
        {
            RepaintFloorTiles(room, dirtyCells);
            visuals.RebuildDirty(room.VisualPlacements, dirtyCells);
        }

        public Vector3 CellToWorld(Vector2 cell, int elevation = 0)
        {
            return new Vector3(cell.x, cell.y, -elevation * 0.01f);
        }

        public Vector2Int WorldToCell(Vector3 world)
        {
            return new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));
        }

        public int SortKey(Vector2 cell, int layer, int subOrder)
        {
            return layer * 1000 + subOrder;
        }
    }

    public sealed class LowTopDownRenderer : IRoomRenderer
    {
        public RoomProjection Projection => RoomProjection.LowTopDown;
        private Tilemap floor;
        private ChunkedSpriteLayer visualChunks;
        private WallCapLayer wallCaps;

        public bool Supports(RoomData room, RoomVisualProfile profile)
        {
            return room.vertexGrid != null && room.wallEdges != null && room.walkable != null;
        }

        public RoomRenderOutput Render(RoomData room, RoomVisualProfile profile, RoomRenderContext context)
        {
            var output = CreateOutputRoot("LowTopDownRoom", context);
            ConfigureOrthographicCamera(profile.Camera, tiltDegrees: 30f);
            floor = CreateTilemap(output.Root, "L1_L2_Floor");
            PaintBaseFloor(room, profile, context);
            PaintWangFeatureEdgesOnly(room, profile, context);
            wallCaps = new WallCapLayer(output.Root.transform);
            wallCaps.Build(room.wallEdges, profile.WallKit, context.Seed);
            visualChunks = new ChunkedSpriteLayer(output.Root.transform, 8, Projection);
            visualChunks.SortMode = ChunkSortMode.YThenLayer;
            visualChunks.Rebuild(room.VisualPlacements, context.Assets, fullRoom: true);
            Build2DColliders(output, room);
            output.StructuralSurfaces.Add(floor);
            output.VisualChunkSurfaces.AddRange(visualChunks.Meshes);
            output.VisualChunkSurfaces.AddRange(wallCaps.Meshes);
            return output;
        }

        public void RebuildDirty(RoomRenderOutput output, RoomData room, RectInt dirtyCells)
        {
            RepaintFloorTiles(room, dirtyCells);
            wallCaps.RebuildDirty(room.wallEdges, dirtyCells);
            visualChunks.RebuildDirty(room.VisualPlacements, dirtyCells);
        }

        public Vector3 CellToWorld(Vector2 cell, int elevation = 0)
        {
            return new Vector3(cell.x, cell.y, -elevation * 0.02f);
        }

        public Vector2Int WorldToCell(Vector3 world)
        {
            return new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));
        }

        public int SortKey(Vector2 cell, int layer, int subOrder)
        {
            return Mathf.RoundToInt(-cell.y * 100f) + layer * 10000 + subOrder;
        }
    }

    public sealed class IsometricRenderer : IRoomRenderer
    {
        public RoomProjection Projection => RoomProjection.Isometric;
        private IsoDiamondTileLayer terrain;
        private ChunkedSpriteLayer visualChunks;

        public bool Supports(RoomData room, RoomVisualProfile profile)
        {
            return room.vertexGrid != null && profile.HasIsoSpriteSet;
        }

        public RoomRenderOutput Render(RoomData room, RoomVisualProfile profile, RoomRenderContext context)
        {
            var output = CreateOutputRoot("IsometricRoom", context);
            terrain = new IsoDiamondTileLayer(output.Root.transform, profile.IsoTileSize);
            terrain.BuildFromVertexGrid(room.vertexGrid, room.size, profile.IsoTerrainAtlas);
            terrain.ApplyWangMasks(room.vertexGrid, profile.IsoWangAtlas);
            visualChunks = new ChunkedSpriteLayer(output.Root.transform, 8, Projection);
            visualChunks.CellProjector = CellToWorld;
            visualChunks.SortMode = ChunkSortMode.IsoYThenXThenLayer;
            visualChunks.Rebuild(room.VisualPlacements, context.Assets, fullRoom: true);
            BuildCellSpace2DColliders(output, room);
            output.StructuralSurfaces.AddRange(terrain.Renderers);
            output.VisualChunkSurfaces.AddRange(visualChunks.Meshes);
            return output;
        }

        public void RebuildDirty(RoomRenderOutput output, RoomData room, RectInt dirtyCells)
        {
            terrain.RebuildDirty(room.vertexGrid, dirtyCells);
            visualChunks.RebuildDirty(room.VisualPlacements, dirtyCells);
        }

        public Vector3 CellToWorld(Vector2 cell, int elevation = 0)
        {
            float x = (cell.x - cell.y) * 0.5f;
            float y = (cell.x + cell.y) * 0.25f + elevation * 0.25f;
            return new Vector3(x, y, -y * 0.01f);
        }

        public Vector2Int WorldToCell(Vector3 world)
        {
            float gx = world.x + world.y * 2f;
            float gy = world.y * 2f - world.x;
            return new Vector2Int(Mathf.FloorToInt(gx), Mathf.FloorToInt(gy));
        }

        public int SortKey(Vector2 cell, int layer, int subOrder)
        {
            return Mathf.RoundToInt(-(cell.x + cell.y) * 100f) + layer * 10000 + subOrder;
        }
    }

    public sealed class HD2DRenderer : IRoomRenderer
    {
        public RoomProjection Projection => RoomProjection.HD2D;
        private TerrainMeshLayer terrain;
        private BillboardSpriteLayer billboards;

        public bool Supports(RoomData room, RoomVisualProfile profile)
        {
            return room.vertexGrid != null && room.terrainGrid != null && profile.HasHD2DMaterials;
        }

        public RoomRenderOutput Render(RoomData room, RoomVisualProfile profile, RoomRenderContext context)
        {
            var output = CreateOutputRoot("HD2DRoom", context);
            Configure3DCamera(profile.Camera, profile.HD2DAngle);
            terrain = new TerrainMeshLayer(output.Root.transform, chunkSize: 8);
            terrain.Build(room.vertexGrid, room.terrainGrid, profile.MaterialSet, profile.HeightScale);
            terrain.ApplySplatMasks(room.VisualPlacements, profile.MaterialSet);
            billboards = new BillboardSpriteLayer(output.Root.transform, BillboardMode.YBillboard);
            billboards.Projector = CellToWorld;
            billboards.Rebuild(room.VisualPlacements, context.Assets);
            Build3DWalkableColliders(output, room);
            Apply3DLighting(profile.Lighting, output.Root);
            output.StructuralSurfaces.AddRange(terrain.MeshFilters);
            output.VisualChunkSurfaces.AddRange(billboards.Renderers);
            return output;
        }

        public void RebuildDirty(RoomRenderOutput output, RoomData room, RectInt dirtyCells)
        {
            terrain.RebuildDirty(room.vertexGrid, room.terrainGrid, dirtyCells);
            billboards.RebuildDirty(room.VisualPlacements, dirtyCells);
        }

        public Vector3 CellToWorld(Vector2 cell, int elevation = 0)
        {
            return new Vector3(cell.x, elevation, cell.y);
        }

        public Vector2Int WorldToCell(Vector3 world)
        {
            return new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.z));
        }

        public int SortKey(Vector2 cell, int layer, int subOrder)
        {
            return layer * 1000 + subOrder;
        }
    }
}
```

## 2. Asset pipeline per projection

Use one asset identity catalog, but projection-specific render variants. The current Phase 1A SO set is close: `TerrainDefinitionSO` stores terrain identity, walkability, movement blocking, base tile variants, visual category, and density defaults (`Assets/Scripts/Rima/MapDesigner/SO/TerrainDefinitionSO.cs:5-17`); `PatchAtlasSO` stores role, valid terrains, variants, density, min distance, edge/wall bias, and allowed transforms (`Assets/Scripts/Rima/MapDesigner/SO/PatchAtlasSO.cs:6-18`); `RoomVisualProfileSO` groups visual mode, WallKit flag, allowed patch atlases, allowed props, and lighting profile id (`Assets/Scripts/Rima/MapDesigner/SO/RoomVisualProfileSO.cs:6-17`). The tests assert the import roles and visual modes exist (`Assets/Tests/EditMode/MapDesigner/SO/Phase1ASoContractsTests.cs:25-53`).

Projection rules:

- Top-down: a floor can be one 32x32 sprite variant set; characters can use 1, 4, or 8 directions depending on facing fidelity. This is the cheapest projection for future games.
- Low top-down: keep RIMA's 30-35 deg sprite language. Characters and asymmetric combat effects need 8-direction production for the polished target, even if V1 can map movement to fewer directions.
- Isometric: do not reuse low-top-down 8-dir sprites blindly. Cardinal screen directions differ from iso world axes. The natural Little Rocket Lab look is achievable, but RIMA combat pays in aiming, hitbox perception, pathing display, and iso-specific sprite authoring.
- HD-2D: use the low-top-down character set on Y-billboard or fixed-plane quads. Ground art becomes splat/material/mesh texture data; props remain 2D sprites placed in 3D space.

Painterly texture from Codex imagegen can work in all four only if it is stored as source material, not as final map pixels. Top-down and low-top-down consume it as macro patch atlases or tile variants. Isometric needs diamond-sliced variants or projected quad patches. HD-2D uses it as splat/albedo detail on mesh surfaces.

Import settings should stay semantic: `ImportAssetRole` already separates terrain, macro patch, organic decal, scatter, accent, prop, character, background, and light source (`Assets/Scripts/Rima/MapDesigner/SO/ImportAssetRole.cs:3-14`). Add `ProjectionVariantSetSO` with per-projection sprite/material slots and keep PPU/import/filtering on the variant, not on gameplay data.

## 3. Brush authoring projection-agnostic

Brushes must write in cell space and world-normalized masks. `BrushStroke` already carries world positions, start/current cell, seed, `RoomData`, biome skin, and stroke path (`Assets/Scripts/MapDesigner/Brush/Stroke/BrushStroke.cs:8-20`). `IBrushExecutor` already abstracts "mode plus operation" (`Assets/Scripts/MapDesigner/Brush/Executors/IBrushExecutor.cs:9-23`). That is the right input layer.

The wrong abstraction is "brush creates SpriteRenderer object." `TransitionBrushPainter` currently places patches by iterating walkable cells, density, min distance, encounter avoidance, and then instantiating a GameObject with SpriteRenderer (`Assets/Scripts/MapDesigner/TransitionBrushPainter.cs:40-80`, `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs:256-288`). Keep the density logic. Move the result into `VisualPlacement` data. The previous tweet review already recommends this: L2b/L4/L5/L6 become serialized placements rendered by chunked layers (`STAGING/CODEX_TWEET_REVIEW_xhigh.md:193-199`), and high-density production should not keep scaling the current GameObject-per-decal path (`STAGING/CODEX_TWEET_REVIEW_xhigh.md:228`).

For a user painting "moss", the brush writes:

- semantic layer: L4 organic decal or L5 detail scatter;
- mask: cell-space stroke path and optional alpha texture;
- constraints: walkable, distance from encounters, wall/feature proximity;
- placement seed: deterministic variant/rotation/scale/tint;
- projection hint: none by default, unless the asset lacks a variant for a renderer.

Walls should be semantic wall edges, not tile art. The generator already derives `wallEdges` from walkable to non-walkable transitions (`Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs:275-322`). `WallOverlayPainter` turns those segments into wall sprites (`Assets/Scripts/MapDesigner/WallOverlayPainter.cs:19-45`). Top-down can render them as flat blockers or low caps; low-top-down as Hades-style caps; isometric as diamond-edge wall modules; HD-2D as mesh cliffs or vertical sprite facades.

Props are already close to projection-neutral. Runtime spawning creates a GameObject from GUID-based placement, tile position, rotation, variant, sorter, and collider (`Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs:17-74`). Behavior props should remain GameObjects. Visual-only props can join chunked placement data. Sorting differs per renderer: current Y-position sorting is explicit (`Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs:40-50`), iso needs diagonal sort, and HD-2D relies more on depth testing plus billboard order.

## 4. Lighting per projection

`LightingProfileSO` should be a semantic profile with per-projection adapters. Current Phase 1A SOs only store `lightingProfileId` on room visual and prop definition (`Assets/Scripts/Rima/MapDesigner/SO/RoomVisualProfileSO.cs:16`, `Assets/Scripts/Rima/MapDesigner/SO/PropDefinitionSO.cs:20`). The planned design puts global ambient and emitter profiles into a `LightingProfileSO`, driven by visible prop sources and a room-level global light (`STAGING/RIMA_FLUID_TRANSITION_DESIGN.md:224-261`).

Projection behavior:

- Top-down and low-top-down: URP 2D Global/Point/Freeform lights, sprite blob shadows for grounding, no baked light in tile art.
- Isometric: URP 2D can still work, but shadow direction and sprite contact blobs must use iso axes. A north wall in cell space is not a screen-up wall.
- HD-2D: use URP 3D lights and materials. Keep the same semantic fields: ambient color, emitter budget, source prop id, radius, color, flicker. The renderer maps them to Light2D or Light components.

The profile should not promise identical lighting. It should promise equivalent intent: "cold dungeon ambient, warm torch sources, sparse rift cyan accents." Each renderer owns the physical implementation.

## 5. Wang16 across projections

Wang16 remains a semantic boundary resolver, not the naturalness solution. The current `CornerWangPainter` reads four corner terrain values and paints a Tilemap cell (`Assets/Scripts/Systems/Map/CornerWangPainter.cs:24-44`). `CornerWangTileSetSO.GetTile` maps NW/NE/SW/SE to a 16-key lookup (`Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs:22-30`). Feature-edge mode only resolves special pairings when allowed (`Assets/Scripts/Systems/Map/CornerWangPainter.cs:108-120`). This is already compatible with a renderer abstraction.

Projection mapping:

- Top-down: current square cell, current corner lookup.
- Low top-down: same lookup, but use it for elevation/feature edges only. Natural same-height moss/dirt blending comes from L2b/L4/L5.
- Isometric: same corner bits, different art slicing and placement. The diamond tile is a projected cell; the corner mask is still cell-space.
- HD-2D: the 16-key mask becomes mesh topology/material transition input. It does not have to pick a sprite tile.

The docs are aligned: Wang16 is 32x32 native, one cell (`STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md:29-31`), reserved for elevation/feature boundaries (`STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md:37-45`), and the final L0-L11 layer model keeps L3 as feature/elevation edges only (`STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md:93-109`). The Phase 0 review explicitly found same-elevation Wang moss transitions visually rigid and grid-fragmented (`STAGING/CHATGPT_PHASE0_REVIEW.md:41-42`).

## 6. Recommended implementation order

Do not start by deepening all four renderers. Start by separating data and rendering.

1. Continue Phase 1A low-top-down 2D as the base validation path. It is the RIMA shipping path and exercises the full map stack.
2. Add data-first visual placement plus chunked renderer for L2b/L4/L5/L6. This is the minimum shared substrate for all projections and solves the scaling risk identified in the PowerPlay review (`STAGING/CODEX_TWEET_REVIEW_xhigh.md:50-51`, `STAGING/CODEX_TWEET_REVIEW_xhigh.md:256-258`).
3. Add TopDownRenderer second. It is closest to the existing square grid, easiest to test, and proves projection swap without asset reauthoring.
4. Add IsometricRenderer as a prototype only. Natural look yes; RIMA combat feel not proven. It needs iso-facing sprite production and movement readability work.
5. Add HD2DRenderer as a separate proof, not a migration. It has the highest ceiling but changes lighting, mesh build, colliders, materials, and test expectations.

One-week prototype if doing it now:

- Day 1: create `MapComposer.Runtime` assembly, `IRoomRenderer`, `RoomRenderContext`, `RoomRenderOutput`, and `ProjectionVariantSetSO`.
- Day 2: wrap existing low-top-down pipeline behind `LowTopDownRenderer`.
- Day 3: implement `TopDownRenderer` with the same `RoomData`.
- Day 4: implement `VisualPlacementStore` and chunked L5 renderer only.
- Day 5: create one shared room asset and two scenes: low-top-down and top-down.
- Day 6: add graybox iso and HD-2D preview using placeholders, not production art.
- Day 7: capture side-by-side screenshots, measure hierarchy object count, dirty rebuild time, draw calls, and combat readability with one character.

## 7. Reusability across games

Package split:

```text
Assets/Packages/MapComposer.Runtime/
  Data/              RoomData adapter interfaces, terrain ids, masks
  Rendering/         IRoomRenderer, projection math, chunk layers
  Brushes/           semantic brush ops, no RIMA terms
  Assets/            ProjectionVariantSetSO, VisualAssetRegistrySO
  Lighting/          LightingProfileSO semantic schema

Assets/Scripts/Rima/MapComposer/
  RimaRoomDataAdapter.cs
  RimaLowTopDownRendererConfig.cs
  RimaBiomeToComposerAdapter.cs
  RimaCombatReadabilityRules.cs
```

Generic namespace: `MapComposer.*`. RIMA namespace: `RIMA.*`. Generic code cannot reference class names, acts, rift palette, RIMA biome enums, RIMA room archetypes, or class/mob rules. This follows the existing portable-core decision that Core must be RIMA-free while game-layer classes stay in `Assets/Scripts/Systems/Map/Rima*` (`TASARIM/MASTER_KARAR_BELGESI.md:126`).

Version the package by data contract, not renderer count:

- `0.1`: low-top-down adapter and top-down renderer.
- `0.2`: data-first visual placement and chunk renderer.
- `0.3`: iso preview.
- `0.4`: HD-2D preview.
- `1.0`: stable authoring/save format with migration tools.

## 8. Risk and tradeoff matrix

| Area | Implementation risk | Asset risk | Performance risk | Art ceiling | Verdict |
|---|---:|---:|---:|---:|---|
| Top-down | Low | Low | Low | Medium-high | Best second renderer |
| Low top-down | Medium | Medium | Medium | High enough for RIMA V1 | Shipping path |
| Isometric | Medium | High | Medium | High for cozy/strategy | Future game, not RIMA V1 default |
| HD-2D | High | Medium | High | Highest | Prototype before pivot |
| Shared `RoomData` | Low | Low | Low | Enables all | Keep |
| VisualPlacement chunks | Medium | Low | Low after done | High | Do now |
| LightingProfile adapters | Medium | Low | Medium | High | Semantic now, per-renderer later |
| Wang16 semantic mask | Low | Medium | Low | Medium | Keep scoped to features |

What breaks later if wrong:

- If brushes continue spawning visual GameObjects, every projection inherits a migration debt.
- If `RoomData` stores renderer objects, package portability dies.
- If iso ships without iso-specific directions, combat and facing readability suffer.
- If HD-2D starts before data-first chunks, it becomes a rewrite instead of a renderer.
- If all natural blending is forced through Tilemap, 3D portability dies and the grid fight returns.

## 9. Concrete validation prototype

Minimum demo:

- One `RoomData` JSON/asset: 16x12, one walkable floor, one non-walkable perimeter, one elevation/feature patch, one encounter slot, one wall edge set.
- One `RoomVisualProfile`: shared palette, one `LightingProfileSO` id, one WallKit reference, one patch/decal/accent atlas set.
- One base floor texture: 32x32, four variants.
- Five prop sprites: crate, column, brazier, rubble, shrine. Behavior props spawn as GameObjects; visual-only rubble can be chunked.
- One character: 64x64 placeholder, low-top-down facing set.
- Four scenes reading the same room data: `Room_TopDown`, `Room_LowTopDown`, `Room_Isometric_Graybox`, `Room_HD2D_Graybox`.
- Pass criteria: all scenes show the same room topology; walkable cells match; wall/perimeter matches; decals do not reveal a square grid; prop collision matches cell footprint; dirty rebuild after a brush stroke affects only dirty chunks; the low-top-down scene remains the combat readability baseline.

Metrics:

- Hierarchy count: visual-only L2b/L4/L5/L6 should not create one object per placement.
- Rebuild: affected chunk rebuild under 16 ms or deferred until stroke end.
- Rendering: batches/chunks per layer, not per decal.
- Design: iso screenshot may be natural, but combat test must be judged separately.

## 10. Final verdict

Commit now to a multi-projection-compatible architecture, not to a full multi-projection product. RIMA should not migrate to HD-2D yet and should not switch to isometric for V1 combat. The exact one-week scope is `IRoomRenderer`, low-top-down adapter, top-down second renderer, data-first L5 chunk renderer, one shared room, and graybox iso/HD-2D previews. During V1, the hard rules are: `RoomData` remains renderer-agnostic, brushes write semantic cell-space placement data, Wang16 stays feature/elevation only, visual-only layers render through chunked data, PixelLab pixel art remains the asset source, and low-top-down remains the shipping RIMA renderer until a side-by-side prototype proves a better path.
