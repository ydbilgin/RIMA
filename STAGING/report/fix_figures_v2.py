"""
figures_v2 mikro-fix turu (FIX-1, FIX-2, FIX-3)
Run: python fix_figures_v2.py
"""

from PIL import Image, ImageDraw, ImageFont
import os

ROOT = r"F:\Antigravity Projeler\2d roguelite\RIMA"
OUT  = os.path.join(ROOT, "STAGING", "report", "figures_v2")

FONT_BOLD   = "C:/Windows/Fonts/arialbd.ttf"
FONT_REG    = "C:/Windows/Fonts/arial.ttf"

# ─────────────────────────────────────────────────────────────
# FIX-1: fig14_qc_before_after.png — doğru oda çifti
# ─────────────────────────────────────────────────────────────
def fix14():
    before_path = os.path.join(ROOT, "Assets/Screenshots/RoomQC/combat_large_diamond_01.png")
    after_path  = os.path.join(ROOT, "Assets/Screenshots/RoomQC_v2/combat_large_diamond_01.png")

    before = Image.open(before_path).convert("RGB")
    after  = Image.open(after_path).convert("RGB")

    # Uniform target width; keep aspect ratio
    TARGET_W = 900
    def resize_keep_ar(img, target_w):
        w, h = img.size
        scale = target_w / w
        return img.resize((target_w, int(h * scale)), Image.NEAREST)

    before_r = resize_keep_ar(before, TARGET_W)
    after_r  = resize_keep_ar(after,  TARGET_W)

    BAND_H   = 48   # title band height (top)
    BOTTOM_H = 72   # bottom caption band height
    PAD      = 12   # gap between panels
    CANVAS_W = TARGET_W * 2 + PAD
    CANVAS_H = BAND_H + max(before_r.height, after_r.height) + BOTTOM_H

    canvas = Image.new("RGB", (CANVAS_W, CANVAS_H), (245, 245, 248))
    draw   = ImageDraw.Draw(canvas)

    # Title bands
    draw.rectangle([0, 0, TARGET_W, BAND_H],                         fill=(220, 60, 60))
    draw.rectangle([TARGET_W + PAD, 0, CANVAS_W, BAND_H],            fill=(50, 160, 80))

    try:
        font_title  = ImageFont.truetype(FONT_BOLD, 22)
        font_cap    = ImageFont.truetype(FONT_REG,  18)
    except Exception:
        font_title  = ImageFont.load_default()
        font_cap    = font_title

    draw.text((TARGET_W // 2, BAND_H // 2), "ÖNCE", fill="white",
              font=font_title, anchor="mm")
    draw.text((TARGET_W + PAD + TARGET_W // 2, BAND_H // 2), "SONRA", fill="white",
              font=font_title, anchor="mm")

    # Paste panels
    canvas.paste(before_r, (0, BAND_H))
    canvas.paste(after_r,  (TARGET_W + PAD, BAND_H))

    # Red ellipse overlay on BEFORE — props at bottom-left edge (outside walkable area)
    # The before image shows cyan crystal props placed near the bottom-left edge cliff
    # and a gate structure at the top-right corner outside the main walkable floor.
    # We draw attention to the bottom-left area where props hang off the cliff edge.
    draw_on_before = ImageDraw.Draw(canvas)
    # bottom-left prop cluster (cyan crystals spilling off cliff)
    ex, ey = 30, BAND_H + before_r.height - 160
    ew, eh = 180, 130
    draw_on_before.ellipse([ex, ey, ex + ew, ey + eh],
                           outline=(220, 30, 30), width=5)

    # top-right gate/prop outside main floor
    ex2, ey2 = TARGET_W - 220, BAND_H + 60
    draw_on_before.ellipse([ex2, ey2, ex2 + 200, ey2 + 130],
                           outline=(220, 30, 30), width=5)

    # Green tick on AFTER panel
    tick_cx = TARGET_W + PAD + TARGET_W // 2
    tick_cy = BAND_H + after_r.height // 2
    # Simple bold checkmark via lines
    check_d = ImageDraw.Draw(canvas)
    cs = 40  # size
    check_d.line([tick_cx - cs, tick_cy,
                  tick_cx - cs//4, tick_cy + cs,
                  tick_cx + cs, tick_cy - cs],
                 fill=(40, 180, 70), width=8)

    # Bottom caption band
    cap_y = BAND_H + max(before_r.height, after_r.height)
    draw.rectangle([0, cap_y, CANVAS_W, CANVAS_H], fill=(235, 235, 240))
    caption = "2 FAIL + 9 şüpheli  →  yerleştiricide walkable doğrulama fix'i  →  yeniden denetim"
    draw.text((CANVAS_W // 2, cap_y + BOTTOM_H // 2), caption, fill=(50, 50, 80),
              font=font_cap, anchor="mm")

    out_path = os.path.join(OUT, "fig14_qc_before_after.png")
    canvas.save(out_path)
    print(f"[FIX-1] Saved {out_path}  ({canvas.size})")
    return canvas.size


# ─────────────────────────────────────────────────────────────
# FIX-2: fig06_warblade.png — caption yazım hatası
# ─────────────────────────────────────────────────────────────
def fix06():
    sprite_path = os.path.join(ROOT,
        "Assets/Resources/Characters/Warblade/warblade_idle_south.png")

    sprite = Image.open(sprite_path).convert("RGBA")
    SCALE  = 7
    sw, sh = sprite.size
    scaled = sprite.resize((sw * SCALE, sh * SCALE), Image.NEAREST)

    MARGIN  = 60
    CAP_H   = 52
    BG_COLOR = (230, 230, 232)

    canvas_w = scaled.width  + MARGIN * 2
    canvas_h = scaled.height + MARGIN * 2 + CAP_H

    canvas = Image.new("RGB", (canvas_w, canvas_h), BG_COLOR)

    # Paste sprite (blend alpha over background)
    bg_region = Image.new("RGB", scaled.size, BG_COLOR)
    bg_region.paste(scaled, mask=scaled.split()[3])
    canvas.paste(bg_region, (MARGIN, MARGIN))

    draw = ImageDraw.Draw(canvas)

    try:
        font_cap = ImageFont.truetype(FONT_REG, 22)
    except Exception:
        font_cap = ImageFont.load_default()

    # CORRECTED caption
    caption = "Warblade — güney yönü idle sprite’ı (7× büyütme, nearest-neighbor)"
    draw.text((canvas_w // 2, canvas_h - CAP_H // 2), caption,
              fill=(50, 50, 50), font=font_cap, anchor="mm")

    out_path = os.path.join(OUT, "fig06_warblade.png")
    canvas.save(out_path)
    print(f"[FIX-2] Saved {out_path}  ({canvas.size})")
    return canvas.size


# ─────────────────────────────────────────────────────────────
# FIX-3: fig08_pipeline.png — terim düzeltme
# ─────────────────────────────────────────────────────────────
def fix08():
    try:
        font_title = ImageFont.truetype(FONT_BOLD, 32)
        font_h2    = ImageFont.truetype(FONT_BOLD, 22)
        font_sub   = ImageFont.truetype(FONT_REG,  17)
        font_note  = ImageFont.truetype(FONT_REG,  16)
    except Exception:
        font_title = ImageFont.load_default()
        font_h2    = font_title
        font_sub   = font_title
        font_note  = font_title

    W        = 1200
    BOX_W    = 820
    BOX_H    = 80
    BOX_GAP  = 18
    TOP_PAD  = 90
    SIDE_PAD = (W - BOX_W) // 2
    BOTTOM   = 60
    ARROW_H  = BOX_GAP

    STAGES = [
        {
            "title": "Oda JSON / Editör Boyama",
            "sub":   "Kaynak: JSON dosyası veya Map Designer editörü",
            "color": (240, 240, 248),
            "border": (180, 180, 200),
            "highlight": False,
        },
        {
            "title": "RoomJsonImporter",
            "sub":   "JSON → Unity ScriptableObject dönüşümü",
            "color": (240, 240, 248),
            "border": (180, 180, 200),
            "highlight": False,
        },
        {
            "title": "RoomTemplateSO",
            # FIXED: removed redundant "(canonical kaynak)"
            "sub":   "Canonical oda verisi — tek doğruluk kaynağı",
            "color": (220, 248, 248),
            "border": (0, 180, 190),
            "highlight": True,
        },
        {
            "title": "Validator (8 kural)",
            "sub":   "Walkable grid, kapı yönü, prop çakışma…",
            "color": (240, 240, 248),
            "border": (180, 180, 200),
            "highlight": False,
        },
        {
            "title": "IsoRoomBuilder",
            "sub":   "Runtime izometrik ada inşası",
            "color": (240, 240, 248),
            "border": (180, 180, 200),
            "highlight": False,
        },
        {
            "title": "_Arena Runtime Odası",
            "sub":   "Oynanabilir sahne: zemin + clifflar + prop’lar",
            "color": (240, 240, 248),
            "border": (180, 180, 200),
            "highlight": False,
        },
        {
            # FIXED: "Duman Testi" → "Smoke Test"
            "title": "QC Ekran Görüntüsü / Smoke Test (26/26)",
            "sub":   "Otomatik görsel denetim + test koşusu",
            "color": (240, 240, 248),
            "border": (180, 180, 200),
            "highlight": False,
        },
    ]

    total_h = (TOP_PAD
               + len(STAGES) * BOX_H
               + (len(STAGES) - 1) * BOX_GAP
               + BOTTOM + 50)

    canvas = Image.new("RGB", (W, total_h), (255, 255, 255))
    draw   = ImageDraw.Draw(canvas)

    # Title
    draw.text((W // 2, 44), "Oda Üretim Boru Hattı",
              fill=(20, 20, 40), font=font_title, anchor="mm")

    y = TOP_PAD
    for i, stage in enumerate(STAGES):
        x0, y0 = SIDE_PAD, y
        x1, y1 = SIDE_PAD + BOX_W, y + BOX_H
        r = 10  # corner radius

        # Rounded rect via polygon approximation
        draw.rounded_rectangle([x0, y0, x1, y1], radius=r,
                                fill=stage["color"], outline=stage["border"],
                                width=3 if stage["highlight"] else 1)

        # Box text
        cy = (y0 + y1) // 2
        draw.text((W // 2, cy - 12), stage["title"],
                  fill=(20, 20, 40), font=font_h2, anchor="mm")
        draw.text((W // 2, cy + 16), stage["sub"],
                  fill=(90, 90, 110), font=font_sub, anchor="mm")

        y += BOX_H

        # Arrow between boxes
        if i < len(STAGES) - 1:
            ax = W // 2
            draw.line([(ax, y + 3), (ax, y + BOX_GAP - 3)],
                      fill=(100, 100, 120), width=2)
            # Arrowhead
            draw.polygon([(ax - 7, y + BOX_GAP - 8),
                          (ax + 7, y + BOX_GAP - 8),
                          (ax,     y + BOX_GAP - 1)],
                         fill=(100, 100, 120))
            y += BOX_GAP

    # Footer note
    draw.text((W // 2, total_h - BOTTOM // 2 - 10),
              "Cyan çerçeve = canonical kaynak (RoomTemplateSO)",
              fill=(0, 150, 160), font=font_note, anchor="mm")

    out_path = os.path.join(OUT, "fig08_pipeline.png")
    canvas.save(out_path)
    print(f"[FIX-3] Saved {out_path}  ({canvas.size})")
    return canvas.size


# ─────────────────────────────────────────────────────────────
if __name__ == "__main__":
    s14 = fix14()
    s06 = fix06()
    s08 = fix08()
    print("\nAll done.")
    print(f"  fig14: {s14}")
    print(f"  fig06: {s06}")
    print(f"  fig08: {s08}")
