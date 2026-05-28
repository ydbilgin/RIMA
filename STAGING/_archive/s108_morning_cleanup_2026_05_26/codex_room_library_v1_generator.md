# Codex Task — Room Library v1 Generator (10 RoomTemplateSO assets)

## Görev
**Opus blueprint tasarımı** + **Codex Editor script implementation** → `Assets/Data/Rooms/Library/` altında 10 `RoomTemplateSO.asset` üretim. User Unity'de Library/ açar → 10 oda görür → Brush V1 editor'da yükleyebilir.

---

## Mevcut API (read-only reference)

### `RoomTemplateSO` (`Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`)
```csharp
public class RoomTemplateSO : ScriptableObject {
    public string schemaVersion = "1.0";
    public string roomId;
    public string biomeId;
    public RIMA.RoomType roomType;
    public RectInt bounds;
    public List<DoorSocket> doorSockets;
    public PlayerSpawnSocket playerSpawn;  // nullable
    public List<EnemySpawnSocket> enemySpawnSockets;
    public CameraBounds cameraBounds;
    public GameObject prefabRef;
    public List<string> encounterTags;
    public List<string> difficultyTags;
    public List<string> blockerTags;
    public List<PropPlacementData> props;  // Sprint 12
    public bool[] walkableGrid;             // Sprint 13 — index = (y * bounds.width) + x
}
```

### `DoorSocket` (`Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs`)
```csharp
public class DoorSocket {
    public string socketId;
    public Vector2Int position;
    public RIMA.DoorDirection direction;  // North / South / East / West
    public int widthInTiles = 2;
    public bool isExit = true;
}
```

### `PlayerSpawnSocket` / `EnemySpawnSocket`
- PlayerSpawnSocket: position + facing (DoorDirection)
- EnemySpawnSocket: position + tierHint (string — "standard", "elite", "boss")

### `PropPlacementData`
```csharp
public class PropPlacementData {
    public string propDefinitionGuid;
    public Vector2Int tilePosition;
    public int rotationSteps = 0;  // 0/1/2/3 = 0/90/180/270
    public int variantIndex = -1;  // -1 = use stable hash
}
```

### `RIMA.RoomType` enum
`Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor`

### Existing Prop reference
- `Assets/Data/Brush/Props/Barrel/barrel_001.asset` — propDefinitionGuid: **AssetDatabase.AssetPathToGUID** ile resolve et (asset path → GUID)

---

## OPUS BLUEPRINT — 10 Room Specs

### Convention (ASCII grids)
- `.` = floor (walkable=true)
- `#` = wall (walkable=false)
- `P` = player spawn anchor (walkable=true, no prop)
- `M` = mob spawn anchor "standard" (walkable=true, no prop)
- `E` = elite spawn anchor "elite" (walkable=true, no prop)
- `B` = boss spawn anchor "boss" (walkable=true, no prop)
- `D` = door socket position (walkable=true, edge of bounds)
- `b` = barrel prop placement (walkable=false until removed, use barrel_001 GUID)
- ` ` = empty (no special meaning, treat as floor)

**Y axis convention:** Top row of ASCII = HIGHEST y (north), Bottom row = LOWEST y (south). Adjust if Codex's chosen convention differs — just be consistent.

---

### Room 1 — Spawn Room (Corridor type, 8×6, single door E)
```
########
#......#
#.P....D
#......#
#......#
########
```
- `roomId`: "Spawn_01"
- `biomeId`: "ShatteredKeep"
- `roomType`: Corridor
- `bounds`: (0, 0, 8, 6)
- Player spawn at (2, 3), facing East
- 1 door East at (7, 3), width 2, isExit=true
- No enemies, no props
- Tags: ["spawn", "tutorial", "entrance"]

### Room 2 — Corridor Linear (Corridor, 12×4, doors W+E)
```
############
D....M.....D
D..........D
############
```
- `roomId`: "Corridor_Linear_01"
- `roomType`: Corridor
- `bounds`: (0, 0, 12, 4)
- 2 doors: West at (0, 1) facing W, East at (11, 1) facing E
- 1 enemy spawn at (5, 2), tier "standard"
- No player spawn (transit room)
- Tags: ["corridor", "transit"]

