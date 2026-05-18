using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(menuName = "RIMA/MapDesigner/TerrainDefinition")]
    public class TerrainDefinitionSO : ScriptableObject
    {
        public string terrainId;
        public string displayName;
        public Sprite[] baseTileVariants;
        public bool walkable = true;
        public bool blocksMovement = false;
        public VisualCategory visualCategory;
        public Color averageColor = new Color(0.23f, 0.26f, 0.31f);
        [Range(0f, 1f)] public float defaultDecalDensity = 0.10f;
        [Range(0f, 1f)] public float defaultScatterDensity = 0.18f;
    }

    public enum VisualCategory
    {
        Stone,
        Dirt,
        Grass,
        Sand,
        Water,
        Lava,
        Wood,
        Metal,
        RiftFloor,
        Custom
    }
}
