# Codex Task — Filled 3D Box Silhouettes (Replace Wireframe Outlines)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Mevcut wireframe outline solo PNG'leri (`STAGING/_pixellab_inputs/solo/act1_wall_*_box.png`) **filled 3D silhouette** versiyonlarıyla replace et.

Sorun: Wireframe versiyonda AI construction line'larını kısmen output'a sızdırdı (bounding box artifacts). Filled silhouette ile AI'a sadece **3D shape** ver, internal line yok.

## Output Specs — 4 PNG

| Dosya | Boyut | İçerik |
|---|---|---|
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_silhouette.png` | 96×160 | Filled iso 3D box silhouette |
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_silhouette.png` | 128×160 | Filled L-shape 3D silhouette |
| `STAGING/_pixellab_inputs/solo/act1_wall_archway_silhouette.png` | 128×160 | Filled box + transparent arch opening |
| `STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_silhouette.png` | 48×160 | Filled narrow column silhouette |

## Filled Silhouette Specs

Her box için **3 görünür face** ayrı shade ile filled:

| Face | Shade | Hex |
|---|---|---|
| **Top face** | En açık gri | `#A0A0A0` |
| **Front face** (camera-facing) | Orta gri | `#808080` |
| **Right face** (right side) | En koyu gri | `#606060` |

Bu 3-shade differentiation AI'a subtle 3D form ipucu verir, internal line gerek kalmaz.

**Kenarlar:** Solid edge, no outline lines (filled polygon → kenarda doğal olarak siyah border yok, sadece face renkleri).

**Hidden edges (back):** Görünmez (filled polygon kapsayacak).

**Background:** Transparent.

## Python Skeleton

```python
from PIL import Image, ImageDraw
import math
import os

SQRT3_2 = math.sqrt(3) / 2

def project(x, y, z, origin):
    sx = origin[0] + (x - y) * SQRT3_2
    sy = origin[1] + (x + y) * 0.5 - z
    return (sx, sy)

# Color palette for 3 faces
TOP_COLOR = (160, 160, 160, 255)    # lightest
FRONT_COLOR = (128, 128, 128, 255)  # mid
RIGHT_COLOR = (96, 96, 96, 255)     # darkest

def draw_filled_iso_box(draw, x0, y0, w, d, h):
    """Filled 3D iso box with 3 visible faces shaded."""
    # 8 vertices
    v = {
        'fbl': project(0, 0, 0, (x0, y0)),  # front-bottom-left
        'fbr': project(w, 0, 0, (x0, y0)),  # front-bottom-right
        'bbr': project(w, d, 0, (x0, y0)),  # back-bottom-right
        'bbl': project(0, d, 0, (x0, y0)),  # back-bottom-left
        'ftl': project(0, 0, h, (x0, y0)),  # front-top-left
        'ftr': project(w, 0, h, (x0, y0)),  # front-top-right
        'btr': project(w, d, h, (x0, y0)),  # back-top-right
        'btl': project(0, d, h, (x0, y0)),  # back-top-left
    }
    # 3 visible faces — draw in back-to-front order
    # Front face: fbl, fbr, ftr, ftl
    draw.polygon([v['fbl'], v['fbr'], v['ftr'], v['ftl']], fill=FRONT_COLOR)
    # Right face: fbr, bbr, btr, ftr
    draw.polygon([v['fbr'], v['bbr'], v['btr'], v['ftr']], fill=RIGHT_COLOR)
    # Top face: ftl, ftr, btr, btl
    draw.polygon([v['ftl'], v['ftr'], v['btr'], v['btl']], fill=TOP_COLOR)

os.makedirs("STAGING/_pixellab_inputs/solo", exist_ok=True)

# 1. wall_tall_straight 96×160
img = Image.new("RGBA", (96, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
draw_filled_iso_box(d, 8, 152, 80, 20, 140)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_silhouette.png")

# 2. wall_tall_corner 128×160 — L-shape (2 box union)
img = Image.new("RGBA", (128, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
# Box 1: X-arm w=60, d=20, h=140
draw_filled_iso_box(d, 20, 150, 60, 20, 140)
# Box 2: Y-arm w=20, d=60, h=140
draw_filled_iso_box(d, 20, 150, 20, 60, 140)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_silhouette.png")

# 3. wall_archway 128×160 — Filled box + transparent arch cutout
img = Image.new("RGBA", (128, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
ox, oy = 8, 152
draw_filled_iso_box(d, ox, oy, 100, 20, 140)
# Cutout arch from front face: 60×96, centered horizontally
# Compute arch position in screen coords (front face is at y=0 world)
# In screen space, front face goes from project(0,0,0)→project(100,0,140)
arch_world_left = 20    # 20..80 in world X
arch_world_right = 80
arch_world_bottom = 0   # z=0
arch_world_top = 80     # z=80 (arch top before curve)
arch_curve_top = 95     # z=95 (apex of arc)

# Convert to screen polygon for the arch region on front face
# Front face is in plane y=0, so arch points all have y=0
# Polygon points (filled with TRANSPARENT = effectively erase)
arch_points = [
    project(arch_world_left, 0, arch_world_bottom, (ox, oy)),
    project(arch_world_right, 0, arch_world_bottom, (ox, oy)),
    project(arch_world_right, 0, arch_world_top, (ox, oy)),
]
# Add semicircle points for arch curve
import math as m
center_x_world = (arch_world_left + arch_world_right) / 2
radius_world = (arch_world_right - arch_world_left) / 2
# Sample arc from right to left over top
for angle in range(0, 181, 15):
    rad = m.radians(angle)
    x_world = center_x_world + radius_world * m.cos(rad)
    z_world = arch_world_top + radius_world * m.sin(rad)
    arch_points.append(project(x_world, 0, z_world, (ox, oy)))
arch_points.append(project(arch_world_left, 0, arch_world_top, (ox, oy)))

# Erase arch from filled box (alpha = 0 fill)
d.polygon(arch_points, fill=(0,0,0,0))
img.save("STAGING/_pixellab_inputs/solo/act1_wall_archway_silhouette.png")

# 4. wall_endcap_column 48×160
img = Image.new("RGBA", (48, 160), (0,0,0,0))
d = ImageDraw.Draw(img)
draw_filled_iso_box(d, 8, 152, 32, 32, 140)
img.save("STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_silhouette.png")

print("Done — 4 filled silhouettes generated.")
```

## Edge Case Notes

**Archway transparent arch:**
- PIL polygon with alpha 0 fill might not work as expected (composite mode)
- Alternative: Generate the wall using mask — start with transparent canvas, draw filled wall, then use PIL's `Image.alpha_composite` with arch mask
- Veya: Use 2 layers — fill box → punch arch hole via `ImageDraw.polygon` with `fill=(0,0,0,0)` (test this works in RGBA mode)

**L-shape (corner):**
- 2 box union — drawing 2 boxes overlapping creates valid L silhouette
- Order matters: draw far box first, near box second (z-order)

**Polygon ordering:**
- Front-to-back face draw order: top first (back), then right, then front
- OR back-to-front: right, top, front
- Test which order produces correct visual (no face hidden by another)

## Verification

Her PNG için QC:
- ✅ Canvas tam target boyut
- ✅ Transparent background (silhouette dışı)
- ✅ 3 face görünür shade differentiation
- ✅ İçi LİNE YOK, sadece filled regions
- ✅ Archway arch interior TRANSPARENT (görülebilir cyan rift için)
- ✅ Corner L-shape iki box union

## Effort

medium — polygon math doğru, archway transparency tricky.

## Output Confirmation

4 PNG path + visual note (her birinin görünüşü kısa).
