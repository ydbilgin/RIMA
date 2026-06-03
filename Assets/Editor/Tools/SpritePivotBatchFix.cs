using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class SpritePivotBatchFix
{
    private static readonly string[] SearchFolders =
    {
        "Assets/Resources/Characters"
    };

    [MenuItem("RIMA/Utilities/Fix All Sprite Pivots")]
    public static void FixAllSpritePivots()
    {
        int updated = 0;
        var pivotsByClass = new Dictionary<string, List<float>>();
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", ExistingFolders());

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!IsIdleSprite(path) || AssetImporter.GetAtPath(path) is not TextureImporter importer)
            {
                continue;
            }

            if (ApplyMeasuredFeetPivot(path, importer, out float pivotY))
            {
                updated++;
            }

            string className = Path.GetFileName(Path.GetDirectoryName(path.Replace('\\', '/')));
            if (!pivotsByClass.TryGetValue(className, out List<float> pivots))
            {
                pivots = new List<float>();
                pivotsByClass[className] = pivots;
            }

            pivots.Add(pivotY);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"SpritePivotBatchFix: Measured feet pivot applied to {updated} idle sprite texture importer(s).\n{BuildPivotReport(pivotsByClass)}");
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

    private static bool IsIdleSprite(string path)
    {
        string fileName = Path.GetFileName(path);
        return path.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase)
            && fileName.Contains("_idle_");
    }

    private static bool ApplyMeasuredFeetPivot(string path, TextureImporter importer, out float pivotY)
    {
        bool changed = false;
        bool originalReadable = importer.isReadable;

        if (importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            changed = true;
        }

        if (importer.spriteImportMode != SpriteImportMode.Single)
        {
            importer.spriteImportMode = SpriteImportMode.Single;
            changed = true;
        }

        importer.isReadable = true;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.spritePixelsPerUnit = 64f;
        importer.SaveAndReimport();

        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        int lowestOpaqueRow = FindLowestOpaqueRow(texture);
        pivotY = Mathf.Clamp01((float)lowestOpaqueRow / texture.height);
        Vector2 feetPivot = new Vector2(0.5f, pivotY);

        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        if (settings.spriteAlignment != (int)SpriteAlignment.Custom)
        {
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            changed = true;
        }

        if (settings.spritePivot != feetPivot)
        {
            settings.spritePivot = feetPivot;
            changed = true;
        }

        importer.SetTextureSettings(settings);
        if (importer.spritePivot != feetPivot)
        {
            importer.spritePivot = feetPivot;
            changed = true;
        }

        if (importer.isReadable != originalReadable)
        {
            importer.isReadable = originalReadable;
            changed = true;
        }

        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.spritePixelsPerUnit = 64f;
        importer.SaveAndReimport();

        return changed;
    }

    private static int FindLowestOpaqueRow(Texture2D texture)
    {
        Color32[] pixels = texture.GetPixels32();
        for (int y = 0; y < texture.height; y++)
        {
            int rowOffset = y * texture.width;
            for (int x = 0; x < texture.width; x++)
            {
                if (pixels[rowOffset + x].a > 10)
                {
                    return y;
                }
            }
        }

        return 0;
    }

    private static string BuildPivotReport(Dictionary<string, List<float>> pivotsByClass)
    {
        return string.Join("\n", pivotsByClass
            .OrderBy(pair => pair.Key)
            .Select(pair =>
            {
                float min = pair.Value.Min();
                float max = pair.Value.Max();
                return $"{pair.Key}: {pair.Value.Count} sprite(s), pivot.y {min.ToString("0.###", CultureInfo.InvariantCulture)}-{max.ToString("0.###", CultureInfo.InvariantCulture)}";
            }));
    }
}
