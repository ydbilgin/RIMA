# Phase B-4: Map Designer Pro UI/UX — Save/Load + Variant + Layer Toggle + Persistent Binding

Status: SPEC_DRAFT_FOR_CONSULTATION
Date: 2026-05-18
Profile: TBD (auto-quota, prefer FRESH)
Effort: xhigh
Timeout: 5400s

## Context

Phase B-3 LIVE (commit f76c36e): Blueprint Painter window + AutoPopulator + Adjacency. Combat v15 + v15b iterated (4x transitions improvement). 364/364 EditMode PASS.

User direktifi: PRO UI/UX Map Designer = asıl büyük iş. B-1 + B-2 + B-3 LIVE. Phase B-4 = persistence + variant + visual debug layer for "professional editor feel".

## Goal

Add 4 PRO UX features:
1. **Room save/load** — BlueprintCanvas IntentMap → `RoomBlueprintSO` serialize/deserialize, multiple rooms managed in `Assets/Data/Blueprint/Rooms/`
2. **Persistent active target binding** — Active Room Root Transform survives editor restart via EditorPrefs (InstanceID-based)
3. **Random variant button** — re-roll seed without changing IntentMap; click "Auto-Populate" produces different placement
4. **Layer toggle** — show/hide per zone visual debug (e.g., display only path cells, dim others) for inspection workflow

## Files to ADD

### Shared runtime types (REQUIRED REFACTOR — rima-sonnet Change 1)

**Critical:** `BlueprintCanvas.cs` already defines `IntentMapEntry` as `[Serializable] struct` inside `#if UNITY_EDITOR` guard. `RoomBlueprintSO` lives in runtime assembly and CANNOT reference Editor-guarded types. Resolution = move `IntentMapEntry` out to a shared runtime-visible file BEFORE creating `RoomBlueprintSO`:

- `Assets/Scripts/Rima/MapDesigner/Data/IntentMapEntry.cs` (NEW)
  - Namespace: `RIMA.MapDesigner.Data`
  - `[Serializable] public struct IntentMapEntry { public Vector2Int pos; public string zoneId; }`
  - NO `#if UNITY_EDITOR` guard (runtime-visible)
  - Constructor: `public IntentMapEntry(Vector2Int pos, string zoneId)`

- **MODIFY `Assets/Editor/MapDesigner/Blueprint/BlueprintCanvas.cs`** — remove internal `IntentMapEntry` struct declaration, add `using RIMA.MapDesigner.Data;` directive. All existing usages (`SyncSerializedFromMap()`, `FromJson()`, `SerializedCanvas`) continue working with the shared type. **No public API surface change** — BlueprintCanvasTests 6/6 must still PASS.

### SO Contracts (`RIMA.MapDesigner.SO` namespace — Phase 1A SO style)

- `Assets/Scripts/Rima/MapDesigner/SO/RoomBlueprintSO.cs`
  - Fields:
    - `string roomId`
    - `string displayName`
    - `BlueprintProfileSO profile` (reference to active profile when saved)
    - `Vector2Int gridSize`
    - `int defaultSeed`
    - `int currentSeed`
    - `List<IntentMapEntry> intentMap` where `[Serializable] struct IntentMapEntry { Vector2Int pos; string zoneId; }` (Dictionary not serializable, flat list is)
  - Methods (instance):
    - `BlueprintCanvas ToCanvas()` — rebuild Dictionary from list
    - `void FromCanvas(BlueprintCanvas canvas)` — flatten Dictionary to list
  - CreateAssetMenu: `RIMA/MapDesigner/Blueprint/Room`

### Editor

