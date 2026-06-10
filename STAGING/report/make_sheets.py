"""
RIMA raporu için kontak sheet üretici.
- mob_roster_sheet.png  : Act1Roster 12 sprite, 4×3 grid
- class_lineup_sheet.png: 10 oynanabilir sınıf, 2×5 grid
Pikseli korumak için nearest-neighbor (NEAREST) ölçekleme.
"""

from PIL import Image, ImageDraw, ImageFont
import os

# ─── YOLLAR ────────────────────────────────────────────────────────────────────
BASE    = "F:/Antigravity Projeler/2d roguelite/RIMA"
MOB_DIR = os.path.join(BASE, "Assets/Art/Mobs/Act1Roster")
CHAR_BASE = os.path.join(BASE, "Assets/Resources/Characters")
OUT_DIR = os.path.join(BASE, "STAGING/report/figures_v2")

# ─── MOB KADROsu ───────────────────────────────────────────────────────────────
# Dosya adı → Görüntü etiketi (sayı önekini çıkar, alt çizgiyi boşluğa çevir, büyüt)
MOB_FILES_RAW = [
    "fracture_imp.png",
    "seam_crawler.png",
    "rift_gound.png",
    "hollow_arbitter.png",
    "plate_widow.png",
    "relic_caster.png",
    "11_spire_choirling.png",
    "12_shard_walker.png",
    "13_penitent_bruiser.png",
    "14_riftbound_augur.png",
    "15_hollow_hulk.png",        # BOSS
    "16_rift_acolyte.png",
]

def mob_label(filename):
    name = os.path.splitext(filename)[0]
    # "15_hollow_hulk" → "hollow_hulk" (sayı öneki + _ kaldır)
    import re
    name = re.sub(r'^\d+_', '', name)
    # alt çizgiyi boşluğa çevir, kelime büyüt
    words = name.replace('_', ' ').title()
    if filename == "15_hollow_hulk.png":
        words += "\n[BOSS]"
    return words

# ─── SINIF KADROSU ─────────────────────────────────────────────────────────────
CLASS_LIST = [
    ("Warblade",     "warblade_idle_south.png"),
    ("Elementalist", "elementalist_idle_south.png"),
    ("Shadowblade",  "shadowblade_idle_south.png"),
    ("Ranger",       "ranger_idle_south.png"),
    ("Brawler",      "brawler_idle_south.png"),
    ("Gunslinger",   "gunslinger_idle_south.png"),
    ("Hexer",        "hexer_idle_south.png"),
    ("Ravager",      "ravager_idle_south.png"),
    ("Ronin",        "ronin_idle_south.png"),
    ("Summoner",     "summoner_idle_south.png"),
]

# ─── AYARLAR ───────────────────────────────────────────────────────────────────
SCALE       = 3          # nearest-neighbor büyütme katsayısı
COLS_MOB    = 4          # mob grid sütunu
COLS_CLASS  = 5          # sınıf grid sütunu
PAD         = 14         # hücre iç kenar boşluğu (piksel, ölçekli)
LABEL_H     = 34         # etiket yüksekliği (piksel)
BG_COLOR    = (20, 18, 24, 255)    # koyu mor-siyah arka plan
CELL_BG     = (36, 32, 44, 255)    # hücre arka planı
BOSS_ACCENT = (0, 220, 200, 255)   # boss vurgu (cyan)
LABEL_COLOR = (220, 215, 230)      # normal etiket rengi
BOSS_LABEL  = (0, 220, 200)        # boss etiket rengi
HEADER_H    = 48                   # başlık satırı yüksekliği
HEADER_BG   = (12, 10, 18, 255)

# Pillow'un dahili bitmap font'unu kullan (harici font gerekmez)
try:
    FONT_LABEL = ImageFont.truetype("arial.ttf", 12)
    FONT_HEAD  = ImageFont.truetype("arialbd.ttf", 14)
    FONT_BOSS  = ImageFont.truetype("arialbd.ttf", 13)
except:
    FONT_LABEL = ImageFont.load_default()
    FONT_HEAD  = FONT_LABEL
    FONT_BOSS  = FONT_LABEL


def load_and_scale(path, scale=SCALE):
    img = Image.open(path).convert("RGBA")
    w, h = img.size
    return img.resize((w * scale, h * scale), Image.NEAREST)


