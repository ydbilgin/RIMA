#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.Brush.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Import.Editor
{
    public static class BrushAtlasImporter
    {
        private const string DefaultPoolFolder = "Assets/Art/BrushAtlas/Pools";

        public static ImportResult Import(
            string pngPath,
            SliceLayoutTemplateSO template,
            string poolName,
            TargetLayer layer,
            string namespacePrefix = "")
        {
            var result = new ImportResult();

            if (string.IsNullOrEmpty(pngPath) || !File.Exists(pngPath))
            {
                result.issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_PNG_UNREADABLE",
                    $"PNG not found at '{pngPath}'", "atlas"));
                return result;
            }
            if (template == null)
            {
                result.issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_TEMPLATE_NULL",
                    "SliceLayoutTemplateSO is null", "atlas"));
                return result;
            }

            string assetPath = ToAssetPath(pngPath);
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                result.issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_PNG_UNREADABLE",
                    $"TextureImporter null at '{assetPath}' (asset may not be imported yet)", "atlas"));
                return result;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.spritePixelsPerUnit = 32f;
            importer.spritePivot = template.defaultPivot;

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            List<SliceCell> cells = template.wangAware
                ? WangSliceGenerator.GenerateCells(texture, template, FindMetadataJsonForPng(pngPath))
                : template.cells;

            if (cells == null || cells.Count == 0)
            {
                result.issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_TEMPLATE_NULL",
                    "Template produced 0 cells (check cells list or Wang metadata.json)", "atlas"));
                return result;
            }

            var metadata = new SpriteMetaData[cells.Count];
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                metadata[i] = new SpriteMetaData
                {
                    name = cell.cellName,
                    rect = new Rect(cell.rect.x, cell.rect.y, cell.rect.width, cell.rect.height),
                    pivot = cell.usePivotOverride ? cell.pivotOverride : template.defaultPivot,
                    alignment = (int)SpriteAlignment.Custom
                };
            }
            importer.spritesheet = metadata;
            importer.SaveAndReimport();
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            var spriteLookup = new Dictionary<string, Sprite>();
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] is Sprite sp && sp != null && !spriteLookup.ContainsKey(sp.name))
                {
                    spriteLookup[sp.name] = sp;
                }
            }

            var variants = new List<BrushAssetVariant>();
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                bool isAllFloorReference = (cell.cellName != null && cell.cellName.StartsWith("all_floor_reference"))
                    || (cell.tags != null && System.Array.IndexOf(cell.tags, "all_floor_reference") >= 0);
                if (isAllFloorReference)
                {
                    continue;
                }

                spriteLookup.TryGetValue(cell.cellName, out var sprite);

                var variant = new BrushAssetVariant
                {
                    sprite = sprite,
                    variantId = $"{namespacePrefix}{poolName}_{cell.cellName}",
                    bucket = cell.bucket,
                    weight = 1f,
                    targetLayer = layer,
                    nativeSize = new Vector2Int(cell.rect.width, cell.rect.height),
                    sourceRect = cell.rect,
                    pivot = cell.usePivotOverride ? cell.pivotOverride : template.defaultPivot,
                    footprintRadius = Mathf.Max(cell.rect.width, cell.rect.height) * 0.5f / 32f,
                    allowFlipX = false,
                    allowFlipY = false,
                    allowRotation = false,
                    rotationSnapDegrees = 0f,
                    tags = cell.tags ?? new string[0],
                    heroAllowed = cell.heroAllowed,
                    respectsWalkableMask = true,
                    minDistance = 32f,
                    wallProximityFactor = 1f,
                    schemaVersion = "1.0"
                };
                variants.Add(variant);
            }

            string poolPath = $"{DefaultPoolFolder}/{poolName}.asset";
            EnsureDirectoryExists(poolPath);
            var pool = AssetDatabase.LoadAssetAtPath<AssetPoolSO>(poolPath);
            bool isNew = pool == null;
            if (isNew)
            {
                pool = ScriptableObject.CreateInstance<AssetPoolSO>();
                pool.poolName = poolName;
                AssetDatabase.CreateAsset(pool, poolPath);
            }

            pool.variants = variants;
            pool.sourceMasterTexture = texture;
            pool.importTemplate = template;
            pool.namespacePrefix = namespacePrefix ?? string.Empty;
            pool.defaultTargetLayer = layer;
            if (pool.heroLayerWhitelist == null || pool.heroLayerWhitelist.Length != 6)
            {
                pool.heroLayerWhitelist = new bool[6];
            }

            EditorUtility.SetDirty(pool);
            AssetDatabase.SaveAssets();

            result.pool = pool;
            result.variantCount = variants.Count;
            result.issues.AddRange(BrushAtlasValidator.Validate(pool, texture, template));
            return result;
        }

        public static string ToAssetPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return fullPath;
            }
            string normalized = fullPath.Replace('\\', '/');
            int idx = normalized.IndexOf("Assets/");
            return idx >= 0 ? normalized.Substring(idx) : normalized;
        }

        private static string FindMetadataJsonForPng(string pngPath)
        {
            string dir = Path.GetDirectoryName(pngPath);
            if (string.IsNullOrEmpty(dir))
            {
                return null;
            }
            string candidate = Path.Combine(dir, "metadata.json");
            return File.Exists(candidate) ? candidate : null;
        }

        private static void EnsureDirectoryExists(string assetPath)
        {
            string dir = Path.GetDirectoryName(assetPath);
            if (string.IsNullOrEmpty(dir))
            {
                return;
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif
