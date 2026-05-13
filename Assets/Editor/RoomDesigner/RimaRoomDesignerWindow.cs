namespace RIMA.Editor.RoomDesigner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using RIMA.Editor.RoomDesigner.Brushes;
    using RIMA.RoomDesigner.Core;
    using RIMA.RoomDesigner.Core.Editor;
    using RIMA.Runtime.Rooms;
    using RIMA.Systems.Map;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;
    using Object = UnityEngine.Object;

    public sealed class RimaRoomDesignerWindow : EditorWindow, IRoomDesignerContext
    {
        private const string UxmlPath = "Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uxml";
        private const string UssPath = "Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss";
        private const string McpResponsePath = "Assets/Editor/RoomDesigner/McpResponses";
        private const string RoomDesignerLayerName = "RoomDesigner";
        private const double PollIntervalSeconds = 0.5d;

        private RoomDesignerCanvas canvas;
        private VisualElement leftPanel;
        private VisualElement rightPanel;
        private VisualElement canvasHost;
        private Label cellDebugLabel;
        private DropdownField activeLayerDropdown;
        private BrushController brushController;
        private RoomBlueprint activeBp;
        private bool isCanvasHovered;
        private bool isDirty;
        private bool isStrokeActive;
        private PixelPerfectCanvasPreview ppPreview;
        private double nextPollTime;

        [SerializeField] private RimaRoomBaselineTemplate activeTemplate;

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
        public RoomBlueprint ActiveBlueprint => activeBp;
        public bool IsWallOverrideMode { get; set; }

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
            EnsureRoomDesignerLayer();
            ActiveBrush = BrushMode.Stamp;
            ActiveLayer = RoomLayer.Floor;
            HoveredCell = Vector3Int.zero;
            if (activeBp == null)
                activeBp = ScriptableObject.CreateInstance<RoomBlueprint>();
            EnsureCanvas();
            ppPreview = new PixelPerfectCanvasPreview(this);
            EditorApplication.update += PollMcp;
        }

        private void OnDisable()
        {
            EditorApplication.update -= PollMcp;
            ppPreview?.Dispose();
            ppPreview = null;
            canvas?.Dispose();
            canvas = null;
            brushController = null;
            isStrokeActive = false;
            if (activeBp != null)
            {
                DestroyImmediate(activeBp);
                activeBp = null;
            }
        }

        private void EnsureCanvas()
        {
            if (canvas == null)
            {
                EnsureRoomDesignerLayer();
                canvas = new RoomDesignerCanvas(this);
            }
        }

        private static int EnsureRoomDesignerLayer()
        {
            int existingLayer = LayerMask.NameToLayer(RoomDesignerLayerName);
            if (existingLayer >= 0)
            {
                return existingLayer;
            }

            Object[] tagManagerAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (tagManagerAssets == null || tagManagerAssets.Length == 0)
            {
                Debug.LogWarning("Room Designer could not load ProjectSettings/TagManager.asset to create preview layer.");
                return -1;
            }

            var tagManager = new SerializedObject(tagManagerAssets[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            if (layers == null || !layers.isArray)
            {
                Debug.LogWarning("Room Designer could not find Unity layer settings in TagManager.asset.");
                return -1;
            }

            for (int i = 31; i >= 8; i--)
            {
                SerializedProperty slot = layers.GetArrayElementAtIndex(i);
                if (!string.IsNullOrEmpty(slot.stringValue))
                {
                    continue;
                }

                slot.stringValue = RoomDesignerLayerName;
                tagManager.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
                return i;
            }

            Debug.LogWarning("Room Designer could not create the RoomDesigner Unity layer: no free user layer slot.");
            return -1;
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

            var generateButton = rootVisualElement.Q<Button>("btn-generate");
            if (generateButton != null)
            {
                generateButton.clicked += GenerateRoom;
            }

            var decalBtn = rootVisualElement.Q<Button>("btn-paint-decals");
            if (decalBtn != null)
            {
                decalBtn.clicked += PaintDecals;
            }

            var propBtn = rootVisualElement.Q<Button>("btn-place-props");
            if (propBtn != null)
            {
                propBtn.clicked += PlacePropsAction;
            }

            var transBtn = rootVisualElement.Q<Button>("btn-apply-transitions");
            if (transBtn != null)
            {
                transBtn.clicked += ApplyTransitionsAction;
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
                if (toolbar.Q<ObjectField>("active-template") == null)
                {
                    var templateField = new ObjectField("Template")
                    {
                        name = "active-template",
                        objectType = typeof(RimaRoomBaselineTemplate),
                        value = activeTemplate
                    };
                    templateField.style.minWidth = 220f;
                    templateField.RegisterValueChangedCallback(evt =>
                    {
                        activeTemplate = evt.newValue as RimaRoomBaselineTemplate;
                        MarkDirty();
                    });
                    toolbar.Add(templateField);
                }

                cellDebugLabel = new Label();
                cellDebugLabel.name = "cell-debug";
                cellDebugLabel.AddToClassList("rd-cell-debug");
                toolbar.Add(cellDebugLabel);

                var ppBtn = new Button(() => { ppPreview?.Toggle(); MarkDirty(); });
                ppBtn.text = "[Pixel-Perfect Preview] OFF";
                ppBtn.name = "btn-pixel-perfect";
                toolbar.Add(ppBtn);
                ppPreview?.SetLabelRef(ppBtn);
            }
        }

        private void GenerateRoom()
        {
            if (activeTemplate == null)
            {
                Debug.LogError("Room Designer generate failed: assign a RimaRoomBaselineTemplate.");
                return;
            }

            if (activeTemplate.floorVariants == null || activeTemplate.floorVariants.Length == 0)
            {
                Debug.LogError("Room Designer generate failed: template floorVariants must contain at least one tile.");
                return;
            }

            if (activeTemplate.wallVariants == null || activeTemplate.wallVariants.Length == 0)
            {
                Debug.LogError("Room Designer generate failed: template wallVariants must contain at least one tile.");
                return;
            }

            EnsureCanvas();
            if (FloorTilemap == null || WallsTilemap == null)
            {
                Debug.LogError("Room Designer generate failed: tilemaps are not ready.");
                return;
            }

            string roomId = "room_001";
            int noiseSeed = activeBp != null ? activeBp.noiseSeed : 0;
            int gateCount = activeBp != null ? activeBp.gateCount : 2;
            var metaPanel = rightPanel?.Q<RoomMetadataPanel>(RoomMetadataPanel.PanelName);
            if (metaPanel != null)
            {
                var data = metaPanel.GetBlueprintData();
                roomId = data.roomId;
                noiseSeed = data.noiseSeed;
                gateCount = data.gateCount;
            }

            if (activeBp == null)
            {
                activeBp = ScriptableObject.CreateInstance<RoomBlueprint>();
            }

            int minWidth = Mathf.Max(3, activeTemplate.minWidth);
            int maxWidth = Mathf.Max(minWidth, activeTemplate.maxWidth);
            int minHeight = Mathf.Max(3, activeTemplate.minHeight);
            int maxHeight = Mathf.Max(minHeight, activeTemplate.maxHeight);
            int width = Mathf.Clamp(activeBp.roomWidth > 0 ? activeBp.roomWidth : maxWidth, minWidth, maxWidth);
            int height = Mathf.Clamp(activeBp.roomHeight > 0 ? activeBp.roomHeight : maxHeight, minHeight, maxHeight);

            activeBp.roomId = roomId;
            activeBp.roomType = RoomType.Combat.ToString();
            activeBp.biomeType = activeTemplate.biome;
            activeBp.noiseSeed = noiseSeed;
            activeBp.gateCount = gateCount;
            activeBp.roomWidth = width;
            activeBp.roomHeight = height;

            var input = new GenerationInput(
                noiseSeed,
                activeTemplate.biome.ToString(),
                activeTemplate.archetypeId,
                width,
                height,
                activeTemplate.generatorVersion);

            GridSnapshot snapshot = CoreRoomBaselineRunner.Run(new RimaRoomBaselineGenerator(), input, FloorTilemap, WallsTilemap);
            activeBp.roomOrigin = snapshot.origin;
            activeBp.floorVariantIndex = new byte[width * height];
            activeBp.wallVariantIndex = new byte[width * height];
            activeBp.overrideVariantIndex = new bool[width * height];

            FloorVariantPainter.BakeVariants(FloorTilemap, activeBp, activeTemplate.floorVariants);

            List<Vector3Int> allWallCells = CollectOccupiedCells(WallsTilemap);
            WallAutoConnect.RefreshNeighborhood(WallsTilemap, allWallCells, activeTemplate.wallVariants, activeBp);
            WallAutoConnect.BakeWallVariants(WallsTilemap, activeBp, activeTemplate.wallVariants);

            MarkDirty();
        }

        private void PaintDecals()
        {
            if (activeTemplate == null)
            {
                Debug.LogError("Room Designer decal paint failed: assign a RimaRoomBaselineTemplate.");
                return;
            }

            if (activeBp == null)
            {
                Debug.LogError("Room Designer decal paint failed: generate a room first.");
                return;
            }

            EnsureCanvas();
            if (DecalsTilemap == null)
            {
                Debug.LogError("Room Designer decal paint failed: DecalsTilemap is not ready.");
                return;
            }

            if (DecalPainter.PaintDecals(DecalsTilemap, activeBp, activeTemplate.decalSprites, activeBp.noiseSeed, activeTemplate.decalDensity))
            {
                MarkDirty();
            }
        }

        private void PlacePropsAction()
        {
            if (activeTemplate == null)
            {
                Debug.LogError("Room Designer prop placement failed: assign a RimaRoomBaselineTemplate.");
                return;
            }

            if (activeBp == null)
            {
                Debug.LogError("Room Designer prop placement failed: generate a room first.");
                return;
            }

            EnsureCanvas();
            if (canvas?.StageRoot == null)
            {
                Debug.LogError("Room Designer prop placement failed: stage root is not ready.");
                return;
            }

            AnchorZone[] anchors = RimaArchetypeGenerators.GetDefaultAnchorZones(
                activeTemplate.archetypeId,
                activeBp.noiseSeed,
                activeBp.roomWidth,
                activeBp.roomHeight);
            List<GameObject> placed = PropPlacer.PlaceProps(canvas.StageRoot, anchors, activeTemplate.propSpecs, activeBp, activeBp.noiseSeed);
            if (placed.Count > 0)
            {
                MarkDirty();
            }
        }

        private void ApplyTransitionsAction()
        {
            if (activeTemplate == null)
            {
                Debug.LogError("Room Designer transition apply failed: assign a RimaRoomBaselineTemplate.");
                return;
            }

            if (activeBp == null)
            {
                Debug.LogError("Room Designer transition apply failed: generate a room first.");
                return;
            }

            EnsureCanvas();
            if (FloorTilemap == null)
            {
                Debug.LogError("Room Designer transition apply failed: FloorTilemap is not ready.");
                return;
            }

            if (BiomeTransitionPainter.ApplyBiomeTransitions(FloorTilemap, activeBp, activeTemplate.floorVariants))
            {
                MarkDirty();
            }
        }

        private static List<Vector3Int> CollectOccupiedCells(Tilemap tilemap)
        {
            var cells = new List<Vector3Int>();
            if (tilemap == null)
            {
                return cells;
            }

            BoundsInt bounds = tilemap.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var cell = new Vector3Int(x, y, 0);
                    if (tilemap.GetTile(cell) != null)
                    {
                        cells.Add(cell);
                    }
                }
            }

            return cells;
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
                string roomId = "room_001";
                BiomeType biome = BiomeType.Keep;
                int noiseSeed = 0;
                int gateCount = 2;

                var metaPanel = rightPanel?.Q<RoomMetadataPanel>(RoomMetadataPanel.PanelName);
                if (metaPanel != null)
                {
                    (roomId, biome, noiseSeed, gateCount) = metaPanel.GetBlueprintData();
                }

                int width = activeBp != null ? activeBp.roomWidth : 20;
                int height = activeBp != null ? activeBp.roomHeight : 20;
                var saved = RoomSaver.Save(canvas.StageRoot, roomId, biome, noiseSeed, width, height);
                Debug.Log($"Room Designer saved: {saved.prefabPath}, {saved.blueprintPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Room Designer save failed: {ex.Message}");
            }
        }

        private void NewRoom()
        {
            if (activeBp != null)
                DestroyImmediate(activeBp);
            activeBp = ScriptableObject.CreateInstance<RoomBlueprint>();
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

        private void OnGUI()
        {
            if (ppPreview == null || canvas == null) return;
            var el = canvas.Element;
            if (el != null) ppPreview.DrawOverlay(el.worldBound);
        }
    }
}