def draw_text_centered(draw, text, cx, cy, font, fill):
    """Metni (cx, cy) merkezine göre yaz."""
    # Pillow ≥ 9.2: textbbox, daha eski: textsize
    try:
        bbox = draw.textbbox((0, 0), text, font=font)
        tw = bbox[2] - bbox[0]
        th = bbox[3] - bbox[1]
    except AttributeError:
        tw, th = draw.textsize(text, font=font)
    draw.text((cx - tw // 2, cy - th // 2), text, font=font, fill=fill)


def make_grid_sheet(items, labels, cols, out_path, title,
                    is_boss_fn=None, scale=SCALE):
    """
    items  : [(path_str, ...), ...]
    labels : [str, ...]
    is_boss_fn: callable(index) → bool
    """
    # Tüm resimleri yükle ve ölçekle
    images = []
    for path in items:
        img = load_and_scale(path, scale)
        images.append(img)

    # Ortak hücre boyutu = en büyük sprite + padding
    cell_w = max(img.width  for img in images) + PAD * 2
    cell_h = max(img.height for img in images) + PAD * 2 + LABEL_H

    rows = (len(images) + cols - 1) // cols
    sheet_w = cell_w * cols
    sheet_h = cell_h * rows + HEADER_H

    sheet = Image.new("RGBA", (sheet_w, sheet_h), BG_COLOR)
    draw  = ImageDraw.Draw(sheet)

    # Başlık çubuğu
    draw.rectangle([0, 0, sheet_w, HEADER_H], fill=HEADER_BG)
    draw_text_centered(draw, title, sheet_w // 2, HEADER_H // 2,
                       FONT_HEAD, (255, 255, 255))

    for idx, (img, label) in enumerate(zip(images, labels)):
        row = idx // cols
        col = idx % cols
        x0  = col * cell_w
        y0  = HEADER_H + row * cell_h

        boss = is_boss_fn(idx) if is_boss_fn else False

        # Hücre arka planı
        cell_fill = CELL_BG
        draw.rectangle([x0, y0, x0 + cell_w - 1, y0 + cell_h - 1], fill=cell_fill)

        if boss:
            # Boss çerçevesi
            draw.rectangle([x0, y0, x0 + cell_w - 1, y0 + cell_h - 1],
                           outline=BOSS_ACCENT, width=3)

        # Sprite ortala
        sx = x0 + (cell_w - img.width) // 2
        sy = y0 + PAD
        sheet.paste(img, (sx, sy), img)

        # Etiket
        lx = x0 + cell_w // 2
        ly = y0 + cell_h - LABEL_H // 2

        if boss:
            lines = label.split("\n")
            draw_text_centered(draw, lines[0], lx, ly - 8, FONT_BOSS, BOSS_LABEL)
            if len(lines) > 1:
                draw_text_centered(draw, lines[1], lx, ly + 8, FONT_BOSS, BOSS_ACCENT)
        else:
            draw_text_centered(draw, label, lx, ly, FONT_LABEL, LABEL_COLOR)

    # RGBA → RGB (PNG'de şeffaflık korunur)
    out = sheet.convert("RGBA")
    out.save(out_path, "PNG")
    print(f"Kaydedildi: {out_path}  ({sheet_w}×{sheet_h})")


# ═══════════════════════════════════════════════════════════════════════════════
# 1) MOB KADROSU KONTAK SHEET
# ═══════════════════════════════════════════════════════════════════════════════
mob_paths  = [os.path.join(MOB_DIR, f) for f in MOB_FILES_RAW]
mob_labels = [mob_label(f) for f in MOB_FILES_RAW]

def is_boss_mob(idx):
    return MOB_FILES_RAW[idx] == "15_hollow_hulk.png"

make_grid_sheet(
    mob_paths, mob_labels,
    cols=COLS_MOB,
    out_path=os.path.join(OUT_DIR, "mob_roster_sheet.png"),
    title="RIMA — Act 1 Mob Kadrosu (12 Düşman + Hollow Hulk Boss)",
    is_boss_fn=is_boss_mob,
)

# ═══════════════════════════════════════════════════════════════════════════════
# 2) SINIF DİZİLİMİ KONTAK SHEET
# ═══════════════════════════════════════════════════════════════════════════════
class_paths  = [os.path.join(CHAR_BASE, cls, fname) for cls, fname in CLASS_LIST]
class_labels = [cls for cls, _ in CLASS_LIST]

make_grid_sheet(
    class_paths, class_labels,
    cols=COLS_CLASS,
    out_path=os.path.join(OUT_DIR, "class_lineup_sheet.png"),
    title="RIMA — Oynanabilir Sınıf Kadrosu (10 Sınıf)",
    is_boss_fn=None,
)

# ═══════════════════════════════════════════════════════════════════════════════
# 3) WARBLADE TEK-FİGÜR (fig06_warblade.png)
# ═══════════════════════════════════════════════════════════════════════════════
wb_src = os.path.join(CHAR_BASE, "Warblade", "warblade_idle_south.png")
wb_img = load_and_scale(wb_src, scale=4)

BORDER = 20
wb_out = Image.new("RGBA", (wb_img.width + BORDER*2, wb_img.height + BORDER*2 + LABEL_H), BG_COLOR)
draw_wb = ImageDraw.Draw(wb_out)
draw_wb.rectangle([0, 0, wb_out.width-1, wb_out.height-1], outline=(0,220,200,255), width=2)
wb_out.paste(wb_img, (BORDER, BORDER), wb_img)
draw_text_centered(draw_wb, "Warblade",
                   wb_out.width // 2,
                   wb_img.height + BORDER + LABEL_H // 2,
                   FONT_LABEL, LABEL_COLOR)

wb_out_path = os.path.join(OUT_DIR, "fig06_warblade.png")
wb_out.convert("RGBA").save(wb_out_path, "PNG")
print(f"Kaydedildi: {wb_out_path}  ({wb_out.width}×{wb_out.height})")

print("\nTüm sheet'ler üretildi.")
