# RIMA Fresh Asset Pack Plan -- Karar #150 LIVE Act 1 Skeleton

**Date:** 2026-05-19 S94 LATE NIGHT
**Author:** rima-asset (Sonnet 4.6)
**Status:** PLAN ONLY -- no gen dispatched. User approval required before any batch.

---

## 1. PixelLab Account Inventory Summary

| Category | Count |
|---|---|
| Total objects | 147 |
| Total topdown_tilesets (Wang) | 25 |
| Total tiles_pro | 52 |
| Total isometric_tiles | 4 |
| Total map_objects (list_map_objects not exposed in MCP) | 0 confirmed |
| **Current balance** | **3,200 / 5,000 subscription generations** ($0.00 USD credits) |

**Critical note:** 1,800 generations have been spent across all prior work. The "reserve 3,500/5,000" figure in Karar #150 spec is stale -- actual remaining is 3,200.

### Reusable for Karar #150 Act 1 (PASS filter): 29 assets

Filter criteria applied: cool granite palette (#3A3D42 range), dungeon-inside vocabulary, PURE top-down floor tiles, fake-iso wall depth, rift accent, scatter debris.

| Asset ID | Type | Tag / Description | Karar #150 Layer | Compatibility |
|---|---|---|---|---|
| **FLOOR -- tiles_pro (L1 + L2 candidates)** | | | | |
| a9b580a3 | tiles_pro | Cool weathered granite floor (square_topdown 64px 16var) | L1 Floor base | HIGH -- cool granite matches #3A3D42 |
| 78c971fc | tiles_pro | Cool weathered granite floor (square_topdown 64px 16var) | L1 Floor base | HIGH -- same batch variant |
| 295d585a | tiles_pro | Cool weathered granite floor (square_topdown 64px 16var) | L1 Floor base | HIGH -- same batch variant |
| 14486e25 | tiles_pro | Cool weathered granite floor (square_topdown 32px 16var) | L1 Floor base | HIGH -- 32px canonical size |
| bd85039a | tiles_pro | Cool weathered granite floor (square_topdown 32px 16var) | L1 Floor base | HIGH -- 32px canonical size |
| 08e9754d | tiles_pro | Cool weathered granite floor (square_topdown 64px 16var) | L1 Floor base | HIGH |
| e195a7aa | tiles_pro | Cool grey granite stone surface (square_topdown 32px 16var) | L1 Floor base | HIGH |
| 2862f47a | tiles_pro | Cool grey granite floor (square_topdown 32px 16var) | L1 Floor base | HIGH |
| 4ef03888 | tiles_pro | Dark dungeon stone floor, cracked (square_topdown 64px 16var) | L1 / L2 | MEDIUM -- darker, good L2 worn variant |
| f68f7389 | tiles_pro | Worn polished stone walkway (square_topdown 32px 16var) | L2 Floor variation | HIGH -- worn path matches L2 spec |
| 269e0a00 | tiles_pro | Top-down weathered ancient stone floor (square_topdown 32px 16var) | L1 / L2 | HIGH |
| 51845163 | tiles_pro | Top-down ancient dungeon floor (square_topdown 32px 16var) | L1 | HIGH |
| 0ba34423 | tiles_pro | Top-down ancient ruined dungeon floor (square_topdown 32px 16var) | L1 / L2 | HIGH |
| 475ebff7 | tiles_pro | Top-down dark stone dungeon floor (square_topdown 32px 16var) | L2 | MEDIUM |
| e9fb0d5f | tiles_pro | Dark rubble stone floor variation (square_topdown 32px 16var) | L2 | MEDIUM |
| **FLOOR -- topdown_tilesets (Wang, chainable)** | | | | |
| 62d91ed9 | topdown_tileset | Cool weathered granite floor, chunky style | L1 Floor base | HIGH |
| 88fbb4e7 | topdown_tileset | Flat cool grey granite stone floor | L1 Floor base | HIGH |
| f6c16987 | topdown_tileset | Flat cool grey granite stone floor | L1 Floor base | HIGH |
| 2f886879 | topdown_tileset | Flat cool grey granite stone floor | L1 Floor base | HIGH |
| ff9f5489 | topdown_tileset | Flat cool grey granite stone floor | L1 Floor base | HIGH |
| 7b34aa6b | topdown_tileset | Fractured stone keep floor tiles | L1 Floor base | HIGH -- "keep" vocabulary match |
| 04633962 | topdown_tileset | Dark rubble stone floor for shattered keep | L2 Floor variation | HIGH -- "shattered keep" exact match |
| 9591f35a | topdown_tileset | Dark rubble stone floor for shattered keep | L2 Floor variation | HIGH |
| 49913501 | topdown_tileset | Dark rubble stone floor for shattered keep | L2 Floor variation | HIGH |
| 9ffbb4d1 | topdown_tileset | Dark rubble stone floor for shattered keep | L2 Floor variation | HIGH |
| **SCATTER -- objects** | | | | |
| 0b239a15 | object | Broken stone rubble piece (32x32, tag: scatter_rubble_f1) | L5 Scatter | HIGH -- direct Karar #150 L5 spec |
| 5cdd41f6 | object | Broken stone rubble piece (32x32, tag: scatter_rubble_f1) | L5 Scatter | HIGH |
| 4e947c06 | object | Broken stone rubble piece (32x32, tag: scatter_rubble_f1) | L5 Scatter | HIGH |
| 79c3845c | object | Broken stone rubble piece (32x32, tag: scatter_rubble_f1) | L5 Scatter | HIGH |
| 361267b0 | object | Irregular stone cluster (48x48, tag: scatter_stones_f1) | L5 Scatter / _Universal | HIGH -- neutral gray, tintable |
| 488fb7dd | object | Irregular stone cluster (48x48, tag: scatter_stones_f1) | L5 Scatter / _Universal | HIGH |
| 80c1ac28 | object | Irregular stone cluster (48x48, tag: scatter_stones_f1) | L5 Scatter / _Universal | HIGH |
| 1e23fcc2 | object | Irregular stone cluster (48x48, tag: scatter_stones_f1) | L5 Scatter / _Universal | HIGH |

**Reusable total: 33 assets** (15 tiles_pro + 9 topdown_tilesets + 4 rubble scatter + 4 stone cluster + 1 rift placeholder pending review)

### L6 accent -- conditional reusable

| Asset ID | Type | Description | Status |
|---|---|---|---|
| c2b48c99 | object | Cyan-violet rift crack (32x32, review:awaiting-selection) | PENDING -- needs select_object_frames before usable |
| f2ba1bed | object | Hairline rift fracture crack (64x64, completed) | MEDIUM -- check if cyan accent tone matches #00FFCC |

### Legacy / incompatible (skip)

- All `keep_wall_v2` objects (76693f8f, 825ddbdd, f053b5f0, 06338801): warm warm grey, "dark warm grey" description -- Karar #150 spec says warm brown BANNED. CHECK actual image before discarding.
- All `alabaster_decal` / `alabaster_wall` objects (tag: alabaster_*): Alabaster Dawn material, NOT Shattered Keep granite
- All skill icons (tiles_pro isometric 32px 1var, objects tag:dark_fantasy): HUD scope, out of environment scope
- All weapons (tag: weapon_*): character scope
- All full-scene concept images (256x256, "Full 16:9 pixel art gameplay"): concept reference only, not production sprites
- `scatter_dirt_f1` objects: warm brown dirt -- incompatible with cool granite palette
- `scatter_moss_f1` objects (5c474658, 8cd4557a, etc.): CONDITIONAL -- generic moss may work for L4 cave moss if palette is neutral. Flag for visual QC.
- Isometric tiles (6e921ad8, 06998d6f, etc.): 4 granite floor isometric tiles -- ARCHITECTURE MISMATCH (Karar #150 uses fake-iso wall depth via objects, not isometric tile endpoint)
- Warm palette tilesets (4235c9c1, aa7ca5bb, cc1d7d6f, 0a361fb8, ea19bab2, a1b63282): warm brown / pink -- incompatible
- `keep_decal_v2` objects (dabbb30f through 21406da0, 8 items): "dark dungeon floor decal" -- POTENTIALLY REUSABLE for L5 scatter. Visual QC needed before confirm.

---

## 2. Reusable Assets -- Download List

Priority download list for Unity import into `Assets/Art/AssetPacks/Act1_ShatteredKeep/`:

### L1 Floor Base (use 32px variants -- canonical PPU=64, 32px tile)

| Asset ID | Type | Description | Target Path |
|---|---|---|---|
| 14486e25 | tiles_pro | Cool weathered granite floor 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/granite_base_v1/ |
| bd85039a | tiles_pro | Cool weathered granite floor 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/granite_base_v2/ |
| e195a7aa | tiles_pro | Cool grey granite stone 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/granite_base_v3/ |
| 2862f47a | tiles_pro | Cool grey granite floor 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/granite_base_v4/ |
| 269e0a00 | tiles_pro | Weathered ancient stone 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/granite_ancient_v1/ |
| 51845163 | tiles_pro | Ancient dungeon floor 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/granite_ancient_v2/ |
| 62d91ed9 | topdown_tileset | Cool weathered granite (Wang, chainable) | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/wang_granite_v1/ |
| 7b34aa6b | topdown_tileset | Fractured stone keep floor (Wang) | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/wang_keep_fractured/ |

### L2 Floor Variation

| Asset ID | Type | Description | Target Path |
|---|---|---|---|
| f68f7389 | tiles_pro | Worn polished stone walkway 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/worn_path_v1/ |
| 0ba34423 | tiles_pro | Ancient ruined dungeon floor 32px 16var | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/worn_path_v2/ |
| 04633962 | topdown_tileset | Dark rubble stone floor - shattered keep (Wang) | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/wang_rubble_v1/ |
| 9591f35a | topdown_tileset | Dark rubble stone floor - shattered keep (Wang) | Assets/Art/AssetPacks/Act1_ShatteredKeep/floor/wang_rubble_v2/ |

### L5 Scatter (_Universal eligible)

| Asset ID | Type | Description | Target Path |
|---|---|---|---|
| 361267b0 | object | Irregular stone cluster 48x48 | Assets/Art/AssetPacks/_Universal/scatter/stones/ |
| 488fb7dd | object | Irregular stone cluster 48x48 | Assets/Art/AssetPacks/_Universal/scatter/stones/ |
| 80c1ac28 | object | Irregular stone cluster 48x48 | Assets/Art/AssetPacks/_Universal/scatter/stones/ |
| 1e23fcc2 | object | Irregular stone cluster 48x48 | Assets/Art/AssetPacks/_Universal/scatter/stones/ |
| 0b239a15 | object | Broken stone rubble 32x32 | Assets/Art/AssetPacks/Act1_ShatteredKeep/scatter/rubble/ |
| 5cdd41f6 | object | Broken stone rubble 32x32 | Assets/Art/AssetPacks/Act1_ShatteredKeep/scatter/rubble/ |
| 4e947c06 | object | Broken stone rubble 32x32 | Assets/Art/AssetPacks/Act1_ShatteredKeep/scatter/rubble/ |
| 79c3845c | object | Broken stone rubble 32x32 | Assets/Art/AssetPacks/Act1_ShatteredKeep/scatter/rubble/ |

### L6 Accent -- review pending

| Asset ID | Type | Status | Action |
|---|---|---|---|
| c2b48c99 | object | review:awaiting-selection | Run select_object_frames after user visual review |
| f2ba1bed | object | completed | Download if cyan tone confirmed (#00FFCC range) |

---

## 3. Fresh Gen Dispatch -- Batches

### CRITICAL GAP CONFIRMED: L3 Wall classes -- ZERO reusable

The `keep_wall_v2` objects (4 items, 32x32, tag: keep_wall_v2) describe "dark warm grey weathered stone" -- potentially warm-toned wall caps, NOT fake-isometric depth sprites (top cap + front face + base shadow). They are 32x32 (too small for the 5-class wall system at 128x128 canvas) and single-direction. All 24 L3 wall sprites must be freshly generated.

**L3 wall gap: 8 classes x 3 variants = 24 sprites -- all fresh gen required.**

Also missing (confirmed zero): L4 large patches (cave moss, dust drift, cracked rubble), L6 hero brazier/candle/banner.

---

### Batch 1 -- L3 Wall Classes Faz 1 Skeleton (create_object, primary gap)

**Endpoint:** `create_object`
**Direction:** 1
**n_frames:** 16 (review pipeline -- select best 3 variants per class)
**View:** `high top-down`
**Size:** 128x128

Generate 8 dispatch calls (one per wall class). Each call = 16 candidates, select 3 best = 3 variants.

#### Wall class 1: Top-hero wall (top cap face, player-facing, full-height)

`Pixel art 128x128, high top-down view, single cool granite stone wall block, fake-isometric dungeon wall with visible top cap face (lighter #4E5260 granite), front face (mid-tone #3A3D42), base shadow gradient into floor, rectangular stacked stone block pattern, weathered mortar lines, muted desaturated palette, weathered field-worn, cool shadow #252830 at base, no background, transparent bg, isolated sprite. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, bright green, cartoon outline, grid frame, sidescroller, warm palette, border frame, isometric diamond, flat no-depth`

#### Wall class 2: Bottom-hero wall (base stub, low height, player-facing front face primary)

`Pixel art 128x128, high top-down view, low granite stone wall base stub, fake-isometric dungeon wall base section with narrow top cap face, thick front face showing #3A3D42 cool granite blocks, base shadow anchor into floor, shorter height than full wall, weathered stone mortar, muted desaturated palette, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, full-height wall, bright green, grid frame, flat no-depth, cartoon outline, border frame`

#### Wall class 3: Side-left wall (left edge facing right)

`Pixel art 128x128, high top-down view, left-side facing granite dungeon wall segment, fake-isometric side wall with top cap face visible, left-facing front face showing stacked cool granite blocks #3A3D42, base shadow gradient, vertical orientation, muted desaturated palette, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, bright green, flat no-depth, full-height horizontal wall, border frame`

#### Wall class 4: Side-right wall (right edge facing left)

`Pixel art 128x128, high top-down view, right-side facing granite dungeon wall segment, fake-isometric side wall with top cap face visible from right angle, front face cool granite blocks #3A3D42, base shadow gradient, vertical orientation, muted desaturated palette, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, bright green, flat no-depth, horizontal wall, border frame`

#### Wall class 5: Corner wall (inner L-corner, two faces visible)

`Pixel art 128x128, high top-down view, inner L-corner granite dungeon wall, fake-isometric two-face corner with top cap plane visible, front face A (#3A3D42 cool granite blocks), front face B perpendicular, base shadow spreading from both faces, sharp 90-degree interior corner geometry, muted desaturated palette, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, single-face wall, bright green, flat no-depth, border frame`

#### Wall class 6: Granite arch (archway connector, interior portal)

`Pixel art 128x128, high top-down view, granite stone archway portal, fake-isometric dungeon arch with thick stone voussoir blocks, curved opening interior, top cap visible on arch crown, cyan glow accent #00FFCC radiating from inside the arch opening, muted desaturated granite palette #3A3D42, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, no arch opening, flat no-depth, bright unlit stone, border frame`

#### Wall class 7: Rectangular granite pillar (free-standing column)

`Pixel art 128x128, high top-down view, free-standing rectangular granite dungeon pillar column, fake-isometric depth with square top cap plane #4E5260, four front faces visible (two main faces), stacked cool granite blocks #3A3D42, base shadow gradient radiating around pillar base, chain motif hanging element suggestion, muted desaturated palette, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, cylindrical, round pillar, flat no-depth, bright stone, border frame`

#### Wall class 8: Collapsed wall stub (partial broken wall section)

`Pixel art 128x128, high top-down view, collapsed broken granite wall stub, fake-isometric partial wall section with fractured top edge, rubble chunks scattered at base, exposed broken stone cross-section showing granite interior, cool grey #3A3D42 broken stone, debris pile at base, muted desaturated palette, weathered field-worn, isolated sprite transparent bg. MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. Negative Prompt: warm brown, intact wall, clean edge, flat no-depth, border frame`

**Batch 1 estimated cost:** 8 calls x ~3 gens each (with review pipeline) = ~24 generations for selected 24 sprites. Raw call cost: 8 x 16 candidates = 128 candidate gens. Select 3/16 per class = 24 final sprites.

---

### Batch 2 -- L1+L2 Floor (create_tiles_pro, supplemental only if reusables fail QC)

**Assessment:** The account already has 8+ confirmed cool granite tiles_pro at 32px and multiple Wang tilesets. Batch 2 is LOW PRIORITY -- attempt reusable download first. Only gen if visual QC reveals palette mismatch.

**IF needed**, use 4-type batch pattern:

**Endpoint:** `create_tiles_pro`
**tile_type:** `square_topdown`
**tile_size:** 32
**tile_view:** `top-down` (PURE -- NOT low/high)
**tile_depth_ratio:** 0
**outline_mode:** `segmentation`

Numbered prompt:
`1) cool granite dungeon floor surface, muted grey #3A3D42 palette, weathered mortar joints between rectangular stone blocks, chipped edges, cool shadow tones, field-worn 2) worn stone walkway path, polished centre lane, lighter grey #4E5260 worn track, subtle directional scuff marks, same cool granite palette 3) cracked fractured granite floor, hairline crack network spreading across stone surface, cool grey palette, slightly darker fracture shadows 4) mossy granite floor, sparse cool grey-green organic moss growth in mortar joints, same grey granite base, field-worn. ABSOLUTELY NO BORDERS. No black frame around any tile. Seamless edges.`

**Batch 2 estimated cost:** 1 dispatch x 25 gens = 25 generations (if needed).

---

### Batch 3 -- L4 Large Patches (create_object, review pipeline)

**Endpoint:** `create_object`
**Direction:** 1
**n_frames:** 16
**View:** `high top-down`
**Size:** 128x128 (large soft-edge patch)

#### Patch A: Cave moss cluster

`Pixel art 128x128, high top-down view, large organic cave moss patch spread on stone dungeon floor, soft irregular edge blending into transparent background, cool grey-green moss tone on cool granite surface, wet sheen suggestion, dense centre fading to sparse edge, painterly soft brush texture, no hard outline, muted desaturated palette, weathered field-worn, transparent bg isolated. Negative Prompt: bright green, cartoon outline, warm tone, flat uniform patch, border frame`

#### Patch B: Dust drift / fine debris drift

`Pixel art 128x128, high top-down view, fine stone dust drift patch on dungeon floor surface, soft-edged irregular shape, cool pale grey dust accumulation in floor cracks and along wall base, subtle directional drift pattern suggesting air current, transparent bg soft edge, muted desaturated palette, field-worn. Negative Prompt: bright colour, warm tone, cartoon outline, uniform rectangle, border frame`

#### Patch C: Cracked rubble pile patch

`Pixel art 128x128, high top-down view, loose cracked rubble spread patch on dungeon floor, irregular cluster of granite fragment chips and crushed stone, cool grey #3A3D42 fragments, transparent bg, larger pieces at centre smaller at edges, muted desaturated palette, weathered field-worn dungeon debris. Negative Prompt: warm brown, bright green, cartoon outline, border frame, uniform pattern`

**Batch 3 estimated cost:** 3 calls x ~3 gens selected = ~9 generations. Raw: 3 x 16 = 48 candidates.

---

### Batch 4 -- L6 Hero Accent (create_object)

**Endpoint:** `create_object`
**Direction:** 1
**n_frames:** 16 (review pipeline)
**View:** `high top-down`
**Size:** 128x128

#### Accent A: Cyan rift fracture (L6 hero)

`Pixel art 128x128, high top-down view, glowing cyan rift fracture crack in dungeon stone floor, hero-level environmental accent, branching fracture line with inner glow emission colour #00FFCC cyan, fracture edges dark granite #252830, subtle cyan light halo radiating outward on floor surface, fracture width varies thick to thin, isolated sprite transparent bg, muted desaturated palette around the glow accent. Negative Prompt: warm colour, no glow, flat crack, violet rift, border frame`

#### Accent B: Iron brazier with candle flame (point light source)

`Pixel art 128x128, high top-down view, iron wall bracket brazier with small candle flame, fake-isometric wall-mounted light source, iron mounting bracket attached to granite wall face, small orange flame #C4682A torch tip, warm spot-light glow puddle on surrounding floor fading to dark, isolated sprite transparent bg, muted desaturated iron palette, weathered field-worn. Negative Prompt: floating unattached, no glow, bright warm flood, cartoon outline, border frame`

#### Accent C: Hanging stone banner (chain motif)

`Pixel art 128x128, high top-down view, weathered stone-grey hanging banner with chain suspension, dungeon keep banner hanging from wall, faded heraldic mark barely visible on worn banner fabric, iron chain links at top, muted desaturated palette, weathered field-worn fabric, isolated sprite transparent bg. Negative Prompt: bright colour, cartoon outline, clean new banner, warm palette, border frame`

**Batch 4 estimated cost:** 3 calls x ~3 gens selected = ~9 generations. Raw: 3 x 16 = 48 candidates.

---

## 4. Credit Budget Breakdown

| Item | Gens used | Notes |
|---|---|---|
| Reusable download | 0 | Already generated, no additional cost |
| Batch 1 L3 walls (8 calls, 16 candidates each, select 3) | ~128 raw candidates | Budget for 50% regen failures: +32 = 160 budget |
| Batch 2 L1+L2 floor (conditional, 1 call) | 25 if needed | Skip if reusables pass QC |
| Batch 3 L4 patches (3 calls x 16) | ~48 raw candidates | Budget +16 regen = 64 budget |
| Batch 4 L6 accent (3 calls x 16) | ~48 raw candidates | Budget +16 regen = 64 budget |
| **Act 1 Faz 1 total budget (raw candidates)** | **~256-300 gens** | With 25% regen buffer |
| Remaining after Act 1 Faz 1 | ~2,900 gens | From 3,200 current |

**Budget verdict: SAFE.** 3,200 remaining is sufficient for all 3 Acts at 110 assets each (~446 effective gens per Karar #150 math). Act 1 Faz 1 skeleton consumes ~250-300 gens total.

**Balance alert: $0.00 USD credits.** All remaining budget is subscription generations (3,200). No pay-per-gen fallback available. If subscription resets or gen limit is hit mid-batch, dispatches will fail. No immediate risk at current spend rate.

---

## 5. Dispatch Sequence (Faz 1 Priority Order)

```
Priority 1: Batch 1 (L3 walls) -- SKELETON FIRST
  Why: Walls are the architectural skeleton. Floor and decoration meaningless without skeleton.
  Gate: 5-gate Karar #150 QC after each wall class (silhouette / inside-feel / wall depth / arch readability / 35deg compatibility)
  
Priority 2: Download reusables (L1+L2 floor)
  Why: Free -- zero gens. Import existing granite tiles_pro + Wang tilesets into Act1_ShatteredKeep.
  Gate: Visual palette QC against #3A3D42. If cool granite confirmed, Batch 2 is skipped.
  
Priority 3: Batch 2 (L1+L2 floor -- conditional)
  Why: Only if reusable download QC fails. High probability SKIP given 8+ granite tile sets on account.
  
Priority 4: Batch 3 (L4 patches) + Batch 4 (L6 accent) -- parallel
  Why: Both are decorative layers, can dispatch simultaneously after floor confirmed.
  Gate: Style match against wall class renders before compositing.
```

**Recommended first dispatch: Batch 1, wall class 6 (arch) FIRST within the batch.**
Rationale: The archway is the most architecture-defining element and the hardest to get right (fake-iso depth + cyan glow). If arch fails the 5-gate QC, it signals a root-cause prompt issue before committing 7 more wall class dispatches.

---

## 6. Quality Gates Per Batch

Applied after each `create_object` dispatch + `select_object_frames` review:

| Gate | Check | Pass criterion |
|---|---|---|
| 1. Silhouette | Internal architecture primary, perimeter off-screen | Wall sprite readable as standalone architectural element |
| 2. Inside-feel | Depth readable without full scene context | Top cap + front face + shadow visible in isolation |
| 3. Wall depth | Fake-iso top cap + front face + base shadow | Three distinct shading zones present, flat FAIL |
| 4. Arch readability | Opening silhouette clear, gate identity visible | Arch portal opening unambiguous |
| 5. 35deg compatibility | Overhead angle consistent with floor perspective | Character-to-wall spatial read coherent at #3A3D42 palette |

**FAIL rule:**
- 1-2 gates fail on a single wall class: regen that class only, same prompt, different seed
- 3+ gates fail on a single class: inspect prompt -- likely forbidden phrase or anchor drift
- Cross-class palette inconsistency: use `create_object_state` with source=best-passing-class for forced style lock

---

## 7. Pending Review Items (action before gen)

Before dispatching any batch, resolve these:

1. **c2b48c99** (object, review:awaiting-selection): Cyan-violet rift crack -- run `select_object_frames` if any candidates are usable as L6 accent. This may save 1 Batch 4 accent dispatch.

2. **keep_wall_v2 objects** (76693f8f, 825ddbdd, f053b5f0, 06338801): 4 objects at 32x32 with tag `keep_wall_v2`. Too small for L3 (32x32 is tile scale, not wall sprite scale) but descriptions say "weathered stone dungeon wall". Visual inspect before writing off -- may contain L3-adjacent wall detail elements.

3. **keep_decal_v2 objects** (dabbb30f through 21406da0, 8 items): "dark dungeon floor decal" -- potential L5 scatter decals. Visual QC needed. If cool grey tone confirmed, these save partial Batch 3 cost.

4. **scatter_moss_f1 objects** (5c474658, 8cd4557a, 20517ffc, 0df42e80): Organic moss 48x48. Generic moss palette unknown -- may be usable as L4 cave moss or save Batch 3 Patch A dispatch if cool-toned.

---

## Related Karar / Spec References

- Karar #150 LIVE: `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md`
- Codex review: `STAGING/CODEX_DONE_karar_150_review.md`
- Concept reference v4 (PASS): `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`
- Asset pack hierarchy: `Assets/Art/AssetPacks/{_Universal, Act1_ShatteredKeep, Act2, Act3, Special}`
- 6-layer pipeline: Karar #143 (Alabaster Dawn Pipeline LOCK)
- create_tiles_pro 4-type batch: memory `reference_pixellab_create_tiles_pro_4type.md`
