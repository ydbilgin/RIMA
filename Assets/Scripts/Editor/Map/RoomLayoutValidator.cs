#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using RIMA.Map;

namespace RIMA.Editor.Map
{
    public static class RoomLayoutValidator
    {
        private const string SchemaPath = "Assets/Data/Map/Schemas/room_v1.schema.json";

        public static bool Validate(string jsonPath)
        {
            return Validate(jsonPath, FindFirstAsset<MaterialVariantPoolSO>(), FindFirstAsset<WallPrefabRegistry>());
        }

        public static bool Validate(string jsonPath, MaterialVariantPoolSO materialPool, WallPrefabRegistry wallRegistry)
        {
            List<string> errors = new List<string>();
            List<string> warnings = new List<string>();

            string text = ReadText(jsonPath);
            if (string.IsNullOrWhiteSpace(text))
            {
                Debug.LogError("[RoomLayoutValidator] Missing or empty JSON: " + jsonPath);
                return false;
            }

            if (!File.Exists(AssetPathToFullPath(SchemaPath)))
            {
                warnings.Add("Schema file not found: " + SchemaPath);
            }

            RoomLayoutJson room = null;
            try
            {
                room = JsonUtility.FromJson<RoomLayoutJson>(text);
            }
            catch (System.Exception ex)
            {
                errors.Add("JsonUtility parse failed: " + ex.Message);
            }

            if (room == null)
            {
                errors.Add("Parsed room is null.");
            }
            else
            {
                Require(room.schema_version, "schema_version", errors);
                Require(room.room_id, "room_id", errors);
                if (room.width <= 0) errors.Add("width must be positive.");
                if (room.height <= 0) errors.Add("height must be positive.");
                if (room.floor == null) errors.Add("floor is required.");
                else Require(room.floor.default_material, "floor.default_material", errors);

                ValidateMaterials(room, materialPool, errors, warnings);
                ValidateWalls(room, wallRegistry, warnings);
            }

            for (int i = 0; i < warnings.Count; i++)
            {
                Debug.LogWarning("[RoomLayoutValidator] " + warnings[i]);
            }

            if (errors.Count > 0)
            {
                for (int i = 0; i < errors.Count; i++)
                {
                    Debug.LogError("[RoomLayoutValidator] " + errors[i]);
                }

                return false;
            }

            Debug.Log("[RoomLayoutValidator] PASS: " + jsonPath);
            return true;
        }

        private static void ValidateMaterials(RoomLayoutJson room, MaterialVariantPoolSO pool, List<string> errors, List<string> warnings)
        {
            if (pool == null)
            {
                warnings.Add("No MaterialVariantPoolSO found; material references not cross-checked.");
                return;
            }

            HashSet<string> materialIds = new HashSet<string>();
            if (room.floor != null)
            {
                Add(materialIds, room.floor.default_material);
                if (room.floor.zones != null)
                {
                    for (int i = 0; i < room.floor.zones.Length; i++) Add(materialIds, room.floor.zones[i] != null ? room.floor.zones[i].material : null);
                }

                if (room.floor.accents != null)
                {
                    for (int i = 0; i < room.floor.accents.Length; i++) Add(materialIds, room.floor.accents[i] != null ? room.floor.accents[i].material : null);
                }
            }

            foreach (string materialId in materialIds)
            {
                if (pool.GetVariant(materialId, 0) == null) errors.Add("Missing material tile variants for '" + materialId + "'.");
            }
        }

        private static void ValidateWalls(RoomLayoutJson room, WallPrefabRegistry registry, List<string> warnings)
        {
            if (room.walls == null || room.walls.Length == 0) return;
            if (registry == null)
            {
                warnings.Add("No WallPrefabRegistry found; wall prefabs will be skipped.");
                return;
            }

            for (int i = 0; i < room.walls.Length; i++)
            {
                RoomWall wall = room.walls[i];
                if (wall != null && !string.IsNullOrEmpty(wall.prefab) && registry.GetPrefab(wall.prefab) == null)
                {
                    warnings.Add("Missing wall prefab ref: " + wall.prefab);
                }
            }
        }

        private static T FindFirstAsset<T>() where T : Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            if (guids == null || guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        private static void Require(string value, string field, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value)) errors.Add(field + " is required.");
        }

        private static void Add(HashSet<string> set, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) set.Add(value);
        }

        private static string ReadText(string path)
        {
            string fullPath = AssetPathToFullPath(path);
            return File.Exists(fullPath) ? File.ReadAllText(fullPath) : string.Empty;
        }

        private static string AssetPathToFullPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            string normalized = path.Replace('\\', '/');
            if (normalized.StartsWith("Assets/"))
            {
                return Path.Combine(Application.dataPath, normalized.Substring("Assets/".Length));
            }

            return normalized;
        }
    }
}
#endif
