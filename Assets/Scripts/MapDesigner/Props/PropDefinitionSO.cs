using RIMA.MapDesigner.Composition;
using UnityEngine;

namespace RIMA.MapDesigner.Props
{
    [CreateAssetMenu(menuName = "RIMA/MapDesigner/Props/PropDefinition", fileName = "NewPropDefinition")]
    public sealed class PropDefinitionSO : ScriptableObject
    {
        [Header("Identity")]
        public string propId;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public Sprite icon;
        public Sprite worldSprite;

        [Header("Footprint (V1: rect-based, no rotation)")]
        public Vector2Int footprintSize = Vector2Int.one;
        public Vector2Int spriteAnchor = Vector2Int.zero;

        [Header("Placement Rules")]
        public bool blocksWalkable = true;
        public bool requiresWalkableTile = false;
        public CompositionRole[] preferredRoles;
        public CompositionRole[] forbiddenRoles = new[]
        {
            CompositionRole.DoorSafety,
            CompositionRole.WallBand
        };
        public float distanceFromOtherProps = 1f;

        [Header("Karar #143 Compliance (Sprint 12 V1 LOCK)")]
        public bool respectsWalkableMask = true;

        [Header("Auto Collider (Phase B-2)")]
        public bool blocksMovement = false;
        public ColliderShape colliderShape = ColliderShape.None;
        [Range(0.3f, 1.0f)] public float colliderFootprintRatio = 0.7f;
        public Vector2 colliderOffset = Vector2.zero;
        public bool isTrigger = false;
        public string colliderLayer = "Walls";

        [Header("Visual Sorting")]
        public PropSortingMode sortingMode = PropSortingMode.YPosition;
        public int sortingLayerOverride = 0;
        public int sortingOrder = 0;

        [Header("Variant Pool (Sprint 13)")]
        [Tooltip("Optional alternative sprites. Empty = use worldSprite only. Populated = deterministic pick per placement seed.")]
        public Sprite[] variantSprites;

        public Sprite PickVariant(int seed)
        {
            int idx = PickVariantIndex(seed);
            if (idx < 0) return worldSprite;
            return variantSprites[idx] != null ? variantSprites[idx] : worldSprite;
        }

        public int PickVariantIndex(int seed)
        {
            if (variantSprites == null || variantSprites.Length == 0) return -1;
            int hashed = unchecked(seed * 1103515245 + 12345) & int.MaxValue;
            return hashed % variantSprites.Length;
        }

        public int PickVariantIndexForTile(Vector2Int tilePos)
        {
            return PickVariantIndex(StableTileSeed(tilePos));
        }

        public static int StableTileSeed(Vector2Int tilePos)
        {
            unchecked
            {
                return (tilePos.x * 73856093) ^ (tilePos.y * 19349663);
            }
        }

        public enum PropSortingMode
        {
            YPosition,
            FixedOrder,
            AboveAll
        }

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
