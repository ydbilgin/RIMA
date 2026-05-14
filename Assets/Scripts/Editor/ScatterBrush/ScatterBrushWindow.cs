#if UNITY_EDITOR
using System;
using RIMA.Runtime.Scatter;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.ScatterBrush
{
    public sealed class ScatterBrushWindow : EditorWindow
    {
        private static readonly string[] Categories = { "Stones", "Moss", "Rubble", "Dirt" };
        private static readonly Vector2[] NoiseOffsets =
        {
            new Vector2(0f, 0f),
            new Vector2(50f, 50f),
            new Vector2(100f, 0f),
            new Vector2(0f, 100f)
        };
        private static readonly float[] NoiseThresholds = { 0.6f, 0.5f, 0.7f, 0.45f };
        private static readonly float[] NaturalDensities = { 0.3f, 0.4f, 0.2f, 0.5f };

        private const string DatabasePath = "Assets/Art/Scatter/ScatterDatabase.asset";
        private const string RootName = "ScatterRoot";
        private const float NoiseScale = 0.15f;
        private const float GridStep = 0.5f;

        private ScatterDatabase database;
        private int selectedCategory;
        private float brushRadius = 2f;
        private float density = 0.5f;
        private float minScale = 0.8f;
        private float maxScale = 1.2f;
        private bool isPainting;

        [MenuItem("Tools/RIMA/Scatter Brush")]
        public static void Open()
        {
            ScatterBrushWindow window = GetWindow<ScatterBrushWindow>("Scatter Brush");
            window.minSize = new Vector2(320f, 300f);
            window.Show();
        }

        private void OnEnable()
        {
            database = AssetDatabase.LoadAssetAtPath<ScatterDatabase>(DatabasePath);
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
            database = (ScatterDatabase)EditorGUILayout.ObjectField("Scatter Database", database, typeof(ScatterDatabase), false);

            EditorGUILayout.Space(8f);
            selectedCategory = GUILayout.Toolbar(selectedCategory, Categories);

            EditorGUILayout.Space(8f);
            brushRadius = EditorGUILayout.Slider("Brush Radius", brushRadius, 0.5f, 8f);
            density = EditorGUILayout.Slider("Density", density, 0.1f, 1f);
            minScale = EditorGUILayout.FloatField("Min Scale", minScale);
            maxScale = EditorGUILayout.FloatField("Max Scale", maxScale);
            if (maxScale < minScale)
                maxScale = minScale;

            EditorGUILayout.Space(8f);
            using (new EditorGUI.DisabledScope(database == null))
            {
                if (GUILayout.Button("Generate Natural Map", GUILayout.Height(30f)))
                    GenerateNaturalMap();

                if (GUILayout.Button("Clear Category", GUILayout.Height(26f)))
                    ClearCategory(CurrentCategory);

                if (GUILayout.Button("Clear All", GUILayout.Height(26f)))
                    ClearAll();
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (database == null)
                return;

            Event current = Event.current;
            Vector3 mouseWorld = GetMouseWorldPosition(current.mousePosition);

            Handles.color = new Color(0.2f, 0.85f, 0.95f, 0.9f);
            Handles.DrawWireDisc(mouseWorld, Vector3.forward, brushRadius);
            sceneView.Repaint();

            if (current.alt || current.button != 0)
                return;

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

        private void PaintAt(Vector3 center)
        {
            ScatterItem item = GetCurrentItem();
            if (!CanPlace(item))
                return;

            int count = Mathf.Max(1, Mathf.RoundToInt(brushRadius * brushRadius * Mathf.PI * density));
            GameObject parent = GetCategoryRoot(item.category);

            for (int i = 0; i < count; i++)
            {
                Vector2 offset = UnityEngine.Random.insideUnitCircle * brushRadius;
                Vector3 position = new Vector3(center.x + offset.x, center.y + offset.y, 0f);
                CreateScatterObject(item, parent.transform, position, minScale, maxScale);
            }

            MarkActiveSceneDirty();
        }

        private void GenerateNaturalMap()
        {
            if (database == null)
                return;

            Bounds bounds = GetFloorBounds();
            int created = 0;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Generate Natural Scatter Map");
            int undoGroup = Undo.GetCurrentGroup();

            for (int i = 0; i < Categories.Length; i++)
            {
                ScatterItem item = database.GetItem(Categories[i]);
                if (!CanPlace(item))
                    continue;

                GameObject parent = GetCategoryRoot(item.category);
                Vector2 noiseOffset = NoiseOffsets[i];
                float threshold = NoiseThresholds[i];
                float categoryDensity = NaturalDensities[i];

                for (float x = bounds.min.x; x <= bounds.max.x; x += GridStep)
                {
                    for (float y = bounds.min.y; y <= bounds.max.y; y += GridStep)
                    {
                        float noise = Mathf.PerlinNoise((x + noiseOffset.x) * NoiseScale, (y + noiseOffset.y) * NoiseScale);
                        if (noise <= threshold || UnityEngine.Random.value > categoryDensity)
                            continue;

                        Vector3 position = new Vector3(
                            x + UnityEngine.Random.Range(-GridStep * 0.35f, GridStep * 0.35f),
                            y + UnityEngine.Random.Range(-GridStep * 0.35f, GridStep * 0.35f),
                            0f);
                        CreateScatterObject(item, parent.transform, position, item.minScale, item.maxScale);
                        created++;
                    }
                }
            }

            Undo.CollapseUndoOperations(undoGroup);
            MarkActiveSceneDirty();
            Debug.Log($"[ScatterBrush] Generated {created} natural scatter objects.");
        }

        private void ClearCategory(string category)
        {
            GameObject root = GameObject.Find(RootName);
            if (root == null)
                return;

            Transform categoryRoot = root.transform.Find(category);
            if (categoryRoot == null)
                return;

            Undo.RegisterFullObjectHierarchyUndo(categoryRoot.gameObject, $"Clear {category} Scatter");
            for (int i = categoryRoot.childCount - 1; i >= 0; i--)
                Undo.DestroyObjectImmediate(categoryRoot.GetChild(i).gameObject);

            MarkActiveSceneDirty();
        }

        private void ClearAll()
        {
            GameObject root = GameObject.Find(RootName);
            if (root == null)
                return;

            Undo.RegisterFullObjectHierarchyUndo(root, "Clear All Scatter");
            for (int i = root.transform.childCount - 1; i >= 0; i--)
            {
                Transform categoryRoot = root.transform.GetChild(i);
                for (int j = categoryRoot.childCount - 1; j >= 0; j--)
                    Undo.DestroyObjectImmediate(categoryRoot.GetChild(j).gameObject);
            }

            MarkActiveSceneDirty();
        }

        private ScatterItem GetCurrentItem()
        {
            return database != null ? database.GetItem(CurrentCategory) : null;
        }

        private bool CanPlace(ScatterItem item)
        {
            return item != null && item.sprites != null && item.sprites.Length > 0;
        }

        private GameObject GetCategoryRoot(string category)
        {
            GameObject root = GameObject.Find(RootName);
            if (root == null)
            {
                root = new GameObject(RootName);
                Undo.RegisterCreatedObjectUndo(root, "Create ScatterRoot");
            }

            EnsureCategoryRoots(root);

            Transform categoryRoot = root.transform.Find(category);
            return categoryRoot != null ? categoryRoot.gameObject : root;
        }

        private static void EnsureCategoryRoots(GameObject root)
        {
            foreach (string category in Categories)
            {
                if (root.transform.Find(category) != null)
                    continue;

                GameObject child = new GameObject(category);
                Undo.RegisterCreatedObjectUndo(child, $"Create {category} Scatter Root");
                child.transform.SetParent(root.transform);
            }
        }

        private static void CreateScatterObject(ScatterItem item, Transform parent, Vector3 position, float overrideMinScale, float overrideMaxScale)
        {
            Sprite sprite = item.sprites[UnityEngine.Random.Range(0, item.sprites.Length)];
            GameObject scatter = new GameObject($"{item.category}_scatter_{DateTime.Now:yyyyMMddHHmmssfff}");
            Undo.RegisterCreatedObjectUndo(scatter, "Paint Scatter");

            scatter.transform.SetParent(parent);
            scatter.transform.position = position;

            float scale = UnityEngine.Random.Range(overrideMinScale, overrideMaxScale);
            scatter.transform.localScale = Vector3.one * scale;

            if (item.randomRotation)
                scatter.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));

            SpriteRenderer renderer = scatter.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = item.sortingOrder;
        }

        private static Bounds GetFloorBounds()
        {
            Tilemap floor = FindFloorTilemap();
            if (floor != null)
            {
                floor.CompressBounds();
                Bounds localBounds = floor.localBounds;
                Vector3 min = floor.transform.TransformPoint(localBounds.min);
                Vector3 max = floor.transform.TransformPoint(localBounds.max);
                return new Bounds((min + max) * 0.5f, max - min);
            }

            return new Bounds(Vector3.zero, new Vector3(20f, 20f, 0f));
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
                        return tilemap;
                }
            }
            return null;
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
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private string CurrentCategory => Categories[Mathf.Clamp(selectedCategory, 0, Categories.Length - 1)];
    }
}
#endif
