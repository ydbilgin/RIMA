# PROMPT — Outer Boundary Wall (Map sınırı, dış yüz)
# Status: READY TO GENERATE (2026-05-08)
# Purpose: Map dışı siyah void'i kapatmak için. Oyuncunun görebileceği map sınırlarına yerleştirilir.
# Output: 1024×1536 PORTRAIT | Grid: 4×3 | Tile: 64×128 | Count: 12
# Slice: python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/WB" --cols 4 --rows 3 --width 64 --height 128 --prefix "wb_"
# MATH: 1024÷4=256, 1536÷3=512 → 256×512 → 64×128 = exact 4× uniform ✅
# Style anchor: STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (3).png (W1)
# NOTE: 2× taller than W1/W2 — fills vertical void at map edges.

---

Create a pixel art isometric dungeon exterior boundary wall tile sprite sheet.

OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide). NOT square.
GRID: 4 columns × 3 rows = 12 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×512 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all shapes. Pure flat magenta only.

PIXEL ART SCALE: Each tile is 64×128 logical pixels (twice the height of a standard wall). Pixel clusters minimum 4px — no sub-pixel detail.

STYLE (match attached W1 reference — exterior/rougher version):
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source
- Same cold blue-grey dungeon stone: #1E2030/#262838
- Top strip #3A3C50. Left shadow strip 4px #12141A. No mortar grid — exterior face is more raw/unfinished.
- Slightly rougher surface texture than interior walls — no iron fixtures, no decorative details
- These are structural boundary walls seen from inside the dungeon

TILES:
1). plain raw exterior stone, solid face — reference tile, no damage
2). subtle surface grain variation only
3). slight horizontal crack, shallow
4). vertical moisture stain running full height
5). rough chisel-mark surface texture
6). slightly protruding edge stone at corner
7). single mortar trace, barely visible
8). surface pitting and age weathering
9). shallow crack + moisture combined
10). rough hewn stone, irregular surface blocks
11). minimal moss trace in one surface crack
12). heavy surface weathering, pitted texture
13). two faint horizontal crack lines
14). stone surface worn smooth from age
15). corner edge slightly chipped, minimal debris
16). most weathered: combined surface variation — still structurally solid
