#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.MapDesigner
{
    public sealed class MinimalTilePainter : EditorWindow
    {
        private enum Tool { Paint, Erase }
        private enum Mode { GroupTheme, SingleTile }

        private delegate void ContentDelegate();

        private const string TileFolder = "Assets/ScriptableObjects/Floor/IsoTiles35";
        private const float SidePanelMinWidth = 160f;
        private const float MainPanelMinWidth = 240f;
        private const float ResizeHandleWidth = 8f;
        private const float SideHeaderHeight = 46f;
        private const float SideFooterHeight = 34f;
        private const float ScrollbarWidth = 16f;
        private const float SectionHeaderHeight = 22f;
        private const float TileGap = 6f;
        private const float MinTileCell = 76f;
        private const float BadgeWidth = 42f;
        private const float AccentStripeWidth = 4f;
        private const float CompactThresholdWidth = 520f;
        private const float CollapseSideBreakpoint = 600f;
        private const float HideSideBreakpoint = 400f;

        private const float ToolbarHeight = 34f;
        private const float StatusHeight = 22f;
        private const float CollapsedStripWidth = 40f;
        private const float ActiveCardHeight = 86f;
        private const float PanelPad = 6f;

        private static readonly Color PanelBg = new Color(0.11f, 0.12f, 0.14f, 1f);
        private static readonly Color BodyBg = new Color(0.16f, 0.18f, 0.20f, 1f);
        private static readonly Color SectionBg = new Color(0.12f, 0.14f, 0.16f, 1f);
        private static readonly Color MutedBg = new Color(0f, 0f, 0f, 0.22f);
        private static readonly Color HandleColor = new Color(0.28f, 0.30f, 0.34f, 1f);
        private static readonly Color HandleHoverColor = new Color(0.10f, 0.88f, 1.00f, 1f);
        private static readonly Color BorderDim = new Color(0.28f, 0.30f, 0.34f, 1f);

        // Theme groups are data, not enum - easy to extend when more tiles arrive.
        // Each: (label, range start, count, accent color).
        private struct ThemeGroup
        {
            public string Label;
            public int Start;
            public int Count;
            public Color Accent;
            public ThemeGroup(string l, int s, int c, Color a) { Label = l; Start = s; Count = c; Accent = a; }
        }

        // Default groupings for the existing 16 iso-35 tiles.
        // Verified against TILE_CATEGORIZATION.md (2026-05-25 Opus visual inspection).
        private static readonly ThemeGroup[] DefaultGroups =
        {
            new ThemeGroup("Cobblestone (stone)", 0, 4, new Color(0.55f, 0.55f, 0.55f)),
            new ThemeGroup("Cyan Veins (accent)", 4, 3, new Color(0.10f, 0.86f, 1.00f)),
            new ThemeGroup("Dirt (variation)",    7, 4, new Color(0.75f, 0.50f, 0.30f)),
            new ThemeGroup("Ritual Rune (focal)", 11, 5, new Color(1.00f, 0.40f, 0.85f)),
        };

        [SerializeField] private Tilemap activeTilemap;
        [SerializeField] private Mode mode = Mode.GroupTheme;
        [SerializeField] private int themeIndex;
        [SerializeField] private Tool tool;
        [SerializeField] private int singleTileIndex;
        [SerializeField] private bool randomVariant = true;
        [SerializeField] private int brushSize = 1;
        [SerializeField] private float thumbnailSize = 48f;
        [SerializeField] private float sidePanelWidth = 220f;
        [SerializeField] private bool showCustomGroups;
        [SerializeField] private Vector2 sideScroll;
        [SerializeField] private Vector2 mainScroll;
        [SerializeField] private bool[] groupCollapsed;
        [SerializeField] private string searchFilter = string.Empty;
        [SerializeField] private bool collapsedLibraryExpanded;
        [SerializeField] private bool drawerOpen;
        [SerializeField] private bool isResizingPanel;
        [SerializeField] private List<int> overrideTileIndices = new();
        [SerializeField] private List<int> overrideGroupIndices = new();

        private readonly List<TileBase> tiles = new();
        private readonly Dictionary<int, int> tileGroupOverrides = new();
        private ThemeGroup[] groups;
        private Vector3Int hoverCell;
        private bool hasHover;
        private bool isPainting;
        private string status = "SceneView click to paint";
        private GUIStyle sectionHeaderStyle;
        private GUIStyle subtleLabelStyle;
        private GUIStyle badgeStyle;
        private GUIStyle activeThemeStyle;
        private GUIStyle compactButtonStyle;
        private GUIStyle libraryHeaderStyle;
        private GUIStyle searchFieldStyle;

        // [MenuItem removed — replaced by RIMA/Room Painter]
        public static void Open()
        {
            MinimalTilePainter window = FindStandaloneWindow();
            if (window == null)
            {
                window = CreateInstance<MinimalTilePainter>();
                window.titleContent = new GUIContent("Tile Painter (Minimal)");
            }
            window.minSize = new Vector2(320f, 420f);
            window.Show();
            window.Focus();
        }

        private static MinimalTilePainter FindStandaloneWindow()
        {
            foreach (MinimalTilePainter window in Resources.FindObjectsOfTypeAll<MinimalTilePainter>())
            {
                if (window != null && (window.hideFlags & HideFlags.HideInHierarchy) == 0)
                    return window;
            }
            return null;
        }

        private void OnEnable()
        {
            groups = DefaultGroups;
            EnsureGroupState();
            LoadOverrideDictionary();
            LoadTileAssets();
            if (activeTilemap == null) activeTilemap = FindFloorTilemap();
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SaveOverrideLists();
            SceneView.duringSceneGui -= OnSceneGUI;
            isPainting = false;
        }

        private void EnsureStyles()
        {
            if (sectionHeaderStyle == null)
            {
                sectionHeaderStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 10,
                    padding = new RectOffset(8, 8, 0, 0),
                    clipping = TextClipping.Clip,
                };
            }
            if (subtleLabelStyle == null)
            {
                subtleLabelStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    fontSize = 9,
                    normal = { textColor = new Color(0.70f, 0.75f, 0.78f, 1f) },
                    wordWrap = true,
                };
            }
            if (badgeStyle == null)
            {
                badgeStyle = new GUIStyle(EditorStyles.miniBoldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 9,
                    normal = { textColor = new Color(0.90f, 0.96f, 1f, 1f) },
                    clipping = TextClipping.Clip,
                };
            }
            if (activeThemeStyle == null)
            {
                activeThemeStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 11,
                    wordWrap = true,
                    clipping = TextClipping.Clip,
                };
            }
            if (compactButtonStyle == null)
            {
                compactButtonStyle = new GUIStyle(EditorStyles.miniButton)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 10,
                    clipping = TextClipping.Clip,
                };
            }
            if (libraryHeaderStyle == null)
            {
                libraryHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 11,
                    clipping = TextClipping.Clip,
                };
            }
            if (searchFieldStyle == null)
            {
                searchFieldStyle = GUI.skin.FindStyle("ToolbarSearchTextField") ?? EditorStyles.textField;
            }
        }

        private void OnGUI()
        {
            EnsureStyles();
            EnsureGroupState();

            float visibleWidth = Mathf.Max(320f, EditorGUIUtility.currentViewWidth - 24f);
            float visibleHeight = Mathf.Max(420f, position.height);
            bool compact = visibleWidth < CompactThresholdWidth;
            bool hideSide = visibleWidth < HideSideBreakpoint;
            bool collapseSide = visibleWidth < CollapseSideBreakpoint && !hideSide;

            float maxSide = Mathf.Max(SidePanelMinWidth, visibleWidth - MainPanelMinWidth - ResizeHandleWidth);
            sidePanelWidth = Mathf.Clamp(sidePanelWidth, SidePanelMinWidth, maxSide);
            thumbnailSize = Mathf.Clamp(thumbnailSize, 32f, 64f);

            Rect root = GUILayoutUtility.GetRect(visibleWidth, visibleHeight,
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            root.width = Mathf.Max(root.width, visibleWidth);

            EditorGUI.DrawRect(root, BodyBg);
            DrawToolbar(new Rect(root.x, root.y, root.width, ToolbarHeight), compact, hideSide);

            Rect statusRect = new Rect(root.x, root.yMax - StatusHeight, root.width, StatusHeight);
            Rect bodyRect = new Rect(root.x, root.y + ToolbarHeight, root.width,
                Mathf.Max(1f, root.height - ToolbarHeight - StatusHeight));

            DrawBody(bodyRect, visibleWidth, compact, collapseSide, hideSide);
            DrawStatusBar(statusRect, visibleWidth);
        }

        private void DrawToolbar(Rect rect, bool compact, bool hideSide)
        {
            EditorGUI.DrawRect(rect, new Color(0.09f, 0.10f, 0.12f, 1f));

            float x = rect.x + 6f;
            if (hideSide)
            {
                Rect libRect = new Rect(x, rect.y + 5f, 48f, 24f);
                if (GUI.Button(libRect, new GUIContent("Lib", "Open tile library drawer"), compactButtonStyle))
                {
                    drawerOpen = !drawerOpen;
                    Repaint();
                }
                x = libRect.xMax + 6f;
            }

            float saveWidth = compact ? 48f : 84f;
            float clearWidth = compact ? 52f : 74f;
            Rect clearRect = new Rect(rect.xMax - clearWidth - 6f, rect.y + 5f, clearWidth, 24f);
            Rect saveRect = new Rect(clearRect.x - saveWidth - 6f, rect.y + 5f, saveWidth, 24f);
            float labelWidth = compact ? 48f : 72f;
            Rect labelRect = new Rect(x, rect.y + 8f, labelWidth, 18f);
            Rect fieldRect = new Rect(labelRect.xMax + 4f, rect.y + 5f,
                Mathf.Max(64f, saveRect.x - labelRect.xMax - 10f), 22f);

            GUI.Label(labelRect, compact ? "Tilemap" : "Tilemap Field", subtleLabelStyle);
            activeTilemap = (Tilemap)EditorGUI.ObjectField(fieldRect, activeTilemap, typeof(Tilemap), true);

            if (GUI.Button(saveRect, new GUIContent(compact ? "Save" : "Save Scene", "Save open scenes"), compactButtonStyle))
            {
                EditorSceneManager.SaveOpenScenes();
                status = "Scene saved";
            }

            if (GUI.Button(clearRect, new GUIContent(compact ? "Clear" : "Clear", "Clear selected tilemap"), compactButtonStyle))
            {
                ClearSelectedTilemap();
            }
        }

        private void DrawBody(Rect rect, float visibleWidth, bool compact, bool collapseSide, bool hideSide)
        {
            if (hideSide)
            {
                DrawMainPanel(rect, compact);
                if (drawerOpen)
                {
                    Rect drawer = new Rect(rect.x + 8f, rect.y + 8f,
                        Mathf.Min(260f, rect.width - 16f), rect.height - 16f);
                    DrawPanelShadow(drawer);
                    DrawSidePanel(drawer);
                }
                return;
            }

            if (collapseSide && !collapsedLibraryExpanded)
            {
                Rect strip = new Rect(rect.x, rect.y, CollapsedStripWidth, rect.height);
                DrawCollapsedSideStrip(strip);
                DrawMainPanel(new Rect(strip.xMax, rect.y, rect.width - CollapsedStripWidth, rect.height), compact);
                return;
            }

            float panelWidth = collapseSide
                ? Mathf.Min(sidePanelWidth, Mathf.Max(SidePanelMinWidth, visibleWidth - MainPanelMinWidth - ResizeHandleWidth))
                : sidePanelWidth;
            Rect sideRect = new Rect(rect.x, rect.y, panelWidth, rect.height);
            Rect handleRect = new Rect(sideRect.xMax, rect.y, ResizeHandleWidth, rect.height);
            Rect mainRect = new Rect(handleRect.xMax, rect.y,
                Mathf.Max(MainPanelMinWidth, rect.xMax - handleRect.xMax), rect.height);

            DrawSidePanel(sideRect);
            DrawResizeHandle(handleRect, visibleWidth);
            DrawMainPanel(mainRect, compact);
        }

        private void DrawPanelShadow(Rect rect)
        {
            EditorGUI.DrawRect(new Rect(rect.x - 2f, rect.y - 2f, rect.width + 4f, rect.height + 4f),
                new Color(0f, 0f, 0f, 0.45f));
        }

        // -------- Side panel (tile library, sticky header/footer) --------
        private void DrawSidePanel(Rect panel)
        {
            EditorGUI.DrawRect(panel, PanelBg);
            DrawBorder(panel, BorderDim, 1f);

            Rect header = new Rect(panel.x, panel.y, panel.width, SideHeaderHeight);
            Rect footer = new Rect(panel.x + PanelPad, panel.yMax - SideFooterHeight + 5f,
                panel.width - PanelPad * 2f, 24f);
            Rect scroll = new Rect(panel.x, header.yMax, panel.width,
                Mathf.Max(1f, panel.height - SideHeaderHeight - SideFooterHeight));

            DrawLibraryHeader(header);
            DrawLibraryScroll(scroll, panel.width);

            EditorGUI.DrawRect(new Rect(panel.x, panel.yMax - SideFooterHeight, panel.width, SideFooterHeight),
                new Color(0.09f, 0.10f, 0.12f, 1f));
            if (GUI.Button(footer, new GUIContent("Refresh", $"Re-scan {TileFolder} for tile_*.asset files"), compactButtonStyle))
            {
                LoadTileAssets();
                Repaint();
            }
        }

        private void DrawLibraryHeader(Rect rect)
        {
            EditorGUI.DrawRect(rect, new Color(0.10f, 0.11f, 0.13f, 1f));

            Rect titleRect = new Rect(rect.x + PanelPad, rect.y + 4f, 78f, 18f);
            Rect searchRect = new Rect(titleRect.xMax + 4f, rect.y + 3f,
                Mathf.Max(42f, rect.xMax - titleRect.xMax - PanelPad - 4f), 20f);
            Rect labelRect = new Rect(rect.x + PanelPad, rect.y + 27f, 34f, 16f);
            Rect sliderRect = new Rect(labelRect.xMax + 4f, rect.y + 29f,
                Mathf.Max(32f, rect.width - 86f), 14f);
            Rect valueRect = new Rect(rect.xMax - 42f, rect.y + 27f, 38f, 16f);

            GUI.Label(titleRect, $"Library ({tiles.Count})", libraryHeaderStyle);
            searchFilter = EditorGUI.TextField(searchRect, searchFilter ?? string.Empty, searchFieldStyle);
            GUI.Label(labelRect, "Size", subtleLabelStyle);
            thumbnailSize = GUI.HorizontalSlider(sliderRect, thumbnailSize, 32f, 64f);
            GUI.Label(valueRect, $"{Mathf.RoundToInt(thumbnailSize)}px", subtleLabelStyle);
        }

        private void DrawLibraryScroll(Rect scroll, float panelWidth)
        {
            List<int> matches = SearchMatches();
            bool searching = !string.IsNullOrWhiteSpace(searchFilter);
            float contentHeight = CalculateLibraryContentHeight(panelWidth, searching, matches);
            Rect content = new Rect(0f, 0f, Mathf.Max(1f, panelWidth - ScrollbarWidth), contentHeight);

            sideScroll = GUI.BeginScrollView(scroll, sideScroll, content);
            if (groups == null || groups.Length == 0)
            {
                GUI.Label(new Rect(PanelPad, PanelPad, content.width - PanelPad * 2f, 30f),
                    "No theme groups configured.", subtleLabelStyle);
            }
            else if (searching)
            {
                DrawSearchResults(content, matches);
            }
            else
            {
                DrawGroupedLibrary(content);
            }
            GUI.EndScrollView();
        }

        private float CalculateLibraryContentHeight(float panelWidth, bool searching, List<int> matches)
        {
            float usable = Mathf.Max(1f, panelWidth - ScrollbarWidth - PanelPad * 2f);
            int columns = LibraryColumnCount(usable);
            float card = LibraryCardSize(usable, columns);
            float height = PanelPad;

            if (searching)
            {
                int rows = Mathf.CeilToInt(matches.Count / (float)Mathf.Max(1, columns));
                return height + Mathf.Max(1, rows) * (card + TileGap) + PanelPad;
            }

            for (int i = 0; i < groups.Length; i++)
            {
                height += SectionHeaderHeight + TileGap;
                if (groupCollapsed != null && i < groupCollapsed.Length && groupCollapsed[i]) continue;
                int rows = Mathf.CeilToInt(EffectiveTileCountForGroup(i) / (float)Mathf.Max(1, columns));
                height += rows * (card + TileGap) + TileGap;
            }
            return Mathf.Max(height, 60f);
        }

        private void DrawGroupedLibrary(Rect content)
        {
            float usable = Mathf.Max(1f, content.width - PanelPad * 2f);
            int columns = LibraryColumnCount(usable);
            float card = LibraryCardSize(usable, columns);
            float y = PanelPad;

            for (int i = 0; i < groups.Length; i++)
            {
                ThemeGroup g = groups[i];
                Rect header = new Rect(PanelPad, y, usable, SectionHeaderHeight);
                DrawGroupHeader(header, i, g);
                y += SectionHeaderHeight + TileGap;

                if (groupCollapsed[i]) continue;

                List<int> groupTiles = EffectiveTileIndicesForGroup(i);
                for (int j = 0; j < groupTiles.Count; j++)
                {
                    int tileIdx = groupTiles[j];
                    int col = j % columns;
                    int row = j / columns;
                    Rect cell = new Rect(PanelPad + col * (card + TileGap),
                        y + row * (card + TileGap), card, card);
                    DrawTileCard(cell, tileIdx, g.Accent);
                }

                int rows = Mathf.CeilToInt(groupTiles.Count / (float)Mathf.Max(1, columns));
                y += rows * (card + TileGap) + TileGap;
            }
        }

        private void DrawSearchResults(Rect content, List<int> matches)
        {
            float usable = Mathf.Max(1f, content.width - PanelPad * 2f);
            int columns = LibraryColumnCount(usable);
            float card = LibraryCardSize(usable, columns);

            if (matches.Count == 0)
            {
                GUI.Label(new Rect(PanelPad, PanelPad, usable, 24f), "No matching tiles", subtleLabelStyle);
                return;
            }

            for (int i = 0; i < matches.Count; i++)
            {
                int tileIdx = matches[i];
                int col = i % columns;
                int row = i / columns;
                Rect cell = new Rect(PanelPad + col * (card + TileGap),
                    PanelPad + row * (card + TileGap), card, card);
                DrawTileCard(cell, tileIdx, AccentForTile(tileIdx));
            }
        }

        private int LibraryColumnCount(float usableWidth)
        {
            int columns = Mathf.FloorToInt((usableWidth + TileGap) / MinTileCell);
            return Mathf.Clamp(columns, 2, 4);
        }

        private float LibraryCardSize(float usableWidth, int columns)
        {
            float maxByPanel = (usableWidth - TileGap * (columns - 1)) / Mathf.Max(1, columns);
            return Mathf.Clamp(Mathf.Floor(maxByPanel), 40f, thumbnailSize);
        }

        private void DrawGroupHeader(Rect rect, int groupIdx, ThemeGroup group)
        {
            int loaded = CountLoadedInEffectiveGroup(groupIdx);
            int total = EffectiveTileCountForGroup(groupIdx);
            bool isActive = mode == Mode.GroupTheme && themeIndex == groupIdx;
            Color bg = isActive ? new Color(0.20f, 0.30f, 0.36f, 0.65f) : new Color(0.18f, 0.19f, 0.22f, 0.7f);

            EditorGUI.DrawRect(rect, bg);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, AccentStripeWidth, rect.height), group.Accent);

            Rect fold = new Rect(rect.x + AccentStripeWidth + 2f, rect.y + 2f, 20f, rect.height - 4f);
            Rect badge = new Rect(rect.xMax - BadgeWidth - 5f, rect.y + 3f, BadgeWidth, rect.height - 6f);
            Rect label = new Rect(fold.xMax + 2f, rect.y, Mathf.Max(20f, badge.x - fold.xMax - 6f), rect.height);

            string arrow = groupCollapsed[groupIdx] ? ">" : "v";
            if (GUI.Button(fold, new GUIContent(arrow, "Collapse or expand group"), GUIStyle.none))
            {
                groupCollapsed[groupIdx] = !groupCollapsed[groupIdx];
                Repaint();
            }

            if (GUI.Button(label, new GUIContent(group.Label, group.Label), EditorStyles.label))
            {
                mode = Mode.GroupTheme;
                themeIndex = groupIdx;
                status = $"Theme: {group.Label} ({loaded}/{total})";
                Repaint();
            }

            EditorGUI.DrawRect(badge, new Color(0f, 0f, 0f, 0.35f));
            GUI.Label(badge, $"{loaded}/{total}", badgeStyle);
        }

        private void DrawTileCard(Rect rect, int tileIndex, Color accent)
        {
            bool selectedSingle = mode == Mode.SingleTile && singleTileIndex == tileIndex;
            Color fill = selectedSingle ? new Color(accent.r, accent.g, accent.b, 0.22f) : MutedBg;
            bool overridden = IsTileGroupOverridden(tileIndex);
            EditorGUI.DrawRect(rect, fill);

            Rect imgRect = new Rect(rect.x + 3f, rect.y + 3f, rect.width - 6f, rect.height - 6f);
            DrawTilePreview(imgRect, tileIndex);

            Rect badgeRect = new Rect(rect.xMax - BadgeWidth + 4f, rect.yMax - 18f, BadgeWidth - 6f, 15f);
            Color badgeBg = overridden ? new Color(accent.r, accent.g, accent.b, 0.45f) : new Color(0f, 0f, 0f, 0.58f);
            EditorGUI.DrawRect(badgeRect, badgeBg);
            GUI.Label(badgeRect, $"t{tileIndex}", badgeStyle);

            if (overridden)
            {
                Rect dot = new Rect(rect.x + 6f, rect.y + 6f, 7f, 7f);
                EditorGUI.DrawRect(dot, accent);
                DrawBorder(dot, new Color(0f, 0f, 0f, 0.55f), 1f);
            }

            DrawBorder(rect, selectedSingle ? accent : new Color(accent.r, accent.g, accent.b, 0.85f),
                selectedSingle ? 2.5f : 1f);

            string custom = overridden ? "\nCustom group" : string.Empty;
            string tooltip = $"{GetTileDisplayName(tileIndex)}\ntile_{tileIndex}\n{EffectiveGroupLabel(tileIndex)}{custom}";
            GUI.Label(rect, new GUIContent(string.Empty, tooltip));

            Event e = Event.current;
            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                if (e.button == 1)
                {
                    ShowTileGroupContextMenu(tileIndex);
                }
                else if (e.button == 0)
                {
                    mode = Mode.SingleTile;
                    singleTileIndex = tileIndex;
                    status = $"Single: tile_{tileIndex}";
                }
                e.Use();
                Repaint();
            }
        }

        // -------- Collapse and resize --------
        private void DrawCollapsedSideStrip(Rect rect)
        {
            EditorGUI.DrawRect(rect, PanelBg);
            DrawBorder(rect, BorderDim, 1f);

            Rect toggle = new Rect(rect.x + 5f, rect.y + 6f, rect.width - 10f, 24f);
            if (GUI.Button(toggle, new GUIContent(">", "Expand tile library"), compactButtonStyle))
            {
                collapsedLibraryExpanded = true;
                Repaint();
            }

            if (groups == null) return;

            float y = toggle.yMax + 12f;
            for (int i = 0; i < Mathf.Min(4, groups.Length); i++)
            {
                Rect button = new Rect(rect.x + 8f, y, rect.width - 16f, rect.width - 16f);
                EditorGUI.DrawRect(button, groups[i].Accent);
                DrawBorder(button, i == themeIndex && mode == Mode.GroupTheme ? Color.white : BorderDim, 1.5f);
                GUI.Label(button, new GUIContent(string.Empty, groups[i].Label));

                Event e = Event.current;
                if (e.type == EventType.MouseDown && e.button == 0 && button.Contains(e.mousePosition))
                {
                    mode = Mode.GroupTheme;
                    themeIndex = i;
                    status = $"Theme: {groups[i].Label}";
                    e.Use();
                    Repaint();
                }
                y += button.height + 8f;
            }
        }

        private void DrawResizeHandle(Rect handle, float visibleWidth)
        {
            Event e = Event.current;
            bool hover = handle.Contains(e.mousePosition);
            Color color = hover || isResizingPanel ? HandleHoverColor : HandleColor;

            EditorGUI.DrawRect(handle, new Color(0.08f, 0.09f, 0.10f, 1f));
            Rect line = new Rect(handle.center.x - 0.5f, handle.y, 1f, handle.height);
            EditorGUI.DrawRect(line, color);

            Vector2 c = handle.center;
            for (int i = -1; i <= 1; i++)
            {
                EditorGUI.DrawRect(new Rect(c.x - 1f, c.y + i * 7f - 1f, 2f, 2f), color);
            }

            EditorGUIUtility.AddCursorRect(handle, MouseCursor.ResizeHorizontal);

            if (e.type == EventType.MouseDown && e.button == 0 && handle.Contains(e.mousePosition))
            {
                isResizingPanel = true;
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && isResizingPanel)
            {
                float maxSide = Mathf.Max(SidePanelMinWidth, visibleWidth - MainPanelMinWidth - ResizeHandleWidth);
                sidePanelWidth = Mathf.Clamp(sidePanelWidth + e.delta.x, SidePanelMinWidth, maxSide);
                e.Use();
                Repaint();
            }
            else if (e.type == EventType.MouseUp && isResizingPanel)
            {
                isResizingPanel = false;
                e.Use();
            }
        }

        // -------- Main panel --------
        private void DrawMainPanel(Rect rect, bool compact)
        {
            EditorGUI.DrawRect(rect, BodyBg);

            GUILayout.BeginArea(new Rect(rect.x + 8f, rect.y + 8f,
                Mathf.Max(1f, rect.width - 16f), Mathf.Max(1f, rect.height - 16f)));
            mainScroll = EditorGUILayout.BeginScrollView(mainScroll, false, true);

            CliffGenerateAction.DrawButton(32f);
            GUILayout.Space(6f);
            DrawActiveSelectionCard();
            GUILayout.Space(8f);

            DrawSection("TOOL", () =>
            {
                GUIContent[] labels =
                {
                    new GUIContent("Paint", "Paint tiles in the Scene View"),
                    new GUIContent("Erase", "Erase tiles in the Scene View"),
                };
                tool = (Tool)GUILayout.Toolbar((int)tool, labels, compactButtonStyle,
                    GUILayout.Height(24f), GUILayout.MinWidth(160f));
            });

            DrawSection("PAINT MODE", () =>
            {
                GUIContent[] labels = compact
                    ? new[] { new GUIContent("Group", "Paint variants from one theme"), new GUIContent("Single", "Paint one selected tile") }
                    : new[] { new GUIContent("Group Theme", "Paint variants from one theme"), new GUIContent("Single Tile", "Paint one selected tile") };
                mode = (Mode)GUILayout.Toolbar((int)mode, labels, compactButtonStyle,
                    GUILayout.Height(24f), GUILayout.MinWidth(compact ? 160f : 220f));
            });

            DrawSection("BRUSH SIZE", () =>
            {
                int brushIdx = Mathf.Clamp(brushSize, 1, 5) - 1;
                GUIContent[] labels = compact
                    ? new[] { new GUIContent("1"), new GUIContent("2"), new GUIContent("3"), new GUIContent("4"), new GUIContent("5") }
                    : new[] { new GUIContent("1x1"), new GUIContent("2x2"), new GUIContent("3x3"), new GUIContent("4x4"), new GUIContent("5x5") };
                brushSize = GUILayout.Toolbar(brushIdx, labels, compactButtonStyle,
                    GUILayout.Height(24f), GUILayout.MinWidth(220f)) + 1;
            });

            DrawSection("SETTINGS", () =>
            {
                randomVariant = EditorGUILayout.ToggleLeft(
                    new GUIContent("Random variant", "Each painted cell picks a random tile from the active theme."),
                    randomVariant);

                using (new EditorGUI.DisabledScope(tileGroupOverrides.Count == 0))
                {
                    if (GUILayout.Button(new GUIContent($"Reset all overrides ({tileGroupOverrides.Count})",
                            "Clear every custom tile-to-theme assignment."), compactButtonStyle,
                            GUILayout.Height(22f), GUILayout.MinWidth(180f)))
                    {
                        ResetAllTileGroupOverrides();
                    }
                }

                showCustomGroups = EditorGUILayout.Foldout(showCustomGroups, "Theme range debug");
                if (showCustomGroups && groups != null)
                {
                    for (int i = 0; i < groups.Length; i++)
                    {
                        ThemeGroup g = groups[i];
                        int loaded = CountLoadedInRange(g.Start, g.Count);
                        int effectiveLoaded = CountLoadedInEffectiveGroup(i);
                        int effectiveTotal = EffectiveTileCountForGroup(i);
                        EditorGUILayout.LabelField(
                            $"{g.Label}: default tile_{g.Start}..{g.Start + g.Count - 1} ({loaded}/{g.Count}), effective {effectiveLoaded}/{effectiveTotal}",
                            subtleLabelStyle);
                    }
                }
            });

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawActiveSelectionCard()
        {
            ThemeGroup group = ActiveGroup();
            Color accent = ActiveAccent();
            int activeGroupIndex = mode == Mode.GroupTheme
                ? Mathf.Clamp(themeIndex, 0, groups != null ? groups.Length - 1 : 0)
                : GetEffectiveGroupIndex(singleTileIndex);
            Rect card = GUILayoutUtility.GetRect(1f, ActiveCardHeight,
                GUILayout.ExpandWidth(true), GUILayout.MinWidth(MainPanelMinWidth));

            EditorGUI.DrawRect(card, new Color(0.12f, 0.14f, 0.16f, 1f));
            DrawBorder(card, accent, 2f);

            Rect preview = new Rect(card.x + 10f, card.y + 11f, 64f, 64f);
            EditorGUI.DrawRect(preview, new Color(0f, 0f, 0f, 0.25f));
            DrawTilePreview(new Rect(preview.x + 4f, preview.y + 4f, 56f, 56f), ActivePreviewTileIndex());
            DrawBorder(preview, new Color(accent.r, accent.g, accent.b, 0.85f), 1f);

            Rect textRect = new Rect(preview.xMax + 10f, card.y + 10f,
                Mathf.Max(60f, card.xMax - preview.xMax - 20f), card.height - 20f);

            string title = mode == Mode.GroupTheme ? group.Label : GetTileDisplayName(singleTileIndex);
            int loaded = mode == Mode.GroupTheme
                ? CountLoadedInEffectiveGroup(activeGroupIndex)
                : (IsTileLoaded(singleTileIndex) ? 1 : 0);
            int total = mode == Mode.GroupTheme ? EffectiveTileCountForGroup(activeGroupIndex) : 1;
            string detail = mode == Mode.GroupTheme
                ? $"{GroupDefaultRangeLabel(activeGroupIndex)}\n{loaded}/{total} effective loaded"
                : $"tile_{singleTileIndex} | {(loaded == 1 ? "Loaded" : "Not loaded")}\n{EffectiveGroupLabel(singleTileIndex)}{(IsTileGroupOverridden(singleTileIndex) ? " (custom)" : string.Empty)}";

            Rect titleRect = new Rect(textRect.x, textRect.y, textRect.width, 34f);
            Rect detailRect = new Rect(textRect.x, titleRect.yMax + 2f, textRect.width, 32f);
            GUI.Label(titleRect, new GUIContent(title, title), activeThemeStyle);
            GUI.Label(detailRect, detail, subtleLabelStyle);
        }

        private void DrawSection(string title, ContentDelegate content)
        {
            GUILayout.Space(2f);
            Rect header = GUILayoutUtility.GetRect(1f, SectionHeaderHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(header, SectionBg);
            GUI.Label(header, title, sectionHeaderStyle);
            DrawBorder(header, new Color(0f, 0f, 0f, 0.18f), 1f);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                content();
            }
            GUILayout.Space(5f);
        }

        private void DrawStatusBar(Rect rect, float visibleWidth)
        {
            EditorGUI.DrawRect(rect, new Color(0.08f, 0.09f, 0.10f, 1f));
            string full = $"Status: {status} | Tilemap: {TilemapName()} | Selection: {ActiveSelectionSummary()}";
            string shown = visibleWidth < CompactThresholdWidth ? FitText(full, visibleWidth - 12f) : full;
            GUI.Label(new Rect(rect.x + 6f, rect.y + 2f, rect.width - 12f, rect.height - 4f),
                new GUIContent(shown, full), subtleLabelStyle);
        }

        private void ClearSelectedTilemap()
        {
            if (activeTilemap == null)
            {
                status = "No active tilemap";
                return;
            }

            if (!EditorUtility.DisplayDialog("Clear Tilemap?",
                $"Clear ALL tiles on '{activeTilemap.name}'?", "Clear", "Cancel"))
            {
                return;
            }

            Undo.RegisterCompleteObjectUndo(activeTilemap, "Clear Tilemap");
            activeTilemap.ClearAllTiles();
            EditorUtility.SetDirty(activeTilemap);
            status = "Tilemap cleared";
        }

        // -------- Tile and selection helpers --------
        private void EnsureGroupState()
        {
            if (groups == null) groups = DefaultGroups;
            if (groupCollapsed != null && groupCollapsed.Length == groups.Length) return;

            bool[] next = new bool[groups.Length];
            if (groupCollapsed != null)
            {
                Array.Copy(groupCollapsed, next, Mathf.Min(groupCollapsed.Length, next.Length));
            }
            groupCollapsed = next;
        }

        private void EnsureOverrideLists()
        {
            if (overrideTileIndices == null) overrideTileIndices = new List<int>();
            if (overrideGroupIndices == null) overrideGroupIndices = new List<int>();
        }

        private void LoadOverrideDictionary()
        {
            EnsureOverrideLists();
            tileGroupOverrides.Clear();

            int count = Mathf.Min(overrideTileIndices.Count, overrideGroupIndices.Count);
            for (int i = 0; i < count; i++)
            {
                int tileIdx = overrideTileIndices[i];
                int groupIdx = overrideGroupIndices[i];
                if (tileIdx < 0 || !IsValidGroupIndex(groupIdx)) continue;
                if (groupIdx == GetDefaultGroupIndex(tileIdx)) continue;
                tileGroupOverrides[tileIdx] = groupIdx;
            }
        }

        private void SaveOverrideLists()
        {
            EnsureOverrideLists();
            RemoveInvalidOverrideEntries();
            overrideTileIndices.Clear();
            overrideGroupIndices.Clear();

            foreach (KeyValuePair<int, int> entry in tileGroupOverrides.OrderBy(pair => pair.Key))
            {
                overrideTileIndices.Add(entry.Key);
                overrideGroupIndices.Add(entry.Value);
            }
        }

        private void RemoveInvalidOverrideEntries()
        {
            List<int> stale = new();
            foreach (KeyValuePair<int, int> entry in tileGroupOverrides)
            {
                if (entry.Key < 0 || !IsValidGroupIndex(entry.Value) || entry.Value == GetDefaultGroupIndex(entry.Key))
                {
                    stale.Add(entry.Key);
                }
            }

            foreach (int key in stale)
            {
                tileGroupOverrides.Remove(key);
            }
        }

        private void ShowTileGroupContextMenu(int tileIndex)
        {
            if (groups == null || groups.Length == 0) return;

            int currentGroup = GetEffectiveGroupIndex(tileIndex);
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < groups.Length; i++)
            {
                int groupIdx = i;
                menu.AddItem(new GUIContent($"Move to: {groups[groupIdx].Label}"),
                    currentGroup == groupIdx,
                    () => SetTileGroupOverride(tileIndex, groupIdx));
            }

            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Reset to default group"), false, () => ResetTileGroupOverride(tileIndex));
            menu.ShowAsContext();
        }

        private void SetTileGroupOverride(int tileIndex, int groupIdx)
        {
            if (!IsValidGroupIndex(groupIdx)) return;

            int defaultGroup = GetDefaultGroupIndex(tileIndex);
            if (groupIdx == defaultGroup)
            {
                tileGroupOverrides.Remove(tileIndex);
            }
            else
            {
                tileGroupOverrides[tileIndex] = groupIdx;
            }

            SaveOverrideLists();
            EditorUtility.SetDirty(this);
            status = $"tile_{tileIndex} -> {groups[groupIdx].Label}";
            Repaint();
        }

        private void ResetTileGroupOverride(int tileIndex)
        {
            bool removed = tileGroupOverrides.Remove(tileIndex);
            SaveOverrideLists();
            EditorUtility.SetDirty(this);
            status = removed ? $"tile_{tileIndex} reset to default group" : $"tile_{tileIndex} already uses default group";
            Repaint();
        }

        private void ResetAllTileGroupOverrides()
        {
            if (tileGroupOverrides.Count == 0)
            {
                status = "No tile group overrides";
                return;
            }

            if (!EditorUtility.DisplayDialog("Reset all overrides?",
                $"Clear {tileGroupOverrides.Count} custom tile group assignment(s)?", "Reset", "Cancel"))
            {
                return;
            }

            tileGroupOverrides.Clear();
            SaveOverrideLists();
            EditorUtility.SetDirty(this);
            status = "All tile group overrides reset";
            Repaint();
        }

        private bool IsValidGroupIndex(int groupIdx)
        {
            return groups != null && groupIdx >= 0 && groupIdx < groups.Length;
        }

        private int GetDefaultGroupIndex(int tileIndex)
        {
            if (groups == null) return -1;
            for (int i = 0; i < groups.Length; i++)
            {
                ThemeGroup g = groups[i];
                if (tileIndex >= g.Start && tileIndex < g.Start + g.Count) return i;
            }
            return -1;
        }

        private int GetEffectiveGroupIndex(int tileIndex)
        {
            if (tileGroupOverrides.TryGetValue(tileIndex, out int groupIdx) && IsValidGroupIndex(groupIdx))
            {
                return groupIdx;
            }
            return GetDefaultGroupIndex(tileIndex);
        }

        private bool IsTileGroupOverridden(int tileIndex)
        {
            return tileGroupOverrides.TryGetValue(tileIndex, out int groupIdx) &&
                IsValidGroupIndex(groupIdx) &&
                groupIdx != GetDefaultGroupIndex(tileIndex);
        }

        private string EffectiveGroupLabel(int tileIndex)
        {
            int groupIdx = GetEffectiveGroupIndex(tileIndex);
            return IsValidGroupIndex(groupIdx) ? groups[groupIdx].Label : "No group";
        }

        private string GroupDefaultRangeLabel(int groupIdx)
        {
            if (!IsValidGroupIndex(groupIdx)) return "No group range";
            ThemeGroup g = groups[groupIdx];
            return $"tile_{g.Start}..{g.Start + g.Count - 1}";
        }

        private int LibraryTileScanLimit()
        {
            int limit = tiles.Count;
            if (groups != null)
            {
                foreach (ThemeGroup g in groups)
                {
                    limit = Mathf.Max(limit, g.Start + g.Count);
                }
            }

            foreach (int tileIdx in tileGroupOverrides.Keys)
            {
                limit = Mathf.Max(limit, tileIdx + 1);
            }
            return Mathf.Max(0, limit);
        }

        private List<int> EffectiveTileIndicesForGroup(int groupIdx)
        {
            List<int> indices = new();
            if (!IsValidGroupIndex(groupIdx)) return indices;

            int limit = LibraryTileScanLimit();
            for (int tileIdx = 0; tileIdx < limit; tileIdx++)
            {
                if (GetEffectiveGroupIndex(tileIdx) == groupIdx) indices.Add(tileIdx);
            }
            return indices;
        }

        private int EffectiveTileCountForGroup(int groupIdx)
        {
            return EffectiveTileIndicesForGroup(groupIdx).Count;
        }

        private int CountLoadedInEffectiveGroup(int groupIdx)
        {
            int n = 0;
            foreach (int tileIdx in EffectiveTileIndicesForGroup(groupIdx))
            {
                if (IsTileLoaded(tileIdx)) n++;
            }
            return n;
        }

        private List<int> SearchMatches()
        {
            List<int> matches = new();
            if (string.IsNullOrWhiteSpace(searchFilter)) return matches;

            string q = searchFilter.Trim();
            for (int i = 0; i < LibraryTileScanLimit(); i++)
            {
                if (TileMatchesSearch(i, q)) matches.Add(i);
            }
            return matches;
        }

        private bool TileMatchesSearch(int tileIdx, string query)
        {
            if (GetTileDisplayName(tileIdx).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if ($"tile_{tileIdx}".IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (groups == null) return false;

            int groupIdx = GetEffectiveGroupIndex(tileIdx);
            if (groupIdx >= 0 && groupIdx < groups.Length &&
                groups[groupIdx].Label.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) return true;

            return false;
        }

        private ThemeGroup ActiveGroup()
        {
            if (groups == null || groups.Length == 0) return new ThemeGroup("No group", 0, 0, Color.gray);
            if (mode == Mode.SingleTile)
            {
                int groupIdx = GetEffectiveGroupIndex(singleTileIndex);
                if (groupIdx >= 0 && groupIdx < groups.Length) return groups[groupIdx];
            }
            return groups[Mathf.Clamp(themeIndex, 0, groups.Length - 1)];
        }

        private int ActivePreviewTileIndex()
        {
            if (mode == Mode.SingleTile) return singleTileIndex;
            int groupIdx = Mathf.Clamp(themeIndex, 0, groups != null ? groups.Length - 1 : 0);
            List<int> groupTiles = EffectiveTileIndicesForGroup(groupIdx);
            foreach (int idx in groupTiles)
            {
                if (IsTileLoaded(idx)) return idx;
            }
            return groupTiles.Count > 0 ? groupTiles[0] : ActiveGroup().Start;
        }

        private Color ActiveAccent()
        {
            return mode == Mode.SingleTile ? AccentForTile(singleTileIndex) : ActiveGroup().Accent;
        }

        private string ActiveSelectionSummary()
        {
            if (mode == Mode.SingleTile)
            {
                string custom = IsTileGroupOverridden(singleTileIndex) ? " (custom)" : string.Empty;
                return $"tile_{singleTileIndex} {EffectiveGroupLabel(singleTileIndex)}{custom}";
            }
            ThemeGroup g = ActiveGroup();
            int groupIdx = Mathf.Clamp(themeIndex, 0, groups != null ? groups.Length - 1 : 0);
            return $"{g.Label} {CountLoadedInEffectiveGroup(groupIdx)}/{EffectiveTileCountForGroup(groupIdx)} tiles";
        }

        private string TilemapName()
        {
            return activeTilemap != null ? activeTilemap.name : "None";
        }

        private string FitText(string text, float width)
        {
            int maxChars = Mathf.Max(16, Mathf.FloorToInt(width / 7f));
            if (text.Length <= maxChars) return text;
            return text.Substring(0, Mathf.Max(0, maxChars - 3)) + "...";
        }

        private bool IsTileLoaded(int index)
        {
            return index >= 0 && index < tiles.Count && tiles[index] != null;
        }

        private string GetTileDisplayName(int index)
        {
            if (IsTileLoaded(index) && !string.IsNullOrEmpty(tiles[index].name)) return tiles[index].name;
            return $"tile_{index}";
        }

        private Color AccentForTile(int idx)
        {
            if (groups == null) return Color.gray;
            int groupIdx = GetEffectiveGroupIndex(idx);
            if (groupIdx >= 0 && groupIdx < groups.Length) return groups[groupIdx].Accent;
            return Color.gray;
        }

        private void DrawTilePreview(Rect rect, int index)
        {
            if (!IsTileLoaded(index))
            {
                GUI.Label(rect, "-", badgeStyle);
                return;
            }

            if (tiles[index] is Tile concrete && concrete.sprite != null && concrete.sprite.texture != null)
            {
                Sprite sprite = concrete.sprite;
                Texture2D tex = sprite.texture;
                Rect tr = sprite.textureRect;
                Rect coords = new Rect(tr.x / tex.width, tr.y / tex.height, tr.width / tex.width, tr.height / tex.height);
                GUI.DrawTextureWithTexCoords(rect, tex, coords, true);
                return;
            }

            Texture2D preview = AssetPreview.GetAssetPreview(tiles[index]);
            if (preview != null)
            {
                GUI.DrawTexture(rect, preview, ScaleMode.ScaleToFit, true);
            }
            else
            {
                GUI.Label(rect, "-", badgeStyle);
            }
        }

        private void DrawBorder(Rect rect, Color color, float thickness)
        {
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, thickness), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - thickness, rect.width, thickness), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, thickness, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - thickness, rect.y, thickness, rect.height), color);
        }

        // -------- Scene interaction --------
        private void OnSceneGUI(SceneView sceneView)
        {
            if (activeTilemap == null)
            {
                activeTilemap = FindFloorTilemap();
                hasHover = false;
                return;
            }

            Event current = Event.current;
            hoverCell = activeTilemap.WorldToCell(MouseToWorld(current.mousePosition));
            hasHover = true;
            DrawBrushPreview();
            sceneView.Repaint();

            if (current.alt || current.button != 0)
            {
                if (current.type == EventType.MouseUp) isPainting = false;
                return;
            }

            if (current.type == EventType.MouseDown)
            {
                isPainting = true;
                ApplyBrush(hoverCell);
                current.Use();
            }
            else if (current.type == EventType.MouseDrag && isPainting)
            {
                ApplyBrush(hoverCell);
                current.Use();
            }
            else if (current.type == EventType.MouseUp && isPainting)
            {
                isPainting = false;
                current.Use();
            }
        }

        private void ApplyBrush(Vector3Int center)
        {
            if (activeTilemap == null) return;
            Undo.RegisterCompleteObjectUndo(activeTilemap, tool == Tool.Paint ? "Paint Tile" : "Erase Tile");

            foreach (Vector3Int cell in BrushCells(center))
            {
                TileBase selectedTile = tool == Tool.Paint ? PickTile() : null;
                if (tool == Tool.Paint && selectedTile == null)
                {
                    status = "No tile loaded for current selection";
                    Repaint();
                    return;
                }
                activeTilemap.SetTile(cell, selectedTile);
            }

            EditorUtility.SetDirty(activeTilemap);
            EditorSceneManager.MarkSceneDirty(activeTilemap.gameObject.scene);
            string what = mode == Mode.GroupTheme
                ? (groups != null && themeIndex < groups.Length ? groups[themeIndex].Label : "theme")
                : $"tile_{singleTileIndex}";
            status = tool == Tool.Paint
                ? $"Painted {brushSize}x{brushSize} ({what})"
                : $"Erased {brushSize}x{brushSize}";
            Repaint();
        }

        private TileBase PickTile()
        {
            if (mode == Mode.SingleTile)
            {
                if (singleTileIndex < 0 || singleTileIndex >= tiles.Count) return null;
                return tiles[singleTileIndex];
            }

            if (groups == null || themeIndex < 0 || themeIndex >= groups.Length) return null;
            List<int> groupTiles = EffectiveTileIndicesForGroup(themeIndex);
            if (groupTiles.Count <= 0) return null;

            int written = 0;
            TileBase[] buf = new TileBase[groupTiles.Count];
            foreach (int tileIdx in groupTiles)
            {
                if (!IsTileLoaded(tileIdx)) continue;
                TileBase t = tiles[tileIdx];
                if (t != null) buf[written++] = t;
            }
            if (written == 0) return null;
            int pick = randomVariant ? UnityEngine.Random.Range(0, written) : 0;
            return buf[pick];
        }

        private IEnumerable<Vector3Int> BrushCells(Vector3Int center)
        {
            int min = -(brushSize - 1) / 2;
            int max = brushSize / 2;
            for (int y = min; y <= max; y++)
            {
                for (int x = min; x <= max; x++)
                {
                    yield return new Vector3Int(center.x + x, center.y + y, center.z);
                }
            }
        }

        private void DrawBrushPreview()
        {
            if (!hasHover || activeTilemap == null) return;
            Handles.color = tool == Tool.Erase
                ? new Color(1f, 0.35f, 0.25f, 0.95f)
                : ActiveAccent();
            foreach (Vector3Int cell in BrushCells(hoverCell))
            {
                DrawWireDiamond(cell);
            }
        }

        private void DrawWireDiamond(Vector3Int cell)
        {
            Vector3 center = activeTilemap.GetCellCenterWorld(cell);
            Vector3 size = activeTilemap.layoutGrid != null ? activeTilemap.layoutGrid.cellSize : Vector3.one;
            float halfWidth = Mathf.Max(0.1f, size.x * 0.5f);
            float halfHeight = Mathf.Max(0.1f, size.y * 0.5f);
            Vector3[] points =
            {
                center + new Vector3(0f, halfHeight, 0f),
                center + new Vector3(halfWidth, 0f, 0f),
                center + new Vector3(0f, -halfHeight, 0f),
                center + new Vector3(-halfWidth, 0f, 0f),
                center + new Vector3(0f, halfHeight, 0f),
            };
            Handles.DrawAAPolyLine(3f, points);
        }

        private Vector3 MouseToWorld(Vector2 guiPosition)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(guiPosition);
            Plane plane = new Plane(Vector3.forward, activeTilemap.transform.position);
            return plane.Raycast(ray, out float distance)
                ? ray.GetPoint(distance)
                : activeTilemap.transform.position;
        }

        // -------- Tile loading (dynamic, supports any tile_N.asset count) --------
        private void LoadTileAssets()
        {
            tiles.Clear();
            if (!AssetDatabase.IsValidFolder(TileFolder))
            {
                status = $"Folder missing: {TileFolder}";
                return;
            }

            // Sort tile_N.asset by N to keep stable order regardless of disk order.
            string[] guids = AssetDatabase.FindAssets("t:TileBase", new[] { TileFolder });
            var loaded = new List<(int idx, TileBase tile)>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string name = Path.GetFileNameWithoutExtension(path);
                if (!name.StartsWith("tile_", StringComparison.OrdinalIgnoreCase)) continue;
                if (!int.TryParse(name.Substring(5), out int n)) continue;
                TileBase t = AssetDatabase.LoadAssetAtPath<TileBase>(path);
                if (t != null) loaded.Add((n, t));
            }

            loaded.Sort((a, b) => a.idx.CompareTo(b.idx));
            int maxIdx = loaded.Count == 0 ? -1 : loaded[^1].idx;

            // Pad list so indexing by tile number works.
            for (int i = 0; i <= maxIdx; i++) tiles.Add(null);
            foreach ((int idx, TileBase tile) in loaded) tiles[idx] = tile;

            status = $"Loaded {loaded.Count} tile(s) (indices 0..{maxIdx})";
        }

        private int CountLoadedInRange(int start, int count)
        {
            int n = 0;
            int end = Mathf.Min(start + count, tiles.Count);
            for (int i = start; i < end; i++)
            {
                if (tiles[i] != null) n++;
            }
            return n;
        }

        private static Tilemap FindFloorTilemap()
        {
            Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            string[] priority = { "Floor", "Ground", "L1", "Base", "Tilemap" };
            foreach (string keyword in priority)
            {
                Tilemap match = tilemaps.FirstOrDefault(tilemap =>
                    tilemap != null && tilemap.name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                if (match != null) return match;
            }
            return tilemaps.Length > 0 ? tilemaps[0] : null;
        }
    }
}
#endif
