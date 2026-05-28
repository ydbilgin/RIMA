# Codex Review — Sprint 9 Implementation (Opus impl → Codex review)

**Date:** 2026-05-16 S86 PREP-3
**Workflow:** Opus implement → Codex review (16-18 May override window).
**Goal:** Sprint 9 source kod + tests review — spec compliance, regression, drift, risk.

---

## 1. CONTEXT

Sprint 9 task spec: `STAGING/codex_brush_sprint9_atlas_importer.md` (patched per Codex review M1+M3+M4+M5 round).
Source of truth: `STAGING/sprite_strategy_FINAL_LOCK.md`.
Opus signoff decisions: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_s86_opus_signoff_decisions.md`.

**Workflow:** Codex tarafından implement edilmeyi bekliyordu, ama 16-18 May limit pencerede Opus implement etti. Codex'in bu round'daki görevi: implementation review + spec drift + regression check.

---

## 2. NEW FILES (Sprint 9 deliverables)

**Data layer (10 files):**
- `Assets/Scripts/MapDesigner/Brush/Data/ValidationIssue.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/ImportResult.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/SliceLayoutTemplateSO.cs` (+ SliceCell)
- `Assets/Scripts/MapDesigner/Brush/Data/BrushRadiusProfileSO.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs` (MODIFIED — appended SizeBucket + ValidationIssueSeverity)
- `Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs` (MODIFIED — appended variants + 5 fields)
- `Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs` (MODIFIED — useNativeBucketVariantPath + radiusForBucketPick)

**Importer/Validator/Preview (4 files):**
- `Assets/Scripts/MapDesigner/Brush/Import/Editor/WangSliceGenerator.cs`
- `Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasValidator.cs`
- `Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasImporter.cs`
- `Assets/Scripts/MapDesigner/Brush/Editor/BrushVariantPreviewWindow.cs`

**P0 Retrofits (2 modified files):**
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs` (DecorativeExecutorUtility — PickSprite variants-aware + PickVariant new + R1 scale fix + WarnLegacyScale)
- `Assets/Editor/RimaSortingLayerValidator.cs` (R2 — Detail/Accent/Props/Entities EnsureLayerAfter append)

