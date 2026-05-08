using UnityEngine;

namespace RIMA.Systems.Map
{
    using RoomType = global::RIMA.RoomType;

    [CreateAssetMenu(menuName = "RIMA/Room Registry")]
    public class RoomRegistry : ScriptableObject
    {
        [System.Serializable]
        public class Entry
        {
            public RoomType roomType;
            public int depthBandMin;
            public int depthBandMax;
            public GameObject[] prefabs;
        }

        public Entry[] entries;

        public GameObject GetRandom(RoomType type, int depth)
        {
            var candidates = System.Array.FindAll(entries, e =>
                e.roomType == type && depth >= e.depthBandMin && depth <= e.depthBandMax);
            if (candidates.Length == 0) return null;
            var entry = candidates[Random.Range(0, candidates.Length)];
            if (entry.prefabs == null || entry.prefabs.Length == 0) return null;
            return entry.prefabs[Random.Range(0, entry.prefabs.Length)];
        }
    }
}
