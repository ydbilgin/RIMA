"""
Empirical sırıtma test: paste RIMA Warblade chibi onto Codex's painterly concept_art_v1 room.
Tests user's hypothesis: "characters won't stand out on painterly rooms"
"""
import os
from PIL import Image

REPO = r"F:/Antigravity Projeler/2d roguelite/RIMA"
ROOM = os.path.join(REPO, "STAGING/concept_art_rima_sample_room.png")
CHAR = os.path.join(REPO, "ANCHORS/characters/01_warblade.png")
OUT = os.path.join(REPO, "STAGING/test_warblade_on_painterly_room.png")

room = Image.open(ROOM).convert("RGBA")
char = Image.open(CHAR).convert("RGBA")

print(f"Room: {room.size}, Char: {char.size}")

# Scale char up to be realistic in this room
# Room is 1700+ wide, char is 64. Probably need ~128-192 char size to read.
char_scaled = char.resize((char.width * 3, char.height * 3), Image.NEAREST)  # 64 -> 192 hard pixel
print(f"Char scaled (3x nearest): {char_scaled.size}")

# Place chars at multiple positions to test:
# 1. Where Codex chibi already is (center-left)
# 2. Right side near rift
# 3. Top-right (clean spot)
canvas = room.copy()

# Char1: bottom center (replacing where Codex char was)
x1 = room.width // 2 - char_scaled.width // 2 - 50
y1 = room.height // 2 - 30
canvas.paste(char_scaled, (x1, y1), char_scaled)

# Char2: right of rift scar
x2 = int(room.width * 0.72)
y2 = int(room.height * 0.55)
canvas.paste(char_scaled, (x2, y2), char_scaled)

# Char3: top-right clean area
x3 = int(room.width * 0.78)
y3 = int(room.height * 0.18)
canvas.paste(char_scaled, (x3, y3), char_scaled)

canvas.save(OUT)
print(f"Saved: {OUT} ({canvas.size})")
print("DONE")
