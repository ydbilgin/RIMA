using RIMA;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class CreateCornerWangTileSetAsset
{
    private const string DemoScenePath = "Assets/Scenes/Demo/_FazMVP_Demo.unity";
    private const string FloorWallAssetPath = "Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset";
    private const string RubblePathAssetPath = "Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset";

    [MenuItem("RIMA/Tools/Create Corner Wang TileSets")]
    public static void CreateAll()
    {
        CreateForPrefix("wang_floor_wall", FloorWallAssetPath);
        CreateForPrefix("wang_rubble_path", RubblePathAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[RIMA] Corner Wang TileSet assets created.");
    }

    public static void SetupDemoSceneAndGenerateFloorWall()
    {
        CreateAll();

        EditorSceneManager.OpenScene(DemoScenePath);

        Tilemap baseTilemap = FindTilemapByName("BaseTilemap");
        var floorWall = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(FloorWallAssetPath);
        var rubblePath = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(RubblePathAssetPath);

        if (baseTilemap == null)
        {
            Debug.LogError("[RIMA] BaseTilemap not found in demo scene.");
            EditorApplication.Exit(1);
            return;
        }

        if (floorWall == null)
        {
            Debug.LogError($"[RIMA] Missing tile set asset: {FloorWallAssetPath}");
            EditorApplication.Exit(1);
            return;
        }

        GameObject generatorObject = GameObject.Find("DungeonRoomGenerator");
        if (generatorObject == null)
        {
            generatorObject = new GameObject("DungeonRoomGenerator");
        }

        var generator = generatorObject.GetComponent<DungeonRoomGenerator>();
        if (generator == null)
        {
            generator = generatorObject.AddComponent<DungeonRoomGenerator>();
        }

        generator.targetTilemap = baseTilemap;
        generator.floorWallTileSet = floorWall;
        generator.rubblePathTileSet = rubblePath;
        generator.roomWidth = 16;
        generator.roomHeight = 12;
        generator.GenerateFloorWallRoom();

        EditorSceneManager.MarkSceneDirty(generatorObject.scene);
        EditorSceneManager.SaveScene(generatorObject.scene);
        AssetDatabase.SaveAssets();

        Debug.Log("[RIMA] Demo scene wired and floor+wall room generated.");
    }

    private static void CreateForPrefix(string prefix, string assetPath)
    {
        var so = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(assetPath);
        if (so == null)
        {
            so = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
            AssetDatabase.CreateAsset(so, assetPath);
        }

        so.tiles = new TileBase[16];

        for (int i = 0; i < 16; i++)
        {
            string tilePath = $"Assets/Art/Tiles/F1/Generated/{prefix}_tile_{i}.asset";
            var tile = AssetDatabase.LoadAssetAtPath<TileBase>(tilePath);
            if (tile == null)
            {
                Debug.LogWarning($"[RIMA] Tile not found: {tilePath}");
            }

            so.tiles[i] = tile;
        }

        EditorUtility.SetDirty(so);
        Debug.Log($"[RIMA] Created: {assetPath}");
    }

    private static Tilemap FindTilemapByName(string name)
    {
        foreach (var tilemap in Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            if (tilemap.name == name)
            {
                return tilemap;
            }
        }

        return null;
    }
}
