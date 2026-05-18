# Phase B-3 Asset Gap Imagegen (Blueprint Painter pool fill)

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: yasinderyabilgin (reset 05:28, after which 5h % drops to ~5%)
Effort: high
Timeout: 7200s
Skill: imagegen (gpt-image-1, built-in Codex tool)
Source: Phase B-3 DONE marker `STAGING/CODEX_TASK_PHASE_B3_IMPLEMENT_DONE.md` ## Asset Gaps section

## Context

Phase B-3 Blueprint Painter LIVE (commit f76c36e, 364/364 PASS). Auto-populate works using existing 124 sprites mapped via category-name matching. But:

- `pool_water.asset` — EMPTY by design (no matching category in RIMA_v2/v3)
- `pool_grass.asset` — sparse (only moss + mossy floor, lacking flora)
- 3 transition rules placeholder pools (grass/stone, path/grass, water/grass)
- 3 transition rules absent (stone/wall, wall/water, water/feature) — Phase 2 dispatch later

This dispatch fills **Phase 1 critical gaps** (~32 sprites) so Combat v15 blueprint-first redesign has full visual fidelity.

## Goal

Produce PNG sprites via Codex imagegen skill (gpt-image-1). Each sprite:
- 256×256 transparent background source PNG
- Painterly RIMA art style (Hades-tone: cold slate dungeon palette + warm accent glows)
- Top-down view, slight 30-35° tilt (matches PlayableRoom camera)
- Suitable for 64×64 downsample (nearest-neighbor) for in-game use
- Each sprite stands alone (no scene background, no UI chrome)

## Categories

### 1. pool_water (8 sprites)

Prompt template: "top-down pixel art shallow water decals on dark stone dungeon floor, painterly RIMA style, transparent background, cold slate palette with subtle teal-blue water reflections, 30-35 degree tilt. Single isolated decal, no scene background. Variations:"

- `water_puddle_small_v1.png` — small round puddle, ~half tile diameter
- `water_puddle_small_v2.png` — irregular puddle variant
- `water_puddle_medium_v1.png` — larger oval puddle, ~1.5 tile
- `water_puddle_medium_v2.png` — medium puddle with stone edge poking through
- `water_flood_decal_v1.png` — flooded stone floor patch with ripples
- `water_flood_decal_v2.png` — wider flood patch with lichen edges
- `water_ripple_v1.png` — water surface ripple/wave pattern
- `water_dark_pool_v1.png` — deep dark pool with subtle glow reflection

### 2. pool_grass enhancement (6 sprites)

Prompt template: "top-down pixel art grass tufts, weeds, and dungeon overgrowth decals, painterly RIMA style, transparent background, dark mossy green palette with subtle brown accents, 30-35 degree tilt. Single isolated decal, no scene background. Variations:"

- `grass_tuft_small_v1.png` — small grass clump
- `grass_tuft_small_v2.png` — clump variant
- `grass_tuft_medium_v1.png` — denser grass patch
- `grass_weeds_v1.png` — wild weeds with seed heads
- `grass_overgrowth_v1.png` — broken dungeon flooring with grass through cracks
- `grass_overgrowth_v2.png` — mossy stones with grass tufts

### 3. transition grass/stone (6 sprites)

Prompt template: "top-down pixel art moss-on-stone boundary decals, painterly RIMA style, transparent background, dark green moss creeping over cracked grey stone, 30-35 degree tilt. Single isolated decal designed to span boundary between two tile zones. Variations:"

- `transition_grass_stone_edge_h_v1.png` — horizontal edge strip
- `transition_grass_stone_edge_h_v2.png` — h-edge variant
- `transition_grass_stone_edge_v_v1.png` — vertical edge strip
- `transition_grass_stone_corner_v1.png` — corner patch
- `transition_grass_stone_blob_v1.png` — irregular blob spanning
- `transition_grass_stone_blob_v2.png` — blob variant with lichen

