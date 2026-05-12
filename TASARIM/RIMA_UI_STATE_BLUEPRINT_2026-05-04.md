---
status: REFERENCE
faz: 1
tarih: 2026-05-04
ozet: "RIMA UI state blueprint"
---
# RIMA UI State Blueprint

Status: design blueprint, not implementation lock
Date: 2026-05-04
Audience: AI agents first; keep compact.

## Core Rule

RIMA UI supports combat decisions. It is not a skill catalogue.

Priority:
1. player/enemy readability
2. danger telegraphs
3. HP/resource
4. interaction/objective
5. skill readiness
6. route/reward/build detail

Skills stay visible but quiet in combat. Full skill details live in reward/build overlays.

## Visual Rule

Use dark fractured stone, pale cyan spirit light, controlled gold for reward/value, and red rift
light only for danger/curse/elite/boss. Panels should be translucent, stable, sharp-edged, and
small enough to leave combat space readable.

Avoid giant skill names, full inventory feel, always-visible passive text, floating arrow-only
gates, full map reveal, and color-only meaning.

Production detail:
- use 9-slice frames and runtime text/icons
- no baked gameplay text in generated UI art
- icons must read as high-contrast silhouettes
- important reward/combat icons should target 48px+ readability
- common/rare/epic/legendary = gray/teal-blue/purple/gold
- reward cards should slide in, glow on hover, and flash on selection
- hold/inspect can show long descriptions when input flow is ready

## Current Runtime Surfaces

- `HUDController`: HP, resource, gold, room status, interact prompt, objective arrow
- `SkillBarUI`: Q/E/R/F primary, Z/X secondary
- `PassiveStatusUI`: passive/proc surface
- `DungeonMapUI`: M map, fog, step-1/step-2 reveal
- `SkillOfferUI`: reward cards and replace mode
- `ChestUI`, `ForgeUI`: special reward interactions
- `GateBehavior`: gate room-type sprite
- `RuntimeRoomManager`: room clear, rewards, gates, map fragment

## Combat HUD

Show:
- top-left compact HP/resource
- bottom-center small skill bar
- top-center tiny room objective/status
- passive/proc icons only when relevant

Do not show skill descriptions, full passive list, large map, or route cards.

Skill bar:
- icon first, cooldown radial second, small key label
- ready state = subtle glow/border
- locked/cooldown state never resizes slots
- secondary slots are visually quieter until unlocked

## Danger State

Use for low HP, elite/boss pressure, curse, or hazards.

Show HP pulse/thin vignette plus a small danger icon near HP or room status. Do not cover
enemy/projectile space or stack warning panels.

## Interaction State

Sources: reward, map fragment, chest, forge, event, merchant, gate.

Show small `[G] Action` prompt, object highlight/beam, and one-line action label. Do not open a
modal before interaction.

## Room Clear

Before reward pickup:
- brief `ROOM CLEARED`
- reward is highlighted
- gates stay locked/dim

After reward pickup:
- valid gates open
- each gate shows room-type promise in world
- optional route strip shows next 1-2 nodes

## Reward Draft

Use 3-card structure because current draft flow exists.

Card content:
- icon, name, type, tier
- 1-line effect
- tags/synergy chips

Layout:
- 3 cards center
- side tooltip/build summary
- current active slots small

If active slots are full, show replace mode. Skill names are visible, but not oversized.

## Gate Choice

Gate count is data-driven: 1, 2, 3, or more exits.

Gate visuals are in-world thresholds: arch, breach, stair, chained doorway, rift threshold, lift,
bridge mouth, or shrine passage.

Each gate shows room-type icon/light, lock/reveal state, and target promise. Unrevealed gates use
door silhouette plus fog/dim icon, not a fake missing doorway.

Reference: `TASARIM/UI_CONCEPTS/rima_gate_socket_concept_2026-05-04.png`
Detail: `TASARIM/GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT_2026-05-04.md`

## Map

Partial by default.

Visible: visited, current, direct exits, fragment-revealed nodes, maybe step-2 fog preview.
Hidden: full route, exact far-node count, boss certainty unless revealed.

Map fragments reveal next 1-2 nodes unless a special rule says more.

## Build Overlay

Pause/character menu only, not normal combat.

Show class identity, active kit, passives/echoes, tags/synergies, current route/room, recent reward.
Avoid equipment grid, backpack, and classic RPG stat-sheet focus.

Only show `Rift Tension`, `Soul`, `Reroll`, or similar run resources when backed by real runtime
systems. Reserve layout space if needed; do not fake values.

Reference: `TASARIM/UI_CONCEPTS/rima_ui_template_concept_2026-05-04.png`
Production notes: `TASARIM/UI_PRODUCTION_RULES_FROM_OPUS_REVIEW_2026-05-04.md`

## Special Rooms

- Chest: simple reward choice; no inventory.
- Forge: upgrade/milestone UI; show eligible skills/passives.
- Merchant: prices later; current prototype should not promise deep economy.
- Event: one dilemma panel with choice, cost, reward, risk.
- Curse: risk-first panel; red/rift visual; confirm before accept.
- Boss: gate confirmation plus identity hint; no full mechanic tutorial.

## Implementation Rule

Build UI as templates fed by runtime data. Do not bake text into concept art. Frames/cards/slots
are reusable; content comes from player, room, route, offer, and passive systems.

