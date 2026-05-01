# SKILL VISUAL CONTRACT -- Ranger: Wireline Trap

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_WIRELINE_TRAP` |
| display_name | Wireline Trap |
| slot | signature |
| role | control |
| state_owner | yes (world object: tensioned line between 2 points; triggers apply Snare + Mark for 8s to crossing enemies) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: precision zone denial. Ranger sets two anchor points; a tensioned line (wire/sinew) spans between them. This is NOT a projectile between two casts -- it is a placement of two physical anchors with a visible line connecting them. The caster animation covers both anchor placements in sequence. The wire itself is a persistent world-space object, NOT a caster sprite element.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | Ranger reaches to belt, draws a small anchor/spike in non-bow hand -- placement readiness tell | short; signals intent without telegraphing direction |
| active_point1 | yes | 2 | arm extends, first anchor planted at cursor point A on F2 | caster does not move to point A; placement is gestural, like Bone Trap; anchor appears at cursor world position on F2 |
| active_point2 | yes | 2 | arm re-extends, second anchor planted at cursor point B on F2; wire VFX snaps between A and B on this frame | wire materializes as a tight sinew/bone-white line stretched between the two world anchors on F2 of this state; wire has tension-vibration micro-animation (1-2 loop frames on the wire object, separate from caster sheet) |
| recovery | yes | 2 | hand returns to neutral; Ranger steps back to observe zone | |

Frame total: 8. Within Ranger signature quota.

The wire world-object is a separate sprite/prefab with its own loop animation (tension vibration). The caster sheet covers only the two anchor placement gestures.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | crossing enemy receives Snare + Mark for 8s | on enemy crossing the wire: (1) wire snap-reaction (wire object brightens on contact frame, 1 frame); (2) Snare ring appears at ground level around enemy feet (bone-white thin circle, tighter than Root ring -- indicates slowed movement not full stop); (3) Mark reticle appears hovering above enemy body | Snare ring: thinner bone-white circle than Root; persists with a slow fade-pulse for 8s. Mark reticle: same spec as all Ranger Mark applications |
| reads | if Marked Detonate is active, each crossing triggers a blast | wire object gains same earth-green pulse as Bone Trap chain-state indicator when Marked Detonate is primed | wire chain-state: 1-frame green outline pulse propagates along wire length from anchor to anchor |
| consumes | none (wire persists until 8s elapsed or manually cancelled) | -- | -- |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. Mark applied by wire crossing is identical to all other Ranger Mark applications: circular hover reticle above body, earth-green tint, NOT on-skin. Snare ring is distinct from Root ring (Pinning Shot / Bone Trap): Root ring is solid bone-white, ~1.5 tile radius; Snare ring is thinner, ~0.75 tile radius, with a slow fade-pulse. Wire itself (sinew/bone-white stretched line) must NOT be mistaken for a Hexer binding line (Hexer uses arcane rune-script language on bindings; Ranger wire is a physical sinew/cord with no rune markings). |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | ground placement via gesture; no pre-cast particle trail from caster |
| impact_particle | yes | point A anchor plant: small bone-white thud particle on active_point1 F2; point B anchor plant: same on active_point2 F2; wire snap-reaction on crossing enemy: wire brightness flash + Snare ring expand + Mark reticle appear -- three simultaneous events on crossing frame |
| trail | no | wire itself is persistent world-object, not a trail; no caster trail |
| screen_overlay | no | signature skill -- no screen overlay |
| hit_reaction_on_enemy | yes | Snare ring + `marked` reticle on crossing; if chain fires, add `generic_stagger` + Marked Detonate explosion |
| audio_anchor_frame | active_point1 F2 (first anchor plant click); active_point2 F2 (second anchor + wire tension snap -- audible wire-tension sound); separate trigger SFX when enemy crosses |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | tensioned trap line, zone denial, mark application from range -- core Ranger OWNS |
| Avoids every AVOIDS item | PASS | no caster movement, no projectile travel, no run-and-gun; wire language is sinew/physical, not arcane |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | triggers on crossing condition, not HP; 8s duration is state-driven |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no plate or armor language; bone-white wire is physical sinew register |
| Silhouette distinct at 64px | PASS | dual anchor-plant gesture is a unique sequence in the Ranger kit; distinct from single-placement Bone Trap |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (wind_up, active_point1, active_point2, recovery) |
| frames_per_row | 2, 2, 2, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 8 frames (caster sheet) + wire world-object sprite with tension-loop (separate dispatch) |
| priority | P1 |

Dependency: Bone Trap and Wireline Trap world-object sprites share bone/sinew visual language and must be dispatched in the same batch for style consistency.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Bone Trap world-object sprite must be in same batch; Marked Detonate chain VFX must be confirmed before wire chain state is signed off) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
