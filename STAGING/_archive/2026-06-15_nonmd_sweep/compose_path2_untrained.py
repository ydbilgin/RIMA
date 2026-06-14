"""
Path 2 — Pure untrained Kenney/0x72 sample room compose.
Uses ONLY existing CC0 16x16 native tiles, no AI generation.
Output: STAGING/room_compare_path2_untrained.png
"""
import os
import random
from PIL import Image

random.seed(42)  # deterministic compose

REPO = r"F:/Antigravity Projeler/2d roguelite/RIMA"
REFS = r"F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/codex"
OUT_RAW = os.path.join(REPO, "STAGING/room_compare_path2_untrained_raw.png")
OUT_4X = os.path.join(REPO, "STAGING/room_compare_path2_untrained.png")
OUT_SPLIT = os.path.join(REPO, "Assets/Sprites/Environment/Untrained_Path2/")
os.makedirs(OUT_SPLIT, exist_ok=True)
os.makedirs(os.path.join(OUT_SPLIT, "floor"), exist_ok=True)
os.makedirs(os.path.join(OUT_SPLIT, "walls"), exist_ok=True)

# === LOAD 0x72 atlas ===
floor_atlas = Image.open(os.path.join(REFS, "0x72_dungeon_tileset_ii", "0x72_DungeonTilesetII_v1.7_atlas_floor-16x16.png")).convert("RGBA")
walls_atlas = Image.open(os.path.join(REFS, "0x72_dungeon_tileset_ii", "0x72_DungeonTilesetII_v1.7_atlas_walls_low-16x16.png")).convert("RGBA")

print(f"Floor atlas: {floor_atlas.size}, Walls atlas: {walls_atlas.size}")

# === SPLIT into 16x16 tiles ===
TILE = 16

def split_atlas(atlas, prefix, out_dir):
    cols = atlas.width // TILE
    rows = atlas.height // TILE
    tiles = []
    for r in range(rows):
        for c in range(cols):
            tile = atlas.crop((c*TILE, r*TILE, (c+1)*TILE, (r+1)*TILE))
            # skip fully transparent
            if tile.getbbox() is None:
                continue
            name = f"{prefix}_r{r}_c{c}.png"
            tile.save(os.path.join(out_dir, name))
            tiles.append((r, c, tile))
    print(f"{prefix}: split {len(tiles)} tiles ({cols}x{rows} grid)")
    return tiles

floor_tiles = split_atlas(floor_atlas, "floor", os.path.join(OUT_SPLIT, "floor"))
wall_tiles = split_atlas(walls_atlas, "wall", os.path.join(OUT_SPLIT, "walls"))

# === SELECT usable tiles ===
# 0x72 floor atlas: top-left ~3x3 are main floor variants. Cells (0,0) (0,1) (0,2) (1,0) (1,1) (1,2)
# top-right column has some good stones
GOOD_FLOOR_COORDS = [(0,0), (0,1), (0,2), (1,0), (1,1), (1,2), (0,3), (1,3), (2,3)]
good_floor = [t for (r,c,t) in floor_tiles if (r,c) in GOOD_FLOOR_COORDS]
print(f"Selected {len(good_floor)} good floor variants from 0x72")

# 0x72 walls atlas 12x4 grid — pick from various positions for variety
# row 0: top wall pieces, row 1: middle wall, row 2: bottom wall, row 3: floor edges
# Pick a few clean ones
GOOD_WALL_COORDS = {
    'top':       [(0,0), (0,1), (0,2), (0,3)],   # top wall horizontal run
    'bottom':    [(2,0), (2,1), (2,2), (2,3)],   # bottom wall
    'side_left': [(1,4), (1,5)],
    'side_right':[(1,6), (1,7)],
    'corner_tl': [(0,8)],
    'corner_tr': [(0,9)],
    'corner_bl': [(2,8)],
    'corner_br': [(2,9)],
}
wall_by_type = {}
for typ, coords in GOOD_WALL_COORDS.items():
    wall_by_type[typ] = []
    for (r, c) in coords:
        for (rr, cc, t) in wall_tiles:
            if rr == r and cc == c:
                wall_by_type[typ].append(t)
                break

