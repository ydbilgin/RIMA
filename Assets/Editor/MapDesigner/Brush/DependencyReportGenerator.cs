#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Editor.Utilities
{
    public static class DependencyReportGenerator
    {
        public const string DefaultOutputPath = "STAGING/RIMA_BrushTool_Dependencies.md";

        [MenuItem("RIMA/MapDesigner/Brush/Generate Dependency Report")]
        public static void GenerateReportMenu()
        {
            string report = BuildReport();
            string fullPath = Path.Combine(Application.dataPath, "..", DefaultOutputPath);
            string dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(fullPath, report);
            Debug.Log($"Brush dependency report written to {DefaultOutputPath} ({report.Length} chars)");
        }

        public static string BuildReport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# RIMA Brush Tool Dependency Report");
            sb.AppendLine($"Generated: {System.DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");
            sb.AppendLine();
            AppendScriptableObjectSection(sb, "PropDefinitionSO assets", "t:PropDefinitionSO", "Assets/Data/Brush/Props");
            AppendScriptableObjectSection(sb, "SliceLayoutTemplateSO assets", "t:SliceLayoutTemplateSO", "Assets/Data/Brush/SliceTemplates");
            AppendScriptableObjectSection(sb, "AssetPoolSO assets", "t:AssetPoolSO", "Assets/Art/BrushAtlas/Pools");
            AppendScriptableObjectSection(sb, "RoomTemplateSO assets", "t:RoomTemplateSO", "Assets/Data/Rooms");
            AppendScriptableObjectSection(sb, "RoomBankSO assets", "t:RoomBankSO", "Assets/Data/Rooms");
            AppendScriptableObjectSection(sb, "BrushRadiusProfileSO assets", "t:BrushRadiusProfileSO", "Assets/Data/Brush/SliceTemplates");
            AppendAsmdefSection(sb);
            return sb.ToString();
        }

        private static void AppendScriptableObjectSection(StringBuilder sb, string title, string filter, string folder)
        {
            sb.AppendLine($"## {title}");
            if (!AssetDatabase.IsValidFolder(folder))
            {
                sb.AppendLine($"_Folder missing: {folder}_");
                sb.AppendLine();
                return;
            }
            string[] guids = AssetDatabase.FindAssets(filter, new[] { folder });
            if (guids == null || guids.Length == 0)
            {
                sb.AppendLine($"_No assets matched filter `{filter}` under {folder}._");
                sb.AppendLine();
                return;
            }
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                sb.AppendLine($"- `{path}`");
            }
            sb.AppendLine();
        }

        private static void AppendAsmdefSection(StringBuilder sb)
        {
            sb.AppendLine("## Assembly Definitions");
            string[] guids = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset");
            HashSet<string> mapDesignerAsmdefs = new HashSet<string>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (path.Contains("MapDesigner") || path.Contains("Brush") || path.Contains("RoomDesigner") || path.Contains("Composition") || path.Contains("Props"))
                {
                    mapDesignerAsmdefs.Add(path);
                }
            }
            if (mapDesignerAsmdefs.Count == 0)
            {
                sb.AppendLine("_No MapDesigner asmdef files detected._");
                sb.AppendLine();
                return;
            }
            foreach (string p in mapDesignerAsmdefs)
            {
                sb.AppendLine($"- `{p}`");
            }
            sb.AppendLine();
        }
    }
}
#endif
