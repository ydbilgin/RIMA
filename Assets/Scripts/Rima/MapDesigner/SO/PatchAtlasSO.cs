using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(menuName = "RIMA/MapDesigner/PatchAtlas")]
    public class PatchAtlasSO : ScriptableObject
    {
        public string atlasId;
        public PatchRole role;
        public List<string> validTerrainIds;
        public Sprite[] variants;
        [Range(0f, 1f)] public float density = 0.10f;
        [Min(0f)] public float minDistance = 2f;
        public bool edgeBiased = false;
        public bool wallProximityBiased = false;
        public AllowedTransforms allowedTransforms;
    }

    public enum PatchRole
    {
        BaseFloor,
        MacroPatch,
        OrganicDecal,
        DetailScatter,
        Accent
    }

    [System.Serializable]
    public struct AllowedTransforms
    {
        public bool flipX;
        public bool flipY;
        public bool rotate90;
        public bool rotate180;
        public bool rotate270;
        public Vector2 scaleRange;
        public Vector2 alphaRange;
    }
}
