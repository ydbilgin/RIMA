# Sprint 13 Impl Review Request
**Date:** 2026-05-17 S87 night
**Reviewer:** Codex (GPT-5.5, profile auto-select)
**Spec under review:** `STAGING/codex_brush_sprint13_production_hardening.md` v1.1
**Impl commit:** `cb4303b [Brush Sprint 13] Production Hardening LIVE (321/321 PASS — Brush V1 SHIP-READY)`
**Owner:** Opus orchestrator

---

## 1. Scope

Sprint 13 spec review (v1.0) returned FAIL with 4 P0 + 5 P1 blockers. v1.1 incorporated resolutions and Opus implemented in commit `cb4303b`. Test suite 321/321 PASS.

This review verifies the LIVE implementation against the v1.1 spec.

---

## 2. Files to inspect (LIVE in commit cb4303b)

### Modified (additive only)
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` — `walkableGrid` bool[] + `IsWalkable(Vector2Int)` method
- `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs` — `variantSprites` Sprite[] + `PickVariant` + `PickVariantIndex` + `PickVariantIndexForTile` + static `StableTileSeed`
- `Assets/Scripts/MapDesigner/Props/PropPlacementData.cs` — `variantIndex` int + `RotateClockwise()`
- `Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs` — `Validate(propDef, tilePos, rotationSteps, ...)` overload + rotation-aware `GetSafeFootprint`/`GetRotatedFootprint` + replaced `IsWalkableTile` body with `template.IsWalkable`
- `Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs` — `CurrentRotation` + `RotateClockwise()` + variant pick on click
- `Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs` — R hotkey wire (`OnSceneGUI` KeyDown handler)

### New
- `Assets/Scripts/MapDesigner/Props/PropRegistrySO.cs`
- `Assets/Scripts/MapDesigner/Props/Editor/PropDefinitionPostprocessor.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs`
- `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs`
- `Assets/Editor/MapDesigner/Brush/DependencyReportGenerator.cs`

### Tests (39 new)
- `Assets/Tests/EditMode/Room/RoomTemplateWalkableGridTests.cs`
- `Assets/Tests/EditMode/Props/PropRotationTests.cs`
- `Assets/Tests/EditMode/Props/PropVariantTests.cs`
- `Assets/Tests/EditMode/Props/PropRegistryTests.cs`
- `Assets/Tests/EditMode/Props/BridsonPoissonAutoPlacerTests.cs`
- `Assets/Tests/EditMode/Props/PropColliderTests.cs`
- `Assets/Tests/EditMode/Props/PropSorterTests.cs`
- `Assets/Tests/EditMode/Props/PropRuntimeSpawnerTests.cs`
- `Assets/Tests/EditMode/Brush/DependencyReportGeneratorTests.cs`
- `Assets/Tests/EditMode/Brush/UndoStressTests.cs`

### Assets
- `Assets/Data/Rooms/Library/.gitkeep` (10-room library scaffold)

---

## 3. Verification checklist (each item: ✅ VERIFIED / ⚠️ GAP / ❌ BLOCKER + evidence)

### A. v1.0 → v1.1 resolutions

- **P0-1 PropRegistry runtime path** — RebuildIndex() iterates allProps. Editor uses AssetDatabase + repairs propId. Runtime uses prop.propId directly. Test PropRegistryTests covers both paths via EditorAddProp+RebuildIndex flow.
- **P0-2 PropDefinitionPostprocessor** — AssetPostprocessor OnPostprocessAllAssets iterates importedAssets + movedAssets; LoadAssetAtPath + AssetPathToGUID; SetDirty if propId differs.
- **P0-3 Rotation-aware Validate overload** — 7-arg overload with rotationSteps; 5-arg overload calls 7-arg with 0; PropPlacer uses CurrentRotation; PropsTab wires R hotkey via Event.KeyDown.
- **P0-4 PropRuntimeSpawner** — Spawn() iterates template.props, registry.ResolveGuid, instantiates GameObject with SpriteRenderer + PropSorterRuntime + PropColliderAutoBuilder (if blocksWalkable).
- **P1-1 PropSorterRuntime default Props layer** — ResolveDefaultLayerId() caches SortingLayer.NameToID("Props"). Apply() uses override if non-zero, else default Props.
- **P1-2 Variant seed stable formula** — PropDefinitionSO.StableTileSeed = unchecked(x*73856093 ^ y*19349663). PickVariantIndexForTile wraps. PropVariantTests covers known seed inputs.
- **P1-3 Variant pick text** — spec v1.1 §2.4 updated (verify text refers to PickVariantIndexForTile/StableTileSeed, no "seed-based or random").
- **P1-4 RoomBankSO_Library_v1.asset** — .gitkeep notes that user creates the asset manually after authoring 10 rooms. Spec v1.1 mentions this in §5.
- **P1-5 DependencyReportGenerator tests** — DependencyReportGeneratorTests covers BuildReport title, sections, timestamp.

### B. Karar #143-D/E/K compliance

- **#143-D:** No L3 wall sprite scale changes; PropSorterRuntime doesn't touch SpriteRenderer.transform.localScale.
- **#143-E:** PropSorterRuntime defaults to "Props" sortingLayer (between Accent and Entities per `RimaSortingLayerValidator.cs`).
- **#143-K:** No FeatureMask alpha tolerance changes.

### C. Karar #144 (weaponless body) orthogonality

Sprint 13 touches only brush/props/room layers. No character animation, weapon attachment, body sprite changes.

### D. Additive-only on Sprint 9-12 LIVE

- `RoomTemplateSO`: added `walkableGrid` field + `IsWalkable` method. Existing fields unchanged.
- `PropDefinitionSO`: added `variantSprites` + 4 helper methods. Existing fields unchanged.
- `PropPlacementData`: added `variantIndex` + `RotateClockwise()`. Existing fields unchanged.
- `PropFootprintValidator`: added 7-arg `Validate` overload; existing 5-arg signature delegates to it with rotationSteps=0. `IsWalkableTile` body replaced (signature unchanged).
- `PropPlacer`: added `CurrentRotation` + `RotateClockwise()` + `PickVariantIndex` static. Existing flow unchanged.
- `PropsTab`: added R key handler in `OnSceneGUI`. Existing flow unchanged.

### E. Acceptance criteria (Sprint 13 §7)

- [ ] dotnet build PASS (all RIMA csproj) → 0 errors, only pre-existing warnings.
- [ ] All 24+ Sprint 13 new tests PASS → 39 new tests, 321/321 EditMode PASS.
- [ ] Full EditMode 306+/306+ PASS → 321/321 PASS (exceeds target).
- [ ] Sprint 12 Condition 1 resolved → PropFootprintValidator uses `template.IsWalkable`.
- [ ] All 7 forward-path items LIVE → walkableGrid, Bridson, rotation, variant, Collider2D, PropRegistry, Sorting.
- [ ] Perf smoke under 2s → PropRuntimeSpawnerTests.Spawn_TenRoomLibrary_RendersUnder2Seconds PASS.
- [ ] Undo stress no crash → UndoStressTests both PASS.
- [ ] Dependency report generated → DependencyReportGenerator.GenerateReportMenu writes to STAGING/RIMA_BrushTool_Dependencies.md; tests verify content.
- [ ] 10-room library scaffolding ready → Assets/Data/Rooms/Library/.gitkeep with user-author instructions.
- [ ] NO modification to Sprint 9-12 LIVE method signatures → verified above.
- [ ] Karar #143-D/E/K compliance verified → above.

### F. Spot-check potential issues

1. **PropRegistrySO.RebuildIndex** lazy init — first call from ResolveGuid. Verify no race condition (single-threaded editor, safe).
2. **PropSorterRuntime.cachedDefaultLayerId** static across instances. Verify NameToID returns 0 when layer missing (acceptable fallback to Default).
3. **PropRuntimeSpawner.PickVariantSprite** — variantIndex out-of-range fallback uses placement.tilePosition seed; this matches PropDefinitionSO.PickVariantIndexForTile but duplicates logic. Consider DRY refactor in next sprint.
4. **BridsonPoissonAutoPlacer safety loop** — `bounds.width * bounds.height * 4` iteration cap prevents infinite loop on dense room. Verify reasonable for 20x15 room (1200 iter cap).
5. **PropFootprintValidator.GetSafeFootprint(propDef, rotationSteps)** with negative rotation — normalized via `((r % 4) + 4) % 4`. Tested?

---

## 4. Output format

Write verdict to: `STAGING/codex_review_sprint13_impl_DONE.md`

Top: `VERDICT: PASS | PASS-WITH-CONDITIONS | FAIL`

Then sections A-F with verification status + evidence.

Then if P0/P1 issues, list with file/line + fix recommendation.

Then 1-paragraph "Next action" summary.

---

## 5. Dispatch parameters

- Effort: high
- Background: yes
- Profile: laurethgame (auto-select)
- Estimated: 10-20 min

---

End of Sprint 13 impl review task.
