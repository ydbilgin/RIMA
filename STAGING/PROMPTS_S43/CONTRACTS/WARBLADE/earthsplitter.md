# SKILL VISUAL CONTRACT -- Warblade: Earthsplitter

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_EARTHSPLITTER` |
| display_name | Earthsplitter |
| slot | 6 (Core) |
| role | knockup + Rage builder / Broken stack applier (hold variant) |
| state_owner | no (applies Broken stacks to enemies; Broken is an enemy status, not a caster state requiring Unity overlay) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: a ground-slam that erupts upward -- the weapon drives into the floor and the reverb lifts enemies off their feet for 2 seconds. On hold (0.4s), the slam becomes the first of three sequential ground-crack waves rolling outward, each depositing a Broken stack on any enemy they pass through. Broken state = visible stagger, stumble, fractured posture on enemy. Not a magic wave -- the cracks are physical terrain fractures, amber-orange tinted. During Bladestorm, the knockup duration extends +1s.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | weapon overhead then drives straight down; same overhead prep as Gravity Cleave but purpose shifts to vertical eruption | differentiate from Gravity Cleave by more vertical alignment and foot-plant emphasis |
| impact | yes | 1 | weapon contacts ground; immediate ground eruption spike at contact | impact frame 1: ground fracture line upward; dust and debris go UP, not outward |
| knockup_hold | yes | 1 | character holds impact pose; enemies visible in air above | 1 frame static communicates 2s knockup duration |
| wave_variant_1 | yes | 2 | hold mode only: first wave crack rolling forward from impact point; ground tiles split | wave is a ground-level horizontal crack, not a projectile; 2 frames of crack advancing |
| wave_variant_2 | yes | 1 | second wave launches from crack endpoint; Broken stack applied to enemies in path | overlap with wave_variant_1 progression |
| wave_variant_3 | yes | 1 | third wave; final Broken stack application | |
| recovery | yes | 1 | weapon pulled from ground; guard restored | |

Frame total: 9 (standard: 5; full hold chain: 9). Within Warblade core quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Rage +25 on knockup | Rage bar surge | amber crack accent brightens on caster at impact frame |
| applies | Broken stacks (hold variant, per wave) | enemy receives stagger posture per stack | each wave crack applies Broken; enemy sprite shifts toward stagger posture (Unity handles stack counter) |
| reads | Bladestorm active (synergy: +1s knockup) | no caster visual change; knockup timer extended in code | -- |
| consumes | none | -- | -- |

Broken stack note: Broken is an enemy status that accumulates visually as enemy posture degrades toward a full Broken stagger. Warblade OWNS this visual language. Each wave = +1 stack, max stagger at threshold set in game logic.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | wind_up telegraphed by pose |
| impact_particle | yes | upward ground eruption at contact: debris chunks 3-5 px fly upward; ground fissure line 2 px radiates from point; amber-orange tint on fissure edge |
| trail | yes | downswing weapon smear on wind_up to impact transition; vertical directional blur; dissolves on impact frame |
| screen_overlay | no | core skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `knockup` -- enemies visibly elevated above ground plane on knockup_hold frame; wave variant: `broken_stack_flinch` per wave hit |
| audio_anchor_frame | impact F1 | deep reverberant ground crack + rumble |

VFX layer count: 4 (impact_particle, trail, hit_reaction x2 types -- counted as 1 layer category). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | physical ground slam, Broken state application, amber crack tint -- all Warblade territory |
| Avoids every AVOIDS item | PASS | wave cracks are terrain fractures not Ravager ruptures; knockup is physical not magical |
| Counter-archetype distinct (Rule #57) | n/a | Earthsplitter is an active attack, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Broken stacks are flat-application per wave |
| No cross-class state confusion (Rule #58) | PASS | ground fissure amber distinct from Elementalist Frost Wall, Ranger trap lines |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | fissure is terrain/ground effect; Broken is a posture state, not an armor-crack decal; Sundered not applied here |
| Silhouette distinct from class neutral at 64px | PASS | two-hand overhead slam into ground, vertical eruption clearly distinguish from neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, impact, knockup_hold, wave_chain [3 sub-frames], recovery) |
| frames_per_row | 2, 1, 1, 3, 1 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 8 frames (standard 5 + wave extension 3) |
| priority | P1 -- Broken stack origin skill; required before Bladestorm synergy testing |

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
