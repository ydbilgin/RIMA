using System.Collections.Generic;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
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
        public List<GameObject> propInstances = new List<GameObject>();
        public int propsRequested;
        public int propsSpawned;
        public int propsUnresolved;
        public bool hasExitSocket;
        public List<string> diagnostics = new List<string>();
    }

    public class RoomBankRuntimeTester : MonoBehaviour
    {
        public RoomBankSO bank;
        public GameObject playerPrefab;
        public GameObject enemyPlaceholderPrefab;
        public PropRegistrySO propRegistry;
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

            if (picked.backgroundLayers != null && picked.backgroundLayers.Count > 0)
            {
                GameObject bgRoot = new GameObject("PaintedBackground");
                Transform parentTransform = result.roomInstance != null ? result.roomInstance.transform : transform;
                bgRoot.transform.SetParent(parentTransform, false);

                bgRoot.transform.localPosition = new Vector3(
                    picked.bounds.xMin + picked.bounds.width * 0.5f,
                    picked.bounds.yMin + picked.bounds.height * 0.5f,
                    0f
                );

                int spawnedLayers = 0;
                for (int i = 0; i < picked.backgroundLayers.Count; i++)
                {
                    var layer = picked.backgroundLayers[i];
                    if (layer == null || !layer.visible || layer.sprite == null) continue;

                    GameObject layerGO = new GameObject($"Layer_{i:D2}_{layer.layerName}");
                    layerGO.transform.SetParent(bgRoot.transform, false);
                    layerGO.transform.localPosition = new Vector3(layer.offset.x, layer.offset.y, 1f);
                    layerGO.transform.localScale = new Vector3(layer.scale.x, layer.scale.y, 1f);

                    var sr = layerGO.AddComponent<SpriteRenderer>();
                    sr.sprite = layer.sprite;
                    sr.sortingOrder = layer.sortingOrder;
                    sr.color = layer.tint;
                    sr.drawMode = SpriteDrawMode.Simple;
                    spawnedLayers++;
                }
                result.diagnostics.Add($"Painted background: spawned {spawnedLayers}/{picked.backgroundLayers.Count} layers.");
            }
            else
            {
                result.diagnostics.Add("No painted background layers; skipped.");
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

            SpawnProps(picked, result);

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

        private void SpawnProps(RoomTemplateSO picked, RoomTestResult result)
        {
            if (picked.props == null || picked.props.Count == 0)
            {
                result.diagnostics.Add("No props on template; skipped prop spawn.");
                return;
            }
            if (propRegistry == null)
            {
                result.diagnostics.Add($"propRegistry null; skipped {picked.props.Count} prop placements.");
                return;
            }

            Transform parent = result.roomInstance != null ? result.roomInstance.transform : transform;
            PropRuntimeSpawner spawner = new PropRuntimeSpawner();
            PropRuntimeSpawner.SpawnResult spawn = spawner.Spawn(picked, propRegistry, parent);
            result.propsRequested = spawn.requested;
            result.propsSpawned = spawn.spawned;
            result.propsUnresolved = spawn.unresolved;
            if (spawn.instances != null) result.propInstances.AddRange(spawn.instances);
            result.diagnostics.Add($"Props requested={spawn.requested} spawned={spawn.spawned} unresolved={spawn.unresolved}.");
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
