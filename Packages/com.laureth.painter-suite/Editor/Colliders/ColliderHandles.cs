#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LaurethStudio.PainterSuite.Editor.Colliders
{
    /// <summary>
    /// Renders editable handles for Collider2D components on a target GameObject.
    /// Box: 4 corner resize + center move. Circle: 1 radius + center move.
    /// Polygon/Edge: vertex move (each point).
    /// </summary>
    /// <remarks>
    /// v0.4.0. Selected collider gets full handle set; others render outline only
    /// (drawn separately by ColliderPainter.DrawExistingColliders).
    /// </remarks>
    public static class ColliderHandles
    {
        private static readonly Color SelectedEdge = new Color(1f, 0.65f, 0.2f, 0.95f);
        private static readonly Color HandleFill = new Color(1f, 0.85f, 0.4f, 0.9f);

        public static void DrawAndEditSelected(GameObject target, int selectedIndex)
        {
            if (target == null || selectedIndex < 0) return;
            var all = target.GetComponents<Collider2D>();
            if (selectedIndex >= all.Length) return;

            var c = all[selectedIndex];
            if (c == null) return;

            switch (c)
            {
                case BoxCollider2D b: DrawBox(target, b); break;
                case CircleCollider2D circ: DrawCircle(target, circ); break;
                case PolygonCollider2D p: DrawPolygon(target, p); break;
                case EdgeCollider2D e: DrawEdge(target, e); break;
            }
        }

        private static void DrawBox(GameObject target, BoxCollider2D box)
        {
            Vector2 worldCenter = (Vector2)target.transform.TransformPoint(box.offset);
            Vector2 size = box.size;
            Vector2 half = size * 0.5f;

            // Compute world corners
            Vector3 bl = new Vector3(worldCenter.x - half.x, worldCenter.y - half.y, 0);
            Vector3 br = new Vector3(worldCenter.x + half.x, worldCenter.y - half.y, 0);
            Vector3 tr = new Vector3(worldCenter.x + half.x, worldCenter.y + half.y, 0);
            Vector3 tl = new Vector3(worldCenter.x - half.x, worldCenter.y + half.y, 0);

            Handles.color = SelectedEdge;
            Handles.DrawLine(bl, br); Handles.DrawLine(br, tr);
            Handles.DrawLine(tr, tl); Handles.DrawLine(tl, bl);

            float handleSize = HandleUtility.GetHandleSize(worldCenter) * 0.08f;

            // 4 corner handles -- drag changes size + offset (resize about opposite corner)
            EditorGUI.BeginChangeCheck();
            Vector3 nbl = Handles.FreeMoveHandle(bl, handleSize, Vector3.zero, Handles.RectangleHandleCap);
            Vector3 nbr = Handles.FreeMoveHandle(br, handleSize, Vector3.zero, Handles.RectangleHandleCap);
            Vector3 ntr = Handles.FreeMoveHandle(tr, handleSize, Vector3.zero, Handles.RectangleHandleCap);
            Vector3 ntl = Handles.FreeMoveHandle(tl, handleSize, Vector3.zero, Handles.RectangleHandleCap);
            // Center move
            Vector3 ncenter = Handles.PositionHandle(new Vector3(worldCenter.x, worldCenter.y, 0), Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(box, "Edit BoxCollider2D");
                // Center moved?
                Vector2 newWorldCenter = new Vector2(ncenter.x, ncenter.y);
                if (newWorldCenter != worldCenter)
                {
                    box.offset = (Vector2)target.transform.InverseTransformPoint(newWorldCenter);
                }
                else
                {
                    // Corner moved: compute new bounds
                    float minX = Mathf.Min(nbl.x, nbr.x, ntr.x, ntl.x);
                    float maxX = Mathf.Max(nbl.x, nbr.x, ntr.x, ntl.x);
                    float minY = Mathf.Min(nbl.y, nbr.y, ntr.y, ntl.y);
                    float maxY = Mathf.Max(nbl.y, nbr.y, ntr.y, ntl.y);
                    Vector2 newSize = new Vector2(maxX - minX, maxY - minY);
                    Vector2 newCenterWorld = new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);
                    box.size = new Vector2(Mathf.Max(0.05f, newSize.x), Mathf.Max(0.05f, newSize.y));
                    box.offset = (Vector2)target.transform.InverseTransformPoint(newCenterWorld);
                }
                EditorUtility.SetDirty(box);
            }

            Handles.Label(tr + new Vector3(0.1f, 0.1f, 0), $"{box.size.x:F2} x {box.size.y:F2}");
        }

        private static void DrawCircle(GameObject target, CircleCollider2D circ)
        {
            Vector2 worldCenter = (Vector2)target.transform.TransformPoint(circ.offset);
            Vector3 c3 = new Vector3(worldCenter.x, worldCenter.y, 0);

            Handles.color = SelectedEdge;
            Handles.DrawWireDisc(c3, Vector3.forward, circ.radius);

            float handleSize = HandleUtility.GetHandleSize(c3) * 0.08f;

            EditorGUI.BeginChangeCheck();
            Vector3 ncenter = Handles.PositionHandle(c3, Quaternion.identity);
            Vector3 edgePoint = c3 + Vector3.right * circ.radius;
            Vector3 nedge = Handles.FreeMoveHandle(edgePoint, handleSize, Vector3.zero, Handles.RectangleHandleCap);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(circ, "Edit CircleCollider2D");
                if (ncenter != c3)
                {
                    circ.offset = (Vector2)target.transform.InverseTransformPoint(ncenter);
                }
                else
                {
                    float newR = Vector2.Distance(new Vector2(ncenter.x, ncenter.y), new Vector2(nedge.x, nedge.y));
                    circ.radius = Mathf.Max(0.05f, newR);
                }
                EditorUtility.SetDirty(circ);
            }

            Handles.Label(c3 + Vector3.up * (circ.radius + 0.15f), $"r={circ.radius:F2}");
        }

        private static void DrawPolygon(GameObject target, PolygonCollider2D p)
        {
            if (p.pathCount == 0) return;
            Vector2[] path = p.GetPath(0);
            if (path == null || path.Length == 0) return;

            Handles.color = SelectedEdge;
            for (int i = 0; i < path.Length; i++)
            {
                Vector2 a = (Vector2)target.transform.TransformPoint(path[i] + p.offset);
                Vector2 b = (Vector2)target.transform.TransformPoint(path[(i + 1) % path.Length] + p.offset);
                Handles.DrawLine(new Vector3(a.x, a.y, 0), new Vector3(b.x, b.y, 0));
            }

            EditorGUI.BeginChangeCheck();
            bool any = false;
            for (int i = 0; i < path.Length; i++)
            {
                Vector2 w = (Vector2)target.transform.TransformPoint(path[i] + p.offset);
                float hs = HandleUtility.GetHandleSize(new Vector3(w.x, w.y, 0)) * 0.07f;
                Vector3 nw = Handles.FreeMoveHandle(new Vector3(w.x, w.y, 0), hs, Vector3.zero, Handles.SphereHandleCap);
                if (nw.x != w.x || nw.y != w.y)
                {
                    Vector2 local = (Vector2)target.transform.InverseTransformPoint(nw) - p.offset;
                    path[i] = local;
                    any = true;
                }
            }
            if (EditorGUI.EndChangeCheck() && any)
            {
                Undo.RecordObject(p, "Edit PolygonCollider2D");
                p.SetPath(0, path);
                EditorUtility.SetDirty(p);
            }
        }

        private static void DrawEdge(GameObject target, EdgeCollider2D e)
        {
            Vector2[] path = e.points;
            if (path == null || path.Length < 2) return;

            Handles.color = SelectedEdge;
            for (int i = 0; i < path.Length - 1; i++)
            {
                Vector2 a = (Vector2)target.transform.TransformPoint(path[i] + e.offset);
                Vector2 b = (Vector2)target.transform.TransformPoint(path[i + 1] + e.offset);
                Handles.DrawLine(new Vector3(a.x, a.y, 0), new Vector3(b.x, b.y, 0));
            }

            EditorGUI.BeginChangeCheck();
            bool any = false;
            for (int i = 0; i < path.Length; i++)
            {
                Vector2 w = (Vector2)target.transform.TransformPoint(path[i] + e.offset);
                float hs = HandleUtility.GetHandleSize(new Vector3(w.x, w.y, 0)) * 0.07f;
                Vector3 nw = Handles.FreeMoveHandle(new Vector3(w.x, w.y, 0), hs, Vector3.zero, Handles.SphereHandleCap);
                if (nw.x != w.x || nw.y != w.y)
                {
                    Vector2 local = (Vector2)target.transform.InverseTransformPoint(nw) - e.offset;
                    path[i] = local;
                    any = true;
                }
            }
            if (EditorGUI.EndChangeCheck() && any)
            {
                Undo.RecordObject(e, "Edit EdgeCollider2D");
                e.points = path;
                EditorUtility.SetDirty(e);
            }
        }
    }
}
#endif
