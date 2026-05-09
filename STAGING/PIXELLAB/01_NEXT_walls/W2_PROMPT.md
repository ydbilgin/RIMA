# W2 — Daha Derin / Daha Bozulmuş Duvar (Orta Derinlik)

**Boyut:** 64x128px → **8 varyasyon**
**Style Reference:** W1 approved tile yükle (ortak palette zorunlu)
**Tool:** Create Tile — Isometric

## Prompt

```
Isometric pixel art stone wall tile, 64x128 pixels, 2:1 isometric projection. Pure solid green #00FF00 background.

Same three-zone structure as W1: top face 12px, front face 104px, base shadow 12px.

Palette STRICTLY: #18181E (deep mortar), #26293A (very dark stone), #363A4A (dark mid stone), #464B5E (mid stone), #565C70 (lit face). Slightly cooler/bluer than W1 — deeper dungeon feeling.

Same flat-shaded pixel art rules: NO gradient, NO smooth shading, dithered only, hard pixel edges, pixel clusters min 4px.

Brick pattern shows MORE damage than W1: wider mortar cracks, missing brick corners, occasional horizontal fracture line crossing 2-3 bricks. One variation has a faint bioluminescent lichen vein (#1E3028, max 8px) along a crack.

Same 8 connection variants as W1: straight north, straight south, outer corner NE, outer corner NW, inner corner, T-junction, end-cap north, end-cap south.
```

## Process

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 128 --prefix w2_
```
