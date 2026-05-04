# Hades-like Attack Facing Fix - 2026-05-03

## User Problem

When holding movement input, for example holding `D` to walk right, attacking in the opposite
or another direction still left the character visually facing the movement direction.

Expected behavior:
- Movement direction and combat aim are separate.
- If attack/skill aim is cursor-based, the character should turn toward the attack/cursor at
  attack start, even while WASD movement continues.
- After the short attack-facing window, movement may return the character to the held movement
  direction.

## Hades Reference Summary

Hades has separate controls/options for:
- Aim Assist
- Attack at Cursor
- Dash at Cursor

Sources checked:
- Black Screen Gaming Hades menu guide lists `Aim Assist`, `Attack at Cursor`, and
  `Dash at Cursor` as separate controls-menu options:
  `https://www.blackscreengaming.com/hades/menus/`
- Steam Hades discussion summary for `Attack at Cursor`: it means the cursor affects attacking,
  not walking:
  `https://steamcommunity.com/app/1145360/discussions/0/3412054783683776585/`
- HadesTheGame Reddit support discussion: with `attack at cursor` active, holding left click and
  moving the cursor should make Zagreus aim/follow that cursor direction for bow aim:
  `https://www.reddit.com/r/HadesTheGame/comments/t4d2zo/mouse_aiming_not_working_nor_controller/`

Design conclusion for RIMA:
- RIMA should not continuously rotate walking facing toward the mouse.
- RIMA should rotate combat facing at the instant of attack/skill activation.
- The combat-facing override must not be overwritten by held movement input in the same attack
  startup window.

## Root Cause In RIMA

Before this fix, `PlayerController.FacingDirection` was a single field used by both movement and
combat. `FaceCombatTarget()` set it toward the mouse, but movement input could still be the dominant
visual read because animator/movement-facing logic was not explicitly separated.

There was also a practical player-pref issue: old local `AttackAimMode` data could keep the player
in `CharacterFacing` mode, which intentionally attacks in the movement/last-facing direction. For
the current Hades-like default, old local prefs should migrate once back to cursor aim.

## Code Changes

Files:
- `Assets/Scripts/Player/PlayerController.cs`
- `Assets/Scripts/Player/PlayerAnimator.cs`
- `Assets/Tests/EditMode/PlayerControllerCombatFacingTests.cs`

### PlayerController

Changed facing model from one shared direction to two directions:
- `movementFacingDir`: last movement/WASD facing.
- `combatFacingDir`: short-lived attack/skill facing override.

Public behavior:
- `FacingDirection` now returns `combatFacingDir` while the combat override is active.
- Otherwise it returns `movementFacingDir`.
- `MovementFacingDirection` exposes movement-facing explicitly.
- `HasCombatFacingOverride` tells presentation code that combat-facing is currently authoritative.
- `FaceCombatDirection(Vector2 direction, float lockDuration = -1f)` was added for deterministic
  combat-facing override.
- `FaceCombatTarget()` now computes cursor/movement target, then routes through
  `FaceCombatDirection()`.
- `combatFacingLockDuration` increased from `0.22` to `0.35` seconds so basic attack startup is
  covered more reliably.

Preference migration:
- Added one-time key `AttackAimModeCursorDefault_20260503`.
- On first run after this fix, old local `AttackAimMode` is set to `TowardsMouse`.
- After that migration, the settings toggle can still change the mode normally.

### PlayerAnimator

Changed animator-facing update:
- If `controller.HasCombatFacingOverride` is true, facing is applied immediately.
- Movement turn delay/hysteresis is used only for normal movement-facing changes.

This prevents the combat turn from being delayed or visually overridden by held movement input.

### Tests

Added `PlayerControllerCombatFacingTests`:
- `CombatFacingOverrideWinsOverMovementFacingWhileLocked`
- `MovementFacingReturnsAfterCombatFacingLockExpires`

## Verification

Completed:
- Script validation clean for:
  - `PlayerController.cs`
  - `PlayerAnimator.cs`
  - `PlayerControllerCombatFacingTests.cs`
- Unity console after compile showed no game compile errors.
- Unity `execute_code` smoke verified the core facing rule without the test runner:
  - during combat override, `FacingDirection` stayed left even after movement-facing was set right
  - after override expiry, `FacingDirection` returned right
  - returned result: `during=(-1,00,0,00) after=(1,00,0,00) overrideAfter=False`

Not completed:
- EditMode test runner failed to initialize repeatedly with:
  `Test job failed to initialize (tests did not start within timeout)`
- The same initialization failure also occurred on an older existing test group, so this appears
  to be Unity/MCP test-runner state rather than a failure from the new tests.

## Follow-up Hotfix After Manual FAIL

Manual Play Mode check failed: holding movement while attacking toward cursor still did not show
the expected turn.

Root cause:
- Current wave-1 Animator controllers have directional `idle_*` states, but no production run
  states yet.
- Their direction transitions are gated by `Speed < 0.5`.
- `PlayerAnimator` correctly changed `DirX/DirY` during combat override, but it still sent
  `Speed=1` while movement input was held.
- Result: the controller stayed in the current moving/previous state and did not visually switch
  to the cursor-facing idle/startup pose.

Fix:
- `PlayerAnimator` now treats active combat-facing override as visual startup, not movement:
  `showMovement = controller.IsMoving && !controller.HasCombatFacingOverride`.
- During attack/skill combat override, Animator `Speed` is sent as `0`, allowing existing
  directional idle clips to switch immediately toward cursor.
- After the short override ends, held movement resumes normal visual movement speed.
- Follow-up correction: when combat-facing override ends while movement is still held,
  `PlayerAnimator` now returns to movement-facing immediately instead of applying movement turn
  delay to the return.
- `combatFacingLockDuration` reduced from `0.35` to `0.18` seconds so cursor-facing is a short
  attack startup cue, not a lingering movement-facing replacement.

Regression:
- Added `PlayerAnimatorDirectionTests.WarbladeController_CombatFacingNeedsIdleSpeedToChangeDirectionWithoutRunStates`.
- Added `PlayerAnimatorDirectionTests.CombatFacing_ReturnsToHeldMovementFacingWhenOverrideEnds`.
- Unity test runner still failed to initialize before executing tests.
- Independent `execute_code` smoke confirmed the Animator controller behavior:
  `Speed=1` stayed on `warblade_idle_south`; `Speed=0` with `DirX=-1, DirY=-1` switched to
  `warblade_idle_SW`.
- Independent `execute_code` smoke after the follow-up correction confirmed:
  `before speed=1 dir=(1,-1); during speed=0 dir=(-1,-1) lock=0.18 override=True; after speed=1 dir=(1,-1) override=False`.

## Next Manual Check

In Play Mode:
1. Make sure Attack target is `IMLEC` / cursor in ESC settings.
2. Hold `D` to walk right.
3. Move mouse left of the player.
4. Press LMB.
5. Expected: player turns toward the mouse/left for the attack startup, then may return to right
   after the short combat-facing lock if `D` is still held.
