using System.Collections.Generic;
using System.IO;
using System.Linq;
using RIMA.MapDesigner.Encounter;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor
{
    public static class EncounterMenu
    {
        [MenuItem("Tools/RIMA/Validate Encounter")]
        public static void ValidateSelectedEncounter()
        {
            var encounter = Selection.activeObject as EncounterTemplateSO;
            if (encounter == null)
            {
                Debug.LogWarning("[Encounter Validator] Select one EncounterTemplateSO asset.");
                return;
            }

            ValidationResult result = EncounterTemplateValidator.Validate(encounter);
            LogResult(encounter, AssetDatabase.GetAssetPath(encounter), result);
        }

        [MenuItem("Tools/RIMA/Validate Encounter", true)]
        private static bool ValidateSelectedEncounterEnabled()
        {
            return Selection.activeObject is EncounterTemplateSO;
        }

        [MenuItem("Tools/RIMA/Validate All Encounters")]
        public static void ValidateAllEncounters()
        {
            string[] guids = AssetDatabase.FindAssets("t:EncounterTemplateSO");
            int totalErrors = 0;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var encounter = AssetDatabase.LoadAssetAtPath<EncounterTemplateSO>(path);
                if (encounter == null)
                {
                    continue;
                }

                ValidationResult result = EncounterTemplateValidator.Validate(encounter);
                totalErrors += result.Errors.Count;
                LogResult(encounter, path, result);
            }

            Debug.Log($"[Encounter Validator] Validated {guids.Length} encounter assets. Total errors: {totalErrors}");
        }

        [MenuItem("Tools/RIMA/Encounter/Build From Selected Room Templates")]
        public static void BuildFromSelectedRoomTemplates()
        {
            List<RoomTemplateSO> rooms = Selection.objects.OfType<RoomTemplateSO>().ToList();
            if (rooms.Count < 3 || rooms.Count > 5)
            {
                Debug.LogWarning($"[Encounter Builder] Select 3-5 RoomTemplateSO assets. Selected: {rooms.Count}");
                return;
            }

            var encounter = ScriptableObject.CreateInstance<EncounterTemplateSO>();
            encounter.encounterId = $"encounter_{System.DateTime.UtcNow:yyyyMMdd_HHmmss}";
            encounter.macroRoomType = RIMA.RoomType.Combat;
            encounter.biomeId = rooms[0] != null ? rooms[0].biomeId : string.Empty;
            encounter.encounterSeed = Random.Range(int.MinValue, int.MaxValue);
            encounter.encounterBankKey = encounter.encounterId;

            for (int i = 0; i < rooms.Count; i++)
            {
                RoomTemplateSO room = rooms[i];
                var entry = new SubRoomEntry
                {
                    subRoomKey = i == 0 ? "entry" : i == rooms.Count - 1 ? "final" : $"sub_{i}",
                    room = room,
                    isEntry = i == 0,
                    isFinal = i == rooms.Count - 1,
                    subRoomTag = i == 0 ? "warmup" : i == rooms.Count - 1 ? "final_mix" : "connector",
                    links = new List<SubRoomLink>()
                };

                if (i < rooms.Count - 1)
                {
                    entry.links.Add(new SubRoomLink
                    {
                        fromDoorSocketId = FirstSocketId(room),
                        toSubRoomKey = i == rooms.Count - 2 ? "final" : $"sub_{i + 1}",
                        toEntryDoorSocketId = FirstSocketId(rooms[i + 1]),
                        requiresClear = true
                    });
                }

                encounter.subRooms.Add(entry);
            }

            string folder = "Assets/ScriptableObjects/Encounters";
            EnsureFolder(folder);
            string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{encounter.encounterId}.asset");
            AssetDatabase.CreateAsset(encounter, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = encounter;
            Debug.Log($"[Encounter Builder] Created EncounterTemplateSO at {path}");
        }

        [MenuItem("Tools/RIMA/Encounter/Build From Selected Room Templates", true)]
        private static bool BuildFromSelectedRoomTemplatesEnabled()
        {
            int count = Selection.objects.OfType<RoomTemplateSO>().Count();
            return count >= 3 && count <= 5;
        }

        private static void LogResult(EncounterTemplateSO encounter, string path, ValidationResult result)
        {
            string prefix = result.IsValid ? "PASS" : "FAIL";
            Debug.Log($"[Encounter Validator] {prefix}: {encounter.name} ({path}) Errors={result.Errors.Count} Warnings={result.Warnings.Count}");

            foreach (string error in result.Errors)
            {
                Debug.LogError($"[Encounter Validator] {encounter.name}: {error}");
            }

            foreach (string warning in result.Warnings)
            {
                Debug.LogWarning($"[Encounter Validator] {encounter.name}: {warning}");
            }
        }

        private static string FirstSocketId(RoomTemplateSO room)
        {
            if (room == null || room.doorSockets == null)
            {
                return string.Empty;
            }

            DoorSocket socket = room.doorSockets.FirstOrDefault(s => s != null && !string.IsNullOrWhiteSpace(s.socketId));
            return socket == null ? string.Empty : socket.socketId;
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder))
            {
                return;
            }

            string parent = Path.GetDirectoryName(folder)?.Replace('\\', '/');
            string leaf = Path.GetFileName(folder);
            if (!string.IsNullOrEmpty(parent))
            {
                EnsureFolder(parent);
                AssetDatabase.CreateFolder(parent, leaf);
            }
        }
    }
}
