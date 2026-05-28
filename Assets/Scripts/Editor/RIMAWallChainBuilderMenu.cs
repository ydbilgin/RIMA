using System;
using System.Collections.Generic;
using System.IO;
using RIMA.Rooms;
using RIMA.Walls;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using RandomTile = UnityEngine.Tilemaps.RandomTile;

namespace RIMA.Editor
{
    public static class RIMAWallChainBuilderMenu
    {
        private const string WallPrefabFolder = "Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/HighTopDown_3_4";
        private const string WallSpriteFolder = "Assets/Art/Walls/Act1_ShatteredKeep/HighTopDown_3_4";
        private const string WallDataFolder = "Assets/Data/Walls/Act1_ShatteredKeep/HighTopDown_3_4";
        private const string WallLibraryPath = "Assets/Data/Walls/Act1_ShatteredKeep_Library.asset";
        private const string RoomDataFolder = "Assets/Data/Rooms";
        private const string SampleFootprintPath = "Assets/Data/Rooms/SampleRoomFootprint.asset";
        private const string TileDataFolder = "Assets/Data/Tiles/Act1_ShatteredKeep/HighTopDown_3_4";
        private const string FloorRandomTilePath = "Assets/Data/Tiles/Act1_ShatteredKeep/HighTopDown_3_4/iso_floor_random.asset";
        private const string ScenePath = "Assets/Scenes/Demo/DiamondRoom_v1.unity";
        private const string TemplatePrefabPath = "Assets/Prefabs/Environment/Walls/_template/WallChunk_Template.prefab";
        private const string ScreenshotPath = "STAGING/screenshots/diamond_room_v1.png";

        private static readonly string[] FloorSpriteNames =
        {
            "iso_floor_broken",
            "iso_floor_clean",
            "iso_floor_cracked",
            "iso_floor_debris",
            "iso_floor_edge_light",
            "iso_floor_rift_glow"
        };

        // [MenuItem removed — diamond room diagnostic, archived]
        public static void BuildTestDiamondRoom()
        {
            EnsureSortingLayers();
            EnsureTileAssets();
            List<ChunkSpec> specs = BuildSpecs();
            WallChunkLibrary library = EnsureWallDataPrefabsAndLibrary(specs);
            RoomFootprintPolygon footprint = EnsureSampleFootprint();
            CreateDiamondRoomScene(footprint, library);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("RIMA wall chain test diamond room built.");
        }

        // [MenuItem removed — diamond room screenshot diagnostic, archived]
        public static void CaptureDiamondRoomScreenshot()
        {
            if (SceneManager.GetActiveScene().path != ScenePath)
            {
                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            }

            Camera camera = Camera.main ?? Object.FindFirstObjectByType<Camera>();
            if (camera == null)
            {
                Debug.LogError("No camera found for diamond room screenshot.");
                return;
            }

            string absolutePath = ProjectPath(ScreenshotPath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));

            var rt = new RenderTexture(1280, 720, 24);
            RenderTexture previous = RenderTexture.active;
            RenderTexture previousCamera = camera.targetTexture;
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            camera.Render();

            var image = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            image.Apply();
            File.WriteAllBytes(absolutePath, image.EncodeToPNG());

            camera.targetTexture = previousCamera;
            RenderTexture.active = previous;
            Object.DestroyImmediate(image);
            Object.DestroyImmediate(rt);
            AssetDatabase.Refresh();
            Debug.Log($"Diamond room screenshot saved: {ScreenshotPath}");
        }

        private static List<ChunkSpec> BuildSpecs()
        {
            return new List<ChunkSpec>
            {
                new("wall_nw_mid_plain", "nw_mid_plain", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_nw_mid_variant", "nw_mid_variant", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_nw_mid_broken", "nw_mid_broken", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_nw_doorway", "nw_doorway", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_ne_mid_plain", "ne_mid_plain", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_ne_mid_variant", "ne_mid_variant", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_ne_mid_broken", "ne_mid_broken", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_ne_doorway", "ne_doorway", WallType.WallSpan_Short, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_n_corner", "n_corner", WallType.Connector_CornerOuter, OneCell(), new Vector2(0f, -0.5f), true),
                new("wall_n_landmark", "n_landmark", WallType.Landmark, TwoCells(), new Vector2(0f, -0.5f), true),
                new("wall_pillar_universal", "pillar", WallType.Pillar, OneCell(), new Vector2(0f, -0.5f), true),
                new("iso_floor_broken", "iso_floor_broken", WallType.Landmark, OneCell(), Vector2.zero, false),
                new("iso_floor_clean", "iso_floor_clean", WallType.Landmark, OneCell(), Vector2.zero, false),
                new("iso_floor_cracked", "iso_floor_cracked", WallType.Landmark, OneCell(), Vector2.zero, false),
                new("iso_floor_debris", "iso_floor_debris", WallType.Landmark, OneCell(), Vector2.zero, false),
                new("iso_floor_edge_light", "iso_floor_edge_light", WallType.Landmark, OneCell(), Vector2.zero, false),
                new("iso_floor_rift_glow", "iso_floor_rift_glow", WallType.Landmark, OneCell(), Vector2.zero, false)
            };
        }

