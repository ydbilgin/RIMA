# RIMA Live Editor T3 Scaffold — Adversarial Review & Integration Plan

Reviewer pass: opened every scaffold file in `STAGING/livetool_t3/` and cross-checked every
external API call against the REAL source on disk. Verdict per file below, then the exact
Unity-side manual steps, then the C6/C7/C9 IMGUI→UI Toolkit porting plan.

Ground-truth sources read for verification:
- `Assets/Scripts/Live/RuntimeAssetRegistry.cs` (RegistryEntry, AssetKind, GetByTag/GetTile/GetPrefab/GetSprite/Instance/Count — namespace `RIMA.Live`)
- `Assets/Scripts/Live/RoomLayoutData.cs` (RoomLayoutData/RoomLayoutMeta/FloorTileData/CliffCellData/PropData/ColliderOverrideData/FromJson — namespace `RIMA.Live`)
- `Assets/Scripts/Live/LiveRoomReloader.cs` (`#if DEVELOPMENT_BUILD || UNITY_EDITOR`, ApplyCliffTiles consumes tile_guid, StableId)
- `Assets/Scripts/Live/JsonFileWatcher.cs` (`.lock` wait loop line 165, `#if DEVELOPMENT_BUILD || UNITY_EDITOR`)
- `Assets/Editor/RoomPainter/LiveTool/RuntimeBrushPalette.cs` (PaletteMode enum + namespace `RIMA.Editor.RoomPainter.LiveTool`)
- `Assets/Editor/RoomPainter/LiveTool/RuntimeColliderHandles.cs` (current API = `Draw(Rect)`, `new()`, IMGUI)
- `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetLoader.cs`, `RoomLayoutSerializer.cs`, `RuntimeAssetRegistryBaker.cs`, `LiveToolLauncher.cs`
- `Assets/Scripts/RoomPainter/RoomLayer.cs`, `RoomPainterAsset.cs` (ColliderShape) — namespace `RIMA.RoomPainter`
- `Assets/Scripts/RIMA.Runtime.asmdef`, `Assets/Editor/RoomPainter/RIMA.RoomPainter.Editor.asmdef`
- `ProjectSettings/ProjectVersion.txt` (6000.3.6f1), `Packages/manifest.json` (inputsystem 1.18.0), `ProjectSettings.asset` (`activeInputHandler: 1`)

---

## 0. The single most important ground-truth correction (overrides the contract's §1)

**The contract's Assembly Strategy is built on a FALSE premise.** Contract §1 states:

> "`Assets\Scripts\Live\` (existing C4/C10/C11/RoomLayoutData) has **no asmdef** → it folds into the predefined `Assembly-CSharp`."

This is wrong. `Assets/Scripts/RIMA.Runtime.asmdef` exists at the **root of `Assets/Scripts/`** (`"name":"RIMA.Runtime"`, `"includePlatforms":[]` = all platforms). Every subfolder — `Assets/Scripts/Live/`, `Assets/Scripts/RoomPainter/` — is therefore compiled into **`RIMA.Runtime`**, NOT `Assembly-CSharp`. The only nested asmdef under `Scripts/` is `Assets/Scripts/Editor/RIMA.Editor.asmdef`; there is none under `Live/` or `RoomPainter/`.

Consequences that simplify the whole integration:
- `RIMA.Live.*` (RoomLayoutData, RuntimeAssetRegistry, RegistryEntry, AssetKind, all sub-DTOs) live in `RIMA.Runtime`.
- `RIMA.RoomPainter.RoomLayer` and `RIMA.RoomPainter.ColliderShape` ALSO live in `RIMA.Runtime`.
- A new `RIMA.LiveTool.asmdef` that references `RIMA.Runtime` gets ALL of those types directly via `using RIMA.Live;` / `using RIMA.RoomPainter;` — exactly what the scaffold files already write.
- **`RIMA.Live.asmdef` is NOT needed.** Do not create it. The contract's "relocate the four RIMA.Live files / add RIMA.Live ref to RIMA.RoomPainter.Editor" churn is unnecessary and would actively break `RIMA.RoomPainter.Editor` (which already reaches `RIMA.Live` through its existing `RIMA.Runtime` reference).
- This means C10/C11 do NOT "ship in Game.exe via Assembly-CSharp" — they ship via `RIMA.Runtime`, gated by their own `#if DEVELOPMENT_BUILD || UNITY_EDITOR`. The 0-byte-in-release behavior is preserved by that `#if`, not by an asmdef.

