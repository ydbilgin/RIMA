# RIMA Progression Plan v1 - Codex Review

**Basis:** `STAGING/PROGRESSION_PLAN_v0_DRAFT.md`
**Status:** v1 CODEX - open questions resolved, implementation not started.
**Scope:** map fragment progression + reward + room type + threshold + rune integration.

---

## 0. Codex Review Verdict

| Section | Verdict | Reason |
|---|---|---|
| 2a Threshold karar | **NEEDS REVISION** | A2 and C1 are both valid, but RIMA needs one canonical threshold language. Use A2 as base, C1 as overlay/state only. |
| 2b Room Type Mapping | **NEEDS REVISION** | 9 room variants are viable, but Merchant/Corridor should not imply reward doors or drops. Boss needs lock/key state separation. |
| 2c Reward Drop Canonical | **PASS** | 8-drop set covers run, meta, healing, risk, progression, and boss gating cleanly. |
| 2d Map Fragment Progression | **NEEDS REVISION** | "Every room" conflicts with Karar #149 because sub-rooms cannot pay rewards. Fragment must drop at macro-node final reward only. |
| 2e Rune Sistemi | **NEEDS REVISION** | Rune concept is good, but needs an explicit data/modifier layer instead of hardcoding per skill. |
| 2f Echo Imprint Cascade Integration | **NEEDS REVISION** | Theme enriches the system, but death = fragment loss is too punitive for map reveal progression. Convert loss into temporary corruption/obscure. |

**Counts:** PASS 1 / NEEDS REVISION 5 / FAIL 0.

---

## 1. Locked Decisions

### LOCK 1 - Threshold Visual

**Decision:** Use **A2 Imprint Scar / Floor Rift** as the canonical room threshold.

**Reason:** RIMA's current door spec and Act 1 rift accent inventory already point to a gravity-aligned scar/rift language. A2 is floor-native, avoids Hades framed-door risk, and fits the 35 degree iso floor read. C1 Scar Compass should be reused as an **overlay state**, not the base doorway.

**CHANGED:** v0 treated A2 vs C1 as open. v1 locks A2 base + C1 directional overlay.

**Implementation meaning:**
- Base sprite: floor scar/rift threshold.
- Direction/readability overlay: compass needle/ring only when the exit needs route choice clarity.
- Locked/active/final states: shader + overlay state, not separate full asset sets.

### LOCK 2 - Room Type Variant Strategy

**Decision:** Use **1 canonical A2 threshold base sprite + shader/material overlays** for room variants.

**Reason:** 9 authored threshold sprites are not needed for MVP. A shader-driven palette/accent approach is realistic for color, pulse, emission, distortion, and small symbol overlays. Shape changes must stay limited to avoid needing 9 new sprite gens.

**CHANGED:** v0 implied 9 visually distinct variants. v1 limits variant difference to color, emission, symbol, pulse cadence, and optional small overlay sprite.

| Room Type | Threshold variant | Door/reward hint | Floor drop rule |
|---|---|---|---|
| Combat | Cyan scar, low pulse | Echo Essence | Macro encounter final only |
| Elite | Cyan scar + heavier sigil overlay | Memory Shard / Rune chance | Macro encounter final only |
| Boss | Sealed scar + lock sigil | Boss Key state | Boss clear only |
| Chest | Gold fleck overlay | Gold / Shard | Node clear only |
| Merchant | Warm trade mark, no threat pulse | Merchant marker | No automatic drop |
| Forge | Ember secondary + rune mark | Skill Rune | Node interaction / clear |
| Event | Pale asymmetric flicker | Event marker | Event-defined |
| Curse | Red-black fray + warning pulse | Curse Stone | Choice reward only |
| Corridor | Faint scar, no icon | Transit only | No drop |

### LOCK 3 - Reward Drop Canonical

**Decision:** Keep the 8-drop canonical set.

**Reason:** The set is complete and non-overlapping if each item has one role.

| Drop | Locked role |
|---|---|
| Echo Essence Orb | Run currency / skill energy economy |
| Memory Shard | Meta upgrade currency |
| Gold Pile | Merchant economy |
| Skill Rune | Run-local skill modifier |
| Health Orb | Immediate sustain |
| Map Fragment | Map reveal progression token |
| Curse Stone | Optional high-risk modifier pickup |
| Boss Key | Boss gate unlock / act progression gate |

**CHANGED:** Boss Key is explicitly not a Map Fragment abstraction.

### LOCK 4 - Map Fragment Progression

**Decision:** Map Fragments drop from **macro node completion**, not every sub-room.

