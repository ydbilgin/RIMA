using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal static class PreviewOverlayRenderer
    {
        private static readonly Color GhostFill = new Color(0.4f, 0.9f, 1f, 0.15f);
        private static readonly Color GhostEdge = new Color(0.4f, 0.9f, 1f, 0.85f);
        private static readonly Color TriggerEdge = new Color(1f, 0.85f, 0.25f, 0.85f);

        public static void DrawDepthUnderlay(
            Rect canvas,
            PreviewSpriteDrawInfo info,
            RoomPainterAsset asset,
            RoomLayer targetLayer,
            bool show3DMock)
        {
            if (!show3DMock || !info.IsValid)
            {
                return;
            }

            Handles.BeginGUI();
            DrawDropShadow(info, asset, targetLayer);
            Handles.EndGUI();
        }

        public static void DrawDepthOverlay(
            Rect canvas,
            PreviewSpriteDrawInfo info,
            RoomPainterAsset asset,
            RoomLayer targetLayer,
            bool show3DMock)
        {
            if (!show3DMock || !info.IsValid)
            {
                return;
            }

            RoomLayer layer = asset != null ? asset.defaultLayer : targetLayer;
            if (layer == RoomLayer.Cliff)
            {
                DrawCliffRamp(info.DrawRect);
            }
            else if (layer == RoomLayer.Parallax)
            {
                DrawParallaxTint(info.DrawRect);
            }
        }

        public static void DrawTopOverlays(Rect canvas, PreviewSpriteDrawInfo info, RoomPainterAsset asset)
        {
            if (!info.IsValid)
            {
                return;
            }

            Handles.BeginGUI();
            DrawYSortAxis(canvas, info, asset);
            DrawPivotCrosshair(info.PivotGui);
            DrawBoundingBox(info.DrawRect, asset);
            Handles.EndGUI();
        }

        private static void DrawDropShadow(PreviewSpriteDrawInfo info, RoomPainterAsset asset, RoomLayer targetLayer)
        {
            RoomLayer layer = asset != null ? asset.defaultLayer : targetLayer;
            if (layer == RoomLayer.Floor)
            {
                return;
            }

            Rect shadowRect = new Rect(
                info.PivotGui.x - info.DrawRect.width * 0.40f,
                info.PivotGui.y - info.DrawRect.height * 0.07f,
                info.DrawRect.width * 0.80f,
                Mathf.Max(8f, info.DrawRect.height * 0.25f));

            DrawEllipse(shadowRect, new Color(0f, 0f, 0f, 0.35f));
        }

        private static void DrawCliffRamp(Rect drawRect)
        {
            Rect rampRect = new Rect(drawRect.x, drawRect.y + drawRect.height * 0.60f, drawRect.width, drawRect.height * 0.40f);
            int steps = 12;
            for (int i = 0; i < steps; i++)
            {
                float t = (i + 1f) / steps;
                Color color = new Color(0f, 0f, 0f, Mathf.Lerp(0.03f, 0.35f, t));
                Rect stepRect = new Rect(rampRect.x, rampRect.y + rampRect.height * i / steps, rampRect.width, rampRect.height / steps + 1f);
                EditorGUI.DrawRect(stepRect, color);
            }
        }

        private static void DrawParallaxTint(Rect drawRect)
        {
            EditorGUI.DrawRect(drawRect, new Color(0.70f, 0.82f, 1f, 0.18f));
        }

        private static void DrawYSortAxis(Rect canvas, PreviewSpriteDrawInfo info, RoomPainterAsset asset)
        {
            if (asset == null || !asset.ySortEnabled || asset.ySortAxisOverride == YSortAxis.None)
            {
                return;
            }

            Color color = GhostEdge;
            if (asset.ySortAxisOverride == YSortAxis.Y)
            {
                color = new Color(1f, 0.55f, 0.20f, 0.95f);
            }
            else if (asset.ySortAxisOverride == YSortAxis.Custom || asset.ySortAxisOverride == YSortAxis.X)
            {
                color = new Color(1f, 0.25f, 0.9f, 0.95f);
            }

            Handles.color = color;
            Handles.DrawDottedLine(
                new Vector3(info.PivotGui.x, canvas.yMin, 0f),
                new Vector3(info.PivotGui.x, canvas.yMax, 0f),
                4f);
        }

        private static void DrawPivotCrosshair(Vector2 pivot)
        {
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(2f, new Vector3(pivot.x - 12f, pivot.y, 0f), new Vector3(pivot.x + 12f, pivot.y, 0f));
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(2f, new Vector3(pivot.x, pivot.y - 12f, 0f), new Vector3(pivot.x, pivot.y + 12f, 0f));
        }

        private static void DrawBoundingBox(Rect rect, RoomPainterAsset asset)
        {
            Color edge = asset != null && asset.isTrigger ? TriggerEdge : GhostEdge;
            Rect fillRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            Color previousColor = GUI.color;
            GUI.color = GhostFill;
            GUI.DrawTexture(fillRect, Texture2D.whiteTexture);
            GUI.color = previousColor;

            Handles.color = edge;
            Vector3 topLeft = new Vector3(rect.xMin, rect.yMin, 0f);
            Vector3 topRight = new Vector3(rect.xMax, rect.yMin, 0f);
            Vector3 bottomRight = new Vector3(rect.xMax, rect.yMax, 0f);
            Vector3 bottomLeft = new Vector3(rect.xMin, rect.yMax, 0f);

            Handles.DrawDottedLine(topLeft, topRight, 4f);
            Handles.DrawDottedLine(topRight, bottomRight, 4f);
            Handles.DrawDottedLine(bottomRight, bottomLeft, 4f);
            Handles.DrawDottedLine(bottomLeft, topLeft, 4f);
        }

        private static void DrawEllipse(Rect rect, Color color)
        {
            const int segments = 36;
            Vector3[] points = new Vector3[segments];
            Vector2 center = rect.center;
            float radiusX = rect.width * 0.5f;
            float radiusY = rect.height * 0.5f;

            for (int i = 0; i < segments; i++)
            {
                float angle = i / (float)segments * Mathf.PI * 2f;
                points[i] = new Vector3(center.x + Mathf.Cos(angle) * radiusX, center.y + Mathf.Sin(angle) * radiusY, 0f);
            }

            Handles.color = color;
            Handles.DrawAAConvexPolygon(points);
        }
    }
}
