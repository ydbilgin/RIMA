# Codex Strategic Verdict - 2D vs HD-2D

Date: 2026-05-17
Scope: read-only strategic review. No `Assets/` files modified.

## 1. Independent Verdict

Verdict: **C, but as a hard-gated one-week prototype, not an open-ended parallel production track.**

I do not recommend a full B pivot today, because the current Brush V1/data stack is too valuable to throw into a 5-6 week renderer migration before seeing a RIMA-specific HD-2D slice. I also do not recommend pure A as the only path, because the requested target is not merely "better 2D floor art"; it is the Amine PowerPlay natural terrain impression: continuous terrain, real elevation, palette/biome swaps over the same shape, and decorations that sit in a spatial world. A can improve a lot, but it cannot honestly guarantee that ceiling.

So the right decision-quality move is **C with kill criteria**:

- Continue Yol A only where it remains renderer-agnostic: clean base, macro patch source, SO contracts, placement data, room/procedural data, and asset QC.
- Start one HD-2D prototype that proves the hard unknowns with RIMA assets and RIMA data, not a generic Unity demo.
- Decide after the prototype whether to stay A, pivot B, or keep only selected HD-2D techniques.
- Stop the HD-2D branch after one week unless it produces a direct visual answer.

This is not "agreeing with C" by default. It is rejecting A-only because it risks another tuning loop, rejecting B-now because it is too expensive without a slice, and narrowing C so it cannot become a two-track maintenance drag.

## 2. Amine / PowerPlay Validation

`ffprobe` on `STAGING/X.mp4` confirms the source is a real 1920x1080, 60 fps H.264 video, 94.916667 seconds, 5695 video frames. The extracted `STAGING/X_frames/frame_01.png` and `frame_12.png` are 960x540 RGB frames.

Direct frame review supports the briefing: terrain is authored on a grid but rendered as continuous 3D terrain. The cliff faces and ramps are vertical/depth geometry, the water/forest/sand/snow/red biome states change material appearance without exposing cell seams, and units/structures remain separate scene entities. This validates the architectural analogy: data/grid authoring plus non-grid continuous rendering.

## 3. Time / Risk / Cost Estimates

| Path | Time | Content risk | Quality ceiling | Engineering risk |
|---|---:|---|---|---|
| A: continue 2D layered composition | ~3 weeks to a strong MVP if current Phase 1A asset work continues | Medium-high. Needs many accepted macro/decal assets and careful density tuning | Very good 2D room screenshots; not PowerPlay-grade continuous terrain/elevation | Medium. Current per-decal GameObject path must become data/chunked or it will become debt |
| B: pivot HD-2D now | ~5-6 weeks to migration MVP, likely 7+ if lighting/camera/sorting fights appear | Medium. Existing raw sprites survive, but many need material/wrapper checks | Highest. Can mathematically remove tile grid and own elevation/biome swaps | High. Tilemap, renderer, brush output, tests, lighting, and sprite-on-quad sorting all change |
| C: one-week prototype while A continues narrowly | 5 working days prototype + 1 day review/decision | Low-medium if scope is enforced | Decision-quality evidence, not production quality | Medium. Risk is false confidence if prototype omits hard cases |

Two-track maintenance cost for C is acceptable only if the prototype is isolated under `Assets/Scenes/Prototypes/` and `Assets/Scripts/Prototypes/HD2D/`, with no shared production rewrites until the verdict is made.

## 4. Concrete Prototype Scope For C

Minimum demo: **`Assets/Scenes/Prototypes/HD2D_Prototype.unity`**.

The prototype should prove or disprove this exact question: "Can RIMA data and RIMA sprite assets produce a natural, non-grid, Amine-like room faster than extended 2D decal tuning?"

Required vertical slice:

1. Build one 24x16 terrain mesh from `RoomData` or a small `RoomData` fixture. One cell = one Unity unit. Use vertex/cell terrain IDs, not Tilemap.
2. Add two elevation levels plus one ramp/cliff edge. If the prototype cannot make a cliff/ramp feel better than Yol A, HD-2D loses most of its reason.
3. Use one painterly 1024 source texture or equivalent material texture on the terrain. Add at least three palette/material states: normal, snow, red/Mars. The room shape and object positions must remain identical across states.
4. Add five 2D sprite quads in the 3D world: one character-scale sprite, one brazier/prop, one tree/large deco, one wall/rock accent, one decal-like flat quad.
5. Add a fixed ortho or shallow perspective camera at RIMA's high top-down angle. Confirm sprites do not look skewed, blurred, or scale-broken.
6. Add one primitive brush/editor action: paint terrain ID into a `RoomData` field and rebuild dirty mesh or full mesh. Do not port full Brush V1.
7. Capture a side-by-side: current Yol A room screenshot vs HD-2D prototype, same room footprint and similar palette.

Kill criteria:

- If the mesh terrain still reads tiled or noisy after 2 days, stop.
- If sprite quads cannot match PixelLab asset readability at camera angle by day 3, stop.
- If material/lighting setup consumes more time than terrain/asset proof, stop.
- If the prototype needs a full Brush V1 rewrite before visual proof, stop.

