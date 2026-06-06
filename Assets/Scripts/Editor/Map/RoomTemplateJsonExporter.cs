#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor.Map
{
    /// <summary>
    /// Deterministic RoomTemplateSO → schema-v2 JSON exporter.
    /// Output path: STAGING/rooms_json/&lt;roomId&gt;.json (outside Unity asset watcher).
    /// Y-flip rule: jsonRow 0 = top (north), gridY = bottom-origin.
    ///   jsonY = (height - 1 - gridY)
    /// Diff-check: only writes if payload differs from existing file.
    /// </summary>
    public static class RoomTemplateJsonExporter
    {
        private const string JsonOutputFolder = "STAGING/rooms_json";
        private const int SchemaVersion = 2;

        // ── public API ───────────────────────────────────────────────────────

        /// <summary>Export a single template. Returns true if file was written (payload changed or new).</summary>
        public static bool Export(RoomTemplateSO template)
        {
            if (template == null) return false;
            string roomId = string.IsNullOrEmpty(template.roomId) ? template.name : template.roomId;
            string json = BuildJson(template, roomId);
            return WriteIfChanged(roomId, json);
        }

        /// <summary>Export all RoomTemplateSO assets found under Assets/Data/Rooms.</summary>
        [MenuItem("RIMA/Rooms/Export All JSON (v2)")]
        public static void ExportAll()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { "Assets/Data/Rooms" });
            int written = 0;
            int skipped = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                RoomTemplateSO template = AssetDatabase.LoadAssetAtPath<RoomTemplateSO>(path);
                if (template == null) continue;
                bool changed = Export(template);
                if (changed) written++; else skipped++;
            }
            Debug.Log($"[RoomTemplateJsonExporter] Export All: {written} written, {skipped} unchanged. Total={guids.Length}");
        }

        // ── JSON builder ─────────────────────────────────────────────────────

        public static string BuildJson(RoomTemplateSO template, string roomId)
        {
            int w = template.bounds.width;
            int h = template.bounds.height;

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"version\": {SchemaVersion},");
            sb.AppendLine($"  \"id\": \"{EscapeJson(roomId)}\",");
            sb.AppendLine($"  \"roomType\": \"{EscapeJson(template.roomType.ToString())}\",");
            sb.AppendLine($"  \"size\": {{\"w\": {w}, \"h\": {h}}},");

            // walkable rows: row 0 = north (highest gridY = h-1)
            sb.AppendLine("  \"walkable\": [");
            for (int jsonRow = 0; jsonRow < h; jsonRow++)
            {
                int gridY = (h - 1) - jsonRow; // Y-flip
                var rowSb = new StringBuilder(w);
                for (int x = 0; x < w; x++)
                {
                    Vector2Int tile = new Vector2Int(template.bounds.xMin + x, template.bounds.yMin + gridY);
                    rowSb.Append(template.IsWalkable(tile) ? '#' : '.');
                }
                bool last = (jsonRow == h - 1);
                sb.AppendLine($"    \"{rowSb}\"{(last ? "" : ",")}");
            }
            sb.AppendLine("  ],");

            // spawn: Y-flip
            if (template.playerSpawn != null)
            {
                int spawnLocalX = template.playerSpawn.position.x - template.bounds.xMin;
                int spawnLocalY = template.playerSpawn.position.y - template.bounds.yMin;
                int spawnJsonY = (h - 1) - spawnLocalY;
                sb.AppendLine($"  \"spawn\": {{\"x\": {spawnLocalX}, \"y\": {spawnJsonY}}},");
            }
            else
            {
                sb.AppendLine("  \"spawn\": null,");
            }

            // exitSlots: named NW/N/NE from door_NW_01/door_N_01/door_NE_01
            DoorSocket[] slots = template.ResolveExitSlots();
            sb.AppendLine("  \"exitSlots\": {");
            string[] slotNames = { "NW", "N", "NE" };
            List<string> slotLines = new List<string>();
            for (int i = 0; i < slots.Length; i++)
            {
                DoorSocket slot = slots[i];
                if (slot == null) continue;
                int lx = slot.position.x - template.bounds.xMin;
                int ly = slot.position.y - template.bounds.yMin;
                int jy = (h - 1) - ly;
                slotLines.Add($"    \"{slotNames[i]}\": {{\"x\": {lx}, \"y\": {jy}}}");
            }
            sb.AppendLine(string.Join(",\n", slotLines));
            sb.AppendLine("  },");

            // props
            sb.AppendLine("  \"props\": [");
            if (template.props != null && template.props.Count > 0)
            {
                List<string> propLines = new List<string>();
                for (int i = 0; i < template.props.Count; i++)
                {
                    PropPlacementData p = template.props[i];
                    if (p == null) continue;
                    int plx = p.tilePosition.x - template.bounds.xMin;
                    int ply = p.tilePosition.y - template.bounds.yMin;
                    int pjy = (h - 1) - ply;
                    // resolve display name for comment (cheap lookup)
                    string nameComment = ResolveDisplayName(p.propDefinitionGuid);
                    string nameHint = string.IsNullOrEmpty(nameComment) ? "" : $" /* {nameComment} */";
                    propLines.Add(
                        $"    {{\"id\": \"{EscapeJson(p.propDefinitionGuid)}\", \"x\": {plx}, \"y\": {pjy}, \"variant\": {p.variantIndex}, \"flipX\": {(p.flipX ? "true" : "false")}}}{nameHint}");
                }
                sb.AppendLine(string.Join(",\n", propLines));
            }
            sb.AppendLine("  ],");

            // enemySpawns
            sb.AppendLine("  \"enemySpawns\": [");
            if (template.enemySpawnSockets != null && template.enemySpawnSockets.Count > 0)
            {
                List<string> enemyLines = new List<string>();
                for (int i = 0; i < template.enemySpawnSockets.Count; i++)
                {
                    EnemySpawnSocket e = template.enemySpawnSockets[i];
                    if (e == null) continue;
                    int elx = e.position.x - template.bounds.xMin;
                    int ely = e.position.y - template.bounds.yMin;
                    int ejy = (h - 1) - ely;
                    string tier = string.IsNullOrEmpty(e.tierHint) ? "standard" : e.tierHint;
                    enemyLines.Add($"    {{\"x\": {elx}, \"y\": {ejy}, \"tier\": \"{EscapeJson(tier)}\"}}");
                }
                sb.AppendLine(string.Join(",\n", enemyLines));
            }
            sb.AppendLine("  ],");

            sb.Append("  \"notes\": \"\"");
            sb.AppendLine();
            sb.Append("}");

            return sb.ToString();
        }

        // ── file I/O ─────────────────────────────────────────────────────────

        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        private static bool WriteIfChanged(string roomId, string json)
        {
            EnsureOutputFolder();
            string safeName = SanitizeFileName(roomId);
            string absPath = Path.GetFullPath(Path.Combine(JsonOutputFolder, safeName + ".json"));
            if (File.Exists(absPath))
            {
                // Read raw bytes to strip any BOM before comparison
                byte[] existingBytes = File.ReadAllBytes(absPath);
                string existing = StripBom(existingBytes);
                if (existing == json) return false;
            }
            File.WriteAllBytes(absPath, Utf8NoBom.GetBytes(json));
            return true;
        }

        private static string StripBom(byte[] bytes)
        {
            // Strip UTF-8 BOM (ef bb bf) if present
            int start = (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF) ? 3 : 0;
            return Encoding.UTF8.GetString(bytes, start, bytes.Length - start);
        }

        private static void EnsureOutputFolder()
        {
            string abs = Path.GetFullPath(JsonOutputFolder);
            if (!Directory.Exists(abs))
            {
                Directory.CreateDirectory(abs);
            }
        }

        // ── helpers ──────────────────────────────────────────────────────────

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string SanitizeFileName(string id)
        {
            char[] chars = id.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (!char.IsLetterOrDigit(chars[i]) && chars[i] != '_' && chars[i] != '-')
                    chars[i] = '_';
            }
            return new string(chars);
        }

        private static string ResolveDisplayName(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return string.Empty;
            try
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path)) return string.Empty;
                PropDefinitionSO prop = AssetDatabase.LoadAssetAtPath<PropDefinitionSO>(path);
                return prop != null ? prop.displayName : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
#endif
