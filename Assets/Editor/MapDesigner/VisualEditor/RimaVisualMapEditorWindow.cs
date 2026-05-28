#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.VisualEditor
{
    public enum VisualBrushToolMode { Brush, Erase }

    public class RimaVisualMapEditorWindow : EditorWindow
    {
        [SerializeField] private MapDesignerBrushPresetSO selectedBrush;
        [SerializeField] private BrushPackSO activePack;
        [SerializeField] private BiomeSkinSO activeSkin;
        [SerializeField] private int activeSeed = 12345;
        [SerializeField] private VisualBrushToolMode toolMode = VisualBrushToolMode.Brush;
        [SerializeField] private float brushSize = 64f;
        [SerializeField] private float brushDensity = 1.0f;
        [SerializeField] private string selectedCategory = "All";
        [SerializeField] private float currentRotation = 0f;

        private VisualEditorScenePainter scenePainter;
        private Vector2 scrollPosition = Vector2.zero;

        public MapDesignerBrushPresetSO SelectedBrush => selectedBrush;
        public BrushPackSO ActivePack => activePack;
        public BiomeSkinSO ActiveSkin => activeSkin;
        public int ActiveSeed => activeSeed;
        public VisualBrushToolMode ToolMode => toolMode;
        public float BrushSize => brushSize;
        public float BrushDensity => brushDensity;
        public float CurrentRotation => currentRotation;

        public void RotateBrush(float delta)
        {
            currentRotation = (currentRotation + delta) % 360f;
            Repaint();
            SceneView.RepaintAll();
        }

        [MenuItem("RIMA/Visual Map Designer (New)", priority = 21)]
        public static void Open()
        {
            var w = GetWindow<RimaVisualMapEditorWindow>("Visual Map Designer");
            w.minSize = new Vector2(400f, 600f);
            w.Show();
        }

        private void OnEnable()
        {
            scenePainter = new VisualEditorScenePainter(this);
            SceneView.duringSceneGui -= OnSceneGUIHook;
            SceneView.duringSceneGui += OnSceneGUIHook;
            
            // Auto-load default assets to improve UX on first start
            AutoLoadDefaultAssets();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUIHook;
            if (scenePainter != null)
            {
                scenePainter.Cleanup();
            }
        }

        private void AutoLoadDefaultAssets()
        {
            if (activePack == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:BrushPackSO");
                if (guids.Length > 0)
                {
                    activePack = AssetDatabase.LoadAssetAtPath<BrushPackSO>(AssetDatabase.GUIDToAssetPath(guids[0]));
                }
            }
            if (activeSkin == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:BiomeSkinSO");
                if (guids.Length > 0)
                {
                    activeSkin = AssetDatabase.LoadAssetAtPath<BiomeSkinSO>(AssetDatabase.GUIDToAssetPath(guids[0]));
                }
            }
        }

        private void OnGUI()
        {
            EnsureInitialized();
            
            // Window Header
            EditorGUILayout.Space(8);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("RIMA Visual Map Designer", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Design Log", EditorStyles.miniButton, GUILayout.Width(80)))
                {
                    string logPath = "Assets/Editor/MapDesigner/VisualEditor/DESIGN_LOG.md";
                    var obj = AssetDatabase.LoadAssetAtPath<TextAsset>(logPath);
                    if (obj != null) AssetDatabase.OpenAsset(obj);
                }
            }
            EditorGUILayout.Space(4);

            // Settings Fields
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                activePack = (BrushPackSO)EditorGUILayout.ObjectField("Active Pack:", activePack, typeof(BrushPackSO), false);
                activeSkin = (BiomeSkinSO)EditorGUILayout.ObjectField("Biome Skin:", activeSkin, typeof(BiomeSkinSO), false);
                activeSeed = EditorGUILayout.IntField("Active Seed:", activeSeed);
            }
            EditorGUILayout.Space(6);

            // Brush Settings
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                toolMode = (VisualBrushToolMode)EditorGUILayout.EnumPopup("Tool Mode:", toolMode);
                brushSize = EditorGUILayout.Slider("Brush Size (px):", brushSize, 8f, 512f);
                brushDensity = EditorGUILayout.Slider("Brush Density:", brushDensity, 0.05f, 1.0f);
            }
            EditorGUILayout.Space(6);

            // Main Brush Palette inside the Dockable Window
            DrawMainPalettePanel();

            // Status Bar
            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.Label(selectedBrush != null ? $"Active Brush: {selectedBrush.brushName}" : "No brush selected.");
            }
        }

        private void EnsureInitialized()
        {
            if (scenePainter == null) scenePainter = new VisualEditorScenePainter(this);
        }

        private void DrawMainPalettePanel()
        {
            if (activePack == null)
            {
                EditorGUILayout.HelpBox("Assign a Brush Pack in the settings above to see available brushes.", MessageType.Info);
                return;
            }

            GUILayout.Label("Brushes", EditorStyles.boldLabel);
            
            // Category Selector
            string[] categories = { "All", "Floor", "Wall", "Cliff", "Transition", "Detail" };
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (var cat in categories)
                {
                    bool isSelected = selectedCategory == cat;
                    if (GUILayout.Toggle(isSelected, cat, EditorStyles.miniButton, GUILayout.ExpandWidth(true)))
                    {
                        selectedCategory = cat;
                    }
                }
            }
            EditorGUILayout.Space(4);

            // Scroll view grid of brushes
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            using (new EditorGUILayout.VerticalScope())
            {
                int count = 0;
                EditorGUILayout.BeginHorizontal();

                foreach (var brush in activePack.brushes)
                {
                    if (brush == null) continue;

                    // Apply category filtering
                    if (selectedCategory != "All" && brush.category.ToString() != selectedCategory)
                        continue;

                    // Generate icon preview
                    Texture2D icon = null;
                    if (brush.previewIcon != null)
                    {
                        icon = AssetPreview.GetAssetPreview(brush.previewIcon);
                    }
                    if (icon == null)
                    {
                        icon = AssetPreview.GetAssetPreview(brush);
                    }

                    GUIContent buttonContent = new GUIContent(brush.brushName, icon);
                    
                    // Button cell
                    bool isSelected = selectedBrush == brush;
                    GUIStyle style = new GUIStyle(GUI.skin.button);
                    if (isSelected)
                    {
                        style.normal.background = style.active.background;
                        style.normal.textColor = Color.cyan;
                    }
                    style.imagePosition = ImagePosition.ImageAbove;
                    
                    if (GUILayout.Button(buttonContent, style, GUILayout.Width(110), GUILayout.Height(110)))
                    {
                        selectedBrush = brush;
                    }

                    count++;
                    if (count % 3 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        private void OnSceneGUIHook(SceneView sceneView)
        {
            if (scenePainter == null) scenePainter = new VisualEditorScenePainter(this);
            scenePainter.OnSceneGUI(sceneView);
            
            // Draw floating SceneView Palette Overlay
            DrawSceneViewOverlay(sceneView);
        }

        private void DrawSceneViewOverlay(SceneView sceneView)
        {
            Handles.BeginGUI();
            
            // Layout dimensions for overlay
            float width = 230f;
            float height = 300f;
            Rect overlayRect = new Rect(10, 30, width, height);

            // Background box styling
            GUI.Box(overlayRect, GUIContent.none, GetOverlayStyle());

            // Overlay Area
            GUILayout.BeginArea(new Rect(20, 40, width - 20, height - 20));
            
            GUILayout.Label("RIMA Visual Editor Overlay", GetOverlayHeaderStyle());
            GUILayout.Space(8);

            // Mode Selector
            using (new GUILayout.HorizontalScope())
            {
                bool isBrush = toolMode == VisualBrushToolMode.Brush;
                if (GUILayout.Toggle(isBrush, "Brush", EditorStyles.miniButtonLeft, GUILayout.Height(24)))
                {
                    toolMode = VisualBrushToolMode.Brush;
                }
                bool isErase = toolMode == VisualBrushToolMode.Erase;
                if (GUILayout.Toggle(isErase, "Erase", EditorStyles.miniButtonRight, GUILayout.Height(24)))
                {
                    toolMode = VisualBrushToolMode.Erase;
                }
            }
            GUILayout.Space(8);

            // Parameters Adjustments
            GUILayout.Label($"Size: {brushSize:F0}px (Scroll to change)", GetOverlayTextStyle());
            brushSize = GUILayout.HorizontalSlider(brushSize, 8f, 512f);
            
            GUILayout.Label($"Density: {brushDensity:F2}", GetOverlayTextStyle());
            brushDensity = GUILayout.HorizontalSlider(brushDensity, 0.05f, 1.0f);
            
            GUILayout.Space(8);

            // Active brush overview
            if (selectedBrush != null)
            {
                GUILayout.Label($"Selected: {selectedBrush.brushName}", GetOverlayBoldTextStyle());
                GUILayout.Label($"Type: {selectedBrush.category}", GetOverlayTextStyle());
                
                // Active preview icon inside overlay
                Texture2D icon = null;
                if (selectedBrush.previewIcon != null)
                {
                    icon = AssetPreview.GetAssetPreview(selectedBrush.previewIcon);
                }
                if (icon != null)
                {
                    GUILayout.Label(new GUIContent(icon), GUILayout.Width(64), GUILayout.Height(64));
                }
            }
            else
            {
                GUILayout.Label("No brush selected. Select from dock window.", GetOverlayTextStyle());
            }

            GUILayout.EndArea();
            Handles.EndGUI();
            
            // Intercept mouse scrolls in SceneView to change brush size dynamically
            Event e = Event.current;
            if (e.type == EventType.ScrollWheel && !e.alt)
            {
                brushSize = Mathf.Clamp(brushSize - e.delta.y * 8f, 8f, 512f);
                e.Use();
            }
        }

        private GUIStyle GetOverlayStyle()
        {
            GUIStyle style = new GUIStyle();
            Texture2D bg = new Texture2D(1, 1);
            bg.SetPixel(0, 0, new Color(0.12f, 0.12f, 0.14f, 0.92f));
            bg.Apply();
            style.normal.background = bg;
            return style;
        }

        private GUIStyle GetOverlayHeaderStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = Color.white;
            style.fontSize = 13;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }

        private GUIStyle GetOverlayTextStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
            style.fontSize = 11;
            return style;
        }

        private GUIStyle GetOverlayBoldTextStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = Color.cyan;
            style.fontSize = 11;
            return style;
        }
    }
}
#endif
