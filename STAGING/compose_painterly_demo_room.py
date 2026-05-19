"""
Demo composite for RIMA Painterly Pack v1 — all 50 sprites in sample dungeon room.
Layout matches concept_art_v1 style: floor grid + perimeter walls + decals + accent + props + RIMA char.
"""
import os
import random
from PIL import Image

random.seed(7)  # consistent layout

REPO = r"F:/Antigravity Projeler/2d roguelite/RIMA"
PACK = os.path.join(REPO, "Assets/Sprites/Environment/RIMA_Painterly_Pack_v1")
CHAR = os.path.join(REPO, "ANCHORS/characters/01_warblade.png")
OUT = os.path.join(REPO, "STAGING/painterly_pack_v1_demo_room.png")

TILE = 128

def load(cat, name):
    p = os.path.join(PACK, cat, name)
    return Image.open(p).convert("RGBA") if os.path.exists(p) else None

# Floor tiles
floor_tiles = [load("floor", f"floor_{i:02d}_{n}.png") for i, n in [
    (1, "clean"), (2, "mossy"), (3, "cracked"), (4, "worn"),
    (5, "stained"), (6, "rift_touched"), (7, "dirt"), (8, "blood_old")
]]
floor_tiles = [f for f in floor_tiles if f]

# Wall tiles
walls = {
    'tl': load("walls", "wall_01_top_left_corner.png"),
    'top': load("walls", "wall_02_top_edge.png"),
    'tr': load("walls", "wall_03_top_right_corner.png"),
    'left': load("walls", "wall_04_left_edge.png"),
    'right': load("walls", "wall_05_right_edge.png"),
    'bl': load("walls", "wall_06_bottom_left_corner.png"),
    'bot': load("walls", "wall_07_bottom_edge.png"),
    'br': load("walls", "wall_08_bottom_right_corner.png"),
}

# Room dims: 10 wide x 7 tall (floor area)
ROOM_W = 10
ROOM_H = 7
CANVAS_W = (ROOM_W + 0) * TILE
CANVAS_H = (ROOM_H + 1) * TILE  # +1 for wall top cap visibility

canvas = Image.new("RGBA", (CANVAS_W, CANVAS_H), (15, 12, 10, 255))

# Floor grid (random per cell, ~70% clean variants to reduce visual chaos)
for ty in range(1, ROOM_H):
    for tx in range(0, ROOM_W):
        f = random.choice(floor_tiles)
        canvas.paste(f, (tx * TILE, ty * TILE), f if f.mode == 'RGBA' else None)

# Walls perimeter
# Walls are 128x192 — top cap visible above the y position
# Top wall row at y=0
WALL_Y_OFFSET = -64  # walls have top cap, so place y = tile_y * TILE - 64
for tx in range(0, ROOM_W):
    w = walls['top']
    canvas.paste(w, (tx * TILE, 0), w)
# Top corners
if walls['tl']: canvas.paste(walls['tl'], (0, 0), walls['tl'])
if walls['tr']: canvas.paste(walls['tr'], ((ROOM_W - 1) * TILE, 0), walls['tr'])

# Sides
for ty in range(2, ROOM_H):
    if walls['left']:
        canvas.paste(walls['left'], (0, ty * TILE - 64), walls['left'])
    if walls['right']:
        canvas.paste(walls['right'], ((ROOM_W - 1) * TILE, ty * TILE - 64), walls['right'])

# Bottom corners + bottom row
if walls['bot']:
    for tx in range(1, ROOM_W - 1):
        canvas.paste(walls['bot'], (tx * TILE, ROOM_H * TILE - 64), walls['bot'])
if walls['bl']: canvas.paste(walls['bl'], (0, ROOM_H * TILE - 64), walls['bl'])
if walls['br']: canvas.paste(walls['br'], ((ROOM_W - 1) * TILE, ROOM_H * TILE - 64), walls['br'])

# Accent overlay — rift scar center
accent = load("accents", "accent_01_rift_scar.png")
if accent:
    cx = (CANVAS_W - accent.width) // 2 + 100
    cy = (CANVAS_H - accent.height) // 2
    canvas.paste(accent, (cx, cy), accent)

# Decal scatter — random selection placed in floor area
decal_names = [f for f in os.listdir(os.path.join(PACK, "decals")) if f.endswith(".png")]
random.shuffle(decal_names)
for d in decal_names[:7]:
    decal = load("decals", d)
    if decal:
        x = random.randint(TILE, CANVAS_W - TILE - decal.width)
        y = random.randint(TILE + 64, CANVAS_H - TILE - decal.height)
        canvas.paste(decal, (x, y), decal)

# Props
prop_placements = [
    ("prop_01_wooden_crate.png", 200, 300),
    ("prop_03_stone_urn_broken.png", 350, 250),
    ("prop_06_burning_brazier.png", CANVAS_W - 250, CANVAS_H - 350),
    ("prop_05_candle_holder.png", 180, CANVAS_H - 300),
    ("prop_07_hanging_banner_torn.png", CANVAS_W // 2 - 64, 100),
]
for name, x, y in prop_placements:
    prop = load("props", name)
    if prop:
        canvas.paste(prop, (x, y), prop)

# RIMA character — center, scale 2x (64 -> 128) to match tile scale
char = Image.open(CHAR).convert("RGBA")
char_scaled = char.resize((char.width * 2, char.height * 2), Image.NEAREST)
cx = (CANVAS_W - char_scaled.width) // 2
cy = (CANVAS_H - char_scaled.height) // 2 + 50
canvas.paste(char_scaled, (cx, cy), char_scaled)

canvas.save(OUT)
print(f"Saved: {OUT} ({canvas.size})")
print("DONE")
