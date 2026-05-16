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

        [Header("Visual Sorting")]
        public PropSortingMode sortingMode = PropSortingMode.YPosition;
        public int sortingLayerOverride = 0;
        public int sortingOrder = 0;

        public enum PropSortingMode
        {
            YPosition,
            FixedOrder,
            AboveAll
        }
    }
}
