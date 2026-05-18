# Phase A v15c: 8-Layer Painted Top-Down Refactor (Hades + Alabaster Dawn canonical)

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: laurethgame (FRESH %1 post-reset)
Effort: xhigh
Timeout: 7200s
Reference memory: `feedback_layered_terrain_mandatory.md` (8-layer canonical recipe)

## Context

User feedback 2026-05-18 sabah (2 ardışık mesaj):
1. "Playable room mantığında saçma duruyor, en altı kaplayacak bir şey olacak, üstüne bir kaplama daha, demek ki yerde siyah siyah görünmeyecek, onun üstüne diğer kaplamalar olacak"
2. "Hades ve Alabaster Dawn tarzına uygun 6 layerlı 8 layerlı mantıklı boyamayı düşün tekrardan, bu olmadı belli ki temelde bi hata yapıyoruz"
3. "8 layer iyi olsun istiyorum bu sistemi oturtursak bütün mapleri daha rahat çizebileceğim"

**v15b sorunu**: Sadece Layer 6 (medium props) + Layer 7 (tall focal) populated. Layer 1-5 + 8 yok → "skulls floating in dark void" effect. Phase B-3 Blueprint Painter sadece scatter yapıyor, fill yapmıyor.

**Hedef**: Hades + Alabaster Dawn + Octopath stilinde **8-layer painted top-down**. Tüm gelecek map'ler bu sistem üzerine inşa edilecek — bir kez kurulan canonical pipeline.

## 8-Layer canonical recipe

| # | Layer | Coverage | Sorting | Cell alignment |
|---|---|---|---|---|
| **1** | **Macro ambient fill** — painterly room background, big sweeping shapes per zone | %100 | -100 | Cell-aligned (1×1 unit), pivot center |
| **2** | **Base floor tile** — cell-aligned biome tile, asla dark görünmez | %100 | -90 | Cell-aligned (1×1 unit), pivot center |
| **3** | **Mid-tone gradient overlay** — subtle color region patches | %30-50 | -80 | Cell-jittered, sub-cell position OK |
| **4** | **Detail texture** — cracks, mossy patches, dirt stains | %30 | -70 | Cell-jittered, sub-cell position OK |
| **5** | **Small scatter** — pebbles, grass tufts, dried leaves, footprints | %40 | -60 | Free position within cell, density-based |
| **6** | **Medium props** — rocks, bushes, debris piles | %15 | YPosition | Cell-center + jitter, footprint up to 2×2 |
| **7** | **Tall focal** — statues, banners, columns, ritual circles | %5 (cap 1-2/region) | YPosition | Cell-center, footprint 1×1 or 2×2 |
| **8** | **Atmospheric overlay** — god rays, fog, embers, ambient particles | %10-30 | +100 | Cell-aligned with rotation jitter |

## Per-zone layer asset mapping

For data assets only — Codex assigns existing sprites + flags imagegen gaps:

### zone_path
- L1 macro: **IMAGEGEN GAP** (sandy warm-tan macro sweeping shape, 2 variants)
- L2 base: `BiomeFloor_Sandy` (RIMA_v3_Pack)
- L3 midtone: AtmosphericAccents warm-tinted
- L4 detail: Dirt + path-grass transitions
- L5 scatter: pebbles + dried grass tufts
- L6 medium: AssetParts_v2 small debris
- L7 focal: NONE (path no major features, sparse population)
- L8 atmospheric: **IMAGEGEN GAP** (warm dust motes, sand-haze)

### zone_grass
- L1 macro: **IMAGEGEN GAP** (forest-canopy dark green macro)
- L2 base: `BiomeFloor_Mossy`
- L3 midtone: AtmosphericAccents green-tinted
- L4 detail: Moss + mossy-stone transitions
- L5 scatter: grass tufts (Phase B-3 imagegen), weeds, overgrowth
- L6 medium: bushes, AssetParts_v2 medium foliage
- L7 focal: VerticalProps tall trees (rare, 1-2 per region)
- L8 atmospheric: **IMAGEGEN GAP** (green ambient haze, drifting pollen)

### zone_stone
- L1 macro: **IMAGEGEN GAP** (stone-vault grey macro)
- L2 base: `BiomeFloor_Cave`
- L3 midtone: AtmosphericAccents cool-tinted
- L4 detail: Cracks + Pebbles small + grass-stone transitions
- L5 scatter: pebbles, bone fragments, small skulls
- L6 medium: Pebbles big variants, AssetParts_v2 rocks
- L7 focal: Walls fragments, columns
- L8 atmospheric: **IMAGEGEN GAP** (cool ambient mist, dust)

