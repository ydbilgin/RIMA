# TASK: Python Cliff Generator REVIZE — Tile-Cell-Aligned + 3D Dimetric Mock

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Mevcut `tools/cliff_generator.py` REVIZE. Yeni spec — cliff top edge piksel boyutu **TAM 1 tile cell width = 64 px** (floor sprite 64×32 dimetric ile uyumlu). 3D dimetric-ish mock (top deck + drop face), tek gri renk, transparent BG. Sonra PixelLab Web UI'a init image olarak verilecek.

## Spec

| Parametre | Değer | Sebep |
|---|---|---|
| Canvas | **64 × 96** | Width = 1 tile cell pixel width (64), Height = 1.5 cell (top 16 + drop 80) |
| Top deck height | 16 px | Tile cell altında oturur, dimetric perspective |
| Drop face height | 80 px (96 - 16) | Void'e sarkan dikey yüz |
| Renk | Tek tonlu gri ailesi | PixelLab AI texture ekleyecek |
| Top deck color | `#6e6e6e` (110,110,110) | Catch light üst |
| Face color | `#464646` (70,70,70) | Drop face (orta-koyu) |
| Outline | `#282828` (40,40,40) | Sharp dark edge |
| Right side shadow | `#323232` (50,50,50) | 2px strip sağ kenar |
| Background | Transparent | RGBA alpha=0 |
| Variant count | 10 (seed 1-10) | Jagged top variation |

## 3D Dimetric Mock Yapısı

```
y=0   ─────────────────────────────  (transparent above top edge)
y=4   ┌──── jagged top edge ────┐    ← Top edge points (±2 px variation)
      │       TOP DECK          │    ← lighter color #6e6e6e
y=16  ├─────────────────────────┤    ← Top deck bottom (face starts)
      │                         │
      │      DROP FACE          │    ← darker #464646
      │  (subtle 1-2 cracks)    │
      │                       ║│    ← Right side shadow strip
y=95  └─────────────────────────┘    ← Bottom edge (sharp outline)
      0px                    63px
```

Önemli:
- Top edge x-range: 0 → 63 (full width)
- Top edge y: 4 + random(-2, +2) at each control point
- Control points every 8 px → 9 points
- Polygon main fill: jagged top + face + bottom rectangle
- Top deck overlay: same jagged top + horizontal line y=16 = lighter color block
- Side shadow: x=62, x=63 from y=16 to y=95 = darker grey

## Implementation

`tools/cliff_generator.py` mevcut dosyayı OVERWRITE et:

