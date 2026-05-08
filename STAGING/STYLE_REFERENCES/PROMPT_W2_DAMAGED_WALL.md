# PROMPT — W2 Damaged Wall
# Status: NEEDS REGENERATION (2026-05-08) — current version has vertical squash
# Output: 1024×1536 PORTRAIT | Grid: 4×4 | Tile: 64×96 | Count: 16
# Slice: python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/W2" --cols 4 --rows 4 --width 64 --height 96 --prefix "w2_"
# Style anchor: STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (3).png (W1 — same stone base)
# IMPORTANT: Output MUST be 1024×1536 portrait.
# NOTE: NO ember/lava glow tiles — Act 1 palette is cold only. Exclude warm colors entirely.

---

Create a pixel art isometric dungeon heavily damaged wall tile sprite sheet.

OUTPUT SIZE: 1024×1536 pixels, portrait orientation (taller than wide). NOT square.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×384 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all wall shapes. Pure flat magenta only.

PIXEL ART SCALE: Each tile is 64×96 logical pixels. Pixel clusters minimum 4px — no sub-pixel detail. This image will be downscaled 4× to 64×96px tiles; design accordingly.

STYLE (same stone as W1, heavy damage is dominant):
- Hard aliased edges — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL 16 tiles
- Same wall geometry as W1: top strip #3A3C50, front face #1E2030/#262838, shadow #12141A, mortar #161620
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, mortar #161620, shadow #12141A/#080810, moss #1A2810/#263A1A. No other hues. Voids are deep blue-black #080810 only.
- Structural failure reads immediately in silhouette

TILES:
1). deep single crack full height, 2px wide gap
2). spiderweb fracture from mortar joint intersection
3). two heavy cracks crossing, X pattern
4). one stone block protruding 2px from face
5). heavy scorch mark, 40% front face coverage #0E0E0E
6). crack with 2px separation gap, dark void inside #080810
7). moss growing inside crack — organic growth in the gap
8). collapsed upper corner: top strip partially broken, debris pixels
9). two parallel vertical cracks, block segment displaced
10). lower corner fully broken off, large debris at base
11). heavy moss spreading across 50% of front face
12). deep structural gap: dark interior void visible through crack
13). multiple cracks + moss + scorch combined
14). top strip fully cracked through, stone cap pieces displaced
15). large section collapsed inward: hole shape in wall face, dark void behind
16). worst damage: structural failure, multiple voids, debris, moss — barely standing
