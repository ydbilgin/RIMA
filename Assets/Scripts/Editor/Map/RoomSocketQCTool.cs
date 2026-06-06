#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using RIMA;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.Map
{
    public static class RoomSocketQCTool
    {
        private const string RoomsRoot = "Assets/Data/Rooms";
        private const string CharSelectPath = "Assets/Data/Rooms/Special/Chamber_CharSelect.asset";
        private const int DoorWidth = 2;
        private const float MinimumSpawnDoorDistance = 4f;

        [MenuItem("RIMA/Rooms/QC/Audit Sockets")]
        public static void AuditSockets()
        {
            List<RoomTemplateSO> rooms = LoadRoomTemplates();
            int issueCount = 0;
            StringBuilder report = new StringBuilder();
            report.AppendLine("[RoomSocketQC] Audit Sockets");
            report.AppendLine($"templates={rooms.Count}");

            for (int i = 0; i < rooms.Count; i++)
            {
                RoomTemplateSO room = rooms[i];
                List<string> issues = CollectIssues(room);
                issueCount += issues.Count;
                report.AppendLine($"{Label(room)}: issues={issues.Count}");
                for (int issueIndex = 0; issueIndex < issues.Count; issueIndex++)
                {
                    report.AppendLine($"  - {issues[issueIndex]}");
                }
            }

            report.AppendLine($"totalIssues={issueCount}");
            if (issueCount == 0)
            {
                Debug.Log(report.ToString());
            }
            else
            {
                Debug.LogWarning(report.ToString());
            }
        }

        [MenuItem("RIMA/Rooms/QC/Fix Sockets")]
        public static void FixSockets()
        {
            List<RoomTemplateSO> rooms = LoadRoomTemplates();
            int changed = 0;
            int skipped = 0;
            List<string> changedPaths = new List<string>();
            StringBuilder report = new StringBuilder();
            report.AppendLine("[RoomSocketQC] Fix Sockets");
            report.AppendLine($"templates={rooms.Count}");

            AssetDatabase.StartAssetEditing();
            try
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    RoomTemplateSO room = rooms[i];
                    string path = AssetDatabase.GetAssetPath(room);
                    if (path == CharSelectPath)
                    {
                        skipped++;
                        report.AppendLine($"{Label(room)}: skipped hand-authored special room");
                        continue;
                    }

                    if (!TryBuildFixedSockets(room, out PlayerSpawnSocket spawn, out List<DoorSocket> doors, out string reason))
                    {
                        skipped++;
                        report.AppendLine($"{Label(room)}: skipped {reason}");
                        continue;
                    }

                    Undo.RecordObject(room, "Fix room spawn and door sockets");
                    room.playerSpawn = spawn;
                    room.doorSockets = doors;
                    EditorUtility.SetDirty(room);
                    changedPaths.Add(path);
                    changed++;
                    report.AppendLine($"{Label(room)}: spawn={spawn.position}, doors={doors.Count}");
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
            if (changedPaths.Count > 0)
            {
                AssetDatabase.ForceReserializeAssets(changedPaths);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            report.AppendLine($"changed={changed}, skipped={skipped}");
            Debug.Log(report.ToString());
        }

        private static List<string> CollectIssues(RoomTemplateSO room)
        {
            List<string> issues = new List<string>();

            if (room.playerSpawn == null)
            {
                issues.Add("missing playerSpawn");
            }
            else
            {
                if (!room.IsWalkable(room.playerSpawn.position))
                {
                    issues.Add($"playerSpawn not walkable at {room.playerSpawn.position}");
                }

                if (room.doorSockets != null)
                {
                    for (int i = 0; i < room.doorSockets.Count; i++)
                    {
                        DoorSocket socket = room.doorSockets[i];
                        if (socket != null && socket.isExit && Vector2Int.Distance(room.playerSpawn.position, socket.position) < MinimumSpawnDoorDistance)
                        {
                            issues.Add($"playerSpawn <4 tiles from exit socket '{socket.socketId}' at {socket.position}");
                        }
                    }
                }
            }

            bool hasExit = false;
            bool hasNorthExit = false;
            if (room.doorSockets != null)
            {
                for (int i = 0; i < room.doorSockets.Count; i++)
                {
                    DoorSocket socket = room.doorSockets[i];
                    if (socket == null)
                    {
                        continue;
                    }

                    if (socket.isExit)
                    {
                        hasExit = true;
                        if (socket.direction == DoorDirection.North)
                        {
                            hasNorthExit = true;
                        }

                        if (socket.direction == DoorDirection.South)
                        {
                            issues.Add($"South exit forbidden at {socket.position}");
                        }
                    }

                    if (!IsDoorEdge(room, socket.position, socket.direction))
                    {
                        issues.Add($"socket '{socket.socketId}' is not a walkable {socket.direction} edge cell at {socket.position}");
                    }
                }
            }

            if (!hasExit)
            {
                issues.Add("missing exit sockets");
            }

            if (!hasNorthExit)
            {
                issues.Add("missing North exit socket");
            }

            return issues;
        }

        private static bool TryBuildFixedSockets(RoomTemplateSO room, out PlayerSpawnSocket spawn, out List<DoorSocket> doors, out string reason)
        {
            spawn = null;
            doors = new List<DoorSocket>();
            reason = string.Empty;

            if (!TryFindEdgeCell(room, DoorDirection.North, out Vector2Int north))
            {
                reason = "no North walkable edge";
                return false;
            }

            doors.Add(new DoorSocket { socketId = "door_N_01", position = north, direction = DoorDirection.North, widthInTiles = DoorWidth, isExit = true });

            if (TryFindEdgeCell(room, DoorDirection.East, out Vector2Int east))
            {
                doors.Add(new DoorSocket { socketId = "door_E_01", position = east, direction = DoorDirection.East, widthInTiles = DoorWidth, isExit = true });
            }

            if (TryFindEdgeCell(room, DoorDirection.West, out Vector2Int west))
            {
                doors.Add(new DoorSocket { socketId = "door_W_01", position = west, direction = DoorDirection.West, widthInTiles = DoorWidth, isExit = true });
            }

            if (!TryFindPlayerSpawn(room, north, out Vector2Int spawnCell))
            {
                reason = "no walkable player spawn";
                return false;
            }

            spawn = new PlayerSpawnSocket
            {
                socketId = "player_spawn_01",
                position = spawnCell,
                facing = DoorDirection.North
            };
            return true;
        }

        private static bool TryFindPlayerSpawn(RoomTemplateSO room, Vector2Int northDoor, out Vector2Int spawnCell)
        {
            spawnCell = default;
            if (!TryGetWalkableBounds(room, out int minX, out int maxX, out _, out _))
            {
                return false;
            }

            float centerX = (minX + maxX) * 0.5f;
            bool found = false;
            int bestY = int.MaxValue;
            float bestDistance = float.MaxValue;

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!room.IsWalkable(candidate) || Vector2Int.Distance(candidate, northDoor) < MinimumSpawnDoorDistance)
                    {
                        continue;
                    }

                    float distance = Mathf.Abs(x - centerX);
                    if (!found || y < bestY || (y == bestY && distance < bestDistance))
                    {
                        found = true;
                        spawnCell = candidate;
                        bestY = y;
                        bestDistance = distance;
                    }
                }
            }

            if (found)
            {
                return true;
            }

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!room.IsWalkable(candidate))
                    {
                        continue;
                    }

                    float distance = Mathf.Abs(x - centerX);
                    if (!found || y < bestY || (y == bestY && distance < bestDistance))
                    {
                        found = true;
                        spawnCell = candidate;
                        bestY = y;
                        bestDistance = distance;
                    }
                }
            }

            return found;
        }

        private static bool TryFindEdgeCell(RoomTemplateSO room, DoorDirection direction, out Vector2Int edgeCell)
        {
            edgeCell = default;
            if (!TryGetWalkableBounds(room, out int minX, out int maxX, out int minY, out int maxY))
            {
                return false;
            }

            float centerX = (minX + maxX) * 0.5f;
            float centerY = (minY + maxY) * 0.5f;
            bool found = false;
            float bestCenterDistance = float.MaxValue;
            int bestExtreme = direction == DoorDirection.North || direction == DoorDirection.East ? int.MinValue : int.MaxValue;

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (!IsDoorEdge(room, candidate, direction))
                    {
                        continue;
                    }

                    int extreme = direction == DoorDirection.North || direction == DoorDirection.South ? y : x;
                    float centerDistance = direction == DoorDirection.North || direction == DoorDirection.South
                        ? Mathf.Abs(x - centerX)
                        : Mathf.Abs(y - centerY);
                    bool betterExtreme = direction == DoorDirection.North || direction == DoorDirection.East
                        ? extreme > bestExtreme
                        : extreme < bestExtreme;

                    if (!found || betterExtreme || (extreme == bestExtreme && centerDistance < bestCenterDistance))
                    {
                        found = true;
                        edgeCell = candidate;
                        bestExtreme = extreme;
                        bestCenterDistance = centerDistance;
                    }
                }
            }

            return found;
        }

        private static bool TryGetWalkableBounds(RoomTemplateSO room, out int minX, out int maxX, out int minY, out int maxY)
        {
            minX = int.MaxValue;
            maxX = int.MinValue;
            minY = int.MaxValue;
            maxY = int.MinValue;
            bool found = false;

            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    if (!room.IsWalkable(new Vector2Int(x, y)))
                    {
                        continue;
                    }

                    found = true;
                    minX = Mathf.Min(minX, x);
                    maxX = Mathf.Max(maxX, x);
                    minY = Mathf.Min(minY, y);
                    maxY = Mathf.Max(maxY, y);
                }
            }

            return found;
        }

        private static bool IsDoorEdge(RoomTemplateSO room, Vector2Int cell, DoorDirection direction)
        {
            return room.IsWalkable(cell) && !room.IsWalkable(cell + DirectionOffset(direction));
        }

        private static Vector2Int DirectionOffset(DoorDirection direction)
        {
            switch (direction)
            {
                case DoorDirection.North:
                    return new Vector2Int(0, 1);
                case DoorDirection.South:
                    return new Vector2Int(0, -1);
                case DoorDirection.East:
                    return new Vector2Int(1, 0);
                case DoorDirection.West:
                    return new Vector2Int(-1, 0);
                default:
                    return Vector2Int.zero;
            }
        }

        private static List<RoomTemplateSO> LoadRoomTemplates()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { RoomsRoot });
            List<RoomTemplateSO> rooms = new List<RoomTemplateSO>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                RoomTemplateSO room = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(path);
                if (room != null)
                {
                    rooms.Add(room);
                }
            }

            rooms.Sort((a, b) => string.Compare(AssetDatabase.GetAssetPath(a), AssetDatabase.GetAssetPath(b), System.StringComparison.OrdinalIgnoreCase));
            return rooms;
        }

        private static string Label(RoomTemplateSO room)
        {
            return !string.IsNullOrEmpty(room.roomId) ? room.roomId : room.name;
        }
    }
}
#endif