### Room 3 — Corridor L-Shape (Corridor, 10×8, doors S+E)
```
##########
#........#
#........#
#........#
#####....#
####.....D
####.....#
####DD####
```
- `roomId`: "Corridor_LShape_01"
- `roomType`: Corridor
- `bounds`: (0, 0, 10, 8)
- 2 doors: South at (4, 0) facing S, East at (9, 2) facing E
- 2 enemy spawn: (7, 4), (5, 6), both "standard"
- L-shape walkable path
- Tags: ["corridor", "transit", "l-shape"]

### Room 4 — Combat Small (Combat, 8×6, doors N+S)
```
###DD###
#......#
#.M..M.#
#..b...#
#.M....#
###DD###
```
- `roomId`: "Combat_Small_01"
- `roomType`: Combat
- `bounds`: (0, 0, 8, 6)
- 2 doors: North at (3, 5) facing N, South at (3, 0) facing S
- 3 enemy spawn: (2, 3), (5, 3), (2, 1), tier "standard"
- 1 barrel prop at (3, 2)
- Tags: ["combat", "small", "3-enemy"]

### Room 5 — Combat Medium (Combat, 12×8, doors N+S+E)
```
####DD######
#..........#
#.M.K..K.M.#
#..........#
#.bb....bb.#
#..........#
#.M.K..K.M.#
####DD######
```
Note: `K` = column = use barrel placeholder (Codex will mark TODO comment for column replacement post-prop-production)

- `roomId`: "Combat_Medium_01"
- `roomType`: Combat
- `bounds`: (0, 0, 12, 8)
- 3 doors: North at (4, 7), South at (4, 0), East at (11, 4) all wide=2
- 4 enemy spawn: (2, 6), (9, 6), (2, 1), (9, 1) tier "standard"
- 6 barrel props at: (3, 6), (8, 6), (3, 1), (8, 1), (2, 3), (9, 3)
- Tags: ["combat", "medium", "4-enemy", "pillars"]

### Room 6 — Combat Large (Combat, 16×10, all 4 doors)
```
########DD######
D..............#
D..............#
#..M.....M....M#
#..............#
#......b.......#
#..M.....M....M#
#..............D
#..............D
########DD######
```
- `roomId`: "Combat_Large_01"
- `roomType`: Combat
- `bounds`: (0, 0, 16, 10)
- 4 doors: North (8, 9), South (8, 0), West (0, 7), East (15, 2)
- 6 enemy spawn: (3, 6), (8, 6), (13, 6), (3, 3), (8, 3), (13, 3) tier "standard"
- 1 barrel at (7, 5)
- Tags: ["combat", "large", "6-enemy", "open-arena"]

### Room 7 — Elite Room (Elite, 10×8, doors N+S)
```
####DD####
#........#
#..M....M#
#........#
#...EE...#
#...EE...#
#........#
####DD####
```
- `roomId`: "Elite_01"
- `roomType`: Elite
- `bounds`: (0, 0, 10, 8)
- 2 doors: North (4, 7), South (4, 0)
- 1 elite spawn at (4, 4) (center, mark EE as 1 elite at center), tier "elite"
- 2 mob spawns: (3, 6), (7, 6) tier "standard"
- Tags: ["elite", "mid-tier", "central-feature"]

### Room 8 — Treasure Room (Chest, 6×6, single door S)
```
######
#.b.b#
#....#
#....#
#.b.b#
###DD#
```
- `roomId`: "Treasure_01"
- `roomType`: Chest
- `bounds`: (0, 0, 6, 6)
- 1 door South at (3, 0)
- No enemies
- 4 barrel props at (1, 4), (3, 4), (1, 1), (3, 1) — "candles" placeholder (TODO: replace with candle prop when produced)
- Tags: ["treasure", "chest", "safe"]

### Room 9 — Shrine Room (Curse, 8×8, doors W+E)
```
########
#......#
#..bb..#
D.b..b.D
D.b..b.D
#..bb..#
#......#
########
```
- `roomId`: "Shrine_01"
- `roomType`: Curse
- `bounds`: (0, 0, 8, 8)
- 2 doors: West (0, 3), East (7, 3) both wide=2 vertically
- No enemies
- 8 barrel props in altar pattern around (3-4, 3-4) center — TODO: 4 candles + 1 altar replacement
- Tags: ["shrine", "curse", "no-combat"]

