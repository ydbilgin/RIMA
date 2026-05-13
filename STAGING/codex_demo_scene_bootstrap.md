# Demo Scene Bootstrap — execute every step, commit at end

## Context

The school-deadline demo target. Creates `_FazMVP_Demo.unity` with all P0 systems connected:
- F1 Shattered Keep room (LayeredRoomPainter biome wiring — commit 804a3f6)
- Player.prefab (commit c33c5bd) spawned at room center
- Pixel Perfect Camera (PPU=64, 480x270 ref)
- 4-layer tilemap stack (commit 562c575)
- Antigravity 4 P0: Y-Sort + URP 2D Renderer

The scene is created via Editor C# script (MenuItem) since Codex cannot directly create Unity scenes.

## Key files (DO NOT read others)

- `Assets/Scripts/Demo/RoomPipelineTestController.cs` (for tilemap/biome field names)
- `Assets/Prefabs/Player.prefab` (check it exists)
- `Assets/Scripts/Systems/Map/RimaBiomePreset.cs` (field names)

## STEP 1 — Read key files

Read RoomPipelineTestController.cs and verify Player.prefab exists.

## STEP 2 — Create Editor script for demo scene

Create `Assets/Editor/CreateDemoScene.cs`:

```csharp
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;
using RIMA.Systems.Map;

public static class CreateDemoScene
{
    [MenuItem("RIMA/Tools/Create FazMVP Demo Scene")]
    public static void Create()
    {
        // New scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Camera — Pixel Perfect
        var camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.07f, 0.07f, 0.07f);

        // Try to add PixelPerfectCamera if available
        var ppc = camGO.AddComponent<UnityEngine.U2D.PixelPerfectCamera>();
        ppc.assetsPPU = 64;
        ppc.refResolutionX = 480;
        ppc.refResolutionY = 270;
        ppc.cropFrameX = false;
        ppc.cropFrameY = false;

        // Global 2D Light
        var lightGO = new GameObject("Global Light 2D");
        var light2d = lightGO.AddComponent<Light2D>();
        light2d.lightType = Light2D.LightType.Global;
        light2d.intensity = 0.9f;

        // Grid + Tilemap stack
        var gridGO = new GameObject("Grid");
        var grid = gridGO.AddComponent<Grid>();
        grid.cellSize = new Vector3(1f, 1f, 0f);

        Tilemap baseTm      = CreateTilemap(gridGO, "BaseTilemap",       0);
        Tilemap decalTm     = CreateTilemap(gridGO, "DecalTilemap",      1);
        Tilemap wallFrontTm = CreateTilemap(gridGO, "WallsTilemap_Front",2, hasCollider: true);
        Tilemap wallTopTm   = CreateTilemap(gridGO, "WallsTilemap_Top",  3);

        // Composite Collider for WallFront
        var wallFrontGO = wallFrontTm.gameObject;
        var rb = wallFrontGO.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        wallFrontGO.AddComponent<CompositeCollider2D>();

        // PropContainer
        var propContainerGO = new GameObject("PropContainer");

        // RoomPipelineTestController on Grid
        var controller = gridGO.AddComponent<RIMA.Demo.RoomPipelineTestController>();
        // Wire tilemaps via SerializedObject
        var so = new SerializedObject(controller);
        so.FindProperty("baseTilemap").objectReferenceValue     = baseTm;
        so.FindProperty("decalsTilemap").objectReferenceValue   = decalTm;
        so.FindProperty("wallsFrontTilemap").objectReferenceValue = wallFrontTm;
        so.FindProperty("wallsTopTilemap").objectReferenceValue  = wallTopTm;
        so.FindProperty("stageRoot").objectReferenceValue        = propContainerGO;

        // Try to load F1 Shattered Keep biome preset
        var preset = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset");
        if (preset == null)
            preset = Resources.Load<RimaBiomePreset>("Biomes/Shattered_Keep_F1_BiomePreset");
        if (preset != null)
            so.FindProperty("biomePreset").objectReferenceValue = preset;
        else
            Debug.LogWarning("CreateDemoScene: Could not find Shattered_Keep_F1_BiomePreset.asset — assign manually in Inspector.");

        so.ApplyModifiedProperties();

        // Y-Sort Renderer Feature — set sortingAxis on all TilemapRenderer
        foreach (var tmr in gridGO.GetComponentsInChildren<TilemapRenderer>())
            tmr.sortOrder = TilemapRenderer.SortOrder.BottomToTop;

        // Player
        var playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        if (playerPrefab != null)
        {
            var player = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
            player.transform.position = new Vector3(5f, 5f, 0f);
        }
        else
        {
            Debug.LogWarning("CreateDemoScene: Player.prefab not found at Assets/Prefabs/Player.prefab — spawn manually.");
        }

        // Save scene
        string dir = "Assets/Scenes/Demo";
        if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
        string path = $"{dir}/_FazMVP_Demo.unity";
        EditorSceneManager.SaveScene(scene, path);
        AssetDatabase.Refresh();
        Debug.Log($"CreateDemoScene: Saved {path}");

        // Add to build settings (as scene 0, replace existing)
        var scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(path, true),
        };
        // Keep existing scenes, prepend demo
        var existing = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        existing.RemoveAll(s => s.path == path);
        existing.Insert(0, scenes[0]);
        EditorBuildSettings.scenes = existing.ToArray();
        Debug.Log("CreateDemoScene: Added to Build Settings as scene 0.");
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
```

