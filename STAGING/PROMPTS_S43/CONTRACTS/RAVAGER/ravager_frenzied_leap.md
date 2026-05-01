# SKILL VISUAL CONTRACT -- Ravager: Frenzied Leap

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_FRENZIED_LEAP` |
| display_name | Frenzied Leap |
| slot | signature |
| role | reposition / pressure |
| state_owner | no (Frenzy buff is self-state on caster, not an enemy state) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a desperate forward throw of the whole body -- arms wide, weapon forward, landing with full mass impact. NOT a graceful arc. NOT acrobatic. Reads as someone hurling themselves as a weapon. Fancy movement (Shadowblade) must be avoided: no flourish, no elegant arc, no aerial pose.

Fury 80%+ variant: identical animation but active frame gets a 15% lifesteal effect (visible as a brief red pull on landing) + 2s CC immunity (body flare on recovery start).

Chain unlock: 3 different leap targets hit -> 5s Frenzy buff (+50% damage). Frenzy is a self-state buff; it is represented by a persistent body aura on the caster (see Section C).

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | crouch-and-launch -- knees bent hard, weapon tucked for throw; reads explosive not graceful | body low; no tiptoeing |
| active | yes | 4 | airborne arc F1-F2, peak height F2, descent F3, slam landing F4 | landing AoE triggers on F4; audio anchor F4 |
| recovery | yes | 3 | ground impact recovery -- knee planted, weapon driven down, dust settling | punish window; if Fury 80%+ F1 of recovery shows CC-immunity body flare |
| cancel | no | -- | CD resets on landing hit; no cancel state needed | |

Frame total: 9. Within Ravager advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | 3 distinct targets hit -> Frenzy 5s buff on CASTER | persistent hot-red body outline flicker on caster for Frenzy duration; inner red pulse every 1s | outline flicker only -- NOT a full-screen overlay; Frenzy is self-state |
| reads | Fury >= 80 at cast time -> lifesteal + CC immunity variant | hot-red body pulse on wind_up F1; CC immunity body flare on recovery F1 (bone-white ring expanding from torso) | body-local effects only |
| consumes | none | -- | -- |
| disambiguation_note | Frenzy self-buff vs any enemy state | Frenzy outline is on CASTER only, hot-red flicker; no pip on enemy; not to be confused with any enemy debuff state |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no telegraphed cast particle |
| impact_particle | yes | landing AoE: heavy red dust ring + ground crack lines radiating from landing point on active F4 |
| trail | yes | descent arc trail (active F2->F4), hot red smear, 2-frame; weapon forward so trail is weapon-tip led, NOT a spinning trail |
| screen_overlay | no | no overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` on AoE landing hit; no state pip on enemy |
| audio_anchor_frame | active F4 | slam landing is the audible beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | body-throw leap, hot red trail, landing AoE; Frenzy buff is self-state, fits OWNS |
| Avoids every AVOIDS item | PASS | no fancy movement language: no elegant arc, no aerial pose, no flourish; reads as thrown mass not acrobatic jump (Shadowblade territory avoided) |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | Fury 80%+ variant is gated on Fury resource (0-100), not on enemy HP; no HP color cue |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | landing uses dust + ground crack lines; no plate shard language |
| Silhouette distinct at 64px | PASS | airborne body-throw silhouette with weapon forward reads vs Ravager neutral; descent arc unmistakable |

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
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
