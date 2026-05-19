using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using RIMA.MapDesigner.Editor.Blueprint;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class V15gMinimalPixelLabTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            RimaV15gMinimalComposer.EnsureV15gAssetGraph();
        }

        [Test]
        public void V15gCompositionBudget_UsesStrictMinimalBudget()
        {
            BlueprintProfileSO profile = Load<BlueprintProfileSO>(RimaV15gMinimalComposer.ProfilePath);

            Assert.AreEqual("profile_v15g_minimal_pixellab", profile.profileId);
            Assert.AreEqual(3, profile.zones.Length);
            for (int i = 0; i < profile.zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = profile.zones[i];
                Assert.NotNull(zone);
                Assert.AreEqual(0.25f, zone.negativeSpaceRatio);
                Assert.AreEqual(new Vector3(0.75f, 0.18f, 0.07f), zone.floorWeights);
                Assert.AreEqual(2, zone.heroPropClusterCap);
                Assert.AreEqual(5, zone.atmosphericCap);
                Assert.IsTrue(zone.pathProtect);
                Assert.NotNull(zone.dominantFloorPool);
                Assert.NotNull(zone.secondaryFloorPool);
                Assert.NotNull(zone.accentFloorPool);
            }
        }

        [Test]
        public void V15gPixelLabTilePools_AreWiredToImportedSprites()
        {
            AssertPool("Assets/Data/Blueprint/PropPools/pool_v15g_pixellab_dominant.asset", 5);
            AssertPool("Assets/Data/Blueprint/PropPools/pool_v15g_pixellab_secondary.asset", 3);
            AssertPool("Assets/Data/Blueprint/PropPools/pool_v15g_pixellab_path.asset", 2);
            AssertPool("Assets/Data/Blueprint/PropPools/pool_v15g_pixellab_transition.asset", Is.GreaterThanOrEqualTo(8));

            int dirtSourceCount = Directory.GetFiles("STAGING/pixellab_dirt_v1", "*.png", SearchOption.TopDirectoryOnly).Length;
            AssertPool("Assets/Data/Blueprint/PropPools/pool_v15g_pixellab_dirt.asset", dirtSourceCount);
        }

        [Test]
        public void V15gProfile_DoesNotReferenceV15dZoneOrPoolAssets()
        {
            Assert.NotNull(AssetDatabase.LoadAssetAtPath<BlueprintZoneTypeSO>("Assets/Data/Blueprint/ZoneTypes/zone_stone.asset"));
            Assert.NotNull(AssetDatabase.LoadAssetAtPath<BlueprintPropPoolSO>("Assets/Data/Blueprint/PropPools/pool_v15d_combat_dominant.asset"));

            BlueprintProfileSO profile = Load<BlueprintProfileSO>(RimaV15gMinimalComposer.ProfilePath);
            for (int i = 0; i < profile.zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = profile.zones[i];
                string zonePath = AssetDatabase.GetAssetPath(zone);
                StringAssert.StartsWith("Assets/Data/Blueprint/ZoneTypes/v15g/", zonePath);
                AssertPoolPath(zone.dominantFloorPool);
                AssertPoolPath(zone.secondaryFloorPool);
                AssertPoolPath(zone.accentFloorPool);
                AssertPoolPath(zone.detailTexturePool);
                AssertPoolPath(zone.atmosphericPool);
            }
        }

        [Test]
        public void V15gMetricsOutputFormat_IsParseable()
        {
            string metrics = RimaV15gMinimalComposer.BuildPreviewMetricsText();

            StringAssert.Contains("[v15d Metrics] CombatRoom v15g PixelLab", metrics);
            StringAssert.Contains("v15g minimal PixelLab scene placement metrics", metrics);
            StringAssert.Contains("Budget check: OK", metrics);
            AssertMetricInt(metrics, "cells");
            AssertMetricInt(metrics, "zonePlaced");
            AssertMetricInt(metrics, "adjacencyPlaced");
            AssertMetricInt(metrics, "L8");
            CollectionAssert.IsEmpty(AutoPopulator.LastCompositionReport.BudgetFailures());
        }

        private static T Load<T>(string path) where T : UnityEngine.Object
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            Assert.NotNull(asset, path);
            return asset;
        }

        private static void AssertPool(string path, int expectedCount)
        {
            BlueprintPropPoolSO pool = Load<BlueprintPropPoolSO>(path);
            Assert.NotNull(pool.entries);
            Assert.AreEqual(expectedCount, pool.entries.Length, path);
            AssertPoolEntries(pool);
        }

        private static void AssertPool(string path, IResolveConstraint countConstraint)
        {
            BlueprintPropPoolSO pool = Load<BlueprintPropPoolSO>(path);
            Assert.NotNull(pool.entries);
            Assert.That(pool.entries.Length, countConstraint, path);
            AssertPoolEntries(pool);
        }

        private static void AssertPoolEntries(BlueprintPropPoolSO pool)
        {
            for (int i = 0; i < pool.entries.Length; i++)
            {
                WeightedProp entry = pool.entries[i];
                Assert.NotNull(entry);
                Assert.NotNull(entry.prop);
                Assert.NotNull(entry.prop.visual);

                string spritePath = AssetDatabase.GetAssetPath(entry.prop.visual);
                StringAssert.StartsWith(RimaV15gMinimalComposer.AssetPartsPath + "/", spritePath);

                TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
                Assert.NotNull(importer, spritePath);
                Assert.AreEqual(TextureImporterType.Sprite, importer.textureType);
                Assert.AreEqual(SpriteImportMode.Single, importer.spriteImportMode);
                Assert.AreEqual(32f, importer.spritePixelsPerUnit);
                Assert.AreEqual(FilterMode.Point, importer.filterMode);
                Assert.AreEqual(TextureImporterCompression.Uncompressed, importer.textureCompression);
            }
        }

        private static void AssertPoolPath(BlueprintPropPoolSO pool)
        {
            Assert.NotNull(pool);
            string path = AssetDatabase.GetAssetPath(pool);
            StringAssert.Contains("pool_v15g_pixellab_", path);
            StringAssert.DoesNotContain("v15d", path);
        }

        private static void AssertMetricInt(string metrics, string key)
        {
            string prefix = key + "=";
            string[] lines = metrics.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(prefix, StringComparison.Ordinal))
                {
                    Assert.IsTrue(int.TryParse(lines[i].Substring(prefix.Length), out _), lines[i]);
                    return;
                }
            }

            Assert.Fail("Missing metric line: " + prefix);
        }
    }
}
