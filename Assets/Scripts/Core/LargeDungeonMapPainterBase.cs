using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

namespace RIMA
{
    [DefaultExecutionOrder(-200)]
    public class LargeDungeonMapPainterBase : MonoBehaviour
    {
        public enum LayoutKind
        {
            BrokenEntryHall,
            ChainGallery,
            ShrineCrossroad,
            CryptBasin,
            PillarArena,
            SplitVault,
            RitualHall,
            CollapsedLibrary,
            NarrowApproach,
            CrescentSanctum,
            BrokenCauseway,
            ReliquaryLoop,
            ForkedOssuary,
            AmbushCloister,
            RiftWell,
            BossAntechamber
        }

        private static readonly LayoutKind[] ThresholdLayouts =
        {
            LayoutKind.BrokenEntryHall,
            LayoutKind.ChainGallery,
            LayoutKind.NarrowApproach,
        };

        private static readonly LayoutKind[] OssuaryLayouts =
        {
            LayoutKind.CryptBasin,
            LayoutKind.ForkedOssuary,
            LayoutKind.ReliquaryLoop,
        };

        private static readonly LayoutKind[] SanctumLayouts =
        {
            LayoutKind.ShrineCrossroad,
            LayoutKind.RitualHall,
            LayoutKind.CrescentSanctum,
        };

        private static readonly LayoutKind[] RiftLayouts =
        {
            LayoutKind.SplitVault,
            LayoutKind.AmbushCloister,
            LayoutKind.RiftWell,
            LayoutKind.PillarArena,
        };

        private static readonly LayoutKind[] PreviewLayouts =
        {
            LayoutKind.BrokenEntryHall,
            LayoutKind.PillarArena,
            LayoutKind.CollapsedLibrary,
            LayoutKind.BrokenCauseway,
            LayoutKind.AmbushCloister,
            LayoutKind.CryptBasin,
            LayoutKind.ReliquaryLoop,
            LayoutKind.ShrineCrossroad,
            LayoutKind.RiftWell,
            LayoutKind.BossAntechamber,
        };

        private static readonly string[] PreviewLayoutNames =
        {
            "R01 Broken Entry Gate",
            "R02 Ordered Guard Hall",
            "R03 Cell Spine",
            "R04 Broken Causeway",
            "R05 Cross-Chain Clamp",
            "R06 Sunken Crypt Basin",
            "R07 Reliquary Loop",
            "R08 Shrine Crossroad",
            "R09 Rift Well Edge Tear",
            "R10 Containment Arena",
        };

        private readonly struct RoomLightSpec
        {
            public readonly Vector2 normalizedPosition;
            public readonly Color color;
            public readonly float intensity;
            public readonly float innerRadius;
            public readonly float outerRadius;
            public readonly bool flicker;

            public RoomLightSpec(Vector2 normalizedPosition, Color color, float intensity, float innerRadius, float outerRadius, bool flicker)
            {
                this.normalizedPosition = normalizedPosition;
                this.color = color;
                this.intensity = intensity;
                this.innerRadius = innerRadius;
                this.outerRadius = outerRadius;
                this.flicker = flicker;
            }
        }

        private readonly struct DecorSpec
        {
            public readonly string spritePath;
            public readonly Vector2 normalizedPosition;
            public readonly float scale;
            public readonly int sortingOrder;

            public DecorSpec(string spritePath, Vector2 normalizedPosition, float scale, int sortingOrder)
            {
                this.spritePath = spritePath;
                this.normalizedPosition = normalizedPosition;
                this.scale = scale;
                this.sortingOrder = sortingOrder;
            }
        }

        [Header("Tilemaps")]
        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private Tilemap wallTilemap;

        [Header("Size")]
        [SerializeField] private int defaultWidth = 220;
        [SerializeField] private int defaultHeight = 150;
        [SerializeField] private int wallThickness = 3;

        [Header("Tiles")]
        [SerializeField] private TileBase[] floorTiles;
        [SerializeField] private TileBase[] wallTiles;

        [Header("Layout")]
        [SerializeField] private bool paintOnAwake = false;
        [SerializeField] private int seed = 4303;
        [SerializeField, Range(0, 16)] private int cameraSafetyFloorPadding = 0;

        [Header("Lighting")]
        [SerializeField] private bool createProceduralLights = true;
        [SerializeField, Range(0.05f, 1f)] private float globalLightIntensity = 0.34f;
        [SerializeField] private Color globalLightColor = new Color(0.18f, 0.23f, 0.29f, 1f);
        [SerializeField, Range(0f, 2f)] private float localLightAccentScale = 0.82f;
        [SerializeField] private string proceduralLightRootName = "Procedural Room Lights";
        [SerializeField] private string proceduralDecorRootName = "Procedural Room Decor";

        private readonly List<TileBase> cachedFloorTiles = new List<TileBase>();
        private readonly List<TileBase> cachedWallTiles = new List<TileBase>();
        private bool[,] lastPlayableFloorMask;
        private int roomWidth;
        private int roomHeight;
        private Vector3Int paintCellOffset = Vector3Int.zero;

        public int RoomWidth => roomWidth;
        public int RoomHeight => roomHeight;
        public int PreviewLayoutCount => PreviewLayouts.Length;
        public static int DefaultPreviewLayoutCount => PreviewLayouts.Length;

        public string GetPreviewLayoutName(int index) => GetDefaultPreviewLayoutName(index);

        public Vector3 GetNearestPlayableFloorPosition(Vector3 preferred, Vector3 fallback, int searchRadius)
        {
            if (floorTilemap == null || lastPlayableFloorMask == null)
                return fallback;

            Vector3Int origin = floorTilemap.WorldToCell(preferred);
            for (int r = 0; r <= searchRadius; r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        if (r > 0 && Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;
                        var tile = new Vector3Int(origin.x + dx, origin.y + dy, 0);
                        if (!IsPlayableCell(tile.x, tile.y)) continue;
                        if (wallTilemap != null && wallTilemap.GetTile(tile) != null) continue;
                        return floorTilemap.GetCellCenterWorld(tile);
                    }
                }
            }

            return GetPlayableCenterFallback(fallback);
        }

        public bool IsPlayableWorldPosition(Vector3 world)
        {
            if (floorTilemap == null || lastPlayableFloorMask == null) return false;
            Vector3Int cell = floorTilemap.WorldToCell(world);
            return IsPlayableCell(cell.x, cell.y) &&
                   (wallTilemap == null || wallTilemap.GetTile(cell) == null);
        }

        public bool IsPlayableCell(Vector3Int cell)
        {
            return IsPlayableCell(cell.x, cell.y) &&
                   (wallTilemap == null || wallTilemap.GetTile(cell) == null);
        }

