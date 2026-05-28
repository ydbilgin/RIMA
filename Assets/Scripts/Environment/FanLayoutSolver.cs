using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Walkability-constrained fan layout solver.
    /// Hard rule: a portal is NEVER placed on a non-walkable cell.
    /// Strategy order: default fan -> anchor shift -> spacing compress -> 30 deg down rotation -> reduce count.
    /// </summary>
    public static class FanLayoutSolver
    {
        public struct Result
        {
            public Vector3[] positions;
            public int finalCount;
            public bool blocked;            // anchor itself non-walkable -> caller must abort
            public string adjustmentNote;   // human-readable note for logging
        }

        // Spacing tiers (Adım B). Index 0 = default.
        private static readonly float[] SpacingTiers = { 1.2f, 1.0f, 0.8f };
        private const int MaxAnchorShiftIterations = 4;     // Adım A
        private const float AnchorShiftStep = 0.5f;         // 4 * 0.5 = 2 unit max
        private const float RotationDegrees = 30f;          // Adım C (downward bias)

        public static Result Solve(
            Vector3 anchor,
            Vector2 fanDir,
            int requestedCount,
            WalkabilityMap walkMap,
            Grid grid)
        {
            var result = new Result { finalCount = 0, blocked = true, adjustmentNote = "uninitialized" };

            if (walkMap == null || grid == null)
            {
                result.adjustmentNote = "walkMap or grid null";
                return result;
            }

            // Normalize fan direction; bail to right if degenerate.
            Vector2 dir = fanDir.sqrMagnitude > 0.0001f ? fanDir.normalized : Vector2.right;
            int count = Mathf.Clamp(requestedCount, 1, 3);

            // 1) Validate anchor itself.
            if (!IsWalkableAt(anchor, walkMap, grid))
            {
                result.adjustmentNote = "anchor not walkable";
                result.positions = new Vector3[0];
                return result;
            }
            result.blocked = false;

            // 2) Default fan.
            if (TryFan(anchor, dir, count, SpacingTiers[0], 0f, walkMap, grid, out var positions))
            {
                result.positions = positions;
                result.finalCount = count;
                result.adjustmentNote = "default";
                return result;
            }

            // 3) Adım A: anchor shift along counter-direction of first failing portal.
            for (int currentCount = count; currentCount >= 1; currentCount--)
            {
                // Try each combination of [anchor shift x spacing tier x optional rotation].
                for (int spacingIdx = 0; spacingIdx < SpacingTiers.Length; spacingIdx++)
                {
                    float spacing = SpacingTiers[spacingIdx];

                    // No rotation pass.
                    if (TrySolveWithShifts(anchor, dir, currentCount, spacing, 0f, walkMap, grid, out var pos1, out string shiftNote1))
                    {
                        result.positions = pos1;
                        result.finalCount = currentCount;
                        result.adjustmentNote = BuildNote(currentCount, spacing, 0f, shiftNote1, count);
                        return result;
                    }

                    // Rotated pass (Adım C): -30 deg around Z so fan tilts downward in world.
                    if (TrySolveWithShifts(anchor, RotateDeg(dir, -RotationDegrees), currentCount, spacing, -RotationDegrees, walkMap, grid, out var pos2, out string shiftNote2))
                    {
                        result.positions = pos2;
                        result.finalCount = currentCount;
                        result.adjustmentNote = BuildNote(currentCount, spacing, -RotationDegrees, shiftNote2, count);
                        return result;
                    }
                }
            }

            // 4) Last resort: anchor alone (we already validated it).
            result.positions = new[] { anchor };
            result.finalCount = 1;
            result.adjustmentNote = $"fallback anchor-only (requested {count})";
            return result;
        }

        // Attempts Adım A: shift anchor in counter-direction of the failing portal up to MaxAnchorShiftIterations.
        private static bool TrySolveWithShifts(
            Vector3 anchor,
            Vector2 dir,
            int count,
            float spacing,
            float appliedRotation,
            WalkabilityMap walkMap,
            Grid grid,
            out Vector3[] outPositions,
            out string shiftNote)
        {
            outPositions = null;
            shiftNote = "no-shift";

            // First: try at the original anchor.
            if (TryFan(anchor, dir, count, spacing, appliedRotation, walkMap, grid, out outPositions))
            {
                return true;
            }

            // Determine shift direction from the first failing portal at iteration 0.
            Vector3 currentAnchor = anchor;
            for (int iter = 1; iter <= MaxAnchorShiftIterations; iter++)
            {
                Vector2 shiftDir = ComputeShiftDirection(currentAnchor, dir, count, spacing, walkMap, grid);
                if (shiftDir.sqrMagnitude < 0.0001f) return false;

                currentAnchor += (Vector3)(shiftDir * AnchorShiftStep);

                // Shifted anchor must itself stay walkable.
                if (!IsWalkableAt(currentAnchor, walkMap, grid)) return false;

                if (TryFan(currentAnchor, dir, count, spacing, appliedRotation, walkMap, grid, out outPositions))
                {
                    shiftNote = $"shift x{iter}";
                    return true;
                }
            }

            return false;
        }

        // Looks at default fan positions; returns the counter-direction of the first failing slot.
        private static Vector2 ComputeShiftDirection(
            Vector3 anchor,
            Vector2 dir,
            int count,
            float spacing,
            WalkabilityMap walkMap,
            Grid grid)
        {
            var offsets = BuildOffsets(count, spacing);
            for (int i = 0; i < offsets.Length; i++)
            {
                Vector3 p = anchor + (Vector3)(dir * offsets[i]);
                if (!IsWalkableAt(p, walkMap, grid))
                {
                    // Shift opposite of the failing offset's sign so we move away from the bad side.
                    float sign = Mathf.Sign(offsets[i]);
                    if (Mathf.Approximately(sign, 0f)) sign = 1f;
                    return -dir * sign;
                }
            }
            return Vector2.zero;
        }

        private static bool TryFan(
            Vector3 anchor,
            Vector2 dir,
            int count,
            float spacing,
            float appliedRotation,
            WalkabilityMap walkMap,
            Grid grid,
            out Vector3[] positions)
        {
            var offsets = BuildOffsets(count, spacing);
            positions = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                positions[i] = anchor + (Vector3)(dir * offsets[i]);
                if (!IsWalkableAt(positions[i], walkMap, grid))
                {
                    positions = null;
                    return false;
                }
            }
            return true;
        }

        // Symmetric offsets along the fan axis.
        // 1 -> [0]
        // 2 -> [-0.5*s, +0.5*s]
        // 3 -> [-s, 0, +s]
        private static float[] BuildOffsets(int count, float spacing)
        {
            switch (count)
            {
                case 1: return new[] { 0f };
                case 2: return new[] { -0.5f * spacing, 0.5f * spacing };
                default: return new[] { -spacing, 0f, spacing };
            }
        }

        private static bool IsWalkableAt(Vector3 worldPos, WalkabilityMap walkMap, Grid grid)
        {
            Vector3Int cell = grid.WorldToCell(worldPos);
            return walkMap.IsWalkable(cell);
        }

        private static Vector2 RotateDeg(Vector2 v, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float c = Mathf.Cos(rad);
            float s = Mathf.Sin(rad);
            return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
        }

        private static string BuildNote(int finalCount, float spacing, float rotation, string shiftNote, int requested)
        {
            string countNote = finalCount < requested ? $"reduced {requested}->{finalCount}" : $"count {finalCount}";
            string spacingNote = $"spacing {spacing:0.0}";
            string rotNote = Mathf.Approximately(rotation, 0f) ? "no-rot" : $"rot {rotation:0}";
            return $"{countNote}, {spacingNote}, {rotNote}, {shiftNote}";
        }
    }
}
