using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    [RequireComponent(typeof(Tilemap))]
    [ExecuteAlways]
    public class CliffYSortManager : MonoBehaviour
    {
        public static readonly int[] CliffKeys = { 1, 2, 3, 5, 6, 7, 9, 10, 11 };
        public static readonly float[] YOffsetByKey =
        {
            0f, -0.5f, -0.5f, -0.5f, 0.5f, -0.25f, -0.25f, -0.5f,
            0.5f, -0.25f, -0.25f, -0.5f, 0.5f, 0.5f, 0.5f, 0f
        };

        [SerializeField] public CornerWangTileSetSO tileSet;

        private void Awake()
        {
            ApplySortMode();
        }

        public void ApplySortMode()
        {
            TilemapRenderer renderer = GetComponent<TilemapRenderer>();
            Tilemap tilemap = GetComponent<Tilemap>();
            if (tileSet == null || renderer == null || tilemap == null)
            {
                return;
            }

            int cliffCount = 0;
            BoundsInt bounds = tilemap.cellBounds;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                    if (tile == null)
                    {
                        continue;
                    }

                    int key = GetCornerKey(tile);
                    if (System.Array.IndexOf(CliffKeys, key) >= 0)
                    {
                        cliffCount++;
                    }
                }
            }

            renderer.mode = cliffCount > 0 ? TilemapRenderer.Mode.Individual : TilemapRenderer.Mode.Chunk;
        }

        public int GetCornerKey(TileBase tile)
        {
            if (tileSet == null || tileSet.tiles == null)
            {
                return -1;
            }

            for (int i = 0; i < tileSet.tiles.Length && i < 16; i++)
            {
                if (tileSet.tiles[i] == tile)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool IsCliffCell(Vector3Int pos)
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            return tilemap != null && System.Array.IndexOf(CliffKeys, GetCornerKey(tilemap.GetTile(pos))) >= 0;
        }
    }
}
