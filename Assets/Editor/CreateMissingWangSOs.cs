using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RIMA;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class CreateMissingWangSOs
{
    private static readonly TilesetRequest[] Requests =
    {
        new TilesetRequest("Assets/Art/Tiles/F1/Tilesets/debris_rift", "DebrisRift"),
        new TilesetRequest("Assets/Art/Tiles/F1/Tilesets/cold_floor_wall", "ColdFloorWall"),
        new TilesetRequest("Assets/Art/Tiles/F1/Tilesets/slate_mineral", "SlateMineral"),
        new TilesetRequest("Assets/Art/Tiles/F1/Tilesets/mauve_hexagon", "MauveHexagon")
    };

    [MenuItem("RIMA/Tools/Create Missing Wang SOs")]
    public static void CreateAll()
    {
        foreach (TilesetRequest request in Requests)
        {
            string metaPath = $"{request.TilesetFolder}/tileset_meta.json";
            WangTilesetBuilder.CreateFromMeta(metaPath, request.PascalName);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[RIMA] Missing Wang SOs created.");
    }

    private readonly struct TilesetRequest
    {
        public readonly string TilesetFolder;
        public readonly string PascalName;

        public TilesetRequest(string tilesetFolder, string pascalName)
        {
            TilesetFolder = tilesetFolder;
            PascalName = pascalName;
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
            throw new InvalidOperationException($"Invalid tileset meta path: {metaPath}");
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
        Debug.Log($"[RIMA] Created Wang tileset SO: {soPath}");
        return soPath;
    }

    public static TilesetMeta LoadMeta(string metaPath)
    {
        if (!File.Exists(metaPath))
        {
            throw new FileNotFoundException("Missing tileset_meta.json", metaPath);
        }

        string json = File.ReadAllText(metaPath);
        TilesetMeta meta = JsonUtility.FromJson<TilesetMeta>(json);
        if (meta == null)
        {
            throw new InvalidOperationException($"Could not parse tileset meta: {metaPath}");
        }

        if (string.IsNullOrEmpty(meta.name))
        {
            throw new InvalidOperationException($"Tileset meta missing name: {metaPath}");
        }

        if (meta.tileSize <= 0 || meta.gridRows <= 0 || meta.gridCols <= 0)
        {
            throw new InvalidOperationException($"Tileset meta has invalid grid data: {metaPath}");
        }

        if (meta.cornerKeyToSpriteIndex == null || meta.cornerKeyToSpriteIndex.Length != 16)
        {
            throw new InvalidOperationException($"Tileset meta must define 16 corner keys: {metaPath}");
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
        importer.spritePixelsPerUnit = meta.tileSize;

        var rects = new List<SpriteMetaData>();
        for (int row = 0; row < meta.gridRows; row++)
        {
            for (int col = 0; col < meta.gridCols; col++)
            {
                int index = row * meta.gridCols + col;
                rects.Add(new SpriteMetaData
                {
                    name = $"{meta.name}_{index}",
                    rect = new Rect(
                        col * meta.tileSize,
                        (meta.gridRows - 1 - row) * meta.tileSize,
                        meta.tileSize,
                        meta.tileSize),
                    pivot = new Vector2(0.5f, 0.5f),
                    alignment = (int)SpriteAlignment.Center
                });
            }
        }

        importer.spritesheet = rects.ToArray();
        AssetDatabase.ImportAsset(spritesheetPath, ImportAssetOptions.ForceUpdate);
    }

    private static Sprite[] LoadSprites(string spritesheetPath, TilesetMeta meta)
    {
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritesheetPath)
            .OfType<Sprite>()
            .OrderBy(sprite => -(int)sprite.rect.y)
            .ThenBy(sprite => (int)sprite.rect.x)
            .ToArray();

        int expectedCount = meta.gridRows * meta.gridCols;
        if (sprites.Length != expectedCount)
        {
            throw new InvalidOperationException(
                $"Expected {expectedCount} sprites at {spritesheetPath}, found {sprites.Length}.");
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
                throw new InvalidOperationException(
                    $"Sprite index {spriteIndex} is out of range for {meta.name} corner key {cornerKey}.");
            }

            string tilePath = GetTilePath(meta.name, cornerKey);
            AssetDatabase.DeleteAsset(tilePath);

            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprites[spriteIndex];
            AssetDatabase.CreateAsset(tile, tilePath);
        }
    }

    private static string CreateTileSet(TilesetMeta meta, string pascalName)
    {
        string soPath = $"{GeneratedFolder}/{pascalName}_CornerWangTileSet.asset";
        AssetDatabase.DeleteAsset(soPath);

        var tileSet = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
        tileSet.lowerTerrainLabel = meta.lower;
        tileSet.upperTerrainLabel = meta.upper;
        tileSet.tiles = new TileBase[16];

        for (int cornerKey = 0; cornerKey < 16; cornerKey++)
        {
            string tilePath = GetTilePath(meta.name, cornerKey);
            tileSet.tiles[cornerKey] = AssetDatabase.LoadAssetAtPath<TileBase>(tilePath);
        }

        AssetDatabase.CreateAsset(tileSet, soPath);
        return soPath;
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
        string[] parts = value.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(parts.Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
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
