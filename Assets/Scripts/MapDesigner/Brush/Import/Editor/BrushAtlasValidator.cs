#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Import.Editor
{
    public static class BrushAtlasValidator
    {
        public static List<ValidationIssue> Validate(AssetPoolSO pool, Texture2D texture, SliceLayoutTemplateSO template)
        {
            var issues = new List<ValidationIssue>();
            if (pool == null)
            {
                issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_POOL_NULL", "AssetPool is null", "atlas"));
                return issues;
            }
            if (texture == null)
            {
                issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_PNG_UNREADABLE", "Texture is null", "atlas"));
                return issues;
            }

            if (template != null && template.masterSize.x > 0 && template.masterSize.y > 0)
            {
                if (texture.width != template.masterSize.x || texture.height != template.masterSize.y)
                {
                    issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_MASTER_SIZE_MISMATCH",
                        $"Texture {texture.width}x{texture.height} != template {template.masterSize.x}x{template.masterSize.y}", "atlas"));
                }
            }

            string assetPath = AssetDatabase.GetAssetPath(texture);
            if (!string.IsNullOrEmpty(assetPath))
            {
                var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer != null)
                {
                    if (importer.filterMode != FilterMode.Point)
                    {
                        issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_FILTER_NOT_POINT", "filterMode is not Point", "atlas"));
                    }
                    if (importer.textureCompression != TextureImporterCompression.Uncompressed)
                    {
                        issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_COMPRESSION_NOT_NONE", "textureCompression is not Uncompressed", "atlas"));
                    }
                    if (importer.mipmapEnabled)
                    {
                        issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_COMPRESSION_NOT_NONE", "mipmapEnabled is true", "atlas"));
                    }
                }
            }

            var seen = new HashSet<string>();
            int variantCount = pool.variants != null ? pool.variants.Count : 0;
            var bucketCounts = new Dictionary<SizeBucket, int>();
            int heroCount = 0;
            for (int i = 0; i < variantCount; i++)
            {
                var v = pool.variants[i];
                if (v == null)
                {
                    issues.Add(new ValidationIssue(ValidationIssueSeverity.Warning, "VAL_NULL_VARIANT", $"variant index {i} is null", "atlas"));
                    continue;
                }

                if (string.IsNullOrEmpty(v.variantId))
                {
                    issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_DUPLICATE_VARIANT_ID", $"variant index {i} has empty variantId", "atlas"));
                }
                else if (!seen.Add(v.variantId))
                {
                    issues.Add(new ValidationIssue(ValidationIssueSeverity.Error, "VAL_DUPLICATE_VARIANT_ID", $"duplicate variantId '{v.variantId}'", v.variantId));
                }

                if (v.weight <= 0f)
                {
                    issues.Add(new ValidationIssue(ValidationIssueSeverity.Warning, "VAL_ZERO_WEIGHT", $"variant '{v.variantId}' weight is 0 (will never pick)", v.variantId));
                }

                if (!bucketCounts.ContainsKey(v.bucket))
                {
                    bucketCounts[v.bucket] = 0;
                }
                bucketCounts[v.bucket]++;
                if (v.heroAllowed)
                {
                    heroCount++;
                }
            }

            issues.Add(new ValidationIssue(ValidationIssueSeverity.Info, "INF_VARIANT_COUNT", $"Total variants: {variantCount}", "atlas"));
            foreach (var kv in bucketCounts)
            {
                issues.Add(new ValidationIssue(ValidationIssueSeverity.Info, "INF_BUCKET_DISTRIBUTION", $"{kv.Key}: {kv.Value}", "atlas"));
            }
            issues.Add(new ValidationIssue(ValidationIssueSeverity.Info, "INF_HERO_RATIO", $"Hero allowed: {heroCount}/{variantCount}", "atlas"));

            return issues;
        }
    }
}
#endif
