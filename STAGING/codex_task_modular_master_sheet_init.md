# Codex Task — Modular Wall Asset Pack Master Sheet Init

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Python PIL ile 512×512 master sheet **init image** üret. Bu sheet PixelLab Web UI Create Image Pro'ya init olarak yüklenecek, AI dark fantasy dungeon wall asset pack'ine dönüştürecek.

## Output

`STAGING/_pixellab_inputs/master_sheets/act1_wall_modular_pack_v1.png`

512×512 PNG, transparent background, RGBA.

## Layout Spec

```
Y=0..128 (128 tall) — Section A: FEATURE pieces (128×128 each, 4 slots)
Y=128..192 (64 tall) — Section B Row 1: modular base (64×64 each, 8 slots)
Y=192..256 (64 tall) — Section B Row 2: modular extras (64×64 each, 8 slots)
Y=256..288 (32 tall) — Section C Row 1: rift overlays (32×32 each, 16 slots)
Y=288..320 (32 tall) — Section C Row 2: decoration (32×32 each, 16 slots)
Y=320..512 (192 tall) — Empty (labels only or padding)
```

Toplam: 4 + 16 + 32 = **52 slot** (essential 36 + 16 buffer)

## Slot Specs

Her slot:
- **Slot border:** light gray (#A0A0A0, 1px) hafif çerçeve
- **Slot label:** üstünde text, font default, color #B0B0B0, font size 8-10
- **Slot content:** ortalanmış outline/silhouette (aşağıdaki tabloya göre)

## Per-Slot Content Definitions

### Section A (128×128 slots, Y=0..128)

| Slot | X range | Label | Content |
|---|---|---|---|
| A1 | 0..128 | `archway_full 128x128` | 3D iso box (W=110, D=30, H=100) with arch opening (front face, 50w × 70h, semi-circle top) |
| A2 | 128..256 | `big_corner 128x128` | L-shape 3D iso (arm1 W=70, D=20; arm2 W=20, D=70; both H=100) |
| A3 | 256..384 | `big_column 128x128` | 3D iso narrow column (W=40, D=40, H=110) centered |
| A4 | 384..512 | `wall_tall_hero 128x128` | 3D iso box (W=110, D=30, H=110) — large straight wall |

### Section B Row 1 (64×64 slots, Y=128..192)

| Slot | X range | Label | Content |
|---|---|---|---|
| B1 | 0..64 | `straight_NE 64x64` | 3D iso box (W=56, D=12, H=50) — wall along NE axis |
| B2 | 64..128 | `straight_NW 64x64` | 3D iso box (W=12, D=56, H=50) — wall along NW axis |
| B3 | 128..192 | `corner_outer_a 64x64` | Small L-shape outer corner |
| B4 | 192..256 | `corner_outer_b 64x64` | Small L-shape outer corner mirrored |
| B5 | 256..320 | `corner_inner_a 64x64` | Small concave inner corner |
| B6 | 320..384 | `corner_inner_b 64x64` | Small concave inner corner mirrored |
| B7 | 384..448 | `T_junction_a 64x64` | T-shape 3D iso (3 way meeting) |
| B8 | 448..512 | `T_junction_b 64x64` | T-shape 3D iso (different orient) |

### Section B Row 2 (64×64 slots, Y=192..256)

| Slot | X range | Label | Content |
|---|---|---|---|
| B9 | 0..64 | `endcap_a 64x64` | Narrow column endcap (W=20, D=20, H=50) |
| B10 | 64..128 | `endcap_b 64x64` | Narrow column endcap (mirror) |
| B11 | 128..192 | `low_wall_str 64x64` | Low wall straight (W=56, D=12, H=30) |
| B12 | 192..256 | `low_wall_corner 64x64` | Low wall L-shape (small) |
| B13 | 256..320 | `low_wall_end 64x64` | Low wall endcap |
| B14 | 320..384 | `foundation_a 64x64` | Iso platform/foundation block (W=56, D=56, H=12) |
| B15 | 384..448 | `foundation_b 64x64` | Iso platform/foundation block variant |
| B16 | 448..512 | `floor_edge 64x64` | Floor edge transition (small ramp/step iso) |

### Section C Row 1 (32×32 slots, Y=256..288)

RIFT OVERLAYS — 2D pattern outlines (lightning bolt / crack styled):

| Slot | X range | Label | Content |
|---|---|---|---|
| C1 | 0..32 | `rift_crack_h` | Horizontal jagged line pattern (lightning-bolt across width) |
| C2 | 32..64 | `rift_crack_v` | Vertical jagged line pattern |
| C3 | 64..96 | `rift_burst_s` | Small star-burst 8-ray pattern |
| C4 | 96..128 | `rift_burst_l` | Large star-burst 12-ray |
| C5 | 128..160 | `rift_scar_a` | Diagonal scar/scratch lines (3 parallel) |
| C6 | 160..192 | `rift_scar_b` | Crosshatch scar pattern |
| C7 | 192..224 | `rift_glow_a` | Ring/halo outline (circle) |
| C8 | 224..256 | `rift_glow_b` | Glow halo (oval) |
| C9 | 256..288 | `rift_drop_a` | Teardrop/dripping rift shape |
| C10 | 288..320 | `rift_drop_b` | Multiple teardrops |
| C11 | 320..352 | `rift_spiral` | Spiral/swirl |
| C12 | 352..384 | `rift_zigzag` | Zigzag lightning pattern |
| C13 | 384..416 | `rift_pulse_a` | Concentric ring pulse |
| C14 | 416..448 | `rift_pulse_b` | Asymmetric pulse |
| C15 | 448..480 | `rift_burst_h` | Horizontal burst lines |
| C16 | 480..512 | `rift_burst_v` | Vertical burst lines |

### Section C Row 2 (32×32 slots, Y=288..320)

DECORATION — Simple silhouettes:

| Slot | X range | Label | Content |
|---|---|---|---|
| D1 | 0..32 | `moss_a` | Organic blob cluster (3-4 round shapes) |
| D2 | 32..64 | `moss_b` | Vertical moss strand outline |
| D3 | 64..96 | `moss_c` | Spreading moss patch outline |
| D4 | 96..128 | `moss_d` | Hanging moss strand |
| D5 | 128..160 | `candle_a` | Small candle silhouette (cylinder + flame top) |
| D6 | 160..192 | `candle_b` | Wall-mounted candle sconce silhouette |
| D7 | 192..224 | `torch_unlit` | Torch silhouette (handle + head, no flame) |
| D8 | 224..256 | `torch_lit` | Torch silhouette with flame top |
| D9 | 256..288 | `banner_a` | Hanging banner rectangle (with bottom point) |
| D10 | 288..320 | `banner_b` | Torn banner irregular bottom |
| D11 | 320..352 | `chain_short` | Short hanging chain (links) |
| D12 | 352..384 | `chain_long` | Long hanging chain |
| D13 | 384..416 | `scatter_stone` | Small stone scatter cluster |
| D14 | 416..448 | `dust_pile` | Dust pile silhouette (low oval mound) |
| D15 | 448..480 | `skull_floor` | Skull silhouette |
| D16 | 480..512 | `gem_pickup` | Gem/crystal silhouette (diamond shape) |

## Drawing Details

**3D iso box (for walls):**
- 30° dimetric projection
- 3 visible faces (front + right + top) as light gray outlines
- Hidden edges: dashed thin lines (light)
- No filled regions, just outlines

**L-shape and T-shape:**
- 2-box union outlines for L
- 3-box T pattern
- Iso projection

**Overlay shapes (Section C Row 1):**
- 2D pattern outlines (not 3D)
- Light gray line drawing
- Examples: jagged lines, star bursts, concentric rings
- Each pattern visually distinct

**Decoration silhouettes (Section C Row 2):**
- 2D iconic shapes
- Light gray fill (#808080) or outline
- Recognizable at small size (32×32)
- Examples: candle = small cylinder + triangle flame, banner = rectangle with bottom triangle cutout

## Python Skeleton

```python
from PIL import Image, ImageDraw, ImageFont
import math
import os

SQRT3_2 = math.sqrt(3) / 2

def project_iso(x, y, z, origin):
    sx = origin[0] + (x - y) * SQRT3_2
    sy = origin[1] + (x + y) * 0.5 - z
    return (sx, sy)

def draw_iso_box_outline(draw, x0, y0, w, d, h, color=(128,128,128,255), lw=1):
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
        x1,y1 = v[a]; x2,y2 = v[b]
        for i in range(0, 6, 2):
            t0, t1 = i/6, (i+1)/6
            draw.line([(x1+(x2-x1)*t0, y1+(y2-y1)*t0), (x1+(x2-x1)*t1, y1+(y2-y1)*t1)], fill=color, width=lw)

# Canvas
img = Image.new("RGBA", (512, 512), (0, 0, 0, 0))
draw = ImageDraw.Draw(img)
slot_border = (160, 160, 160, 200)
label_color = (180, 180, 180, 200)

# Helper to draw slot frame + label
def draw_slot(x, y, w, h, label):
    # Border
    draw.rectangle([x, y, x+w-1, y+h-1], outline=slot_border, width=1)
    # Label above (or beside if no space)
    label_y = max(y - 11, 0)
    if y < 12:  # if no room above, label inside top
        label_y = y + 1
    draw.text((x + 2, label_y), label, fill=label_color)

# Section A — 4 features (128×128)
# ... per slot draw_slot + draw_iso_box_outline with proper geometry
# (Implement per spec table above)

# Section B Row 1 (64×64, Y=128..192)
# ... 8 slots

# Section B Row 2 (64×64, Y=192..256)
# ... 8 slots

# Section C Row 1 RIFT OVERLAYS (32×32, Y=256..288)
# For each: 2D pattern (jagged lines, bursts, etc.)

# Section C Row 2 DECOR (32×32, Y=288..320)
# Simple 2D silhouettes

# Save
os.makedirs("STAGING/_pixellab_inputs/master_sheets", exist_ok=True)
img.save("STAGING/_pixellab_inputs/master_sheets/act1_wall_modular_pack_v1.png")
```

**Implement creatively** — yukarıdaki spec'i takip et ama her tile için sensible şekil tasarla. Çok detay gerekmiyor, sadece "AI'a slot içeriği için ipucu" yeterli.

## Verification

- ✅ 512×512 PNG, RGBA transparent
- ✅ 4 + 16 + 16 + 16 = 52 slot görünür
- ✅ Slot border + label her birinde
- ✅ Section A: 3D iso box outlines (varied shapes)
- ✅ Section B: smaller 3D iso boxes per slot type
- ✅ Section C Row 1: 2D pattern outlines for rift overlays (visually distinct each)
- ✅ Section C Row 2: simple silhouettes for decoration

## Effort

medium-high — geometric variety var, ama math hazır (önceki task'lerden adapt et). Section C 2D shapes için yaratıcı serbest, sadece "AI'a şekil ipucu" yeterli.

## Output Confirmation

PNG path + slot count + screenshot description.