- `Assets/Editor/MapDesigner/Blueprint/RoomSaveLoadService.cs` — Static class
  - `public static RoomBlueprintSO SaveAsNew(BlueprintCanvas canvas, BlueprintProfileSO profile, int seed, string savePath, string roomId, string displayName)` — creates new SO via `AssetDatabase.CreateAsset`
  - `public static void Overwrite(RoomBlueprintSO room, BlueprintCanvas canvas, int seed)` — updates existing SO, marks dirty
  - `public static (BlueprintCanvas canvas, int seed) Load(RoomBlueprintSO room)` — read SO, return canvas + seed
  - `public static IEnumerable<RoomBlueprintSO> ListRoomsInFolder(string folderPath = "Assets/Data/Blueprint/Rooms")` — find all saved rooms

- **MODIFY** `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs` — add 4 UI sections:
  - **"Rooms" section** (right panel, after "Populate"):
    - `ObjectField` for `RoomBlueprintSO` ("Active Room")
    - Button "Load" — calls `RoomSaveLoadService.Load`, paints canvas, sets seed
    - Button "Save Over" — overwrites active room (disabled if no active room)
    - Button "Save As New..." — opens SaveFilePanel under `Assets/Data/Blueprint/Rooms/`, calls SaveAsNew
  - **"Variant" section** (right panel, after Auto-Populate button):
    - Button "Random Seed" — sets seed = `new System.Random().Next()` (display in seed field), does NOT auto-populate (user clicks Auto-Populate next)
  - **"Layer Visibility" foldout** (left panel, below brush palette):
    - Foldout titled "Layer Visibility"
    - 6 toggle checkboxes (one per zone), default all ON
    - Toggles do NOT change IntentMap data — purely visual (dimmed alpha 0.2 vs full 1.0 for non-shown zones in canvas paint)
  - **Persistent Active Room Root** logic:
    - On window OnEnable: read `EditorPrefs.GetInt("RIMA_BlueprintPainter_ActiveRoomRoot_InstanceID", 0)`, lookup via `EditorUtility.InstanceIDToObject` as Transform, set as activeRoomRoot
    - On user assigning new Active Room Root: write InstanceID to EditorPrefs
    - On Active Room Root null/destroyed: clear EditorPrefs key

### Tests

- `Assets/Tests/EditMode/MapDesigner/Blueprint/RoomSaveLoadServiceTests.cs` (7 tests):
  1. SaveAsNew_CreatesAssetAtPath
  2. SaveAsNew_PersistsIntentMap (paint 10 cells, save, load → 10 cells match)
  3. SaveAsNew_PersistsSeed
  4. SaveAsNew_PersistsProfileReference
  5. Overwrite_UpdatesIntentMap (existing SO new canvas → IntentMap changed)
  6. Load_RestoresCanvas (round-trip equality)
  7. ListRoomsInFolder_FindsAllRoomAssets
  
  **Test isolation:** each test creates SO in `Assets/Tests/_TestArtifacts/` and deletes in `[TearDown]` to avoid polluting project. Use `AssetDatabase.DeleteAsset`.

- `Assets/Tests/EditMode/MapDesigner/Blueprint/BlueprintPainterWindowTests.cs` (5 tests):
  1. RandomSeed_ProducesDifferentSeedValue
  2. LayerVisibility_AllOn_NoCellsDimmed
  3. LayerVisibility_PathOff_PathCellsDimmed
  4. ActiveRoomRoot_PersistsAcrossWindowReopen (mock EditorPrefs)
  5. ActiveRoomRoot_HandlesDestroyedTransformGracefully

## Data assets

- `Assets/Data/Blueprint/Rooms/combat_room_v15b.asset` — save current v15b canvas as reference
  - Codex extracts v15b's BlueprintCanvas from scene (read `Pro_Redesign_v15b_FullAdjacency_CombatRoom` children, infer IntentMap from `_BlueprintPlaced_{zoneId}_{x}_{y}` names) and calls SaveAsNew

## Files to MODIFY

- `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs` (sections above)
- No other modifications

## Acceptance Criteria

1. **Save/Load workflow:**
   - Paint canvas → "Save As New..." → SaveFilePanel → asset created
   - Drag existing RoomBlueprintSO into "Active Room" field → "Load" button → canvas painted from saved IntentMap
   - "Save Over" updates existing asset