        private static List<Vector2Int> OneCell()
        {
            return new List<Vector2Int> { Vector2Int.zero };
        }

        private static List<Vector2Int> TwoCells()
        {
            return new List<Vector2Int> { Vector2Int.zero, new Vector2Int(1, 0) };
        }

        private static void EnsureSortingLayers()
        {
            EnsureSortingLayer("Floor");
            EnsureSortingLayer("Walls");
            EnsureSortingLayer("Characters");
            EnsureSortingLayer("Props");
            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            GraphicsSettings.transparencySortAxis = new Vector3(0f, 1f, 0f);
        }

        private static void EnsureSortingLayer(string layerName)
        {
            Object[] tagManagerAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            var tagManager = new SerializedObject(tagManagerAssets[0]);
            SerializedProperty sortingLayers = tagManager.FindProperty("m_SortingLayers");

            for (int i = 0; i < sortingLayers.arraySize; i++)
            {
                SerializedProperty existing = sortingLayers.GetArrayElementAtIndex(i);
                if (existing.FindPropertyRelative("name").stringValue == layerName)
                {
                    return;
                }
            }

            int index = sortingLayers.arraySize;
            sortingLayers.InsertArrayElementAtIndex(index);
            SerializedProperty layerProperty = sortingLayers.GetArrayElementAtIndex(index);
            layerProperty.FindPropertyRelative("name").stringValue = layerName;
            layerProperty.FindPropertyRelative("uniqueID").intValue = SortingLayerId(layerName);
            layerProperty.FindPropertyRelative("locked").boolValue = false;
            tagManager.ApplyModifiedProperties();
        }

        private static int SortingLayerId(string layerName)
        {
            return layerName switch
            {
                "Floor" => 1843609376,
                "Walls" => 593505845,
                "Characters" => 1200000003,
                "Props" => 1200000004,
                _ => 1200000099
            };
        }

        private static void EnsureTileAssets()
        {
            EnsureFolder(TileDataFolder);
            var floorSprites = new List<Sprite>();

            foreach (string spriteName in FloorSpriteNames)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{WallSpriteFolder}/{spriteName}.png");
                if (sprite == null)
                {
                    Debug.LogWarning($"Missing floor sprite: {spriteName}");
                    continue;
                }

                floorSprites.Add(sprite);
                string tilePath = $"{TileDataFolder}/{spriteName}_tile.asset";
                Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
                if (tile == null)
                {
                    tile = ScriptableObject.CreateInstance<Tile>();
                    AssetDatabase.CreateAsset(tile, tilePath);
                }

                tile.sprite = sprite;
                tile.colliderType = Tile.ColliderType.None;
                EditorUtility.SetDirty(tile);
            }

            RandomTile randomTile = AssetDatabase.LoadAssetAtPath<RandomTile>(FloorRandomTilePath);
            if (randomTile == null)
            {
                randomTile = ScriptableObject.CreateInstance<RandomTile>();
                AssetDatabase.CreateAsset(randomTile, FloorRandomTilePath);
            }

