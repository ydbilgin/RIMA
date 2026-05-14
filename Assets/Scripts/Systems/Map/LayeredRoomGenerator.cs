using RIMA.RoomDesigner.Core;
using UnityEngine;

namespace RIMA.Systems.Map
{
    /// <summary>
    /// Cellular automata cave generator for Alabaster Dawn layered room system.
    /// Drops Wang transition tiles in favor of single floor + single wall sprite layers.
    /// </summary>
    public sealed class LayeredRoomGenerator : RoomBaselineGeneratorBase
    {
        private const float InitialWallFillRatio = 0.44f;
        private const int Iterations = 5;
        private const int BornThreshold = 5;
        private const int SurviveThreshold = 4;

        public override GridSnapshot Generate(GenerationInput input)
        {
            int width = Mathf.Max(8, input.width);
            int height = Mathf.Max(8, input.height);
            int seed = SeedPipeline.DeriveSubSeed(input.seed, $"{input.biomeId}:{input.archetypeId}:{input.generatorVersion}:layered");
            var rng = new System.Random(seed);

            bool[,] wall = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool border = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                    wall[x, y] = border || rng.NextDouble() < InitialWallFillRatio;
                }
            }

            for (int iter = 0; iter < Iterations; iter++)
            {
                wall = StepCA(wall, width, height);
            }

            wall = KeepLargestFloorComponent(wall, width, height);

            int floorCount = CountFloor(wall, width, height);
            int minFloor = (int)(width * height * 0.40f);
            int retries = 0;
            while (floorCount < minFloor && retries < 3)
            {
                CarveCirclePocket(wall, width, height);
                floorCount = CountFloor(wall, width, height);
                retries++;
            }

            byte[] floorArr = new byte[width * height];
            byte[] wallArr = new byte[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    if (wall[x, y])
                    {
                        wallArr[idx] = 1;
                    }
                    else
                    {
                        floorArr[idx] = 1;
                    }
                }
            }

            return new GridSnapshot(floorArr, wallArr, width, height, Vector3Int.zero, input.generatorVersion);
        }

        private static bool[,] StepCA(bool[,] src, int w, int h)
        {
            var dst = new bool[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x == 0 || y == 0 || x == w - 1 || y == h - 1)
                    {
                        dst[x, y] = true;
                        continue;
                    }

                    int wallNbrs = 0;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                            {
                                continue;
                            }

                            if (src[x + dx, y + dy])
                            {
                                wallNbrs++;
                            }
                        }
                    }

                    dst[x, y] = src[x, y] ? wallNbrs >= SurviveThreshold : wallNbrs >= BornThreshold;
                }
            }

            return dst;
        }

        private static bool[,] KeepLargestFloorComponent(bool[,] wall, int w, int h)
        {
            var visited = new bool[w, h];
            int bestSize = 0;
            int bestSx = -1;
            int bestSy = -1;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (wall[x, y] || visited[x, y])
                    {
                        continue;
                    }

                    int size = FloodFillCount(wall, visited, w, h, x, y);
                    if (size > bestSize)
                    {
                        bestSize = size;
                        bestSx = x;
                        bestSy = y;
                    }
                }
            }

            visited = new bool[w, h];
            if (bestSx >= 0)
            {
                FloodFillMark(wall, visited, w, h, bestSx, bestSy);
            }

            var result = new bool[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    result[x, y] = wall[x, y] || !visited[x, y];
                }
            }

            return result;
        }

        private static int FloodFillCount(bool[,] wall, bool[,] visited, int w, int h, int sx, int sy)
        {
            var stack = new System.Collections.Generic.Stack<(int, int)>();
            stack.Push((sx, sy));
            int count = 0;
            while (stack.Count > 0)
            {
                var (cx, cy) = stack.Pop();
                if (cx < 0 || cy < 0 || cx >= w || cy >= h)
                {
                    continue;
                }

                if (visited[cx, cy] || wall[cx, cy])
                {
                    continue;
                }

                visited[cx, cy] = true;
                count++;
                stack.Push((cx + 1, cy));
                stack.Push((cx - 1, cy));
                stack.Push((cx, cy + 1));
                stack.Push((cx, cy - 1));
            }

            return count;
        }

        private static void FloodFillMark(bool[,] wall, bool[,] visited, int w, int h, int sx, int sy)
        {
            var stack = new System.Collections.Generic.Stack<(int, int)>();
            stack.Push((sx, sy));
            while (stack.Count > 0)
            {
                var (cx, cy) = stack.Pop();
                if (cx < 0 || cy < 0 || cx >= w || cy >= h)
                {
                    continue;
                }

                if (visited[cx, cy] || wall[cx, cy])
                {
                    continue;
                }

                visited[cx, cy] = true;
                stack.Push((cx + 1, cy));
                stack.Push((cx - 1, cy));
                stack.Push((cx, cy + 1));
                stack.Push((cx, cy - 1));
            }
        }

        private static int CountFloor(bool[,] wall, int w, int h)
        {
            int c = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (!wall[x, y])
                    {
                        c++;
                    }
                }
            }

            return c;
        }

        private static void CarveCirclePocket(bool[,] wall, int w, int h)
        {
            int cx = w / 2;
            int cy = h / 2;
            int r = Mathf.Min(w, h) / 4;
            for (int x = cx - r; x <= cx + r; x++)
            {
                for (int y = cy - r; y <= cy + r; y++)
                {
                    if (x <= 0 || y <= 0 || x >= w - 1 || y >= h - 1)
                    {
                        continue;
                    }

                    int dx = x - cx;
                    int dy = y - cy;
                    if (dx * dx + dy * dy <= r * r)
                    {
                        wall[x, y] = false;
                    }
                }
            }
        }
    }
}
