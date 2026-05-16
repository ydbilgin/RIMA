using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Composition
{
    public static class CompositionRoleMapGenerator
    {
        public const int DecoratedEdgeWidth = 2;
        public const int DoorSafetyRadius = 3;

        public static CompositionRoleMap GenerateFromRoom(RoomTemplateSO room)
        {
            if (room == null)
            {
                return new CompositionRoleMap(new RectInt(0, 0, 0, 0));
            }

            RectInt bounds = room.bounds;
            CompositionRoleMap map = new CompositionRoleMap(bounds);

            FillCleanCenter(map, bounds);
            MarkDecoratedEdge(map, bounds);
            MarkWallBand(map, bounds);
            MarkDoorSafety(map, bounds, room);

            return map;
        }

        private static void FillCleanCenter(CompositionRoleMap map, RectInt bounds)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    map.SetRoleAt(new Vector2Int(x, y), CompositionRole.CleanCenter);
                }
            }
        }

        private static void MarkDecoratedEdge(CompositionRoleMap map, RectInt bounds)
        {
            if (bounds.width < 2 * (DecoratedEdgeWidth + 1) || bounds.height < 2 * (DecoratedEdgeWidth + 1))
            {
                return;
            }

            int innerInset = 1;
            int outerInset = innerInset + DecoratedEdgeWidth;
            for (int y = bounds.yMin + innerInset; y < bounds.yMax - innerInset; y++)
            {
                for (int x = bounds.xMin + innerInset; x < bounds.xMax - innerInset; x++)
                {
                    bool nearLeft = x < bounds.xMin + outerInset;
                    bool nearRight = x >= bounds.xMax - outerInset;
                    bool nearBottom = y < bounds.yMin + outerInset;
                    bool nearTop = y >= bounds.yMax - outerInset;
                    if (nearLeft || nearRight || nearBottom || nearTop)
                    {
                        map.SetRoleAt(new Vector2Int(x, y), CompositionRole.DecoratedEdge);
                    }
                }
            }
        }

        private static void MarkWallBand(CompositionRoleMap map, RectInt bounds)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                map.SetRoleAt(new Vector2Int(x, bounds.yMin), CompositionRole.WallBand);
                map.SetRoleAt(new Vector2Int(x, bounds.yMax - 1), CompositionRole.WallBand);
            }
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                map.SetRoleAt(new Vector2Int(bounds.xMin, y), CompositionRole.WallBand);
                map.SetRoleAt(new Vector2Int(bounds.xMax - 1, y), CompositionRole.WallBand);
            }
        }

        private static void MarkDoorSafety(CompositionRoleMap map, RectInt bounds, RoomTemplateSO room)
        {
            if (room.doorSockets == null) return;
            int r = DoorSafetyRadius;
            foreach (var door in room.doorSockets)
            {
                if (door == null) continue;
                Vector2Int c = door.position;
                for (int dy = -r; dy <= r; dy++)
                {
                    for (int dx = -r; dx <= r; dx++)
                    {
                        if (Mathf.Abs(dx) + Mathf.Abs(dy) > r) continue;
                        Vector2Int p = new Vector2Int(c.x + dx, c.y + dy);
                        if (p.x < bounds.xMin || p.x >= bounds.xMax ||
                            p.y < bounds.yMin || p.y >= bounds.yMax) continue;
                        map.SetRoleAt(p, CompositionRole.DoorSafety);
                    }
                }
            }
        }
    }
}
