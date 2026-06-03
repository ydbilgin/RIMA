using UnityEngine;

namespace RIMA.RoomPainter
{
    /// <summary>
    /// Floor same-layer auto-merge (iso diamond Wang). Keyed off <see cref="WangResolver.EdgeMask8"/>
    /// (8-neighbour) but only the 4 CARDINAL edges matter for blending: a present same-layer floor
    /// neighbour means that diamond edge is INTERIOR (blend), absent means it is a capped rim.
    /// Floor uses sprite-swap ONLY (16 pre-resolved silhouettes), never rotation.
    /// Bit order here is local-compact N=1,E=2,S=4,W=8 -> 0..15 key into <see cref="FloorTileName"/>.
    /// </summary>
    public static class FloorWangResolver
    {
        // EdgeMask8 cardinal bits: N=1, E=4, S=16, W=64. Collapse to a 0..15 local key.
        public static int ResolveFloorTile(Vector3Int cell, System.Func<Vector3Int, bool> sameLayerOccupied)
        {
            int m = WangResolver.EdgeMask8(cell, sameLayerOccupied);
            bool n = (m & 1) != 0, e = (m & 4) != 0, s = (m & 16) != 0, w = (m & 64) != 0;
            return (n ? 1 : 0) | (e ? 2 : 0) | (s ? 4 : 0) | (w ? 8 : 0); // 0..15
        }

        // key bit order: N=1,E=2,S=4,W=8. Index -> base sprite suffix.
        public static readonly string[] FloorTileName = new string[16]
        {
            "isolated", // 0  none
            "end_N",    // 1  N
            "end_E",    // 2  E
            "cor_NE",   // 3  N+E
            "end_S",    // 4  S
            "str_NS",   // 5  N+S
            "cor_SE",   // 6  E+S
            "T_openW",  // 7  N+E+S (capped W)
            "end_W",    // 8  W
            "cor_NW",   // 9  N+W
            "str_EW",   // 10 E+W
            "T_openS",  // 11 N+E+W (capped S)
            "cor_SW",   // 12 S+W
            "T_openE",  // 13 N+S+W (capped E)
            "T_openN",  // 14 E+S+W (capped N)
            "fill_a"    // 15 N+E+S+W interior (variant picked by cell hash)
        };

        /// <summary>Full sprite asset name for a resolved key, with stable interior variant pick.</summary>
        public static string FloorAssetName(Vector3Int cell, int key)
        {
            string name = FloorTileName[key];
            if (key == 15) // interior fill: pick a/b/c deterministically by cell hash (stable, non-repeating)
            {
                int h = ((cell.x * 73856093) ^ (cell.y * 19349663)) % 3;
                if (h < 0) h += 3;
                name = h == 0 ? "fill_a" : h == 1 ? "fill_b" : "fill_c";
            }
            return "floor_iso_" + name;
        }
    }
}
