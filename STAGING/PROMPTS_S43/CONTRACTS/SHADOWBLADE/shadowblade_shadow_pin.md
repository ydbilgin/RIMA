# SKILL VISUAL CONTRACT -- Shadowblade: Shadow Pin

---

## Section A -- Identity Metadata

| Field | Value |
|---|---|
| class | Shadowblade |
| skill_id | `SHADOWBLADE_SHADOW_PIN` |
| display_name | Shadow Pin |
| slot | signature |
| role | control (1.5s root) |
| state_owner | yes (applies root state to target via dagger throw) |
| class_anchor_ref | inherit (OWNS: Scar placement/collapse, phase-through geometry; AVOIDS: generic teleport-slash) |

Concept: A thrown dagger that roots the target for 1.5s. Silent and precise -- the dagger pins the target's shadow to the ground rather than physically impaling them. Root state visual must read as shadow-anchor, not a bleed or physical pin.

---

## Section B -- Animation States Required

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| wind_up | yes | 2 | reverse-grip dagger drawn back to throwing position, weight shifts | |
| active | yes | 3 | F1: release frame (dagger leaves hand), F2: mid-flight (projectile frame), F3: impact + root application on target | F1 is audio anchor (throw); F3 is root-seal moment |
| recovery | yes | 2 | throwing arm returns to guard | |

Root state on TARGET:

| State | Required | Frames | Purpose | Notes |
|---|---|---|---|---|
| root_idle | yes | 3 | target shadow-pinned: dark shadow-anchor expands from feet/base of target, loops for 1.5s | shadow bloom on ground below target; target body cannot move but animates in place |
| root_expire | yes | 2 | shadow-anchor dissolves, target freed | |

Frame total: 7 (caster) + 5 (root sub-asset). Caster within quota.

---

## Section C -- State Indicators

| Indicator | Source | Action | Visual element |
|---|---|---|---|
| applies | dagger impact applies root | dark shadow-anchor bloom on ground at target feet; target body shows faint edge-darken (shadow grip) | shadow-anchor is a ground decal, NOT a body decal; distinct from Scar (body-anchored) and Death Mark (body radiant pulse) |
| reads | n/a | -- | -- |
| consumes | root expires after 1.5s | root_expire plays: shadow-anchor shrinks and dissolves | no burst on expiry -- clean fade-out |
| disambiguation_note | Shadow Pin root vs Scar vs Death Mark | Shadow Pin root: ground shadow-anchor at feet, no body gash | Scar: diagonal gash decal on body torso; Death Mark: radiant pulse on body. Shadow Pin is the only Shadowblade state anchored to the GROUND rather than the body. |

---

## Section D -- VFX Requirements

| VFX layer | Required | Spec |
|---|---|---|
| cast_particle | no | dagger throw has no cast warmup particle -- stealth throw |
| impact_particle | yes | on F3: dagger embeds + shadow-anchor bloom expands from impact point; dark violet shadow burst, ground-plane |
| trail | yes | dagger in-flight trail: thin dark line across F1->F3 path |
| screen_overlay | no | signature -- no screen FX |
| hit_reaction_on_enemy | yes | `generic_stagger` on impact + root overlay applied; no knockback (root holds in place) |
| audio_anchor_frame | active F1 | dagger release is the beat -- sharp, silent (silenced throw) |

---

## Section E -- Identity Compliance Check

| Check | Pass/Fail | Note |
|---|---|---|
| Uses only OWNS visual language for this class | PASS | shadow-geometry pin, dark-violet palette, precision throw; root as shadow-anchor fits phase-geometry OWNS |
| Avoids every AVOIDS item | PASS | projectile throw is not a teleport-slash; caster stays at range |
| Counter-archetype distinct (Rule #57) | n/a | not a counter |
| No HP-execute visual cue (Rule #56) | PASS | root applies regardless of target HP |
| No cracked-armor / Sundered VFX (Rule #55 -- Warblade only) | PASS | shadow-anchor bloom; no armor-crack or shard language |
| Silhouette distinct at 64px | PASS | reverse-grip throw arm extension reads distinct from all other Shadowblade poses |

---

## Section F -- PixelLab Generation Parameters

| Field | Value |
|---|---|
| endpoint | `create_spritesheet` |
| canvas | 128x128 |
| ppu | 64 |
| rows | 5 (wind_up, active, recovery, root_idle, root_expire) |
| frames_per_row | 2, 3, 2, 3, 2 |
| palette_ref | Shadowblade palette (violet-black, low-saturation, single bright accent on blade edge) |
| reference_sprite | `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png` |
| gen_budget_estimate | 12 frames |
| priority | P1 -- signature; root requires enemy movement-lock system functional |

Note for rima-codex: root_idle and root_expire are ground-plane sub-assets (not body-layer). Gen as separate mini-sheet for ground-decal instantiation. Dagger projectile may need a standalone 1-frame asset for in-flight rendering.

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
