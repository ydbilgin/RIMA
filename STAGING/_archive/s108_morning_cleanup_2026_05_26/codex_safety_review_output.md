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