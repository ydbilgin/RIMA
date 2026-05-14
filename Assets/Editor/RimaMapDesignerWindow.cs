using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RIMA;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public class RimaMapDesignerWindow : EditorWindow
    {
        private const int DefaultRoomWidth = 16;
        private const int DefaultRoomHeight = 12;
        private const int MinRoomSize = 4;
        private const int MaxRoomSize = 64;
        private const int MaxLayers = 4;
        private const float LeftPanelWidth = 220f;
        private const float RightPanelWidth = 200f;
        private const float ToolbarHeight = 28f;
        private const float StatusHeight = 22f;
        private const float VertexRadius = 5f;
        private const float CanvasPadding = 24f;
        private const string MapDataFolder = "Assets/RIMA_MapData";
        private const string DefaultBiomePresetPath = "Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset";

        private static readonly string[] CornerKeyNames =
        {
            "All Floor",
            "SE Corner",
            "SW Corner",
            "S Edge",
            "NE Corner",
            "E Edge",
            "NE-SW Diag",
            "No NW",
            "NW Corner",
            "NW-SE Diag",
            "W Edge",
            "No NE",
            "N Edge",
            "No SW",
            "No SE",
            "All Wall"
        };

        public enum PaintMode
        {
            Cell,
            Vertex
        }

        private enum PaintTool
        {
            Brush,
            Fill,
            Rectangle
        }

        [Serializable]
        public class MapLayer
        {
            public string name = "Base";
            public Tilemap tilemap;
            public CornerWangTileSetSO tileSet;
            public bool enabled = true;
            [HideInInspector] public int[] flatVertexData;
            [NonSerialized] public int[,] vertGrid;
        }

        [Serializable]
        public class MapSaveData
        {
            public int width;
            public int height;
            public int[] terrainGrid;
            public string biomePresetGuid;
            public LayerSaveData[] layers;

            public int[] vertexData;
            public string[] layerNames;
        }

        [Serializable]
        public class LayerSaveData
        {
            public string name;
            public string tileSet;
            public bool enabled;
            public int[] vertexData;
        }

        [SerializeField] private int roomWidth = DefaultRoomWidth;
        [SerializeField] private int roomHeight = DefaultRoomHeight;
        [SerializeField] private float cellSize = 32f;
        [SerializeField] private int currentPaintValue = 1;
        [SerializeField] private RimaBiomePreset activeBiome;
        [SerializeField] private int activeTerrainId = 1;
        [SerializeField] private int wallThickness = 2;
        [SerializeField] private float noiseDensity = 0.45f;
        [SerializeField] private int noiseSeed = 12345;
        [SerializeField] private bool proceduralFoldout;
        [SerializeField] private bool showTilePreview = true;
        [SerializeField] private PaintTool activeTool = PaintTool.Brush;
        [SerializeField] private PaintMode paintMode = PaintMode.Cell;
        [SerializeField] private bool eraseMode;
        [SerializeField] private int brushRadius = 1;
        [SerializeField] private List<MapLayer> layers = new List<MapLayer>();

        private readonly BrushInputHandler brushInput = new BrushInputHandler();
        private readonly TilesetPaletteDrawer tilesetPalette = new TilesetPaletteDrawer();
        private ReorderableList layerList;
        [NonSerialized] private int[,] terrainGrid;
        [SerializeField, HideInInspector] private int[] flatTerrainData;
        private Vector2 gridScroll;
        private Vector2 leftScroll;
        private Vector2Int hoveredVertex = new Vector2Int(-1, -1);
        private Vector2Int hoveredCell = new Vector2Int(-1, -1);
        private Vector2Int rectStart = new Vector2Int(-1, -1);
        private Vector2Int rectCurrent = new Vector2Int(-1, -1);
        private bool isPainting;
        private bool isRectangleDragging;
        private bool isPanning;
        private bool spaceHeld;
        private Vector2 panStartMouse;
        private Vector2 panStartScroll;
        private int activeLayerIndex;
        private Vector2Int lastPaintedCell = new Vector2Int(-1, -1);

        [MenuItem("RIMA/Tools/Map Designer")]
        public static void Open()
        {
            RimaMapDesignerWindow window = GetWindow<RimaMapDesignerWindow>("RIMA Map Designer");
            window.minSize = new Vector2(720f, 420f);
            window.Show();
        }

        private void OnEnable()
        {
            EnsureInitialized();
            BuildLayerList();
            tilesetPalette.Refresh();
        }

        private void OnDisable()
        {
            StoreAllLayerGrids();
            StoreTerrainGrid();
        }

        private void OnGUI()
        {
            EnsureInitialized();
            DrawToolbar();

            Rect contentRect = new Rect(0f, ToolbarHeight, position.width, Mathf.Max(0f, position.height - ToolbarHeight - StatusHeight));
            GUILayout.BeginArea(contentRect);
            EditorGUILayout.BeginHorizontal();
            DrawLeftPanel();
            DrawCenterPanel(contentRect.height);
            DrawRightPanel();
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();

            DrawStatusBar();
        }

        private void EnsureInitialized()
        {
            if (layers == null)
            {
                layers = new List<MapLayer>();
            }

            if (layers.Count == 0)
            {
                layers.Add(new MapLayer { name = "Base" });
            }

            activeLayerIndex = Mathf.Clamp(activeLayerIndex, 0, layers.Count - 1);
            foreach (MapLayer layer in layers)
            {
                EnsureLayerGrid(layer, roomWidth, roomHeight, false);
            }

            if (activeBiome == null)
            {
                activeBiome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>(DefaultBiomePresetPath);
            }

            EnsureTerrainGrid(roomWidth, roomHeight, false);
            EnsureValidActiveTerrain();
        }

        private void EnsureTerrainGrid(int width, int height, bool preserve)
        {
            int expectedWidth = width + 1;
            int expectedHeight = height + 1;
            if (terrainGrid != null && terrainGrid.GetLength(0) == expectedWidth && terrainGrid.GetLength(1) == expectedHeight)
            {
                return;
            }

            int[,] oldGrid = preserve ? terrainGrid : null;
            int[,] newGrid = new int[expectedWidth, expectedHeight];
            if (oldGrid != null)
            {
                int copyWidth = Mathf.Min(oldGrid.GetLength(0), expectedWidth);
                int copyHeight = Mathf.Min(oldGrid.GetLength(1), expectedHeight);
                for (int y = 0; y < copyHeight; y++)
                {
                    for (int x = 0; x < copyWidth; x++)
                    {
                        newGrid[x, y] = oldGrid[x, y];
                    }
                }
            }
            else if (flatTerrainData != null)
            {
                UnflattenGridInto(flatTerrainData, newGrid, width, height, false);
            }

            terrainGrid = newGrid;
            StoreTerrainGrid();
        }

        private void EnsureValidActiveTerrain()
        {
            if (activeBiome == null || activeBiome.terrains == null)
            {
                activeTerrainId = Mathf.Max(1, activeTerrainId);
                currentPaintValue = activeTerrainId;
                return;
            }

            if (!activeBiome.terrains.Any(t => t != null && t.id == activeTerrainId && t.id != 0))
            {
                MapTerrain firstPaintable = activeBiome.terrains.FirstOrDefault(t => t != null && t.id != 0);
                activeTerrainId = firstPaintable != null ? firstPaintable.id : 1;
            }

            currentPaintValue = activeTerrainId;
        }

        private void EnsureLayerGrid(MapLayer layer, int width, int height, bool preserve)
        {
            if (layer == null)
            {
                return;
            }

            int expectedWidth = width + 1;
            int expectedHeight = height + 1;
            if (layer.vertGrid != null && layer.vertGrid.GetLength(0) == expectedWidth && layer.vertGrid.GetLength(1) == expectedHeight)
            {
                return;
            }

            int[,] oldGrid = preserve ? layer.vertGrid : null;
            int[,] newGrid = new int[expectedWidth, expectedHeight];

            if (oldGrid != null)
            {
                int copyWidth = Mathf.Min(oldGrid.GetLength(0), expectedWidth);
                int copyHeight = Mathf.Min(oldGrid.GetLength(1), expectedHeight);
                for (int y = 0; y < copyHeight; y++)
                {
                    for (int x = 0; x < copyWidth; x++)
                    {
                        newGrid[x, y] = oldGrid[x, y];
                    }
                }
            }
            else if (layer.flatVertexData != null)
            {
                UnflattenGridInto(layer.flatVertexData, newGrid, width, height);
            }

            layer.vertGrid = newGrid;
            StoreLayerGrid(layer);
        }

        private void BuildLayerList()
        {
            layerList = new ReorderableList(layers, typeof(MapLayer), true, true, true, true);
            layerList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Layers");
            layerList.elementHeightCallback = _ => EditorGUIUtility.singleLineHeight * 4f + 14f;
            layerList.onSelectCallback = list =>
            {
                activeLayerIndex = Mathf.Clamp(list.index, 0, layers.Count - 1);
                Repaint();
            };
            layerList.onCanAddCallback = _ => layers.Count < MaxLayers;
            layerList.onAddCallback = _ =>
            {
                if (layers.Count >= MaxLayers)
                {
                    return;
                }

                var layer = new MapLayer { name = layers.Count == 0 ? "Base" : "Layer " + (layers.Count + 1) };
                EnsureLayerGrid(layer, roomWidth, roomHeight, false);
                layers.Add(layer);
                activeLayerIndex = layers.Count - 1;
                layerList.index = activeLayerIndex;
            };
            layerList.onRemoveCallback = list =>
            {
                if (layers.Count <= 1)
                {
                    return;
                }

                layers.RemoveAt(Mathf.Clamp(list.index, 0, layers.Count - 1));
                activeLayerIndex = Mathf.Clamp(activeLayerIndex, 0, layers.Count - 1);
                list.index = activeLayerIndex;
            };
            layerList.drawElementCallback = DrawLayerElement;
        }

        private void DrawLayerElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index >= layers.Count)
            {
                return;
            }

            if (isActive)
            {
                EditorGUI.DrawRect(new Rect(rect.x, rect.y + 1f, rect.width, rect.height - 2f), new Color(0.18f, 0.42f, 0.56f, 0.22f));
            }

            MapLayer layer = layers[index];
            float line = EditorGUIUtility.singleLineHeight;
            float y = rect.y + 4f;

            EditorGUI.LabelField(new Rect(rect.x, y, 54f, line), "Name");
            layer.name = EditorGUI.TextField(new Rect(rect.x + 58f, y, rect.width - 58f, line), layer.name);

            y += line + 3f;
            EditorGUI.LabelField(new Rect(rect.x, y, 54f, line), "Tilemap");
            layer.tilemap = (Tilemap)EditorGUI.ObjectField(new Rect(rect.x + 58f, y, rect.width - 58f, line), layer.tilemap, typeof(Tilemap), true);

            y += line + 3f;
            EditorGUI.LabelField(new Rect(rect.x, y, 54f, line), "Tileset");
            layer.tileSet = (CornerWangTileSetSO)EditorGUI.ObjectField(new Rect(rect.x + 58f, y, rect.width - 58f, line), layer.tileSet, typeof(CornerWangTileSetSO), false);

            y += line + 3f;
            layer.enabled = EditorGUI.ToggleLeft(new Rect(rect.x + 58f, y, rect.width - 58f, line), "Enabled", layer.enabled);
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(ToolbarHeight));

            if (GUILayout.Button("New", EditorStyles.toolbarButton, GUILayout.Width(48f)) &&
                EditorUtility.DisplayDialog("New Map", "Reset to an empty 16x12 floor map?", "New", "Cancel"))
            {
                ResetMap();
            }

            if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(48f)))
            {
                SaveMap();
            }

            if (GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(48f)))
            {
                LoadMap();
            }

            if (GUILayout.Button("Apply to Scene", EditorStyles.toolbarButton, GUILayout.Width(104f)))
            {
                ApplyToScene();
            }

            if (GUILayout.Button("Generate Room", EditorStyles.toolbarButton, GUILayout.Width(110f)))
            {
                RoomGeneratorWindow.Open(this);
            }

            if (GUILayout.Button("Clear All", EditorStyles.toolbarButton, GUILayout.Width(72f)))
            {
                ClearAllTilemaps();
            }

            GUILayout.Space(8f);
            paintMode = (PaintMode)GUILayout.Toolbar((int)paintMode, new[] { "Cell", "Vertex" }, EditorStyles.toolbarButton, GUILayout.Width(112f));
            eraseMode = GUILayout.Toggle(eraseMode, "Erase", EditorStyles.toolbarButton, GUILayout.Width(54f));

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Fit", EditorStyles.toolbarButton, GUILayout.Width(40f)))
            {
                FitCanvasToWindow();
            }

            GUILayout.Label("Cell", GUILayout.Width(28f));
            cellSize = GUILayout.HorizontalSlider(cellSize, 10f, 80f, GUILayout.Width(120f));
            GUILayout.Label(Mathf.RoundToInt(cellSize) + "px", GUILayout.Width(34f));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(LeftPanelWidth), GUILayout.ExpandHeight(true));
            leftScroll = EditorGUILayout.BeginScrollView(leftScroll);

            EditorGUI.BeginChangeCheck();
            activeBiome = (RimaBiomePreset)EditorGUILayout.ObjectField("Biome", activeBiome, typeof(RimaBiomePreset), false);
            if (EditorGUI.EndChangeCheck())
            {
                EnsureValidActiveTerrain();
                Repaint();
            }

            if (activeBiome == null)
            {
                EditorGUILayout.HelpBox("Assign a Biome preset.", MessageType.Info);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Terrains", EditorStyles.boldLabel);
            if (activeBiome.terrains == null || activeBiome.terrains.Count == 0)
            {
                EditorGUILayout.HelpBox("Biome has no terrains.", MessageType.Warning);
            }
            else
            {
                foreach (MapTerrain terrain in activeBiome.terrains)
                {
                    if (terrain == null || terrain.id == 0)
                    {
                        continue;
                    }

                    bool isActive = activeTerrainId == terrain.id;
                    Color prevBg = GUI.backgroundColor;
                    GUI.backgroundColor = isActive ? terrain.paletteColor : Color.Lerp(prevBg, terrain.paletteColor, 0.25f);

                    string label = string.IsNullOrEmpty(terrain.name) ? "Terrain " + terrain.id : terrain.name;
                    if (GUILayout.Button(label, GUILayout.Height(40f)))
                    {
                        activeTerrainId = terrain.id;
                        currentPaintValue = activeTerrainId;
                        eraseMode = false;
                    }

                    GUI.backgroundColor = prevBg;
                }
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
            MapLayer activeLayer = GetActiveLayer();
            if (activeLayer != null)
            {
                activeLayer.tilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", activeLayer.tilemap, typeof(Tilemap), true);
                activeLayer.enabled = EditorGUILayout.Toggle("Enabled", activeLayer.enabled);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawCenterPanel(float height)
        {
            Rect centerRect = GUILayoutUtility.GetRect(0f, height, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            float canvasWidth = (roomWidth * cellSize) + CanvasPadding * 2f;
            float canvasHeight = (roomHeight * cellSize) + CanvasPadding * 2f;
            Rect viewRect = new Rect(0f, 0f, canvasWidth, canvasHeight);

            gridScroll = GUI.BeginScrollView(centerRect, gridScroll, viewRect, true, true);
            DrawGridCanvas(viewRect);
            HandleGridInput(viewRect);
            GUI.EndScrollView();
        }

        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(RightPanelWidth), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField("Paint", EditorStyles.boldLabel);

            GUIStyle bigButton = new GUIStyle(GUI.skin.button) { fixedHeight = 40f, fontSize = 12, fontStyle = FontStyle.Bold };

            Color prevBg = GUI.backgroundColor;
            GUI.backgroundColor = !eraseMode ? new Color(0.3f, 0.7f, 0.4f) : prevBg;
            if (GUILayout.Button("PAINT", bigButton))
            {
                eraseMode = false;
                currentPaintValue = activeTerrainId;
            }

            GUI.backgroundColor = eraseMode ? new Color(0.9f, 0.3f, 0.3f) : prevBg;
            if (GUILayout.Button("ERASE", bigButton))
            {
                eraseMode = true;
            }

            GUI.backgroundColor = prevBg;

            EditorGUILayout.Space(6f);

            brushRadius = EditorGUILayout.IntSlider("Brush", brushRadius, 1, 5);

            EditorGUILayout.Space(6f);

            paintMode = (PaintMode)GUILayout.Toolbar((int)paintMode, new[] { "Cell", "Vertex" });

            EditorGUILayout.Space(8f);

            proceduralFoldout = EditorGUILayout.Foldout(proceduralFoldout, "Advanced", true);
            if (proceduralFoldout)
            {
                EditorGUILayout.LabelField("Procedural Helpers", EditorStyles.miniBoldLabel);
                if (GUILayout.Button("Make Rectangular Room"))
                {
                    MakeRectangularRoom();
                }

                if (GUILayout.Button("L-Shape Room"))
                {
                    MakeLShapeRoom();
                }

                if (GUILayout.Button("Perlin Noise Fill"))
                {
                    PerlinNoiseFill();
                }

                EditorGUILayout.Space(4f);
                EditorGUILayout.LabelField("Tool", EditorStyles.miniBoldLabel);
                activeTool = (PaintTool)GUILayout.Toolbar((int)activeTool, new[] { "Brush", "Fill", "Rect" });

                EditorGUILayout.Space(4f);
                EditorGUILayout.LabelField("Room Size", EditorStyles.miniBoldLabel);
                int newW = EditorGUILayout.IntField("W", roomWidth);
                int newH = EditorGUILayout.IntField("H", roomHeight);
                newW = Mathf.Clamp(newW, MinRoomSize, MaxRoomSize);
                newH = Mathf.Clamp(newH, MinRoomSize, MaxRoomSize);
                if (newW != roomWidth || newH != roomHeight)
                {
                    roomWidth = newW;
                    roomHeight = newH;
                }

                if (GUILayout.Button("Resize"))
                {
                    ResizeGrid(roomWidth, roomHeight, true);
                }

                EditorGUILayout.Space(4f);
                showTilePreview = EditorGUILayout.Toggle("Show Tiles on Canvas", showTilePreview);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawGridCanvas(Rect viewRect)
        {
            EditorGUI.DrawRect(viewRect, new Color(0.12f, 0.12f, 0.12f, 1f));
            int[,] grid = GetActiveGrid();

            if (showTilePreview)
            {
                DrawLiveTilePreviewCells(grid);
            }

            Handles.BeginGUI();
            Handles.color = new Color(0.333f, 0.333f, 0.333f, 0.5f);

            for (int y = 0; y <= roomHeight; y++)
            {
                Handles.DrawLine(VertexToCanvasPosition(0, y), VertexToCanvasPosition(roomWidth, y));
            }

            for (int x = 0; x <= roomWidth; x++)
            {
                Handles.DrawLine(VertexToCanvasPosition(x, 0), VertexToCanvasPosition(x, roomHeight));
            }

            if (grid != null)
            {
                for (int y = 0; y <= roomHeight; y++)
                {
                    for (int x = 0; x <= roomWidth; x++)
                    {
                        Handles.color = GetTerrainColor(grid[x, y]);
                        Handles.DrawSolidDisc(VertexToCanvasPosition(x, y), Vector3.forward, VertexRadius);
                    }
                }
            }

            if (paintMode == PaintMode.Vertex && IsValidVertex(hoveredVertex))
            {
                Handles.color = eraseMode ? new Color(1f, 0.3f, 0.3f, 0.6f) : new Color(0f, 1f, 1f, 0.6f);
                Handles.DrawWireDisc(VertexToCanvasPosition(hoveredVertex.x, hoveredVertex.y), Vector3.forward, VertexRadius + 2f);
            }

            if (paintMode == PaintMode.Cell && brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight))
            {
                DrawCellHover(hoveredCell);
            }

            if (isRectangleDragging)
            {
                DrawRectangleOverlay(rectStart, rectCurrent);
            }

            Handles.EndGUI();
        }

        private void DrawCellHover(Vector2Int cell)
        {
            Rect rect = CellToCanvasRect(cell);
            Color fill = eraseMode ? new Color(1f, 0.2f, 0.2f, 0.4f) : new Color(0.2f, 1f, 0.4f, 0.4f);
            Color border = eraseMode ? Color.red : Color.green;
            EditorGUI.DrawRect(rect, fill);
            Handles.color = border;
            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, border);
        }

        private void DrawLiveTilePreviewCells(int[,] grid)
        {
            if (grid == null || activeBiome == null)
            {
                return;
            }

            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    int nw = grid[x, y + 1];
                    int ne = grid[x + 1, y + 1];
                    int sw = grid[x, y];
                    int se = grid[x + 1, y];
                    var unique = new HashSet<int> { nw, ne, sw, se };
                    TileBase tile = CornerWangPainter.ResolveTile(activeBiome, nw, ne, sw, se);
                    Sprite sprite = (tile as Tile)?.sprite;
                    Rect cellRect = CellToCanvasRect(new Vector2Int(x, y));

                    if (sprite != null && sprite.texture != null)
                    {
                        // Sprite slice rect -> normalized texCoords (avoid drawing whole spritesheet)
                        Rect tc = new Rect(
                            sprite.rect.x / sprite.texture.width,
                            sprite.rect.y / sprite.texture.height,
                            sprite.rect.width / sprite.texture.width,
                            sprite.rect.height / sprite.texture.height);
                        GUI.DrawTextureWithTexCoords(cellRect, sprite.texture, tc);
                    }
                    else if (unique.Count >= 3)
                    {
                        EditorGUI.DrawRect(cellRect, new Color(1f, 0f, 0f, 0.5f));
                        Handles.BeginGUI();
                        Handles.color = Color.red;
                        Handles.DrawLine(new Vector3(cellRect.x, cellRect.y), new Vector3(cellRect.xMax, cellRect.yMax));
                        Handles.DrawLine(new Vector3(cellRect.xMax, cellRect.y), new Vector3(cellRect.x, cellRect.yMax));
                        Handles.EndGUI();
                    }
                    else if (unique.Count == 2)
                    {
                        EditorGUI.DrawRect(cellRect, new Color(1f, 0.7f, 0f, 0.4f));
                    }
                    else
                    {
                        EditorGUI.DrawRect(cellRect, GetTerrainColor(nw) * new Color(1f, 1f, 1f, 0.55f));
                    }
                }
            }
        }

        private Texture GetCanvasTileTexture(TileBase tile)
        {
            Texture tex = (tile as Tile)?.sprite?.texture;
            return tex != null ? tex : tile != null ? AssetPreview.GetAssetPreview(tile) : null;
        }

        private static Color GetLiveTileFallbackColor(int wangKey, int filledCorners)
        {
            Color floorColor = new Color(0.20f, 0.20f, 0.20f);
            Color wallColor = new Color(0.45f, 0.28f, 0.16f);
            if (wangKey == 0)
            {
                return floorColor;
            }

            if (wangKey == 15)
            {
                return wallColor;
            }

            return Color.Lerp(floorColor, wallColor, filledCorners / 4f);
        }

        private void HandleGridInput(Rect viewRect)
        {
            Event evt = Event.current;
            Vector2 mouse = evt.mousePosition;
            bool inCanvas = viewRect.Contains(mouse);

            if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Space)
            {
                spaceHeld = true;
                evt.Use();
                return;
            }

            if (evt.type == EventType.KeyUp && evt.keyCode == KeyCode.Space)
            {
                spaceHeld = false;
                evt.Use();
                return;
            }

            if (evt.type == EventType.KeyDown)
            {
                if (evt.keyCode == KeyCode.Plus || evt.keyCode == KeyCode.KeypadPlus || evt.character == '+')
                {
                    cellSize = Mathf.Clamp(cellSize + 4f, 10f, 80f);
                    Repaint();
                    evt.Use();
                    return;
                }

                if (evt.keyCode == KeyCode.Minus || evt.keyCode == KeyCode.KeypadMinus || evt.character == '-')
                {
                    cellSize = Mathf.Clamp(cellSize - 4f, 10f, 80f);
                    Repaint();
                    evt.Use();
                    return;
                }
            }

            if (evt.type == EventType.ScrollWheel && inCanvas)
            {
                cellSize = Mathf.Clamp(cellSize - evt.delta.y * 2f, 10f, 80f);
                evt.Use();
                Repaint();
                return;
            }

            hoveredCell = brushInput.GetCellAtMouse(mouse, cellSize, CanvasPadding, roomHeight);
            hoveredVertex = GetNearestVertex(mouse);

            if (evt.type == EventType.MouseMove && inCanvas)
            {
                Repaint();
            }

            if (evt.type == EventType.MouseDown && evt.button == 2)
            {
                isPanning = true;
                panStartMouse = mouse;
                panStartScroll = gridScroll;
                evt.Use();
                return;
            }

            if (evt.type == EventType.MouseDrag && isPanning && evt.button == 2)
            {
                gridScroll = panStartScroll - (mouse - panStartMouse);
                evt.Use();
                Repaint();
                return;
            }

            if (spaceHeld && evt.type == EventType.MouseDrag && evt.button == 0)
            {
                gridScroll -= evt.delta;
                evt.Use();
                Repaint();
                return;
            }

            if (evt.type == EventType.MouseUp && evt.button == 2)
            {
                isPanning = false;
                lastPaintedCell = new Vector2Int(-1, -1);
                evt.Use();
                return;
            }

            if (!inCanvas)
            {
                return;
            }

            bool hasPaintTarget = paintMode == PaintMode.Cell
                ? brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight)
                : IsValidVertex(hoveredVertex);
            if (!hasPaintTarget)
            {
                if (evt.type == EventType.MouseMove)
                {
                    Repaint();
                }

                return;
            }

            if (evt.type == EventType.MouseDown && (evt.button == 0 || evt.button == 1))
            {
                int value = GetActualPaintValue(evt.button == 1);
                Vector2Int start = paintMode == PaintMode.Cell ? hoveredCell : hoveredVertex;

                if (activeTool == PaintTool.Rectangle)
                {
                    rectStart = start;
                    rectCurrent = start;
                    isRectangleDragging = true;
                }
                else if (activeTool == PaintTool.Fill)
                {
                    FloodFill(start, value);
                }
                else
                {
                    PaintWithRadius(start, value);
                    lastPaintedCell = paintMode == PaintMode.Cell ? hoveredCell : new Vector2Int(-1, -1);
                    isPainting = true;
                }

                evt.Use();
                Repaint();
            }
            else if (evt.type == EventType.MouseDrag && (evt.button == 0 || evt.button == 1))
            {
                int value = GetActualPaintValue(evt.button == 1);
                Vector2Int current = paintMode == PaintMode.Cell ? hoveredCell : hoveredVertex;

                if (activeTool == PaintTool.Rectangle && isRectangleDragging)
                {
                    rectCurrent = current;
                }
                else if (activeTool == PaintTool.Brush && isPainting)
                {
                    if (paintMode != PaintMode.Cell || current != lastPaintedCell)
                    {
                        PaintWithRadius(current, value);
                        lastPaintedCell = paintMode == PaintMode.Cell ? current : new Vector2Int(-1, -1);
                    }
                }

                evt.Use();
                Repaint();
            }
            else if (evt.type == EventType.MouseUp && (evt.button == 0 || evt.button == 1))
            {
                int value = GetActualPaintValue(evt.button == 1);
                Vector2Int current = paintMode == PaintMode.Cell ? hoveredCell : hoveredVertex;

                if (activeTool == PaintTool.Rectangle && isRectangleDragging)
                {
                    PaintRectangle(rectStart, current, value);
                    isRectangleDragging = false;
                    rectStart = new Vector2Int(-1, -1);
                    rectCurrent = new Vector2Int(-1, -1);
                }

                isPainting = false;
                lastPaintedCell = new Vector2Int(-1, -1);
                evt.Use();
                Repaint();
            }
        }

        private int GetActualPaintValue(bool invertForButton)
        {
            int value = eraseMode || invertForButton ? 0 : activeTerrainId;
            currentPaintValue = value;
            return value;
        }

        private Vector2 VertexToCanvasPosition(int x, int y)
        {
            return new Vector2(CanvasPadding + x * cellSize, CanvasPadding + (roomHeight - y) * cellSize);
        }

        private Rect CellToCanvasRect(Vector2Int cell)
        {
            return new Rect(
                CanvasPadding + cell.x * cellSize,
                CanvasPadding + (roomHeight - cell.y - 1) * cellSize,
                cellSize,
                cellSize);
        }

        private Vector2Int GetNearestVertex(Vector2 mousePosition)
        {
            int x = Mathf.RoundToInt((mousePosition.x - CanvasPadding) / cellSize);
            int invertedY = Mathf.RoundToInt((mousePosition.y - CanvasPadding) / cellSize);
            int y = roomHeight - invertedY;
            Vector2 vertexPos = VertexToCanvasPosition(x, y);
            if (Vector2.Distance(mousePosition, vertexPos) > cellSize * 0.45f)
            {
                return new Vector2Int(-1, -1);
            }

            return new Vector2Int(x, y);
        }

        private bool IsValidVertex(Vector2Int vertex)
        {
            return vertex.x >= 0 && vertex.y >= 0 && vertex.x <= roomWidth && vertex.y <= roomHeight;
        }

        private MapLayer GetActiveLayer()
        {
            if (layers == null || layers.Count == 0)
            {
                return null;
            }

            return layers[Mathf.Clamp(activeLayerIndex, 0, layers.Count - 1)];
        }

        private int[,] GetActiveGrid()
        {
            EnsureTerrainGrid(roomWidth, roomHeight, false);
            return terrainGrid;
        }

        private void PaintVertex(Vector2Int vertex, int value)
        {
            int[,] grid = GetActiveGrid();
            if (grid == null || !IsValidVertex(vertex))
            {
                return;
            }

            grid[vertex.x, vertex.y] = Mathf.Max(0, value);
            StoreTerrainGrid();
        }

        private void PaintCell(Vector2Int cellOrigin, int value)
        {
            if (!brushInput.IsValidCell(cellOrigin, roomWidth, roomHeight))
            {
                return;
            }

            PaintVertex(new Vector2Int(cellOrigin.x, cellOrigin.y), value);
            PaintVertex(new Vector2Int(cellOrigin.x + 1, cellOrigin.y), value);
            PaintVertex(new Vector2Int(cellOrigin.x, cellOrigin.y + 1), value);
            PaintVertex(new Vector2Int(cellOrigin.x + 1, cellOrigin.y + 1), value);
        }

        private void PaintWithRadius(Vector2Int center, int value)
        {
            int r = brushRadius - 1;
            for (int dy = -r; dy <= r; dy++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    Vector2Int p = new Vector2Int(center.x + dx, center.y + dy);
                    if (paintMode == PaintMode.Cell)
                    {
                        PaintCell(p, value);
                    }
                    else
                    {
                        PaintVertex(p, value);
                    }
                }
            }
        }

        private void PaintRectangle(Vector2Int start, Vector2Int end, int value)
        {
            int minX = Mathf.Min(start.x, end.x);
            int maxX = Mathf.Max(start.x, end.x);
            int minY = Mathf.Min(start.y, end.y);
            int maxY = Mathf.Max(start.y, end.y);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (paintMode == PaintMode.Cell)
                    {
                        PaintCell(new Vector2Int(x, y), value);
                    }
                    else
                    {
                        PaintVertex(new Vector2Int(x, y), value);
                    }
                }
            }
        }

        private void FloodFill(Vector2Int start, int value)
        {
            if (paintMode == PaintMode.Cell)
            {
                PaintCell(start, value);
                return;
            }

            int[,] grid = GetActiveGrid();
            if (grid == null || !IsValidVertex(start))
            {
                return;
            }

            int target = grid[start.x, start.y];
            value = Mathf.Max(0, value);
            if (target == value)
            {
                return;
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            grid[start.x, start.y] = value;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                TryFloodNeighbor(queue, current.x + 1, current.y, target, value);
                TryFloodNeighbor(queue, current.x - 1, current.y, target, value);
                TryFloodNeighbor(queue, current.x, current.y + 1, target, value);
                TryFloodNeighbor(queue, current.x, current.y - 1, target, value);
            }

            StoreTerrainGrid();
        }

        private void TryFloodNeighbor(Queue<Vector2Int> queue, int x, int y, int target, int value)
        {
            int[,] grid = GetActiveGrid();
            if (grid == null || x < 0 || y < 0 || x > roomWidth || y > roomHeight || grid[x, y] != target)
            {
                return;
            }

            grid[x, y] = value;
            queue.Enqueue(new Vector2Int(x, y));
        }

        private void DrawRectangleOverlay(Vector2Int start, Vector2Int end)
        {
            if (start.x < 0 || end.x < 0)
            {
                return;
            }

            if (paintMode == PaintMode.Cell)
            {
                Rect a = CellToCanvasRect(new Vector2Int(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y)));
                Rect b = CellToCanvasRect(new Vector2Int(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y)));
                Rect rect = Rect.MinMaxRect(a.xMin, b.yMin, b.xMax, a.yMax);
                Handles.DrawSolidRectangleWithOutline(rect, new Color(0f, 1f, 1f, 0.12f), Color.cyan);
                return;
            }

            Vector2 va = VertexToCanvasPosition(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
            Vector2 vb = VertexToCanvasPosition(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y));
            Rect vertexRect = Rect.MinMaxRect(Mathf.Min(va.x, vb.x), Mathf.Min(va.y, vb.y), Mathf.Max(va.x, vb.x), Mathf.Max(va.y, vb.y));
            Handles.DrawSolidRectangleWithOutline(vertexRect, new Color(0f, 1f, 1f, 0.12f), Color.cyan);
        }

        private static Color HexColor(byte r, byte g, byte b)
        {
            return new Color(r / 255f, g / 255f, b / 255f, 1f);
        }

        private Color GetTerrainColor(int terrainId)
        {
            MapTerrain terrain = activeBiome != null && activeBiome.terrains != null
                ? activeBiome.terrains.FirstOrDefault(t => t != null && t.id == terrainId)
                : null;

            if (terrain != null)
            {
                return terrain.paletteColor;
            }

            return terrainId == 0 ? HexColor(0x3A, 0x3A, 0x3A) : HexColor(0x7A, 0x4A, 0x2A);
        }

        private string GetTerrainName(int terrainId)
        {
            MapTerrain terrain = activeBiome != null && activeBiome.terrains != null
                ? activeBiome.terrains.FirstOrDefault(t => t != null && t.id == terrainId)
                : null;

            return terrain != null && !string.IsNullOrEmpty(terrain.name) ? terrain.name : "Terrain " + terrainId;
        }

        private void FitCanvasToWindow()
        {
            float availW = position.width - LeftPanelWidth - RightPanelWidth - CanvasPadding * 2f;
            float availH = position.height - ToolbarHeight - StatusHeight - CanvasPadding * 2f;
            cellSize = Mathf.Floor(Mathf.Min(availW / Mathf.Max(1, roomWidth), availH / Mathf.Max(1, roomHeight)));
            cellSize = Mathf.Clamp(cellSize, 10f, 80f);
            gridScroll = Vector2.zero;
            Repaint();
        }

        private void ResetMap()
        {
            roomWidth = DefaultRoomWidth;
            roomHeight = DefaultRoomHeight;
            ResizeGrid(roomWidth, roomHeight, false);
            FillAll(0);
        }

        private void ResizeGrid(int newWidth, int newHeight, bool preserve)
        {
            newWidth = Mathf.Clamp(newWidth, MinRoomSize, MaxRoomSize);
            newHeight = Mathf.Clamp(newHeight, MinRoomSize, MaxRoomSize);
            StoreAllLayerGrids();
            StoreTerrainGrid();
            roomWidth = newWidth;
            roomHeight = newHeight;

            foreach (MapLayer layer in layers)
            {
                EnsureLayerGrid(layer, roomWidth, roomHeight, preserve);
            }

            EnsureTerrainGrid(roomWidth, roomHeight, preserve);
            Repaint();
        }

        private void FillAll(int value)
        {
            int[,] grid = GetActiveGrid();
            if (grid == null)
            {
                return;
            }

            value = Mathf.Max(0, value);
            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    grid[x, y] = value;
                }
            }

            StoreTerrainGrid();
            Repaint();
        }

        private void MakeRectangularRoom()
        {
            int[,] grid = GetActiveGrid();
            if (grid == null)
            {
                return;
            }

            wallThickness = Mathf.Clamp(wallThickness, 1, Mathf.Min(roomWidth, roomHeight) / 2);
            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    bool isWall = x < wallThickness || y < wallThickness || x > roomWidth - wallThickness || y > roomHeight - wallThickness;
                    grid[x, y] = isWall ? 1 : 0;
                }
            }

            StoreTerrainGrid();
            Repaint();
        }

        private void MakeLShapeRoom()
        {
            FillAll(1);
            int[,] grid = GetActiveGrid();
            if (grid == null)
            {
                return;
            }

            int splitX = Mathf.Max(wallThickness + 2, Mathf.RoundToInt(roomWidth * 0.58f));
            int splitY = Mathf.Max(wallThickness + 2, Mathf.RoundToInt(roomHeight * 0.52f));
            for (int y = wallThickness; y <= roomHeight - wallThickness; y++)
            {
                for (int x = wallThickness; x <= roomWidth - wallThickness; x++)
                {
                    if (y <= splitY || x <= splitX)
                    {
                        grid[x, y] = 0;
                    }
                }
            }

            StoreTerrainGrid();
            Repaint();
        }

        private void PerlinNoiseFill()
        {
            int[,] grid = GetActiveGrid();
            if (grid == null)
            {
                return;
            }

            float offsetX = noiseSeed * 0.173f;
            float offsetY = noiseSeed * 0.317f;
            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    float sample = Mathf.PerlinNoise(offsetX + x * 0.18f, offsetY + y * 0.18f);
                    grid[x, y] = sample < noiseDensity ? 1 : 0;
                }
            }

            StoreTerrainGrid();
            Repaint();
        }

        private void PaintHorizontalCorridor()
        {
            int[,] grid = GetActiveGrid();
            if (grid == null)
            {
                return;
            }

            int centerY = roomHeight / 2;
            int half = Mathf.Max(1, wallThickness / 2);
            for (int y = Mathf.Max(0, centerY - half); y <= Mathf.Min(roomHeight, centerY + half); y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    grid[x, y] = 0;
                }
            }

            StoreTerrainGrid();
            Repaint();
        }

        private void PaintVerticalCorridor()
        {
            int[,] grid = GetActiveGrid();
            if (grid == null)
            {
                return;
            }

            int centerX = roomWidth / 2;
            int half = Mathf.Max(1, wallThickness / 2);
            for (int x = Mathf.Max(0, centerX - half); x <= Mathf.Min(roomWidth, centerX + half); x++)
            {
                for (int y = 0; y <= roomHeight; y++)
                {
                    grid[x, y] = 0;
                }
            }

            StoreTerrainGrid();
            Repaint();
        }

        private void SaveMap()
        {
            Directory.CreateDirectory(MapDataFolder);
            string path = EditorUtility.SaveFilePanelInProject("Save RIMA Map", "RimaMap", "json", "Save map data", MapDataFolder);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            StoreAllLayerGrids();
            StoreTerrainGrid();
            MapSaveData data = new MapSaveData
            {
                width = roomWidth,
                height = roomHeight,
                terrainGrid = FlattenGrid(terrainGrid),
                biomePresetGuid = activeBiome != null ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(activeBiome)) : string.Empty,
                layers = layers.Select(layer => new LayerSaveData
                {
                    name = layer.name,
                    tileSet = layer.tileSet != null ? AssetDatabase.GetAssetPath(layer.tileSet) : string.Empty,
                    enabled = layer.enabled,
                    vertexData = FlattenGrid(layer.vertGrid)
                }).ToArray()
            };

            File.WriteAllText(path, JsonUtility.ToJson(data, true));
            AssetDatabase.Refresh();
            Debug.Log("[MapDesigner] Saved map data: " + path);
        }

        private void LoadMap()
        {
            string absolutePath = EditorUtility.OpenFilePanel("Load RIMA Map", MapDataFolder, "json");
            if (string.IsNullOrEmpty(absolutePath))
            {
                return;
            }

            string json = File.ReadAllText(absolutePath);
            MapSaveData data = JsonUtility.FromJson<MapSaveData>(json);
            if (data == null)
            {
                Debug.LogError("[MapDesigner] Invalid map data: " + absolutePath);
                return;
            }

            roomWidth = Mathf.Clamp(data.width, MinRoomSize, MaxRoomSize);
            roomHeight = Mathf.Clamp(data.height, MinRoomSize, MaxRoomSize);

            if (!string.IsNullOrEmpty(data.biomePresetGuid))
            {
                string biomePath = AssetDatabase.GUIDToAssetPath(data.biomePresetGuid);
                RimaBiomePreset loadedBiome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>(biomePath);
                if (loadedBiome != null)
                {
                    activeBiome = loadedBiome;
                }
            }

            flatTerrainData = data.terrainGrid != null && data.terrainGrid.Length > 0
                ? data.terrainGrid
                : ConvertLegacyLayersToTerrainGrid(data);
            terrainGrid = null;
            EnsureTerrainGrid(roomWidth, roomHeight, false);

            if (data.layers != null && data.layers.Length > 0)
            {
                layers = data.layers.Select(saved => new MapLayer
                {
                    name = string.IsNullOrEmpty(saved.name) ? "Layer" : saved.name,
                    tileSet = string.IsNullOrEmpty(saved.tileSet) ? null : AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(saved.tileSet),
                    enabled = saved.enabled,
                    flatVertexData = saved.vertexData
                }).ToList();
            }
            else
            {
                layers = new List<MapLayer> { new MapLayer { name = "Base", flatVertexData = data.vertexData } };
                if (data.layerNames != null)
                {
                    for (int i = 0; i < Mathf.Min(data.layerNames.Length, layers.Count); i++)
                    {
                        layers[i].name = data.layerNames[i];
                    }
                }
            }

            if (layers.Count == 0)
            {
                layers.Add(new MapLayer { name = "Base" });
            }

            foreach (MapLayer layer in layers)
            {
                EnsureLayerGrid(layer, roomWidth, roomHeight, false);
            }

            activeLayerIndex = Mathf.Clamp(activeLayerIndex, 0, layers.Count - 1);
            EnsureValidActiveTerrain();
            BuildLayerList();
            Debug.Log("[MapDesigner] Loaded map data: " + absolutePath);
            Repaint();
        }

        public void LoadFromGenerator(MapSaveData generated)
        {
            if (generated == null)
            {
                Debug.LogError("[MapDesigner] Generator returned no map data.");
                return;
            }

            Tilemap[] existingTilemaps = layers != null ? layers.Select(layer => layer != null ? layer.tilemap : null).ToArray() : Array.Empty<Tilemap>();
            roomWidth = Mathf.Clamp(generated.width, MinRoomSize, MaxRoomSize);
            roomHeight = Mathf.Clamp(generated.height, MinRoomSize, MaxRoomSize);
            if (!string.IsNullOrEmpty(generated.biomePresetGuid))
            {
                string biomePath = AssetDatabase.GUIDToAssetPath(generated.biomePresetGuid);
                RimaBiomePreset generatedBiome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>(biomePath);
                if (generatedBiome != null)
                {
                    activeBiome = generatedBiome;
                }
            }

            flatTerrainData = generated.terrainGrid != null && generated.terrainGrid.Length > 0
                ? generated.terrainGrid
                : ConvertLegacyLayersToTerrainGrid(generated);
            terrainGrid = null;
            EnsureTerrainGrid(roomWidth, roomHeight, false);

            layers = new List<MapLayer>();

            if (generated.layers != null)
            {
                for (int i = 0; i < generated.layers.Length; i++)
                {
                    LayerSaveData layerData = generated.layers[i];
                    var layer = new MapLayer
                    {
                        name = string.IsNullOrEmpty(layerData.name) ? "Layer " + (i + 1) : layerData.name,
                        enabled = layerData.enabled,
                        tileSet = FindTilesetByName(layerData.tileSet),
                        tilemap = i < existingTilemaps.Length ? existingTilemaps[i] : null,
                        flatVertexData = layerData.vertexData
                    };

                    EnsureLayerGrid(layer, roomWidth, roomHeight, false);
                    layers.Add(layer);
                }
            }

            if (layers.Count == 0)
            {
                layers.Add(new MapLayer { name = "Output", tilemap = existingTilemaps.Length > 0 ? existingTilemaps[0] : null });
                EnsureLayerGrid(layers[0], roomWidth, roomHeight, false);
            }

            activeLayerIndex = Mathf.Clamp(activeLayerIndex, 0, layers.Count - 1);
            EnsureValidActiveTerrain();
            BuildLayerList();
            Repaint();
            Debug.Log("[MapDesigner] Loaded generated room " + roomWidth + "x" + roomHeight + " with multi-terrain grid.");
        }

        private static CornerWangTileSetSO FindTilesetByName(string tileSetNameOrPath)
        {
            if (string.IsNullOrEmpty(tileSetNameOrPath))
            {
                return null;
            }

            var byPath = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(tileSetNameOrPath);
            if (byPath != null)
            {
                return byPath;
            }

            string expectedName = Path.GetFileNameWithoutExtension(tileSetNameOrPath);
            foreach (string guid in AssetDatabase.FindAssets("t:CornerWangTileSetSO"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var candidate = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(path);
                if (candidate != null &&
                    (string.Equals(candidate.name, tileSetNameOrPath, StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(candidate.name, expectedName, StringComparison.OrdinalIgnoreCase) ||
                     path.IndexOf(tileSetNameOrPath, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    return candidate;
                }
            }

            Debug.LogWarning("[MapDesigner] Tileset not found for generated layer: " + tileSetNameOrPath);
            return null;
        }

        private static int[] ConvertLegacyLayersToTerrainGrid(MapSaveData data)
        {
            if (data == null)
            {
                return Array.Empty<int>();
            }

            int width = Mathf.Clamp(data.width, MinRoomSize, MaxRoomSize);
            int height = Mathf.Clamp(data.height, MinRoomSize, MaxRoomSize);
            int[] result = new int[(width + 1) * (height + 1)];

            if (data.layers != null && data.layers.Length > 0)
            {
                for (int layerIndex = 0; layerIndex < data.layers.Length; layerIndex++)
                {
                    LayerSaveData layer = data.layers[layerIndex];
                    if (layer == null || !layer.enabled || layer.vertexData == null)
                    {
                        continue;
                    }

                    int terrainId = LegacyLayerTerrainId(layer, layerIndex);
                    for (int i = 0; i < result.Length && i < layer.vertexData.Length; i++)
                    {
                        if (layer.vertexData[i] != 0)
                        {
                            result[i] = terrainId;
                        }
                    }
                }

                return result;
            }

            if (data.vertexData != null)
            {
                for (int i = 0; i < result.Length && i < data.vertexData.Length; i++)
                {
                    result[i] = data.vertexData[i] != 0 ? 1 : 0;
                }
            }

            return result;
        }

        private static int LegacyLayerTerrainId(LayerSaveData layer, int layerIndex)
        {
            string key = ((layer.name ?? string.Empty) + " " + (layer.tileSet ?? string.Empty)).ToLowerInvariant();
            if (key.Contains("rift"))
            {
                return 3;
            }

            if (key.Contains("path") || key.Contains("rubble"))
            {
                return 2;
            }

            return layerIndex == 0 ? 1 : layerIndex + 1;
        }

        private int[] FlattenGrid(int[,] grid)
        {
            int[] values = new int[(roomWidth + 1) * (roomHeight + 1)];
            if (grid == null)
            {
                return values;
            }

            int index = 0;
            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    values[index++] = grid[x, y];
                }
            }

            return values;
        }

        private int[] FlattenGrid(int[,] grid, int width, int height)
        {
            int[] values = new int[(width + 1) * (height + 1)];
            if (grid == null)
            {
                return values;
            }

            int index = 0;
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    values[index++] = grid[x, y];
                }
            }

            return values;
        }

        private void StoreTerrainGrid()
        {
            flatTerrainData = FlattenGrid(terrainGrid);
        }

        private void StoreLayerGrid(MapLayer layer)
        {
            if (layer != null)
            {
                layer.flatVertexData = FlattenGrid(layer.vertGrid);
            }
        }

        private void StoreAllLayerGrids()
        {
            if (layers == null)
            {
                return;
            }

            foreach (MapLayer layer in layers)
            {
                StoreLayerGrid(layer);
            }
        }

        private static void UnflattenGridInto(int[] values, int[,] grid, int width, int height, bool clampBinary = true)
        {
            int index = 0;
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    int value = values != null && index < values.Length ? values[index] : 0;
                    grid[x, y] = clampBinary ? Mathf.Clamp(value, 0, 1) : Mathf.Max(0, value);
                    index++;
                }
            }
        }

        private void ApplyToScene()
        {
            StoreAllLayerGrids();
            StoreTerrainGrid();
            int applied = 0;
            MapLayer output = GetActiveLayer();
            if (output != null && output.enabled && output.tilemap != null && activeBiome != null && terrainGrid != null)
            {
                Undo.RegisterCompleteObjectUndo(output.tilemap, "Apply RIMA Multi-Terrain Map");
                CornerWangPainter.Paint(output.tilemap, activeBiome, terrainGrid, roomWidth, roomHeight);
                EditorUtility.SetDirty(output.tilemap);
                applied = 1;

                TilemapRenderer renderer = output.tilemap.GetComponent<TilemapRenderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = 0;
                }

                if (output.tileSet != null)
                {
                    CliffYSortManager sorter = output.tilemap.GetComponent<CliffYSortManager>();
                    if (sorter == null)
                    {
                        sorter = Undo.AddComponent<CliffYSortManager>(output.tilemap.gameObject);
                    }

                    sorter.tileSet = output.tileSet;
                    sorter.ApplySortMode();
                    EditorUtility.SetDirty(sorter);
                }
            }

            Debug.Log("[MapDesigner] Applied " + roomWidth + "x" + roomHeight + " multi-terrain map to " + applied + " tilemap(s).");
        }

        private void ClearAllTilemaps()
        {
            int cleared = 0;
            foreach (MapLayer layer in layers)
            {
                if (layer.tilemap == null)
                {
                    continue;
                }

                Undo.RegisterCompleteObjectUndo(layer.tilemap, "Clear RIMA Tilemap");
                layer.tilemap.ClearAllTiles();
                EditorUtility.SetDirty(layer.tilemap);
                cleared++;
            }

            Debug.Log("[MapDesigner] Cleared " + cleared + " tilemap(s).");
        }

        private void DrawStatusBar()
        {
            Rect rect = new Rect(0f, position.height - StatusHeight, position.width, StatusHeight);
            EditorGUI.DrawRect(rect, new Color(0.16f, 0.16f, 0.16f, 1f));
            MapLayer layer = GetActiveLayer();
            string tilemapName = layer != null && layer.tilemap != null ? layer.tilemap.name : "No Tilemap";
            string terrainName = GetTerrainName(activeTerrainId);
            string status = string.Format("Room {0}x{1} | Biome: {2} | Terrain: {3} | Output: {4} | Tool: {5} | Mode: {6} | Erase: {7}",
                roomWidth,
                roomHeight,
                activeBiome != null ? activeBiome.biomeName : "None",
                terrainName,
                tilemapName,
                activeTool,
                paintMode,
                eraseMode ? "On" : "Off");

            if (brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight))
            {
                int[,] grid = terrainGrid;
                if (grid != null)
                {
                    int nw = grid[hoveredCell.x, hoveredCell.y + 1];
                    int ne = grid[hoveredCell.x + 1, hoveredCell.y + 1];
                    int sw = grid[hoveredCell.x, hoveredCell.y];
                    int se = grid[hoveredCell.x + 1, hoveredCell.y];
                    var unique = new HashSet<int> { nw, ne, sw, se };
                    if (unique.Count >= 3)
                    {
                        status += string.Format(" | ERROR: {0} terrains in cell ({1},{2})", unique.Count, hoveredCell.x, hoveredCell.y);
                    }
                    else if (unique.Count == 2)
                    {
                        int lower = unique.Min();
                        int upper = unique.Max();
                        int wangKey = ((nw == upper ? 1 : 0) << 3) | ((ne == upper ? 1 : 0) << 2) | ((sw == upper ? 1 : 0) << 1) | (se == upper ? 1 : 0);
                        string keyName = wangKey >= 0 && wangKey < CornerKeyNames.Length ? CornerKeyNames[wangKey] : "Unknown";
                        status += string.Format(" | Cell ({0},{1}) {2}/{3} WangKey={4} ({5})", hoveredCell.x, hoveredCell.y, lower, upper, wangKey, keyName);
                    }
                    else
                    {
                        status += string.Format(" | Cell ({0},{1}) Terrain={2}", hoveredCell.x, hoveredCell.y, nw);
                    }
                }
            }

            EditorGUI.LabelField(new Rect(rect.x + 8f, rect.y + 2f, rect.width - 16f, rect.height - 4f), status, EditorStyles.miniLabel);
        }
    }
}
