# Unity Safety Review - Brush Tool Refactor

## 1. Plan Section A - Pre-flight

Validation: mostly correct, but not sufficient for this project history.

Additions:
- Create the sprint branch from a clean base and record the base commit: `git rev-parse HEAD > STAGING/brush_sprint_N_base.txt`.
- Close Unity before script/asmdef generation batches. Reopen only after `dotnet build` is clean.
- Create the disposable scene plus `.meta`, commit both, and add it to build settings only if a PlayMode test explicitly needs it.
- Add an asset/meta preflight: `git ls-files Assets | % { if ($_ -notmatch '\.meta$' -and (Test-Path "$_.meta")) {} }` is not enough; use Unity validation or a script to check duplicate GUIDs in all `.meta` files before and after each sprint.
- The current `.gitignore` correctly excludes `Library/`, `Temp/`, `Logs/`, `obj/`, generated csproj/sln, and `UserSettings/`. Gap: it globally ignores binary art files (`*.png`, etc.). Sprint 5 creates sprites, so asset policy must be explicit: either version sprites elsewhere and keep Unity refs stable, or temporarily allow required imported source art plus `.meta`.
- Commit `.asmdef`, `.asmdef.meta`, `.asset`, `.asset.meta`, `.unity`, and `.unity.meta` together. Never separate Unity asset from meta.
- Before Sprint 1, snapshot current live safety surface: build pass, EditMode pass, and `Phase1_ProceduralMap_Test` smoke pass. That becomes the regression baseline.

## 2. Plan Section B - Codex edit rules

Validation: correct direction, but needs tighter Unity-specific controls.

Additions:
- Max 8-10 files per dispatch is safe only for ordinary C# edits. For asmdef, ScriptableObject type declarations, importers, or editor asset writes, cap at 3-5 files plus tests.
- Every edit must be preceded by a read of the exact file region. For generated new files, read the target directory and asmdef first.
- Do not add `[InitializeOnLoad]` unless the static constructor is idempotent, fast, does not create assets, and defers slow work with `EditorApplication.delayCall`.
- New runtime data types go under `Assets/Scripts/...` and compile in `RIMA.Runtime`. Editor import/export, windows, asset creation, and executors that depend on `UnityEditor` go under `Assets/Scripts/Editor` or `Assets/Editor` and compile in `RIMA.Editor`.
- Asset creation order should be: validate inputs, copy/import external files if needed, load dependencies, `StartAssetEditing`, create Unity assets in a `try`, `StopAssetEditing` in `finally`, `SaveAssets`, one `Refresh` only if non-AssetDatabase file IO happened.
- `EditorUtility.SetDirty` is required for modified ScriptableObjects after field assignment. Prefer `Undo.RecordObject` or `Undo.RegisterCompleteObjectUndo` when user-facing editor actions mutate existing assets.
- `dotnet build RIMA.Runtime.csproj` is useful but incomplete because editor-only code can still fail. Also build or let Unity compile `RIMA.Editor`, and run EditMode tests after Unity regenerates projects.
- Existing `RimaMapDesignerWindow.cs` currently calls `AssetDatabase.SaveAssets()` and `Refresh()` directly in multiple paths. Refactor work should centralize these calls before adding brush pack import.

## 3. Plan Section C - Sprint-end smoke gate

Validation: correct, but too narrow.

Additions:
- Add `git status --short` review before commit. Unexpected `.meta`, ProjectSettings, or asset churn is a gate failure until explained.
- Add duplicate GUID scan before opening Unity and before tagging.
- Add `RIMA.Editor` compile gate, not only `RIMA.Runtime`.
- Add console gate: after Unity open and tests, collect errors/warnings. No new error or null reference is allowed.
- Add asset import gate for Sprint 1/4/5: created SOs open in Inspector, references resolve, and reimport does not rewrite unrelated `.meta`.
- Add domain reload gate for Sprint 3: open Map Designer, close it, trigger recompile, reopen, verify no duplicate `SceneView.duringSceneGui` subscription symptoms and no lost selected brush state.
- Tag only after commit and smoke gates pass: `brush-sprint-N-pass`.

## 4. Plan Section D - Forbidden ops

Validation: correct and important.

Additions:
- No `AssetDatabase.StartAssetEditing()` without `StopAssetEditing()` in `finally`.
- No editing `.meta` GUIDs manually.
- No changing asmdef references while Unity is compiling or while the editor is unresponsive.
- No `EditorPrefs` for project data, selected asset paths, or brush pack state that must travel with the project.
- No long work in `OnGUI`, static constructors, or `SceneView.duringSceneGui`.
- No recursive `AssetDatabase.Refresh()` from import callbacks or `[InitializeOnLoad]`.
- No direct writes under `Library/`, `Temp/`, `obj/`, `Logs/`, `PackageCache/`, or generated csproj/sln.
- No running asset-generation scripts that create hundreds of files while Unity is open unless they use a reviewed batch API and a test scene.

## 5. Plan Section E - Test strategy

Validation: good base, but broaden by risk.

