# SKILL VISUAL CONTRACT -- Warblade: Gravity Cleave

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_GRAVITY_CLEAVE` |
| display_name | Gravity Cleave |
| slot | 4 (Core) |
| role | AoE pull + damage + slow |
| state_owner | no (applies slow to enemies; no persistent Warblade class state) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: weapon is raised overhead with both hands and slammed straight down into the ground. The impact creates a 4m shockwave that drags nearby enemies inward. No magic; the pull is pure physical force -- shockwave compression, not telekinesis. Slow is communicated by enemy stumble animation. After Iron Charge the pulled enemies are stun-locked for 1.5s and Rage surges +15. Heavyweight two-handed cleave energy.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | both hands grip weapon overhead, body rises on toes then drives weight down -- full preparation | 2 frames: raise then apex; weapon tip must clear character silhouette top edge |
| active | yes | 2 | slam into ground; impact frame then ground shockwave ring expanding outward | frame 1: contact; frame 2: shockwave visible as ground displacement ring |
| pull_settle | yes | 1 | brief hold at impact -- enemies visually dragged inward around character | 1 frame static holds the pull moment before recovery |
| recovery | yes | 2 | weapon wrenched up from ground, guard restored; debris settles | deliberate heavy recovery; 2 frames |

Frame total: 7. Within Warblade core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | slow 0.8s on pulled enemies | enemy stumble/drag animation toward character | no Warblade caster state change; pull is a hit effect on enemies |
| applies | Rage +15 (Iron Charge synergy: +15 per pulled enemy stun) | Rage bar surge on HUD | amber crack briefly brightens on caster at pull_settle frame |
| reads | Iron Charge stun active (synergy: pulled enemies stun 1.5s) | no visual change on caster | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | wind_up telegraphed by pose; no pre-cast particle |
| impact_particle | yes | ground crack ring from impact point; 4m radius indicator ring (1-2 px wide); debris chunks 2-4 px scatter outward then snap back inward (communicates pull direction) |
| trail | yes | weapon downswing arc on active F1; heavy directional smear top-to-bottom; dissolves in 1 frame |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `pull_stagger` -- enemies visibly slide toward impact point on pull_settle frame; no Sundered decal |
| audio_anchor_frame | active F1 | deep ground impact boom |

VFX layer count: 4 (impact_particle, trail, hit_reaction, ring indicator). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | two-handed ground slam, physical shockwave pull -- no magic, no projectile |
| Avoids every AVOIDS item | PASS | pull is physical compression, not Ravager corruption drag or Brawler launch |
| Counter-archetype distinct (Rule #57) | n/a | Gravity Cleave is an active AoE, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; all cues are flat-activation |
| No cross-class state confusion (Rule #58) | PASS | ground ring is physical/amber, distinct from Elementalist arcane circles or Shadowblade sigils |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | ground crack ring is a terrain effect, not an enemy armor decal; Sundered not applied here |
| Silhouette distinct from class neutral at 64px | PASS | overhead two-hand grip and downward slam posture clearly differ from neutral guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, pull_settle, recovery) |
| frames_per_row | 2, 2, 1, 2 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P1 -- core AoE skill, required for crowd-control chain |

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
