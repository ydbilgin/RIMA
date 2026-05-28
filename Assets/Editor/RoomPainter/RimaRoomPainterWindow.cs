using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RimaRoomPainterWindow : EditorWindow
    {
        private const int ThumbnailSize = 64;
        private const int GridSpacing = 4;
        private const float GridScrollbarReserve = 18f;
        private const string DefaultFolderPath = "Assets/Sprites/Environment";
        private const string OtherFilter = "Other";
        private const int GameplayCliffsTab = 0;
        private const int ParallaxCliffsTab = 1;
        private const float MinWindowWidth = 1180f;
        private const float MinWindowHeight = 700f;
        private const float SplitterWidth = 5f;
        private const float MinPaneWidth = 200f;
        private const float DefaultPaletteWidth = 270f;
        private const float DefaultPreviewWidth = 580f;
        private const float DefaultInspectorWidth = 320f;
        private const string PaletteWidthKey = "RIMA.RoomPainter.PalettePx";
        private const string PreviewWidthKey = "RIMA.RoomPainter.PreviewPx";
        private const string InspectorWidthKey = "RIMA.RoomPainter.InspectorPx";
        private const string PainterModeKey = "RIMA.RoomPainter.Mode";

        // D3: Layer filter bitmask EditorPrefs key
        private const string LayerFilterMaskKey = "RIMA.RoomPainter.LayerMask";

        // D3: Mode colours for statusbar label
        private static readonly Color ModeColorTile   = new Color(0.30f, 0.55f, 1.00f, 1f);
        private static readonly Color ModeColorCliff  = new Color(0.65f, 0.65f, 0.65f, 1f);
        private static readonly Color ModeColorDecor  = new Color(0.35f, 0.80f, 0.45f, 1f);
        private static readonly Color ModeColorObject = new Color(0.95f, 0.78f, 0.20f, 1f);

        // D3: Mode → default layer filter mask (bitmask over RoomLayer values 0-9)
        // Tile   → L1 Floor (0)
        // Cliff  → L2 Cliff (2)
        // Decor  → L5 Decals (5) + L3 Wall (3)
        // Object → L4 Props (4)
        private static readonly int[] ModeDefaultLayerMask =
        {
            1 << (int)RoomLayer.Floor,                                    // Tile
            1 << (int)RoomLayer.Cliff,                                    // Cliff
            (1 << (int)RoomLayer.Decals) | (1 << (int)RoomLayer.Wall),   // Decor
            1 << (int)RoomLayer.Props                                     // Object
        };

        private static readonly string[] ModeNames = { "Tile", "Cliff", "Decor", "Object" };
        private static readonly string[] LayerFilterNames = { "Floor", "Edge", "Cliff", "Wall", "Props", "Decals", "Lighting", "Collision", "Occlusion", "Parallax" };

        private static readonly RoomLayer[] HighlightedFilterLayers =
        {
            RoomLayer.Floor,
            RoomLayer.Cliff,
            RoomLayer.Props,
            RoomLayer.Parallax
        };

        private static readonly string[] ParallaxTierNames =
        {
            "FG",
            "Playable",
            "Near",
            "Mid",
            "Far",
            "Skyline",
            "Horizon"
        };

        private static readonly string[] ParallaxTierOptions =
        {
            "FG 1.20",
            "Playable 1.00",
            "Near 0.65",
            "Mid 0.40",
            "Far 0.22",
            "Skyline 0.10",
            "Horizon 0.03"
        };

        private static readonly float[] ParallaxTierValues =
        {
            1.20f,
            1.00f,
            0.65f,
            0.40f,
            0.22f,
            0.10f,
            0.03f
        };

        [SerializeField] private AssetEntry _selectedAsset;
        [SerializeField] private RoomLayer _targetLayer = RoomLayer.Cliff;
        [SerializeField] private int _activePaletteTab = GameplayCliffsTab;
        [SerializeField] private int _selectedParallaxTier = 2;
        [SerializeField] private string _filterCategory = string.Empty;
        [SerializeField] private string _folderPath = DefaultFolderPath;
        [SerializeField] private List<AssetEntry> _assetCache = new List<AssetEntry>();
        [SerializeField] private Vector2 _paletteScroll;
        [SerializeField] private float _paletteWidth = DefaultPaletteWidth;
        [SerializeField] private float _previewWidth = DefaultPreviewWidth;
        [SerializeField] private float _inspectorWidth = DefaultInspectorWidth;

        // D3: mode + layer sub-filter
        [SerializeField] private RoomPainterMode _currentMode = RoomPainterMode.Tile;
        [SerializeField] private int _layerFilterMask = -1; // -1 = unset sentinel, real value loaded OnEnable

        // D5: last regeneration tile count for statusbar display
        private int _lastRegenCount = -1;

        private RoomPainterScenePlacer _scenePlacer;
        private RoomPainterInspectorPanel _inspectorPanel;
        private RoomPainterPreviewPane _previewPane;
        private int _activeSplitter = -1;
        private float _lastSplitterMouseX;
        private GUIStyle _modeStatusLabelStyle;

        [MenuItem("RIMA/Room Painter")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, false, "RIMA Room Painter");
            window.minSize = new Vector2(MinWindowWidth, MinWindowHeight);
            window.Show();
        }

        [MenuItem("RIMA/Room Painter Tools/Toggle Visual Collider Edit (SceneView)")]
        public static void ToggleVisualColliderEdit()
        {
            RoomPainterColliderEditor.Enabled = !RoomPainterColliderEditor.Enabled;
            Debug.Log("Room Painter: Visual Collider Edit = " + (RoomPainterColliderEditor.Enabled ? "ON" : "OFF") + ". Select a painted asset in scene to see drag handles.");
        }

        // D3: Menu surface for 4 modes (4-surface visibility hard rule)
        [MenuItem("RIMA/Room Painter Tools/Mode/Tile (1)")]
        public static void SetModeTileMenu()
        {
            foreach (RimaRoomPainterWindow w in Resources.FindObjectsOfTypeAll<RimaRoomPainterWindow>())
            {
                w.SetMode(RoomPainterMode.Tile);
            }
        }

        [MenuItem("RIMA/Room Painter Tools/Mode/Cliff (2)")]
        public static void SetModeCliffMenu()
        {
            foreach (RimaRoomPainterWindow w in Resources.FindObjectsOfTypeAll<RimaRoomPainterWindow>())
            {
                w.SetMode(RoomPainterMode.Cliff);
            }
        }

        [MenuItem("RIMA/Room Painter Tools/Mode/Decor (3)")]
        public static void SetModeDecorMenu()
        {
            foreach (RimaRoomPainterWindow w in Resources.FindObjectsOfTypeAll<RimaRoomPainterWindow>())
            {
                w.SetMode(RoomPainterMode.Decor);
            }
        }

        [MenuItem("RIMA/Room Painter Tools/Mode/Object (4)")]
        public static void SetModeObjectMenu()
        {
            foreach (RimaRoomPainterWindow w in Resources.FindObjectsOfTypeAll<RimaRoomPainterWindow>())
            {
                w.SetMode(RoomPainterMode.Object);
            }
        }

        [MenuItem("RIMA/Room Painter Tools/Generate Metadata for All Sprites")]
        public static void GenerateMetadataForAllSprites()
        {
            int created = 0;
            string[] folders = { "Assets/Sprites", "Assets/Prefabs" };
            for (int i = 0; i < folders.Length; i++)
            {
                created += RoomPainterAssetPostprocessor.BackfillMetadataUnderFolder(folders[i]);
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(
                "Room Painter",
                "Metadata backfill complete.\nCreated: " + created + " new asset metadata file(s).",
                "OK");

            foreach (RimaRoomPainterWindow openWindow in Resources.FindObjectsOfTypeAll<RimaRoomPainterWindow>())
            {
                openWindow.RefreshAssetCache();
                openWindow.Repaint();
            }
        }

        private void OnEnable()
        {
            minSize = new Vector2(MinWindowWidth, MinWindowHeight);

            if (string.IsNullOrEmpty(_folderPath))
            {
                _folderPath = DefaultFolderPath;
            }

            _paletteWidth = EditorPrefs.GetFloat(PaletteWidthKey, DefaultPaletteWidth);
            _previewWidth = EditorPrefs.GetFloat(PreviewWidthKey, DefaultPreviewWidth);
            _inspectorWidth = EditorPrefs.GetFloat(InspectorWidthKey, DefaultInspectorWidth);

            // D3: restore mode + layer filter
            _currentMode = (RoomPainterMode)EditorPrefs.GetInt(PainterModeKey, (int)RoomPainterMode.Tile);
            int savedMask = EditorPrefs.GetInt(LayerFilterMaskKey, -1);
            _layerFilterMask = savedMask == -1 ? ModeDefaultLayerMask[(int)_currentMode] : savedMask;

            if (_scenePlacer == null)
            {
                _scenePlacer = new RoomPainterScenePlacer();
            }

            if (_inspectorPanel == null)
            {
                _inspectorPanel = new RoomPainterInspectorPanel();
            }

            if (_previewPane == null)
            {
                _previewPane = new RoomPainterPreviewPane();
            }

            SceneView.duringSceneGui -= OnSceneGui;
            SceneView.duringSceneGui += OnSceneGui;
            CliffHoverIndicator.Active = (_currentMode == RoomPainterMode.Cliff);
            RoomPainterAssetEvents.AssetCreatedOrUpdated -= OnAssetCreatedOrUpdated;
            RoomPainterAssetEvents.AssetCreatedOrUpdated += OnAssetCreatedOrUpdated;
            RoomPainterAssetEvents.AssetDeleted -= OnAssetDeleted;
            RoomPainterAssetEvents.AssetDeleted += OnAssetDeleted;
            SyncTargetLayerWithTab();
            RefreshAssetCache();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGui;
            RoomPainterAssetEvents.AssetCreatedOrUpdated -= OnAssetCreatedOrUpdated;
            RoomPainterAssetEvents.AssetDeleted -= OnAssetDeleted;
            SavePaneWidths();
            CliffHoverIndicator.Active = false;

            if (_scenePlacer != null)
            {
                _scenePlacer.Reset();
            }
        }

        private void OnGUI()
        {
            minSize = new Vector2(MinWindowWidth, MinWindowHeight);
            HandleModeHotkeys();
            SyncTargetLayerWithTab();
            ClampPaneWidths();
            DrawToolbar();
            DrawModeToolbar();
            DrawLayerSubFilter();
            DrawCategoryFilters();
            DrawMainPanels();
            DrawStatusBar();
        }

        private void HandleModeHotkeys()
        {
            Event evt = Event.current;
            if (evt == null || evt.type != EventType.KeyDown)
            {
                return;
            }

            bool changed = false;
            if (evt.keyCode == KeyCode.Alpha1 || evt.keyCode == KeyCode.Keypad1)
            {
                _currentMode = RoomPainterMode.Tile;
                changed = true;
            }
            else if (evt.keyCode == KeyCode.Alpha2 || evt.keyCode == KeyCode.Keypad2)
            {
                _currentMode = RoomPainterMode.Cliff;
                changed = true;
            }
            else if (evt.keyCode == KeyCode.Alpha3 || evt.keyCode == KeyCode.Keypad3)
            {
                _currentMode = RoomPainterMode.Decor;
                changed = true;
            }
            else if (evt.keyCode == KeyCode.Alpha4 || evt.keyCode == KeyCode.Keypad4)
            {
                _currentMode = RoomPainterMode.Object;
                changed = true;
            }
            // D5: C = Regenerate cliff (Cliff mode only)
            else if (evt.keyCode == KeyCode.C && _currentMode == RoomPainterMode.Cliff)
            {
                TriggerCliffRegenerate();
                evt.Use();
                Repaint();
                return;
            }

            if (changed)
            {
                ApplyModeDefaults();
                evt.Use();
                Repaint();
            }
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Folder:", EditorStyles.miniLabel, GUILayout.Width(44f));

                EditorGUI.BeginChangeCheck();
                string nextFolderPath = EditorGUILayout.TextField(_folderPath, EditorStyles.toolbarTextField, GUILayout.MinWidth(220f));
                if (EditorGUI.EndChangeCheck())
                {
                    _folderPath = nextFolderPath;
                    RefreshAssetCache();
                }

                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(64f)))
                {
                    RefreshAssetCache();
                }

                GUILayout.Space(12f);
                GUILayout.Label("Target: " + _targetLayer, EditorStyles.miniLabel, GUILayout.Width(100f));

                if (HasSelection() && _selectedAsset.suggestedLayer != _targetLayer)
                {
                    GUIContent warningContent = EditorGUIUtility.IconContent("console.warnicon.sml");
                    warningContent.tooltip = "Selected asset suggested layer differs from target layer.";
                    GUILayout.Label(warningContent, GUILayout.Width(22f), GUILayout.Height(18f));
                }

                GUILayout.FlexibleSpace();

                bool editEnabled = RoomPainterColliderEditor.Enabled;
                Color prevBg = GUI.backgroundColor;
                if (editEnabled)
                {
                    GUI.backgroundColor = new Color(0.30f, 0.95f, 0.60f, 1f);
                }

                GUIContent toggleContent = new GUIContent(
                    editEnabled ? "Edit Hitbox: ON" : "Edit Hitbox: OFF",
                    "Toggle SceneView drag handles for the selected painted asset's Collider2D. " +
                    "When ON: select a painted GameObject in scene → green outline + yellow drag dots appear.");
                if (GUILayout.Toggle(editEnabled, toggleContent, EditorStyles.toolbarButton, GUILayout.Width(120f)) != editEnabled)
                {
                    RoomPainterColliderEditor.Enabled = !editEnabled;
                }

                GUI.backgroundColor = prevBg;

                // F6 — Launch Live Tool button (spec: right of Edit Hitbox).
                GUILayout.Space(6f);
                bool liveRunning = RIMA.Editor.RoomPainter.LiveTool.LiveToolLauncher.IsRunning;
                Color liveColor = liveRunning
                    ? new Color(0.20f, 0.90f, 0.55f, 1f)   // green tint when live
                    : new Color(0.60f, 0.85f, 1.00f, 1f);  // cyan tint when idle
                GUI.backgroundColor = liveColor;
                string liveLabel = liveRunning ? "Live Tool: ON" : "Launch Live Tool";
                GUIContent liveContent = new GUIContent(
                    liveLabel,
                    liveRunning
                        ? RIMA.Editor.RoomPainter.LiveTool.LiveToolLauncher.StatusText + "\nClick to stop."
                        : "Serialize current room to room_current.json and launch Tool.exe + Game.exe side by side.");
                if (GUILayout.Button(liveContent, EditorStyles.toolbarButton, GUILayout.Width(130f)))
                {
                    if (liveRunning)
                        RIMA.Editor.RoomPainter.LiveTool.LiveToolLauncher.StopAll();
                    else
                        RIMA.Editor.RoomPainter.LiveTool.LiveToolLauncher.Launch();
                }

                GUI.backgroundColor = prevBg;
            }
        }

        private void DrawModeToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                Color prevBg = GUI.backgroundColor;

                for (int i = 0; i < ModeNames.Length; i++)
                {
                    RoomPainterMode mode = (RoomPainterMode)i;
                    bool active = _currentMode == mode;
                    GUI.backgroundColor = active ? GetModeColor(mode) : prevBg;

                    string label = ModeNames[i] + " (" + (i + 1) + ")";
                    if (GUILayout.Toggle(active, label, EditorStyles.toolbarButton, GUILayout.Width(90f)) && !active)
                    {
                        SetMode(mode);
                    }
                }

                GUI.backgroundColor = prevBg;
                GUILayout.FlexibleSpace();
            }
        }

        private void DrawLayerSubFilter()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(4f);
                GUILayout.Label("Layers:", EditorStyles.miniLabel, GUILayout.Width(44f));

                // "All" toggle — set all bits
                int allMask = (1 << LayerFilterNames.Length) - 1;
                bool allActive = _layerFilterMask == allMask;
                if (GUILayout.Toggle(allActive, "All", EditorStyles.miniButtonLeft, GUILayout.Width(34f)) && !allActive)
                {
                    _layerFilterMask = allMask;
                    EditorPrefs.SetInt(LayerFilterMaskKey, _layerFilterMask);
                    Repaint();
                }

                for (int i = 0; i < LayerFilterNames.Length; i++)
                {
                    bool layerActive = (_layerFilterMask & (1 << i)) != 0;
                    GUIStyle chipStyle = i == LayerFilterNames.Length - 1
                        ? EditorStyles.miniButtonRight
                        : EditorStyles.miniButtonMid;
                    bool next = GUILayout.Toggle(layerActive, LayerFilterNames[i], chipStyle, GUILayout.Width(66f));
                    if (next != layerActive)
                    {
                        if (next)
                        {
                            _layerFilterMask |= 1 << i;
                        }
                        else
                        {
                            _layerFilterMask &= ~(1 << i);
                        }

                        EditorPrefs.SetInt(LayerFilterMaskKey, _layerFilterMask);
                        Repaint();
                    }
                }

                GUILayout.FlexibleSpace();
            }
        }

        private void DrawCategoryFilters()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(4f);
                DrawFilterChip("All", string.Empty, EditorStyles.miniButtonLeft);
                DrawFilterChip("Floor", RoomLayer.Floor.ToString(), EditorStyles.miniButtonMid);
                DrawFilterChip("Cliff", RoomLayer.Cliff.ToString(), EditorStyles.miniButtonMid);
                DrawFilterChip("Props", RoomLayer.Props.ToString(), EditorStyles.miniButtonMid);
                DrawFilterChip("Parallax", RoomLayer.Parallax.ToString(), EditorStyles.miniButtonMid);
                DrawFilterChip("All others", OtherFilter, EditorStyles.miniButtonRight);
                GUILayout.FlexibleSpace();
            }
        }

        private void DrawFilterChip(string label, string filterValue, GUIStyle style)
        {
            bool isSelected = _filterCategory == filterValue;
            if (GUILayout.Toggle(isSelected, label, style, GUILayout.Width(78f)))
            {
                _filterCategory = filterValue;
            }
        }

        private void DrawMainPanels()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandHeight(true)))
            {
                DrawPalettePanel(_paletteWidth);
                DrawSplitter(0);
                DrawPreviewPanel(_previewWidth);
                DrawSplitter(1);
                DrawInspectorPanel(_inspectorWidth);
            }
        }

        private void DrawPalettePanel(float width)
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(width), GUILayout.ExpandHeight(true)))
            {
                DrawPaletteTabs();
                _paletteScroll = EditorGUILayout.BeginScrollView(_paletteScroll, false, true);
                DrawAssetGrid();
                EditorGUILayout.EndScrollView();
            }
        }

        private int ComputeGridColumns()
        {
            float content = Mathf.Max(0f, _paletteWidth - GridScrollbarReserve - GridSpacing * 2f);
            int columns = Mathf.FloorToInt(content / (ThumbnailSize + GridSpacing));
            return Mathf.Max(1, columns);
        }

        private void DrawPaletteTabs()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                Color previousColor = GUI.backgroundColor;
                GUI.backgroundColor = _activePaletteTab == GameplayCliffsTab ? new Color(0.4f, 0.9f, 1f, 1f) : previousColor;
                if (GUILayout.Toggle(_activePaletteTab == GameplayCliffsTab, "Gameplay Cliffs", EditorStyles.miniButtonLeft, GUILayout.Height(22f)))
                {
                    SetPaletteTab(GameplayCliffsTab);
                }

                GUI.backgroundColor = _activePaletteTab == ParallaxCliffsTab ? new Color(0.8f, 0.5f, 1f, 1f) : previousColor;
                if (GUILayout.Toggle(_activePaletteTab == ParallaxCliffsTab, "Parallax BG Cliffs", EditorStyles.miniButtonRight, GUILayout.Height(22f)))
                {
                    SetPaletteTab(ParallaxCliffsTab);
                }

                GUI.backgroundColor = previousColor;
            }

            if (_activePaletteTab == ParallaxCliffsTab)
            {
                EditorGUI.BeginChangeCheck();
                _selectedParallaxTier = EditorGUILayout.Popup("Tier", _selectedParallaxTier, ParallaxTierOptions);
                if (EditorGUI.EndChangeCheck())
                {
                    SceneView.RepaintAll();
                }
            }
        }

        private void DrawAssetGrid()
        {
            int columns = ComputeGridColumns();
            int visibleCount = 0;
            using (new EditorGUILayout.VerticalScope())
            {
                for (int i = 0; i < _assetCache.Count; i++)
                {
                    AssetEntry entry = _assetCache[i];
                    if (!MatchesCurrentFilter(entry))
                    {
                        continue;
                    }

                    if (visibleCount % columns == 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                    }

                    DrawAssetButton(entry);
                    visibleCount++;

                    if (visibleCount % columns == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                    }
                }

                if (visibleCount % columns != 0)
                {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }

                if (visibleCount == 0)
                {
                    EditorGUILayout.HelpBox("No sprite or prefab assets found for this filter.", MessageType.Info);
                }
            }
        }

        private void DrawAssetButton(AssetEntry entry)
        {
            UnityEngine.Object assetObject = entry.AssetObject;
            string assetName = assetObject != null ? assetObject.name : entry.path;
            Texture2D preview = assetObject != null ? AssetPreview.GetAssetPreview(assetObject) : null;
            if (preview == null && assetObject != null)
            {
                preview = AssetPreview.GetMiniThumbnail(assetObject) as Texture2D;
            }

            Color previousColor = GUI.backgroundColor;
            if (HasSelection() && _selectedAsset.path == entry.path)
            {
                GUI.backgroundColor = Color.cyan;
            }

            GUIContent content = new GUIContent(preview, assetName);
            if (GUILayout.Button(content, GUILayout.Width(ThumbnailSize), GUILayout.Height(ThumbnailSize)))
            {
                if (!HasSelection() || _selectedAsset.path != entry.path)
                {
                    _selectedAsset = entry;
                    EnsureMetadataForSelection();
                    Repaint();
                }
            }

            GUI.backgroundColor = previousColor;
        }

        private void DrawPreviewPanel(float width)
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(width), GUILayout.ExpandHeight(true)))
            {
                if (_previewPane == null)
                {
                    _previewPane = new RoomPainterPreviewPane();
                }

                Rect area = GUILayoutUtility.GetRect(width, 10f, GUILayout.Width(width), GUILayout.ExpandHeight(true));
                _previewPane.Draw(area, HasSelection() ? _selectedAsset : default, _targetLayer, 0f);
            }
        }

        private void DrawInspectorPanel(float width)
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(width), GUILayout.ExpandHeight(true)))
            {
                if (_inspectorPanel == null)
                {
                    _inspectorPanel = new RoomPainterInspectorPanel();
                }

                // D3: pass current mode for mode-specific section
                _inspectorPanel.Draw(HasSelection() ? _selectedAsset.metadata : null, Selection.activeGameObject, _currentMode);
            }
        }

        private void DrawSplitter(int index)
        {
            Rect splitterRect = GUILayoutUtility.GetRect(SplitterWidth, 10f, GUILayout.Width(SplitterWidth), GUILayout.ExpandHeight(true));
            EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);
            EditorGUI.DrawRect(splitterRect, new Color(0.12f, 0.12f, 0.12f, 1f));

            Event current = Event.current;
            if (current.type == EventType.MouseDown && current.button == 0 && splitterRect.Contains(current.mousePosition))
            {
                _activeSplitter = index;
                _lastSplitterMouseX = current.mousePosition.x;
                current.Use();
            }
            else if (current.type == EventType.MouseDrag && _activeSplitter == index)
            {
                float delta = current.mousePosition.x - _lastSplitterMouseX;
                ResizePanes(index, delta);
                _lastSplitterMouseX = current.mousePosition.x;
                current.Use();
                Repaint();
            }
            else if (current.type == EventType.MouseUp && _activeSplitter == index)
            {
                _activeSplitter = -1;
                SavePaneWidths();
                current.Use();
            }
        }

        private void ResizePanes(int splitterIndex, float delta)
        {
            float availableWidth = GetAvailablePaneWidth();

            if (splitterIndex == 0)
            {
                float combinedWidth = _paletteWidth + _previewWidth;
                _paletteWidth = Mathf.Clamp(_paletteWidth + delta, MinPaneWidth, combinedWidth - MinPaneWidth);
                _previewWidth = combinedWidth - _paletteWidth;
            }
            else
            {
                float maxPreviewWidth = availableWidth - _paletteWidth - MinPaneWidth;
                _previewWidth = Mathf.Clamp(_previewWidth + delta, MinPaneWidth, maxPreviewWidth);
                _inspectorWidth = availableWidth - _paletteWidth - _previewWidth;
            }

            ClampPaneWidths();
            SavePaneWidths();
        }

        private void ClampPaneWidths()
        {
            float availableWidth = GetAvailablePaneWidth();
            _paletteWidth = Mathf.Clamp(_paletteWidth, MinPaneWidth, availableWidth - MinPaneWidth * 2f);
            _previewWidth = Mathf.Clamp(_previewWidth, MinPaneWidth, availableWidth - _paletteWidth - MinPaneWidth);
            _inspectorWidth = availableWidth - _paletteWidth - _previewWidth;

            if (_inspectorWidth < MinPaneWidth)
            {
                float deficit = MinPaneWidth - _inspectorWidth;
                _inspectorWidth = MinPaneWidth;
                _previewWidth = Mathf.Max(MinPaneWidth, _previewWidth - deficit);
                _paletteWidth = Mathf.Clamp(availableWidth - _previewWidth - _inspectorWidth, MinPaneWidth, availableWidth - MinPaneWidth * 2f);
            }
        }

        private float GetAvailablePaneWidth()
        {
            return Mathf.Max(MinPaneWidth * 3f, position.width - SplitterWidth * 2f - 8f);
        }

        private void SavePaneWidths()
        {
            EditorPrefs.SetFloat(PaletteWidthKey, _paletteWidth);
            EditorPrefs.SetFloat(PreviewWidthKey, _previewWidth);
            EditorPrefs.SetFloat(InspectorWidthKey, _inspectorWidth);
        }

        private void DrawStatusBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // D3: Mode label (left, coloured)
                GUIStyle modeStyle = GetModeStatusStyle();
                Color prevContent = GUI.contentColor;
                GUI.contentColor = GetModeColor(_currentMode);
                GUILayout.Label("Mode: " + ModeNames[(int)_currentMode], modeStyle, GUILayout.Width(80f));
                GUI.contentColor = prevContent;

                GUILayout.Space(8f);

                // D5: show cliff erase counter and last regen count when in Cliff mode
                if (_currentMode == RoomPainterMode.Cliff)
                {
                    RIMA.Environment.CliffAutoPlacer statusPlacer =
                        Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
                    if (statusPlacer != null && statusPlacer.ManualOverrideCells.Count > 0)
                    {
                        GUILayout.Label("Erased: " + statusPlacer.ManualOverrideCells.Count, EditorStyles.miniLabel, GUILayout.Width(72f));
                    }

                    if (_lastRegenCount >= 0)
                    {
                        GUILayout.Label("Cliff regenerated: " + _lastRegenCount + " tiles", EditorStyles.miniLabel, GUILayout.Width(160f));
                    }

                    // D5.5: Shift-held indicator + decor paint counter
                    Event curEvt = Event.current;
                    if (curEvt != null && curEvt.shift)
                    {
                        Color prevDecorContent = GUI.contentColor;
                        GUI.contentColor = new Color(0.25f, 0.95f, 0.95f, 1f);
                        GUILayout.Label("Free-form Decor mode", EditorStyles.miniLabel, GUILayout.Width(148f));
                        GUI.contentColor = prevDecorContent;
                    }

                    if (DecorCliffPainter.DecorPaintedThisSession > 0)
                    {
                        GUILayout.Label("Decor cliff painted: " + DecorCliffPainter.DecorPaintedThisSession, EditorStyles.miniLabel, GUILayout.Width(148f));
                    }
                }

                if (HasSelection())
                {
                    string selectedName = _selectedAsset.AssetObject != null ? _selectedAsset.AssetObject.name : _selectedAsset.path;
                    GUILayout.Label("Selected: " + selectedName + " [" + _selectedAsset.suggestedLayer + "]", EditorStyles.miniLabel);
                }
                else
                {
                    GUILayout.Label("No selection", EditorStyles.miniLabel);
                }

                GUILayout.FlexibleSpace();

                // D4: Prefab Mode indicator — shown whenever Prefab Mode is active
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    string prefabName = System.IO.Path.GetFileNameWithoutExtension(prefabStage.assetPath);
                    GUIStyle prefabLabelStyle = new GUIStyle(EditorStyles.miniLabel) { fontStyle = UnityEngine.FontStyle.Bold };
                    Color prevContentD4 = GUI.contentColor;
                    GUI.contentColor = new Color(0.95f, 0.78f, 0.20f, 1f);
                    GUILayout.Label("Editing prefab: " + prefabName, prefabLabelStyle);
                    GUI.contentColor = prevContentD4;
                    Repaint();
                }
                else if (RoomPainterColliderEditor.Enabled)
                {
                    GameObject sel = Selection.activeGameObject;
                    bool hasBinding = sel != null && sel.GetComponent<RIMA.RoomPainter.RoomPainterAssetBinding>() != null;
                    string hint = hasBinding
                        ? "Hitbox editing live on '" + sel.name + "' — drag yellow dots in SceneView"
                        : "Hitbox edit ON — select a painted GameObject in the scene";
                    GUILayout.Label(hint, EditorStyles.miniLabel);
                }
            }
        }

        public void RefreshAssetCache()
        {
            _assetCache = RoomPainterAssetScanner.Scan(_folderPath);

            if (HasSelection())
            {
                bool selectionStillExists = false;
                for (int i = 0; i < _assetCache.Count; i++)
                {
                    if (_assetCache[i].path == _selectedAsset.path)
                    {
                        _selectedAsset = _assetCache[i];
                        selectionStillExists = true;
                        break;
                    }
                }

                if (!selectionStillExists)
                {
                    _selectedAsset = default;
                }
            }
        }

        private void EnsureMetadataForSelection()
        {
            if (!HasSelection() || _selectedAsset.metadata != null)
            {
                return;
            }

            RoomPainterAsset metadata = RoomPainterAssetPostprocessor.EnsureMetadataForAssetPath(_selectedAsset.path);
            if (metadata == null)
            {
                return;
            }

            AssetEntry updated = _selectedAsset;
            updated.metadata = metadata;
            _selectedAsset = updated;

            for (int i = 0; i < _assetCache.Count; i++)
            {
                if (_assetCache[i].path == _selectedAsset.path)
                {
                    AssetEntry cached = _assetCache[i];
                    cached.metadata = metadata;
                    _assetCache[i] = cached;
                    break;
                }
            }
        }

        private bool MatchesCurrentFilter(AssetEntry entry)
        {
            // D3: layer sub-filter (bitmask) gates visibility before category filter
            int layerBit = 1 << (int)entry.suggestedLayer;
            int allMask = (1 << LayerFilterNames.Length) - 1;
            bool allActive = _layerFilterMask == allMask;
            if (!allActive && (_layerFilterMask & layerBit) == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(_filterCategory))
            {
                return true;
            }

            if (_filterCategory == OtherFilter)
            {
                for (int i = 0; i < HighlightedFilterLayers.Length; i++)
                {
                    if (entry.suggestedLayer == HighlightedFilterLayers[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return entry.suggestedLayer.ToString() == _filterCategory;
        }

        // D5: Cliff regenerate helper — finds CliffAutoPlacer, calls Regenerate(), updates statusbar
        private void TriggerCliffRegenerate()
        {
            RIMA.Environment.CliffAutoPlacer placer =
                Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
            if (placer == null)
            {
                Debug.LogWarning("[RoomPainter] No CliffAutoPlacer found in scene.");
                return;
            }

            placer.Regenerate();
            _lastRegenCount = placer.LastGeneratedCount;
            SceneView.RepaintAll();
        }

        // D3: mode helpers
        public void SetMode(RoomPainterMode mode)
        {
            if (_currentMode == mode)
            {
                return;
            }

            _currentMode = mode;
            ApplyModeDefaults();
            CliffHoverIndicator.Active = (_currentMode == RoomPainterMode.Cliff);
            Repaint();
            SceneView.RepaintAll();
        }

        private void ApplyModeDefaults()
        {
            _layerFilterMask = ModeDefaultLayerMask[(int)_currentMode];
            EditorPrefs.SetInt(PainterModeKey, (int)_currentMode);
            EditorPrefs.SetInt(LayerFilterMaskKey, _layerFilterMask);
            CliffHoverIndicator.Active = (_currentMode == RoomPainterMode.Cliff);
        }

        private static Color GetModeColor(RoomPainterMode mode)
        {
            switch (mode)
            {
                case RoomPainterMode.Tile:   return ModeColorTile;
                case RoomPainterMode.Cliff:  return ModeColorCliff;
                case RoomPainterMode.Decor:  return ModeColorDecor;
                case RoomPainterMode.Object: return ModeColorObject;
                default:                     return Color.white;
            }
        }

        private GUIStyle GetModeStatusStyle()
        {
            if (_modeStatusLabelStyle == null)
            {
                _modeStatusLabelStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    fontStyle = UnityEngine.FontStyle.Bold
                };
            }

            return _modeStatusLabelStyle;
        }

        private void OnSceneGui(SceneView sceneView)
        {
            if (_scenePlacer == null)
            {
                _scenePlacer = new RoomPainterScenePlacer();
            }

            SyncTargetLayerWithTab();

            // D5.5: In Cliff mode with Shift held → route to DecorCliffPainter (free-form)
            if (_currentMode == RoomPainterMode.Cliff)
            {
                RIMA.Environment.CliffAutoPlacer placer =
                    Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
                TileBase cliffTile = placer != null ? placer.cliffTile : null;

                bool consumed = DecorCliffPainter.OnSceneGui(sceneView, cliffTile);
                if (consumed)
                {
                    Repaint(); // refresh statusbar counter
                    return;
                }
            }

            _scenePlacer.OnSceneGUI(
                sceneView,
                _selectedAsset,
                _targetLayer,
                GetCurrentParallaxTierName(),
                GetCurrentParallaxTierValue(),
                _selectedParallaxTier,
                _filterCategory,
                _assetCache,
                SelectAssetFromScene);
        }

        private void SelectAssetFromScene(AssetEntry entry)
        {
            _selectedAsset = entry;
            EnsureMetadataForSelection();
            Repaint();
        }

        private void OnAssetCreatedOrUpdated(RoomPainterAsset asset)
        {
            RefreshAssetCache();
            Repaint();
        }

        private void OnAssetDeleted(string guid)
        {
            RefreshAssetCache();
            Repaint();
        }

        private void SetPaletteTab(int tab)
        {
            if (_activePaletteTab == tab)
            {
                return;
            }

            _activePaletteTab = tab;
            SyncTargetLayerWithTab();
            Repaint();
            SceneView.RepaintAll();
        }

        private void SyncTargetLayerWithTab()
        {
            _targetLayer = _activePaletteTab == ParallaxCliffsTab ? RoomLayer.Parallax : RoomLayer.Cliff;

            if (_selectedParallaxTier < 0 || _selectedParallaxTier >= ParallaxTierValues.Length)
            {
                _selectedParallaxTier = 2;
            }
        }

        private string GetCurrentParallaxTierName()
        {
            return ParallaxTierNames[_selectedParallaxTier];
        }

        private float GetCurrentParallaxTierValue()
        {
            return ParallaxTierValues[_selectedParallaxTier];
        }

        private bool HasSelection()
        {
            return !string.IsNullOrEmpty(_selectedAsset.path);
        }
    }
}
