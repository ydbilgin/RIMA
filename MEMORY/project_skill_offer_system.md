---
name: Skill offer system
description: Hades-style room reward model, active-to-passive routing, minimum tags
type: project
---

Canonical decision doc: `TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md`.

RIMA does not use a visible skill tree for run progression. Use Hades-style 3-choice room rewards:
new active, owned-skill upgrade, passive/echo, tag synergy, resource mod, risk offer.

Active slot rule: an active skill must apply/consume a readable state, create a positional/zone question,
move in a class-owned way, answer an encounter problem, or create visible resource-risk. Pure damage,
pure buff, or "future cast is stronger" effects route to passive/echo/upgrade offers.

High-priority active-to-passive candidates: Warblade Iron Crush/Ironclad Momentum/Battle Surge,
Elementalist Radiant Pillar/Element Charge, Shadowblade Night Aperture/Shadow Clone, Ronin Iai Pressure,
Gunslinger Hot Lead, Brawler Unstoppable Force, Summoner Dark Pact/Lich Form, Ravager Death Wish.

PixelLab rule: skill sheets are concept references only. Production splits caster animation, VFX/projectile,
ground decal, impact VFX, state pip/overlay, and Unity hit-stop/camera shake/flash/slide. No per-skill
bespoke enemy animation, ragdoll, struggle, boss-specific reaction, or baked character+VFX+enemy panel.

Phase 1 implementation tag minimum: `broken`, `sundered`, `marked`, `trapped`, `burning`, `frozen`,
`scar`, `cracked`, plus shape tags `melee`, `projectile`, `line`, `cone`, `zone`, `dash`, `counter`, `summon`.
