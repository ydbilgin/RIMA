#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Map;

namespace RIMA.Editor.Map
{
    public static class RoomLoaderMenu
    {
        private const string MenuRoot = "RIMA/Map/";
        private const string TestJsonPath = "Assets/Data/Map/Act1_ShatteredKeep/json/act1_entry_hall.json";
        private const string TestPoolPath = "Assets/Data/Map/Act1_ShatteredKeep/MaterialPool_Act1_ShatteredKeep.asset";
        private const string TestRoomManifestPath = "Assets/Data/Map/Act1_ShatteredKeep/RoomManifest_EntryHall.asset";
        private const string TestWallRegistryPath = "Assets/Data/Map/Act1_ShatteredKeep/WallPrefabRegistry_Act1.asset";
        private const string TestScenePath = "Assets/Scenes/Demo/TopDownTest_Map1.unity";
        private const string ScreenshotPath = "Assets/Screenshots/Phase_H_entry_hall_loaded.png";

        [MenuItem(MenuRoot + "Load Room JSON to Scene")]
        public static void LoadRoomJsonToScene()
        {
            string path = EditorUtility.OpenFilePanel("Load Room Layout JSON", Application.dataPath + "/Data/Map", "json");
            if (string.IsNullOrEmpty(path)) return;

            LoadRoomJsonToActiveScene(path);
        }

        [MenuItem(MenuRoot + "Validate Room JSON")]
        public static void ValidateRoomJson()
        {
            string path = EditorUtility.OpenFilePanel("Validate Room Layout JSON", Application.dataPath + "/Data/Map", "json");
            if (!string.IsNullOrEmpty(path)) RoomLayoutValidator.Validate(path);
        }

