# SKILL VISUAL CONTRACT -- Ravager: Death Wish

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_DEATH_WISH` |
| display_name | Death Wish |
| slot | ultimate |
| role | sustain / pressure |
| state_owner | yes (self-state: 5s window where HP cannot drop below 1; fatal hits become 1HP + 4s damage immunity; Fury x3 multiplier) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: the Ravager throws all safety away. For 5s they cannot die from damage. Every fatal hit is absorbed and converts to HP=1. Fury triples. No healing during window. CD 45s. The player enters a state of pure berserker defiance -- this is the most dangerous and most desperate moment in the Ravager's kit.

Chain unlock: if Fury reaches 100% during the window, the [V] Burst (Berserk Mode) becomes immediately triggerable. This is a HUD event (Burst button lights up) -- no additional animation state in this contract.

Screen overlay note: Death Wish IS flagged for minimal screen_overlay (Section D). Body aura flare ONLY. Not a full-screen tint or flash. The overlay is a body-local aura that extends just past the sprite boundary -- a hot-red crackling outline, persistent for the 5s window. This is the minimum screen presence for a Master skill. Flagged here per brief.

Fatal hit absorption note: when a fatal hit is taken during the window, the Ravager's sprite flashes bone-white (1 frame) and the HP drops to 1 -- this is a REACTIVE visual on the caster, not a damage number on the enemy. Immediately followed by 4s damage immunity aura (bone-white body shield outline).

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | activation: weapon planted or raised to the side, chest open, head thrown back -- defiant stance; body reads as welcoming the pain | NOT a roar (that is Bloodied Roar); this is a silent defiant pose |
| active | yes | 2 | 5s stance: body in heightened aggressive guard, hot-red body aura crackling; F1 is entry frame, F2 is hold/loop frame | active is a held state for 5s; loop on F2 |
| recovery | yes | 2 | 5s window ends: brief sag, aura fades; if no fatal hit taken: normal exit; if fatal hit absorbed: bone-white immunity shell fades after 4s | both exit types share recovery base; immunity shell is a VFX layer, not a separate row |
| loop | yes | 2 | 5s active hold (loops F2 of active) | tight loop; aura flicker is VFX layer, not new keyframes |
| cancel | no | -- | not cancellable once activated | |
| fatal_flash | yes | 1 | 1-frame bone-white full-body flash when a fatal hit is absorbed | reactive; inserted by engine at hit event; provided as a single-frame sprite for that moment |

Frame total: 9 + 1 (fatal flash) = 10. Master skill; elevated quota justified.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies (self) | Death Wish active: 5s window, Fury x3, HP floor at 1 | persistent hot-red crackling aura on caster for 5s duration; aura is body-local outline extending ~4px past sprite boundary | hot-red crackling outline; NOT a full-screen tint; NOT a purple halo (Ravager palette rule) |
| applies (fatal hit absorbed) | fatal damage taken during window -> HP drops to 1 + 4s damage immunity | 1-frame bone-white body flash on caster at hit moment; immediately followed by bone-white shield outline overlay for 4s | bone-white flash: 1 frame; shield outline: persists 4s, then fades; hp counter visibly shows 1 |
| applies (4s immunity shell) | 4s damage immunity after fatal absorption | bone-white semi-translucent body shell outline; pulses slowly; fades at immunity end | shell outline is distinct from the hot-red Death Wish aura; layered on top |
| reads | Fury reaches 100% during window -> Burst triggerable | Burst HUD button flash (UI event); no additional caster animation | HUD event only; not in this contract's sprite scope |
| consumes | none | -- | -- |
| disambiguation_note | Death Wish aura vs Berserk Mode (V Burst) aura | Death Wish aura: hot-red crackling outline on body, 5s window. Berserk Mode (V Burst): blood ring AoE, separate activation. These must be visually layerable but distinguishable: Death Wish = tight body outline; Berserk = wider blood ring AoE expanding from body. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | activation: hot-red pulse burst from caster body outward, single expansion ring (1 frame + fade 2 frames); marks the moment Death Wish activates |
| impact_particle | yes | fatal hit absorption: bone-white particle burst at impact point (1 frame); small, on the caster's hit location |
| trail | no | no trail for a stance skill |
| screen_overlay | yes (MINIMAL -- FLAGGED) | body aura flare ONLY: hot-red crackling outline extending ~4px past sprite boundary for the 5s duration; this is the minimum screen presence for a Master skill; NOT a full-screen tint; NOT a full-screen flash; FLAGGED for design lead review at Section G |
| hit_reaction_on_enemy | no | Death Wish is entirely self-targeted |
| audio_anchor_frame | wind_up F1 (activation beat) | activation is the audible beat; deep resonant bass hit; NOT the roar (that is Bloodied Roar) |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | ultimate defiance stance, HP floor mechanic, Fury multiplier -- all fit HP-trade low-life identity; hot red + bone-white palette |
| Avoids every AVOIDS item | PASS | no armor-break language; no fancy movement; stance is planted and defiant |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | Death Wish reads own HP floor mechanically but does NOT visually cue targeting enemies at low HP; the aura is a self-state indicator only; no change in how enemies are visually highlighted |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | activation pulse and aura use hot-red and bone-white; no plate shards, no Sundered language |
| Silhouette distinct at 64px | PASS | defiant planted stance with hot-red crackling aura reads vs Ravager neutral and vs Bloodied Roar (which opens chest forward); Death Wish is a pulled-back defiant pose |
| Screen overlay flagged for review | FLAGGED | screen_overlay is YES (minimal body aura only) per brief instruction; flagged in Section G for design lead sign-off |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, recovery, loop) + 1 (fatal_flash, single frame) |
| frames_per_row | 2, 2, 2, 2, 1 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| gen_budget_estimate | 9 frames (+1 fatal flash) |
| priority | P1 |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Screen overlay (minimal body aura) approved for Master skill | design lead | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (paired with ravager_bone_crack.md -- Death Wish active modifies Bone Crack; both contracts in this batch) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
