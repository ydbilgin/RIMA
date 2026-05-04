# SKILL VISUAL CONTRACT -- Shadowblade: Scar Collapse

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_SCAR_COLLAPSE` |
| display_name | Scar Collapse |
| slot | signature |
| role | closer / pressure (detonation payoff) |
| state_owner | no (reads and consumes Scar; does not apply new Scar) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Requires 3+ Scars active on target. Shadowblade draws a line through the marked silhouette; all Scars collapse simultaneously, dealing Sever+50 per Scar consumed. The line is a collapse arc, NOT a slash attack -- no weapon swing animation. The payoff of Scarbinding.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | open hand raised, fingers spread toward target -- tracing the collapse arc in air; reads as geometric intent not weapon threat | |
| active | yes | 4 | F1: arc line drawn (thin violet trace connecting all Scar points), F2: simultaneous Scar implosion on target, F3: burst -- gash lines close inward, F4: residual void-dark afterimage on target silhouette | audio anchor F2; no weapon swing in any frame |
| recovery | yes | 2 | caster exhales, hand drops, stance resets | |

Frame total: 8. Within Shadowblade signature quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | n/a -- this skill does not place new Scars | -- | -- |
| reads | requires 3+ Scars active on target | caster hand trace glows violet proportional to Scar count (dim at 3, brighter at 5+) | no screen FX; glow is on caster hand only, world-space |
| consumes | detonates ALL active Scars on target simultaneously | each Scar decal implodes (gash lines close inward, 1-frame per Scar), then burst frame; decals removed from target after F3 | collapse is inward implosion, NOT outward explosion; no knockback; target body flickers void-dark 1 frame |
| disambiguation_note | Scar vs Ranger Mark vs Hexer pip | Scar decals: diagonal black-violet gash, body-anchored world-space; collapse shows gash lines closing inward | Ranger Mark: circular reticle, not body-anchored; Hexer pip: floating orb stack. Collapse burst must not resemble Hexer detonation (orb pop) -- use inward gash-line implosion, not orb burst |

Scar is Shadowblade-EXCLUSIVE. Collapse burst must use inward-closing gash geometry, not radial explosion shape.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | thin violet arc trace from caster fingertip to target on F1 -- geometric line, not a projectile trail |
| impact_particle | yes | on F2-F3: simultaneous per-Scar implosion sparks, violet-black; brief void-dark flicker on target silhouette |
| trail | no | no weapon swing, no trail needed |
| screen_overlay | no | signature tier -- identity through silhouette and Scar implosion geometry, not screen FX (Q3 locked) |
| hit_reaction_on_enemy | yes | `scarred` reaction set extended: multiple Scar implosions then 1-frame void-dark full-body flicker; NO knockback |
| audio_anchor_frame | active F2 | simultaneous Scar implosion is the beat -- layered soft impacts, not a single punch |

VFX layer count: 4 active (cast_particle, impact_particle, hit_reaction, audio). Within gate.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Scar collapse, geometric arc trace, phase-geometry dissolution |
| Avoids every AVOIDS item | PASS | NOT a teleport-slash -- no weapon swing, no blink-flash; the attack is the closing geometry |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | collapse triggers on Scar count (3+), not target HP threshold |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | void-dark gash implosion only; no armor-crack or shard-burst language |
| Silhouette distinct at 64px | PASS | open hand + geometric arc trace is unique vs all Shadowblade weapon-hold poses |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active, recovery) |
| frames_per_row | 2, 4, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 8 frames (Scar decal sub-asset shared with Scarbinding -- do not re-gen) |
| priority | P1 -- batch-paired with SHADOWBLADE_SCARBINDING; neither dispatches without the other |

Note for rima-codex: Scar implosion uses the same decal asset as Scarbinding (pulse + idle sheet). The collapse animation closes those frames in reverse. Do not gen a new decal; reuse Scarbinding decal sheet.

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [x] |
| Frame budget within class quota | design lead | [x] |
| State indicator ownership clean | design lead | [x] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Placement skill (SHADOWBLADE_SCARBINDING) paired in same batch | design lead | [ ] (gate-blocker -- collapse without placement contract is incomplete) |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
