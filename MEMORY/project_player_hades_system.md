---
name: Player Hades-Style Movement and Combat Aim System
type: project
description: Architecture of player facing, movement, and combat aim separation. Implemented 2026-05-03.
---

## Core Architecture (LOCKED 2026-05-03)

Movement facing, combat aim, and visual facing are **three separate concerns** in RIMA.

### Movement Facing
- 4 diagonal quadrants only: `SE / NE / NW / SW`
- PlayerController starts at SE on spawn
- Deadzone-safe: pure horizontal/vertical input preserves missing axis (no snapping to cardinal)
- Visual facing switch delay: adjacent = 0.05s, opposite = 0.10s (reduces hard 180-cuts)

### Combat Aim (`AttackAimMode`)
- Two modes selectable in Settings UI (ESC overlay):
  - `SON YON` = use last character movement facing
  - `MOUSE` = aim toward mouse cursor position
- Mouse mode does NOT drive walking facing continuously -- only changes visual facing when an attack or skill fires (Hades-like)
- `PlayerController.FaceCombatTarget()` is called by:
  - `PlayerAttack` before hit/VFX/anim trigger (basic attack)
  - `SkillBase.TryActivate()` before skill execution (all skill classes inherit this)

### Settings UI Integration
- `SettingsMenuUI` auto-initializes at runtime via `[RuntimeInitializeOnLoadMethod]`
- Creates overlay canvas -- no scene setup needed
- ESC opens/closes the settings menu even without a SettingsMenu object in scene

**Why:** Hades 1/2 pattern proven for isometric action: character visually faces movement direction, but skills/attacks snap to aim direction at cast moment. Prevents awkward tank-turning mid-movement.

**How to apply:** When adding new skills or attack classes, always call `FaceCombatTarget()` in TryActivate/execute before spawning VFX or triggering anim. Do not try to add continuous mouse-walk-facing; it was explicitly rejected.
