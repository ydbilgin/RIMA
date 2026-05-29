// TARGET: Assets/Scripts/LiveTool/ToolBootstrap.cs
// ─────────────────────────────────────────────────────────────────────────────
// C5 — ToolBootstrap (T3 Live Editor Tool.exe entry point).
//
// Single MonoBehaviour that owns the whole Tool lifecycle. On Start() it:
//   1. loads the baked RuntimeAssetRegistry (Resources/Live/RuntimeAssetRegistry),
//   2. builds the runtime RuntimeBrushPalette model,
//   3. binds the ToolMain.uxml panel (left palette / center preview / right inspector),
//   4. populates the thumbnail grid + wires palette ClickEvents,
//   5. routes pointer + keyboard input from preview-canvas into the runtime
//      BrushExecutorRouter (paint / erase) and RuntimeColliderHandles (C7),
//   6. drives RuntimeCliffHoverIndicator (C8) in Cliff mode,
//   7. serializes the in-memory RoomLayoutData → StreamingAssets/live/room_current.json
//      (the file Game.exe's LiveRoomReloader watches), using the .lock protocol.
//
// Runtime-legal: ZERO UnityEditor / Handles / Gizmos. Compiles into Tool.exe via
// the RIMA.LiveTool asmdef (defineConstraints: ["RIMA_LIVE_TOOL"]) — stripped from
// the shipping Game.exe (0 byte cost) because that build does not set the define.
//
// Contract: BUILD CONTRACT §2 C5, §3 F3, §4 cross-cutting wiring constraints.
// Source-verified API references are cited inline (file:line).
// ─────────────────────────────────────────────────────────────────────────────

#if RIMA_LIVE_TOOL

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;          // Mouse.current / Keyboard.current (no legacy Input.Get*)
using RIMA.Live;                        // RuntimeAssetRegistry, RegistryEntry, RoomLayoutData + sub-models
using RIMA.RoomPainter;                 // RoomLayer, ColliderShape

namespace RIMA.LiveTool
{
    /// <summary>
    /// Tool.exe entry point. The only MonoBehaviour driving the live-edit session.
    /// Attached to the <c>[ToolRoot]</c> GameObject in ToolMain.unity.
    /// </summary>
    public sealed class ToolBootstrap : MonoBehaviour
    {
        // ── Inspector wiring (set in ToolMain.unity) ───────────────────────────

        [Header("Scene wiring (ToolMain.unity)")]
        [SerializeField] private UIDocument uiDocument;     // ToolMain.uxml + PanelSettings host
        [SerializeField] private Camera previewCamera;      // orthographic, PPU 64, renders the room
        [SerializeField] private Grid grid;                 // cellSize (1,1,1) rectangular — cell math source
        [SerializeField] private Tilemap floorTilemap;      // [LiveProps] floor target
        [SerializeField] private Tilemap cliffTilemap;      // cliff target (name contains "cliff")
        [SerializeField] private Transform propRoot;        // parent for spawned prop instances

        // ── Public API (BUILD CONTRACT §2 C5 exact signatures) ─────────────────

        /// <summary>Runtime palette model (C6 twin). Null until Start() succeeds.</summary>
        public RuntimeBrushPalette Palette { get; private set; }

        /// <summary>The currently selected brush entry, or null. Mirrors Palette.SelectedEntry.</summary>
        public RegistryEntry SelectedEntry => Palette?.SelectedEntry;

        /// <summary>Current paint mode (drives palette filter + which router branch runs).</summary>
        public PaletteMode CurrentMode { get; private set; } = PaletteMode.All;

        /// <summary>Brush rotation in degrees, snapped to 90° steps via the R key.
        /// Parity with VisualEditorScenePainter rotation handling.</summary>
        public float CurrentRotation { get; private set; }

        // ── Internal state ─────────────────────────────────────────────────────

        // Save path: MUST equal LiveRoomReloader.JsonPath (LiveRoomReloader.cs:45) and
        // RoomLayoutSerializer.CurrentJsonPath (RoomLayoutSerializer.cs:17).
        private static string JsonPath =>
            Path.Combine(Application.streamingAssetsPath, "live", "room_current.json");
        private static string LockPath => JsonPath + ".lock";

