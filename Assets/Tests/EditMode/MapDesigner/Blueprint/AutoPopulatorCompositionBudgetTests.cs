using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using RIMA.MapDesigner.SO;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class AutoPopulatorCompositionBudgetTests
    {
        private readonly List<Object> createdObjects = new List<Object>();
        private Transform root;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("CompositionBudgetRoot").transform;
            createdObjects.Add(root.gameObject);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                if (createdObjects[i] != null)
                {
                    Object.DestroyImmediate(createdObjects[i]);
                }
            }

            createdObjects.Clear();
            root = null;
        }

        [Test]
        public void CompositionBudget_NegativeSpaceRatioRespected()
        {
            RunBudgetedRoom(111);

            AutoPopulator.CompositionReport report = AutoPopulator.LastCompositionReport;
            float ratio = (float)report.NegativeSpaceCells.Count / report.TotalCells;

            Assert.That(ratio, Is.InRange(0.18f, 0.22f));
        }

        [Test]
        public void CompositionBudget_FloorWeightsWithinFivePercent()
        {
            RunBudgetedRoom(112);

            AutoPopulator.CompositionReport report = AutoPopulator.LastCompositionReport;
            int total = report.DominantFloorCount + report.SecondaryFloorCount + report.AccentFloorCount;

            Assert.That((float)report.DominantFloorCount / total, Is.InRange(0.65f, 0.75f));
            Assert.That((float)report.SecondaryFloorCount / total, Is.InRange(0.15f, 0.25f));
            Assert.That((float)report.AccentFloorCount / total, Is.InRange(0.05f, 0.15f));
        }

        [Test]
        public void CompositionBudget_PathCellsConnectedAndMinWidthWide()
        {
            RunBudgetedRoom(113);

            HashSet<Vector2Int> pathCells = AutoPopulator.LastCompositionReport.PathCells;

            Assert.IsTrue(IsConnected(pathCells));
            for (int x = 0; x < 20; x++)
            {
                int widthAtX = pathCells.Where(cell => cell.x == x).Select(cell => cell.y).Distinct().Count();
                Assert.That(widthAtX, Is.GreaterThanOrEqualTo(2));
            }
        }

        [Test]
        public void CompositionBudget_PathRatioReached()
        {
            RunBudgetedRoom(114);

            AutoPopulator.CompositionReport report = AutoPopulator.LastCompositionReport;

            Assert.That(report.PathCells.Count, Is.GreaterThanOrEqualTo(Mathf.CeilToInt(report.TotalCells * 0.15f)));
        }

        [Test]
        public void CompositionBudget_HeroClusterCountDoesNotExceedCap()
        {
            RunBudgetedRoom(115);

            AutoPopulator.CompositionReport report = AutoPopulator.LastCompositionReport;

            Assert.That(report.HeroClusters.Count, Is.LessThanOrEqualTo(report.HeroClusterCap));
        }

        [Test]
        public void CompositionBudget_EachClusterContainsTwoToFiveProps()
        {
            RunBudgetedRoom(116);

            List<AutoPopulator.HeroClusterReport> clusters = AutoPopulator.LastCompositionReport.HeroClusters;

            Assert.That(clusters.Count, Is.GreaterThan(0));
            for (int i = 0; i < clusters.Count; i++)
            {
                Assert.That(clusters[i].Cells.Count, Is.InRange(2, 5));
            }
        }

        [Test]
        public void CompositionBudget_ClusterBufferRespected()
        {
            RunBudgetedRoom(117);

            List<AutoPopulator.HeroClusterReport> clusters = AutoPopulator.LastCompositionReport.HeroClusters;

            for (int i = 0; i < clusters.Count; i++)
            {
                for (int j = i + 1; j < clusters.Count; j++)
                {
                    int buffer = Mathf.Max(clusters[i].Buffer, clusters[j].Buffer);
                    for (int a = 0; a < clusters[i].Cells.Count; a++)
                    {
                        for (int b = 0; b < clusters[j].Cells.Count; b++)
                        {
                            Assert.That(ChebyshevDistance(clusters[i].Cells[a], clusters[j].Cells[b]), Is.GreaterThan(buffer));
                        }
                    }
                }
            }
        }

        [Test]
        public void CompositionBudget_BackwardCompatFallsBackToLegacyWhenPoolsUnset()
        {
            BlueprintPropPoolSO pool = CreatePool("legacy_pool", CreateProp("legacy_prop"));
            BlueprintZoneTypeSO zone = CreateLegacyZone("legacy", pool);
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(1, 1, "legacy");

            int placed = AutoPopulator.PopulateZones(canvas, profile, root, 118);

            Assert.AreEqual(3, placed);
            Assert.IsNotNull(root.Find("_BlueprintPlaced_L6_legacy_0_0"));
            Assert.AreEqual(0, AutoPopulator.LastCompositionReport.TotalCells);
        }

        [Test]
        public void CompositionBudget_MetricsOutputIsParseable()
        {
            RunBudgetedRoom(119);

            string metrics = AutoPopulator.LastCompositionReport.ToMetricsString("CombatRoom");

            Assert.IsTrue(metrics.StartsWith("[v15d Metrics] CombatRoom"));
            Assert.IsTrue(metrics.Contains("Reserved cells:"));
            Assert.IsTrue(metrics.Contains("Budget check: OK"));
            CollectionAssert.IsEmpty(AutoPopulator.LastCompositionReport.BudgetFailures());
        }

        [Test]
        public void CompositionBudget_DeterministicForFixedSeed()
        {
            BlueprintZoneTypeSO zone = CreateBudgetZone("combat");
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(20, 10, "combat");
            Transform otherRoot = new GameObject("CompositionBudgetOtherRoot").transform;
            createdObjects.Add(otherRoot.gameObject);

            AutoPopulator.PopulateZones(canvas, profile, root, 120);
            AutoPopulator.PopulateZones(canvas, profile, otherRoot, 120);

            CollectionAssert.AreEqual(PlacementSignature(root), PlacementSignature(otherRoot));
        }

        [Test]
        public void CompositionBudget_ReservedCellsExcludedFromLayerTwoThroughSeven()
        {
            RunBudgetedRoom(121);

            HashSet<Vector2Int> reservedCells = AutoPopulator.LastCompositionReport.ReservedCells;
            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                if (child == root || !child.name.StartsWith(AutoPopulator.PlacedPrefix))
                {
                    continue;
                }

                int layer = ParseLayer(child.name);
                if (layer >= 2 && layer <= 7)
                {
                    Assert.IsFalse(reservedCells.Contains(ParseCell(child.name)), child.name);
                }
            }
        }

        [Test]
        public void CompositionBudget_AtmosphericCapRespectedPerZone()
        {
            BlueprintZoneTypeSO stone = CreateBudgetZone("stone");
            stone.atmosphericPool = CreatePool("stone_atmospheric_pool", CreateProp("stone_atmospheric"));
            stone.atmosphericDensity = 1f;
            stone.atmosphericCap = 3;

            BlueprintZoneTypeSO grass = CreateBudgetZone("grass");
            grass.atmosphericPool = CreatePool("grass_atmospheric_pool", CreateProp("grass_atmospheric"));
            grass.atmosphericDensity = 1f;
            grass.atmosphericCap = 2;

            BlueprintProfileSO profile = CreateProfile(stone, grass);
            BlueprintCanvas canvas = new BlueprintCanvas(new Vector2Int(10, 6));
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    canvas.Paint(new Vector2Int(x, y), "stone", 1);
                    canvas.Paint(new Vector2Int(x, y + 3), "grass", 1);
                }
            }

            AutoPopulator.PopulateZones(canvas, profile, root, 122);

            Assert.AreEqual(3, CountPlaced(8, "stone"));
            Assert.AreEqual(2, CountPlaced(8, "grass"));
            Assert.AreEqual(5, AutoPopulator.LastCompositionReport.AtmosphericCapTotal);
            Assert.AreEqual(5, AutoPopulator.LastCompositionReport.AtmosphericPropCount);
            CollectionAssert.IsEmpty(AutoPopulator.LastCompositionReport.BudgetFailures());
        }

        [Test]
        public void CompositionBudget_AtmosphericCapZeroUsesLegacyUnboundedPlacement()
        {
            BlueprintZoneTypeSO zone = CreateBudgetZone("legacy_fog");
            zone.atmosphericPool = CreatePool("legacy_fog_atmospheric_pool", CreateProp("legacy_fog_atmospheric"));
            zone.atmosphericDensity = 1f;
            zone.atmosphericCap = 0;

            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(12, 1, "legacy_fog");

            AutoPopulator.PopulateZones(canvas, profile, root, 123);

            Assert.AreEqual(12, CountPlaced(8, "legacy_fog"));
            Assert.AreEqual(12, AutoPopulator.LastCompositionReport.AtmosphericPropCount);
            Assert.AreEqual(0, AutoPopulator.LastCompositionReport.AtmosphericCapTotal);
            Assert.IsTrue(AutoPopulator.LastCompositionReport.HasUncappedAtmospheric);
        }

        [Test]
        public void CompositionBudget_SecondaryClusterCapRespected()
        {
            BlueprintZoneTypeSO zone = CreateBudgetZone("secondary_cap");
            zone.secondaryClusterCap = 2;
            zone.smallScatterDensity = 1f;

            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(20, 10, "secondary_cap");

            AutoPopulator.PopulateZones(canvas, profile, root, 124);

            AutoPopulator.CompositionReport report = AutoPopulator.LastCompositionReport;
            Assert.AreEqual(2, report.SecondaryClusterCapTotal);
            Assert.That(report.SecondaryClusters.Count, Is.LessThanOrEqualTo(2));
            Assert.That(CountPlaced(5, "secondary_cap"), Is.LessThanOrEqualTo(6));
            Assert.IsTrue(report.ToMetricsString("CombatRoom").Contains("Secondary clusters:"));
            CollectionAssert.IsEmpty(report.BudgetFailures());
        }

        [Test]
        public void CompositionBudget_SecondaryClusterCapZeroUsesLegacyUnboundedPlacement()
        {
            BlueprintZoneTypeSO zone = CreateBudgetZone("secondary_legacy");
            zone.secondaryClusterCap = 0;
            zone.smallScatterDensity = 1f;
            zone.detailTexturePool = null;
            zone.mediumPropPool = null;
            zone.tallFocalPool = null;
            zone.negativeSpaceRatio = 0f;
            zone.pathProtect = false;

            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(12, 1, "secondary_legacy");

            AutoPopulator.PopulateZones(canvas, profile, root, 125);

            AutoPopulator.CompositionReport report = AutoPopulator.LastCompositionReport;
            Assert.AreEqual(12, CountPlaced(5, "secondary_legacy"));
            Assert.AreEqual(0, report.SecondaryClusterCapTotal);
            Assert.IsTrue(report.HasUncappedSecondaryClusters);
            CollectionAssert.IsEmpty(report.BudgetFailures());
        }

        private void RunBudgetedRoom(int seed)
        {
            BlueprintZoneTypeSO zone = CreateBudgetZone("combat");
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(20, 10, "combat");

            AutoPopulator.PopulateZones(canvas, profile, root, seed);
        }

        private BlueprintCanvas FilledCanvas(int width, int height, string zoneId)
        {
            var canvas = new BlueprintCanvas(new Vector2Int(width, height));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    canvas.Paint(new Vector2Int(x, y), zoneId, 1);
                }
            }

            return canvas;
        }

        private BlueprintProfileSO CreateProfile(params BlueprintZoneTypeSO[] zones)
        {
            BlueprintProfileSO profile = ScriptableObject.CreateInstance<BlueprintProfileSO>();
            profile.profileId = "composition_budget_test_profile";
            profile.zones = zones;
            profile.adjacencyRules = new BlueprintAdjacencyRuleSO[0];
            profile.gridSize = new Vector2Int(20, 10);
            createdObjects.Add(profile);
            return profile;
        }

        private BlueprintZoneTypeSO CreateBudgetZone(string zoneId)
        {
            BlueprintPropPoolSO dominantPool = CreatePool(zoneId + "_dominant_pool", CreateProp(zoneId + "_dominant"));
            BlueprintPropPoolSO secondaryPool = CreatePool(zoneId + "_secondary_pool", CreateProp(zoneId + "_secondary"));
            BlueprintPropPoolSO accentPool = CreatePool(zoneId + "_accent_pool", CreateProp(zoneId + "_accent"));
            BlueprintPropPoolSO detailPool = CreatePool(zoneId + "_detail_pool", CreateProp(zoneId + "_detail"));
            BlueprintPropPoolSO smallPool = CreatePool(zoneId + "_small_pool", CreateProp(zoneId + "_small"));
            BlueprintPropPoolSO mediumPool = CreatePool(zoneId + "_medium_pool", CreateProp(zoneId + "_medium"));
            BlueprintPropPoolSO tallPool = CreatePool(zoneId + "_tall_pool", CreateProp(zoneId + "_tall"));

            BlueprintZoneTypeSO zone = ScriptableObject.CreateInstance<BlueprintZoneTypeSO>();
            zone.zoneId = zoneId;
            zone.displayName = zoneId;
            zone.macroFillSprites = new[] { dominantPool.entries[0].prop.visual };
            zone.baseFloorSprites = new[] { dominantPool.entries[0].prop.visual };
            zone.negativeSpaceRatio = 0.20f;
            zone.floorWeights = new Vector3(0.70f, 0.20f, 0.10f);
            zone.dominantFloorPool = dominantPool;
            zone.secondaryFloorPool = secondaryPool;
            zone.accentFloorPool = accentPool;
            zone.pathProtect = true;
            zone.heroPropClusterCap = 3;
            zone.heroPropClusterSize = new Vector2Int(2, 5);
            zone.heroPropClusterBuffer = 2;
            zone.pathCellRatio = 0.15f;
            zone.pathMinWidth = 2;
            zone.detailTexturePool = detailPool;
            zone.smallScatterPool = smallPool;
            zone.mediumPropPool = mediumPool;
            zone.tallFocalPool = tallPool;
            createdObjects.Add(zone);
            return zone;
        }

        private BlueprintZoneTypeSO CreateLegacyZone(string zoneId, BlueprintPropPoolSO pool)
        {
            BlueprintZoneTypeSO zone = ScriptableObject.CreateInstance<BlueprintZoneTypeSO>();
            zone.zoneId = zoneId;
            zone.displayName = zoneId;
            zone.macroFillSprites = new[] { pool.entries[0].prop.visual };
            zone.baseFloorSprites = new[] { pool.entries[0].prop.visual };
            zone.mediumPropPool = pool;
            zone.mediumPropDensity = 1f;
            createdObjects.Add(zone);
            return zone;
        }

        private BlueprintPropPoolSO CreatePool(string poolId, PropDefinitionSO prop)
        {
            BlueprintPropPoolSO pool = ScriptableObject.CreateInstance<BlueprintPropPoolSO>();
            pool.poolId = poolId;
            pool.entries = new[] { new WeightedProp { prop = prop, weight = 1f } };
            createdObjects.Add(pool);
            return pool;
        }

        private PropDefinitionSO CreateProp(string propId)
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.name = propId;
            prop.propId = propId;
            prop.visual = CreateSprite(propId + "_sprite");
            createdObjects.Add(prop);
            return prop;
        }

        private Sprite CreateSprite(string spriteName)
        {
            var texture = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            texture.name = spriteName + "_Texture";
            createdObjects.Add(texture);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 8f);
            sprite.name = spriteName;
            createdObjects.Add(sprite);
            return sprite;
        }

        private static bool IsConnected(HashSet<Vector2Int> cells)
        {
            if (cells.Count == 0)
            {
                return false;
            }

            var visited = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();
            Vector2Int start = cells.First();
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Vector2Int cell = queue.Dequeue();
                EnqueueIfPresent(cells, visited, queue, new Vector2Int(cell.x + 1, cell.y));
                EnqueueIfPresent(cells, visited, queue, new Vector2Int(cell.x - 1, cell.y));
                EnqueueIfPresent(cells, visited, queue, new Vector2Int(cell.x, cell.y + 1));
                EnqueueIfPresent(cells, visited, queue, new Vector2Int(cell.x, cell.y - 1));
            }

            return visited.Count == cells.Count;
        }

        private static void EnqueueIfPresent(HashSet<Vector2Int> cells, HashSet<Vector2Int> visited, Queue<Vector2Int> queue, Vector2Int cell)
        {
            if (cells.Contains(cell) && visited.Add(cell))
            {
                queue.Enqueue(cell);
            }
        }

        private static int ChebyshevDistance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
        }

        private static int ParseLayer(string objectName)
        {
            string marker = AutoPopulator.PlacedPrefix + "L";
            int start = objectName.IndexOf(marker, System.StringComparison.Ordinal);
            Assert.That(start, Is.GreaterThanOrEqualTo(0), objectName);
            int layerStart = start + marker.Length;
            int layerEnd = objectName.IndexOf('_', layerStart);
            return int.Parse(objectName.Substring(layerStart, layerEnd - layerStart));
        }

        private static Vector2Int ParseCell(string objectName)
        {
            string[] parts = objectName.Split('_');
            return new Vector2Int(int.Parse(parts[parts.Length - 2]), int.Parse(parts[parts.Length - 1]));
        }

        private int CountPlaced(int layer, string zoneId)
        {
            string prefix = AutoPopulator.PlacedPrefix + "L" + layer + "_" + zoneId + "_";
            return root.Cast<Transform>().Count(child => child.name.StartsWith(prefix));
        }

        private static string[] PlacementSignature(Transform targetRoot)
        {
            return targetRoot.Cast<Transform>()
                .Where(child => child.name.StartsWith(AutoPopulator.PlacedPrefix))
                .Select(child =>
                {
                    SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
                    string spriteName = renderer != null && renderer.sprite != null ? renderer.sprite.name : "null";
                    Vector3 pos = child.position;
                    int sortingOrder = renderer != null ? renderer.sortingOrder : 0;
                    return $"{child.name}|{spriteName}|{pos.x:F3}|{pos.y:F3}|{sortingOrder}";
                })
                .OrderBy(value => value)
                .ToArray();
        }
    }
}
