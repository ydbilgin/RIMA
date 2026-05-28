#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LaurethStudio.PainterSuite.Editor.Colliders
{
    public enum ShapeMode { Box, Circle, Polygon, Edge }

    /// <summary>
    /// Drag/click-to-create Collider2D painter for SceneView.
    /// Supports Box (drag), Circle (drag radius), Polygon (click vertices + double-click close), Edge (click vertices).
    /// </summary>
    /// <remarks>
    /// v0.3.0. Pattern adapted from RIMA VisualEditorScenePainter (mouse handle +
    /// ghost preview + undo group) but RIMA-decoupled.
    /// </remarks>
    public sealed class ColliderPainter
    {
        public GameObject Target { get; set; }
        public ShapeMode Mode { get; set; } = ShapeMode.Box;
        public bool SnapToPixel { get; set; } = true;
        public int PixelsPerUnit { get; set; } = 64;
        public bool DrawExisting { get; set; } = true;

        // Drag state (Box / Circle)
        private bool _isDragging;
        private Vector2 _dragStartWorld;
        private Vector2 _dragCurrentWorld;
        private int _undoGroup;

        // Polygon / Edge in-progress state
        private readonly List<Vector2> _polygonVerts = new List<Vector2>();
        private Vector2 _polygonHover;

        private static readonly Color GhostFill = new Color(0.4f, 0.9f, 1f, 0.15f);
        private static readonly Color GhostEdge = new Color(0.4f, 0.9f, 1f, 0.85f);
        private static readonly Color ExistingFill = new Color(0.4f, 1f, 0.4f, 0.05f);
        private static readonly Color ExistingEdge = new Color(0.4f, 1f, 0.4f, 0.85f);

        public void Reset()
        {
            _isDragging = false;
            _polygonVerts.Clear();
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (Target == null) return;

            Event e = Event.current;
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            Vector2 worldPos = MouseWorld(e);
            if (SnapToPixel) worldPos = Snap(worldPos);

            switch (Mode)
            {
                case ShapeMode.Box: HandleBox(sceneView, e, worldPos); break;
                case ShapeMode.Circle: HandleCircle(sceneView, e, worldPos); break;
                case ShapeMode.Polygon: HandlePolygonOrEdge(sceneView, e, worldPos, true); break;
                case ShapeMode.Edge: HandlePolygonOrEdge(sceneView, e, worldPos, false); break;
            }

            if (e.type == EventType.Repaint && DrawExisting) DrawExistingColliders();
        }

        // -------- BOX --------

        private void HandleBox(SceneView sceneView, Event e, Vector2 worldPos)
        {
            switch (e.type)
            {
                case EventType.MouseDown when e.button == 0:
                    StartDrag(worldPos, "Paint BoxCollider2D");
                    e.Use();
                    break;
                case EventType.MouseDrag when _isDragging:
                    _dragCurrentWorld = worldPos;
                    sceneView.Repaint();
                    e.Use();
                    break;
                case EventType.MouseUp when _isDragging && e.button == 0:
                    _isDragging = false;
                    _dragCurrentWorld = worldPos;
                    TryCreateBox(_dragStartWorld, _dragCurrentWorld);
                    Undo.CollapseUndoOperations(_undoGroup);
                    e.Use();
                    sceneView.Repaint();
                    break;
                case EventType.Repaint:
                    if (_isDragging) DrawBoxGhost(_dragStartWorld, _dragCurrentWorld);
                    break;
            }
        }

        private void TryCreateBox(Vector2 a, Vector2 b)
        {
            Vector2 size = new Vector2(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y));
            if (size.x < 0.05f || size.y < 0.05f) return;
            Vector2 worldCenter = (a + b) * 0.5f;
            Vector2 localCenter = Target.transform.InverseTransformPoint(worldCenter);
            var col = Undo.AddComponent<BoxCollider2D>(Target);
            col.size = size;
            col.offset = localCenter;
            EditorUtility.SetDirty(Target);
        }

        private static void DrawBoxGhost(Vector2 a, Vector2 b)
        {
            Vector3[] quad =
            {
                new Vector3(a.x, a.y, 0), new Vector3(b.x, a.y, 0),
                new Vector3(b.x, b.y, 0), new Vector3(a.x, b.y, 0)
            };
            Handles.DrawSolidRectangleWithOutline(quad, GhostFill, GhostEdge);
            Vector2 sz = new Vector2(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y));
            Vector3 mid = new Vector3((a.x + b.x) * 0.5f, Mathf.Max(a.y, b.y) + 0.18f, 0);
            Handles.Label(mid, $"{sz.x:F2} x {sz.y:F2}");
        }

        // -------- CIRCLE --------

        private void HandleCircle(SceneView sceneView, Event e, Vector2 worldPos)
        {
            switch (e.type)
            {
                case EventType.MouseDown when e.button == 0:
                    StartDrag(worldPos, "Paint CircleCollider2D");
                    e.Use();
                    break;
                case EventType.MouseDrag when _isDragging:
                    _dragCurrentWorld = worldPos;
                    sceneView.Repaint();
                    e.Use();
                    break;
                case EventType.MouseUp when _isDragging && e.button == 0:
                    _isDragging = false;
                    _dragCurrentWorld = worldPos;
                    TryCreateCircle(_dragStartWorld, _dragCurrentWorld);
                    Undo.CollapseUndoOperations(_undoGroup);
                    e.Use();
                    sceneView.Repaint();
                    break;
                case EventType.Repaint:
                    if (_isDragging) DrawCircleGhost(_dragStartWorld, _dragCurrentWorld);
                    break;
            }
        }

        private void TryCreateCircle(Vector2 center, Vector2 edge)
        {
            float r = Vector2.Distance(center, edge);
            if (r < 0.05f) return;
            Vector2 localCenter = Target.transform.InverseTransformPoint(center);
            var col = Undo.AddComponent<CircleCollider2D>(Target);
            col.radius = r;
            col.offset = localCenter;
            EditorUtility.SetDirty(Target);
        }

        private static void DrawCircleGhost(Vector2 center, Vector2 edge)
        {
            float r = Vector2.Distance(center, edge);
            Handles.color = GhostEdge;
            Handles.DrawWireDisc(new Vector3(center.x, center.y, 0), Vector3.forward, r);
            Handles.Label(new Vector3(center.x, center.y + r + 0.15f, 0), $"r={r:F2}");
        }

        // -------- POLYGON / EDGE --------

        private void HandlePolygonOrEdge(SceneView sceneView, Event e, Vector2 worldPos, bool closes)
        {
            _polygonHover = worldPos;

            switch (e.type)
            {
                case EventType.MouseDown when e.button == 0:
                    if (e.clickCount >= 2 && _polygonVerts.Count >= 2)
                    {
                        FinalizePolygonOrEdge(closes);
                        e.Use();
                        sceneView.Repaint();
                        return;
                    }
                    if (_polygonVerts.Count == 0)
                    {
                        Undo.IncrementCurrentGroup();
                        _undoGroup = Undo.GetCurrentGroup();
                        Undo.SetCurrentGroupName(closes ? "Paint PolygonCollider2D" : "Paint EdgeCollider2D");
                    }
                    _polygonVerts.Add(worldPos);
                    e.Use();
                    sceneView.Repaint();
                    break;

                case EventType.MouseDown when e.button == 1: // RMB cancel
                    if (_polygonVerts.Count > 0)
                    {
                        _polygonVerts.Clear();
                        e.Use();
                        sceneView.Repaint();
                    }
                    break;

                case EventType.MouseMove:
                case EventType.MouseDrag:
                    if (_polygonVerts.Count > 0) sceneView.Repaint();
                    break;

                case EventType.KeyDown when e.keyCode == KeyCode.Return && _polygonVerts.Count >= 2:
                    FinalizePolygonOrEdge(closes);
                    e.Use();
                    sceneView.Repaint();
                    break;

                case EventType.Repaint:
                    DrawPolygonInProgress(closes);
                    break;
            }
        }

        private void FinalizePolygonOrEdge(bool closes)
        {
            if (Target == null || _polygonVerts.Count < 2) { _polygonVerts.Clear(); return; }

            var local = new Vector2[_polygonVerts.Count];
            for (int i = 0; i < _polygonVerts.Count; i++)
            {
                local[i] = Target.transform.InverseTransformPoint(_polygonVerts[i]);
            }

            if (closes)
            {
                var col = Undo.AddComponent<PolygonCollider2D>(Target);
                col.pathCount = 1;
                col.SetPath(0, local);
            }
            else
            {
                var col = Undo.AddComponent<EdgeCollider2D>(Target);
                col.points = local;
            }
            Undo.CollapseUndoOperations(_undoGroup);
            EditorUtility.SetDirty(Target);
            _polygonVerts.Clear();
        }

        private void DrawPolygonInProgress(bool closes)
        {
            if (_polygonVerts.Count == 0) return;

            Handles.color = GhostEdge;
            for (int i = 0; i < _polygonVerts.Count; i++)
            {
                var v = _polygonVerts[i];
                Handles.SphereHandleCap(0, new Vector3(v.x, v.y, 0), Quaternion.identity, 0.08f, EventType.Repaint);
                if (i > 0)
                {
                    var p = _polygonVerts[i - 1];
                    Handles.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(v.x, v.y, 0));
                }
            }
            // Hover preview line from last vertex
            var last = _polygonVerts[_polygonVerts.Count - 1];
            Handles.color = new Color(GhostEdge.r, GhostEdge.g, GhostEdge.b, 0.4f);
            Handles.DrawDottedLine(new Vector3(last.x, last.y, 0), new Vector3(_polygonHover.x, _polygonHover.y, 0), 3f);
            if (closes && _polygonVerts.Count >= 2)
            {
                var first = _polygonVerts[0];
                Handles.DrawDottedLine(new Vector3(_polygonHover.x, _polygonHover.y, 0), new Vector3(first.x, first.y, 0), 3f);
            }

            string hint = closes
                ? $"Polygon: {_polygonVerts.Count} pts (double-click or Enter to close, RMB cancel)"
                : $"Edge: {_polygonVerts.Count} pts (double-click or Enter to finish, RMB cancel)";
            Handles.Label(new Vector3(_polygonHover.x + 0.2f, _polygonHover.y + 0.2f, 0), hint);
        }

        // -------- EXISTING DRAW --------

        private void DrawExistingColliders()
        {
            if (Target == null) return;
            // Box
            foreach (var col in Target.GetComponents<BoxCollider2D>())
            {
                Vector2 wc = (Vector2)Target.transform.TransformPoint(col.offset);
                Vector2 s = col.size;
                Vector3[] q =
                {
                    new Vector3(wc.x - s.x * 0.5f, wc.y - s.y * 0.5f, 0),
                    new Vector3(wc.x + s.x * 0.5f, wc.y - s.y * 0.5f, 0),
                    new Vector3(wc.x + s.x * 0.5f, wc.y + s.y * 0.5f, 0),
                    new Vector3(wc.x - s.x * 0.5f, wc.y + s.y * 0.5f, 0)
                };
                Handles.DrawSolidRectangleWithOutline(q, ExistingFill, ExistingEdge);
            }
            // Circle
            Handles.color = ExistingEdge;
            foreach (var col in Target.GetComponents<CircleCollider2D>())
            {
                Vector2 wc = (Vector2)Target.transform.TransformPoint(col.offset);
                Handles.DrawWireDisc(new Vector3(wc.x, wc.y, 0), Vector3.forward, col.radius);
            }
            // Polygon
            foreach (var col in Target.GetComponents<PolygonCollider2D>())
            {
                for (int p = 0; p < col.pathCount; p++)
                {
                    Vector2[] path = col.GetPath(p);
                    DrawClosedPath(path, col);
                }
            }
            // Edge
            foreach (var col in Target.GetComponents<EdgeCollider2D>())
            {
                Vector2[] path = col.points;
                DrawOpenPath(path, col);
            }
        }

        private void DrawClosedPath(Vector2[] path, Collider2D col)
        {
            if (path == null || path.Length < 2) return;
            for (int i = 0; i < path.Length; i++)
            {
                Vector2 a = (Vector2)Target.transform.TransformPoint(path[i] + col.offset);
                Vector2 b = (Vector2)Target.transform.TransformPoint(path[(i + 1) % path.Length] + col.offset);
                Handles.DrawLine(new Vector3(a.x, a.y, 0), new Vector3(b.x, b.y, 0));
            }
        }

        private void DrawOpenPath(Vector2[] path, Collider2D col)
        {
            if (path == null || path.Length < 2) return;
            for (int i = 0; i < path.Length - 1; i++)
            {
                Vector2 a = (Vector2)Target.transform.TransformPoint(path[i] + col.offset);
                Vector2 b = (Vector2)Target.transform.TransformPoint(path[i + 1] + col.offset);
                Handles.DrawLine(new Vector3(a.x, a.y, 0), new Vector3(b.x, b.y, 0));
            }
        }

        // -------- UTIL --------

        private void StartDrag(Vector2 worldPos, string undoName)
        {
            _isDragging = true;
            _dragStartWorld = worldPos;
            _dragCurrentWorld = worldPos;
            Undo.IncrementCurrentGroup();
            _undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName(undoName);
        }

        private static Vector2 MouseWorld(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Mathf.Abs(ray.direction.z) > Mathf.Epsilon)
            {
                float t = -ray.origin.z / ray.direction.z;
                Vector3 hit = ray.origin + ray.direction * t;
                return new Vector2(hit.x, hit.y);
            }
            return new Vector2(ray.origin.x, ray.origin.y);
        }

        private Vector2 Snap(Vector2 world)
        {
            float p = PixelsPerUnit;
            if (p <= 0) return world;
            return new Vector2(Mathf.Round(world.x * p) / p, Mathf.Round(world.y * p) / p);
        }
    }
}
#endif
