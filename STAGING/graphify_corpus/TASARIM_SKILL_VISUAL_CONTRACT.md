---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Skill VFX görsel contract"
---
# SKILL VISUAL CONTRACT
> Per-skill visual gate document. No contract = no PixelLab generation.
> Owner: rima-asset (fills) -> design lead (signs off) -> rima-codex (gens after sign-off).
> Target fill time: ~15 min per skill. Anything marked `inherit` pulls from class anchor in `SINIF_VE_SKILL_KARAR_BELGESI.md`.

---

## Schema (Template)

### Section A -- Identity Metadata

| Field | Value | Notes |
|---|---|---|
| `class` | <class name> | one of the 10 anchor classes |
| `skill_id` | <SNAKE_CASE> | matches code/data id |
| `display_name` | <name> | player-facing |
| `slot` | basic / signature / ultimate / passive | |
| `role` | opener / closer / reposition / control / pressure / counter / sustain | drives silhouette readability priority |
| `state_owner` | yes/no | does this skill APPLY a class state, or only READ one |
| `class_anchor_ref` | inherit | OWNS/AVOIDS pulled from anchor table -- do not redeclare |

### Section B -- Animation States Required

Frames are PEAK keyframes only (V3 workflow). Typical 3-6 frames per state. List only states this skill needs; omit unused rows.

| State | Required? | Frame count | Purpose | Notes |
|---|---|---|---|---|
| `wind_up` | yes/no | N | tell -- reads at 8m | silhouette must differ from class neutral |
| `active` | yes/no | N | hit/cast moment | impact frame must land on contact |
| `recovery` | yes/no | N | punish window | shows whether hand/weapon is committed |
| `loop` | yes/no | N | for held/channelled only | |
| `cancel` | yes/no | N | only if skill is cancellable mid-active | |

Total frame budget: <sum>. Flag if >18 -- design must justify or trim.

### Section C -- State Indicators (game-state pips / overlays)

Pip = small UI/world icon over actor. Overlay = full-body tint or decal.

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| `applies` | this skill applies state X to enemy/self | new pip/overlay frame needed on TARGET | <describe> |
| `reads` | skill behaves differently if state X present | tell on CASTER (e.g. glow tier) | <describe> |
| `consumes` | skill detonates/clears state X | collapse/burst frame on target | <describe> |

