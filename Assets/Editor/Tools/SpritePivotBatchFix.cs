using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SpritePivotBatchFix
{
    private static readonly string[] SearchFolders =
    {
        "Assets/Art/Characters",
        "Assets/Art/Tiles/F1",
        "Assets/Art/Mobs"
    };

    [MenuItem("RIMA/Tools/Fix All Sprite Pivots")]
    public static void FixAllSpritePivots()
    {
        int updated = 0;
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", ExistingFolders());

        AssetDatabase.StartAssetEditing();
        try
        {
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetImporter.GetAtPath(path) is not TextureImporter importer)
                {
                    continue;
                }

                if (ApplyBottomCenterPivot(importer))
                {
                    importer.SaveAndReimport();
                    updated++;
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log($"SpritePivotBatchFix: Bottom-Center pivot applied to {updated} texture importer(s).");
    }

    private static string[] ExistingFolders()
    {
        var folders = new List<string>();
        foreach (string folder in SearchFolders)
        {
            if (AssetDatabase.IsValidFolder(folder))
            {
                folders.Add(folder);
            }
        }

        return folders.ToArray();
    }

    private static bool ApplyBottomCenterPivot(TextureImporter importer)
    {
        bool changed = false;

        if (importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            changed = true;
        }

        if (importer.spritePivot != new Vector2(0.5f, 0f))
        {
            importer.spritePivot = new Vector2(0.5f, 0f);
            changed = true;
        }

        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;

        if (importer.spriteImportMode == SpriteImportMode.Multiple)
        {
#pragma warning disable 0618
            SpriteMetaData[] metas = importer.spritesheet;
            for (int i = 0; i < metas.Length; i++)
            {
                if (metas[i].alignment != (int)SpriteAlignment.Custom || metas[i].pivot != new Vector2(0.5f, 0f))
                {
                    metas[i].alignment = (int)SpriteAlignment.Custom;
                    metas[i].pivot = new Vector2(0.5f, 0f);
                    changed = true;
                }
            }

            importer.spritesheet = metas;
#pragma warning restore 0618
        }

        return changed;
    }
}
