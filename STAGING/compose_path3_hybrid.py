"""
Path 3 — Hybrid: 0x72 walls (pixel discipline) + Codex Test v2 floor (painterly atmosphere)
Output: STAGING/room_compare_path3_hybrid.png
"""
import os
import random
from PIL import Image

random.seed(42)

REPO = r"F:/Antigravity Projeler/2d roguelite/RIMA"
CODEX_TILES = os.path.join(REPO, "Assets/Sprites/Environment/Codex_Test_v2/tiles")
SPLIT_0X72 = os.path.join(REPO, "Assets/Sprites/Environment/Untrained_Path2/walls")
OUT = os.path.join(REPO, "STAGING/room_compare_path3_hybrid.png")

# === LOAD Codex floor tiles (painterly, 128x128 each) ===
codex_floors = []
for fname in sorted(os.listdir(CODEX_TILES)):
    if fname.endswith(".png"):
        im = Image.open(os.path.join(CODEX_TILES, fname)).convert("RGBA")
        # Resize 128 -> 64 for tile scale (RIMA spec)
        im_64 = im.resize((64, 64), Image.LANCZOS)  # better quality downsample
        codex_floors.append(im_64)
        print(f"  Codex floor: {fname} 128->64")
print(f"Loaded {len(codex_floors)} Codex floor tiles (downsampled to 64x64)")

# === LOAD 0x72 walls (16x16 native, upscaled 4x = 64x64) ===
def load_wall(row, col):
    path = os.path.join(SPLIT_0X72, f"wall_r{row}_c{col}.png")
    if os.path.exists(path):
        im = Image.open(path).convert("RGBA")
        return im.resize((64, 64), Image.NEAREST)  # nearest for pixel discipline
    return None

# Wall layout selection from 0x72 walls atlas
wall_top = [load_wall(0, c) for c in range(8) if load_wall(0, c)]
wall_bottom = [load_wall(2, c) for c in range(8) if load_wall(2, c)]
wall_left = [load_wall(1, 4), load_wall(1, 5)]
wall_left = [w for w in wall_left if w]
wall_right = [load_wall(1, 6), load_wall(1, 7)]
wall_right = [w for w in wall_right if w]
wall_corner_tl = load_wall(0, 8)
wall_corner_tr = load_wall(0, 9)
wall_corner_bl = load_wall(2, 8)
wall_corner_br = load_wall(2, 9)

# === COMPOSE 64px tile room ===
TILE = 64
ROOM_W = 16   # tiles wide
ROOM_H = 12   # tiles tall
canvas = Image.new("RGBA", (ROOM_W * TILE, ROOM_H * TILE), (15, 12, 10, 255))

# Floor — random Codex floor tile per cell
for ty in range(1, ROOM_H - 1):
    for tx in range(1, ROOM_W - 1):
        f = random.choice(codex_floors)
        canvas.paste(f, (tx * TILE, ty * TILE), f)

# Walls
for tx in range(1, ROOM_W - 1):
    if wall_top:
        canvas.paste(random.choice(wall_top), (tx * TILE, 0), random.choice(wall_top))
    if wall_bottom:
        w = random.choice(wall_bottom)
        canvas.paste(w, (tx * TILE, (ROOM_H - 1) * TILE), w)
for ty in range(1, ROOM_H - 1):
    if wall_left:
        w = random.choice(wall_left)
        canvas.paste(w, (0, ty * TILE), w)
    if wall_right:
        w = random.choice(wall_right)
        canvas.paste(w, ((ROOM_W - 1) * TILE, ty * TILE), w)
if wall_corner_tl: canvas.paste(wall_corner_tl, (0, 0), wall_corner_tl)
if wall_corner_tr: canvas.paste(wall_corner_tr, ((ROOM_W - 1) * TILE, 0), wall_corner_tr)
if wall_corner_bl: canvas.paste(wall_corner_bl, (0, (ROOM_H - 1) * TILE), wall_corner_bl)
if wall_corner_br: canvas.paste(wall_corner_br, ((ROOM_W - 1) * TILE, (ROOM_H - 1) * TILE), wall_corner_br)

# === PASTE RIMA character anchor ===
char_path = os.path.join(REPO, "ANCHORS/characters/01_warblade.png")
if os.path.exists(char_path):
    char = Image.open(char_path).convert("RGBA")
    cx = (canvas.width - char.width) // 2
    cy = (canvas.height - char.height) // 2
    canvas.paste(char, (cx, cy), char)

canvas.save(OUT)
print(f"Saved: {OUT} ({canvas.size})")
print("DONE")
