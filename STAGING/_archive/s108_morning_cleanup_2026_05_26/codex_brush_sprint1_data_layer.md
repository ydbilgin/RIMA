# Codex Task — Sprint 1: Brush Tool Data Layer (V1 minimum)

**Type:** Implementation (scaffolding only, no executors, no UI)
**Effort:** high
**Estimated:** 1 day Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint1_data_layer.md --effort high`
**Output:** Code + .asset files + EditMode tests + CODEX_DONE.md report

---

## 0. MUST READ FIRST

Before any code, read these files for full context:
1. `STAGING/map_designer_unified_brush_design.md` — full design spec, 15 sections + 6 addendum sections (V1/V2 scope, ChatGPT locked answers)
2. `STAGING/codex_unity_safety_review.md` — Unity stability safety contract you must honor
3. `CODEX_DONE.md` — if BG safety review by another Codex run finished, read its augmentations before starting

**If you skip step 0, you will violate the V1 scope and the task will be rejected.**

---

## 1. Context

The RIMA Map Designer is being refactored into a Photoshop-style unified brush tool. This is **Sprint 1 of 8**: data layer scaffolding only. You will NOT write any paint executor, EditorWindow UI, or runtime painter integration in this sprint — those are Sprints 2–8.

**V1 scope locked by user + ChatGPT cross-review (§16 of design spec).** Implementing V2 features (marketplace, namespace prefixing, nested composites, etc.) will cause the task to fail review.

**Existing LIVE code that you must NOT modify:**

- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs`
- `Assets/Scripts/MapDesigner/WallOverlayPainter.cs`
- `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs`
- `Assets/Scripts/MapDesigner/DetailDecalPainter.cs`
- `Assets/Scripts/MapDesigner/AccentPainter.cs`
- `Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs`
- `Assets/Scripts/MapDesigner/VoronoiWaterFeatureGenerator.cs`
- `Assets/Scripts/MapDesigner/VoronoiElevationFeatureGenerator.cs`
- `Assets/Scripts/MapDesigner/FeatureEdgeSmoothingPass.cs`
- `Assets/Scripts/Data/FeatureMaskSO.cs`
- `Assets/Scripts/Data/WallBrushSetSO.cs`
- `Assets/Scripts/Data/NaturalFeatureSettingsSO.cs`
- `Assets/Editor/RimaMapDesignerWindow.cs`

You may **read** them to understand types you reference (e.g., `FeatureMaskSO`), but never edit them.

---

## 2. Scope — Files to Create

### 2.1 Source files under `Assets/Scripts/MapDesigner/Brush/Data/`

All files in namespace `RIMA.MapDesigner.Brush.Data`. Asmdef: `RIMA.Runtime` (these are data types, used at runtime).

#### 2.1.1 `Enums.cs`

```csharp
public enum BrushCategory { Floor, Variation, Wall, Transition, Detail, RiftAccent, Composite }

public enum PaintMode {
    GridTile,
    GridTileRandom,
    FreeformDecal,
    ScatterAlongStroke,
    Stamp,
    CompositeStroke,
    EraseByLayer,
    EraseAllDecorative
}

public enum TargetLayer { L1, L2, L3, L4, L5, L6 }

public enum SnapMode { None, FullGrid32, HalfGrid16, QuarterGrid8 }

public enum AlphaMode { Hard, SoftAlpha8, SoftAlpha16, MultiplyBlend }

public enum AssetCategory {
    Floor, FloorVariation, Wall, WallCorner, Doorway,
    MossPatch, DirtPatch, BiomeBlend,
    Crack, Rubble, Pebble, SmallMoss, DirtChip,
    RiftCrack, RiftCorruption, MagicalMark
}
```

#### 2.1.2 `BrushLayerOperation.cs` (serializable class, NOT ScriptableObject)

