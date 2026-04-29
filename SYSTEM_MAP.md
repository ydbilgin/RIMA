# RIMA — Sistem Mimarisi Hızlı Referans
*Token tasarrufu için: dosya okumadan önce buraya bak. Sadece edit gerektiğinde kaynak aç.*
*Son güncelleme: 2026-04-18*

---

## Oda Sistemi

**Tek oda aynı anda** — Hades modeli. Fiziksel dungeon yok, her oda aynı tilemap'te rebuild edilir.

| Script | Yol | Satır | Görevi |
|---|---|---|---|
| RuntimeRoomManager | Core/RuntimeRoomManager.cs | ~745 | Oda lifecycle, spawn, kapı, ödül, floor tint |
| RoomTransitionFX | Core/RoomTransitionFX.cs | ~105 | Fade in/out ekran geçişi (oda arası) |
| DungeonGraph | Core/DungeonGraph.cs | ~258 | STS2-stil graf: 12 sabit node + opsiyonel 3. yol |
| DungeonMapUI | UI/DungeonMapUI.cs | ~413 | M tuşu harita overlay (ScreenSpaceOverlay canvas) |
| RoomBuilder | Map/RoomBuilder.cs | ~471 | Editor-only oda inşa (menu item) |
| Act1RoomPainterEnhanced | Editor/Act1RoomPainterEnhanced.cs | ~341 | Act1 test oda paint (dekoratif) |

### Oda Boyutları (sabit)
- Grid: **32×24** tile, duvar kalınlığı=2, kapı genişliği=2
- Floor alanı: 28×20 (duvarlar hariç)
- Kapı pozisyonları: N/S center X=15, E/W center Y=11

### Oda Lifecycle (RRM)
```
StartRoom() → CloseAllDoors → ClearEnemies/Rewards → ApplyRoomTint(roomType)
  → Boss check (DungeonGraph.IsBossRoom)
  → StartRoomByType(roomType) → SpawnEnemies / TrySpawnChest / OpenDoorsAfterClear
  → OnEnemyDied → aliveEnemies-- → RoomClearedSequence
  → SpawnReward ? (Wait for interaction → OpenDoorsNow) : OpenDoorsNow
  → TrySpawnMapFragment

OnPlayerEnteredDoor(dir) → RoomTransitionFX.DoTransition( → Teleport + StartRoom)
```

### Kapı Sistemi
- **DoorTrigger.cs** (Core/) — OnTriggerEnter2D → RRM.OnPlayerEnteredDoor(direction)
- **GateBehavior.cs** — Görsel gate sprite (açık/kapalı)
- Kapılar: doorNorth/South/East/West (DoorTrigger) + gateN/S/E/W (GateBehavior)
- Duvar tile'ı kapı boşluğuna koyarak kapatılır, kaldırılarak açılır

### DungeonGraph Yapısı
- 12 sabit RoomNode (id 0-11) + opsiyonel fork1W, fork3W
- Her node: id, roomType, depth (Y), lane (X), exits dict, visited, revealed
- Fork'lar: depth 2 (fork1), depth 4 (fork2), depth 6 (fork3)
- Navigate(direction) → currentNodeId güncelle → RevealExitsFrom

### Oda Tipleri (RoomType enum)
Combat, Elite, Boss, Chest, Merchant, Event, Forge

---

## Oyuncu Sistemi

| Script | Yol | Görevi |
|---|---|---|
| PlayerController | Player/PlayerController.cs | Hareket (WASD), Dash (Space), LMB/RMB skill |
| Health | Core/Health.cs | HP, OnDeath event, TakeDamage/Heal |
| RageSystem | Player/RageSystem.cs | V meter: hit→+2, kill→+5, decay 10/s |
| PlayerStats | Player/PlayerStats.cs | Base stats, modifier system |

---

## Skill Sistemi

| Script | Yol | Görevi |
|---|---|---|
| SkillBase | Skills/Base/SkillBase.cs | Abstract skill: Execute, Cooldown, Tier |
| SkillController | Skills/SkillController.cs | 6 slot, LMB/RMB/1-4 input binding |
| DraftManager | Skills/DraftManager.cs | 3 offer UI, seçim, IsDraftActive |
| SkillOfferGenerator | Skills/SkillOfferGenerator.cs | Havuz oluşturma, tier/class ağırlıkları |
| SkillDatabase | Skills/SkillDatabase.cs | ScriptableObject registry |

