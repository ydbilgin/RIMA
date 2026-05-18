using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using RIMA.MapDesigner.SO;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class AutoPopulatorTests
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
        public void WeightedPick_DeterministicWithSeed()
        {
            PropDefinitionSO a = CreateProp("a");
            PropDefinitionSO b = CreateProp("b");
            BlueprintPropPoolSO pool = CreatePool("pool", (a, 1f), (b, 1f));

            Assert.AreEqual(pool.PickWeighted(42), pool.PickWeighted(42));
        }

        [Test]
        public void WeightedPick_RespectsWeights()
        {
            PropDefinitionSO a = CreateProp("a");
            PropDefinitionSO b = CreateProp("b");
            BlueprintPropPoolSO pool = CreatePool("pool", (a, 3f), (b, 1f));

            int pickedA = 0;
            for (int i = 0; i < 1000; i++)
            {
                if (pool.PickWeighted(i) == a)
                {
                    pickedA++;
                }
            }

            Assert.That(pickedA, Is.InRange(700, 800));
        }

        [Test]
        public void PopulateZones_PlacesPropsOnlyFromZonePool()
        {
            PropDefinitionSO grassProp = CreateProp("grass_prop");
            PropDefinitionSO stoneProp = CreateProp("stone_prop");
            BlueprintPropPoolSO grassPool = CreatePool("pool_grass", (grassProp, 1f));
            BlueprintPropPoolSO stonePool = CreatePool("pool_stone", (stoneProp, 1f));
            BlueprintProfileSO profile = CreateProfile(
                CreateZone("grass", grassPool, 1f),
                CreateZone("stone", stonePool, 1f));
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            canvas.Paint(new Vector2Int(0, 0), "grass", 1);
            canvas.Paint(new Vector2Int(1, 0), "stone", 1);

            int placed = AutoPopulator.PopulateZones(canvas, profile, root, 7);

            Assert.AreEqual(2, placed);
            Assert.AreEqual(grassProp.visual, root.Find("_BlueprintPlaced_grass_0_0").GetComponent<SpriteRenderer>().sprite);
            Assert.AreEqual(stoneProp.visual, root.Find("_BlueprintPlaced_stone_1_0").GetComponent<SpriteRenderer>().sprite);
        }

        [Test]
        public void PopulateZones_RespectsDensity()
        {
            BlueprintPropPoolSO pool = CreatePool("pool_grass", (CreateProp("grass"), 1f));
            BlueprintProfileSO profile = CreateProfile(CreateZone("grass", pool, 0.5f));
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    canvas.Paint(new Vector2Int(x, y), "grass", 1);
                }
            }

            int placed = AutoPopulator.PopulateZones(canvas, profile, root, 11);

            Assert.That(placed, Is.InRange(40, 60));
        }

        [Test]
        public void PopulateZones_FeatureZoneRespectsMaxCap()
        {
            BlueprintPropPoolSO pool = CreatePool("pool_feature", (CreateProp("feature"), 1f));
            BlueprintZoneTypeSO feature = CreateZone("feature", pool, 1f);
            feature.maxFeatureProps = 2;
            BlueprintProfileSO profile = CreateProfile(feature);
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            canvas.Paint(new Vector2Int(5, 5), "feature", 3);

            int placed = AutoPopulator.PopulateZones(canvas, profile, root, 13);

            Assert.AreEqual(2, placed);
        }

        [Test]
        public void PopulateAdjacency_PlacesTransitionAtBoundary()
        {
            BlueprintPropPoolSO grassPool = CreatePool("pool_grass", (CreateProp("grass_transition"), 1f));
            BlueprintZoneTypeSO grass = CreateZone("grass", grassPool, 1f);
            BlueprintZoneTypeSO stone = CreateZone("stone", CreatePool("pool_stone", (CreateProp("stone"), 1f)), 1f);
            BlueprintAdjacencyRuleSO rule = CreateRule("grass_stone", "grass", "stone", grassPool, 1f);
            BlueprintProfileSO profile = CreateProfile(new[] { grass, stone }, new[] { rule });
            var canvas = new BlueprintCanvas(new Vector2Int(36, 22));
            canvas.Paint(new Vector2Int(0, 0), "grass", 1);
            canvas.Paint(new Vector2Int(1, 0), "stone", 1);

            int placed = AutoPopulator.PopulateAdjacency(canvas, profile, root, 17);

            Assert.AreEqual(1, placed);
            Assert.IsTrue(root.Cast<Transform>().Any(child => child.name.StartsWith("_BlueprintPlaced_adjacency_")));
        }

        [Test]
        public void ClearPlacedProps_RemovesOnlyBlueprintTagged()
        {
            new GameObject("_BlueprintPlaced_grass_0_0").transform.SetParent(root);
            new GameObject("UserAuthored").transform.SetParent(root);

            int cleared = AutoPopulator.ClearPlacedProps(root);

            Assert.AreEqual(1, cleared);
            Assert.IsNull(root.Find("_BlueprintPlaced_grass_0_0"));
            Assert.IsNotNull(root.Find("UserAuthored"));
        }

        private BlueprintProfileSO CreateProfile(params BlueprintZoneTypeSO[] zones)
        {
            return CreateProfile(zones, new BlueprintAdjacencyRuleSO[0]);
        }

        private BlueprintProfileSO CreateProfile(BlueprintZoneTypeSO[] zones, BlueprintAdjacencyRuleSO[] rules)
        {
            BlueprintProfileSO profile = ScriptableObject.CreateInstance<BlueprintProfileSO>();
            profile.profileId = "test_profile";
            profile.zones = zones;
            profile.adjacencyRules = rules;
            profile.gridSize = new Vector2Int(36, 22);
            createdObjects.Add(profile);
            return profile;
        }

        private BlueprintZoneTypeSO CreateZone(string zoneId, BlueprintPropPoolSO pool, float density)
        {
            BlueprintZoneTypeSO zone = ScriptableObject.CreateInstance<BlueprintZoneTypeSO>();
            zone.zoneId = zoneId;
            zone.displayName = zoneId;
            zone.propPool = pool;
            zone.defaultDensity = density;
            createdObjects.Add(zone);
            return zone;
        }

        private BlueprintAdjacencyRuleSO CreateRule(string ruleId, string zoneA, string zoneB, BlueprintPropPoolSO pool, float density)
        {
            BlueprintAdjacencyRuleSO rule = ScriptableObject.CreateInstance<BlueprintAdjacencyRuleSO>();
            rule.ruleId = ruleId;
            rule.zoneIdA = zoneA;
            rule.zoneIdB = zoneB;
            rule.transitionPool = pool;
            rule.density = density;
            createdObjects.Add(rule);
            return rule;
        }

        private BlueprintPropPoolSO CreatePool(string poolId, params (PropDefinitionSO prop, float weight)[] entries)
        {
            BlueprintPropPoolSO pool = ScriptableObject.CreateInstance<BlueprintPropPoolSO>();
            pool.poolId = poolId;
            pool.entries = entries.Select(entry => new WeightedProp { prop = entry.prop, weight = entry.weight }).ToArray();
            createdObjects.Add(pool);
            return pool;
        }

        private PropDefinitionSO CreateProp(string propId)
        {
            var texture = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            texture.name = propId + "_Texture";
            createdObjects.Add(texture);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 8f);
            sprite.name = propId + "_Sprite";
            createdObjects.Add(sprite);

            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.name = propId;
            prop.propId = propId;
            prop.visual = sprite;
            createdObjects.Add(prop);
            return prop;
        }
    }
}
