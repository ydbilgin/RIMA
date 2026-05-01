# SKILL VISUAL CONTRACT -- RAVAGER / BLOOD_PACT

---

## Section A -- Identity Metadata

| Field | Value | Notes |
|---|---|---|
| `class` | Ravager | |
| `skill_id` | `RAVAGER_BLOOD_PACT` | |
| `display_name` | Blood Pact | |
| `slot` | basic | RMB |
| `role` | sustain / resource | |
| `state_owner` | no | applies a self-buff on hold variant, but this is a duration modifier, not a named class state |
| `class_anchor_ref` | inherit (OWNS: HP trade, Fury escalation, meat/bone VFX; AVOIDS: armor-crack/plate-shard VFX, HP-execute visual cue) | |

Tone note: HP cost is the identity of this skill. The caster draws from their own body -- visual must make the self-damage legible without ambiguity. NOT an elegant ritual; a brutal pact. Single tap for quick Fury. Hold for sustained dmg boost at HP cost.

---

## Section B -- Animation States Required

| State | Required? | Frame count | Purpose | Notes |
|---|---|---|---|---|
| `wind_up` | no | -- | skill has no wind_up; instant on tap / hold begins immediately | |
| `active` | yes | 3 | hand-to-chest F1 (grip on own chest/side), blood draw F2 (brief red flash on own body), release F3 (arm extends, blood mist off hand) | impact on caster F2; audio anchor F2; hold variant extends F2 duration via loop |
| `loop` | yes | 1 | hold variant: blood draw sustained (F2 pose held, slow bleed pulse) | loop plays during 1s hold channel |
| `recovery` | yes | 1 | arm drops, brief stagger-twitch (HP has been spent) | must read as cost paid, not strength gained |

Total frame budget: 5 (active 3 + loop 1 + recovery 1). Within basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| `applies` | hold variant: 6s dmg +30% self-buff | brief full-body dark-red pulse on caster at release (F3), then no persistent overlay -- buff duration tracked by HUD only | single-frame aura flash; does NOT use a world-space pip |
| `reads` | no-combat block check | if skill is blocked (not in combat), active frame does not play; caster raises hand then lowers (aborted gesture) | handled by code; no new sprite state needed |

No enemy state applied. Visual emphasis must remain on CASTER cost (red flash on own body at F2), not enemy state. Rule #56 compliance: buff conditional reads time-gate (hold 1s), not any HP threshold shown on screen.

---

## Section D -- VFX Requirements

| VFX layer | Required? | Spec |
|---|---|---|
| `cast_particle` | no | no telegraphed cast |
| `impact_particle` | yes | F2: brief red-blood flash centered on caster torso (self-damage visual); hold variant: slow drip particles from caster hand during loop |
| `trail` | no | no weapon; hand motion only |
| `screen_overlay` | no | basic skill |
| `hit_reaction_on_enemy` | no | no enemy contact |
| `audio_anchor_frame` | active F2 | blood draw is the audible beat (wet, visceral) |

VFX layer count: 1 active (impact_particle on self). Within <= 4 gate.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | HP-cost self-damage visual (blood flash on own body); Fury resource gain |
| Avoids every AVOIDS item | PASS | no armor-crack, no plate-shard; no enemy HP read |
| Counter-archetype distinct (n/a if not counter) | n/a | Rule #57 |
| No HP-execute visual cue | PASS | buff conditional is time-gate (hold 1s), not HP bar color; self-damage flash is CASTER not enemy |
| No cracked-armor / Sundered VFX unless class == Warblade (Rule #55) | PASS | impact is blood flash on own body; no armor-crack language |
| Silhouette distinct from class neutral pose at 64px downscale | PASS | hand-to-chest grip (F1) and arm extension with blood mist (F3) are distinct from neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| `endpoint` | `create_spritesheet` |
| `canvas` | 128x128 |
| `ppu` | 64 |
| `rows` | 3 (active, loop, recovery) |
| `frames_per_row` | 3, 1, 1 |
| `palette_ref` | Ravager class palette (dark red / bone-white accent / blood-stain ground) |
| `reference_sprite` | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| `gen_budget_estimate` | 5 frames |
| `priority` | P0 -- basic RMB, sustain loop required for playable build |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
