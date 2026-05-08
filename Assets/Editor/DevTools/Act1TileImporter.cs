using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public static class Act1TileImporter
{
    [MenuItem("RIMA/Import Act1 Tiles")]
    public static void ImportAll()
    {
        string root = "Assets/Art/Tiles/Act1";
        string[] folders = AssetDatabase.GetSubFolders(root);
        int created = 0, skipped = 0;

        // Pre-pass: bust stale DefaultAsset cache for all tile PNGs
        string sysRoot = Path.Combine(Application.dataPath, "Art/Tiles/Act1");
        foreach (string sysFile in Directory.GetFiles(sysRoot, "*.png", SearchOption.AllDirectories))
        {
            string ap = "Assets" + sysFile.Substring(Application.dataPath.Length).Replace('\\', '/');
            AssetDatabase.ImportAsset(ap, ImportAssetOptions.ForceUpdate);
        }
        AssetDatabase.Refresh();

        foreach (string folder in folders)
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });
            foreach (string guid in guids)
            {
                string texPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!texPath.EndsWith(".png")) continue;

                TextureImporter ti = AssetImporter.GetAtPath(texPath) as TextureImporter;
                if (ti != null)
                {
                    ti.textureType = TextureImporterType.Sprite;
                    ti.spriteImportMode = SpriteImportMode.Single;
                    ti.spritePixelsPerUnit = 64f;
                    ti.filterMode = FilterMode.Point;
                    ti.textureCompression = TextureImporterCompression.Uncompressed;
                    ti.alphaIsTransparency = true;
                    ti.SaveAndReimport();
                }

                string tilePath = texPath.Replace(".png", ".asset");
                if (AssetDatabase.LoadAssetAtPath<Tile>(tilePath) == null)
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texPath);
                    if (sprite != null)
                    {
                        Tile tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = sprite;
                        AssetDatabase.CreateAsset(tile, tilePath);
                        created++;
                    }
                }
                else
                {
                    skipped++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[Act1TileImporter] Done. Created: {created}, Skipped: {skipped}");
    }
}
