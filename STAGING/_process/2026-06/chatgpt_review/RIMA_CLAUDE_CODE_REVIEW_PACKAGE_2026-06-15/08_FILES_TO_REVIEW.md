# Source Review Manifest

Claude must inspect the current repository versions of these files.

## Primary files

1. `Assets/Scripts/UI/DirectorMode.cs`
   - Bootstrap
   - Update
   - ToggleState
   - SetState
   - ResolveTimeScaleForState
   - player activation
   - F12 interactions through UIManager

2. `Assets/Scripts/UI/BuildModeController.cs`
   - Bootstrap
   - Update / Toggle
   - EnterBuildMode
   - ExitBuildMode
   - StartZoom / ZoomRoutine
   - RestoreCameraRig
   - HideOtherUiCanvases / RestoreOtherUiCanvases
   - working-template lifecycle

3. `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs`
   - SetBuildModeActive
   - SetActiveTool
   - Update
   - HandleKeyboard
   - HandleCursor
   - search-field focus
   - command stack

4. `Assets/Scripts/Skills/DraftManager.cs`
   - Awake / OnEnable / OnDisable / Start
   - secondary-class subscription
   - ShowDraftDelayed
   - HandleRoomCleared
   - TriggerDraftFromFragment
   - ShowDraft / HideDraft
   - ShowOpeningKitDraft
   - ShowDraftWithSkill
   - selection callbacks
   - replace mode
   - pending/active state
   - dependency creation

5. `Assets/Scripts/Core/RewardPickup.cs`
   - Collect
   - ForceCollect
   - DraftThenOpenExit
   - timeout behavior

6. `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`
   - BeginRun
   - OpeningKitDraftSequence
   - RoomClearSequence
   - StopClearSequences
   - reward auto-collect
   - draft timeout constants
   - ForceOpenExitDoorsFromAnyClearedState
   - lifecycle transitions

## Time/modal contracts

7. `Assets/Scripts/UI/UIManager.cs`
   - all state flags
   - ApplyTimeScale
   - scene-load reset
   - Open/Close SkillOffer
   - panic behavior

8. `Assets/Scripts/UI/SkillOfferUI.cs`
   - Show
   - ShowReplaceMode
   - Hide
   - callback storage/replacement
   - confirm animation/coroutines
   - UIManager calls

9. Locate and inspect:
   - `DeathScreenManager`
   - `PauseMenuUI`
   - `SettingsMenuUI`
   - `SkillCodexUI`
   - `CharacterSheetUI`
   - scene-transition manager(s)

## Class/draft dependencies

10. Locate and inspect:
    - `PlayerClassManager`
    - `Warblade_SkillController`
    - `Elementalist_SkillController`
    - `Ranger_SkillController`
    - `Shadowblade_SkillController`
    - `RoninController`
    - `SkillOfferGenerator`
    - `RoomLoader`
    - `RuntimeRoomManager`
    - `RoomClearVictoryTrigger`
    - `Portal`
    - `FragmentDropAnchor`

## Damage contract

11. `Assets/Scripts/Balance/DamageCalculator.cs`
12. `Assets/Scripts/Balance/ClassStatRuntime.cs`
13. `Assets/Scripts/Balance/ClassStatProfile.cs`
14. `Assets/Scripts/Balance/ClassStatDatabase.cs`
15. `DamagePacket` definition and all major call sites
16. Existing damage tests

## Repository-wide searches required

Run searches for:

```text
Time.timeScale =
ShowDraft(
ShowOpeningKitDraft(
ShowDraftWithSkill(
StartCoroutine(ShowDraftDelayed
IsDraftPending
IsDraftActive
OnSecondaryClassSelected +=
OnSecondaryClassSelected -=
BeginRun(
StopAllCoroutines
StopClearSequences
sceneLoaded +=
RuntimeInitializeOnLoadMethod
DontDestroyOnLoad
```

The exact file list may expand based on those searches. Do not constrain the verification to this manifest when a caller proves relevant.
