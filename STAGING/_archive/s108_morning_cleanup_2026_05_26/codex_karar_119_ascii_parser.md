# Karar #119 AI ASCII Matrix Parser — execute every step, commit at end

## Context

LOCKED Karar #119 (S68 P4, Faz 1.6). Offline LLM produces 3-layer ASCII maps:
- Elevation layer: `.` = floor, `#` = wall, `^` = elevated wall
- Ground layer: `.` = bare floor, `~` = mossy, `x` = rubble
- Props layer: ` ` = empty, `P` = pillar, `C` = chest, `T` = torch

An EditorWindow (`AITilemapImporter`) accepts the 3 ASCII strings and paints the active scene's 4-layer tilemap stack using existing TileAssetMetadata + WangTileResolver.

## Key files (DO NOT read others)

- `Assets/Scripts/Systems/Map/TileAssetMetadata.cs`
- `Assets/Scripts/Systems/Map/WangTileResolver.cs`
- `Assets/Scripts/Systems/Map/RimaBiomePreset.cs`
- `Assets/Scripts/Systems/Map/LayeredRoomPainter.cs` (for PaintBiome reference)

## STEP 1 — Add charKey field to TileAssetMetadata

Read `Assets/Scripts/Systems/Map/TileAssetMetadata.cs`.

Add field after `tileId`:
```csharp
public string charKey; // single char used in ASCII map import (e.g. "#", "^", "~")
```

## STEP 2 — Create AITilemapImporter EditorWindow

Create `Assets/Scripts/Editor/AITilemapImporter.cs`:

```csharp
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using RIMA.Systems.Map;

public class AITilemapImporter : EditorWindow
{
    [MenuItem("RIMA/Tools/AI Tilemap Importer")]
    public static void Open() => GetWindow<AITilemapImporter>("AI Tilemap Importer");

    private string elevationASCII = "# # #\n# . #\n# # #";
    private string groundASCII    = ". . .\n. ~ .\n. . .";
    private string propsASCII     = "      \n  P   \n      ";

    private TileAssetMetadata[] tileLibrary;
    private Tilemap baseTm, decalTm, wallFrontTm, wallTopTm;
    private Transform propContainer;
    private int seed = 42;

    private void OnGUI()
    {
        GUILayout.Label("AI ASCII Tilemap Importer (Karar #119)", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Elevation layer  (# = wall, ^ = elevated, . = floor)");
        elevationASCII = EditorGUILayout.TextArea(elevationASCII, GUILayout.Height(80));

        EditorGUILayout.LabelField("Ground layer  (. = bare, ~ = moss, x = rubble)");
        groundASCII = EditorGUILayout.TextArea(groundASCII, GUILayout.Height(80));

        EditorGUILayout.LabelField("Props layer  (space = empty, P = pillar, C = chest, T = torch)");
        propsASCII = EditorGUILayout.TextArea(propsASCII, GUILayout.Height(80));

        GUILayout.Space(8);
        baseTm       = (Tilemap)EditorGUILayout.ObjectField("Base Tilemap", baseTm, typeof(Tilemap), true);
        decalTm      = (Tilemap)EditorGUILayout.ObjectField("Decal Tilemap", decalTm, typeof(Tilemap), true);
        wallFrontTm  = (Tilemap)EditorGUILayout.ObjectField("Wall Front Tilemap", wallFrontTm, typeof(Tilemap), true);
        wallTopTm    = (Tilemap)EditorGUILayout.ObjectField("Wall Top Tilemap", wallTopTm, typeof(Tilemap), true);
        propContainer = (Transform)EditorGUILayout.ObjectField("Prop Container", propContainer, typeof(Transform), true);

        GUILayout.Label("Tile Library (assign TileAssetMetadata[] here or load from biome preset)");
        ScriptableObject libSO = null;
        // User assigns via tileLibrary field below
        if (GUILayout.Button("Reload from Selection (select TileAssetMetadata[] array parent)"))
            RefreshLibraryFromSelection();

        seed = EditorGUILayout.IntField("Seed", seed);

        GUILayout.Space(8);
        GUI.enabled = baseTm != null;
        if (GUILayout.Button("Import ASCII to Tilemaps"))
            DoImport();
        GUI.enabled = true;
    }

    private void RefreshLibraryFromSelection()
    {
        if (Selection.activeObject is RimaBiomePreset bp)
        {
            var combined = new System.Collections.Generic.List<TileAssetMetadata>();
            if (bp.allowedFloorTiles != null)  combined.AddRange(bp.allowedFloorTiles);
            if (bp.allowedWallTiles  != null)  combined.AddRange(bp.allowedWallTiles);
            if (bp.decalTiles        != null)  combined.AddRange(bp.decalTiles);
            tileLibrary = combined.ToArray();
            Debug.Log($"AITilemapImporter: Loaded {tileLibrary.Length} tiles from {bp.biomeName}");
        }
        else
        {
            Debug.LogWarning("AITilemapImporter: Select a RimaBiomePreset in Project window first.");
        }
    }

    private void DoImport()
    {
        string[] elevLines = elevationASCII.Split('\n');
        string[] groundLines = groundASCII.Split('\n');
        string[] propLines = propsASCII.Split('\n');
        int h = elevLines.Length;
        int w = elevLines[0].Replace(" ", "").Length; // trim spaces for width

        // Parse to bool/char grids
        bool[,] isWall     = new bool[w, h];
        char[,] groundChar = new char[w, h];
        char[,] propChar   = new char[w, h];

        for (int row = 0; row < h; row++)
        {
            // elevation: chars separated by spaces or raw
            string elevRow   = row < elevLines.Length  ? elevLines[row]  : "";
            string groundRow = row < groundLines.Length ? groundLines[row] : "";
            string propRow   = row < propLines.Length  ? propLines[row]  : "";

            for (int col = 0; col < w; col++)
            {
                char ec = col < elevRow.Length   ? elevRow[col]   : '.';
                char gc = col < groundRow.Length ? groundRow[col] : '.';
                char pc = col < propRow.Length   ? propRow[col]   : ' ';
                int y = h - 1 - row; // flip Y (top-left → bottom-left)
                isWall[col, y]     = (ec == '#' || ec == '^');
                groundChar[col, y] = gc;
                propChar[col, y]   = pc;
            }
        }

        if (baseTm) baseTm.ClearAllTiles();
        if (decalTm) decalTm.ClearAllTiles();
        if (wallFrontTm) wallFrontTm.ClearAllTiles();
        if (wallTopTm) wallTopTm.ClearAllTiles();

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                var cell = new Vector3Int(x, y, 0);
                if (isWall[x, y])
                {
                    // Wang mask for wall
                    bool north = IsWall(isWall, x, y+1, w, h);
                    bool east  = IsWall(isWall, x+1, y, w, h);
                    bool south = IsWall(isWall, x, y-1, w, h);
                    bool west  = IsWall(isWall, x-1, y, w, h);
                    int mask = WangTileResolver.ComputeWangMask(north, east, south, west);
                    var meta = ResolveCharOrWang("#", mask, cell);
                    if (meta?.tile != null)
                    {
                        if (wallFrontTm) wallFrontTm.SetTile(cell, meta.tile);
                        if (wallTopTm && meta.isCliffTop) wallTopTm.SetTile(cell, meta.tile);
                    }
                }
                else
                {
                    // Floor tile from ground layer char
                    string gc = groundChar[x, y].ToString();
                    var floorMeta = ResolveChar(gc) ?? ResolveChar(".");
                    if (floorMeta?.tile != null && baseTm) baseTm.SetTile(cell, floorMeta.tile);

                    // Decal from ground char
                    if (gc != "." && gc != " ")
                    {
                        var decalMeta = ResolveChar(gc + "_decal") ?? ResolveChar(gc);
                        if (decalMeta?.tile != null && decalTm) decalTm.SetTile(cell, decalMeta.tile);
                    }

                    // Props
                    char pc = propChar[x, y];
                    if (pc != ' ' && propContainer != null)
                        SpawnProp(pc.ToString(), cell);
                }
            }
        }

        Debug.Log($"AITilemapImporter: Import complete ({w}x{h})");
    }

    private TileAssetMetadata ResolveChar(string key)
    {
        if (tileLibrary == null) return null;
        foreach (var t in tileLibrary)
            if (t != null && t.charKey == key) return t;
        return null;
    }

    private TileAssetMetadata ResolveCharOrWang(string key, int mask, Vector3Int cell)
    {
        if (tileLibrary == null) return null;
        // Try wang mask first
        var candidates = System.Array.FindAll(tileLibrary, t => t != null && t.wangMask == mask && (t.charKey == key || t.charKey == "#"));
        if (candidates.Length > 0) return candidates[0];
        // Fallback: any wall tile
        return ResolveChar(key);
    }

    private bool IsWall(bool[,] grid, int x, int y, int w, int h)
    {
        if (x < 0 || y < 0 || x >= w || y >= h) return true;
        return grid[x, y];
    }

    private void SpawnProp(string key, Vector3Int cell)
    {
        // Stub — prop prefab lookup by key in Resources/Props/
        var prefab = Resources.Load<GameObject>($"Props/{key}");
        if (prefab == null)
        {
            Debug.LogWarning($"AITilemapImporter: No prop prefab at Resources/Props/{key}");
            return;
        }
        var world = baseTm ? baseTm.CellToWorld(cell) : (Vector3)cell;
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, propContainer);
        go.transform.position = world;
    }
}
```

