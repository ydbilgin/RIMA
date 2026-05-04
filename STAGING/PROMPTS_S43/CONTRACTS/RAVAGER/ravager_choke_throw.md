# SKILL VISUAL CONTRACT -- Ravager: Choke Throw

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_CHOKE_THROW` |
| display_name | Choke Throw |
| slot | signature |
| role | control / reposition |
| state_owner | no (stun on throw landing is contextual, not class-owned pip) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a lunge grab followed by a one-arm hold and a brutal throw arc. Grab range is close (<=3m). The 1.5s hold reads as dominance -- target is helpless. The throw is power, not finesse. Fury+30 on the throw. Animation is three-phase: lunge-grab -> hold (one arm) -> throw arc.

Impact frame (for generation purposes) is the throw LANDING, not the throw release. The landing moment is the audible and visual beat.

Chain unlock: if thrown enemy hits a third character, both the thrown enemy AND the third are stunned. The third-hit stun is a physics/collision outcome -- no separate animation state needed for this contract (handled in engine). The thrown enemy impact frame is the only frame this contract covers.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | lunging grab: F1 lunge start (short range, <=3m), F2 grab contact (one hand closes on target), F3 grip established -- target is lifted/held at arm's length | grab must read as one-arm dominant hold; not a wrestling clinch |
| active | yes | 3 | 1.5s hold phase (2 keyframes) + throw release: F1 hold start (arm extended, target dangling), F2 hold mid (slight shake/dominance), F3 throw release (arm flings forward + up) | hold duration is 1.5s in engine; keyframes just cover start, mid, release |
| recovery | yes | 3 | F1 is the throw ARC (target is in flight, caster follows through); F2 is the throw IMPACT (target hits ground/wall -- this is the impact frame); F3 caster recovery stance after throw | impact frame is F2; audio anchor F2 |
| cancel | no | -- | no cancel during hold (committed grab) | |

Frame total: 9. Within Ravager advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Fury+30 on throw | brief hot-red Fury flash on caster at recovery F3 (after throw released) -- indicates Fury gain | single-frame Fury tick flash at Fury bar area on caster; NOT a screen effect |
| applies (throw impact) | thrown enemy hits ground/wall -> enemy stunned | standard stagger/stun reaction on thrown enemy at impact; no pip placed | generic_stagger on landing impact |
| applies (third-hit) | thrown enemy collides with third -> both stunned | both characters play stagger reaction at collision point | engine-handled; no separate animation frame in this contract |
| reads | none for activation | -- | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | lunge is the physical tell |
| impact_particle | yes | throw landing (recovery F2): target impact creates red dust + ground crater-ring at landing point; reads as body hitting hard |
| trail | yes | throw arc trail on recovery F1: target body in flight gets a brief hot-red smear, 2-frame; tracks the throw arc path |
| screen_overlay | no | no screen overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` on throw impact (recovery F2); no state pip |
| audio_anchor_frame | recovery F2 | throw landing is the audible beat; heavy thud + impact crunch |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | grab + hold + throw is brute-force dominance; hot red palette; fits HP-trade primal identity |
| Avoids every AVOIDS item | PASS | no armor-break language; no fancy movement (the throw arc is a power throw, not an acrobatic technique; caster stays planted during hold) |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | Fury+30 is resource gain on throw, not HP-gated; no HP color cue on target |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | impact uses dust + crater ring; no plate shards, no Sundered language |
| Silhouette distinct at 64px | PASS | lunge-grab with held target at arm's length is unmistakably different from Ravager neutral; hold pose is a unique silhouette |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 3, 3 |
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
