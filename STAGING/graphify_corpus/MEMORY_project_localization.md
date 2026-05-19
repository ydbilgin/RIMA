---
name: Localization Policy
description: Day 1 Modular, TR+EN Priority. No hardcoded strings.
type: project
---

# DECISION #51 (2026-04-24)
* Priority: TR (Turkish), EN (English) - Phase 1
* Expansion: DE, FR, ES, RU, PT, ZH-CN, JP, KR (Phase 2+)

# TECHNICAL SPEC
* System: Unity Localization Package (SO-based tables)
* Constraint: NO hardcoded strings. Use LocalizedString (TMP) or LocalizationManager.Get(key)
* Naming: ui.hud.hp, skill.warblade.slash.name, room.clear.message
* Font: TMP SDF multi-atlas (Latin+TR Phase 1, CJK Phase 4)
* Formatter: {0}, {1} dynamic placeholders

# PHASE PLAN
* Phase 1: HUD, Death screen, Skill draft, Tooltips, Settings
* Phase 2+: Dialog, Lore, Boss intro
