# Unified Designer UX, Organization & References Report (AGY - Design/UX Lens)

This report details the consolidation strategy, user experience layout, references, and asset/room organization for the **RIMA Unified Room Designer** tool. It provides a clean-canvas, dual-surface blueprint that works identically in both the Unity Editor and the in-game (F2) overlay.

---

## 1. Reference Tools (Dual-Surface & In-Game Editors)

To achieve a tool that feels tactile, fast, and satisfying, we draw UX concepts from the best level-editing interfaces in the industry:

1.  **Townscaper**: *Pure Context-Aware Tessellation*. 
    *   **The Idea to Steal**: The user never manually picks corners, endpoints, or T-junctions. They paint a generic "Wall" or "Floor" family, and the engine's `WangResolver` automatically evaluates neighbor occupancy (dirty 3x3 grid) to swap variants in real-time.
2.  **Super Mario Maker 2**: *Instant Playtest/Edit Swap*.
    *   **The Idea to Steal**: A single prominent button that instantly swaps between Edit mode and Play mode at the current camera/player focus point.
3.  **Tiled**: *Advanced Sub-Filtering Drawer*.
    *   **The Idea to Steal**: Fold the complex details (layer masks, sorting orders, collision configurations) into a collapsible "Advanced Settings" twirl-down to keep the workspace 90% clutter-free.