        // Schema version accepted by RoomLayoutSerializer.IsCompatibleSchema (:140) — "1.1".
        private const string SchemaVersion = "1.1";

        private RuntimeAssetRegistry _registry;

        // Sibling runtime systems (C7 / C8 / runtime router). Built in Start().
        private BrushExecutorRouter _router;            // RIMA.LiveTool runtime router (NEW, not the Editor one)
        private RuntimeColliderHandles _colliderHandles; // C7-runtime
        private RuntimeCliffHoverIndicator _cliffHover;  // C8

        // The live, in-memory truth that RequestSave() serializes.
        private RoomLayoutData _liveDoc;

        // Currently selected prop instance for the collider inspector (C7 target).
        private GameObject _selectedInstance;
        private RegistryEntry _selectedInstanceEntry;

        // ── UXML element handles (bound in BindUi) ─────────────────────────────

        private VisualElement _root;
        private VisualElement _leftPanel;
        private VisualElement _centerPanel;
        private VisualElement _rightPanel;
        private VisualElement _previewCanvas;       // pointer target + C7 handle / C8 hover parent
        private DropdownField _modeDropdown;
        private TextField _searchField;
        private ScrollView _thumbnailGrid;
        private Label _selectedNameLabel;
        private DropdownField _colliderShapeDropdown;
        private FloatField _colliderSizeX;
        private FloatField _colliderSizeY;
        private Button _saveButton;
        private Button _openInEditorButton;
        private Label _bannerLabel;                  // "Registry not baked" abort banner

        // Painting drag state.
        private bool _isPainting;
        private bool _isErasing;

        // Re-entrancy guard so a save triggered mid-paint doesn't double-write.
        private bool _saveQueued;

        // ── Bootstrap ──────────────────────────────────────────────────────────

        private void Start()
        {
            // 1) Load baked registry. Abort to an on-screen banner if missing.
            _registry = RuntimeAssetRegistry.Instance;   // RuntimeAssetRegistry.cs:123 (Resources.Load + EnsureInitialized)
            if (_registry == null || _registry.Count == 0)
            {
                ShowAbortBanner(
                    "Registry not baked.\n" +
                    "Run RIMA → Live Tool → Bake Asset Registry in the Editor, then rebuild Tool.exe.");
                Debug.LogError("[ToolBootstrap] RuntimeAssetRegistry not found or empty — aborting.");
                return;
            }

            // 2) Build the runtime palette twin and bind the registry.
            Palette = new RuntimeBrushPalette();
            Palette.SetRegistry(_registry);              // RuntimeBrushPalette.cs:47
            Palette.SetMode(CurrentMode);

            // 3) Seed / load the live document.
            _liveDoc = LoadOrCreateLiveDoc();

            // 4) Build runtime systems (router + collider handles + cliff hover).
            BuildRuntimeSystems();

            // 5) Bind UXML and populate.
            if (!BindUi())
            {
                ShowAbortBanner("ToolMain.uxml failed to bind — UIDocument has no rootVisualElement.");
                Debug.LogError("[ToolBootstrap] UIDocument.rootVisualElement was null.");
                return;
            }

            PopulateThumbnailGrid();
            RefreshInspector();

            Debug.Log($"[ToolBootstrap] Ready — {_registry.Count} registry entries, doc '{_liveDoc.room_id}'.");
        }

        private void BuildRuntimeSystems()
        {
            // Runtime paint dispatcher (NEW minimal router — BUILD CONTRACT §0.1 / BrushExecutorRouter section).
            // ctor: (Tilemap floorTilemap, Tilemap cliffTilemap, Transform propRoot, RuntimeAssetRegistry registry)
            _router = new BrushExecutorRouter(floorTilemap, cliffTilemap, propRoot, _registry);

            // C7 collider handles (created now; canvas bound after BindUi). new() — MonoBehaviour-free per its runtime API.
            _colliderHandles = new RuntimeColliderHandles();

            // C8 cliff hover indicator — a SpriteRenderer GameObject. Create + initialize.
            var hoverGo = new GameObject("[CliffHoverIndicator]");
            hoverGo.transform.SetParent(transform, false);
            _cliffHover = hoverGo.AddComponent<RuntimeCliffHoverIndicator>();
            _cliffHover.Initialize(previewCamera, _registry);   // C8 API
            _cliffHover.SetActive(false);
        }

