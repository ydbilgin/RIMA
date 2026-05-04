# SKILL VISUAL CONTRACT -- Warblade: Deep Wound

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_DEEP_WOUND` |
| display_name | Deep Wound |
| slot | 11 (Advanced) |
| role | DoT applier + Rage builder |
| state_owner | no (applies bleed DoT to enemy; no persistent Warblade caster state) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: a deliberate raking slash or gouge aimed at leaving a lasting wound -- the physical kind, not a curse. Bleed DoT runs 8s and grants Rage +35, making this a resource-building advanced skill. During Iron Crush window the bleed tick doubles -- communicated by a brighter/faster amber pulse on the enemy bleed indicator rather than a new animation. The motion is controlled and precise: a calculated slash, not a wild swing. Warblade keeps weight through every strike.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 1 | weapon draws back along hip -- low angle prep; no overhead lift | 1 frame; fast prep distinguishes this as a surgical strike, not a power blow |
| active | yes | 3 | raking weapon draw across target; angled slash leaving wound trail; weapon follows through | frame 1: entry contact; frame 2: drag across; frame 3: exit follow-through; weapon angle stays consistent |
| recovery | yes | 2 | weapon returns; Rage bar surges; character checks stance | Rage +35 surge visible on bar; 2 frames allows brief pause that sells the wound-application deliberateness |

Frame total: 6. Within Warblade advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | bleed DoT 8s on enemy | enemy receives bleed indicator (HUD + small red tick above head) | no armor-crack decal; bleed is a wound, not a Sundered armor break |
| applies | Rage +35 | Rage bar surge on HUD | amber crack accent brightens on caster at recovery F1 |
| reads | Iron Crush active (synergy: bleed tick 2x) | no caster visual change; bleed tick frequency doubled in code | -- |
| consumes | none | -- | -- |

Bleed note: bleed is a generic wound DoT. It is NOT the same as Sundered (armor crack). No amber fissure decal; bleed uses its own red tick VFX which is a HUD/status layer.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | low hip draw needs no pre-cast particle |
| impact_particle | yes | slash trail on active frames: thin red-tinged line following weapon path; 1-2 px width; fast fade -- communicates wound, not explosion |
| trail | yes | weapon drag smear along active F1-F3; directional horizontal smear; fast dissolve |
| screen_overlay | no | advanced skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `wound_flinch` -- target recoils from slash contact; bleed indicator appears overhead (HUD layer) |
| audio_anchor_frame | active F1 | sharp wet slash |

VFX layer count: 3 (impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | precision weapon slash, Rage build, physical bleed -- no magic |
| Avoids every AVOIDS item | PASS | bleed DoT is a physical wound, distinct from Ravager's toxic/corruption DoT; no Brawler break language |
| Counter-archetype distinct (Rule #57) | n/a | Deep Wound is an active attacker, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; all cues are flat-activation |
| No cross-class state confusion (Rule #58) | PASS | thin red wound trail is generic bleed -- distinct from Shadowblade poison trail or Ravager corruption splash |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | bleed is a wound DoT, not an armor-crack decal; no fissure applied |
| Silhouette distinct from class neutral at 64px | PASS | low hip draw and horizontal raking slash differ clearly from vertical power blows and neutral guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 1, 3, 2 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 6 frames |
| priority | P2 -- advanced DoT skill; secondary to P0/P1 combat chain skills |

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
