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

        public const int Layer1SortingOrder = -100;
        public const int Layer2SortingOrder = -90;
        public const int Layer3SortingOrder = -80;
        public const int Layer4SortingOrder = -70;
        public const int Layer5SortingOrder = -60;
        public const int Layer8SortingOrder = 100;

        public static int PopulateZones(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                return 0;
            }

            ClearPlacedProps(roomRoot);

            int total = 0;
            total += PopulateLayer1Macro(canvas, profile, roomRoot, seed);
            total += PopulateLayer2BaseFloor(canvas, profile, roomRoot, seed + 1);
            total += PopulateLayer3MidTone(canvas, profile, roomRoot, seed + 2);
            total += PopulateLayer4Detail(canvas, profile, roomRoot, seed + 3);
            total += PopulateLayer5SmallScatter(canvas, profile, roomRoot, seed + 4);
            total += PopulateLayer6Medium(canvas, profile, roomRoot, seed + 5);
            total += PopulateLayer7TallFocal(canvas, profile, roomRoot, seed + 6);
            total += PopulateLayer8Atmospheric(canvas, profile, roomRoot, seed + 7);
            return total;
        }

        public static int PopulateLayer1Macro(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulateSpriteLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                1,
                zone => zone.macroFillSprites,
                Layer1SortingOrder,
                false,
                "[Blueprint] Zone '{0}' missing Layer 1 macro fill, will use solid fallback color");
        }

        public static int PopulateLayer2BaseFloor(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulateSpriteLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                2,
                zone => zone.baseFloorSprites,
                Layer2SortingOrder,
                true,
                "[Blueprint] Zone '{0}' missing Layer 2 base floor sprites");
        }

        public static int PopulateLayer3MidTone(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulatePoolLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                3,
                zone => zone.midToneOverlayPool,
                zone => zone.midToneDensity,
                _ => Layer3SortingOrder,
                true,
                false);
        }

        public static int PopulateLayer4Detail(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulatePoolLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                4,
                zone => zone.detailTexturePool,
                zone => zone.detailDensity,
                _ => Layer4SortingOrder,
                true,
                false);
        }

        public static int PopulateLayer5SmallScatter(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulatePoolLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                5,
                zone => zone.smallScatterPool,
                zone => zone.smallScatterDensity,
                _ => Layer5SortingOrder,
                true,
                false);
        }

        public static int PopulateLayer6Medium(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulatePoolLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                6,
                zone => zone.mediumPropPool,
                zone => zone.mediumPropDensity,
                YSortingOrder,
                true,
                false);
        }

        public static int PopulateLayer7TallFocal(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                return 0;
            }

            int placedCount = 0;
            BlueprintZoneTypeSO[] zones = profile.zones ?? Array.Empty<BlueprintZoneTypeSO>();
            for (int i = 0; i < zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = zones[i];
                if (zone == null || string.IsNullOrEmpty(zone.zoneId) || !HasEntries(zone.tallFocalPool))
                {
                    continue;
                }

                List<Vector2Int> cells = SortedCells(canvas.CellsForZone(zone.zoneId));
                if (cells.Count == 0)
                {
                    continue;
                }

                List<List<Vector2Int>> regions = BuildRegions(canvas, zone.zoneId, cells);
                for (int regionIndex = 0; regionIndex < regions.Count; regionIndex++)
                {
                    List<Vector2Int> region = regions[regionIndex];
                    region.Sort((a, b) => StableSeed(seed, a.x, a.y, regionIndex, 0, 701).CompareTo(StableSeed(seed, b.x, b.y, regionIndex, 0, 701)));

                    int maxPerRegion = ResolveTallFocalCap(zone);
                    int targetCount = Mathf.Min(maxPerRegion, region.Count / 8);
                    for (int c = 0; c < targetCount; c++)
                    {
                        Vector2Int cell = region[c];
                        Vector3 worldPos = CellWorldPosition(roomRoot, cell, seed, 7, true);
                        if (PlacePoolProp(7, zone.zoneId, zone.tallFocalPool, roomRoot, cell, worldPos, seed, 702 + c + regionIndex * 31, YSortingOrder(worldPos), false))
                        {
                            placedCount++;
                        }
                    }
                }
            }

            return placedCount;
        }

        public static int PopulateLayer8Atmospheric(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            return PopulatePoolLayer(
                canvas,
                profile,
                roomRoot,
                seed,
                8,
                zone => zone.atmosphericPool,
                zone => zone.atmosphericDensity,
                _ => Layer8SortingOrder,
                false,
                true);
        }

        public static int PopulateAdjacency(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
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
                if (!HasEntries(pool))
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

        private static int PopulateSpriteLayer(
            BlueprintCanvas canvas,
            BlueprintProfileSO profile,
            Transform roomRoot,
            int seed,
            int layer,
            Func<BlueprintZoneTypeSO, Sprite[]> spriteSelector,
            int sortingOrder,
            bool criticalMissing,
            string missingMessage)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                return 0;
            }

            int placedCount = 0;
            var warnedZones = new HashSet<string>();
            List<KeyValuePair<Vector2Int, string>> cells = SortedIntentCells(canvas);
            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int cell = cells[i].Key;
                BlueprintZoneTypeSO zone = profile.GetZone(cells[i].Value);
                if (zone == null)
                {
                    continue;
                }

                Sprite sprite = PickSprite(spriteSelector(zone), seed, cell.x, cell.y, layer, 501);
                if (sprite == null)
                {
                    if (warnedZones.Add(zone.zoneId))
                    {
                        string message = string.Format(missingMessage, zone.zoneId);
                        if (criticalMissing)
                        {
                            Debug.LogError(message);
                        }
                        else
                        {
                            Debug.LogWarning(message);
                        }
                    }

                    continue;
                }

                Vector3 worldPos = CellWorldPosition(roomRoot, cell, seed, layer, false);
                string name = PlacedName(layer, zone.zoneId, cell);
                if (PropPlacementService.PlaceSprite(sprite, roomRoot, worldPos, name, sortingOrder) != null)
                {
                    placedCount++;
                }
            }

            return placedCount;
        }

        private static int PopulatePoolLayer(
            BlueprintCanvas canvas,
            BlueprintProfileSO profile,
            Transform roomRoot,
            int seed,
            int layer,
            Func<BlueprintZoneTypeSO, BlueprintPropPoolSO> poolSelector,
            Func<BlueprintZoneTypeSO, float> densitySelector,
            Func<Vector3, int> sortingOrderSelector,
            bool jitterPosition,
            bool rotationJitter)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                return 0;
            }

            int placedCount = 0;
            List<KeyValuePair<Vector2Int, string>> cells = SortedIntentCells(canvas);
            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int cell = cells[i].Key;
                BlueprintZoneTypeSO zone = profile.GetZone(cells[i].Value);
                if (zone == null)
                {
                    continue;
                }

                BlueprintPropPoolSO pool = poolSelector(zone);
                if (!HasEntries(pool))
                {
                    continue;
                }

                float density = Mathf.Clamp01(densitySelector(zone));
                if (Stable01(seed, cell.x, cell.y, layer, i, 601) > density)
                {
                    continue;
                }

                Vector3 worldPos = CellWorldPosition(roomRoot, cell, seed, layer, jitterPosition);
                if (PlacePoolProp(layer, zone.zoneId, pool, roomRoot, cell, worldPos, seed, 602 + i, sortingOrderSelector(worldPos), rotationJitter))
                {
                    placedCount++;
                }
            }

            return placedCount;
        }

        private static bool PlacePoolProp(
            int layer,
            string zoneId,
            BlueprintPropPoolSO pool,
            Transform roomRoot,
            Vector2Int cell,
            Vector3 worldPos,
            int seed,
            int salt,
            int sortingOrder,
            bool rotationJitter)
        {
            PropDefinitionSO prop = pool.PickWeighted(StableSeed(seed, cell.x, cell.y, layer, salt, 801));
            if (prop == null)
            {
                return false;
            }

            GameObject placed = PropPlacementService.PlacePropDefinition(prop, roomRoot, worldPos, PlacedName(layer, zoneId, cell), true);
            if (placed == null)
            {
                return false;
            }

            SpriteRenderer renderer = placed.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = sortingOrder;
            }

            if (rotationJitter)
            {
                float angle = Mathf.Lerp(0f, 360f, Stable01(seed, cell.x, cell.y, layer, salt, 802));
                placed.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            return true;
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

        private static bool CanPopulate(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot)
        {
            return canvas != null && profile != null && roomRoot != null && canvas.Count > 0;
        }

        private static bool HasEntries(BlueprintPropPoolSO pool)
        {
            if (pool == null || pool.entries == null || pool.entries.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < pool.entries.Length; i++)
            {
                WeightedProp entry = pool.entries[i];
                if (entry != null && entry.prop != null && entry.weight > 0f)
                {
                    return true;
                }
            }

            return false;
        }

        private static Sprite PickSprite(Sprite[] sprites, int seed, int x, int y, int layer, int salt)
        {
            if (sprites == null || sprites.Length == 0)
            {
                return null;
            }

            int start = (int)(StableHash(seed, x, y, layer, salt, 901) % (uint)sprites.Length);
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite sprite = sprites[(start + i) % sprites.Length];
                if (sprite != null)
                {
                    return sprite;
                }
            }

            return null;
        }

        private static int ResolveTallFocalCap(BlueprintZoneTypeSO zone)
        {
            if (zone.maxTallFocalPerRegion > 0)
            {
                return zone.maxTallFocalPerRegion;
            }

            return zone.maxFeatureProps == 99 ? 2 : Mathf.Max(0, zone.maxFeatureProps);
        }

        private static int YSortingOrder(Vector3 worldPos)
        {
            return Mathf.RoundToInt(-worldPos.y);
        }

        private static string PlacedName(int layer, string zoneId, Vector2Int cell)
        {
            return $"{PlacedPrefix}L{layer}_{zoneId}_{cell.x}_{cell.y}";
        }

        private static Vector3 CellWorldPosition(Transform roomRoot, Vector2Int cell, int seed, int layer, bool jitter)
        {
            float x = cell.x + 0.5f;
            float y = cell.y + 0.5f;
            if (jitter)
            {
                x += Mathf.Lerp(-0.3f, 0.3f, Stable01(seed, cell.x, cell.y, layer, 0, 1001));
                y += Mathf.Lerp(-0.3f, 0.3f, Stable01(seed, cell.x, cell.y, layer, 1, 1002));
            }

            return roomRoot.position + new Vector3(x, y, 0f);
        }

        private static List<KeyValuePair<Vector2Int, string>> SortedIntentCells(BlueprintCanvas canvas)
        {
            var result = new List<KeyValuePair<Vector2Int, string>>(canvas.IntentMap);
            result.Sort((a, b) =>
            {
                int y = a.Key.y.CompareTo(b.Key.y);
                return y != 0 ? y : a.Key.x.CompareTo(b.Key.x);
            });
            return result;
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