            randomTile.sprites = floorSprites.ToArray();
            randomTile.weights = BuildWeights(floorSprites.Count);
            randomTile.colliderType = Tile.ColliderType.None;
            EditorUtility.SetDirty(randomTile);
        }

        private static float[] BuildWeights(int count)
        {
            float[] weights = new float[count];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = 1f;
            }

            return weights;
        }

        private static WallChunkLibrary EnsureWallDataPrefabsAndLibrary(List<ChunkSpec> specs)
        {
            EnsureFolder(WallDataFolder);
            EnsureFolder("Assets/Data/Walls");
            EnsureTemplatePrefab();

            var entries = new List<WallChunkLibrary.LibEntry>();
            foreach (ChunkSpec spec in specs)
            {
                string prefabPath = $"{WallPrefabFolder}/{spec.prefabName}.prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning($"Missing wall prefab: {prefabPath}");
                    continue;
                }

                WallChunkData data = EnsureWallChunkData(spec, prefab);
                RetrofitPrefab(prefabPath, data, spec);
                entries.Add(new WallChunkLibrary.LibEntry { data = data, prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) });
            }

            WallChunkLibrary library = AssetDatabase.LoadAssetAtPath<WallChunkLibrary>(WallLibraryPath);
            if (library == null)
            {
                library = ScriptableObject.CreateInstance<WallChunkLibrary>();
                AssetDatabase.CreateAsset(library, WallLibraryPath);
            }

            library.entries = entries;
            EditorUtility.SetDirty(library);
            return library;
        }

        private static WallChunkData EnsureWallChunkData(ChunkSpec spec, GameObject prefab)
        {
            string dataPath = $"{WallDataFolder}/{spec.chunkId}.asset";
            WallChunkData data = AssetDatabase.LoadAssetAtPath<WallChunkData>(dataPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<WallChunkData>();
                AssetDatabase.CreateAsset(data, dataPath);
            }

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{WallSpriteFolder}/{spec.prefabName}.png");
            if (sprite == null)
            {
                SpriteRenderer prefabRenderer = prefab.GetComponent<SpriteRenderer>();
                sprite = prefabRenderer != null ? prefabRenderer.sprite : null;
            }

            data.chunkId = spec.chunkId;
            data.wallType = spec.wallType;
            data.heightVariant = WallHeight.Normal;
            data.visual = sprite;
            data.footprintCells = new List<Vector2Int>(spec.footprintCells);
            data.anchorOffset = spec.anchorOffset;
            data.sockets = new List<SocketDef>();
            data.colliderSize = spec.usesCollider && spec.footprintCells.Count > 1 ? new Vector2(4f, 1f) : new Vector2(2f, 1f);
            data.colliderOffset = Vector2.zero;
            EditorUtility.SetDirty(data);
            return data;
        }

        private static void RetrofitPrefab(string prefabPath, WallChunkData data, ChunkSpec spec)
        {
            GameObject root = PrefabUtility.LoadPrefabContents(prefabPath);
            try
            {
                SpriteRenderer renderer = root.GetComponent<SpriteRenderer>();
                if (renderer == null)
                {
                    renderer = root.AddComponent<SpriteRenderer>();
                }

                renderer.sprite = data.visual;
                renderer.sortingLayerName = spec.usesCollider ? "Walls" : "Floor";

                BoxCollider2D collider = root.GetComponent<BoxCollider2D>();
                if (spec.usesCollider)
                {
                    if (collider == null)
                    {
                        collider = root.AddComponent<BoxCollider2D>();
                    }

                    collider.size = data.colliderSize;
                    collider.offset = data.colliderOffset;
                }

                WallChunk chunk = root.GetComponent<WallChunk>();
                if (chunk == null)
                {
                    chunk = root.AddComponent<WallChunk>();
                }

                Transform footprintAnchor = EnsureChild(root.transform, "FootprintAnchor");
                Transform leftSocket = EnsureChild(root.transform, "LeftSocket");
                Transform rightSocket = EnsureChild(root.transform, "RightSocket");
                Transform torchSocket = EnsureChild(root.transform, "TorchSocket");
                Transform bannerSocket = EnsureChild(root.transform, "BannerSocket");
                Transform seamSocket = EnsureChild(root.transform, "SeamSocket");
                footprintAnchor.localPosition = data.anchorOffset;

                var serializedChunk = new SerializedObject(chunk);
                serializedChunk.FindProperty("data").objectReferenceValue = data;
                serializedChunk.FindProperty("visualRenderer").objectReferenceValue = renderer;
                serializedChunk.FindProperty("footprintCollider").objectReferenceValue = collider;
                serializedChunk.FindProperty("footprintAnchor").objectReferenceValue = footprintAnchor;
                serializedChunk.FindProperty("leftSocket").objectReferenceValue = leftSocket;
                serializedChunk.FindProperty("rightSocket").objectReferenceValue = rightSocket;
                serializedChunk.FindProperty("torchSocket").objectReferenceValue = torchSocket;
                serializedChunk.FindProperty("bannerSocket").objectReferenceValue = bannerSocket;
                serializedChunk.FindProperty("seamSocket").objectReferenceValue = seamSocket;
                serializedChunk.ApplyModifiedPropertiesWithoutUndo();
                chunk.ApplyData();

                PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        private static Transform EnsureChild(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child != null)
            {
                return child;
            }

            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go.transform;
        }

        private static void EnsureTemplatePrefab()
        {
            EnsureFolder("Assets/Prefabs/Environment/Walls/_template");
            if (AssetDatabase.LoadAssetAtPath<GameObject>(TemplatePrefabPath) != null)
            {
                return;
            }

            var root = new GameObject("WallChunk_Template");
            root.AddComponent<SpriteRenderer>();
            root.AddComponent<BoxCollider2D>();
            root.AddComponent<WallChunk>();
            EnsureChild(root.transform, "FootprintAnchor");
            EnsureChild(root.transform, "LeftSocket");
            EnsureChild(root.transform, "RightSocket");
            EnsureChild(root.transform, "TorchSocket");
            EnsureChild(root.transform, "BannerSocket");
            EnsureChild(root.transform, "SeamSocket");
            PrefabUtility.SaveAsPrefabAsset(root, TemplatePrefabPath);
            Object.DestroyImmediate(root);
        }

        private static RoomFootprintPolygon EnsureSampleFootprint()
        {
            EnsureFolder(RoomDataFolder);
            RoomFootprintPolygon footprint = AssetDatabase.LoadAssetAtPath<RoomFootprintPolygon>(SampleFootprintPath);
            if (footprint == null)
            {
                footprint = ScriptableObject.CreateInstance<RoomFootprintPolygon>();
                AssetDatabase.CreateAsset(footprint, SampleFootprintPath);
            }

            footprint.vertices = new List<Vector2Int>
            {
                new Vector2Int(0, 0),
                new Vector2Int(4, 1),
                new Vector2Int(5, 3),
                new Vector2Int(4, 5),
                new Vector2Int(0, 5),
                new Vector2Int(-1, 3)
            };
            footprint.openEdgeIndices = new List<int> { 0 };
            footprint.entryPoints = new List<Vector2Int> { new Vector2Int(2, 0) };
            EditorUtility.SetDirty(footprint);
            return footprint;
        }

        private static void CreateDiamondRoomScene(RoomFootprintPolygon footprint, WallChunkLibrary library)
        {
            EnsureFolder("Assets/Scenes/Demo");
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject worldRoot = new GameObject("WorldRoot");
            GameObject gridGo = new GameObject("Grid");
            gridGo.transform.SetParent(worldRoot.transform, false);
            Grid grid = gridGo.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.IsometricZAsY;
            grid.cellSize = new Vector3(1f, 0.5f, 1f);
            grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;

            GameObject floorGo = new GameObject("Floor Tilemap");
            floorGo.transform.SetParent(gridGo.transform, false);
            Tilemap floorTilemap = floorGo.AddComponent<Tilemap>();
            TilemapRenderer floorRenderer = floorGo.AddComponent<TilemapRenderer>();
            floorRenderer.mode = TilemapRenderer.Mode.Chunk;
            floorRenderer.sortingLayerName = "Floor";
            FillFloor(floorTilemap, footprint);

            GameObject wallsGo = new GameObject("Walls");
            wallsGo.transform.SetParent(worldRoot.transform, false);
            WallChainBuilder.Build(footprint, library, wallsGo.transform);

            CreateWarbladePreview(worldRoot.transform);
            CreateLighting(worldRoot.transform);
            CreateCamera();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }

        private static void FillFloor(Tilemap tilemap, RoomFootprintPolygon footprint)
        {
            TileBase floorTile = AssetDatabase.LoadAssetAtPath<TileBase>(FloorRandomTilePath);
            if (floorTile == null)
            {
                return;
            }

            BoundsInt bounds = GetBounds(footprint.vertices);
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
                {
                    if (PointInPolygon(new Vector2(x + 0.5f, y + 0.5f), footprint.vertices))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                    }
                }
            }
        }

        private static BoundsInt GetBounds(List<Vector2Int> points)
        {
            int minX = points[0].x;
            int maxX = points[0].x;
            int minY = points[0].y;
            int maxY = points[0].y;
            foreach (Vector2Int point in points)
            {
                minX = Mathf.Min(minX, point.x);
                maxX = Mathf.Max(maxX, point.x);
                minY = Mathf.Min(minY, point.y);
                maxY = Mathf.Max(maxY, point.y);
            }

            return new BoundsInt(minX, minY, 0, maxX - minX, maxY - minY, 1);
        }

        private static bool PointInPolygon(Vector2 point, List<Vector2Int> vertices)
        {
            bool inside = false;
            for (int i = 0, j = vertices.Count - 1; i < vertices.Count; j = i++)
            {
                Vector2 a = vertices[i];
                Vector2 b = vertices[j];
                if (((a.y > point.y) != (b.y > point.y)) &&
                    point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private static void CreateWarbladePreview(Transform parent)
        {
            var go = new GameObject("Warblade");
            go.transform.SetParent(parent, false);
            go.transform.position = WallChainBuilder.GridToWorld(new Vector2Int(2, 1));
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Warblade/Rotations/warblade_south.png");
            renderer.sortingLayerName = "Characters";
            renderer.sortingOrder = 100;
        }

        private static void CreateLighting(Transform parent)
        {
            GameObject lighting = new GameObject("Lighting");
            lighting.transform.SetParent(parent, false);

            GameObject directional = new GameObject("Directional Light");
            directional.transform.SetParent(lighting.transform, false);
            Light light = directional.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.4f;
            directional.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            AddLight2D(lighting.transform, "Global Light 2D", 0.85f, Color.white, Vector3.zero, "Global");
            AddLight2D(lighting.transform, "Warm Accent Light 2D", 0.45f, new Color(1f, 0.72f, 0.45f), new Vector3(0f, 2.5f, 0f), "Point");
        }

        private static void AddLight2D(Transform parent, string name, float intensity, Color color, Vector3 position, string lightType)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = position;
            Light2D light = go.AddComponent<Light2D>();
            light.lightType = lightType == "Point" ? Light2D.LightType.Point : Light2D.LightType.Global;
            light.intensity = intensity;
            light.color = color;

            var serialized = new SerializedObject(light);
            SetSerialized(serialized, "m_PointLightOuterRadius", 6f);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateCamera()
        {
            GameObject cameraGo = new GameObject("Main Camera");
            cameraGo.tag = "MainCamera";
            cameraGo.transform.position = new Vector3(0f, 2.5f, -10f);
            Camera camera = cameraGo.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.04f, 0.05f, 1f);
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = new Vector3(0f, 1f, 0f);

            PixelPerfectCamera pixelPerfect = cameraGo.AddComponent<PixelPerfectCamera>();
            pixelPerfect.assetsPPU = 64;
            pixelPerfect.refResolutionX = 640;
            pixelPerfect.refResolutionY = 360;
            pixelPerfect.gridSnapping = PixelPerfectCamera.GridSnapping.UpscaleRenderTexture;
            pixelPerfect.upscaleRT = true;
            pixelPerfect.pixelSnapping = true;

            var serialized = new SerializedObject(pixelPerfect);
            SetSerialized(serialized, "m_AssetsPPU", 64);
            SetSerialized(serialized, "m_RefResolutionX", 640);
            SetSerialized(serialized, "m_RefResolutionY", 360);
            SetSerialized(serialized, "m_UpscaleRT", true);
            SetSerialized(serialized, "m_PixelSnapping", true);
            SetSerialized(serialized, "m_GridSnapping", 2);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerialized(SerializedObject serialized, string propertyName, int value)
        {
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property != null)
            {
                property.intValue = value;
            }
        }

        private static void SetSerialized(SerializedObject serialized, string propertyName, float value)
        {
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property != null)
            {
                property.floatValue = value;
            }
        }

        private static void SetSerialized(SerializedObject serialized, string propertyName, bool value)
        {
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property != null)
            {
                property.boolValue = value;
            }
        }

        private static void EnsureFolder(string folderPath)
        {
            string[] parts = folderPath.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        private static string ProjectPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath));
        }

        private readonly struct ChunkSpec
        {
            public readonly string prefabName;
            public readonly string chunkId;
            public readonly WallType wallType;
            public readonly List<Vector2Int> footprintCells;
            public readonly Vector2 anchorOffset;
            public readonly bool usesCollider;

            public ChunkSpec(
                string prefabName,
                string chunkId,
                WallType wallType,
                List<Vector2Int> footprintCells,
                Vector2 anchorOffset,
                bool usesCollider)
            {
                this.prefabName = prefabName;
                this.chunkId = chunkId;
                this.wallType = wallType;
                this.footprintCells = footprintCells;
                this.anchorOffset = anchorOffset;
                this.usesCollider = usesCollider;
            }
        }
    }
}
