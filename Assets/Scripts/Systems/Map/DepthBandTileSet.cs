using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(fileName = "DepthBandTileSet", menuName = "RIMA/Map/Depth Band Tile Set")]
    public class DepthBandTileSet : ScriptableObject
    {
        public int minDepth;
        public int maxDepth;
        [SerializeField] public TileBase[] floorTiles;
        [SerializeField] public TileBase[] wallTiles;
    }
}
