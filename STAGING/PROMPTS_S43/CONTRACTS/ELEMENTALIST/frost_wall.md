# SKILL VISUAL CONTRACT -- Elementalist: Frost Wall

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_FROST_WALL` |
| display_name | Frost Wall |
| slot | 8 (Core, Unity Light State pending) |
| role | cursor-placed line barrier / contact slow / radiant crack damage |
| state_owner | yes -- Light State (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: the caster defines a line position with cursor, then summons a wall of fused ice and light along that line. The wall persists for 4s, slowing any enemy that contacts it and dealing radiant crack damage on contact. The wall is ice-light -- blue-white with prismatic refractions along the surface. Not a physical barrier the caster constructs; it is summoned from the ground up. Near a frozen enemy, the Freeze duration extends 1s. Cast gesture: arm sweeps horizontally across the body then pushes outward -- defining and then materializing the wall line.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| cast_start | yes | 2 | arm sweeps horizontally across front of body, tracing wall line direction in air | frame 1: arm sweep across; frame 2: arm at end of sweep; wall rising from ground begins |
| cast_release | yes | 2 | arm pushes forward -- wall fully materializes along defined line; prismatic light pulses through ice | frame 1: push forward; frame 2: arm extended, wall confirmed |
| recovery | yes | 1 | arm lowers; caster may continue moving | wall persists independently of caster |

Frame total: 5. Within Elementalist core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Light State +1 (Light element present in wall) | HUD Light State counter increments | Unity overlay pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md |
| applies | 4s ice-light wall at defined line | scene VFX object placed in world | wall VFX is scene-level; no persistent caster sprite change |
| reads | frozen enemy near wall (synergy: Freeze +1s extended) | no caster visual change; handled in code | -- |
| consumes | none | -- | -- |

state_owner note: Unity Light State overlay pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | wall rising from ground: ice crystals forming upward along the defined line -- cold blue-white with prismatic edge; cast_start F2 to cast_release F2 |
| impact_particle | yes | enemy contact with wall: frost + radiant spark burst at contact point; 2-3 px; slowing film on target |
| trail | no | no caster movement trail |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `contact_slow_frost` -- enemy slows on wall contact; frost film applied |
| audio_anchor_frame | cast_release F1 | crystalline crack as wall forms; sustained cold hum during wall duration |

VFX layer count: 3 (cast_particle, impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | wall spell shape, combined Frost+Light elements -- Elementalist OWNS wall spell shapes |
| Avoids every AVOIDS item | PASS | summoned wall is a spell construct, not a physical placed trap (Ranger territory); caster does not physically plant an object |
| Counter-archetype distinct (Rule #57) | n/a | Frost Wall is field control, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals |
| No cross-class state confusion (Rule #58) | PASS | ice-light wall is distinct from Ranger physical barriers, Warblade ground fissures |
| No Sundered/armor crack VFX (Rule #55) | PASS | radiant crack on wall is frost-light material, not enemy armor fissure |
| Silhouette distinct from class neutral at 64px | PASS | horizontal arm sweep and push gesture is unique vs vertical/forward single-arm casts |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (cast_start, cast_release, recovery) |
| frames_per_row | 2, 2, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, Frost+Light combined: cold blue-white with prismatic edge) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 5 frames |
| priority | P1 -- core wall control skill; required for Light State chain testing |

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
