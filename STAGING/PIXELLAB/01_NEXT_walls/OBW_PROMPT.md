# OBW — Overbuilt Wall (Yüksek Duvar, Tavan/Kemer)

**Boyut:** 64x128px → **4 varyasyon**
**Style Reference:** W1 approved tile yükle
**Tool:** Create Tile — Isometric (veya Create Image S-XL eğer connection-aware gerekmiyorsa)

## Prompt

```
Isometric pixel art tall architectural wall section, 64x128 pixels, 2:1 isometric. Pure solid green #00FF00 background. This is a TALLER wall section — no top face visible (wall extends above frame), no base shadow (wall extends below frame). Front face only: 128px vertical stone masonry. Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260. Flat-shaded, dithered, hard pixel edges. 4 variations: plain stone, with narrow window slit (4x12px dark void), with iron wall bracket, with carved rune (simple geometric glyph, 8x8px, raised relief).
```

## Process

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/OBW --cols 2 --rows 2 --width 64 --height 128 --prefix obw_
```
