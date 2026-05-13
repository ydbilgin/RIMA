namespace RIMA.Editor.TileImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using Object = UnityEngine.Object;

    public static class PixelLabWangImporter
    {
        public const int GridSize = 4;
        public const int TileSize = 32;
        public const int ExpectedTileCount = 16;
        private const int DontCare = 0;
        private const int PixelsPerUnit = 32;
        private const string DefaultOutputFolder = "Assets/Art/Tiles/F1/Generated";

        private static readonly Vector3Int[] NeighborPositions =
        {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(-1, 1, 0)
        };

        [MenuItem("RIMA/PixelLab Wang Tileset Importer")]
        public static void ImportFromMenu()
        {
            string selected = EditorUtility.OpenFilePanel("PixelLab Wang Tileset Importer", "Assets/Art/Tiles/F1", "png");
            if (string.IsNullOrWhiteSpace(selected))
            {
                return;
            }

            string projectPath = ToProjectPath(selected);
            if (string.IsNullOrEmpty(projectPath))
            {
                Debug.LogError("PixelLab Wang import skipped: selected PNG must be inside this Unity project.");
                return;
            }

            try
            {
                string assetPath = ImportWangSheetAsset(projectPath);
                Debug.Log($"Wang RuleTile created at {assetPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"PixelLab Wang import failed: {ex.Message}");
            }
        }

        public static WangMetadataDocument ParseMetadata(string metadataPath)
        {
            if (string.IsNullOrWhiteSpace(metadataPath) || !File.Exists(metadataPath))
            {
                throw new FileNotFoundException("metadata.json gerekli", metadataPath);
            }

            var document = JsonUtility.FromJson<WangMetadataDocument>(File.ReadAllText(metadataPath));
            if (document == null || document.tileset_data == null || document.tileset_data.tiles == null)
            {
                throw new InvalidDataException("PixelLab Wang metadata is missing tileset_data.tiles.");
            }

            if (document.tileset_data.tiles.Length != ExpectedTileCount)
            {
                throw new InvalidDataException($"PixelLab Wang metadata expected {ExpectedTileCount} tiles, found {document.tileset_data.tiles.Length}.");
            }

            return document;
        }

        public static string ImportWangSheetAsset(string texturePath, string outputFolder = DefaultOutputFolder)
        {
            string normalizedTexturePath = NormalizePath(texturePath);
            string metadataPath = NormalizePath(Path.Combine(Path.GetDirectoryName(normalizedTexturePath) ?? string.Empty, "metadata.json"));
            WangMetadataDocument metadata = ParseMetadata(metadataPath);

            EnsureFolder(outputFolder);
            ConfigureImporter(normalizedTexturePath);
            Sprite[] sprites = LoadSlicedSprites(normalizedTexturePath);
            string assetPath = $"{NormalizePath(outputFolder)}/{Path.GetFileNameWithoutExtension(normalizedTexturePath)}_RuleTile.asset";
            CreateRuleTileAsset(metadata, sprites, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Wang RuleTile created at {assetPath}");
            return assetPath;
        }

        public static RuleTile CreateRuleTileAsset(WangMetadataDocument metadata, Sprite[] sprites, string assetPath)
        {
            if (metadata == null || metadata.tileset_data == null || metadata.tileset_data.tiles == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (sprites == null || sprites.Length != ExpectedTileCount)
            {
                throw new ArgumentException($"Expected {ExpectedTileCount} sprites.", nameof(sprites));
            }

            var ruleTile = ScriptableObject.CreateInstance<RuleTile>();
            ruleTile.name = Path.GetFileNameWithoutExtension(assetPath);
            ruleTile.m_DefaultColliderType = Tile.ColliderType.None;

            WangTileMetadata allLower = metadata.tileset_data.tiles.FirstOrDefault(IsAllLower);
            ruleTile.m_DefaultSprite = allLower != null ? sprites[SpriteIndexFor(allLower)] : sprites[0];

            foreach (WangTileMetadata tileMetadata in metadata.tileset_data.tiles.OrderByDescending(Specificity))
            {
                int spriteIndex = SpriteIndexFor(tileMetadata);
                var rule = new RuleTile.TilingRule
                {
                    m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                    m_Sprites = new[] { sprites[Mathf.Clamp(spriteIndex, 0, sprites.Length - 1)] },
                    m_ColliderType = Tile.ColliderType.None,
                    m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed,
                    m_NeighborPositions = new List<Vector3Int>(NeighborPositions),
                    m_Neighbors = BuildNeighborMask(tileMetadata)
                };
                ruleTile.m_TilingRules.Add(rule);
            }

            ReplaceAsset(ruleTile, assetPath);
            return ruleTile;
        }

        private static void ConfigureImporter(string texturePath)
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
            importer.spritesheet = BuildSpriteMetadata(Path.GetFileNameWithoutExtension(texturePath));
            importer.SaveAndReimport();
        }

        private static SpriteMetaData[] BuildSpriteMetadata(string baseName)
        {
            var metadata = new SpriteMetaData[ExpectedTileCount];
            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    int index = row * GridSize + column;
                    metadata[index] = new SpriteMetaData
                    {
                        name = $"{baseName}_wang_{index:00}",
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f),
                        rect = new Rect(column * TileSize, (GridSize - 1 - row) * TileSize, TileSize, TileSize)
                    };
                }
            }

            return metadata;
        }

        private static Sprite[] LoadSlicedSprites(string texturePath)
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(texturePath)
                .OfType<Sprite>()
                .OrderByDescending(sprite => sprite.rect.y)
                .ThenBy(sprite => sprite.rect.x)
                .ToArray();

            if (sprites.Length != ExpectedTileCount)
            {
                throw new InvalidDataException($"Expected {ExpectedTileCount} sliced sprites, found {sprites.Length}.");
            }

            return sprites;
        }

        private static List<int> BuildNeighborMask(WangTileMetadata tile)
        {
            CornerSet corners = tile.corners;
            return new List<int>
            {
                SideMatch(corners.NW, corners.NE),
                CornerMatch(corners.NE),
                SideMatch(corners.NE, corners.SE),
                CornerMatch(corners.SE),
                SideMatch(corners.SW, corners.SE),
                CornerMatch(corners.SW),
                SideMatch(corners.NW, corners.SW),
                CornerMatch(corners.NW)
            };
        }

        private static int SideMatch(string a, string b)
        {
            if (IsUpper(a) && IsUpper(b))
            {
                return RuleTile.TilingRuleOutput.Neighbor.This;
            }

            if (!IsUpper(a) && !IsUpper(b))
            {
                return RuleTile.TilingRuleOutput.Neighbor.NotThis;
            }

            return DontCare;
        }

        private static int CornerMatch(string value)
        {
            return IsUpper(value) ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis;
        }

        private static int Specificity(WangTileMetadata tile)
        {
            return BuildNeighborMask(tile).Count(value => value != DontCare);
        }

        private static int SpriteIndexFor(WangTileMetadata tile)
        {
            int column = Mathf.Clamp(tile.bounding_box.x / TileSize, 0, GridSize - 1);
            int row = Mathf.Clamp(tile.bounding_box.y / TileSize, 0, GridSize - 1);
            return row * GridSize + column;
        }

        private static bool IsAllLower(WangTileMetadata tile)
        {
            CornerSet corners = tile.corners;
            return !IsUpper(corners.NW) && !IsUpper(corners.NE) && !IsUpper(corners.SW) && !IsUpper(corners.SE);
        }

        private static bool IsUpper(string value)
        {
            return string.Equals(value, "upper", StringComparison.OrdinalIgnoreCase);
        }

        private static void ReplaceAsset(Object asset, string assetPath)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(asset, assetPath);
        }

        private static void EnsureFolder(string folderPath)
        {
            string normalized = NormalizePath(folderPath);
            string[] parts = normalized.Split('/');
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

        private static string ToProjectPath(string absolutePath)
        {
            string normalized = NormalizePath(absolutePath);
            string projectRoot = NormalizePath(Directory.GetCurrentDirectory());
            if (!normalized.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return normalized.Substring(projectRoot.Length).TrimStart('/');
        }

        private static string NormalizePath(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value.Replace('\\', '/');
        }

        [Serializable]
        public sealed class WangMetadataDocument
        {
            public WangTilesetData tileset_data;
        }

        [Serializable]
        public sealed class WangTilesetData
        {
            public WangTileMetadata[] tiles;
            public TileSizeMetadata tile_size;
            public int total_tiles;
        }

        [Serializable]
        public sealed class WangTileMetadata
        {
            public string id;
            public string name;
            public CornerSet corners;
            public BoundingBox bounding_box;
        }

        [Serializable]
        public sealed class CornerSet
        {
            public string NE;
            public string NW;
            public string SE;
            public string SW;
        }

        [Serializable]
        public sealed class BoundingBox
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        [Serializable]
        public sealed class TileSizeMetadata
        {
            public int width;
            public int height;
        }
    }
}
