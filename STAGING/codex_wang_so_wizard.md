# Codex Task: Wang SO Wizard + 4 Remaining Tileset SOs

## Context
RIMA 2D top-down roguelite, Unity 6, URP 2D.

CornerWangTileSetSO: `Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs`
- `TileBase[] tiles` — 16 slot, index = corner key = (NW<<3)|(NE<<2)|(SW<<1)|SE
- `string lowerTerrainLabel`, `string upperTerrainLabel`
- `GetTile(nw,ne,sw,se)` → `tiles[(nw<<3)|(ne<<2)|(sw<<1)|se]`

Mevcut çalışan SO'lar (referans al): 
- `Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset` (16/16 tiles dolu)
- `Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset` (16/16 tiles dolu)
- `Assets/Art/Tiles/F1/Generated/wang_floor_wall_tile_0.asset` ... `wang_floor_wall_tile_15.asset`

Spritesheet format: 4×4 grid, her tile 32×32px. Sprite'lar sol-sağ üst-alt sırasıyla index 0-15.

tileset_meta.json formatı (her tileset klasöründe var):
```json
{
  "name": "floor_wall",
  "tileSize": 32,
  "upper": "broken stone wall",
  "lower": "rubble floor",
  "gridRows": 4, "gridCols": 4,
  "cornerKeyToSpriteIndex": [6,7,10,9,2,11,4,15,5,14,1,8,3,0,13,12]
}
```
`cornerKeyToSpriteIndex[cornerKey]` = spritesheeteki sprite indexi.
Yani: `tiles[cornerKey] = sprite[cornerKeyToSpriteIndex[cornerKey]]`

## Görev 1: 4 Remaining Tileset SO Oluştur

Şu 4 tileset klasörü için SO oluştur:
- `Assets/Art/Tiles/F1/Tilesets/debris_rift/`   → `DebrisRift_CornerWangTileSet.asset`
- `Assets/Art/Tiles/F1/Tilesets/cold_floor_wall/` → `ColdFloorWall_CornerWangTileSet.asset`
- `Assets/Art/Tiles/F1/Tilesets/slate_mineral/`  → `SlateMineral_CornerWangTileSet.asset`
- `Assets/Art/Tiles/F1/Tilesets/mauve_hexagon/`  → `MauveHexagon_CornerWangTileSet.asset`

Her biri için adımlar:

**a) Spritesheet'i Multiple sprite olarak import et:**
```csharp
TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(spritesheetPath);
importer.textureType = TextureImporterType.Sprite;
importer.spriteImportMode = SpriteImportMode.Multiple;
importer.filterMode = FilterMode.Point;
importer.textureCompression = TextureImporterCompression.Uncompressed;
// tileSize = 32 (meta.json'dan)
int gridCols = 4, gridRows = 4;
var rects = new List<SpriteMetaData>();
for (int row = 0; row < gridRows; row++) {
    for (int col = 0; col < gridCols; col++) {
        int idx = row * gridCols + col;
        rects.Add(new SpriteMetaData {
            name = name + "_" + idx,
            rect = new Rect(col * tileSize, (gridRows - 1 - row) * tileSize, tileSize, tileSize),
            pivot = new Vector2(0.5f, 0.5f)
        });
    }
}
importer.spritesheet = rects.ToArray();
AssetDatabase.ImportAsset(spritesheetPath, ImportAssetOptions.ForceUpdate);
```

**b) 16 sprite yükle (Y'ye göre desc, X'e göre asc = index 0=topleft):**
```csharp
var allSprites = AssetDatabase.LoadAllAssetsAtPath(spritesheetPath)
    .OfType<Sprite>()
    .OrderBy(s => -(int)s.rect.y)
    .ThenBy(s => (int)s.rect.x)
    .ToArray();
```

**c) 16 Tile asset oluştur:**
```csharp
for (int cornerKey = 0; cornerKey < 16; cornerKey++) {
    int spriteIdx = cornerKeyToSpriteIndex[cornerKey];
    var tile = ScriptableObject.CreateInstance<Tile>();
    tile.sprite = allSprites[spriteIdx];
    string tilePath = $"Assets/Art/Tiles/F1/Generated/wang_{name}_tile_{cornerKey}.asset";
    AssetDatabase.CreateAsset(tile, tilePath);
}
```

**d) CornerWangTileSetSO oluştur:**
```csharp
var so = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
so.lowerTerrainLabel = meta.lower;
so.upperTerrainLabel = meta.upper;
so.tiles = new TileBase[16];
for (int cornerKey = 0; cornerKey < 16; cornerKey++) {
    string tilePath = $"Assets/Art/Tiles/F1/Generated/wang_{name}_tile_{cornerKey}.asset";
    so.tiles[cornerKey] = AssetDatabase.LoadAssetAtPath<TileBase>(tilePath);
}
AssetDatabase.CreateAsset(so, $"Assets/Art/Tiles/F1/Generated/{pascalName}_CornerWangTileSet.asset");
```

Bu 4 işlemi tek bir `[MenuItem("RIMA/Tools/Create Missing Wang SOs")]` script'ine koy.
Script: `Assets/Editor/CreateMissingWangSOs.cs`

## Görev 2: WangTileSetWizard EditorWindow
`Assets/Editor/WangTileSetWizard.cs` oluştur. Menu: `RIMA/Tools/Wang Tileset Wizard`

```
Window alanları:
- Texture2D sourceTexture (object field)
- string lowerLabel, upperLabel (otomatik meta.json'dan doldur)
- [Button] "Load from tileset_meta.json" → texture'ın yanındaki .json oku, lower/upper doldur
- [Button] "Create SO" → Görev 1'deki adımları tek texture için yap
- Oluşturulduktan sonra: "✓ SO created: {path}" label göster
```

## Okuyacağın dosyalar
- Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs
- Assets/Art/Tiles/F1/Tilesets/debris_rift/tileset_meta.json
- Assets/Art/Tiles/F1/Tilesets/cold_floor_wall/tileset_meta.json
- Assets/Art/Tiles/F1/Tilesets/slate_mineral/tileset_meta.json
- Assets/Art/Tiles/F1/Tilesets/mauve_hexagon/tileset_meta.json

## Compile check
read_console 0 error doğrula. Error varsa düzelt.

## Commit
```
git add Assets/Editor/CreateMissingWangSOs.cs Assets/Editor/WangTileSetWizard.cs Assets/Art/Tiles/F1/Generated/
git commit -m "[wang-wizard] 4 remaining tileset SOs (DebrisRift+ColdFloorWall+SlateMineral+MauveHexagon) + WangTileSetWizard"
```
