using System;
using System.IO;
using RIMA.Rooms;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class CombatRoomV14Builder
{
    private const string ScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";
    private const string TextureFolder = "Assets/Sprites/Environment/CombatV14";
    private const string FloorPath = TextureFolder + "/PlayableRoom_DesignedFloor_combat_v14.png";
    private const string VignettePath = TextureFolder + "/CombatV14_EdgeVignette.png";
    private const string AshCirclePath = TextureFolder + "/CombatV14_AshCircle.png";
    private const string BloodSplatPath = TextureFolder + "/CombatV14_BloodSplat.png";
    private const string RiftCrackPath = TextureFolder + "/CombatV14_RiftCrackRed.png";

    private const float Ppu = 32f;
    private static readonly Material SpriteLit = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
    private static Rigidbody2D probeRb;
    private static TestPlayerMovement probeMovement;
    private static Vector2 probeStartPosition;
    private static float probeStartTime;

    [MenuItem("RIMA/Playable Room/Build Combat v14")]
    public static void Build()
    {
        EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        EnsureTextureFolder();
        GenerateProceduralAssets();

        GameObject playableRoom = GameObject.Find("PlayableRoom");
        if (playableRoom == null)
        {
            throw new InvalidOperationException("PlayableRoom root was not found.");
        }

        PreserveRitualReference(playableRoom.transform);
        ReplaceActiveCombatRoom(playableRoom.transform);
        ConfigurePlayer(playableRoom.transform);
        ConfigureCameraAndDoor(playableRoom.transform);
        ConfigureLighting();
        int removedMissingScripts = RemoveMissingScriptsInScene();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[CombatRoomV14] Built Pro_Redesign_v14_CombatRoom and saved RoomPipelineTest. Missing scripts removed: " + removedMissingScripts);
    }

    [MenuItem("RIMA/Playable Room/Probe Combat v14 Play")]
    public static void ProbePlayMode()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError("[CombatRoomV14] Probe requires Play mode.");
            return;
        }

        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("[CombatRoomV14] Probe failed: Player not found.");
            return;
        }

        probeRb = player.GetComponent<Rigidbody2D>();
        if (probeRb == null)
        {
            Debug.LogError("[CombatRoomV14] Probe failed: Player Rigidbody2D not found.");
            return;
        }

        probeMovement = player.GetComponent<TestPlayerMovement>();
        if (probeMovement != null)
        {
            probeMovement.enabled = false;
        }

        probeStartPosition = probeRb.position;
        probeStartTime = Time.realtimeSinceStartup;
        probeRb.linearVelocity = new Vector2(0f, 2.2f);
        EditorApplication.update -= ProbeTick;
        EditorApplication.update += ProbeTick;
        Debug.Log("[CombatRoomV14] Probe started: rb.linearVelocity=(0,2.2) for 2 seconds.");
    }

    private static void EnsureTextureFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Sprites/Environment"))
        {
            throw new InvalidOperationException("Assets/Sprites/Environment folder is missing.");
        }

        if (!AssetDatabase.IsValidFolder(TextureFolder))
        {
            AssetDatabase.CreateFolder("Assets/Sprites/Environment", "CombatV14");
        }
    }

    private static void GenerateProceduralAssets()
    {
        SaveTexture(FloorPath, BuildFloorTexture(1152, 704), false);
        SaveTexture(VignettePath, BuildVignetteTexture(1152, 704), true);
        SaveTexture(AshCirclePath, BuildAshCircleTexture(128, 128), true);
        SaveTexture(BloodSplatPath, BuildBloodSplatTexture(128, 128), true);
        SaveTexture(RiftCrackPath, BuildRiftCrackTexture(128, 128), true);
    }

    private static void SaveTexture(string assetPath, Texture2D texture, bool alpha)
    {
        string fullPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, assetPath).Replace('\\', '/');
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllBytes(fullPath, texture.EncodeToPNG());
        UnityEngine.Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer == null)
        {
            throw new InvalidOperationException("Texture importer missing for " + assetPath);
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = Ppu;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.alphaIsTransparency = alpha;
        importer.SaveAndReimport();
    }

    private static Texture2D BuildFloorTexture(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var pixels = new Color32[width * height];
        var spawnPoints = new[]
        {
            new Vector2(10f, 18f),
            new Vector2(26f, 18f),
            new Vector2(10f, 6f),
            new Vector2(26f, 6f),
            new Vector2(18f, 18f)
        };

        for (int y = 0; y < height; y++)
        {
            float wy = y / Ppu;
            for (int x = 0; x < width; x++)
            {
                float wx = x / Ppu;
                float n = Hash01(x / 11, y / 11) * 0.05f + Hash01(x / 43, y / 37) * 0.08f;
                float tileLine = (x % 32 == 0 || y % 32 == 0) ? 0.045f : 0f;
                float centerDist = Vector2.Distance(new Vector2(wx, wy), new Vector2(18f, 11f)) / 18f;
                float vignette = Mathf.Clamp01((centerDist - 0.42f) * 1.25f);
                float red = 0.125f + n - tileLine;
                float green = 0.128f + n * 0.75f - tileLine;
                float blue = 0.142f + n * 0.95f - tileLine;

                float path = Mathf.Exp(-Mathf.Pow((wx - 18f) / 1.4f, 2f)) * SmoothStep(1.5f, 10.8f, wy) * (1f - SmoothStep(12.2f, 15f, wy));
                red += path * 0.035f;
                green += path * 0.025f;
                blue += path * 0.012f;

                foreach (Vector2 point in spawnPoints)
                {
                    float d = Vector2.Distance(new Vector2(wx, wy), point);
                    float blood = Mathf.Clamp01(1f - d / 2.1f) * 0.11f;
                    red += blood;
                    green -= blood * 0.045f;
                    blue -= blood * 0.055f;
                }

                float edgeDark = vignette * 0.105f;
                red -= edgeDark;
                green -= edgeDark;
                blue -= edgeDark;

                pixels[y * width + x] = ToColor32(red, green, blue, 1f);
            }
        }

        DrawFloorCrack(pixels, width, height, new Vector2(8.5f, 5.5f), new Vector2(16f, 10f));
        DrawFloorCrack(pixels, width, height, new Vector2(21f, 12f), new Vector2(29f, 17.5f));
        DrawFloorCrack(pixels, width, height, new Vector2(13.5f, 17.7f), new Vector2(22.5f, 17.2f));
        DrawFloorCrack(pixels, width, height, new Vector2(17.4f, 9.2f), new Vector2(18.8f, 12.3f));

        texture.SetPixels32(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    private static Texture2D BuildVignetteTexture(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var pixels = new Color32[width * height];

        for (int y = 0; y < height; y++)
        {
            float ny = Mathf.Abs((y / (float)(height - 1)) * 2f - 1f);
            for (int x = 0; x < width; x++)
            {
                float nx = Mathf.Abs((x / (float)(width - 1)) * 2f - 1f);
                float edge = Mathf.Pow(Mathf.Clamp01(Mathf.Max(nx * 0.9f, ny)), 2.6f);
                byte a = (byte)Mathf.RoundToInt(edge * 92f);
                pixels[y * width + x] = new Color32(0, 0, 0, a);
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    private static Texture2D BuildAshCircleTexture(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var pixels = new Color32[width * height];
        Vector2 center = new Vector2(width * 0.5f, height * 0.5f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 p = new Vector2(x, y);
                float d = Vector2.Distance(p, center);
                float wobble = Mathf.Sin((x * 12.9898f + y * 78.233f) * 0.025f) * 3f;
                float ring = Mathf.Clamp01(1f - Mathf.Abs(d - 43f - wobble) / 6.5f);
                float inner = Mathf.Clamp01(1f - d / 39f) * 0.18f;
                float fleck = Hash01(x, y) > 0.985f && d < 48f ? 0.45f : 0f;
                byte a = (byte)Mathf.RoundToInt(Mathf.Clamp01(ring * 0.58f + inner + fleck) * 190f);
                byte r = (byte)Mathf.RoundToInt(68f + fleck * 90f);
                byte g = (byte)Mathf.RoundToInt(55f + ring * 25f);
                byte b = (byte)Mathf.RoundToInt(50f + ring * 18f);
                pixels[y * width + x] = new Color32(r, g, b, a);
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    private static Texture2D BuildBloodSplatTexture(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var pixels = new Color32[width * height];
        Vector2 center = new Vector2(width * 0.5f, height * 0.5f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 p = new Vector2(x, y);
                float dx = (p.x - center.x) / 47f;
                float dy = (p.y - center.y) / 34f;
                float d = Mathf.Sqrt(dx * dx + dy * dy);
                float n = Hash01(x / 4, y / 4);
                float body = Mathf.Clamp01(1.05f - d - n * 0.22f);
                float drip = Hash01(x, y / 7) > 0.965f && d < 1.25f ? 0.35f : 0f;
                byte a = (byte)Mathf.RoundToInt(Mathf.Clamp01(body * 0.7f + drip) * 150f);
                pixels[y * width + x] = new Color32(78, 16, 15, a);
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    private static Texture2D BuildRiftCrackTexture(int width, int height)
    {
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        var pixels = new Color32[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(0, 0, 0, 0);
        }

        Vector2[] path =
        {
            new Vector2(20, 69),
            new Vector2(41, 61),
            new Vector2(55, 69),
            new Vector2(72, 55),
            new Vector2(88, 61),
            new Vector2(109, 48)
        };

        for (int i = 0; i < path.Length - 1; i++)
        {
            DrawLine(pixels, width, height, path[i], path[i + 1], 4f, new Color32(21, 4, 3, 170));
            DrawLine(pixels, width, height, path[i], path[i + 1], 1.4f, new Color32(235, 67, 35, 220));
        }

        DrawLine(pixels, width, height, new Vector2(55, 69), new Vector2(49, 91), 1.2f, new Color32(238, 70, 34, 180));
        DrawLine(pixels, width, height, new Vector2(72, 55), new Vector2(76, 35), 1.2f, new Color32(238, 70, 34, 180));

        texture.SetPixels32(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    private static void PreserveRitualReference(Transform playableRoom)
    {
        Transform reference = playableRoom.Find("Pro_Redesign_v13_RitualChamber");
        Transform v13 = playableRoom.Find("Pro_Redesign_v13");
        if (reference == null && v13 != null)
        {
            v13.name = "Pro_Redesign_v13_RitualChamber";
            v13.gameObject.SetActive(false);
            return;
        }

        if (reference != null)
        {
            reference.gameObject.SetActive(false);
        }

        if (v13 != null)
        {
            v13.gameObject.SetActive(false);
        }
    }

    private static void ReplaceActiveCombatRoom(Transform playableRoom)
    {
        Transform old = playableRoom.Find("Pro_Redesign_v14_CombatRoom");
        if (old != null)
        {
            UnityEngine.Object.DestroyImmediate(old.gameObject);
        }

        GameObject root = NewChild(playableRoom, "Pro_Redesign_v14_CombatRoom");
        root.SetActive(true);

        Transform floor = NewChild(root.transform, "01_Floor").transform;
        Transform decals = NewChild(root.transform, "02_Combat_Decals").transform;
        Transform north = NewChild(root.transform, "03_North_Exit").transform;
        Transform cover = NewChild(root.transform, "04_Perimeter_Cover").transform;
        Transform light = NewChild(root.transform, "05_Lighting").transform;
        Transform markers = NewChild(root.transform, "06_Encounter_Markers").transform;
        Transform bounds = NewChild(root.transform, "07_Collision_Bounds").transform;

        AddSprite(floor, "Floor_BloodStone_Combat_v14", FloorPath, new Vector3(18f, 11f, 0f), Vector3.one, 0, Color.white);
        AddSprite(light, "Edge_Vignette_Combat_v14", VignettePath, new Vector3(18f, 11f, 0f), Vector3.one, 95, Color.white);

        AddNorthExit(north);
        AddPerimeterCover(cover);
        AddEncounterMarkers(markers);
        AddEntryTrail(decals);
        AddCenterThreat(decals);
        AddCollisionBounds(bounds);
    }

    private static void AddNorthExit(Transform parent)
    {
        string[] walls =
        {
            "wall_01", "wall_02", "wall_04", "wall_11",
            "wall_05", "wall_07", "wall_08", "wall_10"
        };
        float[] xs = { 11f, 13f, 15f, 18f, 21f, 23f, 25f, 27f };

        for (int i = 0; i < xs.Length; i++)
        {
            string path = "Assets/Sprites/Environment/RIMA_AssetParts_v3/walls/" + walls[i] + ".png";
            GameObject wall = AddSprite(parent, "North_Wall_" + i.ToString("00") + "_" + walls[i], path, new Vector3(xs[i], 20f, 0f), Vector3.one, 520, Color.white);
            if (walls[i] != "wall_11")
            {
                BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(1.7f, 1.05f);
                collider.offset = new Vector2(0f, 0.65f);
            }
        }

        GameObject doorLabel = NewChild(parent, "North_Exit_DoorSocket");
        doorLabel.transform.position = new Vector3(18f, 20.65f, 0f);
    }

    private static void AddPerimeterCover(Transform parent)
    {
        AddCoverSprite(parent, "NW_BrokenColumn_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_02.png", new Vector3(8.2f, 16.0f, 0f), new Vector3(0.82f, 0.82f, 1f), new Vector2(1.05f, 0.85f), new Vector2(0f, 0.55f));
        AddCoverSprite(parent, "NW_Debris_Pile_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_07.png", new Vector3(10.0f, 16.8f, 0f), new Vector3(0.42f, 0.42f, 1f), new Vector2(1.25f, 0.85f), new Vector2(0f, 0.45f));

        AddCoverSprite(parent, "NE_Brazier_ThreatLight", "Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_03.png", new Vector3(27.2f, 16.05f, 0f), new Vector3(0.72f, 0.72f, 1f), new Vector2(0.85f, 0.7f), new Vector2(0f, 0.45f));
        AddPointLight(parent.Find("NE_Brazier_ThreatLight").gameObject, new Color(1f, 0.35f, 0.16f), 1.75f, 4.2f);
        AddCoverSprite(parent, "NE_Column_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_01.png", new Vector3(29.4f, 15.35f, 0f), new Vector3(0.78f, 0.78f, 1f), new Vector2(0.8f, 0.8f), new Vector2(0f, 0.55f));

        AddCoverSprite(parent, "SW_DefeatedStatue_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_05.png", new Vector3(8.4f, 6.0f, 0f), new Vector3(0.76f, 0.76f, 1f), new Vector2(1.15f, 0.9f), new Vector2(0f, 0.5f));
        AddSprite(parent, "SW_Scorch_Telegraph", AshCirclePath, new Vector3(10.2f, 6.15f, 0f), new Vector3(0.58f, 0.44f, 1f), 120, new Color(0.78f, 0.36f, 0.28f, 0.8f));

        AddCoverSprite(parent, "SE_DebrisStack_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_07.png", new Vector3(27.7f, 6.25f, 0f), new Vector3(0.62f, 0.62f, 1f), new Vector2(1.55f, 1.0f), new Vector2(0f, 0.55f));
        AddCoverSprite(parent, "East_BrokenWall_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/walls/wall_08.png", new Vector3(31.2f, 10.9f, 0f), new Vector3(0.92f, 0.92f, 1f), new Vector2(1.55f, 0.95f), new Vector2(0f, 0.6f));
        AddCoverSprite(parent, "West_BrokenWall_Cover", "Assets/Sprites/Environment/RIMA_AssetParts_v3/walls/wall_05.png", new Vector3(5.2f, 11.6f, 0f), new Vector3(0.92f, 0.92f, 1f), new Vector2(1.55f, 0.95f), new Vector2(0f, 0.6f));
    }

    private static void AddEncounterMarkers(Transform parent)
    {
        AddSpawnMarker(parent, "SpawnMarker_NW_Ash", new Vector3(10f, 18f, 0f), 0.68f, "flank,north");
        AddSpawnMarker(parent, "SpawnMarker_NE_Ash", new Vector3(26f, 18f, 0f), 0.68f, "flank,north");
        AddSpawnMarker(parent, "SpawnMarker_SW_BloodAsh", new Vector3(10f, 6f, 0f), 0.62f, "flank,south");
        AddSpawnMarker(parent, "SpawnMarker_SE_BloodAsh", new Vector3(26f, 6f, 0f), 0.62f, "flank,south");
        AddSpawnMarker(parent, "SpawnMarker_NorthCenter_Ash", new Vector3(18f, 18f, 0f), 0.58f, "center,north");

        AddSprite(parent, "BloodDrag_NE", BloodSplatPath, new Vector3(24.8f, 17.1f, 0f), new Vector3(0.44f, 0.32f, 1f), 118, Color.white).transform.rotation = Quaternion.Euler(0f, 0f, 19f);
        AddSprite(parent, "BloodDrag_SW", BloodSplatPath, new Vector3(11.2f, 6.7f, 0f), new Vector3(0.38f, 0.3f, 1f), 118, Color.white).transform.rotation = Quaternion.Euler(0f, 0f, -27f);
    }

    private static void AddSpawnMarker(Transform parent, string name, Vector3 position, float scale, string tags)
    {
        GameObject marker = AddSprite(parent, name, AshCirclePath, position, new Vector3(scale, scale * 0.62f, 1f), 115, Color.white);
        MobSpawnPoint spawn = marker.AddComponent<MobSpawnPoint>();
        spawn.spawnTier = "combat_placeholder";
        spawn.spawnTags = tags;
    }

    private static void AddEntryTrail(Transform parent)
    {
        string[] pebbleSprites =
        {
            "Assets/Sprites/Environment/RIMA_AssetParts_v2/pebbles/pebbles_03.png",
            "Assets/Sprites/Environment/RIMA_AssetParts_v2/pebbles/pebbles_05.png",
            "Assets/Sprites/Environment/RIMA_AssetParts_v2/pebbles/pebbles_09.png"
        };
        Vector3[] positions =
        {
            new Vector3(17.2f, 3.6f, 0f),
            new Vector3(18.7f, 4.7f, 0f),
            new Vector3(17.8f, 6.1f, 0f),
            new Vector3(18.4f, 7.4f, 0f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject pebble = AddSprite(parent, "South_Entry_Pebble_" + i.ToString("00"), pebbleSprites[i % pebbleSprites.Length], positions[i], new Vector3(0.26f, 0.26f, 1f), 110, new Color(0.75f, 0.73f, 0.68f, 0.72f));
            pebble.transform.rotation = Quaternion.Euler(0f, 0f, i * 31f);
        }
    }

    private static void AddCenterThreat(Transform parent)
    {
        AddSprite(parent, "Center_Small_Scorch_NoCollision", "Assets/Sprites/Environment/RIMA_AssetParts_v3/accents/accent_02.png", new Vector3(18f, 11f, 0f), new Vector3(0.34f, 0.26f, 1f), 130, new Color(1f, 0.72f, 0.55f, 0.86f));
        AddSprite(parent, "Center_Red_Rift_Crack_NoCollision", RiftCrackPath, new Vector3(18f, 11.05f, 0f), new Vector3(0.62f, 0.34f, 1f), 145, Color.white);
    }

    private static void AddCollisionBounds(Transform parent)
    {
        AddCollider(parent, "RoomBound_West", new Vector2(2.2f, 11f), new Vector2(1f, 22f));
        AddCollider(parent, "RoomBound_East", new Vector2(33.8f, 11f), new Vector2(1f, 22f));
        AddCollider(parent, "RoomBound_SouthLeft", new Vector2(7.25f, 0.55f), new Vector2(13.5f, 1f));
        AddCollider(parent, "RoomBound_SouthRight", new Vector2(28.75f, 0.55f), new Vector2(13.5f, 1f));
        AddCollider(parent, "RoomBound_NorthLeft", new Vector2(9.8f, 21.15f), new Vector2(13.6f, 1f));
        AddCollider(parent, "RoomBound_NorthRight", new Vector2(26.2f, 21.15f), new Vector2(13.6f, 1f));
    }

    private static void ConfigurePlayer(Transform playableRoom)
    {
        Transform player = playableRoom.Find("Player");
        if (player == null)
        {
            throw new InvalidOperationException("Player was not found under PlayableRoom.");
        }

        player.gameObject.SetActive(true);
        player.position = new Vector3(18f, 2.85f, 0f);
        player.localScale = new Vector3(0.7f, 0.7f, 1f);
        player.tag = "Player";

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 620;
            if (SpriteLit != null)
            {
                sr.sharedMaterial = SpriteLit;
            }
        }
    }

    private static void ConfigureCameraAndDoor(Transform playableRoom)
    {
        GameObject cameraObject = GameObject.Find("Main Camera");
        Transform player = playableRoom.Find("Player");
        if (cameraObject != null && player != null)
        {
            cameraObject.transform.position = new Vector3(18f, 11.15f, -10f);
            Camera camera = cameraObject.GetComponent<Camera>();
            if (camera != null)
            {
                camera.orthographic = true;
                camera.orthographicSize = 10.6f;
                camera.backgroundColor = new Color(0.018f, 0.017f, 0.022f, 1f);
            }

            TestCameraFollow follow = cameraObject.GetComponent<TestCameraFollow>();
            if (follow != null)
            {
                follow.target = player;
                follow.offset = new Vector3(0f, 8.3f, -10f);
                follow.lerp = 0.15f;
            }
        }

        Transform door = playableRoom.Find("DoorExit");
        if (door != null)
        {
            door.gameObject.SetActive(true);
            door.position = new Vector3(18f, 20.55f, 0f);
            BoxCollider2D collider = door.GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                collider = door.gameObject.AddComponent<BoxCollider2D>();
            }

            collider.isTrigger = true;
            collider.size = new Vector2(2.25f, 1.15f);
            collider.offset = new Vector2(0f, 0.15f);
        }
    }

    private static void ConfigureLighting()
    {
        GameObject globalObject = GameObject.Find("Global Light 2D");
        if (globalObject != null)
        {
            Light2D light = globalObject.GetComponent<Light2D>();
            if (light != null)
            {
                light.lightType = Light2D.LightType.Global;
                light.color = new Color(0.55f, 0.61f, 0.72f, 1f);
                light.intensity = 0.68f;
            }
        }

        GameObject mainLight = GameObject.Find("Main Light");
        if (mainLight != null)
        {
            Light light = mainLight.GetComponent<Light>();
            if (light != null)
            {
                light.color = new Color(0.72f, 0.76f, 0.84f, 1f);
                light.intensity = 0.55f;
            }
        }
    }

    private static int RemoveMissingScriptsInScene()
    {
        int removed = 0;
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject go = objects[i];
            if (!go.scene.IsValid() || go.scene != EditorSceneManager.GetActiveScene())
            {
                continue;
            }

            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
            if (count <= 0)
            {
                continue;
            }

            Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            removed += count;
        }

        return removed;
    }

    private static void ProbeTick()
    {
        if (!EditorApplication.isPlaying || probeRb == null)
        {
            CleanupProbe();
            return;
        }

        if (Time.realtimeSinceStartup - probeStartTime < 2f)
        {
            return;
        }

        Vector2 endPosition = probeRb.position;
        float distance = Vector2.Distance(probeStartPosition, endPosition);
        probeRb.linearVelocity = Vector2.zero;

        Camera camera = Camera.main;
        TestCameraFollow follow = camera != null ? camera.GetComponent<TestCameraFollow>() : null;
        bool cameraTargetsPlayer = follow != null && follow.target == probeRb.transform;

        if (probeMovement != null)
        {
            probeMovement.enabled = true;
        }

        if (distance < 1.2f)
        {
            Debug.LogError("[CombatRoomV14] Probe failed: player moved only " + distance.ToString("0.00") + " units.");
        }
        else if (!cameraTargetsPlayer)
        {
            Debug.LogError("[CombatRoomV14] Probe failed: camera follow target is not Player.");
        }
        else
        {
            Debug.Log("[CombatRoomV14] Probe PASS: player moved " + distance.ToString("0.00") + " units and camera follows Player.");
        }

        CleanupProbe();
    }

    private static void CleanupProbe()
    {
        EditorApplication.update -= ProbeTick;
        probeRb = null;
        probeMovement = null;
    }

    private static GameObject AddCoverSprite(Transform parent, string name, string spritePath, Vector3 position, Vector3 scale, Vector2 colliderSize, Vector2 colliderOffset)
    {
        GameObject go = AddSprite(parent, name, spritePath, position, scale, Mathf.RoundToInt(650f - position.y * 5f), Color.white);
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.size = colliderSize;
        collider.offset = colliderOffset;
        return go;
    }

    private static void AddPointLight(GameObject target, Color color, float intensity, float radius)
    {
        Light2D light = target.GetComponent<Light2D>();
        if (light == null)
        {
            light = target.AddComponent<Light2D>();
        }

        light.lightType = Light2D.LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.pointLightOuterRadius = radius;
        light.pointLightInnerRadius = radius * 0.25f;
    }

    private static GameObject AddSprite(Transform parent, string name, string spritePath, Vector3 position, Vector3 scale, int order, Color color)
    {
        GameObject go = NewChild(parent, name);
        go.transform.position = position;
        go.transform.localScale = scale;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = LoadSprite(spritePath);
        sr.sortingOrder = order;
        sr.color = color;
        if (SpriteLit != null)
        {
            sr.sharedMaterial = SpriteLit;
        }

        return go;
    }

    private static GameObject NewChild(Transform parent, string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        return go;
    }

    private static void AddCollider(Transform parent, string name, Vector2 center, Vector2 size)
    {
        GameObject go = NewChild(parent, name);
        go.transform.position = center;
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.size = size;
    }

    private static Sprite LoadSprite(string path)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite != null)
        {
            return sprite;
        }

        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        for (int i = 0; i < assets.Length; i++)
        {
            sprite = assets[i] as Sprite;
            if (sprite != null)
            {
                return sprite;
            }
        }

        throw new InvalidOperationException("Sprite missing at " + path);
    }

    private static void DrawFloorCrack(Color32[] pixels, int width, int height, Vector2 worldA, Vector2 worldB)
    {
        Vector2 a = worldA * Ppu;
        Vector2 b = worldB * Ppu;
        DrawLine(pixels, width, height, a, b, 1.2f, new Color32(20, 17, 16, 120));
        DrawLine(pixels, width, height, a + new Vector2(3f, -2f), b + new Vector2(-4f, 2f), 0.6f, new Color32(78, 44, 36, 70));
    }

    private static void DrawLine(Color32[] pixels, int width, int height, Vector2 a, Vector2 b, float radius, Color32 color)
    {
        int minX = Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(a.x, b.x) - radius - 2f), 0, width - 1);
        int maxX = Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(a.x, b.x) + radius + 2f), 0, width - 1);
        int minY = Mathf.Clamp(Mathf.FloorToInt(Mathf.Min(a.y, b.y) - radius - 2f), 0, height - 1);
        int maxY = Mathf.Clamp(Mathf.CeilToInt(Mathf.Max(a.y, b.y) + radius + 2f), 0, height - 1);
        Vector2 ab = b - a;
        float abLenSq = Mathf.Max(0.0001f, ab.sqrMagnitude);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Vector2 p = new Vector2(x, y);
                float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / abLenSq);
                Vector2 closest = a + ab * t;
                float d = Vector2.Distance(p, closest);
                if (d > radius)
                {
                    continue;
                }

                int idx = y * width + x;
                pixels[idx] = AlphaBlend(pixels[idx], color);
            }
        }
    }

    private static Color32 AlphaBlend(Color32 dst, Color32 src)
    {
        float a = src.a / 255f;
        float inv = 1f - a;
        return new Color32(
            (byte)Mathf.RoundToInt(src.r * a + dst.r * inv),
            (byte)Mathf.RoundToInt(src.g * a + dst.g * inv),
            (byte)Mathf.RoundToInt(src.b * a + dst.b * inv),
            (byte)Mathf.Clamp(Mathf.RoundToInt(src.a + dst.a * inv), 0, 255));
    }

    private static float SmoothStep(float edge0, float edge1, float x)
    {
        float t = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
        return t * t * (3f - 2f * t);
    }

    private static float Hash01(int x, int y)
    {
        unchecked
        {
            uint h = (uint)(x * 374761393 + y * 668265263);
            h = (h ^ (h >> 13)) * 1274126177u;
            return (h ^ (h >> 16)) / 4294967295f;
        }
    }

    private static Color32 ToColor32(float r, float g, float b, float a)
    {
        return new Color32(
            (byte)Mathf.RoundToInt(Mathf.Clamp01(r) * 255f),
            (byte)Mathf.RoundToInt(Mathf.Clamp01(g) * 255f),
            (byte)Mathf.RoundToInt(Mathf.Clamp01(b) * 255f),
            (byte)Mathf.RoundToInt(Mathf.Clamp01(a) * 255f));
    }
}
