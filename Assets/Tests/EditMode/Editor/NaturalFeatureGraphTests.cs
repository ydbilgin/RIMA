using System.Diagnostics;
using NUnit.Framework;
using RIMA.MapDesigner;
using UnityEngine;

namespace RIMA.Tests.Editor
{
    public sealed class NaturalFeatureGraphTests
    {
        [Test]
        public void GenerateSites_SiteCount64_ProducesGridCount()
        {
            var sites = NaturalFeatureGraph.GenerateSites(new Vector2Int(32, 32), 64, 123);

            Assert.GreaterOrEqual(sites.Count, 64);
            Assert.LessOrEqual(sites.Count, 65);
        }

        [Test]
        public void Generate_IsDeterministicForSameSeed()
        {
            bool[,] walkable = CreateWalkable(24, 18, true);

            NaturalFeatureGraphResult a = NaturalFeatureGraph.Generate(new Vector2Int(24, 18), walkable, 64, 78101, FeatureType.Water);
            NaturalFeatureGraphResult b = NaturalFeatureGraph.Generate(new Vector2Int(24, 18), walkable, 64, 78101, FeatureType.Water);

            Assert.AreEqual(a.sites.Length, b.sites.Length);
            for (int i = 0; i < a.sites.Length; i++)
            {
                Assert.AreEqual(a.sites[i], b.sites[i]);
                Assert.AreEqual(a.siteTypes[i], b.siteTypes[i]);
            }

            for (int y = 0; y < 18; y++)
            {
                for (int x = 0; x < 24; x++)
                {
                    Assert.AreEqual(a.siteIndex[x, y], b.siteIndex[x, y]);
                    Assert.AreEqual(a.featureMask[x, y], b.featureMask[x, y]);
                }
            }
        }

        [Test]
        public void Generate_FeatureMaskSkipsNonWalkableCells()
        {
            bool[,] walkable = CreateWalkable(12, 12, true);
            walkable[4, 4] = false;
            walkable[5, 4] = false;

            NaturalFeatureGraphResult result = NaturalFeatureGraph.Generate(new Vector2Int(12, 12), walkable, 64, 42, FeatureType.Elevation, 1f);

            Assert.IsFalse(result.featureMask[4, 4]);
            Assert.IsFalse(result.featureMask[5, 4]);
        }

        [Test]
        public void Generate_200x200_256Sites_StaysInsideEditorBudget()
        {
            bool[,] walkable = CreateWalkable(200, 200, true);
            Stopwatch sw = Stopwatch.StartNew();

            NaturalFeatureGraphResult result = NaturalFeatureGraph.Generate(new Vector2Int(200, 200), walkable, 256, 99, FeatureType.Water);

            sw.Stop();
            Assert.IsTrue(result.HasData);
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 20, "Voronoi feature generation exceeded budget: " + sw.ElapsedMilliseconds + "ms");
        }

        private static bool[,] CreateWalkable(int width, int height, bool value)
        {
            bool[,] walkable = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    walkable[x, y] = value;
                }
            }

            return walkable;
        }
    }
}
