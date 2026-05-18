using System;
using UnityEngine;

namespace RIMA.MapDesigner.Data
{
    [Serializable]
    public struct IntentMapEntry
    {
        public Vector2Int pos;
        public string zoneId;

        public IntentMapEntry(Vector2Int pos, string zoneId)
        {
            this.pos = pos;
            this.zoneId = zoneId;
        }
    }
}
