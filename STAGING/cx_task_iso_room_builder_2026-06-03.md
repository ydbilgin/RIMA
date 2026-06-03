# CX TASK — P2 IsoRoomBuilder (runtime, RIMA Room System Model B)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the files listed below (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç (purpose)
RIMA artık odaları SAHNE değil, DATA olarak tutuyor (mevcut `RoomTemplateSO`). Eksik olan tek şey: bu data'yı RIMA'nın **izometrik yüzen-ada** görünümünde (0.96×0.585 elmas hücre + yönlü auto-cliff + sınır collider) çizen bir RUNTIME builder. Bu task = o builder'ı (`IsoRoomBuilder`) + küçük bir test sürücüsü yazmak. **SADECE C# yaz — sahne/asset OLUŞTURMA, MCP kullanma.** Sahne kurulumu + play-test ayrı adımda (Opus) yapılacak.

## SCOPE — yalnızca bu 2 dosyayı oluştur
1. `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`
2. `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilderTester.cs`

Başka hiçbir dosyayı değiştirme. Derleme TEMİZ olmalı (0 error). Editor-only API (UnityEditor / AssetDatabase / PrefabUtility) KULLANMA — saf runtime.

---

## MEVCUT API'LER (doğrulanmış — bunlara göre yaz, kendin keşfetme)

### RoomTemplateSO — `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
namespace `RIMA.MapDesigner.Room.Data`
```csharp
public string roomId, biomeId;
public RIMA.RoomType roomType;
public RectInt bounds;                          // tile-space; origin = bounds.xMin/yMin
public List<DoorSocket> doorSockets;
public PlayerSpawnSocket playerSpawn;
public List<EnemySpawnSocket> enemySpawnSockets;
public CameraBounds cameraBounds;               // .tileRect (RectInt)
public bool[] walkableGrid;                     // row-major; index = (ly*bounds.width)+lx
public bool IsWalkable(Vector2Int tilePos);     // GLOBAL tile coords; does bounds-origin math; null grid => full bounds walkable
```
Socket tipleri (hepsi `RIMA.MapDesigner.Room.Data`, Vector2Int = GLOBAL tile coords):
```csharp
class DoorSocket      { string socketId; Vector2Int position; RIMA.DoorDirection direction; int widthInTiles=2; bool isExit=true; }
class PlayerSpawnSocket{ string socketId; Vector2Int position; RIMA.DoorDirection facing=South; }
class EnemySpawnSocket { string socketId; Vector2Int position; string tierHint="standard"; float avoidRadius=1.5f; }
```
`RIMA.DoorDirection` = { North, South, East, West } (`Assets/Scripts/Core/DoorTrigger.cs`).

### RoomCliffSolver — `Assets/Scripts/RoomPainter/RoomCliffSolver.cs`  (runtime-safe, NO editor API)
namespace `RIMA.RoomPainter`
```csharp
public static HashSet<Vector3Int> Solve(IEnumerable<Vector3Int> floorCellsSource, int southClearCells = 5);
```
Girdi = floor hücreleri (Vector3Int, z=0). Çıktı = cliff KONULACAK hücre seti. SADECE hangi hücreler-cliff'i söyler; YÖN/sprite yerleştirmez. southClearCells=5 default kullan.

### Iso cell→world — `Assets/Scripts/Systems/Map/RoomConfig.cs`
```csharp
public static readonly Vector3 IsoCellSize = new Vector3(0.96f, 0.585f, 1f);
// Grid: cellLayout = Isometric, cellSwizzle = XYZ
```
Dünya konumu: **`grid.CellToWorld(new Vector3Int(x,y,0))`** kullan (Grid iso ayarlı olduğu varsayımıyla — sahne adımında öyle kurulacak). Doğrulanmış komşu vektörleri (iso): South=(-1,-1,0), SouthEast=(0,-1,0), SouthWest=(-1,0,0), North=(1,1,0), East=(1,-1,0), West=(-1,1,0).

### RoomType — `Assets/Scripts/Core/RoomType.cs`
{ Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor }

---

## IsoRoomBuilder.cs — SPEC

`public sealed class IsoRoomBuilder : MonoBehaviour` (namespace `RIMA.MapDesigner.Room.Runtime`)

