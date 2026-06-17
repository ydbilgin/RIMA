# COUNCIL-TEST 3 — HUD readability BAĞIMSIZ RUNTIME (cx lens)

ACTIVE RULES: (1) think before acting (2) READ-ONLY test (3) Unity KOD/GIT mutasyonu YOK — sadece play+assert+screenshot+read_console (4) BLOCKED if unclear.

## ⛔ READ-ONLY
Kod/asset/scene DEĞİŞTİRME-KAYDETME. `git add/commit` YOK. HP'yi force-sürmek için RUNTIME-only state set (kalıcı dosya/asset yazma YOK; geçici test component eklersen play sonunda kaldır). Bitince STOP + playModeStartScene=MainMenu.

## Amaç
cx executor HUD resize + low-HP vignette yaptı ama (1) screenshot YANLIŞ ekranı (ödül) yakaladı — combat HUD görülmedi, (2) low-HP vignette canlı sürülmedi. Sen bu boşlukları kapat + SDF-netliği doğrula.

## Değişen dosyalar (referans — DOKUNMA)
`Assets/Scripts/UI/HUDController.cs`, `Assets/Scripts/UI/SkillBarUI.cs`.

## Test (runtime — COMBAT sahnesi, ödül DEĞİL)
1. Combat sahnesine gir (oyuncu + HUD görünür; ödül/menü ekranı değil).
2. **HUD screenshot** al → game_view. Kontrol et:
   - HP bar ~212×16, resource ~160×10, slotlar LMB/RMB ~56 / Q-E-R-F ~44 — okunur büyüklükte mi?
   - **⚠️ SDF/pixel-perfect NETLİK:** büyütülen font/bar **bulanık mı net mi?** (ax Pro uyarısı — bulanıksa FAIL). Yakın-plan kanıtla.
3. **Low-HP vignette:** `execute_code` ile oyuncu HP'sini <%20'ye düşür (runtime-only) → screenshot. Kontrol et:
   - Full-screen kırmızı wash YOK, sadece **kenar vignette** (%12-18)?
   - **Merkez + ekran-ortası TEMİZ** mi (oynanış/telegraph görünür)?
   - <%20 pulse var mı?
4. `read_console` (Error+Warning) — 0 (post-stop teardown "objects not cleaned up" benign, ayır).
5. STOP + playModeStartScene=MainMenu.

## ÇIKTI (dönüş ≤8 satır)
`STAGING/_process/2026-06/demo_fix_tasks/TESTRESP_3_hud_cx.md`'ye yaz + screenshot yolları. Dönüşte: bar/slot okunur mu · **SDF net/bulanık** · vignette kenar-only+merkez-temiz mi · console.
