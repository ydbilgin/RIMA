# Codex Task — v15d Tile Asset Support (gpt-image-1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Background
v15d composition budget LOCK requires aligned tile assets:
- 3 floor variants per biome (dominant/secondary/accent)
- 8-direction path tile set (Wang-style)
- Transition decals (biome edges)

Hybrid pipeline LOCK (`hybrid_asset_pipeline_lock.md` S87 Karar #157): tiles use **gpt-image-1 backend** via Codex imagegen workflow (NOT PixelLab).

## Goal
Produce 16-24 sprites for combat biome v15d composition.

## Scope — Combat Biome only (Phase 1)

### Group A — Dominant Floor Variants (3 sprites, 64×64)
Style anchor: "RIMA stone-brick combat floor, painterly pixel art top-down 30-35° angled, muted grey-blue palette, subtle weathered texture, NO purple crystals, NO blue runes, NO grass tufts. Clean readable surface for combat overlay."

- v1: clean weathered stone-brick base
- v2: same with minor crack detail (5% variation)
- v3: same with subtle moss specks (8% variation)

All 3 are interchangeable as Layer 2 dominant floor.

### Group B — Secondary Floor Overlay (3 sprites, 64×64)
"RIMA combat floor secondary — darker stone variant, slightly cracked, slightly mossier than dominant. Same palette family, lower value. For Layer 3 overlay at 20% coverage."

### Group C — Accent Floor (2 sprites, 64×64)
"RIMA combat floor accent — single deliberate detail (faint glow rune OR cracked tile reveal). Sparse use at 10% coverage. Same palette family."

### Group D — Path Tile Set (8 sprites, 64×64, Wang corner set)
"RIMA combat path — cobblestone walk variant of dominant floor, distinct value (lighter or darker by ΔL=20+), 8-direction Wang corner set for AutoTile."

Wang directions (per `reference_pixellab_direction_sequence`): N, NE, E, SE, S, SW, W, NW. Generate 8 corner tiles for Unity AutoTile import.

### Group E — Transition Decals (4-6 sprites, 64×64)
"RIMA biome edge decal — soft fade transition. Stone→Grass, Stone→Dirt, Stone→Water. ~50% opacity organic edge mask."

3-6 unique decals depending on layout fit.

## Production workflow
1. Use existing Codex imagegen tooling (see `STAGING/codex_imagegen_*` task files for reference format)
2. gpt-image-1 backend, painterly RIMA style (see existing v15c sprites in `Assets/Data/Brush/AssetParts_v3/` for style match)
3. Save to: `Assets/Data/Brush/AssetParts_v3/CombatBiome_v15d/`
4. Generate Unity .meta files
5. Wire to zone .asset: `Assets/Data/Blueprint/Zones/combat_zone.asset` (or equivalent — find correct path)
   - dominantFloorPool ← Group A pool
   - secondaryFloorPool ← Group B pool
   - accentFloorPool ← Group C pool
   - pathPool (new field in v15d composition) ← Group D
   - transitionDecals ← Group E

## Style consistency check
Visual A/B against existing PixelLab characters/props in `Assets/Sprites/RIMA_PixelLab/`. Tile output should look like SAME WORLD as character anchors. Take a side-by-side screenshot:
- Save to: `STAGING/style_check_v15d_tiles_vs_pixellab_chars.png`
- If clash > 30% perceptual difference → flag for user review (memory hybrid LOCK fallback trigger)

## Acceptance
- 16-24 sprites produced + Unity-imported (.meta present)
- Pool ScriptableObjects created/updated
- Combat zone .asset references all new pools
- Style check screenshot saved
- DONE marker: `STAGING/CODEX_TASK_v15d_TILE_ASSETS_DONE.md` listing files + style check verdict

## What NOT to do
- No PixelLab calls (hybrid LOCK)
- No new biome (combat only Phase 1; other biomes Phase 2)
- No character/mob/prop generation (PixelLab budget)
- Don't touch v15d composition implementation (Codex laurethgame parallel task)
- Don't commit
