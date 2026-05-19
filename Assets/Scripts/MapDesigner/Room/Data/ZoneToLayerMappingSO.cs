using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [CreateAssetMenu(menuName = "RIMA/Room/ZoneToLayerMapping", fileName = "ZoneToLayerMapping_New", order = 210)]
    public class ZoneToLayerMappingSO : ScriptableObject
    {
        [System.Serializable]
        public class ZoneLayerEntry
        {
            public string zoneId = "stone";
            public string displayName = "Stone Floor";
            public Sprite sprite;
            public int defaultSortingOrder = -100;
            public Vector2 defaultOffset = Vector2.zero;
            public Vector2 defaultScale = Vector2.one;
            public Color defaultTint = Color.white;
        }

        public List<ZoneLayerEntry> zoneMap = new List<ZoneLayerEntry>();

        public ZoneLayerEntry GetForZone(string zoneId)
        {
            foreach (var entry in zoneMap)
            {
                if (entry != null && string.Equals(entry.zoneId, zoneId, System.StringComparison.OrdinalIgnoreCase))
                {
                    return entry;
                }
            }

            return null;
        }
    }
}
