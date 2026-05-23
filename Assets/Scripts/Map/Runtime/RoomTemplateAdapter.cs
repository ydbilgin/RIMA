using System.Collections.Generic;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Map
{
    public static class RoomTemplateAdapter
    {
        public const int PaintLayerCount = 6;

        public static RoomData Convert(RoomTemplateSO template)
        {
            return Convert(template, StableSeed(template));
        }

        public static RoomData Convert(RoomTemplateSO template, int seed)
        {
            if (template == null)
            {
                return default;
            }

            int width = Mathf.Max(1, template.bounds.width);
            int height = Mathf.Max(1, template.bounds.height);
            bool[,] walkable = BuildWalkable(template, width, height);

            return new RoomData
            {
                size = new Vector2Int(width, height),
                seed = seed,
                vertexGrid = BuildVertexGrid(walkable, width, height),
                terrainGrid = BuildTerrainGrid(walkable, width, height),
                walkable = walkable,
                wallEdges = ProceduralRoomGenerator.BuildWallEdges(walkable, width, height),
                encounters = BuildEncounters(template),
                backgroundLayers = BuildPaintLayers(template),
            };
        }

        private static bool[,] BuildWalkable(RoomTemplateSO template, int width, int height)
        {
            bool[,] result = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector2Int tile = new Vector2Int(template.bounds.xMin + x, template.bounds.yMin + y);
                    result[x, y] = template.IsWalkable(tile);
                }
            }

            return result;
        }

        private static int[,] BuildTerrainGrid(bool[,] walkable, int width, int height)
        {
            int[,] grid = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[x, y] = walkable[x, y] ? 0 : 1;
                }
            }

            return grid;
        }

        private static int[,] BuildVertexGrid(bool[,] walkable, int width, int height)
        {
            int[,] grid = new int[width + 1, height + 1];
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    grid[x, y] = TouchesWalkableCell(walkable, width, height, x, y) ? 0 : 1;
                }
            }

            return grid;
        }

        private static bool TouchesWalkableCell(bool[,] walkable, int width, int height, int x, int y)
        {
            for (int dy = -1; dy <= 0; dy++)
            {
                for (int dx = -1; dx <= 0; dx++)
                {
                    int cellX = x + dx;
                    int cellY = y + dy;
                    if (cellX >= 0 && cellY >= 0 && cellX < width && cellY < height && walkable[cellX, cellY])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static List<EncounterPlacement> BuildEncounters(RoomTemplateSO template)
        {
            var placements = new List<EncounterPlacement>();
            if (template.enemySpawnSockets == null)
            {
                return placements;
            }

            for (int i = 0; i < template.enemySpawnSockets.Count; i++)
            {
                EnemySpawnSocket socket = template.enemySpawnSockets[i];
                if (socket == null)
                {
                    continue;
                }

                placements.Add(new EncounterPlacement
                {
                    gridPos = ToLocal(template, socket.position),
                    slotType = socket.tierHint,
                    prefabHint = null,
                });
            }

            return placements;
        }

        private static List<BackgroundLayerData> BuildPaintLayers(RoomTemplateSO template)
        {
            var layers = new List<BackgroundLayerData>(PaintLayerCount);
            for (int i = 0; i < PaintLayerCount; i++)
            {
                BackgroundLayerData source = template.backgroundLayers != null && i < template.backgroundLayers.Count
                    ? template.backgroundLayers[i]
                    : null;
                layers.Add(CloneLayer(source, i));
            }

            return layers;
        }

        private static BackgroundLayerData CloneLayer(BackgroundLayerData source, int index)
        {
            if (source == null)
            {
                return new BackgroundLayerData
                {
                    layerName = LayerName(index),
                    sortingOrder = -200 + (index * 30),
                    visible = false
                };
            }

            return new BackgroundLayerData
            {
                layerName = string.IsNullOrEmpty(source.layerName) ? LayerName(index) : source.layerName,
                sprite = source.sprite,
                sortingOrder = source.sortingOrder,
                offset = source.offset,
                scale = source.scale,
                tint = source.tint,
                visible = source.visible
            };
        }

        private static string LayerName(int index)
        {
            switch (index)
            {
                case 0: return "L1 Floor Base";
                case 1: return "L2 Floor Variation";
                case 2: return "L3 Wall Overlay";
                case 3: return "L4 Large Patches";
                case 4: return "L5 Scatter";
                case 5: return "L6 Accent";
                default: return "Layer";
            }
        }

        private static Vector2Int ToLocal(RoomTemplateSO template, Vector2Int tile)
        {
            return new Vector2Int(tile.x - template.bounds.xMin, tile.y - template.bounds.yMin);
        }

        private static int StableSeed(RoomTemplateSO template)
        {
            string value = template != null && !string.IsNullOrEmpty(template.roomId) ? template.roomId : string.Empty;
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < value.Length; i++)
                {
                    hash = (hash * 31) + value[i];
                }

                return hash;
            }
        }
    }
}
