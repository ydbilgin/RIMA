# SKILL VISUAL CONTRACT -- Brawler: Jab

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_JAB` |
| display_name | Jab |
| slot | basic |
| role | opener / pressure |
| state_owner | no (builds Charge on hit; does not apply a class state) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: single fast straight punch, street-economy motion. No martial-clean extension. Shoulder-first, elbow tucked, not a boxer's textbook jab -- a dirty fast one. 4x rapid LMB chains into auto-combo, each hit adds +1 Charge.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 1 | micro shoulder coil -- barely a tell, speed is the read | silhouette must break from neutral via shoulder rotation only; no full torso pivot |
| active | yes | 1 | single fast punch -- impact frame | audio anchor F1; knuckle contact is the beat |
| recovery | yes | 1 | arm retracts, guard returns -- gap is short, not punishable | recovery collapses fast to communicate Jab chains |

Frame total: 3. Within Brawler basic quota.

Auto-combo note: 4x LMB repeats this sheet 4 times in sequence. No separate multi-hit sheet needed -- timing controller handles chain in code.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none -- Jab does not apply a class state | -- | -- |
| reads | none -- Jab has no conditional branch on existing states | -- | -- |
| consumes | none | -- | -- |

NONE. Jab is state-agnostic. Charge build is a resource counter handled by the HUD and caster glow tier (see class anchor); no per-hit overlay needed on Jab itself.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | Jab has no telegraphed cast -- speed is the point |
| impact_particle | yes | tiny asymmetric dust puff on knuckle contact; small, fast-dissolve (2-3 px at 64px view); repeat per hit in auto-combo |
| trail | no | single jab at this speed has no perceivable trail; omit for clarity |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `generic_stagger` -- micro-flinch only; no Cracked pip (Jab does not apply Cracked) |
| audio_anchor_frame | active F1 | single sharp impact beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | shoulder-driven punch, no weapon, no magic |
| Avoids every AVOIDS item | PASS | no weapon-armor break language, no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Jab is opener, not counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Charge build is a caster resource, not a target-HP read |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | Jab does not apply Cracked; impact_particle is dust only, no fissure decal |
| Silhouette distinct from class neutral at 64px | PASS | single-frame punch extension clearly differs from neutral guard stance |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 1, 1, 1 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 3 frames |
| priority | P0 -- basic skill, blocks Brawler playable build |

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
