# Codex Task: Corner Wang Painter — PixelLab Tileset → Unity Map Generation

## Context

RIMA is a 2D top-down Unity roguelite. We have PixelLab-generated Wang tilesets locally.

**Existing assets (already in project):**
- `Assets/Art/Tiles/F1/wang_floor_wall.png` — 128×128px, 4×4 grid, 16 Wang tiles (floor→wall blend)
- `Assets/Art/Tiles/F1/Generated/wang_floor_wall_tile_0.asset` through `wang_floor_wall_tile_15.asset`
- `Assets/Art/Tiles/F1/wang_rubble_path.png` + `wang_rubble_path_tile_0.asset` through `_15.asset`

**Tile asset index convention:** These were sliced from the spritesheet row by row.  
`tile_N` = sprite at row=N/4, col=N%4 → bounding_box x=(N%4)*32, y=(N/4)*32.

## PixelLab Corner Wang System

All PixelLab topdown tilesets use the SAME corner→sprite mapping:

- **lower** terrain = 0 (floor/rubble)  
- **upper** terrain = 1 (wall/path)  
- **Key encoding:** `key = (NW << 3) | (NE << 2) | (SW << 1) | SE`  
  where NW/NE/SW/SE are the corner terrain values (0 or 1)

**Lookup table — key (0..15) → sprite asset index (0..15):**
```
int[] cornerKeyToSpriteIndex = { 6, 7, 10, 9, 2, 11, 4, 15, 5, 14, 1, 8, 3, 0, 13, 12 };
```
Example: key=0 (all lower/floor) → sprite index 6 → `wang_floor_wall_tile_6`  
Example: key=15 (all upper/wall) → sprite index 12 → `wang_floor_wall_tile_12`

## Vertex Grid System

For a W×H tile room, define a `(W+1)×(H+1)` vertex grid:  
- Each vertex value: 0 = lower/floor, 1 = upper/wall  
- For tile at Unity cell `(tx, ty)`:
  - NW corner = `vertGrid[tx,   ty+1]`
  - NE corner = `vertGrid[tx+1, ty+1]`
  - SW corner = `vertGrid[tx,   ty  ]`
  - SE corner = `vertGrid[tx+1, ty  ]`

## Files to Create

### 1. `Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs`

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    [CreateAssetMenu(fileName = "CornerWangTileSet", menuName = "RIMA/Corner Wang Tile Set")]
    public class CornerWangTileSetSO : ScriptableObject
    {
        [Tooltip("16 tiles indexed by spritesheet position (row*4+col). Assign wang_X_tile_0 through _15.")]
        public TileBase[] tiles = new TileBase[16];

        // Lookup: cornerKey (NW<<3|NE<<2|SW<<1|SE) → sprite index
        private static readonly int[] KeyToIndex = { 6, 7, 10, 9, 2, 11, 4, 15, 5, 14, 1, 8, 3, 0, 13, 12 };

        public TileBase GetTile(int nw, int ne, int sw, int se)
        {
            int key = (nw << 3) | (ne << 2) | (sw << 1) | se;
            int idx = KeyToIndex[key];
            if (idx < 0 || idx >= tiles.Length || tiles[idx] == null) return null;
            return tiles[idx];
        }
    }
}
```

### 2. `Assets/Scripts/Systems/Map/CornerWangPainter.cs`

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public static class CornerWangPainter
    {
        /// <summary>
        /// Paints a tilemap using corner Wang tiles.
        /// vertGrid is (width+1) x (height+1), values 0=lower/floor, 1=upper/wall.
        /// originCell is the bottom-left Unity tilemap cell to start painting at.
        /// </summary>
        public static void Paint(
            Tilemap tilemap,
            CornerWangTileSetSO tileSet,
            int[,] vertGrid,
            int width,
            int height,
            Vector3Int originCell = default)
        {
            tilemap.ClearAllTiles();

            for (int ty = 0; ty < height; ty++)
            {
                for (int tx = 0; tx < width; tx++)
                {
                    int nw = vertGrid[tx,     ty + 1];
                    int ne = vertGrid[tx + 1, ty + 1];
                    int sw = vertGrid[tx,     ty    ];
                    int se = vertGrid[tx + 1, ty    ];

                    TileBase tile = tileSet.GetTile(nw, ne, sw, se);
                    if (tile != null)
                    {
                        Vector3Int cell = originCell + new Vector3Int(tx, ty, 0);
                        tilemap.SetTile(cell, tile);
                    }
                }
            }
        }
    }
}
```

