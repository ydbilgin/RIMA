# SKILL VISUAL CONTRACT -- Elementalist: Blizzard

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_BLIZZARD` |
| display_name | Blizzard |
| slot | 12 (Master) |
| role | zone-independent AoE slow+tick field / Meteor knockdown extender |
| state_owner | no (applies slow and blizzard field; no caster state; caster movement continues) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: 1-second cast then an 8-second persistent blizzard zone that the Elementalist does not need to maintain (no channel, movement continues after cast). The blizzard is a cursor-placed zone -- large AoE of falling ice shards and slow-tick. The caster gesture is both arms raised high then pulled apart and downward in an arc -- calling the storm from above across a wide area. Master-tier weight: the arms-wide arc is a large imposing silhouette. Before Meteor: blizzard slow primes the Meteor for 4s knockdown. The blizzard persists zone-independently -- the caster can reposition freely.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 4 | both arms rise together overhead then spread wide in an arc -- calling the storm; 1s wind-up | frame 1: both arms rise; frame 2: arms reach overhead apex; frame 3: arms begin spreading wide; frame 4: arms fully spread arc -- storm called |
| cast_release | yes | 2 | both arms pull downward in final arc push -- blizzard zone materializes at cursor target | frame 1: downward pull; frame 2: arms at sides, zone confirmed; caster free to move |
| recovery | yes | 1 | arms lower fully; caster resumes movement-ready stance | 1 frame; zone persists without caster attention |

Frame total: 7. Within Elementalist Master quota.

Blizzard zone VFX is scene-level (falling ice shards, slow indicator ring) -- no caster animation loop needed during the 8s zone duration. The caster returns to neutral idle or other skills immediately.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | blizzard zone (8s): slow + tick damage in AoE | persistent zone VFX in scene; no caster indicator after cast | slow film on all enemies in zone; ice shards falling |
| reads | before Meteor cast (synergy: Meteor knockdown 4s instead of 2s) | no caster visual change; Meteor knockdown modifier handled in code when blizzard zone overlaps Meteor target | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | wind_up: cold energy builds in outstretched arms as they spread; ice crystal wisps form at fingertips on frame 3-4; cold blue-white |
| impact_particle | yes | cast_release: blizzard zone materializes -- ice shards begin falling across target zone; wide-area AoE indicator ring (cold blue) appears at cursor point |
| trail | no | no caster movement trail |
| screen_overlay | no | Master skill -- overlay forbidden (class house rule applies) |
| hit_reaction_on_enemy | yes | `blizzard_slow` -- enemies in zone receive continuous frost slow; periodic ice shard impact on sprite |
| audio_anchor_frame | cast_release F1 | deep cold rumble + storm opening crack |

VFX layer count: 3 (cast_particle, impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | wide-arc storm-calling gesture, Frost element, AoE zone spell -- Elementalist Master signature |
| Avoids every AVOIDS item | PASS | blizzard zone is a summoned spell field, not a physical trap array (Ranger territory) |
| Counter-archetype distinct (Rule #57) | n/a | Blizzard is a zone caster, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Meteor synergy is a spatial-overlap conditional |
| No cross-class state confusion (Rule #58) | PASS | cold blue blizzard zone is distinct from Warblade amber shockwave, Ranger mine fields |
| No Sundered/armor crack VFX (Rule #55) | PASS | frost zone has no armor-fissure element |
| Silhouette distinct from class neutral at 64px | PASS | both-arms-raised wide arc is the largest caster silhouette in Elementalist kit; unmistakable Master tier |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, cast_release, recovery) |
| frames_per_row | 4, 2, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, Frost element: cold blue-white at maximum Master intensity; arm arc wider than any previous skill) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P1 -- Master AoE skill; required for Meteor synergy chain testing |

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
