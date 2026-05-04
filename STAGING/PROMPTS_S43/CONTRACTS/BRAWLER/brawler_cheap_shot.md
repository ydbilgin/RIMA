# SKILL VISUAL CONTRACT -- Brawler: Cheap Shot

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_CHEAP_SHOT` |
| display_name | Cheap Shot |
| slot | basic |
| role | pressure / control |
| state_owner | yes (applies defense debuff state to enemy; +3 Charge to caster) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: dirty, opportunistic. Not a clean strike -- a thumb to the throat or knuckle to the solar plexus. Animation should read as intentionally illegal. Shoulder caves inward on delivery.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | feint lean -- disguised as neutral, short tell | silhouette must not telegraph the dirty strike clearly |
| active | yes | 3 | off-axis punch delivery (F1-F2), sneer/lean-away F3 | impact on F2; audio anchor F2 |
| recovery | yes | 2 | nonchalant shoulder roll -- short recovery, this is a pressure skill | |

Frame total: 7. Within Brawler basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | defense -40% debuff on enemy, 6s | enemy gets a bruise-purple body pip (smear mark on torso, not an armor crack) | single pip: dark-purple smear decal on enemy chest, fades at 6s |
| applies | +3 Charge on caster | 3-pulse brown-flash on caster knuckles | same knuckle-pulse pattern as Crackjaw |
| reads | Charged State: debuff becomes -60% + 1s stun | caster knuckles glow purple on active F1; stun plays on enemy as brief daze ring | daze ring = 2-frame white star-ring above enemy head |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no telegraphed cast -- that is the point of Cheap Shot |
| impact_particle | yes | low dust-smear on contact (F2), asymmetric; Charged State adds small purple bruise-burst |
| trail | no | no trail -- strike is compact and sneaky |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `generic_stagger` on base; Charged State version adds `bleed` or daze ring (no Cracked -- this is not a fissure hit) |
| audio_anchor_frame | active F2 | impact is the audible beat; Charged stun adds secondary chime at F2 |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | bruise-purple debuff mark, off-axis body, Charge pips |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Cheap Shot is pressure/control, not counter |
| No HP-execute visual cue (Rule #56) | PASS | debuff reads enemy defense state, not HP; Charged State reads Charge resource |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | debuff pip is a bruise smear, NOT a fissure or shard; no Cracked/Sundered language |
| Silhouette distinct at 64px | PASS | feint lean + compact off-axis punch reads vs neutral; recovery nonchalance is a readable contrast |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 7 frames |
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
