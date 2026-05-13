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

        bool[,] isWall     = new bool[w, h];
        char[,] groundChar = new char[w, h];
        char[,] propChar   = new char[w, h];

        for (int row = 0; row < h; row++)
        {
            string elevRow   = row < elevLines.Length  ? elevLines[row]  : "";
            string groundRow = row < groundLines.Length ? groundLines[row] : "";
            string propRow   = row < propLines.Length  ? propLines[row]  : "";

            for (int col = 0; col < w; col++)
            {
                char ec = col < elevRow.Length   ? elevRow[col]   : '.';
                char gc = col < groundRow.Length ? groundRow[col] : '.';
                char pc = col < propRow.Length   ? propRow[col]   : ' ';
                int y = h - 1 - row;
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
                    string gc = groundChar[x, y].ToString();
                    var floorMeta = ResolveChar(gc) ?? ResolveChar(".");
                    if (floorMeta?.tile != null && baseTm) baseTm.SetTile(cell, floorMeta.tile);

                    if (gc != "." && gc != " ")
                    {
                        var decalMeta = ResolveChar(gc + "_decal") ?? ResolveChar(gc);
                        if (decalMeta?.tile != null && decalTm) decalTm.SetTile(cell, decalMeta.tile);
                    }

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
        var candidates = System.Array.FindAll(tileLibrary, t => t != null && t.wangMask == mask && (t.charKey == key || t.charKey == "#"));
        if (candidates.Length > 0) return candidates[0];
        return ResolveChar(key);
    }

    private bool IsWall(bool[,] grid, int x, int y, int w, int h)
    {
        if (x < 0 || y < 0 || x >= w || y >= h) return true;
        return grid[x, y];
    }

    private void SpawnProp(string key, Vector3Int cell)
    {
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
