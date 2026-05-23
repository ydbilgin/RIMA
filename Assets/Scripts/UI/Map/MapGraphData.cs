using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.UI.Map
{
    [Serializable]
    public class MapGraphData
    {
        public List<MapNodeData> nodes = new List<MapNodeData>();

        public IReadOnlyList<MapNodeData> Nodes => nodes;

        public MapNodeData GetNode(int id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].id == id)
                    return nodes[i];
            }

            return null;
        }

        public bool HasNode(int id)
        {
            return GetNode(id) != null;
        }

        public static MapGraphData CreateFiveNodePlaceholder()
        {
            var graph = new MapGraphData();

            var entry = new MapNodeData(0, "Entry", MapNodeType.Entry, new Vector2(0f, -220f))
            {
                isVisited = true
            };
            entry.connections.Add(1);

            var combat = new MapNodeData(1, "Combat", MapNodeType.Combat, new Vector2(-120f, -90f))
            {
                isCurrentRoom = true,
                threatTier = 1
            };
            combat.connections.Add(2);
            combat.connections.Add(3);

            var eventNode = new MapNodeData(2, "Mystery", MapNodeType.Mystery, new Vector2(-190f, 55f))
            {
                threatTier = 1
            };
            eventNode.connections.Add(4);

            var rest = new MapNodeData(3, "Rest", MapNodeType.Rest, new Vector2(80f, 60f));
            rest.connections.Add(4);

            var boss = new MapNodeData(4, "Boss", MapNodeType.Boss, new Vector2(0f, 220f))
            {
                threatTier = 3
            };

            graph.nodes.Add(entry);
            graph.nodes.Add(combat);
            graph.nodes.Add(eventNode);
            graph.nodes.Add(rest);
            graph.nodes.Add(boss);
            return graph;
        }
    }
}
