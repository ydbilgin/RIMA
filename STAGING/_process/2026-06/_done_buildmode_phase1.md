# DONE — Build Mode Phase 1 (camera-zoom wrapper) 2026-06-13

Spec: `STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md` (sec 4 + Phase 1 row).
Scope implemented: camera + input + tab ONLY. No tile/prop/light placement, no save, no palette.

## Files

### NEW: `Assets/Scripts/UI/BuildModeController.cs` (~270 lines)
Phase-1 controller MonoBehaviour, gated by the same `#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR`
as DirectorMode.
- `Toggle()` / `EnterBuildMode()` / `ExitBuildMode()` / `IsBuildModeActive`.
- Self-bootstraps via `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` + `DontDestroyOnLoad`
  (mirrors DirectorMode.Bootstrap) so `BuildModeController.Instance` is always live and NO scene/
  prefab wiring is required.
- ENTER: captures `{camera.orthographicSize, DirectorMode.State, DirectorMode.ActiveTab}`; disables
  `CameraZoom` + URP `PixelPerfectCamera` + legacy `UnityEngine.U2D.PixelPerfectCamera` (resolved by
  FullName, copied verbatim from ChamberSelect) + `RIMA.CameraSystem.CameraFollow`; calls
  `DirectorMode.SetState(Director)` (timeScale 0 + player input off via the existing API) and
  `DirectorMode.ShowTab(Build)`; starts an unscaled-time SmoothStep glide of ortho size out to
  `buildOverviewOrthoSize` (default 9).
- EXIT: restores DirectorMode to the captured state+tab, glides ortho size back to the captured
  (already-crisp) size, then `RestoreCameraRig()` re-enables PPC -> CameraFollow (+`SnapToTarget()`)
  -> CameraZoom.
- Free-cam panning (WASD/arrows): NOT re-implemented. While DirectorMode is in the Director state it
  already runs `UpdateFreeCamera` every frame and writes the camera transform, which is exactly the
  reuse the spec asks for. CameraFollow is disabled during build so it does not fight that.
- `OnDestroy` calls `RestoreCameraRig()` as a safety net (never leave the rig stuck in build state).

### EDIT: `Assets/Scripts/UI/DirectorMode.cs` (+6 lines, in `Update()`)
Added a quote-key sibling branch right after the existing backquote branch:
```
if (keyboard != null && keyboard.quoteKey.wasPressedThisFrame && BuildModeController.Instance != null)
{
    BuildModeController.Instance.Toggle();
}
```
Backquote stays the raw Director toggle (unchanged). Nothing else in DirectorMode was touched. No new
public API was needed on DirectorMode — the existing public `State`, `ActiveTab`, `SetState(...)`,
`ShowTab(...)` were sufficient (the task allowed adding a tab-force / state hook, but none was
required because those are already public).

## Verification (Unity, NO play mode)
- `refresh_unity` (force, scripts) compiled clean.
- `read_console` errors: **0**. Warnings from BuildMode files: **0**.
- Edit-time inspection of the loaded `_Arena` scene Main Camera (via execute_code, read-only):
  components = Transform, Camera (orthographic, size 5.668), UniversalAdditionalCameraData ONLY.
  CameraZoom / PixelPerfectCamera / CameraFollow are attached at RUNTIME by
  `RoomRunDirector.cs` (confirmed by grep), not baked in the scene. The controller treats every one
  of those as optional (`!= null` guards), so it is correct whether they exist yet or not.

## How to test in-editor (post-demo)
1. Enter Play Mode in `_Arena` (UNITY_EDITOR satisfies the `#if` gate; RoomRunDirector attaches the
   camera rig). BuildModeController self-bootstraps — no GameObject wiring needed.
2. Press `"` (quote): camera glides OUT to a wider build framing, gameplay pauses (timeScale 0,
   player input off), the Director overlay shows the **Build** tab.
3. WASD / arrows pan the camera (DirectorMode free-cam).
4. Press `"` again: camera glides back to the original framing, PixelPerfectCamera + CameraZoom
   re-enable (crisp, no PPU-64 pop), CameraFollow snaps to the player, DirectorMode returns to the
   previous state+tab.

## Deviations / notes (flagged per ACTIVE RULE 4)
- **CameraZoom.NearestCrispZoom / ApplyPixelPerfect are PRIVATE** (verified) and `CameraZoom.cs` is
  READ-ONLY in this task. The spec/brief say to "land on a crisp ratio via
  CameraZoom.NearestCrispZoom/ApplyPixelPerfect". I do NOT call them directly (cannot, and editing
  CameraZoom is out of scope). Instead I copy the SHIPPED ChamberSelect precedent exactly:
  disable CameraZoom during the glide, then RE-ENABLE it on exit. On re-enable, CameraZoom's own
  Update reclaims ortho-size ownership and (its last target was already a crisp ratio) immediately
  calls ApplyPixelPerfect at that crisp zoom -> zero pop. So the crisp landing is performed BY
  CameraZoom via the exact private methods named, just owned by CameraZoom rather than duplicated.
  This is strictly better than re-implementing the math (single source of truth) and is the verbatim
  ChamberSelect pattern the brief told me to copy. Not a BLOCKED condition; calling out the wording
  gap for the record.
- **Two `CameraFollow` types exist**: obsolete `RIMA.CameraFollow` (`Assets/Scripts/Player/`) and the
  live `RIMA.CameraSystem.CameraFollow` (`Assets/Scripts/Camera/`, the one with `SnapToTarget`). Same
  namespace as this controller resolves to the obsolete one, so I fully-qualified every reference to
  `RIMA.CameraSystem.CameraFollow` (same fix ChamberSelect uses). This caused one compile error that
  is now resolved.
- The controller disables `CameraFollow` during build (rather than re-targeting it like ChamberSelect)
  so DirectorMode's free-cam owns the transform; re-enabled + `SnapToTarget()` on exit.
- §3.5 (no rectangular tile math): N/A — Phase 1 places no tiles/props. Respected by omission.
- Did NOT enter Play Mode, did NOT modify any `.unity` / `.prefab` / scene asset.
