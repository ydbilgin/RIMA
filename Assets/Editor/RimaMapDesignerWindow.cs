using System.Collections.Generic;
using System.IO;
using RIMA;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public class RimaMapDesignerWindow : EditorWindow
    {
        private const int DefaultRoomWidth = 20;
        private const int DefaultRoomHeight = 15;
        private const int MinRoomSize = 4;
        private const int MaxRoomSize = 64;
        private const int MaxLayers = 4;
        private const float LeftPanelWidth = 200f;
        private const float RightPanelWidth = 200f;
        private const float ToolbarHeight = 28f;
        private const float StatusHeight = 22f;
        private const float VertexRadius = 5f;
        private const float CanvasPadding = 24f;
        private const string MapDataFolder = "Assets/RIMA_MapData";
        private static readonly string[] CornerKeyNames =
        {
            "All Floor",
            "SE Corner",
            "SW Corner",
            "S Edge",
            "NE Corner",
            "E Edge",
            "NE↔SW Diag",
            "No NW",
            "NW Corner",
            "NW↔SE Diag",
            "W Edge",
            "No NE",
            "N Edge",
            "No SW",
            "No SE",
            "All Wall"
        };

        private static readonly int[] KeyToIndex = { 6, 7, 10, 9, 2, 11, 4, 15, 5, 14, 1, 8, 3, 0, 13, 12 };

        private enum PaintTool
        {
            Brush,
            Fill,
            Rectangle
        }

        [System.Serializable]
        public class MapLayer
        {
            public string name = "Base";
            public Tilemap tilemap;
            public CornerWangTileSetSO tileSet;
            public bool enabled = true;
        }

        [System.Serializable]
        public class MapSaveData
        {
            public int width;
            public int height;
            public int[] vertexData;
            public string[] layerNames;
        }

        [SerializeField] private int roomWidth = DefaultRoomWidth;
        [SerializeField] private int roomHeight = DefaultRoomHeight;
        [SerializeField] private float cellSize = 28f;
        [SerializeField] private int currentPaintValue = 1;
        [SerializeField] private int wallThickness = 2;
        [SerializeField] private float noiseDensity = 0.45f;
        [SerializeField] private int noiseSeed = 12345;
        [SerializeField] private bool proceduralFoldout = true;
        [SerializeField] private bool tilePreviewFoldout = true;
        [SerializeField] private PaintTool activeTool = PaintTool.Brush;
        [SerializeField] private List<MapLayer> layers = new List<MapLayer>();

        private int[,] vertGrid;
        private ReorderableList layerList;
        private Vector2 gridScroll;
        private Vector2Int hoveredVertex = new Vector2Int(-1, -1);
        private Vector2Int rectStart = new Vector2Int(-1, -1);
        private Vector2Int rectCurrent = new Vector2Int(-1, -1);
        private bool isPainting;
        private bool isRectangleDragging;
        private bool isPanning;
        private Vector2 panStartMouse;
        private Vector2 panStartScroll;
        private int activeLayerIndex;

        [MenuItem("RIMA/Tools/Map Designer")]
        public static void Open()
        {
            var window = GetWindow<RimaMapDesignerWindow>("RIMA Map Designer");
            window.minSize = new Vector2(720f, 420f);
            window.Show();
        }

        private void OnEnable()
        {
            EnsureInitialized();
            BuildLayerList();
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

            if (vertGrid == null || vertGrid.GetLength(0) != roomWidth + 1 || vertGrid.GetLength(1) != roomHeight + 1)
            {
                ResizeGrid(roomWidth, roomHeight, true);
            }
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

                layers.Add(new MapLayer { name = layers.Count == 0 ? "Base" : "Layer " + (layers.Count + 1) });
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
            Rect labelRect = new Rect(rect.x, y, 54f, line);
            Rect fieldRect = new Rect(rect.x + 58f, y, rect.width - 58f, line);

            EditorGUI.LabelField(labelRect, "Name");
            layer.name = EditorGUI.TextField(fieldRect, layer.name);

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

            if (GUILayout.Button("New", EditorStyles.toolbarButton, GUILayout.Width(48f)))
            {
                if (EditorUtility.DisplayDialog("New Map", "Reset to an empty 20x15 floor map?", "New", "Cancel"))
                {
                    ResetMap();
                }
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

            if (GUILayout.Button("Clear All", EditorStyles.toolbarButton, GUILayout.Width(72f)))
            {
                ClearAllTilemaps();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Cell", GUILayout.Width(28f));
            cellSize = GUILayout.HorizontalSlider(cellSize, 16f, 36f, GUILayout.Width(120f));
            GUILayout.Label(Mathf.RoundToInt(cellSize) + "px", GUILayout.Width(34f));

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(LeftPanelWidth), GUILayout.ExpandHeight(true));
            if (layerList == null)
            {
                BuildLayerList();
            }

            layerList.DoLayoutList();
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
            HandleGridInput(centerRect);
            GUI.EndScrollView();
        }

        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(RightPanelWidth), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField("Paint Tools", EditorStyles.boldLabel);
            activeTool = (PaintTool)GUILayout.Toolbar((int)activeTool, new[] { "Brush", "Fill", "Rect" });
            currentPaintValue = GUILayout.Toolbar(currentPaintValue == 0 ? 0 : 1, new[] { "Floor (0)", "Wall (1)" });

            EditorGUILayout.Space(8f);
            DrawTilePreviewPanel(layers.Count > 0 ? layers[activeLayerIndex] : null);

            EditorGUILayout.Space(8f);
            proceduralFoldout = EditorGUILayout.Foldout(proceduralFoldout, "Procedural Generation", true);
            if (proceduralFoldout)
            {
                wallThickness = EditorGUILayout.IntField("Wall Thickness", wallThickness);
                wallThickness = Mathf.Clamp(wallThickness, 1, Mathf.Min(roomWidth, roomHeight) / 2);

                if (GUILayout.Button("Make Rectangular Room"))
                {
                    MakeRectangularRoom();
                }

                if (GUILayout.Button("L-Shape Room"))
                {
                    MakeLShapeRoom();
                }

                noiseDensity = EditorGUILayout.Slider("Density", noiseDensity, 0f, 1f);
                noiseSeed = EditorGUILayout.IntField("Seed", noiseSeed);
                if (GUILayout.Button("Perlin Noise Fill"))
                {
                    PerlinNoiseFill();
                }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Corridor H"))
                {
                    PaintHorizontalCorridor();
                }

                if (GUILayout.Button("Corridor V"))
                {
                    PaintVerticalCorridor();
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Clear -> All Floor"))
                {
                    FillAll(0);
                }

                if (GUILayout.Button("Fill -> All Wall"))
                {
                    FillAll(1);
                }
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("Room Size", EditorStyles.boldLabel);
            int newWidth = EditorGUILayout.IntField("Width", roomWidth);
            int newHeight = EditorGUILayout.IntField("Height", roomHeight);
            newWidth = Mathf.Clamp(newWidth, MinRoomSize, MaxRoomSize);
            newHeight = Mathf.Clamp(newHeight, MinRoomSize, MaxRoomSize);

            if (newWidth != roomWidth || newHeight != roomHeight)
            {
                roomWidth = newWidth;
                roomHeight = newHeight;
            }

            if (GUILayout.Button("Resize"))
            {
                ResizeGrid(roomWidth, roomHeight, true);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawTilePreviewPanel(MapLayer activeLayer)
        {
            tilePreviewFoldout = EditorGUILayout.Foldout(tilePreviewFoldout, "Wang Tile Preview", true);
            if (!tilePreviewFoldout)
            {
                return;
            }

            if (activeLayer == null || activeLayer.tileSet == null)
            {
                EditorGUILayout.HelpBox("Assign a CornerWangTileSetSO to the active layer to see previews.", MessageType.Info);
                return;
            }

            CornerWangTileSetSO ts = activeLayer.tileSet;
            EditorGUILayout.LabelField("Layer: " + activeLayer.name, EditorStyles.miniLabel);
            EditorGUILayout.LabelField("Lower: " + ts.lowerTerrainLabel + "  |  Upper: " + ts.upperTerrainLabel, EditorStyles.miniLabel);

            const float previewSize = 40f;
            const float labelHeight = 28f;
            const float cellW = previewSize + 4f;
            const float cellH = previewSize + labelHeight;
            const int cols = 4;
            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };

            for (int key = 0; key < CornerKeyNames.Length; key++)
            {
                int col = key % cols;
                if (col == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                }

                int spriteIdx = KeyToIndex[key];
                TileBase tile = ts.tiles != null && spriteIdx >= 0 && spriteIdx < ts.tiles.Length ? ts.tiles[spriteIdx] : null;
                Texture preview = GetTilePreview(tile);
                Color bg = GetTilePreviewBackground(key);
                string tooltip = GetCornerTooltip(key, spriteIdx, activeLayer.name, ts.lowerTerrainLabel, ts.upperTerrainLabel);

                GUILayout.BeginVertical(GUILayout.Width(cellW));
                Rect bgRect = GUILayoutUtility.GetRect(cellW, cellH);
                EditorGUI.DrawRect(bgRect, bg);

                Rect previewRect = new Rect(bgRect.x + 2f, bgRect.y + 2f, previewSize, previewSize);
                if (preview != null)
                {
                    GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
                }
                else
                {
                    EditorGUI.DrawRect(previewRect, new Color(0.15f, 0.15f, 0.15f, 0.8f));
                }

                Rect labelRect = new Rect(bgRect.x, bgRect.y + previewSize + 2f, cellW, labelHeight);
                GUI.Label(labelRect, new GUIContent(CornerKeyNames[key], tooltip), labelStyle);

                GUILayout.EndVertical();

                if (col == cols - 1)
                {
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                Repaint();
            }
        }

        private Texture GetTilePreview(TileBase tile)
        {
            if (tile == null)
            {
                return null;
            }

            Texture preview = AssetPreview.GetAssetPreview(tile);
            if (preview != null)
            {
                return preview;
            }

            return EditorGUIUtility.ObjectContent(tile, typeof(TileBase)).image;
        }

        private Color GetTilePreviewBackground(int key)
        {
            bool isSelectedFloor = currentPaintValue == 0 && key == 0;
            bool isSelectedWall = currentPaintValue != 0 && key == 15;
            if (isSelectedFloor)
            {
                return new Color(0.16f, 0.28f, 0.45f, 0.5f);
            }

            if (isSelectedWall)
            {
                return new Color(0.45f, 0.28f, 0.16f, 0.5f);
            }

            if (key == 0)
            {
                return new Color(0.16f, 0.28f, 0.45f, 0.28f);
            }

            if (key == 15)
            {
                return new Color(0.45f, 0.28f, 0.16f, 0.28f);
            }

            return key < 8 ? new Color(0.16f, 0.28f, 0.45f, 0.2f) : new Color(0.45f, 0.28f, 0.16f, 0.2f);
        }

        private static string GetCornerTooltip(int key, int spriteIdx, string layerName, string lowerLabel, string upperLabel)
        {
            return string.Format(
                "{0}: NW {1}, NE {2}, SW {3}, SE {4}\nLayer: {5}\nLower: {6}\nUpper: {7}\nSprite Index: {8}",
                CornerKeyNames[key],
                (key & 8) != 0 ? "upper" : "lower",
                (key & 4) != 0 ? "upper" : "lower",
                (key & 2) != 0 ? "upper" : "lower",
                (key & 1) != 0 ? "upper" : "lower",
                layerName,
                lowerLabel,
                upperLabel,
                spriteIdx);
        }

        private void DrawGridCanvas(Rect viewRect)
        {
            EditorGUI.DrawRect(viewRect, new Color(0.12f, 0.12f, 0.12f, 1f));

            Handles.BeginGUI();
            Color lineColor = new Color(0.333f, 0.333f, 0.333f, 0.5f);
            Handles.color = lineColor;

            for (int y = 0; y <= roomHeight; y++)
            {
                Vector3 from = VertexToCanvasPosition(0, y);
                Vector3 to = VertexToCanvasPosition(roomWidth, y);
                Handles.DrawLine(from, to);
            }

            for (int x = 0; x <= roomWidth; x++)
            {
                Vector3 from = VertexToCanvasPosition(x, 0);
                Vector3 to = VertexToCanvasPosition(x, roomHeight);
                Handles.DrawLine(from, to);
            }

            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    Handles.color = vertGrid[x, y] == 0 ? HexColor(0x3A, 0x3A, 0x3A) : HexColor(0x7A, 0x4A, 0x2A);
                    Handles.DrawSolidDisc(VertexToCanvasPosition(x, y), Vector3.forward, VertexRadius);
                }
            }

            if (IsValidVertex(hoveredVertex))
            {
                Handles.color = Color.cyan;
                Handles.DrawWireDisc(VertexToCanvasPosition(hoveredVertex.x, hoveredVertex.y), Vector3.forward, VertexRadius + 2f);
            }

            if (isRectangleDragging && IsValidVertex(rectStart) && IsValidVertex(rectCurrent))
            {
                DrawRectangleOverlay(rectStart, rectCurrent);
            }

            Handles.EndGUI();
        }

        private void HandleGridInput(Rect centerRect)
        {
            Event evt = Event.current;
            Vector2 canvasMouse = evt.mousePosition;
            hoveredVertex = GetNearestVertex(canvasMouse);

            if (evt.type == EventType.MouseMove && centerRect.Contains(evt.mousePosition + gridScroll))
            {
                Repaint();
            }

            if (evt.type == EventType.MouseDown && evt.button == 2)
            {
                isPanning = true;
                panStartMouse = evt.mousePosition;
                panStartScroll = gridScroll;
                evt.Use();
                return;
            }

            if (evt.type == EventType.MouseDrag && isPanning && evt.button == 2)
            {
                gridScroll = panStartScroll - (evt.mousePosition - panStartMouse);
                evt.Use();
                Repaint();
                return;
            }

            if (evt.type == EventType.MouseUp && evt.button == 2)
            {
                isPanning = false;
                evt.Use();
                return;
            }

            if (!IsValidVertex(hoveredVertex))
            {
                return;
            }

            if (evt.type == EventType.MouseDown && (evt.button == 0 || evt.button == 1))
            {
                int value = evt.button == 0 ? currentPaintValue : 1 - currentPaintValue;

                if (activeTool == PaintTool.Rectangle)
                {
                    rectStart = hoveredVertex;
                    rectCurrent = hoveredVertex;
                    isRectangleDragging = true;
                }
                else if (activeTool == PaintTool.Fill)
                {
                    FloodFill(hoveredVertex, value);
                }
                else
                {
                    PaintVertex(hoveredVertex, value);
                    isPainting = true;
                }

                evt.Use();
                Repaint();
            }
            else if (evt.type == EventType.MouseDrag && (evt.button == 0 || evt.button == 1))
            {
                int value = evt.button == 0 ? currentPaintValue : 1 - currentPaintValue;

                if (activeTool == PaintTool.Rectangle && isRectangleDragging)
                {
                    rectCurrent = hoveredVertex;
                }
                else if (activeTool == PaintTool.Brush && isPainting)
                {
                    PaintVertex(hoveredVertex, value);
                }

                evt.Use();
                Repaint();
            }
            else if (evt.type == EventType.MouseUp && (evt.button == 0 || evt.button == 1))
            {
                int value = evt.button == 0 ? currentPaintValue : 1 - currentPaintValue;

                if (activeTool == PaintTool.Rectangle && isRectangleDragging)
                {
                    PaintRectangle(rectStart, hoveredVertex, value);
                    isRectangleDragging = false;
                    rectStart = new Vector2Int(-1, -1);
                    rectCurrent = new Vector2Int(-1, -1);
                }

                isPainting = false;
                evt.Use();
                Repaint();
            }
        }

        private Vector2 VertexToCanvasPosition(int x, int y)
        {
            return new Vector2(CanvasPadding + x * cellSize, CanvasPadding + (roomHeight - y) * cellSize);
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

        private void PaintVertex(Vector2Int vertex, int value)
        {
            if (!IsValidVertex(vertex))
            {
                return;
            }

            vertGrid[vertex.x, vertex.y] = Mathf.Clamp(value, 0, 1);
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
                    PaintVertex(new Vector2Int(x, y), value);
                }
            }
        }

        private void FloodFill(Vector2Int start, int value)
        {
            if (!IsValidVertex(start))
            {
                return;
            }

            int target = vertGrid[start.x, start.y];
            value = Mathf.Clamp(value, 0, 1);
            if (target == value)
            {
                return;
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            queue.Enqueue(start);
            vertGrid[start.x, start.y] = value;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                TryFloodNeighbor(queue, current.x + 1, current.y, target, value);
                TryFloodNeighbor(queue, current.x - 1, current.y, target, value);
                TryFloodNeighbor(queue, current.x, current.y + 1, target, value);
                TryFloodNeighbor(queue, current.x, current.y - 1, target, value);
            }
        }

        private void TryFloodNeighbor(Queue<Vector2Int> queue, int x, int y, int target, int value)
        {
            if (x < 0 || y < 0 || x > roomWidth || y > roomHeight || vertGrid[x, y] != target)
            {
                return;
            }

            vertGrid[x, y] = value;
            queue.Enqueue(new Vector2Int(x, y));
        }

        private void DrawRectangleOverlay(Vector2Int start, Vector2Int end)
        {
            Vector2 a = VertexToCanvasPosition(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
            Vector2 b = VertexToCanvasPosition(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y));
            Rect rect = Rect.MinMaxRect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));

            Handles.color = new Color(0f, 1f, 1f, 0.18f);
            Handles.DrawSolidRectangleWithOutline(rect, new Color(0f, 1f, 1f, 0.12f), Color.cyan);
        }

        private static Color HexColor(byte r, byte g, byte b)
        {
            return new Color(r / 255f, g / 255f, b / 255f, 1f);
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
            int[,] oldGrid = vertGrid;
            int[,] newGrid = new int[newWidth + 1, newHeight + 1];

            if (preserve && oldGrid != null)
            {
                int copyWidth = Mathf.Min(oldGrid.GetLength(0), newWidth + 1);
                int copyHeight = Mathf.Min(oldGrid.GetLength(1), newHeight + 1);
                for (int y = 0; y < copyHeight; y++)
                {
                    for (int x = 0; x < copyWidth; x++)
                    {
                        newGrid[x, y] = oldGrid[x, y];
                    }
                }
            }

            roomWidth = newWidth;
            roomHeight = newHeight;
            vertGrid = newGrid;
            Repaint();
        }

        private void FillAll(int value)
        {
            value = Mathf.Clamp(value, 0, 1);
            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    vertGrid[x, y] = value;
                }
            }

            Repaint();
        }

        private void MakeRectangularRoom()
        {
            wallThickness = Mathf.Clamp(wallThickness, 1, Mathf.Min(roomWidth, roomHeight) / 2);

            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    bool isWall = x < wallThickness || y < wallThickness || x > roomWidth - wallThickness || y > roomHeight - wallThickness;
                    vertGrid[x, y] = isWall ? 1 : 0;
                }
            }

            Repaint();
        }

        private void MakeLShapeRoom()
        {
            FillAll(1);

            int splitX = Mathf.Max(wallThickness + 2, Mathf.RoundToInt(roomWidth * 0.58f));
            int splitY = Mathf.Max(wallThickness + 2, Mathf.RoundToInt(roomHeight * 0.52f));

            for (int y = wallThickness; y <= roomHeight - wallThickness; y++)
            {
                for (int x = wallThickness; x <= roomWidth - wallThickness; x++)
                {
                    bool lowerArm = y <= splitY;
                    bool leftArm = x <= splitX;
                    if (lowerArm || leftArm)
                    {
                        vertGrid[x, y] = 0;
                    }
                }
            }

            Repaint();
        }

        private void PerlinNoiseFill()
        {
            float offsetX = noiseSeed * 0.173f;
            float offsetY = noiseSeed * 0.317f;

            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    float sample = Mathf.PerlinNoise(offsetX + x * 0.18f, offsetY + y * 0.18f);
                    vertGrid[x, y] = sample < noiseDensity ? 1 : 0;
                }
            }

            Repaint();
        }

        private void PaintHorizontalCorridor()
        {
            int centerY = roomHeight / 2;
            int half = Mathf.Max(1, wallThickness / 2);
            for (int y = Mathf.Max(0, centerY - half); y <= Mathf.Min(roomHeight, centerY + half); y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    vertGrid[x, y] = 0;
                }
            }

            Repaint();
        }

        private void PaintVerticalCorridor()
        {
            int centerX = roomWidth / 2;
            int half = Mathf.Max(1, wallThickness / 2);
            for (int x = Mathf.Max(0, centerX - half); x <= Mathf.Min(roomWidth, centerX + half); x++)
            {
                for (int y = 0; y <= roomHeight; y++)
                {
                    vertGrid[x, y] = 0;
                }
            }

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

            MapSaveData data = new MapSaveData
            {
                width = roomWidth,
                height = roomHeight,
                vertexData = FlattenGrid(),
                layerNames = GetLayerNames()
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
            if (data == null || data.vertexData == null)
            {
                Debug.LogError("[MapDesigner] Invalid map data: " + absolutePath);
                return;
            }

            roomWidth = Mathf.Clamp(data.width, MinRoomSize, MaxRoomSize);
            roomHeight = Mathf.Clamp(data.height, MinRoomSize, MaxRoomSize);
            ResizeGrid(roomWidth, roomHeight, false);
            UnflattenGrid(data.vertexData);

            if (data.layerNames != null)
            {
                for (int i = 0; i < Mathf.Min(data.layerNames.Length, layers.Count); i++)
                {
                    layers[i].name = data.layerNames[i];
                }
            }

            Debug.Log("[MapDesigner] Loaded map data: " + absolutePath);
            Repaint();
        }

        private int[] FlattenGrid()
        {
            int[] values = new int[(roomWidth + 1) * (roomHeight + 1)];
            int index = 0;

            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    values[index++] = vertGrid[x, y];
                }
            }

            return values;
        }

        private void UnflattenGrid(int[] values)
        {
            int index = 0;
            for (int y = 0; y <= roomHeight; y++)
            {
                for (int x = 0; x <= roomWidth; x++)
                {
                    vertGrid[x, y] = index < values.Length ? Mathf.Clamp(values[index], 0, 1) : 0;
                    index++;
                }
            }
        }

        private string[] GetLayerNames()
        {
            string[] names = new string[layers.Count];
            for (int i = 0; i < layers.Count; i++)
            {
                names[i] = layers[i].name;
            }

            return names;
        }

        private void ApplyToScene()
        {
            int applied = 0;
            foreach (MapLayer layer in layers)
            {
                if (!layer.enabled || layer.tilemap == null || layer.tileSet == null)
                {
                    continue;
                }

                CornerWangPainter.Paint(layer.tilemap, layer.tileSet, vertGrid, roomWidth, roomHeight);
                EditorUtility.SetDirty(layer.tilemap);
                applied++;
            }

            Debug.Log("[MapDesigner] Applied " + roomWidth + "x" + roomHeight + " map to " + applied + " layer(s).");
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
            MapLayer layer = layers != null && layers.Count > 0 ? layers[Mathf.Clamp(activeLayerIndex, 0, layers.Count - 1)] : null;
            string layerName = layer != null ? layer.name : "None";
            string tileSetName = layer != null && layer.tileSet != null ? layer.tileSet.name : "No Tileset";
            string status = string.Format(
                "Room: {0}x{1} | Vertices: {2}x{3} | Active Layer: {4} ({5}) | Tool: {6}",
                roomWidth,
                roomHeight,
                roomWidth + 1,
                roomHeight + 1,
                layerName,
                tileSetName,
                activeTool);

            EditorGUI.LabelField(new Rect(rect.x + 8f, rect.y + 2f, rect.width - 16f, rect.height - 4f), status, EditorStyles.miniLabel);
        }
    }
}
