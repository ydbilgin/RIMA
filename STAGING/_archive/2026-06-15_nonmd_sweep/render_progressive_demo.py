"""4-stage progressive demo: grid -> organic Hades-feel through layered overlays."""
from PIL import Image, ImageDraw, ImageFont
import os
import random

W, H = 1800, 1400
img = Image.new("RGB", (W, H), "#1A2438")
draw = ImageDraw.Draw(img)


def load_font(size, bold=False):
    paths = [
        "C:/Windows/Fonts/segoeuib.ttf" if bold else "C:/Windows/Fonts/segoeui.ttf",
        "C:/Windows/Fonts/arialbd.ttf" if bold else "C:/Windows/Fonts/arial.ttf",
    ]
    for p in paths:
        if os.path.exists(p):
            return ImageFont.truetype(p, size)
    return ImageFont.load_default()


f_huge = load_font(34, bold=True)
f_title = load_font(22, bold=True)
f_section = load_font(20, bold=True)
f_text = load_font(15)
f_text_b = load_font(15, bold=True)
f_small = load_font(12)
f_tiny = load_font(10)


def center_text(x, y, text, font, fill="#E8DCC4"):
    bbox = draw.textbbox((0, 0), text, font=font)
    tw = bbox[2] - bbox[0]
    draw.text((x - tw / 2, y), text, fill=fill, font=font)


# Title
center_text(W / 2, 30, "RIMA Layered Painting - 4 Asamali Donusum", f_huge)
center_text(W / 2, 75, "Grid-tile uretiminden dogal gorunumlu, yapay durmayan odaya nasil ulasiyoruz?", f_text, "#9EB3C2")
draw.line([(150, 105), (W - 150, 105)], fill="#4A5F7A", width=2)

