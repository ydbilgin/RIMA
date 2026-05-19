# Phase 1.5 RoomData Architecture Specification
Version: DRAFT-01  |  Date: 2026-05-18  |  Status: Pending Opus approval

Codex eval verdict source: `STAGING/CODEX_DONE_ROOM_COMPOSER_LIBRARY_EVAL.md`
v15d composition budget source: `STAGING/CODEX_TASK_v15d_COMPOSITION_LOCK.md`
Current 8-layer canonical recipe: `STAGING/CODEX_TASK_PHASE_A_v15c_8_LAYER_REFACTOR.md`

---

## Section 1 — RoomData Schema

`RoomData` is the authoritative serialized room record. Unity scene = render target only.
(Adopts Codex eval verdict: "RoomData remains source of truth." — HIGH confidence)

### 1.1 Root fields

| Field | Type | Default | Comment |
|-------|------|---------|---------|
| `schemaVersion` | `int` | `2` | Increment on breaking change |
| `roomId` | `string` | `""` | Stable GUID-based room identity |
| `roomType` | `RIMA.RoomType` | `Combat` | Enum: Spawn, Combat, Elite, Boss, Shrine, Treasure, Corridor |
| `width` | `int` | `36` | Grid columns (current canonical: 36) |
| `height` | `int` | `22` | Grid rows (current canonical: 22) |
| `seed` | `uint` | `0` | Room-level deterministic seed |
| `seedSalt` | `uint` | `0` | Per-session override; 0 = use seed |
| `biomeId` | `string` | `"shattered_keep"` | Maps to RoomVisualProfileSO |
| `formatVersion` | `string` | `"v15d"` | Human-readable generation tag |

### 1.2 Zones

`zones: List<ZoneRecord>`

| Field | Type | Comment |
|-------|------|---------|
| `zoneId` | `int` | Unique per room, matches SemanticMaskData.zoneGrid |
| `semanticType` | `BlueprintZoneType` | Enum: path / grass / stone / wall / water / feature |
| `paintMask` | `bool[]` | Flat array [width×height], true = this zone owns cell |
| `visualProfileRef` | `string` | Asset GUID of RoomVisualProfileSO |
| `budgetSnapshot` | `ZoneBudgetRecord` | Copy of budget at last paint; see 1.5 |

### 1.3 Base floor IDs per cell

`baseFloorGrid: int[]`
- Flat [width×height], value = tile asset index from PatchAtlasSO
- -1 = empty/unassigned
- Written by RimaOrganicBrush and RimaFeatureBrush; read by L2 Tilemap renderer

### 1.4 Hard features

`hardFeatures: List<HardFeatureRecord>`

| Field | Type | Comment |
|-------|------|---------|
| `featureId` | `int` | Unique per room |
| `featureType` | `HardFeatureType` | Enum: Wall, Cliff, Water, Lava, Poison, ElevationRise, ElevationDrop |
| `cells` | `Vector2Int[]` | Cells this feature occupies |
| `wangEdgeFlags` | `byte` | 4-bit NE/NW/SE/SW Wang16 edge bitmask |
| `ruleTileRef` | `string` | Asset GUID of RuleTile SO for L3 rendering |

### 1.5 Semantic masks

`masks: SemanticMaskData`

All fields are flat arrays [width×height] unless noted.

| Field | Type | Comment |
|-------|------|---------|
| `pathMask` | `bool[]` | True = primary walkable path cell (connected, ≥pathMinWidth) |
| `negativeSpaceMask` | `bool[]` | True = intentional empty cell, excluded from L3+ placement |
| `floorFamilyGrid` | `byte[]` | 0=none, 1=stone, 2=grass, 3=sand, 4=water, 5=lava — drives same-elevation blending |
| `elevationGrid` | `sbyte[]` | Relative elevation (-128 to 127), 0 = ground |
| `hazardGrid` | `byte[]` | 0=none, 1=lava, 2=poison, 3=void — drives hazard VFX layer |
| `hardBoundaryMask` | `bool[]` | True = Wang16/RuleTile hard boundary cell |
| `clusterIdGrid` | `int[]` | 0=none; hero cluster id (L4-L7); secondary cluster id (L5) packed in upper byte |
| `zoneIdGrid` | `byte[]` | Zone id per cell; matches ZoneRecord.zoneId |
| `painterLockMask` | `bool[]` | True = cell locked, brush writes rejected |

