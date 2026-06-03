// In-Play Map-Paint Overlay (dev / authoring tool).
//
// While the game runs in Play mode (Editor or a DEVELOPMENT_BUILD), press F2 to
// toggle a live IMGUI palette and paint TileBase assets directly onto the active
// room's floor / cliff Tilemaps — no separate Tool.exe / build required.
//
// This is a DEV tool, NOT a shipped player feature: the whole file is wrapped in
// #if UNITY_EDITOR || DEVELOPMENT_BUILD so it strips from release builds.
//
// No Editor APIs are used (the DEVELOPMENT_BUILD half must compile without
// UnityEditor). It self-bootstraps via RuntimeInitializeOnLoadMethod, so NO
// manual scene wiring is needed — just press F2 in Play mode.

#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using RIMA.Live;          // RuntimeAssetRegistry, RegistryEntry (same RIMA.Runtime assembly — Assets/Scripts/Live has no own asmdef)
using RIMA.RoomPainter;
using RIMA.Systems.Map;   // RoomLoader.OnRoomLoaded (live spine)

namespace RIMA.DevTools
{
    /// <summary>
    /// Runtime, in-Play map-paint overlay. Self-bootstrapping dev tool: F2 toggles a
    /// scrollable IMGUI tile palette; LMB paints the selected tile, RMB erases, on the
    /// active layer's Tilemap (Floor or Cliff). Mirrors the DemoDebugPanel (F1) pattern.
    /// </summary>
    public sealed class InPlayMapPaintOverlay : MonoBehaviour
    {
        // ── Bootstrap (no manual scene wiring) ─────────────────────────────────

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (FindFirstObjectByType<InPlayMapPaintOverlay>() != null) return;

            GameObject host = new GameObject("[InPlayMapPaintOverlay]");
            DontDestroyOnLoad(host);
            host.AddComponent<InPlayMapPaintOverlay>();
        }

        // ── Layer selection ────────────────────────────────────────────────────

        private enum PaintLayer { Floor, Cliff, Prop }
        private enum AssetCat { Floor, Wall, Decor }

        // ── State ──────────────────────────────────────────────────────────────

        private bool _visible;
        private bool _eraseMode;
        private PaintLayer _layer = PaintLayer.Floor;
        private float _prevTimeScale = 1f;
        private bool _pausedByOverlay;
        private Vector3 _camPosCache;
        private float _orthoCache;
        private bool _camCached;
        private bool _ppcWasEnabled;
        private Behaviour _camFollowCache;
        private bool _camFollowWasEnabled;

        private Tilemap _floorTilemap;
        private Tilemap _cliffTilemap;
        private Grid _grid;
        private GameObject _roomInstance;

        // Paintable tiles gathered from the registry (preferred) or, as a fallback,
        // scanned from the tiles already present on the discovered tilemaps.
        private readonly List<TileBase> _palette = new List<TileBase>();
        private readonly List<string> _paletteNames = new List<string>();
        private int _selected;

        private readonly List<WallPiece> _propPalette = new List<WallPiece>();
        private int _propSelected;
        private Transform _wallParent;
        private Transform _decorParent;
        private RoomData _activeRoomData;
        private string _activeRoomId;
        private RoomData _canonicalRoomData; // RoomConfig.roomData (shared asset link); null until authored.
        private bool _roomDirty;
        private List<WallCell> _wallStrokeBefore;
        private readonly Stack<List<WallCell>> _wallUndo = new Stack<List<WallCell>>();

        private bool _dragging;
        private bool _dragSet;
        private int _dragButton;
        private Vector3Int _lastCell;
        private bool _hasLastCell;
        private Vector3Int _strokeAnchor;

        private struct TileEdit
        {
            public Vector3Int cell;
            public TileBase before;
            public Tilemap tilemap;
        }

        private readonly List<TileEdit> _currentStroke = new List<TileEdit>();
        private readonly Stack<List<TileEdit>> _undo = new Stack<List<TileEdit>>();

        private Vector2 _scroll;
        private AssetCat _browserTab = AssetCat.Floor;
        private int _browserSelected = -1;
        private readonly List<BrowserEntry> _browserEntries = new List<BrowserEntry>();
        private readonly Dictionary<Sprite, Tile> _runtimeSpriteTiles = new Dictionary<Sprite, Tile>();
        private readonly Dictionary<string, Sprite> _browserSpriteByName = new Dictionary<string, Sprite>();

        private struct BrowserEntry
        {
            public Sprite sprite;
            public string name;
            public AssetCat cat;
            public Vector2 px;
            public Vector2 worldUnits;
            public Vector2Int footprint;
        }

        // The screen-space rect occupied by the IMGUI palette, so clicks over the
        // palette do not paint into the world. Stored in GUI coords (origin top-left).
        private Rect _paletteRect;

        // ── Lifecycle ──────────────────────────────────────────────────────────

        private void OnEnable()
        {
            RoomLoader.OnRoomLoaded += HandleRoomLoaded;
        }

        private void OnDisable()
        {
            RoomLoader.OnRoomLoaded -= HandleRoomLoaded;
            RestoreTimeScale();
        }

        private void OnDestroy()
        {
            RestoreTimeScale();
        }

        private void Start()
        {
            // A room may already be loaded before this component exists — discover now.
            DiscoverTilemaps(null);
        }

