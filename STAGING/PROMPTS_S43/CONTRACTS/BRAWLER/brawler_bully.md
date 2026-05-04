# SKILL VISUAL CONTRACT -- Brawler: Bully

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_BULLY` |
| display_name | Bully |
| slot | basic |
| role | opener / pressure |
| state_owner | yes (applies Cracked, escalates to Shattered on third stack) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: street/foul, not martial-clean. Body language is shoulder-driven, off-axis -- NOT samurai-stance. Reads as a guy who fights ugly.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | low shoulder dip, lead foot scuff -- short tell, this is a fast jab | |
| active | yes | 4 | three-jab chain (jab-jab-cross), final cross is the launcher hint frame | impact: jabs on F1+F2, cross on F4; audio anchor F4 |
| recovery | yes | 2 | overcommitted lean, rear hand low -- punishable if whiffed | |
| cancel | yes | 1 | shoulder roll-out, allowed only after jab #2 | |

Frame total: 9. Within Brawler basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | skill stacks Cracked on target | each connecting jab adds 1 Cracked pip on enemy chest; third stack flips to Shattered overlay (full-body hairline crack decal, 2 frames) | small spider-crack decal, white-on-dark, no metal shards (those are Warblade Sundered) |
| reads | if target already Off-Balance, cross frame plays launcher variant | caster gets a brief knuckle flash on F3->F4 | single-frame yellow knuckle pop, no screen FX |
| consumes | none | -- | -- |
| disambiguation_note | Cracked/Shattered (Brawler) vs Sundered (Warblade) | Brawler: organic hairline body fissures, no metal fragments | Warblade Sundered uses plate shards + metallic burst; Brawler must NEVER use shard language |

Cracked/Shattered are Brawler-OWNED. No Warblade armor-break shard language -- use hairline cracks, not plate fragments.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | Bully has no telegraphed cast -- that is the point |
| impact_particle | yes | small dust-puff on each jab contact, asymmetric (street-fight feel); cross gets a heavier dust ring |
| trail | yes | knuckle smear on cross only (active F4), 1-frame motion blur |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `cracked` reaction set; on third stack swap to `shattered` |
| audio_anchor_frame | active F4 | cross is the audible beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Cracked/Shattered, off-axis body |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Bully is opener, not counter |
| No HP-execute visual cue (Rule #56) | PASS | launcher conditional reads Off-Balance state, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Brawler Cracked = organic body hairline fissures, NOT metal plate shards; Sundered shard language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | shoulder dip + jab extension reads vs Brawler neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, recovery, cancel) |
| frames_per_row | 2, 4, 2, 1 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 9 frames |
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
