# RIMA Visual Map Designer (Level Editor) - Guide

This document explains the features, file structure, and technical layout of the new visual level editor tool built for RIMA.

---

## 📂 Where is the Tool?

All scripts and documentation files for this editor are isolated in the following folder to ensure it doesn't interfere with RIMA's legacy brush window:
`Assets/Editor/MapDesigner/VisualEditor/`

### File Layout:
1. **[RimaVisualMapEditorWindow.cs](file:///f:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/MapDesigner/VisualEditor/RimaVisualMapEditorWindow.cs)**: 
   * Initializes the dockable editor window and draws the semi-transparent floating Overlay Panel directly in the `SceneView` viewport.
2. **[VisualEditorScenePainter.cs](file:///f:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs)**: 
   * Manages mouse clicks/drags, converts screen coordinates to snapped isometric grid cells, renders preview ghosts, and dispatches strokes to the native engine.
3. **[AutoLayeringService.cs](file:///f:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/MapDesigner/VisualEditor/AutoLayeringService.cs)**: 
   * Resolves target tilemaps (`FloorTilemap` aka `"Tilemap"`, `WallTilemap`, `CliffTilemap`) and target game object folders (`PropsContainer`) dynamically based on active brush categories.
4. **[LiveAutotiler.cs](file:///f:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/MapDesigner/VisualEditor/LiveAutotiler.cs)**: 
   * Localizes autotiling and triggers `CliffAutoPlacer.Regenerate()` in real-time as the designer paints floor tiles.
5. **[DESIGN_LOG.md](file:///f:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/MapDesigner/VisualEditor/DESIGN_LOG.md)**: 
   * Persistent design document detailing goals, system architecture, progress logs, and structural maps.

---

## 🕹️ How to Open & Use it in Unity

1. **Open the window:**
   * Go to Unity's top menu and select: **`RIMA` ➔ `Visual Map Designer (New)`**.
2. **Dock the panel:**
   * Dock the window beside your SceneView or Inspector. It will auto-load the default Brush Pack (`BrushPackSO`) and Biome Skin (`BiomeSkinSO`) from your project.
3. **Select a Brush:**
   * Choose a category tab (Floor, Wall, Transition, Detail) and select your brush.
4. **Paint on the Grid:**
   * Hover over the SceneView. You will see a blue isometric snapped diamond outline tracking the active cell.
   * If the selected brush has sprites or prefabs, a translucent cyan **Ghost Preview** will float under the mouse.
   * Press **`R`** on your keyboard to rotate the preview ghost by 90 degrees before placement.
   * Scroll your **Mouse Wheel** in SceneView to increase or decrease the brush size dynamically.
   * Left-click and drag to paint tiles or scatter props.
5. **Erase Assets:**
   * Toggle to **`Erase`** mode on the SceneView overlay.
   * Drag your mouse over tiles or props to remove them cleanly within the visual radius.
6. **Undo edits:**
   * Press `Ctrl+Z` to undo the entire brush stroke at once.

---

## 🛠️ Technical Details & Native Pipeline Integration

To comply with RIMA's architectural guidelines, the editor routes all operations directly through the project's native engine:

1. **`BrushExecutorRouter` Routing:**
   * We do not instantiate tiles or objects manually. We call `BrushExecutorRouter.Dispatch(stroke, op, preset)` which dynamically handles biome skins, sprite weights, and native executors (`GridTileExecutor`, `FreeformDecalExecutor`, etc.).
2. **Dummy `RoomData` Context:**
   * Native executors check room boundaries and wall proximity. To support this in the editor, we build a dummy `RoomData` context with a large $500 \times 500$ walkable grid (all `true`) and assign it to the `BrushStroke`.
3. **Walkable Check Bypass:**
   * To enable painting at negative grid coordinates or outside the dummy room bounds, the painter temporarily disables walkable checks (`op.respectsWalkableMask = false`) during editor stroke dispatch, restoring the original value immediately after.
4. **Custom Rotation Injection:**
   * Placed game objects are post-processed post-dispatch to apply the user's custom SceneView rotation angle (`window.CurrentRotation`).
5. **Ghost Preview Fallback:**
   * The preview ghost system resolves first-prefabs and first-sprites from the active asset pool. Sprite-only pools are rendered by attaching a temporary sprite renderer component to the ghost object.
