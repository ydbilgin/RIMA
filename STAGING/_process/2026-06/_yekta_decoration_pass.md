ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Live odalara DEKORASYON-ONLY late-pass ekle: mevcut prop/composition/autoplacer sistemini kullanarak düz zemine collider'sız görsel dekor saç. FEATURE-FLAG DEFAULT-OFF (demoyu bozamaz). Mevcut 17 Act1 prop ile, SIFIR yeni art. Bu, council kararının #1 ROI adımı.

# OKU (sırayla)
1. `STAGING/TILEMAP_VISUAL_QUALITY_DECISION_2026-06-11.md` (council kararı — özellikle cx FEASIBILITY bölümü, senin önceki audit'in)
2. `STAGING/PROPS_DOORS_PLACEMENT_PLAN_2026-06-11.md` (§6 AI placement, §9 yok; §1 pipeline)
3. Kod: `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (Build:109-116, BuildFloor:283-314, BuildProps:667-718), `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs`, `Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs`, `Assets/Scripts/MapDesigner/Composition/CompositionRoleMapGenerator.cs`, `Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs`, `Assets/Scripts/MapDesigner/Props/PropRegistrySO.cs`

# YAP (cerrahi, min kod)
1. **YENİ dosya** `Assets/Scripts/MapDesigner/Room/Runtime/RoomDecorationPass.cs` (blast-radius'u IsoRoomBuilder'dan ayır):
   - `public static int Apply(RoomTemplateSO template, HashSet<Vector3Int> floorCells, Transform decorContainer, PropRegistrySO registry, int runSeed, float tileSize)` benzeri imza (mevcut tiplere göre uyarla).
   - Akış: `CompositionRoleMapGenerator` ile zone grid → `BridsonPoissonAutoPlacer` ile seeded aday listesi (DecoratedEdge/FocalCluster yoğunluğu; CleanCenter düşük; DoorSafety/Wall=0) → her aday `PropFootprintValidator.Validate` → geçerliler `PropRuntimeSpawner` mantığıyla GameObject (SpriteRenderer + PropSorterRuntime).
   - **DEKORASYON-ONLY:** collider EKLEME (PropColliderAutoBuilder ÇAĞIRMA, blocksWalkable prop'ları decoration olarak SAYMA ya da collider'sız spawn et). WalkableGrid'i DEĞİŞTİRME.
   - **GUARD'lar (HARD):** DoorSafety radius=3 önü, player spawn, enemy spawn, reward alanı = dekor YASAK (CompositionRole + template socket'lerinden). Dekor asla walkable'ı bloklamaz.
   - Deterministik (runSeed) — aynı oda+seed = aynı dekor.
2. **IsoRoomBuilder.cs'e TEK guarded hook** (Build sonunda, BuildProps/BuildBoundary SONRASI):
   - `[SerializeField] private bool enableAutoDecoration = false;` (DEFAULT FALSE — demo davranışı DEĞİŞMEZ)
   - `[SerializeField] private PropRegistrySO decorationRegistry;` (null-safe; null ise pass atla + uyarı yok)
   - `if (enableAutoDecoration && decorationRegistry != null) RoomDecorationPass.Apply(...);`
   - Dekor için ayrı child container oluştur (örn. "Decorations"), ClearPrevious'a ekle (rebuild'de temizlensin).
3. **RoomRunDirector'ın Combat/Elite prop-disable mantığına (315-318) DOKUNMA.** Bu ayrı bir non-blocking pass.

# TEST (EditMode, mevcut Props test paterni)
- `RoomDecorationPassTests.cs`: (a) flag-off davranışı (Apply çağrılmazsa 0 dekor / mevcut testler bozulmaz), (b) Apply geçerli non-blocking dekor üretir + DoorSafety/spawn hücreleri BOŞ kalır, (c) determinizm (aynı seed = aynı sayı/pozisyon), (d) hiçbir dekor walkable'ı bloklamaz.

# KISIT
- COMMIT ETME. Değişiklikleri working tree'de bırak (Claude QC + kullanıcı playtest sonrası commit edilecek).
- Sadece listelenen dosyalar + yeni 2 dosya. İlgisiz refactor yok.
- Unity AÇIKSA `read_console` ile compile 0-error teyit et; değilse C# syntax'ı dikkatli doğrula + raporda "Unity-compile doğrulanMADI" yaz.
- Belirsizlik → BLOCKED yaz, tahmin etme (özellikle PropRuntimeSpawner'ın internal spawn metodunu reuse edebiliyor musun, yoksa mantığı tekrar mı gerek — net değilse sor).
- Sonucu `CODEX_DONE_yekta.md`'ye yaz: değişen/yeni dosyalar, test sonucu, compile durumu, varsayımlar, kalan riskler.
