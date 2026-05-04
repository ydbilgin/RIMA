# SKILL VISUAL CONTRACT -- Elementalist: Fireball

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_FIREBALL` |
| display_name | Fireball |
| slot | 1 (Core, starred) |
| role | primary fire damage / Fire State builder / Living Bomb trigger setup |
| state_owner | no (builds Fire State on caster; Fire State is a resource counter, not a Unity overlay state) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: academic spell-weaver casting an orb of fire. The projectile is a clean tight orb -- not a chaotic fireball, not an explosion on cast. Cast gesture is controlled: arm extends, hand opens, orb forms and launches. Fire DoT on the enemy is communicated by a persistent ember-flicker VFX on the target, not on the caster. Fire State +1 stacks silently on the caster HUD. On the 3rd consecutive Fireball, Living Bomb is triggered free -- no animation change on caster; Living Bomb plays its own sequence.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| cast_start | yes | 2 | arm extends forward, hand opens -- orb forming in palm | frame 1: arm raise; frame 2: orb visible in hand, glowing red-orange |
| cast_release | yes | 1 | hand pushes forward -- orb launches; palm follow-through | single decisive launch frame; orb departs as projectile |
| recovery | yes | 1 | arm retracts; caster returns to neutral ready stance | fast recovery -- Fireball is a repeatable spam skill |

Frame total: 4. Within Elementalist core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Fire State +1 on caster | HUD stack counter increments | no per-cast caster sprite change; resource is HUD-only |
| applies | Fire DoT 4s on hit target | target receives ember flicker VFX (projectile hit effect) | ember particles on target sprite; handled as hit VFX layer |
| reads | 3rd consecutive cast (synergy: Living Bomb triggered free) | no visual change on caster; Living Bomb fires independently | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | orb formation in palm on cast_start F2: tight red-orange glow ball 2-3 px diameter; warm light |
| impact_particle | yes | orb hit on enemy: small burst 3-4 px radius; ember fragments scatter; ember-flicker remains on target as DoT indicator (2-3 frame dissolve loop handled by hit-effect system) |
| trail | yes | orb in flight: short warm light trail 2-3 px behind projectile; thin, not explosive |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `fire_flinch` -- standard hit recoil; ember sparks on target body |
| audio_anchor_frame | cast_release F1 | soft orb whoosh + impact fire crack |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | orb spell shape, Fire element, palm cast gesture -- no physical trap or weapon |
| Avoids every AVOIDS item | PASS | no ground trap, no net, no physical mechanism -- pure spell |
| Counter-archetype distinct (Rule #57) | n/a | Fireball is a direct attack, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Living Bomb trigger is a cast-count conditional, not an HP read |
| No cross-class state confusion (Rule #58) | PASS | red-orange fire orb distinct from Warblade amber Rage, Shadowblade dark orb |
| No Sundered/armor crack VFX (Rule #55) | PASS | fire orb has no armor-fissure effect; impact is heat burst only |
| Silhouette distinct from class neutral at 64px | PASS | extended arm with palm-open orb clearly differs from neutral ready stance |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (cast_start, cast_release, recovery) |
| frames_per_row | 2, 1, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, fire element color on spell VFX only: red-orange) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 4 frames |
| priority | P0 -- starred core skill, blocks Elementalist playable build |

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
