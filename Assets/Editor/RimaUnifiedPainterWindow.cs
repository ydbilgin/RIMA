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
using RIMA;

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
        private GameObject cachedHoveredObject;

        // Grouping & Randomization state
        [SerializeField] private bool useRandomVariants = false;
        [SerializeField] private bool autoConnectWalls = true;
        [SerializeField] private bool randomizeWallCracks = true;

        [System.Serializable]
        public class TerrainGroup
        {
            public string terrainName;
            public TileBase baseTile;
            public List<TileBase> allTiles = new List<TileBase>();
        }
        private List<TerrainGroup> terrainGroups = new List<TerrainGroup>();


        // UI state
        private Vector2 paletteScroll;
        private Vector2 optionsScroll;
        private Dictionary<int, Texture2D> previewCache = new Dictionary<int, Texture2D>();

        [SerializeField] private string activeMapName = string.Empty;
        [SerializeField] private string activeMapPath = string.Empty;
        [SerializeField] private bool showMapList = true;
        [SerializeField] private bool showTargetsSection = true;
        [SerializeField] private bool showToolSettingsSection = true;
        [SerializeField] private bool showPrefabSettingsSection = true;
        [SerializeField] private bool showMapManagementSection = true;
        private List<string> savedMapPaths = new List<string>();

        [System.Serializable]
        public class UnifiedTileData
        {
            public Vector3Int position;
            public string tileAssetPath;
        }

        [System.Serializable]
        public class UnifiedObjectData
        {
            public string prefabAssetPath;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale = Vector3.one;
            public CollisionMode collisionMode = CollisionMode.Auto;
            public Vector2 colliderSize = Vector2.one;
            public Vector2 colliderOffset = Vector2.zero;
            public string name;
        }

        [System.Serializable]
        public class UnifiedMapSaveData
        {
            public string mapName;
            public List<UnifiedTileData> tiles = new List<UnifiedTileData>();
            public List<UnifiedObjectData> objects = new List<UnifiedObjectData>();
        }


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
            RefreshSavedMapsList();
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

        private double lastRepaintTime;

        private void OnEditorUpdate()
        {
            // Rate limit repaints during preview loading to avoid overloading GUI pipeline
            if (AssetPreview.IsLoadingAssetPreviews())
            {
                if (EditorApplication.timeSinceStartup - lastRepaintTime > 0.25)
                {
                    lastRepaintTime = EditorApplication.timeSinceStartup;
                    Repaint();
                }
            }
        }

        private void ScanAllAssets()
        {
            // 1. Scan Biome for Floors
            floorTiles.Clear();
            terrainGroups.Clear();
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
                    
                    var tg = new TerrainGroup { terrainName = terrain.name, baseTile = terrain.baseTile };
                    
                    if (terrain.baseTile != null)
                    {
                        tg.allTiles.Add(terrain.baseTile);
                        AddTileToScanList(terrain.baseTile, terrain.name);
                    }
                    if (terrain.variantPool != null)
                    {
                        for (int i = 0; i < terrain.variantPool.Count; i++)
                        {
                            if (terrain.variantPool[i] != null)
                            {
                                tg.allTiles.Add(terrain.variantPool[i]);
                                AddTileToScanList(terrain.variantPool[i], $"{terrain.name} (Var {i+1})");
                            }
                        }
                    }
                    if (tg.allTiles.Count > 0)
                    {
                        terrainGroups.Add(tg);
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
            // Intercept keyboard shortcuts inside the editor window
            Event e = Event.current;
            if (e != null && e.type == EventType.KeyDown)
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
                else if (e.keyCode == KeyCode.Escape)
                {
                    selectedTile = null;
                    selectedPrefab = null;
                    selectedAssetName = "None";
                    selectedAssetIcon = null;
                    e.Use();
                    Repaint();
                }
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

            // Active Brush Details & Quick Cancel
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Selected Brush Status", EditorStyles.boldLabel);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (selectedAssetIcon != null)
                    {
                        Rect iconRect = GUILayoutUtility.GetRect(40f, 40f, GUILayout.Width(40f), GUILayout.Height(40f));
                        GUI.DrawTexture(iconRect, selectedAssetIcon.texture, ScaleMode.ScaleToFit, true);
                    }
                    else
                    {
                        GUILayout.Box("No Icon", GUILayout.Width(40f), GUILayout.Height(40f));
                    }
                    
                    using (new EditorGUILayout.VerticalScope())
                    {
                        GUILayout.Label(selectedAssetName, EditorStyles.boldLabel);
                        GUILayout.Label($"Category: {currentCategory}", EditorStyles.miniLabel);
                    }
                }
                
                if (selectedTile != null || selectedPrefab != null)
                {
                    EditorGUILayout.Space(5);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Clear Brush (Esc)", GUILayout.Height(20f)))
                        {
                            selectedTile = null;
                            selectedPrefab = null;
                            selectedAssetName = "None";
                            selectedAssetIcon = null;
                        }
                    }
                }
            }
            EditorGUILayout.Space(10);

            // 1. Target Configuration Foldout
            showTargetsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showTargetsSection, "1. Target Configuration");
            if (showTargetsSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUI.BeginChangeCheck();
                    activeBiome = (RimaBiomePreset)EditorGUILayout.ObjectField("Biome Preset", activeBiome, typeof(RimaBiomePreset), false);
                    if (EditorGUI.EndChangeCheck())
                    {
                        ScanAllAssets();
                    }

                    targetTilemap = (Tilemap)EditorGUILayout.ObjectField("Target Tilemap", targetTilemap, typeof(Tilemap), true);
                    targetParent = (Transform)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(Transform), true);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);

            // 2. Painting Tools (Core - Always visible)
            EditorGUILayout.LabelField("2. Painting Tools", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawToolButton("Brush (B)", ToolMode.Paint);
                DrawToolButton("Erase (E)", ToolMode.Erase);
                DrawToolButton("Eyedropper (I)", ToolMode.Eyedropper);
            }
            EditorGUILayout.Space(10);

            // 3. Tool Settings Foldout
            showToolSettingsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showToolSettingsSection, "3. Brush & Placement Settings");
            if (showToolSettingsSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    snapToGrid = EditorGUILayout.Toggle("Snap to Grid", snapToGrid);
                    
                    if (currentCategory == PaletteCategory.Floor)
                    {
                        brushSize = EditorGUILayout.IntSlider("Brush Size", brushSize, 1, 5);
                        useRandomVariants = EditorGUILayout.Toggle("Randomize Variants", useRandomVariants);
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

                        if (currentCategory == PaletteCategory.Wall)
                        {
                            EditorGUILayout.Space(5);
                            autoConnectWalls = EditorGUILayout.Toggle("Auto-Connect Walls", autoConnectWalls);
                            if (autoConnectWalls)
                            {
                                randomizeWallCracks = EditorGUILayout.Toggle("Yarık Süslemesi (15%)", randomizeWallCracks);
                                if (GUILayout.Button("Rebuild All Connections", GUILayout.Height(20f)))
                                {
                                    RebuildAllWallConnections();
                                }
                            }
                        }
                    }

                    EditorGUILayout.Space(5);
                    if (GUILayout.Button("Reset Settings to Default", GUILayout.Height(20f)))
                    {
                        ResetToolSettings();
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);

            // 4. Collision Settings Foldout
            if (currentCategory == PaletteCategory.Prop || currentCategory == PaletteCategory.Wall)
            {
                showPrefabSettingsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showPrefabSettingsSection, "4. Collider Boundaries");
                if (showPrefabSettingsSection)
                {
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        customCollisionMode = (CollisionMode)EditorGUILayout.EnumPopup("Collision Mode", customCollisionMode);
                        if (customCollisionMode == CollisionMode.Custom)
                        {
                            EditorGUI.indentLevel++;
                            customColliderSize = EditorGUILayout.Vector2Field("Custom Size", customColliderSize);
                            customColliderOffset = EditorGUILayout.Vector2Field("Custom Offset", customColliderOffset);
                            EditorGUI.indentLevel--;
                        }
                        
                        EditorGUILayout.Space(5);
                        EditorGUILayout.HelpBox(
                            "Colliders are set in local space at the base (footprint) of the sprite. Under rotation, " +
                            "the collider rotates together with the GameObject to block the player correctly.", 
                            MessageType.Info
                        );
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space(5);
            }

            // 5. Map Management Foldout
            showMapManagementSection = EditorGUILayout.BeginFoldoutHeaderGroup(showMapManagementSection, "5. Level File Management");
            if (showMapManagementSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    string activeMapDisplay = string.IsNullOrEmpty(activeMapName) ? "None (Unsaved)" : activeMapName;
                    GUILayout.Label($"Active: {activeMapDisplay}", EditorStyles.miniBoldLabel);
                    
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (!string.IsNullOrEmpty(activeMapPath))
                        {
                            if (GUILayout.Button("Save", GUILayout.Height(22f)))
                            {
                                SaveMapData(activeMapPath);
                            }
                        }
                        if (GUILayout.Button("Save As...", GUILayout.Height(22f)))
                        {
                            SaveActiveMapAs();
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("New Map", GUILayout.Height(22f)))
                        {
                            CreateNewMap();
                        }
                        if (GUILayout.Button("Refresh List", GUILayout.Height(22f)))
                        {
                            RefreshSavedMapsList();
                        }
                    }

                    EditorGUILayout.Space(5);
                    if (GUILayout.Button("Attach IsoSorter to All Placed Objects", GUILayout.Height(22f)))
                    {
                        AttachIsoSorterToAllPlacedObjects();
                    }

                    showMapList = EditorGUILayout.Foldout(showMapList, "Saved Level Files", true);
                    if (showMapList)
                    {
                        if (savedMapPaths == null || savedMapPaths.Count == 0)
                        {
                            EditorGUILayout.LabelField("No maps found in UnifiedMaps/", EditorStyles.miniLabel);
                        }
                        else
                        {
                            EditorGUI.indentLevel++;
                            foreach (var path in savedMapPaths)
                            {
                                string mName = Path.GetFileNameWithoutExtension(path);
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    bool isActive = path == activeMapPath;
                                    GUIStyle nameStyle = isActive ? EditorStyles.boldLabel : EditorStyles.label;
                                    GUILayout.Label(mName, nameStyle);
                                    
                                    if (GUILayout.Button("Load", GUILayout.Width(45f)))
                                    {
                                        if (EditorUtility.DisplayDialog("Load Map", $"Load '{mName}' and overwrite the scene canvas?", "Yes", "No"))
                                        {
                                            LoadMapData(path);
                                        }
                                    }
                                    if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                                    {
                                        if (EditorUtility.DisplayDialog("Overwrite Map", $"Overwrite '{mName}' with the current canvas?", "Yes", "No"))
                                        {
                                            SaveMapData(path);
                                        }
                                    }
                                }
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);

            // Status message helpbox at the bottom
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
                EditorGUILayout.HelpBox("Use standard keys in Scene View:\n- R: Rotate prefab\n- B: Brush Tool\n- E: Erase Tool\n- I: Eyedropper\n- Esc: Clear active brush", MessageType.Info);
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

        private void ResetToolSettings()
        {
            snapToGrid = true;
            brushSize = 1;
            useRandomVariants = false;
            prefabRotation = 0;
            prefabScaleMultiplier = 1.0f;
            autoAlignBaseToGrid = true;
            positionOffset = Vector3.zero;
            customCollisionMode = CollisionMode.Auto;
            customColliderSize = Vector2.one;
            customColliderOffset = Vector2.zero;
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
            Color outlineColor = isSelected ? new Color(0f, 0.7f, 1f, 0.8f) : (hover ? new Color(1f, 1f, 1f, 0.3f) : new Color(0.25f, 0.25f, 0.25f, 0.5f));
            EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y, btnRect.width, 1f), outlineColor);
            EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y + btnRect.height - 1f, btnRect.width, 1f), outlineColor);
            EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y, 1f, btnRect.height), outlineColor);
            EditorGUI.DrawRect(new Rect(btnRect.x + btnRect.width - 1f, btnRect.y, 1f, btnRect.height), outlineColor);

            if (isSelected)
            {
                Color innerColor = new Color(0f, 0.7f, 1f, 0.8f);
                EditorGUI.DrawRect(new Rect(btnRect.x + 1f, btnRect.y + 1f, btnRect.width - 2f, 1f), innerColor);
                EditorGUI.DrawRect(new Rect(btnRect.x + 1f, btnRect.y + btnRect.height - 2f, btnRect.width - 2f, 1f), innerColor);
                EditorGUI.DrawRect(new Rect(btnRect.x + 1f, btnRect.y + 1f, 1f, btnRect.height - 2f), innerColor);
                EditorGUI.DrawRect(new Rect(btnRect.x + btnRect.width - 2f, btnRect.y + 1f, 1f, btnRect.height - 2f), innerColor);
            }

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

            if (currentTool != ToolMode.Erase)
            {
                cachedHoveredObject = null;
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
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                UpdatePreviewObject(selectedPrefab, snapPos, finalOffset, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos + finalOffset, prefabRotation, new Color(0.2f, 1f, 0.35f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Prop && selectedPrefab != null)
            {
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                UpdatePreviewObject(selectedPrefab, snapPos, finalOffset, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos + finalOffset, prefabRotation, new Color(0.2f, 1f, 0.35f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Mob && selectedPrefab != null)
            {
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                UpdatePreviewObject(selectedPrefab, snapPos, finalOffset, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos + finalOffset, prefabRotation, new Color(1f, 0.4f, 0.1f, 0.8f));
            }
            else if (currentTool == ToolMode.Erase)
            {
                ClearPreviewObject();
                if (cachedHoveredObject != null)
                {
                    DrawTargetObjectOutline(cachedHoveredObject, new Color(1f, 0.2f, 0.2f, 0.8f));
                }
                else
                {
                    DrawCellOutline(targetTilemap, grid, cellPos, new Color(1f, 0.2f, 0.2f, 0.8f));
                }
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

            if (e.type == EventType.MouseMove || e.type == EventType.MouseDrag || e.type == EventType.MouseDown || e.type == EventType.MouseUp)
            {
                bool hoverChanged = false;
                if (currentTool == ToolMode.Erase)
                {
                    GameObject currentHover = GetEraseTargetObject(cellPos, snapPos);
                    if (currentHover != cachedHoveredObject)
                    {
                        cachedHoveredObject = currentHover;
                        hoverChanged = true;
                    }
                }

                if (cellPos != lastCellPos || hoverChanged || e.type == EventType.MouseDown || e.type == EventType.MouseUp)
                {
                    lastCellPos = cellPos;
                    SceneView.RepaintAll();
                }
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

        private Vector3 GetCellWorldPosition(Vector3Int cellPos)
        {
            if (targetTilemap != null)
            {
                Vector3 localPos = targetTilemap.GetCellCenterLocal(cellPos);
                return targetTilemap.transform.TransformPoint(localPos);
            }
            Grid grid = GameObject.FindObjectOfType<Grid>();
            if (grid != null)
            {
                Vector3 localPos = grid.GetCellCenterLocal(cellPos);
                return grid.transform.TransformPoint(localPos);
            }
            return new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0f);
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

        private void UpdatePreviewObject(GameObject sourcePrefab, Vector3 position, Vector3 offset, int rotationSteps)
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

            previewObject.transform.position = position + offset;
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
                        if (currentCategory == PaletteCategory.Wall && autoConnectWalls)
                        {
                            PaintWallWithConnections(cellPos, snapPos, selectedPrefab);
                        }
                        else
                        {
                            PaintPrefab(snapPos, selectedPrefab);
                        }
                    }
                }
            }
            else if (currentTool == ToolMode.Erase)
            {
                if (currentCategory == PaletteCategory.Floor)
                {
                    // Drag to erase floor tiles
                    EraseAt(cellPos, snapPos);
                }
                else
                {
                    // Only erase GameObjects (prefabs) on MouseDown to prevent accidental deletion dragging
                    if (Event.current.type == EventType.MouseDown)
                    {
                        EraseAt(cellPos, snapPos);
                    }
                }
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
                    TileBase finalTile = tile;
                    if (useRandomVariants)
                    {
                        finalTile = GetRandomTileFromGroup(tile);
                    }
                    targetTilemap.SetTile(pos, finalTile);
                }
            }
            EditorUtility.SetDirty(targetTilemap);
        }

        private TileBase GetRandomTileFromGroup(TileBase tile)
        {
            if (tile == null) return null;
            
            // Find if there is a group that contains this tile
            var group = terrainGroups.Find(tg => tg.allTiles.Contains(tile));
            if (group != null && group.allTiles.Count > 0)
            {
                int rIndex = UnityEngine.Random.Range(0, group.allTiles.Count);
                return group.allTiles[rIndex];
            }
            return tile;
        }

        private void PaintPrefab(Vector3 snapPos, GameObject prefab)
        {
            Transform baseParent = GetTargetParent();
            Transform parent = GetOrCreateGroupParent(baseParent, prefab.name, currentCategory);
            Vector3 finalOffset = GetPlacementOffset(prefab);
            Vector3 targetPos = snapPos + finalOffset;
            
            // Check if there is already an object placed in the exact cell to avoid stacking
            if (baseParent != null)
            {
                List<Transform> allChildren = new List<Transform>();
                GetRecursiveChildren(baseParent, allChildren);
                foreach (Transform child in allChildren)
                {
                    if (child != null && child.GetComponent<SpriteRenderer>() != null && Vector3.Distance(child.position, targetPos) < 0.1f)
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
            placed.transform.position = targetPos;
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

                // Add IsoSorter to the GameObject containing the SpriteRenderer
                var sorter = r.GetComponent<RIMA.IsoSorter>();
                if (sorter == null)
                {
                    sorter = r.gameObject.AddComponent<RIMA.IsoSorter>();
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
            if (currentCategory == PaletteCategory.Floor)
            {
                // Only erase Floor tiles
                if (targetTilemap != null && targetTilemap.HasTile(cellPos))
                {
                    Undo.RegisterCompleteObjectUndo(targetTilemap, "Erase Tiles");
                    targetTilemap.SetTile(cellPos, null);
                    EditorUtility.SetDirty(targetTilemap);
                }
            }
            else
            {
                // Only erase GameObjects
                Transform parent = GetTargetParent();
                if (parent != null)
                {
                    List<GameObject> toDelete = new List<GameObject>();

                    if (cachedHoveredObject != null && cachedHoveredObject.transform.IsChildOf(parent))
                    {
                        toDelete.Add(cachedHoveredObject);
                        cachedHoveredObject = null;
                    }
                    else
                    {
                        // Fallback: check objects snapped to the same cell position or within close distance
                        List<Transform> allChildren = new List<Transform>();
                        GetRecursiveChildren(parent, allChildren);
                        foreach (Transform child in allChildren)
                        {
                            if (child == null || child.gameObject == null) continue;
                            if (child.GetComponent<Tilemap>() != null) continue;
                            if (child.GetComponent<SpriteRenderer>() == null) continue; // Skip group containers

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
                        EditorUtility.SetDirty(parent.gameObject);

                        if (currentCategory == PaletteCategory.Wall && autoConnectWalls)
                        {
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, 1, 0));  // NE
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, -1, 0)); // SW
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(-1, 0, 0)); // NW
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(1, 0, 0));  // SE
                        }
                    }
                }
            }
        }

        private GameObject FindPlacedParent(GameObject picked, Transform root)
        {
            if (picked == null || root == null) return null;
            Transform current = picked.transform;
            Transform child = current;
            while (current != null && current != root)
            {
                if (current.parent == root)
                {
                    string n = current.name;
                    if (n == "Walls" || n == "Statues" || n == "WallMountings" || n == "Patches" || n == "Mobs" || n == "FloorProps")
                    {
                        return child.gameObject;
                    }
                    return current.gameObject;
                }
                child = current;
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

            float scale = prefabScaleMultiplier != 0f ? prefabScaleMultiplier : 1.0f;
            float h = spriteHeight * scale;

            Vector2 localSize = Vector2.one;
            Vector2 localOffset = Vector2.zero;
            float localVisibleBottom = sr != null && sr.sprite != null ? GetSpriteVisibleBounds(sr.sprite).minY : 0f;

            if (activeMode == CollisionMode.Passable)
            {
                localSize = new Vector2(spriteWidth * 0.8f, 0.1f);
                localOffset = new Vector2(0f, localVisibleBottom + 0.05f);
            }
            else if (activeMode == CollisionMode.Custom)
            {
                localSize = customColliderSize / scale;
                localOffset = customColliderOffset / scale;
            }
            else
            {
                float desiredWorldWidth = 0.85f;
                float desiredWorldHeight = 0.85f;

                switch (activeMode)
                {
                    case CollisionMode.WallBlock:
                        desiredWorldWidth = spriteWidth * scale;
                        desiredWorldHeight = 0.8f;
                        break;
                    case CollisionMode.FullFootprint:
                        desiredWorldWidth = spriteWidth * scale * 0.85f;
                        desiredWorldHeight = 0.6f;
                        break;
                    case CollisionMode.SmallFootprint:
                        desiredWorldWidth = 0.4f;
                        desiredWorldHeight = 0.3f;
                        break;
                }

                localSize = new Vector2(desiredWorldWidth / scale, desiredWorldHeight / scale);
                localOffset = new Vector2(0f, localVisibleBottom + localSize.y / 2f);
            }

            // Compute local corners relative to the pivot (0, 0)
            float halfX = localSize.x / 2f;
            float halfY = localSize.y / 2f;
            Vector3 localC0 = localOffset + new Vector2(-halfX, -halfY);
            Vector3 localC1 = localOffset + new Vector2(halfX, -halfY);
            Vector3 localC2 = localOffset + new Vector2(halfX, halfY);
            Vector3 localC3 = localOffset + new Vector2(-halfX, halfY);

            // Rotate and scale the local corners, then translate to snapPos
            Quaternion rot = Quaternion.Euler(0f, 0f, rotation * 90f);
            Vector3 b0 = snapPos + rot * Vector3.Scale(localC0, new Vector3(scale, scale, 1f));
            Vector3 b1 = snapPos + rot * Vector3.Scale(localC1, new Vector3(scale, scale, 1f));
            Vector3 b2 = snapPos + rot * Vector3.Scale(localC2, new Vector3(scale, scale, 1f));
            Vector3 b3 = snapPos + rot * Vector3.Scale(localC3, new Vector3(scale, scale, 1f));

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
                List<Transform> allChildren = new List<Transform>();
                GetRecursiveChildren(parent, allChildren);
                foreach (Transform child in allChildren)
                {
                    if (child == null || child.GetComponent<SpriteRenderer>() == null) continue;
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

            float scale = scaleMultiplier != 0f ? scaleMultiplier : 1.0f;

            if (mode == CollisionMode.Custom)
            {
                var customBox = placed.AddComponent<BoxCollider2D>();
                customBox.size = customColliderSize / scale;
                customBox.offset = customColliderOffset / scale;

                int wl = LayerMask.NameToLayer("Walls");
                if (wl != -1)
                {
                    placed.layer = wl;
                }
                return;
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
            float localVisibleBottom = GetSpriteVisibleBounds(sr.sprite).minY;

            float desiredWorldWidth = 0.85f;
            float desiredWorldHeight = 0.85f;

            switch (mode)
            {
                case CollisionMode.WallBlock:
                    desiredWorldWidth = spriteWidth * scale; // Full sprite width in world
                    desiredWorldHeight = 0.8f; // Standard wall height blocking depth
                    break;
                case CollisionMode.FullFootprint:
                    desiredWorldWidth = spriteWidth * scale * 0.85f; // Most of the width
                    desiredWorldHeight = 0.6f;
                    break;
                case CollisionMode.SmallFootprint:
                    desiredWorldWidth = 0.4f;
                    desiredWorldHeight = 0.3f;
                    break;
            }

            // Convert world size to local size (since transform scale is applied to this GameObject)
            float localWidth = desiredWorldWidth / scale;
            float localHeight = desiredWorldHeight / scale;

            var newBox = placed.AddComponent<BoxCollider2D>();
            newBox.size = new Vector2(localWidth, localHeight);
            newBox.offset = new Vector2(0f, localVisibleBottom + localHeight / 2f);

            // Assign to walls layer if blocking
            int wallsLayer = LayerMask.NameToLayer("Walls");
            if (wallsLayer != -1)
            {
                placed.layer = wallsLayer;
            }
        }

        private struct SpriteVisibleBounds
        {
            public float minX;
            public float maxX;
            public float minY;
            public float maxY;
        }

        private Dictionary<Sprite, SpriteVisibleBounds> visibleBoundsCache = new Dictionary<Sprite, SpriteVisibleBounds>();

        private Vector3 GetPlacementOffset(GameObject prefab)
        {
            Vector3 finalOffset = positionOffset;
            if (autoAlignBaseToGrid && currentCategory != PaletteCategory.Floor && prefab != null)
            {
                var sr = prefab.GetComponentInChildren<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    float autoY = CalculateAutoYOffset(sr.sprite, prefabScaleMultiplier, prefabRotation);
                    finalOffset.y += autoY;
                }
            }
            return finalOffset;
        }

        private float GetRotatedLocalVisibleBottom(Sprite sprite, int rotationSteps)
        {
            if (sprite == null) return 0f;
            SpriteVisibleBounds bounds = GetSpriteVisibleBounds(sprite);
            int r = (rotationSteps % 4 + 4) % 4;
            if (r == 0) return bounds.minY;
            if (r == 1) return bounds.minX;
            if (r == 2) return -bounds.maxY;
            return -bounds.maxX;
        }

        private float CalculateAutoYOffset(Sprite sprite, float worldScaleY, int rotationSteps)
        {
            if (sprite == null) return 0f;

            float cellHeight = 0.47f; // Default fallback for typical isometric ratio
            Grid grid = GameObject.FindObjectOfType<Grid>();
            if (targetTilemap != null)
            {
                cellHeight = targetTilemap.cellSize.y;
            }
            else if (grid != null)
            {
                cellHeight = grid.cellSize.y;
            }

            float localVisibleBottom = GetRotatedLocalVisibleBottom(sprite, rotationSteps);

            // Align visible bottom to the bottom vertex of the cell (-cellHeight / 2)
            float targetLocalY = -cellHeight / 2f;
            float currentLocalY = localVisibleBottom * worldScaleY;
            return targetLocalY - currentLocalY;
        }

        private SpriteVisibleBounds GetSpriteVisibleBounds(Sprite sprite)
        {
            if (sprite == null) return new SpriteVisibleBounds { minX = 0f, maxX = 0f, minY = 0f, maxY = 0f };

            if (visibleBoundsCache.TryGetValue(sprite, out var bounds))
            {
                return bounds;
            }

            // Default fallback is sprite.bounds
            SpriteVisibleBounds result = new SpriteVisibleBounds
            {
                minX = sprite.bounds.min.x,
                maxX = sprite.bounds.max.x,
                minY = sprite.bounds.min.y,
                maxY = sprite.bounds.max.y
            };

            Texture2D texture = sprite.texture;
            if (texture == null) return result;

            string path = AssetDatabase.GetAssetPath(texture);
            if (string.IsNullOrEmpty(path)) return result;

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) return result;

            bool wasReadable = importer.isReadable;
            if (!wasReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }

            try
            {
                Rect rect = sprite.rect;
                int minX = (int)rect.width;
                int maxX = 0;
                int minY = (int)rect.height;
                int maxY = 0;
                bool found = false;

                for (int y = 0; y < (int)rect.height; y++)
                {
                    for (int x = 0; x < (int)rect.width; x++)
                    {
                        Color pixel = texture.GetPixel((int)rect.x + x, (int)rect.y + y);
                        if (pixel.a > 0.05f)
                        {
                            if (x < minX) minX = x;
                            if (x > maxX) maxX = x;
                            if (y < minY) minY = y;
                            if (y > maxY) maxY = y;
                            found = true;
                        }
                    }
                }

                if (found)
                {
                    float unitPerPixelX = sprite.bounds.size.x / rect.width;
                    float unitPerPixelY = sprite.bounds.size.y / rect.height;

                    result.minX = sprite.bounds.min.x + minX * unitPerPixelX;
                    result.maxX = sprite.bounds.min.x + maxX * unitPerPixelX;
                    result.minY = sprite.bounds.min.y + minY * unitPerPixelY;
                    result.maxY = sprite.bounds.min.y + maxY * unitPerPixelY;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[Unified Painter] Could not read sprite pixels: {ex.Message}");
            }
            finally
            {
                if (!wasReadable)
                {
                    importer.isReadable = false;
                    importer.SaveAndReimport();
                }
            }

            visibleBoundsCache[sprite] = result;
            return result;
        }

        private GameObject GetEraseTargetObject(Vector3Int cellPos, Vector3 snapPos)
        {
            if (currentCategory == PaletteCategory.Floor) return null;

            Transform parent = GetTargetParent();
            if (parent == null) return null;

            // 1. Try PickGameObject first
            if (Event.current != null)
            {
                try
                {
                    GameObject picked = HandleUtility.PickGameObject(Event.current.mousePosition, false);
                    GameObject placedParent = FindPlacedParent(picked, parent);
                    if (placedParent != null)
                    {
                        return placedParent;
                    }
                }
                catch
                {
                    // Ignore event exceptions and proceed to fallback
                }
            }

            // 2. Fallback: check close distance or cell matching
            List<Transform> allChildren = new List<Transform>();
            GetRecursiveChildren(parent, allChildren);
            foreach (Transform child in allChildren)
            {
                if (child == null || child.gameObject == null) continue;
                if (child.GetComponent<Tilemap>() != null) continue;
                if (child.GetComponent<SpriteRenderer>() == null) continue; // Skip group containers

                Vector3Int childCell;
                Vector3 childSnap;
                GetCellAndSnapPos(child.position, out childCell, out childSnap);

                if (childCell == cellPos || Vector3.Distance(child.position, snapPos) < 0.4f)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        private void DrawTargetObjectOutline(GameObject go, Color color)
        {
            if (go == null) return;

            var box = go.GetComponentInChildren<BoxCollider2D>(true);
            var sr = go.GetComponentInChildren<SpriteRenderer>(true);

            Vector2 localSize = Vector2.one;
            Vector2 localOffset = Vector2.zero;
            float scaleY = go.transform.lossyScale.y;
            float spriteHeight = 1.0f;

            if (sr != null && sr.sprite != null)
            {
                spriteHeight = sr.sprite.bounds.size.y;
            }

            if (box != null)
            {
                localSize = box.size;
                localOffset = box.offset;
            }
            else if (sr != null && sr.sprite != null)
            {
                // Passable or no collider, draw thin base outline
                float localVisibleBottom = GetSpriteVisibleBounds(sr.sprite).minY;
                localSize = new Vector2(sr.sprite.bounds.size.x * 0.8f, 0.1f);
                localOffset = new Vector2(0f, localVisibleBottom + 0.05f);
            }

            // Local corners of collider
            float halfX = localSize.x / 2f;
            float halfY = localSize.y / 2f;
            Vector3 localC0 = localOffset + new Vector2(-halfX, -halfY);
            Vector3 localC1 = localOffset + new Vector2(halfX, -halfY);
            Vector3 localC2 = localOffset + new Vector2(halfX, halfY);
            Vector3 localC3 = localOffset + new Vector2(-halfX, halfY);

            // Transform to world space
            Transform trans = box != null ? box.transform : (sr != null ? sr.transform : go.transform);
            Vector3 b0 = trans.TransformPoint(localC0);
            Vector3 b1 = trans.TransformPoint(localC1);
            Vector3 b2 = trans.TransformPoint(localC2);
            Vector3 b3 = trans.TransformPoint(localC3);

            // Project upwards by sprite height
            float h = spriteHeight * scaleY;
            Vector3 offset = trans.up * h;
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

            // Side fills
            Color sideFill = color;
            sideFill.a = 0.05f;
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b1, t1, t0 }, sideFill, Color.clear);
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b3, t3, t0 }, sideFill, Color.clear);
        }

        private Transform GetTargetParent()
        {
            Transform p = null;
            if (targetParent != null) p = targetParent;
            else if (targetTilemap != null) p = targetTilemap.transform.parent != null ? targetTilemap.transform.parent : targetTilemap.transform;

            if (p != null)
            {
                bool isGridOrTilemap = p.GetComponent<Grid>() != null || 
                                       p.GetComponent<Tilemap>() != null || 
                                       p.name.ToLower().Contains("grid") || 
                                       p.name.ToLower().Contains("tilemap");
                
                if (isGridOrTilemap)
                {
                    GameObject propsRoot = GameObject.Find("Props_Root");
                    if (propsRoot == null)
                    {
                        propsRoot = new GameObject("Props_Root");
                        propsRoot.transform.position = Vector3.zero;
                        propsRoot.transform.rotation = Quaternion.identity;
                        propsRoot.transform.localScale = Vector3.one;
                        Undo.RegisterCreatedObjectUndo(propsRoot, "Create Props_Root");
                    }
                    return propsRoot.transform;
                }
            }
            return p;
        }

        private void GetRecursiveChildren(Transform current, List<Transform> result)
        {
            foreach (Transform child in current)
            {
                if (child == null) continue;
                result.Add(child);
                GetRecursiveChildren(child, result);
            }
        }

        private Transform GetOrCreateGroupParent(Transform root, string prefabName, PaletteCategory category)
        {
            if (root == null) return null;
            
            string groupName = "FloorProps";
            string lowerName = prefabName.ToLower();
            
            if (category == PaletteCategory.Wall || lowerName.StartsWith("wall_"))
            {
                groupName = "Walls";
            }
            else if (lowerName.StartsWith("statue_"))
            {
                groupName = "Statues";
            }
            else if (lowerName.StartsWith("mounting_") || lowerName.Contains("torch") || lowerName.Contains("banner"))
            {
                groupName = "WallMountings";
            }
            else if (lowerName.StartsWith("patch_") || lowerName.StartsWith("rug_") || lowerName.StartsWith("carpet_"))
            {
                groupName = "Patches";
            }
            else if (category == PaletteCategory.Mob || lowerName.StartsWith("mob_"))
            {
                groupName = "Mobs";
            }
            
            Transform groupTransform = root.Find(groupName);
            if (groupTransform == null)
            {
                GameObject groupGo = new GameObject(groupName);
                groupGo.transform.SetParent(root, false);
                groupGo.transform.localPosition = Vector3.zero;
                groupGo.transform.localRotation = Quaternion.identity;
                groupGo.transform.localScale = Vector3.one;
                Undo.RegisterCreatedObjectUndo(groupGo, "Create Group " + groupName);
                groupTransform = groupGo.transform;
            }
            return groupTransform;
        }

        private void DrawSeparator()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 1f));
        }

        private void RefreshSavedMapsList()
        {
            savedMapPaths.Clear();
            string folder = "Assets/RIMA_MapData/UnifiedMaps";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string[] files = Directory.GetFiles(folder, "*.json");
            foreach (var f in files)
            {
                savedMapPaths.Add(f.Replace('\\', '/'));
            }
        }

        private void CreateNewMap()
        {
            if (EditorUtility.DisplayDialog("New Map", "Are you sure you want to clear the canvas and start a new map?", "Yes", "No"))
            {
                ClearSceneCanvas();
                activeMapName = "NewMap";
                activeMapPath = string.Empty;
            }
        }

        private void ClearSceneCanvas()
        {
            if (targetTilemap != null)
            {
                Undo.RegisterCompleteObjectUndo(targetTilemap, "Clear Tilemap");
                targetTilemap.ClearAllTiles();
            }
            Transform parent = GetTargetParent();
            if (parent != null)
            {
                Undo.RegisterCompleteObjectUndo(parent, "Clear Spawned Objects");
                List<GameObject> childrenToDestroy = new List<GameObject>();
                foreach (Transform child in parent)
                {
                    if (child != null && child.gameObject != null && child.GetComponent<Tilemap>() == null)
                    {
                        childrenToDestroy.Add(child.gameObject);
                    }
                }
                foreach (var child in childrenToDestroy)
                {
                    Undo.DestroyObjectImmediate(child);
                }
            }
        }

        private void SaveActiveMapAs()
        {
            string folder = "Assets/RIMA_MapData/UnifiedMaps";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string path = EditorUtility.SaveFilePanelInProject("Save Unified Map", "Map_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "json", "Choose where to save map data", folder);
            if (!string.IsNullOrEmpty(path))
            {
                SaveMapData(path);
            }
        }

        private void SaveMapData(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            UnifiedMapSaveData data = new UnifiedMapSaveData();
            data.mapName = Path.GetFileNameWithoutExtension(path);

            // 1. Serialize Tiles
            if (targetTilemap != null)
            {
                BoundsInt bounds = targetTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    TileBase tile = targetTilemap.GetTile(pos);
                    if (tile != null)
                    {
                        string tilePath = AssetDatabase.GetAssetPath(tile);
                        data.tiles.Add(new UnifiedTileData
                        {
                            position = pos,
                            tileAssetPath = tilePath
                        });
                    }
                }
            }

            // 2. Serialize Objects
            Transform parent = GetTargetParent();
            if (parent != null)
            {
                List<Transform> allChildren = new List<Transform>();
                GetRecursiveChildren(parent, allChildren);
                foreach (Transform child in allChildren)
                {
                    if (child == null || child.gameObject == null) continue;
                    if (child.GetComponent<Tilemap>() != null) continue;
                    if (child.GetComponent<SpriteRenderer>() == null) continue; // Skip group containers

                    // Try to get original prefab asset path
                    GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                    string prefabPath = prefabAsset != null ? AssetDatabase.GetAssetPath(prefabAsset) : string.Empty;
                    
                    if (string.IsNullOrEmpty(prefabPath))
                    {
                        // Fallback: search by name in scanned prefabs
                        string cName = child.name.Replace("(Clone)", "").Trim();
                        var scan = wallPrefabs.Concat(propPrefabs).Concat(mobPrefabs).FirstOrDefault(x => x.prefab != null && x.prefab.name == cName);
                        if (scan != null)
                        {
                            prefabPath = scan.assetPath;
                        }
                    }

                    if (!string.IsNullOrEmpty(prefabPath))
                    {
                        var col = child.GetComponent<BoxCollider2D>();
                        CollisionMode mode = CollisionMode.Auto;
                        Vector2 colSize = Vector2.one;
                        Vector2 colOffset = Vector2.zero;

                        if (col != null)
                        {
                            colSize = col.size;
                            colOffset = col.offset;
                            mode = CollisionMode.Custom;
                        }

                        data.objects.Add(new UnifiedObjectData
                        {
                            prefabAssetPath = prefabPath,
                            position = child.localPosition,
                            rotation = child.localEulerAngles,
                            scale = child.localScale,
                            collisionMode = mode,
                            colliderSize = colSize,
                            colliderOffset = colOffset,
                            name = child.name
                        });
                    }
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
            
            activeMapPath = path;
            activeMapName = data.mapName;
            
            RefreshSavedMapsList();
            Debug.Log($"[Unified Painter] Saved map to: {path}");
        }

        private void LoadMapData(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Debug.LogError($"[Unified Painter] Map file not found: {path}");
                return;
            }

            string json = File.ReadAllText(path);
            UnifiedMapSaveData data = JsonUtility.FromJson<UnifiedMapSaveData>(json);
            if (data == null)
            {
                Debug.LogError($"[Unified Painter] Failed to parse map data from {path}");
                return;
            }

            ClearSceneCanvas();

            // 1. Deserialize Tiles
            if (targetTilemap != null && data.tiles != null)
            {
                foreach (var tileData in data.tiles)
                {
                    TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(tileData.tileAssetPath);
                    if (tile != null)
                    {
                        targetTilemap.SetTile(tileData.position, tile);
                    }
                }
            }

            // 2. Deserialize Objects
            Transform parent = GetTargetParent();
            if (parent != null && data.objects != null)
            {
                foreach (var objData in data.objects)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(objData.prefabAssetPath);
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                        go.transform.localPosition = objData.position;
                        go.transform.localEulerAngles = objData.rotation;
                        go.transform.localScale = objData.scale;
                        go.name = objData.name;

                        ConfigureCollider(go, objData.collisionMode, objData.scale.y);
                        
                        if (objData.collisionMode == CollisionMode.Custom)
                        {
                            var box = go.GetComponent<BoxCollider2D>();
                            if (box == null) box = go.AddComponent<BoxCollider2D>();
                            box.size = objData.colliderSize;
                            box.offset = objData.colliderOffset;
                        }

                        Undo.RegisterCreatedObjectUndo(go, "Load Map Object");
                    }
                }
            }

            activeMapPath = path;
            activeMapName = data.mapName;
            Debug.Log($"[Unified Painter] Loaded map: {path}");
        }

        private GameObject FindWallAtCell(Vector3Int cellPos)
        {
            Transform parent = GetTargetParent();
            if (parent == null || targetTilemap == null) return null;
            List<Transform> allChildren = new List<Transform>();
            GetRecursiveChildren(parent, allChildren);
            foreach (Transform child in allChildren)
            {
                if (child == null || child.GetComponent<SpriteRenderer>() == null) continue;
                if (child.name.StartsWith("wall_") || (child.GetComponentInChildren<SpriteRenderer>() != null && child.GetComponentInChildren<SpriteRenderer>().sprite != null && child.GetComponentInChildren<SpriteRenderer>().sprite.name.StartsWith("wall_")))
                {
                    Vector3Int childCell = targetTilemap.WorldToCell(child.position);
                    if (childCell == cellPos)
                    {
                        return child.gameObject;
                    }
                }
            }
            return null;
        }

        private void PaintWallWithConnections(Vector3Int cellPos, Vector3 snapPos, GameObject selectedPrefab)
        {
            Transform parent = GetTargetParent();
            if (parent == null || targetTilemap == null) return;

            GameObject existingWall = FindWallAtCell(cellPos);
            if (existingWall == null)
            {
                existingWall = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                if (parent != null)
                {
                    existingWall.transform.SetParent(parent, false);
                }
                existingWall.transform.position = snapPos + GetPlacementOffset(selectedPrefab);
                existingWall.transform.rotation = Quaternion.Euler(0f, 0f, prefabRotation * 90f);
                
                if (parent != null)
                {
                    existingWall.transform.localScale = ComputeCompensatedLocalScale(
                        parent,
                        selectedPrefab.transform.localScale * prefabScaleMultiplier,
                        prefabRotation * 90f
                    );
                }
                else
                {
                    existingWall.transform.localScale = selectedPrefab.transform.localScale * prefabScaleMultiplier;
                }

                CollisionMode activeMode = customCollisionMode == CollisionMode.Auto ? GetDefaultCollisionMode(selectedPrefab, PaletteCategory.Wall) : customCollisionMode;
                ConfigureCollider(existingWall, activeMode, prefabScaleMultiplier);

                var sr = existingWall.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 20;
                    var sorter = existingWall.GetComponent<RIMA.IsoSorter>();
                    if (sorter == null) sorter = existingWall.AddComponent<RIMA.IsoSorter>();
                }

                Undo.RegisterCreatedObjectUndo(existingWall, "Place Wall");
            }

            UpdateWallConnectionsAt(cellPos);
            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, 1, 0));  // NE
            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, -1, 0)); // SW
            UpdateWallConnectionsAt(cellPos + new Vector3Int(-1, 0, 0)); // NW
            UpdateWallConnectionsAt(cellPos + new Vector3Int(1, 0, 0));  // SE

            if (parent != null)
            {
                EditorUtility.SetDirty(parent.gameObject);
            }
        }

        private void UpdateWallConnectionsAt(Vector3Int pos)
        {
            if (targetTilemap == null) return;
            GameObject wallObj = FindWallAtCell(pos);
            if (wallObj == null) return;

            if (wallPrefabs == null || wallPrefabs.Count < 3) return;

            bool hasNE = FindWallAtCell(pos + new Vector3Int(0, 1, 0)) != null;
            bool hasSW = FindWallAtCell(pos + new Vector3Int(0, -1, 0)) != null;
            bool hasNW = FindWallAtCell(pos + new Vector3Int(-1, 0, 0)) != null;
            bool hasSE = FindWallAtCell(pos + new Vector3Int(1, 0, 0)) != null;

            GameObject newPrefab = null;
            int rotationSteps = 0;

            GameObject pNW_SE = wallPrefabs[0].prefab;
            GameObject pNE_SW = wallPrefabs.Count > 1 ? wallPrefabs[1].prefab : pNW_SE;
            GameObject pCorner = wallPrefabs.Count > 2 ? wallPrefabs[2].prefab : pNW_SE;
            GameObject pCrack = wallPrefabs.Count > 3 ? wallPrefabs[3].prefab : pNW_SE;

            if ((hasNE || hasSW) && !hasNW && !hasSE)
            {
                newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f) ? pCrack : pNE_SW;
                rotationSteps = 0;
            }
            else if ((hasNW || hasSE) && !hasNE && !hasSW)
            {
                newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f) ? pCrack : pNW_SE;
                rotationSteps = 0;
            }
            else if (hasNW && hasNE && !hasSE && !hasSW)
            {
                newPrefab = pCorner;
                rotationSteps = 0;
            }
            else if (hasNE && hasSE && !hasNW && !hasSW)
            {
                newPrefab = pCorner;
                rotationSteps = 1;
            }
            else if (hasSE && hasSW && !hasNW && !hasNE)
            {
                newPrefab = pCorner;
                rotationSteps = 2;
            }
            else if (hasSW && hasNW && !hasNE && !hasSE)
            {
                newPrefab = pCorner;
                rotationSteps = 3;
            }
            else if (hasNE || hasSW || hasNW || hasSE)
            {
                newPrefab = pCrack;
                rotationSteps = 0;
            }
            else
            {
                newPrefab = pNW_SE;
                rotationSteps = prefabRotation;
            }

            GameObject prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(wallObj);
            bool isSamePrefab = (prefabSource != null && prefabSource == newPrefab);
            int currentRotationSteps = Mathf.RoundToInt(wallObj.transform.localEulerAngles.z / 90f);

            if (!isSamePrefab || currentRotationSteps != rotationSteps)
            {
                Transform parent = wallObj.transform.parent;

                GameObject newWall = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);
                if (parent != null)
                {
                    newWall.transform.SetParent(parent, false);
                }
                newWall.transform.position = targetTilemap.GetCellCenterWorld(pos) + GetPlacementOffset(newPrefab);
                newWall.transform.rotation = Quaternion.Euler(0f, 0f, rotationSteps * 90f);
                
                if (parent != null)
                {
                    newWall.transform.localScale = ComputeCompensatedLocalScale(
                        parent,
                        newPrefab.transform.localScale * prefabScaleMultiplier,
                        rotationSteps * 90f
                    );
                }
                else
                {
                    newWall.transform.localScale = newPrefab.transform.localScale * prefabScaleMultiplier;
                }

                CollisionMode activeMode = customCollisionMode == CollisionMode.Auto ? GetDefaultCollisionMode(newPrefab, PaletteCategory.Wall) : customCollisionMode;
                ConfigureCollider(newWall, activeMode, prefabScaleMultiplier);

                var sr = newWall.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 20;
                    var sorter = newWall.GetComponent<RIMA.IsoSorter>();
                    if (sorter == null) sorter = newWall.AddComponent<RIMA.IsoSorter>();
                }

                Undo.RegisterCreatedObjectUndo(newWall, "Auto-Connect Wall");
                Undo.DestroyObjectImmediate(wallObj);
            }
        }

        private void RebuildAllWallConnections()
        {
            Transform parent = GetTargetParent();
            if (parent == null || targetTilemap == null) return;
            
            List<Vector3Int> wallCells = new List<Vector3Int>();
            List<Transform> allChildren = new List<Transform>();
            GetRecursiveChildren(parent, allChildren);
            foreach (Transform child in allChildren)
            {
                if (child == null || child.GetComponent<SpriteRenderer>() == null) continue;
                if (child.name.StartsWith("wall_") || (child.GetComponentInChildren<SpriteRenderer>() != null && child.GetComponentInChildren<SpriteRenderer>().sprite != null && child.GetComponentInChildren<SpriteRenderer>().sprite.name.StartsWith("wall_")))
                {
                    Vector3Int cell = targetTilemap.WorldToCell(child.position);
                    if (!wallCells.Contains(cell))
                    {
                        wallCells.Add(cell);
                    }
                }
            }

            if (wallCells.Count == 0) return;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Rebuild All Wall Connections");

            foreach (var cell in wallCells)
            {
                UpdateWallConnectionsAt(cell);
            }

            EditorUtility.SetDirty(parent.gameObject);
            Debug.Log($"[Unified Painter] Rebuilt connections for {wallCells.Count} walls.");
        }

        private void AttachIsoSorterToAllPlacedObjects()
        {
            Transform parent = GetTargetParent();
            if (parent == null)
            {
                Debug.LogWarning("[Unified Painter] Target parent is not assigned.");
                return;
            }

            int count = 0;
            var renderers = parent.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var sr in renderers)
            {
                if (sr == null) continue;
                if (sr.GetComponentInParent<Tilemap>() != null) continue;

                GameObject go = sr.gameObject;
                var sorter = go.GetComponent<RIMA.IsoSorter>();
                if (sorter == null)
                {
                    sorter = go.AddComponent<RIMA.IsoSorter>();
                    EditorUtility.SetDirty(go);
                    count++;
                }
            }

            Debug.Log($"[Unified Painter] Attached IsoSorter component to {count} GameObjects with SpriteRenderer.");
        }
    }
}
#endif

