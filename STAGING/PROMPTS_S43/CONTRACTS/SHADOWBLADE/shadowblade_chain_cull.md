# SKILL VISUAL CONTRACT -- Shadowblade: Chain Cull

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_CHAIN_CULL` |
| display_name | Chain Cull |
| slot | ultimate |
| role | closer / pressure (chained multi-target) |
| state_owner | yes (reads mark state on enemies; 3-hop chain between marked targets) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Shadowblade phase-lunges between up to 3 marked enemies in sequence. Each hop is a phase-lunge (body dissolves, crosses distance, emerges and strikes), NOT a blink-flash (instant teleport with no in-between). The traversal arc must be visible. This is the key Ronin distinction: Ronin blink-teleports to exact target position; Shadowblade phase-lunges along a visible arc.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | crouch-lock onto first marked target, blade cocked; body edge begins dissolve | |
| active_hop | yes | 4 | F1: phase-out (dissolve), F2: lunge arc mid-point (ghost outline visible along arc path), F3: phase-in + emerge (solidify at target), F4: strike frame | this 4-frame set repeats per hop (x3); audio anchor per hop at F4 |
| recovery | yes | 3 | final hop recovery: body fully rematerializes, stance drops low -- heavier recovery than single hop | |

Note: active_hop repeats 3x for a 3-hop chain. Each repetition targets a different marked enemy. Total active frames = 12 (3 hops x 4 frames). Frame total: 2 + 12 + 3 = 17. Near 18-frame cap -- flag for design lead review but within limit.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | n/a -- Chain Cull reads marks, does not apply them | -- | -- |
| reads | requires marked enemies for hop targets | mark decals on enemies light up (brief violet pulse) at wind_up as targeting indicator -- 1-frame highlight per target | mark pulse is on enemy decals, not on caster |
| consumes | mark consumed on each hop-strike | mark decal removed on strike frame (F4 per hop) with a brief flicker | mark removal is instantaneous per hop |
| disambiguation_note | Chain Cull phase-lunge vs Ronin blink-teleport | Chain Cull: ghost outline arc visible during F2 of each hop; traversal path is geometrically visible | Ronin: instant position swap, no mid-air body visible. Chain Cull MUST show the arc path (F2 ghost outline + arc trail) or it will be indistinguishable from Ronin blink at play distance |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | yes | at wind_up: shadow tendril connection lines from caster to each marked target -- shows chaining intent before launch |
| impact_particle | yes | per hop at F4: compact blade-flash on each marked target hit, violet-dark; x3 |
| trail | yes | per hop: arc ghost-trail from source to destination, faint violet; 3 arc trails visible briefly after each hop |
| screen_overlay | no | ultimate -- Q3 locked: no screen FX |
| hit_reaction_on_enemy | yes | `marked` reaction set consumed per hop: mark flicker-remove + generic_stagger on each struck target |
| audio_anchor_frame | active_hop F4 | each strike is a beat; 3 rapid beats in sequence |

VFX layer count: 4 active (cast_particle, impact_particle, trail, hit_reaction). At gate limit.

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | phase-lunge arcs, ghost outline mid-hop, mark consumption |
| Avoids every AVOIDS item | PASS | hop is a phase-lunge (arc visible via F2 ghost outline + trail), NOT a blink-flash teleport; near-miss: if F2 ghost and arc trail are absent this collapses into Ronin-style blink -- both are MANDATORY |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | hops target marked enemies regardless of HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | violet-dark strike sparks only |
| Silhouette distinct at 64px | PASS | mid-hop ghost arc (F2) is unique silhouette -- no other Shadowblade or Ronin state shows an in-transit body arc |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 3 (wind_up, active_hop, recovery) |
| frames_per_row | 2, 4, 3 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 9 frames (active_hop sheet is tiled x3 in engine; gen once) |
| priority | P2 -- ultimate; requires marked-enemy targeting system functional |

Note for rima-codex: active_hop is a single 4-frame sequence instantiated 3 times in engine. Generate once; engine applies to each hop target. Confirm engine supports hop-chaining animation replay before dispatch.

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
