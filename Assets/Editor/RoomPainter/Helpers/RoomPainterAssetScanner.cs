using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    [Serializable]
    public struct AssetEntry
    {
        public string path;
        public Sprite sprite;
        public GameObject prefab;
        public RoomPainterAsset metadata;
        public RoomLayer suggestedLayer;
        public string category;

        public UnityEngine.Object AssetObject
        {
            get { return sprite != null ? sprite : prefab; }
        }
    }

    public static class RoomPainterAssetScanner
    {
        public static List<AssetEntry> Scan(string folderPath, string[] categoryFilters = null)
        {
            List<AssetEntry> entries = new List<AssetEntry>();

            if (string.IsNullOrEmpty(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
            {
                return entries;
            }

            string[] searchFolders = { folderPath };
            string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", searchFolders);
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", searchFolders);

            for (int i = 0; i < spriteGuids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(spriteGuids[i]);
                string category = GetParentFolderName(path);
                if (!MatchesCategoryFilter(category, categoryFilters))
                {
                    continue;
                }

                Sprite sprite = LoadSprite(path);
                if (sprite == null)
                {
                    continue;
                }

                entries.Add(new AssetEntry
                {
                    path = path,
                    sprite = sprite,
                    prefab = null,
                    metadata = RoomPainterAssetPostprocessor.LoadMetadataForAssetPath(path),
                    suggestedLayer = InferLayer(path),
                    category = category
                });
            }

            for (int i = 0; i < prefabGuids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabGuids[i]);
                string category = GetParentFolderName(path);
                if (!MatchesCategoryFilter(category, categoryFilters))
                {
                    continue;
                }

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    continue;
                }

                entries.Add(new AssetEntry
                {
                    path = path,
                    sprite = null,
                    prefab = prefab,
                    metadata = RoomPainterAssetPostprocessor.LoadMetadataForAssetPath(path),
                    suggestedLayer = InferLayer(path),
                    category = category
                });
            }

            entries.Sort((left, right) => string.Compare(left.path, right.path, StringComparison.OrdinalIgnoreCase));
            return entries;
        }

        private static bool MatchesCategoryFilter(string category, string[] categoryFilters)
        {
            if (categoryFilters == null || categoryFilters.Length == 0)
            {
                return true;
            }

            for (int i = 0; i < categoryFilters.Length; i++)
            {
                if (string.Equals(category, categoryFilters[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static Sprite LoadSprite(string path)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                return sprite;
            }

            UnityEngine.Object[] representations = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
            for (int i = 0; i < representations.Length; i++)
            {
                if (representations[i] is Sprite spriteRepresentation)
                {
                    return spriteRepresentation;
                }
            }

            return null;
        }

        // S110 — Sonnet Day 2 review HIGH #1 fix (2026-05-26): keyword set
        // genişletildi. Önce daha spesifik (Cliff/Wall/Lighting) sonra geniş
        // (Floor) sırasıyla evaluate — ilk match kazanır.
        private static readonly (string Keyword, RoomLayer Layer)[] _layerKeywords = new[]
        {
            // Cliff variants
            ("cliff", RoomLayer.Cliff),

            // Wall variants
            ("wall", RoomLayer.Wall),
            ("edge", RoomLayer.Edge),

            // Decals (organic surface details)
            ("moss", RoomLayer.Decals),
            ("pebbles", RoomLayer.Decals),
            ("cracks_bones", RoomLayer.Decals),
            ("decals", RoomLayer.Decals),
            ("decal", RoomLayer.Decals),
            ("crack", RoomLayer.Decals),

            // Props (interactive/decorative objects)
            ("prop", RoomLayer.Props),
            ("ritual", RoomLayer.Props),
            ("altar", RoomLayer.Props),
            ("pillar", RoomLayer.Props),
            ("brazier", RoomLayer.Props),
            ("banner", RoomLayer.Props),

            // Lighting (sources or baked emissive)
            ("torch", RoomLayer.Lighting),
            ("lamp", RoomLayer.Lighting),
            ("glow", RoomLayer.Lighting),
            ("light", RoomLayer.Lighting),
            ("flame", RoomLayer.Lighting),
            ("ember", RoomLayer.Lighting),

            // Parallax / background (incl. rift / cyan rune art)
            ("parallax", RoomLayer.Parallax),
            ("bg", RoomLayer.Parallax),
            ("rift", RoomLayer.Parallax),
            ("sky", RoomLayer.Parallax),
            ("horizon", RoomLayer.Parallax),
            ("background", RoomLayer.Parallax),

            // Floor variants (genişletilmiş — wang16, macro, dirt, sand vb.)
            ("floor", RoomLayer.Floor),
            ("dirt", RoomLayer.Floor),
            ("sand", RoomLayer.Floor),
            ("stone", RoomLayer.Floor),
            ("tile", RoomLayer.Floor),
            ("wang16", RoomLayer.Floor),
            ("wang", RoomLayer.Floor),
            ("macro", RoomLayer.Floor),
            ("rune", RoomLayer.Floor),
            ("cobble", RoomLayer.Floor),

            // Collision / Occlusion (rare in sprite paths but keep)
            ("collision", RoomLayer.Collision),
            ("collider", RoomLayer.Collision),
            ("occlusion", RoomLayer.Occlusion),
            ("fade", RoomLayer.Occlusion),
        };

        public static RoomLayer InferLayer(string assetPath)
        {
            string lowerPath = assetPath.ToLowerInvariant();

            for (int i = 0; i < _layerKeywords.Length; i++)
            {
                if (lowerPath.Contains(_layerKeywords[i].Keyword))
                {
                    return _layerKeywords[i].Layer;
                }
            }

            return RoomLayer.Floor;
        }

        private static string GetParentFolderName(string assetPath)
        {
            string normalizedPath = assetPath.Replace('\\', '/');
            int fileSlashIndex = normalizedPath.LastIndexOf('/');
            if (fileSlashIndex < 0)
            {
                return string.Empty;
            }

            string folderPath = normalizedPath.Substring(0, fileSlashIndex);
            int parentSlashIndex = folderPath.LastIndexOf('/');
            return parentSlashIndex >= 0 ? folderPath.Substring(parentSlashIndex + 1) : folderPath;
        }
    }
}
