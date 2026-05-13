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
    }
}
