# Room Painter — ALL-IN-ONE UX Specification

**Status:** DESIGN ONLY — Sonnet sub-agent (S110 Phase A, 2026-05-26)
**Scope:** Day 4-9 redesign of `Assets/Editor/RoomPainter/*` to eliminate every reason a designer would open Unity Inspector.
**Inputs:** Day 1-3 LIVE state, S110 Day 2 review (`STAGING/s110_phase_a_day2_review_sonnet.md`), Day 3 Sonnet review (R-rotate patch applied), cliff pivot memory (`cliff_pivot_manual_brush_2026_05_26.md`), `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs` reuse patterns.

---

## Section 1 — UX Vision Manifesto

### 1.1 What Day 1-3 delivers today
1. **Asset palette** (`RimaRoomPainterWindow`, 417 LOC) with folder scan, 64×64 thumbnail grid, 5 category chips (All/Floor/Cliff/Props/Parallax/Other) and a 2-tab Gameplay/Parallax mode switch (Pattern C).
2. **Keyword-based layer inference** (`RoomPainterAssetScanner`, 30+ keywords, first-match wins).
3. **SceneView placement** (`RoomPainterScenePlacer`, 380 LOC): ghost preview, single-click paint, drag-paint with `_paintedCells` dedupe, `R` 90° rotate, undo group, ScrollWheel variant cycling, parallax tier dropdown drives sortingOrder.
4. **Hardcoded post-paint config**: `Cliff_*` GameObject under active `Grid`, sortingLayer="Floor", order=5; or `Parallax_*` with `ParallaxLayer.factor` and order=-100-tier.

### 1.2 What Day 1-3 does NOT deliver (the gap the user named)
1. **No Inspector substitution.** To add Rigidbody2D, Collider2D, custom sortingLayer, Y-sort axis, parallax fine-tune, tag or physics layer the designer MUST select the painted GameObject and open Unity Inspector. The painter has a `DrawInspectorPanel` stub that only renders the literal string "Day 3 - inspector".
2. **No physics inference.** A `wall` and a `decal` come out of the painter with identical components (a bare `SpriteRenderer`). Block/no-block is implicit, not authored.
3. **No metadata persistence per asset.** `AssetEntry` is a transient struct rebuilt every scan; per-sprite overrides (custom pivot, custom collider shape, tint) survive only the current Editor session.
4. **No event-driven refresh.** New sprites added to the folder require a manual "Refresh" click. There is no `AssetPostprocessor` hook.

### 1.3 All-in-one paradigm — target state
The designer launches `RIMA/Room Painter`, drags from the left palette into the SceneView, the painted instance is **already correctly authored** (correct sortingLayer, Y-sort axis, optional Rigidbody2D, sized BoxCollider2D, parallax factor, physics layer, Unity tag). When the designer needs to override any of those, the **right-side inspector pane** of the same window is the only surface — Unity's default Inspector should never be opened during a paint session.

Paradigm references:
- **Tiled** (one window, layer list + tile palette + properties pane + status bar).
- **Aseprite** (single-canvas + dockable property strips, never modal).
- **Pyxel Edit** (drag-from-palette spawns committed tile, properties always visible).
- **Krita** (context-aware right panel reflects active selection).

The unifying principle: **paint = author**. The painter is not a placement tool; it is a fully scoped asset authoring tool that happens to use SceneView as its canvas.

### 1.4 Layout decision (3-pane vs multi-tab vs floating)
Chosen: **fixed 3-pane horizontal** (palette | minimap | inspector) with a top toolbar and bottom status bar. Multi-tab was rejected because every tab swap forces a mental reset; context-aware floating panel was rejected because designers move the SceneView a lot and a floating panel either follows the mouse (annoying) or stays parked (no different from the current sub-window). 3-pane is the Tiled/Aseprite canonical layout the designer audience already knows.

---

## Section 2 — Main Layout (ASCII Mockup)

