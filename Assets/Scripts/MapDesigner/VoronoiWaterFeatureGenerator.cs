using RIMA.Data;
using UnityEngine;

namespace RIMA.MapDesigner
{
    public static class VoronoiWaterFeatureGenerator
    {
        public static NaturalFeatureGraphResult Generate(RoomData room, NaturalFeatureSettingsSO settings, int seed)
        {
            int siteCount = settings != null ? settings.siteCount : 64;
            float ratio = settings != null ? settings.featureSiteRatio : 0.18f;
            return NaturalFeatureGraph.Generate(room.size, room.walkable, siteCount, seed, FeatureType.Water, ratio);
        }

        public static NaturalFeatureGraphResult Generate(Vector2Int roomSize, bool[,] walkable, int siteCount, int seed)
        {
            return NaturalFeatureGraph.Generate(roomSize, walkable, siteCount, seed, FeatureType.Water);
        }
    }
}
