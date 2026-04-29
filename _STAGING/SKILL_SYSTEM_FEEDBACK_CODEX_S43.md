# RIMA Skill System Feedback — Codex S43

**Date:** 2026-04-28  
**Author:** Codex  
**Scope:** Character skills, mob skills, boss skills, cross-class consistency.  
**Output type:** Design feedback only. No source/design files were changed.

## Sources Read

- `CURRENT_STATUS.md`
- `SYSTEM_MAP.md`
- `TASARIM/GDD.md`
- `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
- `TASARIM/CROSS_CLASS_SKILL_MATRIX.md`
- `TASARIM/COMBAT_ROSTER.md`
- `TASARIM/BOSS_DESIGN.md`
- `_STAGING/SKILL_REVIZYON_PLANI.md`
- `_STAGING/MOB_BOSS_REDESIGN_S42.md`
- `Assets/Data/Skills/*.asset`
- `Assets/Data/CrossClass/*.asset`

## Executive Verdict

RIMA's strongest current direction is:

> Hades-style room combat + MMORPG class fantasy + draft-driven build mutation.

The skill system should therefore avoid three things:

- passive MMO filler that only says "+damage / +crit / +buff"
- slow channeling skills that stop the player from moving for too long
- enemy/boss attacks that are generic melee auto-attacks instead of readable skill tests

The most current and compatible direction is `_STAGING/SKILL_REVIZYON_PLANI.md` + `_STAGING/MOB_BOSS_REDESIGN_S42.md`.  
`TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`, `TASARIM/CROSS_CLASS_SKILL_MATRIX.md`, `TASARIM/COMBAT_ROSTER.md`, and `TASARIM/BOSS_DESIGN.md` still contain older mechanics and should be treated as partially stale until synced.

## Global Design Rules I Would Lock

| Rule | Reason |
|---|---|
| Every active skill must change position, target priority, resource state, or enemy state | Prevents generic MMO filler |
| Every class needs at least one cursor/aim/ground-target skill | Draft decisions feel more tactical |
| Every class needs one emergency/mobility answer, but not all of them should be dash clones | Keeps class identity readable |
| Enemy auto-attack should not be the main threat | Supports RIMA's telegraph-window-punish combat |
| Boss phases should introduce a new decision, not just faster versions | Prevents HP sponge pacing |
| V Burst must express the class thesis | Avoids "big damage button" sameness |

## Document Drift Problems

### Drift 1 — Active skill plan vs main class document

`_STAGING/SKILL_REVIZYON_PLANI.md` is newer and better aligned with RIMA's current tone.  
`TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` still keeps older versions:

- Shadowblade still has CP/stealth rogue-style skills in the main doc, while S41 redesign turns it into Rift Scar / phase-position gameplay.
- Ranger still has Aimed Shot, Flare, Tethering Arrow, Explosive Trap in the main doc, while S41 redesign pivots toward Rift Arrow, Pinning Shot, Bone Trap, Marked Detonate, Predator's Mark.
- Elementalist in the main doc is still Fire/Frost/Arcane-heavy; S41 wants Fire/Frost/Radiance with Trinity Storm.
- Gunslinger still has Critical Shot / Dead Eye as buff-ish skills; S41 replaces them with Deadshot / Rift Grenade.

**Recommendation:** Treat `_STAGING/SKILL_REVIZYON_PLANI.md` as the active design source and schedule a doc sync pass before implementing more skill assets.

### Drift 2 — Cross-class matrix uses stale skill names

`TASARIM/CROSS_CLASS_SKILL_MATRIX.md` still references:

- Shadowblade: Vanish, Hemorrhage, Kidney Shot, Fan of Knives, Evasion
- Ranger: Aimed Shot, Concussive Arrow, Explosive Trap, Flare, Tethering Arrow
- Gunslinger: Critical Shot, Dead Eye
- Ravager: Battle Cry / Intimidating Shout

These conflict with the S41 redesign.

**Recommendation:** Do not implement more cross-class assets until the matrix is rebuilt using the updated 8-exportable skill pools.

### Drift 3 — Implemented Unity skill assets are still early Warblade-only

Current `Assets/Data/Skills/*.asset` are still old/early prototype names:

- `Skill_CrushingBlow`
- `Skill_RiftStrike`
- `Skill_WhirlwindSlash`
- `Skill_WarStomp`

These do not fully match the newer Warblade list:

- Crippling Blow
- Sunder Mark
- Earthsplitter instead of War Stomp
- Bladestorm as V Burst

**Recommendation:** Before building more art, freeze Warblade's 6-skill Faz 1 implementation list and rename assets to match the current skill language.

## Character Skill Review

### Warblade

**Status:** Strong core, needs naming/shape cleanup.

Current fantasy works: approach, lock, armor break, execute. It fits Faz 1 and teaches melee fundamentals.

Keep:

- Iron Charge
- Crippling Blow
- Gravity Cleave
- Sunder Mark
- Iron Counter
- Death Blow
- Bladestorm V Burst

Change:

- `War Stomp` should become `Earthsplitter`.
- Reason: War Stomp is generic MMO language and overlaps visually with Shockwave Slam / Seismic Stomp / Quake Slam enemies.
- New behavior: line crack forward, small knock-up along fissure, steel/ember fracture visual.

Risk:

- Too many Warblade skills are "CC then punish". Keep that identity, but make each CC shape different:
  - Iron Charge = line engage
  - Gravity Cleave = radial pull
  - Earthsplitter = forward fissure
  - Iron Counter = timing punish

### Elementalist

**Status:** Needs full sync to S41 redesign.

The main doc's Elementalist is too close to generic mage: Fireball, Meteor, Chain Lightning, Blizzard, Arcane Surge. That is functional but not RIMA-specific enough.

Use S41 direction:

- Fire = aggressive burst / DoT ignition
- Frost = control geometry / walls / slow
- Radiance = prism/beam/pillar state, not generic holy magic

Recommended changes:

- `Prism Lance` → `Prism Beam`: line-aim channel, cursor-facing beam.
- `Halo Fracture` → `Frost Wall`: geometry skill, not another AoE pop.
- `Sunshard Torrent` → `Solar Flare`: cone burst with visual identity.
- `Luminary Surge` → `Radiant Pillar`: self/nearby pillar, readable anchor.
- `Inferno` V Burst → `Trinity Storm`: all three elements together.

Why:

- RIMA needs skill silhouettes, not just element names.
- Trinity Storm better expresses "rhythm mastery" than single-element Inferno.

### Shadowblade

**Status:** Old rogue kit should be replaced.

Old Shadowblade is too close to WoW rogue:

- Backstab
- Hemorrhage
- Kidney Shot
- Vanish
- Fan of Knives
- Evasion

These are readable but generic and overlap with common MMO vocabulary.

Use the S41 Rift Scar redesign:

- Veil Strike
- Phase Step
- Backstab Mark
- Death Mark
- Veil Burst
- Sever
- Smoke Veil
- Shadow Pin
- Twin Carve
- Wraith Form

One adjustment:

- Do not overuse "Mark". Ranger also owns mark gameplay.
- Shadowblade marks should be assassination windows, not long-range hunting marks.

Recommended rename:

- `Backstab Mark` → `Veil Mark` or `Rift Scar`
- `Death Mark` → `Delayed Scar`

Reason:

- Keeps Ranger's mark identity cleaner.

### Ranger

**Status:** S41 redesign is much stronger than the main doc.

Old Ranger has too much classic hunter:

- Aimed Shot
- Flare
- Explosive Trap
- Tethering Arrow
- Volley

The redesigned Ranger should be a positional predator:

- Rift Arrow
- Pinning Shot
- Hunter's Step
- Bone Trap
- Marked Detonate
- Predator's Mark
- Sweep Volley
- Rift Step
- Spirit Bow V Burst

Change:

- `Marked Detonate` should not be just "press button, marks explode".
- Better: `Cull Signal`: mark detonation scales with distance or angle.

Reason:

- Distance and positioning are Ranger's actual gameplay identity.

Mob interaction:

- Ranger should hard-counter Shard Walker and Relic Caster if positioned well.
- Chain Warden and Seam Crawler should counter Ranger's comfort zone.

### Ravager

**Status:** Strong fantasy, but avoid Warblade overlap.

Ravager works when it is not "another heavy melee".

Keep:

- Fury from taking damage
- Blood Pact / HP trade
- Carnage Spin
- Frenzied Leap
- Reckless Swing
- Death Wish
- Berserk Mode

Change:

- `Bloodied Roar` should not be a pure buff.
- Better: Roar forces nearby enemies to commit to their next telegraphed skill faster, creating a danger/reward window.

Reason:

- Ravager should invite damage and then exploit the chaos, not just gain stats.

Avoid:

- Too many red AoE spins. Carnage Spin, Berserk Mode, Blood-Drunk Leap and Bloodlust Strike need distinct shapes.

### Ronin

**Status:** Good identity, but naming needs clarity.

Ronin should stay precise, low-VFX, timing-based.

Keep:

- Quickdraw Slash
- Iaido Stance
- Counter Draw
- Flash Draw
- Void Cleave
- Mugen No Kiri

Change:

- `Mille Feuille Cut` → `Soken-giri` is fine, but be careful with obscure naming.
- If player readability matters, use `Soken Cut` or `Thousand-Slice Draw`.

Risk:

- Wind Step, Phantom Step, Haste Dash can blur together.

Shape separation:

- Haste Dash = straight slide
- Wind Step = three directional cuts
- Phantom Step = afterimage deception

### Gunslinger

**Status:** Good class, but remove static sniper/buff language.

Gunslinger should be run-and-gun, not stand-and-aim.

Keep:

- Rift Dash
- Quickdraw
- Fan the Hammer
- Suppression Fire
- Ricochet
- Reload Dance
- Full Metal Storm

Change:

- `Critical Shot` → `Deadshot`
- `Dead Eye` → `Rift Grenade`
- `Bullet Rain` → `Cursor Storm`

Reason:

- Critical Shot and Dead Eye are generic stat-skill language.
- Rift Grenade gives the class a ground-target zone and helps room-control.

Risk:

- Gunslinger + Ranger overlap.

Separation:

- Ranger = distance, trap, mark, measured shot.
- Gunslinger = lateral movement, heat, reload rhythm, short windows.

### Brawler

**Status:** Strong, but must avoid becoming "punch Warblade".

Brawler's identity should be rhythm/tempo, not raw melee damage.

Keep:

- Mach Punch
- Weave
- Counter Blow
- Aerial Rave
- Guard Break
- Overdrive

Change:

- `Rush Combo` → `Combo Chain`
- `Momentum Strike` → `Pivot Hook`

Add/clarify:

- Weave must be a core input, not just flavor.
- Perfect Weave should be the main skill expression.

Enemy relation:

- Brawler should be good into Chain Warden if timing is clean.
- Brawler should be punished by Decay Anchor / Null Knight later because tempo breaks hurt him.

### Summoner

**Status:** Good macro identity, but needs active command pressure.

Summoner should not become passive pet AI.

Keep:

- Raise Skeleton
- Summon Golem
- Corpse Explosion
- Blood for Power
- Mass Sacrifice
- Bone Shield
- Army of the Dead

Change:

- `Rally Cry` → `Command Beacon`

Reason:

- Rally Cry is a generic MMO buff.
- Command Beacon forces positioning and active control.

Add:

- At least two skills should require cursor placement:
  - Command Beacon
  - Soul Siphon Totem

Boss concern:

- Echo Twin / Mirror bosses must not copy Summoner by spawning too many adds; it will become visual noise. Boss mirror should copy "command line" and sacrifice windows, not full pet count.

### Hexer

**Status:** Strong concept, but rotation-lock risk is high.

Hexer has one of the best identities: patience, stacks, payoff.

Keep:

- Corruption
- Agony
- Pandemic
- Hexblast
- Enervate
- Mass Hex
- Hex Overload
- Hex Cascade

Change:

- `Soul Bargain` should probably be replaced by `Blight Sigil`.

Reason:

- Soul Bargain is meta/stat-like HP trade.
- Blight Sigil gives ground control, stack routing, and visual identity.

Risk:

- "Build to 10 stacks then Hexblast" can become too linear.

Fix:

- Add alternate payoffs:
  - 4-6 stacks = control mode
  - 7-9 stacks = vulnerability mode
  - 10 stacks = detonation mode
- Skills should interact differently with these bands.

## Mob Skill Review

### Active source

Use `_STAGING/MOB_BOSS_REDESIGN_S42.md` over `TASARIM/COMBAT_ROSTER.md`.

The S42 redesign correctly implements the user requirement:

> no ordinary melee auto-attack mobs; every mob has characteristic skill behavior.

### Fracture Imp

Keep:

- Rift Lunge
- Death Splatter

Adjustment:

- Death Splatter slow should not stack infinitely.
- Cap goo slow at one active debuff per player.

Reason:

- 4 imps dying together could create unfair floor glue.

### Shard Walker

Keep:

- Triple Shard
- Fracture Burst

Adjustment:

- Triple Shard should have a minimum range deadzone.

Reason:

- Warblade dash-in should feel like a valid answer. If Walker can shotgun at point blank, the intended counter fails.

### Seam Crawler

Keep:

- Submerge
- Burst Strike

Adjustment:

- Submerge invulnerability needs a strict max duration and visible distortion.
- Do not allow chain-submerge if the player is waiting correctly.

Reason:

- Invisible/untargetable enemies can feel unfair if the tell is weak.

### Penitent Bruiser

Keep:

- Anti-Heal Aura
- Penitent Surge

Change:

- Slow melee swing should remain secondary pressure, not its main damage source.

Risk:

- Aura + Augur + Walker can overcompress Warblade's options. Use this composition only after player has learned dodge windows.

### Chain Warden Echo

Keep:

- Triple Chain
- Chain Pull

Critical note:

- Chain Pull being dash-immune is good for ranged punishers, but Warblade needs a non-dash answer.

Recommendation:

- Allow Iron Counter / parry / timed attack to break or reduce pull.

Reason:

- "Dash does not solve it" is good. "Nothing solves it except eat it" is bad.

### Relic Caster

Keep:

- Summon Shardling
- Aegis Mark

Adjustment:

- Aegis Mark should have a visible shield-break reward.

Example:

- Breaking Aegis staggers both shielded target and caster for 0.5s.

Reason:

- Gives non-burst builds a meaningful route.

### Riftbound Augur

Keep:

- Mark of Folly
- Time Shudder

Change:

- "Sight-break by turning away" is conceptually interesting but may be hard in top-down controls.

Better implementation:

- Break line-of-sight with obstacle, dash through Augur, or face away for 0.3s using movement vector.

Reason:

- Input readability matters more than novelty.

### Hollow Hulk

Keep:

- Quake Slam
- Cavity Pulse
- Fracture Charge

Adjustment:

- This should fully replace old `Ruin Hulk` false-threat in Faz 1 if S42 is accepted.

Reason:

- The old false-threat idea is clever, but Faz 1 needs skill-test enemies more than fake-out enemies. False threats can return later as room literacy jokes.

## Boss Skill Review

### Penitent Sovereign

Use `_STAGING/MOB_BOSS_REDESIGN_S42.md` 3-phase version over `TASARIM/BOSS_DESIGN.md` 2-phase version.

Reason:

- The 3-phase version better matches Hades-style learn → pressure → desperation pacing.
- It also makes Act 1 boss a true milestone before secondary class unlock.

Recommended final kit:

#### Phase 1

- Chain Whip
- Penitent Surge
- Shackle Cast

Good:

- Teaches line dodge, radial dodge, control effect.

Change:

- Remove extra 180-degree whip from the older boss doc.

Reason:

- Phase 1 should be clean and teachable.

#### Phase 2

- Fracture Strike
- Chain Detonation
- Shackle Cast carry-over
- Rift Tear arena hazard

Good:

- Adds arena management.

Risk:

- Shackle Cast + Rift Tear + Chain Detonation can hard-lock Warblade if timings overlap.

Rule:

- Never allow Shackle Cast to fire inside 0.8s before Chain Detonation explosion.

#### Phase 3

- Fracture Charge
- Sovereign's Wrath
- Faster Chain Detonation
- Faster Fracture Strike

Good:

- True desperation.

Change:

- Sovereign's Wrath safe circle should not always be center if Rift Tear is center.

Better:

- Safe circle alternates center / edge / boss-behind arc.

Reason:

- If center is both safe and hazardous, it becomes ambiguous instead of skillful.

### Echo Twin

Current concept is good, but dangerous to implement too early.

Risk:

- Mirroring all possible primary/secondary combinations is scope explosion.

Recommendation:

- First version should mirror only player behavior categories:
  - high mobility
  - ranged spam
  - heavy melee
  - summon/zone

Not exact class kits.

### Fracture Sovereign

Good late-game boss concept, but platform mechanics are a separate system.

Recommendation:

- Do not design skill balance around this boss until movement/platform/hazard tech exists.

### Architect

Strong narrative boss, but some mechanics risk feeling unfair:

- Build Breaker disabling most-used skill for 8s
- Silence blocking all skills for 4s

Adjustment:

- Build Breaker should disable only one slot and telegraph it clearly.
- Silence should leave LMB/RMB/Dash usable or it stops being action combat.

## Cross-Class Review

### Current problem

The cross-class matrix is based on stale skill names and old class mechanics.

### Recommendation

Do not patch individual rows. Rebuild the matrix from a normalized table:

```text
Class
  - 12 skills
  - 8 exportable secondary skills
  - 3 class verbs
  - forbidden overlaps
```

Example class verbs:

| Class | Verbs |
|---|---|
| Warblade | engage, break, execute |
| Elementalist | switch, shape, detonate |
| Shadowblade | phase, scar, collapse |
| Ranger | mark, trap, detonate |
| Ravager | suffer, trade, frenzy |
| Ronin | wait, draw, punish |
| Gunslinger | slide, shoot, reload |
| Brawler | weave, combo, launch |
| Summoner | command, sacrifice, raise |
| Hexer | stack, spread, blast |

Then cross-class skills should use one verb from each side.

Example:

- Warblade + Ranger = engage + trap
- Ranger + Warblade = mark + execute
- Shadowblade + Elementalist = scar + detonate
- Summoner + Hexer = command + stack

## Highest Priority Changes

1. Sync `SINIF_VE_SKILL_KARAR_BELGESI.md` to `_STAGING/SKILL_REVIZYON_PLANI.md`.
2. Rebuild `CROSS_CLASS_SKILL_MATRIX.md` after the class sync, not before.
3. Replace War Stomp with Earthsplitter in all active Warblade docs/assets if accepted.
4. Replace generic buff skills:
   - Elementalist Inferno → Trinity Storm
   - Gunslinger Dead Eye → Rift Grenade
   - Summoner Rally Cry → Command Beacon
   - Hexer Soul Bargain → Blight Sigil
   - Ravager Battle Cry/Intimidating Shout → Blood Pact/Bloodied Roar with active tradeoff
5. Use S42 mob/boss redesign as the active enemy baseline.
6. For Penitent Sovereign, move to the 3-phase version but add overlap-safety rules.

## Implementation Warning

Do not implement every redesigned skill immediately. The practical order should be:

1. Warblade Faz 1 playable set, 6 skills max.
2. Act 1 mob skill behaviors, 5-6 mobs max.
3. Penitent Sovereign 3-phase prototype.
4. Only then sync all 10 class docs.
5. Cross-class matrix rebuild after class docs are stable.

Reason:

The design is now rich enough to overproduce. The next risk is not lack of ideas; it is implementing stale names or too many untested mechanics.

## Final Recommendation

RIMA's skill direction is good, but the live docs are split across eras.  
The game logic should follow the newer philosophy:

- player skills = class verbs + positional/resource decisions
- mob skills = habit breakers with readable tells
- boss skills = phase lessons, not damage spam

If a skill does not ask the player to move, aim, time, choose a target, spend a resource, or create a build interaction, it should be renamed, redesigned, or removed.
