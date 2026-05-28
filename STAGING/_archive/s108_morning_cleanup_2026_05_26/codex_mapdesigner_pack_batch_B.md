# Codex Task — MapDesigner Pack v2 — Batch B (Wang16 Walls remaining + L4 Decals)

## CRITICAL

**MUST USE imagegen skill. NOT Pillow.**

Bu pack **RIMA Brush V1 Map Designer** içindir. Karar #143 6-layer pipeline.

**Stil:** Painterly Hades-tradition, same as `RIMA_Painterly_Pack_v1/`.

**Palette:** Vivid Vulnerability (dark slate gray + deep brown + dusty blue + moss green + crimson rift + cold blue rim).

**Perspective:** High top-down 30-35° (Karar #100).

## OUTPUT — 18 sprites to `Assets/Sprites/Environment/RIMA_MapDesigner_v2/`

### L3 WANG16 WALLS (remaining 6 → `walls/`, 64×96 NATIVE)

11. `wang_1010.png` — NE+SE = east wall (vertical right)
12. `wang_1011.png` — All but NW
13. `wang_1100.png` — NE+NW = north wall (horizontal top)
14. `wang_1101.png` — All but SE
15. `wang_1110.png` — All but SW
16. `wang_1111.png` — All 4 corners (full enclosed)

Same style as Batch A walls — painterly stone with top cap, moss at base.

### L4 ORGANIC DECALS (8 → `decals_L4/`, 64×64 NATIVE, transparent BG)

Patch overlays for layered compose (Brush V1 L4 layer):

1. `L4_moss_patch_01.png` — Dense moss tuft cluster
2. `L4_moss_patch_02.png` — Sparse moss spread
3. `L4_dirt_patch.png` — Brown organic dirt patch
4. `L4_wet_patch.png` — Dark wet/damp stain
5. `L4_grass_tuft.png` — Small green grass cluster
6. `L4_vine_patch.png` — Creeping vine fragment
7. `L4_ash_patch.png` — Grey ash residue
8. `L4_blood_patch.png` — Old blood stain

Each: painterly painted patch, irregular organic shape, soft transparent edges for blend on top of floor tiles.

### L5 DETAIL SCATTER (4 → `detail_L5/`, 32×32 NATIVE, transparent BG)

Small scatter elements for layered compose (Brush V1 L5):

1. `L5_pebbles.png` — Small dark stones cluster
2. `L5_crack_pattern.png` — Hairline crack (small)
3. `L5_bone_fragment.png` — Single bone shard
4. `L5_debris.png` — Small rubble cluster

## TOTAL: 18 sprites = 18 imagegen calls

## DONE Report

`STAGING/codex_mapdesigner_pack_batch_B_DONE.md`

## Constraints

- **18 imagegen calls only**
- Time budget: ~14-17 min
- Native sizes EXACT (64×96 walls, 64×64 decals, 32×32 details)
- Style match Painterly Pack v1
- Output paths exact:
  ```
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/walls/
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/decals_L4/
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/detail_L5/
  ```
