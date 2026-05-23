using System;
using System.Collections.Generic;
using RIMA.Data;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner
{
    [Serializable]
    public struct EncounterPlacement
    {
        public Vector2Int gridPos;
        public string slotType;
        public GameObject prefabHint;
    }

    [Serializable]
    public struct RoomData
    {
        public Vector2Int size;
        public int seed;
        public int[,] vertexGrid;
        public int[,] terrainGrid;
        public bool[,] walkable;
        public List<WallSegment> wallEdges;
        public List<EncounterPlacement> encounters;
        public PatchAtlasSO patchAtlas;
        public WallBrushSetSO wallBrushSet;
        public PatchAtlasSO transitionAtlas;
        public PatchAtlasSO decalAtlas;
        public PatchAtlasSO accentAtlas;
        public ScatterBrushSO scatterBrush;
        public List<BackgroundLayerData> backgroundLayers;
        public NaturalFeatureGraphResult naturalFeatures;
        public FeatureEdgeSmoothingProfileSO featureEdgeSmoothingProfile;

        public string ToJson()
        {
            return JsonUtility.ToJson(ProceduralRoomGenerator.RoomDataDto.FromRoomData(this), true);
        }

        public static RoomData FromJson(string json, PatchAtlasSO patchAtlas = null, ScatterBrushSO scatterBrush = null)
        {
            ProceduralRoomGenerator.RoomDataDto dto = JsonUtility.FromJson<ProceduralRoomGenerator.RoomDataDto>(json);
            return dto != null ? dto.ToRoomData(patchAtlas, scatterBrush) : default;
        }
    }

    public static class ProceduralRoomGenerator
    {
        public static RoomData Generate(RoomRecipe recipe)
        {
            return recipe != null ? Generate(recipe, recipe.seed) : default;
        }

        public static RoomData Generate(RoomRecipe recipe, int seed)
        {
            if (recipe == null)
            {
                return default;
            }

            int width = Mathf.Max(4, recipe.size.x);
            int height = Mathf.Max(4, recipe.size.y);
            int floorTerrain = GetTerrainId(recipe, 0, 0);
            int secondaryTerrain = GetTerrainId(recipe, 1, floorTerrain);
            int wallTerrain = GetTerrainId(recipe, recipe.allowedTerrains != null ? recipe.allowedTerrains.Count - 1 : 0, 1);

            int[,] grid = new int[width + 1, height + 1];
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    bool border = x == 0 || y == 0 || x == width || y == height;
                    grid[x, y] = border ? wallTerrain : floorTerrain;
                }
            }

            PaintOrganicSecondary(grid, width, height, seed, floorTerrain, secondaryTerrain);
            CarvePath(grid, width, height, floorTerrain);

            bool[,] walkable = BuildWalkableCells(grid, width, height, recipe, wallTerrain);
            int[,] terrainGrid = BuildCellTerrainGrid(grid, walkable, width, height, wallTerrain);

            RoomData room = new RoomData
            {
                size = new Vector2Int(width, height),
                seed = seed,
                vertexGrid = grid,
                terrainGrid = terrainGrid,
                walkable = walkable,
                wallEdges = BuildWallEdges(walkable, width, height),
                encounters = BuildEncounters(recipe, width, height),
                patchAtlas = recipe.patchAtlas,
                wallBrushSet = recipe.wallBrushSet,
                transitionAtlas = recipe.transitionAtlas != null ? recipe.transitionAtlas : recipe.patchAtlas,
                decalAtlas = recipe.decalAtlas,
                accentAtlas = recipe.accentAtlas,
                scatterBrush = recipe.scatterBrush,
                featureEdgeSmoothingProfile = recipe.featureEdgeSmoothingProfile
            };

            if (recipe.featureSettings != null)
            {
                int featureSeed = recipe.featureSettings.seed != 0 ? recipe.featureSettings.seed : seed;
                if (recipe.featureSettings.featureType == FeatureType.Water)
                {
                    room.naturalFeatures = VoronoiWaterFeatureGenerator.Generate(room, recipe.featureSettings, featureSeed);
                }
                else
                {
                    room.naturalFeatures = VoronoiElevationFeatureGenerator.Generate(room, recipe.featureSettings, featureSeed);
                }
            }

            return room;
        }

        public static string Serialize(RoomData data)
        {
            return data.ToJson();
        }

        public static RoomData Deserialize(string json, PatchAtlasSO patchAtlas = null, ScatterBrushSO scatterBrush = null)
        {
            return RoomData.FromJson(json, patchAtlas, scatterBrush);
        }

        private static int GetTerrainId(RoomRecipe recipe, int index, int fallback)
        {
            if (recipe.allowedTerrains == null || recipe.allowedTerrains.Count == 0)
            {
                return fallback;
            }

            index = Mathf.Clamp(index, 0, recipe.allowedTerrains.Count - 1);
            TerrainDefinition terrain = recipe.allowedTerrains[index];
            return terrain != null ? terrain.terrainId : fallback;
        }

        private static void PaintOrganicSecondary(int[,] grid, int width, int height, int seed, int floorTerrain, int secondaryTerrain)
        {
            if (secondaryTerrain == floorTerrain)
            {
                return;
            }

            float ox = seed * 0.013f;
            float oy = seed * 0.029f;
            for (int y = 1; y < height; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    float noise = Mathf.PerlinNoise(ox + x * 0.18f, oy + y * 0.18f);
                    if (noise > 0.63f && !IsCenterCombatZone(x, y, width, height))
                    {
                        grid[x, y] = secondaryTerrain;
                    }
                }
            }
        }

        private static void CarvePath(int[,] grid, int width, int height, int floorTerrain)
        {
            int centerY = height / 2;
            int centerX = width / 2;
            for (int x = 1; x < width; x++)
            {
                grid[x, centerY] = floorTerrain;
                if (centerY + 1 < height)
                {
                    grid[x, centerY + 1] = floorTerrain;
                }
            }

            for (int y = 1; y < height; y++)
            {
                grid[centerX, y] = floorTerrain;
                if (centerX + 1 < width)
                {
                    grid[centerX + 1, y] = floorTerrain;
                }
            }
        }

        private static bool IsCenterCombatZone(int x, int y, int width, int height)
        {
            return x > width * 0.3f && x < width * 0.7f && y > height * 0.3f && y < height * 0.7f;
        }

        private static List<EncounterPlacement> BuildEncounters(RoomRecipe recipe, int width, int height)
        {
            var placements = new List<EncounterPlacement>();
            if (recipe.encounters == null)
            {
                return placements;
            }

            for (int i = 0; i < recipe.encounters.Count; i++)
            {
                EncounterSlot slot = recipe.encounters[i];
                placements.Add(new EncounterPlacement
                {
                    gridPos = new Vector2Int(Mathf.Clamp(slot.gridPos.x, 1, width - 1), Mathf.Clamp(slot.gridPos.y, 1, height - 1)),
                    slotType = slot.slotType,
                    prefabHint = slot.prefabHint
                });
            }

            return placements;
        }

        private static bool[,] BuildWalkableCells(int[,] vertexGrid, int width, int height, RoomRecipe recipe, int wallTerrain)
        {
            bool[,] walkable = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    walkable[x, y] =
                        IsTerrainWalkable(recipe, vertexGrid[x, y], wallTerrain) &&
                        IsTerrainWalkable(recipe, vertexGrid[x + 1, y], wallTerrain) &&
                        IsTerrainWalkable(recipe, vertexGrid[x, y + 1], wallTerrain) &&
                        IsTerrainWalkable(recipe, vertexGrid[x + 1, y + 1], wallTerrain);
                }
            }

            return walkable;
        }

        private static int[,] BuildCellTerrainGrid(int[,] vertexGrid, bool[,] walkable, int width, int height, int wallTerrain)
        {
            int[,] cellGrid = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cellGrid[x, y] = walkable[x, y] ? MajorityTerrain(vertexGrid[x, y], vertexGrid[x + 1, y], vertexGrid[x, y + 1], vertexGrid[x + 1, y + 1]) : wallTerrain;
                }
            }

            return cellGrid;
        }

        private static int MajorityTerrain(int sw, int se, int nw, int ne)
        {
            if (sw == se || sw == nw || sw == ne)
            {
                return sw;
            }

            if (se == nw || se == ne)
            {
                return se;
            }

            return nw == ne ? nw : sw;
        }

        private static bool IsTerrainWalkable(RoomRecipe recipe, int terrainId, int wallTerrain)
        {
            if (recipe != null && recipe.allowedTerrains != null)
            {
                for (int i = 0; i < recipe.allowedTerrains.Count; i++)
                {
                    TerrainDefinition terrain = recipe.allowedTerrains[i];
                    if (terrain != null && terrain.terrainId == terrainId)
                    {
                        return terrain.walkable;
                    }
                }
            }

            return terrainId != wallTerrain;
        }

        public static List<WallSegment> BuildWallEdges(bool[,] walkable, int width, int height)
        {
            var edges = new List<WallSegment>();
            if (walkable == null)
            {
                return edges;
            }

            int maxX = Mathf.Min(width, walkable.GetLength(0));
            int maxY = Mathf.Min(height, walkable.GetLength(1));
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (!walkable[x, y])
                    {
                        continue;
                    }

                    bool northBlocked = !IsWalkable(walkable, x, y + 1);
                    bool southBlocked = !IsWalkable(walkable, x, y - 1);
                    bool eastBlocked = !IsWalkable(walkable, x + 1, y);
                    bool westBlocked = !IsWalkable(walkable, x - 1, y);

                    if (northBlocked)
                    {
                        edges.Add(CreateWallSegment(x, y + 1, x + 1, y + 1, WallDirection.North, eastBlocked || westBlocked));
                    }

                    if (southBlocked)
                    {
                        edges.Add(CreateWallSegment(x, y, x + 1, y, WallDirection.South, eastBlocked || westBlocked));
                    }

                    if (eastBlocked)
                    {
                        edges.Add(CreateWallSegment(x + 1, y, x + 1, y + 1, WallDirection.East, northBlocked || southBlocked));
                    }

                    if (westBlocked)
                    {
                        edges.Add(CreateWallSegment(x, y, x, y + 1, WallDirection.West, northBlocked || southBlocked));
                    }
                }
            }

            return edges;
        }

        private static WallSegment CreateWallSegment(int startX, int startY, int endX, int endY, WallDirection direction, bool isCorner)
        {
            return new WallSegment
            {
                start = new Vector2Int(startX, startY),
                end = new Vector2Int(endX, endY),
                direction = direction,
                isCorner = isCorner,
                isDoorway = false
            };
        }

        private static bool IsWalkable(bool[,] walkable, int x, int y)
        {
            return walkable != null &&
                x >= 0 &&
                y >= 0 &&
                x < walkable.GetLength(0) &&
                y < walkable.GetLength(1) &&
                walkable[x, y];
        }

        [Serializable]
        internal class RoomDataDto
        {
            public int width;
            public int height;
            public int seed;
            public int[] vertexGrid;
            public int[] terrainGrid;
            public bool[] walkable;
            public List<WallSegment> wallEdges = new List<WallSegment>();
            public List<EncounterPlacement> encounters = new List<EncounterPlacement>();

            public static RoomDataDto FromRoomData(RoomData data)
            {
                return new RoomDataDto
                {
                    width = data.size.x,
                    height = data.size.y,
                    seed = data.seed,
                    vertexGrid = Flatten(data.vertexGrid, data.size.x, data.size.y),
                    terrainGrid = FlattenCellGrid(data.terrainGrid, data.size.x, data.size.y),
                    walkable = FlattenWalkable(data.walkable, data.size.x, data.size.y),
                    wallEdges = data.wallEdges ?? new List<WallSegment>(),
                    encounters = data.encounters ?? new List<EncounterPlacement>()
                };
            }

            public RoomData ToRoomData(PatchAtlasSO patchAtlas, ScatterBrushSO scatterBrush)
            {
                return new RoomData
                {
                    size = new Vector2Int(width, height),
                    seed = seed,
                    vertexGrid = Unflatten(vertexGrid, width, height),
                    terrainGrid = UnflattenCellGrid(terrainGrid, width, height),
                    walkable = UnflattenWalkable(walkable, width, height),
                    wallEdges = wallEdges ?? new List<WallSegment>(),
                    encounters = encounters ?? new List<EncounterPlacement>(),
                    patchAtlas = patchAtlas,
                    transitionAtlas = patchAtlas,
                    scatterBrush = scatterBrush
                };
            }

            private static int[] Flatten(int[,] grid, int width, int height)
            {
                int[] values = new int[(width + 1) * (height + 1)];
                int index = 0;
                for (int y = 0; y <= height; y++)
                {
                    for (int x = 0; x <= width; x++)
                    {
                        values[index++] = grid != null && x < grid.GetLength(0) && y < grid.GetLength(1) ? grid[x, y] : 0;
                    }
                }

                return values;
            }

            private static int[] FlattenCellGrid(int[,] grid, int width, int height)
            {
                int[] values = new int[width * height];
                int index = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        values[index++] = grid != null && x < grid.GetLength(0) && y < grid.GetLength(1) ? grid[x, y] : 0;
                    }
                }

                return values;
            }

            private static bool[] FlattenWalkable(bool[,] grid, int width, int height)
            {
                bool[] values = new bool[width * height];
                int index = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        values[index++] = grid != null && x < grid.GetLength(0) && y < grid.GetLength(1) && grid[x, y];
                    }
                }

                return values;
            }

            private static int[,] Unflatten(int[] values, int width, int height)
            {
                int[,] grid = new int[width + 1, height + 1];
                int index = 0;
                for (int y = 0; y <= height; y++)
                {
                    for (int x = 0; x <= width; x++)
                    {
                        grid[x, y] = values != null && index < values.Length ? values[index] : 0;
                        index++;
                    }
                }

                return grid;
            }

            private static int[,] UnflattenCellGrid(int[] values, int width, int height)
            {
                int[,] grid = new int[width, height];
                int index = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y] = values != null && index < values.Length ? values[index] : 0;
                        index++;
                    }
                }

                return grid;
            }

            private static bool[,] UnflattenWalkable(bool[] values, int width, int height)
            {
                bool[,] grid = new bool[width, height];
                int index = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y] = values != null && index < values.Length && values[index];
                        index++;
                    }
                }

                return grid;
            }
        }
    }
}
