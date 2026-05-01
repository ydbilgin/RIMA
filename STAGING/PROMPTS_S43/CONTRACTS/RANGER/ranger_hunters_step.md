# SKILL VISUAL CONTRACT -- Ranger: Hunter's Step

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_HUNTERS_STEP` |
| display_name | Hunter's Step |
| slot | signature |
| role | reposition |
| state_owner | yes (applies next-attack-crit buff to self on tap branch) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: this is a tactical repositioning tool, not an aggressive charge. Tap branch is a quick disciplined side-step (maintains distance). Hold branch enters a brief void phase -- a deliberate stealth-step, NOT a Shadowblade-style teleport. The distinction is critical: Ranger's void step is 4m, grounded, directional -- it reads as a rapid ghost-step along the ground plane, not a disappear-reappear warp.

---

## Section B -- Animation States Required

Two input branches. Both share wind_up. Active splits into two sub-states.

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | coiled weight shift, rear foot lifts slightly -- shared by both branches; hold branch adds a 0.1s held-coil frame extension (1 extra frame) | silhouette reads as a loaded step, not a bow draw |
| active_tap | yes | 3 | quick lateral dash step, trailing earth-green afterimage dissolve, bow raised for next shot (crit-ready tell) | body displacement over 3 frames; afterimage is 1-frame ghost behind departure point; bow raise on F3 signals crit buff active |
| active_hold | yes | 4 | void entry shimmer F1; full transparency (void phase) F2-F3; void exit re-materialize F4 | void phase: sprite desaturates to near-transparent with faint bone-white outline; 4m forward on ground plane; NOT a warp flash -- transition is a slow shimmer in + shimmer out |
| recovery | yes | 2 | landing settle, weapon resettled -- shared recovery for both branches | |

Frame total: tap path = 2+3+2 = 7. Hold path = 3+4+2 = 9. Flag: hold path is 9, within signature quota.

Branch note: wind_up F1-F2 is shared. Hold branch adds one held-coil frame (F2 extended) before diverging into active_hold. Sprite sheet rows: wind_up (2), active_tap (3), active_hold (4), recovery (2) = 4 rows.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | tap branch applies next-attack-crit buff to self | on active_tap F3 (bow-raise frame), caster bow limb gets a single-frame bone-white pulse indicating crit buff is loaded | small bone-white glow on bow limb only; NOT a full-body aura; persists as a dim bow-limb highlight until next attack fires |
| reads | hold branch requires 0.3s hold input | wind_up holds on F2 for the extra coil frame when hold input is detected; no additional visual element needed beyond the held pose | |
| consumes | crit buff consumed on next attack (handled by attack animation, not this skill) | -- | -- |
| disambiguation_note | active_hold void phase must NOT resemble Shadowblade teleport. Shadowblade teleport: full instantaneous warp, dark smoke portal, no ground contact during phase. Ranger void step: gradual shimmer transparency along ground plane, bone-white outline retained, 4m linear movement visible, no portal or smoke. The ground shadow of the Ranger must remain visible during void phase to anchor the read. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no pre-cast particle; the coil pose is the tell |
| impact_particle | yes | tap: brief earth-green afterimage burst at departure point (1 frame); hold: bone-white shimmer dissolve at entry + bone-white shimmer reassemble at exit |
| trail | yes | tap: faint earth-green motion smear across dash arc; hold: NO trail during void phase (transparency is the visual language) |
| screen_overlay | no | signature reposition -- no screen overlay |
| hit_reaction_on_enemy | n/a | repositioning skill; no enemy hit |
| audio_anchor_frame | tap active F1 (step), hold active F1 (shimmer entry) and F4 (exit materialize) |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | tactical step, kill-zone repositioning; earth-green + bone-white palette throughout |
| Avoids every AVOIDS item | PASS | tap is not run-and-gun (it ends planted); hold void step explicitly distinguished from Shadowblade generic teleport by ground-plane shimmer vs portal warp |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | crit buff is unconditional on tap input, not tied to enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor language anywhere |
| Silhouette distinct at 64px | PASS | weight-shift coil is distinct from neutral; void phase silhouette is near-transparent with outline -- unique in roster |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active_tap, active_hold, recovery) |
| frames_per_row | 2, 3, 4, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 11 frames |
| priority | P1 |

Generation note: active_hold frames F2-F3 require sprite transparency layering -- confirm PixelLab alpha channel support before dispatch.

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
