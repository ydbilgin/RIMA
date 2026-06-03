# DEMO B-LITE — TEST MATRİSİ KARARI (Council synthesis, 2026-06-03)

**Yöntem:** /council — cx (feasibility/reuse) + ax Gemini 3.1 Pro (deep test-architecture) + ax Gemini 3.5 Flash (lean) → Opus sentez.
Advisor çıktıları: `STAGING/CODEX_DONE_yasinderyabilgin.md` (cx, → CODEX_DONE arşivi), `_council_a_31pro_demo_test_matrix.md`, `_council_a_35flash_demo_test_matrix.md`.

## Uzlaşı (üçü de aynı yöne işaret etti)
1. **DungeonGraph (YENİ saf-C# class) = EditMode, en yüksek regresyon değeri.** Property-style invariant testleri.
2. **RoomRunDirector navigasyonu = EditMode**, `builder == null` ile (BuildCurrentRoom null-builder'da LogError atıp erken döner → state yine kurulur). IsoRoomBuilder `sealed` + interface yok → mock imkânsız, null-builder tek scene-siz yol.
3. **IsoRoomBuilder = PlayMode**, kod-ile-kurulu Grid+Tilemap+Tile (─ `_Arena` sahnesi GEREKMEZ). Sadece: build exception atmaz, floor hücre sayısı, PlayerSpawnMarker, BuildExitDoors kapı sayısı.
4. **RunMapOverlay + branch-doors görsel + cliff tuck/sort matematiği = UNIT TEST YAZMA.** Manuel/agent play-verify. (3.5 Flash: bunlar demo-fazında oynak, görsel; unit test = yüksek bakım maliyeti, düşük değer.)

## İSİM ÇAKIŞMASI (cx doğruladı — kritik)
İki `DungeonGraph` var:
- ESKİ `RIMA.DungeonGraph` (`Assets/Scripts/Core/DungeonGraph.cs`, MonoBehaviour singleton) — HÂLÂ KULLANIMDA (RuntimeRoomManager, DungeonMapUI, MiniMap, DungeonWorldBuilder, MapFragment, eski testler). **Mevcut `DungeonGraphTests.cs` BUNU test ediyor, demo'nun class'ını DEĞİL.**
- YENİ `RIMA.MapDesigner.Room.Runtime.DungeonGraph` (saf C# class) — TESTSİZ.
- **Karar:** Eski class'a DOKUNMA (rename/deprecate AYRI bir iş). Yeni testler: class adı `RoomRuntimeDungeonGraphTests` + `using RuntimeDungeonGraph = RIMA.MapDesigner.Room.Runtime.DungeonGraph;` alias. `using RIMA;` + çıplak `DungeonGraph` ASLA birlikte kullanma.

## asmdef (cx doğruladı)
- Room.Runtime/Data tipleri → `RIMA.Runtime` assembly. Hem `RIMA.Tests.EditMode` hem `RIMA.Tests.PlayMode` zaten `RIMA.Runtime` reference ediyor. **Yeni asmdef GEREKMEZ.**
- EditMode testleri → `Assets/Tests/EditMode/Room/`. PlayMode → `Assets/Tests/PlayMode/Room/`.

## REUSE pattern (mevcut RoomBankPickTests / RoomTemplateWalkableGridTests'ten)
- `ScriptableObject.CreateInstance<RoomTemplateSO>()`; alanları doldur (`roomId`, `roomType`, `bounds`, `walkableGrid`, `playerSpawn`…); `Object.DestroyImmediate` TearDown'da.
- `walkableGrid` = null → bounds içi hepsi walkable (fallback); ya da `new bool[w*h]`, idx = `localY*w + localX`.
- Her test class'ına küçük private helper (`CreateTemplate(...)`, `CreateBank(...)`). Global fixture YOK.

---

## NİHAİ TEST MATRİSİ (10 test — yazılacak)

### A) EditMode — `Assets/Tests/EditMode/Room/RoomRuntimeDungeonGraphTests.cs`  (P0)
| # | Test | Assert |
|---|---|---|
| 1 | `Generate_SameSeed_ProducesIdenticalGraph` | seed=123,depth=5 iki kez → aynı node sayısı/id/depth/roomType/childIds. Determinizm. |
| 2 | `Generate_StructureInvariants_HoldForManySeeds` | PROPERTY: seed 0..49 × depthCount∈{2,3,5,8}. Her biri: startId==0; maxDepth==depthCount-1; depth0 = 1 node & Combat; son depth = 1 node & Boss & childIds.Count==0; her depth>0 node ≥1 parent (childIds birleşiminden); BFS(startId) tüm node'ları gezer (reachability); her non-son node childIds.Count∈[1,3]. |
| 3 | `Generate_DepthCountBelowTwo_ClampsToTwo` | Generate(seed,1) → maxDepth==1, 2 depth, depth0 Combat, depth1 Boss. |
| 4 | `Generate_ZeroAndNegativeSeed_DoesNotThrow` | Generate(0,5) & Generate(-99,5) exception atmaz, geçerli graf. |

### B) EditMode — `Assets/Tests/EditMode/Room/RoomRunDirectorTests.cs`  (P0)
> SetUp'ta `LogAssert.ignoreFailingMessages = true` (null-builder her BuildCurrentRoom'da LogError atar — sayılmasın).
| # | Test | Assert |
|---|---|---|
| 5 | `BeginRun_WithMissingBuilder_GeneratesNavigableGraph` | GO+RoomRunDirector, builder atanmadan `BeginRun()`. Graph!=null; CurrentNodeId==Graph.startId(0); CurrentChoices.Count>0; IsRunComplete==false. |
| 6 | `AdvanceTo_ValidChoice_MovesToChild` | BeginRun → ilk choice id'yi sakla → AdvanceTo(0) → CurrentNodeId == o id. |
| 7 | `AdvanceTo_InvalidIndex_IsNoOp` | BeginRun → AdvanceTo(-1) ve AdvanceTo(999) → CurrentNodeId değişmez. |
| 8 | `IsRunComplete_TrueAtBoss` | BeginRun → `while(!IsRunComplete) AdvanceTo(0)` (max 20 güvenlik) → CurrentNode.roomType==Boss & CurrentChoices.Count==0. |

### C) PlayMode — `Assets/Tests/PlayMode/Room/IsoRoomBuilderTests.cs`  (P1)
> Private [SerializeField] alanları reflection ile enjekte (BindingFlags.NonPublic|Instance). floorTile/collisionTile = `ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>()`. gateNorthSprite = runtime `Sprite.Create(new Texture2D(4,4), rect, pivot)`. Grid GO + 2 child Tilemap. Plain `[Test]` (Build senkron). TearDown'da tüm GO + runtime asset'leri yok et.
| # | Test | Assert |
|---|---|---|
| 9 | `Build_CodeBuiltGrid_PaintsFloorAndSpawnMarker` | 5×5 all-walkable template + playerSpawn(2,2). Build → LastFloorCells.Count==25; groundTilemap.GetTile((2,2,0))!=null; PlayerSpawnMarker!=null. |
| 10 | `BuildExitDoors_ReturnsOneObjectPerDoorType` | Build sonrası gateNorthSprite enjekte → BuildExitDoors([Combat,Elite]) → dönen liste Count==2; isimler "ExitDoor_0_Combat","ExitDoor_1_Elite". |

## YAZILMAYACAK (3.5 Flash over-engineering kritiği — bilinçli atlandı)
- RunMapOverlay OnGUI renk/koordinat/label/M-tuş testleri (görsel, Event.current güvenilmez → manuel play-verify).
- Cliff tuck offset / sortOrder float matematiği (oynak görsel değişken).
- CompositeCollider2D fizik settling / player.position teleport matematiği (flaky, frame-bağımlı).
- `_Arena` tam-rota progression (encounter/reward/timeScale/input) — lifecycle wiring oturunca AYRI integration QC.

## MANUEL / AGENT PLAY-VERIFY KABUL KRİTERLERİ (unit test yerine)
- **RunMapOverlay:** M → harita açılır; tekrar M → kapanır. Start(depth0) altta, Boss üstte. TAM 1 node cyan-border (current). Çizgiler sadece yukarı akar, kopuk node yok.
- **Branch-doors:** Oda-sonu kapı sayısı = graph node çocuk sayısı (1/2/3); yan yana, floor genişliğine sığar (void'de yüzmez); her kapı hedef-tip rünü.

## SONRAKİ ADIM
cx → 3 test dosyasını yaz (`STAGING/cx_task_demo_tests_2026-06-03.md`) → Unity refresh + console (compile) → run_tests EditMode → run_tests PlayMode → fix → checkpoint commit. Görsel kısımlar Unity play + screenshot ile ayrıca doğrulanır.
