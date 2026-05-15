using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.MapDesigner.Brush.Data
{
    public static class BrushJsonSerializer
    {
        public static string ExportBrushToJson(MapDesignerBrushPresetSO brush)
        {
            return JsonUtility.ToJson(ToBrushPresetDTO(brush), true);
        }

        public static BrushPresetDTO ImportBrushFromJson(string json)
        {
            return string.IsNullOrWhiteSpace(json) ? new BrushPresetDTO() : JsonUtility.FromJson<BrushPresetDTO>(json);
        }

        public static string ExportAssetPoolToJson(AssetPoolSO pool)
        {
            return JsonUtility.ToJson(ToAssetPoolDTO(pool), true);
        }

        public static AssetPoolDTO ImportAssetPoolFromJson(string json)
        {
            return string.IsNullOrWhiteSpace(json) ? new AssetPoolDTO() : JsonUtility.FromJson<AssetPoolDTO>(json);
        }

        public static string ExportBrushPackToJson(BrushPackSO pack)
        {
            return JsonUtility.ToJson(ToBrushPackDTO(pack), true);
        }

        public static BrushPackDTO ImportBrushPackFromJson(string json)
        {
            return string.IsNullOrWhiteSpace(json) ? new BrushPackDTO() : JsonUtility.FromJson<BrushPackDTO>(json);
        }

        public static Sprite ResolveSpritePath(string path)
        {
#if UNITY_EDITOR
            return string.IsNullOrWhiteSpace(path) ? null : AssetDatabase.LoadAssetAtPath<Sprite>(path);
#else
            return null;
#endif
        }

        private static BrushPresetDTO ToBrushPresetDTO(MapDesignerBrushPresetSO brush)
        {
            var dto = new BrushPresetDTO();
            if (brush == null)
            {
                return dto;
            }

            dto.brushName = brush.brushName;
            dto.category = brush.category;
            dto.paintMode = brush.paintMode;
            dto.previewIconPath = GetAssetPath(brush.previewIcon);
            dto.showInPalette = brush.showInPalette;
            dto.description = brush.description;
            dto.hotkeyIndex = brush.hotkeyIndex;

            if (brush.operations != null)
            {
                foreach (BrushLayerOperation operation in brush.operations)
                {
                    dto.operations.Add(ToBrushLayerOperationDTO(operation));
                }
            }

            return dto;
        }

        private static BrushLayerOperationDTO ToBrushLayerOperationDTO(BrushLayerOperation operation)
        {
            var dto = new BrushLayerOperationDTO();
            if (operation == null)
            {
                return dto;
            }

            dto.targetLayer = operation.targetLayer;
            dto.assetPoolPath = GetAssetPath(operation.assetPool);
            dto.density = operation.density;
            dto.probability = operation.probability;
            dto.minDistance = operation.minDistance;
            dto.scaleRange = operation.scaleRange;
            dto.allowRotation = operation.allowRotation;
            dto.allowFlipX = operation.allowFlipX;
            dto.allowFlipY = operation.allowFlipY;
            dto.rotationSnapDegrees = operation.rotationSnapDegrees;
            dto.tint = operation.tint;
            dto.positionJitter = operation.positionJitter;
            dto.sortingOrderOffset = operation.sortingOrderOffset;
            dto.affectsCollision = operation.affectsCollision;
            dto.respectsWalkableMask = operation.respectsWalkableMask;
            dto.wallProximityCurve = operation.wallProximityCurve;
            dto.featureMaskMultiplierPath = GetAssetPath(operation.featureMaskMultiplier);
            return dto;
        }

        private static AssetPoolDTO ToAssetPoolDTO(AssetPoolSO pool)
        {
            var dto = new AssetPoolDTO();
            if (pool == null)
            {
                return dto;
            }

            dto.poolName = pool.poolName;
            dto.category = pool.category;
            dto.spriteWeights = pool.spriteWeights != null ? new List<float>(pool.spriteWeights) : new List<float>();
            dto.nativeSize = pool.nativeSize;
            dto.supportsRotation = pool.supportsRotation;
            dto.supportsFlip = pool.supportsFlip;
            dto.isSoftEdge = pool.isSoftEdge;

            AddAssetPaths(pool.sprites, dto.spritePaths);
            AddAssetPaths(pool.tiles, dto.tilePaths);
            AddAssetPaths(pool.prefabs, dto.prefabPaths);
            return dto;
        }

        private static BrushPackDTO ToBrushPackDTO(BrushPackSO pack)
        {
            var dto = new BrushPackDTO();
            if (pack == null)
            {
                return dto;
            }

            dto.packName = pack.packName;
            dto.version = pack.version;
            dto.coverImagePath = GetAssetPath(pack.coverImage);

            if (pack.brushes != null)
            {
                foreach (MapDesignerBrushPresetSO brush in pack.brushes)
                {
                    dto.brushes.Add(ToBrushPresetDTO(brush));
                    dto.brushPaths.Add(GetAssetPath(brush));
                }
            }

            if (pack.referencedPools != null)
            {
                foreach (AssetPoolSO pool in pack.referencedPools)
                {
                    dto.referencedPools.Add(ToAssetPoolDTO(pool));
                    dto.referencedPoolPaths.Add(GetAssetPath(pool));
                }
            }

            return dto;
        }

        private static void AddAssetPaths<T>(IEnumerable<T> assets, List<string> paths) where T : UnityEngine.Object
        {
            if (assets == null)
            {
                return;
            }

            foreach (T asset in assets)
            {
                paths.Add(GetAssetPath(asset));
            }
        }

        private static string GetAssetPath(UnityEngine.Object asset)
        {
#if UNITY_EDITOR
            return asset == null ? string.Empty : AssetDatabase.GetAssetPath(asset);
#else
            return string.Empty;
#endif
        }
    }

    [Serializable]
    public class BrushPresetDTO
    {
        public int schemaVersion = 1;
        public string brushName;
        public BrushCategory category;
        public PaintMode paintMode;
        public List<BrushLayerOperationDTO> operations = new List<BrushLayerOperationDTO>();
        public string previewIconPath;
        public bool showInPalette = true;
        public string description;
        public int hotkeyIndex = -1;
    }

    [Serializable]
    public class BrushLayerOperationDTO
    {
        public TargetLayer targetLayer;
        public string assetPoolPath;
        [Range(0f, 1f)] public float density = 0.5f;
        [Range(0f, 1f)] public float probability = 1.0f;
        public float minDistance = 32f;
        public Vector2 scaleRange = new Vector2(0.85f, 1.15f);
        public bool allowRotation = true;
        public bool allowFlipX = true;
        public bool allowFlipY = false;
        public float rotationSnapDegrees = 0f;
        public Color tint = Color.white;
        public Vector2 positionJitter = Vector2.zero;
        public int sortingOrderOffset = 0;
        public bool affectsCollision = false;
        public bool respectsWalkableMask = true;
        public AnimationCurve wallProximityCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        public string featureMaskMultiplierPath;
    }

    [Serializable]
    public class AssetPoolDTO
    {
        public int schemaVersion = 1;
        public string poolName;
        public AssetCategory category;
        public List<string> spritePaths = new List<string>();
        public List<float> spriteWeights = new List<float>();
        public List<string> tilePaths = new List<string>();
        public List<string> prefabPaths = new List<string>();
        public Vector2Int nativeSize = new Vector2Int(64, 64);
        public bool supportsRotation = true;
        public bool supportsFlip = true;
        public bool isSoftEdge = false;
    }

    [Serializable]
    public class BrushPackDTO
    {
        public int schemaVersion = 1;
        public string packName;
        public string version = "1.0";
        public List<BrushPresetDTO> brushes = new List<BrushPresetDTO>();
        public List<string> brushPaths = new List<string>();
        public List<AssetPoolDTO> referencedPools = new List<AssetPoolDTO>();
        public List<string> referencedPoolPaths = new List<string>();
        public string coverImagePath;
    }
}
