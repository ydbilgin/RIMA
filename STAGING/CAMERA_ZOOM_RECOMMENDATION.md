# Camera Zoom Recommendation (Triple-AI, S114 2026-05-28)

Amac: Karakteri hero-scale'e yaklastirmak (kullanici "yaklasip kisalabilir" istedi). Mevcut ekranda karakter cok kucuk/genis.

## Mevcut Durum (Unity'den okundu)
- Main Camera: orthographic, orthographicSize=4, pos=(0,-0.5,-10)
- PixelPerfectCamera: assetsPPU=64, refResolution=1280x720, upscaleRT=false, pixelSnapping=false, crop=false
- Karakter sprite: warblade_south, canvas 120px / icerik 64px, PPU=64
- ETKI: 64px karakter ekran yuksekliginin ~%6-9'u -> cok genis

## KRITIK TEKNIK BULGU (Codex, paket-kaynak dogrulamali)
Pixel Perfect Camera ACIK iken `Camera.orthographicSize` runtime'da OVERRIDE edilir.
Gercek zoom kontrolu = `refResolutionX/Y` + `assetsPPU`. orthographicSize sadece PPC kapaliyken fallback.
=> Zoom degistirmek icin orthographicSize'a DOKUNMA; refResolution'i degistir.

## ONERI: refResolution 640 x 360
- Karakter ekran yuksekligi: ~%9 -> ~%17.8 (2x zoom, belirgin hero presence artisi)
- Integer-scale (shimmer/crop yok): 1080p=3x, 1440p=4x, 2160p=6x -> 3 yaygin cozunurlukte de TAM kat (tek boyle deger)
- GridSnapping = UpscaleRenderTexture (eski U2D field: upscaleRT=true), pixelSnapping kapali

## Neden 640x360 (sentez)
- agy: Hades/CoM/D3 karakter ~%10-12 -> 960x540. Ama 960x540 1440p'de integer DEGIL (2.667x).
- Codex: hero-band ~%30 -> 384x216, ama "combat icin cok dar" uyarisi + 1440p integer degil.
- 640x360 = ortadaki tatli nokta (~%17.8), tek cross-resolution integer-safe deger. Hem belirgin hero his hem telegraph/dash okunabilirligi korunur.

## Trade-off / Fallback
- 640x360 Codex'in 384x216'sindan daha az "agresif". Daha fazla punch istenirse 384x216 (~%29.6) alternatif AMA 1440p integer kaybi.
- Cok yakin (>%35 / <180px) = dash hedefi + uzak telegraph ekran disi kalir.
- Cok genis (mevcut ~%9) = hero his + screen shake + slash etkisi zayif.

## Uygulanis (kullanici onayindan SONRA)
Main Camera -> PixelPerfectCamera:
- refResolutionX = 640
- refResolutionY = 360
- upscaleRT (GridSnapping) = UpscaleRenderTexture / true
- orthographicSize'a DOKUNMA (PPC zaten override eder)
