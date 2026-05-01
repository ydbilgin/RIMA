# SKILL VISUAL CONTRACT -- Brawler: Kidney Hook

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Brawler |
| skill_id | `BRAWLER_KIDNEY_HOOK` |
| display_name | Kidney Hook |
| slot | advanced |
| role | closer / pressure |
| state_owner | yes (damage scales with Charge; During Overdrive: x1.5 damage multiplier) |
| class_anchor_ref | inherit (OWNS: Cracked/Shattered, launch/juggle, whiff body counter; AVOIDS: weapon-armor break, pre-draw counter) |

Tone note: a side-angle footwork-driven power hook. The Brawler steps to the side (footwork), shifts weight, and drives a full-body hook into the target's flank. Not a straight punch -- the arc is horizontal, aiming for the torso side. The power comes from the step and hip rotation. At max Charge (5 = 500% single hit), the body overextends -- full commitment, ugly, devastating. During Overdrive the motion is the same but larger and faster.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 3 | side footwork step (F1-F2), weight transfer + shoulder chamber (F3) -- the step is a visible tell | silhouette shifts laterally on F1; distinct from straight-punch tells |
| active | yes | 3 | hook drive (F1 power arc peak), follow-through rotation (F2), overextend snap (F3) | single hit lands on F1; audio anchor F1; damage = Charge x multiplier applied at F1 impact |
| recovery | yes | 2 | overcommit lean, off-axis after full rotation -- punishable window | exaggerated lean at max Charge; at 5 Charge, recovery lean is more dramatic |

Frame total: 8. Within Brawler advanced quota.

Charge scaling visual note: wind_up and active frames are the same animation for all Charge levels. Charge level affects VFX intensity (particle weight) and recovery exaggeration, not new keyframes.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | none -- Kidney Hook deals damage scaled by Charge but does not apply a new state | -- | -- |
| reads | Charge level (0-5) scales damage: 5 Charge = 500% single hit | Charge level shows as knuckle aura intensity before impact: 1 Charge = faint brown, 5 Charge = full bruise-purple burst at F1 | aura on caster fist during wind_up F3; at 5 Charge, full purple flare before impact |
| reads | During Overdrive (V Burst active): x1.5 damage multiplier | caster fist shows red-orange Overdrive tint on active F1 when Overdrive is active | red-orange rim flash at F1 impact; layered over Charge aura |
| consumes | none -- Charge is read as multiplier, not consumed | -- | -- |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | no cast particle -- footwork step is the tell |
| impact_particle | yes | heavy asymmetric dust-burst at hook contact (F1); weight scales with Charge: at 5 Charge, full bruise-purple burst ring; at 1 Charge, light dust-smear |
| trail | yes | forearm/knuckle horizontal arc smear on active F1, 1-frame motion blur; at 5 Charge, smear is longer and purple-toned |
| screen_overlay | no | advanced skill -- overlay forbidden |
| hit_reaction_on_enemy | yes | `cracked` on base; at 5 Charge, `shattered` if Cracked stack already present; `generic_stagger` if no Cracked |
| audio_anchor_frame | active F1 | hook impact is the audible beat; Overdrive adds secondary low-bass layer at F1 |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Charge multiplier scaling, Cracked/Shattered, off-axis hook body; footwork read |
| Avoids every AVOIDS item | PASS | no weapon-armor break; no pre-draw stillness; hook is body-driven not weapon |
| Counter-archetype distinct (Rule #57) | n/a | Kidney Hook is closer/pressure, not counter |
| No HP-execute visual cue (Rule #56) | PASS | Charge aura reads Charge resource (0-5); Overdrive tint reads V Burst state; neither reads enemy HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Brawler Cracked = organic body hairline fissures; at 5 Charge uses shattered if stacked; no metallic plate shards; Sundered language is Warblade-exclusive |
| Silhouette distinct at 64px | PASS | lateral footwork step + horizontal hook arc is distinct from straight punches and vertical slams; step-and-rotate silhouette is unique |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 3, 3, 2 |
| palette_ref | Brawler class palette (dirt-brown / bruise-purple accent) |
| reference_sprite | `Assets/Sprites/Brawler/brawler_neutral_S43.png` |
| gen_budget_estimate | 8 frames |
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
