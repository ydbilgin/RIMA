# SKILL VISUAL CONTRACT -- Ranger: Pinning Shot

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_PINNING_SHOT` |
| display_name | Pinning Shot |
| slot | basic |
| role | control |
| state_owner | yes (applies Root 1.5s to target) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: precise, methodical. No flourish -- this is a disciplined snap-shot fired from a braced stance. Ranger plants, draws, releases. Movement tells are minimal and intentional.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | half-draw stance, rear elbow raised, front arm extended -- readable tell at 8m | bow limbs flex slightly; silhouette differs from neutral: wider arm span |
| active | yes | 3 | full draw on F1, release snap on F2, follow-through on F3 | arrow leaves bow on F2; audio anchor F2; impact frame is F2 |
| recovery | yes | 2 | bow arm lowers, weight settles -- short but visible punish window | caster remains planted; no movement |

Frame total: 7. Within Ranger basic quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | skill roots target for 1.5s | circular pin overlay on ground under target feet on arrow impact | thin bone-white ring decal around feet, 2-frame flash then held until root expires; NOT a full-body overlay |
| reads | none | -- | -- |
| consumes | none | -- | -- |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. Root ring is ground-anchored and circular; it does NOT resemble any of these. Mark (Ranger) is a circular reticle hovering ABOVE the enemy body, not touching the ground -- Root ring sits at ground level and is a different visual register. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | snap-shot has no pre-cast telegraphing particle -- that is the point |
| impact_particle | yes | small bone-white dust burst on arrow contact (active F2); secondary root-ring expand pulse at ground level on same frame |
| trail | yes | arrow shaft trail from bow to target, active F2 only -- thin line, earth-green tint |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `generic_stagger` on impact then root idle pose held; root ring persists until 1.5s elapsed |
| audio_anchor_frame | active F2 | bowstring release is the audible beat |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | distance shot, planted stance, arrow projectile -- all Ranger OWNS |
| Avoids every AVOIDS item | PASS | no run-and-gun movement, no teleport element |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | root is unconditional on hit, no low-HP color reference |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor-break language; root ring is ground-level bone-white, not plate shard burst |
| Silhouette distinct at 64px | PASS | wide arm span of full draw distinguishes from Ranger neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 7 frames |
| priority | P1 |

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
