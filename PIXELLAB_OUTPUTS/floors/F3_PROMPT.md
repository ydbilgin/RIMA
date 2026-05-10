# F3 — Volkanik Karanlık Taş (Boss Bölgesi)

**Boyut:** 64x64px → **16 varyasyon**
**Style Reference:** F2 tile en güzel varyantını yükle
**Tool:** Create Tiles Pro (Isometric, High top-down)

## Prompt

```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. Same 2:1 isometric diamond. Dark volcanic basalt floor, heavy fracture lines, glowing energy crack details. Palette STRICTLY 5 colors: #14141A (near-black volcanic), #222230 (dark basalt), #323240 (mid basalt), #424255 (lifted face), #4A1A1A (deep energy glow — dark crimson, max 8 pixels per tile for crack glow). Flat baked lighting. Pixel clusters minimum 4px. Hard pixel edges. Each variation has one prominent lava-crack vein running diagonally, glowing #4A1A1A at center darkening to stone. Some variations add crumbled edge. Seamless tessellation. Atmosphere: oppressive, corrupted, near the boss.
```

## Process

```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_
```
