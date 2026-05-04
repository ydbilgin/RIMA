using System.IO;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace RIMA.Editor
{
    /// <summary>
    /// Auto-applies RIMA S43 import settings to any PNG dropped into
    /// Assets/Sprites/Characters/<ClassName>/Animations/.
    /// Settings: Sprite/Multiple, PPU=64, Point filter, No compression.
    /// Sprite sheet is sliced into CELL_SIZE x CELL_SIZE cells (row-major, top-to-bottom).
    /// </summary>
    public class AnimationImportHelper : AssetPostprocessor
    {
        private const int PPU       = 64;
        private const int CELL_SIZE = 128;
        private const string WATCH_PATH  = "Assets/Sprites/Characters/";
        private const string ANIM_FOLDER = "/Animations/";

        private void OnPreprocessTexture()
        {
            if (!IsAnimationSprite(assetPath)) return;

            var importer = (TextureImporter)assetImporter;

            importer.textureType         = TextureImporterType.Sprite;
            importer.spriteImportMode    = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = PPU;
            importer.filterMode          = FilterMode.Point;
            importer.mipmapEnabled       = false;
            importer.textureCompression  = TextureImporterCompression.Uncompressed;
            importer.alphaIsTransparency = true;
            importer.isReadable          = false;

            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            settings.spriteMeshType  = SpriteMeshType.FullRect;
            settings.spriteAlignment = (int)SpriteAlignment.Center;
            settings.spritePivot     = new Vector2(0.5f, 0.5f);
            importer.SetTextureSettings(settings);
        }

        private void OnPostprocessTexture(Texture2D texture)
        {
            if (!IsAnimationSprite(assetPath)) return;

            var importer   = (TextureImporter)assetImporter;
            int cols       = Mathf.Max(1, texture.width  / CELL_SIZE);
            int rows       = Mathf.Max(1, texture.height / CELL_SIZE);
            int frameCount = cols * rows;

            string baseName = Path.GetFileNameWithoutExtension(assetPath);

            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
            dataProvider.InitSpriteEditorDataProvider();

            var spriteRects = new SpriteRect[frameCount];
            int index = 0;

            for (int row = rows - 1; row >= 0; row--)
            {
                for (int col = 0; col < cols; col++)
                {
                    spriteRects[index] = new SpriteRect
                    {
                        name      = $"{baseName}_{index:D2}",
                        rect      = new Rect(col * CELL_SIZE, row * CELL_SIZE, CELL_SIZE, CELL_SIZE),
                        alignment = SpriteAlignment.Center,
                        pivot     = new Vector2(0.5f, 0.5f),
                        spriteID  = GUID.Generate()
                    };
                    index++;
                }
            }

            dataProvider.SetSpriteRects(spriteRects);
            dataProvider.Apply();

            Debug.Log($"[RIMA] Auto-imported: {Path.GetFileName(assetPath)} -> {frameCount} frames @ PPU{PPU}");
        }

        private static bool IsAnimationSprite(string path)
        {
            if (!path.StartsWith(WATCH_PATH)) return false;
            if (!path.Contains(ANIM_FOLDER)) return false;
            return Path.GetExtension(path).ToLower() == ".png";
        }
    }
}