### Room 10 — Boss Intro (Boss, 14×10, doors N+S)
```
######DD######
#............#
#............#
#.....BB.....#
#.....BB.....#
#............#
#............#
#.....P......#
#............#
######DD######
```
- `roomId`: "Boss_Intro_01"
- `roomType`: Boss
- `bounds`: (0, 0, 14, 10)
- 2 doors: North (6, 9), South (6, 0)
- 1 boss spawn at (6, 6) (center BB region marks boss), tier "boss"
- Player spawn at (6, 2), facing North
- No barrels (dramatic empty space)
- Tags: ["boss", "intro", "dramatic"]

---

## CODEX IMPLEMENTATION SPEC

### File to write: `Assets/Editor/MapDesigner/SampleRoomLibraryGenerator.cs`

Editor menu item: `RIMA → MapDesigner → Brush → Generate Sample Library v1`

When clicked:
1. Verify `Assets/Data/Rooms/Library/` exists (create if not)
2. Resolve barrel propDefinitionGuid from `AssetDatabase.AssetPathToGUID("Assets/Data/Brush/Props/Barrel/barrel_001.asset")` — bu GUID Brush V1 PropRegistry'sinin beklediği guid string
3. Generate 10 `RoomTemplateSO` instances per blueprints above
4. For each room:
   - Set schemaVersion, roomId, biomeId="ShatteredKeep", roomType, bounds
   - Build walkableGrid bool[] from ASCII layout (index = y * width + x; **be careful about ASCII row→y mapping**: top ASCII row = y=height-1, bottom ASCII row = y=0)
   - Build doorSockets from D positions + edge detection for direction
   - Set playerSpawn if P exists
   - Build enemySpawnSockets from M/E/B with appropriate tierHint
   - Build props from b positions (PropPlacementData with barrelGuid)
   - Set encounterTags from Opus tag list
5. Save asset: `AssetDatabase.CreateAsset(template, $"Assets/Data/Rooms/Library/{roomId}.asset")`
6. `AssetDatabase.SaveAssets()` + `AssetDatabase.Refresh()`
7. Log: `Debug.Log($"[SampleRoomLibraryGenerator] Generated {count} room templates in Library/")`
8. **NO EditorUtility.DisplayDialog** (memory feedback: BANNED in editor scripts)

### Coordinate convention assurance
- Bounds origin: (0,0) at bottom-left
- ASCII top row = max-y, bottom row = y=0
- WalkableGrid index = (y * width) + x

### Tags / encounter / difficulty / blocker
- encounterTags: Opus tag listesi yukarda
- difficultyTags: ilk pass'ta empty (Sprint 15 affix sistemi #149 sonra doldurur)
- blockerTags: empty

### Tests
**Yeni file:** `Assets/Tests/EditMode/Editor/SampleRoomLibraryGeneratorTests.cs`
- 1 test: `GenerateLibrary_Creates10Templates_AllSerialize` — generator çalıştır, Library'de 10 .asset oluştu mu? Each RoomTemplateSO load et + bounds + walkableGrid + doorSockets count verify et.
- 1 test: `Room1_Spawn_HasPlayerSpawnSocket` — Spawn_01 yüklendi mi, playerSpawn null değil, position (2,3)?
- 1 test: `Room10_BossIntro_HasBossSpawn` — Boss_Intro_01 yüklendi mi, enemySpawn tierHint="boss"?

### Acceptance Criteria
- [ ] `Assets/Editor/MapDesigner/SampleRoomLibraryGenerator.cs` written
- [ ] `Assets/Tests/EditMode/Editor/SampleRoomLibraryGeneratorTests.cs` written (3 test)
- [ ] Menu item runs without compile error
- [ ] Library/ klasöründe 10 `.asset` (Spawn_01, Corridor_Linear_01, Corridor_LShape_01, Combat_Small_01, Combat_Medium_01, Combat_Large_01, Elite_01, Treasure_01, Shrine_01, Boss_Intro_01)
- [ ] All EditMode tests PASS (3 new + existing 321 must remain GREEN)
- [ ] `Debug.Log` summary at completion, no dialogs

### Çıktı
`STAGING/codex_room_library_v1_DONE.md` — implementation transcript + test results.
