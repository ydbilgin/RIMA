# SKILL VISUAL CONTRACT -- Shadowblade: Scarbinding

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_SCARBINDING` |
| display_name | Scarbinding |
| slot | signature |
| role | control / pressure (delayed-payoff) |
| state_owner | yes (applies Scar; collapse triggered separately) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Shadowblade phases past the target rather than at them, leaving a Scar mark anchored on the enemy silhouette. The skill is the PLACEMENT, not the payoff. Collapse is a separate trigger.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | low crouch + blade angled back, body half-dissolved into shadow | |
| active | yes | 4 | phase-through pass: F1 enter (silhouette dissolves), F2 mid-pass (overlap with target -- both half-rendered), F3 emerge behind, F4 turn-back glance with blade still extended | impact (Scar placement): F2 only; audio anchor F2 |
| recovery | yes | 2 | exhale, body rematerializes, blade lowered | |

Frame total: 8. Within Shadowblade signature quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | places Scar on target | new persistent decal anchored to enemy torso/back, world-space (does not rotate with camera) | thin diagonal black-violet gash, 1-frame pulse on placement, then static idle decal |
| reads | n/a | -- | -- |
| consumes | no -- collapse is a separate skill detonating Scar | -- | -- |
| disambiguation_note | Scar vs Ranger Mark vs Hexer pip | Scar: diagonal gash decal, black-violet, body-anchored world-space | Ranger Mark: circular reticle overlay, NOT body-anchored; Hexer pip: small floating orb stack counter, NOT a decal. Any visual ambiguity with these two is a generation blocker. |

Scar is Shadowblade-EXCLUSIVE per anchor.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | shadow-tendril pull at wind_up, brief -- signals geometry phase intent |
| impact_particle | yes | on F2 only: a ribbon of violet-black mist trailing from caster THROUGH target -- must visually cross target body to communicate phase-through |
| trail | yes | blade trail across the pass, drawn as one continuous arc spanning F1->F3 |
| screen_overlay | no | signature, but earns identity through silhouette work, not screen FX |
| hit_reaction_on_enemy | yes | `scarred` reaction set: 1-frame body twitch + Scar decal placement; NO knockback (Shadowblade does not displace, it marks) |
| audio_anchor_frame | active F2 | the pass-through moment is the beat -- soft, not punchy |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | phase-through geometry + Scar placement |
| Avoids every AVOIDS item | PASS | NOT a teleport-slash -- caster physically crosses target frame, blade arc is one continuous line, no flash-cut |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | Scar persists regardless of target HP; collapse skill reads Scar presence, not HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Scar is a gash decal; no armor-crack or shard-burst language used |
| Silhouette distinct at 64px | PASS | mid-pass overlap frame (F2) is unique -- no other class has caster-target body overlap |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 4, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 8 frames + 1 Scar decal asset (gen separately: 2 frames -- pulse + idle) |
| priority | P1 -- signature; collapse skill must ship same batch or feature is incomplete |

Note for rima-codex: Scar decal must be gen'd as its own micro-spritesheet (1 row, 2 frames: pulse + idle) so it can be instantiated independently of the cast animation. Do NOT bake the decal into the caster sheet.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget (incl. decal sub-asset) | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Collapse skill (SHADOWBLADE_SCAR_COLLAPSE) paired in same batch | design lead | [ ] (gate-blocker -- Scar without collapse is a dead state) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
