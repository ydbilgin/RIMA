using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public class CornerWangTileSetSO : ScriptableObject
    {
        [Header("Terrain Labels")]
        public string lowerTerrainLabel = "lower (floor)";
        public string upperTerrainLabel = "upper (wall)";

        public TileBase[] tiles = new TileBase[16];

        public TileBase GetTile(int nw, int ne, int sw, int se)
        {
            int index = (nw != 0 ? 1 : 0)
                | (ne != 0 ? 2 : 0)
                | (sw != 0 ? 4 : 0)
                | (se != 0 ? 8 : 0);

            if (tiles == null || index < 0 || index >= tiles.Length)
            {
                return null;
            }

            return tiles[index];
        }
    }
}