# Room renderer
def render_room(ox, oy, room_w, room_h, stage):
    """Render a room at stage 1-4 (progressively richer)."""
    # Border (background)
    draw.rectangle([(ox, oy), (ox + room_w, oy + room_h)], fill="#2D3548", outline="#4A5F7A", width=3)

    tile_size = 60
    cols = room_w // tile_size
    rows = room_h // tile_size

    # Floor variant colors (StoneDungeon)
    tile_colors = ["#4A3A2A", "#3D2D1F", "#4D3A28", "#3F2E1F", "#4A3A2A", "#3D2D1F"]
    grid_color = "#5A4A3A"

    random.seed(101)

    # STAGE 1+: floor tiles
    for tx in range(cols):
        for ty in range(rows):
            x1 = ox + tx * tile_size
            y1 = oy + ty * tile_size
            c = random.choice(tile_colors)
            draw.rectangle([(x1, y1), (x1 + tile_size, y1 + tile_size)], fill=c, outline=grid_color, width=1)

    # STAGE 2+: Wang16 walls (border)
    if stage >= 2:
        wall = "#4A4A5A"
        wall_outline = "#7A7A9A"
        wt = 24
        # top
        for tx in range(cols):
            x1 = ox + tx * tile_size
            draw.rectangle([(x1, oy), (x1 + tile_size, oy + wt)], fill=wall, outline=wall_outline, width=1)
        # bottom
        for tx in range(cols):
            x1 = ox + tx * tile_size
            draw.rectangle([(x1, oy + room_h - wt), (x1 + tile_size, oy + room_h)], fill=wall, outline=wall_outline, width=1)
        # left
        for ty in range(rows):
            y1 = oy + ty * tile_size
            draw.rectangle([(ox, y1), (ox + wt, y1 + tile_size)], fill=wall, outline=wall_outline, width=1)
        # right
        for ty in range(rows):
            y1 = oy + ty * tile_size
            draw.rectangle([(ox + room_w - wt, y1), (ox + room_w, y1 + tile_size)], fill=wall, outline=wall_outline, width=1)

    # STAGE 3+: L4 Organic (moss patches spanning multiple tiles)
    if stage >= 3:
        # Moss patches - irregular ovals that SPAN tile boundaries
        moss_patches = [
            (ox + 90, oy + 260, 95, 50),
            (ox + 380, oy + 80, 110, 60),
            (ox + 250, oy + 300, 80, 45),
            (ox + 50, oy + 100, 70, 40),
            (ox + 450, oy + 320, 85, 50),
        ]
        for mx, my, mw, mh in moss_patches:
            draw.ellipse([(mx, my), (mx + mw, my + mh)], fill="#3A5A3A")
            # Smaller moss tuft inside for depth
            draw.ellipse([(mx + 15, my + 10), (mx + mw - 15, my + mh - 10)], fill="#4A6A4A")

        # L5 cracks (lines crossing tiles)
        cracks = [
            [(ox + 100, oy + 180), (ox + 220, oy + 200), (ox + 280, oy + 240)],
            [(ox + 350, oy + 220), (ox + 420, oy + 250), (ox + 480, oy + 270)],
            [(ox + 150, oy + 350), (ox + 230, oy + 380)],
        ]
        for crack in cracks:
            for i in range(len(crack) - 1):
                draw.line([crack[i], crack[i+1]], fill="#1A1A1A", width=2)

        # Small pebbles + bones scatter
        random.seed(202)
        for _ in range(15):
            px = ox + random.randint(40, room_w - 40)
            py = oy + random.randint(40, room_h - 40)
            sz = random.randint(2, 4)
            draw.ellipse([(px - sz, py - sz), (px + sz, py + sz)], fill="#5A4A3A")

    # STAGE 4: L6 Accents + Props + lighting
    if stage >= 4:
        # L6 large rift scar (sprawling across center, organic shape)
        # Multiple overlapping dark stains
        draw.ellipse([(ox + 200, oy + 150), (ox + 380, oy + 250)], fill="#4A1F2A")
        draw.ellipse([(ox + 230, oy + 170), (ox + 350, oy + 230)], fill="#3A1A1F")
        # Crack lines radiating from rift
        rift_cx, rift_cy = ox + 290, oy + 200
        for angle_deg in [30, 80, 130, 200, 260, 320]:
            import math
            ang = math.radians(angle_deg)
            ex = rift_cx + int(70 * math.cos(ang))
            ey = rift_cy + int(40 * math.sin(ang))
            draw.line([(rift_cx, rift_cy), (ex, ey)], fill="#2A0A0A", width=2)

        # Props
        # Crate (top-left area)
        cx, cy = ox + 80, oy + 220
        draw.rectangle([(cx, cy), (cx + 40, cy + 40)], fill="#5A4A3A", outline="#1A1A1A", width=2)
        draw.line([(cx, cy + 8), (cx + 40, cy + 8)], fill="#3A2A1A", width=1)
        draw.line([(cx, cy + 32), (cx + 40, cy + 32)], fill="#3A2A1A", width=1)
        draw.ellipse([(cx - 4, cy + 42), (cx + 44, cy + 50)], fill="#000000")

        # Urn (top-right)
        ux, uy = ox + 470, oy + 90
        draw.ellipse([(ux - 12, uy - 4), (ux + 12, uy + 4)], fill="#3A4045")
        draw.polygon([
            (ux - 10, uy), (ux - 10, uy + 22),
            (ux, uy + 28), (ux + 10, uy + 22), (ux + 10, uy)
        ], fill="#4A5050", outline="#1A1A1A")
        draw.ellipse([(ux - 12, uy + 28), (ux + 12, uy + 33)], fill="#000000")

        # Candle pair (right side)
        for cidx, (clx, cly) in enumerate([(ox + 510, oy + 220), (ox + 510, oy + 280)]):
            draw.rectangle([(clx, cly), (clx + 4, cly + 18)], fill="#E8DCC4")
            draw.ellipse([(clx - 1, cly - 8), (clx + 5, cly + 4)], fill="#FFB347")
            # Glow halo
            for r in range(20, 5, -3):
                alpha_overlay = Image.new("RGBA", (r*2, r*2), (255, 179, 71, 30))
                # simple emulation: small filled ellipse w/ low alpha approximated by light color
                draw.ellipse([(clx + 2 - r, cly - 2 - r), (clx + 2 + r, cly - 2 + r)], outline=None)
            # Center bright glow
            draw.ellipse([(clx - 8, cly - 10), (clx + 12, cly + 8)], outline="#FFB347", width=1)

        # Brazier (bottom area)
        bx, by = ox + 290, oy + 360
        draw.rectangle([(bx - 22, by + 5), (bx + 22, by + 15)], fill="#2A2A2A", outline="#1A1A1A", width=2)
        draw.rectangle([(bx - 18, by - 5), (bx + 18, by + 5)], fill="#3A3A3A", outline="#1A1A1A", width=2)
        draw.ellipse([(bx - 14, by - 10), (bx + 14, by - 2)], fill="#FF6347")
        draw.ellipse([(bx - 10, by - 14), (bx + 10, by - 6)], fill="#FFB347")
        # Warm glow halo (light area around brazier)
        draw.ellipse([(bx - 35, by - 30), (bx + 35, by + 30)], outline="#5A3A1A", width=2)
        # Ground shadow
        draw.ellipse([(bx - 25, by + 15), (bx + 25, by + 22)], fill="#000000")


# 4 panels side by side
panel_w = 540
panel_h = 460
panel_gap = 20
panels_total = panel_w * 2 + panel_gap
panel_start_x = (W - panels_total) / 2

