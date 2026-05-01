# SKILL VISUAL CONTRACT -- Ranger: Spirit Bow

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ranger |
| skill_id | `RANGER_SPIRIT_BOW` |
| display_name | Spirit Bow |
| slot | ultimate |
| role | pressure / opener |
| state_owner | yes (6s duration: infinite ammo; every hit applies Mark; this is the V Burst) |
| class_anchor_ref | inherit (OWNS: trap lines, marks, kill zones, distance discipline; AVOIDS: run-and-gun, generic teleport) |

Tone note: V Burst -- the Ranger's ultimate expression of distance mastery. A spirit-form bow manifests over or alongside the physical bow, amplifying it for 6 seconds. This is not a new weapon; it is a transcendent state of the existing weapon. The animation must read as an awakening or empowerment of what the Ranger already carries. Every shot during the active window auto-applies Mark. The tone is focused, apex-predator calm -- not explosive rage.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | Ranger raises both arms -- bow arm elevated, non-bow hand open upward, head tilted back briefly -- spirit bow materializes over physical bow during this state | spirit bow VFX materializes as an overlay on the physical bow sprite; wind_up F3 is peak spirit-bow-full-form hold before active |
| active_entry | yes | 2 | Ranger snaps to firing stance -- spirit bow fully visible; first arrow nocked; eyes forward, grounded | this is the start of the 6s active window; caster returns to near-neutral firing stance but with spirit bow visible |
| loop | yes | 3 | looping idle-with-spirit-bow: slight bow-holding sway, spirit bow maintains visible glow; Ranger remains in heightened stance | loop fires for the 6s duration; must not feel restless or aggressive -- methodical, ready |
| active_fire | yes | 3 | per-shot animation during V Burst: draw, release, follow-through -- same structure as Pinning Shot active but faster (1-frame draw, 1-frame release, 1-frame follow-through) | every active_fire cycle auto-applies Mark to hit target; spirit bow glows briefly on each release |
| recovery | yes | 2 | spirit bow dissolves, physical bow only remains; Ranger lowers weapon, exhale settle | spirit bow dissolve is a fade-out over 2 frames, not a sudden disappearance |

Frame total: wind_up 3 + active_entry 2 + loop 3 + active_fire 3 + recovery 2 = 13 frames. Justified: V Burst ultimate, higher frame budget approved. Flag for design lead review.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | every hit during V Burst applies Mark to struck enemy | per-hit: Mark reticle appears hovering above struck enemy immediately after active_fire release frame | identical to all Ranger Mark applications: circular hover reticle, earth-green tint, above body, not on skin |
| reads | V Burst is active state on caster | spirit bow overlay on physical bow persists throughout loop and active_fire states; a subtle earth-green aura pulses on caster body during the 6s window -- dim, not blinding | spirit bow overlay: semi-transparent bone-white second bow silhouette overlaid on physical bow; aura: 1-frame-interval dim earth-green pulse on caster body edges |
| consumes | V Burst ends after 6s | recovery animation plays on duration end; spirit bow fades; no explicit consume event per-hit | |
| disambiguation_note | Mark (Ranger) vs Scar (Shadowblade diagonal gash decal) vs Hexer pip (floating orb stack) -- all three are mandatory disambiguation targets. Auto-applied Mark per hit must retain the circular hover-above-body read even when multiple enemies are being marked in rapid succession during the 6s window. Spirit bow overlay on physical bow must NOT resemble a Hexer spirit-form or arcane transformation -- Ranger spirit bow is a physical second bow silhouette (same bow shape), bone-white, semi-transparent, not rune-covered or orb-decorated. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | spirit bow materialization aura during wind_up: brief bow materialization effect -- bone-white semi-transparent second bow shape rises from physical bow over 3 frames; earth-green mist at bow limb tips; this is the screen overlay moment |
| impact_particle | yes | per active_fire hit: standard arrow impact dust puff (same as Pinning Shot) + immediate Mark reticle spawn above target; faster cadence than single skills but each impact is individually readable |
| trail | yes | per active_fire arrow: same spec as Pinning Shot trail but with a faint bone-white core to signal spirit-bow empowerment; arrow trails are slightly brighter/wider than non-V-Burst arrows |
| screen_overlay | yes | V Burst activation only -- brief bow materialization aura on wind_up F1-F3: a soft earth-green + bone-white radial glow emanating from Ranger's bow; fades before active_entry; NOT a full-screen flash; overlays edges and weapon only |
| hit_reaction_on_enemy | yes | `marked` reticle auto-applied on every hit throughout 6s window; `generic_stagger` on each hit |
| audio_anchor_frame | wind_up F1 (spirit bow summoning chord); active_entry F1 (stance lock -- sharp resonant click); active_fire F2 (per-shot release -- each shot has a distinctive spirit-bow tone atop physical bowstring); recovery F1 (spirit bow dissolve exhale) |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | spirit bow is a transcendent state of the physical bow (Ranger OWNS); auto-Mark on hit is core Ranger identity; distance discipline maintained throughout 6s window |
| Avoids every AVOIDS item | PASS | no run-and-gun; Ranger maintains distance throughout V Burst; spirit bow is physical-bow-form, NOT an arcane or teleport element |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | V Burst is triggered by player input (Focus mechanics), not enemy HP; duration is time-based |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | no armor language; bone-white spirit overlay is weapon-form, not plate |
| Silhouette distinct at 64px | PASS | dual-bow silhouette (physical + semi-transparent spirit overlay) is unique in the roster; readable at 64px because spirit bow extends slightly beyond physical bow limb tips |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, active_entry, loop, active_fire, recovery) |
| frames_per_row | 3, 2, 3, 3, 2 |
| palette_ref | Ranger class palette (earth-green / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ranger/ranger_neutral_S43.png` |
| gen_budget_estimate | 13 frames -- above standard; justified by V Burst ultimate tier |
| priority | P1 |

Generation note: spirit bow overlay is a layered VFX element on the physical bow. Confirm PixelLab layering approach before dispatch -- options are (a) bake spirit bow into caster frames directly, (b) generate spirit bow overlay as a separate transparent-background sprite composited in Unity. Option (b) is preferred for animation flexibility during the loop state.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Skill dependencies paired in batch (if any) | design lead | [ ] (Mark reticle VFX asset must be finalized before Spirit Bow dispatch -- auto-Mark per hit relies on same asset; all Mark-applying skills should ship in one batch) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
