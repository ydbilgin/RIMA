namespace RIMA.Editor.RoomDesigner.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Tilemaps;
    using Object = UnityEngine.Object;

    public sealed class TileImportWizard : EditorWindow
    {
        public const string GeneratedFolder = "Assets/Art/Tiles/F1/Generated";
        public const string TemplatePath = GeneratedFolder + "/RuleTile_F1_Wang_Template.asset";
        public const string DemoScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";

        private const int DefaultTileSize = 32;
        private const int PixelsPerUnit = 32;
        private const string DefaultFolder = "Assets/Art/Tiles/F1";

        private static readonly Vector3Int[] NsewPositions =
        {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, 0, 0)
        };

        private static readonly Vector3Int[] EightWayPositions =
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

        private string selectedFolder = DefaultFolder;
        private string lastResult = "Select a PixelLab export folder.";
        private Vector2 scroll;

        [MenuItem("RIMA/Tile Import Wizard")]
        public static void Open()
        {
            var window = GetWindow<TileImportWizard>();
            window.titleContent = new GUIContent("Tile Import Wizard");
            window.minSize = new Vector2(560f, 360f);
            window.Show();
        }

        [MenuItem("RIMA/Tile Import Wizard/Create Wang Template")]
        public static void CreateWangTemplateFromMenu()
        {
            CreateWangTemplateAsset();
        }

        [MenuItem("RIMA/Tile Import Wizard/Apply Demo 4-Layer Stack")]
        public static void ApplyDemoStackFromMenu()
        {
            ApplyFourLayerStackToDemoScene();
        }

        public static void BatchCreateTemplate()
        {
            CreateWangTemplateAsset();
        }

        public static void BatchApplyDemoStack()
        {
            ApplyFourLayerStackToDemoScene();
        }

        public static void BatchImportDefaultF1()
        {
            ImportFolder("Assets/Art/Tiles/F1");
        }

        public static void BatchImportGeneratedTileAssets()
        {
            ImportFolder(GeneratedFolder);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("PixelLab Tile Import Wizard", EditorStyles.boldLabel);
            EditorGUILayout.Space(4f);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.TextField("Folder", selectedFolder);
                if (GUILayout.Button("Select", GUILayout.Width(96f)))
                {
                    string absolute = EditorUtility.OpenFolderPanel("PixelLab Export Folder", DefaultFolder, string.Empty);
                    if (!string.IsNullOrEmpty(absolute))
                    {
                        string projectPath = ToProjectPath(absolute);
                        selectedFolder = string.IsNullOrEmpty(projectPath) ? absolute : projectPath;
                    }
                }
            }

            if (GUILayout.Button("Import Folder", GUILayout.Height(30f)))
            {
                try
                {
                    lastResult = ImportFolder(selectedFolder);
                }
                catch (Exception ex)
                {
                    lastResult = ex.Message;
                    Debug.LogError($"Tile Import Wizard failed: {ex}");
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Create Template"))
                {
                    CreateWangTemplateAsset();
                }

                if (GUILayout.Button("Apply Demo Stack"))
                {
                    ApplyFourLayerStackToDemoScene();
                }
            }

            EditorGUILayout.Space(8f);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.HelpBox(lastResult, MessageType.Info);
            EditorGUILayout.EndScrollView();
        }

        public static string ImportFolder(string folderPath)
        {
            string projectFolder = NormalizeProjectPath(folderPath);
            EnsureFolder(GeneratedFolder);
            CreateWangTemplateAsset();

            string manifestPath = FindManifest(projectFolder);
            if (!string.IsNullOrEmpty(manifestPath))
            {
                string texturePath = FindTextureForManifest(projectFolder, manifestPath);
                if (string.IsNullOrEmpty(texturePath))
                {
                    throw new InvalidDataException($"No PNG sheet found beside {manifestPath}.");
                }

                return ImportManifest(projectFolder, manifestPath, texturePath);
            }

            string migrated = TryImportExistingTileAssets(projectFolder);
            if (!string.IsNullOrEmpty(migrated))
            {
                return migrated;
            }

            throw new InvalidDataException($"No asset_000.json, metadata.json, or wang_*_tile_*.asset set found in {projectFolder}.");
        }

        public static string ImportManifest(string folderPath, string manifestPath, string texturePath)
        {
            string json = File.ReadAllText(manifestPath);
            if (json.IndexOf("\"tileset_data\"", StringComparison.Ordinal) >= 0)
            {
                var document = JsonUtility.FromJson<TopdownTilesetDocument>(json);
                ValidateTopdownDocument(document, manifestPath);
                ConfigureTextureImporter(texturePath, document.tileset_data.tiles, document.tileset_data.tile_size.width, Path.GetFileNameWithoutExtension(texturePath));
                Sprite[] sprites = LoadSlicedSprites(texturePath, document.tileset_data.tiles.Length);
                string outputPath = $"{GeneratedFolder}/{Path.GetFileNameWithoutExtension(texturePath)}_wizard_RuleTile.asset";
                CreateTopdownRuleTile(document, sprites, outputPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                string result = $"Imported topdown Wang tileset: {outputPath}";
                Debug.Log(result);
                return result;
            }

            var flat = JsonUtility.FromJson<FlatExportDocument>(json);
            ValidateFlatDocument(flat, manifestPath);
            ConfigureTextureImporter(texturePath, flat.tiles, flat.tile_size > 0 ? flat.tile_size : DefaultTileSize, Path.GetFileNameWithoutExtension(texturePath));
            Sprite[] flatSprites = LoadSlicedSprites(texturePath, flat.tiles.Length);

            if (IsWang(flat.tile_type))
            {
                string outputPath = $"{GeneratedFolder}/{Path.GetFileNameWithoutExtension(texturePath)}_wizard_RuleTile.asset";
                CreateFlatRuleTile(flat, flatSprites, outputPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                string result = $"Imported create_tiles_pro Wang tileset: {outputPath}";
                Debug.Log(result);
                return result;
            }

            string tileFolder = $"{GeneratedFolder}/{Sanitize(Path.GetFileNameWithoutExtension(texturePath))}_Tiles";
            EnsureFolder(tileFolder);
            for (int i = 0; i < flatSprites.Length; i++)
            {
                CreateTileAsset(flatSprites[i], $"{tileFolder}/{flatSprites[i].name}.asset", Tile.ColliderType.None);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            string nonWangResult = $"Imported {flat.tile_type} sprite tiles: {tileFolder}";
            Debug.Log(nonWangResult);
            return nonWangResult;
        }

        public static RuleTile CreateWangTemplateAsset()
        {
            EnsureFolder(GeneratedFolder);
            var template = ScriptableObject.CreateInstance<RuleTile>();
            template.name = Path.GetFileNameWithoutExtension(TemplatePath);
            template.m_DefaultColliderType = Tile.ColliderType.None;

            for (int mask = 0; mask < 16; mask++)
            {
                template.m_TilingRules.Add(CreateNsewRule(mask, null, Tile.ColliderType.None));
            }

            ReplaceAsset(template, TemplatePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return template;
        }

        public static void ApplyFourLayerStackToDemoScene()
        {
            Scene scene = EditorSceneManager.OpenScene(DemoScenePath, OpenSceneMode.Single);
            GameObject gridObject = GameObject.Find("Grid");
            if (gridObject == null)
            {
                gridObject = new GameObject("Grid");
                gridObject.AddComponent<Grid>();
            }

            Grid grid = gridObject.GetComponent<Grid>();
            if (grid == null)
            {
                grid = gridObject.AddComponent<Grid>();
            }
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
            grid.cellSize = new Vector3(1f, 0.5f, 0f);

            RenameChild(gridObject.transform, "FloorTilemap", "BaseTilemap");
            RenameChild(gridObject.transform, "DecalsTilemap", "DecalTilemap");
            RenameChild(gridObject.transform, "WallsTilemap", "WallsTilemap_Front");

            EnsureTilemap(gridObject.transform, "BaseTilemap", 0, false);
            EnsureTilemap(gridObject.transform, "DecalTilemap", 1, false);
            EnsureTilemap(gridObject.transform, "WallsTilemap_Front", 2, true);
            EnsureTilemap(gridObject.transform, "WallsTilemap_Top", 3, false);
            EnsureChild(gridObject.transform, "PropContainer");

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("RoomPipelineTest demo scene updated with Base/Decal/Wall_Front/Wall_Top/Prop stack.");
        }

        private static string TryImportExistingTileAssets(string folderPath)
        {
            string[] tileGuids = AssetDatabase.FindAssets("t:TileBase", new[] { folderPath });
            var tiles = tileGuids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => Regex.IsMatch(Path.GetFileNameWithoutExtension(path), @"^wang_.+_tile_\d+$"))
                .Select(path => new { Path = path, Tile = AssetDatabase.LoadAssetAtPath<Tile>(path) })
                .Where(item => item.Tile != null)
                .ToList();

            if (tiles.Count < 16)
            {
                return null;
            }

            string baseName = Regex.Replace(Path.GetFileNameWithoutExtension(tiles[0].Path), @"_tile_\d+$", string.Empty);
            Sprite[] sprites = tiles
                .OrderBy(item => TileIndex(item.Path))
                .Take(16)
                .Select(item => item.Tile.sprite)
                .ToArray();
            if (sprites.All(sprite => sprite == null))
            {
                string siblingTexture = $"Assets/Art/Tiles/F1/{baseName}.png";
                if (File.Exists(siblingTexture))
                {
                    sprites = LoadSlicedSprites(siblingTexture, 16);
                }
            }

            string outputPath = $"{GeneratedFolder}/{baseName}_tileassets_wizard_RuleTile.asset";
            CreateNsewRuleTileFromTemplate(baseName + "_wizard_RuleTile", sprites, outputPath, Tile.ColliderType.None);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            string result = $"Migrated existing tile assets into {outputPath}";
            Debug.Log(result);
            return result;
        }

        private static void CreateTopdownRuleTile(TopdownTilesetDocument document, Sprite[] sprites, string outputPath)
        {
            var tile = ScriptableObject.CreateInstance<RuleTile>();
            tile.name = Path.GetFileNameWithoutExtension(outputPath);
            tile.m_DefaultColliderType = Tile.ColliderType.None;

            TopdownTile allLower = document.tileset_data.tiles.FirstOrDefault(IsAllLower);
            tile.m_DefaultSprite = allLower != null ? sprites[Mathf.Clamp(SpriteIndexFor(allLower), 0, sprites.Length - 1)] : sprites[0];

            foreach (TopdownTile item in document.tileset_data.tiles.OrderByDescending(Specificity))
            {
                int index = Mathf.Clamp(SpriteIndexFor(item), 0, sprites.Length - 1);
                var rule = new RuleTile.TilingRule
                {
                    m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                    m_Sprites = new[] { sprites[index] },
                    m_ColliderType = Tile.ColliderType.None,
                    m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed,
                    m_NeighborPositions = new List<Vector3Int>(EightWayPositions),
                    m_Neighbors = BuildCornerNeighborMask(item)
                };
                tile.m_TilingRules.Add(rule);
            }

            ReplaceAsset(tile, outputPath);
        }

        private static void CreateFlatRuleTile(FlatExportDocument document, Sprite[] sprites, string outputPath)
        {
            var ordered = document.tiles
                .Select((tile, i) => new { Tile = tile, Sprite = sprites[Mathf.Clamp(i, 0, sprites.Length - 1)] })
                .OrderBy(item => item.Tile.index)
                .ToArray();

            var byMask = new Dictionary<int, Sprite>();
            for (int i = 0; i < ordered.Length; i++)
            {
                int mask = ParseMask(ordered[i].Tile.wang_mask, ordered[i].Tile.index);
                byMask[mask] = ordered[i].Sprite;
            }

            Sprite[] nsewSprites = new Sprite[16];
            for (int mask = 0; mask < 16; mask++)
            {
                nsewSprites[mask] = byMask.TryGetValue(mask, out Sprite sprite) ? sprite : ordered[Mathf.Clamp(mask, 0, ordered.Length - 1)].Sprite;
            }

            CreateNsewRuleTileFromTemplate(Path.GetFileNameWithoutExtension(outputPath), nsewSprites, outputPath, Tile.ColliderType.None);
        }

        private static void CreateNsewRuleTileFromTemplate(string name, Sprite[] sprites, string outputPath, Tile.ColliderType colliderType)
        {
            RuleTile source = AssetDatabase.LoadAssetAtPath<RuleTile>(TemplatePath);
            if (source == null)
            {
                source = CreateWangTemplateAsset();
            }

            RuleTile tile = Instantiate(source);
            tile.name = name;
            tile.m_DefaultSprite = sprites.FirstOrDefault(sprite => sprite != null);
            tile.m_DefaultColliderType = colliderType;
            tile.m_TilingRules.Clear();

            for (int mask = 0; mask < 16; mask++)
            {
                tile.m_TilingRules.Add(CreateNsewRule(mask, sprites[Mathf.Clamp(mask, 0, sprites.Length - 1)], colliderType));
            }

            ReplaceAsset(tile, outputPath);
        }

        private static RuleTile.TilingRule CreateNsewRule(int mask, Sprite sprite, Tile.ColliderType colliderType)
        {
            return new RuleTile.TilingRule
            {
                m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                m_Sprites = sprite == null ? Array.Empty<Sprite>() : new[] { sprite },
                m_ColliderType = colliderType,
                m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed,
                m_NeighborPositions = new List<Vector3Int>(NsewPositions),
                m_Neighbors = new List<int>
                {
                    (mask & 1) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis,
                    (mask & 2) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis,
                    (mask & 4) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis,
                    (mask & 8) != 0 ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis
                }
            };
        }

        private static void ConfigureTextureImporter(string texturePath, TopdownTile[] tiles, int tileSize, string baseName)
        {
            var metadata = new SpriteMetaData[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                TopdownTile tile = tiles[i];
                int index = SpriteIndexFor(tile);
                metadata[i] = CreateSpriteMetaData(baseName, index, tile.bounding_box.x, tile.bounding_box.y, tile.bounding_box.width, tile.bounding_box.height);
            }

            ConfigureImporter(texturePath, metadata, tileSize);
        }

        private static void ConfigureTextureImporter(string texturePath, FlatTile[] tiles, int tileSize, string baseName)
        {
            var metadata = new SpriteMetaData[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                FlatTile tile = tiles[i];
                metadata[i] = CreateSpriteMetaData(baseName, tile.index, tile.sprite_x, tile.sprite_y, tile.width > 0 ? tile.width : tileSize, tile.height > 0 ? tile.height : tileSize);
            }

            ConfigureImporter(texturePath, metadata, tileSize);
        }

        private static void ConfigureImporter(string texturePath, SpriteMetaData[] metadata, int tileSize)
        {
            var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null)
            {
                throw new IOException($"TextureImporter not found for {texturePath}.");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = tileSize > 0 ? tileSize : PixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.spritesheet = metadata;
            importer.SaveAndReimport();
        }

        private static SpriteMetaData CreateSpriteMetaData(string baseName, int index, int x, int y, int width, int height)
        {
            return new SpriteMetaData
            {
                name = $"{Sanitize(baseName)}_{index:00}",
                alignment = (int)SpriteAlignment.Center,
                pivot = new Vector2(0.5f, 0.5f),
                rect = new Rect(x, y, width, height)
            };
        }

        private static Sprite[] LoadSlicedSprites(string texturePath, int expectedCount)
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(texturePath)
                .OfType<Sprite>()
                .OrderBy(sprite => sprite.name, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (sprites.Length != expectedCount)
            {
                throw new InvalidDataException($"Expected {expectedCount} sliced sprites in {texturePath}, found {sprites.Length}.");
            }

            return sprites;
        }

        private static void CreateTileAsset(Sprite sprite, string assetPath, Tile.ColliderType colliderType)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            tile.colliderType = colliderType;
            ReplaceAsset(tile, assetPath);
        }

        private static void EnsureTilemap(Transform parent, string name, int sortingOrder, bool collider)
        {
            GameObject go = EnsureChild(parent, name);
            if (go.GetComponent<Tilemap>() == null)
            {
                go.AddComponent<Tilemap>();
            }

            TilemapRenderer renderer = go.GetComponent<TilemapRenderer>();
            if (renderer == null)
            {
                renderer = go.AddComponent<TilemapRenderer>();
            }

            renderer.sortingOrder = sortingOrder;

            TilemapCollider2D existingCollider = go.GetComponent<TilemapCollider2D>();
            if (collider && existingCollider == null)
            {
                go.AddComponent<TilemapCollider2D>();
            }
            else if (!collider && existingCollider != null)
            {
                DestroyImmediate(existingCollider);
            }
        }

        private static GameObject EnsureChild(Transform parent, string name)
        {
            Transform existing = parent.Find(name);
            if (existing != null)
            {
                return existing.gameObject;
            }

            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go;
        }

        private static void RenameChild(Transform parent, string oldName, string newName)
        {
            Transform child = parent.Find(oldName);
            if (child != null)
            {
                child.name = newName;
            }
        }

        private static string FindManifest(string folderPath)
        {
            foreach (string file in new[] { "asset_000.json", "metadata.json" })
            {
                string path = $"{folderPath}/{file}";
                if (File.Exists(path))
                {
                    return NormalizeProjectPath(path);
                }
            }

            return Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly).Select(NormalizeProjectPath).FirstOrDefault()
                : null;
        }

        private static string FindTextureForManifest(string folderPath, string manifestPath)
        {
            string manifestDirectory = NormalizeProjectPath(Path.GetDirectoryName(manifestPath));
            string preferred = Directory.GetFiles(manifestDirectory, "*.png", SearchOption.TopDirectoryOnly)
                .Select(NormalizeProjectPath)
                .FirstOrDefault();
            return preferred ?? Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories).Select(NormalizeProjectPath).FirstOrDefault();
        }

        private static void ValidateTopdownDocument(TopdownTilesetDocument document, string path)
        {
            if (document == null || document.tileset_data == null || document.tileset_data.tiles == null)
            {
                throw new InvalidDataException($"{path}: missing tileset_data.tiles.");
            }

            if (document.tileset_data.tile_size == null || document.tileset_data.tile_size.width <= 0 || document.tileset_data.tile_size.height <= 0)
            {
                throw new InvalidDataException($"{path}: missing valid tile_size.");
            }

            if (document.tileset_data.tiles.Length != 16 && document.tileset_data.tiles.Length != 47)
            {
                throw new InvalidDataException($"{path}: expected 16 or 47 topdown Wang tiles, found {document.tileset_data.tiles.Length}.");
            }
        }

        private static void ValidateFlatDocument(FlatExportDocument document, string path)
        {
            if (document == null || string.IsNullOrWhiteSpace(document.tile_type) || document.tiles == null || document.tiles.Length == 0)
            {
                throw new InvalidDataException($"{path}: expected tile_size, tile_type, and tiles[].");
            }

            foreach (FlatTile tile in document.tiles)
            {
                if (tile.width <= 0 || tile.height <= 0)
                {
                    throw new InvalidDataException($"{path}: tile index {tile.index} has invalid width/height.");
                }
            }
        }

        private static List<int> BuildCornerNeighborMask(TopdownTile tile)
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
            if (IsUpper(a) && IsUpper(b)) return RuleTile.TilingRuleOutput.Neighbor.This;
            if (!IsUpper(a) && !IsUpper(b)) return RuleTile.TilingRuleOutput.Neighbor.NotThis;
            return 0;
        }

        private static int CornerMatch(string value)
        {
            return IsUpper(value) ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis;
        }

        private static int Specificity(TopdownTile tile)
        {
            return BuildCornerNeighborMask(tile).Count(value => value != 0);
        }

        private static int SpriteIndexFor(TopdownTile tile)
        {
            int tileSize = tile.bounding_box.width > 0 ? tile.bounding_box.width : DefaultTileSize;
            int column = Mathf.Max(0, tile.bounding_box.x / tileSize);
            int row = Mathf.Max(0, tile.bounding_box.y / tileSize);
            return row * 4 + column;
        }

        private static bool IsAllLower(TopdownTile tile)
        {
            CornerSet c = tile.corners;
            return !IsUpper(c.NE) && !IsUpper(c.NW) && !IsUpper(c.SE) && !IsUpper(c.SW);
        }

        private static bool IsUpper(string value)
        {
            return string.Equals(value, "upper", StringComparison.OrdinalIgnoreCase);
        }

        private static int ParseMask(string mask, int fallback)
        {
            if (!string.IsNullOrWhiteSpace(mask) && mask.Length == 4)
            {
                int result = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (mask[i] == '1')
                    {
                        result |= 1 << i;
                    }
                }

                return result;
            }

            return Mathf.Clamp(fallback, 0, 15);
        }

        private static bool IsWang(string tileType)
        {
            return tileType != null && tileType.IndexOf("wang", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static int TileIndex(string path)
        {
            Match match = Regex.Match(Path.GetFileNameWithoutExtension(path), @"_tile_(\d+)$");
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
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
            if (string.IsNullOrEmpty(value))
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

        [Serializable]
        public sealed class FlatExportDocument
        {
            public int tile_size;
            public string tile_type;
            public FlatTile[] tiles;
        }

        [Serializable]
        public sealed class FlatTile
        {
            public int index;
            public string wang_mask;
            public int sprite_x;
            public int sprite_y;
            public int width;
            public int height;
        }

        [Serializable]
        public sealed class TopdownTilesetDocument
        {
            public TopdownTilesetData tileset_data;
        }

        [Serializable]
        public sealed class TopdownTilesetData
        {
            public TopdownTile[] tiles;
            public TileSize tile_size;
        }

        [Serializable]
        public sealed class TopdownTile
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
        public sealed class TileSize
        {
            public int width;
            public int height;
        }
    }
}
