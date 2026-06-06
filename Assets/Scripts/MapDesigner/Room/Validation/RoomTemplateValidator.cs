using System.Collections.Generic;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Validation
{
    public static class RoomTemplateValidator
    {
        public static List<RoomValidationIssue> Validate(RoomTemplateSO template)
        {
            var issues = new List<RoomValidationIssue>();
            if (template == null)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_NULL_TEMPLATE", "RoomTemplateSO is null.", string.Empty));
                return issues;
            }

            string id = template.roomId ?? string.Empty;

            if (string.IsNullOrEmpty(template.biomeId))
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_MISSING_BIOME_ID", "biomeId is null or empty.", id));
            }

            if (template.prefabRef == null)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_MISSING_PREFAB_REF", "prefabRef is null; authoring incomplete.", id));
            }

            ValidatePlayerSpawn(template, id, issues);
            ValidateDoors(template, id, issues);
            ValidateCameraBounds(template, id, issues);
            ValidateEnemySpawns(template, id, issues);
            ValidateBoundsShape(template, id, issues);
            ValidateTags(template, id, issues);
            EmitInfo(template, id, issues);

            return issues;
        }

        public static List<RoomValidationIssue> ValidateBank(RoomBankSO bank)
        {
            if (bank == null)
            {
                return new List<RoomValidationIssue>
                {
                    new RoomValidationIssue(ValidationSeverity.Error,
                        "ERR_NULL_BANK", "RoomBankSO is null.", string.Empty),
                };
            }
            return bank.ValidateAll();
        }

        private static void ValidatePlayerSpawn(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            if (template.playerSpawn == null)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_NO_PLAYER_SPAWN", "Exactly 1 PlayerSpawnSocket required; none assigned.", id));
                return;
            }

            if (!RectContains(template.bounds, template.playerSpawn.position))
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_PLAYER_OUT_OF_BOUNDS",
                    $"PlayerSpawnSocket position {template.playerSpawn.position} is outside room bounds {template.bounds}.",
                    id));
                return;
            }

            if (!template.IsWalkable(template.playerSpawn.position))
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_PLAYER_NOT_WALKABLE",
                    $"PlayerSpawnSocket position {template.playerSpawn.position} is not walkable.",
                    id));
            }
        }

        private static void ValidateDoors(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            DoorSocket[] slots = template.ResolveExitSlots();
            int validSlotCount = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    validSlotCount++;
                }
            }

            if (template.doorSockets != null)
            {
                for (int i = 0; i < template.doorSockets.Count; i++)
                {
                    DoorSocket d = template.doorSockets[i];
                    if (d == null) continue;

                    if (d.isExit && d.direction == RIMA.DoorDirection.South)
                    {
                        issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                            "ERR_SOUTH_EXIT_SOCKET",
                            $"DoorSocket[{i}] '{d.socketId}' is a South exit; south exits are forbidden.",
                            id));
                    }

                    if (d.isExit && RoomTemplateSO.ExitSlotIndex(d) < 0)
                    {
                        issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                            "ERR_INVALID_EXIT_SLOT_ID",
                            $"DoorSocket[{i}] '{d.socketId}' is not one of door_NW_01 / door_N_01 / door_NE_01.",
                            id));
                    }

                    if (!IsValidDoorEdge(template, d))
                    {
                        issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                            "ERR_DOOR_SOCKET_NOT_EDGE",
                            $"DoorSocket[{i}] '{d.socketId}' at {d.position} is not a walkable {d.direction} edge cell.",
                            id));
                    }

                    if (d.isExit && RoomTemplateSO.ExitSlotIndex(d) >= 0 && !HasSouthCorridor(template, d.position))
                    {
                        issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                            "ERR_EXIT_SLOT_NO_SOUTH_CORRIDOR",
                            $"DoorSocket[{i}] '{d.socketId}' at {d.position} does not have 2 walkable cells to the south.",
                            id));
                    }
                }
            }

            if (validSlotCount == 0)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_NO_EXIT_SLOT", "No valid authored exit slot.", id));
            }

            if (slots[1] == null)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_MISSING_N_EXIT_SLOT", "Missing valid door_N_01 exit slot.", id));
            }

            ValidateSlotDistinctnessAndSeparation(slots, id, issues);
            ValidateSlotSpawnDistance(template, slots, id, issues);
            ValidateSlotWarnings(template, slots, id, issues);
        }

        private static void ValidateCameraBounds(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            RectInt cam = template.cameraBounds.tileRect;
            if (cam.width <= 0 || cam.height <= 0)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_CAMERA_BOUNDS_NO_WALKABLE",
                    "CameraBounds.tileRect has zero or negative dimensions; cannot contain walkable tiles.",
                    id));
                return;
            }

            if (!RectsOverlap(cam, template.bounds))
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_CAMERA_BOUNDS_NO_WALKABLE",
                    $"CameraBounds.tileRect {cam} does not overlap room bounds {template.bounds}.",
                    id));
            }
        }

        private static void ValidateEnemySpawns(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            if (template.enemySpawnSockets == null) return;
            for (int i = 0; i < template.enemySpawnSockets.Count; i++)
            {
                var spawn = template.enemySpawnSockets[i];
                if (spawn == null) continue;
                if (!RectContains(template.bounds, spawn.position))
                {
                    issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                        "ERR_ENEMY_OUT_OF_BOUNDS",
                        $"EnemySpawnSocket[{i}] '{spawn.socketId}' at {spawn.position} is outside room bounds {template.bounds}.",
                        id));
                }
            }
        }

        private static void ValidateBoundsShape(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            int w = template.bounds.width;
            int h = template.bounds.height;

            if (w < 6 || h < 6)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                    "WARN_SMALL_BOUNDS",
                    $"Room bounds {w}x{h} below recommended minimum 6x6.",
                    id));
            }

            if (w > 0 && h > 0)
            {
                float aspect = (float)w / h;
                if (aspect < 0.5f || aspect > 2.0f)
                {
                    issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                        "WARN_UNUSUAL_ASPECT",
                        $"Aspect ratio {aspect:F2} outside 0.5-2.0 range.",
                        id));
                }
            }
        }

        private static void ValidateTags(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            if (template.encounterTags == null || template.encounterTags.Count == 0)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                    "WARN_NO_ENCOUNTER_TAGS",
                    "encounterTags is empty; EncounterBank will fall back to defaults.",
                    id));
            }
        }

        private static void EmitInfo(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            issues.Add(new RoomValidationIssue(ValidationSeverity.Info,
                "INFO_ROOM_DIMENSIONS",
                $"Bounds: {template.bounds.width}x{template.bounds.height} tiles.",
                id));

            int doorCount = template.doorSockets?.Count ?? 0;
            int enemyCount = template.enemySpawnSockets?.Count ?? 0;
            issues.Add(new RoomValidationIssue(ValidationSeverity.Info,
                "INFO_SOCKET_COUNTS",
                $"Doors: {doorCount}, Enemies: {enemyCount}, Player: {(template.playerSpawn != null ? 1 : 0)}.",
                id));

            int encTags = template.encounterTags?.Count ?? 0;
            int diffTags = template.difficultyTags?.Count ?? 0;
            int blkTags = template.blockerTags?.Count ?? 0;
            issues.Add(new RoomValidationIssue(ValidationSeverity.Info,
                "INFO_TAG_COVERAGE",
                $"encounter={encTags}, difficulty={diffTags}, blocker={blkTags}.",
                id));
        }

        private static bool RectContains(RectInt rect, Vector2Int p)
        {
            return p.x >= rect.xMin && p.x < rect.xMax && p.y >= rect.yMin && p.y < rect.yMax;
        }

        private static bool RectsOverlap(RectInt a, RectInt b)
        {
            return !(a.xMax <= b.xMin || a.xMin >= b.xMax || a.yMax <= b.yMin || a.yMin >= b.yMax);
        }

        private static bool IsValidDoorEdge(RoomTemplateSO template, DoorSocket socket)
        {
            if (!RectContains(template.bounds, socket.position) || !template.IsWalkable(socket.position))
            {
                return false;
            }

            Vector2Int neighbor = socket.position + DirectionOffset(socket.direction);
            return !template.IsWalkable(neighbor);
        }

        private static bool HasSouthCorridor(RoomTemplateSO template, Vector2Int position)
        {
            return template.IsWalkable(position + Vector2Int.down)
                && template.IsWalkable(position + Vector2Int.down * 2);
        }

        private static void ValidateSlotDistinctnessAndSeparation(DoorSocket[] slots, string id, List<RoomValidationIssue> issues)
        {
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
                        issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                            "ERR_EXIT_SLOTS_NOT_DISTINCT",
                            $"{RoomTemplateSO.ExitSlotLabel(i)} and {RoomTemplateSO.ExitSlotLabel(j)} share {slots[i].position}.",
                            id));
                    }

                    if (Vector2Int.Distance(slots[i].position, slots[j].position) < 3f)
                    {
                        issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                            "ERR_EXIT_SLOTS_TOO_CLOSE",
                            $"{RoomTemplateSO.ExitSlotLabel(i)} and {RoomTemplateSO.ExitSlotLabel(j)} are less than 3 tiles apart.",
                            id));
                    }
                }
            }
        }

        private static void ValidateSlotSpawnDistance(RoomTemplateSO template, DoorSocket[] slots, string id, List<RoomValidationIssue> issues)
        {
            if (template.playerSpawn == null)
            {
                return;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                DoorSocket slot = slots[i];
                if (slot == null)
                {
                    continue;
                }

                if (Vector2Int.Distance(template.playerSpawn.position, slot.position) < 4f)
                {
                    issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                        "ERR_PLAYER_TOO_CLOSE_TO_EXIT_SLOT",
                        $"Player spawn is less than 4 tiles from {RoomTemplateSO.ExitSlotLabel(i)} at {slot.position}.",
                        id));
                }
            }
        }

        private static void ValidateSlotWarnings(RoomTemplateSO template, DoorSocket[] slots, string id, List<RoomValidationIssue> issues)
        {
            if (slots[0] != null && slots[2] != null && Mathf.Abs(slots[0].position.y - slots[2].position.y) > 2)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                    "WARN_EXIT_SLOT_WINGS_MISALIGNED",
                    $"NW/NE slot Y values differ by more than 2: {slots[0].position.y} vs {slots[2].position.y}.",
                    id));
            }

            if (template.props != null)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    DoorSocket slot = slots[i];
                    if (slot == null)
                    {
                        continue;
                    }

                    for (int p = 0; p < template.props.Count; p++)
                    {
                        var prop = template.props[p];
                        if (prop != null && Vector2Int.Distance(slot.position, prop.tilePosition) < 2f)
                        {
                            issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                                "WARN_EXIT_SLOT_NEAR_PROP",
                                $"{RoomTemplateSO.ExitSlotLabel(i)} at {slot.position} is less than 2 tiles from prop at {prop.tilePosition}.",
                                id));
                        }
                    }
                }
            }

            if (template.playerSpawn == null || !template.IsWalkable(template.playerSpawn.position))
            {
                return;
            }

            HashSet<Vector2Int> reachable = FloodFill(template, template.playerSpawn.position);
            for (int i = 0; i < slots.Length; i++)
            {
                DoorSocket slot = slots[i];
                if (slot != null && !reachable.Contains(slot.position))
                {
                    issues.Add(new RoomValidationIssue(ValidationSeverity.Warning,
                        "WARN_EXIT_SLOT_UNREACHABLE",
                        $"{RoomTemplateSO.ExitSlotLabel(i)} at {slot.position} is not flood-fill reachable from playerSpawn.",
                        id));
                }
            }
        }

        private static HashSet<Vector2Int> FloodFill(RoomTemplateSO template, Vector2Int start)
        {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Queue<Vector2Int> open = new Queue<Vector2Int>();
            visited.Add(start);
            open.Enqueue(start);

            while (open.Count > 0)
            {
                Vector2Int cell = open.Dequeue();
                TryVisit(template, cell + Vector2Int.up, visited, open);
                TryVisit(template, cell + Vector2Int.down, visited, open);
                TryVisit(template, cell + Vector2Int.left, visited, open);
                TryVisit(template, cell + Vector2Int.right, visited, open);
            }

            return visited;
        }

        private static void TryVisit(RoomTemplateSO template, Vector2Int cell, HashSet<Vector2Int> visited, Queue<Vector2Int> open)
        {
            if (!template.IsWalkable(cell) || !visited.Add(cell))
            {
                return;
            }

            open.Enqueue(cell);
        }

        private static Vector2Int DirectionOffset(RIMA.DoorDirection direction)
        {
            switch (direction)
            {
                case RIMA.DoorDirection.North:
                    return new Vector2Int(0, 1);
                case RIMA.DoorDirection.South:
                    return new Vector2Int(0, -1);
                case RIMA.DoorDirection.East:
                    return new Vector2Int(1, 0);
                case RIMA.DoorDirection.West:
                    return new Vector2Int(-1, 0);
                default:
                    return Vector2Int.zero;
            }
        }
    }
}
