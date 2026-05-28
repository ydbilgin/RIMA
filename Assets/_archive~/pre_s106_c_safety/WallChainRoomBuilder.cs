using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls.V2
{
    /// <summary>
    /// Grid-anchored wall chain construction system.
    /// Input: RoomSpec (cells, shape, edges, alcoves, protrusions, door)
    /// Output: spawned WallPiece instances under roomParent forming closed room.
    /// </summary>
    public class WallChainRoomBuilder : MonoBehaviour
    {
        public WallPieceRegistry registry;
        public Transform roomParent;
        public float cellSize = 1f;

        // ---- Public entry ----

        public void Build(RoomSpec spec)
        {
            if (registry == null) { Debug.LogError("[V2] Registry missing"); return; }
            EnsureParent(spec);
            Clear();

            HashSet<Vector2Int> footprint = ComputeFootprint(spec);
            List<EdgeSegment> rear = ExtractRearEdges(footprint);
            List<EdgeSegment> left = ExtractSideEdges(footprint, side: -1);
            List<EdgeSegment> right = ExtractSideEdges(footprint, side: +1);
            List<EdgeSegment> front = ExtractFrontEdges(footprint);
            EnforceDoorCenter(rear, spec);
            HashSet<Vector2Int> forcedFrontOpen = EnsureFrontMinOpening(front, spec);

            if (spec.rearWallEnabled) BuildRearChain(rear, spec, footprint);
            if (spec.sideWallsEnabled)
            {
                BuildSideChain(left, WallDirection.SideLeft, spec, footprint);
                BuildSideChain(right, WallDirection.SideRight, spec, footprint);
            }
            BuildFrontEdge(front, spec, forcedFrontOpen);
            PlaceCornersAtJunctions(footprint, spec);
            PlaceNicheProtrusionFormulaCorners(spec);
            PlaceCenterReserved(spec);
            PlaceInteriorIslands(spec);
            PlaceWaterPoolZones(spec);
        }

        public void Clear()
        {
            if (roomParent == null) return;
            for (int i = roomParent.childCount - 1; i >= 0; i--)
            {
                var c = roomParent.GetChild(i);
#if UNITY_EDITOR
                if (Application.isPlaying) Destroy(c.gameObject);
                else DestroyImmediate(c.gameObject);
#else
                Destroy(c.gameObject);
#endif
            }
        }

        // ---- Footprint computation ----

        HashSet<Vector2Int> ComputeFootprint(RoomSpec spec)
        {
            var set = new HashSet<Vector2Int>();
            switch (spec.shapeType)
            {
                case RoomShapeType.Rectangle:
                    for (int x = 0; x < spec.widthCells; x++)
                        for (int y = 0; y < spec.heightCells; y++)
                            set.Add(new Vector2Int(x, y));
                    break;
                case RoomShapeType.Diamond:
                    {
                        int mid = spec.heightCells / 2;
                        int top = Mathf.Clamp(spec.diamondTopWidthCells, 1, Mathf.Max(1, spec.widthCells));
                        int max = Mathf.Max(1, spec.widthCells);
                        int rowWidth = top;
                        for (int row = 0; row < spec.heightCells; row++)
                        {
                            int y = spec.heightCells - 1 - row;
                            AddMirroredRow(set, spec.widthCells, y, rowWidth);
                            int step = GetDiamondStep(spec, row);
                            if (row < mid) rowWidth = Mathf.Min(max, rowWidth + step * 2);
                            else rowWidth = Mathf.Max(top, rowWidth - step * 2);
                        }
                        break;
                    }
                case RoomShapeType.Irregular:
                    if (spec.HasCustomWalkable)
                    {
                        foreach (var w in spec.walkableCells) set.Add(w);
                    }
                    else
                    {
                        for (int x = 0; x < spec.widthCells; x++)
                            for (int y = 0; y < spec.heightCells; y++)
                                set.Add(new Vector2Int(x, y));
                    }
                    break;
            }
            // Apply alcoves (cut cells)
            foreach (var a in spec.alcovePositions)
                set.Remove(a);
            // Apply protrusions (add cells)
            foreach (var p in spec.protrusionPositions)
                set.Add(p);
            ApplyNicheSpecs(set, spec);
            ApplyProtrusionSpecs(set, spec);
            return set;
        }

        void AddMirroredRow(HashSet<Vector2Int> set, int widthCells, int y, int requestedWidth)
        {
            int width = Mathf.Clamp(requestedWidth, 1, Mathf.Max(1, widthCells));
            int left = Mathf.Max(0, (widthCells - width) / 2);
            int right = Mathf.Min(widthCells - 1, widthCells - 1 - left);
            for (int x = left; x <= right; x++)
                set.Add(new Vector2Int(x, y));
        }

        int GetDiamondStep(RoomSpec spec, int row)
        {
            int min = Mathf.Max(0, spec.diamondStepMin);
            int max = Mathf.Max(min, spec.diamondStepMax);
            int range = max - min + 1;
            return min + (range <= 1 ? 0 : row % range);
        }

        void ApplyNicheSpecs(HashSet<Vector2Int> footprint, RoomSpec spec)
        {
            if (spec.nicheSpecs == null) return;
            foreach (var niche in spec.nicheSpecs)
                ApplyNicheSpec(footprint, spec, niche);
        }

        void ApplyNicheSpec(HashSet<Vector2Int> footprint, RoomSpec spec, NicheSpec niche)
        {
            int width = Mathf.Max(1, niche.width);
            int depth = Mathf.Max(1, niche.depth);
            string side = NormalizeSide(niche.side);
            CutNiche(footprint, side, niche.anchorRow, width, depth);
            if (niche.mirror)
                CutNiche(footprint, MirrorSide(side), MirrorAnchor(spec, side, niche.anchorRow, width), width, depth);
        }

        void ApplyProtrusionSpecs(HashSet<Vector2Int> footprint, RoomSpec spec)
        {
            if (spec.protrusionSpecs == null) return;
            foreach (var protrusion in spec.protrusionSpecs)
                ApplyProtrusionSpec(footprint, spec, protrusion);
        }

        void ApplyProtrusionSpec(HashSet<Vector2Int> footprint, RoomSpec spec, ProtrusionSpec protrusion)
        {
            int width = Mathf.Max(1, protrusion.width);
            int depth = Mathf.Max(1, protrusion.depth);
            string side = NormalizeSide(protrusion.side);
            AddProtrusion(footprint, side, protrusion.anchorRow, width, depth);
            if (protrusion.mirror)
                AddProtrusion(footprint, MirrorSide(side), MirrorAnchor(spec, side, protrusion.anchorRow, width), width, depth);
        }

        void CutNiche(HashSet<Vector2Int> footprint, string side, int anchor, int width, int depth)
        {
            if (side == "rear")
            {
                for (int x = anchor; x < anchor + width; x++)
                {
                    if (!TryGetColumnBounds(footprint, x, out _, out int maxY)) continue;
                    for (int d = 0; d < depth; d++)
                        footprint.Remove(new Vector2Int(x, maxY - d));
                }
                return;
            }

            for (int y = anchor; y < anchor + width; y++)
            {
                if (!TryGetRowBounds(footprint, y, out int minX, out int maxX)) continue;
                for (int d = 0; d < depth; d++)
                {
                    int x = side == "right" ? maxX - d : minX + d;
                    footprint.Remove(new Vector2Int(x, y));
                }
            }
        }

        void AddProtrusion(HashSet<Vector2Int> footprint, string side, int anchor, int width, int depth)
        {
            if (side == "rear")
            {
                for (int x = anchor; x < anchor + width; x++)
                {
                    if (!TryGetColumnBounds(footprint, x, out _, out int maxY)) continue;
                    for (int d = 1; d <= depth; d++)
                        footprint.Add(new Vector2Int(x, maxY + d));
                }
                return;
            }

            for (int y = anchor; y < anchor + width; y++)
            {
                if (!TryGetRowBounds(footprint, y, out int minX, out int maxX)) continue;
                for (int d = 1; d <= depth; d++)
                {
                    int x = side == "right" ? maxX + d : minX - d;
                    footprint.Add(new Vector2Int(x, y));
                }
            }
        }

        bool TryGetRowBounds(HashSet<Vector2Int> footprint, int y, out int minX, out int maxX)
        {
            minX = int.MaxValue;
            maxX = int.MinValue;
            foreach (var c in footprint)
            {
                if (c.y != y) continue;
                minX = Mathf.Min(minX, c.x);
                maxX = Mathf.Max(maxX, c.x);
            }
            return minX != int.MaxValue;
        }

        bool TryGetColumnBounds(HashSet<Vector2Int> footprint, int x, out int minY, out int maxY)
        {
            minY = int.MaxValue;
            maxY = int.MinValue;
            foreach (var c in footprint)
            {
                if (c.x != x) continue;
                minY = Mathf.Min(minY, c.y);
                maxY = Mathf.Max(maxY, c.y);
            }
            return minY != int.MaxValue;
        }

        string NormalizeSide(string side)
        {
            if (string.IsNullOrEmpty(side)) return "rear";
            side = side.ToLowerInvariant();
            if (side == "left" || side == "right" || side == "rear") return side;
            return "rear";
        }

        string MirrorSide(string side)
        {
            if (side == "left") return "right";
            if (side == "right") return "left";
            return "rear";
        }

        int MirrorAnchor(RoomSpec spec, string side, int anchor, int width)
        {
            if (side == "rear")
                return Mathf.Max(0, spec.widthCells - anchor - width);
            return anchor;
        }

        // ---- Edge extraction ----

        public struct EdgeSegment
        {
            public Vector2Int cell;
            public Vector2Int dir; // outward normal (0,1)=north, (0,-1)=south, (1,0)=east, (-1,0)=west
        }

        List<EdgeSegment> ExtractRearEdges(HashSet<Vector2Int> footprint)
        {
            var list = new List<EdgeSegment>();
            foreach (var c in footprint)
                if (!footprint.Contains(new Vector2Int(c.x, c.y + 1)))
                    list.Add(new EdgeSegment { cell = c, dir = new Vector2Int(0, 1) });
            list.Sort((a, b) => a.cell.x.CompareTo(b.cell.x));
            return list;
        }

        List<EdgeSegment> ExtractSideEdges(HashSet<Vector2Int> footprint, int side)
        {
            var list = new List<EdgeSegment>();
            Vector2Int normal = new Vector2Int(side, 0);
            foreach (var c in footprint)
                if (!footprint.Contains(new Vector2Int(c.x + side, c.y)))
                    list.Add(new EdgeSegment { cell = c, dir = normal });
            list.Sort((a, b) => a.cell.y.CompareTo(b.cell.y));
            return list;
        }

        List<EdgeSegment> ExtractFrontEdges(HashSet<Vector2Int> footprint)
        {
            var list = new List<EdgeSegment>();
            foreach (var c in footprint)
                if (!footprint.Contains(new Vector2Int(c.x, c.y - 1)))
                    list.Add(new EdgeSegment { cell = c, dir = new Vector2Int(0, -1) });
            list.Sort((a, b) => a.cell.x.CompareTo(b.cell.x));
            return list;
        }

        // ---- Chain builders ----

        void BuildRearChain(List<EdgeSegment> rear, RoomSpec spec, HashSet<Vector2Int> footprint)
        {
            // Group consecutive segments by Y row
            var byRow = GroupConsecutive(rear, axis: 'x');
            foreach (var run in byRow)
            {
                int startX = run[0].cell.x;
                int endX = run[run.Count - 1].cell.x;
                int y = run[0].cell.y;
                int worldY = y + 1; // rear wall sits at row above
                bool startIsCorner = IsOuterCornerCell(run[0].cell, footprint);
                bool endIsCorner = IsOuterCornerCell(run[run.Count - 1].cell, footprint);
                FillRunWithSpans(startX, endX, worldY, WallDirection.Rear, spec, horizontal: true, startIsCorner, endIsCorner);
            }
        }

        void BuildSideChain(List<EdgeSegment> side, WallDirection dir, RoomSpec spec, HashSet<Vector2Int> footprint)
        {
            var byCol = GroupConsecutive(side, axis: 'y');
            foreach (var run in byCol)
            {
                int startY = run[0].cell.y;
                int endY = run[run.Count - 1].cell.y;
                int x = run[0].cell.x + (dir == WallDirection.SideRight ? 1 : 0);
                bool startIsCorner = IsOuterCornerCell(run[0].cell, footprint);
                bool endIsCorner = IsOuterCornerCell(run[run.Count - 1].cell, footprint);
                FillRunWithSpans(startY, endY, x, dir, spec, horizontal: false, startIsCorner, endIsCorner);
            }
        }

        void BuildFrontEdge(List<EdgeSegment> front, RoomSpec spec, HashSet<Vector2Int> forcedOpen)
        {
            var byRow = GroupConsecutive(front, axis: 'x');
            foreach (var run in byRow)
            {
                int startX = run[0].cell.x;
                int endX = run[run.Count - 1].cell.x;
                int y = run[0].cell.y;
                int worldY = y; // front edge at same row, below
                switch (spec.frontEdgeMode)
                {
                    case FrontEdgeMode.LowWall:
                        FillFrontWithLow(startX, endX, worldY, forcedOpen);
                        break;
                    case FrontEdgeMode.Open:
                        FillFrontOpen(startX, endX, worldY);
                        break;
                    case FrontEdgeMode.Broken:
                        FillFrontBroken(startX, endX, worldY, forcedOpen);
                        break;
                }
            }
        }

        void EnforceDoorCenter(List<EdgeSegment> rear, RoomSpec spec)
        {
            if (!spec.enforceCenteredRearDoor || !spec.HasDoor || rear.Count == 0) return;
            var byRow = GroupConsecutive(rear, axis: 'x');
            List<EdgeSegment> widest = null;
            foreach (var run in byRow)
            {
                if (widest == null || run.Count > widest.Count)
                    widest = run;
            }
            if (widest == null || widest.Count < 2) return;

            int startX = widest[0].cell.x;
            int endX = widest[widest.Count - 1].cell.x;
            int doorX = Mathf.Clamp(startX + (widest.Count - 2) / 2, startX, endX - 1);
            spec.doorPosition = new Vector2Int(doorX, widest[0].cell.y);

            if (registry.GetByType(WallPieceType.DoorArch, WallDirection.Rear) == null)
                Debug.LogWarning("[V2] DoorArch/Rear missing; centered rear door may render as gap");
        }

        HashSet<Vector2Int> EnsureFrontMinOpening(List<EdgeSegment> front, RoomSpec spec)
        {
            var forcedOpen = new HashSet<Vector2Int>();
            if (spec.frontEdgeMode == FrontEdgeMode.Open || front.Count == 0) return forcedOpen;

            int minOpening = Mathf.Max(0, spec.frontMinOpeningCells);
            if (minOpening == 0) return forcedOpen;

            var byRow = GroupConsecutive(front, axis: 'x');
            foreach (var run in byRow)
            {
                int count = run.Count;
                int opening = Mathf.Min(count, minOpening);
                int startIndex = Mathf.Max(0, (count - opening) / 2);
                for (int i = startIndex; i < startIndex + opening; i++)
                    forcedOpen.Add(run[i].cell);
            }

            return forcedOpen;
        }

        // ---- Fill helpers ----

        void FillRunWithSpans(int start, int end, int fixedCoord, WallDirection dir, RoomSpec spec, bool horizontal, bool startIsCorner, bool endIsCorner)
        {
            int length = end - start + 1;
            if (length == 1)
            {
                if (startIsCorner || endIsCorner) return;

                bool flipX;
                var single = GetSpanForLength(dir, 1, out flipX);
                if (single != null)
                {
                    SpawnPieceFromData(single, GetCellWorld(start, fixedCoord, horizontal), flipX);
                    return;
                }

                SpawnPiece(WallPieceType.Connector, WallDirection.Any, GetCellWorld(start, fixedCoord, horizontal));
                return;
            }

            if (length == 2)
            {
                bool flipX;
                var single = GetSpanForLength(dir, 1, out flipX);
                if (single != null)
                {
                    SpawnPieceFromData(single, GetCellWorld(start, fixedCoord, horizontal), flipX);
                    SpawnPieceFromData(single, GetCellWorld(start + 1, fixedCoord, horizontal), flipX);
                }
                return;
            }

            int cursor = start;
            int connectorIndex = 0;
            int lastConnectorCursor = start;
            int nextConnectorSpacing = GetConnectorSpacing(spec, start, fixedCoord, dir, connectorIndex);
            // Connector at start
            if (!startIsCorner)
                SpawnPiece(WallPieceType.Connector, WallDirection.Any, GetCellWorld(cursor, fixedCoord, horizontal));
            cursor += 1;
            int spanLength = length - 2; // reserve 1 cell at each end for connector
            if (spanLength < 0) spanLength = 0;

            // Door insertion (only rear chain supported)
            int doorAt = -1;
            if (spec != null && dir == WallDirection.Rear && spec.HasDoor && spec.doorPosition.y == fixedCoord - 1)
                doorAt = spec.doorPosition.x;

            while (spanLength > 0)
            {
                if (doorAt >= 0 && cursor == doorAt)
                {
                    SpawnPiece(WallPieceType.DoorArch, dir, GetCellWorld(cursor, fixedCoord, horizontal));
                    lastConnectorCursor = cursor;
                    cursor += 2; spanLength -= 2;
                    continue;
                }

                if (spanLength > 1 && cursor - lastConnectorCursor >= nextConnectorSpacing)
                {
                    SpawnPiece(WallPieceType.Connector, WallDirection.Any, GetCellWorld(cursor, fixedCoord, horizontal));
                    lastConnectorCursor = cursor;
                    connectorIndex++;
                    nextConnectorSpacing = GetConnectorSpacing(spec, cursor, fixedCoord, dir, connectorIndex);
                    cursor += 1; spanLength -= 1;
                    continue;
                }

                int available = spanLength;
                if (doorAt >= 0 && cursor < doorAt)
                    available = Mathf.Min(available, doorAt - cursor);
                if (available <= 0) break;

                bool flipX;
                var span = GetSpanForLength(dir, available, out flipX);
                if (span == null) break;
                SpawnPieceFromData(span, GetCellWorld(cursor, fixedCoord, horizontal), flipX);
                cursor += span.lengthInCells;
                spanLength -= span.lengthInCells;
            }

            // Connector at end
            if (!endIsCorner)
                SpawnPiece(WallPieceType.Connector, WallDirection.Any, GetCellWorld(end, fixedCoord, horizontal));
        }

        void FillFrontWithLow(int startX, int endX, int y, HashSet<Vector2Int> forcedOpen)
        {
            int cursor = startX;
            int remaining = endX - startX + 1;
            while (remaining > 0)
            {
                if (IsForcedFrontOpen(cursor, y, forcedOpen))
                {
                    SpawnPiece(WallPieceType.OpenGap, WallDirection.Front, GetCellWorld(cursor, y, horizontal: true));
                    cursor += 1;
                    remaining -= 1;
                    continue;
                }

                int available = GetAvailableFrontSpan(cursor, y, remaining, forcedOpen);
                var d = GetLowFrontForLength(available);
                if (d == null) break;
                SpawnPieceFromData(d, GetCellWorld(cursor, y, horizontal: true));
                int used = Mathf.Max(1, d.lengthInCells);
                cursor += used;
                remaining -= used;
            }
        }

        int GetAvailableFrontSpan(int cursor, int y, int remaining, HashSet<Vector2Int> forcedOpen)
        {
            int available = 0;
            while (available < remaining && !IsForcedFrontOpen(cursor + available, y, forcedOpen))
                available++;
            return Mathf.Max(1, available);
        }

        WallPieceData GetLowFrontForLength(int cellsRemaining)
        {
            WallPieceData best = null;
            int bestLen = 0;
            foreach (var p in registry.pieces)
            {
                if (p == null || p.type != WallPieceType.LowFront) continue;
                int len = Mathf.Max(1, p.lengthInCells);
                if (len <= cellsRemaining && len > bestLen)
                {
                    best = p;
                    bestLen = len;
                }
            }
            return best;
        }

        void FillFrontOpen(int startX, int endX, int y)
        {
            for (int x = startX; x <= endX; x++)
                SpawnPiece(WallPieceType.OpenGap, WallDirection.Front, GetCellWorld(x, y, horizontal: true));
        }

        void FillFrontBroken(int startX, int endX, int y, HashSet<Vector2Int> forcedOpen)
        {
            for (int x = startX; x <= endX; x++)
            {
                if (IsForcedFrontOpen(x, y, forcedOpen))
                {
                    SpawnPiece(WallPieceType.OpenGap, WallDirection.Front, GetCellWorld(x, y, horizontal: true));
                    continue;
                }

                bool low = ((x - startX) % 2 == 0);
                SpawnPiece(low ? WallPieceType.LowFront : WallPieceType.OpenGap, WallDirection.Front,
                    GetCellWorld(x, y, horizontal: true));
            }
        }

        bool IsForcedFrontOpen(int x, int y, HashSet<Vector2Int> forcedOpen)
        {
            return forcedOpen != null && forcedOpen.Contains(new Vector2Int(x, y));
        }

        int GetConnectorSpacing(RoomSpec spec, int cursor, int fixedCoord, WallDirection dir, int index)
        {
            int min = spec != null ? Mathf.Max(1, spec.connectorSpacingMin) : 4;
            int max = spec != null ? Mathf.Max(min, spec.connectorSpacingMax) : 7;
            int range = max - min + 1;
            if (range <= 1) return min;
            int seed = Mathf.Abs(cursor * 31 + fixedCoord * 17 + ((int)dir + 3) * 13 + index * 7);
            return min + seed % range;
        }

        void PlaceCornersAtJunctions(HashSet<Vector2Int> footprint, RoomSpec spec)
        {
            // Detect corner cells (convex = outer corner, concave = inner corner)
            foreach (var c in footprint)
            {
                bool n = footprint.Contains(new Vector2Int(c.x, c.y + 1));
                bool s = footprint.Contains(new Vector2Int(c.x, c.y - 1));
                bool e = footprint.Contains(new Vector2Int(c.x + 1, c.y));
                bool w = footprint.Contains(new Vector2Int(c.x - 1, c.y));
                if (ShouldPlaceOuterCorner(n, s, e, w))
                {
                    SpawnPiece(WallPieceType.OuterCorner, GetCornerDirection(n, s, e, w),
                        GetCellWorld(c.x, c.y, horizontal: true));
                }
            }

            // Inner corners on protrusions
            foreach (var p in spec.protrusionPositions)
            {
                SpawnPiece(WallPieceType.InnerCorner, GetCornerDirectionAt(p, footprint),
                    GetCellWorld(p.x, p.y, horizontal: true));
            }
            // Inner corners around alcoves
            foreach (var a in spec.alcovePositions)
            {
                SpawnPiece(WallPieceType.InnerCorner, GetCornerDirectionAt(a, footprint),
                    GetCellWorld(a.x, a.y, horizontal: true));
            }
        }

        bool IsOuterCornerCell(Vector2Int c, HashSet<Vector2Int> footprint)
        {
            bool n = footprint.Contains(new Vector2Int(c.x, c.y + 1));
            bool s = footprint.Contains(new Vector2Int(c.x, c.y - 1));
            bool e = footprint.Contains(new Vector2Int(c.x + 1, c.y));
            bool w = footprint.Contains(new Vector2Int(c.x - 1, c.y));
            return ShouldPlaceOuterCorner(n, s, e, w);
        }

        bool ShouldPlaceOuterCorner(bool n, bool s, bool e, bool w)
        {
            int openSides = (n ? 0 : 1) + (s ? 0 : 1) + (e ? 0 : 1) + (w ? 0 : 1);
            bool isStraightHorizontalCorridor = (!n && !s) && (e && w);
            bool isStraightVerticalCorridor = (!e && !w) && (n && s);
            return openSides >= 2 && !isStraightHorizontalCorridor && !isStraightVerticalCorridor;
        }

        WallDirection GetCornerDirectionAt(Vector2Int c, HashSet<Vector2Int> footprint)
        {
            bool n = footprint.Contains(new Vector2Int(c.x, c.y + 1));
            bool s = footprint.Contains(new Vector2Int(c.x, c.y - 1));
            bool e = footprint.Contains(new Vector2Int(c.x + 1, c.y));
            bool w = footprint.Contains(new Vector2Int(c.x - 1, c.y));
            return GetCornerDirection(n, s, e, w);
        }

        WallDirection GetCornerDirection(bool n, bool s, bool e, bool w)
        {
            if (!n && !e) return WallDirection.Rear;
            if (!n && !w) return WallDirection.Rear;
            if (!s && !e) return WallDirection.Front;
            if (!s && !w) return WallDirection.Front;
            if (!e) return WallDirection.SideRight;
            if (!w) return WallDirection.SideLeft;
            return WallDirection.Any;
        }

        void PlaceNicheProtrusionFormulaCorners(RoomSpec spec)
        {
            if (spec.nicheSpecs != null)
            {
                foreach (var niche in spec.nicheSpecs)
                {
                    string side = NormalizeSide(niche.side);
                    int width = Mathf.Max(1, niche.width);
                    int depth = Mathf.Max(1, niche.depth);
                    PlaceFormulaCorners(spec, side, niche.anchorRow, width, depth, WallPieceType.InnerCorner);
                    if (niche.mirror)
                        PlaceFormulaCorners(spec, MirrorSide(side), MirrorAnchor(spec, side, niche.anchorRow, width), width, depth, WallPieceType.InnerCorner);
                }
            }

            if (spec.protrusionSpecs != null)
            {
                foreach (var protrusion in spec.protrusionSpecs)
                {
                    string side = NormalizeSide(protrusion.side);
                    int width = Mathf.Max(1, protrusion.width);
                    int depth = Mathf.Max(1, protrusion.depth);
                    PlaceFormulaCorners(spec, side, protrusion.anchorRow, width, depth, WallPieceType.OuterCorner);
                    if (protrusion.mirror)
                        PlaceFormulaCorners(spec, MirrorSide(side), MirrorAnchor(spec, side, protrusion.anchorRow, width), width, depth, WallPieceType.OuterCorner);
                }
            }
        }

        void PlaceFormulaCorners(RoomSpec spec, string side, int anchor, int width, int depth, WallPieceType cornerType)
        {
            bool protrusion = cornerType == WallPieceType.OuterCorner;
            if (side == "rear")
            {
                int y = protrusion ? spec.heightCells + depth : spec.heightCells - depth;
                SpawnPiece(cornerType, WallDirection.Rear, GetCellWorld(anchor, y, horizontal: true));
                SpawnPiece(cornerType, WallDirection.Rear, GetCellWorld(anchor + width - 1, y, horizontal: true));
                return;
            }

            int x = side == "right"
                ? (protrusion ? spec.widthCells + depth : spec.widthCells - depth)
                : (protrusion ? -depth : depth);
            WallDirection dir = side == "right" ? WallDirection.SideRight : WallDirection.SideLeft;
            SpawnPiece(cornerType, dir, GetCellWorld(anchor, x, horizontal: false));
            SpawnPiece(cornerType, dir, GetCellWorld(anchor + width - 1, x, horizontal: false));
        }

        void PlaceCenterReserved(RoomSpec spec)
        {
            if (spec.reservedCenterRadiusCells <= 0) return;

            var go = CreateMarkerObject("ReservedCenter");
            go.transform.localPosition = new Vector3(spec.widthCells * 0.5f * cellSize, spec.heightCells * 0.5f * cellSize, -0.05f);
            var line = ConfigureLine(go, new Color(1f, 0.85f, 0.2f, 0.9f), loop: true);
            int segments = 32;
            line.positionCount = segments;
            float radius = spec.reservedCenterRadiusCells * cellSize;
            for (int i = 0; i < segments; i++)
            {
                float angle = i / (float)segments * Mathf.PI * 2f;
                line.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
            }
        }

        void PlaceInteriorIslands(RoomSpec spec)
        {
            if (spec.interiorIslandRects == null) return;
            for (int i = 0; i < spec.interiorIslandRects.Count; i++)
            {
                var rect = spec.interiorIslandRects[i];
                var go = CreateMarkerObject("Island_" + i);
                DrawRectMarker(go, rect, new Color(1f, 0.65f, 0.2f, 0.9f));
            }
        }

        void PlaceWaterPoolZones(RoomSpec spec)
        {
            if (spec.waterPoolRects == null) return;
            for (int i = 0; i < spec.waterPoolRects.Count; i++)
            {
                var rect = spec.waterPoolRects[i];
                var go = CreateMarkerObject("Water_" + i);
                go.transform.localPosition = new Vector3((rect.x + rect.width * 0.5f) * cellSize, (rect.y + rect.height * 0.5f) * cellSize, 0.05f);
                go.transform.localScale = new Vector3(rect.width * cellSize, rect.height * cellSize, 1f);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
                sr.color = new Color(0f, 0.9f, 1f, 0.35f);
                sr.sortingOrder = -20;
            }
        }

        GameObject CreateMarkerObject(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(roomParent, false);
            return go;
        }

        void DrawRectMarker(GameObject go, RectInt rect, Color color)
        {
            go.transform.localPosition = new Vector3((rect.x + rect.width * 0.5f) * cellSize, (rect.y + rect.height * 0.5f) * cellSize, -0.05f);
            var line = ConfigureLine(go, color, loop: true);
            float halfW = rect.width * cellSize * 0.5f;
            float halfH = rect.height * cellSize * 0.5f;
            line.positionCount = 4;
            line.SetPosition(0, new Vector3(-halfW, -halfH, 0f));
            line.SetPosition(1, new Vector3(-halfW, halfH, 0f));
            line.SetPosition(2, new Vector3(halfW, halfH, 0f));
            line.SetPosition(3, new Vector3(halfW, -halfH, 0f));
        }

        LineRenderer ConfigureLine(GameObject go, Color color, bool loop)
        {
            var line = go.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.loop = loop;
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.startColor = color;
            line.endColor = color;
            var material = new Material(Shader.Find("Sprites/Default"));
            material.color = color;
            line.sharedMaterial = material;
            return line;
        }

        // ---- Helpers ----

        Vector3 GetCellWorld(int a, int b, bool horizontal)
        {
            return horizontal
                ? new Vector3(a * cellSize, b * cellSize, 0)
                : new Vector3(b * cellSize, a * cellSize, 0);
        }

        void SpawnPiece(WallPieceType type, WallDirection dir, Vector3 worldPos)
        {
            bool flipX;
            var data = GetByType(type, dir, out flipX);
            if (data == null) { Debug.LogWarning("[V2] No data for " + type + "/" + dir); return; }
            SpawnPieceFromData(data, worldPos, flipX);
        }

        WallPieceData GetByType(WallPieceType type, WallDirection dir, out bool flipX)
        {
            flipX = false;
            var data = registry.GetByType(type, dir);
            if (data == null && dir == WallDirection.SideRight)
            {
                data = registry.GetByType(type, WallDirection.SideLeft);
                flipX = data != null;
            }
            return data;
        }

        WallPieceData GetSpanForLength(WallDirection dir, int length, out bool flipX)
        {
            flipX = false;
            var data = registry.GetSpanForLength(dir, length);
            if (data == null && dir == WallDirection.SideRight)
            {
                data = registry.GetSpanForLength(WallDirection.SideLeft, length);
                flipX = data != null;
            }
            return data;
        }

        void SpawnPieceFromData(WallPieceData data, Vector3 worldPos, bool flipX = false)
        {
            if (data.prefab == null) { Debug.LogWarning("[V2] No prefab for " + data.id); return; }
            GameObject go;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(data.prefab, roomParent);
            else
                go = Instantiate(data.prefab, roomParent);
#else
            go = Instantiate(data.prefab, roomParent);
#endif
            go.transform.localPosition = worldPos;
            if (flipX)
            {
                Vector3 scale = go.transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                go.transform.localScale = scale;
            }
            var piece = go.GetComponent<WallPiece>();
            if (piece != null) piece.Initialize(data);
        }

        List<List<EdgeSegment>> GroupConsecutive(List<EdgeSegment> edges, char axis)
        {
            var groups = new List<List<EdgeSegment>>();
            if (edges.Count == 0) return groups;
            var current = new List<EdgeSegment> { edges[0] };
            for (int i = 1; i < edges.Count; i++)
            {
                bool consecutive;
                if (axis == 'x')
                    consecutive = edges[i].cell.y == current[current.Count - 1].cell.y
                        && edges[i].cell.x == current[current.Count - 1].cell.x + 1;
                else
                    consecutive = edges[i].cell.x == current[current.Count - 1].cell.x
                        && edges[i].cell.y == current[current.Count - 1].cell.y + 1;
                if (consecutive) current.Add(edges[i]);
                else { groups.Add(current); current = new List<EdgeSegment> { edges[i] }; }
            }
            groups.Add(current);
            return groups;
        }

        void EnsureParent(RoomSpec spec)
        {
            if (roomParent != null) return;
            var go = new GameObject("Room_" + spec.roomName);
            go.transform.SetParent(transform, false);
            roomParent = go.transform;
        }

        void OnDrawGizmos()
        {
            if (roomParent == null) return;
            Gizmos.color = Color.white;
            foreach (Transform t in roomParent)
                Gizmos.DrawLine(t.position, t.position + Vector3.up * 0.1f);
        }
    }
}
