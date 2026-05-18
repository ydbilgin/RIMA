using System;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "BlueprintPropPool", menuName = "RIMA/MapDesigner/Blueprint/Prop Pool")]
    public sealed class BlueprintPropPoolSO : ScriptableObject
    {
        public string poolId;
        public WeightedProp[] entries;

        public PropDefinitionSO PickWeighted(int seed)
        {
            if (entries == null || entries.Length == 0)
            {
                return null;
            }

            float totalWeight = 0f;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].prop != null && entries[i].weight > 0f)
                {
                    totalWeight += entries[i].weight;
                }
            }

            if (totalWeight <= 0f)
            {
                return null;
            }

            float cursor = Hash01(seed) * totalWeight;
            for (int i = 0; i < entries.Length; i++)
            {
                WeightedProp entry = entries[i];
                if (entry.prop == null || entry.weight <= 0f)
                {
                    continue;
                }

                cursor -= entry.weight;
                if (cursor <= 0f)
                {
                    return entry.prop;
                }
            }

            for (int i = entries.Length - 1; i >= 0; i--)
            {
                if (entries[i].prop != null && entries[i].weight > 0f)
                {
                    return entries[i].prop;
                }
            }

            return null;
        }

        private static float Hash01(int seed)
        {
            unchecked
            {
                uint value = (uint)seed;
                value ^= 2747636419u;
                value *= 2654435769u;
                value ^= value >> 16;
                value *= 2654435769u;
                value ^= value >> 16;
                value *= 2654435769u;
                return (value & 0x00FFFFFFu) / 16777216f;
            }
        }
    }

    [Serializable]
    public sealed class WeightedProp
    {
        public PropDefinitionSO prop;
        [Min(0f)] public float weight = 1f;
    }
}
