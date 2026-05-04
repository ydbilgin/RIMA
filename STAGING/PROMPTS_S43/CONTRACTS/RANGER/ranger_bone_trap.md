# SKILL VISUAL CONTRACT -- Ranger: Bone Trap

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_BONE_TRAP` |
| display_name | Bone Trap |
| slot | basic |
| role | control |
| state_owner | yes (trap object placed in world; triggers apply Root + Mark to crossing enemy) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: cursor-zone placement. Ranger does not throw the trap -- they place it via a cursor targeting reticle. The caster's animation is a deliberate, low-energy drop-placement gesture. The trap object is ground-resting, NOT a projectile.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | Ranger looks toward cursor zone, hand reaches to belt/pouch -- short placement tell | no bow involvement; this is a free-hand gesture |
| active | yes | 2 | arm extends toward target zone, trap drops from hand on F2 | trap object VFX appears at cursor ground position on F2; caster does NOT move toward zone |
| recovery | yes | 2 | arm pulls back, neutral stance resumed | caster re-locks distance stance; no lingering commitment |

Frame total: 6. Within Ranger basic quota.

Trap object is a persistent world-space object (separate sprite/prefab). The trap asset itself is NOT part of the caster spritesheet. Caster sheet only covers the placement gesture.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | trap triggers apply Root + Mark on crossing enemy | when an enemy crosses the trap, two sequential VFX: (1) bone-white snap ring on ground (Root applied); (2) circular reticle appears hovering above enemy body (Mark applied) | Root ring: same bone-white ground-circle as Pinning Shot. Mark reticle: circular hover overlay above body, NOT baked to skin |
| reads | if Marked Detonate is active, trap crossing triggers blast instead of (or in addition to) standard root | trap object gets a faint earth-green pulse when Marked Detonate is primed -- single-frame indicator on the trap sprite to signal chain state | trap world-object pulse: 1-frame green outline flash on trap sprite |
| consumes | none | -- | -- |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. Mark placed by trap trigger is identical to Mark placed by other Ranger skills: circular reticle hover above body, not touching skin, earth-green tint. Root ring is ground-level bone-white and is a separate visual register from Mark. Trap snap-ring must NOT resemble a Hexer binding circle (Hexer uses arcane rune language; Ranger uses bone/sinew language). |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | ground placement, no projectile trail from caster to zone |
| impact_particle | yes | trap trigger reaction only: bone-white snap-burst + Root ring expand at ground level when enemy crosses; if chain fires, add earth-green explosion burst (same spec as Marked Detonate impact_particle) |
| trail | no | trap itself has no trail; the trigger reaction has a brief snap-burst but no directional trail |
| screen_overlay | no | basic skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | Root ring on ground + `marked` reticle hover above body on trigger; if chain fires, add `generic_stagger` |
| audio_anchor_frame | active F2 (trap placement click); separate trigger SFX on world-event when enemy crosses |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | ground trap placement, mark application, kill-zone setup -- all Ranger OWNS |
| Avoids every AVOIDS item | PASS | no run-and-gun, no projectile travel, no teleport; placement is deliberate and stationary |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | trap triggers on crossing condition, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | bone/sinew trap language; no plate shards or armor language |
| Silhouette distinct at 64px | PASS | free-hand pouch-reach gesture is distinct from bow-draw neutral |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 2, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 6 frames (caster sheet) + trap world-object sprite (separate dispatch) |
| priority | P1 |

Dependency: Bone Trap and Wireline Trap share trap-object visual language. Both world-object sprites should be dispatched in the same batch to ensure consistent bone/sinew style.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Wireline Trap world-object sprite must be in same batch for visual consistency; Marked Detonate chain VFX must be confirmed before trap chain state can be signed off) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
