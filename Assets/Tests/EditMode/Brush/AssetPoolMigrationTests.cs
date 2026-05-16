#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors.Editor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Tests
{
    public class AssetPoolMigrationTests
    {
        [Test]
        public void LegacySprites_StillReadable()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            var tex = new Texture2D(32, 32);
            var sprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
            pool.sprites = new List<Sprite> { sprite };
            pool.variants = new List<BrushAssetVariant>();

            var picked = DecorativeExecutorUtility.PickSprite(pool, 42, 0);
            Assert.That(picked, Is.EqualTo(sprite));
        }

        [Test]
        public void VariantsPath_PreferredOverLegacy()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            var tex = new Texture2D(32, 32);
            var legacySprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
            var variantSprite = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
            pool.sprites = new List<Sprite> { legacySprite };
            pool.variants = new List<BrushAssetVariant>
            {
                new BrushAssetVariant { sprite = variantSprite, variantId = "v1", weight = 1f, bucket = SizeBucket.Micro }
            };

            var picked = DecorativeExecutorUtility.PickSprite(pool, 42, 0);
            Assert.That(picked, Is.EqualTo(variantSprite), "Variants list should take precedence over legacy sprites");
        }

        [Test]
        public void EmptyPool_ReturnsNull()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.sprites = new List<Sprite>();
            pool.variants = new List<BrushAssetVariant>();
            var picked = DecorativeExecutorUtility.PickSprite(pool, 1, 0);
            Assert.That(picked, Is.Null);
        }

        [Test]
        public void PickVariant_BucketAware()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            var tex = new Texture2D(32, 32);
            var microSprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
            var heroSprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
            pool.variants = new List<BrushAssetVariant>
            {
                new BrushAssetVariant { sprite = microSprite, variantId = "m", weight = 1f, bucket = SizeBucket.Micro },
                new BrushAssetVariant { sprite = heroSprite, variantId = "h", weight = 1f, bucket = SizeBucket.Hero, heroAllowed = true }
            };

            var profile = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            profile.PopulateDefaultSoftOverlap();

            var picked = DecorativeExecutorUtility.PickVariant(pool, profile, 1, 100, 0);
            Assert.That(picked, Is.Not.Null);
            Assert.That(picked.bucket, Is.EqualTo(SizeBucket.Micro), "Radius 1 should pick Micro");

            var pickedHero = DecorativeExecutorUtility.PickVariant(pool, profile, 10, 100, 0);
            Assert.That(pickedHero, Is.Not.Null);
            Assert.That(pickedHero.bucket, Is.EqualTo(SizeBucket.Hero), "Radius 10 should pick Hero");
        }
    }
}
#endif
