# CX TASK — DungeonGraph (branching run-graph) + RoomRunDirector node-upgrade + RunMapOverlay branch render

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the 3 files listed (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / code / STAGING.

## Amaç
RIMA demo (B-lite). Şu an `RoomRunDirector` LİNEER bir route tutuyor. Kullanıcı **dallı (branching) run-path** istiyor: her oda'da bir sonraki seçenek 1/2/3 olabilir → oda sonunda 1/2/3 KAPI çıkar. M-haritası bu dallanmayı gösterir. Bu task = (A) dallı graph data+generator, (B) RoomRunDirector'ı node-bazlı yap, (C) RunMapOverlay'i dallı render et. SADECE C# yaz, Unity sahne/asset DOKUNMA. Derleme TEMİZ.

## SCOPE — bu 3 dosya
1. YENİ: `Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs`
2. DEĞİŞTİR: `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` (mevcut — OKU)
3. DEĞİŞTİR: `Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs` (mevcut — OKU)

Editor-only API YOK (saf runtime; ContextMenu OK). Mevcut API'ler:
- `IsoRoomBuilder.Build(RoomTemplateSO)` + `Transform PlayerSpawnMarker {get;}`
- `RoomBankSO.Pick(RIMA.RoomType, int seed)` → RoomTemplateSO (null olabilir)
- `RIMA.RoomType` = { Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor }
- `System.Random` ile seeded üret (Math.random/Unity Random YASAK değil ama deterministik için System.Random seed kullan).

---

## A. DungeonGraph.cs — SPEC
namespace `RIMA.MapDesigner.Room.Runtime`
```csharp
public sealed class DungeonNode {
    public int id;
    public int depth;
    public RIMA.RoomType roomType;
    public System.Collections.Generic.List<int> childIds = new();  // bir sonraki depth'teki node id'leri
}
public sealed class DungeonGraph {
    public System.Collections.Generic.List<DungeonNode> nodes = new();
    public int startId;          // depth 0 node
    public int maxDepth;
    public DungeonNode Get(int id);                       // id->node
    public System.Collections.Generic.List<DungeonNode> ChildrenOf(int id);   // node.childIds -> node list
    public System.Collections.Generic.List<DungeonNode> NodesAtDepth(int depth);
    public static DungeonGraph Generate(int seed, int depthCount);  // depthCount>=2
}
```
**Generate kuralı (StS-lite, deterministik System.Random(seed)):**
- depth 0: TEK node, roomType=Combat (start).
- depth `depthCount-1`: TEK node, roomType=Boss.
- ara depth'ler (1..depthCount-2): rastgele **2 veya 3** node.
- roomType ara node'larda ağırlıklı: Combat %55, Elite %20, Chest %15, Event %10 (RoomBankSO yalnız Combat/Elite/Boss/Merchant/Event çözer; Chest/Event çözülmezse director fallback kullanır — sen sadece type ata).
- **Kenarlar:** her node (depth d, d<depthCount-1) → bir sonraki depth'teki node'lara 1..min(3, nextCount) çocuk bağla. KURAL: (1) her node en az 1 çocuk; (2) bir sonraki depth'teki HER node en az 1 parent (erişilemez node olmasın); (3) çocuklar rastgele ama bitişik-lane tercih et (planar görünsün — opsiyonel). Boss'a giden son-ara-depth node'larının hepsi boss'a bağlanır.
- id'ler 0'dan artan; startId=0.

## B. RoomRunDirector.cs — node-bazlı upgrade
Mevcut serialized koru: `builder, roomBank, player, fallbackTemplate, runSeed, buildOnStart`. 
KALDIR: `route` (List<RoomType>) ve onunla ilgili `Route`/`AdvanceRoom`. EKLE: `[SerializeField] private int depthCount = 5;`
Yeni durum/akış:
- `private DungeonGraph graph;  public DungeonGraph Graph => graph;`
- `public int CurrentNodeId { get; private set; }`
- `public DungeonNode CurrentNode => graph?.Get(CurrentNodeId);`
- `public RIMA.RoomType CurrentRoomType => CurrentNode != null ? CurrentNode.roomType : RIMA.RoomType.Combat;`
- `public System.Collections.Generic.List<DungeonNode> CurrentChoices => graph != null ? graph.ChildrenOf(CurrentNodeId) : new();`  // oda-sonu kapıları bunlar
- `public bool IsRunComplete => CurrentNode == null || CurrentChoices.Count == 0;`  // boss = çocuğu yok
- `public RoomTemplateSO CurrentTemplate { get; private set; }`
- `Start()` → buildOnStart ise `BeginRun()`.
- `public void BeginRun()` → `graph = DungeonGraph.Generate(runSeed, depthCount); CurrentNodeId = graph.startId; BuildCurrentRoom();`
- `public void BuildCurrentRoom()`: guard builder/graph null. template = `roomBank?.Pick(CurrentRoomType, runSeed + CurrentNodeId)` ?? fallbackTemplate; null ise LogError+return. `CurrentTemplate=template; builder.Build(template);` player teleport (PlayerSpawnMarker'a, mevcut mantık). Debug.Log node id+depth+type+choices.Count. // TODO: encounter start + reward + door illuminate (sonraki adım, Opus)
- `public void AdvanceTo(int choiceIndex)`: guard IsRunComplete→Log "run complete". choices=CurrentChoices; index sınır kontrol; `CurrentNodeId = choices[choiceIndex].id; BuildCurrentRoom();`
- `[ContextMenu("Advance First Choice")] private void DebugAdvance(){ if(!IsRunComplete) AdvanceTo(0); }`
- Update() / Input YOK (yeni Input System çakışıyor — KOYMA).

## C. RunMapOverlay.cs — dallı render
Mevcut OnGUI overlay (M ile toggle, Event.current ile — KORU, Input System'e dokunma). `director.Route` yerine `director.Graph` + `director.CurrentNodeId` kullan.
Render: graph'ı **depth bazlı** çiz — her depth bir SATIR (boss üstte, start altta VEYA start altta boss üstte; dikey). Her depth'teki node'lar o satırda yan yana kutular. Kenarları (parent→child) ince çizgi ile bağla. Node kutusu rengi roomType'a göre (mevcut ColorFor reuse). CurrentNodeId = cyan border + parlak. Ziyaret edilemeyen/ileri node'lar normal. Başlık "RUN PATH" + "M ile kapat". Konumlandırma: ekran ortası, depth sayısına göre dikey dağıt, her satırdaki node'ları yatay ortala. Çizgiler için mevcut DrawRect/pixel kullan (köşegen çizgi gerekmez; parent-merkez ile child-merkez arası kısa dik/eğik segment yeterli — basit tut: parent alt-merkezden child üst-merkeze düz çizgi yerine, iki nokta arası 1-2 segment).

## Doğrulama
`dotnet build` ile derlemeyi doğrula (RIMA.Runtime.csproj). Sonuç raporunda: DungeonGraph public API, RoomRunDirector yeni public API, ve generate edilen örnek graph'ın (seed=12345, depthCount=5) node/edge özeti (kaç node, her depth kaç, örnek choices). API uyuşmazsa DUR, BLOCKED yaz.
