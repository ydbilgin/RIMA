using System.Text;
using RIMA.MapDesigner.Encounter;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor
{
    [CustomEditor(typeof(EncounterTemplateSO))]
    public class EncounterTemplateSOEditor : UnityEditor.Editor
    {
        private ValidationResult lastResult;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawProperty("schemaVersion");
            DrawProperty("encounterId");
            DrawProperty("macroRoomType");
            DrawProperty("biomeId");
            DrawProperty("encounterSeed");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("subRooms"), true);
            DrawProperty("encounterTags");
            DrawProperty("encounterBankKey");

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(8f);
            if (GUILayout.Button("Validate"))
            {
                ValidateAndLog();
            }

            DrawLastResult();
        }

        private void DrawProperty(string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property != null)
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private void ValidateAndLog()
        {
            var encounter = target as EncounterTemplateSO;
            lastResult = EncounterTemplateValidator.Validate(encounter);
            if (lastResult.IsValid)
            {
                Debug.Log($"[Encounter Validator] PASS: {encounter.name}");
                return;
            }

            Debug.Log($"[Encounter Validator] FAIL: {encounter.name} Errors={lastResult.Errors.Count} Warnings={lastResult.Warnings.Count}");
            foreach (string error in lastResult.Errors)
            {
                Debug.LogError($"[Encounter Validator] {encounter.name}: {error}");
            }

            foreach (string warning in lastResult.Warnings)
            {
                Debug.LogWarning($"[Encounter Validator] {encounter.name}: {warning}");
            }
        }

        private void DrawLastResult()
        {
            if (lastResult == null)
            {
                return;
            }

            MessageType type = lastResult.IsValid ? MessageType.Info : MessageType.Error;
            var builder = new StringBuilder();
            builder.Append(lastResult.IsValid ? "Encounter template is valid." : $"Errors: {lastResult.Errors.Count}");

            if (lastResult.Warnings.Count > 0)
            {
                builder.Append($" Warnings: {lastResult.Warnings.Count}");
            }

            foreach (string error in lastResult.Errors)
            {
                builder.AppendLine();
                builder.Append(error);
            }

            foreach (string warning in lastResult.Warnings)
            {
                builder.AppendLine();
                builder.Append(warning);
            }

            EditorGUILayout.HelpBox(builder.ToString(), type);
        }
    }
}
