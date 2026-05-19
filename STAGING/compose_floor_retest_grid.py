"""Compose floor retest tiles into a single inspection grid + tilemap test."""
from pathlib import Path
from PIL import Image

SRC = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/Phase0_FloorRetest")
OUT_GRID = SRC / "_grid_inspection.png"
OUT_TILED = SRC / "_tilemap_test.png"

tiles = []
for i in range(16):
    p = SRC / f"tile_{i}.png"
    if p.exists():
        tiles.append(Image.open(p).convert("RGBA"))

print(f"Loaded {len(tiles)} tiles, size {tiles[0].size}")

# Inspection grid: 4x4 at 4x scale for visibility
scale = 4
tw = tiles[0].width * scale
canvas = Image.new("RGBA", (tw * 4 + 30, tw * 4 + 30), (24, 22, 28, 255))
for i, t in enumerate(tiles):
    r, c = divmod(i, 4)
    big = t.resize((tw, tw), Image.NEAREST)
    canvas.paste(big, (c * tw + 6 * (c + 1), r * tw + 6 * (r + 1)), big)
canvas.save(OUT_GRID)
print(f"Wrote {OUT_GRID}")

# Tilemap test: 12x8 tiles tiled with random selection from pool, 4x scaled per tile
import random
random.seed(7)
cols, rows = 12, 8
tile_native = tiles[0].width
tilemap = Image.new("RGBA", (cols * tile_native * scale, rows * tile_native * scale), (24, 22, 28, 255))
for r in range(rows):
    for c in range(cols):
        t = random.choice(tiles)
        big = t.resize((tile_native * scale, tile_native * scale), Image.NEAREST)
        tilemap.paste(big, (c * tile_native * scale, r * tile_native * scale), big)
tilemap.save(OUT_TILED)
print(f"Wrote {OUT_TILED}")
