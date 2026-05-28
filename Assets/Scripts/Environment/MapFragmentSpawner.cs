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

        [Tooltip("If true, spawner activates only when MapFragmentBridge.useFragmentGateFlow is also true at scene level. Default true.")]
        public bool gateOnBridgeFlag = true;

        private void OnEnable() { RoomLoader.OnRoomCleared += HandleRoomCleared; }
        private void OnDisable() { RoomLoader.OnRoomCleared -= HandleRoomCleared; }

        private void HandleRoomCleared()
        {
            if (gateOnBridgeFlag)
            {
                // Find bridge in scene; bail if useFragmentGateFlow=false (Day 1 portal flow active)
                var bridge =
#if UNITY_2023_1_OR_NEWER
                    Object.FindFirstObjectByType<MapFragmentBridge>();
#else
                    Object.FindObjectOfType<MapFragmentBridge>();
#endif
                if (bridge == null || !bridge.useFragmentGateFlow)
                {
                    Debug.Log("[MapFragmentSpawner] Skipped — useFragmentGateFlow=false (Day 1 portal flow).");
                    return;
                }
            }

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