Net: the asmdef plan reduces to **one** new runtime asmdef (`RIMA.LiveTool`) + **one** new editor asmdef (`RIMA.Build.Editor`). Two, not four. See §3 for the corrected `RIMA.LiveTool.asmdef` reference list.

---

## 1. PASS / FAIL verdict per file

| # | File | Verdict | Blocking issue |
|---|------|---------|----------------|
| 1 | `BrushExecutorRouter.cs` | **FAIL (compile)** | Hard dependency on `PaletteMode`, which is NOT visible to a `RIMA.LiveTool` assembly (see B1). All other API calls verified correct. |
| 2 | `RuntimeCliffHoverIndicator.cs` | **CONDITIONAL PASS** | Compiles once the asmdef references `Unity.InputSystem` (B2). Logic/API all correct. One behavioral bug (B6, non-blocking). |
| 3 | `ToolBootstrap.cs` | **FAIL (compile)** | Depends on `RuntimeBrushPalette` (runtime twin) + `PaletteMode` (B1) AND on the runtime `RuntimeColliderHandles` API (`Initialize/SetTarget/Tick/Undo`) that does NOT exist yet (B3). Neither type is in this scaffold batch. |
| 4 | `LiveToolBuildProcessor.cs` | **CONDITIONAL PASS** | Editor-only guard correct; all cross-assembly calls verified. Needs its own editor asmdef referencing `RIMA.RoomPainter.Editor` (B4). One Game-build default-options concern (B7, non-blocking). |
| 5 | `ToolMain.uxml` | **CONDITIONAL PASS** | Well-formed for runtime UITK, but declares `xsi=` without the `xmlns:` prefix and references `UnityEditor.UIElements` (B5, cosmetic/strip on import). Element names match ToolBootstrap bindings — but UXML exposes elements (layer toggles, apply-room, collider-offset-*, status-bar, canvas-hud) that ToolBootstrap never binds (B8, non-blocking gap). |
| 6 | `ToolMain.uss` | **PASS** | Valid USS. Selector mismatch with code: USS styles `.brush-thumb.selected` but ToolBootstrap toggles class `brush-thumb--selected` (B9, cosmetic — selection highlight silently no-ops). |

**Net: 1 clean PASS, 3 CONDITIONAL PASS, 2 FAIL.** The two FAILs are not defects in the authored files themselves — they are unmet dependencies on components the contract lists as separately-missing (runtime C6 `RuntimeBrushPalette` and runtime C7 `RuntimeColliderHandles`). The scaffold cannot compile as a unit until those two runtime twins are authored in the same `RIMA.LiveTool` assembly. Runtime-safety (no UnityEditor in runtime files) is fully satisfied; the Editor processor is correctly Editor-gated.

---

## 2. Findings detail

### B1 — `PaletteMode` is unreachable from `RIMA.LiveTool` (FAIL, blocks files 1 & 3)
`PaletteMode` is declared in `RuntimeBrushPalette.cs` line 160, namespace **`RIMA.Editor.RoomPainter.LiveTool`**, inside the **Editor-only** assembly `RIMA.RoomPainter.Editor` (`"includePlatforms":["Editor"]`). A Player-build assembly (`RIMA.LiveTool`) cannot reference an Editor assembly, and even in-Editor a `using RIMA.Live; using RIMA.RoomPainter;` does NOT bring in the `RIMA.Editor.RoomPainter.LiveTool.PaletteMode` symbol. Both `BrushExecutorRouter.cs` (uses `PaletteMode` in `Paint`/`Erase`/`ResolveAction`) and `ToolBootstrap.cs` (uses it pervasively) will not compile.
**Fix:** the runtime C6 port (`Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs`, namespace `RIMA.LiveTool`) MUST carry the `PaletteMode` enum into the `RIMA.LiveTool` namespace. The scaffold already assumes `PaletteMode` resolves under `namespace RIMA.LiveTool` (no extra `using`), so porting C6 with its enum into `RIMA.LiveTool` resolves B1 for both files. **C6-runtime is a hard prerequisite for this batch to compile and is not included in it.**

