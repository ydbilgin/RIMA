# SKILL VISUAL CONTRACT -- Brawler: Curbstomp

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_CURBSTOMP` |
| display_name | Curbstomp |
| slot | advanced |
| role | control / pressure |
| state_owner | yes (applies airborne on all hit enemies; Cracked may apply on multi-hit; Charge 3+ extends range and adds ring blast) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: the Brawler slams a fist (or both fists) into the ground in a straight line, sending a shockwave forward. Ground energy, not a blade cut. 6m line AoE launches all enemies airborne for 1.5s. With 3+ Charge, the line extends to 8m and a ring blast radiates from the impact point. The cracking visual is ground energy / earth fracture -- NOT armor-break shards.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | high chamber: fist raised overhead, weight drops onto front foot -- reads as downward ground slam | body goes HIGH before impact; silhouette tall before stomp |
| active | yes | 4 | fist slams ground (F1 peak impact), ground crack wave travels forward (F2-F3), full line extends to 6m (F4) | all enemies on line go airborne at F2; audio anchor F1; Charge 3+ variant: line extends to 8m at F4b + ring blast at F1 |
| recovery | yes | 2 | kneel/crouch after impact, arm planted on ground -- recovery from slam force | punishable from behind |

Frame total: 8 (base). Charge 3+ variant adds ring blast at F1 as overlay layer (no extra animation row needed -- same frames, added VFX).

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | airborne state on all enemies in line | each enemy in line rises with dot-trail (rising dots, same Brawler juggle visual) | 3-4 rising white dots per enemy; vertical arc; NOT horizontal shard burst |
| applies | Cracked on multi-hit (if target contacted by crack wave on F2 and F3) | hairline fissure decal on enemy torso at second contact | organic body hairline crack; no metallic shards |
| reads | Charge 3+: 8m range + ring blast | caster fist glows brown-orange on wind_up F2 when Charge >= 3 | 1-frame brown-orange knuckle flare on wind_up F2; ring blast at F1 is a separate VFX layer |
| consumes | Charge 3+ is read but not fully consumed (only used as threshold gate) | no collapse VFX | -- |
| disambiguation_note | Curbstomp ground crack vs Warblade Sundered / armor-break | Curbstomp: crack is in the GROUND (earth fracture line, dirt color, travels forward); no body shard fragmentation on enemy | Warblade Sundered = metallic plate shards burst from enemy body; Brawler cracks = earth/ground energy only; enemy bodies rise intact |
| disambiguation_note | Curbstomp airborne vs Aerial Rave juggle | both use rising dot-trail; Curbstomp affects multiple enemies in a line (mass launch); Aerial Rave is single-target focused juggle | same visual language, different scope |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no cast particle -- slam initiates from movement |
| impact_particle | yes | ground slam crater at fist impact point (F1): dirt burst upward + forward; crack wave (F2-F4): traveling ground fracture line, dirt-brown tones, 6m; Charge 3+ ring blast: radial dirt ring at F1 impact, expands outward |
| trail | no | no trail -- ground slam has no projectile or weapon path |
| screen_overlay | no | advanced skill -- overlay not needed here |
| hit_reaction_on_enemy | yes | `generic_stagger` on launch; `cracked` if second contact (multi-hit from wave); rising dot-trail per enemy for airborne duration |
| audio_anchor_frame | active F1 | slam is the audible beat; Charge 3+ adds secondary bass ring-blast at F1 |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | launch/juggle (OWNS), Cracked (OWNS), Charge threshold read; off-axis body |
| Avoids every AVOIDS item | PASS | no weapon-armor break; crack is ground energy (earth fracture), not armor-break; no pre-draw stillness |
| Counter-archetype distinct (Rule #57) | n/a | Curbstomp is control/pressure, not counter |
| No HP-execute visual cue (Rule #56) | PASS | Charge 3+ threshold reads Charge resource, not enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | ground crack wave = earth fracture (dirt color, travels in ground plane); enemy bodies rise intact with dot-trail; no metallic plate shards; no body shard fragmentation; Sundered language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | overhead chamber + downward slam is a vertical slam silhouette; distinct from all punch and spin poses |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 4, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 8 frames |
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
| Skill dependencies paired in batch (if any) | design lead | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
