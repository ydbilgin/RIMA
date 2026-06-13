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

        // --- ISO-grid overlay (consolidation item 6 — user: "no grid") ---
        // A pooled set of world-space LineRenderer diamonds drawn around the camera while Build Mode
        // is active, so the iso cell layout is visible (and screenshot-verifiable, unlike a
        // ScreenSpaceOverlay canvas). Built lazily, shown/hidden with the build session, rebuilt only
        // when the visible cell window changes (no per-cell per-frame GC).
        private const int GridOverlayRadius = 14;             // cells out from the centre cell each axis
        private static readonly Color GridLineColor = new Color(0.71f, 0.74f, 0.79f, 0.16f); // dim slate
        private GameObject gridOverlay;
        private readonly List<LineRenderer> gridLines = new List<LineRenderer>();
        private Material gridLineMaterial;
        private Vector3Int gridOverlayCentre;
        private bool gridOverlayBuilt;

        // --- UI (runtime, no prefab) ---
        private Canvas paletteCanvas;
        private RectTransform paletteRoot;
        private TextMeshProUGUI statusLabel;
        private TextMeshProUGUI hintLabel;
        private BuildModeUiStyle.ButtonStyle propToolSwatch;
        private BuildModeUiStyle.ButtonStyle tileToolSwatch;
        private bool paletteBuilt;

        // --- PHASE A asset browser (data-driven catalog + thumbnail-card grid) ---
        private readonly BuildModeAssetCatalog catalog = new BuildModeAssetCatalog();
        private BuildModeUiStyle.TabHandle[] tabHandles;
        private int activeCategory;                 // index into catalog.Categories
        private TMP_InputField searchField;
        private string searchTerm = string.Empty;
        private BuildModeUiStyle.ScrollGrid assetGrid;
        private GameObject emptyState;
        // One built card per catalog entry of the active category (rebuilt on tab/filter change).
        private readonly List<BuildModeUiStyle.AssetCard> cards = new List<BuildModeUiStyle.AssetCard>();
        private readonly List<BuildModeAssetCatalog.AssetEntry> cardEntries = new List<BuildModeAssetCatalog.AssetEntry>();
        private BuildModeAssetCatalog.AssetEntry selectedEntry;

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
                ShowGridOverlay();
                ActiveTool = BuildTool.Prop; // enter always starts on the prop tool (Phase 1/2 default).
                if (selectedIndex < 0 && palette.Count > 0) SelectPalette(0);
                RefreshToolHighlight();
                UpdateStatus();
            }
            else
            {
                HideGhost();
                HideGridOverlay();
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
            AlignBrowserTabToTool(tool); // keep the asset-browser tab coherent with the segmented control.
            UpdateStatus();
        }

        // Point the browser at the category that matches the active tool WITHOUT re-firing the tool
        // switch (avoids SetActiveTool <-> SelectCategory recursion). Only acts once the UI exists.
        private void AlignBrowserTabToTool(BuildTool tool)
        {
            if (!paletteBuilt || tabHandles == null) return;
            BuildModeAssetCatalog.AssetCategory want = tool == BuildTool.Tile
                ? BuildModeAssetCatalog.AssetCategory.Tiles
                : BuildModeAssetCatalog.AssetCategory.Props;
            for (int i = 0; i < catalog.Categories.Count; i++)
            {
                if (catalog.Categories[i].category != want) continue;
                if (i == activeCategory) return; // already showing the right tab.
                activeCategory = i;
                for (int t = 0; t < tabHandles.Length; t++)
                    BuildModeUiStyle.ApplyTabSelected(tabHandles[t], t == activeCategory);
                RebuildCards();
                return;
            }
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

            // Refresh the iso-grid overlay's visible window (rebuilds only when the centre cell moves).
            RefreshGridOverlay();

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

        // ---------------------------------------------------------------- iso-grid overlay

        /// <summary>Build (lazily) + show the iso-grid overlay. Gated on Build Mode being active.</summary>
        private void ShowGridOverlay()
        {
            if (!IsBuildModeActive || !BuildModeController.IsActive) return;
            ResolveSceneRefs();
            if (grid == null) return; // can be fake-null under DisableDomainReload until a room loads.

            if (gridOverlay == null)
            {
                gridOverlay = new GameObject("BuildGridOverlay");
                gridOverlay.transform.position = Vector3.zero;
            }
            gridOverlay.SetActive(true);
            gridOverlayBuilt = false; // force a rebuild on the next refresh so the window is current.
            RefreshGridOverlay();
        }

        /// <summary>
        /// Rebuild the pooled diamond LineRenderers only when the camera-centre cell changes, so a
        /// stationary camera draws zero per-frame garbage. Diamonds use the neighbour-midpoint method
        /// (NEVER rectangular cellX*size math) so they tile seamlessly on the iso Grid.
        /// </summary>
        private void RefreshGridOverlay()
        {
            if (gridOverlay == null || !gridOverlay.activeSelf) return;
            if (!IsBuildModeActive || !BuildModeController.IsActive) return;
            if (grid == null) return;

            Vector3Int centre = CameraCentreCell();
            if (gridOverlayBuilt && centre == gridOverlayCentre) return; // window unchanged, nothing to do.
            gridOverlayCentre = centre;
            gridOverlayBuilt = true;

            int side = GridOverlayRadius * 2 + 1;
            int needed = side * side;
            EnsureGridLinePool(needed);

            int li = 0;
            for (int dy = -GridOverlayRadius; dy <= GridOverlayRadius; dy++)
            {
                for (int dx = -GridOverlayRadius; dx <= GridOverlayRadius; dx++)
                {
                    Vector3Int cell = new Vector3Int(centre.x + dx, centre.y + dy, centre.z);
                    SetDiamond(gridLines[li++], cell);
                }
            }

            // Disable any pooled lines beyond what this window needs (shrunk window / smaller radius).
            for (int i = li; i < gridLines.Count; i++)
            {
                if (gridLines[i] != null) gridLines[i].enabled = false;
            }
        }

        // Centre the overlay window on the cell under the camera (cursor-independent so the grid is
        // stable while painting). Uses the iso WorldToCell, consistent with placement.
        private Vector3Int CameraCentreCell()
        {
            if (buildCamera == null) buildCamera = Camera.main;
            Vector3 world = buildCamera != null ? buildCamera.transform.position : Vector3.zero;
            world.z = 0f;
            return grid.WorldToCell(world);
        }

        private void EnsureGridLinePool(int count)
        {
            if (gridLineMaterial == null)
            {
                // Unlit vertex-colour material so the lines render correctly in URP 2D.
                gridLineMaterial = new Material(Shader.Find("Sprites/Default")) { hideFlags = HideFlags.DontSave };
            }

            while (gridLines.Count < count)
            {
                GameObject go = new GameObject("GridDiamond");
                go.transform.SetParent(gridOverlay.transform, false);
                LineRenderer lr = go.AddComponent<LineRenderer>();
                lr.useWorldSpace = true;
                lr.loop = true;
                lr.positionCount = 4;
                lr.widthMultiplier = 0.03f;
                lr.numCornerVertices = 0;
                lr.numCapVertices = 0;
                lr.material = gridLineMaterial;
                lr.startColor = GridLineColor;
                lr.endColor = GridLineColor;
                lr.sortingOrder = 32750; // just under the prop ghost (32760) so the ghost stays on top.
                int layerId = SortingLayer.NameToID(PropsSortingLayer);
                if (layerId != 0) lr.sortingLayerID = layerId;
                lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                lr.receiveShadows = false;
                gridLines.Add(lr);
            }
        }

        // Diamond corners via the neighbour-midpoint method: each of top/right/bottom/left is the
        // midpoint between this cell's centre and the corresponding 4-neighbour's centre. On an iso
        // Grid this yields the cell's true rhombus and tiles seamlessly (council rule: no rect math).
        private void SetDiamond(LineRenderer lr, Vector3Int cell)
        {
            if (lr == null) return;
            Vector3 c = grid.GetCellCenterWorld(cell);
            Vector3 up = grid.GetCellCenterWorld(cell + new Vector3Int(0, 1, 0));
            Vector3 right = grid.GetCellCenterWorld(cell + new Vector3Int(1, 0, 0));
            Vector3 down = grid.GetCellCenterWorld(cell + new Vector3Int(0, -1, 0));
            Vector3 left = grid.GetCellCenterWorld(cell + new Vector3Int(-1, 0, 0));

            Vector3 top = (c + up) * 0.5f;
            Vector3 rgt = (c + right) * 0.5f;
            Vector3 bot = (c + down) * 0.5f;
            Vector3 lft = (c + left) * 0.5f;
            top.z = rgt.z = bot.z = lft.z = 0f;

            lr.enabled = true;
            lr.SetPosition(0, top);
            lr.SetPosition(1, rgt);
            lr.SetPosition(2, bot);
            lr.SetPosition(3, lft);
        }

        private void HideGridOverlay()
        {
            if (gridOverlay != null) gridOverlay.SetActive(false);
            gridOverlayBuilt = false;
        }

        private void DestroyGridOverlay()
        {
            gridLines.Clear();
            if (gridOverlay != null) DestroyRuntimeObject(gridOverlay);
            gridOverlay = null;
            if (gridLineMaterial != null) DestroyRuntimeObject(gridLineMaterial);
            gridLineMaterial = null;
            gridOverlayBuilt = false;
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
            paletteRoot.sizeDelta = new Vector2(BuildModeUiStyle.PanelWidth, 680f); // taller = grid room (spec sec 3).
            paletteRoot.anchoredPosition = new Vector2(BuildModeUiStyle.Padding, 0f);

            // MakePanel returns the CONTENT rect already inset by the border + Padding.
            RectTransform content = BuildModeUiStyle.MakePanel(paletteRoot, "Panel", BuildModeUiStyle.PanelWidth);

            float headerH = BuildModeUiStyle.MakeHeader(content, "BUILD");

            // Segmented PROP | TILE control at the top. Selecting a tool routes ALL clicks to ONLY
            // that tool (SetActiveTool) — the Phase 3 exclusivity selector surfaced in the live UI.
            const float segH = 40f;
            RectTransform seg = new GameObject("Segmented", typeof(RectTransform)).GetComponent<RectTransform>();
            seg.SetParent(content, false);
            BuildModeUiStyle.Top(seg, segH, headerH);
            BuildModeUiStyle.ButtonStyle[] toolSegs = BuildModeUiStyle.MakeSegmented(seg, new[] { "PROP", "TILE" });
            propToolSwatch = toolSegs[0];
            tileToolSwatch = toolSegs[1];
            propToolSwatch.button.onClick.AddListener(() => SetActiveTool(BuildTool.Prop));
            tileToolSwatch.button.onClick.AddListener(() => SetActiveTool(BuildTool.Tile));

            // --- PHASE A ASSET BROWSER: data-driven tab bar -> search -> thumbnail-card grid. -----
            // Builds the catalog from the live project data (PropRegistry -> Props, brush -> Tiles).
            catalog.Build(registry);

            const float tabsH = 26f;
            const float searchH = 30f;
            const float hintH = 86f;
            float browserTop = headerH + segH + BuildModeUiStyle.ItemGap;

            // Category tab bar (data-driven over catalog.Categories).
            RectTransform tabRow = new GameObject("Tabs", typeof(RectTransform)).GetComponent<RectTransform>();
            tabRow.SetParent(content, false);
            BuildModeUiStyle.Top(tabRow, tabsH, browserTop);
            List<BuildModeUiStyle.TabSpec> tabSpecs = new List<BuildModeUiStyle.TabSpec>();
            for (int i = 0; i < catalog.Categories.Count; i++)
            {
                BuildModeAssetCatalog.AssetCategoryGroup g = catalog.Categories[i];
                tabSpecs.Add(new BuildModeUiStyle.TabSpec { label = g.label, count = g.Count, dot = g.dot });
            }
            tabHandles = BuildModeUiStyle.MakeTabBar(tabRow, tabSpecs, SelectCategory);

            // Search field.
            RectTransform searchRow = new GameObject("SearchRow", typeof(RectTransform)).GetComponent<RectTransform>();
            searchRow.SetParent(content, false);
            BuildModeUiStyle.Top(searchRow, searchH, browserTop + tabsH + BuildModeUiStyle.Space1);
            searchField = BuildModeUiStyle.MakeSearchField(searchRow, OnSearchChanged);

            // Scrollable thumbnail-card grid (2 cols @ 96x116) filling the middle band.
            float gridTop = browserTop + tabsH + searchH + (BuildModeUiStyle.Space1 * 2f);
            RectTransform gridRow = new GameObject("GridRow", typeof(RectTransform)).GetComponent<RectTransform>();
            gridRow.SetParent(content, false);
            gridRow.anchorMin = new Vector2(0f, 0f);
            gridRow.anchorMax = new Vector2(1f, 1f);
            gridRow.offsetMin = new Vector2(0f, hintH + BuildModeUiStyle.ItemGap);
            gridRow.offsetMax = new Vector2(0f, -gridTop);
            assetGrid = BuildModeUiStyle.MakeScrollGrid(gridRow, new Vector2(96f, 116f), 2);
            emptyState = BuildModeUiStyle.MakeEmptyState(assetGrid.viewport, "No assets",
                "Try another category or clear search.");
            emptyState.SetActive(false);

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
            // Default the browser to the Props category (preserves Phase 1/2 enter behavior).
            activeCategory = 0;
            SelectCategory(0);
            RefreshToolHighlight();
            UpdateStatus();
        }

        // ---------------------------------------------------------------- asset browser (Phase A)

        /// <summary>
        /// Data-driven category switch (tab click). Highlights the tab, rebuilds the card grid for the
        /// new category, and aligns the tool: Props -> Prop tool; Tiles -> Tile tool. Lights/Decals are
        /// stub categories (empty grid + empty-state). The browser only ROUTES selection; placement
        /// logic stays in the existing prop tool / tile brush.
        /// </summary>
        private void SelectCategory(int index)
        {
            if (catalog.Categories.Count == 0) return;
            activeCategory = Mathf.Clamp(index, 0, catalog.Categories.Count - 1);
            for (int i = 0; i < tabHandles.Length; i++)
                BuildModeUiStyle.ApplyTabSelected(tabHandles[i], i == activeCategory);

            BuildModeAssetCatalog.AssetCategory cat = catalog.Categories[activeCategory].category;
            if (cat == BuildModeAssetCatalog.AssetCategory.Props && ActiveTool != BuildTool.Prop)
                SetActiveTool(BuildTool.Prop);
            else if (cat == BuildModeAssetCatalog.AssetCategory.Tiles && ActiveTool != BuildTool.Tile)
                SetActiveTool(BuildTool.Tile);

            RebuildCards();
        }

        private void OnSearchChanged(string term)
        {
            searchTerm = term ?? string.Empty;
            RebuildCards();
        }

        // Rebuild the thumbnail-card grid for the active category, applying the search filter. Cards
        // are torn down + rebuilt (the demo catalog is small; a future large registry would pool).
        private void RebuildCards()
        {
            for (int i = 0; i < cards.Count; i++)
                if (cards[i] != null && cards[i].root != null) DestroyRuntimeObject(cards[i].root.gameObject);
            cards.Clear();
            cardEntries.Clear();

            BuildModeAssetCatalog.AssetCategoryGroup group =
                activeCategory >= 0 && activeCategory < catalog.Categories.Count ? catalog.Categories[activeCategory] : null;
            if (group != null)
            {
                for (int i = 0; i < group.entries.Count; i++)
                {
                    BuildModeAssetCatalog.AssetEntry e = group.entries[i];
                    if (!PassesFilter(e, searchTerm)) continue;
                    BuildModeAssetCatalog.AssetEntry captured = e;
                    BuildModeUiStyle.AssetCard card = BuildModeUiStyle.MakeAssetCard(
                        assetGrid.content, e.icon, e.displayName, () => SelectEntry(captured));
                    BuildModeUiStyle.ApplyCardDisabled(card, !e.enabled);
                    cards.Add(card);
                    cardEntries.Add(e);
                }
            }

            if (emptyState != null) emptyState.SetActive(cards.Count == 0);
            RefreshPaletteHighlight();
        }

        // Case-insensitive substring filter with '-' exclude tokens (spec 2.5.2). e.g. "table -broken".
        private static bool PassesFilter(BuildModeAssetCatalog.AssetEntry entry, string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return true;
            string name = (entry.displayName ?? string.Empty).ToLowerInvariant();
            string[] tokens = term.ToLowerInvariant().Split(' ');
            bool hasInclude = false;
            for (int i = 0; i < tokens.Length; i++)
            {
                string tok = tokens[i].Trim();
                if (tok.Length == 0) continue;
                if (tok[0] == '-')
                {
                    string ex = tok.Substring(1);
                    if (ex.Length > 0 && name.Contains(ex)) return false;
                }
                else
                {
                    hasInclude = true;
                    if (!name.Contains(tok)) return false;
                }
            }
            return hasInclude || true;
        }

        /// <summary>
        /// Card click. Routes the SELECTED catalog entry to the existing tool: a Props entry drives
        /// the Phase 2 prop tool (SelectPalette); a Tiles entry drives the Phase 3 brush mode. No
        /// placement logic is duplicated here.
        /// </summary>
        private void SelectEntry(BuildModeAssetCatalog.AssetEntry entry)
        {
            if (entry == null) return;
            selectedEntry = entry;
            if (entry.category == BuildModeAssetCatalog.AssetCategory.Props && entry.payload is PropDefinitionSO def)
            {
                int idx = palette.IndexOf(def);
                if (idx >= 0) SelectPalette(idx);
            }
            else if (entry.category == BuildModeAssetCatalog.AssetCategory.Tiles && entry.payload is int mode)
            {
                if (ActiveTool != BuildTool.Tile) SetActiveTool(BuildTool.Tile);
                if (BuildTileBrushController.Instance != null)
                    BuildTileBrushController.Instance.SetMode((BuildTileBrushController.BrushMode)mode);
                UpdateStatus($"Selected {entry.displayName}. ");
            }
            RefreshPaletteHighlight();
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

        // Highlight the card matching the current selection. For Props the authority is the prop
        // tool's selectedIndex (so eyedropper/SelectPalette keep the card in sync); for Tiles it is
        // the last-selected tile entry. Cards not in the active category simply don't exist now.
        private void RefreshPaletteHighlight()
        {
            PropDefinitionSO selDef = SelectedDef();
            for (int i = 0; i < cards.Count; i++)
            {
                BuildModeAssetCatalog.AssetEntry e = cardEntries[i];
                bool isSel;
                if (e.category == BuildModeAssetCatalog.AssetCategory.Props)
                    isSel = selDef != null && ReferenceEquals(e.payload, selDef);
                else
                    isSel = ReferenceEquals(e, selectedEntry);
                BuildModeUiStyle.ApplyCardSelected(cards[i], isSel);
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
            DestroyGridOverlay();
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
            tabHandles = null;
            searchField = null;
            searchTerm = string.Empty;
            assetGrid = null;
            emptyState = null;
            cards.Clear();
            cardEntries.Clear();
            selectedEntry = null;
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

        // Grid-overlay lifecycle proof (consolidation item 6): the world-space overlay GameObject is
        // active while Build Mode is active and inactive on exit. Needs a live Grid (PlayMode / a
        // hand-built scene) to have been built.
        public bool GridOverlayActiveForValidation() => gridOverlay != null && gridOverlay.activeSelf;
    }
}
#endif
