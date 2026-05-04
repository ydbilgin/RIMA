# SKILL VISUAL CONTRACT -- Warblade: Crippling Blow

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_CRIPPLING_BLOW` |
| display_name | Crippling Blow |
| slot | 2 (Core) |
| role | heavy damage + healing debuff applier |
| state_owner | no (applies healing reduction debuff to enemy; no persistent Warblade class state) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: a single massive weapon blow aimed at the enemy's limb joint or side -- not center mass. The intent is anatomical damage, not kill. The healing debuff reads as a cripple, not a curse. After Iron Charge combo, the blow lands on a staggered target and amplifies the debuff to full heal-block. Steel palette with muted amber accent on impact.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | weapon raised high overhead -- weight loaded into strike; body twists at waist | 2 frames: coil then apex; silhouette must show weapon fully raised above character center |
| active | yes | 2 | downward diagonal weapon slam into target's flank; impact frame then follow-through | frame 1: contact; frame 2: weapon buried/drag |
| recovery | yes | 2 | weapon wrenched back to guard; character shoulders settle | recovery is deliberate -- this is a big swing, not a jab |

Frame total: 6. Within Warblade core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | healing reduction -%50 (6s) on enemy | enemy receives debuff icon (HUD handled) | no per-sprite cue required; impact_particle color shift signals debuff |
| reads | Iron Charge stun active on target (synergy: healing -%100) | no caster visual change -- damage and debuff magnitude handled in code | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | wind_up telegraphed by pose alone; no cast particle needed |
| impact_particle | yes | blunt force impact flash at contact point; muted amber-orange spark cluster 2-3 px; no fissure decal |
| trail | yes | weapon swing arc smear on active frames; directional blur, 2-3 px width, dissolves in 1 frame |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `heavy_stagger` -- large body recoil on impact frame; no Sundered/armor-crack decal |
| audio_anchor_frame | active F1 | heavy metallic crunch |

VFX layer count: 3 (impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | melee weapon blow, physical debuff cue, no magic |
| Avoids every AVOIDS item | PASS | no armor-crack decal; debuff is wound-cripple, not Ravager poison or Brawler break |
| Counter-archetype distinct (Rule #57) | n/a | Crippling Blow is an active attack, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals anywhere in this contract |
| No cross-class state confusion (Rule #58) | PASS | impact_particle amber matches Warblade palette; no bleed/purple/green |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | impact_particle is spark cluster only; no armor fissure |
| Silhouette distinct from class neutral at 64px | PASS | raised weapon apex and diagonal slam clearly differ from neutral guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 2, 2 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 6 frames |
| priority | P1 -- core damage skill, required for Warblade combo chain |

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