### B2 — `RIMA.LiveTool.asmdef` reference list is missing `Unity.InputSystem` (CONDITIONAL, blocks files 2 & 3)
`RuntimeCliffHoverIndicator.cs:23` and `ToolBootstrap.cs:32` do `using UnityEngine.InputSystem;` (`Mouse.current`, `Keyboard.current`). The contract's proposed references are `["RIMA.Runtime", "RIMA.RoomDesigner.Core", "Unity.ugui"]` — **no InputSystem**. Verified: package `com.unity.inputsystem@1.18.0` present, asmdef name is `Unity.InputSystem`, and `activeInputHandler: 1` (New Input System active, so `ENABLE_INPUT_SYSTEM` is defined). Without the reference these two files fail with "type or namespace InputSystem does not exist."
**Fix:** add `"Unity.InputSystem"` to `RIMA.LiveTool.asmdef` references. Also `"RIMA.RoomDesigner.Core"` is unused by any scaffold file (grep clean) — keep only if a later component needs it, otherwise drop. `"Unity.ugui"` is not used by the scaffold either (UI is UI Toolkit, not uGUI) — UI Toolkit (`UnityEngine.UIElements`) is part of the `com.unity.modules.uielements` engine module and needs no asmdef reference. Recommended minimal list: `["RIMA.Runtime", "Unity.InputSystem"]`.

### B3 — `ToolBootstrap` calls a `RuntimeColliderHandles` API that does not exist (FAIL, blocks file 3)
The CURRENT `RuntimeColliderHandles.cs` (Editor) exposes `new RuntimeColliderHandles()` + `Draw(Rect canvasRect)` (IMGUI). `ToolBootstrap` instead calls `Initialize(VisualElement, Camera)`, `SetTarget(GameObject, RegistryEntry)`, `Tick(RoomLayoutData)`, `Undo()`. None of these exist yet — they are the planned runtime port API (contract §3 C7). The default `new RuntimeColliderHandles()` ctor used in `BuildRuntimeSystems()` line 166 also assumes the runtime type lives in `RIMA.LiveTool` (no `using` for the Editor namespace). So even ignoring the missing methods, the bare `RuntimeColliderHandles` identifier won't resolve in `RIMA.LiveTool`.
**Fix:** author the runtime C7 (`Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs`, namespace `RIMA.LiveTool`) with exactly `Initialize/SetTarget/Tick/Undo/CurrentShape` before ToolBootstrap will compile. **C7-runtime is a hard prerequisite for this batch and is not included in it.**

### B4 — `LiveToolBuildProcessor` needs an Editor asmdef that references `RIMA.RoomPainter.Editor` (CONDITIONAL, blocks file 4)
`LiveToolBuildProcessor.cs:53` does `using RIMA.Editor.RoomPainter.LiveTool;` to reach `RoomLayoutSerializer` + `RuntimeAssetRegistryBaker`, both in assembly `RIMA.RoomPainter.Editor`. Verified calls are correct: `RuntimeAssetRegistryBaker.Bake()` returns `RuntimeAssetRegistry` (`:63`), `RoomLayoutSerializer.WriteCurrent()` (`:29`) and `RoomLayoutSerializer.CurrentJsonPath` (`:17`) exist. But the file's target home (`Assets/Editor/Build/`) has no asmdef. If placed under a new `RIMA.Build.Editor.asmdef`, that asmdef MUST list `"RIMA.RoomPainter.Editor"` (and inherit `RIMA.Runtime` transitively) or the `using` fails.
**Fix:** create `Assets/Editor/Build/RIMA.Build.Editor.asmdef` with `"includePlatforms":["Editor"]`, `"references":["RIMA.RoomPainter.Editor"]`. (Folding the file into `RIMA.RoomPainter.Editor` instead also works and needs no new asmdef — simpler; choose one.)

