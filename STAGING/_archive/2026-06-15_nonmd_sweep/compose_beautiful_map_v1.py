"""
Beautiful zoomed-out map — RIMA_Painterly_Pack_v1 + dense layered compose.
Hades-style atmospheric arena look. Camera zoomed out.
"""
import os
import random
import math
from PIL import Image, ImageDraw

random.seed(42)

REPO = r"F:/Antigravity Projeler/2d roguelite/RIMA"
PACK = os.path.join(REPO, "Assets/Sprites/Environment/RIMA_Painterly_Pack_v1")
CHAR = os.path.join(REPO, "ANCHORS/characters/01_warblade.png")
OUT = os.path.join(REPO, "STAGING/beautiful_map_v1.png")

COLS, ROWS = 16, 10
TILE = 128
W = COLS * TILE
H = ROWS * TILE
print(f"Canvas: {W}x{H}")

def list_pngs(subfolder):
    folder = os.path.join(PACK, subfolder)
    return sorted([f for f in os.listdir(folder) if f.endswith(".png")])

def load(cat, name):
    p = os.path.join(PACK, cat, name)
    return Image.open(p).convert("RGBA") if os.path.exists(p) else None

floor_files = list_pngs("floor")
floors = [load("floor", f) for f in floor_files]
print(f"Floors: {len(floors)}")

wall_files_list = list_pngs("walls")
wall_files = {f: load("walls", f) for f in wall_files_list}
print(f"Walls: {len(wall_files)}")

decal_files = list_pngs("decals")
decals = [load("decals", f) for f in decal_files]
print(f"Decals: {len(decals)}")

accent_files = list_pngs("accents")
accents = [load("accents", f) for f in accent_files]
print(f"Accents: {len(accents)}")

prop_files = list_pngs("props")
props = {f: load("props", f) for f in prop_files}
print(f"Props: {len(props)}")

canvas = Image.new("RGBA", (W, H), (12, 10, 14, 255))

# L2 Floor — random variants, slight jitter, random rotation
print("L2 floor...")
for ty in range(ROWS):
    for tx in range(COLS):
        f = random.choice(floors)
        rot = random.choice([0, 90, 180, 270])
        f_rot = f.rotate(rot)
        jx = random.randint(-2, 2)
        jy = random.randint(-2, 2)
        canvas.paste(f_rot, (tx * TILE + jx, ty * TILE + jy), f_rot)