        // ── Input ──────────────────────────────────────────────────────────────

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.f2Key.wasPressedThisFrame)
            {
                SetVisible(!_visible);
                if (_visible && _palette.Count == 0 && _propPalette.Count == 0 && _browserEntries.Count == 0) RebuildPalette();
            }

            if (!_visible) return;

            if (keyboard != null && IsCtrlPressed(keyboard) && keyboard.zKey.wasPressedThisFrame)
            {
                UndoStroke();
                return;
            }

            Mouse mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 mousePos = mouse.position.ReadValue();
            Vector2 guiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
            bool overPalette = _paletteRect.Contains(guiPos);

            if (!_dragging && !overPalette)
            {
                if (mouse.leftButton.wasPressedThisFrame)
                    BeginStroke(set: !_eraseMode, button: 0);
                else if (mouse.rightButton.wasPressedThisFrame)
                    BeginStroke(set: false, button: 1);
            }

            if (!_dragging) return;

            if (_dragButton == 0 && mouse.rightButton.wasPressedThisFrame)
            {
                CancelStroke();
                return;
            }

            bool pressed = _dragButton == 0 ? mouse.leftButton.isPressed : mouse.rightButton.isPressed;
            if (pressed)
                ContinueStroke(mousePos, keyboard);

            bool released = _dragButton == 0 ? mouse.leftButton.wasReleasedThisFrame : mouse.rightButton.wasReleasedThisFrame;
            if (released)
                CommitStroke();
        }

        private void BeginStroke(bool set, int button)
        {
            _dragging = true;
            _dragSet = set;
            _dragButton = button;
            _hasLastCell = false;
            _strokeAnchor = CellUnderMouse();
            _currentStroke.Clear();

            if (_layer == PaintLayer.Prop)
            {
                EnsureActiveRoomData();
                _wallStrokeBefore = _activeRoomData != null
                    ? new List<WallCell>(_activeRoomData.wallCells)
                    : null;
            }
        }

        private void ContinueStroke(Vector2 screenPos, Keyboard keyboard)
        {
            Vector3Int cell = CellUnderMouse();
            if (IsShiftPressed(keyboard))
                cell = AxisLockedCell(_strokeAnchor, cell);

            if (_hasLastCell && cell == _lastCell) return;

            Vector3Int start = _hasLastCell ? _lastCell : cell;
            BrowserEntry browserEntry;
            if (TrySelectedBrowserEntry(out browserEntry))
            {
                if (browserEntry.cat == AssetCat.Wall)
                {
                    if (_dragSet)
                        BuildBrowserWallRun(start, cell, browserEntry);
                    else
                        ErasePropRun(start, cell);
                }
                else
                {
                    foreach (Vector3Int c in GridLine(start, cell))
                    {
                        if (_dragSet)
                            PlaceBrowserEntry(c, browserEntry);
                        else
                            EraseBrowserEntry(c, browserEntry.cat);
                    }
                }
            }
            else if (_layer == PaintLayer.Prop)
            {
                if (_dragSet)
                    BuildPropRun(start, cell);
                else
                    ErasePropRun(start, cell);
            }
            else
            {
                foreach (Vector3Int c in GridLine(start, cell))
                    PaintCell(c, _dragSet);
            }

            _lastCell = cell;
            _hasLastCell = true;
        }

        private void CommitStroke()
        {
            if (_layer == PaintLayer.Prop)
            {
                if (_wallStrokeBefore != null && _activeRoomData != null && WallCellsChanged(_wallStrokeBefore, _activeRoomData.wallCells))
                {
                    _wallUndo.Push(_wallStrokeBefore);
                    _roomDirty = true;
                }
            }
            else if (_currentStroke.Count > 0)
            {
                _undo.Push(new List<TileEdit>(_currentStroke));
            }

            _dragging = false;
            _currentStroke.Clear();
            _wallStrokeBefore = null;
        }

        private void CancelStroke()
        {
            if (_layer == PaintLayer.Prop)
            {
                if (_wallStrokeBefore != null && _activeRoomData != null)
                {
                    _activeRoomData.wallCells = new List<WallCell>(_wallStrokeBefore);
                    ComposeWallCellsRuntime();
                }
            }
            else
            {
                RestoreTileEdits(_currentStroke);
            }

            _dragging = false;
            _currentStroke.Clear();
            _wallStrokeBefore = null;
        }

        private void UndoStroke()
        {
            if (_layer == PaintLayer.Prop)
            {
                if (_wallUndo.Count == 0 || _activeRoomData == null) return;

                _activeRoomData.wallCells = _wallUndo.Pop();
                WangRebuild.ReorientWallCells(_activeRoomData, AllWallCells(_activeRoomData));
                ComposeWallCellsRuntime();
                _roomDirty = true;
                return;
            }

            if (_undo.Count == 0) return;
            RestoreTileEdits(_undo.Pop());
        }

        private static bool IsCtrlPressed(Keyboard keyboard)
        {
            return keyboard != null && (keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed);
        }

        private static bool IsShiftPressed(Keyboard keyboard)
        {
            return keyboard != null && (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);
        }

        private void SetVisible(bool visible)
        {
            if (_visible == visible) return;

            _visible = visible;
            if (_visible)
            {
                _prevTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                _pausedByOverlay = true;
                EnterOverviewCamera();
            }
            else
            {
                RestoreTimeScale();
            }
        }

        private void RestoreTimeScale()
        {
            ExitOverviewCamera();
            if (!_pausedByOverlay) return;

            Time.timeScale = _prevTimeScale;
            _pausedByOverlay = false;
        }

        private void EnterOverviewCamera()
        {
            Camera cam = Camera.main;
            if (cam == null || _camCached) return;

            Tilemap floor = _floorTilemap != null ? _floorTilemap : FindOverviewFloorTilemap();
            if (floor == null) return;

            floor.CompressBounds();
            Bounds bounds = floor.localBounds;
            if (bounds.size.x <= 0f || bounds.size.y <= 0f) return;

            _floorTilemap = floor;
            _camPosCache = cam.transform.position;
            _orthoCache = cam.orthographicSize;

            UnityEngine.Rendering.Universal.PixelPerfectCamera ppc =
                cam.GetComponent<UnityEngine.Rendering.Universal.PixelPerfectCamera>();
            if (ppc != null)
            {
                _ppcWasEnabled = ppc.enabled;
                ppc.enabled = false;
            }

            _camFollowCache = FindCameraFollow(cam);
            if (_camFollowCache != null)
            {
                _camFollowWasEnabled = _camFollowCache.enabled;
                _camFollowCache.enabled = false;
            }

            Vector3 worldCenter = floor.transform.TransformPoint(bounds.center);
            cam.transform.position = new Vector3(worldCenter.x, worldCenter.y, _camPosCache.z);

            float halfH = bounds.size.y * 0.5f;
            float halfW = (bounds.size.x * 0.5f) / cam.aspect;
            cam.orthographicSize = Mathf.Max(halfH, halfW) * 1.15f;
            _camCached = true;
        }

        private void ExitOverviewCamera()
        {
            if (!_camCached) return;

            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.transform.position = _camPosCache;
                cam.orthographicSize = _orthoCache;

                UnityEngine.Rendering.Universal.PixelPerfectCamera ppc =
                    cam.GetComponent<UnityEngine.Rendering.Universal.PixelPerfectCamera>();
                if (ppc != null) ppc.enabled = _ppcWasEnabled;
            }

            if (_camFollowCache != null) _camFollowCache.enabled = _camFollowWasEnabled;
            _camFollowCache = null;
            _camCached = false;
        }

        private Tilemap FindOverviewFloorTilemap()
        {
            if (_grid != null)
            {
                Tilemap[] gridTilemaps = _grid.GetComponentsInChildren<Tilemap>(true);
                for (int i = 0; i < gridTilemaps.Length; i++)
                {
                    if (IsFloorTilemap(gridTilemaps[i])) return gridTilemaps[i];
                }
            }

            Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < tilemaps.Length; i++)
            {
                if (IsFloorTilemap(tilemaps[i])) return tilemaps[i];
            }

            return null;
        }

        private static bool IsFloorTilemap(Tilemap tilemap)
        {
            if (tilemap == null) return false;
            string name = tilemap.name.ToLowerInvariant();
            return name.Contains("floor") || name.Contains("ground");
        }

        private static Behaviour FindCameraFollow(Camera cam)
        {
            if (cam == null) return null;

            MonoBehaviour[] behaviours = cam.GetComponents<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                Behaviour behaviour = behaviours[i];
                if (behaviour != null && behaviour.GetType().Name == "CameraFollow")
                    return behaviour;
            }

            return null;
        }

        private static Vector3Int AxisLockedCell(Vector3Int anchor, Vector3Int cell)
        {
            int dx = cell.x - anchor.x;
            int dy = cell.y - anchor.y;
            if (Mathf.Abs(dx) >= Mathf.Abs(dy))
                return new Vector3Int(cell.x, anchor.y, cell.z);
            return new Vector3Int(anchor.x, cell.y, cell.z);
        }

        // ── Painting ───────────────────────────────────────────────────────────

        private Vector3Int CellUnderMouse()
        {
            Camera cam = Camera.main;
            if (_grid == null || cam == null) return Vector3Int.zero;

            Vector2 screenPos = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
            Vector3 world = cam.ScreenToWorldPoint(screenPos);
            world.z = 0f;
            return _grid.WorldToCell(world);
        }

        private void PaintCell(Vector3Int cell, bool set)
        {
            Tilemap target = ActiveTilemap();
            if (target == null) return;

            TileBase before = target.GetTile(cell);
            TileBase next = set ? SelectedTile() : null;
            if (set && next == null) return;
            if (before == next) return;

            _currentStroke.Add(new TileEdit { cell = cell, before = before, tilemap = target });
            target.SetTile(cell, next);
        }

        private static void RestoreTileEdits(List<TileEdit> edits)
        {
            if (edits == null) return;
            for (int i = edits.Count - 1; i >= 0; i--)
            {
                Tilemap target = edits[i].tilemap;
                if (target != null) target.SetTile(edits[i].cell, edits[i].before);
            }
        }

        private Tilemap ActiveTilemap()
        {
            if (_layer == PaintLayer.Floor) return _floorTilemap;
            if (_layer == PaintLayer.Cliff) return _cliffTilemap;
            return null;
        }

        private TileBase SelectedTile()
        {
            if (_selected < 0 || _selected >= _palette.Count) return null;
            return _palette[_selected];
        }

        private bool TrySelectedBrowserEntry(out BrowserEntry entry)
        {
            if (_browserSelected >= 0 && _browserSelected < _browserEntries.Count)
            {
                entry = _browserEntries[_browserSelected];
                return entry.sprite != null;
            }

            entry = new BrowserEntry();
            return false;
        }

        private void PlaceBrowserEntry(Vector3Int cell, BrowserEntry entry)
        {
            if (entry.cat == AssetCat.Floor)
            {
                PlaceBrowserFloorCell(cell, entry);
            }
            else if (entry.cat == AssetCat.Decor)
            {
                PlaceBrowserDecor(cell, entry);
            }
        }

        private void EraseBrowserEntry(Vector3Int cell, AssetCat cat)
        {
            EnsureActiveRoomData();
            if (_activeRoomData == null) return;

            if (cat == AssetCat.Floor)
            {
                Tilemap target = _floorTilemap;
                TileBase before = target != null ? target.GetTile(cell) : null;
                if (target != null && before != null)
                {
                    _currentStroke.Add(new TileEdit { cell = cell, before = before, tilemap = target });
                    target.SetTile(cell, null);
                }

                RoomDataMutator.RemoveFloorCell(_activeRoomData, cell);
                ResolveFloorCellsAround(cell, null);
                _roomDirty = true;
            }
            else if (cat == AssetCat.Decor)
            {
                RoomDataMutator.RemoveProp(_activeRoomData, cell, RoomLayer.Props);
                ClearDecorAtCell(cell);
                _roomDirty = true;
            }
        }

        private void PlaceBrowserFloorCell(Vector3Int cell, BrowserEntry entry)
        {
            if (_grid == null || entry.sprite == null) return;

            EnsureActiveRoomData();
            if (_activeRoomData == null) return;

            Tilemap target = _floorTilemap;
            Vector3 worldPos = _grid.GetCellCenterWorld(cell);
            RoomDataMutator.PutFloorCell(_activeRoomData, entry.name, cell, worldPos, 0f, Vector2.one);
            ResolveFloorCellsAround(cell, entry.sprite);
            TileBase after = target != null ? target.GetTile(cell) : null;
            if (target != null && after == null)
                SetBrowserTile(target, cell, entry.sprite);
            _roomDirty = true;
        }

        private void ResolveFloorCellsAround(Vector3Int cell, Sprite fallbackSprite)
        {
            if (_activeRoomData == null) return;

            Vector3Int[] targets =
            {
                cell,
                cell + Vector3Int.up,
                cell + Vector3Int.right,
                cell + Vector3Int.down,
                cell + Vector3Int.left
            };

            for (int i = 0; i < targets.Length; i++)
            {
                Vector3Int targetCell = targets[i];
                if (!FloorCellOccupied(targetCell)) continue;

                int key = FloorWangResolver.ResolveFloorTile(targetCell, FloorCellOccupied);
                string assetName = FloorWangResolver.FloorAssetName(targetCell, key);
                RewriteFloorCellAsset(targetCell, assetName);

                Sprite resolved = FindBrowserSprite(assetName, AssetCat.Floor);
                if (resolved == null && targetCell == cell) resolved = fallbackSprite;
                if (resolved != null && _floorTilemap != null)
                    SetBrowserTile(_floorTilemap, targetCell, resolved);
            }
        }

        private bool FloorCellOccupied(Vector3Int cell)
        {
            if (_activeRoomData == null || _activeRoomData.floorCells == null) return false;
            for (int i = 0; i < _activeRoomData.floorCells.Count; i++)
            {
                if (_activeRoomData.floorCells[i].cell == cell) return true;
            }

            return false;
        }

        private void RewriteFloorCellAsset(Vector3Int cell, string assetName)
        {
            if (_activeRoomData == null || _activeRoomData.floorCells == null) return;
            for (int i = 0; i < _activeRoomData.floorCells.Count; i++)
            {
                RoomData.TileCellRecord record = _activeRoomData.floorCells[i];
                if (record.cell != cell) continue;
                record.assetGuidOrName = assetName;
                _activeRoomData.floorCells[i] = record;
                return;
            }
        }

        private void SetBrowserTile(Tilemap target, Vector3Int cell, Sprite sprite)
        {
            if (target == null || sprite == null) return;

            TileBase before = target.GetTile(cell);
            Tile next = RuntimeTileFor(sprite);
            if (before == next) return;

            _currentStroke.Add(new TileEdit { cell = cell, before = before, tilemap = target });
            target.SetTile(cell, next);
        }

        private Tile RuntimeTileFor(Sprite sprite)
        {
            if (sprite == null) return null;
            Tile tile;
            if (_runtimeSpriteTiles.TryGetValue(sprite, out tile) && tile != null) return tile;

            tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = sprite.name;
            tile.sprite = sprite;
            _runtimeSpriteTiles[sprite] = tile;
            return tile;
        }

        private Sprite FindBrowserSprite(string assetName, AssetCat cat)
        {
            if (string.IsNullOrEmpty(assetName)) return null;

            Sprite sprite;
            string key = BrowserSpriteKey(assetName, cat);
            if (_browserSpriteByName.TryGetValue(key, out sprite)) return sprite;
            if (_browserSpriteByName.TryGetValue(BrowserSpriteKey(assetName.ToLowerInvariant(), cat), out sprite)) return sprite;
            return null;
        }

        private static IEnumerable<Vector3Int> GridLine(Vector3Int a, Vector3Int b)
        {
            int x0 = a.x;
            int y0 = a.y;
            int x1 = b.x;
            int y1 = b.y;
            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                yield return new Vector3Int(x0, y0, a.z);
                if (x0 == x1 && y0 == y1) break;

                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        // ── Prop placement ────────────────────────────────────────────────────

        private void BuildBrowserWallRun(Vector3Int fromCell, Vector3Int toCell, BrowserEntry entry)
        {
            if (_grid == null || entry.sprite == null) return;

            EnsureActiveRoomData();
            if (_activeRoomData == null) return;

            RoomDataMutator.AppendWallRun(_activeRoomData, fromCell, toCell, entry.name, entry.footprint);
            ComposeWallCellsRuntime();
        }

        private void BuildPropRun(Vector3Int fromCell, Vector3Int toCell)
        {
            if (_grid == null || _propSelected < 0 || _propSelected >= _propPalette.Count) return;

            EnsureActiveRoomData();
            if (_activeRoomData == null) return;

            WallPiece piece = _propPalette[_propSelected];
            RoomDataMutator.AppendWallRun(_activeRoomData, fromCell, toCell, piece.pieceId, piece.footprint);
            ComposeWallCellsRuntime();
        }

        private void ErasePropRun(Vector3Int fromCell, Vector3Int toCell)
        {
            EnsureActiveRoomData();
            if (_activeRoomData == null) return;

            foreach (Vector3Int cell in GridLine(fromCell, toCell))
            {
                RoomDataMutator.RemoveWallCell(_activeRoomData, cell);
            }

            ComposeWallCellsRuntime();
        }

        private void PlaceBrowserDecor(Vector3Int cell, BrowserEntry entry)
        {
            if (_grid == null || entry.sprite == null) return;

            EnsureActiveRoomData();
            if (_activeRoomData == null) return;

            Vector3 worldPos = _grid.GetCellCenterWorld(cell);
            RoomDataMutator.PutProp(_activeRoomData, entry.name, cell, worldPos, 0f, Vector2.one, RoomLayer.Props);
            PlaceDecorSprite(cell, entry);
            _roomDirty = true;
        }

        private Transform DecorParent()
        {
            if (_decorParent != null) return _decorParent;

            Transform root = _roomInstance != null ? _roomInstance.transform : null;
            Transform existing = root != null ? root.Find("[RoomData_Props]") : null;
            if (existing == null)
            {
                GameObject sceneExisting = GameObject.Find("[RoomData_Props]");
                if (sceneExisting != null) existing = sceneExisting.transform;
            }

            if (existing != null)
            {
                _decorParent = existing;
                return _decorParent;
            }

            GameObject go = new GameObject("[RoomData_Props]");
            if (root != null) go.transform.SetParent(root, false);
            _decorParent = go.transform;
            return _decorParent;
        }

        private void PlaceDecorSprite(Vector3Int cell, BrowserEntry entry)
        {
            ClearDecorAtCell(cell);

            Transform parent = DecorParent();
            if (parent == null) return;

            GameObject go = new GameObject(DecorObjectName(cell));
            go.transform.SetParent(parent, false);
            go.transform.position = _grid.GetCellCenterWorld(cell);
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = entry.sprite;
            renderer.sortingLayerName = "Entities";
        }

        private void ClearDecorAtCell(Vector3Int cell)
        {
            Transform parent = DecorParent();
            if (parent == null) return;

            string wanted = DecorObjectName(cell);
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child != null && child.name == wanted)
                    Destroy(child.gameObject);
            }
        }

        private static string DecorObjectName(Vector3Int cell)
        {
            return "[BrowserProp] " + cell.x + "_" + cell.y + "_" + cell.z;
        }

        private Transform WallParent()
        {
            if (_wallParent != null) return _wallParent;

            Transform root = _roomInstance != null ? _roomInstance.transform : null;
            Transform existing = root != null ? root.Find("[RoomData_Walls]") : null;
            if (existing == null)
            {
                GameObject sceneExisting = GameObject.Find("[RoomData_Walls]");
                if (sceneExisting != null) existing = sceneExisting.transform;
            }

            if (existing != null)
            {
                _wallParent = existing;
                return _wallParent;
            }

            GameObject go = new GameObject("[RoomData_Walls]");
            if (root != null) go.transform.SetParent(root, false);
            _wallParent = go.transform;
            return _wallParent;
        }

        private void ComposeWallCellsRuntime()
        {
            if (_grid == null || _activeRoomData == null) return;

            Transform parent = WallParent();
            ClearChildren(parent);
            _activeRoomData.EnsureDefaults();

            for (int i = 0; i < _activeRoomData.wallCells.Count; i++)
            {
                WallCell wallCell = _activeRoomData.wallCells[i];
                WallPiece piece = ResolveWallPiece(wallCell.pieceId);
                WallRunBuilder.PlaceOne(_grid, wallCell.cell, piece, parent, wallCell.shape, wallCell.rotation);
            }
        }

        private WallPiece ResolveWallPiece(string pieceId)
        {
            for (int i = 0; i < _propPalette.Count; i++)
            {
                WallPiece piece = _propPalette[i];
                if (piece.pieceId == pieceId || piece.displayName == pieceId)
                {
                    return piece;
                }
            }

            for (int i = 0; i < _browserEntries.Count; i++)
            {
                BrowserEntry browserEntry = _browserEntries[i];
                if (browserEntry.cat != AssetCat.Wall) continue;
                if (browserEntry.name == pieceId || (browserEntry.sprite != null && browserEntry.sprite.name == pieceId))
                    return WallPieceFromBrowserEntry(browserEntry);
            }

            RuntimeAssetRegistry registry = RuntimeAssetRegistry.Instance;
            RegistryEntry entry = registry != null ? registry.Get(pieceId) : null;
            if (entry != null)
            {
                return WallPieceFromRegistryEntry(entry, null);
            }

            return new WallPiece
            {
                footprint = Vector2Int.one,
                displayName = string.IsNullOrEmpty(pieceId) ? "Wall" : pieceId,
                pieceId = pieceId
            };
        }

        private static WallPiece WallPieceFromBrowserEntry(BrowserEntry entry)
        {
            return new WallPiece
            {
                prefab = null,
                sprite = entry.sprite,
                straightSprite = entry.sprite,
                footprint = entry.footprint == Vector2Int.zero ? Vector2Int.one : entry.footprint,
                displayName = entry.name,
                pieceId = entry.name
            };
        }

        private static void ClearChildren(Transform parent)
        {
            if (parent == null) return;
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }

        private static void ClearLegacyPropParent(GameObject roomInstance)
        {
            Transform root = roomInstance != null ? roomInstance.transform : null;
            Transform legacy = root != null ? root.Find("[DragPlace_Props]") : null;
            if (legacy == null)
            {
                GameObject sceneLegacy = GameObject.Find("[DragPlace_Props]");
                if (sceneLegacy != null) legacy = sceneLegacy.transform;
            }

            if (legacy != null)
            {
                Destroy(legacy.gameObject);
            }
        }

        private void EnsureActiveRoomData()
        {
            if (_activeRoomData != null) return;
            LoadActiveRoomData();
        }

        private void LoadActiveRoomData()
        {
            string roomId = _activeRoomId;
            if (string.IsNullOrEmpty(roomId))
            {
                Debug.LogWarning("[InPlayMapPaintOverlay] Active room has no roomId / RoomData " +
                    "reference; edits fall back to the shared 'runtime_room' file and may clobber " +
                    "other rooms. Assign RoomConfig.roomData (or RoomConfig.roomId) to give each " +
                    "room its own canonical RoomData.");
                roomId = "runtime_room";
            }

            string path = RoomDataPaths.JsonFor(roomId);
            _activeRoomData = RoomDataJson.ReadRoom(path);
            if (_activeRoomData == null)
            {
                if (_canonicalRoomData != null)
                {
                    // No JSON sidecar yet: seed from the canonical asset (cloned so play-mode
                    // edits stay uncommitted until Save) and fold any legacy wallSegments in, so
                    // a segment-only room shows its walls in F2 instead of appearing empty.
                    _activeRoomData = Instantiate(_canonicalRoomData);
                    _activeRoomData.EnsureDefaults();
                    RoomDataMutator.MigrateSegmentsToCells(_activeRoomData);
                }
                else
                {
                    _activeRoomData = ScriptableObject.CreateInstance<RoomData>();
                    _activeRoomData.roomId = roomId;
                    _activeRoomData.displayName = roomId;
                    _activeRoomData.EnsureDefaults();
                }
            }

            _activeRoomId = _activeRoomData.roomId;
            _roomDirty = false;
            _wallUndo.Clear();
        }

        private void SaveActiveRoomData()
        {
            if (_activeRoomData == null) return;

            _activeRoomData.EnsureDefaults();
            RoomDataJson.Write(_activeRoomData, RoomDataPaths.JsonFor(_activeRoomData.roomId));
#if UNITY_EDITOR
            CopyRoomDataToAsset(_activeRoomData);
#endif
            _roomDirty = false;
        }

