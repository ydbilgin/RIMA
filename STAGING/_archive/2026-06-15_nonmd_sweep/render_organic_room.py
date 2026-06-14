"""Render a TRULY organic-looking room — no visible grid borders.
Show parts separately AND combined with real character anchor.
"""
from PIL import Image, ImageDraw, ImageFilter, ImageEnhance
import os, random, math

W, H = 1600, 1400
img = Image.new("RGBA", (W, H), (26, 36, 56, 255))
draw = ImageDraw.Draw(img)


def load_font(size, bold=False):
    paths = [
        "C:/Windows/Fonts/segoeuib.ttf" if bold else "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    ]
    for p in paths:
        if os.path.exists(p):
            return ImageFont.truetype(p, size)
    from PIL import ImageFont as IF
    return IF.load_default()


from PIL import ImageFont
f_huge = load_font(34, bold=True)
f_title = load_font(22, bold=True)
f_label = load_font(15, bold=True)
f_text = load_font(13)
f_small = load_font(11)


def center_text(target, x, y, text, font, fill="#E8DCC4"):
    d = ImageDraw.Draw(target)
    bbox = d.textbbox((0, 0), text, font=font)
    tw = bbox[2] - bbox[0]
    d.text((x - tw / 2, y), text, fill=fill, font=font)


center_text(img, W / 2, 25, "RIMA - Dogal Gorunumlu Oda (organik, yapay durmayan)", f_huge)
center_text(img, W / 2, 70, "Yapitaslari + Birlesim + Karakter ile", f_text, "#9EB3C2")
ImageDraw.Draw(img).line([(100, 100), (W - 100, 100)], fill="#4A5F7A", width=2)

# ============ SECTION 1: PARTS (top) ============
center_text(img, W / 2, 130, "1. YAPITASLARI (her biri ayri uretim)", f_title, "#7BA7BC")

parts_y = 175
part_size = 130
parts = [
    ("Floor Tile", "L2 - 64x64", (74, 58, 42), False),
    ("Wall Tile", "L3 - 64x96", (74, 74, 90), False),
    ("Moss Patch", "L4 - 96x64", (58, 90, 58), True),
    ("Crack Detail", "L5 - 32x32", None, False),
    ("Rift Accent", "L6 - 128x128", (90, 40, 50), True),
    ("Wooden Crate", "Prop - 64x64", (90, 74, 58), False),
    ("Character", "Anchor - 64x64", None, False),
]

# Compute total width + spacing
total_parts_w = len(parts) * part_size + (len(parts) - 1) * 30
parts_start_x = (W - total_parts_w) / 2

for i, (label, dims, base_color, organic) in enumerate(parts):
    px = parts_start_x + i * (part_size + 30)
    # Border + bg
    draw.rounded_rectangle([(px, parts_y), (px + part_size, parts_y + part_size + 40)],
                            radius=8, fill=(45, 53, 72, 255), outline="#4A5F7A", width=2)
    # Sample box (inner area)
    inner = (int(px + 15), int(parts_y + 15), int(px + part_size - 15), int(parts_y + part_size - 15))

    if label == "Character":
        # Load real anchor — try a few classes
        anchor_path = "F:/Antigravity Projeler/2d roguelite/RIMA/ANCHORS/characters/04_ranger.png"
        if os.path.exists(anchor_path):
            char = Image.open(anchor_path).convert("RGBA")
            char_w = inner[2] - inner[0]
            char_h = inner[3] - inner[1]
            # Scale character up - keep pixel-perfect
            char_scaled = char.resize((char_w, char_h), Image.NEAREST)
            img.paste(char_scaled, (inner[0], inner[1]), char_scaled)
    elif label == "Crack Detail":
        # Dark base + crack line
        draw.rectangle(inner, fill=(60, 45, 32, 255))
        # crack lines
        cx_, cy_ = (inner[0] + inner[2]) // 2, (inner[1] + inner[3]) // 2
        draw.line([(cx_ - 30, cy_ - 20), (cx_ + 5, cy_), (cx_ + 25, cy_ + 15)], fill=(20, 20, 20), width=3)
        draw.line([(cx_ - 15, cy_ + 10), (cx_ + 10, cy_ + 25)], fill=(20, 20, 20), width=2)
    elif organic:
        # Organic shape with soft edges
        layer = Image.new("RGBA", (part_size, part_size + 40), (0, 0, 0, 0))
        ldraw = ImageDraw.Draw(layer)
        # Background floor color
        draw.rectangle(inner, fill=(60, 45, 32, 255))
        # Add small noise to background
        for _ in range(150):
            nx = random.randint(inner[0], inner[2])
            ny = random.randint(inner[1], inner[3])
            offset = random.randint(-8, 8)
            base_r, base_g, base_b = 60, 45, 32
            r = max(30, min(85, base_r + offset))
            g = max(25, min(65, base_g + offset))
            b = max(15, min(45, base_b + offset))
            draw.ellipse([(nx, ny), (nx + 2, ny + 2)], fill=(r, g, b, 255))
        # Organic blob (irregular ellipse)
        cx_, cy_ = (inner[0] + inner[2]) // 2, (inner[1] + inner[3]) // 2
        # Multi-blob organic shape
        if base_color:
            r, g, b = base_color
            for blob_i in range(4):
                bx = cx_ + random.randint(-12, 12)
                by = cy_ + random.randint(-8, 8)
                bw = random.randint(20, 32)
                bh = random.randint(14, 22)
                blob = Image.new("RGBA", (part_size, part_size + 40), (0, 0, 0, 0))
                bdraw = ImageDraw.Draw(blob)
                bdraw.ellipse([(bx - bw, by - bh), (bx + bw, by + bh)],
                              fill=(r, g, b, 180))
                blob = blob.filter(ImageFilter.GaussianBlur(2))
                img.alpha_composite(blob)
            # Inner highlight
            for blob_i in range(3):
                bx = cx_ + random.randint(-10, 10)
                by = cy_ + random.randint(-6, 6)
                bw = random.randint(8, 14)
                bh = random.randint(5, 10)
                blob = Image.new("RGBA", (part_size, part_size + 40), (0, 0, 0, 0))
                bdraw = ImageDraw.Draw(blob)
                hr = min(255, r + 20)
                hg = min(255, g + 20)
                hb = min(255, b + 20)
                bdraw.ellipse([(bx - bw, by - bh), (bx + bw, by + bh)],
                              fill=(hr, hg, hb, 150))
                blob = blob.filter(ImageFilter.GaussianBlur(1))
                img.alpha_composite(blob)
    elif label == "Wooden Crate":
        # Background
        draw.rectangle(inner, fill=(60, 45, 32, 255))
        # Tiny noise
        for _ in range(100):
            nx = random.randint(inner[0], inner[2])
            ny = random.randint(inner[1], inner[3])
            offset = random.randint(-6, 6)
            draw.point((nx, ny), fill=(60 + offset, 45 + offset, 32 + offset, 255))
        # Crate
        cx_, cy_ = (inner[0] + inner[2]) // 2, (inner[1] + inner[3]) // 2
        # Ground shadow first
        draw.ellipse([(cx_ - 35, cy_ + 22), (cx_ + 35, cy_ + 32)], fill=(0, 0, 0, 120))
        # Crate body
        draw.rectangle([(cx_ - 30, cy_ - 25), (cx_ + 30, cy_ + 25)], fill=(90, 74, 58, 255), outline=(40, 28, 18), width=2)
        # Iron bands
        draw.rectangle([(cx_ - 30, cy_ - 22), (cx_ + 30, cy_ - 17)], fill=(70, 58, 48, 255))
        draw.rectangle([(cx_ - 30, cy_ + 17), (cx_ + 30, cy_ + 22)], fill=(70, 58, 48, 255))
        # Rivets
        for rx in [-25, 25]:
            draw.ellipse([(cx_ + rx - 2, cy_ - 22), (cx_ + rx + 2, cy_ - 18)], fill=(120, 100, 60))
            draw.ellipse([(cx_ + rx - 2, cy_ + 18), (cx_ + rx + 2, cy_ + 22)], fill=(120, 100, 60))
    elif label == "Wall Tile":
        # Wall with depth
        r, g, b = base_color
        draw.rectangle(inner, fill=(r, g, b, 255))
        # Wall top cap (lighter, depth)
        wall_top = (inner[0], inner[1], inner[2], inner[1] + 30)
        draw.rectangle(wall_top, fill=(r + 15, g + 15, b + 25, 255))
        # Subtle stone pattern noise
        for _ in range(150):
            nx = random.randint(inner[0], inner[2])
            ny = random.randint(inner[1], inner[3])
            offset = random.randint(-8, 8)
            draw.point((nx, ny), fill=(r + offset, g + offset, b + offset, 255))
        # Cracks
        draw.line([(inner[0] + 30, inner[1] + 40), (inner[0] + 60, inner[1] + 70)], fill=(25, 25, 35), width=1)
    else:  # Floor Tile
        r, g, b = base_color
        draw.rectangle(inner, fill=(r, g, b, 255))
        # Heavy noise for organic floor
        for _ in range(300):
            nx = random.randint(inner[0], inner[2])
            ny = random.randint(inner[1], inner[3])
            offset = random.randint(-15, 10)
            draw.point((nx, ny), fill=(max(0, r + offset), max(0, g + offset), max(0, b + offset), 255))
        # Few small dark spots (organic stains)
        for _ in range(5):
            nx = random.randint(inner[0] + 10, inner[2] - 10)
            ny = random.randint(inner[1] + 10, inner[3] - 10)
            sz = random.randint(3, 6)
            draw.ellipse([(nx - sz, ny - sz), (nx + sz, ny + sz)], fill=(max(0, r - 20), max(0, g - 15), max(0, b - 10), 255))

    # Label
    center_text(img, px + part_size / 2, parts_y + part_size + 5, label, f_label, "#E8DCC4")
    center_text(img, px + part_size / 2, parts_y + part_size + 24, dims, f_small, "#9EB3C2")

# ============ SECTION 2: COMBINED ROOM ============
ImageDraw.Draw(img).line([(100, 405), (W - 100, 405)], fill="#4A5F7A", width=2)
center_text(img, W / 2, 435, "2. HEPSI BIRLESINCE (gercek odadaki gorunum, grid kaybolur)", f_title, "#7BA7BC")

# Big room area
room_x, room_y = 100, 490
room_w, room_h = 1400, 600

# Base floor with heavy noise (no grid lines!)
floor_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
fdraw = ImageDraw.Draw(floor_layer)

# Floor base
base_r, base_g, base_b = 70, 53, 38
for px_ in range(room_w):
    for py_ in range(room_h):
        # Per-pixel variation
        noise = random.randint(-18, 10)
        edge_dark = 0
        # Edge darkening (vignette)
        if px_ < 100 or px_ > room_w - 100 or py_ < 100 or py_ > room_h - 100:
            edge_dark = -10
        r = max(20, min(110, base_r + noise + edge_dark))
        g = max(15, min(80, base_g + noise + edge_dark))
        b = max(10, min(60, base_b + noise + edge_dark))
        fdraw.point((px_, py_), fill=(r, g, b, 255))

# Large irregular floor stains for natural variation
random.seed(77)
for _ in range(20):
    sx = random.randint(50, room_w - 50)
    sy = random.randint(50, room_h - 50)
    sw = random.randint(40, 90)
    sh = random.randint(25, 55)
    stain = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
    sdraw = ImageDraw.Draw(stain)
    darken = random.randint(-25, -10)
    sdraw.ellipse([(sx - sw, sy - sh), (sx + sw, sy + sh)],
                  fill=(max(0, base_r + darken),
                        max(0, base_g + darken),
                        max(0, base_b + darken), 100))
    stain = stain.filter(ImageFilter.GaussianBlur(8))
    floor_layer.alpha_composite(stain)

# Composite floor onto main
img.alpha_composite(floor_layer, (room_x, room_y))

# Walls (Wang16) - top, bottom borders
wall_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
wdraw = ImageDraw.Draw(wall_layer)

wall_thickness = 50
# Top wall
for px_ in range(room_w):
    for py_ in range(wall_thickness):
        noise = random.randint(-12, 8)
        r = max(30, min(110, 70 + noise))
        g = max(30, min(110, 70 + noise))
        b = max(40, min(140, 90 + noise))
        wdraw.point((px_, py_), fill=(r, g, b, 255))
# wall cap (darker top edge)
for py_ in range(wall_thickness - 15, wall_thickness):
    alpha = int(120 * (py_ - (wall_thickness - 15)) / 15)
    wdraw.line([(0, py_), (room_w, py_)], fill=(0, 0, 0, alpha))

# Bottom wall
for px_ in range(room_w):
    for py_ in range(room_h - wall_thickness, room_h):
        noise = random.randint(-12, 8)
        r = max(30, min(110, 70 + noise))
        g = max(30, min(110, 70 + noise))
        b = max(40, min(140, 90 + noise))
        wdraw.point((px_, py_), fill=(r, g, b, 255))

# Left wall
for py_ in range(room_h):
    for px_ in range(wall_thickness):
        noise = random.randint(-12, 8)
        r = max(30, min(110, 70 + noise))
        g = max(30, min(110, 70 + noise))
        b = max(40, min(140, 90 + noise))
        wdraw.point((px_, py_), fill=(r, g, b, 255))
# Right wall
for py_ in range(room_h):
    for px_ in range(room_w - wall_thickness, room_w):
        noise = random.randint(-12, 8)
        r = max(30, min(110, 70 + noise))
        g = max(30, min(110, 70 + noise))
        b = max(40, min(140, 90 + noise))
        wdraw.point((px_, py_), fill=(r, g, b, 255))

img.alpha_composite(wall_layer, (room_x, room_y))

# L4 Moss patches — large organic, semi-transparent
random.seed(123)
moss_count = 15
for _ in range(moss_count):
    mx = random.randint(80, room_w - 80)
    my = random.randint(80, room_h - 80)
    mw = random.randint(40, 100)
    mh = random.randint(25, 60)
    moss_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
    mdraw = ImageDraw.Draw(moss_layer)
    # Multi-blob
    for sub in range(3):
        ox = random.randint(-15, 15)
        oy = random.randint(-8, 8)
        sw = mw + random.randint(-20, 5)
        sh = mh + random.randint(-12, 3)
        mdraw.ellipse([(mx + ox - sw, my + oy - sh), (mx + ox + sw, my + oy + sh)],
                      fill=(50, 85, 50, 140))
    moss_layer = moss_layer.filter(ImageFilter.GaussianBlur(4))
    img.alpha_composite(moss_layer, (room_x, room_y))
    # Inner highlight
    hl_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
    hldraw = ImageDraw.Draw(hl_layer)
    for sub in range(2):
        ox = random.randint(-10, 10)
        oy = random.randint(-5, 5)
        sw = max(10, mw - 30)
        sh = max(6, mh - 20)
        hldraw.ellipse([(mx + ox - sw, my + oy - sh), (mx + ox + sw, my + oy + sh)],
                       fill=(75, 110, 60, 120))
    hl_layer = hl_layer.filter(ImageFilter.GaussianBlur(2))
    img.alpha_composite(hl_layer, (room_x, room_y))

# L5 Cracks — organic line strokes
random.seed(55)
for _ in range(8):
    cx_start = random.randint(80, room_w - 200)
    cy_start = random.randint(80, room_h - 80)
    cracks_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
    cdraw = ImageDraw.Draw(cracks_layer)
    points = [(cx_start, cy_start)]
    cx, cy = cx_start, cy_start
    for step in range(random.randint(3, 6)):
        cx += random.randint(20, 60)
        cy += random.randint(-25, 25)
        points.append((cx, cy))
    for i in range(len(points) - 1):
        cdraw.line([points[i], points[i + 1]], fill=(15, 15, 18, 220), width=2)
    img.alpha_composite(cracks_layer, (room_x, room_y))

# L5 Pebbles + bones scatter
random.seed(88)
for _ in range(40):
    pbx = random.randint(60, room_w - 60)
    pby = random.randint(60, room_h - 60)
    sz = random.randint(2, 5)
    if random.random() < 0.3:
        # Bone fragment
        draw.ellipse([(room_x + pbx - sz - 1, room_y + pby - sz),
                      (room_x + pbx + sz + 1, room_y + pby + sz)],
                     fill=(180, 165, 140, 220))
    else:
        # Pebble
        draw.ellipse([(room_x + pbx - sz, room_y + pby - sz),
                      (room_x + pbx + sz, room_y + pby + sz)],
                     fill=(70, 60, 48, 220))

# L6 Rift scar — big organic dark stain (off-center)
rift_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
rdraw = ImageDraw.Draw(rift_layer)
rift_cx, rift_cy = 950, 350
# Multi-blob organic shape
for blob in range(6):
    ox = random.randint(-30, 30)
    oy = random.randint(-20, 20)
    bw = random.randint(80, 130)
    bh = random.randint(50, 85)
    rdraw.ellipse([(rift_cx + ox - bw, rift_cy + oy - bh),
                   (rift_cx + ox + bw, rift_cy + oy + bh)],
                  fill=(75, 25, 35, 150))
rift_layer = rift_layer.filter(ImageFilter.GaussianBlur(6))
img.alpha_composite(rift_layer, (room_x, room_y))
# Dark center
center_rift = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
crdraw = ImageDraw.Draw(center_rift)
crdraw.ellipse([(rift_cx - 60, rift_cy - 40), (rift_cx + 60, rift_cy + 40)],
               fill=(40, 15, 20, 180))
center_rift = center_rift.filter(ImageFilter.GaussianBlur(3))
img.alpha_composite(center_rift, (room_x, room_y))
# Crack radials from rift
for angle_deg in [25, 75, 135, 200, 260, 320]:
    ang = math.radians(angle_deg)
    crk_layer = Image.new("RGBA", (room_w, room_h), (0, 0, 0, 0))
    ckdraw = ImageDraw.Draw(crk_layer)
    ex = rift_cx + int(140 * math.cos(ang))
    ey = rift_cy + int(80 * math.sin(ang))
    ckdraw.line([(rift_cx, rift_cy), (ex, ey)], fill=(20, 5, 10, 200), width=2)
    img.alpha_composite(crk_layer, (room_x, room_y))

# Props
# Wooden Crate (bottom-left area)
crate_x, crate_y = 200, 450
# Ground shadow first (soft)
shadow_layer = Image.new("RGBA", (200, 100), (0, 0, 0, 0))
sdraw = ImageDraw.Draw(shadow_layer)
sdraw.ellipse([(20, 70), (180, 95)], fill=(0, 0, 0, 130))
shadow_layer = shadow_layer.filter(ImageFilter.GaussianBlur(5))
img.alpha_composite(shadow_layer, (room_x + crate_x - 100, room_y + crate_y))
# Crate
draw.rectangle([(room_x + crate_x - 30, room_y + crate_y),
                (room_x + crate_x + 30, room_y + crate_y + 50)],
               fill=(90, 74, 58, 255), outline=(40, 28, 18), width=2)
draw.rectangle([(room_x + crate_x - 30, room_y + crate_y + 4),
                (room_x + crate_x + 30, room_y + crate_y + 9)],
               fill=(70, 58, 48))
draw.rectangle([(room_x + crate_x - 30, room_y + crate_y + 41),
                (room_x + crate_x + 30, room_y + crate_y + 46)],
               fill=(70, 58, 48))

# Brazier (bottom-right)
brazier_x, brazier_y = 1180, 450
# Warm light cone (alpha gradient)
glow_layer = Image.new("RGBA", (300, 300), (0, 0, 0, 0))
gdraw = ImageDraw.Draw(glow_layer)
for r in range(150, 0, -3):
    alpha = max(0, int(180 * math.exp(-r / 60)))
    gdraw.ellipse([(150 - r, 150 - r), (150 + r, 150 + r)],
                  fill=(255, 140, 60, alpha // 5))
glow_layer = glow_layer.filter(ImageFilter.GaussianBlur(8))
img.alpha_composite(glow_layer, (room_x + brazier_x - 150, room_y + brazier_y - 150))
# Brazier base
draw.rectangle([(room_x + brazier_x - 22, room_y + brazier_y + 10),
                (room_x + brazier_x + 22, room_y + brazier_y + 25)], fill=(40, 35, 30), outline=(20, 18, 15), width=1)
draw.rectangle([(room_x + brazier_x - 18, room_y + brazier_y),
                (room_x + brazier_x + 18, room_y + brazier_y + 10)], fill=(55, 50, 45), outline=(20, 18, 15), width=1)
# Fire
draw.ellipse([(room_x + brazier_x - 14, room_y + brazier_y - 8),
              (room_x + brazier_x + 14, room_y + brazier_y + 2)], fill=(255, 90, 50))
draw.ellipse([(room_x + brazier_x - 10, room_y + brazier_y - 14),
              (room_x + brazier_x + 10, room_y + brazier_y - 4)], fill=(255, 180, 80))

# Urn (top-middle)
urn_x, urn_y = 760, 200
draw.ellipse([(room_x + urn_x - 20, room_y + urn_y + 35), (room_x + urn_x + 20, room_y + urn_y + 42)],
             fill=(0, 0, 0, 130))
draw.ellipse([(room_x + urn_x - 18, room_y + urn_y - 5), (room_x + urn_x + 18, room_y + urn_y + 5)],
             fill=(55, 60, 65), outline=(25, 28, 30))
draw.polygon([(room_x + urn_x - 15, room_y + urn_y),
              (room_x + urn_x - 15, room_y + urn_y + 32),
              (room_x + urn_x, room_y + urn_y + 40),
              (room_x + urn_x + 15, room_y + urn_y + 32),
              (room_x + urn_x + 15, room_y + urn_y)],
             fill=(70, 75, 80), outline=(25, 28, 30))

# Candles (multiple, near walls)
for cx_pos, cy_pos in [(450, 200), (450, 470), (1080, 200)]:
    # Glow halo
    glow_l = Image.new("RGBA", (80, 80), (0, 0, 0, 0))
    gdr = ImageDraw.Draw(glow_l)
    for r in range(30, 0, -2):
        alpha = max(0, int(140 * math.exp(-r / 18)))
        gdr.ellipse([(40 - r, 40 - r), (40 + r, 40 + r)],
                    fill=(255, 200, 100, alpha // 4))
    glow_l = glow_l.filter(ImageFilter.GaussianBlur(4))
    img.alpha_composite(glow_l, (room_x + cx_pos - 40, room_y + cy_pos - 40))
    # Candle
    draw.rectangle([(room_x + cx_pos - 2, room_y + cy_pos - 12),
                    (room_x + cx_pos + 2, room_y + cy_pos + 10)], fill=(230, 220, 195))
    draw.ellipse([(room_x + cx_pos - 3, room_y + cy_pos - 18),
                  (room_x + cx_pos + 3, room_y + cy_pos - 8)], fill=(255, 180, 80))
    draw.rectangle([(room_x + cx_pos - 5, room_y + cy_pos + 10),
                    (room_x + cx_pos + 5, room_y + cy_pos + 16)], fill=(50, 45, 35))

# CHARACTER — load real anchor and paste at center-ish (visible)
anchor_path = "F:/Antigravity Projeler/2d roguelite/RIMA/ANCHORS/characters/04_ranger.png"
if os.path.exists(anchor_path):
    char = Image.open(anchor_path).convert("RGBA")
    char_size = 200  # 3x scale from 64
    char_scaled = char.resize((char_size, char_size), Image.NEAREST)
    # Char shadow
    cshadow = Image.new("RGBA", (char_size + 20, 30), (0, 0, 0, 0))
    csdraw = ImageDraw.Draw(cshadow)
    csdraw.ellipse([(10, 5), (char_size + 10, 25)], fill=(0, 0, 0, 150))
    cshadow = cshadow.filter(ImageFilter.GaussianBlur(4))
    char_x_pos = room_x + 580
    char_y_pos = room_y + 250
    img.alpha_composite(cshadow, (char_x_pos - 10, char_y_pos + char_size - 30))
    img.alpha_composite(char_scaled, (char_x_pos, char_y_pos))

# Annotations on combined room
draw.text((room_x + 20, room_y + 20), "L4 moss patches (large organic spans)", fill="#9CFF9C", font=f_small)
draw.text((room_x + 700, room_y + 80), "Urn prop", fill="#FFCFA0", font=f_small)
draw.text((room_x + 850, room_y + 380), "L6 rift accent (big spanning oval)", fill="#FFA0A0", font=f_small)
draw.text((room_x + 1130, room_y + 380), "Brazier + warm light cone", fill="#FFD080", font=f_small)
draw.text((room_x + 580, room_y + 240), "Character (3x scale ANCHORS/04_ranger.png)", fill="#FFFFE0", font=f_small)

# ============ SECTION 3: KEY INSIGHT ============
ImageDraw.Draw(img).line([(100, 1115), (W - 100, 1115)], fill="#4A5F7A", width=2)
center_text(img, W / 2, 1145, "3. NEDEN ARTIK GRID GORUNMUYOR?", f_title, "#FFB347")

insights = [
    "Tile borders YOK - tile'lar arasi hard line cizilmemis, sadece per-pixel renk variation var",
    "Moss patches 80-100 piksel genis - 1-2 tile span ediyor, irregular oval = grid gizliyor",
    "Rift scar 200+ piksel genis multi-blob - dev organik leke, GRID TAMAMEN gomulu",
    "Light cones (brazier + candles) - warm glow halo'lari area tonu degistiriyor, grid kontur kayboluyor",
    "Karakter ANCHORS/04_ranger.png 3x scale - chibi sprite sahnenin merkezinde, oran tutarli",
    "Per-pixel noise + Gaussian blur ile soft edges - keskin sinir yok, oda tek butun parca hissi",
]
for i, line in enumerate(insights):
    draw.text((140, 1190 + i * 28), "* " + line, fill="#E8DCC4", font=f_text)

# Footer
ImageDraw.Draw(img).line([(100, H - 40), (W - 100, H - 40)], fill="#4A5F7A", width=2)
center_text(img, W / 2, H - 25, "RIMA Organic Room Render - 2026-05-18 S87", f_small, "#6E7E92")

# Final composite to RGB
final = Image.new("RGB", (W, H), (26, 36, 56))
final.paste(img, (0, 0), img)
out_path = "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/RIMA_ORGANIC_ROOM_RENDER.png"
final.save(out_path, "PNG", optimize=True)
print(f"Organic room render saved: {os.path.getsize(out_path)} bytes -> {out_path}")
