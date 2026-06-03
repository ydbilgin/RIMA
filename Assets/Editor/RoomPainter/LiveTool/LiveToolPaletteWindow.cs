using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RIMA.Live;
using RIMA.RoomPainter;

namespace RIMA.Editor.RoomPainter.LiveTool
{
    /// <summary>
    /// C5 — LiveToolPaletteWindow (F3).
    /// EditorWindow: palette + filter UI for the Live Tool brush selection.
    /// Menu: RIMA → Live Tool → Palette
    ///
    /// Layout (spec §F3 UXML hierarchy adapted for IMGUI EditorWindow):
    ///   Toolbar: [Mode dropdown] [Layer dropdown] [Search field] [Reload]
    ///   Thumbnail grid: clickable asset buttons (sprite or displayName label)
    ///   Bottom strip: selected asset name + GUID
    ///
    /// Reads RuntimeAssetRegistry via RuntimeAssetLoader (C9).
    /// Selection state held in RuntimeBrushPalette (C6) — F4/F5 read palette.SelectedEntry.
    /// </summary>
    public sealed class LiveToolPaletteWindow : EditorWindow
    {
        // ── Constants ──────────────────────────────────────────────────────────

        private const string WindowTitle  = "Live Tool Palette";
        private const int    ThumbSize    = 64;
        private const int    ThumbPadding = 4;
        private const int    ToolbarHeight   = 22;
        private const int    BottomStripHeight = 42;
        /// <summary>Height of the C7 collider handles panel (shown when a prefab entry is selected).</summary>
        private const int    ColliderPanelHeight = 220;

        // ── Singleton ──────────────────────────────────────────────────────────

        /// <summary>
        /// Access the active palette model from F4/F5 code:
        ///   LiveToolPaletteWindow.Palette?.SelectedEntry
        /// Returns null if the window has never been opened this session.
        /// </summary>
        public static RuntimeBrushPalette Palette => _palette;
        private static RuntimeBrushPalette _palette;

        // ── Fields ─────────────────────────────────────────────────────────────

        private Vector2 _scrollPos;
        private string  _searchText = string.Empty;

        // C7 — RuntimeColliderHandles instance (F4).
        // One per window; instantiated lazily on first OnGUI.
        private RuntimeColliderHandles _colliderHandles;

        // Dropdown display arrays
        private static readonly string[] ModeLabels  = { "All", "Tile", "Cliff", "Decor", "Object" };
        private static readonly PaletteMode[] ModeValues = { PaletteMode.All, PaletteMode.Tile, PaletteMode.Cliff, PaletteMode.Decor, PaletteMode.Object };
        private static readonly string[] LayerLabels = { "All Layers", "Floor", "Edge", "Cliff", "Wall", "Props", "Decals", "Lighting", "Parallax" };
        private static readonly RoomLayer[] LayerValues = { RoomLayer.Floor, RoomLayer.Edge, RoomLayer.Cliff, RoomLayer.Wall, RoomLayer.Props, RoomLayer.Decals, RoomLayer.Lighting, RoomLayer.Parallax };

        private int _selectedModeIndex  = 0;
        private int _selectedLayerIndex = 0; // 0 = "All Layers"

        // Thumbnail cache: guid → loaded preview texture (static: single window instance)
        private static readonly Dictionary<string, Texture2D> _thumbCache = new Dictionary<string, Texture2D>();
        // Tracks textures we allocated via new Texture2D (crop) — must be DestroyImmediate'd
        private static readonly HashSet<Texture2D> _ownedThumbs = new HashSet<Texture2D>();

        // ── Menu ───────────────────────────────────────────────────────────────

        [MenuItem("RIMA/Legacy/Live Tool/Palette")]
        public static void Open()
        {
            LiveToolPaletteWindow win = GetWindow<LiveToolPaletteWindow>(false, WindowTitle, true);
            win.minSize = new Vector2(340, 300);
            win.Show();
        }

        // ── Lifecycle ──────────────────────────────────────────────────────────

        private void OnEnable()
        {
            if (_palette == null) _palette = new RuntimeBrushPalette();
            ReloadRegistry();
        }

        private void OnDisable()
        {
            DestroyOwnedThumbs();
        }

        private static void DestroyOwnedThumbs()
        {
            foreach (Texture2D tex in _ownedThumbs)
            {
                if (tex != null)
                    Object.DestroyImmediate(tex);
            }
            _ownedThumbs.Clear();
            _thumbCache.Clear();
        }

        // ── GUI ────────────────────────────────────────────────────────────────

