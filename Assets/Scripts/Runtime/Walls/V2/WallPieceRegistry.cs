using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls.V2
{
    [CreateAssetMenu(fileName = "WallPieceRegistry", menuName = "RIMA/Walls V2/Wall Piece Registry")]
    public class WallPieceRegistry : ScriptableObject
    {
        public List<WallPieceData> pieces = new List<WallPieceData>();

        public WallPieceData GetById(string id)
        {
            foreach (var p in pieces)
                if (p != null && p.id == id) return p;
            return null;
        }

        public WallPieceData GetByType(WallPieceType type, WallDirection dir = WallDirection.Any, bool preferReal = false)
        {
            WallPieceData exact = null;
            WallPieceData any = null;
            WallPieceData fallback = null;
            foreach (var p in pieces)
            {
                if (p == null || p.type != type) continue;
                bool exactDirection = dir == WallDirection.Any || p.direction == dir;
                bool anyDirection = p.direction == WallDirection.Any;
                if (exactDirection || anyDirection)
                {
                    if (preferReal && IsRealPiece(p) && exactDirection) return p;
                    if (exactDirection && exact == null) exact = p;
                    if (anyDirection && any == null) any = p;
                }
                if (fallback == null) fallback = p;
            }
            return exact ?? any ?? fallback;
        }

        public WallPieceData GetPiece(WallPieceType type, WallDirection dir, int length, bool preferReal = true)
        {
            WallPieceData exact = null;
            WallPieceData any = null;
            WallPieceData fallback = null;
            foreach (var p in pieces)
            {
                if (p == null || p.type != type) continue;
                if (length > 0 && p.lengthInCells != length) continue;
                bool exactDirection = dir == WallDirection.Any || p.direction == dir;
                bool anyDirection = p.direction == WallDirection.Any;
                if (!exactDirection && !anyDirection) continue;
                if (preferReal && IsRealPiece(p) && exactDirection) return p;
                if (exactDirection && exact == null) exact = p;
                if (anyDirection && any == null) any = p;
                if (fallback == null) fallback = p;
            }
            return exact ?? any ?? fallback;
        }

        public WallPieceData GetSpanForLength(WallDirection dir, int cellsRemaining, bool preferReal = false)
        {
            WallPieceType spanType = (dir == WallDirection.Rear) ? WallPieceType.RearWall : WallPieceType.SideWall;
            WallPieceData realBest = null;
            int realBestLen = 0;
            WallPieceData best = null;
            int bestLen = 0;
            foreach (var p in pieces)
            {
                if (p == null || p.type != spanType) continue;
                bool exactDirection = dir == WallDirection.Any || p.direction == dir;
                bool anyDirection = p.direction == WallDirection.Any;
                if (!exactDirection && !anyDirection) continue;
                if (preferReal && IsRealPiece(p) && p.lengthInCells <= cellsRemaining && p.lengthInCells > realBestLen)
                {
                    if (!exactDirection) continue;
                    realBest = p;
                    realBestLen = p.lengthInCells;
                }
                if (p.lengthInCells <= cellsRemaining && p.lengthInCells > bestLen)
                {
                    best = p;
                    bestLen = p.lengthInCells;
                }
            }
            return realBest ?? best;
        }

        private static bool IsRealPiece(WallPieceData p)
        {
            return p != null && !string.IsNullOrEmpty(p.id) && p.id.EndsWith("_real", System.StringComparison.Ordinal);
        }
    }
}
