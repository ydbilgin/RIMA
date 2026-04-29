using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace RIMA
{
    /// <summary>
    /// Act 1 test odası çizer — Tile Palette UI gerekmez.
    /// Menu: RIMA / Paint Act1 Test Room
    /// </summary>
    public static class Act1RoomPainter
    {
        private const int RoomW  = 20;
        private const int RoomH  = 15;
        private const string FloorTilePath = "Assets/Art/Tiles/Act1/Act1_Floor.asset";
        private const string WallTilePath  = "Assets/Art/Tiles/Act1/Act1_Wall.asset";
        private const string FloorTilemapName = "Tilemap_Floor";
        private const string WallTilemapName  = "Tilemap_Walls";

        [MenuItem("RIMA/Paint Act1 Test Room")]
        public static void Paint()
        {
            var floorTile = GetOrCreateTile(FloorTilePath, "Assets/Art/Tiles/Act1/Act1_Floor.png");
            var wallTile  = GetOrCreateTile(WallTilePath, "Assets/Art/Tiles/Act1/Act1_Wall.png");

            if (floorTile == null) { Debug.LogError($"[RoomPainter] Floor tile oluşturulamadı: {FloorTilePath}"); return; }
            if (wallTile  == null) { Debug.LogError($"[RoomPainter] Wall tile oluşturulamadı: {WallTilePath}");  return; }

            var floorTilemap = FindTilemap(FloorTilemapName);
            var wallTilemap  = FindTilemap(WallTilemapName);

            if (floorTilemap == null || wallTilemap == null) return;

            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();

            // Zemin — iç alan
            for (int x = 1; x < RoomW - 1; x++)
                for (int y = 1; y < RoomH - 1; y++)
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);

            // Duvarlar — kenar halkası
            for (int x = 0; x < RoomW; x++)
            {
                wallTilemap.SetTile(new Vector3Int(x, 0,        0), wallTile);
                wallTilemap.SetTile(new Vector3Int(x, RoomH - 1, 0), wallTile);
            }
            for (int y = 0; y < RoomH; y++)
            {
                wallTilemap.SetTile(new Vector3Int(0,        y, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(RoomW - 1, y, 0), wallTile);
            }

            // Player'ı ortaya taşı
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = new Vector3(RoomW / 2f, RoomH / 2f, 0f);

            EditorUtility.SetDirty(floorTilemap);
            EditorUtility.SetDirty(wallTilemap);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                floorTilemap.gameObject.scene);

            Debug.Log($"[RoomPainter] Test odası çizildi — {RoomW}×{RoomH}");
        }

        private static Tilemap FindTilemap(string name)
        {
            var go = GameObject.Find(name);
            if (go == null) { Debug.LogError($"[RoomPainter] GameObject bulunamadı: {name}"); return null; }
            var tm = go.GetComponent<Tilemap>();
            if (tm == null) { Debug.LogError($"[RoomPainter] Tilemap component yok: {name}"); return null; }
            return tm;
        }

        private static TileBase GetOrCreateTile(string tilePath, string spritePath)
        {
            var tile = AssetDatabase.LoadAssetAtPath<TileBase>(tilePath);
            if (tile != null) return tile;

            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite == null)
            {
                Debug.LogError($"[RoomPainter] Sprite bulunamadı: {spritePath}");
                return null;
            }

            var newTile = UnityEngine.Tilemaps.Tile.CreateInstance<UnityEngine.Tilemaps.Tile>();
            newTile.sprite = sprite;
            AssetDatabase.CreateAsset(newTile, tilePath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[RoomPainter] Tile oluşturuldu: {tilePath}");
            return newTile;
        }
    }
}