Additions:
- Sprint 1: JSON round-trip tests must include missing sprite path, moved asset path, unknown future field, older version input, and deterministic ordering of exported lists.
- Sprint 2: executor tests must verify Karar #143-D/E/K: non-walkable cells receive no L4/L5/L6, edge cells get higher density, feature masks multiply density deterministically.
- Sprint 3: EditorWindow tests should cover open/close/reopen, domain reload state, hotkeys, scene event subscription, and undo group collapse.
- Sprint 4/5: asset pack tests should verify every brush references an existing pool, every pool has at least one valid asset, and no brush creates an invalid layer operation.
- Sprint 6: automation tests should verify Auto-Dress and Regenerate Decorative preserve L1/L2 when required and clear only intended roots.
- Sprint 7: render rule tests should verify sorting layer/order, material assignment, and missing material fallback.
- Keep "unit per public method" flexible. Test behavior, not every method mechanically, or the test suite will become brittle during refactor.

## 6. Answers to Q1-Q8

### Q1 - AssetDatabase write order

For 50+ ScriptableObject assets, split external file IO from Unity asset creation.

Canonical order:
1. Validate JSON and planned asset paths before creating anything.
2. If importing external sprites from a brushpack folder, copy files into `Assets/...` first.
3. If external files were copied, call one `AssetDatabase.Refresh()` before resolving those sprites.
4. Load dependencies once and cache by path/GUID.
5. Call `AssetDatabase.StartAssetEditing()`.
6. In a `try`, create/populate SOs and call `AssetDatabase.CreateAsset`.
7. Call `EditorUtility.SetDirty` for each created or modified SO.
8. In `finally`, call `AssetDatabase.StopAssetEditing()`.
9. Call `AssetDatabase.SaveAssets()`.
10. Call one `AssetDatabase.Refresh()` only if files were written outside AssetDatabase or if importers must run now.

Example:

```csharp
public static IReadOnlyList<ScriptableObject> ImportBrushPack(BrushPackJson pack, string outputFolder)
{
    if (pack == null) throw new ArgumentNullException(nameof(pack));
    if (!outputFolder.StartsWith("Assets/", StringComparison.Ordinal))
        throw new ArgumentException("Output folder must be under Assets/.", nameof(outputFolder));

    Directory.CreateDirectory(outputFolder);

    var copiedExternalFiles = CopyBundledArtIfNeeded(pack, outputFolder);
    if (copiedExternalFiles)
        AssetDatabase.Refresh(ImportAssetOptions.Default);

    var resolver = new BrushPackAssetResolver();
    resolver.PreloadSprites(pack.GetAllSpritePaths()); // cache AssetDatabase.LoadAssetAtPath results

    var created = new List<ScriptableObject>();
    AssetDatabase.StartAssetEditing();
    try
    {
        foreach (var poolJson in pack.assetPools)
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.poolName = poolJson.poolName;
            pool.sprites = resolver.ResolveSprites(poolJson.sprites);
            pool.sourcePath = poolJson.sourcePath;

            var path = AssetDatabase.GenerateUniqueAssetPath(
                $"{outputFolder}/{SanitizeFileName(pool.poolName)}.asset");
            AssetDatabase.CreateAsset(pool, path);
            EditorUtility.SetDirty(pool);
            created.Add(pool);
        }

        foreach (var brushJson in pack.brushes)
        {
            var brush = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brush.FromJsonData(brushJson, resolver);

            var path = AssetDatabase.GenerateUniqueAssetPath(
                $"{outputFolder}/{SanitizeFileName(brush.brushName)}.asset");
            AssetDatabase.CreateAsset(brush, path);
            EditorUtility.SetDirty(brush);
            created.Add(brush);
        }
    }
    finally
    {
        AssetDatabase.StopAssetEditing();
    }

    AssetDatabase.SaveAssets();
    if (copiedExternalFiles)
        AssetDatabase.Refresh(ImportAssetOptions.Default);

    return created;
}
```

Do not put `SaveAssets` or `Refresh` inside the loop. Do not swallow exceptions before `StopAssetEditing`.

### Q2 - Domain reload safety

Add types in coherent batches with Unity closed when possible. Compile from the command line first, then open Unity once.

`[InitializeOnLoad]` is safe only when:
- The static constructor is idempotent.
- It does no expensive scan, asset creation, scene mutation, network IO, or recursive refresh.
- It guards one-time per-session work with `SessionState`.
- It defers optional work with `EditorApplication.delayCall`.
- It checks `EditorApplication.isCompiling` / `EditorApplication.isUpdating` before doing work.

Avoid serialization mismatch:
- Never rename serialized fields without `[FormerlySerializedAs]`.
- Prefer additive fields with defaults over destructive signature changes.
- Add `schemaVersion` to every JSON DTO and SO root type.
- Use `ISerializationCallbackReceiver.OnAfterDeserialize` to migrate old in-memory data.
- Keep runtime SO fields as serializable primitives, enums, Unity refs, and serializable classes. Avoid dictionaries in Unity serialization unless wrapped into serializable lists.
- For JSON, version DTOs separately from SO implementation classes.

