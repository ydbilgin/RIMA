#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor
{
    public static class AssetPackBrowserAdjacency
    {
        public const string FloorDominant = "Floor Dominant";
        public const string Path = "Path";
        public const string Transition = "Transition";
        public const string Dirt = "Dirt";
        public const string Secondary = "Secondary";
        public const double HoverPopupDelaySeconds = 0.5d;

        private static readonly string[] RoleFallbackCategories =
        {
            "BaseFloor",
            "Macro",
            "OrganicDecal",
            "DetailScatter",
            "Accent",
            "Unknown"
        };

        public static AssetPackEntry[] BuildAdjacencyMatrix(AssetPackEntry center)
        {
            return BuildAdjacencyMatrix(center, null);
        }

        public static AssetPackEntry[] BuildAdjacencyMatrix(AssetPackEntry center, IReadOnlyList<AssetPackEntry> candidates)
        {
            var result = new AssetPackEntry[8];
            if (center == null)
            {
                return result;
            }

            var matches = new List<AssetPackEntry>();
            string centerGroup = GetSemanticCategory(center);
            if (candidates != null)
            {
                for (int i = 0; i < candidates.Count; i++)
                {
                    AssetPackEntry candidate = candidates[i];
                    if (candidate == null || candidate.sprite == null || candidate.isPreviewOnly)
                    {
                        continue;
                    }

                    if (string.Equals(candidate.entryId, center.entryId, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (string.Equals(GetSemanticCategory(candidate), centerGroup, StringComparison.OrdinalIgnoreCase))
                    {
                        matches.Add(candidate);
                    }
                }
            }

            matches.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = matches.Count > 0 ? matches[i % matches.Count] : center;
            }

            return result;
        }

        public static void RenderAdjacency3x3(Rect rect, Sprite center, Sprite[] neighbors)
        {
            float cell = Mathf.Floor(Mathf.Min(rect.width, rect.height) / 3f);
            Rect gridRect = new Rect(rect.x, rect.y, cell * 3f, cell * 3f);
            int neighborIndex = 0;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Rect cellRect = new Rect(gridRect.x + col * cell, gridRect.y + row * cell, cell, cell);
                    DrawChecker(cellRect);
                    Sprite sprite = row == 1 && col == 1 ? center : GetSprite(neighbors, neighborIndex++);
                    if (sprite != null)
                    {
                        DrawSprite(cellRect, sprite, ScaleMode.ScaleToFit);
                    }

                    Handles.color = new Color(0f, 0f, 0f, 0.45f);
                    DrawGridLines(cellRect);
                }
            }
        }

        public static void RenderWang2x2(Rect rect, Sprite[] corners)
        {
            float cell = Mathf.Floor(Mathf.Min(rect.width, rect.height) / 2f);
            for (int i = 0; i < 4; i++)
            {
                int row = i / 2;
                int col = i % 2;
                Rect cellRect = new Rect(rect.x + col * cell, rect.y + row * cell, cell, cell);
                DrawChecker(cellRect);
                Sprite sprite = GetSprite(corners, i);
                if (sprite != null)
                {
                    DrawSprite(cellRect, sprite, ScaleMode.ScaleToFit);
                }

                Handles.color = new Color(0f, 0f, 0f, 0.45f);
                DrawGridLines(cellRect);
            }
        }

        public static string ResolveCategory(Sprite sprite, PatchAtlasSO atlas, string manifestCategory)
        {
            if (!IsRoleFallbackCategory(manifestCategory))
            {
                return string.IsNullOrEmpty(manifestCategory) ? GetSemanticCategory(sprite != null ? sprite.name : string.Empty) : manifestCategory;
            }

            string semantic = GetSemanticCategory(
                sprite != null ? sprite.name : string.Empty,
                atlas != null ? atlas.atlasId : string.Empty,
                atlas != null ? atlas.name : string.Empty);
            return string.IsNullOrEmpty(semantic) ? manifestCategory : semantic;
        }

        public static string GetSemanticCategory(AssetPackEntry entry)
        {
            if (entry == null)
            {
                return string.Empty;
            }

            string semantic = GetSemanticCategory(entry.displayName, entry.sourceAtlas, entry.categoryName);
            return string.IsNullOrEmpty(semantic) ? entry.categoryName : semantic;
        }

        public static string GetSemanticCategory(params string[] namesOrReferences)
        {
            string key = NormalizeKey(namesOrReferences);
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            if (ContainsAny(key, "combat_floor_dominant", "floor_dominant", "dominant_floor"))
            {
                return FloorDominant;
            }

            if (ContainsAny(key, "combat_path", "_path_", "path_tile", "floor_path"))
            {
                return Path;
            }

            if (ContainsAny(key, "transition", "wang", "blend", "edge", "corner"))
            {
                return Transition;
            }

            if (ContainsAny(key, "dirt", "mud", "soil", "dust"))
            {
                return Dirt;
            }

            if (ContainsAny(key, "secondary", "alternate", "_alt_", "variant_b"))
            {
                return Secondary;
            }

            return string.Empty;
        }

        public static bool ShouldShowHoverPopup(double hoverStartedAt, double now)
        {
            return ShouldShowHoverPopup(hoverStartedAt, now, HoverPopupDelaySeconds);
        }

        public static bool ShouldShowHoverPopup(double hoverStartedAt, double now, double delaySeconds)
        {
            return hoverStartedAt > 0d && now - hoverStartedAt >= delaySeconds;
        }

        private static bool IsRoleFallbackCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return true;
            }

            for (int i = 0; i < RoleFallbackCategories.Length; i++)
            {
                if (string.Equals(categoryName, RoleFallbackCategories[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string NormalizeKey(params string[] values)
        {
            if (values == null || values.Length == 0)
            {
                return string.Empty;
            }

            string joined = string.Join("_", values);
            return joined.Replace('-', '_').Replace(' ', '_').ToLowerInvariant();
        }

        private static bool ContainsAny(string value, params string[] needles)
        {
            for (int i = 0; i < needles.Length; i++)
            {
                if (value.IndexOf(needles[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static Sprite GetSprite(Sprite[] sprites, int index)
        {
            return sprites != null && index >= 0 && index < sprites.Length ? sprites[index] : null;
        }

        private static void DrawGridLines(Rect rect)
        {
            Handles.color = new Color(0f, 0f, 0f, 0.45f);
            Handles.DrawAAPolyLine(1f, new Vector3(rect.x, rect.y, 0f), new Vector3(rect.xMax, rect.y, 0f));
            Handles.DrawAAPolyLine(1f, new Vector3(rect.x, rect.yMax, 0f), new Vector3(rect.xMax, rect.yMax, 0f));
            Handles.DrawAAPolyLine(1f, new Vector3(rect.x, rect.y, 0f), new Vector3(rect.x, rect.yMax, 0f));
            Handles.DrawAAPolyLine(1f, new Vector3(rect.xMax, rect.y, 0f), new Vector3(rect.xMax, rect.yMax, 0f));
        }

        private static void DrawChecker(Rect rect)
        {
            const float tile = 16f;
            for (float y = rect.y; y < rect.yMax; y += tile)
            {
                for (float x = rect.x; x < rect.xMax; x += tile)
                {
                    bool dark = ((int)((x - rect.x) / tile) + (int)((y - rect.y) / tile)) % 2 == 0;
                    EditorGUI.DrawRect(new Rect(x, y, Mathf.Min(tile, rect.xMax - x), Mathf.Min(tile, rect.yMax - y)),
                        dark ? new Color(0.22f, 0.22f, 0.22f, 1f) : new Color(0.30f, 0.30f, 0.30f, 1f));
                }
            }
        }

        private static void DrawSprite(Rect rect, Sprite sprite, ScaleMode scaleMode)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(sprite);
            if (texture == null)
            {
                texture = AssetPreview.GetMiniThumbnail(sprite) as Texture2D;
            }

            if (texture != null)
            {
                GUI.DrawTexture(rect, texture, scaleMode, true);
            }
        }
    }
}
#endif
