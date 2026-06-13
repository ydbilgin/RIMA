#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using RIMA.Environment;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace RIMA.UI.BuildMode
{
    /// <summary>
    /// Build Mode PHASE 3 — a CELL-AUTHORITATIVE tile / walkability / overlay BRUSH
    /// (design doc STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md Phase 3 row +
    /// STAGING/BUILDMODE_TERRAIN_DECISION_2026-06-13.md). Discrete Grid SetTile + RoomTemplateSO
    /// arrays — NOT organic, NOT a shader (the terrain council explicitly rejected world-space /
    /// organic terrain for the demo).
    ///
    /// SUB-MODES (LMB paints / RMB erases at the hovered cell):
    ///   FloorPaint    LMB = mark cell walkable + paint floor tile;  RMB = void + clear floor.
    ///   WalkableToggle LMB = mark walkable;  RMB = mark blocked (walkability only, no floor change).
    ///   OverlayPaint  LMB = paint overlay tile (index 1);  RMB = clear overlay.
    ///
    /// SECTION 3.5 COMPLIANCE (the #1 review criterion):
    ///   mouse -> cell  = grid.WorldToCell(mouseWorld)        (NEVER rectangular cellX*size)
    ///   cell  -> world = grid.GetCellCenterWorld(cell)       (cursor highlight + tile snap)
    ///   tiles          = Tilemap.SetTile(cell, tile) / erase = SetTile(cell, null)
    ///   ONLY the flat array index is rectangular: idx = (ly*bounds.width)+lx, template-LOCAL origin.
    ///
    /// TOOL EXCLUSIVITY (#1 integration constraint): this brush is owned by BuildPlacementController
    /// via the BuildTool selector. It runs its cursor/click loop ONLY while ActiveTool == Tile, so a
    /// single LMB click acts through EXACTLY ONE tool (Prop OR Tile) — never both.
    ///
    /// NO ASSET POLLUTION (#2 integration constraint): RoomRunDirector.CurrentTemplate IS the raw
    /// source .asset (no clone). This brush NEVER mutates it. PHASE 3.1: the brush does NOT own a copy
    /// — it binds to the ONE session working copy owned by BuildModeController (created on enter via
    /// Object.Instantiate, deep-copying the bool[]/int[] arrays + props List; hideFlags = DontSave).
    /// The prop tool edits the SAME instance, so the live Tilemap, the pathing authority and the prop
    /// validator can never disagree. The brush edits the shared copy's arrays + the live Tilemaps and
    /// always points WalkabilityMap.InitFromTemplate at it. EditorUtility.SetDirty is NEVER called
    /// (disk persistence is Phase 4 on explicit command).
    ///
    /// Self-bootstrapped (no scene/prefab wiring). DisableDomainReload-safe (lazy field + OnDestroy
    /// clears statics + all runtime objects torn down on disable).
    /// </summary>
    public sealed class BuildTileBrushController : MonoBehaviour
    {
        public enum BrushMode { FloorPaint, WalkableToggle, OverlayPaint }

        private const string OverlayTilemapName = "OverlayTilemap";

        private static BuildTileBrushController _instance;

        public static BuildTileBrushController Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<BuildTileBrushController>();
                if (_instance != null) return _instance;
                GameObject go = new GameObject("BuildTileBrushController");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<BuildTileBrushController>();
                return _instance;
            }
        }

        // Non-creating accessor: lets callers (e.g. Build Mode exit) tear the brush down WITHOUT
        // lazily spawning one that never existed.
        public static BuildTileBrushController InstanceIfExists => _instance;

        public bool IsActive { get; private set; }
        public BrushMode Mode { get; private set; } = BrushMode.FloorPaint;

        // The brush's OWN overlay canvas. BuildModeController reads this to EXEMPT it from the
        // enter-time "hide every other UI canvas" pass. May be null until the Tile tool is first
        // shown (the brush UI builds lazily) — created enabled AFTER the hide pass, so it is safe.
        internal Canvas OwnCanvas => brushCanvas;

        private int brushRadius = 1; // 1x1 must be perfect first; 1-3 optional (- / + keys).

        // --- Resolved-on-demand scene refs (tolerant of fake-null under DisableDomainReload) ---
        private Grid grid;
        private Camera brushCamera;
        private Tilemap groundTilemap;
        private Tilemap overlayTilemap;
        private RoomRunDirector runDirector;

        // --- Shared session working copy (owned by BuildModeController, NO source-asset pollution) ---
        private RoomTemplateSO workTemplate;     // the shared runtime copy we edit + feed to WalkabilityMap

        // --- Sampled tile stamps (IsoRoomBuilder tile assets are private; sample from live map) ---
        private TileBase floorTileStamp;
        private TileBase floorTileAltStamp;
        private TileBase overlayTileStamp;

        // --- Cursor highlight ---
        // PHASE 3.1 (audit MINOR fix): the painted footprint is an iso DIAMOND of cells, so the
        // highlight draws ONE small quad per actual FootprintCells entry (each snapped to its iso cell
        // center) instead of a single square scaled by radius. cursor = parent container; cursorCells =
        // pooled per-cell quads (radius 1 = exactly one quad, the already-correct case).
        private GameObject cursor;
        private readonly List<SpriteRenderer> cursorCells = new List<SpriteRenderer>();
        private float cursorPulse;
        private Vector3Int lastCursorCell;
        private bool lastCursorValid;

        // --- Hold-to-paint (continuous): one reversible op per NEW cell entered while a button is held ---
        private bool painting;
        private bool paintingLeft;
        private Vector3Int lastPaintedCell;

        // --- UI ---
        private Canvas brushCanvas;
        private TextMeshProUGUI statusLabel;
        private TextMeshProUGUI hintLabel;
        private TextMeshProUGUI radiusLabel;
        private readonly List<BuildModeUiStyle.ButtonStyle> modeSwatches = new List<BuildModeUiStyle.ButtonStyle>();
        private bool uiBuilt;

        private static readonly Color CursorGreen = new Color(0.25f, 0.95f, 0.45f, 0.45f);
        private static readonly Color CursorRed = new Color(0.95f, 0.28f, 0.28f, 0.45f);

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

        /// <summary>Enabled / disabled by BuildPlacementController.SetActiveTool. Active = show UI + cursor.</summary>
        public void SetActive(bool active)
        {
            if (active == IsActive) return;
            IsActive = active;
            if (active)
            {
                ResolveSceneRefs();
                EnsureWorkingCopy();
                EnsureUi();
                SetUiVisible(true);
                UpdateStatus();
            }
            else
            {
                painting = false; // never leak an in-progress stroke across a tool switch / exit.
                HideCursor();
                SetUiVisible(false);
            }
        }

        private void Update()
        {
            if (!IsActive) return;

            // Defensive: the brush only runs while Build Mode + the Tile tool are active.
            if (!BuildModeController.IsActive ||
                BuildPlacementController.Instance == null ||
                BuildPlacementController.Instance.ActiveTool != BuildPlacementController.BuildTool.Tile)
            {
                SetActive(false);
                return;
            }

            ResolveSceneRefs();
            HandleModeKeys();
            HandleCursor();
        }

        // ---------------------------------------------------------------- input

        private void HandleModeKeys()
        {
            Keyboard kb = Keyboard.current;
            if (kb == null) return;

            // Mode cycle: 1 = Floor, 2 = Walkable, 3 = Overlay. (Undo/redo stays on
            // BuildPlacementController's shared Ctrl+Z/Y — do not duplicate it here.)
            if (kb.digit1Key.wasPressedThisFrame) SetMode(BrushMode.FloorPaint);
            else if (kb.digit2Key.wasPressedThisFrame) SetMode(BrushMode.WalkableToggle);
            else if (kb.digit3Key.wasPressedThisFrame) SetMode(BrushMode.OverlayPaint);

            // Optional brush radius 1-3 (- shrink, + grow). 1x1 is the default + the perfect case.
            if (kb.minusKey.wasPressedThisFrame) { brushRadius = Mathf.Max(1, brushRadius - 1); UpdateStatus(); }
            else if (kb.equalsKey.wasPressedThisFrame) { brushRadius = Mathf.Min(3, brushRadius + 1); UpdateStatus(); }
        }

        private void HandleCursor()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null || grid == null)
            {
                HideCursor();
                return;
            }

            Vector3 mouseWorld = MouseToWorld(mouse.position.ReadValue());
            // SECTION 3.5: mouse -> ISO cell -> cell center for the highlight. Never rectangular math.
            Vector3Int cell = grid.WorldToCell(mouseWorld);
            bool inBounds = CellInBounds(cell);
            UpdateCursor(cell, inBounds);

            // Hold-to-paint: while a button is held, commit one reversible op per NEW cell entered
            // (tracked via lastPaintedCell). Each cell stays its own op so undo is granular. The
            // ActiveTool == Tile gate in Update() still guarantees ONE tool acts per click (no
            // double-place). A click that starts over the UI panel is ignored for the whole hold.
            bool lmbHeld = mouse.leftButton.isPressed;
            bool rmbHeld = mouse.rightButton.isPressed;
            bool lmbDown = mouse.leftButton.wasPressedThisFrame;
            bool rmbDown = mouse.rightButton.wasPressedThisFrame;

            // End the current stroke when no button is held (or both released).
            if (!lmbHeld && !rmbHeld)
            {
                painting = false;
                return;
            }

            // Start a stroke only on a fresh press that is NOT over the UI.
            if (!painting)
            {
                if (!lmbDown && !rmbDown) return;       // mid-hold that began elsewhere; ignore.
                if (IsPointerOverUi()) return;          // press started over the panel.
                painting = true;
                paintingLeft = lmbDown || (!rmbDown && lmbHeld);
                lastPaintedCell = new Vector3Int(int.MinValue, int.MinValue, 0); // force first commit.
            }

            // Same cell as the last one handled this stroke -> nothing to do (continuous, not spammy).
            if (cell == lastPaintedCell) return;
            lastPaintedCell = cell;

            if (!inBounds) { UpdateStatus("Out of room bounds. "); return; }

            // One reversible op per cell covers the whole brush footprint at that cell.
            Commit(cell, paint: paintingLeft);
        }

        // ---------------------------------------------------------------- edits (reversible)

        // Build the reversible op for the active mode over the brush footprint, then run it through
        // BuildPlacementController's SHARED command stack so Ctrl+Z/Y interleave prop + tile undo.
        private void Commit(Vector3Int centerCell, bool paint)
        {
            EnsureWorkingCopy();
            if (workTemplate == null) { UpdateStatus("No active room template. "); return; }

            List<Vector3Int> cells = FootprintCells(centerCell);
            IBuildOp op = Mode switch
            {
                BrushMode.FloorPaint => new FloorOp(this, cells, paint),
                BrushMode.WalkableToggle => new WalkableOp(this, cells, paint),
                BrushMode.OverlayPaint => new OverlayOp(this, cells, paint),
                _ => null
            };
            if (op == null) return;

            BuildCommandStack stack = BuildPlacementController.Instance != null
                ? BuildPlacementController.Instance.CommandStack : null;
            if (stack != null) stack.Execute(op);
            else op.Do();

            UpdateStatus(paint ? "Painted. " : "Erased. ");
        }

        private List<Vector3Int> FootprintCells(Vector3Int center)
        {
            List<Vector3Int> cells = new List<Vector3Int>();
            int r = brushRadius - 1; // radius 1 = single cell
            for (int dy = -r; dy <= r; dy++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    Vector3Int c = new Vector3Int(center.x + dx, center.y + dy, 0);
                    if (CellInBounds(c)) cells.Add(c);
                }
            }
            if (cells.Count == 0) cells.Add(center);
            return cells;
        }

        // --- low-level per-cell writes (used by ops; also the read-back authority for validation) ---

        // FLOOR: walkable + floor tile (paint) OR void + clear floor (erase). Writes BOTH the working
        // copy's walkableGrid AND the live ground Tilemap, then refreshes WalkabilityMap.
        private void SetFloor(Vector3Int cell, bool walkable)
        {
            EnsureFullArrays();
            int idx = LocalIndex(cell);
            if (idx >= 0 && workTemplate.walkableGrid != null && idx < workTemplate.walkableGrid.Length)
                workTemplate.walkableGrid[idx] = walkable;

            if (groundTilemap != null)
            {
                if (walkable)
                {
                    // Match IsoRoomBuilder's checker: floorTileAlt on (x+y)&1==1 when present.
                    TileBase tile = (floorTileAltStamp != null && ((cell.x + cell.y) & 1) == 1)
                        ? floorTileAltStamp : floorTileStamp;
                    groundTilemap.SetTile(cell, tile);
                }
                else
                {
                    groundTilemap.SetTile(cell, null);
                }
            }
            RefreshWalkability();
        }

        // WALKABILITY ONLY: flip the working-copy walkableGrid; live floor tile is untouched
        // (the cell may stay visually a floor but become blocked, e.g. an invisible wall).
        private void SetWalkable(Vector3Int cell, bool walkable)
        {
            EnsureFullArrays();
            int idx = LocalIndex(cell);
            if (idx >= 0 && workTemplate.walkableGrid != null && idx < workTemplate.walkableGrid.Length)
                workTemplate.walkableGrid[idx] = walkable;
            RefreshWalkability();
        }

        // OVERLAY: working-copy overlayMask (1-based, 0 = none) + live OverlayTilemap.
        private void SetOverlay(Vector3Int cell, int maskValue)
        {
            EnsureFullArrays();
            int idx = LocalIndex(cell);
            if (idx >= 0 && workTemplate.overlayMask != null && idx < workTemplate.overlayMask.Length)
                workTemplate.overlayMask[idx] = maskValue;

            Tilemap map = EnsureOverlayTilemap();
            if (map != null)
            {
                map.SetTile(cell, maskValue > 0 ? overlayTileStamp : null);
            }
        }

        // Read-back of the current walkability at a cell from the working copy (mirrors IsWalkable math).
        private bool ReadWalkable(Vector3Int cell)
        {
            if (workTemplate == null) return false;
            return workTemplate.IsWalkable(new Vector2Int(cell.x, cell.y));
        }

        private int ReadOverlay(Vector3Int cell)
        {
            if (workTemplate == null) return 0;
            return workTemplate.GetOverlayTileIndex(new Vector2Int(cell.x, cell.y));
        }

        // ---------------------------------------------------------------- reversible ops
        // Each op captures the PRIOR per-cell state so Undo restores exactly what was there.

        private sealed class FloorOp : IBuildOp
        {
            private readonly BuildTileBrushController owner;
            private readonly List<Vector3Int> cells;
            private readonly bool paint;
            private readonly bool[] prevWalkable;
            private readonly TileBase[] prevTiles;

            public FloorOp(BuildTileBrushController o, List<Vector3Int> c, bool paint)
            {
                owner = o; cells = c; this.paint = paint;
                prevWalkable = new bool[c.Count];
                prevTiles = new TileBase[c.Count];
                for (int i = 0; i < c.Count; i++)
                {
                    prevWalkable[i] = owner.ReadWalkable(c[i]);
                    prevTiles[i] = owner.groundTilemap != null ? owner.groundTilemap.GetTile(c[i]) : null;
                }
            }

            public void Do()
            {
                for (int i = 0; i < cells.Count; i++) owner.SetFloor(cells[i], paint);
            }

            public void Undo()
            {
                owner.EnsureFullArrays();
                for (int i = 0; i < cells.Count; i++)
                {
                    int idx = owner.LocalIndex(cells[i]);
                    if (idx >= 0 && owner.workTemplate.walkableGrid != null && idx < owner.workTemplate.walkableGrid.Length)
                        owner.workTemplate.walkableGrid[idx] = prevWalkable[i];
                    if (owner.groundTilemap != null) owner.groundTilemap.SetTile(cells[i], prevTiles[i]);
                }
                owner.RefreshWalkability();
            }
        }

        private sealed class WalkableOp : IBuildOp
        {
            private readonly BuildTileBrushController owner;
            private readonly List<Vector3Int> cells;
            private readonly bool walkable;
            private readonly bool[] prev;

            public WalkableOp(BuildTileBrushController o, List<Vector3Int> c, bool walkable)
            {
                owner = o; cells = c; this.walkable = walkable;
                prev = new bool[c.Count];
                for (int i = 0; i < c.Count; i++) prev[i] = owner.ReadWalkable(c[i]);
            }

            public void Do()
            {
                for (int i = 0; i < cells.Count; i++) owner.SetWalkable(cells[i], walkable);
            }

            public void Undo()
            {
                for (int i = 0; i < cells.Count; i++) owner.SetWalkable(cells[i], prev[i]);
            }
        }

        private sealed class OverlayOp : IBuildOp
        {
            private readonly BuildTileBrushController owner;
            private readonly List<Vector3Int> cells;
            private readonly bool paint;
            private readonly int[] prev;

            public OverlayOp(BuildTileBrushController o, List<Vector3Int> c, bool paint)
            {
                owner = o; cells = c; this.paint = paint;
                prev = new int[c.Count];
                for (int i = 0; i < c.Count; i++) prev[i] = owner.ReadOverlay(c[i]);
            }

            public void Do()
            {
                for (int i = 0; i < cells.Count; i++) owner.SetOverlay(cells[i], paint ? 1 : 0);
            }

            public void Undo()
            {
                for (int i = 0; i < cells.Count; i++) owner.SetOverlay(cells[i], prev[i]);
            }
        }

        // ---------------------------------------------------------------- working copy (no pollution)

        // PHASE 3.1 (audit MAJOR fix): the brush no longer owns its own copy. It binds to the ONE
        // session working copy owned by BuildModeController, so the prop tool and the brush edit the
        // SAME instance and WalkabilityMap is always pointed at that single template. The source
        // .asset is never instantiated/mutated here. WalkabilityMap is (re)pointed at the shared copy
        // so brush edits drive pathing; SetDirty is NEVER called.
        private void EnsureWorkingCopy()
        {
            RoomTemplateSO shared = BuildModeController.ActiveWorkingTemplate;
            if (shared == null) return;
            if (shared == workTemplate) return;

            workTemplate = shared;
            floorTileStamp = null; // re-sample stamps against the (possibly new) room's tilemaps.
            floorTileAltStamp = null;
            overlayTileStamp = null;

            EnsureFullArrays();
            SampleTileStamps();
            RefreshWalkability();
        }

        // walkableGrid / overlayMask may be null or empty (= 'whole bounds walkable / no overlay').
        // To author walls / overlay we MUST first materialize full width*height arrays.
        private void EnsureFullArrays()
        {
            if (workTemplate == null) return;
            int w = workTemplate.bounds.width;
            int h = workTemplate.bounds.height;
            if (w <= 0 || h <= 0) return;
            int len = w * h;

            if (workTemplate.walkableGrid == null || workTemplate.walkableGrid.Length != len)
            {
                bool[] arr = new bool[len];
                // Preserve the existing fallback meaning: null/empty == whole bounds walkable.
                bool fillTrue = workTemplate.walkableGrid == null || workTemplate.walkableGrid.Length == 0;
                if (fillTrue)
                {
                    for (int i = 0; i < len; i++) arr[i] = true;
                }
                else
                {
                    int copy = Mathf.Min(len, workTemplate.walkableGrid.Length);
                    for (int i = 0; i < copy; i++) arr[i] = workTemplate.walkableGrid[i];
                }
                workTemplate.walkableGrid = arr;
            }

            if (workTemplate.overlayMask == null || workTemplate.overlayMask.Length != len)
            {
                int[] arr = new int[len]; // default 0 = no overlay
                if (workTemplate.overlayMask != null)
                {
                    int copy = Mathf.Min(len, workTemplate.overlayMask.Length);
                    for (int i = 0; i < copy; i++) arr[i] = workTemplate.overlayMask[i];
                }
                workTemplate.overlayMask = arr;
            }
        }

        // SECTION 3.5: the flat array index is rectangular (template-LOCAL origin), NOT the world
        // mapping. idx = (ly*width)+lx with lx=cell.x-xMin, ly=cell.y-yMin. -1 if out of bounds.
        private int LocalIndex(Vector3Int cell)
        {
            if (workTemplate == null) return -1;
            RectInt b = workTemplate.bounds;
            int lx = cell.x - b.xMin;
            int ly = cell.y - b.yMin;
            if (lx < 0 || lx >= b.width || ly < 0 || ly >= b.height) return -1;
            return (ly * b.width) + lx;
        }

        private bool CellInBounds(Vector3Int cell)
        {
            if (workTemplate == null)
            {
                EnsureWorkingCopy();
                if (workTemplate == null) return false;
            }
            RectInt b = workTemplate.bounds;
            return cell.x >= b.xMin && cell.x < b.xMax && cell.y >= b.yMin && cell.y < b.yMax;
        }

        // WalkabilityMap aliases the array we hand it; pointing it at the working copy means brush
        // edits update pathing live. Re-call resets the reachability cache. NEVER SetDirty.
        private void RefreshWalkability()
        {
            WalkabilityMap walk = WalkabilityMap.Instance;
            if (walk != null && workTemplate != null) walk.InitFromTemplate(workTemplate);
        }

        // ---------------------------------------------------------------- tile stamps

        // IsoRoomBuilder's tile assets are private (no accessor). Sample them from the live tilemaps:
        // pick any existing floor / collision tile to reuse as the brush stamp.
        private void SampleTileStamps()
        {
            if (groundTilemap != null && floorTileStamp == null)
            {
                floorTileStamp = SampleAnyTile(groundTilemap);
            }
            // floorTileAlt + overlay are best-effort; checker alt and overlay default to the base
            // floor tile so a paint never silently produces an empty (invisible) cell.
            if (floorTileAltStamp == null) floorTileAltStamp = floorTileStamp;
            if (overlayTileStamp == null) overlayTileStamp = floorTileStamp;
        }

        private static TileBase SampleAnyTile(Tilemap map)
        {
            if (map == null) return null;
            foreach (Vector3Int c in map.cellBounds.allPositionsWithin)
            {
                TileBase t = map.GetTile(c);
                if (t != null) return t;
            }
            return null;
        }

        // ---------------------------------------------------------------- cursor highlight

        private void UpdateCursor(Vector3Int cell, bool valid)
        {
            if (cursor == null) BuildCursor();
            if (cursor == null) return;

            cursor.SetActive(true);

            // PHASE 3.1: one quad per ACTUAL footprint cell. For radius 1 that is exactly one quad at
            // the hovered cell (the already-correct case); for radius>1 the quads trace the iso DIAMOND
            // of cells the brush will paint, each snapped to its own cell center (SECTION 3.5 rule 4).
            List<Vector3Int> cells = FootprintCells(cell);
            EnsureCursorCells(cells.Count);

            cursorPulse += Time.unscaledDeltaTime * 6f;
            float pulse = 0.92f + Mathf.Sin(cursorPulse) * 0.06f;
            Color tint = (valid ? CursorGreen : CursorRed) * new Color(1f, 1f, 1f, pulse);

            Vector3 cs = grid.cellSize;
            Vector3 quadScale = new Vector3(Mathf.Max(0.01f, cs.x), Mathf.Max(0.01f, cs.y), 1f);

            for (int i = 0; i < cursorCells.Count; i++)
            {
                SpriteRenderer sr = cursorCells[i];
                if (sr == null) continue;
                bool used = i < cells.Count;
                sr.gameObject.SetActive(used);
                if (!used) continue;
                Vector3 center = grid.GetCellCenterWorld(cells[i]);
                sr.transform.position = new Vector3(center.x, center.y, 0f);
                sr.transform.localScale = quadScale;
                sr.color = tint;
            }

            lastCursorCell = cell;
            lastCursorValid = valid;
        }

        private void BuildCursor()
        {
            HideCursor();
            cursor = new GameObject("BuildTileBrushCursor");
            DontDestroyOnLoad(cursor);
            cursorPulse = 0f;
        }

        // Grow the per-cell quad pool to at least 'count'; reused across frames (footprint size is
        // tiny, 1..~13 cells, so the pool never churns).
        private void EnsureCursorCells(int count)
        {
            if (cursor == null) return;
            int layerId = SortingLayer.NameToID("Props");
            while (cursorCells.Count < count)
            {
                GameObject go = new GameObject("BrushCell", typeof(SpriteRenderer));
                go.transform.SetParent(cursor.transform, false);
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = MakeSolidSprite();
                if (layerId != 0) sr.sortingLayerID = layerId;
                sr.sortingOrder = 32760; // above floor + props
                cursorCells.Add(sr);
            }
        }

        private void HideCursor()
        {
            if (cursor != null) DestroyRuntimeObject(cursor);
            cursor = null;
            cursorCells.Clear();
        }

        private static Sprite _solidSprite;
        private static Sprite MakeSolidSprite()
        {
            if (_solidSprite != null) return _solidSprite;
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            tex.hideFlags = HideFlags.DontSave;
            // 1 unit = 1 px so localScale maps directly to world units (cellSize).
            _solidSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            _solidSprite.hideFlags = HideFlags.DontSave;
            return _solidSprite;
        }

        // ---------------------------------------------------------------- overlay tilemap

        // Overlay lives on its own runtime tilemap (IsoRoomBuilder.CreateOverlayTilemap). It may not
        // exist yet (only created when a room had overlay placements) — find or create it here.
        private Tilemap EnsureOverlayTilemap()
        {
            if (overlayTilemap != null) return overlayTilemap;
            if (grid == null) return null;

            foreach (Tilemap tm in grid.GetComponentsInChildren<Tilemap>(true))
            {
                if (tm != null && tm.gameObject.name == OverlayTilemapName)
                {
                    overlayTilemap = tm;
                    return overlayTilemap;
                }
            }

            // Create one matching IsoRoomBuilder.CreateOverlayTilemap (sortingOrder = ground+1).
            GameObject go = new GameObject(OverlayTilemapName);
            go.transform.SetParent(grid.transform, false);
            overlayTilemap = go.AddComponent<Tilemap>();
            TilemapRenderer r = go.AddComponent<TilemapRenderer>();
            TilemapRenderer groundRenderer = groundTilemap != null ? groundTilemap.GetComponent<TilemapRenderer>() : null;
            if (groundRenderer != null)
            {
                r.sortingLayerID = groundRenderer.sortingLayerID;
                r.sortingOrder = groundRenderer.sortingOrder + 1;
            }
            else
            {
                r.sortingOrder = 1;
            }
            return overlayTilemap;
        }

        // ---------------------------------------------------------------- resolution

        private void ResolveSceneRefs()
        {
            if (brushCamera == null) brushCamera = Camera.main;

            if (grid == null || groundTilemap == null)
            {
                WalkabilityMap walk = WalkabilityMap.Instance;
                if (walk != null && walk.floorTilemap != null)
                {
                    groundTilemap = walk.floorTilemap; // same object as IsoRoomBuilder.groundTilemap
                    if (grid == null) grid = walk.floorTilemap.layoutGrid;
                }
                if (grid == null) grid = FindObjectOfType<Grid>();
            }

            if (runDirector == null) runDirector = FindObjectOfType<RoomRunDirector>();
        }

        private RoomTemplateSO CurrentTemplate()
        {
            if (runDirector == null) runDirector = FindObjectOfType<RoomRunDirector>();
            return runDirector != null ? runDirector.CurrentTemplate : null;
        }

        private Vector3 MouseToWorld(Vector2 screen)
        {
            if (brushCamera == null) brushCamera = Camera.main;
            if (brushCamera == null) return Vector3.zero;
            Vector3 world = brushCamera.ScreenToWorldPoint(
                new Vector3(screen.x, screen.y, -brushCamera.transform.position.z));
            world.z = 0f;
            return world;
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        // ---------------------------------------------------------------- mode + UI

        public void SetMode(BrushMode mode)
        {
            Mode = mode;
            RefreshModeHighlight();
            UpdateStatus();
        }

        private void EnsureUi()
        {
            if (uiBuilt) return;

            brushCanvas = new GameObject("BuildTileBrushCanvas").AddComponent<Canvas>();
            brushCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            brushCanvas.sortingOrder = 5001;
            CanvasScaler scaler = brushCanvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            brushCanvas.gameObject.AddComponent<GraphicRaycaster>();
            DontDestroyOnLoad(brushCanvas.gameObject);

            // RIGHT panel = TILE BRUSH. Same width / padding / border as the BUILD panel for a
            // consistent, deliberate look across both tools.
            RectTransform root = new GameObject("BuildTileBrushRoot", typeof(RectTransform)).GetComponent<RectTransform>();
            root.SetParent(brushCanvas.transform, false);
            root.anchorMin = new Vector2(1f, 0.5f);
            root.anchorMax = new Vector2(1f, 0.5f);
            root.pivot = new Vector2(1f, 0.5f);
            root.sizeDelta = new Vector2(BuildModeUiStyle.PanelWidth, 380f);
            root.anchoredPosition = new Vector2(-BuildModeUiStyle.Padding, 0f);

            // MakePanel returns the CONTENT rect already inset by the border + Padding.
            RectTransform content = BuildModeUiStyle.MakePanel(root, "Panel", BuildModeUiStyle.PanelWidth);

            float headerH = BuildModeUiStyle.MakeHeader(content, "TILE BRUSH");

            const float hintH = 86f;
            const float radiusH = 26f;

            RectTransform list = new GameObject("Modes", typeof(RectTransform), typeof(VerticalLayoutGroup)).GetComponent<RectTransform>();
            list.SetParent(content, false);
            list.anchorMin = new Vector2(0f, 0f);
            list.anchorMax = new Vector2(1f, 1f);
            list.offsetMin = new Vector2(0f, hintH + radiusH + (BuildModeUiStyle.ItemGap * 2f));
            list.offsetMax = new Vector2(0f, -headerH);
            VerticalLayoutGroup vl = list.GetComponent<VerticalLayoutGroup>();
            vl.spacing = BuildModeUiStyle.ItemGap;
            vl.childControlHeight = true;
            vl.childForceExpandHeight = false;
            vl.childControlWidth = true;
            vl.childForceExpandWidth = true;

            modeSwatches.Clear();
            AddModeButton(list, "FLOOR", BrushMode.FloorPaint);
            AddModeButton(list, "WALKABLE", BrushMode.WalkableToggle);
            AddModeButton(list, "OVERLAY", BrushMode.OverlayPaint);

            // Small radius indicator above the hint box.
            radiusLabel = new GameObject("Radius", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            radiusLabel.rectTransform.SetParent(content, false);
            radiusLabel.rectTransform.anchorMin = new Vector2(0f, 0f);
            radiusLabel.rectTransform.anchorMax = new Vector2(1f, 0f);
            radiusLabel.rectTransform.pivot = new Vector2(0.5f, 0f);
            radiusLabel.rectTransform.sizeDelta = new Vector2(0f, radiusH);
            radiusLabel.rectTransform.anchoredPosition = new Vector2(0f, hintH + BuildModeUiStyle.ItemGap);
            radiusLabel.font = BuildModeUiStyle.Font;
            radiusLabel.fontSize = 14f;
            radiusLabel.fontStyle = FontStyles.Bold;
            radiusLabel.color = BuildModeUiStyle.Ember;
            radiusLabel.alignment = TextAlignmentOptions.MidlineLeft;
            radiusLabel.enableWordWrapping = false;
            radiusLabel.raycastTarget = false;

            hintLabel = BuildModeUiStyle.MakeHintBox(content, hintH);
            statusLabel = hintLabel; // status text rides in the hint box for the brush panel.

            uiBuilt = true;
            RefreshModeHighlight();
            UpdateStatus();
        }

        private void AddModeButton(RectTransform parent, string text, BrushMode mode)
        {
            BuildModeUiStyle.ButtonStyle b = BuildModeUiStyle.MakeButton(parent, text);
            b.button.gameObject.AddComponent<LayoutElement>().preferredHeight = 40f;
            modeSwatches.Add(b);
            BrushMode captured = mode;
            b.button.onClick.AddListener(() => SetMode(captured));
        }

        private void RefreshModeHighlight()
        {
            for (int i = 0; i < modeSwatches.Count; i++)
            {
                BuildModeUiStyle.ApplySelected(modeSwatches[i], i == (int)Mode);
            }
        }

        private void SetUiVisible(bool visible)
        {
            if (brushCanvas != null) brushCanvas.gameObject.SetActive(visible);
        }

        private void UpdateStatus(string prefix = "")
        {
            if (radiusLabel != null) radiusLabel.text = $"RADIUS  {brushRadius}";
            if (hintLabel == null) return;
            string lmb = Mode switch
            {
                BrushMode.FloorPaint => "LMB floor   RMB void",
                BrushMode.WalkableToggle => "LMB walk   RMB block",
                BrushMode.OverlayPaint => "LMB overlay   RMB clear",
                _ => string.Empty
            };
            hintLabel.text = $"{lmb}\n1 2 3 mode   - / + radius\nCtrl+Z/Y undo";
        }

        // ---------------------------------------------------------------- teardown

        private void TeardownAll()
        {
            HideCursor();
            if (brushCanvas != null) DestroyRuntimeObject(brushCanvas.gameObject);
            brushCanvas = null;
            statusLabel = null;
            hintLabel = null;
            radiusLabel = null;
            modeSwatches.Clear();
            uiBuilt = false;

            // The working copy is owned + destroyed by BuildModeController; only drop our reference.
            workTemplate = null;
        }

        private static void DestroyRuntimeObject(UnityEngine.Object target)
        {
            if (target == null) return;
            if (Application.isPlaying) Destroy(target);
            else DestroyImmediate(target);
        }

        // ---------------------------------------------------------------- *ForValidation (data-proof)
        // Overlay UI / cursor are invisible to MCP screenshots; these mirror the Phase 2 pattern so
        // the orchestrator can data-proof Phase 3 without reading pixels.

        // mode: 0 = Floor, 1 = Walkable, 2 = Overlay (matches BrushMode order).
        public bool SelectModeForValidation(int mode)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            SetMode((BrushMode)Mathf.Clamp(mode, 0, 2));
            return true;
        }

        public bool PaintFloorForValidation(Vector3Int cell)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            if (!CellInBounds(cell)) return false;
            ExecuteOp(new FloorOp(this, new List<Vector3Int> { cell }, true));
            return ReadWalkable(cell);
        }

        public bool EraseFloorForValidation(Vector3Int cell)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            if (!CellInBounds(cell)) return false;
            ExecuteOp(new FloorOp(this, new List<Vector3Int> { cell }, false));
            return !ReadWalkable(cell);
        }

        public bool ToggleWalkableForValidation(Vector3Int cell, bool walkable)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            if (!CellInBounds(cell)) return false;
            ExecuteOp(new WalkableOp(this, new List<Vector3Int> { cell }, walkable));
            return ReadWalkable(cell) == walkable;
        }

        public bool PaintOverlayForValidation(Vector3Int cell, bool paint)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            if (!CellInBounds(cell)) return false;
            ExecuteOp(new OverlayOp(this, new List<Vector3Int> { cell }, paint));
            return (ReadOverlay(cell) > 0) == paint;
        }

        // Read-backs (mirror IsWalkable / GetOverlayTileIndex on the WORKING COPY).
        public bool IsWalkableAtForValidation(Vector3Int cell)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            return ReadWalkable(cell);
        }

        public int OverlayAtForValidation(Vector3Int cell)
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            return ReadOverlay(cell);
        }

        // Live WalkabilityMap read-back: proves the brush refresh actually changed pathing authority.
        public bool WalkabilityMapAtForValidation(Vector3Int cell)
        {
            WalkabilityMap walk = WalkabilityMap.Instance;
            return walk != null && walk.IsWalkable(cell);
        }

        public bool HasCursorForValidation() => cursor != null;
        public Vector3Int BrushHighlightCellForValidation() => lastCursorCell;
        public bool LastCursorValidForValidation() => lastCursorValid;

        // Proves the source .asset is NOT polluted: the working copy must be a DIFFERENT instance
        // than the run director's live source template.
        public bool UsesWorkingCopyForValidation()
        {
            ResolveSceneRefs();
            EnsureWorkingCopy();
            RoomTemplateSO source = CurrentTemplate();
            return workTemplate != null && source != null && workTemplate != source;
        }

        public bool UndoForValidation()
        {
            BuildCommandStack stack = BuildPlacementController.Instance != null
                ? BuildPlacementController.Instance.CommandStack : null;
            return stack != null && stack.Undo();
        }

        public bool RedoForValidation()
        {
            BuildCommandStack stack = BuildPlacementController.Instance != null
                ? BuildPlacementController.Instance.CommandStack : null;
            return stack != null && stack.Redo();
        }

        // Route a validation op through the SHARED command stack (so undo/redo proofs work end-to-end).
        private void ExecuteOp(IBuildOp op)
        {
            BuildCommandStack stack = BuildPlacementController.Instance != null
                ? BuildPlacementController.Instance.CommandStack : null;
            if (stack != null) stack.Execute(op);
            else op.Do();
        }
    }
}
#endif
