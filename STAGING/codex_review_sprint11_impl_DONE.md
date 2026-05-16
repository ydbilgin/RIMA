# Codex Review - Sprint 11 Implementation DONE

Date: 2026-05-16
Reviewer: Codex laurethgame
Verdict: FAIL

## Verdict Rationale

Sprint 11 compiles and the targeted Composition/Brush test sets pass in the active Unity instance, but the implementation does not fully match the locked spec. The main blockers are API drift in `WallOverlayPainter.PlaceWallSprite_ContextAware`, missing `WangContextResolver` self-cell gating, and an uncleared current worktree scope breach outside the Sprint 11 file lock.

## Commands / Checks Run

- `Get-Content CODEX_TASK_laurethgame.md`
- `Get-Content STAGING/codex_brush_sprint11_natural_engine.md`
- `rg --files -g 'ANTIGRAVITY.md' -g '!Library' -g '!PackageCache'`
  - Result: no `ANTIGRAVITY.md` found.
- `git status --short`
- `dotnet build`
  - Result: failed with MSB1011 because the directory has multiple project files and no solution selected.
- `dotnet test`
  - Result: failed with MSB1011 for the same reason.
- `dotnet build RIMA.Runtime.csproj`
  - Result: PASS, 46 warnings, 0 errors.
- `dotnet build Assembly-CSharp.csproj`
  - First parallel attempt hit Defender file lock; sequential rerun PASS, 3 warnings, 0 errors.
- `dotnet test RIMA.Tests.EditMode.csproj --no-build`
  - Result: exit 0 but no test runner output; Unity-generated project is not a normal VSTest project.
- `dotnet test RIMA.Brush.Tests.csproj --no-build`
  - Result: exit 0 but no test runner output.
- Unity batch test shell command:
  - `Unity.exe -batchmode -quit -projectPath ... -runTests -testPlatform EditMode ...`
  - Result: blocked because another Unity instance already has the project open.
- Active Unity test runner:
  - Composition tests: 15/15 PASS.
  - Brush tests: 65/65 PASS.
  - Full `RIMA.Tests.EditMode`: 197 total, 193 pass, 4 fail. Failures match the listed pre-existing failures.
- `rg` checks for `using UnityEditor`, `transform.localScale`, forbidden terms, Wang encoding, deterministic indicators, and WallOverlay signature.

## EC Matrix

| EC | Criterion | Result | Evidence |
|---|---|---|---|
| EC-1 | Build PASS | PASS with command caveat | Root `dotnet build` is ambiguous; `RIMA.Runtime.csproj` and `Assembly-CSharp.csproj` pass after sequential rerun. |
| EC-2 | Existing Brush V1 + Sprint 10 tests green | PASS | Active Unity runner: `RIMA.Brush.Tests` 65/65 PASS. Full EditMode has 4 known pre-existing failures. |
| EC-3 | 10x10 room role grid | PASS | Targeted Composition tests 15/15 PASS; `Generate_10x10WithNorthDoor_AssignsExpectedRoles` included. |
| EC-4 | DoorSafety radius 3 overrides WallBand | PASS | `DoorSafetyRadius = 3`; Manhattan override applied after WallBand; test PASS. |
| EC-5 | 4x4 minimal no DecoratedEdge | PASS | `Generate_4x4Minimal_NoDecoratedEdge` PASS. |
| EC-6 | L-shape Wang case | PASS by test, contract issue | `ResolveCaseAt_LShapeCorner_ReturnsExpectedCase` PASS, but resolver does not return null when `pos` itself has no tile. |
| EC-7 | Pick matching variant | PASS | `PickVariantForCase_MatchByVariantId_ReturnsExactMatch` PASS. |
| EC-8 | WallBand paints, CleanCenter skips | PASS by test, integration gap | Tests pass, but they pass with `walkableMask: null` and do not verify real Wang-context placement. |
| EC-9 | No runtime non-integer scale | PASS | `rg transform.localScale` in Sprint 11 source returned no matches. |
| EC-10 | Runtime-safe Composition source | PASS | `rg using UnityEditor/#if UNITY_EDITOR` in Composition source returned no matches. |

## Top Blockers

1. `WallOverlayPainter.PlaceWallSprite_ContextAware` does not match the locked §2.5 signature.
   - Spec required: `PlaceWallSprite_ContextAware(Vector2Int pos, CompositionRoleMap compositionMap, WangContextResolver wangResolver, Tilemap walkableMask)`.
   - Implementation adds `List<BrushAssetVariant> candidates` and optional `Tilemap baseTilemap`.
   - This creates API drift from the Sprint 11 contract and forces callers to own candidate selection.

2. `PlaceWallSprite_ContextAware` does not implement the required fallback behavior.
   - Spec says fallback to existing random pick if `compositionMap` or `wangResolver` is null.
   - Implementation returns null if `compositionMap`, `wangResolver`, `candidates`, or candidates count is invalid.

3. `WangContextResolver.ResolveCaseAt` does not honor its own contract/spec note: "Returns null if pos itself is not a wall cell."
   - Current implementation only checks diagonal neighbors and returns `wang_0000` for empty surrounding context.
   - Current tests reinforce the looser behavior by expecting `wang_0000` on an empty tilemap.

4. File-scope verification cannot pass on the current worktree.
   - `git status --porcelain` shows modified/untracked C# files outside the locked Sprint 11 list, including `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs`.
   - This may be previous sprint residue, but the current review cannot certify "files modified outside the 8 listed paths: no".

## Spec Compliance Notes

- §2.5 sealed class: PASS for placement inside existing `public sealed class WallOverlayPainter`; no `partial` keyword found.
- §3 file scope: FAIL in current worktree due multiple modified/untracked C# files outside the listed scope.
- §4 forbidden list: PASS for Sprint 11 files; no forbidden Sprint 12+ terms found except enum value `FocalCluster`, which is explicitly part of `CompositionRole`.
- §6 OQ resolutions: PASS for runtime generation, NE-NW-SE-SW encoding, 1 tile WallBand + 2 tile DecoratedEdge, and DoorSafety radius 3.
- Wang encoding: PASS for order and format. `WangSliceGenerator` emits `wang_{ne}{nw}{se}{sw}` and resolver matches that order.
- Runtime safety: PASS for Composition files; no `UnityEditor` references.

## Sprint 12 Forward-Compat Notes

- `CompositionRoleMap.GetRoleAt(pos)` is suitable for props gating once the API drift above is resolved.
- Before Sprint 12 props consume this, add a regression test for resolver null-on-non-wall behavior or explicitly update the spec if interior-cell Wang probing is the intended model.
- Consider moving candidate acquisition into `WallOverlayPainter` or adding a second overload while preserving the exact locked four-argument API.
- Keep `DoorSafety` priority stable for prop placement; it correctly overrides edge and wall roles now.

