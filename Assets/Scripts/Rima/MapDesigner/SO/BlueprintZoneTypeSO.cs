using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "BlueprintZoneType", menuName = "RIMA/MapDesigner/Blueprint/Zone Type")]
    public sealed class BlueprintZoneTypeSO : ScriptableObject
    {
        public string zoneId;
        public string displayName;
        public Color brushColor = Color.white;
        public BlueprintPropPoolSO propPool;
        [Range(0f, 1f)] public float defaultDensity = 1f;
        public int maxFeatureProps = 99;
    }
}
