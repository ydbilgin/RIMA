using RIMA.MapDesigner;
using UnityEngine;

namespace RIMA.Data
{
    public enum NaturalFeatureSiteMode
    {
        UniformGridJitter
    }

    [CreateAssetMenu(fileName = "NaturalFeatureSettings", menuName = "RIMA/Map/Natural Feature Settings")]
    public class NaturalFeatureSettingsSO : ScriptableObject
    {
        public NaturalFeatureSiteMode siteMode = NaturalFeatureSiteMode.UniformGridJitter;
        [Range(64, 256)] public int siteCount = 64;
        public FeatureType featureType = FeatureType.Water;
        public int seed = 12345;
        [Range(0.01f, 1f)] public float featureSiteRatio = 0.18f;
    }
}
