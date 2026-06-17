# COUNCIL-TEST 0 — GATE bootstrap fix BAĞIMSIZ RUNTIME REPRO (cx yekta lens)

ACTIVE RULES: (1) think before acting (2) READ-ONLY test (3) Unity'de KOD/GIT mutasyonu YOK — sadece play + assert + read_console (4) BLOCKED if unclear.

## ⛔ READ-ONLY
Kod DEĞİŞTİRME. `git add/commit` YOK. Asset/scene KAYDETME. Sadece: playModeStartScene ayarla → play → assert (execute_code) → read_console → stop → rapor. Bitince play'i STOP et + playModeStartScene'i MainMenu'de bırak.

## Amaç
Başka bir cx (executor) full-flow Director/F2 bootstrap fix'i yaptı ve "PASS" dedi. **Önceki bir denemede (ax) benzer iddia VERIFY'da çökmüştü.** Sen BAĞIMSIZ doğrula: gerçekten menüden başlayınca Director bootstrap oluyor mu?

## Değişen dosyalar (sadece referans — DOKUNMA)
`Assets/Scripts/UI/DirectorMode.cs`, `Assets/Scripts/UI/BuildModeController.cs`.

## Test adımları (RUNTIME)
1. `manage_editor` ile `playModeStartScene = MainMenu` ayarla → play.
2. Full-flow ilerle: MainMenu start → CharacterSelect → oyun/oda (executor'ın yolu: `_Arena`). Mümkünse normal akışla.
3. `execute_code` ile assert (compact string döndür):
   - `DirectorMode.Instance != null` ?
   - backquote (`) Director'ı açıyor mu (input-state veya direkt API ile)?
   - F2 → `BuildModeController` aktif + `DirectorMode.Instance`'a erişiyor mu?
   - mevcut scene adı + directorState.
4. `read_console` (Error+Warning) — 0 olmalı.
5. STOP + playModeStartScene=MainMenu.

## Sonuç
- **PASS:** üç de doğrulandı + 0 console-error. Assert string'ini kanıt olarak ver.
- **FAIL:** hangisi tutmadı + ne gördün. (Sessizce "PASS" deme — bu testin amacı ax-tipi yanlış-pozitifi yakalamak.)

## ÇIKTI (dönüş ≤8 satır)
`STAGING/_process/2026-06/demo_fix_tasks/TESTRESP_0_runtime_cx.md`'ye yaz. Dönüşte: PASS/FAIL + assert kanıtı + console durumu.
