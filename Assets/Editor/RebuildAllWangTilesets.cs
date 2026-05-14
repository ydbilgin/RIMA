using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RIMA;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public static class RebuildAllWangTilesets
    {
        private const string TilesetRoot = "Assets/Art/Tiles/F1/Tilesets";
        private const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";

        private static readonly string[] TilesetNames =
        {
            "floor_wall",
            "rubble_path",
            "debris_rift",
            "cold_floor_wall",
            "slate_mineral",
            "mauve_hexagon",
            "wall_path",
            "wall_rift",
            "path_rift",
            "rubble_moss",
            "pink_cream"
        };

        [MenuItem("RIMA/Tools/Rebuild All Wang Tilesets")]
        public static void RebuildAll()
        {
            EnsureGeneratedFolder();

            foreach (string name in TilesetNames)
            {
                RebuildOne(name);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[RIMA] Rebuilt all Wang tilesets.");
        }

        private static void RebuildOne(string name)
        {
            string tilesetFolder = $"{TilesetRoot}/{name}";
            string metaPath = $"{tilesetFolder}/tileset_meta.json";
            TilesetMeta meta = LoadMeta(metaPath);
            string spritesheetPath = $"{tilesetFolder}/{(string.IsNullOrEmpty(meta.spritesheet) ? "spritesheet.png" : meta.spritesheet)}";

            ImportSpritesheet(spritesheetPath, meta);
            Sprite[] sprites = LoadSprites(spritesheetPath, meta);
            CreateTiles(meta, sprites);
            CreateTileSet(meta);
        }

        private static TilesetMeta LoadMeta(string metaPath)
        {
            if (!File.Exists(metaPath))
            {
                throw new FileNotFoundException("Missing tileset metadata", metaPath);
            }

            TilesetMeta meta = JsonUtility.FromJson<TilesetMeta>(File.ReadAllText(metaPath));
            if (meta == null || string.IsNullOrEmpty(meta.name))
            {
                throw new InvalidOperationException("Invalid tileset metadata: " + metaPath);
            }

            if (meta.tileSize <= 0)
            {
                meta.tileSize = 32;
            }

            if (meta.gridCols <= 0)
            {
                meta.gridCols = 4;
            }

            if (meta.gridRows <= 0)
            {
                meta.gridRows = 4;
            }

            if (meta.cornerKeyToSpriteIndex == null || meta.cornerKeyToSpriteIndex.Length != 16)
            {
                throw new InvalidOperationException("cornerKeyToSpriteIndex must contain 16 entries: " + metaPath);
            }

            return meta;
        }

        private static void ImportSpritesheet(string spritesheetPath, TilesetMeta meta)
        {
            TextureImporter importer = AssetImporter.GetAtPath(spritesheetPath) as TextureImporter;
            if (importer == null)
            {
                throw new FileNotFoundException("Missing spritesheet texture", spritesheetPath);
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.spritePixelsPerUnit = meta.tileSize;

            var rects = new List<SpriteRect>();
            for (int row = 0; row < meta.gridRows; row++)
            {
                for (int col = 0; col < meta.gridCols; col++)
                {
                    int idx = row * meta.gridCols + col;
                    rects.Add(new SpriteRect
                    {
                        name = $"{meta.name}_{idx}",
                        rect = new Rect(
                            col * meta.tileSize,
                            (meta.gridRows - 1 - row) * meta.tileSize,
                            meta.tileSize,
                            meta.tileSize),
                        pivot = new Vector2(0.5f, 0.5f),
                        alignment = SpriteAlignment.Center,
                        spriteID = GUID.Generate()
                    });
                }
            }

            ApplySpriteRects(importer, rects);
            AssetDatabase.ImportAsset(spritesheetPath, ImportAssetOptions.ForceUpdate);
        }

        private static Sprite[] LoadSprites(string spritesheetPath, TilesetMeta meta)
        {
            Sprite[] allSprites = AssetDatabase.LoadAllAssetsAtPath(spritesheetPath)
                .OfType<Sprite>()
                .OrderByDescending(s => (int)s.rect.y)
                .ThenBy(s => (int)s.rect.x)
                .ToArray();

            int expected = meta.gridRows * meta.gridCols;
            if (allSprites.Length != expected)
            {
                throw new InvalidOperationException($"Expected {expected} sprites in {spritesheetPath}, found {allSprites.Length}.");
            }

            return allSprites;
        }

        private static void CreateTiles(TilesetMeta meta, Sprite[] sprites)
        {
            for (int cornerKey = 0; cornerKey < 16; cornerKey++)
            {
                int spriteIndex = meta.cornerKeyToSpriteIndex[cornerKey];
                if (spriteIndex < 0 || spriteIndex >= sprites.Length)
                {
                    throw new InvalidOperationException($"Invalid sprite index {spriteIndex} for {meta.name} corner key {cornerKey}.");
                }

                string tilePath = $"{GeneratedFolder}/wang_{meta.name}_tile_{cornerKey}.asset";
                AssetDatabase.DeleteAsset(tilePath);

                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = sprites[spriteIndex];
                AssetDatabase.CreateAsset(tile, tilePath);
            }
        }

        private static void CreateTileSet(TilesetMeta meta)
        {
            string pascalName = ToPascal(meta.name);
            string soPath = $"{GeneratedFolder}/{pascalName}_CornerWangTileSet.asset";
            AssetDatabase.DeleteAsset(soPath);

            CornerWangTileSetSO so = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
            so.lowerTerrainLabel = meta.lower;
            so.upperTerrainLabel = meta.upper;
            so.tiles = new TileBase[16];
            for (int cornerKey = 0; cornerKey < 16; cornerKey++)
            {
                so.tiles[cornerKey] = AssetDatabase.LoadAssetAtPath<TileBase>($"{GeneratedFolder}/wang_{meta.name}_tile_{cornerKey}.asset");
            }

            so.variantsByKey = BuildVariantsByKey(meta.name);
            AssetDatabase.CreateAsset(so, soPath);
        }

        private static CornerWangTileSetSO.WangVariants[] BuildVariantsByKey(string tilesetName)
        {
            var variantsByKey = new CornerWangTileSetSO.WangVariants[16];
            for (int cornerKey = 0; cornerKey < 16; cornerKey++)
            {
                var variants = new List<TileBase>();
                AddIfPresent(variants, $"{GeneratedFolder}/wang_{tilesetName}_tile_{cornerKey}.asset");
                for (int variantIndex = 1; variantIndex <= 5; variantIndex++)
                {
                    AddIfPresent(variants, $"{GeneratedFolder}/wang_{tilesetName}_tile_{cornerKey}_v{variantIndex}.asset");
                }

                variantsByKey[cornerKey] = new CornerWangTileSetSO.WangVariants { variants = variants.ToArray() };
            }

            return variantsByKey;
        }

        private static void AddIfPresent(List<TileBase> variants, string path)
        {
            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile != null)
            {
                variants.Add(tile);
            }
        }

        private static void EnsureGeneratedFolder()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Art/Tiles/F1/Generated"))
            {
                AssetDatabase.CreateFolder("Assets/Art/Tiles/F1", "Generated");
            }
        }

        private static string ToPascal(string value)
        {
            return string.Concat(value.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
        }

        private static void ApplySpriteRects(TextureImporter importer, List<SpriteRect> rects)
        {
            var factories = new SpriteDataProviderFactories();
            factories.Init();
            ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
            provider.InitSpriteEditorDataProvider();
            provider.SetSpriteRects(rects.ToArray());
            provider.Apply();
        }

        [Serializable]
        private class TilesetMeta
        {
            public string name;
            public int tileSize;
            public string lower;
            public string upper;
            public string spritesheet;
            public int gridRows;
            public int gridCols;
            public int[] cornerKeyToSpriteIndex;
        }
    }
}

