# REVIEW_DESIGN_CX - Katmanli iso loop teknik review

Scope: sadece `STAGING/LAYERED_ISO_LOOP_DESIGN_S6.md` ve izinli `Assets/Scripts` kodlari okundu. NotebookLM denendi fakat auth expired dondu; sahne/prefab-only iddialar bu yuzden "koddan dogrulanamadi" olarak isaretlendi.

## A. Katman modeli

VERDICT: CONCERN

Gerekce:
- Custom-axis global olarak Y eksenine ayarli: `Assets/Scripts/Core/GraphicsSettingsBootstrap.cs:22-23`.
- Dinamik Y-sort manuel `sortingOrder` yazar: `IsoSorter` Y'ye gore order hesapliyor (`Assets/Scripts/Core/IsoSorter.cs:31-33`), `YSortBehaviour` de layer/order yazar (`Assets/Scripts/Core/YSortBehaviour.cs:24-37`).
- Ayrik sorting layer kullanilirsa PreviewBand/FarParallax/Cliffs, Entities'in Y-sort order'lariyla ayni layer icinde karismaz. Ancak mevcut kodda yeni layer adlari yok; mevcut depth stack `Floor`, `Ground`, `BackwallLandmark` kullaniyor ve preview icin sadece order sabiti var (`Assets/Scripts/RoomPainter/RoomDepthStack.cs:30-40`, `:47-62`).
- Portal su an sadece `sortingOrder=5` ayarliyor, sorting layer ayarlamiyor (`Assets/Scripts/Environment/Portal.cs:46`).

FIX:
- Yeni layer adlarini once TagManager/scene setup'a ekle veya mevcut `RoomDepthStack` ile uyumlu adlara indir: `Floor`, `Ground`, `BackwallLandmark`, `Entities`.
- Preview/parallax/cliff root'larina `IsoSorter` veya `YSortBehaviour` ekleme. Statik band'lerde sabit layer/order kullan.
- Portal/gate/preview icin tek sorting politikasi yaz: entity ise `Entities` + pivot Y-sort, band ise statik depth layer.

## B. Boundary

VERDICT: CONCERN

Gerekce:
- 4-kose iso elmas collider demo icin ancak zemin gercekten tek elmas footprint ise saglam. Organik/concave oda icin dis kenar trace gerekir.
- Kodda floor tilemap zaten otorite olarak var: `WalkabilityMap` "cell walkable iff Floor has tile" diyor (`Assets/Scripts/Environment/WalkabilityMap.cs:7-10`, `:186-199`).
- Fizik tarafinda optional `VoidBlocker` var; empty cell'leri doldurup CompositeCollider2D ile cikisi bloklamayi hedefliyor (`Assets/Scripts/Environment/WalkabilityMap.cs:18-22`, `:160-184`).
- Player hareketi floor-disina cikisi kodla da engelliyor: next-frame `IsWalkableWorld` kontrolu ve edge-slide var (`Assets/Scripts/Player/PlayerController.cs:316-338`); dash icin de `IsDashableWorld` + reachability var (`Assets/Scripts/Player/PlayerController.cs:228-243`, `:390-397`).
- Mevcut Walls/CompositeCollider2D sadece duvar/obstacle fizigi kuruyor (`Assets/Scripts/Map/RoomBuilder.cs:363-374`, `:388-397`); bu, floor disi void boundary garantisi degil.
- Player root runtime'da `Player` physics layer'a aliniyor (`Assets/Scripts/Player/PlayerController.cs:111-117`). Hangi layer matrix ile collide ettigi koddan tam dogrulanamadi; RRM sadece Boundary(12) ile Default(0) collision'i acik tutan bir yorum/ayar tasiyor (`Assets/Scripts/Core/RuntimeRoomManager.cs:137-139`).

FIX:
- Demo diamond icin 4-kose collider kabul, ama M1'de asil otoriteyi `WalkabilityMap + VoidBlocker` veya dolu-hucre dis kenar trace yap.
- Boundary collider layer'ini acikca `Boundary` yap, Player layer ile collision matrix'i editor script veya bootstrap ile assert et.
- Walls collider'i boundary olarak yeniden kullanma; duvar/gate delikleri graph/door akisi ile degisir.

## C. Graph-aware loader

VERDICT: BLOCKER

