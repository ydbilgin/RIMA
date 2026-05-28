using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RIMA.Walls.V2;

namespace RIMA.Walls.V2.EditorTools
{
    public static class PainterValidator
    {
        private const string RegistryPath = "Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset";

        public enum Severity { Info, Warning, Error }

        public struct Issue
        {
            public Severity sev;
            public string code;
            public string message;
            public Vector2Int? cell;

            public Issue(Severity sev, string code, string message, Vector2Int? cell = null)
            {
                this.sev = sev;
                this.code = code;
                this.message = message;
                this.cell = cell;
            }
        }

        public static List<Issue> Validate(RoomSpec spec, HashSet<Vector2Int> walkable, Vector2Int? door,
            List<RectInt> water, List<RectInt> islands, List<RoomSocket> sockets)
        {
            var issues = new List<Issue>();
            var footprint = walkable != null ? new HashSet<Vector2Int>(walkable) : new HashSet<Vector2Int>();

            if (footprint.Count == 0)
                issues.Add(new Issue(Severity.Warning, "W103", "Empty paint: draw at least one walkable cell before generating."));

            if (footprint.Count > 0 && !door.HasValue && !HasFrontOpening(spec))
                issues.Add(new Issue(Severity.Error, "E001", "Player trapped: no painted door and no front opening."));

            if (door.HasValue && !DoorTouchesRearOrFrontEdge(footprint, door.Value))
                issues.Add(new Issue(Severity.Error, "E003", "Door cell is not on a rear or front wall edge.", door.Value));

            foreach (var rect in water ?? new List<RectInt>())
            {
                foreach (var cell in CellsIn(rect))
                {
                    if (footprint.Contains(cell))
                    {
                        issues.Add(new Issue(Severity.Error, "E004", "Water overlaps walkable footprint.", cell));
                        break;
                    }
                }
            }

            foreach (var cell in footprint)
            {
                if (CountWalkableNeighbors(footprint, cell) == 0)
                    issues.Add(new Issue(Severity.Error, "E005", "Orphan walkable cell has no walkable neighbors.", cell));
            }

            var components = GetComponents(footprint, null);
            if (components.Count >= 2)
                issues.Add(new Issue(Severity.Warning, "W101", "Walkable footprint has disconnected regions.", components[1][0]));

            foreach (var cell in footprint)
            {
                if (!IsOneCellBottleneck(footprint, cell)) continue;
                issues.Add(new Issue(Severity.Error, "E002", "One-cell bottleneck splits combat space into larger areas.", cell));
                break;
            }

            CheckCornerRegistry(issues, spec, footprint);

            if (footprint.Count > 0)
            {
                GetBounds(footprint, out int minX, out int minY, out int maxX, out int maxY);
                if (maxX - minX + 1 < 6 || maxY - minY + 1 < 6)
                    issues.Add(new Issue(Severity.Info, "I201", "Small footprint: consider at least 6x6 for combat."));
            }

            return issues;
        }

        private static bool HasFrontOpening(RoomSpec spec)
        {
            if (spec == null) return false;
            return spec.frontEdgeMode == FrontEdgeMode.Open || spec.frontMinOpeningCells > 0;
        }

        private static bool DoorTouchesRearOrFrontEdge(HashSet<Vector2Int> footprint, Vector2Int door)
        {
            if (!footprint.Contains(door)) return false;
            return !footprint.Contains(new Vector2Int(door.x, door.y + 1))
                || !footprint.Contains(new Vector2Int(door.x, door.y - 1));
        }

        private static IEnumerable<Vector2Int> CellsIn(RectInt rect)
        {
            for (int x = rect.xMin; x < rect.xMax; x++)
                for (int y = rect.yMin; y < rect.yMax; y++)
                    yield return new Vector2Int(x, y);
        }

        private static int CountWalkableNeighbors(HashSet<Vector2Int> footprint, Vector2Int cell)
        {
            int count = 0;
            foreach (var n in Neighbors(cell))
                if (footprint.Contains(n)) count++;
            return count;
        }

        private static bool IsOneCellBottleneck(HashSet<Vector2Int> footprint, Vector2Int cell)
        {
            bool left = footprint.Contains(new Vector2Int(cell.x - 1, cell.y));
            bool right = footprint.Contains(new Vector2Int(cell.x + 1, cell.y));
            bool up = footprint.Contains(new Vector2Int(cell.x, cell.y + 1));
            bool down = footprint.Contains(new Vector2Int(cell.x, cell.y - 1));
            bool corridor = (left && right && !up && !down) || (up && down && !left && !right);
            if (!corridor) return false;

            var comps = GetComponents(footprint, cell);
            if (comps.Count < 2) return false;
            comps.Sort((a, b) => b.Count.CompareTo(a.Count));
            return comps[0].Count >= 4 && comps[1].Count >= 4;
        }

