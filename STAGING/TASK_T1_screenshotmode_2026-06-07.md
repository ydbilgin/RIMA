# TASK T1: ScreenshotMode-lite + deterministic seed

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç
MASTER_PLAN T1 (`STAGING/MASTER_PLAN_FINAL_2026-06-06.md` §C/T1 — READ): tek tuşla debug'sız, tekrarlanabilir kareler + temiz canlı demo. Jüri sunumu ve rapor şekilleri (T9) buna bağlı. Unity OPEN.

## Known debug surfaces (prior cx audit, file:line — verify, may have drifted)
- `Assets/Scripts/Debug/DemoDebugPanel.cs` (F1 panel: kill mobs/god/speed/force clear)
- `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (F2 overlay)
- Dummy HP label (ChamberSelectBootstrap ~:345-372)
- `IsoRoomBuilder` marker container (spawn/door markers, :551-601 BuildMarkers)
- DeathScreen/retry overlay leak risk during combat shots (DeathScreenManager)
- Editor: `Assets/Editor/DevTools/GameViewSetup.cs` (1920x1080 + maximize — REUSE)

## Work items
1. **ScreenshotMode runtime controller** (new, small): static toggle (F12) + registry pattern — components self-register as "debug surface" (interface or attribute, your call, MIN CODE) → toggle hides/shows all. Register: DemoDebugPanel, InPlayMapPaintOverlay, dummy-HP label, marker container, any dev-only TMP/text found. Death overlay: NOT hidden when actually dead (it's content) — only guard against leak-during-combat if such a bug exists (verify; don't invent).
2. **Kamera preset'leri (6):** chamber-wide · pedestal-close · combat · draft · doors · room-overview. Her preset = kamera pos/ortho-size (+ hangi sahnede anlamlıysa). Basit: ScriptableObject ya da serialized liste + F11 cycle / menü. Screenshot çekimi: `ScreenCapture.CaptureScreenshot` (süper-size 1-2) → `STAGING/screenshots_auto/<preset>_<timestamp>.png`. NOT: ScreenCapture overlay-UI'ı DA yakalar (bilinen davranış) — HUD'lı/HUD'suz iki varyant flag'i ekle (canvas'ları kapatıp çek).
3. **Deterministic seed:** BridsonPoissonAutoPlacer + oda re-seed yolundaki Random kullanımına seed parametresi; RoomRunDirector zaten `runSeed + CurrentNodeId` kullanıyor (Pick) — prop placer'ın da aynı deterministik kaynaktan beslendiğini garanti et. Editor Rooms tab "Auto Props 🎲" mevcut seed akışı bozulmasın. AC: aynı seed + aynı template → birebir aynı prop dizilimi (iki koşuda pozisyon listesi eşit — test ya da execute_code kanıtı).
4. **Verify:** compile clean · F12 toggle play-mode'da debug yüzeylerini gizliyor (probe + screenshot kanıtı) · 6 preset çekimi dosya üretiyor · seed determinizmi kanıtlı · smoke 26/26 + EditMode önceki yeşiller bozulmadı.
5. **Commit** (ydbilgin, English, no Co-Authored-By): `feat(tools): screenshot mode with camera presets and deterministic prop seeds`. CODEX_DONE'a file:line + kanıtlar.

## Constraints
- UnifiedMapDesigner / RoomTemplateJsonExporter / RoomJsonImporter / WalkabilityMap / KnockbackReceiver dosyalarına DOKUNMA (bu gece başka commit'ler dokundu, çakışma istemiyoruz; gerekiyorsa BLOCKED yaz).
- Build Settings'e yeni sahne EKLEME.
- Working tree'deki ilgisiz dosyaları commit'leme.