# Fallback — any non-empty wall
all_walls = [t for (r,c,t) in wall_tiles]

# === COMPOSE ROOM ===
ROOM_W = 24   # tiles wide
ROOM_H = 16   # tiles tall
canvas = Image.new("RGBA", (ROOM_W * TILE, ROOM_H * TILE), (0, 0, 0, 255))

# Fill floor (random from good_floor)
for ty in range(1, ROOM_H - 1):  # leave 1 row top/bottom for wall
    for tx in range(1, ROOM_W - 1):  # leave 1 col for side wall
        f = random.choice(good_floor) if good_floor else None
        if f:
            canvas.paste(f, (tx * TILE, ty * TILE), f)

# Top wall row
for tx in range(1, ROOM_W - 1):
    w = random.choice(wall_by_type.get('top', all_walls[:5]))
    canvas.paste(w, (tx * TILE, 0 * TILE), w)
# Bottom wall row
for tx in range(1, ROOM_W - 1):
    w = random.choice(wall_by_type.get('bottom', all_walls[:5]))
    canvas.paste(w, (tx * TILE, (ROOM_H - 1) * TILE), w)
# Left wall col
for ty in range(1, ROOM_H - 1):
    w = random.choice(wall_by_type.get('side_left', all_walls[:5]))
    canvas.paste(w, (0, ty * TILE), w)
# Right wall col
for ty in range(1, ROOM_H - 1):
    w = random.choice(wall_by_type.get('side_right', all_walls[:5]))
    canvas.paste(w, ((ROOM_W - 1) * TILE, ty * TILE), w)
# Corners
if wall_by_type.get('corner_tl'):
    canvas.paste(wall_by_type['corner_tl'][0], (0, 0), wall_by_type['corner_tl'][0])
if wall_by_type.get('corner_tr'):
    canvas.paste(wall_by_type['corner_tr'][0], ((ROOM_W - 1) * TILE, 0), wall_by_type['corner_tr'][0])
if wall_by_type.get('corner_bl'):
    canvas.paste(wall_by_type['corner_bl'][0], (0, (ROOM_H - 1) * TILE), wall_by_type['corner_bl'][0])
if wall_by_type.get('corner_br'):
    canvas.paste(wall_by_type['corner_br'][0], ((ROOM_W - 1) * TILE, (ROOM_H - 1) * TILE), wall_by_type['corner_br'][0])

# Save raw 16px
canvas.save(OUT_RAW)
print(f"Saved raw 16px: {OUT_RAW} ({canvas.size})")

# === ADD RIMA character anchor for scale reference ===
char_path = os.path.join(REPO, "ANCHORS/characters/01_warblade.png")
if os.path.exists(char_path):
    char = Image.open(char_path).convert("RGBA")
    print(f"Char anchor: {char.size}")
    # RIMA char is 64x64 native. Tiles are 16x16 native. To put them on same canvas,
    # we upscale tiles to match char scale (16 -> 64 = 4x)
    # OR scale char DOWN to 16x16 (not informative, char will be tiny)
    # BEST: upscale entire compose 4x nearest, then paste char at native 64x64 size
    pass

# === UPSCALE 4x nearest for display + scale match with 64px RIMA chars ===
scaled = canvas.resize((canvas.width * 4, canvas.height * 4), Image.NEAREST)

# Paste char anchor in center of upscaled room
if os.path.exists(char_path):
    char = Image.open(char_path).convert("RGBA")
    # char is 64x64; place center
    cx = (scaled.width - char.width) // 2
    cy = (scaled.height - char.height) // 2
    scaled.paste(char, (cx, cy), char)
    print(f"Pasted char at ({cx}, {cy})")

scaled.save(OUT_4X)
print(f"Saved 4x upscaled with char: {OUT_4X} ({scaled.size})")
print("DONE")
