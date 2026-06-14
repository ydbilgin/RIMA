"""
RIMA Painterly Pack v1 — visual contact sheet of all 50 sprites.
Organized by category with labels.
"""
import os
from PIL import Image, ImageDraw, ImageFont

REPO = r"F:/Antigravity Projeler/2d roguelite/RIMA"
PACK = os.path.join(REPO, "Assets/Sprites/Environment/RIMA_Painterly_Pack_v1")
OUT = os.path.join(REPO, "STAGING/painterly_pack_v1_CONTACT_SHEET.png")

# Categories with cell size + col count
SECTIONS = [
    ("FLOOR (8)", "floor", 8, 128, 128),
    ("WALLS (12)", "walls", 6, 128, 192),
    ("DECALS (12)", "decals", 6, 96, 96),
    ("ACCENTS (6)", "accents", 6, 192, 192),
    ("PROPS (12)", "props", 6, 128, 128),
]

CELL_PAD = 16
LABEL_H = 28
SECTION_H_PAD = 48
LEFT_PAD = 40

# Compute total dimensions
section_widths = []
section_heights = []
for title, folder, cols, cw, ch in SECTIONS:
    files = sorted(os.listdir(os.path.join(PACK, folder)))
    rows = (len(files) + cols - 1) // cols
    width = LEFT_PAD * 2 + cols * (cw + CELL_PAD)
    height = SECTION_H_PAD + rows * (ch + CELL_PAD + LABEL_H)
    section_widths.append(width)
    section_heights.append(height)

CANVAS_W = max(section_widths)
CANVAS_H = sum(section_heights) + 50

# Build canvas
canvas = Image.new("RGB", (CANVAS_W, CANVAS_H), (24, 22, 28))
draw = ImageDraw.Draw(canvas)

# Try to load font
try:
    font_title = ImageFont.truetype("arial.ttf", 32)
    font_label = ImageFont.truetype("arial.ttf", 16)
except:
    font_title = ImageFont.load_default()
    font_label = ImageFont.load_default()

y_cursor = 20
for (title, folder, cols, cw, ch), s_height in zip(SECTIONS, section_heights):
    # Section title
    draw.text((LEFT_PAD, y_cursor), title, fill=(220, 200, 160), font=font_title)
    y_cursor += 40

    files = sorted(os.listdir(os.path.join(PACK, folder)))
    for idx, fname in enumerate(files):
        col = idx % cols
        row = idx // cols
        x = LEFT_PAD + col * (cw + CELL_PAD)
        y = y_cursor + row * (ch + CELL_PAD + LABEL_H)

        # Draw cell border
        draw.rectangle([x - 2, y - 2, x + cw + 2, y + ch + 2], outline=(60, 55, 70), width=1)

        # Load and paste sprite
        sprite_path = os.path.join(PACK, folder, fname)
        sprite = Image.open(sprite_path).convert("RGBA")
        # Center within cell
        sx = x + (cw - sprite.width) // 2
        sy = y + (ch - sprite.height) // 2
        canvas.paste(sprite, (sx, sy), sprite)

        # Label
        label = fname.replace(".png", "").replace("_", " ")
        # Truncate long labels
        if len(label) > 24:
            label = label[:22] + ".."
        draw.text((x, y + ch + 4), label, fill=(140, 130, 150), font=font_label)

    # Move cursor down by section height
    rows_used = (len(files) + cols - 1) // cols
    y_cursor += rows_used * (ch + CELL_PAD + LABEL_H) + 20

canvas.save(OUT)
print(f"Saved: {OUT} ({canvas.size})")
