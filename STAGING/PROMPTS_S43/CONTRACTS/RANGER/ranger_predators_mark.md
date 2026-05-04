# SKILL VISUAL CONTRACT -- Ranger: Predator's Mark

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_PREDATORS_MARK` |
| display_name | Predator's Mark |
| slot | signature |
| role | control / opener |
| state_owner | yes (applies Mark to all enemies in cursor 4m AoE; Focus 75+ expands to 5 targets) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: cursor-zone area cast. Ranger paints a kill zone from a distance. Multiple enemies receive Mark simultaneously -- the animation must read as area cast, not single-target. The Focus 75+ expansion variant should read as the same cast with a wider ground reticle, not a separate animation.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | Ranger raises non-bow hand, palm forward, eye-contact toward cursor zone -- deliberate scanning pose | wind_up longer than basic skills to convey intention and area assessment; F3 is peak palm-forward |
| active | yes | 3 | F1: ground reticle expands from center at cursor zone (world VFX); F2: peak expansion flash, Mark reticles drop onto all enemies in area simultaneously; F3: caster hand drops | area cast: multiple Mark reticles appear in the same frame (F2); Focus 75+ variant: reticle radius on ground is visibly larger (world VFX scale, not caster animation change) |
| recovery | yes | 2 | caster hand lowers, scanning stance relaxes | |

Frame total: 8. Within Ranger signature quota.

Area cast read requirement: active F2 must show multiple Mark reticles appearing on different enemies in the same frame. This distinguishes from Pinning Shot (single arrow) and LMB Rift Arrow (single target). The caster pose on active F1-F3 is the same regardless of how many enemies are in zone -- the count distinction is handled by VFX, not caster frames.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | skill applies Mark to all enemies in 4m AoE (5 targets at Focus 75+) | Mark reticle appears hovering above each targeted enemy body on active F2 simultaneously; reticles are circular hover overlays, NOT body-anchored decals | earth-green circular reticle, hovering ~0.5 tile above enemy head; NOT touching skin; persists until detonated or expired |
| reads | Focus 75+ expands effective area to 5 targets | world-space ground reticle (cast zone indicator) is visibly larger radius on F1; caster animation is identical -- only the zone VFX scale changes | ground zone reticle: standard = 4m radius bone-white ring; Focus 75+ = 5m radius with a faint secondary outer ring to signal expansion |
| consumes | none | -- | -- |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. With multiple enemies marked simultaneously, the disambiguation is especially load-bearing: each Ranger Mark is a circular hover reticle above the body; Shadowblade Scar is a diagonal slash on the body surface (NOT hovering); Hexer pip is a small floating orb stacked at body side, NOT centered above. When 3-5 Mark reticles appear in one frame, each must retain the circular centered-above-head read. No reticle must be positioned at body-side (Hexer territory) or on-skin (Scar territory). |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | ground zone reticle expands from cursor point on active F1; bone-white ring grow animation, 0.3s; Focus 75+ variant is same but larger radius and faint double-ring |
| impact_particle | yes | per-enemy Mark application flash on F2: brief bone-white pulse at each enemy's hover-above-head position; must fire simultaneously for all targets in zone |
| trail | no | AoE cast with no projectile; no trail |
| screen_overlay | no | signature skill but AoE Mark -- no screen overlay; area cast reads from world VFX alone |
| hit_reaction_on_enemy | yes | `marked` reticle hover applied on F2; no stagger (Mark application is non-damaging) |
| audio_anchor_frame | active F2 -- simultaneous Mark application is the audible beat; a single resonant sound for the group application, not N individual sounds |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | area Mark from range, kill-zone designation, cursor placement -- Ranger OWNS |
| Avoids every AVOIDS item | PASS | no movement, no run-and-gun; no teleport; AoE does not use Hexer or arcane rune language |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | Mark applies unconditionally to all in zone; Focus 75+ expands range, not threshold |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor language; bone-white ring is sinew/bone visual register |
| Silhouette distinct at 64px | PASS | raised palm-forward non-bow-hand is distinct from bow-draw; area cast pose vs single-target draw reads differently |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 3, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 8 frames |
| priority | P1 |

Note: world-space ground reticle and per-enemy Mark reticles are VFX assets dispatched separately. Caster sheet only covers the placement gesture.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Mark reticle VFX asset must be in same batch as Marked Detonate to ensure visual consistency of the apply/consume cycle) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
