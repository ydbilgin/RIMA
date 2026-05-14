using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA;

namespace RIMA.Systems.Map
{
    public enum TerrainCollisionType
    {
        None,
        Wall,
        Cliff,
        Hazard
    }

    [System.Serializable]
    public class MapTerrain
    {
        public int id;
        public string name;
        public Color paletteColor = Color.gray;
        public TileBase baseTile;
        public CornerWangTileSetSO baseTileSource;

        [Header("Gameplay")]
        public bool walkable = true;
        [Range(0, 3)] public int elevationLevel;
        public TerrainCollisionType collisionType;

        [Header("Visual Variants")]
        public List<TileBase> variantPool = new List<TileBase>();
    }
}
