# SKILL VISUAL CONTRACT -- Ranger: Final Strike

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_FINAL_STRIKE` |
| display_name | Final Strike |
| slot | signature |
| role | closer |
| state_owner | no (requires BOTH Mark AND Trap active on target; consumes both; deals 400% damage) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: this is the execution shot of a methodical kill-zone sequence. Ranger stands at distance, takes a slow deliberate aim, and fires one devastating arrow. The double-condition gate (Marked AND Trapped) means the player has earned this moment. The animation must communicate: weight, inevitability, distance discipline. It is NOT a dash, NOT a teleport, NOT a melee approach. Ranger stays planted. The arrow travels. The target was already boxed.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 4 | full draw, deliberate -- slower than Pinning Shot wind_up; Ranger lowers eye to arrow, weight fully planted; F4 is peak-hold frame (held draw, maximum tension) | longer wind_up signals power; silhouette: fully planted stance, bow at full draw, head angled down to aim |
| active | yes | 3 | F1: release (arrow departs); F2: arrow impact on target + double-consume collapse (Mark reticle and Trap object both destroyed simultaneously); F3: camera-hold frame (damage number registers here) | both Mark and Trap consume VFX fire on F2 simultaneously; the impact is the only moment of excess -- the caster never moves |
| recovery | yes | 2 | bow arm lowers slowly, stance relaxes -- weight settles; deliberate unhurried recovery signals confidence, not vulnerability | intentionally calm recovery; punish window is minimal (Ranger earned this shot) |

Frame total: 9. Within Ranger signature quota. Flag: 4-frame wind_up is above typical but justified by Master tier and 400% power gate.

Critical constraint: Ranger does NOT move toward target at any point. No dash, no lunge, no step-in. The full animation happens from the caster's original position. Any approach movement would read as Shadowblade territory and must be rejected at QC.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none | -- | -- |
| reads | skill only activates if target has BOTH Mark AND Trap active | caster wind_up gains a dual-pulse on F1: one earth-green pulse (Mark detected) + one bone-white pulse (Trap detected) on the bow limb -- two distinct brief flashes confirming both conditions | dual-pulse: earth-green first (0.5 frame), then bone-white (0.5 frame); if only one condition is met, skill is not castable (handled by system, no visual needed for partial state) |
| consumes | both Mark AND Trap are consumed simultaneously on active F2 | Mark reticle implosion (ring collapses inward, same as Marked Detonate spec) + Trap object snap-burst (world trap object destroyed, bone-white debris scatter, 2 frames) fire simultaneously on F2 | both consume VFX must be readable as two distinct events happening at once: reticle implosion (top of enemy) + trap burst (ground level below enemy) |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. The dual-consume on F2 must clearly show the Mark reticle (circular, hovering above body) collapsing, NOT a Scar gash closing or a Hexer orb popping. The trap burst is ground-level bone-white debris, NOT an arcane circle (Hexer) and NOT plate shards (Warblade Sundered). |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | dual-pulse on bow limb during wind_up F1: earth-green + bone-white sequential flash; brief and tight -- signals condition confirmation, not a power-up charge |
| impact_particle | yes | on active F2: (1) Mark reticle implosion at hover position above target; (2) Trap snap-burst at ground level below target; (3) primary arrow impact burst -- heavy bone-white + earth-green energy burst, larger than Pinning Shot impact; all three fire simultaneously |
| trail | yes | single arrow shaft trail, same spec as Pinning Shot but heavier weight -- earth-green with bone-white core on active F1 |
| screen_overlay | no | Master-tier but the power is expressed through impact VFX, not screen overlay; overlay is reserved for V Burst (Spirit Bow) |
| hit_reaction_on_enemy | yes | double-consume: `marked` reticle implosion + trap-burst sequence; then heavy `generic_stagger` with knockback; damage number spike on F3 |
| audio_anchor_frame | active F1 (bowstring release, heavy/deep); active F2 (dual impact explosion -- the loudest beat in the skill) |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | empowered aimed shot from range, dual-state consumption, bone/sinew visual register -- all Ranger OWNS |
| Avoids every AVOIDS item | PASS | zero caster movement; explicitly not a dash or teleport; no melee approach; no Shadowblade or Gunslinger visual language |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill; it is a set-up execution |
| No HP-execute visual cue (Rule #56) | PASS | gates on Mark + Trap state flags, not enemy HP level; wind_up dual-pulse shows state detection, not HP threshold |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | trap burst is bone-white debris (sinew/bone register), NOT plate shards; impact burst is energy, NOT armor fragmentation |
| Silhouette distinct at 64px | PASS | 4-frame slow deliberate full-draw with planted stance and lowered-head aim is unique in the Ranger kit and in the broader roster |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 4, 3, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 9 frames |
| priority | P1 |

Note: dual-consume VFX (Mark implosion + Trap burst) are world-space effects dispatched separately. Both must be in the same generation batch as Final Strike caster sheet.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Marked Detonate and Bone Trap contracts must be signed before Final Strike dispatch -- dual-consume VFX references both; all three must ship in same batch) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