2. **Random Seed button** updates seed field display, doesn't auto-populate (user-controlled)
3. **Layer Visibility** toggles dim cells per zone visually, doesn't change IntentMap data
4. **Persistent Active Room Root:**
   - Set Active Room Root → close window → reopen window → Active Room Root field auto-populated
   - Reopen Unity (manual test, not automated) → still bound
5. **`combat_room_v15b.asset` exists** as reference save
6. **12 new EditMode tests PASS** (7 SaveLoad + 5 Window)
7. **Full EditMode: 376/376 PASS** (364 baseline + 12 new)
8. **Screenshot:** `Assets/Screenshots/phase_b4_save_load_demo.png` — window with active RoomBlueprintSO loaded + Layer Visibility foldout open with 2 zones toggled off (path zone showing alone)
9. **Console:** 0 errors, 0 warnings from B-4 code
10. **DONE marker:** `STAGING/CODEX_TASK_PHASE_B4_SAVE_LOAD_VARIANT_LAYER_DONE.md` with full verification report

## Edge cases

- Save with empty IntentMap → still creates asset (allowed; user can paint then Save Over later)
- Load over already-painted canvas → confirmation dialog "Overwrite current paint?" (Yes/No)
- Active Room Root in EditorPrefs but Transform deleted from scene → clear EditorPrefs, status text "Previous Active Room Root no longer exists, please re-bind"
- Random Seed: store last 3 seed values in window state for "Undo seed" quick action (defer to B-5 if too complex)
- LayerVisibility: also dim AutoPopulate output? **NO** — only canvas paint cells are dimmed (visual debug, not asset state). AutoPopulate-placed GameObjects in scene are unaffected.
- "Save As New..." path validation: must be under `Assets/Data/Blueprint/Rooms/` — if user picks elsewhere, warn but allow

## Architecture notes

- **EditorPrefs key:** `RIMA_BlueprintPainter_ActiveRoomRoot_InstanceID` (project-prefixed to avoid conflicts)
- **InstanceID survival:** `EditorUtility.InstanceIDToObject` works within same editor session reliably; between editor restarts, InstanceID may change → fallback to name-based lookup or accept that binding clears on restart (document as known limitation, not bug)
- **OnEnable guard (rima-sonnet Change 4):** `activeRoomRoot` is already `[SerializeField]` (line 18 of current window). The OnEnable EditorPrefs lookup MUST guard against double-assignment via Unity's serialized window state:
  ```csharp
  void OnEnable() {
      if (activeRoomRoot == null) {
          int id = EditorPrefs.GetInt("RIMA_BlueprintPainter_ActiveRoomRoot_InstanceID", 0);
          if (id != 0) activeRoomRoot = EditorUtility.InstanceIDToObject(id) as Transform;
      }
  }
  ```
- **RoomBlueprintSO independence:** SO references BlueprintProfileSO but doesn't own it — multiple rooms can share one profile
- **IntentMap serialization:** Dictionary<Vector2Int, string> not Unity-serializable → flatten to `List<IntentMapEntry>` for asset; `ToCanvas()/FromCanvas()` handles conversion
- **No new Blueprint asmdef** — files under `Assets/Editor/MapDesigner/Blueprint/` covered by existing `RIMA.MapDesigner.Editor.asmdef` (Unity directory inheritance, per rima-sonnet B-3 critique Change 2)

## Codex consultation gate

This spec should be critiqued by **rima-sonnet read-only review** BEFORE IMPLEMENT dispatch:
- EditorPrefs key naming + scope concerns
- IntentMap List serialization edge cases (struct vs class)
- SaveFilePanel UX (does it allow saving outside Rooms folder?)
- Test isolation cleanup (orphan asset risk)
- BlueprintPainterWindow modification surface area — does this risk breaking B-3 baseline?

Orchestrator will run rima-sonnet, apply changes, then dispatch to Codex.

