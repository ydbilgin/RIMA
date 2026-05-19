using System.Collections.Generic;
using System.Linq;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Props;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.MapDesigner.Encounter
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors = new List<string>();
        public List<string> Warnings = new List<string>();
    }

    public static class EncounterTemplateValidator
    {
        public static ValidationResult Validate(EncounterTemplateSO et)
        {
            var result = new ValidationResult();
            if (et == null)
            {
                result.Errors.Add("EncounterTemplateSO is null.");
                return result;
            }

            ValidateEncounterId(et, result);
            ValidateMacroRoomType(et, result);
            ValidateSubRoomCount(et, result);

            List<SubRoomEntry> subRooms = et.subRooms ?? new List<SubRoomEntry>();
            ValidateSubRoomKeys(subRooms, result);
            ValidateEntryFinal(subRooms, result);
            ValidateLinks(subRooms, result);
            ValidateReachability(subRooms, result);
            ValidateSpawnSockets(subRooms, result);

            return result;
        }

        private static void ValidateEncounterId(EncounterTemplateSO et, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(et.encounterId))
            {
                result.Errors.Add("encounterId must be non-empty.");
                return;
            }

#if UNITY_EDITOR
            string currentPath = AssetDatabase.GetAssetPath(et);
            string[] guids = AssetDatabase.FindAssets("t:EncounterTemplateSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path == currentPath)
                {
                    continue;
                }

                var other = AssetDatabase.LoadAssetAtPath<EncounterTemplateSO>(path);
                if (other != null && other.encounterId == et.encounterId)
                {
                    result.Errors.Add($"encounterId '{et.encounterId}' is duplicated by asset '{path}'.");
                }
            }
#else
            EncounterTemplateSO[] loaded = Resources.FindObjectsOfTypeAll<EncounterTemplateSO>();
            foreach (EncounterTemplateSO other in loaded)
            {
                if (other != null && other != et && other.encounterId == et.encounterId)
                {
                    result.Errors.Add($"encounterId '{et.encounterId}' is duplicated by loaded asset '{other.name}'.");
                }
            }
#endif
        }

        private static void ValidateMacroRoomType(EncounterTemplateSO et, ValidationResult result)
        {
            if (et.macroRoomType != RIMA.RoomType.Combat && et.macroRoomType != RIMA.RoomType.Elite)
            {
                result.Errors.Add("macroRoomType must be Combat or Elite.");
            }
        }

        private static void ValidateSubRoomCount(EncounterTemplateSO et, ValidationResult result)
        {
            int count = et.subRooms == null ? 0 : et.subRooms.Count;
            if (count < 3 || count > 5)
            {
                result.Errors.Add($"subRooms.Count must be in [3,5]; found {count}.");
            }
        }

        private static void ValidateSubRoomKeys(List<SubRoomEntry> subRooms, ValidationResult result)
        {
            var seen = new HashSet<string>();
            foreach (SubRoomEntry subRoom in subRooms)
            {
                if (subRoom == null)
                {
                    result.Errors.Add("subRooms contains a null entry.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(subRoom.subRoomKey))
                {
                    result.Errors.Add("subRoomKey must be non-empty.");
                    continue;
                }

                if (!seen.Add(subRoom.subRoomKey))
                {
                    result.Errors.Add($"duplicate subRoomKey '{subRoom.subRoomKey}'.");
                }
            }
        }

        private static void ValidateEntryFinal(List<SubRoomEntry> subRooms, ValidationResult result)
        {
            List<SubRoomEntry> entries = subRooms.Where(r => r != null && r.isEntry).ToList();
            List<SubRoomEntry> finals = subRooms.Where(r => r != null && r.isFinal).ToList();

            if (entries.Count != 1)
            {
                result.Errors.Add($"exactly 1 sub-room must be isEntry; found {entries.Count}.");
            }

            if (finals.Count != 1)
            {
                result.Errors.Add($"exactly 1 sub-room must be isFinal; found {finals.Count}.");
            }

            if (entries.Count == 1 && finals.Count == 1 && ReferenceEquals(entries[0], finals[0]))
            {
                result.Errors.Add("entry and final sub-room must be distinct.");
            }
        }

        private static void ValidateLinks(List<SubRoomEntry> subRooms, ValidationResult result)
        {
            Dictionary<string, SubRoomEntry> byKey = BuildUniqueKeyMap(subRooms);

            foreach (SubRoomEntry source in subRooms)
            {
                if (source == null)
                {
                    continue;
                }

                if (source.room == null)
                {
                    result.Errors.Add($"sub-room '{SafeKey(source)}' has no room.");
                    continue;
                }

                List<SubRoomLink> links = source.links ?? new List<SubRoomLink>();
                foreach (SubRoomLink link in links)
                {
                    if (link == null)
                    {
                        result.Errors.Add($"sub-room '{SafeKey(source)}' contains a null link.");
                        continue;
                    }

                    bool fromValid = HasDoorSocket(source.room, link.fromDoorSocketId);
                    if (!fromValid)
                    {
                        result.Errors.Add($"sub-room '{SafeKey(source)}' link fromDoorSocketId '{link.fromDoorSocketId}' does not resolve.");
                    }

                    if (string.IsNullOrWhiteSpace(link.toSubRoomKey) || !byKey.TryGetValue(link.toSubRoomKey, out SubRoomEntry destination))
                    {
                        result.Errors.Add($"sub-room '{SafeKey(source)}' link toSubRoomKey '{link.toSubRoomKey}' does not resolve.");
                        continue;
                    }

                    if (destination.room == null)
                    {
                        result.Errors.Add($"sub-room '{SafeKey(destination)}' has no room for link destination socket validation.");
                        continue;
                    }

                    bool toValid = HasDoorSocket(destination.room, link.toEntryDoorSocketId);
                    if (!toValid)
                    {
                        result.Errors.Add($"sub-room '{SafeKey(source)}' link toEntryDoorSocketId '{link.toEntryDoorSocketId}' does not resolve on '{SafeKey(destination)}'.");
                    }

                    if (fromValid && toValid)
                    {
                        DoorSocket fromSocket = source.room.doorSockets.FirstOrDefault(s => s != null && s.socketId == link.fromDoorSocketId);
                        DoorSocket toSocket = destination.room.doorSockets.FirstOrDefault(s => s != null && s.socketId == link.toEntryDoorSocketId);

                        if (fromSocket != null && toSocket != null)
                        {
                            // 1. Inverse direction check
                            bool isInverse = false;
                            if (fromSocket.direction == DoorDirection.North && toSocket.direction == DoorDirection.South) isInverse = true;
                            else if (fromSocket.direction == DoorDirection.South && toSocket.direction == DoorDirection.North) isInverse = true;
                            else if (fromSocket.direction == DoorDirection.East && toSocket.direction == DoorDirection.West) isInverse = true;
                            else if (fromSocket.direction == DoorDirection.West && toSocket.direction == DoorDirection.East) isInverse = true;

                            if (!isInverse)
                            {
                                result.Errors.Add($"Link direction from '{SafeKey(source)}' ({fromSocket.direction}) to '{SafeKey(destination)}' ({toSocket.direction}) is not inverse.");
                            }

                            // 2. Compatible edge check
                            if (!CheckSocketEdge(source.room, fromSocket))
                            {
                                result.Errors.Add($"Socket '{fromSocket.socketId}' on '{SafeKey(source)}' is facing {fromSocket.direction} but is not on the compatible edge of bounds {source.room.bounds}.");
                            }
                            if (!CheckSocketEdge(destination.room, toSocket))
                            {
                                result.Errors.Add($"Socket '{toSocket.socketId}' on '{SafeKey(destination)}' is facing {toSocket.direction} but is not on the compatible edge of bounds {destination.room.bounds}.");
                            }

                            // 3. Width match check (Warning)
                            if (Mathf.Abs(fromSocket.widthInTiles - toSocket.widthInTiles) > 1)
                            {
                                result.Warnings.Add($"Width mismatch between socket '{fromSocket.socketId}' ({fromSocket.widthInTiles}) and socket '{toSocket.socketId}' ({toSocket.widthInTiles}).");
                            }

                            // 4. Mirrored placement check (Warning)
                            if (fromSocket.direction == DoorDirection.North || fromSocket.direction == DoorDirection.South)
                            {
                                int fromRelX = fromSocket.position.x - source.room.bounds.xMin;
                                int toRelX = toSocket.position.x - destination.room.bounds.xMin;
                                if (Mathf.Abs(fromRelX - toRelX) > 2)
                                {
                                    result.Warnings.Add($"Mirrored placement warning: Horizontal offset between socket '{fromSocket.socketId}' ({fromRelX}) and socket '{toSocket.socketId}' ({toRelX}) is greater than 2.");
                                }
                            }
                            else if (fromSocket.direction == DoorDirection.East || fromSocket.direction == DoorDirection.West)
                            {
                                int fromRelY = fromSocket.position.y - source.room.bounds.yMin;
                                int toRelY = toSocket.position.y - destination.room.bounds.yMin;
                                if (Mathf.Abs(fromRelY - toRelY) > 2)
                                {
                                    result.Warnings.Add($"Mirrored placement warning: Vertical offset between socket '{fromSocket.socketId}' ({fromRelY}) and socket '{toSocket.socketId}' ({toRelY}) is greater than 2.");
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool CheckSocketEdge(RoomTemplateSO room, DoorSocket socket)
        {
            if (room == null || socket == null) return false;
            RectInt b = room.bounds;
            switch (socket.direction)
            {
                case DoorDirection.North:
                    return socket.position.y >= b.yMax - 2;
                case DoorDirection.South:
                    return socket.position.y <= b.yMin + 1;
                case DoorDirection.East:
                    return socket.position.x >= b.xMax - 2;
                case DoorDirection.West:
                    return socket.position.x <= b.xMin + 1;
                default:
                    return false;
            }
        }

        private static void ValidateReachability(List<SubRoomEntry> subRooms, ValidationResult result)
        {
            Dictionary<string, SubRoomEntry> byKey = BuildUniqueKeyMap(subRooms);
            SubRoomEntry entry = subRooms.FirstOrDefault(r => r != null && r.isEntry);
            SubRoomEntry final = subRooms.FirstOrDefault(r => r != null && r.isFinal);
            if (entry == null || final == null || string.IsNullOrWhiteSpace(entry.subRoomKey))
            {
                return;
            }

            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            visited.Add(entry.subRoomKey);
            queue.Enqueue(entry.subRoomKey);

            while (queue.Count > 0)
            {
                string currentKey = queue.Dequeue();
                if (!byKey.TryGetValue(currentKey, out SubRoomEntry current))
                {
                    continue;
                }

                List<SubRoomLink> links = current.links ?? new List<SubRoomLink>();
                foreach (SubRoomLink link in links)
                {
                    if (link == null || string.IsNullOrWhiteSpace(link.toSubRoomKey) || !byKey.ContainsKey(link.toSubRoomKey))
                    {
                        continue;
                    }

                    if (visited.Add(link.toSubRoomKey))
                    {
                        queue.Enqueue(link.toSubRoomKey);
                    }
                }
            }

            foreach (SubRoomEntry subRoom in subRooms)
            {
                if (subRoom != null && !string.IsNullOrWhiteSpace(subRoom.subRoomKey) && !visited.Contains(subRoom.subRoomKey))
                {
                    result.Errors.Add($"sub-room '{subRoom.subRoomKey}' is unreachable from entry.");
                }
            }

            if (!string.IsNullOrWhiteSpace(final.subRoomKey) && !visited.Contains(final.subRoomKey))
            {
                result.Errors.Add($"final sub-room '{final.subRoomKey}' is unreachable from entry.");
            }
        }

        private static void ValidateSpawnSockets(List<SubRoomEntry> subRooms, ValidationResult result)
        {
            foreach (SubRoomEntry subRoom in subRooms)
            {
                RoomTemplateSO room = subRoom == null ? null : subRoom.room;
                if (room == null)
                {
                    continue;
                }

                List<EnemySpawnSocket> sockets = room.enemySpawnSockets ?? new List<EnemySpawnSocket>();
                foreach (EnemySpawnSocket socket in sockets)
                {
                    if (socket == null)
                    {
                        result.Errors.Add($"sub-room '{SafeKey(subRoom)}' has a null enemy spawn socket.");
                        continue;
                    }

                    if (!room.bounds.Contains(socket.position))
                    {
                        result.Errors.Add($"sub-room '{SafeKey(subRoom)}' spawn socket '{socket.socketId}' lies outside bounds.");
                    }

                    if (!room.IsWalkable(socket.position))
                    {
                        result.Errors.Add($"sub-room '{SafeKey(subRoom)}' spawn socket '{socket.socketId}' is not walkable.");
                    }

                    if (TryGetBlockingProp(room, socket.position, out PropPlacementData prop))
                    {
                        result.Errors.Add($"spawn socket {socket.socketId} blocked by prop {prop.propDefinitionGuid}");
                    }
                }
            }
        }

        private static Dictionary<string, SubRoomEntry> BuildUniqueKeyMap(List<SubRoomEntry> subRooms)
        {
            var byKey = new Dictionary<string, SubRoomEntry>();
            foreach (SubRoomEntry subRoom in subRooms)
            {
                if (subRoom == null || string.IsNullOrWhiteSpace(subRoom.subRoomKey) || byKey.ContainsKey(subRoom.subRoomKey))
                {
                    continue;
                }

                byKey.Add(subRoom.subRoomKey, subRoom);
            }

            return byKey;
        }

        private static bool HasDoorSocket(RoomTemplateSO room, string socketId)
        {
            if (room == null || string.IsNullOrWhiteSpace(socketId) || room.doorSockets == null)
            {
                return false;
            }

            return room.doorSockets.Any(socket => socket != null && socket.socketId == socketId);
        }

        private static bool TryGetBlockingProp(RoomTemplateSO room, Vector2Int spawnPosition, out PropPlacementData blockingProp)
        {
            blockingProp = null;
            if (room == null || room.props == null)
            {
                return false;
            }

            Rect spawnBounds = new Rect(spawnPosition.x, spawnPosition.y, 1f, 1f);
            foreach (PropPlacementData prop in room.props)
            {
                if (prop == null)
                {
                    continue;
                }

                Rect propBounds = new Rect(prop.tilePosition.x, prop.tilePosition.y, 1f, 1f);
                if (spawnBounds.Overlaps(propBounds))
                {
                    blockingProp = prop;
                    return true;
                }
            }

            return false;
        }

        private static string SafeKey(SubRoomEntry subRoom)
        {
            return subRoom == null || string.IsNullOrWhiteSpace(subRoom.subRoomKey) ? "<missing>" : subRoom.subRoomKey;
        }
    }
}
