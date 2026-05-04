# SKILL VISUAL CONTRACT -- Shadowblade: Backstab Mark

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_BACKSTAB_MARK` |
| display_name | Backstab Mark |
| slot | passive |
| role | pressure / opener (conditional crit gate) |
| state_owner | yes (reads mark state on enemy; backstab hit = guaranteed crit) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: Passive. When a marked enemy is hit from behind, the hit is a guaranteed critical strike. No active cast. Visual tells: brief crit flash on strike + mark consumed. Silent and precise -- no announcement.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| active | yes | 2 | F1: backstab contact frame (behind enemy), F2: crit flash + mark consumed | passive trigger -- no wind_up or recovery; plays on top of whichever attack triggered it; audio anchor F2 |

Frame total: 2. Passive overlay -- does not count against standard skill quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | n/a -- passive does not apply state itself; reads existing mark | -- | -- |
| reads | checks if marked enemy is hit from behind | brief violet edge-glow on caster blade on F1 -- signals crit condition met; visible only to player not enemies | single-frame blade edge highlight, violet-white, 1 pixel wide max at 128px scale |
| consumes | backstab hit consumes mark | mark decal snaps off target on F2 with a single-frame flicker | mark removal is instantaneous; no linger |
| disambiguation_note | Backstab Mark read-glow vs Scar state | blade edge-glow (violet-white, on caster weapon) is a CASTER-side tell | Scar is a target-side body-anchored decal; these live on different objects and must not be confused |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | passive -- no cast |
| impact_particle | yes | on F2: crit burst -- sharp violet-white spark cluster at contact, slightly larger than standard hit spark |
| trail | no | passive overlay; inherits trail from triggering attack |
| screen_overlay | no | passive -- overlay forbidden |
| hit_reaction_on_enemy | yes | `marked` reaction set consumed: mark flicker-remove on F2 + standard crit stagger |
| audio_anchor_frame | active F2 | crit confirmation is the beat -- crisp, short |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | mark consumption, blade-edge glow on caster, phase-geometry context |
| Avoids every AVOIDS item | PASS | no teleport-slash element; passive overlay only |
| Counter-archetype distinct (Rule #57) | n/a | not a counter archetype |
| No HP-execute visual cue (Rule #56) | PASS | crit triggers on positional condition (behind + marked), not target HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | crit spark is violet-white, not shard-burst |
| Silhouette distinct at 64px | PASS | passive overlay -- silhouette is that of the triggering attack; crit spark differentiates the hit visually |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 1 (active overlay) |
| frames_per_row | 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 2 frames |
| priority | P1 -- passive; requires mark system functional in build |

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
