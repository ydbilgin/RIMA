# TASK 0 GATE - Director/F2 bootstrap fix

Status: PASS

Changed files:
- Assets/Scripts/UI/DirectorMode.cs
- Assets/Scripts/UI/BuildModeController.cs

Root cause:
- The sceneLoaded-only Director bootstrap was still not reliable in the full editor lifecycle. Repro before fallback: MainMenu -> CharacterSelect -> _Arena produced `DirectorMode.Instance=False` while `BuildModeController.Instance=True`.
- BuildModeController is already a persistent self-bootstrapper from MainMenu, so it can act as a minimal watchdog without touching menu/class/room systems.

Fix:
- DirectorMode now resets static bootstrap state at SubsystemRegistration, hooks sceneLoaded before first scene load, and exposes `EnsureRuntimeInstanceForCurrentScene()`.
- BuildModeController calls that idempotent ensure path each Update and also uses it before entering Build Mode if `DirectorMode.Instance` is missing.
- UI scenes remain guarded: MainMenu and CharacterSelect do not spawn Director.

Runtime verify:
- Set `EditorSceneManager.playModeStartScene=MainMenu`.
- Entered Play via `manage_editor`.
- Invoked `MainMenuController.OnStartClicked()`.
- Invoked active CharacterSelect `StartButton.onClick`.
- In `_Arena`, initial runtime assert after fix:
  `C scene=_Arena; director=True; directorState=Test; buildController=True; buildActive=False; uiOverlay=False; draftActive=True; draftPending=False; roomDirector=True; currentTemplate=combat_large_teardrop_01; timeScale=0`
- Closed the opening kit draft through public `DraftManager.HideDraft()` and resumed gameplay, because the draft intentionally blocks Director/Build input.
- Queued backquote through Unity Input System and let the player loop process it:
  `afterBackquoteLoop director=True; state=Director; backquotePressed=False; timeScale=0`
- Queued F2 through Unity Input System and let the player loop process it:
  `FINAL scene=_Arena; DirectorMode.Instance=True; directorState=Director; BuildModeController.Instance=True; F2BuildActive=True; draftActive=False; roomDirector=True; template=combat_large_teardrop_01; timeScale=0`

Evidence:
- Scene-view screenshot: `STAGING/_process/2026-06/demo_fix_tasks/TASK_0_GATE_scene_view.png`
- Unity console after runtime verify: 0 error/warning entries from `read_console`.
- `git diff --check` on changed files: clean.

Console notes:
- Compile produced existing warnings only: obsolete `FindObjectOfType` / `FindObjectsOfType` in BuildModeController and obsolete TMP word-wrapping calls in DirectorMode.
- No compile errors found in Editor.log for the touched files.

Remaining risk:
- This task verified editor runtime only. Standalone still needs `DEMO_BUILD` / development define coverage before demo build confidence.
