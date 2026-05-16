# Codex Review Sprint 9 Implementation - DONE Report

## VERDICT
**Status:** FAIL
**Confidence:** 86%
**Reason (1 sentence):** Source compiles, but the Wang metadata import path and runtime bucket-aware variant placement are not wired to the Sprint 9 contract, so M8 vertical slice should not proceed before fixes.

## A. SPEC COMPLIANCE
1. D1 thin RoomTemplateSO: PASS. `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs:8-12` has exactly `schemaVersion`, `roomId`, `biomeId`, `RIMA.RoomType roomType`, and `RectInt bounds`; no DoorSocket/player/enemy/camera helper types are present.
2. D2 global RoomType: PASS. `RoomTemplateSO.cs:11` uses `RIMA.RoomType`; no local room enum was added.
3. D3 live path fixes: PASS. `Enums.cs:43-45` appends `SizeBucket` and `ValidationIssueSeverity`; `Assets/Editor/RimaSortingLayerValidator.cs` is modified; `DecorativeExecutorUtility` is nested in `FreeformDecalExecutor.cs:23`.
4. R1 scaleRange fix: PARTIAL. `PlaceAt` enforces `scale = 1f` when `useNativeBucketVariantPath` is true (`FreeformDecalExecutor.cs:223-225`) and warns on legacy scale (`164-170`). However `PlaceSingle` still calls `PickSprite` directly (`39`) and never calls `PickVariant`, so radius bucket selection is not used in actual placement.
5. R2 sorting layers: PASS. `RimaSortingLayerValidator.cs:41-46` ensures Patch, Scatter, Detail, Accent, Props, Entities in order.
6. Wang generator: FAIL. `WangSliceGenerator` expects metadata at top-level `tiles` (`WangSliceGenerator.cs:12,25-28`), but the real PixelLab file has `tileset_data.tiles` and no top-level `tiles` (`STAGING/TILESET_OUTPUT/L3_Wang_Floor_Wall_v1/metadata.json`, shell check: `topLevelTiles=False`, `tilesetDataTiles=25`). Current code will fall back to generic `cell_x_y` tags (`WangSliceGenerator.cs:53-67`) instead of `wang_NESW`; `all_floor_reference` skip only works if the metadata branch runs (`BrushAtlasImporter.cs:96-99`).
7. AssetPoolSO migration: PARTIAL. `variants` exists and `PickSprite` prefers variants (`AssetPoolSO.cs:21`, `FreeformDecalExecutor.cs:83-94`), while legacy `sprites` fallback remains (`AssetPoolSO.cs:12`, `FreeformDecalExecutor.cs:96-108`). This preserves existing data, but it does not implement `legacySprites`, `[Obsolete]`, `assetGuid`, `defaultBucketRules`, or `defaultWeights` from the spec.
8. Validator severity: PARTIAL. Severity typed issues exist and core codes are stable strings (`BrushAtlasValidator.cs:16-98`), but several required validator checks/codes are absent: missing bucket/layer, Wang gutter fail, palette drift, AA suspicion, outline drift, low alpha noise, border proximity, tag coverage, memory footprint.

## B. EXIT CRITERIA STATUS
- EC-1 compile clean: AGREE for shell build. `dotnet restore RIMA.Brush.Tests.csproj` then `dotnet build RIMA.Brush.Tests.csproj --no-restore` succeeded with 0 errors and 43 warnings. Notable Sprint 9 warning: `BrushAtlasImporter.cs:78` uses obsolete `TextureImporter.spritesheet`; Wang metadata private fields also warn as never assigned.
- EC-2 existing Brush V1 tests: PARTIAL/UNKNOWN. I did not rerun Unity Test Runner from shell. Git diff shows the named failing tests and Erase/Grid executor files are not modified; only `FreeformDecalExecutor.cs` changed under executors.
- EC-3 synthetic 512x512 import: PARTIAL. Importer supports variant creation, but the committed test uses only 3 cells (`BrushAtlasImporterTests.cs:71-82`), not the spec's 17-25 L4 template range.
- EC-4 Wang generator: DISAGREE/FAIL. Real metadata shape is not parsed; `wang_NESW` tags will not be produced for the verified PixelLab output.
- EC-5 validator severity typed: AGREE for core severity model; incomplete for full required validator matrix.
- EC-6 AssetPool GUID preserved: AGREE. Existing asset is loaded and updated in place (`BrushAtlasImporter.cs:128-139`), and test asserts GUID before/after (`BrushAtlasImporterTests.cs:104-110`).
- EC-7 no runtime scale on new path: AGREE for transform scale, DISAGREE for full bucket path. Scale is 1.0 in `PlaceAt`, but actual placement does not call `PickVariant`.
- EC-8 sorting layers: AGREE for Patch/Scatter/Detail/Accent/Props/Entities; UI/Lights are not included, but Sprint task critical list did not require them.
- EC-9 preview window opens: AGREE for menu (`BrushVariantPreviewWindow.cs:15-18`). Spec UI is incomplete: no AssetDatabase pool dropdown, sample distribution, pivot/footprint overlays, or test brush button.
- EC-10 RoomTemplate stub: AGREE.

## C. REGRESSION CHECK
The 3 claimed baseline failures do not appear caused by Sprint 9 file modifications.

