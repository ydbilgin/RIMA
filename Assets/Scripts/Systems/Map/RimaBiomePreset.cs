using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(fileName = "BiomePreset", menuName = "RIMA/Biome Preset")]
    public class RimaBiomePreset : ScriptableObject
    {
        public RimaBiomeType biomeType;
        public string biomeName;
        [TextArea] public string description;

        public TileAssetMetadata[] allowedFloorTiles;
        public TileAssetMetadata[] allowedWallTiles;
        public TileAssetMetadata[] transitionTiles;
        public TileAssetMetadata[] decalTiles;

        public string[] allowedScatterTags;

        public Color paletteBaseColor = Color.gray;
        public Color paletteShadowColor = Color.black;

        [Range(0f, 1f)] public float decalDensity = 0.3f;
        [Range(0f, 1f)] public float scatterDensity = 0.2f;

        public bool useCliffFront = true;
        public bool useCliffTop = true;

        public List<MapTerrain> terrains = new List<MapTerrain>();
        public List<TilesetPairing> tilesetPairings = new List<TilesetPairing>();

        public TilesetPairing FindPairing(int t1, int t2)
        {
            foreach (TilesetPairing pairing in tilesetPairings)
            {
                if (pairing == null)
                {
                    continue;
                }

                if ((pairing.lowerTerrainId == t1 && pairing.upperTerrainId == t2) ||
                    (pairing.lowerTerrainId == t2 && pairing.upperTerrainId == t1))
                {
                    return pairing;
                }
            }

            return null;
        }

        public bool IsValidPair(int t1, int t2)
        {
            return t1 == t2 || FindPairing(t1, t2) != null;
        }
    }
}
