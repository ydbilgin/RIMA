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

        // ── Demo sequence ─────────────────────────────────────────────────────────
        // DEMO-ONLY: fixed linear sequence so every playthrough visits the same
        // room types in the same order. NOT used by the regular random generator.
        // Sequence: Combat → Combat → Merchant → Combat → Boss → Combat (6 nodes, linear).
        // The final Combat node (post-boss dual-class arena) is terminal; Boss is NOT.
        public static readonly RIMA.RoomType[] DemoSequence =
        {
            RIMA.RoomType.Combat,
            RIMA.RoomType.Combat,
            RIMA.RoomType.Merchant,
            RIMA.RoomType.Combat,
            RIMA.RoomType.Boss,
            RIMA.RoomType.Combat,   // post-boss dual-class arena
        };

        /// <summary>
        /// Builds a deterministic linear graph matching DemoSequence exactly.
        /// Each node has exactly ONE child, except the final post-boss Combat node (0 children).
        /// Seed is ignored — the sequence is always identical.
        /// </summary>
        public static DungeonGraph BuildDemoSequence()
        {
            var graph = new DungeonGraph
            {
                startId = 0,
                maxDepth = DemoSequence.Length - 1,
            };

            for (int i = 0; i < DemoSequence.Length; i++)
            {
                var node = new DungeonNode
                {
                    id = i,
                    depth = i,
                    roomType = DemoSequence[i],
                };
                graph.nodes.Add(node);
            }

            // Wire up: each non-terminal node points to the next one.
            for (int i = 0; i < DemoSequence.Length - 1; i++)
            {
                graph.nodes[i].childIds.Add(i + 1);
            }
            // Post-boss Combat node (last) has no children — terminal.

            return graph;
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
                    };
                    graph.nodes.Add(node);
                    row.Add(node);
                }

                rows.Add(row);
            }

            // RIMA-canon node mix (RUNMAP_BRANCHING_DESIGN_2026-06-16 + critic binding fixes):
            // depth 0 = Combat (entry), last depth = Boss; mid-depths roll the weighted mix
            // with a no-consecutive-Elite fairness guard, then a post-pass guarantees Merchant.
            AssignRoomTypes(random, rows, depthCount);

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

        // Mid-depth weighted node mix (RUNMAP_BRANCHING_DESIGN_2026-06-16):
        //   Combat ~50% · Elite ~20% (no two consecutive Elites in generation order) ·
        //   Chest ~15% · Merchant ~15% — plus a guaranteed-Merchant post-pass (>=1, mid-depth).
        // Event / Forge / Curse / Rest are POST-DEMO (kept out of the mix).
        private static void AssignRoomTypes(Random random, List<List<DungeonNode>> rows, int depthCount)
        {
            bool prevWasElite = false;
            bool hasMerchant = false;
            int lastMidDepth = depthCount - 2; // last branching depth before the boss

            for (int depth = 0; depth < rows.Count; depth++)
            {
                List<DungeonNode> row = rows[depth];
                for (int i = 0; i < row.Count; i++)
                {
                    RIMA.RoomType type = RollRoomType(random, depth, depthCount, prevWasElite);
                    row[i].roomType = type;
                    prevWasElite = type == RIMA.RoomType.Elite;
                    if (type == RIMA.RoomType.Merchant)
                    {
                        hasMerchant = true;
                    }
                }
            }

            // Guaranteed Merchant (>=1, mid-depth). If the rolls produced none, deterministically
            // convert a non-Elite mid-depth node to Merchant so the demo never loses the shop beat.
            if (!hasMerchant)
            {
                ForceOneMerchant(random, rows, depthCount, lastMidDepth);
            }
        }

        private static RIMA.RoomType RollRoomType(Random random, int depth, int depthCount, bool prevWasElite)
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

            // Combat ~50%
            if (roll < 50)
            {
                return RIMA.RoomType.Combat;
            }

            // Elite ~20% — fairness guard: never two consecutive Elites (in generation order).
            if (roll < 70)
            {
                return prevWasElite ? RIMA.RoomType.Combat : RIMA.RoomType.Elite;
            }

            // Chest ~15%
            if (roll < 85)
            {
                return RIMA.RoomType.Chest;
            }

            // Merchant ~15%
            return RIMA.RoomType.Merchant;
        }

        // Deterministic fallback that turns one mid-depth node into a Merchant when the weighted
        // rolls produced none. Prefers a non-Elite node (so it does not also break the Elite mix);
        // walks mid-depths from the latest toward the earliest so the shop sits late (canon).
        private static void ForceOneMerchant(Random random, List<List<DungeonNode>> rows, int depthCount, int lastMidDepth)
        {
            for (int depth = lastMidDepth; depth >= 1; depth--)
            {
                if (depth >= rows.Count)
                {
                    continue;
                }

                List<DungeonNode> row = rows[depth];
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].roomType != RIMA.RoomType.Elite && row[i].roomType != RIMA.RoomType.Boss)
                    {
                        row[i].roomType = RIMA.RoomType.Merchant;
                        return;
                    }
                }
            }

            // Last resort (e.g. all mid nodes are Elite): convert the first mid node anyway.
            for (int depth = 1; depth <= lastMidDepth && depth < rows.Count; depth++)
            {
                List<DungeonNode> row = rows[depth];
                if (row.Count > 0)
                {
                    row[0].roomType = RIMA.RoomType.Merchant;
                    return;
                }
            }
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
