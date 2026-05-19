# Room Composer Library Evaluation

## 1. Ranked Tier List

### MUST STUDY
- Unity 2D Tilemap Extras: custom GridBrush architecture, RuleTile, RuleOverrideTile, WeightedRandomTile, GridInformation. Best direct Unity reference.
- QuickRuleTileEditor: fast RuleTile authoring for 16/15/47 hard-boundary sets. Useful for RIMA's Wang16 authoring, not for natural blending.
- Ogmo Editor 3: Tile/Decal/Entity/Grid layer separation and JSON-first workflow. Strong conceptual fit for RoomData as source of truth.
- MackySoft Choice: clean weighted-selection API and O(1)/O(log n) options. Good for deterministic variant selection after RIMA wraps the RNG.

### USEFUL REFERENCE
- LDtk plus LDtkToUnity: excellent schema/importer study for worlds, levels, IntGrid, AutoLayers, entities, fields, and ScriptedImporter behavior.
- SuperTiled2Unity: practical ScriptedImporter, custom properties, object layers, prefab replacement, Tilemap import. Useful importer reference.
- PathPaintTool: multi-module stroke composition and inner/middle/outer brush idea. Translate concept only.
- Weighted Random Brush gist plus Unity WeightedRandomTile docs: useful for paint overrides, box/flood fill, weighted picks. Gist code quality is not integration quality.

### OPTIONAL
- Unity 2D Tech Demos: useful sample scenes/assets for Tilemap Extras usage, but not architecture.
- Tiled editor core: good external blockout option and TMX/TSX format reference; too broad and GPL-heavy to embed.

### NOT WORTH INTEGRATING
- PVTUT: inspiration only for preview-vs-bake, chunk/page-table thinking, and decal bake concepts. Not a 2D room composer fit.

## 2. Per-repo deep-dive

### 1. Unity 2D Tilemap Extras
- Take: Directly study `Editor/Brushes/RandomBrush/RandomBrush.cs`, `GameObjectBrush.cs`, `GroupBrush.cs`, `LineBrush.cs`, `PrefabBrushes/PrefabRandomBrush.cs`, `Runtime/Tiles/RuleTile/RuleTile.cs`, `RuleOverrideTile.cs`, `WeightedRandomTile.cs`, and `Runtime/GridInformation/GridInformation.cs`.
- Avoid: Do not use GameObject Brush or PrefabRandomBrush for visual decals/scatter. They instantiate scene objects and violate the no GameObject per decal/patch/scatter rule. Do not make RuleTile the source of truth.
- RIMA integration idea: Use Tilemap Extras as an editor/brush foundation, but override paint operations to write RoomData placements, semantic masks, patch data, and dirty chunks. Use RuleTile/RuleOverrideTile only for L3 hard boundaries and authoring workflows.
- Risk: Medium. Unity package is stable, but its defaults assume direct Tilemap/scene edits. RIMA must route edits through RoomData first.
- License + Unity version: Unity Companion License, package `com.unity.2d.tilemap.extras` 2.2.5, Unity 2021.1. Safe in Unity projects, not general-purpose redistributable.

### 2. Unity 2D Tech Demos
- Take: Study `Assets/Tilemap/Brushes/Random Brush`, `Prefab Brush`, `Tint Brush`, `Rule Tiles/Custom Rule Tile`, `Rule Override Tile`, and `Palette Swap`. These show real asset setup around Tilemap Extras.
- Avoid: Do not copy demo scene structure as architecture. Demos are scene/asset examples, not RoomData systems. Avoid runtime destructible Tilemap logic unless a later gameplay feature needs it.
- RIMA integration idea: Use as a visual sanity reference for how artists configure Tile assets, palettes, and RuleTile samples. It can inform sample rooms and editor training docs.
- Risk: Low. Study-only; high risk only if demo patterns become production architecture.
- License + Unity version: MIT license in repo; README says Unity 2021.1.0f1 onward.

### 3. QuickRuleTileEditor
- Take: Study `RuleTilePatternsProvider.cs` for 16/15/47 pattern selection, `PatternedRuleTileEditModel.cs` for copying pattern rules into assets, and `QuickRuleTileWindow*.cs` for UI Toolkit workflow.
- Avoid: Do not expand this into same-elevation natural floor blending. Do not make every floor variation a RuleTile. Do not depend on this for runtime rendering.
- RIMA integration idea: Use or fork-adapt as an authoring helper for cliff, raised platform, water/lava/poison/hazard borders, and hard terrain edges. Generate/validate RIMA Wang16 boundary tile assets from a controlled pattern set.
- Risk: Medium. It depends on Unity Tilemap Extras RuleTile internals like `m_TilingRules`. Good for editor tooling, fragile as core runtime dependency.
- License + Unity version: MIT, package `com.stalengd.quickruletileeditor` 0.3.1, Unity 2021.1, requires 2D Tilemap Extras.

