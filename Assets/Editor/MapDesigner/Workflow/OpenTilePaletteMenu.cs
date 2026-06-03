namespace RIMA.MapDesigner.Editor.Workflow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using Object = UnityEngine.Object;

    public static class OpenTilePaletteMenu
    {
        private const int PixelsPerUnit = 32;
        private const string TileRoot = "Assets/Data/Brush/AssetParts_v3/CombatBiome_v15g/Tiles";
        private const string SpriteRoot = TileRoot + "/Sprites";
        private const string PaletteFolder = "Assets/Editor/TilePalettes";
        private const string PalettePath = PaletteFolder + "/RIMA_Combat_v15g.prefab";
        private const string WorkflowDocPath = "STAGING/UNITY_TILE_PALETTE_WORKFLOW.md";
        private const string DonePath = "STAGING/CODEX_TASK_enable_tile_palette_DONE.md";

        [MenuItem("RIMA/Legacy/Open Tile Palette")]
        public static void OpenTilePalette()
        {
            Type windowType = Type.GetType("UnityEditor.Tilemaps.GridPaintPaletteWindow,Unity.2D.Tilemap.Editor")
                ?? Type.GetType("UnityEditor.Tilemaps.GridPaintPaletteWindow,UnityEditor");
            if (windowType == null)
            {
                Debug.LogError("Unity Tile Palette window type was not found.");
                return;
            }

            EditorWindow.GetWindow(windowType);
        }

        [MenuItem("RIMA/Legacy/Rebuild v15g Tile Palette Assets")]
        public static void RebuildV15gTilePaletteAssets()
        {
            GenerateAssets();
        }

        public static void GenerateAssets()
        {
            EnsureFolder(TileRoot);
            EnsureFolder(SpriteRoot + "/Cobble");
            EnsureFolder(SpriteRoot + "/Dirt");
            EnsureFolder(PaletteFolder);

            List<Tile> cobbleTiles = ImportTileSet(
                "STAGING/pixellab_tiles_pro_pilot",
                SpriteRoot + "/Cobble",
                "RIMA_v15g_cobble",
                16);

            List<Tile> dirtTiles = ImportTileSet(
                "STAGING/pixellab_dirt_v1",
                SpriteRoot + "/Dirt",
                "RIMA_v15g_dirt",
                16);

            if (cobbleTiles.Count != 16 || dirtTiles.Count != 16)
            {
                throw new InvalidOperationException($"Expected 16 cobble and 16 dirt tiles, got {cobbleTiles.Count} cobble and {dirtTiles.Count} dirt.");
            }

            CreateRandomPool("WeightedRandomTile_dominant_pool.asset", cobbleTiles.Take(8).Select(tile => tile.sprite));
            CreateRandomPool("WeightedRandomTile_secondary_pool.asset", cobbleTiles.Skip(8).Take(4).Select(tile => tile.sprite));
            CreateRandomPool("WeightedRandomTile_dirt_pool.asset", dirtTiles.Select(tile => tile.sprite));
            CreatePalette(cobbleTiles, dirtTiles);
            WriteWorkflowDoc();
            WriteDoneMarker(cobbleTiles.Count + dirtTiles.Count);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"RIMA v15g Tile Palette assets rebuilt: {PalettePath}");
        }

        private static List<Tile> ImportTileSet(string sourceFolder, string spriteFolder, string prefix, int expectedCount)
        {
            string[] sourceFiles = Directory.GetFiles(sourceFolder, "*.png", SearchOption.TopDirectoryOnly)
                .OrderBy(NaturalTileOrder)
                .ToArray();

            if (sourceFiles.Length == 0)
            {
                throw new FileNotFoundException($"No PNG files found under {sourceFolder}.");
            }

            var result = new List<Tile>(expectedCount);
            for (int i = 0; i < expectedCount; i++)
            {
                string source = sourceFiles[i % sourceFiles.Length];
                string spritePath = $"{spriteFolder}/{prefix}_{i:00}.png";
                File.Copy(source, spritePath, true);
                AssetDatabase.ImportAsset(spritePath, ImportAssetOptions.ForceUpdate);
                ConfigureSpriteImport(spritePath);

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (sprite == null)
                {
                    throw new InvalidOperationException($"Sprite import failed for {spritePath}.");
                }

                string tilePath = $"{TileRoot}/{prefix}_{i:00}.asset";
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = sprite;
                tile.colliderType = Tile.ColliderType.None;
                ReplaceAsset(tile, tilePath);
                result.Add(tile);
            }

            return result;
        }

        private static int NaturalTileOrder(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            int underscore = fileName.LastIndexOf('_');
            if (underscore >= 0 && int.TryParse(fileName.Substring(underscore + 1), out int value))
            {
                return value;
            }

            return int.MaxValue;
        }

        private static void ConfigureSpriteImport(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                throw new IOException($"TextureImporter not found for {assetPath}.");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = PixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.SaveAndReimport();
        }

        private static void CreateRandomPool(string assetName, IEnumerable<Sprite> sprites)
        {
            Sprite[] spriteArray = sprites.Where(sprite => sprite != null).ToArray();
            var randomTile = ScriptableObject.CreateInstance<RandomTile>();
            randomTile.sprites = spriteArray;
            randomTile.weights = Enumerable.Repeat(1f, spriteArray.Length).ToArray();
            randomTile.colliderType = Tile.ColliderType.None;
            ReplaceAsset(randomTile, $"{TileRoot}/{assetName}");
        }

        private static void CreatePalette(IReadOnlyList<Tile> cobbleTiles, IReadOnlyList<Tile> dirtTiles)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(PalettePath) != null)
            {
                AssetDatabase.DeleteAsset(PalettePath);
            }

            GameObject palette = CreateStandardPalette();
            if (palette == null)
            {
                throw new InvalidOperationException("Unity standard Tile Palette prefab creation failed.");
            }

            GameObject root = PrefabUtility.LoadPrefabContents(PalettePath);
            Tilemap tilemap = root.GetComponentInChildren<Tilemap>();
            if (tilemap == null)
            {
                throw new InvalidOperationException("Created Tile Palette prefab has no Tilemap layer.");
            }

            tilemap.ClearAllTiles();
            SetRow(tilemap, cobbleTiles.Take(8), 0);
            SetRow(tilemap, cobbleTiles.Skip(8).Take(4), -2);
            SetRow(tilemap, dirtTiles, -4);
            SetRow(tilemap, cobbleTiles.Skip(12).Take(4), -6);
            tilemap.CompressBounds();

            PrefabUtility.SaveAsPrefabAsset(root, PalettePath);
            PrefabUtility.UnloadPrefabContents(root);
        }

        private static GameObject CreateStandardPalette()
        {
            Type utilityType = Type.GetType("UnityEditor.Tilemaps.GridPaletteUtility,Unity.2D.Tilemap.Editor");
            Type gridPaletteType = Type.GetType("UnityEditor.GridPalette,UnityEditor.TilemapModule")
                ?? Type.GetType("UnityEditor.GridPalette,Unity.2D.Tilemap.Editor")
                ?? Type.GetType("UnityEditor.Tilemaps.GridPalette,Unity.2D.Tilemap.Editor");
            Type cellSizingType = gridPaletteType?.GetNestedType("CellSizing", BindingFlags.Public | BindingFlags.NonPublic);
            if (utilityType == null || cellSizingType == null)
            {
                throw new InvalidOperationException("Unity GridPaletteUtility or GridPalette.CellSizing type was not found.");
            }

            object cellSizing = Enum.Parse(cellSizingType, "Manual");
            MethodInfo method = utilityType.GetMethod(
                "CreateNewPalette",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(string), typeof(GridLayout.CellLayout), cellSizingType, typeof(Vector3), typeof(GridLayout.CellSwizzle) },
                null);

            if (method == null)
            {
                throw new MissingMethodException("GridPaletteUtility.CreateNewPalette overload was not found.");
            }

            return method.Invoke(
                null,
                new object[]
                {
                    PaletteFolder,
                    "RIMA_Combat_v15g",
                    GridLayout.CellLayout.Rectangle,
                    cellSizing,
                    Vector3.one,
                    GridLayout.CellSwizzle.XYZ
                }) as GameObject;
        }

        private static void SetRow(Tilemap tilemap, IEnumerable<Tile> tiles, int y)
        {
            int x = 0;
            foreach (Tile tile in tiles)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                x++;
            }
        }

        private static void ReplaceAsset(Object asset, string assetPath)
        {
            Object existing = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (existing != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(asset, assetPath);
        }

        private static void EnsureFolder(string path)
        {
            string normalized = path.Replace('\\', '/');
            string[] parts = normalized.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        private static void WriteWorkflowDoc()
        {
            string markdown =
                "# Unity Tile Palette Workflow - RIMA Combat v15g\n\n" +
                "Use Blueprint Painter for STRUCTURE and Unity Tile Palette for POLISH.\n\n" +
                "## Open\n" +
                "- Open `Window > 2D > Tile Palette`.\n" +
                "- Or use `Tools > RIMA > Map Designer > Open Tile Palette`.\n\n" +
                "## Select Palette\n" +
                "- Use the dropdown at the top of the Tile Palette window.\n" +
                "- Select `RIMA_Combat_v15g`.\n\n" +
                "## Paint\n" +
                "- Select the brush tool.\n" +
                "- Click a tile in the palette to switch the active tile.\n" +
                "- Click or drag on the target Tilemap for immediate visual feedback.\n\n" +
                "## Tools\n" +
                "- Erase: select the eraser toolbar button and drag on the Tilemap.\n" +
                "- Bucket fill: select the fill toolbar button and click a connected area.\n" +
                "- Rectangle: select the rectangle toolbar button and drag an area.\n" +
                "- Line: use the line toolbar button when available in the Tile Palette toolbar.\n\n" +
                "## Categories\n" +
                "- Floor: top row, cobble dominant variants.\n" +
                "- Path: second row, cobble secondary/path variants.\n" +
                "- Dirt: third row, dirt variants.\n" +
                "- Transition: fourth row, cobble transition candidates.\n";

            File.WriteAllText(WorkflowDocPath, markdown);
        }

        private static void WriteDoneMarker(int tileAssetCount)
        {
            int dirtSourceCount = Directory.GetFiles("STAGING/pixellab_dirt_v1", "*.png", SearchOption.TopDirectoryOnly).Length;
            string markdown =
                "# DONE - Enable Unity Tile Palette Direct Paint Workflow\n\n" +
                $"- Tile asset count: {tileAssetCount} standard Tile assets, plus 3 weighted RandomTile pool assets.\n" +
                $"- Palette path: `{PalettePath}`\n" +
                $"- Workflow doc path: `{WorkflowDocPath}`\n" +
                $"- Tile asset folder: `{TileRoot}`\n" +
                $"- Source note: `STAGING/pixellab_dirt_v1/` contained {dirtSourceCount} PNGs, so those PNGs were repeated to fill 16 dirt Tile assets without external generation.\n";

            File.WriteAllText(DonePath, markdown);
        }
    }
}
