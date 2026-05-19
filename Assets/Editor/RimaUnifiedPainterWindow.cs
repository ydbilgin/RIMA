#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Systems.Map;
using RIMA.Editor;

namespace RIMA.Editor.MapDesigner
{
    public class RimaUnifiedPainterWindow : EditorWindow
    {
        public enum PaletteCategory { Floor, Wall, Prop, Mob }
        public enum ToolMode { Paint, Erase, Eyedropper }
        public enum CollisionMode { Auto, Passable, SmallFootprint, FullFootprint, WallBlock, Custom }

        [System.Serializable]
        public class ScanResult
        {
            public string assetPath;
            public string displayName;
            public GameObject prefab;
            public TileBase tile;
            public Sprite icon;
        }

        [SerializeField] private PaletteCategory currentCategory = PaletteCategory.Floor;
        [SerializeField] private ToolMode currentTool = ToolMode.Paint;
        [SerializeField] private string searchQuery = string.Empty;
        [SerializeField] private RimaBiomePreset activeBiome;
        [SerializeField] private Tilemap targetTilemap;
        [SerializeField] private Transform targetParent;
        [SerializeField] private int prefabRotation = 0; // 0=0, 1=90, 2=180, 3=270
        [SerializeField] private bool snapToGrid = true;
        [SerializeField] private int brushSize = 1;
        [SerializeField] private float prefabScaleMultiplier = 0.5f;
        [SerializeField] private CollisionMode customCollisionMode = CollisionMode.Auto;
        [SerializeField] private Vector2 customColliderSize = new Vector2(1f, 1f);
        [SerializeField] private Vector2 customColliderOffset = Vector2.zero;
        [SerializeField] private bool autoAlignBaseToGrid = true;
        [SerializeField] private Vector3 positionOffset = Vector3.zero;

        // Selection state
        private TileBase selectedTile;
        private GameObject selectedPrefab;
        private string selectedAssetName = "None";
        private Sprite selectedAssetIcon;

        // Scanned assets lists
        private List<ScanResult> floorTiles = new List<ScanResult>();
        private List<ScanResult> wallPrefabs = new List<ScanResult>();
        private List<ScanResult> propPrefabs = new List<ScanResult>();
        private List<ScanResult> mobPrefabs = new List<ScanResult>();

        // Preview rendering state
        private GameObject previewObject;
        private GameObject lastPreviewPrefab;
        private int lastPreviewRotation;
        private Vector3 lastMouseWorldPos;
        private Vector3Int lastCellPos;
        private bool isMouseInSceneView;

        // UI state
        private Vector2 paletteScroll;
        private Vector2 optionsScroll;
        private Dictionary<int, Texture2D> previewCache = new Dictionary<int, Texture2D>();

        [MenuItem("RIMA/Tools/Unified Painter", false, 10)]
        public static void Open()
        {
            var window = GetWindow<RimaUnifiedPainterWindow>("Unified Painter");
            window.minSize = new Vector2(850f, 500f);
            window.Show();
        }

        private void OnEnable()
        {
            // Register callbacks
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;

            // Auto scan assets
            ScanAllAssets();
            TryAutoAssignTargets();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            EditorApplication.update -= OnEditorUpdate;
            ClearPreviewObject();
        }

        private void OnDestroy()
        {
            ClearPreviewObject();
        }

        private void OnEditorUpdate()
        {
            // Auto repaint if asset previews are loading
            if (AssetPreview.IsLoadingAssetPreviews())
            {
                Repaint();
            }
        }

        private void ScanAllAssets()
        {
            // 1. Scan Biome for Floors
            floorTiles.Clear();
            if (activeBiome == null)
            {
                // Try load Shattered Keep biome preset as default
                activeBiome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset");
            }

            if (activeBiome != null && activeBiome.terrains != null)
            {
                foreach (var terrain in activeBiome.terrains)
                {
                    if (terrain == null) continue;
                    
                    if (terrain.baseTile != null)
                    {
                        AddTileToScanList(terrain.baseTile, terrain.name);
                    }
                    if (terrain.variantPool != null)
                    {
                        for (int i = 0; i < terrain.variantPool.Count; i++)
                        {
                            if (terrain.variantPool[i] != null)
                            {
                                AddTileToScanList(terrain.variantPool[i], $"{terrain.name} (Var {i+1})");
                            }
                        }
                    }
                }
            }

            // 2. Scan Wall Prefabs (wall_*)
            wallPrefabs.Clear();
            ScanPrefabsInFolder("Assets/Prefabs/Props/ShatteredKeep_PixelLab", "wall_", wallPrefabs);

            // 3. Scan Prop Prefabs (mounting_*, statue_*)
            propPrefabs.Clear();
            ScanPrefabsInFolder("Assets/Prefabs/Props/ShatteredKeep_PixelLab", "mounting_", propPrefabs);
            ScanPrefabsInFolder("Assets/Prefabs/Props/ShatteredKeep_PixelLab", "statue_", propPrefabs);

            // 4. Scan Mob Prefabs (enemy_*)
            mobPrefabs.Clear();
            ScanPrefabsInFolder("Assets/Prefabs/Mobs/ShatteredKeep_PixelLab", "enemy_", mobPrefabs);
        }

        private void AddTileToScanList(TileBase tile, string name)
        {
            if (tile == null) return;
            if (floorTiles.Any(x => x.tile == tile)) return;

            Sprite s = (tile as Tile)?.sprite;
            floorTiles.Add(new ScanResult
            {
                assetPath = AssetDatabase.GetAssetPath(tile),
                displayName = name,
                tile = tile,
                icon = s
            });
        }

        private void ScanPrefabsInFolder(string folderPath, string prefixFilter, List<ScanResult> targetList)
        {
            if (!AssetDatabase.IsValidFolder(folderPath)) return;

            string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string filename = Path.GetFileName(path).ToLower();
                
                if (filename.StartsWith(prefixFilter.ToLower()) && filename.EndsWith(".prefab"))
                {
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (go != null)
                    {
                        var sr = go.GetComponentInChildren<SpriteRenderer>();
                        Sprite s = sr != null ? sr.sprite : null;

                        // Check if already exists
                        if (targetList.Any(x => x.prefab == go)) continue;

                        targetList.Add(new ScanResult
                        {
                            assetPath = path,
                            displayName = CleanName(go.name),
                            prefab = go,
                            icon = s
                        });
                    }
                }
            }
        }

