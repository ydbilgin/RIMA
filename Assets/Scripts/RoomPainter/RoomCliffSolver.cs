using System.Collections.Generic;
using UnityEngine;

namespace RIMA.RoomPainter
{
    /// <summary>
    /// Pure, runtime-safe cliff solver: given the set of floor cells, returns the cells that
    /// should carry a cliff (the camera-facing drop edge of a floating island). This is the
    /// LOGICAL cliff-generation the user asked for — it works directly off RoomData.floorCells,
    /// so it needs no scene Tilemap and is fully unit-testable (no Unity object, no AssetDatabase).
    ///
    /// Logic is ported from the proven S114 CliffAutoPlacer.CollectCliffCells algorithm:
    ///   - flood exterior void from the padded bounds (interior pockets excluded),
    ///   - place a cliff on a floor cell only when a camera-facing (S/SE/SW) neighbour is
    ///     exterior void AND the drop opens monotonically south for `southClearCells` steps,
    ///   - cut back-side cells (N/NE/NW exterior void) and narrow protrusions (>=5 void neighbours).
    /// Iso neighbour vectors are the project-canonical ones (verified 2026-05-26).
    ///
    /// CANONICAL SOURCE (S6 2026-06-01): this solver is the single source of truth for
    /// RoomData-driven cliff generation. It intentionally does NOT replicate the legacy
    /// CliffAutoPlacer orphan-cluster filter (ComputeOrphanClusters): a clean authored room
    /// has no isolated 1-2 cell floor islands, so the filter is unnecessary here and the
    /// extra coupling is not worth it. If a future room genuinely needs orphan suppression,
    /// pre-filter floorCells before calling Solve rather than re-adding scene-placer state.
    /// </summary>
    public static class RoomCliffSolver
    {
        private static readonly Vector3Int South = new Vector3Int(-1, -1, 0);
        private static readonly Vector3Int North = new Vector3Int(1, 1, 0);
        private static readonly Vector3Int East = new Vector3Int(1, -1, 0);
        private static readonly Vector3Int West = new Vector3Int(-1, 1, 0);
        private static readonly Vector3Int SouthEast = new Vector3Int(0, -1, 0);
        private static readonly Vector3Int SouthWest = new Vector3Int(-1, 0, 0);
        private static readonly Vector3Int NorthEast = new Vector3Int(1, 0, 0);
        private static readonly Vector3Int NorthWest = new Vector3Int(0, 1, 0);

        /// <summary>Solve cliff cells from a floor-cell set. southClearCells = required open-drop depth.</summary>
        public static HashSet<Vector3Int> Solve(IEnumerable<Vector3Int> floorCellsSource, int southClearCells = 5)
        {
            var floorCells = floorCellsSource as HashSet<Vector3Int> ?? new HashSet<Vector3Int>(floorCellsSource);
            var result = new HashSet<Vector3Int>();
            if (floorCells.Count == 0) return result;

            int depth = Mathf.Max(1, southClearCells);
            HashSet<Vector3Int> exteriorVoid = FloodExteriorVoid(floorCells, depth + 2);

            foreach (Vector3Int cell in floorCells)
            {
                if (!HasCameraFacingExteriorDrop(cell, exteriorVoid, depth)) continue;

                // cut back-side (far-edge) cells
                bool hasNorthVoid = exteriorVoid.Contains(cell + North)
                                    || exteriorVoid.Contains(cell + NorthEast)
                                    || exteriorVoid.Contains(cell + NorthWest);
                if (hasNorthVoid) continue;

                // cut narrow protrusions (5+ of 8 neighbours are exterior void)
                int voidNeighbours = 0;
                if (exteriorVoid.Contains(cell + South)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + SouthEast)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + SouthWest)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + East)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + West)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + North)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + NorthEast)) voidNeighbours++;
                if (exteriorVoid.Contains(cell + NorthWest)) voidNeighbours++;
                if (voidNeighbours >= 5) continue;

                result.Add(cell);
            }

            return result;
        }

        /// <summary>Convenience: read floor cells straight off a RoomData and return cliff cells.</summary>
        public static HashSet<Vector3Int> SolveFromRoom(RoomData room, int southClearCells = 5)
        {
            var floor = new HashSet<Vector3Int>();
            if (room != null && room.floorCells != null)
            {
                for (int i = 0; i < room.floorCells.Count; i++)
                {
                    floor.Add(room.floorCells[i].cell);
                }
            }
            return Solve(floor, southClearCells);
        }

        private static HashSet<Vector3Int> FloodExteriorVoid(HashSet<Vector3Int> floorCells, int pad)
        {
            int xMin = int.MaxValue, xMax = int.MinValue, yMin = int.MaxValue, yMax = int.MinValue;
            foreach (Vector3Int c in floorCells)
            {
                if (c.x < xMin) xMin = c.x;
                if (c.x > xMax) xMax = c.x;
                if (c.y < yMin) yMin = c.y;
                if (c.y > yMax) yMax = c.y;
            }
            xMin -= pad; xMax += pad; yMin -= pad; yMax += pad;

            var exterior = new HashSet<Vector3Int>();
            var queue = new Queue<Vector3Int>();
            System.Action<Vector3Int> enroll = (c) =>
            {
                if (c.x < xMin || c.x > xMax || c.y < yMin || c.y > yMax) return;
                if (floorCells.Contains(c)) return;
                if (exterior.Add(c)) queue.Enqueue(c);
            };

            for (int x = xMin; x <= xMax; x++) { enroll(new Vector3Int(x, yMin, 0)); enroll(new Vector3Int(x, yMax, 0)); }
            for (int y = yMin; y <= yMax; y++) { enroll(new Vector3Int(xMin, y, 0)); enroll(new Vector3Int(xMax, y, 0)); }

            Vector3Int[] dirs = { South, North, East, West, SouthEast, SouthWest };
            while (queue.Count > 0)
            {
                Vector3Int c = queue.Dequeue();
                foreach (Vector3Int d in dirs) enroll(c + d);
            }
            return exterior;
        }

        private static bool HasCameraFacingExteriorDrop(Vector3Int cell, HashSet<Vector3Int> exteriorVoid, int depth)
        {
            Vector3Int[] frontDirs = { South, SouthEast, SouthWest };
            foreach (Vector3Int dir in frontDirs)
            {
                Vector3Int seed = cell + dir;
                if (!exteriorVoid.Contains(seed)) continue;
                if (OpensMonotonicSouth(seed, exteriorVoid, depth)) return true;
            }
            return false;
        }

        private static bool OpensMonotonicSouth(Vector3Int seed, HashSet<Vector3Int> exteriorVoid, int depth)
        {
            var visited = new HashSet<Vector3Int> { seed };
            var queue = new Queue<Vector3Int>();
            var dist = new Dictionary<Vector3Int, int> { { seed, 0 } };
            queue.Enqueue(seed);
            Vector3Int[] openDirs = { South, SouthEast, SouthWest };
            while (queue.Count > 0)
            {
                Vector3Int cur = queue.Dequeue();
                int d = dist[cur];
                if (d >= depth) return true;
                foreach (Vector3Int dir in openDirs)
                {
                    Vector3Int next = cur + dir;
                    if (!exteriorVoid.Contains(next)) continue;
                    if (!visited.Add(next)) continue;
                    dist[next] = d + 1;
                    queue.Enqueue(next);
                }
            }
            return false;
        }
    }
}
