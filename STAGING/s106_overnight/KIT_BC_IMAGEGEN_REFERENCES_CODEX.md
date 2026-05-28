# Codex Task — Kit B + Kit C Reference Image Generation (for PixelLab S-XL Pro init)

ACTIVE RULES: (1) think before generating (2) $imagegen built-in tool only (NOT scripts/image_gen.py) (3) reference quality first, exact pixel size secondary (will be re-rendered in PixelLab) (4) BLOCKED if $imagegen unavailable.

# AMAÇ
User wall-less V1 Hades Elysium style locked. Kit B (cliff face) + Kit C (bg layers) için reference image'lar üret. Bu görseller PixelLab S-XL Pro'ya **init image** olarak verilip pixel art versiyonu üretilecek. SEN pixel art üretmek zorunda değilsin — high-quality concept art / illustration yeterli, S-XL Pro pixelify edecek.

# REFERENCE IMAGES NEEDED (16 image)

## KIT B — Cliff Face (9 image, 64×96 vertical aspect ratio)
Generate at any size you can but TARGET ASPECT ≈ 2:3 vertical (64:96 = 2:3). Higher resolution than 64×96 OK — these are references for downscaling.

| # | Asset | Prompt focus |
|---|---|---|
| 1 | **N edge cliff face** | Hanging dark granite stone cliff face, NORTH-facing vertical edge of floating arena, jagged rocks descending into void, top section (top 1/3) has flat stone seam connecting to floor above, bottom 2/3 = chunky broken stone hanging down with moss and faint cyan #00FFCC glow at base, 35° dimetric/isometric projection angle, dark slate #3A3D42 palette, dark fantasy ARPG, premium painterly pixel-art-friendly style, transparent or simple black bg |
| 2 | **S edge cliff face** | Same as N but SOUTH-facing (mirror) |
| 3 | **E edge cliff face** | Same as N but EAST-facing |
| 4 | **W edge cliff face** | Same as N but WEST-facing |
| 5 | **NE corner cliff** | L-shaped corner break, both N and E edges meet, more dramatic hanging stone at corner, same palette + style |
| 6 | **NW corner cliff** | Mirror of NE |
| 7 | **SE corner cliff** | Mirror of NE south-side |
| 8 | **SW corner cliff** | Mirror of NW south-side |
| 9 | **Cyan-glow variant edge** | N edge style + heavy cyan #00FFCC rift glow at hanging base (rift bleeding from underside), for bridge transitions and rift seepage rooms |

## KIT C — Background Layers (7 image, various aspects)

| # | Asset | Target aspect | Prompt focus |
|---|---|---|---|
| 10 | **L0 Void Base** (tileable opaque) | 1:1 square ~512×512 | Top-down 2D view of deep magical void, dark indigo #0A0E1A space with subtle cyan #00FFCC rift veins, low contrast center, seamless tileable horizontal AND vertical (edges match), no characters, no walls, dark fantasy ARPG Hades Elysium feel, premium painterly pixel-art-friendly |
| 11 | **L1 Cyan Nebula / Rift Hero** (unique) | 1:1 square ~512×512 | Large cyan #00FFCC magical rift nebula sprite, swirling energy with glowing core fading to deep purple #3A1A4A edges, soft radial alpha gradient (center bright, edges fade), premium ARPG dark fantasy, no characters, no floor, transparent or solid void background |
| 12 | **L2 Far Ruins Strip A** (horizontal tileable) | 16:9 wide ~688×384 | Wide horizontal pixel art background strip of distant broken marble ruins floating far below the camera in a sky void, warm gold #E89020 highlights on column edges, cyan #00FFCC cracks in stone, high top-down 3/4 camera angle, soft silhouettes, low contrast, seamless horizontal tiling (left edge matches right edge), no floor, no characters |
| 13 | **L2 Far Ruins Strip B** (variant) | 16:9 wide ~688×384 | Same family as Strip A but with structural variation — different ruin arrangement, additional broken obelisk, similar palette + style + seamless tiling rules |
| 14 | **L3 Floating Island small (single, 4-set sheet)** | 1:1 ~256×256 each, optionally 2×2 grid sheet ~512×512 | Transparent floating stone island chunk, weathered dark slate #3A3D42 with broken irregular edges, faint cyan #00FFCC glow underside, top-down 3/4 ARPG perspective, mossy patches on top, designed to be readable at 256px, no background |
| 15 | **L3 Floating Island large (boss landmark)** | 1:1 ~512×512 | Larger transparent stone monument fragment, ancient marble pillars + broken stairs + ritual altar visible on top, cyan rift cracks at base, warm gold weathering, top-down 3/4 ARPG perspective, designed as boss arena backdrop landmark, no characters, no surrounding floor |
| 16 | **L4 Fog Veil** (low alpha horizontal) | 16:9 ~688×384 | Transparent soft magical fog veil, pale cyan #80D0FF and warm gold #E89020 tints blended low alpha, horizontal drifting strip, low detail organic shapes, designed to overlay void background as depth cue without obscuring gameplay above, seamless horizontal tiling |

## BONUS (optional, if time/credits allow):
- Light beam decal 512×512 vertical additive (cyan rift → warm orange base)
- Warm amber overlay 512×512 (treasure mood sanctuary)
- Rift Tear hazard 128×128 (boss arena 3m circular hazard, viewable top-down)
- Particle sheet 256×256 4×4 grid (cyan motes + warm sparks)

# OUTPUT STRUCTURE
Save to: `STAGING/s106_overnight/ref_kit_b/` (cliff face 9 images) + `STAGING/s106_overnight/ref_kit_c/` (bg 7+ images)

Filename convention:
- Kit B: `cliff_N.png`, `cliff_S.png`, `cliff_E.png`, `cliff_W.png`, `cliff_NE.png`, `cliff_NW.png`, `cliff_SE.png`, `cliff_SW.png`, `cliff_cyan_glow.png`
- Kit C: `bg_L0_void.png`, `bg_L1_nebula.png`, `bg_L2_ruins_A.png`, `bg_L2_ruins_B.png`, `bg_L3_island_small.png`, `bg_L3_island_large.png`, `bg_L4_fog.png`
- Bonus: `bonus_light_beam.png`, `bonus_warm_overlay.png`, `bonus_rift_tear.png`, `bonus_particles.png`

# STYLE CONSISTENCY (critical — all 16+ images must feel same family)
- Camera: 35° dimetric / high top-down 3/4 (Hades, Diablo III ref)
- Palette: dark slate #3A3D42 base, cyan #00FFCC accent, warm orange #E89020 secondary accent, deep purple #3A1A4A bg depth
- Mood: dark fantasy, broken ancient ruins, rift catastrophe aftermath
- NO modern UI, NO HUD, NO characters in any image
- NO anti-aliasing soft edges in concept (will be pixelified)
- High contrast lighting (dim ambient + focal cyan + warm accent)

# REPORT
Save brief report at `STAGING/s106_overnight/KIT_BC_REFERENCES_REPORT.md`:
- Per image: 1 line saying it was generated successfully
- Note any prompts that needed adjustment
- Total time

# Constraints
- Use $imagegen ONLY
- Style consistency across all images (treat as one project, not isolated calls)
- Estimated time: 25-40 min for full 16 images
- Bonus 4 images optional based on time
