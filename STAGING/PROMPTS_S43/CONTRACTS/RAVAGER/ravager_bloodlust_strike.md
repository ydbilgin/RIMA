# SKILL VISUAL CONTRACT -- Ravager: Bloodlust Strike

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_BLOODLUST_STRIKE` |
| display_name | Bloodlust Strike (star) |
| slot | signature |
| role | pressure / closer |
| state_owner | no (reads own HP level mechanically; applies no enemy state) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: brutal forward lunge into a wide cone swing. Rage-fueled, not precise. Body reads as someone throwing their whole mass into the strike. NOT a graceful sweep.

Chain unlock note: Fury 80%+ causes Slaughter skill slot to open instantly. No visual change to THIS skill's animation -- Slaughter opening is a separate UI/HUD event.

HP-scaling note: damage scales mechanically with missing HP (at 30% HP -> +120% damage). This is a NUMBER property only. Section E Rule #56 applies -- see compliance check.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | weapon arm hauled back, hunched low, wide stance -- reads aggression, not finesse | body leans INTO the swing direction; silhouette lopsided |
| active | yes | 4 | sweeping cone strike, F1 is initiation, F3 is peak arc, F4 is follow-through | cone arc is forward 120 degrees; impact anchor F3 |
| recovery | yes | 2 | overcommit lean, weapon low and to the side | punishable window |
| cancel | no | -- | not cancellable | |

Frame total: 9. Within Ravager signature quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none -- skill applies no state to enemy | -- | -- |
| reads | Fury >= 80 at cast time | brief hot-red body pulse on caster at wind_up F1 -- indicates frenzy threshold met; Slaughter unlock is HUD event, not animation change | single-frame red aura ring at feet, fades in 2 frames; NOT a screen overlay |
| consumes | none | -- | -- |

State disambiguation: Ravager has no class-owned enemy debuff state. The hot-red body pulse reads self-state (Fury threshold), NOT an enemy marker. No pip placed on enemy.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no telegraphed cast -- lunge reads immediately |
| impact_particle | yes | heavy red-tinted dust + flesh-fleck spray on active F3; cone-shaped spread matching swing arc |
| trail | yes | weapon blade smear across cone arc, active F1->F3, hot red tint, 2-frame motion blur |
| screen_overlay | no | signature skill but NO full-screen overlay -- HP pulse is body-local only |
| hit_reaction_on_enemy | yes | `generic_stagger` reaction set; no state pip placed on enemy |
| audio_anchor_frame | active F3 | peak arc impact is the audible beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | hot red palette, forward-mass cone swing, no armor break or fancy footwork |
| Avoids every AVOIDS item | PASS | no armor-break shard VFX (Warblade territory); no repositioning arc (Shadowblade territory) |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | HP-scaling is mechanical only; damage number reflects scaling but NO visual changes target based on their HP level; no HP-bar color shift; no low-HP glow on enemy; Fury threshold pulse is on CASTER only and reads Fury (0-100 resource), not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | impact uses flesh-fleck + dust; no plate shards, no Sundered language |
| Silhouette distinct at 64px | PASS | hauled-back wide-stance wind_up reads vs Ravager neutral; cone active arc is unmistakable |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 4, 2 |
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