### 1.6 Visual placements

`visualPlacements: List<VisualPlacementData>`

(Adopts Codex eval verdict: "No GameObject per decal/patch/scatter." — HIGH confidence)

| Field | Type | Comment |
|-------|------|---------|
| `placementId` | `int` | Unique per room, stable across rebuilds |
| `layer` | `RenderLayer` | Enum: L1 / L2b / L3 / L4 / L5 / L6 / L7 / L8 |
| `cellX` | `int` | Grid column |
| `cellY` | `int` | Grid row |
| `localOffset` | `Vector2` | Sub-cell jitter (0,0 = cell center) |
| `spriteRef` | `string` | Asset GUID of sprite or atlas entry |
| `variantId` | `int` | Index into palette variant list |
| `scale` | `Vector2` | (1,1) = native size |
| `rotationDeg` | `float` | Degrees CW |
| `flipX` | `bool` | Horizontal flip |
| `flipY` | `bool` | Vertical flip |
| `paletteId` | `int` | Which DecalPaletteSO or ScatterPaletteSO drove this placement |
| `seedSalt` | `uint` | Per-placement salt for deterministic variant selection |
| `sortOffset` | `float` | Added to Y-sort; negative = behind, positive = in front |
| `colorTint` | `Color32` | (255,255,255,255) = no tint |
| `placementSource` | `PlacementSource` | Enum: Brush / AutoPopulator / Manual |

### 1.7 Entities

`entities: List<EntityRecord>` — Gameplay-significant only. NOT visual placements.

| Field | Type | Comment |
|-------|------|---------|
| `entityId` | `int` | Stable across reloads |
| `entityType` | `string` | "Enemy", "Interactable", "Spawn", "Objective" |
| `prefabRef` | `string` | Asset GUID of prefab |
| `cell` | `Vector2Int` | Spawn cell |
| `factionId` | `int` | 0 = neutral |
| `customFields` | `string` | JSON blob for type-specific data |

### 1.8 Budget counters

`budgetCounters: ZoneBudgetRecord[]` — one per zone.

| Field | Type | Comment |
|-------|------|---------|
| `zoneId` | `int` | Matches ZoneRecord.zoneId |
| `totalCells` | `int` | Painted cells in zone |
| `pathCells` | `int` | Current path cell count |
| `negSpaceCells` | `int` | Current neg-space count |
| `dominantFloorCells` | `int` | L2 dominant floor count |
| `secondaryFloorCells` | `int` | L3 secondary overlay count |
| `accentFloorCells` | `int` | L3 accent count |
| `heroClusterCount` | `int` | Active L4-L7 hero clusters |
| `secondaryClusterCount` | `int` | Active L5 secondary clusters |
| `atmosphericCount` | `int` | L8 placement count |
| `budgetViolations` | `string[]` | Human-readable violation strings; empty = OK |

### 1.9 Dirty chunks

`dirtyChunks: List<DirtyChunkData>`

| Field | Type | Comment |
|-------|------|---------|
| `chunkCoord` | `Vector2Int` | Chunk grid position (chunk = 8×8 cells default) |
| `changedLayerFlags` | `int` | Bitmask: bit N = layer N dirty |
| `bounds` | `RectInt` | Cell-space bounding rect of changed region |
| `rebuildReason` | `RebuildReason` | Enum: BrushStroke / AutoPopulate / AssetChange / Manual |
| `revision` | `uint` | Increments each time this chunk is marked dirty |

