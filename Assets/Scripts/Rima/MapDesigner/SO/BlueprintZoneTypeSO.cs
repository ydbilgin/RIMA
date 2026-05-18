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
