VERDICT: FAIL

## Verification Matrix

A. Sprint 9-12 LIVE additive-only: ❌ BLOCKER - Existing field/method signatures are mostly additive as scoped, but rotated candidate validation cannot work without changing or overloading `PropFootprintValidator.Validate(...)`; current call has no rotation parameter (`Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs:21`, `Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs:24`).

B. Karar #143-D/E/K compliance: ⚠️ GAP - #143-D and #143-K remain untouched (`FreeformDecalExecutor.cs:232`, `BrushDecorativeExecutorTests.cs:133`), but `PropSorterRuntime` only applies `sortingLayerOverride` when nonzero and otherwise can leave props on `Default`, below the locked `Patch/Detail/Accent/Props/Entities` hierarchy (`STAGING/codex_brush_sprint13_production_hardening.md:287`, `Assets/Editor/RimaSortingLayerValidator.cs:41`).

C. Karar #144 weaponless body orthogonality: ✅ VERIFIED - Sprint 13 spec confines changes to brush, props, room, editor utility, and tests; forbidden list explicitly excludes combat integration and Karar #144 changes (`STAGING/codex_brush_sprint13_production_hardening.md:415`).

D. File path lock: ❌ BLOCKER - §5 omits required support paths: an `AssetPostprocessor` or equivalent propId GUID writer for OQ4, a runtime prop spawn/wiring path for collider/sorter components, and `RoomBankSO_Library_v1.asset` even though acceptance requires it (`STAGING/codex_brush_sprint13_production_hardening.md:430`, `STAGING/codex_brush_sprint13_production_hardening.md:455`, `STAGING/codex_brush_sprint13_production_hardening.md:480`).

E. OQ resolutions: ❌ BLOCKER - OQ1 is consistent with tile distance (`PropFootprintValidator.cs:111`), OQ5 is explicit, but OQ2 lacks a validator API path for current rotation, OQ3 relies on `Vector2Int.GetHashCode()` stability, and OQ4 requires an AssetPostprocessor missing from §5 (`STAGING/codex_brush_sprint13_production_hardening.md:464`, `STAGING/codex_brush_sprint13_production_hardening.md:465`).

F. PropRegistrySO runtime semantics: ❌ BLOCKER - `RebuildIndex()` builds the dictionary only inside `#if UNITY_EDITOR`, so player builds create an empty dictionary and `ResolveGuid()` returns null for valid props (`STAGING/codex_brush_sprint13_production_hardening.md:214`).

G. Test coverage: ⚠️ GAP - The 7 forward-path items have named tests, but Stream B has no dependency-report test and no explicit library scaffold/bank validation beyond the perf smoke placeholder (`STAGING/codex_brush_sprint13_production_hardening.md:407`).

H. Acceptance criteria: ⚠️ GAP - Most criteria are measurable, and `STAGING/` is acceptable for a generated dependency report, but the 10-room scaffold criterion requires `RoomBankSO_Library_v1.asset` while §5 asset files list only `.gitkeep` and `PropRegistry_v1.asset` (`STAGING/codex_brush_sprint13_production_hardening.md:455`, `STAGING/codex_brush_sprint13_production_hardening.md:480`).

I. Memory anchors: ⚠️ GAP - `[[combat-feel-research-combined]]` is listed, but Sprint 13 forbids combat integration and has no combat-side work (`STAGING/codex_brush_sprint13_production_hardening.md:15`, `STAGING/codex_brush_sprint13_production_hardening.md:416`).

## P0 / P1 Issues

### P0 BLOCKER - PropRegistry runtime lookup is empty in player builds

File: `STAGING/codex_brush_sprint13_production_hardening.md:197`

Issue: The proposed `PropRegistrySO.RebuildIndex()` initializes `guidToProp`, but the only population loop is inside `#if UNITY_EDITOR`. In a player build, `ResolveGuid()` calls `RebuildIndex()` and receives an empty dictionary. OQ4 says runtime uses `propDef.propId`, but the code sample does not implement that path.

Fix: Specify Option A: `RebuildIndex()` always iterates `allProps`; in editor it also repairs/validates `propId` from AssetDatabase GUID, and at runtime it indexes `prop.propId`. Add tests that simulate runtime-style lookup without AssetDatabase.

### P0 BLOCKER - Missing propId GUID auto-population file

File: `STAGING/codex_brush_sprint13_production_hardening.md:465`

Issue: OQ4 locks `propId IS the GUID at edit-time (auto-populated via AssetPostprocessor)`, but §5 does not include a `PropDefinitionPostprocessor.cs` or equivalent editor menu validation path. Existing `PropDefinitionSO.propId` is a plain string (`Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs:10`).

