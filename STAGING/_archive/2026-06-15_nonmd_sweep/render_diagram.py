from PIL import Image, ImageDraw, ImageFont
import os
import random

W, H = 1400, 1800
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


f_huge = load_font(38, bold=True)
f_title = load_font(26, bold=True)
f_section = load_font(22, bold=True)
f_subtitle = load_font(18)
f_text = load_font(16)
f_text_b = load_font(16, bold=True)
f_small = load_font(13)
f_small_b = load_font(13, bold=True)
f_tiny = load_font(11)


def center_text(x, y, text, font, fill="#E8DCC4"):
    bbox = draw.textbbox((0, 0), text, font=font)
    tw = bbox[2] - bbox[0]
    draw.text((x - tw / 2, y), text, fill=fill, font=font)


# Title
center_text(W / 2, 30, "RIMA Map Painting System", f_huge)
center_text(W / 2, 80, "Brush V1 - Katmanli Mimari ve Uretim Akisi", f_subtitle, "#9EB3C2")
draw.line([(200, 110), (1200, 110)], fill="#4A5F7A", width=2)

# Section 1: Layer stack
center_text(W / 2, 145, "1. KATMAN MIMARISI", f_title, "#7BA7BC")
center_text(W / 2, 175, "(her oda ust uste katmanlardan olusur)", f_small, "#9EB3C2")

layers = [
    ("PROPS (crate, urn, brazier - stamp)",          "#6A4A2A", "#9A7A4A", 220),
    ("L6 - Accent (rift scar, battle aftermath)",    "#6A2A3A", "#8A4A5A", 274),
    ("L5 - Detail (cracks, pebbles, bone fragments)","#4A4A4A", "#6A6A6A", 328),
    ("L4 - Organic (moss, dirt patches)",            "#3A5A3A", "#5A8A5A", 382),
    ("L3 - Walls Wang16 (16-tile, auto-edge)",       "#4A4A5A", "#6A6A8A", 436),
    ("L2 - Floor Atlas (tiles, random variant)",     "#5A4A3A", "#7A6A5A", 490),
    ("L1 - Base Tone (zemin rengi)",                  "#2A2F3F", "#4A5F7A", 544),
]

layer_x, layer_w = 90, 460
for label, fill, stroke, y in layers:
    draw.rectangle([(layer_x, y), (layer_x + layer_w, y + 48)], fill=fill, outline=stroke, width=2)
    bbox = draw.textbbox((0, 0), label, font=f_text_b)
    tw = bbox[2] - bbox[0]
    draw.text((layer_x + layer_w / 2 - tw / 2, y + 14), label, fill="#E8DCC4", font=f_text_b)

draw.line([(layer_x + layer_w / 2, 200), (layer_x + layer_w / 2, 220)], fill="#7BA7BC", width=3)
draw.polygon([
    (layer_x + layer_w / 2 - 6, 215),
    (layer_x + layer_w / 2 + 6, 215),
    (layer_x + layer_w / 2, 224)
], fill="#7BA7BC")
center_text(layer_x + layer_w / 2, 200, "ust uste binerler", f_small, "#7BA7BC")

# Mock room (right side)
mock_x, mock_y = 680, 215
center_text(mock_x + 250, mock_y - 5, "Sonuc: Hades-tarzi Zengin Oda", f_subtitle, "#E8DCC4")
draw.rectangle([(mock_x, mock_y + 15), (mock_x + 500, mock_y + 335)], fill="#2D3548", outline="#4A5F7A", width=3)

random.seed(42)
tile_colors = ["#4A3A2A", "#3A2D1F", "#4D3A28", "#3D2D1B", "#3F2E1F"]
for tx in range(7):
    for ty in range(5):
        c = random.choice(tile_colors)
        x1 = mock_x + 20 + tx * 67
        y1 = mock_y + 30 + ty * 60
        draw.rectangle([(x1, y1), (x1 + 65, y1 + 58)], fill=c, outline="#5A4A3A", width=1)

