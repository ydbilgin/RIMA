using System;
using RIMA.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [Serializable]
    public class BrushLayerOperation
    {
        public TargetLayer targetLayer;
        public AssetPoolSO assetPool;
        [Range(0f, 1f)] public float density = 0.5f;
        [Range(0f, 1f)] public float probability = 1.0f;
        public float minDistance = 32f;
        public Vector2 scaleRange = new Vector2(0.85f, 1.15f);
        public bool allowRotation = true;
        public bool allowFlipX = true;
        public bool allowFlipY = false;
        public float rotationSnapDegrees = 0f;
        public Color tint = Color.white;
        public Vector2 positionJitter = Vector2.zero;
        public int sortingOrderOffset = 0;
        public bool affectsCollision = false;

        public bool respectsWalkableMask = true;
        public AnimationCurve wallProximityCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        public FeatureMaskSO featureMaskMultiplier;

        public bool useNativeBucketVariantPath = true;
        public int radiusForBucketPick = 4;

        public BrushPipelineConfigSO pipelineConfig;
        public RoomDecalDataSO roomDecalData;
        public RIMA.MapDesigner.SO.PatchAtlasSO patchAtlas;
    }
}
