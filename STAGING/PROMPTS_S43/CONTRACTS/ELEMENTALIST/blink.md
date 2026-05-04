# SKILL VISUAL CONTRACT -- Elementalist: Blink

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_BLINK` |
| display_name | Blink |
| slot | 4 (Core, Unity Light State pending) |
| role | 6m teleport / damage to crossed enemies / next spell +%20 |
| state_owner | yes -- Light State (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: an arcane teleport through space -- the Elementalist vanishes in a flash of light and reappears 6m forward. Any enemy in the path is hit by residual radiant energy as the caster passes through the dimension. The departure is a white-blue flash with brief afterimage; the arrival is a reverse flash. The next-spell buff (+%20) is a caster aura highlight that persists for one cast. Blink through enemy = 0.5s stun; this is a physics-of-passing-through event, not a deliberate strike -- communicated by enemy body shock on the Blink path.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| depart | yes | 2 | body begins dissolve into light -- frame 1: light accumulates around caster; frame 2: caster fades to white flash departure | departure flash should leave a brief afterimage silhouette |
| in_transit | yes | 1 | invisible (or near-invisible) -- represented by empty/ghost frame; travel is instantaneous in sprite | 1 placeholder frame; game handles positional jump |
| arrive | yes | 2 | reverse flash arrival -- frame 1: light burst at destination; frame 2: caster solidifies from light into stance | arrival flash is same color family (white-blue radiant) as departure |
| post_blink_ready | yes | 2 | caster in boosted stance -- subtle aura highlight around body communicating next-spell +%20 buff | 2-frame loop; brief shimmer; dissolves when next spell cast |

Frame total: 7. Within Elementalist core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | next spell +%20 damage | caster aura shimmer on post_blink_ready frames | Light State stack +1 communicated by Unity overlay (pending) |
| applies | Light State (contributes to Prism Beam / Radiant Pillar) | HUD Light State counter increments | Unity overlay pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md |
| applies | damage to enemies in path | enemy shock/jolt on Blink path | no caster sprite change for path damage |
| reads | Blink through enemy (synergy: 0.5s stun) | enemy stun lock at arrival if enemy was in path | -- |
| consumes | none | -- | -- |

state_owner note: Unity Light State overlay pending. Sprite records aura shimmer on post_blink_ready; Unity implements persistent Light State band UI.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | depart: white-blue light accumulation on body; radiant ring 2-3 px radius at departure point; fast |
| impact_particle | yes | path damage: radiant flash line between departure and arrival points (handled as line VFX in scene); enemies in path receive `radiant_jolt` |
| trail | no | teleport is instant; no motion trail needed |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `radiant_jolt` -- enemies on Blink path receive sudden body-shock flinch as caster passes through |
| audio_anchor_frame | depart F2 | sharp whoosh + radiant crack at arrival |

VFX layer count: 3 (cast_particle, impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | teleport with radiant flash, Light State build -- Elementalist owns Lightbreak and radiant |
| Avoids every AVOIDS item | PASS | Blink is a movement spell, not a physical trap or snare |
| Counter-archetype distinct (Rule #57) | n/a | Blink is a movement skill, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; stun trigger is path-contact conditional |
| No cross-class state confusion (Rule #58) | PASS | white-blue radiant flash distinct from Shadowblade shadow-blink, Warblade amber charge |
| No Sundered/armor crack VFX (Rule #55) | PASS | radiant flash has no armor-fissure element |
| Silhouette distinct from class neutral at 64px | PASS | dissolution and light-coalesce posture clearly differ from any other skill in kit |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (depart, in_transit, arrive, post_blink_ready) |
| frames_per_row | 2, 1, 2, 2 |
| palette_ref | Elementalist class palette (muted blue-grey base, Light element color: white-blue radiant) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P1 -- core mobility skill; required for Frozen Orb and Light State chain testing |

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
