# SKILL VISUAL CONTRACT -- Warblade: Iron Counter

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_IRON_COUNTER` |
| display_name | Iron Counter |
| slot | 8 (Core) |
| role | reactive counter / Rage builder / stun |
| state_owner | no (0.8s window applies; no persistent caster state beyond Rage gain) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: 0.8-second window opens -- character shifts weight into a ready brace. If struck within the window, they pivot and deliver a devastating counter-slam (%180 damage) that stuns for 0.5s and generates Rage +25. At Rage 80+ the counter adds a knockback shockwave and 0.5s stagger. The counter visual is impact-on-impact: the enemy's hit is absorbed by armor, then the Warblade erupts back. Heavy planted counter energy -- not a dodge, not a deflection, not a parry flip. Pure force returned.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| ready_window | yes | 2 | character drops weight slightly, weapon arm cocks back -- ready brace communicates the window | frame 1: weight drop; frame 2: fully braced; amber seam flicker signals window is open |
| absorb_flash | yes | 1 | frame played when incoming hit lands in window: armor flash, body absorbs impact | single frame; timing: when enemy hit arrives |
| counter_strike | yes | 3 | explosive pivot + weapon slam into enemy; body rotates then drives force outward | frame 1: pivot turn; frame 2: impact; frame 3: follow-through with stun visual on enemy |
| recovery | yes | 1 | weapon returns; Rage bar surge visible | |

Frame total: 7. Within Warblade core quota.

Rage 80+ variant: counter_strike F3 adds shockwave ring at impact point. Same animation row; shockwave is an additive VFX layer triggered by game logic at Rage threshold.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Rage +25 on successful counter | Rage bar surge | amber crack accent brightens on caster at counter_strike F2 |
| applies | 0.5s stun on target | enemy body lock | generic stun lock hold on counter target |
| applies (Rage 80+) | knockback shockwave + 0.5s stagger | shockwave ring at impact point | additive VFX layer triggered by code; no separate animation row |
| reads | incoming hit within 0.8s window | no visual change -- timing is the read | ready_window amber flicker signals open window to player |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | ready_window amber seam flicker on weapon arm and armor shoulder -- 2-3 px glow pulse; communicates active window to player |
| impact_particle | yes | counter_strike F2: explosive impact burst; amber-orange sparks 3-4 px scatter; heavier than normal hit sparks to communicate %180 damage weight |
| trail | yes | counter_strike F1-F2: pivot weapon swing smear; rotational arc blur; fast dissolve |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `stun_lock` on counter target; `shockwave_knockback` at Rage 80+ (additive, conditional VFX layer) |
| audio_anchor_frame | counter_strike F2 | metallic boom + stun crack |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | absorb-counter with armor-planted brace is Warblade OWNS (Rule #57); amber seam window signal is Warblade-exclusive |
| Avoids every AVOIDS item | PASS | no Brawler whiff/evade agility; no Ronin pre-draw stillness; Warblade counter is braced and explosive, not nimble |
| Counter-archetype distinct (Rule #57) | PASS | Warblade=absorb/break: planted armor brace absorbs, then erupts. Ronin=pre-draw timing: stillness before draw. Brawler=whiff/evade body: evasion-based. All three are visually and mechanically distinct. |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Rage 80+ variant is a resource-state gate, not an HP read |
| No cross-class state confusion (Rule #58) | PASS | amber window glow distinct from Ronin gold stillness, Brawler blue Charge flicker |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | impact sparks are generic physical; no armor-fissure decal on enemy from this skill |
| Silhouette distinct from class neutral at 64px | PASS | ready brace weight drop and pivot counter clearly differ from neutral guard and from Ironclad Momentum stance |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (ready_window, absorb_flash, counter_strike, recovery) |
| frames_per_row | 2, 1, 3, 1 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P0 -- counter identity skill; required for Rule #57 compliance review and class distinction audit |

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
