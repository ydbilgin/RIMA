# W1 Wall Tile — PixelLab Prompt v2
# Act 1: Shattered Keep | 64x96 px | 16 variations

---

## Arka Plan

Bu prompt, RIMA roguelite oyununun Act 1 "Shattered Keep" bolumu icin duvar tile'lari uretir.
Stil: Cok koyu lacivert-gri tas tugla, kismen ustu gorunen isometrik on cephe, sade zindan duvari.
Canvas 64x96'dir — duvar zeminden 1.5x daha yuksek, Unity'de 64px PPU ile dogru yukseklik verir.

Isometrik duvarda uc katman net ayirt edilmeli:
  - Ust yuz (8-12px): hafif daha acik, tepeden gelen ambient isigin vurugu
  - On yuz (geri kalan yukseklik): ana tugla desen
  - Alt birlesim golgesi (4-6px): zemin tile ile birleşen AO serit

Kapi, pencere, dekorasyon KESINLIKLE olmayacak.

---

## Tam PixelLab Promptu (copy-paste hazir)

```
Isometric pixel art wall tile, 64x96 pixels, pure solid green background #00FF00 fills every pixel outside the tile shape. The tile has three distinct zones stacked vertically: top face zone (top 10 pixels) shows the wall top surface in a slightly lighter tone viewed from isometric angle; front face zone (middle 76 pixels) shows staggered brick masonry pattern on a vertical front surface; base shadow zone (bottom 10 pixels) shows a soft ambient occlusion gradient where the wall meets the floor. Palette strictly: very dark navy-gray #1A1A25, dark gray #2D2D40, mid stone #3A3A50, top-face highlight #464660. Flat baked dungeon lighting, NO windows, NO doors, NO torches, NO decorations — plain stone wall only. Brick pattern uses irregular mortar lines, each brick 12-16px wide, 6-8px tall in the front face zone. Pixel clusters minimum 4px. Each of the 16 variations has one subtle weathering detail: a hairline crack, a moss streak, a water damage discoloration, or a chipped brick corner — never more than one accent per tile. Top face edge aligns flush with the tile's top boundary so adjacent wall tiles and floor tiles connect without gap. Background outside the tile silhouette is pure flat #00FF00 only. No anti-aliasing on tile boundary. Hard pixel edges.
```

---

## PixelLab Ayarlari (Settings)

| Parametre         | Deger              |
|-------------------|--------------------|
| Tool              | Create Image Pro   |
| Canvas Width      | 64                 |
| Canvas Height     | 96                 |
| Pixel Art Mode    | ON                 |
| Style Preset      | (bos birak)        |
| Variations        | 16                 |
| Background Color  | #00FF00            |
| Upscale           | OFF                |

---

## Chromakey Notu

Arka plan MUTLAKA saf yesil #00FF00 olmali.
process_tiles.py filtresi: G > 200 AND R < 60 AND B < 60
Duvar tile'larinda arka plan rengi ile duvar koyu tonlari cok yakin olmamali —
#1A1A25 chromakey filtreden gecmez (yeterince koyu, R ve B dusuk ama G=0x1A=26, filtre guvende).

---

## process_tiles.py Komutu

```
python process_tiles.py \
  --input   STAGING/TILESET_OUTPUT/w1_raw/ \
  --output  Assets/Art/Tiles/Act1/W1/ \
  --prefix  w1_ \
  --size    64x96 \
  --chroma  "0,255,0" \
  --threshold 60
```

Cikan PNG dosyalari: w1_00.png ... w1_15.png (16 adet)
Her dosya 64x96, seffaf arka plan (alpha channel).

---

## Unity Import Notu

- Sprite PPU: 64 (her iki boyut icin)
- Tile height Unity'de 96px = 1.5 Unity birimi
- Wall tile'lari, floor tile'larinin uzerine Y offset ile yerlestirilmeli
- Sorting order: wall tile floor'dan yuksek olmali (Y-Sort aktif)
- Pivot: tile'in alt ortasi (0.5, 0.0) olmali ki zemin hizasi dogru cakissin

---

## Dikkat Edilecekler

- 64x96 canvas'ta ust yuz ile on yuz arasindaki gecis cizgisi net olmali — iki zon tek bir duvarmis gibi akmalı ama ton farki ayirt edilebilmeli.
- PixelLab bazen duvar on yuzunu yatay duzlemde gosterebilir (floor gibi) — bu yanlistir. Prompt'ta "vertical front surface" vurgusu bunu onler.
- 16 varyasyonun hepsi ayni aci ve ayni uc-zon yapisini koruyor olmali.
- Alt AO golge seridi floor tile ile gorsel baglantı saglar — bu serit olmadan duvar zeminde yuksuyor gibi gorunur.
