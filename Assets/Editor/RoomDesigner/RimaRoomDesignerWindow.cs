namespace RIMA.Editor.RoomDesigner
{
    using System;
    using System.IO;
    using RIMA.Editor.RoomDesigner.Brushes;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;

    public sealed class RimaRoomDesignerWindow : EditorWindow, IRoomDesignerContext
    {
        private const string UxmlPath = "Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uxml";
        private const string UssPath = "Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss";
        private const string McpResponsePath = "Assets/Editor/RoomDesigner/McpResponses";
        private const double PollIntervalSeconds = 0.5d;

        private RoomDesignerCanvas canvas;
        private VisualElement leftPanel;
        private VisualElement rightPanel;
        private VisualElement canvasHost;
        private Label cellDebugLabel;
        private DropdownField activeLayerDropdown;
        private BrushController brushController;
        private bool isCanvasHovered;
        private bool isDirty;
        private bool isStrokeActive;
        private double nextPollTime;

        [MenuItem("RIMA/Room Designer")]
        public static void Open()
        {
            var window = GetWindow<RimaRoomDesignerWindow>();
            window.titleContent = new GUIContent("Room Designer");
            window.minSize = new Vector2(960f, 560f);
            window.Show();
        }

        public Tilemap FloorTilemap => canvas?.FloorTilemap;
        public Tilemap WallsTilemap => canvas?.WallsTilemap;
        public Tilemap DecalsTilemap => canvas?.DecalsTilemap;

        public RoomLayer ActiveLayer { get; set; } = RoomLayer.Floor;
        public TileBase ActiveTile { get; set; }
        public BrushMode ActiveBrush { get; set; } = BrushMode.Stamp;

        private Vector3Int hoveredCell;

        public Vector3Int HoveredCell
        {
            get => hoveredCell;
            set
            {
                if (hoveredCell == value)
                {
                    return;
                }

                hoveredCell = value;
                UpdateCellLabel();
                MarkDirty();
            }
        }

        public bool IsCanvasHovered => isCanvasHovered;
        public bool IsDirty => isDirty;
        public VisualElement LeftPanel => leftPanel;
        public VisualElement RightPanel => rightPanel;

        public Tilemap GetActiveTilemap()
        {
            switch (ActiveLayer)
            {
                case RoomLayer.Walls:
                    return WallsTilemap;
                case RoomLayer.Decals:
                    return DecalsTilemap;
                case RoomLayer.Floor:
                default:
                    return FloorTilemap;
            }
        }

        public void InvokeBrush(int mouseButton, Vector3Int cell)
        {
            if (brushController == null)
            {
                return;
            }

            if (!isStrokeActive)
            {
                brushController.OnInvoke(this, mouseButton, cell);
                isStrokeActive = true;
            }
            else
            {
                brushController.OnDrag(this, cell);
            }

            MarkDirty();
        }

        public void OnBrushRelease()
        {
            if (!isStrokeActive)
            {
                return;
            }

            brushController?.OnRelease(this);
            isStrokeActive = false;
        }

        public void MarkDirty()
        {
            isDirty = true;
            Repaint();
        }

        public void SetCanvasHovered(bool hovered)
        {
            isCanvasHovered = hovered;
        }

        public bool ConsumeDirty()
        {
            if (!isDirty)
            {
                return false;
            }

            isDirty = false;
            return true;
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            if (visualTree == null)
            {
                rootVisualElement.Add(new Label("RoomDesignerWindow.uxml missing"));
                return;
            }

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);
            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }

            visualTree.CloneTree(rootVisualElement);

            leftPanel = rootVisualElement.Q<VisualElement>("left-panel");
            rightPanel = rootVisualElement.Q<VisualElement>("right-panel");
            canvasHost = rootVisualElement.Q<VisualElement>("canvas-host");

            BindToolbar();
            EnsureCanvas();

            canvasHost?.Clear();
            if (canvasHost != null && canvas != null)
            {
                canvasHost.Add(canvas.Element);
            }

            UpdateCellLabel();
            EnsureBrushController();
        }

        private void OnEnable()
        {
            ActiveBrush = BrushMode.Stamp;
            ActiveLayer = RoomLayer.Floor;
            HoveredCell = Vector3Int.zero;
            EnsureCanvas();
            EditorApplication.update += PollMcp;
        }

        private void OnDisable()
        {
            EditorApplication.update -= PollMcp;
            canvas?.Dispose();
            canvas = null;
            brushController = null;
            isStrokeActive = false;
        }

        private void EnsureCanvas()
        {
            if (canvas == null)
            {
                canvas = new RoomDesignerCanvas(this);
            }
        }

        private void EnsureBrushController()
        {
            if (brushController == null)
            {
                brushController = new BrushController();
            }

            if (rightPanel != null)
            {
                brushController.Initialize(this);
            }
        }

        private void BindToolbar()
        {
            var saveButton = rootVisualElement.Q<Button>("btn-save");
            if (saveButton != null)
            {
                saveButton.clicked += SaveCurrentRoom;
            }

            var newButton = rootVisualElement.Q<Button>("btn-new");
            if (newButton != null)
            {
                newButton.clicked += NewRoom;
            }

            activeLayerDropdown = rootVisualElement.Q<DropdownField>("active-layer");
            if (activeLayerDropdown != null)
            {
                activeLayerDropdown.choices = new System.Collections.Generic.List<string>(Enum.GetNames(typeof(RoomLayer)));
                activeLayerDropdown.value = ActiveLayer.ToString();
                activeLayerDropdown.RegisterValueChangedCallback(evt =>
                {
                    if (Enum.TryParse(evt.newValue, out RoomLayer parsed))
                    {
                        ActiveLayer = parsed;
                        MarkDirty();
                    }
                });
            }

            var toolbar = rootVisualElement.Q<VisualElement>("toolbar");
            if (toolbar != null)
            {
                cellDebugLabel = new Label();
                cellDebugLabel.name = "cell-debug";
                cellDebugLabel.AddToClassList("rd-cell-debug");
                toolbar.Add(cellDebugLabel);
            }
        }

        private void SaveCurrentRoom()
        {
            if (canvas?.StageRoot == null)
            {
                Debug.LogWarning("Room Designer save skipped: no stage root.");
                return;
            }

            try
            {
                var saved = RoomSaver.Save(canvas.StageRoot, "test_room", "_TEST");
                Debug.Log($"Room Designer saved: {saved.prefabPath}, {saved.blueprintPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Room Designer save failed: {ex.Message}");
            }
        }

        private void NewRoom()
        {
            canvas?.ClearTilemaps();
            HoveredCell = Vector3Int.zero;
            MarkDirty();
        }

        private void UpdateCellLabel()
        {
            if (cellDebugLabel == null)
            {
                return;
            }

            cellDebugLabel.text = $"cell: ({hoveredCell.x},{hoveredCell.y})";
        }

        private void PollMcp()
        {
            double now = EditorApplication.timeSinceStartup;
            if (now < nextPollTime)
            {
                return;
            }

            nextPollTime = now + PollIntervalSeconds;
            if (!Directory.Exists(McpResponsePath))
            {
                return;
            }

#if ROOM_DESIGNER_DEBUG_PERF
            int count = Directory.GetFiles(McpResponsePath, "*.json", SearchOption.TopDirectoryOnly).Length;
            Debug.Log($"Room Designer MCP poll: {count} json response file(s)");
#endif
        }
    }
}
