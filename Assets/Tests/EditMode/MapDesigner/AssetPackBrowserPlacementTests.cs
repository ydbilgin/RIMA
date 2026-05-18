using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Editor;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class AssetPackBrowserPlacementTests
    {
        private readonly List<Object> createdObjects = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            Selection.activeObject = null;
            Undo.ClearAll();

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
        public void PlacementMode_GhostFollowsCursor_PPUSnapped()
        {
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone");
            Transform target = CreateTarget();
            var window = CreateWindow(entry);
            window.SetActiveRoomRootForTests(target);

            window.SelectEntryForTests(entry);
            window.UpdateGhostPositionForTests(new Vector3(0.049f, 0.065f, 0f));

            Assert.IsTrue(window.IsPlacementMode);
            Assert.IsNotNull(window.GhostPreview);
            Assert.AreEqual(AssetPackBrowserWindow.SnapWorldPosition(new Vector3(0.049f, 0.065f, 0f)), window.GhostPreview.transform.position);
        }

        [Test]
        public void LeftClick_CreatesGameObject_UnderActiveTarget()
        {
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone");
            Transform target = CreateTarget();
            var window = CreateWindow(entry);

            GameObject placed = window.PlaceEntryForTests(entry, target, new Vector3(0.12f, 0.22f, 0f));
            createdObjects.Add(placed);

            Assert.IsNotNull(placed);
            Assert.AreEqual(target, placed.transform.parent);
            Assert.AreEqual(entry.sprite, placed.GetComponent<SpriteRenderer>().sprite);
        }

        [Test]
        public void LeftClick_AppliesCorrectSortingOrder_PerCategory()
        {
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone");
            Transform target = CreateTarget();
            var window = CreateWindow(entry);

            GameObject placed = window.PlaceEntryForTests(entry, target, Vector3.zero);
            createdObjects.Add(placed);

            Assert.AreEqual(20, placed.GetComponent<SpriteRenderer>().sortingOrder);
        }

        [Test]
        public void LeftClick_AppliesAutoCollider_WhenBlocksMovement()
        {
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone");
            Transform target = CreateTarget();
            var window = CreateWindow(entry);

            GameObject placed = window.PlaceEntryForTests(entry, target, Vector3.zero);
            createdObjects.Add(placed);

            BoxCollider2D collider = placed.GetComponent<BoxCollider2D>();
            Assert.IsNotNull(collider);
            Assert.AreEqual(new Vector2(1f, 1f), collider.size);
            Assert.IsFalse(collider.isTrigger);
        }

        [Test]
        public void RightClick_ExitsPlacementMode_DestroysGhost()
        {
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone");
            var window = CreateWindow(entry);

            window.SelectEntryForTests(entry);
            Assert.IsNotNull(window.GhostPreview);

            window.DisablePlacementForTests();

            Assert.IsFalse(window.IsPlacementMode);
            Assert.IsNull(window.GhostPreview);
        }

        [Test]
        public void InspectorScaleSlider_AppliesToSelected()
        {
            GameObject placed = CreatePlacedSprite("placed", CreateSprite("sprite", 32, 32, 32f));

            SelectedSpriteInspector.ApplyScale(placed, 1.45f);

            Assert.AreEqual(new Vector3(1.45f, 1.45f, 1.45f), placed.transform.localScale);
        }

        [Test]
        public void InspectorVariantSlider_CyclesAtlasVariants()
        {
            Sprite first = CreateSprite("variant_a", 32, 32, 32f);
            Sprite second = CreateSprite("variant_b", 32, 32, 32f);
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone", first, second);
            GameObject placed = CreatePlacedSprite("placed", first);

            SelectedSpriteInspector.ApplyVariantIndex(placed, entry, 3);

            Assert.AreEqual(second, placed.GetComponent<SpriteRenderer>().sprite);
        }

        [Test]
        public void Undo_RemovesLastPlacement()
        {
            AssetPackEntry entry = CreateEntry("Walls", "wall_stone");
            Transform target = CreateTarget();
            var window = CreateWindow(entry);

            GameObject placed = window.PlaceEntryForTests(entry, target, Vector3.zero);

            Assert.IsNotNull(placed);
            Undo.PerformUndo();

            Assert.IsTrue(placed == null);
        }

        [Test]
        public void PropWithoutColliderConfig_DoesNotAttachCollider()
        {
            AssetPackEntry entry = CreateEntry("Accent", "rift_glow");
            Transform target = CreateTarget();
            var window = CreateWindow(entry);

            GameObject placed = window.PlaceEntryForTests(entry, target, Vector3.zero);
            createdObjects.Add(placed);

            Assert.IsNull(placed.GetComponent<Collider2D>());
        }

        [Test]
        public void ColliderFootprint_Respects_Ratio()
        {
            Sprite sprite = CreateSprite("wide_wall", 64, 32, 32f);
            GameObject placed = CreatePlacedSprite("placed", sprite);
            var preset = new CollisionPreset
            {
                blocksMovement = true,
                colliderShape = ColliderShape.Box,
                colliderFootprintRatio = 0.5f,
                colliderOffset = Vector2.zero,
                isTrigger = false,
                colliderLayer = string.Empty
            };

            BoxCollider2D collider = (BoxCollider2D)AssetPackBrowserWindow.AttachAutoCollider(placed, sprite, preset);

            Assert.AreEqual(new Vector2(1f, 0.5f), collider.size);
        }

        private AssetPackBrowserWindow CreateWindow(AssetPackEntry entry)
        {
            var window = ScriptableObject.CreateInstance<AssetPackBrowserWindow>();
            createdObjects.Add(window);
            return window;
        }

        private AssetPackEntry CreateEntry(string categoryName, string atlasName, params Sprite[] sprites)
        {
            if (sprites == null || sprites.Length == 0)
            {
                sprites = new[] { CreateSprite(atlasName, 32, 32, 32f) };
            }

            PatchAtlasSO atlas = CreateAtlas(atlasName, sprites);
            AssetPackManifestSO manifest = CreateManifest("Placement Pack", Category(categoryName, atlasName), atlas);
            var catalog = new AssetPackCatalog(new[] { manifest });
            AssetPackEntry entry = catalog.Query(manifest, categoryName, string.Empty)[0];
            return entry;
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

        private Transform CreateTarget()
        {
            GameObject target = new GameObject("PlacementTarget");
            createdObjects.Add(target);
            return target.transform;
        }

        private GameObject CreatePlacedSprite(string name, Sprite sprite)
        {
            GameObject placed = new GameObject(name);
            SpriteRenderer renderer = placed.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            createdObjects.Add(placed);
            return placed;
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
    }
}
