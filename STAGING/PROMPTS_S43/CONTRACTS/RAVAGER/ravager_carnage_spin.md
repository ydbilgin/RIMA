# SKILL VISUAL CONTRACT -- Ravager: Carnage Spin

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_CARNAGE_SPIN` |
| display_name | Carnage Spin |
| slot | signature |
| role | pressure / control |
| state_owner | no (defense reduction is a mechanical debuff, not a class-owned pip state) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: BRUTE spin with heavy weapon and mass. Body reads as a large fighter using momentum and bulk -- shoulders heavy, feet planted-to-planted, not lifted. This is NOT elegant. NOT Bladestorm (Warblade). Silhouette must show axe + body bulk. Spinning is momentum-driven, not trained technique.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | plant feet, raise weapon to begin momentum -- squat and heavy; body low before spin | reads as bulldozer winding up, not a dancer |
| active | yes | 6 | 2s spin loop (3 full rotations worth in keyframes): F1 first arc, F2 peak F, F3 full extension, F4 mid-spin hit, F5 second arc, F6 final hit frame | each hit tick on F2 and F4 and F6; AoE extends to weapon reach |
| recovery | yes | 3 | dizzy stumble out of spin, weapon dragging -- recovery is slow and punishable | head shake on F2; weapon low and dragging, not raised |
| loop | yes | 2 | held spin cycle (2s channel keyframes only) | minimal; reuses active F2-F3 as loop |
| cancel | no | -- | not cancellable mid-spin | |

Frame total: 13. Within Ravager signature quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | each hit applies -5% defense to enemy (max -30% after 6 hits) | small bone-white crack marker on enemy torso per stack, max 6 pips; at max (-30%) pips pulse once | bone-white hairline crack pip -- small, NOT plate-shard (that is Warblade Sundered) |
| reads | none (skill does not read pre-existing states) | -- | -- |
| consumes | none | -- | -- |
| disambiguation_note | defense reduction pip vs Warblade Sundered vs Brawler Cracked | Ravager defense pips: bone-white hairline crack, small circular pip shape, 6-stack max; Brawler Cracked: organic body fissure, 3-stack; Warblade Sundered: plate shards + metallic burst. Ravager pips must NOT use shard language or full-body overlay |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | wind_up is a body tell, no particle cast |
| impact_particle | yes | red dust ring + bone-chip spray on each hit tick (F2, F4, F6); ring shape matches AoE radius |
| trail | yes | axe arc smear per rotation, hot red, 2-frame persistence; NOT a clean blade trail -- ragged edge |
| screen_overlay | no | no overlay for this skill |
| hit_reaction_on_enemy | yes | `generic_stagger` on each hit tick; at max defense stack pip pulses once (no separate reaction set) |
| audio_anchor_frame | active F2, F4, F6 | each hit tick is an audio beat; lower-frequency thud to distinguish from Bladestorm |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | bulk-mass spin, hot red palette, bone-chip particles |
| Avoids every AVOIDS item | PASS | no armor-break shard VFX (Warblade); spin silhouette explicitly brute (axe + body bulk), NOT elegant spin like Warblade Bladestorm -- heavy foot placement, dragging recovery, ragged arc trail |
| Silhouette note -- EXPLICIT CALL (per brief) | PASS | Carnage Spin silhouette is brute: axe + body bulk in frame, feet heavy, recovery stumble. NOT elegant spin like Warblade Bladestorm. Bladestorm uses clean blade geometry and lifted footwork. These must NOT be visually confused at 64px. |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | defense reduction scales with hit count, not HP; no HP color cue |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Ravager defense pips are bone-white hairline pip shapes, not plate shards; Sundered shard-burst language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | bulk axe-spin with stumble recovery reads unmistakably vs Ravager neutral and vs Bladestorm |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, recovery, loop) |
| frames_per_row | 2, 6, 3, 2 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| gen_budget_estimate | 13 frames |
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
