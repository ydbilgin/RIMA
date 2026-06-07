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
        private const string CharSelectPath = "Assets/Data/Rooms/Special/Chamber_CharSelect.asset";
        private const string EchoPedestalPath = "Assets/Data/Props/EchoPedestal.asset";
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
        public void CharSelectRoom_HasOnlyPedestalsAndArchGateProps()
        {
            RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(CharSelectPath);
            Assert.IsNotNull(room);
            Assert.IsNotNull(room.props);

            string echoGuid = AssetDatabase.AssetPathToGUID(EchoPedestalPath);
            string archGuid = AssetDatabase.AssetPathToGUID(ArchGatePath);
            HashSet<string> forbidden = new HashSet<string>
            {
                AssetDatabase.AssetPathToGUID(BrazierPath),
                AssetDatabase.AssetPathToGUID(PillarPath),
                AssetDatabase.AssetPathToGUID(FloorRiftCrackPath)
            };
            HashSet<UnityEngine.Vector2Int> expectedPedestals = new HashSet<UnityEngine.Vector2Int>
            {
                new UnityEngine.Vector2Int(6, 6), new UnityEngine.Vector2Int(8, 8),
                new UnityEngine.Vector2Int(11, 10), new UnityEngine.Vector2Int(14, 11),
                new UnityEngine.Vector2Int(17, 10), new UnityEngine.Vector2Int(19, 8),
                new UnityEngine.Vector2Int(17, 6), new UnityEngine.Vector2Int(14, 5),
                new UnityEngine.Vector2Int(11, 4), new UnityEngine.Vector2Int(8, 4)
            };
            HashSet<UnityEngine.Vector2Int> actualPedestals = new HashSet<UnityEngine.Vector2Int>();
            int archCount = 0;

            foreach (var prop in room.props)
            {
                Assert.IsFalse(forbidden.Contains(prop.propDefinitionGuid), $"Forbidden chamber prop remains: {prop.propDefinitionGuid} at {prop.tilePosition}");
                if (prop.propDefinitionGuid == echoGuid)
                {
                    actualPedestals.Add(prop.tilePosition);
                }
                else if (prop.propDefinitionGuid == archGuid)
                {
                    archCount++;
                    Assert.AreEqual(new UnityEngine.Vector2Int(20, 13), prop.tilePosition);
                }
                else
                {
                    Assert.Fail($"Unexpected chamber prop remains: {prop.propDefinitionGuid} at {prop.tilePosition}");
                }
            }

            Assert.AreEqual(11, room.props.Count);
            Assert.AreEqual(1, archCount);
            CollectionAssert.AreEquivalent(expectedPedestals, actualPedestals);
        }
    }
}
#endif
