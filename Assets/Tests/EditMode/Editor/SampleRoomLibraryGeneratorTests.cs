#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.Editor
{
    public sealed class SampleRoomLibraryGeneratorTests
    {
        private const string MenuPath = "RIMA/MapDesigner/Brush/Generate Sample Library v1";
        private const string LibraryPath = "Assets/Data/Rooms/Library";

        private static readonly Dictionary<string, ExpectedRoom> ExpectedRooms = new Dictionary<string, ExpectedRoom>
        {
            { "Spawn_01", new ExpectedRoom(new RectInt(0, 0, 8, 6), 1) },
            { "Corridor_Linear_01", new ExpectedRoom(new RectInt(0, 0, 12, 4), 2) },
            { "Corridor_LShape_01", new ExpectedRoom(new RectInt(0, 0, 10, 8), 2) },
            { "Combat_Small_01", new ExpectedRoom(new RectInt(0, 0, 8, 6), 2) },
            { "Combat_Medium_01", new ExpectedRoom(new RectInt(0, 0, 12, 8), 3) },
            { "Combat_Large_01", new ExpectedRoom(new RectInt(0, 0, 16, 10), 4) },
            { "Elite_01", new ExpectedRoom(new RectInt(0, 0, 10, 8), 2) },
            { "Treasure_01", new ExpectedRoom(new RectInt(0, 0, 6, 6), 1) },
            { "Shrine_01", new ExpectedRoom(new RectInt(0, 0, 8, 8), 2) },
            { "Boss_Intro_01", new ExpectedRoom(new RectInt(0, 0, 14, 10), 2) }
        };

        [Test]
        public void GenerateLibrary_Creates10Templates_AllSerialize()
        {
            GenerateLibrary();

            string[] roomGuids = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { LibraryPath });
            Assert.AreEqual(10, roomGuids.Length, "Sample room library should contain exactly 10 RoomTemplateSO assets.");

            foreach (KeyValuePair<string, ExpectedRoom> expected in ExpectedRooms)
            {
                RoomTemplateSO room = LoadRoom(expected.Key);
                Assert.IsNotNull(room, $"{expected.Key} should load.");
                Assert.AreEqual(expected.Key, room.roomId);
                Assert.AreEqual("ShatteredKeep", room.biomeId);
                Assert.AreEqual(expected.Value.Bounds, room.bounds);
                Assert.IsNotNull(room.walkableGrid, $"{expected.Key} should have a walkable grid.");
                Assert.AreEqual(room.bounds.width * room.bounds.height, room.walkableGrid.Length, $"{expected.Key} walkable grid size should match bounds.");
                Assert.IsNotNull(room.doorSockets, $"{expected.Key} should have door sockets.");
                Assert.AreEqual(expected.Value.DoorCount, room.doorSockets.Count, $"{expected.Key} door count mismatch.");
            }
        }

        [Test]
        public void Room1_Spawn_HasPlayerSpawnSocket()
        {
            GenerateLibrary();

            RoomTemplateSO room = LoadRoom("Spawn_01");
            Assert.IsNotNull(room);
            Assert.IsNotNull(room.playerSpawn);
            Assert.AreEqual(new Vector2Int(2, 3), room.playerSpawn.position);
            Assert.AreEqual(DoorDirection.East, room.playerSpawn.facing);
        }

        [Test]
        public void Room10_BossIntro_HasBossSpawn()
        {
            GenerateLibrary();

            RoomTemplateSO room = LoadRoom("Boss_Intro_01");
            Assert.IsNotNull(room);
            Assert.IsNotNull(room.enemySpawnSockets);
            Assert.IsTrue(room.enemySpawnSockets.Exists(spawn => spawn.tierHint == "boss" && spawn.position == new Vector2Int(6, 6)));
        }

        private static void GenerateLibrary()
        {
            bool executed = EditorApplication.ExecuteMenuItem(MenuPath);
            Assert.IsTrue(executed, $"Menu item should execute: {MenuPath}");
            AssetDatabase.Refresh();
        }

        private static RoomTemplateSO LoadRoom(string roomId)
        {
            return AssetDatabase.LoadAssetAtPath<RoomTemplateSO>($"{LibraryPath}/{roomId}.asset");
        }

        private readonly struct ExpectedRoom
        {
            public readonly RectInt Bounds;
            public readonly int DoorCount;

            public ExpectedRoom(RectInt bounds, int doorCount)
            {
                Bounds = bounds;
                DoorCount = doorCount;
            }
        }
    }
}
#endif
