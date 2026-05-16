#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class FreeformDecalExecutor : IBrushExecutor
    {
        public PaintMode SupportedMode
        {
            get { return PaintMode.FreeformDecal; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            return DecorativeExecutorUtility.PlaceSingle(stroke, op, "Brush Decal Stamp", 0);
        }
    }

    public static class DecorativeExecutorUtility
    {
        private const string TransitionPainterName = "TransitionBrushPainter";
        private const string DetailPainterName = "DetailDecalPainter";
        private const string AccentPainterName = "AccentPainter";
        private const string TransitionRootName = "TransitionBrushLayer";
        private const string DetailRootName = "DetailDecalLayer";
        private const string AccentRootName = "AccentLayer";

        public static BrushExecutorResult PlaceSingle(BrushStroke stroke, BrushLayerOperation op, string undoName, int salt)
        {
            if (!CanPlace(stroke, op, salt))
            {
                return new BrushExecutorResult { success = true, spawnedCount = 0 };
            }

            Sprite sprite = null;
            if (op != null && op.useNativeBucketVariantPath && op.assetPool != null && op.assetPool.variants != null && op.assetPool.variants.Count > 0)
            {
                var variant = PickVariant(op.assetPool, null, op.radiusForBucketPick, stroke.seed, salt);
                sprite = variant != null ? variant.sprite : null;
            }
            if (sprite == null)
            {
                sprite = PickSprite(op.assetPool, stroke.seed, salt);
            }
            if (sprite == null)
            {
                return Error("Decorative sprite asset pool is empty");
            }

            GameObject spawned = PlaceAt(stroke.currentPositionWorld, sprite, op.targetLayer, op, stroke.seed, salt);
            if (spawned == null)
            {
                return Error("Unsupported decorative target layer " + op.targetLayer);
            }

            Undo.RegisterCreatedObjectUndo(spawned, undoName);
            return new BrushExecutorResult
            {
                success = true,
                spawnedCount = 1,
                spawnedObjects = new List<GameObject> { spawned }
            };
        }

        public static bool CanPlace(BrushStroke stroke, BrushLayerOperation op, int salt)
        {
            if (op == null || op.assetPool == null)
            {
                return false;
            }

            float density = Karar143Enforcement.EffectiveDensity(stroke.currentCell, stroke.currentPositionWorld, stroke.room, op);
            if (density <= 0f)
            {
                return false;
            }

            return Hash01(stroke.seed, stroke.currentCell.x, stroke.currentCell.y, salt) <= Mathf.Clamp01(density);
        }

        public static Sprite PickSprite(AssetPoolSO pool, int seed, int salt)
        {
            if (pool == null)
            {
                return null;
            }

            if (pool.variants != null && pool.variants.Count > 0)
            {
                int vIndex = PickWeightedIndex(pool.variants.Count, null, seed, salt);
                for (int i = 0; i < pool.variants.Count; i++)
                {
                    var variant = pool.variants[(vIndex + i) % pool.variants.Count];
                    if (variant != null && variant.sprite != null)
                    {
                        return variant.sprite;
                    }
                }
            }

            if (pool.sprites == null || pool.sprites.Count == 0)
            {
                return null;
            }

            int index = PickWeightedIndex(pool.sprites.Count, pool.spriteWeights, seed, salt);
            for (int i = 0; i < pool.sprites.Count; i++)
            {
                Sprite sprite = pool.sprites[(index + i) % pool.sprites.Count];
                if (sprite != null)
                {
                    return sprite;
                }
            }

            return null;
        }

        public static BrushAssetVariant PickVariant(AssetPoolSO pool, BrushRadiusProfileSO profile, int radius, int seed, int salt)
        {
            if (pool == null || pool.variants == null || pool.variants.Count == 0)
            {
                return null;
            }

            if (profile == null)
            {
                int idx = PositiveModulo(Mix(seed, salt, 313), pool.variants.Count);
                return pool.variants[idx];
            }

            var weights = profile.ResolveWeights(radius);
            if (weights == null || weights.Count == 0)
            {
                int idx = PositiveModulo(Mix(seed, salt, 313), pool.variants.Count);
                return pool.variants[idx];
            }

            float total = 0f;
            for (int i = 0; i < pool.variants.Count; i++)
            {
                var v = pool.variants[i];
                if (v == null) continue;
                if (!weights.TryGetValue(v.bucket, out float bw)) continue;
                total += Mathf.Max(0f, v.weight) * Mathf.Max(0f, bw);
            }

            if (total <= 0f)
            {
                int idx = PositiveModulo(Mix(seed, salt, 313), pool.variants.Count);
                return pool.variants[idx];
            }

            float roll = Hash01(seed, salt, pool.variants.Count, 911) * total;
            float acc = 0f;
            for (int i = 0; i < pool.variants.Count; i++)
            {
                var v = pool.variants[i];
                if (v == null) continue;
                if (!weights.TryGetValue(v.bucket, out float bw)) continue;
                acc += Mathf.Max(0f, v.weight) * Mathf.Max(0f, bw);
                if (roll < acc) return v;
            }
            return pool.variants[pool.variants.Count - 1];
        }

        private static readonly System.Collections.Generic.HashSet<string> WarnedLegacyPools = new System.Collections.Generic.HashSet<string>();

        private static void WarnLegacyScale(BrushLayerOperation op)
        {
            if (op == null || op.assetPool == null) return;
            string key = string.IsNullOrEmpty(op.assetPool.poolName) ? op.assetPool.GetInstanceID().ToString() : op.assetPool.poolName;
            if (WarnedLegacyPools.Add(key))
            {
                Debug.LogWarning($"[Brush V1 LEGACY] scaleRange applied for pool '{key}'. Set useNativeBucketVariantPath=true on the BrushLayerOperation to switch to native size variant path.");
            }
        }

        public static int PickWeightedIndex(int count, IList<float> weights, int seed, int salt)
        {
            if (count <= 0)
            {
                return -1;
            }

            if (weights != null && weights.Count >= count)
            {
                float total = 0f;
                for (int i = 0; i < count; i++)
                {
                    total += Mathf.Max(0f, weights[i]);
                }

                if (total > 0f)
                {
                    float roll = Hash01(seed, salt, count, 911) * total;
                    float acc = 0f;
                    for (int i = 0; i < count; i++)
                    {
                        acc += Mathf.Max(0f, weights[i]);
                        if (roll < acc)
                        {
                            return i;
                        }
                    }
                }
            }

            return PositiveModulo(Mix(seed, salt, 313), count);
        }

        public static GameObject PlaceAt(Vector2 worldPos, Sprite sprite, TargetLayer layer, BrushLayerOperation op, int seed, int salt)
        {
            Transform parent = ResolveParent(layer);
            if (parent == null || sprite == null)
            {
                return null;
            }

            GameObject decal = new GameObject(layer + "_Decal_" + parent.childCount.ToString("0000"));
            decal.transform.SetParent(parent, false);
            Vector2 jitter = op != null ? op.positionJitter : Vector2.zero;
            float offsetX = jitter.x == 0f ? 0f : (Hash01(seed + 11, salt, 0, 0) * 2f - 1f) * jitter.x;
            float offsetY = jitter.y == 0f ? 0f : (Hash01(seed + 17, salt, 0, 0) * 2f - 1f) * jitter.y;
            decal.transform.position = new Vector3(worldPos.x + offsetX, worldPos.y + offsetY, 0f);

            float scale;
            if (op != null && op.useNativeBucketVariantPath)
            {
                scale = 1f;
            }
            else
            {
                scale = op != null ? Mathf.Lerp(op.scaleRange.x, op.scaleRange.y, Hash01(seed + 23, salt, 0, 0)) : 1f;
                if (op != null)
                {
                    WarnLegacyScale(op);
                }
            }
            float scaleX = scale;
            float scaleY = scale;
            if (op != null && op.allowFlipX && Hash01(seed + 29, salt, 0, 0) < 0.5f)
            {
                scaleX = -scaleX;
            }

            if (op != null && op.allowFlipY && Hash01(seed + 31, salt, 0, 0) < 0.5f)
            {
                scaleY = -scaleY;
            }

            decal.transform.localScale = new Vector3(scaleX, scaleY, 1f);
            if (op != null && op.allowRotation)
            {
                float angle = Hash01(seed + 37, salt, 0, 0) * 360f;
                if (op.rotationSnapDegrees > 0f)
                {
                    angle = Mathf.Round(angle / op.rotationSnapDegrees) * op.rotationSnapDegrees;
                }

                decal.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            SpriteRenderer renderer = decal.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = op != null ? op.tint : Color.white;
            renderer.sortingLayerName = SortingLayerFor(layer);
            renderer.sortingOrder = SortingOrderFor(layer) + (op != null ? op.sortingOrderOffset : 0);
            return decal;
        }

        public static Vector2Int WorldToCell(Vector2 worldPos)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
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

        private static Transform ResolveParent(TargetLayer layer)
        {
            switch (layer)
            {
                case TargetLayer.L4:
                    return EnsureRoot(ResolvePainter<TransitionBrushPainter>(TransitionPainterName).transform, TransitionRootName);
                case TargetLayer.L5:
                    return EnsureRoot(ResolvePainter<DetailDecalPainter>(DetailPainterName).transform, DetailRootName);
                case TargetLayer.L6:
                    return EnsureRoot(ResolvePainter<AccentPainter>(AccentPainterName).transform, AccentRootName);
                default:
                    return null;
            }
        }

        private static T ResolvePainter<T>(string objectName) where T : Component
        {
            T[] painters = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
            if (painters.Length > 0 && painters[0] != null)
            {
                return painters[0];
            }

            GameObject host = new GameObject(objectName);
            return host.AddComponent<T>();
        }

        private static Transform EnsureRoot(Transform host, string rootName)
        {
            Transform existing = host.Find(rootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(rootName);
            root.transform.SetParent(host, false);
            return root.transform;
        }

        private static string SortingLayerFor(TargetLayer layer)
        {
            switch (layer)
            {
                case TargetLayer.L5:
                    return "Detail";
                case TargetLayer.L6:
                    return "Accent";
                default:
                    return "Patch";
            }
        }

        private static int SortingOrderFor(TargetLayer layer)
        {
            switch (layer)
            {
                case TargetLayer.L5:
                    return 3;
                case TargetLayer.L6:
                    return 4;
                default:
                    return 1;
            }
        }

        private static int Mix(int seed, int a, int b)
        {
            unchecked
            {
                int hash = seed;
                hash = (hash * 397) ^ a;
                hash = (hash * 397) ^ b;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
