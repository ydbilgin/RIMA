# RIMA Phase 1 Implementation Spec

**Source:** PROGRESSION_PLAN_v2_3_LOCK.md Section 6 Roadmap
**Audience:** Codex dispatch — one section per task
**Philosophy check:** basit tut / scope dar / mekanik şişirme yok / demo önce
**Date:** 2026-05-21

---

## Recommended Dispatch Order

1. **6.1b** (graph generator) — foundation for 6.1, 6.2, 6.3
2. **6.1** (UI: MapPanel + HUD badges)
3. **6.2** (Reward flow: fragment pickup + Skill Draft trigger)
4. **6.3** (Sub-room MacroRoomController + fragment guard)
5. **6.7** (Hybrid Rest — LOW effort quick win)
6. **6.5** (Boss gate depth 12 auto-unlock)
7. **6.4** (Cross-class secondary selection + slot unlock)
8. **6.6** (Item economy + Forge + inventory)
9. **6.8** (4-class hub + Shattered Echoes)
10. **6.9** (Image 13 ending strip)
11. **6.10** — DEFER POST-LAUNCH placeholder only

---

## Section 6.1 — UI Foundation: MapPanel + HUD Badges

### Goal
Extend `DungeonMapUI.cs` (TAB key, StS2-style graph) + `MiniMap.cs` for 15-node/7-row/3-column v2.3 layout, fog-of-war 5-state visuals, Tablet HUD badge (X/8 + bonus reveal).

### Code Architecture
```
DungeonMapUI (Assets/Scripts/UI/DungeonMapUI.cs) — EXTEND
  ├─ NodeViewState enum { Unrevealed, Revealed, Current, Visited, NextUp }
  ├─ Rebuild() adapt to 15-node variable layout
  └─ AnimateCurrent() existing

TabletHUDBadge (Assets/Scripts/UI/TabletHUDBadge.cs) — NEW
  ├─ int fragmentCount, int bonusRevealCount
  ├─ void Refresh(int total, int bonusReveals)
  └─ Renders: "Tablet X/8" + "+N reveal" sub-badge

MiniMap (Assets/Scripts/UI/MiniMap.cs) — EXTEND
  └─ NextUp bright-border highlight
```

### Unity Setup
- TabletHUDBadge → HUDController GameObject, anchored top-center below HP
- DungeonMapUI toggle changes M → TAB
- MiniMap: existing 72×72 top-right + NextUp ring overlay

### Data Flow
Fragment pickup → DungeonGraph.RevealAhead(rollResult) → TabletHUDBadge.Refresh → DungeonMapUI.RefreshMap → MiniMap auto-poll

### Acceptance
1. TAB opens map fullscreen (M deprecated)
2. 5 node states render distinctly (?, type icon, pulsing current, gray check, bright NextUp)
3. HUD "Tablet X/8" + "+N reveal" badge updates per fragment pickup

### Effort: MEDIUM (1-2 days)

---

## Section 6.1b — Procedural Map Generation

### Goal
Replace hardcoded `DungeonGraph.BuildGraph()` with seed-deterministic procedural generator producing 15 nodes, 7 depth rows, max 3 lanes per v2.3 constraints. Forge/CurseGate mutually exclusive.

### Code Architecture
```
DungeonGraph (Assets/Scripts/Core/DungeonGraph.cs) — REPLACE BuildGraph()
  ├─ int seed (run start)
  ├─ ProceduralGraphBuilder (nested)
  │    ├─ PlaceNodesByConstraints()
  │    ├─ ConnectEdges() — branching 2-3, all paths reach row 6
  │    └─ AssignBranchNode() — B01 CurseGate OR B02 Forge (mutual exclusive)
  └─ Generate(int seed) public

RunSeedManager (Assets/Scripts/Core/RunSeedManager.cs) — NEW
  └─ static int GenerateSeed()
```

### Constraints (hard)
- Row 0: Entry (1) / Row 6: Boss (1)
- No Shop-Shop adjacent
- No Elite-Elite adjacent
- Row 5 cannot be Elite (no Elite adjacent to Boss row)
- Rest at row ~3-4
- Branch (CurseGate XOR Forge) row 2-4
- At least 1 Entry→Boss path bypasses Branch

