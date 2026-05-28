namespace RIMA.Editor.TileImport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class TileImportWizard : EditorWindow
    {
        private const int PPU = 64;
        private const int SingleSize = 32;
        private const int SheetSize = 128;
        private const int SheetColumns = 4;
        private const int SheetRows = 4;
        private const int TilePixels = 32;
        private const string TileRoot = "Assets/Art/Tiles";

        private static readonly string[] Categories = { "floor", "wall", "decal", "obstacle" };
        private static readonly Color32 MortarColor = new Color32(0x1A, 0x1C, 0x20, 0xFF);

        private readonly List<ValidationMessage> validationMessages = new List<ValidationMessage>();

        private InputMode mode = InputMode.Single;
        private Texture2D sourceTexture;
        private string sourcePath = string.Empty;
        private string baseName = string.Empty;
        private int categoryIndex;
        private bool convertChromakey = true;
        private bool autoFixImporter = true;
        private bool hasChromakey;
        private bool hasErrors;
        private Vector2 validationScroll;

        private enum InputMode
        {
            Single,
            Sheet
        }

        private enum ValidationSeverity
        {
            Info,
            Warning,
            Error
        }

        // [MenuItem removed — legacy, replaced by RIMA/Room Painter Tools]
        public static void Open()
        {
            var window = GetWindow<TileImportWizard>();
            window.titleContent = new GUIContent("Tile Import Wizard");
            window.minSize = new Vector2(520f, 560f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Tile Import Wizard", EditorStyles.boldLabel);
            DrawModeSelector();

            sourceTexture = (Texture2D)EditorGUILayout.ObjectField("Source PNG", sourceTexture, typeof(Texture2D), false);
            categoryIndex = EditorGUILayout.Popup("Category", categoryIndex, Categories);
            baseName = EditorGUILayout.TextField("Base Name", baseName);

            if (EditorGUI.EndChangeCheck())
            {
                SyncSourcePathAndName();
                ValidateSource();
            }

            EditorGUILayout.Space(6f);
            DrawPreviewPanel();
            EditorGUILayout.Space(6f);
            DrawValidationPanel();

            using (new EditorGUI.DisabledScope(sourceTexture == null))
            {
                if (GUILayout.Button("Import", GUILayout.Height(30f)))
                {
                    ImportSelectedTexture();
                }
            }
        }

        private void DrawModeSelector()
        {
            EditorGUILayout.LabelField("Input Mode");
            EditorGUILayout.BeginHorizontal();
            InputMode selectedMode = mode;
            if (GUILayout.Toggle(mode == InputMode.Single, "Single (32x32 PNG)", EditorStyles.radioButton))
            {
                selectedMode = InputMode.Single;
            }

            if (GUILayout.Toggle(mode == InputMode.Sheet, "Sheet (128x128 PNG)", EditorStyles.radioButton))
            {
                selectedMode = InputMode.Sheet;
            }

            mode = selectedMode;
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPreviewPanel()
        {
            Rect previewRect = GUILayoutUtility.GetRect(180f, 180f, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(previewRect, new Color(0.07f, 0.08f, 0.09f, 1f));

            if (sourceTexture == null)
            {
                GUI.Label(previewRect, "Select a PNG texture", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            Rect imageRect = FitRect(previewRect, sourceTexture.width, sourceTexture.height, 8f);
            EditorGUI.DrawPreviewTexture(imageRect, sourceTexture, null, ScaleMode.ScaleToFit);

            if (mode == InputMode.Sheet)
            {
                Handles.BeginGUI();
                Color previous = Handles.color;
                Handles.color = new Color(1f, 1f, 1f, 0.55f);
                for (int i = 1; i < SheetColumns; i++)
                {
                    float x = imageRect.xMin + imageRect.width * i / SheetColumns;
                    Handles.DrawLine(new Vector3(x, imageRect.yMin), new Vector3(x, imageRect.yMax));
                }

                for (int i = 1; i < SheetRows; i++)
                {
                    float y = imageRect.yMin + imageRect.height * i / SheetRows;
                    Handles.DrawLine(new Vector3(imageRect.xMin, y), new Vector3(imageRect.xMax, y));
                }

                Handles.color = previous;
                Handles.EndGUI();
            }
        }

        private void DrawValidationPanel()
        {
            EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
            validationScroll = EditorGUILayout.BeginScrollView(validationScroll, GUILayout.MinHeight(130f), GUILayout.MaxHeight(180f));

            if (validationMessages.Count == 0)
            {
                EditorGUILayout.HelpBox("No validation messages.", MessageType.Info);
            }
            else
            {
                foreach (ValidationMessage message in validationMessages)
                {
                    EditorGUILayout.HelpBox(message.Text, ToMessageType(message.Severity));
                }
            }

            EditorGUILayout.EndScrollView();

            if (hasChromakey)
            {
                convertChromakey = EditorGUILayout.ToggleLeft("Convert chromakey #00FF00 to transparent alpha", convertChromakey);
            }

            autoFixImporter = EditorGUILayout.ToggleLeft("Auto-fix TextureImporter settings", autoFixImporter);
        }

        private void SyncSourcePathAndName()
        {
            sourcePath = sourceTexture == null ? string.Empty : AssetDatabase.GetAssetPath(sourceTexture);
            if (sourceTexture != null && string.IsNullOrWhiteSpace(baseName))
            {
                baseName = Path.GetFileNameWithoutExtension(sourcePath);
            }
        }

        private void ValidateSource()
        {
            validationMessages.Clear();
            hasChromakey = false;
            hasErrors = false;

            if (sourceTexture == null)
            {
                AddValidation(ValidationSeverity.Info, "Select a texture asset to validate.");
                return;
            }

            if (string.IsNullOrEmpty(sourcePath) || !sourcePath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
            {
                AddValidation(ValidationSeverity.Error, "Source must be a PNG asset inside the Unity project.");
            }

            int expectedSize = mode == InputMode.Single ? SingleSize : SheetSize;
            if (sourceTexture.width != expectedSize || sourceTexture.height != expectedSize)
            {
                AddValidation(ValidationSeverity.Error, $"Size must be {expectedSize}x{expectedSize}; found {sourceTexture.width}x{sourceTexture.height}.");
            }

            TextureImporter importer = AssetImporter.GetAtPath(sourcePath) as TextureImporter;
            if (importer == null)
            {
                AddValidation(ValidationSeverity.Error, "TextureImporter not found for selected asset.");
            }
            else if (NeedsImporterFix(importer))
            {
                AddValidation(ValidationSeverity.Warning, "TextureImporter is not Point/NoCompression/NoMipmap and can be auto-fixed.");
            }

            Color32[] pixels = ReadTexturePixels(sourceTexture);
            if (pixels == null || pixels.Length == 0)
            {
                AddValidation(ValidationSeverity.Error, "Could not read source pixels for validation.");
                return;
            }

            if (pixels.Any(pixel => pixel.a > 0 && pixel.a < 255))
            {
                AddValidation(ValidationSeverity.Warning, "Semi-transparent alpha detected; expected binary alpha only.");
            }

            if (!HasValidBorderMortar(pixels, sourceTexture.width, sourceTexture.height))
            {
                AddValidation(ValidationSeverity.Warning, "Outer 1px border is not #1A1C20 within +/-5 RGB tolerance.");
            }

            hasChromakey = pixels.Any(IsChromakey);
            if (hasChromakey)
            {
                AddValidation(ValidationSeverity.Info, "Chromakey pixels detected with G>200, R<60, B<60.");
            }
        }

        private void ImportSelectedTexture()
        {
            SyncSourcePathAndName();
            ValidateSource();

            if (hasErrors)
            {
                bool cancel = EditorUtility.DisplayDialog(
                    "Validation failed. Import anyway?",
                    "Validation errors were found. Importing anyway may create incorrect tile assets.",
                    "Cancel",
                    "Import anyway");
                if (cancel)
                {
                    return;
                }
            }

            if (sourceTexture == null || string.IsNullOrWhiteSpace(sourcePath))
            {
                return;
            }

            string sanitizedName = SanitizeAssetName(string.IsNullOrWhiteSpace(baseName) ? Path.GetFileNameWithoutExtension(sourcePath) : baseName);
            string category = Categories[Mathf.Clamp(categoryIndex, 0, Categories.Length - 1)];

            try
            {
                EditorUtility.DisplayProgressBar("Tile Import Wizard", "Preparing texture", 0f);

                if (hasChromakey && convertChromakey)
                {
                    ApplyChromakeyConversion();
                }

                TextureImporter importer = AssetImporter.GetAtPath(sourcePath) as TextureImporter;
                if (importer == null)
                {
                    throw new IOException("TextureImporter not found after texture preparation.");
                }

                ConfigureImporter(importer, sanitizedName);

                AssetDatabase.ImportAsset(sourcePath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();

                EditorUtility.DisplayProgressBar("Tile Import Wizard", "Creating tile assets", 0.45f);
                if (mode == InputMode.Single)
                {
                    CreateSingleTile(category, sanitizedName);
                }
                else
                {
                    CreateSheetTiles(category, sanitizedName);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                ValidateSource();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Tile Import Wizard failed: {ex.Message}");
                EditorUtility.DisplayDialog("Tile Import Wizard", ex.Message, "OK");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void ApplyChromakeyConversion()
        {
            Color32[] pixels = ReadTexturePixels(sourceTexture);
            if (pixels == null || pixels.Length == 0)
            {
                return;
            }

            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 pixel = pixels[i];
                pixel.a = IsChromakey(pixel) ? (byte)0 : (pixel.a > 0 ? (byte)255 : (byte)0);
                pixels[i] = pixel;
            }

            var output = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
            output.SetPixels32(pixels);
            output.Apply(false, false);
            File.WriteAllBytes(sourcePath, output.EncodeToPNG());
            DestroyImmediate(output);
            AssetDatabase.ImportAsset(sourcePath, ImportAssetOptions.ForceUpdate);
            sourceTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(sourcePath);
        }

        private void ConfigureImporter(TextureImporter importer, string sanitizedName)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = PPU;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.isReadable = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.spriteImportMode = mode == InputMode.Single ? SpriteImportMode.Single : SpriteImportMode.Multiple;

            var platformSettings = importer.GetDefaultPlatformTextureSettings();
            platformSettings.format = TextureImporterFormat.RGBA32;
            importer.SetPlatformTextureSettings(platformSettings);

            if (mode == InputMode.Sheet)
            {
                importer.spritesheet = BuildSpriteMetadata(sanitizedName);
            }

            importer.SaveAndReimport();
        }

        private SpriteMetaData[] BuildSpriteMetadata(string sanitizedName)
        {
            var metadata = new SpriteMetaData[SheetColumns * SheetRows];
            for (int row = 0; row < SheetRows; row++)
            {
                for (int column = 0; column < SheetColumns; column++)
                {
                    int index = row * SheetColumns + column;
                    metadata[index] = new SpriteMetaData
                    {
                        name = $"{sanitizedName}_v{index + 1:00}",
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f),
                        rect = new Rect(column * TilePixels, (SheetRows - 1 - row) * TilePixels, TilePixels, TilePixels)
                    };
                }
            }

            return metadata;
        }

        private void CreateSingleTile(string category, string sanitizedName)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(sourcePath);
            if (sprite == null)
            {
                throw new IOException("Could not load sprite from imported texture.");
            }

            string folder = EnsureTileFolder(category);
            CreateTileAsset(sprite, $"{folder}/{sanitizedName}.asset");
        }

        private void CreateSheetTiles(string category, string sanitizedName)
        {
            string folder = EnsureTileFolder(category);
            var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(sourcePath)
                .OfType<Sprite>()
                .ToDictionary(sprite => sprite.name, sprite => sprite);

            for (int i = 1; i <= SheetColumns * SheetRows; i++)
            {
                string spriteName = $"{sanitizedName}_v{i:00}";
                if (!sprites.TryGetValue(spriteName, out Sprite sprite))
                {
                    throw new IOException($"Could not load sprite {spriteName} from imported sheet.");
                }

                float progress = 0.45f + 0.5f * i / (SheetColumns * SheetRows);
                EditorUtility.DisplayProgressBar("Tile Import Wizard", $"Creating {spriteName}", progress);
                CreateTileAsset(sprite, $"{folder}/{spriteName}.asset");
            }
        }

        private static void CreateTileAsset(Sprite sprite, string assetPath)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;

            if (AssetDatabase.LoadAssetAtPath<Tile>(assetPath) != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(tile, assetPath);
        }

        private static string EnsureTileFolder(string category)
        {
            EnsureFolder("Assets", "Art");
            EnsureFolder("Assets/Art", "Tiles");
            EnsureFolder(TileRoot, category);
            return $"{TileRoot}/{category}";
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static bool NeedsImporterFix(TextureImporter importer)
        {
            return importer.filterMode != FilterMode.Point ||
                importer.textureCompression != TextureImporterCompression.Uncompressed ||
                importer.mipmapEnabled;
        }

        private static Color32[] ReadTexturePixels(Texture2D texture)
        {
            if (texture == null)
            {
                return null;
            }

            if (texture.isReadable)
            {
                return texture.GetPixels32();
            }

            RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            RenderTexture previous = RenderTexture.active;
            var readable = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

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
                DestroyImmediate(readable);
            }
        }

        private static bool HasValidBorderMortar(Color32[] pixels, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x != 0 && y != 0 && x != width - 1 && y != height - 1)
                    {
                        continue;
                    }

                    if (!IsMortar(pixels[y * width + x]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool IsMortar(Color32 pixel)
        {
            return Mathf.Abs(pixel.r - MortarColor.r) <= 5 &&
                Mathf.Abs(pixel.g - MortarColor.g) <= 5 &&
                Mathf.Abs(pixel.b - MortarColor.b) <= 5;
        }

        private static bool IsChromakey(Color32 pixel)
        {
            return pixel.g > 200 && pixel.r < 60 && pixel.b < 60;
        }

        private static Rect FitRect(Rect outer, int width, int height, float padding)
        {
            Rect padded = new Rect(outer.x + padding, outer.y + padding, outer.width - padding * 2f, outer.height - padding * 2f);
            float scale = Mathf.Min(padded.width / Mathf.Max(1, width), padded.height / Mathf.Max(1, height));
            float fittedWidth = width * scale;
            float fittedHeight = height * scale;
            return new Rect(
                padded.x + (padded.width - fittedWidth) * 0.5f,
                padded.y + (padded.height - fittedHeight) * 0.5f,
                fittedWidth,
                fittedHeight);
        }

        private static string SanitizeAssetName(string value)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            var chars = value.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray();
            string sanitized = new string(chars).Trim();
            return string.IsNullOrWhiteSpace(sanitized) ? "tile" : sanitized;
        }

        private void AddValidation(ValidationSeverity severity, string text)
        {
            validationMessages.Add(new ValidationMessage(severity, text));
            if (severity == ValidationSeverity.Error)
            {
                hasErrors = true;
            }
        }

        private static MessageType ToMessageType(ValidationSeverity severity)
        {
            switch (severity)
            {
                case ValidationSeverity.Error:
                    return MessageType.Error;
                case ValidationSeverity.Warning:
                    return MessageType.Warning;
                case ValidationSeverity.Info:
                default:
                    return MessageType.Info;
            }
        }

        private readonly struct ValidationMessage
        {
            public ValidationMessage(ValidationSeverity severity, string text)
            {
                Severity = severity;
                Text = text;
            }

            public ValidationSeverity Severity { get; }
            public string Text { get; }
        }
    }
}
