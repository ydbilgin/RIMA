# RIMA UI / QA / AI Skill Notes

Status: design input, not lock
Date: 2026-05-04
Audience: AI agents first; keep compact.

## Rule

If writing for AI agents, write compact operational context:
`decision -> why -> source/pointer -> action -> avoid`.

Do not write long explanatory prose unless the user is the audience. Keep enough context to prevent
drift.

## Runtime Truth

Exists now:
- Room types/code paths: Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse.
- Route graph: `DungeonGraph` branches and assigns room types.
- Gates: `GateBehavior` can show room-type sprites.
- Reward draft: `RewardPickup -> DraftManager -> SkillOfferGenerator`.
- Offers: new active, active upgrade, passive, passive upgrade.
- UI bases: `SkillOfferUI`, `SkillBarUI`, `DungeonMapUI`, `ChestUI`, `ForgeUI`,
  `CharacterSheetUI`.

Partial/prototype:
- Merchant/Event/Forge/Curse content is not yet rich encounter design.
- Traits surface in UI but are partly placeholder.
- Spirit/Unknown are design-doc concepts, not current `RoomType` entries.

Avoid:
- Do not present RIMA as a classic equipment/inventory RPG until item/relic systems exist.
- Do not make level/stat/equipment grid the primary character menu.

## UI Direction

Source image: `TASARIM/UI_CONCEPTS/character_menu_concept.png`.
Generated reference: `TASARIM/UI_CONCEPTS/rima_ui_template_concept_2026-05-04.png`.
State blueprint: `TASARIM/RIMA_UI_STATE_BLUEPRINT_2026-05-04.md`.

Keep from image:
- in-world dungeon background behind UI
- left info spine
- top-right map
- bottom skill bar
- dark stone/gold frame mood

Replace content with RIMA run systems:
- class identity
- HP/resource
- active kit and upgrades
- passives/echoes/stacks
- tags/synergies
- current room objective
- route/gate preview
- recent reward state

First mockup target:
- `RIMA Run Codex - Character Build Overlay`
- Treat image output as template/reference, not final static UI.
- Runtime UI should fill frames/cards/slots from current player, room, route, and offer data.
- no equipment grid
- no backpack
- no generic RPG item slots

## Encounter UI

Use Hades-like three-choice reward UI because draft flow exists.

Do not imply full Hades-like encounter depth yet:
- Combat: clear + draft
- Elite: harder clear + better weighted reward
- Chest: simple reward choice
- Forge: upgrade/milestone interaction
- Merchant/Event/Curse: route promises, prototype content until implemented

## QA Position

Unity tests are useful regression guards, not a full QA tester.

EditMode covers:
- compile/API regressions
- room graph invariants
- offer pool rules
- active/passive routing
- class/skill slot contracts

QA/playtest covers:
- combat readability
- room framing
- UI obstruction
- reward timing
- camera/gates/spawns
- repro steps + screenshot/log + severity

## Memory vs Skill

Memory = facts, decisions, pointers.
Skill = repeatable behavior: what to open, what to avoid, steps, output format, verification.

Create a skill only for repeated, error-prone workflows that save context. Do not create skills for
one-off knowledge.

Potential RIMA skills:
- `rima-context-router`: choose relevant memory/design files only.
- `rima-status-compactor`: keep status compact and archive detail.
- `rima-room-blueprinter`: output room masks, gates, combat question, module needs.
- `rima-ui-designer`: turn real systems into UI prompts; reject generic RPG panels.
- `rima-unity-qc`: define/run test + Play Mode + screenshot/log checks.
- `rima-pixellab-module-planner`: batch PixelLab modules with size/cost assumptions.
- `rima-doc-lint`: ASCII, stale notes, pointer validity, bloat.
- `rima-playtest-report`: QA report from manual/MCP play sessions.

## Image Prompt Pointer

Use this prompt idea for UI concept generation:

Dark mythic pixel art UI for RIMA. Isometric ruined dungeon background visible behind translucent
dark stone and tarnished gold frames. Character build overlay, not RPG inventory. Show class,
HP/resource, current act/room, four active skill cards, passive/echo rows, stack counts, synergy
tags, recent reward, partial branching map with only next 1-2 nodes visible, and bottom skill bar.
No equipment grid, no backpack, no generic item slots. Pale cyan spirit light, controlled gold,
subtle red rift cracks, readable premium roguelite UI.
