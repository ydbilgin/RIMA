using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Composition
{
    public class CompositionRoleMapGeneratorTests
    {
        private List<RoomTemplateSO> _created;

        [SetUp]
        public void SetUp()
        {
            _created = new List<RoomTemplateSO>();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var r in _created)
            {
                if (r != null) Object.DestroyImmediate(r);
            }
        }

        [Test]
        public void Generate_10x10WithNorthDoor_AssignsExpectedRoles()
        {
            var room = ScriptableObject.CreateInstance<RoomTemplateSO>();
            _created.Add(room);
            room.roomId = "test_10x10";
            room.biomeId = "TestBiome";
            room.roomType = RIMA.RoomType.Combat;
            room.bounds = new RectInt(0, 0, 10, 10);
            room.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = "door_N_01", position = new Vector2Int(5, 9), direction = RIMA.DoorDirection.North, isExit = true },
            };

            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);

            Assert.AreEqual(CompositionRole.DoorSafety, map.GetRoleAt(new Vector2Int(5, 9)),
                "Door socket itself must be DoorSafety.");
            Assert.AreEqual(CompositionRole.DoorSafety, map.GetRoleAt(new Vector2Int(5, 7)),
                "DoorSafety radius 3 includes 2 tiles south of door.");

            Assert.AreEqual(CompositionRole.WallBand, map.GetRoleAt(new Vector2Int(0, 0)),
                "Corner cell at perimeter must be WallBand.");
            Assert.AreEqual(CompositionRole.WallBand, map.GetRoleAt(new Vector2Int(9, 0)),
                "Other perimeter corner must be WallBand.");
            Assert.AreEqual(CompositionRole.WallBand, map.GetRoleAt(new Vector2Int(0, 5)),
                "Mid-left perimeter must be WallBand.");

            Assert.AreEqual(CompositionRole.CleanCenter, map.GetRoleAt(new Vector2Int(5, 4)),
                "Center cell far from door must be CleanCenter.");

            CompositionRole inset1 = map.GetRoleAt(new Vector2Int(1, 1));
            Assert.IsTrue(inset1 == CompositionRole.DecoratedEdge,
                $"Inset-1 cell (1,1) should be DecoratedEdge, was {inset1}.");

            int wallCount = map.CountOfRole(CompositionRole.WallBand);
            Assert.IsTrue(wallCount > 0 && wallCount < 36,
                $"WallBand covers perimeter MINUS door-safety overlap. Got {wallCount}, expected (0, 36) since DoorSafety overrides WallBand at door socket.");
            int doorSafetyCount = map.CountOfRole(CompositionRole.DoorSafety);
            Assert.IsTrue(doorSafetyCount > 0,
                "DoorSafety should cover at least the door cell and immediate neighbors.");
        }

        [Test]
        public void Generate_4x4Minimal_NoDecoratedEdge()
        {
            var room = ScriptableObject.CreateInstance<RoomTemplateSO>();
            _created.Add(room);
            room.roomId = "test_4x4";
            room.biomeId = "TestBiome";
            room.roomType = RIMA.RoomType.Combat;
            room.bounds = new RectInt(0, 0, 4, 4);
            room.doorSockets = new List<DoorSocket>();

            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);

            int decorated = map.CountOfRole(CompositionRole.DecoratedEdge);
            Assert.AreEqual(0, decorated,
                "4x4 room is too small for DecoratedEdge band (gracefully degrades).");

            Assert.AreEqual(CompositionRole.WallBand, map.GetRoleAt(new Vector2Int(0, 0)));
            Assert.AreEqual(CompositionRole.CleanCenter, map.GetRoleAt(new Vector2Int(1, 1)));
        }

        [Test]
        public void Generate_NullRoom_ReturnsEmptyMap()
        {
            var map = CompositionRoleMapGenerator.GenerateFromRoom(null);
            Assert.IsNotNull(map);
            Assert.AreEqual(0, map.bounds.width);
            Assert.AreEqual(0, map.bounds.height);
        }

        [Test]
        public void GetRoleAt_OutOfBounds_ReturnsEmpty()
        {
            var room = ScriptableObject.CreateInstance<RoomTemplateSO>();
            _created.Add(room);
            room.bounds = new RectInt(0, 0, 5, 5);
            room.doorSockets = new List<DoorSocket>();

            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);
            Assert.AreEqual(CompositionRole.Empty, map.GetRoleAt(new Vector2Int(-1, -1)));
            Assert.AreEqual(CompositionRole.Empty, map.GetRoleAt(new Vector2Int(99, 99)));
        }

        [Test]
        public void Generate_DoorSafetyOverridesWallBand()
        {
            var room = ScriptableObject.CreateInstance<RoomTemplateSO>();
            _created.Add(room);
            room.bounds = new RectInt(0, 0, 10, 10);
            room.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = "door_N_01", position = new Vector2Int(5, 9), direction = RIMA.DoorDirection.North, isExit = true },
            };

            var map = CompositionRoleMapGenerator.GenerateFromRoom(room);
            Assert.AreEqual(CompositionRole.DoorSafety, map.GetRoleAt(new Vector2Int(5, 9)),
                "Door position lies on perimeter — DoorSafety must override WallBand at door socket.");
        }
    }
}