```csharp
[Serializable]
public class BrushLayerOperation {
    public TargetLayer targetLayer;
    public AssetPoolSO assetPool;
    [Range(0f, 1f)] public float density = 0.5f;
    [Range(0f, 1f)] public float probability = 1.0f;
    public float minDistance = 32f;
    public Vector2 scaleRange = new Vector2(0.85f, 1.15f);
    public bool allowRotation = true;
    public bool allowFlipX = true;
    public bool allowFlipY = false;
    public float rotationSnapDegrees = 0f;
    public Color tint = Color.white;
    public Vector2 positionJitter = Vector2.zero;
    public int sortingOrderOffset = 0;
    public bool affectsCollision = false;

    // Karar #143 enforcement fields — ALL THREE REQUIRED:
    public bool respectsWalkableMask = true;             // Karar #143-D
    public AnimationCurve wallProximityCurve =           // Karar #143-E
        AnimationCurve.Linear(0f, 1f, 1f, 1f);           // default flat (no edge bias)
    public FeatureMaskSO featureMaskMultiplier;          // Karar #143-K (nullable)
}
```

**Critical:** `BrushLayerOperation` MUST NOT reference any `MapDesignerBrushPresetSO` — that would create nested composites (V2 only).

#### 2.1.3 `AssetPoolSO.cs`

```csharp
[CreateAssetMenu(fileName = "AssetPool_New", menuName = "RIMA/Brush/Asset Pool", order = 100)]
public class AssetPoolSO : ScriptableObject {
    public string poolName;
    public AssetCategory category;
    public List<Sprite> sprites = new();
    public List<float> spriteWeights = new();        // optional, parallel to sprites; empty = uniform
    public List<TileBase> tiles = new();
    public List<GameObject> prefabs = new();         // optional
    public Vector2Int nativeSize = new(64, 64);
    public bool supportsRotation = true;
    public bool supportsFlip = true;
    public bool isSoftEdge = false;
}
```

#### 2.1.4 `MapDesignerBrushPresetSO.cs`

```csharp
[CreateAssetMenu(fileName = "Brush_New", menuName = "RIMA/Brush/Brush Preset", order = 101)]
public class MapDesignerBrushPresetSO : ScriptableObject {
    public string brushName;
    public BrushCategory category;
    public PaintMode paintMode;
    public List<BrushLayerOperation> operations = new();
    public Sprite previewIcon;
    public bool showInPalette = true;
    [TextArea(2, 5)] public string description;
    [Range(-1, 9)] public int hotkeyIndex = -1;     // 1–9 quick select, -1 = none
}
```

#### 2.1.5 `BrushPackSO.cs` (V1 minimum metadata only)

```csharp
[CreateAssetMenu(fileName = "BrushPack_New", menuName = "RIMA/Brush/Brush Pack", order = 102)]
public class BrushPackSO : ScriptableObject {
    public string packName;
    public string version = "1.0";
    public List<MapDesignerBrushPresetSO> brushes = new();
    public List<AssetPoolSO> referencedPools = new();
    public Texture2D coverImage;
    // NO author, license, downloadCount, namespace fields (V2)
}
```

#### 2.1.6 `BiomeSkinSO.cs`

```csharp
[CreateAssetMenu(fileName = "BiomeSkin_New", menuName = "RIMA/Brush/Biome Skin", order = 103)]
public class BiomeSkinSO : ScriptableObject {
    public string skinName;
    public BrushPackSO defaultBrushPack;
    public List<LayerRenderRule> layerRenderRules = new();
    public Color globalTint = Color.white;
    [Range(0f, 1f)] public float ambientLightIntensity = 0.35f;
}

[Serializable]
public class LayerRenderRule {
    public TargetLayer layer;
    public AlphaMode alphaMode = AlphaMode.Hard;
    public Color tint = Color.white;
    public Material overrideMaterial;
    public int sortingOrder;
}
```

**Note:** No render implementation in Sprint 1 — just data. Sprint 8 wires this up.

#### 2.1.7 `BrushJsonSerializer.cs` (V1 minimal round-trip)

Static utility class for JSON export/import. Use Unity `JsonUtility` (NOT Newtonsoft).

Required methods:
- `string ExportBrushToJson(MapDesignerBrushPresetSO brush)`
- `BrushPresetDTO ImportBrushFromJson(string json)` (returns DTO, not SO; caller creates SO if desired)
- `string ExportAssetPoolToJson(AssetPoolSO pool)`
- `AssetPoolDTO ImportAssetPoolFromJson(string json)`
- `string ExportBrushPackToJson(BrushPackSO pack)`
- `BrushPackDTO ImportBrushPackFromJson(string json)`

Define DTOs (data transfer objects) as `[Serializable]` mirrors of each SO with sprite/tile references as project-relative paths (string) instead of object refs.

