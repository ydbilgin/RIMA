using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    public enum FeatureEdgeSmoothingMode
    {
        WangAndSprite,
        SpriteOnly
    }

    [CreateAssetMenu(fileName = "FeatureEdgeSmoothingProfile", menuName = "RIMA/Map/Feature Edge Smoothing Profile")]
    public class FeatureEdgeSmoothingProfileSO : ScriptableObject
    {
        public FeatureEdgeSmoothingMode smoothingMode = FeatureEdgeSmoothingMode.WangAndSprite;
        public List<Sprite> overlaySpriteSet = new List<Sprite>();
        [Min(1)] public int boundaryWidth = 1;
    }
}
