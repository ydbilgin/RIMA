using System;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [Serializable]
    public class MapObjectPlacement
    {
        public string id = Guid.NewGuid().ToString();
        public string prefabPath;
        public Vector2 positionPx;
        public int layer = 0;
        public bool visible = true;
        public string displayName;
    }
}
