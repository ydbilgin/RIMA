# SKILL VISUAL CONTRACT -- Warblade: Iron Crush

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_IRON_CRUSH` |
| display_name | Iron Crush |
| slot | 3 (Core, Unity state pending) |
| role | damage amplification stance / Sunder Mark multiplier |
| state_owner | yes -- Iron Crush (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: a 6-second power stance. Character anchors weight, weapon lowered and forward -- projecting threat, not swinging. The damage bonus is communicated by a persistent amber-orange glow along weapon edge and shoulder armor. When Sunder Mark is active on a target, this state's multiplier kicks in -- shown by intensified glow on the weapon, not by a new animation. Heavy stillness; controlled power.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| activate | yes | 3 | character shifts into low stance, weapon angles forward, armor edge glow appears | frame 1: shift; frame 2: settle; frame 3: glow established on weapon edge |
| active_loop | yes | 2 | subtle breathing hold -- weapon forward, body planted; glow pulses slowly | 2-frame loop; minimal movement; communicates duration without idle restlessness |
| deactivate | yes | 1 | quick return to guard stance; glow fades | 1 frame is sufficient -- skill expires, not manually cancelled |

Frame total: 6. Within Warblade core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Iron Crush active state (6s, all damage +%30) | caster enters state overlay | amber-orange weapon edge glow persists for duration (Unity overlay pending) |
| reads | Sunder Mark on target (synergy: damage multiplied) | weapon glow intensifies momentarily when hitting a Sundered target | brief bright pulse on weapon -- not a new animation row, handled by Unity shader |
| consumes | none | -- | -- |

state_owner note: Unity overlay implementation pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md. Sprite contract records glow intent; Unity team implements the persistent edge-light layer.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | on activate: small amber crack lines radiate from boots as character plants -- 2-3 px, fast dissolve; communicates physical anchor |
| impact_particle | no | this is a stance, not a direct hit skill |
| trail | no | no movement; no trail |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | no | Iron Crush does not itself strike; damage bonus applied by other skills during window |
| audio_anchor_frame | activate F1 | armor settle clank + low hum of focused power |

VFX layer count: 1 (cast_particle). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | weapon-forward stance, amber Rage glow, physical plant -- no magic shapes |
| Avoids every AVOIDS item | PASS | glow is Rage-accent, not Ravager aura or Brawler charge; no armor-breaking language |
| Counter-archetype distinct (Rule #57) | n/a | Iron Crush is a stance, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; damage bonus is flat percentage, glow does not change with target HP |
| No cross-class state confusion (Rule #58) | PASS | amber weapon glow is distinct from Ronin draw-still, Brawler Charge glow, Ravager corruption |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | this skill does not apply or display Sundered; crack lines are at boots (physical anchor), not enemy armor |
| Silhouette distinct from class neutral at 64px | PASS | low forward stance with weapon angled out clearly differs from upright guard neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (activate, active_loop, deactivate) |
| frames_per_row | 3, 2, 1 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 6 frames |
| priority | P1 -- core stance skill; required before Sunder Mark synergy testing |

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
