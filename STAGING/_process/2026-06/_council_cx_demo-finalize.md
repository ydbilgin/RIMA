ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA demo basit oynanabilir vertical slice — iki tekrarlayan bug'in KESIN teshisi + cleanup eksikleri + YAZILABILIR test otomasyonu (kod-tarafi feasibility, cx tek tek baksin). ANALIZ ONLY, kod degistirme. Sonucu CODEX_DONE.md'ye yaz. Onceki audit'i TEKRARLAMA.

LENS: feasibility / RIMA'da-ne-zaten-var / reuse-vs-build + KOD-TARAFI TEST OTOMASYONU feasibility (senin asil gucun bu).

## Analiz konulari (read-only, dosya:satir kanit)

1) MOB GORUNMEME/SIYAH RENDER: Bazi moblar gorunmuyor/siyah render oluyor. Mob prefablari (Assets/Prefabs/Enemies/) 12 Act-1 sprite ile wire edildi. EncounterController/EncounterBank hangi prefablari spawn ediyor + fallback wave hangi prefabi kullaniyor (RoomRunDirector.CreateDefaultCombatFallbackWave / ResolveDefaultEnemyPrefab)? Wire edilen prefab seti ile SPAWN-EDILEBILEN set arasinda bosluk var mi (wire'siz/PlaceholderSprite acik prefab spawn olabiliyor mu)? Siyah render = eksik sprite mi, sorting/material mi? Kok-neden + dosya:satir.

2) SKILL-ICON BOS: DraftManager.ClassKits sadece Warblade+Elementalist iceriyor (DraftManager.cs:70). GetOpeningKitDraft: `if (!ClassKits.TryGetValue(primary, out kit)) return;` (DraftManager.cs:263,293) -> kit'siz sinif acilis draft'i almiyor -> bos bar. Dogrula: hangi siniflar secilebilir (ClassUnlockPolicy: Warblade+Elementalist default; chamber kilitli sinif sectirebiliyor mu?), kit'siz sinif secilirse ne olur, baska kok-neden var mi (SkillBarOffset, ikon yukleme yolu, oda-clear draft kit'siz sinifta calisiyor mu).

3) 8-P0 CLEANUP eksik/risk: DraftManager depth->RoomRunDirector.CurrentNodeId+1, Build Settings tek-yol, MapFragment prefab repoint, kapi konsolidasyonu, MainMenuScreen.AutoInit disable, boss->victory, opening-draft timeout fallback, HUD/SkillBar retry. Bu listede EKSIK kalan cakisma/risk var mi.

4) TEST OTOMASYONU (EN ONEMLI, tek tek bak): Bu regresyonlari otomatik yakalayacak Unity EditMode/PlayMode testleri YAZILABILIR MI? Her biri icin: test adi -> ne assert eder -> EditMode/PlayMode -> yazilabilirlik (kolay/orta/zor) -> hangi mevcut test altyapisina dayanir (Assets/Tests/). Ornek hipotezler (dogrula/genislet): (i) her spawn-edilebilir mob prefabinin SpriteRenderer.sprite!=null + PlaceholderSprite disabled [EditMode prefab taramasi], (ii) her oynanabilir sinifin ClassKit'i var [EditMode], (iii) depth RoomRunDirector'dan ilerliyor draft tier'i etkiliyor [PlayMode], (iv) combat clear -> kapi acilir softlock yok [PlayMode], (v) Build Settings sadece kanonik 3 sahne [EditMode], (vi) boss clear -> Victory [PlayMode].

CIKTI (CODEX_DONE.md): (a) kesin teshis tablosu (bug->kok-neden->dosya:satir->fix), (b) cleanup eksikleri, (c) yazilabilir test otomasyonu listesi.
