using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Encounter;
using RIMA.MapDesigner.Room.Data;
using RIMA.Runtime.Encounter;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests
{
    public class SubRoomSequenceControllerTests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [SetUp]
        public void SetUp()
        {
            ResetStaticSingleton(typeof(RuntimeRoomManager), "Instance");
            ResetStaticSingleton(typeof(SubRoomSequenceController), "Active");
        }

        [TearDown]
        public void TearDown()
        {
            foreach (Object obj in cleanup)
            {
                if (obj != null)
                    Object.DestroyImmediate(obj);
            }
            cleanup.Clear();

            foreach (var controller in Object.FindObjectsByType<SubRoomSequenceController>(FindObjectsSortMode.None))
                Object.DestroyImmediate(controller.gameObject);
            foreach (var rrm in Object.FindObjectsByType<RuntimeRoomManager>(FindObjectsSortMode.None))
                Object.DestroyImmediate(rrm.gameObject);
            foreach (var reward in Object.FindObjectsByType<RewardPickup>(FindObjectsSortMode.None))
                Object.DestroyImmediate(reward.gameObject);
            foreach (var bank in Object.FindObjectsByType<TestEncounterBank>(FindObjectsSortMode.None))
                Object.DestroyImmediate(bank.gameObject);

            ResetStaticSingleton(typeof(RuntimeRoomManager), "Instance");
            ResetStaticSingleton(typeof(SubRoomSequenceController), "Active");
            Time.timeScale = 1f;
        }

        [UnityTest]
        public IEnumerator SubRoomSequenceController_FinalClearTriggersReward()
        {
            RuntimeRoomManager rrm = CreateRuntimeRoomManager();
            EncounterTemplateSO template = CreateEncounterTemplate(1);
            GameObject enemyPrefab = CreateEnemyPrefab();
            CreateBank(template, enemyPrefab);

            SubRoomSequenceController controller = CreateController();
            controller.StartEncounter(template);
            RunPrivateEnumerator(controller, "LoadSubRoomCoroutine");
            Assert.AreEqual(SubRoomState.Active, controller.State);

            int rewardCountBefore = CountRewardPickups();
            GameObject killed = new GameObject("KilledEnemy_Final");
            cleanup.Add(killed);
            controller.OnEnemyKilled(killed);
            Assert.IsTrue(rrm.IsRoomCleared, "Final sub-room clear should call RuntimeRoomManager.OnEncounterFinalCleared.");
            RunPrivateEnumerator(rrm, "RoomClearedSequence");
            yield return null;
            yield return null;

            Assert.Greater(CountRewardPickups(), rewardCountBefore, "Final sub-room clear should run the gated reward sequence.");
            Assert.IsNull(SubRoomSequenceController.Active, "Active should be null after encounter completion.");
        }

        [UnityTest]
        public IEnumerator SubRoomSequenceController_NonFinalClearSkipsReward()
        {
            CreateRuntimeRoomManager();
            EncounterTemplateSO template = CreateEncounterTemplate(2);
            GameObject enemyPrefab = CreateEnemyPrefab();
            CreateBank(template, enemyPrefab);

            SubRoomSequenceController controller = CreateController();
            controller.StartEncounter(template);
            RunPrivateEnumerator(controller, "LoadSubRoomCoroutine");
            Assert.AreEqual(SubRoomState.Active, controller.State);

            int rewardCountBefore = CountRewardPickups();
            GameObject killed = new GameObject("KilledEnemy_NonFinal");
            cleanup.Add(killed);
            controller.OnEnemyKilled(killed);
            yield return null;
            yield return null;

            Assert.AreEqual(SubRoomState.Cleared, controller.State, "First sub-room should clear but stay inside the encounter.");
            Assert.AreEqual(rewardCountBefore, CountRewardPickups(), "Non-final sub-room clear must not spawn macro reward.");
            Assert.AreSame(controller, SubRoomSequenceController.Active, "Controller should remain active before final sub-room completion.");
        }

        private RuntimeRoomManager CreateRuntimeRoomManager()
        {
            GameObject go = new GameObject("RuntimeRoomManager_Test");
            cleanup.Add(go);
            RuntimeRoomManager rrm = go.AddComponent<RuntimeRoomManager>();
            rrm.enabled = false;
            SetStaticSingleton(typeof(RuntimeRoomManager), "Instance", rrm);

            GameObject player = new GameObject("Player_Test");
            cleanup.Add(player);
            player.transform.position = Vector3.zero;
            TrySetPlayerTag(player);

            GameObject rewardPrefab = new GameObject("RewardPickupPrefab_Test");
            cleanup.Add(rewardPrefab);
            rewardPrefab.AddComponent<SpriteRenderer>();
            rewardPrefab.AddComponent<RewardPickup>();

            SetPrivateField(rrm, "playerTransform", player.transform);
            SetPrivateField(rrm, "rewardPickupPrefab", rewardPrefab);
            SetPrivateField(rrm, "clearRewardSpawnDelay", 0f);
            SetPrivateField(rrm, "clearSlowdownDuration", 0f);
            SetPrivateField(rrm, "mapFragmentPrefab", null);
            SetPrivateField(rrm, "chestPrefab", null);
            return rrm;
        }

        private SubRoomSequenceController CreateController()
        {
            GameObject go = new GameObject("SubRoomSequenceController_Test");
            cleanup.Add(go);
            return go.AddComponent<SubRoomSequenceController>();
        }

        private void CreateBank(EncounterTemplateSO template, GameObject enemyPrefab)
        {
            GameObject go = new GameObject("TestEncounterBank");
            cleanup.Add(go);
            TestEncounterBank bank = go.AddComponent<TestEncounterBank>();
            bank.Template = template;
            bank.EnemyPrefab = enemyPrefab;
        }

        private GameObject CreateEnemyPrefab()
        {
            GameObject enemyPrefab = new GameObject("EnemyPrefab_Test");
            cleanup.Add(enemyPrefab);
            enemyPrefab.AddComponent<Health>();
            return enemyPrefab;
        }

        private EncounterTemplateSO CreateEncounterTemplate(int subRoomCount)
        {
            EncounterTemplateSO template = ScriptableObject.CreateInstance<EncounterTemplateSO>();
            cleanup.Add(template);
            template.encounterId = "test_encounter";
            template.macroRoomType = RoomType.Combat;

            for (int i = 0; i < subRoomCount; i++)
            {
                RoomTemplateSO room = ScriptableObject.CreateInstance<RoomTemplateSO>();
                cleanup.Add(room);
                room.roomId = $"room_{i}";
                room.bounds = new RectInt(0, 0, 12, 8);
                room.cameraBounds = CameraBounds.FromBounds(room.bounds);
                room.enemySpawnSockets.Add(new EnemySpawnSocket
                {
                    socketId = "enemy_0",
                    position = new Vector2Int(4, 4)
                });
                room.doorSockets.Add(new DoorSocket
                {
                    socketId = "exit",
                    position = new Vector2Int(10, 4),
                    direction = DoorDirection.North,
                    isExit = true
                });
                room.doorSockets.Add(new DoorSocket
                {
                    socketId = "entry",
                    position = new Vector2Int(1, 4),
                    direction = DoorDirection.South,
                    isExit = false
                });

                SubRoomEntry entry = new SubRoomEntry
                {
                    subRoomKey = $"sub_{i}",
                    room = room,
                    isEntry = i == 0,
                    isFinal = i == subRoomCount - 1
                };

                if (i < subRoomCount - 1)
                {
                    entry.links.Add(new SubRoomLink
                    {
                        fromDoorSocketId = "exit",
                        toSubRoomKey = $"sub_{i + 1}",
                        toEntryDoorSocketId = "entry",
                        requiresClear = true
                    });
                }

                template.subRooms.Add(entry);
            }

            return template;
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Missing private field '{fieldName}'.");
            field.SetValue(target, value);
        }

        private static void ResetStaticSingleton(System.Type type, string propertyName)
        {
            FieldInfo field = type.GetField($"<{propertyName}>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, null);
        }

        private static void SetStaticSingleton(System.Type type, string propertyName, object value)
        {
            FieldInfo field = type.GetField($"<{propertyName}>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Missing static property backing field '{propertyName}'.");
            field.SetValue(null, value);
        }

        private static void TrySetPlayerTag(GameObject player)
        {
            try
            {
                player.tag = "Player";
            }
            catch (UnityException)
            {
                // Test projects without the Player tag still exercise the reward gate via injected transform.
            }
        }

        private static int CountRewardPickups()
        {
            return Object.FindObjectsByType<RewardPickup>(FindObjectsSortMode.None).Length;
        }

        private static void RunPrivateEnumerator(object target, string methodName)
        {
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(method, $"Missing private method '{methodName}'.");
            IEnumerator routine = (IEnumerator)method.Invoke(target, null);
            while (routine.MoveNext()) { }
        }

        private sealed class TestEncounterBank : MonoBehaviour, IEncounterBank
        {
            public EncounterTemplateSO Template;
            public GameObject EnemyPrefab;

            public bool TryResolve(RoomNode macroNode, string biomeId, out EncounterTemplateSO template)
            {
                template = Template;
                return template != null;
            }

            public SubRoomEnemyPlan GetSubRoomEnemies(string encounterId, int subRoomIndex)
            {
                SubRoomEnemyPlan plan = new SubRoomEnemyPlan();
                if (Template == null || EnemyPrefab == null) return plan;
                RoomTemplateSO room = Template.subRooms[subRoomIndex].room;
                foreach (EnemySpawnSocket socket in room.enemySpawnSockets)
                {
                    plan.Enemies.Add(new EnemyAssignment
                    {
                        socketId = socket.socketId,
                        enemyPrefab = EnemyPrefab,
                        isElite = false
                    });
                }
                return plan;
            }

            public int GetSubRoomKillQuota(string encounterId, int subRoomIndex)
            {
                return 1;
            }
        }
    }
}