Success criteria:

- No visible 32x32 grid on the main ground plane.
- Cliff/elevation reads better than any 2D Wang/decal solution.
- Biome swap is a material/palette swap over the same geometry.
- PixelLab sprites remain readable and correctly grounded.
- The demo can be explained as a renderer swap fed by the same room/placement data.

## 5. Migration Scope For B

### Survives Mostly Intact

`BrushStroke` survives as authoring intent: it already carries world positions, grid cells, seed, `RoomData`, biome skin, and stroke path without directly naming Tilemap or SpriteRenderer (`Assets/Scripts/MapDesigner/Brush/Stroke/BrushStroke.cs:11-19`).

`RoomData` survives as the best source of terrain truth: it stores size, seed, `vertexGrid`, `terrainGrid`, `walkable`, wall edges, encounters, patch atlases, scatter brush, and natural features (`Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs:17-33`). Its JSON DTO already flattens and restores vertex/cell grids and walkability (`Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs:346-387`).

`RoomTemplateSO.props` and `PropPlacementData` survive as data-first prop placement. The template stores prop placements as data (`Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:25-30`), and each placement is a GUID plus tile position, rotation steps, user, and variant index (`Assets/Scripts/MapDesigner/Props/PropPlacementData.cs:7-14`).

The Phase 1A SOs mostly survive as catalogs: `TerrainDefinitionSO` stores terrain ID, display name, sprite variants, walkability, visual category, color, and density (`Assets/Scripts/Rima/MapDesigner/SO/TerrainDefinitionSO.cs:8-17`); `PatchAtlasSO` stores role, valid terrain IDs, variants, density, distance, edge/wall bias, and transforms (`Assets/Scripts/Rima/MapDesigner/SO/PatchAtlasSO.cs:9-18`); `RoomVisualProfileSO` stores mode, wall/background flags, tone, allowed atlases/props, and lighting profile ID (`Assets/Scripts/Rima/MapDesigner/SO/RoomVisualProfileSO.cs:9-17`).

### Must Be Rewritten

`GridTileExecutor` is Tilemap-bound. It resolves a Tilemap, picks a `TileBase`, calls `tilemap.SetTile`, and returns the Tilemap as modified (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/GridTileExecutor.cs:33-53`). In B this becomes `TerrainMeshPaintExecutor`: write terrain/elevation IDs into room data, then rebuild mesh chunks.

`WallStampExecutor` is GameObject/SpriteRenderer-oriented wall overlay code. It finds wall segments, resolves a `WallOverlayPainter`, resolves a Tilemap, places a wall sprite GameObject, and registers undo for the spawned object (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/WallStampExecutor.cs:41-55`). In B this becomes either mesh cliff edge generation or billboard wall stamps.

`DecorativeExecutorUtility` is the clearest object-first coupling. It creates a new GameObject per decal (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs:224-229`), adds a `SpriteRenderer`, assigns sprite/tint/sorting layer/order (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs:268-273`), and returns it through `spawnedObjects`.

