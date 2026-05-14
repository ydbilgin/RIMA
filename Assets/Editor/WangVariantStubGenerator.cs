using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RIMA;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public static class WangVariantStubGenerator
    {
        private const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";
        private static readonly Regex BaseTileRegex = new Regex(@"^wang_(.+)_tile_(\d+)$", RegexOptions.Compiled);

        [MenuItem("RIMA/Tools/Generate Wang Variant Stubs (rotated)")]
        public static void Generate()
        {
            if (!AssetDatabase.IsValidFolder(GeneratedFolder))
            {
                Debug.LogWarning("[RIMA] Generated folder not found: " + GeneratedFolder);
                return;
            }

            int created = 0;
            var touchedTilesets = new HashSet<string>();
            foreach (string guid in AssetDatabase.FindAssets("t:Tile", new[] { GeneratedFolder }))
            {
                string tilePath = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileNameWithoutExtension(tilePath);
                Match match = BaseTileRegex.Match(fileName);
                if (!match.Success)
                {
                    continue;
                }

                string tilesetName = match.Groups[1].Value;
                int cornerKey = int.Parse(match.Groups[2].Value);
                Tile sourceTile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
                if (sourceTile == null || sourceTile.sprite == null)
                {
                    continue;
                }

                for (int turns = 1; turns <= 3; turns++)
                {
                    CreateRotatedVariant(tilesetName, cornerKey, sourceTile.sprite, turns);
                    created++;
                }

                touchedTilesets.Add(tilesetName);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            foreach (string tilesetName in touchedTilesets)
            {
                PopulateTilesetVariants(tilesetName);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[RIMA] Generated rotated Wang variant stubs: " + created);
        }

        private static void CreateRotatedVariant(string tilesetName, int cornerKey, Sprite sourceSprite, int turns)
        {
            RotatedImage image = RotateSprite(sourceSprite, turns);
            string baseName = $"wang_{tilesetName}_tile_{cornerKey}_v{turns}";
            string pngPath = $"{GeneratedFolder}/{baseName}.png";
            string tilePath = $"{GeneratedFolder}/{baseName}.asset";

            Texture2D texture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
            texture.SetPixels32(image.pixels);
            texture.Apply(false, false);
            File.WriteAllBytes(pngPath, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate);
            ConfigureTextureImporter(pngPath, sourceSprite.pixelsPerUnit);
            AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate);

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);
            AssetDatabase.DeleteAsset(tilePath);
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            AssetDatabase.CreateAsset(tile, tilePath);
        }

        private static void ConfigureTextureImporter(string path, float pixelsPerUnit)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = pixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        private static RotatedImage RotateSprite(Sprite sprite, int quarterTurns)
        {
            Rect rect = sprite.rect;
            int width = Mathf.RoundToInt(rect.width);
            int height = Mathf.RoundToInt(rect.height);
            int startX = Mathf.RoundToInt(rect.x);
            int startY = Mathf.RoundToInt(rect.y);
            Color32[] sourcePixels = ReadTexturePixels(sprite.texture);
            Color32[] cropped = new Color32[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cropped[y * width + x] = sourcePixels[(startY + y) * sprite.texture.width + startX + x];
                }
            }

            return RotatePixels(cropped, width, height, quarterTurns);
        }

        private static RotatedImage RotatePixels(Color32[] source, int width, int height, int quarterTurns)
        {
            int turns = ((quarterTurns % 4) + 4) % 4;
            Color32[] current = source;
            int currentWidth = width;
            int currentHeight = height;

            for (int turn = 0; turn < turns; turn++)
            {
                Color32[] rotated = new Color32[current.Length];
                int nextWidth = currentHeight;
                int nextHeight = currentWidth;
                for (int y = 0; y < currentHeight; y++)
                {
                    for (int x = 0; x < currentWidth; x++)
                    {
                        int targetX = currentHeight - 1 - y;
                        int targetY = x;
                        rotated[targetY * nextWidth + targetX] = current[y * currentWidth + x];
                    }
                }

                current = rotated;
                currentWidth = nextWidth;
                currentHeight = nextHeight;
            }

            return new RotatedImage
            {
                pixels = current,
                width = currentWidth,
                height = currentHeight
            };
        }

        private static Color32[] ReadTexturePixels(Texture2D texture)
        {
            if (texture.isReadable)
            {
                return texture.GetPixels32();
            }

            RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            RenderTexture previous = RenderTexture.active;
            Texture2D readable = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            try
            {
                Graphics.Blit(texture, temporary);
                RenderTexture.active = temporary;
                readable.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0);
                readable.Apply(false, false);
                return readable.GetPixels32();
            }
            finally
            {
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(temporary);
                Object.DestroyImmediate(readable);
            }
        }

        private static void PopulateTilesetVariants(string tilesetName)
        {
            string tilesetPath = $"{GeneratedFolder}/{ToPascalCase(tilesetName)}_CornerWangTileSet.asset";
            CornerWangTileSetSO tileSet = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(tilesetPath);
            if (tileSet == null)
            {
                return;
            }

            tileSet.variantsByKey = new CornerWangTileSetSO.WangVariants[16];
            for (int cornerKey = 0; cornerKey < 16; cornerKey++)
            {
                var variants = new List<TileBase>();
                AddIfPresent(variants, $"{GeneratedFolder}/wang_{tilesetName}_tile_{cornerKey}.asset");
                for (int variantIndex = 1; variantIndex <= 5; variantIndex++)
                {
                    AddIfPresent(variants, $"{GeneratedFolder}/wang_{tilesetName}_tile_{cornerKey}_v{variantIndex}.asset");
                }

                tileSet.variantsByKey[cornerKey] = new CornerWangTileSetSO.WangVariants { variants = variants.ToArray() };
            }

            EditorUtility.SetDirty(tileSet);
        }

        private static void AddIfPresent(List<TileBase> variants, string path)
        {
            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile != null)
            {
                variants.Add(tile);
            }
        }

        private static string ToPascalCase(string value)
        {
            return string.Concat(value.Split(new[] { '_' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
        }

        private struct RotatedImage
        {
            public Color32[] pixels;
            public int width;
            public int height;
        }
    }
}
