# PROMPT — W1 Base Wall
# Status: NEEDS REGENERATION (2026-05-08) — current version has vertical squash from wrong output size
# Output: 1024×1536 PORTRAIT | Grid: 4×4 | Tile: 64×96 | Count: 16
# Slice: python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/W1" --cols 4 --rows 4 --width 64 --height 96 --prefix "w1_"
# Style anchor: STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (3).png (W1 — PRIMARY style anchor)
# IMPORTANT: Output MUST be 1024×1536 portrait. 1024×1024 causes vertical squash at 64×96 target.

---

Create a pixel art isometric dungeon wall tile sprite sheet.

OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide). NOT square.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×384 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all wall shapes. Pure flat magenta only.

PIXEL ART SCALE: Each tile is 64×96 logical pixels. Pixel clusters minimum 4px wide — no sub-pixel detail. Image will be downscaled 4× to 64×96px tiles.

STYLE (match attached reference image exactly):
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL 16 tiles
- Wall geometry: narrow top strip ~8px lit #3A3C50, tall front face #1E2030/#262838, left shadow strip 4px #12141A, mortar joints #161620 (~4px wide in output = 16px at source resolution)
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, top strip #3A3C50, mortar #161620, shadow #12141A, iron fixtures #181820. No other hues.
- Mortar grid visible on front face, horizontal and vertical pattern

TILES:
1). plain clean masonry, no damage — reference tile
2). single horizontal crack mid-height #0A0A14
3). vertical crack top strip to base
4). two parallel horizontal cracks, upper and lower
5). chipped upper-right corner, 3-4 debris pixels at top
6). mortar joint recessed on one horizontal line, deeper shadow
7). scorch mark: dark carbon smear on lower front face
8). iron ring mount on front face, dark metal #181820
9). moss in one horizontal mortar joint, #1A2810 thin line
10). moisture stain running vertically from top to base
11). chipped lower-left corner, stone debris at base
12). diagonal crack upper-right toward lower-left
13). two crossing cracks + chipped corner combined
14). moss in joint + moisture stain combined
15). deep crack with pale mineral deposit along one edge
16). most damaged: heavy crack + stone debris at base + surface weathering
