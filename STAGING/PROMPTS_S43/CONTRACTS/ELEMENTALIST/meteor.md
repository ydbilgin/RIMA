# SKILL VISUAL CONTRACT -- Elementalist: Meteor

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_METEOR` |
| display_name | Meteor |
| slot | 7 (Core) |
| role | large AoE knockdown / Frost/slow synergy |
| state_owner | no (applies knockdown to enemies; no caster state) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: 0.5s wind-up then a large meteor falls from above-frame onto a cursor-designated point. This is NOT a channel spell -- the caster movement continues during wind-up and after launch. The wind-up gesture is a single raised arm pointing skyward (calling the meteor down), then the arm drops as the meteor arrives. The caster does not freeze. Impact: large AoE fire-rock explosion with knockdown on all in radius. Under Frozen/slowed target condition the knockdown extends to 3s. Academic spell-caster energy -- this is called/summoned, not physically thrown.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | arm raises skyward, index finger pointing up -- meteor called; 0.5s window | frame 1: arm rising; frame 2: arm fully raised, pointing up; frame 3: moment of arrival above -- arm pulls down sharply to signal impact |
| release | yes | 1 | arm drops, palm faces down -- impact moment | caster can continue moving; this frame is not a freeze |
| recovery | yes | 2 | caster steps back or shifts stance; watches impact zone; dust settles | 2 frames; movement continues during recovery |

Frame total: 6. Within Elementalist core quota.

Meteor impact VFX is a scene-level event (falling object from above-frame) -- no caster animation row needed for the meteor object itself. Impact explosion is a separate scene VFX.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | knockdown 2s (standard) or 3s (frozen/slowed synergy) | enemies at target point receive knockdown | scene impact VFX; no caster indicator |
| reads | Frozen or slowed target at impact point (synergy: knockdown 3s + damage +%50) | no caster visual change; extended knockdown handled in code | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | wind_up: warm orange glow builds above caster arm while pointing up; 2-3 px sky-glow above fingertip; communicates meteor approaching |
| impact_particle | yes | impact zone: large fire-rock explosion 5-6 px radius; debris scatter; ground scorch ring; knockdown shockwave ring |
| trail | no | caster has no projectile trail; meteor descent is scene VFX |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `meteor_knockdown` -- massive body displacement; enemies thrown outward from impact point |
| audio_anchor_frame | release F1 | deep rumble build + catastrophic impact boom |

VFX layer count: 3 (cast_particle, impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | summoned spell, fire element, academic calling gesture -- Elementalist |
| Avoids every AVOIDS item | PASS | meteor is a summoned spell-object, not a physical trap or physical thrown weapon |
| Counter-archetype distinct (Rule #57) | n/a | Meteor is an active cast, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; extended knockdown is a target-state conditional (Frozen/slow), not HP |
| No cross-class state confusion (Rule #58) | PASS | fire-rock meteor distinct from Warblade ground slam (weapon-physical), Ranger AoE mines |
| No Sundered/armor crack VFX (Rule #55) | PASS | meteor explosion is fire-rock; no armor fissure decal |
| Silhouette distinct from class neutral at 64px | PASS | single arm raised skyward pointing is unique in entire Elementalist kit |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, release, recovery) |
| frames_per_row | 3, 1, 2 |
| palette_ref | Elementalist class palette (muted blue-grey base, fire element color: orange-red on meteor glow) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 6 frames |
| priority | P1 -- core large AoE skill; required for Blizzard synergy testing |

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
