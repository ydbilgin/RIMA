using System;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public enum SegmentKind
    {
        SolidWall,
        VoidEdge,
        Entrance,
        BrokenGap,
        Anchor
    }

    [Serializable]
    public struct WallPiece
    {
        public GameObject prefab;
        public Sprite sprite;
        public Sprite straightSprite;
        public Sprite cornerSprite;
        public Sprite tSprite;
        public Sprite crossSprite;
        public Sprite endSprite;
        public Sprite singleSprite;
        public Vector2Int footprint;
        public string displayName;
        public string pieceId;
    }

    [Serializable]
    public struct WallSegment
    {
        public SegmentKind kind;
        public Vector3Int fromCell;
        public Vector3Int toCell;
        public WallPiece piece;
        public float height;
    }
}
