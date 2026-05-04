# SKILL VISUAL CONTRACT -- Ranger: Marked Detonate

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_MARKED_DETONATE` |
| display_name | Marked Detonate |
| slot | basic |
| role | closer |
| state_owner | no (consumes Mark -- Mark is applied by other Ranger skills) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: remote detonation from a distance. Ranger does NOT move toward target. Brief aim pose, a deliberate finger-trigger gesture, then the explosion happens on the target -- not on the caster. Reads as a technician pressing a detonator, not a melee finisher.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | brief raised-arm aim pose, two fingers extended forward toward target -- small but readable tell | no bow draw; this is a gesture-cast |
| active | yes | 3 | F1: finger-trigger squeeze (caster side); F2: off-caster explosion burst on target (world-space, not caster sprite); F3: caster arm lowers | explosion VFX is a separate particle at target location -- caster animation does NOT travel to target |
| recovery | yes | 2 | arm drop, weight neutral -- Ranger is still planted at range | |

Frame total: 7. Within Ranger basic quota.

Explosion frame (F2) is a world-space VFX event at the marked target's position. The caster sprite only shows the trigger gesture; no caster displacement.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none | -- | -- |
| reads | skill is only castable if target carries active Mark | brief bone-white pulse on caster trigger hand on wind_up F1 when valid target is in range -- single frame confirmation flash | hand glow, earth-green tint, fades before active |
| consumes | Mark state on target is destroyed on detonation | Mark reticle collapses inward (1-frame implosion) immediately before explosion burst on F2; reticle must visibly vanish before blast fills frame | Mark reticle collapse: circular ring shrinks to center point in 1 frame; do NOT let it linger into or after the explosion |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. Mark reticle (Ranger) is a circular hover-overlay above enemy body, not touching skin. Scar is a diagonal slash decal on the body surface. Hexer pip is a small floating orb stacked beside the enemy portrait. The collapse animation on Mark must be the ring-inward-shrink and NOT a gash-close or orb-pop. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | gesture-cast; no pre-cast particle trail |
| impact_particle | yes | explosion burst at target world position on active F2; radius ~1.5 tiles; earth-green + bone-white color; NOT a fire or arcane burst (those are Gunslinger/Hexer territory) |
| trail | no | no projectile travels from caster to target -- this is remote detonation |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `marked` collapse on F2 (ring implosion), then `generic_stagger` from blast force |
| audio_anchor_frame | active F2 | the off-caster explosion is the audible beat; F1 finger-trigger gets a soft click SFX |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | remote detonation from range, mark consumption, no melee contact -- all Ranger OWNS |
| Avoids every AVOIDS item | PASS | no caster movement toward target (run-and-gun), no teleport, explosion does not resemble Gunslinger fire burst |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | skill gates on Mark state presence, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor or plate language; explosion is earth-green energy burst |
| Silhouette distinct at 64px | PASS | raised two-finger trigger gesture reads clearly vs Ranger neutral bow stance |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 7 frames |
| priority | P1 |

Note: explosion VFX at target position is a separate particle asset dispatched alongside the caster spritesheet. Both must be in the same generation batch.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Mark-applying skills must ship in same batch so the consume animation has a reference Mark visual to collapse) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
