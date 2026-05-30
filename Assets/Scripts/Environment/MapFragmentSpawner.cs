using UnityEngine;
using RIMA.Systems.Map;

namespace RIMA.Environment
{
    /// <summary>
    /// Day 2.5: Subscribes to RoomLoader.OnRoomCleared.
    /// Finds first FragmentDropAnchor in scene + instantiates a MapFragment GameObject
    /// at the anchor position. Drop count from anchor.roomType.category:
    /// Combat=1, BossApproach=1 (Elite), others=0.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MapFragmentSpawner : MonoBehaviour
    {
        [Tooltip("Optional fragment prefab. If null a runtime GO with MapFragment + SpriteRenderer + CircleCollider2D is built.")]
        public MapFragment fragmentPrefab;

        // Passive helper — driven only via RoomLoader.SpawnFragmentThenDraftUnlock (SendMessage).
        // No OnRoomCleared auto-subscription → RoomLoader stays the single spawn authority (no double-spawn).
        private void HandleRoomCleared()
        {
            // Find anchor
            FragmentDropAnchor anchor =
#if UNITY_2023_1_OR_NEWER
                Object.FindFirstObjectByType<FragmentDropAnchor>();
#else
                Object.FindObjectOfType<FragmentDropAnchor>();
#endif
            if (anchor == null) { Debug.LogWarning("[MapFragmentSpawner] No FragmentDropAnchor in scene."); return; }

            int count = DropCountForRoom(anchor.roomType);
            if (count <= 0) { Debug.Log("[MapFragmentSpawner] Drop count=0 for room type — skipping."); return; }

            for (int i = 0; i < count; i++) SpawnFragment(anchor.transform.position);
        }

        private static int DropCountForRoom(RoomTypeData rt)
        {
            if (rt == null) return 1; // default fallback
            switch (rt.category)
            {
                case RoomTypeData.RoomCategory.Combat:       return 1;
                case RoomTypeData.RoomCategory.BossApproach: return 1; // Elite
                default: return 0;
            }
        }

        private void SpawnFragment(Vector3 position)
        {
            MapFragment fragment;
            if (fragmentPrefab != null)
            {
                fragment = Instantiate(fragmentPrefab, position, Quaternion.identity);
            }
            else
            {
                var go = new GameObject("MapFragment_AutoSpawn");
                go.transform.position = position;
                // Required components auto-added via [RequireComponent] on MapFragment
                fragment = go.AddComponent<MapFragment>();
            }
            Debug.Log($"[MapFragmentSpawner] Spawned MapFragment at {position}");
        }
    }
}
