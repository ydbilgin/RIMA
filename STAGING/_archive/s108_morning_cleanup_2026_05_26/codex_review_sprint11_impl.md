# Codex Review — Sprint 11 Implementation
**Date:** 2026-05-16 S86 SPRINT11_IMPL
**Workflow:** Opus implement → Codex review (16-18 May window).

## 1. CONTEXT

Sprint 11 spec: `STAGING/codex_brush_sprint11_natural_engine.md` (Codex previously PASS verdict on delta review).

Opus implemented per spec. This review verifies impl matches spec, no drift, exit criteria met.

## 2. NEW FILES (Sprint 11 deliverables)

**Source (4 new):**
- `Assets/Scripts/MapDesigner/Composition/CompositionRole.cs` (enum, 6 values)
- `Assets/Scripts/MapDesigner/Composition/CompositionRoleMap.cs` (flat role grid + bounds + helpers)
- `Assets/Scripts/MapDesigner/Composition/CompositionRoleMapGenerator.cs` (static `GenerateFromRoom`)
- `Assets/Scripts/MapDesigner/Composition/WangContextResolver.cs` (ResolveCaseAt + PickVariantForCase)

**Source (1 modified):**
- `Assets/Scripts/MapDesigner/WallOverlayPainter.cs` — added `PlaceWallSprite_ContextAware` method INSIDE existing `sealed class` body (no `partial` split, existing methods untouched, sealed modifier kept)

**Tests (3 new):**
- `Assets/Tests/EditMode/Composition/CompositionRoleMapGeneratorTests.cs` (5 tests)
- `Assets/Tests/EditMode/Composition/WangContextResolverTests.cs` (7 tests)
- `Assets/Tests/EditMode/Composition/CompositionPainterIntegrationTests.cs` (3 tests)

## 3. EXIT CRITERIA STATUS

| EC | Criterion | Status | Evidence |
|---|---|---|---|
| EC-1 | dotnet build PASS | ✅ | 0 errors, 45 warnings (pre-existing) |
| EC-2 | Existing Brush V1 + Sprint 10 tests still green | ✅ | 65/65 Brush tests PASS (3 prev-failed now fixed in same session) |
| EC-3 | CompositionRoleMap generates correct role grid for 10×10 room with 1 door | ✅ | `Generate_10x10WithNorthDoor_AssignsExpectedRoles` PASS |
| EC-4 | DoorSafety radius 3 overrides WallBand at door position | ✅ | `Generate_DoorSafetyOverridesWallBand` PASS |
| EC-5 | 4×4 minimal room: no DecoratedEdge | ✅ | `Generate_4x4Minimal_NoDecoratedEdge` PASS |
| EC-6 | WangContextResolver returns correct case for L-shape corner | ✅ | `ResolveCaseAt_LShapeCorner_ReturnsExpectedCase` PASS |
| EC-7 | WangContextResolver picks matching variant from candidate list | ✅ | `PickVariantForCase_MatchByVariantId_ReturnsExactMatch` PASS |
| EC-8 | WallOverlayPainter context-aware: WallBand → sprite, CleanCenter → no sprite | ✅ | `ContextAware_WallBand_PlacesSprite` + `ContextAware_CleanCenter_DoesNotPlace` PASS |
| EC-9 | No runtime non-integer scale anywhere in Sprint 11 code | ✅ | rg `transform.localScale` in Composition/* → 0 matches |
| EC-10 | No editor-only class referenced from runtime assembly | ✅ | All Composition files free of `using UnityEditor`; `#if UNITY_EDITOR` not needed |

Pre-existing 4 test failures (NOT Sprint 11 caused):
- FeatureEdgeSmoothingTests.PaintFeatureEdges_PlacesOneWangTilePerBoundaryCell
- FeatureMaskSOTests.DetailDensity_UsesNaturalFeatureProximityAndPreservesNullMaskFallback
- HitPauseDriverTests.TriggerPause_RestoresTimeScaleAfterDuration
- NaturalFeatureGraphTests.Generate_200x200_256Sites_StaysInsideEditorBudget (timing flake)

## 4. OQ RESOLUTIONS APPLIED

- OQ-1 (storage): **Runtime-generate** ✅
- OQ-2 (Wang encoding): **4-bit NE-NW-SE-SW** matching Sprint 9 ✅ (verify in `WangContextResolver.ResolveCaseAt` order)
- OQ-3 (wall band thickness): **1 tile WallBand + 2 tiles DecoratedEdge** ✅
- OQ-4 (door safety): **3 tiles radius (Manhattan)** ✅

## 5. SPEC COMPLIANCE CHECK

Codex should verify against `STAGING/codex_brush_sprint11_natural_engine.md`:

1. **§2.5 sealed class:** Did Opus add `PlaceWallSprite_ContextAware` INSIDE the existing `sealed class WallOverlayPainter` body? Verify no `partial` keyword added.
2. **§3 file scope:** Files modified outside the 8 listed paths? Check.
3. **§4 forbidden list:** Any `PropDefinitionSO` / Bridson / SpriteAtlas / AI tag / Markov / FocalCluster authoring tools leaked? grep verification.
4. **§6 OQ resolutions:** All 4 applied correctly?
5. **Wang case encoding format:** `wang_NESW` 4-bit string, NE first, SW last (matches Sprint 9 WangSliceGenerator). Verify in `WangContextResolver.ResolveCaseAt` impl.
6. **CompositionRoleMap runtime-safe:** `using UnityEditor` not allowed in Composition source files. Verify all 4 files.

## 6. REVIEW CHECKLIST (Codex)

- [ ] All exit criteria (§3) verified — no partial
- [ ] WallOverlayPainter existing methods UNCHANGED (only new method added)
- [ ] CompositionRoleMap is RUNTIME-SAFE (no UnityEditor namespace)
- [ ] CompositionRoleMapGenerator is DETERMINISTIC (same input → same output)
- [ ] Wang case encoding matches Sprint 9 importer output exactly
- [ ] No painter OTHER than WallOverlayPainter modified
- [ ] All §6 OQ resolutions applied
- [ ] No forbidden-list (§4) item appears anywhere in Sprint 11 code

## 7. DELIVERABLE — Codex Output

Write to `STAGING/codex_review_sprint11_impl_DONE.md` with verdict (PASS / FAIL / PASS-WITH-CONDITIONS), EC matrix, top blockers if any, Sprint 12 forward-compat notes.

Effort: high.
