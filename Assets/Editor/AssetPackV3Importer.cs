using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public class AssetPackV3Importer : AssetPostprocessor
    {
        private const string WallsRoot = "Assets/Sprites/AssetPackV3/walls/";
        private const string FloorRoot = "Assets/Sprites/AssetPackV3/floor/";
        private const int WallsPPU = 64;
        private const int FloorPPU = 32;

        void OnPreprocessTexture()
        {
            var path = assetPath.Replace("\\", "/");
            bool isWall = path.StartsWith(WallsRoot);
            bool isFloor = path.StartsWith(FloorRoot);
            if (!isWall && !isFloor)
                return;

            var ti = (TextureImporter)assetImporter;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Single;
            ti.spritePixelsPerUnit = isFloor ? FloorPPU : WallsPPU;
            ti.filterMode = FilterMode.Point;
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.mipmapEnabled = false;
            ti.isReadable = false;
            ti.alphaIsTransparency = true;

            var settings = new TextureImporterSettings();
            ti.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;
            settings.spriteMeshType = SpriteMeshType.Tight;
            settings.spriteExtrude = 1;
            ti.SetTextureSettings(settings);

            var platform = ti.GetDefaultPlatformTextureSettings();
            platform.format = TextureImporterFormat.RGBA32;
            ti.SetPlatformTextureSettings(platform);
        }
    }
}