#if UNITY_EDITOR
        private static void CopyRoomDataToAsset(RoomData source)
        {
            if (source == null || string.IsNullOrEmpty(source.roomId)) return;

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:RoomData", new[] { "Assets/Data/Rooms" });
            for (int i = 0; i < guids.Length; i++)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                RoomData target = UnityEditor.AssetDatabase.LoadAssetAtPath<RoomData>(path);
                if (target == null || target.roomId != source.roomId) continue;

                UnityEditor.EditorUtility.CopySerialized(source, target);
                target.EnsureDefaults();
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                return;
            }

            // No existing asset matched this roomId: create one so an F2-authored room
            // becomes visible in the Editor Map Library (t:RoomData) instead of living
            // only in the JSON sidecar. Pairs with the RoomConfig.roomData id contract.
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Data"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Data");
            }
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Data/Rooms"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets/Data", "Rooms");
            }

            RoomData created = Instantiate(source);
            created.EnsureDefaults();
            UnityEditor.AssetDatabase.CreateAsset(created, RoomDataPaths.AssetPathFor(source.roomId));
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif

        private static bool WallCellsChanged(List<WallCell> before, List<WallCell> after)
        {
            if (before == null || after == null) return before != after;
            if (before.Count != after.Count) return true;
            for (int i = 0; i < before.Count; i++)
            {
                if (before[i].cell != after[i].cell ||
                    before[i].shape != after[i].shape ||
                    !Mathf.Approximately(before[i].rotation, after[i].rotation) ||
                    before[i].pieceId != after[i].pieceId)
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<Vector3Int> AllWallCells(RoomData room)
        {
            if (room == null || room.wallCells == null) yield break;
            for (int i = 0; i < room.wallCells.Count; i++)
            {
                yield return room.wallCells[i].cell;
            }
        }

        private static Vector2Int FootprintFromSpriteSize(Sprite sprite)
        {
            if (sprite == null) return Vector2Int.one;
            return new Vector2Int(
                Mathf.Max(1, Mathf.CeilToInt(sprite.rect.width / 64f)),
                Mathf.Max(1, Mathf.CeilToInt(sprite.rect.height / 64f)));
        }

        // ── Tilemap discovery ──────────────────────────────────────────────────

        private void HandleRoomLoaded(RoomConfig config, GameObject roomInstance)
        {
            _roomInstance = roomInstance;
            _wallParent = null;
            _decorParent = null;
            // Canonical shared RoomData: prefer the explicit RoomConfig.roomData asset so the
            // F2 overlay and the Editor edit the SAME data (same roomId -> same JSON sidecar +
            // same .asset). Fall back to the hand-authored roomId string only for rooms not
            // yet wired to an asset.
            _canonicalRoomData = config != null ? config.roomData : null;
            if (_canonicalRoomData != null) _canonicalRoomData.EnsureDefaults(); // guarantee a non-blank roomId so it can't collapse to the shared runtime_room file
            _activeRoomId = _canonicalRoomData != null
                ? _canonicalRoomData.roomId
                : (config != null ? config.roomId : null);
            DiscoverTilemaps(roomInstance);
            ClearLegacyPropParent(roomInstance);
            LoadActiveRoomData();
            ComposeWallCellsRuntime();
            // New room may carry new tile types if we're on the fallback scan path.
            if (_palette.Count == 0 || _propPalette.Count == 0) RebuildPalette();
        }

        private void DiscoverTilemaps(GameObject roomInstance)
        {
            _floorTilemap = null;
            _cliffTilemap = null;

            // Prefer the freshly loaded room's tilemaps; otherwise scan the whole scene
            // (covers the case where a room was already loaded before bootstrap).
            Tilemap[] tilemaps = roomInstance != null
                ? roomInstance.GetComponentsInChildren<Tilemap>(true)
                : FindObjectsByType<Tilemap>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (Tilemap tm in tilemaps)
            {
                if (tm == null) continue;
                string name = tm.name.ToLowerInvariant();
                if (_floorTilemap == null && (name.Contains("floor") || name.Contains("ground"))) _floorTilemap = tm;
                if (_cliffTilemap == null && name.Contains("cliff")) _cliffTilemap = tm;
            }

            // Grid is shared by all tilemaps; take it from whichever we found.
            Tilemap any = _floorTilemap != null ? _floorTilemap : _cliffTilemap;
            _grid = any != null ? any.layoutGrid : FindFirstObjectByType<Grid>();
        }

        // ── Palette construction ───────────────────────────────────────────────

        // Path: registry-first (baked RuntimeAssetRegistry — accessible since it lives
        // in RIMA.Runtime), fall back to scanning tiles already on the tilemaps so the
        // tool still works when the registry is missing/unbaked or carries no tiles.
        private void RebuildPalette()
        {
            _palette.Clear();
            _paletteNames.Clear();
            _selected = 0;
            _propPalette.Clear();
            _propSelected = 0;
            _browserEntries.Clear();
            _browserSpriteByName.Clear();
            _browserSelected = -1;

            HashSet<TileBase> seen = new HashSet<TileBase>();

            RuntimeAssetRegistry registry = RuntimeAssetRegistry.Instance;
            if (registry != null)
            {
                foreach (RegistryEntry e in registry.Entries)
                {
                    if (e == null || e.tile == null || !seen.Add(e.tile)) continue;
                    _palette.Add(e.tile);
                    _paletteNames.Add(string.IsNullOrEmpty(e.displayName) ? e.tile.name : e.displayName);
                }

                RebuildPropPalette(registry);
            }

            if (_palette.Count == 0)
            {
                ScanTilemapTiles(_floorTilemap, seen);
                ScanTilemapTiles(_cliffTilemap, seen);
            }

            if (_propPalette.Count == 0)
                LoadResourceWallKit();

            LoadIsoKitBrowser();
            SelectFirstBrowserEntry(_browserTab);
        }

        private void LoadIsoKitBrowser()
        {
            HashSet<Sprite> seen = new HashSet<Sprite>();
            AddGraniteFloorSprites(seen);
            AddIsoKitSprites("Assets/Sprites/Environment/IsoKit/floor", "Environment/IsoKit/floor", AssetCat.Floor, seen);
            AddIsoKitSprites("Assets/Sprites/Environment/IsoKit/walls", "Environment/IsoKit/walls", AssetCat.Wall, seen);
            AddIsoKitSprites("Assets/Sprites/Environment/IsoKit/decor", "Environment/IsoKit/decor", AssetCat.Decor, seen);
        }

        private void AddGraniteFloorSprites(HashSet<Sprite> seen)
        {
            string[] liveGraniteNames = { "floor451_0", "floor451_1", "floor451_14", "floor451_15" };

#if UNITY_EDITOR
            const string graniteFolder = "Assets/Sprites/Environment/PixelLabFloor451";
            for (int i = 0; i < liveGraniteNames.Length; i++)
            {
                string spriteName = liveGraniteNames[i];
                string path = graniteFolder + "/" + spriteName + ".png";
                Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
                for (int j = 0; j < assets.Length; j++)
                {
                    Sprite sprite = assets[j] as Sprite;
                    if (sprite == null) continue;
                    if (sprite.name != spriteName && sprite.name != spriteName + "_0") continue;
                    if (!seen.Add(sprite)) continue;
                    AddBrowserEntry(sprite, AssetCat.Floor);
                }
            }
#else
            Sprite[] sprites = Resources.LoadAll<Sprite>("Environment/PixelLabFloor451");
            AddGraniteFloorSpritesFromResources(sprites, liveGraniteNames, seen);
            Sprite[] mirroredSprites = Resources.LoadAll<Sprite>("Sprites/Environment/PixelLabFloor451");
            AddGraniteFloorSpritesFromResources(mirroredSprites, liveGraniteNames, seen);
#endif
        }

#if !UNITY_EDITOR
        private void AddGraniteFloorSpritesFromResources(Sprite[] sprites, string[] liveGraniteNames, HashSet<Sprite> seen)
        {
            for (int i = 0; i < liveGraniteNames.Length; i++)
            {
                string spriteName = liveGraniteNames[i];
                for (int j = 0; j < sprites.Length; j++)
                {
                    Sprite sprite = sprites[j];
                    if (sprite == null) continue;
                    if (sprite.name != spriteName && sprite.name != spriteName + "_0") continue;
                    if (!seen.Add(sprite)) continue;
                    AddBrowserEntry(sprite, AssetCat.Floor);
                }
            }
        }
#endif

        private void AddIsoKitSprites(string assetFolder, string resourcesPath, AssetCat cat, HashSet<Sprite> seen)
        {
#if UNITY_EDITOR
            if (UnityEditor.AssetDatabase.IsValidFolder(assetFolder))
            {
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Sprite", new[] { assetFolder });
                foreach (string guid in guids)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
                    foreach (Object asset in assets)
                    {
                        Sprite sprite = asset as Sprite;
                        if (sprite == null || !seen.Add(sprite)) continue;
                        AddBrowserEntry(sprite, cat);
                    }
                }
            }
#else
            Sprite[] sprites = Resources.LoadAll<Sprite>(resourcesPath);
            foreach (Sprite sprite in sprites)
            {
                if (sprite == null || !seen.Add(sprite)) continue;
                AddBrowserEntry(sprite, cat);
            }

            Sprite[] mirroredSprites = Resources.LoadAll<Sprite>("Sprites/" + resourcesPath);
            foreach (Sprite sprite in mirroredSprites)
            {
                if (sprite == null || !seen.Add(sprite)) continue;
                AddBrowserEntry(sprite, cat);
            }
#endif
        }

        private void AddBrowserEntry(Sprite sprite, AssetCat cat)
        {
            if (sprite == null) return;

            float ppu = Mathf.Approximately(sprite.pixelsPerUnit, 0f) ? 100f : sprite.pixelsPerUnit;
            Vector2 px = sprite.rect.size;
            BrowserEntry entry = new BrowserEntry
            {
                sprite = sprite,
                name = sprite.name,
                cat = cat,
                px = px,
                worldUnits = px / ppu,
                footprint = FootprintFromSpriteSize(sprite)
            };

            _browserEntries.Add(entry);
            _browserSpriteByName[BrowserSpriteKey(entry.name, cat)] = sprite;
            _browserSpriteByName[BrowserSpriteKey(entry.name.ToLowerInvariant(), cat)] = sprite;
        }

        private static string BrowserSpriteKey(string name, AssetCat cat)
        {
            return cat.ToString() + ":" + (string.IsNullOrEmpty(name) ? string.Empty : name);
        }

        private void RebuildPropPalette(RuntimeAssetRegistry registry)
        {
            if (registry == null) return;

            Dictionary<string, RegistryEntry> byName = new Dictionary<string, RegistryEntry>();
            List<RegistryEntry> entries = new List<RegistryEntry>();
            AddTaggedEntries(registry.GetByTag("wall"), entries, byName);
            AddTaggedEntries(registry.GetByTag("prop"), entries, byName);

            HashSet<Object> seenAssets = new HashSet<Object>();
            foreach (RegistryEntry e in entries)
            {
                if (e == null) continue;
                Object key = e.prefab != null ? e.prefab : e.sprite;
                if (key == null || !seenAssets.Add(key)) continue;

                _propPalette.Add(WallPieceFromRegistryEntry(e, byName));
            }
        }

        private static void AddTaggedEntries(
            IReadOnlyList<RegistryEntry> source,
            List<RegistryEntry> entries,
            Dictionary<string, RegistryEntry> byName)
        {
            if (source == null) return;
            foreach (RegistryEntry e in source)
            {
                if (e == null) continue;
                entries.Add(e);

                string displayName = !string.IsNullOrEmpty(e.displayName)
                    ? e.displayName
                    : e.prefab != null
                        ? e.prefab.name
                        : e.sprite != null
                            ? e.sprite.name
                            : null;

                if (string.IsNullOrEmpty(displayName)) continue;
                byName[NormalizeName(displayName)] = e;
            }
        }

        private static string NormalizeName(string name)
        {
            return string.IsNullOrEmpty(name) ? string.Empty : name.Trim().ToLowerInvariant();
        }

        private static WallPiece WallPieceFromRegistryEntry(
            RegistryEntry entry,
            Dictionary<string, RegistryEntry> byName)
        {
            Object key = entry.prefab != null ? entry.prefab : entry.sprite;
            Sprite sprite = entry.sprite;
            string displayName = string.IsNullOrEmpty(entry.displayName)
                ? key != null ? key.name : entry.guid
                : entry.displayName;

            return new WallPiece
            {
                prefab = entry.prefab,
                sprite = sprite,
                straightSprite = VariantSprite(byName, displayName, "straight") ?? sprite,
                cornerSprite = VariantSprite(byName, displayName, "corner"),
                tSprite = VariantSprite(byName, displayName, "t"),
                crossSprite = VariantSprite(byName, displayName, "cross"),
                endSprite = VariantSprite(byName, displayName, "end"),
                singleSprite = VariantSprite(byName, displayName, "single"),
                footprint = FootprintFromSpriteSize(sprite),
                displayName = displayName,
                pieceId = entry.guid
            };
        }

        private static Sprite VariantSprite(
            Dictionary<string, RegistryEntry> byName,
            string displayName,
            string suffix)
        {
            if (byName == null || string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(suffix))
            {
                return null;
            }

            string normalized = NormalizeName(displayName);
            string wanted = normalized + "_" + suffix;
            if (byName.TryGetValue(wanted, out RegistryEntry exact) && exact != null)
            {
                return exact.sprite;
            }

            string compactWanted = normalized + suffix;
            if (byName.TryGetValue(compactWanted, out RegistryEntry compact) && compact != null)
            {
                return compact.sprite;
            }

            return null;
        }

        private void LoadResourceWallKit()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Live/WallKit");
            foreach (Sprite sprite in sprites)
            {
                if (sprite == null) continue;
                _propPalette.Add(new WallPiece
                {
                    prefab = null,
                    sprite = sprite,
                    straightSprite = sprite,
                    footprint = FootprintFromSpriteSize(sprite),
                    displayName = sprite.name,
                    pieceId = sprite.name
                });
            }

#if UNITY_EDITOR
            // Editor fallback: the Ruined-Keep wall kit lives under Assets/ (not Resources,
            // not yet re-baked into the registry). Scan it directly so Prop drag-place works
            // in the Editor F2 tool without waiting for a registry re-bake.
            if (_propPalette.Count == 0)
            {
                const string kitFolder = "Assets/Sprites/Environment/RuinedKeepKit";
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Sprite", new[] { kitFolder });
                foreach (string guid in guids)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    if (sprite == null) continue;
                    _propPalette.Add(new WallPiece
                    {
                        prefab = null,
                        sprite = sprite,
                        straightSprite = sprite,
                        footprint = FootprintFromSpriteSize(sprite),
                        displayName = sprite.name,
                        pieceId = guid
                    });
                }
            }