### Serialized inspector alanları (hepsi public veya [SerializeField]):
- `Grid grid;`  (iso Grid; null ise GetComponentInParent / FindObjectOfType<Grid>())
- `Tilemap groundTilemap;`  (floor çizimi)
- `Tilemap collisionTilemap;` (sınır collider; ayrı tilemap)
- `Transform cliffContainer;` (cliff SpriteRenderer'ları buraya; null ise child "CliffSprites" yarat)
- `Transform markerContainer;` (spawn/door marker'ları; null ise child "RoomMarkers" yarat)
- `TileBase floorTile;`
- `TileBase collisionTile;`  (görünmez/herhangi tile — sadece collider için)
- `Sprite cliffSouth, cliffSouthEast, cliffSouthWest;`
- `Vector2 tuckSouth = new Vector2(0f, 0.29f);`
- `Vector2 tuckSouthEast = new Vector2(-0.48f, 0.29f);`
- `Vector2 tuckSouthWest = new Vector2(0.48f, 0.29f);`
- `string cliffSortingLayer = "Floor";`
- `int cliffSortOrderBase = -30;`
- `float cliffSortYSpan = 20f;`   // order = cliffSortOrderBase + round(cliffSortYSpan - worldY)
- `int cliffPixelsPerUnit = 64;`
- `int southClearCells = 5;`
- `Color playerMarkerColor` / `enemyMarkerColor` (opsiyonel gizmo amaçlı — gerekmezse atla)

### `public void Build(RoomTemplateSO template)`
Adımlar (her biri ayrı private metoda bölünmeli; cliff'i izole tut ki sonra tune edelim):
1. **Guard:** template null veya bounds boş → `Debug.LogWarning` + return. Grid/tilemap null ise resolve et (yoksa create child değil — eksikse LogError + return; bunlar sahnede atanmış olacak).
2. **Clear previous:** `groundTilemap.ClearAllTiles()`, `collisionTilemap.ClearAllTiles()`, cliffContainer & markerContainer altındaki TÜM child'ları Destroy (runtime: `Destroy`). Container null ise yarat.
3. **Floor + floorCells topla:**
   - `var floorCells = new HashSet<Vector3Int>();`
   - `for y in [bounds.yMin, bounds.yMax): for x in [bounds.xMin, bounds.xMax):` if `template.IsWalkable(new Vector2Int(x,y))` → cell=(x,y,0); `groundTilemap.SetTile(cell, floorTile)`; `floorCells.Add(cell)`.
   - (SetTilesBlock ile batch yapabilirsen yap; gerekmezse SetTile yeterli.)
4. **Cliffs** (`BuildCliffs(floorCells)`):
   - `var cliffCells = RoomCliffSolver.Solve(floorCells, southClearCells);`
   - foreach cliff cell: yön sınıflandır →
     - `bool swVoid = !floorCells.Contains(cell + new Vector3Int(-1,0,0));`  // SW komşu boş?
     - `bool seVoid = !floorCells.Contains(cell + new Vector3Int(0,-1,0));`  // SE komşu boş?
     - yön: `(swVoid && seVoid)` → South; else if swVoid → SouthWest; else if seVoid → SouthEast; else → South.
   - sprite + tuck seç (South→cliffSouth/tuckSouth, SouthEast→cliffSouthEast/tuckSouthEast, SouthWest→cliffSouthWest/tuckSouthWest). Sprite null ise o cliff'i atla (LogWarning bir kez).
   - GameObject yarat (cliffContainer child), `SpriteRenderer` ekle: sprite, `sortingLayerName = cliffSortingLayer`.
   - world = `grid.CellToWorld(cell)`; pozisyon = `world + (Vector3)tuck`;
   - `sr.sortingOrder = cliffSortOrderBase + Mathf.RoundToInt(cliffSortYSpan - pos.y);`
   - (PPU sprite import'ta zaten ayarlı; kodda PPU set etme.)
5. **Boundary** (`BuildBoundary(template, floorCells)`):
   - Void-ring yaklaşımı (hole-aware): bounds'u 1 hücre genişlet; bu genişletilmiş aralıkta her cell C için: `if (!template.IsWalkable(C2D))` VE 8-komşusundan en az biri walkable ise → `collisionTilemap.SetTile(C, collisionTile)`. (8-komşu: iso 8 vektör.) Bu, ada çevresine + hole çevresine 1-hücre kalın bariyer çizer.
   - collisionTilemap GameObject'inde garanti et (yoksa AddComponent): `TilemapCollider2D` (`usedByComposite=true`), `Rigidbody2D` (`bodyType=Static`), `CompositeCollider2D` (`geometryType=Polygons`, isTrigger=false). collisionTilemap GameObject layer = "Default" (Player Default ile çarpışır). TilemapRenderer varsa `enabled=false` (görünmez bariyer).
6. **Markers** (`BuildMarkers(template)`):
   - PlayerSpawn: `markerContainer` altında empty GO "PlayerSpawn", pozisyon `grid.CellToWorld(...(playerSpawn.position))`.
   - Her EnemySpawnSocket: empty GO "EnemySpawn_<socketId|index>", pozisyon CellToWorld(position).
   - Her DoorSocket: empty GO "Door_<direction>_<socketId>", pozisyon CellToWorld(position). DoorSocket alanlarını (direction, isExit, widthInTiles) GO adına yansıt veya basit bir küçük component'e yazma — ama YENİ component sınıfı YARATMA; sadece adlandır + Transform konumla (gerçek DoorTrigger wiring = P5, bu task DEĞİL).
   - Marker'lar görünür olması için küçük gizmo gerekmez; boş Transform yeterli.
7. Build sonunda `Debug.Log($"[IsoRoomBuilder] Built {template.roomId}: {floorCells.Count} floor, {cliffCells.Count} cliff cells.")`.

Public yardımcı: `public HashSet<Vector3Int> LastFloorCells { get; private set; }` (opsiyonel, test/verify için).

## IsoRoomBuilderTester.cs — SPEC
`public sealed class IsoRoomBuilderTester : MonoBehaviour` (aynı namespace)
- `[SerializeField] IsoRoomBuilder builder;`
- `[SerializeField] RoomTemplateSO template;`
- `[SerializeField] bool buildOnStart = true;`
- `void Start()` → buildOnStart && builder && template ise `builder.Build(template);`
- `[ContextMenu("Rebuild")] void Rebuild()` → builder.Build(template).
Min kod, spekülasyon yok.

---

## Doğrulama (senin yapacağın)
- `dotnet`/Unity yok → derlemeyi SEN doğrulayamazsın; ama kodun derlenebilir olduğundan emin ol (using'ler doğru, tip imzaları yukarıdaki API'lerle birebir, RoomCliffSolver.Solve imzası `IEnumerable<Vector3Int>`/`HashSet<Vector3Int>`).
- using listesi: `UnityEngine`, `UnityEngine.Tilemaps`, `System.Collections.Generic`, `RIMA`, `RIMA.MapDesigner.Room.Data`, `RIMA.RoomPainter`.
- Sonuç raporunda yaz: oluşturulan public API (Build imzası, serialized alanlar listesi), varsayımlar, ve "compile-ready" gerekçesi. Eğer yukarıdaki API'lerden biri uyuşmazsa DUR ve BLOCKED yaz — uydurma.
