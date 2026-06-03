ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA'yı SCENE-başına-harita modelinden DATA-DRIVEN oda modeline (Model B) geçireceğiz. SENDEN İSTENEN = SADECE ANALİZ + MİMARİ ÖNERİ RAPORU (KOD YAZMA, DOSYA DEĞİŞTİRME YOK). Opus senin + 2 Gemini'nin görüşünü sentezleyip karar verecek, sonra ayrı bir dispatch implement edecek.

# Bağlam (doğrulanmış, bu session)
- RIMA = Unity URP 2D, iso-görünümlü top-down roguelite ARPG. PPU64. Depth-sort = Camera custom-axis (0,1,0), tek "Entities" layer. iso floor Tilemap cellSize 0.96 x 0.585.
- CANLI map flow: `Assets/Scripts/Systems/Map/MapFlowManager.cs` singleton (DontDestroyOnLoad, BeforeSceneLoad bootstrap) `Resources.Load("Map/MapList_Act1")` MapListSO'dan RASTGELE bir SAHNE seçer. mapsPerRun=3, 3. clear'da victory (`RoomLoader.RaiseDemoComplete`). Anti-immediate-repeat var. Her harita ayrı baked `.unity` sahnesi (6 sahne: _IsoGame + Map02..Map06), build settings'te.
- Oda-clear zinciri: `RoomClearVictoryTrigger` (moblar olunce kuzey kapi GateBehavior.Unlock + relic spawn) -> `RewardPickup` -> `DraftManager.ShowDraft` (3-kart) -> `ActivateExitDoors` -> `DoorTrigger.TriggerTransition` -> RuntimeRoomManager.Instance==null -> `MapFlowManager.GoToNextMap`.
- 6 harita sekli (bridge/cross/ell/hourglass/donut/elmas) gecen session bir subagent'in TEK SEFERLIK execute_code script'iyle uretildi: floor predicate (hangi hucre zemin) -> otomatik cliff (yonlu SW/SE + iceri-tuck) -> otomatik boundary (DCEL kenar-izleyici EdgeCollider2D) -> sahneyi kaydet. BU GENERATOR HICBIR YERE TOOL OLARAK KAYDEDILMEDI (sadece o calistirmada vardi). Genel/sekil-bagimsiz parcalar (floor-paint, auto-cliff, auto-boundary) KANITLI calisiyor.

# Mevcut (KOPUK / yarim) oda altyapisi — audit et, reuse vs rebuild karari ver
Asagidaki dosyalarin GERCEKTE ne yaptigini, canli akista kullanilip kullanilmadigini, ve data-driven sistemde KURTARILABILIR mi yoksa OLU mu oldugunu degerlendir:
- `Assets/Scripts/RoomPainter/RoomData.cs` (ScriptableObject: floorCells/cliffCells/wallCells/props/portals per-cell + layers)
- `Assets/Scripts/RoomPainter/RoomDataJson.cs` (RoomDataDTO + Write/Read/ReadRoom JSON)  -> DISKTE TEK ODA JSON'U YOK.
- `Assets/Scripts/Core/RoomType.cs` (enum: Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor)
- `Assets/Scripts/Environment/RoomTypeData.cs` (SO: portal-count agirliklari)
- `Assets/Scripts/Map/RoomBuilder.cs` (EDITOR-ONLY, eski 24x18 DUVARLI top-down oda builder — iso degil)
- `Assets/Scripts/Systems/Map/RoomConfig.cs`, `RoomLoader.cs` (prefab-tabanli RoomRegistry.GetRandom + RoomSequenceData — canli MapFlowManager akisindan KULLANILMIYOR gorunuyor; dogrula)
- `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` + `Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs` (elle-cizim tool, yarim)
- `Assets/Scripts/Core/RuntimeRoomManager.cs` ([Obsolete] mi? canli mi?)

# SORULAR (rapor halinde cevapla)
1. AUDIT: yukaridaki her dosya icin: CANLI mi / OLU mu / KURTARILABILIR mi (1 satir + neden). Hangi sistem canli akisin gercek temeli?
2. MIMARI: data-driven oda sistemi icin EN YALIN saglam mimari ne? Sunlari netlestir:
   - Oda tanim formati: ScriptableObject mi, JSON (RoomDataJson) mi, hibrit mi? (degerlendir: elle-yazim, git-diff, runtime yukleme, designer-export, MapFlowManager entegrasyonu)
   - Sekil temsili: parametrik predicate (kompakt, proceduel cesitlilik) mi, per-cell mask (tam kontrol/elle-cizim) mi, hibrit mi? "Mantikli, kare-olmayan, gecisli/akan" odalari OLCEKTE ucuza ureten hangisi?
   - Runtime builder nerede yasamali, hangi mevcut kodu cagirmali (floor-paint/auto-cliff/auto-boundary RUNTIME'da calisabilir mi — su an editor-only mu? eger oyle ise runtime'a tasima maliyeti?)
   - MapFlowManager nasil degisir: sahne-secme -> oda-tipi-secme + tek Arena sahnesi. RunDirector/Act yapisi gerekir mi?
3. RUN YAPISI: "rastgelelik ama mantikli" icin: StS-tarzi tipli-sira (start->combat->combat->elite->reward->...->boss) + her slota tipe-uygun havuzdan rastgele layout mu? Lineer mi, dallanan graph mi? DEMO icin dogru kapsam ne?
4. ODA TIPLERI + BOYUT: demo icin hangi tipler (start/combat/elite/reward/boss + opsiyonel)? Boyut (small/med/large/big-arena) + tip oda tanimina nasil kodlanir? Tatmin edici demo icin tip basina kac layout?
5. COGALTMA: sistem kurulduktan SONRA tek bir yeni oda eklemenin TAM minimal adimi ne olmali? (orn "X klasorune 1 SO yarat: shape+type+size+anchors")
6. REUSE vs REBUILD: net tavsiye + neden. Yarim altyapiyi temizleme/arsivleme onerisi.
7. RISKLER: runtime tilemap paint perf, EdgeCollider runtime, cliff sprite count, save/scene yok artik vs build-settings, vb.

# Cikti
Markdown rapor olarak CODEX_DONE.md'ye yaz. Net, madde madde. KOD YAZMA, DOSYA DEGISTIRME. Sadece audit + mimari oneri + risk.
