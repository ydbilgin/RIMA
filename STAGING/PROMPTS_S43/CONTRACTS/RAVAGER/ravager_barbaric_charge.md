# SKILL VISUAL CONTRACT -- Ravager: Barbaric Charge

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_BARBARIC_CHARGE` |
| display_name | Barbaric Charge |
| slot | core |
| role | reposition / control |
| state_owner | no (stun on wall impact is contextual, not a class-owned pip state) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a straight-line shoulder-first bull run. Pushes everything out of the way. Stun/root immune while charging -- cannot be stopped mid-run. If the Ravager hits a wall: 2s stun (on caster -- disoriented from wall impact). Tone is pure aggression, no elegance.

Movement class note: this IS a movement/dash skill but it fits Ravager identity because it is brute-force linear and uncontrolled (no arc, no redirect). Fancy movement (Shadowblade) = acrobatic arcs, repositioning slides, aerial evasion. Barbaric Charge = head-down bull rush. These must read differently at 64px.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | low shoulder drop, weight shifts to the charge leg -- tells "incoming" | body leans hard into the run direction; head down |
| active | yes | 4 | straight-line charge: F1 launch, F2-F3 mid-run (loop-able), F4 contact frame where enemies are pushed | push reaction on enemies at F2-F4; stun/root immune the entire active phase |
| recovery | yes | 3 | skid stop; if wall impact: F1-F3 dazed stagger (2s stun on caster, head shake, weapon dragging) | wall impact version: 2s stun displayed as stagger loop on F1-F3 |
| cancel | no | -- | stun/root immune -- cannot be cancelled by enemies; no self-cancel | |

Frame total: 9. Within Ravager core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | push on all enemies in straight-line path | enemies in path stagger-launch sideways; generic_stagger reaction; no pip | enemies visibly pushed out of path; no state overlay on them |
| applies (wall impact) | hitting wall -> 2s stun on CASTER | dazed overlay on Ravager during recovery F1-F3: brief star/spark flicker at head + stagger animation | stun indicator is on caster only; bone-white spark flicker at head; 2s duration |
| reads | stun/root immunity during active | hot-red body outline on active F1-F4 indicating immune state | thin hot-red outline on caster during active; fades at recovery |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | wind_up is the tell |
| impact_particle | yes | ground dust trail along charge path (active F1-F4); push contact: burst of red dust + body-impact fragments on each enemy contact at active F4 |
| trail | yes | body motion trail on active F2-F3, hot red, 2-frame smear; shoulder-led (NOT a weapon trail -- the weapon is tucked for the charge) |
| screen_overlay | no | no screen overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` (push-type); enemies launch sideways from path |
| audio_anchor_frame | active F4 | collision frame is the audible beat; heavy thud + rumble |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | bull-rush linear charge, hot red palette, brute pushing mechanic fits OWNS |
| Avoids every AVOIDS item | PASS | NOT fancy movement (Shadowblade): no arc, no redirect, no aerial; charge is purely linear and shoulder-first; no armor-break language |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | wall stun is geometry-based (wall contact), not HP-gated; no HP color cue |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | contact uses dust + fragment spray; no plate shards, no Sundered language |
| Silhouette distinct at 64px | PASS | low shoulder-drop charge silhouette with body trail is unmistakably different from Ravager neutral and from Shadowblade dash |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 4, 3 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| gen_budget_estimate | 9 frames |
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
