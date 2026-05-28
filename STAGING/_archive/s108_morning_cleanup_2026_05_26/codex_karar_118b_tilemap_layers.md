# Karar #118b — 4-layer tilemap iskelet + Demo scene — execute every step, commit at end

## Context

Karar #118 3 parçaya bölündü. Bu #118b: Demo scene'e 4-layer tilemap iskelet + Room Designer brush mode dropdown.

#118a (TileImportWizard) ayrı dispatch'te tamamlandı veya paralel gidiyor.

## STEP 1 — Read existing Demo scene structure

Read `Assets/Scenes/Demo/RoomPipelineTest.unity` to understand current tilemap hierarchy.
Also read `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` to understand brush controller.

## STEP 2 — 4-Layer tilemap iskelet (Demo scene)

In `Assets/Scenes/Demo/RoomPipelineTest.unity`, modify the Grid hierarchy to have:

```
Grid (existing)
  BaseTilemap        (sortingLayerName "Default", sortingOrder 0, TilemapCollider2D OFF)
  DecalTilemap       (sortingOrder 1, TilemapCollider2D OFF, Decal transparency overlay)
  WallsTilemap_Front (sortingOrder 2, TilemapCollider2D ON, CompositeCollider2D)
  WallsTilemap_Top   (sortingOrder 3, TilemapCollider2D OFF — walkable surfaces)
  PropContainer      (empty GameObject, prop prefabs instantiated here at runtime)
```

Steps:
- Rename existing FloorTilemap → BaseTilemap (if exists)
- Rename existing WallsTilemap → WallsTilemap_Front (if exists)
- Add new: DecalTilemap, WallsTilemap_Top, PropContainer
- Use `manage_scene` or `manage_gameobject` MCP tools to add GameObjects
- Alternatively use UnityMCP `execute_code` to create tilemap GameObjects programmatically

## STEP 3 — Room Designer brush mode dropdown

In `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs`:
- Add enum `BrushLayerMode { Base, Decal, Wall, Prop }` (in file or Core namespace)
- Add SerializedField / local field `BrushLayerMode _brushMode`
- In OnGUI: toolbar or dropdown to select brush mode
- Pass selected tilemap to brush controller based on mode:
  - Base → BaseTilemap
  - Decal → DecalTilemap
  - Wall → WallsTilemap_Front
  - Prop → PropContainer (show popup note "Props use prefab drag-drop")
- Karar #117 compliance: BrushLayerMode enum can live in Editor layer (no Core dependency needed for enum)

## STEP 4 — Compile + test

- `read_console` — 0 errors required
- Open Demo scene in Unity, verify 5 tilemap objects visible in Hierarchy

## STEP 5 — Commit

```bash
git add Assets/Scenes/Demo/RoomPipelineTest.unity Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs
git commit -m "[karar118b] 4-layer tilemap stack + brush mode dropdown

- BaseTilemap / DecalTilemap / WallsTilemap_Front / WallsTilemap_Top / PropContainer
- Room Designer brush mode dropdown (Base/Decal/Wall/Prop)
- Antigravity 4 P0.3 Wall_Front + Wall_Top split alignment
- Karar #117 portable core compliance"
```

## STEP 6 — Report

Write `STAGING/karar_118b_tilemap_layers_report.md`:
```
# Karar #118b 4-Layer Tilemap Report

## Tilemap layers
[5 layers in Demo scene Y/N]
[sortingOrder correct Y/N]

## Brush mode dropdown
[enum added Y/N, dropdown works Y/N]

## Console
[0 errors Y/N]
```

Append CODEX_DONE.md:
```
## [2026-05-14] Karar #118b 4-layer tilemap + brush mode
- 5 tilemap layers in Demo scene: Y/N
- Brush mode dropdown: Y/N
- Compile clean: Y/N
```

## Constraints

- Do NOT rewrite TileImportWizard (that is #118a)
- Do NOT delete existing tilemap data — rename carefully, preserve tile data
- If scene binary conflict: use `execute_code` or `manage_gameobject` MCP tool for scene edits
- Keep BrushLayerMode enum in RimaRoomDesignerWindow.cs (not a separate Core file — simple enum, not worth splitting)

## Source References

1. `Assets/Scenes/Demo/RoomPipelineTest.unity` — Demo scene (mevcut)
2. `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` — brush window
3. `Assets/Editor/RoomDesigner/` — diğer brush/controller scriptler