### 4. transition path/grass (6 sprites)

Prompt template: "top-down pixel art worn dirt path edge with grass tufts and pebbles, painterly RIMA style, transparent background, warm brown earth blending into dark green grass, 30-35 degree tilt. Single isolated decal for boundary between path and grass zones. Variations:"

- `transition_path_grass_edge_h_v1.png` — horizontal edge with scattered pebbles
- `transition_path_grass_edge_h_v2.png` — edge variant with grass tufts
- `transition_path_grass_edge_v_v1.png` — vertical edge
- `transition_path_grass_corner_v1.png` — corner
- `transition_path_grass_scatter_v1.png` — sparse pebble+grass scatter
- `transition_path_grass_scatter_v2.png` — scatter variant

### 5. transition water/grass (6 sprites)

Prompt template: "top-down pixel art reeds and wet grass at water edge, painterly RIMA style, transparent background, dark teal water blending into dark green wet grass with thin upright reeds, 30-35 degree tilt. Single isolated decal for boundary between water and grass zones. Variations:"

- `transition_water_grass_reeds_v1.png` — clump of tall reeds
- `transition_water_grass_reeds_v2.png` — sparse reeds variant
- `transition_water_grass_edge_h_v1.png` — horizontal wet grass edge
- `transition_water_grass_edge_v_v1.png` — vertical wet grass edge
- `transition_water_grass_algae_v1.png` — algae mat at water edge
- `transition_water_grass_wet_v1.png` — wet darkened grass patch

## Output structure

```
STAGING/RIMA_AssetParts_Phase_B3_Gaps/
  pool_water/
    *.png (8 files)
  pool_grass_enhancement/
    *.png (6 files)
  transition_grass_stone/
    *.png (6 files)
  transition_path_grass/
    *.png (6 files)
  transition_water_grass/
    *.png (6 files)
```

Total: 32 PNG files

## Post-imagegen Unity integration (Codex task, same dispatch)

After all 32 PNGs generated:

1. **Move to Unity asset path:**
   ```
   Assets/Data/Brush/AssetParts_v4_Phase_B3_Gaps/
     Water/, Grass/, TransitionGrassStone/, TransitionPathGrass/, TransitionWaterGrass/
   ```

2. **Sprite import settings:**
   - PixelsPerUnit: 32
   - FilterMode: Point (no filter)
   - Compression: None
   - SpriteMode: Single
   - PivotMode: Bottom-Center (for prop) or Center (for transition decal)

