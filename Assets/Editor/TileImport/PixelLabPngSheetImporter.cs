namespace RIMA.Editor.TileImport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public static class PixelLabPngSheetImporter
    {
        private const int SheetSize = 256;
        private const int GridSize = 8;
        private const int TileSize = 32;
        private const int PixelsPerUnit = 32;
        private const string OutputRoot = "Assets/Art/Tiles";

        [MenuItem("RIMA/Utilities/PixelLab PNG Sheet Importer")]
        public static void ImportFolder()
        {
            string selected = EditorUtility.OpenFolderPanel("PixelLab PNG Sheet Importer", "STAGING/TILESET_OUTPUT", string.Empty);
            if (string.IsNullOrWhiteSpace(selected))
            {
                return;
            }

            string projectFolder = ToProjectPath(selected);
            if (string.IsNullOrEmpty(projectFolder))
            {
                Debug.LogError("PixelLab import skipped: selected folder must be inside this Unity project.");
                return;
            }

            string[] pngPaths = Directory.GetFiles(projectFolder, "*.png", SearchOption.AllDirectories)
                .Select(NormalizePath)
                .ToArray();

            if (pngPaths.Length == 0)
            {
                Debug.LogWarning($"PixelLab import found no PNG sheets under {projectFolder}.");
                return;
            }

            try
            {
                EditorUtility.DisplayProgressBar("PixelLab PNG Sheet Importer", "Importing sheets", 0f);
                for (int i = 0; i < pngPaths.Length; i++)
                {
                    string path = pngPaths[i];
                    EditorUtility.DisplayProgressBar("PixelLab PNG Sheet Importer", Path.GetFileName(path), i / (float)pngPaths.Length);
                    ImportSheet(path);
                }

                CreateTilemapSkeleton();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static void ImportSheet(string texturePath)
        {
            string sourcePath = NormalizePath(texturePath);
            string baseName = Sanitize(ReadMetadataName(sourcePath) ?? Path.GetFileNameWithoutExtension(sourcePath));
            var parts = DeriveOutputParts(sourcePath);
            string outputFolder = EnsureOutputFolder(parts.biome, parts.type);
            string assetTexturePath = sourcePath.StartsWith("Assets/") ? sourcePath : $"{outputFolder}/{baseName}.png";

            if (!sourcePath.StartsWith("Assets/"))
            {
                File.Copy(sourcePath, assetTexturePath, true);
                AssetDatabase.ImportAsset(assetTexturePath, ImportAssetOptions.ForceUpdate);
            }

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetTexturePath);
            if (texture == null)
            {
                Debug.LogWarning($"PixelLab import skipped {texturePath}: not a Unity texture asset.");
                return;
            }

            if (texture.width != SheetSize || texture.height != SheetSize)
            {
                Debug.LogWarning($"PixelLab import skipped {texturePath}: expected {SheetSize}x{SheetSize}, found {texture.width}x{texture.height}.");
                return;
            }

            ConfigureImporter(assetTexturePath, baseName);

            Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetTexturePath)
                .OfType<Sprite>()
                .OrderByDescending(sprite => sprite.rect.y)
                .ThenBy(sprite => sprite.rect.x)
                .ToArray();

            if (sprites.Length != GridSize * GridSize)
            {
                Debug.LogWarning($"PixelLab import skipped {texturePath}: expected 64 sliced sprites, found {sprites.Length}.");
                return;
            }

            CreateRandomTile(sprites, $"{outputFolder}/random_{baseName}.asset");
            CreateRuleTile(sprites, $"{outputFolder}/rule_{baseName}.asset");
            Debug.Log($"PixelLab imported {texturePath} -> {outputFolder}");
        }

        private static void ConfigureImporter(string texturePath, string baseName)
        {
            var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null)
            {
                throw new IOException($"TextureImporter not found for {texturePath}.");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = PixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.spritesheet = BuildSpriteMetadata(baseName);
            importer.SaveAndReimport();
        }

        private static SpriteMetaData[] BuildSpriteMetadata(string baseName)
        {
            var metadata = new SpriteMetaData[GridSize * GridSize];
            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    int index = row * GridSize + column;
                    metadata[index] = new SpriteMetaData
                    {
                        name = $"{baseName}_{index:00}",
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f),
                        rect = new Rect(column * TileSize, (GridSize - 1 - row) * TileSize, TileSize, TileSize)
                    };
                }
            }

            return metadata;
        }

        private static void CreateRandomTile(Sprite[] sprites, string assetPath)
        {
            var tile = ScriptableObject.CreateInstance<RandomTile>();
            tile.sprites = sprites;
            tile.weights = Enumerable.Repeat(1f, sprites.Length).ToArray();
            tile.colliderType = Tile.ColliderType.None;
            ReplaceAsset(tile, assetPath);
        }

        private static void CreateRuleTile(Sprite[] sprites, string assetPath)
        {
            var tile = ScriptableObject.CreateInstance<RuleTile>();
            tile.m_DefaultSprite = sprites[0];
            tile.m_DefaultColliderType = Tile.ColliderType.Sprite;

            for (int mask = 0; mask < 16; mask++)
            {
                var rule = new RuleTile.TilingRule
                {
                    m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                    m_Sprites = new[] { sprites[Mathf.Clamp(mask, 0, sprites.Length - 1)] },
                    m_ColliderType = Tile.ColliderType.Sprite,
                    m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed
                };

                rule.m_NeighborPositions = new List<Vector3Int>
                {
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, -1, 0)
                };
                rule.m_Neighbors = new List<int>
                {
                    (mask & 1) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis,
                    (mask & 2) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis,
                    (mask & 4) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis,
                    (mask & 8) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis
                };
                tile.m_TilingRules.Add(rule);
            }

            ReplaceAsset(tile, assetPath);
        }

        private static void ReplaceAsset(Object asset, string assetPath)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(asset, assetPath);
        }

        private static void CreateTilemapSkeleton()
        {
            GameObject root = GameObject.Find("PixelLab Tilemap Skeleton");
            if (root == null)
            {
                root = new GameObject("PixelLab Tilemap Skeleton");
                root.AddComponent<Grid>();
                Undo.RegisterCreatedObjectUndo(root, "Create PixelLab Tilemap Skeleton");
            }

            Grid grid = root.GetComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
            grid.cellSize = new Vector3(1f, 0.5f, 0f);

            EnsureTilemap(root.transform, "Base", 0, false);
            EnsureTilemap(root.transform, "Decal", 10, false);
            EnsureTilemap(root.transform, "Wall", 20, true);
            EnsureTilemap(root.transform, "Prop", 30, false);
        }

        private static void EnsureTilemap(Transform parent, string name, int sortingOrder, bool collider)
        {
            Transform existing = parent.Find(name);
            GameObject go = existing != null ? existing.gameObject : new GameObject(name);
            if (existing == null)
            {
                go.transform.SetParent(parent, false);
                Undo.RegisterCreatedObjectUndo(go, $"Create {name} Tilemap");
            }

            if (go.GetComponent<Tilemap>() == null)
            {
                go.AddComponent<Tilemap>();
            }

            TilemapRenderer renderer = go.GetComponent<TilemapRenderer>() ?? go.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;

            TilemapCollider2D currentCollider = go.GetComponent<TilemapCollider2D>();
            if (collider && currentCollider == null)
            {
                go.AddComponent<TilemapCollider2D>();
            }
            else if (!collider && currentCollider != null)
            {
                Object.DestroyImmediate(currentCollider);
            }
        }

        private static string EnsureOutputFolder(string biome, string type)
        {
            EnsureFolder("Assets", "Art");
            EnsureFolder("Assets/Art", "Tiles");
            EnsureFolder(OutputRoot, biome);
            EnsureFolder($"{OutputRoot}/{biome}", type);
            return $"{OutputRoot}/{biome}/{type}";
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static (string biome, string type) DeriveOutputParts(string texturePath)
        {
            string directory = NormalizePath(Path.GetDirectoryName(texturePath));
            string type = Sanitize(Path.GetFileName(directory));
            string biome = Sanitize(Path.GetFileName(Path.GetDirectoryName(directory)));
            return (string.IsNullOrWhiteSpace(biome) ? "common" : biome, string.IsNullOrWhiteSpace(type) ? "tiles" : type);
        }

        private static string ReadMetadataName(string texturePath)
        {
            string directory = Path.GetDirectoryName(texturePath);
            foreach (string fileName in new[] { "asset_000.json", "metadata.json" })
            {
                string jsonPath = Path.Combine(directory, fileName);
                if (!File.Exists(jsonPath))
                {
                    continue;
                }

                Match match = Regex.Match(File.ReadAllText(jsonPath), "\"name\"\\s*:\\s*\"([^\"]+)\"");
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return null;
        }

        private static string ToProjectPath(string absolutePath)
        {
            string normalized = NormalizePath(absolutePath);
            string projectRoot = NormalizePath(Directory.GetCurrentDirectory());
            if (!normalized.StartsWith(projectRoot))
            {
                return null;
            }

            string relative = normalized.Substring(projectRoot.Length).TrimStart('/');
            return string.IsNullOrEmpty(relative) ? "Assets" : relative;
        }

        private static string NormalizePath(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value.Replace('\\', '/');
        }

        private static string Sanitize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "tiles";
            }

            char[] invalid = Path.GetInvalidFileNameChars();
            string sanitized = new string(value.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray()).Trim();
            return string.IsNullOrWhiteSpace(sanitized) ? "tiles" : sanitized;
        }
    }
}
