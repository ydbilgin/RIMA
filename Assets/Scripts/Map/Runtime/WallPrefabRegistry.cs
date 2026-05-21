using System;
using UnityEngine;

namespace RIMA.Map
{
    [CreateAssetMenu(fileName = "WallPrefabRegistry", menuName = "RIMA/Map/Wall Registry")]
    public class WallPrefabRegistry : ScriptableObject
    {
        [Serializable]
        public class WallEntry
        {
            public string wallId;
            public GameObject prefab;
        }

        public WallEntry[] walls;

        public GameObject GetPrefab(string wallId)
        {
            if (walls == null || string.IsNullOrEmpty(wallId)) return null;
            WallEntry entry = Array.Find(walls, w => w != null && w.wallId == wallId);
            return entry != null ? entry.prefab : null;
        }
    }
}
