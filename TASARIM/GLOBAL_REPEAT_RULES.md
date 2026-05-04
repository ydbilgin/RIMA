# GLOBAL REPEAT RULES
**Date:** 2026-04-30 | **Source:** Claude R4 final lock (Codex R4 ACCEPT_WITH_CHANGES)
**Status:** v0.1 lock. Locks recurring patterns: counter / movement / execute / zone / ulti / toggle.

## Why This Doc Exists
Cross-class skill design produces recurring shapes (counters, movement, executes). Without explicit rules, classes start to overlap. This doc locks the **separation rules** for each repeating skill family so identity stays distinct.

---

## 1. Counter Skills

Three classes have counter mechanics. They must read differently.

| Class | Counter Type | Input/Trigger | State Output |
|---|---|---|---|
| Warblade | **Absorb / Break** | Hold/timed block on incoming melee | Broken (on attacker) |
| Ronin | **Pre-Draw Timing** | Frame-perfect parry during Draw Window | Opened (on attacker) |
| Brawler | **Whiff / Body Movement** | Dodge into enemy whiff frame | Off-Balance (on attacker) |

### Distinction Rules
- **Warblade absorb is reactive,** uses shield/weapon block frames, scales with armor stat.
- **Ronin pre-draw is offensive timing,** consumes Tension on miss, rewards Tension on hit (high skill ceiling).
- **Brawler whiff is movement-based,** requires positioning, no input parry button -- evade-in.

### Forbidden Overlaps
- No "absorb + parry" stacked on one class.
- No counter that produces another class's state (Warblade counter cannot produce Opened).
- No counter that bypasses Perfect Condition for ulti charge (counter fills resource normally, not bonus).

### Counter Ulti Behavior
- All three counters can be **Ulti-toggled** (Shift+key) to upgrade the empowered cast.
- Empowered counter on Perfect Condition triggers ulti version, then **relocks** per Ulti Toggle rule.

---

## 2. Movement Skills (Option C Locked)

### Space (universal)
- **Short dash, no state, no damage, resource-neutral.**
- All classes share: Space = ~3 tile dash, 0.4s i-frame, 1.5s CD.
- Space cannot apply state, cannot trigger Perfect Condition, cannot be Ulti-toggled.

### Skill Movement (per-class)
- Skill movement requires **state interaction** to be valid (Codex R4 cross-class compliance).
- Examples: Shadowblade Veil Flicker (Phased Through state), Ronin Iaido dash (Afterimage), Gunslinger Reload Roll (Phase 2: produces Exposed Line on exit).
- **Build Cap:** **max 1 skill movement per build.** Cannot stack 2 dashes on top of Space.
- **No CD reset loops:** state apply cannot reset movement CD (e.g., Shadowblade Severance does not refresh Veil Flicker).
- **No i-frame stacking:** Space + skill movement i-frames cannot overlap (skill movement i-frame disabled if Space is on cooldown <0.5s).

### Movement Skill Ulti Rule
- Movement skill is Ulti-capable **only if** its empowered version becomes a major state route, not just safer movement.
- Per Codex R4: "Reload Roll as ulti-capable is not ideal -- it is utility and could become mandatory." -> **Reload Roll is NOT ulti-capable.**

---

## 3. Execute Gates

**Universal rule: NO HP<30% execute.** All executes require class-specific state gate.

| Class | Execute Skill | Gate (must hold) |
|---|---|---|
| Warblade | Death Blow | Broken / Sundered / Staggered target |
| Ranger | Final Strike | Marked + Trap-triggered (Trapped) |
| Gunslinger | Deadshot | Last Bullet / Perfect Reload / Exposed Line aim |
| Ronin | Flash Draw | Tension MAX (Draw Window open) |
| Shadowblade | Severance | Scar Collapse afterward (3+ Scars on target) |

### Universal Boss Rule
- **No execute on boss-tier.** All executes downgrade to high-damage burst on boss (50-70% of execute damage).
- Reason: boss design requires HP-bar pacing; instant execute breaks fight rhythm.

### Forbidden Patterns
- No execute that does not consume the gate state.
- No execute on missing-HP gates (banned per R3 lock).
- No multi-class execute chain (e.g., Severance cannot trigger Death Blow auto-cast).

---

## 4. Zone / Area Skills

Three categories of zone skill, must be distinguished:

| Zone Type | Definition | Examples |
|---|---|---|
| **Damage Zone** | Persistent ground area, ticks damage | Elementalist Element Trail, Fire Pyre |
| **Status Zone** | Persistent ground area, applies state on enter/tick | Ranger Wireline Trap (Snared+Marked), Hexer Curse Mist |
| **Buff Zone** | Persistent ground area, buffs caster/allies | Summoner Corpse Field (Frenzied bonus), Ronin Stillness (Tension passive) |