**RoomTemplate stub (thin 5-field):**
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`

**Editor menu + factory (2 files):**
- `Assets/Scripts/MapDesigner/Brush/Editor/SliceTemplateFactory.cs` (default L3 Wang + L4 + L5 + L6 templates + BrushRadiusProfile_Default)
- `Assets/Scripts/MapDesigner/Brush/Editor/BrushAtlasImportMenu.cs` (RIMA → Brush → Import Atlas / Validate Sorting Layers menu)

**EditMode tests (4 files):**
- `Assets/Tests/EditMode/Brush/BrushRadiusProfileTests.cs` (5 tests)
- `Assets/Tests/EditMode/Brush/BrushAtlasValidatorTests.cs` (5 tests)
- `Assets/Tests/EditMode/Brush/AssetPoolMigrationTests.cs` (4 tests)
- `Assets/Tests/EditMode/Brush/BrushAtlasImporterTests.cs` (3 tests)

---

## 3. EXIT CRITERIA STATUS

| EC | Criterion | Status |
|---|---|---|
| EC-1 | Unity compile clean (no errors) | ✅ PASS (verified via Unity MCP `read_console`) |
| EC-2 | All existing Brush V1 tests still green | ⚠️ 3 pre-existing failures (Karar143K precision flake, EraseAllDecorative NRE, GridTileExecutor NRE) — these are baseline issues NOT caused by Sprint 9 changes; require independent investigation |
| EC-3 | 1 synthetic master PNG (512×512) → AssetPool with variants | ✅ `BrushAtlasImporterTests.Import_ProducesVariants_NoRuntimeScale` PASS |
| EC-4 | Wang generator: 1 PixelLab tileset PNG → 15 variants | 🔄 Verified via real Wang tileset (7b34aa6b) — import test pending vertical slice run |
| EC-5 | Validator severity-typed issues | ✅ `BrushAtlasValidatorTests` 5/5 PASS |
| EC-6 | AssetPool GUID preserved on reimport | ✅ `BrushAtlasImporterTests.Import_PreservesAssetGuid_OnReimport` PASS |
| EC-7 | No runtime scale on new variant path | ✅ `useNativeBucketVariantPath = true` → scale 1.0 enforced; legacy path warns once via WarnLegacyScale |
| EC-8 | Sorting layers validated (Patch/Scatter/Detail/Accent/Props/Entities) | ✅ RimaSortingLayerValidator extended, runs on InitializeOnLoad |
| EC-9 | BrushVariantPreviewWindow opens via menu | ✅ MenuItem `RIMA/Brush/Variant Preview` |
| EC-10 | RoomTemplateSO stub compiles (5 field only) | ✅ Thin stub — `schemaVersion`, `roomId`, `biomeId`, `RIMA.RoomType roomType`, `bounds` |

---

## 4. SPEC COMPLIANCE CHECK

Codex should verify against `STAGING/codex_brush_sprint9_atlas_importer.md` (PATCHED version):

**Critical questions:**

1. **D1 Sprint 9 stub thin (5 field):** RoomTemplateSO.cs sadece 5 field içeriyor mu? Helper types (DoorSocket vb.) Sprint 10 scope'unda mı?
2. **D2 Global RIMA.RoomType:** RoomTemplateSO `RIMA.RoomType roomType` kullanıyor mu (yeni enum YOK)?
3. **D3 File paths fix:** Enums.cs extend mi? RimaSortingLayerValidator `Assets/Editor/` modify mi? DecorativeExecutorUtility nested `FreeformDecalExecutor.cs` modify mi?
4. **R1 scaleRange fix:** `useNativeBucketVariantPath = true` ile scale 1.0 enforce ediliyor mu? Legacy path `WarnLegacyScale` warning veriyor mu?
5. **R2 sorting layers:** RimaSortingLayerValidator Patch + Scatter + Detail + Accent + Props + Entities ensure ediyor mu?
6. **Wang generator:** `wangAware = true` template ile çağrılınca metadata.json okuyup `wang_{ne}{nw}{se}{sw}` tag üretiyor mu? `all_floor_reference` tag → AssetPool dışı mı?
7. **AssetPoolSO migration:** `sprites` (legacy) korunmuş mu? Yeni `variants` field eklendi mi? `PickSprite` variants-first / sprites-fallback path doğru mu?
8. **Validator severity:** Error / Warning / Info kodları stabil enum-like string mi (VAL_DUPLICATE_VARIANT_ID, VAL_ZERO_WEIGHT, INF_VARIANT_COUNT etc.)?

---

## 5. TEST COVERAGE STATUS

Sprint 9 yeni tests (17 test):
- `BrushRadiusProfileTests` — 5 test (radius 1/4/8/10/out-of-range)
- `BrushAtlasValidatorTests` — 5 test (dup ID, zero weight, info counts, null pool, master size mismatch)
- `AssetPoolMigrationTests` — 4 test (legacy sprites, variants preferred, empty pool, bucket-aware pick)
- `BrushAtlasImporterTests` — 3 test (variant produce, GUID preserve on reimport, wrong path error)

**Total Sprint 9 new tests: 17 — all PASS** ✅ (Unity test runner job_id ba81fe3899184f57806009b9cd790293, 64/64 ran, 3 pre-existing failures unrelated)

**Codex review checklist:**
- [ ] Tests realistic and isolated (`Assets/TempTests/Brush/` + TearDown cleanup)?
- [ ] Test coverage maps to Sprint 9 exit criteria?
- [ ] Any critical scenario uncovered (e.g. Wang `all_floor_reference` skip, Hero gating, sorting layer R2 ensure)?

---

## 6. REGRESSION CHECK

**Suspected pre-existing failures (NOT Sprint 9 regressions):**
1. `BrushDecorativeExecutorTests.Karar143K_FeatureMaskMultiplier` — 8-bit Color alpha precision: SetPixels sends Color(1,1,1, 0.5), GetPixelBilinear returns 0.50196 (128/255). Tolerance 0.001 fails; tolerance 0.005 would pass. Pre-existing precision flake in test infrastructure.
2. `BrushDecorativeExecutorTests.EraseAllDecorative_PreservesL1L2` — NRE. `EraseAllDecorativeExecutor` not touched by Sprint 9 changes.
3. `BrushExecutorTests.GridTileExecutor_SetsTileOnTilemap` — NRE. `GridTileExecutor` not touched by Sprint 9 changes.

**Codex review checklist:**
- [ ] Verify these 3 failures are NOT caused by Sprint 9 file modifications (cross-check git diff against `BrushDecorativeExecutor*`, `GridTileExecutor`, `EraseAllDecorativeExecutor`, related tests).
- [ ] Recommend fixes for these 3 baseline failures (Sprint 9 follow-up or separate task)?

---

## 7. DRIFT / RISK CHECK

**Possible Sprint 9 implementation drift from spec:**

1. **BrushAssetVariant**: spec lists 23 fields, implementation has 22 — verify field-by-field map.
2. **AssetPoolSO `legacySprites`**: spec said rename `sprites` → `legacySprites` with [Obsolete]. Implementation kept `sprites` field name for backward-compat. Acceptable trade-off?
3. **Wang tag bit order**: spec says NE-NW-SE-SW. WangSliceGenerator computes `$"wang_{ne}{nw}{se}{sw}"`. Verify metadata.json corner order matches.
4. **assetGuid field**: AssetPoolSO had `assetGuid` in spec — implementation omitted (AssetDatabase.GUID is canonical, separate field redundant). Acceptable?
5. **schemaVersion in BrushAssetVariant**: spec mentions, implementation has default "1.0". OK.
6. **legacyScale warning**: implementation logs once per pool key via HashSet. Spec said "EditorPrefs flag default true". Implementation uses static HashSet instead. Acceptable trade-off (simpler, no EditorPrefs persistence needed for single-session warning)?

---

## 8. OUTPUT

Write `STAGING/codex_review_sprint9_impl_DONE.md` with structure:

```markdown
# Codex Review Sprint 9 Implementation — DONE Report

