# Codex Task — 3D Box Outline Sheet (Sprint 1 P0 Tall Walls)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Python PIL ile RIMA Phase 1 Sprint 1 için **3D box outline sheet** üret.

Bu sheet PixelLab Web UI **Edit Image (Pro)** tool'una input olarak verilecek. AI bu outline'ları RIMA Shattered Keep duvarlarına dönüştürecek.

## Output Specs

**Path:** `STAGING/_pixellab_inputs/act1_sprint1_tall_outlines_v1.png`

**Canvas:** 512×512 PNG, transparent background

**Layout: 2×2 grid, 16px gutter between slots**

```
+---------------------+---------------------+
| Slot 1 (240×240)    | Slot 2 (240×240)   |
| wall_tall_straight  | wall_tall_corner   |
| target: 96×160      | target: 96×160     |
+---------------------+---------------------+
| Slot 3 (240×240)    | Slot 4 (240×240)   |
| wall_archway        | wall_endcap_column |
| target: 128×160     | target: 48×160     |
+---------------------+---------------------+
```

## Slot Specs

Her slot:
- 240×240 alan içinde
- **Light gray slot border** (#A0A0A0, 1px) — slot sınırı görsel
- **Slot label** üstünde (slot'un dışında, üst margin'de): "wall_tall_straight 96×160" tarzı, font small
- **3D box outline** slot'un içinde MERKEZLENMİŞ, target dimensions'ta
- Box çizgileri: **light gray** (#808080), 1-2px line width
- Slot dışında ve arasında **transparent**

## 3D Iso Box Specs (Per Slot)

**Iso projection:** 30° dimetric (2:1 ratio)
- World X axis → screen ekseni: (cos(30°), sin(30°)) ≈ (0.866, 0.5)
- World Y axis → screen ekseni: (-cos(30°), sin(30°)) ≈ (-0.866, 0.5)  
- World Z axis → screen ekseni: (0, -1) (yukarı)

**Box dimensions (world space):**
- wall_tall_straight: width=96, depth=32, height=160
- wall_tall_corner: L-shape (96×96 footprint with corner cutout), height=160
- wall_archway: width=128, depth=32, height=160 (with arch opening in middle-bottom)
- wall_endcap_column: width=48, depth=48, height=160

**Box vertices:** 8 corners of cube → projeksiyon
**Box edges:** 12 lines connecting corners + 3 hidden (dashed) for clarity
**Visible faces:** front, right, top (3 faces visible in iso)
**Hidden edges:** back-bottom diagonals (dashed thin line)

**Archway special:**
- Box outline çiz
- Ön yüzde arch opening çiz (yarım daire üst, dikey kenarlar, 64px width × 96px tall opening centered horizontally)
- Arch içi transparent (cyan rift için boş)

**Corner (L-shape) special:**
- 2 cube outline, biri X eksenine 1 birim diğeri Y eksenine 1 birim
- Birleşim noktasında corner mass
- L-shape footprint outline

## Python Skeleton

```python
from PIL import Image, ImageDraw, ImageFont
import math

SQRT3_2 = math.sqrt(3) / 2  # ~0.866

def project_iso(x, y, z, origin=(0, 0), scale=1.0):
    """30° dimetric isometric projection. Y axis flipped because PIL Y down."""
    screen_x = origin[0] + (x - y) * SQRT3_2 * scale
    screen_y = origin[1] + (x + y) * 0.5 * scale - z * scale
    return (screen_x, screen_y)

def draw_iso_box(draw, x0, y0, w, d, h, color=(128, 128, 128), line_width=1):
    """Draw 3D box outline in iso projection. (x0,y0) = base footprint origin (screen)."""
    # 8 vertices: world coords (0,0,0) to (w,d,h)
    v = [
        project_iso(0, 0, 0, origin=(x0, y0)),  # 0: front-bottom-left
        project_iso(w, 0, 0, origin=(x0, y0)),  # 1: front-bottom-right
        project_iso(w, d, 0, origin=(x0, y0)),  # 2: back-bottom-right
        project_iso(0, d, 0, origin=(x0, y0)),  # 3: back-bottom-left
        project_iso(0, 0, h, origin=(x0, y0)),  # 4: front-top-left
        project_iso(w, 0, h, origin=(x0, y0)),  # 5: front-top-right
        project_iso(w, d, h, origin=(x0, y0)),  # 6: back-top-right
        project_iso(0, d, h, origin=(x0, y0)),  # 7: back-top-left
    ]
    # 12 edges (solid for visible, dashed for hidden)
    visible = [(0,1),(1,2),(0,4),(1,5),(2,6),(4,5),(5,6),(6,7),(7,4)]
    hidden = [(0,3),(2,3),(3,7)]  # back-left edges
    for a, b in visible:
        draw.line([v[a], v[b]], fill=color, width=line_width)
    for a, b in hidden:
        # Dashed line approximation
        x1, y1 = v[a]; x2, y2 = v[b]
        steps = 8
        for i in range(0, steps, 2):
            t0 = i/steps; t1 = (i+1)/steps
            draw.line([(x1 + (x2-x1)*t0, y1 + (y2-y1)*t0),
                       (x1 + (x2-x1)*t1, y1 + (y2-y1)*t1)],
                       fill=color, width=line_width)

# Canvas
img = Image.new("RGBA", (512, 512), (0, 0, 0, 0))
draw = ImageDraw.Draw(img)

# Slot border + label color
slot_border = (160, 160, 160, 255)
box_color = (128, 128, 128, 255)
label_color = (180, 180, 180, 255)

# Slot positions (top-left corners of each 240×240 slot)
slots = [
    (16, 32, "wall_tall_straight 96x160", 96, 32, 160),
    (272, 32, "wall_tall_corner 96x160 L-shape", 96, 96, 160),  # L-shape special
    (16, 288, "wall_archway 128x160", 128, 32, 160),  # archway special
    (272, 288, "wall_endcap_column 48x160", 48, 48, 160),
]

# Draw each slot
for sx, sy, label, bw, bd, bh in slots:
    # Slot border
    draw.rectangle([sx, sy, sx+240, sy+240], outline=slot_border, width=1)
    # Label above slot
    # Use default font
    draw.text((sx+4, sy-18), label, fill=label_color)
    # Center box inside slot
    box_origin_x = sx + 120 - bw//4  # center horizontally for iso projection
    box_origin_y = sy + 200          # near bottom of slot
    # TODO: special handling for archway + corner
    draw_iso_box(draw, box_origin_x, box_origin_y, bw, bd, bh, color=box_color, line_width=2)

# Save
img.save("STAGING/_pixellab_inputs/act1_sprint1_tall_outlines_v1.png")
```

**Note:** Archway ve corner için ek geometri lazım. Archway = box + arch opening front face'te (yarım daire çizimi). Corner = 2 box L-shape footprint.

Bu skeleton'ı tamamla, edge cases handle et, output PNG üret.

## Verification

Çıktı sheet'i şu QC'den geçmeli:
- ✅ 512×512 PNG
- ✅ Transparent background (slot dışı)
- ✅ 4 slot görünür (light gray border)
- ✅ Her slot'ta iso 3D box outline (light gray lines)
- ✅ Slot labels readable (üst margin'de)
- ✅ Archway slot'ta arch opening boş (transparent inside arch)
- ✅ Corner slot'ta L-shape topology

## Effort

medium — concrete Python implementation, math doğru olmalı, edge cases (archway/corner) için yaratıcı geometry. Skeleton verildi, refine edilip çalıştırılacak.

## Output Confirmation

Üretim sonrası rapor et:
- Output path
- PNG dimensions
- Slot count + label list
- Any issues / edge cases (archway/corner geometry)