Gerekce:
- `RuntimeRoomManager` sinifinda dogru hook gercekten var: `OnPlayerEnteredDoor` room clear/open guard'i kontrol ediyor (`Assets/Scripts/Core/RuntimeRoomManager.cs:1141-1143`), `DungeonGraph.Navigate(direction)` cagiriyor (`:1153-1160`) ve sonra `StartRoom` ile yukluyor (`:1165-1169`).
- RRM exit'leri graph'tan okuyor ve direction'a gore door/gate aciyor (`Assets/Scripts/Core/RuntimeRoomManager.cs:893-902`, `:1235-1263`).
- Ancak RRM kod uzerinde obsolete: "Not the live spine... Live flow = Systems/Map/RoomLoader.cs" (`Assets/Scripts/Core/RuntimeRoomManager.cs:25`).
- `RoomLoader` ise linear sequence omurga: `_sequence` dizisi (`Assets/Scripts/Systems/Map/RoomLoader.cs:27-30`), `LoadNextInstance` index+1 yukluyor (`:132-147`) ve gate entered event'i `LoadNext()` cagiriyor (`:370-377`). Graph hook yok.
- `DungeonGraph` gereken model ve API'yi sagliyor: node exits direction->target (`Assets/Scripts/Core/DungeonGraph.cs:7-15`), branching exits (`:121-125`), `Navigate` (`:191-204`), current exits type map (`:206-212`).

FIX:
- Once tek live spine kararini ver. Kod kaniti ile tercih: `RoomLoader` omurga kalsin, ustune `RunController/GraphRoomLoaderAdapter` yazilsin.
- `RoomLoader.LoadNextInstance` yerine `LoadGraphExit(DoorDirection dir)` eklenmeli: `DungeonGraph.Navigate(dir)` -> target `RoomType/depth/lane` -> `RoomSequenceData` veya RoomTemplate pick -> `SwapRoomWhileBlack`.
- RRM'deki graph-door mantigi kopyalanabilir, ama RRM'yi buyutmek obsolete notu temizlenmeden riskli.

## D. Portal

VERDICT: CONCERN

Gerekce:
- `Portal.cs` su an sadece `DestinationType`, `visualSprite`, `OnEntered(Action<Portal>)` tasiyor (`Assets/Scripts/Environment/Portal.cs:13-24`).
- `OnTriggerEnter2D` her player enter'da action invoke ediyor, collider disable veya one-shot guard yok (`Assets/Scripts/Environment/Portal.cs:53-58`).
- Mevcut `Gate` zinciri one-shot unsubscribe ile double-trigger'i azaltmis (`Assets/Scripts/Systems/Map/RoomLoader.cs:370-377`), `Gate.Unlock` idempotent (`Assets/Scripts/Environment/Gate.cs:136-142`) ve collider sadece Unlocked iken aciliyor (`Assets/Scripts/Environment/Gate.cs:165-193`).
- `PortalSpawnController` portal sayisini graph exit'ten degil random/RoomTypeData'dan seciyor (`Assets/Scripts/Environment/PortalSpawnController.cs:71-94`, `:127-165`).

FIX:
- `Portal.Configure(ExitChoice choice, Action<ExitChoice> onEntered)` ekle; `DestinationType` sadece visual fallback olsun.
- `bool entered`, collider disable, `RunController.IsTravelling` guard'i ekle.
- Portal spawn sayisi ve destination listesi `DungeonGraph.CurrentNode.exits` kaynakli olmali; `PortalSpawnController` random pick path'i graph mode'da bypass edilmeli.

## E. Preview adalar

VERDICT: CONCERN

Gerekce:
- Statik prefab band teknik olarak feasible, ama mevcut runtime composer bu isi hazir yapmiyor.
- `RoomSequenceData` room icerigini mobSpawns, focalElementPrefab, decorProps olarak tutuyor (`Assets/Scripts/Systems/Map/RoomSequenceData.cs:13-39`); `RoomLoader.BuildRoomContent` bunlari gercek odaya spawn ediyor (`Assets/Scripts/Systems/Map/RoomLoader.cs:229-277`).
- RoomTemplate -> RoomData adapter var; walkable, wallEdges, encounters, backgroundLayers uretiyor (`Assets/Scripts/Map/Runtime/RoomTemplateAdapter.cs:25-39`, `:103-165`). Ancak preview-only composer/hydrator hook'u kodda gorulmedi.
- RRM'de `PreviewRoomByIndex` var ama bu test odasini ana floor'a paint ediyor, void band'de mini ada degil (`Assets/Scripts/Core/RuntimeRoomManager.cs:1391-1430`).

FIX:
- Demo icin elle hazir `RoomPreviewIsland` prefab kullan. Import/on spawn adiminda tum `Collider2D`, AI, `Health`, damage scripts ve spawner komponentlerini disable/destroy eden `PreviewOnlySanitizer` sart.
- Preview ile portal ayni `ExitChoice` objesini paylasmali; index/destination tekrar hesaplanmamali.
- Final composer daha sonra RoomTemplate/RoomData'dan sadece floor/cliff/decor/background okuyan, encounters/mobSpawns atlayan ayri path olmali.

## F. Orb travel

VERDICT: CONCERN

