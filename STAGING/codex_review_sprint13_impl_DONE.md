VERDICT: FAIL

# Sprint 13 Implementation Review DONE

Spec under review: STAGING/codex_brush_sprint13_production_hardening.md v1.1
Impl commit: cb4303b
Review date: 2026-05-17

## A. v1.0 -> v1.1 Resolutions

- VERIFIED: P0-1 PropRegistry runtime path. PropRegistrySO.RebuildIndex iterates allProps, editor repairs propId from AssetDatabase GUID, runtime falls back to prop.propId. Evidence: Assets/Scripts/MapDesigner/Props/PropRegistrySO.cs:15,22.
- VERIFIED: P0-2 PropDefinitionPostprocessor. OnPostprocessAllAssets handles importedAssets and movedAssets, loads PropDefinitionSO, writes AssetPathToGUID to propId, and SetDirty when changed. Evidence: Assets/Scripts/MapDesigner/Props/Editor/PropDefinitionPostprocessor.cs:9.
- VERIFIED: P0-3 rotation-aware Validate overload. 5-arg Validate delegates to 7-arg overload with rotationSteps=0; PropPlacer passes CurrentRotation; PropsTab handles R KeyDown. Evidence: Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs:21,32; Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs:35; Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs:80.
- BLOCKER: P0-4 runtime spawner is implemented as a callable service, but not wired into the live room runtime path or scene-load flow required by v1.1. PropRuntimeSpawner is not a MonoBehaviour and appears only in its own implementation/tests; RoomBankRuntimeTester instantiates room/player/enemy but has no registry field and no PropRuntimeSpawner invocation. Evidence: Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs:7,17; Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs:45,47,65,80; rg found PropRuntimeSpawner usage only in PropRuntimeSpawner.cs and PropRuntimeSpawnerTests.cs.
- VERIFIED: P1-1 PropSorterRuntime default Props layer. ResolveDefaultLayerId caches SortingLayer.NameToID("Props") and Apply uses override when non-zero. Evidence: Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs:10,32,71.
- VERIFIED: P1-2 stable variant seed in code. StableTileSeed uses unchecked x*73856093 ^ y*19349663; PickVariantIndexForTile wraps PickVariantIndex. Evidence: Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs:57,62.
- GAP: P1-3 spec text is not fully updated. v1.1 header claims the text was replaced, but section 2.4 still says "seed-based or random". Evidence: STAGING/codex_brush_sprint13_production_hardening.md:11,178.
- VERIFIED WITH SPEC CONTRADICTION: P1-4 .gitkeep scaffold exists and says user authors/populates RoomBankSO_Library_v1 after Sprint 13 PASS. Evidence: Assets/Data/Rooms/Library/.gitkeep. However the same spec still references RoomBankSO_Library_v1.asset and PropRegistry_v1.asset stubs in later sections. Evidence: STAGING/codex_brush_sprint13_production_hardening.md:375,467,492.
- VERIFIED: P1-5 DependencyReportGenerator tests cover title, sections, and timestamp. Evidence: Assets/Tests/EditMode/Brush/DependencyReportGeneratorTests.cs.

## B. Karar #143-D/E/K Compliance

- VERIFIED: #143-D. PropSorterRuntime sets sorting layer/order only; no transform.localScale changes in Sprint 13 prop sorter path. Evidence: Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs.
- VERIFIED: #143-E. Props sorting layer is the default in PropSorterRuntime and RimaSortingLayerValidator orders Props between Accent and Entities. Evidence: Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs:10; Assets/Editor/RimaSortingLayerValidator.cs:45,46.
- VERIFIED: #143-K. Sprint 13 files do not touch FeatureMask alpha tolerance logic. rg hits for FeatureMask were outside Sprint 13 touched files.

## C. Karar #144 Orthogonality

- VERIFIED: Sprint 13 touched brush/props/room code and tests only. No character animation, weapon attachment, or body sprite code changes were found in the reviewed commit file list.

## D. Additive-Only on Sprint 9-12 LIVE

- VERIFIED: RoomTemplateSO added walkableGrid and IsWalkable without removing existing fields. Evidence: Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:30,32.
- VERIFIED: PropDefinitionSO added variantSprites and deterministic helper methods without removing existing fields. Evidence: Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs:41,57,62.
- VERIFIED: PropPlacementData added variantIndex and RotateClockwise without removing existing fields. Evidence: Assets/Scripts/MapDesigner/Props/PropPlacementData.cs:13,25.
- VERIFIED: PropFootprintValidator kept the existing 5-arg Validate signature and added the rotation-aware overload. Evidence: Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs:21,32.
- VERIFIED: PropPlacer added CurrentRotation/R handling path and deterministic variant index selection. Evidence: Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs:16,19,61,129.
- VERIFIED: PropsTab added scene R hotkey handling in OnSceneGUI. Evidence: Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs:75.

## E. Acceptance Criteria

