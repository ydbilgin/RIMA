# Sprint 9 — Atlas Importer + Metadata Retrofit + RoomTemplate Stub + 2 P0 Retrofit

**Task type:** Codex implementation task (NORMAL routing) — but **16-18 May 2026 override:** Opus implement → Codex review. Bu doc Codex'in review için kullanacağı kontrat + Opus'un implementation yaparken takip edeceği spec.

**Authority:** `STAGING/sprite_strategy_FINAL_LOCK.md` (Bölüm 4 Data Model + Bölüm 7 Importer + Bölüm 9 P0 Retrofit + Bölüm 10 Sprint 9).

**Estimate:** 1.5-2 day
**Dependencies:** Brush V1 LIVE codebase (Sprint 1-8), AssetPoolSO existing, Brush executors existing.
**Forbidden in this sprint:** RoomBank impl, save/load impl, full Natural Engine, Props Mode (Sprint 10+ scope).

---

## 1. Deliverables (Exhaustive Checklist)

### 1.1 New Enums + Classes (Data Layer) — PATCHED (Opus signoff)

**Files:** `Assets/Scripts/MapDesigner/Brush/Data/`

**KRİTİK:** `TargetLayer` enum zaten `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs:17`'de mevcut (L1-L6). **YENI dosya açma**, mevcut Enums.cs'i **EXTEND** et.

```csharp
// Enums.cs — EXTEND (existing file, do NOT create new)
public enum SizeBucket { Micro, Small, Medium, Large, Hero }                        // NEW append
public enum ValidationIssueSeverity { Error, Warning, Info }                         // NEW append
// public enum TargetLayer { L1, L2, L3, L4, L5, L6 }  — ALREADY EXISTS, do NOT redefine

// ValidationIssue.cs
[Serializable]
public class ValidationIssue
{
    public ValidationIssueSeverity severity;
    public string code;        // stable enum-like string, e.g. "VAL_NON_POINT_FILTER"
    public string message;
    public string subjectId;   // variantId or "atlas" or "pool"
}

// ImportResult.cs
public class ImportResult
{
    public int variantCount;
    public List<ValidationIssue> issues;
    public AssetPoolSO pool;
    public bool Success => !issues.Any(i => i.severity == ValidationIssueSeverity.Error);
}

// BrushAssetVariant.cs — see Bölüm 4 of FINAL_LOCK for full field list
[Serializable]
public class BrushAssetVariant { /* 22 fields, see LOCK doc */ }
```

### 1.2 New ScriptableObjects

**Files:** `Assets/Scripts/MapDesigner/Brush/Data/`

```csharp
// BrushRadiusProfileSO.cs
public class BrushRadiusProfileSO : ScriptableObject
{
    [Serializable] public class RadiusBucketWeight
    {
        public int radius;
        public SizeBucket bucket;
        public float weight;
    }
    public List<RadiusBucketWeight> table; // pre-populated with FINAL_LOCK Bölüm 3 soft-overlap table
    public Dictionary<SizeBucket, float> ResolveWeights(int radius); // helper
}

// SliceLayoutTemplateSO.cs
public class SliceLayoutTemplateSO : ScriptableObject
{
    public string templateName;
    public Vector2Int masterSize;
    public List<SliceCell> cells;
    public int gutterSize;
    public Vector2 defaultPivot;
    public bool wangAware;        // L3 Wang generator flag
}

[Serializable]
public class SliceCell
{
    public string cellName;
    public RectInt rect;
    public SizeBucket bucket;
    public string[] tags;
    public Vector2? pivotOverride;  // Nullable<Vector2>
    public bool heroAllowed;
}
```

### 1.3 AssetPoolSO Extend (BACKWARD COMPATIBLE)

**File:** `Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs`

