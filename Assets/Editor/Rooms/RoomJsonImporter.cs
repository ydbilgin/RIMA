#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.Rooms
{
    public static class RoomJsonImporter
    {
        private const string MenuPath = "RIMA/Rooms/Import ChatGPT JSON";
        private const string DefaultJsonPath = "STAGING/chatgpt_rooms.json";
        private const string OutputFolder = "Assets/Data/Rooms/Generated";
        private const string DefaultBiomeId = "ShatteredKeep";

        [MenuItem(MenuPath)]
        public static void ImportChatGptJson()
        {
            string absolutePath = Path.GetFullPath(DefaultJsonPath);
            if (!File.Exists(absolutePath))
            {
                // Dry test path: select STAGING/chatgpt_rooms.sample.json in the file panel.
                // Production path: place ChatGPT output at STAGING/chatgpt_rooms.json and run this menu.
                absolutePath = EditorUtility.OpenFilePanel("Import ChatGPT Room JSON", Path.GetFullPath("STAGING"), "json");
            }

            if (string.IsNullOrEmpty(absolutePath))
            {
                Debug.Log("[RoomJsonImporter] Import cancelled.");
                return;
            }

            ImportFromPath(absolutePath);
        }

        private static void ImportFromPath(string absolutePath)
        {
            ImportReport report = new ImportReport();

            string json;
            try
            {
                json = File.ReadAllText(absolutePath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[RoomJsonImporter] Failed to read JSON '{absolutePath}': {ex.Message}");
                return;
            }

            RoomJsonDocument document = ParseDocument(json, report);
            if (document == null || document.rooms == null || document.rooms.Length == 0)
            {
                Debug.LogError("[RoomJsonImporter] No rooms found. Expected a top-level JSON array or an object with a rooms array.");
                return;
            }

            EnsureFolder(OutputFolder);

            var importedIds = new List<string>();
            foreach (RoomJson room in document.rooms)
            {
                if (TryImportRoom(room, report, out string importedId))
                {
                    importedIds.Add(importedId);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string ids = importedIds.Count == 0 ? "(none)" : string.Join(", ", importedIds);
            Debug.Log($"[RoomJsonImporter] Imported {importedIds.Count}/{document.rooms.Length} rooms. Warnings={report.WarningCount}, Errors={report.ErrorCount}. RoomIds: {ids}");
        }

        private static RoomJsonDocument ParseDocument(string json, ImportReport report)
        {
            string trimmed = string.IsNullOrWhiteSpace(json) ? string.Empty : json.Trim();
            if (trimmed.Length == 0)
            {
                report.Error("JSON is empty.");
                return null;
            }

            if (trimmed.StartsWith("[", StringComparison.Ordinal))
            {
                trimmed = "{\"rooms\":" + trimmed + "}";
            }

            try
            {
                return JsonUtility.FromJson<RoomJsonDocument>(trimmed);
            }
            catch (Exception ex)
            {
                report.Error($"JSON parse failed: {ex.Message}");
                return null;
            }
        }

        private static bool TryImportRoom(RoomJson room, ImportReport report, out string importedId)
        {
            importedId = null;
            if (room == null)
            {
                report.Error("Skipped null room entry.");
                return false;
            }

            // v2: id field overrides roomId; size.w/h override width/height
            if (!string.IsNullOrWhiteSpace(room.id) && string.IsNullOrWhiteSpace(room.roomId))
                room.roomId = room.id;
            if (room.version >= 2 && room.size != null)
            {
                if (room.size.w > 0) room.width = room.size.w;
                if (room.size.h > 0) room.height = room.size.h;
            }
            // v2: if no grid but has walkable array, use walkable as grid (pure '#'/'.')
            if ((room.grid == null || room.grid.Length == 0) && room.walkable != null && room.walkable.Length > 0)
                room.grid = room.walkable;

            string roomId = string.IsNullOrWhiteSpace(room.roomId) ? string.Empty : room.roomId.Trim();
            if (string.IsNullOrEmpty(roomId))
            {
                report.Error("Skipped room with missing roomId.");
                return false;
            }

            if (room.width <= 0 || room.height <= 0)
            {
                report.Error($"{roomId}: skipped because width/height must be positive.");
                return false;
            }

            string assetName = SanitizeAssetName(roomId, report);
            string assetPath = $"{OutputFolder}/{assetName}.asset";
            RoomBuildData buildData = BuildRoomData(room, report);

            RoomTemplateSO template = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(assetPath);
            if (template == null)
            {
                template = ScriptableObject.CreateInstance<RoomTemplateSO>();
                FillTemplate(template, room, buildData);
                AssetDatabase.CreateAsset(template, assetPath);
            }
            else
            {
                FillTemplate(template, room, buildData);
                EditorUtility.SetDirty(template);
            }

            importedId = roomId;
            return true;
        }

        private static RoomBuildData BuildRoomData(RoomJson room, ImportReport report)
        {
            var data = new RoomBuildData(room.width, room.height, report);
            bool playerFound = false;
            int chestCount = 0;

            if (!string.IsNullOrWhiteSpace(room.notes))
            {
                data.encounterTags.Add("json_notes_present");
            }

            // --- walkable grid (v1 marker parse OR v2 pure '#'/'.' parse) ---
            bool isV2 = room.version >= 2;

            if (room.grid == null)
            {
                report.Warning($"{room.roomId}: grid missing; generated all-void walkable grid and fallback player spawn.");
            }
            else if (room.grid.Length != room.height)
            {
                report.Warning($"{room.roomId}: grid row count {room.grid.Length} does not match height {room.height}; missing rows are void, extra rows ignored.");
            }

            for (int inputY = 0; inputY < room.height; inputY++)
            {
                string row = GetRow(room, inputY, report);
                int tileY = ToTemplateY(inputY, room.height);
                for (int x = 0; x < room.width; x++)
                {
                    char c = x < row.Length ? row[x] : ' ';
                    Vector2Int position = new Vector2Int(x, tileY);

                    if (isV2)
                    {
                        // v2: '#' = walkable, '.' = void; no embedded markers
                        data.walkableGrid[(tileY * room.width) + x] = (c == '#');
                    }
                    else
                    {
                        data.walkableGrid[(tileY * room.width) + x] = IsWalkable(c, room.roomId, position, report);

                        if (c == 'P' && !playerFound)
                        {
                            data.playerSpawn = position;
                            playerFound = true;
                        }
                        else if (c == 'P')
                        {
                            report.Warning($"{room.roomId}: multiple P markers found; using the first player spawn.");
                        }
                        else if (c == 'e')
                        {
                            data.enemySpawns.Add(new EnemySpawnSocket
                            {
                                socketId = $"enemy_spawn_{data.enemySpawns.Count + 1:00}",
                                position = position,
                                tierHint = "standard",
                                avoidRadius = 1.5f
                            });
                        }
                        else if (c == 'B')
                        {
                            data.enemySpawns.Add(new EnemySpawnSocket
                            {
                                socketId = $"boss_spawn_{data.enemySpawns.Count + 1:00}",
                                position = position,
                                tierHint = "boss",
                                avoidRadius = 3f
                            });
                        }
                        else if (c == 'C')
                        {
                            chestCount++;
                            data.chestMarkerTags.Add($"chest_marker_{x}_{tileY}");
                        }
                    }
                }
            }

            // --- spawn position ---
            if (isV2 && room.spawn != null)
            {
                // v2 spawn: JSON coords (jsonY = h-1-gridY) → invert back
                int spawnGridY = (room.height - 1) - room.spawn.y;
                data.playerSpawn = new Vector2Int(room.spawn.x, spawnGridY);
                playerFound = true;
            }

            if (!playerFound)
            {
                data.playerSpawn = FindFallbackPlayerSpawn(data.walkableGrid, room.width, room.height);
                if (!isV2)
                    report.Warning($"{room.roomId}: no P marker found; player spawn fell back to {data.playerSpawn}.");
            }

            if (chestCount > 0)
            {
                report.Warning($"{room.roomId}: {chestCount} C marker(s) preserved as tags only; RoomTemplateSO has no chest socket field.");
            }

            // --- door sockets ---
            if (isV2 && room.exitSlots != null)
            {
                data.doorSockets.AddRange(BuildExitSlotsV2(room, report));
            }
            else
            {
                data.doorSockets.AddRange(BuildDoorSockets(room, report));
            }

            // --- v2 enemy spawns ---
            if (isV2 && room.enemySpawns != null)
            {
                for (int i = 0; i < room.enemySpawns.Length; i++)
                {
                    RoomEnemyJson e = room.enemySpawns[i];
                    if (e == null) continue;
                    int gridY = (room.height - 1) - e.y;
                    data.enemySpawns.Add(new EnemySpawnSocket
                    {
                        socketId = $"enemy_spawn_{data.enemySpawns.Count + 1:00}",
                        position = new Vector2Int(e.x, gridY),
                        tierHint = string.IsNullOrEmpty(e.tier) ? "standard" : e.tier,
                        avoidRadius = 1.5f
                    });
                }
            }

            // --- v2 props ---
            if (isV2)
            {
                data.hasPropsArray = true; // always true for v2 even if array is empty
                data.incomingProps = new List<RIMA.MapDesigner.Props.PropPlacementData>();
                if (room.props != null)
                {
                    for (int i = 0; i < room.props.Length; i++)
                    {
                        RoomPropJson p = room.props[i];
                        if (p == null || string.IsNullOrEmpty(p.id)) continue;
                        int gridY = (room.height - 1) - p.y;
                        data.incomingProps.Add(new RIMA.MapDesigner.Props.PropPlacementData
                        {
                            propDefinitionGuid = p.id,
                            tilePosition = new Vector2Int(p.x, gridY),
                            variantIndex = p.variant,
                            flipX = p.flipX
                        });
                    }
                }
            }
            // v1: hasPropsArray remains false → props preserved

            return data;
        }

        private static List<DoorSocket> BuildExitSlotsV2(RoomJson room, ImportReport report)
        {
            var sockets = new List<DoorSocket>();
            ExitSlotsJson slots = room.exitSlots;
            if (slots == null) return sockets;

            void AddSlot(ExitSlotPosJson pos, string socketId, string slotName)
            {
                if (pos == null) return;
                int gridY = (room.height - 1) - pos.y;
                sockets.Add(new DoorSocket
                {
                    socketId = socketId,
                    position = new Vector2Int(pos.x, gridY),
                    direction = DoorDirection.North,
                    widthInTiles = 2,
                    isExit = true
                });
            }

            AddSlot(slots.NW, RoomTemplateSO.ExitSlotNorthWestId, "NW");
            AddSlot(slots.N,  RoomTemplateSO.ExitSlotNorthId,     "N");
            AddSlot(slots.NE, RoomTemplateSO.ExitSlotNorthEastId, "NE");
            return sockets;
        }

        private static string GetRow(RoomJson room, int inputY, ImportReport report)
        {
            if (room.grid == null || inputY >= room.grid.Length || room.grid[inputY] == null)
            {
                return string.Empty;
            }

            string row = room.grid[inputY];
            if (row.Length < room.width)
            {
                report.Warning($"{room.roomId}: row {inputY} length {row.Length} < width {room.width}; padded with void.");
            }
            else if (row.Length > room.width)
            {
                report.Warning($"{room.roomId}: row {inputY} length {row.Length} > width {room.width}; extra chars ignored.");
            }

            return row;
        }

        private static bool IsWalkable(char c, string roomId, Vector2Int position, ImportReport report)
        {
            switch (c)
            {
                case '.':
                case 'P':
                case 'e':
                case 'C':
                case 'B':
                    return true;
                case ' ':
                    return false;
                default:
                    report.Warning($"{roomId}: unknown grid char '{c}' at {position}; treated as void.");
                    return false;
            }
        }

        private static List<DoorSocket> BuildDoorSockets(RoomJson room, ImportReport report)
        {
            var sockets = new List<DoorSocket>();
            if (room.doors == null)
            {
                report.Warning($"{room.roomId}: doors array missing.");
                return sockets;
            }

            foreach (DoorJson door in room.doors)
            {
                if (door == null)
                {
                    report.Warning($"{room.roomId}: skipped null door entry.");
                    continue;
                }

                if (!TryMapDoorDirection(door.dir, out DoorDirection direction, out string suffix))
                {
                    if (string.Equals(door.dir, "S", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(door.dir, "South", StringComparison.OrdinalIgnoreCase))
                    {
                        report.Warning($"{room.roomId}: south door at ({door.x},{door.y}) skipped by rule.");
                    }
                    else
                    {
                        report.Warning($"{room.roomId}: unknown door direction '{door.dir}' skipped.");
                    }
                    continue;
                }

                int x = ClampWithWarning(door.x, 0, room.width - 1, $"{room.roomId}: door x", report);
                int inputY = ClampWithWarning(door.y, 0, room.height - 1, $"{room.roomId}: door y", report);
                sockets.Add(new DoorSocket
                {
                    socketId = $"door_{suffix}_{sockets.Count + 1:00}",
                    position = new Vector2Int(x, ToTemplateY(inputY, room.height)),
                    direction = direction,
                    widthInTiles = 2,
                    isExit = true
                });
            }

            return sockets;
        }

        private static bool TryMapDoorDirection(string raw, out DoorDirection direction, out string suffix)
        {
            switch ((raw ?? string.Empty).Trim().ToUpperInvariant())
            {
                case "N":
                case "NORTH":
                    direction = DoorDirection.North;
                    suffix = "N";
                    return true;
                case "E":
                case "EAST":
                    direction = DoorDirection.East;
                    suffix = "E";
                    return true;
                case "W":
                case "WEST":
                    direction = DoorDirection.West;
                    suffix = "W";
                    return true;
                default:
                    direction = DoorDirection.North;
                    suffix = "X";
                    return false;
            }
        }

        private static int ClampWithWarning(int value, int min, int max, string label, ImportReport report)
        {
            int clamped = Mathf.Clamp(value, min, max);
            if (clamped != value)
            {
                report.Warning($"{label} {value} outside [{min},{max}]; clamped to {clamped}.");
            }

            return clamped;
        }

        private static Vector2Int FindFallbackPlayerSpawn(bool[] walkableGrid, int width, int height)
        {
            Vector2 center = new Vector2((width - 1) * 0.5f, (height - 1) * 0.5f);
            Vector2Int best = new Vector2Int(width / 2, height / 2);
            float bestDistance = float.MaxValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width) + x;
                    if (!walkableGrid[index])
                    {
                        continue;
                    }

                    float distance = (new Vector2(x, y) - center).sqrMagnitude;
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        best = new Vector2Int(x, y);
                    }
                }
            }

            return best;
        }

        private static void FillTemplate(RoomTemplateSO template, RoomJson room, RoomBuildData data)
        {
            RIMA.RoomType roomType = MapRoomType(room.roomType, room.roomId, data.difficultyTags, data.encounterTags, data.report);

            template.schemaVersion = "1.0";
            template.roomId = room.roomId.Trim();
            template.biomeId = DefaultBiomeId;
            template.roomType = roomType;
            template.bounds = new RectInt(0, 0, room.width, room.height);
            template.cameraBounds = CameraBounds.FromBounds(template.bounds);
            template.prefabRef = null;
            template.doorSockets = data.doorSockets;
            template.playerSpawn = new PlayerSpawnSocket
            {
                socketId = "player_spawn_01",
                position = data.playerSpawn,
                facing = DoorDirection.South
            };
            template.enemySpawnSockets = data.enemySpawns;
            template.backgroundLayers = new List<BackgroundLayerData>();
            template.encounterTags = data.encounterTags;
            template.difficultyTags = data.difficultyTags;
            template.blockerTags = data.chestMarkerTags;
            // CRITICAL FIX (Flash finding): preserve existing props when incoming JSON has no props array.
            // Only replace when data.hasPropsArray is true (incoming JSON explicitly supplies props).
            if (data.hasPropsArray)
            {
                template.props = data.incomingProps ?? new List<RIMA.MapDesigner.Props.PropPlacementData>();
            }
            else if (template.props == null)
            {
                template.props = new List<RIMA.MapDesigner.Props.PropPlacementData>();
            }
            // else: keep existing template.props unchanged
            template.walkableGrid = data.walkableGrid;
        }

        private static RIMA.RoomType MapRoomType(string raw, string roomId, List<string> difficultyTags, List<string> encounterTags, ImportReport report)
        {
            string normalized = (raw ?? string.Empty).Trim().Replace("_", string.Empty).Replace("-", string.Empty).ToLowerInvariant();
            switch (normalized)
            {
                case "combat":
                    encounterTags.Add("combat");
                    return RIMA.RoomType.Combat;
                case "combatlarge":
                case "largecombat":
                    encounterTags.Add("combat");
                    difficultyTags.Add("combat_large");
                    return RIMA.RoomType.Combat;
                case "spawn":
                    encounterTags.Add("spawn");
                    return RIMA.RoomType.Combat;
                case "elite":
                    encounterTags.Add("elite");
                    return RIMA.RoomType.Elite;
                case "boss":
                    encounterTags.Add("boss");
                    return RIMA.RoomType.Boss;
                case "chest":
                    encounterTags.Add("chest");
                    return RIMA.RoomType.Chest;
                case "merchant":
                    encounterTags.Add("merchant");
                    return RIMA.RoomType.Merchant;
                case "forge":
                    encounterTags.Add("forge");
                    return RIMA.RoomType.Forge;
                case "event":
                    encounterTags.Add("event");
                    return RIMA.RoomType.Event;
                case "curse":
                    encounterTags.Add("curse");
                    return RIMA.RoomType.Curse;
                case "corridor":
                    encounterTags.Add("corridor");
                    return RIMA.RoomType.Corridor;
                default:
                    report.Warning($"{roomId}: unknown roomType '{raw}', imported as Combat.");
                    encounterTags.Add("combat");
                    difficultyTags.Add("unknown_type_fallback");
                    return RIMA.RoomType.Combat;
            }
        }

        private static string SanitizeAssetName(string roomId, ImportReport report)
        {
            char[] chars = roomId.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (!char.IsLetterOrDigit(chars[i]) && chars[i] != '_' && chars[i] != '-')
                {
                    chars[i] = '_';
                }
            }

            string sanitized = new string(chars);
            if (sanitized != roomId)
            {
                report.Warning($"{roomId}: asset file name sanitized to {sanitized}.");
            }

            return sanitized;
        }

        private static int ToTemplateY(int inputY, int height)
        {
            return height - 1 - inputY;
        }

        private static void EnsureFolder(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        [Serializable]
        private class RoomJsonDocument
        {
            public RoomJson[] rooms;
        }

        [Serializable]
        private class RoomJson
        {
            // v1 fields
            public string roomId;
            public string roomType;
            public int width;
            public int height;
            public string[] grid;       // v1: grid with P/e/B/C markers; v2: pure '#'/'.' walkable
            public DoorJson[] doors;    // v1 doors array
            public string notes;

            // v2 additional fields (JsonUtility ignores unknown keys; missing = default)
            public int version;         // 0 = v1 (not set), 2 = v2
            public string id;           // v2 roomId alias
            public RoomSizeJson size;   // v2 size object {w, h}
            public string[] walkable;   // v2 walkable rows (pure '#'/'.')
            public RoomSpawnJson spawn; // v2 spawn {x, y}
            public ExitSlotsJson exitSlots; // v2 named exit slots
            public RoomPropJson[] props;    // v2 props array
            public RoomEnemyJson[] enemySpawns; // v2 enemy spawns
        }

        [Serializable]
        private class RoomSizeJson
        {
            public int w;
            public int h;
        }

        [Serializable]
        private class RoomSpawnJson
        {
            public int x;
            public int y;
        }

        [Serializable]
        private class ExitSlotsJson
        {
            public ExitSlotPosJson NW;
            public ExitSlotPosJson N;
            public ExitSlotPosJson NE;
        }

        [Serializable]
        private class ExitSlotPosJson
        {
            public int x;
            public int y;
        }

        [Serializable]
        private class RoomPropJson
        {
            public string id;   // propDefinitionGuid or propName
            public int x;
            public int y;
            public int variant = -1;
            public bool flipX;
        }

        [Serializable]
        private class RoomEnemyJson
        {
            public int x;
            public int y;
            public string tier;
        }

        [Serializable]
        private class DoorJson
        {
            public string dir;
            public int x;
            public int y;
        }

        private class RoomBuildData
        {
            public readonly ImportReport report;
            public readonly bool[] walkableGrid;
            public readonly List<DoorSocket> doorSockets = new List<DoorSocket>();
            public readonly List<EnemySpawnSocket> enemySpawns = new List<EnemySpawnSocket>();
            public readonly List<string> encounterTags = new List<string> { "json_imported" };
            public readonly List<string> difficultyTags = new List<string>();
            public readonly List<string> chestMarkerTags = new List<string>();
            public Vector2Int playerSpawn;
            // schema-v2 props support
            public bool hasPropsArray;
            public List<RIMA.MapDesigner.Props.PropPlacementData> incomingProps;

            public RoomBuildData(int width, int height, ImportReport report)
            {
                this.report = report;
                walkableGrid = new bool[width * height];
            }
        }

        private class ImportReport
        {
            public int WarningCount { get; private set; }
            public int ErrorCount { get; private set; }

            public void Warning(string message)
            {
                WarningCount++;
                Debug.LogWarning($"[RoomJsonImporter] {message}");
            }

            public void Error(string message)
            {
                ErrorCount++;
                Debug.LogError($"[RoomJsonImporter] {message}");
            }
        }
    }
}
#endif
