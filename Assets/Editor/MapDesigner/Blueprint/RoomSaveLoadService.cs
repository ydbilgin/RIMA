#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public static class RoomSaveLoadService
    {
        public const string DefaultRoomsFolder = "Assets/Data/Blueprint/Rooms";
        public const string CombatRoomV15bAssetPath = DefaultRoomsFolder + "/combat_room_v15b.asset";
        private const string DefaultProfilePath = "Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset";
        private const string V15bScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";
        private const string V15bRootName = "Pro_Redesign_v15b_FullAdjacency_CombatRoom";
        private const string AdjacencyPrefix = "_BlueprintPlaced_adjacency_";

        public static RoomBlueprintSO SaveAsNew(
            BlueprintCanvas canvas,
            BlueprintProfileSO profile,
            int seed,
            string savePath,
            string roomId,
            string displayName)
        {
            string assetPath = NormalizeAssetPath(savePath);
            if (!assetPath.StartsWith("Assets/", StringComparison.Ordinal))
            {
                throw new ArgumentException("Room blueprints must be saved under Assets/.", nameof(savePath));
            }

            if (!assetPath.StartsWith(DefaultRoomsFolder + "/", StringComparison.Ordinal))
            {
                Debug.LogWarning($"[RoomSaveLoadService] Saving room outside default folder: {assetPath}");
            }

            EnsureFolder(Path.GetDirectoryName(assetPath)?.Replace('\\', '/'));

            var room = ScriptableObject.CreateInstance<RoomBlueprintSO>();
            room.roomId = string.IsNullOrWhiteSpace(roomId) ? Path.GetFileNameWithoutExtension(assetPath) : roomId;
            room.displayName = string.IsNullOrWhiteSpace(displayName) ? room.roomId : displayName;
            room.profile = profile;
            room.defaultSeed = seed;
            room.currentSeed = seed;
            room.FromCanvas(canvas);

            AssetDatabase.CreateAsset(room, assetPath);
            EditorUtility.SetDirty(room);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return room;
        }

        public static void Overwrite(RoomBlueprintSO room, BlueprintCanvas canvas, int seed)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            room.currentSeed = seed;
            room.FromCanvas(canvas);
            EditorUtility.SetDirty(room);
            AssetDatabase.SaveAssets();
        }

        public static (BlueprintCanvas canvas, int seed) Load(RoomBlueprintSO room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            return (room.ToCanvas(), room.currentSeed);
        }

        public static IEnumerable<RoomBlueprintSO> ListRoomsInFolder(string folderPath = DefaultRoomsFolder)
        {
            string assetFolder = NormalizeAssetPath(folderPath);
            if (!AssetDatabase.IsValidFolder(assetFolder))
            {
                yield break;
            }

            string[] guids = AssetDatabase.FindAssets("t:RoomBlueprintSO", new[] { assetFolder });
            Array.Sort(guids, StringComparer.Ordinal);
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(path);
                if (room != null)
                {
                    yield return room;
                }
            }
        }

        public static BlueprintCanvas ExtractCanvasFromPlacedChildren(
            Transform root,
            Vector2Int gridSize,
            out int matchedCount,
            out int eligiblePlacedChildren,
            out int totalChildren)
        {
            var canvas = new BlueprintCanvas(gridSize);
            matchedCount = 0;
            eligiblePlacedChildren = 0;
            totalChildren = 0;

            if (root == null)
            {
                return canvas;
            }

            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                if (child == root)
                {
                    continue;
                }

                totalChildren++;
                string childName = child.name;
                if (!childName.StartsWith(AutoPopulator.PlacedPrefix, StringComparison.Ordinal) ||
                    childName.StartsWith(AdjacencyPrefix, StringComparison.Ordinal))
                {
                    continue;
                }

                eligiblePlacedChildren++;
                if (TryParsePlacedName(childName, out string zoneId, out Vector2Int cell))
                {
                    canvas.Paint(cell, zoneId, 1);
                    matchedCount++;
                }
            }

            return canvas;
        }

        public static void GenerateCombatRoomV15bReferenceAsset()
        {
            Scene scene = EditorSceneManager.OpenScene(V15bScenePath, OpenSceneMode.Single);
            if (!scene.IsValid())
            {
                throw new InvalidOperationException($"Failed to open scene: {V15bScenePath}");
            }

            GameObject root = GameObject.Find(V15bRootName);
            if (root == null)
            {
                throw new InvalidOperationException($"Could not find {V15bRootName} in {V15bScenePath}");
            }

            BlueprintProfileSO profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(DefaultProfilePath);
            Vector2Int gridSize = profile != null ? profile.gridSize : new Vector2Int(36, 22);
            BlueprintCanvas canvas = ExtractCanvasFromPlacedChildren(
                root.transform,
                gridSize,
                out int matchedCount,
                out int eligiblePlacedChildren,
                out int totalChildren);

            EnsureFolder(DefaultRoomsFolder);
            if (AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(CombatRoomV15bAssetPath) != null)
            {
                AssetDatabase.DeleteAsset(CombatRoomV15bAssetPath);
            }

            SaveAsNew(canvas, profile, 2026, CombatRoomV15bAssetPath, "combat_room_v15b", "Combat Room v15b");
            Debug.Log(
                $"[RoomSaveLoadService] Generated {CombatRoomV15bAssetPath}; " +
                $"matched={matchedCount}, eligiblePlacedChildren={eligiblePlacedChildren}, totalChildren={totalChildren}, canvasCells={canvas.Count}");
        }

        private static bool TryParsePlacedName(string objectName, out string zoneId, out Vector2Int cell)
        {
            zoneId = null;
            cell = default;
            string payload = objectName.Substring(AutoPopulator.PlacedPrefix.Length);
            string[] parts = payload.Split('_');
            if (parts.Length < 3)
            {
                return false;
            }

            if (!int.TryParse(parts[parts.Length - 2], out int x) ||
                !int.TryParse(parts[parts.Length - 1], out int y))
            {
                return false;
            }

            zoneId = string.Join("_", parts, 0, parts.Length - 2);
            cell = new Vector2Int(x, y);
            return !string.IsNullOrEmpty(zoneId);
        }

        private static string NormalizeAssetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Asset path is required.", nameof(path));
            }

            string normalized = path.Replace('\\', '/');
            string dataPath = Application.dataPath.Replace('\\', '/');
            if (normalized.StartsWith(dataPath, StringComparison.OrdinalIgnoreCase))
            {
                normalized = "Assets" + normalized.Substring(dataPath.Length);
            }

            if (!normalized.StartsWith("Assets", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Path must be under Assets/: {path}", nameof(path));
            }

            return normalized;
        }

        private static void EnsureFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || AssetDatabase.IsValidFolder(folderPath))
            {
                return;
            }

            string[] parts = folderPath.Replace('\\', '/').Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }
    }
}
#endif
