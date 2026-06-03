using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public class AssetPackV3Importer : AssetPostprocessor
    {
        private const string WallsRoot = "Assets/Sprites/AssetPackV3/walls/";
        private const string FloorRoot = "Assets/Sprites/AssetPackV3/floor/";
        // Iso diamond-room pack (S6 iso pivot). Same import rules, floor gets 128px painterly headroom.
        private const string IsoWallsRoot = "Assets/Sprites/Environment/IsoKit/walls/";
        private const string IsoFloorRoot = "Assets/Sprites/Environment/IsoKit/floor/";
        private const string IsoDecorRoot = "Assets/Sprites/Environment/IsoKit/decor/";
        private const int WallsPPU = 64;
        private const int FloorPPU = 32;
        // IsoKit working scale (S6 iso pivot): floor diamond 256x128, wall cell 256x512, all PPU 256 ->
        // floor = 1x0.5 world units, wall = 1 wide x 2 tall (just over the ~1.88-unit hero = "tall masonry").
        // Keeps floor and walls in CONSISTENT world scale (the old 128/64 split made walls 4x too tall).
        private const int IsoPPU = 256;

        void OnPreprocessTexture()
        {
            var path = assetPath.Replace("\\", "/");
            bool isWall = path.StartsWith(WallsRoot);
            bool isFloor = path.StartsWith(FloorRoot);
            bool isIsoWall = path.StartsWith(IsoWallsRoot);
            bool isIsoFloor = path.StartsWith(IsoFloorRoot);
            bool isIsoDecor = path.StartsWith(IsoDecorRoot);
            if (!isWall && !isFloor && !isIsoWall && !isIsoFloor && !isIsoDecor)
                return;

            var ti = (TextureImporter)assetImporter;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Single;
            ti.spritePixelsPerUnit = (isIsoFloor || isIsoWall || isIsoDecor) ? IsoPPU : isFloor ? FloorPPU : WallsPPU;
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
