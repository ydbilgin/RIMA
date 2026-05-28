# Codex Task — Sprint 5: Editor UI Refactor (3-panel + Hotkeys + Ghost Preview)

**Type:** Implementation (EditorWindow UX layer)
**Effort:** high
**Estimated:** 1.5 days Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint5_editor_ui.md --effort high`
**Output:** Code + EditMode tests + CODEX_DONE.md report

---

## 0. MUST READ FIRST

1. `STAGING/map_designer_unified_brush_design.md` — full design (§7 UI Design especially)
2. `STAGING/codex_safety_review_output.md` — Unity safety contract (§Q4 EditorWindow IMGUI especially)
3. `STAGING/codex_brush_sprint1_data_layer.md` — data SOs to display
4. `STAGING/codex_brush_sprint2_executor_l3wall.md` — executor router to call
5. `STAGING/codex_brush_sprint4_decorative_executors.md` — decorative executors to call
6. `Assets/Editor/RimaMapDesignerWindow.cs` — existing window to refactor (do not delete; transform)

---

## 1. Context

Sprints 1–4 built the data + execution layers. Sprint 5 makes them usable. The existing `RimaMapDesignerWindow` is refactored into the 3-panel layout from design §7.1. The user selects a brush, the tool routes to the correct executor, the scene paints.

**Critical design constraint:** the user must NOT think in technical layers (L1-L6). The brush IS the artistic intent; the layer is metadata. The Layer panel on the right is for visibility/solo only — not for choosing what to paint.

---

## 2. Scope — Files to Create/Modify

### 2.1 Editor asmdef files (under `Assets/Editor/MapDesigner/Brush/` — NEW folder)

All in namespace `RIMA.MapDesigner.Brush.Editor.UI`. Asmdef: existing `RIMA.Editor`.

#### 2.1.1 `MapDesignerBrushWindow.cs` (new — REPLACES the brush-related parts of RimaMapDesignerWindow)

This is the main 3-panel EditorWindow.

**Layout responsibilities (delegated to sub-panels — see 2.1.2-2.1.5):**
- Top bar: Room dropdown + Skin dropdown + Settings gear
- Toolbar: tool mode buttons (Pick/Brush/Erase/Composite) + Auto-Dress + Undo/Redo
- Left panel: BrushPalettePanel
- Center: Scene view (delegated to SceneView.duringSceneGui)
- Right panel: BrushSettingsPanel + LayerVisibilityPanel
- Bottom bar: status line

```csharp
public class MapDesignerBrushWindow : EditorWindow {
    [MenuItem("RIMA/Map Designer/Brush Tool")]
    public static void Open() => GetWindow<MapDesignerBrushWindow>("RIMA Brush Tool");

    [SerializeField] private MapDesignerBrushPresetSO selectedBrush;
    [SerializeField] private BrushPackSO activePack;
    [SerializeField] private RoomRecipeSO activeRoom;
    [SerializeField] private BiomeSkinSO activeSkin;
    [SerializeField] private int activeSeed;

    private BrushPalettePanel palettePanel;
    private BrushSettingsPanel settingsPanel;
    private LayerVisibilityPanel layerPanel;
    private BrushSceneTooling sceneTooling;

    private void OnEnable() {
        palettePanel = new BrushPalettePanel();
        settingsPanel = new BrushSettingsPanel();
        layerPanel = new LayerVisibilityPanel();
        sceneTooling = new BrushSceneTooling();
        SceneView.duringSceneGui += sceneTooling.OnSceneGUI;
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= sceneTooling.OnSceneGUI;
    }

