# SKILL VISUAL CONTRACT -- RANGER: Rift Arrow (LMB Basic)

---

## Section A -- Identity Metadata

| Field | Value | Notes |
|---|---|---|
| `class` | Ranger | |
| `skill_id` | `RANGER_RIFT_ARROW` | |
| `display_name` | Rift Arrow | |
| `slot` | basic | LMB; hold variant charges and guarantees Mark |
| `role` | pressure / opener | |
| `state_owner` | yes (hold variant applies Mark to target) | instant tap: no state; 1s hold: Mark on hit |
| `class_anchor_ref` | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, close-range aggression) | |

Tone note: methodical opener, not an aggressive spray. Instant tap is a quick snap-shot to maintain Focus generation; hold is a deliberate commitment to establish kill-zone conditions.

---

## Section B -- Animation States Required

| State | Required? | Frame count | Purpose | Notes |
|---|---|---|---|---|
| `wind_up` | yes | 2 | bow draw -- hold version must visually extend beyond tap version | tap: partial draw (1f notch + 1f partial pull); hold: same start but loop-held until release |
| `active` | yes | 2 | release + arrow flight start | F1 = release (string snap, body snap back); F2 = follow-through; charged variant has a brighter bow-limb flash on F1 |
| `recovery` | yes | 1 | bow arm lowers, weight shifts back | same for both variants; audio anchor: active F1 |

Frame total: 5. Within basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| `applies` | hold variant hits target | Mark reticle appears on target | geometric circular aim-point overlay centered on enemy, thin ring + crosshair tick marks; 1-frame pulse on placement, then persistent idle state |
| `reads` | Focus meter (caster state) | bow/limb glow tier changes | low (<75): no glow; mid (75-99): faint green-amber limb glow; 100: full bright pulse on bow |
| `consumes` | none | -- | -- |
| `disambiguation_note` | Mark (Ranger) vs Scar (Shadowblade) vs Hexer pip | Mark: geometric circular reticle, camera-facing overlay, NOT body-anchored gash | Scar = diagonal black-violet gash decal anchored world-space on enemy body; Hexer pip = small floating orb stack counter above head. Any visual overlap with these two is a generation blocker. |

---

## Section D -- VFX Requirements

| VFX layer | Required? | Spec |
|---|---|---|
| `cast_particle` | no | tap variant has no telegraphed cast; hold variant may show a faint bow-charge shimmer on wind_up F2 only |
| `impact_particle` | yes | small arrow-strike puff on contact; charged variant adds a rift-ring concentric ripple at impact point |
| `trail` | yes | arrow projectile trail, thin line; charged variant trail is brighter and slightly wider |
| `screen_overlay` | no | basic skill -- overlay forbidden |
| `hit_reaction_on_enemy` | yes | `marked` reaction set (hold variant only); tap variant: `generic_stagger` |
| `audio_anchor_frame` | active F1 | string snap is the audible beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | bow draw, Mark reticle, distance-oriented framing |
| Avoids every AVOIDS item | PASS | no run-and-gun; hold requires standing still at range; no close-range visual language |
| Counter-archetype distinct | n/a | not a counter skill |
| No HP-execute visual cue | PASS | Mark is applied on hit, not gated by target HP; Focus glow reads caster resource, not enemy state |
| No cracked-armor / Sundered VFX unless class == Warblade (Rule #55) | PASS | no armor-crack or shard language; impact is arrow + rift ripple only |
| Silhouette distinct from class neutral pose at 64px downscale | PASS | draw stance (bow arm extended, string arm pulled) differs clearly from neutral standing posture |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| `endpoint` | `create_spritesheet` |
| `canvas` | 128x128 |
| `ppu` | 64 |
| `rows` | 3 (wind_up, active, recovery) |
| `frames_per_row` | 2, 2, 1 |
| `palette_ref` | Ranger class palette (earthy green-brown / amber focus accent) |
| `reference_sprite` | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| `gen_budget_estimate` | 5 frames; Mark reticle decal as separate micro-asset (2 frames: pulse + idle) |
| `priority` | P0 -- LMB basic, blocks Ranger playable build |

Note for rima-codex: Mark reticle must be gen'd as its own micro-spritesheet (1 row, 2 frames: placement pulse + idle) so it can be instantiated independently on any target. Do NOT bake the reticle into the caster sheet.

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
