#if UNITY_EDITOR
using System.IO;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Validation;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Editor
{
    public static class RoomTemplateSaver
    {
        private const string DefaultRootDir = "Assets/Data/Rooms";

        public static SaveResult SaveRoom(
            GameObject authoringRoot,
            RoomTemplateSO template,
            bool overwriteExisting = true,
            string rootDirOverride = null)
        {
            var result = new SaveResult();

            if (template == null)
            {
                result.issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_NULL_TEMPLATE", "Template is null.", string.Empty));
                return result;
            }

            if (string.IsNullOrEmpty(template.biomeId) || string.IsNullOrEmpty(template.roomId))
            {
                result.issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_MISSING_IDS",
                    "Both biomeId and roomId must be set before saving.",
                    template.roomId ?? string.Empty));
                return result;
            }

            string rootDir = string.IsNullOrEmpty(rootDirOverride) ? DefaultRootDir : rootDirOverride;
            string biomeDir = Path.Combine(rootDir, template.biomeId).Replace('\\', '/');
            EnsureDirectory(biomeDir);

            string prefabPath = $"{biomeDir}/{template.roomId}.prefab";
            string templateAssetPath = $"{biomeDir}/{template.roomId}.asset";

            result.prefabPath = prefabPath;
            result.templateAssetPath = templateAssetPath;

            if (!overwriteExisting && (AssetExists(prefabPath) || AssetExists(templateAssetPath)))
            {
                result.issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_ASSET_EXISTS",
                    $"Asset already exists at '{prefabPath}' or '{templateAssetPath}' and overwriteExisting=false.",
                    template.roomId));
                return result;
            }

            string existingPrefabGuid = AssetDatabase.AssetPathToGUID(prefabPath);
            string existingTemplateGuid = AssetDatabase.AssetPathToGUID(templateAssetPath);
            bool prefabExisted = !string.IsNullOrEmpty(existingPrefabGuid);
            bool templateExisted = !string.IsNullOrEmpty(existingTemplateGuid);

            GameObject prefabAsset = null;
            if (authoringRoot != null)
            {
                ApplyDeterministicNaming(authoringRoot);
                prefabAsset = PrefabUtility.SaveAsPrefabAsset(authoringRoot, prefabPath, out bool prefabSaved);
                if (!prefabSaved || prefabAsset == null)
                {
                    result.issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                        "ERR_PREFAB_SAVE_FAILED",
                        $"PrefabUtility.SaveAsPrefabAsset returned failure for '{prefabPath}'.",
                        template.roomId));
                    return result;
                }
                template.prefabRef = prefabAsset;
            }

            if (templateExisted)
            {
                var existing = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(templateAssetPath);
                if (existing != null)
                {
                    EditorUtility.CopySerialized(template, existing);
                    EditorUtility.SetDirty(existing);
                }
                else
                {
                    result.issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                        "WARN_TEMPLATE_RELOAD_FAILED",
                        $"Existing template at '{templateAssetPath}' could not be loaded for in-place update; creating new asset instead.",
                        template.roomId));
                    AssetDatabase.CreateAsset(template, templateAssetPath);
                }
            }
            else
            {
                AssetDatabase.CreateAsset(template, templateAssetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(templateAssetPath);

            string newPrefabGuid = AssetDatabase.AssetPathToGUID(prefabPath);
            string newTemplateGuid = AssetDatabase.AssetPathToGUID(templateAssetPath);

            bool prefabGuidStable = !prefabExisted || existingPrefabGuid == newPrefabGuid;
            bool templateGuidStable = !templateExisted || existingTemplateGuid == newTemplateGuid;
            result.guidPreserved = prefabGuidStable && templateGuidStable;

            if (!result.guidPreserved)
            {
                result.issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                    "WARN_GUID_CHANGED",
                    $"Asset GUID changed during save (prefabStable={prefabGuidStable}, templateStable={templateGuidStable}).",
                    template.roomId));
            }

            result.issues.AddRange(RoomTemplateValidator.Validate(template));
            result.success = !HasError(result.issues);
            return result;
        }

        private static void ApplyDeterministicNaming(GameObject root)
        {
            if (root == null) return;
            RenameChildren(root.transform);
        }

        private static void RenameChildren(Transform parent)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = parent.GetChild(i);
                string baseName = ExtractBaseName(child.gameObject);
                child.name = $"{baseName}_{i:000}";
                if (child.childCount > 0)
                {
                    RenameChildren(child);
                }
            }
        }

        private static string ExtractBaseName(GameObject go)
        {
            string n = go.name;
            int underscoreIdx = n.LastIndexOf('_');
            if (underscoreIdx > 0 && underscoreIdx < n.Length - 1)
            {
                string suffix = n.Substring(underscoreIdx + 1);
                bool numeric = true;
                foreach (var c in suffix)
                {
                    if (!char.IsDigit(c)) { numeric = false; break; }
                }
                if (numeric) return n.Substring(0, underscoreIdx);
            }
            int parenIdx = n.IndexOf(" (");
            if (parenIdx > 0) return n.Substring(0, parenIdx);
            return n;
        }

        private static void EnsureDirectory(string assetDir)
        {
            if (AssetDatabase.IsValidFolder(assetDir)) return;
            string parent = Path.GetDirectoryName(assetDir)?.Replace('\\', '/') ?? "Assets";
            string leaf = Path.GetFileName(assetDir);
            if (!AssetDatabase.IsValidFolder(parent))
            {
                EnsureDirectory(parent);
            }
            AssetDatabase.CreateFolder(parent, leaf);
        }

        private static bool AssetExists(string path)
        {
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path));
        }

        private static bool HasError(System.Collections.Generic.List<RoomValidationIssue> issues)
        {
            foreach (var issue in issues)
            {
                if (issue.severity == ValidationSeverity.Error) return true;
            }
            return false;
        }
    }
}
#endif
