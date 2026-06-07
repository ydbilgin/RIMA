ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Demo B-lite sistemine 10 NUnit testi yaz (3 dosya). Test matrisi council ile kararlaştırıldı (STAGING/DEMO_TEST_MATRIX_DECISION_2026-06-03.md). SADECE test dosyaları + .meta'sız (Unity kendi üretir) — runtime/gameplay koduna DOKUNMA. Unity action YOK, sadece .cs dosyaları diske yaz.

# Önce OKU (doğrulama için, çoğu zaten audit edildi):
- Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs
- Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
- Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs
- Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs  (alan tipleri: playerSpawn=PlayerSpawnSocket, doorSockets=List<DoorSocket>, enemySpawnSockets=List<EnemySpawnSocket>, walkableGrid=bool[], bounds=RectInt, roomType, roomId)
- Assets/Scripts/MapDesigner/Room/Data/PlayerSpawnSocket.cs  (public Vector2Int position)
- Assets/Tests/EditMode/Room/RoomBankPickTests.cs  (REUSE pattern referansı)
- Assets/Tests/EditMode/RIMA_EditMode_Tests.asmdef + Assets/Tests/PlayMode/RIMA.Tests.PlayMode.asmdef  (ikisi de RIMA.Runtime reference ediyor — yeni asmdef GEREKMEZ)