- GAP: dotnet build all RIMA csproj did not fully pass. With restore enabled, all projects except RIMA.RoomDesigner.Editor.csproj and dependent RIMA.Demo.Editor.csproj passed. The failure is CS0006 for missing local analyzer path C:\Users\ydbil\.vscode\extensions\visualstudiotoolsforunity.vstuc-1.2.1\Analyzers\Microsoft.Unity.Analyzers.dll, not a Sprint 13 compile error.
- VERIFIED: Full EditMode run via live Unity editor succeeded. MCP test job 00d10c81e872445ebb75b02af01aca49 completed 321/321 progress, result state Passed, 0 failed. Summary reported 320 passed, 0 failed, 0 skipped, with one inconclusive PrefabHealth scene lookup.
- VERIFIED: Sprint 13 listed test files contain 38 [Test] methods across the 10 listed files; they were included in the passing full EditMode run. Count command output: 3+5+5+4+6+3+3+4+3+2 = 38.
- VERIFIED: Sprint 12 Condition 1 resolved. PropFootprintValidator.IsWalkableTile now calls template.IsWalkable. Evidence: Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs:177.
- BLOCKER: All 7 forward-path items are not fully live because PropRuntimeSpawner is not integrated with RoomBankRuntimeTester or any scene-load path. Direct spawner tests pass, but runtime invocation is missing.
- VERIFIED: Perf smoke under 2s is covered by PropRuntimeSpawnerTests.Spawn_TenRoomLibrary_RendersUnder2Seconds in EditMode and passed in the full EditMode run. Evidence: Assets/Tests/EditMode/Props/PropRuntimeSpawnerTests.cs:103.
- VERIFIED: Undo stress no crash covered by UndoStressTests and passed in the full EditMode run. Evidence: Assets/Tests/EditMode/Brush/UndoStressTests.cs.
- VERIFIED: Dependency report generated. Menu execution succeeded and STAGING/RIMA_BrushTool_Dependencies.md was written, length 1181 bytes, timestamp 2026-05-16 22:59 UTC.
- VERIFIED: 10-room library scaffold ready via Assets/Data/Rooms/Library/.gitkeep with user-author instructions.
- VERIFIED: No Sprint 9-12 public method signature removal observed in reviewed files.
- VERIFIED: Karar #143-D/E/K compliance verified above.
- GAP: Full PlayMode run via live Unity editor failed 25+ tests because scene _IsoGame is not in the active build profile/shared scene list. This appears pre-existing and outside Sprint 13 prop implementation, but it blocks a clean all-tests statement.
- GAP: Unity batchmode shell test command could not run because another Unity instance already had the project open. Used live Unity MCP test runner instead.

## F. Spot-Check Potential Issues

1. VERIFIED: PropRegistrySO.RebuildIndex lazy init is safe for normal Unity single-threaded editor/runtime access; ResolveGuid calls RebuildIndex when guidToProp is null.
2. VERIFIED: PropSorterRuntime.cachedDefaultLayerId caches the Props layer ID and leaves Default fallback alone when NameToID returns 0.
3. GAP: PropRuntimeSpawner.PickVariantSprite duplicates the stable tile seed/LCG logic from PropDefinitionSO. Behavior matches now, but a future helper refactor would reduce divergence risk.
4. VERIFIED: BridsonPoissonAutoPlacer safety loop uses bounds.width * bounds.height * 4; for a 20x15 room this is 1200 iterations.
5. VERIFIED: GetSafeFootprint normalizes negative rotations with ((rotationSteps % 4) + 4) % 4. Existing tests cover 90/270; no explicit negative rotation test found.

## P0/P1 Issues

### P0: Runtime prop spawning is not live-wired

Evidence:
- Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs:7 defines a plain sealed class, not a MonoBehaviour/scene-load component.
- Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs:17 exposes Spawn(template, registry, parent, tileSize) but requires an external caller.
- Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs:45-80 instantiates room/player/enemy only; no PropRegistrySO field and no PropRuntimeSpawner call.
- rg found PropRuntimeSpawner references only in PropRuntimeSpawner.cs and PropRuntimeSpawnerTests.cs.

Fix recommendation: Add the v1.1 runtime integration: either make PropRuntimeSpawner a MonoBehaviour with serialized PropRegistrySO and scene-load/room-load entrypoint, or add registry/spawner wiring to RoomBankRuntimeTester immediately after room prefab instantiate. Add a test that RunTest() on a template with props produces prop GameObjects through the live runtime path, not by calling PropRuntimeSpawner directly.

### P1: v1.1 spec text still contains the old variant wording

Evidence:
- STAGING/codex_brush_sprint13_production_hardening.md:11 claims the text was replaced.
- STAGING/codex_brush_sprint13_production_hardening.md:178 still says `variantIndex = (seed-based or random) % variantSprites.Length`.

Fix recommendation: Replace the remaining text in section 2.4 with the implemented deterministic PickVariantIndexForTile/StableTileSeed rule.

### P1/GAP: Build and PlayMode verification are not clean in this environment

Evidence:
- dotnet build with restore fails only RIMA.RoomDesigner.Editor.csproj and dependent RIMA.Demo.Editor.csproj on missing Microsoft.Unity.Analyzers.dll under the local VS Code extension path.
- PlayMode test job 5df8d61c924e4adb9c723e7e52841a9d failed because _IsoGame is not in the active build profile/shared scene list.

Fix recommendation: Restore/fix the local Visual Studio Tools for Unity analyzer reference or regenerate csproj files, then add _IsoGame to the active build profile/shared scene list before rerunning full build and PlayMode tests.

## Next Action

Do not ship Sprint 13 as PASS yet. Wire PropRuntimeSpawner into the actual runtime room-load path, fix the remaining spec wording, then rerun full EditMode plus the relevant runtime/PlayMode verification after the local analyzer and _IsoGame build profile issues are corrected.
