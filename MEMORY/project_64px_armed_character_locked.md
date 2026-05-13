# 64x64 Armed Character Sprite — LOCKED (Karar #73)

**Tarih:** 2026-05-12
**Status:** LOCKED
**Owner:** rima-asset, rima-design

## Mimari Karar
Karakter sprite formatı: **64x64 chibi pixel art**, silah karakterle aynı sprite içinde (1-piece, body+weapon separate DEĞİL).

## Gerekçe
- 32x32 chibi'de ~11px kafa = yüz detayı yok, animasyon pipeline yetersiz (S62 mature pivot REVOKED, Karar #100)
- Body+weapon ayrı sprite anchor sistemi (eski S57-S58) revoked — Karar #72 ile 2.5D pipeline tamamen kaldırıldı
- 64x64 chibi: yüz okunaklı, animasyon detayı yeterli, PPU=64 ile pixel-perfect uyum

## Uygulama Kuralları
- Tüm playable class (10), T2 mob (6+), boss (4): 64x64 native generation
- Silah karakterin elinde (sol el konvansiyonu, Karar #99 weapon-in-hand)
- 8 yön animasyon (Karar #114): 5 gen + 3 mirror
- View angle: ~35° high top-down (Karar #113)
- PPU=64, Point filter, no compression, no mipmap (S60 LOCKED pipeline)

## Referanslar
- Karar #100 (chibi 64x64 RESTORE)
- Karar #104 (10/10 anchor PASS)
- Karar #105 (Create Character: view=low top-down, n_directions=8, proportions=chibi)
- Karar #113 (Camera Convergence ~35°)
- Karar #114 (8 Direction Animation)
