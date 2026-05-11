using System.Collections.Generic;
using RIMA.Runtime.Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public static class WallAutoConnect
    {
        // Connection type indices (match 8-variant tileset order)
        // 0=straight_H, 1=straight_V, 2=corner_NW, 3=corner_NE, 4=corner_SW, 5=corner_SE, 6=end_L, 7=end_R

        private static readonly Vector3Int[] NeighborOffsets = {
            new Vector3Int(-1, 0, 0),  // W
            new Vector3Int(1, 0, 0),   // E
            new Vector3Int(0, 1, 0),   // N
            new Vector3Int(0, -1, 0),  // S
            new Vector3Int(-1, 1, 0),  // NW
            new Vector3Int(1, 1, 0),   // NE
            new Vector3Int(-1, -1, 0), // SW
            new Vector3Int(1, -1, 0),  // SE
        };

        public static void RefreshNeighborhood(Tilemap wallTilemap, IEnumerable<Vector3Int> touchedCells,
            TileBase[] wallVariants, RoomBlueprint bp = null)
        {
            HashSet<Vector3Int> toRefresh = new HashSet<Vector3Int>();

            foreach (Vector3Int touchedCell in touchedCells)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        toRefresh.Add(new Vector3Int(touchedCell.x + dx, touchedCell.y + dy, touchedCell.z));
                    }
                }
            }

            foreach (Vector3Int cell in toRefresh)
            {
                RefreshCell(wallTilemap, cell, wallVariants, bp);
            }
        }

        public static void RefreshCell(Tilemap wallTilemap, Vector3Int cell,
            TileBase[] wallVariants, RoomBlueprint bp = null)
        {
            if (wallTilemap.GetTile(cell) == null)
                return;

            if (bp != null)
            {
                int cellIdx = (cell.y - bp.roomOrigin.y) * bp.roomWidth + (cell.x - bp.roomOrigin.x);
                if (bp.overrideVariantIndex != null && cellIdx >= 0 && cellIdx < bp.overrideVariantIndex.Length && bp.overrideVariantIndex[cellIdx])
                    return;
            }

            bool hasW = wallTilemap.GetTile(new Vector3Int(cell.x - 1, cell.y, cell.z)) != null;
            bool hasE = wallTilemap.GetTile(new Vector3Int(cell.x + 1, cell.y, cell.z)) != null;
            bool hasN = wallTilemap.GetTile(new Vector3Int(cell.x, cell.y + 1, cell.z)) != null;
            bool hasS = wallTilemap.GetTile(new Vector3Int(cell.x, cell.y - 1, cell.z)) != null;

            int mask = (hasW ? 1 : 0) | (hasE ? 2 : 0) | (hasN ? 4 : 0) | (hasS ? 8 : 0);

            int connType;
            switch (mask)
            {
                case 10: connType = 0; break;
                case 5:  connType = 1; break;
                case 12: connType = 2; break;
                case 6:  connType = 3; break;
                case 9:  connType = 4; break;
                case 3:  connType = 5; break;
                case 4:  connType = 6; break;
                case 2:  connType = 6; break;
                case 8:  connType = 7; break;
                case 1:  connType = 7; break;
                default: connType = 0; break;
            }

            TileBase selected = GetVariantTile(wallVariants, connType);
            wallTilemap.SetTile(cell, selected);
        }

        public static bool BakeWallVariants(Tilemap wallTilemap, RoomBlueprint bp, TileBase[] wallVariants)
        {
            if (wallTilemap == null || bp == null || wallVariants == null)
                return false;

            BoundsInt bounds = wallTilemap.cellBounds;

            if (bp.wallVariantIndex == null || bp.wallVariantIndex.Length != bp.roomWidth * bp.roomHeight)
                bp.wallVariantIndex = new byte[bp.roomWidth * bp.roomHeight];

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);

                    if (wallTilemap.GetTile(cell) == null)
                        continue;

                    bool hasW = wallTilemap.GetTile(new Vector3Int(x - 1, y, 0)) != null;
                    bool hasE = wallTilemap.GetTile(new Vector3Int(x + 1, y, 0)) != null;
                    bool hasN = wallTilemap.GetTile(new Vector3Int(x, y + 1, 0)) != null;
                    bool hasS = wallTilemap.GetTile(new Vector3Int(x, y - 1, 0)) != null;

                    int mask = (hasW ? 1 : 0) | (hasE ? 2 : 0) | (hasN ? 4 : 0) | (hasS ? 8 : 0);

                    int connType;
                    switch (mask)
                    {
                        case 10: connType = 0; break;
                        case 5:  connType = 1; break;
                        case 12: connType = 2; break;
                        case 6:  connType = 3; break;
                        case 9:  connType = 4; break;
                        case 3:  connType = 5; break;
                        case 4:  connType = 6; break;
                        case 2:  connType = 6; break;
                        case 8:  connType = 7; break;
                        case 1:  connType = 7; break;
                        default: connType = 0; break;
                    }

                    int arrIdx = (y - bp.roomOrigin.y) * bp.roomWidth + (x - bp.roomOrigin.x);
                    if (arrIdx >= 0 && arrIdx < bp.wallVariantIndex.Length)
                        bp.wallVariantIndex[arrIdx] = (byte)connType;

                    TileBase selected = GetVariantTile(wallVariants, connType);
                    wallTilemap.SetTile(cell, selected);
                }
            }

            wallTilemap.RefreshAllTiles();
            return true;
        }

        private static TileBase GetVariantTile(TileBase[] wallVariants, int connType)
        {
            if (wallVariants != null && connType < wallVariants.Length)
                return wallVariants[connType];

            if (wallVariants != null)
            {
                foreach (TileBase t in wallVariants)
                {
                    if (t != null)
                        return t;
                }
            }

            return null;
        }
    }
}
