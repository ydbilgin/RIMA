# SKILL VISUAL CONTRACT -- Elementalist: Solar Flare

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_SOLAR_FLARE` |
| display_name | Solar Flare |
| slot | 9 (Advanced) |
| role | cursor-aimed cone radiant burst / Light State amplifier |
| state_owner | no (no caster state applied) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: a cone of intense solar radiant light projected from the caster's raised palm in the cursor direction. The cone shape is wide at the far end -- a spreading fan of light that pierces all enemies it intersects. Light State active adds an extra radiant pulse ripple in the cone after the initial burst. The gesture is one arm raised at chest height, palm facing forward, then a sharp forward push as the cone fires. Solar energy: warm white-gold, not cold blue. This is the Light element at maximum output for an Advanced skill.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | arm rises to chest height, palm faces forward -- solar radiance accumulates in palm; warm gold glow | frame 1: arm rises; frame 2: palm charged with gold-white glow, ready |
| active | yes | 2 | sharp forward push -- cone fires; arm fully extended; palm flares to maximum brightness | frame 1: push mid-extension; frame 2: fully extended, cone visible as outward spread |
| radiant_pulse | yes | 1 | Light State active only: after main cone, a secondary pulse wave ripples outward from palm | 1 frame pulse extension; conditional, plays after active F2 if Light State present |
| recovery | yes | 2 | arm lowers; palm dims; caster tilts head back (brief recoil from sustained light output) | 2 frames; advanced skill deserves a slightly longer recovery beat |

Frame total: 7. Within Elementalist advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | radiant damage to all enemies in cone | cone VFX + hit reactions on intersecting enemies | no caster state |
| reads | Light State active (synergy: extra radiant pulse in cone) | radiant_pulse frame fires after active F2 if Light State present | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | palm charge on wind_up F2: warm gold-white glow accumulating; 2-3 px radius; light leaks at finger edges |
| impact_particle | yes | cone hit on each enemy: radiant spark burst at intersection point; gold-white; radiant_pulse adds secondary softer burst ring per enemy |
| trail | no | cone is a spell shape VFX rendered in scene; no caster sprite trail |
| screen_overlay | no | advanced skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `radiant_pierce` -- body illuminated and pushed back by cone light |
| audio_anchor_frame | active F1 | solar flare crack + warmth rush |

VFX layer count: 3 (cast_particle, impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | cone spell shape, Light/solar element -- Elementalist OWNS light-radiant shapes |
| Avoids every AVOIDS item | PASS | cone is a pure spell burst; no physical grenade or thrown device |
| Counter-archetype distinct (Rule #57) | n/a | Solar Flare is an active attack, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Light State synergy is class-state-gated, not HP |
| No cross-class state confusion (Rule #58) | PASS | warm gold-white cone distinct from Prism Beam (prismatic white), Fireball (red-orange), Warblade amber |
| No Sundered/armor crack VFX (Rule #55) | PASS | radiant pierce has no armor-fissure element |
| Silhouette distinct from class neutral at 64px | PASS | horizontal forward-push palm at chest height is distinct from raised-skyward Meteor and vertical Fireball push |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, radiant_pulse, recovery) |
| frames_per_row | 2, 2, 1, 2 |
| palette_ref | Elementalist class palette (muted blue-grey base, Light element solar variant: warm gold-white) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P2 -- advanced Light skill; secondary to P0/P1 core chain |

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
