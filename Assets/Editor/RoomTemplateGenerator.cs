using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public static class RoomTemplateGenerator
    {
        public const string TemplateFolder = "Assets/RIMA_MapData/templates";
        public const string FloorWallTileSetPath = "Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset";
        public const string RubblePathTileSetPath = "Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset";
        public const string DebrisRiftTileSetPath = "Assets/Art/Tiles/F1/Generated/DebrisRift_CornerWangTileSet.asset";

        public static readonly string[] TemplateNames =
        {
            "small_chamber_24x16",
            "large_hall_36x24",
            "corridor_junction_28x20",
            "l_shape_keep_30x24",
            "broken_courtyard_32x28",
            "twin_chamber_28x20",
            "crypt_aisle_24x32",
            "rift_shrine_24x24"
        };

        [MenuItem("RIMA/Tools/Generate Room Templates")]
        public static void GenerateAllTemplates()
        {
            Directory.CreateDirectory(TemplateFolder);
            foreach (string templateName in TemplateNames)
            {
                string path = Path.Combine(TemplateFolder, templateName + ".json").Replace('\\', '/');
                File.WriteAllText(path, JsonUtility.ToJson(BuildTemplate(templateName), true));
            }

            AssetDatabase.Refresh();
            Debug.Log("[RoomTemplateGenerator] Generated " + TemplateNames.Length + " room templates.");
        }

        [MenuItem("RIMA/Tools/Test - Generate All Templates")]
        public static void TestAll()
        {
            GenerateAllTemplates();
            foreach (string name in TemplateNames)
            {
                var data = LoadTemplate(name);
                Debug.Assert(data.layers != null && data.layers.Length >= 2, name + " missing layers");
                Debug.Assert(data.layers[0].vertexData.Length == (data.width + 1) * (data.height + 1), name + " invalid vertex count");
            }

            Debug.Log("All 8 templates valid");
        }

        public static RimaMapDesignerWindow.MapSaveData LoadTemplate(string templateName)
        {
            string path = GetTemplatePath(templateName);
            if (!File.Exists(path))
            {
                GenerateAllTemplates();
            }

            return JsonUtility.FromJson<RimaMapDesignerWindow.MapSaveData>(File.ReadAllText(path));
        }

        public static string GetTemplatePath(string templateName)
        {
            return Path.Combine(TemplateFolder, templateName + ".json").Replace('\\', '/');
        }

        public static RimaMapDesignerWindow.MapSaveData BuildTemplate(string templateName)
        {
            switch (templateName)
            {
                case "small_chamber_24x16":
                    return BuildSmallChamber();
                case "large_hall_36x24":
                    return BuildLargeHall();
                case "corridor_junction_28x20":
                    return BuildCorridorJunction();
                case "l_shape_keep_30x24":
                    return BuildLShapeKeep();
                case "broken_courtyard_32x28":
                    return BuildBrokenCourtyard();
                case "twin_chamber_28x20":
                    return BuildTwinChamber();
                case "crypt_aisle_24x32":
                    return BuildCryptAisle();
                case "rift_shrine_24x24":
                    return BuildRiftShrine();
                default:
                    throw new ArgumentException("Unknown room template: " + templateName);
            }
        }

        private static RimaMapDesignerWindow.MapSaveData BuildSmallChamber()
        {
            int w = 24;
            int h = 16;
            int[,] baseGrid = WallGrid(w, h);
            CarveEllipse(baseGrid, 10, 9, 8, 5);
            CarveRect(baseGrid, 8, 4, 18, 12);
            SetRect(baseGrid, 18, 1, 23, 5, 1);
            RaggedBoundary(baseGrid, w, h, StableHash("small_chamber_24x16"), 0.33f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 4, 4, 19, 12, 2, 1);
            return Data(w, h, baseGrid, path, null);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildLargeHall()
        {
            int w = 36;
            int h = 24;
            int[,] baseGrid = WallGrid(w, h);
            CarveRect(baseGrid, 4, 5, 31, 19);
            CarveEllipse(baseGrid, 8, 18, 5, 4);
            CarveEllipse(baseGrid, 28, 6, 5, 4);
            SetRect(baseGrid, 14, 10, 16, 12, 1);
            SetRect(baseGrid, 22, 12, 24, 14, 1);
            RaggedBoundary(baseGrid, w, h, StableHash("large_hall_36x24"), 0.27f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 3, 12, 18, 12, 3, 1);
            DrawLine(path, 18, 12, 29, 6, 3, 1);
            return Data(w, h, baseGrid, path, null);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildCorridorJunction()
        {
            int w = 28;
            int h = 20;
            int[,] baseGrid = WallGrid(w, h);
            CarveRect(baseGrid, 10, 6, 18, 14);
            CarveRect(baseGrid, 0, 8, 28, 12);
            CarveRect(baseGrid, 12, 0, 16, 20);
            RaggedBoundary(baseGrid, w, h, StableHash("corridor_junction_28x20"), 0.22f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 0, 10, 28, 10, 2, 1);
            DrawLine(path, 14, 0, 14, 20, 2, 1);
            return Data(w, h, baseGrid, path, null);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildLShapeKeep()
        {
            int w = 30;
            int h = 24;
            int[,] baseGrid = WallGrid(w, h);
            CarveRect(baseGrid, 3, 4, 27, 14);
            CarveRect(baseGrid, 3, 4, 15, 20);
            SetRect(baseGrid, 21, 14, 27, 20, 1);
            RaggedBoundary(baseGrid, w, h, StableHash("l_shape_keep_30x24"), 0.3f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 5, 8, 22, 8, 2, 1);
            DrawLine(path, 8, 8, 8, 18, 2, 1);

            int[,] rift = EmptyGrid(w, h);
            DrawLine(rift, 18, 14, 26, 21, 1, 1);
            return Data(w, h, baseGrid, path, rift);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildBrokenCourtyard()
        {
            int w = 32;
            int h = 28;
            int[,] baseGrid = WallGrid(w, h);
            CarveEllipse(baseGrid, 16, 14, 12, 10);
            CarveRect(baseGrid, 7, 6, 25, 22);
            CarveRect(baseGrid, 0, 12, 5, 16);
            CarveRect(baseGrid, 13, 23, 18, 28);
            CarveRect(baseGrid, 27, 6, 32, 10);
            RaggedBoundary(baseGrid, w, h, StableHash("broken_courtyard_32x28"), 0.36f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 4, 14, 12, 18, 2, 1);
            DrawLine(path, 12, 18, 18, 11, 2, 1);
            DrawLine(path, 18, 11, 28, 8, 2, 1);
            return Data(w, h, baseGrid, path, null);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildTwinChamber()
        {
            int w = 28;
            int h = 20;
            int[,] baseGrid = WallGrid(w, h);
            CarveEllipse(baseGrid, 8, 10, 6, 6);
            CarveEllipse(baseGrid, 20, 10, 6, 6);
            CarveRect(baseGrid, 8, 8, 20, 12);
            SetRect(baseGrid, 13, 5, 15, 15, 1);
            CarveRect(baseGrid, 13, 9, 15, 11);
            RaggedBoundary(baseGrid, w, h, StableHash("twin_chamber_28x20"), 0.28f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 3, 7, 13, 11, 2, 1);
            DrawLine(path, 15, 9, 25, 14, 2, 1);
            return Data(w, h, baseGrid, path, null);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildCryptAisle()
        {
            int w = 24;
            int h = 32;
            int[,] baseGrid = WallGrid(w, h);
            CarveRect(baseGrid, 5, 2, 19, 30);
            for (int y = 7; y <= 25; y += 6)
            {
                SetRect(baseGrid, 9, y, 10, y + 1, 1);
                SetRect(baseGrid, 14, y, 15, y + 1, 1);
                CarveRect(baseGrid, 2, y - 1, 5, y + 2);
                CarveRect(baseGrid, 19, y - 1, 22, y + 2);
            }

            RaggedBoundary(baseGrid, w, h, StableHash("crypt_aisle_24x32"), 0.2f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 12, 1, 12, 31, 3, 1);
            return Data(w, h, baseGrid, path, null);
        }

        private static RimaMapDesignerWindow.MapSaveData BuildRiftShrine()
        {
            int w = 24;
            int h = 24;
            int[,] baseGrid = WallGrid(w, h);
            CarveEllipse(baseGrid, 12, 12, 9, 9);
            CarveRect(baseGrid, 6, 6, 18, 18);
            RaggedBoundary(baseGrid, w, h, StableHash("rift_shrine_24x24"), 0.3f);

            int[,] path = EmptyGrid(w, h);
            DrawLine(path, 12, 4, 12, 20, 2, 1);
            DrawLine(path, 4, 12, 20, 12, 2, 1);
            DrawLine(path, 7, 7, 17, 17, 1, 1);
            DrawLine(path, 17, 7, 7, 17, 1, 1);

            int[,] rift = EmptyGrid(w, h);
            CarveEllipse(rift, 12, 12, 3, 3, 1);
            DrawLine(rift, 10, 10, 14, 14, 1, 1);
            DrawLine(rift, 14, 10, 10, 14, 1, 1);
            return Data(w, h, baseGrid, path, rift);
        }

        private static RimaMapDesignerWindow.MapSaveData Data(int w, int h, int[,] baseGrid, int[,] path, int[,] rift)
        {
            if (rift == null)
            {
                return new RimaMapDesignerWindow.MapSaveData
                {
                    width = w,
                    height = h,
                    layers = new[]
                    {
                        Layer("Base", FloorWallTileSetPath, true, baseGrid, w, h),
                        Layer("Path", RubblePathTileSetPath, true, path, w, h)
                    }
                };
            }

            return new RimaMapDesignerWindow.MapSaveData
            {
                width = w,
                height = h,
                layers = new[]
                {
                    Layer("Base", FloorWallTileSetPath, true, baseGrid, w, h),
                    Layer("Path", RubblePathTileSetPath, true, path, w, h),
                    Layer("Rift", DebrisRiftTileSetPath, true, rift, w, h)
                }
            };
        }

        private static RimaMapDesignerWindow.LayerSaveData Layer(string name, string tileSet, bool enabled, int[,] grid, int w, int h)
        {
            return new RimaMapDesignerWindow.LayerSaveData
            {
                name = name,
                tileSet = tileSet,
                enabled = enabled,
                vertexData = RoomVariationProcessor.Flatten(grid, w, h)
            };
        }

        private static int[,] WallGrid(int w, int h)
        {
            int[,] grid = new int[w + 1, h + 1];
            SetRect(grid, 0, 0, w, h, 1);
            return grid;
        }

        private static int[,] EmptyGrid(int w, int h)
        {
            return new int[w + 1, h + 1];
        }

        private static void CarveRect(int[,] grid, int xMin, int yMin, int xMax, int yMax)
        {
            SetRect(grid, xMin, yMin, xMax, yMax, 0);
        }

        private static void CarveEllipse(int[,] grid, int cx, int cy, int rx, int ry, int value = 0)
        {
            for (int y = cy - ry; y <= cy + ry; y++)
            {
                for (int x = cx - rx; x <= cx + rx; x++)
                {
                    float nx = (x - cx) / Mathf.Max(1f, rx);
                    float ny = (y - cy) / Mathf.Max(1f, ry);
                    if (nx * nx + ny * ny <= 1f)
                    {
                        Set(grid, x, y, value);
                    }
                }
            }
        }

        private static void SetRect(int[,] grid, int xMin, int yMin, int xMax, int yMax, int value)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    Set(grid, x, y, value);
                }
            }
        }

        private static void DrawLine(int[,] grid, int x0, int y0, int x1, int y1, int radius, int value)
        {
            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            while (true)
            {
                SetDisc(grid, x0, y0, radius, value);
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private static void SetDisc(int[,] grid, int cx, int cy, int radius, int value)
        {
            for (int y = cy - radius; y <= cy + radius; y++)
            {
                for (int x = cx - radius; x <= cx + radius; x++)
                {
                    if ((x - cx) * (x - cx) + (y - cy) * (y - cy) <= radius * radius)
                    {
                        Set(grid, x, y, value);
                    }
                }
            }
        }

        private static void RaggedBoundary(int[,] grid, int w, int h, int seed, float strength)
        {
            int[,] source = (int[,])grid.Clone();
            for (int y = 1; y < h; y++)
            {
                for (int x = 1; x < w; x++)
                {
                    if (!Boundary(source, x, y))
                    {
                        continue;
                    }

                    float n = Mathf.PerlinNoise(seed * 0.011f + x * 0.37f, seed * 0.017f + y * 0.37f);
                    if (n < strength * 0.45f)
                    {
                        grid[x, y] = Majority(source, x, y);
                    }
                    else if (n > 1f - strength * 0.25f)
                    {
                        grid[x, y] = 1 - source[x, y];
                    }
                }
            }

            for (int x = 0; x <= w; x++)
            {
                grid[x, 0] = 1;
                grid[x, h] = 1;
            }

            for (int y = 0; y <= h; y++)
            {
                grid[0, y] = 1;
                grid[w, y] = 1;
            }
        }

        private static bool Boundary(int[,] grid, int x, int y)
        {
            int v = grid[x, y];
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if ((dx != 0 || dy != 0) && grid[x + dx, y + dy] != v)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static int Majority(int[,] grid, int x, int y)
        {
            int ones = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx != 0 || dy != 0)
                    {
                        ones += grid[x + dx, y + dy];
                    }
                }
            }

            return ones >= 4 ? 1 : 0;
        }

        private static void Set(int[,] grid, int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1))
            {
                grid[x, y] = Mathf.Clamp(value, 0, 1);
            }
        }

        private static int StableHash(string value)
        {
            unchecked
            {
                int hash = 23;
                for (int i = 0; i < value.Length; i++)
                {
                    hash = hash * 31 + value[i];
                }

                return Mathf.Abs(hash);
            }
        }
    }
}
