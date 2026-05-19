---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Summoner kaynak ekonomi kuralları"
---
# SUMMONER ECONOMY RULES
**Date:** 2026-04-30 | **Source:** Claude R4 final lock (Codex R4: "Summoner economy abuse risk -- do not approve without rules")
**Status:** v0.1 lock. Required before any Summoner skill implementation.

## Why This Doc Exists
Summoner abuse vectors are unique among classes:
- Minion stacking can multiply DPS without player input.
- Sacrifice charge feedback loop can fast-track ulti.
- Corpse Field auto-pickup can chain Hex/Wounded spread infinitely.
- Mass Sacrifice + minion respawn can deal full ulti damage twice in one fight.

R4 verdict: Summoner Economy Rules **must lock before** Bone Tide / Soul Tax / Mass Sacrifice tuning. This doc defines the caps.

---

## Hard Caps (no exceptions, no upgrade bypass)

### Minion Cap
| Tier | Max Simultaneous | Notes |
|---|---|---|
| Skeleton (basic) | **6** | LMB summon |
| Bone Knight (heavy) | **2** | Skill summon |
| Wraith (ranged) | **3** | Skill summon |
| **Total minion cap** | **8** | Across all tiers combined |

- Excess summon attempt **fails** (no oldest-replace, no stacking). Player must sacrifice or wait for death.
- Phase 1: 6 skeleton cap only. Other tiers Phase 2.

### Sacrifice CD
- **Sacrifice CD: 1.5s global** (cannot sacrifice multiple minions in same frame).
- **Per-minion ICD: 0.5s** after summon (cannot summon-and-sacrifice instantly for charge farming).
- Mass Sacrifice ulti is **exempt** from per-minion ICD (consumes all at once).

### Sacrifice Charge Fill Rules
- Sacrificing minion fills Sacrifice Charge based on minion tier:
  - Skeleton: **+10**
  - Bone Knight: **+25**
  - Wraith: **+15**
- Max Sacrifice Charge: **100** (= Perfect Condition for Mass Sacrifice ulti).
- **Decay:** if no sacrifice action for 8s, charge decays 5/s (prevents pre-fill before combat).
- **Combat-only fill:** sacrifices outside combat (no enemy aggro within 12 tiles) fill charge at **30% rate**.

### Corpse Field Caps
- Corpse Field tile persists **6s** after enemy death within Summoner aura (8 tile radius).
- **Max 8 corpses tracked simultaneously.** 9th enemy death does not spawn new corpse (oldest persists).
- Boss death: spawns **1 Corpse Field tile only**, not multi-tile.
- Cross-class consumption (Hexer Cursed Link, Ravager Frenzied) shares the same 8-corpse pool.

---

## Ulti / Perfect Condition Rules

### Mass Sacrifice (Phase 2 ulti)
- **Trigger:** Sacrifice Charge = 100 (Perfect Condition)
- **Effect:** sacrifices ALL minions, deals damage burst per minion sacrificed, scales by minion tier
- **Cooldown:** 25s after cast
- **Rule:** consumed minions cannot be re-summoned for 4s after cast (prevents instant re-fill loop)
- **Boss Rule:** burst damage 60% on boss; no panic state applied
- **Lock Reset:** ulti relocks after cast (per R4 Ulti Toggle rule). Charge resets to 0.

### Mass Sacrifice Abuse Vectors (closed)
1. **Pre-summon farm before boss:** sacrifices outside combat fill at 30% rate (closed).
2. **Summon spam mid-fight:** per-minion ICD 0.5s + minion cap 8 (closed).
3. **Re-cast within seconds:** 25s ulti CD + 4s summon lockout after cast (closed).
4. **Multi-class stacking:** Hexer Cursed Link reading Corpse Field corpses caps at 8 shared (closed).

---

## Phase Tags (R4 Lock)

### Phase 1 (Demo)
- Skeleton summon (basic LMB)
- Sacrifice (basic active)
- Corpse Field tracking
- **No ulti, no Mass Sacrifice, no Bone Tide, no Soul Tax**

### Phase 2
- Bone Knight + Wraith tiers unlocked
- Mass Sacrifice ulti
- Command Beacon (with Beacon Pull as upgrade, not separate skill)
- Sacrifice Charge HUD bar

### Later (R4 deferred)
- **Bone Tide** (mass spawn skill) -- requires asset/AI scope and economy retest
- **Soul Tax** (HP-cost economy skill) -- requires Blood Debt cross-class testing
- **Lich Form** (transformation ulti candidate, not in roster yet)

---

## Beacon Pull Decision (R4 Reorganized)
Original R4 proposal: separate active skill.
**Final lock:** Beacon Pull is a **Command Beacon upgrade**, not a separate active skill.
- Reason: per Codex R4 -- "too many new actives" + slot pressure.
- Implementation: when player upgrades Command Beacon (Phase 2 build choice), beacon also recalls Commanded minions to its position.
- Slot economy: 1 active (Command Beacon) instead of 2 (Beacon + Pull).

---

## Cross-Class Sharing Rules

### Corpse Field Consumers
- Hexer Cursed Link can spread Hex through Corpse Field tiles (treats corpse as link node, no damage).
- Ravager Frenzied gains +20% damage standing on Corpse Field tile.
- Both consumers share the 8-corpse pool. **No double-counting.**

### Sacrifice Mark Consumers
- Sacrifice Mark is internal-only. No cross-class consumption.
- Reason: prevents Hexer/Ravager from triggering sacrifice charge externally.

### Commanded State
- Commanded is internal-only. Beacon Pull (Command Beacon upgrade) is the only consumer.
- Other classes cannot interact with minions via state.

---

## HUD / Readability
1. **Sacrifice Charge bar:** under HP bar, fills purple-grey, Perfect Condition pulse at 100.
2. **Minion count counter:** small icon row showing alive minion count and tier breakdown.
3. **Corpse Field tiles:** dark mist ground overlay with skull marker icons (max 8 visible).
4. **Active ulti lock state:** Mass Sacrifice icon shows lock/unlock per Ulti Toggle rule.

---

## Open Questions (v0.1 -> v0.2)
1. Bone Knight vs Wraith damage tuning -- needs Phase 2 playtest.
2. Mass Sacrifice burst formula -- linear (10 dmg per skeleton) vs scaling (cube root) -- TBD playtest.
3. Cross-class corpse consumption order -- if Hexer and Ravager both want same corpse, who wins? **Default: first-frame consumer wins, no double-tap.**

## Numerical Tuning Targets
- Mass Sacrifice burst (8 skeletons): ~600-800 damage (kill-shot tier on basic mob group).
- Sacrifice Charge fill rate: full charge from 0 in ~10s of active sacrifice farming (combat).
- Corpse Field damage (passive aura + Frenzied bonus): tunable, target ~5-10% Ravager DPS bonus.

## Versioning
- v0.1 (2026-04-30): R4 lock. Hard caps applied. Phase tags locked.
- v0.2 (target after Phase 2 Summoner playtest): tune numerical caps + ulti formula.

