# SKILL VISUAL CONTRACT -- Shadowblade: Night Aperture

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_NIGHT_APERTURE` |
| display_name | Night Aperture |
| slot | advanced |
| role | control / pressure (Scar multiplication) |
| state_owner | yes (spawns Scar at dash entry AND exit points) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Tone note: passive geometry threat. Each dash silently doubles the Scar grid. No announcement -- the player feels the trap closing only when collapse happens. Reads as a predator that leaves wounds just by moving.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | brief crouch + blade drawn back, body edge dims -- signals 6s buff activation | silhouette must differ from neutral; low-energy tell, not dramatic |
| active | yes | 3 | buff active state: idle loop variant with faint violet edge on limbs + shadow-thread trail at feet | 1-frame entry pulse when buff activates; loop is continuous for 6s duration |
| dash_scar_spawn | yes | 2 | on each dash during buff: F1 = entry point flash (small Scar decal pops at origin), F2 = exit point flash (Scar decal pops at destination) | not a full animation row -- these are world-space decal events, not caster animation frames; see Section D note |
| recovery | yes | 2 | exhale + buff fade; shadow-thread at feet dissipates | on buff expiry (6s elapsed) or manual cancel |

Frame total: 9 caster frames + 2-frame Scar spawn event (world-space). Within Shadowblade advanced quota.

Note for rima-codex: Scar decal spawned by Night Aperture is IDENTICAL to Scarbinding Scar decal (thin diagonal black-violet gash, world-space anchor). Do NOT gen a new decal asset -- reuse `SHADOWBLADE_SCARBINDING` Scar decal sheet.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | each dash during buff spawns 2 Scars (entry + exit) | new Scar decal placed at dash origin AND destination in world space | thin diagonal black-violet gash decal, 1-frame pop, then static idle -- identical to Scarbinding Scar |
| reads | buff is active (Night Aperture running) | caster has faint violet edge shimmer on body outline for buff duration | 1-pixel violet rim light, continuous, NOT a glow burst |
| consumes | none -- Scars accumulated here are collapsed by SHADOWBLADE_SCAR_COLLAPSE (Severance) or other collapse triggers | -- | -- |
| disambiguation_note | Scar (Shadowblade) vs Ranger Mark vs Hexer pip | Scar: diagonal gash decal, black-violet, body-anchored world-space | Ranger Mark: circular reticle, NOT body-anchored; Hexer pip: small floating orb stack counter. Night Aperture Scars are IDENTICAL in visual language to Scarbinding Scars -- same decal asset. |

Scar is Shadowblade-EXCLUSIVE per anchor. Night Aperture Scars must share the same decal asset as Scarbinding to ensure visual consistency and avoid state confusion.

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | on buff activation: brief shadow-tendril burst at caster feet -- signals geometry mode active |
| impact_particle | yes | on each dash during buff: small violet-black mist puff at entry point (F1) and exit point (F2); each puff marks the Scar placement moment |
| trail | yes | during buff: persistent shadow-thread trail at caster feet when moving; 1-2 frame fade. Reinforces "leaving marks" readability. |
| screen_overlay | no | buff skill -- identity comes from world-space Scar placements, not screen announcement. Q3 decision locked: screen_overlay = no for signature/advanced tier. |
| hit_reaction_on_enemy | no | Night Aperture does not hit enemies directly; Scars are placed at caster movement points, not on enemy bodies |
| audio_anchor_frame | active F1 (buff activation) | soft activation chime; each dash-Scar spawn has its own micro-SFX (same as Scarbinding placement beat) |

Note: Scar spawn events (entry + exit) are world-space decal instantiations, not caster animation frames. They reuse the Scarbinding Scar decal asset and its placement SFX. No additional gen budget required for decal.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | Scar placement + phase-through geometry (dash = phase movement); shadow-thread trail; violet-black palette |
| Avoids every AVOIDS item | PASS | no generic teleport-slash; dash during buff is standard Shadowblade phase movement, not a Ronin-style blink-cut |
| Counter-archetype distinct (Rule #57) | n/a | Night Aperture is a buff enabler, not a counter |
| No HP-execute visual cue (Rule #56) | PASS | Scar placement has no relationship to enemy HP; buff places Scars on world positions, not enemy states |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | Scar decal is a gash, not an armor crack or plate shard; no shard-burst language used |
| Silhouette distinct at 64px | PASS | buff-active rim light + shadow-thread trail distinguishes from Shadowblade neutral; dash-Scar pop is a world event, not ambiguous with caster silhouette |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active/loop, recovery) |
| frames_per_row | 2, 3, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 7 caster frames (Scar decal reused from SHADOWBLADE_SCARBINDING -- 0 additional gen cost) |
| priority | P1 -- Scar-multiplication tool; pairs with Scar Collapse for combo payoff |

---

## Section G -- Approval Gate

| Sign-off | Who | Status |
|---|---|---|
| Identity check (Section E) | design lead | [ ] |
| Frame budget within class quota | design lead | [ ] |
| State indicator ownership clean | design lead | [ ] |
| VFX layer count <= 4 | rima-asset | [ ] |
| Reference sprite exists | rima-asset | [ ] |
| Scar decal reuse confirmed (SHADOWBLADE_SCARBINDING asset, no new gen) | rima-codex | [ ] |
| Ready for `create_spritesheet` dispatch | rima-codex | [ ] |
