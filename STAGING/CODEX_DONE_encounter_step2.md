# CODEX DONE - Encounter Step 2 Runtime

Date: 2026-05-19

## Files created
- `Assets/Scripts/Runtime/Encounter/IEncounterBank.cs`
- `Assets/Scripts/Runtime/Encounter/SubRoomEnemyPlan.cs`
- `Assets/Scripts/Runtime/Encounter/EncounterAssignment.cs`
- `Assets/Scripts/Runtime/Encounter/EncounterBankStub.cs`
- `Assets/Scripts/Runtime/Encounter/IntraEncounterDoorTrigger.cs`
- `Assets/Scripts/Runtime/Encounter/SubRoomSequenceController.cs`
- `Assets/Tests/EditMode/Encounter/SubRoomSequenceControllerTests.cs`

Unity generated matching `.meta` files for the new assets. No new asmdef was added because `Assets/Scripts/RIMA.Runtime.asmdef` already covers `Assets/Scripts/Runtime/Encounter`.

## Files modified
- `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs`
  - Added `EncounterBankStub` / `IEncounterBank` explicit lookup.
  - Added combat/elite encounter bootstrap in `StartRoom`.
  - Added `isEncounterContext`, `isFinalSubRoom`, and `OnEncounterFinalCleared()`.
  - Gated reward pickup, map fragment, and chest spawning behind `(!isEncounterContext || isFinalSubRoom)`.
- `Assets/Scripts/Player/CameraFollow.cs`
  - Added `SetBounds(Vector2 min, Vector2 max)` and `SetBounds(Bounds worldBounds)`.
- `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs`
  - Added `avoidRadius = 1.5f` and the separate-spec TODO.

## Verification
- `dotnet build RIMA.Runtime.csproj`: PASS, 0 errors.
- `dotnet build RIMA.Tests.EditMode.csproj`: PASS, 0 errors.
- Unity refresh/domain reload: PASS, editor ready, `is_compiling=false`, `is_domain_reload_pending=false`.
- Unity targeted EditMode tests: PASS, 2/2.
  - `RIMA.Tests.SubRoomSequenceControllerTests.SubRoomSequenceController_FinalClearTriggersReward`
  - `RIMA.Tests.SubRoomSequenceControllerTests.SubRoomSequenceController_NonFinalClearSkipsReward`
- `read_console`: no compile/runtime error entries after the final targeted run; only MCP/test-run informational lines were returned.
- Grep verify: `Assets/Scripts/Runtime/Encounter` contains no `DungeonGraph.Navigate` and no `OnPlayerEnteredDoor` calls.

## Open issues / deviations
- `MapLayerOrchestrator` currently has no `Paint(RoomTemplateSO)` API, and this dispatch explicitly forbids changing `MapLayerOrchestrator.Paint(...)`. `SubRoomSequenceController` therefore instantiates `RoomTemplateSO.prefabRef` and painted background layers directly.
- The requested tests were added as dedicated EditMode tests under `Assets/Tests/EditMode/Encounter/` because the existing `RoomFlowTests.cs` is a PlayMode scene integration suite.
- A full EditMode suite run was accidentally triggered once and failed on pre-existing MCP `NetworkStream` log assertions in unrelated tests. The required targeted `SubRoomSequenceController_*` tests pass.

## Estimated LOC
- New runtime/test files: approximately 730 lines.
- Existing runtime modifications: `+86 / -3` across `LegacyRuntimeRoomManager.cs`, `CameraFollow.cs`, and `EnemySpawnSocket.cs`.
- Estimated total added/modified LOC for this dispatch: approximately 816.
