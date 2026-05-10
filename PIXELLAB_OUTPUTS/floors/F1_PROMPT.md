# F1 — Soğuk Gri Taş (Entry/İlk Odalar)

**Boyut:** 64x64px → **16 varyasyon**
**Tool:** Create Tiles Pro (Isometric, High top-down)

## Prompt

```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background fills every pixel outside the tile diamond. The tile is a perfect 2:1 isometric diamond rhombus viewed from top. Cold grey granite stone surface, staggered rectangular brick pattern with visible mortar lines. Palette STRICTLY 4 colors only: #1E2028 (darkest crack/mortar), #2E3038 (dark stone face), #424555 (mid stone), #555868 (lightest face, top-lit). Flat baked dungeon lighting, NO gradient, NO smooth shading, NO ambient occlusion render. Pixel clusters minimum 4px wide. Hard pixel edges, no anti-aliasing on tile boundary. Each of 16 variations has ONE subtle accent only: hairline crack, faint moss stain (1-3 pixels #2D4030 max), corner chip, or water mark — never two accents on same tile. Seamless — pattern continues flush to diamond edge so adjacent tiles tessellate without gap.
```

## Process

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_
```
