# RIMA -- Room Authoring Spec
**Karar tarihi: 2026-05-08 | Durum: LOCKED**

## Mimari Karar
Procedural tile paint (DungeonWorldBuilder) -> **Prefab-per-room** sistemi.
Her oda varyanti `Assets/Prefabs/Rooms/Act1/{type}/room_name.prefab` formatinda ayri bir prefab.

## Sistem Bilesenleri
- `RoomConfig.cs` -- MonoBehaviour: oda meta verisi + anchor Transform'lari
- `RoomRegistry.cs` -- ScriptableObject: (roomType, depthBand) -> prefab pool + GetRandom()
- `RoomLoader.cs` -- MonoBehaviour: prefab load/unload + event publish
- `LegacyRuntimeRoomManager.cs` -- eski RRM, [Obsolete], Task B'de baglanacak

## RoomConfig Schema (LOCKED)
```
string roomId
RoomType roomType          // Combat/Elite/Boss/Chest/Merchant/Event/Forge/Corridor
int depthBandMin, depthBandMax
Transform[] spawnPoints    // mob spawn (RoomLoader publish eder, mob sistemi tuketir)
Transform[] entryAnchors   // corridor giris noktalari
Transform[] exitAnchors    // corridor cikis noktalari
Transform[] doorAnchors    // kapi pozisyonlari (N/S/E/W)
Transform[] pickupAnchors  // reward/chest spawn noktalari
Vector3 cellSize           // validation icin (base IsoGrid ile compare)
GridLayout.CellLayout gridLayout
GridLayout.CellSwizzle orientation
```

## RoomLoader Event API (LOCKED)
```
static event Action<RoomConfig, GameObject> OnRoomLoaded
static event Action OnRoomCleared
```
- RoomLoader: selection / load / unload / validation / event publish
- RoomLoader YAPMAZ: door state, enemy wave, map fragment, reward pickup, combat completion

## Render Contract (Prefab Authoring Kurallari -- ZORUNLU)
Her room prefab'inda uygulanmali:
- **Sorting Layer**: Ground / Walls / Props / Overlay -- explicit atanmali, hierarchy order'a guvenme
- **Collision tilemap**: `TilemapCollider2D + CompositeCollider2D + static Rigidbody2D` seti tam olmali
- **Y-sort gereken prop'lar**: ayri SpriteRenderer GameObject, TilemapRenderer Chunk mode'da birak; karistirma
- **Room root Transform**: position (0,0,0), scale (1,1,1), rotation identity -- RoomLoader bu root'u parent'lar

## Prefab Hiyerarsisi
```
RoomRoot (RoomConfig component)
+-- Grid
|   +-- Tilemap_Floor     [Sorting Layer: Ground, Order: 0]
|   +-- Tilemap_Walls     [Sorting Layer: Walls, Order: 1] [TilemapCollider2D + CompositeCollider2D + Rigidbody2D static]
|   +-- Tilemap_Overlay   [Sorting Layer: Props, Order: 2]
+-- SpawnPoints/
|   +-- SpawnPoint_1..N
+-- Anchors/
    +-- DoorAnchor_N, _S, _E, _W
    +-- EntryAnchor, ExitAnchor
    +-- PickupAnchor_1..N
```

## Pilot Prefablar (Task A)
- `combat_01` -- Combat, spawnPoints x4, doorAnchors x4
- `reward_01` -- Reward/Chest, pickupAnchors x1, doorAnchors x2
- `corridor_01` -- Corridor, entryAnchor + exitAnchor

## Validation Kurali
RoomLoader, Load() sirasinda prefab cellSize/gridLayout/orientation'ini base IsoGrid ile compare eder. Mismatch -> `Debug.LogWarning` + fallback abort.

## Migration Plani
- Task A: RoomConfig + RoomRegistry + RoomLoader + 3 pilot prefab placeholder
- Task B: RuntimeRoomManager -> LegacyRuntimeRoomManager rename + [Obsolete] + event subscribe
- Task C: Tile paint (3 pilot prefab icin F1 tile kullanimi)
- Task D (Task B playtest PASS sonrasi): Legacy + DungeonWorldBuilder + RoomTemplate + DepthBandTileSet tek commit'te sil
