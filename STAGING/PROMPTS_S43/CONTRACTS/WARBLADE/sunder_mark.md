# SKILL VISUAL CONTRACT -- Warblade: Sunder Mark

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_SUNDER_MARK` |
| display_name | Sunder Mark |
| slot | 5 (Core, Unity state pending) |
| role | armor debuff mark applier / damage amplifier setup |
| state_owner | yes -- Sundered (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: a targeted weapon strike specifically aimed at the enemy's armor plating. The strike is precise, not explosive. The result is a visible armor crack decal on the target -- this is the Sundered state, Warblade-exclusive (Rule #55). The crack glows faintly with amber to indicate ongoing armor weakness (-40% for 8s). Death Blow active deepens to -60%. Visual identity: the Sundered fissure is the core Warblade signature; it must read clearly as a crack in physical armor, not a magic curse or a Brawler shatter.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 1 | weapon tip angled at target -- aimed strike, controlled; no overhead lift | 1 frame: targeted stance; lean forward slightly; precision not power |
| active | yes | 2 | sharp forward thrust/slash at target armor location; precise impact contact | frame 1: strike contact; frame 2: crack propagates on target armor -- hold frame shows Sundered decal appearing |
| recovery | yes | 1 | weapon returns; character scans target (small head tilt) | communicates readout of debuff success |

Frame total: 4. Within Warblade core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Sundered state on enemy: armor -%40 (8s) | enemy receives Sundered decal -- armor crack fissure with faint amber glow | decal is Unity overlay (pending); sprite contract records amber fissure crack on active F2 |
| reads | Death Blow active (synergy: armor -%60 instead of -%40) | no caster visual change; deeper crack depth on Sundered decal (Unity overlay variant) | -- |
| consumes | none | -- | -- |

Sundered state note: this is Rule #55 territory. The armor crack decal belongs ONLY to Warblade. Unity overlay implementation pending. Sprite records the crack-on-impact intent; Unity team provides the persistent overlay layer for the 8s duration.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | precise aimed strike needs no cast telegraph beyond stance |
| impact_particle | yes | small sharp spark cluster at contact point; amber-orange; 1-2 px chips; communicates metal-on-metal precision strike |
| trail | no | this is a precise thrust, not a sweep; no trail |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `armor_crack_apply` -- enemy flinch at contact + Sundered decal appears (Unity overlay); crack fissure must be visually distinct from generic stagger |
| audio_anchor_frame | active F1 | sharp metallic crack -- higher pitch than heavy slam skills |

VFX layer count: 2 (impact_particle, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | armor crack is Warblade OWNS territory; precise weapon strike, no magic |
| Avoids every AVOIDS item | PASS | Ravager has no armor crack; Brawler has Cracked/Shattered (bone/flesh break), not armor fissure; these are distinct |
| Counter-archetype distinct (Rule #57) | n/a | Sunder Mark is an active debuffer, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; debuff depth (-%40 vs -%60) is a class-state conditional (Death Blow), not an HP read |
| No cross-class state confusion (Rule #58) | PASS | amber armor fissure decal is exclusive to Warblade Sundered; no other class produces this visual |
| No cracked-armor / Sundered VFX stolen (Rule #55) | PASS | this contract IS the Sundered origin skill; it correctly owns this VFX |
| Silhouette distinct from class neutral at 64px | PASS | forward aimed stance with weapon tip extended clearly differs from neutral guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 1, 2, 1 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 4 frames |
| priority | P0 -- Sundered state origin skill; required before Death Blow and Iron Crush synergy chains |

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
