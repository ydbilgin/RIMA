using System.Collections.Generic;
using RIMA.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public class TransitionBrushPainter : MonoBehaviour
    {
        private const string RootName = "TransitionBrushLayer";

        [SerializeField] private string sortingLayerName = "Patch";
        [SerializeField] private int sortingOrder = 1;

        public void PaintTransitions(Tilemap baseTilemap, RoomData room, PatchAtlasSO atlas, int seed)
        {
            PaintAtlas(RootName, sortingLayerName, sortingOrder, baseTilemap, room, atlas, seed, 0.35f);
        }

        protected void PaintAtlas(string rootName, string layerName, int order, Tilemap baseTilemap, RoomData room, PatchAtlasSO atlas, int seed, float offsetJitter)
        {
            Transform root = EnsureRoot(rootName);
            ClearChildren(root);

            if (atlas == null || atlas.patches == null || atlas.patches.Count == 0 || room.walkable == null)
            {
                return;
            }

            int[,] distances = ComputeWallDistanceMap(room);
            int created = 0;
            int width = room.walkable.GetLength(0);
            int height = room.walkable.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!room.walkable[x, y])
                    {
                        continue;
                    }

                    for (int i = 0; i < atlas.patches.Count; i++)
                    {
                        PatchEntry entry = atlas.patches[i];
                        if (entry == null || entry.sprite == null || entry.density <= 0f)
                        {
                            continue;
                        }

                        Vector2Int cell = new Vector2Int(x, y);
                        float density = atlas.edgeBiased
                            ? DensityForCell(cell, room, entry.density * Mathf.Max(0f, atlas.wallProximityFactor), distances, atlas.featureMask, atlas.featureMaskWeight)
                            : ApplyFeatureDensity(cell, room, entry.density, atlas.featureMask, atlas.featureMaskWeight);

                        if (Hash01(seed, x, y, i) > Mathf.Clamp01(density))
                        {
                            continue;
                        }

                        CreatePatch(root, baseTilemap, new Vector2Int(x, y), entry, seed, i, created++, layerName, order, offsetJitter);
                    }
                }
            }
        }

        public static float DensityForCell(Vector2Int cell, RoomData room, float baseDensity)
        {
            return DensityForCell(cell, room, baseDensity, null);
        }

        public static float DensityForCell(Vector2Int cell, RoomData room, float baseDensity, int[,] distanceMap)
        {
            return DensityForCell(cell, room, baseDensity, distanceMap, null, 0f);
        }

        public static float DensityForCell(Vector2Int cell, RoomData room, float baseDensity, int[,] distanceMap, FeatureMaskSO featureMask, float featureMaskWeight)
        {
            if (!IsWalkable(room, cell.x, cell.y))
            {
                return 0f;
            }

            int distToWall = distanceMap != null ? distanceMap[cell.x, cell.y] : ComputeDistanceToWall(cell, room);
            float factor = distToWall <= 1 ? 1.0f
                : distToWall == 2 ? 0.6f
                : distToWall == 3 ? 0.3f
                : 0.1f;
            return ApplyFeatureDensity(cell, room, baseDensity * factor, featureMask, featureMaskWeight);
        }

        public static float ApplyFeatureDensity(Vector2Int cell, RoomData room, float density, FeatureMaskSO featureMask, float featureMaskWeight)
        {
            if (!IsWalkable(room, cell.x, cell.y))
            {
                return 0f;
            }

            float featureProximity = NaturalFeatureGraph.SampleFeatureProximity(cell, room.naturalFeatures, room.size);
            float maskFactor = 1f;
            if (featureMask != null)
            {
                maskFactor = Mathf.Lerp(1f, featureMask.Sample(cell, room.size), Mathf.Clamp01(featureMaskWeight));
            }

            return density * featureProximity * maskFactor;
        }

        public static int ComputeDistanceToWall(Vector2Int cell, RoomData room)
        {
            int[,] map = ComputeWallDistanceMap(room);
            if (map == null || cell.x < 0 || cell.y < 0 || cell.x >= map.GetLength(0) || cell.y >= map.GetLength(1))
            {
                return int.MaxValue;
            }

            return map[cell.x, cell.y];
        }

        public static int[,] ComputeWallDistanceMap(RoomData room)
        {
            if (room.walkable == null)
            {
                return null;
            }

            int width = room.walkable.GetLength(0);
            int height = room.walkable.GetLength(1);
            int[,] distance = new int[width, height];
            var queue = new Queue<Vector2Int>(width * height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!room.walkable[x, y])
                    {
                        distance[x, y] = 0;
                        queue.Enqueue(new Vector2Int(x, y));
                    }
                    else
                    {
                        distance[x, y] = int.MaxValue;
                    }
                }
            }

            if (queue.Count == 0)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            distance[x, y] = 1;
                            queue.Enqueue(new Vector2Int(x, y));
                        }
                    }
                }
            }

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                int nextDistance = distance[current.x, current.y] + 1;
                TryVisit(current.x + 1, current.y, nextDistance, distance, queue);
                TryVisit(current.x - 1, current.y, nextDistance, distance, queue);
                TryVisit(current.x, current.y + 1, nextDistance, distance, queue);
                TryVisit(current.x, current.y - 1, nextDistance, distance, queue);
            }

            return distance;
        }

        protected static bool IsWalkable(RoomData room, int x, int y)
        {
            return room.walkable != null &&
                x >= 0 &&
                y >= 0 &&
                x < room.walkable.GetLength(0) &&
                y < room.walkable.GetLength(1) &&
                room.walkable[x, y];
        }

        protected static float Hash01(int seed, int x, int y, int salt)
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

        private static void TryVisit(int x, int y, int nextDistance, int[,] distance, Queue<Vector2Int> queue)
        {
            if (x < 0 || y < 0 || x >= distance.GetLength(0) || y >= distance.GetLength(1) || nextDistance >= distance[x, y])
            {
                return;
            }

            distance[x, y] = nextDistance;
            queue.Enqueue(new Vector2Int(x, y));
        }

        private void CreatePatch(Transform root, Tilemap tilemap, Vector2Int cell, PatchEntry entry, int seed, int entryIndex, int index, string layerName, int order, float offsetJitter)
        {
            GameObject patch = new GameObject(root.name + "_" + index.ToString("0000"));
            patch.transform.SetParent(root, false);

            Vector3 basePosition = CellToWorld(tilemap, new Vector2(cell.x + 0.5f, cell.y + 0.5f));
            float offsetX = (Hash01(seed + 17, cell.x, cell.y, entryIndex) * 2f - 1f) * offsetJitter;
            float offsetY = (Hash01(seed + 29, cell.x, cell.y, entryIndex) * 2f - 1f) * offsetJitter;
            patch.transform.position = basePosition + new Vector3(offsetX, offsetY, 0f);
            patch.transform.localScale = new Vector3(entry.size == Vector2.zero ? 1f : entry.size.x, entry.size == Vector2.zero ? 1f : entry.size.y, 1f);
            patch.transform.rotation = Quaternion.Euler(0f, 0f, (Hash01(seed + 43, cell.x, cell.y, entryIndex) * 2f - 1f) * Mathf.Max(0f, entry.rotationJitter));

            SpriteRenderer renderer = patch.AddComponent<SpriteRenderer>();
            renderer.sprite = entry.sprite;
            renderer.color = LerpColor(entry.tintMin, entry.tintMax, Hash01(seed + 61, cell.x, cell.y, entryIndex));
            renderer.sortingLayerName = layerName;
            renderer.sortingOrder = order;
        }

        private Transform EnsureRoot(string rootName)
        {
            Transform existing = transform.Find(rootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(rootName);
            root.transform.SetParent(transform, false);
            return root.transform;
        }

        private static Vector3 CellToWorld(Tilemap tilemap, Vector2 cell)
        {
            if (tilemap == null)
            {
                return new Vector3(cell.x, cell.y, 0f);
            }

            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 unitX = tilemap.CellToWorld(Vector3Int.right) - origin;
            Vector3 unitY = tilemap.CellToWorld(Vector3Int.up) - origin;
            return origin + unitX * cell.x + unitY * cell.y;
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            return new Color(Mathf.Lerp(a.r, b.r, t), Mathf.Lerp(a.g, b.g, t), Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject child = root.GetChild(i).gameObject;
                if (Application.isPlaying)
                {
                    Destroy(child);
                }
                else
                {
                    DestroyImmediate(child);
                }
            }
        }
    }
}
