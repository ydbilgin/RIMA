using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public class ScatterBrushPainter : MonoBehaviour
    {
        private const string RootName = "ScatterLayer";
        private const int ChunkSize = 8;

        [SerializeField] private string sortingLayerName = "Scatter";
        [SerializeField] private int sortingOrder = 2;

        public void PaintScatter(Tilemap baseTilemap, ScatterBrushSO brush, int seed)
        {
            if (baseTilemap == null || brush == null || brush.entries == null || brush.entries.Count == 0)
            {
                return;
            }

            Transform root = EnsureRoot();
            ClearChildren(root);

            baseTilemap.CompressBounds();
            BoundsInt bounds = baseTilemap.cellBounds;
            int created = 0;

            for (int cy = bounds.yMin; cy < bounds.yMax; cy += ChunkSize)
            {
                for (int cx = bounds.xMin; cx < bounds.xMax; cx += ChunkSize)
                {
                    for (int entryIndex = 0; entryIndex < brush.entries.Count; entryIndex++)
                    {
                        ScatterEntry entry = brush.entries[entryIndex];
                        if (entry == null || entry.sprite == null)
                        {
                            continue;
                        }

                        int min = Mathf.Max(0, entry.minCount);
                        int max = Mathf.Max(min, entry.maxCount);
                        int count = min + Mathf.FloorToInt(Hash01(seed, cx, cy, entryIndex) * (max - min + 1));

                        for (int i = 0; i < count; i++)
                        {
                            Vector3Int cell = PickCellInChunk(bounds, cx, cy, seed, entryIndex, i);
                            if (!baseTilemap.HasTile(cell))
                            {
                                continue;
                            }

                            float noise = Mathf.PerlinNoise(
                                (cell.x + seed * 0.017f + entryIndex * 31f) * Mathf.Max(0.001f, entry.perlinFrequency),
                                (cell.y + seed * 0.031f + entryIndex * 47f) * Mathf.Max(0.001f, entry.perlinFrequency));

                            if (noise < Mathf.Clamp01(entry.perlinThreshold))
                            {
                                continue;
                            }

                            CreateScatter(root, baseTilemap, cell, entry, seed, entryIndex, i, created++);
                        }
                    }
                }
            }
        }

        private Transform EnsureRoot()
        {
            Transform existing = transform.Find(RootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(RootName);
            root.transform.SetParent(transform, false);
            return root.transform;
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

        private static Vector3Int PickCellInChunk(BoundsInt bounds, int cx, int cy, int seed, int entryIndex, int index)
        {
            int width = Mathf.Max(1, Mathf.Min(ChunkSize, bounds.xMax - cx));
            int height = Mathf.Max(1, Mathf.Min(ChunkSize, bounds.yMax - cy));
            int x = cx + Mathf.FloorToInt(Hash01(seed + 11, cx, cy, entryIndex * 101 + index) * width);
            int y = cy + Mathf.FloorToInt(Hash01(seed + 23, cx, cy, entryIndex * 131 + index) * height);
            return new Vector3Int(x, y, 0);
        }

        private void CreateScatter(Transform root, Tilemap tilemap, Vector3Int cell, ScatterEntry entry, int seed, int entryIndex, int index, int objectIndex)
        {
            GameObject scatter = new GameObject("Scatter_" + objectIndex.ToString("0000"));
            scatter.transform.SetParent(root, false);

            Vector3 basePosition = tilemap.GetCellCenterWorld(cell);
            float offsetX = HashSigned(seed + 37, cell.x, cell.y, entryIndex * 17 + index) * 0.42f;
            float offsetY = HashSigned(seed + 53, cell.x, cell.y, entryIndex * 19 + index) * 0.42f;
            scatter.transform.position = basePosition + new Vector3(offsetX, offsetY, 0f);
            scatter.transform.rotation = Quaternion.Euler(0f, 0f, HashSigned(seed + 71, cell.x, cell.y, entryIndex * 23 + index) * 180f);

            float scale = Mathf.Lerp(0.75f, 1.2f, Hash01(seed + 89, cell.x, cell.y, entryIndex * 29 + index));
            scatter.transform.localScale = Vector3.one * scale;

            SpriteRenderer renderer = scatter.AddComponent<SpriteRenderer>();
            renderer.sprite = entry.sprite;
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = sortingOrder;
        }

        private static float HashSigned(int seed, int x, int y, int salt)
        {
            return Hash01(seed, x, y, salt) * 2f - 1f;
        }

        private static float Hash01(int seed, int x, int y, int salt)
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
    }
}
