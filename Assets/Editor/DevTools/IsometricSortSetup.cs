using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public static class IsometricSortSetup
{
    [MenuItem("RIMA/Setup Isometric Sorting")]
    public static void Run()
    {
        foreach (var cam in Camera.allCameras)
        {
            cam.transparencySortMode = TransparencySortMode.CustomAxis;
            cam.transparencySortAxis = new Vector3(0, 1, 0);
            EditorUtility.SetDirty(cam);
        }
        if (Camera.allCameras.Length == 0)
            Debug.LogWarning("RIMA: No cameras in scene — set Transparency Sort Mode manually on your camera.");

        Grid[] grids = Object.FindObjectsByType<Grid>(FindObjectsSortMode.None);
        if (grids.Length == 0)
        {
            Debug.LogError("RIMA: No Grid found in the active scene.");
            return;
        }

        EnsureSortingLayer("Ground");
        EnsureSortingLayer("Wall");

        Grid grid = grids[0];
        TilemapRenderer[] renderers = grid.GetComponentsInChildren<TilemapRenderer>(true);

        foreach (TilemapRenderer renderer in renderers)
        {
            string objectName = renderer.gameObject.name;
            bool configured = true;

            if (objectName.Contains("Ground") || objectName.Contains("Floor"))
            {
                renderer.sortingLayerName = "Ground";
                renderer.sortingOrder = 0;
            }
            else if (objectName.Contains("Detail"))
            {
                renderer.sortingLayerName = "Ground";
                renderer.sortingOrder = 1;
            }
            else if (objectName.Contains("AO"))
            {
                renderer.sortingLayerName = "Ground";
                renderer.sortingOrder = 2;
            }
            else if (objectName.Contains("Wall") || objectName.Contains("Structural"))
            {
                renderer.sortingLayerName = "Wall";
                renderer.sortingOrder = 10;
            }
            else if (objectName.Contains("Obstacle"))
            {
                renderer.sortingLayerName = "Wall";
                renderer.sortingOrder = 5;
            }
            else
            {
                configured = false;
                Debug.Log($"RIMA: Skipped TilemapRenderer '{objectName}'.");
            }

            if (!configured)
            {
                continue;
            }

            renderer.mode = TilemapRenderer.Mode.Individual;
            EditorUtility.SetDirty(renderer);
        }

        EditorUtility.SetDirty(grid);
        AssetDatabase.SaveAssets();
        Debug.Log("RIMA: Isometric sort configured.");
    }

    private static void EnsureSortingLayer(string layerName)
    {
        Object tagManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0];
        SerializedObject tagManager = new SerializedObject(tagManagerAsset);
        SerializedProperty sortingLayersProp = tagManager.FindProperty("m_SortingLayers");

        for (int i = 0; i < sortingLayersProp.arraySize; i++)
        {
            SerializedProperty layerProp = sortingLayersProp.GetArrayElementAtIndex(i);
            if (layerProp.FindPropertyRelative("name").stringValue == layerName)
            {
                return;
            }
        }

        sortingLayersProp.InsertArrayElementAtIndex(sortingLayersProp.arraySize);
        SerializedProperty newLayerProp = sortingLayersProp.GetArrayElementAtIndex(sortingLayersProp.arraySize - 1);
        newLayerProp.FindPropertyRelative("name").stringValue = layerName;
        newLayerProp.FindPropertyRelative("uniqueID").intValue = GenerateSortingLayerId(sortingLayersProp);
        newLayerProp.FindPropertyRelative("locked").boolValue = false;

        tagManager.ApplyModifiedProperties();
        EditorUtility.SetDirty(tagManagerAsset);
    }

    private static int GenerateSortingLayerId(SerializedProperty sortingLayersProp)
    {
        int id = 1;

        for (int i = 0; i < sortingLayersProp.arraySize; i++)
        {
            SerializedProperty uniqueIdProp = sortingLayersProp.GetArrayElementAtIndex(i).FindPropertyRelative("uniqueID");
            if (uniqueIdProp.intValue >= id)
            {
                id = uniqueIdProp.intValue + 1;
            }
        }

        return id;
    }
}