EditorWindow persistence:
- Use `[SerializeField]` fields on the `EditorWindow` for transient UI selection that should survive domain reload while the window is open.
- Use `SessionState` for per-editor-session values such as selected brush GUID, foldouts, active tab, or last tool mode.
- Use `EditorPrefs` only for machine/user preferences such as default panel width or confirmation toggles. Never use it for project data or asset identity.
- Use project assets or ScriptableSingleton only for project-level settings that should be shared.

### Q3 - Asmdef hygiene

Recommendation: do not create one asmdef per subsystem in this refactor. The current project already has `RIMA.Runtime` and `RIMA.Editor`; keep that unless compile times or dependency boundaries force a split.

Suggested structure:
- `RIMA.Runtime`: data SOs, enums, JSON DTOs that do not reference `UnityEditor`, deterministic brush math, interfaces needed by runtime painters, and existing MapDesigner painters.
- `RIMA.Editor`: EditorWindow, import/export UI, AssetDatabase resolvers, SceneView tooling, editor-only executors if they manipulate editor scene objects.
- Optional later split: `RIMA.Brush.Runtime` and `RIMA.Brush.Editor`, but only after Sprint 2 if the dependency graph is clean.

Prevent circular references:
- Runtime must never reference Editor.
- Define shared runtime contracts in Runtime: `IBrushExecutor`, `BrushStroke`, `BrushLayerOperation`, `TargetLayer`, resolver interfaces without `AssetDatabase`.
- Editor composes implementations and calls runtime painters.
- If an executor needs AssetDatabase or Undo, it belongs in Editor. If it only places runtime objects using passed dependencies, it can be Runtime.
- `RimaMapDesignerWindow` depends on an editor service/router. Executors must not depend on the window; pass context objects instead.

Editor folder vs Editor asmdef:
- Code under an `Editor` folder is editor-only by folder convention.
- The `RIMA.Editor.asmdef` with `includePlatforms: ["Editor"]` is the stronger boundary. Put all brush editor code under its folder or another folder covered by that asmdef.
- Runtime SO types that must be referenced by scenes/assets cannot live in `Editor`.

### Q4 - EditorWindow IMGUI stability

Known risks:
- `OnGUI` can be called many times per frame with Layout/Repaint/Input events. Do not mutate assets, create objects, refresh the AssetDatabase, or run long scans directly inside drawing code.
- Wrap top-level `OnGUI` in a narrow guard that logs once and still unwinds layout groups. Prefer separating `Draw` from `HandleCommand`.
- Do not catch and ignore exceptions around actual asset mutations. Fail visibly.

Pattern:

```csharp
private void OnGUI()
{
    try
    {
        EnsureInitialized();
        DrawWindow();
        HandleDeferredCommands();
    }
    catch (ExitGUIException)
    {
        throw;
    }
    catch (Exception ex)
    {
        EditorGUILayout.HelpBox("RIMA Map Designer failed during repaint. See Console.", MessageType.Error);
        Debug.LogException(ex);
    }
}
```

`SceneView.duringSceneGui`:
- Subscribe in `OnEnable`, unsubscribe in `OnDisable`.
- Unsubscribe before subscribe if there is any chance of duplicate registration.
- Do not capture stale window state in lambdas; use a method group.
- Call `SceneView.RepaintAll()` when the brush ghost needs repaint.

Event order:
- Read `Event.current` once.
- Ignore events outside the paint rect or when modifier keys mean Unity navigation.
- On `Layout`, add control IDs if needed.
- On `MouseDown` claim hot control.
- On `MouseDrag` paint/preview if this tool owns hot control.
- On `MouseUp` release hot control and collapse undo group.
- Call `Event.Use()` only after your tool has actually handled the event.

Undo:
- `Undo.RecordObject` is best for small serialized field changes on existing objects/assets.
- `Undo.RegisterCompleteObjectUndo` is safer for Tilemap, Texture2D mask edits, and composite mutations where Unity cannot diff granularly.
- `Undo.RegisterCreatedObjectUndo` for spawned overlay GameObjects.
- For composite brush strokes:

```csharp
Undo.IncrementCurrentGroup();
int group = Undo.GetCurrentGroup();
Undo.SetCurrentGroupName("RIMA Composite Brush Stroke");

Undo.RegisterCompleteObjectUndo(tilemap, "RIMA Composite Brush Stroke");
foreach (var existingAsset in modifiedAssets)
    Undo.RecordObject(existingAsset, "RIMA Composite Brush Stroke");
foreach (var go in createdObjects)
    Undo.RegisterCreatedObjectUndo(go, "RIMA Composite Brush Stroke");

Undo.CollapseUndoOperations(group);
```

### Q5 - Tilemap + SpriteRenderer mixed rendering performance

Sprite atlas:
- Yes, use Sprite Atlas for L3-L6 art. Mixed SpriteRenderers batch better when sprites share atlas/material/shader state.
- Keep material count low. Per-sprite material overrides will break batching.

Sorting:
- Use named sorting layers aligned to the 6-layer model, or at minimum reserve fixed sorting order bands:
  - L1 floor: Ground order 0
  - L2 variation: Ground order 10
  - L3 wall overlay: Wall order 0-99
  - L4 transition: Patch order 0-99
  - L5 detail: Detail order 0-99
  - L6 accent: Accent order 0-99
