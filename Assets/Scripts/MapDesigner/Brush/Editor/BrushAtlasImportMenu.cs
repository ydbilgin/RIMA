#if UNITY_EDITOR
using System.IO;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Import.Editor;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Editor
{
    public static class BrushAtlasImportMenu
    {
        [MenuItem("RIMA/Brush/Import Atlas...")]
        public static void OpenImportDialog()
        {
            string startDir = Path.Combine(Application.dataPath, "Art/BrushAtlas/Intake");
            if (!Directory.Exists(startDir))
            {
                startDir = Application.dataPath;
            }

            string pngPath = EditorUtility.OpenFilePanel("Select master PNG", startDir, "png");
            if (string.IsNullOrEmpty(pngPath))
            {
                return;
            }

            string assetPath = BrushAtlasImporter.ToAssetPath(pngPath);
            if (string.IsNullOrEmpty(assetPath) || !assetPath.StartsWith("Assets/"))
            {
                EditorUtility.DisplayDialog("Brush Atlas Import",
                    "The selected PNG must live under the Assets/ folder. Move the file into Assets/ first.",
                    "OK");
                return;
            }

            string poolName = Path.GetFileNameWithoutExtension(pngPath);
            SliceLayoutTemplateSO template = PromptForTemplate();
            if (template == null)
            {
                return;
            }

            TargetLayer layer = template.wangAware ? TargetLayer.L3 : TargetLayer.L4;
            var result = BrushAtlasImporter.Import(assetPath, template, poolName, layer);
            ReportResult(result, poolName, template);
        }

        [MenuItem("RIMA/Brush/Validate Sorting Layers")]
        public static void ValidateSortingLayers()
        {
            // RimaSortingLayerValidator runs via [InitializeOnLoad] (RIMA.Editor asmdef, see Assets/Editor/RimaSortingLayerValidator.cs).
            // Manual revalidation: trigger a recompile so the static ctor runs again.
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            EditorUtility.DisplayDialog("Sorting Layers",
                "Requested script recompilation. RimaSortingLayerValidator will run on the next domain reload and ensure Patch / Scatter / Detail / Accent / Props / Entities.\n\nCheck Project Settings → Tags & Layers to confirm.",
                "OK");
        }

        private static SliceLayoutTemplateSO PromptForTemplate()
        {
            string[] guids = AssetDatabase.FindAssets("t:SliceLayoutTemplateSO");
            if (guids == null || guids.Length == 0)
            {
                bool make = EditorUtility.DisplayDialog("No Templates",
                    "No SliceLayoutTemplateSO assets found. Create defaults now (L3 Wang + L4 + L5 + L6)?",
                    "Create defaults", "Cancel");
                if (!make)
                {
                    return null;
                }
                SliceTemplateFactory.CreateAll();
                guids = AssetDatabase.FindAssets("t:SliceLayoutTemplateSO");
                if (guids == null || guids.Length == 0)
                {
                    return null;
                }
            }

            if (guids.Length == 1)
            {
                return AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }

            // V1 simple selection: prefer Wang-aware if any
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var t = AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(path);
                if (t != null && t.wangAware) return t;
            }
            return AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        private static void ReportResult(ImportResult result, string poolName, SliceLayoutTemplateSO template)
        {
            int errors = 0, warnings = 0, infos = 0;
            foreach (var issue in result.issues)
            {
                switch (issue.severity)
                {
                    case ValidationIssueSeverity.Error:
                        errors++;
                        Debug.LogError($"[BrushAtlasImporter] {issue.code}: {issue.message}");
                        break;
                    case ValidationIssueSeverity.Warning:
                        warnings++;
                        Debug.LogWarning($"[BrushAtlasImporter] {issue.code}: {issue.message}");
                        break;
                    default:
                        infos++;
                        Debug.Log($"[BrushAtlasImporter] {issue.code}: {issue.message}");
                        break;
                }
            }

            string status = result.Success ? "OK" : "FAILED";
            string msg = $"Import {status}: pool '{poolName}', template '{(template ? template.templateName : "?")}'\n" +
                         $"Variants: {result.variantCount}\nErrors: {errors} | Warnings: {warnings} | Info: {infos}";
            EditorUtility.DisplayDialog("Brush Atlas Import", msg, "OK");
        }
    }
}
#endif