### 4. Weighted Random Brush / Prefab Random Brush / WeightedRandomTile
- Take: Study Unity's `WeightedRandomTile.cs` location-hash stable choice, the gist `PrefabRandomBrush` override of `Paint`, `Erase`, `BoxFill`, `BoxErase`, and `FloodFill`, plus the simple cumulative-weight selector.
- Avoid: Do not copy the gist as-is. It is named `PrefabRandomBrush` but writes tiles, uses Unity global random, lacks deterministic room seed integration, has no undo/data model integration, and flood fill can be expensive without bounds rules.
- RIMA integration idea: Implement RIMA-owned weighted asset selection service: `RoomSeed + layer + cell + palette id + salt` -> deterministic variant. Brushes write weighted placement decisions into RoomData and mark DirtyChunkData.
- Risk: Medium. Easy to implement badly because randomness must be repeatable and not depend on brush stroke order unless intentionally recorded.
- License + Unity version: Unity WeightedRandomTile is Unity Companion License. Gist has no explicit license in fetched content; treat as study only unless permission is clarified. Unity docs fetched for package 1.5 preview, but repo package inspected is 2.2.5.

### 5. Ogmo Editor 3
- Take: Study the layer model: Tile layers, Grid layers, Entity layers, Decal layers. The manual explicitly supports decals placed freely, scaled/rotated/flipped, entities with resize/rotation/nodes, grid metadata, and JSON level output.
- Avoid: Do not import Ogmo as the editor for RIMA. It is engine-agnostic and lacks RIMA-specific budgets, fake-isometric rules, chunked renderer, and RoomData schema. Do not copy its unrestricted JS hooks into Unity editor code.
- RIMA integration idea: Adopt the separation model: base tiles, semantic grid/masks, decals/patches as data placements, entities as gameplay records. Ogmo validates that a data-first room file can be artist-friendly.
- Risk: Low for concept, high for direct import. RIMA needs stricter budgets: 20 percent negative space, 70/20/10 floor, 3 cluster cap, 8 palette/zone, 15 percent path.
- License + Unity version: Site says MIT and free/open source. Engine-agnostic desktop app; no Unity compatibility issue for concept-only use.

### 6. LDtk + LDtkToUnity
- Take: Study LDtk JSON schema fields like `worldLayout`, `levels`, `layerInstances`, `__gridSize`, `intGridCsv`, `autoLayerTiles`, `gridTiles`, `entityInstances`, `fieldInstances`. In LDtkToUnity study `LDtkProjectImporter.cs`, `LDtkBuilderLevel.cs`, `LDtkBuilderEntity.cs`, `LDtkBuilderIntGridValue.cs`, `LDtkBuilderTileset.cs`, and `TilemapTilesBuilder.cs`.
- Avoid: Do not import LDtk worlds directly into runtime RoomData as the main production path. LDtkToUnity creates GameObjects/components for levels, layers, entities, and fields, which conflicts with RoomData source-of-truth if used naively. Do not use AutoLayers as RIMA's natural blending model.
- RIMA integration idea: Optional external blockout importer. Convert LDtk IntGrid/entity fields into RoomData, then discard importer hierarchy. Use its ScriptedImporter/dependency caching approach as reference.
- Risk: Medium. Strong tools, but lock-in and parallel level schema drift are likely if LDtk becomes an authoring source beside Room Composer.
- License + Unity version: LDtk MIT, app package version observed 1.5.3. LDtkToUnity MIT, `com.cammin.ldtkunity` 6.12.3, Unity 2019.3.

### 7. Tiled + SuperTiled2Unity
- Take: From Tiled, study TMX/TSX map/layer/object/property concepts: `map.h`, `tilelayer.h`, `objectgroup.h`, `properties.h`, `tmxmapformat.h`, `wangset.h`. From SuperTiled2Unity, study `SuperImporter.cs`, `TmxAssetImporter.cs`, `TiledAssetImporter.cs`, `SuperObjectLayerLoader.cs`, `SuperCustomProperties.cs`, `SuperObject.cs`, `ST2USettings.cs`, and prefab replacement settings.
- Avoid: Do not embed Tiled editor code. Do not rely on per-object imported GameObjects for decals/scatter. Do not adopt Tiled Wang sets for same-elevation organic floor blending.
- RIMA integration idea: Optional TMX blockout import: tile layers -> RoomData base/hard boundaries, object layers/custom properties -> gameplay entities or markers. SuperTiled2Unity is a reference for robust ScriptedImporter errors, dependency tracking, custom properties, and prefab replacement.
- Risk: Medium. Tiled is powerful but generic. SuperTiled2Unity includes Burst dependency and builds Unity Tilemaps/GameObjects; RIMA would need a narrow one-way converter.
- License + Unity version: Tiled app is GPL; `libtiled` is BSD 2-clause. Do not embed GPL editor code. SuperTiled2Unity is MIT, package 2.4.0, Unity 2020.3.

