# RIMA Phase 2+ Planning Document

**Status:** DRAFT REFERENCE — orchestrator onayı bekler.
**Basis:** PROGRESSION_PLAN_v2_3_LOCK.md Section 2 + 5, SKILL_DESIGN_CHECKLIST.md
**Date:** 2026-05-21
**Filozofi:** Phase 1 demo'yu bloklayan hiçbir şey YOK.

---

## 1. Skill Expansion Roadmap — 6 Class

### Targets

| Class | Current | Target | Gap |
|---|---|---|---|
| Ronin | 4 | 13 | +9 |
| Gunslinger | 8 | 13 | +5 |
| Ravager | 8 | 13 | +5 |
| Hexer | 8 | 13 | +5 |
| Brawler | 8 | 13 | +5 |
| Summoner | 8 | 13 | +5 |

### Ronin 2.1 batch (3 skills, immediate post-demo)
- **Wind Step** (Surge / AfterimageTrail) — dash with sakura afterimage
- **Crimson Storm** (Burst / SequentialStrike) — 3-target chain leap
- **Ki Deflect** (Sustain / PlacedEffect) — parry sigil, invuln window

### Gunslinger 2.1 batch (2 skills)
- **Scatter Mine** (Outlet / PlacedEffect) — proximity mine
- **Sidestep** (Surge / AfterimageTrail) — lateral dash + invuln

### Ravager 2.1 batch (2 skills)
- **Rampage** (Surge / AfterimageTrail) — rage-charge path damage
- **Blood Trail** (Sustain / PlacedEffect) — damaging zone behind movement

### Hexer 2.1 batch (2 skills)
- **Blight Step** (Surge / AfterimageTrail) — teleport + poison cloud at origin
- **Wither Burst** (Burst / PlacedEffect) — detonates all active Curse Marks

### Brawler 2.1 batch (2 skills)
- **Ground Pound** (Outlet / PlacedEffect) — only ranged option for class
- **Gut Check** (Surge / AfterimageTrail) — dash + counter punch

### Summoner 2.1 batch (2 skills)
- **Spirit Surge** (Surge / AfterimageTrail) — consume wisp dash + explosion
- **Spectral Barrage** (Outlet / ProjectileFan) — wisps as homing bolts

Phases 2.2 + 2.3: additional skills to reach 13 each (see source draft for details).

---

## 2. Death Imprint Echo Drop — Full Post-Launch Spec

### Save Data Schema
```
DeathImprintRecord {
  runId: GUID, encounterId: string, subRoomIndex: int,
  subRoomTag: enum, mobComposition: List<MobType>,
  playerClass: ClassType, activeSkills: List<SkillId>,
  echoImprint: EchoImprintId, lightingContext: string,
  timestamp: long
}
```
Max 5 records persisted (LRU eviction).

### Ghost Echo Actor
- Silhouette: class idle/walk/attack frames, desaturated, 30% opacity, cyan tint
- Replay `activeSkills[0]` at 0.85× speed
- No collision with player/mobs, walls only
- Lifetime: 45s or room clear

### Spawn Rules
- Filter: `subRoomTag` match + `mobComposition` intersection > 0
- Chance: 35% first visit / 15% revisit per tag
- Anti-farming: same `runId` max 1 spawn per Act, cross-run only

### Reward
- Ghost kill: +25-40 Shattered Echoes
- Ghost escape: no reward

---

## 3. Tag Synergy System (Phase 2.2)

- On Skill Draft commit: scan slots, tally tags
- 2+ same tag → "Synergy: [Tag] ×N" passive
- Examples: Strike×2 = +8% Strike damage, Outlet×2 = +10% projectile speed, Surge×2 = -1s dash CD
- Max 2 synergy active (highest tags win)
- HUD: bottom-right "SYN" badge
- Phase 2 tuning pass required for magnitudes

---

## 4. Spirit Encounter (Phase 3+)

6 types:
- **Forge Wraith** — craft 1 Combined free
- **Shadow Hound** — +1 active skill slot Act only
- **Blood Oracle** — reveal all + 2 Map Fragments
- **Void Seer** — re-roll entire loadout
- **Fallen Champion** — duplicate 1 skill to new slot
- **Ancient Relic** — class-specific Relic 3-choice

Spirit Tag affinity bonus (cross-run): +15% offer quality after 2+ encounters same tag.
Topology: replaces 1 Combat node Act 2-3, never adjacent to Boss. Max 1/Act.

---

## 5. Shards Phase 2+ Spend

3 options (gate decision Phase 2.1):
- **A** Upgrade Currency — 50 Shards = Tier Upgrade any skill (LOW complexity)
- **B** Re-roll Resource — 30 Shards = re-roll 1 Skill Draft slot (LOW)
- **C** Crafting Material — 40 Shards = 1 Component substitute (MEDIUM, may conflict w/ Forge balance)

**Recommendation:** Option B first.

---

## 6. Architect Break NPC Removal (Post-Launch)

```
HubState {
  architectDefeated: bool,
  endingChoice: enum { Stay, Break, Carry, None },
  npcsRemoved: List<NpcId>,
  breakRunCount: int
}
```

- Break ending → `npcsRemoved.Add("Architect")`
- Hub load post-Break: Architect absent, pedestal cracked
- All Architect services routed to Echo Keeper NPC fallback
- NG+: HubState resets (Architect returns, "echoed back" lore)

---

## 7. Hub Phase 2+ Catalog

| Feature | Phase | Scope |
|---|---|---|
| Starting Imprint Pre-Pick (80 Echoes) | 2.1 | LOW |
| Cosmetic Shop (skins/variants) | 2.2 | MEDIUM (PixelLab gen) |
| Hub Decoration (player-placed props) | 2.3 | LOW-MED |
| Run Modifier / Challenge Mode | 2.2 | MEDIUM (flag-based locks) |
| Merchant NPC expanded pool | 2.2 | LOW |
| Lorekeeper NPC (story fragments) | 3+ | LOW |

---

## 8. Architect Ending Meta-Unlock

| Option | Cost | Replay Value |
|---|---|---|
| **A** 11th Class | HIGH (full pipeline) | Very High |
| **B** Keepsake Item (3 endings unlock) | LOW-MED | High |
| **C** Hub Feature (new NPC) | LOW | Medium |
| **D** Story Ending Epilogue | LOW-MED (art + text) | Medium |

**Recommendation order:** D first, then B, then C, then A.

---

## 9. Phase 2 Implementation Roadmap

### Phase 2.1 (2-3 weeks immediate post-demo)
1. Starting Imprint Pre-Pick (Hub §7)
2. Shards Spend Option B re-roll (§5)
3. Ronin 2.1 batch (3 skills)
4. Gunslinger 2.1 batch (2 skills)
5. Story Ending Epilogue Option D (§8)

### Phase 2.2 (4-6 weeks)
1. Tag Synergy System (design gate first)
2. 4 remaining class 2.1 batches
3. All 6-class 2.2 batches
4. Hub Cosmetic Shop + Run Modifier
5. Keepsake Item Option B
6. Death Imprint Echo Drop

### Phase 3+ (long term)
1. Spirit Encounter Node
2. Architect Break NPC Removal
3. Hub Lorekeeper NPC
4. 6-class 2.3 batches
5. 11th Class Option A (largest scope)

---

## Conflicts / Flags

1. **11th Class** not canonical — no roster entry. Opus design gate before work.
2. **Shards Option C** may conflict w/ Forge balance — Phase 2.2 design gate.
3. **Spirit Encounter** not in StS2 graph generator constraint table (Section 6.1b) — Phase 3 planning gate.
