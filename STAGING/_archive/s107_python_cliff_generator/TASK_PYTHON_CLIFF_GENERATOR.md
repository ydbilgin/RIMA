# TASK: Python Cliff Geometric Generator (Tool)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Cliff sprite üretiminde **perspective doğruluğu + style consistency** için Python geometric base generator. Output: 10 farklı seed ile 10 base PNG. Kullanıcı bunları PixelLab S-XL New'e **init image** olarak verecek (AI Freedom 0.3 ile texture/lighting eklenir). PixelLab'ın "wall/brick pattern" yorumu engellenir, silhouette geometric olarak garanti.

## Output Spec

**Dosya:** `tools/cliff_generator.py`
**Output klasör:** `STAGING/cliff_bases/cliff_v01.png ... cliff_v10.png` (10 variant)
**Format:** PNG RGBA, transparent background
**Boyut:** 128 × 96 px

## Geometri Kuralları (high top-down 75°)

```
Y axis (pixel coord, top=0)
0 ────────── jagged top edge (subtle, ±2-3 px variation)
              ↑ top edge ~10-15% of height (visible flat top)
top_y=12 ────────────────────────────────
              ↑ vertical drop face (rest of canvas)
              dark grey gradient (top lighter, bottom darker)
              optional subtle vertical cracks (random thin lines)
96 ──────── bottom (cliff base)
```

### Renkler (Hades Elysium palette uyumlu)
- Top edge highlight: `#6b6b6b` (lighter grey)
- Top edge shadow: `#3a3a3a` (top edge bottom line)
- Face main: `#4a4a4a` (mid grey)
- Face shadow: `#2a2a2a` (bottom shadow)
- Crack hint: `#1a1a1a` (very dark, thin lines)

### Jagged Top Algorithm
```python
# Top edge: 8-12 control points across 128px width
# Each point Y = top_y + random.randint(-2, 3)  (max ±3 px)
# Connect with straight lines (not bezier — pixel art)
# Random.seed(seed) for determinism per variant
```

### Variant Strategy
- 10 farklı seed (1-10)
- Her seed farklı jagged pattern
- Aynı palette + aynı silhouette logic
- PixelLab style ref olunca tutarlılık garanti

## Python Implementation

**Dependencies:** Pillow (PIL) sadece — proje zaten kullanıyor olabilir, check requirements
**Stdlib:** random, os, argparse

