#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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

        public static CompositionReport LastCompositionReport { get; private set; } = CompositionReport.Empty;

        public static int PopulateZones(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                LastCompositionReport = CompositionReport.Empty;
                return 0;
            }

            ClearPlacedProps(roomRoot);

            int total = 0;
            total += PopulateLayer1Macro(canvas, profile, roomRoot, seed);

            if (HasCompositionBudget(profile))
            {
                total += PopulateCompositionBudget(canvas, profile, roomRoot, seed + 1);
            }
            else
            {
                LastCompositionReport = CompositionReport.Empty;
                total += PopulateLayer2BaseFloor(canvas, profile, roomRoot, seed + 1);
                total += PopulateLayer3MidTone(canvas, profile, roomRoot, seed + 2);
                total += PopulateLayer4Detail(canvas, profile, roomRoot, seed + 3);
                total += PopulateLayer5SmallScatter(canvas, profile, roomRoot, seed + 4);
                total += PopulateLayer6Medium(canvas, profile, roomRoot, seed + 5);
                total += PopulateLayer7TallFocal(canvas, profile, roomRoot, seed + 6);
            }

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
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                return 0;
            }

            bool enforceCap = HasCompositionBudget(profile);
            if (enforceCap)
            {
                LastCompositionReport.ResetAtmospheric();
                RegisterAtmosphericCaps(canvas, profile);
            }

            int placedCount = 0;
            var placedByZone = new Dictionary<string, int>();
            List<KeyValuePair<Vector2Int, string>> cells = SortedIntentCells(canvas);
            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int cell = cells[i].Key;
                BlueprintZoneTypeSO zone = profile.GetZone(cells[i].Value);
                if (zone == null || !HasEntries(zone.atmosphericPool))
                {
                    continue;
                }

                int cap = Mathf.Clamp(zone.atmosphericCap, 0, 25);
                if (enforceCap && cap > 0 && placedByZone.TryGetValue(zone.zoneId, out int zonePlaced) && zonePlaced >= cap)
                {
                    continue;
                }

                float density = Mathf.Clamp01(zone.atmosphericDensity);
                if (Stable01(seed, cell.x, cell.y, 8, i, 601) > density)
                {
                    continue;
                }

                Vector3 worldPos = CellWorldPosition(roomRoot, cell, seed, 8, false);
                if (PlacePoolProp(8, zone.zoneId, zone.atmosphericPool, roomRoot, cell, worldPos, seed, 602 + i, Layer8SortingOrder, true))
                {
                    placedCount++;
                    if (!placedByZone.ContainsKey(zone.zoneId))
                    {
                        placedByZone[zone.zoneId] = 0;
                    }

                    placedByZone[zone.zoneId]++;
                    if (enforceCap)
                    {
                        LastCompositionReport.RegisterAtmosphericPlacement(zone);
                    }
                }
            }

            return placedCount;
        }

        public static int PopulateAdjacency(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            if (!CanPopulate(canvas, profile, roomRoot))
            {
                return 0;
            }

            ClearPlacedPropsWithPrefix(roomRoot, AdjacencyPrefix);

            int placedCount = 0;
            var placedByRule = new Dictionary<BlueprintAdjacencyRuleSO, int>();
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

                int cap = Mathf.Clamp(rule.decalsPerRoomCap, 0, 30);
                if (cap == 0)
                {
                    continue;
                }

                placedByRule.TryGetValue(rule, out int rulePlacedCount);
                if (rulePlacedCount >= cap)
                {
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
                    placedByRule[rule] = rulePlacedCount + 1;
                }
            }

            return placedCount;
        }

        public static int ClearPlacedProps(Transform roomRoot)
        {
            return ClearPlacedPropsWithPrefix(roomRoot, PlacedPrefix);
        }

        public static CompositionReport PreviewCompositionBudget(BlueprintCanvas canvas, BlueprintProfileSO profile, int seed)
        {
            if (canvas == null || profile == null || canvas.Count == 0)
            {
                return CompositionReport.Empty;
            }

            return BuildCompositionPlan(canvas, profile, seed).Report;
        }

        private static int PopulateCompositionBudget(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)
        {
            CompositionPlan plan = BuildCompositionPlan(canvas, profile, seed);
            CompositionReport report = plan.Report;
            LastCompositionReport = report;

            int placedCount = 0;
            for (int i = 0; i < plan.Placements.Count; i++)
            {
                PlannedPlacement placement = plan.Placements[i];
                Vector3 worldPos = CellWorldPosition(roomRoot, placement.Cell, seed, placement.Layer, placement.Jitter);
                int sortingOrder = SortingOrderForLayer(placement.Layer, worldPos);
                if (PlacePoolProp(placement.Layer, placement.ZoneId, placement.Pool, roomRoot, placement.Cell, worldPos, seed, placement.Salt, sortingOrder, placement.RotationJitter))
                {
                    placedCount++;
                    if (placement.Layer >= 0 && placement.Layer < report.LayerPropTotals.Length)
                    {
                        report.LayerPropTotals[placement.Layer]++;
                    }
                }
            }

            return placedCount;
        }

        private static CompositionPlan BuildCompositionPlan(BlueprintCanvas canvas, BlueprintProfileSO profile, int seed)
        {
            var plan = new CompositionPlan();
            CompositionReport report = plan.Report;
            report.TotalCells = canvas.Count;
            report.GridSize = canvas.GridSize;

            List<KeyValuePair<Vector2Int, string>> intentCells = SortedIntentCells(canvas);
            var cellsByZone = new Dictionary<string, List<Vector2Int>>();
            for (int i = 0; i < intentCells.Count; i++)
            {
                string zoneId = intentCells[i].Value;
                BlueprintZoneTypeSO zone = profile.GetZone(zoneId);
                if (!UsesCompositionBudget(zone))
                {
                    continue;
                }

                if (!cellsByZone.TryGetValue(zoneId, out List<Vector2Int> cells))
                {
                    cells = new List<Vector2Int>();
                    cellsByZone[zoneId] = cells;
                    report.ActiveZoneCount++;
                }

                cells.Add(intentCells[i].Key);
            }

            var clusterCandidates = new List<ClusterCandidate>();
            var secondaryClusterCandidates = new List<ClusterCandidate>();
            foreach (KeyValuePair<string, List<Vector2Int>> zonePair in cellsByZone)
            {
                BlueprintZoneTypeSO zone = profile.GetZone(zonePair.Key);
                List<Vector2Int> zoneCells = SortedCells(zonePair.Value);
                HashSet<Vector2Int> pathCells = BuildPathCells(zoneCells, zone, seed);
                foreach (Vector2Int cell in pathCells)
                {
                    report.PathCells.Add(cell);
                }

                int expectedPath = zone.pathProtect
                    ? Mathf.CeilToInt(zoneCells.Count * Mathf.Clamp(zone.pathCellRatio, 0f, 0.3f))
                    : 0;
                report.ExpectedPathCells += expectedPath;

                List<Vector2Int> negativeSpaceCells = SelectNegativeSpaceCells(zoneCells, pathCells, zone, seed);
                for (int i = 0; i < negativeSpaceCells.Count; i++)
                {
                    report.NegativeSpaceCells.Add(negativeSpaceCells[i]);
                }

                report.ExpectedNegativeSpaceCells += Mathf.RoundToInt(zoneCells.Count * Mathf.Clamp(zone.negativeSpaceRatio, 0f, 0.4f));

                var reservedCells = new HashSet<Vector2Int>(pathCells);
                for (int i = 0; i < negativeSpaceCells.Count; i++)
                {
                    reservedCells.Add(negativeSpaceCells[i]);
                }

                var unreservedCells = new List<Vector2Int>();
                for (int i = 0; i < zoneCells.Count; i++)
                {
                    Vector2Int cell = zoneCells[i];
                    if (!reservedCells.Contains(cell))
                    {
                        unreservedCells.Add(cell);
                    }
                }

                foreach (Vector2Int cell in reservedCells)
                {
                    report.ReservedCells.Add(cell);
                }

                AddFloorPlacements(plan, zone, zonePair.Key, unreservedCells, seed);
                AddClusterCandidates(clusterCandidates, zone, zonePair.Key, unreservedCells);
                AddSecondaryClusterCandidates(secondaryClusterCandidates, zone, zonePair.Key, unreservedCells, seed);
            }

            AddHeroClusterPlacements(plan, clusterCandidates, seed);
            AddSecondaryClusterPlacements(plan, secondaryClusterCandidates, seed);
            return plan;
        }

        private static void AddFloorPlacements(CompositionPlan plan, BlueprintZoneTypeSO zone, string zoneId, List<Vector2Int> unreservedCells, int seed)
        {
            if (unreservedCells.Count == 0)
            {
                return;
            }

            Vector3 weights = NormalizedFloorWeights(zone);
            int accentCount = Mathf.RoundToInt(unreservedCells.Count * weights.z);
            int secondaryCount = Mathf.RoundToInt(unreservedCells.Count * weights.y);
            if (secondaryCount + accentCount > unreservedCells.Count)
            {
                secondaryCount = Mathf.Max(0, unreservedCells.Count - accentCount);
            }

            CompositionReport report = plan.Report;
            report.ExpectedDominantFloorCount += Mathf.RoundToInt(unreservedCells.Count * weights.x);
            report.ExpectedSecondaryFloorCount += secondaryCount;
            report.ExpectedAccentFloorCount += accentCount;
            report.DominantFloorCount += Mathf.Max(0, unreservedCells.Count - secondaryCount - accentCount);
            report.SecondaryFloorCount += secondaryCount;
            report.AccentFloorCount += accentCount;

            for (int i = 0; i < unreservedCells.Count; i++)
            {
                if (HasEntries(zone.dominantFloorPool))
                {
                    plan.Placements.Add(new PlannedPlacement(2, zoneId, zone.dominantFloorPool, unreservedCells[i], 2000 + i, false, false));
                }
            }

            List<Vector2Int> overlayCells = SortedByStableSeed(unreservedCells, seed, 3101);
            int cursor = 0;
            for (int i = 0; i < accentCount && cursor < overlayCells.Count; i++, cursor++)
            {
                if (HasEntries(zone.accentFloorPool))
                {
                    plan.Placements.Add(new PlannedPlacement(3, zoneId, zone.accentFloorPool, overlayCells[cursor], 3000 + cursor, false, false));
                }
            }

            for (int i = 0; i < secondaryCount && cursor < overlayCells.Count; i++, cursor++)
            {
                if (HasEntries(zone.secondaryFloorPool))
                {
                    plan.Placements.Add(new PlannedPlacement(3, zoneId, zone.secondaryFloorPool, overlayCells[cursor], 3200 + cursor, false, false));
                }
            }
        }

        private static void AddClusterCandidates(List<ClusterCandidate> candidates, BlueprintZoneTypeSO zone, string zoneId, List<Vector2Int> unreservedCells)
        {
            if (!HasAnyHeroClusterPool(zone))
            {
                return;
            }

            for (int i = 0; i < unreservedCells.Count; i++)
            {
                candidates.Add(new ClusterCandidate(unreservedCells[i], zoneId, zone));
            }
        }

        private static void AddSecondaryClusterCandidates(List<ClusterCandidate> candidates, BlueprintZoneTypeSO zone, string zoneId, List<Vector2Int> unreservedCells, int seed)
        {
            if (!HasEntries(zone.smallScatterPool))
            {
                return;
            }

            float density = Mathf.Clamp01(zone.smallScatterDensity);
            for (int i = 0; i < unreservedCells.Count; i++)
            {
                Vector2Int cell = unreservedCells[i];
                if (Stable01(seed, cell.x, cell.y, 5, i, 5101) <= density)
                {
                    candidates.Add(new ClusterCandidate(cell, zoneId, zone));
                }
            }
        }

        private static void AddHeroClusterPlacements(CompositionPlan plan, List<ClusterCandidate> candidates, int seed)
        {
            if (candidates.Count == 0)
            {
                return;
            }

            candidates.Sort((a, b) => StableSeed(seed, a.Cell.x, a.Cell.y, 4, 0, 4101).CompareTo(StableSeed(seed, b.Cell.x, b.Cell.y, 4, 0, 4101)));

            int cap = 0;
            var cellsByZone = new Dictionary<string, List<Vector2Int>>();
            for (int i = 0; i < candidates.Count; i++)
            {
                ClusterCandidate candidate = candidates[i];
                cap = Mathf.Max(cap, Mathf.Clamp(candidate.Zone.heroPropClusterCap, 1, 6));
                if (!cellsByZone.TryGetValue(candidate.ZoneId, out List<Vector2Int> cells))
                {
                    cells = new List<Vector2Int>();
                    cellsByZone[candidate.ZoneId] = cells;
                }

                cells.Add(candidate.Cell);
            }

            plan.Report.HeroClusterCap = cap;
            var blockedCells = new HashSet<Vector2Int>();
            int clusterIndex = 0;
            for (int i = 0; i < candidates.Count && clusterIndex < cap; i++)
            {
                ClusterCandidate candidate = candidates[i];
                if (blockedCells.Contains(candidate.Cell))
                {
                    continue;
                }

                List<LayerPool> pools = ClusterPools(candidate.Zone);
                if (pools.Count == 0 || !cellsByZone.TryGetValue(candidate.ZoneId, out List<Vector2Int> zoneCells))
                {
                    continue;
                }

                int minSize = Mathf.Clamp(Mathf.Min(candidate.Zone.heroPropClusterSize.x, candidate.Zone.heroPropClusterSize.y), 2, 5);
                int maxSize = Mathf.Clamp(Mathf.Max(candidate.Zone.heroPropClusterSize.x, candidate.Zone.heroPropClusterSize.y), minSize, 5);
                int targetSize = minSize + Mathf.Abs(StableSeed(seed, candidate.Cell.x, candidate.Cell.y, clusterIndex, 0, 4201)) % (maxSize - minSize + 1);

                List<Vector2Int> clusterCells = BuildClusterCells(candidate.Cell, zoneCells, blockedCells, targetSize, minSize, seed, clusterIndex);
                if (clusterCells.Count < minSize)
                {
                    continue;
                }

                var cluster = new HeroClusterReport(clusterIndex, Mathf.Clamp(candidate.Zone.heroPropClusterBuffer, 1, 4), minSize, maxSize);
                for (int c = 0; c < clusterCells.Count; c++)
                {
                    Vector2Int cell = clusterCells[c];
                    LayerPool layerPool = pools[(clusterIndex + c) % pools.Count];
                    plan.Placements.Add(new PlannedPlacement(layerPool.Layer, candidate.ZoneId, layerPool.Pool, cell, 4000 + clusterIndex * 100 + c, true, false));
                    cluster.Cells.Add(cell);
                    cluster.Layers.Add(layerPool.Layer);
                }

                plan.Report.HeroClusters.Add(cluster);
                MarkClusterBuffer(blockedCells, clusterCells, cluster.Buffer);
                clusterIndex++;
            }
        }

        private static void AddSecondaryClusterPlacements(CompositionPlan plan, List<ClusterCandidate> candidates, int seed)
        {
            if (candidates.Count == 0)
            {
                return;
            }

            var candidatesByZone = new Dictionary<string, List<ClusterCandidate>>();
            for (int i = 0; i < candidates.Count; i++)
            {
                ClusterCandidate candidate = candidates[i];
                if (!candidatesByZone.TryGetValue(candidate.ZoneId, out List<ClusterCandidate> zoneCandidates))
                {
                    zoneCandidates = new List<ClusterCandidate>();
                    candidatesByZone[candidate.ZoneId] = zoneCandidates;
                    plan.Report.RegisterSecondaryClusterCap(candidate.Zone);
                }

                zoneCandidates.Add(candidate);
            }

            int globalClusterIndex = 0;
            foreach (KeyValuePair<string, List<ClusterCandidate>> zonePair in candidatesByZone)
            {
                List<ClusterCandidate> zoneCandidates = zonePair.Value;
                zoneCandidates.Sort((a, b) => StableSeed(seed, a.Cell.x, a.Cell.y, 5, 0, 5201).CompareTo(StableSeed(seed, b.Cell.x, b.Cell.y, 5, 0, 5201)));

                BlueprintZoneTypeSO zone = zoneCandidates[0].Zone;
                int cap = Mathf.Clamp(zone.secondaryClusterCap, 0, 8);
                if (cap == 0)
                {
                    AddLegacySecondaryPlacements(plan, zoneCandidates, seed);
                    continue;
                }

                var zoneCells = new List<Vector2Int>();
                for (int i = 0; i < zoneCandidates.Count; i++)
                {
                    zoneCells.Add(zoneCandidates[i].Cell);
                }

                var blockedCells = new HashSet<Vector2Int>();
                int zoneClusterIndex = 0;
                for (int i = 0; i < zoneCandidates.Count && zoneClusterIndex < cap; i++)
                {
                    ClusterCandidate candidate = zoneCandidates[i];
                    if (blockedCells.Contains(candidate.Cell))
                    {
                        continue;
                    }

                    int targetSize = 1 + Mathf.Abs(StableSeed(seed, candidate.Cell.x, candidate.Cell.y, zoneClusterIndex, 0, 5301)) % 3;
                    List<Vector2Int> clusterCells = BuildClusterCells(candidate.Cell, zoneCells, blockedCells, targetSize, 1, seed, globalClusterIndex);
                    if (clusterCells.Count == 0)
                    {
                        continue;
                    }

                    var cluster = new SecondaryClusterReport(globalClusterIndex, cap, 1, 3);
                    for (int c = 0; c < clusterCells.Count; c++)
                    {
                        Vector2Int cell = clusterCells[c];
                        plan.Placements.Add(new PlannedPlacement(5, candidate.ZoneId, candidate.Zone.smallScatterPool, cell, 5400 + globalClusterIndex * 100 + c, true, false));
                        cluster.Cells.Add(cell);
                    }

                    plan.Report.SecondaryClusters.Add(cluster);
                    MarkClusterBuffer(blockedCells, clusterCells, 2);
                    globalClusterIndex++;
                    zoneClusterIndex++;
                }
            }
        }

        private static void AddLegacySecondaryPlacements(CompositionPlan plan, List<ClusterCandidate> candidates, int seed)
        {
            for (int i = 0; i < candidates.Count; i++)
            {
                ClusterCandidate candidate = candidates[i];
                plan.Placements.Add(new PlannedPlacement(5, candidate.ZoneId, candidate.Zone.smallScatterPool, candidate.Cell, 5500 + i, true, false));
            }
        }

        private static HashSet<Vector2Int> BuildPathCells(List<Vector2Int> zoneCells, BlueprintZoneTypeSO zone, int seed)
        {
            var pathCells = new HashSet<Vector2Int>();
            if (zoneCells.Count == 0 || zone == null || !zone.pathProtect)
            {
                return pathCells;
            }

            int targetCount = Mathf.CeilToInt(zoneCells.Count * Mathf.Clamp(zone.pathCellRatio, 0f, 0.3f));
            if (targetCount <= 0)
            {
                return pathCells;
            }

            BoundsInt bounds = BoundsFor(zoneCells);
            int width = Mathf.Clamp(zone.pathMinWidth, 1, 4);
            int centerY = Mathf.RoundToInt((bounds.yMin + bounds.yMax - 1) * 0.5f);
            List<int> rows = PathRows(bounds.yMin, bounds.yMax - 1, centerY, width);
            var cellSet = new HashSet<Vector2Int>(zoneCells);

            for (int r = 0; r < rows.Count; r++)
            {
                int row = rows[r];
                for (int i = 0; i < zoneCells.Count; i++)
                {
                    Vector2Int cell = zoneCells[i];
                    if (cell.y == row)
                    {
                        pathCells.Add(cell);
                    }
                }

                if (pathCells.Count >= targetCount && r >= width - 1)
                {
                    break;
                }
            }

            ExpandConnectedCells(pathCells, cellSet, targetCount, seed);
            if (pathCells.Count < targetCount)
            {
                List<Vector2Int> fallback = SortedByStableSeed(zoneCells, seed, 4301);
                for (int i = 0; i < fallback.Count && pathCells.Count < targetCount; i++)
                {
                    pathCells.Add(fallback[i]);
                }
            }

            return pathCells;
        }

        private static List<Vector2Int> SelectNegativeSpaceCells(List<Vector2Int> zoneCells, HashSet<Vector2Int> pathCells, BlueprintZoneTypeSO zone, int seed)
        {
            int targetCount = Mathf.RoundToInt(zoneCells.Count * Mathf.Clamp(zone.negativeSpaceRatio, 0f, 0.4f));
            var available = new List<Vector2Int>();
            for (int i = 0; i < zoneCells.Count; i++)
            {
                if (!pathCells.Contains(zoneCells[i]))
                {
                    available.Add(zoneCells[i]);
                }
            }

            List<Vector2Int> ordered = SortedByStableSeed(available, seed, 4401);
            if (ordered.Count > targetCount)
            {
                ordered.RemoveRange(targetCount, ordered.Count - targetCount);
            }

            return ordered;
        }

        private static List<Vector2Int> BuildClusterCells(Vector2Int anchor, List<Vector2Int> zoneCells, HashSet<Vector2Int> blockedCells, int targetSize, int minSize, int seed, int clusterIndex)
        {
            var result = new List<Vector2Int> { anchor };
            var candidates = new List<Vector2Int>();
            for (int i = 0; i < zoneCells.Count; i++)
            {
                Vector2Int cell = zoneCells[i];
                if (cell == anchor || blockedCells.Contains(cell))
                {
                    continue;
                }

                int distance = ChebyshevDistance(anchor, cell);
                if (distance <= 2)
                {
                    candidates.Add(cell);
                }
            }

            candidates.Sort((a, b) =>
            {
                int distance = ChebyshevDistance(anchor, a).CompareTo(ChebyshevDistance(anchor, b));
                if (distance != 0)
                {
                    return distance;
                }

                return StableSeed(seed, a.x, a.y, clusterIndex, 0, 4501).CompareTo(StableSeed(seed, b.x, b.y, clusterIndex, 0, 4501));
            });

            for (int i = 0; i < candidates.Count && result.Count < targetSize; i++)
            {
                result.Add(candidates[i]);
            }

            if (result.Count < minSize)
            {
                result.Clear();
            }

            return result;
        }

        private static void MarkClusterBuffer(HashSet<Vector2Int> blockedCells, List<Vector2Int> clusterCells, int buffer)
        {
            for (int i = 0; i < clusterCells.Count; i++)
            {
                Vector2Int cell = clusterCells[i];
                for (int y = -buffer; y <= buffer; y++)
                {
                    for (int x = -buffer; x <= buffer; x++)
                    {
                        blockedCells.Add(new Vector2Int(cell.x + x, cell.y + y));
                    }
                }
            }
        }

        private static void ExpandConnectedCells(HashSet<Vector2Int> current, HashSet<Vector2Int> allowedCells, int targetCount, int seed)
        {
            while (current.Count < targetCount)
            {
                var candidates = new List<Vector2Int>();
                foreach (Vector2Int cell in current)
                {
                    AddPathExpansionCandidate(candidates, allowedCells, current, new Vector2Int(cell.x + 1, cell.y));
                    AddPathExpansionCandidate(candidates, allowedCells, current, new Vector2Int(cell.x - 1, cell.y));
                    AddPathExpansionCandidate(candidates, allowedCells, current, new Vector2Int(cell.x, cell.y + 1));
                    AddPathExpansionCandidate(candidates, allowedCells, current, new Vector2Int(cell.x, cell.y - 1));
                }

                if (candidates.Count == 0)
                {
                    return;
                }

                candidates = SortedByStableSeed(candidates, seed, 4601);
                current.Add(candidates[0]);
            }
        }

        private static void AddPathExpansionCandidate(List<Vector2Int> candidates, HashSet<Vector2Int> allowedCells, HashSet<Vector2Int> current, Vector2Int cell)
        {
            if (allowedCells.Contains(cell) && !current.Contains(cell) && !candidates.Contains(cell))
            {
                candidates.Add(cell);
            }
        }

        private static List<int> PathRows(int minY, int maxY, int centerY, int width)
        {
            var rows = new List<int>();
            int start = centerY - width / 2;
            for (int i = 0; i < width; i++)
            {
                int row = Mathf.Clamp(start + i, minY, maxY);
                if (!rows.Contains(row))
                {
                    rows.Add(row);
                }
            }

            for (int distance = 1; rows.Count < (maxY - minY + 1); distance++)
            {
                int upper = centerY + distance;
                int lower = centerY - distance;
                if (upper <= maxY && !rows.Contains(upper))
                {
                    rows.Add(upper);
                }

                if (lower >= minY && !rows.Contains(lower))
                {
                    rows.Add(lower);
                }

                if (upper > maxY && lower < minY)
                {
                    break;
                }
            }

            return rows;
        }

        private static BoundsInt BoundsFor(List<Vector2Int> cells)
        {
            int minX = cells[0].x;
            int minY = cells[0].y;
            int maxX = cells[0].x;
            int maxY = cells[0].y;
            for (int i = 1; i < cells.Count; i++)
            {
                Vector2Int cell = cells[i];
                minX = Mathf.Min(minX, cell.x);
                minY = Mathf.Min(minY, cell.y);
                maxX = Mathf.Max(maxX, cell.x);
                maxY = Mathf.Max(maxY, cell.y);
            }

            return new BoundsInt(minX, minY, 0, maxX - minX + 1, maxY - minY + 1, 1);
        }

        private static bool HasCompositionBudget(BlueprintProfileSO profile)
        {
            if (profile == null || profile.zones == null)
            {
                return false;
            }

            for (int i = 0; i < profile.zones.Length; i++)
            {
                if (UsesCompositionBudget(profile.zones[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static void RegisterAtmosphericCaps(BlueprintCanvas canvas, BlueprintProfileSO profile)
        {
            if (canvas == null || profile == null || profile.zones == null)
            {
                return;
            }

            for (int i = 0; i < profile.zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = profile.zones[i];
                if (zone == null || string.IsNullOrEmpty(zone.zoneId) || !HasEntries(zone.atmosphericPool))
                {
                    continue;
                }

                List<Vector2Int> cells = SortedCells(canvas.CellsForZone(zone.zoneId));
                if (cells.Count == 0)
                {
                    continue;
                }

                LastCompositionReport.RegisterAtmosphericCap(zone);
            }
        }

        private static bool UsesCompositionBudget(BlueprintZoneTypeSO zone)
        {
            return zone != null &&
                   (HasEntries(zone.dominantFloorPool) || HasEntries(zone.secondaryFloorPool) || HasEntries(zone.accentFloorPool));
        }

        private static bool HasAnyHeroClusterPool(BlueprintZoneTypeSO zone)
        {
            return HasEntries(zone.detailTexturePool) ||
                   HasEntries(zone.mediumPropPool) ||
                   HasEntries(zone.tallFocalPool);
        }

        private static List<LayerPool> ClusterPools(BlueprintZoneTypeSO zone)
        {
            var pools = new List<LayerPool>();
            if (HasEntries(zone.detailTexturePool))
            {
                pools.Add(new LayerPool(4, zone.detailTexturePool));
            }

            if (HasEntries(zone.mediumPropPool))
            {
                pools.Add(new LayerPool(6, zone.mediumPropPool));
            }

            if (HasEntries(zone.tallFocalPool))
            {
                pools.Add(new LayerPool(7, zone.tallFocalPool));
            }

            return pools;
        }

        private static Vector3 NormalizedFloorWeights(BlueprintZoneTypeSO zone)
        {
            Vector3 weights = zone.floorWeights;
            weights.x = Mathf.Max(0f, weights.x);
            weights.y = Mathf.Max(0f, weights.y);
            weights.z = Mathf.Max(0f, weights.z);
            float total = weights.x + weights.y + weights.z;
            if (total <= 0f)
            {
                return new Vector3(0.70f, 0.20f, 0.10f);
            }

            return weights / total;
        }

        private static int SortingOrderForLayer(int layer, Vector3 worldPos)
        {
            if (layer == 2)
            {
                return Layer2SortingOrder;
            }

            if (layer == 3)
            {
                return Layer3SortingOrder;
            }

            if (layer == 4)
            {
                return Layer4SortingOrder;
            }

            if (layer == 5)
            {
                return Layer5SortingOrder;
            }

            return YSortingOrder(worldPos);
        }

        private static List<Vector2Int> SortedByStableSeed(List<Vector2Int> cells, int seed, int salt)
        {
            var result = new List<Vector2Int>(cells);
            result.Sort((a, b) => StableSeed(seed, a.x, a.y, 0, 0, salt).CompareTo(StableSeed(seed, b.x, b.y, 0, 0, salt)));
            return result;
        }

        private static int ChebyshevDistance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
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

        public sealed class CompositionReport
        {
            public static CompositionReport Empty => new CompositionReport();

            public Vector2Int GridSize;
            public int TotalCells;
            public int ActiveZoneCount;
            public int ExpectedNegativeSpaceCells;
            public int ExpectedPathCells;
            public int ExpectedDominantFloorCount;
            public int ExpectedSecondaryFloorCount;
            public int ExpectedAccentFloorCount;
            public int DominantFloorCount;
            public int SecondaryFloorCount;
            public int AccentFloorCount;
            public int HeroClusterCap;
            public int SecondaryClusterCapTotal;
            public int AtmosphericPropCount;
            public int AtmosphericCappedPropCount;
            public int AtmosphericCapTotal;
            public bool HasUncappedSecondaryClusters;
            public bool HasUncappedAtmospheric;
            public readonly HashSet<Vector2Int> ReservedCells = new HashSet<Vector2Int>();
            public readonly HashSet<Vector2Int> PathCells = new HashSet<Vector2Int>();
            public readonly HashSet<Vector2Int> NegativeSpaceCells = new HashSet<Vector2Int>();
            public readonly List<HeroClusterReport> HeroClusters = new List<HeroClusterReport>();
            public readonly List<SecondaryClusterReport> SecondaryClusters = new List<SecondaryClusterReport>();
            public readonly int[] LayerPropTotals = new int[9];

            public void ResetAtmospheric()
            {
                AtmosphericPropCount = 0;
                AtmosphericCappedPropCount = 0;
                AtmosphericCapTotal = 0;
                HasUncappedAtmospheric = false;
                LayerPropTotals[8] = 0;
            }

            public void RegisterAtmosphericCap(BlueprintZoneTypeSO zone)
            {
                int cap = zone != null ? Mathf.Clamp(zone.atmosphericCap, 0, 25) : 0;
                if (cap > 0)
                {
                    AtmosphericCapTotal += cap;
                }
                else
                {
                    HasUncappedAtmospheric = true;
                }
            }

            public void RegisterSecondaryClusterCap(BlueprintZoneTypeSO zone)
            {
                int cap = zone != null ? Mathf.Clamp(zone.secondaryClusterCap, 0, 8) : 0;
                if (cap > 0)
                {
                    SecondaryClusterCapTotal += cap;
                }
                else
                {
                    HasUncappedSecondaryClusters = true;
                }
            }

            public void RegisterAtmosphericPlacement(BlueprintZoneTypeSO zone)
            {
                AtmosphericPropCount++;
                LayerPropTotals[8]++;
                if (zone != null && Mathf.Clamp(zone.atmosphericCap, 0, 25) > 0)
                {
                    AtmosphericCappedPropCount++;
                }
            }

            public string ToMetricsString(string label)
            {
                var builder = new StringBuilder();
                builder.AppendLine($"[v15d Metrics] {label} {GridSize.x}x{GridSize.y} = {TotalCells} cells");
                builder.AppendLine($"  Reserved cells: {ReservedCells.Count} ({Percent(ReservedCells.Count)}) -- path: {PathCells.Count} ({Percent(PathCells.Count)}), neg space: {NegativeSpaceCells.Count} ({Percent(NegativeSpaceCells.Count)})");
                builder.AppendLine($"  Floor split: dominant {DominantFloorCount} ({Percent(DominantFloorCount)}), secondary {SecondaryFloorCount} ({Percent(SecondaryFloorCount)}), accent {AccentFloorCount} ({Percent(AccentFloorCount)})");
                builder.AppendLine($"  Hero clusters: {HeroClusters.Count} / {HeroClusterCap} cap -- sizes: [{ClusterSizes()}] = {ClusterPropCount()} props");
                builder.AppendLine($"  Secondary clusters: {SecondaryClusters.Count} / {SecondaryClusterCapText()} cap -- {SecondaryClusterStatus()}");
                builder.AppendLine($"  L8 atmospheric: {AtmosphericPropCount} / {AtmosphericCapText()} cap -- {AtmosphericCapStatus()}");
                builder.AppendLine($"  Layer prop totals: L4={LayerPropTotals[4]}, L5={LayerPropTotals[5]}, L6={LayerPropTotals[6]}, L7={LayerPropTotals[7]}, L8={LayerPropTotals[8]}");

                List<string> failures = BudgetFailures();
                if (failures.Count == 0)
                {
                    builder.AppendLine("  Budget check: OK neg space, OK floor weights, OK cluster cap, OK secondary cap, OK path ratio");
                }
                else
                {
                    builder.AppendLine("  Budget check: WARNING " + string.Join(", ", failures.ToArray()));
                }

                return builder.ToString();
            }

            public List<string> BudgetFailures()
            {
                var failures = new List<string>();
                if (TotalCells <= 0)
                {
                    failures.Add("empty room");
                    return failures;
                }

                if (ExpectedNegativeSpaceCells > 0 && Mathf.Abs((float)NegativeSpaceCells.Count / TotalCells - (float)ExpectedNegativeSpaceCells / TotalCells) > 0.025f)
                {
                    failures.Add("neg space");
                }

                if (ExpectedPathCells > 0 && PathCells.Count < ExpectedPathCells)
                {
                    failures.Add("path ratio");
                }

                int actualFloorTotal = DominantFloorCount + SecondaryFloorCount + AccentFloorCount;
                int expectedFloorTotal = ExpectedDominantFloorCount + ExpectedSecondaryFloorCount + ExpectedAccentFloorCount;
                if (actualFloorTotal > 0 && expectedFloorTotal > 0)
                {
                    if (!RatioWithinTolerance(DominantFloorCount, actualFloorTotal, ExpectedDominantFloorCount, expectedFloorTotal) ||
                        !RatioWithinTolerance(SecondaryFloorCount, actualFloorTotal, ExpectedSecondaryFloorCount, expectedFloorTotal) ||
                        !RatioWithinTolerance(AccentFloorCount, actualFloorTotal, ExpectedAccentFloorCount, expectedFloorTotal))
                    {
                        failures.Add("floor weights");
                    }
                }

                if (HeroClusterCap > 0 && HeroClusters.Count > HeroClusterCap)
                {
                    failures.Add("cluster cap");
                }

                if (SecondaryClusterCapTotal > 0 && SecondaryClusters.Count > SecondaryClusterCapTotal)
                {
                    failures.Add("secondary cluster cap");
                }

                if (AtmosphericCapTotal > 0 && AtmosphericCappedPropCount > AtmosphericCapTotal)
                {
                    failures.Add("L8 cap");
                }

                for (int i = 0; i < HeroClusters.Count; i++)
                {
                    HeroClusterReport cluster = HeroClusters[i];
                    if (cluster.Cells.Count < cluster.MinSize || cluster.Cells.Count > cluster.MaxSize)
                    {
                        failures.Add("cluster size");
                        break;
                    }
                }

                for (int i = 0; i < SecondaryClusters.Count; i++)
                {
                    SecondaryClusterReport cluster = SecondaryClusters[i];
                    if (cluster.Cells.Count < cluster.MinSize || cluster.Cells.Count > cluster.MaxSize)
                    {
                        failures.Add("secondary cluster size");
                        break;
                    }
                }

                if (!ClusterBuffersClear())
                {
                    failures.Add("cluster buffer");
                }

                return failures;
            }

            private string SecondaryClusterCapText()
            {
                if (SecondaryClusterCapTotal <= 0)
                {
                    return HasUncappedSecondaryClusters ? "legacy unbounded" : "0";
                }

                return SecondaryClusterCapTotal.ToString(CultureInfo.InvariantCulture);
            }

            private string SecondaryClusterStatus()
            {
                if (SecondaryClusterCapTotal <= 0)
                {
                    return HasUncappedSecondaryClusters ? "LEGACY" : "OK";
                }

                return SecondaryClusters.Count <= SecondaryClusterCapTotal ? "OK" : "WARNING";
            }

            private string AtmosphericCapText()
            {
                if (AtmosphericCapTotal <= 0)
                {
                    return HasUncappedAtmospheric ? "legacy unbounded" : "0";
                }

                return AtmosphericCapTotal.ToString(CultureInfo.InvariantCulture);
            }

            private string AtmosphericCapStatus()
            {
                if (AtmosphericCapTotal <= 0)
                {
                    return HasUncappedAtmospheric ? "LEGACY" : "OK";
                }

                return AtmosphericCappedPropCount <= AtmosphericCapTotal ? "OK" : "WARNING";
            }

            private string Percent(int count)
            {
                float percent = TotalCells > 0 ? count * 100f / TotalCells : 0f;
                return percent.ToString("0.0", CultureInfo.InvariantCulture) + "%";
            }

            private int ClusterPropCount()
            {
                int count = 0;
                for (int i = 0; i < HeroClusters.Count; i++)
                {
                    count += HeroClusters[i].Cells.Count;
                }

                return count;
            }

            private string ClusterSizes()
            {
                var sizes = new string[HeroClusters.Count];
                for (int i = 0; i < HeroClusters.Count; i++)
                {
                    sizes[i] = HeroClusters[i].Cells.Count.ToString();
                }

                return string.Join(", ", sizes);
            }

            private bool ClusterBuffersClear()
            {
                for (int i = 0; i < HeroClusters.Count; i++)
                {
                    for (int j = i + 1; j < HeroClusters.Count; j++)
                    {
                        int buffer = Mathf.Max(HeroClusters[i].Buffer, HeroClusters[j].Buffer);
                        for (int a = 0; a < HeroClusters[i].Cells.Count; a++)
                        {
                            for (int b = 0; b < HeroClusters[j].Cells.Count; b++)
                            {
                                if (ChebyshevDistance(HeroClusters[i].Cells[a], HeroClusters[j].Cells[b]) <= buffer)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }

            private static bool RatioWithinTolerance(int actual, int actualTotal, int expected, int expectedTotal)
            {
                return Mathf.Abs((float)actual / actualTotal - (float)expected / expectedTotal) <= 0.05f;
            }
        }

        public sealed class HeroClusterReport
        {
            public readonly int Index;
            public readonly int Buffer;
            public readonly int MinSize;
            public readonly int MaxSize;
            public readonly List<Vector2Int> Cells = new List<Vector2Int>();
            public readonly List<int> Layers = new List<int>();

            public HeroClusterReport(int index, int buffer, int minSize, int maxSize)
            {
                Index = index;
                Buffer = buffer;
                MinSize = minSize;
                MaxSize = maxSize;
            }
        }

        public sealed class SecondaryClusterReport
        {
            public readonly int Index;
            public readonly int Cap;
            public readonly int MinSize;
            public readonly int MaxSize;
            public readonly List<Vector2Int> Cells = new List<Vector2Int>();

            public SecondaryClusterReport(int index, int cap, int minSize, int maxSize)
            {
                Index = index;
                Cap = cap;
                MinSize = minSize;
                MaxSize = maxSize;
            }
        }

        private sealed class CompositionPlan
        {
            public readonly CompositionReport Report = new CompositionReport();
            public readonly List<PlannedPlacement> Placements = new List<PlannedPlacement>();
        }

        private sealed class PlannedPlacement
        {
            public readonly int Layer;
            public readonly string ZoneId;
            public readonly BlueprintPropPoolSO Pool;
            public readonly Vector2Int Cell;
            public readonly int Salt;
            public readonly bool Jitter;
            public readonly bool RotationJitter;

            public PlannedPlacement(int layer, string zoneId, BlueprintPropPoolSO pool, Vector2Int cell, int salt, bool jitter, bool rotationJitter)
            {
                Layer = layer;
                ZoneId = zoneId;
                Pool = pool;
                Cell = cell;
                Salt = salt;
                Jitter = jitter;
                RotationJitter = rotationJitter;
            }
        }

        private sealed class ClusterCandidate
        {
            public readonly Vector2Int Cell;
            public readonly string ZoneId;
            public readonly BlueprintZoneTypeSO Zone;

            public ClusterCandidate(Vector2Int cell, string zoneId, BlueprintZoneTypeSO zone)
            {
                Cell = cell;
                ZoneId = zoneId;
                Zone = zone;
            }
        }

        private sealed class LayerPool
        {
            public readonly int Layer;
            public readonly BlueprintPropPoolSO Pool;

            public LayerPool(int layer, BlueprintPropPoolSO pool)
            {
                Layer = layer;
                Pool = pool;
            }
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
