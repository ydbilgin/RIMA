using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using RuntimeDungeonGraph = RIMA.MapDesigner.Room.Runtime.DungeonGraph;
using RuntimeDungeonNode = RIMA.MapDesigner.Room.Runtime.DungeonNode;

namespace RIMA.Tests.Room
{
    /// <summary>
    /// Validates the fixed demo sequence graph (Task 1) and that the DemoRoomBank
    /// can resolve a non-null template for every node type in the sequence (Task 2).
    /// </summary>
    public class DemoSequenceTests
    {
        private const string DemoRoomBankPath = "Assets/Data/Rooms/DemoRoomBank.asset";

        // ── Graph structure ───────────────────────────────────────────────────────

        [Test]
        public void DemoSequence_ProducesExactlyFiveNodes()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();
            Assert.AreEqual(5, graph.nodes.Count, "Demo sequence must have exactly 5 nodes.");
        }

        [Test]
        public void DemoSequence_NodeOrder_IsCorrect()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();

            Assert.AreEqual(RIMA.RoomType.Combat,   graph.nodes[0].roomType, "Node 0 must be Combat.");
            Assert.AreEqual(RIMA.RoomType.Combat,   graph.nodes[1].roomType, "Node 1 must be Combat.");
            Assert.AreEqual(RIMA.RoomType.Merchant, graph.nodes[2].roomType, "Node 2 must be Merchant.");
            Assert.AreEqual(RIMA.RoomType.Combat,   graph.nodes[3].roomType, "Node 3 must be Combat.");
            Assert.AreEqual(RIMA.RoomType.Boss,     graph.nodes[4].roomType, "Node 4 must be Boss.");
        }

        [Test]
        public void DemoSequence_NonTerminalNodes_HaveExactlyOneChild()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();

            // Nodes 0–3 must each have exactly one child.
            for (int i = 0; i < graph.nodes.Count - 1; i++)
            {
                Assert.AreEqual(1, graph.nodes[i].childIds.Count,
                    $"Node {i} (type={graph.nodes[i].roomType}) must have exactly 1 child.");
                Assert.AreEqual(i + 1, graph.nodes[i].childIds[0],
                    $"Node {i} child must be node {i + 1}.");
            }
        }

        [Test]
        public void DemoSequence_BossNode_HasZeroChildren()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();
            RuntimeDungeonNode boss = graph.nodes[graph.nodes.Count - 1];

            Assert.AreEqual(RIMA.RoomType.Boss, boss.roomType, "Last node must be Boss.");
            Assert.AreEqual(0, boss.childIds.Count, "Boss node must have 0 children (terminal).");
        }

        [Test]
        public void DemoSequence_StartId_IsZero()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();
            Assert.AreEqual(0, graph.startId);
        }

        [Test]
        public void DemoSequence_IsFullyLinear_AllNodesReachable()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();

            // Walk from start to end and confirm we visit all 5 nodes in order.
            int current = graph.startId;
            int steps = 0;
            while (true)
            {
                RuntimeDungeonNode node = graph.Get(current);
                Assert.IsNotNull(node, $"Node id={current} must exist in graph.");
                steps++;

                if (node.childIds.Count == 0) break;
                Assert.AreEqual(1, node.childIds.Count, $"Node {current} must have exactly 1 child in linear sequence.");
                current = node.childIds[0];
            }

            Assert.AreEqual(5, steps, "Traversal must visit exactly 5 nodes.");
        }

        // ── Bank resolution ───────────────────────────────────────────────────────

        [Test]
        public void DemoRoomBank_ResolvesNonNullTemplate_ForCombat()
        {
            RoomBankSO bank = AssetDatabase.LoadAssetAtPath<RoomBankSO>(DemoRoomBankPath);
            Assert.IsNotNull(bank, $"Missing bank at {DemoRoomBankPath}.");

            RoomTemplateSO template = bank.Pick(RIMA.RoomType.Combat, seed: 0);
            Assert.IsNotNull(template, "DemoRoomBank must resolve a non-null template for Combat.");
        }

        [Test]
        public void DemoRoomBank_ResolvesNonNullTemplate_ForMerchant()
        {
            RoomBankSO bank = AssetDatabase.LoadAssetAtPath<RoomBankSO>(DemoRoomBankPath);
            Assert.IsNotNull(bank, $"Missing bank at {DemoRoomBankPath}.");

            // PLACEHOLDER: merchantRooms reuses Combat_Large_01 until the real shop room is built.
            RoomTemplateSO template = bank.Pick(RIMA.RoomType.Merchant, seed: 0);
            Assert.IsNotNull(template,
                "DemoRoomBank must resolve a non-null template for Merchant (placeholder = Combat_Large_01).");
        }

        [Test]
        public void DemoRoomBank_ResolvesNonNullTemplate_ForBoss()
        {
            RoomBankSO bank = AssetDatabase.LoadAssetAtPath<RoomBankSO>(DemoRoomBankPath);
            Assert.IsNotNull(bank, $"Missing bank at {DemoRoomBankPath}.");

            RoomTemplateSO template = bank.Pick(RIMA.RoomType.Boss, seed: 0);
            Assert.IsNotNull(template, "DemoRoomBank must resolve a non-null template for Boss.");
        }

        [Test]
        public void DemoSequence_AllNodeTypes_ResolveNonNullTemplateInBank()
        {
            RoomBankSO bank = AssetDatabase.LoadAssetAtPath<RoomBankSO>(DemoRoomBankPath);
            Assert.IsNotNull(bank, $"Missing bank at {DemoRoomBankPath}.");

            RuntimeDungeonGraph graph = RuntimeDungeonGraph.BuildDemoSequence();
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                RIMA.RoomType roomType = graph.nodes[i].roomType;
                RoomTemplateSO template = bank.Pick(roomType, seed: i);
                Assert.IsNotNull(template,
                    $"Node {i} type={roomType} must resolve a non-null template in DemoRoomBank.");
            }
        }
    }
}
