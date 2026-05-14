using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public static class CornerWangPainter
    {
        public static void Paint(Tilemap tilemap, CornerWangTileSetSO tileSet, int[,] vertices, int width, int height, Vector3Int origin = default)
        {
            if (tilemap == null || tileSet == null || vertices == null)
            {
                return;
            }

            tilemap.ClearAllTiles();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TileBase tile = tileSet.GetTile(
                        vertices[x, y + 1],
                        vertices[x + 1, y + 1],
                        vertices[x, y],
                        vertices[x + 1, y]);

                    if (tile != null)
                    {
                        tilemap.SetTile(origin + new Vector3Int(x, y, 0), tile);
                    }
                }
            }

            tilemap.RefreshAllTiles();
        }
    }
}
