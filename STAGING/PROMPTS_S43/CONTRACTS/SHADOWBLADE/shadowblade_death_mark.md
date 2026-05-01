# SKILL VISUAL CONTRACT -- Shadowblade: Death Mark

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_DEATH_MARK` |
| display_name | Death Mark |
| slot | signature |
| role | pressure / closer (delayed detonation) |
| state_owner | yes (applies Death Mark state to enemy; detonates after 4s) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Applies a 4-second countdown mark to an enemy. After 4s the mark detonates automatically. Shadowblade does not need to trigger it -- the threat is the timer. Mark visual must communicate countdown urgency without requiring screen overlay.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | blade tip angled at target, body still -- precision tell; minimal motion | |
| active | yes | 3 | F1: thin dark-violet ray projected from blade tip to target, F2: mark seals on target (placement frame), F3: caster withdraws blade to guard | audio anchor F2; no physical contact -- mark is projected, not melee |
| recovery | yes | 2 | guard resume | |

Mark countdown state (on TARGET, not caster):

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| mark_idle | yes | 3 | mark pulses slowly on target body: F1 dim, F2 mid-bright, F3 bright-peak, loops | countdown loop; pulse rate increases as 4s approaches (animation speed, not new frames) |
| mark_detonate | yes | 3 | F1: mark flares white-violet, F2: implosion burst (inward, not outward), F3: residual void-dark smoke | audio anchor F1 of detonate |

Frame total: 7 (caster) + 6 (mark sub-asset). Caster within quota; mark is a sub-asset.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | places Death Mark on target | persistent pulsing mark on enemy -- dark violet core, white-violet edge pulse, countdown communicated by pulse frequency | mark sits on enemy torso, world-space anchored; similar anchor to Scar but visually distinct: mark is a radiant pulse, Scar is a static gash |
| reads | n/a | -- | -- |
| consumes | auto-detonates after 4s (or earlier trigger if applicable) | mark_detonate sub-asset plays: inward implosion, void-dark smoke residual | detonation is inward implosion, NOT outward explosion; consistent with Scar Collapse visual language |
| disambiguation_note | Death Mark vs Scar vs Ranger Mark | Death Mark: pulsing radiant dark-violet body mark, world-space, countdown pulse frequency | Scar: static diagonal gash decal, no pulse; Ranger Mark: circular reticle, NOT body-anchored. Death Mark pulse must not be confused with Hexer pip (floating orb stack -- different object position entirely) |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | thin violet ray from blade tip to target on active F1 -- sharp geometric line, brief |
| impact_particle | yes | on detonate F1-F2: inward implosion burst, violet-white sparks collapsing toward body center |
| trail | no | projected mark -- no physical weapon trail during cast |
| screen_overlay | no | signature -- countdown urgency communicated through mark pulse frequency on target, not screen FX |
| hit_reaction_on_enemy | yes | on detonation: `marked` reaction extended with 1-frame void-dark full-body flicker; no knockback |
| audio_anchor_frame | active F2 | mark seal is the beat; detonate has its own audio anchor at detonate F1 |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | projected mark placement, countdown decal, inward implosion detonation |
| Avoids every AVOIDS item | PASS | cast is a precision ray projection, not a teleport-slash; caster does not move during cast |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | detonation is time-gated (4s), not HP-threshold triggered; mark appears at any HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | implosion uses void-dark and violet-white only; no armor-crack or shard-burst language |
| Silhouette distinct at 64px | PASS | stillness + blade-tip extension distinguishes cast pose from all active Shadowblade states |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, active, recovery, mark_idle, mark_detonate) |
| frames_per_row | 2, 3, 2, 3, 3 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 13 frames |
| priority | P1 -- signature; countdown detonation requires timer system functional |

Note for rima-codex: mark_idle and mark_detonate are target-side sub-assets. Generate as separate mini-sheet (2 rows) for independent instantiation on any enemy. Do NOT bake into caster sheet.

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
