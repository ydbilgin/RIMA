# TASK 4 — Black-blob rim-light okunabilirlik (~1-1.5h)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`).

## Bağlam
Karar: `STAGING/CHATGPT_REV2_COUNCIL_DECISION_2026-06-17.md` (Adım 4). ChatGPT: koyu düşman + kapılar void arka planda "siyah boşluk" gibi kayboluyor → 1-2px rim-light + iç-değer ver.

## Mevcut altyapı (ÖNCE oku — yeniden kurma)
- `Assets/Scripts/Enemy/Telegraph/EnemyOutlinePulse.cs` — outline-shader props (`_OutlineColor/_OutlineAlpha/_OutlineThickness/_OutlineWidth`) VAR; material desteklemezse sprite-color tint fallback. AMA sadece telegraph SIRASINDA pulse ediyor (geçici).
- Outline shader/material asset gerçekten var mı kontrol et (EnemyOutlinePulse'ın beklediği `_Outline*` prop'ları hangi material/shader'da). Yoksa fallback path aktif demektir.

## Hedef
Koyu sprite'lar (dark düşmanlar + kapı/DoorTrigger görselleri) void arka planda **okunur** olsun: **kalıcı (telegraph-dışı), hafif rim/outline veya iç-değer.**
- Yaklaşımı SEN seç (mevcut mimariye en cerrahi olan), gerekçeni yaz:
  - (a) Mevcut outline shader varsa: dark objelere kalıcı düşük-alpha outline (telegraph pulse onun ÜstÜne binsin, çakışmasın), VEYA
  - (b) Hafif rim-light material/sprite-outline, VEYA
  - (c) Çok dark sprite'lara hafif iç-değer/ambient-tint.
- Subtle: amaç "okunur silüet", neon-çerçeve DEĞİL. Palet uyumlu (cyan ≤%15; rim için kırık-beyaz/soğuk-ton).

## Kısıt
- **Telegraph `EnemyOutlinePulse` davranışını BOZMA** — kalıcı outline eklenirse pulse hâlâ çalışmalı (çakışma yok).
- Cerrahi: rim/outline ekleme noktası + (gerekirse) material. Combat/AI mantığına DOKUNMA.
- Tüm sprite'lara global outline ATMA — sadece void'de kaybolan koyu düşman + kapı. Hero/parlak sprite'lar etkilenmesin.
- git'e DOKUNMA.

## VERIFY (runtime)
- Combat sahnesi + boss/dark-mob + void arka plan. Screenshot: koyu düşman + kapı silüeti **okunur** mu (önce/sonra fark)? Telegraph hâlâ çalışıyor mu (pulse görünür)?
- `read_console` 0-error.

## ÇIKTI (E1: ≤10 satır)
Evidence + screenshot → `STAGING/_process/2026-06/demo_fix_tasks/DONE_4_blackblob.md`. Dönüşte: değişen dosyalar + hangi yaklaşım+gerekçe + okunabilirlik verify (önce/sonra) + telegraph-bozulmadı teyidi + console.
