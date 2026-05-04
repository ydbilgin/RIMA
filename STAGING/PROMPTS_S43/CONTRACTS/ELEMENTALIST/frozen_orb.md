# SKILL VISUAL CONTRACT -- Elementalist: Frozen Orb

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_FROZEN_ORB` |
| display_name | Frozen Orb |
| slot | 5 (Core) |
| role | slow-moving AoE chill field / Blink detonation trigger |
| state_owner | no (applies chill to enemies; no caster state) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: a slow-moving large ice orb that rolls across the battlefield, chilling everything in its path for 5 seconds. It is a field-control spell. The orb is visually distinct from Fireball -- larger, slower, rotating, shedding frost particles as it travels. When Blinked through, the orb detonates immediately with a frost explosion (Frozen 2s effect). Cast gesture: arms raise overhead in arc then project forward with both hands -- the orb is too large for a single-hand push.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| cast_start | yes | 3 | arms arc overhead then come down and project forward -- large orb builds between raised arms | frame 1: arms rise; frame 2: orb forms between raised arms (larger than Fireball); frame 3: arms push down to project |
| cast_release | yes | 1 | both arms push forward and outward -- orb launched at slow speed | |
| recovery | yes | 1 | arms lower; caster watches orb travel | caster can continue acting during orb travel |

Frame total: 5. Within Elementalist core quota.

Blink detonation note: if caster Blinks through the orb during its travel, detonation is triggered by code. No additional caster animation row needed; detonation plays as a separate scene VFX event.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | chill 5s to all enemies in orb path | frost film on hit enemies; movement slow communicated by target stutter | hit VFX system handles persistent chill indicator |
| reads | Blink through orb (synergy: orb detonates, Frozen 2s AoE) | no caster animation change; detonation handled by collision event in code | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | orb building between raised arms: cold blue-white light gathers; larger glow circle than Fireball (3-4 px at 64px view); slow pulse |
| impact_particle | yes | contact with enemies: frost scatter 2-3 px; chill film on target body; frost particles shed from orb continuously during travel (ambient trail) |
| trail | yes | orb travel: continuous frost particle shed behind orb as it moves; persistent ambient trail (not a motion blur -- the orb sheds material) |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `chill_slow` -- enemy movement visibly slows; frost film on sprite |
| audio_anchor_frame | cast_release F1 | cold rolling hum + frost crackle on contact |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | slow-moving orb spell shape, Frost element, two-arm cast -- Elementalist |
| Avoids every AVOIDS item | PASS | ice orb is a spell, not a physical trap or caltrops; no terrain placement mechanism |
| Counter-archetype distinct (Rule #57) | n/a | Frozen Orb is field control, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals |
| No cross-class state confusion (Rule #58) | PASS | large cold blue orb distinct from Warblade shockwave rings, Ranger proximity mines |
| No Sundered/armor crack VFX (Rule #55) | PASS | frost chill has no armor-fissure element |
| Silhouette distinct from class neutral at 64px | PASS | overhead arc arms and two-hand forward push is a distinct large-spell gesture vs one-arm casts |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (cast_start, cast_release, recovery) |
| frames_per_row | 3, 1, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, frost element color: cold blue-white; orb larger and slower palette than Glacial Spike) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 5 frames |
| priority | P1 -- core Frost control skill; required for Blink detonation synergy testing |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
