# SKILL VISUAL CONTRACT -- Elementalist: Glacial Spike

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_GLACIAL_SPIKE` |
| display_name | Glacial Spike |
| slot | 2 (Core) |
| role | Frost line damage / slow / Fire State consumer / Freeze trigger |
| state_owner | no (builds Frost State +2; Frost State is a resource counter, HUD-only) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: a line spell -- ice spike projected in a 6m straight line from the caster. The visual shape is a sharp elongated shard traveling at medium speed through all enemies in its path. Cold blue-white palette on the shard itself; the slow debuff is communicated by a frost film on hit enemies. The element reaction with Fireball (Freeze 2s + DoT pop) is a hit-effect interaction -- no new caster animation needed. The cast gesture is the opposite arm from Fireball: one hand draws back then thrusts forward in a stabbing motion.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| cast_start | yes | 2 | arm draws back with fingers tensed -- cold energy gathering in fist; frost mist forms at knuckles | frame 1: draw back; frame 2: frost mist visible at fist |
| cast_release | yes | 1 | arm thrusts forward in stabbing motion -- spike launches as a line projectile | single thrust frame; spike departs as elongated shard |
| recovery | yes | 1 | arm retracts; caster holds ready stance | |

Frame total: 4. Within Elementalist core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Frost State +2 on caster | HUD stack counter increments | no per-cast caster sprite change |
| applies | slow %40 + %180 damage on hit target | frost film on target; slow communicated by target movement stutter | ice shard impact VFX |
| consumes | 1 Fire State stack (if held by caster) | Fire State HUD decrements | no caster sprite change; element reaction handles in code |
| reads | Fireball DoT active on target (synergy: Freeze 2s + DoT pop) | no caster visual change; Freeze reaction plays as hit-effect on target | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | frost mist forming at fist on cast_start F2: cold blue-white wisps 2 px; sub-zero chill feel |
| impact_particle | yes | shard hit on enemy: sharp ice fragments scatter 2-4 px; frost film covers hit area; blue-white only |
| trail | yes | shard in flight: elongated ice trail behind shard (line shape reinforcement); cold light 1-2 px |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `frost_flinch` -- enemy recoil + frost film overlay on target sprite |
| audio_anchor_frame | cast_release F1 | sharp ice crack launch + piercing freeze on impact |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | line spell shape, Frost element, stabbing cast gesture -- pure Elementalist |
| Avoids every AVOIDS item | PASS | ice line is a spell, not a physical trap or snare wire (Ranger territory) |
| Counter-archetype distinct (Rule #57) | n/a | Glacial Spike is a direct attack, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; element reaction triggers on DoT state, not HP |
| No cross-class state confusion (Rule #58) | PASS | cold blue-white shard distinct from Warblade amber, Shadowblade dark lines |
| No Sundered/armor crack VFX (Rule #55) | PASS | frost film on target is cold-skin effect, not an armor fissure |
| Silhouette distinct from class neutral at 64px | PASS | draw-back and thrust stabbing arm differs from Fireball palm-push and neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (cast_start, cast_release, recovery) |
| frames_per_row | 2, 1, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, frost element color: cold blue-white) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 4 frames |
| priority | P1 -- core Frost spell; required for element reaction chain testing |

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