---

## Section 2 — Supporting ScriptableObjects

### 2.1 DecalPaletteSO
Lives at: `Assets/Data/Brush/Palettes/Decal/`

| Field | Type | Comment |
|-------|------|---------|
| `paletteId` | `int` | Matches VisualPlacementData.paletteId |
| `biomeTag` | `string` | "shattered_keep", "void", etc. |
| `entries` | `DecalEntry[]` | See below |
| `maxDensityPerChunk` | `int` | Hard cap per 8×8 chunk |
| `excludePathCells` | `bool` | True = no decals on pathMask cells |
| `excludeNegSpaceCells` | `bool` | True = no decals on negativeSpaceMask cells |
| `minClearance` | `int` | Cells; minimum distance between two decals from same palette |

`DecalEntry`: spriteRef, weight, allowedFloorFamilies[], scaleRange, rotationRange, allowFlipX

### 2.2 ScatterPaletteSO
Lives at: `Assets/Data/Brush/Palettes/Scatter/`

| Field | Type | Comment |
|-------|------|---------|
| `paletteId` | `int` | |
| `biomeTag` | `string` | |
| `entries` | `ScatterEntry[]` | |
| `maxPerChunk` | `int` | Hard cap per 8×8 chunk |
| `maxPerRoom` | `int` | Global hard cap |
| `clusterRadius` | `int` | Cells; scatter groups within this radius count as one cluster |
| `pathExclusion` | `bool` | True = banned from pathMask cells |

`ScatterEntry`: spriteRef, weight, footprintCells, clearanceCells, allowedSemanticTypes[]

### 2.3 PatchAtlasSO
Lives at: `Assets/Data/Brush/Palettes/Patch/`
Maps to 8-layer recipe: L1 macro fill + L2 base tiles + L3 midtone overlay.

| Field | Type | Comment |
|-------|------|---------|
| `atlasId` | `int` | |
| `biomeTag` | `string` | |
| `entries` | `PatchEntry[]` | |

`PatchEntry`: spriteRef, layer (L1/L2/L3), footprintCells, blendCategory, anchorRule, allowedFloorFamilies[], weight, maskRef

### 2.4 RoomVisualProfileSO
Lives at: `Assets/Data/Brush/Profiles/`
One per biome × room-type combination.

| Field | Type | Comment |
|-------|------|---------|
| `profileId` | `string` | Stable GUID-based identity |
| `biomeId` | `string` | |
| `roomType` | `RIMA.RoomType` | |
| `decalPaletteRef` | `string` | Asset GUID of DecalPaletteSO |
| `scatterPaletteRef` | `string` | Asset GUID of ScatterPaletteSO |
| `patchAtlasRef` | `string` | Asset GUID of PatchAtlasSO |
| `negativeSpaceRatioTarget` | `float` | 0.20 |
| `floorWeights` | `Vector3` | (dominant, secondary, accent) |
| `heroPropClusterCap` | `int` | 3 |
| `secondaryClusterCap` | `int` | 4 (v15e-B) |
| `atmosphericCap` | `int` | 10 (v15e-A) |
| `paletteCapPerZone` | `int` | 8 max palettes per zone |
| `layerRendererMap` | `RendererEntry[]` | Maps RenderLayer → renderer type |
| `deterministicSeedSalts` | `uint[]` | One per RenderLayer (8 entries) |

---

## Section 3 — 4 RIMA Brushes

