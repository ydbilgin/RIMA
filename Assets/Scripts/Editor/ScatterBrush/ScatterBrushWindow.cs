#if UNITY_EDITOR
using System;
using RIMA.MapDesigner;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.ScatterBrush
{
    public sealed class ScatterBrushWindow : EditorWindow
    {
        private const string RootName = "ScatterBrushPreview";

        [SerializeField] private ScatterBrushSO brush;
        [SerializeField] private Tilemap baseTilemap;
        [SerializeField] private ScatterBrushPainter painter;
        [SerializeField] private int seed = 12345;
        [SerializeField] private float brushRadius = 2f;
        [SerializeField] private bool showPreview = true;
        private bool isPainting;

        [MenuItem("Tools/RIMA/Scatter Brush")]
        public static void Open()
        {
            ScatterBrushWindow window = GetWindow<ScatterBrushWindow>("Scatter Brush");
            window.minSize = new Vector2(320f, 280f);
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            isPainting = false;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8f);
            brush = (ScatterBrushSO)EditorGUILayout.ObjectField("Brush", brush, typeof(ScatterBrushSO), false);
            baseTilemap = (Tilemap)EditorGUILayout.ObjectField("Base Tilemap", baseTilemap, typeof(Tilemap), true);
            painter = (ScatterBrushPainter)EditorGUILayout.ObjectField("Painter", painter, typeof(ScatterBrushPainter), true);
            seed = EditorGUILayout.IntField("Seed", seed);
            brushRadius = EditorGUILayout.Slider("Paint Radius", brushRadius, 0.5f, 8f);
            showPreview = EditorGUILayout.ToggleLeft("Preview Gizmo", showPreview);

            EditorGUILayout.Space(8f);
            using (new EditorGUI.DisabledScope(brush == null))
            {
                if (GUILayout.Button("Generate Natural Map", GUILayout.Height(30f)))
                {
                    GenerateNaturalMap();
                }

                if (GUILayout.Button("Clear Scatter", GUILayout.Height(26f)))
                {
                    ClearScatter();
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (brush == null || !showPreview)
            {
                return;
            }

            Event current = Event.current;
            Vector3 mouseWorld = GetMouseWorldPosition(current.mousePosition);

            Handles.color = new Color(0.2f, 0.85f, 0.95f, 0.9f);
            Handles.DrawWireDisc(mouseWorld, Vector3.forward, brushRadius);
            DrawPreviewPoints(mouseWorld);
            sceneView.Repaint();

            if (current.alt || current.button != 0)
            {
                return;
            }

            if (current.type == EventType.MouseDown)
            {
                isPainting = true;
                PaintAt(mouseWorld);
                current.Use();
            }
            else if (current.type == EventType.MouseDrag && isPainting)
            {
                PaintAt(mouseWorld);
                current.Use();
            }
            else if (current.type == EventType.MouseUp && isPainting)
            {
                isPainting = false;
                current.Use();
            }
        }

        private void GenerateNaturalMap()
        {
            Tilemap tilemap = baseTilemap != null ? baseTilemap : FindFloorTilemap();
            ScatterBrushPainter activePainter = painter != null ? painter : EnsurePainter(tilemap);
            if (tilemap == null || activePainter == null)
            {
                Debug.LogWarning("[ScatterBrush] Assign a base Tilemap or place one in the scene.");
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(activePainter.gameObject, "Generate Scatter Brush");
            activePainter.PaintScatter(tilemap, brush, seed);
            EditorUtility.SetDirty(activePainter);
            MarkActiveSceneDirty();
        }

        private void PaintAt(Vector3 center)
        {
            if (brush == null || brush.entries == null || brush.entries.Count == 0)
            {
                return;
            }

            Transform root = EnsurePreviewRoot();
            int count = Mathf.Max(1, Mathf.RoundToInt(brushRadius * brushRadius));
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Paint Scatter Brush");
            for (int i = 0; i < count; i++)
            {
                ScatterEntry entry = brush.entries[i % brush.entries.Count];
                if (entry == null || entry.sprite == null)
                {
                    continue;
                }

                Vector2 offset = UnityEngine.Random.insideUnitCircle * brushRadius;
                GameObject scatter = new GameObject("ScatterPaint_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                Undo.RegisterCreatedObjectUndo(scatter, "Paint Scatter Brush");
                scatter.transform.SetParent(root, false);
                scatter.transform.position = center + new Vector3(offset.x, offset.y, 0f);
                scatter.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
                scatter.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.75f, 1.2f);
                SpriteRenderer renderer = scatter.AddComponent<SpriteRenderer>();
                renderer.sprite = entry.sprite;
                renderer.sortingLayerName = "Scatter";
                renderer.sortingOrder = 2;
            }

            MarkActiveSceneDirty();
        }

        private static ScatterBrushPainter EnsurePainter(Tilemap tilemap)
        {
            if (tilemap == null)
            {
                return null;
            }

            ScatterBrushPainter existing = tilemap.GetComponentInParent<ScatterBrushPainter>();
            if (existing != null)
            {
                return existing;
            }

            GameObject host = new GameObject("ScatterBrushPainter");
            Undo.RegisterCreatedObjectUndo(host, "Create ScatterBrushPainter");
            host.transform.SetParent(tilemap.transform.parent != null ? tilemap.transform.parent : tilemap.transform, false);
            return host.AddComponent<ScatterBrushPainter>();
        }

        private static void ClearScatter()
        {
            foreach (ScatterBrushPainter activePainter in FindObjectsByType<ScatterBrushPainter>(FindObjectsSortMode.None))
            {
                Transform layer = activePainter.transform.Find("ScatterLayer");
                if (layer == null)
                {
                    continue;
                }

                Undo.RegisterFullObjectHierarchyUndo(layer.gameObject, "Clear Scatter");
                for (int i = layer.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(layer.GetChild(i).gameObject);
                }
            }

            GameObject previewRoot = GameObject.Find(RootName);
            if (previewRoot != null)
            {
                Undo.DestroyObjectImmediate(previewRoot);
            }

            MarkActiveSceneDirty();
        }

        private void DrawPreviewPoints(Vector3 center)
        {
            if (brush.entries == null)
            {
                return;
            }

            Handles.color = new Color(0.35f, 1f, 0.55f, 0.7f);
            int points = Mathf.Min(16, Mathf.Max(0, brush.entries.Count * 4));
            for (int i = 0; i < points; i++)
            {
                float angle = i * 137.5f * Mathf.Deg2Rad;
                float radius = brushRadius * Mathf.Sqrt((i + 0.5f) / points);
                Vector3 point = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
                Handles.DrawSolidDisc(point, Vector3.forward, 0.035f);
            }
        }

        private static Transform EnsurePreviewRoot()
        {
            GameObject root = GameObject.Find(RootName);
            if (root == null)
            {
                root = new GameObject(RootName);
                Undo.RegisterCreatedObjectUndo(root, "Create Scatter Preview Root");
            }

            return root.transform;
        }

        private static Tilemap FindFloorTilemap()
        {
            Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            string[] priority = { "FloorTilemap", "Floor", "Base", "BaseTilemap" };
            foreach (string keyword in priority)
            {
                foreach (Tilemap tilemap in tilemaps)
                {
                    if (tilemap != null && tilemap.name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return tilemap;
                    }
                }
            }

            return tilemaps.Length > 0 ? tilemaps[0] : null;
        }

        private static Vector3 GetMouseWorldPosition(Vector2 guiPosition)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            return plane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : Vector3.zero;
        }

        private static void MarkActiveSceneDirty()
        {
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}
#endif
