# SKILL VISUAL CONTRACT -- Warblade: Death Blow

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_DEATH_BLOW` |
| display_name | Death Blow |
| slot | 12 (Master) |
| role | execute finisher / Rage dump / maximum single-target damage |
| state_owner | no (requires Broken OR Sundered on target to activate; empties Rage on use; no persistent caster state applied) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: the Warblade's signature finishing move. Requires target to be Broken OR Sundered -- the warrior locks onto a visually compromised enemy and delivers a single devastating blow that expends all Rage in one explosive release. %400 damage (Crippling Blow active: %600). The motion is slow and absolute: wind-up that consumes a full breath, then a single overwhelming downward strike. No flourish, no chain -- one terminal hit. The Rage dump is visual: all amber seam energy channels into the weapon for the wind-up, then detonates at impact. Master-tier presence.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 4 | character raises weapon with both hands, body fully rotates to load; all Rage channels visibly into weapon -- seams across armor drain toward weapon | 4 frames for Master tier; each frame increases weapon glow intensity; Rage bar drains on HUD during this sequence |
| apex | yes | 1 | weapon held at full overhead -- maximum Rage glow; brief hold frame before execution | 1 frame; communicates weight of the moment |
| active | yes | 2 | catastrophic downward slam onto Broken/Sundered target; impact frame then explosion of energy release | frame 1: contact -- Rage detonation burst; frame 2: aftermath dust and debris settling |
| recovery | yes | 2 | weapon heavy on ground; character straightens; all glow gone -- Rage empty | slow recovery; communicates full Rage expenditure |

Frame total: 9. Within Warblade Master quota.

Activation gate: Death Blow is only available when target is Broken OR Sundered. This is a CLASS STATE gate (Rule #56 compliant). The skill UI greys out if neither state is present. No HP-percentage conditional.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| requires | target is Broken OR Sundered (class state gate) | skill available indicator in HUD; no caster sprite change | target's Broken stagger or Sundered fissure decal is the visual pre-condition |
| applies | empties all Rage on cast | Rage bar drains to zero during wind_up frames | amber glow channels from armor into weapon across wind_up sequence |
| applies | %400 (or %600 with Crippling Blow) damage | single massive hit number; no special VFX for damage number | detonation burst on active F1 communicates the magnitude |
| reads | Crippling Blow active on target (synergy: %600 instead of %400) | no visual change on caster; damage multiplier handled in code | -- |
| consumes | all Rage | Rage drains during wind_up | see wind_up animation row |

Rule #56 compliance note: activation requires Broken OR Sundered (class states), NOT raw HP%. This contract is compliant.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | wind_up: amber Rage channels from armor seams toward weapon -- progressive glow build; each wind_up frame increases glow radius 1-2 px outward from weapon |
| impact_particle | yes | active F1: Rage detonation burst at impact -- large amber explosion 5-6 px radius; chunks of debris and ground crack at contact; fade over 2 frames |
| trail | yes | weapon downswing on active F1: heavy directional smear; amber-charged trail, wider than standard skills (3-4 px) -- Master tier weight |
| screen_overlay | no | Master skill -- overlay still forbidden (class house rule: overlay reserved for only specific Master effects if approved separately) |
| hit_reaction_on_enemy | yes | `execution_impact` -- maximum body displacement on Broken/Sundered target; Sundered fissure decal intensifies at impact point before target dies/staggers |
| audio_anchor_frame | active F1 | massive boom + Rage detonation crack -- loudest single hit in Warblade kit |

VFX layer count: 4 (cast_particle, impact_particle, trail, hit_reaction). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Rage drain into weapon, Sundered/Broken prerequisite, single devastating blow -- all Warblade OWNS territory |
| Avoids every AVOIDS item | PASS | no Ravager/Brawler armor language; Sundered on target is prerequisite read, not new application by this skill (Rule #55 compliant) |
| Counter-archetype distinct (Rule #57) | n/a | Death Blow is a finisher, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | activation requires Broken OR Sundered class state, NOT raw HP%. Gate is class-state-driven. Compliant. |
| No cross-class state confusion (Rule #58) | PASS | amber Rage detonation distinct from Elementalist radiant burst, Shadowblade shadow explosion |
| No cracked-armor / Sundered VFX stolen (Rule #55) | PASS | this skill READS the existing Sundered decal on target (applied by Sunder Mark); it does not independently produce armor-crack VFX. The intensification of fissure on impact is a read-confirm, not a new Sundered application. |
| Silhouette distinct from class neutral at 64px | PASS | dual-hand overhead raise with Rage glow is unmistakably distinct from all other Warblade poses |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, apex, active, recovery) |
| frames_per_row | 4, 1, 2, 2 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent -- maximum intensity on wind_up and active rows) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 9 frames |
| priority | P1 -- Master finisher; required for Warblade identity review; depends on Sunder Mark and Earthsplitter contracts |

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
