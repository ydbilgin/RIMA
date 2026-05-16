using System.Collections.Generic;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Runtime
{
    public class RoomTestResult
    {
        public bool success;
        public string message;
        public RoomTemplateSO pickedTemplate;
        public GameObject roomInstance;
        public GameObject playerInstance;
        public GameObject enemyInstance;
        public bool hasExitSocket;
        public List<string> diagnostics = new List<string>();
    }

    public class RoomBankRuntimeTester : MonoBehaviour
    {
        public RoomBankSO bank;
        public GameObject playerPrefab;
        public GameObject enemyPlaceholderPrefab;
        public int testSeed = 42;
        public RIMA.RoomType roomTypeToTest = RIMA.RoomType.Combat;

        public RoomTestResult RunTest()
        {
            var result = new RoomTestResult();
            if (bank == null)
            {
                result.message = "RoomBank not assigned.";
                return result;
            }

            var picked = bank.Pick(roomTypeToTest, testSeed);
            if (picked == null)
            {
                result.message = $"RoomBank.Pick({roomTypeToTest}, {testSeed}) returned null.";
                return result;
            }
            result.pickedTemplate = picked;
            result.diagnostics.Add($"Picked {picked.roomId} for {roomTypeToTest}, seed {testSeed}.");

            if (picked.prefabRef != null)
            {
                result.roomInstance = Instantiate(picked.prefabRef);
                result.roomInstance.name = $"Room_{picked.roomId}";
                result.diagnostics.Add("Room prefab instantiated.");
            }
            else
            {
                result.diagnostics.Add("Room prefabRef null; skipped room instantiation.");
            }

            if (picked.playerSpawn == null)
            {
                result.message = "PlayerSpawnSocket missing.";
                return result;
            }

            if (playerPrefab != null)
            {
                Vector3 playerPos = TileToWorld(picked.playerSpawn.position);
                result.playerInstance = Instantiate(playerPrefab, playerPos, Quaternion.identity);
                result.playerInstance.name = $"Player_TestSpawn_{picked.roomId}";
                result.diagnostics.Add($"Player spawned at {playerPos}.");
            }
            else
            {
                result.diagnostics.Add("playerPrefab null; skipped player instantiation.");
            }

            if (enemyPlaceholderPrefab != null && picked.enemySpawnSockets != null && picked.enemySpawnSockets.Count > 0)
            {
                var firstEnemy = picked.enemySpawnSockets[0];
                if (firstEnemy != null)
                {
                    Vector3 enemyPos = TileToWorld(firstEnemy.position);
                    result.enemyInstance = Instantiate(enemyPlaceholderPrefab, enemyPos, Quaternion.identity);
                    result.enemyInstance.name = $"Enemy_TestSpawn_{firstEnemy.socketId}";
                    result.diagnostics.Add($"Enemy spawned at {enemyPos}.");
                }
            }
            else
            {
                result.diagnostics.Add("enemyPlaceholderPrefab null or no enemy sockets.");
            }

            result.hasExitSocket = HasExit(picked);
            if (!result.hasExitSocket)
            {
                result.message = "No exit DoorSocket found.";
                return result;
            }
            result.diagnostics.Add("Exit DoorSocket validated.");

            result.success = true;
            result.message = "OK";
            return result;
        }

        public static Vector3 TileToWorld(Vector2Int tile)
        {
            return new Vector3(tile.x, tile.y, 0f);
        }

        private static bool HasExit(RoomTemplateSO template)
        {
            if (template.doorSockets == null) return false;
            foreach (var d in template.doorSockets)
            {
                if (d != null && d.isExit) return true;
            }
            return false;
        }
    }
}
