# F1 Floor Tile — PixelLab Prompt v2
# Act 1: Shattered Keep | 64x64 px | 16 variations

---

## Arka Plan

Bu prompt, RIMA roguelite oyununun Act 1 "Shattered Keep" bolumu icin zemin tile'lari uretir.
Stil: Sogukkanlı gri granit tas, seyrek yosun, baskici bir zindan atmosferi.
Mevcut pipeline: PixelLab (Create Image Pro) → process_tiles.py (green chromakey) → Unity Tilemap.

Onceki F1 promtlarinda gorsel tutarsizliklar yaşandi: bazi varyasyonlarda
highlight cok belirgin, bazi varyasyonlarda perspective bozuldu.
Bu v2 prompt bu sorunlari gidermek icin yeniden yazildi.

---

## Tam PixelLab Promptu (copy-paste hazir)

```
Isometric pixel art floor tile, 64x64 pixels, pure solid green background #00FF00 fills every pixel outside the tile shape. The tile is a perfect 2:1 diamond rhombus viewed strictly from top-down isometric angle: left and right corners at horizontal midpoints, top and bottom corners at vertical midpoints. Stone surface texture with visible mortar lines forming a staggered brick pattern. Palette strictly: dark gray #2A2A35, mid gray #404050, muted blue-gray #505565, highlight cap #606575. Flat dungeon baked-in lighting, NO dramatic highlights, NO gradients, NO dithering beyond simple 2x2 checker. Pixel clusters minimum 4px wide. Each of the 16 variations has a subtly different worn stone pattern: micro-cracks, single moss patch, corner erosion, or faint water stain — only one accent feature per tile, never more than 3 pixels of moss color. Tile edges are seamless — the pattern continues flush to the diamond boundary so adjacent tiles tesselate without visible seam. No shadows outside the tile shape. Background is pure flat #00FF00 only.
```

---

## PixelLab Ayarlari (Settings)

| Parametre         | Deger              |
|-------------------|--------------------|
| Tool              | Create Image Pro   |
| Canvas Width      | 64                 |
| Canvas Height     | 64                 |
| Pixel Art Mode    | ON                 |
| Style Preset      | (bos birak)        |
| Variations        | 16                 |
| Background Color  | #00FF00            |
| Upscale           | OFF                |

---

## Chromakey Notu

Arka plan MUTLAKA saf yesil #00FF00 olmali.
process_tiles.py filtresi: G > 200 AND R < 60 AND B < 60
Magenta artik kullanilmiyor (deprecated 2026-05-09).
Turuncu, beyaz veya seffaf arka plan kullanma — pipeline bozulur.

---

## process_tiles.py Komutu

```
python process_tiles.py \
  --input   STAGING/TILESET_OUTPUT/f1_raw/ \
  --output  Assets/Art/Tiles/Act1/F1/ \
  --prefix  f1_ \
  --size    64 \
  --chroma  "0,255,0" \
  --threshold 60
```

Cikan PNG dosyalari: f1_00.png ... f1_15.png (16 adet)
Her dosya 64x64, seffaf arka plan (alpha channel).

---

## Dikkat Edilecekler

- Tile kenar pikselleri: diamond sinirinin tam disindaki piksel #00FF00 olmali, icindeki tas rengi olmali.
- PixelLab bazen kose piksellerini yumusatir — prompt'a "hard pixel edges, no anti-aliasing on tile boundary" eklenebilir.
- 16 varyasyonun tamami ayni kamera acisinda olmali — bazi uretimlerde 1-2 varyasyon yanlik aci uretebilir, bunlari ayikla.
- Varyasyonlari numune goz ile kontrol et: duzensizlik iyi, belirgin hue kayması kotu.
