#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using LaurethStudio.PainterSuite.Editor.Colliders;
using LaurethStudio.PainterSuite.Runtime;

namespace LaurethStudio.PainterSuite.Editor.Core
{
    public sealed class PainterSuiteWindow : EditorWindow
    {
        public enum PainterMode { Collider, Layer, Template }

        [SerializeField] private PainterMode mode = PainterMode.Collider;
        [SerializeField] private ShapeMode shapeMode = ShapeMode.Box;
        [SerializeField] private GameObject target;
        [SerializeField] private bool snapToPixel = true;
        [SerializeField] private int pixelsPerUnit = 64;
        [SerializeField] private int selectedColliderIndex = -1;
        [SerializeField] private string newTemplateName = "";

        private ColliderPainter _colliderPainter;
        private Vector2 _templateScroll;

        [MenuItem("Window/LaurethStudio/Painter Suite", priority = 2000)]
        public static void Open()
        {
            var w = GetWindow<PainterSuiteWindow>("Painter Suite");
            w.minSize = new Vector2(480f, 640f);
            w.Show();
        }

        private void OnEnable()
        {
            _colliderPainter = new ColliderPainter();
            SceneView.duringSceneGui -= OnSceneGUIHook;
            SceneView.duringSceneGui += OnSceneGUIHook;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUIHook;
        }

        // ---- Public API for shortcuts ----
        public void SetShapeMode(ShapeMode m)
        {
            shapeMode = m;
            _colliderPainter?.Reset();
            Repaint();
            SceneView.RepaintAll();
        }

        public void CancelInProgressShape()
        {
            _colliderPainter?.Reset();
            SceneView.RepaintAll();
        }

        public void DeleteSelectedCollider()
        {
            if (target == null) return;
            var all = target.GetComponents<Collider2D>();
            if (selectedColliderIndex < 0 || selectedColliderIndex >= all.Length) return;
            Undo.DestroyObjectImmediate(all[selectedColliderIndex]);
            selectedColliderIndex = -1;
            Repaint();
            SceneView.RepaintAll();
        }
        // -----------------------------------

        private void OnSceneGUIHook(SceneView sceneView)
        {
            if (target == null) return;
            switch (mode)
            {
                case PainterMode.Collider:
                    _colliderPainter.Target = target;
                    _colliderPainter.Mode = shapeMode;
                    _colliderPainter.SnapToPixel = snapToPixel;
                    _colliderPainter.PixelsPerUnit = pixelsPerUnit;
                    _colliderPainter.OnSceneGUI(sceneView);
                    // Render edit handles for selected collider
                    if (selectedColliderIndex >= 0)
                    {
                        ColliderHandles.DrawAndEditSelected(target, selectedColliderIndex);
                    }
                    break;
                case PainterMode.Layer:
                case PainterMode.Template:
                    break;
            }
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawModeTabs();
            EditorGUILayout.Space(8);
            DrawTargetSection();
            EditorGUILayout.Space(8);
            DrawSettingsSection();
            EditorGUILayout.Space(8);
            DrawModeBody();
        }

        private void DrawHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("LaurethStudio Painter Suite", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.Label("v0.3.0", EditorStyles.miniLabel);
            }
        }

