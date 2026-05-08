using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SceneFloorPainter
{
    [MenuItem("RIMA/Paint Scene Floor (F1 30x30)")]
    public static void PaintSceneFloor()
    {
        var grid = GameObject.Find("IsoGrid");
        if (grid == null) { Debug.LogError("[SceneFloorPainter] IsoGrid not found"); return; }

        Tilemap floor = null;
        foreach (Transform child in grid.transform)
        {
            if (child.name == "Ground")
            {
                floor = child.GetComponent<Tilemap>();
                break;
            }
        }
        if (floor == null)
        {
            // fallback: first Tilemap child
            floor = grid.GetComponentInChildren<Tilemap>();
        }
        if (floor == null) { Debug.LogError("[SceneFloorPainter] No Tilemap found under IsoGrid"); return; }

        var tiles = new TileBase[16];
        int loaded = 0;
        for (int i = 0; i < 16; i++)
        {
            string path = $"Assets/Art/Tiles/Act1/F1/f1_{i:D2}.asset";
            tiles[i] = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tiles[i] != null) loaded++;
        }
        if (loaded == 0) { Debug.LogError("[SceneFloorPainter] No F1 tiles found at Assets/Art/Tiles/Act1/F1/"); return; }

        floor.ClearAllTiles();
        var rng = new System.Random(42);
        int w = 30, h = 30;
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                var t = tiles[rng.Next(16)];
                if (t != null) floor.SetTile(new Vector3Int(x - w / 2, y - h / 2, 0), t);
            }

        EditorSceneManager.SaveOpenScenes();
        Debug.Log($"[SceneFloorPainter] Painted {w}x{h} F1 floor on {floor.gameObject.name}. Scene saved.");
    }
}