**V1 RULES:**
- Sprite path resolution helper: `Sprite ResolveSpritePath(string path)` — uses `AssetDatabase.LoadAssetAtPath<Sprite>(path)`, returns null on missing (graceful, no throw).
- `AnimationCurve` serializes via Unity's built-in JsonUtility support — verify keyframes round-trip.
- NO namespace prefixing, NO conflict resolution, NO version migration (V2 features).

### 2.2 Sample asset files under `Assets/Data/Brush/`

#### 2.2.1 `AssetPool_Floor_ShatteredKeep.asset`
- poolName: "Floor_ShatteredKeep"
- category: Floor
- sprites: 5–10 entries pointing to existing assets in `Assets/Art/Tiles/Keep/Floor/` (e.g., `tile_1.png` through `tile_10.png`)
- nativeSize: (32, 32)
- supportsRotation: false
- supportsFlip: false
- isSoftEdge: false

#### 2.2.2 `Brush_CleanStoneFloor.asset`
- brushName: "Clean Stone Floor"
- category: Floor
- paintMode: GridTile
- operations: 1 entry
  - targetLayer: L1
  - assetPool: ref to AssetPool_Floor_ShatteredKeep
  - density: 1.0
  - probability: 1.0
  - respectsWalkableMask: true
  - affectsCollision: true
- description: "Base floor tile, structural"
- hotkeyIndex: 1

#### 2.2.3 `Brush_MossyCorner_Composite.asset`
- brushName: "Mossy Broken Edge"
- category: Composite
- paintMode: CompositeStroke
- operations: 3 entries
  - Op 1: targetLayer=L2, density=0.35, probability=1.0, respectsWalkableMask=true
  - Op 2: targetLayer=L4, density=0.45, probability=0.85, scaleRange=(0.85, 1.15), allowRotation=true, respectsWalkableMask=true, wallProximityCurve = edge-biased curve (sample keyframes: (0,1.0), (1,0.6), (2,0.3), (3,0.1))
  - Op 3: targetLayer=L5, density=0.20, probability=0.6, minDistance=32, respectsWalkableMask=true
- assetPool refs can be empty (just structural proof — Sprint 4/5 will populate)
- description: "Natural moss erosion near walls — L2 dark + L4 patch + L5 cracks"
- hotkeyIndex: 9

### 2.3 EditMode tests under `Assets/Tests/EditMode/Brush/`

`BrushDataTests.cs` — minimum 6 cases:

1. **DefaultValues_BrushLayerOperation** — verify `respectsWalkableMask == true`, `wallProximityCurve` not null with 2 keyframes
2. **AssetPoolSO_RoundTrip_Json** — create pool with 3 sprite refs, export JSON, parse DTO, sprite paths match
3. **MapDesignerBrushPresetSO_Composite_HasThreeOperations** — load `Brush_MossyCorner_Composite.asset`, assert ops count == 3, layers are L2/L4/L5
4. **BrushJsonSerializer_AnimationCurve_RoundTrip** — create curve with 4 keyframes, export JSON, parse, keyframe values match within 0.001 tolerance
5. **SpritePathResolution_Existing_ReturnsSprite** — pass a known asset path, verify returns non-null Sprite
6. **SpritePathResolution_Missing_ReturnsNull** — pass a fake path, verify returns null without throwing
7. **BrushPackSO_PreservesOrder_OnRoundTrip** — pack with 2 brushes A and B, export JSON, import DTO, brushes[0].name == A's name

Test asmdef: ensure `Assets/Tests/EditMode/Brush.asmdef` exists with reference to `RIMA.Runtime` + `nunit.framework`.

---

## 3. V1 EXCLUSIONS (do NOT implement)

The following are V2 features. If you implement any of them, the task fails review:

- Marketplace metadata (author, license, downloadCount, signature)
- Namespace prefixing for pack imports
- Conflict resolution UI
- Nested composite (BrushLayerOperation referring to another BrushPreset)
- Biome graph / sub-region biome painting
- Editor window UI (any IMGUI/UI Toolkit code)
- Paint executors (IBrushExecutor, GridTileExecutor, FreeformDecalExecutor, etc.)
- Soft alpha shader implementation
- BiomeSkin live re-render logic
- LayerRenderRule material wiring
- Brushpack folder format I/O (zip, manifest discovery)
- Wall placement automation
- Asset pack download manager

---

## 4. Acceptance Criteria

