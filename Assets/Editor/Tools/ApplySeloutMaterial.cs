using UnityEditor;
using UnityEngine;

public static class ApplySeloutMaterial
{
    private const string CharacterSearchFolder = "Assets/Resources/Characters";
    private const string SeloutMaterialPath = "Assets/Art/Materials/SeloutSprite.mat";

    [MenuItem("RIMA/Tools/Apply Selout to All Characters")]
    public static void ApplyToAllCharacters()
    {
        Material material = AssetDatabase.LoadAssetAtPath<Material>(SeloutMaterialPath);
        if (material == null)
        {
            Debug.LogError($"ApplySeloutMaterial: Could not load material at {SeloutMaterialPath}.");
            return;
        }

        if (!AssetDatabase.IsValidFolder(CharacterSearchFolder))
        {
            Debug.LogWarning($"ApplySeloutMaterial: Search folder does not exist: {CharacterSearchFolder}.");
            return;
        }

        int updated = 0;
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { CharacterSearchFolder });
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                continue;
            }

            bool changed = false;
            SpriteRenderer[] renderers = prefab.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer.sharedMaterial == material)
                {
                    continue;
                }

                renderer.sharedMaterial = material;
                changed = true;
            }

            if (changed)
            {
                EditorUtility.SetDirty(prefab);
                updated++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"ApplySeloutMaterial: Applied selout material to {updated} character prefab(s).");
    }
}
