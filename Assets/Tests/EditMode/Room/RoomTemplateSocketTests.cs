#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;

namespace RIMA.Tests.Room
{
    public sealed class RoomTemplateSocketTests
    {
        private const string RoomsRoot = "Assets/Data/Rooms";
        private const string CharSelectPath = "Assets/Resources/ChamberSelect/Chamber_CharSelect.asset";
        private const string ArchGatePath = "Assets/Data/Props/ArchGate.asset";
        private const string BrazierPath = "Assets/Data/Props/Brazier.asset";
        private const string PillarPath = "Assets/Data/Props/Pillar.asset";
        private const string FloorRiftCrackPath = "Assets/Data/Props/FloorRiftCrack.asset";

        [Test]
        public void RoomTemplatesExceptCharSelect_HaveAuthoredNorthExitSlots()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { RoomsRoot });
            List<string> failures = new List<string>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (path == CharSelectPath)
                {
                    continue;
                }

                RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(path);
                if (room == null)
                {
                    continue;
                }

                if (room.playerSpawn == null || !room.IsWalkable(room.playerSpawn.position))
                {
                    failures.Add($"{path}: missing or non-walkable player spawn");
                }

                DoorSocket[] slots = room.ResolveExitSlots();
                int validSlotCount = 0;
                if (room.doorSockets != null)
                {
                    for (int socketIndex = 0; socketIndex < room.doorSockets.Count; socketIndex++)
                    {
                        DoorSocket socket = room.doorSockets[socketIndex];
                        if (socket == null || !socket.isExit)
                        {
                            continue;
                        }

                        if (socket.direction == DoorDirection.South)
                        {
                            failures.Add($"{path}: has South exit at {socket.position}");
                        }
                    }
                }

                for (int slotIndex = 0; slotIndex < slots.Length; slotIndex++)
                {
                    if (slots[slotIndex] != null)
                    {
                        validSlotCount++;
                    }
                }

                if (validSlotCount == 0)
                {
                    failures.Add($"{path}: missing valid authored exit slots");
                }

                if (slots[1] == null)
                {
                    failures.Add($"{path}: missing valid N slot");
                }

                for (int a = 0; a < slots.Length; a++)
                {
                    if (slots[a] == null)
                    {
                        continue;
                    }

                    for (int b = a + 1; b < slots.Length; b++)
                    {
                        if (slots[b] == null)
                        {
                            continue;
                        }

                        if (slots[a].position == slots[b].position)
                        {
                            failures.Add($"{path}: {RoomTemplateSO.ExitSlotLabel(a)} and {RoomTemplateSO.ExitSlotLabel(b)} share {slots[a].position}");
                        }

                        if (UnityEngine.Vector2Int.Distance(slots[a].position, slots[b].position) < 3f)
                        {
                            failures.Add($"{path}: {RoomTemplateSO.ExitSlotLabel(a)} and {RoomTemplateSO.ExitSlotLabel(b)} are less than 3 tiles apart");
                        }
                    }
                }
            }

            Assert.IsEmpty(failures, string.Join("\n", failures));
        }

        [Test]
        public void CharSelectRoom_HasOnlyArchGateProp()
        {
            RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(CharSelectPath);
            Assert.IsNotNull(room);
            Assert.IsNotNull(room.props);

            string archGuid = AssetDatabase.AssetPathToGUID(ArchGatePath);
            HashSet<string> forbidden = new HashSet<string>
            {
                AssetDatabase.AssetPathToGUID("Assets/Data/Props/EchoPedestal.asset"),
                AssetDatabase.AssetPathToGUID(BrazierPath),
                AssetDatabase.AssetPathToGUID(PillarPath),
                AssetDatabase.AssetPathToGUID(FloorRiftCrackPath)
            };
            int archCount = 0;

            foreach (var prop in room.props)
            {
                Assert.IsFalse(forbidden.Contains(prop.propDefinitionGuid), $"Forbidden chamber prop remains: {prop.propDefinitionGuid} at {prop.tilePosition}");
                if (prop.propDefinitionGuid == archGuid)
                {
                    archCount++;
                    Assert.AreEqual(new UnityEngine.Vector2Int(24, 17), prop.tilePosition);
                }
                else
                {
                    Assert.Fail($"Unexpected chamber prop remains: {prop.propDefinitionGuid} at {prop.tilePosition}");
                }
            }

            Assert.AreEqual(1, room.props.Count);
            Assert.AreEqual(1, archCount);
        }

        [Test]
        public void CharSelectRoom_IsLargeTwoArcLayoutWithOpenCenter()
        {
            RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(CharSelectPath);
            Assert.IsNotNull(room);
            Assert.AreEqual(new UnityEngine.RectInt(0, 0, 28, 20), room.bounds);
            Assert.AreEqual(new UnityEngine.RectInt(0, 0, 28, 20), room.cameraBounds.tileRect);
            Assert.AreEqual(new UnityEngine.Vector2Int(3, 3), room.playerSpawn.position);
            Assert.IsTrue(room.IsWalkable(new UnityEngine.Vector2Int(24, 17)), "Rift exit must sit on walkable floor.");

            var centerCorridor = new[]
            {
                new UnityEngine.Vector2Int(12, 8),
                new UnityEngine.Vector2Int(13, 10),
                new UnityEngine.Vector2Int(14, 12),
                new UnityEngine.Vector2Int(13, 14),
                new UnityEngine.Vector2Int(14, 16),
                new UnityEngine.Vector2Int(18, 16),
                new UnityEngine.Vector2Int(22, 17),
                new UnityEngine.Vector2Int(24, 17)
            };

            foreach (var cell in centerCorridor)
            {
                Assert.IsTrue(room.IsWalkable(cell), $"Center corridor cell {cell} must be walkable.");
            }

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Assert.IsTrue(room.IsWalkable(new UnityEngine.Vector2Int(x, y)), $"Chamber floor cell {x},{y} must be walkable.");
                }
            }
        }
    }
}
#endif