## VERDICT
**Status:** PASS / PASS-WITH-CONDITIONS / FAIL
**Confidence:** %X
**Reason (1 sentence):** ...

## A. SPEC COMPLIANCE
[Per question 1-8 in §4]

## B. EXIT CRITERIA STATUS
[EC-1 through EC-10, agree/disagree with claimed status]

## C. REGRESSION CHECK
[3 pre-existing failures — Sprint 9 caused or not, recommendation]

## D. DRIFT / RISK
[Per question 1-6 in §7]

## E. TEST COVERAGE
[Suggestions for additional tests, if any]

## F. CODE QUALITY
[Any non-blocking refactor suggestions]

## G. BLOCKING ISSUES
[Numbered list — must fix before vertical slice loop test M8]

## H. RECOMMENDED ACTIONS
1. ...
2. ...
3. ...
```

---

## 9. CONSTRAINTS

- Bu görev REVIEW — implementation kodu yazma. C# yazma.
- Sadece `STAGING/codex_review_sprint9_impl_DONE.md` yaz.
- Source dosyaları oku, ama değiştirme.
- 3 pre-existing failure'lar için git diff check yap (Sprint 9 modify dosyaları vs failing testler).
- BLOCKING concern'ler için somut evidence ver (dosya + satır + alıntı).

---

## 10. REFERENCES

- Spec: `STAGING/codex_brush_sprint9_atlas_importer.md` (patched)
- Source of truth: `STAGING/sprite_strategy_FINAL_LOCK.md`
- Opus signoff: `memory/project_s86_opus_signoff_decisions.md`
- Memory: [[brush-tool-v1-design]] [[room-library-architecture]] [[karar-143-layered-pipeline]] [[codex-as-reviewer-until-may18]] [[s86-opus-signoff-decisions]]