3. **PropDefinitionSO wrapper creation** (mirror existing GeneratedProps pattern):
   - For each PNG, create `Assets/Data/Blueprint/GeneratedProps/Phase_B3_Gaps/{category}/{name}.asset` with:
     - `worldSprite` field pointing to the imported sprite
     - `propId` = filename without .png
     - `footprintSize` = (1,1)
     - `blocksWalkable` = false for transitions/decals; true for solid props
     - `respectsWalkableMask` = true (Karar #143 compliance)
   - Use Phase 1A SO (`RIMA.MapDesigner.SO.PropDefinitionSO`), NOT Brush V1

4. **Pool updates:**
   - Update `Assets/Data/Blueprint/PropPools/pool_water.asset` — add 8 new entries (weight 1.0 each)
   - Update `Assets/Data/Blueprint/PropPools/pool_grass.asset` — add 6 new entries (weight 1.0 each)
   - Create `Assets/Data/Blueprint/PropPools/pool_transition_grass_stone.asset` — 6 entries
   - Create `Assets/Data/Blueprint/PropPools/pool_transition_path_grass.asset` — 6 entries
   - Create `Assets/Data/Blueprint/PropPools/pool_transition_water_grass.asset` — 6 entries

5. **Adjacency rule updates:**
   - `Assets/Data/Blueprint/AdjacencyRules/rule_grass_stone.asset` → transitionPool = pool_transition_grass_stone
   - `Assets/Data/Blueprint/AdjacencyRules/rule_path_grass.asset` → transitionPool = pool_transition_path_grass
   - `Assets/Data/Blueprint/AdjacencyRules/rule_water_grass.asset` → transitionPool = pool_transition_water_grass

## Acceptance Criteria

1. 32 PNG files created in `STAGING/RIMA_AssetParts_Phase_B3_Gaps/` matching listed names
2. 32 sprites imported to `Assets/Data/Brush/AssetParts_v4_Phase_B3_Gaps/`
3. 32 PropDefinitionSO wrappers created in `Assets/Data/Blueprint/GeneratedProps/Phase_B3_Gaps/`
4. 5 pool .asset files updated/created with new entries
5. 3 adjacency rules updated with new transition pools
6. **Test:** open Blueprint Painter window, load profile_combat_room_default, paint a 4-zone test (water/grass/stone/path patches), click Auto-Populate, click Adjacency Pass → verify all zones have ≥ 3 props placed + transitions visible at boundaries
7. **Screenshot:** `Assets/Screenshots/phase_b3_asset_gap_filled_test.png` showing populated 4-zone test scene
8. **EditMode tests:** 364/364 PASS preserved (no new tests required for this dispatch, just asset additions)
9. **Console:** 0 errors, 0 warnings
10. **DONE marker:** `STAGING/CODEX_TASK_PHASE_B3_ASSET_GAP_IMAGEGEN_DONE.md` with file count + screenshot path + verification report

## Visual style consistency notes

- Match Hades-tone: cold slate (#3A4A5C base) + warm accents (#D4AF37 gold, #C9472D red)
- Painterly NOT cartoon — visible brush strokes, organic edges
- Hades 30-35° tilt — top elements slightly compressed, bottom elements stretched
- Transparent backgrounds via alpha channel
- Sprite footprint approximate matching: small decals fit within 32×32 tile, medium/large 1.5-2×

## Phase 2 deferred (separate dispatch later)

- transition stone/wall (6 sprites)
- transition wall/water (4 sprites)
- transition water/feature (4 sprites)
- Total deferred: ~14 sprites for adjacency completeness

## Verdict format

```
# Phase B-3 Asset Gap Imagegen Done
Status: DONE_FOR_ORCHESTRATOR_REVIEW
Date: 2026-05-18
Executor: Codex yasinderyabilgin

## Files added
- 32 PNG sources in STAGING/RIMA_AssetParts_Phase_B3_Gaps/
- 32 Unity sprite assets in Assets/Data/Brush/AssetParts_v4_Phase_B3_Gaps/
- 32 PropDefinitionSO wrappers in Assets/Data/Blueprint/GeneratedProps/Phase_B3_Gaps/
- 3 new pool .asset files
- 2 updated pool .asset files

## Files modified
- Assets/Data/Blueprint/PropPools/pool_water.asset
- Assets/Data/Blueprint/PropPools/pool_grass.asset
- Assets/Data/Blueprint/AdjacencyRules/rule_grass_stone.asset
- Assets/Data/Blueprint/AdjacencyRules/rule_path_grass.asset
- Assets/Data/Blueprint/AdjacencyRules/rule_water_grass.asset

## Visual style verification
- Painterly Hades-tone preserved: yes/no with sample comparison
- Transparent backgrounds: yes/no
- Top-down 30-35° tilt: yes/no

## Auto-Populate test result
- Paint test layout: 4-zone (water/grass/stone/path) painted
- Props placed per zone: water=X, grass=Y, stone=Z, path=W
- Adjacency transitions visible: yes/no

## Sample screenshots
- {path}

## EditMode regression
- 364/364 PASS preserved

## Console errors
- {none / list}

## Imagegen Phase B-3 Asset Gap deliverable verdict
{PASS_FOR_ORCHESTRATOR_REVIEW / NEEDS_HELP}
```
