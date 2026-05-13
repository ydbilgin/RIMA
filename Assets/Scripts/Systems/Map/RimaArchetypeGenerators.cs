using System;
using RIMA.RoomDesigner.Core;
using UnityEngine;

namespace RIMA.Systems.Map
{
    public static class RimaArchetypeGenerators
    {
        public static AnchorZone[] GetDefaultAnchorZones(string archetypeId, int seed, int roomWidth, int roomHeight)
        {
            string id = string.IsNullOrWhiteSpace(archetypeId) ? string.Empty : archetypeId.Trim().ToLowerInvariant();
            switch (id)
            {
                case "combat":
                    return BuildRandom(seed, roomWidth, roomHeight, ("MobSpawner", 4, 1.5f), ("Loot", 1, 1.0f));
                case "elite":
                    return BuildRandom(seed, roomWidth, roomHeight, ("MobSpawner", 2, 1.5f), ("EliteSpawner", 1, 1.5f), ("Loot", 1, 1.0f));
                case "boss":
                    return BuildBoss(seed, roomWidth, roomHeight);
                case "loot":
                    return BuildRandom(seed, roomWidth, roomHeight, ("Loot", 3, 1.0f));
                case "vista":
                    return BuildRandom(seed, roomWidth, roomHeight, ("Scenery", 2, 1.25f));
                default:
                    return BuildRandom(seed, roomWidth, roomHeight, ("MobSpawner", 1, 1.5f));
            }
        }

        private static AnchorZone[] BuildRandom(int seed, int roomWidth, int roomHeight, params (string Tag, int Count, float Radius)[] groups)
        {
            int total = 0;
            for (int i = 0; i < groups.Length; i++)
            {
                total += Math.Max(0, groups[i].Count);
            }

            var anchors = new AnchorZone[total];
            var random = new System.Random(SeedPipeline.DeriveSubSeed(seed, "anchor"));
            float marginX = Math.Max(1f, roomWidth * 0.1f);
            float marginY = Math.Max(1f, roomHeight * 0.1f);
            float minX = marginX;
            float maxX = Math.Max(minX, roomWidth - marginX);
            float minY = marginY;
            float maxY = Math.Max(minY, roomHeight - marginY);
            int index = 0;

            for (int groupIndex = 0; groupIndex < groups.Length; groupIndex++)
            {
                var group = groups[groupIndex];
                for (int i = 0; i < group.Count; i++)
                {
                    anchors[index++] = new AnchorZone
                    {
                        tag = group.Tag,
                        radius = group.Radius,
                        position = new Vector2(NextRange(random, minX, maxX), NextRange(random, minY, maxY))
                    };
                }
            }

            return anchors;
        }

        private static AnchorZone[] BuildBoss(int seed, int roomWidth, int roomHeight)
        {
            var random = new System.Random(SeedPipeline.DeriveSubSeed(seed, "anchor"));
            return new[]
            {
                new AnchorZone
                {
                    tag = "BossSpawner",
                    radius = 2.0f,
                    position = new Vector2(roomWidth * 0.5f, roomHeight * 0.5f)
                },
                new AnchorZone
                {
                    tag = "Loot",
                    radius = 1.0f,
                    position = new Vector2(Math.Max(1f, roomWidth * 0.15f), NextRange(random, roomHeight * 0.2f, roomHeight * 0.8f))
                },
                new AnchorZone
                {
                    tag = "Loot",
                    radius = 1.0f,
                    position = new Vector2(Math.Max(1f, roomWidth * 0.85f), NextRange(random, roomHeight * 0.2f, roomHeight * 0.8f))
                }
            };
        }

        private static float NextRange(System.Random random, float min, float max)
        {
            if (max <= min)
            {
                return min;
            }

            return min + (float)random.NextDouble() * (max - min);
        }
    }
}