### B5 — UXML namespace declaration + editor-namespace reference (CONDITIONAL/cosmetic, file 5)
`ToolMain.uxml:30` declares `xsi="http://www.w3.org/2001/XMLSchema-instance"` — to be schema-valid this must be `xmlns:xsi="..."` and the next line should be `xsi:noNamespaceSchemaLocation="..."` (the `xsi:` prefix is missing). Line 29 declares `xmlns:uie="UnityEditor.UIElements"` but `uie:` is never used in the body, and `UnityEditor.UIElements` is an Editor-only namespace — harmless at runtime (no `uie:` elements exist) but should be removed for a runtime panel. Unity's UXML importer is lenient and will still import; this is cosmetic, not a compile blocker. The `<Style src="ToolMain.uss" />` element is valid runtime stylesheet attachment.
**Fix (optional):** `xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"`, prefix the schema-location attr with `xsi:`, drop the `uie` xmlns.

### B6 — Cliff hover ghost depth math is wrong for an orthographic camera looking down -Z (non-blocking, file 2)
`RuntimeCliffHoverIndicator.Update()` line 154 builds the screen point with `z = GhostDepth (10f)`. For the standard 2D setup (camera at `z = -10`, looking +Z), `ScreenToWorldPoint` z is the distance IN FRONT of the camera. With camera at `z=-10`, `z=10` lands the unprojected point at world `z = 0` — which happens to be correct here, but only by coincidence of the default `-10` camera. `ToolBootstrap.ScreenCellUnderPointer` (line 571) instead uses `z = -previewCamera.transform.position.z` (= `+10` for a `-10` camera) — same result, different derivation. The two paths agree for the default camera but will silently diverge if the camera z changes. For ortho cameras the unprojected x/y are independent of the supplied z anyway, so x/y snapping is robust; only the ghost's world z placement matters and it is forced to 0 on line 166. Net: works for the locked camera, fragile if camera z is edited.
**Fix (optional):** compute depth as `previewCamera.nearClipPlane` or just `Mathf.Abs(previewCamera.transform.position.z)` in both places, and document the camera-z assumption.

### B7 — Game build ships as a Development build (non-blocking, file 4)
`LiveToolBuilds.BuildGame()` line 184 sets `options = BuildOptions.Development`. That is deliberate (comment: keeps LiveRoomReloader/JsonFileWatcher alive via `DEVELOPMENT_BUILD`), and matches the existing `LiveToolLauncher.BuildBothTargets`. But it means "Game" is never a clean release here. Acceptable for the dev-loop, but note: a true shipping `RIMA.exe` must drop `Development`, which also removes live-reload from that binary (by design). Flagged so it is a conscious choice, not an oversight.

### B8 — UXML exposes controls ToolBootstrap never binds (non-blocking gap, files 3 & 5)
UXML declares `layer-filter` (10 `layer-*` toggles), `apply-room`, `collider-offset-x/-y`, `status-bar`, `canvas-hud`/`hud-mode`/`hud-rotation`. ToolBootstrap binds NONE of these:
- Layer toggles: no `palette.SetLayerFilter(...)` wiring at all → the layer filter UI is inert. (RuntimeBrushPalette HAS `SetLayerFilter/ClearLayerFilter`, verified `:65/:73`, just unused.)
- `apply-room`: contract/UXML say it's a `RequestSave` alias; ToolBootstrap only wires `save-room`.
- `collider-offset-x/-y`: BrushExecutorRouter/RuntimeColliderHandles will write `ColliderOverrideData.offset`, but there's no edit surface wired, and `OnColliderSizeChanged` never touches offset.
- `status-bar` / HUD labels: never updated (the success path logs to Console, the abort path `ShowAbortBanner` does `root.Clear()` + adds its own Label rather than using `status-bar`/`error-banner`).
None of these block compilation; they are functional gaps to close in C5 follow-up.

### B9 — Selected-thumb highlight class mismatch (non-blocking, files 3 & 6)
`ToolBootstrap.HighlightSelectedThumb` (line 627) toggles class `"brush-thumb--selected"`. `ToolMain.uss` (line 129) styles `.brush-thumb.selected`. The names don't match → selecting a thumbnail will never show the cyan selected border. Pick one (recommend the USS `selected` BEM-less form OR change USS to `.brush-thumb--selected`).

