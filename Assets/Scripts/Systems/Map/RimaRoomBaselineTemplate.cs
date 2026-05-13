using RIMA.Runtime.Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(menuName = "RIMA/Room Baseline Template")]
    public sealed class RimaRoomBaselineTemplate : ScriptableObject
    {
        public BiomeType biome = BiomeType.Keep;
        public string archetypeId = "combat";
        public int minWidth = 16;
        public int maxWidth = 20;
        public int minHeight = 16;
        public int maxHeight = 20;
        public TileBase[] floorVariants;
        public TileBase[] wallVariants;
        public Sprite[] decalSprites;
        [Range(0f, 1f)] public float decalDensity = 0.35f;
        public PropSpec[] propSpecs;
        public int generatorVersion = 1;
    }
}
