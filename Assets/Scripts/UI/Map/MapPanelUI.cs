using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA.UI.Map
{
    public class MapPanelUI : MonoBehaviour
    {
        [Header("Preview")]
        [SerializeField] private bool showPlaceholderOnAwake = true;
        [SerializeField] private MapGraphData placeholderGraph = MapGraphData.CreateFiveNodePlaceholder();

        [Header("Layout")]
        [SerializeField] private RectTransform tabletRoot;
        [SerializeField] private RectTransform connectionRoot;
        [SerializeField] private RectTransform nodeRoot;
        [SerializeField] private Vector2 tabletSize = new Vector2(900f, 650f);
        [SerializeField] private float nodeSize = 62f;
        [SerializeField] private float connectionThickness = 5f;

        [Header("Colors")]
        [SerializeField] private Color tabletColor = new Color(0.09f, 0.08f, 0.07f, 0.95f);
        [SerializeField] private Color tabletFrameColor = new Color(0.58f, 0.42f, 0.18f, 0.85f);
        [SerializeField] private Color connectionColor = new Color(0.15f, 0.95f, 0.82f, 0.58f);
        [SerializeField] private Color visitedConnectionColor = new Color(0.12f, 0.45f, 0.4f, 0.45f);

        private readonly Dictionary<int, MapNodeUI> nodeViews = new Dictionary<int, MapNodeUI>();
        private readonly List<MapConnectionUI> connectionViews = new List<MapConnectionUI>();

        private MapGraphData activeGraph;

        private void Awake()
        {
            EnsureRoots();

            if (showPlaceholderOnAwake)
                Show(placeholderGraph);
        }

        public void Show(MapGraphData graph)
        {
            EnsureRoots();

            activeGraph = graph ?? placeholderGraph ?? MapGraphData.CreateFiveNodePlaceholder();
            gameObject.SetActive(true);
            Rebuild();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Refresh()
        {
            if (!isActiveAndEnabled)
                return;

            Rebuild();
        }

        private void Rebuild()
        {
            Clear();

            if (activeGraph == null || activeGraph.nodes == null)
                return;

            for (int i = 0; i < activeGraph.nodes.Count; i++)
            {
                MapNodeData node = activeGraph.nodes[i];
                for (int j = 0; j < node.connections.Count; j++)
                {
                    MapNodeData target = activeGraph.GetNode(node.connections[j]);
                    if (target == null)
                        continue;

                    CreateConnection(node, target);
                }
            }

            for (int i = 0; i < activeGraph.nodes.Count; i++)
                CreateNode(activeGraph.nodes[i]);
        }

        private void CreateConnection(MapNodeData from, MapNodeData to)
        {
            var connectionObject = new GameObject($"Connection_{from.id}_{to.id}", typeof(RectTransform), typeof(Image), typeof(MapConnectionUI));
            connectionObject.transform.SetParent(connectionRoot, false);
            connectionObject.transform.SetAsFirstSibling();

            var view = connectionObject.GetComponent<MapConnectionUI>();
            Color color = from.isVisited && to.isVisited ? visitedConnectionColor : connectionColor;
            view.SetEndpoints(from.position, to.position, color, connectionThickness);
            connectionViews.Add(view);
        }

        private void CreateNode(MapNodeData node)
        {
            var nodeObject = new GameObject($"Node_{node.id}_{node.nodeType}", typeof(RectTransform), typeof(Image), typeof(MapNodeUI));
            nodeObject.transform.SetParent(nodeRoot, false);

            var rect = nodeObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(nodeSize, nodeSize);
            rect.anchoredPosition = node.position;

            var view = nodeObject.GetComponent<MapNodeUI>();
            view.Initialize(node);
            nodeViews[node.id] = view;
        }

        private void Clear()
        {
            for (int i = nodeRoot.childCount - 1; i >= 0; i--)
                DestroyChild(nodeRoot.GetChild(i).gameObject);

            for (int i = connectionRoot.childCount - 1; i >= 0; i--)
                DestroyChild(connectionRoot.GetChild(i).gameObject);

            nodeViews.Clear();
            connectionViews.Clear();
        }

        private static void DestroyChild(GameObject child)
        {
            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        private void EnsureRoots()
        {
            RectTransform root = GetComponent<RectTransform>();
            if (root == null)
                root = gameObject.AddComponent<RectTransform>();

            root.anchorMin = new Vector2(0.5f, 0.5f);
            root.anchorMax = new Vector2(0.5f, 0.5f);
            root.pivot = new Vector2(0.5f, 0.5f);
            root.sizeDelta = tabletSize;

            if (tabletRoot == null)
                tabletRoot = CreateRoot("BrokenStoneTablet", transform, tabletSize);

            if (tabletRoot.GetComponent<Image>() == null)
            {
                var tabletImage = tabletRoot.gameObject.AddComponent<Image>();
                tabletImage.color = tabletColor;
                tabletImage.raycastTarget = true;
            }

            if (connectionRoot == null)
                connectionRoot = CreateRoot("Connections", tabletRoot, tabletSize);

            if (nodeRoot == null)
                nodeRoot = CreateRoot("Nodes", tabletRoot, tabletSize);

            EnsureFrame("RustGoldFrameTop", new Vector2(0f, tabletSize.y * 0.5f - 10f), new Vector2(tabletSize.x, 20f));
            EnsureFrame("RustGoldFrameBottom", new Vector2(0f, -tabletSize.y * 0.5f + 10f), new Vector2(tabletSize.x, 20f));
            EnsureFrame("RustGoldFrameLeft", new Vector2(-tabletSize.x * 0.5f + 10f, 0f), new Vector2(20f, tabletSize.y));
            EnsureFrame("RustGoldFrameRight", new Vector2(tabletSize.x * 0.5f - 10f, 0f), new Vector2(20f, tabletSize.y));
        }

        private RectTransform CreateRoot(string rootName, Transform parent, Vector2 size)
        {
            var rootObject = new GameObject(rootName, typeof(RectTransform));
            rootObject.transform.SetParent(parent, false);

            var rect = rootObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;
            return rect;
        }

        private void EnsureFrame(string frameName, Vector2 position, Vector2 size)
        {
            Transform existing = tabletRoot.Find(frameName);
            if (existing != null)
                return;

            var frameObject = new GameObject(frameName, typeof(RectTransform), typeof(Image));
            frameObject.transform.SetParent(tabletRoot, false);
            frameObject.transform.SetAsFirstSibling();

            var rect = frameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;

            var image = frameObject.GetComponent<Image>();
            image.color = tabletFrameColor;
            image.raycastTarget = false;
        }
    }
}
