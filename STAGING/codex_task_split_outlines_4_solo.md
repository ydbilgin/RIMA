# Codex Task — Split Master Outline into 4 Solo Reference PNGs

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

`STAGING/_pixellab_inputs/act1_sprint1_tall_outlines_v1.png` (512×512 master sheet) mevcut. Bu sheet'ten 4 ayrı **solo reference PNG** üret. Her biri **target wall dimensions**'da, içinde sadece o piece'in 3D box outline'ı olacak.

User Web UI Edit Image (Pro)'ya **tek tek** yükleyecek, her bir wall için ayrı generate edecek.

## Output Dosyalar

| Dosya | Boyut (canvas W×H) | İçerik |
|---|---|---|
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_box.png` | **96×160** | 3D box outline (straight wall) |
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_box.png` | **128×160** | 3D L-shape box outline (corner) |
| `STAGING/_pixellab_inputs/solo/act1_wall_archway_box.png` | **128×160** | 3D box + arch opening |
| `STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_box.png` | **48×160** | Narrow vertical box (endcap) |

## Yöntem (Tercih)

**Tercih: Yeniden generate** (daha temiz output, target dimensions native)

Her PNG için:
- Canvas = target W × H exactly (96×160, 128×160, 128×160, 48×160)
- Transparent background
- İçinde 30° dimetric iso 3D box outline, **canvas'a sığacak şekilde merkezlenmiş**
- Outline: light gray (#808080), 2px line width
- Hidden edges: dashed line
- **Slot label / construction frame YOK** — sadece box outline

Master sheet'teki box geometry'sini kullan, target canvas'a yeniden render et. Margins minimum (5-10px her kenardan).

**Alternatif:** Master sheet'i crop et (PIL crop), background transparent koruyarak slot label'ları kaldır.

## Output Specs (Her Solo PNG)

1. **act1_wall_tall_straight_box.png** (96×160):
   - Box: 90 wide × 30 depth × 156 tall (5px her kenardan margin)
   - Front face görünür (96 width)
   - Tek box, düz wall

2. **act1_wall_tall_corner_box.png** (128×160):
   - L-shape: 2 box, biri X yönde, diğeri Y yönde
   - Iso projection ile fit (toplam görsel width ~120, height ~156)
   - Corner mass birleşim noktasında

3. **act1_wall_archway_box.png** (128×160):
   - Box: 120 wide × 30 depth × 156 tall
   - Front face'te arch opening: 60 wide × 90 tall, alttan başlar, yarım daire üst
   - Arch içi tamamen transparent (boş)

4. **act1_wall_endcap_column_box.png** (48×160):
   - Box: 42 wide × 42 depth × 156 tall
   - Düz column / endcap, capstone üst

## Python Skeleton

```python
from PIL import Image, ImageDraw
import math

SQRT3_2 = math.sqrt(3) / 2

def project_iso(x, y, z, origin=(0, 0)):
    sx = origin[0] + (x - y) * SQRT3_2
    sy = origin[1] + (x + y) * 0.5 - z
    return (sx, sy)

def draw_iso_box(draw, x0, y0, w, d, h, color=(128,128,128), lw=2):
    v = [
        project_iso(0, 0, 0, (x0, y0)),
        project_iso(w, 0, 0, (x0, y0)),
        project_iso(w, d, 0, (x0, y0)),
        project_iso(0, d, 0, (x0, y0)),
        project_iso(0, 0, h, (x0, y0)),
        project_iso(w, 0, h, (x0, y0)),
        project_iso(w, d, h, (x0, y0)),
        project_iso(0, d, h, (x0, y0)),
    ]
    visible = [(0,1),(1,2),(0,4),(1,5),(2,6),(4,5),(5,6),(6,7),(7,4)]
    hidden = [(0,3),(2,3),(3,7)]
    for a,b in visible:
        draw.line([v[a], v[b]], fill=color, width=lw)
    for a,b in hidden:
        # dashed
        x1,y1 = v[a]; x2,y2 = v[b]
        steps = 6
        for i in range(0, steps, 2):
            t0 = i/steps; t1 = (i+1)/steps
            draw.line([(x1+(x2-x1)*t0, y1+(y2-y1)*t0),
                       (x1+(x2-x1)*t1, y1+(y2-y1)*t1)],
                      fill=color, width=lw)

import os
os.makedirs("STAGING/_pixellab_inputs/solo", exist_ok=True)

# 1. wall_tall_straight 96x160
img = Image.new("RGBA", (96, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
# Center box: w=80 (96-16 margin), d=20, h=140
# Origin: bottom-left of box footprint in image
ox = 8  # x0 in image
oy = 152  # bottom (y grows down in PIL)
draw_iso_box(d, ox, oy, 80, 20, 140)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_box.png")

# 2. wall_tall_corner 128x160 — L-shape (2 boxes)
img = Image.new("RGBA", (128, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
# Two boxes: arm1 (X direction) + arm2 (Y direction), meeting at corner
# Box 1: w=60, d=20, h=140, origin=(20, 150)
# Box 2: w=20, d=60, h=140, origin=(20, 150) (Y arm extends backward)
draw_iso_box(d, 20, 150, 60, 20, 140)
draw_iso_box(d, 20, 150, 20, 60, 140)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_box.png")

# 3. wall_archway 128x160
img = Image.new("RGBA", (128, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
# Box: w=100, d=20, h=140
ox = 8; oy = 152
draw_iso_box(d, ox, oy, 100, 20, 140)
# Arch opening on front face (z=0..z=h, x=arch_left..x=arch_right)
# Arch: 50 wide × 80 tall, centered horizontally on front face
arch_left = ox + 25 * SQRT3_2  # x=25 in world
arch_right = ox + 75 * SQRT3_2  # x=75 in world  
arch_bottom_y = oy + 25 * 0.5  # at z=0
arch_top_y = oy + 25 * 0.5 - 80  # at z=80
# Draw arch outline: vertical lines + arc on top
d.line([(arch_left, arch_bottom_y), (arch_left, arch_top_y)], fill=(128,128,128), width=2)
d.line([(arch_right, arch_bottom_y), (arch_right, arch_top_y)], fill=(128,128,128), width=2)
# Arc top
import math
center_x = (arch_left + arch_right) / 2
arc_radius = (arch_right - arch_left) / 2
# Semicircle from arch_left,arch_top_y to arch_right,arch_top_y (curve upward)
bbox = [center_x - arc_radius, arch_top_y - arc_radius, center_x + arc_radius, arch_top_y + arc_radius]
d.arc(bbox, start=180, end=360, fill=(128,128,128), width=2)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_archway_box.png")

# 4. wall_endcap_column 48x160
img = Image.new("RGBA", (48, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
# Box: w=32, d=32, h=140
ox = 8; oy = 152
draw_iso_box(d, ox, oy, 32, 32, 140)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_box.png")
```

**Math note:** Box origin (ox, oy) screen koordinatlarında, y aşağı doğru artıyor. Iso projection sonrası box yukarı doğru çıkar.

`oy` değerini ayarlarken: box'ın top-back-left corner görsel olarak yukarıda olmalı, front-bottom-right ise canvas'ın alt-sağında. Margin korunmalı.

Skeleton'ı tamamla, edge case'leri (arch geometry doğru) handle et, 4 PNG üret.

## Verification

Her PNG için QC:
- ✅ Canvas tam target boyut (96×160 / 128×160 / 128×160 / 48×160)
- ✅ Transparent background
- ✅ Box outline canvas'a sığar, hiçbir kenar dışarı taşmaz
- ✅ Iso 30° dimetric angle korunur
- ✅ Archway slot'ta arch opening transparent (içi boş)
- ✅ Corner L-shape iki box görünür

## Effort

medium — math doğru, 4 ayrı PNG, edge case (archway arc) dikkat.

## Output Confirmation

Tüm 4 PNG path + dimensions + brief note (issues if any).
