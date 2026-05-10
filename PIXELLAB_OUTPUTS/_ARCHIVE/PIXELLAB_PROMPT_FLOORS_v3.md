# PixelLab Floor Tile Prompts — v3
# F1 / F2 / F3 | Act 1: Shattered Keep
# Tarih: 2026-05-09 | Opus onaylı

---

## TOOL SEÇİMİ

**Tool: Create Tiles Pro** (Map bölümünde — Standard değil, Pro)
- Shape: **Isometric** seç (dropdown: Square / Hex / Isometric / Octagon)
- View: High top-down (zemin için tepeden bakış)
- Seamless edge handling otomatik
- Size: 64px → 16 variations, 128px → 4 variations
- Style reference: önceki approved tile'ı yükle (tutarlılık için zorunlu)
- Transparent background: docs'ta belirtilmemiş — test et; yoksa #00FF00 chromakey kullan

---

## ORTAK AYARLAR (tüm floor tile'lar için)

| Parametre | Değer |
|---|---|
| Tool | Create Tiles Pro |
| Tile Type | Isometric |
| Pixel Art Mode | ON |
| Background | #00FF00 (chromakey green) |
| Upscale | OFF |
| Anti-aliasing | OFF |

**Chromakey:** #00FF00 — process_tiles.py filter: `G>200 AND R<60 AND B<60`

---

## F1 FLOOR — Soğuk Gri Taş (Entry/İlk Odalar)

**Boyut:** 64x64px → **16 varyasyon**

**Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background fills every pixel outside the tile diamond. The tile is a perfect 2:1 isometric diamond rhombus viewed from top. Cold grey granite stone surface, staggered rectangular brick pattern with visible mortar lines. Palette STRICTLY 4 colors only: #1E2028 (darkest crack/mortar), #2E3038 (dark stone face), #424555 (mid stone), #555868 (lightest face, top-lit). Flat baked dungeon lighting, NO gradient, NO smooth shading, NO ambient occlusion render. Pixel clusters minimum 4px wide. Hard pixel edges, no anti-aliasing on tile boundary. Each of 16 variations has ONE subtle accent only: hairline crack, faint moss stain (1-3 pixels #2D4030 max), corner chip, or water mark — never two accents on same tile. Seamless — pattern continues flush to diamond edge so adjacent tiles tessellate without gap.
```

**process_tiles.py:**
```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/F1 --cols 4 --rows 4 --width 64 --height 64 --prefix f1_
```

---

## F2 FLOOR — Çatlak Taş, Biyolüminesan İz (Orta Derinlik)

**Boyut:** 64x64px → **16 varyasyon**
**Style Reference:** F1 tile en güzel varyantını yükle → palette lock için

**Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. Same 2:1 isometric diamond as F1 floor. Cracked stone surface, wider mortar fractures than F1. Palette STRICTLY 5 colors: #1A1C22 (deep crack), #2A2C35 (dark stone), #3C3F4E (mid stone), #4E5260 (lit face), #263530 (bioluminescent lichen accent — use sparingly, max 6 pixels per tile). Flat baked lighting only, NO gradient. Pixel clusters minimum 4px. Hard pixel edges. Each of 16 variations has one main crack feature (longer hairline crossing 20+ pixels) PLUS one optional lichen dot cluster. Seamless tessellation. The overall feel is darker and more stressed than F1.
```

**process_tiles.py:**
```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/F2 --cols 4 --rows 4 --width 64 --height 64 --prefix f2_
```

---

## F3 FLOOR — Volkanik Karanlık Taş (Boss Bölgesi)

**Boyut:** 64x64px → **16 varyasyon**
**Style Reference:** F2 tile en güzel varyantını yükle

**Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. Same 2:1 isometric diamond. Dark volcanic basalt floor, heavy fracture lines, glowing energy crack details. Palette STRICTLY 5 colors: #14141A (near-black volcanic), #222230 (dark basalt), #323240 (mid basalt), #424255 (lifted face), #4A1A1A (deep energy glow — dark crimson, max 8 pixels per tile for crack glow). Flat baked lighting. Pixel clusters minimum 4px. Hard pixel edges. Each variation has one prominent lava-crack vein running diagonally, glowing #4A1A1A at center darkening to stone. Some variations add crumbled edge. Seamless tessellation. Atmosphere: oppressive, corrupted, near the boss.
```

**process_tiles.py:**
```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/F3 --cols 4 --rows 4 --width 64 --height 64 --prefix f3_
```

---

## TRANSITION FLOORS (F1→F2, F2→F3)

**Boyut:** 64x64px → **8 varyasyon** (2 cols × 4 rows sheet)

**F1→F2 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. 2:1 isometric diamond. Transition stone: left half of tile is clean F1 grey granite (#2E3038, #424555), right half transitions into cracked F2 style (#2A2C35, #3C3F4E, #263530 lichen). The split is diagonal not vertical, creating a natural rock fault line. Flat baked lighting only. Hard pixel edges. 8 variations, each with slightly different fault line angle and lichen spread.
```

**F2→F3 Prompt:**
```
Isometric pixel art floor tile, 64x64 pixels. Pure solid green #00FF00 background. 2:1 isometric diamond. Transition: F2 cracked stone (#3C3F4E) bleeding into F3 volcanic basalt (#222230, #4A1A1A energy). Diagonal fault line with glowing crack appears at transition. 8 variations, varied crack thickness and glow intensity.
```

---

## TUTARLILIK KURALLARI

1. Her session başında önceki onaylı tile'ı **style reference** olarak yükle
2. Palette hex kodlarını prompt'ta AYNEN kullan — "dark grey" gibi belirsiz terimler kullanma
3. Önce 4 varyasyon üret → QC → beğendiysen 16'ya çıkar
4. Farklı F1/F2/F3 session'larında aynı style reference tile'ı kullan (F1 veya F2 approved)
5. `process_tiles.py` sonrası Unity'de yan yana kontrol et — hue kayması olursa yeniden üret
