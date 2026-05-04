# SKILL VISUAL CONTRACT -- Shadowblade: Veil Strike

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_VEIL_STRIKE` |
| display_name | Veil Strike |
| slot | basic |
| role | opener / pressure |
| state_owner | yes (applies mark on hit; Sever+8 per hit) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Quick reverse-grip slash. Single hit places a mark. LMB x3 + Hold 0.3s triggers Twin Carve variant: 2-slash combo + phase-step backward. Sever resource fills +8 per connecting hit. Basic attack -- reads fast and precise, not flashy.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | reverse-grip blade drawn back, weight shifted -- short tell | |
| active | yes | 3 | F1: slash extension (reverse-grip path, upward diagonal), F2: contact frame with mark flash, F3: return arc | audio anchor F2; mark placement on F2 |
| recovery | yes | 2 | blade returns to reverse-grip guard, stance resets | |
| cancel | yes | 1 | momentum redirect -- allowed after F1 to chain next input | for LMB chaining |

Twin Carve variant (triggered on LMB x3 + Hold 0.3s):

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| loop | yes | 2 | hold-charge shimmer on blade (0.3s window), body lowers | only during hold window |
| active_twin | yes | 4 | F1-F2: two rapid slashes (reverse-grip, crossing arcs), F3: phase-step backward begins (body half-dissolves), F4: caster rematerializes behind previous position | phase-step is BACKWARD; audio anchor F2 for slashes, F3 for phase |
| recovery_twin | yes | 2 | rematerialized stance settle | |

Frame total: 8 base + 8 Twin Carve = 16. Within 18-frame hard cap. Flag: two-state budget, must track separately.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | hit places mark on enemy | small dark-violet diagonal slash-scar marker on enemy, distinct from Scar decal (lighter, thinner line) | thin violet slash line on hit; NOT a Scar (Scar is thicker black-violet gash placed by Scarbinding/Night Aperture) |
| reads | Sever resource state on caster | no direct visual read on Veil Strike itself; Sever fills per hit | |
| consumes | n/a | -- | -- |
| disambiguation_note | Veil Strike mark vs Scar decal | Veil Strike mark: thin light-violet slash line, smaller, NOT world-space anchored (simpler overlay) | Scar (Scarbinding/Night Aperture): thick diagonal black-violet gash, body-anchored world-space, persistent. Must NOT look identical -- scale and weight must differ clearly at 64px |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | basic attack -- no telegraph particle |
| impact_particle | yes | on F2: small blade-edge flash, thin violet spark at contact point |
| trail | yes | reverse-grip blade arc trace, F1->F2, single violet line; Twin Carve gets two crossing arc traces |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `marked` reaction set: brief body flash + mark overlay placement |
| audio_anchor_frame | active F2 | blade contact is the beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | reverse-grip slash, phase-step backward (geometry phase, not blink), mark placement |
| Avoids every AVOIDS item | PASS | Twin Carve phase-step is a short body-dissolve backward reposition, NOT a teleport-slash forward into target |
| Counter-archetype distinct (Rule #57) | n/a | opener, not counter |
| No HP-execute visual cue (Rule #56) | PASS | mark applies regardless of HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | mark is slash-line overlay, no armor-crack language |
| Silhouette distinct at 64px | PASS | reverse-grip hold + diagonal upward slash arc distinct from class neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 7 (wind_up, active, recovery, cancel, loop, active_twin, recovery_twin) |
| frames_per_row | 2, 3, 2, 1, 2, 4, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 16 frames |
| priority | P0 -- basic skill, blocks Shadowblade playable build |

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