        [MenuItem(MenuRoot + "Create Phase H Test Assets")]
        public static void CreatePhaseHTestAssets()
        {
            EnsureFolders();
            File.Copy(FullPath("STAGING/map_schema_v1.json"), FullPath("Assets/Data/Map/Schemas/room_v1.schema.json"), true);
            ExtractEntryHallJson();
            AssetDatabase.Refresh();

            AssetDatabase.StartAssetEditing();
            try
            {
                CreateMaterialPool();
                CreateWallRegistry();
                CreateRoomManifest();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[PhaseH] Test assets created.");
        }

        public static bool LoadRoomJsonToActiveScene(string path)
        {
            Tilemap floorMap = ResolveFloorTilemap();
            if (floorMap == null)
            {
                Debug.LogError("[RoomLoaderMenu] No active scene Tilemap found.");
                return false;
            }

            Transform parent = ResolveRoomContentParent();
            MaterialVariantPoolSO pool = FindFirstAsset<MaterialVariantPoolSO>();
            WallPrefabRegistry registry = FindFirstAsset<WallPrefabRegistry>();
            RoomInstance room = RoomLoader.LoadJsonToScene(path, floorMap, parent, pool, registry);
            if (room == null) return false;

            EditorUtility.SetDirty(floorMap);
            EditorSceneManager.MarkSceneDirty(floorMap.gameObject.scene);
            return true;
        }

        public static void RunPhaseHBatch()
        {
            CreatePhaseHTestAssets();
            bool valid = RoomLayoutValidator.Validate(TestJsonPath, AssetDatabase.LoadAssetAtPath<MaterialVariantPoolSO>(TestPoolPath), AssetDatabase.LoadAssetAtPath<WallPrefabRegistry>(TestWallRegistryPath));
            if (!valid) throw new System.InvalidOperationException("Phase H validation failed.");

            EditorSceneManager.OpenScene(TestScenePath);
            Tilemap floorMap = ResolveFloorTilemap();
            if (floorMap == null) floorMap = CreateFloorTilemap();

            bool loaded = LoadRoomJsonToActiveScene(TestJsonPath);
            if (!loaded) throw new System.InvalidOperationException("Phase H JSON load failed.");

            EditorSceneManager.SaveOpenScenes();
            CaptureScenePng(ScreenshotPath, floorMap);
            AssetDatabase.Refresh();
            Debug.Log("[PhaseH] Batch complete. Screenshot: " + ScreenshotPath);
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Data");
            EnsureFolder("Assets/Data/Map");
            EnsureFolder("Assets/Data/Map/Schemas");
            EnsureFolder("Assets/Data/Map/Act1_ShatteredKeep");
            EnsureFolder("Assets/Data/Map/Act1_ShatteredKeep/json");
            EnsureFolder("Assets/Screenshots");
        }

        private static void ExtractEntryHallJson()
        {
            string manifestText = File.ReadAllText(FullPath("STAGING/act1_shattered_keep_layout_v1.json"));
            RoomManifestJson manifest = JsonUtility.FromJson<RoomManifestJson>(manifestText);
            RoomLayoutJson room = manifest != null && manifest.rooms != null ? manifest.rooms.FirstOrDefault(x => x.room_id == "act1_entry_hall") : null;
            if (room == null) throw new System.InvalidOperationException("act1_entry_hall not found in staging manifest.");

            File.WriteAllText(FullPath(TestJsonPath), JsonUtility.ToJson(room, true));
        }

        private static void CreateMaterialPool()
        {
            MaterialVariantPoolSO pool = AssetDatabase.LoadAssetAtPath<MaterialVariantPoolSO>(TestPoolPath);
            if (pool == null)
            {
                pool = ScriptableObject.CreateInstance<MaterialVariantPoolSO>();
                AssetDatabase.CreateAsset(pool, TestPoolPath);
            }

            pool.materials = new[]
            {
                new MaterialVariantPoolSO.MaterialEntry { materialId = "granite", variants = LoadTiles("Assets/Tiles/Act1/floor_base_01.asset", "Assets/Tiles/Act1/floor_base_02.asset", "Assets/Tiles/Act1/floor_base_03.asset", "Assets/Tiles/Act1/floor_base_04.asset") },
                new MaterialVariantPoolSO.MaterialEntry { materialId = "walkway", variants = LoadTiles("Assets/Tiles/Act1/floor_crack_01.asset", "Assets/Tiles/Act1/floor_crack_02.asset", "Assets/Tiles/Act1/floor_crack_03.asset", "Assets/Tiles/Act1/floor_crack_04.asset") },
                new MaterialVariantPoolSO.MaterialEntry { materialId = "rift", variants = LoadTiles("Assets/Tiles/Act1/pit_dark.asset", "Assets/Tiles/Act1/pit_edge.asset") },
                new MaterialVariantPoolSO.MaterialEntry { materialId = "rubble", variants = LoadTiles("Assets/Tiles/Act1/decor_debris_01.asset", "Assets/Tiles/Act1/decor_debris_02.asset", "Assets/Tiles/Act1/decor_debris_03.asset", "Assets/Tiles/Act1/decor_debris_04.asset") },
            };
            EditorUtility.SetDirty(pool);
        }

        private static void CreateWallRegistry()
        {
            WallPrefabRegistry registry = AssetDatabase.LoadAssetAtPath<WallPrefabRegistry>(TestWallRegistryPath);
            if (registry == null)
            {
                registry = ScriptableObject.CreateInstance<WallPrefabRegistry>();
                AssetDatabase.CreateAsset(registry, TestWallRegistryPath);
            }

            registry.walls = new WallPrefabRegistry.WallEntry[0];
            EditorUtility.SetDirty(registry);
        }

        private static void CreateRoomManifest()
        {
            AssetDatabase.ImportAsset(TestJsonPath);
            RoomManifestSO manifest = AssetDatabase.LoadAssetAtPath<RoomManifestSO>(TestRoomManifestPath);
            if (manifest == null)
            {
                manifest = ScriptableObject.CreateInstance<RoomManifestSO>();
                AssetDatabase.CreateAsset(manifest, TestRoomManifestPath);
            }

            manifest.roomId = "act1_entry_hall";
            manifest.jsonLayout = AssetDatabase.LoadAssetAtPath<TextAsset>(TestJsonPath);
            manifest.defaultCameraBounds = new Vector2Int(32, 24);
            manifest.connectedRooms = new RoomManifestSO[0];
            EditorUtility.SetDirty(manifest);
        }

        private static TileBase[] LoadTiles(params string[] paths)
        {
            return paths.Select(AssetDatabase.LoadAssetAtPath<TileBase>).Where(x => x != null).ToArray();
        }

        private static Tilemap ResolveFloorTilemap()
        {
            Tilemap[] tilemaps = Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            return tilemaps.FirstOrDefault(x => x.name.Contains("Floor") || x.name.Contains("Zemin") || x.name.Contains("L1")) ?? tilemaps.FirstOrDefault();
        }

        private static Tilemap CreateFloorTilemap()
        {
            GameObject gridGo = GameObject.Find("Grid") ?? new GameObject("Grid", typeof(Grid));
            GameObject floorGo = new GameObject("L1_Floor", typeof(Tilemap), typeof(TilemapRenderer));
            floorGo.transform.SetParent(gridGo.transform, false);
            return floorGo.GetComponent<Tilemap>();
        }

        private static Transform ResolveRoomContentParent()
        {
            GameObject existing = GameObject.Find("PhaseH_RoomContent");
            if (existing != null) Object.DestroyImmediate(existing);

            GameObject root = new GameObject("PhaseH_RoomContent");
            return root.transform;
        }

        private static void CaptureScenePng(string assetPath, Tilemap floorMap)
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                GameObject cameraGo = new GameObject("PhaseH_ScreenshotCamera", typeof(Camera));
                camera = cameraGo.GetComponent<Camera>();
                camera.tag = "MainCamera";
            }

            camera.orthographic = true;
            camera.orthographicSize = 14f;
            camera.transform.position = new Vector3(16f, 12f, -10f);
            camera.transform.rotation = Quaternion.identity;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.08f, 0.08f, 0.09f, 1f);

            RenderTexture rt = new RenderTexture(1024, 768, 24);
            RenderTexture previous = RenderTexture.active;
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            camera.Render();

            Texture2D image = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            image.Apply();

            File.WriteAllBytes(FullPath(assetPath), image.EncodeToPNG());

            camera.targetTexture = null;
            RenderTexture.active = previous;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(image);
            EditorUtility.SetDirty(floorMap);
        }

        private static T FindFirstAsset<T>() where T : Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            if (guids == null || guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            string name = Path.GetFileName(path);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }

        private static string FullPath(string path)
        {
            string normalized = path.Replace('\\', '/');
            if (normalized.StartsWith("Assets/"))
            {
                return Path.Combine(Application.dataPath, normalized.Substring("Assets/".Length));
            }

            return Path.GetFullPath(normalized);
        }
    }
}
#endif
