using UnityEngine;

namespace RIMA.Editor
{
    public static class RoomVariationProcessor
    {
        public enum Level
        {
            Subtle = 1,
            Medium = 3,
            Wild = 6
        }

        public static void Apply(int[,] grid, int w, int h, int seed, Level level)
        {
            if (grid == null)
            {
                return;
            }

            int width = Mathf.Min(w, grid.GetLength(0) - 1);
            int height = Mathf.Min(h, grid.GetLength(1) - 1);
            int passes = Mathf.Clamp((int)level, 1, 6);
            float toggleThreshold = 0.12f + passes * 0.03f;

            int[,] source = (int[,])grid.Clone();
            for (int y = 1; y < height; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    if (!IsBoundaryVertex(source, x, y, width, height))
                    {
                        continue;
                    }

                    float noise = Sample(seed, x, y, 0.27f);
                    if (noise < toggleThreshold)
                    {
                        grid[x, y] = MajorityNeighborValue(source, x, y);
                    }
                    else if (noise > 1f - toggleThreshold * 0.65f)
                    {
                        grid[x, y] = 1 - source[x, y];
                    }
                }
            }

            JitterThinFeatures(grid, width, height, seed + 9173, passes);
            if (level == Level.Wild)
            {
                ScatterRuins(grid, width, height, seed + 4517);
            }

            ClampOuterFrame(grid, width, height);
        }

        public static int[] Flatten(int[,] grid, int w, int h)
        {
            int[] values = new int[(w + 1) * (h + 1)];
            int index = 0;
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    values[index++] = grid != null && x < grid.GetLength(0) && y < grid.GetLength(1)
                        ? Mathf.Clamp(grid[x, y], 0, 1)
                        : 0;
                }
            }

            return values;
        }

        public static int[,] Unflatten(int[] values, int w, int h)
        {
            int[,] grid = new int[w + 1, h + 1];
            int index = 0;
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    grid[x, y] = values != null && index < values.Length ? Mathf.Clamp(values[index], 0, 1) : 0;
                    index++;
                }
            }

            return grid;
        }

        private static bool IsBoundaryVertex(int[,] grid, int x, int y, int w, int h)
        {
            int value = grid[x, y];
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx < 0 || ny < 0 || nx > w || ny > h)
                    {
                        continue;
                    }

                    if (grid[nx, ny] != value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static int MajorityNeighborValue(int[,] grid, int x, int y)
        {
            int ones = 0;
            int total = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx < 0 || ny < 0 || nx >= grid.GetLength(0) || ny >= grid.GetLength(1))
                    {
                        continue;
                    }

                    ones += grid[nx, ny] != 0 ? 1 : 0;
                    total++;
                }
            }

            return ones * 2 >= total ? 1 : 0;
        }

        private static void JitterThinFeatures(int[,] grid, int w, int h, int seed, int passes)
        {
            int[,] source = (int[,])grid.Clone();
            float threshold = 0.045f + passes * 0.014f;
            for (int y = 1; y < h; y++)
            {
                for (int x = 1; x < w; x++)
                {
                    int ones = CountNeighbors(source, x, y, 1);
                    if (source[x, y] == 1 && ones >= 1 && ones <= 4 && Sample(seed, x, y, 0.51f) < threshold)
                    {
                        int dx = Sample(seed + 31, x, y, 0.43f) < 0.5f ? -1 : 1;
                        int dy = Sample(seed + 73, x, y, 0.39f) < 0.5f ? -1 : 1;
                        grid[Mathf.Clamp(x + dx, 1, w - 1), Mathf.Clamp(y + dy, 1, h - 1)] = 1;
                    }
                }
            }
        }

        private static void ScatterRuins(int[,] grid, int w, int h, int seed)
        {
            for (int y = 2; y < h - 1; y++)
            {
                for (int x = 2; x < w - 1; x++)
                {
                    if (grid[x, y] == 0 && CountNeighbors(grid, x, y, 1) == 0 && Sample(seed, x, y, 0.19f) < 0.012f)
                    {
                        grid[x, y] = 1;
                    }
                }
            }
        }

        private static int CountNeighbors(int[,] grid, int x, int y, int target)
        {
            int count = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < grid.GetLength(0) && ny < grid.GetLength(1) && grid[nx, ny] == target)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static float Sample(int seed, int x, int y, float scale)
        {
            int sx = Mathf.Abs(seed % 9973);
            int sy = Mathf.Abs((seed * 37) % 7919);
            return Mathf.PerlinNoise(sx * 0.1231f + x * scale, sy * 0.0973f + y * scale);
        }

        private static void ClampOuterFrame(int[,] grid, int w, int h)
        {
            for (int x = 0; x <= w; x++)
            {
                grid[x, 0] = Mathf.Clamp(grid[x, 0], 0, 1);
                grid[x, h] = Mathf.Clamp(grid[x, h], 0, 1);
            }

            for (int y = 0; y <= h; y++)
            {
                grid[0, y] = Mathf.Clamp(grid[0, y], 0, 1);
                grid[w, y] = Mathf.Clamp(grid[w, y], 0, 1);
            }
        }
    }
}
