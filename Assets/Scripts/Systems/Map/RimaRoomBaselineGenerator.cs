using RIMA.RoomDesigner.Core;
using UnityEngine;

namespace RIMA.Systems.Map
{
    public sealed class RimaRoomBaselineGenerator : RoomBaselineGeneratorBase
    {
        public override GridSnapshot Generate(GenerationInput input)
        {
            int width = Mathf.Max(3, input.width);
            int height = Mathf.Max(3, input.height);
            byte[] floor = new byte[width * height];
            byte[] wall = new byte[width * height];

            int seed = SeedPipeline.DeriveSubSeed(input.seed, $"{input.biomeId}:{input.archetypeId}:{input.generatorVersion}");
            var random = new System.Random(seed);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    bool border = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                    if (border)
                    {
                        wall[index] = 1;
                    }
                    else
                    {
                        floor[index] = 1;
                    }
                }
            }

            CarveAnchorPocket(floor, wall, width, height, random, 1, 1);
            CarveAnchorPocket(floor, wall, width, height, random, width - 2, 1);
            CarveAnchorPocket(floor, wall, width, height, random, 1, height - 2);
            CarveAnchorPocket(floor, wall, width, height, random, width - 2, height - 2);

            return new GridSnapshot(floor, wall, width, height, Vector3Int.zero, input.generatorVersion);
        }

        private static void CarveAnchorPocket(byte[] floor, byte[] wall, int width, int height, System.Random random, int anchorX, int anchorY)
        {
            int radius = random.Next(1, 3);
            for (int y = anchorY - radius; y <= anchorY + radius; y++)
            {
                for (int x = anchorX - radius; x <= anchorX + radius; x++)
                {
                    if (x <= 0 || y <= 0 || x >= width - 1 || y >= height - 1)
                    {
                        continue;
                    }

                    int index = y * width + x;
                    floor[index] = 1;
                    wall[index] = 0;
                }
            }
        }
    }
}