A. `dotnet build RIMA.Runtime.csproj` returns **0 errors**. Run this BEFORE reporting done.

B. All new files declare `namespace RIMA.MapDesigner.Brush.Data`.

C. All new SO types have `[CreateAssetMenu]` attribute with `RIMA/Brush/...` menu paths.

D. All public fields visible in Unity Inspector (no `[HideInInspector]`).

E. 3 sample `.asset` files exist under `Assets/Data/Brush/`, all openable in Inspector without null reference exceptions.

F. EditMode tests under `Assets/Tests/EditMode/Brush/`: 6–7 cases, all PASS.

G. JSON round-trip test passes for `Brush_CleanStoneFloor.asset`: export to temp file, import as DTO, field-by-field comparison.

H. `BrushLayerOperation.respectsWalkableMask` defaults to `true`.

I. `BrushLayerOperation.wallProximityCurve` is a non-null `AnimationCurve` with at least 2 keyframes by default.

J. No file modified outside the scope folders:
   - `Assets/Scripts/MapDesigner/Brush/Data/` (create)
   - `Assets/Tests/EditMode/Brush/` (create)
   - `Assets/Data/Brush/` (create)
   - Asmdef files may be created if missing, but existing asmdefs must not be edited

---

## 5. Safety Rules (binding)

These rules apply regardless of what `codex_unity_safety_review.md` says when it returns. If the safety review adds more rules, honor those too.

1. **Read before Edit.** For any file you reference but did not create, `Read` it first before any operation.

2. **No destructive ops.** Never run `git reset --hard`, `git clean -fd`, `rm -rf Library/`, or delete any `.meta` file.

3. **AssetDatabase batch pattern.** When creating multiple `.asset` files:
   ```csharp
   AssetDatabase.StartAssetEditing();
   try {
       // CreateAsset calls here
       AssetDatabase.CreateAsset(obj, path);
       EditorUtility.SetDirty(obj);
   } finally {
       AssetDatabase.StopAssetEditing();
   }
   AssetDatabase.SaveAssets();
   AssetDatabase.Refresh();   // ONE refresh at end
   ```

4. **No mid-batch `AssetDatabase.Refresh()`.**

5. **Single commit boundary.** Do NOT commit. Orchestrator commits after rima-qc review.

6. **Compile gate.** If `dotnet build` fails, halt and report the exact error in CODEX_DONE.md. Do not auto-retry by deleting/regenerating files.

7. **Asmdef integrity.** New scripts go in correct asmdef:
   - `RIMA.Runtime` for data SOs (already exists at `Assets/Scripts/RIMA.Runtime.asmdef`)
   - Test asmdef at `Assets/Tests/EditMode/Brush/Brush.Tests.asmdef` (create if missing, reference `RIMA.Runtime` + `UnityEngine.TestRunner` + `nunit.framework`)

8. **Library/Temp/obj are off-limits.** Never delete or modify these folders.

9. **Branch.** You are running from current branch. Do not switch or create branches. Orchestrator handles branching.

10. **Halt on ambiguity.** If a spec point is unclear, write your best interpretation in code, flag the ambiguity in CODEX_DONE.md "Open Questions" section, and continue. Do NOT pause to ask — this is BG.

---

## 6. Codex Self-Review Checklist

Answer yes/no for each in CODEX_DONE.md before reporting success. Any "no" requires explanation.

1. Did I read `STAGING/map_designer_unified_brush_design.md` (all 21 sections including addendum)?
2. Did I read `STAGING/codex_unity_safety_review.md`?
3. Did I check whether `CODEX_DONE.md` had a recent Unity safety review entry to honor?
4. Are all new files under `Assets/Scripts/MapDesigner/Brush/Data/` in namespace `RIMA.MapDesigner.Brush.Data`?
5. Does `BrushLayerOperation.respectsWalkableMask` default to `true`?
6. Is `BrushLayerOperation.wallProximityCurve` a non-null `AnimationCurve` with ≥2 default keyframes?
7. Does `BrushLayerOperation` AVOID referencing any `MapDesignerBrushPresetSO` (no nested composites)?
8. Do all 3 sample `.asset` files exist and open without errors in Inspector?
9. Did the JSON round-trip test PASS for `Brush_CleanStoneFloor.asset`?
10. Does `dotnet build RIMA.Runtime.csproj` return 0 errors?
11. Did I implement any V1 EXCLUSION listed in §3? (must be NO)
12. Did I modify any file outside `Assets/Scripts/MapDesigner/Brush/Data/`, `Assets/Tests/EditMode/Brush/`, `Assets/Data/Brush/`, and possibly create-only the test asmdef? (must be NO)
13. Did I wrap multi-asset creation in `AssetDatabase.StartAssetEditing()/StopAssetEditing()`?
14. Did I commit any code? (must be NO — orchestrator commits)

