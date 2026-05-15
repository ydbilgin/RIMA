using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner
{
    public enum FeatureType
    {
        None,
        Water,
        Elevation,
        Rift
    }

    [Serializable]
    public struct NaturalFeatureGraphResult
    {
        public Vector2[] sites;
        public int[,] siteIndex;
        public bool[,] featureMask;
        public FeatureType[] siteTypes;

        public bool HasData
        {
            get
            {
                return sites != null &&
                    siteIndex != null &&
                    featureMask != null &&
                    siteTypes != null &&
                    sites.Length == siteTypes.Length;
            }
        }
    }

    public static class NaturalFeatureGraph
    {
        public const int FloorTerrainId = 1;
        public const int WaterTerrainId = 10;
        public const int ElevationTerrainId = 11;
        public const int RiftTerrainId = 12;

        public static List<Vector2> GenerateSites(Vector2Int roomSize, int siteCount, int seed)
        {
            int gridN = Mathf.Max(2, Mathf.RoundToInt(Mathf.Sqrt(Mathf.Max(1, siteCount))));
            float cellW = Mathf.Max(1, roomSize.x) / (float)gridN;
            float cellH = Mathf.Max(1, roomSize.y) / (float)gridN;
            var sites = new List<Vector2>(gridN * gridN);

            for (int gy = 0; gy < gridN; gy++)
            {
                for (int gx = 0; gx < gridN; gx++)
                {
                    float jx = Hash01(seed, gx, gy, 0);
                    float jy = Hash01(seed, gx, gy, 1);
                    sites.Add(new Vector2((gx + jx) * cellW, (gy + jy) * cellH));
                }
            }

            return sites;
        }

        public static int[,] RasterizeVoronoi(Vector2Int size, List<Vector2> sites)
        {
            int width = Mathf.Max(1, size.x);
            int height = Mathf.Max(1, size.y);
            int[,] indices = new int[width, height];
            if (sites == null || sites.Count == 0)
            {
                return indices;
            }

            Vector2[] siteArray = sites.ToArray();
            for (int y = 0; y < height; y++)
            {
                float cy = y + 0.5f;
                for (int x = 0; x < width; x++)
                {
                    float cx = x + 0.5f;
                    float bestDistSq = float.MaxValue;
                    int bestIndex = 0;
                    for (int i = 0; i < siteArray.Length; i++)
                    {
                        float dx = siteArray[i].x - cx;
                        float dy = siteArray[i].y - cy;
                        float distSq = dx * dx + dy * dy;
                        if (distSq < bestDistSq)
                        {
                            bestDistSq = distSq;
                            bestIndex = i;
                        }
                    }

                    indices[x, y] = bestIndex;
                }
            }

            return indices;
        }

        public static NaturalFeatureGraphResult Generate(Vector2Int roomSize, bool[,] walkable, int siteCount, int seed, FeatureType featureType, float featureSiteRatio = 0.18f)
        {
            List<Vector2> sites = GenerateSites(roomSize, siteCount, seed);
            int[,] siteIndex = RasterizeVoronoi(roomSize, sites);
            FeatureType[] siteTypes = AssignFeatureTypes(sites.Count, seed, featureType, featureSiteRatio);
            bool[,] featureMask = BuildFeatureMask(roomSize, walkable, siteIndex, siteTypes);

            return new NaturalFeatureGraphResult
            {
                sites = sites.ToArray(),
                siteIndex = siteIndex,
                featureMask = featureMask,
                siteTypes = siteTypes
            };
        }

        public static bool HasFeatureData(NaturalFeatureGraphResult result)
        {
            return result.HasData &&
                result.featureMask.GetLength(0) > 0 &&
                result.featureMask.GetLength(1) > 0;
        }

        public static int TerrainIdForFeature(FeatureType featureType)
        {
            switch (featureType)
            {
                case FeatureType.Water:
                    return WaterTerrainId;
                case FeatureType.Elevation:
                    return ElevationTerrainId;
                case FeatureType.Rift:
                    return RiftTerrainId;
                default:
                    return FloorTerrainId;
            }
        }

        public static float SampleFeatureProximity(Vector2Int cell, NaturalFeatureGraphResult features, Vector2Int roomSize)
        {
            if (!HasFeatureData(features) ||
                cell.x < 0 ||
                cell.y < 0 ||
                cell.x >= roomSize.x ||
                cell.y >= roomSize.y ||
                cell.x >= features.featureMask.GetLength(0) ||
                cell.y >= features.featureMask.GetLength(1))
            {
                return 1f;
            }

            Vector2 cellCenter = new Vector2(cell.x + 0.5f, cell.y + 0.5f);
            float minDistSq = float.MaxValue;
            int nearestSite = -1;
            for (int i = 0; i < features.sites.Length; i++)
            {
                if (features.siteTypes[i] == FeatureType.None)
                {
                    continue;
                }

                float distSq = (features.sites[i] - cellCenter).sqrMagnitude;
                if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    nearestSite = i;
                }
            }

            if (nearestSite < 0)
            {
                return 1f;
            }

            if (features.featureMask[cell.x, cell.y])
            {
                return 1f;
            }

            float dist = Mathf.Sqrt(minDistSq);
            if (dist <= 4f)
            {
                return 0.8f;
            }

            if (dist <= 8f)
            {
                return 0.3f;
            }

            return 0.1f;
        }

        public static float Hash01(int seed, int x, int y, int salt)
        {
            unchecked
            {
                uint hash = 2166136261u;
                hash = (hash ^ (uint)seed) * 16777619u;
                hash = (hash ^ (uint)x) * 16777619u;
                hash = (hash ^ (uint)y) * 16777619u;
                hash = (hash ^ (uint)salt) * 16777619u;
                hash ^= hash >> 13;
                hash *= 1274126177u;
                return (hash & 0x00FFFFFF) / 16777215f;
            }
        }

        private static FeatureType[] AssignFeatureTypes(int siteCount, int seed, FeatureType featureType, float featureSiteRatio)
        {
            FeatureType[] siteTypes = new FeatureType[siteCount];
            if (featureType == FeatureType.None || siteCount <= 0)
            {
                return siteTypes;
            }

            int assigned = 0;
            float ratio = Mathf.Clamp01(featureSiteRatio);
            for (int i = 0; i < siteTypes.Length; i++)
            {
                if (Hash01(seed, i, siteCount, 97) <= ratio)
                {
                    siteTypes[i] = featureType;
                    assigned++;
                }
            }

            if (assigned == 0)
            {
                siteTypes[PositiveModulo(seed, siteTypes.Length)] = featureType;
            }

            return siteTypes;
        }

        private static bool[,] BuildFeatureMask(Vector2Int size, bool[,] walkable, int[,] siteIndex, FeatureType[] siteTypes)
        {
            int width = Mathf.Max(1, size.x);
            int height = Mathf.Max(1, size.y);
            bool[,] mask = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (walkable != null &&
                        (x >= walkable.GetLength(0) || y >= walkable.GetLength(1) || !walkable[x, y]))
                    {
                        continue;
                    }

                    int site = siteIndex[x, y];
                    mask[x, y] = site >= 0 && site < siteTypes.Length && siteTypes[site] != FeatureType.None;
                }
            }

            return mask;
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }
    }
}
