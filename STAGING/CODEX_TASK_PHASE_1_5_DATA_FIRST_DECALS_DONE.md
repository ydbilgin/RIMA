STATUS: DONE

Phase 1.5 data-first decal migration implemented behind feature flags.

FILES_CREATED:
- Assets/Scripts/MapDesigner/Brush/Data/RoomDecalDataSO.cs
- Assets/Scripts/MapDesigner/Brush/Data/BrushPipelineConfigSO.cs
- Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalDataExecutor.cs
- Assets/Scripts/MapDesigner/Brush/Executors/Editor/ScatterAlongStrokeDataExecutor.cs
- Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs
- Assets/Editor/MapDesigner/PatchAtlasSpriteAtlasBuilder.cs

TESTS_CREATED:
- Assets/Tests/EditMode/Brush/BrushDataFirstExecutorTests.cs
  - FreeformDecalData_Appends_Placement_NotGameObject
  - ScatterAlongStrokeData_Samples_Along_Path
  - Seed_Determinism_Same_Seed_Same_Placements
  - Flag_Off_Uses_Legacy_GameObject_Path
  - Chunk_Renderer_Builds_Mesh_From_SO

UNITY_COMPILE_STATUS:
- Unity refresh/compile through active editor: PASS, no compile errors.
- dotnet build RIMA.Runtime.csproj: PASS, 0 warnings, 0 errors.
- dotnet build RIMA.Brush.Tests.csproj: PASS, 0 errors.
- dotnet build Assembly-CSharp-Editor.csproj: PASS, 0 errors. Existing unrelated warnings remain.
- Shell Unity batchmode was attempted but blocked because the project is already open in another Unity instance.

TEST_STATUS:
- RIMA.Brush.Tests EditMode: 75 total, 75 passed, 0 failed.
- Full EditMode: 333 passed, 0 failed, 1 existing inconclusive prefab-health check.
- Test count delta: 328 baseline -> 333 passing tests.

ROUTER_SCOPE:
- BrushExecutorRouter modification is additive only.
- Legacy registry remains intact.
- DataExecutor alternatives are registered separately and selected only when BrushPipelineConfigSO enables data-first decals/scatter.
- Existing FreeformDecalExecutor and ScatterAlongStrokeExecutor were not modified.

DEVIATIONS:
- Added field-additive references to BrushLayerOperation for pipeline config, active RoomDecalDataSO, and PatchAtlasSO.
- Added field-additive pipelineConfig to MapDesignerBrushPresetSO so Dispatch can select the data path without changing method signatures.
- ScatterAlongStroke routes to data-first when either useDataFirstScatter or useDataFirstDecals is true.
- Performance benchmark snippet omitted.