---

## 7. Output to CODEX_DONE.md

```markdown
# Sprint 1 — Data Layer (V1 minimum) — Codex Report

## Files Created
[list with line counts]

## Sample Assets Created
[list 3 .asset files]

## Field-by-Field Compliance With design.md §4
| Spec Field | Implemented | Notes |
|---|---|---|
| ... | ✓/✗ | ... |

## Self-Review Checklist (1–14)
1. [yes/no + brief note]
...

## Build Result
`dotnet build RIMA.Runtime.csproj`: [PASS / FAIL + error excerpt]

## Test Results
EditMode test suite: [count PASS / count FAIL]
Failing tests + reason: [if any]

## Open Questions / Deviations
[any spec ambiguity you resolved, with justification]

## Files Modified Outside Scope
[must be empty; if not, explain why and how to revert]
```

---

## 8. Estimated Effort

~6–8 hours of Codex time (high effort flag). One dispatch, one CODEX_DONE.md report. Orchestrator will then run rima-qc review before marking Sprint 1 complete.

---

## 9. Codex Safety Review Addendum (LOCKED 2026-05-16)

The Unity safety review (`STAGING/codex_safety_review_output.md`) completed BG with verdict **"Approve with additions"**. The following constraints are now **binding** on Sprint 1. Where this addendum conflicts with §1–§8, the addendum wins.

### 9.1 Canonical Asset Creation Pattern

For the 3 sample .asset files, follow this exact order:

```csharp
// 1. Validate inputs first — fail before mutating Unity state
if (string.IsNullOrEmpty(outputFolder) || !outputFolder.StartsWith("Assets/", StringComparison.Ordinal))
    throw new ArgumentException("Output folder must be under Assets/.");

Directory.CreateDirectory(outputFolder);

// 2. NO external file IO in Sprint 1 — skip Refresh-before-create

// 3. Batch create
AssetDatabase.StartAssetEditing();
try {
    var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
    pool.poolName = "Floor_ShatteredKeep";
    // ... populate fields
    var path = AssetDatabase.GenerateUniqueAssetPath($"{outputFolder}/AssetPool_Floor_ShatteredKeep.asset");
    AssetDatabase.CreateAsset(pool, path);
    EditorUtility.SetDirty(pool);
    // ... repeat for brush + composite
} finally {
    AssetDatabase.StopAssetEditing();   // CRITICAL — must be in finally
}

AssetDatabase.SaveAssets();
// NO AssetDatabase.Refresh() call in Sprint 1 (no external file IO occurred)
```

**Rules:**
- `StopAssetEditing()` MUST be in `finally` — never let an exception leave Unity in mid-batch state
- `SaveAssets()` is outside the try/finally, AFTER `StopAssetEditing`
- NO `AssetDatabase.Refresh()` in Sprint 1 (no files written outside Unity)
- NO mid-batch `SaveAssets()` or `Refresh()`

### 9.2 schemaVersion Field — Required

Add a `schemaVersion` field to every JSON DTO root (NOT the SO itself — the DTO).

```csharp
[Serializable]
public class BrushPresetDTO {
    public int schemaVersion = 1;     // REQUIRED — write as first field
    public string brushName;
    public BrushCategory category;
    // ... rest
}

[Serializable]
public class AssetPoolDTO {
    public int schemaVersion = 1;
    // ... rest
}

[Serializable]
public class BrushPackDTO {
    public int schemaVersion = 1;
    // ... rest
}
```

V1 behavior: **store only, no migration logic**. V2 will add migration. The field's job in V1 is to be present so V2 round-trip can detect old data.

### 9.3 Asmdef Constraints (Sprint 1)

