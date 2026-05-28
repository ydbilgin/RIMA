# Codex Task — MapDesigner Pack v2 — Batch C (L6 Accents + Props)

## CRITICAL

**MUST USE imagegen skill. NOT Pillow.**

Bu pack **RIMA Brush V1 Map Designer** içindir. Karar #143 6-layer pipeline.

**Stil:** Painterly Hades-tradition, same as `RIMA_Painterly_Pack_v1/`.

**Palette:** Vivid Vulnerability.

**Perspective:** High top-down 30-35°.

## OUTPUT — 20 sprites to `Assets/Sprites/Environment/RIMA_MapDesigner_v2/`

### L6 ACCENTS (4 → `accents_L6/`, 128×128 NATIVE, transparent BG)

Large overlay accents — central features per room:

1. `L6_rift_scar.png` — Dark crimson irregular multi-blob with radial cracks + cold blue rim glow
2. `L6_battle_aftermath.png` — Blood splatter + dust cloud
3. `L6_scorch_burn.png` — Burned area, charcoal center fading to ember edges
4. `L6_ritual_circle.png` — Faded ritual circle with sigil markings

### PROPS (12 → `props/`, 64×64 NATIVE, transparent BG)

Free-standing objects for environmental scatter:

1. `prop_crate.png` — Wooden crate
2. `prop_urn_intact.png` — Stone urn intact
3. `prop_urn_broken.png` — Broken stone urn
4. `prop_barrel.png` — Wooden barrel
5. `prop_candle.png` — Iron candle holder with lit candle
6. `prop_brazier.png` — Iron tripod brazier with fire
7. `prop_banner.png` — Torn red banner hanging (vertical) — may need 64×128 tall variant
8. `prop_column.png` — Stone column intact
9. `prop_pillar_broken.png` — Broken pillar fragment
10. `prop_chest_closed.png` — Closed treasure chest
11. `prop_chest_open.png` — Open chest revealing contents
12. `prop_statue_torso.png` — Stone statue torso

All 64×64 native (banner can be 64×128 if needed for visual tallness — note in DONE.md).

## TOTAL: 16 sprites = 16 imagegen calls

## DONE Report

`STAGING/codex_mapdesigner_pack_batch_C_DONE.md`

## Constraints

- **16 imagegen calls only**
- Time budget: ~13-15 min
- Native sizes EXACT (128×128 accents, 64×64 props, banner optionally 64×128)
- Style match Painterly Pack v1
- Output paths exact:
  ```
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/accents_L6/
  Assets/Sprites/Environment/RIMA_MapDesigner_v2/props/
  ```
