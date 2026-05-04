# SKILL VISUAL CONTRACT -- Elementalist: Element Charge

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Elementalist |
| skill_id | `ELEMENTALIST_ELEMENT_CHARGE` |
| display_name | Element Charge |
| slot | 11 (Advanced) |
| role | Fire spell haste buff / mana cost amplifier / Fire State optimizer |
| state_owner | no (no Unity state overlay required; buff is an internal cast-speed modifier) |
| class_anchor_ref | inherit (OWNS: spell shapes line/wall/orb/beam, element reactions, Lightbreak; AVOIDS: physical traps -- Ranger territory) |

Tone note: 8-second window where all Fire spells become instant-cast (mana cost x2 as trade-off). At Fire State 5 stacks, no mana increase applies. The visual is a focused fire energy rising in the caster -- not explosive, but concentrated. Both hands close into fists at the sides, and a rapid orange-red shimmer ripples through the caster's body from feet to head as the buff activates. Academic but intense -- this is deliberate power acceleration, not rage. During the window, each Fire spell cast produces a brief fire shimmer on the caster's hands (handled by active_loop).

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| activate | yes | 3 | both fists close at sides -- fire energy rises through body from feet to head in a shimmer | frame 1: hands close; frame 2: shimmer rises to waist; frame 3: shimmer reaches head; buff fully active |
| active_loop | yes | 2 | hands remain slightly closed, orange shimmer on knuckles; ready-cast stance | 2-frame loop; minimal movement; fire shimmer on hands communicates active window |
| fire_cast_flash | yes | 1 | per Fire spell cast during window: brief intensification of hand shimmer -- orange flash on both hands | 1 frame; plays each time a Fire spell fires; fast dissolve back to active_loop |
| deactivate | yes | 1 | fists open; shimmer fades; caster exhales | |

Frame total: 7. Within Elementalist advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Element Charge active (8s): all Fire spells instant-cast | caster enters fire-charged stance | orange-red shimmer on hands for duration |
| reads | Fire State 5 stacks (synergy: no mana cost increase) | no visual change on caster; mana cost handled in code | -- |
| consumes | none | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | activate: fire shimmer ripple rising through body; orange-red wave 1-2 px wide traveling upward over 3 frames |
| impact_particle | no | stance skill; no direct hit |
| trail | no | no movement |
| screen_overlay | no | advanced skill -- overlay forbidden |
| hit_reaction_on_enemy | no | this skill affects caster only |
| audio_anchor_frame | activate F3 | focused fire crackle + rapid shimmer hum |

VFX layer count: 1 (cast_particle). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | fire element shimmer, academic power-concentration stance -- Elementalist |
| Avoids every AVOIDS item | PASS | fire energy is internal/caster-only; no physical trap or weapon |
| Counter-archetype distinct (Rule #57) | n/a | Element Charge is a buff stance, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Fire State 5 synergy is a resource-stack gate, not HP |
| No cross-class state confusion (Rule #58) | PASS | orange-red fire shimmer on hands distinct from Warblade amber armor seams, Brawler Charge glow |
| No Sundered/armor crack VFX (Rule #55) | PASS | no armor fissure element |
| Silhouette distinct from class neutral at 64px | PASS | closed fists with visible shimmer rising through body is unique among Elementalist stances |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (activate, active_loop, fire_cast_flash, deactivate) |
| frames_per_row | 3, 2, 1, 1 |
| palette_ref | Elementalist class palette (muted blue-grey base, fire element: orange-red shimmer) |
| reference_sprite | `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 7 frames |
| priority | P2 -- advanced fire haste skill; secondary to core chain skills |

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