- Avoid relying on Z offsets for 2D ordering. Use sorting layer and order.
- The project already has sorting-layer setup code and existing painter sorting orders. The brush refactor should consolidate these into `LayerRenderRule`.

2D Light:
- Overlay sprites receive 2D light only if they use a 2D lit material/shader and their sorting layer is included in the Light2D target sorting layers.
- If using Sprite-Lit-Default for overlays, verify all L3-L6 sorting layers are included in lights. If using Sprite-Unlit-Default, they will not respond to 2D lights.
- BiomeSkin should explicitly declare lit/unlit material choice per layer.

Pool vs mesh batching:
- 50-200 SpriteRenderers per room is acceptable for editor tooling and small rooms if atlased and pooled.
- Use pooling for interactive strokes to avoid create/destroy spikes.
- Mesh batching is a later optimization if profiling shows SpriteRenderer overhead. Do not start Sprint 2 with custom mesh aggregation; it increases undo, selection, lighting, and authoring complexity.

### Q6 - JSON serialization safety

`JsonUtility` vs Newtonsoft:
- `JsonUtility` is fast and Unity-native but limited: no dictionaries, weak polymorphism, poor missing/unknown field control, no robust custom converters.
- Newtonsoft Json.NET is safer for brushpack DTOs because versioned JSON, missing fields, unknown fields, dictionaries, and custom reference formats matter here. The lockfile already includes `com.unity.nuget.newtonsoft-json` transitively; add it explicitly to `manifest.json` before relying on it.
- Do not serialize SOs directly. Serialize DTOs, then map DTOs to SOs.

Asset reference resolution:
- Store stable project-relative asset paths plus optional GUID. Path is human-readable; GUID survives moves if `.meta` is preserved.
- Batch resolve by collecting unique refs, loading once into a dictionary, and reporting all misses together.
- Avoid repeated `AssetDatabase.LoadAssetAtPath` in inner loops.

Missing assets:
- Import should not crash after partial creation. Validate first, then either abort with a full report or import with placeholders under explicit user approval.
- For runtime/editor safety, use a placeholder sprite/material and mark the brush invalid in validation results.
- Export should include a manifest of unresolved refs if any exist.

Versioning:
- Add `formatVersion` to `pack.json` and each major DTO.
- Use `minToolVersion` and `createdWithToolVersion`.
- Unknown newer major version: refuse import.
- Older version: migrate DTO in memory, then save current version only after user confirms.

### Q7 - Codex-specific risks

Patterns Codex should follow:
- Read before every edit, including exact method/class regions.
- Keep dispatches small: 3-5 files for Unity editor/asmdef/asset creation work; 8-10 only for plain mechanical runtime edits.
- Never edit generated csproj/sln.
- Prefer one coherent patch plus tests, then build.
- Do not call `AssetDatabase.Refresh()` from generated code unless the workflow writes files outside AssetDatabase. Let Unity auto-detect script file changes; use `AssetDatabase.SaveAssets()` for dirty assets.
- Mention "requires Unity restart" when changing asmdefs, package manifest dependencies, editor initialization, render pipeline assets, or any code that cannot be safely validated until domain reload completes.
- Do not run `git reset --hard`, `git clean -fd`, or delete Library.
- Before final handoff, report changed files, build/test commands run, and whether Unity was opened.

### Q8 - Rollback hierarchy

Rollback two sprints later:
1. Identify first bad behavior with tests and `git bisect` if possible.
2. Prefer `git revert <commit>` for isolated code changes.
3. Revert a sprint tag range if the sprint introduced cross-file asset/schema changes.
4. Use branch reset only on disposable branches and only with orchestrator approval.

Detect code vs Library/cache corruption:
- Code likely: clean checkout on another clone or after Library rebuild reproduces the same compile/test/runtime failure.
- Library/cache likely: git tree is clean, project compiles from command line, but Unity editor shows importer/package/cache errors that disappear after closing Unity and deleting generated caches.
- Meta/GUID issue likely: missing references in scenes/assets, duplicate GUID scan fails, or reimport changes references.

Safe to nuke with Unity closed:
- `Library/`
- `Temp/`
- `obj/`
- `Logs/`
- generated `*.csproj`, `*.sln`, `*.slnx`

Do not nuke without approval:
- `Assets/`
- `Packages/`
- `ProjectSettings/`
- `UserSettings/` if user editor layout/preferences matter
- `.meta` files

## 7. Risk Matrix

