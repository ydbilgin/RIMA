using System;
using UnityEngine;

namespace RIMA.Rooms
{
    /// <summary>
    /// Defines a local-space room point where decor may be spawned.
    /// </summary>
    [Serializable]
    public struct OverlayAnchor
    {
        public Vector2 localPos;
        public DecorCategory category;
        public bool required;
        public float spawnWeight;
        public string optionalTag;
    }
}
