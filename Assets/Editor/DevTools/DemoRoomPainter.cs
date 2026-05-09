using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class DemoRoomPainter
{
    [MenuItem("RIMA/Paint Demo Room")]
    public static void Paint()
    {
        // --- Find tilemaps ---
        var isoGrid = GameObject.Find("IsoGrid");
        if (isoGrid == null)
        {
            Debug.LogError("[DemoRoomPainter] 'IsoGrid' GameObject not found in open scene.");
            return;
        }

        Tilemap ground = null;
        Tilemap walls  = null;
        foreach (var tm in isoGrid.GetComponentsInChildren<Tilemap>())
        {
            if (tm.name == "Ground") ground = tm;
            else if (tm.name == "Walls")  walls  = tm;
        }

        if (ground == null) { Debug.LogError("[DemoRoomPainter] 'Ground' Tilemap not found under IsoGrid."); return; }
        if (walls  == null) { Debug.LogError("[DemoRoomPainter] 'Walls' Tilemap not found under IsoGrid.");  return; }

        // --- Load F1 floor tiles ---
        var floorTiles = new Tile[16];
        for (int i = 0; i < 16; i++)
        {
            string path = $"Assets/Art/Tiles/Act1/F1/f1_{i:D2}.asset";
            floorTiles[i] = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (floorTiles[i] == null)
                Debug.LogWarning($"[DemoRoomPainter] Missing floor tile: {path}");
        }

        // --- Load W1 wall tiles ---
        var wallTiles = new Tile[16];
        for (int i = 0; i < 16; i++)
        {
            string path = $"Assets/Art/Tiles/Act1/W1/w1_{i:D2}.asset";
            wallTiles[i] = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (wallTiles[i] == null)
                Debug.LogWarning($"[DemoRoomPainter] Missing wall tile: {path}");
        }

        // --- Clear ---
        ground.ClearAllTiles();
        walls.ClearAllTiles();

        // --- Paint floor: 14x10 rectangle, x: -7..6, y: -5..4 ---
        for (int x = -7; x <= 6; x++)
        {
            for (int y = -5; y <= 4; y++)
            {
                var t = floorTiles[UnityEngine.Random.Range(0, 16)];
                if (t != null) ground.SetTile(new Vector3Int(x, y, 0), t);
            }
        }

        // --- Paint walls ---
        int wallCount = 0;

        // North wall: y=5, x: -7..6
        for (int x = -7; x <= 6; x++)
        {
            var t = wallTiles[UnityEngine.Random.Range(0, 16)];
            if (t != null) { walls.SetTile(new Vector3Int(x, 5, 0), t); wallCount++; }
        }

        // South wall: y=-6, x: -7..6
        for (int x = -7; x <= 6; x++)
        {
            var t = wallTiles[UnityEngine.Random.Range(0, 16)];
            if (t != null) { walls.SetTile(new Vector3Int(x, -6, 0), t); wallCount++; }
        }

        // --- Mark scene dirty ---
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        Debug.Log($"[DemoRoomPainter] Done. Floor=140 tiles, Walls={wallCount}");
    }
}
