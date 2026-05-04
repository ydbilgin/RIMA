# SKILL VISUAL CONTRACT -- Elementalist: Living Bomb

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_LIVING_BOMB` |
| display_name | Living Bomb |
| slot | 3 (Core) |
| role | delayed AoE explosion / chain propagation on kill |
| state_owner | no (applies Living Bomb marker to target; no caster state) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: the Elementalist plants a magical time-delayed orb on a target -- the enemy becomes a walking bomb. The 5-second delay is communicated by a pulsing red-orange orb embedded in the target's sprite (handled by hit VFX system, not the caster animation). The explosion on detonation is a large sphere of fire. On kill, the bomb copies to 3 nearby enemies -- a chain propagation that is handled in code with the same explosion VFX replaying. Under Glacial Spike slow, explosion radius doubles. Cast animation is deliberate two-hand gesture: both hands come together, then project the bomb orb forward.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| cast_start | yes | 3 | both hands come together at chest -- fire orb builds between palms; larger and slower than Fireball orb | frame 1: hands converge; frame 2: orb forming between palms; frame 3: orb at full size, pulsing |
| cast_release | yes | 2 | both hands push forward -- orb launches and embeds in target; follow-through with both arms | frame 1: push launch; frame 2: hands spread after release |
| recovery | yes | 1 | return to ready stance | |

Frame total: 6. Within Elementalist core quota.

Living Bomb detonation is a separate hit-effect VFX event after 5s delay -- no additional caster animation row needed. Explosion sphere radius indicator handled by game system.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Living Bomb marker on target (5s delay) | pulsing embedded orb on target sprite (HUD/hit-effect system) | no caster sprite indicator |
| reads | Glacial Spike slow on target (synergy: explosion radius 2x) | no caster visual change; radius doubled in code | -- |
| reads | 3rd Fireball cast (synergy: Living Bomb triggered free, no mana cost) | no caster sprite change; triggered from Fireball combo logic | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | orb building between palms on cast_start: warm red-orange glow grows from 1 to 3 px diameter; slower pulse than Fireball (communicates weight/danger of delayed bomb) |
| impact_particle | yes | bomb embed on target: orb embeds into target with soft thud flash; pulsing red embedded orb persists for 5s (hit-effect system handles persistence) |
| trail | yes | orb in flight: short heavy trail behind orb; red-orange, wider than Fireball trail (3 px) -- communicates this is heavier/slower than basic Fireball |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `bomb_embed` -- light flinch on embed; heavy `explosion_knockback` at detonation (detonation is a separate hit event from the delayed timer) |
| audio_anchor_frame | cast_release F1 | deep thud + embedded pulse; explosion boom at detonation (separate audio event) |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | orb spell shape, fire element, two-hand cast -- Elementalist |
| Avoids every AVOIDS item | PASS | delayed bomb is a magical orb embed, not a physical trap or mine (Ranger territory); no physical placement gesture |
| Counter-archetype distinct (Rule #57) | n/a | Living Bomb is an active cast, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; all triggers are spell-state or cast-count gated |
| No cross-class state confusion (Rule #58) | PASS | pulsing red orb on target is distinct from Warblade Sundered fissure, Shadowblade mark |
| No Sundered/armor crack VFX (Rule #55) | PASS | explosion is fire burst, no armor fissure element |
| Silhouette distinct from class neutral at 64px | PASS | two-hand convergence and push gesture is unique in Elementalist kit vs single-arm Fireball and Glacial Spike |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (cast_start, cast_release, recovery) |
| frames_per_row | 3, 2, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, fire element color: red-orange; bomb orb is deeper red than Fireball) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 6 frames |
| priority | P1 -- core fire chain skill; required for Fireball combo testing |

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
