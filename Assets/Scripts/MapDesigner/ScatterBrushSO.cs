using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner
{
    [Serializable]
    public class ScatterEntry
    {
        public Sprite sprite;
        public float perlinFrequency = 0.18f;
        [Range(0f, 1f)] public float perlinThreshold = 0.55f;
        public int minCount = 1;
        public int maxCount = 3;
    }

    [CreateAssetMenu(fileName = "ScatterBrush", menuName = "RIMA/Map/Scatter Brush")]
    public class ScatterBrushSO : ScriptableObject
    {
        public List<ScatterEntry> entries = new List<ScatterEntry>();
    }
}