### Verified-correct API surface (no issues found)
- `RuntimeAssetRegistry.Instance` (`:123`), `.Count` (`:30`), `.GetByTag` returns non-null `IReadOnlyList<RegistryEntry>` (`:83-89`), `.GetTile/.GetPrefab/.GetSprite/.Get/.Contains/.Entries` — all signatures match scaffold usage.
- `RegistryEntry` fields `guid/displayName/tag/layer/sprite/tile/prefab/kind` (`:187-212`) — all used correctly; `AssetKind.Tile/Sprite/Prefab/TileAndSprite` (`:216`) used correctly in `ResolveAction`.
- `RoomLayoutData` fields `schema_version/room_id/metadata/floor_tiles/cliff_cells/prop_instances/collider_overrides` + `FromJson` (`:31`) — all correct.
- `FloorTileData{cell,tile_guid}`, `CliffCellData{cell,tile_guid,is_decor}`, `PropData{prefab_guid,position,rotation,instance_id}`, `ColliderOverrideData{instance_id,size,offset,shape}`, `RoomLayoutMeta{name,created,modified}` — every field the scaffold writes exists with the exact name/type.
- JSON path literal `StreamingAssets/live/room_current.json` matches `LiveRoomReloader.JsonPath` (`:45`) and `RoomLayoutSerializer.CurrentJsonPath` (`:17`); `.lock` protocol matches `JsonFileWatcher` wait loop (`:165`). Schema `"1.1"` matches `RoomLayoutSerializer.IsCompatibleSchema` (`:140`).
- Cliff `tile_guid` write closes the "cliff no-op": `LiveRoomReloader.ApplyCliffTiles` (`:207-234`) early-returns unless a cell has a non-empty `tile_guid`; BrushExecutorRouter.PaintCliff sets it — correct.
- `instance_id` format `"{guid}_{seq}"` is consumed by `LiveRoomReloader.StableId` (`:371`, prefers explicit instance_id) — correct diff keying.
- `RuntimeAssetRegistryBaker.Bake()` / `RoomLayoutSerializer.WriteCurrent()` / `CurrentJsonPath` calls in the build processor — all verified present.
- Output path literals in `LiveToolBuilds` equal `LiveToolLauncher.ToolExeRelative/GameExeRelative` (`:28-29`) — correct.
- `NamedBuildTarget.Standalone` + `PlayerSettings.Get/SetScriptingDefineSymbols(NamedBuildTarget,...)` valid on Unity 6000.3.6f1 (confirmed).
- Runtime files contain ZERO `UnityEditor`/`Handles`/`Gizmos`/`AssetDatabase` references (grep-clean). The build processor is fully wrapped in `#if UNITY_EDITOR`. `RIMA_LIVE_TOOL` guards present on all four runtime `.cs` files (belt-and-suspenders over the asmdef `defineConstraints`).

### JSON serializer interop note (verified safe)
The Tool writes with `JsonUtility.ToJson` (ToolBootstrap `:226`); the Game reads with `JsonUtility.FromJson` via `RoomLayoutData.FromJson` (`:40`). Same type, same serializer → round-trips exactly. The Editor `RoomLayoutSerializer` uses Newtonsoft with snake_case field names that match `RoomLayoutData` (verified field-by-field), and `NullValueHandling.Ignore` only omits nulls which JsonUtility tolerates on read. No drift. (Caveat: `JsonUtility` cannot serialize a `null` list as omitted — it writes `[]` — but the reader and reloader both handle empty lists, so this is benign.)

---

## 3. Corrected new-asmdef contents (use these, not the contract's four)

**`Assets/Scripts/LiveTool/RIMA.LiveTool.asmdef`**
```json
{
  "name": "RIMA.LiveTool",
  "rootNamespace": "RIMA.LiveTool",
  "references": ["RIMA.Runtime", "Unity.InputSystem"],
  "includePlatforms": [],
  "excludePlatforms": [],
  "defineConstraints": ["RIMA_LIVE_TOOL"],
  "autoReferenced": true,
  "noEngineReferences": false
}
```
(Drop `RIMA.RoomDesigner.Core` and `Unity.ugui` — unused by the scaffold. `RIMA.Live`/`RIMA.RoomPainter` types arrive via `RIMA.Runtime`.)

**`Assets/Editor/Build/RIMA.Build.Editor.asmdef`** (or fold the processor into `RIMA.RoomPainter.Editor` and skip this)
```json
{
  "name": "RIMA.Build.Editor",
  "references": ["RIMA.RoomPainter.Editor"],
  "includePlatforms": ["Editor"]
}
```

