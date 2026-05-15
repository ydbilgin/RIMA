using System.Collections.Generic;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public static class CornerWangPainter
    {
        public static void Paint(Tilemap tilemap, RimaBiomePreset biome, int[,] terrainGrid, int width, int height, Vector3Int origin = default, int variantSeed = 0, bool allowFeatureEdges = false)
        {
            if (tilemap == null || biome == null || terrainGrid == null)
            {
                return;
            }

            tilemap.ClearAllTiles();

            var positions = new List<Vector3Int>();
            var tiles = new List<TileBase>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int nw = terrainGrid[x, y + 1];
                    int ne = terrainGrid[x + 1, y + 1];
                    int sw = terrainGrid[x, y];
                    int se = terrainGrid[x + 1, y];

                    TileBase tile = ResolveTile(biome, nw, ne, sw, se, x, y, variantSeed, allowFeatureEdges);
                    if (tile == null)
                    {
                        continue;
                    }

                    positions.Add(origin + new Vector3Int(x, y, 0));
                    tiles.Add(tile);
                }
            }

            tilemap.SetTiles(positions.ToArray(), tiles.ToArray());
            tilemap.RefreshAllTiles();
        }

        public static FeatureEdgeSmoothingPass.PaintResult Paint(Tilemap tilemap, RimaBiomePreset biome, RoomData room, FeatureEdgeSmoothingProfileSO smoothingProfile, Vector3Int origin = default, int variantSeed = 0)
        {
            if (tilemap == null || room.vertexGrid == null)
            {
                return default;
            }

            Paint(tilemap, biome, room.vertexGrid, room.size.x, room.size.y, origin, variantSeed, false);
            return FeatureEdgeSmoothingPass.PaintFeatureEdges(tilemap.transform.parent != null ? tilemap.transform.parent : tilemap.transform, tilemap, biome, room, smoothingProfile, variantSeed);
        }

        public static TileBase ResolveTile(RimaBiomePreset biome, int nw, int ne, int sw, int se, int x = 0, int y = 0, int variantSeed = 0, bool allowFeatureEdges = false)
        {
            if (biome == null)
            {
                return null;
            }

            var unique = new HashSet<int> { nw, ne, sw, se };
            if (unique.Count == 1)
            {
                int id = nw;
                MapTerrain terrain = biome.terrains != null ? biome.terrains.Find(t => t != null && t.id == id) : null;
                if (terrain == null)
                {
                    return null;
                }

                if (!terrain.walkable && !allowFeatureEdges)
                {
                    return null;
                }

                TileBase variant = ResolveVariantTile(terrain, x, y, variantSeed);
                if (variant != null)
                {
                    return variant;
                }

                if (terrain.baseTile != null)
                {
                    return terrain.baseTile;
                }

                return terrain.baseTileSource != null ? terrain.baseTileSource.GetTile(0, 0, 0, 0) : null;
            }

            if (unique.Count != 2)
            {
                return null;
            }

            int lower = int.MaxValue;
            int upper = int.MinValue;
            foreach (int id in unique)
            {
                lower = Mathf.Min(lower, id);
                upper = Mathf.Max(upper, id);
            }

            TilesetPairing pairing = biome.FindPairing(lower, upper);
            if (pairing == null || pairing.tileSet == null || !pairing.isFeatureEdge || !allowFeatureEdges)
            {
                return ResolvePhaseOneFallbackTile(biome, lower, upper, x, y, variantSeed);
            }

            int nwBit = nw == upper ? 1 : 0;
            int neBit = ne == upper ? 1 : 0;
            int swBit = sw == upper ? 1 : 0;
            int seBit = se == upper ? 1 : 0;
            int seed = (x * 73856093) ^ (y * 19349663);
            return pairing.tileSet.GetTile(nwBit, neBit, swBit, seBit, seed);
        }

        private static TileBase ResolvePhaseOneFallbackTile(RimaBiomePreset biome, int lower, int upper, int x, int y, int variantSeed)
        {
            MapTerrain lowerTerrain = biome.terrains != null ? biome.terrains.Find(t => t != null && t.id == lower) : null;
            MapTerrain upperTerrain = biome.terrains != null ? biome.terrains.Find(t => t != null && t.id == upper) : null;
            MapTerrain selected = lowerTerrain != null && lowerTerrain.walkable ? lowerTerrain : upperTerrain != null && upperTerrain.walkable ? upperTerrain : lowerTerrain;
            if (selected == null || !selected.walkable)
            {
                return null;
            }

            TileBase variant = ResolveVariantTile(selected, x, y, variantSeed);
            if (variant != null)
            {
                return variant;
            }

            if (selected.baseTile != null)
            {
                return selected.baseTile;
            }

            return selected.baseTileSource != null ? selected.baseTileSource.GetTile(0, 0, 0, 0) : null;
        }

        private static TileBase ResolveVariantTile(MapTerrain terrain, int x, int y, int variantSeed)
        {
            if (terrain == null || terrain.variantPool == null || terrain.variantPool.Count == 0)
            {
                return null;
            }

            int start = PositiveModulo(MixHash(x, y, terrain.id, variantSeed), terrain.variantPool.Count);
            for (int i = 0; i < terrain.variantPool.Count; i++)
            {
                TileBase candidate = terrain.variantPool[(start + i) % terrain.variantPool.Count];
                if (candidate != null)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static int MixHash(int x, int y, int terrainId, int seed)
        {
            unchecked
            {
                int hash = seed;
                hash = (hash * 397) ^ x;
                hash = (hash * 397) ^ y;
                hash = (hash * 397) ^ terrainId;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }

        // Deprecated: binary per-layer painter kept for older tools and scenes.
        public static void Paint(Tilemap tilemap, CornerWangTileSetSO tileSet, int[,] vertices, int width, int height, Vector3Int origin = default)
        {
            if (tilemap == null || tileSet == null || vertices == null)
            {
                return;
            }

            tilemap.ClearAllTiles();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TileBase tile = tileSet.GetTile(
                        vertices[x, y + 1],
                        vertices[x + 1, y + 1],
                        vertices[x, y],
                        vertices[x + 1, y],
                        (x * 73856093) ^ (y * 19349663));

                    if (tile != null)
                    {
                        tilemap.SetTile(origin + new Vector3Int(x, y, 0), tile);
                    }
                }
            }

            tilemap.RefreshAllTiles();
        }
    }
}
