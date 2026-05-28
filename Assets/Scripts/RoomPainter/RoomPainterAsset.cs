using System.Collections.Generic;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public enum ColliderShape
    {
        Box,
        Circle,
        Capsule,
        Polygon
    }

    public enum RoomPainterBoolOverride
    {
        UseLayerDefault,
        Disabled,
        Enabled
    }

    [CreateAssetMenu(fileName = "RoomPainterAsset", menuName = "RIMA/Room Painter/Asset")]
    public sealed class RoomPainterAsset : ScriptableObject
    {
        public string id;
        public string displayName;
        public string category;
        public Sprite sprite;
        public GameObject prefab;
        public RoomLayer defaultLayer = RoomLayer.Props;
        public string defaultSortingLayer = "Default";
        public int defaultOrder;
        public Vector2 defaultScale = Vector2.one;
        public Vector2 defaultVisualOffset = Vector2.zero;
        public bool ySortEnabled = true;
        public YSortAxis ySortAxisOverride = YSortAxis.UseLayerDefault;
        public Vector2 pivotAnchor = new Vector2(0.5f, 0f);

        public int parallaxTier;
        public float parallaxFactor;
        public RoomPainterBoolOverride cameraRelative = RoomPainterBoolOverride.UseLayerDefault;
        public RoomPainterBoolOverride pixelSnap = RoomPainterBoolOverride.UseLayerDefault;

        public bool isBlock;
        public RigidbodyType2D bodyType = RigidbodyType2D.Static;
        public ColliderShape colliderShape = ColliderShape.Box;
        public Vector2 colliderSize = Vector2.one;
        public bool isTrigger;
        public string physicsLayerName = "Default";
        public bool respectPrefabColliders = true;

        public Color tint = Color.white;
        public string materialOverridePath;
        public bool castShadow = true;
        public bool receiveLight = true;

        public List<string> tags = new List<string>();
        public string notes;
    }
}