        private string CleanName(string raw)
        {
            // Remove guid suffixes like mounting_00_7227fa35-ade1-...
            int firstUnderscore = raw.IndexOf('_');
            if (firstUnderscore < 0) return raw;
            int secondUnderscore = raw.IndexOf('_', firstUnderscore + 1);
            if (secondUnderscore < 0) return raw;

            // Check if there is a third part
            int thirdUnderscore = raw.IndexOf('_', secondUnderscore + 1);
            if (thirdUnderscore > 0)
            {
                return raw.Substring(0, thirdUnderscore);
            }
            return raw.Substring(0, secondUnderscore);
        }

        private void TryAutoAssignTargets()
        {
            // Try to find open RimaMapDesignerWindow and read fields via reflection
            var windowType = typeof(RimaMapDesignerWindow);
            var windows = Resources.FindObjectsOfTypeAll(windowType);
            if (windows.Length > 0)
            {
                var mapDesigner = windows[0] as EditorWindow;
                if (mapDesigner != null)
                {
                    var biomeField = windowType.GetField("activeBiome", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (biomeField != null)
                    {
                        var biomeVal = biomeField.GetValue(mapDesigner) as RimaBiomePreset;
                        if (biomeVal != null) activeBiome = biomeVal;
                    }

                    var tilemapField = windowType.GetField("outputTilemap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (tilemapField != null)
                    {
                        var tilemapVal = tilemapField.GetValue(mapDesigner) as Tilemap;
                        if (tilemapVal != null) targetTilemap = tilemapVal;
                    }
                }
            }

            // Fallbacks in scene
            if (targetTilemap == null)
            {
                targetTilemap = GameObject.FindObjectsOfType<Tilemap>().FirstOrDefault(x => x.name.Contains("Floor") || x.name.Contains("Zemin"));
            }

            if (targetTilemap != null && targetParent == null)
            {
                targetParent = targetTilemap.transform.parent;
            }
        }

        private void OnGUI()
        {
            // Ensure style and scanning is updated
            if (floorTiles.Count == 0 && activeBiome != null)
            {
                ScanAllAssets();
            }

            DrawHeader();

            using (new EditorGUILayout.HorizontalScope())
            {
                // Left Column: Options, Snapping, Targets
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(260f)))
                {
                    DrawOptionsPanel();
                }

                // Right Column: Search + Palette Grid
                using (new EditorGUILayout.VerticalScope())
                {
                    DrawPalettePanel();
                }
            }
        }

        private void DrawHeader()
        {
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                margin = new RectOffset(6, 6, 8, 8),
                alignment = TextAnchor.MiddleLeft
            };

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("RIMA Unified Level Designer & Painter", headerStyle);
                GUILayout.FlexibleSpace();
                
