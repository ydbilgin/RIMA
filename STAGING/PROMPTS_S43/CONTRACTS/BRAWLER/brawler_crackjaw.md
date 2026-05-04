# SKILL VISUAL CONTRACT -- Brawler: Crackjaw

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_CRACKJAW` |
| display_name | Crackjaw |
| slot | basic |
| role | pressure / closer |
| state_owner | yes (applies +3 Charge; Charged State triggers bonus on last hit) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: four-hit dirty combo -- jab, cross, hook, uppercut. Each punch adds momentum; uppercut is the payoff. Body is off-axis, shoulders drive. Not textbook boxing -- street-finish energy.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | short drop-step, lead shoulder rolls forward -- reads as chain opener | silhouette must differ from neutral stance |
| active | yes | 6 | jab (F1), cross (F2), hook (F3), uppercut net pose (F4 peak -- 4-frame net pose hold), lunge-forward F5-F6 | 5m forward lunge visible on F5; audio anchors: F1 jab, F4 uppercut |
| recovery | yes | 2 | uppercut overreach, off-balance lean -- punishable window | |
| cancel | no | -- | -- | not cancellable mid-chain |

Frame total: 10. Within Brawler basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | +3 Charge on chain completion | Charge counter increments 3 steps on caster | brown-pulse flicker on knuckles (3 quick pulses, one per Charge added); no enemy pip |
| reads | 5 Charge = Charged State: last hit (uppercut) plays Charged variant | caster knuckles glow bruise-purple on F4 when entering Charged State | single-frame purple knuckle burst on uppercut contact; no screen overlay |
| consumes | Charged State consumed by last hit | uppercut burst collapses charge glow | purple glow collapses inward on F4 impact, small shockwave ring |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | chain opens from movement, no telegraphed cast particle |
| impact_particle | yes | dust-puff on each hit (F1-F3 light, F4 heavy uppercut ring); Charged State F4 adds bruise-purple ring burst |
| trail | yes | knuckle smear on cross (F2) and uppercut (F4); 1-frame motion blur each |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `cracked` reaction set on F1-F3; Charged State last hit upgrades to `shattered` if third Cracked stack already present |
| audio_anchor_frame | active F4 | uppercut is the audible beat; secondary spike at F1 |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Charge pips, Cracked/Shattered, off-axis body |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Crackjaw is pressure/closer, not counter |
| No HP-execute visual cue (Rule #56) | PASS | Charged State reads Charge count (resource), not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Brawler Cracked = organic body hairline fissures; NO metal plate shards; Sundered shard language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | four-punch chain with forward lunge creates horizontal motion arc distinct from neutral |

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
| gen_budget_estimate | 10 frames |
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
