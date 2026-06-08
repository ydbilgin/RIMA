using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Encounter;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests.Room
{
    public class RoomCompletionInvariantTests
    {
        private const string DemoRoomBankPath = "Assets/Data/Rooms/DemoRoomBank.asset";

        private static readonly string[] RoomTemplateSearchFolders =
        {
            "Assets/Data/Rooms/Generated",
            "Assets/Data/Rooms/Library"
        };

        [Test]
        public void DemoRoomBank_AllTemplatesAreCompletable()
        {
            RoomBankSO bank = AssetDatabase.LoadAssetAtPath<RoomBankSO>(DemoRoomBankPath);
            Assert.IsNotNull(bank, $"Missing room bank at {DemoRoomBankPath}.");

            int checkedCount = 0;
            checkedCount += AssertRoomListCompletable("combatRooms", bank.combatRooms);
            checkedCount += AssertRoomListCompletable("eliteRooms", bank.eliteRooms);
            checkedCount += AssertRoomListCompletable("bossRooms", bank.bossRooms);
            checkedCount += AssertRoomListCompletable("merchantRooms", bank.merchantRooms);
            checkedCount += AssertRoomListCompletable("eventRooms", bank.eventRooms);

            Assert.Greater(checkedCount, 0, $"{DemoRoomBankPath} contains no room templates.");
        }

        [Test]
        public void GeneratedAndLibraryRoomTemplates_AreCompletable()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoomTemplateSO", RoomTemplateSearchFolders);
            Assert.Greater(guids.Length, 0, "No RoomTemplateSO assets found in generated/library room folders.");

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                RoomTemplateSO template = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(path);
                AssertCompletable(template, path);
            }
        }

        [Test]
        public void EncounterController_NullWave_AutoClears()
        {
            GameObject host = new GameObject("EncounterController_NullWave_AutoClears");
            try
            {
                EncounterController controller = host.AddComponent<EncounterController>();
                bool cleared = false;
                controller.OnRoomCleared.AddListener(() => cleared = true);

                LogAssert.Expect(LogType.Warning, "[EncounterController] No EncounterWaveSO available; treating room as cleared.");
                controller.BeginEncounter(null, new Transform[0], 0f, 0, false);

                Assert.IsTrue(cleared, "A null/empty encounter wave must auto-clear instead of leaving the room pending.");
                Assert.AreEqual(0, controller.ActiveEnemyCount);
            }
            finally
            {
                Object.DestroyImmediate(host);
            }
        }

        private static int AssertRoomListCompletable(string listName, List<RoomTemplateSO> templates)
        {
            if (templates == null)
            {
                return 0;
            }

            for (int i = 0; i < templates.Count; i++)
            {
                AssertCompletable(templates[i], $"{DemoRoomBankPath}:{listName}[{i}]");
            }

            return templates.Count;
        }

        private static void AssertCompletable(RoomTemplateSO template, string label)
        {
            Assert.IsNotNull(template, $"{label}: null RoomTemplateSO reference.");

            int enemyCount = template.enemySpawnSockets != null ? template.enemySpawnSockets.Count : 0;
            int exitSlots = template.ValidExitSlotCount;
            string path = AssetDatabase.GetAssetPath(template);
            string roomId = string.IsNullOrEmpty(template.roomId) ? template.name : template.roomId;

            Assert.GreaterOrEqual(exitSlots, 1, $"{label}: {roomId} has no valid exit slot.");

            if (enemyCount > 0)
            {
                TestContext.Out.WriteLine($"PASS {roomId} ({template.roomType}) enemies={enemyCount} exits={exitSlots} path={path}");
                return;
            }

            TestContext.Out.WriteLine($"PASS {roomId} ({template.roomType}) enemyless-auto-clear exits={exitSlots} path={path}");
        }
    }
}
