ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Demo B-lite test matrisi — FEASIBILITY/REUSE lens: mevcut RIMA test altyapısında neyi yeniden kullanabiliriz, hangi davranış EditMode'da hangisi PlayMode'da test edilebilir, ve iki DungeonGraph isim çakışmasını nasıl çözeriz.

# Görev (ANALYSIS ONLY — no code changes, answer to CODEX_DONE.md)

Demo B-lite sistemi 5 yeni runtime bileşeninden oluşuyor. Bunlara test yazacağız. SEN feasibility/reuse lens'inden cevapla: RIMA'da ZATEN ne var, neyi yeniden kullanmalıyız, ne EditMode ne PlayMode olmalı.

## READ these source files (kendi dosya araçlarınla oku, INLINE edilmedi):
- Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs        (YENİ: RIMA.MapDesigner.Room.Runtime.DungeonGraph — saf C# class)
- Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
- Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs
- Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs
- STAGING/DEMO_ARCHITECTURE_DECISION_2026-06-03.md

## REUSE recon — mevcut test altyapısını incele (pattern çıkar):
- Assets/Tests/EditMode/DungeonGraphTests.cs   (DİKKAT: bu ESKİ RIMA.DungeonGraph MonoBehaviour'unu test ediyor, YENİ class'ı DEĞİL)
- Assets/Tests/EditMode/Room/RoomBankPickTests.cs
- Assets/Tests/EditMode/Room/RoomTemplateWalkableGridTests.cs
- Assets/Tests/PlayMode/RoomFlowTests.cs
- Assets/Tests/PlayMode/Room/RoomBankRuntimeSpawnTests.cs
- Assets/Tests/EditMode/RIMA_EditMode_Tests.asmdef
- Assets/Tests/PlayMode/RIMA.Tests.PlayMode.asmdef

## CEVAPLA (6 alt-soru):
1. **REUSE:** Mevcut EditMode/PlayMode testlerinde RoomTemplateSO / RoomBankSO'yu kodla nasıl kuruyorlar (ScriptableObject.CreateInstance + walkableGrid doldurma)? Yeni testler bu helper/pattern'i nasıl yeniden kullanmalı? Paylaşılan bir test-fixture/base var mı?
2. **EditMode vs PlayMode split:** Her bileşen için hangi davranışlar Unity sahnesi OLMADAN (EditMode, stub/null builder ile) test edilebilir, hangileri PlayMode + _Arena sahnesi GEREKTİRİR? Özellikle: RoomRunDirector.AdvanceTo/CurrentChoices/IsRunComplete navigasyonu sahnesiz test edilebilir mi (builder.Build NRE atar mı, stub'lanabilir mi)?
3. **IsoRoomBuilder testability:** Bir PlayMode testi insan gözü olmadan neyi assert edebilir? (build exception atmıyor, floor hücre sayısı > 0, PlayerSpawnMarker != null, BuildExitDoors döndürdüğü GO listesi sayısı == doorTypes.Count). Bunun için _Arena sahnesinin mi yüklenmesi gerekir yoksa kod-içi Grid/Tilemap kurulabilir mi?
4. **İSİM ÇAKIŞMASI:** İki DungeonGraph var: ESKİ `RIMA.DungeonGraph` (MonoBehaviour, mevcut testli) + YENİ `RIMA.MapDesigner.Room.Runtime.DungeonGraph` (saf class, testsiz). Yeni testler çakışmayı nasıl önler (full namespace + farklı test class adı)? Eski class hâlâ kullanılıyor mu (grep), yoksa deprecate/rename edilmeli mi? (ikincisi AYRI bir karar — sadece risk flag'le).
5. **asmdef:** Room.Runtime tipleri hangi assembly'ye derleniyor (RIMA.Runtime?) ve RIMA.Tests.EditMode bunu zaten reference ediyor mu? Yeni testler mevcut RIMA.Tests.EditMode + RIMA.Tests.PlayMode asmdef'lerine mi girmeli, yeni asmdef gerekir mi?
6. **MİN SET:** En yüksek regresyon-değerli minimal test seti hangisi (over-engineering'den kaçın)? Hangi testler Unity-timing yüzünden flaky-riskli ve basitleştirilmeli/atlanmalı?

Sonucu CODEX_DONE.md'ye yaz. Prior audit'i tekrar üretme.
