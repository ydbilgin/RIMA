# CODEX TASK — Unified Painter Paint Regression Diagnose (S95)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun

User feedback: "Unified painter ile eskiden doğru boyayabiliyordum, şimdi boyayamıyorum."

S95 LATE NIGHT 2 büyük refactor (+1316/-355 satır) sonrası regression. CURRENT_STATUS'ta 4 bug fix PASS yazıyor ama yeni bir regression olmuş.

## Görev — Sadece TANI, FIX YOK

Şu dosyaları inceleyip neden paint çalışmıyor diye RAPOR yaz. Hiçbir kod değişikliği yapma. Hipotez listesi ver, en güçlüsünü işaret et.

## Read

1. `Assets/Editor/RimaUnifiedPainterWindow.cs` — ana painter window
2. `Assets/Editor/CollisionRulesSO.cs` — yeni helper
3. `Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset` — default collision rules
4. `Assets/Editor/IsometricSortSetup.cs` — Walls sortingLayer fix
5. `Assets/Editor/RimaSortingLayerValidator.cs` — canonical 5-layer
6. Recent commits: `git log --oneline -20 -- Assets/Editor/`
7. Sahne: `Assets/Scenes/Demo/PathC_BaseTest.unity` (live) ve yeni `IsoShowcaseRoom_S95.unity`

## Hipotezler (kontrol et)

1. **Palette discovery broken** — `pilot_a_wall_*` görünmüyor mu? Folder scan kuralları
2. **Collision rule denying** — DefaultCollisionRules.asset ile collision check her şeyi reject ediyor mu?
3. **GameObject parent missing** — Painter doğru parent'ı bulamıyor (Walls_Root vs Grid)
4. **Sorting layer literal mismatch** — Hardcoded "Wall" (singular) script'lerde kaldı mı? Canonical "Walls"
5. **Selected Instance Editor scope bug** — Panel 5 instance edit modda paint engellenmiş olabilir
6. **Multi-folder palette scanner regression** — Sadece eski path tarıyor olabilir
7. **GroupClassifier strict** — Wall group classification fail edip palette boş kalıyor

## Rapor

`STAGING/CODEX_DONE_painter_diagnose.md`:
- Hipotez listesi
- En muhtemel sebep + evidence (kod satırı + neden)
- Reproduction step (user'ın deneyimi)
- Önerilen fix yol (sadece taslak, KOD YAZMA)
- Risk: refactor sonrası şu update'te paint regression kalıcı olabilir mi?

## Constraint

- Kod yazma. Sadece raporla.
- Sahne dosyalarına dokunma.
- ~150 satır rapor yeterli.

## Effort

low-medium — ~30 dakika kod okuma.
