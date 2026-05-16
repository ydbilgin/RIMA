using System;
using RIMA.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [Serializable]
    public class BrushAssetVariant
    {
        public Sprite sprite;
        public string variantId;
        public SizeBucket bucket;
        public float weight = 1f;
        public TargetLayer targetLayer;
        public Vector2Int nativeSize;
        public RectInt sourceRect;
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
        public float footprintRadius;
        public bool allowFlipX;
        public bool allowFlipY;
        public bool allowRotation;
        public float rotationSnapDegrees;
        public string[] tags;
        public bool respectsWalkableMask = true;
        public float minDistance = 32f;
        public float encounterAvoidRadius;
        public bool edgeBiased;
        public float wallProximityFactor = 1f;
        public FeatureMaskSO featureMaskMultiplier;
        public bool heroAllowed;
        public string schemaVersion = "1.0";
    }
}
