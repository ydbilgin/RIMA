# PROMPT — F3 Mossy Floor
# Status: APPROVED (2026-05-08)
# Output: 1024×1024 | Grid: 4×4 | Tile: 64×64 | Count: 16
# MATH: 1024÷4=256, 1024÷4=256 → 256×256 → 64×64 = exact 4× ✅ (4×3 was WRONG — 1024÷3=341.33 not integer)
# Slice: python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/F3" --cols 4 --rows 4 --width 64 --height 64 --prefix "f3_"
# Style anchor: STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (1).png (F2 — same stone)
# IMPORTANT: Attach style anchor. F3 must NEVER be placed in same Random Tile pool as F1 or F2.
# NOTE: Gemini review suggests lightening mortar contrast -10% before Unity import.

---

Create a pixel art isometric dungeon floor tile sprite sheet.

OUTPUT SIZE: 1024×1024 square image.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×256 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all diamond shapes. Pure flat magenta only.

PIXEL ART SCALE: Each tile is 64×64 logical pixels. Pixel clusters minimum 4×4px — no sub-pixel detail. Image will be downscaled 4× to 64px tiles.

STYLE (match attached reference — same stone, organic growth added):
- Isometric diamond shape, 2:1 width-to-height ratio
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL tiles
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, mortar #161620, shadow #151820, moss #1A2810/#263A1A. No other hues. Especially NO bright green, NO yellow-green.
- Moss: desaturated dark green ONLY — #1A2810 (shadow) to #263A1A (lit edge).
- Organic growth is the dominant feature, stone base identity still readable underneath

TILES:
1). base stone with moisture sheen only — wet surface, faint specular line, no moss yet
2). moss trace in mortar joints only, thin dark line #1A2810
3). small moss colony on one corner, 5-7px organic blob
4). moss spreading across two adjacent block faces
5). heavy moss patch on one full face, 40% coverage
6). moss patch at one diamond corner edge, organic shape
7). pale mineral stain ring, dried moisture circle #1E2030
8). moisture sheen on full surface, wet stone look
9). fungal growth: pale dot cluster 3-4px each, scattered on one block face
10). moss plus hairline crack — organic growth inside the crack line
11). mold bloom: dark irregular patch #141414, organic texture
12). water pooling at corner: darkened diamond edge, moisture gradient
13). stone base with two types: moisture stain ring + faint moss trace combined
14). moss covering 60% of surface, crack visible underneath
15). mold plus moisture plus faint crack: three combined subtle elements
16). most organic: heavy moss AND moisture stain AND mold bloom — richly textured surface
