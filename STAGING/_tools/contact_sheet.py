# -*- coding: utf-8 -*-
"""Compose all S6 placeholder PNGs into one labeled contact sheet over a checkerboard."""
import os
from PIL import Image, ImageDraw, ImageFont

ROOT = r"F:\Antigravity Projeler\2d roguelite\RIMA"
UI   = os.path.join(ROOT, "Assets", "Resources", "UI", "RIMA")
MOB  = os.path.join(ROOT, "Assets", "Sprites", "Mobs", "_Placeholders")

FILES = [
    (UI,"menu_bg_island.png"),(UI,"victory_bg_bloom.png"),(UI,"death_overlay.png"),
    (UI,"lowhp_vignette.png"),(UI,"logo_rima_glyph.png"),(UI,"next_class_silhouette.png"),
    (MOB,"PenitentSovereign_placeholder.png"),(UI,"card_frame_stone.png"),(UI,"card_select_flash.png"),
    (UI,"wishlist_cta_btn.png"),(UI,"wishlist_cta_btn_lg.png"),(UI,"minimap_frame_stone.png"),
    (UI,"icon_frame_hex.png"),(UI,"hex_slot_mask.png"),(UI,"banner_underline_rune.png"),
    (UI,"rarity_glow_common.png"),(UI,"rarity_glow_rare.png"),(UI,"rarity_glow_epic.png"),
    (UI,"rarity_glow_legendary.png"),(UI,"boss_skull_glyph.png"),(UI,"steam_glyph_cyan.png"),
    (UI,"particle_ember.png"),(UI,"dust_mote.png"),
]

CELL_W, CELL_H, PAD, LABEL_H = 240, 150, 10, 16
COLS = 5
rows = (len(FILES) + COLS - 1) // COLS
SHEET_W = COLS * (CELL_W + PAD) + PAD
SHEET_H = rows * (CELL_H + LABEL_H + PAD) + PAD

def checker(w, h, sz=8, a=(70,70,80), b=(96,96,108)):
    img = Image.new("RGBA", (w, h), a + (255,))
    d = ImageDraw.Draw(img)
    for y in range(0, h, sz):
        for x in range(0, w, sz):
            if (x//sz + y//sz) % 2:
                d.rectangle([x, y, x+sz-1, y+sz-1], fill=b + (255,))
    return img

sheet = Image.new("RGBA", (SHEET_W, SHEET_H), (24, 22, 30, 255))
d = ImageDraw.Draw(sheet)
font = None
for fp in [r"C:\Windows\Fonts\consola.ttf", r"C:\Windows\Fonts\arial.ttf"]:
    if os.path.exists(fp):
        font = ImageFont.truetype(fp, 11); break
if font is None: font = ImageFont.load_default()

for i, (folder, name) in enumerate(FILES):
    r, c = divmod(i, COLS)
    x0 = PAD + c * (CELL_W + PAD)
    y0 = PAD + r * (CELL_H + LABEL_H + PAD)
    cell = checker(CELL_W, CELL_H)
    p = os.path.join(folder, name)
    if os.path.exists(p):
        im = Image.open(p).convert("RGBA")
        im.thumbnail((CELL_W - 8, CELL_H - 8), Image.NEAREST)
        cell.alpha_composite(im, ((CELL_W - im.width)//2, (CELL_H - im.height)//2))
    sheet.alpha_composite(cell, (x0, y0))
    d.rectangle([x0, y0, x0+CELL_W-1, y0+CELL_H-1], outline=(140,140,150,255))
    orig = Image.open(p).size if os.path.exists(p) else (0,0)
    d.text((x0+2, y0+CELL_H+2), f"{name}  {orig[0]}x{orig[1]}", fill=(210,210,220,255), font=font)

out = os.path.join(ROOT, "STAGING", "_tools", "placeholder_contact_sheet.png")
sheet.convert("RGB").save(out)
print("wrote", out, sheet.size)
