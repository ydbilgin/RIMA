# Phase A v15c Layer 1 + Layer 8 Imagegen (8-Layer Painted Top-Down asset gap fill)

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: yasinderyabilgin (FRESH post-reset)
Effort: high
Timeout: 7200s
Skill: imagegen (gpt-image-1)
Reference memory: `feedback_layered_terrain_mandatory.md`

## Context

Phase A v15c refactor dispatched in parallel (b7cq6s9a8 laurethgame). v15c spec marks Layer 1 (macro ambient fill) + Layer 8 (atmospheric overlay) as IMAGEGEN GAP per zone. Total ~20 sprites needed.

## Goal

Generate Layer 1 macro fill + Layer 8 atmospheric overlay sprites for 6 zones (path/grass/stone/wall/water/feature) in painterly RIMA Hades-tone style.

## Output structure

```
STAGING/RIMA_AssetParts_v15c_Layered/
  layer1_macro_fill/
    macro_path_v1.png, macro_path_v2.png
    macro_grass_v1.png, macro_grass_v2.png
    macro_stone_v1.png, macro_stone_v2.png
    macro_wall_v1.png, macro_wall_v2.png
    macro_water_v1.png, macro_water_v2.png
    macro_feature_v1.png, macro_feature_v2.png
  layer8_atmospheric/
    atmos_path_dust_motes.png, atmos_path_warm_haze.png
    atmos_grass_pollen.png, atmos_grass_green_mist.png
    atmos_stone_cool_mist.png, atmos_stone_dust.png
    atmos_wall_deep_shadow.png
    atmos_water_mist.png
    atmos_feature_blood_mist.png, atmos_feature_ember_particles.png
```

Total: 22 PNG (12 macro + 10 atmospheric)

## Visual style

ALL sprites:
- 256×256 transparent background RGBA PNG
- Painterly RIMA Hades-tone (cold slate dungeon palette + warm accents)
- Top-down 30-35° tilt view
- Cell-tilable (Layer 1 macro fits within 1 unit / 32 PPU cell)
- Layer 8 atmospheric supports rotation jitter at runtime

## Layer 1 prompts (12 sprites)

### Path zone (warm earth)
- `macro_path_v1.png`: "Top-down pixel art warm tan painterly stone-and-dirt path floor base, sweeping organic shape, dungeon palette with warm earth tones, transparent background, 32 PPU pixel art, 30-35 degree tilt, designed as base ambient fill"
- `macro_path_v2.png`: Same prompt, "variant 2" with different organic shape

### Grass zone (forest canopy)
- `macro_grass_v1.png`: "Top-down pixel art dark mossy green forest floor base, dense vegetation undertones, organic painterly sweeping shape, dungeon palette, transparent background, 32 PPU, 30-35 degree tilt"
- `macro_grass_v2.png`: variant 2

### Stone zone (vault grey)
- `macro_stone_v1.png`: "Top-down pixel art grey stone vault floor base, cool slate undertones, painterly cobble suggestions, organic ambient fill, transparent background, 32 PPU, 30-35 degree tilt"
- `macro_stone_v2.png`: variant 2

### Wall zone (dark vault)
- `macro_wall_v1.png`: "Top-down pixel art deep grey stone vault floor base, very dark, wall-base ambient, painterly slate, transparent background, 32 PPU, 30-35 degree tilt"
- `macro_wall_v2.png`: variant 2

### Water zone (dark teal pool)
- `macro_water_v1.png`: "Top-down pixel art deep teal water pool floor base, dark blue-green water surface, painterly ambient water, transparent background, 32 PPU, 30-35 degree tilt"
- `macro_water_v2.png`: variant 2

### Feature zone (blood altar)
- `macro_feature_v1.png`: "Top-down pixel art dark red blood altar floor base, ritual ambient, occult painterly undertones, transparent background, 32 PPU, 30-35 degree tilt"
- `macro_feature_v2.png`: variant 2

## Layer 8 prompts (10 sprites)

### Path
- `atmos_path_dust_motes.png`: "Top-down pixel art warm dust mote particles drifting over path, painterly small specks with subtle warm glow, transparent background, 32 PPU, designed as atmospheric overlay"
- `atmos_path_warm_haze.png`: "Top-down pixel art warm ambient haze patch, soft golden glow, painterly fog, transparent background, 32 PPU"

