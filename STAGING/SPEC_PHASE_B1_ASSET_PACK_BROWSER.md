---
title: Phase B-1 Spec - Asset Pack Browser
status: SPEC_READY_FOR_ORCHESTRATOR_REVIEW
date: 2026-05-18
scope: spec only, no implementation
---

# Phase B-1 Asset Pack Browser Spec

## 2.1 User stories

1. As a designer, I open the asset browser and see all available packs grouped by pack name, so I can choose between RIMA AssetParts v2, RIMA AssetParts v3, and future PixelLab packs without digging through Project folders.

2. As a designer, I select a category such as Walls, Vertical Props, Biome Floors, Accents, Decals, or Scatter, and the sprite grid immediately shows only the relevant sprites.

3. As a designer, I search for a term like "wall", "rift", "moss", or "column" and the browser filters the current pack/category without losing my selected sprite.

4. As a designer, I hover a sprite and see an enlarged preview plus metadata: sprite name, source pack, source atlas or prop definition, pixel size, PPU, category, default sorting, and collision default.

5. As a designer, I select a sprite from the browser and later click in Scene View to place it with a ghost preview that follows the cursor.

6. As a designer, I want auto-collider defaults when I place a wall or blocking prop, so the player cannot walk through it without manual collider setup.

7. As a designer, I can switch packs without corrupting undo/redo history or losing the ability to return to the previously selected sprite.

8. As a designer, I can browse 124 production sprites for the B-1 pass gate: 84 imported v2 sprites plus 40 real v3 asset sprites, excluding 4 v3 preview contact sheets.

## 2.2 UI layout

Target Unity window: `AssetPackBrowserWindow : EditorWindow`

Menu path:

```text
Tools/RIMA/Map Designer/Asset Pack Browser
```

Docking behavior:

- Default min size: 1100 x 620.
- Intended docking: left or right of Scene View.
- Window remains usable as a floating utility window.
- B-1 does not require custom overlays in Scene View, but reserves center/scene status space for B-2 placement state.

Text wireframe:

```text
+--------------------------------------------------------------------------------+
| RIMA Asset Pack Browser                    Pack: [RIMA_AssetParts_v2      v]   |
+-------------------------+-----------------------------------+------------------+
| LEFT: BROWSER           | CENTER: PREVIEW / SCENE STATUS    | RIGHT: INSPECTOR |
| width 260               | flexible                          | width 300        |
|                         |                                   |                  |
| Search [_____________]  | Selected sprite preview           | Selected Sprite  |
|                         | 256 x 256 max, checker bg         | Name             |
| Pack Tree               |                                   | Category         |
| - RIMA v2               | Placement status placeholder      | Source path      |
|   - Floors              | B-1: read-only                    | Pixel size       |
|   - Macro               | B-2: ghost/click status           | PPU              |
|   - Decals              |                                   | Sorting          |
|   - Scatter             | Hover metadata                    | Collision        |
|   - Accents             |                                   |                  |
| - RIMA v3               |                                   | Future controls: |
|   - Walls               |                                   | Variant slider   |
|   - Vertical Props      |                                   | Scale slider     |
|   - Biome Floors        |                                   | Alpha slider     |
|   - Accents             |                                   | Flip toggles     |
|                         |                                   | Collider config  |
+-------------------------+-----------------------------------+------------------+
| BOTTOM: SPRITE GRID                                                            |
| height 190-260, thumbnail size 48-96, virtualized scroll                        |
| [thumb][thumb][thumb][thumb][thumb][thumb][thumb][thumb][thumb][thumb]         |
+--------------------------------------------------------------------------------+
```

Panel dimensions:

- Left browser panel: 260 px fixed width.
- Right inspector panel: 300 px fixed width.
- Bottom sprite grid: 220 px default height, resizable later.
- Center preview/status: flexible width.
- Thumbnail size: 64 px default, user-adjustable 48-96 px.

B-1 controls:

- Pack dropdown.
- Search field.
- Category tree.
- Sprite thumbnail grid.
- Hover preview and metadata.
- Selected sprite inspector in read-only mode.

B-2 reserved controls:

