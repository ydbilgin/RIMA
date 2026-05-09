# 02 — Floors (F1 / F2 / F3 + Transitions)
*Source: `STAGING/PIXELLAB_PROMPT_FLOORS_v3.md` (LOCKED 2026-05-09)*

## Tool

**PixelLab → Create Tiles Pro** (Map bölümünde — Standard değil, **Pro**)
- Shape: **Isometric** seç (dropdown: Square / Hex / Isometric / Octagon)
- View: **High top-down** (zemin için tepeden bakış)
- Seamless edge handling otomatik

## Settings (her floor gen call'da)

| Param | Value |
|---|---|
| Tool | Create Tiles Pro |
| Tile Type | Isometric |
| Background | #00FF00 (chromakey green) |
| Pixel Art Mode | ON |
| Upscale | OFF |
| Anti-aliasing | OFF |
| Style Reference | önceki approved tile yükle (zorunlu) |

**Chromakey:** #00FF00 — `process_tiles.py` filter `G>200 AND R<60 AND B<60`.

## Üretim Sırası

1. **F1** (16 var, 64x64) — entry rooms, soğuk gri granit
2. F1 approved → **style ref** → **F2** üret
3. F2 approved → **style ref** → **F3** üret
4. F1 + F2 → **Trans_F1F2** (8 var)
5. F2 + F3 → **Trans_F2F3** (8 var)

## Output

PNG'leri `outputs/f1/`, `outputs/f2/`, `outputs/f3/`, `outputs/trans/` altına koy.

## Process to Unity

```powershell
# F1
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f1/f1_sheet_v1.png --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_

# F2
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f2/f2_sheet_v1.png --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_

# F3
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/f3/f3_sheet_v1.png --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_

# Trans_F1F2 (8 var, 2 cols × 4 rows)
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f1f2_v1.png --output Assets/Art/Tiles/Act1/Trans_F1F2 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f1f2_

# Trans_F2F3
python STAGING/process_tiles.py --source STAGING/PIXELLAB/02_NEXT_floors/outputs/trans/trans_f2f3_v1.png --output Assets/Art/Tiles/Act1/Trans_F2F3 --cols 2 --rows 4 --width 64 --height 64 --prefix trans_f2f3_
```

## Unity Import

- PPU: **64**
- Pivot: **Center (0.5, 0.5)**
- Sorting layer: **Ground**
- Filter: Point, Compression: None

## Tutarlılık Kuralları

1. Her session başında önceki onaylı tile'ı **style reference** olarak yükle
2. Palette hex kodlarını prompt'ta AYNEN kullan ("dark grey" gibi belirsiz terim YASAK)
3. Önce 4 varyasyon üret → QC → beğendiysen 16'ya çıkar
4. F1/F2/F3 farklı session'larda aynı style reference (F1 approved tile)
5. Process sonrası Unity'de F1+F2+F3 yan yana — hue drift kontrol → drift varsa yeniden üret

## QC Checklist

- [ ] 2:1 isometric diamond rhombus (perfect, viewed from top)
- [ ] Background tam #00FF00
- [ ] Palette hex'leri tam uyuyor (5 renk lock)
- [ ] Pixel cluster min 4px
- [ ] Hard pixel edge, anti-alias yok
- [ ] Seamless tessellation — pattern flush to diamond edge
- [ ] One subtle accent per tile (F1: hairline crack/moss/chip/water mark)
- [ ] F2: wider mortar fracture + lichen accent
- [ ] F3: lava-crack vein + crimson glow (#4A1A1A max 8px)
