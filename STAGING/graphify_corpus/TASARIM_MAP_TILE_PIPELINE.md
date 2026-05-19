# RIMA Map Tile Pipeline

Karar #116, #117, and #118 define the Room Designer tile pipeline:
PixelLab produces compact source assets, while Unity composes natural rooms through
separate Base, Decal, Wall, and Prop layers.

## 4-Layer Tilemap Stack

1. BaseTilemap
   - Uses RuleTile and RandomTile floor/path assets.
   - Walkable. TilemapCollider2D must stay off.
   - Holds Karar #116 terrain variants, with at least three weighted variants per terrain when RandomTile pools are used.

2. DecalTilemap
   - Transparent overlays: moss, rift cracks, dust, grime, ash, and small debris.
   - Walkable. TilemapCollider2D must stay off.
   - Sorts above BaseTilemap and below wall layers.

3. WallsTilemap_Front and WallsTilemap_Top
   - WallsTilemap_Front owns collision and blocks movement.
   - WallsTilemap_Top is visual/walkable top surface detail.
   - Wang 16-tile NSEW masks are the baseline. 47-tile exports are accepted by the wizard schema path, but 16-tile NSEW remains the portable minimum.

4. PropContainer
   - Props are individual GameObject prefabs, not painted tilemap cells.
   - Large decor, pillars, rubble, braziers, interactables, and shadows live here.

## TileImportWizard Usage

Open `RIMA > Tile Import Wizard`.

1. Select a PixelLab export folder.
2. Use `Import Folder`.
3. The wizard searches for `asset_000.json`, then `metadata.json`, then another top-level `.json`.
4. The selected PNG sheet is sliced with TextureImporter sprite mode Multiple.
5. A RuleTile is generated under `Assets/Art/Tiles/F1/Generated/`.
6. Use `Apply Demo Stack` to enforce the RoomPipelineTest scene hierarchy.

Batch-callable methods are also available:
- `RIMA.Editor.RoomDesigner.Tools.TileImportWizard.BatchCreateTemplate`
- `RIMA.Editor.RoomDesigner.Tools.TileImportWizard.BatchApplyDemoStack`
- `RIMA.Editor.RoomDesigner.Tools.TileImportWizard.BatchImportDefaultF1`
- `RIMA.Editor.RoomDesigner.Tools.TileImportWizard.BatchImportGeneratedTileAssets`

## PixelLab Export Formats

`create_topdown_tileset` metadata is supported through the PixelLab `tileset_data.tiles` schema:

```json
{
  "tileset_data": {
    "tile_size": { "width": 32, "height": 32 },
    "tiles": [
      {
        "name": "wang_0",
        "corners": { "NE": "lower", "NW": "lower", "SE": "lower", "SW": "lower" },
        "bounding_box": { "x": 0, "y": 0, "width": 32, "height": 32 }
      }
    ]
  }
}
```

`create_tiles_pro` flat metadata is supported through the wizard schema:

```json
{
  "tile_size": 32,
  "tile_type": "topdown_wang",
  "tiles": [
    { "index": 0, "wang_mask": "0000", "sprite_x": 0, "sprite_y": 0, "width": 32, "height": 32 }
  ]
}
```

For `tile_type` values such as `object` or `sidescroller`, the wizard slices sprites and creates plain Tile assets instead of RuleTiles.

## Portable Core Compliance

`RoomLayer` and `BrushMode` live in `Assets/RoomDesigner.Core/Runtime/RoomDesignerEnums.cs`.
Editor code maps those portable enum values to concrete Unity objects:
Base -> BaseTilemap, Decal -> DecalTilemap, Wall -> WallsTilemap_Front,
Prop -> PropContainer.

Biome-specific RuleTiles, palette choices, template assets, and scene GameObjects stay in the Game/Editor layer.
