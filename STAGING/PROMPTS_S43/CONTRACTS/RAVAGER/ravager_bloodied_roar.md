# SKILL VISUAL CONTRACT -- Ravager: Bloodied Roar

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_BLOODIED_ROAR` |
| display_name | Bloodied Roar |
| slot | core |
| role | control / pressure |
| state_owner | no (stagger is a generic reaction, not a class-owned state pip; buff scales on caster's missing HP) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a blood-choked war cry from a battered fighter. The sound and force stagger everything nearby. The lower the Ravager's HP, the more devastating the buff it provides to the follow-up. NOT a clean shout or rallying cry -- raw, desperate, blood-in-the-throat.

HP-scaling buff note: the roar grants a damage buff to the caster that scales with missing HP. This is a mechanical buff property -- the visual intensity of the body aura can reflect it modestly (brighter/larger at low HP) but must NOT be a clean HP-execute cue (Rule #56). Aura intensity change is subtle; it does not explicitly signal "target is near death."

Chain unlock: if a staggered enemy is then hit by Bloodlust Strike, +100% damage bonus. This is a mechanical pairing; no visual on THIS skill's contract beyond the stagger state (which Bloodlust Strike will read separately in its own execution).

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | head tilts back, chest opens -- drawing breath for the roar; weapon arm dropped | body opens up, vulnerable mid-wind_up |
| active | yes | 3 | F1 roar burst: mouth open, chest expanded, AoE ring emanates; F2 peak ring radius; F3 ring fades | AoE is 3m radius centered on caster; stagger applies on F1-F2 |
| recovery | yes | 2 | head drops, body sags slightly -- effort shown; damage buff aura lingers on caster | aura persists after recovery as a status effect overlay |
| cancel | no | -- | not cancellable | |

Frame total: 7. Within Ravager core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | 3m AoE 3s stagger on all enemies in radius | stagger reaction on each enemy hit; no state pip placed on enemy (generic_stagger, not a class-owned state) | enemies stagger-flinch in place; no overlay or pip; standard stagger reaction |
| applies (self) | damage buff from roar scales with Ravager missing HP | hot-red body aura on caster during buff duration; aura is slightly more intense at lower HP (subtle, NOT a high-contrast HP-execute indicator) | body aura stays local to caster; fades at buff end; max intensity is a warm glow, not a blinding flare |
| reads | none (skill does not consume or read pre-existing states for its own activation) | -- | -- |
| consumes | none | -- | -- |
| disambiguation_note | stagger vs any class-owned state | stagger here is generic_stagger reaction only; no Brawler Cracked pip, no Ravager-specific pip; stagger just plays the enemy flinch animation and 3s movement interrupt |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | blood-mist breath on wind_up F1->F2; small red particle exhale from mouth/chest area |
| impact_particle | yes | AoE shockwave ring on active F1: hot red with bone-white edge; 3m radius ring expanding outward; 2 frame expansion |
| trail | no | no trail for a roar skill |
| screen_overlay | no | no screen overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` on all enemies in AoE radius |
| audio_anchor_frame | active F1 | roar burst is the audible beat; heavy bass resonance |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | AoE roar, HP-scaling self-buff, hot red palette; fits low-life danger identity |
| Avoids every AVOIDS item | PASS | no armor-break language; no precise movement; raw battered power |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | damage buff aura intensity on caster subtly scales with OWN missing HP (not enemy HP); aura is a warm body glow -- does NOT use HP-bar color change, does NOT highlight enemies at low HP, does NOT create a execute-frame targeting cue; aura reads as "bloodied fighter buffed by pain," not "kill shot indicator" |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | AoE ring is hot-red + bone-white shockwave; no plate shards, no Sundered language |
| Silhouette distinct at 64px | PASS | open-chest roar pose with ring emanation reads vs Ravager neutral; AoE ring is spatially distinctive |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
| gen_budget_estimate | 7 frames |
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
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
