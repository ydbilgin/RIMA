# W1 — Koyu Granit Taş Tuğla (Ana Duvar, Act 1)

**Boyut:** 64x128px → **8 varyasyon** (straight ×2, corner ×2, T-junction ×2, end-cap ×2)
**Style Reference:** F1 floor approved tile yükle
**Tool:** Create Tile — Isometric

## Prompt

```
Isometric pixel art stone wall tile, 64x128 pixels, 2:1 isometric projection. Pure solid green #00FF00 background fills all pixels outside the wall shape.

The tile has three vertical zones:
TOP FACE (top 12px): wall surface viewed from isometric top — slightly lighter, shows stone top, ambient light catch.
FRONT FACE (middle 104px): vertical stone brick masonry, staggered courses, each brick 12-16px wide and 7-9px tall. Main surface.
BASE SHADOW (bottom 12px): ambient occlusion strip where wall meets floor, gradient from #1A1C20 to transparent.

Palette STRICTLY: #1A1C20 (mortar/shadow), #2A2D34 (dark stone face), #3A3D48 (mid stone), #4E5260 (lit face), #606575 (top face highlight). NO other colors.

Flat-shaded pixel art, NO smooth gradient, NO ambient occlusion render, NO 3D software look. Hand-pixeled appearance. Dithered shading only (2x2 checker). Hard pixel edges. No anti-aliasing on tile boundary. Pixel clusters minimum 4px.

Generate 8 connection variants: (1) straight north-facing, (2) straight south-facing, (3) outer corner NE, (4) outer corner NW, (5) inner corner, (6) T-junction, (7) end-cap north, (8) end-cap south. Each shares identical brick pattern and palette — only silhouette and corner geometry differ.

One weathering accent per tile max: hairline crack, chipped brick corner, or iron ring anchor.
```

## Process

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 2 --width 64 --height 128 --prefix w1_
```
