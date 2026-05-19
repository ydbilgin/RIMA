using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "BlueprintZoneType", menuName = "RIMA/MapDesigner/Blueprint/Zone Type")]
    public sealed class BlueprintZoneTypeSO : ScriptableObject
    {
        [Header("Identity")]
        public string zoneId;
        public string displayName;
        public Color brushColor = Color.white;
        [Range(0f, 1f)] public float defaultDensity = 1f;
        public int maxFeatureProps = 99;

        [Header("Composition Budget v15d")]
        [Tooltip("Fraction of zone cells reserved as intentional empty floor.")]
        [SerializeField, Range(0f, 0.4f)] public float negativeSpaceRatio = 0.20f;
        [Tooltip("Dominant, secondary, and accent floor ratios.")]
        [SerializeField] public Vector3 floorWeights = new Vector3(0.70f, 0.20f, 0.10f);
        [Tooltip("Layer 2 dominant/base floor prop pool.")]
        [SerializeField] public BlueprintPropPoolSO dominantFloorPool;
        [Tooltip("Layer 3 secondary floor overlay pool.")]
        [SerializeField] public BlueprintPropPoolSO secondaryFloorPool;
        [Tooltip("Layer 3 accent floor overlay pool.")]
        [SerializeField] public BlueprintPropPoolSO accentFloorPool;
        [Tooltip("Reserve a connected gameplay path before decorative placement.")]
        [SerializeField] public bool pathProtect = false;
        [Tooltip("Maximum total hero prop clusters for this composition budget.")]
        [SerializeField, Range(1, 6)] public int heroPropClusterCap = 3;
        [Tooltip("Minimum and maximum props per hero cluster.")]
        [SerializeField] public Vector2Int heroPropClusterSize = new Vector2Int(2, 5);
        [Tooltip("Empty-cell buffer around hero cluster footprints.")]
        [SerializeField, Range(1, 4)] public int heroPropClusterBuffer = 2;
        [Range(0, 8)]
        [Tooltip("Max L5 secondary prop clusters per zone. Default 4. Prevents purple crystal scatter from breaking hero cluster cap.")]
        public int secondaryClusterCap = 4;
        [Tooltip("Minimum reserved path cell ratio when path protection is enabled.")]
        [SerializeField, Range(0f, 0.3f)] public float pathCellRatio = 0.15f;
        [Tooltip("Minimum path corridor width in cells when path protection is enabled.")]
        [SerializeField, Range(1, 4)] public int pathMinWidth = 2;
        [Tooltip("Max L8 atmospheric props (mist/particles/scatter). 0=disabled, default 10. Prevents whole-room fog noise.")]
        [Range(0, 25)] public int atmosphericCap = 10;

        [Header("Layer 1 - Macro Ambient Fill (full coverage)")]
        public Sprite[] macroFillSprites;

        [Header("Layer 2 - Base Floor Tile (full coverage)")]
        public Sprite[] baseFloorSprites;

        [Header("Layer 3 - Mid-tone Gradient Overlay (sparse 30-50%)")]
        public BlueprintPropPoolSO midToneOverlayPool;
        [Range(0f, 1f)] public float midToneDensity = 0.4f;

        [Header("Layer 4 - Detail Texture (sparse 30%)")]
        public BlueprintPropPoolSO detailTexturePool;
        [Range(0f, 1f)] public float detailDensity = 0.3f;

        [Header("Layer 5 - Small Scatter (40%)")]
        public BlueprintPropPoolSO smallScatterPool;
        [Range(0f, 1f)] public float smallScatterDensity = 0.4f;

        [Header("Layer 6 - Medium Props (15%)")]
        public BlueprintPropPoolSO mediumPropPool;
        [Range(0f, 1f)] public float mediumPropDensity = 0.15f;

        [Header("Layer 7 - Tall Focal (capped per region)")]
        public BlueprintPropPoolSO tallFocalPool;
        public int maxTallFocalPerRegion = 2;

        [Header("Layer 8 - Atmospheric Overlay (10-30%)")]
        public BlueprintPropPoolSO atmosphericPool;
        [Range(0f, 1f)] public float atmosphericDensity = 0.2f;
    }
}