### zone_wall
- L1 macro: **IMAGEGEN GAP** (stone-vault dark variant)
- L2 base: `BiomeFloor_Cave` darker variant (or recolor existing)
- L3 midtone: deep shadow patches
- L4 detail: cracks + grime
- L5 scatter: dust, rubble small
- L6 medium: rubble medium
- L7 focal: Walls (full Wall sprites), VerticalProps columns/arches
- L8 atmospheric: **IMAGEGEN GAP** (deep shadow accents)

### zone_water
- L1 macro: **IMAGEGEN GAP** (water-pool dark teal macro)
- L2 base: water tile variants (pool_water 8 sprites from Phase B-3 imagegen, treat as full-cell candidates)
- L3 midtone: water surface variation
- L4 detail: ripples (from pool_water)
- L5 scatter: reeds (water-grass transitions), wet stones
- L6 medium: larger reeds, water debris
- L7 focal: water shrine or center pool feature (sparse)
- L8 atmospheric: **IMAGEGEN GAP** (water mist, drips)

### zone_feature
- L1 macro: **IMAGEGEN GAP** (blood-altar dark red macro, ritual ambient)
- L2 base: `BiomeFloor_Blood` (RIMA_v3_Pack)
- L3 midtone: red-tint AtmosphericAccents
- L4 detail: blood splatters (Rift assets), ritual marks
- L5 scatter: candles, bone fragments, ritual remnants
- L6 medium: smaller ritual objects, broken altars
- L7 focal: Ritual + Rift complete sets (pentagrams, occult symbols, banners)
- L8 atmospheric: **IMAGEGEN GAP** (blood-mist, ember particles, occult glow)

## Files to MODIFY

### `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs` — MAJOR schema refactor

Replace existing fields with:
```csharp
[Header("Identity")]
public string zoneId;
public string displayName;
public Color brushColor;
public float defaultDensity;  // legacy, used for Layer 5+6 scatter density
public int maxFeatureProps;   // legacy, repurpose as maxTallFocalPerRegion (Layer 7 cap)

[Header("Layer 1 — Macro Ambient Fill (full coverage)")]
public Sprite[] macroFillSprites;  // multi-variant per zone, deterministic pick per cell

[Header("Layer 2 — Base Floor Tile (full coverage)")]
public Sprite[] baseFloorSprites;  // multi-variant per zone

[Header("Layer 3 — Mid-tone Gradient Overlay (sparse 30-50%)")]
public BlueprintPropPoolSO midToneOverlayPool;
[Range(0,1)] public float midToneDensity = 0.4f;

[Header("Layer 4 — Detail Texture (sparse 30%)")]
public BlueprintPropPoolSO detailTexturePool;
[Range(0,1)] public float detailDensity = 0.3f;

[Header("Layer 5 — Small Scatter (40%)")]
public BlueprintPropPoolSO smallScatterPool;
[Range(0,1)] public float smallScatterDensity = 0.4f;

[Header("Layer 6 — Medium Props (15%)")]
public BlueprintPropPoolSO mediumPropPool;
[Range(0,1)] public float mediumPropDensity = 0.15f;

[Header("Layer 7 — Tall Focal (capped per region)")]
public BlueprintPropPoolSO tallFocalPool;
public int maxTallFocalPerRegion = 2;

[Header("Layer 8 — Atmospheric Overlay (10-30%)")]
public BlueprintPropPoolSO atmosphericPool;
[Range(0,1)] public float atmosphericDensity = 0.2f;
```

**Migration**: existing zone_*.asset files MUST be updated. Codex sets old `propPool` field's entries into new `mediumPropPool` (Layer 6) as default migration. User can refine per-zone post-dispatch.

### `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs` — 8-pass refactor

Replace single `PopulateZones` with chained 8-pass:
```csharp
public static int PopulateZones(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed) {
    int total = 0;
    total += PopulateLayer1Macro(canvas, profile, roomRoot, seed);
    total += PopulateLayer2BaseFloor(canvas, profile, roomRoot, seed + 1);
    total += PopulateLayer3MidTone(canvas, profile, roomRoot, seed + 2);
    total += PopulateLayer4Detail(canvas, profile, roomRoot, seed + 3);
    total += PopulateLayer5SmallScatter(canvas, profile, roomRoot, seed + 4);
    total += PopulateLayer6Medium(canvas, profile, roomRoot, seed + 5);
    total += PopulateLayer7TallFocal(canvas, profile, roomRoot, seed + 6);
    total += PopulateLayer8Atmospheric(canvas, profile, roomRoot, seed + 7);
    return total;
}
```

