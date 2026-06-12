# 01 — Tool Vizyonu (kullanıcı)

## Konsept: Sandbox / Director Mode
Oyun-içi, build'de çalışan, sekmeli, GÜZEL bir HUD (raw IMGUI/wireframe DEĞİL). Demo amacı: **dengeleme altyapısını canlı interaktif göstermek** ("altyapı kuruldu, animasyonlar sonra gelecek").

## Mod akışı
```
[OYUN] ──`──> [DIRECTOR: kur] ──Başlat──> [TEST: oyna] ──`──> [DIRECTOR]
```
- Master tuş ` (backtick) → Director Mode: oyun durur (timeScale=0), kamera yukarı serbest-cam'e kalkar (pan+zoom), sahneyi kurarsın
- "Başlat" → kameraya dalış, timeScale=1, karakteri oynat = stress test
- ` tekrar → Director'a dön (sahne korunur, kümülatif)

## Kullanıcının istediği yetenekler
- **Mob/boss spawn — SINIRSIZ**, tıkla-koy, istediğin kadar
- **Map** uzaktan görüntüle + oyun içinden seç (node'a atla)
- **Stress test** (kur → başlat → izle)
- **Karakter değiştir** oyun içinde (10 class)
- **Skill değiştir** oyun içinde
- **Tile koyma** (ayrı sekme)
- **Cliff generate** (ayrı sekme)
- **Stat ayarla** (canlı slider: HP/phys/AP/atkSpeed/moveSpeed)
- **Telemetri** (DPS/TTK/CSV export — "dengeleme verisi")

## Görsel hedef
Mevcut ürettiğimiz **pixel-art chrome kit** ile skinlenmeli (ekte). Act1 paleti: void mor #3A1A4A, ember #E89020, slate #3A3D42, cyan ≤%15. "Debug ama profesyonel görünen" tool.