### Zone Stacking Rules
- **Same class zones do not stack** (Pyre + Pyre = single zone with refreshed duration, not double damage).
- **Cross-class zones stack** (Pyre + Curse Mist on same tile = both effects active).
- **Zone count cap:** max 6 active zones in scene at once. 7th zone dispels oldest.
- **Boss arena zones:** zones still apply but tick rate halved (0.5s vs 1s) to prevent zone-camping vs boss.

---

## 5. Ulti System (R4 Final Lock)

### Trigger Concept (Renamed)
- **Old:** "Class resource MAX = ulti."
- **New (R4):** **"Class resource at Perfect Condition triggers empowered cast."**
- Reason: Gunslinger Heat ZERO and Hexer Stack 10 don't fit MAX. Universal term needed.

### Per-Class Perfect Condition
| Class | Resource | Perfect Condition |
|---|---|---|
| Warblade | Rage | Rage MAX (100) |
| Ravager | Fury | Fury MAX (100) |
| Ronin | Tension | Tension MAX (100, opens Draw Window) |
| Shadowblade | Sever | Sever MAX (100) |
| Ranger | Focus | Focus MAX (100) |
| **Gunslinger** | Heat | **Heat ZERO** (perfect management state) |
| Elementalist | Convergence | Convergence Full (both Fire+Frost bars filled) |
| Summoner | Sacrifice Charge | Charge MAX (100, see SUMMONER_ECONOMY_RULES.md) |
| **Hexer** | Hex Stacks (target) | **Hex Stack 10 on target** (target-specific, not player resource) |
| Brawler | Charge | Charge MAX (100) |

### Per-Skill Toggle (Shift+Key)
- Each ulti-capable skill has independent lock/unlock state.
- **Default: Lock ON** (deliberate ulti use, no accidental burn).
- **Toggle persistence:** unlocked skill stays unlocked **until cast** OR **room clear**.
- **Relock rule (R4 NEW):** **after ulti cast, the skill auto-relocks.** Re-toggle Shift+key to unlock again.
- **Room start reset (R4 NEW):** all locks default ON at room entry.

### HUD Requirements (R4 Mandatory)
1. **Lock state icon:** lock symbol on skill slot when locked, glow when unlocked.
2. **Armed cue:** when resource hits Perfect Condition AND skill is unlocked -> distinct glow + audio cue.
3. **Confirmation cast:** ulti burn shows clear VFX/audio (not popup, no modal).
4. **Resource pulse:** at Perfect Condition, resource bar pulses regardless of lock state.

### Ulti-Capable Skills (R4 Final List)
| Class | Skill 1 | Skill 2 |
|---|---|---|
| Warblade | Death Blow | Iron Charge |
| Ravager | Bloodied Roar | Carnage Spin |
| Ronin | Flash Draw | Iaido Strike |
| Shadowblade | Severance | Veil Flicker |
| Ranger | Final Strike | Quiver Pulse |
| Gunslinger | Deadshot | Empty Mag Burst (R4: now applies Suppressed) |
| Elementalist | **Trinity Storm** | -- (Lightbreak is system trigger, not ulti slot) |
| Summoner | Mass Sacrifice (Phase 2) | -- (1 ulti acceptable Phase 1) |
| Hexer | Hexblast | Hex Cascade |
| Brawler | Pulverize finisher | Glass Strike |

---

## 6. Skill Test (6-Line Rule)

Every new active skill must pass:
1. **Verb** -- what does it do in one verb? (slash, ignite, pull, brand, etc.)
2. **State** -- what state does it produce or consume? (cross-class compliance check)
3. **Slot Reason** -- why does this earn an active skill slot vs being a passive/upgrade?
4. **Overlap** -- does it overlap with another class's identity? If yes, rename/redesign.
5. **Abuse** -- what's the abuse vector? What caps the abuse?
6. **Encounter Question** -- what mob/boss situation does it answer that no other skill answers?

R4 application: extra skill list filtered through this -- ~50% converted to passive/upgrade per Codex R4 Q7.

---

## 7. Pixel Art Constraint (Universal)

### Allowed Skill Components
- Caster animation (existing or new)
- Overlay VFX on enemy (existing hit-react, freeze, slide)
- Environmental VFX (impact decals, particles)
- Code-driven mob slide (transform lerp with nav-safe sweep)
- Camera shake + hit-stop
- Audio cues

### Forbidden Components
- Custom mob animation (no per-skill enemy reaction anim)
- Custom mob lift/throw/grapple/ragdoll/struggle anim
- Bespoke boss reaction anim per skill

