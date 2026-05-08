# PROMPT — F2 Cracked Floor
# Status: APPROVED (2026-05-08) — ChatGPT üretimi onaylandı
# Output: 1024×1024 | Grid: 4×4 | Tile: 64×64 | Count: 16
# Slice: python STAGING/process_tiles.py --source "<file>" --output "Assets/Art/Tiles/Act1/F2" --cols 4 --rows 4 --width 64 --height 64 --prefix "f2_"
# Style anchor: STAGING/tiles_raw/ChatGPT Image 8 May 2026 01_47_54 (1).png
# IMPORTANT: Attach style anchor image to ChatGPT before sending this prompt.
# NOTE: Tile 13-16 (row 4) = moloz/rubble — NOT for Random Tile pool, use as prop overlay only.

---

Create a pixel art isometric dungeon floor tile sprite sheet.

OUTPUT SIZE: 1024×1024 square image.
GRID: 4 columns × 4 rows = 16 tiles, zero gaps, perfectly equal sizing.
Each cell is exactly 256×256 pixels in the output.

BACKGROUND: Solid magenta #FF00FF outside all diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat magenta only.

PIXEL ART SCALE: Each tile is 64×64 logical pixels. Design pixel clusters at minimum 4×4px — no detail finer than 4px, no sub-pixel features. This image will be downscaled 4× to 64px tiles.

STYLE (match the attached reference image exactly):
- Isometric diamond shape, 2:1 width-to-height ratio
- Hard aliased edges only — NO anti-aliasing, NO smooth gradients, NO 3D rendering
- Top-left light source, consistent across ALL tiles
- Cold blue-grey dungeon stone: main face #1E2030–#262838, lit edge #363A48, shadow #151820
- Mortar joints: #161620
- PALETTE RESTRICTION: use ONLY these colors — stone #1E2030/#262838/#363A48, mortar/crack #161620/#0A0A14, shadow #151820, mineral seepage #1A2810. No other hues.
- All 16 tiles share identical stone color, lighting, and diamond proportions
- Cracks are the dominant feature — stone identity reads first, then damage

TILES:
1). plain worn stone, very subtle surface grain only — almost no cracks
2). single thin hairline crack, one corner to midpoint
3). diagonal crack corner to corner
4). spiderweb fracture radiating from center, 4-5 branches
5). two parallel longitudinal cracks
6). crack through mortar joint continuing across block
7). multiple micro-cracks forming network across two faces
8). corner rubble: 4-6 loose debris pixels at diamond edge
9). single deep crack, dark shadow inside, runs half tile
10). mortar joint fully cracked, raw block edge exposed
11). diagonal crack with small debris cluster at terminus
12). heavy crack with block inset 1px, surface depression
13). crack pattern radiates from corner across full face
14). two crossing cracks, X pattern
15). crack with dark green mineral seepage along one side
16). worst damage: heavy crack network + debris at two corners (use as prop/overlay, not floor tile)