Gerekce:
- PlayerController disable/reenable mevcut RoomLoader pattern'i ile uyumlu: `pc.enabled=false` ve fade bitince enable (`Assets/Scripts/Systems/Map/RoomLoader.cs:117-129`, `:149-161`, `:568-572`).
- `PlayerAttack` root komponent ve PlayerController'a bagli (`Assets/Scripts/Player/PlayerAttack.cs:7-8`, `PlayerController.cs:100-104`).
- `visualRoot` adinda kanitli bir field/hook yok. PlayerAnimator child `Animator` ve onun `SpriteRenderer`'ini buluyor; root'ta ayri SpriteRenderer olabilecegini not ediyor (`Assets/Scripts/Player/PlayerAnimator.cs:55-64`).
- PlayerController disable input action'lari kapatir (`Assets/Scripts/Player/PlayerController.cs:165-179`), ama rb velocity/collider/health immunity/attack commit state restore'u travel director tarafinda net cache edilmezse risk var.

FIX:
- `PlayerTravelVisualState` visualRoot'u serialize etsin; yoksa `GetComponentInChildren<Animator>()?.transform` fallback kullansin.
- Cache/restore listesi: PlayerController.enabled, PlayerAttack.enabled, Collider2D.enabled/isTrigger, Rigidbody2D.bodyType/linearVelocity, SpriteRenderer enabled states, Health immunity.
- Travel boyunca portal collider'larini ve inputu kapat; load bitince restore tek noktadan yap.

## Acik sorulara teknik cevap

1. Boundary: Demo diamond icin 4-kose polygon yeterli olabilir, ama kalici cevap dolu hucre dis kenar trace veya mevcut `WalkabilityMap + VoidBlocker`. Walls/CompositeCollider2D sadece duvar/obstacle; floor disi void icin yeterli kabul edilmemeli.
2. Preview reveal: Teknik olarak room clear sonrasinda pan-down + hafif zoom-out daha dusuk risk. Combat sirasinda kamera zoom/readability degismemeli. Reveal state'i oda clear event'ine baglanmali.
3. Preview detay: Demo'da gercek mini layout tercih edilmeli, fakat preview-only sanitization zorunlu. Dusmanlar spawn edilmemeli; collider/AI/Health yok. Siluet tek basina portal secimini yeterince anlatmaz.
4. Loader: Kod kanitina gore RoomLoader live spine, RRM obsolete. Graph-driven omurga RoomLoader uzerine adapter ile kurulmasi daha temiz. Sahne gercekten RRM kullaniyorsa once obsolete notu/status temizlenmeli.
5. Depth cliffs: Yakindaki island cliff icin mevcut cliff sprite/tile pipeline; far parallax icin basit koyu siluet veya ayni sprite'in dim/scale kopyasi. Far layer collision/y-sort tasimamali.
6. Cliff overflow: Mevcut `CliffAutoPlacer` cliff'i void cell'e degil floor cell'e koyuyor, top contact line'i koruyup transformOffset ile asagi sarkitmayi hedefliyor (`Assets/Scripts/Environment/CliffAutoPlacer.cs:251-255`). `southClearCells=5` sprite yuksekligine gore overflow/standing-column filtresi olarak var (`:20-25`, `:263-270`). `DirectionalCliffTile` offset/scale/height variation yapabiliyor (`Assets/Scripts/Environment/DirectionalCliffTile.cs:19-64`) ve collider yok (`:36-39`). Buna ragmen buyuk sprite overflow'u gorsel risk; cozum scale-down + top-pivot offset + per-cell crop/variant, floor ustune bindirme yok.
7. Tek-yon cliff: Tek S sprite demo icin hizli olur ama mevcut tile 8 direction array destekliyor (`Assets/Scripts/Environment/DirectionalCliffTile.cs:9-17`, `:98-106`). En az S/SE/SW gerekli; full 8 direction arka/yan yuz kalite icin daha dogru. Mevcut placer zaten camera-facing S/SE/SW drop'a agirlik veriyor (`Assets/Scripts/Environment/CliffAutoPlacer.cs:347-358`).

## Build order onerisi

M0: Loader karari ve graph adapter tasarimi. Bu, mevcut M1'den once gelmeli; aksi halde portal/preview hook'lari yanlis spine'a baglanabilir.

M1: Katman depth stack + boundary. Sorting layer adlari, static band policy, WalkabilityMap/VoidBlocker veya traced boundary ayni milestone'da.

M2: Graph-driven portal/gate. `DungeonGraph.CurrentNode.exits` -> ExitChoice -> portal count/bind -> one-shot enter -> load.

M3: Preview islands. Once static sanitized prefab, sonra data-driven composer.

M4: Orb travel. Sadece M2 load zinciri stabil olduktan sonra.

## En riskli 3 nokta

1. Loader spine celiskisi: RRM graph-ready ama obsolete; RoomLoader live ama linear. Yanlis yere uygulama loop'u bozar.
2. Boundary otoritesi: 4-kose polygon organik odada false positive/negative verir; Walls collider void boundary degil.
3. Portal/preview double state: Random PortalSpawnController, Portal.OnEntered guard eksigi ve preview ExitChoice drift'i ayni anda olursa oyuncu yanlis node'a gidebilir veya iki load tetiklenebilir.
