> ⚠️ **STALE / SUPERSEDED (2026-06-07).** Bu harita 2026-04-29 dönemini anlatır (RuntimeRoomManager + fiziksel N/S/E/W kapılar = REVOKED mimari).
> Canlı akış için kazanan kaynaklar: `AI_READER_GUIDE.md` + `CURRENT_STATUS.md` + `STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md`.
> Canlı yol: _Arena → RoomRunDirector → IsoRoomBuilder.BuildExitDoors (NW/N/NE Rift portal soketleri).

# RIMA — System Architecture Quick Reference
*Token Saving: Check here before reading source files. Only open source for editing.*
*Last Updated: 2026-04-29*

---

## 🏗️ Room System
**Single Room Model** (Hades-style). No physical dungeon; room rebuilt on same tilemap.

| Script | Path | Line | Role |
|---|---|---|---|
| `RuntimeRoomManager` | `Core/RuntimeRoomManager.cs` | ~745 | Lifecycle, spawn, doors, rewards, floor tint |
| `RoomTransitionFX` | `Core/RoomTransitionFX.cs` | ~105 | Fade in/out screen transition |
| `DungeonGraph` | `Core/DungeonGraph.cs` | ~258 | STS2-style graph: 12 fixed nodes + optional forks |
| `DungeonMapUI` | `UI/DungeonMapUI.cs` | ~413 | Map overlay (M key) |
| `RoomBuilder` | `Map/RoomBuilder.cs` | ~471 | Editor-only room construction (Menu Item) |
| `Act1RoomPainterEnhanced` | `Editor/Act1RoomPainterEnhanced.cs` | ~341 | Act1 test room painting (Decorative) |

### Room Specs (Fixed)
- **Grid:** 32×24 tiles | Wall Thickness = 2 | Door Width = 2
- **Floor Area:** 28×20 (excluding walls)
- **Door Positions:** N/S center X=15 | E/W center Y=11

### Room Lifecycle (RRM)
```csharp
StartRoom() → CloseDoors → ClearEnemies/Rewards → ApplyRoomTint(roomType)
  → Boss check (DungeonGraph.IsBossRoom)
  → StartRoomByType(roomType) → SpawnEnemies / TrySpawnChest / OpenDoorsAfterClear
  → OnEnemyDied → aliveEnemies-- → RoomClearedSequence
  → SpawnReward ? (Wait for interaction → OpenDoorsNow) : OpenDoorsNow
  → TrySpawnMapFragment

OnPlayerEnteredDoor(dir) → RoomTransitionFX.DoTransition( → Teleport + StartRoom)
```

### Door System
- `DoorTrigger.cs` (Core/): `OnTriggerEnter2D` → `RRM.OnPlayerEnteredDoor(direction)`
- `GateBehavior.cs`: Visual gate sprite (open/closed)
- **Components:** `doorNorth/South/East/West` (Trigger) + `gateN/S/E/W` (Behavior)
- **Mechanism:** Wall tile placed in gap to close, removed to open.

### DungeonGraph Structure
- 12 fixed `RoomNode` (ID 0-11) + optional `fork1W`, `fork3W`.
- **Node Data:** ID, `roomType`, depth (Y), lane (X), exits dict, visited, revealed.
- **Forks:** Depth 2 (fork1), Depth 4 (fork2), Depth 6 (fork3).
- **Flow:** `Navigate(direction)` → Update `currentNodeId` → `RevealExitsFrom`.

### Room Types (Enum)
`Combat`, `Elite`, `Boss`, `Chest`, `Merchant`, `Event`, `Forge`

---

## 👤 Player System
| Script | Path | Role |
|---|---|---|
| `PlayerController` | `Player/PlayerController.cs` | Movement (WASD), Dash (Space), LMB/RMB skills |
| `Health` | `Core/Health.cs` | HP, `OnDeath` event, `TakeDamage/Heal` |
| `RageSystem` | `Player/RageSystem.cs` | V-Meter: 1/hit-dealt, 5/hit-taken, 3/kill, Decay=10/s |
| `PlayerStats` | `Player/PlayerStats.cs` | Base stats, modifier system |

---

## ⚔️ Skill System
| Script | Path | Role |
|---|---|---|
| `SkillBase` | `Skills/Base/SkillBase.cs` | Abstract: Execute, Cooldown, Tier |
| `SkillController` | `Skills/SkillController.cs` | 6 slots, LMB/RMB/1-4 input binding |
| `DraftManager` | `Skills/DraftManager.cs` | 3-offer UI, Selection, `IsDraftActive` |
| `SkillOfferGenerator` | `Skills/SkillOfferGenerator.cs` | Pool generation, Tier/Class weights |
| `SkillDatabase` | `Skills/SkillDatabase.cs` | ScriptableObject registry |
| `SkillFlowTracker` | `Systems/SkillFlowTracker.cs` | `OnSkillUsed` event bridge (SkillController → HUD/VFX) |

