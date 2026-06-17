# COUNCIL-TEST 11 — Build Mode functional BAĞIMSIZ RUNTIME (cx lens)

ACTIVE RULES: (1) think before acting (2) READ-ONLY test (3) Unity KOD/GIT mutasyonu YOK — sadece F2 aç + paint dene + assert + read_console (4) BLOCKED if unclear.

## ⛔ READ-ONLY
Kalıcı kod/asset/scene KAYDETME (Build working-copy testi serbest ama save etme). `git add/commit` YOK. Bitince play STOP + playModeStartScene=MainMenu.

## Amaç
Başka cx executor Build Mode functional fix yaptı (kullanıcının canlı-test bug'ları). Sen BAĞIMSIZ doğrula — kullanıcının ASIL şikayetleri:
1. **Floor boyanabiliyor mu:** F2 aç → FLOOR brush seç → viewport'a TIKLA/SÜRÜKLE → tile GERÇEKTEN yerleşiyor mu? (önceden IsPointerOverUi tüm-ekranı blokluyordu)
2. **Walkable boyanabiliyor mu:** WALKABLE layer seç → boya → yerleşiyor mu?
3. **Snap/hiza:** 2-3 komşu floor tile boya → dikişsiz mi, grid-diamond'a oturuyor mu, overlap/gap var mı?
4. **Phantom barrel:** asset browser'da "wooden barrel" GÖRÜNÜYOR mu (görünmemeli — sprite'sız filtrelendi)?

## Değişen dosyalar (referans — DOKUNMA)
`BuildTileBrushController.cs`, `BuildPlacementController.cs`, `BuildModeAssetCatalog.cs`.

## Test (runtime — F2 Build Mode)
GATE fix'i sayesinde F2 full-flow veya _Arena'dan açılır. `execute_code` ile gerçek paint-op tetikle (cursor world-pos → commit), tile-count önce/sonra assert et. Screenshot al.
- `read_console` 0-error.

## ÇIKTI (dönüş ≤8 satır)
`STAGING/_process/2026-06/demo_fix_tasks/TESTRESP_11_build_cx.md`'ye yaz + screenshot. Dönüşte: floor-paint PASS/FAIL · walkable-paint PASS/FAIL · snap (overlap/gap?) · barrel görünür mü · console.