### Draft Akışı
```
RoomCleared → RewardPickup spawn → G tuşu → DraftManager.ShowDraft()
  → SkillOfferGenerator.Generate(3) → UI göster → Seçim → SkillController.EquipSkill
  → IsDraftActive=false → RewardPickup.Destroy
```

---

## HUD Sistemi

| Script | Yol | Görevi |
|---|---|---|
| HUDController | UI/HUDController.cs | Ana HUD: HP bar, Rage bar, room status, skill bar |
| DungeonMapUI | UI/DungeonMapUI.cs | M tuşu map overlay |
| DeathScreenManager | Core/DeathScreenManager.cs | Ölüm ekranı |
| SkillBarUI | UI/SkillBarUI.cs | Skill ikonları + cooldown gösterge |

**Not:** HUDManager.cs legacy — kullanılmıyor. RRM.hud → HUDController tipinde.

---

## İnteraksiyon Sistemi

- **Tek tuş:** G (tüm toplanabilir objeler)
- **RewardPickup.cs** (Core/) — Proximity prompt → G → draft → destroy
- **MapFragment.cs** (Core/) — Proximity prompt → G → reveal → destroy
- **Prompt yapısı:** WorldSpace Canvas, sortingOrder=20, scale=0.012f
- **interactRadius:** ~1.5f

---

## Düşman Sistemi

| Script | Yol | Görevi |
|---|---|---|
| BaseMobBehavior | Enemies/BaseMobBehavior.cs | Temel AI: chase, attack, patrol |
| EnemyAnimator | Enemies/EnemyAnimator.cs | Sprite flip, anim state |
| Health | Core/Health.cs | Ortak HP component |

**Prefab'lar:** Assets/Prefabs/Enemies/ — ChainWarden, FractureImp, HalfThrall, Penitent, RelicCaster, SeamCrawler, VoidThrall
**Boss:** Assets/Prefabs/Enemies/Boss/PenitentSovereign.prefab

---

## Inspector Bağlantıları (Systems GO)

| Component | Field | Değer |
|---|---|---|
| RuntimeRoomManager | wallTilemap | IsoGrid/Walls |
| RuntimeRoomManager | floorTilemap | IsoGrid/Ground |
| RuntimeRoomManager | hud | HUD_Canvas (HUDController) |
| RuntimeRoomManager | playerTransform | Player |
| RuntimeRoomManager | bossPrefab | PenitentSovereign.prefab |
| RuntimeRoomManager | rewardPickupPrefab | RewardPickup.prefab |
| RuntimeRoomManager | mapFragmentPrefab | MapFragment.prefab |
| RuntimeRoomManager | bossRoomNumber | 10 |
| RuntimeRoomManager | baseEnemyCount | 4 |

---

## Test Durumu
- **EditMode:** 51/51 geçiyor
- **asmdef:** Assets/Tests/EditMode/RIMA.Tests.EditMode.asmdef
- **Test komutu:** MCP run_tests mode=EditMode

---

## Dosya Yapısı (Kısaltma)
```
Assets/
  Scripts/
    Core/      — RuntimeRoomManager, RoomTransitionFX, DungeonGraph, DoorTrigger, Health, RewardPickup, MapFragment
    Player/    — PlayerController, RageSystem, PlayerStats
    Skills/    — SkillBase, SkillController, DraftManager, SkillOfferGenerator, SkillDatabase
    Enemies/   — BaseMobBehavior, EnemyAnimator
    UI/        — HUDController, DungeonMapUI, SkillBarUI
    Map/       — RoomBuilder
    Obstacles/ — WoodenCrate, DestructibleObstacle
  Editor/      — Act1RoomPainterEnhanced, RoomBuilder (editor-only)
  Prefabs/     — Enemies/, Obstacles/, RewardPickup, MapFragment
  Tiles/       — Floor, Wall, Column .asset
  Scenes/      — _IsoGame.unity (ana sahne)
```
