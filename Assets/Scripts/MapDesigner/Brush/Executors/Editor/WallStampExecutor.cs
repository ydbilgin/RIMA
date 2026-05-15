#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.Data;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class WallStampExecutor : IBrushExecutor
    {
        private const string PainterName = "WallOverlayPainter";
        private const string RootName = "WallOverlay";

        public PaintMode SupportedMode
        {
            get { return PaintMode.Stamp; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            if (op == null || op.assetPool == null || op.assetPool.sprites == null || op.assetPool.sprites.Count == 0)
            {
                return Error("Wall sprite asset pool is empty");
            }

            WallSegment segment;
            if (!TryFindNearestSegment(stroke.currentCell, stroke.room, out segment))
            {
                return Error("No wall segment found for stamp");
            }

            Sprite sprite = PickSprite(op.assetPool, segment, stroke.seed);
            if (sprite == null)
            {
                return Error("No matching wall sprite found");
            }

            WallOverlayPainter painter = ResolvePainter();
            Transform parent = EnsureWallRoot(painter.transform);
            Tilemap tilemap = ResolveTilemap();
            GameObject spawned = painter.PlaceWallSprite(segment, sprite, parent, tilemap, parent.childCount);
            if (spawned == null)
            {
                return Error("WallOverlayPainter failed to place sprite");
            }

            Undo.RegisterCreatedObjectUndo(spawned, "Brush Wall Stamp");
            return new BrushExecutorResult
            {
                success = true,
                spawnedCount = 1,
                spawnedObjects = new List<GameObject> { spawned }
            };
        }

        private static bool TryFindNearestSegment(Vector2Int cell, RoomData room, out WallSegment nearest)
        {
            nearest = default(WallSegment);
            if (room.wallEdges == null || room.wallEdges.Count == 0)
            {
                return false;
            }

            int bestDistance = int.MaxValue;
            for (int i = 0; i < room.wallEdges.Count; i++)
            {
                WallSegment segment = room.wallEdges[i];
                int distance = SegmentDistance(cell, segment);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    nearest = segment;
                }
            }

            return true;
        }

        private static int SegmentDistance(Vector2Int cell, WallSegment segment)
        {
            int start = Mathf.Abs(cell.x - segment.start.x) + Mathf.Abs(cell.y - segment.start.y);
            int end = Mathf.Abs(cell.x - segment.end.x) + Mathf.Abs(cell.y - segment.end.y);
            return Mathf.Min(start, end);
        }

        private static Sprite PickSprite(AssetPoolSO pool, WallSegment segment, int seed)
        {
            int start = PositiveModulo(Mix(seed, (int)segment.direction, segment.isCorner ? 1 : 0), pool.sprites.Count);
            for (int i = 0; i < pool.sprites.Count; i++)
            {
                Sprite candidate = pool.sprites[(start + i) % pool.sprites.Count];
                if (candidate != null && MatchesSegment(pool.category, segment))
                {
                    return candidate;
                }
            }

            for (int i = 0; i < pool.sprites.Count; i++)
            {
                if (pool.sprites[i] != null)
                {
                    return pool.sprites[i];
                }
            }

            return null;
        }

        private static bool MatchesSegment(AssetCategory category, WallSegment segment)
        {
            if (segment.isDoorway)
            {
                return category == AssetCategory.Doorway || category == AssetCategory.Wall;
            }

            if (segment.isCorner)
            {
                return category == AssetCategory.WallCorner || category == AssetCategory.Wall;
            }

            return category == AssetCategory.Wall;
        }

        private static WallOverlayPainter ResolvePainter()
        {
            WallOverlayPainter[] painters = UnityEngine.Object.FindObjectsByType<WallOverlayPainter>(FindObjectsSortMode.None);
            if (painters.Length > 0 && painters[0] != null)
            {
                return painters[0];
            }

            GameObject host = new GameObject(PainterName);
            return host.AddComponent<WallOverlayPainter>();
        }

        private static Transform EnsureWallRoot(Transform host)
        {
            Transform existing = host.Find(RootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(RootName);
            root.transform.SetParent(host, false);
            return root.transform;
        }

        private static Tilemap ResolveTilemap()
        {
            Tilemap[] tilemaps = UnityEngine.Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            if (tilemaps.Length == 0)
            {
                return null;
            }

            return tilemaps[0];
        }

        private static int Mix(int seed, int a, int b)
        {
            unchecked
            {
                int hash = seed;
                hash = (hash * 397) ^ a;
                hash = (hash * 397) ^ b;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