- All new source files: namespace `RIMA.MapDesigner.Brush.Data`, asmdef `RIMA.Runtime` (existing — do NOT create new asmdef)
- **DO NOT modify** any existing `.asmdef` file
- **DO NOT create** new `.asmdef` files in Sprint 1
- Test asmdef may be created IF missing: `Assets/Tests/EditMode/Brush/RIMA.Brush.Tests.asmdef`, references: `RIMA.Runtime`, `UnityEngine.TestRunner`, `UnityEditor.TestRunner`, `nunit.framework`. Include `Editor` platform only. If the test asmdef already exists, do not touch it beyond adding references if needed.
- Editor concerns (executors, EditorWindow, BrushJsonSerializer's AssetDatabase calls) are **deferred to Sprint 2/5**. In Sprint 1, `BrushJsonSerializer` is a pure data class — no `AssetDatabase` calls. Sprite path resolution helper that uses `AssetDatabase.LoadAssetAtPath` lives in `Assets/Scripts/MapDesigner/Brush/Data/Editor/` under a separate small Editor asmdef IF needed. Prefer: put sprite resolution under conditional `#if UNITY_EDITOR` block so Runtime asmdef can still reference it without a separate asmdef.

### 9.4 Build Gate — Both Asmdefs

- `dotnet build RIMA.Runtime.csproj` MUST pass 0 errors
- `dotnet build RIMA.Editor.csproj` should also pass 0 errors (no Sprint 1 changes expected here, but verify nothing accidentally regresses)
- Report both build outputs in CODEX_DONE.md

### 9.5 Duplicate GUID Scan

Before reporting done:
- List every new `.meta` file Codex created (path + GUID)
- Verify no GUID collision with existing `.meta` files in `Assets/`
- If a collision is found, halt and report — do not auto-resolve

### 9.6 Updated Dispatch Limit

For Sprint 1 (ScriptableObject + test + asset creation work), the safe per-dispatch file count is **max 5 source files + tests**. The Sprint 1 scope (7 source files + 1 test file + 3 .asset files) exceeds this, so:

**Split Sprint 1 into 2 sub-dispatches IF Codex prefers**, or proceed as one dispatch with extra care. Recommended split:
- Sub-dispatch 1: Enums.cs + BrushLayerOperation.cs + AssetPoolSO.cs + MapDesignerBrushPresetSO.cs + AssetPool_Floor.asset (data + 1 sample)
- Sub-dispatch 2: BrushPackSO.cs + BiomeSkinSO.cs + BrushJsonSerializer.cs + BrushDataTests.cs + 2 remaining .asset files

Either approach is acceptable. Codex decides; document choice in CODEX_DONE.md.

### 9.7 Console Gate (orchestrator verification, Sprint 1 end)

After Codex reports done, orchestrator will:
1. Open Unity
2. Wait for csproj regenerate
3. Inspect Console — no new error or null reference allowed
4. Open each new .asset in Inspector — no missing field exceptions
5. Run EditMode test suite — all PASS

Codex cannot verify Console state (no Unity access), but Codex must produce code that does not throw at SO `OnEnable` or default-value access.

### 9.8 Updated Self-Review Checklist (additions to §6)

Append these to the §6 checklist Codex must answer in CODEX_DONE.md:

15. Did I add `schemaVersion = 1` field to every JSON DTO root (BrushPresetDTO, AssetPoolDTO, BrushPackDTO)?
16. Did I respect the max 5-file/dispatch limit, or did I split into 2 sub-dispatches with clear boundaries?
17. Did I list all new `.meta` files (with paths and GUIDs) in CODEX_DONE.md?
18. Did `dotnet build RIMA.Editor.csproj` also pass 0 errors (regression check)?
19. Did I AVOID modifying any existing `.asmdef` file?
20. Is `BrushJsonSerializer` a pure data class with NO `AssetDatabase` calls in the Runtime path?

### 9.9 Pre-Flight Snapshot (orchestrator does before dispatch)

Before dispatching this task, the orchestrator MUST:
1. Record current commit: `git rev-parse HEAD > STAGING/brush_sprint_1_base.txt`
2. Verify clean working tree: `git status --short` returns empty
3. Confirm Unity is **closed** before dispatch (Codex generates files; Unity reopens after)
4. Snapshot current EditMode test results as regression baseline

These steps are orchestrator responsibility; Codex assumes they were done.

---

**This addendum is the binding contract for Sprint 1. All other Sprint 1 task spec sections (§0–§8) remain valid; §9 supersedes any conflicting detail.**
