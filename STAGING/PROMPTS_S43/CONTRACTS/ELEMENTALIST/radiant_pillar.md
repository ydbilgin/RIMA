# SKILL VISUAL CONTRACT -- Elementalist: Radiant Pillar

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_RADIANT_PILLAR` |
| display_name | Radiant Pillar |
| slot | 10 (Advanced, Unity Light State pending) |
| role | sustained aura stance / elemental echo amplifier / Light recovery buff |
| state_owner | yes -- Light State (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: the Elementalist activates a 6-second radiant aura around self. Every Fire or Frost spell cast during this window creates a radiant echo (a secondary small radiant burst at the same location, no extra targeting required). Light skill recovery is reduced by 30% during the window. After Lightbreak cast, the duration extends to 10s. The visual identity is a persistent radiant column of light rising from the caster's feet -- narrow and tall, not spreading. Academic control. The activation is a downward-pointed index finger at arm's length, calling the pillar up from below.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| activate | yes | 3 | arm extends downward at angle, index finger points at ground -- pillar of radiant light rises from feet | frame 1: arm lowers; frame 2: finger points at ground; frame 3: pillar rising confirmed, caster stands in radiant column |
| active_loop | yes | 3 | caster stands in radiant pillar -- arms at sides, head slightly raised; light column pulses slowly | 3-frame loop; slow radiant pulse rising through the column |
| echo_pulse | yes | 1 | per Fire/Frost skill cast during window: brief radiant flash on caster as echo fires | 1 frame; plays each time echo is triggered; fast dissolve back to active_loop |
| deactivate | yes | 1 | pillar fades downward; caster returns to neutral | |

Frame total: 8. Within Elementalist advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Radiant Pillar active state (6s or 10s after Lightbreak) | caster enters radiant column aura | Unity overlay pending: Light State band UI for duration |
| applies | Light State +1 (Light element present in pillar) | HUD Light State counter increments | Unity overlay pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md |
| applies | radiant echo per Fire/Frost cast | echo_pulse frame fires per triggering cast | echo VFX plays at spell impact location in scene |
| reads | Lightbreak cast before (synergy: 10s duration) | no visual change beyond pillar persisting longer; duration in code | -- |
| consumes | none | -- | -- |

state_owner note: Unity Light State overlay pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | activate: radiant light pillar rising from feet; narrow vertical column 1-2 px wide at caster feet; gold-white; frames 2-3 of activate |
| impact_particle | yes | echo_pulse: radiant flash on caster body -- outward ring from center 2-3 px; fires per echo event |
| trail | no | stance skill, no movement |
| screen_overlay | no | advanced skill -- overlay forbidden |
| hit_reaction_on_enemy | no | Radiant Pillar affects caster aura; echo damage at impact location handled by scene VFX echo event |
| audio_anchor_frame | activate F3 | radiant hum establishing; echo_pulse: soft radiant chime per echo |

VFX layer count: 2 (cast_particle, impact_particle). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | radiant pillar aura, Light element, academic standing pose -- Elementalist |
| Avoids every AVOIDS item | PASS | radiant pillar is a spell aura, not a physical trap or placed device |
| Counter-archetype distinct (Rule #57) | n/a | Radiant Pillar is a sustain stance, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; duration extension requires Lightbreak (class-state conditional) |
| No cross-class state confusion (Rule #58) | PASS | gold-white vertical pillar distinct from Warblade amber seam burn, Brawler Charge glow |
| No Sundered/armor crack VFX (Rule #55) | PASS | radiant column has no armor-fissure element |
| Silhouette distinct from class neutral at 64px | PASS | downward pointing arm and standing-in-column pillar pose are distinct from all action cast gestures |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (activate, active_loop, echo_pulse, deactivate) |
| frames_per_row | 3, 3, 1, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, Light element solar-radiant: gold-white pillar) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 8 frames |
| priority | P2 -- advanced aura skill; secondary to core chain skills |

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