## STEP 3 — Add sample ASCII fixture

Write `STAGING/sample_ascii_f1_room.md`:
```
# Sample F1 Shattered Keep ASCII Room

## Elevation (# = wall, . = floor)
```
###########
#.........#
#....^....#
#.........#
#.........#
###########
```

## Ground (. = bare floor, ~ = moss, x = rubble)
```
...........
...~~......
...~x~.....
...~~......
...........
...........
```

## Props (P = pillar, C = chest, T = torch, space = empty)
```
           
  T     T  
     P     
           
      C    
           
```
```

## STEP 4 — Compile check

`read_console` — 0 errors required. Fix any compiler errors.

## STEP 5 — Commit

```bash
git add Assets/Scripts/Systems/Map/TileAssetMetadata.cs Assets/Scripts/Editor/AITilemapImporter.cs STAGING/sample_ascii_f1_room.md
git commit -m "[karar119] AITilemapImporter EditorWindow — 3-layer ASCII → 4-layer tilemap

- TileAssetMetadata: charKey field for ASCII char lookup
- AITilemapImporter: EditorWindow, 3 ASCII fields (Elevation/Ground/Props)
- Elevation → WangMask wall routing (WallsFront/WallsTop per Antigravity P0)
- Ground char → base/decal tile lookup; Props → prefab spawn stub
- RimaBiomePreset selection reloads tile library
- Karar #119 LOCK — S68 P4 Faz 1.6"
```

## STEP 6 — Report

Write `STAGING/karar_119_ascii_parser_report.md`:
```
# Karar #119 AI ASCII Parser Report

## TileAssetMetadata.charKey
[added Y/N]

## AITilemapImporter EditorWindow
[created Y/N, RIMA > Tools > AI Tilemap Importer Y/N]

## 3-layer parsing
[Elevation→walls Y/N, Ground→floor+decal Y/N, Props→stub Y/N]

## Compile
[0 errors Y/N]
```

Append `CODEX_DONE_laurethgame.md`:
```
## [2026-05-14] Karar #119 AI ASCII Matrix Parser
- TileAssetMetadata.charKey: Y/N
- AITilemapImporter EditorWindow: Y/N
- 3-layer parsing: Y/N
- Compile: Y/N
- Commit: [hash]
```

## Constraints

- DO NOT add charKey to YAML serialized assets — only the CS script
- Prop spawn is a STUB (prefab lookup by key in Resources/Props/) — no actual prefabs needed
- Wang mask computation: reuse WangTileResolver.ComputeWangMask (static method, accessible from Editor assembly)
- NAMESPACE: AITilemapImporter is in global namespace (Editor script), TileAssetMetadata is RIMA.Systems.Map

## Source References

1. `Assets/Scripts/Systems/Map/TileAssetMetadata.cs` — add charKey field here
2. `Assets/Scripts/Systems/Map/WangTileResolver.cs` — ComputeWangMask static method
3. `Assets/Scripts/Systems/Map/RimaBiomePreset.cs` — allowedFloorTiles/allowedWallTiles/decalTiles
4. `Assets/Scripts/Systems/Map/LayeredRoomPainter.cs` — IsWall helper pattern to reuse
