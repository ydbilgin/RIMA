using UnityEngine;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(fileName = "TerrainBlendConfig", menuName = "RIMA/Terrain Blend Config")]
    public class TerrainBlendConfig : ScriptableObject
    {
        [Tooltip("Terrain textures in channel order: R, G, B, A")]
        public Texture2D[] terrainTextures = new Texture2D[4];

        [Tooltip("Noise texture for organic edge breaking")]
        public Texture2D noiseTexture;

        [Range(0.5f, 8f)]
        public float terrainTiling = 2f;

        [Range(0.1f, 4f)]
        public float noiseScale = 0.8f;

        [Range(0f, 0.5f)]
        public float noiseStrength = 0.12f;

        [Range(0.5f, 10f)]
        public float blendSharpness = 2.5f;

        [Tooltip("Material using RIMA/TerrainBlend shader")]
        public Material blendMaterial;
    }
}
