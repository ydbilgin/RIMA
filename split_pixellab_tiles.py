"""Split PixelLab Create Tiles PRO output into individual 64x64 tiles."""
from PIL import Image
import os, shutil

src = r"C:\Users\ydbil\Downloads\pixellab-1-dark-dungeon-stone-floor--cr-1779131555652.png"
dst_dir = r"F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Art\TerrainBlend"
os.makedirs(dst_dir, exist_ok=True)

img = Image.open(src)
w, h = img.size
cols = 4
rows = 4
tw = w // cols  # tile width
th = h // rows  # tile height

print(f"Image: {w}x{h}, Grid: {cols}x{rows}, Tile: {tw}x{th}")

# Grid layout (4x4):
# Row 0: stone variants (tiles 0-3)
# Row 1: grass variants (tiles 4-7)
# Row 2: dirt variants (tiles 8-11)
# Row 3: moss variants (tiles 12-15)

# Extract all tiles first
tiles = []
for row in range(rows):
    for col in range(cols):
        x = col * tw
        y = row * th
        tile = img.crop((x, y, x + tw, y + th))
        tiles.append(tile)
        # Save numbered version for reference
        tile.save(os.path.join(dst_dir, f"tile_{row}_{col}.png"))

# Pick best tile from each row:
# Row 0 (stone): col 0 looks best - clean cracked stone
# Row 1 (grass): col 0 looks best - clean dark grass
# Row 2 (dirt):  col 0 looks best - clean brown dirt
# Row 3 (moss):  col 1 looks best - mossy stone

selections = {
    "stone_floor.png":  (0, 0),  # Row 0, Col 0
    "grass_ground.png": (1, 0),  # Row 1, Col 0
    "dirt_path.png":    (2, 0),  # Row 2, Col 0
    "moss_ground.png":  (3, 1),  # Row 3, Col 1
}

for name, (row, col) in selections.items():
    tile = tiles[row * cols + col]
    # Resize to exact 64x64 if needed
    if tile.size != (64, 64):
        tile = tile.resize((64, 64), Image.NEAREST)
    path = os.path.join(dst_dir, name)
    tile.save(path)
    print(f"Saved: {name} from ({row},{col}) -> {path}")

# Clean up numbered tiles
for row in range(rows):
    for col in range(cols):
        p = os.path.join(dst_dir, f"tile_{row}_{col}.png")
        if os.path.exists(p):
            os.remove(p)

print("\nDone! 4 terrain tiles saved to Assets/Art/TerrainBlend/")
