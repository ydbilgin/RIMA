using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    /// <summary>
    /// DungeonGraph mantığı için Edit Mode testleri.
    /// Window > General > Test Runner > EditMode ile çalıştır.
    /// </summary>
    public class DungeonGraphTests
    {
        private DungeonGraph graph;

        [SetUp]
        public void SetUp()
        {
            Random.InitState(42); // Deterministik test
            var go = new GameObject("DungeonGraph_Test");
            graph = go.AddComponent<DungeonGraph>();
            // Awake manuel çağrılmıyor; Generate() ile init ediyoruz
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(graph.gameObject);
        }

        [Test]
        public void Generate_CreatesExpectedNodes()
        {
            graph.Generate();
            // Triple fork mantığı: 12 base + 0-2 extra node
            Assert.That(graph.TotalNodes, Is.InRange(12, 14),
                $"Graf 12-14 node içermeli (triple fork dahil). Gerçek: {graph.TotalNodes}");
        }

        [Test]
        public void StartNode_IsVisited()
        {
            graph.Generate();
            var start = graph.AllNodes[0];
            Assert.IsTrue(start.visited, "Başlangıç node'u visited=true olmalı.");
        }

        [Test]
        public void BossNode_IsLastNode()
        {
            graph.Generate();
            Assert.AreEqual(graph.BossNodeId, 11, "Boss node ID 11 olmalı.");
            Assert.AreEqual(RoomType.Boss, graph.AllNodes[11].roomType, "Node 11 Boss tipinde olmalı.");
        }

        [Test]
        public void StartNode_HasExits()
        {
            graph.Generate();
            var start = graph.AllNodes[0];
            Assert.IsTrue(start.exits.Count > 0, "Başlangıç node'unun çıkışı olmalı.");
        }

        [Test]
        public void RevealAhead_1Step_RevealsDirectExits()
        {
            graph.Generate();
            // Başlangıç node'unun direkt çıkışlarını kontrol et
            graph.RevealAhead(1);
            var start = graph.AllNodes[0];
            foreach (var kvp in start.exits)
            {
                Assert.IsTrue(graph.AllNodes[kvp.Value].revealed,
                    $"Node {kvp.Value} (direkt çıkış) revealed=true olmalı.");
            }
        }

        [Test]
        public void AllNodes_HaveValidDepths()
        {
            graph.Generate();
            foreach (var node in graph.AllNodes)
            {
                Assert.GreaterOrEqual(node.depth, 0, $"Node {node.id} depth negatif olamaz.");
                Assert.LessOrEqual(node.depth, 10, $"Node {node.id} depth makul sınırda olmalı.");
            }
        }

        [Test]
        public void BossNode_HasNoExits()
        {
            graph.Generate();
            var boss = graph.AllNodes[graph.BossNodeId];
            Assert.AreEqual(0, boss.exits.Count, "Boss node'unun çıkışı olmamalı.");
        }
    }
}
