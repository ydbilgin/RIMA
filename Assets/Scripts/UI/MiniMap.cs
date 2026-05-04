using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace RIMA
{
    /// <summary>
    /// Simple dot-based mini-map.
    /// Shows room border + player (white) + enemies (red) as colored dots.
    /// Attach to the MiniMap root UI object; assign mapBackground RectTransform.
    ///
    /// TODO (tasarım güncellenince):
    ///   - Düz renkli dotlar → ikon/sprite (player silüeti, skull)
    ///   - Tek oda → multi-room harita (geçilen odaları göster)
    ///   - Kapı ikonları N/S/E/W kenarlarda
    ///   - Minimap toggle (M tuşu ile aç/kapat)
    /// </summary>
    public class MiniMap : MonoBehaviour
    {
        [Header("Room Bounds (override if RuntimeRoomManager absent)")]
        [SerializeField] private float roomWidth  = 20f;
        [SerializeField] private float roomHeight = 15f;

        [Header("Dot Sizes")]
        [SerializeField] private float playerDotSize = 8f;
        [SerializeField] private float enemyDotSize  = 5f;
        [SerializeField] private float fragmentDotSize = 6f;

        [Header("Colors")]
        [SerializeField] private Color playerColor = Color.white;
        [SerializeField] private Color enemyColor  = new Color(0.9f, 0.25f, 0.2f, 1f);
        [SerializeField] private Color fragmentColor = new Color(0.35f, 1f, 0.55f, 1f);

        [Header("Update Rate")]
        [SerializeField] private float updateInterval = 0.1f;

        // Runtime refs
        private RectTransform rootRect;
        private RectTransform mapRect;
        private RectTransform runRect;
        private RawImage roomMapImage;
        private Texture2D roomMapTexture;
        private Tilemap floorTilemap;
        private Tilemap wallTilemap;
        private LargeDungeonMapPainterBase roomPainter;
        private Transform playerTransform;
        private RectTransform playerDot;
        private Text roomLabel;
        private Text titleText;
        private bool expanded;

        private readonly List<(Transform enemy, RectTransform dot)> enemyDots
            = new List<(Transform, RectTransform)>();
        private readonly List<(MapFragment fragment, RectTransform dot)> fragmentDots
            = new List<(MapFragment, RectTransform)>();
        private readonly List<GameObject> exitMarkers = new List<GameObject>();
        private readonly List<GameObject> runMarkers = new List<GameObject>();

        private float nextUpdate;
        private int lastNodeId = -999;
        private int lastExitCount = -1;
        private int lastFloorTileCount = -1;
        private int lastWallTileCount = -1;
        private BoundsInt lastFloorBounds;

        private void Awake()
        {
            rootRect = GetComponent<RectTransform>();
            EnsureVisuals();
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;

            RefreshRoomBounds();

            playerDot = CreateDot("PlayerDot", playerColor, playerDotSize);
            RebuildExitMarkers();
            RebuildRunMarkers();
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
                SetExpanded(!expanded);

            if (Time.time < nextUpdate) return;
            nextUpdate = Time.time + updateInterval;

            RefreshRoomBounds();
            RefreshRoomMapTexture();
            UpdatePlayerDot();
            SyncEnemyDots();
            SyncFragmentDots();
            RefreshDynamicMarkers();
        }

        // ─── Dot updates ────────────────────────────────────────

        private void UpdatePlayerDot()
        {
            if (playerTransform == null || playerDot == null) return;
            playerDot.anchoredPosition = WorldToMap(playerTransform.position);
        }

        private void SyncEnemyDots()
        {
            // Remove dots for destroyed enemies
            for (int i = enemyDots.Count - 1; i >= 0; i--)
            {
                if (enemyDots[i].enemy == null)
                {
                    if (enemyDots[i].dot != null) Destroy(enemyDots[i].dot.gameObject);
                    enemyDots.RemoveAt(i);
                }
            }

            // Gather currently tracked transforms for fast lookup
            var tracked = new HashSet<Transform>();
            foreach (var pair in enemyDots) tracked.Add(pair.enemy);

            // Add dots for newly spawned enemies
            foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (!tracked.Contains(go.transform))
                    enemyDots.Add((go.transform, CreateDot("EnemyDot", enemyColor, enemyDotSize)));
            }

            // Update positions
            foreach (var (t, dot) in enemyDots)
            {
                if (t != null && dot != null)
                    dot.anchoredPosition = WorldToMap(t.position);
            }
        }

        private void SyncFragmentDots()
        {
            for (int i = fragmentDots.Count - 1; i >= 0; i--)
            {
                if (fragmentDots[i].fragment == null)
                {
                    if (fragmentDots[i].dot != null) Destroy(fragmentDots[i].dot.gameObject);
                    fragmentDots.RemoveAt(i);
                }
            }

            var tracked = new HashSet<MapFragment>();
            foreach (var pair in fragmentDots) tracked.Add(pair.fragment);

            foreach (var fragment in Object.FindObjectsByType<MapFragment>(FindObjectsSortMode.None))
            {
                if (!tracked.Contains(fragment))
                    fragmentDots.Add((fragment, CreateDot("MapFragmentDot", fragmentColor, fragmentDotSize)));
            }

            foreach (var (fragment, dot) in fragmentDots)
            {
                if (fragment != null && dot != null)
                    dot.anchoredPosition = WorldToMap(fragment.transform.position);
            }
        }

        // ─── Coordinate helpers ──────────────────────────────────

        private Vector2 WorldToMap(Vector3 world)
        {
            float w = mapRect.rect.width;
            float h = mapRect.rect.height;

            ResolveTilemaps();
            if (floorTilemap != null && TryGetMapBounds(out BoundsInt bounds))
            {
                if (bounds.size.x > 0 && bounds.size.y > 0)
                {
                    Vector3Int cell = floorTilemap.WorldToCell(world);
                    float nx = Mathf.InverseLerp(bounds.xMin, bounds.xMax, cell.x);
                    float ny = Mathf.InverseLerp(bounds.yMin, bounds.yMax, cell.y);
                    return new Vector2(nx * w - w * 0.5f, ny * h - h * 0.5f);
                }
            }

            // world origin (0,0) → map bottom-left corner;  pivot is center (0.5,0.5)
            float x = (world.x / roomWidth)  * w - w * 0.5f;
            float y = (world.y / roomHeight) * h - h * 0.5f;
            return new Vector2(x, y);
        }

        // ─── Dot factory ────────────────────────────────────────

        private RectTransform CreateDot(string dotName, Color color, float size)
        {
            var go  = new GameObject(dotName, typeof(RectTransform));
            go.transform.SetParent(mapRect, false);
            var rt  = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(size, size);
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot     = new Vector2(0.5f, 0.5f);
            var img = go.AddComponent<Image>();
            img.sprite = dotName == "PlayerDot"
                ? RimaGeneratedSpriteCache.Load("UI/RIMA/RIMA_UI_Node_Current")
                : null;
            img.color = img.sprite != null ? Color.white : color;
            img.preserveAspect = true;
            img.raycastTarget = false;
            return rt;
        }

        // ─── Runtime visuals ─────────────────────────────────────

        private void EnsureVisuals()
        {
            if (rootRect == null) return;

            Image bg = rootRect.GetComponent<Image>() ?? rootRect.gameObject.AddComponent<Image>();
            bg.sprite = RimaUITheme.MiniMapFrame;
            bg.color = RimaUITheme.PanelTint;
            bg.preserveAspect = false;
            bg.raycastTarget = false;

            titleText = EnsureText("Title", rootRect, "MAP");
            var titleRt = titleText.transform as RectTransform;
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0f, -8f);
            titleRt.sizeDelta = new Vector2(0f, 20f);
            titleText.fontSize = 13;
            titleText.fontStyle = FontStyle.Bold;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = new Color(0.72f, 0.88f, 0.92f, 1f);

            mapRect = EnsureRect("RoomArea", rootRect);
            mapRect.anchorMin = new Vector2(0f, 0f);
            mapRect.anchorMax = new Vector2(0f, 0f);
            mapRect.pivot = new Vector2(0f, 0f);
            mapRect.anchoredPosition = new Vector2(18f, 22f);
            mapRect.sizeDelta = new Vector2(132f, 112f);
            Image roomBg = mapRect.GetComponent<Image>() ?? mapRect.gameObject.AddComponent<Image>();
            roomBg.color = new Color(0.02f, 0.025f, 0.03f, 0.42f);
            roomBg.raycastTarget = false;
            var roomOutline = mapRect.GetComponent<Outline>() ?? mapRect.gameObject.AddComponent<Outline>();
            roomOutline.effectColor = new Color(0.34f, 0.55f, 0.62f, 0.75f);
            roomOutline.effectDistance = new Vector2(1f, -1f);

            roomMapImage = EnsureRect("RoomMapTexture", mapRect).gameObject.GetComponent<RawImage>();
            if (roomMapImage == null) roomMapImage = mapRect.Find("RoomMapTexture").gameObject.AddComponent<RawImage>();
            var roomMapRt = roomMapImage.transform as RectTransform;
            roomMapRt.anchorMin = Vector2.zero;
            roomMapRt.anchorMax = Vector2.one;
            roomMapRt.offsetMin = new Vector2(6f, 6f);
            roomMapRt.offsetMax = new Vector2(-6f, -6f);
            roomMapImage.color = new Color(1f, 1f, 1f, 0.92f);
            roomMapImage.raycastTarget = false;
            roomMapRt.SetAsFirstSibling();

            runRect = EnsureRect("RunArea", rootRect);
            runRect.anchorMin = new Vector2(1f, 0f);
            runRect.anchorMax = new Vector2(1f, 0f);
            runRect.pivot = new Vector2(1f, 0f);
            runRect.anchoredPosition = new Vector2(-18f, 22f);
            runRect.sizeDelta = new Vector2(58f, 112f);
            Image runBg = runRect.GetComponent<Image>() ?? runRect.gameObject.AddComponent<Image>();
            runBg.color = new Color(0.025f, 0.032f, 0.04f, 0.35f);
            runBg.raycastTarget = false;

            roomLabel = EnsureText("RoomLabel", rootRect, "");
            var labelRt = roomLabel.transform as RectTransform;
            labelRt.anchorMin = new Vector2(0f, 1f);
            labelRt.anchorMax = new Vector2(1f, 1f);
            labelRt.pivot = new Vector2(0.5f, 1f);
            labelRt.anchoredPosition = new Vector2(0f, -28f);
            labelRt.sizeDelta = new Vector2(0f, 18f);
            roomLabel.fontSize = 10;
            roomLabel.alignment = TextAnchor.MiddleCenter;
            roomLabel.color = new Color(0.58f, 0.68f, 0.72f, 0.92f);

            ApplyMapMode();
        }

        private void RefreshRoomBounds()
        {
            if (RuntimeRoomManager.Instance == null) return;
            roomWidth = Mathf.Max(1f, RuntimeRoomManager.Instance.RoomWidth);
            roomHeight = Mathf.Max(1f, RuntimeRoomManager.Instance.RoomHeight);
            if (roomLabel != null)
                roomLabel.text = $"ROOM {RuntimeRoomManager.Instance.CurrentRoom}";
        }

        private void ResolveTilemaps()
        {
            if (floorTilemap == null)
            {
                var floor = GameObject.Find("IsoGrid/Ground") ?? GameObject.Find("Room/Floor");
                if (floor != null) floorTilemap = floor.GetComponent<Tilemap>();
            }

            if (wallTilemap == null)
            {
                var wall = GameObject.Find("IsoGrid/Walls") ?? GameObject.Find("Room/Wall");
                if (wall != null) wallTilemap = wall.GetComponent<Tilemap>();
            }

            if (roomPainter == null)
                roomPainter = FindAnyObjectByType<LargeDungeonMapPainterBase>();
        }

        private void RefreshRoomMapTexture()
        {
            ResolveTilemaps();
            if (floorTilemap == null || roomMapImage == null) return;

            int floorCount = floorTilemap.GetUsedTilesCount();
            int wallCount = wallTilemap != null ? wallTilemap.GetUsedTilesCount() : 0;
            BoundsInt bounds = TryGetMapBounds(out BoundsInt playableBounds)
                ? playableBounds
                : floorTilemap.cellBounds;
            if (floorCount == lastFloorTileCount &&
                wallCount == lastWallTileCount &&
                bounds.Equals(lastFloorBounds))
                return;

            lastFloorTileCount = floorCount;
            lastWallTileCount = wallCount;
            lastFloorBounds = bounds;

            const int textureSize = 192;
            if (roomMapTexture == null)
            {
                roomMapTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
                roomMapTexture.filterMode = FilterMode.Point;
                roomMapTexture.wrapMode = TextureWrapMode.Clamp;
                roomMapImage.texture = roomMapTexture;
            }

            Color clear = new Color(0f, 0f, 0f, 0f);
            Color floor = new Color(0.24f, 0.32f, 0.36f, 0.72f);
            Color wall = new Color(0.58f, 0.78f, 0.86f, 0.90f);
            Color edge = new Color(0.08f, 0.14f, 0.16f, 0.80f);

            for (int x = 0; x < textureSize; x++)
                for (int y = 0; y < textureSize; y++)
                    roomMapTexture.SetPixel(x, y, clear);

            if (bounds.size.x <= 0 || bounds.size.y <= 0)
            {
                roomMapTexture.Apply(false);
                return;
            }

            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                bool hasFloor = IsMapFloorCell(cell);
                bool hasWall = wallTilemap != null && wallTilemap.GetTile(cell) != null;
                if (!hasFloor && !hasWall) continue;

                float nx = (cell.x - bounds.xMin) / Mathf.Max(1f, bounds.size.x - 1f);
                float ny = (cell.y - bounds.yMin) / Mathf.Max(1f, bounds.size.y - 1f);
                int px = Mathf.Clamp(Mathf.RoundToInt(nx * (textureSize - 1)), 0, textureSize - 1);
                int py = Mathf.Clamp(Mathf.RoundToInt(ny * (textureSize - 1)), 0, textureSize - 1);
                Color c = hasWall ? wall : floor;
                PaintMapPixel(roomMapTexture, px, py, c);

                if (hasFloor && TouchesEmpty(cell))
                    PaintMapPixel(roomMapTexture, px, py, edge);
            }

            roomMapTexture.Apply(false);
        }

        private bool TouchesEmpty(Vector3Int cell)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (!IsMapFloorCell(new Vector3Int(cell.x + dx, cell.y + dy, 0)))
                        return true;
                }
            }
            return false;
        }

        private bool TryGetMapBounds(out BoundsInt bounds)
        {
            if (roomPainter != null && roomPainter.TryGetPlayableCellBounds(out bounds))
                return true;

            bounds = default;
            return false;
        }

        private bool IsMapFloorCell(Vector3Int cell)
        {
            if (roomPainter != null)
                return roomPainter.IsPlayableCell(cell);

            return floorTilemap != null && floorTilemap.GetTile(cell) != null;
        }

        private static void PaintMapPixel(Texture2D texture, int x, int y, Color color)
        {
            for (int ox = -1; ox <= 1; ox++)
            {
                for (int oy = -1; oy <= 1; oy++)
                {
                    int px = x + ox;
                    int py = y + oy;
                    if (px < 0 || py < 0 || px >= texture.width || py >= texture.height) continue;
                    texture.SetPixel(px, py, color);
                }
            }
        }

        private void SetExpanded(bool value)
        {
            expanded = value;
            ApplyMapMode();
            RebuildExitMarkers();
            RebuildRunMarkers();
            if (rootRect != null) rootRect.SetAsLastSibling();
        }

        private void ApplyMapMode()
        {
            if (rootRect == null || mapRect == null || runRect == null) return;

            rootRect.sizeDelta = expanded ? new Vector2(520f, 330f) : new Vector2(244f, 176f);
            if (titleText != null) titleText.text = expanded ? "TACTICAL MAP" : "MAP";

            mapRect.anchoredPosition = expanded ? new Vector2(28f, 36f) : new Vector2(18f, 22f);
            mapRect.sizeDelta = expanded ? new Vector2(328f, 246f) : new Vector2(132f, 112f);

            runRect.anchoredPosition = expanded ? new Vector2(-28f, 36f) : new Vector2(-18f, 22f);
            runRect.sizeDelta = expanded ? new Vector2(120f, 246f) : new Vector2(58f, 112f);

            if (playerDot != null) playerDot.sizeDelta = Vector2.one * (expanded ? 14f : playerDotSize);
            foreach (var pair in enemyDots)
                if (pair.dot != null) pair.dot.sizeDelta = Vector2.one * (expanded ? 9f : enemyDotSize);
            foreach (var pair in fragmentDots)
                if (pair.dot != null) pair.dot.sizeDelta = Vector2.one * (expanded ? 10f : fragmentDotSize);
        }

        private void RefreshDynamicMarkers()
        {
            int nodeId = DungeonGraph.Instance != null ? DungeonGraph.Instance.CurrentNodeId : -1;
            int exits = DungeonGraph.Instance != null ? DungeonGraph.Instance.CurrentNode.exits.Count : 0;
            if (nodeId == lastNodeId && exits == lastExitCount) return;

            RebuildExitMarkers();
            RebuildRunMarkers();
            lastNodeId = nodeId;
            lastExitCount = exits;
        }

        private void RebuildExitMarkers()
        {
            foreach (GameObject marker in exitMarkers)
                if (marker != null) Destroy(marker);
            exitMarkers.Clear();

            if (DungeonGraph.Instance == null || mapRect == null) return;
            foreach (var kvp in DungeonGraph.Instance.GetCurrentExits())
                exitMarkers.Add(CreateExitMarker(kvp.Key, kvp.Value));
        }

        private GameObject CreateExitMarker(DoorDirection direction, RoomType targetType)
        {
            var go = new GameObject("Exit_" + direction, typeof(RectTransform));
            go.transform.SetParent(mapRect, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);

            rt.anchoredPosition = TryGetExitMapPosition(direction, out Vector2 exitPosition)
                ? exitPosition
                : GetFallbackExitMapPosition(direction);
            rt.sizeDelta = Vector2.one * (expanded ? 28f : 22f);

            var gatePlate = new GameObject("GatePlate", typeof(RectTransform));
            gatePlate.transform.SetParent(go.transform, false);
            var plateRt = gatePlate.GetComponent<RectTransform>();
            plateRt.anchorMin = new Vector2(0.5f, 0.5f);
            plateRt.anchorMax = new Vector2(0.5f, 0.5f);
            plateRt.pivot = new Vector2(0.5f, 0.5f);
            plateRt.anchoredPosition = Vector2.zero;
            plateRt.sizeDelta = direction == DoorDirection.East || direction == DoorDirection.West
                ? new Vector2(7f, expanded ? 42f : 30f)
                : new Vector2(expanded ? 42f : 30f, 7f);
            var plateImage = gatePlate.AddComponent<Image>();
            plateImage.color = new Color(0.62f, 0.88f, 0.96f, 0.92f);
            plateImage.raycastTarget = false;

            var icon = new GameObject("Icon", typeof(RectTransform));
            icon.transform.SetParent(go.transform, false);
            var iconRt = icon.GetComponent<RectTransform>();
            iconRt.anchorMin = new Vector2(0.5f, 0.5f);
            iconRt.anchorMax = new Vector2(0.5f, 0.5f);
            iconRt.pivot = new Vector2(0.5f, 0.5f);
            iconRt.anchoredPosition = Vector2.zero;
            iconRt.sizeDelta = rt.sizeDelta;

            var img = icon.AddComponent<Image>();
            img.sprite = RimaUITheme.NodeIcon(targetType);
            img.color = GetRoomColor(targetType);
            img.preserveAspect = true;
            img.raycastTarget = false;

            var buttonImage = go.AddComponent<Image>();
            buttonImage.color = new Color(1f, 1f, 1f, 0.01f);
            buttonImage.raycastTarget = true;

            var btn = go.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.70f, 0.95f, 1f, 1f);
            colors.pressedColor = new Color(0.40f, 0.82f, 1f, 1f);
            btn.colors = colors;
            return go;
        }

        private Vector2 GetFallbackExitMapPosition(DoorDirection direction)
        {
            float halfW = mapRect.rect.width * 0.5f;
            float halfH = mapRect.rect.height * 0.5f;
            return direction switch
            {
                DoorDirection.East => new Vector2(halfW - 9f, 0f),
                DoorDirection.West => new Vector2(-halfW + 9f, 0f),
                DoorDirection.South => new Vector2(0f, -halfH + 9f),
                _ => new Vector2(0f, halfH - 9f),
            };
        }

        private bool TryGetExitMapPosition(DoorDirection direction, out Vector2 position)
        {
            position = default;
            if (floorTilemap == null || !TryGetMapBounds(out BoundsInt bounds)) return false;

            Vector3Int best = default;
            bool found = false;
            int cx = bounds.xMin + bounds.size.x / 2;
            int cy = bounds.yMin + bounds.size.y / 2;
            float bestScore = float.MinValue;

            foreach (Vector3Int cell in bounds.allPositionsWithin)
            {
                if (!IsMapFloorCell(cell)) continue;

                float score = direction switch
                {
                    DoorDirection.East => (cell.x - bounds.xMin) * 1000f - Mathf.Abs(cell.y - cy),
                    DoorDirection.West => (bounds.xMax - cell.x) * 1000f - Mathf.Abs(cell.y - cy),
                    DoorDirection.South => (bounds.yMax - cell.y) * 1000f - Mathf.Abs(cell.x - cx),
                    _ => (cell.y - bounds.yMin) * 1000f - Mathf.Abs(cell.x - cx),
                };

                if (!found || score > bestScore)
                {
                    found = true;
                    bestScore = score;
                    best = cell;
                }
            }

            if (!found) return false;
            position = WorldToMap(floorTilemap.GetCellCenterWorld(best));
            return true;
        }

        private void RebuildRunMarkers()
        {
            foreach (GameObject marker in runMarkers)
                if (marker != null) Destroy(marker);
            runMarkers.Clear();

            if (DungeonGraph.Instance == null || runRect == null) return;

            RoomNode current = DungeonGraph.Instance.CurrentNode;
            Vector2 currentPos = new Vector2(0f, -46f);
            runMarkers.Add(CreateRunNode("Current", currentPos, Color.white, "", null, isCurrent: true));

            int count = Mathf.Max(1, current.exits.Count);
            int index = 0;
            foreach (var kvp in current.exits)
            {
                RoomNode target = DungeonGraph.Instance.AllNodes[kvp.Value];
                float x = count == 1 ? 0f : Mathf.Lerp(-24f, 24f, index / (float)(count - 1));
                Vector2 targetPos = new Vector2(x, 42f);
                runMarkers.Add(CreateRunLine(currentPos, targetPos));
                runMarkers.Add(CreateRunNode("Next_" + kvp.Key, targetPos, GetRoomColor(target.roomType), GetRoomSymbol(target.roomType), target.roomType, isCurrent: false));
                index++;
            }
        }

        private GameObject CreateRunLine(Vector2 from, Vector2 to)
        {
            var go = new GameObject("RunLine", typeof(RectTransform));
            go.transform.SetParent(runRect, false);
            go.transform.SetAsFirstSibling();
            var rt = go.GetComponent<RectTransform>();
            var img = go.AddComponent<Image>();
            img.color = new Color(0.48f, 0.62f, 0.66f, 0.45f);
            img.raycastTarget = false;

            Vector2 dir = to - from;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = (from + to) * 0.5f;
            rt.sizeDelta = new Vector2(dir.magnitude, 2f);
            rt.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            return go;
        }

        private GameObject CreateRunNode(string name, Vector2 position, Color color, string symbol, RoomType? roomType, bool isCurrent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(runRect, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = position;
            rt.sizeDelta = new Vector2(30f, 30f);
            var img = go.AddComponent<Image>();
            img.sprite = isCurrent ? RimaGeneratedSpriteCache.Load("UI/RIMA/RIMA_UI_Node_Current") : (roomType.HasValue ? RimaUITheme.NodeIcon(roomType.Value) : null);
            img.color = img.sprite != null ? Color.white : color;
            img.preserveAspect = true;
            img.raycastTarget = false;

            if (img.sprite == null)
            {
                var text = EnsureText("Symbol", rt, symbol);
                var textRt = text.transform as RectTransform;
                textRt.anchorMin = Vector2.zero;
                textRt.anchorMax = Vector2.one;
                textRt.offsetMin = Vector2.zero;
                textRt.offsetMax = Vector2.zero;
                text.fontSize = 11;
                text.fontStyle = FontStyle.Bold;
                text.alignment = TextAnchor.MiddleCenter;
                text.color = Color.black;
            }
            return go;
        }

        private static RectTransform EnsureRect(string name, Transform parent)
        {
            Transform found = parent.Find(name);
            if (found != null) return found as RectTransform;

            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.GetComponent<RectTransform>();
        }

        private static Text EnsureText(string name, Transform parent, string value)
        {
            RectTransform rt = EnsureRect(name, parent);
            Text text = rt.GetComponent<Text>() ?? rt.gameObject.AddComponent<Text>();
            text.text = value;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.raycastTarget = false;
            return text;
        }

        private Color GetRoomColor(RoomType type) => type switch
        {
            RoomType.Combat => new Color(0.58f, 0.63f, 0.66f, 1f),
            RoomType.Elite => new Color(0.95f, 0.48f, 0.16f, 1f),
            RoomType.Boss => new Color(0.92f, 0.16f, 0.18f, 1f),
            RoomType.Chest => new Color(0.95f, 0.74f, 0.24f, 1f),
            RoomType.Merchant => new Color(0.28f, 0.74f, 0.96f, 1f),
            RoomType.Event => new Color(0.64f, 0.42f, 0.92f, 1f),
            RoomType.Forge => new Color(0.82f, 0.54f, 0.25f, 1f),
            _ => new Color(0.58f, 0.63f, 0.66f, 1f),
        };

        private static string GetRoomSymbol(RoomType type) => type switch
        {
            RoomType.Combat => "C",
            RoomType.Elite => "E",
            RoomType.Boss => "B",
            RoomType.Chest => "$",
            RoomType.Merchant => "M",
            RoomType.Event => "?",
            RoomType.Forge => "F",
            _ => ".",
        };
    }
}
