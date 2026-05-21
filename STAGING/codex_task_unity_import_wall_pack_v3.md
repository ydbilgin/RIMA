# Codex Task — Unity Import Wall Pack v3 + Tile Asset Creation

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed for RIMA visual context.

## Görev

22 sliced wall pack PNG'leri Unity'e import et + her biri için Tile asset oluştur.

## Input

`Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/` altında 22 PNG (sliced tiles).

## Görev Adımları

### Adım 1 — Sprite Import Settings (Batch via UnityMCP execute_code)

Her PNG için:
- **Texture Type:** Sprite (2D and UI)
- **Sprite Mode:** Single
- **Pixels Per Unit:** 64 (RIMA standard, matches floor tiles)
- **Filter Mode:** Point (no filter)
- **Compression:** None (sharp pixel art preservation)
- **Pivot:** Bottom Center (so walls sit on tile cell bottom)
- **Mesh Type:** Full Rect
- **Alpha Source:** Input Texture Alpha
- **Alpha is Transparency:** True

### Adım 2 — Tile Asset Creation (UnityEditor.Tilemaps.Tile per sprite)

Klasör: `Assets/Data/Tiles/Act1_ShatteredKeep/walls_v3/`

Her sprite için `.asset` Tile dosyası:
- `walls_v3/tile_archway_NE.asset`
- `walls_v3/tile_archway_SE.asset`
- ... (22 total)

Her Tile asset:
- Type: UnityEngine.Tilemaps.Tile
- Sprite: Linked to corresponding PNG sprite
- Color: white (default)
- Collider Type: Grid (or Sprite if precise collision needed)
- Transform Matrix: identity

### Adım 3 — Verification

- 22 sprite imported with correct settings
- 22 tile asset created and linked
- No console errors/warnings
- Sample: load 1 tile in tilemap, verify visual

## Python/C# Approach

UnityMCP `execute_code` ile C# script çağır:

```csharp
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

string spriteDir = "Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/";
string tileDir = "Assets/Data/Tiles/Act1_ShatteredKeep/walls_v3/";

// Ensure tile dir exists
if (!AssetDatabase.IsValidFolder(tileDir.TrimEnd('/'))) {
    AssetDatabase.CreateFolder("Assets/Data/Tiles/Act1_ShatteredKeep", "walls_v3");
}

string[] pngs = AssetDatabase.FindAssets("t:Texture2D", new[] { spriteDir });
int imported = 0, tilesCreated = 0;

foreach (var guid in pngs) {
    string path = AssetDatabase.GUIDToAssetPath(guid);
    string filename = Path.GetFileNameWithoutExtension(path);
    if (filename.StartsWith("_")) continue;  // skip contact sheet
    
    // 1. Import settings
    var imp = AssetImporter.GetAtPath(path) as TextureImporter;
    if (imp != null) {
        imp.textureType = TextureImporterType.Sprite;
        imp.spriteImportMode = SpriteImportMode.Single;
        imp.spritePixelsPerUnit = 64f;
        imp.filterMode = FilterMode.Point;
        imp.textureCompression = TextureImporterCompression.Uncompressed;
        imp.alphaIsTransparency = true;
        var settings = new TextureImporterSettings();
        imp.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;
        imp.SetTextureSettings(settings);
        imp.SaveAndReimport();
        imported++;
    }
    
    // 2. Create Tile asset
    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
    if (sprite != null) {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = Color.white;
        tile.colliderType = Tile.ColliderType.Grid;
        string tilePath = $"{tileDir}{filename}.asset";
        AssetDatabase.CreateAsset(tile, tilePath);
        tilesCreated++;
    }
}

AssetDatabase.SaveAssets();
AssetDatabase.Refresh();
Debug.Log($"Imported {imported} sprites, created {tilesCreated} tile assets.");
```

## Verification After Import

UnityMCP `read_console` ile:
- 0 error, 0 warning
- Log: imported + tilesCreated count

`manage_asset action="search"` ile tile asset listesi confirm

## Output Confirmation

- Import sprite count
- Tile asset count
- Console state (errors/warnings)
- Sample tile asset path verification

## Effort

medium — straightforward UnityMCP execute_code, batch processing.