wall = "#4A4A5A"
wall_outline = "#6A6A8A"
draw.rectangle([(mock_x, mock_y + 15), (mock_x + 500, mock_y + 32)], fill=wall, outline=wall_outline)
draw.rectangle([(mock_x, mock_y + 318), (mock_x + 500, mock_y + 335)], fill=wall, outline=wall_outline)
draw.rectangle([(mock_x, mock_y + 15), (mock_x + 17, mock_y + 335)], fill=wall, outline=wall_outline)
draw.rectangle([(mock_x + 483, mock_y + 15), (mock_x + 500, mock_y + 335)], fill=wall, outline=wall_outline)

draw.ellipse([(mock_x + 110, mock_y + 280), (mock_x + 180, mock_y + 320)], fill="#3A5A3A")
draw.ellipse([(mock_x + 410, mock_y + 50), (mock_x + 480, mock_y + 90)], fill="#3A5A3A")
draw.ellipse([(mock_x + 320, mock_y + 270), (mock_x + 380, mock_y + 305)], fill="#3A5A3A")

draw.line([(mock_x + 170, mock_y + 180), (mock_x + 260, mock_y + 230)], fill="#1A1A1A", width=2)
draw.line([(mock_x + 260, mock_y + 230), (mock_x + 310, mock_y + 200)], fill="#1A1A1A", width=2)
draw.line([(mock_x + 380, mock_y + 160), (mock_x + 450, mock_y + 210)], fill="#1A1A1A", width=2)

draw.ellipse([(mock_x + 240, mock_y + 140), (mock_x + 380, mock_y + 220)], fill="#3A1A1A")

# Crate
draw.rectangle([(mock_x + 130, mock_y + 155), (mock_x + 170, mock_y + 195)], fill="#5A4A3A", outline="#1A1A1A", width=2)
draw.line([(mock_x + 130, mock_y + 162), (mock_x + 170, mock_y + 162)], fill="#3A2A1A", width=1)
draw.line([(mock_x + 130, mock_y + 188), (mock_x + 170, mock_y + 188)], fill="#3A2A1A", width=1)
draw.ellipse([(mock_x + 128, mock_y + 200), (mock_x + 172, mock_y + 210)], fill="#000000")

# Urn
urn_x = mock_x + 410
urn_y = mock_y + 220
draw.ellipse([(urn_x - 15, urn_y - 5), (urn_x + 15, urn_y + 5)], fill="#3A4045")
draw.polygon([
    (urn_x - 12, urn_y), (urn_x - 12, urn_y + 25),
    (urn_x, urn_y + 32), (urn_x + 12, urn_y + 25), (urn_x + 12, urn_y)
], fill="#4A5050", outline="#1A1A1A")
draw.ellipse([(urn_x - 14, urn_y + 30), (urn_x + 14, urn_y + 36)], fill="#000000")

# Candle
draw.rectangle([(mock_x + 280, mock_y + 130), (mock_x + 286, mock_y + 155)], fill="#E8DCC4")
draw.ellipse([(mock_x + 280, mock_y + 122), (mock_x + 286, mock_y + 134)], fill="#FFB347")
draw.rectangle([(mock_x + 276, mock_y + 153), (mock_x + 290, mock_y + 162)], fill="#3A2D1F")
draw.ellipse([(mock_x + 274, mock_y + 162), (mock_x + 292, mock_y + 170)], fill="#000000")

center_text(mock_x + 250, mock_y + 350, "6 katman + props ust uste = derin doku-dolu oda", f_small, "#9EB3C2")

# Section 2: Flow
draw.line([(80, 650), (1320, 650)], fill="#4A5F7A", width=2)
center_text(W / 2, 690, "2. URETIM AKISI (PNG'den oyuna)", f_title, "#7BA7BC")

