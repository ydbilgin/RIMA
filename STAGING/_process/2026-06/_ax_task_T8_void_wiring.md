# GÖREV T8: Void parallax + oda makyajı wiring (Unity implementasyonu — ax) [SONRAKİ SESSION DISPATCH EDER — T5 bitmeden BAŞLATMA]

Sen RIMA Unity projesinde çalışan bir geliştirici ajansın. UnityMCP araçların var (Unity AÇIK olmalı). Proje kökü: `F:\Antigravity Projeler\2d roguelite\RIMA`.

## Bağlam
Run odaları void üstünde yüzen adalar; arka plan şu an düz siyah. Batch-3 asset'leri HAZIR: `STAGING/imagegen/assets/batch3_2026-06-07/` (3 uzak ada silüeti, 1024² nebula, delik-rim ×4, zemin decal ×6, kenar prop ×6, parapet ×2). Spec: `STAGING/VOID_BACKGROUND_DEPTH_SPEC_2026-06-07.md` (5 katman + parallax ≤0.2 + pixel-quantize + mood-tint). Oda kurucu: `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (canlı yol — LIVE_FLOW_PROOF_2026-06-07.md).

## KURALLAR
- Legacy dosyalara dokunma · COMMIT yapma · sahne kaydetme · çıkışta play kapalı
- Sorting: nebula/silüetler EN ARKA (Floor'un altında yeni "VoidBackground" layer gerekirse TagManager'a ekle), rim/decal'ler "Decals" layer (mevcut)
- Parallax: mevcut parallax altyapısı varsa REUSE (rg "parallax"), yoksa basit kamera-takipli offset script'i (≤0.2 çarpan, jitter'sız — pozisyonu pixel-snap'le)

## İşler
1. Batch-3 asset'lerini import et: silüetler+nebula → `Assets/Art/Void/`, rim/decal/filler/parapet → `Assets/Art/RoomMakeup/` (Point, PPU 64, uncompressed)
2. **Void katmanları:** _Arena'da oda kurulurken arkaya nebula (geniş, koyu, tile'lı) + 2-3 uzak ada silüeti (farklı derinlik, parallax 0.05/0.1/0.15) — IsoRoomBuilder'a veya ayrı VoidBackdropBuilder'a bağla (hangisi minimal, sen seç+gerekçele)
3. **Delik rim'leri:** donut tipi odalarda iç-delik kenarlarına rim decal'leri otomatik yerleşsin (IsoRoomBuilder delik kenarı tespiti — walkable'dan void'e geçen iç kenar hücreleri)
4. **Mood-tint:** oda tipine göre void tint'i (combat=nötr koyu, elite=kızıl esinti, boss=daha koyu+kızıl) — spec'teki değerler
5. Edge filler/parapet OTOMATİK YERLEŞİM YOK (o ayrı kompozisyon işi) — sadece import edilip hazır dursunlar

## DOĞRULAMA
Compile 0 error · play probe: combat + donut + boss odası kareleri `STAGING/_process/2026-06/t8_void_combat.png`, `t8_void_donut.png`, `t8_void_boss.png` (silüetler+nebula görünür, parallax çalışır, rim'ler delik kenarında) · smoke 26/26 bozulmaz

## ÇIKTI
`STAGING/_process/2026-06/_done_T8_void.md`: işler DONE/BLOCKED + seçilen mimari + dosya:satır + 3 screenshot.
