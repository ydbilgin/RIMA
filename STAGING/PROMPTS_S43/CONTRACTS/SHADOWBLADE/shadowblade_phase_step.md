# SKILL VISUAL CONTRACT -- Shadowblade: Phase Step

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_PHASE_STEP` |
| display_name | Phase Step |
| slot | basic |
| role | reposition |
| state_owner | yes (applies 0.3s invisibility on self after dash) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Short dash with 0.3s invisibility on exit. Body dissolves mid-dash and rematerializes at endpoint. The invisibility is brief -- a window, not sustained stealth. Reads as geometry phase, not speed-blur.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | slight lean in dash direction, body edge begins to dissolve | 1-frame dissolve start on F2 |
| active | yes | 3 | F1: body fully dissolved (ghost outline only, no solid fill), F2: mid-dash void (invisible to enemies -- show as near-transparent outline for player read), F3: rematerialization at endpoint | audio anchor F3; invis window = F2 |
| recovery | yes | 2 | stance settle post-rematerialization | |

Frame total: 7. Within Shadowblade basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | dash exit applies 0.3s invis to self | caster body becomes semi-transparent outline on F2; rematerializes solid on F3 | ghost-outline render: dark violet tint, alpha ~30%, no solid fill; must be distinguishable from full Smoke Veil stealth (which is deeper alpha fade) |
| reads | n/a | -- | -- |
| consumes | n/a | -- | -- |
| disambiguation_note | Phase Step 0.3s invis vs Smoke Veil AoE stealth | Phase Step: brief individual outline-fade, single caster only, 0.3s | Smoke Veil: deeper alpha fade, area effect, longer duration. Phase Step outline must be visually lighter/shorter to distinguish at a glance |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | edge-dissolution shimmer at wind_up F2 -- violet-dark particle fringe on body outline |
| impact_particle | no | no impact target |
| trail | yes | ghost-trail from dash origin to endpoint: faint violet afterimage line, fades over 2 frames |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | no | no target |
| audio_anchor_frame | active F3 | rematerialization pop is the beat -- soft, dimensional |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | phase-through geometry (body dissolve/rematerialize), not speed-blur dash |
| Avoids every AVOIDS item | PASS | NOT a teleport-slash -- no weapon swing during dash, no attack component |
| Counter-archetype distinct (Rule #57) | n/a | reposition, not counter |
| No HP-execute visual cue (Rule #56) | PASS | invis applies on all uses regardless of HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor language |
| Silhouette distinct at 64px | PASS | dissolved ghost-outline mid-frame reads distinct from any solid pose |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 7 frames |
| priority | P0 -- basic mobility skill, blocks Shadowblade playable build |

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
