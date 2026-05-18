#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public static class AutoPopulator
    {
        public const string PlacedPrefix = "_BlueprintPlaced_";
        private const string AdjacencyPrefix = "_BlueprintPlaced_adjacency_";

        public static int PopulateZones(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (canvas == null || profile == null || roomRoot == null || canvas.Count == 0)
            {
                return 0;
            }

            ClearPlacedProps(roomRoot);

            int placedCount = 0;
            BlueprintZoneTypeSO[] zones = profile.zones ?? Array.Empty<BlueprintZoneTypeSO>();
            for (int i = 0; i < zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = zones[i];
                if (zone == null || string.IsNullOrEmpty(zone.zoneId))
                {
                    continue;
                }

                List<Vector2Int> cells = SortedCells(canvas.CellsForZone(zone.zoneId));
                if (cells.Count == 0)
                {
                    continue;
                }

                BlueprintPropPoolSO pool = zone.propPool;
                if (pool == null || pool.entries == null || pool.entries.Length == 0 || pool.PickWeighted(seed) == null)
                {
                    Debug.LogWarning($"[Blueprint] Pool '{(pool != null ? pool.poolId : "null")}' is empty, skipping zone '{zone.zoneId}'");
                    continue;
                }

                if (IsFeatureZone(zone))
                {
                    placedCount += PopulateFeatureZone(canvas, zone, pool, roomRoot, cells, seed);
                }
                else
                {
                    placedCount += PopulateRegularZone(zone, pool, roomRoot, cells, seed);
                }
            }

            return placedCount;
        }

        public static int PopulateAdjacency(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (canvas == null || profile == null || roomRoot == null || canvas.Count == 0)
            {
                return 0;
            }

            ClearPlacedPropsWithPrefix(roomRoot, AdjacencyPrefix);

            int placedCount = 0;
            foreach ((Vector2Int a, Vector2Int b) in canvas.BoundaryEdges())
            {
                string zoneA = canvas.GetZoneAt(a);
                string zoneB = canvas.GetZoneAt(b);
                BlueprintAdjacencyRuleSO rule = profile.GetRule(zoneA, zoneB);
                if (rule == null)
                {
                    Debug.Log($"[Blueprint] No adjacency rule for '{zoneA}'/'{zoneB}', skipping boundary.");
                    continue;
                }

                if (Stable01(seed, a.x, a.y, b.x, b.y, 201) > Mathf.Clamp01(rule.density))
                {
                    continue;
                }

                BlueprintPropPoolSO pool = rule.transitionPool;
                if (pool == null || pool.entries == null || pool.entries.Length == 0)
                {
                    Debug.LogWarning($"[Blueprint] Pool '{(pool != null ? pool.poolId : "null")}' is empty, skipping adjacency '{rule.ruleId}'");
                    continue;
                }

                PropDefinitionSO prop = pool.PickWeighted(StableSeed(seed, a.x, a.y, b.x, b.y, 202));
                if (prop == null)
                {
                    Debug.LogWarning($"[Blueprint] Pool '{pool.poolId}' is empty, skipping adjacency '{rule.ruleId}'");
                    continue;
                }

                Vector3 worldPos = roomRoot.position + new Vector3((a.x + b.x) * 0.5f + 0.5f, (a.y + b.y) * 0.5f + 0.5f, 0f);
                string name = $"{AdjacencyPrefix}{zoneA}_{zoneB}_{a.x}_{a.y}_{b.x}_{b.y}";
                GameObject placed = PropPlacementService.PlacePropDefinition(prop, roomRoot, worldPos, name, true);
                if (placed != null)
                {
                    placedCount++;
                }
            }

            return placedCount;
        }

        public static int ClearPlacedProps(Transform roomRoot)
        {
            return ClearPlacedPropsWithPrefix(roomRoot, PlacedPrefix);
        }

        private static int PopulateRegularZone(BlueprintZoneTypeSO zone, BlueprintPropPoolSO pool, Transform roomRoot, List<Vector2Int> cells, int seed)
        {
            int placedCount = 0;
            float density = Mathf.Clamp01(zone.defaultDensity);
            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int cell = cells[i];
                if (Stable01(seed, cell.x, cell.y, i, 0, 101) > density)
                {
                    continue;
                }

                if (PlaceCellProp(zone.zoneId, pool, roomRoot, cell, seed, 102 + i))
                {
                    placedCount++;
                }
            }

            return placedCount;
        }

        private static int PopulateFeatureZone(BlueprintCanvas canvas, BlueprintZoneTypeSO zone, BlueprintPropPoolSO pool, Transform roomRoot, List<Vector2Int> cells, int seed)
        {
            int placedCount = 0;
            int maxPerRegion = Mathf.Max(1, zone.maxFeatureProps);
            List<List<Vector2Int>> regions = BuildRegions(canvas, zone.zoneId, cells);
            for (int i = 0; i < regions.Count; i++)
            {
                List<Vector2Int> region = regions[i];
                region.Sort((a, b) => StableSeed(seed, a.x, a.y, i, 0, 301).CompareTo(StableSeed(seed, b.x, b.y, i, 0, 301)));
                int densityTarget = Mathf.CeilToInt(region.Count * Mathf.Clamp01(zone.defaultDensity));
                int targetCount = Mathf.Clamp(densityTarget, 0, Mathf.Min(maxPerRegion, region.Count));
                if (targetCount == 0 && region.Count > 0 && zone.defaultDensity > 0f)
                {
                    targetCount = 1;
                }

                for (int c = 0; c < targetCount; c++)
                {
                    if (PlaceCellProp(zone.zoneId, pool, roomRoot, region[c], seed, 302 + c + i * 31))
                    {
                        placedCount++;
                    }
                }
            }

            return placedCount;
        }

        private static bool PlaceCellProp(string zoneId, BlueprintPropPoolSO pool, Transform roomRoot, Vector2Int cell, int seed, int salt)
        {
            PropDefinitionSO prop = pool.PickWeighted(StableSeed(seed, cell.x, cell.y, salt, 0, 401));
            if (prop == null)
            {
                return false;
            }

            Vector3 worldPos = roomRoot.position + new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);
            string name = $"{PlacedPrefix}{zoneId}_{cell.x}_{cell.y}";
            return PropPlacementService.PlacePropDefinition(prop, roomRoot, worldPos, name, true) != null;
        }

        private static List<List<Vector2Int>> BuildRegions(BlueprintCanvas canvas, string zoneId, List<Vector2Int> cells)
        {
            var cellSet = new HashSet<Vector2Int>(cells);
            var visited = new HashSet<Vector2Int>();
            var regions = new List<List<Vector2Int>>();

            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int start = cells[i];
                if (visited.Contains(start))
                {
                    continue;
                }

                var region = new List<Vector2Int>();
                var queue = new Queue<Vector2Int>();
                queue.Enqueue(start);
                visited.Add(start);

                while (queue.Count > 0)
                {
                    Vector2Int current = queue.Dequeue();
                    region.Add(current);
                    TryEnqueueRegionCell(new Vector2Int(current.x + 1, current.y), zoneId, canvas, cellSet, visited, queue);
                    TryEnqueueRegionCell(new Vector2Int(current.x - 1, current.y), zoneId, canvas, cellSet, visited, queue);
                    TryEnqueueRegionCell(new Vector2Int(current.x, current.y + 1), zoneId, canvas, cellSet, visited, queue);
                    TryEnqueueRegionCell(new Vector2Int(current.x, current.y - 1), zoneId, canvas, cellSet, visited, queue);
                }

                regions.Add(region);
            }

            return regions;
        }

        private static void TryEnqueueRegionCell(Vector2Int cell, string zoneId, BlueprintCanvas canvas, HashSet<Vector2Int> cellSet, HashSet<Vector2Int> visited, Queue<Vector2Int> queue)
        {
            if (!cellSet.Contains(cell) || visited.Contains(cell) || !string.Equals(canvas.GetZoneAt(cell), zoneId, StringComparison.Ordinal))
            {
                return;
            }

            visited.Add(cell);
            queue.Enqueue(cell);
        }

        private static int ClearPlacedPropsWithPrefix(Transform roomRoot, string prefix)
        {
            if (roomRoot == null)
            {
                return 0;
            }

            Transform[] children = roomRoot.GetComponentsInChildren<Transform>(true);
            var toDestroy = new List<GameObject>();
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                if (child != roomRoot && child.name.StartsWith(prefix, StringComparison.Ordinal))
                {
                    toDestroy.Add(child.gameObject);
                }
            }

            for (int i = 0; i < toDestroy.Count; i++)
            {
                Undo.DestroyObjectImmediate(toDestroy[i]);
            }

            return toDestroy.Count;
        }

        private static bool IsFeatureZone(BlueprintZoneTypeSO zone)
        {
            return string.Equals(zone.zoneId, "feature", StringComparison.OrdinalIgnoreCase) || zone.maxFeatureProps != 99;
        }

        private static List<Vector2Int> SortedCells(IEnumerable<Vector2Int> cells)
        {
            var result = new List<Vector2Int>(cells);
            result.Sort((a, b) =>
            {
                int y = a.y.CompareTo(b.y);
                return y != 0 ? y : a.x.CompareTo(b.x);
            });
            return result;
        }

        private static float Stable01(int seed, int a, int b, int c, int d, int salt)
        {
            uint hash = StableHash(seed, a, b, c, d, salt);
            return (hash & 0x00FFFFFFu) / 16777216f;
        }

        private static int StableSeed(int seed, int a, int b, int c, int d, int salt)
        {
            return unchecked((int)StableHash(seed, a, b, c, d, salt));
        }

        private static uint StableHash(int seed, int a, int b, int c, int d, int salt)
        {
            unchecked
            {
                uint hash = 2166136261u;
                Mix(ref hash, seed);
                Mix(ref hash, a);
                Mix(ref hash, b);
                Mix(ref hash, c);
                Mix(ref hash, d);
                Mix(ref hash, salt);
                hash ^= hash >> 13;
                hash *= 1274126177u;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static void Mix(ref uint hash, int value)
        {
            unchecked
            {
                hash ^= (uint)value;
                hash *= 16777619u;
            }
        }
    }
}
#endif
