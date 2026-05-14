using System.Collections.Generic;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Data
{
    [CreateAssetMenu(fileName = "TerrainDefinition", menuName = "RIMA/Map/Terrain Definition")]
    public class TerrainDefinition : ScriptableObject
    {
        public int terrainId;
        public string displayName;
        public Color paletteSwatch = Color.gray;
        public TileBase baseTile;
        public CornerWangTileSetSO baseTileSource;

        [Header("Gameplay")]
        public bool walkable = true;
        [Range(0, 3)] public int elevationLevel;
        public TerrainCollisionType collisionType;

        [Header("Visual Variants")]
        public List<TileBase> variantPool = new List<TileBase>();

        public MapTerrain ToMapTerrain()
        {
            return new MapTerrain
            {
                id = terrainId,
                name = displayName,
                paletteColor = paletteSwatch,
                baseTile = baseTile,
                baseTileSource = baseTileSource,
                walkable = walkable,
                elevationLevel = elevationLevel,
                collisionType = collisionType,
                variantPool = variantPool != null ? new List<TileBase>(variantPool) : new List<TileBase>()
            };
        }
    }
}
