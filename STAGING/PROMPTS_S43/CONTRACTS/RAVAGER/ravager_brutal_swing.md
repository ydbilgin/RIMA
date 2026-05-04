# SKILL VISUAL CONTRACT -- RAVAGER / BRUTAL_SWING

---

## Section A -- Identity Metadata

| Field | Value | Notes |
|---|---|---|
| `class` | Ravager | |
| `skill_id` | `RAVAGER_BRUTAL_SWING` | |
| `display_name` | Brutal Swing | |
| `slot` | basic | LMB |
| `role` | opener / pressure | |
| `state_owner` | no | reads Fury meter (class resource) but does not apply a named state |
| `class_anchor_ref` | inherit (OWNS: HP trade, Fury escalation, frenzy chain, meat/bone VFX; AVOIDS: armor-crack/plate-shard VFX, HP-execute visual cue) | |

Tone note: 3-hit axe chain -- wide arc into overhead slam into ground pound. Each hit progressively heavier. Recovery on 3rd hit is deliberate commitment window. Body language is forward-leaning, overextended -- berserker, not skilled.

---

## Section B -- Animation States Required

| State | Required? | Frame count | Purpose | Notes |
|---|---|---|---|---|
| `wind_up` | yes | 1 | wide axe wind-back, weight shifted back -- readable at 8m | silhouette: weapon raised and angled back, body low |
| `active` | yes | 5 | 3-hit chain: arc F1-F2, overhead slam F3, ground pound F4-F5 | impact frames: F1 (arc contact), F3 (slam contact), F5 (ground pound AoE); audio anchor F5 |
| `recovery` | yes | 2 | post-ground-pound crouch, weapon tip touching ground, fully committed | punish window; Fury+30 bonus triggers here if 3rd hit landed |

Total frame budget: 8. Within basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| `reads` | Fury meter level (class state, owned) | body-heat tint on caster scales with Fury: pale at 0-33, warm red at 34-66, dark red at 67-100 | tint is ambient -- always present; no per-skill pip needed |
| `reads` | last 1s of taking damage after 3rd hit lands | Fury+20 on caster (passive gain, no visual beyond tint ramp) | tint escalation handles this visually |

State is agnostic. No new pip or overlay applied to enemy. Fury+30 on 3rd hit is resource gain only -- handled by HUD, not a world-space indicator.

---

## Section D -- VFX Requirements

| VFX layer | Required? | Spec |
|---|---|---|
| `cast_particle` | no | wind_up has no telegraphed cast particle -- weight shift and weapon angle are the tell |
| `impact_particle` | yes | F1 arc: horizontal blood-dust sweep, wide; F3 slam: downward bone-impact shockwave; F5 ground pound: radial dirt-blood AoE burst (largest of the three) |
| `trail` | yes | axe blade trail across all three hits; each trail arc distinct in shape (horizontal / diagonal / vertical) |
| `screen_overlay` | no | basic skill |
| `hit_reaction_on_enemy` | yes | `bleed` + `generic_stagger` reaction set; NO cracked/sundered/plate-shard language |
| `audio_anchor_frame` | active F5 | ground pound is the audible climax beat |

VFX layer count: 3 active (impact_particle, trail, hit_reaction). Within <= 4 gate.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | meat/bone/blood impact; Fury tint on caster; no elegant motion |
| Avoids every AVOIDS item | PASS | no armor-crack, no plate-shard VFX; no cracked-armor language anywhere |
| Counter-archetype distinct (n/a if not counter) | n/a | Rule #57 -- Ravager is not a counter class |
| No HP-execute visual cue | PASS | Fury+20 conditional reads time-gate (last 1s after hit), not enemy HP bar |
| No cracked-armor / Sundered VFX unless class == Warblade (Rule #55) | PASS | impact VFX is blood-dust and bone-shockwave; no armor-crack or shard-burst |
| Silhouette distinct from class neutral pose at 64px downscale | PASS | axe raised-back + low body stance vs neutral; ground pound (weapon tip down) is unique |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| `endpoint` | `create_spritesheet` |
| `canvas` | 128x128 |
| `ppu` | 64 |
| `rows` | 3 (wind_up, active, recovery) |
| `frames_per_row` | 1, 5, 2 |
| `palette_ref` | Ravager class palette (dark red / bone-white accent / blood-stain ground) |
| `reference_sprite` | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| `gen_budget_estimate` | 8 frames |
| `priority` | P0 -- basic LMB, blocks Ravager playable build |

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
