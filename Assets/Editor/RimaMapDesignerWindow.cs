using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RIMA;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public class RimaMapDesignerWindow : EditorWindow
    {
        private const int DefaultRoomWidth = 32;
        private const int DefaultRoomHeight = 24;
        private const int MinRoomSize = 4;
        private const int MaxRoomSize = 64;
        private const float LeftPanelWidth = 220f;
        private const float RightPanelWidth = 200f;
        private const float ToolbarHeight = 28f;
        private const float StatusHeight = 60f;
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

        private struct CellPairingInfo
        {
            public bool isValid;
            public Vector2Int cell;
            public int nw;
            public int ne;
            public int sw;
            public int se;
            public int uniqueCount;
            public int lower;
            public int upper;
            public int wangKey;
            public string wangName;
            public string resolves;
            public float transitionSize;
            public TileBase tile;
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
            public MapObjectPlacement[] objects;
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
        [SerializeField] private float cellSize = 40f;
        [SerializeField] private int currentPaintValue = 1;
        [SerializeField] private RimaBiomePreset activeBiome;
        [SerializeField] private int activeTerrainId = 1;
        [SerializeField] private int wallThickness = 2;
        [SerializeField] private float noiseDensity = 0.45f;
        [SerializeField] private int noiseSeed = 12345;
        [SerializeField] private bool advancedFoldout;
        [SerializeField] private bool proceduralFoldout;
        [SerializeField] private bool showTilePreview = true;
        [SerializeField] private bool showMouseDebug;
        [SerializeField] private PaintTool activeTool = PaintTool.Brush;
        [SerializeField] private PaintMode paintMode = PaintMode.Cell;
        [SerializeField] private bool eraseMode;
        [SerializeField] private int brushRadius = 1;
        [SerializeField] private Tilemap outputTilemap;
        [SerializeField, HideInInspector] private bool hasFittedOnce;

        private readonly BrushInputHandler brushInput = new BrushInputHandler();
        [NonSerialized] private int[,] terrainGrid;
        [SerializeField, HideInInspector] private int[] flatTerrainData;
        private Vector2 gridScroll;
        private Vector2 leftScroll;
        private Vector2 lastMousePosition;
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
        private Vector2Int lastPaintedCell = new Vector2Int(-1, -1);
        [SerializeField] private List<MapObjectPlacement> mapObjects = new List<MapObjectPlacement>();
        [SerializeField] private ObjectsPanelDrawer objectsPanel = new ObjectsPanelDrawer();

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
        }

        private void OnDisable()
        {
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
            if (mapObjects == null)
            {
                mapObjects = new List<MapObjectPlacement>();
            }

            if (objectsPanel == null)
            {
                objectsPanel = new ObjectsPanelDrawer();
            }

            NormalizeCellSize();
            if (activeBiome == null)
            {
                activeBiome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>(DefaultBiomePresetPath);
            }

            EnsureTerrainGrid(roomWidth, roomHeight, false);
            EnsureValidActiveTerrain();
            if (paintMode != PaintMode.Vertex)
            {
                paintMode = PaintMode.Cell;
            }

            TryAutoFitOnce();
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

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(ToolbarHeight));

            if (GUILayout.Button("New", EditorStyles.toolbarButton, GUILayout.Width(48f)) &&
                EditorUtility.DisplayDialog("New Map", "Reset to an empty 32x24 floor map?", "New", "Cancel"))
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

            GUILayout.Label("|", EditorStyles.miniLabel, GUILayout.Width(8f));

            if (GUILayout.Button("Apply", EditorStyles.toolbarButton, GUILayout.Width(54f)))
            {
                ApplyToScene();
            }

            if (GUILayout.Button("Generate", EditorStyles.toolbarButton, GUILayout.Width(72f)))
            {
                RoomGeneratorWindow.Open(this);
            }

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(54f)))
            {
                ClearAllTilemaps();
            }

            GUILayout.Label("|", EditorStyles.miniLabel, GUILayout.Width(8f));
            if (GUILayout.Button("Objects", EditorStyles.toolbarButton, GUILayout.Width(66f)))
            {
                objectsPanel.isOpen = !objectsPanel.isOpen;
                if (!objectsPanel.isOpen)
                {
                    objectsPanel.placeMode = false;
                }

                Repaint();
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Fit", EditorStyles.toolbarButton, GUILayout.Width(40f)))
            {
                FitCanvasToWindow();
            }

            GUILayout.Label("Cell", GUILayout.Width(28f));
            cellSize = GUILayout.HorizontalSlider(cellSize, 10f, 80f, GUILayout.Width(112f));
            NormalizeCellSize();
            GUILayout.Label(Mathf.RoundToInt(cellSize) + "px", GUILayout.Width(34f));

            if (GUILayout.Button("Auto-Biome", EditorStyles.toolbarButton, GUILayout.Width(86f)))
            {
                EditorApplication.ExecuteMenuItem("RIMA/Tools/Auto-Build BiomePreset from Tilesets");
            }

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

            EditorGUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("New Biome", GUILayout.Height(22f)))
            {
                CreateNewBiomePreset();
            }

            EditorGUI.BeginDisabledGroup(activeBiome == null);
            if (GUILayout.Button("Edit Biome", GUILayout.Height(22f)))
            {
                BiomeQuickEditorWindow.Open(activeBiome);
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (activeBiome == null)
            {
                EditorGUILayout.HelpBox("Assign or create a Biome preset.", MessageType.Info);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Terrains", EditorStyles.boldLabel);
            DrawTerrainPalette();

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
            outputTilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", outputTilemap, typeof(Tilemap), true);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawTerrainPalette()
        {
            if (activeBiome == null || activeBiome.terrains == null || activeBiome.terrains.Count == 0)
            {
                EditorGUILayout.HelpBox("Biome has no terrains.", MessageType.Warning);
                return;
            }

            const float buttonWidth = 64f;
            const float buttonHeight = 98f;
            const int columns = 3;
            int column = 0;

            EditorGUILayout.BeginHorizontal();
            foreach (MapTerrain terrain in activeBiome.terrains)
            {
                if (terrain == null || terrain.id == 0)
                {
                    continue;
                }

                DrawTerrainButton(terrain, buttonWidth, buttonHeight);
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

        private void DrawTerrainButton(MapTerrain terrain, float width, float height)
        {
            Rect btnRect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
            bool active = activeTerrainId == terrain.id;
            bool hover = btnRect.Contains(Event.current.mousePosition);
            Color baseColor = terrain.paletteColor;
            EditorGUI.DrawRect(btnRect, active ? baseColor * 0.6f : baseColor * (hover ? 0.45f : 0.3f));

            Rect spriteRect = new Rect(btnRect.x + 8f, btnRect.y + 5f, 48f, 42f);
            Rect labelRect = new Rect(btnRect.x + 2f, btnRect.y + 48f, btnRect.width - 4f, 16f);
            Rect idRect = new Rect(btnRect.x + 2f, btnRect.y + 64f, btnRect.width - 4f, 13f);
            Rect pairRect = new Rect(btnRect.x + 2f, btnRect.y + 78f, btnRect.width - 4f, 16f);
            DrawTerrainSpritePreview(spriteRect, terrain);
            EditorGUI.DrawRect(new Rect(btnRect.x + 2f, btnRect.y + btnRect.height - 7f, btnRect.width - 4f, 5f), baseColor);

            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniBoldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                clipping = TextClipping.Clip
            };
            GUI.Label(labelRect, GetTerrainName(terrain.id), labelStyle);
            GUIStyle subStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                clipping = TextClipping.Clip
            };
            GUI.Label(idRect, "id=" + terrain.id, subStyle);
            GUI.Label(pairRect, GetTerrainPairingHint(terrain.id), subStyle);

            Handles.BeginGUI();
            Handles.color = active ? Color.cyan : hover ? new Color(1f, 1f, 1f, 0.5f) : new Color(0f, 0f, 0f, 0.35f);
            Handles.DrawSolidRectangleWithOutline(btnRect, Color.clear, Handles.color);
            if (active)
            {
                Rect inner = new Rect(btnRect.x + 1f, btnRect.y + 1f, btnRect.width - 2f, btnRect.height - 2f);
                Handles.DrawSolidRectangleWithOutline(inner, Color.clear, Color.cyan);
            }

            Handles.EndGUI();

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && btnRect.Contains(Event.current.mousePosition))
            {
                activeTerrainId = terrain.id;
                currentPaintValue = activeTerrainId;
                eraseMode = false;
                Event.current.Use();
                Repaint();
            }
        }

        private static void DrawTerrainSpritePreview(Rect spriteRect, MapTerrain terrain)
        {
            TileBase tile = terrain.baseTile != null
                ? terrain.baseTile
                : terrain.baseTileSource != null ? terrain.baseTileSource.GetTile(0, 0, 0, 0) : null;
            Sprite sprite = (tile as Tile)?.sprite;
            if (sprite != null && sprite.texture != null)
            {
                Rect tc = new Rect(
                    sprite.rect.x / sprite.texture.width,
                    sprite.rect.y / sprite.texture.height,
                    sprite.rect.width / sprite.texture.width,
                    sprite.rect.height / sprite.texture.height);
                GUI.DrawTextureWithTexCoords(spriteRect, sprite.texture, tc);
                return;
            }

            EditorGUI.DrawRect(spriteRect, terrain.paletteColor);
        }

        private void DrawCenterPanel(float height)
        {
            NormalizeCellSize();
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
            float panelWidth = GetRightPanelWidth();
            EditorGUILayout.BeginVertical(GUILayout.Width(panelWidth), GUILayout.ExpandHeight(true));
            GUILayout.Space(8f);

            GUIStyle eraseButton = new GUIStyle(GUI.skin.button) { fixedHeight = 38f, fontSize = 12, fontStyle = FontStyle.Bold };

            Color prevBg = GUI.backgroundColor;
            GUI.backgroundColor = eraseMode ? new Color(0.9f, 0.28f, 0.24f) : prevBg;
            if (GUILayout.Button("ERASE", eraseButton))
            {
                eraseMode = !eraseMode;
                currentPaintValue = eraseMode ? 0 : activeTerrainId;
            }

            GUI.backgroundColor = prevBg;

            EditorGUILayout.Space(6f);

            brushRadius = EditorGUILayout.IntSlider("Brush", brushRadius, 1, 5);

            EditorGUILayout.Space(6f);
            DrawSeparator();

            advancedFoldout = EditorGUILayout.Foldout(advancedFoldout, "Advanced", true);
            if (advancedFoldout)
            {
                bool vertexMode = paintMode == PaintMode.Vertex;
                vertexMode = EditorGUILayout.ToggleLeft("Vertex mode", vertexMode);
                paintMode = vertexMode ? PaintMode.Vertex : PaintMode.Cell;

                EditorGUILayout.Space(4f);
                EditorGUILayout.LabelField("Tool", EditorStyles.miniLabel);
                activeTool = (PaintTool)GUILayout.Toolbar((int)activeTool, new[] { "Brush", "Fill", "Rect" });

                EditorGUILayout.Space(4f);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Room W", GUILayout.Width(54f));
                roomWidth = Mathf.Clamp(EditorGUILayout.IntField(roomWidth), MinRoomSize, MaxRoomSize);
                EditorGUILayout.LabelField("H", GUILayout.Width(14f));
                roomHeight = Mathf.Clamp(EditorGUILayout.IntField(roomHeight), MinRoomSize, MaxRoomSize);
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Resize"))
                {
                    ResizeGrid(roomWidth, roomHeight, true);
                }

                EditorGUILayout.Space(4f);
                showTilePreview = EditorGUILayout.ToggleLeft("Show tiles", showTilePreview);
                showMouseDebug = EditorGUILayout.ToggleLeft("Show mouse debug", showMouseDebug);
            }

            DrawSeparator();

            proceduralFoldout = EditorGUILayout.Foldout(proceduralFoldout, "Procedural", true);
            if (proceduralFoldout)
            {
                if (GUILayout.Button("Rectangular"))
                {
                    MakeRectangularRoom();
                }

                if (GUILayout.Button("L-Shape"))
                {
                    MakeLShapeRoom();
                }

                if (GUILayout.Button("Perlin Noise"))
                {
                    PerlinNoiseFill();
                }
            }

            GUILayout.FlexibleSpace();
            DrawActivePairingPanel();

            if (objectsPanel.isOpen)
            {
                GUILayout.Space(8f);
                float objectsHeight = Mathf.Min(360f, Mathf.Max(220f, position.height - ToolbarHeight - StatusHeight - 260f));
                Rect objectsRect = GUILayoutUtility.GetRect(panelWidth - 12f, objectsHeight, GUILayout.ExpandWidth(true));
                objectsPanel.Draw(objectsRect, mapObjects, AddMapObject, RemoveMapObject);
            }

            EditorGUILayout.EndVertical();
        }

        private float GetRightPanelWidth()
        {
            return objectsPanel != null && objectsPanel.isOpen ? 360f : RightPanelWidth;
        }

        private static void DrawSeparator()
        {
            Rect rect = GUILayoutUtility.GetRect(1f, 8f, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(new Rect(rect.x, rect.y + 4f, rect.width, 1f), new Color(0.28f, 0.28f, 0.28f, 1f));
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

            DrawCursorPreviewOverlay();
            Handles.EndGUI();

            DrawPlacedObjectsOnCanvas();
            DrawObjectPlacePreview();
        }

        private void DrawCellHover(Vector2Int cell)
        {
            Rect rect = CellToCanvasRect(cell);
            Color border = eraseMode ? Color.red : Color.green;

            if (paintMode == PaintMode.Cell)
            {
                int previewValue = eraseMode ? 0 : activeTerrainId;
                CellPairingInfo preview = BuildCellPairingInfo(cell, true, previewValue);
                if (preview.tile != null)
                {
                    DrawTileBase(rect, preview.tile, GetTerrainColor(previewValue), 0.6f);
                }
                else
                {
                    Color fill = eraseMode ? new Color(1f, 0.2f, 0.2f, 0.35f) : new Color(0.2f, 1f, 0.4f, 0.28f);
                    EditorGUI.DrawRect(rect, fill);
                }

                MapTerrain activeTerrain = GetTerrain(activeTerrainId);
                if (activeTerrain != null)
                {
                    Rect thumbRect = new Rect(rect.xMax + 4f, rect.yMin - 4f, 24f, 24f);
                    DrawTileBase(thumbRect, GetTerrainBaseTile(activeTerrain), activeTerrain.paletteColor, 0.9f);
                }
            }
            else
            {
                Color fill = eraseMode ? new Color(1f, 0.2f, 0.2f, 0.4f) : new Color(0.2f, 1f, 0.4f, 0.4f);
                EditorGUI.DrawRect(rect, fill);
            }

            Handles.color = border;
            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, border);

            if (brushRadius > 1)
            {
                int r = brushRadius - 1;
                Vector2Int min = new Vector2Int(Mathf.Max(0, cell.x - r), Mathf.Max(0, cell.y - r));
                Vector2Int max = new Vector2Int(Mathf.Min(roomWidth - 1, cell.x + r), Mathf.Min(roomHeight - 1, cell.y + r));
                Rect a = CellToCanvasRect(min);
                Rect b = CellToCanvasRect(max);
                Rect outline = Rect.MinMaxRect(
                    Mathf.Min(a.xMin, b.xMin),
                    Mathf.Min(a.yMin, b.yMin),
                    Mathf.Max(a.xMax, b.xMax),
                    Mathf.Max(a.yMax, b.yMax));
                Handles.color = new Color(0f, 1f, 1f, 0.5f);
                Handles.DrawSolidRectangleWithOutline(outline, Color.clear, Handles.color);
            }
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
                    TileBase tile = CornerWangPainter.ResolveTile(activeBiome, nw, ne, sw, se, x, y);
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
            lastMousePosition = mouse;
            NormalizeCellSize();
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
                if (evt.keyCode == KeyCode.Escape && objectsPanel != null && objectsPanel.placeMode)
                {
                    objectsPanel.placeMode = false;
                    evt.Use();
                    Repaint();
                    return;
                }

                if (evt.keyCode == KeyCode.Plus || evt.keyCode == KeyCode.KeypadPlus || evt.character == '+')
                {
                    SetCellSize(cellSize + 4f);
                    Repaint();
                    evt.Use();
                    return;
                }

                if (evt.keyCode == KeyCode.Minus || evt.keyCode == KeyCode.KeypadMinus || evt.character == '-')
                {
                    SetCellSize(cellSize - 4f);
                    Repaint();
                    evt.Use();
                    return;
                }
            }

            if (evt.type == EventType.ScrollWheel && inCanvas)
            {
                float delta = evt.delta.y;
                float t = Mathf.Sign(delta) * Mathf.Pow(Mathf.Abs(delta) * 0.1f, 1.5f);
                SetCellSize(cellSize - t * 2f);
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

            if (HandleObjectLayerInput(evt, mouse, inCanvas))
            {
                return;
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
                        if (paintMode == PaintMode.Cell && brushInput.IsValidCell(lastPaintedCell, roomWidth, roomHeight))
                        {
                            PaintLineFromTo(lastPaintedCell, current, value);
                        }
                        else
                        {
                            PaintWithRadius(current, value);
                        }

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

        private bool HandleObjectLayerInput(Event evt, Vector2 mouse, bool inCanvas)
        {
            if (objectsPanel == null || !objectsPanel.isOpen || !inCanvas)
            {
                return false;
            }

            bool canPlace = objectsPanel.placeMode && objectsPanel.selectedPrefab != null;
            if (canPlace)
            {
                if (evt.type == EventType.MouseDown && evt.button == 0)
                {
                    AddMapObject(objectsPanel.CreatePlacement(CanvasMouseToObjectPositionPx(mouse)));
                    evt.Use();
                    Repaint();
                    return true;
                }

                if (evt.type == EventType.MouseMove)
                {
                    Repaint();
                }

                return true;
            }

            if (evt.type == EventType.MouseDown && evt.button == 0)
            {
                MapObjectPlacement hit = GetObjectAtCanvasPosition(mouse);
                if (hit != null)
                {
                    objectsPanel.selectedPlacement = hit;
                    evt.Use();
                    Repaint();
                    return true;
                }
            }

            return false;
        }

        private void AddMapObject(MapObjectPlacement placement)
        {
            if (placement == null)
            {
                return;
            }

            mapObjects.Add(placement);
            objectsPanel.selectedPlacement = placement;
        }

        private void RemoveMapObject(MapObjectPlacement placement)
        {
            if (placement == null)
            {
                return;
            }

            mapObjects.Remove(placement);
            if (objectsPanel != null && objectsPanel.selectedPlacement == placement)
            {
                objectsPanel.selectedPlacement = null;
            }

            Repaint();
        }

        private Vector2 CanvasMouseToObjectPositionPx(Vector2 mouse)
        {
            float widthPx = roomWidth * cellSize;
            float heightPx = roomHeight * cellSize;
            return new Vector2(
                Mathf.Clamp(mouse.x - CanvasPadding, 0f, widthPx),
                Mathf.Clamp(heightPx - (mouse.y - CanvasPadding), 0f, heightPx));
        }

        private Vector2 ObjectPositionPxToCanvas(MapObjectPlacement placement)
        {
            float heightPx = roomHeight * cellSize;
            return new Vector2(
                CanvasPadding + placement.positionPx.x,
                CanvasPadding + heightPx - placement.positionPx.y);
        }

        private Rect GetObjectCanvasRect(MapObjectPlacement placement)
        {
            Vector2 center = ObjectPositionPxToCanvas(placement);
            float size = Mathf.Clamp(cellSize * 0.8f, 18f, 56f);
            return new Rect(center.x - size * 0.5f, center.y - size * 0.5f, size, size);
        }

        private MapObjectPlacement GetObjectAtCanvasPosition(Vector2 mouse)
        {
            for (int i = mapObjects.Count - 1; i >= 0; i--)
            {
                MapObjectPlacement placement = mapObjects[i];
                if (placement != null && placement.visible && GetObjectCanvasRect(placement).Contains(mouse))
                {
                    return placement;
                }
            }

            return null;
        }

        private void DrawPlacedObjectsOnCanvas()
        {
            if (mapObjects == null || mapObjects.Count == 0)
            {
                return;
            }

            foreach (MapObjectPlacement placement in mapObjects.OrderBy(o => o != null ? o.layer : 0))
            {
                if (placement == null || !placement.visible)
                {
                    continue;
                }

                Rect rect = GetObjectCanvasRect(placement);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(placement.prefabPath);
                Texture preview = prefab != null ? AssetPreview.GetAssetPreview(prefab) : null;
                if (preview == null && prefab != null)
                {
                    preview = AssetPreview.GetMiniThumbnail(prefab);
                }

                Color oldColor = GUI.color;
                GUI.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.92f);
                if (preview != null)
                {
                    GUI.DrawTexture(rect, preview, ScaleMode.ScaleToFit);
                }
                else
                {
                    EditorGUI.DrawRect(rect, new Color(0.25f, 0.55f, 0.8f, 0.75f));
                    GUI.Label(rect, "OBJ", EditorStyles.centeredGreyMiniLabel);
                }

                GUI.color = oldColor;

                Handles.BeginGUI();
                Handles.color = objectsPanel != null && objectsPanel.selectedPlacement == placement ? Color.cyan : new Color(1f, 1f, 1f, 0.45f);
                Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Handles.color);
                Handles.EndGUI();
            }
        }

        private void DrawObjectPlacePreview()
        {
            if (objectsPanel == null || !objectsPanel.isOpen || !objectsPanel.placeMode || objectsPanel.selectedPrefab == null)
            {
                return;
            }

            Event evt = Event.current;
            Vector2 mouse = evt.mousePosition;
            float widthPx = roomWidth * cellSize;
            float heightPx = roomHeight * cellSize;
            Rect canvasBounds = new Rect(CanvasPadding, CanvasPadding, widthPx, heightPx);
            if (!canvasBounds.Contains(mouse))
            {
                return;
            }

            float size = Mathf.Clamp(cellSize * 0.8f, 18f, 56f);
            Rect rect = new Rect(mouse.x - size * 0.5f, mouse.y - size * 0.5f, size, size);
            Texture preview = AssetPreview.GetAssetPreview(objectsPanel.selectedPrefab);
            if (preview == null)
            {
                preview = AssetPreview.GetMiniThumbnail(objectsPanel.selectedPrefab);
            }

            Color oldColor = GUI.color;
            GUI.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
            if (preview != null)
            {
                GUI.DrawTexture(rect, preview, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.DrawRect(rect, new Color(0f, 1f, 1f, 0.28f));
            }

            GUI.color = oldColor;

            Handles.BeginGUI();
            Handles.color = Color.cyan;
            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Handles.color);
            Handles.EndGUI();
        }

        private int GetActualPaintValue(bool invertForButton)
        {
            int value = eraseMode || invertForButton ? 0 : activeTerrainId;
            currentPaintValue = value;
            return value;
        }

        private Vector2 VertexToCanvasPosition(int x, int y)
        {
            return new Vector2(
                Mathf.Round(CanvasPadding + x * cellSize),
                Mathf.Round(CanvasPadding + (roomHeight - y) * cellSize));
        }

        private Rect CellToCanvasRect(Vector2Int cell)
        {
            return new Rect(
                Mathf.Round(CanvasPadding + cell.x * cellSize),
                Mathf.Round(CanvasPadding + (roomHeight - cell.y - 1) * cellSize),
                Mathf.Round(cellSize),
                Mathf.Round(cellSize));
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

        private void PaintLineFromTo(Vector2Int a, Vector2Int b, int value)
        {
            int dx = Mathf.Abs(b.x - a.x);
            int dy = Mathf.Abs(b.y - a.y);
            int sx = a.x < b.x ? 1 : -1;
            int sy = a.y < b.y ? 1 : -1;
            int err = dx - dy;
            int x = a.x;
            int y = a.y;

            while (true)
            {
                PaintWithRadius(new Vector2Int(x, y), value);
                if (x == b.x && y == b.y)
                {
                    break;
                }

                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
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

        private string GetPairingName(int lowerTerrainId, int upperTerrainId, string fallback)
        {
            TilesetPairing pairing = activeBiome != null ? activeBiome.FindPairing(lowerTerrainId, upperTerrainId) : null;
            if (pairing != null && pairing.tileSet != null)
            {
                return pairing.tileSet.name;
            }

            return fallback;
        }

        private MapTerrain GetTerrain(int terrainId)
        {
            return activeBiome != null && activeBiome.terrains != null
                ? activeBiome.terrains.FirstOrDefault(t => t != null && t.id == terrainId)
                : null;
        }

        private static TileBase GetTerrainBaseTile(MapTerrain terrain)
        {
            if (terrain == null)
            {
                return null;
            }

            return terrain.baseTile != null
                ? terrain.baseTile
                : terrain.baseTileSource != null ? terrain.baseTileSource.GetTile(0, 0, 0, 0) : null;
        }

        private static void DrawTileBase(Rect rect, TileBase tile, Color fallbackColor, float alpha)
        {
            Sprite sprite = (tile as Tile)?.sprite;
            Color oldColor = GUI.color;
            GUI.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a * alpha);
            if (sprite != null && sprite.texture != null)
            {
                Rect tc = new Rect(
                    sprite.rect.x / sprite.texture.width,
                    sprite.rect.y / sprite.texture.height,
                    sprite.rect.width / sprite.texture.width,
                    sprite.rect.height / sprite.texture.height);
                GUI.DrawTextureWithTexCoords(rect, sprite.texture, tc);
            }
            else
            {
                EditorGUI.DrawRect(rect, new Color(fallbackColor.r, fallbackColor.g, fallbackColor.b, fallbackColor.a * alpha));
            }

            GUI.color = oldColor;
        }

        private CellPairingInfo BuildCellPairingInfo(Vector2Int cell, bool simulatePaint, int simulatedTerrainId)
        {
            CellPairingInfo info = new CellPairingInfo
            {
                cell = cell,
                wangName = "Unknown",
                resolves = "None",
                transitionSize = 0.25f
            };

            int[,] grid = GetActiveGrid();
            if (grid == null || !brushInput.IsValidCell(cell, roomWidth, roomHeight))
            {
                return info;
            }

            int value = Mathf.Max(0, simulatedTerrainId);
            info.nw = simulatePaint ? value : grid[cell.x, cell.y + 1];
            info.ne = simulatePaint ? value : grid[cell.x + 1, cell.y + 1];
            info.sw = simulatePaint ? value : grid[cell.x, cell.y];
            info.se = simulatePaint ? value : grid[cell.x + 1, cell.y];
            info.tile = CornerWangPainter.ResolveTile(activeBiome, info.nw, info.ne, info.sw, info.se, cell.x, cell.y);

            var unique = new HashSet<int> { info.nw, info.ne, info.sw, info.se };
            info.uniqueCount = unique.Count;
            info.isValid = true;

            if (unique.Count == 1)
            {
                info.lower = info.nw;
                info.upper = info.nw;
                info.wangKey = 0;
                info.wangName = "All " + GetTerrainName(info.nw);
                info.resolves = info.tile != null ? GetTerrainName(info.nw) : "Missing base tile";
                info.transitionSize = 0f;
                return info;
            }

            if (unique.Count != 2)
            {
                info.lower = unique.Min();
                info.upper = unique.Max();
                info.wangKey = -1;
                info.wangName = "Fallback";
                info.resolves = "Fallback: " + unique.Count + " terrains";
                return info;
            }

            info.lower = unique.Min();
            info.upper = unique.Max();
            info.wangKey =
                ((info.nw == info.upper ? 1 : 0) << 3) |
                ((info.ne == info.upper ? 1 : 0) << 2) |
                ((info.sw == info.upper ? 1 : 0) << 1) |
                (info.se == info.upper ? 1 : 0);
            info.wangName = info.wangKey >= 0 && info.wangKey < CornerKeyNames.Length ? CornerKeyNames[info.wangKey] : "Unknown";

            TilesetPairing pairing = activeBiome != null ? activeBiome.FindPairing(info.lower, info.upper) : null;
            if (pairing != null)
            {
                info.transitionSize = pairing.transitionSize;
                info.resolves = pairing.tileSet != null ? pairing.tileSet.name + "[" + info.wangKey + "]" : "Missing tileSet";
            }
            else
            {
                info.resolves = "Missing pairing " + GetTerrainName(info.lower) + "<->" + GetTerrainName(info.upper);
            }

            return info;
        }

        private string GetTerrainPairingHint(int terrainId)
        {
            if (activeBiome == null || activeBiome.tilesetPairings == null)
            {
                return "Pairs: none";
            }

            List<string> peers = new List<string>();
            foreach (TilesetPairing pairing in activeBiome.tilesetPairings)
            {
                if (pairing == null)
                {
                    continue;
                }

                if (pairing.lowerTerrainId == terrainId)
                {
                    peers.Add(GetTerrainName(pairing.upperTerrainId));
                }
                else if (pairing.upperTerrainId == terrainId)
                {
                    peers.Add(GetTerrainName(pairing.lowerTerrainId));
                }
            }

            peers = peers.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
            if (peers.Count == 0)
            {
                return "Pairs: none";
            }

            string text = "Pairs: " + peers[0];
            int shown = 1;
            while (shown < peers.Count && (text + ", " + peers[shown]).Length <= 17)
            {
                text += ", " + peers[shown];
                shown++;
            }

            if (shown < peers.Count)
            {
                text += " +" + (peers.Count - shown);
            }

            return text;
        }

        private void DrawActivePairingPanel()
        {
            DrawSeparator();
            EditorGUILayout.LabelField("Active Pairing", EditorStyles.boldLabel);
            if (!brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight))
            {
                EditorGUILayout.LabelField("Hover a cell to see pairing info", EditorStyles.wordWrappedMiniLabel);
                return;
            }

            CellPairingInfo info = BuildCellPairingInfo(hoveredCell, false, activeTerrainId);
            EditorGUILayout.LabelField("Hover cell: (" + info.cell.x + ", " + info.cell.y + ")", EditorStyles.miniLabel);
            EditorGUILayout.LabelField(
                string.Format("Corners: NW={0} NE={1}", info.nw, info.ne),
                EditorStyles.miniLabel);
            EditorGUILayout.LabelField(
                string.Format("         SW={0} SE={1}", info.sw, info.se),
                EditorStyles.miniLabel);
            EditorGUILayout.LabelField("Wang Key: " + info.wangKey + " (" + info.wangName + ")", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("Resolves: " + info.resolves, EditorStyles.wordWrappedMiniLabel);
            EditorGUILayout.LabelField("Transition: " + info.transitionSize.ToString("0.##"), EditorStyles.miniLabel);
        }

        private void DrawCursorPreviewOverlay()
        {
            if (paintMode != PaintMode.Cell || !brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight))
            {
                return;
            }

            Vector2 mouse = Event.current.mousePosition;
            Rect previewRect = new Rect(mouse.x + 12f, mouse.y + 12f, 32f, 32f);
            if (eraseMode)
            {
                EditorGUI.DrawRect(previewRect, new Color(1f, 0f, 0f, 0.18f));
                Handles.color = new Color(1f, 0f, 0f, 0.85f);
                Handles.DrawLine(new Vector3(previewRect.xMin, previewRect.yMin), new Vector3(previewRect.xMax, previewRect.yMax));
                Handles.DrawLine(new Vector3(previewRect.xMax, previewRect.yMin), new Vector3(previewRect.xMin, previewRect.yMax));
                Handles.DrawSolidRectangleWithOutline(previewRect, Color.clear, Handles.color);
                return;
            }

            MapTerrain activeTerrain = GetTerrain(activeTerrainId);
            if (activeTerrain == null)
            {
                return;
            }

            DrawTileBase(previewRect, GetTerrainBaseTile(activeTerrain), activeTerrain.paletteColor, 0.5f);
            Handles.color = new Color(0f, 1f, 1f, 0.45f);
            Handles.DrawSolidRectangleWithOutline(previewRect, Color.clear, Handles.color);
        }

        private void TryAutoFitOnce()
        {
            if (hasFittedOnce || position.width <= 1f || position.height <= 1f)
            {
                return;
            }

            hasFittedOnce = true;
            FitCanvasToWindow();
        }

        private void CreateNewBiomePreset()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create RIMA Biome Preset",
                "RimaBiomePreset",
                "asset",
                "Create biome preset",
                "Assets");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var preset = CreateInstance<RimaBiomePreset>();
            preset.biomeName = Path.GetFileNameWithoutExtension(path);
            preset.terrains = new List<MapTerrain>
            {
                new MapTerrain { id = 0, name = "Floor", paletteColor = new Color(0.25f, 0.25f, 0.25f) },
                new MapTerrain { id = 1, name = "Wall", paletteColor = new Color(0.45f, 0.34f, 0.24f) }
            };

            AssetDatabase.CreateAsset(preset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            activeBiome = preset;
            EnsureValidActiveTerrain();
            Selection.activeObject = preset;
            EditorGUIUtility.PingObject(preset);
        }

        private void FitCanvasToWindow()
        {
            float availW = position.width - LeftPanelWidth - GetRightPanelWidth() - CanvasPadding * 2f;
            float availH = position.height - ToolbarHeight - StatusHeight - CanvasPadding * 2f;
            SetCellSize(Mathf.Floor(Mathf.Min(availW / Mathf.Max(1, roomWidth), availH / Mathf.Max(1, roomHeight))));
            gridScroll = Vector2.zero;
            Repaint();
        }

        private void SetCellSize(float value)
        {
            cellSize = Mathf.Round(Mathf.Clamp(value, 10f, 80f));
        }

        private void NormalizeCellSize()
        {
            SetCellSize(cellSize);
        }

        private void ResetMap()
        {
            roomWidth = DefaultRoomWidth;
            roomHeight = DefaultRoomHeight;
            ResizeGrid(roomWidth, roomHeight, false);
            mapObjects.Clear();
            if (objectsPanel != null)
            {
                objectsPanel.selectedPlacement = null;
                objectsPanel.placeMode = false;
            }

            FillAll(0);
        }

        private void ResizeGrid(int newWidth, int newHeight, bool preserve)
        {
            newWidth = Mathf.Clamp(newWidth, MinRoomSize, MaxRoomSize);
            newHeight = Mathf.Clamp(newHeight, MinRoomSize, MaxRoomSize);
            StoreTerrainGrid();
            roomWidth = newWidth;
            roomHeight = newHeight;

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

            StoreTerrainGrid();
            MapSaveData data = new MapSaveData
            {
                width = roomWidth,
                height = roomHeight,
                terrainGrid = FlattenGrid(terrainGrid),
                biomePresetGuid = activeBiome != null ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(activeBiome)) : string.Empty,
                layers = Array.Empty<LayerSaveData>(),
                objects = mapObjects != null ? mapObjects.ToArray() : Array.Empty<MapObjectPlacement>()
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
            mapObjects = data.objects != null ? data.objects.Where(o => o != null).ToList() : new List<MapObjectPlacement>();
            if (objectsPanel != null)
            {
                objectsPanel.selectedPlacement = null;
                objectsPanel.placeMode = false;
            }

            terrainGrid = null;
            EnsureTerrainGrid(roomWidth, roomHeight, false);

            EnsureValidActiveTerrain();
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
            mapObjects = generated.objects != null ? generated.objects.Where(o => o != null).ToList() : new List<MapObjectPlacement>();
            if (objectsPanel != null)
            {
                objectsPanel.selectedPlacement = null;
                objectsPanel.placeMode = false;
            }

            terrainGrid = null;
            EnsureTerrainGrid(roomWidth, roomHeight, false);

            EnsureValidActiveTerrain();
            Repaint();
            Debug.Log("[MapDesigner] Loaded generated room " + roomWidth + "x" + roomHeight + " with single terrain grid.");
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
            StoreTerrainGrid();
            int applied = 0;
            if (outputTilemap != null && activeBiome != null && terrainGrid != null)
            {
                Undo.RegisterCompleteObjectUndo(outputTilemap, "Apply RIMA Map");
                CornerWangPainter.Paint(outputTilemap, activeBiome, terrainGrid, roomWidth, roomHeight);
                EditorUtility.SetDirty(outputTilemap);
                applied = 1;

                TilemapRenderer renderer = outputTilemap.GetComponent<TilemapRenderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = 0;
                }
            }

            int placedObjects = ApplyObjectsToScene();
            Debug.Log("[MapDesigner] Applied " + roomWidth + "x" + roomHeight + " map to " + applied + " tilemap(s).");
            if (placedObjects > 0)
            {
                Debug.Log("[MapDesigner] Instantiated " + placedObjects + " object(s).");
            }
        }

        private int ApplyObjectsToScene()
        {
            if (outputTilemap == null || mapObjects == null || mapObjects.Count == 0)
            {
                return 0;
            }

            Transform parent = outputTilemap.transform.parent != null ? outputTilemap.transform.parent : outputTilemap.transform;
            int count = 0;
            foreach (MapObjectPlacement obj in mapObjects)
            {
                if (obj == null || !obj.visible || string.IsNullOrEmpty(obj.prefabPath))
                {
                    continue;
                }

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(obj.prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning("[MapDesigner] Object prefab missing: " + obj.prefabPath);
                    continue;
                }

                GameObject instance = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
                if (instance == null)
                {
                    continue;
                }

                instance.transform.position = ObjectPositionPxToWorld(obj.positionPx);
                instance.name = !string.IsNullOrEmpty(obj.displayName) ? obj.displayName : prefab.name;
                foreach (SpriteRenderer renderer in instance.GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.sortingOrder = obj.layer;
                }

                Undo.RegisterCreatedObjectUndo(instance, "Place RIMA Object");
                count++;
            }

            return count;
        }

        private Vector3 ObjectPositionPxToWorld(Vector2 positionPx)
        {
            Vector3 origin = outputTilemap.CellToWorld(Vector3Int.zero);
            Vector3 unitX = outputTilemap.CellToWorld(Vector3Int.right) - origin;
            Vector3 unitY = outputTilemap.CellToWorld(Vector3Int.up) - origin;
            float xCells = cellSize > 0f ? positionPx.x / cellSize : 0f;
            float yCells = cellSize > 0f ? positionPx.y / cellSize : 0f;
            return origin + unitX * xCells + unitY * yCells;
        }

        private void ClearAllTilemaps()
        {
            int cleared = 0;
            if (outputTilemap != null)
            {
                Undo.RegisterCompleteObjectUndo(outputTilemap, "Clear RIMA Tilemap");
                outputTilemap.ClearAllTiles();
                EditorUtility.SetDirty(outputTilemap);
                cleared++;
            }

            Debug.Log("[MapDesigner] Cleared " + cleared + " tilemap(s).");
        }

        private void DrawStatusBar()
        {
            Rect rect = new Rect(0f, position.height - StatusHeight, position.width, StatusHeight);
            EditorGUI.DrawRect(rect, new Color(0.16f, 0.16f, 0.16f, 1f));
            string tilemapName = outputTilemap != null ? outputTilemap.name : "No Tilemap";
            string terrainName = GetTerrainName(activeTerrainId);
            string line1 = string.Format("Room {0}x{1} | Biome: {2} | Active: {3} (id={4}) | Output: {5} | Erase: {6}",
                roomWidth,
                roomHeight,
                activeBiome != null ? activeBiome.biomeName : "None",
                terrainName,
                activeTerrainId,
                tilemapName,
                eraseMode ? "On" : "Off");

            string line2 = "Cell: hover canvas for corners, Wang key, tileSet, transition";
            string line3 = "Tip: Drag to paint, Space+drag to pan, scroll to zoom, +/- to zoom";
            if (brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight))
            {
                CellPairingInfo info = BuildCellPairingInfo(hoveredCell, false, activeTerrainId);
                if (info.isValid)
                {
                    line2 = string.Format("Cell ({0},{1}) | Corners: NW={2} NE={3} SW={4} SE={5} | WangKey={6} | TileSet: {7} | Transition={8:0.##}",
                        hoveredCell.x,
                        hoveredCell.y,
                        info.nw,
                        info.ne,
                        info.sw,
                        info.se,
                        info.wangKey,
                        info.resolves,
                        info.transitionSize);

                    if (info.uniqueCount >= 3)
                    {
                        line3 = string.Format("WARNING: Cell ({0},{1}) has {2} terrains; rendering fallback", hoveredCell.x, hoveredCell.y, info.uniqueCount);
                    }
                }
            }

            if (showMouseDebug)
            {
                line2 += string.Format(" | mouse=({0:0},{1:0}) cellSize={2:0} padding={3:0} -> cell=({4},{5})",
                    lastMousePosition.x, lastMousePosition.y, cellSize, CanvasPadding, hoveredCell.x, hoveredCell.y);
            }

            EditorGUI.LabelField(new Rect(rect.x + 8f, rect.y + 3f, rect.width - 16f, 16f), line1, EditorStyles.miniLabel);
            EditorGUI.LabelField(new Rect(rect.x + 8f, rect.y + 21f, rect.width - 16f, 16f), line2, EditorStyles.miniLabel);
            EditorGUI.LabelField(new Rect(rect.x + 8f, rect.y + 39f, rect.width - 16f, 16f), line3, EditorStyles.miniLabel);
        }
    }
}
