using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Spawn orchestration: FragmentDropAnchor + RoomTypeData + FanLayoutSolver + Portal prefab.
    /// Usage: controller.SpawnPortals(anchor) once the combat clears.
    /// </summary>
    public sealed class PortalSpawnController : MonoBehaviour
    {
        [Tooltip("Optional prefab. If null a runtime GameObject with Portal + SpriteRenderer + CircleCollider2D is built.")]
        public Portal portalPrefab;

        [Tooltip("Grid that defines world->cell for walkability lookups. Auto-found from WalkabilityMap.floorTilemap if null.")]
        public Grid grid;

        [Tooltip("Override walkability map. Falls back to WalkabilityMap.Instance.")]
        public WalkabilityMap walkabilityMap;

        [Tooltip("Optional fixed seed for deterministic spawn (0 = time-based).")]
        public int randomSeed = 0;

        private readonly List<Portal> _activePortals = new();
        public IReadOnlyList<Portal> ActivePortals => _activePortals;

        /// <summary>
        /// Picks portal count via anchor.roomType, runs FanLayoutSolver, instantiates portals.
        /// Returns the spawned portals (empty array on BLOCKED).
        /// </summary>
        [ContextMenu("Test: Spawn From First Anchor")]
        public void SpawnFromFirstAnchorInScene()
        {
#if UNITY_2023_1_OR_NEWER
            var anchor = Object.FindFirstObjectByType<FragmentDropAnchor>();
#else
            var anchor = Object.FindObjectOfType<FragmentDropAnchor>();
#endif
            if (anchor == null) { Debug.LogWarning("[PortalSpawn] No FragmentDropAnchor in scene."); return; }
            SpawnPortals(anchor);
        }

        public Portal[] SpawnPortals(FragmentDropAnchor anchor)
        {
            ClearActivePortals();

            if (anchor == null)
            {
                Debug.LogWarning("[PortalSpawn] BLOCKED — anchor null.");
                return System.Array.Empty<Portal>();
            }

            var walk = walkabilityMap != null ? walkabilityMap : WalkabilityMap.Instance;
            if (walk == null)
            {
                Debug.LogWarning("[PortalSpawn] BLOCKED — WalkabilityMap.Instance null.");
                return System.Array.Empty<Portal>();
            }

            var resolvedGrid = grid;
            if (resolvedGrid == null && walk.floorTilemap != null)
            {
                resolvedGrid = walk.floorTilemap.layoutGrid;
            }
            if (resolvedGrid == null)
            {
                Debug.LogWarning("[PortalSpawn] BLOCKED — no Grid (assign field or set WalkabilityMap.floorTilemap).");
                return System.Array.Empty<Portal>();
            }

            var rng = randomSeed != 0 ? new System.Random(randomSeed) : new System.Random();
            int requested = anchor.roomType != null
                ? anchor.roomType.PickPortalCount(rng)
                : DefaultPickCount(rng);

            var solve = FanLayoutSolver.Solve(anchor.transform.position, anchor.fanDirection, requested, walk, resolvedGrid);

            if (solve.blocked)
            {
                Debug.LogWarning($"[PortalSpawn] BLOCKED — {solve.adjustmentNote} @ {anchor.transform.position}");
                return System.Array.Empty<Portal>();
            }

            Debug.Log($"[PortalSpawn] requested={requested} final={solve.finalCount} note=({solve.adjustmentNote})");

            var destinationPicks = PickDestinations(anchor, solve.finalCount, rng);

            var spawned = new Portal[solve.finalCount];
            for (int i = 0; i < solve.finalCount; i++)
            {
                spawned[i] = SpawnOne(solve.positions[i], destinationPicks[i], i);
            }
            _activePortals.AddRange(spawned);
            return spawned;
        }

        public void ClearActivePortals()
        {
            for (int i = 0; i < _activePortals.Count; i++)
            {
                if (_activePortals[i] == null) continue;
                if (Application.isPlaying) Destroy(_activePortals[i].gameObject);
                else DestroyImmediate(_activePortals[i].gameObject);
            }
            _activePortals.Clear();
        }

        private Portal SpawnOne(Vector3 worldPos, Portal.DestinationType destination, int index)
        {
            Portal portal;
            if (portalPrefab != null)
            {
                portal = Instantiate(portalPrefab, worldPos, Quaternion.identity, transform);
            }
            else
            {
                var go = new GameObject($"Portal_{index}_{destination}");
                go.transform.SetParent(transform, false);
                go.transform.position = worldPos;
                // RequireComponent on Portal pulls SpriteRenderer + CircleCollider2D.
                portal = go.AddComponent<Portal>();
            }
            portal.destination = destination;
            return portal;
        }

        private static Portal.DestinationType[] PickDestinations(FragmentDropAnchor anchor, int count, System.Random rng)
        {
            // Simple placeholder: combat anchors fan out to varied destinations, bridges go boss, treasure stays treasure.
            var picks = new Portal.DestinationType[count];
            var pool = new[]
            {
                Portal.DestinationType.Combat,
                Portal.DestinationType.Treasure,
                Portal.DestinationType.Ritual
            };

            if (anchor.roomType != null)
            {
                switch (anchor.roomType.category)
                {
                    case RoomTypeData.RoomCategory.BossApproach:
                        for (int i = 0; i < count; i++) picks[i] = Portal.DestinationType.BossApproach;
                        return picks;
                    case RoomTypeData.RoomCategory.Bridge:
                        for (int i = 0; i < count; i++) picks[i] = Portal.DestinationType.BossApproach;
                        return picks;
                    case RoomTypeData.RoomCategory.Treasure:
                        for (int i = 0; i < count; i++) picks[i] = Portal.DestinationType.Treasure;
                        return picks;
                }
            }

            for (int i = 0; i < count; i++) picks[i] = pool[rng.Next(pool.Length)];
            return picks;
        }

        private static int DefaultPickCount(System.Random rng)
        {
            // Default weights when no RoomTypeData supplied: 20 / 40 / 40.
            int roll = rng.Next(100);
            if (roll < 20) return 1;
            if (roll < 60) return 2;
            return 3;
        }
    }
}
