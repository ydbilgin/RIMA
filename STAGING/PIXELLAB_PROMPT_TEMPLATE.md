# PixelLab Prompt Yapısı — Genel Şablon
Date: 2026-05-07

Bu yapı net adımlar verince daha iyi çıktı üretiyor.
Her animasyon promptunda bu 3 blok + kapanış satırı kullan.

---

## YAPI

```
[CHARACTER]
TYPE: ...
HEAD: ...
BODY: ...
LIMBS: ...
EXTRA: ...
CLOTHING: ...
HANDS: ...
SILHOUETTE: ...
COLOR: ...

[ACTION]
Frame 1 (...): ...
Frame 2 (...): ...
...
(ya da tek aksiyon açıklaması, frame bazlı değilse)

[CONSTRAINTS]
SIZE LOCK: Each frame must use identical [W]x[H]px canvas. No scaling, zooming, cropping, or resizing between frames.
FOOTPRINT LOCK: All frames must share identical pixel extents top, bottom, left, right. No overflow beyond original sprite bounds.
ANCHOR: Feet must align to the same pixel row across all frames. Head height must match exactly.

2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
```

---

## NOTLAR

- [ACTION] kısmında anatomik dil kullan: "left foot forward and planted, right foot pushing off" — "slight lean" değil
- [CONSTRAINTS] her promptta aynı kalır, değiştirme
- Frame bazlı aksiyonlarda "Frame N (etiket): ..." formatı kullan
- PEAK frame tekniği: önce sadece PEAK frame'i üret, sonra Interpolate (New) ile doldur
- Silah yoksa "HANDS: empty" ve "weapon added as separate pass" ekle

---

## TOOL REFERANSI (2026-05-07 itibarıyla)

| İş | Tool |
|----|------|
| Animasyon | **Animate with Text (New)** |
| Ara frame | **Interpolate (New)** |
| Zemin tile | **Tileset Generation** (Isometric seç) |
| Obje / duvar | **Create Image Pro (Pixflux)** |

> Adında "New" geçen tool varsa onu tercih et. Periyodik kontrol: Gemini veya Codex'e sor.