**Do NOT create `RIMA.Live.asmdef`.** It would carve `RIMA.Live.*` out of `RIMA.Runtime` and break every existing consumer (LiveRoomReloader, the Editor palette, etc.) that reaches those types through `RIMA.Runtime`.

---

## 4. Exact ordered Unity-side manual steps remaining

Do these in order; each unblocks the next. Steps 1–2 must precede dropping in any scaffold file.

1. **Author the two missing runtime prerequisites into `RIMA.LiveTool`** (not in this batch, but required for the batch to compile):
   - `Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs` — copy `Assets/Editor/RoomPainter/LiveTool/RuntimeBrushPalette.cs` verbatim, change namespace to `RIMA.LiveTool`, delete `using UnityEditor;` (line 2), and **carry the `PaletteMode` enum with it** (resolves B1).
   - `Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs` — runtime rewrite with the exact API `Initialize(VisualElement,Camera)` / `SetTarget(GameObject,RegistryEntry)` / `Tick(RoomLayoutData)` / `Undo()` / `CurrentShape` (resolves B3). Port plan in §5.
2. **Create the assemblies** (§3):
   - `Assets/Scripts/LiveTool/RIMA.LiveTool.asmdef` (with `Unity.InputSystem` — resolves B2).
   - `Assets/Editor/Build/RIMA.Build.Editor.asmdef` (resolves B4), or fold the processor into `RIMA.RoomPainter.Editor`.
3. **Move each scaffold file to its TARGET path** (the `// TARGET:` header on each file is authoritative):
   - `STAGING/livetool_t3/ToolBootstrap.cs` → `Assets/Scripts/LiveTool/ToolBootstrap.cs`
   - `STAGING/livetool_t3/BrushExecutorRouter.cs` → `Assets/Scripts/LiveTool/Runtime/BrushExecutorRouter.cs`
   - `STAGING/livetool_t3/RuntimeCliffHoverIndicator.cs` → `Assets/Scripts/LiveTool/Authoring/RuntimeCliffHoverIndicator.cs`
   - `STAGING/livetool_t3/LiveToolBuildProcessor.cs` → `Assets/Editor/Build/LiveToolBuildProcessor.cs`
   - `STAGING/livetool_t3/ToolMain.uxml` → `Assets/UI/LiveTool/ToolMain.uxml`
   - `STAGING/livetool_t3/ToolMain.uss` → `Assets/UI/LiveTool/ToolMain.uss`
   (Use the Editor/OS move so `.meta` files are generated by Unity; do not hand-author `.meta`.)
