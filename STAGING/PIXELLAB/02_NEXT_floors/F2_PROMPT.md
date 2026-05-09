# F2 — Çatlak Taş, Biyolüminesan İz (Orta Derinlik)

**Boyut:** 64x64px → **16 varyasyon**
**Style Reference:** F1 tile en güzel varyantını yükle → palette lock için
**Tool:** Create Tiles Pro (Isometric, High top-down)

## Prompt

```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. Same 2:1 isometric diamond as F1 floor. Cracked stone surface, wider mortar fractures than F1. Palette STRICTLY 5 colors: #1A1C22 (deep crack), #2A2C35 (dark stone), #3C3F4E (mid stone), #4E5260 (lit face), #263530 (bioluminescent lichen accent — use sparingly, max 6 pixels per tile). Flat baked lighting only, NO gradient. Pixel clusters minimum 4px. Hard pixel edges. Each of 16 variations has one main crack feature (longer hairline crossing 20+ pixels) PLUS one optional lichen dot cluster. Seamless tessellation. The overall feel is darker and more stressed than F1.
```

## Process

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_
```
