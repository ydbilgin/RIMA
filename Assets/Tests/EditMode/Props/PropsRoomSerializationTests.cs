#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Editor;
using RIMA.MapDesigner.Room.Validation;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropsRoomSerializationTests
    {
        private const string TestRootDir = "Assets/TempTests/Props";
        private const string TestRoomsDir = "Assets/TempTests/Props/Rooms";
        private const string TestBiome = "TempPropsBiome";
        private const string TestRoomId = "props_room_001";
        private static string TemplateAssetPath => $"{TestRoomsDir}/{TestBiome}/{TestRoomId}.asset";

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
        public void SaveRoomWithThreeProps_LoadsBack_GuidsPreserved()
        {
            RoomTemplateSO template = MakeTemplate();
            template.props = new List<PropPlacementData>
            {
                new PropPlacementData("guid_a", new Vector2Int(2, 2)),
                new PropPlacementData("guid_b", new Vector2Int(3, 2)),
                new PropPlacementData("guid_c", new Vector2Int(4, 2))
            };

            Save(template);

            RoomTemplateSO reloaded = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
            Assert.AreEqual(3, reloaded.props.Count);
            Assert.AreEqual("guid_a", reloaded.props[0].propDefinitionGuid);
            Assert.AreEqual("guid_b", reloaded.props[1].propDefinitionGuid);
            Assert.AreEqual("guid_c", reloaded.props[2].propDefinitionGuid);
        }

        [Test]
        public void SaveEmptyPropsList_LoadsBack_EmptyList()
        {
            RoomTemplateSO template = MakeTemplate();
            template.props = new List<PropPlacementData>();

            Save(template);

            RoomTemplateSO reloaded = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
            Assert.IsNotNull(reloaded.props);
            Assert.AreEqual(0, reloaded.props.Count);
        }

        [Test]
        public void ModifyPropPosition_Resave_PositionPersists()
        {
            RoomTemplateSO template = MakeTemplate();
            template.props = new List<PropPlacementData>
            {
                new PropPlacementData("guid_a", new Vector2Int(2, 2))
            };

            Save(template);
            RoomTemplateSO existing = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
            existing.props[0].tilePosition = new Vector2Int(5, 5);
            Save(existing);

            RoomTemplateSO reloaded = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(TemplateAssetPath);
            Assert.AreEqual(new Vector2Int(5, 5), reloaded.props[0].tilePosition);
        }

        private static void Save(RoomTemplateSO template)
        {
            GameObject root = new GameObject("PropsRoomRoot");
            try
            {
                var result = RoomTemplateSaver.SaveRoom(root, template, true, TestRoomsDir);
                Assert.IsTrue(result.success, DescribeIssues(result.issues));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static RoomTemplateSO MakeTemplate()
        {
            var template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.roomId = TestRoomId;
            template.biomeId = TestBiome;
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 10, 10);
            template.cameraBounds = new CameraBounds { tileRect = new RectInt(0, 0, 10, 10) };
            template.playerSpawn = new PlayerSpawnSocket { socketId = "player", position = new Vector2Int(1, 1), facing = RIMA.DoorDirection.North };
            template.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = "door", position = new Vector2Int(5, 9), direction = RIMA.DoorDirection.North, widthInTiles = 2, isExit = true }
            };
            template.enemySpawnSockets = new List<EnemySpawnSocket>();
            template.encounterTags = new List<string> { "props_test" };
            template.difficultyTags = new List<string>();
            template.blockerTags = new List<string>();
            return template;
        }

        private static string DescribeIssues(List<RoomValidationIssue> issues)
        {
            if (issues == null || issues.Count == 0) return "(no issues)";
            var sb = new System.Text.StringBuilder();
            foreach (RoomValidationIssue issue in issues)
            {
                sb.Append($"[{issue.severity}/{issue.code}] {issue.message}; ");
            }
            return sb.ToString();
        }
    }
}
#endif
