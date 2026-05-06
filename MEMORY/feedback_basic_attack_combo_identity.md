---
name: Basic attack combo identity lock
type: feedback
trigger: basic attack, LMB, RMB, combo, weapon speed, attack animation
description: Character-specific LMB/RMB combo and animation identity rule
---

## Design Lock

Every playable class must own its LMB and RMB identity.

- LMB is not a generic shared attack. It is the class primary attack.
- RMB is not a generic shared utility. It is the class secondary attack/outlet.
- LMB and RMB may each have internal combo/state rhythm when the class fantasy needs it.
- Basic attack animation clips must be short, readable, and fluid.
- Weapon/body fantasy controls timing:
  - heavy weapon = slower anticipation, heavier hit frame, stronger recovery
  - light weapon = faster startup, smaller recovery, sharper chain timing
  - caster/ranged = cast/release rhythm instead of melee slash timing
- Fluid does not mean long. Prefer small 3-6 frame attack beats with clear windup, hit frame,
  and recovery.

## Current Code Reality (checked 2026-05-06)

`Assets/Scripts/Player/PlayerAttack.cs` is not fully aligned with the lock yet.

- Warblade has a real 3-step LMB combo through `comboStep`, `comboLength`, `OnComboStep`,
  per-step damage/range/knockback, and `PlayerAnimator.ComboStep`.
- Elementalist LMB is class-specific (`RiftBolt`) but resets `comboStep` and has no internal
  LMB chain yet.
- Ranger LMB is class-specific (`RiftArrow`, hold/charge) but resets `comboStep` and has no
  internal LMB chain yet.
- Shadowblade LMB is class-specific (`VeilStrike`) but currently uses one repeated strike,
  resets `comboStep`, and does not implement the documented 3-step blade combo.
- Warblade RMB exists (`RageOutlet`), Elementalist RMB exists (`Switch/Lightbreak`), Ranger RMB
  exists (`Roll + arrow`), Shadowblade RMB exists (`VeilFlicker`).
- Other future classes are not implemented as playable primaries yet; do not let them silently
  inherit Warblade generic LMB/RMB behavior.
- `LMBEcolSystem` is still a stub; Forge UI choices do not yet modify LMB attack logic.

## Implementation Rule

Before adding new playable classes or finalizing animation production, introduce an explicit
per-class basic attack contract:

1. LMB identity and combo rhythm.
2. RMB identity and combo/resource relationship.
3. Attack speed class: heavy, medium, fast, ranged, caster.
4. Per-step animation frame count and hit-frame timing.
5. Per-step hitbox/projectile/VFX timing.

No class should ship with default Warblade fallback unless it is Warblade.

## PixelLab Rule

For basic attacks, generate movement/pose sheets first:

- one sheet per class LMB chain
- one sheet per class RMB action if it has animation identity
- separate weapon/body motion from VFX/projectile
- keep prompt action narrow: one step or one chain, no baked enemy reaction

