# Codex Task — IMAGEGEN Master Sheet (Modular Wall Asset Pack)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Codex'in **imagegen** tool'u (gpt-image-1 backend) ile direkt **modular wall asset pack master sheet** üret. PIL outline değil, GERÇEK pixel art.

Memory ([[reference-pixellab-production-knowledge]] Pattern 3): "Codex imagegen for tiles/walls/decals (vs PixelLab for chars/mobs/props)" — bu pattern'i kullan.

## Output

**Path:** `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v1.png`

**Specs:** 512×512 PNG, RGBA transparent background, pixel art (sharp edges, no anti-aliasing).

## Imagegen Prompt

Şu prompt'u **EXACTLY** imagegen tool'una ver:

```
A 512x512 pixel art sprite sheet asset pack for a dark fantasy isometric dungeon game inspired by Hades and Diablo. The sheet is organized in 4 horizontal sections containing 52 distinct game tiles total. Pixel art style with sharp pixel edges, no anti-aliasing, transparent background between tiles.

Visual style for entire sheet:
- 30-degree dimetric isometric perspective (2:1 ratio) for structural tiles
- Dark granite block stonework with deep dark mortar lines between blocks
- Palette: dark granite mid-tone, lighter highlight catching light from top-left, dark mortar lines, vivid cyan magical energy accent
- Ancient abandoned underground sanctuary atmosphere — failed wards bleeding cyan rift energy
- Painterly chunky pixel art density (Hades-inspired)

SECTION A — Top row, 4 large feature tiles 128x128 each (y=0 to 128):
1. archway_full: massive stone archway, dark granite blocks flanking, keystone block at apex, glowing cyan magical portal energy visible inside the arch opening, small crenellation on top
2. big_corner: large L-shape outer corner stonework, two perpendicular wall faces meeting at sharp 90-degree corner, weathered dark granite blocks, crenellation top, no cyan accents
3. big_column: tall freestanding stone column with decorative capstone and wider base block, dark granite blocks stacked, thin cyan glow at base
4. wall_tall_hero: tall straight stone wall feature, multiple rows of large dark granite blocks with deep mortar lines, crenellation top, subtle cyan rift crack accent on lower right

SECTION B — 2 rows of 8 modular wall tiles, 64x64 each (y=128 to 256):
Row B1 (y=128-192, 8 tiles): straight_NE (wall along NE axis with granite blocks designed to tile horizontally), straight_NW (wall along NW axis mirror), corner_outer_a, corner_outer_b (small convex outer corner variants), corner_inner_a, corner_inner_b (small concave inner corner variants), T_junction_a, T_junction_b (3-way meeting points)
Row B2 (y=192-256, 8 tiles): endcap_a, endcap_b (wall terminator mirror variants), low_wall_str (shorter waist-height wall), low_wall_corner (low wall L-shape), low_wall_end (low wall endcap), foundation_a, foundation_b (iso platform/foundation block), floor_edge (floor-to-floor transition step)
All Section B straight tiles have granite block edges designed to mate seamlessly with adjacent copies when placed side by side, like Wang tiles. No visible seams between tile copies.

SECTION C — Row of 16 rift overlay tiles, 32x32 each (y=256 to 288):
2D cyan magical energy overlay patterns on transparent background:
crack_h (horizontal lightning crack), crack_v (vertical lightning crack), burst_s (small 6-ray starburst), burst_l (large 12-ray starburst), scar_a (3 parallel cyan scars), scar_b (cyan crosshatch), glow_a (round halo ring), glow_b (oval halo), drop_a (single teardrop), drop_b (multiple teardrops), spiral (cyan swirl), zigzag (jagged lightning), pulse_a (concentric rings), pulse_b (asymmetric pulse), burst_h (horizontal burst lines), burst_v (vertical burst lines).
All use vivid cyan/teal palette (#40D0E0 to #80F0FF), glowing semi-transparent magical energy aesthetic, transparent background.

SECTION D — Row of 16 decoration tiles, 32x32 each (y=288 to 320):
Small dungeon decorations:
moss_a, moss_b, moss_c, moss_d (4 organic dark earthy green moss patch variants), candle_a (standing candle with warm orange flame), candle_b (wall-mounted candle sconce), torch_unlit (torch without flame), torch_lit (torch with bright orange flame), banner_a (intact deep purple banner with gold trim), banner_b (torn tattered banner), chain_short, chain_long (hanging dark metal chains), scatter_stone (small stone cluster), dust_pile (low gray-brown mound), skull_floor (weathered bone skull lying on side), gem_pickup (angular cyan-glowing gem crystal).

Y=320 to 512 area: empty transparent space (padding/bleed).

Critical requirements:
- Every tile is true pixel art with sharp 1:1 pixel boundaries
- Solid color pixel blocks, NO anti-aliasing, NO smooth gradients, NO blurry edges
- All 52 tiles in a single image, organized in their sections
- Transparent background between tiles and outside sprites
- Unified color palette and pixel density across the entire sheet
- NO TEXT, no labels, no numbers, no writing anywhere in the image — only visual pixel art content
- 30-degree dimetric isometric perspective for structural pieces (Section A and B)
- 2D overlay style for Section C and D (flat icons/patterns)
```

## Workflow Notes

1. **Imagegen call:** Codex'in image generation tool'u (`generate_image` veya `create_image` MCP tool olabilir). Codex CLI'da bu tool varsa otomatik kullan.
2. **Fallback:** Eğer Codex doğrudan imagegen yapamıyorsa, OpenAI API key ile gpt-image-1 endpoint'i çağır:
   ```python
   from openai import OpenAI
   client = OpenAI()
   response = client.images.generate(
       model="gpt-image-1",
       prompt=PROMPT_TEXT,
       size="1024x1024",  # nearest standard
       quality="high",
       n=1
   )
   # Save image_url or base64 to file
   ```
3. **Size:** Imagegen tool 512×512 destekliyorsa direkt. Yoksa 1024×1024 ver, sonra 512×512'ye downscale (nearest-neighbor pixel art friendly).
4. **Iterations:** İlk gen kötüyse, prompt minor tweak + retry. Max 3 iter, kaliteli kabul et.
5. **Quality verification:**
   - 52 tile görünür mü
   - Pixel art style sharp mı (anti-aliasing yok mu)
   - Transparent background korunmuş mu
   - Cyan rift accents Section C'de var mı

## Output Confirmation

- PNG path
- Image dimensions
- Visual description of what was generated
- Any iter count if multiple tries needed
- Issues/observations

## Effort

high — imagegen pixel art kaliteli output için iterasyon gerekebilir.