    private void OnGUI() {
        try {
            DrawTopBar();
            DrawToolbar();
            using (new EditorGUILayout.HorizontalScope()) {
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(260))) {
                    palettePanel.Draw(ref selectedBrush, activePack);
                }
                GUILayout.FlexibleSpace();  // scene view is implicit
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(280))) {
                    settingsPanel.Draw(selectedBrush);
                    layerPanel.Draw();
                }
            }
            DrawBottomBar();
            HandleHotkeys();
        } catch (ExitGUIException) { throw; }
        catch (Exception ex) {
            EditorGUILayout.HelpBox("RIMA Brush Tool failed during repaint. See Console.", MessageType.Error);
            Debug.LogException(ex);
        }
    }

    // ... (other methods)
}
```

#### 2.1.2 `Panels/BrushPalettePanel.cs`
- Category dropdown filter
- Search field
- Scrollable grid of brush thumbnails (2 columns)
- Selected brush display: name, target layer, paint mode, hotkey
- Composite brush expansion: shows operation list "L2: floor_var ×0.35, L4: moss ×0.45, L5: crack ×0.20"

#### 2.1.3 `Panels/BrushSettingsPanel.cs`
- Size slider (brush radius)
- Density slider
- Min Distance int field
- Rotation: random toggle + snap degrees
- Flip X/Y toggles
- Scale Range MinMax slider
- Seed int field
- Walkable filter toggle (read-only display from brush op, but allow temporary override)
- Edge-bias toggle + curve preview (foldout)
- Feature mask reference (ObjectField nullable)

#### 2.1.4 `Panels/LayerVisibilityPanel.cs`
- 6 rows for L1-L6
- Per layer: eye icon (visible toggle), solo icon, lock icon
- Solo radio behavior (one solo at a time)
- Operates on scene parent GameObjects (`Layer_L1`, `Layer_L2`, ..., `Layer_L6`)
- Persists state in `SessionState` (per Unity session, not project-wide)

#### 2.1.5 `Panels/BrushSceneTooling.cs`
- Subscribes to `SceneView.duringSceneGui` (subscription in window OnEnable, see §2.1.1)
- Reads mouse position, raycasts to tile grid
- Ghost preview (semi-transparent sprite at hover position)
- Mouse handling:
  - Layout event: add control ID
  - MouseDown: claim hot control
  - MouseDrag: paint if brush mode active
  - MouseUp: release hot control + collapse undo group
- Calls `BrushExecutorRouter.Dispatch` on actual paint event
- Wraps entire stroke in single Undo group

#### 2.1.6 `Hotkeys/BrushHotkeyHandler.cs`
- `B` = Brush mode
- `E` = Erase mode
- `C` = Composite mode (forces selected brush to composite category if available)
- `[` / `]` = decrease/increase brush size
- `Alt+Click` = eyedropper (pick brush at scene location — match by SortingLayer + sprite name lookup)
- `Shift+Click` = straight line from last point
- `Ctrl+Z` / `Ctrl+Shift+Z` = native Unity Undo/Redo (no override)
- `1` through `9` = quick select brushes from palette where `hotkeyIndex == n`

### 2.2 Existing window transition

`Assets/Editor/RimaMapDesignerWindow.cs` — DO NOT delete. Strip out brush-specific code that moved to new window. Keep:
- Karar #143 6-layer toggle (legacy view, kept for debug)
- Room generation buttons
- Aşama 1/2 test triggers

Brush logic is now in `MapDesignerBrushWindow`.

### 2.3 EditMode tests under `Assets/Tests/EditMode/Brush/`

`BrushWindowTests.cs` — minimum 6 cases:

1. **WindowOpens_NoExceptions** — `EditorWindow.GetWindow<MapDesignerBrushWindow>()` returns instance, no exceptions
2. **WindowSurvivesDomainReload** — open window, force script recompile, window still open with same selectedBrush
3. **HotkeyB_SwitchesToBrushMode** — simulate Event.current with KeyCode.B → tool mode == Brush
4. **HotkeyBracket_AdjustsSize** — simulate `[` → size decreases by 4; `]` → size increases by 4
5. **HotkeyNumber_SelectsBrushByIndex** — palette has brush with hotkeyIndex=3; press `3` → selectedBrush is that one
6. **SceneRaycast_ResolvesToCell** — given world position, returns expected Vector2Int cell

### 2.4 Removed/Migrated

The existing brush-related code from `RimaMapDesignerWindow.cs` migrates to the new window. Keep a thin compatibility shim that opens the new window if the user clicks the old menu item.

---

## 3. V1 EXCLUSIONS

- BiomeSkin live re-render (Sprint 8 — Skin dropdown stub only in Sprint 5)
- Composite executor (Sprint 6)
- Auto-Dress / Regenerate / Smart Fill (Sprint 7 — toolbar buttons may be stubs)
- Soft alpha shader (Sprint 8)
- Brush pack import/export UI (Sprint 8 or V2 — Settings gear has placeholder)

---

## 4. Acceptance Criteria

A. `dotnet build RIMA.Editor.csproj` returns 0 errors.
B. Window opens via `RIMA/Map Designer/Brush Tool` menu.
C. 3-panel layout matches §7.1 of design spec (260/center/280 px).
D. Selecting a brush updates settings panel.
E. Hotkeys B/E/[/]/1-9 functional (tested via Event injection).
F. Ghost preview appears in Scene view when brush mode active.
G. Clicking in scene paints (via router → existing executor).
H. Drag stroke spawns multiple decals (for ScatterAlongStroke brushes).
I. Single Ctrl+Z reverts entire stroke (not individual decals).
J. Domain reload preserves window state (selectedBrush survives recompile).
K. No new Console errors/warnings on window open.
L. EditMode test suite all PASS.

---

## 5. Safety Rules

All `codex_safety_review_output.md` rules apply. Key for EditorWindow refactor:

1. **OnGUI exception handling:** wrap in try/catch (see §Q4 in safety review) with `ExitGUIException` re-throw.
2. **SceneView subscribe/unsubscribe:** `OnEnable` subscribes, `OnDisable` unsubscribes. Pre-unsubscribe before subscribe to avoid duplicate handlers.
3. **No `AssetDatabase` calls in `OnGUI`.** Defer via `EditorApplication.delayCall` if needed.
4. **No `[InitializeOnLoad]`** in this sprint.
5. **`SessionState` for transient UI state** (selected brush, active tab, foldouts). `EditorPrefs` only for user prefs (panel widths).
6. **`Undo.IncrementCurrentGroup` per stroke** in SceneTooling, `CollapseUndoOperations` at stroke end.
7. **No commit.**
8. Max **5 files per dispatch.** Sprint 5 has 6 source files + 1 test = 7 → split into 2 sub-dispatches:
   - Sub 1: `MapDesignerBrushWindow.cs` + `BrushPalettePanel.cs` + `BrushSettingsPanel.cs` + `LayerVisibilityPanel.cs` + test (start) — 5 files
   - Sub 2: `BrushSceneTooling.cs` + `BrushHotkeyHandler.cs` + test extension — 3 files

---

## 6. Codex Self-Review Checklist

1. Read all 6 MUST READ files in §0?
2. Does `OnGUI` wrap in try/catch with `ExitGUIException` re-throw?
3. Does `OnEnable` pre-unsubscribe before subscribing to `SceneView.duringSceneGui`?
4. Is window state preserved across domain reload (via `[SerializeField]` fields)?
5. Are all 6 hotkeys functional?
6. Is selected brush survival tested?
7. Does single stroke produce single undo group (test)?
8. Are no `AssetDatabase` calls made in `OnGUI`?
9. Are existing `RimaMapDesignerWindow` non-brush functions preserved (Karar #143 toggle, room gen buttons)?
10. Did `dotnet build RIMA.Editor.csproj` pass 0 errors?
11. Did EditMode tests pass?
12. Did I respect max 5 files per dispatch?
13. Did I list all new .meta files?
14. Did I avoid modifying any LIVE painter or Sprint 1-4 file?

---

## 7. Output Format

Same as previous sprint CODEX_DONE.md structure.

---

## 8. Dependencies

**Blocked by:** Sprints 1, 2, 4 complete.
**Blocks:** Sprint 6 (composite brush selectable via UI), Sprint 7 (automation buttons functional), Sprint 8 (Skin dropdown live).

---

## 9. Research-Backed Implementation Notes (Gemini S85)

Full research: `STAGING/research_hades_brushux_softalpha.md` (Q3 brush UX + Q4 Unity EditorWindow patterns).

### 9.1 Reference projects (study before implementation)
- **Polybrush** — `github.com/Unity-Technologies/com.unity.polybrush` — brush settings panel + SceneView raycasting + paint stroke. Directly matches our 3-panel + scene paint requirement.
- **ProBuilder** — `github.com/Unity-Technologies/com.unity.probuilder` — custom toolbar pattern + `OnSceneGUI` for viewport manipulation
- **Chisel** — `github.com/chisel-gui/Chisel` — deep nested multi-panel UI

### 9.2 SceneView painting canonical pattern (Polybrush-derived)

```csharp
private void OnSceneGUI(SceneView sceneView) {
    Event e = Event.current;
    int controlID = GUIUtility.GetControlID(FocusType.Passive);

    // Block Unity's default selection behavior in paint mode:
    HandleUtility.AddDefaultControl(controlID);

    if (e.type == EventType.MouseDown && e.button == 0) {
        GUIUtility.hotControl = controlID;
        StartStroke(e.mousePosition);
        e.Use();
    } else if (e.type == EventType.MouseDrag && GUIUtility.hotControl == controlID) {
        ContinueStroke(e.mousePosition);
        e.Use();
    } else if (e.type == EventType.MouseUp && GUIUtility.hotControl == controlID) {
        EndStroke(e.mousePosition);
        GUIUtility.hotControl = 0;
        e.Use();
    }
}
```

**Critical:** call `Event.Use()` ONLY after you actually handle the event. Always use `HandleUtility.AddDefaultControl(controlID)` in paint mode to prevent Unity from picking SceneView objects under the brush cursor.

### 9.3 Global hotkey pattern — `[Shortcut]` attribute (preferred over OnGUI key events)

```csharp
using UnityEditor.ShortcutManagement;

