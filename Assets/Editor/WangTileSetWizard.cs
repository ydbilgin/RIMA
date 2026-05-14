using System;
using UnityEditor;
using UnityEngine;

public class WangTileSetWizard : EditorWindow
{
    private Texture2D sourceTexture;
    private string lowerLabel;
    private string upperLabel;
    private string lastCreatedPath;
    private TilesetMeta loadedMeta;

    [MenuItem("RIMA/Tools/Wang Tileset Wizard")]
    public static void Open()
    {
        GetWindow<WangTileSetWizard>("Wang Tileset Wizard");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        sourceTexture = (Texture2D)EditorGUILayout.ObjectField("Texture", sourceTexture, typeof(Texture2D), false);
        if (EditorGUI.EndChangeCheck())
        {
            loadedMeta = null;
            lastCreatedPath = string.Empty;
        }

        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.TextField("Lower Label", lowerLabel);
            EditorGUILayout.TextField("Upper Label", upperLabel);
        }

        if (GUILayout.Button("Load from tileset_meta.json"))
        {
            LoadMeta();
        }

        using (new EditorGUI.DisabledScope(sourceTexture == null))
        {
            if (GUILayout.Button("Create SO"))
            {
                CreateSo();
            }
        }

        if (!string.IsNullOrEmpty(lastCreatedPath))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"✓ SO created: {lastCreatedPath}", EditorStyles.wordWrappedLabel);
        }
    }

    private void LoadMeta()
    {
        string metaPath = WangTilesetBuilder.FindMetaPathForTexture(sourceTexture);
        if (string.IsNullOrEmpty(metaPath))
        {
            Debug.LogError("[RIMA] Select a source texture inside a tileset folder.");
            return;
        }

        try
        {
            loadedMeta = WangTilesetBuilder.LoadMeta(metaPath);
            lowerLabel = loadedMeta.lower;
            upperLabel = loadedMeta.upper;
            lastCreatedPath = string.Empty;
        }
        catch (Exception exception)
        {
            Debug.LogError($"[RIMA] Could not load tileset_meta.json: {exception.Message}");
        }
    }

    private void CreateSo()
    {
        string metaPath = WangTilesetBuilder.FindMetaPathForTexture(sourceTexture);
        if (string.IsNullOrEmpty(metaPath))
        {
            Debug.LogError("[RIMA] Select a source texture inside a tileset folder.");
            return;
        }

        try
        {
            if (loadedMeta == null)
            {
                loadedMeta = WangTilesetBuilder.LoadMeta(metaPath);
                lowerLabel = loadedMeta.lower;
                upperLabel = loadedMeta.upper;
            }

            lastCreatedPath = WangTilesetBuilder.CreateFromMeta(metaPath);
        }
        catch (Exception exception)
        {
            Debug.LogError($"[RIMA] Could not create Wang tileset SO: {exception.Message}");
        }
    }
}