- `Karar143K_FeatureMaskMultiplier`: failing assertion is in `BrushDecorativeExecutorTests.cs:108-133`, and the path uses `Karar143Enforcement.EffectiveDensity`; no Sprint 9 diff touched that enforcement file. Recommended fix: loosen alpha tolerance to at least 0.005 or assert against 128/255 when using RGBA32 texture sampling.
- `EraseAllDecorative_PreservesL1L2`: test invokes `EraseAllDecorativeExecutor` at `BrushDecorativeExecutorTests.cs:167-169`; `git diff` against `EraseAllDecorativeExecutor.cs` is empty. Recommended fix: investigate missing root/tilemap lookup null checks in erase utility as separate baseline task.
- `GridTileExecutor_SetsTileOnTilemap`: test is in `BrushExecutorTests.cs:91`; `git diff` against `GridTileExecutor.cs` is empty. Recommended fix: inspect tilemap discovery/setup assumptions in the baseline test harness.

## D. DRIFT / RISK
1. `BrushAssetVariant`: PASS. Implementation has 22 fields matching FINAL_LOCK section 4, including `schemaVersion = "1.0"` (`BrushAssetVariant.cs:10-31`).
2. `AssetPoolSO.sprites` vs `legacySprites`: ACCEPTABLE BUT DRIFT. Keeping `sprites` is safer for serialized compatibility, but missing `[Obsolete]`/warning means migration intent is less visible.
3. Wang tag bit order: INTENDED PASS, PRACTICAL FAIL. `WangTagFromCorners` builds `wang_{ne}{nw}{se}{sw}` (`WangSliceGenerator.cs:75-81`), but the actual metadata is nested and also contains `transition` corner values that current code maps as floor by omission.
4. `assetGuid` omitted: ACCEPTABLE ONLY IF documented. GUID preservation is achieved by in-place update and tested, but the data model no longer matches the spec field list.
5. `schemaVersion`: PASS. `BrushAssetVariant.cs:31` and importer assignment at `BrushAtlasImporter.cs:123` use `"1.0"`.
6. Legacy scale warning: ACCEPTABLE. Static HashSet once per pool (`FreeformDecalExecutor.cs:162-170`) is simpler than EditorPrefs and adequate for a per-session editor warning.

## E. TEST COVERAGE
Additional tests needed before M8:

1. Real Wang metadata test using `STAGING/TILESET_OUTPUT/L3_Wang_Floor_Wall_v1/metadata.json`, asserting `wang_` tags and excluding `all_floor_reference` from AssetPool.
2. Placement integration test proving `PlaceSingle`/executor path calls bucket-aware variant selection at radius 1 and 10, not just standalone `PickVariant`.
3. No-runtime-scale test should instantiate placement and assert `spawned.transform.localScale == Vector3.one`; current importer test name says no runtime scale but only checks metadata (`BrushAtlasImporterTests.cs:84-89`).
4. Validator tests for missing bucket/target layer, non-point importer settings, and at least one Wang-specific validation path.
5. L4 template import test should use the actual `L4_Organic_512.asset` and assert 17 variants, not a 3-cell ad hoc template.

## F. CODE QUALITY
- Replace obsolete `TextureImporter.spritesheet` with `UnityEditor.U2D.Sprites.ISpriteEditorDataProvider`; shell build warns at `BrushAtlasImporter.cs:78`.
- Promote validator issue codes to constants to avoid string drift.
- `BrushVariantPreviewWindow` is currently a minimal inspector view, not the requested preview tool surface.
- Consider keeping `sprites` for compatibility but add explicit `[Obsolete]` documentation and a migration warning path.

## G. BLOCKING ISSUES
1. Wang importer does not parse real PixelLab metadata. Evidence: code expects `WangMetadata.tiles` at `WangSliceGenerator.cs:12,25-28`; actual metadata shell check returned `topLevelTiles=False` and `tilesetDataTiles=25`. Result: fallback cells `cell_x_y` from `WangSliceGenerator.cs:53-67`, no `wang_NESW`, no reliable `all_floor_reference` skip.
2. Runtime bucket-aware variant selection is not wired into actual placement. Evidence: `PlaceSingle` calls `PickSprite(op.assetPool, stroke.seed, salt)` at `FreeformDecalExecutor.cs:39`; `PickVariant` is only a public helper at `114-160` and is not called by placement. Result: radius 1/4/10 bucket profile does not affect placed sprites.
3. Sprint 9 tests do not cover the two failing paths above. Evidence: importer tests have only 3 non-Wang cases (`BrushAtlasImporterTests.cs:65-131`), and `PickVariant_BucketAware` tests the helper directly (`AssetPoolMigrationTests.cs:53-74`) rather than executor placement.

## H. RECOMMENDED ACTIONS
1. Fix `WangSliceGenerator` to parse `tileset_data.tiles`, handle `lower`/`upper`/`transition` intentionally, dedupe/choose the intended 15 import variants, and preserve `all_floor_reference` outside AssetPool.
2. Wire the new runtime path so `useNativeBucketVariantPath=true` selects a `BrushAssetVariant` through radius profile/bucket weights before placement; keep legacy `PickSprite` fallback for `sprites` only.
3. Add the missing integration tests from section E and rerun Unity EditMode tests before M8.
4. Decide and document the AssetPool drift: either implement `legacySprites`, `assetGuid`, default rules fields or explicitly lock the compatibility trade-off.
5. Expand validator coverage/codes for the missing required checks after the two blockers are fixed.