### 3. `Assets/Scripts/Demo/DungeonRoomGenerator.cs`

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public class DungeonRoomGenerator : MonoBehaviour
    {
        [Header("Tilemap")]
        public Tilemap targetTilemap;

        [Header("Tile Sets")]
        public CornerWangTileSetSO floorWallTileSet;   // wang_floor_wall tiles
        public CornerWangTileSetSO rubblePathTileSet;  // wang_rubble_path tiles (optional overlay)

        [Header("Room Settings")]
        public int roomWidth  = 16;
        public int roomHeight = 12;

        [Header("Path Settings (0=off)")]
        [Range(0f, 1f)] public float pathDensity = 0.35f;
        public int pathSeed = 42;

        [ContextMenu("Generate Room — Floor+Wall")]
        public void GenerateFloorWallRoom()
        {
            if (floorWallTileSet == null || targetTilemap == null) { Debug.LogError("[DungeonRoomGenerator] Missing refs"); return; }

            int[,] verts = BuildRoomVertGrid(roomWidth, roomHeight, wallThickness: 2);
            CornerWangPainter.Paint(targetTilemap, floorWallTileSet, verts, roomWidth, roomHeight);
            Debug.Log($"[DungeonRoomGenerator] Painted {roomWidth}x{roomHeight} room.");
        }

        [ContextMenu("Generate Room — Rubble+Path Overlay")]
        public void GenerateRubblePathOverlay()
        {
            if (rubblePathTileSet == null || targetTilemap == null) { Debug.LogError("[DungeonRoomGenerator] Missing refs"); return; }

            int[,] verts = BuildPathVertGrid(roomWidth, roomHeight, pathDensity, pathSeed);
            CornerWangPainter.Paint(targetTilemap, rubblePathTileSet, verts, roomWidth, roomHeight);
            Debug.Log($"[DungeonRoomGenerator] Painted {roomWidth}x{roomHeight} rubble+path overlay.");
        }

        // Produces a vertex grid: border=wall(1), interior=floor(0), wallThickness border rings
        private static int[,] BuildRoomVertGrid(int w, int h, int wallThickness)
        {
            int[,] v = new int[w + 1, h + 1];
            for (int y = 0; y <= h; y++)
                for (int x = 0; x <= w; x++)
                    v[x, y] = (x < wallThickness || x > w - wallThickness || y < wallThickness || y > h - wallThickness) ? 1 : 0;
            return v;
        }

        // Produces a vertex grid for path variation using Perlin noise
        private static int[,] BuildPathVertGrid(int w, int h, float density, int seed)
        {
            int[,] v = new int[w + 1, h + 1];
            float offset = seed * 0.1f;
            for (int y = 0; y <= h; y++)
                for (int x = 0; x <= w; x++)
                {
                    float noise = Mathf.PerlinNoise(x * 0.3f + offset, y * 0.3f + offset);
                    v[x, y] = noise > (1f - density) ? 1 : 0;
                }
            return v;
        }
    }
}
```

### 4. `Assets/Editor/CreateCornerWangTileSetAsset.cs`

Editor script that auto-creates the `CornerWangTileSetSO` asset and assigns the 16 existing tiles.

```csharp
using UnityEngine;
using UnityEditor;
using RIMA;

public static class CreateCornerWangTileSetAsset
{
    [MenuItem("RIMA/Tools/Create Corner Wang TileSets")]
    public static void CreateAll()
    {
        CreateForPrefix("wang_floor_wall",   "Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset");
        CreateForPrefix("wang_rubble_path",  "Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[RIMA] Corner Wang TileSet assets created.");
    }

    private static void CreateForPrefix(string prefix, string assetPath)
    {
        var so = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
        so.tiles = new UnityEngine.Tilemaps.TileBase[16];

        for (int i = 0; i < 16; i++)
        {
            string tilePath = $"Assets/Art/Tiles/F1/Generated/{prefix}_tile_{i}.asset";
            var tile = AssetDatabase.LoadAssetAtPath<UnityEngine.Tilemaps.TileBase>(tilePath);
            if (tile == null) Debug.LogWarning($"[RIMA] Tile not found: {tilePath}");
            so.tiles[i] = tile;
        }

        AssetDatabase.CreateAsset(so, assetPath);
        Debug.Log($"[RIMA] Created: {assetPath}");
    }
}
```

## Steps

1. Create all 4 files above.
2. Check Unity console — 0 errors.
3. Run menu `RIMA > Tools > Create Corner Wang TileSets` → creates `FloorWall_CornerWangTileSet.asset` and `RubblePath_CornerWangTileSet.asset`.
4. In the scene `Assets/Scenes/Demo/_FazMVP_Demo.unity`, find or create a GameObject with `DungeonRoomGenerator`, assign `targetTilemap` (the BaseTilemap), assign `floorWallTileSet` = `FloorWall_CornerWangTileSet.asset`.
5. Right-click DungeonRoomGenerator → Context Menu → `Generate Room — Floor+Wall`.
6. Verify tilemap paints correctly in Scene view.
7. Commit all files.

## Success Criteria

- 0 compile errors
- Running "Generate Room — Floor+Wall" from context menu paints a 16×12 room with floor interior and wall border, Wang transitions correct at all edges
- Running "Generate Room — Rubble+Path Overlay" paints Perlin-noise-based path variation on a separate tilemap

## Commit message

`[corner-wang] CornerWangPainter + DungeonRoomGenerator — agent-driven PixelLab tileset map generation`
