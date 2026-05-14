using System;
using System.Collections.Generic;
using RIMA.Data;
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
        public List<EncounterPlacement> encounters;
        public PatchAtlasSO patchAtlas;
        public ScatterBrushSO scatterBrush;

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
            if (recipe == null)
            {
                return default;
            }

            int width = Mathf.Max(4, recipe.size.x);
            int height = Mathf.Max(4, recipe.size.y);
            int seed = recipe.seed;
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

            return new RoomData
            {
                size = new Vector2Int(width, height),
                seed = seed,
                vertexGrid = grid,
                encounters = BuildEncounters(recipe, width, height),
                patchAtlas = recipe.patchAtlas,
                scatterBrush = recipe.scatterBrush
            };
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

        [Serializable]
        internal class RoomDataDto
        {
            public int width;
            public int height;
            public int seed;
            public int[] vertexGrid;
            public List<EncounterPlacement> encounters = new List<EncounterPlacement>();

            public static RoomDataDto FromRoomData(RoomData data)
            {
                return new RoomDataDto
                {
                    width = data.size.x,
                    height = data.size.y,
                    seed = data.seed,
                    vertexGrid = Flatten(data.vertexGrid, data.size.x, data.size.y),
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
                    encounters = encounters ?? new List<EncounterPlacement>(),
                    patchAtlas = patchAtlas,
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
        }
    }
}
