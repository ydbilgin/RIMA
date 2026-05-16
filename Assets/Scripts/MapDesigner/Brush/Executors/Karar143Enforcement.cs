using RIMA.Data;
using RIMA.MapDesigner.Brush.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors
{
    public static class Karar143Enforcement
    {
        public static bool IsCellWalkable(Vector2Int cell, RoomData room)
        {
            if (room.walkable == null)
            {
                return false;
            }

            int width = room.walkable.GetLength(0);
            int height = room.walkable.GetLength(1);
            return cell.x >= 0 &&
                cell.y >= 0 &&
                cell.x < width &&
                cell.y < height &&
                room.walkable[cell.x, cell.y];
        }

        public static float ComputeWallProximityMultiplier(Vector2Int cell, RoomData room, AnimationCurve curve)
        {
            if (curve == null || curve.keys == null || curve.keys.Length == 0)
            {
                return 1f;
            }

            int distToWall = ComputeDistanceToNearestWall(cell, room);
            return curve.Evaluate(distToWall);
        }

        public static float SampleFeatureMask(Vector2 worldPos, RoomData room, FeatureMaskSO mask)
        {
            if (mask == null || mask.alphaMask == null)
            {
                return 1f;
            }

            Vector2Int size = GetRoomSize(room);
            if (size.x <= 0 || size.y <= 0)
            {
                return 1f;
            }

            Vector2 safeScale = mask.scale == Vector2.zero ? Vector2.one : mask.scale;
            float u = (worldPos.x / size.x) * safeScale.x + mask.worldOffset.x;
            float v = (worldPos.y / size.y) * safeScale.y + mask.worldOffset.y;
            float alpha = mask.alphaMask.GetPixelBilinear(u, v).a;
            if (mask.invert)
            {
                alpha = 1f - alpha;
            }

            return mask.remap != null && mask.remap.keys != null && mask.remap.keys.Length > 0
                ? mask.remap.Evaluate(alpha)
                : alpha;
        }

        public static float EffectiveDensity(Vector2Int cell, Vector2 worldPos, RoomData room, BrushLayerOperation op)
        {
            if (op == null)
            {
                return 0f;
            }

            if (op.respectsWalkableMask && !IsCellWalkable(cell, room))
            {
                return 0f;
            }

            float baseDensity = Mathf.Max(0f, op.density);
            float wallMul = ComputeWallProximityMultiplier(cell, room, op.wallProximityCurve);
            float maskMul = op.featureMaskMultiplier != null
                ? SampleFeatureMask(worldPos, room, op.featureMaskMultiplier)
                : 1f;
            return baseDensity * wallMul * maskMul;
        }

        private static int ComputeDistanceToNearestWall(Vector2Int cell, RoomData room)
        {
            if (room.walkable == null)
            {
                return int.MaxValue;
            }

            int width = room.walkable.GetLength(0);
            int height = room.walkable.GetLength(1);
            if (cell.x < 0 || cell.y < 0 || cell.x >= width || cell.y >= height)
            {
                return int.MaxValue;
            }

            int best = int.MaxValue;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool boundary = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                    if (!room.walkable[x, y] || boundary)
                    {
                        int distance = Mathf.Abs(cell.x - x) + Mathf.Abs(cell.y - y);
                        if (distance < best)
                        {
                            best = distance;
                        }
                    }
                }
            }

            return best == int.MaxValue ? 0 : best;
        }

        private static Vector2Int GetRoomSize(RoomData room)
        {
            if (room.size.x > 0 && room.size.y > 0)
            {
                return room.size;
            }

            return room.walkable != null
                ? new Vector2Int(room.walkable.GetLength(0), room.walkable.GetLength(1))
                : Vector2Int.zero;
        }
    }
}
