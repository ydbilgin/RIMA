using System.Collections.Generic;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public static class WangRebuild
    {
        private static readonly Vector3Int[] FourNeighbors =
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };

        public static void ReorientWallCells(RoomData room, IEnumerable<Vector3Int> dirtyCells)
        {
            if (room == null)
            {
                return;
            }

            room.EnsureDefaults();
            if (room.wallCells.Count == 0)
            {
                return;
            }

            Dictionary<Vector3Int, int> index = BuildIndex(room.wallCells);
            HashSet<Vector3Int> targets = new HashSet<Vector3Int>();
            if (dirtyCells != null)
            {
                foreach (Vector3Int dirty in dirtyCells)
                {
                    targets.Add(dirty);
                    for (int i = 0; i < FourNeighbors.Length; i++)
                    {
                        targets.Add(dirty + FourNeighbors[i]);
                    }
                }
            }

            foreach (Vector3Int target in targets)
            {
                if (!index.TryGetValue(target, out int cellIndex))
                {
                    continue;
                }

                WallCell wallCell = room.wallCells[cellIndex];
                WangResult result = WangResolver.Resolve4(target, candidate => index.ContainsKey(candidate));
                wallCell.shape = result.shape;
                wallCell.rotation = result.rotationDegrees;
                room.wallCells[cellIndex] = wallCell;
            }
        }

        private static Dictionary<Vector3Int, int> BuildIndex(List<WallCell> wallCells)
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
