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

    [MenuItem("RIMA/Fix Tile Sprites (Sub-Asset Embed)")]
    public static void FixTileSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Tile", new[] { "Assets/Art/Tiles/Act1" });
        int fixedViaReimport = 0;
        int fixedViaEmbed = 0;
        int alreadyOk = 0;
        int failed = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile == null) continue;

            bool spriteValid = tile.sprite != null
                && tile.sprite.rect.width >= 32f
                && tile.sprite.rect.height >= 32f;
            if (spriteValid)
            {
                alreadyOk++;
                continue;
            }

            // Remove any stale embedded sub-assets (wrong-size textures/sprites from prior runs)
            foreach (var sub in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (sub != tile && !(sub is Tile))
                    Object.DestroyImmediate(sub, true);
            }
            tile.sprite = null;

            string pngPath = Path.ChangeExtension(path, ".png");
            if (!File.Exists(pngPath))
            {
                Debug.LogWarning($"[Act1TileImporter] Missing PNG: {path}");
                failed++;
                continue;
            }

            // Path A: Force-reimport PNG with correct sprite settings, then re-resolve sprite
            TextureImporter ti = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            if (ti != null)
            {
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Single;
                ti.spritePixelsPerUnit = 64f;
                ti.spritePivot = new Vector2(0.5f, 0.5f);
                ti.spriteBorder = Vector4.zero;
                ti.filterMode = FilterMode.Point;
                ti.textureCompression = TextureImporterCompression.Uncompressed;
                ti.alphaIsTransparency = true;
                ti.mipmapEnabled = false;
                ti.maxTextureSize = 2048;
                TextureImporterSettings tis = new TextureImporterSettings();
                ti.ReadTextureSettings(tis);
                tis.spriteAlignment = (int)SpriteAlignment.Center;
                ti.SetTextureSettings(tis);
                ti.SaveAndReimport();
            }

            Sprite reimported = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);
            if (reimported != null)
            {
                tile.sprite = reimported;
                EditorUtility.SetDirty(tile);
                fixedViaReimport++;
                continue;
            }

            // Path B: Sub-asset embed fallback (with exception logging)
            try
            {
                byte[] bytes = File.ReadAllBytes(pngPath);
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Point;
                if (!ImageConversion.LoadImage(tex, bytes, false))
                {
                    Debug.LogWarning($"[Act1TileImporter] LoadImage returned false (bytes={bytes.Length}): {pngPath}");
                    failed++;
                    Object.DestroyImmediate(tex);
                    continue;
                }

                string assetName = Path.GetFileNameWithoutExtension(path);
                tex.name = $"{assetName}_Texture";

                Sprite sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    64f);
                sprite.name = assetName;

                AssetDatabase.AddObjectToAsset(tex, path);
                AssetDatabase.AddObjectToAsset(sprite, path);
                tile.sprite = sprite;
                EditorUtility.SetDirty(tile);
                fixedViaEmbed++;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Act1TileImporter] Embed exception for {pngPath}: {ex.GetType().Name}: {ex.Message}");
                failed++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[Act1TileImporter] FixTileSprites done. reimport={fixedViaReimport} embed={fixedViaEmbed} alreadyOk={alreadyOk} failed={failed}");
    }
}
