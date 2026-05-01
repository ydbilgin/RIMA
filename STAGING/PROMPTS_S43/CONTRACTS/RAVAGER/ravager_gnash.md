# SKILL VISUAL CONTRACT -- Ravager: Gnash

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_GNASH` |
| display_name | Gnash |
| slot | core |
| role | sustain / pressure |
| state_owner | no (applies no enemy state; self-heal is mechanical) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a rapid, savage multi-hit flurry -- short chops, not elegant combos. Each hit tears and each tear heals. Reads as barely controlled violence. At HP<20% + Fury 100% the hits jump from 5 to 8 -- animation stays the same structure but plays faster and with more flicker.

Chain unlock: HP<20% + Fury 100% active -> 8 hits instead of 5. This is a mechanical change; the active loop simply runs more cycles. No new animation states required -- loop frames accelerate.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | quick aggressive hunch -- shoulders forward, weapon raised slightly; short tell, this is a fast opener | body leans into the target; no elegance |
| active | yes | 5 | rapid 5-hit flurry: F1 hit 1, F2 hit 2, F3 hit 3, F4 hit 4, F5 hit 5; each frame is one hit keyframe | hits on F1-F5; each hit has a small self-heal flash (see Section C); audio tick per frame |
| recovery | yes | 2 | stumble back slightly, weapon low; short window | brief exhale feel |
| loop | yes | 3 | 3-frame accelerated loop for 8-hit variant at HP<20%+Fury 100% | reuses active F1-F3 at faster cadence |
| cancel | no | -- | not cancellable mid-flurry | |

Frame total: 12. Within Ravager core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | each hit triggers small self-heal on Ravager | per-hit: brief hot-red micro-pulse on Ravager torso (small, not blinding) -- indicates life drain on contact | micro-pulse is 1 frame per hit; fades instantly; NOT a sustain overlay |
| reads | HP<20% + Fury 100% -> 8-hit mode | at threshold: hot-red body outline flicker at active start (1 frame) + loop plays at 1.4x speed in keyframes | threshold read on caster; no enemy visual change |
| consumes | none | -- | -- |
| disambiguation_note | self-heal pulse vs enemy state | micro-pulse appears on CASTER torso only, per hit tick; not to be confused with any enemy pip; not a lifesteal channel overlay |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no cast particle |
| impact_particle | yes | small red fleck spray per hit (5 ticks, or 8 in enhanced mode); small and fast -- accumulates visually over the flurry |
| trail | yes | weapon smear per hit arc, active F1-F5, hot red, 1-frame per arc; short and ragged |
| screen_overlay | no | no overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` on each hit tick (light stagger; rapid hits keep enemy rattled) |
| audio_anchor_frame | active F1, F2, F3, F4, F5 | each hit is an audio tick; fast percussion pattern; 8-hit variant same but faster |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | rapid flurry with self-heal fits HP-trade sustain identity; hot red palette |
| Avoids every AVOIDS item | PASS | no armor break; no elegant combo footwork; pure savage rapid hits |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | 8-hit threshold gates on HP<20% AND Fury 100% (dual resource check); visual flicker is on caster, gated on Fury resource display; no enemy HP color cue |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | impact uses red fleck spray; no plate shards or Sundered language |
| Silhouette distinct at 64px | PASS | forward-hunched rapid-flurry silhouette reads vs Ravager neutral; each hit frame is a distinct keyframe pose |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, recovery, loop) |
| frames_per_row | 2, 5, 2, 3 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| gen_budget_estimate | 12 frames |
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
