using System.Collections.Generic;
using NUnit.Framework;
using RuntimeDungeonGraph = RIMA.MapDesigner.Room.Runtime.DungeonGraph;
using RuntimeDungeonNode = RIMA.MapDesigner.Room.Runtime.DungeonNode;

namespace RIMA.Tests.Room
{
    public class RoomRuntimeDungeonGraphTests
    {
        [Test]
        public void Generate_SameSeed_ProducesIdenticalGraph()
        {
            RuntimeDungeonGraph g1 = RuntimeDungeonGraph.Generate(123, 5);
            RuntimeDungeonGraph g2 = RuntimeDungeonGraph.Generate(123, 5);

            Assert.AreEqual(g1.nodes.Count, g2.nodes.Count);
            for (int i = 0; i < g1.nodes.Count; i++)
            {
                Assert.AreEqual(g1.nodes[i].id, g2.nodes[i].id);
                Assert.AreEqual(g1.nodes[i].depth, g2.nodes[i].depth);
                Assert.AreEqual(g1.nodes[i].roomType, g2.nodes[i].roomType);
                CollectionAssert.AreEqual(g1.nodes[i].childIds, g2.nodes[i].childIds);
            }
        }

        [Test]
        public void Generate_StructureInvariants_HoldForManySeeds()
        {
            int[] depthCounts = { 2, 3, 5, 8 };
            for (int seed = 0; seed < 50; seed++)
            {
                foreach (int depthCount in depthCounts)
                {
                    RuntimeDungeonGraph graph = RuntimeDungeonGraph.Generate(seed, depthCount);
                    string context = $"seed={seed}, depthCount={depthCount}";

                    Assert.AreEqual(0, graph.startId, context);
                    Assert.AreEqual(depthCount - 1, graph.maxDepth, context);

                    List<RuntimeDungeonNode> starts = graph.NodesAtDepth(0);
                    Assert.AreEqual(1, starts.Count, context);
                    Assert.AreEqual(RIMA.RoomType.Combat, starts[0].roomType, context);

                    List<RuntimeDungeonNode> bosses = graph.NodesAtDepth(graph.maxDepth);
                    Assert.AreEqual(1, bosses.Count, context);
                    Assert.AreEqual(RIMA.RoomType.Boss, bosses[0].roomType, context);
                    Assert.AreEqual(0, bosses[0].childIds.Count, context);

                    var childIds = new HashSet<int>();
                    foreach (RuntimeDungeonNode node in graph.nodes)
                    {
                        foreach (int childId in node.childIds)
                        {
                            childIds.Add(childId);
                        }
                    }

                    foreach (RuntimeDungeonNode node in graph.nodes)
                    {
                        if (node.depth > 0)
                        {
                            Assert.IsTrue(childIds.Contains(node.id), context);
                        }

                        if (node.depth < graph.maxDepth)
                        {
                            Assert.GreaterOrEqual(node.childIds.Count, 1, context);
                            Assert.LessOrEqual(node.childIds.Count, 3, context);
                        }
                    }

                    Assert.AreEqual(graph.nodes.Count, CountReachableNodes(graph), context);
                }
            }
        }

        [Test]
        public void Generate_DepthCountBelowTwo_ClampsToTwo()
        {
            RuntimeDungeonGraph graph = RuntimeDungeonGraph.Generate(7, 1);

            Assert.AreEqual(1, graph.maxDepth);
            Assert.AreEqual(1, HighestDepth(graph));
            Assert.AreEqual(RIMA.RoomType.Combat, graph.NodesAtDepth(0)[0].roomType);
            Assert.AreEqual(RIMA.RoomType.Boss, graph.NodesAtDepth(1)[0].roomType);
        }

        [Test]
        public void Generate_ZeroAndNegativeSeed_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => RuntimeDungeonGraph.Generate(0, 5));
            Assert.DoesNotThrow(() => RuntimeDungeonGraph.Generate(-99, 5));
            Assert.Greater(RuntimeDungeonGraph.Generate(0, 5).nodes.Count, 0);
            Assert.Greater(RuntimeDungeonGraph.Generate(-99, 5).nodes.Count, 0);
        }

        private static int CountReachableNodes(RuntimeDungeonGraph graph)
        {
            var visited = new HashSet<int>();
            var stack = new Stack<int>();
            stack.Push(graph.startId);

            while (stack.Count > 0)
            {
                int id = stack.Pop();
                if (!visited.Add(id))
                {
                    continue;
                }

                RuntimeDungeonNode node = graph.Get(id);
                if (node == null)
                {
                    continue;
                }

                foreach (int childId in node.childIds)
                {
                    stack.Push(childId);
                }
            }

            return visited.Count;
        }

        private static int HighestDepth(RuntimeDungeonGraph graph)
        {
            int highest = 0;
            foreach (RuntimeDungeonNode node in graph.nodes)
            {
                if (node.depth > highest)
                {
                    highest = node.depth;
                }
            }

            return highest;
        }
    }
}