- Placement mode indicator.
- Snap toggle.
- Collider debug toggle.
- Scene placement controller status.

## 2.3 Class architecture

```text
AssetPackBrowserWindow : EditorWindow
|-- AssetPackBrowserPanel
|   |-- CategoryTreeView
|   |-- SpriteGridView
|   `-- SearchBar
|-- ScenePlacementController
|   |-- GhostPreviewRenderer
|   |-- PlacementValidator
|   `-- ClickToPlaceHandler
|-- SelectedSpriteInspector
|   |-- VariantSlider
|   |-- ScaleSlider
|   |-- AlphaSlider
|   |-- FlipToggleRow
|   |-- RotationButtonRow
|   |-- SortingOrderField
|   `-- ColliderConfigPanel
|-- AssetPackCatalog
|   |-- AssetPackManifestLoader
|   |-- LegacyBrushPackAdapter
|   |-- PatchAtlasAdapter
|   `-- PropDefinitionAdapter
`-- UndoRedoManager
    `-- Unity.Undo backed
```

### `AssetPackBrowserWindow`

Responsibilities:

- Own window lifecycle.
- Store selected pack, category, search query, selected sprite entry, thumbnail size, and scroll positions.
- Draw top bar, panels, and bottom grid.
- Subscribe/unsubscribe Scene View callbacks in later phases.
- Keep B-1 read-only.

Non-responsibilities:

- No import mutation in B-1.
- No placement logic in B-1.
- No direct edits to existing Brush V1 data in B-1.

### `AssetPackCatalog`

Responsibilities:

- Build a browseable list of `AssetPackEntry` records from manifest assets and legacy sources.
- Provide filtered query results by pack, category, text search, and tags.
- Count visible/total sprites for the pass gate.
- Hide preview/contact-sheet images from production sprite counts when marked as previews.

### `AssetPackBrowserPanel`

Responsibilities:

- Draw pack selector.
- Draw category tree.
- Draw search.
- Provide current filter state to catalog.

### `SpriteGridView`

Responsibilities:

- Draw selectable thumbnails.
- Support keyboard-friendly selection later.
- Keep stable thumbnail cell dimensions.
- Expose hover entry for center preview and inspector.

### `SelectedSpriteInspector`

Responsibilities:

- B-1: show read-only selected sprite metadata.
- B-2: expose variant, scale, alpha, flip, rotation, sorting, and collider config.
- Never modify source SOs directly unless the implementation phase explicitly adds an edit mode.

### `ScenePlacementController`

B-1 status: skeleton only.

B-2 responsibilities:

- Convert selected sprite entry to a ghost preview.
- Snap mouse position to PPU grid when snap is on.
- Validate placement against scene geometry and collision rules.
- Instantiate placed `GameObject`.
- Attach SpriteRenderer and Collider2D.
- Register Unity Undo.
- Parent placed sprite under the correct layer container.

### `UndoRedoManager`

Responsibilities:

- Thin wrapper over Unity Undo.
- Name operations consistently.
- Collapse drag placement into a single Undo group.
- Keep pack/category browsing out of Undo history.

## 2.4 SO data model

### New: `AssetPackManifestSO`

Purpose:

- A pack-level source of truth for browser grouping.
- Groups v2, v3, and future PixelLab packs without changing existing Brush V1 contracts.

Proposed fields:

```csharp
public sealed class AssetPackManifestSO : ScriptableObject
{
    public string packId;
    public string displayName;
    public string version;
    public Texture2D coverImage;
    public string sourceRoot;
    public List<AssetPackCategory> categories;
    public List<PatchAtlasSO> patchAtlases;
    public List<RIMA.MapDesigner.Props.PropDefinitionSO> props;
    public List<AssetPoolSO> brushPools;
    public List<Sprite> looseSprites;
    public bool includeInBrowser = true;
}
```

Proposed supporting records:

```csharp
[Serializable]
public sealed class AssetPackCategory
{
    public string categoryId;
    public string displayName;
    public AssetPackCategoryKind kind;
    public string[] tags;
    public int defaultSortingOrder;
    public CollisionPreset collisionPreset;
}

public enum AssetPackCategoryKind
{
    Floor,
    Macro,
    OrganicDecal,
    DetailScatter,
    Accent,
    Wall,
    VerticalProp,
    BiomeFloor,
    AtmosphericAccent,
    Unknown
}
```

