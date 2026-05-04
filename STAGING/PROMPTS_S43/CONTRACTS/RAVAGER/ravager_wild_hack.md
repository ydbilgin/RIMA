# SKILL VISUAL CONTRACT -- Ravager: Wild Hack

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_WILD_HACK` |
| display_name | Wild Hack |
| slot | core |
| role | pressure / closer |
| state_owner | no (exposed window is SELF-applied on caster, not an enemy state; no pip on enemy) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a single catastrophic overhead swing that fully commits the body. After the hit the caster is wide open for 2s -- weapon driven into the ground or dragged to the side, body twisted out of guard. The power is real; the cost is real.

Exposed note (CRITICAL): the 2s exposed/vulnerable window is SELF-APPLIED on the Ravager. It is NOT an enemy debuff state. Do NOT place a pip or overlay on the enemy. The visual marker (glowing wound indicator) appears on the CASTER only -- a hot-red pulsing wound mark on the torso/side indicating guard is down. No enemy-state ownership by Ravager.

Chain unlock: if Ravager is hit while in the exposed window, Fury+40 and 0.8s invulnerability trigger. Invuln shows as a brief bone-white body flash on the caster.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | two-handed overhead raise, full extension up -- telegraphed, tells opponent a massive hit is coming | body stretched tall before coming down hard |
| active | yes | 3 | overhead slam down: F1 drop begins, F2 peak impact, F3 follow-through weapon into ground or low | impact anchor F2; massive hit feel |
| recovery | yes | 4 | 2s exposed state -- weapon low or planted, guard arm hanging, glowing wound marker on torso; F1-F4 covers the open window | F1: wound marker appears (hot-red pulse); F4: exposed window ends; punishable the entire duration |
| cancel | no | -- | not cancellable -- that is the point | |

Frame total: 10. Within Ravager core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | 2s exposed window on CASTER after active | glowing wound marker on Ravager torso during recovery F1-F4 -- hot red pulsing scar/mark | wound marker is on caster ONLY; NOT on enemy; NOT a pip on enemy portrait; reads as self-debuff |
| reads | being hit during exposed window triggers Fury+40 + 0.8s invuln | bone-white body flash on caster on hit during recovery (invuln trigger) | single-frame bone-white ring from torso; fades in 1 frame |
| consumes | none | -- | -- |
| disambiguation_note | self-applied exposed vs enemy state pips | Ravager exposed = glowing wound mark on CASTER torso only. No icon or pip on enemy health bar or body. Not to be confused with Bone Crack's 8s vital exposure (which is enemy-targeted, different skill). Wild Hack's window is self-risk only. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no cast particle -- wind_up is the tell |
| impact_particle | yes | heavy impact: red shockwave + bone/ground fragments on active F2; large radius, brutal feel |
| trail | yes | overhead slam arc, active F1->F2, hot red smear, heavy 2-frame blur; weapon-tip leads |
| screen_overlay | no | no screen overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` (heavy); no state pip on enemy |
| audio_anchor_frame | active F2 | slam impact is the audible beat; should be the heaviest single hit sound in Ravager kit |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | overhead slam, hot red palette, self-risk exposure mechanic fits HP-trade identity |
| Avoids every AVOIDS item | PASS | no armor-break language; no repositioning or fancy footwork; pure brute force commit |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | exposed window is time-based (2s), not HP-gated; no HP color cue on enemy |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | impact uses ground fragments and shockwave, not plate shards; no Sundered language |
| Silhouette distinct at 64px | PASS | two-handed overhead wind_up is unmistakably different from Ravager neutral; recovery slump with wound glow reads as exposed |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 3, 4 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
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
