#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Import.Editor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Tests
{
    public class BrushAtlasValidatorTests
    {
        [Test]
        public void DetectsDuplicateVariantId()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.variants = new List<BrushAssetVariant>
            {
                new BrushAssetVariant { variantId = "dup", weight = 1f, bucket = SizeBucket.Micro },
                new BrushAssetVariant { variantId = "dup", weight = 1f, bucket = SizeBucket.Micro }
            };
            var tex = new Texture2D(16, 16);
            var issues = BrushAtlasValidator.Validate(pool, tex, null);

            bool found = false;
            foreach (var i in issues)
            {
                if (i.code == "VAL_DUPLICATE_VARIANT_ID" && i.severity == ValidationIssueSeverity.Error)
                {
                    found = true;
                    break;
                }
            }
            Assert.That(found, Is.True, "Expected VAL_DUPLICATE_VARIANT_ID Error issue");
        }

        [Test]
        public void DetectsZeroWeight()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.variants = new List<BrushAssetVariant>
            {
                new BrushAssetVariant { variantId = "zero", weight = 0f, bucket = SizeBucket.Small }
            };
            var tex = new Texture2D(16, 16);
            var issues = BrushAtlasValidator.Validate(pool, tex, null);

            bool found = false;
            foreach (var i in issues)
            {
                if (i.code == "VAL_ZERO_WEIGHT" && i.severity == ValidationIssueSeverity.Warning)
                {
                    found = true;
                    break;
                }
            }
            Assert.That(found, Is.True, "Expected VAL_ZERO_WEIGHT Warning issue");
        }

        [Test]
        public void EmitsInfoCounts()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.variants = new List<BrushAssetVariant>
            {
                new BrushAssetVariant { variantId = "a", weight = 1f, bucket = SizeBucket.Micro },
                new BrushAssetVariant { variantId = "b", weight = 1f, bucket = SizeBucket.Hero, heroAllowed = true }
            };
            var tex = new Texture2D(16, 16);
            var issues = BrushAtlasValidator.Validate(pool, tex, null);

            bool foundCount = false;
            bool foundHero = false;
            foreach (var i in issues)
            {
                if (i.code == "INF_VARIANT_COUNT") foundCount = true;
                if (i.code == "INF_HERO_RATIO") foundHero = true;
            }
            Assert.That(foundCount, Is.True, "Expected INF_VARIANT_COUNT info");
            Assert.That(foundHero, Is.True, "Expected INF_HERO_RATIO info");
        }

        [Test]
        public void NullPool_ReturnsError()
        {
            var issues = BrushAtlasValidator.Validate(null, new Texture2D(16, 16), null);
            bool found = false;
            foreach (var i in issues)
            {
                if (i.code == "VAL_POOL_NULL" && i.severity == ValidationIssueSeverity.Error)
                {
                    found = true;
                    break;
                }
            }
            Assert.That(found, Is.True);
        }

        [Test]
        public void MasterSizeMismatch_ReturnsError()
        {
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.variants = new List<BrushAssetVariant>();
            var tex = new Texture2D(64, 64);
            var template = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            template.masterSize = new Vector2Int(128, 128);
            var issues = BrushAtlasValidator.Validate(pool, tex, template);

            bool found = false;
            foreach (var i in issues)
            {
                if (i.code == "VAL_MASTER_SIZE_MISMATCH" && i.severity == ValidationIssueSeverity.Error)
                {
                    found = true;
                    break;
                }
            }
            Assert.That(found, Is.True);
        }
    }
}
#endif