[Shortcut("RIMA/Brush/SwitchToBrushMode", KeyCode.B)]
public static void HotkeyBrush() { MapDesignerBrushWindow.GetCurrent()?.SetMode(ToolMode.Brush); }

[Shortcut("RIMA/Brush/SwitchToEraseMode", KeyCode.E)]
public static void HotkeyErase() { MapDesignerBrushWindow.GetCurrent()?.SetMode(ToolMode.Erase); }
```

Advantages:
- User-rebindable via Edit → Shortcuts
- Doesn't conflict with text input fields
- Cleaner than `Event.current.keyCode` matching in OnGUI

For `1`-`9` brush slot hotkeys, use Krita pattern: `Alt+1` through `Alt+9` (not raw `1`-`9`). Reason: raw number keys often conflict with Unity's scene gizmo / view-tool shortcuts.

```csharp
[Shortcut("RIMA/Brush/SlotBrush1", KeyCode.Alpha1, ShortcutModifiers.Alt)]
public static void HotkeySlot1() => MapDesignerBrushWindow.GetCurrent()?.SelectBrushBySlot(1);
// ... repeat for 2-9
```

### 9.4 Brush palette UX pattern (Krita-derived)

The 2-column thumbnail grid in `BrushPalettePanel` should support:
- **Resizable thumbnails** — slider in panel header (32px / 48px / 64px / 80px / 96px), persisted in `SessionState`
- **Multi-tag filter** (not folder hierarchy) — `BrushPackSO` brushes can declare tags as a `List<string>` field; the palette filter is a comma-separated tag query
- **Right-click radial pop-up** (optional V1 stretch goal) — most-recently-used 6 brushes in a radial menu at cursor; deferable to V2 if time-constrained
- **Universal erase** — `E` key activates Erase mode for ANY active brush, not a separate "Erase Brush" preset (Krita pattern)

Add `tags: List<string>` field to `MapDesignerBrushPresetSO` IF not already present (Sprint 1 may have omitted; verify before extending).

### 9.5 Layout — UI Toolkit vs IMGUI

Modern Unity prefers UI Toolkit `TwoPaneSplitView` over IMGUI horizontal/vertical scopes for resizable panels. However: existing `RimaMapDesignerWindow` is IMGUI-based, and refactor cost may not be justified.

**Decision:** Stay with IMGUI for V1 (consistent with existing window). UI Toolkit migration is a V2 candidate. Use simple `EditorGUILayout.BeginHorizontal/Vertical` scopes with `GUILayout.Width()` for fixed panel widths (260 left / flex center / 280 right).

### 9.6 Hades workflow NOT directly applicable

Research found Hades uses pre-rendered 3D-to-2D sprite layers (V-Ray Toon, Bink video, custom C++ engine). This is not portable to our pure 2D pixel-art Unity stack. Our composite brush + executor router achieves the same artistic goal (layered organic environment) through different means. No Hades-specific implementation borrowing.
