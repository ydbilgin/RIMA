ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA bitirme (Senior Design) SUNUMU için GROUND-TRUTH topla — ne GERÇEKTEN demoable, hangi tool çalışıyor, projenin akademik tezi + teknik talking-point'ler/metrikler. ANALYSIS ONLY. Sonuç CODEX_DONE_laurethayday.md.

# OKU
- `CURRENT_STATUS.md` (demo durumu)
- `STAGING/report/RAPOR_DRAFT_2026-06-06.md` (tez + §4.5 görsel-envanter + agentic-AI §5) — özellikle tez/novelty ve metrikler
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` + `DungeonGraph.cs` (demo akışı)
- `Assets/Editor/Rooms/RoomTemplateBrowserWindow.cs` + `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` (sunum tool'ları)

# SORULAR
1. **NE DEMOABLE (uçtan uca)?** MainMenu→CharacterSelect→_Arena→Combat→Shop→Boss→dual-class→Victory zinciri GERÇEKTEN çalışıyor mu (editörden)? Hangi adım kırılgan/riskli? Build gerekli mi yoksa editör-demo yeter mi?
2. **SUNUM TOOL'LARI:** RIMA/Room Browser canlı oda-gösterimi için çalışıyor mu (tıkla→_Arena'da kur)? RIMA/Map Designer Rooms sekmesi authoring-kanıtı olarak gösterilebilir mi? Hangileri stabil, hangileri demo'da patlayabilir?
3. **AKADEMİK TEZ + METRİKLER:** Projenin novelty'si nedir (agentic-AI orkestrasyon pipeline'ı)? Sunumda kullanılacak SOMUT metrikler: test sayısı, commit sayısı, oda template sayısı (DemoRoomBank + Generated), kullanılan agent/model sayısı, üretilen asset sayısı. Rapordan + koddan çek.
4. **SAHNE-HAZIRLIĞI:** Hangi odalar/sahneler sunuma hazır (güzel/stabil), hangileri polish gerek? Mevcut 26 captured room'dan en gösterişli 3-4'ü hangisi?
5. **RİSK:** Demo-günü teknik riskleri (assert-spam, build-gap, softlock, eksik-asset) — ne önceden kapatılmalı?

ÇIKTI: file:line + somut sayılar. NET. CODEX_DONE_laurethayday.md.