Do not extend `PatchAtlasSO.PatchRole` for this phase. Wall and VerticalProp categories live in the manifest layer.

### Existing `PatchAtlasSO`

Use as a legacy source for:

- BaseFloor
- MacroPatch
- OrganicDecal
- DetailScatter
- Accent

Backward compatibility rule:

- Existing PatchAtlas assets remain valid without a manifest.
- Catalog can synthesize a temporary "Legacy Patch Atlases" pack if no manifest references them.

### Existing `PropDefinitionSO`

Use as the source for placeable props.

Recommended field additions for implementation phase, following the auto-collider pipeline:

```csharp
public bool blocksMovement = false;
public ColliderShape colliderShape = ColliderShape.None;
[Range(0.3f, 1.0f)] public float colliderFootprintRatio = 0.7f;
public Vector2 colliderOffset = Vector2.zero;
public bool isTrigger = false;
public string colliderLayer = "Walls";

public enum ColliderShape
{
    None,
    Box,
    Circle,
    Capsule,
    PolygonAutoTrace
}
```

Compatibility mapping from current fields:

- Current `blocksWalkable == true` maps to default `blocksMovement == true`.
- Existing `footprintSize` remains room-template validation metadata.
- New collider fields define physical Scene View placement behavior.

### Existing `BrushPackSO` and `AssetPoolSO`

Use as optional legacy sources:

- `BrushPackSO` can identify high-level brush packs.
- `AssetPoolSO.sprites` and `AssetPoolSO.variants` can populate browser entries.
- Browser entries from Brush V1 data should remain read-only in B-1.

## 2.5 Auto-collider behavior contract

Auto-collider is B-2 implementation behavior, but B-1 must model and display its metadata.

### Placement trigger

When a selected placeable entry resolves to a prop or category with:

```text
blocksMovement == true
colliderShape != None
```

the placement pipeline attaches a Collider2D component to the placed GameObject.

### Shape mapping

```text
None             -> no collider
Box              -> BoxCollider2D
Circle           -> CircleCollider2D
Capsule          -> CapsuleCollider2D
PolygonAutoTrace -> PolygonCollider2D using sprite physics shape when available
```

### Footprint sizing

Inputs:

- `sprite.bounds.size` in local units.
- `colliderFootprintRatio`, range 0.3-1.0.
- `colliderOffset` in local units.

Box/Capsule size:

```text
collider.size = sprite.bounds.size * colliderFootprintRatio
collider.offset = sprite.bounds.center + colliderOffset
```

Circle radius:

```text
radius = min(sprite.bounds.size.x, sprite.bounds.size.y) * colliderFootprintRatio * 0.5
offset = sprite.bounds.center + colliderOffset
```

Default presets:

```text
Floor              blocks false, shape None
Macro              blocks false, shape None
OrganicDecal       blocks false, shape None
DetailScatter      blocks false, shape None
Accent             blocks false, shape None
Wall               blocks true,  shape Box,    ratio 1.0
VerticalProp       blocks true,  shape Box,    ratio 0.6
RoundVerticalProp  blocks true,  shape Circle, ratio 0.6
AtmosphericAccent  blocks false, shape None
```

### Layer assignment

- Blocking placements use `colliderLayer`, default `Walls`.
- If the layer is missing, implementation should warn and keep the GameObject on Default rather than failing placement.
- The B-2 setup checklist should verify the player collision matrix includes player-vs-Walls collision.

### Trigger behavior

- `isTrigger == false` for blocking walls and solid props.
- `isTrigger == true` only for interaction volumes or future special props.

### Parent and static policy

- Visual-only decals parent under the appropriate visual layer container.
- Blocking props parent under a blocking/props layer container.
- Static blockers should be marked static when authored as permanent room geometry.
- Rooms with more than 500 blocking colliders should be considered for CompositeCollider2D batching.

### Debug display

