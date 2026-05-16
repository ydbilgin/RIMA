using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Validation;
using UnityEngine;

namespace RIMA.Tests.Room
{
    public class RoomBankPickTests
    {
        private RoomBankSO bank;
        private List<RoomTemplateSO> created;

        [SetUp]
        public void SetUp()
        {
            bank = ScriptableObject.CreateInstance<RoomBankSO>();
            created = new List<RoomTemplateSO>();
            for (int i = 0; i < 3; i++)
            {
                var room = ScriptableObject.CreateInstance<RoomTemplateSO>();
                room.schemaVersion = "1.0";
                room.roomId = $"combat_test_{i:000}";
                room.biomeId = "TestBiome";
                room.roomType = RIMA.RoomType.Combat;
                room.bounds = new RectInt(0, 0, 10, 10);
                bank.combatRooms.Add(room);
                created.Add(room);
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var r in created)
            {
                if (r != null) Object.DestroyImmediate(r);
            }
            if (bank != null) Object.DestroyImmediate(bank);
        }

        [Test]
        public void Pick_SameSeed_ReturnsSameRoom_TenTimes()
        {
            var first = bank.Pick(RIMA.RoomType.Combat, 42);
            Assert.IsNotNull(first);
            for (int i = 0; i < 10; i++)
            {
                var again = bank.Pick(RIMA.RoomType.Combat, 42);
                Assert.AreSame(first, again, $"Pick iteration {i} returned different room.");
            }
        }

        [Test]
        public void Pick_DifferentSeed_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => bank.Pick(RIMA.RoomType.Combat, 99));
            Assert.DoesNotThrow(() => bank.Pick(RIMA.RoomType.Combat, -5));
            Assert.DoesNotThrow(() => bank.Pick(RIMA.RoomType.Combat, 0));
        }

        [Test]
        public void Pick_EmptyList_ReturnsNull()
        {
            var picked = bank.Pick(RIMA.RoomType.Boss, 42);
            Assert.IsNull(picked);
        }

        [Test]
        public void Pick_OnEliteRoomType_ReturnsFromEliteList()
        {
            var elite = ScriptableObject.CreateInstance<RoomTemplateSO>();
            elite.roomId = "elite_test_001";
            elite.biomeId = "TestBiome";
            elite.roomType = RIMA.RoomType.Elite;
            elite.bounds = new RectInt(0, 0, 12, 12);
            bank.eliteRooms.Add(elite);
            created.Add(elite);

            var picked = bank.Pick(RIMA.RoomType.Elite, 7);
            Assert.AreSame(elite, picked);
        }

        [Test]
        public void ValidateAll_DetectsDuplicateRoomIds()
        {
            var dupe = ScriptableObject.CreateInstance<RoomTemplateSO>();
            dupe.schemaVersion = "1.0";
            dupe.roomId = "combat_test_000";
            dupe.biomeId = "TestBiome";
            dupe.roomType = RIMA.RoomType.Combat;
            dupe.bounds = new RectInt(0, 0, 10, 10);
            bank.combatRooms.Add(dupe);
            created.Add(dupe);

            var issues = bank.ValidateAll();
            bool found = false;
            foreach (var i in issues)
            {
                if (i.code == "ERR_DUPLICATE_ROOM_ID")
                {
                    found = true;
                    Assert.AreEqual(ValidationSeverity.Error, i.severity);
                    break;
                }
            }
            Assert.IsTrue(found, "ValidateAll should report ERR_DUPLICATE_ROOM_ID.");
        }

        [Test]
        public void ValidateAll_DetectsNullRoomRef()
        {
            bank.combatRooms.Add(null);
            var issues = bank.ValidateAll();
            bool found = false;
            foreach (var i in issues)
            {
                if (i.code == "ERR_NULL_ROOM_REF") { found = true; break; }
            }
            Assert.IsTrue(found, "ValidateAll should report ERR_NULL_ROOM_REF.");
        }
    }
}
