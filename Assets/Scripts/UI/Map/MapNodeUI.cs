using UnityEngine;
using UnityEngine.UI;

namespace RIMA.UI.Map
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class MapNodeUI : MonoBehaviour
    {
        [SerializeField] private Image nodeImage;
        [SerializeField] private Image ringImage;
        [SerializeField] private Text label;

        private MapNodeData data;

        private void Awake()
        {
            EnsureReferences();
        }

        public void Initialize(MapNodeData node)
        {
            data = node;
            EnsureReferences();
            Refresh();
        }

        public void Refresh()
        {
            if (data == null)
                return;

            Color nodeColor = GetColor(data.nodeType);
            if (!data.isRevealed)
                nodeColor = new Color(0.12f, 0.12f, 0.14f, 0.9f);
            else if (data.isVisited)
                nodeColor = new Color(nodeColor.r * 0.45f, nodeColor.g * 0.45f, nodeColor.b * 0.45f, 0.85f);

            nodeImage.color = nodeColor;
            nodeImage.raycastTarget = true;

            if (ringImage != null)
            {
                ringImage.enabled = data.isCurrentRoom;
                ringImage.color = new Color(0.26f, 1f, 0.8f, 0.95f);
            }

            if (label != null)
            {
                label.text = GetLabel(data);
                label.color = data.isRevealed ? Color.white : new Color(0.65f, 0.7f, 0.72f, 1f);
            }
        }

        private void EnsureReferences()
        {
            if (nodeImage == null)
                nodeImage = GetComponent<Image>();

            if (ringImage == null)
                ringImage = transform.Find("CurrentRing")?.GetComponent<Image>();

            if (label == null)
                label = GetComponentInChildren<Text>(true);

            if (label == null)
                label = CreateLabel();
        }

        private Text CreateLabel()
        {
            var labelObject = new GameObject("Label", typeof(RectTransform));
            labelObject.transform.SetParent(transform, false);

            var labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            var text = labelObject.AddComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 18;
            text.fontStyle = FontStyle.Bold;
            text.raycastTarget = false;
            return text;
        }

        public static Color GetColor(MapNodeType type)
        {
            switch (type)
            {
                case MapNodeType.Elite:
                    return new Color(0.95f, 0.45f, 0.18f, 1f);
                case MapNodeType.Rest:
                    return new Color(0.25f, 0.78f, 0.45f, 1f);
                case MapNodeType.Boss:
                    return new Color(0.58f, 0.2f, 0.85f, 1f);
                case MapNodeType.Event:
                case MapNodeType.Mystery:
                    return new Color(0.42f, 0.72f, 0.95f, 1f);
                case MapNodeType.Shop:
                    return new Color(0.95f, 0.72f, 0.2f, 1f);
                case MapNodeType.CurseGate:
                    return new Color(0.66f, 0.18f, 0.24f, 1f);
                case MapNodeType.Entry:
                    return new Color(0.58f, 0.6f, 0.62f, 1f);
                default:
                    return new Color(0.82f, 0.18f, 0.14f, 1f);
            }
        }

        private static string GetLabel(MapNodeData node)
        {
            if (!node.isRevealed)
                return "?";

            if (node.isCurrentRoom)
                return "NOW";

            switch (node.nodeType)
            {
                case MapNodeType.Elite:
                    return "EL";
                case MapNodeType.Rest:
                    return "R";
                case MapNodeType.Boss:
                    return "B";
                case MapNodeType.Event:
                    return "!";
                case MapNodeType.Shop:
                    return "$";
                case MapNodeType.CurseGate:
                    return "CG";
                case MapNodeType.Mystery:
                    return "?";
                case MapNodeType.Entry:
                    return "IN";
                default:
                    return "C";
            }
        }
    }
}