### Draft Flow
```
RoomCleared → RewardPickup spawn → [G] key → DraftManager.ShowDraft()
  → SkillOfferGenerator.Generate(3) → Show UI → Select → SkillController.EquipSkill
  → IsDraftActive=false → RewardPickup.Destroy
```

---

## 🖥️ HUD System
| Script | Path | Role |
|---|---|---|
| `HUDController` | `UI/HUDController.cs` | Main HUD: HP/Rage bars, room status, skill bar |
| `DungeonMapUI` | `UI/DungeonMapUI.cs` | Map overlay (M) |
| `DeathScreenManager` | `Core/DeathScreenManager.cs` | Game Over screen |
| `SkillBarUI` | `UI/SkillBarUI.cs` | Icons + cooldown display |

*Note: `HUDManager.cs` is legacy. `RRM.hud` uses `HUDController`.*

---

## ⌨️ Interaction System
- **Key:** [G] (All collectables)
- `RewardPickup.cs` (Core/): Proximity Prompt → [G] → Draft → Destroy
- `MapFragment.cs` (Core/): Proximity Prompt → [G] → Reveal → Destroy
- **WorldSpace Canvas:** `sortingOrder`=20, `scale`=0.012, `interactRadius` 1.5-1.8

---

## 👾 Enemy System
| Script | Path | Role |
|---|---|---|
| `BaseMobBehavior` | `Enemies/BaseMobBehavior.cs` | AI: Chase, Attack, Patrol |
| `EnemyAnimator` | `Enemies/EnemyAnimator.cs` | Sprite flip, anim state |
| `Health` | `Core/Health.cs` | Shared HP component |
| `HollowMite` | `Enemies/HollowMite.cs` | Fast melee mob, swarm AI |
| `TheWound` | `Enemies/TheWound.cs` | Environmental hazard mob |
| `BossAI_PenitentSovereign` | `Enemies/Boss/BossAI_PenitentSovereign.cs` | Phase-1 boss AI |

- **Prefabs:** `Assets/Prefabs/Enemies/` — ChainWarden, FractureImp, HalfThrall, Penitent, RelicCaster, SeamCrawler, VoidThrall, HollowMite, TheWound.
- **Boss:** `Assets/Prefabs/Enemies/Boss/PenitentSovereign.prefab`

---

## ✨ VFX System
| Script | Path | Role |
|---|---|---|
| `HandGlowVFX` | `VFX/HandGlowVFX.cs` | Runtime color tinting via `SetColor()` |
| `RiftGlowVFX` | `VFX/RiftGlowVFX.cs` | Rift crack glow: Rage/cast/hit/death states, secondary PS |

- **Rift Crack Rule:** LINE baked in PixelLab sprite · GLOW runtime via `RiftGlowVFX.cs`
- **Ranger Accent:** Cold Blue `#7BA7BC` applied via `RiftGlowVFX` at runtime

---

## 🔗 Inspector Wiring (Systems GO)
| Component | Field | Value |
|---|---|---|
| `RuntimeRoomManager` | `wallTilemap` | `IsoGrid/Walls` |
| `RuntimeRoomManager` | `floorTilemap` | `IsoGrid/Ground` |
| `RuntimeRoomManager` | `hud` | `HUD_Canvas` (HUDController) |
| `RuntimeRoomManager` | `playerTransform` | `Player` |
| `RuntimeRoomManager` | `bossPrefab` | `PenitentSovereign.prefab` |
| `RuntimeRoomManager` | `rewardPickupPrefab` | `RewardPickup.prefab` |
| `RuntimeRoomManager` | `mapFragmentPrefab` | `MapFragment.prefab` |
| `RuntimeRoomManager` | `bossRoomNumber` | 10 |
| `RuntimeRoomManager` | `baseEnemyCount` | 4 |

---

## 🧪 Test Status
- **EditMode:** 112/112 PASS
- **asmdef:** `Assets/Tests/EditMode/RIMA.Tests.EditMode.asmdef`
- **Cmd:** `MCP run_tests mode=EditMode`

---

## 📂 File Structure (Condensed)
```
Assets/Scripts/
  Core/      — Manager, Transition, Graph, Trigger, Health, Pickup, Fragment
  Player/    — Controller, Rage, Stats
  Skills/    — Base, Controller, Draft, Generator, Database
  Enemies/   — MobBehavior, Animator, HollowMite, TheWound, Boss/BossAI_PenitentSovereign
  VFX/       — HandGlowVFX, RiftGlowVFX
  Systems/   — SkillFlowTracker
  UI/        — HUD, MapUI, SkillBar
  Map/       — Builder
  Obstacles/ — Destructibles
Assets/Editor/ — Painters, Builders, PrefabWiringSetup
Assets/Prefabs/— Enemies, Obstacles, Reward, Map
Assets/Tiles/  — Floor, Wall, Column .asset
Assets/Scenes/ — _IsoGame.unity (Main Scene)
```