        private static List<List<Vector2Int>> GetComponents(HashSet<Vector2Int> footprint, Vector2Int? blocked)
        {
            var result = new List<List<Vector2Int>>();
            var seen = new HashSet<Vector2Int>();
            foreach (var start in footprint)
            {
                if (blocked.HasValue && start == blocked.Value) continue;
                if (seen.Contains(start)) continue;

                var comp = new List<Vector2Int>();
                var q = new Queue<Vector2Int>();
                q.Enqueue(start);
                seen.Add(start);
                while (q.Count > 0)
                {
                    var c = q.Dequeue();
                    comp.Add(c);
                    foreach (var n in Neighbors(c))
                    {
                        if (blocked.HasValue && n == blocked.Value) continue;
                        if (!footprint.Contains(n) || !seen.Add(n)) continue;
                        q.Enqueue(n);
                    }
                }
                result.Add(comp);
            }
            result.Sort((a, b) => b.Count.CompareTo(a.Count));
            return result;
        }

        private static IEnumerable<Vector2Int> Neighbors(Vector2Int c)
        {
            yield return new Vector2Int(c.x + 1, c.y);
            yield return new Vector2Int(c.x - 1, c.y);
            yield return new Vector2Int(c.x, c.y + 1);
            yield return new Vector2Int(c.x, c.y - 1);
        }

        private static void CheckCornerRegistry(List<Issue> issues, RoomSpec spec, HashSet<Vector2Int> footprint)
        {
            var registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
            if (registry == null)
            {
                issues.Add(new Issue(Severity.Warning, "W102", "Wall registry missing; corner seam coverage cannot be verified."));
                return;
            }

            var seen = new HashSet<string>();
            foreach (var piece in WallChainPredictor.PredictPieces(spec, footprint))
            {
                if (piece.type != WallPieceType.OuterCorner && piece.type != WallPieceType.InnerCorner) continue;
                if (FindPiece(registry, piece.type, piece.dir) != null) continue;

                string key = piece.type + "/" + piece.dir + "/" + piece.cell;
                if (!seen.Add(key)) continue;
                issues.Add(new Issue(Severity.Warning, "W102", "Predicted corner has no matching registry piece.", piece.cell));
            }
        }

        private static WallPieceData FindPiece(WallPieceRegistry registry, WallPieceType type, WallDirection dir)
        {
            var data = registry.GetByType(type, dir);
            if (data == null && dir == WallDirection.SideRight)
                data = registry.GetByType(type, WallDirection.SideLeft);
            return data;
        }

        private static void GetBounds(HashSet<Vector2Int> cells, out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = int.MinValue;
            maxY = int.MinValue;
            foreach (var c in cells)
            {
                minX = Mathf.Min(minX, c.x);
                minY = Mathf.Min(minY, c.y);
                maxX = Mathf.Max(maxX, c.x);
                maxY = Mathf.Max(maxY, c.y);
            }
        }
    }

    public struct PreviewPiece
    {
        public Vector2Int cell;
        public WallPieceType type;
        public WallDirection dir;
        public Vector2 colliderSize;
        public int lengthInCells;
    }

    public static class WallChainPredictor
    {
        private const string RegistryPath = "Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset";

        private struct EdgeSegment
        {
            public Vector2Int cell;
        }

        public static List<PreviewPiece> PredictPieces(RoomSpec spec, HashSet<Vector2Int> footprint)
        {
            var result = new List<PreviewPiece>();
            if (spec == null || footprint == null || footprint.Count == 0) return result;

            var rear = ExtractRearEdges(footprint);
            var left = ExtractSideEdges(footprint, -1);
            var right = ExtractSideEdges(footprint, 1);
            var front = ExtractFrontEdges(footprint);
            Vector2Int door = GetPredictedDoor(spec, rear);
            var forcedFrontOpen = EnsureFrontMinOpening(front, spec);

            if (spec.rearWallEnabled) BuildRear(result, rear, spec, footprint, door);
            if (spec.sideWallsEnabled)
            {
                BuildSide(result, left, WallDirection.SideLeft, spec, footprint);
                BuildSide(result, right, WallDirection.SideRight, spec, footprint);
            }
            BuildFront(result, front, spec, forcedFrontOpen);
            AddCorners(result, footprint, spec);
            return result;
        }

