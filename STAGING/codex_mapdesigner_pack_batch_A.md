# Codex Task — MapDesigner Pack v2 — Batch A (Floor + Wang16 Walls partial)

## CRITICAL

**MUST USE imagegen skill. NOT Pillow/numpy/PIL procedural shapes.**

Bu pack **RIMA Brush V1 Map Designer** içindir. Karar #143 6-layer pipeline'a uyumlu **NATIVE PIXEL SIZE** zorunlu.

**Stil:** `STAGING/concept_art_rima_sample_room.png` painterly Hades-tradition. Multi-tone brush strokes, NOT hard-edge minimal. SAME style as `RIMA_Painterly_Pack_v1/`.

**Palette:** Vivid Vulnerability — dark slate gray + deep brown + dusty blue + moss green + warm brown + faint dark red rift + cold blue rim.

**Perspective:** High top-down 30-35° angle (Karar #100). Each tile slight depth recession baked into the painting (asymmetric — back-edge darker, front-edge brighter).

## OUTPUT — 18 sprites to `Assets/Sprites/Environment/RIMA_MapDesigner_v2/`

### L2 FLOOR (8 → `floor/`, 64×64 NATIVE, OPAQUE seamless)

Seamless tileable — edges blend to neighbor, NO border, NO grid line:

1. `floor_01_clean.png` — Clean weathered stone, dark slate dominant
2. `floor_02_mossy.png` — Sparse moss patch
3. `floor_03_cracked.png` — Hairline fractures
4. `floor_04_worn.png` — Polished smooth
5. `floor_05_stained.png` — Dusty blue mineral residue
6. `floor_06_rift_touched.png` — Cold blue glow at cracks
7. `floor_07_dirt.png` — Dirt-covered
8. `floor_08_blood_old.png` — Faded blood stain

### L3 WANG16 WALLS (10 → `walls/`, 64×96 NATIVE PER TILE, transparent top)

Wang16 corner encoding (4-bit, 2^4 = 16 cases). Each tile = NE/NW/SE/SW corner wall/floor flags.

Painterly stone wall with visible top cap (upper 32px = wall top), bottom 64px = wall body. Sparse moss at base.

10 of 16 (rest in Batch B):
1. `wang_0000.png` — All corners floor (isolated/no wall) → minimal/empty
2. `wang_0001.png` — SW corner wall only
3. `wang_0010.png` — SE corner wall
4. `wang_0011.png` — SE+SW = south wall (long horizontal at bottom)
5. `wang_0100.png` — NW corner wall
6. `wang_0101.png` — NW+SW = west wall (vertical)
7. `wang_0110.png` — NW+SE diagonal (rare)
8. `wang_0111.png` — All but NE
9. `wang_1000.png` — NE corner wall
10. `wang_1001.png` — NE+SW diagonal

Each wang_XXXX tile: 64×96 native, painterly stone, top cap visible, moss at bottom edge, transparent BG above wall top.

## TOTAL: 18 sprites = 18 imagegen calls

## DONE Report

`STAGING/codex_mapdesigner_pack_batch_A_DONE.md`

Asset count, output paths, style consistency notes.

## Constraints

- **18 imagegen calls only**
- Time budget: ~14-17 min (fits 20-min cx_dispatch ceiling)
- Native sizes EXACT — no scale-down post-process from larger generation. Use imagegen size param to generate at 64×64 or downscale by Codex's own tool to that exact size if needed.
- Style match Painterly Pack v1 — painterly Hades, NOT hard-pixel minimal
- Output paths exact:
  ```
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/floor/
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/walls/
  ```
