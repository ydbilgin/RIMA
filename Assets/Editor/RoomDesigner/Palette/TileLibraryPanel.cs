namespace RIMA.Editor.RoomDesigner.Palette
{
    using System;
    using System.Collections.Generic;
    using RIMA.RoomDesigner.Core;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public sealed class TileLibraryPanel : VisualElement, IDisposable
    {
        private const string TileRoot = "Assets/Art/Tiles";
        internal const string PanelName = "room-designer-tile-library";
        private const string SelectedClass = "rd-tile-card--selected";
        private const int PreviewSize = 44;

        private static readonly (string label, string value)[] BiomeTabs =
        {
            ("Keep", BiomeFilter.Keep),
            ("Crypt", BiomeFilter.Crypt),
            ("Volcanic", BiomeFilter.Volcanic),
            ("All", BiomeFilter.All)
        };

        private readonly IRoomDesignerContext ctx;
        private readonly AssetPreviewCache previewCache;
        private readonly List<TileEntry> allTiles = new List<TileEntry>();
        private readonly List<TileEntry> filteredTiles = new List<TileEntry>();
        private readonly Dictionary<TileBase, VisualElement> cardsByTile = new Dictionary<TileBase, VisualElement>();
        private readonly Dictionary<TileBase, Image> previewImagesByTile = new Dictionary<TileBase, Image>();
        private readonly Dictionary<string, Button> biomeButtons = new Dictionary<string, Button>();
        private readonly Dictionary<RoomLayer, Button> layerButtons = new Dictionary<RoomLayer, Button>();
        private static readonly RoomLayer[] PaintLayers =
        {
            RoomLayer.Base,
            RoomLayer.Decal,
            RoomLayer.Wall,
            RoomLayer.Prop
        };

        private VisualElement grid;
        private Label emptyState;
        private string activeBiome = BiomeFilter.All;
        private bool disposed;

        public TileLibraryPanel(IRoomDesignerContext ctx)
        {
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            name = PanelName;
            AddToClassList("rd-tile-library");

            previewCache = new AssetPreviewCache(RefreshPreviewImages);

            Build();
            ReloadTiles();
            RegisterCallback<DetachFromPanelEvent>(_ => Dispose());
            schedule.Execute(SyncLayerFromContext).Every(250);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            previewCache.Dispose();
        }

        public void ReloadTiles()
        {
            allTiles.Clear();

            if (AssetDatabase.IsValidFolder(TileRoot))
            {
                string[] guids = AssetDatabase.FindAssets("t:TileBase", new[] { TileRoot });
                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
                    if (tile == null)
                    {
                        continue;
                    }

                    allTiles.Add(new TileEntry(tile, path, BiomeFilter.BiomeOf(path)));
                }
            }

            allTiles.Sort((a, b) => string.Compare(a.Path, b.Path, StringComparison.OrdinalIgnoreCase));
            ApplyFilters();
        }

        private void Build()
        {
            var title = new Label("Tile Library");
            title.AddToClassList("rd-palette-title");
            Add(title);

            var biomeRow = new VisualElement();
            biomeRow.AddToClassList("rd-biome-row");
            Add(biomeRow);

            foreach ((string label, string value) in BiomeTabs)
            {
                string tabValue = value;
                var button = new Button(() => SetBiome(tabValue)) { text = label };
                button.AddToClassList("rd-biome-tab");
                biomeButtons[tabValue] = button;
                biomeRow.Add(button);
            }

            var layerRow = new VisualElement();
            layerRow.AddToClassList("rd-layer-row");
            Add(layerRow);

            foreach (RoomLayer layer in PaintLayers)
            {
                RoomLayer capturedLayer = layer;
                var button = new Button(() =>
                {
                    ctx.ActiveLayer = capturedLayer;
                    SyncLayerFromContext();
                    ctx.MarkDirty();
                })
                {
                    text = layer.ToString()
                };
                button.AddToClassList("rd-layer-chip");
                layerButtons[layer] = button;
                layerRow.Add(button);
            }

            var scrollView = new ScrollView(ScrollViewMode.Vertical);
            scrollView.AddToClassList("rd-tile-scroll");
            Add(scrollView);

            grid = new VisualElement();
            grid.AddToClassList("rd-tile-grid");
            scrollView.Add(grid);

            emptyState = new Label("No tiles");
            emptyState.AddToClassList("rd-empty-state");
            Add(emptyState);

            UpdateBiomeButtonState();
            SyncLayerFromContext();
        }

        private void SetBiome(string biome)
        {
            if (string.Equals(activeBiome, biome, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            activeBiome = biome;
            UpdateBiomeButtonState();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            filteredTiles.Clear();
            for (int i = 0; i < allTiles.Count; i++)
            {
                TileEntry entry = allTiles[i];
                if (BiomeFilter.Matches(entry.Path, activeBiome))
                {
                    filteredTiles.Add(entry);
                }
            }

            RebuildGrid();
        }

        private void RebuildGrid()
        {
            cardsByTile.Clear();
            previewImagesByTile.Clear();
            grid.Clear();

            emptyState.style.display = filteredTiles.Count == 0 ? DisplayStyle.Flex : DisplayStyle.None;

            for (int i = 0; i < filteredTiles.Count; i++)
            {
                TileEntry entry = filteredTiles[i];
                VisualElement card = CreateTileCard(entry);
                cardsByTile[entry.Tile] = card;
                grid.Add(card);
            }

            UpdateSelection();
            RefreshPreviewImages();
        }

        private VisualElement CreateTileCard(TileEntry entry)
        {
            var card = new VisualElement
            {
                tooltip = entry.Tile.name + "\n" + entry.Path
            };
            card.AddToClassList("rd-tile-card");

            var image = new Image
            {
                scaleMode = ScaleMode.ScaleToFit,
                image = Texture2D.grayTexture
            };
            image.style.width = PreviewSize;
            image.style.height = PreviewSize;
            image.style.alignSelf = Align.Center;
            card.Add(image);
            previewImagesByTile[entry.Tile] = image;

            var label = new Label(entry.Tile.name);
            label.AddToClassList("rd-tile-label");
            card.Add(label);

            card.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0)
                {
                    return;
                }

                ctx.ActiveTile = entry.Tile;
                UpdateSelection();
                ctx.MarkDirty();
                evt.StopPropagation();
            });

            return card;
        }

        private void RefreshPreviewImages()
        {
            foreach (KeyValuePair<TileBase, Image> pair in previewImagesByTile)
            {
                Texture2D preview = previewCache.Get(pair.Key);
                pair.Value.image = preview != null ? preview : Texture2D.grayTexture;
            }

            MarkDirtyRepaint();
        }

        private void UpdateSelection()
        {
            foreach (KeyValuePair<TileBase, VisualElement> pair in cardsByTile)
            {
                pair.Value.EnableInClassList(SelectedClass, pair.Key == ctx.ActiveTile);
            }
        }

        private void UpdateBiomeButtonState()
        {
            foreach (KeyValuePair<string, Button> pair in biomeButtons)
            {
                pair.Value.EnableInClassList("rd-biome-tab--active", string.Equals(pair.Key, activeBiome, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void SyncLayerFromContext()
        {
            foreach (KeyValuePair<RoomLayer, Button> pair in layerButtons)
            {
                pair.Value.EnableInClassList("rd-layer-chip--active", pair.Key == ctx.ActiveLayer);
            }
        }

        private readonly struct TileEntry
        {
            public TileEntry(TileBase tile, string path, string biome)
            {
                Tile = tile;
                Path = path;
                Biome = biome;
            }

            public TileBase Tile { get; }
            public string Path { get; }
            public string Biome { get; }
        }

        internal static bool IsMounted(VisualElement leftPanel)
        {
            return leftPanel != null && leftPanel.Q<TileLibraryPanel>(PanelName) != null;
        }
    }

    [InitializeOnLoad]
    internal static class TileLibraryPanelBootstrap
    {
        private const double MountIntervalSeconds = 0.25d;
        private static double nextMountTime;

        static TileLibraryPanelBootstrap()
        {
            EditorApplication.update += MountIntoOpenWindows;
        }

        private static void MountIntoOpenWindows()
        {
            double now = EditorApplication.timeSinceStartup;
            if (now < nextMountTime)
            {
                return;
            }

            nextMountTime = now + MountIntervalSeconds;
            var windows = Resources.FindObjectsOfTypeAll<RimaRoomDesignerWindow>();
            for (int i = 0; i < windows.Length; i++)
            {
                RimaRoomDesignerWindow window = windows[i];
                VisualElement leftPanel = window.LeftPanel;
                if (leftPanel == null || TileLibraryPanel.IsMounted(leftPanel))
                {
                    continue;
                }

                leftPanel.Add(new TileLibraryPanel(window));
                window.MarkDirty();
            }
        }
    }

    internal sealed class TileLibraryAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var windows = Resources.FindObjectsOfTypeAll<RimaRoomDesignerWindow>();
            for (int i = 0; i < windows.Length; i++)
            {
                VisualElement leftPanel = windows[i].LeftPanel;
                var panel = leftPanel?.Q<TileLibraryPanel>(TileLibraryPanel.PanelName);
                panel?.ReloadTiles();
            }
        }
    }
}
