# SKILL VISUAL CONTRACT -- Ranger: Sweep Volley

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_SWEEP_VOLLEY` |
| display_name | Sweep Volley |
| slot | basic |
| role | pressure |
| state_owner | no |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: a deliberate left-to-right sweeping cone of arrows. The Ranger pivots from the hip, bow arm tracking across the target arc. This is NOT a rapid-fire spray (Gunslinger); it is a single sweeping draw-and-release that fans 3-5 arrows across the cone. Methodical, not frantic.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | bow raised to left side of cone -- body rotates toward leftmost sweep target; arm fully drawn | marks start position of the sweep arc; silhouette: bow arm pointed hard left |
| active | yes | 4 | F1: release left arrow; F2: pivot center, release center arrow; F3: pivot right, release right arrow; F4: sweep follow-through, bow arm completes right arc | arrows are released staggered across F1-F3; all three or five arrows in flight simultaneously visible in world by F3; audio anchor F1-F3 staggered |
| recovery | yes | 2 | bow arm lowers from right position, body returns to neutral forward facing | overextended right-pivot shows punish window |

Frame total: 8. Within Ranger basic quota.

Note: arrow projectiles are world-space VFX spawned at F1, F2, F3 respectively. The caster sheet does not need to show arrows in flight -- that is particle/projectile layer.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none | -- | -- |
| reads | none | -- | -- |
| consumes | none | -- | -- |
| disambiguation_note | n/a -- this skill applies no state; no state visual disambiguation required. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no pre-cast particle; wide bow stance is the tell |
| impact_particle | yes | per-arrow impact: small dust puff at each hit point, earth-green tint; impacts land at staggered times matching F1-F3 spacing |
| trail | yes | three arrow shaft trails, staggered left-to-right, earth-green tint; each trail persists 2 frames after arrow spawns; creates visible cone-fan pattern in world space |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `generic_stagger` on each arrow hit |
| audio_anchor_frame | active F1, F2, F3 -- three staggered bowstring release beats; final F3 is loudest (rightmost sweep completes arc) |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | bow sweep, distance discipline maintained, arrows not spray (pivot is deliberate and slow) |
| Avoids every AVOIDS item | PASS | sweep is methodical pivot, not run-and-gun movement; no teleport; not a Gunslinger rapid-fire pattern |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | unconditional cone, no HP gating |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor language; generic stagger only |
| Silhouette distinct at 64px | PASS | hard-left bow-arm start position + rightward pivot arc is unique vs neutral and vs Pinning Shot draw (which is centered) |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 4, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 8 frames |
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