flow_y = 740
flow_boxes = [
    ("1. PIXELLAB",         "Create Image Pro V3",   "Prompt + Output Size",   "-> PNG sprite",      "#3A3A5A"),
    ("2. UNITY IMPORT",     "PPU=64",                "Point filter",           "-> Sprite asset",    "#3A4A3A"),
    ("3. BRUSH V1 REGISTER","Tile: AssetPool+Brush", "Prop: PropDefinitionSO", "-> Editor palette",  "#5A4A3A"),
    ("4. BRUSH EDITOR",     "Paint / Place mode",    "Open room asset",        "-> Room edits",      "#5A3A4A"),
    ("5. SAVE ASSET",       "RoomTemplateSO",        "Library/Combat_X.asset", "-> Persistent",      "#4A4A6A"),
    ("6. GAME RUNTIME",     "Room spawn",            "Props instantiate",      "-> Player sees",     "#3A5A4A"),
]
box_w = 195
gap = 12
total_w = box_w * 6 + gap * 5
start_x = (W - total_w) / 2
for i, (t, l1, l2, l3, c) in enumerate(flow_boxes):
    x = start_x + i * (box_w + gap)
    draw.rounded_rectangle([(x, flow_y), (x + box_w, flow_y + 110)], radius=10, fill=c, outline="#7BA7BC", width=2)
    center_text(x + box_w / 2, flow_y + 12, t, f_small_b, "#E8DCC4")
    center_text(x + box_w / 2, flow_y + 38, l1, f_small, "#9EB3C2")
    center_text(x + box_w / 2, flow_y + 58, l2, f_small, "#9EB3C2")
    center_text(x + box_w / 2, flow_y + 82, l3, f_small_b, "#FFB347")
    if i < 5:
        ax = x + box_w + 1
        ay = flow_y + 55
        draw.line([(ax, ay), (ax + gap - 2, ay)], fill="#7BA7BC", width=3)
        draw.polygon([
            (ax + gap - 2, ay - 5),
            (ax + gap + 3, ay),
            (ax + gap - 2, ay + 5)
        ], fill="#7BA7BC")

# Section 3: Aseprite parallel
draw.line([(80, 890), (1320, 890)], fill="#4A5F7A", width=2)
center_text(W / 2, 930, "3. ASEPRITE PARALELI (zihin modeli)", f_title, "#7BA7BC")

cmp_y = 985
cmp_box_w = 500
gap_mid = 100
asp_x = (W - cmp_box_w * 2 - gap_mid) / 2

draw.rounded_rectangle([(asp_x, cmp_y), (asp_x + cmp_box_w, cmp_y + 290)], radius=10, fill="#2A3548", outline="#9EB3C2", width=2)
center_text(asp_x + cmp_box_w / 2, cmp_y + 18, "ASEPRITE", f_section, "#E8DCC4")


def sub_card(x, y, w, h, title, sub, fill="#3A4558", stroke="#7BA7BC"):
    draw.rounded_rectangle([(x, y), (x + w, y + h)], radius=5, fill=fill, outline=stroke, width=1)
    center_text(x + w / 2, y + 12, title, f_text_b, "#E8DCC4")
    center_text(x + w / 2, y + 38, sub, f_small, "#9EB3C2")


asp_sub_y = cmp_y + 65
sub_card(asp_x + 40, asp_sub_y, 420, 60, "Tilemap Mode", "Tile palette + click+drag paint")
sub_card(asp_x + 40, asp_sub_y + 75, 420, 60, "Stamp Tool", "Saved selection -> click to place")
sub_card(asp_x + 40, asp_sub_y + 150, 420, 50, "Wang Tileset", "Auto-edge connection")

eq_x = asp_x + cmp_box_w + gap_mid / 2
center_text(eq_x, cmp_y + 140, "~", f_huge, "#FFB347")

rima_x = asp_x + cmp_box_w + gap_mid
draw.rounded_rectangle([(rima_x, cmp_y), (rima_x + cmp_box_w, cmp_y + 290)], radius=10, fill="#2A4838", outline="#9EB3C2", width=2)
center_text(rima_x + cmp_box_w / 2, cmp_y + 18, "RIMA BRUSH V1", f_section, "#E8DCC4")

