using RIMA.Systems.Map;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public static class CreateDemoScene
{
    [MenuItem("RIMA/Tools/Create FazMVP Demo Scene")]
    public static void Create()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.07f, 0.07f, 0.07f);
        TryAddPixelPerfectCamera(camGO);

        var lightGO = new GameObject("Global Light 2D");
        var light2d = lightGO.AddComponent<Light2D>();
        light2d.lightType = Light2D.LightType.Global;
        light2d.intensity = 0.9f;

        var gridGO = new GameObject("Grid");
        var grid = gridGO.AddComponent<Grid>();
        grid.cellSize = new Vector3(1f, 1f, 0f);

        Tilemap baseTm = CreateTilemap(gridGO, "BaseTilemap", 0);
        Tilemap decalTm = CreateTilemap(gridGO, "DecalTilemap", 1);
        Tilemap wallFrontTm = CreateTilemap(gridGO, "WallsTilemap_Front", 2, hasCollider: true);
        Tilemap wallTopTm = CreateTilemap(gridGO, "WallsTilemap_Top", 3);

        var wallFrontGO = wallFrontTm.gameObject;
        var rb = wallFrontGO.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        wallFrontGO.AddComponent<CompositeCollider2D>();

        var propContainerGO = new GameObject("PropContainer");

        var controller = gridGO.AddComponent<RIMA.Demo.RoomPipelineTestController>();
        var so = new SerializedObject(controller);
        so.FindProperty("baseTilemap").objectReferenceValue = baseTm;
        so.FindProperty("decalsTilemap").objectReferenceValue = decalTm;
        so.FindProperty("wallsTilemap").objectReferenceValue = wallFrontTm;
        so.FindProperty("wallsFrontTilemap").objectReferenceValue = wallFrontTm;
        so.FindProperty("wallsTopTilemap").objectReferenceValue = wallTopTm;
        so.FindProperty("stageRoot").objectReferenceValue = propContainerGO;

        var preset = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset");
        if (preset == null)
        {
            preset = Resources.Load<RimaBiomePreset>("Biomes/Shattered_Keep_F1_BiomePreset");
        }

        if (preset != null)
        {
            so.FindProperty("biomePreset").objectReferenceValue = preset;
        }
        else
        {
            Debug.LogWarning("CreateDemoScene: Could not find Shattered_Keep_F1_BiomePreset.asset - assign manually in Inspector.");
        }

        so.ApplyModifiedProperties();

        foreach (var tmr in gridGO.GetComponentsInChildren<TilemapRenderer>())
        {
            tmr.sortOrder = TilemapRenderer.SortOrder.TopRight;
        }

        var playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        if (playerPrefab != null)
        {
            var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
            player.transform.position = new Vector3(5f, 5f, 0f);
        }
        else
        {
            Debug.LogWarning("CreateDemoScene: Player.prefab not found at Assets/Prefabs/Player.prefab - spawn manually.");
        }

        string dir = "Assets/Scenes/Demo";
        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }

        string path = $"{dir}/_FazMVP_Demo.unity";
        EditorSceneManager.SaveScene(scene, path);
        AssetDatabase.Refresh();
        Debug.Log($"CreateDemoScene: Saved {path}");

        var sceneEntry = new EditorBuildSettingsScene(path, true);
        var existing = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        existing.RemoveAll(s => s.path == path);
        existing.Insert(0, sceneEntry);
        EditorBuildSettings.scenes = existing.ToArray();
        Debug.Log("CreateDemoScene: Added to Build Settings as scene 0.");
    }

    private static void TryAddPixelPerfectCamera(GameObject camGO)
    {
        var ppcType = System.Type.GetType("UnityEngine.U2D.PixelPerfectCamera, Unity.2D.PixelPerfect");
        if (ppcType == null)
        {
            Debug.LogWarning("CreateDemoScene: PixelPerfectCamera is not available - skipping.");
            return;
        }

        var ppc = camGO.AddComponent(ppcType);
        SetPropertyOrField(ppc, "assetsPPU", 64);
        SetPropertyOrField(ppc, "refResolutionX", 480);
        SetPropertyOrField(ppc, "refResolutionY", 270);
        SetPropertyOrField(ppc, "cropFrameX", false);
        SetPropertyOrField(ppc, "cropFrameY", false);
    }

    private static void SetPropertyOrField(object target, string name, object value)
    {
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
        var property = target.GetType().GetProperty(name, flags);
        if (property != null)
        {
            property.SetValue(target, value);
            return;
        }

        var field = target.GetType().GetField(name, flags);
        if (field != null)
        {
            field.SetValue(target, value);
        }
    }

    private static Tilemap CreateTilemap(GameObject parent, string name, int sortOrder, bool hasCollider = false)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        var tm = go.AddComponent<Tilemap>();
        var tr = go.AddComponent<TilemapRenderer>();
        tr.sortingOrder = sortOrder;
        if (hasCollider)
        {
            var col = go.AddComponent<TilemapCollider2D>();
            col.usedByComposite = true;
        }

        return tm;
    }
}