`ScatterAlongStrokeExecutor` repeats that object path for every sample along a stroke (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/ScatterAlongStrokeExecutor.cs:51-59`). In B this must emit placement records or quad instances, not scene children.

Erasers are hierarchy-bound. `EraseByLayerExecutor` iterates root children and destroys matching GameObjects (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/EraseByLayerExecutor.cs:32-44`); `EraseAllDecorativeExecutor` finds decorative roots and destroys every child (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/EraseAllDecorativeExecutor.cs:25-41`).

`BrushExecutorResult` itself is biased toward GameObjects: it exposes `spawnedCount` and `List<GameObject> spawnedObjects` (`Assets/Scripts/MapDesigner/Brush/Executors/IBrushExecutor.cs:16-22`). It needs additive fields such as `terrainCellsChanged`, `visualPlacementsChanged`, and `dirtyChunks`.

The renderer asset is currently a 2D renderer path: `UniversalRP.asset` points to a renderer list with default renderer index 0 (`Assets/Settings/UniversalRP.asset:17-21`), and that renderer asset is named `Renderer2D` with 2D light blend styles/material defaults (`Assets/Settings/Renderer2D.asset:13-68`). B needs a Universal Renderer or a separate URP asset/renderer setup for 3D terrain plus transparent sprite quads.

### Asset / Shader Rework

Most source images survive. The wrapper changes:

- Terrain/painterly source becomes material textures, splat masks, palette variants, or triplanar/UV textures.
- PixelLab sprites become billboard/fixed-plane quads with a URP 3D-compatible transparent lit/unlit shader.
- `PatchAtlasSO.variants` and `PropDefinitionSO.worldSprite/variantSprites` remain useful references (`Assets/Scripts/Rima/MapDesigner/SO/PatchAtlasSO.cs:12`, `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs:13-15`, `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs:39-41`).
- Tilemap-only assets need adaptation: `MapTerrain` stores `TileBase baseTile`, `CornerWangTileSetSO baseTileSource`, and `variantPool` of `TileBase` (`Assets/Scripts/Systems/Map/MapTerrain.cs:22-31`). B must wrap those in mesh material/edge definitions.

### Tests

Survive with little change: RoomTemplate data tests, prop placement validation, prop rotation, registry, serialization, and many procedural room tests.

Port: brush routing, walkability masks, density, weighted asset pick, auto-dress determinism, and prop runtime spawn tests.

Dead or heavily rewritten: Tilemap `SetTile/GetTile` assertions (`Assets/Tests/EditMode/Brush/BrushExecutorTests.cs:91-108`), wall/decal SpriteRenderer count assertions (`Assets/Tests/EditMode/Brush/BrushExecutorTests.cs:111-153`), Freeform/Scatter spawned GameObject assertions (`Assets/Tests/EditMode/Brush/BrushDecorativeExecutorTests.cs:40-89`), and hierarchy eraser tests (`Assets/Tests/EditMode/Brush/BrushDecorativeExecutorTests.cs:137-175`).

## 6. Quality Ceiling For A

A can become significantly better than the Phase 0/Phase 1 failures if the team follows the ChatGPT final direction: clean low-contrast base, macro patches from large painterly sources, irregular alpha, organic decals, detail scatter, props, shadows, and manual polish.

But A has an inherent ceiling against the Amine target:

- A still starts with a repeated base tile field. Macro patches can hide it, but the foundation can leak whenever density drops, camera zoom changes, or asset contrast is off.
- Same-elevation floors can be hidden well; cliffs, ramps, vertical terrain, and biome swaps cannot be equal to mesh terrain without faking them through many art layers.
- The more A tries to become seamless, the more it depends on large overlays and hand tuning. That can work for hero rooms, but it is fragile for a roguelite room library.
- If A keeps the current GameObject-per-decal path, high-density polish increases hierarchy and undo cost instead of just visual quality.

Honest answer: **A can reach "very good composed 2D"; it cannot reliably reach "PowerPlay seamless terrain" without either becoming a near-baked painting workflow or borrowing mesh/chunk rendering ideas.**

## 7. Renderer-Agnostic Verification

The data layer is close, but not fully renderer-agnostic yet.

Good:

- `RoomData` has grid data, terrain data, walkability, wall edges, natural features, and serialization. That can feed Tilemap or mesh.
- Prop placements are GUID/tile/rotation data, not renderer components.
- Phase 1A SO contracts describe identity, role, density, valid terrain, transforms, profile, and lighting ID. Those concepts carry to both A and B.

Hidden 2D assumptions:

- Several SOs store `Sprite` or `TileBase` directly. That is fine as an asset reference, but B needs renderer-neutral wrappers or parallel material/mesh references.
- `ImportAssetRole` encodes pixel-size roles like `Terrain32` and `MacroPatch64_128` (`Assets/Scripts/Rima/MapDesigner/SO/ImportAssetRole.cs:3-14`). Useful for asset QC, but B should interpret these as source asset roles, not renderer dimensions.
- `BrushLayerOperation.minDistance` defaults to `32f` (`Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs:14`), while `WorldToCell` floors Unity world positions (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs:276-279`). If one cell = one unit, this value is a scale smell. The prototype should normalize brush distances to cells/world units explicitly.
- `BrushExecutorRouter` maps L1/L2 to GridTile and L3-L6 to stamp/freeform decals (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/BrushExecutorRouter.cs:262-275`). That mapping is renderer policy, not data. It must move behind a renderer adapter.

Conclusion: the contracts carry both paths if the next step introduces renderer adapters and placement data. They do not carry both paths if current executor outputs remain Tilemap/GameObject/SpriteRenderer.

## 8. Failure Modes

A likely fails by entering an endless asset/density tuning loop: some screenshots look good, then another room reveals tile repetition, soft sprite outlines, square macro silhouettes, or busy combat center noise. It also fails if decorative density remains object-first and authoring becomes slow.

B likely fails by underestimating migration scope. Camera, transparency sorting, billboard readability, 3D lighting, terrain shader, editor undo, and tests all become active at once. A 5-week migration can become 8 weeks with no shippable room if the visual style mismatches PixelLab sprites.

C likely fails if the prototype is too broad or too fake. A generic terrain scene proves nothing; a full Brush V1 port wastes the week. The prototype must answer only the visual/technical unknowns and then stop.

## 9. Final Recommendation

Proceed with **C as a one-week HD-2D proof while continuing A only on renderer-agnostic assets/data**. Do not pivot B until the prototype shows RIMA sprites, RIMA terrain data, biome material swaps, and elevation working in the same scene. Do not continue A as pure Tilemap/decal production without a placement-data/chunk-rendering rule, because that path can improve the current screenshots but will keep fighting the grid ceiling. The decision should be made from a side-by-side prototype, not from preference: if HD-2D produces the natural room look quickly, pivot B; if sprite readability or migration cost breaks, ship A with data-first rendering and accept its 2D ceiling.
