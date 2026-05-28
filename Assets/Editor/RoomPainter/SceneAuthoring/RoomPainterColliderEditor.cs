using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    [InitializeOnLoad]
    internal static class RoomPainterColliderEditor
    {
        private const string EnabledPrefKey = "RIMA.RoomPainter.SceneColliderEdit";
        private static readonly Color OutlineColor = new Color(0.30f, 0.95f, 0.60f, 1f);
        private static readonly Color OutlineTriggerColor = new Color(1f, 0.85f, 0.25f, 1f);
        private static readonly Color HandleFillColor = new Color(0.95f, 0.95f, 0.20f, 1f);
        private static readonly Vector2[] BoxCornerSigns =
        {
            new Vector2(-1f, -1f),
            new Vector2(+1f, -1f),
            new Vector2(+1f, +1f),
            new Vector2(-1f, +1f)
        };
        private static readonly Vector2[] BoxEdgeSigns =
        {
            new Vector2(0f, -1f),
            new Vector2(+1f, 0f),
            new Vector2(0f, +1f),
            new Vector2(-1f, 0f)
        };

        static RoomPainterColliderEditor()
        {
            SceneView.duringSceneGui -= OnSceneGui;
            SceneView.duringSceneGui += OnSceneGui;
        }

        public static bool Enabled
        {
            get { return EditorPrefs.GetBool(EnabledPrefKey, true); }
            set { EditorPrefs.SetBool(EnabledPrefKey, value); SceneView.RepaintAll(); }
        }

        // D4: Whether we are currently inside Prefab Mode
        public static bool IsInPrefabMode
        {
            get { return PrefabStageUtility.GetCurrentPrefabStage() != null; }
        }

        // D4: Returns the active prefab stage GameObject, or null if not in Prefab Mode
        public static GameObject PrefabStageRoot
        {
            get
            {
                PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
                return stage != null ? stage.prefabContentsRoot : null;
            }
        }

        private static void OnSceneGui(SceneView sceneView)
        {
            if (!Enabled)
            {
                return;
            }

            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                return;
            }

            // D4: In Prefab Mode, draw handles on the prefab root directly (no binding required)
            if (IsInPrefabMode)
            {
                GameObject prefabRoot = PrefabStageRoot;
                if (prefabRoot != null && (go == prefabRoot || go.transform.IsChildOf(prefabRoot.transform)))
                {
                    Collider2D[] prefabColliders = go.GetComponents<Collider2D>();
                    for (int i = 0; i < prefabColliders.Length; i++)
                    {
                        if (prefabColliders[i] != null)
                        {
                            DrawCollider(prefabColliders[i]);
                        }
                    }
                }

                return;
            }

            // Scene mode: existing behaviour — require RoomPainterAssetBinding
            if (go.GetComponent<RoomPainterAssetBinding>() == null)
            {
                return;
            }

            Collider2D[] colliders = go.GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null)
                {
                    continue;
                }

                DrawCollider(colliders[i]);
            }
        }

        private static void DrawCollider(Collider2D collider)
        {
            if (collider is BoxCollider2D box)
            {
                DrawBoxCollider(box);
                return;
            }

            if (collider is CircleCollider2D circle)
            {
                DrawCircleCollider(circle);
                return;
            }

            if (collider is CapsuleCollider2D capsule)
            {
                DrawCapsuleCollider(capsule);
                return;
            }
        }

        private static void DrawBoxCollider(BoxCollider2D box)
        {
            Transform tr = box.transform;
            Vector2 size = box.size;
            Vector2 half = size * 0.5f;
            Color outline = box.isTrigger ? OutlineTriggerColor : OutlineColor;

            Vector3[] worldCorners = new Vector3[5];
            for (int i = 0; i < 4; i++)
            {
                Vector2 local = (Vector2)box.offset + new Vector2(BoxCornerSigns[i].x * half.x, BoxCornerSigns[i].y * half.y);
                worldCorners[i] = tr.TransformPoint(local);
            }

            worldCorners[4] = worldCorners[0];

            Handles.color = outline;
            Handles.DrawAAPolyLine(2.5f, worldCorners);

            float dotSize = HandleUtility.GetHandleSize(tr.TransformPoint(box.offset)) * 0.08f;
            for (int i = 0; i < 4; i++)
            {
                Vector2 cornerLocal = (Vector2)box.offset + new Vector2(BoxCornerSigns[i].x * half.x, BoxCornerSigns[i].y * half.y);
                Vector2 oppositeLocal = (Vector2)box.offset + new Vector2(-BoxCornerSigns[i].x * half.x, -BoxCornerSigns[i].y * half.y);

                Vector3 cornerWorld = tr.TransformPoint(cornerLocal);
                Handles.color = HandleFillColor;

                EditorGUI.BeginChangeCheck();
                Vector3 newWorld = Handles.FreeMoveHandle(cornerWorld, dotSize, Vector3.zero, Handles.DotHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(box, "Resize Box Collider");
                    Vector2 newLocal = tr.InverseTransformPoint(newWorld);
                    Vector2 newCenter = (newLocal + oppositeLocal) * 0.5f;
                    Vector2 newSize = new Vector2(Mathf.Max(0.01f, Mathf.Abs(newLocal.x - oppositeLocal.x)), Mathf.Max(0.01f, Mathf.Abs(newLocal.y - oppositeLocal.y)));
                    box.offset = newCenter;
                    box.size = newSize;
                    EditorUtility.SetDirty(box);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Vector2 sign = BoxEdgeSigns[i];
                Vector2 edgeLocal = (Vector2)box.offset + new Vector2(sign.x * half.x, sign.y * half.y);
                Vector3 edgeWorld = tr.TransformPoint(edgeLocal);

                Handles.color = HandleFillColor * 0.85f;

                EditorGUI.BeginChangeCheck();
                Vector3 newWorld = Handles.FreeMoveHandle(edgeWorld, dotSize * 0.8f, Vector3.zero, Handles.DotHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(box, "Resize Box Edge");
                    Vector2 newLocal = tr.InverseTransformPoint(newWorld);
                    Vector2 newSize = box.size;
                    Vector2 newCenter = (Vector2)box.offset;

                    if (Mathf.Abs(sign.y) > 0.5f)
                    {
                        float opposite = box.offset.y - sign.y * half.y;
                        float draggedY = newLocal.y;
                        newCenter.y = (opposite + draggedY) * 0.5f;
                        newSize.y = Mathf.Max(0.01f, Mathf.Abs(draggedY - opposite));
                    }

                    if (Mathf.Abs(sign.x) > 0.5f)
                    {
                        float opposite = box.offset.x - sign.x * half.x;
                        float draggedX = newLocal.x;
                        newCenter.x = (opposite + draggedX) * 0.5f;
                        newSize.x = Mathf.Max(0.01f, Mathf.Abs(draggedX - opposite));
                    }

                    box.offset = newCenter;
                    box.size = newSize;
                    EditorUtility.SetDirty(box);
                }
            }

            DrawCenterLabel(tr.TransformPoint(box.offset), "Box " + box.size.x.ToString("0.00") + " x " + box.size.y.ToString("0.00"), outline);
        }

        private static void DrawCircleCollider(CircleCollider2D circle)
        {
            Transform tr = circle.transform;
            Vector3 center = tr.TransformPoint(circle.offset);
            float radius = circle.radius * Mathf.Max(Mathf.Abs(tr.lossyScale.x), Mathf.Abs(tr.lossyScale.y));
            Color outline = circle.isTrigger ? OutlineTriggerColor : OutlineColor;

            Handles.color = outline;
            Handles.DrawWireDisc(center, Vector3.forward, radius);

            float dotSize = HandleUtility.GetHandleSize(center) * 0.08f;
            Vector3 edgeRight = center + Vector3.right * radius;
            Vector3 edgeUp = center + Vector3.up * radius;

            Handles.color = HandleFillColor;
            EditorGUI.BeginChangeCheck();
            Vector3 newEdge = Handles.FreeMoveHandle(edgeRight, dotSize, Vector3.zero, Handles.DotHandleCap);
            Vector3 newEdgeUp = Handles.FreeMoveHandle(edgeUp, dotSize * 0.85f, Vector3.zero, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(circle, "Resize Circle Collider");
                float draggedH = Vector3.Distance(center, newEdge);
                float draggedV = Vector3.Distance(center, newEdgeUp);
                float chosen = Mathf.Max(draggedH, draggedV);
                float lossy = Mathf.Max(0.0001f, Mathf.Max(Mathf.Abs(tr.lossyScale.x), Mathf.Abs(tr.lossyScale.y)));
                circle.radius = Mathf.Max(0.01f, chosen / lossy);
                EditorUtility.SetDirty(circle);
            }

            DrawCenterLabel(center, "Circle r=" + circle.radius.ToString("0.00"), outline);
        }

        private static void DrawCapsuleCollider(CapsuleCollider2D capsule)
        {
            Transform tr = capsule.transform;
            Vector2 size = capsule.size;
            Vector2 half = size * 0.5f;
            Color outline = capsule.isTrigger ? OutlineTriggerColor : OutlineColor;

            Vector3[] worldCorners = new Vector3[5];
            worldCorners[0] = tr.TransformPoint((Vector2)capsule.offset + new Vector2(-half.x, -half.y));
            worldCorners[1] = tr.TransformPoint((Vector2)capsule.offset + new Vector2(+half.x, -half.y));
            worldCorners[2] = tr.TransformPoint((Vector2)capsule.offset + new Vector2(+half.x, +half.y));
            worldCorners[3] = tr.TransformPoint((Vector2)capsule.offset + new Vector2(-half.x, +half.y));
            worldCorners[4] = worldCorners[0];

            Handles.color = outline;
            Handles.DrawAAPolyLine(2.5f, worldCorners);

            float dotSize = HandleUtility.GetHandleSize(tr.TransformPoint(capsule.offset)) * 0.08f;
            Vector3 rightEdge = tr.TransformPoint((Vector2)capsule.offset + new Vector2(half.x, 0f));
            Vector3 topEdge = tr.TransformPoint((Vector2)capsule.offset + new Vector2(0f, half.y));

            Handles.color = HandleFillColor;
            EditorGUI.BeginChangeCheck();
            Vector3 newRight = Handles.FreeMoveHandle(rightEdge, dotSize, Vector3.zero, Handles.DotHandleCap);
            Vector3 newTop = Handles.FreeMoveHandle(topEdge, dotSize, Vector3.zero, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(capsule, "Resize Capsule Collider");
                Vector2 localRight = tr.InverseTransformPoint(newRight);
                Vector2 localTop = tr.InverseTransformPoint(newTop);
                float halfX = Mathf.Max(0.01f, Mathf.Abs(localRight.x - capsule.offset.x));
                float halfY = Mathf.Max(0.01f, Mathf.Abs(localTop.y - capsule.offset.y));
                capsule.size = new Vector2(halfX * 2f, halfY * 2f);
                EditorUtility.SetDirty(capsule);
            }

            DrawCenterLabel(tr.TransformPoint(capsule.offset), "Capsule " + capsule.size.x.ToString("0.00") + " x " + capsule.size.y.ToString("0.00"), outline);
        }

        private static void DrawCenterLabel(Vector3 worldPos, string text, Color color)
        {
            Handles.BeginGUI();
            Vector2 screen = HandleUtility.WorldToGUIPoint(worldPos);
            GUIStyle style = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = color },
                fontStyle = FontStyle.Bold
            };

            Rect rect = new Rect(screen.x - 60f, screen.y - 8f, 120f, 16f);
            GUI.Label(rect, text, style);
            Handles.EndGUI();
        }
    }
}