The inspector should reserve a "Show debug colliders" toggle for B-2/B-3:

- Red wireframe for blocking colliders.
- Amber wireframe for triggers.
- Gray wireframe for disabled or invalid collider config.

## 2.6 Acceptance tests (EditMode)

B-1 tests should focus on read-only catalog/browser behavior. B-2 tests should cover placement and colliders. The names below cover the full planned feature.

### 1. `AssetPackBrowserWindow_Open_DoesNotThrow`

Arrange: Ensure editor domain is loaded.

Act: Call `AssetPackBrowserWindow.Open()`.

Assert: Window instance exists and no exception is logged.

### 2. `AssetPackCatalog_LoadsManifestPack`

Arrange: Create an in-memory `AssetPackManifestSO` with one category and one sprite.

Act: Catalog loads manifests.

Assert: One pack and one sprite entry are returned.

### 3. `AssetPackCatalog_SynthesizesLegacyPatchAtlasPack`

Arrange: Create a `PatchAtlasSO` with two non-null variants and no manifest.

Act: Catalog scans legacy patch atlases.

Assert: A legacy pack entry exists with two sprite entries.

### 4. `AssetPackCatalog_ExcludesPreviewSheetsFromProductionCount`

Arrange: Provide 40 asset sprites and 4 sprites named `_preview_*`.

Act: Catalog counts production sprites.

Assert: Production count is 40 and total raw PNG-like entries can be 44.

### 5. `CategoryTree_SelectWalls_FiltersGrid`

Arrange: Catalog contains Wall and Accent entries.

Act: Select Wall category.

Assert: Grid model contains only Wall entries.

### 6. `SearchBar_FilterByName_IsCaseInsensitive`

Arrange: Catalog contains `wall_01`, `Rift_Fracture`, and `MossPatch`.

Act: Search for `rift`.

Assert: Only `Rift_Fracture` remains visible.

### 7. `SpriteGrid_SelectEntry_UpdatesInspector`

Arrange: Grid has one sprite entry.

Act: Select the entry.

Assert: Inspector model receives name, category, source path, pixel size, PPU, and collision preset.

### 8. `SpriteGrid_NullSprite_ShowsMissingState`

Arrange: Entry has a null sprite reference.

Act: Grid draws or builds tile view model.

Assert: Entry is marked missing and no exception occurs.

### 9. `ScenePlacement_SelectSprite_EnablesGhostPreview`

Arrange: Window has selected placeable sprite.

Act: ScenePlacementController enters placement mode.

Assert: GhostPreviewRenderer has a valid sprite and active state.

### 10. `ClickToPlace_CreatesGameObjectWithSpriteRenderer`

Arrange: Selected sprite entry and empty test scene.

Act: Simulate click at world position.

Assert: A GameObject exists at snapped position with SpriteRenderer.sprite assigned.

### 11. `ClickToPlace_BlockingWall_AddsBoxCollider2D`

Arrange: Selected Wall entry has `blocksMovement=true`, `colliderShape=Box`, `colliderFootprintRatio=1.0`.

Act: Place sprite.

Assert: GameObject has BoxCollider2D with expected size, offset, trigger state, and layer.

### 12. `ClickToPlace_RoundProp_AddsCircleCollider2D`

Arrange: Selected prop has `blocksMovement=true`, `colliderShape=Circle`, `colliderFootprintRatio=0.6`.

Act: Place sprite.

Assert: GameObject has CircleCollider2D radius derived from sprite bounds.

### 13. `PlacementValidator_OverlapExistingBlockingGeometry_ReturnsBlocked`

Arrange: Existing blocking collider occupies the target point.

Act: Validate placement for another blocking prop at the same point.

Assert: Validator returns blocked with an overlap reason.

### 14. `UndoRedo_PlacementUndo_RemovesPlacedObject`

Arrange: Place one sprite through ClickToPlaceHandler.

Act: Call `Undo.PerformUndo()`.

Assert: Placed GameObject is removed.

### 15. `UndoRedo_PackSwitch_DoesNotCreateUndoOperation`

Arrange: Window is open with one selected pack.

Act: Switch pack selection.

Assert: Unity Undo group is unchanged and no scene object is modified.

