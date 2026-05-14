using RIMA.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public class PatchOverlayPainter : MonoBehaviour
    {
        private const string RootName = "PatchOverlay";

        [SerializeField] private string sortingLayerName = "Patch";
        [SerializeField] private int sortingOrderOffset = 1;

        public void PaintPatches(Tilemap baseTilemap, PatchAtlasSO atlas, int seed)
        {
            if (baseTilemap == null || atlas == null || atlas.patches == null || atlas.patches.Count == 0)
            {
                return;
            }

            Transform root = EnsureRoot();
            ClearChildren(root);

            baseTilemap.CompressBounds();
            BoundsInt bounds = baseTilemap.cellBounds;
            int painted = 0;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    Vector3Int cell = new Vector3Int(x, y, 0);
                    if (!baseTilemap.HasTile(cell))
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

                        float roll = Hash01(seed, x, y, i);
                        if (roll > Mathf.Clamp01(entry.density))
                        {
                            continue;
                        }

                        CreatePatch(root, baseTilemap, cell, entry, seed, i, painted++);
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

        private void CreatePatch(Transform root, Tilemap tilemap, Vector3Int cell, PatchEntry entry, int seed, int entryIndex, int index)
        {
            GameObject patch = new GameObject("Patch_" + index.ToString("0000"));
            patch.transform.SetParent(root, false);

            Vector3 basePosition = tilemap.GetCellCenterWorld(cell);
            float offsetX = HashSigned(seed + 17, cell.x, cell.y, entryIndex) * 0.45f;
            float offsetY = HashSigned(seed + 29, cell.x, cell.y, entryIndex) * 0.45f;
            patch.transform.position = basePosition + new Vector3(offsetX, offsetY, 0f);

            Vector2 scale = entry.size;
            if (scale == Vector2.zero)
            {
                scale = Vector2.one;
            }

            patch.transform.localScale = new Vector3(scale.x, scale.y, 1f);
            float jitter = entry.rotationJitter <= 0f ? 15f : entry.rotationJitter;
            patch.transform.rotation = Quaternion.Euler(0f, 0f, HashSigned(seed + 43, cell.x, cell.y, entryIndex) * jitter);

            SpriteRenderer renderer = patch.AddComponent<SpriteRenderer>();
            renderer.sprite = entry.sprite;
            renderer.color = LerpColor(entry.tintMin, entry.tintMax, Hash01(seed + 61, cell.x, cell.y, entryIndex));
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = sortingOrderOffset;
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            return new Color(
                Mathf.Lerp(a.r, b.r, t),
                Mathf.Lerp(a.g, b.g, t),
                Mathf.Lerp(a.b, b.b, t),
                Mathf.Lerp(a.a, b.a, t));
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
