using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// "Kemik Tablet" — flat grid minimap driven by DungeonGraph.
    /// 72×72px, top-right corner, 10px margin.
    /// Rooms as 8×6px rectangles, 2px connection lines, fog of war.
    /// </summary>
    public class MiniMap : MonoBehaviour
    {
        // ── Layout constants ─────────────────────────────────────────
        private const float MapSize     = 72f;
        private const float RoomW       = 8f;
        private const float RoomH       = 6f;
        private const float CellSpacingX = 14f;
        private const float CellSpacingY = 8f;
        private const float FrameThickness = 4f;
        private const float ConnectionThickness = 2f;

        // ── State ────────────────────────────────────────────────────
        private DungeonGraph graph;
        private readonly Dictionary<int, Image> roomImages = new Dictionary<int, Image>();
        private readonly List<Image> connections = new List<Image>();
        private RectTransform contentRoot;
        private Image frameImage;
        private Image currentGlow;
        private int lastNodeId = -1;
        private bool built;

        // Room type → symbol
        private static readonly Dictionary<RoomType, string> RoomSymbols = new Dictionary<RoomType, string>
        {
            { RoomType.Boss,     "\u2620" }, // skull
            { RoomType.Chest,    "\u25C6" }, // diamond
            { RoomType.Merchant, "$" },
            { RoomType.Elite,    "!" },
            { RoomType.Event,    "?" },
        };

        private void Start()
        {
            BuildFrame();
        }

        private void Update()
        {
            // Wait for DungeonGraph
            if (graph == null)
            {
                graph = DungeonGraph.Instance;
                if (graph == null) return;
            }

            if (!built)
            {
                BuildMap();
                built = true;
            }

            // Refresh when room changes
            if (graph.CurrentNodeId != lastNodeId)
            {
                lastNodeId = graph.CurrentNodeId;
                RefreshVisuals();
            }
        }

        // ─── Frame ──────────────────────────────────────────────────

        private void BuildFrame()
        {
            var root = GetComponent<RectTransform>();
            root.sizeDelta = new Vector2(MapSize, MapSize);

            // Stone frame
            frameImage = gameObject.GetComponent<Image>();
            if (frameImage == null) frameImage = gameObject.AddComponent<Image>();
            frameImage.color = RimaUITheme.MapFrame;
            frameImage.raycastTarget = false;

            // Content area (inside frame)
            var contentGo = new GameObject("MapContent", typeof(RectTransform));
            contentGo.transform.SetParent(root, false);
            contentRoot = contentGo.GetComponent<RectTransform>();
            contentRoot.anchorMin = Vector2.zero;
            contentRoot.anchorMax = Vector2.one;
            contentRoot.offsetMin = new Vector2(FrameThickness, FrameThickness);
            contentRoot.offsetMax = new Vector2(-FrameThickness, -FrameThickness);

            // Dark inner bg
            var innerBg = contentGo.AddComponent<Image>();
            innerBg.color = RimaUITheme.MapUnvisited;
            innerBg.raycastTarget = false;
        }

        // ─── Build Map ──────────────────────────────────────────────

        private void BuildMap()
        {
            if (graph == null || contentRoot == null) return;

            var allNodes = graph.AllNodes;
            if (allNodes.Count == 0) return;

            // Calculate bounds for centering
            int minDepth = int.MaxValue, maxDepth = int.MinValue;
            int minLane = int.MaxValue, maxLane = int.MinValue;
            for (int i = 0; i < allNodes.Count; i++)
            {
                var n = allNodes[i];
                if (n.depth < minDepth) minDepth = n.depth;
                if (n.depth > maxDepth) maxDepth = n.depth;
                if (n.lane < minLane)   minLane = n.lane;
                if (n.lane > maxLane)   maxLane = n.lane;
            }

            float contentW = contentRoot.rect.width > 0 ? contentRoot.rect.width : MapSize - FrameThickness * 2;
            float contentH = contentRoot.rect.height > 0 ? contentRoot.rect.height : MapSize - FrameThickness * 2;

            float depthRange = Mathf.Max(1, maxDepth - minDepth);
            float laneRange  = Mathf.Max(1, maxLane - minLane);

            // Build connections first (behind rooms)
            for (int i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                foreach (var exit in node.exits)
                {
                    if (exit.Value >= allNodes.Count) continue;
                    var target = allNodes[exit.Value];
                    BuildConnection(node, target, minDepth, depthRange, minLane, laneRange, contentW, contentH);
                }
            }

            // Build room rectangles
            for (int i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                BuildRoomRect(node, minDepth, depthRange, minLane, laneRange, contentW, contentH);
            }
        }

        private Vector2 NodePosition(RoomNode node, int minDepth, float depthRange, int minLane, float laneRange, float w, float h)
        {
            float nx = laneRange > 0 ? (node.lane - minLane) / laneRange : 0.5f;
            float ny = depthRange > 0 ? 1f - (node.depth - minDepth) / depthRange : 0.5f; // top = start
            float margin = 6f;
            float x = margin + nx * (w - 2 * margin);
            float y = margin + ny * (h - 2 * margin);
            return new Vector2(x, y);
        }

        private void BuildRoomRect(RoomNode node, int minDepth, float depthRange, int minLane, float laneRange, float w, float h)
        {
            var pos = NodePosition(node, minDepth, depthRange, minLane, laneRange, w, h);

            var go = new GameObject($"Room_{node.id}", typeof(RectTransform));
            go.transform.SetParent(contentRoot, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(RoomW, RoomH);
            rt.anchoredPosition = pos;

            var img = go.AddComponent<Image>();
            img.color = RimaUITheme.MapUnvisited;
            img.raycastTarget = false;

            roomImages[node.id] = img;

            // Symbol for special rooms
            if (RoomSymbols.TryGetValue(node.roomType, out string symbol))
            {
                var symGo = new GameObject("Sym", typeof(RectTransform));
                symGo.transform.SetParent(go.transform, false);
                var symRt = symGo.GetComponent<RectTransform>();
                symRt.anchorMin = Vector2.zero;
                symRt.anchorMax = Vector2.one;
                symRt.offsetMin = symRt.offsetMax = Vector2.zero;

                var tmp = symGo.AddComponent<TextMeshProUGUI>();
                tmp.text = symbol;
                tmp.fontSize = 5f;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.color = Color.white;
                tmp.alpha = 0f; // hidden until revealed
                tmp.raycastTarget = false;
            }
        }

        private void BuildConnection(RoomNode from, RoomNode to, int minDepth, float depthRange, int minLane, float laneRange, float w, float h)
        {
            var posA = NodePosition(from, minDepth, depthRange, minLane, laneRange, w, h);
            var posB = NodePosition(to, minDepth, depthRange, minLane, laneRange, w, h);

            var go = new GameObject("Conn", typeof(RectTransform));
            go.transform.SetParent(contentRoot, false);
            go.transform.SetAsFirstSibling(); // behind rooms

            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = Vector2.zero;
            rt.pivot = new Vector2(0f, 0.5f);

            Vector2 diff = posB - posA;
            float length = diff.magnitude;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            rt.sizeDelta = new Vector2(length, ConnectionThickness);
            rt.anchoredPosition = posA;
            rt.localRotation = Quaternion.Euler(0, 0, angle);

            var img = go.AddComponent<Image>();
            img.color = new Color(RimaUITheme.MapFrame.r, RimaUITheme.MapFrame.g, RimaUITheme.MapFrame.b, 0f); // hidden
            img.raycastTarget = false;

            connections.Add(img);
        }

        // ─── Refresh ────────────────────────────────────────────────

        private void RefreshVisuals()
        {
            if (graph == null) return;

            var allNodes = graph.AllNodes;

            // Determine visible rooms: current + visited + revealed (1-2 exits ahead)
            var visibleSet = new HashSet<int>();
            for (int i = 0; i < allNodes.Count; i++)
            {
                var n = allNodes[i];
                if (n.visited || n.revealed)
                    visibleSet.Add(n.id);
            }

            for (int i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                if (!roomImages.TryGetValue(node.id, out var img)) continue;

                if (!visibleSet.Contains(node.id))
                {
                    // Fog of war — invisible
                    img.color = Color.clear;
                    SetSymbolAlpha(img.transform, 0f);
                    continue;
                }

                // Determine room color
                Color c;
                if (node.id == graph.CurrentNodeId)
                    c = RimaUITheme.MapActive;
                else if (node.visited)
                    c = RimaUITheme.MapVisited;
                else if (node.roomType == RoomType.Boss)
                    c = RimaUITheme.MapBoss;
                else if (node.roomType == RoomType.Chest || node.roomType == RoomType.Merchant)
                    c = RimaUITheme.MapTreasure;
                else
                    c = RimaUITheme.MapUnvisited;

                img.color = c;
                SetSymbolAlpha(img.transform, node.revealed || node.visited ? 0.9f : 0f);
            }

            // Update connection visibility
            RefreshConnections(allNodes, visibleSet);
        }

        private void RefreshConnections(IReadOnlyList<RoomNode> allNodes, HashSet<int> visibleSet)
        {
            // Simple approach: show connection lines for all visible rooms
            int connIdx = 0;
            for (int i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                foreach (var exit in node.exits)
                {
                    if (connIdx >= connections.Count) return;
                    bool show = visibleSet.Contains(node.id) && visibleSet.Contains(exit.Value);
                    connections[connIdx].color = show
                        ? new Color(RimaUITheme.MapFrame.r, RimaUITheme.MapFrame.g, RimaUITheme.MapFrame.b, 0.6f)
                        : Color.clear;
                    connIdx++;
                }
            }
        }

        private static void SetSymbolAlpha(Transform roomTransform, float alpha)
        {
            var sym = roomTransform.Find("Sym");
            if (sym == null) return;
            var tmp = sym.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.alpha = alpha;
        }
    }
}