| Operation | Risk | Mitigation |
|---|---|---|
| Create sprint branch | Low | Require clean base, record base commit, no direct master commits. |
| Create disposable test scene | Medium | Commit scene and meta together; do not add to build settings unless needed. |
| Add 15 ScriptableObject types | Medium | Add in one closed-Unity compile batch; avoid field renames; use schema defaults. |
| Add JSON DTOs and converters | Medium | Use Newtonsoft DTO layer; version every root; round-trip tests for old/missing/future fields. |
| Import 50+ SO assets | High | Validate first; use `StartAssetEditing` with `finally`; one save/refresh; duplicate GUID scan. |
| Copy bundled brushpack sprites into Assets | High | Copy before asset creation; refresh once; preserve metas when present; explicit binary asset policy. |
| Resolve 100+ sprite refs | Medium | Unique path/GUID cache; collect misses; no inner-loop loads. |
| Modify asmdef references | High | Unity closed; minimal references; build Runtime and Editor; no circular refs. |
| Add `[InitializeOnLoad]` validator | High | Avoid if possible; idempotent, fast, `SessionState` guarded, no refresh or asset creation. |
| Refactor `RimaMapDesignerWindow` layout | Medium | Keep drawing separate from mutation; top-level exception guard; domain reload test. |
| Add SceneView brush painting | High | Subscribe/unsubscribe exactly; event ownership discipline; no duplicate handlers. |
| Add composite brush executor | High | One undo group; enforce walkable/edge/feature rules before painter calls; deterministic seed tests. |
| Spawn 50-200 overlay sprites | Medium | Pool GameObjects; atlas sprites; fixed sorting bands; profile before mesh batching. |
| Add Undo/Redo for strokes | High | Use group/collapse; created object undo plus complete object undo for tilemap/texture. |
| Add default brush pack assets | Medium | Validate references; asset/meta committed together; no global binary ignore surprise. |
| PixelLab sprite import | High | Import in isolated folder; preserve source naming; verify importer settings and metas. |
| Add soft alpha shader/materials | Medium | Opt-in per BiomeSkin; fallback material; visual smoke in `Phase1_ProceduralMap_Test`. |
| BiomeSkin live swap | Medium | Cache render state; avoid asset mutation during preview; test sorting/material fallback. |
| Auto-Dress / Regenerate Decorative | High | Preserve L1/L2 by tests; clear only named roots; single undo group. |
| AssetDatabase.Refresh during tool operations | High | Only once after external file IO; never per file or from `OnGUI`/static constructors. |
| Library rebuild after instability | Medium | Close Unity first; ensure git clean; delete only generated folders. |
| Git revert of sprint assets | Medium | Revert code/assets/metas as a unit; run duplicate GUID and missing reference checks. |

## 8. Additional Recommendations

- Before Sprint 1, add a small editor validation command: brush assets valid, no duplicate meta GUIDs, no missing sprite/tile refs, no invalid sorting layer names.
- Add a `BrushImportReport` object or log output that lists created assets, skipped duplicates, unresolved refs, and schema migrations.
- Prefer GUID plus path for serialized asset refs. On import, resolve GUID first, path second, bundled art third.
- Lock naming conventions now: asset folder, SO names, menu names, sorting layer names, and root GameObject names for L3-L6.
- Use a single `BrushToolSettingsSO` or project settings asset for shared defaults. Do not scatter constants across the EditorWindow and executors.
- Keep `RimaMapDesignerWindow` as UI shell only. Move brush import/export, executor routing, and validation into services that can be EditMode tested.
- Add a dry-run mode for brushpack import that performs validation and dependency resolution without creating assets.
- Because project history includes asmdef lockups, document the dependency graph in the Sprint 1 PR/commit message.

## 9. Verdict

Approve with additions.

The proposed safety plan is directionally correct, but it must add stronger controls around AssetDatabase batching, asmdef changes, `[InitializeOnLoad]`, domain reload behavior, duplicate GUID detection, and JSON/asset-reference validation. The highest-risk areas are batch asset creation, SceneView painting, composite undo, and asmdef changes. With the additions above, the 7-sprint refactor is safe to start sequentially.

# CODEX DONE - laurethgame

Executed `CODEX_TASK_laurethgame.md`.

- Read `STAGING/map_designer_unified_brush_design.md`.
- Checked repo `.gitignore`, asmdef layout, package manifest, current Map Designer patterns, and related painter/editor references.
- Verified relevant Unity editor API docs by shell web requests where reachable.
- Wrote the full advisory review to `CODEX_DONE.md`.
- No code or Unity assets were modified.

Note: `ANTIGRAVITY.md` was not present at repo root; `rg --files -g 'ANTIGRAVITY.md'` found no match.
# Sprint 1 - Data Layer (V1 minimum) - Codex Report

## Files Created
- Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs - 20 lines
- Assets/Scripts/MapDesigner/Brush/Data/BiomeSkinSO.cs - 24 lines
- Assets/Scripts/MapDesigner/Brush/Data/BrushJsonSerializer.cs - 221 lines
- Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs - 27 lines
- Assets/Scripts/MapDesigner/Brush/Data/BrushPackSO.cs - 14 lines
- Assets/Scripts/MapDesigner/Brush/Data/Enums.cs - 37 lines
- Assets/Scripts/MapDesigner/Brush/Data/MapDesignerBrushPresetSO.cs - 17 lines
- Assets/Tests/EditMode/Brush/BrushDataTests.cs - 251 lines
- Assets/Tests/EditMode/Brush/RIMA.Brush.Tests.asmdef - 22 lines

## Sample Assets Created
- Assets/Data/Brush/AssetPool_Floor_ShatteredKeep.asset
- Assets/Data/Brush/Brush_CleanStoneFloor.asset
- Assets/Data/Brush/Brush_MossyCorner_Composite.asset

