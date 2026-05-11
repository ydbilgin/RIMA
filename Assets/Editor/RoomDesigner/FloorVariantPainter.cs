using RIMA.Runtime.Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public static class FloorVariantPainter
    {
        private const float AccentThreshold = 0.65f;
        private const float HeroThreshold = 0.88f;
        private const float WarpFreq = 0.05f;
        private const float WarpStrength = 4.0f;
        private const float ZoneFreq = 0.05f;

        public static bool BakeVariants(Tilemap floorTilemap, RoomBlueprint bp, TileBase[] variantSet)
        {
            if (variantSet == null || variantSet.Length == 0)
                return false;

            BoundsInt bounds = floorTilemap.cellBounds;

            if (bp.floorVariantIndex == null || bp.floorVariantIndex.Length != bp.roomWidth * bp.roomHeight)
                bp.floorVariantIndex = new byte[bp.roomWidth * bp.roomHeight];

            int seed = bp.noiseSeed;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);

                    if (floorTilemap.GetTile(cell) == null)
                        continue;

                    float seedOffset = seed * 0.1f;
                    float wx = Mathf.PerlinNoise((x + 5.2f + seedOffset) * WarpFreq, (y + 1.3f + seedOffset) * WarpFreq) * WarpStrength;
                    float wy = Mathf.PerlinNoise((x + 9.2f + seedOffset) * WarpFreq, (y + 2.8f + seedOffset) * WarpFreq) * WarpStrength;
                    float zone = Mathf.PerlinNoise((x + wx) * ZoneFreq, (y + wy) * ZoneFreq);

                    int tier;
                    if (zone < AccentThreshold)
                        tier = 0;
                    else if (zone < HeroThreshold)
                        tier = 1;
                    else
                        tier = 2;

                    if (tier == 2 && HasHeroNeighbor(floorTilemap, cell, bp, seed))
                        tier = 1;

                    int tierOffset, tierSize;
                    if (tier == 0) { tierOffset = 0; tierSize = 8; }
                    else if (tier == 1) { tierOffset = 8; tierSize = 5; }
                    else { tierOffset = 13; tierSize = 3; }

                    uint hash = (uint)((x * 73856093) ^ (y * 19349663) ^ seed);
                    int varIdx = tierOffset + (int)(hash % (uint)tierSize);

                    floorTilemap.SetTile(cell, variantSet[varIdx]);

                    int arrIdx = (y - bp.roomOrigin.y) * bp.roomWidth + (x - bp.roomOrigin.x);
                    if (arrIdx >= 0 && arrIdx < bp.floorVariantIndex.Length)
                        bp.floorVariantIndex[arrIdx] = (byte)varIdx;
                }
            }

            floorTilemap.RefreshAllTiles();
            return true;
        }

        public static bool PreviewVariants(Tilemap floorTilemap, RoomBlueprint bp, TileBase[] variantSet)
        {
            if (variantSet == null || variantSet.Length == 0)
                return false;

            BoundsInt bounds = floorTilemap.cellBounds;
            int seed = bp.noiseSeed;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);

                    if (floorTilemap.GetTile(cell) == null)
                        continue;

                    float seedOffset = seed * 0.1f;
                    float wx = Mathf.PerlinNoise((x + 5.2f + seedOffset) * WarpFreq, (y + 1.3f + seedOffset) * WarpFreq) * WarpStrength;
                    float wy = Mathf.PerlinNoise((x + 9.2f + seedOffset) * WarpFreq, (y + 2.8f + seedOffset) * WarpFreq) * WarpStrength;
                    float zone = Mathf.PerlinNoise((x + wx) * ZoneFreq, (y + wy) * ZoneFreq);

                    int tier;
                    if (zone < AccentThreshold)
                        tier = 0;
                    else if (zone < HeroThreshold)
                        tier = 1;
                    else
                        tier = 2;

                    if (tier == 2 && HasHeroNeighbor(floorTilemap, cell, bp, seed))
                        tier = 1;

                    int tierOffset, tierSize;
                    if (tier == 0) { tierOffset = 0; tierSize = 8; }
                    else if (tier == 1) { tierOffset = 8; tierSize = 5; }
                    else { tierOffset = 13; tierSize = 3; }

                    uint hash = (uint)((x * 73856093) ^ (y * 19349663) ^ seed);
                    int varIdx = tierOffset + (int)(hash % (uint)tierSize);

                    floorTilemap.SetTile(cell, variantSet[varIdx]);
                }
            }

            floorTilemap.RefreshAllTiles();
            return true;
        }

        public static void RestoreDefault(Tilemap floorTilemap, TileBase baseTile)
        {
            BoundsInt bounds = floorTilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);
                    if (floorTilemap.GetTile(cell) != null)
                        floorTilemap.SetTile(cell, baseTile);
                }
            }

            floorTilemap.RefreshAllTiles();
        }

        private static bool HasHeroNeighbor(Tilemap tilemap, Vector3Int cell, RoomBlueprint bp, int seed)
        {
            float seedOffset = seed * 0.1f;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int nx = cell.x + dx;
                    int ny = cell.y + dy;

                    float wx2 = Mathf.PerlinNoise((nx + 5.2f + seedOffset) * WarpFreq, (ny + 1.3f + seedOffset) * WarpFreq) * WarpStrength;
                    float wy2 = Mathf.PerlinNoise((nx + 9.2f + seedOffset) * WarpFreq, (ny + 2.8f + seedOffset) * WarpFreq) * WarpStrength;
                    float zone2 = Mathf.PerlinNoise((nx + wx2) * ZoneFreq, (ny + wy2) * ZoneFreq);

                    if (zone2 >= HeroThreshold)
                        return true;
                }
            }

            return false;
        }
    }
}