Each `PopulateLayerN` method:
- Iterates `canvas.intentMap` cells
- For each cell, picks zone's layer-N pool/sprite based on `density` (Layer 1+2 always 100%, others density-gated)
- Creates GameObject named `_BlueprintPlaced_L{N}_{zoneId}_{x}_{y}`
- Sets SpriteRenderer sortingOrder per layer (L1=-100, L2=-90, ..., L7=YPos, L8=+100)
- Layer 7 uses region detection (flood-fill contiguous zone region) to cap `maxTallFocalPerRegion`
- Uses `PropPlacementService.PlacePropDefinition` or new `PlaceSprite` helper for Layer 1+2 (no PropDefinitionSO wrapper needed, just sprite)

**New helper** in `PropPlacementService.cs`:
```csharp
public static GameObject PlaceSprite(Sprite sprite, Transform parent, Vector3 worldPos, string name, int sortingOrder) {
    var go = new GameObject(name);
    go.transform.SetParent(parent, false);
    go.transform.position = worldPos;
    var sr = go.AddComponent<SpriteRenderer>();
    sr.sprite = sprite;
    sr.sortingOrder = sortingOrder;
    Undo.RegisterCreatedObjectUndo(go, "Blueprint Layer Place");
    return go;
}
```

### `Assets/Scripts/Rima/MapDesigner/SO/BlueprintProfileSO.cs` — minor update

No schema change needed (zones[] + adjacencyRules[] still). But ensure profile validation: warn if any zone has empty Layer 1 or Layer 2 (mandatory layers).

### Data asset migration

For each existing `Assets/Data/Blueprint/ZoneTypes/zone_*.asset`:
1. Read old `propPool` field
2. Assign to new `mediumPropPool` field (Layer 6 default)
3. Map existing categorized RIMA_v2/v3/GeneratedProps sprites to appropriate layer slots per `Per-zone layer asset mapping` above
4. Leave Layer 1 + Layer 8 fields EMPTY → log `[Blueprint] Zone '{id}' missing Layer 1 macro fill, will use solid fallback color` warning
5. Per-zone layer mapping detailed table provided above — Codex executes mapping

### Window UI update

`BlueprintPainterWindow.cs` — extend Layer Visibility foldout from 6 zones to 8 layers:
- 8 toggle checkboxes (one per layer L1-L8)
- Toggles dim the corresponding layer's placed GameObjects (set `Color.alpha = 0.2f` on SpriteRenderer)
- Default all ON
- Tests: 2 new (`LayerVisibility_L1Off_DimsMacroLayer`, `LayerVisibility_L8Off_DimsAtmospheric`)

## Tests

### `Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulator8LayerTests.cs` — 10 new tests

1. `PopulateLayer1Macro_FullCoverage` — every cell gets Layer 1 sprite
2. `PopulateLayer2BaseFloor_FullCoverage` — every cell gets Layer 2 sprite
3. `PopulateLayer3MidTone_RespectsDensity` — ~40% cells get L3 (within tolerance)
4. `PopulateLayer4Detail_RespectsDensity` — ~30%
5. `PopulateLayer5Scatter_RespectsDensity` — ~40%
6. `PopulateLayer6Medium_RespectsDensity` — ~15%
7. `PopulateLayer7TallFocal_RespectsMaxCap` — feature zone with multiple regions gets ≤2 per region
8. `PopulateLayer8Atmospheric_RespectsDensity` — ~20%
9. `Populate8Layers_SortingOrder_AscendingByLayer` — placed GameObjects' SpriteRenderer.sortingOrder matches layer expectations
10. `Populate8Layers_DeterministicWithSeed` — same seed = same placement across all layers

**Tests preserved**: existing 6+7 AutoPopulatorTests + 6 BlueprintCanvasTests (zone schema migration must not break these — adapter pattern).

## Self-iteration

If first pass has:
- < 364 baseline tests still PASS → fix regression first (especially old AutoPopulatorTests that expect single-pass behavior — adapt assertions to new 8-pass output)
- Any of 10 new tests FAIL → iterate fix locally
- Compile errors → iterate fix
- Asset migration breaks any existing scene (v15/v15b roots in RoomPipelineTest.unity) → handle gracefully, leave deprecated as inactive