#endif
        }

        private void ScanTilemapTiles(Tilemap tilemap, HashSet<TileBase> seen)
        {
            if (tilemap == null) return;
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(pos);
                if (tile == null || !seen.Add(tile)) continue;
                _palette.Add(tile);
                _paletteNames.Add(tile.name);
            }
        }

        // ── GUI ────────────────────────────────────────────────────────────────

        private void OnGUI()
        {
            if (!_visible) return;

            _paletteRect = new Rect(12f, 12f, 300f, 520f);

            Color prevColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.6f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prevColor;

            GUILayout.BeginArea(_paletteRect, GUI.skin.box);

            GUILayout.Label("Map Paint (F2)");
            GUILayout.Label("PAUSED - EDIT MODE");
            GUILayout.Label("Hold-drag · Shift straight · RMB cancel/erase · Ctrl+Z undo", GUI.skin.label);
            GUILayout.BeginHorizontal();
            GUILayout.Label((string.IsNullOrEmpty(_activeRoomId) ? "No room" : _activeRoomId) + (_roomDirty ? " *" : string.Empty));
            if (GUILayout.Button("Save", GUILayout.Width(64f))) SaveActiveRoomData();
            GUILayout.EndHorizontal();

            DrawBrowserTabs();

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(_layer == PaintLayer.Floor, "Floor", GUI.skin.button)) SetLayer(PaintLayer.Floor);
            if (GUILayout.Toggle(_layer == PaintLayer.Cliff, "Cliff", GUI.skin.button)) SetLayer(PaintLayer.Cliff);
            if (GUILayout.Toggle(_layer == PaintLayer.Prop, "Prop", GUI.skin.button)) SetLayer(PaintLayer.Prop);
            GUILayout.EndHorizontal();

            _eraseMode = GUILayout.Toggle(_eraseMode, "Erase mode (LMB erases)");

            if (_layer != PaintLayer.Prop && ActiveTilemap() == null)
                GUILayout.Label($"<no {_layer} tilemap found>");

            if (GUILayout.Button("Refresh palette")) RebuildPalette();
            if (GUILayout.Button("Generate Cliff (from floor)")) GenerateCliffsInPlay();

            GUILayout.Space(4f);
            if (BrowserCount(_browserTab) > 0)
                GUILayout.Label(_browserTab + " browser:");
            else if (_layer == PaintLayer.Prop)
                GUILayout.Label(_propPalette.Count > 0 ? "Props:" : "<no wall/prop assets baked — Refresh after bake>");
            else
                GUILayout.Label(_palette.Count > 0 ? "Tiles:" : "<no tiles — Refresh after a room loads>");

            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Height(220f));
            if (BrowserCount(_browserTab) > 0)
            {
                DrawBrowserGrid();
            }
            else if (_layer == PaintLayer.Prop)
            {
                DrawPropGrid();
            }
            else
            {
                DrawTileGrid();
            }
            GUILayout.EndScrollView();

            GUILayout.Space(6f);
            DrawSelectedPreview();

            GUILayout.EndArea();
        }

        private void DrawBrowserTabs()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(_browserTab == AssetCat.Floor, "Floor", GUI.skin.button)) SetBrowserTab(AssetCat.Floor);
            if (GUILayout.Toggle(_browserTab == AssetCat.Wall, "Wall", GUI.skin.button)) SetBrowserTab(AssetCat.Wall);
            if (GUILayout.Toggle(_browserTab == AssetCat.Decor, "Decor", GUI.skin.button)) SetBrowserTab(AssetCat.Decor);
            GUILayout.EndHorizontal();
        }

        private void SetBrowserTab(AssetCat cat)
        {
            if (_browserTab == cat) return;
            _browserTab = cat;
            _scroll = Vector2.zero;
            _layer = cat == AssetCat.Floor ? PaintLayer.Floor : PaintLayer.Prop;
            SelectFirstBrowserEntry(cat);
        }

        private void SetLayer(PaintLayer layer)
        {
            if (_layer == layer) return;
            _layer = layer;
            _scroll = Vector2.zero;
            _browserSelected = -1;
        }

        private void SelectFirstBrowserEntry(AssetCat cat)
        {
            _browserSelected = -1;
            for (int i = 0; i < _browserEntries.Count; i++)
            {
                if (_browserEntries[i].cat == cat)
                {
                    _browserSelected = i;
                    return;
                }
            }
        }

        private int BrowserCount(AssetCat cat)
        {
            int count = 0;
            for (int i = 0; i < _browserEntries.Count; i++)
            {
                if (_browserEntries[i].cat == cat) count++;
            }

            return count;
        }

        private void DrawBrowserGrid()
        {
            int visibleIndex = 0;
            for (int i = 0; i < _browserEntries.Count; i++)
            {
                BrowserEntry entry = _browserEntries[i];
                if (entry.cat != _browserTab) continue;

                if (visibleIndex % 4 == 0) GUILayout.BeginHorizontal();

                bool isSel = i == _browserSelected;
                if (DrawBrowserThumb(entry, 58f, isSel))
                    _browserSelected = i;

                visibleIndex++;
                if (visibleIndex % 4 == 0) GUILayout.EndHorizontal();
            }

            if (visibleIndex % 4 != 0) GUILayout.EndHorizontal();
        }

        private void DrawTileGrid()
        {
            for (int i = 0; i < _palette.Count; i++)
            {
                if (i % 4 == 0) GUILayout.BeginHorizontal();

                bool isSel = i == _selected;
                Sprite sprite = TileSprite(_palette[i]);
                if (sprite != null)
                {
                    if (DrawSpriteThumb(sprite, 52f))
                    {
                        _selected = i;
                        _browserSelected = -1;
                    }
                    if (isSel) DrawSelectionOutline(GUILayoutUtility.GetLastRect());
                }
                else
                {
                    string label = i < _paletteNames.Count ? _paletteNames[i] : $"Tile {i + 1}";
                    bool nowSel = GUILayout.Toggle(isSel, label, GUI.skin.button, GUILayout.Width(52f), GUILayout.Height(52f));
                    if (nowSel && !isSel)
                    {
                        _selected = i;
                        _browserSelected = -1;
                    }
                    if (isSel) DrawSelectionOutline(GUILayoutUtility.GetLastRect());
                }

                if (i % 4 == 3 || i == _palette.Count - 1) GUILayout.EndHorizontal();
            }
        }

        private void DrawPropGrid()
        {
            for (int i = 0; i < _propPalette.Count; i++)
            {
                if (i % 4 == 0) GUILayout.BeginHorizontal();

                bool isSel = i == _propSelected;
                Sprite sprite = _propPalette[i].sprite;
                if (sprite != null)
                {
                    if (DrawSpriteThumb(sprite, 52f))
                    {
                        _propSelected = i;
                        _browserSelected = -1;
                    }
                    if (isSel) DrawSelectionOutline(GUILayoutUtility.GetLastRect());
                }
                else
                {
                    string label = string.IsNullOrEmpty(_propPalette[i].displayName) ? $"Prop {i + 1}" : _propPalette[i].displayName;
                    bool nowSel = GUILayout.Toggle(isSel, label, GUI.skin.button, GUILayout.Width(52f), GUILayout.Height(52f));
                    if (nowSel && !isSel)
                    {
                        _propSelected = i;
                        _browserSelected = -1;
                    }
                    if (isSel) DrawSelectionOutline(GUILayoutUtility.GetLastRect());
                }

                if (i % 4 == 3 || i == _propPalette.Count - 1) GUILayout.EndHorizontal();
            }
        }

        private void DrawSelectedPreview()
        {
            BrowserEntry entry;
            if (TrySelectedBrowserEntry(out entry))
            {
                DrawSpritePreview(entry.sprite, 96f);
                GUILayout.Label(entry.name);
                GUILayout.Label(SizeLabel(entry));
                if (entry.cat == AssetCat.Wall)
                    GUILayout.Label($"Footprint: {entry.footprint.x}x{entry.footprint.y}");
                return;
            }

            if (_layer == PaintLayer.Prop)
            {
                if (_propSelected < 0 || _propSelected >= _propPalette.Count) return;

                WallPiece piece = _propPalette[_propSelected];
                DrawSpritePreview(piece.sprite, 96f);
                string name = string.IsNullOrEmpty(piece.displayName) ? $"Prop {_propSelected + 1}" : piece.displayName;
                GUILayout.Label(name);
                GUILayout.Label($"Footprint: {piece.footprint.x}x{piece.footprint.y}");
                return;
            }

            if (_selected < 0 || _selected >= _palette.Count) return;

            DrawSpritePreview(TileSprite(_palette[_selected]), 96f);
            string tileName = _selected < _paletteNames.Count ? _paletteNames[_selected] : $"Tile {_selected + 1}";
            GUILayout.Label(tileName);
        }

        private static bool DrawBrowserThumb(BrowserEntry entry, float size, bool selected)
        {
            GUILayout.BeginVertical(GUILayout.Width(size));
            bool clicked = DrawSpriteThumb(entry.sprite, size);
            if (selected) DrawSelectionOutline(GUILayoutUtility.GetLastRect());
            GUIStyle labelStyle = GUI.skin.label;
            int prevSize = labelStyle.fontSize;
            TextAnchor prevAlign = labelStyle.alignment;
            bool prevWordWrap = labelStyle.wordWrap;
            labelStyle.fontSize = 8;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.wordWrap = true;
            GUILayout.Label(SizeLabel(entry), labelStyle, GUILayout.Width(size), GUILayout.Height(28f));
            labelStyle.fontSize = prevSize;
            labelStyle.alignment = prevAlign;
            labelStyle.wordWrap = prevWordWrap;
            GUILayout.EndVertical();
            return clicked;
        }

        private static string SizeLabel(BrowserEntry entry)
        {
            return string.Format("{0:0}x{1:0}px\n{2:0.##}x{3:0.##}u",
                entry.px.x,
                entry.px.y,
                entry.worldUnits.x,
                entry.worldUnits.y);
        }

        private static void DrawSpritePreview(Sprite sprite, float size)
        {
            if (sprite == null)
            {
                GUILayout.Box("<no sprite>", GUILayout.Width(size), GUILayout.Height(size));
                return;
            }

            GUILayout.Box(GUIContent.none, GUILayout.Width(size), GUILayout.Height(size));
            Rect rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint)
                GUI.DrawTextureWithTexCoords(rect, sprite.texture, SpriteUv(sprite), true);
        }

        private static bool DrawSpriteThumb(Sprite sp, float size)
        {
            bool clicked = GUILayout.Button(GUIContent.none, GUILayout.Width(size), GUILayout.Height(size));
            Rect rect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint)
            {
                if (sp != null)
                    GUI.DrawTextureWithTexCoords(rect, sp.texture, SpriteUv(sp), true);
            }

            return clicked;
        }

        private static void DrawSelectionOutline(Rect rect)
        {
            if (Event.current.type != EventType.Repaint) return;

            Color prev = GUI.color;
            GUI.color = Color.cyan;
            GUI.Box(rect, GUIContent.none);
            GUI.color = prev;
        }

        private static Rect SpriteUv(Sprite sp)
        {
            Texture2D texture = sp.texture;
            Rect r = sp.rect;
            return new Rect(
                r.x / texture.width,
                r.y / texture.height,
                r.width / texture.width,
                r.height / texture.height);
        }

        private static Sprite TileSprite(TileBase tile)
        {
            Tile concreteTile = tile as Tile;
            return concreteTile != null ? concreteTile.sprite : null;
        }

        private void ComposeRuinedKeep()
        {
            RuinedKeepComposer composer = FindFirstObjectByType<RuinedKeepComposer>();
            if (composer == null)
            {
                GameObject host = _grid != null ? _grid.gameObject : new GameObject("[RuinedKeepComposer]");
                composer = host.AddComponent<RuinedKeepComposer>();
            }

            composer.Compose();
        }

        // ── Logical cliff generation (parity with the Editor Map Designer) ──
        // Solves the cliff ring from the active room's floor shape (same RoomCliffSolver the
        // Editor uses), writes cliffCells into the shared RoomData, and mirrors them onto the
        // cliff Tilemap so the drop reads immediately in-Play.
        private void GenerateCliffsInPlay()
        {
            EnsureActiveRoomData();

            var placer = FindObjectOfType<RIMA.Environment.CliffAutoPlacer>();
            if (placer != null && placer.IsReady)
            {
                placer.Regenerate();
                if (placer.cliffTilemap != null) placer.cliffTilemap.RefreshAllTiles();
                _roomDirty = true;
                Debug.Log($"[InPlayMapPaint] Regenerated {placer.LastGeneratedCount} cliff cells via CliffAutoPlacer.");
                return;
            }

            if (_activeRoomData == null)
            {
                Debug.LogWarning("[InPlayMapPaint] No active room to generate cliffs for.");
                return;
            }

            HashSet<Vector3Int> cliffs = RoomCliffSolver.SolveFromRoom(_activeRoomData, 5);

            // Resolve a REAL cliff asset (cx review fix): prefer an existing cliff cell's id,
            // then a registry "cliff"-tagged entry; never the global selected floor palette item.
            string cliffAssetId = _activeRoomData.cliffCells.Count > 0
                ? _activeRoomData.cliffCells[0].assetGuidOrName
                : string.Empty;
            TileBase cliffTile = null;

            RuntimeAssetRegistry registry = RuntimeAssetRegistry.Instance;
            if (registry != null)
            {
                System.Collections.Generic.IReadOnlyList<RegistryEntry> cliffEntries = registry.GetByTag("cliff");
                RegistryEntry chosen = null;
                if (!string.IsNullOrEmpty(cliffAssetId))
                {
                    chosen = registry.Get(cliffAssetId);
                }
                if (chosen == null && cliffEntries.Count > 0)
                {
                    chosen = cliffEntries[0];
                    cliffAssetId = chosen.guid;
                }
                if (chosen != null) cliffTile = chosen.tile;
            }

            _activeRoomData.cliffCells.Clear();
            if (_cliffTilemap != null) _cliffTilemap.ClearAllTiles();

            foreach (Vector3Int c in cliffs)
            {
                Vector3 world = _grid != null ? _grid.GetCellCenterWorld(c) : Vector3.zero;
                RoomDataMutator.PutCliffCell(_activeRoomData, cliffAssetId, c, world, 0f, Vector2.one);
                if (_cliffTilemap != null && cliffTile != null) _cliffTilemap.SetTile(c, cliffTile);
            }

            _roomDirty = true;
            Debug.Log($"[InPlayMapPaint] Generated {cliffs.Count} cliff cells from floor shape.");
        }
    }
}

#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
