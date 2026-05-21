# Codex Task — Silhouettes v2 at Standard Sizes

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Mevcut filled silhouette PNG'leri (original target dim: 96×160 vs) **standard power-of-2 sizes**'da regenerate et. Bu PNG'ler PixelLab Web UI **Create Image S-XL (new)** tool'una init image olarak yüklenecek.

## Output Dosyalar — Standard Sizes (Power-of-2)

| Dosya | Boyut | Filled 3D box specs |
|---|---|---|
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_straight_silhouette_v2.png` | **128×256** | Box: w=104, d=24, h=224 (tall wall fills 90% canvas) |
| `STAGING/_pixellab_inputs/solo/act1_wall_tall_corner_silhouette_v2.png` | **256×256** | L-shape: arm1 w=120 d=24, arm2 w=24 d=120, h=224 |
| `STAGING/_pixellab_inputs/solo/act1_wall_archway_silhouette_v2.png` | **256×256** | Box: w=200, d=24, h=224, arch opening 100w × 140h centered |
| `STAGING/_pixellab_inputs/solo/act1_wall_endcap_column_silhouette_v2.png` | **64×256** | Box: w=48, d=48, h=224 |

## Visual Specs (Aynı önceki silhouette yaklaşımı)

3 görünür face shaded:
- **Top face:** `#A0A0A0` (en açık)
- **Front face:** `#808080` (orta)
- **Right face:** `#606060` (en koyu)

Hidden lines YOK, sadece filled polygons.
Transparent background.
Archway için: front face'te arch opening alpha=0 (transparent içeri görünür).

## Python Skeleton (Önceki task'ten adapt)

Önceki `codex_task_filled_silhouette_outlines.md`'deki skeleton'u kullan, sadece:
1. Canvas boyutları yukarıdaki tabloya göre
2. Box dimensions yukarıdaki specs'e göre
3. Output filenames `_silhouette_v2.png` suffix

Edge cases:
- Archway transparent cutout PIL'de düzgün çalışıyor mu önceki testte doğrulandı
- L-shape (corner): 2 box union, draw order doğru
- 256×256 canvas geniş, box merkezde kalsın (margin = (256 - box_visual_width) / 2)

## Verification

Her PNG için:
- ✅ Canvas tam standard boyut
- ✅ Transparent background (silhouette dışı)
- ✅ 3-face shaded silhouette
- ✅ Hidden lines yok
- ✅ Archway arch interior transparent

## Effort

low-medium — mevcut script template var, sadece boyut + scale ayarı.

## Output Confirmation

4 path listele, dimensions doğrula.
