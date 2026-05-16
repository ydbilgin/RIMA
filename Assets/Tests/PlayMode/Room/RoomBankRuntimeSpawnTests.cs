using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

namespace RIMA.Tests.PlayMode.Room
{
    public class RoomBankRuntimeSpawnTests
    {
        private GameObject testerGo;
        private GameObject playerPrefab;
        private GameObject enemyPrefab;
        private RoomBankSO bank;
        private RoomTemplateSO template;
        private List<GameObject> spawned;

        [SetUp]
        public void SetUp()
        {
            spawned = new List<GameObject>();
            playerPrefab = new GameObject("PlayerPrefabSource");
            enemyPrefab = new GameObject("EnemyPrefabSource");

            template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.schemaVersion = "1.0";
            template.roomId = "combat_playmode_test_001";
            template.biomeId = "PlayModeBiome";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 10, 10);
            template.playerSpawn = new PlayerSpawnSocket
            {
                socketId = "player_spawn_01",
                position = new Vector2Int(3, 4),
                facing = DoorDirection.South,
            };
            template.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = "door_N_01", position = new Vector2Int(5, 9), direction = DoorDirection.North, widthInTiles = 2, isExit = true },
            };
            template.enemySpawnSockets = new List<EnemySpawnSocket>
            {
                new EnemySpawnSocket { socketId = "enemy_spawn_01", position = new Vector2Int(7, 6), tierHint = "standard" },
            };
            template.cameraBounds = new CameraBounds { tileRect = new RectInt(0, 0, 10, 10) };
            template.encounterTags = new List<string> { "basic_wave" };

            bank = ScriptableObject.CreateInstance<RoomBankSO>();
            bank.combatRooms.Add(template);

            testerGo = new GameObject("RoomBankRuntimeTester");
            var tester = testerGo.AddComponent<RoomBankRuntimeTester>();
            tester.bank = bank;
            tester.playerPrefab = playerPrefab;
            tester.enemyPlaceholderPrefab = enemyPrefab;
            tester.testSeed = 42;
            tester.roomTypeToTest = RIMA.RoomType.Combat;
        }

        [TearDown]
        public void TearDown()
        {
            if (testerGo != null) Object.DestroyImmediate(testerGo);
            if (playerPrefab != null) Object.DestroyImmediate(playerPrefab);
            if (enemyPrefab != null) Object.DestroyImmediate(enemyPrefab);
            foreach (var go in spawned)
            {
                if (go != null) Object.DestroyImmediate(go);
            }
            if (template != null) Object.DestroyImmediate(template);
            if (bank != null) Object.DestroyImmediate(bank);
        }

        [UnityTest]
        public IEnumerator RunTest_SpawnsPlayerAndEnemy_AtSocketPositions()
        {
            var tester = testerGo.GetComponent<RoomBankRuntimeTester>();
            var result = tester.RunTest();

            if (result.playerInstance != null) spawned.Add(result.playerInstance);
            if (result.enemyInstance != null) spawned.Add(result.enemyInstance);
            if (result.roomInstance != null) spawned.Add(result.roomInstance);

            Assert.IsTrue(result.success, $"RunTest must succeed. Message: {result.message}; Diagnostics: {string.Join(" | ", result.diagnostics)}");
            Assert.IsNotNull(result.playerInstance, "Player instance must exist.");
            Assert.IsNotNull(result.enemyInstance, "Enemy instance must exist.");
            Assert.IsTrue(result.hasExitSocket, "At least one exit DoorSocket must be present.");

            Vector3 expectedPlayer = new Vector3(3, 4, 0);
            Vector3 expectedEnemy = new Vector3(7, 6, 0);
            Assert.Less(Vector3.Distance(result.playerInstance.transform.position, expectedPlayer), 0.1f,
                $"Player position {result.playerInstance.transform.position} not within 0.1 of {expectedPlayer}.");
            Assert.Less(Vector3.Distance(result.enemyInstance.transform.position, expectedEnemy), 0.1f,
                $"Enemy position {result.enemyInstance.transform.position} not within 0.1 of {expectedEnemy}.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator RunTest_NoExitSocket_Fails()
        {
            template.doorSockets.Clear();
            var tester = testerGo.GetComponent<RoomBankRuntimeTester>();
            var result = tester.RunTest();
            if (result.playerInstance != null) spawned.Add(result.playerInstance);
            if (result.enemyInstance != null) spawned.Add(result.enemyInstance);
            if (result.roomInstance != null) spawned.Add(result.roomInstance);

            Assert.IsFalse(result.success, "RunTest must fail when no exit socket.");
            Assert.IsFalse(result.hasExitSocket);
            yield return null;
        }

        [UnityTest]
        public IEnumerator RunTest_WithProps_SpawnsPropsViaRegistry()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = "playmode_prop";
            prop.footprintSize = new Vector2Int(1, 1);
            prop.blocksWalkable = true;
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            registry.EditorAddProp(prop);
            registry.RebuildIndex();

            template.props = new List<PropPlacementData>
            {
                new PropPlacementData("playmode_prop", new Vector2Int(5, 5)),
                new PropPlacementData("playmode_prop", new Vector2Int(2, 3))
            };

            var tester = testerGo.GetComponent<RoomBankRuntimeTester>();
            tester.propRegistry = registry;
            var result = tester.RunTest();

            if (result.playerInstance != null) spawned.Add(result.playerInstance);
            if (result.enemyInstance != null) spawned.Add(result.enemyInstance);
            if (result.roomInstance != null) spawned.Add(result.roomInstance);
            foreach (var p in result.propInstances) spawned.Add(p);

            try
            {
                Assert.IsTrue(result.success, $"RunTest must succeed. Message: {result.message}");
                Assert.AreEqual(2, result.propsRequested, "Props requested count.");
                Assert.AreEqual(2, result.propsSpawned, "Props spawned count.");
                Assert.AreEqual(0, result.propsUnresolved, "No unresolved props.");
                Assert.AreEqual(2, result.propInstances.Count, "Two prop instances.");
            }
            finally
            {
                Object.DestroyImmediate(registry);
                Object.DestroyImmediate(prop);
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator RunTest_PropsButNullRegistry_LogsDiagnostic()
        {
            template.props = new List<PropPlacementData>
            {
                new PropPlacementData("dummy_guid", new Vector2Int(1, 1))
            };
            var tester = testerGo.GetComponent<RoomBankRuntimeTester>();
            tester.propRegistry = null;
            var result = tester.RunTest();

            if (result.playerInstance != null) spawned.Add(result.playerInstance);
            if (result.enemyInstance != null) spawned.Add(result.enemyInstance);
            if (result.roomInstance != null) spawned.Add(result.roomInstance);

            Assert.IsTrue(result.success, "RunTest succeeds even when registry is null (skipped, not blocked).");
            Assert.AreEqual(0, result.propsSpawned, "No props spawned when registry null.");
            bool hasRegistryDiagnostic = false;
            foreach (string d in result.diagnostics)
            {
                if (d.Contains("propRegistry null"))
                {
                    hasRegistryDiagnostic = true;
                    break;
                }
            }
            Assert.IsTrue(hasRegistryDiagnostic, "Diagnostic should mention null propRegistry.");
            yield return null;
        }
    }
}
