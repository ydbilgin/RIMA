#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using RIMA.Environment;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.UI.BuildMode
{
    /// <summary>
    /// Build Mode PHASE 2 — the placement layer (design doc
    /// STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md, Phase 2 row + section 3.5 BINDING).
    ///
    /// PropRegistry-driven palette -> cursor GHOST snapped to the ISO Grid cell center, tinted
    /// GREEN when PropFootprintValidator + walkability pass and RED otherwise -> LMB place (mirrors
    /// IsoRoomBuilder.BuildProps, the iso-correct spawn) / RMB erase / [ ] rotate / F flip /
    /// E eyedropper / Ctrl+Z|Ctrl+Y undo-redo via BuildCommandStack.
    ///
    /// SECTION 3.5 COMPLIANCE (the #1 review criterion):
    ///   mouse -> cell  = grid.WorldToCell(mouseWorld)
    ///   cell -> world  = grid.GetCellCenterWorld(cell)   (NEVER cellX*size rectangular math)
    ///   props          = PropSorterRuntime + PropColliderAutoBuilder (the runtime prop recipe),
    ///                    NEVER a bare Instantiate at a hand-computed position.
    ///   footprint      = PropFootprintValidator.GetRotatedFootprint over iso-adjacent cells.
    ///
    /// LIFECYCLE: ACTIVE ONLY while BuildModeController.IsActive. BuildModeController calls
    /// SetBuildModeActive(true/false) on enter/exit. The free-cam pan + Director pause + UI-hover
    /// guard are NOT duplicated here — BuildModeController already puts DirectorMode in the Director
    /// state (timeScale 0, UpdateFreeCamera running) and the EventSystem hover test below is the
    /// same one-line stock guard DirectorMode uses.
    ///
    /// Self-bootstrapped (no scene/prefab wiring): BuildModeController.Instance creates this if
    /// missing. Singletons tolerate Enter-Play-Mode DisableDomainReload (lazy field + OnDestroy
    /// clears statics + all runtime objects torn down on disable).
    /// </summary>
    public sealed class BuildPlacementController : MonoBehaviour
    {
        /// <summary>
        /// PHASE 3 tool-exclusivity selector. Exactly ONE tool consumes an LMB/RMB click per frame
        /// (the #1 review criterion — re-introducing the double-place bug = REJECT). HandleCursor
        /// dispatches by ActiveTool BEFORE any leftButton read, so the Prop tool and the Tile/
        /// Walkability brush can never both act on a single click.
        /// </summary>
        public enum BuildTool { Prop, Tile }

        private const string PropRegistryResourcePath = "Props/PropRegistry";
        private const string PropsSortingLayer = "Props";
        private const float EraseRadius = 0.6f;

        private static BuildPlacementController _instance;

        public static BuildPlacementController Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<BuildPlacementController>();
                if (_instance != null) return _instance;
                GameObject go = new GameObject("BuildPlacementController");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<BuildPlacementController>();
                return _instance;
            }
        }

        public bool IsBuildModeActive { get; private set; }

        // Build Mode's OWN overlay canvas. BuildModeController reads this to EXEMPT it from the
        // enter-time "hide every other UI canvas" pass (so the panel never hides itself).
        internal Canvas OwnCanvas => paletteCanvas;

        // --- Resolved-on-demand scene refs (tolerant of fake-null under DisableDomainReload) ---
        private Grid grid;
        private Camera buildCamera;
        private PropRegistrySO registry;
        private RoomRunDirector runDirector;

        // --- Per-room cache ---
        private RoomTemplateSO cachedTemplate;
        private CompositionRoleMap cachedRoleMap;

        // --- Authoring state ---
        private readonly List<PropDefinitionSO> palette = new List<PropDefinitionSO>();
        private int selectedIndex = -1;
        private int rotationSteps;
        private bool flipX;

        private readonly BuildCommandStack commandStack = new BuildCommandStack();

        // PHASE 3: shared undo history. The tile brush rides the SAME stack so Ctrl+Z/Y interleave
        // prop + tile ops in one coherent history (a separate stack would split the history).
        internal BuildCommandStack CommandStack => commandStack;

        // PHASE 3 tool-exclusivity. Default = Prop (preserves Phase 1/2 behavior on enter).
        public BuildTool ActiveTool { get; private set; } = BuildTool.Prop;

        // Tracks every prop this controller placed: placement record (in template.props) + instance.
        private sealed class PlacedProp
        {
            public PropDefinitionSO def;
            public PropPlacementData data;
            public GameObject instance;
        }
        private readonly List<PlacedProp> placed = new List<PlacedProp>();

        // --- Ghost ---
        private GameObject ghost;
        private SpriteRenderer ghostRenderer;
        private float ghostPulse;
        private bool lastGhostValid;

        // --- UI (runtime, no prefab) ---
        private Canvas paletteCanvas;
        private RectTransform paletteRoot;
        private TextMeshProUGUI statusLabel;
        private TextMeshProUGUI hintLabel;
        private readonly List<BuildModeUiStyle.ButtonStyle> paletteSwatches = new List<BuildModeUiStyle.ButtonStyle>();
        private BuildModeUiStyle.ButtonStyle propToolSwatch;
        private BuildModeUiStyle.ButtonStyle tileToolSwatch;
        private bool paletteBuilt;

        private static readonly Color GhostGreen = new Color(0.25f, 0.95f, 0.45f, 0.50f);
        private static readonly Color GhostRed = new Color(0.95f, 0.28f, 0.28f, 0.50f);

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this) _instance = null;
            TeardownAll();
        }

        /// <summary>Called by BuildModeController on enter/exit. Enable = build palette + ghost + input.</summary>
        public void SetBuildModeActive(bool active)
        {
            if (active == IsBuildModeActive) return;
            IsBuildModeActive = active;

            if (active)
            {
                ResolveSceneRefs();
                EnsurePaletteUi();
                SetPaletteVisible(true);
                ActiveTool = BuildTool.Prop; // enter always starts on the prop tool (Phase 1/2 default).
                if (selectedIndex < 0 && palette.Count > 0) SelectPalette(0);
                RefreshToolHighlight();
                UpdateStatus();
            }
            else
            {
                HideGhost();
                SetPaletteVisible(false);
                // Disable the tile brush too (hide its cursor highlight + working-copy state).
                // Non-creating: never spawn a brush just to disable one that never existed.
                if (BuildTileBrushController.InstanceIfExists != null)
                    BuildTileBrushController.InstanceIfExists.SetActive(false);
                ActiveTool = BuildTool.Prop;
                // Authored records persist in template.props; only the editor-only command history
                // is dropped on exit (place/erase remain in the room, undo history does not bloat it).
                commandStack.Clear();
            }
        }

        /// <summary>
        /// PHASE 3 tool-exclusivity switch. Activates exactly one tool: hides the other tool's
        /// cursor/ghost and enables the new one. HandleCursor then routes the click to ONLY this tool.
        /// </summary>
        public void SetActiveTool(BuildTool tool)
        {
            ActiveTool = tool;
            if (tool == BuildTool.Prop)
            {
                if (BuildTileBrushController.InstanceIfExists != null)
                    BuildTileBrushController.InstanceIfExists.SetActive(false);
                RebuildGhost();
            }
            else // Tile
            {
                HideGhost(); // prop ghost must NOT render while the tile brush is active.
                if (BuildTileBrushController.Instance != null)
                    BuildTileBrushController.Instance.SetActive(true);
            }
            RefreshToolHighlight();
            UpdateStatus();
        }

        private void RefreshToolHighlight()
        {
            BuildModeUiStyle.ApplySelected(propToolSwatch, ActiveTool == BuildTool.Prop);
            BuildModeUiStyle.ApplySelected(tileToolSwatch, ActiveTool == BuildTool.Tile);
        }

        private void Update()
        {
            if (!IsBuildModeActive) return;
            // Defensive: if the camera-zoom wrapper aborted, never run the placement loop.
            if (!BuildModeController.IsActive)
            {
                SetBuildModeActive(false);
                return;
            }

            ResolveSceneRefs();
            HandleKeyboard();

            // PHASE 3 tool-exclusivity: exactly ONE tool's cursor/click handler runs per frame.
            // When the tile brush is active the prop cursor must NOT run (no ghost, no place) and
            // BuildTileBrushController owns the click; vice versa for the prop tool.
            if (ActiveTool == BuildTool.Prop)
            {
                HandleCursor();
            }
            else
            {
                HideGhost();
            }
        }

        // ---------------------------------------------------------------- input

        private void HandleKeyboard()
        {
            Keyboard kb = Keyboard.current;
            if (kb == null) return;

            bool ctrl = kb.leftCtrlKey.isPressed || kb.rightCtrlKey.isPressed;

            // Undo / Redo (Ctrl+Z / Ctrl+Y). Ctrl held -> these are NOT rotate/flip.
            if (ctrl)
            {
                if (kb.zKey.wasPressedThisFrame)
                {
                    if (commandStack.Undo()) UpdateStatus("Undo. ");
                    else UpdateStatus("Nothing to undo. ");
                    return;
                }
                if (kb.yKey.wasPressedThisFrame)
                {
                    if (commandStack.Redo()) UpdateStatus("Redo. ");
                    else UpdateStatus("Nothing to redo. ");
                    return;
                }
                return;
            }

            // Rotate the pending prop over its iso-adjacent footprint.
            if (kb.leftBracketKey.wasPressedThisFrame)
            {
                rotationSteps = ((rotationSteps - 1) % 4 + 4) % 4;
                RebuildGhost();
            }
            else if (kb.rightBracketKey.wasPressedThisFrame)
            {
                rotationSteps = (rotationSteps + 1) % 4;
                RebuildGhost();
            }

            // Flip the pending prop (records flipX on the placement; mirrors PropPlacementData.flipX).
            if (kb.fKey.wasPressedThisFrame)
            {
                flipX = !flipX;
                if (ghostRenderer != null) ghostRenderer.flipX = flipX;
            }

            // Eyedropper: pick the prop type under the cursor into the palette selection.
            if (kb.eKey.wasPressedThisFrame)
            {
                Eyedropper();
            }
        }

        private void HandleCursor()
        {
            Mouse mouse = Mouse.current;
            PropDefinitionSO def = SelectedDef();
            if (mouse == null || grid == null || def == null)
            {
                HideGhost();
                return;
            }

            Vector3 mouseWorld = MouseToWorld(mouse.position.ReadValue());
            // SECTION 3.5: mouse -> ISO cell -> cell center. Never rectangular world rounding.
            Vector3Int cell = grid.WorldToCell(mouseWorld);
            Vector2Int origin = new Vector2Int(cell.x, cell.y);

            bool valid = ValidatePlacement(def, origin, out string detail);
            UpdateGhost(cell, valid);

            if (IsPointerOverUi()) return;

            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (valid) CommitPlace(def, origin);
                else UpdateStatus(string.IsNullOrEmpty(detail) ? "Cannot place here. " : detail + " ");
            }
            else if (mouse.rightButton.wasPressedThisFrame)
            {
                CommitEraseAt(mouseWorld);
            }
        }

        // ---------------------------------------------------------------- validity

        private bool ValidatePlacement(PropDefinitionSO def, Vector2Int origin, out string detail)
        {
            detail = string.Empty;
            // PHASE 3.1: validate against the shared working copy so brush walkability/overlay edits
            // are honored (props can't land on a cell the brush just blocked, and can land on a cell
            // the brush just made walkable).
            RoomTemplateSO template = WorkingTemplate();
            if (template == null)
            {
                detail = "No active room template.";
                return false;
            }

            EnsureRoleMap(template);

            PropFootprintValidator.ValidationResult res = PropFootprintValidator.Validate(
                def, origin, rotationSteps, template, cachedRoleMap, template.props, out detail);
            if (res != PropFootprintValidator.ValidationResult.Valid) return false;

            // Extra walkability gate (section 3.5 rule 6): every footprint cell must be walkable
            // ground. PropFootprintValidator only enforces this when def.requiresWalkableTile, so
            // we add the authority check here so props never land on void / off-floor cells.
            WalkabilityMap walk = WalkabilityMap.Instance;
            if (walk != null)
            {
                Vector2Int size = PropFootprintValidator.GetRotatedFootprint(def, rotationSteps);
                for (int dy = 0; dy < size.y; dy++)
                {
                    for (int dx = 0; dx < size.x; dx++)
                    {
                        Vector3Int c = new Vector3Int(origin.x + dx, origin.y + dy, 0);
                        if (!walk.IsWalkable(c))
                        {
                            detail = $"Footprint cell {c.x},{c.y} is not walkable.";
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // ---------------------------------------------------------------- place / erase (commands)

        private void CommitPlace(PropDefinitionSO def, Vector2Int origin)
        {
            PropPlacementData data = new PropPlacementData(GuidFor(def), origin)
            {
                rotationSteps = rotationSteps,
                flipX = flipX,
                variantIndex = def.PickVariantIndexForTile(origin),
                placedByUser = "BuildMode"
            };
            commandStack.Execute(new PlaceOp(this, def, data));
            UpdateStatus($"Placed {Label(def)}. ");
        }

        private void CommitEraseAt(Vector3 mouseWorld)
        {
            // SECTION 3.5: resolve the erase target by ISO cell, not a world-radius search.
            // cellSize is 0.96 x 0.59, so a circular EraseRadius is ~1 cell on X but >1 on the
            // compressed Y — it could grab an adjacent prop or miss. Match the prop whose footprint
            // covers grid.WorldToCell(mouse) exactly (same resolution as the *ForValidation path).
            Vector3Int cell = grid.WorldToCell(mouseWorld);
            PlacedProp target = FindPlacedAtCell(new Vector2Int(cell.x, cell.y));
            if (target == null)
            {
                UpdateStatus("No prop at cursor. ");
                return;
            }
            commandStack.Execute(new EraseOp(this, target));
            UpdateStatus("Erased prop. ");
        }

        // Spawn one prop the iso-correct way. Mirrors IsoRoomBuilder.BuildProps (cell center via
        // grid.GetCellCenterWorld) + the runtime recipe (PropSorterRuntime + PropColliderAutoBuilder).
        private GameObject SpawnInstance(PropDefinitionSO def, PropPlacementData data)
        {
            if (def == null || data == null || grid == null) return null;

            Vector3Int cell = new Vector3Int(data.tilePosition.x, data.tilePosition.y, 0);
            Vector3 center = grid.GetCellCenterWorld(cell);

            GameObject go = new GameObject($"prop_{def.propId}_{data.tilePosition.x}_{data.tilePosition.y}");
            go.transform.position = new Vector3(center.x, center.y, 0f);
            int nrot = ((data.rotationSteps % 4) + 4) % 4;
            go.transform.rotation = Quaternion.Euler(0f, 0f, -90f * nrot);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = PickSprite(def, data);
            sr.flipX = data.flipX;

            PropSorterRuntime sorter = go.AddComponent<PropSorterRuntime>();
            sorter.PropDef = def; // triggers Apply()

            if (def.blocksWalkable)
            {
                PropColliderAutoBuilder builder = go.AddComponent<PropColliderAutoBuilder>();
                builder.PropDef = def;
                builder.RotationSteps = nrot;
                builder.EnsureCollider();
            }

            return go;
        }

        // Apply a placement: write the record into the WORKING COPY's props, spawn the instance,
        // track it. PHASE 3.1: writes target the shared working copy (NOT the source .asset) and the
        // source is NEVER SetDirty'd — disk write-back is Phase 4 on explicit user command.
        internal void ApplyPlace(PropDefinitionSO def, PropPlacementData data)
        {
            RoomTemplateSO template = WorkingTemplate();
            if (template == null) return;
            if (template.props == null) template.props = new List<PropPlacementData>();

            if (!template.props.Contains(data)) template.props.Add(data);

            GameObject go = SpawnInstance(def, data);
            placed.Add(new PlacedProp { def = def, data = data, instance = go });
            RefreshWalkability(template);
        }

        // Reverse a placement: destroy the instance, remove the record + tracking entry.
        // PHASE 3.1: removes from the WORKING COPY's props; the source .asset is never touched.
        internal void RevertPlace(PropPlacementData data)
        {
            RoomTemplateSO template = WorkingTemplate();
            if (template != null && template.props != null)
            {
                template.props.Remove(data);
            }
            for (int i = placed.Count - 1; i >= 0; i--)
            {
                if (placed[i].data == data)
                {
                    DestroyRuntimeObject(placed[i].instance);
                    placed.RemoveAt(i);
                }
            }
            if (template != null) RefreshWalkability(template);
        }

        private PlacedProp FindPlacedNear(Vector3 world)
        {
            PlacedProp best = null;
            float bestSqr = EraseRadius * EraseRadius;
            for (int i = 0; i < placed.Count; i++)
            {
                if (placed[i].instance == null) continue;
                float sqr = (placed[i].instance.transform.position - world).sqrMagnitude;
                if (sqr <= bestSqr)
                {
                    bestSqr = sqr;
                    best = placed[i];
                }
            }
            return best;
        }

        // SECTION 3.5: cell-exact target resolution. A placed prop occupies its rotated footprint
        // starting at data.tilePosition; a prop is "at" cell c if c lies inside that footprint rect.
        // Iterates newest-first so the most recently placed prop on a shared cell is erased first.
        private PlacedProp FindPlacedAtCell(Vector2Int cell)
        {
            for (int i = placed.Count - 1; i >= 0; i--)
            {
                PlacedProp p = placed[i];
                if (p == null || p.def == null || p.data == null) continue;
                Vector2Int origin = p.data.tilePosition;
                Vector2Int size = PropFootprintValidator.GetRotatedFootprint(p.def, p.data.rotationSteps);
                if (cell.x >= origin.x && cell.x < origin.x + size.x &&
                    cell.y >= origin.y && cell.y < origin.y + size.y)
                    return p;
            }
            return null;
        }

        private sealed class PlaceOp : IBuildOp
        {
            private readonly BuildPlacementController owner;
            private readonly PropDefinitionSO def;
            private readonly PropPlacementData data;
            public PlaceOp(BuildPlacementController o, PropDefinitionSO d, PropPlacementData p) { owner = o; def = d; data = p; }
            public void Do() => owner.ApplyPlace(def, data);
            public void Undo() => owner.RevertPlace(data);
        }

        private sealed class EraseOp : IBuildOp
        {
            private readonly BuildPlacementController owner;
            private readonly PropDefinitionSO def;
            private readonly PropPlacementData data;
            public EraseOp(BuildPlacementController o, PlacedProp p) { owner = o; def = p.def; data = p.data; }
            public void Do() => owner.RevertPlace(data);
            public void Undo() => owner.ApplyPlace(def, data);
        }

        // ---------------------------------------------------------------- eyedropper

        private void Eyedropper()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null) return;
            Vector3 world = MouseToWorld(mouse.position.ReadValue());
            PlacedProp target = FindPlacedNear(world);
            if (target == null || target.def == null)
            {
                UpdateStatus("Eyedropper: nothing under cursor. ");
                return;
            }
            int idx = palette.IndexOf(target.def);
            if (idx >= 0)
            {
                SelectPalette(idx);
                rotationSteps = ((target.data.rotationSteps % 4) + 4) % 4;
                flipX = target.data.flipX;
                RebuildGhost();
                UpdateStatus($"Picked {Label(target.def)}. ");
            }
        }

        // ---------------------------------------------------------------- ghost

        private void RebuildGhost()
        {
            HideGhost();
            PropDefinitionSO def = SelectedDef();
            if (def == null) return;

            ghost = new GameObject("BuildPlacementGhost", typeof(SpriteRenderer));
            ghostRenderer = ghost.GetComponent<SpriteRenderer>();
            ghostRenderer.sprite = def.worldSprite;
            ghostRenderer.flipX = flipX;
            int nrot = ((rotationSteps % 4) + 4) % 4;
            ghost.transform.rotation = Quaternion.Euler(0f, 0f, -90f * nrot);

            int layerId = SortingLayer.NameToID(PropsSortingLayer);
            if (layerId != 0) ghostRenderer.sortingLayerID = layerId;
            ghostRenderer.sortingOrder = 32760; // draw above placed props
            ghostRenderer.color = GhostGreen;
            ghostPulse = 0f;
        }

        private void UpdateGhost(Vector3Int cell, bool valid)
        {
            if (ghost == null || ghostRenderer == null) RebuildGhost();
            if (ghost == null) return;

            ghost.SetActive(true);
            // SECTION 3.5 rule 4: ghost snaps to the iso cell center, never a raw world point.
            Vector3 center = grid.GetCellCenterWorld(cell);
            ghost.transform.position = new Vector3(center.x, center.y, 0f);

            lastGhostValid = valid;
            ghostPulse += Time.unscaledDeltaTime * 6f;
            float pulse = 0.94f + Mathf.Sin(ghostPulse) * 0.04f;
            ghost.transform.localScale = Vector3.one * pulse;
            ghostRenderer.color = valid ? GhostGreen : GhostRed;
        }

        private void HideGhost()
        {
            if (ghost != null) DestroyRuntimeObject(ghost);
            ghost = null;
            ghostRenderer = null;
        }

        // ---------------------------------------------------------------- palette UI

        private void EnsurePaletteUi()
        {
            BuildPalette();
            if (paletteBuilt) return;

            paletteCanvas = new GameObject("BuildPaletteCanvas").AddComponent<Canvas>();
            paletteCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            paletteCanvas.sortingOrder = 5000;
            CanvasScaler scaler = paletteCanvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            paletteCanvas.gameObject.AddComponent<GraphicRaycaster>();
            DontDestroyOnLoad(paletteCanvas.gameObject);

            // LEFT panel = BUILD. Premium dark-slate panel + 1px border via the shared style helper.
            paletteRoot = new GameObject("BuildPaletteRoot", typeof(RectTransform)).GetComponent<RectTransform>();
            paletteRoot.SetParent(paletteCanvas.transform, false);
            paletteRoot.anchorMin = new Vector2(0f, 0.5f);
            paletteRoot.anchorMax = new Vector2(0f, 0.5f);
            paletteRoot.pivot = new Vector2(0f, 0.5f);
            paletteRoot.sizeDelta = new Vector2(BuildModeUiStyle.PanelWidth, 620f);
            paletteRoot.anchoredPosition = new Vector2(BuildModeUiStyle.Padding, 0f);

            // MakePanel returns the CONTENT rect already inset by the border + Padding.
            RectTransform content = BuildModeUiStyle.MakePanel(paletteRoot, "Panel", BuildModeUiStyle.PanelWidth);

            float headerH = BuildModeUiStyle.MakeHeader(content, "BUILD");

            // Segmented PROP | TILE control at the top. Selecting a tool routes ALL clicks to ONLY
            // that tool (SetActiveTool) — the Phase 3 exclusivity selector surfaced in the live UI.
            const float segH = 40f;
            RectTransform seg = new GameObject("Segmented", typeof(RectTransform), typeof(HorizontalLayoutGroup)).GetComponent<RectTransform>();
            seg.SetParent(content, false);
            BuildModeUiStyle.Top(seg, segH, headerH);
            HorizontalLayoutGroup hl = seg.GetComponent<HorizontalLayoutGroup>();
            hl.spacing = BuildModeUiStyle.ItemGap;
            hl.childControlWidth = true;
            hl.childForceExpandWidth = true;
            hl.childControlHeight = true;
            hl.childForceExpandHeight = true;
            propToolSwatch = BuildModeUiStyle.MakeButton(seg, "PROP");
            propToolSwatch.label.alignment = TextAlignmentOptions.Center;
            propToolSwatch.button.onClick.AddListener(() => SetActiveTool(BuildTool.Prop));
            tileToolSwatch = BuildModeUiStyle.MakeButton(seg, "TILE");
            tileToolSwatch.label.alignment = TextAlignmentOptions.Center;
            tileToolSwatch.button.onClick.AddListener(() => SetActiveTool(BuildTool.Tile));

            float topUsed = headerH + segH + BuildModeUiStyle.ItemGap;
            const float hintH = 86f;

            // Active tool's list: the prop palette as a vertical, premium button column.
            RectTransform list = new GameObject("PropList", typeof(RectTransform), typeof(VerticalLayoutGroup)).GetComponent<RectTransform>();
            list.SetParent(content, false);
            list.anchorMin = new Vector2(0f, 0f);
            list.anchorMax = new Vector2(1f, 1f);
            list.offsetMin = new Vector2(0f, hintH + BuildModeUiStyle.ItemGap);
            list.offsetMax = new Vector2(0f, -topUsed);
            VerticalLayoutGroup vl = list.GetComponent<VerticalLayoutGroup>();
            vl.spacing = BuildModeUiStyle.ItemGap;
            vl.childControlHeight = true;
            vl.childForceExpandHeight = false;
            vl.childControlWidth = true;
            vl.childForceExpandWidth = true;

            paletteSwatches.Clear();
            for (int i = 0; i < palette.Count; i++)
            {
                int captured = i;
                PropDefinitionSO def = palette[i];
                BuildModeUiStyle.ButtonStyle b = BuildModeUiStyle.MakeButton(list, Label(def));
                b.button.gameObject.AddComponent<LayoutElement>().preferredHeight = 38f;
                paletteSwatches.Add(b);
                b.button.onClick.AddListener(() => SelectPalette(captured));
            }

            // Bottom-left hint box (hotkeys) + a one-line status line just above it.
            hintLabel = BuildModeUiStyle.MakeHintBox(content, hintH);

            statusLabel = new GameObject("Status", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            statusLabel.rectTransform.SetParent(content, false);
            statusLabel.rectTransform.anchorMin = new Vector2(0f, 0f);
            statusLabel.rectTransform.anchorMax = new Vector2(1f, 0f);
            statusLabel.rectTransform.pivot = new Vector2(0.5f, 0f);
            statusLabel.rectTransform.sizeDelta = new Vector2(0f, 20f);
            statusLabel.rectTransform.anchoredPosition = new Vector2(0f, hintH + 4f);
            statusLabel.font = BuildModeUiStyle.Font;
            statusLabel.fontSize = 13f;
            statusLabel.color = BuildModeUiStyle.MutedText;
            statusLabel.alignment = TextAlignmentOptions.BottomLeft;
            statusLabel.enableWordWrapping = false;
            statusLabel.overflowMode = TextOverflowModes.Ellipsis;
            statusLabel.raycastTarget = false;

            paletteBuilt = true;
            RefreshPaletteHighlight();
            RefreshToolHighlight();
            UpdateStatus();
        }

        private void BuildPalette()
        {
            if (palette.Count > 0) return;
            if (registry == null) registry = Resources.Load<PropRegistrySO>(PropRegistryResourcePath);
#if UNITY_EDITOR
            if (registry == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:PropRegistrySO");
                if (guids.Length > 0)
                    registry = AssetDatabase.LoadAssetAtPath<PropRegistrySO>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
#endif
            if (registry == null) return;
            registry.RebuildIndex();
            IReadOnlyList<PropDefinitionSO> all = registry.AllProps;
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i] != null) palette.Add(all[i]);
            }
        }

        private void SelectPalette(int index)
        {
            if (index < 0 || index >= palette.Count) return;
            selectedIndex = index;
            rotationSteps = 0;
            flipX = false;
            RefreshPaletteHighlight();
            RebuildGhost();
            UpdateStatus($"Selected {Label(palette[index])}. ");
        }

        private void RefreshPaletteHighlight()
        {
            for (int i = 0; i < paletteSwatches.Count; i++)
            {
                BuildModeUiStyle.ApplySelected(paletteSwatches[i], i == selectedIndex);
            }
        }

        private void SetPaletteVisible(bool visible)
        {
            if (paletteCanvas != null) paletteCanvas.gameObject.SetActive(visible);
        }

        private void UpdateStatus(string prefix = "")
        {
            if (statusLabel != null)
            {
                string sel = SelectedDef() != null ? Label(SelectedDef()) : "none";
                statusLabel.text = $"{prefix}[{sel}]  rot {rotationSteps * 90}  flip {(flipX ? "Y" : "N")}";
            }
            if (hintLabel != null)
            {
                hintLabel.text =
                    "LMB place   RMB erase\n" +
                    "[ ] rotate   F flip   E pick\n" +
                    $"Ctrl+Z/Y undo ({commandStack.UndoCount})";
            }
        }

        // ---------------------------------------------------------------- resolution helpers

        private void ResolveSceneRefs()
        {
            if (buildCamera == null) buildCamera = Camera.main;

            if (grid == null)
            {
                WalkabilityMap walk = WalkabilityMap.Instance;
                if (walk != null && walk.floorTilemap != null) grid = walk.floorTilemap.layoutGrid;
                if (grid == null) grid = FindObjectOfType<Grid>();
            }

            if (runDirector == null) runDirector = FindObjectOfType<RoomRunDirector>();
            if (registry == null) registry = Resources.Load<PropRegistrySO>(PropRegistryResourcePath);
        }

        private RoomTemplateSO CurrentTemplate()
        {
            if (runDirector == null) runDirector = FindObjectOfType<RoomRunDirector>();
            return runDirector != null ? runDirector.CurrentTemplate : null;
        }

        // PHASE 3.1 (audit MAJOR fix): the ONE session working copy owned by BuildModeController.
        // ALL prop placement (validation + props list writes + walkability refresh) targets THIS copy
        // so the prop tool and the tile brush share one authority and the source .asset is never
        // mutated/dirtied. Falls back to the live source ONLY when Build Mode is inactive (e.g. a stray
        // editor call) so reads never NRE.
        private RoomTemplateSO WorkingTemplate()
        {
            return BuildModeController.ActiveWorkingTemplate ?? CurrentTemplate();
        }

        private void EnsureRoleMap(RoomTemplateSO template)
        {
            if (template == cachedTemplate && cachedRoleMap != null) return;
            cachedTemplate = template;
            cachedRoleMap = template != null ? CompositionRoleMapGenerator.GenerateFromRoom(template) : null;
        }

        private static void RefreshWalkability(RoomTemplateSO template)
        {
            // Placed props do NOT mutate WalkabilityMap: they block via their own Default-layer
            // BoxCollider2D (PropColliderAutoBuilder), not template.walkableGrid. InitFromTemplate
            // only re-reads template.walkableGrid (unchanged by placement). It is kept solely for its
            // side effect of resetting the Phase 1 reachability flood-fill cache, so the next
            // validity test re-evaluates from a clean cache after a place/erase.
            WalkabilityMap walk = WalkabilityMap.Instance;
            if (walk != null && template != null) walk.InitFromTemplate(template);
        }

        private Vector3 MouseToWorld(Vector2 screen)
        {
            if (buildCamera == null) buildCamera = Camera.main;
            if (buildCamera == null) return Vector3.zero;
            Vector3 world = buildCamera.ScreenToWorldPoint(
                new Vector3(screen.x, screen.y, -buildCamera.transform.position.z));
            world.z = 0f;
            return world;
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private PropDefinitionSO SelectedDef() => selectedIndex >= 0 && selectedIndex < palette.Count ? palette[selectedIndex] : null;

        private static string Label(PropDefinitionSO def)
        {
            if (def == null) return "Prop";
            return !string.IsNullOrEmpty(def.displayName) ? def.displayName :
                (!string.IsNullOrEmpty(def.name) ? def.name : "Prop");
        }

        private static Sprite PickSprite(PropDefinitionSO def, PropPlacementData data)
        {
            if (def == null) return null;
            if (def.variantSprites != null && data.variantIndex >= 0 && data.variantIndex < def.variantSprites.Length
                && def.variantSprites[data.variantIndex] != null)
                return def.variantSprites[data.variantIndex];
            return def.worldSprite;
        }

        private static string GuidFor(PropDefinitionSO def)
        {
            if (def == null) return string.Empty;
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(def);
            if (!string.IsNullOrEmpty(path))
            {
                string g = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(g)) return g;
            }
#endif
            return def.propId; // RebuildIndex self-heals propId to the asset GUID in-editor.
        }

        private void TeardownAll()
        {
            HideGhost();
            for (int i = 0; i < placed.Count; i++)
            {
                if (placed[i] != null) DestroyRuntimeObject(placed[i].instance);
            }
            placed.Clear();
            commandStack.Clear();
            if (paletteCanvas != null) DestroyRuntimeObject(paletteCanvas.gameObject);
            paletteCanvas = null;
            paletteRoot = null;
            statusLabel = null;
            hintLabel = null;
            propToolSwatch = null;
            tileToolSwatch = null;
            paletteSwatches.Clear();
            paletteBuilt = false;
        }

        private static void DestroyRuntimeObject(UnityEngine.Object target)
        {
            if (target == null) return;
            if (Application.isPlaying) Destroy(target);
            else DestroyImmediate(target);
        }

        // ---------------------------------------------------------------- *ForValidation (data-proof)
        // Overlay UI is invisible to MCP screenshots; these mirror DirectorMode's pattern so the
        // orchestrator can runtime-verify Phase 2 without reading pixels.

        public bool SelectFirstPropForValidation()
        {
            ResolveSceneRefs();
            BuildPalette();
            if (palette.Count == 0) return false;
            SelectPalette(0);
            return SelectedDef() != null;
        }

        public bool PlaceForValidation(Vector2Int cell)
        {
            ResolveSceneRefs();
            PropDefinitionSO def = SelectedDef();
            if (def == null) return false;
            if (!ValidatePlacement(def, cell, out _)) return false;
            CommitPlace(def, cell);
            return true;
        }

        public bool EraseForValidation(Vector2Int cell)
        {
            ResolveSceneRefs();
            if (grid == null) return false;
            PlacedProp target = FindPlacedAtCell(cell);
            if (target == null) return false;
            commandStack.Execute(new EraseOp(this, target));
            return true;
        }

        public bool UndoForValidation() => commandStack.Undo();
        public bool RedoForValidation() => commandStack.Redo();
        public int PlacedCountForValidation() => placed.Count;
        public bool HasGhostForValidation() => ghost != null;
        public bool LastGhostValidForValidation() => lastGhostValid;

        public bool RotateForValidation()
        {
            rotationSteps = (rotationSteps + 1) % 4;
            RebuildGhost();
            return true;
        }

        // PHASE 3 tool-exclusivity proof: switch tools and read back the active tool / prop-ghost
        // presence so the orchestrator can confirm ONE LMB acts through EXACTLY ONE tool.
        // tool: 0 = Prop, 1 = Tile (matches the BuildTool enum order).
        public bool SelectToolForValidation(int tool)
        {
            ResolveSceneRefs();
            SetActiveTool(tool == 1 ? BuildTool.Tile : BuildTool.Prop);
            return true;
        }

        public int ActiveToolForValidation() => (int)ActiveTool;
    }
}
#endif