## Field-by-Field Compliance With design.md Section 4
| Spec Field | Implemented | Notes |
|---|---|---|
| BrushCategory | yes | Floor, Variation, Wall, Transition, Detail, RiftAccent, Composite |
| PaintMode | yes | All 8 V1 paint modes |
| TargetLayer | yes | L1-L6 |
| SnapMode | yes | None, FullGrid32, HalfGrid16, QuarterGrid8 |
| AlphaMode | yes | Hard, SoftAlpha8, SoftAlpha16, MultiplyBlend |
| AssetCategory | yes | All requested V1 categories |
| AssetPoolSO fields | yes | poolName/category/sprites/weights/tiles/prefabs/nativeSize/flags |
| BrushLayerOperation fields | yes | Includes density/probability/scatter/transform/tint/sorting/collision |
| Karar 143-D field | yes | respectsWalkableMask defaults true |
| Karar 143-E field | yes | wallProximityCurve defaults to 2-key flat linear curve |
| Karar 143-K field | yes | nullable FeatureMaskSO reference |
| MapDesignerBrushPresetSO fields | yes | Includes operations, previewIcon, palette flag, description, hotkey |
| BrushPackSO V1 metadata | yes | packName/version/brushes/referencedPools/coverImage only |
| BiomeSkinSO fields | yes | skinName/defaultBrushPack/layerRenderRules/globalTint/ambient |
| LayerRenderRule fields | yes | layer/alphaMode/tint/overrideMaterial/sortingOrder |
| JSON DTO schemaVersion | yes | BrushPresetDTO, AssetPoolDTO, BrushPackDTO root DTOs have schemaVersion = 1 |
| JSON object refs as paths | yes | Sprite/tile/prefab/brush/pool/feature mask references export as project paths |
| Sprite path resolution | yes | ResolveSpritePath returns null on missing path; AssetDatabase call is #if UNITY_EDITOR guarded |
| V2 exclusions | yes | No marketplace metadata, namespaces, nested composites, UI, executors, import conflict logic, zip/folder IO |

## Self-Review Checklist (1-20)
1. yes - Read STAGING/map_designer_unified_brush_design.md, including sections 16-21.
2. yes - Read STAGING/codex_unity_safety_review.md.
3. yes - Read CODEX_DONE.md and honored the safety review additions.
4. yes - All new data source files use namespace RIMA.MapDesigner.Brush.Data.
5. yes - BrushLayerOperation.respectsWalkableMask defaults to true.
6. yes - BrushLayerOperation.wallProximityCurve defaults non-null with 2 keyframes.
7. yes - BrushLayerOperation has no MapDesignerBrushPresetSO reference.
8. yes - All 3 sample .asset files exist under Assets/Data/Brush and were created via Unity AssetDatabase.
9. yes - Clean Stone Floor JSON round-trip field-by-field test passed.
10. yes - dotnet build RIMA.Runtime.csproj returned 0 errors.
11. no - No V1 exclusion was implemented.
12. no - No Sprint 1 implementation files were modified outside allowed scope, excluding this required report file. An accidental Unity scene touch from the editor/test run was restored before reporting.
13. yes - Sample asset creation uses AssetDatabase.StartAssetEditing/StopAssetEditing in finally, SaveAssets after StopAssetEditing, no Refresh.
14. no - No commit was created.
15. yes - schemaVersion = 1 is present in BrushPresetDTO, AssetPoolDTO, and BrushPackDTO.
16. yes - Proceeded as one dispatch with extra care; documented here.
17. yes - New .meta files and GUIDs are listed below.
18. yes - dotnet build RIMA.Editor.csproj returned 0 errors.
19. yes - No existing .asmdef file was modified; only new Assets/Tests/EditMode/Brush/RIMA.Brush.Tests.asmdef was created.
20. yes - BrushJsonSerializer has no AssetDatabase call in player/runtime path; editor-only path resolution is #if UNITY_EDITOR guarded.

## Build Result
- dotnet build RIMA.Runtime.csproj: PASS, 0 errors, existing warnings only.
- dotnet build RIMA.Editor.csproj: PASS, 0 errors, 0 warnings on sequential rerun.
- dotnet build RIMA.Brush.Tests.csproj: PASS, 0 errors, 0 warnings.

Note: the first editor build attempt was started in parallel with the runtime build and hit a transient shared output file lock from Microsoft Defender. No files were deleted or regenerated; the required sequential editor build passed.

## Test Results
- EditMode test suite RIMA.Brush.Tests: 8 PASS / 0 FAIL.
- Tests passed:
  - DefaultValues_BrushLayerOperation
  - AssetPoolSO_RoundTrip_Json
  - MapDesignerBrushPresetSO_Composite_HasThreeOperations
  - BrushJsonSerializer_AnimationCurve_RoundTrip
  - SpritePathResolution_Existing_ReturnsSprite
  - SpritePathResolution_Missing_ReturnsNull
  - BrushPackSO_PreservesOrder_OnRoundTrip
  - CleanStoneFloor_RoundTrip_Json_FieldByField
- Failing tests + reason: none.

