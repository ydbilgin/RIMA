namespace RIMA.Tests.Editor
{
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using RIMA.Editor.TileImport;
    using UnityEditor;
    using UnityEngine;

    public sealed class WangImporterTests
    {
        private const string SourceFolder = "STAGING/TILESET_OUTPUT/F1_Wang_Rubble_Path_Existing";
        private const string SourcePng = SourceFolder + "/wang_rubble_path.png";
        private const string SourceMetadata = SourceFolder + "/metadata.json";
        private const string TempRoot = "Assets/Temp/WangImporterTests";
        private const string TempPng = TempRoot + "/wang_rubble_path.png";
        private const string TempMetadata = TempRoot + "/metadata.json";
        private const string TempGenerated = TempRoot + "/Generated";

        [SetUp]
        public void SetUp()
        {
            AssetDatabase.DeleteAsset(TempRoot);
            Directory.CreateDirectory(TempRoot);
            File.Copy(SourcePng, TempPng, true);
            File.Copy(SourceMetadata, TempMetadata, true);
            AssetDatabase.ImportAsset(TempPng, ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset(TempMetadata, ImportAssetOptions.ForceUpdate);
        }

        [TearDown]
        public void TearDown()
        {
            AssetDatabase.DeleteAsset(TempRoot);
            AssetDatabase.Refresh();
        }

        [Test]
        public void WangImporter_ParsesMetadata_Returns16Tiles()
        {
            PixelLabWangImporter.WangMetadataDocument metadata = PixelLabWangImporter.ParseMetadata(SourceMetadata);

            Assert.AreEqual(16, metadata.tileset_data.tiles.Length);
            foreach (PixelLabWangImporter.WangTileMetadata tile in metadata.tileset_data.tiles)
            {
                Assert.IsNotNull(tile.corners);
                Assert.IsFalse(string.IsNullOrEmpty(tile.corners.NW));
                Assert.IsFalse(string.IsNullOrEmpty(tile.corners.NE));
                Assert.IsFalse(string.IsNullOrEmpty(tile.corners.SW));
                Assert.IsFalse(string.IsNullOrEmpty(tile.corners.SE));
                Assert.AreEqual(32, tile.bounding_box.width);
                Assert.AreEqual(32, tile.bounding_box.height);
            }
        }

        [Test]
        public void WangImporter_CreatesRuleTileAsset_FromSampleSheet()
        {
            string assetPath = PixelLabWangImporter.ImportWangSheetAsset(TempPng, TempGenerated);
            ScriptableObject ruleTile = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            Assert.IsTrue(File.Exists(assetPath));
            Assert.IsNotNull(ruleTile);
            var serialized = new SerializedObject(ruleTile);
            Assert.Greater(serialized.FindProperty("m_TilingRules").arraySize, 0);
        }

        [Test]
        public void WangImporter_AllLowerTile_DefaultSprite()
        {
            string assetPath = PixelLabWangImporter.ImportWangSheetAsset(TempPng, TempGenerated);
            ScriptableObject ruleTile = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            PixelLabWangImporter.WangMetadataDocument metadata = PixelLabWangImporter.ParseMetadata(TempMetadata);
            PixelLabWangImporter.WangTileMetadata allLower = metadata.tileset_data.tiles.First(tile =>
                tile.corners.NW == "lower" &&
                tile.corners.NE == "lower" &&
                tile.corners.SW == "lower" &&
                tile.corners.SE == "lower");

            int expectedIndex = (allLower.bounding_box.y / 32) * 4 + (allLower.bounding_box.x / 32);
            Sprite expectedSprite = AssetDatabase.LoadAllAssetRepresentationsAtPath(TempPng)
                .OfType<Sprite>()
                .OrderByDescending(sprite => sprite.rect.y)
                .ThenBy(sprite => sprite.rect.x)
                .ElementAt(expectedIndex);

            var serialized = new SerializedObject(ruleTile);
            Object defaultSprite = serialized.FindProperty("m_DefaultSprite").objectReferenceValue;
            Assert.IsNotNull(defaultSprite);
            Assert.AreSame(expectedSprite, defaultSprite);
        }
    }
}
