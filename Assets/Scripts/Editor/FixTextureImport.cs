using UnityEngine;
using UnityEditor;

public class FixTextureImport
{
    [MenuItem("RIMA/Fix Act1 Texture Import")]
    public static void FixImport()
    {
        string[] paths = new string[] {
            "Assets/Art/Tiles/Act1/Act1_Floor.png",
            "Assets/Art/Tiles/Act1/Act1_Wall.png"
        };

        foreach (string path in paths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritePixelsPerUnit = 16;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
                Debug.Log("Fixed import for: " + path);
            }
        }

        AssetDatabase.Refresh();
    }
}
