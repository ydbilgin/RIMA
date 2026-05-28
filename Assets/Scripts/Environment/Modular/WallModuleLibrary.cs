using UnityEngine;

namespace RIMA.Environment.Modular
{
    [CreateAssetMenu(fileName = "WallModuleLibrary", menuName = "RIMA/Environment/Wall Module Library")]
    public class WallModuleLibrary : ScriptableObject
    {
        [Header("N wall (faces -Z toward camera)")]
        public GameObject[] northStraightPrefabs;
        public GameObject[] northVariantPrefabs;

        [Header("W wall (faces +X)")]
        public GameObject[] westStraightPrefabs;
        public GameObject[] westVariantPrefabs;

        [Header("Corners")]
        public GameObject neOuterCornerPrefab;
        public GameObject nwInnerCornerPrefab;

        [Header("Detail")]
        public GameObject pillarPrefab;
        public int pillarEveryNCells = 3;

        [Header("Pillar protrusion")]
        [Range(0f, 1f)] public float pillarInteriorOffset = 0.4f;

        [Header("Floor")]
        public GameObject[] floorTilePrefabs;
        public GameObject sigilFloorPrefab;
        public float sigilFloorChance = 0.05f;

        [Header("Variation")]
        [Range(0f, 1f)] public float wallVariantChance = 0.25f;

        [Header("Damaged variants (optional)")]
        public GameObject[] breachPrefabs;
        public GameObject[] toppledPrefabs;
        public GameObject[] heavyPrefabs;

        [Range(0f, 1f)] public float breachChance = 0f;
        [Range(0f, 1f)] public float toppledChance = 0f;
        [Range(0f, 1f)] public float heavyChance = 0f;

        [Header("Theme tint")]
        public Color ambientTintRGB = Color.white;
        public bool tintWalls = false;
    }
}
