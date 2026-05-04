# Input / Settings / Lighting Follow-up - 2026-05-03

## User Feedback

- Holding movement input, then attacking the opposite direction should turn the character for the attack.
- ESC settings should feel more like real game settings.
- Add temporary class switching in ESC for test/playtest convenience.
- Improve lighting without returning to darkness/fog gameplay.

## Changes

### Combat Facing

File:
- `Assets/Scripts/Player/PlayerController.cs`

Change:
- Added a short combat-facing lock after `FaceCombatTarget()`.
- Default attack/skill aim is now `TowardsMouse`.
- Movement input still updates movement direction, but it does not instantly overwrite combat facing during the attack-facing lock.

Expected result:
- Hold `D`, point/cast left, attack: character turns left for the attack.
- After the short attack window, if `D` is still held, movement facing can return right.

### ESC Settings

File:
- `Assets/Scripts/UI/SettingsMenuUI.cs`

Changes:
- Settings now exposes real gameplay toggles:
  - Attack target: cursor / movement direction.
  - Dash direction: cursor / movement direction.
  - Character HP bar visibility.
- Reset now restores:
  - Attack target = cursor.
  - Dash direction = movement direction.
  - Skill keys = defaults.
- Added temporary test class buttons:
  - Warblade
  - Elementalist
  - Ranger
  - Shadowblade

### Lighting

Files:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`
- `Assets/Scenes/_IsoGame.unity`

Changes:
- Global 2D light tuned from flat white/full intensity to readable cool fill:
  - intensity `0.88`
  - color `RGBA(0.78, 0.84, 0.92, 1.0)`
- Procedural room local light accent increased:
  - `localLightAccentScale = 0.52`
- Room lights now use stronger warm/cyan/violet/moon accent combinations with wider falloff.
- Still no darkness/fog/limited-vision gameplay. Full room readability remains the rule.

Verification screenshot:
- `Assets/Screenshots/debug_game_view_lighting_settings_facing_2026_05_03.png`

## Verification

- Script validation:
  - `PlayerController.cs`: no errors.
  - `SettingsMenuUI.cs`: no errors.
  - `RuntimeRoomManager.cs`: no errors.
- Unity compile: no game compile errors observed.
- EditMode tests: 129/129 PASS.
- Play Mode smoke:
  - Default attack aim: `TowardsMouse`.
  - Dash mode: `FacingDirection`.
  - Auto settings menu exists.
  - Test class switch works through `PlayerClassManager`.
  - Global light is `0.88` with cool fill color.
  - Current generated room has 4 procedural point lights.

## Next

- Playtest actual mouse-opposite attack feel and tune `combatFacingLockDuration` if it is too sticky or too short.
- Replace temporary runtime light composition with final authored room lighting after final environment tiles/props are locked.
