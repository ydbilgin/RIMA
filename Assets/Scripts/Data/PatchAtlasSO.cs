using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    [Serializable]
    public class PatchEntry
    {
        public Sprite sprite;
        public Vector2 size = Vector2.one;
        [Range(0f, 1f)] public float density = 0.25f;
        public float rotationJitter = 15f;
        public Color tintMin = Color.white;
        public Color tintMax = Color.white;
    }

    [CreateAssetMenu(fileName = "PatchAtlas", menuName = "RIMA/Map/Patch Atlas")]
    public class PatchAtlasSO : ScriptableObject
    {
        public List<PatchEntry> patches = new List<PatchEntry>();
        public bool edgeBiased;
        [Min(0f)] public float wallProximityFactor = 1f;
        public FeatureMaskSO featureMask;
        [Range(0f, 1f)] public float featureMaskWeight = 0.5f;
    }
}
