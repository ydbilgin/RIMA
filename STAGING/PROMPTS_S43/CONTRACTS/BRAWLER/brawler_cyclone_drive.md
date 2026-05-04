# SKILL VISUAL CONTRACT -- Brawler: Cyclone Drive

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_CYCLONE_DRIVE` |
| display_name | Cyclone Drive |
| slot | signature |
| role | control / pressure |
| state_owner | yes (Charge fills during spin; Charged State upgrades damage per turn) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: the Brawler plants a foot and rotates the full body, arms out, for 2 seconds. This is a shoulder-and-arm body rotation -- not a weapon spin. The blur is flesh and clothing. First rotation is a 360 kick (different limb, still the same body rotation theme). Must read as a meathead whirling himself, not a blade cutting arcs.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | wide stance plant, arms spreading out -- short tell before spin begins | planted foot visible; silhouette widens |
| active_start | yes | 3 | first rotation: 360 kick (F1-F2 kick arc, F3 return to spin stance); +2 Charge fires at F3 | kick is a distinct limb arc read; audio anchor F2 |
| loop | yes | 4 | sustained spin: arms extended blur, contact zone around body (F1-F4 looping for 2s duration) | 100%/turn damage on contact; Charge fills each loop cycle; Charged State upgrades to 150%/turn; body rotation blur (NOT weapon trail) |
| recovery | yes | 2 | spin-out stumble, off-axis lean -- planted foot releases | |

Frame total: 11 (loop repeats for duration). Flag: loop row may need additional frames if spin read is unclear at 64px -- design to confirm.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Charge fills during spin | brown-pulse on caster torso once per loop cycle | 1-frame brown pulse radiating from center of body; not knuckle-specific (whole-body spin source) |
| reads | Charged State: 150%/turn on contact (vs 100%) | caster body aura shifts to bruise-purple during loop when Charged State active | purple body-rim glow during loop frames when Charged State is present |
| consumes | none -- Charge accumulates; Charged State is a threshold read, not consumed by loop | -- | -- |
| disambiguation_note | Brawler body-spin vs Warblade weapon-spin | Brawler Cyclone Drive: blur source is arms/clothing/body mass; no blade, no metallic arc; contact damage = blunt impact dust puffs | Warblade spinning skill would show blade trail arcs; Cyclone Drive must NEVER show metallic arcs |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no cast particle -- spin initiates from body movement |
| impact_particle | yes | small blunt dust puffs on each enemy contact during loop; 360 kick (F2) gets a heavier dust-ring kick impact |
| trail | yes | arm/clothing motion blur during loop frames (radial smear pattern, 1-2 frame persistence, brown-dirt tones); NOT a blade arc |
| screen_overlay | no | signature skill -- overlay not needed here |
| hit_reaction_on_enemy | yes | `generic_stagger` on contact; Cracked applies if target is already Off-Balance or on third contact |
| audio_anchor_frame | active_start F2 and loop F1 | kick beat at F2; loop hit rhythm anchored at F1 of each loop cycle |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | body-rotation spin (OWNS), Charge fill, Cracked; off-axis body |
| Avoids every AVOIDS item | PASS | no weapon-armor break; no pre-draw stillness; spin is purely body/limb, no weapon presence |
| Counter-archetype distinct (Rule #57) | n/a | Cyclone Drive is control/pressure, not counter |
| No HP-execute visual cue (Rule #56) | PASS | Charged State reads Charge resource, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | contact damage uses blunt dust puffs; no metallic arc; no plate shards; Sundered language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | radial spin with arms extended is a unique silhouette; no other Brawler skill has a loop/spin pose |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active_start, loop, recovery) |
| frames_per_row | 2, 3, 4, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 11 frames |
| priority | P1 |

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
