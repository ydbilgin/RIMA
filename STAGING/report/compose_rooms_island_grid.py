"""
RIMA raporu - Sekil 6: Act-1 oda ada izgarasi (2x3 etiketli montaj).
Re-capture sonrasi (void-renkli arka plan, magenta sizinti yok) 6 oda kapatmasini
orijinal izgaranin layout/etiket stiliyle birebir yeniden birlestirir.

Layout (orijinalden olculdu): canvas 1632x782, gutter/bg (24,22,30),
3 sutun x 2 satir, panel genisligi 520 (gorsel 16:9 = 520x292), etiket bandi altta,
isim = off-white (204,208,218), boyut = teal (116,164,173).
Pikseli korumak icin gorseller NEAREST yerine LANCZOS (zaten yuksek-cozunurluk kaynak).
"""
from PIL import Image, ImageDraw, ImageFont
import os

BASE = "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/report/figures_2026-06-18"
ROOMS = os.path.join(BASE, "rooms_island")
OUT = os.path.join(BASE, "fig_rooms_island_grid.png")

# (dosya, isim, boyut-etiketi)
PANELS = [
    ("act1_entry_hall.png",       "Entry Hall",       "32x24"),
    ("act1_west_chamber.png",     "West Chamber",     "24x18"),
    ("act1_east_corridor.png",    "East Corridor",    "8x24"),
    ("act1_treasure_vault.png",   "Treasure Vault",   "16x12"),
    ("act1_north_antechamber.png","North Antechamber","20x16"),
    ("act1_shattered_throne.png", "Shattered Throne",  "6x10"),
]

CANVAS_W, CANVAS_H = 1632, 782
BG = (24, 22, 30)
NAME_COL = (204, 208, 218)
SIZE_COL = (116, 164, 173)

MARGIN = 18          # disa + sutunlar/satirlar arasi bosluk
COLS, ROWS = 3, 2
PANEL_W = (CANVAS_W - MARGIN * (COLS + 1)) // COLS   # ~520
PANEL_BLOCK_H = (CANVAS_H - MARGIN * (ROWS + 1)) // ROWS  # ~364
IMG_W = PANEL_W
IMG_H = round(IMG_W * 9 / 16)                         # 292
LABEL_H = PANEL_BLOCK_H - IMG_H                       # ~72

def load_font(size, bold=False):
    paths = [
        "C:/Windows/Fonts/" + ("arialbd.ttf" if bold else "arial.ttf"),
        "C:/Windows/Fonts/calibri.ttf",
    ]
    for p in paths:
        if os.path.exists(p):
            return ImageFont.truetype(p, size)
    return ImageFont.load_default()

NAME_FONT = load_font(26)
SIZE_FONT = load_font(20)

canvas = Image.new("RGB", (CANVAS_W, CANVAS_H), BG)
draw = ImageDraw.Draw(canvas)

for idx, (fname, name, size) in enumerate(PANELS):
    r, c = divmod(idx, COLS)
    px = MARGIN + c * (PANEL_W + MARGIN)
    py = MARGIN + r * (PANEL_BLOCK_H + MARGIN)

    img = Image.open(os.path.join(ROOMS, fname)).convert("RGB").resize((IMG_W, IMG_H), Image.LANCZOS)
    canvas.paste(img, (px, py))

    # etiket bandi: isim (off-white) + bosluk + boyut (teal), yatayda ortali
    gap = 14
    nb = draw.textbbox((0, 0), name, font=NAME_FONT)
    sb = draw.textbbox((0, 0), size, font=SIZE_FONT)
    nw, nh = nb[2] - nb[0], nb[3] - nb[1]
    sw, sh = sb[2] - sb[0], sb[3] - sb[1]
    total = nw + gap + sw
    tx = px + (PANEL_W - total) // 2
    ty = py + IMG_H + (LABEL_H - nh) // 2 - nb[1]
    draw.text((tx, ty), name, fill=NAME_COL, font=NAME_FONT)
    # boyut etiketini ismin tabanina hizala
    sty = py + IMG_H + (LABEL_H - sh) // 2 - sb[1]
    draw.text((tx + nw + gap, sty), size, fill=SIZE_COL, font=SIZE_FONT)

canvas.save(OUT)
print("wrote", OUT, canvas.size)
