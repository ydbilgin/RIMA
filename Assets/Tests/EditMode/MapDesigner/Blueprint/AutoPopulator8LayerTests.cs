using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using RIMA.MapDesigner.SO;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class AutoPopulator8LayerTests
    {
        private readonly List<Object> createdObjects = new List<Object>();
        private Transform root;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("TestRoot").transform;
            createdObjects.Add(root.gameObject);
        }

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
            root = null;
        }

        [Test]
        public void PopulateLayer1Macro_FullCoverage()
        {
            BlueprintProfileSO profile = CreateProfile(CreateCompleteZone("grass"));
            BlueprintCanvas canvas = FilledCanvas(5, 4, "grass");

            int placed = AutoPopulator.PopulateLayer1Macro(canvas, profile, root, 100);

            Assert.AreEqual(canvas.Count, placed);
        }

        [Test]
        public void PopulateLayer2BaseFloor_FullCoverage()
        {
            BlueprintProfileSO profile = CreateProfile(CreateCompleteZone("grass"));
            BlueprintCanvas canvas = FilledCanvas(5, 4, "grass");

            int placed = AutoPopulator.PopulateLayer2BaseFloor(canvas, profile, root, 101);

            Assert.AreEqual(canvas.Count, placed);
        }

        [Test]
        public void PopulateLayer3MidTone_RespectsDensity()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("grass");
            zone.midToneDensity = 0.4f;
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(10, 10, "grass");

            int placed = AutoPopulator.PopulateLayer3MidTone(canvas, profile, root, 102);

            Assert.That(placed, Is.InRange(30, 50));
        }

        [Test]
        public void PopulateLayer4Detail_RespectsDensity()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("grass");
            zone.detailDensity = 0.3f;
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(10, 10, "grass");

            int placed = AutoPopulator.PopulateLayer4Detail(canvas, profile, root, 103);

            Assert.That(placed, Is.InRange(20, 40));
        }

        [Test]
        public void PopulateLayer5Scatter_RespectsDensity()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("grass");
            zone.smallScatterDensity = 0.4f;
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(10, 10, "grass");

            int placed = AutoPopulator.PopulateLayer5SmallScatter(canvas, profile, root, 104);

            Assert.That(placed, Is.InRange(30, 50));
        }

        [Test]
        public void PopulateLayer6Medium_RespectsDensity()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("grass");
            zone.mediumPropDensity = 0.15f;
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(10, 10, "grass");

            int placed = AutoPopulator.PopulateLayer6Medium(canvas, profile, root, 105);

            Assert.That(placed, Is.InRange(8, 24));
        }

        [Test]
        public void PopulateLayer7TallFocal_RespectsMaxCap()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("feature");
            zone.maxTallFocalPerRegion = 2;
            BlueprintProfileSO profile = CreateProfile(zone);
            var canvas = new BlueprintCanvas(new Vector2Int(16, 8));
            canvas.Paint(new Vector2Int(2, 2), "feature", 5);
            canvas.Paint(new Vector2Int(11, 2), "feature", 5);

            int placed = AutoPopulator.PopulateLayer7TallFocal(canvas, profile, root, 106);

            Assert.AreEqual(4, placed);
        }

        [Test]
        public void PopulateLayer8Atmospheric_RespectsDensity()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("grass");
            zone.atmosphericDensity = 0.2f;
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(10, 10, "grass");

            int placed = AutoPopulator.PopulateLayer8Atmospheric(canvas, profile, root, 107);

            Assert.That(placed, Is.InRange(12, 30));
        }

        [Test]
        public void Populate8Layers_SortingOrder_AscendingByLayer()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("feature");
            zone.midToneDensity = 1f;
            zone.detailDensity = 1f;
            zone.smallScatterDensity = 1f;
            zone.mediumPropDensity = 1f;
            zone.atmosphericDensity = 1f;
            zone.maxTallFocalPerRegion = 2;
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(3, 3, "feature");

            AutoPopulator.PopulateZones(canvas, profile, root, 108);

            Assert.AreEqual(AutoPopulator.Layer1SortingOrder, RendererFor("_BlueprintPlaced_L1_feature_0_0").sortingOrder);
            Assert.AreEqual(AutoPopulator.Layer2SortingOrder, RendererFor("_BlueprintPlaced_L2_feature_0_0").sortingOrder);
            Assert.AreEqual(AutoPopulator.Layer3SortingOrder, RendererFor("_BlueprintPlaced_L3_feature_0_0").sortingOrder);
            Assert.AreEqual(AutoPopulator.Layer4SortingOrder, RendererFor("_BlueprintPlaced_L4_feature_0_0").sortingOrder);
            Assert.AreEqual(AutoPopulator.Layer5SortingOrder, RendererFor("_BlueprintPlaced_L5_feature_0_0").sortingOrder);
            Assert.AreEqual(AutoPopulator.Layer8SortingOrder, RendererFor("_BlueprintPlaced_L8_feature_0_0").sortingOrder);

            SpriteRenderer medium = RendererFor("_BlueprintPlaced_L6_feature_0_0");
            Assert.AreEqual(Mathf.RoundToInt(-medium.transform.position.y), medium.sortingOrder);

            SpriteRenderer tall = root.Cast<Transform>()
                .Where(child => child.name.StartsWith("_BlueprintPlaced_L7_feature_"))
                .Select(child => child.GetComponent<SpriteRenderer>())
                .First(renderer => renderer != null);
            Assert.AreEqual(Mathf.RoundToInt(-tall.transform.position.y), tall.sortingOrder);
        }

        [Test]
        public void Populate8Layers_DeterministicWithSeed()
        {
            BlueprintZoneTypeSO zone = CreateCompleteZone("grass");
            BlueprintProfileSO profile = CreateProfile(zone);
            BlueprintCanvas canvas = FilledCanvas(10, 10, "grass");
            Transform otherRoot = new GameObject("OtherRoot").transform;
            createdObjects.Add(otherRoot.gameObject);

            AutoPopulator.PopulateZones(canvas, profile, root, 109);
            AutoPopulator.PopulateZones(canvas, profile, otherRoot, 109);

            CollectionAssert.AreEqual(PlacementSignature(root), PlacementSignature(otherRoot));
        }

        private SpriteRenderer RendererFor(string name)
        {
            Transform child = root.Find(name);
            Assert.IsNotNull(child, name);
            return child.GetComponent<SpriteRenderer>();
        }

        private string[] PlacementSignature(Transform targetRoot)
        {
            return targetRoot.Cast<Transform>()
                .Where(child => child.name.StartsWith(AutoPopulator.PlacedPrefix))
                .Select(child =>
                {
                    SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
                    string spriteName = renderer != null && renderer.sprite != null ? renderer.sprite.name : "null";
                    Vector3 pos = child.position;
                    int sortingOrder = renderer != null ? renderer.sortingOrder : 0;
                    return $"{child.name}|{spriteName}|{pos.x:F3}|{pos.y:F3}|{sortingOrder}";
                })
                .OrderBy(value => value)
                .ToArray();
        }

        private BlueprintCanvas FilledCanvas(int width, int height, string zoneId)
        {
            var canvas = new BlueprintCanvas(new Vector2Int(width, height));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    canvas.Paint(new Vector2Int(x, y), zoneId, 1);
                }
            }

            return canvas;
        }

        private BlueprintProfileSO CreateProfile(params BlueprintZoneTypeSO[] zones)
        {
            BlueprintProfileSO profile = ScriptableObject.CreateInstance<BlueprintProfileSO>();
            profile.profileId = "test_profile";
            profile.zones = zones;
            profile.adjacencyRules = new BlueprintAdjacencyRuleSO[0];
            profile.gridSize = new Vector2Int(36, 22);
            createdObjects.Add(profile);
            return profile;
        }

        private BlueprintZoneTypeSO CreateCompleteZone(string zoneId)
        {
            Sprite macro = CreateSprite(zoneId + "_macro");
            Sprite baseFloor = CreateSprite(zoneId + "_base");
            BlueprintPropPoolSO pool = CreatePool(zoneId + "_pool", CreateProp(zoneId + "_prop"));
            BlueprintPropPoolSO tallPool = CreatePool(zoneId + "_tall_pool", CreateProp(zoneId + "_tall"));

            BlueprintZoneTypeSO zone = ScriptableObject.CreateInstance<BlueprintZoneTypeSO>();
            zone.zoneId = zoneId;
            zone.displayName = zoneId;
            zone.brushColor = Color.white;
            zone.defaultDensity = 1f;
            zone.maxFeatureProps = 2;
            zone.macroFillSprites = new[] { macro };
            zone.baseFloorSprites = new[] { baseFloor };
            zone.midToneOverlayPool = pool;
            zone.detailTexturePool = pool;
            zone.smallScatterPool = pool;
            zone.mediumPropPool = pool;
            zone.tallFocalPool = tallPool;
            zone.atmosphericPool = pool;
            createdObjects.Add(zone);
            return zone;
        }

        private BlueprintPropPoolSO CreatePool(string poolId, PropDefinitionSO prop)
        {
            BlueprintPropPoolSO pool = ScriptableObject.CreateInstance<BlueprintPropPoolSO>();
            pool.poolId = poolId;
            pool.entries = new[] { new WeightedProp { prop = prop, weight = 1f } };
            createdObjects.Add(pool);
            return pool;
        }

        private PropDefinitionSO CreateProp(string propId)
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.name = propId;
            prop.propId = propId;
            prop.visual = CreateSprite(propId + "_sprite");
            createdObjects.Add(prop);
            return prop;
        }

        private Sprite CreateSprite(string spriteName)
        {
            var texture = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            texture.name = spriteName + "_Texture";
            createdObjects.Add(texture);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 8f);
            sprite.name = spriteName;
            createdObjects.Add(sprite);
            return sprite;
        }
    }
}
