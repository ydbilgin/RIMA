using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Editor;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class AssetPackBrowserTests
    {
        private readonly List<Object> createdObjects = new List<Object>();

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
        }

        [Test]
        public void OpenWindow_Succeeds_Without_Errors()
        {
            AssetPackBrowserWindow.Open();
            var window = EditorWindow.GetWindow<AssetPackBrowserWindow>();

            Assert.IsNotNull(window);
            Assert.AreEqual(new Vector2(1100f, 620f), window.minSize);

            window.Close();
        }

        [Test]
        public void Manifest_Enumerates_All_Atlases()
        {
            AssetPackManifestSO manifest = CreateManifest(
                "Test Pack",
                Category("Walls", "WallsA", "WallsB"),
                CreateAtlas("WallsA", "wall_a"),
                CreateAtlas("WallsB", "wall_b"));
            var catalog = new AssetPackCatalog(new[] { manifest });

            Assert.AreEqual(1, catalog.Packs.Count);
            Assert.AreEqual(2, catalog.Query(manifest, "Walls", string.Empty).Count);
        }

        [Test]
        public void CategorySelect_Populates_SpriteGrid()
        {
            AssetPackManifestSO manifest = CreateManifest(
                "Test Pack",
                Category("Walls", "Walls"),
                CreateAtlas("Walls", "wall_a", "wall_b"));
            var catalog = new AssetPackCatalog(new[] { manifest });
            var window = ScriptableObject.CreateInstance<AssetPackBrowserWindow>();
            createdObjects.Add(window);

            window.SetCatalogForTests(catalog);
            window.SelectPackForTests(manifest);
            window.SelectCategoryForTests("Walls");

            Assert.AreEqual(2, window.VisibleEntries.Count);
        }

        [Test]
        public void Search_Filter_Reduces_Grid_Count()
        {
            AssetPackManifestSO manifest = CreateManifest(
                "Test Pack",
                Category("Walls", "Walls"),
                CreateAtlas("Walls", "wall_stone", "rift_fracture", "moss_patch"));
            var catalog = new AssetPackCatalog(new[] { manifest });

            IReadOnlyList<AssetPackEntry> all = catalog.Query(manifest, "Walls", string.Empty);
            IReadOnlyList<AssetPackEntry> filtered = catalog.Query(manifest, "Walls", "rift");

            Assert.AreEqual(3, all.Count);
            Assert.AreEqual(1, filtered.Count);
            Assert.AreEqual("rift_fracture", filtered[0].displayName);
        }

        [Test]
        public void SpriteSelect_Populates_Inspector()
        {
            AssetPackManifestSO manifest = CreateManifest("Test Pack", Category("Walls", "Walls"), CreateAtlas("Walls", "wall_stone"));
            var catalog = new AssetPackCatalog(new[] { manifest });
            AssetPackEntry entry = catalog.Query(manifest, "Walls", string.Empty)[0];
            var lines = SelectedSpriteInspector.BuildMetadataLines(entry);

            Assert.IsTrue(ContainsLine(lines, "Name: wall_stone"));
            Assert.IsTrue(ContainsLine(lines, "Category: Walls"));
            Assert.IsTrue(ContainsLine(lines, "Blocks Movement: True"));
        }

        [Test]
        public void MetadataReadout_Shows_PPU_And_Category()
        {
            Sprite sprite = CreateSprite("floor_tile", 64, 64, 32f);
            PatchAtlasSO atlas = CreateAtlas("BaseFloor", sprite);
            AssetPackManifestSO manifest = CreateManifest("Test Pack", Category("BaseFloor", "BaseFloor"), atlas);
            var catalog = new AssetPackCatalog(new[] { manifest });
            AssetPackEntry entry = catalog.Query(manifest, "BaseFloor", string.Empty)[0];
            var lines = SelectedSpriteInspector.BuildMetadataLines(entry);

            Assert.IsTrue(ContainsLine(lines, "PPU: 32"));
            Assert.IsTrue(ContainsLine(lines, "Category: BaseFloor"));
            Assert.IsTrue(ContainsLine(lines, "Pixel Size: 64 x 64"));
        }

        [Test]
        public void ThumbnailSize_Slider_Clamps_48_to_96()
        {
            Assert.AreEqual(48f, AssetPackBrowserWindow.ClampThumbnailSize(12f));
            Assert.AreEqual(64f, AssetPackBrowserWindow.ClampThumbnailSize(64f));
            Assert.AreEqual(96f, AssetPackBrowserWindow.ClampThumbnailSize(140f));
        }

        [Test]
        public void PackSwitch_Resets_Selection_Without_Errors()
        {
            AssetPackManifestSO packA = CreateManifest("Pack A", Category("Walls", "Walls"), CreateAtlas("Walls", "wall_stone"));
            AssetPackManifestSO packB = CreateManifest("Pack B", Category("BaseFloor", "BaseFloor"), CreateAtlas("BaseFloor", "floor_tile"));
            var catalog = new AssetPackCatalog(new[] { packA, packB });
            var window = ScriptableObject.CreateInstance<AssetPackBrowserWindow>();
            createdObjects.Add(window);

            window.SetCatalogForTests(catalog);
            window.SelectPackForTests(packA);
            AssetPackEntry entry = window.VisibleEntries[0];
            window.SelectEntryForTests(entry);
            Assert.IsNotNull(window.SelectedEntry);

            window.SelectPackForTests(packB);

            Assert.IsNull(window.SelectedEntry);
            Assert.AreEqual(packB, window.ActivePack);
            Assert.AreEqual(1, window.VisibleEntries.Count);
        }

        private AssetPackManifestSO CreateManifest(string displayName, AssetPackCategory category, params PatchAtlasSO[] atlases)
        {
            AssetPackManifestSO manifest = ScriptableObject.CreateInstance<AssetPackManifestSO>();
            manifest.name = displayName;
            manifest.packId = displayName.Replace(" ", "_").ToLowerInvariant();
            manifest.displayName = displayName;
            manifest.categories = new List<AssetPackCategory> { category };
            manifest.atlases = new List<PatchAtlasSO>(atlases);
            manifest.props = new List<PropDefinitionSO>();
            createdObjects.Add(manifest);
            return manifest;
        }

        private AssetPackCategory Category(string name, params string[] atlasNames)
        {
            return new AssetPackCategory
            {
                categoryName = name,
                atlasNames = new List<string>(atlasNames),
                categoryIcon = null
            };
        }

        private PatchAtlasSO CreateAtlas(string atlasName, params string[] spriteNames)
        {
            Sprite[] sprites = new Sprite[spriteNames.Length];
            for (int i = 0; i < spriteNames.Length; i++)
            {
                sprites[i] = CreateSprite(spriteNames[i], 32, 32, 32f);
            }

            return CreateAtlas(atlasName, sprites);
        }

        private PatchAtlasSO CreateAtlas(string atlasName, params Sprite[] sprites)
        {
            PatchAtlasSO atlas = ScriptableObject.CreateInstance<PatchAtlasSO>();
            atlas.name = atlasName;
            atlas.atlasId = atlasName;
            atlas.role = atlasName.Contains("Floor") ? PatchRole.BaseFloor : PatchRole.MacroPatch;
            atlas.variants = sprites;
            createdObjects.Add(atlas);
            return atlas;
        }

        private Sprite CreateSprite(string spriteName, int width, int height, float pixelsPerUnit)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.name = spriteName + "_Texture";
            createdObjects.Add(texture);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            sprite.name = spriteName;
            createdObjects.Add(sprite);
            return sprite;
        }

        private static bool ContainsLine(IReadOnlyList<string> lines, string expected)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
