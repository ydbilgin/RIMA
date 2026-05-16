using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Editor
{
    public sealed class FeatureEdgeSmoothingTests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null)
                {
                    Object.DestroyImmediate(cleanup[i]);
                }
            }

            cleanup.Clear();
        }

        [Test]
        public void BoundaryCells_AreDeterministicAndOnlyWalkable()
        {
            RoomData room = CreateRoom();

            List<Vector2Int> a = FeatureEdgeSmoothingPass.BuildBoundaryCells(room);
            List<Vector2Int> b = FeatureEdgeSmoothingPass.BuildBoundaryCells(room);

            Assert.AreEqual(a.Count, b.Count);
            Assert.Greater(a.Count, 0);
            for (int i = 0; i < a.Count; i++)
            {
                Assert.AreEqual(a[i], b[i]);
                Assert.IsTrue(room.walkable[a[i].x, a[i].y]);
            }
        }

        [Test]
        public void PaintFeatureEdges_PlacesOneWangTilePerBoundaryCell()
        {
            RoomData room = CreateRoom();
            RimaBiomePreset biome = CreateBiome();
            Tilemap tilemap = CreateTilemap();

            FeatureEdgeSmoothingPass.PaintResult result = FeatureEdgeSmoothingPass.PaintFeatureEdges(tilemap.transform, tilemap, biome, room, null, 42);

            Assert.AreEqual(result.boundaryCellCount, result.wangTilePlacements);
            Assert.AreEqual(result.boundaryCellCount, CountOccupiedCells(tilemap));
        }

        private RoomData CreateRoom()
        {
            int width = 8;
            int height = 8;
            bool[,] walkable = new bool[width, height];
            int[,] terrain = new int[width, height];
            bool[,] mask = new bool[width, height];
            int[,] siteIndex = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    walkable[x, y] = true;
                    terrain[x, y] = NaturalFeatureGraph.FloorTerrainId;
                    siteIndex[x, y] = 0;
                }
            }

            walkable[0, 0] = false;
            for (int y = 2; y <= 4; y++)
            {
                for (int x = 2; x <= 4; x++)
                {
                    mask[x, y] = true;
                }
            }

            return new RoomData
            {
                size = new Vector2Int(width, height),
                walkable = walkable,
                terrainGrid = terrain,
                naturalFeatures = new NaturalFeatureGraphResult
                {
                    sites = new[] { new Vector2(3.5f, 3.5f) },
                    siteIndex = siteIndex,
                    featureMask = mask,
                    siteTypes = new[] { FeatureType.Water }
                }
            };
        }

        private RimaBiomePreset CreateBiome()
        {
            RimaBiomePreset biome = ScriptableObject.CreateInstance<RimaBiomePreset>();
            cleanup.Add(biome);
            biome.terrains.Add(new MapTerrain
            {
                id = NaturalFeatureGraph.FloorTerrainId,
                walkable = true,
                baseTile = CreateTile()
            });
            biome.terrains.Add(new MapTerrain
            {
                id = NaturalFeatureGraph.WaterTerrainId,
                walkable = false,
                baseTile = CreateTile()
            });

            CornerWangTileSetSO wang = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
            cleanup.Add(wang);
            Tile tile = CreateTile();
            for (int i = 0; i < wang.tiles.Length; i++)
            {
                wang.tiles[i] = tile;
            }

            biome.tilesetPairings.Add(new TilesetPairing
            {
                lowerTerrainId = NaturalFeatureGraph.FloorTerrainId,
                upperTerrainId = NaturalFeatureGraph.WaterTerrainId,
                tileSet = wang,
                isFeatureEdge = true
            });
            return biome;
        }

        private Tilemap CreateTilemap()
        {
            GameObject go = new GameObject("FeatureEdgeTilemap_Test");
            cleanup.Add(go);
            go.AddComponent<Grid>();
            GameObject child = new GameObject("Tilemap");
            cleanup.Add(child);
            child.transform.SetParent(go.transform, false);
            Tilemap tilemap = child.AddComponent<Tilemap>();
            child.AddComponent<TilemapRenderer>();
            return tilemap;
        }

        private Tile CreateTile()
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            cleanup.Add(tile);
            return tile;
        }

        private static int CountOccupiedCells(Tilemap tilemap)
        {
            int count = 0;
            BoundsInt bounds = tilemap.cellBounds;
            foreach (Vector3Int position in bounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
