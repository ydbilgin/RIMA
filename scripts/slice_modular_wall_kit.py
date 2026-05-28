"""
Slice STAGING/concepts/modular_wall_kit_v1.png (512x512 atlas, 4x4 grid of 128x128)
into 16 individual sprite PNGs with proper alpha and tight crop.

Output: STAGING/_modular_kit_extracted_v1/
"""
from PIL import Image
import os, json

SRC = "STAGING/concepts/modular_wall_kit_v1.png"
OUT_DIR = "STAGING/_modular_kit_extracted_v1"
os.makedirs(OUT_DIR, exist_ok=True)

CELL = 128
COLS = 4
ROWS = 4

# Asset ID mapping per cell (row, col) → asset_id + canonical Unity name
LAYOUT = [
    # Row 0
    ("M01", "wall_straight_a"),       # straight wall NE-SW
    ("M02", "wall_straight_b"),       # straight wall NW-SE
    ("M03", "corner_outer_NE"),       # outer corner NE
    ("M04", "corner_outer_NW"),       # outer corner NW
    # Row 1
    ("M05", "corner_inner_NE"),       # inner corner NE
    ("M06", "corner_inner_NW"),       # inner corner NW
    ("M07", "wall_straight_c"),       # variant straight (end cap candidate)
    ("M08", "wall_straight_d"),       # variant straight (end cap candidate)
    # Row 2
    ("M09", "doorway_a"),             # archway NE-SW
    ("M10", "doorway_b"),             # archway NW-SE
    ("M11", "wall_broken_a"),         # broken straight w/ sockets
    ("M12", "wall_broken_b"),         # broken straight w/ sockets
    # Row 3
    ("M13", "junction_cross_a"),      # T-junction or cross
    ("M14", "junction_cross_b"),      # cross-junction
    ("M15", "wall_short_half"),       # short half-wall (foreground edge)
    ("M16", "ruined_corner"),         # ruined corner with sockets
]

img = Image.open(SRC).convert("RGBA")
W, H = img.size
print(f"Atlas: {W}x{H}, expected 512x512")
assert W == 512 and H == 512, f"Atlas dimensions unexpected: {W}x{H}"

manifest = {}
for idx, (asset_id, name) in enumerate(LAYOUT):
    row = idx // COLS
    col = idx % COLS
    x0 = col * CELL
    y0 = row * CELL
    cell = img.crop((x0, y0, x0 + CELL, y0 + CELL))

    # Tight crop to non-transparent bbox (atlas already has alpha)
    bbox = cell.getbbox()
    if bbox:
        cropped = cell.crop(bbox)
    else:
        print(f"WARN: {asset_id} {name} has empty bbox, keeping full cell")
        cropped = cell

    fname = f"{asset_id}_{name}.png"
    out_path = os.path.join(OUT_DIR, fname)
    cropped.save(out_path)
    manifest[asset_id] = {
        "name": name,
        "file": fname,
        "cell_pos": [row, col],
        "raw_cell_size": [CELL, CELL],
        "tight_size": cropped.size,
        "bbox_in_cell": bbox,
    }
    print(f"{asset_id} {name}: cell ({row},{col}) -> {cropped.size}")

with open(os.path.join(OUT_DIR, "_manifest.json"), "w") as f:
    json.dump(manifest, f, indent=2)

print(f"\n[DONE] {len(manifest)} sprites extracted to {OUT_DIR}/")
