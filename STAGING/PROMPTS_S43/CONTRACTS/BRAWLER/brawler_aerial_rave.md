# SKILL VISUAL CONTRACT -- Brawler: Aerial Rave

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_AERIAL_RAVE` |
| display_name | Aerial Rave |
| slot | signature |
| role | control / pressure |
| state_owner | yes (applies airborne juggle state to enemy; Charge is preserved on caster throughout) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: upward launch into a 3-hit midair juggle. Brawler leaps or drives enemy upward -- not a graceful aerial, a violent elevation followed by hammer hits. Hits land as enemy rises and peaks. Charge preserved means caster stance stays hot between hits.

Airborne juggle visual note: enemy rises with dot-trail indicating vertical lift. This is Brawler juggle (OWNS). It must NOT resemble Warblade Sundered (which is a ground-blast outward shard burst). Rising dots = juggle; outward shards = Sundered. Never use shard language here.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | low crouch-coil, weight shifts onto drive foot -- short tell | distinct from neutral; reads as upward launch prep |
| active | yes | 6 | launch strike (F1-F2), aerial hit 1 (F3), aerial hit 2 (F4), aerial hit 3 (F5 peak), landing recovery start (F6) | 3 aerial hits on F3-F5; Warblade combo variant extends to 5 hits (F3-F7); audio anchors F2 (launch), F5 (final hit) |
| recovery | yes | 2 | land and reset -- Charge aura remains visible on caster | |
| loop | no | -- | -- | |

Frame total: 10 (base). Warblade combo variant: 12 frames (2 extra aerial hits). Flag: if paired with Warblade Earthsplitter batch, generate 12-frame active row instead. Design must confirm batch pairing.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | airborne juggle state on enemy | enemy rises with dot-trail visual (rising dots above enemy, not outward shard burst) | 3-4 rising white dots above enemy body tracking upward arc; NOT horizontal; NOT metallic |
| reads | Charge preserved -- caster maintains current Charge level throughout | caster knuckle aura stays lit at current Charge tier; does not reset between hits | steady brown-or-purple glow depending on Charge tier |
| reads | After Warblade Earthsplitter: +2 aerial hits | caster shows brief gold-flash on wind_up if Earthsplitter was last skill used | 1-frame gold knuckle flash on wind_up F1 |
| consumes | none | -- | -- |
| disambiguation_note | Brawler juggle (rising dot-trail) vs Warblade Sundered (outward shard burst) | Brawler: enemy rises vertically with dot arc; no metallic shards, no ground crater | Warblade Sundered = horizontal plate-shard blast from ground impact; must never appear in Aerial Rave |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | launch is from body movement, no cast particle |
| impact_particle | yes | launch strike (F2): upward dirt-burst from point of contact; each aerial hit (F3-F5): small compressed puff at contact height |
| trail | yes | caster forearm smear on F2 (upward launch) and F5 (final aerial hit); 1-frame motion blur |
| screen_overlay | no | signature skill -- overlay permitted in principle, but not needed here; leave no |
| hit_reaction_on_enemy | yes | `cracked` reaction set during aerial hits; rising dot-trail overlay on enemy during airborne duration |
| audio_anchor_frame | active F2 and F5 | F2 = launch beat, F5 = final aerial hit beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | launch/juggle (OWNS), Charge preservation, off-axis body |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Aerial Rave is control/pressure, not counter |
| No HP-execute visual cue (Rule #56) | PASS | airborne state and Charge preservation are resource/state reads, not HP checks |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | rising dot-trail is juggle (Brawler OWNS); no outward shard burst; no metallic plate language; Sundered is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | vertical launch coil + aerial punch extension is a unique silhouette vs any ground-bound attack |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 6, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 10 frames (base); 12 frames if Warblade combo variant included |
| priority | P1 |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Warblade Earthsplitter contract must be in same batch if 5-hit variant is generated) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