        private static List<EdgeSegment> ExtractRearEdges(HashSet<Vector2Int> footprint)
        {
            var list = new List<EdgeSegment>();
            foreach (var c in footprint)
                if (!footprint.Contains(new Vector2Int(c.x, c.y + 1)))
                    list.Add(new EdgeSegment { cell = c });
            list.Sort((a, b) =>
            {
                int yc = a.cell.y.CompareTo(b.cell.y);
                return yc != 0 ? yc : a.cell.x.CompareTo(b.cell.x);
            });
            return list;
        }

        private static List<EdgeSegment> ExtractSideEdges(HashSet<Vector2Int> footprint, int side)
        {
            var list = new List<EdgeSegment>();
            foreach (var c in footprint)
                if (!footprint.Contains(new Vector2Int(c.x + side, c.y)))
                    list.Add(new EdgeSegment { cell = c });
            list.Sort((a, b) =>
            {
                int xc = a.cell.x.CompareTo(b.cell.x);
                return xc != 0 ? xc : a.cell.y.CompareTo(b.cell.y);
            });
            return list;
        }

        private static List<EdgeSegment> ExtractFrontEdges(HashSet<Vector2Int> footprint)
        {
            var list = new List<EdgeSegment>();
            foreach (var c in footprint)
                if (!footprint.Contains(new Vector2Int(c.x, c.y - 1)))
                    list.Add(new EdgeSegment { cell = c });
            list.Sort((a, b) =>
            {
                int yc = a.cell.y.CompareTo(b.cell.y);
                return yc != 0 ? yc : a.cell.x.CompareTo(b.cell.x);
            });
            return list;
        }

        private static void BuildRear(List<PreviewPiece> result, List<EdgeSegment> rear, RoomSpec spec,
            HashSet<Vector2Int> footprint, Vector2Int door)
        {
            foreach (var run in GroupConsecutive(rear, 'x'))
            {
                int startX = run[0].cell.x;
                int endX = run[run.Count - 1].cell.x;
                int y = run[0].cell.y;
                AddRun(result, startX, endX, y + 1, WallDirection.Rear, spec, true,
                    IsOuterCornerCell(run[0].cell, footprint),
                    IsOuterCornerCell(run[run.Count - 1].cell, footprint), door);
            }
        }

        private static void BuildSide(List<PreviewPiece> result, List<EdgeSegment> side, WallDirection dir,
            RoomSpec spec, HashSet<Vector2Int> footprint)
        {
            foreach (var run in GroupConsecutive(side, 'y'))
            {
                int startY = run[0].cell.y;
                int endY = run[run.Count - 1].cell.y;
                int x = run[0].cell.x + (dir == WallDirection.SideRight ? 1 : 0);
                AddRun(result, startY, endY, x, dir, spec, false,
                    IsOuterCornerCell(run[0].cell, footprint),
                    IsOuterCornerCell(run[run.Count - 1].cell, footprint), new Vector2Int(-1, -1));
            }
        }

        private static void AddRun(List<PreviewPiece> result, int start, int end, int fixedCoord,
            WallDirection dir, RoomSpec spec, bool horizontal, bool startIsCorner, bool endIsCorner, Vector2Int door)
        {
            int length = end - start + 1;
            if (length == 1)
            {
                if (!startIsCorner && !endIsCorner)
                    AddPiece(result, GetSpanType(dir), dir, ToCell(start, fixedCoord, horizontal), 1);
                return;
            }

            if (length == 2)
            {
                if (!startIsCorner) AddPiece(result, GetSpanType(dir), dir, ToCell(start, fixedCoord, horizontal), 1);
                if (!endIsCorner) AddPiece(result, GetSpanType(dir), dir, ToCell(start + 1, fixedCoord, horizontal), 1);
                return;
            }

            int cursor = start;
            int spanLength = length - 2;
            if (!startIsCorner)
                AddPiece(result, WallPieceType.Connector, WallDirection.Any, ToCell(cursor, fixedCoord, horizontal), 1);
            cursor++;

            int doorAt = dir == WallDirection.Rear && door.x >= 0 && door.y == fixedCoord - 1 ? door.x : -1;
            while (spanLength > 0)
            {
                if (doorAt >= 0 && cursor == doorAt)
                {
                    AddPiece(result, WallPieceType.DoorArch, dir, ToCell(cursor, fixedCoord, horizontal), 2);
                    cursor += 2;
                    spanLength -= 2;
                    continue;
                }

                int available = spanLength;
                if (doorAt >= 0 && cursor < doorAt)
                    available = Mathf.Min(available, doorAt - cursor);
                int len = Mathf.Clamp(GetBestSpanLength(dir, available), 1, available);
                AddPiece(result, GetSpanType(dir), dir, ToCell(cursor, fixedCoord, horizontal), len);
                cursor += len;
                spanLength -= len;
            }

            if (!endIsCorner)
                AddPiece(result, WallPieceType.Connector, WallDirection.Any, ToCell(end, fixedCoord, horizontal), 1);
        }

