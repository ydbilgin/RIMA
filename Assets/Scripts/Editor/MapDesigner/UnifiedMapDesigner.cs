#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RIMA.Editor.Map;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using RIMA.RoomPainter;
using RIMA.RoomPainter.Editor;
using RIMA.Live;

namespace RIMA.Editor.MapDesigner
{
    /// <summary>
    /// THE unified room/level designer (single front door — "RIMA/Map Designer").
    /// Replaces the old reflection-into-two-hidden-windows shell. It is a thin VIEW over the
    /// surface-agnostic <see cref="UnifiedDesignerCore"/> (the same core the in-game F2 overlay
    /// drives), so both surfaces share one data path and can never drift.
    ///
    /// Tabs: Library (room select+edit) · Floor · Cliff · Object · Portal · Light · Layers.
    /// Painting happens in the SceneView: pick a category tab + a palette asset, then left-click
    /// to place / right-click to erase on the active Grid. Cliff tab generates the cliff ring
    /// logically from the floor shape.
    /// </summary>
    public class UnifiedMapDesigner : EditorWindow
    {
        private enum Tab { Rooms, Library, Floor, Cliff, Object, Portal, Light, Layers }

        private const string RoomsRoot = "Assets/Data/Rooms";

        private readonly RoomDataAuthoringController _controller = new RoomDataAuthoringController();
        private readonly UnifiedDesignerCore _core = new UnifiedDesignerCore();
        private readonly RoomDataComposer _composer = new RoomDataComposer();

        private Tab _tab = Tab.Rooms;
        private Vector2 _roomsScroll;
        private Vector2 _libScroll;
        private Vector2 _paletteScroll;
        private string _roomSearch = string.Empty;
        private string _search = string.Empty;
        private bool _painting;
        private int _autoPropsSeed = 12345;
        private RoomTemplateSO _selectedTemplate;
        private List<RoomTemplateSO> _roomTemplates = new List<RoomTemplateSO>();

        [MenuItem("RIMA/Map Designer", priority = 1)]
        public static void Open()
        {
            var w = GetWindow<UnifiedMapDesigner>();
            w.titleContent = new GUIContent("RIMA Map Designer");
            w.minSize = new Vector2(720, 480);
        }

        private void OnEnable()
        {
            _controller.RefreshLibrary();
            RefreshRoomTemplates();
            _core.BeforeMutate = (room, label) => { if (room != null) Undo.RecordObject(room, label); };
            _core.Changed += OnCoreChanged;
            SceneView.duringSceneGui += OnSceneGui;
        }

        private void OnDisable()
        {
            _core.Changed -= OnCoreChanged;
            SceneView.duringSceneGui -= OnSceneGui;
        }

        private void OnCoreChanged()
        {
            if (_core.ActiveRoom != null)
            {
                EditorUtility.SetDirty(_core.ActiveRoom);
                _composer.Compose(_core.ActiveRoom);
                RoomDataAuthoringController.WriteLiveRoom(_core.ActiveRoom);
                EditorSceneManager_MarkActiveDirty();
                SceneView.RepaintAll();
            }

            Repaint();
        }

        // ── main GUI ───────────────────────────────────────────────────────
        private void OnGUI()
        {
            DrawToolbar();
            DrawTabBar();
            EditorGUILayout.Space(2);

            switch (_tab)
            {
                case Tab.Rooms: DrawRooms(); break;
                case Tab.Library: DrawLibrary(); break;
                case Tab.Layers: DrawLayers(); break;
                default: DrawCategory(TabToCategory(_tab)); break;
            }

            DrawStatusBar();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                string roomName = _controller.ActiveRoom != null ? _controller.ActiveRoom.displayName : "<no room>";
                GUILayout.Label("Room: " + roomName, EditorStyles.toolbarButton, GUILayout.Width(220));

                using (new EditorGUI.DisabledScope(_controller.ActiveRoom == null))
                {
                    if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(60)))
                        SaveActive();
                    if (GUILayout.Button("Build Preview", EditorStyles.toolbarButton, GUILayout.Width(100)))
                        BuildPreview();
                }