Fix: Add `Assets/Scripts/MapDesigner/Props/Editor/PropDefinitionPostprocessor.cs` or `Assets/Editor/MapDesigner/Props/PropDefinitionPostprocessor.cs` to §5, and require it to populate/repair `propId` on import/save.

### P0 BLOCKER - Rotated candidate placement cannot be validated as specified

File: `STAGING/codex_brush_sprint13_production_hardening.md:135`

Issue: Sprint 13 requires `R` hotkey preview rotation and rotation-aware footprint validation, but current `PropFootprintValidator.Validate(...)` takes only `propDef`, `tilePosition`, `template`, `roleMap`, and `existingProps` (`Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs:21`). There is no way to pass the current preview rotation without a signature change or overload.

Fix: Add an overload `Validate(..., int rotationSteps, ...)` and have the old signature call it with `0`. Update `PropPlacer` to maintain `currentRotationSteps`, pass it on hover/click, and store it in `PropPlacementData`.

### P0 BLOCKER - Runtime collider/sorter components are not wired into any prop spawn path

File: `STAGING/codex_brush_sprint13_production_hardening.md:170`

Issue: `PropColliderAutoBuilder` and `PropSorterRuntime` require a `propDef`, but the spec does not define a runtime prop spawner, room loader modification, or PropPlacer-created GameObject wiring. Current `RoomBankRuntimeTester` only instantiates room prefab, player, and enemy, with no prop placement rendering path (`Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs:45`).

Fix: Add a new runtime file such as `Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs`, or explicitly modify `RoomBankRuntimeTester`/room load path to resolve placements via `PropRegistrySO`, create SpriteRenderer GameObjects, assign picked variants, and attach/configure collider and sorter components.

### P1 GAP - Prop sorting layer default can violate #143-E

File: `STAGING/codex_brush_sprint13_production_hardening.md:287`

Issue: `PropDefinitionSO.sortingLayerOverride` defaults to `0` (`Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs:36`), and the spec only sets the SpriteRenderer layer when override is nonzero. A prop can remain on Unity `Default`, bypassing the locked `Props` layer between `Accent` and `Entities`.

Fix: Make `PropSorterRuntime` default to `SortingLayer.NameToID("Props")` when no override is provided, or change the data model to store a default layer name/id with validation against `RimaSortingLayerValidator`.

### P1 GAP - Variant seed rule is not contract-stable

File: `STAGING/codex_brush_sprint13_production_hardening.md:464`

Issue: OQ3 locks `tilePosition.GetHashCode()` for deterministic variant choice. `GetHashCode()` is not a good cross-version serialization contract; the spec needs a deterministic explicit hash formula.

Fix: Use a stable formula, for example `unchecked(tilePosition.x * 73856093 ^ tilePosition.y * 19349663 ^ templateSeed)`, and test the exact expected index for known coordinates.

### P1 GAP - Variant pick text conflicts with OQ3

File: `STAGING/codex_brush_sprint13_production_hardening.md:166`

Issue: §2.4 says `seed-based or random`, while OQ3 locks deterministic per-tile variant selection. That ambiguity can produce non-reproducible room saves.

Fix: Replace `seed-based or random` with the exact deterministic seed expression from the resolved OQ3 fix.

### P1 GAP - File list omits accepted library bank stub

File: `STAGING/codex_brush_sprint13_production_hardening.md:455`

Issue: §7 acceptance requires `.gitkeep` + `PropRegistry_v1.asset` + `RoomBankSO_Library_v1.asset`, but §5 asset files list only `.gitkeep` and `PropRegistry_v1.asset`.

Fix: Add `Assets/Data/Rooms/Library/RoomBankSO_Library_v1.asset` to §5 asset files and define whether it starts empty or references placeholder/manual room assets after user authoring.

### P1 GAP - Stream B dependency report lacks direct test coverage

File: `STAGING/codex_brush_sprint13_production_hardening.md:335`

Issue: Dependency report generation is an acceptance criterion, but §3 has no test that invokes the menu/utility and verifies `STAGING/RIMA_BrushTool_Dependencies.md` content.

Fix: Add an EditMode test for `DependencyReportGenerator` that writes to a temp path or dry-run string builder and verifies SO refs, asmdef edges, and expected path sections.

## Next Action

Do not dispatch Sprint 13 implementation yet. Fix the spec around registry runtime indexing, propId GUID population, rotation-aware validator API, runtime prop spawn wiring, prop sorting layer defaults, deterministic variant hashing, and the §5 asset/test omissions; then re-run this spec review before implementation.