Max 3 internal iterations.

## v15c Scene composition

After refactor implementation:
1. Open `Assets/Scenes/Demo/RoomPipelineTest.unity`
   - If unsaved changes warning: **discard** (current dirty scene is untitled, no value loss)
2. Deactivate: `Pro_Redesign_v15b_FullAdjacency_CombatRoom`
3. Create: `Pro_Redesign_v15c_8LayerPainted_CombatRoom` at same position
4. Programmatic Blueprint API run via execute_code:
   - Load `profile_combat_room_default.asset` (updated with new 8-pool zones)
   - Paint same combat-focused canvas (5-zone + small water pocket, identical to v15b)
   - `AutoPopulator.PopulateZones(canvas, profile, v15cRoot, seed: 2026)` → 8-pass execution
   - `AutoPopulator.PopulateAdjacency(canvas, profile, v15cRoot, seed: 2026)`
5. Save scene
6. Screenshot orthographic: `Assets/Screenshots/PlayableRoom_combat_v15c_8layer.png`
7. Asset gap log: list which Layer 1 / Layer 8 fields are empty in DONE marker (for follow-up imagegen)

## Acceptance Criteria

1. BlueprintZoneTypeSO new 8-field schema compiles
2. AutoPopulator 8-pass implemented + 10 new tests PASS
3. Layer Visibility foldout extended to 8 layers
4. 6 zone .asset files migrated to new schema (Layer 6 backfilled from old propPool, others assigned per mapping table)
5. v15c GameObject in RoomPipelineTest.unity with ≥500 Blueprint children (8 layers × ~80 avg cells = ~640 estimate)
6. Layer 1 + Layer 2 = %100 cell coverage (Codex measures: cells_with_L1 / total_painted_cells, cells_with_L2 / total_painted_cells, both should be 1.0 for non-empty pools)
7. Full EditMode: 386+/386+ PASS (376 baseline + 10 new)
8. Console 0 errors (asset gap warnings for L1/L8 empty pools ALLOWED, logged in DONE)
9. Screenshot LIVE
10. DONE marker `STAGING/CODEX_TASK_PHASE_A_v15c_8_LAYER_REFACTOR_DONE.md` with:
    - Files modified count
    - Per-layer placement counts (L1=X, L2=Y, ...)
    - Cell coverage stats
    - Layer 1 + Layer 8 asset gap list per zone (for orchestrator imagegen dispatch)
    - Test count delta
    - Sample screenshot

## Edge cases

- Empty Layer 1 pool (zone has no macroFillSprites) → fallback: skip L1 for that zone, log warning (orchestrator imagegen dispatch fixes)
- Empty Layer 2 pool → CRITICAL: fail dispatch with clear error (Layer 2 mandatory, must exist or use Layer 1 fallback)
- BlueprintProfileSO has zone with unmatched zoneId in profile.zones[] → skip cells gracefully
- v15b scene root persists alongside v15c → preserve (don't delete) for git diff comparison
- Test fixture mocks: AutoPopulator8LayerTests use `new GameObject("TestRoot").transform` + `[TearDown]` Destroy

## Codex implementation notes

- **No new asmdef** — all new files under existing `RIMA.MapDesigner.Editor.asmdef`
- **Migration compatibility** — RoomBlueprintSO (Phase B-4) IntentMap schema unchanged, B-4 save/load still works
- **Sorting layer naming** — use integer sortingOrder (-100 to +100), do NOT add custom Sorting Layers to TagManager (Unity asset, project-level change risky)
- **Cell jitter for L3-5** — `Random.Range(-0.3f, 0.3f)` per cell for organic placement (using zone seed)
- **L7 region detection** — flood-fill BlueprintCanvas to find contiguous regions per zone; per region, randomly select N positions where N = min(maxTallFocalPerRegion, region.cells.Count / 8)
- **L8 atmospheric** — use rotation jitter (Random.Range(0, 360°)) for organic feel

## Verification commands (run in order)

1. `manage_editor` set state to EDITOR
2. `read_console` → expect 0 errors
3. `run_tests` EditMode → 386+ PASS
4. Verify baseline preservation: B-2/B-3/B-4 baselines all PASS
5. Open Blueprint Painter window
6. Verify Layer Visibility shows 8 toggles
7. Run scene composition (programmatic 8-pass)
8. Save scene
9. Screenshot
10. Write DONE marker with asset gap list

## Verdict format

Standard DONE pattern with per-layer placement stats + asset gap list.