Cross-class state usage is FORBIDDEN unless owner class is listed in anchor (Rule #55 etc.). Mark NONE if skill is state-agnostic.

Disambiguation requirement: if the state applied could visually resemble another class state, add a `disambiguation_note` row explicitly naming the look-alike and the visual differentiator. Minimum cases that MUST be called out: Scar vs Ranger Mark vs Hexer pip; Cracked (Brawler body fissure) vs Sundered (Warblade plate shards); Marked vs any glow-based state.

### Section D -- VFX Requirements

| VFX layer | Required? | Spec |
|---|---|---|
| `cast_particle` | y/n | color palette inherit from class element; describe shape only if non-default |
| `impact_particle` | y/n | tied to active frame N |
| `trail` | y/n | weapon/hand/projectile path |
| `screen_overlay` | y/n | rare -- only signature/ult; flag for review |
| `hit_reaction_on_enemy` | y/n | which reaction set: `cracked`, `shattered`, `sundered` (Warblade only -- Rule #55), `scarred` (Shadowblade only), `marked`, `burned`, `frozen`, `shocked`, `bleed`, `generic_stagger` |
| `audio_anchor_frame` | frame N | which frame the SFX spike lands on |

### Section E -- Identity Compliance Check

Hard gate. Any FAIL blocks generation.

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | | inherit class_anchor_ref |
| Avoids every AVOIDS item | | call out each near-miss |
| Counter-archetype distinct (Warblade absorb / Ronin pre-draw / Brawler whiff) | n/a if not counter | Rule #57 |
| No HP-execute visual cue | | Rule #56 -- must reference state gate, not low-HP color |
| No cracked-armor / Sundered VFX unless class == Warblade (Rule #55) | | Brawler Cracked (body fissures) is permitted; Sundered shard-burst is Warblade-only. State the visual differentiator explicitly in Section C. |
| Silhouette distinct from class neutral pose at 64px downscale | | readability gate |

### Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| `endpoint` | `create_spritesheet` or `create_animation` (NEVER `create_character`) |
| `canvas` | 128x128 (S43 fixed) |
| `ppu` | 64 (fixed) |
| `rows` | one per state listed in Section B |
| `frames_per_row` | as per Section B counts |
| `palette_ref` | inherit from class style sheet |
| `reference_sprite` | path to existing class neutral sheet (locks proportions) |
| `gen_budget_estimate` | <N> credits -- sum of frames |
| `priority` | P0/P1/P2 -- P0 only if skill is on FAZ critical path |

### Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (omit row if skill has no dependency; if present, dependent skills MUST have filled+signed contracts in the SAME batch -- checkbox alone is insufficient) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |

Generation forbidden until all rows checked. File filled contracts under `STAGING/PROMPTS_S43/CONTRACTS/<class>_<skill_id>.md` once signed.

---

## Filled Examples

### Brawler -- Bully

**Section A -- Identity**

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_BULLY` |
| display_name | Bully |
| slot | basic |
| role | opener / pressure |
| state_owner | yes (applies Cracked, escalates to Shattered on third stack) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: street/foul, not martial-clean. Body language is shoulder-driven, off-axis -- NOT samurai-stance. Reads as a guy who fights ugly.

**Section B -- Animation States**

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | low shoulder dip, lead foot scuff -- short tell, this is a fast jab | |
| active | yes | 4 | three-jab chain (jab-jab-cross), final cross is the launcher hint frame | impact: jabs on F1+F2, cross on F4; audio anchor F4 |
| recovery | yes | 2 | overcommitted lean, rear hand low -- punishable if whiffed | |
| cancel | yes | 1 | shoulder roll-out, allowed only after jab #2 | |

Frame total: 9. Within Brawler basic quota.

**Section C -- State Indicators**

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | skill stacks Cracked on target | each connecting jab adds 1 Cracked pip on enemy chest; third stack flips to Shattered overlay (full-body hairline crack decal, 2 frames) | small spider-crack decal, white-on-dark, no metal shards (those are Warblade Sundered) |
| reads | if target already Off-Balance, cross frame plays launcher variant | caster gets a brief knuckle flash on F3->F4 | single-frame yellow knuckle pop, no screen FX |
| consumes | none | -- | -- |
| disambiguation_note | Cracked/Shattered (Brawler) vs Sundered (Warblade) | Brawler: organic hairline body fissures, no metal fragments | Warblade Sundered uses plate shards + metallic burst; Brawler must NEVER use shard language |

Cracked/Shattered are Brawler-OWNED. No Warblade armor-break shard language -- use hairline cracks, not plate fragments.

**Section D -- VFX**

| Layer | Required | Spec |
|---|---|---|
| cast_particle | no | Bully has no telegraphed cast -- that is the point |
| impact_particle | yes | small dust-puff on each jab contact, asymmetric (street-fight feel); cross gets a heavier dust ring |
| trail | yes | knuckle smear on cross only (active F4), 1-frame motion blur |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `cracked` reaction set; on third stack swap to `shattered` |
| audio_anchor_frame | active F4 | cross is the audible beat |

**Section E -- Identity Compliance**

| Check | Pass/Fail | Note |
|---|---|---|
| OWNS-only visual language | PASS | Cracked/Shattered, off-axis body |
| AVOIDS clean | PASS | no weapon-armor break, no pre-draw stillness |
| Counter-archetype distinct | n/a | Bully is opener, not counter |
| No HP-execute cue | PASS | launcher conditional reads Off-Balance state, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Brawler Cracked = organic body hairline fissures, NOT metal plate shards; Sundered shard language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | shoulder dip + jab extension reads vs Brawler neutral |

**Section F -- PixelLab Params**

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active, recovery, cancel) |
| frames_per_row | 2, 4, 2, 1 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 9 frames |
| priority | P0 -- basic skill, blocks Brawler playable build |

**Section G -- Approval Gate**

| Sign-off | Who | Status |
|---|---|---|
| Identity check | design lead | [ ] |
| Frame budget | design lead | [ ] |
| State indicator ownership | design lead | [ ] |
| VFX layer count | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Dispatch ready | rima-codex | [ ] |

---

### Shadowblade -- Scarbinding

**Section A -- Identity**

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

**Section B -- Animation States**

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | low crouch + blade angled back, body half-dissolved into shadow | |
| active | yes | 4 | phase-through pass: F1 enter (silhouette dissolves), F2 mid-pass (overlap with target -- both half-rendered), F3 emerge behind, F4 turn-back glance with blade still extended | impact (Scar placement): F2 only; audio anchor F2 |
| recovery | yes | 2 | exhale, body rematerializes, blade lowered | |

Frame total: 8. Within Shadowblade signature quota.

**Section C -- State Indicators**

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | places Scar on target | new persistent decal anchored to enemy torso/back, world-space (does not rotate with camera) | thin diagonal black-violet gash, 1-frame pulse on placement, then static idle decal |
| reads | n/a | -- | -- |
| consumes | no -- collapse is a separate skill detonating Scar | -- | -- |
| disambiguation_note | Scar vs Ranger Mark vs Hexer pip | Scar: diagonal gash decal, black-violet, body-anchored world-space | Ranger Mark: circular reticle overlay, NOT body-anchored; Hexer pip: small floating orb stack counter, NOT a decal. Any visual ambiguity with these two is a generation blocker. |

Scar is Shadowblade-EXCLUSIVE per anchor.

**Section D -- VFX**

| Layer | Required | Spec |
|---|---|---|
| cast_particle | yes | shadow-tendril pull at wind_up, brief -- signals geometry phase intent |
| impact_particle | yes | on F2 only: a ribbon of violet-black mist trailing from caster THROUGH target -- must visually cross target body to communicate phase-through |
| trail | yes | blade trail across the pass, drawn as one continuous arc spanning F1->F3 |
| screen_overlay | no | signature, but earns identity through silhouette work, not screen FX |
| hit_reaction_on_enemy | yes | `scarred` reaction set: 1-frame body twitch + Scar decal placement; NO knockback (Shadowblade does not displace, it marks) |
| audio_anchor_frame | active F2 | the pass-through moment is the beat -- soft, not punchy |

**Section E -- Identity Compliance**

| Check | Pass/Fail | Note |
|---|---|---|
| OWNS-only visual language | PASS | phase-through geometry + Scar placement |
| AVOIDS clean | PASS | NOT a teleport-slash -- caster physically crosses target frame, blade arc is one continuous line, no flash-cut |
| Counter-archetype distinct | n/a | not a counter |
| No HP-execute cue | PASS | Scar persists regardless of target HP; collapse skill reads Scar presence, not HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Scar is a gash decal; no armor-crack or shard-burst language used |
| Silhouette distinct at 64px | PASS | mid-pass overlap frame (F2) is unique -- no other class has caster-target body overlap |

**Section F -- PixelLab Params**

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

**Section G -- Approval Gate**

| Sign-off | Who | Status |
|---|---|---|
| Identity check | design lead | [ ] |
| Frame budget (incl. decal sub-asset) | design lead | [ ] |
| State indicator ownership | design lead | [ ] |
| VFX layer count | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Collapse skill (SHADOWBLADE_SCAR_COLLAPSE) paired in same batch | design lead | [ ] (gate-blocker -- Scar without collapse is a dead state) |
| Dispatch ready | rima-codex | [ ] |

---

## Codex Review Notes

### Changes Made

1. **Schema Section D -- `hit_reaction_on_enemy` enum extended.**
   Added `sundered` (Warblade only, Rule #55) and `scarred` (Shadowblade only) to the valid reaction set list. Both were used in the filled examples but were absent from the schema enumeration, creating an undocumented extension.

2. **Schema Section C -- disambiguation requirement added.**
   Added a mandatory `disambiguation_note` row requirement after the standard indicator rows. Specifies minimum look-alike pairs that must always be called out: Scar vs Ranger Mark vs Hexer pip; Cracked vs Sundered; Marked vs glow-based states.

3. **Schema Section G -- `Skill dependencies paired in batch` row added.**
   Shadowblade example used a bespoke "Collapse skill paired" gate that had no schema basis. Added a generic form of this gate to the schema template, with instruction to omit if skill has no dependency.

4. **Schema Section E -- Rule #55 check label tightened.**
   Old label: "No cracked-armor VFX unless class == Warblade". New label: "No cracked-armor / Sundered VFX unless class == Warblade (Rule #55)". Added inline guidance distinguishing Brawler Cracked (organic body fissures, permitted) from Warblade Sundered (plate shard-burst, exclusive).

5. **Both examples Section B -- `Notes` column restored.**
   Schema defines a 5-column table for Section B; both examples used only 4 columns, dropping `Notes`. Notes column restored in both examples. Impact frame annotations moved from standalone inline prose into the Notes column of the active state row.

6. **Both examples Section C -- column header normalized.**
   Examples used `Visual`; schema uses `Visual element`. Corrected in both examples.

7. **Brawler Section C -- `disambiguation_note` row added.**
   Explicit row calling out Cracked/Shattered (Brawler hairline fissures) vs Sundered (Warblade plate shards). Previously existed only as inline prose after the table; now enforced as a table row per new schema requirement.

8. **Shadowblade Section C -- `disambiguation_note` row added; inline prose removed.**
   Scar vs Ranger Mark vs Hexer pip disambiguation moved from inline prose after the table into the new mandatory table row format. Inline prose note removed to avoid duplication.

9. **Shadowblade Section E -- Rule #55 check label updated** to match new schema wording.

10. **Shadowblade Section G -- `Collapse skill` row label updated** to reference the expected skill_id `SHADOWBLADE_SCAR_COLLAPSE` explicitly, so the gate can be machine-checked.

### Passed Clean (No Changes Required)

- Rule #56 (no HP-execute visual cue): Both examples correctly reference state gates (Off-Balance, Scar presence) rather than HP thresholds. No change needed.
- Rule #57 (counter-archetype distinct): Both examples mark n/a correctly; neither Bully nor Scarbinding is a counter skill. No change needed.
- Brawler OWNS/AVOIDS compliance: Cracked/Shattered used, weapon-armor break and pre-draw counter absent. Correct.
- Shadowblade OWNS/AVOIDS compliance: Scar placement and phase-through geometry used, generic teleport-slash explicitly disclaimed. Correct.
- Frame budgets: Brawler Bully = 9 frames (basic quota), Shadowblade Scarbinding = 8 frames (signature quota). Both within the 18-frame hard cap. Decal sub-asset (2 frames) is separately tracked and does not inflate caster budget.
- Section F parameters: Both examples use `create_spritesheet`, 128x128, ppu=64. Correct.
- Brawler Section E silhouette gate: shoulder dip + jab extension at 64px noted. Correct.
- Shadowblade F2 mid-pass overlap frame: called out as unique silhouette moment. Correct.
- VFX layer counts: Brawler = 3 active layers (impact_particle, trail, hit_reaction); Shadowblade = 4 active layers (cast_particle, impact_particle, trail, hit_reaction). Both within the <=4 gate.

### Design Lead Decisions (LOCKED 2026-05-01)

**Q1 -- SHADOWBLADE_SCAR_COLLAPSE batch requirement:**
DECISION: Filled+signed `SHADOWBLADE_SCAR_COLLAPSE` contract REQUIRED before Scarbinding dispatches. Checkbox alone is insufficient.
RATIONALE: Scar placement without a locked collapse spec risks visual mismatch and a dead-state shippable; rework cost exceeds batch delay.

**Q2 -- Brawler Bully charge-hold variant:**
DECISION: NO charge-hold variant in any in-scope phase. `loop` row stays omitted.
RATIONALE: A held jab bleeds into Ronin's tension-timing lane -- OWNS-AVOIDS conflict with no gameplay justification.

**Q3 -- Scarbinding `screen_overlay = no` at signature tier:**
DECISION: CONFIRMED intentional and locked.
RATIONALE: Shadowblade's "silent precision mark" fantasy requires no screen announcement; a flash would undercut the Collapse payoff.

**Q4 -- Scar decal world-space anchor:**
DECISION: REQUIRED as hard spec. Engineering must confirm feasibility before sign-off. No sprite-local fallback.
RATIONALE: A camera-rotating Scar breaks spatial-memory gameplay; if engineering blocks it, skill needs redesign, not visual downgrade. Flag for engineering feasibility check before Scarbinding Section G can clear.

2. **Brawler `loop` state** -- Bully has no held/channelled mechanic so the `loop` state is omitted. If Bully ever gains a charge-hold variant, this contract must be revised. Confirm with design lead that no charge mechanic is planned for Bully in the current phase.

3. **`screen_overlay` for Shadowblade Scarbinding** -- The example marks this `no` on the basis that "identity comes from silhouette work, not screen FX." At signature tier, this is a judgment call. Design lead should confirm this is intentional and not an oversight.

4. **Scar decal world-space anchor** -- Section C specifies "world-space (does not rotate with camera)." This is a rendering constraint that needs confirmation from the engineering lead -- does the current S43 pipeline support world-anchored decals on enemy silhouettes?

