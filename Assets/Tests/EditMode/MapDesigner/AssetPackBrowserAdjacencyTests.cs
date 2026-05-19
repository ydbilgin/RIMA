using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Editor;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class AssetPackBrowserAdjacencyTests
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
        public void SemanticGrouping_Recognizes_CombatTilePatterns()
        {
            Assert.AreEqual(AssetPackBrowserAdjacency.FloorDominant, AssetPackBrowserAdjacency.GetSemanticCategory("combat_floor_dominant_a"));
            Assert.AreEqual(AssetPackBrowserAdjacency.Path, AssetPackBrowserAdjacency.GetSemanticCategory("combat_path_03"));
            Assert.AreEqual(AssetPackBrowserAdjacency.Transition, AssetPackBrowserAdjacency.GetSemanticCategory("combat_floor_transition_wang_ne"));
            Assert.AreEqual(AssetPackBrowserAdjacency.Dirt, AssetPackBrowserAdjacency.GetSemanticCategory("combat_dirt_scatter_02"));
            Assert.AreEqual(AssetPackBrowserAdjacency.Secondary, AssetPackBrowserAdjacency.GetSemanticCategory("combat_secondary_floor_01"));
        }

        [Test]
        public void BuildAdjacencyMatrix_ReturnsEightEntries_FromSameGroup()
        {
            AssetPackEntry center = CreateEntry("center", "combat_path_center", AssetPackBrowserAdjacency.Path);
            AssetPackEntry pathA = CreateEntry("path_a", "combat_path_a", AssetPackBrowserAdjacency.Path);
            AssetPackEntry pathB = CreateEntry("path_b", "combat_path_b", AssetPackBrowserAdjacency.Path);
            AssetPackEntry transition = CreateEntry("transition", "combat_transition_a", AssetPackBrowserAdjacency.Transition);

            AssetPackEntry[] neighbors = AssetPackBrowserAdjacency.BuildAdjacencyMatrix(center, new[] { center, pathA, transition, pathB });

            Assert.AreEqual(8, neighbors.Length);
            for (int i = 0; i < neighbors.Length; i++)
            {
                Assert.IsNotNull(neighbors[i]);
                Assert.AreEqual(AssetPackBrowserAdjacency.Path, AssetPackBrowserAdjacency.GetSemanticCategory(neighbors[i]));
                Assert.AreNotEqual(transition.entryId, neighbors[i].entryId);
            }
        }

        [Test]
        public void BuildAdjacencyMatrix_FallsBackToCenter_WhenNoMatchesExist()
        {
            AssetPackEntry center = CreateEntry("center", "combat_path_center", AssetPackBrowserAdjacency.Path);

            AssetPackEntry[] neighbors = AssetPackBrowserAdjacency.BuildAdjacencyMatrix(center, new[] { center });

            Assert.AreEqual(8, neighbors.Length);
            for (int i = 0; i < neighbors.Length; i++)
            {
                Assert.AreEqual(center, neighbors[i]);
            }
        }

        [Test]
        public void HoverPopup_RequiresHalfSecondDelay()
        {
            Assert.IsFalse(AssetPackBrowserAdjacency.ShouldShowHoverPopup(10d, 10.49d));
            Assert.IsTrue(AssetPackBrowserAdjacency.ShouldShowHoverPopup(10d, 10.5d));
        }

        private AssetPackEntry CreateEntry(string id, string name, string category)
        {
            return new AssetPackEntry
            {
                entryId = id,
                displayName = name,
                categoryName = category,
                sprite = CreateSprite(name),
                sourcePack = "Test Pack",
                sourceAtlas = category,
                pixelSize = new Vector2Int(32, 32),
                pixelsPerUnit = 32
            };
        }

        private Sprite CreateSprite(string spriteName)
        {
            Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            texture.name = spriteName + "_Texture";
            createdObjects.Add(texture);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32f);
            sprite.name = spriteName;
            createdObjects.Add(sprite);
            return sprite;
        }
    }
}
