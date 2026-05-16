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
            }
        }

        private static void ValidateDoors(RoomTemplateSO template, string id, List<RoomValidationIssue> issues)
        {
            int exitCount = 0;
            if (template.doorSockets != null)
            {
                foreach (var d in template.doorSockets)
                {
                    if (d != null && d.isExit) exitCount++;
                }
            }

            if (exitCount == 0)
            {
                issues.Add(new RoomValidationIssue(ValidationSeverity.Error,
                    "ERR_NO_EXIT_SOCKET", "No DoorSocket with isExit=true.", id));
            }
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
    }
}
