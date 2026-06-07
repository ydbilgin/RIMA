using System;
using System.Collections.Generic;

namespace RIMA.MapDesigner.Room.Runtime
{
    public sealed class DungeonNode
    {
        public int id;
        public int depth;
        public RIMA.RoomType roomType;
        public List<int> childIds = new List<int>();
    }

    public sealed class DungeonGraph
    {
        public List<DungeonNode> nodes = new List<DungeonNode>();
        public int startId;
        public int maxDepth;

        public DungeonNode Get(int id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].id == id)
                {
                    return nodes[i];
                }
            }

            return null;
        }

        public List<DungeonNode> ChildrenOf(int id)
        {
            var result = new List<DungeonNode>();
            DungeonNode node = Get(id);
            if (node == null)
            {
                return result;
            }

            for (int i = 0; i < node.childIds.Count; i++)
            {
                DungeonNode child = Get(node.childIds[i]);
                if (child != null)
                {
                    result.Add(child);
                }
            }

            return result;
        }

        public List<DungeonNode> NodesAtDepth(int depth)
        {
            var result = new List<DungeonNode>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].depth == depth)
                {
                    result.Add(nodes[i]);
                }
            }

            return result;
        }

        public static DungeonGraph Generate(int seed, int depthCount)
        {
            if (depthCount < 2)
            {
                depthCount = 2;
            }

            var random = new Random(seed);
            var graph = new DungeonGraph
            {
                startId = 0,
                maxDepth = depthCount - 1
            };

            var rows = new List<List<DungeonNode>>();
            int nextId = 0;
            for (int depth = 0; depth < depthCount; depth++)
            {
                int count = NodeCountAtDepth(random, depth, depthCount);
                var row = new List<DungeonNode>(count);
                for (int lane = 0; lane < count; lane++)
                {
                    var node = new DungeonNode
                    {
                        id = nextId++,
                        depth = depth,
                        roomType = RoomTypeAtDepth(random, depth, depthCount)
                    };
                    graph.nodes.Add(node);
                    row.Add(node);
                }

                rows.Add(row);
            }

            for (int depth = 0; depth < depthCount - 1; depth++)
            {
                ConnectRows(random, rows[depth], rows[depth + 1]);
            }

            return graph;
        }

        private static int NodeCountAtDepth(Random random, int depth, int depthCount)
        {
            if (depth == 0 || depth == depthCount - 1)
            {
                return 1;
            }

            return random.Next(2, 4);
        }

        private static RIMA.RoomType RoomTypeAtDepth(Random random, int depth, int depthCount)
        {
            if (depth == 0)
            {
                return RIMA.RoomType.Combat;
            }

            if (depth == depthCount - 1)
            {
                return RIMA.RoomType.Boss;
            }

            int roll = random.Next(100);
            if (roll < 55)
            {
                return RIMA.RoomType.Combat;
            }

            if (roll < 75)
            {
                return RIMA.RoomType.Elite;
            }

            if (roll < 90)
            {
                return RIMA.RoomType.Chest;
            }

            // Demo guard (F-008, STATICAUDIT_DECISION_2026-06-07): Event node demo kapsamı dışı — Combat/Elite/Reward/Boss only.
            return RIMA.RoomType.Combat;
        }

        private static void ConnectRows(Random random, List<DungeonNode> current, List<DungeonNode> next)
        {
            if (next.Count == 1)
            {
                for (int i = 0; i < current.Count; i++)
                {
                    current[i].childIds.Add(next[0].id);
                }

                return;
            }

            var parentCounts = new int[next.Count];
            for (int i = 0; i < current.Count; i++)
            {
                int childCount = random.Next(1, Math.Min(3, next.Count) + 1);
                var lanes = PreferredLaneOrder(random, i, current.Count, next.Count);
                for (int c = 0; c < childCount; c++)
                {
                    AddChild(current[i], next[lanes[c]], lanes[c], parentCounts);
                }
            }

            for (int i = 0; i < next.Count; i++)
            {
                if (parentCounts[i] > 0)
                {
                    continue;
                }

                DungeonNode parent = current[NearestParentIndex(i, next.Count, current.Count)];
                AddChild(parent, next[i], i, parentCounts);
            }
        }

        private static List<int> PreferredLaneOrder(Random random, int parentIndex, int parentCount, int childCount)
        {
            var lanes = new List<int>(childCount);
            for (int i = 0; i < childCount; i++)
            {
                lanes.Add(i);
            }

            var tieBreakers = new int[childCount];
            for (int i = 0; i < childCount; i++)
            {
                tieBreakers[i] = random.Next();
            }

            float projected = childCount == 1 || parentCount == 1
                ? (childCount - 1) * 0.5f
                : parentIndex * (childCount - 1f) / (parentCount - 1f);

            lanes.Sort((a, b) =>
            {
                float aDistance = Math.Abs(a - projected);
                float bDistance = Math.Abs(b - projected);
                int compare = aDistance.CompareTo(bDistance);
                if (compare != 0)
                {
                    return compare;
                }

                return tieBreakers[a].CompareTo(tieBreakers[b]);
            });

            return lanes;
        }

        private static int NearestParentIndex(int childIndex, int childCount, int parentCount)
        {
            if (parentCount <= 1 || childCount <= 1)
            {
                return 0;
            }

            return (int)Math.Round(childIndex * (parentCount - 1f) / (childCount - 1f));
        }

        private static void AddChild(DungeonNode parent, DungeonNode child, int childIndex, int[] parentCounts)
        {
            if (!parent.childIds.Contains(child.id))
            {
                parent.childIds.Add(child.id);
                parentCounts[childIndex]++;
            }
        }
    }
}
