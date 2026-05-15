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
        public bool allowFlipX;
        public bool allowFlipY;
        [Min(0f)] public float minDistance;
        public Vector2Int sortingOrderRange = new Vector2Int(0, 0);
    }

    [CreateAssetMenu(fileName = "PatchAtlas", menuName = "RIMA/Map/Patch Atlas")]
    public class PatchAtlasSO : ScriptableObject
    {
        public List<PatchEntry> patches = new List<PatchEntry>();
        public bool edgeBiased;
        [Min(0f)] public float wallProximityFactor = 1f;
        public FeatureMaskSO featureMask;
        [Range(0f, 1f)] public float featureMaskWeight = 0.5f;
        [Tooltip("Encounter slot positions to keep clear (gameplay readability). Density = 0 within radius.")]
        public float encounterAvoidRadius = 2f;
        [Tooltip("Center cells (>=4 tiles from wall) density multiplier. Karar #143-E center reduction.")]
        [Range(0f, 1f)] public float centerPathDensityReduction = 0.1f;
    }
}
