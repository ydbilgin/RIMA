---
title: Codex Phase B-1 Audit - Asset Pack Browser
status: SPEC_AUDIT_COMPLETE
date: 2026-05-18
scope: read-only architecture audit
---

# Phase B-1 Architecture Audit

## Files inspected

- `CODEX_TASK_laurethayday.md`
- `STAGING/PLAN_FAKE3D_AND_UIUX.md`
- `F:/LaurethStudio/01_PIPELINE/auto_collider_from_sprite_pipeline.md`
- `STAGING/BRUSH_V1_WORKFLOW_EXPLAINED.md`
- `Assets/Editor/MapDesigner/Brush/MapDesignerBrushWindow.cs`
- `Assets/Editor/MapDesigner/Brush/Panels/BrushPalettePanel.cs`
- `Assets/Editor/MapDesigner/Brush/Panels/BrushSettingsPanel.cs`
- `Assets/Editor/MapDesigner/Brush/Panels/LayerVisibilityPanel.cs`
- `Assets/Editor/MapDesigner/Brush/Panels/BrushSceneTooling.cs`
- `Assets/Editor/MapDesigner/Brush/Hotkeys/BrushHotkeyHandler.cs`
- `Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs`
- `Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs`
- `Assets/Scripts/MapDesigner/Props/Editor/PropDefinitionPostprocessor.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/BrushPackSO.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/MapDesignerBrushPresetSO.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs`
- `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/PatchAtlasSO.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/PropDefinitionSO.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/RoomVisualProfileSO.cs`
- `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs`

Note: `memory/project_brush_v1_manual_composition_system.md` was requested but is not present at the supplied path. The closest available current-system context was `STAGING/BRUSH_V1_WORKFLOW_EXPLAINED.md`.

## 1. What the existing MapDesignerBrushWindow provides

`MapDesignerBrushWindow` is already a practical Brush V1 editor shell. It provides:

- A top bar for selecting `BrushPackSO`, `BiomeSkinSO`, and seed.
- Tool modes: Pick, Brush, Erase, Composite, Smart Fill, and Props.
- A brush palette panel filtered by brush category and text search.
- Brush settings for size, seed, active operation metadata, density, min distance, scale range, rotation/flip flags, and Karar #143 filters.
- Layer visibility and solo controls for `Layer_L1` through `Layer_L6`.
- Scene View integration through `BrushSceneTooling`.
- Brush and erase stroke dispatch through `BrushExecutorRouter`.
- Unity Undo grouping for brush strokes.
- Hotkeys for brush mode, erase mode, brush size, and brush slots.
- Bottom-bar actions for Auto-Dress, Regenerate Decor, Clear Decor, plus state readout.
- A Props mode that delegates to `PropsTab` and `PropPlacer`.

The existing Props mode provides:

- A prop palette scanning `Assets/Data/Brush/Props`.
- A target `RoomTemplateSO` field.
- A selected prop preview.
- Read-only placement rule display.
- Scene View click placement into `RoomTemplateSO.props`, not direct GameObject creation.
- Footprint validation against `CompositionRoleMap`.
- Rotation by `R`.
- Undo recording on the target template.
- GUID assignment for `PropDefinitionSO` via `PropDefinitionPostprocessor`.

The current system is closer to "brush/preset authoring plus room-template prop placement" than to a modern asset-pack browser. It is useful infrastructure, but it is not yet the requested asset-pack UX.

## 2. Missing pieces for Asset Pack Browser + Click-to-Place + Auto-Collider

### Asset Pack Browser gaps

- No unified `AssetPackManifestSO` concept.
- No pack switcher that groups v2, v3, and future PixelLab packs as browseable packs.
- No category tree for Floor, Macro, Decals, Scatter, Accents, Walls, Vertical Props, and Biome Floors.
- No sprite-level grid showing every sprite/variant directly.
- Existing brush grid selects `MapDesignerBrushPresetSO`, not raw sprites or atlas entries.
- Search is brush-name only, not pack/category/sprite/tag metadata search.
- No thumbnail metadata hover panel with sprite size, source atlas, role, collision defaults, or path.
- No explicit target count validation for the Phase B-1 pass gate: 84 v2 sprites plus 40 v3 asset sprites. Shell count found 84 v2 PNGs and 44 staged v3 PNGs, where 4 are preview sheets and 40 are real asset sprites.

### Click-to-place gaps