### Acceptance
1. Every graph exactly 15 nodes
2. Boss always row 6
3. Constraints not violated (test 100 seeds)
4. Branch = exactly one of CurseGate/Forge
5. debugSeed=42 deterministic across sessions

### Effort: HIGH (2-3 days)

---

## Section 6.2 — Reward Flow: Fragment Pickup + BFS Reveal + Skill Draft

### Goal
Post-combat reward chain: MapFragment spawns after `MacroRoomController` final sub-room clear, G press collects, BFS reveal roll, Skill Draft 3-choice opens.

### Code Architecture
```
MapFragmentPickup — NEW
  ├─ TryCollect() on G press in 2.5u radius
  ├─ RollRevealHops() — 65%=1, 30%=2, 5%=3
  └─ event OnCollected

MapFragmentSpawner — NEW (on MacroRoomController)
  └─ Spawn(roomCenter) after OnFinalSubRoomCleared

DraftManager — EXTEND
  ├─ Weight table (v2.3 Section 1.6)
  ├─ pityCtr (normal-draft-only, Codex Conflict 3 fix)
  └─ SuppressNewSkillIfFull() (Codex Conflict 1 fix)
```

### Weight Table
| State | New | Tier | Imprint |
|---|---|---|---|
| Slots not full + upgrades | 40% | 40% | 20% |
| All 4 slots full | 0% | 80% | 20% |
| No upgrades | 60% | 0% | 40% |

Pity: 5 consecutive normal drafts → 80% NewSkill forced.
Reroll: 1 free/run + 150G Shop.

### Acceptance
1. Fragment spawns only from MacroRoomController final clear
2. G key + 2.5u radius pickup
3. Reveal: 1/2/3 hop %65/30/5 (statistical over 20 runs)
4. Skill Draft auto-opens immediately
5. Pity counts only normal drafts (Elite/Boss excluded)

### Effort: MEDIUM (1-2 days)

---

## Section 6.3 — Sub-Room: MacroRoomController + Fragment Guard

### Goal
MapFragment instantiation only on FINAL sub-room cleared. Mirror edge validator (±2u tolerance) for seam connections.

### Code Architecture
```
MacroRoomController — NEW
  ├─ List<SubRoomTag> sequence
  ├─ int currentSubRoomIndex
  ├─ bool isFinalSubRoom => currentSubRoomIndex == sequence.Count - 1
  ├─ event OnFinalSubRoomCleared
  └─ GUARD: fragment spawn only if isFinalSubRoom

SubRoomTag enum — NEW
  { EntryChamber, PillarArena, CollapseCorridor, RitualHall, CryptCell }

MirrorEdgeValidator — NEW
  └─ Validate(a, b, dir): inverse direction + edge width ±2u + placement ±2u
```

Phase 1 = entry_chamber + pillar_arena sequence only.