        // ── Public API methods (contract signatures) ───────────────────────────

        /// <summary>Select a brush entry (called by palette thumbnail ClickEvent).</summary>
        public void SelectEntry(RegistryEntry entry)
        {
            Palette.Select(entry);                       // RuntimeBrushPalette.cs:93
            if (CurrentMode == PaletteMode.Cliff && entry != null)
                _cliffHover.SetVariant(entry);           // C8: preview sprite at cursor
            HighlightSelectedThumb(entry);
            RefreshInspector();
        }

        /// <summary>Change the active paint mode (palette filter + canvas behaviour).</summary>
        public void SetMode(PaletteMode mode)
        {
            CurrentMode = mode;
            Palette.SetMode(mode);                       // RuntimeBrushPalette.cs:57
            _cliffHover.SetActive(mode == PaletteMode.Cliff);
            if (mode == PaletteMode.Cliff && SelectedEntry != null)
                _cliffHover.SetVariant(SelectedEntry);
            PopulateThumbnailGrid();                     // re-filter
            RefreshInspector();
        }

        /// <summary>
        /// Serialize the current in-memory RoomLayoutData → room_current.json using the
        /// lock protocol JsonFileWatcher waits on (JsonFileWatcher.cs:165):
        /// write .lock → write JSON → delete .lock. Uses JsonUtility (NOT Newtonsoft) so
        /// field names/format match RoomLayoutData.FromJson (RoomLayoutData.cs:40) exactly.
        /// </summary>
        public void RequestSave()
        {
            if (_liveDoc == null) return;

            // Stamp metadata (created preserved if present).
            string now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            _liveDoc.schema_version = SchemaVersion;
            if (_liveDoc.metadata == null) _liveDoc.metadata = new RoomLayoutMeta { created = now };
            _liveDoc.metadata.modified = now;
            if (string.IsNullOrEmpty(_liveDoc.metadata.created)) _liveDoc.metadata.created = now;

            try
            {
                string dir = Path.GetDirectoryName(JsonPath);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

                // 1) acquire lock so the watcher waits until JSON is fully written.
                File.WriteAllText(LockPath, now);

                // 2) write JSON (snake_case fields are declared on RoomLayoutData → JsonUtility matches the reader).
                string json = JsonUtility.ToJson(_liveDoc, prettyPrint: true);
                File.WriteAllText(JsonPath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ToolBootstrap] Save failed: {ex.Message}");
            }
            finally
            {
                // 3) release lock.
                try { if (File.Exists(LockPath)) File.Delete(LockPath); }
                catch (Exception ex) { Debug.LogWarning($"[ToolBootstrap] Lock delete failed: {ex.Message}"); }
            }
        }

        /// <summary>Ctrl+Z — collider drag undo takes priority, else a paint undo (paint undo deferred).</summary>
        public void Undo()
        {
            // Collider handle undo (C7) consumes the event when it has history.
            if (_colliderHandles != null && _colliderHandles.Undo())
            {
                RequestSave();
                return;
            }
            // ASSUMPTION: tile/prop paint undo is out of C5 scope for the first Tool.exe
            // (BUILD CONTRACT §0.1 "lean", §5.3 "70% of Editor quality, accepted"). The
            // runtime router intentionally does NOT replicate the Editor Undo pipeline.
            // Left as a no-op beyond collider undo; flagged so a follow-up can add it.
        }

        // ── UI binding ─────────────────────────────────────────────────────────

