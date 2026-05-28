using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Live;
using RIMA.RoomPainter;
using RIMA.RoomPainter.Editor;

namespace RIMA.Editor.RoomPainter.LiveTool
{
    /// <summary>
    /// C3 — RuntimeAssetRegistryBaker (F2).
    /// Editor-only. Scans the canonical scan roots and bakes a RuntimeAssetRegistry.asset
    /// at Assets/Resources/Live/RuntimeAssetRegistry.asset.
    /// Also writes a registry_manifest.txt diff file alongside the asset so CI / user can
    /// detect dropped GUIDs across bake sessions.
    /// Menu: RIMA → Live Tool → Bake Asset Registry
    /// </summary>
    public static class RuntimeAssetRegistryBaker
    {
        // ── Constants ──────────────────────────────────────────────────────────

        private const string RegistryAssetPath = "Assets/Resources/Live/RuntimeAssetRegistry.asset";
        private const string ManifestPath      = "Assets/Resources/Live/registry_manifest.txt";

        /// <summary>
        /// Canonical scan roots (spec §F2). Add new asset folders here as the project grows.
        /// Key = folder path under Assets/. Value = whether to recurse sub-folders.
        /// </summary>
        private static readonly (string Folder, bool Recursive)[] ScanRoots =
        {
            ("Assets/Sprites/Environment/PixelLab_Selected_Assets", true),
            ("Assets/Sprites/Environment/KitB_Cliff",               true),
            ("Assets/Prefabs/Props/ShatteredKeep_PixelLab",         true),
            ("Assets/Prefabs/Environment/Walls/AssetPackV3",        true),
            ("Assets/Prefabs/Obstacles",                            true),
            ("Assets/ScriptableObjects/Environment",                true),
        };

        // ── Menu entry ─────────────────────────────────────────────────────────

        [MenuItem("RIMA/Live Tool/Bake Asset Registry")]
        public static void BakeFromMenu()
        {
            RuntimeAssetRegistry registry = Bake();
            int count = registry != null ? registry.Count : 0;
            Debug.Log($"[RuntimeAssetRegistryBaker] Bake complete — {count} entries. Asset: {RegistryAssetPath}");
            EditorUtility.DisplayDialog(
                "Registry Baked",
                $"RuntimeAssetRegistry baked successfully.\n{count} entries registered.\nAsset: {RegistryAssetPath}",
                "OK");
        }

        // ── Core bake ─────────────────────────────────────────────────────────

        /// <summary>
        /// Scan all roots, build entries, write/update the ScriptableObject and manifest.
        /// Returns the resulting registry (never null — returns existing on partial failure).
        /// Safe to call from code (e.g. BuildPlayerProcessor pre-build hook).
        /// </summary>
        public static RuntimeAssetRegistry Bake()
        {
            List<RegistryEntry> entries = ScanAllRoots();

            // Load or create the SO
            RuntimeAssetRegistry registry = AssetDatabase.LoadAssetAtPath<RuntimeAssetRegistry>(RegistryAssetPath);
            if (registry == null)
            {
                EnsureDirectory(RegistryAssetPath);
                registry = ScriptableObject.CreateInstance<RuntimeAssetRegistry>();
                AssetDatabase.CreateAsset(registry, RegistryAssetPath);
            }

            // Diff manifest: capture old GUIDs before overwrite
            HashSet<string> oldGuids = new HashSet<string>();
            foreach (RegistryEntry e in registry.Entries) oldGuids.Add(e.guid);

            // Write new entries
            registry.SetEntries(entries);
            EditorUtility.SetDirty(registry);
            AssetDatabase.SaveAssets();

            // Write manifest
            WriteManifest(entries, oldGuids);

            AssetDatabase.Refresh();
            return registry;
        }

        // ── Scan logic ─────────────────────────────────────────────────────────

        private static List<RegistryEntry> ScanAllRoots()
        {
            List<RegistryEntry> entries = new List<RegistryEntry>();
            HashSet<string> seenGuids = new HashSet<string>();

            foreach ((string folder, bool recursive) in ScanRoots)
            {
                if (!AssetDatabase.IsValidFolder(folder))
                {
                    Debug.LogWarning($"[RuntimeAssetRegistryBaker] Scan root not found (skipped): {folder}");
                    continue;
                }

                string searchOption = recursive ? "Assets" : folder;
                string[] guids = AssetDatabase.FindAssets(
                    "t:Sprite t:TileBase t:GameObject",
                    new[] { folder });

                foreach (string guid in guids)
                {
                    if (seenGuids.Contains(guid)) continue;
                    seenGuids.Add(guid);

                    RegistryEntry entry = BuildEntry(guid);
                    if (entry != null) entries.Add(entry);
                }
            }

            return entries;
        }