sub_card(rima_x + 40, asp_sub_y, 420, 60, "Brush Tab", "L2/L3/L4/L5/L6 tile + decal paint", fill="#3A5848")
sub_card(rima_x + 40, asp_sub_y + 75, 420, 60, "Props Tab", "PropDefinitionSO -> click + R rotate", fill="#3A5848")
sub_card(rima_x + 40, asp_sub_y + 150, 420, 50, "Wang16 SliceTemplate", "L3_Wang16_Topdown auto-edge", fill="#3A5848")

# Section 4: Concept
draw.line([(80, 1320), (1320, 1320)], fill="#4A5F7A", width=2)
center_text(W / 2, 1360, "4. NEDEN KATMAN KATMAN?", f_title, "#7BA7BC")

concept_y = 1410
concept_w = 460
concept_gap = 100
concept_total_w = concept_w * 2 + concept_gap
concept_start_x = (W - concept_total_w) / 2

wrong_x = concept_start_x
draw.rounded_rectangle([(wrong_x, concept_y), (wrong_x + concept_w, concept_y + 240)], radius=10, fill="#4A2A2A", outline="#8A4A4A", width=2)
center_text(wrong_x + concept_w / 2, concept_y + 15, "X TEK KATMAN (sterile)", f_section, "#E8DCC4")

bad_lines = [
    "Floor tile'lar tekrarli, sikici",
    "Duvarlar duz, doku yok",
    "Hic moss, crack, decal yok",
    "Bos arena hissi",
    "Her oda ayni",
]
for i, line in enumerate(bad_lines):
    center_text(wrong_x + concept_w / 2, concept_y + 65 + i * 28, "- " + line, f_text, "#FFCACA")
center_text(wrong_x + concept_w / 2, concept_y + 215, "= Variety yok, oyuncu sikilir", f_small, "#9EB3C2")

right_x = concept_start_x + concept_w + concept_gap
draw.rounded_rectangle([(right_x, concept_y), (right_x + concept_w, concept_y + 240)], radius=10, fill="#2A4A2A", outline="#4A8A4A", width=2)
center_text(right_x + concept_w / 2, concept_y + 15, "OK 6 KATMAN + PROPS (rich)", f_section, "#E8DCC4")

good_lines = [
    "L2 random floor + L4 moss -> organic",
    "L3 Wang16 + L4 moss -> dogal duvar",
    "L6 rift scar + L5 bones -> hikaye",
    "Brazier + candles -> isikli, canli",
    "Her oda farkli kombinasyon",
]
for i, line in enumerate(good_lines):
    center_text(right_x + concept_w / 2, concept_y + 65 + i * 28, "- " + line, f_text, "#CAFFCA")
center_text(right_x + concept_w / 2, concept_y + 215, "= Hades derinligi, oyuncu kesfetmek ister", f_small, "#9EB3C2")

# Footer
draw.line([(80, 1690), (1320, 1690)], fill="#4A5F7A", width=2)
center_text(W / 2, 1715, "RIMA Map Painting System v1 - Brush V1 SHIP-READY (321/321 EditMode PASS)", f_small, "#9EB3C2")
center_text(W / 2, 1740, "Karar #74 (PPU=64) + Karar #100 (chibi 30-35) + Karar #143-E (Layered Pipeline) + Karar #144 (silahsiz body)", f_tiny, "#6E7E92")
center_text(W / 2, 1765, "2026-05-18 S87 - Laureth Studio knowledge base", f_tiny, "#6E7E92")

out_path = "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/RIMA_MAP_PAINTING_SYSTEM_DIAGRAM.png"
img.save(out_path, "PNG", optimize=True)
print(f"PNG saved: {os.path.getsize(out_path)} bytes -> {out_path}")
