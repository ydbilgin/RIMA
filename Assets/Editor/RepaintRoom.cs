using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public static class RepaintRoom
    {
        const int ROOM_W = 20;
        const int ROOM_H = 15;

        [MenuItem("RIMA/Setup/Repaint Room (20x15)")]
        public static void Repaint()
        {
            // Tile asset'leri yükle
            var floorTile = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Art/Tiles/Act1/Act1_Floor.asset");
            var wallTile  = AssetDatabase.LoadAssetAtPath<TileBase>("Assets/Art/Tiles/Act1/Act1_Wall.asset");

            if (floorTile == null || wallTile == null)
            {
                Debug.LogError("Tile asset bulunamadi! Art/Tiles/Act1/ kontrol et.");
                return;
            }

            // Hedef tilemap'leri bul (Kiro'nun Grid'inden)
            Tilemap floor = null, walls = null;
            foreach (var tm in Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
            {
                if (tm.name == "Tilemap_Floor") floor = tm;
                if (tm.name == "Tilemap_Walls") walls = tm;
            }

            if (floor == null || walls == null)
            {
                Debug.LogError("Tilemap_Floor veya Tilemap_Walls bulunamadi!");
                return;
            }

            // Temizle
            floor.ClearAllTiles();
            walls.ClearAllTiles();

            // Zemin: tüm oda
            for (int x = 0; x < ROOM_W; x++)
                for (int y = 0; y < ROOM_H; y++)
                    floor.SetTile(new Vector3Int(x, y, 0), floorTile);

            // Duvar: sadece çerçeve
            for (int x = 0; x < ROOM_W; x++)
            {
                walls.SetTile(new Vector3Int(x, 0, 0), wallTile);            // alt
                walls.SetTile(new Vector3Int(x, ROOM_H - 1, 0), wallTile);  // üst
            }
            for (int y = 0; y < ROOM_H; y++)
            {
                walls.SetTile(new Vector3Int(0, y, 0), wallTile);            // sol
                walls.SetTile(new Vector3Int(ROOM_W - 1, y, 0), wallTile);  // sağ
            }

            EditorUtility.SetDirty(floor);
            EditorUtility.SetDirty(walls);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene());

            Debug.Log($"Oda boyandi: {ROOM_W}x{ROOM_H} zemin, {2*ROOM_W + 2*(ROOM_H-2)} duvar tile.");
        }
    }
}