        private static void BuildFront(List<PreviewPiece> result, List<EdgeSegment> front, RoomSpec spec, HashSet<Vector2Int> forcedOpen)
        {
            foreach (var run in GroupConsecutive(front, 'x'))
            {
                int startX = run[0].cell.x;
                int endX = run[run.Count - 1].cell.x;
                int y = run[0].cell.y;
                for (int x = startX; x <= endX; x++)
                {
                    bool open = spec.frontEdgeMode == FrontEdgeMode.Open
                        || forcedOpen.Contains(new Vector2Int(x, y))
                        || (spec.frontEdgeMode == FrontEdgeMode.Broken && ((x - startX) % 2 != 0));
                    AddPiece(result, open ? WallPieceType.OpenGap : WallPieceType.LowFront, WallDirection.Front,
                        new Vector2Int(x, y), 1);
                }
            }
        }

        private static void AddCorners(List<PreviewPiece> result, HashSet<Vector2Int> footprint, RoomSpec spec)
        {
            foreach (var c in footprint)
            {
                bool n = footprint.Contains(new Vector2Int(c.x, c.y + 1));
                bool s = footprint.Contains(new Vector2Int(c.x, c.y - 1));
                bool e = footprint.Contains(new Vector2Int(c.x + 1, c.y));
                bool w = footprint.Contains(new Vector2Int(c.x - 1, c.y));
                if (ShouldPlaceOuterCorner(n, s, e, w))
                    AddPiece(result, WallPieceType.OuterCorner, GetCornerDirection(n, s, e, w), c, 1);
            }

            foreach (var p in spec.protrusionPositions ?? new List<Vector2Int>())
                AddPiece(result, WallPieceType.InnerCorner, GetCornerDirectionAt(p, footprint), p, 1);
            foreach (var a in spec.alcovePositions ?? new List<Vector2Int>())
                AddPiece(result, WallPieceType.InnerCorner, GetCornerDirectionAt(a, footprint), a, 1);
        }

        private static Vector2Int GetPredictedDoor(RoomSpec spec, List<EdgeSegment> rear)
        {
            if (!spec.HasDoor) return new Vector2Int(-1, -1);
            if (!spec.enforceCenteredRearDoor || rear.Count == 0) return spec.doorPosition;

            List<EdgeSegment> widest = null;
            foreach (var run in GroupConsecutive(rear, 'x'))
                if (widest == null || run.Count > widest.Count)
                    widest = run;
            if (widest == null || widest.Count < 2) return spec.doorPosition;

            int startX = widest[0].cell.x;
            int endX = widest[widest.Count - 1].cell.x;
            int doorX = Mathf.Clamp(startX + (widest.Count - 2) / 2, startX, endX - 1);
            return new Vector2Int(doorX, widest[0].cell.y);
        }

        private static HashSet<Vector2Int> EnsureFrontMinOpening(List<EdgeSegment> front, RoomSpec spec)
        {
            var forcedOpen = new HashSet<Vector2Int>();
            if (spec.frontEdgeMode == FrontEdgeMode.Open || front.Count == 0) return forcedOpen;

            int minOpening = Mathf.Max(0, spec.frontMinOpeningCells);
            if (minOpening == 0) return forcedOpen;

            foreach (var run in GroupConsecutive(front, 'x'))
            {
                int opening = Mathf.Min(run.Count, minOpening);
                int startIndex = Mathf.Max(0, (run.Count - opening) / 2);
                for (int i = startIndex; i < startIndex + opening; i++)
                    forcedOpen.Add(run[i].cell);
            }
            return forcedOpen;
        }