        private void OnGUI()
        {
            // Lazy-init C7
            if (_colliderHandles == null)
                _colliderHandles = new RuntimeColliderHandles();

            DrawToolbar();

            if (_palette == null || !_palette.HasEntries)
            {
                DrawEmptyState();
                return;
            }

            DrawThumbnailGrid();
            DrawBottomStrip();

            // C7 — collider handles panel: visible when selected entry has a prefab
            bool hasPrefabEntry = _palette.SelectedEntry?.prefab != null;
            if (hasPrefabEntry)
            {
                DrawColliderHandlesPanel();
                // Repaint each frame while handles panel is open so drag is smooth
                Repaint();
            }
        }

        // ── Collider handles panel (C7) ────────────────────────────────────────

        private void DrawColliderHandlesPanel()
        {
            float panelY = position.height - BottomStripHeight - ColliderPanelHeight;
            Rect panelRect = new Rect(0f, panelY, position.width, ColliderPanelHeight);

            // Separator line
            EditorGUI.DrawRect(new Rect(0f, panelY - 1f, position.width, 1f),
                               new Color(0.20f, 0.20f, 0.22f, 1f));

            // Section header
            Rect headerRect = new Rect(0f, panelY, position.width, 16f);
            EditorGUI.DrawRect(headerRect, new Color(0.16f, 0.16f, 0.18f, 1f));
            GUI.Label(headerRect, "  Collider Handles  (F4 / C7)",
                      new GUIStyle(EditorStyles.miniLabel)
                      {
                          normal = { textColor = new Color(0.70f, 0.70f, 0.75f, 1f) }
                      });

            // Delegate to RuntimeColliderHandles
            Rect drawRect = new Rect(0f, panelY + 17f, position.width,
                                     ColliderPanelHeight - 17f);
            bool changed = _colliderHandles.Draw(drawRect);
            if (changed)
                Repaint();
        }

        // ── Toolbar ────────────────────────────────────────────────────────────

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(ToolbarHeight));

            // Mode dropdown
            int newModeIdx = EditorGUILayout.Popup(_selectedModeIndex, ModeLabels, EditorStyles.toolbarDropDown, GUILayout.Width(72));
            if (newModeIdx != _selectedModeIndex)
            {
                _selectedModeIndex = newModeIdx;
                _palette?.SetMode(ModeValues[newModeIdx]);
            }

            // Layer dropdown
            int newLayerIdx = EditorGUILayout.Popup(_selectedLayerIndex, LayerLabels, EditorStyles.toolbarDropDown, GUILayout.Width(90));
            if (newLayerIdx != _selectedLayerIndex)
            {
                _selectedLayerIndex = newLayerIdx;
                if (_palette != null)
                {
                    // Index 0 = "All Layers" → clear filter; indices 1-N map to LayerValues[index-1]
                    if (newLayerIdx == 0)
                        _palette.ClearLayerFilter();
                    else
                        _palette.SetLayerFilter(LayerValues[newLayerIdx - 1]);
                }
            }

            // Search field
            GUILayout.Space(4);
            string newSearch = EditorGUILayout.TextField(_searchText, EditorStyles.toolbarSearchField, GUILayout.ExpandWidth(true));
            if (newSearch != _searchText)
            {
                _searchText = newSearch;
                _palette?.SetSearch(newSearch);
            }

            // Reload button
            if (GUILayout.Button("Reload", EditorStyles.toolbarButton, GUILayout.Width(52)))
            {
                RuntimeAssetLoader.Reload();
                DestroyOwnedThumbs();
                ReloadRegistry();
            }