### Acceptance
1. Fragment never spawns on intermediate sub-room transition (Bug #3 fix)
2. Fragment spawns once after FINAL sub-room
3. Fade transition 0.2s out + 0.2s in between sub-rooms
4. Phase 1 = 2 sub-room sequence completes

### Effort: MEDIUM (1-2 days)

---

## Section 6.4 — Cross-Class: Secondary Selection + Slot Unlock

### Goal
After Act 1 Boss kill, 2-of-3 secondary class selection (from remaining 3 starting classes), +2 slot unlock (4→6), CrossClassPassive active, CrossClassProcManager enable.

### Code Architecture
```
PlayerClassManager — EXTEND
  └─ GUARD: CrossClassProcManager.enabled = false until secondary selected

SecondaryClassSelectionUI — NEW
  └─ Show([2 candidates from remaining starting classes])

CrossClassProcManager — NEW
  ├─ enabled = false at Awake
  ├─ Enable() called by SelectSecondaryClass
  ├─ OnLMBHit() — 1.2s cooldown + 35% origin damage proc
  └─ event OnCrossClassProc
```

Skill Draft pool shift Act 2 early: primary 65% / sec 20% / neutral 15%.

### Acceptance
1. UI shows exactly 2 candidates from remaining 3 starting
2. +2 slot unlock (total 6 visible SkillBar)
3. CrossClassPassive component added
4. CrossClassProcManager disabled Act 1, enabled post-selection
5. Proc 35% damage, 1.2s cooldown
6. DraftManager weight shifts in Act 2

### Effort: MEDIUM (1-2 days)

---

## Section 6.5 — Boss Door: Depth-12 Auto-Unlock + 8/8 Bonus

### Goal
Boss door unlocks at depth 5 node (Row 5). Fragment count no longer gates. 8/8 fragments = bonus reward (+50 Gold OR +1 free Legendary re-roll).

### Code Architecture
```
BossDoorController — NEW
  └─ IsUnlocked => DungeonGraph.CurrentNode.depth >= 5

TabletBonusRewardUI — NEW (tiny)
  ├─ Show() when fragmentCount == 8
  └─ Choice: +50 Gold OR +1 free Legendary re-roll
```

### Acceptance
1. Boss door opens when depth >= 5
2. Fragment count NEVER gates (Bug #2 fix)
3. 8/8 → bonus UI with 2 options
4. Gold +50 OR free reroll grants properly

### Effort: LOW (4-6 hours)

---

## Section 6.6 — Item Economy: Components + Forge + Inventory

### Goal
3-channel item economy: Component drops (Combat 20%, Elite 100% 2-choice), Forge B02 branch (Combined + Legendary craft, free), Shop Anvil (Gold + rare). 4-slot shared inventory. 9 canonical recipes.

### Code Architecture
```
PlayerInventory — NEW
  ├─ List<ItemData> slots (max 4)
  └─ TryAdd/TryRemove + OnInventoryChanged event

ItemData : SO — NEW
  ├─ itemName, type (Component/Combined/Legendary/Relic)
  ├─ componentType (IronShard/VoidFragment/.../SoulEmber)
  └─ Sprite icon

CraftingSystem — NEW
  ├─ Dictionary<(Comp,Comp), string> recipes [9 entries]
  └─ TryCraft(a, b, out result)

ForgeRoomController — NEW
  ├─ visitCount (0→1 Combined, 1→2 Legendary)
  └─ 1 main action/visit, FREE
```

7 Components: IronShard, VoidFragment, ChainLinks, ShadowVeil, BloodGem, RiftStone, SoulEmber

9 Recipes:
- IronShard + BloodGem → Vampiric Blade
- VoidFragment + ShadowVeil → Phantom Weave
- RiftStone + SoulEmber → Frenzy Core
- IronShard + ChainLinks → Warlord's Plate
- IronShard + RiftStone → Rift Piercer
- BloodGem + SoulEmber → Soul Tap
- VoidFragment + RiftStone → Fracture Amp
- ShadowVeil + SoulEmber → Ghost Step
- ChainLinks + BloodGem → Iron Will

### Acceptance
1. All 9 recipes produce correct output
2. Inventory caps at 4 (5th = error)
3. Forge visit 1 = Combined, visit 2 = Legendary
4. Forge FREE (no Gold)
5. Elite mob guard at Forge entrance
6. Shop Anvil costs Gold + ≤30% probability

### Effort: HIGH (3-4 days)

---

## Section 6.7 — Hybrid Rest: HP Restore OR Skill Tier Upgrade

### Goal
1 Rest node per Act (row 3-4): player chooses "%25 HP restore" OR "1 Skill Tier Upgrade".

### Code Architecture
```
RestRoomController — NEW
  ├─ choiceMade (one-time)
  └─ OnPlayerEnter() → RestChoiceUI.Show()

RestChoiceUI — NEW
  ├─ OnHealChosen() → PlayerStats.Heal(0.25f * maxHP)
  └─ OnTierUpgradeChosen() → SkillTierUpgradeFlow.PickSkillToUpgrade()

SkillTierUpgradeFlow — NEW
  ├─ GetUpgradeableSkills()
  └─ ApplyUpgrade(skill) — tier++ max Legendary
```

### Acceptance
1. 2 options shown
2. Heal restores exactly 25% maxHP capped
3. Tier upgrade lists only skills < Legendary
4. One-time choice per Rest room

### Effort: LOW (4-6 hours)

---

## Section 6.8 — 4-Class Hub + Shattered Echoes

### Goal
4 classes unlocked at run start (Warblade/Elementalist/Ranger/Shadowblade), 6 locked classes show unlock cost in Shattered Echoes. Persistent meta-currency.

### Code Architecture
```
ShatteredEchoesManager — NEW
  ├─ int echoCount (PlayerPrefs persistent)
  ├─ Add/TrySpend
  └─ OnEchoChanged event

ClassUnlockData : SO — NEW
  ├─ classType, echoCost, precondition
  └─ IsUnlocked() — PlayerPrefs check

CharacterSelectScreen — EXTEND
  ├─ Locked cards: silhouette + cost text
  └─ EchoKeeperPanel sub-panel
```

Unlock costs: Ronin/Ravager 120, Gunslinger/Brawler/Summoner 180, Hexer 250 + "Elementalist 1 run" precondition.
Earn: Boss +10, Act complete +5, first class +5, run-end killCount × 0.1.

### Acceptance
1. 4 starting classes selectable
2. 6 locked classes show silhouette + cost (NOT "Coming Soon")
3. Echoes persist across sessions
4. Unlock: deducts cost, marks unlocked, card activates
5. Hexer requires 250 Echoes + 1 Elementalist run

### Effort: MEDIUM (1-2 days)

---

## Section 6.9 — Image 13 Manual Crop + Ending Choice Panel

### Goal
Trim Image 13 bottom band. Replace with compact "Ending Choice: Stay | Break | Carry" strip beneath Architect. Phase 1 = narrative only, no mechanical effect.

### Code Architecture
```
EndingChoicePanel — NEW (minimal)
  ├─ Show() called after Architect defeated
  ├─ 3 buttons Stay/Break/Carry
  └─ NO mechanical effect Phase 1
```

Asset work outside Codex: crop Image 13 bottom band manually.

### Acceptance
1. Panel shows after Architect defeat
2. 3 buttons visible
3. Picking any → identical run-end (narrative only)
4. Image 13 bottom band removed

### Effort: LOW (2-3 hours code + manual asset crop)

---

## Section 6.10 — DEFERRED POST-LAUNCH

| System | Phase | Reason |
|---|---|---|
| Death Imprint Echo Drop | POST-LAUNCH | Persistent room-state architecture exceeds demo scope |
| Tag Synergy | Phase 2+ | Skill tag audit prerequisite |
| Spirit Encounter Node | Phase 3+ | NPC dialogue + encounter system |
| Shards Phase 2 Use | Phase 2+ | Spend mechanic TBD |
| Architect Break NPC Removal | POST-LAUNCH | Phase 1 all 3 endings = same outcome |
| 6-Class Skill Expansion | POST-DEMO | Ronin 4→12, etc. trigger post-playtest |

**Codex MUST NOT implement these in Phase 1.**

---

## Appendix — File Reference

| File | Action |
|---|---|
| `Assets/Scripts/Core/DungeonGraph.cs` | EXTEND BuildGraph() |
| `Assets/Scripts/UI/DungeonMapUI.cs` | EXTEND node state |
| `Assets/Scripts/UI/MiniMap.cs` | EXTEND NextUp highlight |
| `Assets/Scripts/UI/SkillOfferUI.cs` | REUSE as-is |
| `Assets/Scripts/Systems/DraftManager.cs` | EXTEND weights + pity |
| `Assets/Scripts/Systems/PlayerClassManager.cs` | EXTEND secondary select |
| `Assets/Scripts/UI/CharacterSelectScreen.cs` | EXTEND lock states |
| `Assets/Scripts/UI/ForgeUI.cs` | EXTEND item craft mode |

**New files (19):** TabletHUDBadge, RunSeedManager, SubRoomTag, MacroRoomController, MirrorEdgeValidator, BossDoorController, ForgeRoomController, RestRoomController, MapFragmentPickup, MapFragmentSpawner, CraftingSystem, CrossClassProcManager, SkillTierUpgradeFlow, ShatteredEchoesManager, SecondaryClassSelectionUI, RestChoiceUI, TabletBonusRewardUI, EndingChoicePanel, ItemData/ClassUnlockData (SO).