## 2.7 Edge cases and risks

### Empty pack

Risk: Browser appears broken.

Expected behavior: Show an empty-state message with pack name and "No sprites in this pack/category."

### Missing sprite reference

Risk: Null reference exceptions while drawing thumbnails or placing.

Expected behavior: Draw a missing thumbnail tile, disable placement, show source asset path if known.

### Preview sheets counted as sprites

Risk: B-1 pass count becomes 128 instead of 124.

Expected behavior: Manifest marks preview sheets as non-placeable, or catalog excludes names matching `_preview_*` from production count.

### Collider overlap with existing scene geometry

Risk: Designers can accidentally stack blockers and create unreachable room areas.

Expected behavior: B-2 validator warns or blocks based on placement mode. B-1 only documents the risk.

### Undo/redo across pack switches

Risk: Pack browsing creates undo noise or invalidates selected entries.

Expected behavior: Browsing is UI state only. Placement creates Undo records. If selected entry disappears after pack switch, selection is cleared with no scene mutation.

### Performance with 1000+ placed sprites

Risk: Many GameObjects and colliders can degrade Scene View and runtime physics.

Expected behavior: B-2 uses stable hierarchy parents and static flags. Later phase can batch blockers into CompositeCollider2D.

### Existing Brush V1 mental model conflict

Risk: Designers do not know whether to use Brush Tool or Asset Browser.

Expected behavior: Keep separate windows. Brush Tool is for procedural/brush operations. Asset Browser is for pack browsing and direct placeable sprites.

### Data model duplication

Risk: `AssetPackManifestSO`, `BrushPackSO`, `PatchAtlasSO`, and `PropDefinitionSO` overlap.

Expected behavior: Manifest is an index/grouping layer. It references existing SOs instead of replacing them.

### Enum pressure

Risk: Walls/VerticalProps need categories but `PatchAtlasSO.PatchRole` must not be extended.

Expected behavior: Category lives in `AssetPackManifestSO` entries/adapters, not in `PatchRole`.

## 2.8 Phased delivery checkpoints

### B-1 deliverable: read-only browser

Must include:

- New browser window opens from menu.
- Pack dropdown works.
- Category tree works.
- Search works.
- Sprite grid displays thumbnails.
- Hover/selection preview works.
- Inspector shows selected sprite metadata.
- Production sprite count reports 124: 84 v2 sprites plus 40 v3 asset sprites, excluding 4 v3 preview sheets.
- No Scene View placement yet.
- No auto-collider attachment yet.
- No modification of existing Brush V1 data.

B-1 PASS criteria:

- Orchestrator opens the window.
- Orchestrator clicks through all categories.
- Orchestrator sees all 124 production sprites.
- Search filters visible sprites without errors.
- Empty/missing sprite states do not throw.

### B-2 deliverable: click-to-place plus auto-collider

Must include:

- Select sprite from browser.
- Ghost preview follows Scene View mouse.
- Pixel snap to PPU=32 grid.
- Left-click places sprite GameObject.
- Right-click cancels selection.
- Blocking entries attach Collider2D using auto-collider config.
- Unity Undo removes placed objects.

### B-3 deliverable: selected sprite inspector and erase

Must include:

- Variant slider.
- Scale slider.
- Alpha slider.
- Flip toggles.
- Rotation buttons.
- Sorting order field.
- Collider config display/edit flow.
- Eraser mode.

### B-4 deliverable: power user placement

Must include:

- Drag placement stream.
- Selection rectangle.
- Quick-pick from scene.
- Save/load room preset integration.
- Snap/free placement toggle.
- Mirror mode.

### B-5 deliverable: composition assist

Must include:

- Rule-of-thirds guide.
- Density heatmap.
- Optional z-walking suggestion layer.

## Implementation guardrails for next phase

- Do not modify `PatchAtlasSO.PatchRole`.
- Do not alter Phase 1.5 data-first executors.
- Keep B-1 read-only.
- Keep `MapDesignerBrushWindow` intact.
- Prefer adapters over migrations.
- Use `.cs` only in implementation phase; this task writes `.skeleton` only.
