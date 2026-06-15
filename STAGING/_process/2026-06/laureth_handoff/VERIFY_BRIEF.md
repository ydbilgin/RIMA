# VERIFY — Demo batch-fix 6 cerrahi fix, diff'i spec'e karşı bağımsız doğrula

ACTIVE RULES: (1) think (2) min — sadece doğrula, kod yazma (3) surgical — listelenen 6 fix + diff (4) BLOCKED if unclear.
GRAPHIFY: cross-file gerekirse graph.json (STAGING/_process/2026-06/graphify_fullmap/graphify-out/), bulk-read'den ~71x ucuz.

## DURUM
crafter-sonnet 6 cerrahi fix uyguladı (working tree, UNCOMMITTED), compile 0-error rapor etti. Sen **bağımsız reviewer**'sın (writer≠reviewer). Spec: `STAGING/DEMO_BATCH_FIX_SPEC_2026-06-15.md`. Executor raporu: `STAGING/_process/2026-06/laureth_handoff/BATCHFIX_RESULT.md`.

## YÖNTEM
1. `git diff` ile 6 dosyanın working-tree değiş-imini gör: DraftManager.cs · BuildPlacementController.cs · BuildModeController.cs · DirectorMode.cs · RoomRunDirector.cs.
2. Spec'teki FIX-1..FIX-6 ile karşılaştır.
3. **cx'e özel (Unity erişimin var):** TEK `read_console` (Error+Warning) ile 0-error teyit et. **ax_opus: Unity'ye DOKUNMA, salt-statik diff incele.**

## HER FIX İÇİN YARGI (PASS / FAIL / BLOCKED + 1 satır)
- **present?** değişiklik kodda var mı
- **matchesSpec?** guard metodun EN BAŞINDA mı; FIX-1 unsubscribe OnDisable+OnDestroy simetrik+null-guard mı; FIX-6 StopClearSequences stop+null mı
- **surgical?** o dosyanın diff'i SADECE bu fix mi (ilgisiz refactor YOK)
- **regression riski?** golden-path'i (videodaki akış) bozar mı: none/low/med/high
- FIX-5 overlay-fix else bloğuna bağlı — yoksa BLOCKED (uydurma değil)

## EK KONTROL
- **YAPMA listesi gerçekten dokunulmamış mı?** Timescale/GameTimeCoordinator (RIMA-001), draft-serialization, BuildMode-FSM, RewardPickup timeout, Director bootstrap → bunlara DOKUNULMAMIŞ olmalı. Diff'te bunlardan iz varsa FAIL+bildir.

## ÇIKTI (E1)
Dosyaya yaz: cx → `STAGING/_process/2026-06/laureth_handoff/VERIFY_cx.md` · ax_opus → `STAGING/_process/2026-06/laureth_handoff/VERIFY_axopus.md`.
Format: FIX-1..6 her biri tek satır (PASS/FAIL/BLOCKED + present/spec/surgical/risk + neden) + YAPMA-listesi-temiz mi + son "TEK CÜMLE: commit'e hazır mı?". Orchestrator'a dönüş ≤10 satır + dosya yolu.