# Row 1: stages 1 and 2
# Row 2: stages 3 and 4
row1_y = 160
row2_y = row1_y + panel_h + 100

# Stage 1 (Top-left)
stage1_x = panel_start_x
center_text(stage1_x + panel_w / 2, row1_y - 35, "ASAMA 1 - Sadece Floor Tiles", f_section, "#9EB3C2")
center_text(stage1_x + panel_w / 2, row1_y - 12, "Grid hissi belirgin, sterile arena", f_small, "#FFCACA")
render_room(stage1_x, row1_y, panel_w, panel_h, 1)
draw.text((stage1_x + 10, row1_y + panel_h - 30), "L2 Floor Atlas only", fill="#FFCACA", font=f_small)

# Stage 2 (Top-right)
stage2_x = panel_start_x + panel_w + panel_gap
center_text(stage2_x + panel_w / 2, row1_y - 35, "ASAMA 2 - + L3 Walls Wang16", f_section, "#9EB3C2")
center_text(stage2_x + panel_w / 2, row1_y - 12, "Sinirlar belirli ama hala grid hissi", f_small, "#FFCACA")
render_room(stage2_x, row1_y, panel_w, panel_h, 2)
draw.text((stage2_x + 10, row1_y + panel_h - 30), "+ L3 Wang16 wall border", fill="#FFCACA", font=f_small)

# Stage 3 (Bottom-left)
stage3_x = panel_start_x
center_text(stage3_x + panel_w / 2, row2_y - 35, "ASAMA 3 - + L4 Organic + L5 Detail", f_section, "#9EB3C2")
center_text(stage3_x + panel_w / 2, row2_y - 12, "MOSS PATCHES tile sinirlarini AsAR - grid kayboluyor", f_small, "#CAFFCA")
render_room(stage3_x, row2_y, panel_w, panel_h, 3)
draw.text((stage3_x + 10, row2_y + panel_h - 30), "+ Moss patches (cross tiles) + cracks + bone scatter", fill="#CAFFCA", font=f_small)

# Stage 4 (Bottom-right)
stage4_x = panel_start_x + panel_w + panel_gap
center_text(stage4_x + panel_w / 2, row2_y - 35, "ASAMA 4 - + L6 Accent + Props + Lighting", f_section, "#9EB3C2")
center_text(stage4_x + panel_w / 2, row2_y - 12, "Doga gorunumlu, yapay durmayan, hikaye anlatici oda", f_small, "#CAFFCA")
render_room(stage4_x, row2_y, panel_w, panel_h, 4)
draw.text((stage4_x + 10, row2_y + panel_h - 30), "+ Rift scar + crate/urn/candles/brazier + glow halos", fill="#CAFFCA", font=f_small)

# Key insight
insight_y = row2_y + panel_h + 90
draw.line([(150, insight_y), (W - 150, insight_y)], fill="#4A5F7A", width=2)
center_text(W / 2, insight_y + 25, "ANAHTAR ICGORU", f_title, "#FFB347")
center_text(W / 2, insight_y + 65, "Dogal gorunum tile'lardan DEGIL, tile'lari ASAN overlay'lerden geliyor.", f_text_b, "#E8DCC4")
center_text(W / 2, insight_y + 95, "Moss patch 96x64 piksel = 1.5x1 tile = grid'i goz acici sekilde KIRAR.", f_text, "#9EB3C2")
center_text(W / 2, insight_y + 122, "Rift scar 128x128 = 2x2 tile spanning, irregular oval = 'yapay durmayan' gorunum sagliyor.", f_text, "#9EB3C2")
center_text(W / 2, insight_y + 152, "Props ground shadow (oval) + Bridson scatter = grid hizalama yok.", f_text, "#9EB3C2")
center_text(W / 2, insight_y + 182, "Composition role radial density (Sprint 11 LIVE) = orta yogun, kenar seyrek = dogal gradient.", f_text, "#9EB3C2")

# Footer
draw.line([(150, H - 60), (W - 150, H - 60)], fill="#4A5F7A", width=2)
center_text(W / 2, H - 45, "RIMA Layered Painting Progressive Demo - 4 stages reveal the natural look mechanism", f_small, "#9EB3C2")
center_text(W / 2, H - 25, "2026-05-18 S87 - Laureth Studio knowledge base", f_tiny, "#6E7E92")

out_path = "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/RIMA_LAYERED_PAINTING_PROGRESSIVE_DEMO.png"
img.save(out_path, "PNG", optimize=True)
print(f"Progressive demo saved: {os.path.getsize(out_path)} bytes -> {out_path}")