4.  **Hades (Supergiant's Room Editor)**: *Diegetic Depth Sorting & Axis Snapping*.
    *   **The Idea to Steal**: Prop/Decor placement snapped to cell coordinate centers but rendered using a custom-axis Y-sort to ensure characters and tall obstacles never bleed visually.
5.  **Dreams**: *Active Ghost Outline & Snapping affordance*.
    *   **The Idea to Steal**: Hovering showing a snapped, exact resolved preview of the piece (valid/invalid colored tint) and neighbor-joining reactions before clicking.

### Unified Palette & Canvas Mechanics
The best tools handle the palette, layers, and room switching by treating them as relational properties:
*   **Palette Buckets**: Simplified into exactly 3 horizontal tabs (**Floor**, **Wall**, **Prop**).
*   **Automatic Layer Selecting**: Selecting a Floor swatch automatically targets the Floor tilemap layer. Clicking a Wall swatch redirects brushes to the Wall cell grid. Manual layer toggles are hidden under the Advanced tab.
*   **Room Switcher**: A visual sliding sidebar showing room templates as baked thumbnail cards.

---

## 2. UX Layout for RIMA's Unified Tool

The design coordinates the exact same UI structure across both the **Unity Editor Window** and the **F2 In-Play Overlay** for zero mental friction.

```
+--------------------------------------------------------------------------+
|  [Room Library =]  Room: *Act1_RuinedKeep_03  [◀] [▶]  [Save] [PLAYTEST] |
+--------------------------------------------------------------------------+
|                                                              | Search... |
|                                                              | [MRU Grid]|
|                                                              +-----------+
|                                                              | [Floor]   |
|                         MAIN CANVAS                          | [Wall]    |
|                     (Big Isometric View)                     | [Prop]    |
|                                                              +-----------+
|                    [Hover Ghost Preview]                     | Swatches  |
|                                                              | Grid      |
|                                                              |           |
|                                                              |           |
|                                                              | [Advanced]|
+--------------------------------------------------------------------------+
| Hotkeys: LMB: Place/Connect | Drag: Run | RMB: Erase | Shift: Axis-Lock  |
+--------------------------------------------------------------------------+
```

### Layout Components
*   **Top Bar**: Houses the sliding Room Library drawer button, room name display (with `*` dirty indicator), cycle arrows, a Save disk, and a green Playtest/Resume button.
*   **Left Drawer (Room Library)**: Slides open to present 2-up room thumbnail cards, double-click to load, hover to duplicate or delete.
*   **Right Palette Panel**:
    *   *Search Bar*: Filters asset definitions by name substring.
    *   *MRU Row*: Displays the last 6 placed assets.
    *   *Core Tabs*: Floor, Wall, and Prop.
    *   *Swatches Grid*: Large visual previews.
    *   *Advanced Settings (Twirl-down)*: Hidden by default; holds manual layer filters, collision toggle, and height offset.
*   **Bottom Status Strip**: One-line keyboard and mouse hotkey hints.

### Interaction Snappiness (Game Feel)
*   **Honest Ghost**: Snapped to the active coordinate under the cursor, call `WangResolver.Resolve4` to preview connection shapes (e.g. wall corner) prior to mouse-click. Tints cyan-green `(0.4, 0.9, 1.0, 0.55)` for walkable/valid spots and warm-red `(1.0, 0.35, 0.3, 0.55)` for overlaps.
*   **Drag-Runs (Zoops)**: Click-and-hold LMB to drag a run. Renders projected Bresenham path ghosts and shows a length tag (e.g., `x7`).
*   **Place Juice**: A 1-frame white flash + a subtle size bounce on the sprite `(1.0 -> 1.08 -> 1.0)` over 0.12s when placed. Soft audio/tactile "tick" on cell crossings during drags.
*   **Erase Indicator**: Hovering over a placed asset in Erase mode overlays a red target outline.
*   **Single-click & Drag Flow**: LMB click = Place + Connect, LMB Drag = Connected run, RMB click = Erase.

---

## 3. Asset-Pack and Room-Library Organization

### Asset-Pack Palette Setup
*   **Generic ScriptableObject Structure**: An `AssetPack` SO coordinates groupings:
    *   *Floor Sets*: Houses the 4 tile types (Granite, Cyan-Vein, Dirt, Ritual).
    *   *Connector Sets*: Coordinates the 6 wall variants (Straight, Corner, T, Cross, End, Single) per Act theme.
    *   *Prop Sets*: Catalog of decor items and entities.
*   **Registry-First Loading**: The palette reads from the `RuntimeAssetRegistry` so assets resolve dynamically via GUIDs/names in both the Editor and Standalone builds.

### Room-Library Setup
*   **Mirror Files**: Rooms are authored as canonical `.asset` files in the Editor, and automatically mirror to runtime-safe `<roomId>.room.json` sidecars next to them. 
*   **Naming Conventions**: Strict `Act[N]_[RoomType]_[VariantDescriptor]` formatting (e.g., `Act1_Combat_ShatteredKeep_03`).
*   **Room Metadata**: Each template holds tags (e.g., `Treasure`, `Elite`, `Boss`, `Common`) so the Library drawer can filter lists by Act or Difficulty.

---

## 4. Clean-Room Authoring Flow (Floating Isometric Islands)

Building a seamless floating-island room follows an structured, linear pipeline:

1.  **Floor Painting**: Select the **Floor** tab. Paint the walkable shape. The tool automatically applies the 8-neighbor floor auto-edge borders using the 16 seamless PixelLab tiles.
2.  **Generate Cliffs**: Click the `Generate Cliff` action. The editor automatically sweeps the outer border coordinates of the Floor layer, inserting the corresponding cliff sides directly beneath on the Cliff tilemap.
3.  **Wall Structure**: Select the **Wall** tab. Drag-paint layout runs or click to place. The engine joins corners and T-junctions instantly.
4.  **Prop Placement**: Select the **Prop** tab. Stamp pillars, statues, and debris.
5.  **Entities & Lighting**: Stamp braziers and ritual tiles. The tool automatically maps the light emitters and cyan glow effects to the cell center.
6.  **Verify & Test**: Hit **Playtest** to verify walkability.

### Guard-Rails for Tidy Rooms
*   **Separated Layer Slots**: Hard-restricted tilemap/sorting layers:
    *   `Layer 0`: Void background (black/depth).
    *   `Layer 1`: Cliffs (auto-placement).
    *   `Layer 2`: Floor tilemap (walkable terrain).
    *   `Layer 3`: Walls (connecting solids).
    *   `Layer 4`: Props and Interactables (custom-axis Y-sorted entities).
    *   `Layer 5`: VFX/Lights.
*   **Collision Auto-Baking**: Placing wall/cliff structures automatically flags cells as non-walkable on the grid matrix.
*   **No Overlap Bleed**: Coordinate index prevents double-instantiating same-layer assets.

---

## 5. Consolidation Opinion

To solve the developer overlap and clutter, we establish **"RIMA Room Designer"** as the single authority and prune the legacy assets:

### What to Drop (Deprecate)
*   `MapDesignerBrushWindow` / `RimaVisualMapEditorWindow`: Clunky tile stampers that write no persistent RoomData.
*   `BlueprintPainterWindow`: Redundant; room blueprints are managed within the unified Room Library drawer templates.
*   `AssetPackBrowserWindow`: Redundant; fold its directory scanner directly into the palette sidebar panel.
*   `LiveToolPaletteWindow` / `LiveToolLauncher`: Completely redundant now that the F2 overlay in-game uses the exact same drawing logic and writes through the mirrored JSON bridge.
*   `TileImportWizard` (duplicate copies): Merge the two copies into a single helper and move it off the main Tools menu into the Project window context menus.

### The Unified Identity
*   **RIMA Room Designer** handles all cell occupancy modifications, asset palette selections, and room template loading.
*   All operations write to a unified, runtime-safe `RoomDataMutator` which updates the layout collections. The Editor window uses Unity’s `Undo` wrapping, and the F2 overlay uses its local JSON mirror bridge.