**Reason:** Karar #149 locks Combat/Elite as 3-5 sub-room sequences with final reward only. Fragment drops per sub-room would violate that lock and inflate progression pacing.

**CHANGED:** v0 "each room clear gives 1 fragment" becomes "each macro node clear can award fragment by room type".

| Node type | Fragment rule |
|---|---|
| Combat | 1 fragment on final sub-room clear |
| Elite | 1 guaranteed fragment + higher rune/shard chance |
| Boss | Boss Key + map reveal milestone, no normal fragment count abuse |
| Chest | 1 fragment or shard/gold bundle |
| Merchant | None by default |
| Forge | None by default unless event offers map craft |
| Event | Conditional |
| Curse | Optional trade: gain fragment with curse |
| Corridor | None |

**Reveal rule:** 5 fragments reveal the next route band: upcoming macro nodes, connections, and reward icons.

**Sub-room integration detail:** `SubRoomSequenceController` or equivalent reward gate must emit progression rewards only when `isFinalSubRoom == true`. Internal transitions must not call DungeonGraph reward/map progression.

### LOCK 5 - Rune System

**Decision:** Rune = **run-local passive skill modifier**, 3 equipped slots for MVP.

**Reason:** 3 slots are enough to create build identity without overwhelming the 48-skill bank while the bank is still under revision.

**CHANGED:** v0 had the right slot count but no integration model. v1 locks a data-driven modifier layer.

**Drop rule:**
- Forge: primary rune source.
- Elite: rare rune drop chance.
- Boss: class-specific rare/mythic rune chance.
- Combat/Chest: no default rune drop in MVP.

**Rarity:** Common / Rare / Mythic.

**Skill code integration:**
- Add data layer later: `SkillRuneDefinitionSO` with target tags, target skill id/class, modifier type, numeric payload, rarity.
- Add runtime layer later: `SkillRuneLoadout` on player with 3 slots.
- Skills query modifiers at execution time instead of embedding rune conditionals per skill.
- Warblade spotcheck supports this: `IronCounter` exposes tunable fields (`parryWindow`, `counterRange`, `baseDamage`, `damageMultiplier`, `rageOnCounter`, `stunDuration`) and `BladeRush` exposes mobility/damage/knockback fields. Runes should modify these via a common modifier query, not by rewriting skill logic.

**Example rune targets:**
- Iron Counter: +parry window, +counter range, rage gain on counter, stun duration.
- Blade Rush: +charge duration, +hit radius, knockback conversion, damage scalar.
- Elementalist/Ranger/Shadowblade/Ronin: same pattern through skill tags, not one-off branches.

### LOCK 6 - Echo Imprint Cascade

**Decision:** Echo Imprint should flavor the progression system, but **death must not remove earned Map Fragments** in MVP.

**Reason:** Removing map reveal progress after death punishes exploration and makes the run map feel unstable. The theme is stronger if death corrupts visibility rather than deleting earned tokens.

**CHANGED:** v0 death = 1 fragment lost becomes death = revealed map partially obscured until next macro clear.

**Locked behavior:**
- Clear = imprint gained.
- Fragment = stable memory piece.
- Death = temporary memory distortion: some revealed node reward icons become obscured or noisy.
- Next macro clear repairs one distortion.

---

## 2. Seven Open Questions - Resolved

1. **Threshold choice:** **A2 Imprint Scar** should be selected. C1 Scar Compass should be used as overlay because it provides direction clarity, but it should not be the base doorway.
2. **Map Fragment drop rate:** **Macro node completion only.** Combat/Elite final sub-room, Chest/Event/Curse conditional. No sub-room drops.
3. **Rune sistem:** **3 slot MVP.** Drop source Forge primary, Elite rare, Boss rare/mythic. Combat filler drop yok.
4. **Curse Stone:** **Optional high-risk high-reward modifier.** Not a permanent debuff; it should be a run-local curse. Permanent punishment conflicts with meta progression.
5. **Echo Imprint integration:** **Death fragment loss yok.** Death revealed-map distortion yapmalı; earned fragments kalıcı kalmalı.
6. **Universal shader approach:** **Realistic but limited.** Color/emission/pulse/symbol overlays are realistic; 9 different silhouettes cannot be solved by shader alone.
7. **Boss Key vs Map Fragment:** **Keep separate.** Boss Key is a gate/progression token; Map Fragment is an information/reveal token. Merging them blurs economy and route clarity.

---

## 3. Revised System

### 3a. Threshold Production

Use one A2 floor scar/rift threshold base for all room exits.

