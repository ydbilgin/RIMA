using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Runtime
{
    // Press M to toggle a Slay-the-Spire-style overview of the current run's logical path.
    // Demo-grade IMGUI overlay so it works regardless of the active Input System package.
    public sealed class RunMapOverlay : MonoBehaviour
    {
        [SerializeField] private RoomRunDirector director;
        [SerializeField] private bool show;
        [SerializeField] private KeyCode toggleKey = KeyCode.M;

        private Texture2D pixel;
        private GUIStyle nodeStyle;
        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;

        public void Toggle() => show = !show;

        private void Awake()
        {
            pixel = new Texture2D(1, 1);
            pixel.SetPixel(0, 0, Color.white);
            pixel.Apply();
        }

        private void OnGUI()
        {
            Event e = Event.current;
            if (e != null && e.type == EventType.KeyDown && e.keyCode == toggleKey)
            {
                show = !show;
                e.Use();
            }

            if (!show || director == null)
            {
                return;
            }

            DungeonGraph graph = director.Graph;
            if (graph == null || graph.nodes == null || graph.nodes.Count == 0)
            {
                return;
            }

            EnsureStyles();

            DrawRect(new Rect(0, 0, Screen.width, Screen.height), new Color(0.03f, 0.02f, 0.06f, 0.88f));
            GUI.Label(new Rect(0, 26, Screen.width, 44), "RUN PATH", titleStyle);
            GUI.Label(new Rect(0, 72, Screen.width, 24), "M ile kapat   -   cyan = bulundugun oda", subtitleStyle);

            const float nodeW = 172f;
            const float nodeH = 42f;
            const float rowGap = 34f;
            const float minTop = 118f;
            const float bottomPadding = 44f;

            int depthCount = graph.maxDepth + 1;
            float totalH = depthCount * nodeH + (depthCount - 1) * rowGap;
            float bottomY = Mathf.Min(Screen.height - bottomPadding, minTop + totalH);
            var rects = new Dictionary<int, Rect>();

            for (int depth = 0; depth <= graph.maxDepth; depth++)
            {
                List<DungeonNode> row = graph.NodesAtDepth(depth);
                if (row.Count == 0)
                {
                    continue;
                }

                float gap = Mathf.Min(30f, Mathf.Max(12f, (Screen.width - row.Count * nodeW) / (row.Count + 1f)));
                float totalW = row.Count * nodeW + (row.Count - 1) * gap;
                float left = (Screen.width - totalW) * 0.5f;
                float y = bottomY - (depth + 1) * nodeH - depth * rowGap;

                for (int i = 0; i < row.Count; i++)
                {
                    rects[row[i].id] = new Rect(left + i * (nodeW + gap), y, nodeW, nodeH);
                }
            }

            Color lineColor = new Color(0.35f, 0.9f, 0.82f, 0.42f);
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                DungeonNode parent = graph.nodes[i];
                if (!rects.TryGetValue(parent.id, out Rect parentRect))
                {
                    continue;
                }

                for (int c = 0; c < parent.childIds.Count; c++)
                {
                    if (rects.TryGetValue(parent.childIds[c], out Rect childRect))
                    {
                        DrawLine(
                            new Vector2(parentRect.center.x, parentRect.y),
                            new Vector2(childRect.center.x, childRect.yMax),
                            lineColor,
                            2f);
                    }
                }
            }

            for (int i = 0; i < graph.nodes.Count; i++)
            {
                DungeonNode node = graph.nodes[i];
                if (!rects.TryGetValue(node.id, out Rect rect))
                {
                    continue;
                }

                bool isCurrent = node.id == director.CurrentNodeId;
                Color color = ColorFor(node.roomType);
                if (isCurrent)
                {
                    color = Color.Lerp(color, Color.white, 0.18f);
                }

                DrawRect(rect, color);
                DrawBorder(rect, new Color(0.02f, 0.03f, 0.05f, 1f), 1f);
                if (isCurrent)
                {
                    DrawBorder(rect, new Color(0f, 1f, 0.8f, 1f), 3f);
                }

                GUI.Label(rect, $"{node.id}: {node.roomType}", nodeStyle);
            }
        }

        private static Color ColorFor(RIMA.RoomType type)
        {
            switch (type)
            {
                case RIMA.RoomType.Combat: return new Color(0.32f, 0.38f, 0.44f, 0.95f);
                case RIMA.RoomType.Elite: return new Color(0.48f, 0.28f, 0.60f, 0.95f);
                case RIMA.RoomType.Boss: return new Color(0.62f, 0.24f, 0.24f, 0.95f);
                case RIMA.RoomType.Chest: return new Color(0.68f, 0.58f, 0.24f, 0.95f);
                case RIMA.RoomType.Merchant: return new Color(0.22f, 0.52f, 0.50f, 0.95f);
                case RIMA.RoomType.Event: return new Color(0.25f, 0.43f, 0.62f, 0.95f);
                default: return new Color(0.30f, 0.34f, 0.40f, 0.95f);
            }
        }

        private void DrawLine(Vector2 from, Vector2 to, Color color, float thickness)
        {
            int steps = Mathf.Max(1, Mathf.CeilToInt(Vector2.Distance(from, to) / 5f));
            for (int i = 0; i <= steps; i++)
            {
                Vector2 p = Vector2.Lerp(from, to, i / (float)steps);
                DrawRect(new Rect(p.x - thickness * 0.5f, p.y - thickness * 0.5f, thickness, thickness), color);
            }
        }

        private void DrawRect(Rect r, Color color)
        {
            Color prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(r, pixel);
            GUI.color = prev;
        }

        private void DrawBorder(Rect r, Color color, float t)
        {
            DrawRect(new Rect(r.x, r.y, r.width, t), color);
            DrawRect(new Rect(r.x, r.yMax - t, r.width, t), color);
            DrawRect(new Rect(r.x, r.y, t, r.height), color);
            DrawRect(new Rect(r.xMax - t, r.y, t, r.height), color);
        }

        private void EnsureStyles()
        {
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 30,
                    fontStyle = FontStyle.Bold
                };
                titleStyle.normal.textColor = new Color(0.6f, 1f, 0.92f, 1f);
            }

            if (subtitleStyle == null)
            {
                subtitleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 14
                };
                subtitleStyle.normal.textColor = new Color(0.7f, 0.78f, 0.85f, 1f);
            }

            if (nodeStyle == null)
            {
                nodeStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 14,
                    fontStyle = FontStyle.Bold
                };
                nodeStyle.normal.textColor = Color.white;
            }
        }
    }
}
