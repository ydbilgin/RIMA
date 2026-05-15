using RIMA.Data;
using UnityEngine;

namespace RIMA.MapDesigner
{
    public static class VoronoiElevationFeatureGenerator
    {
        public static NaturalFeatureGraphResult Generate(RoomData room, NaturalFeatureSettingsSO settings, int seed)
        {
            int siteCount = settings != null ? settings.siteCount : 64;
            float ratio = settings != null ? settings.featureSiteRatio : 0.18f;
            FeatureType featureType = settings != null && settings.featureType == FeatureType.Rift ? FeatureType.Rift : FeatureType.Elevation;
            return NaturalFeatureGraph.Generate(room.size, room.walkable, siteCount, seed, featureType, ratio);
        }

        public static NaturalFeatureGraphResult Generate(Vector2Int roomSize, bool[,] walkable, int siteCount, int seed)
        {
            return NaturalFeatureGraph.Generate(roomSize, walkable, siteCount, seed, FeatureType.Elevation);
        }
    }
}