        private void DrawModeTabs()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (PainterMode m in System.Enum.GetValues(typeof(PainterMode)))
                {
                    bool active = mode == m;
                    if (GUILayout.Toggle(active, m.ToString(), EditorStyles.toolbarButton) && !active)
                    {
                        mode = m;
                        Repaint();
                        SceneView.RepaintAll();
                    }
                }
            }
        }

        private void DrawTargetSection()
        {
            EditorGUILayout.LabelField("Target", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.BeginChangeCheck();
                target = (GameObject)EditorGUILayout.ObjectField("GameObject", target, typeof(GameObject), true);
                if (EditorGUI.EndChangeCheck())
                {
                    selectedColliderIndex = -1;
                    SceneView.RepaintAll();
                }
                if (target == null)
                {
                    EditorGUILayout.HelpBox("Drop a GameObject (sprite recommended). Then paint colliders in SceneView.", MessageType.Info);
                }
            }
        }

        private void DrawSettingsSection()
        {
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                snapToPixel = EditorGUILayout.Toggle("Snap to Pixel", snapToPixel);
                using (new EditorGUI.DisabledScope(!snapToPixel))
                {
                    pixelsPerUnit = EditorGUILayout.IntField("PPU", pixelsPerUnit);
                    if (pixelsPerUnit < 1) pixelsPerUnit = 1;
                }
            }
        }

        private void DrawModeBody()
        {
            EditorGUILayout.LabelField($"Mode: {mode}", EditorStyles.boldLabel);
            switch (mode)
            {
                case PainterMode.Collider: DrawColliderBody(); break;
                case PainterMode.Layer:
                    EditorGUILayout.HelpBox("Layer painter -- v0.5.0 (Day 5+).", MessageType.Info);
                    break;
                case PainterMode.Template: DrawTemplateBody(); break;
            }
        }

        private void DrawColliderBody()
        {
            EditorGUILayout.LabelField("Shape", EditorStyles.miniBoldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (ShapeMode s in System.Enum.GetValues(typeof(ShapeMode)))
                {
                    bool active = shapeMode == s;
                    if (GUILayout.Toggle(active, s.ToString(), EditorStyles.miniButton) && !active)
                    {
                        SetShapeMode(s);
                    }
                }
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.HelpBox(
                "Workflow:\n" +
                "Box / Circle  -> LMB drag in SceneView\n" +
                "Polygon / Edge -> LMB click each vertex, double-click or Enter to finish, RMB to cancel\n\n" +
                "Shortcuts (when this window is focused):\n" +
                "Shift+B Box | Shift+C Circle | Shift+P Polygon | Shift+E Edge\n" +
                "Escape Cancel | Delete  Remove selected collider\n\n" +
                "Each paint = independent undo step (Ctrl+Z reverts one).",
                MessageType.None);

            if (target == null) return;

            DrawColliderList();
        }

        private void DrawColliderList()
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Colliders on target", EditorStyles.miniBoldLabel);

            var all = target.GetComponents<Collider2D>();
            if (all.Length == 0)
            {
                EditorGUILayout.LabelField("(none yet)", EditorStyles.miniLabel);
                return;
            }

            using (var scroll = new EditorGUILayout.ScrollViewScope(Vector2.zero, GUILayout.MinHeight(60), GUILayout.MaxHeight(160)))
            {
                for (int i = 0; i < all.Length; i++)
                {
                    var c = all[i];
                    if (c == null) continue;
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        bool selected = i == selectedColliderIndex;
                        bool tog = GUILayout.Toggle(selected, "", GUILayout.Width(16));
                        if (tog != selected)
                        {
                            selectedColliderIndex = tog ? i : -1;
                            SceneView.RepaintAll();
                        }

                        GUILayout.Label($"#{i}", GUILayout.Width(28));
                        GUILayout.Label(c.GetType().Name, GUILayout.MinWidth(110));
                        GUILayout.Label(SummarizeCollider(c), EditorStyles.miniLabel);

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Duplicate", EditorStyles.miniButton, GUILayout.Width(70)))
                        {
                            DuplicateCollider(c);
                        }
                        if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(54)))
                        {
                            Undo.DestroyObjectImmediate(c);
                            if (selectedColliderIndex == i) selectedColliderIndex = -1;
                            SceneView.RepaintAll();
                            break;
                        }
                    }
                }
            }
        }

        private static string SummarizeCollider(Collider2D c)
        {
            switch (c)
            {
                case BoxCollider2D b: return $"size {b.size.x:F2}x{b.size.y:F2}  off {b.offset.x:F2},{b.offset.y:F2}";
                case CircleCollider2D circ: return $"r {circ.radius:F2}  off {circ.offset.x:F2},{circ.offset.y:F2}";
                case PolygonCollider2D p:
                    int verts = 0; for (int i = 0; i < p.pathCount; i++) verts += p.GetPath(i).Length;
                    return $"{verts} verts ({p.pathCount} path)";
                case EdgeCollider2D ed: return $"{ed.points.Length} verts (open)";
                default: return "";
            }
        }

        private void DrawTemplateBody()
        {
            EditorGUILayout.HelpBox(
                "Save current target's colliders as a reusable template, or apply an existing one.",
                MessageType.None);

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Save As Template", EditorStyles.miniBoldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                newTemplateName = EditorGUILayout.TextField(newTemplateName);
                using (new EditorGUI.DisabledScope(target == null))
                {
                    if (GUILayout.Button("Save", GUILayout.Width(80)))
                    {
                        var tpl = ColliderTemplateService.SaveAsTemplate(target, newTemplateName);
                        if (tpl != null)
                        {
                            EditorGUIUtility.PingObject(tpl);
                            newTemplateName = "";
                        }
                    }
                }
            }

            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("Library", EditorStyles.miniBoldLabel);

            var templates = ColliderTemplateService.FindAllTemplates();
            if (templates.Length == 0)
            {
                EditorGUILayout.HelpBox("No ColliderTemplate assets found. Save one above, or create via Assets > Create > LaurethStudio > Painter Suite > Collider Template.", MessageType.Info);
                return;
            }

            using (var scroll = new EditorGUILayout.ScrollViewScope(_templateScroll, GUILayout.MinHeight(120), GUILayout.MaxHeight(240)))
            {
                _templateScroll = scroll.scrollPosition;
                foreach (var t in templates)
                {
                    if (t == null) continue;
                    using (new EditorGUILayout.HorizontalScope("box"))
                    {
                        if (t.thumbnail != null)
                            GUILayout.Label(t.thumbnail, GUILayout.Width(48), GUILayout.Height(48));
                        using (new EditorGUILayout.VerticalScope())
                        {
                            EditorGUILayout.LabelField(t.templateName, EditorStyles.boldLabel);
                            EditorGUILayout.LabelField($"{t.shapes.Count} shapes", EditorStyles.miniLabel);
                        }
                        GUILayout.FlexibleSpace();
                        using (new EditorGUI.DisabledScope(target == null))
                        {
                            if (GUILayout.Button("Apply", EditorStyles.miniButton, GUILayout.Width(64)))
                            {
                                ColliderTemplateService.ApplyTemplate(t, target);
                                SceneView.RepaintAll();
                            }
                        }
                        if (GUILayout.Button("Ping", EditorStyles.miniButton, GUILayout.Width(48)))
                        {
                            EditorGUIUtility.PingObject(t);
                        }
                    }
                }
            }
        }

        private void DuplicateCollider(Collider2D src)
        {
            Vector2 nudge = new Vector2(0.5f, 0f);
            switch (src)
            {
                case BoxCollider2D b:
                    var nb = Undo.AddComponent<BoxCollider2D>(target);
                    nb.size = b.size; nb.offset = b.offset + nudge; nb.isTrigger = b.isTrigger;
                    break;
                case CircleCollider2D c:
                    var nc = Undo.AddComponent<CircleCollider2D>(target);
                    nc.radius = c.radius; nc.offset = c.offset + nudge; nc.isTrigger = c.isTrigger;
                    break;
                case PolygonCollider2D p:
                    var np = Undo.AddComponent<PolygonCollider2D>(target);
                    np.pathCount = p.pathCount;
                    for (int i = 0; i < p.pathCount; i++) np.SetPath(i, p.GetPath(i));
                    np.offset = p.offset + nudge; np.isTrigger = p.isTrigger;
                    break;
                case EdgeCollider2D e:
                    var ne = Undo.AddComponent<EdgeCollider2D>(target);
                    ne.points = e.points;
                    ne.offset = e.offset + nudge; ne.isTrigger = e.isTrigger;
                    break;
            }
            EditorUtility.SetDirty(target);
            SceneView.RepaintAll();
        }
    }
}
#endif
