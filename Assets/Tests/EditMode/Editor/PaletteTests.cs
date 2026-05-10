namespace RIMA.Tests.Editor
{
    using NUnit.Framework;
    using RIMA.Editor.RoomDesigner.Palette;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class PaletteTests
    {
        [TestCase("Assets/Art/Tiles/Act1/Keep/floor.asset", BiomeFilter.Keep)]
        [TestCase("Assets/Art/Tiles/Act1/Crypt/floor.asset", BiomeFilter.Crypt)]
        [TestCase("Assets/Art/Tiles/Act1/Volcanic/floor.asset", BiomeFilter.Volcanic)]
        [TestCase("Assets/Art/Tiles/Act1/F1/floor.asset", BiomeFilter.Unknown)]
        [TestCase("", BiomeFilter.Unknown)]
        public void BiomeOfUsesPathSegments(string path, string expected)
        {
            Assert.AreEqual(expected, BiomeFilter.BiomeOf(path));
        }

        [Test]
        public void AssetPreviewCacheGetAndInvalidateAreSafe()
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            var cache = new AssetPreviewCache(() => { });

            try
            {
                Assert.DoesNotThrow(() => cache.Get(tile));
                Assert.DoesNotThrow(() => cache.Invalidate());
                Assert.DoesNotThrow(() => cache.Get(tile));
            }
            finally
            {
                cache.Dispose();
                Object.DestroyImmediate(tile);
            }
        }
    }
}