            EditorGUILayout.EndHorizontal();
        }

        // ── Empty state ────────────────────────────────────────────────────────

        private void DrawEmptyState()
        {
            GUILayout.FlexibleSpace();

            GUIStyle centered = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize   = 11,
            };

            GUILayout.Label(
                "Registry is empty.\n\nRun  RIMA → Legacy → Live Tool → Bake Asset Registry\nthen press Reload.",
                centered);

            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Bake Asset Registry", GUILayout.Width(160)))
            {
                EditorApplication.ExecuteMenuItem("RIMA/Legacy/Live Tool/Bake Asset Registry");
                DestroyOwnedThumbs();
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.Refresh();
                    RuntimeAssetLoader.Reload();
                    ReloadRegistry();
                };
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
        }

        // ── Thumbnail grid ─────────────────────────────────────────────────────

        private void DrawThumbnailGrid()
        {
            IReadOnlyList<RegistryEntry> filtered = _palette.GetFiltered();

            float availableWidth = position.width - 8f; // 4px margin each side
            int columns = Mathf.Max(1, Mathf.FloorToInt(availableWidth / (ThumbSize + ThumbPadding)));
            float cellSize = ThumbSize + ThumbPadding;

            bool hasPrefab = _palette?.SelectedEntry?.prefab != null;
            float gridHeight = position.height - ToolbarHeight - BottomStripHeight
                               - (hasPrefab ? ColliderPanelHeight : 0f);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(gridHeight));

            GUILayout.BeginVertical();
            int col = 0;
            GUILayout.BeginHorizontal();

            foreach (RegistryEntry entry in filtered)
            {
                DrawThumb(entry, cellSize);
                col++;
                if (col >= columns)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    col = 0;
                }
            }

            // Pad remaining cells in the last row
            if (col > 0 && col < columns)
            {
                for (int i = col; i < columns; i++)
                    GUILayout.Space(cellSize);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }

        private void DrawThumb(RegistryEntry entry, float cellSize)
        {
            bool isSelected = _palette.SelectedEntry == entry;

            // Highlight box for selected entry
            Rect thumbRect = GUILayoutUtility.GetRect(cellSize, cellSize, GUILayout.Width(cellSize), GUILayout.Height(cellSize));
            if (isSelected)
            {
                EditorGUI.DrawRect(thumbRect, new Color(0.25f, 0.55f, 1f, 0.35f));
            }

            Texture2D thumb = GetThumb(entry);
            GUIContent content = thumb != null
                ? new GUIContent(thumb, entry.displayName)
                : new GUIContent(entry.displayName[..Mathf.Min(8, entry.displayName.Length)], entry.displayName);

            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                fontSize  = 8,
                wordWrap  = true,
                alignment = TextAnchor.LowerCenter,
            };
            if (thumb != null)
            {
                style.imagePosition = ImagePosition.ImageAbove;
            }

            if (GUI.Button(thumbRect, content, style))
            {
                if (isSelected)
                    _palette.Select(null); // toggle off
                else
                    _palette.Select(entry);

                Repaint();
            }
        }

        // ── Bottom strip ───────────────────────────────────────────────────────

        private void DrawBottomStrip()
        {
            Rect stripRect = new Rect(0, position.height - BottomStripHeight, position.width, BottomStripHeight);
            EditorGUI.DrawRect(stripRect, new Color(0.18f, 0.18f, 0.18f, 1f));

            GUILayout.BeginArea(stripRect);
            GUILayout.Space(4);

            if (_palette.SelectedEntry != null)
            {
                RegistryEntry sel = _palette.SelectedEntry;
                EditorGUILayout.LabelField(sel.displayName, EditorStyles.boldLabel, GUILayout.Height(16));
                EditorGUILayout.LabelField($"[{sel.kind}] [{sel.layer}] [{sel.tag}]", EditorStyles.miniLabel, GUILayout.Height(14));
            }
            else
            {
                EditorGUILayout.LabelField("No brush selected", EditorStyles.centeredGreyMiniLabel, GUILayout.Height(16));
                IReadOnlyList<RegistryEntry> filtered = _palette.GetFiltered();
                EditorGUILayout.LabelField($"{filtered.Count} assets", EditorStyles.miniLabel, GUILayout.Height(14));
            }

            GUILayout.EndArea();
        }

        // ── Thumbnail loading ──────────────────────────────────────────────────

        private Texture2D GetThumb(RegistryEntry entry)
        {
            if (_thumbCache.TryGetValue(entry.guid, out Texture2D cached)) return cached;

            Texture2D tex = null;

            if (entry.sprite != null)
            {
                // Crop sprite rect from texture
                tex = CropSpriteToTexture(entry.sprite);
            }
            else if (entry.prefab != null)
            {
                // AssetPreview for prefabs — may return null until Unity generates it async
                tex = AssetPreview.GetAssetPreview(entry.prefab);
                if (tex == null)
                    tex = AssetPreview.GetMiniThumbnail(entry.prefab);
            }

            _thumbCache[entry.guid] = tex; // store even if null (avoid repeated lookups)
            return tex;
        }

        private static Texture2D CropSpriteToTexture(Sprite sprite)
        {
            if (sprite == null) return null;

            Texture2D src = sprite.texture;
            if (src == null) return null;

            // Check texture is readable; if not return the raw texture as fallback
            if (!src.isReadable)
                return src;

            Rect rect = sprite.rect;
            int x = Mathf.FloorToInt(rect.x);
            int y = Mathf.FloorToInt(rect.y);
            int w = Mathf.FloorToInt(rect.width);
            int h = Mathf.FloorToInt(rect.height);

            if (w <= 0 || h <= 0) return src;

            Color[] pixels = src.GetPixels(x, y, w, h);
            Texture2D dst = new Texture2D(w, h, TextureFormat.RGBA32, false);
            dst.SetPixels(pixels);
            dst.Apply();
            _ownedThumbs.Add(dst); // track for DestroyImmediate in DestroyOwnedThumbs
            return dst;
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private void ReloadRegistry()
        {
            RuntimeAssetRegistry reg = RuntimeAssetLoader.Load();
            if (_palette == null) _palette = new RuntimeBrushPalette();
            _palette.SetRegistry(reg);
            Repaint();
        }
    }
}
