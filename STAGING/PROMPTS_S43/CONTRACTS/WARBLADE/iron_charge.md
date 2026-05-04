# SKILL VISUAL CONTRACT -- Warblade: Iron Charge

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_IRON_CHARGE` |
| display_name | Iron Charge |
| slot | 1 (Core, starred) |
| role | gap-closer / stun initiator / Rage builder |
| state_owner | no (applies stun to enemy; does not apply a persistent Warblade class state) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: heavy armored shoulder charge that transitions into Blade Rush on hold. Ground tremor on impact. Stun is communicated by enemy body lock, not by magic ring. Hold variant extends into a 6m blade slash line through all enemies in path. Rage bar surges visibly on impact. Diablo 2 Barbarian energy -- momentum, mass, brutality.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | weight shift back, weapon arm cocks, boot plants -- telegraphs direction of charge | silhouette must clearly lean back before burst; 2 frames sells mass |
| dash_travel | yes | 3 | low-center-of-gravity sprint toward target -- shoulder leads, weapon trails | no floaty frames; feet cycle matches heavy armor weight |
| impact | yes | 2 | shoulder collision with target, shockwave on ground at contact point | impact frame 1: full collision; frame 2: dust/debris settle; enemy stun lock shown by hold pose |
| recovery | yes | 1 | character straightens, weapon returns to guard | fast recovery communicates chain potential |
| blade_rush_variant | yes | 3 | hold-mode: standing slash across a 6m line -- weapon horizontal sweep, all enemies in path | visually distinct from dash_travel; wider stance, longer weapon arc |

Frame total: 11 (standard + variant). Within Warblade core quota.

Blade Rush note: blade_rush_variant fires only when hold >= 0.5s. Sheet should encode both sequences. Game controller selects branch at runtime.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Rage +20 (standard) / Rage +15 per target (Blade Rush) | Rage bar surge animation | caster Rage crack accent brightens on impact frame |
| reads | stun on target (synergy: +%80 damage if target already stunned) | no visual change on caster -- damage number only | -- |
| consumes | none | -- | -- |

Synergy (3+ targets in Blade Rush): Rage +50 -- bar fill spike visible on HUD; no per-target decal needed on skill itself.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | amber-orange Rage crack along boot/shin as charge launches; 2-3 px width, fast fade |
| impact_particle | yes | ground shockwave ring at contact point (radius ~3 px at 64px view); debris chunks 2-4 px; dust cloud dissolves 3 frames |
| trail | yes | motion blur smear on dash_travel frames only; directional, not magical -- use dirty smudge, not glow |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `stun_lock` -- enemy body held rigid for 1.5s; no Sundered decal (Sunder Mark is a separate skill) |
| audio_anchor_frame | impact F1 | heavy thud + armor clank |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | physical charge, weapon sweep, ground shockwave -- no magic, no range |
| Avoids every AVOIDS item | PASS | no Ravager/Brawler armor language; stun is enemy lock, not cracked-armor decal |
| Counter-archetype distinct (Rule #57) | n/a | Iron Charge is an initiator, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; damage bonus on stun target uses combat log, not sprite cue |
| No cross-class state confusion (Rule #58) | PASS | stun visual is generic enemy lock; Rage crack accent is Warblade-exclusive color (amber-orange) |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | impact_particle is ground debris only; no fissure decal on enemy sprite |
| Silhouette distinct from class neutral at 64px | PASS | charge lean and ground shockwave clearly differ from standing guard neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, dash_travel, impact, recovery, blade_rush_variant) |
| frames_per_row | 2, 3, 2, 1, 3 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 11 frames |
| priority | P0 -- starred core skill, blocks Warblade playable build |

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
