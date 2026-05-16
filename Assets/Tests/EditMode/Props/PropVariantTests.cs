#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropVariantTests
    {
        [Test]
        public void PickVariant_EmptyPool_ReturnsWorldSprite()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.worldSprite = null;
            prop.variantSprites = null;
            try
            {
                Sprite picked = prop.PickVariant(42);
                Assert.AreEqual(prop.worldSprite, picked);
            }
            finally
            {
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void PickVariantIndex_EmptyPool_ReturnsMinusOne()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            try
            {
                int idx = prop.PickVariantIndexForTile(new Vector2Int(5, 5));
                Assert.AreEqual(-1, idx);
            }
            finally
            {
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void PickVariantIndex_DeterministicSeed_SameIndexAcrossCalls()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.variantSprites = new Sprite[4];
            try
            {
                int idx1 = prop.PickVariantIndexForTile(new Vector2Int(3, 7));
                int idx2 = prop.PickVariantIndexForTile(new Vector2Int(3, 7));
                Assert.AreEqual(idx1, idx2);
                Assert.GreaterOrEqual(idx1, 0);
                Assert.Less(idx1, 4);
            }
            finally
            {
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void PickVariantIndex_DifferentTiles_DistributeAcrossPool()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.variantSprites = new Sprite[4];
            try
            {
                bool[] seen = new bool[4];
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int idx = prop.PickVariantIndexForTile(new Vector2Int(x, y));
                        Assert.GreaterOrEqual(idx, 0);
                        Assert.Less(idx, 4);
                        seen[idx] = true;
                    }
                }
                int distinctSeen = 0;
                for (int i = 0; i < 4; i++) if (seen[i]) distinctSeen++;
                Assert.GreaterOrEqual(distinctSeen, 2, "Variant distribution should touch multiple slots across 64 tile samples.");
            }
            finally
            {
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void StableTileSeed_KnownInputs_ReturnsExpectedSeed()
        {
            int seed00 = PropDefinitionSO.StableTileSeed(new Vector2Int(0, 0));
            Assert.AreEqual(0, seed00);

            int seed10 = PropDefinitionSO.StableTileSeed(new Vector2Int(1, 0));
            Assert.AreEqual(73856093, seed10);

            int seed01 = PropDefinitionSO.StableTileSeed(new Vector2Int(0, 1));
            Assert.AreEqual(19349663, seed01);

            int seed11 = PropDefinitionSO.StableTileSeed(new Vector2Int(1, 1));
            Assert.AreEqual(73856093 ^ 19349663, seed11);
        }
    }
}
#endif
