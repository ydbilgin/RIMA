# SKILL VISUAL CONTRACT -- Elementalist: Prism Beam

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_PRISM_BEAM` |
| display_name | Prism Beam |
| slot | 6 (Core, Unity Light State pending) |
| role | cursor-aimed channel beam / pierce / Light State damage scaler |
| state_owner | yes -- Light State (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: a channeled beam of prismatic light aimed along a cursor-defined straight line, piercing all enemies in the path. This is the Elementalist's signature beam spell -- the beam shape is narrow and precise, refracting into rainbow-edge highlights at the periphery. Light State stacks increase damage multiplicatively during the channel. At Light State 3+ stacks, the channel ends in a burst + 2m AoE radiant explosion. The channel pose: one arm extended forward, hand open, beam emanates from palm. Other hand supports at the elbow. Controlled academic concentration.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| cast_start | yes | 2 | arm extends forward, hand opens -- prismatic light gathers in palm; refraction shimmer begins | frame 1: arm raise; frame 2: palm open, beam gathering |
| channel_loop | yes | 3 | beam active: extended pose with tremor in arm from sustained output; rainbow edge glow on beam | 3-frame loop; subtle arm tremor communicates sustained effort; beam VFX runs as separate layer |
| burst_release | yes | 2 | Light State 3+ only: arm jolts back then forward as burst fires; palm flares bright | 2 frames: recoil then forward burst; only plays at Light State threshold |
| recovery | yes | 1 | arm lowers; caster exhales (head tilt) | |

Frame total: 8. Within Elementalist core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Light State +1 per channel tick (or per stack mechanic defined in code) | HUD Light State counter increments | Unity overlay pending; sprite shows beam emanating from palm |
| reads | Light State stacks (synergy: damage scales with stacks) | channel_loop intensity increases with stack count (Unity shader handles) | -- |
| reads | Light State 3 stacks (synergy: burst + 2m AoE radiant) | burst_release fires at end of channel; no separate animation needed for AoE expansion | AoE radiant is VFX-only event at burst_release F2 |
| consumes | none | -- | -- |

state_owner note: Unity Light State overlay pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | palm light gather on cast_start F2: prismatic shimmer 2-3 px; white-spectrum edge highlight |
| impact_particle | yes | beam hits on enemies along line: radiant spark bursts per enemy; rainbow edge; Light State 3 burst: large 2m radiant explosion 5-6 px radius |
| trail | no | beam is a scene VFX channel line rendered independently; no sprite-level trail needed |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `radiant_pierce` -- each enemy in beam line receives radiant jolt; body pierced by light |
| audio_anchor_frame | channel_loop F1 | sustained radiant hum; burst_release: sharp prismatic crack |

VFX layer count: 3 (cast_particle, impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | beam spell shape, prismatic Light element -- Elementalist OWNS beam and Lightbreak |
| Avoids every AVOIDS item | PASS | beam is a pure spell; no physical mechanism |
| Counter-archetype distinct (Rule #57) | n/a | Prism Beam is a channel attack, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; burst fires on Light State stack count (class state gate), not HP |
| No cross-class state confusion (Rule #58) | PASS | prismatic white-spectrum beam distinct from Warblade amber, Shadowblade dark ray |
| No Sundered/armor crack VFX (Rule #55) | PASS | radiant pierce has no armor-fissure element |
| Silhouette distinct from class neutral at 64px | PASS | extended-arm channel pose with supporting arm is unique vs single quick-cast gestures |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (cast_start, channel_loop, burst_release, recovery) |
| frames_per_row | 2, 3, 2, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, Light element color: white-prismatic with rainbow edge) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 8 frames |
| priority | P1 -- core Light channel skill; required for Light State chain testing |

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