        private bool BindUi()
        {
            _root = uiDocument != null ? uiDocument.rootVisualElement : null;
            if (_root == null) return false;

            // Panels.
            _leftPanel    = _root.Q<VisualElement>("left-panel");
            _centerPanel  = _root.Q<VisualElement>("center-panel");
            _rightPanel   = _root.Q<VisualElement>("right-panel");
            _previewCanvas = _root.Q<VisualElement>("preview-canvas");

            // Left panel controls.
            _modeDropdown  = _root.Q<DropdownField>("mode-dropdown");
            _searchField   = _root.Q<TextField>("search-field");
            _thumbnailGrid = _root.Q<ScrollView>("thumbnail-grid");

            // Right panel (inspector).
            _selectedNameLabel    = _root.Q<Label>("selected-name");
            _colliderShapeDropdown = _root.Q<DropdownField>("collider-shape");
            _colliderSizeX        = _root.Q<FloatField>("collider-size-x");
            _colliderSizeY        = _root.Q<FloatField>("collider-size-y");
            _saveButton           = _root.Q<Button>("save-room");
            _openInEditorButton   = _root.Q<Button>("open-in-editor");

            // Mode dropdown: All / Tile / Cliff / Decor / Object (PaletteMode order, RuntimeBrushPalette.cs:160).
            if (_modeDropdown != null)
            {
                _modeDropdown.choices = new List<string>(Enum.GetNames(typeof(PaletteMode)));
                _modeDropdown.index = (int)CurrentMode;
                _modeDropdown.RegisterValueChangedCallback(evt =>
                {
                    if (Enum.TryParse(evt.newValue, out PaletteMode mode)) SetMode(mode);
                });
            }

            // Search field.
            if (_searchField != null)
            {
                _searchField.RegisterValueChangedCallback(evt =>
                {
                    Palette.SetSearch(evt.newValue);     // RuntimeBrushPalette.cs:81
                    PopulateThumbnailGrid();
                });
            }

            // Collider shape dropdown: Box / Circle / Capsule (Polygon deferred — ColliderShape, RoomPainterAsset.cs:6).
            if (_colliderShapeDropdown != null)
            {
                _colliderShapeDropdown.choices = new List<string>
                {
                    ColliderShape.Box.ToString(),
                    ColliderShape.Circle.ToString(),
                    ColliderShape.Capsule.ToString(),
                };
                _colliderShapeDropdown.RegisterValueChangedCallback(OnColliderShapeChanged);
            }

            if (_colliderSizeX != null) _colliderSizeX.RegisterValueChangedCallback(_ => OnColliderSizeChanged());
            if (_colliderSizeY != null) _colliderSizeY.RegisterValueChangedCallback(_ => OnColliderSizeChanged());

            // Buttons.
            if (_saveButton != null) _saveButton.clicked += RequestSave;
            if (_openInEditorButton != null) _openInEditorButton.clicked += OnOpenInEditor;

            // Center-canvas pointer + keyboard input.
            if (_previewCanvas != null)
            {
                _previewCanvas.RegisterCallback<PointerDownEvent>(OnCanvasPointerDown);
                _previewCanvas.RegisterCallback<PointerMoveEvent>(OnCanvasPointerMove);
                _previewCanvas.RegisterCallback<PointerUpEvent>(OnCanvasPointerUp);
                _previewCanvas.focusable = true;
                _previewCanvas.RegisterCallback<KeyDownEvent>(OnCanvasKeyDown);

                // Bind C7 collider handles to the canvas (handle dots + LineRenderer outline).
                _colliderHandles.Initialize(_previewCanvas, previewCamera);   // C7 API
            }

            return true;
        }