        private static List<List<EdgeSegment>> GroupConsecutive(List<EdgeSegment> edges, char axis)
        {
            var groups = new List<List<EdgeSegment>>();
            if (edges.Count == 0) return groups;
            var current = new List<EdgeSegment> { edges[0] };
            for (int i = 1; i < edges.Count; i++)
            {
                bool consecutive = axis == 'x'
                    ? edges[i].cell.y == current[current.Count - 1].cell.y && edges[i].cell.x == current[current.Count - 1].cell.x + 1
                    : edges[i].cell.x == current[current.Count - 1].cell.x && edges[i].cell.y == current[current.Count - 1].cell.y + 1;
                if (consecutive) current.Add(edges[i]);
                else { groups.Add(current); current = new List<EdgeSegment> { edges[i] }; }
            }
            groups.Add(current);
            return groups;
        }

        private static Vector2Int ToCell(int a, int b, bool horizontal)
        {
            return horizontal ? new Vector2Int(a, b) : new Vector2Int(b, a);
        }

        private static WallPieceType GetSpanType(WallDirection dir)
        {
            return dir == WallDirection.Rear ? WallPieceType.RearWall : WallPieceType.SideWall;
        }

        private static int GetBestSpanLength(WallDirection dir, int available)
        {
            var registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
            var data = registry != null ? registry.GetSpanForLength(dir, available) : null;
            if (data == null && dir == WallDirection.SideRight && registry != null)
                data = registry.GetSpanForLength(WallDirection.SideLeft, available);
            return data != null ? Mathf.Max(1, data.lengthInCells) : Mathf.Min(3, available);
        }

        private static void AddPiece(List<PreviewPiece> result, WallPieceType type, WallDirection dir, Vector2Int cell, int length)
        {
            result.Add(new PreviewPiece
            {
                cell = cell,
                type = type,
                dir = dir,
                lengthInCells = Mathf.Max(1, length),
                colliderSize = GetColliderSize(type, dir, Mathf.Max(1, length))
            });
        }

        private static Vector2 GetColliderSize(WallPieceType type, WallDirection dir, int length)
        {
            var registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
            var data = registry != null ? registry.GetByType(type, dir) : null;
            if (data == null && dir == WallDirection.SideRight && registry != null)
                data = registry.GetByType(type, WallDirection.SideLeft);
            if (data != null) return data.colliderSize;
            if (type == WallPieceType.OpenGap || type == WallPieceType.DoorArch) return Vector2.zero;
            return dir == WallDirection.SideLeft || dir == WallDirection.SideRight
                ? new Vector2(1, length)
                : new Vector2(length, 1);
        }

        private static bool IsOuterCornerCell(Vector2Int c, HashSet<Vector2Int> footprint)
        {
            bool n = footprint.Contains(new Vector2Int(c.x, c.y + 1));
            bool s = footprint.Contains(new Vector2Int(c.x, c.y - 1));
            bool e = footprint.Contains(new Vector2Int(c.x + 1, c.y));
            bool w = footprint.Contains(new Vector2Int(c.x - 1, c.y));
            return ShouldPlaceOuterCorner(n, s, e, w);
        }

        private static bool ShouldPlaceOuterCorner(bool n, bool s, bool e, bool w)
        {
            int openSides = (n ? 0 : 1) + (s ? 0 : 1) + (e ? 0 : 1) + (w ? 0 : 1);
            bool straightH = !n && !s && e && w;
            bool straightV = !e && !w && n && s;
            return openSides >= 2 && !straightH && !straightV;
        }

        private static WallDirection GetCornerDirectionAt(Vector2Int c, HashSet<Vector2Int> footprint)
        {
            bool n = footprint.Contains(new Vector2Int(c.x, c.y + 1));
            bool s = footprint.Contains(new Vector2Int(c.x, c.y - 1));
            bool e = footprint.Contains(new Vector2Int(c.x + 1, c.y));
            bool w = footprint.Contains(new Vector2Int(c.x - 1, c.y));
            return GetCornerDirection(n, s, e, w);
        }

        private static WallDirection GetCornerDirection(bool n, bool s, bool e, bool w)
        {
            if (!n && !e) return WallDirection.Rear;
            if (!n && !w) return WallDirection.Rear;
            if (!s && !e) return WallDirection.Front;
            if (!s && !w) return WallDirection.Front;
            if (!e) return WallDirection.SideRight;
            if (!w) return WallDirection.SideLeft;
            return WallDirection.Any;
        }
    }
}
