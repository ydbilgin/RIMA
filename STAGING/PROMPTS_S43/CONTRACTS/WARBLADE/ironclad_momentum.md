# SKILL VISUAL CONTRACT -- Warblade: Ironclad Momentum

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_IRONCLAD_MOMENTUM` |
| display_name | Ironclad Momentum |
| slot | 7 (Core, Unity state pending) |
| role | defensive damage-absorption stance / Rage conversion window |
| state_owner | yes -- Ironclad Momentum (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: 6-second stance where incoming damage is partially absorbed and converted to Rage. Every 10 damage absorbed = +10 Rage. The visual identity is a hardened defensive posture -- weapon held across body, weight low and forward, armor plating highlighted with amber seams as Rage charges. After Earthsplitter the defense bonus deepens to 50%. Absorb-counter energy: this is Warblade's ability to turn punishment into power. Rage accumulation is visible as increasingly bright amber seams along armor edges.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| activate | yes | 2 | character drops into guard stance -- weapon horizontal across body, knees bent, chin down | frame 1: drop into stance; frame 2: armor seams light amber as absorption window opens |
| active_loop | yes | 3 | held defensive posture; armor seam glow intensifies as damage absorbed accumulates | 3-frame loop: seam brightness varies across frames to show Rage charging; subtle body micro-sway |
| hit_absorb | yes | 1 | single-frame response to incoming hit during active window: body braces, armor flashes bright amber | played per incoming hit during stance; communicates each absorption event |
| deactivate | yes | 1 | stance releases; seam glow fades; return to guard | |

Frame total: 7. Within Warblade core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Ironclad Momentum active state (6s) | caster enters defensive posture with amber armor seams | Unity overlay pending: amber seam layer on caster armor for duration |
| applies | Rage +10 per 10 damage absorbed | Rage bar increments on HUD; no new per-absorption sprite | hit_absorb frame fires per hit event |
| reads | Earthsplitter used before (synergy: defense +50%) | no visual change on caster -- depth of absorption handled in code | -- |
| consumes | none | -- | -- |

state_owner note: Unity overlay implementation pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md. Sprite contract records amber seam glow intent; Unity team implements the persistent layer.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | on activate: brief amber flash across armor plates as absorption field activates; fast dissolve 2 frames |
| impact_particle | no | individual hit absorptions are handled by hit_absorb frame animation, not separate VFX layers |
| trail | no | stance skill, no movement |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | no | this skill protects caster; enemy hit reactions handled by whatever hits the Warblade |
| audio_anchor_frame | activate F1 | low metallic hum as absorption field engages |

VFX layer count: 1 (cast_particle). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | absorb-counter is Warblade OWNS territory (Rule #57); amber armor seam glow is Warblade-exclusive |
| Avoids every AVOIDS item | PASS | no Brawler whiff/evade body language; no Ronin pre-draw stillness; Warblade absorb is armor-planted, not nimble |
| Counter-archetype distinct (Rule #57) | PASS | Warblade absorb = planted armor stance with Rage conversion. Ronin = pre-draw timing window. Brawler = whiff/evade body. These are visually and mechanically distinct. |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Rage conversion is per-damage-amount, not per HP threshold |
| No cross-class state confusion (Rule #58) | PASS | amber armor seam glow is distinct from Ronin draw-still, Brawler Charge tier, Ravager mutation aura |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | amber glow is on caster armor (own character), not enemy armor crack; Sundered not applied here |
| Silhouette distinct from class neutral at 64px | PASS | low horizontal-weapon guard stance clearly differs from upright neutral guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (activate, active_loop, hit_absorb, deactivate) |
| frames_per_row | 2, 3, 1, 1 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P1 -- absorb-counter identity skill; required for Rule #57 compliance review |

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
