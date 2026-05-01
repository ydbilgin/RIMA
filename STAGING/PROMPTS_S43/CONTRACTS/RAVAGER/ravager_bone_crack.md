# SKILL VISUAL CONTRACT -- Ravager: Bone Crack

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Ravager |
| skill_id | `RAVAGER_BONE_CRACK` |
| display_name | Bone Crack |
| slot | signature |
| role | opener / pressure |
| state_owner | yes (applies 8s vital exposure on enemy -- see Section C; this is a targeted mark, not a class-owned pip state equivalent to Brawler Cracked) |
| class_anchor_ref | inherit (OWNS: HP trade, low-life danger, frenzy chain; AVOIDS: armor break (Warblade), fancy movement (Shadowblade)) |

Tone note: a single targeted strike to expose a vital point -- aimed at a joint, throat, or unarmored gap. The effect is an 8s window where the next Bloodlust Strike gets +50% (single hit only). With Death Wish active: 12s duration + 2 hits empowered instead of 1.

Disambiguation with Warblade armor break: Bone Crack exposes a VITAL (flesh, joint) NOT plate armor. The visual must use bone/flesh language, NOT metal plate shard language. No Sundered overlay.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | precise targeting pause -- weapon tip raised, head tilted to find the vital; BRIEF but reads differently from wild swings | this is the one moment the Ravager aims; still reads brutal, just aimed |
| active | yes | 3 | targeted strike: F1 weapon drives toward vital point, F2 impact (bone crack audible beat), F3 strike embedded briefly then pulled | impact frame F2; no wide arc -- this is a jabbing drive, not a sweep |
| recovery | yes | 2 | pull back, step away; target has exposure marker (see Section C) | short recovery; skill is fast for a setup move |
| cancel | no | -- | not cancellable | |

Frame total: 7. Within Ravager advanced quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | skill places 8s vital exposure on enemy | bone-white crack marker on enemy at the hit location (joint, torso gap) -- a visible fracture-mark indicating the vital is exposed; pulses slowly for duration | marker is bone-white, small, placed at impact site; NOT full-body overlay; NOT metal shards (Warblade territory) |
| reads (Death Wish) | Death Wish active at cast -> 12s duration + 2 hits empowered | when Death Wish is active: marker gets a faint red inner glow in addition to bone-white; duration counter on marker reads 12s | red inner glow on marker only; subtle, not dominant; differentiated from 8s version by glow presence |
| consumes (paired) | Bloodlust Strike hitting the marked target consumes the vital exposure marker | marker collapses on Bloodlust Strike hit; brief bone-white burst at impact site (1 frame) | collapse is a crack-burst: marker shatters visually and disappears; +50% (or +100% with 2-hit) damage is a number event |
| disambiguation_note | vital exposure marker vs Warblade Sundered vs Brawler Cracked | Bone Crack marker: bone-white fracture mark at a specific body location (joint/flesh), small, pulsing; placed only at the struck vital point. Brawler Cracked: spider-crack fissures across body, organic, 3-stack. Warblade Sundered: plate shards + metallic burst, full armor break. These three must not be visually confused. Bone Crack uses no metal shard language and no full-body fissure pattern. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | wind_up reads as a targeting pause; no particle |
| impact_particle | yes | impact F2: bone-crack particle burst at target vital -- bone-white chips + red blood micro-spray; small, targeted, not wide |
| trail | yes | weapon drive trail on active F1->F2, hot red, 1-frame; short jabbing trail, NOT a sweep arc |
| screen_overlay | no | no screen overlay |
| hit_reaction_on_enemy | yes | `generic_stagger` (light flinch) on impact; the vital marker then persists separately for 8s |
| audio_anchor_frame | active F2 | bone crack impact is the audible beat; sharp crack sound, not a heavy thud |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | targeted vital strike with self/class buff chain; bone-flesh language; hot red palette |
| Avoids every AVOIDS item | PASS | vital marker uses bone/flesh language, NOT plate-shard armor break (Warblade territory explicitly avoided); no fancy movement |
| Counter-archetype distinct (Rule #57) | n/a | not a counter skill |
| No HP-execute visual cue (Rule #56) | PASS | vital exposure is a timed window (8s/12s), not HP-gated; +50% damage is a number property; no HP color cue on target |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | vital marker = bone-white fracture at flesh point; NO plate shards, NO metallic burst; Sundered language is Warblade-only |
| Silhouette distinct at 64px | PASS | brief targeting-pause wind_up is distinct from Ravager's usual wind-up poses; jabbing drive active frame vs sweep-based skills |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Ravager class palette (hot red / bone-white accent) |
| reference_sprite | `Assets/Sprites/Ravager/ravager_neutral_S43.png` |
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
| Skill dependencies paired in batch (if any) | design lead | [ ] (paired with ravager_death_wish.md -- Death Wish active modifies Bone Crack duration; both contracts in this batch) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