### 8. PathPaintTool
- Take: Study branch `unity-2019.3`: `PathPaintTool.cs` module registration and paint mode routing, `BrushSettings.cs`, `PathRecorder.cs`, `StrokeSegment.cs`, `StrokePaintMode.cs`, and modules like `PaintModule.cs`, `BridgeModule.cs`, `SmoothModule.cs`.
- Avoid: Do not copy terrain APIs. It is built on `UnityEngine.Experimental.TerrainAPI`, heightmaps, splatmaps, terrain neighbors, and Dynamic Terrain assumptions. Undo is called incomplete in README.
- RIMA integration idea: Translate the concept into semantic 2D brushes: an inner paint channel, middle transition/patch channel, outer decal/scatter/softening channel, applied in a deterministic order. Stroke mode can inspire RimaOrganicBrush and RimaFeatureBrush path painting.
- Risk: Medium-low for concept; high for code reuse. The source branch is old and terrain-specific.
- License + Unity version: Dual MIT plus Unity Companion notice due to TerrainToolSamples base. Branches target Unity 2018.3/2019.2/2019.3.

### 9. PVTUT
- Take: Study README concepts only: editor Dynamic Decals preview disabled at runtime while baked into texture, tile/chunk/page-table/LOD thinking, limited tiles updated per frame, and `PVT_manager_NORMAL.cs` task queue/update throttling.
- Avoid: Do not integrate. It is terrain/3D/VRAM/texture-array focused, includes bundled Dynamic Decals content, depends on runtime baking complexity, and does not match 32 PPU 2D room editing.
- RIMA integration idea: Use as inspiration for RIMA editor preview vs baked chunked visual renderer. Visual-only details can be represented as data and emitted into chunk meshes/tilemaps/material batches, not scene objects.
- Risk: High if copied. The value is architectural analogy, not code.
- License + Unity version: No explicit license file found in repo sample. Treat as not reusable unless license is clarified. Unity version not declared in package metadata.

### 10. MackySoft Choice
- Take: Study `WeightedSelector.cs`, `WeightedSelector.Creation.cs`, `IWeightedSelector.cs`, `LinearWeightedSelectMethod.cs`, `BinaryWeightedSelectMethod.cs`, and `AliasWeightedSelectMethod.cs`. It provides ergonomic `ToWeightedSelector` conversion and multiple algorithms.
- Avoid: Do not call `SelectItemWithUnityRandom()` for RoomData generation. Do not allocate selectors per cell/stroke. Do not expose LINQ-heavy generation in hot render code.
- RIMA integration idea: Either direct dependency for editor-time/authoring weighted choices or copy the small concept into a deterministic RIMA selector. For per-palette variants, prebuild selectors per palette and feed deterministic float values.
- Risk: Low if wrapped. Medium if Unity global random or disposal/temporary array semantics leak into generation code.
- License + Unity version: MIT, package `com.mackysoft.choice` 1.2.3, Unity 2019.4.

## 3. Architecture recommendation

I agree with the expected pattern, with two modifications:

- Unity 2D Tilemap Extras should be the brush/editor foundation, not the data model. RIMA brushes should subclass or mirror GridBrush behavior but write RoomData first.
- QuickRuleTileEditor/RuleTile should be limited to L3 hard feature boundaries. It should not own same-elevation natural floor blending.
- Weighted random logic should be RIMA-owned or wrapped behind a deterministic selector. Choice is cleaner than the gist for selector architecture; Unity WeightedRandomTile is useful for location hashing.
- Ogmo is the best conceptual layer model: Tile, Decal, Entity, Grid maps cleanly to base floor, visual placements, gameplay entities, and semantic masks.
- LDtk/Tiled should remain optional external blockout importers. They should never become parallel authoritative room formats.
- PathPaintTool is a good model for multi-ring semantic brush behavior, but only as a concept.
- PVTUT is only useful as inspiration for editor preview vs runtime bake and chunk invalidation.
- RoomData remains source of truth. Unity scene hierarchy is a generated preview/render target.
- Visual-only details should render through chunked/batched layers: L2b/L4/L5/L6 chunk renderers, Tilemaps, meshes, or material batches. No GameObject per decal/patch/scatter.
- GameObjects are reserved for active gameplay entities, interactables that need components, authoring handles, and editor UI helpers.

## 4. Phase 1.5 implementation outline