## New Meta Files and GUIDs
- Assets/Data/Brush.meta - 429424f669ac00c4691b495a88acbdc3
- Assets/Data/Brush/AssetPool_Floor_ShatteredKeep.asset.meta - 00772ce5c1c4ec64bba58f490d405363
- Assets/Data/Brush/Brush_CleanStoneFloor.asset.meta - be3815969a0d97543845ca2a4b431739
- Assets/Data/Brush/Brush_MossyCorner_Composite.asset.meta - 11ae3b099831a964f81f7d65e3901464
- Assets/Scripts/MapDesigner/Brush.meta - 8fc5c288171a1cf49abb066a5277a0c6
- Assets/Scripts/MapDesigner/Brush/Data.meta - 7e5b462629a72374494371d7ff7bf5e3
- Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs.meta - 09bd9fbf8ec301241a60babe6bf52f21
- Assets/Scripts/MapDesigner/Brush/Data/BiomeSkinSO.cs.meta - 505523cab4085134d80683e82283b59c
- Assets/Scripts/MapDesigner/Brush/Data/BrushJsonSerializer.cs.meta - 9d2f22a8cbf795d4f893ef02ddaf956b
- Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs.meta - aca197983bf660d4287f32186ec9100c
- Assets/Scripts/MapDesigner/Brush/Data/BrushPackSO.cs.meta - 821f9a97828345f478b9b6c95db3b174
- Assets/Scripts/MapDesigner/Brush/Data/Enums.cs.meta - 3bad01006e0f3f24cbd07a916ee776e2
- Assets/Scripts/MapDesigner/Brush/Data/MapDesignerBrushPresetSO.cs.meta - 9ad35ea03e7fe7646be5af27af1dcc34
- Assets/Tests/EditMode/Brush.meta - 823307614bab11b48a456b11c14806cb
- Assets/Tests/EditMode/Brush/BrushDataTests.cs.meta - c8e60cbbc3140cc41b4962302c778643
- Assets/Tests/EditMode/Brush/RIMA.Brush.Tests.asmdef.meta - 20601985ac31acd4cafe473278e21860

Duplicate GUID scan result: PASS, no duplicate GUIDs found under Assets/.

## Open Questions / Deviations
- Unity was already open when the task started. I did not start a second batchmode Unity instance. I used the open editor MCP bridge for Refresh, sample asset creation, and EditMode tests.
- The current worktree already had unrelated dirty/untracked files before Sprint 1 implementation. They were not changed by this implementation.

## Files Modified Outside Scope
None for Sprint 1 implementation, excluding this mandated CODEX_DONE_laurethgame.md report.
# Sprint 2 - Executor Router + L3 Wall + Brush Along Edges - Codex Report

## Files Created
- `Assets/Scripts/MapDesigner/Brush/Stroke/BrushStroke.cs` - 20 lines
- `Assets/Scripts/MapDesigner/Brush/Executors/IBrushExecutor.cs` - 22 lines
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/BrushExecutorRouter.cs` - 63 lines
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/GridTileExecutor.cs` - 158 lines
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/WallStampExecutor.cs` - 161 lines
- `Assets/Scripts/MapDesigner/Brush/Automation/Editor/BrushAlongEdgesAutomation.cs` - 79 lines
- `Assets/Tests/EditMode/Brush/BrushExecutorTests.cs` - 245 lines

## WallOverlayPainter Changes
- Added one public method:
  - `public GameObject PlaceWallSprite(WallSegment segment, Sprite sprite, Transform parent, Tilemap tilemap = null, int index = 0)`
- Existing `PaintWalls(...)` now calls this method instead of the former private `CreateWall(...)`.
- Logic change scope: placement logic was exposed for delegation; no sorting, anchor, or sprite selection rules were changed.

## New .meta Files (GUID scan)
- `Assets/Scripts/MapDesigner/Brush/Automation.meta` - `036ff258c04adeb408923fb89f0ffe62`
- `Assets/Scripts/MapDesigner/Brush/Automation/Editor.meta` - `97742ba8406a34b42935ee8689742920`
- `Assets/Scripts/MapDesigner/Brush/Automation/Editor/BrushAlongEdgesAutomation.cs.meta` - `22843cbd012ebf048a91d1c05a992dba`
- `Assets/Scripts/MapDesigner/Brush/Executors.meta` - `b3e01f0f17b06864c9242eb3e59691e6`
- `Assets/Scripts/MapDesigner/Brush/Executors/IBrushExecutor.cs.meta` - `f9e94b49a67fb13418bb04a75b7ca902`
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor.meta` - `c035314df35c22041b85cba023147714`
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/BrushExecutorRouter.cs.meta` - `16fef848c9278584f95245ed0c3fe51c`
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/GridTileExecutor.cs.meta` - `807e705da94a4df48a24adbe46fbec29`
- `Assets/Scripts/MapDesigner/Brush/Executors/Editor/WallStampExecutor.cs.meta` - `9e1aa2ec7c45cc344a93e322c27d74a1`
- `Assets/Scripts/MapDesigner/Brush/Stroke.meta` - `fe2a8a70f627a9a4cb730c77356349aa`
- `Assets/Scripts/MapDesigner/Brush/Stroke/BrushStroke.cs.meta` - `7c5b0076fbc3b8540b6b34358ed3f180`
- `Assets/Tests/EditMode/Brush/BrushExecutorTests.cs.meta` - `f46bc4c04b14e3c45a05b8b7ccfb6650`
- Unity refresh also generated missing folder metas for pre-existing folders:
  - `Assets/Data/Brush.meta` - `429424f669ac00c4691b495a88acbdc3`
  - `Assets/Scripts/MapDesigner/Brush.meta` - `8fc5c288171a1cf49abb066a5277a0c6`
  - `Assets/Tests/EditMode/Brush.meta` - `823307614bab11b48a456b11c14806cb`
