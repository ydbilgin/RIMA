# SKILL VISUAL CONTRACT -- Brawler: Counter Blow

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_COUNTER_BLOW` |
| display_name | Counter Blow |
| slot | basic |
| role | counter |
| state_owner | yes (reads incoming hit to trigger counter; +3 Charge on trigger) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: Brawler rolls through incoming damage and punches back mid-motion. Active weave during the 0.4s window -- NOT standing still waiting. Body dips and rotates, punch fires from the hip as the incoming attack passes. Reads as ugly instinct, not discipline.

Disambiguation note (counter archetype, Rule #57): Brawler counter = weave + punch-through (body in motion throughout, off-axis). Ronin counter = sheathed stillness, pre-draw (weapon never visible until release). These must be visually non-overlapping. Brawler must NEVER be in a composed waiting pose.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | active weave stance -- low bob, guard up but shifting; shows the 0.4s window is open | body must be in motion; NO static guard pose (Ronin visual territory) |
| active | yes | 4 | weave dip (F1-F2), counter punch fires from hip rotation (F3 peak), extension snap (F4) | 200% counter punch lands on F3; audio anchor F3; Charged State: F3-F4 extend to brief stun burst |
| recovery | yes | 2 | overcommit lean after punch, weight forward | punishable if whiffed after window |
| cancel | no | -- | -- | window expires naturally, no cancel state |

Frame total: 9. Within Brawler basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | +3 Charge on counter trigger | 3-pulse brown-flash on caster knuckles at F3 contact | same rapid knuckle-pulse as Crackjaw |
| applies | Charged State: 350% + brief stun | bruise-purple knuckle burst on F3 when Charged; stun ring on enemy | 2-frame white daze ring above enemy head |
| reads | Charged State on caster activates upgraded counter damage | caster forearms show purple glow during wind_up weave | glow appears on F1 of wind_up if Charged State is active |
| consumes | Charged State consumed by trigger | purple glow collapses inward at F3 contact | |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no cast particle -- stance is entered silently |
| impact_particle | yes | hip-driven heavy dust burst on F3 (heavier than jab, asymmetric); Charged State adds bruise-purple ring |
| trail | yes | forearm/knuckle smear on counter punch (F3), 1-frame motion blur |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `cracked` reaction set on base counter; Charged State 350% upgrades to `shattered` if Cracked stack present; stun adds daze ring |
| audio_anchor_frame | active F3 | counter punch is the audible beat; Charged State adds secondary impact layer |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | whiff body counter (OWNS), Charge pips, Cracked/Shattered |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | PASS | Brawler weaves and punches through (body in motion throughout); Ronin holds sheathed in composed stillness and draws; no overlap in pose or timing |
| No HP-execute visual cue (Rule #56) | PASS | Charged State upgrade reads Charge resource, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Brawler Cracked = organic body fissures; no plate shards; Sundered is Warblade-only |
| Silhouette distinct at 64px | PASS | low weave dip + hip-rotation punch is a unique silhouette; distinct from neutral and from upright Ronin draw |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 4, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 9 frames |
| priority | P1 |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
