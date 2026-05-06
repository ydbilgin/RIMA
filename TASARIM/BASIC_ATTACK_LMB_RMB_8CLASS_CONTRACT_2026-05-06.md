# Basic Attack LMB/RMB 8-Class Contract
Date: 2026-05-06
Status: LOCKED 2026-05-06

## Architecture Decisions (LOCKED)

### BasicAttackProfile
Each class has a ScriptableObject (`BasicAttackProfile`) defining:
input bindings, step count, per-step timing, per-step damage scalar, animation clip refs,
hit shape, recovery, cancel rules, and a BasicAttackBehavior strategy reference.
PlayerAttackController consumes the profile only. No per-class if-branches. Missing profile = explicit error, no Warblade fallback.

### Ranged/Caster Architecture
Shared IBasicAttackState interface with: int CurrentStep, float StepWindow, void Advance(), void Reset().
Concrete implementations: MeleeChainState, CastRhythmState, ShotCadenceState, HeatGaugeState, MarkPulseState.
Each owns its internal counter but exposes the same surface to controller, animation, and VFX.

Elementalist uses internal castStep (not shared with Warblade's slash index) because its 3rd cast
is empowered on timing window, not on animation advance. Shared interface, separate internals.

### First Codex Task
Create BasicAttackProfile.cs (ScriptableObject), IBasicAttackState interface, MeleeChainState +
CastRhythmState implementations. Refactor PlayerAttackController to consume profile with no fallback.
Migrate Warblade to BasicAttackProfile_Warblade.asset as reference implementation before touching any other class.

### Extended Ideas (Not Yet Scheduled)
- Combo window as resource: skills can read comboWindowOpen bool, spend finisher for free cooldown or extend chain
- Whiff identity: each class's missed attack has a unique tell (Brawler over-rotates 0.1s self-stun, Gunslinger shell eject, Elementalist fizzle)
- Anchor frame: define one max-commitment frame per class; all timing/animation tuned around it
- Cross-class Imprint via rare Rift Gift: graft one step from another class's combo onto current class (curated whitelist of 8 imprints)

## Purpose

RIMA basic attacks must not feel like one shared default attack with different numbers.
Each playable class needs a class-owned LMB and RMB loop with weapon/body timing, short readable
animations, and meaningful combo rhythm.

Scope for this pass:
- Warblade
- Elementalist
- Shadowblade
- Ranger
- Ravager
- Ronin
- Gunslinger
- Brawler

Out of scope for this pass:
- Summoner
- Hexer

## Global Rules

- LMB is the class primary attack.
- RMB is the class secondary attack/outlet.
- LMB/RMB must be readable at RIMA isometric scale.
- Animation clips stay short: usually 3-6 frames per beat.
- Every beat needs windup, hit/release frame, and recovery.
- Heavy weapons feel slower but not sluggish.
- Light weapons feel fast and sharp.
- Ranged/caster attacks use cast/release rhythm instead of melee slash rhythm.
- Ranged/caster classes still need a logical combo system. It should not be a melee-style
  slash chain; it should be cadence, charge, reload, mark, element switch, heat, focus, or
  state sequencing.
- VFX/projectiles are separate from body animation. Do not bake full impact VFX into the character sprite.
- Movement-sheet planning is required before PixelLab production for every LMB chain and RMB action.

## Ranged/Caster Combo Rule For Claude Review

Range classes must not feel like "click projectile forever."

They need a class-owned basic attack loop with a readable internal rhythm:

- Elementalist: 3-bolt spell cadence plus element-state routing. The combo is Fire/Frost/Light
  sequencing and empowered release, not a sword chain.
- Ranger: tap/hold bow cadence. The combo is quick shot -> aimed/marked shot -> payoff shot,
  with Focus and Mark timing.
- Gunslinger: firing cadence / reload / heat rhythm. The combo is alternating pistols, recoil,
  Heat thresholds, and Hip Shot reposition windows.
- Hexer and Summoner are out of this 8-class pass, but the same rule should apply later:
  Hexer stacks/overload cadence; Summoner command/mark/minion focus cadence.

Claude decision needed:
- Should ranged/caster LMB chains share the same `comboStep` infrastructure as melee, or should
  they use per-class cadence counters (`shotStep`, `castStep`, `heatStep`, `markStep`) to avoid
  forcing melee semantics onto ranged classes?
- Preferred direction: shared interface, class-specific state. Do not force all classes into the
  Warblade 0/1/2 slash model.

## 1. Warblade

Weapon/body read: massive two-handed sword, armored weight, heavy shoulder mass.

LMB: Iron Combo
- Tempo: heavy-medium.
- Chain: 3 hits.
- Beat 1: low sweep, 4 frames, hit on frame 3.
- Beat 2: overhead cut, 5 frames, hit on frame 4.
- Beat 3: shoulder ram / blade drive, 5-6 frames, hit on frame 4, strongest recovery.
- Feel: deliberate, grounded, hit-stop friendly.
- Gameplay: Rage builder, final hit knockback/stagger.

RMB: Rage Outlet
- Tempo: short heavy burst.
- Action: feet plant, chest/weapon braced, close AoE shock burst.
- Frames: 4-5.
- Gameplay: spends Rage, creates breathing room, not a normal slash.

Implementation note:
- Current code already has the closest match: 3-step LMB combo exists.
- Needs animation/pose sheet alignment and Forge/LMB ecol binding later.

## 2. Elementalist

Weapon/body read: caster hands, staff optional, projectile/cast rhythm.

LMB: Rift Bolt
- Tempo: fast caster tap.
- Chain: 3-shot spell rhythm, not melee combo.
- Beat 1: quick hand flick, 3 frames, release on frame 2.
- Beat 2: alternate hand or wider wrist arc, 3 frames, release on frame 2.
- Beat 3: empowered bolt, 4 frames, release on frame 3.
- Feel: mobile, light, no planted melee recovery.
- Gameplay: mana/resource builder, every 3rd hit empowers or advances element state.

RMB: Element Switch / Lightbreak
- Tempo: instant tap, deliberate hold.
- Tap: 2-3 frame switch gesture, color/state change.
- Hold: 5-6 frame Lightbreak commit with visible charge/release.
- Gameplay: state routing, not damage filler.

Implementation note:
- Current code has RiftBolt and Switch/Lightbreak, but LMB resets combo state.
- Needs explicit 3-shot rhythm state if this lock is approved.

## 3. Shadowblade

Weapon/body read: dual daggers / reverse grip, close burst, phase movement.

LMB: Veil Strike
- Tempo: very fast.
- Chain: fast 3-hit dagger combo.
- Beat 1: left-to-right slash, 3 frames, hit on frame 2.
- Beat 2: right-hand stab/cross cut, 3 frames, hit on frame 2.
- Beat 3: twin thrust / cross carve, 4 frames, hit on frame 3.
- Optional hold after 3 hits: Twin Carve, 4-5 frames, two quick hit frames.
- Feel: snappy, low recovery, high silhouette clarity.
- Gameplay: Sever/Rift Scar/mark builder; should be the fastest melee loop in the first 8.

RMB: Veil Flicker
- Tempo: fast positional burst.
- Action: short phase-through step with afterimage, not a dodge roll.
- Frames: 3-4 body frames plus separate afterimage/VFX.
- Gameplay: crosses target/vector, applies Rift Scar, creates angle question.

Implementation note:
- Current code has a single VeilStrike-like hit and resets combo.
- This is the clearest mismatch: Shadowblade should become a true fast 3-hit LMB chain.

## 4. Ranger

Weapon/body read: bow, quick shot, draw/charge decision.

LMB: Rift Arrow
- Tempo: fast ranged tap with optional charged hold.
- Chain: cadence chain, not melee combo.
- Tap beat: 3-4 frames, draw/release, projectile release on frame 3.
- Repeated taps: step cycle alternates stance to avoid visual stutter.
- Hold beat: 5-6 frames, full draw, release on final frame.
- Feel: mobile kiting, light recovery.
- Gameplay: Focus builder, hold creates mark/charged shot.

RMB: Tactical Roll
- Tempo: fast evasive action.
- Action: short roll/vault plus immediate arrow release.
- Frames: 5-6 body frames; projectile separate.
- Gameplay: reposition + shot, not generic dodge.

Implementation note:
- Current code supports hold arrow and roll + arrow, but no stance/cadence animation chain yet.

## 5. Ravager

Weapon/body read: axe/brutal weapon, raw body momentum, berserker weight.

LMB: Brutal Swing
- Tempo: slow-heavy.
- Chain: 3 heavy hits.
- Beat 1: wide axe swing, 5 frames, hit on frame 4.
- Beat 2: overhead slam, 6 frames, hit on frame 5.
- Beat 3: ground pound / cleaving crash, 6 frames, hit on frame 4-5.
- Feel: heavier and less disciplined than Warblade; more body lurch, less armor control.
- Gameplay: Fury builder, bigger arcs, rewards taking damage / staying in pocket.

RMB: Blood Pact
- Tempo: deliberate self-risk commit.
- Action: self-wound / blood pull / rage inhale gesture.
- Frames: 5-6.
- Gameplay: HP trade into Fury/damage window; no shout filler.

Implementation note:
- Needs unique class implementation; must not inherit Warblade combo with different numbers.

## 6. Ronin

Weapon/body read: sheathed sword, draw-cut discipline, wait/punish rhythm.

LMB: Sheath Walk
- Tempo: medium-fast but restrained.
- Chain: 3 measured cuts.
- Beat 1: small sheath cut while moving, 3-4 frames, hit on frame 3.
- Beat 2: short returning cut, 3-4 frames, hit on frame 3.
- Beat 3: forward draw finish, 5 frames, hit on frame 4.
- Feel: less spammy than Shadowblade, more precise than Warblade.
- Gameplay: Tension builder; 3rd hit gives short advance/strong hit.

RMB: Drawn Edge
- Tempo: instant punish with optional preparation hold.
- Tap: 3-4 frame iaido slash, release on frame 2-3.
- Hold: 5-6 frame prepared stance, can convert incoming hit into counter/veil.
- Gameplay: spends Tension or opens parry/punish window.

Implementation note:
- Requires hit-frame timing precision. Animation must sell wait -> release.

## 7. Gunslinger

Weapon/body read: dual pistols, hip-fire, slide/shot rhythm.

LMB: Dual Fire
- Tempo: fast ranged sustained fire.
- Chain: 2-beat or 4-beat firing cadence.
- Beat A: both pistols forward, 3 frames, muzzle release on frame 2.
- Beat B: alternating recoil / cross-body shot, 3 frames, release on frame 2.
- Optional 4-beat loop: left/right stance variation to avoid repeated identical pose.
- Feel: mobile, low recovery, recoil animation matters.
- Gameplay: Heat builder, can be held for auto-fire.

RMB: Hip Shot
- Tempo: fast lateral action.
- Action: short side-step/slide plus single precise shot.
- Frames: 4-5.
- Gameplay: reposition + damage + Heat; not a generic dodge.

Implementation note:
- Projectile/muzzle flash must be separate from body frames.

## 8. Brawler

Weapon/body read: fists, elbows, knees, rhythm fighter.

LMB: Jab
- Tempo: fastest body-combat loop, but not as slippery as Shadowblade.
- Chain: 4-hit hand combo.
- Beat 1: jab, 2-3 frames, hit on frame 2.
- Beat 2: cross, 3 frames, hit on frame 2.
- Beat 3: hook, 3-4 frames, hit on frame 3.
- Beat 4: uppercut/knee finisher, 4-5 frames, hit on frame 3-4.
- Feel: rhythmic, percussive, short recovery.
- Gameplay: Charge builder, every hit matters; 4-hit chain feeds launch/overdrive logic.

RMB: Weave
- Tempo: defensive micro-move.
- Action: short side-slip, shoulder dip, guard up.
- Frames: 3-4.
- Perfect window: 0.2s timing should have clear pose.
- Gameplay: perfect timing grants Charge/iframes; Charged State can bank into Crowd Hype.

Implementation note:
- Brawler must not be reduced to "fast Warblade." It needs punch/knee silhouette variety.

## Production Checklist

Before PixelLab generation for each class:
1. Write LMB chain movement sheet prompt.
2. Write RMB movement sheet prompt.
3. Lock frame count and hit-frame index.
4. Generate pose sheet first.
5. Generate animation only after pose sheet is approved.
6. Import into Unity and verify hitbox/projectile timing against frame index.

## Code Gap Summary

Current code implements this design only partially.

- Warblade: closest to target.
- Elementalist/Ranger/Shadowblade: class-specific LMB/RMB exists, but LMB chain identity is incomplete.
- Ravager/Ronin/Gunslinger/Brawler: design exists in docs, playable primary implementation still needs explicit per-class basic attack logic.

Claude review request:
- Confirm whether this 8-class LMB/RMB contract should become the canonical basic attack lock.
- If approved, next Codex task should create a `BasicAttackProfile` or equivalent per-class data/logic boundary so classes cannot silently inherit Warblade fallback behavior.
- Decide ranged/caster combo architecture: shared combo interface with class-specific cadence state,
  or separate per-class counters.