- Duplicate GUID scan under `Assets/`: PASS, no duplicate GUIDs found.

## Self-Review Checklist (1-15)
1. Yes - read all 5 MUST READ files in section 0.
2. Yes - `WallStampExecutor` delegates placement to `WallOverlayPainter.PlaceWallSprite(...)`.
3. Yes - added exactly one public method to `WallOverlayPainter`.
4. Yes - `IBrushExecutor` has no `UnityEditor` dependency.
5. No - concrete executor files are in the requested `Editor/` folder and namespace, but Unity regenerated them into `RIMA.Runtime.csproj` because the requested path is under the existing `Assets/Scripts/RIMA.Runtime.asmdef` tree. I did not modify or create asmdefs.
6. Yes - `BrushExecutorRouter.Dispatch` checks `op.respectsWalkableMask` before executor dispatch.
7. Yes - `BrushAlongEdgesAutomation` skips `isDoorway=true`.
8. Yes - `GridTileExecutor` uses `Undo.RegisterCompleteObjectUndo`; `WallStampExecutor` uses `Undo.RegisterCreatedObjectUndo`.
9. Yes - `BrushAlongEdgesAutomation` uses one `Undo.IncrementCurrentGroup` and `Undo.CollapseUndoOperations`.
10. Yes - both required dotnet builds returned 0 errors.
11. No - Unity EditMode execution could not complete because MCP timed out and direct batchmode refused while the project was already open in another Unity instance. The test assembly itself builds with 0 errors.
12. Yes - all generated `.meta` files and GUIDs are listed above.
13. No - executed as one direct implementation pass: 7 new source/test files plus one allowed existing painter change. No sub-agent dispatch was used.
14. Yes - no existing `.asmdef` was modified.
15. Yes - no Sprint 1 data layer files were modified.

## Build Result
- `dotnet build RIMA.Runtime.csproj`: PASS, 0 warnings, 0 errors on sequential rerun.
- `dotnet build RIMA.Editor.csproj`: PASS, 0 warnings, 0 errors on sequential rerun.
- `dotnet build RIMA.Brush.Tests.csproj`: PASS, 0 warnings, 0 errors.

## Test Results
- EditMode tests: NOT EXECUTED to completion.
- Attempted MCP run: `run_tests(EditMode, assembly_names=["RIMA.Brush.Tests"])` timed out waiting for Unity response.
- Attempted shell runner: Unity batchmode aborted with "another Unity instance is running with this project open."
- Compile coverage: `RIMA.Brush.Tests.csproj` builds successfully with the new executor tests included.

## Sub-dispatch Strategy
- Single direct implementation pass.
- Boundaries:
  - Runtime contracts: `BrushStroke.cs`, `IBrushExecutor.cs`
  - Editor executors and automation: router, grid tile executor, wall stamp executor, brush-along-edges automation
  - Tests: `BrushExecutorTests.cs`
  - Existing painter change: one public delegation method in `WallOverlayPainter.cs`

## Open Questions / Deviations
- Asmdef deviation: the task requested editor files under `Assets/Scripts/MapDesigner/Brush/.../Editor/` while also saying they should compile in `RIMA.Editor`. In this project, the existing `RIMA.Editor.asmdef` is under `Assets/Scripts/Editor`, so Unity includes the requested files in `RIMA.Runtime.csproj`. I did not edit asmdefs per the binding rule.
- `GridTileRandom` weighted tile selection: `AssetPoolSO` has `spriteWeights` but no `tileWeights`. I reused `spriteWeights` as optional parallel weights for tiles; empty or mismatched weights fall back to deterministic uniform selection.
- Wall layer context: Sprint 2 data does not provide an explicit active wall parent or tilemap in `BrushStroke`/`BrushLayerOperation`. `WallStampExecutor` resolves or creates a `WallOverlayPainter` host and `WallOverlay` child, then delegates placement to the painter.

## Files Modified Outside Scope
- `CODEX_DONE_laurethgame.md` - required final report.
- `Assets/Data/Brush.meta` - generated by Unity refresh for a pre-existing Sprint 1 folder.
- Existing unrelated working-tree modifications were present and not touched by this task:
  - `Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF - Fallback.asset`
  - `CODEX_TASK_laurethgame.md`
  - `CURRENT_STATUS.md`
  - `STAGING/3games_lateral_ideation.md`
  - `STAGING/3games_synthesis_s84.md`
  - untracked `STAGING/*` research/status files