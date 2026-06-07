# CX TASK — RoomRunDirector skeleton (Demo B-lite, sıra-1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the file listed (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / code / STAGING / memory.

## Amaç
RIMA demo B-lite (karar: `STAGING/DEMO_ARCHITECTURE_DECISION_2026-06-03.md`). Tek `_Arena` sahnesinde, typed bir oda-rotasını `IsoRoomBuilder` ile çizip oyuncuyu ışınlayan **RoomRunDirector** skeleton'ı yaz. Bu sıra-1: SADECE route + build + player teleport + debug-advance. Encounter/reward/portal/preview SONRAKİ sıralarda — onları YAZMA, sadece TODO yorum bırak.

## SCOPE — yalnızca bu 1 dosyayı oluştur
`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` (namespace `RIMA.MapDesigner.Room.Runtime`)
Başka dosyaya DOKUNMA. Derleme TEMİZ (0 error). Editor-only API YOK (saf runtime; ContextMenu OK).

## OKU (doğrulanmış API — bunlara göre yaz)
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` — `public void Build(RoomTemplateSO template)`; `public Transform PlayerSpawnMarker { get; private set; }` (Build sonrası set, null olabilir).
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` — `roomId`, `roomType` (RIMA.RoomType), `bounds`.
- `Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs` — `public RoomTemplateSO Pick(RIMA.RoomType roomType, int seed)` (seeded; null dönebilir). `GetList(RoomType)`.
- `Assets/Scripts/Core/RoomType.cs` — enum { Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor }.

## RoomRunDirector.cs — SPEC
`public sealed class RoomRunDirector : MonoBehaviour`

### Serialized alanlar
- `[SerializeField] private IsoRoomBuilder builder;`
- `[SerializeField] private RoomBankSO roomBank;`
- `[SerializeField] private Transform player;`         // ışınlanacak oyuncu (null ise teleport atla + LogWarning bir kez)
- `[SerializeField] private RoomTemplateSO fallbackTemplate;`  // bank null/boş dönerse
- `[SerializeField] private List<RIMA.RoomType> route = new List<RIMA.RoomType>{ RIMA.RoomType.Combat, RIMA.RoomType.Combat, RIMA.RoomType.Elite, RIMA.RoomType.Combat, RIMA.RoomType.Boss };`
- `[SerializeField] private int runSeed = 12345;`
- `[SerializeField] private bool buildOnStart = true;`

### Public durum
- `public int RouteIndex { get; private set; }`
- `public RoomTemplateSO CurrentTemplate { get; private set; }`
- `public RIMA.RoomType CurrentRoomType => (route != null && RouteIndex >= 0 && RouteIndex < route.Count) ? route[RouteIndex] : RIMA.RoomType.Combat;`
- `public bool IsRunComplete => route == null || RouteIndex >= route.Count;`

### Metodlar
- `void Start()` → buildOnStart ise `BeginRun()`.
- `public void BeginRun()` → RouteIndex=0; `BuildCurrentRoom();`
- `public void BuildCurrentRoom()`:
  - guard: builder null → LogError + return. route boş → LogWarning + return. IsRunComplete → Log("[RoomRunDirector] Run complete.") + return.
  - template seç: `RoomTemplateSO t = (roomBank != null) ? roomBank.Pick(CurrentRoomType, runSeed + RouteIndex) : null;` if t==null → t=fallbackTemplate; if t==null → LogError("no template for {CurrentRoomType}") + return.
  - `CurrentTemplate = t;`
  - `builder.Build(t);`
  - player teleport: if player != null && builder.PlayerSpawnMarker != null → `player.position = builder.PlayerSpawnMarker.position;` else if player==null → LogWarning bir kez.
  - `Debug.Log($"[RoomRunDirector] Built room {RouteIndex+1}/{route.Count} type={CurrentRoomType} id={t.roomId}");`
  - // TODO sıra-3: StartEncounter(t); sıra-4: clear→reward→draft; sıra-5: typed portals; sıra-6: preview islands
- `public void AdvanceRoom()`:  // sıra-1 debug; sıra-4'te clear-flow tetikleyecek
  - if IsRunComplete → Log("Run already complete") + return.
  - RouteIndex++; if IsRunComplete → Log("[RoomRunDirector] RUN COMPLETE (reached end).") else BuildCurrentRoom().
- `[ContextMenu("Advance Room")] private void DebugAdvance() => AdvanceRoom();`
- `void Update()` → debug: `if (Input.GetKeyDown(KeyCode.N)) AdvanceRoom();`  // sıra-4'te kaldırılacak, şimdilik test için

using: `System.Collections.Generic`, `UnityEngine`. (RIMA tipleri tam-nitelikli ya da `using RIMA;`/`using RIMA.MapDesigner.Room.Data;`).

Min kod, spekülasyon yok. Sonuç raporunda: oluşturulan public API + varsayımlar + "compile-ready" gerekçesi. API uyuşmazsa DUR, BLOCKED yaz.
