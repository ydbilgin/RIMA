"""Build a 2x3 contact sheet of the 6 Act-1 room captures, with labels."""
import os
from PIL import Image, ImageDraw, ImageFont

BASE = r"F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\report\figures_2026-06-18"
ROOMS_DIR = os.path.join(BASE, "rooms")
OUT = os.path.join(BASE, "fig_rooms_grid.png")

# (filename, display_name, "WxH", room_type)
ROOMS = [
    ("act1_entry_hall.png",       "Entry Hall",                  "32x24", "spawn"),
    ("act1_west_chamber.png",     "West Chamber",                "24x18", "combat"),
    ("act1_east_corridor.png",    "East Corridor",               "8x24",  "corridor"),
    ("act1_treasure_vault.png",   "Treasure Vault",              "16x12", "treasure"),
    ("act1_north_antechamber.png","North Antechamber",           "20x16", "shrine"),
    ("act1_shattered_throne.png", "The Shattered Throne",        "40x30", "boss"),
]

COLS, ROWS = 3, 2
CELL_W, CELL_H = 600, 360          # thumbnail draw area
LABEL_H = 64                       # caption strip under each thumb
PAD = 28                           # outer + inter-cell padding
BG = (28, 30, 35)                  # slate dark
CARD_BG = (18, 19, 22)             # thumbnail backdrop
LABEL_BG = (40, 43, 50)
TITLE_COLOR = (236, 238, 242)
SUB_COLOR = (150, 200, 215)        # cyan-ish accent for size/type

def load_font(size, bold=False):
    candidates = [
        r"C:\Windows\Fonts\segoeuib.ttf" if bold else r"C:\Windows\Fonts\segoeui.ttf",
        r"C:\Windows\Fonts\arialbd.ttf" if bold else r"C:\Windows\Fonts\arial.ttf",
    ]
    for c in candidates:
        if os.path.exists(c):
            try:
                return ImageFont.truetype(c, size)
            except Exception:
                pass
    return ImageFont.load_default()

font_title = load_font(26, bold=True)
font_sub = load_font(19, bold=False)
font_header = load_font(30, bold=True)

HEADER_H = 70
cell_total_w = CELL_W
cell_total_h = CELL_H + LABEL_H

grid_w = COLS * cell_total_w + (COLS + 1) * PAD
grid_h = HEADER_H + ROWS * cell_total_h + (ROWS + 1) * PAD

sheet = Image.new("RGB", (grid_w, grid_h), BG)
draw = ImageDraw.Draw(sheet)

# Header
header_text = "RIMA — Act 1: Shattered Keep   |   Data-Driven Room Layouts (JSON -> Tilemap)"
draw.text((PAD, 22), header_text, font=font_header, fill=TITLE_COLOR)

def fit_thumb(img, box_w, box_h):
    iw, ih = img.size
    scale = min(box_w / iw, box_h / ih)
    nw, nh = max(1, int(iw * scale)), max(1, int(ih * scale))
    return img.resize((nw, nh), Image.LANCZOS), nw, nh

for idx, (fname, name, size_lbl, rtype) in enumerate(ROOMS):
    col = idx % COLS
    row = idx // COLS
    x0 = PAD + col * (cell_total_w + PAD)
    y0 = HEADER_H + PAD + row * (cell_total_h + PAD)

    # Thumbnail backdrop card
    draw.rectangle([x0, y0, x0 + CELL_W, y0 + CELL_H], fill=CARD_BG)

    path = os.path.join(ROOMS_DIR, fname)
    img = Image.open(path).convert("RGB")
    thumb, tw, th = fit_thumb(img, CELL_W - 8, CELL_H - 8)
    tx = x0 + (CELL_W - tw) // 2
    ty = y0 + (CELL_H - th) // 2
    sheet.paste(thumb, (tx, ty))

    # Label strip
    ly0 = y0 + CELL_H
    draw.rectangle([x0, ly0, x0 + CELL_W, ly0 + LABEL_H], fill=LABEL_BG)
    draw.text((x0 + 14, ly0 + 8), name, font=font_title, fill=TITLE_COLOR)
    draw.text((x0 + 14, ly0 + 38), f"{size_lbl} tiles  -  {rtype}", font=font_sub, fill=SUB_COLOR)

sheet.save(OUT)
print("Saved:", OUT, sheet.size)
