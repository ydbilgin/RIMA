using System.Collections.Generic;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public static class RoomDataMutator
    {
        public static void PutFloorCell(
            RoomData room,
            string assetGuidOrName,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            PutFloorCell(room, assetGuidOrName, null, cell, worldPos, rotation, scale);
        }

        public static void PutFloorCell(
            RoomData room,
            string assetGuidOrName,
            string sourceGroupId,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            if (room == null) return;
            room.EnsureDefaults();
            PutTileCell(room.floorCells, assetGuidOrName, sourceGroupId, cell, worldPos, rotation, scale);
        }

        public static void PutCliffCell(
            RoomData room,
            string assetGuidOrName,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            PutCliffCell(room, assetGuidOrName, null, cell, worldPos, rotation, scale);
        }

        public static void PutCliffCell(
            RoomData room,
            string assetGuidOrName,
            string sourceGroupId,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            if (room == null) return;
            room.EnsureDefaults();
            PutTileCell(room.cliffCells, assetGuidOrName, sourceGroupId, cell, worldPos, rotation, scale);
        }

        public static void RemoveFloorCell(RoomData room, Vector3Int cell)
        {
            RemoveTileCell(room != null ? room.floorCells : null, cell);
        }

        public static void RemoveCliffCell(RoomData room, Vector3Int cell)
        {
            RemoveTileCell(room != null ? room.cliffCells : null, cell);
        }

        public static void PutProp(
            RoomData room,
            string assetGuidOrName,
            Vector3Int cell,
            Vector3 position,
            float rotation,
            Vector2 scale,
            RoomLayer layer)
        {
            if (room == null)
            {
                return;
            }

            room.EnsureDefaults();
            RemoveProp(room, cell, layer);
            room.propPlacements.Add(new RoomData.PropPlacement
            {
                assetGuidOrName = assetGuidOrName,
                cell = cell,
                position = position,
                rotation = rotation,
                scale = scale == Vector2.zero ? Vector2.one : scale,
                layer = layer
            });
        }

        public static void RemoveProp(RoomData room, Vector3Int cell, RoomLayer layer)
        {
            if (room == null)
            {
                return;
            }

            room.EnsureDefaults();
            for (int i = room.propPlacements.Count - 1; i >= 0; i--)
            {
                if (room.propPlacements[i].cell == cell && room.propPlacements[i].layer == layer)
                {
                    room.propPlacements.RemoveAt(i);
                }
            }
        }

        public static void PutPortal(
            RoomData room,
            string assetGuidOrName,
            Vector3Int cell,
            Vector3 position,
            float rotation,
            Vector2 scale,
            int exitIndex,
            int targetNodeId,
            string roomTypeId)
        {
            if (room == null)
            {
                return;
            }

            room.EnsureDefaults();
            RemovePortal(room, cell);
            room.portalPlacements.Add(new RoomData.PortalPlacement
            {
                assetGuidOrName = assetGuidOrName,
                cell = cell,
                position = position,
                rotation = rotation,
                scale = scale == Vector2.zero ? Vector2.one : scale,
                exitIndex = exitIndex,
                targetNodeId = targetNodeId,
                roomTypeId = roomTypeId
            });
        }

        public static void RemovePortal(RoomData room, Vector3Int cell)
        {
            if (room == null)
            {
                return;
            }

            room.EnsureDefaults();
            for (int i = room.portalPlacements.Count - 1; i >= 0; i--)
            {
                if (room.portalPlacements[i].cell == cell)
                {
                    room.portalPlacements.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Category-aware single-cell place. Routes a <see cref="DesignerCategory"/> to the right
        /// RoomData collection so BOTH authoring surfaces share one entry point and can never
        /// disagree about where a category lands. Walls (Wang) keep their dedicated AppendWallRun path.
        /// </summary>
        public static void PutCategory(
            RoomData room,
            DesignerCategory category,
            string assetGuidOrName,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            PutCategory(room, category, assetGuidOrName, null, cell, worldPos, rotation, scale);
        }

        public static void PutCategory(
            RoomData room,
            DesignerCategory category,
            string assetGuidOrName,
            string sourceGroupId,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            if (room == null)
            {
                return;
            }

            switch (category)
            {
                case DesignerCategory.Floor:
                    PutFloorCell(room, assetGuidOrName, sourceGroupId, cell, worldPos, rotation, scale);
                    break;
                case DesignerCategory.Cliff:
                    PutCliffCell(room, assetGuidOrName, sourceGroupId, cell, worldPos, rotation, scale);
                    break;
                case DesignerCategory.Portal:
                    PutPortal(room, assetGuidOrName, cell, worldPos, rotation, scale, room.portalPlacements.Count, -1, string.Empty);
                    break;
                case DesignerCategory.Object:
                case DesignerCategory.Light:
                default:
                    PutProp(room, assetGuidOrName, cell, worldPos, rotation, scale, DesignerCategoryMap.LayerFor(category));
                    break;
            }
        }

        /// <summary>Category-aware erase, mirror of <see cref="PutCategory"/>.</summary>
        public static void RemoveCategory(RoomData room, DesignerCategory category, Vector3Int cell)
        {
            if (room == null)
            {
                return;
            }

            switch (category)
            {
                case DesignerCategory.Floor:
                    RemoveFloorCell(room, cell);
                    break;
                case DesignerCategory.Cliff:
                    RemoveCliffCell(room, cell);
                    break;
                case DesignerCategory.Portal:
                    RemovePortal(room, cell);
                    break;
                case DesignerCategory.Object:
                case DesignerCategory.Light:
                default:
                    RemoveProp(room, cell, DesignerCategoryMap.LayerFor(category));
                    break;
            }
        }

        public static List<Vector3Int> AppendWallRun(
            RoomData room,
            Vector3Int from,
            Vector3Int to,
            string pieceId,
            Vector2Int footprint)
        {
            return AppendWallRun(room, from, to, pieceId, footprint, SegmentKind.SolidWall, 1f);
        }

        public static List<Vector3Int> AppendWallRun(
            RoomData room,
            Vector3Int from,
            Vector3Int to,
            string pieceId,
            Vector2Int footprint,
            SegmentKind kind,
            float height)
        {
            List<Vector3Int> dirty = new List<Vector3Int>();
            if (room == null)
            {
                return dirty;
            }

            room.EnsureDefaults();
            List<Vector3Int> line = GridLine(from, to);
            if (line.Count == 0)
            {
                return dirty;
            }

            Dictionary<Vector3Int, int> index = BuildWallIndex(room.wallCells);
            bool xDominant = Mathf.Abs(to.x - from.x) >= Mathf.Abs(to.y - from.y);
            int step = Mathf.Max(1, footprint.x);
            int lastPlacedIndex = -1;

            for (int i = 0; i < line.Count; i++)
            {
                if (lastPlacedIndex >= 0)
                {
                    int distance = xDominant
                        ? Mathf.Abs(line[i].x - line[lastPlacedIndex].x)
                        : Mathf.Abs(line[i].y - line[lastPlacedIndex].y);
                    if (distance < step)
                    {
                        continue;
                    }
                }

                Vector3Int cell = line[i];
                WallCell wallCell = new WallCell
                {
                    cell = cell,
                    kind = kind,
                    shape = WangShape.Single,
                    rotation = 0f,
                    pieceId = pieceId,
                    height = height
                };

                if (index.TryGetValue(cell, out int existing))
                {
                    room.wallCells[existing] = wallCell;
                }
                else
                {
                    index[cell] = room.wallCells.Count;
                    room.wallCells.Add(wallCell);
                }

                dirty.Add(cell);
                lastPlacedIndex = i;
            }

            WangRebuild.ReorientWallCells(room, dirty);
            return dirty;
        }

        public static bool RemoveWallCell(RoomData room, Vector3Int cell)
        {
            if (room == null)
            {
                return false;
            }

            room.EnsureDefaults();
            bool removed = false;
            for (int i = room.wallCells.Count - 1; i >= 0; i--)
            {
                if (room.wallCells[i].cell == cell)
                {
                    room.wallCells.RemoveAt(i);
                    removed = true;
                }
            }

            if (removed)
            {
                WangRebuild.ReorientWallCells(room, new[] { cell });
            }

            return removed;
        }

        public static int MigrateSegmentsToCells(RoomData room)
        {
            if (room == null)
            {
                return 0;
            }

            room.EnsureDefaults();
            if (room.wallSegments == null || room.wallSegments.Count == 0)
            {
                return 0;
            }

            int before = room.wallCells.Count;
            for (int i = 0; i < room.wallSegments.Count; i++)
            {
                WallSegment segment = room.wallSegments[i];
                string pieceId = !string.IsNullOrEmpty(segment.piece.pieceId)
                    ? segment.piece.pieceId
                    : !string.IsNullOrEmpty(segment.piece.displayName)
                        ? segment.piece.displayName
                        : segment.piece.sprite != null
                            ? segment.piece.sprite.name
                            : segment.piece.prefab != null
                                ? segment.piece.prefab.name
                                : string.Empty;

                Vector2Int footprint = segment.piece.footprint == Vector2Int.zero
                    ? Vector2Int.one
                    : segment.piece.footprint;

                AppendWallRun(
                    room,
                    segment.fromCell,
                    segment.toCell,
                    pieceId,
                    footprint,
                    segment.kind,
                    segment.height);
            }

            // Cells are now authoritative. Clear the legacy segments so a later write
            // (RoomDataJson.ToDto also calls this) can NOT re-apply stale segments and
            // resurrect walls the user has since erased/edited in F2. Without this, a
            // segment-backed room would overwrite its own cell edits on every save.
            room.wallSegments.Clear();
            return Mathf.Max(0, room.wallCells.Count - before);
        }

        public static List<Vector3Int> GridLine(Vector3Int a, Vector3Int b)
        {
            List<Vector3Int> result = new List<Vector3Int>();

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
                result.Add(new Vector3Int(x0, y0, a.z));
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

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

            return result;
        }

        private static void PutTileCell(
            List<RoomData.TileCellRecord> cells,
            string assetGuidOrName,
            string sourceGroupId,
            Vector3Int cell,
            Vector3 worldPos,
            float rotation,
            Vector2 scale)
        {
            if (cells == null)
            {
                return;
            }

            RemoveTileCell(cells, cell);
            cells.Add(new RoomData.TileCellRecord
            {
                assetGuidOrName = assetGuidOrName,
                sourceGroupId = sourceGroupId,
                cell = cell,
                worldPos = worldPos,
                rotation = rotation,
                scale = scale == Vector2.zero ? Vector2.one : scale
            });
        }

        private static void RemoveTileCell(List<RoomData.TileCellRecord> cells, Vector3Int cell)
        {
            if (cells == null)
            {
                return;
            }

            for (int i = cells.Count - 1; i >= 0; i--)
            {
                if (cells[i].cell == cell)
                {
                    cells.RemoveAt(i);
                }
            }
        }

        private static Dictionary<Vector3Int, int> BuildWallIndex(List<WallCell> wallCells)
        {
            Dictionary<Vector3Int, int> index = new Dictionary<Vector3Int, int>(wallCells.Count);
            for (int i = 0; i < wallCells.Count; i++)
            {
                index[wallCells[i].cell] = i;
            }

            return index;
        }
    }
}
