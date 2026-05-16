#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Editor;
using RIMA.MapDesigner.Room.Validation;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.Room
{
    public class RoomTemplateSaveLoadTests
    {
        private const string TestRootDir = "Assets/TempTests/Room";
        private const string TestRoomsDir = "Assets/TempTests/Room/Rooms";
        private const string TestBiome = "TempTestBiome";
        private const string TestRoomId = "combat_tempbiome_saveload_001";

        private static string TemplateAssetPath => $"{TestRoomsDir}/{TestBiome}/{TestRoomId}.asset";
        private static string PrefabAssetPath => $"{TestRoomsDir}/{TestBiome}/{TestRoomId}.prefab";

        [TearDown]
        public void Cleanup()
        {
            if (AssetDatabase.IsValidFolder(TestRootDir))
            {
                AssetDatabase.DeleteAsset(TestRootDir);
            }
            AssetDatabase.Refresh();
        }

        [Test]
        public void SaveRoom_PopulatesAllFields_ReloadsIdentical()
        {
            var template = MakePopulatedTemplate();
            var authoringRoot = new GameObject("AuthoringRoot");
            AddChild(authoringRoot, "FloorLayer");
            AddChild(authoringRoot, "Walls");

            try
            {
                var result = RoomTemplateSaver.SaveRoom(authoringRoot, template, true, TestRoomsDir);
                Assert.IsTrue(result.success, "SaveRoom should succeed: " + DescribeIssues(result.issues));
                Assert.IsTrue(AssetDatabase.AssetPathToGUID(TemplateAssetPath).Length > 0, "Template asset GUID must exist after save.");
                Assert.IsTrue(AssetDatabase.AssetPathToGUID(PrefabAssetPath).Length > 0, "Prefab asset GUID must exist after save.");

                var reloaded = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
                Assert.IsNotNull(reloaded, "Reloaded template must not be null.");

                Assert.AreEqual("1.0", reloaded.schemaVersion);
                Assert.AreEqual(TestRoomId, reloaded.roomId);
                Assert.AreEqual(TestBiome, reloaded.biomeId);
                Assert.AreEqual(RIMA.RoomType.Combat, reloaded.roomType);
                Assert.AreEqual(new RectInt(0, 0, 12, 10), reloaded.bounds);

                Assert.IsNotNull(reloaded.playerSpawn, "playerSpawn must roundtrip.");
                Assert.AreEqual("player_spawn_01", reloaded.playerSpawn.socketId);
                Assert.AreEqual(new Vector2Int(2, 2), reloaded.playerSpawn.position);
                Assert.AreEqual(RIMA.DoorDirection.North, reloaded.playerSpawn.facing);

                Assert.IsNotNull(reloaded.doorSockets, "doorSockets list must roundtrip.");
                Assert.AreEqual(1, reloaded.doorSockets.Count);
                var door0 = reloaded.doorSockets[0];
                Assert.AreEqual("door_N_01", door0.socketId);
                Assert.AreEqual(new Vector2Int(6, 9), door0.position);
                Assert.AreEqual(RIMA.DoorDirection.North, door0.direction);
                Assert.AreEqual(2, door0.widthInTiles);
                Assert.IsTrue(door0.isExit);

                Assert.IsNotNull(reloaded.enemySpawnSockets, "enemySpawnSockets list must roundtrip.");
                Assert.AreEqual(2, reloaded.enemySpawnSockets.Count);
                var e0 = reloaded.enemySpawnSockets[0];
                Assert.AreEqual("enemy_spawn_01", e0.socketId);
                Assert.AreEqual(new Vector2Int(6, 5), e0.position);
                Assert.AreEqual("standard", e0.tierHint);
                var e1 = reloaded.enemySpawnSockets[1];
                Assert.AreEqual("enemy_spawn_02", e1.socketId);
                Assert.AreEqual(new Vector2Int(8, 6), e1.position);
                Assert.AreEqual("elite", e1.tierHint);

                Assert.AreEqual(new RectInt(0, 0, 12, 10), reloaded.cameraBounds.tileRect);

                Assert.IsNotNull(reloaded.prefabRef, "prefabRef must be set by SaveRoom.");

                Assert.IsNotNull(reloaded.encounterTags);
                Assert.AreEqual(2, reloaded.encounterTags.Count);
                Assert.Contains("elite_wave", reloaded.encounterTags);
                Assert.Contains("ambush", reloaded.encounterTags);

                Assert.IsNotNull(reloaded.difficultyTags);
                Assert.AreEqual(1, reloaded.difficultyTags.Count);
                Assert.Contains("standard", reloaded.difficultyTags);

                Assert.IsNotNull(reloaded.blockerTags);
                Assert.AreEqual(0, reloaded.blockerTags.Count);
            }
            finally
            {
                if (authoringRoot != null) Object.DestroyImmediate(authoringRoot);
            }
        }

        [Test]
        public void SaveRoom_GuidStable_OnResave()
        {
            var template = MakePopulatedTemplate();
            var authoringRoot = new GameObject("AuthoringRoot_Guid");
            AddChild(authoringRoot, "Walls");

            try
            {
                var first = RoomTemplateSaver.SaveRoom(authoringRoot, template, true, TestRoomsDir);
                Assert.IsTrue(first.success, "First save must succeed: " + DescribeIssues(first.issues));
                string templateGuidBefore = AssetDatabase.AssetPathToGUID(TemplateAssetPath);
                string prefabGuidBefore = AssetDatabase.AssetPathToGUID(PrefabAssetPath);
                Assert.IsFalse(string.IsNullOrEmpty(templateGuidBefore));
                Assert.IsFalse(string.IsNullOrEmpty(prefabGuidBefore));

                var existing = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
                Assert.IsNotNull(existing);
                existing.encounterTags = new List<string> { "ambush", "burst" };

                var second = RoomTemplateSaver.SaveRoom(authoringRoot, existing, true, TestRoomsDir);
                Assert.IsTrue(second.success, "Resave must succeed: " + DescribeIssues(second.issues));
                Assert.IsTrue(second.guidPreserved, "guidPreserved flag must be true on resave.");

                string templateGuidAfter = AssetDatabase.AssetPathToGUID(TemplateAssetPath);
                string prefabGuidAfter = AssetDatabase.AssetPathToGUID(PrefabAssetPath);
                Assert.AreEqual(templateGuidBefore, templateGuidAfter, "Template GUID must remain stable across resave.");
                Assert.AreEqual(prefabGuidBefore, prefabGuidAfter, "Prefab GUID must remain stable across resave.");

                var reloaded = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
                Assert.IsNotNull(reloaded);
                Assert.Contains("ambush", reloaded.encounterTags);
            }
            finally
            {
                if (authoringRoot != null) Object.DestroyImmediate(authoringRoot);
            }
        }

        [Test]
        public void SaveRoom_NoOverwrite_ReturnsErrorWhenExisting()
        {
            var template = MakePopulatedTemplate();
            var authoringRoot = new GameObject("AuthoringRoot_NoOverwrite");
            AddChild(authoringRoot, "Floor");

            try
            {
                var first = RoomTemplateSaver.SaveRoom(authoringRoot, template, true, TestRoomsDir);
                Assert.IsTrue(first.success, "First save must succeed: " + DescribeIssues(first.issues));

                var existing = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
                Assert.IsNotNull(existing);
                var blocked = RoomTemplateSaver.SaveRoom(authoringRoot, existing, false, TestRoomsDir);
                Assert.IsFalse(blocked.success, "SaveRoom must refuse to overwrite when overwriteExisting=false.");
                bool foundCode = false;
                foreach (var i in blocked.issues)
                {
                    if (i.code == "ERR_ASSET_EXISTS") { foundCode = true; break; }
                }
                Assert.IsTrue(foundCode, "Expected ERR_ASSET_EXISTS issue.");
            }
            finally
            {
                if (authoringRoot != null) Object.DestroyImmediate(authoringRoot);
            }
        }

        private static RoomTemplateSO MakePopulatedTemplate()
        {
            var t = ScriptableObject.CreateInstance<RoomTemplateSO>();
            t.schemaVersion = "1.0";
            t.roomId = TestRoomId;
            t.biomeId = TestBiome;
            t.roomType = RIMA.RoomType.Combat;
            t.bounds = new RectInt(0, 0, 12, 10);
            t.playerSpawn = new PlayerSpawnSocket { socketId = "player_spawn_01", position = new Vector2Int(2, 2), facing = DoorDirection.North };
            t.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = "door_N_01", position = new Vector2Int(6, 9), direction = DoorDirection.North, widthInTiles = 2, isExit = true },
            };
            t.enemySpawnSockets = new List<EnemySpawnSocket>
            {
                new EnemySpawnSocket { socketId = "enemy_spawn_01", position = new Vector2Int(6, 5), tierHint = "standard" },
                new EnemySpawnSocket { socketId = "enemy_spawn_02", position = new Vector2Int(8, 6), tierHint = "elite" },
            };
            t.cameraBounds = new CameraBounds { tileRect = new RectInt(0, 0, 12, 10) };
            t.encounterTags = new List<string> { "elite_wave", "ambush" };
            t.difficultyTags = new List<string> { "standard" };
            t.blockerTags = new List<string>();
            return t;
        }

        private static void AddChild(GameObject parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent.transform, false);
        }

        private static string DescribeIssues(List<RoomValidationIssue> issues)
        {
            if (issues == null || issues.Count == 0) return "(no issues)";
            var sb = new System.Text.StringBuilder();
            foreach (var i in issues)
            {
                sb.Append($"[{i.severity}/{i.code}] {i.message}; ");
            }
            return sb.ToString();
        }
    }
}
#endif
