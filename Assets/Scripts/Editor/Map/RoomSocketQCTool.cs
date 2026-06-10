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
        private const string CharSelectPath = "Assets/Resources/ChamberSelect/Chamber_CharSelect.asset";
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
            List<string> warningTemplates = new List<string>();
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

                    if (!TryBuildFixedSockets(room, out PlayerSpawnSocket spawn, out List<DoorSocket> doors, out string reason, out List<string> warnings))
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
                    for (int warningIndex = 0; warningIndex < warnings.Count; warningIndex++)
                    {
                        warningTemplates.Add($"{Label(room)}: {warnings[warningIndex]}");
                        report.AppendLine($"  WARN {warnings[warningIndex]}");
                    }
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
            report.AppendLine($"warningTemplates={warningTemplates.Count}");
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

            DoorSocket[] slots = room.ResolveExitSlots();
            int validSlots = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    validSlots++;
                }
            }

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
                        if (socket.direction == DoorDirection.South)
                        {
                            issues.Add($"South exit forbidden at {socket.position}");
                        }

                        if (RoomTemplateSO.ExitSlotIndex(socket) < 0)
                        {
                            issues.Add($"exit socket '{socket.socketId}' does not use NW/N/NE slot convention");
                        }
                    }

                    if (!IsDoorEdge(room, socket.position, socket.direction))
                    {
                        issues.Add($"socket '{socket.socketId}' is not a walkable {socket.direction} edge cell at {socket.position}");
                    }

                    if (socket.isExit && RoomTemplateSO.ExitSlotIndex(socket) >= 0 && !HasSouthCorridor(room, socket.position))
                    {
                        issues.Add($"slot '{socket.socketId}' lacks 2-cell walkable south corridor at {socket.position}");
                    }
                }
            }

            if (validSlots == 0)
            {
                issues.Add("missing valid exit slots");
            }

            if (slots[1] == null)
            {
                issues.Add("missing valid N slot door_N_01");
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    continue;
                }

                for (int j = i + 1; j < slots.Length; j++)
                {
                    if (slots[j] == null)
                    {
                        continue;
                    }

                    if (slots[i].position == slots[j].position)
                    {
                        issues.Add($"{RoomTemplateSO.ExitSlotLabel(i)} and {RoomTemplateSO.ExitSlotLabel(j)} share {slots[i].position}");
                    }

                    if (Vector2Int.Distance(slots[i].position, slots[j].position) < 3f)
                    {
                        issues.Add($"{RoomTemplateSO.ExitSlotLabel(i)} and {RoomTemplateSO.ExitSlotLabel(j)} are less than 3 tiles apart");
                    }
                }
            }

            if (slots[0] != null && slots[2] != null && Mathf.Abs(slots[0].position.y - slots[2].position.y) > 2)
            {
                issues.Add($"WARN NW/NE Y alignment differs by >2 ({slots[0].position.y}, {slots[2].position.y})");
            }

            if (room.playerSpawn != null && room.IsWalkable(room.playerSpawn.position))
            {
                HashSet<Vector2Int> reachable = FloodFill(room, room.playerSpawn.position);
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i] != null && !reachable.Contains(slots[i].position))
                    {
                        issues.Add($"WARN {RoomTemplateSO.ExitSlotLabel(i)} unreachable from playerSpawn at {room.playerSpawn.position}");
                    }
                }
            }

            if (room.props != null)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i] == null)
                    {
                        continue;
                    }

                    for (int p = 0; p < room.props.Count; p++)
                    {
                        var prop = room.props[p];
                        if (prop != null && Vector2Int.Distance(slots[i].position, prop.tilePosition) < 2f)
                        {
                            issues.Add($"WARN {RoomTemplateSO.ExitSlotLabel(i)} slot near prop at {prop.tilePosition}");
                        }
                    }
                }
            }

            return issues;
        }

        private static bool TryBuildFixedSockets(RoomTemplateSO room, out PlayerSpawnSocket spawn, out List<DoorSocket> doors, out string reason, out List<string> warnings)
        {
            spawn = null;
            doors = new List<DoorSocket>();
            reason = string.Empty;
            warnings = new List<string>();

            Vector2Int[] slotPositions = BuildExitSlotPositions(room, warnings);
            Vector2Int firstDoor = default;
            bool hasDoor = false;
            for (int i = 0; i < slotPositions.Length; i++)
            {
                if (slotPositions[i] == InvalidCell)
                {
                    continue;
                }

                DoorSocket door = new DoorSocket
                {
                    socketId = RoomTemplateSO.ExitSlotId(i),
                    position = slotPositions[i],
                    direction = DoorDirection.North,
                    widthInTiles = DoorWidth,
                    isExit = true
                };
                doors.Add(door);
                if (!hasDoor)
                {
                    firstDoor = slotPositions[i];
                    hasDoor = true;
                }
            }

            if (!hasDoor)
            {
                reason = "no North IsDoorEdge cells";
                return false;
            }

            if (!TryFindPlayerSpawn(room, doors, firstDoor, out Vector2Int spawnCell))
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

        private static bool TryFindPlayerSpawn(RoomTemplateSO room, List<DoorSocket> doors, Vector2Int fallbackDoor, out Vector2Int spawnCell)
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
                    if (!room.IsWalkable(candidate) || IsTooCloseToAnyDoor(candidate, doors, fallbackDoor))
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

        private static bool IsTooCloseToAnyDoor(Vector2Int candidate, List<DoorSocket> doors, Vector2Int fallbackDoor)
        {
            if (doors == null || doors.Count == 0)
            {
                return Vector2Int.Distance(candidate, fallbackDoor) < MinimumSpawnDoorDistance;
            }

            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i] != null && Vector2Int.Distance(candidate, doors[i].position) < MinimumSpawnDoorDistance)
                {
                    return true;
                }
            }

            return false;
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

        private static readonly Vector2Int InvalidCell = new Vector2Int(int.MinValue, int.MinValue);

        private static Vector2Int[] BuildExitSlotPositions(RoomTemplateSO room, List<string> warnings)
        {
            Vector2Int[] slots = { InvalidCell, InvalidCell, InvalidCell };
            List<Vector2Int> candidates = CollectNorthDoorEdgeCandidates(room);
            if (candidates.Count == 0)
            {
                warnings.Add("no North IsDoorEdge cells for NW/N/NE slots");
                return slots;
            }

            List<List<Vector2Int>> segments = BuildContiguousSegments(candidates);
            List<Vector2Int> segment = PickPreferredSegment(segments);
            if (TryPickThreeSeparated(segment, out Vector2Int nw, out Vector2Int n, out Vector2Int ne))
            {
                slots[0] = nw;
                slots[1] = n;
                slots[2] = ne;
                return slots;
            }

            warnings.Add("largest north segment could not provide three separated slots; using sorted fallback");
            PickSeparatedFallbackSlots(room, candidates, slots);

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == InvalidCell)
                {
                    warnings.Add($"missing {RoomTemplateSO.ExitSlotLabel(i)} slot");
                }
            }

            return slots;
        }

        private static void PickSeparatedFallbackSlots(RoomTemplateSO room, List<Vector2Int> candidates, Vector2Int[] slots)
        {
            float centerX = room.bounds.xMin + (room.bounds.width - 1) * 0.5f;
            Vector2Int center = PickNearest(candidates, centerX, null);
            slots[1] = center;

            Vector2Int left = InvalidCell;
            float leftDistance = float.MaxValue;
            Vector2Int right = InvalidCell;
            float rightDistance = float.MaxValue;

            for (int i = 0; i < candidates.Count; i++)
            {
                Vector2Int candidate = candidates[i];
                if (candidate == center || Vector2Int.Distance(candidate, center) < 3f)
                {
                    continue;
                }

                if (candidate.x < center.x)
                {
                    float distance = Mathf.Abs(candidate.x - (room.bounds.xMin + room.bounds.width / 3f));
                    if (left == InvalidCell || distance < leftDistance)
                    {
                        left = candidate;
                        leftDistance = distance;
                    }
                }
                else if (candidate.x > center.x)
                {
                    float distance = Mathf.Abs(candidate.x - (room.bounds.xMin + room.bounds.width * 2f / 3f));
                    if (right == InvalidCell || distance < rightDistance)
                    {
                        right = candidate;
                        rightDistance = distance;
                    }
                }
            }

            slots[0] = left;
            slots[2] = right;
        }

        private static List<Vector2Int> CollectNorthDoorEdgeCandidates(RoomTemplateSO room)
        {
            Dictionary<int, Vector2Int> byX = new Dictionary<int, Vector2Int>();
            for (int y = room.bounds.yMin; y < room.bounds.yMax; y++)
            {
                for (int x = room.bounds.xMin; x < room.bounds.xMax; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (!IsDoorEdge(room, cell, DoorDirection.North) || !HasSouthCorridor(room, cell))
                    {
                        continue;
                    }

                    if (!byX.TryGetValue(x, out Vector2Int existing) || cell.y > existing.y)
                    {
                        byX[x] = cell;
                    }
                }
            }

            List<Vector2Int> candidates = new List<Vector2Int>(byX.Values);
            candidates.Sort((a, b) => a.x != b.x ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y));
            return candidates;
        }

        private static List<List<Vector2Int>> BuildContiguousSegments(List<Vector2Int> candidates)
        {
            List<List<Vector2Int>> segments = new List<List<Vector2Int>>();
            List<Vector2Int> current = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                if (current == null || candidates[i].x > current[current.Count - 1].x + 1)
                {
                    current = new List<Vector2Int>();
                    segments.Add(current);
                }

                current.Add(candidates[i]);
            }

            return segments;
        }

        private static List<Vector2Int> PickPreferredSegment(List<List<Vector2Int>> segments)
        {
            List<Vector2Int> best = null;
            for (int i = 0; i < segments.Count; i++)
            {
                List<Vector2Int> segment = segments[i];
                if (best == null || segment.Count > best.Count)
                {
                    best = segment;
                }
            }

            return best ?? new List<Vector2Int>();
        }

        private static bool TryPickThreeSeparated(List<Vector2Int> segment, out Vector2Int nw, out Vector2Int n, out Vector2Int ne)
        {
            nw = n = ne = default;
            if (segment == null || segment.Count < 3 || segment[segment.Count - 1].x - segment[0].x < 6)
            {
                return false;
            }

            int minX = segment[0].x;
            int maxX = segment[segment.Count - 1].x;
            float span = maxX - minX;
            nw = PickNearest(segment, minX + span / 3f, null);
            n = PickNearest(segment, minX + span * 0.5f, new List<Vector2Int> { nw });
            ne = PickNearest(segment, minX + span * 2f / 3f, new List<Vector2Int> { nw, n });

            return Vector2Int.Distance(nw, n) >= 3f
                && Vector2Int.Distance(n, ne) >= 3f
                && Vector2Int.Distance(nw, ne) >= 3f;
        }

        private static Vector2Int PickNearest(List<Vector2Int> cells, float targetX, List<Vector2Int> excluded)
        {
            Vector2Int best = cells[0];
            float bestDistance = float.MaxValue;
            for (int i = 0; i < cells.Count; i++)
            {
                Vector2Int candidate = cells[i];
                if (excluded != null && excluded.Contains(candidate))
                {
                    continue;
                }

                float distance = Mathf.Abs(candidate.x - targetX);
                if (distance < bestDistance)
                {
                    best = candidate;
                    bestDistance = distance;
                }
            }

            return best;
        }

        private static bool IsDoorEdge(RoomTemplateSO room, Vector2Int cell, DoorDirection direction)
        {
            return room.IsWalkable(cell) && !room.IsWalkable(cell + DirectionOffset(direction));
        }

        private static bool HasSouthCorridor(RoomTemplateSO room, Vector2Int cell)
        {
            return room.IsWalkable(cell + Vector2Int.down)
                && room.IsWalkable(cell + Vector2Int.down * 2);
        }

        private static HashSet<Vector2Int> FloodFill(RoomTemplateSO room, Vector2Int start)
        {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Queue<Vector2Int> open = new Queue<Vector2Int>();
            visited.Add(start);
            open.Enqueue(start);
            while (open.Count > 0)
            {
                Vector2Int cell = open.Dequeue();
                TryVisit(room, cell + Vector2Int.up, visited, open);
                TryVisit(room, cell + Vector2Int.down, visited, open);
                TryVisit(room, cell + Vector2Int.left, visited, open);
                TryVisit(room, cell + Vector2Int.right, visited, open);
            }

            return visited;
        }

        private static void TryVisit(RoomTemplateSO room, Vector2Int cell, HashSet<Vector2Int> visited, Queue<Vector2Int> open)
        {
            if (!room.IsWalkable(cell) || !visited.Add(cell))
            {
                return;
            }

            open.Enqueue(cell);
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
