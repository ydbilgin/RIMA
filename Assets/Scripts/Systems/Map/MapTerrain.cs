using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA;

namespace RIMA.Systems.Map
{
    [System.Serializable]
    public class MapTerrain
    {
        public int id;
        public string name;
        public Color paletteColor = Color.gray;
        public TileBase baseTile;
        public CornerWangTileSetSO baseTileSource;
    }
}
