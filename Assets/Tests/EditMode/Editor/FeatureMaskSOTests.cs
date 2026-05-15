using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using UnityEngine;

namespace RIMA.Tests.Editor
{
    public sealed class FeatureMaskSOTests
    {
        [Test]
        public void Sample_NullTexture_ReturnsNeutral()
        {
            FeatureMaskSO mask = ScriptableObject.CreateInstance<FeatureMaskSO>();
            try
            {
                Assert.AreEqual(1f, mask.Sample(new Vector2Int(1, 1), new Vector2Int(4, 4)));
            }
            finally
            {
                Object.DestroyImmediate(mask);
            }
        }

        [Test]
        public void Sample_ReadsAlphaAndInvert()
        {
            FeatureMaskSO mask = ScriptableObject.CreateInstance<FeatureMaskSO>();
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                texture.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.25f));
                texture.SetPixel(1, 0, new Color(1f, 1f, 1f, 0.25f));
                texture.SetPixel(0, 1, new Color(1f, 1f, 1f, 0.25f));
                texture.SetPixel(1, 1, new Color(1f, 1f, 1f, 0.25f));
                texture.Apply();
                mask.alphaMask = texture;
                mask.remap = AnimationCurve.Linear(0f, 0f, 1f, 1f);

                Assert.AreEqual(0.25f, mask.Sample(new Vector2Int(0, 0), new Vector2Int(1, 1)), 0.02f);

                mask.invert = true;
                Assert.AreEqual(0.75f, mask.Sample(new Vector2Int(0, 0), new Vector2Int(1, 1)), 0.02f);
            }
            finally
            {
                Object.DestroyImmediate(texture);
                Object.DestroyImmediate(mask);
            }
        }

        [Test]
        public void DetailDensity_UsesNaturalFeatureProximityAndPreservesNullMaskFallback()
        {
            RoomData room = new RoomData
            {
                size = new Vector2Int(20, 20),
                walkable = CreateWalkable(20, 20)
            };
            float fallback = DetailDecalPainter.DensityForCell(new Vector2Int(10, 10), room, 1f);

            room.naturalFeatures = NaturalFeatureGraph.Generate(room.size, room.walkable, 64, 99, FeatureType.Water, 1f);
            Vector2Int featureCell = FindFeatureCell(room.naturalFeatures.featureMask);
            Vector2Int farCell = FindNonFeatureCellFarFrom(room.naturalFeatures.featureMask, featureCell);

            float near = DetailDecalPainter.DensityForCell(featureCell, room, 1f, null, 0f);
            float far = DetailDecalPainter.DensityForCell(farCell, room, 1f, null, 0f);

            Assert.Greater(fallback, 0f);
            Assert.Greater(near, far);
        }

        private static bool[,] CreateWalkable(int width, int height)
        {
            bool[,] walkable = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    walkable[x, y] = true;
                }
            }

            return walkable;
        }

        private static Vector2Int FindFeatureCell(bool[,] mask)
        {
            for (int y = 0; y < mask.GetLength(1); y++)
            {
                for (int x = 0; x < mask.GetLength(0); x++)
                {
                    if (mask[x, y])
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return Vector2Int.zero;
        }

        private static Vector2Int FindNonFeatureCellFarFrom(bool[,] mask, Vector2Int featureCell)
        {
            Vector2Int best = Vector2Int.zero;
            int bestDist = -1;
            for (int y = 0; y < mask.GetLength(1); y++)
            {
                for (int x = 0; x < mask.GetLength(0); x++)
                {
                    if (mask[x, y])
                    {
                        continue;
                    }

                    int dist = Mathf.Abs(x - featureCell.x) + Mathf.Abs(y - featureCell.y);
                    if (dist > bestDist)
                    {
                        bestDist = dist;
                        best = new Vector2Int(x, y);
                    }
                }
            }

            return best;
        }
    }
}
