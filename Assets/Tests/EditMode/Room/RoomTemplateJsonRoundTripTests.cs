#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Editor.Map;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Room
{
    /// <summary>
    /// Round-trip tests: RoomTemplateSO → schema-v2 JSON (via RoomTemplateJsonExporter.BuildJson)
    /// → parsed back → assert walkable/spawn/slots/props identical (Y-flip correctness included).
    /// Does NOT touch the file system or AssetDatabase.
    /// </summary>
    public class RoomTemplateJsonRoundTripTests
    {
        // ── helpers ──────────────────────────────────────────────────────────

        private static RoomTemplateSO MakeTemplate(int w, int h)
        {
            var t = ScriptableObject.CreateInstance<RoomTemplateSO>();
            t.roomId = "test_roundtrip_01";
            t.roomType = RIMA.RoomType.Combat;
            t.bounds = new RectInt(0, 0, w, h);
            t.biomeId = "TestBiome";

            // fill walkable grid: all walkable except top two rows (y = h-1, h-2) = void
            t.walkableGrid = new bool[w * h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    t.walkableGrid[y * w + x] = (y < h - 2); // top 2 rows void, rest walkable

            // player spawn at (4, 2) (gridY=2, local coords)
            t.playerSpawn = new PlayerSpawnSocket
            {
                socketId = "player_spawn_01",
                position = new Vector2Int(4, 2),
                facing = RIMA.DoorDirection.South
            };

            // exit slots: NW at (2, h-3), N at (w/2, h-3), NE at (w-3, h-3)
            int slotY = h - 3;
            t.doorSockets = new List<DoorSocket>
            {
                new DoorSocket { socketId = RoomTemplateSO.ExitSlotNorthWestId, position = new Vector2Int(2, slotY),     direction = RIMA.DoorDirection.North, isExit = true, widthInTiles = 2 },
                new DoorSocket { socketId = RoomTemplateSO.ExitSlotNorthId,     position = new Vector2Int(w / 2, slotY), direction = RIMA.DoorDirection.North, isExit = true, widthInTiles = 2 },
                new DoorSocket { socketId = RoomTemplateSO.ExitSlotNorthEastId, position = new Vector2Int(w - 3, slotY), direction = RIMA.DoorDirection.North, isExit = true, widthInTiles = 2 }
            };

            // a couple of props (guid strings are fake UUIDs for the test)
            t.props = new List<PropPlacementData>
            {
                new PropPlacementData { propDefinitionGuid = "aabbccdd-0001", tilePosition = new Vector2Int(1, 1), variantIndex = 0, flipX = false },
                new PropPlacementData { propDefinitionGuid = "aabbccdd-0002", tilePosition = new Vector2Int(3, 3), variantIndex = 1, flipX = true }
            };

            // enemy spawns
            t.enemySpawnSockets = new List<EnemySpawnSocket>
            {
                new EnemySpawnSocket { socketId = "enemy_spawn_01", position = new Vector2Int(5, 4), tierHint = "standard", avoidRadius = 1.5f }
            };

            return t;
        }

        // ── manual JSON parsing helpers (no AssetDatabase) ──────────────────

        /// <summary>
        /// Very thin JSON reader that handles the schema-v2 we produce.
        /// Uses only string scanning — avoids JsonUtility (which needs Serializable attrs).
        /// </summary>
        private static class MiniJson
        {
            public static string[] ParseWalkableRows(string json)
            {
                // Find "walkable": [ ... ]
                int start = json.IndexOf("\"walkable\"", StringComparison.Ordinal);
                if (start < 0) return Array.Empty<string>();
                int bracket = json.IndexOf('[', start);
                int endBracket = json.IndexOf(']', bracket);
                string block = json.Substring(bracket + 1, endBracket - bracket - 1);
                // each row is a quoted string
                var rows = new List<string>();
                int pos = 0;
                while (pos < block.Length)
                {
                    int q1 = block.IndexOf('"', pos);
                    if (q1 < 0) break;
                    int q2 = block.IndexOf('"', q1 + 1);
                    if (q2 < 0) break;
                    rows.Add(block.Substring(q1 + 1, q2 - q1 - 1));
                    pos = q2 + 1;
                }
                return rows.ToArray();
            }

            public static (int x, int y) ParseSpawn(string json)
            {
                int idx = json.IndexOf("\"spawn\"", StringComparison.Ordinal);
                if (idx < 0) return (-1, -1);
                return ParseXY(json, idx);
            }

            public static (int x, int y) ParseSlot(string json, string slotName)
            {
                // "exitSlots": { "NW": {"x":..,"y":..}, ... }
                int slots = json.IndexOf("\"exitSlots\"", StringComparison.Ordinal);
                if (slots < 0) return (-1, -1);
                int slotIdx = json.IndexOf($"\"{slotName}\"", slots);
                if (slotIdx < 0) return (-1, -1);
                return ParseXY(json, slotIdx);
            }

            private static (int x, int y) ParseXY(string json, int searchFrom)
            {
                int obj = json.IndexOf('{', searchFrom);
                if (obj < 0) return (-1, -1);
                int close = json.IndexOf('}', obj);
                string block = json.Substring(obj + 1, close - obj - 1);
                int x = ParseIntField(block, "x");
                int y = ParseIntField(block, "y");
                return (x, y);
            }

            private static int ParseIntField(string block, string field)
            {
                string key = $"\"{field}\"";
                int idx = block.IndexOf(key, StringComparison.Ordinal);
                if (idx < 0) return -1;
                int colon = block.IndexOf(':', idx);
                int start = colon + 1;
                while (start < block.Length && (block[start] == ' ' || block[start] == '\n' || block[start] == '\r')) start++;
                int end = start;
                while (end < block.Length && (char.IsDigit(block[end]) || block[end] == '-')) end++;
                if (end == start) return -1;
                return int.Parse(block.Substring(start, end - start));
            }

            public static List<(string id, int x, int y, int variant, bool flipX)> ParseProps(string json)
            {
                var result = new List<(string, int, int, int, bool)>();
                int propsIdx = json.IndexOf("\"props\"", StringComparison.Ordinal);
                if (propsIdx < 0) return result;
                int bracket = json.IndexOf('[', propsIdx);
                int endBracket = FindMatchingBracket(json, bracket, '[', ']');
                string block = json.Substring(bracket + 1, endBracket - bracket - 1);
                // parse each {...} object
                int pos = 0;
                while (pos < block.Length)
                {
                    int ob = block.IndexOf('{', pos);
                    if (ob < 0) break;
                    int cb = FindMatchingBracket(block, ob, '{', '}');
                    string obj = block.Substring(ob + 1, cb - ob - 1);
                    string id = ParseStringField(obj, "id");
                    int x = ParseIntField(obj, "x");
                    int y = ParseIntField(obj, "y");
                    int variant = ParseIntField(obj, "variant");
                    bool flipX = obj.IndexOf("\"flipX\": true", StringComparison.Ordinal) >= 0
                              || obj.IndexOf("\"flipX\":true", StringComparison.Ordinal) >= 0;
                    if (!string.IsNullOrEmpty(id))
                        result.Add((id, x, y, variant, flipX));
                    pos = cb + 1;
                }
                return result;
            }

            private static string ParseStringField(string block, string field)
            {
                string key = $"\"{field}\"";
                int idx = block.IndexOf(key, StringComparison.Ordinal);
                if (idx < 0) return string.Empty;
                int colon = block.IndexOf(':', idx);
                int q1 = block.IndexOf('"', colon + 1);
                if (q1 < 0) return string.Empty;
                int q2 = block.IndexOf('"', q1 + 1);
                if (q2 < 0) return string.Empty;
                return block.Substring(q1 + 1, q2 - q1 - 1);
            }

            private static int FindMatchingBracket(string s, int start, char open, char close)
            {
                int depth = 0;
                for (int i = start; i < s.Length; i++)
                {
                    if (s[i] == open) depth++;
                    else if (s[i] == close) { depth--; if (depth == 0) return i; }
                }
                return s.Length - 1;
            }
        }

        // ── tests ────────────────────────────────────────────────────────────

        [Test]
        public void ExportBuildJson_WalkableRows_YFlipCorrect()
        {
            int w = 10, h = 8;
            RoomTemplateSO t = MakeTemplate(w, h);
            string json = RoomTemplateJsonExporter.BuildJson(t, t.roomId);

            string[] rows = MiniJson.ParseWalkableRows(json);
            Assert.AreEqual(h, rows.Length, "Row count must equal template height.");

            // Our template: gridY < h-2 is walkable → jsonRow 0 (top=north) = gridY=h-1 = void
            // jsonRow 0 → gridY = h-1-0 = h-1 → not walkable (row of '.')
            // jsonRow 1 → gridY = h-2 → not walkable (row of '.')
            // jsonRow 2 → gridY = h-3 → walkable (row of '#')
            Assert.IsTrue(rows[0].Replace(".", "").Length == 0, $"jsonRow 0 should be all void but got: {rows[0]}");
            Assert.IsTrue(rows[1].Replace(".", "").Length == 0, $"jsonRow 1 should be all void but got: {rows[1]}");
            Assert.IsTrue(rows[2].Replace("#", "").Length == 0 || rows[2].Contains("#"),
                $"jsonRow 2 should contain walkable cells but got: {rows[2]}");
        }

        [Test]
        public void ExportBuildJson_SpawnPosition_YFlipCorrect()
        {
            int w = 10, h = 8;
            RoomTemplateSO t = MakeTemplate(w, h);
            // playerSpawn at gridPos (4, 2)
            string json = RoomTemplateJsonExporter.BuildJson(t, t.roomId);

            (int sx, int sy) = MiniJson.ParseSpawn(json);
            Assert.AreEqual(4, sx, "Spawn X must be preserved.");
            // Y-flip: jsonY = (h-1) - gridY = 7 - 2 = 5
            Assert.AreEqual(h - 1 - 2, sy, "Spawn Y must be Y-flipped: jsonY = (h-1) - gridY.");
        }

        [Test]
        public void ExportBuildJson_ExitSlots_YFlipCorrect()
        {
            int w = 12, h = 10;
            RoomTemplateSO t = MakeTemplate(w, h);
            // N slot at gridY = h-3 = 7, x = w/2 = 6
            string json = RoomTemplateJsonExporter.BuildJson(t, t.roomId);

            (int nx, int ny) = MiniJson.ParseSlot(json, "N");
            Assert.AreEqual(w / 2, nx, "N slot X must match.");
            // jsonY = (h-1) - (h-3) = 2
            Assert.AreEqual(2, ny, "N slot Y must be Y-flipped.");

            (int nwx, int nwy) = MiniJson.ParseSlot(json, "NW");
            Assert.AreEqual(2, nwx, "NW slot X must match.");
            Assert.AreEqual(2, nwy, "NW slot Y must be Y-flipped.");
        }

        [Test]
        public void ExportBuildJson_Props_RoundTrip()
        {
            int w = 10, h = 8;
            RoomTemplateSO t = MakeTemplate(w, h);
            // prop at (1, 1) → jsonY = (h-1) - 1 = 6
            string json = RoomTemplateJsonExporter.BuildJson(t, t.roomId);

            var props = MiniJson.ParseProps(json);
            Assert.AreEqual(2, props.Count, "Should have exported 2 props.");

            // first prop at tile (1,1)
            var p0 = props[0];
            Assert.AreEqual("aabbccdd-0001", p0.id);
            Assert.AreEqual(1, p0.x);
            Assert.AreEqual(h - 1 - 1, p0.y, "Prop Y must be Y-flipped.");
            Assert.IsFalse(p0.flipX);

            // second prop at tile (3,3)
            var p1 = props[1];
            Assert.AreEqual("aabbccdd-0002", p1.id);
            Assert.AreEqual(3, p1.x);
            Assert.AreEqual(h - 1 - 3, p1.y, "Prop Y must be Y-flipped.");
            Assert.IsTrue(p1.flipX);
        }

        [Test]
        public void ExportBuildJson_WalkableGrid_FullRoundTrip()
        {
            int w = 8, h = 6;
            RoomTemplateSO t = MakeTemplate(w, h);
            string json = RoomTemplateJsonExporter.BuildJson(t, t.roomId);

            string[] rows = MiniJson.ParseWalkableRows(json);

            // Verify every cell round-trips: jsonRow → gridY → IsWalkable must match row char
            for (int jsonRow = 0; jsonRow < h; jsonRow++)
            {
                int gridY = (h - 1) - jsonRow;
                string row = rows[jsonRow];
                for (int x = 0; x < w; x++)
                {
                    bool expectedWalkable = t.IsWalkable(new Vector2Int(t.bounds.xMin + x, t.bounds.yMin + gridY));
                    char expected = expectedWalkable ? '#' : '.';
                    char actual = x < row.Length ? row[x] : '.';
                    Assert.AreEqual(expected, actual,
                        $"Mismatch at jsonRow={jsonRow} x={x} (gridY={gridY}): expected '{expected}' got '{actual}'");
                }
            }
        }
    }
}
#endif
