# PixelLab Wall Tile Prompts — v3
# W1 / W2 | Act 1: Shattered Keep
# Tarih: 2026-05-09 | Opus onaylı | 64x96 → 64x128 düzeltmesi

---

## TOOL SEÇİMİ

**Tool: Create Tile — Isometric** (Map bölümünde, dedicated tool)
- "Create Tileset Standard" değil — o sadece 16x32px üretir, işe yaramaz
- "Create Tiles Pro" ile karıştırma — bu ayrı, daha spesifik tool
- Connection-aware tile family üretir (straight, corner, T-junction, end-cap)
- Isometric perspektif built-in, 4 ve 8 yönlü view desteği
- Style reference: F1 approved floor tile'ı yükle → ortak palette için
- Transparent background: test et (belgelenmemiş ama muhtemelen var)

> **UYARI:** 64x96 PixelLab'da LİSTEDE YOK.
> Kullanılacak boyut: **64x128** (taller — north/south wall için uygun)
> Alternatif: **48x96** (daha dar, daha az yüksek)

---

## BOYUT / VARİYASYON TABLOSU

| Boyut | Varyasyon | Kullanım |
|---|---|---|
| 64x128 | 8 variation | W1/W2 connection set (straight+corner+T+cap) |
| 128x128 | 4 variation | Büyük/dekoratif wall segment |
| 64x64 | 16 variation | Düz duvar tekrar tile'ı (arka yüz dolgu) |

---

## W1 WALL — Koyu Granit Taş Tuğla (Ana Duvar, Act 1)

**Boyut:** 64x128px → **8 varyasyon** (straight ×2, corner ×2, T-junction ×2, end-cap ×2)
**Style Reference:** F1 floor approved tile yükle

**Prompt:**
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

**process_tiles.py:**
```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/W1 --cols 4 --rows 2 --width 64 --height 128 --prefix w1_
```

**Unity ayarları:**
- Sprite PPU: 64
- Pivot: bottom-center (0.5, 0.0)
- Sorting: Walls layer, Y-sort aktif
- WallOcclusionFader: fadeRadius 2.2, minAlpha 0.38

---

## W2 WALL — Daha Derin / Daha Bozulmuş Duvar (Orta Derinlik)

**Boyut:** 64x128px → **8 varyasyon**
**Style Reference:** W1 approved tile yükle (ortak palette)

**Prompt:**
```
Isometric pixel art stone wall tile, 64x128 pixels, 2:1 isometric projection. Pure solid green #00FF00 background.

Same three-zone structure as W1: top face 12px, front face 104px, base shadow 12px.

Palette STRICTLY: #18181E (deep mortar), #26293A (very dark stone), #363A4A (dark mid stone), #464B5E (mid stone), #565C70 (lit face). Slightly cooler/bluer than W1 — deeper dungeon feeling.

Same flat-shaded pixel art rules: NO gradient, NO smooth shading, dithered only, hard pixel edges, pixel clusters min 4px.

Brick pattern shows MORE damage than W1: wider mortar cracks, missing brick corners, occasional horizontal fracture line crossing 2-3 bricks. One variation has a faint bioluminescent lichen vein (#1E3028, max 8px) along a crack.

Same 8 connection variants as W1: straight north, straight south, outer corner NE, outer corner NW, inner corner, T-junction, end-cap north, end-cap south.
```

**process_tiles.py:**
```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/W2 --cols 4 --rows 2 --width 64 --height 128 --prefix w2_
```

---

## OBW — Overbuilt Wall (Yüksek Duvar, Tavan/Kemer)

**Boyut:** 64x128px → **4 varyasyon**

**Prompt:**
```
Isometric pixel art tall architectural wall section, 64x128 pixels, 2:1 isometric. Pure solid green #00FF00 background. This is a TALLER wall section — no top face visible (wall extends above frame), no base shadow (wall extends below frame). Front face only: 128px vertical stone masonry. Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260. Flat-shaded, dithered, hard pixel edges. 4 variations: plain stone, with narrow window slit (4x12px dark void), with iron wall bracket, with carved rune (simple geometric glyph, 8x8px, raised relief).
```

**process_tiles.py:**
```
python STAGING/process_tiles.py --source <output_sheet> --output Assets/Art/Tiles/Act1/OBW --cols 2 --rows 2 --width 64 --height 128 --prefix obw_
```

---

## ÜRETIM SIRASI

1. W1 straight tile (8 vars) → QC Unity'de
2. Style reference olarak W1 straight'i yükle → W2 üret
3. W1 + W2 yan yana Unity'de karşılaştır → palette tutarlılık check
4. OBW son üret (W1/W2 approved olduktan sonra)
