# LIVE FLOW PROOF

## Active Scene
- scene: MainMenu -> CharacterSelect/Chamber -> _Arena. Evidence: `Assets/Scripts/UI/MainMenuController.cs:24-27` loads `CharacterSelect`; `Assets/Scripts/UI/CharacterSelectScreen.cs:168-177` adds `ChamberSelectBootstrap`; `Assets/Scripts/UI/ChamberSelectBootstrap.cs:563-573` loads `_Arena` if build settings contain it; `ProjectSettings/EditorBuildSettings.asset:35-37` includes `_Arena`.
- active room manager component: `RoomRunDirector` in `_Arena` with `buildOnStart: 1`. Evidence: `Assets/Scenes/_Arena.unity:3463-3481`, `Assets/Scenes/_Arena.unity:3498`.
- active room builder component: `IsoRoomBuilder` in `_Arena`. Evidence: `Assets/Scenes/_Arena.unity:440-472`.
- disabled/legacy manager components: `_Arena` has no `RoomLoader`, `RuntimeRoomManager`, `DoorTrigger`, or `GateBehavior` references in the targeted scene search; `_IsoGame` still has old `DungeonGraph`/`DoorTrigger`/`GateBehavior` components (`Assets/Scenes/_IsoGame.unity:1005`, `:8907`, `:8990`, `:27426`, `:27509`) but this is not the chamber-to-run presentation scene.
- RoomLoader/RoomSequenceData scene status: no `RoomLoader` or `RoomSequenceData` reference in `MainMenu`, `CharacterSelect`, `_Arena`, or `_IsoGame` scenes; `RoomSequenceData` assets exist only under `Assets/ScriptableObjects/Rooms/Phase1_Room*.asset:14`.

## Room Load Path
file/method chain:
1. `Assets/Scripts/UI/ChamberSelectBootstrap.cs:571-573` selects `_Arena` and calls `SceneManager.LoadScene(targetScene)`.
2. `Assets/Scenes/_Arena.unity:3480-3498` serializes `RoomRunDirector` with `builder` and `buildOnStart: 1`.
3. `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:132-153` calls `BeginRun`, generates `DungeonGraph`, sets `CurrentNodeId`, then calls `BuildCurrentRoom`.
4. `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:183-197` selects a `RoomTemplateSO` from `RoomBankSO.Pick(...)` and calls `builder.Build(template)`.
5. `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs:76-90` is the `RoomTemplateSO` build entry point.

## Portal/Gate Spawn Path
file/method chain:
1. `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:214-222` converts current graph children to room-type choices and calls `builder.BuildExitDoors(doorTypes)`.
2. `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs:721-752` resolves authored exit slots from the current template and creates one exit-door object per choice.
3. `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs:805-831` creates `ExitDoor_{choiceIndex}_{doorType}` with `gateNorthSprite` and a child `Rune`.
4. `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:888-919` attaches/enables `BoxCollider2D` and `RoomRunExitDoorTrigger` to those exit doors.

## Choice/Branch Path
- 1 exit: `RoomTemplateSO.TryResolveExitSlotsForDoorCount` selects center/N (`slots[1]`) for one door; evidence `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:135-139`.
- 2 exit: resolver selects NW + NE when both exist, keeping center empty; evidence `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:141-149`.
- 3 exit: resolver selects NW + N + NE; evidence `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:173-183`.
- choice index carried: `IsoRoomBuilder` names doors with `choiceIndex` (`Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs:805-807`); `RoomRunDirector.ConfigureExitDoors` calls `trigger.Configure(this, i)` (`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:911-918`); `RoomRunExitDoorTrigger.OnTriggerEnter2D` calls `director.TryEnterDoor(choiceIndex)` (`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:1032-1040`); `AdvanceTo` sets `CurrentNodeId = choices[choiceIndex].id` (`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:969-985`).

## Verdict
TEMPLATE_BUILDER_LIVE

## Risk
- blocker: none for "which system is live" in the current presentation path; the live path is `_Arena` + `RoomRunDirector` + `IsoRoomBuilder`, not `RoomLoader`.
- patch: T3 portal visual binding should target `IsoRoomBuilder.BuildExitDoors` / `CreateExitDoorObject` and `_Arena` sprite refs, not `RoomLoader.BuildRoomContent`, `Gate.cs`, `DoorTrigger`, or `GateBehavior`.
