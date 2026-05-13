using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    public static class LayeredRoomPainter
    {
        public static void Paint(
            Tilemap floorTm,
            Tilemap wallTm,
            byte[] floor,
            byte[] wall,
            int w,
            int h,
            TileBase singleFloor,
            TileBase singleWall,
            TileBase[] floorAccents,
            float accentRatio,
            int seed)
        {
            if (floorTm == null || wallTm == null || singleFloor == null || singleWall == null)
            {
                return;
            }

            floorTm.ClearAllTiles();
            wallTm.ClearAllTiles();

            var rng = new System.Random(seed);
            bool hasAccents = floorAccents != null && floorAccents.Length > 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * w + x;
                    var cell = new Vector3Int(x, y, 0);
                    if (wall[idx] == 1)
                    {
                        wallTm.SetTile(cell, singleWall);
                    }
                    else if (floor[idx] == 1)
                    {
                        TileBase tile = singleFloor;
                        if (hasAccents && rng.NextDouble() < accentRatio)
                        {
                            tile = floorAccents[rng.Next(0, floorAccents.Length)];
                        }

                        floorTm.SetTile(cell, tile);
                    }
                }
            }

            floorTm.CompressBounds();
            wallTm.CompressBounds();
        }

        public static void PaintBiome(
            Tilemap baseTm,
            Tilemap decalTm,
            Tilemap wallFrontTm,
            Tilemap wallTopTm,
            byte[] floor,
            byte[] wall,
            int w,
            int h,
            RimaBiomePreset biome,
            TileAssetMetadata[] wangLibrary,
            int seed)
        {
            if (baseTm == null || wallFrontTm == null || biome == null)
            {
                return;
            }

            baseTm.ClearAllTiles();
            if (decalTm != null)
            {
                decalTm.ClearAllTiles();
            }

            wallFrontTm.ClearAllTiles();
            if (wallTopTm != null)
            {
                wallTopTm.ClearAllTiles();
            }

            var rng = new System.Random(seed);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * w + x;
                    if (floor[idx] != 1)
                    {
                        continue;
                    }

                    var cell = new Vector3Int(x, y, 0);
                    TileAssetMetadata floorMeta = PickWeighted(biome.allowedFloorTiles, seed, x, y);
                    if (floorMeta?.tile != null)
                    {
                        baseTm.SetTile(cell, floorMeta.tile);
                    }

                    if (decalTm != null && biome.decalTiles != null && biome.decalTiles.Length > 0)
                    {
                        int hash = seed ^ (x * 1000003) ^ (y * 999983);
                        float r = Mathf.Abs(hash % 10000) / 10000f;
                        if (r < biome.decalDensity)
                        {
                            TileAssetMetadata decalMeta = PickWeighted(biome.decalTiles, seed + 1, x, y);
                            if (decalMeta?.tile != null)
                            {
                                decalTm.SetTile(cell, decalMeta.tile);
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int idx = y * w + x;
                    if (wall[idx] != 1)
                    {
                        continue;
                    }

                    var cell = new Vector3Int(x, y, 0);
                    bool north = IsWall(wall, x, y + 1, w, h);
                    bool east = IsWall(wall, x + 1, y, w, h);
                    bool south = IsWall(wall, x, y - 1, w, h);
                    bool west = IsWall(wall, x - 1, y, w, h);
                    int mask = WangTileResolver.ComputeWangMask(north, east, south, west);

                    TileAssetMetadata meta = ResolveWang(wangLibrary, mask, cell, seed);
                    if (meta == null && biome.allowedWallTiles != null && biome.allowedWallTiles.Length > 0)
                    {
                        meta = PickWeighted(biome.allowedWallTiles, seed, x, y);
                    }

                    if (meta?.tile == null)
                    {
                        continue;
                    }

                    wallFrontTm.SetTile(cell, meta.tile);
                    if (wallTopTm != null && meta.isCliffTop)
                    {
                        wallTopTm.SetTile(cell, meta.tile);
                    }
                }
            }

            baseTm.CompressBounds();
            wallFrontTm.CompressBounds();
        }

        private static TileAssetMetadata PickWeighted(TileAssetMetadata[] pool, int seed, int x, int y)
        {
            if (pool == null || pool.Length == 0)
            {
                return null;
            }

            float total = 0f;
            foreach (TileAssetMetadata tile in pool)
            {
                if (tile != null)
                {
                    total += Mathf.Max(0f, tile.weight);
                }
            }

            if (total <= 0f)
            {
                return pool[0];
            }

            int hash = seed ^ (x * 73856093) ^ (y * 19349663);
            float pick = (Mathf.Abs(hash % 10000) / 10000f) * total;
            float cumulative = 0f;
            foreach (TileAssetMetadata tile in pool)
            {
                if (tile == null)
                {
                    continue;
                }

                cumulative += Mathf.Max(0f, tile.weight);
                if (pick <= cumulative)
                {
                    return tile;
                }
            }

            return pool[pool.Length - 1];
        }

        private static bool IsWall(byte[] wall, int x, int y, int w, int h)
        {
            if (x < 0 || y < 0 || x >= w || y >= h)
            {
                return true;
            }

            return wall[y * w + x] == 1;
        }

        private static TileAssetMetadata ResolveWang(TileAssetMetadata[] lib, int mask, Vector3Int cell, int seed)
        {
            if (lib == null || lib.Length == 0)
            {
                return null;
            }

            var candidates = System.Array.FindAll(lib, tile => tile != null && tile.wangMask == mask);
            if (candidates.Length == 0)
            {
                return null;
            }

            if (candidates.Length == 1)
            {
                return candidates[0];
            }

            int hash = seed ^ (cell.x * 73856093) ^ (cell.y * 19349663);
            float total = 0f;
            foreach (TileAssetMetadata candidate in candidates)
            {
                total += Mathf.Max(0f, candidate.weight);
            }

            float pick = (Mathf.Abs(hash % 10000) / 10000f) * total;
            float cumulative = 0f;
            foreach (TileAssetMetadata candidate in candidates)
            {
                cumulative += Mathf.Max(0f, candidate.weight);
                if (pick <= cumulative)
                {
                    return candidate;
                }
            }

            return candidates[candidates.Length - 1];
        }
    }
}
