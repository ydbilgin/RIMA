using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "BrushRadiusProfile_Default", menuName = "RIMA/Brush/Radius Profile", order = 120)]
    public class BrushRadiusProfileSO : ScriptableObject
    {
        [Serializable]
        public class RadiusBucketWeight
        {
            public int radius;
            public SizeBucket bucket;
            [Range(0f, 1f)] public float weight;
        }

        public List<RadiusBucketWeight> table = new List<RadiusBucketWeight>();

        public Dictionary<SizeBucket, float> ResolveWeights(int radius)
        {
            var result = new Dictionary<SizeBucket, float>();
            if (table == null)
            {
                return result;
            }

            for (int i = 0; i < table.Count; i++)
            {
                var entry = table[i];
                if (entry == null || entry.radius != radius || entry.weight <= 0f)
                {
                    continue;
                }

                if (!result.ContainsKey(entry.bucket))
                {
                    result[entry.bucket] = 0f;
                }

                result[entry.bucket] += entry.weight;
            }

            return result;
        }

        public void PopulateDefaultSoftOverlap()
        {
            table.Clear();
            Add(1, SizeBucket.Micro, 1.0f);
            Add(2, SizeBucket.Micro, 0.7f); Add(2, SizeBucket.Small, 0.3f);
            Add(3, SizeBucket.Small, 0.8f); Add(3, SizeBucket.Micro, 0.2f);
            Add(4, SizeBucket.Small, 0.5f); Add(4, SizeBucket.Medium, 0.5f);
            Add(5, SizeBucket.Medium, 0.8f); Add(5, SizeBucket.Small, 0.2f);
            Add(6, SizeBucket.Medium, 0.5f); Add(6, SizeBucket.Large, 0.5f);
            Add(7, SizeBucket.Large, 0.8f); Add(7, SizeBucket.Medium, 0.2f);
            Add(8, SizeBucket.Large, 0.6f); Add(8, SizeBucket.Hero, 0.4f);
            Add(9, SizeBucket.Hero, 0.8f); Add(9, SizeBucket.Large, 0.2f);
            Add(10, SizeBucket.Hero, 1.0f);
        }

        private void Add(int radius, SizeBucket bucket, float weight)
        {
            table.Add(new RadiusBucketWeight { radius = radius, bucket = bucket, weight = weight });
        }
    }
}
