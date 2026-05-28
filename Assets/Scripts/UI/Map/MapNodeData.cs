using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.UI.Map
{
    public enum MapNodeType
    {
        Combat,
        Elite,
        Rest,
        Boss,
        Event,
        Shop,
        CurseGate,
        Mystery,
        Entry
    }

    [Serializable]
    public class MapNodeData
    {
        public int id;
        public string displayName;
        public MapNodeType nodeType;
        public Vector2 position;
        public List<int> connections = new List<int>();
        public bool isVisited;
        public bool isCurrentRoom;
        public bool isRevealed = true;
        public int threatTier;

        public MapNodeData()
        {
        }

        public MapNodeData(int id, string displayName, MapNodeType nodeType, Vector2 position)
        {
            this.id = id;
            this.displayName = displayName;
            this.nodeType = nodeType;
            this.position = position;
            isRevealed = true;
        }
    }
}
