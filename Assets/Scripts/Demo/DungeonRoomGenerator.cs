using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public class DungeonRoomGenerator : MonoBehaviour
    {
        [Header("Tilemap")]
        public Tilemap targetTilemap;

        [Header("Tile Sets")]
        public CornerWangTileSetSO floorWallTileSet;
        public CornerWangTileSetSO rubblePathTileSet;

        [Header("Room Settings")]
        public int roomWidth = 16;
        public int roomHeight = 12;

        [Header("Path Settings (0=off)")]
        [Range(0f, 1f)] public float pathDensity = 0.35f;
        public int pathSeed = 42;

        [ContextMenu("Generate Room — Floor+Wall")]
        public void GenerateFloorWallRoom()
        {
            if (floorWallTileSet == null || targetTilemap == null)
            {
                Debug.LogError("[DungeonRoomGenerator] Missing refs");
                return;
            }

            int[,] verts = BuildRoomVertGrid(roomWidth, roomHeight, wallThickness: 2);
            CornerWangPainter.Paint(targetTilemap, floorWallTileSet, verts, roomWidth, roomHeight);
            Debug.Log($"[DungeonRoomGenerator] Painted {roomWidth}x{roomHeight} room.");
        }

        [ContextMenu("Generate Room — Rubble+Path Overlay")]
        public void GenerateRubblePathOverlay()
        {
            if (rubblePathTileSet == null || targetTilemap == null)
            {
                Debug.LogError("[DungeonRoomGenerator] Missing refs");
                return;
            }

            int[,] verts = BuildPathVertGrid(roomWidth, roomHeight, pathDensity, pathSeed);
            CornerWangPainter.Paint(targetTilemap, rubblePathTileSet, verts, roomWidth, roomHeight);
            Debug.Log($"[DungeonRoomGenerator] Painted {roomWidth}x{roomHeight} rubble+path overlay.");
        }

        private static int[,] BuildRoomVertGrid(int w, int h, int wallThickness)
        {
            int[,] v = new int[w + 1, h + 1];
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    v[x, y] = (x < wallThickness || x > w - wallThickness || y < wallThickness || y > h - wallThickness) ? 1 : 0;
                }
            }

            return v;
        }

        private static int[,] BuildPathVertGrid(int w, int h, float density, int seed)
        {
            int[,] v = new int[w + 1, h + 1];
            float offset = seed * 0.1f;
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    float noise = Mathf.PerlinNoise(x * 0.3f + offset, y * 0.3f + offset);
                    v[x, y] = noise > (1f - density) ? 1 : 0;
                }
            }

            return v;
        }
    }
}
