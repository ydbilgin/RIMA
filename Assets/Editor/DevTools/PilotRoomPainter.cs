using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PilotRoomPainter
{
    private static readonly string[] PrefabPaths =
    {
        "Assets/Prefabs/Rooms/Act1/combat_01.prefab",
        "Assets/Prefabs/Rooms/Act1/reward_01.prefab",
        "Assets/Prefabs/Rooms/Act1/corridor_01.prefab",
    };

    private static readonly int[] Widths  = { 20, 12,  6 };
    private static readonly int[] Heights = { 15, 12, 20 };

    [MenuItem("RIMA/Paint Pilot Rooms")]
    public static void Paint()
    {
        var tiles = new TileBase[16];
        for (int i = 0; i < 16; i++)
        {
            string path = $"Assets/Art/Tiles/Act1/F1/f1_{i:D2}.asset";
            tiles[i] = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tiles[i] == null)
                Debug.LogWarning($"[PilotRoomPainter] Missing tile at {path}");
        }

        for (int p = 0; p < PrefabPaths.Length; p++)
        {
            var root = PrefabUtility.LoadPrefabContents(PrefabPaths[p]);
            if (root == null) { Debug.LogError($"[PilotRoomPainter] Cannot load {PrefabPaths[p]}"); continue; }

            var tilemap = root.GetComponentInChildren<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogWarning($"[PilotRoomPainter] No Tilemap in {PrefabPaths[p]} — skipping");
                PrefabUtility.UnloadPrefabContents(root);
                continue;
            }

            tilemap.ClearAllTiles();
            var rng = new System.Random(42);
            int w = Widths[p], h = Heights[p];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var t = tiles[rng.Next(16)];
                    if (t != null) tilemap.SetTile(new Vector3Int(x, y, 0), t);
                }

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPaths[p]);
            PrefabUtility.UnloadPrefabContents(root);
            Debug.Log($"[PilotRoomPainter] Painted {PrefabPaths[p]} {w}x{h}");
        }

        AssetDatabase.SaveAssets();
        Debug.Log("[PilotRoomPainter] Done.");
    }
}
