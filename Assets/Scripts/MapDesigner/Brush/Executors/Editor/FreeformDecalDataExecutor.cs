#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class FreeformDecalDataExecutor : IBrushExecutor
    {
        public PaintMode SupportedMode
        {
            get { return PaintMode.FreeformDecal; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            return RoomDecalDataExecutorUtility.PlaceSingle(stroke, op, "Brush Decal Data Stamp", 0);
        }
    }

    internal static class RoomDecalDataExecutorUtility
    {
        public static BrushExecutorResult PlaceSingle(BrushStroke stroke, BrushLayerOperation op, string undoName, int salt)
        {
            if (op == null || op.assetPool == null)
            {
                return Error("Decorative data operation or AssetPool is null");
            }

            if (op.roomDecalData == null)
            {
                return Error("RoomDecalDataSO is null");
            }

            if (!DecorativeExecutorUtility.CanPlace(stroke, op, salt))
            {
                return new BrushExecutorResult
                {
                    success = true,
                    spawnedCount = 0,
                    spawnedObjects = null,
                    modifiedAssets = new List<Object> { op.roomDecalData }
                };
            }

            DecalPlacement placement;
            if (!TryCreatePlacement(stroke, op, salt, out placement))
            {
                return Error("Decorative sprite asset pool is empty");
            }

            Undo.RecordObject(op.roomDecalData, undoName);
            op.roomDecalData.placements.Add(placement);
            EditorUtility.SetDirty(op.roomDecalData);

            return new BrushExecutorResult
            {
                success = true,
                spawnedCount = 1,
                spawnedObjects = null,
                modifiedAssets = new List<Object> { op.roomDecalData }
            };
        }

        public static bool TryCreatePlacement(BrushStroke stroke, BrushLayerOperation op, int salt, out DecalPlacement placement)
        {
            placement = default;
            Sprite sprite;
            int spriteId;
            if (!TryPickSprite(op, stroke.seed, salt, out spriteId, out sprite) || sprite == null)
            {
                return false;
            }

            Vector2 jitter = op != null ? op.positionJitter : Vector2.zero;
            float offsetX = jitter.x == 0f ? 0f : (DecorativeExecutorUtility.Hash01(stroke.seed + 11, salt, 0, 0) * 2f - 1f) * jitter.x;
            float offsetY = jitter.y == 0f ? 0f : (DecorativeExecutorUtility.Hash01(stroke.seed + 17, salt, 0, 0) * 2f - 1f) * jitter.y;

            var random = new System.Random(Mix(stroke.seed, salt, 7919));
            byte flags = 0;
            if (AllowsFlipX(op) && random.NextDouble() < 0.5d)
            {
                flags |= 1;
            }

            if (AllowsFlipY(op) && random.NextDouble() < 0.5d)
            {
                flags |= 2;
            }

            placement = new DecalPlacement
            {
                worldPos = new Vector2(stroke.currentPositionWorld.x + offsetX, stroke.currentPositionWorld.y + offsetY),
                spriteId = spriteId,
                layer = LayerByte(op.targetLayer),
                rotationStep = PickRotationStep(op, random),
                flags = flags,
                tintPackedRGB = PackTint(op.tint)
            };
            return true;
        }

        public static BrushExecutorResult BuildResult(RoomDecalDataSO data, int count)
        {
            return new BrushExecutorResult
            {
                success = true,
                spawnedCount = count,
                spawnedObjects = null,
                modifiedAssets = data != null ? new List<Object> { data } : null
            };
        }

        private static bool TryPickSprite(BrushLayerOperation op, int seed, int salt, out int spriteId, out Sprite sprite)
        {
            spriteId = -1;
            sprite = null;

            if (op.patchAtlas != null && op.patchAtlas.variants != null && op.patchAtlas.variants.Length > 0)
            {
                int start = PositiveModulo(Mix(seed, salt, 313), op.patchAtlas.variants.Length);
                for (int i = 0; i < op.patchAtlas.variants.Length; i++)
                {
                    int index = (start + i) % op.patchAtlas.variants.Length;
                    if (op.patchAtlas.variants[index] != null)
                    {
                        spriteId = index;
                        sprite = op.patchAtlas.variants[index];
                        return true;
                    }
                }
            }

            AssetPoolSO pool = op.assetPool;
            if (pool == null)
            {
                return false;
            }

            if (op.useNativeBucketVariantPath && pool.variants != null && pool.variants.Count > 0)
            {
                BrushAssetVariant variant = DecorativeExecutorUtility.PickVariant(pool, null, op.radiusForBucketPick, seed, salt);
                if (variant != null && variant.sprite != null)
                {
                    spriteId = Mathf.Max(0, pool.variants.IndexOf(variant));
                    sprite = variant.sprite;
                    return true;
                }
            }

            if (pool.sprites == null || pool.sprites.Count == 0)
            {
                return false;
            }

            int startIndex = DecorativeExecutorUtility.PickWeightedIndex(pool.sprites.Count, pool.spriteWeights, seed, salt);
            for (int i = 0; i < pool.sprites.Count; i++)
            {
                int index = (startIndex + i) % pool.sprites.Count;
                if (pool.sprites[index] != null)
                {
                    spriteId = index;
                    sprite = pool.sprites[index];
                    return true;
                }
            }

            return false;
        }

        private static bool AllowsFlipX(BrushLayerOperation op)
        {
            if (op == null || !op.allowFlipX)
            {
                return false;
            }

            if (op.assetPool != null && !op.assetPool.supportsFlip)
            {
                return false;
            }

            return op.patchAtlas == null || op.patchAtlas.allowedTransforms.flipX;
        }

        private static bool AllowsFlipY(BrushLayerOperation op)
        {
            if (op == null || !op.allowFlipY)
            {
                return false;
            }

            if (op.assetPool != null && !op.assetPool.supportsFlip)
            {
                return false;
            }

            return op.patchAtlas == null || op.patchAtlas.allowedTransforms.flipY;
        }

        private static byte PickRotationStep(BrushLayerOperation op, System.Random random)
        {
            if (op == null || !op.allowRotation || random == null)
            {
                return 0;
            }

            if (op.assetPool != null && !op.assetPool.supportsRotation)
            {
                return 0;
            }

            var allowed = new List<byte> { 0 };
            if (AllowsRotationStep(op, 1))
            {
                allowed.Add(1);
            }

            if (AllowsRotationStep(op, 2))
            {
                allowed.Add(2);
            }

            if (AllowsRotationStep(op, 3))
            {
                allowed.Add(3);
            }

            return allowed[random.Next(0, allowed.Count)];
        }

        private static bool AllowsRotationStep(BrushLayerOperation op, int step)
        {
            if (op.patchAtlas != null)
            {
                switch (step)
                {
                    case 1:
                        return op.patchAtlas.allowedTransforms.rotate90;
                    case 2:
                        return op.patchAtlas.allowedTransforms.rotate180;
                    case 3:
                        return op.patchAtlas.allowedTransforms.rotate270;
                }
            }

            if (op.rotationSnapDegrees <= 0f)
            {
                return true;
            }

            float angle = step * 90f;
            float snapped = Mathf.Round(angle / op.rotationSnapDegrees) * op.rotationSnapDegrees;
            return Mathf.Abs(Mathf.DeltaAngle(angle, snapped)) < 0.01f;
        }

        private static byte LayerByte(TargetLayer layer)
        {
            switch (layer)
            {
                case TargetLayer.L4:
                    return 4;
                case TargetLayer.L5:
                    return 5;
                case TargetLayer.L6:
                    return 6;
                default:
                    return 4;
            }
        }

        private static short PackTint(Color color)
        {
            if (Approximately(color.r, 1f) && Approximately(color.g, 1f) && Approximately(color.b, 1f))
            {
                return 0;
            }

            int r = Mathf.Clamp(Mathf.RoundToInt(color.r * 31f), 0, 31);
            int g = Mathf.Clamp(Mathf.RoundToInt(color.g * 63f), 0, 63);
            int b = Mathf.Clamp(Mathf.RoundToInt(color.b * 31f), 0, 31);
            int packed = (r << 11) | (g << 5) | b;
            return unchecked((short)packed);
        }

        private static bool Approximately(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.0001f;
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
