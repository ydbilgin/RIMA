# SKILL VISUAL CONTRACT -- Brawler: Weave

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_WEAVE` |
| display_name | Weave |
| slot | basic |
| role | counter / reposition |
| state_owner | no (grants Charge on perfect timing; banks Overdrive Fuel at Charge 5; does not apply a class state to enemy) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: lateral head-and-shoulder duck-step, not a martial sidestep. Body stays low, eyes stay on target. Perfect-timing window = 0.2s (visual tell must be readable). At Charge 5 / Charged State: RMB routes to Overdrive Fuel banking instead of normal Weave.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| active | yes | 2 | lateral step -- F1 lean-initiation, F2 planted-side position | iframes active during both frames on perfect timing; silhouette low and wide on F2 |
| cancel | yes | 1 | snap-back to guard -- earliest return frame | cancel window opens after F2; shoulder roll-back reads as "recovered" |

Frame total: 3. Within Brawler basic quota.

No wind_up row: Weave is reactive, not telegraphed. No recovery row: cancel frame IS the recovery. At Charge 5, the cancel frame is replaced by a 1-frame Overdrive bank flash on caster (knuckle glow drains into V-meter pip; handled by HUD system, not spritesheet).

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none -- Weave does not apply a state to enemy | -- | -- |
| reads | Charge 5 / Charged State check on caster at activation | if Charge == 5: RMB redirects to Overdrive Fuel bank; caster emits single-frame energy drain pulse from knuckles inward to torso | brief white-gold drain line, 1 frame, no overlay; V-meter pip fill is HUD-side |
| consumes | none | -- | -- |

NONE applied to enemy. Self-state read (Charge 5 check) is caster-side only and handled via existing Charged State glow tier system; no new pip asset required.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | Weave has no cast tell -- reactive speed is the identity |
| impact_particle | no | no hit; Weave is movement only |
| trail | yes | on F1->F2 transition only: thin body-motion blur indicating lateral velocity; 1-frame smear, low opacity |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | no | Weave does not hit; enemy receives no reaction |
| audio_anchor_frame | active F1 | cloth-swish audio on step initiation |

VFX note: on perfect-timing success, add a secondary 1-frame brief charge-surge spark on caster knuckles (Charge+2 confirmation tell). This is a caster VFX only; no additional layer beyond the trail.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | body-movement only, shoulder-driven duck step; no weapon, no magic |
| Avoids every AVOIDS item | PASS | no weapon-armor break, no pre-draw counter stillness (Weave is reactive motion, not a held guard) |
| Counter-archetype distinct (Rule #57) | PASS | Weave is a dodge/reposition counter (body movement), not weapon absorb (Warblade) and not pre-draw tension pose (Ronin); lateral step is the visual differentiator |
| No HP-execute visual cue (Rule #56) | PASS | Charge 5 gate reads Charge resource, not target HP |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | no fissure or shard language; motion blur only |
| Silhouette distinct from class neutral at 64px | PASS | low lateral lean on F2 reads clearly against upright neutral guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 2 (active, cancel) |
| frames_per_row | 2, 1 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 3 frames |
| priority | P0 -- basic skill, blocks Brawler playable build |

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