# DOĞRULANMIŞ API GERÇEKLERİ (bunlara güven, ama OKU ile teyit et)
- YENİ class: `RIMA.MapDesigner.Room.Runtime.DungeonGraph` (saf C# class, MonoBehaviour DEĞİL). Statik factory: `DungeonGraph.Generate(int seed, int depthCount)`. Alanlar: `List<DungeonNode> nodes`, `int startId`, `int maxDepth`. Metotlar: `Get(int id)`, `ChildrenOf(int id)→List<DungeonNode>`, `NodesAtDepth(int depth)→List<DungeonNode>`. `DungeonNode{ int id; int depth; RIMA.RoomType roomType; List<int> childIds; }`.
  - depthCount<2 → 2'ye clamp; startId=0; maxDepth=depthCount-1; depth0=1 node Combat; son depth=1 node Boss childIds boş; ara depth 2-3 node; her parent 1-3 çocuk; orphan-fix var.
- `RoomRunDirector` (MonoBehaviour). Public: `BeginRun()`, `BuildCurrentRoom()`, `AdvanceTo(int)`, prop'lar `Graph`, `CurrentNodeId`, `CurrentNode`, `CurrentRoomType`, `CurrentChoices` (=ChildrenOf(CurrentNodeId)), `IsRunComplete` (CurrentNode==null||CurrentChoices.Count==0), `CurrentTemplate`. Private serialized: builder, roomBank, player, fallbackTemplate, runSeed=12345, buildOnStart=true, depthCount=5.
  - `builder==null` iken BuildCurrentRoom: `Debug.LogError("[RoomRunDirector] Missing IsoRoomBuilder reference.")` atıp döner AMA CurrentNodeId ÖNCEDEN set edilir (BeginRun graph üretir+startId set eder; AdvanceTo CurrentNodeId'yi set EDIP sonra BuildCurrentRoom çağırır). Yani navigasyon scene-siz test edilebilir; sadece LogError gürültüsü var.
  - AdvanceTo geçersiz index → `Debug.LogWarning(...)`, no-op. IsRunComplete iken AdvanceTo → `Debug.Log("run complete")`, no-op.
- `IsoRoomBuilder` (sealed MonoBehaviour). Public: `Build(RoomTemplateSO)`, `BuildExitDoors(IReadOnlyList<RIMA.RoomType>)→List<GameObject>`, props `LastFloorCells (HashSet<Vector3Int>)`, `PlayerSpawnMarker (Transform)`.
  - Private [SerializeField] alanlar (reflection ile enjekte): `grid (Grid)`, `groundTilemap (Tilemap)`, `collisionTilemap (Tilemap)`, `floorTile (TileBase)`, `collisionTile (TileBase)`, `gateNorthSprite (Sprite)`. (cliff/rune sprite'ları opsiyonel — atanmazsa uyarı, test için gerekmez.)
  - Build: template null→warn+return; bounds w/h<=0→warn+return; grid/groundTilemap/collisionTilemap/floorTile/collisionTile'dan biri yoksa→LogError+return. Sonra floor (walkable hücreler + blocking-prop footprint) groundTilemap'e SetTile; LastFloorCells doldurulur; PlayerSpawnMarker = playerSpawn varsa marker.
  - BuildExitDoors: doorTypes boş VEYA gateNorthSprite null VEYA LastFloorCells boş VEYA grid null → boş liste döner. Yoksa her doorType için 1 GameObject `ExitDoor_{i}_{doorType}` yaratır (i=0-based, doorType=RoomType.ToString()), liste döner. count == doorTypes.Count.

# YAZILACAK 3 DOSYA

## DOSYA 1 — Assets/Tests/EditMode/Room/RoomRuntimeDungeonGraphTests.cs
- namespace `RIMA.Tests.Room`
- ÇAKIŞMA ÖNLEME (ZORUNLU): dosyanın başında alias kullan:
  `using RuntimeDungeonGraph = RIMA.MapDesigner.Room.Runtime.DungeonGraph;`
  `using RuntimeDungeonNode = RIMA.MapDesigner.Room.Runtime.DungeonNode;`
  `using RIMA;` EKLEME (eski RIMA.DungeonGraph ile çakışır). RoomType'a `RIMA.RoomType` ile tam-nitelikli eriş.
- Testler (NUnit [Test]):
  1. `Generate_SameSeed_ProducesIdenticalGraph`: g1=Generate(123,5), g2=Generate(123,5). nodes.Count eşit; her i için id/depth/roomType eşit ve childIds listesi eşit (sıra dahil).
  2. `Generate_StructureInvariants_HoldForManySeeds`: foreach seed in 0..49, foreach depthCount in {2,3,5,8}: g=Generate(seed,depthCount). Assert: g.startId==0; g.maxDepth==depthCount-1; NodesAtDepth(0).Count==1 && roomType==Combat; NodesAtDepth(maxDepth).Count==1 && roomType==Boss && childIds.Count==0; her node depth>0 için en az 1 parent (tüm node'ların childIds'lerinden gelen id-set'i oluştur, her depth>0 node.id bu set'te olmalı); reachability: startId'den BFS/DFS ile gezilen distinct node sayısı == nodes.Count; her non-son-depth node childIds.Count 1..3 arası. Hata mesajında seed+depthCount yaz.
  3. `Generate_DepthCountBelowTwo_ClampsToTwo`: g=Generate(7,1). g.maxDepth==1; en yüksek depth==1; depth0 Combat; depth1 Boss.
  4. `Generate_ZeroAndNegativeSeed_DoesNotThrow`: Assert.DoesNotThrow(()=>Generate(0,5)); Assert.DoesNotThrow(()=>Generate(-99,5)); ikisinin de nodes.Count>0.

## DOSYA 2 — Assets/Tests/EditMode/Room/RoomRunDirectorTests.cs
- namespace `RIMA.Tests.Room`. `using UnityEngine; using NUnit.Framework; using UnityEngine.TestTools; using RIMA.MapDesigner.Room.Runtime;` (RoomRunDirector buradan). RoomType=`RIMA.RoomType`.
- [SetUp]: `LogAssert.ignoreFailingMessages = true;` (null-builder LogError'ları testi düşürmesin). GameObject yarat + RoomRunDirector AddComponent. [TearDown]: GO'yu DestroyImmediate.
- DİKKAT: EditMode'da AddComponent Start()'ı ÇAĞIRMAZ → BeginRun()'ı elle çağır.
- Testler:
  5. `BeginRun_WithMissingBuilder_GeneratesNavigableGraph`: director.BeginRun(); Assert director.Graph!=null; director.CurrentNodeId==director.Graph.startId; director.CurrentChoices.Count>0; director.IsRunComplete==false.
  6. `AdvanceTo_ValidChoice_MovesToChild`: BeginRun(); int childId=director.CurrentChoices[0].id; director.AdvanceTo(0); Assert director.CurrentNodeId==childId.
  7. `AdvanceTo_InvalidIndex_IsNoOp`: BeginRun(); int before=director.CurrentNodeId; director.AdvanceTo(-1); Assert.AreEqual(before,director.CurrentNodeId); director.AdvanceTo(999); Assert.AreEqual(before,director.CurrentNodeId).
  8. `IsRunComplete_TrueAtBoss`: BeginRun(); int guard=0; while(!director.IsRunComplete && guard++<20){ director.AdvanceTo(0); } Assert director.IsRunComplete; director.CurrentNode!=null; director.CurrentRoomType==RIMA.RoomType.Boss; director.CurrentChoices.Count==0.

## DOSYA 3 — Assets/Tests/PlayMode/Room/IsoRoomBuilderTests.cs
- namespace `RIMA.Tests.Room`. `using System.Reflection; using System.Collections.Generic; using UnityEngine; using UnityEngine.Tilemaps; using NUnit.Framework; using RIMA.MapDesigner.Room.Runtime; using RIMA.MapDesigner.Room.Data;` RoomType=`RIMA.RoomType`.
- Plain [Test] (Build senkron, frame-wait yok). PlayMode assembly'de [Test] play-mode'da koşar.
- Private alan enjeksiyon helper'ı: `static void SetPrivate(object o, string field, object val){ o.GetType().GetField(field, BindingFlags.NonPublic|BindingFlags.Instance).SetValue(o,val); }`
- Test-rig kurulum helper'ı (her testte çağır, oluşturulanları List<Object>'e ekle, TearDown'da DestroyImmediate):
  - Grid GO (`new GameObject("TestGrid").AddComponent<Grid>()`), cellLayout iso şart değil (testler GetCellCenterWorld'a güvenir, default rectangular yeter).
  - groundTilemap: Grid altında child GO "GroundTilemap" + Tilemap component. collisionTilemap: child "CollisionTilemap" + Tilemap.
  - IsoRoomBuilder: ayrı GO + AddComponent. SetPrivate ile grid, groundTilemap, collisionTilemap enjekte.
  - floorTile = ScriptableObject.CreateInstance<Tile>(); collisionTile = ScriptableObject.CreateInstance<Tile>(); SetPrivate ile enjekte. (List<Object>'e ekle, TearDown'da yok et.)
  - template helper: ScriptableObject.CreateInstance<RoomTemplateSO>(); roomId="test"; roomType=Combat; bounds=new RectInt(0,0,5,5); walkableGrid=null (fallback all-walkable); playerSpawn=new PlayerSpawnSocket{ position=new Vector2Int(2,2) }.
- Testler:
  9. `Build_CodeBuiltGrid_PaintsFloorAndSpawnMarker`: rig kur; builder.Build(template); Assert builder.LastFloorCells!=null && builder.LastFloorCells.Count==25; groundTilemap.GetTile(new Vector3Int(2,2,0))!=null; builder.PlayerSpawnMarker!=null.
  10. `BuildExitDoors_ReturnsOneObjectPerDoorType`: rig kur; gateNorthSprite enjekte = `Sprite.Create(new Texture2D(4,4), new Rect(0,0,4,4), new Vector2(0.5f,0.5f))` (texture'ı da List<Object>'e ekle); builder.Build(template); var doors=builder.BuildExitDoors(new List<RIMA.RoomType>{ RIMA.RoomType.Combat, RIMA.RoomType.Elite }); Assert doors.Count==2; doors[0].name=="ExitDoor_0_Combat"; doors[1].name=="ExitDoor_1_Elite".
- TearDown: oluşturulan tüm GameObject (Grid kökü builder GO'su) + runtime asset'leri (Tile'lar, Texture2D, Sprite, template) Object.DestroyImmediate ile yok et. PlayerSpawnMarker/door GO'ları builder hiyerarşisi altında, kökü yok edince gider.

# Notlar
- `using RIMA;` SADECE yeni-DungeonGraph testinde YASAK (alias var). Diğer dosyalarda RoomType'ı `RIMA.RoomType` tam-nitelikli yaz, `using RIMA;` koyma (tutarlılık + çakışma riski sıfır).
- .meta dosyası üretme — Unity import edince kendi üretir.
- Derleme kontrolü: bu testler UnityEngine + nunit'e bağlı; `dotnet build` ile değil, Unity Test Runner ile doğrulanacak (orchestrator yapar). Sen sadece compile-mantıklı, doğru using/namespace ile yaz.
- Sonucu CODEX_DONE.md'ye yaz: hangi 3 dosya yazıldı + her dosyadaki test adları + varsa BLOCKED noktası.