```
+========================================================================================+
| [Gameplay] [Parallax] | Folder: Assets/Sprites/Environment [▼] | [Refresh] [⚙] [?] |
+----------------------------------------------------------------------------------------+
| [All] [Floor] [Cliff] [Wall] [Props] [Decals] [Lighting] [Parallax] [Other]  search:[__]
+================+================================================+======================+
| ASSET PALETTE  | SCENE PREVIEW MINIMAP (optional, collapsible)  | INSPECTOR            |
|   ~ 300 px     |   ~ middle, 200 px (toggle via toolbar ⚙)      |   ~ 350 px           |
|                |                                                |                      |
| [thumb][thumb] | +----------------------------------------+     | Identity ▼           |
| [thumb][thumb] | |                                        |     |  [128×128 preview]   |
| [thumb][thumb] | |   room overview                        |     |  Path: …/cliff_03    |
| [thumb][thumb] | |   (current scene snapshot, painted     |     |  Name: [Cliff03   ]  |
| [thumb][thumb] | |    cells highlighted, camera frustum   |     |                      |
| [thumb][thumb] | |    overlay)                            |     | Placement ▼          |
| [thumb][thumb] | |                                        |     |  Layer:   Cliff   ▼  |
| [thumb][thumb] | |                                        |     |  SortLyr: Floor   ▼  |
|                | +----------------------------------------+     |  Order:   [   5  ]   |
| Pages: 1 / 4   |                                                |  Y-sort:  [x]  axis -Y
| [<] [>]        |   tiny zoom in/out                             |  Pivot:   [center ▼] |
|                |                                                |                      |
|                |                                                | Physics ▼            |
|                |                                                |  Block:   [x] yes    |
|                |                                                |  Body:    Static  ▼  |
|                |                                                |  Shape:   Box     ▼  |
|                |                                                |  Size:    auto [edit]|
|                |                                                |  Trigger: [ ]        |
|                |                                                |  PhysLyr: Default ▼  |
|                |                                                |                      |
|                |                                                | Parallax ▼ (greyed)  |
|                |                                                |  Tier:    Near    ▼  |
|                |                                                |  Factor:  [0.65  ]   |
|                |                                                |  CamRel:  [x]        |
|                |                                                |  PixSnap: [x]        |
|                |                                                |                      |
|                |                                                | Visual ▼             |
|                |                                                |  Tint:    [#FFFFFF]  |
|                |                                                |  Material:URP 2D Lit |
|                |                                                |  Light:   Cast+Recv  |
|                |                                                |                      |
|                |                                                | Metadata ▼           |
|                |                                                |  Tags: [cliff,iso,#] |
|                |                                                |  Notes:[___________] |
+================+================================================+======================+
| Sel: cliff_03 | Layer: Cliff | Sort: Floor/5 | Tier: — | Hint: drag to scene, R rotate |
+========================================================================================+
```

### 2.1 Layout rules
- **Window minSize:** 1100 × 640 (current 840 × 420 is too narrow for 3-pane).
- **Pane resize:** vertical splitters between palette/minimap and minimap/inspector, persisted via `EditorPrefs` keys `RIMA.RoomPainter.PaletteWidth` / `InspectorWidth`.
- **Hide toggles:**
  - Minimap hidden by default for sub-1280px monitors; toggled from toolbar `⚙` menu.
  - Inspector can collapse to 32 px header strip (preserves vertical canvas estate when designer is purely placing).
- **Empty-state inspector:** centered helpbox "Select an asset in palette or click a placed object in scene to author."
- **Foldout state** per inspector section persisted in `EditorPrefs` (`RIMA.RoomPainter.Foldout.Physics` etc.).

---

## Section 3 — Inspector Panel: All-In-One Asset Properties (CRITICAL)

The inspector is the heart of the redesign. It binds to a **single editing target** with two possible source types:

- **Palette target** — designer clicked a thumbnail. Edits go to the underlying `RoomPainterAsset` SO (Section 4). Affects future placements and propagates to all instances that point at the SO (designer can opt out per-instance).
- **Scene target** — designer clicked a placed GameObject in SceneView (via the Pick tool, Section 7). Edits go to the instance only and are recorded into the `RoomData.PlacementRecord` overrides list when the room is saved.

The mode is indicated by a banner at the top of the inspector: "Editing SO: cliff_03 (affects all instances)" in blue vs "Editing instance: Cliff_cliff_03 (123, 456) (overrides SO)" in orange. A "Push to SO" button on the instance banner promotes the override into the SO; a "Reset to SO" button reverts the instance.