4. **Enable the Editor define so the Tool code compiles in-Editor:** menu `RIMA → Live Tool → Enable Tool Define (Editor)` (the processor's `EnableToolDefineInEditor`). Then let Unity recompile; check Console is clean (resolves the `defineConstraints` gate so the IDE sees `RIMA.LiveTool`).
5. **Bake the registry:** menu `RIMA → Live Tool → Bake Asset Registry` (`RuntimeAssetRegistryBaker.BakeFromMenu`). Confirm `Assets/Resources/Live/RuntimeAssetRegistry.asset` is created with >0 entries (currently MISSING per gap). Without this the Tool shows the "Registry not baked" banner.
6. **Create the Tool scene `Assets/Scenes/LiveTool/ToolMain.unity`:**
   - `[ToolRoot]` GameObject with the `ToolBootstrap` component.
   - A `UIDocument` GameObject: assign a `PanelSettings` asset (create one if none exists) + the `ToolMain.uxml` as Source Asset. Drag `ToolMain.uss` is unneeded if the `<Style src>` resolves; verify the panel renders.
   - An **orthographic** `Camera` (PPU 64 per camera lock), one `Grid` (`cellSize (1,1,1)`, Rectangular), two `Tilemap` children — one whose name contains "floor", one whose name contains "cliff" — and a `propRoot` empty Transform. A Directional Light (UnityMCP scene-setup convention).
   - Wire ToolBootstrap's serialized fields: `uiDocument`, `previewCamera`, `grid`, `floorTilemap`, `cliffTilemap`, `propRoot`.
7. **Set up the dual-build config:**
   - Confirm `RIMA_LIVE_TOOL` is in Player Settings → Scripting Define Symbols (Standalone) from step 4. The per-build processor toggles it automatically (Tool ON / Game OFF) for any build whose output path matches `Builds/RIMA_Tool/RIMA_Tool.exe` or `Builds/RIMA_Game/RIMA.exe`.
   - Game build: ensure the gameplay scenes are enabled in Build Settings (BuildGame reads `EditorBuildSettings.scenes`). The Tool scene should NOT be in that enabled list.
8. **First builds + smoke (spec §3 F7):** menu `RIMA → Live Tool → Build Tool`, then `Build Game` (or `Build Both`). Launch via `RIMA → Live Tool → Launch Live Tool`. Verify: Tool paints → writes `room_current.json` (+ `.lock` cycle) → Game's `LiveRoomReloader` applies the diff < 100 ms.
9. **Migrate the launcher (optional cleanup):** repoint `LiveToolLauncher.BuildBothTargets` to call `LiveToolBuilds.BuildTool()/BuildGame()` so there is one define-correct build path instead of two.

---

## 5. IMGUI → UI Toolkit porting plan (C6 / C7 / C9)

### C6 — `RuntimeBrushPalette` — REUSE (mechanical copy). Effort: ~30 min / LOW.
- **Why it can't ship today:** in `RIMA.RoomPainter.Editor` (`includePlatforms:["Editor"]`) and has an incidental `using UnityEditor;` (line 2). The class body uses no Editor API and no IMGUI (its own header says "No IMGUI/UI Toolkit calls here — pure data/filter logic").
- **Change:** copy file → `Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs`, namespace `RIMA.LiveTool`, delete line 2 (`using UnityEditor;`). **Carry the `PaletteMode` enum into `RIMA.LiveTool`** (it currently sits in the same file). Keep every member: `ActiveMode/LayerFilter/SearchText/SelectedEntry`, `SetRegistry/SetMode/SetLayerFilter/ClearLayerFilter/SetSearch/Select/DeselectIfCurrent/GetFiltered/HasEntries`, `PassesModeFilter`.
- **Reuse vs rewrite:** 100% reuse of logic; 0 rewrite. No view layer to port — rendering is ToolBootstrap's job (it already builds `Button.brush-thumb` from `GetFiltered()`).
- **Duplication call:** the Editor copy stays for the Editor hybrid; the runtime copy serves Tool.exe. They share no state. Accept the duplication (the alternative — a shared all-platform asmdef — adds churn for a 168-line data class).

### C7 — `RuntimeColliderHandles` — REWRITE the view, REUSE the math. Effort: ~1–1.5 days / HIGH.
- **Why it can't ship today:** Editor assembly + heavy direct Editor API: `EditorGUI.DrawRect`, `EditorGUILayout.*`, `EditorStyles.*`, `GUILayout/GUI.Label`, `Handles.BeginGUI/DrawAAPolyLine/EndGUI`, `Event.current` IMGUI loop, `EditorUtility.SetDirty`, `AssetDatabase.SaveAssetIfDirty`, and `ColliderShapeSwapper.SwapShape` (an Editor type). Current public surface is `new()` + `Draw(Rect)` — not the API ToolBootstrap calls.
- **REUSE (lift verbatim into `RIMA.LiveTool`):** the geometry/undo math — `ColliderState` struct + `Stack<ColliderState>` depth 32, the box corner/edge sign tables, axis-delta application, unit-scale computation, the 8-handle box / 2-handle circle / 2-handle capsule drag formulas, shape resolution. None of that touches Editor APIs.
- **REWRITE (new view + persistence):**
  - 8 handle dots → `VisualElement`/`Image` children of `preview-canvas`, repositioned each frame via `previewCamera.WorldToScreenPoint` (absolute layout; USS `.collider-handle` already exists). Hit-test radius raise 7px → 16px per spec.
  - Outline → a `LineRenderer` GameObject in the world (not IMGUI `Handles.DrawAAPolyLine`).
  - Input → UXML `PointerDown/Move/Up` on the canvas + `KeyDownEvent` Ctrl+Z (no `Event.current`).
  - Persistence → write into `RoomLayoutData.collider_overrides` (`ColliderOverrideData{instance_id,size,offset,shape}`) and call `ToolBootstrap.RequestSave()` — NO `EditorUtility.SetDirty`/`AssetDatabase`.
  - Shape swap → runtime: `Destroy(oldCollider2D)` + `AddComponent<Box/Circle/CapsuleCollider2D>()` (ToolBootstrap already does this in `OnColliderShapeChanged`; consolidate so C7 owns it). Drop `ColliderShapeSwapper` entirely. Defer `Polygon` as the original does.
  - New public API (must match ToolBootstrap's calls exactly): `Initialize(VisualElement canvas, Camera previewCamera)`, `SetTarget(GameObject, RegistryEntry)`, `Tick(RoomLayoutData)`, `bool Undo()`, `ColliderShape CurrentShape`.
- **Reuse vs rewrite:** ~40% reuse (math) / ~60% rewrite (entire view + IO). This is the single largest remaining work item and the critical path for the FAIL on ToolBootstrap.
- **Wiring note to fix while porting:** add the `collider-offset-x/-y` binding in ToolBootstrap (B8) so offset edits round-trip; today only size is wired.

### C9 — `RuntimeAssetLoader` — REUSE-as-shim or DELETE. Effort: ~10 min / TRIVIAL.
- **Why it can't ship today:** `using UnityEditor;` + `AssetDatabase.LoadAssetAtPath<RuntimeAssetRegistry>(...)`.
- **Change:** the runtime load path is already baked into the SO — `RuntimeAssetRegistry.Instance` (`:123`) does `Resources.Load` + `EnsureInitialized`. ToolBootstrap already calls `RuntimeAssetRegistry.Instance` directly (line 124), so the runtime C9 is OPTIONAL. If kept for symmetry, write the ~5-line `Assets/Scripts/LiveTool/Runtime/RuntimeAssetLoader.cs` (namespace `RIMA.LiveTool`): `Load() => RuntimeAssetRegistry.Instance;` and a no-op `Reload()`. The Editor copy stays for the Editor hybrid.
- **Reuse vs rewrite:** effectively delete-and-replace with a trivial shim, or omit entirely. Recommend OMIT (ToolBootstrap's direct `.Instance` call is cleaner and already in place).

---

## Final verdict

**1 of 6 files is a clean PASS (ToolMain.uss); 3 are CONDITIONAL PASS (compile once the asmdef + Editor asmdef exist); 2 FAIL to compile** purely because they depend on the two runtime twins (`RuntimeBrushPalette`+`PaletteMode`, runtime `RuntimeColliderHandles`) that are NOT in this batch. No runtime file contains any UnityEditor reference, and the build processor is correctly Editor-only — the runtime-safety contract is fully met. The authored code is API-correct against the real `RIMA.Live` / `RoomLayoutData` / `LiveRoomReloader` / `JsonFileWatcher` surface; the blockers are assembly wiring + missing sibling components, not wrong calls.

### Top 3 integration risks
1. **The contract's asmdef plan is wrong (premise-level).** `Assets/Scripts/` is already one all-platform assembly (`RIMA.Runtime`), so `RIMA.Live`/`RIMA.RoomPainter` types are NOT in `Assembly-CSharp`. Creating the contract's `RIMA.Live.asmdef` and editing `RIMA.RoomPainter.Editor` would BREAK existing consumers. Use the corrected single `RIMA.LiveTool.asmdef` (refs `RIMA.Runtime` + `Unity.InputSystem`) — and that asmdef is currently missing `Unity.InputSystem`, which alone fails two files.
2. **The batch cannot compile standalone.** `ToolBootstrap` and `BrushExecutorRouter` hard-depend on `PaletteMode` (Editor-only namespace) and on a runtime `RuntimeColliderHandles` API that does not exist. Both runtime twins (C6 with its enum, C7 rewrite) must land in `RIMA.LiveTool` first. C7 is ~1+ day of view-layer rewrite and is the critical path.
3. **Tool-writes vs Game-reads correctness hinges on the `.lock` + JsonUtility round-trip, which is correct but fragile to camera/scene setup.** The JSON/lock/schema contract is verified consistent across all four touchpoints. The real runtime risk is scene wiring: the Tilemaps MUST be named to contain "floor"/"cliff" (LiveRoomReloader discovers by name substring), the registry MUST be baked (currently absent → "Registry not baked" banner), and the preview camera z assumption (B6) must hold or x/y snapping/ghost placement drifts. Miswire any of these and the loop silently no-ops with no compile error.
