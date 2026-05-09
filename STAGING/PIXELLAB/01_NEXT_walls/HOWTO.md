# 01 — Walls (W1 / W2 / OBW)
*Source: `STAGING/PIXELLAB_PROMPT_WALLS_v3.md` (LOCKED 2026-05-09)*

## Tool

**PixelLab → Create Tile — Isometric** (Map bölümünde, dedicated tool)
- "Create Tileset Standard" değil (sadece 16x32 üretir)
- "Create Tiles Pro" floor için (Pro shape isometric — duvar değil)
- Connection-aware tile family üretir (straight + corner + T + cap)

> **64x96 PixelLab'da YOK.** Kullanılan boyut: **64x128** (taller — north/south wall için ideal)

## Settings (her wall gen call'da)

| Param | Value |
|---|---|
| Tool | Create Tile — Isometric |
| Background | #00FF00 (chromakey) |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | F1 approved floor tile yükle (palette tutarlılık) |

## Üretim Sırası

1. **W1 straight** (8 var, 64x128) → Unity'de QC
2. W1 approved → **style reference** olarak yükle → **W2** üret
3. W1 + W2 yan yana Unity karşılaştırma → palette drift kontrol
4. **OBW** son (yüksek arch wall, 4 var)

## Output

PNG'yi `outputs/w1/`, `outputs/w2/`, `outputs/obw/` altına kaydet.
Dosya adı: `<sheet_name>_v<N>.png` (örn: `w1_sheet_v1.png`).

## Process to Unity

```powershell
# W1
python STAGING/process_tiles.py --source STAGING/PIXELLAB/01_NEXT_walls/outputs/w1/w1_sheet_v1.png --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 2 --width 64 --height 128 --prefix w1_

# W2
python STAGING/process_tiles.py --source STAGING/PIXELLAB/01_NEXT_walls/outputs/w2/w2_sheet_v1.png --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 128 --prefix w2_

# OBW
python STAGING/process_tiles.py --source STAGING/PIXELLAB/01_NEXT_walls/outputs/obw/obw_sheet_v1.png --output Assets/Art/Tiles/Act1/OBW --cols 2 --rows 2 --width 64 --height 128 --prefix obw_
```

## Unity Import

- Sprite PPU: **64**
- Pivot: **bottom-center (0.5, 0.0)**
- Sorting layer: **Walls**, Y-sort aktif
- WallOcclusionFader: fadeRadius 2.2, minAlpha 0.38 (Wall Tilemap'e Add Component)

## QC Checklist

- [ ] Background tam #00FF00 mi (process script gri/kahverengi pixel'i tile içine sızdırmamalı)
- [ ] 8 connection variant net mi (straight ×2, corner ×2, T-junction, end-cap ×2)
- [ ] Palette tablosuna birebir uygun mu (#1A1C20 / #2A2D34 / #3A3D48 / #4E5260 / #606575)
- [ ] Pixel cluster min 4px mi (smooth gradient YASAK)
- [ ] Hard pixel edge (anti-alias yok)
- [ ] One weathering accent / tile max