- Existing brush mode paints strokes through Brush V1 executors.
- Existing props mode writes placement records to `RoomTemplateSO.props`.
- There is no direct Scene View workflow where selecting a sprite gives a ghost sprite cursor and left-click instantiates a placed scene `GameObject`.
- There is no sprite-level placement controller independent of `RoomTemplateSO`.
- No direct right-click cancel behavior.
- No click-drag sprite stream for direct sprite placement.
- No selected sprite transform inspector with scale, alpha, flip, 90-degree rotations, and sorting override.
- No pixel-snapped ghost preview using PPU=32.
- No quick layer parenting policy for placed sprites.

### Auto-collider gaps

- The active Prop SO has `blocksWalkable`, but not the explicit auto-collider fields from the pipeline spec: `blocksMovement`, `colliderShape`, `colliderFootprintRatio`, `colliderOffset`, `isTrigger`, and `colliderLayer`.
- Current prop placement validates room-template footprint, but does not attach Collider2D components to placed GameObjects.
- No collider debug overlay.
- No collision layer creation/validation contract.
- No CompositeCollider2D optimization path for many static blockers.
- No acceptance tests that verify collider shape, size, offset, layer, trigger state, and undo/redo.

## 3. Extend existing window or build a new MapDesignerAssetBrowserWindow?

Recommendation: build a new `MapDesignerAssetBrowserWindow`, and reuse existing panels/controllers where they fit later.

Reasoning:

- `MapDesignerBrushWindow` is already responsible for Brush V1 painting, automation, and template prop placement. Extending it further risks turning one window into a mixed tool with conflicting mental models.
- Phase B-1 is read-only browsing, with a later direct click-to-place pipeline. That is a different user workflow from Brush V1 brush presets and `RoomTemplateSO` placement.
- A new window can be docked beside Scene View and can own an asset-pack-first layout: pack tree, sprite grid, selected sprite inspector, and placement status.
- The old Brush window should remain available for legacy brush workflows and automation.
- Shared lower-level classes can be extracted later only after B-1 proves the UX.

The new window should still interoperate with existing data:

- Read existing `BrushPackSO` and `AssetPoolSO` as legacy sources.
- Read existing `PatchAtlasSO` and `PropDefinitionSO` as map visual sources.
- Eventually push direct placements into either scene GameObjects or room-template data through explicit adapters.

## 4. SO dependencies and asset pack abstraction

`PatchAtlasSO` and `PropDefinitionSO` do not suffice as the asset-pack abstraction on their own.

Current `PatchAtlasSO`:

- Represents one atlas role and its variants.
- Has useful role metadata: BaseFloor, MacroPatch, OrganicDecal, DetailScatter, Accent.
- Does not represent a pack, category tree, pack version, source folder, source sprite counts, thumbnail rules, or future PixelLab import metadata.
- Does not currently include Walls or VerticalProps in `PatchRole`, and this task explicitly forbids extending that enum.

Current `PropDefinitionSO`:

- Represents one prop.
- Has identity, sprite, footprint, collision intent, sorting, and variant support in the active `RIMA.MapDesigner.Props` version.
- Does not group props into packs.
- Does not represent floor/decal/scatter/atlas sprites.
- Does not yet carry the explicit auto-collider config requested in the pipeline spec.

Current `BrushPackSO`:

- Groups brush presets and referenced pools.
- Good for Brush V1 operations, not for sprite-first browsing.
- Does not naturally expose v2/v3 source packs and categories in the requested UX.

Recommendation:

- Add a new `AssetPackManifestSO` in the implementation phase.
- It should group multiple sources without changing existing enum contracts:
  - `List<PatchAtlasSO> patchAtlases`
  - `List<PropDefinitionSO> props`
  - optional `List<AssetPoolSO> brushPools`
  - optional direct `Sprite[] looseSprites` for imported v3 folders before full SO authoring
- It should define categories at the manifest entry level, not by extending `PatchAtlasSO.PatchRole`.
- It should include source pack metadata: pack id, display name, version, source root, sprite count, thumbnail, tags, and import status.

This preserves backward compatibility while giving Phase B a clean pack-level browser.

## Audit verdict

Proceed with a new `MapDesignerAssetBrowserWindow`/`AssetPackBrowserWindow` spec. Do not mutate Brush V1 core for B-1. Treat B-1 as read-only browser foundation. Defer click-to-place and auto-collider implementation to B-2 after the orchestrator approves the spec.
