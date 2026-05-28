using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RIMA.Walls.V2;

namespace RIMA.Walls.V2.EditorTools
{
    public static class RoomBuilderTestRunner
    {
        private const string RegistryPath = "Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset";

        // [MenuItem removed — V2 test runner, run from code if needed]
        public static void RunAll()
        {
            var registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
            if (registry == null)
            {
                Debug.LogError("[V2] Registry not found at " + RegistryPath);
                return;
            }

            // Clear existing rooms root if any
            var existing = GameObject.Find("Rooms_Root");
            if (existing != null) Object.DestroyImmediate(existing);

            var rootsHost = new GameObject("Rooms_Root");

            SpawnRoom(rootsHost.transform, registry, "small_rectangle", new Vector3(0, 0, 0),
                new RoomSpec { roomName = "small_rectangle", widthCells = 6, heightCells = 4, shapeType = RoomShapeType.Rectangle, frontEdgeMode = FrontEdgeMode.LowWall });

            SpawnRoom(rootsHost.transform, registry, "diamond_combat", new Vector3(15, 0, 0),
                new RoomSpec { roomName = "diamond_combat", widthCells = 10, heightCells = 8, shapeType = RoomShapeType.Diamond, frontEdgeMode = FrontEdgeMode.Open });

            var alcoveSpec = new RoomSpec { roomName = "alcove_room", widthCells = 8, heightCells = 6, frontEdgeMode = FrontEdgeMode.LowWall };
            alcoveSpec.alcovePositions.Add(new Vector2Int(2, 4));
            alcoveSpec.alcovePositions.Add(new Vector2Int(5, 4));
            SpawnRoom(rootsHost.transform, registry, "alcove_room", new Vector3(32, 0, 0), alcoveSpec);

            var protSpec = new RoomSpec { roomName = "protrusion_room", widthCells = 8, heightCells = 6, frontEdgeMode = FrontEdgeMode.LowWall };
            protSpec.protrusionPositions.Add(new Vector2Int(3, 6));
            protSpec.protrusionPositions.Add(new Vector2Int(4, 6));
            SpawnRoom(rootsHost.transform, registry, "protrusion_room", new Vector3(0, -12, 0), protSpec);

            var doorSpec = new RoomSpec { roomName = "rear_door_room", widthCells = 8, heightCells = 6, frontEdgeMode = FrontEdgeMode.LowWall };
            doorSpec.doorPosition = new Vector2Int(4, 5);
            SpawnRoom(rootsHost.transform, registry, "rear_door_room", new Vector3(15, -12, 0), doorSpec);

            Debug.Log("[V2] Test rooms spawned");
        }

        // [MenuItem removed — V2 test runner, run from code if needed]
        public static void RunPresetRooms()
        {
            var registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
            if (registry == null)
            {
                Debug.LogError("[V2] Registry not found at " + RegistryPath);
                return;
            }

            var existing = GameObject.Find("Rooms_Root");
            if (existing != null) Object.DestroyImmediate(existing);

            var rootsHost = new GameObject("Rooms_Root");

            var lib = new RoomSpec
            {
                roomName = "library_alcove",
                roomPresetId = "library_alcove",
                widthCells = 22,
                heightCells = 22,
                shapeType = RoomShapeType.Rectangle,
                frontEdgeMode = FrontEdgeMode.Open,
                enforceCenteredRearDoor = true,
                doorPosition = new Vector2Int(11, 21),
                nicheSpecs = new List<NicheSpec>
                {
                    new NicheSpec { side = "left", anchorRow = 10, width = 3, depth = 2, mirror = true }
                },
                interiorIslandRects = new List<RectInt>
                {
                    new RectInt(8, 8, 3, 2),
                    new RectInt(13, 10, 2, 2)
                }
            };

            var combat = new RoomSpec
            {
                roomName = "narrow_combat",
                roomPresetId = "narrow_combat",
                widthCells = 22,
                heightCells = 22,
                shapeType = RoomShapeType.Diamond,
                diamondTopWidthCells = 8,
                diamondStepMin = 1,
                diamondStepMax = 1,
                frontEdgeMode = FrontEdgeMode.Open,
                enforceCenteredRearDoor = true,
                doorPosition = new Vector2Int(11, 21),
                frontMinOpeningCells = 4
            };

            var ritual = new RoomSpec
            {
                roomName = "ritual_diamond",
                roomPresetId = "ritual_diamond",
                widthCells = 22,
                heightCells = 22,
                shapeType = RoomShapeType.Diamond,
                diamondTopWidthCells = 6,
                frontEdgeMode = FrontEdgeMode.LowWall,
                reservedCenterRadiusCells = 3,
                enforceCenteredRearDoor = true,
                doorPosition = new Vector2Int(11, 21)
            };

            var flooded = new RoomSpec
            {
                roomName = "open_flooded",
                roomPresetId = "open_flooded",
                widthCells = 22,
                heightCells = 16,
                shapeType = RoomShapeType.Rectangle,
                frontEdgeMode = FrontEdgeMode.Open,
                waterPoolRects = new List<RectInt>
                {
                    new RectInt(2, 4, 3, 4),
                    new RectInt(17, 4, 3, 4)
                }
            };

            SpawnRoom(rootsHost.transform, registry, lib.roomName, new Vector3(0, 0, 0), lib);
            SpawnRoom(rootsHost.transform, registry, combat.roomName, new Vector3(25, 0, 0), combat);
            SpawnRoom(rootsHost.transform, registry, ritual.roomName, new Vector3(50, 0, 0), ritual);
            SpawnRoom(rootsHost.transform, registry, flooded.roomName, new Vector3(0, -25, 0), flooded);

            Debug.Log("[V2] Guide preset rooms spawned");
        }

        static void SpawnRoom(Transform parent, WallPieceRegistry registry, string name, Vector3 origin, RoomSpec spec)
        {
            var go = new GameObject("Room_" + name);
            go.transform.SetParent(parent, false);
            go.transform.position = origin;
            var builder = go.AddComponent<WallChainRoomBuilder>();
            builder.registry = registry;
            var inner = new GameObject("Pieces");
            inner.transform.SetParent(go.transform, false);
            builder.roomParent = inner.transform;
            builder.cellSize = 1f;
            builder.Build(spec);
        }
    }
}
