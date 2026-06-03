using System;
using UnityEngine;

namespace RIMA.RoomPainter
{
    public enum WangShape
    {
        Single,
        End,
        Straight,
        Corner,
        T,
        Cross
    }

    public readonly struct WangResult
    {
        public readonly WangShape shape;
        public readonly float rotationDegrees;
        public readonly int neighborMask;

        public WangResult(WangShape shape, float rotationDegrees, int neighborMask)
        {
            this.shape = shape;
            this.rotationDegrees = rotationDegrees;
            this.neighborMask = neighborMask;
        }
    }

    public static class WangResolver
    {
        private const int North = 1;
        private const int East = 2;
        private const int South = 4;
        private const int West = 8;

        public static WangResult Resolve4(Vector3Int cell, Func<Vector3Int, bool> isOccupied)
        {
            int mask = 0;
            if (IsOccupied(isOccupied, cell + Vector3Int.up)) mask |= North;
            if (IsOccupied(isOccupied, cell + Vector3Int.right)) mask |= East;
            if (IsOccupied(isOccupied, cell + Vector3Int.down)) mask |= South;
            if (IsOccupied(isOccupied, cell + Vector3Int.left)) mask |= West;

            // Rotation convention: CCW, matching Unity's Quaternion.Euler(0,0,+theta)
            // (applied verbatim in WallRunBuilder.PlaceOne). Canonical (0deg) art:
            //   End connects South (cap points North) · Corner connects South+East ·
            //   T opens North (connects W+E+S) · Straight connects East+West.
            // Each non-anchor mask is the canonical rotated CCW until its connection
            // set / open side matches. Do NOT author a CW table here: Unity renders
            // CCW, so a CW table draws every End/Corner/T mirrored or 90deg off.
            switch (mask)
            {
                case 0:
                    return new WangResult(WangShape.Single, 0f, mask);
                // End: canonical connects South@0. CCW: S->0, E->90, N->180, W->270.
                case North:
                    return new WangResult(WangShape.End, 180f, mask);
                case East:
                    return new WangResult(WangShape.End, 90f, mask);
                case South:
                    return new WangResult(WangShape.End, 0f, mask);
                case West:
                    return new WangResult(WangShape.End, 270f, mask);
                // Straight: flip-symmetric, handedness-agnostic.
                case East | West:
                    return new WangResult(WangShape.Straight, 0f, mask);
                case North | South:
                    return new WangResult(WangShape.Straight, 90f, mask);
                // Corner: canonical connects South+East@0. CCW: S+E->0, N+E->90, N+W->180, S+W->270.
                case South | East:
                    return new WangResult(WangShape.Corner, 0f, mask);
                case North | East:
                    return new WangResult(WangShape.Corner, 90f, mask);
                case North | West:
                    return new WangResult(WangShape.Corner, 180f, mask);
                case South | West:
                    return new WangResult(WangShape.Corner, 270f, mask);
                // T: canonical opens North (mask W+E+S)@0. CCW by open side: openN->0, openW->90, openS->180, openE->270.
                case West | East | South:               // mask 14, open N
                    return new WangResult(WangShape.T, 0f, mask);
                case North | East | South:               // mask 7, open W
                    return new WangResult(WangShape.T, 90f, mask);
                case North | East | West:                // mask 11, open S
                    return new WangResult(WangShape.T, 180f, mask);
                case North | South | West:                // mask 13, open E
                    return new WangResult(WangShape.T, 270f, mask);
                case North | East | South | West:
                    return new WangResult(WangShape.Cross, 0f, mask);
                default:
                    return new WangResult(WangShape.Single, 0f, mask);
            }
        }

        public static int EdgeMask8(Vector3Int cell, Func<Vector3Int, bool> isOccupied)
        {
            int mask = 0;
            if (IsOccupied(isOccupied, cell + Vector3Int.up)) mask |= 1;
            if (IsOccupied(isOccupied, cell + new Vector3Int(1, 1, 0))) mask |= 2;
            if (IsOccupied(isOccupied, cell + Vector3Int.right)) mask |= 4;
            if (IsOccupied(isOccupied, cell + new Vector3Int(1, -1, 0))) mask |= 8;
            if (IsOccupied(isOccupied, cell + Vector3Int.down)) mask |= 16;
            if (IsOccupied(isOccupied, cell + new Vector3Int(-1, -1, 0))) mask |= 32;
            if (IsOccupied(isOccupied, cell + Vector3Int.left)) mask |= 64;
            if (IsOccupied(isOccupied, cell + new Vector3Int(-1, 1, 0))) mask |= 128;
            return mask;
        }

        private static bool IsOccupied(Func<Vector3Int, bool> isOccupied, Vector3Int cell)
        {
            return isOccupied != null && isOccupied(cell);
        }
    }
}
