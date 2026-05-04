# SKILL VISUAL CONTRACT -- Brawler: Shove Off

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_SHOVE_OFF` |
| display_name | Shove Off |
| slot | basic |
| role | reposition / control |
| state_owner | yes (+1 Charge per enemy hit; 4+ enemies = instant 5 Charge / Charged State) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: wide two-handed shove from the center -- the Brawler takes up space aggressively. Arms extend low and wide, palms-out, like clearing a bar table. Not a martial technique. Body stays low and planted.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | crouch-plant, wide arms beginning to spread -- short tell | reads as area coverage, not single target |
| active | yes | 4 | both arms sweep wide (F1-F2), full extension shove (F3 peak), recoil snap (F4) | all nearby enemies pushed on F3; +1 Charge per enemy flashes on caster; audio anchor F3 |
| recovery | yes | 2 | palms-out low stance, slight off-balance from push force | |

Frame total: 8. Within Brawler basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | +1 Charge per enemy hit | one brown-pulse pip per enemy struck, visible on caster; stacks fast if many enemies | rapid brown knuckle-pip pulses; each enemy contact triggers one |
| applies | 4+ enemies -> instant 5 Charge / Charged State | Charged State glow activates on caster immediately | full bruise-purple knuckle aura bursts on at 4th enemy contact |
| reads | none -- skill does not read existing states | -- | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | movement-initiated, no cast particle |
| impact_particle | yes | wide dust-wave emanating from caster on F3 (radial burst, low arc, not upward); one small dust-puff per enemy hit position |
| trail | no | no trail -- this is a shove not a strike |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `generic_stagger` on each enemy pushed; no Cracked (Shove Off is blunt force, not fissure-type) |
| audio_anchor_frame | active F3 | wide shove is the audible beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Charge pips, off-axis wide body, street-fight energy |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Shove Off is reposition/control, not counter |
| No HP-execute visual cue (Rule #56) | PASS | Charged State reads Charge count (resource), not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no fissure or shard language; stagger is blunt-force only |
| Silhouette distinct at 64px | PASS | wide bilateral arm extension is a unique silhouette vs any single-side punch pose |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 4, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 8 frames |
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
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