```python
#!/usr/bin/env python3
"""
RIMA Cliff Geometric Base Generator (REVIZE 2026-05-26)
Tile-cell-aligned: top edge = 1 floor cell width (64 px).
Output: 64x96 RGBA dimetric-ish cliff mock for PixelLab S-XL New init image.

Usage:
    python tools/cliff_generator.py
    python tools/cliff_generator.py --count 20
    python tools/cliff_generator.py --seed 42
"""
import argparse
import os
import random
from PIL import Image, ImageDraw

OUTPUT_DIR = "STAGING/cliff_bases"
WIDTH, HEIGHT = 64, 96
TOP_EDGE_Y = 4
TOP_EDGE_VARIATION = 2
TOP_DECK_BOTTOM = 16     # face starts here
CONTROL_POINTS = 9       # every 8 px across 64 width

# Palette (single-tone grey family for AI to texture later)
COLOR_TOP_DECK = (110, 110, 110, 255)
COLOR_FACE = (70, 70, 70, 255)
COLOR_OUTLINE = (40, 40, 40, 255)
COLOR_SIDE_SHADOW = (50, 50, 50, 255)
COLOR_CRACK = (30, 30, 30, 200)


def generate_cliff(seed: int, output_path: str):
    rng = random.Random(seed)
    img = Image.new('RGBA', (WIDTH, HEIGHT), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)

    # Jagged top edge points
    top_points = []
    for i in range(CONTROL_POINTS):
        x = int(i * (WIDTH - 1) / (CONTROL_POINTS - 1))
        y_offset = rng.randint(-TOP_EDGE_VARIATION, TOP_EDGE_VARIATION)
        top_points.append((x, TOP_EDGE_Y + y_offset))

    # Main polygon (jagged top + bottom rectangle)
    main_polygon = top_points + [(WIDTH - 1, HEIGHT - 1), (0, HEIGHT - 1)]
    d.polygon(main_polygon, fill=COLOR_FACE, outline=COLOR_OUTLINE)

    # Top deck overlay (lighter color from top edge to TOP_DECK_BOTTOM)
    top_deck_polygon = top_points + [(WIDTH - 1, TOP_DECK_BOTTOM), (0, TOP_DECK_BOTTOM)]
    d.polygon(top_deck_polygon, fill=COLOR_TOP_DECK)

    # Right side shadow strip (2 px wide, face only)
    for y in range(TOP_DECK_BOTTOM, HEIGHT):
        d.line([(WIDTH - 2, y), (WIDTH - 1, y)], fill=COLOR_SIDE_SHADOW, width=1)

    # Subtle vertical cracks (1-2 per cliff, face area only)
    crack_count = rng.randint(1, 2)
    for _ in range(crack_count):
        crack_x = rng.randint(8, WIDTH - 10)
        crack_y_start = TOP_DECK_BOTTOM + rng.randint(2, 6)
        crack_y_end = HEIGHT - rng.randint(5, 15)
        # 2-segment wave
        mid_y = (crack_y_start + crack_y_end) // 2
        mid_x_offset = rng.randint(-1, 1)
        d.line([(crack_x, crack_y_start), (crack_x + mid_x_offset, mid_y)], fill=COLOR_CRACK, width=1)
        d.line([(crack_x + mid_x_offset, mid_y), (crack_x, crack_y_end)], fill=COLOR_CRACK, width=1)

    img.save(output_path)
    return output_path


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--count', type=int, default=10)
    parser.add_argument('--seed', type=int, default=None)
    parser.add_argument('--out', type=str, default=OUTPUT_DIR)
    args = parser.parse_args()

    os.makedirs(args.out, exist_ok=True)

    if args.seed is not None:
        path = os.path.join(args.out, f'cliff_v{args.seed:02d}.png')
        generate_cliff(args.seed, path)
        print(f'Generated: {path}')
    else:
        for i in range(1, args.count + 1):
            path = os.path.join(args.out, f'cliff_v{i:02d}.png')
            generate_cliff(i, path)
            print(f'Generated: {path}')

    print(f'\nDone — {args.count if args.seed is None else 1} cliff base(s) at {args.out}')


if __name__ == '__main__':
    main()
```

## README revize

`tools/cliff_generator_README.md` güncelle:
- Yeni boyut spec (64×96, top edge = 1 tile cell width = 64 px)
- 3D dimetric mock açıklama
- PixelLab Web UI workflow:
  1. STAGING/cliff_bases/ klasöründen bir base seç (örn cliff_v05.png)
  2. PixelLab Web UI → S-XL New → Init Image upload
  3. AI Freedom: **0.3-0.4** (silhouette koru, texture ekle)
  4. Canvas: **64×96** (init image boyutu)
  5. Prompt: `pixel art version, dark grey weathered rock texture, subtle moss patches, natural cliff face, sharp pixel edges, no anti-aliasing`
  6. Generate → kontrol → asset import (`Assets/Sprites/Environment/KitB_Cliff/v2/`)

## Yapılacaklar

1. Eski `tools/cliff_generator.py` overwrite
2. README revize
3. Eski `STAGING/cliff_bases/cliff_v01.png ... v10.png` SİL (eski 128×96 format yanlış)
4. Yeni script test çalıştır
5. 10 yeni PNG doğrula (64×96 RGBA)

## Hard Constraints
- Pure Python + Pillow
- Surgical — sadece script + README + eski PNG cleanup
- Yeni PNG: 64×96 RGBA, transparent BG
- BLOCKED: Pillow yoksa raporla

## Inline rapor (<300 kelime)
- Script overwrite OK
- README güncellendi
- Eski PNG silindi (10 dosya)
- Yeni PNG üretildi (10 dosya, 64×96 RGBA)
- Sample dosya boyutları
- BLOCKED varsa neden
