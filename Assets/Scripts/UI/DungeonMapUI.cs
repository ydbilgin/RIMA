using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// M tuşuyla açılan STS2-stilinde dungeon haritası.
    ///
    /// Görünürlük kuralları:
    ///   Visited (girilmiş):  soluk renk + tick
    ///   Current:             parlak beyaz + pulse
    ///   Step-1 (direkt çıkış): oda tipi rengi + ikon
    ///   Revealed by fragment: oda tipi rengi + ikon
    ///   Step-2 fog preview: gri + "?"
    ///   Ötesi: siyah fog paneli — kaç oda var görünmez
    /// </summary>
    public class DungeonMapUI : MonoBehaviour
    {
        public static DungeonMapUI Instance { get; private set; }

        [Header("Layout")]
        [SerializeField] private float nodeSpacingY = 90f;
        [SerializeField] private float nodeSpacingX = 120f;
        [SerializeField] private float nodeRadius   = 28f;

        [Header("Colors")]
        [SerializeField] private Color colorCombat    = new Color(0.55f, 0.55f, 0.60f, 1f);
        [SerializeField] private Color colorElite     = new Color(0.95f, 0.55f, 0.10f, 1f);
        [SerializeField] private Color colorBoss      = new Color(0.85f, 0.10f, 0.10f, 1f);
        [SerializeField] private Color colorChest     = new Color(0.95f, 0.78f, 0.10f, 1f);
        [SerializeField] private Color colorMerchant  = new Color(0.20f, 0.65f, 0.95f, 1f);
        [SerializeField] private Color colorEvent     = new Color(0.65f, 0.30f, 0.90f, 1f);
        [SerializeField] private Color colorForge     = new Color(0.45f, 0.35f, 0.20f, 1f);
        [SerializeField] private Color colorCurrent   = new Color(1.00f, 1.00f, 1.00f, 1f);
        [SerializeField] private Color colorLine      = new Color(0.45f, 0.45f, 0.50f, 0.7f);

        // M tuşu — serialize etme (Key enum value Unity versiyonuna göre kayıyor)
        private readonly Key toggleKey = Key.M;

        // Runtime
        private Canvas        mapCanvas;
        private RectTransform contentRoot;
        private bool          isOpen;
        private Font          uiFont;

        private readonly List<NodeView>   nodeViews  = new List<NodeView>();
        private readonly List<GameObject> drawnLines = new List<GameObject>();
        private GameObject                fogPanelGO;

        private class NodeView
        {
            public int           nodeId;
            public RectTransform rt;
            public Image         circle;
            public Text          label;
        }

        // ── Lifecycle ────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
            BuildCanvas();
        }

        private void Start()
        {
            uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            mapCanvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Keyboard.current == null)
            {
                Debug.LogWarning("[DungeonMapUI] Keyboard.current is NULL — Input System not initialized?");
                return;
            }
            if (Keyboard.current[toggleKey].wasPressedThisFrame)
            {
                Debug.Log("[DungeonMapUI] M pressed → Toggle()");
                Toggle();
            }

            if (isOpen)
                AnimateCurrent();
        }

        // ── Public ───────────────────────────────────────────────────

        public void Toggle()
        {
            isOpen = !isOpen;
            if (mapCanvas == null) { Debug.LogError("[DungeonMapUI] mapCanvas is NULL!"); return; }
            mapCanvas.gameObject.SetActive(isOpen);
            Debug.Log($"[DungeonMapUI] Map canvas → {(isOpen ? "OPEN" : "CLOSED")}");
            if (isOpen) Rebuild();
        }

        public void RefreshMap() { if (isOpen) Rebuild(); }

        // ── Build Canvas ─────────────────────────────────────────────

        private void BuildCanvas()
        {
            var go = new GameObject("MapOverlay");
            go.transform.SetParent(transform, false);
            mapCanvas = go.AddComponent<Canvas>();
            mapCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mapCanvas.sortingOrder = 50;
            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            go.AddComponent<GraphicRaycaster>();

            // Dim background
            var bgGO = new GameObject("BG");
            bgGO.transform.SetParent(go.transform, false);
            var bgImg = bgGO.AddComponent<Image>();
            bgImg.color = new Color(0f, 0f, 0f, 0.82f);
            bgImg.raycastTarget = true;
            var bgRT = bgGO.GetComponent<RectTransform>();
            bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = Vector2.zero; bgRT.offsetMax = Vector2.zero;

            // Title
            var titleGO = new GameObject("Title");
            titleGO.transform.SetParent(go.transform, false);
            var titleTxt = titleGO.AddComponent<Text>();
            titleTxt.text = "DUNGEON MAP  [M]";
            titleTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleTxt.fontSize = 20;
            titleTxt.color = new Color(0.8f, 0.8f, 0.8f, 0.6f);
            titleTxt.alignment = TextAnchor.UpperCenter;
            var titleRT = titleGO.GetComponent<RectTransform>();
            titleRT.anchorMin = new Vector2(0, 1); titleRT.anchorMax = new Vector2(1, 1);
            titleRT.pivot = new Vector2(0.5f, 1f);
            titleRT.anchoredPosition = new Vector2(0, -24);
            titleRT.sizeDelta = new Vector2(0, 40);

            // Content root
            var contentGO = new GameObject("Content");
            contentGO.transform.SetParent(go.transform, false);
            contentRoot = contentGO.AddComponent<RectTransform>();
            contentRoot.anchorMin = new Vector2(0.5f, 0.5f);
            contentRoot.anchorMax = new Vector2(0.5f, 0.5f);
            contentRoot.pivot = new Vector2(0.5f, 0.5f);
            contentRoot.anchoredPosition = new Vector2(0, -30);
            contentRoot.sizeDelta = new Vector2(800, 700);
        }

        // ── Rebuild (fog-of-war) ──────────────────────────────────────

        private void Rebuild()
        {
            // Clear previous
            foreach (var v in nodeViews) if (v.rt != null) Destroy(v.rt.gameObject);
            nodeViews.Clear();
            foreach (var go in drawnLines) if (go != null) Destroy(go);
            drawnLines.Clear();
            if (fogPanelGO != null) { Destroy(fogPanelGO); fogPanelGO = null; }

            if (DungeonGraph.Instance == null) return;

            var nodes     = DungeonGraph.Instance.AllNodes;
            var current   = DungeonGraph.Instance.CurrentNode;
            int currentId = current.id;

            int maxDepth = 0;
            foreach (var n in nodes) if (n.depth > maxDepth) maxDepth = n.depth;

            // Compute reachable sets from current position
            var step1 = new HashSet<int>(); // direct exits
            var step2 = new HashSet<int>(); // exits of exits
            foreach (var kvp in current.exits)
            {
                step1.Add(kvp.Value);
                foreach (var kvp2 in nodes[kvp.Value].exits)
                    step2.Add(kvp2.Value);
            }
            // step2 shouldn't include step1 or current (merge nodes can appear)
            step2.ExceptWith(step1);
            step2.Remove(currentId);

            // Missed paths: exits of visited nodes the player bypassed
            var missed = new HashSet<int>();
            foreach (var node in nodes)
            {
                if (!node.visited) continue;
                foreach (var kvp in node.exits)
                {
                    int exitId = kvp.Value;
                    if (!nodes[exitId].visited && exitId != currentId)
                        missed.Add(exitId);
                }
            }
            missed.ExceptWith(step1);
            missed.ExceptWith(step2);
            missed.Remove(currentId);

            // Draw connection lines (only between visible nodes)
            foreach (var node in nodes)
            {
                if (!IsVisible(node.id, currentId, step1, step2, missed, nodes)) continue;
                foreach (var kvp in node.exits)
                {
                    if (!IsVisible(kvp.Value, currentId, step1, step2, missed, nodes)) continue;
                    var lineCol = missed.Contains(node.id) || missed.Contains(kvp.Value)
                        ? new Color(colorLine.r * 0.4f, colorLine.g * 0.4f, colorLine.b * 0.4f, colorLine.a * 0.5f)
                        : colorLine;
                    DrawLine(GetNodePos(node, maxDepth), GetNodePos(nodes[kvp.Value], maxDepth), lineCol);
                }
            }

            // Draw nodes (ordered: lines → step2 → step1 → visited → current, via sibling order)
            foreach (var node in nodes)
            {
                if (!IsVisible(node.id, currentId, step1, step2, missed, nodes)) continue;
                bool isCurrent = node.id == currentId;
                bool isS1      = step1.Contains(node.id);
                bool isS2      = step2.Contains(node.id);
                bool isMissed  = missed.Contains(node.id);
                bool isRevealed = node.revealed;
                nodeViews.Add(CreateNodeView(node, isCurrent, isS1, isS2, isMissed, isRevealed, maxDepth));
            }

            // Fog panel covering everything beyond step2
            int lastVisible = current.depth + 2;
            foreach (var node in nodes)
                if (node.revealed || node.visited) lastVisible = Mathf.Max(lastVisible, node.depth);
            if (lastVisible < maxDepth)
                DrawFogPanel(lastVisible, maxDepth);
        }

        private static bool IsVisible(int id, int currentId, HashSet<int> step1, HashSet<int> step2,
                                       HashSet<int> missed, IReadOnlyList<RoomNode> nodes)
        {
            return id == currentId || nodes[id].visited || nodes[id].revealed || step1.Contains(id) || step2.Contains(id) || missed.Contains(id);
        }

        // ── Node View ────────────────────────────────────────────────

        private NodeView CreateNodeView(RoomNode node, bool isCurrent, bool isStep1, bool isStep2, bool isMissed, bool isRevealed, int maxDepth)
        {
            var go = new GameObject("Node_" + node.id);
            go.transform.SetParent(contentRoot, false);
            var rt = go.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(nodeRadius * 2, nodeRadius * 2);
            rt.anchoredPosition = GetNodePos(node, maxDepth);

            Color  bgColor;
            string labelText;
            Color  labelColor = Color.white;
            int    fontSize   = 14;

            if (isCurrent)
            {
                bgColor    = colorCurrent;
                labelText  = "●";
                labelColor = Color.black;
                fontSize   = 20;
            }
            else if (node.visited)
            {
                bgColor   = Dim(GetRoomColor(node.roomType));
                labelText = "✓";
            }
            else if (isStep1)
            {
                bgColor   = GetRoomColor(node.roomType);
                labelText = GetRoomSymbol(node.roomType);
            }
            else if (isRevealed)
            {
                bgColor   = Dim(GetRoomColor(node.roomType));
                labelText = GetRoomSymbol(node.roomType);
                labelColor = new Color(0.85f, 0.85f, 0.85f, 0.9f);
            }
            else if (isMissed)
            {
                // Seçilmemiş dallar — çok karanlık, simge soluk görünür
                bgColor    = new Color(0.10f, 0.08f, 0.10f, 0.80f);
                labelText  = GetRoomSymbol(node.roomType);
                labelColor = new Color(0.28f, 0.24f, 0.28f, 0.65f);
                fontSize   = 11;
            }
            else // step2 — "kaçıncı adım" belirsiz, sadece "?"
            {
                bgColor   = new Color(0.20f, 0.20f, 0.25f, 0.85f);
                labelText = "?";
                labelColor = new Color(0.6f, 0.6f, 0.6f, 0.8f);
            }

            var circle = go.AddComponent<Image>();
            circle.color = bgColor;
            circle.raycastTarget = false;

            // Border ring for current
            if (isCurrent)
            {
                var borderGO = new GameObject("Border");
                borderGO.transform.SetParent(go.transform, false);
                borderGO.transform.SetAsFirstSibling();
                var bRT = borderGO.AddComponent<RectTransform>();
                bRT.anchorMin = new Vector2(0.5f, 0.5f); bRT.anchorMax = new Vector2(0.5f, 0.5f);
                bRT.sizeDelta = new Vector2(nodeRadius * 2 + 10, nodeRadius * 2 + 10);
                bRT.anchoredPosition = Vector2.zero;
                var bImg = borderGO.AddComponent<Image>();
                bImg.color = new Color(1f, 1f, 0.5f, 0.8f);
                bImg.raycastTarget = false;
            }

            var labelGO = new GameObject("Label");
            labelGO.transform.SetParent(go.transform, false);
            var labelRT = labelGO.AddComponent<RectTransform>();
            labelRT.anchorMin = Vector2.zero; labelRT.anchorMax = Vector2.one;
            labelRT.offsetMin = Vector2.zero; labelRT.offsetMax = Vector2.zero;
            var txt = labelGO.AddComponent<Text>();
            txt.text      = labelText;
            txt.font      = uiFont ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize  = fontSize;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color     = labelColor;
            txt.raycastTarget = false;

            return new NodeView { nodeId = node.id, rt = rt, circle = circle, label = txt };
        }

        // ── Fog Panel ────────────────────────────────────────────────

        private void DrawFogPanel(int lastVisibleDepth, int maxDepth)
        {
            // Y coordinate of the top edge of the last visible layer
            float topY   = (lastVisibleDepth - maxDepth * 0.5f) * nodeSpacingY + nodeSpacingY * 0.55f;
            float bottomY = (maxDepth - maxDepth * 0.5f) * nodeSpacingY + nodeSpacingY * 0.55f;
            float height = bottomY - topY;
            if (height <= 5f) return;

            fogPanelGO = new GameObject("FogPanel");
            fogPanelGO.transform.SetParent(contentRoot, false);
            fogPanelGO.transform.SetAsFirstSibling();

            var rt = fogPanelGO.AddComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0f, topY + height * 0.5f);
            rt.sizeDelta = new Vector2(contentRoot.sizeDelta.x * 1.1f, height);

            var img = fogPanelGO.AddComponent<Image>();
            img.color = new Color(0.02f, 0.02f, 0.04f, 0.97f);
            img.raycastTarget = false;
        }

        // ── Lines ────────────────────────────────────────────────────

        private void DrawLine(Vector2 from, Vector2 to, Color col)
        {
            var go = new GameObject("Line");
            go.transform.SetParent(contentRoot, false);
            go.transform.SetAsFirstSibling();
            var rt  = go.AddComponent<RectTransform>();
            var img = go.AddComponent<Image>();
            img.color = col;
            img.raycastTarget = false;

            Vector2 dir    = to - from;
            float   length = dir.magnitude;
            float   angle  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            rt.anchoredPosition = (from + to) * 0.5f;
            rt.sizeDelta        = new Vector2(length, 3f);
            rt.localRotation    = Quaternion.Euler(0, 0, angle);

            drawnLines.Add(go);
        }

        // ── Helpers ──────────────────────────────────────────────────

        private Vector2 GetNodePos(RoomNode node, int maxDepth)
            => new Vector2(node.lane * nodeSpacingX, (node.depth - maxDepth * 0.5f) * nodeSpacingY);

        private Color GetRoomColor(RoomType type) => type switch
        {
            RoomType.Combat   => colorCombat,
            RoomType.Elite    => colorElite,
            RoomType.Boss     => colorBoss,
            RoomType.Chest    => colorChest,
            RoomType.Merchant => colorMerchant,
            RoomType.Event    => colorEvent,
            RoomType.Forge    => colorForge,
            _                 => colorCombat,
        };

        private string GetRoomSymbol(RoomType type) => type switch
        {
            RoomType.Combat   => "⚔",
            RoomType.Elite    => "☠",
            RoomType.Boss     => "★",
            RoomType.Chest    => "◆",
            RoomType.Merchant => "$",
            RoomType.Event    => "!",
            RoomType.Forge    => "▲",
            _                 => "·",
        };

        private static Color Dim(Color c) => new Color(c.r * 0.45f, c.g * 0.45f, c.b * 0.45f, 0.65f);

        // Pulse animation for current node
        private float pulseT;
        private void AnimateCurrent()
        {
            pulseT += Time.unscaledDeltaTime * 2.5f;
            float pulse = 0.7f + 0.3f * Mathf.Sin(pulseT);
            foreach (var v in nodeViews)
            {
                if (v.nodeId == DungeonGraph.Instance?.CurrentNode.id && v.circle != null)
                    v.circle.color = new Color(colorCurrent.r, colorCurrent.g, colorCurrent.b, pulse);
            }
        }
    }
}
