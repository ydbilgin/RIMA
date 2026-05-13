namespace RIMA.Editor.RoomDesigner.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class TileImportWizard : EditorWindow
    {
        public const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";
        public const string TemplatePath = GeneratedFolder + "/RuleTile_F1_Wang_Template.asset";

        private const int DefaultTileSize = 32;
        private const string DefaultPickFolder = "Assets/Art/Tiles";

        private static readonly Vector3Int[] NsewPositions =
        {
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0)
        };

        private string selectedFolder = "Assets/Art/Tiles/F1";
        private string result = "Select a PixelLab export folder.";
        private Vector2 scroll;

        [MenuItem("RIMA/Tile Import Wizard")]
        public static void Open()
        {
            var window = GetWindow<TileImportWizard>();
            window.titleContent = new GUIContent("Tile Import Wizard");
            window.minSize = new Vector2(560f, 320f);
            window.Show();
        }

        [MenuItem("RIMA/Tile Import Wizard/Create RuleTile F1 Wang Template")]
        public static void CreateTemplateMenu()
        {
            RuleTile template = CreateWangTemplateAsset();
            Selection.activeObject = template;
        }

        [MenuItem("RIMA/Tile Import Wizard/Import Generated Folder")]
        public static void ImportGeneratedMenu()
        {
            ImportFolder(GeneratedFolder, true);
        }

        public static string BatchImportDefaultF1()
        {
            return ImportFolder("Assets/Art/Tiles/F1", false);
        }

        public static string BatchImportGeneratedFolder()
        {
            return ImportFolder(GeneratedFolder, false);
        }

        public static RuleTile BatchCreateTemplate()
        {
            return CreateWangTemplateAsset();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Tile Import Wizard", EditorStyles.boldLabel);
            EditorGUILayout.Space(4f);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.TextField("Folder", selectedFolder);
                if (GUILayout.Button("Select", GUILayout.Width(96f)))
                {
                    string absolute = EditorUtility.OpenFolderPanel("Select PixelLab export folder", DefaultPickFolder, string.Empty);
                    if (!string.IsNullOrWhiteSpace(absolute))
                    {
                        selectedFolder = ToProjectPath(absolute) ?? absolute;
                    }
                }
            }

            if (GUILayout.Button("Import Wang Tileset", GUILayout.Height(30f)))
            {
                result = ImportFolder(selectedFolder, true);
            }

            if (GUILayout.Button("Create RuleTile_F1_Wang_Template"))
            {
                CreateWangTemplateAsset();
                result = $"Template ready: {TemplatePath}";
            }

            EditorGUILayout.Space(8f);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.HelpBox(result, MessageType.Info);
            EditorGUILayout.EndScrollView();
        }

        public static string ImportFolder(string folderPath, bool showDialogs = true)
        {
            try
            {
                string projectFolder = NormalizeProjectPath(folderPath);
                if (!Directory.Exists(projectFolder))
                {
                    return Fail($"Folder not found: {projectFolder}", showDialogs);
                }

                EnsureFolder(GeneratedFolder);
                CreateWangTemplateAsset();

                string[] jsonPaths = Directory.GetFiles(projectFolder, "*.json", SearchOption.TopDirectoryOnly)
                    .Select(NormalizeProjectPath)
                    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                if (jsonPaths.Length == 0)
                {
                    string migrated = TryImportExistingTileAssets(projectFolder);
                    if (!string.IsNullOrEmpty(migrated))
                    {
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        return migrated;
                    }

                    return Fail($"No PixelLab JSON metadata found in {projectFolder}.", showDialogs);
                }

                var outputs = new List<string>();
                for (int i = 0; i < jsonPaths.Length; i++)
                {
                    EditorUtility.DisplayProgressBar("Tile Import Wizard", $"Parsing {Path.GetFileName(jsonPaths[i])}", Progress(i, jsonPaths.Length, 0f));
                    if (!TryParseManifest(jsonPaths[i], out TileManifest manifest, out string parseError))
                    {
                        return Fail(parseError, showDialogs);
                    }

                    if (!IsWang(manifest.TileType))
                    {
                        continue;
                    }

                    string[] textures = Directory.GetFiles(Path.GetDirectoryName(jsonPaths[i]) ?? projectFolder, "*.png", SearchOption.TopDirectoryOnly)
                        .Select(NormalizeProjectPath)
                        .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    if (textures.Length == 0)
                    {
                        return Fail($"No PNG texture found beside {jsonPaths[i]}.", showDialogs);
                    }

                    foreach (string texturePath in textures)
                    {
                        string output = ImportTexture(manifest, texturePath, i, jsonPaths.Length);
                        outputs.Add(output);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                if (outputs.Count == 0)
                {
                    return Fail($"No Wang tileset metadata found in {projectFolder}.", showDialogs);
                }

                string message = "Imported RuleTiles:\n" + string.Join("\n", outputs);
                Debug.Log(message);
                return message;
            }
            catch (Exception ex)
            {
                return Fail($"Tile import failed: {ex.Message}", showDialogs);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static RuleTile CreateWangTemplateAsset()
        {
            EnsureFolder(GeneratedFolder);

            var template = ScriptableObject.CreateInstance<RuleTile>();
            template.name = Path.GetFileNameWithoutExtension(TemplatePath);
            template.m_DefaultColliderType = Tile.ColliderType.None;
            template.m_DefaultSprite = null;
            template.m_TilingRules.Clear();

            for (int mask = 0; mask < 16; mask++)
            {
                template.m_TilingRules.Add(CreateNsewRule(mask, null));
            }

            RuleTile existing = AssetDatabase.LoadAssetAtPath<RuleTile>(TemplatePath);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(template, TemplatePath);
                existing = template;
            }
            else
            {
                EditorUtility.CopySerialized(template, existing);
                UnityEngine.Object.DestroyImmediate(template);
            }

            EditorUtility.SetDirty(existing);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return existing;
        }

        private static string ImportTexture(TileManifest manifest, string texturePath, int jsonIndex, int jsonCount)
        {
            string textureName = Path.GetFileNameWithoutExtension(texturePath);
            string baseName = Sanitize(textureName);
            int tileSize = manifest.TileSize > 0 ? manifest.TileSize : DefaultTileSize;

            EditorUtility.DisplayProgressBar("Tile Import Wizard", $"Slicing {Path.GetFileName(texturePath)}", Progress(jsonIndex, jsonCount, 0.35f));
            var spriteMeta = manifest.Tiles
                .OrderBy(tile => tile.SortOrder)
                .Select(tile => new SpriteMetaData
                {
                    name = SpriteName(baseName, tile.Mask),
                    rect = new Rect(tile.X, tile.Y, tile.Width > 0 ? tile.Width : tileSize, tile.Height > 0 ? tile.Height : tileSize),
                    alignment = (int)SpriteAlignment.Center,
                    pivot = new Vector2(0.5f, 0.5f)
                })
                .ToArray();

            ConfigureTextureImporter(texturePath, spriteMeta, tileSize);

            EditorUtility.DisplayProgressBar("Tile Import Wizard", $"Creating RuleTile for {textureName}", Progress(jsonIndex, jsonCount, 0.75f));
            Dictionary<string, Sprite> sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(texturePath)
                .OfType<Sprite>()
                .ToDictionary(sprite => sprite.name, StringComparer.OrdinalIgnoreCase);

            Sprite[] maskSprites = new Sprite[16];
            foreach (TileEntry tile in manifest.Tiles)
            {
                sprites.TryGetValue(SpriteName(baseName, tile.Mask), out maskSprites[tile.Mask]);
            }

            string outputBaseName = baseName.StartsWith("wang_", StringComparison.OrdinalIgnoreCase) ? baseName : $"wang_{baseName}";
            string outputName = $"{outputBaseName}_RuleTile.asset";
            string outputPath = UniqueAssetPath($"{GeneratedFolder}/{outputName}");
            CreateRuleTileFromTemplate(Path.GetFileNameWithoutExtension(outputPath), maskSprites, outputPath);
            return outputPath;
        }

        private static string TryImportExistingTileAssets(string folderPath)
        {
            string[] tileGuids = AssetDatabase.FindAssets("t:TileBase", new[] { folderPath });
            var tileSet = tileGuids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => Regex.IsMatch(Path.GetFileNameWithoutExtension(path), @"^wang_.+_tile_\d+$"))
                .Select(path => new { Path = path, Tile = AssetDatabase.LoadAssetAtPath<Tile>(path) })
                .Where(item => item.Tile != null)
                .GroupBy(item => Regex.Replace(Path.GetFileNameWithoutExtension(item.Path), @"_tile_\d+$", string.Empty))
                .OrderByDescending(group => group.Count())
                .FirstOrDefault();

            if (tileSet == null || tileSet.Count() < 16)
            {
                return null;
            }

            Sprite[] spritesByMask = new Sprite[16];
            foreach (var item in tileSet)
            {
                Match match = Regex.Match(Path.GetFileNameWithoutExtension(item.Path), @"_tile_(\d+)$");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int index) && index >= 0 && index < spritesByMask.Length)
                {
                    spritesByMask[index] = item.Tile.sprite;
                }
            }

            string outputPath = UniqueAssetPath($"{GeneratedFolder}/{tileSet.Key}_RuleTile.asset");
            CreateRuleTileFromTemplate(Path.GetFileNameWithoutExtension(outputPath), spritesByMask, outputPath);
            string message = $"Imported existing tile assets:\n{outputPath}";
            Debug.Log(message);
            return message;
        }

        private static void ConfigureTextureImporter(string texturePath, SpriteMetaData[] spriteMeta, int tileSize)
        {
            var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null)
            {
                throw new InvalidDataException($"TextureImporter not found for {texturePath}.");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = tileSize;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
#pragma warning disable 0618
            importer.spritesheet = spriteMeta;
#pragma warning restore 0618
            importer.SaveAndReimport();
        }

        private static void CreateRuleTileFromTemplate(string assetName, Sprite[] spritesByMask, string outputPath)
        {
            RuleTile source = AssetDatabase.LoadAssetAtPath<RuleTile>(TemplatePath);
            if (source == null)
            {
                source = CreateWangTemplateAsset();
            }

            RuleTile tile = Instantiate(source);
            tile.name = assetName;
            tile.m_DefaultColliderType = Tile.ColliderType.None;
            tile.m_DefaultSprite = spritesByMask.FirstOrDefault(sprite => sprite != null);
            tile.m_TilingRules.Clear();

            for (int mask = 0; mask < 16; mask++)
            {
                tile.m_TilingRules.Add(CreateNsewRule(mask, spritesByMask[mask]));
            }

            AssetDatabase.CreateAsset(tile, outputPath);
        }

        private static RuleTile.TilingRule CreateNsewRule(int mask, Sprite sprite)
        {
            return new RuleTile.TilingRule
            {
                m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                m_Sprites = sprite == null ? Array.Empty<Sprite>() : new[] { sprite },
                m_ColliderType = Tile.ColliderType.None,
                m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed,
                m_NeighborPositions = new List<Vector3Int>(NsewPositions),
                m_Neighbors = new List<int>
                {
                    NeighborFor(mask, 0),
                    NeighborFor(mask, 1),
                    NeighborFor(mask, 2),
                    NeighborFor(mask, 3)
                }
            };
        }

        private static int NeighborFor(int mask, int bit)
        {
            return (mask & (1 << bit)) != 0
                ? RuleTile.TilingRuleOutput.Neighbor.This
                : RuleTile.TilingRuleOutput.Neighbor.NotThis;
        }

        private static bool TryParseManifest(string path, out TileManifest manifest, out string error)
        {
            manifest = null;
            error = null;

            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
            {
                error = $"{path}: JSON is empty.";
                return false;
            }

            if (json.IndexOf("\"tileset_data\"", StringComparison.Ordinal) >= 0)
            {
                TopdownDocument document = JsonUtility.FromJson<TopdownDocument>(json);
                if (document == null || document.tileset_data == null || document.tileset_data.tiles == null || document.tileset_data.tiles.Length == 0)
                {
                    error = $"{path}: malformed PixelLab tileset_data JSON.";
                    return false;
                }

                int tileSize = document.tileset_data.tile_size != null && document.tileset_data.tile_size.width > 0
                    ? document.tileset_data.tile_size.width
                    : DefaultTileSize;

                var tiles = new List<TileEntry>();
                foreach (TopdownTile tile in document.tileset_data.tiles)
                {
                    int mask = ParseTopdownMask(tile);
                    if (mask < 0 || mask > 15 || tile.bounding_box == null)
                    {
                        error = $"{path}: malformed Wang tile entry.";
                        return false;
                    }

                    tiles.Add(new TileEntry(mask, mask, tile.bounding_box.x, tile.bounding_box.y, tile.bounding_box.width, tile.bounding_box.height));
                }

                manifest = new TileManifest("topdown_wang", tileSize, tiles);
                return true;
            }

            FlatDocument flat = JsonUtility.FromJson<FlatDocument>(json);
            if (flat == null || string.IsNullOrWhiteSpace(flat.tile_type) || flat.tiles == null || flat.tiles.Length == 0)
            {
                error = $"{path}: expected tile_size, tile_type, and tiles[].";
                return false;
            }

            var flatTiles = new List<TileEntry>();
            foreach (FlatTile tile in flat.tiles)
            {
                if (tile.width <= 0 || tile.height <= 0)
                {
                    error = $"{path}: tile index {tile.index} has invalid width/height.";
                    return false;
                }

                int mask = ParseFlatMask(tile.wang_mask, tile.index);
                flatTiles.Add(new TileEntry(mask, tile.index, tile.sprite_x, tile.sprite_y, tile.width, tile.height));
            }

            manifest = new TileManifest(flat.tile_type, flat.tile_size > 0 ? flat.tile_size : DefaultTileSize, flatTiles);
            return true;
        }

        private static int ParseTopdownMask(TopdownTile tile)
        {
            int fromName = ParseTrailingNumber(tile.name);
            if (fromName >= 0)
            {
                return fromName;
            }

            int fromId = ParseTrailingNumber(tile.id);
            if (fromId >= 0)
            {
                return fromId;
            }

            return -1;
        }

        private static int ParseFlatMask(string wangMask, int fallback)
        {
            if (!string.IsNullOrWhiteSpace(wangMask) && wangMask.Length == 4)
            {
                int value = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (wangMask[i] == '1')
                    {
                        value |= 1 << i;
                    }
                }

                return Mathf.Clamp(value, 0, 15);
            }

            return Mathf.Clamp(fallback, 0, 15);
        }

        private static int ParseTrailingNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return -1;
            }

            Match match = Regex.Match(value, @"(\d+)$");
            return match.Success && int.TryParse(match.Groups[1].Value, out int result) ? result : -1;
        }

        private static bool IsWang(string tileType)
        {
            return tileType != null && tileType.IndexOf("wang", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string UniqueAssetPath(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) == null)
            {
                return path;
            }

            string directory = Path.GetDirectoryName(path)?.Replace('\\', '/') ?? GeneratedFolder;
            string name = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            string candidate = $"{directory}/{name}_new{extension}";
            int suffix = 2;

            while (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(candidate) != null)
            {
                candidate = $"{directory}/{name}_new_{suffix}{extension}";
                suffix++;
            }

            return candidate;
        }

        private static void EnsureFolder(string folderPath)
        {
            string normalized = NormalizeProjectPath(folderPath);
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

        private static string Fail(string message, bool showDialog)
        {
            Debug.LogError(message);
            if (showDialog)
            {
                EditorUtility.DisplayDialog("Tile Import Wizard", message, "OK");
            }

            return message;
        }

        private static float Progress(int index, int total, float localProgress)
        {
            return Mathf.Clamp01((index + localProgress) / Mathf.Max(1, total));
        }

        private static string SpriteName(string baseName, int mask)
        {
            return $"{baseName}_{mask:00}";
        }

        private static string ToProjectPath(string absolutePath)
        {
            string normalized = NormalizeSlashes(absolutePath);
            string projectRoot = NormalizeSlashes(Directory.GetCurrentDirectory());
            return normalized.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase)
                ? normalized.Substring(projectRoot.Length).TrimStart('/')
                : null;
        }

        private static string NormalizeProjectPath(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = NormalizeSlashes(value);
            string projectRoot = NormalizeSlashes(Directory.GetCurrentDirectory());
            return normalized.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase)
                ? normalized.Substring(projectRoot.Length).TrimStart('/')
                : normalized;
        }

        private static string NormalizeSlashes(string value)
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
            string sanitized = new string(value.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
            return string.IsNullOrWhiteSpace(sanitized) ? "tiles" : sanitized.Trim();
        }

        private sealed class TileManifest
        {
            public TileManifest(string tileType, int tileSize, List<TileEntry> tiles)
            {
                TileType = tileType;
                TileSize = tileSize;
                Tiles = tiles;
            }

            public string TileType { get; }
            public int TileSize { get; }
            public List<TileEntry> Tiles { get; }
        }

        private sealed class TileEntry
        {
            public TileEntry(int mask, int sortOrder, int x, int y, int width, int height)
            {
                Mask = Mathf.Clamp(mask, 0, 15);
                SortOrder = sortOrder;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public int Mask { get; }
            public int SortOrder { get; }
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }
        }

        [Serializable]
        private sealed class FlatDocument
        {
            public int tile_size;
            public string tile_type;
            public FlatTile[] tiles;
        }

        [Serializable]
        private sealed class FlatTile
        {
            public int index;
            public string wang_mask;
            public int sprite_x;
            public int sprite_y;
            public int width;
            public int height;
        }

        [Serializable]
        private sealed class TopdownDocument
        {
            public TopdownTilesetData tileset_data;
        }

        [Serializable]
        private sealed class TopdownTilesetData
        {
            public TopdownTile[] tiles;
            public TileSize tile_size;
        }

        [Serializable]
        private sealed class TopdownTile
        {
            public string id;
            public string name;
            public BoundingBox bounding_box;
        }

        [Serializable]
        private sealed class BoundingBox
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        [Serializable]
        private sealed class TileSize
        {
            public int width;
            public int height;
        }
    }
}
