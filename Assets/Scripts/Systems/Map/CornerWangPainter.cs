using System.Collections.Generic;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public static class CornerWangPainter
    {
        public static void Paint(Tilemap tilemap, RimaBiomePreset biome, int[,] terrainGrid, int width, int height, Vector3Int origin = default)
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

                    TileBase tile = ResolveTile(biome, nw, ne, sw, se);
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

        public static TileBase ResolveTile(RimaBiomePreset biome, int nw, int ne, int sw, int se)
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
            if (pairing == null || pairing.tileSet == null)
            {
                return null;
            }

            int nwBit = nw == upper ? 1 : 0;
            int neBit = ne == upper ? 1 : 0;
            int swBit = sw == upper ? 1 : 0;
            int seBit = se == upper ? 1 : 0;
            return pairing.tileSet.GetTile(nwBit, neBit, swBit, seBit);
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
                        vertices[x + 1, y]);

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
