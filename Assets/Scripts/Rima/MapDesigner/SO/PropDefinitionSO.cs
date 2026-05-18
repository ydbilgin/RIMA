using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(menuName = "RIMA/MapDesigner/PropDefinition")]
    public class PropDefinitionSO : ScriptableObject
    {
        public string propId;
        public Sprite visual;
        public Vector2Int footprint = Vector2Int.one;
        public bool hasCollision = false;
        public bool blocksMovement = false;
        public ColliderShape colliderShape = ColliderShape.None;
        [Range(0.3f, 1.0f)] public float colliderFootprintRatio = 0.7f;
        public Vector2 colliderOffset = Vector2.zero;
        public bool isTrigger = false;
        public string colliderLayer = "Walls";
        public Vector2 ySortPivot = new Vector2(0.5f, 0f);
        public List<string> validTerrainIds;
        public bool isFeatureAnchor = false;
        public Sprite shadowSprite;
        public Vector2 shadowOffset = new Vector2(0f, -0.08f);
        public Vector2 shadowScale = new Vector2(1.1f, 0.4f);
        [Range(0f, 1f)] public float shadowAlpha = 0.35f;
        public string lightingProfileId;

        public enum ColliderShape
        {
            None,
            Box,
            Circle,
            Capsule,
            PolygonAutoTrace
        }
    }
}