        public bool TryGetPlayableCellBounds(out BoundsInt bounds)
        {
            bounds = default;
            if (lastPlayableFloorMask == null) return false;

            int minX = roomWidth;
            int minY = roomHeight;
            int maxX = -1;
            int maxY = -1;

            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (!lastPlayableFloorMask[x, y]) continue;
                    minX = Mathf.Min(minX, x);
                    minY = Mathf.Min(minY, y);
                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);
                }
            }

            if (maxX < minX || maxY < minY) return false;
            bounds = new BoundsInt(minX, minY, 0, maxX - minX + 1, maxY - minY + 1, 1);
            return true;
        }

        private Vector3 GetPlayableCenterFallback(Vector3 fallback)
        {
            if (floorTilemap == null || lastPlayableFloorMask == null) return fallback;

            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            for (int r = 0; r <= Mathf.Max(roomWidth, roomHeight); r++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    for (int dy = -r; dy <= r; dy++)
                    {
                        if (r > 0 && Mathf.Abs(dx) != r && Mathf.Abs(dy) != r) continue;
                        int x = cx + dx;
                        int y = cy + dy;
                        if (!IsPlayableCell(x, y)) continue;
                        var tile = new Vector3Int(x, y, 0);
                        if (wallTilemap != null && wallTilemap.GetTile(tile) != null) continue;
                        return floorTilemap.GetCellCenterWorld(tile);
                    }
                }
            }

            return fallback;
        }

        private bool IsPlayableCell(int x, int y)
        {
            return lastPlayableFloorMask != null &&
                   x >= 0 && y >= 0 &&
                   x < roomWidth && y < roomHeight &&
                   lastPlayableFloorMask[x, y];
        }

        public static string GetDefaultPreviewLayoutName(int index)
        {
            if (PreviewLayoutNames.Length == 0) return "Room Preview";
            int clamped = Mathf.Clamp(index, 0, PreviewLayoutNames.Length - 1);
            return PreviewLayoutNames[clamped];
        }

        private void Awake()
        {
            ResolveTilemaps();
            CacheTiles();
            if (paintOnAwake)
            {
                PaintForRoom(1, RoomType.Combat);
            }
        }

        public void PaintForRoom(int roomIndex, RoomType roomType)
        {
            ResolveTilemaps();
            CacheTiles();
            if (floorTilemap == null || wallTilemap == null || cachedFloorTiles.Count == 0 || cachedWallTiles.Count == 0)
            {
                Debug.LogWarning("[LargeDungeonMapPainter] Missing tilemaps or tiles; keeping existing room.");
                return;
            }

            LayoutKind layout = SelectLayout(roomIndex, roomType);
            Vector2Int size = GetLayoutSize(layout, roomType);
            roomWidth = size.x;
            roomHeight = size.y;

            Random.InitState(seed + roomIndex * 97 + (int)roomType * 13);
            PaintPreparedLayout(layout, Vector3Int.zero, clearTilemaps: true, paintRoomObjects: true);
        }

        public void PaintTemplateAtOffset(LayoutKind layout, Vector3Int offset)
        {
            ResolveTilemaps();
            CacheTiles();
            if (floorTilemap == null || wallTilemap == null || cachedFloorTiles.Count == 0 || cachedWallTiles.Count == 0)
            {
                Debug.LogWarning("[LargeDungeonMapPainter] Missing tilemaps or tiles; keeping existing room.");
                return;
            }

            RoomType roomType = layout == LayoutKind.BossAntechamber ? RoomType.Boss : RoomType.Combat;
            Vector2Int size = GetLayoutSize(layout, roomType);
            roomWidth = size.x;
            roomHeight = size.y;

            Random.InitState(seed + (int)layout * 97);
            PaintPreparedLayout(layout, offset, clearTilemaps: false, paintRoomObjects: false);
        }

        public void PaintPreviewLayout(int index)
        {
            int clamped = Mathf.Clamp(index, 0, PreviewLayouts.Length - 1);
            RoomType previewType = PreviewLayouts[clamped] == LayoutKind.BossAntechamber
                ? RoomType.Boss
                : RoomType.Event;
            PaintLayout(PreviewLayouts[clamped], clamped + 1, previewType);
        }

        private void PaintLayout(LayoutKind layout, int roomIndex, RoomType roomType)
        {
            ResolveTilemaps();
            CacheTiles();
            if (floorTilemap == null || wallTilemap == null || cachedFloorTiles.Count == 0 || cachedWallTiles.Count == 0)
            {
                Debug.LogWarning("[LargeDungeonMapPainter] Missing tilemaps or tiles; keeping existing room.");
                return;
            }

            Vector2Int size = GetLayoutSize(layout, roomType);
            roomWidth = size.x;
            roomHeight = size.y;

            Random.InitState(seed + roomIndex * 97 + (int)roomType * 13);
            PaintPreparedLayout(layout, Vector3Int.zero, clearTilemaps: true, paintRoomObjects: true);
        }

        private void PaintPreparedLayout(LayoutKind layout, Vector3Int offset, bool clearTilemaps, bool paintRoomObjects)
        {
            if (clearTilemaps)
            {
                floorTilemap.ClearAllTiles();
                wallTilemap.ClearAllTiles();
            }

            Vector3Int previousOffset = paintCellOffset;
            paintCellOffset = offset;

            try
            {
                bool[,] floorMask = BuildFloorMask(layout);
                lastPlayableFloorMask = floorMask;
                bool[,] visualFloorMask = BuildVisualFloorMask(floorMask);

                PaintFloor(visualFloorMask);
                PaintCameraSafetyFloor();
                PaintBoundaryWalls(floorMask);
                PaintShelterCellPartitions(floorMask);
                PaintLayoutFeatures(layout, floorMask);
                PaintCombatViewKeepAnchors(floorMask);

                if (paintRoomObjects)
                {
                    PaintRoomLighting(layout, floorMask);
                    PaintNarrativeDecor(layout, floorMask);
                }
            }
            finally
            {
                paintCellOffset = previousOffset;
            }

            floorTilemap.CompressBounds();
            wallTilemap.CompressBounds();
        }

        private void ResolveTilemaps()
        {
            if (floorTilemap == null)
            {
                var go = GameObject.Find("IsoGrid/Ground") ?? GameObject.Find("Room/Floor");
                if (go != null) floorTilemap = go.GetComponent<Tilemap>();
            }

            if (wallTilemap == null)
            {
                var go = GameObject.Find("IsoGrid/Walls") ?? GameObject.Find("Room/Wall");
                if (go != null) wallTilemap = go.GetComponent<Tilemap>();
            }
        }

        private void CacheTiles()
        {
            if (cachedFloorTiles.Count == 0)
            {
                AddTiles(floorTiles, cachedFloorTiles);
                AddUsedTiles(floorTilemap, cachedFloorTiles);
            }

            if (cachedWallTiles.Count == 0)
            {
                AddTiles(wallTiles, cachedWallTiles);
                AddUsedTiles(wallTilemap, cachedWallTiles);
            }
        }

        private static void AddTiles(TileBase[] source, List<TileBase> target)
        {
            if (source == null) return;
            foreach (TileBase tile in source)
            {
                if (tile != null && !target.Contains(tile)) target.Add(tile);
            }
        }

        private static void AddUsedTiles(Tilemap source, List<TileBase> target)
        {
            if (source == null) return;

            int count = source.GetUsedTilesCount();
            if (count <= 0) return;

            var used = new TileBase[count];
            source.GetUsedTilesNonAlloc(used);
            foreach (TileBase tile in used)
            {
                if (tile != null && !target.Contains(tile)) target.Add(tile);
            }
        }

        private LayoutKind SelectLayout(int roomIndex, RoomType roomType)
        {
            if (roomType == RoomType.Boss) return LayoutKind.BossAntechamber;
            if (roomType == RoomType.Chest) return LayoutKind.ReliquaryLoop;
            if (roomType == RoomType.Merchant) return LayoutKind.BrokenEntryHall;
            if (roomType == RoomType.Forge) return LayoutKind.BrokenCauseway;
            if (roomType == RoomType.Event) return (roomIndex % 2 == 0) ? LayoutKind.CrescentSanctum : LayoutKind.RiftWell;
            if (roomType == RoomType.Elite) return (roomIndex % 2 == 0) ? LayoutKind.AmbushCloister : LayoutKind.ForkedOssuary;

            if (roomIndex <= 3)
                return ThresholdLayouts[Mathf.Abs(roomIndex - 1) % ThresholdLayouts.Length];
            if (roomIndex <= 6)
                return OssuaryLayouts[Mathf.Abs(roomIndex - 4) % OssuaryLayouts.Length];
            if (roomIndex <= 9)
                return SanctumLayouts[Mathf.Abs(roomIndex - 7) % SanctumLayouts.Length];

            return RiftLayouts[Mathf.Abs(roomIndex - 10) % RiftLayouts.Length];
        }

        private string GetNarrativeBand(LayoutKind layout)
        {
            return layout switch
            {
                LayoutKind.BrokenEntryHall or LayoutKind.ChainGallery or LayoutKind.NarrowApproach => "threshold",
                LayoutKind.CryptBasin or LayoutKind.ForkedOssuary or LayoutKind.ReliquaryLoop or LayoutKind.CollapsedLibrary => "ossuary",
                LayoutKind.ShrineCrossroad or LayoutKind.RitualHall or LayoutKind.CrescentSanctum => "sanctum",
                LayoutKind.SplitVault or LayoutKind.AmbushCloister or LayoutKind.RiftWell or LayoutKind.PillarArena => "rift",
                LayoutKind.BossAntechamber => "boss",
                _ => "threshold",
            };
        }

        private Vector2Int GetLayoutSize(LayoutKind layout, RoomType roomType)
        {
            if (roomType == RoomType.Boss) return new Vector2Int(132, 86);

            return layout switch
            {
                LayoutKind.ChainGallery => new Vector2Int(164, 86),
                LayoutKind.ShrineCrossroad => new Vector2Int(150, 104),
                LayoutKind.CryptBasin => new Vector2Int(144, 104),
                LayoutKind.PillarArena => new Vector2Int(152, 100),
                LayoutKind.SplitVault => new Vector2Int(156, 98),
                LayoutKind.RitualHall => new Vector2Int(148, 96),
                LayoutKind.CollapsedLibrary => new Vector2Int(158, 96),
                LayoutKind.NarrowApproach => new Vector2Int(166, 88),
                LayoutKind.CrescentSanctum => new Vector2Int(150, 102),
                LayoutKind.BrokenCauseway => new Vector2Int(170, 90),
                LayoutKind.ReliquaryLoop => new Vector2Int(154, 102),
                LayoutKind.ForkedOssuary => new Vector2Int(162, 98),
                LayoutKind.AmbushCloister => new Vector2Int(156, 104),
                LayoutKind.RiftWell => new Vector2Int(150, 106),
                _ => new Vector2Int(148, 98),
            };
        }

        private bool[,] BuildFloorMask(LayoutKind layout)
        {
            bool[,] mask = new bool[roomWidth, roomHeight];
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            // Door socket coordinates -- entrance (south) and exit slots (north)
            int entranceX = cx - 5;
            int entranceWidth = 10;
            int exitAreaX = cx - 10;
            int exitAreaWidth = 20;

            AddRect(mask, entranceX, 0, entranceWidth, 20);
            AddRect(mask, exitAreaX, roomHeight - 20, exitAreaWidth, 20);

            switch (layout)
            {
                case LayoutKind.BrokenEntryHall:
                    AddRect(mask, 20, cy - 20, roomWidth - 40, 40);
                    AddRect(mask, 28, cy + 20, 30, 16);
                    AddRect(mask, roomWidth - 58, cy + 20, 30, 16);
                    RemoveRect(mask, cx - 28, cy - 10, 2, 2);
                    RemoveRect(mask, cx + 26, cy - 10, 2, 2);
                    RemoveRect(mask, cx - 28, cy + 8, 2, 2);
                    RemoveRect(mask, cx + 26, cy + 8, 2, 2);
                    break;
                case LayoutKind.ChainGallery:
                    AddRect(mask, 14, cy - 16, roomWidth - 28, 32);
                    AddRect(mask, 22, cy - 34, 34, 18);
                    AddRect(mask, roomWidth - 56, cy + 16, 34, 18);
                    break;
                case LayoutKind.ShrineCrossroad:
                    AddRect(mask, cx - 24, cy - 20, 48, 40);
                    AddRect(mask, cx - 10, 12, 20, roomHeight - 24);
                    AddRect(mask, 18, cy - 10, roomWidth - 36, 20);
                    AddRect(mask, cx - 18, roomHeight - 40, 36, 20);
                    AddRect(mask, cx - 18, 20, 36, 20);
                    AddRect(mask, 32, cy - 18, 28, 36);
                    AddRect(mask, roomWidth - 60, cy - 18, 28, 36);
                    AddEllipse(mask, cx, cy, 13, 10);
                    break;
                case LayoutKind.CryptBasin:
                    AddRect(mask, 24, cy - 28, roomWidth - 48, 56);
                    AddRect(mask, 18, cy + 28, 24, 18);
                    AddRect(mask, roomWidth - 42, cy + 28, 24, 18);
                    AddRect(mask, 18, cy - 46, 24, 18);
                    AddRect(mask, roomWidth - 42, cy - 46, 24, 18);
                    RemoveRect(mask, cx - 16, cy - 10, 32, 20);
                    break;
                case LayoutKind.PillarArena:
                    AddRect(mask, 20, cy - 32, roomWidth - 40, 64);
                    RemoveRect(mask, cx - 36, cy - 16, 2, 2);
                    RemoveRect(mask, cx, cy - 16, 2, 2);
                    RemoveRect(mask, cx + 34, cy - 16, 2, 2);
                    RemoveRect(mask, cx - 36, cy + 14, 2, 2);
                    RemoveRect(mask, cx, cy + 14, 2, 2);
                    RemoveRect(mask, cx + 34, cy + 14, 2, 2);
                    break;
                case LayoutKind.SplitVault:
                    AddRect(mask, 20, cy - 26, 50, 52);
                    AddRect(mask, roomWidth - 70, cy - 26, 50, 52);
                    AddRect(mask, 70, cy - 7, roomWidth - 140, 14);
                    break;
                case LayoutKind.RitualHall:
                    AddRect(mask, 22, cy - 30, roomWidth - 44, 60);
                    AddEllipse(mask, 30, cy, 14, 20);
                    AddEllipse(mask, roomWidth - 30, cy, 14, 20);
                    RemoveRect(mask, cx - 36, cy - 16, 2, 2);
                    RemoveRect(mask, cx - 12, cy - 16, 2, 2);
                    RemoveRect(mask, cx + 12, cy - 16, 2, 2);
                    RemoveRect(mask, cx + 36, cy - 16, 2, 2);
                    RemoveRect(mask, cx - 36, cy + 14, 2, 2);
                    RemoveRect(mask, cx - 12, cy + 14, 2, 2);
                    RemoveRect(mask, cx + 12, cy + 14, 2, 2);
                    RemoveRect(mask, cx + 36, cy + 14, 2, 2);
                    break;
                case LayoutKind.CollapsedLibrary:
                    AddRect(mask, 20, cy - 30, roomWidth - 40, 60);
                    RemoveRect(mask, 20, cy + 18, 18, 12);
                    RemoveRect(mask, roomWidth - 38, cy + 14, 18, 16);
                    RemoveRect(mask, 20, cy - 30, 24, 14);
                    RemoveRect(mask, roomWidth - 44, cy - 30, 24, 18);
                    break;
                case LayoutKind.NarrowApproach:
                    AddRect(mask, cx - 8, 8, 16, roomHeight - 36);
                    AddRect(mask, cx - 34, roomHeight - 48, 68, 28);
                    break;
                case LayoutKind.CrescentSanctum:
                    AddRect(mask, 28, cy - 26, roomWidth - 56, 52);
                    AddEllipse(mask, cx, roomHeight - 36, 28, 18);
                    AddRect(mask, 20, cy - 14, 24, 28);
                    AddRect(mask, roomWidth - 44, cy - 14, 24, 28);
                    break;
                case LayoutKind.BrokenCauseway:
                    AddRect(mask, 20, cy - 9, roomWidth - 40, 18);
                    AddRect(mask, 16, cy - 22, 44, 44);
                    AddRect(mask, roomWidth - 60, cy - 22, 44, 44);
                    RemoveRect(mask, cx - 8, cy - 9, 16, 18);
                    break;
                case LayoutKind.ReliquaryLoop:
                    AddRect(mask, 24, cy - 34, 24, 68);
                    AddRect(mask, 24, cy - 34, roomWidth - 48, 22);
                    AddRect(mask, roomWidth - 48, cy - 34, 24, 68);
                    RemoveRect(mask, cx - 26, cy - 10, 52, 36);
                    break;
                case LayoutKind.ForkedOssuary:
                    AddRect(mask, cx - 28, cy - 20, 56, 40);
                    AddRect(mask, 18, cy + 8, cx - 18, 18);
                    AddRect(mask, 18, cy + 26, 38, 18);
                    AddRect(mask, cx, cy - 26, roomWidth - cx - 18, 18);
                    AddRect(mask, roomWidth - 56, cy - 44, 38, 18);
                    break;
                case LayoutKind.AmbushCloister:
                    AddRect(mask, 22, cy - 34, roomWidth - 44, 68);
                    RemoveRect(mask, cx - 30, cy - 16, 60, 32);
                    AddRect(mask, 14, cy - 42, 22, 22);
                    AddRect(mask, roomWidth - 36, cy - 42, 22, 22);
                    AddRect(mask, 14, cy + 20, 22, 22);
                    AddRect(mask, roomWidth - 36, cy + 20, 22, 22);
                    break;
                case LayoutKind.RiftWell:
                    AddEllipse(mask, cx, cy, 24, 18);
                    AddRect(mask, cx - 10, 18, 20, cy - 18);
                    AddRect(mask, cx - 10, cy, 20, roomHeight - cy - 18);
                    AddRect(mask, 22, cy - 10, cx - 22, 20);
                    AddRect(mask, cx, cy - 10, roomWidth - cx - 22, 20);
                    break;
                case LayoutKind.BossAntechamber:
                    AddRect(mask, 18, cy - 30, roomWidth - 36, 60);
                    AddEllipse(mask, 20, cy, 14, 18);
                    AddEllipse(mask, roomWidth - 20, cy, 14, 18);
                    AddRect(mask, cx - 36, roomHeight - 36, 72, 18);
                    break;
                default:
                    AddRect(mask, 22, cy - 26, roomWidth - 44, 52);
                    AddRect(mask, 32, cy + 26, 28, 16);
                    AddRect(mask, roomWidth - 60, cy - 42, 28, 16);
                    break;
            }

            CarveBrokenEdges(mask, layout);
            AddDoorSockets(mask);
            EnsureCombatCore(mask, cx, cy);
            EnsureTraversalSpine(mask, cx, cy);
            return mask;
        }

        private bool[,] BuildVisualFloorMask(bool[,] playableMask)
        {
            bool[,] visualMask = new bool[roomWidth, roomHeight];
            int shellRadius = Mathf.Clamp(wallThickness + 8, 8, 14);

            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    visualMask[x, y] = playableMask[x, y] || TouchesFloorWithin(playableMask, x, y, shellRadius);
                }
            }

            return visualMask;
        }

        private Vector3Int OffsetCell(int x, int y)
        {
            return new Vector3Int(x + paintCellOffset.x, y + paintCellOffset.y, paintCellOffset.z);
        }

        private void PaintFloor(bool[,] floorMask)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (floorMask[x, y])
                    {
                        floorTilemap.SetTile(OffsetCell(x, y), PickFloorTile(x, y));
                    }
                }
            }
        }

        private void PaintCameraSafetyFloor()
        {
            int padding = Mathf.Clamp(cameraSafetyFloorPadding, 0, 16);
            if (padding <= 0) return;

            for (int x = -padding; x < roomWidth + padding; x++)
            {
                for (int y = -padding; y < roomHeight + padding; y++)
                {
                    if (x >= 0 && y >= 0 && x < roomWidth && y < roomHeight) continue;
                    floorTilemap.SetTile(OffsetCell(x, y), PickFloorTile(x, y));
                }
            }
        }

        private void PaintBoundaryWalls(bool[,] floorMask)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                for (int y = 0; y < roomHeight; y++)
                {
                    if (floorMask[x, y]) continue;
                    if (TouchesFloorWithin(floorMask, x, y, wallThickness))
                    {
                        wallTilemap.SetTile(OffsetCell(x, y), PickWallTile(x, y));
                    }
                }
            }
        }

        private void PaintLayoutFeatures(LayoutKind layout, bool[,] floorMask)
        {
            switch (layout)
            {
                case LayoutKind.ChainGallery:
                    PaintChainGallery(floorMask);
                    break;
                case LayoutKind.ShrineCrossroad:
                    PaintShrineCrossroad(floorMask);
                    break;
                case LayoutKind.CryptBasin:
                    PaintCryptBasin(floorMask);
                    break;
                case LayoutKind.PillarArena:
                    PaintPillarArena(floorMask);
                    break;
                case LayoutKind.SplitVault:
                    PaintSplitVault(floorMask);
                    break;
                case LayoutKind.RitualHall:
                    PaintRitualHall(floorMask);
                    break;
                case LayoutKind.CollapsedLibrary:
                    PaintCollapsedLibrary(floorMask);
                    break;
                case LayoutKind.NarrowApproach:
                    PaintNarrowApproach(floorMask);
                    break;
                case LayoutKind.CrescentSanctum:
                    PaintCrescentSanctum(floorMask);
                    break;
                case LayoutKind.BrokenCauseway:
                    PaintBrokenCauseway(floorMask);
                    break;
                case LayoutKind.ReliquaryLoop:
                    PaintReliquaryLoop(floorMask);
                    break;
                case LayoutKind.ForkedOssuary:
                    PaintForkedOssuary(floorMask);
                    break;
                case LayoutKind.AmbushCloister:
                    PaintAmbushCloister(floorMask);
                    break;
                case LayoutKind.RiftWell:
                    PaintRiftWell(floorMask);
                    break;
                case LayoutKind.BossAntechamber:
                    PaintBossAntechamber(floorMask);
                    break;
                default:
                    PaintBrokenEntryHall(floorMask);
                    break;
            }
        }

        private void PaintCombatViewKeepAnchors(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            PaintAnchorWallRect(floorMask, cx - 23, cy + 13, 12, 3);
            PaintAnchorWallRect(floorMask, cx + 11, cy + 13, 12, 3);
            PaintAnchorWallRect(floorMask, cx - 23, cy - 16, 12, 3);
            PaintAnchorWallRect(floorMask, cx + 11, cy - 16, 12, 3);

            PaintAnchorWallRect(floorMask, cx - 29, cy - 7, 3, 14);
            PaintAnchorWallRect(floorMask, cx + 26, cy - 7, 3, 14);

            PaintAnchorWallRect(floorMask, cx - 17, cy + 8, 4, 4);
            PaintAnchorWallRect(floorMask, cx + 13, cy + 8, 4, 4);
            PaintAnchorWallRect(floorMask, cx - 17, cy - 12, 4, 4);
            PaintAnchorWallRect(floorMask, cx + 13, cy - 12, 4, 4);
        }

        private void PaintAnchorWallRect(bool[,] floorMask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    if (!IsFloor(floorMask, px, py)) continue;
                    wallTilemap.SetTile(OffsetCell(px, py), PickWallTile(px, py));
                }
            }
        }

        private void PaintShelterCellPartitions(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            int sideRoomW = Mathf.Clamp(roomWidth / 7, 22, 36);
            int sideRoomH = Mathf.Clamp(roomHeight / 5, 18, 30);

            PaintWallRect(floorMask, wallThickness + 24, cy + sideRoomH / 2, sideRoomW, 3);
            PaintWallRect(floorMask, wallThickness + 24, cy - sideRoomH / 2 - 3, sideRoomW, 3);
            PaintWallRect(floorMask, roomWidth - wallThickness - 24 - sideRoomW, cy + sideRoomH / 2, sideRoomW, 3);
            PaintWallRect(floorMask, roomWidth - wallThickness - 24 - sideRoomW, cy - sideRoomH / 2 - 3, sideRoomW, 3);

            PaintWallRect(floorMask, cx - sideRoomW - 18, roomHeight - wallThickness - 28, sideRoomW, 3);
            PaintWallRect(floorMask, cx + 18, roomHeight - wallThickness - 28, sideRoomW, 3);
            PaintWallRect(floorMask, cx - sideRoomW - 18, wallThickness + 25, sideRoomW, 3);
            PaintWallRect(floorMask, cx + 18, wallThickness + 25, sideRoomW, 3);

            PaintWallRect(floorMask, wallThickness + 28, cy - 18, 3, 36);
            PaintWallRect(floorMask, roomWidth - wallThickness - 31, cy - 18, 3, 36);
        }

        private void PaintBrokenEntryHall(bool[,] floorMask)
        {
            PaintPillarGrid(floorMask, 20, 14, 2, 2, 2);
            PaintWallRect(floorMask, 12, roomHeight - 17, 12, 3);
            PaintWallRect(floorMask, roomWidth - 28, 13, 13, 3);
            PaintWallRect(floorMask, roomWidth / 2 - 18, roomHeight / 2 + 17, 11, 3);
        }

        private void PaintChainGallery(bool[,] floorMask)
        {
            PaintWallRect(floorMask, 28, roomHeight / 2 - 12, 5, 24);
            PaintWallRect(floorMask, roomWidth - 42, roomHeight / 2 - 16, 5, 32);
            PaintPillarGrid(floorMask, 18, 10, 4, 2, 2);
        }

        private void PaintShrineCrossroad(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 25, cy + 12, 15, 3);
            PaintWallRect(floorMask, cx + 10, cy + 12, 15, 3);
            PaintWallRect(floorMask, cx - 24, cy - 15, 14, 3);
            PaintWallRect(floorMask, cx + 11, cy - 15, 14, 3);
            PaintPillarGrid(floorMask, 18, 16, 2, 2, 2);
        }

        private void PaintCryptBasin(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 19, cy - 3, 9, 3);
            PaintWallRect(floorMask, cx + 10, cy + 2, 9, 3);
            PaintPillarGrid(floorMask, 22, 15, 2, 2, 2);
        }

        private void PaintPillarArena(bool[,] floorMask)
        {
            PaintPillarGrid(floorMask, 22, 16, 3, 2, 2);
            PaintWallRect(floorMask, 18, roomHeight - 23, 14, 3);
            PaintWallRect(floorMask, roomWidth - 36, 20, 16, 3);
        }

        private void PaintSplitVault(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            PaintWallRect(floorMask, cx - 4, roomHeight / 2 + 16, 8, 18);
            PaintWallRect(floorMask, cx - 4, roomHeight / 2 - 34, 8, 18);
            PaintWallRect(floorMask, cx - 2, roomHeight / 2 - 5, 4, 10);
            PaintPillarGrid(floorMask, 24, 14, 2, 2, 2);
        }

        private void PaintRitualHall(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 9, cy - 2, 18, 4);
            PaintWallRect(floorMask, cx - 35, cy + 18, 14, 3);
            PaintWallRect(floorMask, cx + 21, cy - 20, 14, 3);
            PaintPillarGrid(floorMask, 28, 18, 2, 2, 2);
        }

        private void PaintCollapsedLibrary(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 60, cy - 18, 30, 3);
            PaintWallRect(floorMask, cx - 58, cy + 18, 28, 3);
            PaintWallRect(floorMask, cx + 28, cy - 20, 34, 3);
            PaintWallRect(floorMask, cx + 24, cy + 17, 36, 3);
            PaintPillarGrid(floorMask, 30, 18, 2, 2, 2);
        }

        private void PaintNarrowApproach(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 8, cy + 15, 18, 3);
            PaintWallRect(floorMask, cx + 24, cy - 15, 20, 3);
            PaintPillarGrid(floorMask, 24, 12, 3, 1, 2);
        }

        private void PaintCrescentSanctum(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 48, cy + 18, 18, 3);
            PaintWallRect(floorMask, cx - 50, cy - 21, 20, 3);
            PaintWallRect(floorMask, cx + 18, cy + 22, 12, 4);
            PaintPillarGrid(floorMask, 26, 17, 2, 2, 2);
        }

        private void PaintBrokenCauseway(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 46, cy - 11, 22, 3);
            PaintWallRect(floorMask, cx - 3, cy + 14, 20, 3);
            PaintWallRect(floorMask, cx + 42, cy - 10, 24, 3);
            PaintPillarGrid(floorMask, 34, 12, 3, 1, 2);
        }

        private void PaintReliquaryLoop(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 45, cy + 14, 16, 3);
            PaintWallRect(floorMask, cx + 29, cy - 17, 17, 3);
            PaintWallRect(floorMask, cx - 6, cy - 36, 12, 4);
            PaintWallRect(floorMask, cx - 6, cy + 32, 12, 4);
            PaintPillarGrid(floorMask, 28, 20, 2, 2, 2);
        }

        private void PaintForkedOssuary(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 65, cy + 9, 17, 3);
            PaintWallRect(floorMask, cx + 47, cy + 9, 18, 3);
            PaintWallRect(floorMask, cx - 40, cy - 14, 14, 3);
            PaintWallRect(floorMask, cx + 28, cy - 17, 14, 3);
            PaintPillarGrid(floorMask, 30, 16, 2, 2, 2);
        }

        private void PaintAmbushCloister(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 54, cy + 28, 28, 3);
            PaintWallRect(floorMask, cx + 26, cy + 28, 28, 3);
            PaintWallRect(floorMask, cx - 54, cy - 31, 28, 3);
            PaintWallRect(floorMask, cx + 26, cy - 31, 28, 3);
            PaintPillarGrid(floorMask, 30, 22, 3, 2, 2);
        }

        private void PaintRiftWell(bool[,] floorMask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            PaintWallRect(floorMask, cx - 24, cy + 20, 16, 3);
            PaintWallRect(floorMask, cx + 8, cy - 23, 17, 3);
            PaintWallRect(floorMask, cx - 55, cy - 5, 16, 3);
            PaintWallRect(floorMask, cx + 39, cy + 5, 16, 3);
            PaintPillarGrid(floorMask, 28, 18, 2, 2, 2);
        }

        private void PaintBossAntechamber(bool[,] floorMask)
        {
            PaintWallRect(floorMask, roomWidth / 2 - 8, roomHeight - wallThickness - 13, 16, 3);
            PaintWallRect(floorMask, roomWidth / 2 - 8, wallThickness + 10, 16, 3);
            PaintPillarGrid(floorMask, 24, 16, 3, 2, 2);
        }

        private void PaintPillarGrid(bool[,] floorMask, int spacingX, int spacingY, int columns, int rows, int radius)
        {
            int startX = (roomWidth - (columns - 1) * spacingX) / 2;
            int startY = (roomHeight - (rows - 1) * spacingY) / 2;

            for (int ix = 0; ix < columns; ix++)
            {
                for (int iy = 0; iy < rows; iy++)
                {
                    PaintWallRect(floorMask, startX + ix * spacingX - radius, startY + iy * spacingY - radius, radius * 2 + 1, radius * 2 + 1);
                }
            }
        }

        private void PaintWallRect(bool[,] floorMask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    if (!IsFloor(floorMask, px, py)) continue;
                    if (IsProtectedTraversalCell(px, py)) continue;
                    wallTilemap.SetTile(OffsetCell(px, py), PickWallTile(px, py));
                }
            }
        }

        private bool IsProtectedTraversalCell(int x, int y)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;

            if (IsNearCenter(x, y, 18)) return true;
            if (Mathf.Abs(x - cx) <= 12 && y <= wallThickness + 30) return true;
            if (Mathf.Abs(x - cx) <= 12 && y >= roomHeight - wallThickness - 34) return true;
            if (Mathf.Abs(y - cy) <= 9 && x <= wallThickness + 34) return true;
            if (Mathf.Abs(y - cy) <= 9 && x >= roomWidth - wallThickness - 36) return true;

            return false;
        }

        private void PaintNarrativeDecor(LayoutKind layout, bool[,] floorMask)
        {
            Transform root = GetOrCreateDecorRoot();
            ClearChildren(root);

            foreach (DecorSpec spec in GetDecorSpecs(layout))
            {
                Sprite sprite = RimaGeneratedSpriteCache.Load(spec.spritePath);
                if (sprite == null) continue;

                Vector2Int preferred = new Vector2Int(
                    Mathf.RoundToInt(spec.normalizedPosition.x * (roomWidth - 1)),
                    Mathf.RoundToInt(spec.normalizedPosition.y * (roomHeight - 1)));

                if (!TryFindNearestFloorCell(floorMask, preferred, 24, out Vector2Int cell))
                    continue;

                Vector3 world = floorTilemap.GetCellCenterWorld(OffsetCell(cell.x, cell.y));
                var go = new GameObject("Room Decor - " + sprite.name);
                go.transform.SetParent(root, false);
                go.transform.position = new Vector3(world.x, world.y, -0.08f);
                go.transform.localScale = Vector3.one * spec.scale;

                var renderer = go.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingOrder = spec.sortingOrder + cell.y;
                renderer.color = Color.white;
            }
        }

        private IEnumerable<DecorSpec> GetDecorSpecs(LayoutKind layout)
        {
            const string decor = "Environment/StoneDungeon/Decor/";
            const string walls = "Environment/StoneDungeon/Walls/";

            yield return new DecorSpec(walls + "RIMA_gate_arch", new Vector2(0.50f, 0.88f), 1.45f, 40);
            yield return new DecorSpec(walls + "RIMA_gate_spikes", new Vector2(0.50f, 0.14f), 1.25f, 40);

            switch (layout)
            {
                case LayoutKind.ChainGallery:
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.20f, 0.62f), 1.10f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_banner", new Vector2(0.78f, 0.38f), 1.12f, 42);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.36f, 0.30f), 0.88f, 35);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.62f, 0.70f), 0.90f, 35);
                    break;
                case LayoutKind.ShrineCrossroad:
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.50f, 0.55f), 1.18f, 45);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.24f, 0.72f), 1.02f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.76f, 0.28f), 1.02f, 42);
                    break;
                case LayoutKind.CryptBasin:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.52f, 0.52f), 1.08f, 45);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.30f, 0.70f), 0.82f, 35);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.75f, 0.28f), 1.05f, 42);
                    break;
                case LayoutKind.PillarArena:
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.28f, 0.65f), 0.90f, 38);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.72f, 0.35f), 0.90f, 38);
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.50f, 0.76f), 1.15f, 42);
                    break;
                case LayoutKind.SplitVault:
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.34f, 0.52f), 1.08f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.66f, 0.52f), 1.08f, 42);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.28f), 0.88f, 45);
                    break;
                case LayoutKind.RitualHall:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.50f), 1.12f, 45);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.26f, 0.72f), 0.95f, 44);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.74f, 0.28f), 0.95f, 44);
                    break;
                case LayoutKind.CollapsedLibrary:
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.30f, 0.46f), 1.05f, 36);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.68f, 0.58f), 0.95f, 36);
                    yield return new DecorSpec(walls + "RIMA_wall_banner", new Vector2(0.50f, 0.78f), 1.10f, 42);
                    break;
                case LayoutKind.NarrowApproach:
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.22f, 0.60f), 1.05f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.72f, 0.42f), 1.08f, 42);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.50f, 0.30f), 0.82f, 35);
                    break;
                case LayoutKind.CrescentSanctum:
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.34f, 0.42f), 1.05f, 44);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.62f, 0.58f), 0.95f, 45);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.22f, 0.72f), 1.00f, 42);
                    break;
                case LayoutKind.BrokenCauseway:
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.48f, 0.42f), 1.05f, 36);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.26f, 0.58f), 1.05f, 42);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.76f, 0.55f), 0.88f, 45);
                    break;
                case LayoutKind.ReliquaryLoop:
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.50f, 0.30f), 1.00f, 44);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.50f, 0.72f), 1.00f, 44);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.28f, 0.50f), 1.00f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.72f, 0.50f), 1.00f, 42);
                    break;
                case LayoutKind.ForkedOssuary:
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.34f, 0.32f), 0.92f, 36);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.66f, 0.34f), 0.92f, 36);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.24f, 0.72f), 1.00f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.76f, 0.72f), 1.00f, 42);
                    break;
                case LayoutKind.AmbushCloister:
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.28f, 0.74f), 1.10f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_iron", new Vector2(0.72f, 0.26f), 1.10f, 42);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.20f, 0.48f), 0.86f, 38);
                    yield return new DecorSpec(decor + "RIMA_pillar_base", new Vector2(0.80f, 0.52f), 0.86f, 38);
                    break;
                case LayoutKind.RiftWell:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.52f), 1.20f, 45);
                    yield return new DecorSpec(decor + "RIMA_shrine", new Vector2(0.28f, 0.30f), 0.88f, 44);
                    yield return new DecorSpec(walls + "RIMA_wall_cracked", new Vector2(0.72f, 0.70f), 1.05f, 42);
                    break;
                case LayoutKind.BossAntechamber:
                    yield return new DecorSpec(walls + "RIMA_gate_arch", new Vector2(0.50f, 0.76f), 1.65f, 45);
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.48f), 1.25f, 48);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.30f, 0.68f), 1.10f, 42);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.70f, 0.68f), 1.10f, 42);
                    break;
                default:
                    yield return new DecorSpec(decor + "RIMA_rift_crystal", new Vector2(0.50f, 0.50f), 1.00f, 45);
                    yield return new DecorSpec(decor + "RIMA_debris", new Vector2(0.32f, 0.34f), 0.86f, 35);
                    yield return new DecorSpec(walls + "RIMA_wall_torch", new Vector2(0.72f, 0.68f), 1.00f, 42);
                    break;
            }
        }

        private Transform GetOrCreateDecorRoot()
        {
            Transform existing = transform.Find(proceduralDecorRootName);
            if (existing != null) return existing;

            var root = new GameObject(proceduralDecorRootName);
            root.transform.SetParent(transform, false);
            return root.transform;
        }

        private void PaintRoomLighting(LayoutKind layout, bool[,] floorMask)
        {
            if (!createProceduralLights) return;

            TuneGlobalLight();

            Transform root = GetOrCreateLightRoot();
            ClearChildren(root);

            foreach (RoomLightSpec spec in GetLightSpecs(layout))
            {
                Vector2Int preferred = new Vector2Int(
                    Mathf.RoundToInt(spec.normalizedPosition.x * (roomWidth - 1)),
                    Mathf.RoundToInt(spec.normalizedPosition.y * (roomHeight - 1)));

                if (!TryFindNearestFloorCell(floorMask, preferred, 18, out Vector2Int cell))
                    continue;

                Vector3 world = floorTilemap.GetCellCenterWorld(OffsetCell(cell.x, cell.y));
                var lightObject = new GameObject("Room Light");
                lightObject.transform.SetParent(root, false);
                lightObject.transform.position = new Vector3(world.x, world.y, -1f);

                Light2D light = lightObject.AddComponent<Light2D>();
                light.lightType = Light2D.LightType.Point;
                light.color = spec.color;
                light.intensity = spec.intensity * localLightAccentScale;
                light.pointLightInnerRadius = spec.innerRadius;
                light.pointLightOuterRadius = spec.outerRadius;
                light.falloffIntensity = 0.78f;
                light.shadowsEnabled = false;
                ApplyLightToAllSortingLayers(light);

                if (spec.flicker)
                {
                    lightObject.AddComponent<RIMA.Environment.LightFlicker>();
                }

                var poolObject = new GameObject("Room Light Pool");
                poolObject.transform.SetParent(root, false);
                poolObject.transform.position = new Vector3(world.x, world.y, 0f);
                float poolAlpha = Mathf.Clamp(spec.intensity * localLightAccentScale * 0.13f, 0.07f, 0.22f);
                poolObject.AddComponent<RIMA.Environment.RoomMoodLightPool>()
                    .Configure(spec.color, spec.outerRadius * 0.82f, poolAlpha);
            }
        }

        private IEnumerable<RoomLightSpec> GetLightSpecs(LayoutKind layout)
        {
            Color torch = new Color(1f, 0.43f, 0.16f, 1f);
            Color ember = new Color(1f, 0.28f, 0.10f, 1f);
            Color cyan = new Color(0.18f, 0.72f, 1f, 1f);
            Color violet = new Color(0.45f, 0.32f, 0.95f, 1f);
            Color moon = new Color(0.25f, 0.36f, 0.52f, 1f);

            yield return new RoomLightSpec(new Vector2(0.50f, 0.78f), moon, 0.24f, 2.4f, 10.0f, false);

            switch (layout)
            {
                case LayoutKind.ShrineCrossroad:
                case LayoutKind.ReliquaryLoop:
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.52f), cyan, 0.72f, 1.8f, 7.2f, false);
                    yield return new RoomLightSpec(new Vector2(0.24f, 0.70f), torch, 0.86f, 0.9f, 4.7f, true);
                    yield return new RoomLightSpec(new Vector2(0.76f, 0.30f), torch, 0.72f, 0.9f, 4.4f, true);
                    break;

                case LayoutKind.CryptBasin:
                case LayoutKind.RiftWell:
                case LayoutKind.BossAntechamber:
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.52f), violet, 0.82f, 2.1f, 8.0f, false);
                    yield return new RoomLightSpec(new Vector2(0.30f, 0.68f), torch, 0.72f, 0.9f, 4.5f, true);
                    yield return new RoomLightSpec(new Vector2(0.70f, 0.68f), torch, 0.72f, 0.9f, 4.5f, true);
                    break;

                case LayoutKind.BrokenCauseway:
                case LayoutKind.CollapsedLibrary:
                case LayoutKind.NarrowApproach:
                    yield return new RoomLightSpec(new Vector2(0.22f, 0.58f), torch, 0.78f, 0.9f, 4.6f, true);
                    yield return new RoomLightSpec(new Vector2(0.74f, 0.46f), cyan, 0.58f, 1.2f, 5.6f, false);
                    yield return new RoomLightSpec(new Vector2(0.48f, 0.34f), ember, 0.42f, 1.0f, 4.2f, true);
                    break;

                case LayoutKind.AmbushCloister:
                case LayoutKind.ForkedOssuary:
                case LayoutKind.PillarArena:
                    yield return new RoomLightSpec(new Vector2(0.24f, 0.72f), torch, 0.72f, 0.9f, 4.5f, true);
                    yield return new RoomLightSpec(new Vector2(0.76f, 0.28f), torch, 0.72f, 0.9f, 4.5f, true);
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.50f), cyan, 0.40f, 1.5f, 6.0f, false);
                    break;

                default:
                    yield return new RoomLightSpec(new Vector2(0.28f, 0.68f), torch, 0.76f, 0.9f, 4.6f, true);
                    yield return new RoomLightSpec(new Vector2(0.72f, 0.36f), torch, 0.68f, 0.9f, 4.3f, true);
                    yield return new RoomLightSpec(new Vector2(0.50f, 0.50f), cyan, 0.50f, 1.6f, 6.2f, false);
                    break;
            }
        }

        private void TuneGlobalLight()
        {
            Light2D[] lights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
            foreach (Light2D light in lights)
            {
                if (light.lightType != Light2D.LightType.Global) continue;
                if (!light.name.Contains("Global")) continue;

                light.intensity = globalLightIntensity;
                light.color = globalLightColor;
                ApplyLightToAllSortingLayers(light);
            }
        }

        private static void ApplyLightToAllSortingLayers(Light2D light)
        {
            if (light == null) return;

            SortingLayer[] layers = SortingLayer.layers;
            if (layers == null || layers.Length == 0) return;

            int[] layerIds = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                layerIds[i] = layers[i].id;
            }

            light.targetSortingLayers = layerIds;
        }

        private Transform GetOrCreateLightRoot()
        {
            Transform existing = transform.Find(proceduralLightRootName);
            if (existing != null) return existing;

            var root = new GameObject(proceduralLightRootName);
            root.transform.SetParent(transform, false);
            return root.transform;
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject child = root.GetChild(i).gameObject;
                if (Application.isPlaying)
                    Object.Destroy(child);
                else
                    Object.DestroyImmediate(child);
            }
        }

        private bool TryFindNearestFloorCell(bool[,] floorMask, Vector2Int preferred, int maxRadius, out Vector2Int result)
        {
            for (int radius = 0; radius <= maxRadius; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (Mathf.Abs(dx) != radius && Mathf.Abs(dy) != radius) continue;
                        int x = preferred.x + dx;
                        int y = preferred.y + dy;
                        if (!IsFloor(floorMask, x, y)) continue;

                        result = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            result = default;
            return false;
        }

        private void AddDoorSockets(bool[,] mask)
        {
            int cx = roomWidth / 2;
            int cy = roomHeight / 2;
            AddRect(mask, cx - 6, wallThickness + 1, 12, 18);
            AddRect(mask, cx - 6, roomHeight - wallThickness - 19, 12, 18);
            AddRect(mask, wallThickness + 1, cy - 5, 18, 10);
            AddRect(mask, roomWidth - wallThickness - 19, cy - 5, 18, 10);
        }

        private void EnsureCombatCore(bool[,] mask, int cx, int cy)
        {
            AddEllipse(mask, cx, cy, 24, 17);
            AddRect(mask, cx - 22, cy - 14, 44, 28);
        }

        private void EnsureTraversalSpine(bool[,] mask, int cx, int cy)
        {
            AddRect(mask, cx - 8, wallThickness + 1, 16, roomHeight - wallThickness * 2 - 2);
            AddRect(mask, wallThickness + 1, cy - 6, roomWidth - wallThickness * 2 - 2, 12);
            AddEllipse(mask, cx, cy, 30, 21);
        }

        private void CarveBrokenEdges(bool[,] mask, LayoutKind layout)
        {
            int salt = seed + (int)layout * 4099;
            for (int pass = 0; pass < 2; pass++)
            {
                for (int x = wallThickness + 1; x < roomWidth - wallThickness - 1; x++)
                {
                    for (int y = wallThickness + 1; y < roomHeight - wallThickness - 1; y++)
                    {
                        if (!mask[x, y]) continue;
                        if (IsNearCenter(x, y, 18)) continue;
                        if (CountMissingNeighbors(mask, x, y) < 4) continue;

                        int roll = Mathf.Abs((x * 92837111) ^ (y * 689287499) ^ salt ^ pass) % 100;
                        if (roll < 24)
                        {
                            mask[x, y] = false;
                        }
                    }
                }
            }
        }

        private int CountMissingNeighbors(bool[,] mask, int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (!IsFloor(mask, x + dx, y + dy)) count++;
                }
            }
            return count;
        }

        private bool TouchesFloorWithin(bool[,] mask, int x, int y, int radius)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (IsFloor(mask, x + dx, y + dy)) return true;
                }
            }
            return false;
        }

        private void AddRect(bool[,] mask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    SetFloor(mask, px, py, true);
                }
            }
        }

        private void RemoveRect(bool[,] mask, int x, int y, int width, int height)
        {
            for (int px = x; px < x + width; px++)
            {
                for (int py = y; py < y + height; py++)
                {
                    SetFloor(mask, px, py, false);
                }
            }
        }

        private void AddEllipse(bool[,] mask, int cx, int cy, int rx, int ry)
        {
            PaintEllipse(mask, cx, cy, rx, ry, true);
        }

        private void RemoveEllipse(bool[,] mask, int cx, int cy, int rx, int ry)
        {
            PaintEllipse(mask, cx, cy, rx, ry, false);
        }

        private void PaintEllipse(bool[,] mask, int cx, int cy, int rx, int ry, bool value)
        {
            int xMin = cx - rx;
            int xMax = cx + rx;
            int yMin = cy - ry;
            int yMax = cy + ry;
            float invRx = 1f / Mathf.Max(1, rx);
            float invRy = 1f / Mathf.Max(1, ry);

            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    float nx = (x - cx) * invRx;
                    float ny = (y - cy) * invRy;
                    if (nx * nx + ny * ny <= 1f)
                    {
                        SetFloor(mask, x, y, value);
                    }
                }
            }
        }

        private void SetFloor(bool[,] mask, int x, int y, bool value)
        {
            if (x < wallThickness || y < wallThickness) return;
            if (x >= roomWidth - wallThickness || y >= roomHeight - wallThickness) return;
            mask[x, y] = value;
        }

        private bool IsFloor(bool[,] mask, int x, int y)
        {
            return x >= 0 && y >= 0 && x < roomWidth && y < roomHeight && mask[x, y];
        }

        private bool IsNearCenter(int x, int y, int radius)
        {
            int dx = x - roomWidth / 2;
            int dy = y - roomHeight / 2;
            return dx * dx + dy * dy <= radius * radius;
        }

        private TileBase PickFloorTile(int x, int y)
        {
            int roll = Mathf.Abs((x * 73856093) ^ (y * 19349663) ^ seed) % 100;
            int index = roll switch
            {
                < 68 => 0,
                < 82 => Mathf.Min(1, cachedFloorTiles.Count - 1),
                < 92 => Mathf.Min(2, cachedFloorTiles.Count - 1),
                < 97 => Mathf.Min(3, cachedFloorTiles.Count - 1),
                _ => cachedFloorTiles.Count - 1,
            };

            return cachedFloorTiles[Mathf.Clamp(index, 0, cachedFloorTiles.Count - 1)];
        }

        private TileBase PickWallTile(int x, int y)
        {
            int roll = Mathf.Abs((x * 83492791) ^ (y * 297121507) ^ seed) % 100;
            int index = roll switch
            {
                < 72 => 0,
                < 88 => Mathf.Min(1, cachedWallTiles.Count - 1),
                < 96 => Mathf.Min(2, cachedWallTiles.Count - 1),
                _ => cachedWallTiles.Count - 1,
            };

            return cachedWallTiles[Mathf.Clamp(index, 0, cachedWallTiles.Count - 1)];
        }
    }
}