        private void PopulateThumbnailGrid()
        {
            if (_thumbnailGrid == null) return;
            _thumbnailGrid.Clear();

            IReadOnlyList<RegistryEntry> filtered = Palette.GetFiltered();   // RuntimeBrushPalette.cs:111
            foreach (RegistryEntry entry in filtered)
            {
                RegistryEntry captured = entry;     // closure capture
                var thumb = new Button { name = "thumb_" + entry.guid };
                thumb.AddToClassList("brush-thumb");
                thumb.tooltip = entry.displayName;

                // Sprite background. Prefer entry.sprite; fall back to registry lookup by guid.
                Sprite sprite = entry.sprite != null ? entry.sprite : _registry.GetSprite(entry.guid); // :52
                if (sprite != null)
                {
                    thumb.style.backgroundImage = new StyleBackground(sprite);
                    thumb.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                else
                {
                    // Prefab-only / tile-only entry with no sprite — show truncated name.
                    thumb.text = ShortName(entry.displayName);
                }

                thumb.RegisterCallback<ClickEvent>(_ => SelectEntry(captured));
                _thumbnailGrid.Add(thumb);
            }
        }

        // ── Pointer routing (canvas → router) ──────────────────────────────────

        private void OnCanvasPointerDown(PointerDownEvent evt)
        {
            _previewCanvas.Focus();

            // C7 first: if a prop is selected and the press lands on a collider handle,
            // the handle owns the drag (Tick() applies it). We still let painting begin
            // only when no handle was grabbed — RuntimeColliderHandles arbitrates internally.
            // Right button (button 1) = erase; left (0) = paint.
            if (evt.button == 1)
            {
                _isErasing = true;
                DoErase(evt.position);
            }
            else
            {
                _isPainting = true;
                DoPaint(evt.position);
            }
            evt.StopPropagation();
        }

        private void OnCanvasPointerMove(PointerMoveEvent evt)
        {
            if (_isPainting) DoPaint(evt.position);
            else if (_isErasing) DoErase(evt.position);
            // C8 cliff hover follows the cursor each frame in Update() — nothing to do here.
        }

        private void OnCanvasPointerUp(PointerUpEvent evt)
        {
            bool didEdit = _isPainting || _isErasing;
            _isPainting = false;
            _isErasing = false;
            if (didEdit) RequestSave();   // serialize after a stroke completes
        }

        private void DoPaint(Vector2 panelPointerPos)
        {
            RegistryEntry entry = SelectedEntry;
            if (entry == null) return;

            (Vector3Int cell, Vector3 world) = ScreenCellUnderPointer(panelPointerPos);

            // BrushExecutorRouter.Paint(PaletteMode, RegistryEntry, Vector3Int cell,
            //   Vector3 worldPos, float rotationZ, RoomLayoutData liveDoc) → PaintResult
            BrushExecutorRouter.PaintResult result =
                _router.Paint(CurrentMode, entry, cell, world, CurrentRotation, _liveDoc);

            // If a prop was spawned, select it for the collider inspector (C7 target).
            if (result.spawned != null)
            {
                _selectedInstance = result.spawned;
                _selectedInstanceEntry = entry;
                _colliderHandles.SetTarget(_selectedInstance, _selectedInstanceEntry);  // C7 API
                RefreshInspector();
            }
        }

        private void DoErase(Vector2 panelPointerPos)
        {
            (Vector3Int cell, Vector3 world) = ScreenCellUnderPointer(panelPointerPos);
            _router.Erase(CurrentMode, cell, world, _liveDoc);   // BrushExecutorRouter.Erase signature
        }

        // ── Keyboard ───────────────────────────────────────────────────────────

        private void OnCanvasKeyDown(KeyDownEvent evt)
        {
            // R = rotate brush 90° (parity with VisualEditorScenePainter rotation step).
            if (evt.keyCode == KeyCode.R)
            {
                CurrentRotation = Mathf.Repeat(CurrentRotation + 90f, 360f);
                evt.StopPropagation();
                return;
            }

            // Ctrl+Z = undo.
            if (evt.keyCode == KeyCode.Z && (evt.ctrlKey || evt.commandKey))
            {
                Undo();
                evt.StopPropagation();
            }
        }

        // ── Per-frame drive (collider handle reposition + hover follow) ────────

        private void Update()
        {
            // C7: reposition handle dots / outline each frame, apply active drag,
            // write collider_overrides into the live doc when dragged.
            if (_colliderHandles != null && _selectedInstance != null)
                _colliderHandles.Tick(_liveDoc);   // C7 API — mutates _liveDoc.collider_overrides

            // C8 hover updates itself in its own Update(); nothing required here.
        }

        // ── Inspector ──────────────────────────────────────────────────────────

        private void RefreshInspector()
        {
            if (_selectedNameLabel != null)
            {
                _selectedNameLabel.text = _selectedInstanceEntry != null
                    ? _selectedInstanceEntry.displayName
                    : (SelectedEntry != null ? SelectedEntry.displayName : "(nothing selected)");
            }

            // Reflect the selected instance's current collider into the size fields.
            if (_selectedInstance != null)
            {
                Collider2D col = _selectedInstance.GetComponent<Collider2D>();
                if (col is BoxCollider2D box)
                {
                    SetColliderShapeDropdownSilently(ColliderShape.Box);
                    SetSizeFieldsSilently(box.size.x, box.size.y);
                }
                else if (col is CircleCollider2D circle)
                {
                    SetColliderShapeDropdownSilently(ColliderShape.Circle);
                    SetSizeFieldsSilently(circle.radius, circle.radius);
                }
                else if (col is CapsuleCollider2D capsule)
                {
                    SetColliderShapeDropdownSilently(ColliderShape.Capsule);
                    SetSizeFieldsSilently(capsule.size.x, capsule.size.y);
                }
            }
        }

        private void OnColliderShapeChanged(ChangeEvent<string> evt)
        {
            if (_selectedInstance == null) return;
            if (!Enum.TryParse(evt.newValue, out ColliderShape shape)) return;

            // Runtime collider swap (no AssetDatabase): destroy old, add new (BUILD CONTRACT §3 C7).
            Collider2D existing = _selectedInstance.GetComponent<Collider2D>();
            if (existing != null) Destroy(existing);

            switch (shape)
            {
                case ColliderShape.Box:     _selectedInstance.AddComponent<BoxCollider2D>(); break;
                case ColliderShape.Circle:  _selectedInstance.AddComponent<CircleCollider2D>(); break;
                case ColliderShape.Capsule: _selectedInstance.AddComponent<CapsuleCollider2D>(); break;
                // Polygon deferred (RuntimeColliderHandles.cs:149 parity).
            }

            // Re-point C7 at the new collider and persist.
            _colliderHandles.SetTarget(_selectedInstance, _selectedInstanceEntry);
            RefreshInspector();
            RequestSave();
        }

        private void OnColliderSizeChanged()
        {
            if (_selectedInstance == null) return;
            float x = _colliderSizeX != null ? _colliderSizeX.value : 0f;
            float y = _colliderSizeY != null ? _colliderSizeY.value : 0f;

            Collider2D col = _selectedInstance.GetComponent<Collider2D>();
            if (col is BoxCollider2D box) box.size = new Vector2(Mathf.Max(0.01f, x), Mathf.Max(0.01f, y));
            else if (col is CircleCollider2D circle) circle.radius = Mathf.Max(0.01f, x);
            else if (col is CapsuleCollider2D capsule) capsule.size = new Vector2(Mathf.Max(0.01f, x), Mathf.Max(0.01f, y));

            RequestSave();
        }

        private void OnOpenInEditor()
        {
            // Spec §5.3 R3 mitigation: hand off to the full Editor for richer authoring.
            // Tool.exe cannot open the Editor; persist truth + signal intent so the Editor
            // side (LiveToolLauncher) can pick the room up. We mark a sentinel next to the JSON.
            RequestSave();
            try
            {
                string flag = Path.Combine(Path.GetDirectoryName(JsonPath) ?? ".", "open_in_editor.flag");
                File.WriteAllText(flag, DateTime.UtcNow.ToString("o"));
                Debug.Log("[ToolBootstrap] open_in_editor.flag written — Editor can pick up the room.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[ToolBootstrap] open-in-editor handoff failed: {ex.Message}");
            }
            // ASSUMPTION: the Editor-side pickup of open_in_editor.flag is a separate task
            // (not in this contract). Tool side only writes truth + the sentinel.
        }

        // ── Grid / cursor math ─────────────────────────────────────────────────

        /// <summary>
        /// Convert a UXML panel pointer position into (snapped cell, world point).
        /// Panel coords have origin top-left; screen coords (for the camera) have origin
        /// bottom-left, so Y is flipped against the canvas height before ScreenToWorldPoint.
        /// Snap = Mathf.RoundToInt (BUILD CONTRACT §C8 / VisualEditorScenePainter parity).
        /// </summary>
        private (Vector3Int cell, Vector3 world) ScreenCellUnderPointer(Vector2 panelPos)
        {
            float screenX = panelPos.x;
            float screenY = Screen.height - panelPos.y;   // UXML(top-left) → screen(bottom-left)

            // Distance from the (orthographic) camera plane: use camera nearClip offset along forward.
            Vector3 screenPoint = new Vector3(screenX, screenY, -previewCamera.transform.position.z);
            Vector3 world = previewCamera.ScreenToWorldPoint(screenPoint);

            Vector3Int cell = new Vector3Int(
                Mathf.RoundToInt(world.x),
                Mathf.RoundToInt(world.y),
                0);
            return (cell, world);
        }

        // ── Live-doc load/create ───────────────────────────────────────────────

        private RoomLayoutData LoadOrCreateLiveDoc()
        {
            if (File.Exists(JsonPath))
            {
                try
                {
                    string json = File.ReadAllText(JsonPath);
                    RoomLayoutData parsed = RoomLayoutData.FromJson(json);   // RoomLayoutData.cs:31
                    if (parsed != null)
                    {
                        if (parsed.floor_tiles    == null) parsed.floor_tiles    = new List<FloorTileData>();
                        if (parsed.cliff_cells     == null) parsed.cliff_cells     = new List<CliffCellData>();
                        if (parsed.prop_instances  == null) parsed.prop_instances  = new List<PropData>();
                        if (parsed.collider_overrides == null) parsed.collider_overrides = new List<ColliderOverrideData>();
                        return parsed;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[ToolBootstrap] Could not load existing room_current.json ({ex.Message}); starting blank.");
                }
            }

            return new RoomLayoutData
            {
                schema_version = SchemaVersion,
                room_id = "tool_room",
                metadata = new RoomLayoutMeta
                {
                    name = "Tool Room",
                    created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    modified = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                },
            };
        }

        // ── Small UI helpers ───────────────────────────────────────────────────

        private void HighlightSelectedThumb(RegistryEntry entry)
        {
            if (_thumbnailGrid == null) return;
            foreach (VisualElement child in _thumbnailGrid.Children())
            {
                bool isSel = entry != null && child.name == "thumb_" + entry.guid;
                child.EnableInClassList("brush-thumb--selected", isSel);
            }
        }

        private void SetColliderShapeDropdownSilently(ColliderShape shape)
        {
            if (_colliderShapeDropdown == null) return;
            _colliderShapeDropdown.SetValueWithoutNotify(shape.ToString());
        }

        private void SetSizeFieldsSilently(float x, float y)
        {
            _colliderSizeX?.SetValueWithoutNotify(x);
            _colliderSizeY?.SetValueWithoutNotify(y);
        }

        private void ShowAbortBanner(string message)
        {
            VisualElement root = uiDocument != null ? uiDocument.rootVisualElement : null;
            if (root == null) return;
            root.Clear();
            _bannerLabel = new Label(message);
            _bannerLabel.style.color = new StyleColor(new Color(1f, 0.35f, 0.35f));
            _bannerLabel.style.fontSize = 20;
            _bannerLabel.style.whiteSpace = WhiteSpace.Normal;
            _bannerLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _bannerLabel.style.flexGrow = 1;
            root.Add(_bannerLabel);
        }

        private static string ShortName(string s)
        {
            if (string.IsNullOrEmpty(s)) return "?";
            return s.Length <= 6 ? s : s.Substring(0, 6);
        }
    }
}

#endif // RIMA_LIVE_TOOL
