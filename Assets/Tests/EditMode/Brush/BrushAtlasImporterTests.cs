#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Import.Editor;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Tests
{
    public class BrushAtlasImporterTests
    {
        private const string TempFolder = "Assets/TempTests/Brush";
        private const string PoolFolder = "Assets/Art/BrushAtlas/Pools";
        private string testPngPath;
        private string testPoolPath;

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }
            if (!Directory.Exists(PoolFolder))
            {
                Directory.CreateDirectory(PoolFolder);
            }
            AssetDatabase.Refresh();

            testPngPath = $"{TempFolder}/sprint9_test_atlas.png";
            testPoolPath = $"{PoolFolder}/sprint9_test_atlas.asset";

            var tex = new Texture2D(512, 512, TextureFormat.RGBA32, false);
            var pixels = new Color32[512 * 512];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color32(128, 128, 128, 255);
            }
            tex.SetPixels32(pixels);
            tex.Apply();
            File.WriteAllBytes(testPngPath, tex.EncodeToPNG());
            AssetDatabase.ImportAsset(testPngPath);
        }

        [TearDown]
        public void Teardown()
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(testPoolPath) != null)
            {
                AssetDatabase.DeleteAsset(testPoolPath);
            }
            if (AssetDatabase.IsValidFolder(TempFolder))
            {
                AssetDatabase.DeleteAsset(TempFolder);
            }
            if (AssetDatabase.IsValidFolder("Assets/TempTests"))
            {
                AssetDatabase.DeleteAsset("Assets/TempTests");
            }
        }

        [Test]
        public void Import_ProducesVariants_NoRuntimeScale()
        {
            var template = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            template.templateName = "TestL4";
            template.masterSize = new Vector2Int(512, 512);
            template.defaultPivot = new Vector2(0.5f, 0.5f);
            template.cells = new List<SliceCell>
            {
                new SliceCell { cellName = "hero", rect = new RectInt(0, 0, 256, 256), bucket = SizeBucket.Hero, heroAllowed = true, tags = new[] { "hero" } },
                new SliceCell { cellName = "medium_1", rect = new RectInt(256, 0, 128, 128), bucket = SizeBucket.Medium, tags = new[] { "medium" } },
                new SliceCell { cellName = "small_1", rect = new RectInt(0, 256, 64, 64), bucket = SizeBucket.Small, tags = new[] { "small" } }
            };

            var result = BrushAtlasImporter.Import(testPngPath, template, "sprint9_test_atlas", TargetLayer.L4);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.variantCount, Is.EqualTo(3));
            Assert.That(result.pool, Is.Not.Null);
            Assert.That(result.pool.variants.Count, Is.EqualTo(3));

            foreach (var v in result.pool.variants)
            {
                Assert.That(v.nativeSize.x, Is.GreaterThan(0));
                Assert.That(v.nativeSize.y, Is.GreaterThan(0));
                Assert.That(v.schemaVersion, Is.EqualTo("1.0"));
            }
        }

        [Test]
        public void Import_PreservesAssetGuid_OnReimport()
        {
            var template = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            template.templateName = "TestGuid";
            template.masterSize = new Vector2Int(512, 512);
            template.defaultPivot = new Vector2(0.5f, 0.5f);
            template.cells = new List<SliceCell>
            {
                new SliceCell { cellName = "only", rect = new RectInt(0, 0, 64, 64), bucket = SizeBucket.Small, tags = new[] { "only" } }
            };

            BrushAtlasImporter.Import(testPngPath, template, "sprint9_test_atlas", TargetLayer.L4);
            string guidBefore = AssetDatabase.AssetPathToGUID(testPoolPath);

            BrushAtlasImporter.Import(testPngPath, template, "sprint9_test_atlas", TargetLayer.L4);
            string guidAfter = AssetDatabase.AssetPathToGUID(testPoolPath);

            Assert.That(guidAfter, Is.EqualTo(guidBefore), "Asset GUID should be preserved on reimport (in-place update)");
        }

        [Test]
        public void Import_WangAware_ParsesTilesetDataNested()
        {
            // Real PixelLab tileset15 format has nested `tileset_data.tiles`, not top-level `tiles`.
            string wangPngPath = $"{TempFolder}/sprint9_wang_atlas.png";
            string wangMetaPath = $"{TempFolder}/metadata.json";

            var wangTex = new Texture2D(128, 256, TextureFormat.RGBA32, false);
            var pixels = new Color32[128 * 256];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(64, 64, 64, 255);
            wangTex.SetPixels32(pixels);
            wangTex.Apply();
            File.WriteAllBytes(wangPngPath, wangTex.EncodeToPNG());

            string wangJson = "{\"id\":\"test\",\"tileset_data\":{\"tiles\":[" +
                "{\"id\":\"t0\",\"corners\":{\"NE\":\"lower\",\"NW\":\"lower\",\"SE\":\"lower\",\"SW\":\"lower\"},\"bounding_box\":{\"x\":0,\"y\":0,\"width\":32,\"height\":32}}," +
                "{\"id\":\"t1\",\"corners\":{\"NE\":\"upper\",\"NW\":\"lower\",\"SE\":\"lower\",\"SW\":\"upper\"},\"bounding_box\":{\"x\":32,\"y\":0,\"width\":32,\"height\":32}}," +
                "{\"id\":\"t2\",\"corners\":{\"NE\":\"upper\",\"NW\":\"upper\",\"SE\":\"upper\",\"SW\":\"upper\"},\"bounding_box\":{\"x\":64,\"y\":0,\"width\":32,\"height\":32}}" +
                "]}}";
            File.WriteAllText(wangMetaPath, wangJson);
            AssetDatabase.ImportAsset(wangPngPath);

            var template = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            template.templateName = "TestWang";
            template.masterSize = Vector2Int.zero;
            template.defaultPivot = new Vector2(0.5f, 0.5f);
            template.wangAware = true;

            var result = BrushAtlasImporter.Import(wangPngPath, template, "sprint9_wang_atlas", TargetLayer.L3);
            Assert.That(result, Is.Not.Null);
            // 3 tiles in JSON; 0000 (t0) is all_floor_reference and excluded from pool
            Assert.That(result.variantCount, Is.EqualTo(2), "Expected 2 Wang variants (3 tiles minus all_floor_reference)");

            bool foundUpper = false;
            foreach (var v in result.pool.variants)
            {
                if (v.variantId.Contains("wang_1001"))
                {
                    foundUpper = true;
                    break;
                }
            }
            Assert.That(foundUpper, Is.True, "Expected wang_1001 variant from corner NE=upper, NW=lower, SE=lower, SW=upper");

            // Cleanup wang pool
            string wangPoolPath = $"{PoolFolder}/sprint9_wang_atlas.asset";
            if (AssetDatabase.LoadAssetAtPath<Object>(wangPoolPath) != null)
            {
                AssetDatabase.DeleteAsset(wangPoolPath);
            }
        }

        [Test]
        public void Import_WrongPath_ReturnsError()
        {
            var template = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            template.masterSize = new Vector2Int(128, 128);
            template.cells = new List<SliceCell>();
            var result = BrushAtlasImporter.Import("Assets/DoesNotExist/nothing.png", template, "fail_pool", TargetLayer.L4);

            Assert.That(result.Success, Is.False);
            bool foundError = false;
            foreach (var i in result.issues)
            {
                if (i.code == "VAL_PNG_UNREADABLE" && i.severity == ValidationIssueSeverity.Error)
                {
                    foundError = true;
                    break;
                }
            }
            Assert.That(foundError, Is.True);
        }
    }
}
#endif
