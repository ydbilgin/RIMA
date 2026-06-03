using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using RIMA.Systems.Map;

public static class CreateDepthBandSOs
{
    [MenuItem("RIMA/Utilities/Create DepthBand SOs")]
    public static void CreateAll()
    {
        string outDir = "Assets/Resources/Map/DepthBands";
        if (!Directory.Exists(outDir))
            Directory.CreateDirectory(outDir);

        CreateBand(outDir, "DepthBandTileSet_F1", 0, 2,
            "Assets/Art/Tiles/Act1/F1", "f1_",
            "Assets/Art/Tiles/Act1/W1", "w1_");

        CreateBand(outDir, "DepthBandTileSet_F2", 3, 5,
            "Assets/Art/Tiles/Act1/F2", "f2_",
            "Assets/Art/Tiles/Act1/W2", "w2_");

        CreateBand(outDir, "DepthBandTileSet_F3", 6, 999,
            "Assets/Art/Tiles/Act1/F3", "f3_",
            "Assets/Art/Tiles/Act1/WB", "wb_");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[RIMA] DepthBand SO creation complete.");
    }

    private static void CreateBand(string outDir, string assetName, int minD, int maxD,
        string floorDir, string floorPrefix, string wallDir, string wallPrefix)
    {
        string assetPath = $"{outDir}/{assetName}.asset";

        var so = AssetDatabase.LoadAssetAtPath<DepthBandTileSet>(assetPath);
        if (so == null)
        {
            so = ScriptableObject.CreateInstance<DepthBandTileSet>();
            AssetDatabase.CreateAsset(so, assetPath);
        }

        so.minDepth = minD;
        so.maxDepth = maxD;
        so.floorTiles = LoadTiles(floorDir, floorPrefix);
        so.wallTiles  = LoadTiles(wallDir,  wallPrefix);

        EditorUtility.SetDirty(so);
        Debug.Log($"[RIMA] {assetName}: floor={so.floorTiles.Length} wall={so.wallTiles.Length}");
    }

    private static TileBase[] LoadTiles(string dir, string prefix)
    {
        if (!Directory.Exists(dir))
        {
            Debug.LogWarning($"[RIMA] Tile directory not found: {dir}");
            return new TileBase[0];
        }

        var guids = AssetDatabase.FindAssets($"t:TileBase {prefix}", new[] { dir });
        return guids
            .Select(g => AssetDatabase.GUIDToAssetPath(g))
            .Where(p => Path.GetFileName(p).StartsWith(prefix))
            .Select(p => AssetDatabase.LoadAssetAtPath<TileBase>(p))
            .Where(t => t != null)
            .ToArray();
    }
}
