# AX (Gemini) REVIEW — EXEC1 (menu consolidation) + EXEC2 (asset pack + registry)

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"`

## Rolün
cx kod yazdı, sen (ax/Gemini) + Opus reviewer'sınız. Opus zaten compile-clean + registry-bake (floor=16/cliff=13/prop=18/light=1/portal=0) doğruladı. Sen İKİNCİ GÖZ olarak SADECE şu riskleri ara (kod oku, çalıştırma):

### EXEC1 — menü konsolidasyonu (sadece MenuItem string'leri değişti)
Dosyalar: `git diff --stat` ile gör. Riskler:
1. Kaçırılan ExecuteMenuItem/test literal'i var mı? `grep -rn 'ExecuteMenuItem\|"RIMA/' Assets --include=*.cs | grep -v _archive` ile çapraz-kontrol. (Opus 2 buldu: LiveToolPaletteWindow:229 + SampleRoomLibraryGeneratorTests:12/76 — güncellenmiş mi doğrula.)
2. Validate overload'lar (DungeonSetup, CombatTestSetup) ana giriş ile AYNI path mi?
3. İki MenuItem aynı path'e çakışıyor mu (Unity duplicate menu warning verir)?

### EXEC2 — asset pack + registry baker
Dosyalar: `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs` + `Assets/Scripts/RoomPainter/AssetPackSO.cs`. Riskler:
1. Tag-öncelik sırası (floor>cliff>portal>light>prop>decal) bir asset'i YANLIŞ tag'e mi düşürüyor? Özellikle: `decor_brazier_lit`→light (kasıtlı), ama `decor_brazier_unlit`→? `pl_floor_solid`→floor mu prop mu? `cliff_cyan_glow`/`corner_fade`/`edge_ao_rim` (KitB_Cliff'te ama cliff-face değil)→ yanlışlıkla cliff mi oluyor (cx "3 misc" dedi, doğru mu)?
2. AssetPackSO bake-bridge mevcut root-scan'i bozuyor mu (dedup GUID doğru mu)?
3. AssetPackSO runtime-safe mi (UnityEditor using YOK, RIMA.Runtime asmdef'te derlenir mi)?

## Çıktı
Her bulgu: dosya:satır + SEVERITY (BLOCKER/SHOULD/NIT) + önerilen fix. Temizse "PASS — no blocking issues". Kısa tut. AGY_DONE_<account>.md'ye yazılır.
