# SKILL VISUAL CONTRACT -- Shadowblade: Shadow Clone

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_SHADOW_CLONE` |
| display_name | Shadow Clone |
| slot | signature |
| role | control / pressure (misdirection) |
| state_owner | no (clone is visual misdirection; applies no Scar, no mark) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Spawns a decoy phantom at a target point. Clone mirrors caster idle animation, draws enemy attention. Clone has no combat state: it does not place Scars, does not share Ranger Mark visual language, and does not attack. Pure misdirection tool. Clone dissipates after duration or on taking a hit.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | caster extends hand, shadow-matter pools at target point | |
| active | yes | 3 | F1: clone coalesces (shadow to solid outline), F2: clone fully formed (idle pose), F3: caster returns to guard | clone asset is separate from caster sheet; caster active frames = cast motion only |
| recovery | yes | 2 | caster guard resumes | |
| loop | yes | 3 | clone idle loop (mirrors caster neutral -- slow breath, blade at low guard) | clone loops until dissipated; gen as separate mini-sheet |

Clone dissipation state (triggered on clone hit or duration end):

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| dissipate | yes | 2 | clone dissolves: solid -> ghost outline -> gone | audio anchor F1 of dissipate; soft dimensional exhale |

Frame total: 7 (caster) + 5 (clone loop + dissipate). Caster within signature quota; clone is a sub-asset.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | clone spawns | clone appears at target point; caster has no persistent state applied | no pip on caster; clone presence is the indicator |
| reads | n/a | -- | -- |
| consumes | n/a -- clone removed on hit or duration; no state consumed | -- | -- |
| disambiguation_note | Shadow Clone vs Ranger Mark vs Hexer pip | Clone is a full phantom entity; NOT a mark, pip, or decal | Ranger Mark: circular reticle on enemy; Hexer pip: floating orb counter. Clone must be read as a character unit, not a state indicator. Clone must NOT share circular reticle visual language with Ranger Mark. |

Clone does NOT place Scars. It does NOT trigger Scar-related mechanics. This must be visually unambiguous -- clone carries no gash decals.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | shadow-matter pool at cast destination during wind_up -- violet-dark smoke coil, ground-up formation |
| impact_particle | no | no impact on spawn; formation is particle-driven |
| trail | no | clone does not move during idle loop |
| screen_overlay | no | signature -- earns identity through phantom silhouette, not screen FX |
| hit_reaction_on_enemy | no | clone is caster-side asset |
| audio_anchor_frame | active F1 | clone coalescence is the beat -- hollow, dimensional |

VFX layer count: 2 active (cast_particle, audio). Well within gate.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | shadow-matter geometry (phase physics), phantom silhouette; no Scar or Mark language on clone |
| Avoids every AVOIDS item | PASS | clone does not attack; no teleport-slash element |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | clone spawn is unconditional |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | shadow-matter / violet-dark only |
| Silhouette distinct at 64px | PASS | clone at rest reads as second character presence; hand-extension cast pose distinct from caster neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, active, recovery, clone_loop, clone_dissipate) |
| frames_per_row | 2, 3, 2, 3, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 12 frames |
| priority | P1 -- signature; requires phantom entity rendering pipeline functional |

Note for rima-codex: Clone loop and dissipate rows are the clone sub-asset. Generate as a separate mini-sheet (2 rows: loop, dissipate) for independent instantiation. Do NOT bake into caster sheet.

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
