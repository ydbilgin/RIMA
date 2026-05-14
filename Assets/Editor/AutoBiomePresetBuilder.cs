using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RIMA;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public static class AutoBiomePresetBuilder
    {
        private const string DefaultTilesetRoot = "Assets/Art/Tiles/F1/Tilesets";
        private const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";

        [MenuItem("RIMA/Tools/Auto-Build BiomePreset from Tilesets")]
        public static void BuildFromTilesets()
        {
            string root = EditorUtility.OpenFolderPanel(
                "Select tileset root",
                ToAbsolutePath(DefaultTilesetRoot),
                string.Empty);

            if (string.IsNullOrEmpty(root))
            {
                return;
            }

            string[] metaPaths = Directory.GetFiles(root, "tileset_meta.json", SearchOption.AllDirectories);
            if (metaPaths.Length == 0)
            {
                Debug.LogWarning("[RIMA] Auto-Build BiomePreset found no tileset_meta.json files under: " + root);
                return;
            }

            List<TilesetMeta> metas = metaPaths
                .Select(LoadMeta)
                .Where(meta => meta != null)
                .OrderBy(meta => meta.name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            Dictionary<string, int> terrainIds = AssignTerrainIds(metas);
            string assetPath = EditorUtility.OpenFilePanelWithFilters(
                "Select RimaBiomePreset asset",
                Application.dataPath,
                new[] { "Biome Preset", "asset" });

            if (string.IsNullOrEmpty(assetPath))
            {
                return;
            }

            assetPath = ToAssetPath(assetPath);
            RimaBiomePreset preset = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>(assetPath);
            if (preset == null)
            {
                Debug.LogError("[RIMA] Selected asset is not a RimaBiomePreset: " + assetPath);
                return;
            }

            preset.terrains = BuildTerrains(terrainIds, metas);
            preset.tilesetPairings = BuildPairings(terrainIds, metas);

            int nullBaseTiles = preset.terrains.Count(terrain => terrain == null || terrain.baseTile == null);
            EditorUtility.SetDirty(preset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[RIMA] Auto-Built BiomePreset '{preset.name}': {preset.terrains.Count} terrain(s), {preset.tilesetPairings.Count} pairing(s), {nullBaseTiles} null baseTile(s).");
        }

        private static List<MapTerrain> BuildTerrains(Dictionary<string, int> terrainIds, List<TilesetMeta> metas)
        {
            return terrainIds
                .OrderBy(pair => pair.Value)
                .Select(pair =>
                {
                    BaseTilePick baseTile = FindBaseTile(pair.Key, metas);
                    int id = pair.Value;
                    return new MapTerrain
                    {
                        id = id,
                        name = ShortTerrainName(pair.Key, id),
                        paletteColor = Color.HSVToRGB((id * 0.137f) % 1f, 0.55f, 0.75f),
                        baseTile = baseTile.tile,
                        baseTileSource = baseTile.tileSet
                    };
                })
                .ToList();
        }

        private static List<TilesetPairing> BuildPairings(Dictionary<string, int> terrainIds, List<TilesetMeta> metas)
        {
            var pairings = new List<TilesetPairing>();
            foreach (TilesetMeta meta in metas)
            {
                if (!terrainIds.TryGetValue(meta.lower, out int lowerId) ||
                    !terrainIds.TryGetValue(meta.upper, out int upperId))
                {
                    Debug.LogWarning("[RIMA] Skipping tileset meta with unmapped terrain ids: " + meta.name);
                    continue;
                }

                pairings.Add(new TilesetPairing
                {
                    lowerTerrainId = lowerId,
                    upperTerrainId = upperId,
                    tileSet = LoadTileSet(meta),
                    transitionSize = IsFlatGroundPair(meta.lower, meta.upper) ? 0f : 0.25f,
                    transitionDescription = meta.TransitionDescription
                });
            }

            return pairings;
        }

        private static BaseTilePick FindBaseTile(string terrainName, List<TilesetMeta> metas)
        {
            foreach (TilesetMeta meta in metas)
            {
                if (!EqualsTerrain(meta.upper, terrainName))
                {
                    continue;
                }

                CornerWangTileSetSO tileSet = LoadTileSet(meta);
                return new BaseTilePick(tileSet, GetTile(tileSet, 15));
            }

            foreach (TilesetMeta meta in metas)
            {
                if (!EqualsTerrain(meta.lower, terrainName))
                {
                    continue;
                }

                CornerWangTileSetSO tileSet = LoadTileSet(meta);
                return new BaseTilePick(tileSet, GetTile(tileSet, 0));
            }

            return new BaseTilePick(null, null);
        }

        private static TileBase GetTile(CornerWangTileSetSO tileSet, int index)
        {
            if (tileSet == null || tileSet.tiles == null || index < 0 || index >= tileSet.tiles.Length)
            {
                return null;
            }

            return tileSet.tiles[index];
        }

        private static CornerWangTileSetSO LoadTileSet(TilesetMeta meta)
        {
            string path = $"{GeneratedFolder}/{ToPascalCase(meta.name)}_CornerWangTileSet.asset";
            CornerWangTileSetSO tileSet = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(path);
            if (tileSet == null)
            {
                Debug.LogWarning("[RIMA] Missing generated CornerWangTileSetSO: " + path);
            }

            return tileSet;
        }

        private static Dictionary<string, int> AssignTerrainIds(List<TilesetMeta> metas)
        {
            List<string> names = metas
                .SelectMany(meta => new[] { meta.lower, meta.upper })
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            string floorName = names.FirstOrDefault(IsFloorLike) ?? names.FirstOrDefault();
            var ids = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(floorName))
            {
                return ids;
            }

            ids[floorName] = 0;
            int nextId = 1;
            foreach (string name in names)
            {
                if (EqualsTerrain(name, floorName))
                {
                    continue;
                }

                ids[name] = nextId++;
            }

            return ids;
        }

        private static TilesetMeta LoadMeta(string metaPath)
        {
            try
            {
                TilesetMeta meta = JsonUtility.FromJson<TilesetMeta>(File.ReadAllText(metaPath));
                if (meta == null || string.IsNullOrEmpty(meta.name))
                {
                    Debug.LogWarning("[RIMA] Invalid tileset meta skipped: " + metaPath);
                    return null;
                }

                return meta;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[RIMA] Could not read tileset meta '{metaPath}': {ex.Message}");
                return null;
            }
        }

        private static string ShortTerrainName(string terrainName, int id)
        {
            if (!string.IsNullOrWhiteSpace(terrainName) && terrainName.Length <= 24)
            {
                return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(terrainName);
            }

            string lower = terrainName == null ? string.Empty : terrainName.ToLowerInvariant();
            string[] keywords = { "wall", "path", "rift", "moss", "cream", "floor", "rubble", "debris", "slate", "mineral" };
            string keyword = keywords.FirstOrDefault(lower.Contains);
            return string.IsNullOrEmpty(keyword)
                ? "Terrain " + id
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(keyword);
        }

        private static bool IsFlatGroundPair(string lower, string upper)
        {
            return IsFloorLike(lower) && ContainsAny(upper, "floor", "rubble", "moss", "cream", "path");
        }

        private static bool IsFloorLike(string value)
        {
            return ContainsAny(value, "floor", "rubble");
        }

        private static bool ContainsAny(string value, params string[] needles)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            string lower = value.ToLowerInvariant();
            return needles.Any(lower.Contains);
        }

        private static bool EqualsTerrain(string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }

        private static string ToPascalCase(string value)
        {
            return string.Concat(value.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
        }

        private static string ToAbsolutePath(string assetPath)
        {
            string projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            return string.IsNullOrEmpty(projectRoot) ? assetPath : Path.Combine(projectRoot, assetPath);
        }

        private static string ToAssetPath(string absolutePath)
        {
            string dataPath = Application.dataPath.Replace('\\', '/');
            absolutePath = absolutePath.Replace('\\', '/');
            if (!absolutePath.StartsWith(dataPath, StringComparison.OrdinalIgnoreCase))
            {
                return absolutePath;
            }

            return "Assets" + absolutePath.Substring(dataPath.Length);
        }

        private readonly struct BaseTilePick
        {
            public readonly CornerWangTileSetSO tileSet;
            public readonly TileBase tile;

            public BaseTilePick(CornerWangTileSetSO tileSet, TileBase tile)
            {
                this.tileSet = tileSet;
                this.tile = tile;
            }
        }

#pragma warning disable 0649
        [Serializable]
        private class TilesetMeta
        {
            public string name;
            public string lower;
            public string upper;
            public string transitionDescription;
            public string transition_description;

            public string TransitionDescription
            {
                get
                {
                    return !string.IsNullOrEmpty(transitionDescription)
                        ? transitionDescription
                        : (transition_description ?? string.Empty);
                }
            }
        }
#pragma warning restore 0649
    }
}