### 1.5A Data model
- `RoomData`: authoritative serialized room record. Owns dimensions, seed, zones, base floor ids, hard features, semantic masks, visual placements, entities, budget counters, and dirty chunks.
- `VisualPlacementData`: id, layer, cell/position, sprite or atlas ref, variant id, scale, rotation, flip, color, sort offset, palette id, seed salt. Used for decals, patches, scatter, shadows, glow, polish.
- `SemanticMaskData`: compact grids for path, negative space, floor family, elevation, hazard, hard boundary, cluster id, zone id, painter locks.
- `DirtyChunkData`: chunk coord, changed layer flags, bounds, rebuild reason, revision.
- `DecalPaletteSO`: weighted decal entries, placement constraints, scale/rotation ranges, density limits.
- `ScatterPaletteSO`: weighted scatter entries, footprint, clearance, cluster rules, path exclusion, max per chunk/room.
- `PatchAtlasSO`: macro floor patches with masks, footprints, blend category, anchor rules, allowed floor families.
- `RoomVisualProfileSO`: room/biome style rules, palette caps, readability budgets, renderer layer mapping, deterministic seed salts.

### 1.5B Brushes
- `RimaOrganicBrush`: paints same-elevation natural floors as base floor plus macro patches, organic decals, detail scatter, and optional shadow/glow polish. Enforces 70/20/10 floor budget and path/negative-space constraints.
- `RimaScatterBrush`: places weighted visual-only scatter into RoomData, chunks by spatial buckets, no scene object instantiation.
- `RimaFeatureBrush`: paints hard boundaries and elevation/hazard features. This is the only brush family allowed to invoke Wang16/RuleTile logic.
- `RimaPropClusterBrush`: places curated gameplay/visual prop clusters, enforces 3 cluster cap and 8 palette/zone cap, and distinguishes active entity records from visual placements.

### 1.5C Rendering
- L2 Tilemap base: base floor and broad readable terrain.
- L3 Tilemap/Wang hard boundary: cliffs, raised platforms, water/lava/poison/hazard borders, and hard terrain edges only.
- L2b/L4/L5/L6 chunked visual layer: macro floor patches, decals, detail scatter, shadows, glow, manual polish. Implement chunk rebuild from RoomData dirty flags.
- Use Tilemaps for dense tile-like layers, chunk meshes or batched sprites for free-position visual placements. Do not instantiate per placement.
- Editor preview can create temporary handles/gizmos, but save data must remain RoomData.

### 1.5D Benchmark plan
- Build two identical visual test rooms with 1000 visual placements.
- Case A: 1000 GameObjects with SpriteRenderers/Transforms.
- Case B: 1000 `VisualPlacementData` records rendered through chunked renderer.
- Measure editor rebuild time, play-mode enter time, frame CPU cost, GC alloc, memory, hierarchy usability, selection cost, and chunk rebuild cost after editing 1/10/100 placements.
- Pass condition: chunked renderer preserves visual fidelity while materially reducing hierarchy count, per-frame overhead, and edit-time churn.

## 5. Final recommendation

- Unity 2D Tilemap Extras: direct package dependency if project Unity version supports it; custom RIMA wrappers required.
- Unity 2D Tech Demos: study only.
- QuickRuleTileEditor: fork+adapt or study; use only for hard-boundary RuleTile authoring.
- Weighted Random Brush gist: study only. Unity WeightedRandomTile: study/copy concept under Unity license constraints only if acceptable.
- Ogmo Editor 3: study only; copy layer/data concepts, not tooling.
- LDtk + LDtkToUnity: optional importer reference; do not direct-integrate as core.
- Tiled + SuperTiled2Unity: optional blockout path; use SuperTiled2Unity as importer reference, not runtime dependency.
- PathPaintTool: inspiration only for multi-ring semantic brushes.
- PVTUT: inspiration only; do not integrate.
- MackySoft Choice: direct dependency is acceptable for editor/data generation if wrapped; otherwise copy the selector concept into RIMA deterministic code.

## 6. Confidence + caveats

- HIGH: RoomData must remain source of truth. Every inspected editor/importer that writes scene hierarchy directly would conflict with RIMA unless adapted.
- HIGH: RuleTile/Wang16 belongs only to hard boundaries. The repo evidence supports RuleTile for neighbor-based tiles, but RIMA's natural same-elevation look needs layered data placements instead.
- HIGH: GameObject Brush, PrefabRandomBrush, LDtkToUnity entities/layers, and SuperTiled object imports are anti-patterns for visual-only details at RIMA scale.
- HIGH: Ogmo layer separation is the cleanest conceptual model for Room Composer data.
- MED: Direct dependency on Tilemap Extras is safe if Unity package/version matches the project. The GitHub repo is read-only; prefer Package Manager source.
- MED: Choice is useful, but deterministic RNG and allocation policy must be RIMA-owned.
- MED: LDtk/Tiled external blockout can help early production, but only if import is one-way into RoomData.
- LOW: PVTUT code reuse. License is unclear and domain mismatch is large; use only ideas.
- Caveat: This evaluation sampled READMEs and 3-5 key files per repo as requested, not complete code audits.
