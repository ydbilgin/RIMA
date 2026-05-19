using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "BlueprintAdjacencyRule", menuName = "RIMA/MapDesigner/Blueprint/Adjacency Rule")]
    public sealed class BlueprintAdjacencyRuleSO : ScriptableObject
    {
        public string ruleId;
        public string zoneIdA;
        public string zoneIdB;
        public BlueprintPropPoolSO transitionPool;
        [Range(0f, 1f)] public float density = 0.5f;
        [Range(0, 30)] public int decalsPerRoomCap = 8;
    }
}
