#if UNITY_EDITOR
using System.Text;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Validation;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Editor
{
    public static class RoomTemplateMenu
    {
        [MenuItem("RIMA/Room/Save Selection as Room Template...")]
        public static void SaveSelectionAsRoom()
        {
            var root = Selection.activeGameObject;
            if (root == null || !root.scene.IsValid())
            {
                EditorUtility.DisplayDialog("Save Room",
                    "Select a scene root GameObject (the room's authoring root) in the Hierarchy first.",
                    "OK");
                return;
            }

            var template = PromptForTemplate("Pick RoomTemplateSO to save into");
            if (template == null) return;

            var result = RoomTemplateSaver.SaveRoom(root, template, overwriteExisting: true);
            ReportResult(result, template);
        }

        [MenuItem("RIMA/Room/Load Template Into Scene...")]
        public static void LoadTemplateIntoScene()
        {
            var template = PromptForTemplate("Pick RoomTemplateSO to load");
            if (template == null) return;

            var result = RoomTemplateLoader.LoadIntoAuthoringScene(template);
            EditorUtility.DisplayDialog("Load Room",
                $"{(result.success ? "OK" : "FAILED")}\n{result.message}",
                "OK");
            if (result.success && result.instance != null)
            {
                Selection.activeGameObject = result.instance;
                EditorGUIUtility.PingObject(result.instance);
            }
        }

        [MenuItem("RIMA/Room/Validate Template")]
        public static void ValidateSelectedTemplate()
        {
            var template = Selection.activeObject as RoomTemplateSO;
            if (template == null)
            {
                EditorUtility.DisplayDialog("Validate Room",
                    "Select a RoomTemplateSO asset in the Project window first.",
                    "OK");
                return;
            }

            var issues = RoomTemplateValidator.Validate(template);
            ReportIssues(issues, template.roomId);
        }

        [MenuItem("RIMA/Room/Validate Bank")]
        public static void ValidateSelectedBank()
        {
            var bank = Selection.activeObject as RoomBankSO;
            if (bank == null)
            {
                EditorUtility.DisplayDialog("Validate Bank",
                    "Select a RoomBankSO asset in the Project window first.",
                    "OK");
                return;
            }

            var issues = bank.ValidateAll();
            ReportIssues(issues, bank.name);
        }

        private static RoomTemplateSO PromptForTemplate(string title)
        {
            string startDir = "Assets/Data/Rooms";
            string assetAbsPath = EditorUtility.OpenFilePanel(title, startDir, "asset");
            if (string.IsNullOrEmpty(assetAbsPath)) return null;

            string assetPath = "Assets" + assetAbsPath.Replace('\\', '/').Substring(Application.dataPath.Replace('\\', '/').Length);
            var template = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(assetPath);
            if (template == null)
            {
                EditorUtility.DisplayDialog("Pick Template",
                    $"Asset at '{assetPath}' is not a RoomTemplateSO.",
                    "OK");
                return null;
            }
            return template;
        }

        private static void ReportResult(SaveResult result, RoomTemplateSO template)
        {
            int errors = 0, warnings = 0;
            foreach (var i in result.issues)
            {
                if (i.severity == ValidationSeverity.Error) errors++;
                else if (i.severity == ValidationSeverity.Warning) warnings++;
                LogIssue(i);
            }
            string status = result.success ? "OK" : "FAILED";
            EditorUtility.DisplayDialog("Save Room",
                $"{status} — {template.roomId}\nPrefab: {result.prefabPath}\nTemplate: {result.templateAssetPath}\nGUID preserved: {result.guidPreserved}\nErrors: {errors} | Warnings: {warnings}",
                "OK");
        }

        private static void ReportIssues(System.Collections.Generic.List<RoomValidationIssue> issues, string subjectId)
        {
            int errors = 0, warnings = 0, infos = 0;
            var sb = new StringBuilder();
            foreach (var i in issues)
            {
                LogIssue(i);
                switch (i.severity)
                {
                    case ValidationSeverity.Error: errors++; break;
                    case ValidationSeverity.Warning: warnings++; break;
                    case ValidationSeverity.Info: infos++; break;
                }
                sb.AppendLine($"[{i.severity}] {i.code}: {i.message}");
            }
            EditorUtility.DisplayDialog($"Validate {subjectId}",
                $"Errors: {errors} | Warnings: {warnings} | Info: {infos}\n\n{sb}",
                "OK");
        }

        private static void LogIssue(RoomValidationIssue issue)
        {
            string line = $"[Room/{issue.code}] {issue.message} (roomId={issue.roomId})";
            switch (issue.severity)
            {
                case ValidationSeverity.Error: Debug.LogError(line); break;
                case ValidationSeverity.Warning: Debug.LogWarning(line); break;
                default: Debug.Log(line); break;
            }
        }
    }
}
#endif
