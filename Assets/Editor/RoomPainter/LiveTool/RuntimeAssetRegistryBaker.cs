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
            ("Assets/Sprites/Environment/PixelLabFloor451",          true),
            ("Assets/Sprites/Environment/IsoKit/decor",              true),
            ("Assets/Sprites/Environment/PixelLabKit",               true),
            ("Assets/Prefabs/Props/ShatteredKeep_PixelLab",         true),
            ("Assets/Prefabs/Environment/Walls/AssetPackV3",        true),
            ("Assets/Prefabs/Obstacles",                            true),
            ("Assets/ScriptableObjects/Environment",                true),
        };

        // ── Menu entry ─────────────────────────────────────────────────────────

        [MenuItem("RIMA/Legacy/Live Tool/Bake Asset Registry")]
        public static void BakeFromMenu()
        {
            RuntimeAssetRegistry registry = Bake();
            int count = registry != null ? registry.Count : 0;
            // Non-blocking feedback only. A modal EditorUtility.DisplayDialog here blocks Unity's
            // main thread, which (a) trips MCP execute_menu_item timeouts and (b) can spawn a dialog
            // "storm" when the bridge re-queues the timed-out command. Console log is sufficient.
            Debug.Log($"[RuntimeAssetRegistryBaker] Bake complete — {count} entries registered. Asset: {RegistryAssetPath}");
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

            AddAssetPackEntries(entries, seenGuids);
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
            RoomLayer layer    = ResolveLayer(tag, cfg.layer);

            return new RegistryEntry
            {
                guid        = guid,
                displayName = displayName,
                tag         = tag,
                layer       = layer,
                sprite      = sprite,
                tile        = tile,
                prefab      = prefab,
                kind        = kind,
            };
        }

        private static void AddAssetPackEntries(List<RegistryEntry> entries, HashSet<string> seenGuids)
        {
            string[] packGuids = AssetDatabase.FindAssets("t:AssetPackSO");
            foreach (string packGuid in packGuids)
            {
                string packPath = AssetDatabase.GUIDToAssetPath(packGuid);
                AssetPackSO pack = AssetDatabase.LoadAssetAtPath<AssetPackSO>(packPath);
                if (pack == null || pack.entries == null) continue;

                foreach (AssetPackSO.Entry packEntry in pack.entries)
                {
                    RegistryEntry entry = BuildEntry(packEntry);
                    if (entry == null || string.IsNullOrEmpty(entry.guid) || seenGuids.Contains(entry.guid)) continue;

                    seenGuids.Add(entry.guid);
                    entries.Add(entry);
                }
            }
        }

        private static RegistryEntry BuildEntry(AssetPackSO.Entry packEntry)
        {
            if (packEntry == null) return null;

            string guid = ResolvePackGuid(packEntry);
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath)) return null;

            Sprite sprite = packEntry.sprite != null ? packEntry.sprite : AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            TileBase tile = packEntry.tile != null ? packEntry.tile : AssetDatabase.LoadAssetAtPath<TileBase>(assetPath);
            GameObject prefab = packEntry.prefab != null ? packEntry.prefab : AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (sprite == null && tile == null && prefab == null) return null;

            AssetKind kind;
            if (tile != null && sprite != null) kind = AssetKind.TileAndSprite;
            else if (tile != null) kind = AssetKind.Tile;
            else if (sprite != null) kind = AssetKind.Sprite;
            else kind = AssetKind.Prefab;

            string tag = !string.IsNullOrEmpty(packEntry.registryTag)
                ? packEntry.registryTag
                : DesignerCategoryMap.RegistryTag(packEntry.category);

            RoomLayer fallbackLayer = DesignerCategoryMap.LayerFor(packEntry.category);
            RoomLayer layer = ResolvePackLayer(packEntry.layer, tag, fallbackLayer);

            return new RegistryEntry
            {
                guid = guid,
                displayName = Path.GetFileNameWithoutExtension(assetPath),
                tag = tag,
                layer = layer,
                sprite = sprite,
                tile = tile,
                prefab = prefab,
                kind = kind,
            };
        }

        private static string ResolvePackGuid(AssetPackSO.Entry packEntry)
        {
            if (!string.IsNullOrEmpty(packEntry.guid)) return packEntry.guid;
            Object asset = packEntry.prefab != null
                ? (Object)packEntry.prefab
                : packEntry.tile != null
                    ? packEntry.tile
                    : packEntry.sprite;

            if (asset == null) return string.Empty;
            string path = AssetDatabase.GetAssetPath(asset);
            return string.IsNullOrEmpty(path) ? string.Empty : AssetDatabase.AssetPathToGUID(path);
        }

        // ── Tag resolution ─────────────────────────────────────────────────────

        /// <summary>
        /// Derive a palette tag from the asset filename.
        /// Designer palette tags win first; legacy tags remain as fallback for existing tools.
        /// </summary>
        private static string ResolveTag(string assetPath)
        {
            string filename = Path.GetFileNameWithoutExtension(assetPath)?.ToLowerInvariant() ?? string.Empty;
            if (HasAny(filename, "flat_", "floor", "pl_floor")) return "floor";
            // glow/rim/fade overlays live in the cliff kit folder but are decals, not cliff faces (ax review)
            if (HasAny(filename, "glow", "_rim", "edge_ao", "corner_fade")) return "decal";
            if (HasAny(filename, "cliff")) return "cliff";
            if (HasAny(filename, "portal", "gate")) return "portal";
            if (HasAny(filename, "brazier_lit", "light", "lamp", "torch")) return "light";
            if (HasAny(
                    filename,
                    "decor_", "pillar", "brazier", "rubble", "banner", "sarcophagus",
                    "bones", "moss", "rune", "seal", "slab", "rift", "debris",
                    "blocks", "bricks"))
                return "prop";
            if (HasAny(filename, "decal", "crack")) return "decal";
            if (HasAny(filename, "mounting", "vine", "chain")) return FirstMatch(filename, "mounting", "vine", "chain");
            if (HasAny(filename, "rune_circle", "bone_cluster")) return FirstMatch(filename, "rune_circle", "bone_cluster");
            if (HasAny(filename, "statue", "pedestal", "plinth")) return FirstMatch(filename, "statue", "pedestal", "plinth");
            if (HasAny(filename, "wall", "column", "door")) return FirstMatch(filename, "wall", "column", "door");
            if (HasAny(filename, "altar")) return "altar";
            if (HasAny(filename, "ritual", "enemy", "npc")) return FirstMatch(filename, "ritual", "enemy", "npc");
            if (HasAny(filename, "pickup", "item", "coin", "chest")) return FirstMatch(filename, "pickup", "item", "coin", "chest");
            if (HasAny(filename, "parallax", "bg", "sky")) return FirstMatch(filename, "parallax", "bg", "sky");
            if (HasAny(filename, "glow", "flame", "ember")) return FirstMatch(filename, "glow", "flame", "ember");
            if (HasAny(filename, "trigger", "zone", "tile", "wang16")) return FirstMatch(filename, "trigger", "zone", "tile", "wang16");
            if (HasAny(filename, "dirt", "sand", "stone", "cobble")) return FirstMatch(filename, "dirt", "sand", "stone", "cobble");
            return "misc";
        }

        private static bool HasAny(string filename, params string[] keywords)
        {
            foreach (string keyword in keywords)
                if (filename.IndexOf(keyword, System.StringComparison.Ordinal) >= 0)
                    return true;
            return false;
        }

        private static string FirstMatch(string filename, params string[] keywords)
        {
            foreach (string keyword in keywords)
                if (filename.IndexOf(keyword, System.StringComparison.Ordinal) >= 0)
                    return keyword;
            return "misc";
        }

        private static RoomLayer ResolvePackLayer(string layer, string tag, RoomLayer fallback)
        {
            if (!string.IsNullOrEmpty(layer) && System.Enum.TryParse(layer, true, out RoomLayer parsed))
                return parsed;
            return ResolveLayer(tag, fallback);
        }

        private static RoomLayer ResolveLayer(string tag, RoomLayer fallback)
        {
            switch (tag)
            {
                case "floor":  return RoomLayer.Floor;
                case "cliff":  return RoomLayer.Cliff;
                case "prop":   return RoomLayer.Props;
                case "portal": return RoomLayer.Props;
                case "light":  return RoomLayer.Lighting;
                default:       return fallback;
            }
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
