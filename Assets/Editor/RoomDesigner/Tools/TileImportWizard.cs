using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class TileImportWizard : EditorWindow
{
    private const string DefaultFolder = "Assets/Art/Tiles";
    private const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";
    private const string TemplateAssetPath = GeneratedFolder + "/RuleTile_F1_Wang_Template.asset";

    private string selectedFolder = GeneratedFolder;
    private Vector2 scrollPosition;
    private string lastResult = "No import run yet.";

    [MenuItem("RIMA/Tile Import Wizard")]
    public static void Open()
    {
        EnsureTemplateAsset();
        GetWindow<TileImportWizard>("Tile Import Wizard");
    }

    public static ImportReport ImportFolderForAutomation(string folderPath)
    {
        return ImportFolder(folderPath, false);
    }

    public static RuleTile EnsureTemplateAsset()
    {
        EnsureAssetFolder(GeneratedFolder);

        RuleTile existing = AssetDatabase.LoadAssetAtPath<RuleTile>(TemplateAssetPath);
        if (existing != null)
        {
            return existing;
        }

        RuleTile template = CreateWangRuleTile(null);
        AssetDatabase.CreateAsset(template, TemplateAssetPath);
        AssetDatabase.SaveAssets();
        return template;
    }

    private void OnEnable()
    {
        EnsureTemplateAsset();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("PixelLab Export Folder", EditorStyles.boldLabel);

        using (new EditorGUILayout.HorizontalScope())
        {
            selectedFolder = EditorGUILayout.TextField(selectedFolder);
            if (GUILayout.Button("Select Folder", GUILayout.Width(120)))
            {
                string absoluteDefault = ToAbsolutePath(DefaultFolder);
                string chosen = EditorUtility.OpenFolderPanel("Select PixelLab export folder", absoluteDefault, "");
                if (!string.IsNullOrEmpty(chosen))
                {
                    selectedFolder = ToAssetPath(chosen);
                }
            }
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Import"))
            {
                ImportReport report = ImportFolder(selectedFolder, true);
                lastResult = report.ToDisplayText();
            }

            if (GUILayout.Button("Create Template"))
            {
                RuleTile template = EnsureTemplateAsset();
                Selection.activeObject = template;
                lastResult = "Template ready: " + TemplateAssetPath;
            }
        }

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Last Result", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.HelpBox(lastResult, MessageType.Info);
        EditorGUILayout.EndScrollView();
    }

    private static ImportReport ImportFolder(string folderPath, bool showDialogs)
    {
        ImportReport report = new ImportReport();

        string assetFolder = NormalizeAssetFolder(folderPath);
        if (string.IsNullOrEmpty(assetFolder) || !AssetDatabase.IsValidFolder(assetFolder))
        {
            report.Errors.Add("Folder is not inside this Unity project: " + folderPath);
            ShowError(showDialogs, "Invalid Folder", report.Errors[0]);
            return report;
        }

        EnsureTemplateAsset();

        string absoluteFolder = ToAbsolutePath(assetFolder);
        string[] jsonPaths = Directory.GetFiles(absoluteFolder, "*.json", SearchOption.TopDirectoryOnly);
        if (jsonPaths.Length == 0)
        {
            string message = "No PixelLab JSON metadata found. Falling back to existing wang tile assets.";
            report.Warnings.Add(message);
            ShowWarning(showDialogs, "Missing Metadata", message);

            try
            {
                report.RuleTilesCreated += CreateRuleTilesFromExistingTileAssets(assetFolder, report);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception exception)
            {
                report.Errors.Add(exception.Message);
                ShowError(showDialogs, "Fallback Import Failed", exception.Message);
            }

            return report;
        }

        try
        {
            for (int i = 0; i < jsonPaths.Length; i++)
            {
                string jsonPath = jsonPaths[i];
                EditorUtility.DisplayProgressBar("Tile Import Wizard", "Reading " + Path.GetFileName(jsonPath), Progress(i, jsonPaths.Length));

                PixelLabTileBatch batch = ParseBatch(jsonPath, report);
                if (batch == null)
                {
                    continue;
                }

                string jsonAssetPath = ToAssetPath(jsonPath);
                string jsonName = Path.GetFileNameWithoutExtension(jsonPath);
                Dictionary<int, Sprite> spritesByMask = SliceTextures(assetFolder, batch, report);

                if (spritesByMask.Count > 0)
                {
                    string tileName = SanitizeTileSetName(jsonName);
                    CreateRuleTileAsset(tileName, spritesByMask);
                    report.RuleTilesCreated++;
                    report.ProcessedMetadata.Add(jsonAssetPath);
                }
            }
        }
        catch (Exception exception)
        {
            report.Errors.Add(exception.Message);
            ShowError(showDialogs, "Import Failed", exception.Message);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (report.Errors.Count == 0 && report.RuleTilesCreated == 0)
        {
            string message = "Metadata was found, but no RuleTile assets were created.";
            report.Warnings.Add(message);
            ShowWarning(showDialogs, "No RuleTiles Created", message);
        }

        return report;
    }

    private static PixelLabTileBatch ParseBatch(string jsonPath, ImportReport report)
    {
        string json = File.ReadAllText(jsonPath);
        PixelLabTileBatch batch = JsonUtility.FromJson<PixelLabTileBatch>(json);
        if (batch == null || batch.tiles == null || batch.tiles.Length == 0 || batch.tile_size <= 0)
        {
            string message = "Malformed PixelLab metadata: " + ToAssetPath(jsonPath);
            report.Errors.Add(message);
            return null;
        }

        if (!string.Equals(batch.tile_type, "topdown_wang", StringComparison.OrdinalIgnoreCase))
        {
            report.Warnings.Add("Metadata tile_type is not topdown_wang: " + ToAssetPath(jsonPath));
        }

        return batch;
    }

    private static Dictionary<int, Sprite> SliceTextures(string assetFolder, PixelLabTileBatch batch, ImportReport report)
    {
        Dictionary<int, Sprite> spritesByMask = new Dictionary<int, Sprite>();
        string[] texturePaths = Directory.GetFiles(ToAbsolutePath(assetFolder), "*.png", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < texturePaths.Length; i++)
        {
            string textureAssetPath = ToAssetPath(texturePaths[i]);
            EditorUtility.DisplayProgressBar("Tile Import Wizard", "Slicing " + Path.GetFileName(textureAssetPath), Progress(i, texturePaths.Length));

            TextureImporter importer = AssetImporter.GetAtPath(textureAssetPath) as TextureImporter;
            if (importer == null)
            {
                continue;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;
            importer.spritePixelsPerUnit = batch.tile_size;
            ApplySpriteRects(importer, Path.GetFileNameWithoutExtension(textureAssetPath), batch);

            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(textureAssetPath);
            foreach (PixelLabTileEntry entry in batch.tiles)
            {
                int mask = ParseMask(entry);
                if (mask < 0 || mask > 15 || spritesByMask.ContainsKey(mask))
                {
                    continue;
                }

                string spriteName = BuildSpriteName(Path.GetFileNameWithoutExtension(textureAssetPath), entry);
                Sprite sprite = assets.OfType<Sprite>().FirstOrDefault(candidate => candidate.name == spriteName);
                if (sprite != null)
                {
                    spritesByMask[mask] = sprite;
                }
            }
        }

        if (spritesByMask.Count == 0)
        {
            report.Warnings.Add("No sliced sprites matched Wang masks in " + assetFolder);
        }

        return spritesByMask;
    }

    private static void ApplySpriteRects(TextureImporter importer, string textureName, PixelLabTileBatch batch)
    {
        SpriteDataProviderFactories factories = new SpriteDataProviderFactories();
        factories.Init();

        ISpriteEditorDataProvider dataProvider = factories.GetSpriteEditorDataProviderFromObject(importer);
        dataProvider.InitSpriteEditorDataProvider();

        SpriteRect[] spriteRects = CreateSpriteRects(textureName, batch);
        dataProvider.SetSpriteRects(spriteRects);

        ISpriteNameFileIdDataProvider nameFileIdProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
        if (nameFileIdProvider != null)
        {
            nameFileIdProvider.SetNameFileIdPairs(spriteRects.Select(rect => new SpriteNameFileIdPair(rect.name, rect.spriteID)));
        }

        dataProvider.Apply();
        importer.SaveAndReimport();
    }

    private static SpriteRect[] CreateSpriteRects(string textureName, PixelLabTileBatch batch)
    {
        SpriteRect[] spriteRects = new SpriteRect[batch.tiles.Length];
        for (int i = 0; i < batch.tiles.Length; i++)
        {
            PixelLabTileEntry entry = batch.tiles[i];
            spriteRects[i] = new SpriteRect
            {
                name = BuildSpriteName(textureName, entry),
                rect = new Rect(entry.sprite_x, entry.sprite_y, entry.width, entry.height),
                pivot = new Vector2(0.5f, 0.5f),
                alignment = SpriteAlignment.Center,
                spriteID = GUID.Generate()
            };
        }

        return spriteRects;
    }

    private static int CreateRuleTilesFromExistingTileAssets(string assetFolder, ImportReport report)
    {
        string[] tileGuids = AssetDatabase.FindAssets("t:Tile wang", new[] { assetFolder });
        Dictionary<string, Dictionary<int, Sprite>> groupedSprites = new Dictionary<string, Dictionary<int, Sprite>>();

        foreach (string guid in tileGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!TryParseExistingTileAsset(assetPath, out string tileSetName, out int mask))
            {
                continue;
            }

            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(assetPath);
            if (tile == null || tile.sprite == null)
            {
                continue;
            }

            if (!groupedSprites.TryGetValue(tileSetName, out Dictionary<int, Sprite> sprites))
            {
                sprites = new Dictionary<int, Sprite>();
                groupedSprites.Add(tileSetName, sprites);
            }

            if (!sprites.ContainsKey(mask))
            {
                sprites.Add(mask, tile.sprite);
            }
        }

        int created = 0;
        foreach (KeyValuePair<string, Dictionary<int, Sprite>> group in groupedSprites)
        {
            if (group.Value.Count < 16)
            {
                report.Warnings.Add("Skipped incomplete Wang set " + group.Key + " (" + group.Value.Count + "/16).");
                continue;
            }

            CreateRuleTileAsset(group.Key, group.Value);
            created++;
        }

        return created;
    }

    private static bool TryParseExistingTileAsset(string assetPath, out string tileSetName, out int mask)
    {
        tileSetName = null;
        mask = -1;

        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        int marker = fileName.LastIndexOf("_tile_", StringComparison.Ordinal);
        if (marker < 0)
        {
            return false;
        }

        string suffix = fileName.Substring(marker + "_tile_".Length);
        if (!int.TryParse(suffix, out mask) || mask < 0 || mask > 15)
        {
            return false;
        }

        tileSetName = SanitizeTileSetName(fileName.Substring(0, marker));
        return !string.IsNullOrEmpty(tileSetName);
    }

    private static RuleTile CreateRuleTileAsset(string tileSetName, Dictionary<int, Sprite> spritesByMask)
    {
        RuleTile ruleTile = CreateWangRuleTile(spritesByMask);
        string assetName = "wang_" + SanitizeTileSetName(tileSetName) + "_RuleTile.asset";
        string assetPath = GetUniqueNewAssetPath(GeneratedFolder + "/" + assetName);
        AssetDatabase.CreateAsset(ruleTile, assetPath);
        return ruleTile;
    }

    private static RuleTile CreateWangRuleTile(Dictionary<int, Sprite> spritesByMask)
    {
        RuleTile ruleTile = ScriptableObject.CreateInstance<RuleTile>();
        ruleTile.m_DefaultColliderType = Tile.ColliderType.Sprite;
        ruleTile.m_TilingRules = new List<RuleTile.TilingRule>();

        for (int mask = 0; mask < 16; mask++)
        {
            RuleTile.TilingRule rule = new RuleTile.TilingRule
            {
                m_ColliderType = Tile.ColliderType.Sprite,
                m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed,
                m_Sprites = new Sprite[1]
            };

            if (spritesByMask != null && spritesByMask.TryGetValue(mask, out Sprite sprite))
            {
                rule.m_Sprites[0] = sprite;
            }

            rule.ApplyNeighbors(CreateNsewNeighborMap(mask));
            ruleTile.m_TilingRules.Add(rule);
        }

        return ruleTile;
    }

    private static Dictionary<Vector3Int, int> CreateNsewNeighborMap(int mask)
    {
        return new Dictionary<Vector3Int, int>
        {
            { Vector3Int.up, HasBit(mask, 0) ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis },
            { Vector3Int.down, HasBit(mask, 1) ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis },
            { Vector3Int.right, HasBit(mask, 2) ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis },
            { Vector3Int.left, HasBit(mask, 3) ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis }
        };
    }

    private static bool HasBit(int mask, int bit)
    {
        return (mask & (1 << bit)) != 0;
    }

    private static int ParseMask(PixelLabTileEntry entry)
    {
        if (!string.IsNullOrEmpty(entry.wang_mask))
        {
            if (entry.wang_mask.Length == 4 && entry.wang_mask.All(character => character == '0' || character == '1'))
            {
                int mask = 0;
                for (int i = 0; i < entry.wang_mask.Length; i++)
                {
                    if (entry.wang_mask[i] == '1')
                    {
                        mask |= 1 << i;
                    }
                }

                return mask;
            }

            if (int.TryParse(entry.wang_mask, out int parsedMask))
            {
                return parsedMask;
            }
        }

        return entry.index;
    }

    private static string BuildSpriteName(string textureName, PixelLabTileEntry entry)
    {
        return textureName + "_" + ParseMask(entry);
    }

    private static string SanitizeTileSetName(string value)
    {
        string name = value;
        if (name.StartsWith("wang_", StringComparison.OrdinalIgnoreCase))
        {
            name = name.Substring("wang_".Length);
        }

        return string.Join("_", name.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries))
            .Replace(" ", "_")
            .Trim('_');
    }

    private static string GetUniqueNewAssetPath(string desiredPath)
    {
        if (!File.Exists(ToAbsolutePath(desiredPath)))
        {
            return desiredPath;
        }

        string folder = Path.GetDirectoryName(desiredPath).Replace('\\', '/');
        string baseName = Path.GetFileNameWithoutExtension(desiredPath);
        string extension = Path.GetExtension(desiredPath);
        string candidate = folder + "/" + baseName + "_new" + extension;
        int suffix = 2;

        while (File.Exists(ToAbsolutePath(candidate)))
        {
            candidate = folder + "/" + baseName + "_new" + suffix + extension;
            suffix++;
        }

        return candidate;
    }

    private static void EnsureAssetFolder(string assetFolder)
    {
        string absolutePath = ToAbsolutePath(assetFolder);
        if (!Directory.Exists(absolutePath))
        {
            Directory.CreateDirectory(absolutePath);
            AssetDatabase.Refresh();
        }
    }

    private static string NormalizeAssetFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            return null;
        }

        string normalized = folderPath.Replace('\\', '/');
        if (normalized.StartsWith("Assets/", StringComparison.Ordinal) || normalized == "Assets")
        {
            return normalized.TrimEnd('/');
        }

        return ToAssetPath(normalized).TrimEnd('/');
    }

    private static string ToAbsolutePath(string assetPath)
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName.Replace('\\', '/');
        if (assetPath.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase))
        {
            return assetPath.Replace('\\', '/');
        }

        return Path.Combine(projectRoot, assetPath).Replace('\\', '/');
    }

    private static string ToAssetPath(string absolutePath)
    {
        string normalized = absolutePath.Replace('\\', '/');
        string projectRoot = Directory.GetParent(Application.dataPath).FullName.Replace('\\', '/');
        if (normalized.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase))
        {
            return normalized.Substring(projectRoot.Length + 1);
        }

        return normalized;
    }

    private static float Progress(int index, int count)
    {
        if (count <= 0)
        {
            return 0f;
        }

        return Mathf.Clamp01((index + 1f) / count);
    }

    private static void ShowError(bool showDialogs, string title, string message)
    {
        if (showDialogs)
        {
            EditorUtility.DisplayDialog(title, message, "OK");
        }
    }

    private static void ShowWarning(bool showDialogs, string title, string message)
    {
        if (showDialogs)
        {
            EditorUtility.DisplayDialog(title, message, "OK");
        }
    }

    [Serializable]
    private sealed class PixelLabTileBatch
    {
        public int tile_size;
        public string tile_type;
        public PixelLabTileEntry[] tiles;
    }

    [Serializable]
    private sealed class PixelLabTileEntry
    {
        public int index;
        public string wang_mask;
        public int sprite_x;
        public int sprite_y;
        public int width;
        public int height;
    }

    [Serializable]
    public sealed class ImportReport
    {
        public int RuleTilesCreated;
        public readonly List<string> ProcessedMetadata = new List<string>();
        public readonly List<string> Warnings = new List<string>();
        public readonly List<string> Errors = new List<string>();

        public string ToDisplayText()
        {
            List<string> lines = new List<string>
            {
                "RuleTiles created: " + RuleTilesCreated,
                "Metadata files: " + ProcessedMetadata.Count,
                "Warnings: " + Warnings.Count,
                "Errors: " + Errors.Count
            };

            lines.AddRange(Warnings.Select(warning => "Warning: " + warning));
            lines.AddRange(Errors.Select(error => "Error: " + error));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
