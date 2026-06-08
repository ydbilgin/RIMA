using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Encounter;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests.Room
{
    /// <summary>
    /// Regression tests for the room-clear → reward → door-open → advance lifecycle.
    /// These guard against the recurring "stuck after room 2" deadlock where missing timeouts
    /// or lifecycle state mismatches left the player with no working exit.
    /// </summary>
    public class RoomLifecycleRegressionTests
    {
        // ── RoomRunLifecycle state-machine happy-path ─────────────────────────────

        [Test]
        public void Lifecycle_FullHappyPath_CompletesWithoutStuck()
        {
            // Simulates: Combat → Cleared → RewardTaken → DoorOpen → Advancing
            // This is the critical path that must work for EVERY room in the run.
            var lc = new RoomRunLifecycle();
            Assert.AreEqual(RoomRunLifecycleState.Idle, lc.State);

            lc.BeginCombat();
            Assert.AreEqual(RoomRunLifecycleState.Combat, lc.State);

            Assert.IsTrue(lc.MarkCleared(), "MarkCleared must succeed from Combat.");
            Assert.AreEqual(RoomRunLifecycleState.Cleared, lc.State);

            Assert.IsTrue(lc.MarkRewardTaken(), "MarkRewardTaken must succeed from Cleared.");
            Assert.AreEqual(RoomRunLifecycleState.RewardTaken, lc.State);

            Assert.IsTrue(lc.MarkDoorsOpened(), "MarkDoorsOpened must succeed from RewardTaken.");
            Assert.AreEqual(RoomRunLifecycleState.DoorOpen, lc.State);

            Assert.IsTrue(lc.MarkAdvancing(), "MarkAdvancing must succeed from DoorOpen.");
            Assert.AreEqual(RoomRunLifecycleState.Advancing, lc.State);
        }

        [Test]
        public void Lifecycle_MultipleRooms_ResetsCorrectlyEachRoom()
        {
            // Simulates the lifecycle being reset via BeginCombat across 3 consecutive rooms,
            // confirming the state machine is not "stuck" in a terminal state between rooms.
            var lc = new RoomRunLifecycle();

            for (int room = 1; room <= 3; room++)
            {
                // BeginCombat is the reset call at the start of each BuildCurrentRoom().
                lc.BeginCombat();
                Assert.AreEqual(RoomRunLifecycleState.Combat, lc.State, $"Room {room}: BeginCombat must reset to Combat.");

                Assert.IsTrue(lc.MarkCleared(), $"Room {room}: MarkCleared must succeed.");
                Assert.IsTrue(lc.MarkRewardTaken(), $"Room {room}: MarkRewardTaken must succeed.");
                Assert.IsTrue(lc.MarkDoorsOpened(), $"Room {room}: MarkDoorsOpened must succeed.");
                Assert.IsTrue(lc.MarkAdvancing(), $"Room {room}: MarkAdvancing must succeed.");

                // At end of room the director calls BuildCurrentRoom → BeginCombat resets the
                // lifecycle to Combat. Verify the cycle is idempotent across rooms.
            }
        }

        [Test]
        public void Lifecycle_MarkCleared_FailsIfNotCombat()
        {
            // Guards against double-firing of HandleEncounterCleared (the if (!MarkCleared()) guard).
            var lc = new RoomRunLifecycle();
            lc.BeginCombat();
            Assert.IsTrue(lc.MarkCleared());
            // Calling again when already Cleared must return false — prevents double clear-sequences.
            Assert.IsFalse(lc.MarkCleared(), "MarkCleared must be idempotent — second call must fail.");
        }

        [Test]
        public void Lifecycle_MarkAdvancing_FailsUnlessDoorOpen()
        {
            // TryEnterDoor calls MarkAdvancing; it must be a no-op if called before doors open
            // (e.g. player walks into a still-closed door collider).
            var lc = new RoomRunLifecycle();
            lc.BeginCombat();
            Assert.IsFalse(lc.MarkAdvancing(), "MarkAdvancing from Combat must fail — door is not open yet.");

            lc.MarkCleared();
            Assert.IsFalse(lc.MarkAdvancing(), "MarkAdvancing from Cleared must fail — reward not taken.");

            lc.MarkRewardTaken();
            Assert.IsFalse(lc.MarkAdvancing(), "MarkAdvancing from RewardTaken must fail — doors not opened yet.");

            lc.MarkDoorsOpened();
            Assert.IsTrue(lc.MarkAdvancing(), "MarkAdvancing from DoorOpen must succeed.");
        }

        // ── EncounterController clears correctly (existing test moved for grouping) ──

        [Test]
        public void EncounterController_NullWave_AutoClears_Regression()
        {
            // Regression: a null wave must never leave the room stuck waiting for enemies.
            GameObject host = new GameObject("EC_NullWave_Regression");
            try
            {
                EncounterController controller = host.AddComponent<EncounterController>();
                bool cleared = false;
                controller.OnRoomCleared.AddListener(() => cleared = true);

                LogAssert.Expect(LogType.Warning, "[EncounterController] No EncounterWaveSO available; treating room as cleared.");
                controller.BeginEncounter(null, new Transform[0], 0f, 0, false);

                Assert.IsTrue(cleared, "Null wave must fire OnRoomCleared immediately.");
                Assert.AreEqual(0, controller.ActiveEnemyCount, "No enemies must remain after null-wave auto-clear.");
            }
            finally
            {
                Object.DestroyImmediate(host);
            }
        }
    }

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
