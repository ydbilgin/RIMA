using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner.Tests
{
    public sealed class V15hPlayableMapTests
    {
        private readonly List<UnityEngine.Object> createdObjects = new List<UnityEngine.Object>();
        private Transform root;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("V15hPlayableMapTestRoot").transform;
            createdObjects.Add(root.gameObject);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                if (createdObjects[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(createdObjects[i]);
                }
            }

            createdObjects.Clear();
            root = null;
        }

        [Test]
        public void V15hWangRuleTile_Validates16TileMapping()
        {
            RimaV15hPlayableComposer.EnsureV15hAssetGraph();

            BlueprintProfileSO profile = Load<BlueprintProfileSO>(RimaV15hPlayableComposer.ProfilePath);
            Assert.NotNull(profile.wangRuleTileRef);
            Assert.AreEqual(RimaV15hPlayableComposer.WangRuleTilePath, AssetDatabase.GetAssetPath(profile.wangRuleTileRef));

            for (int i = 0; i < 16; i++)
            {
                Tile tile = Load<Tile>(RimaV15hPlayableComposer.WangTilePath(i));
                Assert.NotNull(tile.sprite, "tile sprite " + i);
            }

            ScriptableObject ruleTile = Load<ScriptableObject>(RimaV15hPlayableComposer.WangRuleTilePath);
            var serialized = new SerializedObject(ruleTile);
            SerializedProperty rules = serialized.FindProperty("m_TilingRules");
            Assert.NotNull(rules);
            Assert.AreEqual(16, rules.arraySize);
        }

        [Test]
        public void V15hAdjacencyDecalsCap_IsRespected()
        {
            BlueprintPropPoolSO pool = CreatePool("adjacency_pool", CreateProp("adjacency_prop"));
            BlueprintZoneTypeSO dirt = CreateZone("dirt", pool);
            BlueprintZoneTypeSO stone = CreateZone("stone", pool);
            BlueprintAdjacencyRuleSO rule = ScriptableObject.CreateInstance<BlueprintAdjacencyRuleSO>();
            rule.ruleId = "cap_rule";
            rule.zoneIdA = "dirt";
            rule.zoneIdB = "stone";
            rule.transitionPool = pool;
            rule.density = 1f;
            rule.decalsPerRoomCap = 8;
            createdObjects.Add(rule);

            BlueprintProfileSO profile = ScriptableObject.CreateInstance<BlueprintProfileSO>();
            profile.zones = new[] { dirt, stone };
            profile.adjacencyRules = new[] { rule };
            createdObjects.Add(profile);

            var canvas = new BlueprintCanvas(new Vector2Int(18, 2));
            for (int x = 0; x < 18; x++)
            {
                canvas.Paint(new Vector2Int(x, 0), x % 2 == 0 ? "dirt" : "stone", 1);
                canvas.Paint(new Vector2Int(x, 1), x % 2 == 0 ? "stone" : "dirt", 1);
            }

            int placed = AutoPopulator.PopulateAdjacency(canvas, profile, root, 55);

            Assert.AreEqual(8, placed);
            Assert.AreEqual(8, root.Cast<Transform>().Count(child => child.name.StartsWith("_BlueprintPlaced_adjacency_", StringComparison.Ordinal)));
        }

        [Test]
        public void V15hDensity85_ProducesAbout85PercentCellFill()
        {
            string metrics = RimaV15hPlayableComposer.BuildPreviewMetricsText();
            float coverage = ParseFloatMetric(metrics, "FloorFillCoverage");

            Assert.That(coverage, Is.InRange(0.82f, 0.88f));
        }

        private static T Load<T>(string path) where T : UnityEngine.Object
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            Assert.NotNull(asset, path);
            return asset;
        }

        private BlueprintZoneTypeSO CreateZone(string zoneId, BlueprintPropPoolSO pool)
        {
            BlueprintZoneTypeSO zone = ScriptableObject.CreateInstance<BlueprintZoneTypeSO>();
            zone.zoneId = zoneId;
            zone.displayName = zoneId;
            zone.macroFillSprites = new[] { pool.entries[0].prop.visual };
            zone.baseFloorSprites = new[] { pool.entries[0].prop.visual };
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

        private static float ParseFloatMetric(string metrics, string key)
        {
            string prefix = key + "=";
            string[] lines = metrics.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(prefix, StringComparison.Ordinal))
                {
                    return float.Parse(lines[i].Substring(prefix.Length), System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            Assert.Fail("Missing metric line: " + prefix);
            return 0f;
        }
    }
}