### 3.1 RimaOrganicBrush
Input: designer paints semantic zone type (path/grass/stone/water) over cells.
Output: sets zoneIdGrid, baseFloorGrid (PatchAtlasSO L2), appends VisualPlacementData for L1/L3/L4/L5, updates pathMask + negativeSpaceMask, marks dirtyChunks.
Enforcement: budget violations WARN (don't block).

### 3.2 RimaScatterBrush
Input: designer selects ScatterPaletteSO + paints radius.
Output: appends VisualPlacementData entries for L5 scatter, NO scene GameObjects, spatial bucketing into clusterRadius chunks, deterministic seedSalt per placement.
Enforcement: maxPerChunk/maxPerRoom exceeded = STOP. pathExclusion = silent skip.

### 3.3 RimaFeatureBrush
Input: designer paints hard boundary type (wall/cliff/water edge/hazard).
Output: appends HardFeatureRecord with Wang16 wangEdgeFlags, sets hardBoundaryMask + elevationGrid + hazardGrid, marks L3 Tilemap dirty, NO VisualPlacementData (RuleTile renders).
Enforcement: Wang16 edge conflict = BLOCKED. Entity overlap = WARN.

### 3.4 RimaPropClusterBrush
Input: designer places curated prop cluster anchor on a cell.
Output: computes cluster footprint (2-5 props, buffer=heroPropClusterBuffer), appends VisualPlacementData for L6/L7 cluster items, appends EntityRecord if interactable, updates clusterIdGrid, increments heroClusterCount.
Enforcement: heroClusterCount >= heroPropClusterCap = STOP. Cluster buffer overlap = STOP. pathProtect path cell = WARN.

---

## Section 4 — Rendering Pipeline

(Adopts Codex eval: "Visual-only details should render through chunked/batched layers. No GameObject per decal/patch/scatter." HIGH confidence.)
(Adopts Codex eval: Ogmo layer model as conceptual reference; PVTUT chunk/dirty-rebuild analogy.)

### Layer stack (bottom to top)

| Layer | Renderer | Source | Notes |
|-------|----------|--------|-------|
| L1 macro fill | Chunked batch (ChunkMesh or BatchedSprites) | RoomData VisualPlacementData | Per-zone macro shape; no Tilemap |
| L2 base floor | Unity Tilemap | RoomData baseFloorGrid | Cell-aligned, 100% coverage |
| L2b midtone overlay | Chunked batch | RoomData VisualPlacementData | Sub-cell jitter OK; 30-50% coverage |
| L3 Wang hard boundary | Unity Tilemap + RuleTile | RoomData hardFeatures | ONLY layer using Wang16/RuleTile |
| L4 detail patches | Chunked batch | RoomData VisualPlacementData | Cracks/moss/stains; 30% coverage |
| L5 scatter | Chunked batch | RoomData VisualPlacementData | Pebbles/tufts; free position; density-capped |
| L6 medium props | Chunked batch | RoomData VisualPlacementData | Y-sorted; footprint up to 2×2 |
| L7 tall focal | Chunked batch | RoomData VisualPlacementData | Y-sorted; 1-2 per region; rare |
| L8 atmospheric | SpriteRenderer (sparse) or Chunked batch | RoomData VisualPlacementData | God rays/fog/embers; capped at 10/zone |

### Chunk rebuild contract
- Chunk size: 8×8 cells (configurable per profile)
- Trigger: any DirtyChunkData entry with matching chunkCoord
- Rebuild scope: only layers in changedLayerFlags bitmask — not full room
- L2/L3 Tilemap: Tilemap.SetTilesBlock batch API, not per-cell SetTile
- ChunkMesh/BatchedSprites: reconstruct from filtered RoomData VisualPlacementData
- After rebuild: remove chunk from dirtyChunks, increment revision

### GameObject reservation policy
GameObjects allowed for:
1. Active gameplay entities (enemies, interactables) — driven by EntityRecord
2. Authoring handles (editor-only, stripped at runtime)
3. Camera + lights
4. Any component needing Unity lifecycle (Coroutine, Collider, Animator)

GameObjects BANNED for: individual decals/scatter/floor patches/shadow overlays/atmospheric overlays.

---

## Section 5 — Migration Path from v15d

### 5.1 What stays
- `BlueprintZoneTypeSO`: all v15d composition budget fields unchanged
- `AutoPopulator` two-pass logic (Reserve → Place) remains as internal implementation
- Existing zone .asset files keep working — no field removal
- All 403 EditMode tests must remain green through migration

### 5.2 What gets refactored
| Current | Phase 1.5 replacement | Notes |
|---------|----------------------|-------|
| `AutoPopulator.PopulatePoolLayer` | `VisualPlacementBuilder` — writes to RoomData.visualPlacements | RoomData becomes output |
| `RimaV15cSceneComposer` | `RoomDataRenderer` — reads RoomData, drives Tilemap + chunk layers | Composer becomes thin orchestrator |
| `PropPlacementService` | Folds into `VisualPlacementBuilder`; cluster logic promoted to `RimaPropClusterBrush` | |

### 5.3 What gets added
- `RoomData` runtime + serialization class
- Visual/Mask/Dirty/Budget/HardFeature/Entity records
- 4 SO (Decal/Scatter/Patch/RoomVisualProfile)
- 4 RIMA brushes (Organic/Scatter/Feature/PropCluster)
- Chunk renderer (ChunkMesh OR BatchedSprites — TBD in 1.5C)
- Deterministic selector (MackySoft Choice wrapped OR RIMA-owned — TBD)

### 5.4 What gets removed
Nothing. Removal pass deferred to post-1.5D benchmark sign-off.

### 5.5 Stepwise phases
- **1.5A Data**: Structs + RoomData class. Dual-write mode (AutoPopulator writes both scene + RoomData). ~20 new EditMode tests.
- **1.5B Brushes**: 4 RIMA brushes write RoomData. SOs created with ShatteredKeep entries. AutoPopulator scene writes deprecated.
- **1.5C Renderer**: Chunked visual layer renderer. L2/L3 Tilemap from baseFloorGrid/hardFeatures. L2b/L4/L5/L6/L7/L8 from VisualPlacementData via chunk rebuild. AutoPopulator dual-write removed.
- **1.5D Benchmark**: 1000 placements GameObject vs ChunkMesh comparison. Measure: rebuild time, play-mode enter, frame CPU, GC alloc, hierarchy, rebuild cost.

---

## Section 6 — Risks + Open Questions

### Risks
1. **Dual-write phase complexity (1.5A)**: AutoPopulator dual-writing increases state-sync surface. Mitigation: time-box to 1.5A.
2. **Chunked renderer perf**: Chunk rebuild for 1000 placements may spike. Mitigation: tune chunk size + dirty-only rebuild.
3. **RuleTile/Wang16 depends on Tilemap Extras version**: QuickRuleTileEditor uses internals. Mitigation: pin com.unity.2d.tilemap.extras 2.2.5+.
4. **Deterministic variant selection**: Stroke-order vs cell-position selection. Mitigation: use cell-position + seed + salt, not stroke sequence.
5. **Migration parity**: ChunkMesh visual must match existing v15d screenshots. Mitigation: keep reference screenshot test as acceptance gate.

### Open Questions for Codex/Opus before 1.5A
1. **Chunk renderer strategy**: ChunkMesh (single draw call) vs BatchedSprites (Unity SpriteBatch)? Affects RendererType enum + 1.5C scope.
2. **RoomData serialization format**: Unity `[Serializable]` SO vs JSON ScriptedImporter vs binary? JSON inspectable; SO Unity-native.
3. **Deterministic selector**: MackySoft Choice direct dep vs RIMA-owned `RimaWeightedSelector`? Eval recommends "wrapped".
4. **Wang16 edge owner**: RimaFeatureBrush computes at paint vs separate WangResolver re-compute? Affects dirty-chunk scoping.
5. **Entity vs VisualPlacement for L6/L7 props**: Shrine fragments need Collider — EntityRecord with prefab OR VisualPlacementData with deferred flag? Establish before 1.5A.
