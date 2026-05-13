using UnityEngine;

namespace RIMA.Systems.Map
{
    [System.Serializable]
    public struct PropSpec
    {
        public GameObject prefab;
        public bool emitsLight;
        public PropLightKind lightSourceKind;
        public bool requiresVisibleSource;
        public int depthBandMin;
        public int depthBandMax;
        public string anchorTag;
    }
}