                // Active selection preview
                using (new EditorGUILayout.HorizontalScope(GUILayout.Height(40f)))
                {
                    GUILayout.Label("Active Brush:", EditorStyles.miniBoldLabel, GUILayout.Height(30f));
                    if (selectedAssetIcon != null)
                    {
                        Rect iconRect = GUILayoutUtility.GetRect(32f, 32f, GUILayout.Width(32f), GUILayout.Height(32f));
                        GUI.DrawTexture(iconRect, selectedAssetIcon.texture, ScaleMode.ScaleToFit, true);
                    }
                    GUILayout.Label(selectedAssetName, EditorStyles.boldLabel, GUILayout.Height(30f));
                }
            }
        }

        private void DrawOptionsPanel()
        {
            optionsScroll = EditorGUILayout.BeginScrollView(optionsScroll, EditorStyles.helpBox);

            // Category Selection (Zeminler, Duvarlar, Objeler, Canavarlar)
            EditorGUILayout.LabelField("Category", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawCategoryButton("Zemin", PaletteCategory.Floor);
                DrawCategoryButton("Duvar", PaletteCategory.Wall);
                DrawCategoryButton("Obje", PaletteCategory.Prop);
                DrawCategoryButton("Canavar", PaletteCategory.Mob);
            }
            EditorGUILayout.Space(10);

            // Targets settings
            EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();
            activeBiome = (RimaBiomePreset)EditorGUILayout.ObjectField("Biome Preset", activeBiome, typeof(RimaBiomePreset), false);
            if (EditorGUI.EndChangeCheck())
            {
                ScanAllAssets();
            }

            targetTilemap = (Tilemap)EditorGUILayout.ObjectField("Target Tilemap", targetTilemap, typeof(Tilemap), true);
            targetParent = (Transform)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(Transform), true);
            EditorGUILayout.Space(10);

            // Tools Panel
            EditorGUILayout.LabelField("Painting Tools", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawToolButton("Brush (B)", ToolMode.Paint);
                DrawToolButton("Erase (E)", ToolMode.Erase);
                DrawToolButton("Eyedropper (I)", ToolMode.Eyedropper);
            }
            EditorGUILayout.Space(10);

            // Snapping & Sizing
            EditorGUILayout.LabelField("Tool Settings", EditorStyles.boldLabel);
            snapToGrid = EditorGUILayout.Toggle("Snap to Grid", snapToGrid);
            
            if (currentCategory == PaletteCategory.Floor)
            {
                brushSize = EditorGUILayout.IntSlider("Brush Size", brushSize, 1, 5);
            }
            else
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Rotation (R)", GUILayout.Width(80f));
                    if (GUILayout.Button("Rot L ↺", GUILayout.Width(60f)))
                    {
                        prefabRotation = (prefabRotation + 3) % 4;
                    }
                    GUILayout.Label($"{prefabRotation * 90}°", EditorStyles.centeredGreyMiniLabel);
                    if (GUILayout.Button("Rot R ↻", GUILayout.Width(60f)))
                    {
                        prefabRotation = (prefabRotation + 1) % 4;
                    }
                }

                EditorGUILayout.Space(5);
                prefabScaleMultiplier = EditorGUILayout.Slider("Scale Multiplier", prefabScaleMultiplier, 0.1f, 3.0f);

                EditorGUILayout.Space(5);
                autoAlignBaseToGrid = EditorGUILayout.Toggle("Auto-Align Base", autoAlignBaseToGrid);
                positionOffset = EditorGUILayout.Vector3Field("Position Offset", positionOffset);
                
                if (currentCategory == PaletteCategory.Prop || currentCategory == PaletteCategory.Wall)
                {
                    EditorGUILayout.Space(5);
                    customCollisionMode = (CollisionMode)EditorGUILayout.EnumPopup("Collision Mode", customCollisionMode);
                    if (customCollisionMode == CollisionMode.Custom)
                    {
                        EditorGUI.indentLevel++;
                        customColliderSize = EditorGUILayout.Vector2Field("Custom Size", customColliderSize);
                        customColliderOffset = EditorGUILayout.Vector2Field("Custom Offset", customColliderOffset);
                        EditorGUI.indentLevel--;
                    }
                }
            }
            EditorGUILayout.Space(15);

            // Status message
            if (targetTilemap == null)
            {
                EditorGUILayout.HelpBox("Assign a Target Tilemap to start painting tiles.", MessageType.Warning);
            }
            else if (targetParent == null)
            {
                EditorGUILayout.HelpBox("Assign a Target Parent to organize spawned prefabs.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Use standard keys in Scene View:\n- R: Rotate prefab\n- B: Brush Tool\n- E: Erase Tool\n- I: Eyedropper", MessageType.Info);
            }

            if (GUILayout.Button("Rescan & Sync Assets", GUILayout.Height(30f)))
            {
                ScanAllAssets();
                TryAutoAssignTargets();
            }

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Setup Asset Pack Colliders", GUILayout.Height(26f)))
            {
                if (EditorUtility.DisplayDialog("Setup Asset Pack Colliders?", 
                    "This will automatically configure colliders for all prefabs in ShatteredKeep_PixelLab:\n\n- wall_* & statue_* will get BoxCollider2D (Blocking)\n- mounting_* will have colliders removed (Passable)\n\nAre you sure you want to proceed?", "Yes", "No"))
                {
                    ConfigureAssetPackColliders();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawCategoryButton(string label, PaletteCategory category)
        {
            bool isSelected = currentCategory == category;
            Color prevBg = GUI.backgroundColor;
            if (isSelected) GUI.backgroundColor = new Color(0.2f, 0.6f, 1f, 1f);
            
            if (GUILayout.Button(label, EditorStyles.miniButton, GUILayout.Height(24f)))
            {
                currentCategory = category;
                ClearSelection();
            }
            GUI.backgroundColor = prevBg;
        }

        private void DrawToolButton(string label, ToolMode tool)
        {
            bool isSelected = currentTool == tool;
            Color prevBg = GUI.backgroundColor;
            if (isSelected) GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
            
            if (GUILayout.Button(label, EditorStyles.miniButton, GUILayout.Height(26f)))
            {
                currentTool = tool;
            }
            GUI.backgroundColor = prevBg;
        }

        private void ClearSelection()
        {
            selectedTile = null;
            selectedPrefab = null;
            selectedAssetName = "None";
            selectedAssetIcon = null;
            ClearPreviewObject();
        }

        private void DrawPalettePanel()
        {
            // Search field
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Filter Search:", GUILayout.Width(80f));
                searchQuery = EditorGUILayout.TextField(searchQuery, EditorStyles.toolbarSearchField);
                if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(45f)))
                {
                    searchQuery = string.Empty;
                }
            }

            paletteScroll = EditorGUILayout.BeginScrollView(paletteScroll, EditorStyles.helpBox);

            List<ScanResult> activeList = null;
            switch (currentCategory)
            {
                case PaletteCategory.Floor: activeList = floorTiles; break;
                case PaletteCategory.Wall: activeList = wallPrefabs; break;
                case PaletteCategory.Prop: activeList = propPrefabs; break;
                case PaletteCategory.Mob: activeList = mobPrefabs; break;
            }

            if (activeList == null || activeList.Count == 0)
            {
                EditorGUILayout.HelpBox("No assets found in this category. Make sure paths and filters match.", MessageType.Info);
                EditorGUILayout.EndScrollView();
                return;
            }

            // Filter the list based on query
            var filtered = activeList.Where(x => string.IsNullOrEmpty(searchQuery) || x.displayName.ToLower().Contains(searchQuery.ToLower())).ToList();

            // Draw items in responsive Grid
            float viewWidth = position.width - 290f; // subtract options panel width
            float itemWidth = 92f;
            float itemHeight = 110f;
            int columns = Mathf.Max(1, Mathf.FloorToInt(viewWidth / itemWidth));

            int column = 0;
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < filtered.Count; i++)
            {
                var result = filtered[i];
                DrawPaletteItemButton(result, itemWidth, itemHeight);

                column++;
                if (column >= columns)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    column = 0;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }

        private void DrawPaletteItemButton(ScanResult item, float width, float height)
        {
            Rect btnRect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
            
            bool isSelected = false;
            if (currentCategory == PaletteCategory.Floor)
            {
                isSelected = selectedTile == item.tile;
            }
            else
            {
                isSelected = selectedPrefab == item.prefab;
            }

            bool hover = btnRect.Contains(Event.current.mousePosition);
            Color hoverColor = hover ? new Color(1f, 1f, 1f, 0.1f) : Color.clear;
            EditorGUI.DrawRect(btnRect, isSelected ? new Color(0.2f, 0.5f, 0.9f, 0.25f) : hoverColor);

            // Thumbnail Rect
            Rect thumbRect = new Rect(btnRect.x + 8f, btnRect.y + 6f, width - 16f, width - 16f);
            Texture2D thumbnail = null;

            if (currentCategory == PaletteCategory.Floor && item.tile != null)
            {
                thumbnail = GetCachedPreview(item.tile);
            }
            else if (item.prefab != null)
            {
                thumbnail = GetCachedPreview(item.prefab);
            }

            if (thumbnail != null)
            {
                GUI.DrawTexture(thumbRect, thumbnail, ScaleMode.ScaleToFit);
            }
            else if (item.icon != null && item.icon.texture != null)
            {
                Rect tc = new Rect(
                    item.icon.rect.x / item.icon.texture.width,
                    item.icon.rect.y / item.icon.texture.height,
                    item.icon.rect.width / item.icon.texture.width,
                    item.icon.rect.height / item.icon.texture.height);
                GUI.DrawTextureWithTexCoords(thumbRect, item.icon.texture, tc);
            }
            else
            {
                EditorGUI.DrawRect(thumbRect, new Color(0.2f, 0.2f, 0.2f));
                GUI.Label(thumbRect, "No Image", EditorStyles.centeredGreyMiniLabel);
            }

            // Outline
            Handles.BeginGUI();
            Handles.color = isSelected ? new Color(0f, 0.7f, 1f, 0.8f) : (hover ? new Color(1f, 1f, 1f, 0.3f) : new Color(0.25f, 0.25f, 0.25f, 0.5f));
            Handles.DrawSolidRectangleWithOutline(btnRect, Color.clear, Handles.color);
            if (isSelected)
            {
                Rect innerRect = new Rect(btnRect.x + 1f, btnRect.y + 1f, btnRect.width - 2f, btnRect.height - 2f);
                Handles.DrawSolidRectangleWithOutline(innerRect, Color.clear, new Color(0f, 0.7f, 1f, 0.8f));
            }
            Handles.EndGUI();

            // Label Rect
            Rect labelRect = new Rect(btnRect.x + 4f, btnRect.y + height - 24f, btnRect.width - 8f, 20f);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                clipping = TextClipping.Clip
            };
            GUI.Label(labelRect, item.displayName, labelStyle);

            // Handle Selection click
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && hover)
            {
                if (currentCategory == PaletteCategory.Floor)
                {
                    selectedTile = item.tile;
                    selectedPrefab = null;
                }
                else
                {
                    selectedPrefab = item.prefab;
                    selectedTile = null;
                }
                selectedAssetName = item.displayName;
                selectedAssetIcon = item.icon;
                customCollisionMode = CollisionMode.Auto;
                
                // Automatically switch to Paint mode if Eyedropper selected
                if (currentTool == ToolMode.Eyedropper)
                {
                    currentTool = ToolMode.Paint;
                }

                ClearPreviewObject();
                Event.current.Use();
                Repaint();
            }
        }

        private Texture2D GetCachedPreview(UnityEngine.Object asset)
        {
            if (asset == null) return null;
            int id = asset.GetInstanceID();
            
            if (previewCache.TryGetValue(id, out Texture2D tex) && tex != null)
            {
                return tex;
            }

            tex = AssetPreview.GetAssetPreview(asset);
            if (tex != null)
            {
                previewCache[id] = tex;
            }
            return tex;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            // Intercept hotkeys
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.R && !e.alt && !e.control)
                {
                    prefabRotation = (prefabRotation + 1) % 4;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.B && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Paint;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.E && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Erase;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.I && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Eyedropper;
                    e.Use();
                    Repaint();
                }
            }

            // Check if there is an active brush, or if we are in Erase / Eyedropper mode
            bool hasActiveBrush = (currentCategory == PaletteCategory.Floor && selectedTile != null) ||
                                  (currentCategory != PaletteCategory.Floor && selectedPrefab != null);

            if (!hasActiveBrush && currentTool != ToolMode.Erase && currentTool != ToolMode.Eyedropper)
            {
                ClearPreviewObject();
                return;
            }

            // Disable standard Unity selection click
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            // Get snapped cell and center positions
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Grid grid = GameObject.FindObjectOfType<Grid>();
            
            Vector3 worldPos = Vector3.zero;
            Vector3 gridNormal = Vector3.forward;
            Vector3 gridPoint = Vector3.zero;
            if (targetTilemap != null)
            {
                gridNormal = targetTilemap.transform.forward;
                gridPoint = targetTilemap.transform.position;
            }
            else if (grid != null)
            {
                gridNormal = grid.transform.forward;
                gridPoint = grid.transform.position;
            }
            
            Plane gridPlane = new Plane(gridNormal, gridPoint);
            if (gridPlane.Raycast(ray, out float enter))
            {
                worldPos = ray.GetPoint(enter);
            }
            else
            {
                worldPos = new Vector3(ray.origin.x, ray.origin.y, 0f);
            }

            Vector3Int cellPos;
            Vector3 snapPos;
            GetCellAndSnapPos(worldPos, out cellPos, out snapPos);

            // Draw scene previews/outlines
            if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Floor && selectedTile != null)
            {
                ClearPreviewObject();
                DrawCellOutline(targetTilemap, grid, cellPos, new Color(0.2f, 0.8f, 1f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Wall && selectedPrefab != null)
            {
                UpdatePreviewObject(selectedPrefab, snapPos, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos, prefabRotation, new Color(0.2f, 1f, 0.35f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Prop && selectedPrefab != null)
            {
                UpdatePreviewObject(selectedPrefab, snapPos, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos, prefabRotation, new Color(0.2f, 1f, 0.35f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Mob && selectedPrefab != null)
            {
                UpdatePreviewObject(selectedPrefab, snapPos, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos, prefabRotation, new Color(1f, 0.4f, 0.1f, 0.8f));
            }
            else if (currentTool == ToolMode.Erase)
            {
                ClearPreviewObject();
                DrawCellOutline(targetTilemap, grid, cellPos, new Color(1f, 0.2f, 0.2f, 0.8f));
            }
            else if (currentTool == ToolMode.Eyedropper)
            {
                ClearPreviewObject();
                DrawCellOutline(targetTilemap, grid, cellPos, new Color(1f, 0.85f, 0f, 0.8f));
            }

            // Paint/Erase actions
            if (e.button == 0 && !e.alt && !e.control)
            {
                if (e.type == EventType.MouseDown)
                {
                    GUIUtility.hotControl = controlId;
                    PerformAction(cellPos, snapPos);
                    e.Use();
                }
                else if (e.type == EventType.MouseDrag && GUIUtility.hotControl == controlId)
                {
                    PerformAction(cellPos, snapPos);
                    e.Use();
                }
                else if (e.type == EventType.MouseUp && GUIUtility.hotControl == controlId)
                {
                    GUIUtility.hotControl = 0;
                    e.Use();
                }
            }

            if (e.type == EventType.Repaint)
            {
                SceneView.RepaintAll();
            }
        }

        private void GetCellAndSnapPos(Vector3 worldPos, out Vector3Int cellPos, out Vector3 snapPos)
        {
            if (targetTilemap != null)
            {
                Vector3 localPos = targetTilemap.transform.InverseTransformPoint(worldPos);
                cellPos = targetTilemap.LocalToCell(localPos);
                Vector3 localCenter = targetTilemap.GetCellCenterLocal(cellPos);
                snapPos = targetTilemap.transform.TransformPoint(localCenter);
            }
            else
            {
                Grid grid = GameObject.FindObjectOfType<Grid>();
                if (grid != null)
                {
                    Vector3 localPos = grid.transform.InverseTransformPoint(worldPos);
                    cellPos = grid.LocalToCell(localPos);
                    Vector3 localCenter = grid.GetCellCenterLocal(cellPos);
                    snapPos = grid.transform.TransformPoint(localCenter);
                }
                else
                {
                    cellPos = new Vector3Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), 0);
                    snapPos = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0f);
                }
            }
        }

        private void DrawCellOutline(Tilemap tilemap, Grid grid, Vector3Int cellPos, Color color)
        {
            Vector3 worldC0, worldC1, worldC2, worldC3;

            if (tilemap != null)
            {
                worldC0 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos));
                worldC1 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                worldC2 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                worldC3 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else if (grid != null)
            {
                worldC0 = grid.transform.TransformPoint(grid.CellToLocal(cellPos));
                worldC1 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                worldC2 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                worldC3 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else
            {
                worldC0 = new Vector3(cellPos.x, cellPos.y, 0f);
                worldC1 = new Vector3(cellPos.x + 1f, cellPos.y, 0f);
                worldC2 = new Vector3(cellPos.x + 1f, cellPos.y + 1f, 0f);
                worldC3 = new Vector3(cellPos.x, cellPos.y + 1f, 0f);
            }

            Handles.color = color;
            Handles.DrawAAPolyLine(3f, worldC0, worldC1, worldC2, worldC3, worldC0);

            Color fillCol = color;
            fillCol.a = 0.15f;
            Handles.DrawSolidRectangleWithOutline(new[] { worldC0, worldC1, worldC2, worldC3 }, fillCol, Color.clear);
        }

        private void DrawBoxOutline(Tilemap tilemap, Grid grid, Vector3Int cellPos, Color color, float height)
        {
            Vector3 b0, b1, b2, b3;

            if (tilemap != null)
            {
                b0 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos));
                b1 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                b2 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                b3 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else if (grid != null)
            {
                b0 = grid.transform.TransformPoint(grid.CellToLocal(cellPos));
                b1 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                b2 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                b3 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else
            {
                b0 = new Vector3(cellPos.x, cellPos.y, 0f);
                b1 = new Vector3(cellPos.x + 1f, cellPos.y, 0f);
                b2 = new Vector3(cellPos.x + 1f, cellPos.y + 1f, 0f);
                b3 = new Vector3(cellPos.x, cellPos.y + 1f, 0f);
            }

            Vector3 offset = new Vector3(0f, height, 0f);
            Vector3 t0 = b0 + offset;
            Vector3 t1 = b1 + offset;
            Vector3 t2 = b2 + offset;
            Vector3 t3 = b3 + offset;

            Handles.color = color;
            
            // Draw base diamond (dotted)
            Handles.DrawDottedLine(b0, b1, 2f);
            Handles.DrawDottedLine(b1, b2, 2f);
            Handles.DrawDottedLine(b2, b3, 2f);
            Handles.DrawDottedLine(b3, b0, 2f);

            // Draw top diamond (solid)
            Handles.DrawAAPolyLine(3f, t0, t1, t2, t3, t0);

            // Draw vertical pillars
            Handles.DrawAAPolyLine(2f, b0, t0);
            Handles.DrawAAPolyLine(2f, b1, t1);
            Handles.DrawAAPolyLine(2f, b2, t2);
            Handles.DrawAAPolyLine(2f, b3, t3);

            // Draw transparent fill on the top face
            Color topFill = color;
            topFill.a = 0.1f;
            Handles.DrawSolidRectangleWithOutline(new[] { t0, t1, t2, t3 }, topFill, Color.clear);
            
            // Draw transparent fill on the side faces facing the camera
            Color sideFill = color;
            sideFill.a = 0.05f;
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b1, t1, t0 }, sideFill, Color.clear);
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b3, t3, t0 }, sideFill, Color.clear);
        }

        private void UpdatePreviewObject(GameObject sourcePrefab, Vector3 position, int rotationSteps)
        {
            if (sourcePrefab == null)
            {
                ClearPreviewObject();
                return;
            }

            if (previewObject == null || lastPreviewPrefab != sourcePrefab)
            {
                ClearPreviewObject();
                previewObject = Instantiate(sourcePrefab);
                previewObject.name = "__RimaUnifiedPainter_Preview__";
                previewObject.hideFlags = HideFlags.HideAndDontSave;

                // Strip non-visual gameplay scripts/colliders
                var components = previewObject.GetComponentsInChildren<Component>(true);
                foreach (var comp in components)
                {
                    if (comp == null) continue;
                    if (comp is Transform || comp is Renderer || comp is SpriteRenderer || comp is MeshFilter || comp is MeshRenderer)
                    {
                        continue;
                    }
                    DestroyImmediate(comp);
                }

                // Opacity reduction for ghost effect and set sorting order
                int previewSortingOrder = 0;
                if (currentCategory == PaletteCategory.Wall) previewSortingOrder = 20;
                else if (currentCategory == PaletteCategory.Prop) previewSortingOrder = 30;
                else if (currentCategory == PaletteCategory.Mob) previewSortingOrder = 40;

                var renderers = previewObject.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var r in renderers)
                {
                    Color col = r.color;
                    col.a = 0.55f;
                    r.color = col;
                    
                    if (r.sortingOrder == 0)
                    {
                        r.sortingOrder = previewSortingOrder;
                    }
                }

                lastPreviewPrefab = sourcePrefab;
            }

            previewObject.transform.position = position;
            previewObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationSteps * 90f);
            
            // In the world, we want the preview to be at original prefab world size (un-squashed)
            previewObject.transform.localScale = sourcePrefab.transform.localScale * prefabScaleMultiplier;
            
            lastPreviewRotation = rotationSteps;
        }

        private void ClearPreviewObject()
        {
            if (previewObject != null)
            {
                DestroyImmediate(previewObject);
                previewObject = null;
            }
            lastPreviewPrefab = null;
        }

        private void PerformAction(Vector3Int cellPos, Vector3 snapPos)
        {
            if (currentTool == ToolMode.Paint)
            {
                if (currentCategory == PaletteCategory.Floor && selectedTile != null)
                {
                    PaintTile(cellPos, selectedTile);
                }
                else if (selectedPrefab != null)
                {
                    // For prefabs, we place on MouseDown (not drag, to avoid spawning duplicate overlap prefabs in the same cell)
                    if (Event.current.type == EventType.MouseDown)
                    {
                        PaintPrefab(snapPos, selectedPrefab);
                    }
                }
            }
            else if (currentTool == ToolMode.Erase)
            {
                EraseAt(cellPos, snapPos);
            }
            else if (currentTool == ToolMode.Eyedropper)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    PerformEyedropper(cellPos, snapPos);
                }
            }
        }

        private void PaintTile(Vector3Int cellPos, TileBase tile)
        {
            if (targetTilemap == null) return;

            // Brush size loop
            int half = brushSize / 2;
            int oddOffset = brushSize % 2 == 0 ? 0 : 0;
            
            Undo.RegisterCompleteObjectUndo(targetTilemap, "Paint Tiles");

            for (int dx = -half; dx <= half + oddOffset; dx++)
            {
                for (int dy = -half; dy <= half + oddOffset; dy++)
                {
                    Vector3Int pos = cellPos + new Vector3Int(dx, dy, 0);
                    targetTilemap.SetTile(pos, tile);
                }
            }
            EditorUtility.SetDirty(targetTilemap);
        }

        private void PaintPrefab(Vector3 snapPos, GameObject prefab)
        {
            Transform parent = GetTargetParent();
            
            // Check if there is already an object placed in the exact cell to avoid stacking
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (Vector3.Distance(child.position, snapPos) < 0.1f)
                    {
                        return; // Already occupied
                    }
                }
            }

            GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (parent != null)
            {
                placed.transform.SetParent(parent, false);
            }
            placed.transform.position = snapPos;
            placed.transform.rotation = Quaternion.Euler(0f, 0f, prefabRotation * 90f);
            
            // Un-squash localScale relative to parent's lossyScale to maintain the prefab's intended world scale
            if (parent != null)
            {
                placed.transform.localScale = ComputeCompensatedLocalScale(
                    parent,
                    prefab.transform.localScale * prefabScaleMultiplier,
                    prefabRotation * 90f
                );
            }
            else
            {
                placed.transform.localScale = prefab.transform.localScale * prefabScaleMultiplier;
            }

            // Logical Walkability & Collider Automation
            CollisionMode activeMode = customCollisionMode;
            if (activeMode == CollisionMode.Auto)
            {
                activeMode = GetDefaultCollisionMode(prefab, currentCategory);
            }
            ConfigureCollider(placed, activeMode, prefabScaleMultiplier);

            // Set sorting order based on category to ensure they are on the correct layer above the floor
            int targetSortingOrder = 0;
            if (currentCategory == PaletteCategory.Wall)
            {
                targetSortingOrder = 20;
            }
            else if (currentCategory == PaletteCategory.Prop)
            {
                var collider = placed.GetComponentInChildren<Collider2D>(true);
                targetSortingOrder = (collider != null) ? 30 : 8;
            }
            else if (currentCategory == PaletteCategory.Mob)
            {
                targetSortingOrder = 40;
            }

            var renderers = placed.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var r in renderers)
            {
                // Only override if the current order is 0 (default)
                if (r.sortingOrder == 0)
                {
                    r.sortingOrder = targetSortingOrder;
                }
            }

            Undo.RegisterCreatedObjectUndo(placed, "Place Prefab");
            EditorUtility.SetDirty(placed);
            
            // Sync Scene GUI
            if (parent != null)
            {
                EditorUtility.SetDirty(parent.gameObject);
            }
        }

        private void EraseAt(Vector3Int cellPos, Vector3 snapPos)
        {
            Transform parent = GetTargetParent();
            bool erasedObject = false;

            if (parent != null)
            {
                List<GameObject> toDelete = new List<GameObject>();

                // 1. Try PickGameObject first (prioritizes whatever is directly under the mouse cursor!)
                if (Event.current != null)
                {
                    GameObject picked = HandleUtility.PickGameObject(Event.current.mousePosition, false);
                    GameObject placedParent = FindPlacedParent(picked, parent);
                    if (placedParent != null)
                    {
                        toDelete.Add(placedParent);
                    }
                }

                // 2. Fallback: check objects snapped to the same cell position or within close distance
                if (toDelete.Count == 0)
                {
                    foreach (Transform child in parent)
                    {
                        if (child == null || child.gameObject == null) continue;
                        if (child.GetComponent<Tilemap>() != null) continue;

                        Vector3Int childCell;
                        Vector3 childSnap;
                        GetCellAndSnapPos(child.position, out childCell, out childSnap);

                        if (childCell == cellPos || Vector3.Distance(child.position, snapPos) < 0.4f)
                        {
                            toDelete.Add(child.gameObject);
                        }
                    }
                }

                if (toDelete.Count > 0)
                {
                    foreach (var go in toDelete)
                    {
                        Undo.DestroyObjectImmediate(go);
                    }
                    erasedObject = true;
                }
            }

            // Only erase the Floor Tile if we did NOT erase any game objects in this call!
            if (!erasedObject && targetTilemap != null)
            {
                if (targetTilemap.HasTile(cellPos))
                {
                    Undo.RegisterCompleteObjectUndo(targetTilemap, "Erase Tiles");
                    targetTilemap.SetTile(cellPos, null);
                    EditorUtility.SetDirty(targetTilemap);
                }
            }
        }

        private GameObject FindPlacedParent(GameObject picked, Transform root)
        {
            if (picked == null || root == null) return null;
            Transform current = picked.transform;
            while (current != null && current != root)
            {
                if (current.parent == root)
                {
                    return current.gameObject;
                }
                current = current.parent;
            }
            return null;
        }

        private void DrawPrefabOutline(GameObject prefab, Vector3 snapPos, int rotation, Color color)
        {
            if (prefab == null) return;

            var sr = prefab.GetComponentInChildren<SpriteRenderer>();
            float spriteWidth = 1.0f;
            float spriteHeight = 1.0f;
            if (sr != null && sr.sprite != null)
            {
                spriteWidth = sr.sprite.bounds.size.x;
                spriteHeight = sr.sprite.bounds.size.y;
            }

            // Get collision mode for footprint depth
            CollisionMode activeMode = customCollisionMode;
            if (activeMode == CollisionMode.Auto)
            {
                activeMode = GetDefaultCollisionMode(prefab, currentCategory);
            }

            float w = spriteWidth * prefabScaleMultiplier;
            float h = spriteHeight * prefabScaleMultiplier;
            float d = 0.85f; // depth

            switch (activeMode)
            {
                case CollisionMode.WallBlock:
                    d = 0.8f;
                    break;
                case CollisionMode.FullFootprint:
                    w = spriteWidth * prefabScaleMultiplier * 0.85f;
                    d = 0.6f;
                    break;
                case CollisionMode.SmallFootprint:
                    w = 0.4f;
                    d = 0.3f;
                    break;
                case CollisionMode.Passable:
                    d = 0.2f; // thin base line
                    break;
            }

            // Adjust width and depth for 90 or 270 degrees rotation
            if (rotation == 1 || rotation == 3)
            {
                float temp = w;
                w = d;
                d = temp;
            }

            // Footprint corners on the ground (flat rectangular base aligned with the sprite orientation)
            Vector3 b0 = snapPos + new Vector3(-w / 2f, -d / 2f, 0f);
            Vector3 b1 = snapPos + new Vector3(w / 2f, -d / 2f, 0f);
            Vector3 b2 = snapPos + new Vector3(w / 2f, d / 2f, 0f);
            Vector3 b3 = snapPos + new Vector3(-w / 2f, d / 2f, 0f);

            // Project upwards by sprite height
            Vector3 offset = new Vector3(0f, h, 0f);
            Vector3 t0 = b0 + offset;
            Vector3 t1 = b1 + offset;
            Vector3 t2 = b2 + offset;
            Vector3 t3 = b3 + offset;

            Handles.color = color;

            // Draw base rectangle (dotted)
            Handles.DrawDottedLine(b0, b1, 2f);
            Handles.DrawDottedLine(b1, b2, 2f);
            Handles.DrawDottedLine(b2, b3, 2f);
            Handles.DrawDottedLine(b3, b0, 2f);

            // Draw top rectangle (solid)
            Handles.DrawAAPolyLine(3f, t0, t1, t2, t3, t0);

            // Draw vertical pillars
            Handles.DrawLine(b0, t0);
            Handles.DrawLine(b1, t1);
            Handles.DrawLine(b2, t2);
            Handles.DrawLine(b3, t3);
        }

        private void PerformEyedropper(Vector3Int cellPos, Vector3 snapPos)
        {
            // Try Eyedropper on prefab first
            Transform parent = GetTargetParent();
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (child == null) continue;
                    if (Vector3.Distance(child.position, snapPos) < 0.25f)
                    {
                        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                        if (sourcePrefab == null)
                        {
                            GameObject outermostRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(child.gameObject);
                            if (outermostRoot != null)
                            {
                                sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(outermostRoot);
                            }
                        }

                        if (sourcePrefab != null)
                        {
                            // Select category
                            string filename = sourcePrefab.name.ToLower();
                            if (filename.StartsWith("wall_")) currentCategory = PaletteCategory.Wall;
                            else if (filename.StartsWith("enemy_")) currentCategory = PaletteCategory.Mob;
                            else currentCategory = PaletteCategory.Prop;

                            selectedPrefab = sourcePrefab;
                            selectedTile = null;
                            selectedAssetName = CleanName(sourcePrefab.name);
                            customCollisionMode = CollisionMode.Auto;

                            float origScaleX = sourcePrefab.transform.localScale.x;
                            if (origScaleX != 0f)
                            {
                                prefabScaleMultiplier = child.transform.lossyScale.x / origScaleX;
                            }
                            else
                            {
                                prefabScaleMultiplier = 1.0f;
                            }
                            
                            var sr = sourcePrefab.GetComponentInChildren<SpriteRenderer>();
                            selectedAssetIcon = sr != null ? sr.sprite : null;

                            currentTool = ToolMode.Paint;
                            prefabRotation = Mathf.RoundToInt(child.transform.rotation.eulerAngles.z / 90f) % 4;

                            Repaint();
                            return;
                        }
                    }
                }
            }

            // Try Eyedropper on Floor Tile
            if (targetTilemap != null)
            {
                TileBase tile = targetTilemap.GetTile(cellPos);
                if (tile != null)
                {
                    currentCategory = PaletteCategory.Floor;
                    selectedTile = tile;
                    selectedPrefab = null;
                    selectedAssetName = tile.name;

                    Sprite s = (tile as Tile)?.sprite;
                    selectedAssetIcon = s;

                    currentTool = ToolMode.Paint;
                    Repaint();
                }
            }
        }



        private void ConfigureAssetPackColliders()
        {
            string folderPath = "Assets/Prefabs/Props/ShatteredKeep_PixelLab";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError($"[Unified Painter] Folder not found: {folderPath}");
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });
            int modifiedCount = 0;

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;

                string name = prefab.name.ToLower();

                // Load prefab contents as editing root
                GameObject editRoot = PrefabUtility.LoadPrefabContents(path);
                
                PaletteCategory category = name.StartsWith("wall_") ? PaletteCategory.Wall : PaletteCategory.Prop;
                CollisionMode mode = GetDefaultCollisionMode(prefab, category);
                
                ConfigureCollider(editRoot, mode, 0.5f);
                
                PrefabUtility.SaveAsPrefabAsset(editRoot, path);
                modifiedCount++;

                PrefabUtility.UnloadPrefabContents(editRoot);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[Unified Painter] Configured {modifiedCount} prefab assets in {folderPath}.");
            EditorUtility.DisplayDialog("Success", $"Successfully updated {modifiedCount} prefabs in ShatteredKeep_PixelLab pack!", "OK");
        }

        private Vector3 ComputeCompensatedLocalScale(Transform parent, Vector3 targetWorldScale, float rotationAngleDegrees)
        {
            if (parent == null)
            {
                return targetWorldScale;
            }
            
            Vector3 parentScale = parent.lossyScale;
            if (Mathf.Approximately(parentScale.x, parentScale.y) && Mathf.Approximately(parentScale.y, parentScale.z))
            {
                // Uniform parent scale: localScale = targetWorldScale / parentScale.x
                float pScale = parentScale.x != 0f ? parentScale.x : 1f;
                return targetWorldScale / pScale;
            }
            
            // Non-uniform parent scale: compute scale along local axes to prevent skewing/shearing
            float rad = rotationAngleDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            
            float px = parentScale.x != 0f ? parentScale.x : 1f;
            float py = parentScale.y != 0f ? parentScale.y : 1f;
            float pz = parentScale.z != 0f ? parentScale.z : 1f;
            
            float lx = Mathf.Sqrt((cos * px) * (cos * px) + (sin * py) * (sin * py));
            float ly = Mathf.Sqrt((-sin * px) * (-sin * px) + (cos * py) * (cos * py));
            
            float sx = lx > 0f ? targetWorldScale.x / lx : targetWorldScale.x;
            float sy = ly > 0f ? targetWorldScale.y / ly : targetWorldScale.y;
            float sz = pz > 0f ? targetWorldScale.z / pz : targetWorldScale.z;
            
            return new Vector3(sx, sy, sz);
        }

        private CollisionMode GetDefaultCollisionMode(GameObject prefab, PaletteCategory category)
        {
            if (prefab == null) return CollisionMode.Passable;
            
            string name = prefab.name.ToLower();
            
            if (category == PaletteCategory.Wall || name.StartsWith("wall_"))
            {
                return CollisionMode.WallBlock;
            }
            
            if (category == PaletteCategory.Prop)
            {
                // Check for passable keywords
                if (name.Contains("bone") || name.Contains("skeleton") || name.Contains("skull") || 
                    name.Contains("rubble") || name.Contains("debris") || name.Contains("stone_small") || 
                    name.Contains("pebble") || name.Contains("carpet") || name.Contains("rug") || 
                    name.Contains("blood") || name.Contains("splat") || name.Contains("crack") || 
                    name.Contains("decal") || name.Contains("mounting_"))
                {
                    return CollisionMode.Passable;
                }
                
                // Check for statue / blocking structures
                if (name.StartsWith("statue_") || name.Contains("statue") || 
                    name.Contains("column") || name.Contains("pillar") || 
                    name.Contains("chest") || name.Contains("box") || 
                    name.Contains("barrel") || name.Contains("crate") || 
                    name.Contains("tomb") || name.Contains("sarcophagus"))
                {
                    return CollisionMode.FullFootprint;
                }
                
                // Check for small footprint objects (lamps, candles, torches, etc.)
                if (name.Contains("lamp") || name.Contains("torch") || name.Contains("lantern") || 
                    name.Contains("candle") || name.Contains("light") || name.Contains("urn") || 
                    name.Contains("pot"))
                {
                    return CollisionMode.SmallFootprint;
                }
                
                // Default prop collision mode: check if it has a collider in the prefab
                var col = prefab.GetComponentInChildren<Collider2D>(true);
                if (col != null)
                {
                    return CollisionMode.FullFootprint;
                }
            }
            
            return CollisionMode.Passable;
        }

        private void ConfigureCollider(GameObject placed, CollisionMode mode, float scaleMultiplier)
        {
            // First, remove any existing colliders if they exist
            var existingColliders = placed.GetComponentsInChildren<Collider2D>(true);
            foreach (var col in existingColliders)
            {
                DestroyImmediate(col);
            }

            if (mode == CollisionMode.Passable)
            {
                return; // Passable, no collider needed
            }

            var sr = placed.GetComponentInChildren<SpriteRenderer>();
            if (sr == null || sr.sprite == null)
            {
                // Fallback if no sprite renderer (e.g. mob)
                var box = placed.AddComponent<BoxCollider2D>();
                box.size = new Vector2(0.85f, 0.85f);
                box.offset = Vector2.zero;
                return;
            }

            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            float desiredWorldWidth = 0.85f;
            float desiredWorldHeight = 0.85f;

            switch (mode)
            {
                case CollisionMode.WallBlock:
                    desiredWorldWidth = spriteWidth * scaleMultiplier; // Full sprite width in world
                    desiredWorldHeight = 0.8f; // Standard wall height blocking depth
                    break;
                case CollisionMode.FullFootprint:
                    desiredWorldWidth = spriteWidth * scaleMultiplier * 0.85f; // Most of the width
                    desiredWorldHeight = 0.6f;
                    break;
                case CollisionMode.SmallFootprint:
                    desiredWorldWidth = 0.4f;
                    desiredWorldHeight = 0.3f;
                    break;
            }

            // Convert world size to local size (since transform scale is applied to this GameObject)
            float localWidth = desiredWorldWidth / scaleMultiplier;
            float localHeight = desiredWorldHeight / scaleMultiplier;

            var newB