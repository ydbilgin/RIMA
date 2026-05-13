---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Room authoring rehberi"
---
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

## RIMA Map Builder (Karar #115 LOCKED)

### Mimari
- Unity Editor Window tabanli (mevcut F2 hotkey).
- Fullscreen "in-game editor" framing REJECTED — mevcut Editor Window kalir, brush UX ve toolbar polish ile evrilir.
- LLM/PixelLab API cagrisi YASAK (Karar #106 uyumlu) — tum AI baseline pure C# deterministic.

### Faz 1.0 Bilesenleri (MVP, 12-16 saat)
- `Assets/Scripts/Systems/Map/RoomBaselineGenerator.cs` — System.Random, GenerationInput kontrati.
- `Assets/Scripts/Systems/Map/RoomBaselineTemplate.cs` — ScriptableObject (biome, archetypeId, w/h araligi, floor variant weight, wall variant rules, anchor zone defaults).
- `RoomDesignerWindow` toolbar `btn-generate` butonu.
- Save akisi: RoomBlueprint asset + Room Prefab + RoomPrefabLink + **RoomConfig** (zorunlu, RoomLoader kontratina uyumluluk).
- FloorVariantPainter + WallAutoConnect bake entegrasyonu (mevcut sistemler cagrilir).
- byte[] grid + LUT variant metadata (mevcut RoomBlueprint.floorVariantIndex byte[] uyumlu).

### Faz 1.5 Bilesenleri (Polish, 30-40 saat)
- Inpaint Region brush mode — kilitsiz hucreleri re-seed, locked hucreler dokunulmaz.
- Force re-seed komutu — lock'lari yok sayar (explicit designer action).
- Anchor Zone painter — tile-mask + zone type enum + weight float. Save sirasinda Transform/child marker'a donusur.
- RenderTexture cache + repaint debounce.
- Preview kamera ~35 derece konverjans kalibrasyonu (Karar #113 uyumu).
- floorOverrideVariantIndex eklenmesi (wall icin mevcut overrideVariantIndex var, floor icin de gerekli).

### Exit Criteria Faz 1.0
- Ayni GenerationInput ile bit-identical RoomBlueprint uretilir.
- 5 farkli seed/biome/archetype generate, RoomLoader runtime hatasiz yukler.
- Designer manuel duzeltme orani %20 alti.
- Save edilen prefab RoomConfig referansi tasir, RoomLoader RoomConfig-missing hata atmaz.
- UnityEngine.Random global state generator cagrisi sirasinda degismez.

### Naming Netlestirme
"8 wall variant" terminolojisi yanlis okunabilir. Dogrusu: **4-bit NSEW mask → 8 wall connection tile variants** (her komsuluk kombinasyonu icin ayri tile). Karakter 8 yon animasyonu (Karar #114) ile karistirilmamali — ayri domain.

### REJECTED Listesi
- Fullscreen "oyun gibi" in-game editor (Antigravity onerisi, scope yutucu)
- LLM runtime/editor cagrisi
- PixelLab Inpaint API cagrisi (Karar #106 ihlali)
- PNG export (RIMA prefab tabanli)
- Runtime procedural 15-node placement override (Karar #62 ihlali)
- RoomLoader secim mantigi bypass
- Rect/polygon anchor schema (tile-mask kullanilir)
- UnityEngine.Random global state kullanimi (determinism)