        private static RegistryEntry BuildEntry(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath)) return null;

            // Determine asset kind
            Sprite sprite    = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            TileBase tile    = AssetDatabase.LoadAssetAtPath<TileBase>(assetPath);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            // Skip if none of the supported types loaded (e.g. texture without sprite import)
            if (sprite == null && tile == null && prefab == null) return null;

            AssetKind kind;
            if (tile != null && sprite != null)       kind = AssetKind.TileAndSprite;
            else if (tile != null)                     kind = AssetKind.Tile;
            else if (sprite != null)                   kind = AssetKind.Sprite;
            else                                       kind = AssetKind.Prefab;

            // Resolve physics/layer via PhysicsRules keyword matching
            PhysicsConfig cfg = RoomPainterPhysicsRules.Resolve(assetPath);

            string displayName = Path.GetFileNameWithoutExtension(assetPath);
            string tag         = ResolveTag(assetPath);

            return new RegistryEntry
            {
                guid        = guid,
                displayName = displayName,
                tag         = tag,
                layer       = cfg.layer,
                sprite      = sprite,
                tile        = tile,
                prefab      = prefab,
                kind        = kind,
            };
        }

        // ── Tag resolution ─────────────────────────────────────────────────────

        /// <summary>
        /// Derive a palette tag from the asset filename.
        /// Mirrors RoomPainterPhysicsRules keyword priority order so both systems agree.
        /// </summary>
        private static readonly string[] TagKeywords =
        {
            "mounting", "vine", "chain",
            "rune_circle", "bone_cluster",
            "statue", "pedestal", "plinth",
            "wall", "cliff", "pillar", "column", "door",
            "altar", "brazier", "banner",
            "prop", "ritual", "enemy", "npc",
            "pickup", "item", "coin", "chest",
            "floor", "decal", "moss", "crack",
            "parallax", "bg", "rift", "sky",
            "torch", "lamp", "light", "glow", "flame", "ember",
            "trigger", "zone", "tile", "wang16",
            "dirt", "sand", "stone", "cobble",
        };

        private static string ResolveTag(string assetPath)
        {
            string filename = Path.GetFileNameWithoutExtension(assetPath)?.ToLowerInvariant() ?? string.Empty;
            foreach (string kw in TagKeywords)
                if (filename.IndexOf(kw, System.StringComparison.Ordinal) >= 0)
                    return kw;
            return "misc";
        }

        // ── Manifest writer ────────────────────────────────────────────────────

        private static void WriteManifest(List<RegistryEntry> newEntries, HashSet<string> oldGuids)
        {
            HashSet<string> newGuids = new HashSet<string>();
            foreach (RegistryEntry e in newEntries) newGuids.Add(e.guid);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"# RuntimeAssetRegistry manifest — {System.DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            sb.AppendLine($"# Total: {newEntries.Count} entries");
            sb.AppendLine();

            // Added since last bake
            int addedCount = 0;
            foreach (RegistryEntry e in newEntries)
                if (!oldGuids.Contains(e.guid)) { sb.AppendLine($"+  {e.guid}  {e.displayName}  [{e.kind}] [{e.layer}]"); addedCount++; }

            // Dropped since last bake
            int droppedCount = 0;
            foreach (string old in oldGuids)
                if (!newGuids.Contains(old)) { sb.AppendLine($"-  {old}  (removed)"); droppedCount++; }

            // Stable entries
            foreach (RegistryEntry e in newEntries)
                if (oldGuids.Contains(e.guid)) sb.AppendLine($"   {e.guid}  {e.displayName}  [{e.kind}] [{e.layer}] [{e.tag}]");

            sb.AppendLine();
            sb.AppendLine($"# Added: {addedCount}  Dropped: {droppedCount}");

            EnsureDirectory(ManifestPath);
            File.WriteAllText(ManifestPath, sb.ToString(), Encoding.UTF8);
            AssetDatabase.ImportAsset(ManifestPath);
        }

        // ── Utilities ──────────────────────────────────────────────────────────

        private static void EnsureDirectory(string assetPath)
        {
            string dir = Path.GetDirectoryName(assetPath)?.Replace('\\', '/');
            if (string.IsNullOrEmpty(dir) || AssetDatabase.IsValidFolder(dir)) return;

            // Walk parent chain, creating any missing folders
            string[] parts = dir.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
