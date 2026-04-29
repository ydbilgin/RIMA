# KIRO TASK: Unity Scene Wiring — Core Loop Components

**Priority:** ⭐ CRITICAL — tüm yeni scriptler çalışmak için sahneye bağlanmalı
**Estimated time:** 10-15 dakika
**Risk:** LOW — sadece component ekleme ve referans atama

---

## Context

5 yeni script yazıldı, sahneye bağlanmaları gerekiyor:
- `RageSystem.cs` — Player'a eklenecek
- `RuntimeRoomManager.cs` — Günü kurtaracak oda yöneticisi
- `DoorTrigger.cs` — 4 kapıya yerleştirilecek
- `DeathScreenManager.cs` — Ölüm ekranı
- `EliteAffix.cs` — RuntimeRoomManager otomatik kullanıyor, wire gerekmez

## Görevler

### 1. RageSystem → Player'a ekle

```
find_gameobjects: search_term="Player", search_method="by_tag"
manage_components: action="add", target=<player_id>, component_type="RageSystem"
```

Varsayılan değerler iyi, değiştirme.

### 2. RuntimeRoomManager → GameManager objesi oluştur

```
manage_gameobject: action="create", name="GameManager"
manage_components: action="add", target="GameManager", component_type="RuntimeRoomManager"
```

Sonra Inspector'dan şu referansları ata:
- `enemyPrefabs` → Assets/Prefabs/Enemies/ içindeki enemy prefab'ları sürükle
- `wallTilemap` → Room/Wall
- `floorTilemap` → Room/Floor
- `playerTransform` → Player object
- `hud` → HUDManager (zaten sahnede olmalı)

### 3. DoorTrigger × 4 — Kapı pozisyonlarına yerleştir

Room grid: 20×15 tiles. Door gap: 2 tile, wall thickness: 2 tile.

```
# North door (tiles 9-10, y=14-13 → world: ~9.5, 14)
manage_gameobject: action="create", name="Door_North", position=[9.5, 14.5, 0]
manage_components: action="add", target="Door_North", component_type="BoxCollider2D"
manage_components: action="set_property", target="Door_North", component_type="BoxCollider2D", property="isTrigger", value=true
manage_components: action="set_property", target="Door_North", component_type="BoxCollider2D", property="size", value={"x": 2.5, "y": 1.5}
manage_components: action="add", target="Door_North", component_type="DoorTrigger"

# South door
manage_gameobject: action="create", name="Door_South", position=[9.5, 0.5, 0]
# ... aynı BoxCollider2D + DoorTrigger

# East door (tiles x=18-19, y=6-7 → world: ~19, 6.5)
manage_gameobject: action="create", name="Door_East", position=[19.5, 6.5, 0]
# ... BoxCollider2D size={1.5, 2.5} + DoorTrigger

# West door
manage_gameobject: action="create", name="Door_West", position=[0.5, 6.5, 0]
# ... BoxCollider2D size={1.5, 2.5} + DoorTrigger
```

Her DoorTrigger'da `direction` field'ını set et:
- Door_North → DoorDirection.North (0)
- Door_South → DoorDirection.South (1)
- Door_East → DoorDirection.East (2)
- Door_West → DoorDirection.West (3)

```
manage_components: action="set_property", target="Door_North", component_type="DoorTrigger", property="direction", value=0
manage_components: action="set_property", target="Door_South", component_type="DoorTrigger", property="direction", value=1
manage_components: action="set_property", target="Door_East", component_type="DoorTrigger", property="direction", value=2
manage_components: action="set_property", target="Door_West", component_type="DoorTrigger", property="direction", value=3
```

### 4. RuntimeRoomManager'a door referansları ata

```
manage_components: action="set_property", target="GameManager", component_type="RuntimeRoomManager", property="doorNorth", value=<Door_North_id>
manage_components: action="set_property", target="GameManager", component_type="RuntimeRoomManager", property="doorSouth", value=<Door_South_id>
manage_components: action="set_property", target="GameManager", component_type="RuntimeRoomManager", property="doorEast", value=<Door_East_id>
manage_components: action="set_property", target="GameManager", component_type="RuntimeRoomManager", property="doorWest", value=<Door_West_id>
```

### 5. DeathScreenManager → GameManager'a ekle

```
manage_components: action="add", target="GameManager", component_type="DeathScreenManager"
```

UI panel (DeathScreen) henüz yok — sadece component'ı ekle, UI Canvas ayrı yapılacak.

### 6. Eski RoomManager'ı devre dışı bırak

Eğer sahnede eski `RoomManager` component'ı varsa disable et (silme, referans olabilir):
```
find_gameobjects: search_term="RoomManager", search_method="by_component"
# Eğer bulursa: component'ı disable et
```

## DO NOT
- ❌ Scriptleri DEĞİŞTİRME — sadece sahneye ekle
- ❌ Prefab'ları değiştirme
- ❌ Enemy prefab'ları içine bir şey ekleme
- ❌ Tilemap'leri silme veya değiştirme

## REPORT

```
STATUS: DONE
COMPLETED:
  - RageSystem → Player (ID: 51676)
  - GameManager created (ID: -3344)
  - RuntimeRoomManager → GameManager (with all references wired)
    - doorNorth/South/East/West → Door objects
    - wallTilemap → Room/Wall (51812)
    - floorTilemap → Room/Floor (51748)
    - playerTransform → Player (51676)
    - enemyPrefabs → 7 enemy prefabs (FractureImp, SeamCrawler, Penitent, RelicCaster, ChainWarden, VoidThrall, HalfThrall)
  - DeathScreenManager → GameManager
  - Door_North/South/East/West created with BoxCollider2D + DoorTrigger
  - All door directions set correctly (0=North, 1=South, 2=East, 3=West)
  - Old RoomManager (ID: 51866) disabled
  - Scene saved

ERRORS: NONE

NOTES:
  - HUDManager not found in scene (expected, UI Canvas separate task — RuntimeRoomManager.hud field left null)
  - Elite prefabs (VoidThrall_Elite, SeamCrawler_Elite) excluded from enemyPrefabs array
  - Projectile.prefab excluded (not a spawnable enemy)

NEXT_SIGNAL: "Wire done — Claude test edebilir"
```
