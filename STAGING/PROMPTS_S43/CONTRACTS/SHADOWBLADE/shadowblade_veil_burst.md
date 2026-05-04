# SKILL VISUAL CONTRACT -- Shadowblade: Veil Burst

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_VEIL_BURST` |
| display_name | Veil Burst |
| slot | ultimate |
| role | opener / pressure (AoE multi-phase strikes) |
| state_owner | yes (applies Sever per hit; each strike hits a different radial point) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Shadowblade executes 4 phase-strikes radially around their current position. Each strike is a body-dissolve-emerge at a different radial point -- the caster appears to phase through space in 4 directions simultaneously. Must read as geometry-phase (body dissolve/emerge pattern), NOT as 4 separate blink-flashes or 4 separate teleport-slashes. The identity is the pattern, not the individual strikes.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | body begins to fragment: edges dissolve outward in 4 radial hints, brief gathering pose | must show 4-direction radial intent in wind_up silhouette |
| active | yes | 6 | F1: phase-out (body fully dissolves from center position), F2: strike N (appear at radial point 1, slash), F3: strike S, F4: strike E, F5: strike W, F6: return phase-in at origin | all 4 strikes are body-emerge-slash-dissolve; NO blink-flash between points; must read as one continuous phase pattern |
| recovery | yes | 3 | body fully rematerializes, stance lowers, breath -- 3 frames: heavy recovery | |

Frame total: 12. Within ultimate quota.

Special note: F2-F5 each show caster body at a different radial position -- these must be clearly spatially distinct frames (N, S, E, W quadrant positions), not reused from same position. This is what distinguishes phase-pattern from generic AoE.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | each of 4 strikes applies Sever to caster and hit reaction to enemies | no persistent decal on enemies from Veil Burst itself | hit flash per strike on enemy contact frames (F2-F5) |
| reads | n/a | -- | -- |
| consumes | n/a | -- | -- |
| disambiguation_note | Veil Burst phase-pattern vs generic teleport-slash | Veil Burst: body dissolves BETWEEN each radial point; shape of AoE is the 4-point radial phase path, not 4 separate blink-flash arrivals | A blink-flash arrival shows: origin disappear -> destination appear -> slash. Veil Burst shows: dissolve in place -> traverse arc -> emerge -> slash -> dissolve again -> traverse -> emerge. The motion path must be visible as continuous arcs, not point-to-point jumps. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | radial dissolution shimmer at wind_up: 4 directional shadow-tendril arms extending outward, then pulling back for launch |
| impact_particle | yes | per strike (F2-F5): compact blade-spark at each radial contact, violet-black; same sparks x4, spatially offset |
| trail | yes | continuous arc trace connecting origin -> radial point 1 -> 2 -> 3 -> 4 -> origin; drawn as one looping violet path, visible briefly after F6 |
| screen_overlay | no | ultimate -- Q3 locked: identity through silhouette pattern, not screen FX |
| hit_reaction_on_enemy | yes | `generic_stagger` on each of 4 hits; no Scar applied |
| audio_anchor_frame | active F2 | first strike is the beat; subsequent strikes layer at F3, F4, F5 (rapid-fire) |

VFX layer count: 4 active (cast_particle, impact_particle, trail, hit_reaction). At gate limit.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | 4-point radial phase pattern, body-dissolve between points, arc trail connecting path |
| Avoids every AVOIDS item | PASS | NOT 4 separate teleport-slashes; dissolution arcs between points and continuous trail distinguish this; near-miss: individual strike frames (F2-F5) could read as blink-flashes if trail is absent -- trail is MANDATORY to carry the geometry-phase read |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | Veil Burst triggers on activation, not HP gate |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | violet-black sparks only; no armor-crack |
| Silhouette distinct at 64px | PASS | 4-direction radial dissolution at wind_up is unique; no other class shows multi-directional body fragment in wind_up |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 6, 3 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 12 frames |
| priority | P2 -- ultimate; blocks late-game build but not playable prototype |

Note for rima-codex: Active frames F2-F5 each represent caster at a distinct radial position (N/S/E/W). Prompt must specify different spatial offset per frame -- not a rotated copy of the same frame.

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