## Codex implementation notes (rima-sonnet inline guidance)

- **Test isolation belt-and-suspenders:** Use `[OneTimeTearDown]` with `AssetDatabase.FindAssets` glob over `Assets/Tests/_TestArtifacts/` deleting all `*.asset`, IN ADDITION to per-test `[TearDown]` + try/finally inside each test body. Prevents orphan assets if a test throws mid-run.
- **SaveFilePanel path validation:** Hard-reject (not warn) if save path does not start with `"Assets/"`. `AssetDatabase.CreateAsset` will throw on filesystem paths outside Assets — fail fast with clear error. Within-Assets-but-outside-Rooms is the warn-only case.
- **Right panel layout:** Adding 4+ controls to a 200px-wide right panel may overflow vertical at default window height (460px min). Wrap right panel in `EditorGUILayout.BeginScrollView` or increase `minSize` height.
- **combat_room_v15b.asset extraction logging:** When inferring IntentMap from `_BlueprintPlaced_{zoneId}_{x}_{y}` child names, log `matched_count vs total_children` — if mismatch, include in DONE marker so orchestrator can verify completeness.
- **IntentMapEntry conversion:** With Change 1 applied, both `BlueprintCanvas` and `RoomBlueprintSO` use the same `RIMA.MapDesigner.Data.IntentMapEntry` type. `ToCanvas()` and `FromCanvas()` are direct copies — no element-by-element manual mapping needed.

## Self-iteration mandate

If first implementation pass has:
- < 364 baseline tests still PASS → fix regression first
- Any of 12 new tests FAIL → iterate fix locally before DONE
- BlueprintPainterWindow doesn't compile → iterate fix (using statements, namespace)
- Save/Load roundtrip produces canvas mismatch → iterate (likely IntentMapEntry serialization issue)

Max 3 internal iterations before DONE.

## Verification commands

1. `manage_editor` set state EDITOR
2. `read_console` → 0 errors
3. `run_tests` EditMode → 376+ PASS
4. Open Blueprint Painter window
5. Paint a test layout, "Save As New..." → `Assets/Data/Blueprint/Rooms/_test_room.asset`
6. Clear blueprint, drag `_test_room.asset` into Active Room field, "Load" → canvas restored
7. Toggle 2 Layer Visibility checkboxes off → screenshot
8. Save scene + write DONE marker
9. Delete `_test_room.asset` test artifact (or leave with comment)

## Non-goals (defer to B-5)

- Multi-select rooms
- Eyedropper tool (pick zone at cursor)
- Duplicate-with-offset
- Custom zone type creation UI
- Realtime auto-populate as user paints
- Undo seed history beyond current+previous

## Verdict format

```
# Phase B-4 Save/Load + Variant + Layer Toggle + Persistent Binding Done
Status: DONE_FOR_ORCHESTRATOR_REVIEW
Date: 2026-05-18
Executor: Codex {profile}

## Files added
- {list}

## Files modified
- Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs

## Test count delta
- New RoomSaveLoadServiceTests: X/X PASS (target 7/7)
- New BlueprintPainterWindowTests: X/X PASS (target 5/5)
- Full EditMode: X/X PASS (target 376+/376+)

## Baseline preservation
- BlueprintCanvasTests: 6/6 PASS
- AutoPopulatorTests: 7/7 PASS
- AssetPackBrowserTests: 8/8 PASS
- AssetPackBrowserPlacementTests: 10/10 PASS

## Save/Load roundtrip evidence
- Test canvas painted: X cells
- Saved to: {path}
- Loaded back: X cells (match)

## Persistent binding evidence
- EditorPrefs key written/read: yes/no
- Reopen survival: tested via {method}

## Sample screenshot
- Assets/Screenshots/phase_b4_save_load_demo.png

## Console errors
- {none / list}

## Phase B-4 deliverable verdict
{PASS_FOR_ORCHESTRATOR_REVIEW / NEEDS_HELP}
```