### R4 Pixel-Art PASS_WITH_RISK Items
- **Brawler Wall Eat (was Wall Slam Combo):** must be slide + VFX + hit-react only. Fallback when no wall (Ground-Slammed) required.
- **Brawler Liver Shot (was Pin Strike):** freeze max 0.8s normal / 0.3s boss; no bespoke pinned anim.
- **Ranger Wireline Trap:** Snared = slow + line VFX, no struggle anim.
- **Elementalist Element Trail:** persistent ground VFX -- performance budget (max 6 zones, see Section 4).
- **Summoner Bone Tide:** asset/AI scope, not animation. Phase Later.

---

## 8. Cross-Class State Compliance (R4 FLAG Resolutions)

R4 flagged skills as "just damage" (no state interaction). All resolved:

| Skill | R4 FLAG | Resolution |
|---|---|---|
| Empty Mag Burst | "needs state" | Now applies Suppressed (Gunslinger public) + Last Bullet trigger |
| Backfire Shot | "no state" | Now applies Exposed Line (Gunslinger internal) + self Backfire DoT |
| Shockwave Fist | "needs Cracked/Off-Balance" | Now applies Off-Balance (Brawler internal) |
| Crimson Pact | "no enemy state" | Now produces Blood Debt (Ravager self-internal) + empowers Wounded application |
| Wound Echo | "no state" | Confirmed passive only (not cross-class skill, no state required) |
| Wind Read | "needs Opened" | On enemy whiff -> Opened (Ronin public, cross-class consumable) |

Rule: every active skill must produce or consume a defined state. Pure damage skills are forbidden unless flagged passive.

---

## 9. Phase Discipline (R4 Lock)

### Phase 1 (Demo) -- Active Now
- Warblade core (Death Blow gate, Iron Crush redesign, base movement)
- Cross-class state foundation: Broken / Sundered / Marked / Cracked
- Ulti Toggle UI + Perfect Condition framework
- Penitent Sovereign boss
- Shared systems: i-frame rules, zone count cap, HP/resource HUD

### Phase 2
- Other 9 classes' core skill set
- Most R4 new active skills (see CODEX_TASKS list)
- Passive skill upgrade tree
- Mass Sacrifice ulti (per Summoner Economy Rules)

### Later (Deferred R4)
- Brawler Wall Eat (collision/layout high-risk)
- Summoner Bone Tide + Soul Tax (economy heavy)
- Ronin Sheath Pressure (proximity tuning depth)
- Elementalist Element Trail (persistent VFX performance budget)

---

## 10. Forbidden Patterns (Universal)

1. **HP<X% execute** -- banned (R3 lock).
2. **Cooldown reset on state apply** -- banned (movement i-frame stacking abuse).
3. **Multi-class execute chain** -- banned (Severance cannot auto-trigger Death Blow).
4. **Ulti without Perfect Condition** -- banned (must hit class resource gate).
5. **Bespoke mob animation per skill** -- banned (pixel art constraint).
6. **Counter without input cost** -- banned (must consume Tension/positioning/timing).
7. **Zone stacking same-class for double damage** -- banned (refresh, not stack).
8. **Boss execute** -- banned (downgrade to high-damage burst).
9. **Pre-fill resource before combat** -- banned (combat-only fill rates apply).
10. **Per-class state collision** -- banned (state ownership lock per CLASS_STATE_CONTRACT.md).

---

## 11. Run Offer / Active Slot Routing (2026-05-03 Lock)

RIMA run progression uses Hades-style room reward offers, not a visible skill tree UI.

Canonical detail: `TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md`.

### Offer Categories

- New active skill
- Owned-skill upgrade
- Passive/echo rule
- Tag synergy
- Resource mod
- Risk offer

### Active Slot Gate

An active skill must create a button-worthy combat event. It must apply or consume a readable state, create a positional/zone question, move the player in a class-owned way, answer a specific encounter problem, or create visible resource-risk.

Pure damage, pure self-buff, pure future-cast improvement, and passive economy rules should route to passive/echo/upgrade offers unless redesigned to satisfy the 6-line rule.

### Production Gate

PixelLab concept sheets are mood/reference only. Production skill work must split caster animation, VFX/projectile, ground decal, impact, and state overlays. Per-skill bespoke enemy animation, grapple/struggle/ragdoll animation, boss-specific reaction animation, and baked character+enemy+VFX panels are forbidden.

---

## Versioning
- v0.1 (2026-04-30): R4 lock. Counter / movement / execute / zone / ulti / pixel constraint patterns locked.
- v0.1.1 (2026-05-03): Added Hades-style run offer / active slot routing lock.
- v0.2 (target after Phase 1 playtest): tune i-frame windows, zone count caps, ulti relock UI feedback.
- Future: as new classes enter Phase 2/Later, revisit forbidden patterns for new conflict cases.