# L4 Organic decal scatter — DENSE per ChatGPT zone-based density
print("L4 decals (dense scatter)...")
num_decals = 80
for i in range(num_decals):
    d = random.choice(decals)
    if random.random() < 0.7:
        edge_choice = random.choice(["top", "bottom", "left", "right"])
        if edge_choice == "top":
            x = random.randint(TILE, W - TILE)
            y = random.randint(TILE // 2, 3 * TILE)
        elif edge_choice == "bottom":
            x = random.randint(TILE, W - TILE)
            y = random.randint(H - 3 * TILE, H - TILE // 2)
        elif edge_choice == "left":
            x = random.randint(TILE // 2, 3 * TILE)
            y = random.randint(TILE, H - TILE)
        else:
            x = random.randint(W - 3 * TILE, W - TILE // 2)
            y = random.randint(TILE, H - TILE)
    else:
        x = random.randint(2 * TILE, W - 2 * TILE)
        y = random.randint(2 * TILE, H - 2 * TILE)
    rot = random.choice([0, 90, 180, 270])
    scale = 0.6 + random.random() * 0.8
    flip_x = random.random() < 0.3
    d_t = d.rotate(rot, expand=True)
    if flip_x:
        d_t = d_t.transpose(Image.FLIP_LEFT_RIGHT)
    new_w = max(1, int(d_t.width * scale))
    new_h = max(1, int(d_t.height * scale))
    d_t = d_t.resize((new_w, new_h), Image.LANCZOS)
    canvas.paste(d_t, (x - new_w // 2, y - new_h // 2), d_t)

# L6 Accents — 2 hero pieces
print("L6 accents...")
if accents:
    rift = accents[0]
    canvas.paste(rift, (W * 3 // 5 - rift.width // 2, H // 2 - rift.height // 2), rift)
if len(accents) > 3:
    rit = accents[3]
    canvas.paste(rit, (W // 4 - rit.width // 2, H * 2 // 3 - rit.height // 2), rit)

# Walls perimeter
print("Walls perimeter...")
wall_top = wall_files.get("wall_02_top_edge.png")
wall_tl = wall_files.get("wall_01_top_left_corner.png")
wall_tr = wall_files.get("wall_03_top_right_corner.png")
wall_left = wall_files.get("wall_04_left_edge.png")
wall_right = wall_files.get("wall_05_right_edge.png")
wall_bl = wall_files.get("wall_06_bottom_left_corner.png")
wall_bot = wall_files.get("wall_07_bottom_edge.png")
wall_br = wall_files.get("wall_08_bottom_right_corner.png")

WALL_CAP_OFFSET = 64

if wall_tl: canvas.paste(wall_tl, (0, -WALL_CAP_OFFSET), wall_tl)
if wall_top:
    for tx in range(1, COLS - 1):
        canvas.paste(wall_top, (tx * TILE, -WALL_CAP_OFFSET), wall_top)
if wall_tr: canvas.paste(wall_tr, ((COLS - 1) * TILE, -WALL_CAP_OFFSET), wall_tr)

for ty in range(1, ROWS - 1):
    if wall_left: canvas.paste(wall_left, (0, ty * TILE - WALL_CAP_OFFSET), wall_left)
    if wall_right: canvas.paste(wall_right, ((COLS - 1) * TILE, ty * TILE - WALL_CAP_OFFSET), wall_right)

if wall_bl: canvas.paste(wall_bl, (0, (ROWS - 1) * TILE - WALL_CAP_OFFSET), wall_bl)
if wall_bot:
    for tx in range(1, COLS - 1):
        canvas.paste(wall_bot, (tx * TILE, (ROWS - 1) * TILE - WALL_CAP_OFFSET), wall_bot)
if wall_br: canvas.paste(wall_br, ((COLS - 1) * TILE, (ROWS - 1) * TILE - WALL_CAP_OFFSET), wall_br)

# Props — strategic placement
print("Props placement...")
prop_placements = [
    ("prop_07_hanging_banner_torn.png", W * 0.30, TILE * 0.6),
    ("prop_07_hanging_banner_torn.png", W * 0.70, TILE * 0.6),
    ("prop_01_wooden_crate.png", TILE * 2.5, TILE * 2.5),
    ("prop_03_stone_urn_broken.png", TILE * 3.5, TILE * 2.2),
    ("prop_12_statue_torso.png", W - TILE * 3.5, TILE * 2.5),
    ("prop_06_burning_brazier.png", W - TILE * 2.5, H * 0.5),
    ("prop_05_candle_holder.png", TILE * 1.8, H * 0.5),
    ("prop_05_candle_holder.png", TILE * 1.8, H * 0.7),
    ("prop_08_stone_column.png", TILE * 2.5, H - TILE * 2.5),
    ("prop_09_stone_pillar_broken.png", W - TILE * 4, H - TILE * 2.5),
    ("prop_10_chest_closed.png", W - TILE * 2.5, H - TILE * 3),
    ("prop_04_barrel.png", TILE * 4, H - TILE * 3.5),
    ("prop_02_stone_urn_intact.png", W * 0.42, H * 0.7),
]
for name, x, y in prop_placements:
    p = props.get(name)
    if p:
        canvas.paste(p, (int(x - p.width / 2), int(y - p.height / 2)), p)

# Character — 2x scale for visibility in zoomed-out view
print("Character...")
char = Image.open(CHAR).convert("RGBA")
char_scaled = char.resize((char.width * 2, char.height * 2), Image.NEAREST)
canvas.paste(char_scaled, (W // 2 - char_scaled.width // 2, H // 2 - char_scaled.height // 2), char_scaled)

# Vignette overlay — radial dark corners
print("Vignette...")
cx_v, cy_v = W // 2, H // 2
max_radius = math.sqrt(cx_v ** 2 + cy_v ** 2)
vignette = Image.new("RGBA", (W, H), (0, 0, 0, 0))
for x in range(0, W, 4):
    for y in range(0, H, 4):
        dx = (x - cx_v) / cx_v
        dy = (y - cy_v) / cy_v
        d = math.sqrt(dx ** 2 + dy ** 2)
        if d > 0.55:
            alpha = int(min(180, ((d - 0.55) * 2.2) ** 2 * 240))
            for yy in range(y, min(y + 4, H)):
                for xx in range(x, min(x + 4, W)):
                    vignette.putpixel((xx, yy), (4, 3, 6, alpha))

canvas = Image.alpha_composite(canvas, vignette)

canvas.save(OUT)
print(f"Saved: {OUT} ({canvas.size})")
