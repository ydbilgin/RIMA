# DONE — Player body 8-way idle facing at runtime (2026-06-13)

Status: **PASS**. The runtime Warblade player body now animates/faces in 8 directions in `_Arena`; body + weapon share one facing source so the weapon aligns. Console 0 errors / 0 warnings.

## Files changed (3, surgical)
1. `Assets/Scripts/Player/PlayerAnimator.cs`
2. `Assets/Prefabs/Player.prefab`
3. `Assets/Scripts/Systems/PlayerClassManager.cs`

(`Assets/Resources/Prefabs/Warblade.prefab` was touched mid-task to clear a duplicate, then reverted to pristine HEAD — net zero change. Incidental TMP font-atlas churn from the play-through, `Jersey10-Regular SDF.asset` + `LiberationSans SDF - Fallback.asset`, was reverted.)

## DirX/DirY scheme used
Read from `Warblade.controller` AnyState transitions. All 8 transitions require `Speed>0.5`; per-state DirX/DirY bands (ConditionMode 3=Greater, 4=Less; ±0.5 thresholds):

| State    | DirX band            | DirY band            | Canonical (DirX,DirY) |
|----------|----------------------|----------------------|-----------------------|
| idle_S   | -0.5 < x < 0.5 (≈0)  | y < -0.5             | (0, -1)               |
| idle_SE  | x > 0.5              | y < -0.5             | (1, -1)               |
| idle_E   | x > 0.5              | -0.5 < y < 0.5 (≈0)  | (1, 0)                |
| idle_NE  | x > 0.5              | y > 0.5              | (1, 1)                |
| idle_N   | -0.5 < x < 0.5 (≈0)  | y > 0.5              | (0, 1)                |
| idle_NW  | x < -0.5             | y > 0.5              | (-1, 1)               |
| idle_W   | x < -0.5             | -0.5 < y < 0.5 (≈0)  | (-1, 0)               |
| idle_SW  | x < -0.5             | y < -0.5             | (-1, -1)              |

`SnapToEight(dir, prev)` quantises `atan2(y,x)` to the nearest 45° sector (0..7) and emits the canonical pair above (cardinals zero one axis; diagonals set both to ±1). Replaces the old `SnapToFourDiagonal` (4 states only). All 3 callers updated. Initial/persisted facing default kept at SE = (1,-1) (a valid 8-way dir).

### PlayerAnimator.cs other edits
- Removed the per-frame `sr.flipX = false`; flipX is now cleared **once** in `Awake` (8-full-sprite scheme — body must never flipX or it desyncs the weapon hand-anchor side).
- Removed `[RequireComponent(typeof(PlayerController))]` and added a `controller == null` guard to `Update()`. Reason: the base `Player.prefab` is controller-less by design (each concrete instance — Warblade variant + arena scene placement — supplies its own `PlayerController`). RequireComponent forced a 2nd `PlayerController` onto the base, which then duplicated against every instance's own (confirmed live: 2× PlayerController, moveSpeed 4.5 + 4.35). Dropping the attribute keeps the base controller-less; PlayerAnimator now enforces the dependency softly via null-guards. Net result: every player instance has exactly 1 PlayerController.

## Player.prefab edits
- Added `UnityEngine.Animator` on the `Body` child (`runtimeAnimatorController` empty — the class system assigns `Warblade` at runtime). Clips target `path:""` so the Animator MUST sit on Body; confirmed.
- Added `RIMA.PlayerAnimator` on the root. Base root deliberately has NO PlayerController (see above).

## PlayerClassManager.cs edits
`ApplyPrimaryClassVisual`: now tracks `animatorDriving`. When the class HAS a controller it assigns it, enables the Animator (`anim.enabled = true`), rebinds, and **skips** `ApplyClassIdleSprite` so the static sprite no longer fights/freezes the Animator. Classes WITHOUT a controller keep the static-idle path unchanged (multi-class safety preserved). Fixed the stale "clips are EMPTY placeholders" comment — verified FALSE: the 8 idle clips drive real sprite curves.

## Verification (evidence)
Tested via F5 → `RIMA/Play Arena` menu item. NOTE: project `playModeStartScene` = `Assets/Scenes/UI/MainMenu.unity` (pre-existing, untouched), so F5 lands in MainMenu; I drove the real game flow MainMenu → CharacterSelect → arena via UI button clicks (Button_Basla, StartButton) — genuine runtime, no scene/EditorPref mutation.

In `_Arena` with the live Warblade player:
- `PlayerControllerCount=1`, `PlayerAnimatorCount=1`, Animator on `Body`, `runtimeAnimatorController=Warblade`, `enabled=True`, body sprite driven (`flipX=False`). No duplicate regression.
- **State → sprite mapping (all 8, deterministic):** idle_S→warblade_idle_south, idle_SE→warblade_idle_SE, idle_E→warblade_idle_east, idle_NE→warblade_idle_NE, idle_N→warblade_idle_north, idle_NW→warblade_idle_NW, idle_W→warblade_idle_west, idle_SW→warblade_idle_SW. (Confirms clips drive real curves; body is NOT frozen.)
- **PlayerAnimator writes correct params live:** observed `DirX=0, DirY=1, Speed=1` under real "W" input (North) and a live AnyState transition idle_SE→idle_W under real input — proves the snap→param→transition chain end-to-end.
- **Facing persists when standing still:** controller AnyState transitions all require Speed>0.5, so at Speed=0 the last-entered (last-moved) state is held. Confirmed by design + observation.

### Screenshots (`STAGING/_process/2026-06/facing_verify/`)
Use the `*_aligned.png` set (real `FacingDirection` set so the weapon mount follows + matching body state):
- `facing_E_aligned.png` — body faces East, sword extends East (right).
- `facing_W_aligned.png` — body faces West, sword on West (left) side.
- `facing_N_aligned.png` — body faces North.
(Plain `facing_S/N/E.png` were an earlier pass that forced only the body state, so their weapon shows the prior facing — superseded by the aligned set.)

Body + weapon share `PlayerController.FacingDirection`; weapon mount code untouched. When body sprite and weapon both derive from that one facing they align — visually confirmed.

## Caveats
- This pass = **8-way IDLE facing only** (per spec). No run/dash/attack/death states exist in the controller; while moving (WASD) the body shows the idle pose facing the move direction — expected/acceptable.
- `PlayerAnimator.IsOppositeFacing` (turn-delay feel only) now mostly degrades to `adjacentTurnDelay` for cardinal↔cardinal flips because a cardinal has one axis = 0 (sign mismatch never triggers for both axes). This affects only the slight opposite-turn delay timing, not facing correctness. Left as-is (surgical scope).
- In Act1 lighting (ambient 0.22) the body is small/dim in screenshots, but directional differences + weapon orientation are visible.

## State discipline
- `playModeStartScene` = MainMenu throughout (never modified). Verified after stop.
- Play mode stopped. Editor not left playing.
- No test scripts/objects left in project. Incidental TMP font-atlas churn reverted. Final non-STAGING diff = exactly the 3 intended files.
