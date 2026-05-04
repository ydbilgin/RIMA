# SKILL VISUAL CONTRACT -- Warblade: Battle Surge

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Warblade |
| skill_id | `WARBLADE_BATTLE_SURGE` |
| display_name | Battle Surge |
| slot | 10 (Advanced, Unity state pending) |
| role | Rage-to-HP conversion stance / sustain window |
| state_owner | yes -- Battle Surge (Unity overlay pending, spec: TASARIM/UNITY_STATE_OVERLAY_SPEC.md) |
| class_anchor_ref | inherit (OWNS: Sundered/Broken state, absorb-counter, weapon-impact armor crack; AVOIDS: armor language by Ravager/Brawler) |

Tone note: 8-second surge window where every Rage spend recovers HP. The visual identity is controlled burning aggression -- the character's armor seams and weapon pulse with amber-orange, and each Rage-spend event produces a brief life-surge flash on the caster (not a heal beam, not a magic glow -- a physical surge of momentum). At Rage 80+ the window extends to 12s; this is represented by a deeper seam burn, not a new animation. Advanced skill -- more intense amber presence than core stances.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| activate | yes | 3 | character drives weapon pommel into ground then rises -- aggressive planting; armor seams ignite amber | frame 1: pommel plant; frame 2: rise with seam ignition; frame 3: fully lit surge stance |
| active_loop | yes | 3 | combat-ready loop with persistent amber seam burn; stance is aggressive-forward, not defensive | 3-frame loop; seam brightness holds steady at full burn; character shifts weight forward-ready |
| surge_pulse | yes | 1 | per Rage-spend event: brief amber flash across chest and shoulders -- life-surge moment | 1 frame; plays per HP recovery trigger; fast dissolve back to active_loop |
| deactivate | yes | 1 | seam burn fades; weapon pommel lifts from ground; return to guard | |

Frame total: 8. Within Warblade advanced quota.

Rage 80+ note: seam burn is rendered at a second brightness tier by Unity shader (not a separate animation row). Sprite contract records base burn; Unity implements the conditional tier.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | Battle Surge active state (8s) | caster enters surge stance | Unity overlay pending: amber seam burn on caster armor for duration |
| applies | HP +%5 per Rage spend (2s internal CD) | surge_pulse frame fires per recovery event | brief amber chest flash -- physical surge cue, not a heal animation |
| reads | Rage 80+ (synergy: 12s duration) | seam burn tier brightens (Unity shader); no new animation row | -- |
| consumes | none | -- | -- |

state_owner note: Unity overlay implementation pending per TASARIM/UNITY_STATE_OVERLAY_SPEC.md.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | on activate: amber crack lines radiate from pommel-to-ground contact; 3-4 px lines; fast fade into active_loop |
| impact_particle | no | surge stance has no direct hit |
| trail | no | stance skill, no movement |
| screen_overlay | no | advanced skill -- overlay still forbidden |
| hit_reaction_on_enemy | no | this skill affects caster only |
| audio_anchor_frame | activate F2 | low rumble surge + Rage ignition hum |

VFX layer count: 1 (cast_particle). Compliant.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Rage-driven amber burn, weapon pommel plant, life-surge from aggression -- no magic, no range |
| Avoids every AVOIDS item | PASS | HP recovery is from Rage resource, not a green heal aura; no Ravager lifesteal visual |
| Counter-archetype distinct (Rule #57) | n/a | Battle Surge is a sustain stance, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | no HP-conditional visuals; Rage 80+ is a resource-state gate, not an HP read |
| No cross-class state confusion (Rule #58) | PASS | amber armor burn distinct from Ravager mutation aura, Brawler Charge glow, Elementalist Fire State |
| No cracked-armor / Sundered VFX (Rule #55) | PASS | cast_particle crack lines are at ground from pommel (terrain), not enemy armor; Sundered not applied |
| Silhouette distinct from class neutral at 64px | PASS | pommel-plant activation and forward-ready surge stance differ from neutral and from Ironclad Momentum guard |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 4 (activate, active_loop, surge_pulse, deactivate) |
| frames_per_row | 3, 3, 1, 1 |
| palette_ref | Warblade class palette (muted steel-grey, amber-orange Rage crack accent) |
| reference_sprite | `Assets/Sprites/Characters/Warblade/base/warblade_S.png` |
| camera_note | MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally. |
| gen_budget_estimate | 8 frames |
| priority | P2 -- advanced sustain skill; secondary to P0/P1 combat chain skills |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
