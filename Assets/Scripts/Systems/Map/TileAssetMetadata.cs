using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(fileName = "TileAssetMetadata", menuName = "RIMA/Tile Asset Metadata")]
    public class TileAssetMetadata : ScriptableObject
    {
        public string tileId;
        public string charKey; // single char used in ASCII map import (e.g. "#", "^", "~")
        public RimaBiomeType biomeType;
        public RimaTerrainType terrainType;
        public TileBase tile;
        [Range(0f, 1f)] public float weight = 1f;
        public bool supportsCollision;
        public bool isCliffFront;
        public bool isCliffTop;
        public bool isTransition;
        public bool decalAllowed = true;
        public bool scatterAllowed = true;
        public bool shadowRequired;
        public int wangMask;
        public string variantGroup;
    }
}
