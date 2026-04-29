using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class FixTilemapMaterials
{
    [MenuItem("RIMA/Fix Tilemap Materials (Unlit)")]
    public static void FixUnlit()
    {
        SetMat("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Unlit-Default.mat", "Sprite-Unlit-Default");
    }

    [MenuItem("RIMA/Fix Tilemap Materials (Lit)")]
    public static void FixLit()
    {
        SetMat("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat", "Sprite-Lit-Default");
    }

    static void SetMat(string path, string matName)
    {
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null) { Debug.LogError("[FixTilemapMaterials] NOT FOUND: " + path); return; }
        var trs = Object.FindObjectsOfType<TilemapRenderer>(true);
        foreach (var tr in trs)
        {
            Debug.Log("[FixTilemapMaterials] " + tr.gameObject.name + " -> " + matName);
            tr.sharedMaterial = mat;
            EditorUtility.SetDirty(tr);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("[FixTilemapMaterials] Done.");
    }
}
