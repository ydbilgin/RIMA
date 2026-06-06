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

        [Test]
        public void RoomTemplatesExceptCharSelect_HaveWalkableNorthSpawnSockets()
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

                bool hasNorthExit = false;
                bool hasSouthExit = false;
                if (room.doorSockets != null)
                {
                    for (int socketIndex = 0; socketIndex < room.doorSockets.Count; socketIndex++)
                    {
                        DoorSocket socket = room.doorSockets[socketIndex];
                        if (socket == null || !socket.isExit)
                        {
                            continue;
                        }

                        hasNorthExit |= socket.direction == DoorDirection.North;
                        hasSouthExit |= socket.direction == DoorDirection.South;
                    }
                }

                if (!hasNorthExit)
                {
                    failures.Add($"{path}: missing North exit");
                }

                if (hasSouthExit)
                {
                    failures.Add($"{path}: has South exit");
                }
            }

            Assert.IsEmpty(failures, string.Join("\n", failures));
        }
    }
}
#endif
