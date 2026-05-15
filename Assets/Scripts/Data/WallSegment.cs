using System;
using UnityEngine;

namespace RIMA.Data
{
    public enum WallDirection
    {
        North,
        South,
        East,
        West
    }

    [Serializable]
    public struct WallSegment
    {
        public Vector2Int start;
        public Vector2Int end;
        public WallDirection direction;
        public bool isCorner;
        public bool isDoorway;
    }
}