                GUILayout.FlexibleSpace();
                _painting = GUILayout.Toggle(_painting, "Paint in SceneView", EditorStyles.toolbarButton, GUILayout.Width(140));
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
                {
                    _controller.RefreshLibrary();
                    RefreshRoomTemplates();
                }
            }
        }

        private void DrawTabBar()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (Tab t in System.Enum.GetValues(typeof(Tab)))
                {
                    bool active = _tab == t;
                    Color prev = GUI.backgroundColor;
                    if (active) GUI.backgroundColor = new Color(0.4f, 0.7f, 1f);
                    if (GUILayout.Button(t.ToString(), active ? EditorStyles.miniButtonMid : EditorStyles.miniButton))
                    {
                        _tab = t;
                        if (t != Tab.Library && t != Tab.Layers)
                            _core.Category = TabToCategory(t);
                    }
                    GUI.backgroundColor = prev;
                }
            }
        }

        // -- Rooms tab (RoomTemplateSO front door) ---------------------------
        private void RefreshRoomTemplates()
        {
            _roomTemplates = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { RoomsRoot })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<RoomTemplateSO>)
                .Where(t => t != null)
                .OrderBy(AssetDatabase.GetAssetPath)
                .ToList();

            if (_selectedTemplate == null && _roomTemplates.Count > 0)
            {
                _selectedTemplate = _roomTemplates[0];
                _autoPropsSeed = RoomTemplateAutoPropsUtility.StableSeed(_selectedTemplate.name);
            }
        }

        private void DrawRooms()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(310f)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        _roomSearch = EditorGUILayout.TextField(_roomSearch, EditorStyles.toolbarSearchField);
                        if (GUILayout.Button("Refresh", GUILayout.Width(70f)))
                        {
                            RefreshRoomTemplates();
                        }
                    }

                    EditorGUILayout.LabelField($"RoomTemplateSO Library ({_roomTemplates.Count})", EditorStyles.boldLabel);
                    _roomsScroll = EditorGUILayout.BeginScrollView(_roomsScroll);
                    string currentFolder = null;
                    for (int i = 0; i < _roomTemplates.Count; i++)
                    {
                        RoomTemplateSO template = _roomTemplates[i];
                        if (template == null) continue;

                        string path = AssetDatabase.GetAssetPath(template);
                        if (!MatchesRoomSearch(template, path)) continue;

                        string folder = Path.GetDirectoryName(path)?.Replace('\\', '/');
                        if (folder != currentFolder)
                        {
                            currentFolder = folder;
                            EditorGUILayout.Space(5f);
                            EditorGUILayout.LabelField(string.IsNullOrEmpty(folder) ? "Rooms" : folder.Replace(RoomsRoot, "Rooms"), EditorStyles.boldLabel);
                        }

                        bool selected = _selectedTemplate == template;
                        Color previous = GUI.backgroundColor;
                        if (selected) GUI.backgroundColor = new Color(0.45f, 0.75f, 1f);
                        if (GUILayout.Button(template.name, selected ? EditorStyles.miniButtonMid : EditorStyles.miniButton, GUILayout.Height(22f)))
                        {
                            _selectedTemplate = template;
                            _autoPropsSeed = RoomTemplateAutoPropsUtility.StableSeed(template.name);
                            GUI.FocusControl(null);
                        }
                        GUI.backgroundColor = previous;
                    }
                    EditorGUILayout.EndScrollView();
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    DrawSelectedTemplateHeader();
                    DrawSelectedTemplateActions();
                    DrawTemplatePreview(_selectedTemplate);
                }
            }
        }

        private bool MatchesRoomSearch(RoomTemplateSO template, string path)
        {
            if (string.IsNullOrWhiteSpace(_roomSearch))
            {
                return true;
            }

            string needle = _roomSearch.Trim();
            return template.name.IndexOf(needle, System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                   (!string.IsNullOrEmpty(template.roomId) && template.roomId.IndexOf(needle, System.StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (!string.IsNullOrEmpty(path) && path.IndexOf(needle, System.StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void DrawSelectedTemplateHeader()
        {
            if (_selectedTemplate == null)
            {
                EditorGUILayout.HelpBox("No RoomTemplateSO found under Assets/Data/Rooms.", MessageType.Warning);
                return;
            }

            bool dirty = EditorUtility.IsDirty(_selectedTemplate);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(_selectedTemplate.name, EditorStyles.boldLabel);
                if (dirty)
                {
                    GUILayout.Label("* Unsaved Changes", EditorStyles.miniLabel, GUILayout.Width(130f));
                }
            }

            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(_selectedTemplate), EditorStyles.miniLabel);
        }

        private void DrawSelectedTemplateActions()
        {
            using (new EditorGUI.DisabledScope(_selectedTemplate == null))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (DrawColoredButton("Build in Arena", new Color(0.35f, 0.8f, 0.35f), GUILayout.Height(28f)))
                    {
                        RoomTemplateBuildUtility.BuildInArena(_selectedTemplate, "Map Designer");
                    }

                    if (DrawColoredButton("Auto Props", new Color(0.35f, 0.6f, 1f), GUILayout.Height(28f)))
                    {
                        AutoPopulateSelectedTemplate();
                    }

                    if (DrawColoredButton("Save Assets", new Color(1f, 0.6f, 0.22f), GUILayout.Height(28f)))
                    {
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Debug.Log("[UnifiedMapDesigner] Saved RoomTemplateSO assets.");
                        Repaint();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    _autoPropsSeed = EditorGUILayout.IntField("Seed", _autoPropsSeed);
                    if (GUILayout.Button("Randomize", GUILayout.Width(90f)))
                    {
                        _autoPropsSeed = UnityEngine.Random.Range(1, int.MaxValue);
                    }
                }
            }
        }

        private bool DrawColoredButton(string label, Color color, params GUILayoutOption[] options)
        {
            Color previous = GUI.backgroundColor;
            GUI.backgroundColor = color;
            bool clicked = GUILayout.Button(label, options);
            GUI.backgroundColor = previous;
            return clicked;
        }

        private void AutoPopulateSelectedTemplate()
        {
            if (_selectedTemplate == null)
            {
                return;
            }

            int before = _selectedTemplate.props != null ? _selectedTemplate.props.Count : 0;
            if (!EditorUtility.DisplayDialog(
                    "Auto Props",
                    $"{before} prop(s) will be deleted and regenerated for '{_selectedTemplate.name}'.",
                    "Regenerate",
                    "Cancel"))
            {
                return;
            }

            IReadOnlyList<PropDefinitionSO> pool = RoomTemplateAutoPropsUtility.LoadPropPool();
            if (pool == null || pool.Count == 0)
            {
                EditorUtility.DisplayDialog("Auto Props", "No prop pool found. Check Assets/Data/Props/PropRegistry.asset.", "OK");
                return;
            }

            RoomTemplateAutoPropsUtility.Result result =
                RoomTemplateAutoPropsUtility.Populate(_selectedTemplate, pool, _autoPropsSeed, true, "Auto Props RoomTemplate");
            bool rebuilt = RoomTemplateBuildUtility.TryRebuildIfActiveArenaTemplate(_selectedTemplate);
            Debug.Log($"[UnifiedMapDesigner] Auto Props '{_selectedTemplate.name}': {result.beforeCount}->{result.afterCount}, seed={result.seed}, rebuilt={rebuilt}.");
            Repaint();
        }

        private void DrawTemplatePreview(RoomTemplateSO template)
        {
            EditorGUILayout.Space(4f);
            EditorGUILayout.LabelField("2D Schematic Preview", EditorStyles.boldLabel);

            Rect area = GUILayoutUtility.GetRect(260f, 360f, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(area, new Color(0.08f, 0.08f, 0.08f));
            if (template == null || template.bounds.width <= 0 || template.bounds.height <= 0)
            {
                return;
            }

            const float padding = 10f;
            Rect inner = new Rect(area.x + padding, area.y + padding, area.width - padding * 2f, area.height - padding * 2f);
            float cell = Mathf.Floor(Mathf.Min(inner.width / template.bounds.width, inner.height / template.bounds.height));
            if (cell < 1f)
            {
                return;
            }

            float gridWidth = cell * template.bounds.width;
            float gridHeight = cell * template.bounds.height;
            Rect gridRect = new Rect(
                inner.x + (inner.width - gridWidth) * 0.5f,
                inner.y + (inner.height - gridHeight) * 0.5f,
                gridWidth,
                gridHeight);

            for (int y = template.bounds.yMin; y < template.bounds.yMax; y++)
            {
                for (int x = template.bounds.xMin; x < template.bounds.xMax; x++)
                {
                    Vector2Int tile = new Vector2Int(x, y);
                    Rect cellRect = TilePreviewRect(template.bounds, gridRect, cell, tile);
                    Color fill = template.IsWalkable(tile)
                        ? new Color(0.16f, 0.18f, 0.18f)
                        : Color.black;
                    EditorGUI.DrawRect(cellRect, fill);
                    EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, cellRect.width, 1f), new Color(0.25f, 0.25f, 0.25f));
                    EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, 1f, cellRect.height), new Color(0.25f, 0.25f, 0.25f));
                }
            }

            if (template.doorSockets != null)
            {
                for (int i = 0; i < template.doorSockets.Count; i++)
                {
                    DoorSocket door = template.doorSockets[i];
                    if (door == null) continue;
                    Rect doorRect = TilePreviewRect(template.bounds, gridRect, cell, door.position);
                    EditorGUI.DrawRect(InflateToMinimum(doorRect, 4f), Color.cyan);
                }
            }

            if (template.playerSpawn != null)
            {
                Rect spawnRect = TilePreviewRect(template.bounds, gridRect, cell, template.playerSpawn.position);
                Rect dot = new Rect(spawnRect.center.x - 3f, spawnRect.center.y - 3f, 6f, 6f);
                EditorGUI.DrawRect(dot, Color.green);
            }
        }

        private static Rect TilePreviewRect(RectInt bounds, Rect gridRect, float cell, Vector2Int tile)
        {
            int lx = tile.x - bounds.xMin;
            int ly = tile.y - bounds.yMin;
            return new Rect(
                gridRect.x + lx * cell,
                gridRect.y + (bounds.height - 1 - ly) * cell,
                cell,
                cell);
        }

        private static Rect InflateToMinimum(Rect rect, float minSize)
        {
            float width = Mathf.Max(rect.width, minSize);
            float height = Mathf.Max(rect.height, minSize);
            return new Rect(rect.center.x - width * 0.5f, rect.center.y - height * 0.5f, width, height);
        }

        private static DesignerCategory TabToCategory(Tab t)
        {
            switch (t)
            {
                case Tab.Floor: return DesignerCategory.Floor;
                case Tab.Cliff: return DesignerCategory.Cliff;
                case Tab.Object: return DesignerCategory.Object;
                case Tab.Portal: return DesignerCategory.Portal;
                case Tab.Light: return DesignerCategory.Light;
                default: return DesignerCategory.Floor;
            }
        }

        // ── Library tab (room select + edit) ────────────────────────────────
        private void DrawLibrary()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("New Room"))
                {
                    _controller.CreateNewRoom();
                    _core.SetActiveRoom(_controller.ActiveRoom);
                }
                using (new EditorGUI.DisabledScope(_controller.ActiveRoom == null))
                {
                    if (GUILayout.Button("Duplicate"))
                    {
                        _controller.DuplicateRoom(_controller.ActiveRoom);
                        _core.SetActiveRoom(_controller.ActiveRoom);
                    }
                    if (GUILayout.Button("Delete"))
                        _controller.DeleteRoom(_controller.ActiveRoom);
                }
            }

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Room Library", EditorStyles.boldLabel);

            _libScroll = EditorGUILayout.BeginScrollView(_libScroll);
            foreach (RoomLibraryEntry entry in _controller.LibraryEntries)
            {
                if (entry == null || entry.room == null) continue;
                bool isActive = _controller.ActiveRoom == entry.room;
                using (new EditorGUILayout.HorizontalScope(isActive ? EditorStyles.helpBox : GUIStyle.none))
                {
                    EditorGUILayout.LabelField(entry.DisplayName, GUILayout.Width(220));
                    EditorGUILayout.LabelField(entry.room.roomId, EditorStyles.miniLabel);
                    if (GUILayout.Button("Open", GUILayout.Width(60)))
                    {
                        _controller.OpenRoom(entry.room, entry.path);
                        _core.SetActiveRoom(entry.room);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        // ── Category tabs (palette + per-category actions) ──────────────────
        private void DrawCategory(DesignerCategory category)
        {
            if (_controller.ActiveRoom == null)
            {
                EditorGUILayout.HelpBox("Open a room in the Library tab to start placing.", MessageType.Info);
                return;
            }
            if (_core.ActiveRoom != _controller.ActiveRoom)
                _core.SetActiveRoom(_controller.ActiveRoom);

            using (new EditorGUILayout.HorizontalScope())
            {
                _core.EraseMode = GUILayout.Toggle(_core.EraseMode, _core.EraseMode ? "Erase" : "Paint", "Button", GUILayout.Width(80));
                EditorGUILayout.LabelField("Rotation", GUILayout.Width(60));
                _core.Rotation = EditorGUILayout.Slider(_core.Rotation, 0f, 270f);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Brush:", GUILayout.Width(45));
                foreach (int sz in new[] { 1, 3, 5, 10 })
                {
                    bool on = _core.BrushSize == sz;
                    if (GUILayout.Toggle(on, sz.ToString(), EditorStyles.miniButton) && !on)
                        _core.BrushSize = sz;
                }
            }

            if (category == DesignerCategory.Cliff) DrawCliffControls();

            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField($"{DesignerCategoryMap.Label(category)} Palette", EditorStyles.boldLabel);
            _search = EditorGUILayout.TextField("Search", _search);

            DrawPalette(category);
        }

        private void DrawCliffControls()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Logical Cliff Generation", EditorStyles.boldLabel);
                _core.SouthClearCells = EditorGUILayout.IntSlider("Drop Depth", _core.SouthClearCells, 1, 10);
                EditorGUILayout.LabelField($"Preview: {_core.PreviewCliffCount()} cliff cells from current floor", EditorStyles.miniLabel);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Generate Cliffs (from floor)", GUILayout.Height(26)))
                    {
                        Grid grid = ActiveGrid();
                        System.Func<Vector3Int, Vector3> toWorld = grid != null ? (c => grid.GetCellCenterWorld(c)) : (System.Func<Vector3Int, Vector3>)null;
                        int n = _core.GenerateCliffsFromFloor(toWorld, _core.SelectedAssetId);
                        Debug.Log($"[UnifiedMapDesigner] Generated {n} cliff cells from floor shape.");
                    }
                    // Scene-tilemap cliff path (repairs/uses CliffAutoPlacer)
                    CliffGenerateAction.DrawButton(26f);
                }
            }
        }

        private void DrawPalette(DesignerCategory category)
        {
            var registry = RuntimeAssetRegistry.Instance;
            if (registry == null)
            {
                EditorGUILayout.HelpBox("RuntimeAssetRegistry not found. Bake it via RIMA/Live Tool/Bake Asset Registry.", MessageType.Warning);
                return;
            }

            string tag = DesignerCategoryMap.RegistryTag(category);
            IReadOnlyList<RegistryEntry> entries = registry.GetByTag(tag);
            List<PaletteGroup> groups = BuildPaletteGroups(category, entries);
            SelectDefaultPaletteGroup(category, groups);

            _paletteScroll = EditorGUILayout.BeginScrollView(_paletteScroll);
            int perRow = Mathf.Max(1, (int)(position.width / 96f));
            int i = 0;
            EditorGUILayout.BeginHorizontal();
            foreach (PaletteGroup group in groups)
            {
                if (group == null) continue;
                if (!string.IsNullOrEmpty(_search) &&
                    group.label.IndexOf(_search, System.StringComparison.OrdinalIgnoreCase) < 0 &&
                    group.key.IndexOf(_search, System.StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                if (i > 0 && i % perRow == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }

                bool selected = group.Contains(_core.SelectedAssetId);
                Color prev = GUI.backgroundColor;
                if (selected) GUI.backgroundColor = new Color(0.4f, 0.9f, 1f);
                Texture preview = group.preview != null && group.preview.sprite != null ? group.preview.sprite.texture : null;
                var content = new GUIContent(preview, group.label + " (" + group.variantCount + ")");
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(88)))
                {
                    if (GUILayout.Button(content, GUILayout.Width(80), GUILayout.Height(64)))
                        _core.SelectedAssetId = group.groupId;
                    EditorGUILayout.LabelField(group.label, EditorStyles.miniLabel, GUILayout.Width(88));
                    EditorGUILayout.LabelField(group.variantCount + " variants", EditorStyles.miniLabel, GUILayout.Width(88));
                }
                GUI.backgroundColor = prev;
                i++;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();

            if (groups.Count == 0)
                EditorGUILayout.HelpBox($"No '{tag}' assets in the registry yet.", MessageType.Info);
        }

        private sealed class PaletteGroup
        {
            public string key;
            public string groupId;
            public string label;
            public RegistryEntry preview;
            public int variantCount;
            public readonly HashSet<string> guids = new HashSet<string>();
            public readonly HashSet<string> displayNames = new HashSet<string>();

            public bool Contains(string assetId)
            {
                return !string.IsNullOrEmpty(assetId) &&
                       (assetId == groupId || guids.Contains(assetId));
            }
        }

        private static List<PaletteGroup> BuildPaletteGroups(DesignerCategory category, IReadOnlyList<RegistryEntry> entries)
        {
            Dictionary<string, PaletteGroup> byKey = new Dictionary<string, PaletteGroup>();
            if (entries == null)
            {
                return new List<PaletteGroup>();
            }

            for (int i = 0; i < entries.Count; i++)
            {
                RegistryEntry entry = entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.guid))
                {
                    continue;
                }

                string key = UnifiedPaintVariantResolver.GroupKeyFor(entry, category);
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                if (!byKey.TryGetValue(key, out PaletteGroup group))
                {
                    group = new PaletteGroup
                    {
                        key = key,
                        groupId = UnifiedPaintVariantResolver.BuildGroupId(category, key),
                        label = LabelForGroupKey(key)
                    };
                    byKey[key] = group;
                }

                group.guids.Add(entry.guid);
                if (!string.IsNullOrEmpty(entry.displayName))
                {
                    group.displayNames.Add(entry.displayName);
                }

                if (group.preview == null ||
                    (group.preview.sprite == null && entry.sprite != null))
                {
                    group.preview = entry;
                }
            }

            List<PaletteGroup> groups = new List<PaletteGroup>(byKey.Values);
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].variantCount = Mathf.Max(1, groups[i].displayNames.Count);
            }

            groups.Sort((a, b) => string.Compare(a.label, b.label, System.StringComparison.OrdinalIgnoreCase));
            return groups;
        }

        private void SelectDefaultPaletteGroup(DesignerCategory category, List<PaletteGroup> groups)
        {
            if (groups == null || groups.Count == 0)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_core.SelectedAssetId))
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    if (groups[i].Contains(_core.SelectedAssetId))
                    {
                        return;
                    }
                }
            }

            PaletteGroup selected = groups[0];
            if (category == DesignerCategory.Floor)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    if (groups[i].key == "floor451" || groups[i].key == "iso_floor")
                    {
                        selected = groups[i];
                        break;
                    }
                }
            }

            _core.SelectedAssetId = selected.groupId;
        }

        private static string LabelForGroupKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "Material";
            }

            if (key == "iso_floor")
            {
                return "Iso Granite";
            }

            if (key == "floor451")
            {
                return "Iso Granite 451";
            }

            if (key == "flat_tile" || key == "flat")
            {
                return "Flat Topdown";
            }

            string[] parts = key.Split('_', '-');
            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i]))
                {
                    continue;
                }

                parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1);
            }

            return string.Join(" ", parts);
        }

        // ── Layers tab (shiftable depth stack) ──────────────────────────────
        private void DrawLayers()
        {
            EditorGUILayout.HelpBox(
                "Depth stack. L1 Floor sits above L2 Cliff; preview islands and the far backdrop sit below. " +
                "Default sorting slots come from RoomDepthStack.", MessageType.Info);

            foreach (RoomLayer layer in System.Enum.GetValues(typeof(RoomLayer)))
            {
                RoomDepthStack.DepthSlot slot = RoomDepthStack.SlotFor(layer);
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField(layer.ToString(), GUILayout.Width(90));
                    EditorGUILayout.LabelField("Layer: " + slot.sortingLayer, GUILayout.Width(170));
                    EditorGUILayout.LabelField("Order: " + slot.sortingOrder, GUILayout.Width(90));
                    EditorGUILayout.LabelField(slot.ySort ? "Y-sort" : "", GUILayout.Width(60));
                }
            }
        }

        private void DrawStatusBar()
        {
            GUILayout.FlexibleSpace();
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                string brush = string.IsNullOrEmpty(_core.SelectedAssetId) ? "<none>" : _core.SelectedAssetId;
                EditorGUILayout.LabelField(
                    $"Category: {DesignerCategoryMap.Label(_core.Category)}   Brush: {brush} size:{_core.BrushSize}   " +
                    $"{(_painting ? "PAINTING (LMB place / RMB erase / Alt=erase)" : "paint off")}",
                    EditorStyles.miniLabel);
            }
        }

        // ── SceneView painting ──────────────────────────────────────────────
        private void OnSceneGui(SceneView view)
        {
            if (!_painting || _core.ActiveRoom == null) return;
            if (_tab == Tab.Library || _tab == Tab.Layers) return;

            Grid grid = ActiveGrid();
            if (grid == null) return;

            Event e = Event.current;
            Vector3 mouseWorld = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            mouseWorld.z = 0f;
            Vector3Int cell = grid.WorldToCell(mouseWorld);
            Vector3 center = grid.GetCellCenterWorld(cell);

            Handles.color = (_core.EraseMode) ? new Color(1f, 0.35f, 0.3f, 0.8f) : new Color(0.4f, 0.9f, 1f, 0.8f);
            Handles.DrawWireCube(center, grid.cellSize * 0.9f);
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (e.type == EventType.MouseDown || (e.type == EventType.MouseDrag && e.button == 0))
            {
                if (e.button == 0)
                {
                    if (e.alt || _core.EraseMode) _core.Erase(cell);
                    else _core.Paint(cell, center);
                    e.Use();
                }
                else if (e.button == 1)
                {
                    _core.Erase(cell);
                    e.Use();
                }
            }
            view.Repaint();
        }

        private Grid ActiveGrid()
        {
            if (_composer.PreviewGrid == null && _controller.ActiveRoom != null)
                _composer.Compose(_controller.ActiveRoom);
            return _composer.PreviewGrid;
        }

        // ── actions ─────────────────────────────────────────────────────────
        private void SaveActive()
        {
            _controller.SaveActiveRoom(_composer);
            Debug.Log("[UnifiedMapDesigner] Saved room + JSON sidecars.");
        }

        private void BuildPreview()
        {
            if (_controller.ActiveRoom == null) return;
            _composer.Compose(_controller.ActiveRoom);
            EditorSceneManager_MarkActiveDirty();
        }

        private static void EditorSceneManager_MarkActiveDirty()
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
#endif
