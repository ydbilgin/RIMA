# SKILL VISUAL CONTRACT -- Brawler: Flying Knee

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_FLYING_KNEE` |
| display_name | Flying Knee |
| slot | basic |
| role | opener |
| state_owner | no (grants Charge+2 on hit; does not apply a class state to enemy) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: dash + 0.5s LMB input triggers aerial knee strike. Body is fully airborne on active frames -- not a hop, a genuine leap. Small stagger on hit. Off-axis entry angle (body tilted, not centered). Ugly momentum, not acrobatic.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 1 | dash lean -- body pitched forward low, leading knee rising | communicates aerial intent; silhouette: wide horizontal lean |
| active | yes | 2 | F1: peak airborne -- knee fully extended into target; F2: contact moment -- knee planted, body weight behind it | audio anchor F2; impact lands on contact; F1 reads at 8m as incoming launcher threat |
| recovery | yes | 1 | land + stumble-step, guard hands re-raise | shows commitment cost; off-axis landing, not clean |

Frame total: 4. Within Brawler basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none -- Flying Knee does not apply a class state | -- | -- |
| reads | none -- no conditional state branch | -- | -- |
| consumes | none | -- | -- |

NONE. Charge+2 grant is a resource increment; no pip or overlay asset needed. Stagger on enemy is handled by hit_reaction_on_enemy row.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | dash initiation is a movement system event, not a cast |
| impact_particle | yes | heavy dust burst on knee contact (active F2); asymmetric spray, ground-direction bias (caster came from above) |
| trail | yes | body-arc motion smear F1->F2 showing aerial approach path; 1 frame, low opacity, horizontal orientation |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `generic_stagger` -- enemy rocks backward, no Cracked pip |
| audio_anchor_frame | active F2 | heavy thud on knee contact |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | body-weapon strike (knee), airborne momentum; no magic, no blade |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Flying Knee is opener, not counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; stagger is flat on hit |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | impact dust only; no fissure or shard language |
| Silhouette distinct from class neutral at 64px | PASS | fully airborne extended-knee pose is unambiguous vs ground-neutral stance |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 1, 2, 1 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 4 frames |
| priority | P0 -- basic skill, blocks Brawler playable build |

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