**Migration policy:**
- Add `List<BrushAssetVariant> variants` field
- Keep `List<Sprite> legacySprites` (renamed from `sprites`, with [Obsolete] attribute and migration warning)
- Importer always writes to `variants`; runtime brush executors read `variants` first, fallback `legacySprites` with warning log
- `assetGuid` field preserved on reimport (GUID stability — Risk #4 mitigation)

```csharp
public class AssetPoolSO : ScriptableObject
{
    public string poolName;
    public AssetCategory category;
    public List<BrushAssetVariant> variants;   // PRIMARY
    [Obsolete("Use variants. legacySprites is V0 backward compat only.")]
    public List<Sprite> legacySprites;
    public Texture2D sourceMasterTexture;
    public SliceLayoutTemplateSO importTemplate;
    public string namespacePrefix;
    public TargetLayer defaultTargetLayer;
    public BucketDefaultRules defaultBucketRules;
    public WeightDefaultRules defaultWeights;
    public bool[] heroLayerWhitelist;
    public string assetGuid; // preserved across reimport
}
```

### 1.4 BrushAtlasImporter (Editor Utility)

**File:** `Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasImporter.cs`

**Signature:**
```csharp
public static class BrushAtlasImporter
{
    public static ImportResult Import(
        string pngPath,
        SliceLayoutTemplateSO template,
        string poolName,
        TargetLayer layer,
        string namespacePrefix);
}
```

**Steps (implementation contract):**
1. Validate PNG exists and is readable. FAIL → Error `VAL_PNG_UNREADABLE`.
2. Load TextureImporter, set:
   - `textureType = TextureImporterType.Sprite`
   - `spriteImportMode = SpriteImportMode.Multiple`
   - `filterMode = FilterMode.Point`
   - `textureCompression = TextureImporterCompression.Uncompressed`
   - `mipmapEnabled = false`
   - `spritePixelsPerUnit = 32`
   - `spritePivot = template.defaultPivot`
3. Build `SpriteMetaData[]` from `template.cells`. If `template.wangAware == true`, call `WangSliceGenerator.GenerateMetadata(template, texture)` (delegated utility — implements Wang 16 case generation from master texture).
4. Apply via `textureImporter.spritesheet = metadata`.
5. `textureImporter.SaveAndReimport()`.
6. `AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate)`.
7. Load resulting `Sprite[]` assets via `AssetDatabase.LoadAllAssetsAtPath`.
8. Build `BrushAssetVariant` per sprite:
   - `sprite` = loaded Sprite
   - `variantId` = `$"{namespacePrefix}{poolName}_{cell.cellName}"` (human-readable, deterministic)
   - `bucket` = `cell.bucket` (or derived from `rect.size` if cell.bucket missing)
   - `targetLayer` = parameter
   - `nativeSize` = rect.size
   - `sourceRect` = cell.rect
   - `pivot` = `cell.pivotOverride ?? template.defaultPivot`
   - `tags` = cell.tags (copied)
   - `heroAllowed` = cell.heroAllowed
   - `footprintRadius` = `Mathf.Max(rect.width, rect.height) * 0.5f / 32f`
   - `weight` = 1.0 (default)
   - `respectsWalkableMask` = true (Karar #143-D)
   - `minDistance`, `edgeBiased`, `wallProximityFactor` from layer-default presets (constants per layer)
   - `allowFlipX`, `allowFlipY` from layer-default
   - `schemaVersion` = "1.0"
9. Find existing `AssetPoolSO` at `Assets/Art/BrushAtlas/Pools/{poolName}.asset`. **If exists, preserve GUID**, in-place update fields. Else create new with deterministic GUID.
10. Set `pool.sourceMasterTexture`, `pool.importTemplate`, `pool.variants = builtList`, `pool.defaultTargetLayer = layer`.
11. `EditorUtility.SetDirty(pool)` + `AssetDatabase.SaveAssets()`.
12. Run `BrushAtlasValidator.Validate(pool, texture, template)` → collect issues.
13. Return `ImportResult { variantCount = builtList.Count, issues = validatorIssues, pool = pool }`.

**Wang generator delegated:** `Assets/Scripts/MapDesigner/Brush/Import/Editor/WangSliceGenerator.cs` — given master texture + Wang-aware template, returns 16 SpriteMetaData entries with `wang_NESW` naming and 32×32 rects (or larger if PixelLab output is larger — auto-detect grid size).

### 1.5 BrushAtlasValidator

**File:** `Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasValidator.cs`

```csharp
public static class BrushAtlasValidator
{
    public static List<ValidationIssue> Validate(
        AssetPoolSO pool,
        Texture2D texture,
        SliceLayoutTemplateSO template);
}
```

**Checks (each emits ValidationIssue with stable code):**

**Error (BLOCK import / fails ImportResult.Success):**
- `VAL_PNG_UNREADABLE` — texture readable=false or null
- `VAL_MASTER_SIZE_MISMATCH` — texture.size != template.masterSize
- `VAL_DUPLICATE_VARIANT_ID` — variantId appears >1 in pool.variants
- `VAL_MISSING_BUCKET` — variant.bucket invalid (out of enum range)
- `VAL_MISSING_TARGET_LAYER` — variant.targetLayer invalid
- `VAL_FILTER_NOT_POINT` — TextureImporter filterMode != Point
- `VAL_COMPRESSION_NOT_NONE` — textureCompression != Uncompressed or mipmapEnabled = true
- `VAL_WANG_GUTTER_FAIL` — only for `template.wangAware`: any Wang tile's border row/column has alpha > threshold at edge pixels (fallback hint message)

**Warning (allow import, advise):**
- `VAL_PALETTE_DRIFT` — sample 16 random pixels per variant, compare distinct color count vs sibling variants; >30% drift → warning
- `VAL_AA_SUSPICION` — non-binary alpha (alpha values between 32 and 224 indicate anti-aliasing) detected on outline pixels
- `VAL_OUTLINE_DRIFT` — outline pixel weight variance > threshold across variants
- `VAL_LOW_ALPHA_NOISE` — interior pixels with alpha 1-15 (likely noise, not intentional transparency)
- `VAL_BORDER_PROXIMITY` — variant rect within 4px of texture edge but still usable
- `VAL_ZERO_WEIGHT` — variant.weight ≤ 0

**Info:**
- `INF_VARIANT_COUNT` — total variant count
- `INF_BUCKET_DISTRIBUTION` — count per bucket (Micro/Small/Medium/Large/Hero)
- `INF_HERO_RATIO` — heroAllowed count / total
- `INF_TAG_COVERAGE` — unique tags count
- `INF_MEMORY_FOOTPRINT` — estimated texture memory (master texture size × 1 + variant overhead)

### 1.6 BrushVariantPreviewWindow

**File:** `Assets/Scripts/MapDesigner/Brush/Editor/BrushVariantPreviewWindow.cs`

EditorWindow with:
- Pool dropdown (all AssetPoolSO via `AssetDatabase.FindAssets("t:AssetPoolSO")`)
- Bucket grid: 5 rows (Micro/Small/Medium/Large/Hero), per row variant cards
- Per card: sprite preview (Texture2D from sprite.texture, sprite.rect), weight, pivot dot overlay, footprint radius circle overlay, tags as chips
- "Sample distribution" button: simulates 50 picks at radius 1/4/7/10, shows histogram (use BrushRadiusProfileSO.ResolveWeights)
- "Test brush" button: opens brush window (existing) with this pool selected

**Acceptance:** Window opens via menu `RIMA → Brush → Variant Preview`, displays all pools, sample distribution button works.

### 1.7 Starter SliceLayoutTemplateSO Assets

**Files:** `Assets/Data/Brush/SliceTemplates/`

Create 4 `.asset` files via `[MenuItem]` utility or CreateAssetMenu:
- `L3_Wang16_Topdown.asset` — `wangAware = true`, masterSize TBD (Wang generator computes from texture), cells empty (Wang generator fills), gutterSize 1, defaultPivot (0.5, 0.5)
- `L4_Organic_512.asset` — masterSize (512, 512), cells = 1 Hero (256×256 @ 0,0) + 4 Medium (128×128 @ top-right quadrant 4 cells) + 8 Small (64×64 @ bottom-left) + 4 Micro (32×32 @ bottom-right corner), gutterSize 16, defaultPivot (0.5, 0.5)
- `L5_Detail_512.asset` — 4 Medium + 12 Small + 16 Micro, gutterSize 8, heroAllowed = FALSE all
- `L6_Accent_512.asset` — 1 Hero (320×320 centered) + 4 Medium 96 (corners) + 6-8 Small 48 (edges), gutterSize 24, heroAllowed = TRUE for Hero only

**Pivot calc helper:** Each cell pivot defaults to template.defaultPivot unless overridden.

### 1.8 RoomTemplateV1 Contract Stub — PATCHED (Opus signoff, thin)

**File:** `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`

**Scope (Sprint 9 = TRUE THIN STUB):** Sadece **5 field**. Helper types (DoorSocket, PlayerSpawnSocket, EnemySpawnSocket, CameraBounds) Sprint 10'a defer.

```csharp
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [CreateAssetMenu(menuName = "RIMA/Room/RoomTemplate", fileName = "RoomTemplate_New")]
    public class RoomTemplateSO : ScriptableObject
    {
        public string schemaVersion = "1.0";
        public string roomId;          // human-readable, e.g. "combat_shatteredkeep_001"
        public string biomeId;         // e.g. "ShatteredKeep"
        public RIMA.RoomType roomType; // **GLOBAL ENUM REUSE** (Combat/Elite/Boss/Merchant/Event/...)
        public RectInt bounds;         // tile-space room bounds
    }
}
```

**RoomType (Opus signoff):** Global `RIMA.RoomType` (RoomType.cs Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor) **kullan**. **YENİ enum YASAK** (Sprite strategy LOCK doc'da yazıldığı gibi Shop→Merchant, Shrine→Event mapping). RoomBank V1 surface: Combat + Elite + Boss + Merchant + Event lists (Chest/Forge/Curse/Corridor V1+ opsiyonel).

**Forbidden in Sprint 9:** RoomBankSO impl, save/load utilities, validator impl, runtime test loader, helper types (DoorSocket vs.) — **bunların TÜMÜ Sprint 10**. Sadece thin 5-field data model.

**Why thin:** Sprint 9 stub'la Sprint 10 full arası type churn'ü önler (Codex review blocker #1 fix). Sprint 10 helper types ekler, Sprint 9 stub'a dokunmaz.

### 1.9 P0 Retrofit R1 — scaleRange Runtime Non-Integer Scale — PATCHED

**Affected files (LIVE PATHS):**
- `Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs` (modify — `scaleRange` field)
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs` (modify — `DecorativeExecutorUtility` nested static class at line 23, `PlaceAt()` at line 129, scale lerp at line 144, localScale at line 157)

**Fix:**
1. Add `bool useNativeBucketVariantPath` field to `BrushLayerOperation` (default `true` for new pools, legacy assets keep `false` until migrated).
2. In `DecorativeExecutorUtility.PlaceAt()` (FreeformDecalExecutor.cs:129):
   - If `useNativeBucketVariantPath = true` → `scale = 1.0f` always, select variant by bucket weights via `BrushRadiusProfileSO.ResolveWeights(radius)`.
   - If `useNativeBucketVariantPath = false` (legacy) → existing `Mathf.Lerp(op.scaleRange.x, op.scaleRange.y, ...)` at line 144 preserved, emit `[Brush V1 LEGACY] scaleRange used for pool {poolName}` warning (once per session via EditorPrefs flag).
3. Add EditorPrefs key `RIMA.Brush.WarnOnLegacyScale` (default `true`) — single warning per session, not per-stroke.
4. Variant pick path delegates to new method `DecorativeExecutorUtility.PickVariant(AssetPoolSO pool, int radius, int seed, int salt)` that uses `BrushAssetVariant` list with bucket filtering — fallback to legacy `PickSprite(pool, seed, salt)` if `pool.variants.Count == 0`.

### 1.10 P0 Retrofit R2 — Sorting Layer Mismatch — PATCHED

**Affected files (LIVE PATHS):**
- `Assets/Editor/RimaSortingLayerValidator.cs` (modify — currently `[InitializeOnLoad]`, only ensures Patch+Scatter at lines 10-11, 36-48)
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs` (modify — `SortingLayerFor()` at line 237-247 already emits Detail/Accent but validator doesn't ensure them)

**Fix:**
1. Extend `RimaSortingLayerValidator` (existing file, do NOT create new):
   - Add `EnsureLayerAfter(sortingLayers, "Detail", "Scatter")`
   - Add `EnsureLayerAfter(sortingLayers, "Accent", "Detail")`
   - Add `EnsureLayerAfter(sortingLayers, "Props", "Accent")`
   - Add `EnsureLayerAfter(sortingLayers, "Entities", "Props")`
   - (UI + Lights as needed; UI usually default top layer, Lights independent for URP 2D)
2. Pattern aynı `EnsureLayerAfter` helper'ı kullanır — existing `CreateUniqueId` hash bazlı stabil ID üretimi korunur (RIMA.SortingLayer.{layerName} hash).
3. Each decorative executor zaten doğru `SortingLayerFor()` çağırıyor (FreeformDecalExecutor.cs:237-247) — sadece layer existence guarantee gerekli (no executor change).
4. New menu: `RIMA → Brush → Validate Sorting Layers` → re-runs `EnsureSortingLayers()` + reports active layers.

### 1.11 EditMode Tests

**Files:** `Assets/Tests/EditMode/Brush/`

Test coverage required:
1. **BrushAtlasImporter_ProducesVariants** — synthetic 512×512 PNG + L4_Organic_512 template → ImportResult.variantCount in expected range (17-25), pool has variants, GUID preserved on reimport.
2. **BrushAtlasImporter_WangGenerator** — synthetic Wang tileset PNG + L3_Wang16 template → 16 variants with `wang_NESW` tags.
3. **BrushAtlasValidator_DetectsNonPointFilter** — set filterMode=Bilinear → validator returns Error VAL_FILTER_NOT_POINT.
4. **BrushAtlasValidator_DetectsDuplicateIDs** — manually inject duplicate variantId → validator returns Error VAL_DUPLICATE_VARIANT_ID.
5. **BrushRadiusProfile_ResolveWeights** — radius 1 → Micro 100%; radius 4 → Small 50% + Medium 50%; radius 10 → Hero 100%.
6. **BucketVariantPick_NoRuntimeScale** — place via new path, assert Transform.localScale == Vector3.one.
7. **AssetPoolSO_BackwardCompat** — old pool with `legacySprites` only → runtime executor reads it with warning log; importer migration utility moves legacySprites → variants.

**Test isolation (Risk #6):** All test fixtures under `Assets/TempTests/Brush/` (clean up in `[TearDown]` via `AssetDatabase.DeleteAsset`).

### 1.12 Menu Integration

`Assets/Scripts/MapDesigner/Brush/Editor/BrushMenu.cs` — add:
- `RIMA → Brush → Import Atlas` (opens import dialog)
- `RIMA → Brush → Variant Preview` (opens BrushVariantPreviewWindow)
- `RIMA → Brush → Validate Sorting Layers`
- Right-click PNG context menu: "Import as Brush Atlas" (via AssetPostprocessor or OnPostprocessAllAssets hook — opens dialog)

---

## 2. File Scope (Codex/Opus dokunabileceği dosyalar) — PATCHED (LIVE PATHS)

**New files:**
- `Assets/Scripts/MapDesigner/Brush/Data/ValidationIssue.cs` (severity-typed issue record)
- `Assets/Scripts/MapDesigner/Brush/Data/ImportResult.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/BrushRadiusProfileSO.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/SliceLayoutTemplateSO.cs` (+ SliceCell partial in same file or sibling)
- `Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasImporter.cs`
- `Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasValidator.cs`
- `Assets/Scripts/MapDesigner/Brush/Import/Editor/WangSliceGenerator.cs`
- `Assets/Scripts/MapDesigner/Brush/Editor/BrushVariantPreviewWindow.cs`
- `Assets/Scripts/MapDesigner/Brush/Editor/BrushMenu.cs` (yeni veya extend if exists)
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` (**thin 5-field only**, NO helper types)
- `Assets/Data/Brush/SliceTemplates/L3_Wang16_Topdown.asset`
- `Assets/Data/Brush/SliceTemplates/L4_Organic_512.asset`
- `Assets/Data/Brush/SliceTemplates/L5_Detail_512.asset`
- `Assets/Data/Brush/SliceTemplates/L6_Accent_512.asset`
- `Assets/Tests/EditMode/Brush/BrushAtlasImporterTests.cs`
- `Assets/Tests/EditMode/Brush/BrushAtlasValidatorTests.cs`
- `Assets/Tests/EditMode/Brush/BrushRadiusProfileTests.cs`
- `Assets/Tests/EditMode/Brush/AssetPoolMigrationTests.cs`

**Modified files (live paths):**
- `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs` (**APPEND** `SizeBucket`, `ValidationIssueSeverity` — TargetLayer already exists)
- `Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs` (extend with `variants` list, GUID preservation, backward compat with `sprites`)
- `Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs` (add `useNativeBucketVariantPath bool`)
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs` (modify nested `DecorativeExecutorUtility` — R1 fix at PlaceAt line 129-157, add PickVariant method)
- `Assets/Editor/RimaSortingLayerValidator.cs` (R2 fix — append Detail/Accent/Props/Entities to EnsureSortingLayers)

**Forbidden:**
- Anything under `Assets/Scripts/MapDesigner/Room/` beyond RoomTemplateSO data stub (Sprint 10 scope)
- Natural Engine / Composition Roles / Bridson Poisson (Sprint 11)
- Props (Sprint 12)
- SpriteAtlas integration (V2)

---

## 3. Exit Criteria (PASS gate)

- [ ] `dotnet build` PASS (or Unity Editor compile clean — no errors)
- [ ] All existing Brush V1 tests still green (37 currently)
- [ ] 1 synthetic master PNG (512×512) imports into L4 AssetPool with 17-25 variants
- [ ] 1 synthetic Wang master imports into L3 AssetPool with 16 variants tagged `wang_NESW`
- [ ] Validator returns severity-typed issues (not raw exceptions)
- [ ] AssetPoolSO GUID preserved on reimport (test asserts)
- [ ] BucketVariantPick test: variant placed with `Transform.localScale == Vector3.one` (no runtime scale)
- [ ] Sorting layer validator: missing layer → Error log + dialog (manual check)
- [ ] BrushVariantPreviewWindow opens via menu, displays pools, sample distribution shows correct buckets per radius
- [ ] RoomTemplateSO stub compiles, no impl beyond data fields
- [ ] No editor-only class referenced from runtime

---

## 4. Open Questions (Opus signoff needed)

1. **AssetPostprocessor vs OnPostprocessAllAssets** for PNG right-click integration — which gives cleaner UX without forcing reimport on every PNG drop?
2. **Wang generator grid auto-detect:** PixelLab `create_topdown_tileset` output may be 4×4 grid of 32×32 (=128×128) or larger if multi-variant per case. Should generator scan transparent rows/columns to find grid?
3. **`legacySprites` removal timeline** — V2 (after Sprint 13) or earlier?
4. **BucketDefaultRules / WeightDefaultRules** — already exist in codebase? If not, separate sub-task or include in this sprint?
5. **Editor dialog vs Console-only for validator failure** — dialog is intrusive but unmissable; console-only respects flow. Recommendation: Error → dialog, Warning/Info → console only.

---

## 5. Codex Review Checklist (review pass için)

- [ ] Veri modeli FINAL_LOCK Bölüm 4 ile bire bir uyumlu mu?
- [ ] BrushAtlasImporter steps complete + correct order?
- [ ] Validator codes stable enum-like strings (not magic strings)?
- [ ] Wang generator delegated cleanly (not embedded in importer)?
- [ ] AssetPoolSO migration safe (no GUID churn, no data loss)?
- [ ] R1 scaleRange fix bypasses runtime scale on new variant path?
- [ ] R2 sorting layer validation extended to all required layers?
- [ ] Tests cover all critical paths + isolated under TempTests/?
- [ ] No Sprint 10+ scope creep?
- [ ] All exit criteria mapped to specific tests or manual checks?

---

## 6. Authoritative References

- `STAGING/sprite_strategy_FINAL_LOCK.md` (TEK YETKİ)
- `STAGING/codex_meta_review.md` (Sprint 9 Deliverables + Risks)
- Memory: [[brush-tool-v1-design]] [[karar-143-layered-pipeline]] [[room-library-architecture]] [[codex-as-reviewer-until-may18]]

---

**Workflow note (16-18 May):** Bu spec normalde Codex implement, Opus review. Şu an override: Opus implement, Codex review. cx_dispatch.py kullanarak Codex'e bu spec'le birlikte "implementation review" görevi verilecek (M7 sonrası). Codex spec'i okur, kodu okur, eşleşme + risk + drift raporu döner. Opus son kararı verir.