## STEP 3 — Run the menu item

Execute menu item "RIMA/Tools/Create FazMVP Demo Scene" via `execute_menu_item` (UnityMCP) or via `read_console` to confirm.

If UnityMCP not available, run manually from Unity Editor.

## STEP 4 — Check RoomPipelineTestController field names

If compilation fails on `wallsFrontTilemap` or `wallsTopTilemap` fields, read the actual field names from RoomPipelineTestController.cs and adjust the SerializedObject property paths.

## STEP 5 — Compile check

`read_console` — 0 errors required. Fix any compiler errors.

## STEP 6 — Commit

```bash
git add Assets/Editor/CreateDemoScene.cs Assets/Scenes/Demo/_FazMVP_Demo.unity Assets/Scenes/Demo/_FazMVP_Demo.unity.meta Assets/Scenes/Demo/ ProjectSettings/EditorBuildSettings.asset
git commit -m "[demo] _FazMVP_Demo.unity — school deadline demo scene bootstrap

- Grid + BaseTilemap/DecalTilemap/WallsFront/WallsTop + PropContainer
- RoomPipelineTestController wired to Shattered Keep F1 biome preset
- Player.prefab spawned at center
- PixelPerfectCamera PPU=64, 480x270 ref
- Global 2D Light 0.9 intensity
- Build Settings scene 0"
```

## STEP 7 — Report

Write `STAGING/demo_scene_bootstrap_report.md`:
```
# Demo Scene Bootstrap Report

## _FazMVP_Demo.unity
[created Y/N, path]

## Tilemap stack
[Base/Decal/WallFront/WallTop Y/N]

## RoomPipelineTestController
[biomePreset assigned Y/N]

## Player.prefab
[spawned in scene Y/N]

## Build Settings
[scene 0 Y/N]

## Compile
[0 errors Y/N]
```

Append `CODEX_DONE_yasinderyabilgin.md`:
```
## [2026-05-14] Demo Scene Bootstrap
- _FazMVP_Demo.unity: Y/N
- Tilemap stack: Y/N
- BiomePreset assigned: Y/N
- Player spawned: Y/N
- Compile: Y/N
- Commit: [hash]
```

## Constraints

- DO NOT add gameplay systems (movement, combat) to scene — Player.prefab already has them
- DO NOT delete existing scenes (RoomPipelineTest.unity stays in Build Settings)
- `RoomPipelineTestController` field names for tilemaps added in Karar #118b / biome wiring — verify before wiring
- PixelPerfectCamera may not be available if package missing — skip and log warning
- Biome preset path: `Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset` (verified)

## Source References

1. `Assets/Scripts/Demo/RoomPipelineTestController.cs` — field names for tilemaps + biomePreset
2. `Assets/Prefabs/Player.prefab` — verify path before instantiation
3. `Assets/Scripts/Systems/Map/RimaBiomePreset.cs` — type name
