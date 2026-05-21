FIX 1 - Gate Reference: PASS
- File(s) touched: Assets/Scripts/Core/LargeDungeonMapPainterBase.cs
- Compile: PASS (dotnet build Assembly-CSharp.csproj: 0 errors)
- Notes: RIMA_gate_arch and RIMA_gate_spikes now use a gate-specific null-safe decor loader warning and skip missing sprites gracefully.

FIX 2 - Test Path Migration: PASS
- File touched: Assets/Tests/EditMode/Brush/BrushDataTests.cs
- Test run output: BrushData 8 pass / 0 fail (Unity Editor test runner); dotnet test RIMA.Tests.EditMode.csproj --filter BrushData exited 0 but emitted no test counts.
- Notes: Act1 floor fixture paths now use Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_tile_01-03.png for the JSON round-trip, and the shared sample fixture/KnownSpritePath use Act1 granite tiles.

FIX 3 - Wang16 Dead Code: PASS
- Files deleted: Assets/Editor/RebuildAllWangTilesets.cs; Assets/Editor/RebuildAllWangTilesets.cs.meta; Assets/Editor/CreateCornerWangTileSetAsset.cs; Assets/Editor/CreateCornerWangTileSetAsset.cs.meta
- Files modified: Assets/Editor/AutoBiomePresetBuilder.cs; Assets/Tests/EditMode/Editor/WangImporterTests.cs
- Compile: PASS (dotnet build RIMA.Tests.EditMode.csproj: 0 errors; dotnet build RIMA.Brush.Tests.csproj: 0 errors; dotnet build Assembly-CSharp.csproj: 0 errors)
- Tests: Wang tests IGNORED count 3 in source and rebuilt DLL; full EditMode 420 pass / 0 fail. Note: the already-open Unity AppDomain kept a stale loaded test assembly until editor domain reload, so direct Wang fixture execution in that session still reported the old loaded methods as passed.

OVERALL: COMMIT_READY
Git status: staged changes before requested commit