internal static class WangTilesetBuilder
{
    private const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";

    public static string CreateFromMeta(string metaPath, string pascalNameOverride = null)
    {
        TilesetMeta meta = LoadMeta(metaPath);
        string tilesetFolder = Path.GetDirectoryName(metaPath)?.Replace('\\', '/');
        if (string.IsNullOrEmpty(tilesetFolder))
        {
            throw new InvalidOperationException("Invalid tileset meta path: " + metaPath);
        }

        string spritesheetFile = string.IsNullOrEmpty(meta.spritesheet) ? "spritesheet.png" : meta.spritesheet;
        string spritesheetPath = $"{tilesetFolder}/{spritesheetFile}";
        string pascalName = string.IsNullOrEmpty(pascalNameOverride) ? ToPascalCase(meta.name) : pascalNameOverride;

        EnsureGeneratedFolder();
        ImportSpritesheet(spritesheetPath, meta);
        Sprite[] sprites = LoadSprites(spritesheetPath, meta);
        CreateTiles(meta, sprites);
        string soPath = CreateTileSet(meta, pascalName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[RIMA] Created Wang tileset SO: " + soPath);
        return soPath;
    }

    public static TilesetMeta LoadMeta(string metaPath)
    {
        if (!File.Exists(metaPath))
        {
            throw new FileNotFoundException("Missing tileset_meta.json", metaPath);
        }

        TilesetMeta meta = JsonUtility.FromJson<TilesetMeta>(File.ReadAllText(metaPath));
        if (meta == null || string.IsNullOrEmpty(meta.name))
        {
            throw new InvalidOperationException("Could not parse tileset meta: " + metaPath);
        }

        if (meta.tileSize <= 0)
        {
            meta.tileSize = 32;
        }

        if (meta.gridRows <= 0)
        {
            meta.gridRows = 4;
        }

        if (meta.gridCols <= 0)
        {
            meta.gridCols = 4;
        }

        if (meta.cornerKeyToSpriteIndex == null || meta.cornerKeyToSpriteIndex.Length != 16)
        {
            throw new InvalidOperationException("Tileset meta must define 16 corner keys: " + metaPath);
        }

        return meta;
    }

    public static string FindMetaPathForTexture(Texture2D texture)
    {
        if (texture == null)
        {
            return string.Empty;
        }

        string texturePath = AssetDatabase.GetAssetPath(texture);
        if (string.IsNullOrEmpty(texturePath))
        {
            return string.Empty;
        }

        string directory = Path.GetDirectoryName(texturePath)?.Replace('\\', '/');
        return string.IsNullOrEmpty(directory) ? string.Empty : $"{directory}/tileset_meta.json";
    }

    private static void ImportSpritesheet(string spritesheetPath, TilesetMeta meta)
    {
        TextureImporter importer = AssetImporter.GetAtPath(spritesheetPath) as TextureImporter;
        if (importer == null)
        {
            throw new FileNotFoundException("Missing spritesheet texture", spritesheetPath);
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.spritePixelsPerUnit = meta.tileSize;

        var rects = new List<SpriteRect>();
        for (int row = 0; row < meta.gridRows; row++)
        {
            for (int col = 0; col < meta.gridCols; col++)
            {
                int index = row * meta.gridCols + col;
                rects.Add(new SpriteRect
                {
                    name = $"{meta.name}_{index}",
                    rect = new Rect(
                        col * meta.tileSize,
                        (meta.gridRows - 1 - row) * meta.tileSize,
                        meta.tileSize,
                        meta.tileSize),
                    pivot = new Vector2(0.5f, 0.5f),
                    alignment = SpriteAlignment.Center,
                    spriteID = GUID.Generate()
                });
            }
        }

        ApplySpriteRects(importer, rects);
        AssetDatabase.ImportAsset(spritesheetPath, ImportAssetOptions.ForceUpdate);
    }

    private static Sprite[] LoadSprites(string spritesheetPath, TilesetMeta meta)
    {
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritesheetPath)
            .OfType<Sprite>()
            .OrderByDescending(sprite => (int)sprite.rect.y)
            .ThenBy(sprite => (int)sprite.rect.x)
            .ToArray();

        int expectedCount = meta.gridRows * meta.gridCols;
        if (sprites.Length != expectedCount)
        {
            throw new InvalidOperationException($"Expected {expectedCount} sprites at {spritesheetPath}, found {sprites.Length}.");
        }

        return sprites;
    }

    private static void CreateTiles(TilesetMeta meta, Sprite[] sprites)
    {
        for (int cornerKey = 0; cornerKey < 16; cornerKey++)
        {
            int spriteIndex = meta.cornerKeyToSpriteIndex[cornerKey];
            if (spriteIndex < 0 || spriteIndex >= sprites.Length)
            {
                throw new InvalidOperationException($"Sprite index {spriteIndex} is out of range for {meta.name} corner key {cornerKey}.");
            }

            string tilePath = GetTilePath(meta.name, cornerKey);
            AssetDatabase.DeleteAsset(tilePath);

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprites[spriteIndex];
            AssetDatabase.CreateAsset(tile, tilePath);
        }
    }

    private static string CreateTileSet(TilesetMeta meta, string pascalName)
    {
        string soPath = $"{GeneratedFolder}/{pascalName}_CornerWangTileSet.asset";
        AssetDatabase.DeleteAsset(soPath);

        CornerWangTileSetSO tileSet = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
        tileSet.lowerTerrainLabel = meta.lower;
        tileSet.upperTerrainLabel = meta.upper;
        tileSet.tiles = new TileBase[16];
        for (int cornerKey = 0; cornerKey < 16; cornerKey++)
        {
            tileSet.tiles[cornerKey] = AssetDatabase.LoadAssetAtPath<TileBase>(GetTilePath(meta.name, cornerKey));
        }

        tileSet.variantsByKey = BuildVariantsByKey(meta.name);
        AssetDatabase.CreateAsset(tileSet, soPath);
        return soPath;
    }

    private static CornerWangTileSetSO.WangVariants[] BuildVariantsByKey(string tilesetName)
    {
        var variantsByKey = new CornerWangTileSetSO.WangVariants[16];
        for (int cornerKey = 0; cornerKey < 16; cornerKey++)
        {
            var variants = new List<TileBase>();
            AddIfPresent(variants, GetTilePath(tilesetName, cornerKey));
            for (int variantIndex = 1; variantIndex <= 5; variantIndex++)
            {
                AddIfPresent(variants, $"{GeneratedFolder}/wang_{tilesetName}_tile_{cornerKey}_v{variantIndex}.asset");
            }

            variantsByKey[cornerKey] = new CornerWangTileSetSO.WangVariants { variants = variants.ToArray() };
        }

        return variantsByKey;
    }

    private static void AddIfPresent(List<TileBase> variants, string path)
    {
        TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
        if (tile != null)
        {
            variants.Add(tile);
        }
    }

    private static string GetTilePath(string name, int cornerKey)
    {
        return $"{GeneratedFolder}/wang_{name}_tile_{cornerKey}.asset";
    }

    private static void EnsureGeneratedFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Art/Tiles/F1/Generated"))
        {
            AssetDatabase.CreateFolder("Assets/Art/Tiles/F1", "Generated");
        }
    }

    private static string ToPascalCase(string value)
    {
        return string.Concat(value.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
    }

    private static void ApplySpriteRects(TextureImporter importer, List<SpriteRect> rects)
    {
        var factories = new SpriteDataProviderFactories();
        factories.Init();
        ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
        provider.InitSpriteEditorDataProvider();
        provider.SetSpriteRects(rects.ToArray());
        provider.Apply();
    }
}

[Serializable]
internal class TilesetMeta
{
    public string name;
    public int tileSize;
    public string upper;
    public string lower;
    public string spritesheet;
    public int gridRows;
    public int gridCols;
    public int[] cornerKeyToSpriteIndex;
}