### 3.A Identity
- **Asset path** (read-only text field).
- **Preview thumbnail** 128 × 128 using `AssetPreview.GetAssetPreview` with the async-loaded fallback to `GetMiniThumbnail` (today's pattern in `RimaRoomPainterWindow.DrawAssetButton` is reused).
- **Display name override** (string). Used for the `GameObject.name` when painted; defaults to the underlying asset name.
- **Asset GUID** (read-only, displayed in monospace, for cross-referencing logs).

### 3.B Placement
- **Default Layer** — `RoomLayer` enum popup (Floor/Edge/Cliff/Wall/Props/Decals/Lighting/Collision/Occlusion/Parallax). Pre-populated by `RoomPainterAssetScanner.InferLayer`. Designer override stored on the `RoomPainterAsset` SO.
- **Default Sorting Layer + Order** — both Day 4's `RoomLayerData` SO (one per RoomLayer, project-scoped) and the per-asset override. The inspector first reads `RoomLayerData.sortingLayerName` and `defaultOrder`, then overlays the asset-specific override if non-empty.
- **Y-sort enabled** — toggle. Default `true` for Cliff/Wall/Props, `false` for Floor/Parallax. When ON, the painted instance gets a runtime Y-sort component (Day 4 introduces `RoomYSorter` runtime); axis defaults to **-Y** (3/4 top-down convention, Karar #150 + `project_high_top_down_3_4_lock_2026_05_24.md`).
- **Y-sort axis** — sub-control, only visible when Y-sort enabled. Three options: `-Y` (default), `+Y`, `+Z`.
- **Pivot anchor** — popup `Bottom / Center / Top / Custom`. Default value is auto-detected from the sprite's pivot at import (`SpriteImportData.pivot`). Custom shows a Vector2 field (normalized 0..1).
- **Scale override** — Vector2, default (1,1).
- **Visual offset** — Vector2, applied after grid snap. Useful for cliffs that need to sit 0.1 units up to align with iso diamond face.

### 3.C Physics (Rigidbody2D + Collider2D) — the user's headline ask
This section is the explicit answer to "rigidbody2body'i unity üzerinden ayarlamak yerine kendi içindeki bi mantıkla ben ayarlasam".

- **Block toggle** (`bool`). The single most important field. UI label: "Block — is this a physics blocker?". Default value is inferred from Section 5's keyword table; designer can override per-asset.
- **When Block = NO:** all subsequent physics controls are greyed and the painted instance ships with **no** Rigidbody2D and **no** Collider2D (matching current behavior).
- **When Block = YES, the following authoring controls appear:**
  - **Body type** popup: `None (collider only) / Static / Dynamic / Kinematic`. Default `Static` for environment, `Dynamic` only when the asset name keyword hits `enemy|npc`. `None` means the painter adds Collider2D but no Rigidbody2D — appropriate for tilemap-style static obstacles when a Rigidbody2D would be redundant.
  - **Collider shape** popup: `Box / Circle / Capsule / Polygon`. Default `Box`. `Capsule` for character-shaped assets (enemy/npc).
  - **Size — auto / manual.** A "Auto from sprite bounds" toggle that reads `sprite.bounds.size` and shrinks by a configurable margin (default 0.85× to keep the collider inside the sprite outline). Manual mode exposes Vector2 size + Vector2 offset.
  - **Trigger** toggle. Auto-enabled when keyword hits `pickup|item|coin|trigger|zone`.
  - **Physics Layer (Unity)** popup. Pulled from `UnityEditorInternal.InternalEditorUtility.layers`. Default `Default`; auto-set to `Obstacle` (project layer, S100-era) when keyword hits `wall|cliff|pillar|door`.

The painter's `RoomPainterScenePlacer.PaintCell` path is extended on Day 4-5 to invoke a new `RoomPainterPhysicsApplier.Apply(GameObject, RoomPainterAsset)` helper. The helper is also called by the inspector's "Re-apply physics to selection" button when the designer changes the SO after a room was already painted.

#### Reuse from `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs`
- `ColliderTemplateService` (sibling file, not yet read but referenced from `ColliderPainter`) likely already houses Box/Circle/Polygon collider authoring with undo. Use this service directly rather than duplicating logic.
- `ColliderPainter.SnapToPixel` + `Snap()` for the "Size — auto" margin snap to 1/PPU.
- Ghost preview color palette (`GhostFill`, `GhostEdge`) for the inspector's "Preview shape on selected instance" overlay.
- Reuse the drag-to-define-Box pattern in the Manual-size mode of the inspector: when the designer clicks "Draw collider manually" in the inspector, the SceneView temporarily enters `ColliderPainter`'s Box mode targeted at the selected instance, then returns to paint mode.

### 3.D Parallax
This section is **always visible** but **only enabled** when Default Layer = Parallax OR designer manually expands it for a non-parallax layer (e.g. they want a Mid-tier prop that floats slowly).

- **Tier** popup using existing `ParallaxTierNames` array (FG/Playable/Near/Mid/Far/Skyline/Horizon) and `ParallaxTierValues` from `RimaRoomPainterWindow`.
- **Factor (manual override)** Vector2 slider, range 0.01–1.50, default = the tier's value. Live-binds to the ghost preview in SceneView so the designer can iterate without leaving the window.
- **Camera relative** toggle.
- **Pixel snap** toggle.

When the designer changes the tier in the inspector, `RoomPainterScenePlacer` is notified via the existing `parallaxTierName/Value/Index` parameters that flow into `OnSceneGUI`.

### 3.E Visual
- **Tint** Color field, default white. Stored as `Color` on `RoomPainterAsset` SO; applied via `SpriteRenderer.color` at paint time.
- **Material** popup. Choices: `URP 2D Lit` (default), `URP 2D Unlit`, `Custom (Object field)`. Picked materials are resolved at paint time; missing custom shows a console warning.
- **Light interaction** tri-state popup: `Cast shadow / Receive light / Both`. Maps to `Renderer.shadowCastingMode` and a child `ShadowCaster2D` toggle (URP 2D pipeline).

### 3.F Metadata
- **Tags** — multi-string list (custom designer tags, not Unity tags). Used for in-painter search.
- **Notes** — free text. Useful when handing off rooms to other team members.

### 3.G Inspector empty / multi-select states
- Empty: helpbox + a "Recent assets" mini-grid (last 5 painted), so the designer can re-select quickly.
- Multi-select (Box-Select tool, Section 7): only the **common** fields are shown; non-common fields display as "—" placeholders with an "Edit individually" button.

---

## Section 4 — Auto-Import Classification (AssetPostprocessor)

When the designer drops a new PNG or prefab into `Assets/Sprites/Environment` (or any folder configured in the painter's `_folderPath`), the following pipeline runs **before** the painter window is even opened:

### 4.1 Pipeline
1. `RoomPainterAssetPostprocessor : AssetPostprocessor` overrides `OnPostprocessAllAssets(string[] imported, ...)`.
2. For each imported asset under any configured root (default `Assets/Sprites/Environment` and `Assets/Prefabs/Environment`):
   1. Skip if the asset is not a Sprite or GameObject (prefab).
   2. Skip `.meta`, `.png.meta` and animation files.
   3. Compute the **target metadata path**: `Assets/RoomPainter/AssetMetadata/<assetGUID>.asset` (GUID, not filename, so renames don't break the link).
   4. If a metadata SO already exists, refresh only the GUID-linked sprite/prefab reference (in case the underlying asset moved); do not overwrite designer overrides.
   5. If no metadata SO exists, create one:
      - `RoomPainterAsset` SO instantiated with `id = guid`, `displayName = assetName`.
      - `defaultLayer = RoomPainterAssetScanner.InferLayer(path)`.
      - Block / body / collider / physics-layer inferred via Section 5 table.
      - `defaultSortingLayer` / `defaultOrder` defaulted to the layer's `RoomLayerData` SO (Day 4 creates these — one SO per layer in `Assets/RoomPainter/LayerData/`).
      - `defaultScale = (1,1)`, `defaultVisualOffset = (0,0)`.
   6. `AssetDatabase.CreateAsset(so, metadataPath)`.
3. If a `RimaRoomPainterWindow` is open, fire a static `RoomPainterAssetEvents.OnAssetsChanged` event which causes the window to call `RefreshAssetCache()` and `Repaint()` — eliminating the current manual "Refresh" requirement. The window keeps the Refresh button as a fallback (graceful degradation).

### 4.2 Trade-offs
- **GUID-based metadata path:** robust to renames/moves but produces a non-human-readable file list. Acceptable because designers interact through the painter, not the Project window.
- **Postprocessor cost:** running keyword inference on every asset import is O(n × keywords) ≈ 35 string compares per asset. Negligible.
- **First-time bulk import:** when the designer adds the entire `Assets/Sprites/Environment` folder for the first time, the postprocessor will create hundreds of SOs. Mitigated by a "Bulk classify" menu item (`RIMA/Room Painter/Bulk Classify Folder…`) that runs the same logic in a single `AssetDatabase.StartAssetEditing()` / `StopAssetEditing()` window.

### 4.3 Refresh anti-pattern
**Do NOT** subscribe both to `AssetPostprocessor` AND a MonoBehaviour-side event that triggers refresh. Memory `feedback_double_auto_regen_avoid.md` documents this: in S109 the cliff auto-placer wired both `tilemapTileChanged` AND an Editor MouseUp hook and produced double-execute. The painter must funnel all "assets changed" notifications through a single static event bus and let the window subscribe once.

---

## Section 5 — Block / Physics Inference Keyword Table

Used by Section 4's postprocessor to seed default physics on new assets. First match wins (longer/more specific keywords come first, mirroring the existing `RoomPainterAssetScanner._layerKeywords` ordering pattern).

| # | Keyword                | Block | Body type | Collider shape | Trigger | Physics layer | Notes                                          |
|---|------------------------|-------|-----------|----------------|---------|---------------|------------------------------------------------|
| 1 | `wall`                 | YES   | Static    | Box            | NO      | Obstacle      | Hard environment block.                        |
| 2 | `cliff`                | YES   | Static    | Box            | NO      | Obstacle      | Cliff edge, paired with cliff pivot system.    |
| 3 | `pillar`               | YES   | Static    | Capsule        | NO      | Obstacle      | Round-section column.                          |
| 4 | `column`               | YES   | Static    | Capsule        | NO      | Obstacle      | Alias of pillar.                               |
| 5 | `door`                 | YES   | Static    | Box            | NO      | Obstacle      | Becomes Trigger when state-machine flips it.   |
| 6 | `altar`                | YES   | Static    | Box            | NO      | Prop          | Interactable, may receive trigger child later. |
| 7 | `brazier`              | YES   | Static    | Circle         | NO      | Prop          | Light source baked in.                         |
| 8 | `banner`               | NO    | —         | —              | —       | —             | Visual only, hangs from wall.                  |
| 9 | `prop`                 | YES   | Static    | Box            | NO      | Prop          | Generic prop default.                          |
|10 | `ritual`               | YES   | Static    | Box            | NO      | Prop          | Floor circle, but solid for navmesh.           |
|11 | `enemy`                | YES   | Dynamic   | Capsule        | NO      | Enemy         | Spawned via separate system, painter only places marker. |
|12 | `npc`                  | YES   | Dynamic   | Capsule        | NO      | NPC           | As above.                                      |
|13 | `pickup`               | NO    | —         | Box            | YES     | Pickup        | Trigger only.                                  |
|14 | `item`                 | NO    | —         | Box            | YES     | Pickup        | Alias of pickup.                               |
|15 | `coin`                 | NO    | —         | Circle         | YES     | Pickup        | Small circular trigger.                        |
|16 | `chest`                | YES   | Static    | Box            | NO      | Interactable  | Hard block until opened.                       |
|17 | `floor`                | NO    | —         | —              | —       | —             | Background, no physics.                        |
|18 | `decal`                | NO    | —         | —              | —       | —             | Pure visual.                                   |
|19 | `moss`                 | NO    | —         | —              | —       | —             | Decal subtype.                                 |
|20 | `crack`                | NO    | —         | —              | —       | —             | Decal subtype.                                 |
|21 | `parallax`             | NO    | —         | —              | —       | —             | Background plate.                              |
|22 | `bg`                   | NO    | —         | —              | —       | —             | Background plate.                              |
|23 | `rift`                 | NO    | —         | —              | —       | —             | Cyan rune art (`project_yarik_3scale_language`).|
|24 | `sky`                  | NO    | —         | —              | —       | —             | Far parallax.                                  |
|25 | `torch`                | YES   | Static    | Box            | NO      | Prop          | Wall-mounted, blocks pathing in 1 cell.        |
|26 | `lamp` / `light`/`glow`| NO    | —         | —              | —       | —             | Lighting-only.                                 |
|27 | `flame` / `ember`      | NO    | —         | —              | —       | —             | Lighting-only.                                 |
|28 | `trigger` / `zone`     | YES   | Static    | Box            | YES     | Trigger       | Explicit gameplay zone.                        |
|29 | `tile` / `wang16`      | NO    | —         | —              | —       | —             | Floor tile, never blocks.                      |
|30 | `dirt` / `sand` / `stone` / `cobble` | NO | — | — | — | — | Floor variants. |

Implementation note: the table lives in a single `RoomPainterPhysicsRules` static array (mirrors `_layerKeywords`) so designers / programmers maintain it in one place. Adding a row should be a one-line change.

---

## Section 6 — Drag-Drop Workflow (Palette → SceneView)

### 6.1 Current pattern
Designer clicks a palette button → `_selectedAsset` updates → SceneView click triggers `PaintCell`. Selection is **sticky**; SceneView clicks anywhere on the active Grid will keep painting until the designer changes selection.

### 6.2 Target pattern (additive, both supported)
- **Click selection** mode preserved (sticky behavior for repeated placement).
- **Drag from palette to SceneView** added. Implementation:
  - In `DrawAssetButton`, detect `EventType.MouseDrag` over the button: call `DragAndDrop.PrepareStartDrag()`, set `DragAndDrop.objectReferences = new[] { entry.AssetObject }`, set custom data key `RIMA.RoomPainter.AssetEntry` carrying the full `AssetEntry`, then `DragAndDrop.StartDrag("Paint " + entry.path)`.
  - In `RoomPainterScenePlacer.OnSceneGUI`, handle `EventType.DragUpdated` (set `DragAndDrop.visualMode = DragAndDropVisualMode.Copy` and reuse the existing `DrawGhost` to follow the mouse — the ghost is already mouse-follow during paint mode, so we route it through the same code).
  - On `EventType.DragPerform` over the SceneView, accept the drag (`DragAndDrop.AcceptDrag()`), call `PaintCell` once at the drop location, then exit the drag.
- **Multi-select palette** (Ctrl-click thumbnails): selected set held in `_multiSelection`. Dragging a multi-selection into SceneView runs `PaintCell` once per item using either a random scatter (Phase B, weighted by tag) or a row layout (Phase A simple default).

### 6.3 Reuse opportunities
- The ghost rendering already exists (`DrawGhost`, `DrawSpriteGhost`). Drag mode reuses these unchanged.
- The undo group machinery (`BeginUndoGroup` / `Undo.CollapseUndoOperations`) wraps the drop seamlessly.
- `ColliderPainter._isDragging` + `_dragStartWorld` + `_dragCurrentWorld` is the exact same drag-state-machine; lift the pattern, do not import the file.

### 6.4 Edge cases
- Dragging outside an active Grid: ghost shows "no active Grid" (existing label); drop is ignored.
- Drag canceled (Esc): `DragAndDrop.PrepareStartDrag()` is called again to reset state; ghost disappears.
- Drag started but designer switches focus to Hierarchy: Unity's `DragAndDrop` system handles the abort; painter releases its hooks via `OnDisable`'s existing `Reset()`.

---

## Section 7 — Erase / Pick / Box-Select Tools

Day 3 has only the implicit "click to paint" tool. Day 5 adds three more.

### 7.1 Tool palette UI
Top-left toolbar gets a 4-button tool group:
```
[ Paint ] [ Pick ] [ Erase ] [ Box-Select ]
```
Each button has a 1-letter shortcut (B / P / E / X) consistent with Aseprite/Photoshop muscle memory (B for brush, P for pick, E for erase, X reused for marquee). Active tool tint cyan (existing `CliffGhostTint` reuse).

### 7.2 Erase (E)
- Cursor changes to a red X overlay; ghost cube tint becomes red.
- MouseDown over a painted instance: raycast from mouse into scene via `HandleUtility.PickGameObject(e.mousePosition, true)`; if the hit GameObject is a child of the active Grid AND has a parent name starting with `Cliff_` or `Parallax_`, queue for deletion.
- MouseDrag erases along the path (uses the same `_paintedCells` dedupe but inverted — store deletes in a `_erasedInstances` set).
- MouseUp collapses the undo group.

### 7.3 Pick (P)
- Cursor: eyedropper icon.
- MouseDown: `HandleUtility.PickGameObject` → look up the painted GameObject's source asset by name suffix (`Cliff_<assetName>` → re-resolve via `_assetCache` lookup). If a `RoomPainterAsset` SO is attached as a `RoomPainterAssetBinding` component (added at paint time, Day 4), use that directly — much more robust than name parsing.
- Selected asset becomes `_selectedAsset`; inspector switches to "Editing instance" mode (Section 3) so the designer can immediately tweak the picked object.

### 7.4 Box-Select (X)
- Click-drag in SceneView draws a rectangle (Handles-drawn, existing wirecube pattern reused at 2D rect).
- On MouseUp, every painted instance with `transform.position` inside the rect joins `_multiSelection`.
- Inspector switches to multi-select mode (Section 3.G).
- Hotkeys while box-selection is active: Delete = erase all selected; Ctrl+D = duplicate at +1 cell offset; Arrow keys = move by one cell.

### 7.5 Lasso — Phase B
Free-form lasso uses `Handles.DrawAAPolyLine` to render the path while the mouse drags; point-in-polygon test on MouseUp. Defer to Phase B to keep Day 5 scope reasonable.

### 7.6 Tool state machine
A single `_activeTool` enum guards mouse events. All tools share the same `OnSceneGUI` entrypoint; the switch statement in `RoomPainterScenePlacer` becomes a delegate dispatch:
```
private readonly Dictionary<RoomPainterTool, IRoomPainterToolHandler> _handlers;
```
This is the only structural refactor needed in the placer. Existing paint code becomes `PaintToolHandler` implementing the interface.

---

## Section 8 — Save / Load + RoomData Integration

Day 6 wires the in-memory painted scene into the existing `RoomData` SO and `RoomLayerData` SO.

### 8.1 RoomData snapshot
A new toolbar button **"Save Room"** opens a popup:
- Field "Room ID" (string, populated from current scene name).
- Field "Display name" (string).
- "Target asset" — dropdown of existing `RoomData` SOs under `Assets/RoomPainter/Rooms/` plus a "New…" option.
- "Include layers" — multi-select of `RoomLayer` (default all 10).

On confirm:
1. Walk all GameObjects parented to the active `Grid`.
2. For each GameObject whose name matches `Cliff_*`, `Parallax_*`, or `Prop_*` (Day 5 naming convention extended), build a `PlacementRecord`:
   - `asset` — resolved by looking at the `RoomPainterAssetBinding` component (added at paint time in Day 4).
   - `worldPos` — `transform.position`.
   - `layer` — `RoomLayer` from the binding.
   - `orderOverride` — `SpriteRenderer.sortingOrder` if it differs from the layer default.
   - `scaleOverride` — `transform.localScale.xy` if non-(1,1).
3. Assign the placements list to the chosen `RoomData.placements`.
4. Save via `AssetDatabase.SaveAssetIfDirty(roomData)`.

### 8.2 RoomData load
**"Load Room"** dropdown lists existing `RoomData` assets. On select:
1. Modal: "Replace current painted layers? (Yes / Append / Cancel)".
2. If Replace: walk Grid children, delete those tagged as painter-spawned (the `RoomPainterAssetBinding` component is the marker).
3. For each `PlacementRecord` in the loaded `RoomData.placements`, call the standard `PaintCell` pipeline at `worldPos`, applying overrides.

### 8.3 Export prefab
**"Export Prefab"** captures the current Grid children as a prefab variant. Uses `PrefabUtility.SaveAsPrefabAsset` with the active Grid as the root. Produces `Assets/Prefabs/Rooms/<roomId>.prefab`.

### 8.4 Auto-save
Toggle in toolbar `⚙` menu: "Auto-save every 5 min". When enabled, an `EditorApplication.update` tick checks elapsed time and writes a sibling `<roomId>.autosave.asset` next to the canonical `RoomData`. On window open, if the autosave is newer than the canonical, prompt the designer.

### 8.5 RoomLayerData integration
The 10 `RoomLayerData` SOs live in `Assets/RoomPainter/LayerData/Layer_<Name>.asset`. They are auto-created on first window open if missing. The inspector's Placement section reads `RoomLayerData.sortingLayerName / defaultOrder / ySortEnabled / isCameraRelative` to fill the controls; an "Edit layer defaults" button on the inspector header opens the SO in a small modal (without leaving the painter window — the SO is inlined via `Editor.CreateEditor`).

---

## Section 9 — Day 4-9 Roadmap (Revised)

The previous Day 4-9 plan (D4 Layer/sorting/Y-sort, D5 Save/Load, D6-7 polish) is rewritten to absorb Sections 3-8 of this spec.

### Day 4 — Inspector + Auto-Import + Block Inference (BIG day)
**Deliverable:** Designer never opens Unity Inspector for layer/sorting/physics again. New assets auto-classify on drop.

Files created:
- `Assets/Editor/RoomPainter/RoomPainterAssetPostprocessor.cs` (Section 4 pipeline).
- `Assets/Editor/RoomPainter/RoomPainterPhysicsRules.cs` (Section 5 table).
- `Assets/Editor/RoomPainter/RoomPainterAssetEvents.cs` (single static event bus to defeat double-trigger pattern, ref `feedback_double_auto_regen_avoid.md`).
- `Assets/Editor/RoomPainter/RoomPainterPhysicsApplier.cs` (applies Rigidbody2D/Collider2D to painted GameObjects, uses `ColliderTemplateService` from PainterSuite where applicable).
- `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (Sections 3.A-G).
- `Assets/Editor/RoomPainter/Inspector/Sections/IdentitySection.cs`, `PlacementSection.cs`, `PhysicsSection.cs`, `ParallaxSection.cs`, `VisualSection.cs`, `MetadataSection.cs` (one file per foldout).
- `Assets/Scripts/RoomPainter/RoomPainterAssetBinding.cs` (runtime MonoBehaviour, links painted GameObject to its source `RoomPainterAsset` GUID — required by Pick/Save tools in Day 5/6).

Files extended:
- `Assets/Scripts/RoomPainter/RoomPainterAsset.cs` — adds `block`, `bodyType`, `colliderShape`, `colliderSizeAuto`, `colliderSizeManual`, `colliderTrigger`, `physicsLayer`, `tint`, `material`, `lightInteraction`, `ySortEnabled`, `ySortAxis`, `pivotAnchor`, `customTags`, `notes` fields.
- `Assets/Scripts/RoomPainter/RoomLayerData.cs` — already has `sortingLayerName / defaultOrder / ySortEnabled`. Add `ySortAxis` (enum).
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` — `DrawInspectorPanel` stub replaced with `_inspectorPanel.Draw()`.

Compile check: must pass with zero errors before Day 5 starts. Sonnet/Codex Day 4 review afterwards.

### Day 5 — Drag-Drop + Erase/Pick/Box-Select + Physics Applier wiring
**Deliverable:** Tool palette LIVE. Drag from palette spawns fully authored instance (with Day 4's physics applier). Erase/Pick/Box-Select all functional in SceneView.

Files created:
- `Assets/Editor/RoomPainter/Tools/IRoomPainterToolHandler.cs` (interface).
- `Assets/Editor/RoomPainter/Tools/PaintToolHandler.cs` (extracted from `RoomPainterScenePlacer`).
- `Assets/Editor/RoomPainter/Tools/EraseToolHandler.cs`, `PickToolHandler.cs`, `BoxSelectToolHandler.cs`.
- `Assets/Editor/RoomPainter/Tools/RoomPainterToolDispatcher.cs` (refactor of current placer dispatch).

Files extended:
- `RoomPainterScenePlacer.cs` becomes a thin shell that owns the dispatcher and the shared state (mouse world, snap grid).
- `RimaRoomPainterWindow.cs` toolbar — add tool buttons + shortcuts.

### Day 6 — Save / Load / Export Prefab / Auto-save
**Deliverable:** "Save Room" button populates `RoomData.placements`. "Load Room" reconstructs a saved room into the active Grid. Export prefab works. Auto-save tick LIVE.

Files created:
- `Assets/Editor/RoomPainter/RoomDataSerializer.cs`.
- `Assets/Editor/RoomPainter/RoomPainterAutoSave.cs`.
- `Assets/Editor/RoomPainter/Dialogs/SaveRoomDialog.cs`, `LoadRoomDialog.cs`.

Files extended:
- `Assets/Scripts/RoomPainter/RoomData.cs` — add `lastModifiedUtc`, `version` int.

### Day 7 — Parallax fine controls + Mini-map + Polish pass
**Deliverable:** Inspector's parallax section fully wired (live SceneView preview of factor changes). Mini-map collapsible pane added. UX polish: tooltips, foldout persistence, keyboard nav.

### Day 8 — Auto cliff edge brush + Hybrid manual override
**Deliverable:** From `cliff_pivot_manual_brush_2026_05_26.md` / Pattern C. Designer paints floor; cliff edge auto-suggests on perimeter; manual override paints the opposite side per the user's verbatim direction. Reuses the painter's tool dispatch from Day 5.

### Day 9 — Docs + Demo Room
**Deliverable:** `Assets/RoomPainter/Demo/HadesElysium_Demo.unity` showing a complete room painted entirely through the window with zero Unity Inspector touches. README per-tool in `Assets/Editor/RoomPainter/README.md`. Optional screen recording for designer onboarding.

### Risk gates between days
- After Day 4: Sonnet review of inspector + postprocessor; Codex regression pass on existing Day 3 paint workflow.
- After Day 5: Antigravity verification of drag/drop and tool dispatch (covers `feedback_antigravity_in_every_pipeline.md`).
- After Day 6: end-to-end test — paint a room, save, close Unity, reopen, load, verify byte-identical scene.

---

## Appendix A — Files that will NOT change

- `Assets/Scripts/Runtime/Parallax/ParallaxLayer.cs` (continues to be the runtime component the painter sets up).
- `Packages/com.laureth.painter-suite/Editor/Colliders/*.cs` (consumed read-only as a service; modifications go through the PainterSuite repo, not RIMA).
- Existing Day 3 keybindings (R rotate, ScrollWheel variant cycle) remain unchanged.

## Appendix B — Pattern C lock alignment

Per `cliff_pivot_manual_brush_2026_05_26.md`, the cliff workflow is hybrid auto+manual. This spec is fully compatible: Day 8's auto cliff edge brush is added as a tool handler in the Day 5 dispatch framework, and the manual override step is the same paint tool with a one-cell offset hint. No conflict with this redesign.

## Appendix C — Risks & mitigations

1. **Inspector pane explosion.** Six sections × multiple controls = ~30 fields. Mitigation: every section is a foldout, default-collapse all except Placement; persist foldout state in `EditorPrefs`.
2. **Postprocessor performance on huge first-time imports.** Mitigation: explicit "Bulk classify" menu item that uses `AssetDatabase.StartAssetEditing()`; postprocessor itself short-circuits when `EditorApplication.isUpdating` is true and queues a deferred pass.
3. **State drift between SO edits and live instances.** If the designer changes a SO field after a room is painted, instances do not auto-update. Mitigation: "Re-apply to instances" button in inspector header; document the asymmetry in Day 9 README.
4. **`RoomPainterAssetBinding` MonoBehaviour pollution.** Adding a component to every painted GameObject is mildly invasive. Mitigation: the component is empty at runtime (no Update/Awake cost), holds a single GUID string, and can be stripped at build time via a `[Conditional("UNITY_EDITOR")]` guard or build processor.