States:
- Dormant: faint scar, no reward pulse.
- Preview: reward icon/symbol visible.
- Active: cyan pulse and interaction affordance.
- Locked: sealed sigil overlay.
- Final/Boss: larger seal + boss key marker.

Asset plan:
- Use existing compact sheets for concept direction.
- No new threshold generation for MVP unless A2 source cannot be extracted cleanly.
- C1 compass ring becomes reusable overlay for direction-explicit exits.

### 3b. Room Type Mapping

Room type communicates through four layers:
- base A2 scar sprite
- shader color/emission
- small symbol overlay
- reward icon when applicable

Do not use automatic floor drops for Merchant or Corridor. Do not display reward icons for pure transit exits.

### 3c. Reward Canon

The 8 drops remain canonical. Reward source must be macro-level:
- Combat/Elite sub-room sequence: final sub-room reward only.
- Non-combat single node: node resolution reward.
- Boss: boss clear reward.
- Corridor: no reward.

### 3d. Map Fragment Progression

Fragment count is information economy, not boss unlock economy.

MVP pacing:
- 5 fragments reveal next route band.
- Elite and Chest accelerate reveal.
- Curse can offer "take curse, gain fragment".
- Death obscures revealed reward icons, not fragment count.

### 3e. Rune System

Rune MVP is a run-local modifier loadout:
- 3 slots.
- Skill tag and class targeting.
- Numeric modifiers, no behavior rewrites for v1.
- Behavior-changing runes are Phase 2 after Skill Bank V1 settles.

Minimum modifier families:
- Damage scalar.
- Cooldown scalar.
- Radius/range scalar.
- Duration scalar.
- Resource gain/cost scalar.
- Status duration/stacks scalar.

Warblade integration target:
- Start with Iron Counter and Blade Rush because the serialized knobs are obvious and low-risk.
- Keep modifiers external to skill classes where possible.

### 3f. Echo Imprint Cascade Integration

Theme terms:
- Fragment = stable memory.
- Reward = imprint from a resolved node.
- Death = distortion.
- Reveal repair = memory restabilized after next macro clear.

This enriches progression without stealing rewards the player already earned.

---

## 4. Production Cost Update

| Item | v1 cost | Note |
|---|---:|---|
| Threshold base | 0-1 batch | Existing A2 sheet likely enough; only regenerate if extraction fails. |
| C1 compass overlay | 0 | Existing sheet/showcase can guide overlay extraction. |
| Reward drops | 0 | Sheet 3 covers 8 drops. |
| Map fragment mark/UI | 0 | Sheet 4 covers Fragment Map/Tapestry direction. |
| Room variants | 0 gen + shader/code | Shader/material overlays, not new sprites. |
| Rune icon set | 1 batch optional | Needed only when rune pickup/UI moves to implementation. |
| Boss key | 0 | Sheet 3 already includes it. |
| Sub-room reward gate | code task later | No asset gen. |
| Rune data/runtime layer | code task later | No asset gen. |

**CHANGED:** v0 "0-1 batch" remains correct, but the optional batch is specifically rune icons. Threshold regeneration is fallback only.

---

## 5. Needs Revision List For Orchestrator

1. Replace every-room fragment wording with macro-node completion wording.
2. Lock A2 as threshold base and demote C1 to overlay.
3. Remove death fragment loss; use temporary revealed-map distortion.
4. Add `isFinalSubRoom` reward/progression guard to the future sub-room implementation spec.
5. Define rune implementation as data-driven modifiers before any skill-specific edits.
6. Limit shader variant promise to palette/emission/pulse/symbol overlays.

---

## 6. Next-Step Dispatch Recommendations

1. **Serial - Design signoff:** Orchestrator approves A2 base + C1 overlay + no death fragment loss.
2. **Serial after signoff - Code spec:** dispatch code planning for reward/progression gate around Karar #149 final sub-room only.
3. **Parallel after signoff - Shader work:** dispatch Universal Threshold Variant shader/material prototype using one base sprite and 9 config presets.
4. **Parallel after signoff - Asset extraction:** dispatch asset task to extract A2 threshold, C1 overlay, Sheet 3 drops, Sheet 4 fragment mark into clean sprites.
5. **Serial after Skill Bank decision - Rune implementation:** dispatch data/runtime rune layer after 48-skill bank reclassification is settled.

---

## Final v1 Decision

Proceed with **A2 Imprint Scar as the canonical threshold**, **macro-node-only rewards/fragments**, **separate Boss Key and Map Fragment economies**, **3-slot run-local rune modifiers**, and **Echo Imprint distortion instead of fragment loss**.
