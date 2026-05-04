# SKILL VISUAL CONTRACT -- Shadowblade: Smoke Veil

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_SMOKE_VEIL` |
| display_name | Smoke Veil |
| slot | signature |
| role | reposition / control (AoE stealth window) |
| state_owner | yes (applies stealth to self; AoE obscures in single-player context) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Releases a burst of shadow-smoke in an area around the caster, applying stealth to the caster. Single-player context: the AoE smoke is environmental visual cover. Caster fades into the smoke. Must read as deliberate concealment, not explosive blast.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | hand draws across body -- gathering shadow-smoke; body compresses inward, preparing release | |
| active | yes | 3 | F1: smoke burst releases outward from caster body (not an explosion -- a controlled unfurl), F2: caster begins fade into smoke (body alpha drops), F3: caster at full stealth alpha within smoke cloud | audio anchor F1; smoke unfurl is AoE; caster disappearance is F2-F3 |
| recovery | yes | 2 | smoke persists as environment; caster at stealth -- recovery is near-invisible (faint outline) | |
| loop | yes | 3 | stealth idle: caster as ghost outline within smoke, minimal movement | for stealth duration loop |

Stealth exit state:

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| exit_stealth | yes | 2 | smoke dissipates (edge-fade), caster rematerializes solid | audio anchor F1 |

Frame total: 10 (caster) + smoke AoE as environment particle (not a caster frame).

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | caster stealth applied on active F2 | caster body alpha drops to ghost outline (~20% solid), contained within smoke cloud | ghost outline darker and more faded than Phase Step 0.3s invis (Phase Step = ~30% alpha, brief; Smoke Veil = ~20% alpha, sustained loop) |
| reads | n/a | -- | -- |
| consumes | stealth ends on skill use or duration exit | exit_stealth plays: smoke edge-fade, caster rematerializes | |
| disambiguation_note | Smoke Veil stealth vs Phase Step 0.3s invis | Smoke Veil: deeper fade (~20% alpha), AoE smoke environment, sustained loop, deliberate concealment | Phase Step: lighter fade (~30% alpha), no AoE, single brief frame, mobility context. Must visually differ in alpha depth and environmental context. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | smoke unfurl burst on active F1: outward radial dark-violet smoke cloud, NOT an explosion ring -- controlled outward seep from caster body |
| impact_particle | no | no impact target |
| trail | no | smoke is environmental, not a trail |
| screen_overlay | no | signature -- concealment identity through smoke environment, not screen tint |
| hit_reaction_on_enemy | no | no target |
| audio_anchor_frame | active F1 | smoke release is the beat -- soft, muffled |

VFX layer count: 2 active (cast_particle, audio). Well within gate.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | shadow-smoke geometry, stealth fade, phase-consistent alpha treatment |
| Avoids every AVOIDS item | PASS | smoke unfurl is NOT a teleport-slash; caster stays in place during release |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | stealth applies unconditionally |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | smoke only; no armor language |
| Silhouette distinct at 64px | PASS | outward smoke unfurl + caster fade-in is unique; smoke cloud shape distinguishes from Phase Step |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, active, recovery, loop, exit_stealth) |
| frames_per_row | 2, 3, 2, 3, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 12 frames |
| priority | P1 -- signature; smoke AoE environment requires particle system functional |

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
