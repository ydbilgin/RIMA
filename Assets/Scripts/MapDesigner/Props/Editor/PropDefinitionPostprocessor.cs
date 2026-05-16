#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Props.Editor
{
    public sealed class PropDefinitionPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            for (int i = 0; i < importedAssets.Length; i++)
            {
                EnsurePropIdForAsset(importedAssets[i]);
            }
            for (int i = 0; i < movedAssets.Length; i++)
            {
                EnsurePropIdForAsset(movedAssets[i]);
            }
        }

        private static void EnsurePropIdForAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return;
            if (!assetPath.EndsWith(".asset")) return;

            PropDefinitionSO prop = AssetDatabase.LoadAssetAtPath<PropDefinitionSO>(assetPath);
            if (prop == null) return;

            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid)) return;

            if (prop.propId != guid)
            {
                prop.propId = guid;
                EditorUtility.SetDirty(prop);
            }
        }
    }
}
#endif