```python
#!/usr/bin/env python3
"""
RIMA Cliff Geometric Base Generator
Produces 128x96 RGBA cliff silhouettes for PixelLab S-XL New init image.

Usage:
    python tools/cliff_generator.py                  # 10 variants default
    python tools/cliff_generator.py --count 20       # custom count
    python tools/cliff_generator.py --seed 42        # single seed
"""
import argparse
import os
import random
from PIL import Image, ImageDraw

OUTPUT_DIR = "STAGING/cliff_bases"
WIDTH, HEIGHT = 128, 96
TOP_Y_BASE = 12       # base top edge Y position
TOP_Y_VARIATION = 3   # ±3 px jagged
CONTROL_POINT_COUNT = 10   # top edge segments

# Palette
COLOR_TOP_LIGHT = (107, 107, 107, 255)
COLOR_TOP_SHADOW = (58, 58, 58, 255)
COLOR_FACE_MAIN = (74, 74, 74, 255)
COLOR_FACE_SHADOW = (42, 42, 42, 255)
COLOR_CRACK = (26, 26, 26, 200)


def generate_cliff(seed: int, output_path: str):
    rng = random.Random(seed)
    img = Image.new('RGBA', (WIDTH, HEIGHT), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)

    # Jagged top edge points
    top_points = []
    step = WIDTH / (CONTROL_POINT_COUNT - 1)
    for i in range(CONTROL_POINT_COUNT):
        x = int(i * step)
        y_offset = rng.randint(-TOP_Y_VARIATION, TOP_Y_VARIATION)
        top_points.append((x, TOP_Y_BASE + y_offset))

    # Bottom edge (straight)
    bottom_points = [(WIDTH - 1, HEIGHT - 1), (0, HEIGHT - 1)]

    # Polygon — main face
    polygon = top_points + bottom_points
    d.polygon(polygon, fill=COLOR_FACE_MAIN, outline=COLOR_TOP_SHADOW)

    # Top edge highlight (1 px above top edge)
    for i in range(len(top_points) - 1):
        x1, y1 = top_points[i]
        x2, y2 = top_points[i+1]
        d.line([(x1, y1-1), (x2, y2-1)], fill=COLOR_TOP_LIGHT, width=1)

    # Vertical shading — gradient effect via stripes
    for x in range(0, WIDTH, 4):
        # Random small variation for texture
        if rng.random() < 0.3:
            shade_y_start = TOP_Y_BASE + 4
            shade_y_end = HEIGHT - rng.randint(0, 15)
            d.line([(x, shade_y_start), (x, shade_y_end)],
                   fill=COLOR_FACE_SHADOW, width=1)

    # Subtle vertical cracks (1-3 per cliff)
    crack_count = rng.randint(1, 3)
    for _ in range(crack_count):
        crack_x = rng.randint(10, WIDTH - 10)
        crack_y_start = TOP_Y_BASE + rng.randint(2, 10)
        crack_y_end = HEIGHT - rng.randint(5, 20)
        # Slight wave (3 segments)
        for seg in range(3):
            seg_y1 = crack_y_start + int((crack_y_end - crack_y_start) * seg / 3)
            seg_y2 = crack_y_start + int((crack_y_end - crack_y_start) * (seg+1) / 3)
            seg_x_offset = rng.randint(-1, 1)
            d.line([(crack_x, seg_y1), (crack_x + seg_x_offset, seg_y2)],
                   fill=COLOR_CRACK, width=1)

    img.save(output_path)
    return output_path


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--count', type=int, default=10, help='Variant count')
    parser.add_argument('--seed', type=int, default=None, help='Single seed (overrides count)')
    args = parser.parse_args()

    os.makedirs(OUTPUT_DIR, exist_ok=True)

    if args.seed is not None:
        path = os.path.join(OUTPUT_DIR, f'cliff_v{args.seed:02d}.png')
        generate_cliff(args.seed, path)
        print(f'Generated: {path}')
    else:
        for i in range(1, args.count + 1):
            path = os.path.join(OUTPUT_DIR, f'cliff_v{i:02d}.png')
            generate_cliff(i, path)
            print(f'Generated: {path}')

    print(f'\nDone — {args.count if args.seed is None else 1} cliff base(s) at {OUTPUT_DIR}')


if __name__ == '__main__':
    main()
```

## Yapılacaklar

### Adım 1: Script oluştur
- `tools/cliff_generator.py` yazılır (yukarıdaki kod tam aldığın gibi, küçük tweak istersen yap ama spec'ten sapma)
- Pillow dependency check (eğer yoksa `pip install Pillow` instruction comment ekle)

### Adım 2: Test çalıştır
- `python tools/cliff_generator.py` çalıştır
- `STAGING/cliff_bases/cliff_v01.png ... cliff_v10.png` oluştuğunu doğrula
- 1-2 sample PNG'yi check et (file size > 1KB, RGBA mode)

### Adım 3: README ekle
- `tools/cliff_generator_README.md` (kısa, ~30 satır)
- PixelLab S-XL New workflow nasıl:
  1. STAGING/cliff_bases/ klasöründen seç
  2. PixelLab Web UI → S-XL New → Init Image upload
  3. AI Freedom: 0.3-0.4
  4. Prompt: `dark weathered stone texture, subtle moss, natural rock formation, sharp pixel art, no bricks`
  5. Generate → asset import to Assets/Sprites/Environment/KitB_Cliff/v2/

## Hard Constraints
- Pure Python + Pillow, başka dep YOK
- Surgical — sadece 2 dosya (script + README)
- Output PNG transparent BG, 128×96 RGBA
- BLOCKED: Pillow yoksa raporla, alternatif yaklaşım önerme

## Inline rapor (<300 kelime)
- Script path
- Test çalıştırma sonucu (10 PNG üretildi mi)
- Sample dosya boyutları
- README path
- BLOCKED varsa neden