### Grass
- `atmos_grass_pollen.png`: "Top-down pixel art drifting green pollen particles, organic plant ambient, painterly small specks, transparent background, 32 PPU"
- `atmos_grass_green_mist.png`: "Top-down pixel art subtle green mist patch, forest atmosphere, painterly soft fog, transparent background, 32 PPU"

### Stone
- `atmos_stone_cool_mist.png`: "Top-down pixel art cool grey ambient mist, dungeon dampness, painterly soft fog, transparent background, 32 PPU"
- `atmos_stone_dust.png`: "Top-down pixel art grey dust particles, dungeon stone air, painterly specks, transparent background, 32 PPU"

### Wall
- `atmos_wall_deep_shadow.png`: "Top-down pixel art deep shadow accent overlay, dark gradient patch, painterly umbra, transparent background, 32 PPU"

### Water
- `atmos_water_mist.png`: "Top-down pixel art water mist particles, cool teal evaporation, painterly droplet specks, transparent background, 32 PPU"

### Feature
- `atmos_feature_blood_mist.png`: "Top-down pixel art dark red ritual mist, occult atmosphere, painterly soft red fog, transparent background, 32 PPU"
- `atmos_feature_ember_particles.png`: "Top-down pixel art glowing orange-red ember particles, ritual fire ambient, painterly bright specks, transparent background, 32 PPU"

## Post-imagegen Unity integration (same dispatch)

1. **Import 22 PNGs** to `Assets/Data/Brush/AssetParts_v5_Layer1_8/{macro_fill, atmospheric}/`
2. **Sprite import settings**: PPU 32, Point filter, no compression, Single mode, Center pivot
3. **PropDefinitionSO wrappers** in `Assets/Data/Blueprint/GeneratedProps/v15c_Layered/`:
   - Macro sprites: `worldSprite` + `propId` = filename
   - Atmospheric sprites: `worldSprite` + `propId` = filename, `blocksWalkable = false`, `respectsWalkableMask = true`
4. **Pool .asset files**:
   - 6 new `Assets/Data/Blueprint/PropPools/pool_atmospheric_{zone}.asset` (one per zone with that zone's atmospheric sprites)
5. **Zone .asset migration** (combines with v15c refactor dispatch):
   - For each zone, assign `macroFillSprites` array = corresponding 2 macro sprites
   - For each zone, assign `atmosphericPool` field = corresponding pool_atmospheric_{zone}.asset

   **IMPORTANT**: v15c refactor dispatch (b7cq6s9a8) will also touch zone .asset files. **Coordinate**: this imagegen dispatch should run AFTER v15c refactor base schema is in place. If race condition risk, write asset assignments to a separate `Assets/Data/Blueprint/v15c_PostImagegen_Migration.asset` script that orchestrator can apply manually.

## Acceptance Criteria

1. 22 PNG files created in `STAGING/RIMA_AssetParts_v15c_Layered/`
2. 22 Unity sprites imported to `Assets/Data/Brush/AssetParts_v5_Layer1_8/`
3. 22 PropDefinitionSO wrappers in `Assets/Data/Blueprint/GeneratedProps/v15c_Layered/`
4. 6 new atmospheric pool .asset files
5. Zone .asset macroFillSprites + atmosphericPool fields assigned (or migration script written)
6. Painterly Hades-tone consistency: sample comparison vs existing AssetParts_v4 quality
7. EditMode tests: existing 386+/386+ PASS (no test changes needed)
8. DONE marker `STAGING/CODEX_TASK_PHASE_A_v15c_LAYER1_LAYER8_IMAGEGEN_DONE.md`

## Edge cases

- v15c refactor dispatch (b7cq6s9a8) hasn't completed when imagegen dispatch starts → write zone asset assignments to migration script for manual orchestrator application post-v15c
- gpt-image-1 returns ambiguous painterly result → use Phase B-3 imagegen DONE sample (`water_puddle_medium_v1`, `transition_grass_stone_blob_v1`) as quality reference

## Verdict format

Standard DONE pattern with:
- 22 PNG file list
- Unity import verification
- Pool/asset assignment status
- Painterly style verification (sample comparison)
- Race condition with v15c refactor: resolved / pending manual apply
