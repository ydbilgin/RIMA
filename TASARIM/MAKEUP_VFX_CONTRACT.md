# Makeup VFX Contract
Status: LOCKED 2026-05-06

## What Is It

Small runtime visual cues that make the character's state readable at a glance.
Not baked into PixelLab sprite frames. Unity-side particles, overlays, shader params.
Required per skill contract — not a separate polish pass added later.

## Color Rule

- Universal states (speed, shield, vulnerable, stun, invulnerable, low HP, resource full):
  neutral cyan/white
- Class-identity states (dash buff, damage up, marked, scar, cracked, sundered, burn, frozen, shock):
  class color tint

Rule of thumb: if the cue says "who you are" -> class tint. If it says "what state you are in" -> neutral.

## Simultaneous Loop Cap

| Layer | Max active loops |
|---|---|
| Body | 2 |
| Foot | 2 |
| Weapon/hand | 1 |
| HUD pips | unlimited |

Priority when cap is hit (highest wins, lower fades out):
1. Class-identity (Scar, Sundered, Cracked, Burn)
2. Defensive (shield, invulnerable, low HP)
3. Offensive (damage up)
4. Movement (speed up, speed down)

## Decay Rule

Any looping body/weapon makeup desaturates ~20% in its final 1.5s before expiring.
Universal "about to drop" tell. No timer UI needed.

## Body Overlays vs HUD/Pip Only

### Body overlays allowed
Speed up, Speed down, Dash buff, Damage up, Defense up/shield, Vulnerable,
Burn, Frozen/Chill, Scar, Cracked/Shattered, Sundered, Invulnerable, Low HP

### HUD or world pip only (no body loop)
Attack speed up (HUD pip + weapon shimmer only),
Shock (target pip on enemy),
Marked (target reticle pip),
Stun (above-head ring pulse),
Resource full (HUD primary; optional tiny body accent only),
Rift Portal nearby (world floor cue, not on character body)

## State Taxonomy

| State | Makeup Cue | Owner | Notes |
|---|---|---|---|
| Speed up | wind streaks at feet/back, short dust taper | universal | neutral cyan |
| Speed down | heavy foot shadow, dragging dust, muted trail | universal | not ice look unless frozen |
| Dash buff | short afterimage, foot spark, directional smear | class tint OK | |
| Attack speed up | HUD pip + weapon cadence shimmer | universal | no body loop |
| Damage up | pulse on hands/weapon/core rift | universal | activation burst > idle cue |
| Defense up/shield | thin rim shield, tiny impact glint on hit | universal | no full bubble |
| Vulnerable | cracked outline or exposed core pip | universal | not Warblade Sundered language |
| Burn | ember motes, heat shimmer | Elementalist (when applied by class) | |
| Frozen/Chill | cold mist at feet, frost pips | universal | no full body recolor |
| Shock | small arc pip on target | universal | pip only, no body loop |
| Marked | target reticle pip, line flash on apply | Ranger | must not look like Shadowblade Scar |
| Scar | violet seam/thread decal, collapses on consume | Shadowblade | class-owned |
| Cracked/Shattered | organic body fissure decal | Brawler | not metallic |
| Sundered | armor plate fissure/shard decal | Warblade | class-owned |
| Bleed | red droplet motes at wound point, HUD pip | Ravager | no body overlay |
| Stun | ring pulse above head or center mass | universal | avoid cartoon stars |
| Invulnerable | brief white/cyan rim blink | universal | not same look as damage shield |
| Low HP | blood-edge vignette or heartbeat pulse | universal | not on enemies |
| Resource full | class icon pulse + tiny body accent | class | HUD first, body accent second |
| Rift Portal nearby | floor crack smoke + low hum + interact glint | world | room-local, not on character |

## Extended Ideas (Not Yet Scheduled)

- Enemy makeup mirroring: elites carry a desaturated tint matching the threat type they punish
- Floor-reactive foot VFX: foot loop samples tile material (stone -> sparks, water -> splash, burned -> ash)
- Combo ignition threshold: body loop intensifies (param ramp) when 2+ states stack, no extra layer added
- StateMakeupProfile ScriptableObject per state family, bound to existing status/effect IDs
- Debug overlay to preview all states on player and dummy enemy
