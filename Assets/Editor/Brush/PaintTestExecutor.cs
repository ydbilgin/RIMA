#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Runtime;
using RIMA.MapDesigner.Brush.Stroke;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using BrushPatchAtlasSO = RIMA.MapDesigner.SO.PatchAtlasSO;
using Object = UnityEngine.Object;

namespace RIMA.Editor.Brush
{
    public static class PaintTestExecutor
    {
        private const int Seed = 42;
        private const string ScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";
        private const string RoomPath = "Assets/Data/Rooms/Library/Combat_Small_01.asset";
        private const string ConfigPath = "Assets/Data/Brush/AssetParts_v2/PaintTest_PipelineConfig.asset";
        private const string DataFolder = "Assets/Data/Brush/AssetParts_v2";
        private const string ScreenshotRelativePath = "STAGING/Brush_V1_paint_test_screenshot_01.png";
        private const string RigName = "BrushV1PaintTestRig";

        private static readonly Dictionary<string, int> Counts = new Dictionary<string, int>();

        [MenuItem("Tools/Brush V1/Run Paint Test")]
        public static void RunPaintTest()
        {
            Counts.Clear();

            EnsureScene();
            RoomTemplateSO roomTemplate = LoadRequired<RoomTemplateSO>(RoomPath);
            BrushPipelineConfigSO config = EnsureConfig();
            BrushPatchAtlasSO baseFloor = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/BaseFloor.asset");
            BrushPatchAtlasSO moss = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/OrganicDecal_Moss.asset");
            BrushPatchAtlasSO dirt = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/OrganicDecal_Dirt.asset");
            BrushPatchAtlasSO pebbles = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/DetailScatter_Pebbles.asset");
            BrushPatchAtlasSO cracksBones = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/DetailScatter_CracksBones.asset");
            BrushPatchAtlasSO rift = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/Accent_Rift.asset");
            BrushPatchAtlasSO ritual = LoadRequired<BrushPatchAtlasSO>("Assets/Data/Brush/AssetParts_v2/Accent_Ritual.asset");

            Transform rig = EnsureRig();
            RectInt bounds = roomTemplate.bounds;
            RoomData room = BuildRoomData(bounds);

            ConfigureCamera(bounds);
            ConfigureMainLight();

            RoomDecalDataSO baseData = EnsureData("PaintTest_BaseFloor_Data.asset", roomTemplate.roomId);
            RoomDecalDataSO mossData = EnsureData("PaintTest_Moss_Data.asset", roomTemplate.roomId);
            RoomDecalDataSO dirtData = EnsureData("PaintTest_Dirt_Data.asset", roomTemplate.roomId);
            RoomDecalDataSO pebblesData = EnsureData("PaintTest_Pebbles_Data.asset", roomTemplate.roomId);
            RoomDecalDataSO cracksData = EnsureData("PaintTest_CracksBones_Data.asset", roomTemplate.roomId);
            RoomDecalDataSO riftData = EnsureData("PaintTest_Rift_Data.asset", roomTemplate.roomId);
            RoomDecalDataSO ritualData = EnsureData("PaintTest_Ritual_Data.asset", roomTemplate.roomId);

            PlaceBaseFloorAndMacros(baseData, baseFloor, bounds);
            RunFreeform(mossData, moss, config, room, bounds, TargetLayer.L4, "OrganicDecal_Moss", 10, Seed + 100);
            RunFreeform(dirtData, dirt, config, room, bounds, TargetLayer.L4, "OrganicDecal_Dirt", 7, Seed + 200);
            RunScatter(pebblesData, pebbles, config, room, bounds, TargetLayer.L5, "DetailScatter_Pebbles", Seed + 300, 1f, 0.72f, new Vector2(0.16f, 0.16f));
            RunScatter(cracksData, cracksBones, config, room, bounds, TargetLayer.L5, "DetailScatter_CracksBones", Seed + 400, 1f, 1.38f, new Vector2(0.08f, 0.08f));
            PlaceAccent(riftData, rift, config, room, bounds, TargetLayer.L6, "Accent_Rift", bounds.center, Seed + 500);
            PlaceAccent(ritualData, ritual, config, room, bounds, TargetLayer.L6, "Accent_Ritual", bounds.center + new Vector2(1.4f, -1.1f), Seed + 600);

            CreateRenderer(rig, "BaseFloor_Renderer", baseData, baseFloor, 0);
            CreateRenderer(rig, "Moss_Renderer", mossData, moss, 2);
            CreateRenderer(rig, "Dirt_Renderer", dirtData, dirt, 2);
            CreateRenderer(rig, "Pebbles_Renderer", pebblesData, pebbles, 3);
            CreateRenderer(rig, "CracksBones_Renderer", cracksData, cracksBones, 3);
            CreateRenderer(rig, "Rift_Renderer", riftData, rift, 4);
            CreateRenderer(rig, "Ritual_Renderer", ritualData, ritual, 4);

            EditorUtility.SetDirty(baseData);
            EditorUtility.SetDirty(mossData);
            EditorUtility.SetDirty(dirtData);
            EditorUtility.SetDirty(pebblesData);
            EditorUtility.SetDirty(cracksData);
            EditorUtility.SetDirty(riftData);
            EditorUtility.SetDirty(ritualData);
            AssetDatabase.SaveAssets();

            string screenshotPath = Capture(bounds);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            Debug.Log(FormatSummary(screenshotPath));
        }

        private static void EnsureScene()
        {
            Scene active = SceneManager.GetActiveScene();
            if (active.path != ScenePath)
            {
                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            }
        }

        private static T LoadRequired<T>(string path) where T : Object
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                throw new InvalidOperationException("Missing required asset: " + path);
            }

            return asset;
        }

        private static BrushPipelineConfigSO EnsureConfig()
        {
            BrushPipelineConfigSO config = AssetDatabase.LoadAssetAtPath<BrushPipelineConfigSO>(ConfigPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<BrushPipelineConfigSO>();
                AssetDatabase.CreateAsset(config, ConfigPath);
            }

            config.useDataFirstDecals = true;
            config.useDataFirstScatter = true;
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static Transform EnsureRig()
        {
            GameObject stageRoot = GameObject.Find("StageRoot");
            if (stageRoot == null)
            {
                stageRoot = new GameObject("StageRoot");
            }

            Transform existing = stageRoot.transform.Find(RigName);
            if (existing != null)
            {
                Object.DestroyImmediate(existing.gameObject);
            }

            GameObject rig = new GameObject(RigName);
            rig.transform.SetParent(stageRoot.transform, false);
            rig.transform.localPosition = Vector3.zero;
            return rig.transform;
        }

        private static void ConfigureCamera(RectInt bounds)
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                GameObject cameraGo = GameObject.Find("Main Camera");
                if (cameraGo == null)
                {
                    cameraGo = new GameObject("Main Camera");
                }

                cameraGo.tag = "MainCamera";
                camera = cameraGo.GetComponent<Camera>();
                if (camera == null)
                {
                    camera = cameraGo.AddComponent<Camera>();
                }
            }

            Vector2 center = bounds.center;
            camera.orthographic = true;
            camera.nearClipPlane = -100f;
            camera.farClipPlane = 100f;
            camera.orthographicSize = bounds.height * 0.5f + 2f;
            camera.transform.position = new Vector3(center.x, center.y, -10f);
            camera.transform.rotation = Quaternion.identity;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.065f, 0.06f, 0.052f, 1f);
        }

        private static void ConfigureMainLight()
        {
            GameObject lightGo = GameObject.Find("Main Light");
            if (lightGo == null)
            {
                lightGo = new GameObject("Main Light");
            }

            Light light = lightGo.GetComponent<Light>();
            if (light == null)
            {
                light = lightGo.AddComponent<Light>();
            }

            light.type = LightType.Directional;
            light.color = new Color(1f, 0.92f, 0.82f, 1f);
            light.intensity = 0.6f;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            GameObject global2D = GameObject.Find("Global Light 2D");
            if (global2D == null)
            {
                return;
            }

            Component light2D = global2D.GetComponent("Light2D");
            if (light2D == null)
            {
                return;
            }

            SerializedObject serialized = new SerializedObject(light2D);
            SerializedProperty color = serialized.FindProperty("m_Color");
            if (color != null)
            {
                color.colorValue = new Color(1f, 0.92f, 0.82f, 1f);
            }

            SerializedProperty intensity = serialized.FindProperty("m_Intensity");
            if (intensity != null)
            {
                intensity.floatValue = 0.6f;
            }

            serialized.ApplyModifiedProperties();
        }

        private static RoomData BuildRoomData(RectInt bounds)
        {
            bool[,] walkable = new bool[bounds.width, bounds.height];
            for (int y = 0; y < bounds.height; y++)
            {
                for (int x = 0; x < bounds.width; x++)
                {
                    walkable[x, y] = true;
                }
            }

            return new RoomData
            {
                size = new Vector2Int(bounds.width, bounds.height),
                seed = Seed,
                walkable = walkable,
                wallEdges = new List<WallSegment>()
            };
        }

        private static RoomDecalDataSO EnsureData(string fileName, string roomId)
        {
            string path = DataFolder + "/" + fileName;
            RoomDecalDataSO data = AssetDatabase.LoadAssetAtPath<RoomDecalDataSO>(path);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<RoomDecalDataSO>();
                AssetDatabase.CreateAsset(data, path);
            }

            data.roomId = roomId;
            data.placements.Clear();
            EditorUtility.SetDirty(data);
            return data;
        }

        private static void PlaceBaseFloorAndMacros(RoomDecalDataSO data, BrushPatchAtlasSO atlas, RectInt bounds)
        {
            System.Random random = new System.Random(Seed);
            int floorVariantCount = Mathf.Min(16, atlas.variants != null ? atlas.variants.Length : 0);
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    data.placements.Add(new DecalPlacement
                    {
                        worldPos = new Vector2(x + 0.5f, y + 0.5f),
                        spriteId = random.Next(0, floorVariantCount),
                        layer = 4
                    });
                }
            }

            Counts["BaseFloor"] = bounds.width * bounds.height;

            int macroStart = Mathf.Min(16, atlas.variants != null ? atlas.variants.Length : 0);
            int macroCount = Mathf.Max(0, (atlas.variants != null ? atlas.variants.Length : 0) - macroStart);
            Vector2[] positions =
            {
                bounds.center + new Vector2(-2.0f, -1.2f),
                bounds.center + new Vector2(1.6f, 0.8f),
                bounds.center + new Vector2(-0.8f, 1.3f),
                bounds.center + new Vector2(2.1f, -1.4f)
            };

            int placed = 0;
            for (int i = 0; i < positions.Length && macroCount > 0; i++)
            {
                data.placements.Add(new DecalPlacement
                {
                    worldPos = Clamp(bounds, positions[i]),
                    spriteId = macroStart + i % macroCount,
                    layer = 4,
                    rotationStep = (byte)(i % 4),
                    flags = (byte)(i % 2)
                });
                placed++;
            }

            Counts["MacroPatch"] = placed;
        }

        private static void RunFreeform(
            RoomDecalDataSO data,
            BrushPatchAtlasSO atlas,
            BrushPipelineConfigSO config,
            RoomData room,
            RectInt bounds,
            TargetLayer layer,
            string label,
            int count,
            int seed)
        {
            BrushLayerOperation op = BuildOperation(data, atlas, config, layer);
            op.positionJitter = new Vector2(0.18f, 0.18f);
            op.rotationSnapDegrees = 90f;

            MapDesignerBrushPresetSO preset = BuildPreset(PaintMode.FreeformDecal, config);
            BrushExecutorRouter router = new BrushExecutorRouter();
            System.Random random = new System.Random(seed);
            int before = data.placements.Count;
            for (int i = 0; i < count; i++)
            {
                Vector2 pos = new Vector2(
                    Mathf.Lerp(bounds.xMin + 0.8f, bounds.xMax - 0.8f, (float)random.NextDouble()),
                    Mathf.Lerp(bounds.yMin + 0.8f, bounds.yMax - 0.8f, (float)random.NextDouble()));
                BrushExecutorResult result = router.Dispatch(CreateStroke(pos, room, seed + i), op, preset);
                if (!result.success)
                {
                    throw new InvalidOperationException(label + " failed: " + result.errorMessage);
                }
            }

            Counts[label] = data.placements.Count - before;
            Object.DestroyImmediate(op.assetPool);
            Object.DestroyImmediate(preset);
        }

        private static void RunScatter(
            RoomDecalDataSO data,
            BrushPatchAtlasSO atlas,
            BrushPipelineConfigSO config,
            RoomData room,
            RectInt bounds,
            TargetLayer layer,
            string label,
            int seed,
            float density,
            float minDistance,
            Vector2 jitter)
        {
            BrushLayerOperation op = BuildOperation(data, atlas, config, layer);
            op.density = density;
            op.minDistance = minDistance;
            op.positionJitter = jitter;
            op.rotationSnapDegrees = 90f;

            MapDesignerBrushPresetSO preset = BuildPreset(PaintMode.ScatterAlongStroke, config);
            BrushExecutorRouter router = new BrushExecutorRouter();
            Vector2 start = new Vector2(bounds.xMin + 0.7f, bounds.yMin + 0.6f);
            Vector2 end = new Vector2(bounds.xMax - 0.7f, bounds.yMax - 0.6f);
            BrushStroke stroke = CreateStroke(end, room, seed);
            stroke.startPositionWorld = start;
            stroke.startCell = Vector2Int.zero;
            stroke.strokePath = new List<Vector2> { start, bounds.center + new Vector2(-0.3f, 0.5f), end };

            int before = data.placements.Count;
            BrushExecutorResult result = router.Dispatch(stroke, op, preset);
            if (!result.success)
            {
                throw new InvalidOperationException(label + " failed: " + result.errorMessage);
            }

            Counts[label] = data.placements.Count - before;
            Object.DestroyImmediate(op.assetPool);
            Object.DestroyImmediate(preset);
        }

        private static void PlaceAccent(
            RoomDecalDataSO data,
            BrushPatchAtlasSO atlas,
            BrushPipelineConfigSO config,
            RoomData room,
            RectInt bounds,
            TargetLayer layer,
            string label,
            Vector2 position,
            int seed)
        {
            BrushLayerOperation op = BuildOperation(data, atlas, config, layer);
            op.positionJitter = Vector2.zero;
            op.rotationSnapDegrees = 90f;
            MapDesignerBrushPresetSO preset = BuildPreset(PaintMode.FreeformDecal, config);
            BrushExecutorResult result = new BrushExecutorRouter().Dispatch(CreateStroke(Clamp(bounds, position), room, seed), op, preset);
            if (!result.success)
            {
                throw new InvalidOperationException(label + " failed: " + result.errorMessage);
            }

            Counts[label] = data.placements.Count;
            Object.DestroyImmediate(op.assetPool);
            Object.DestroyImmediate(preset);
        }

        private static BrushLayerOperation BuildOperation(RoomDecalDataSO data, BrushPatchAtlasSO atlas, BrushPipelineConfigSO config, TargetLayer layer)
        {
            AssetPoolSO pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            pool.poolName = atlas != null ? atlas.atlasId : "paint_test_pool";
            pool.supportsFlip = atlas == null || atlas.allowedTransforms.flipX || atlas.allowedTransforms.flipY;
            pool.supportsRotation = atlas == null || atlas.allowedTransforms.rotate90 || atlas.allowedTransforms.rotate180 || atlas.allowedTransforms.rotate270;
            if (atlas != null && atlas.variants != null)
            {
                for (int i = 0; i < atlas.variants.Length; i++)
                {
                    if (atlas.variants[i] != null)
                    {
                        pool.sprites.Add(atlas.variants[i]);
                    }
                }
            }

            return new BrushLayerOperation
            {
                targetLayer = layer,
                assetPool = pool,
                density = 1f,
                probability = 1f,
                minDistance = 0.5f,
                scaleRange = Vector2.one,
                allowRotation = true,
                allowFlipX = true,
                allowFlipY = false,
                tint = Color.white,
                positionJitter = Vector2.zero,
                respectsWalkableMask = false,
                useNativeBucketVariantPath = false,
                pipelineConfig = config,
                roomDecalData = data,
                patchAtlas = atlas
            };
        }

        private static MapDesignerBrushPresetSO BuildPreset(PaintMode mode, BrushPipelineConfigSO config)
        {
            MapDesignerBrushPresetSO preset = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            preset.brushName = "Brush V1 Paint Test " + mode;
            preset.paintMode = mode;
            preset.pipelineConfig = config;
            return preset;
        }

        private static BrushStroke CreateStroke(Vector2 pos, RoomData room, int seed)
        {
            return new BrushStroke
            {
                startPositionWorld = pos,
                currentPositionWorld = pos,
                startCell = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)),
                currentCell = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)),
                isDrag = false,
                seed = seed,
                room = room
            };
        }

        private static Vector2 Clamp(RectInt bounds, Vector2 pos)
        {
            return new Vector2(
                Mathf.Clamp(pos.x, bounds.xMin + 0.5f, bounds.xMax - 0.5f),
                Mathf.Clamp(pos.y, bounds.yMin + 0.5f, bounds.yMax - 0.5f));
        }

        private static void CreateRenderer(Transform rig, string name, RoomDecalDataSO data, BrushPatchAtlasSO atlas, int sortingOrder)
        {
            GameObject host = new GameObject(name);
            host.transform.SetParent(rig, false);
            RoomDecalChunkRenderer renderer = host.AddComponent<RoomDecalChunkRenderer>();
            renderer.DecalData = data;
            renderer.PatchAtlas = atlas;
            renderer.Build();

            MeshRenderer[] meshRenderers = host.GetComponentsInChildren<MeshRenderer>(true);
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sortingOrder = sortingOrder;
            }
        }

        private static string Capture(RectInt bounds)
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Main Camera missing before capture.");
            }

            ConfigureCamera(bounds);
            RenderTexture previous = RenderTexture.active;
            RenderTexture target = new RenderTexture(1280, 720, 24, RenderTextureFormat.ARGB32);
            Texture2D image = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
            try
            {
                camera.targetTexture = target;
                camera.Render();
                RenderTexture.active = target;
                image.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
                image.Apply();

                string projectRoot = Directory.GetParent(Application.dataPath).FullName;
                string fullPath = Path.Combine(projectRoot, ScreenshotRelativePath.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllBytes(fullPath, image.EncodeToPNG());
                return ScreenshotRelativePath;
            }
            finally
            {
                camera.targetTexture = null;
                RenderTexture.active = previous;
                Object.DestroyImmediate(image);
                Object.DestroyImmediate(target);
            }
        }

        private static string FormatSummary(string screenshotPath)
        {
            int rendererCount = Object.FindObjectsByType<RoomDecalChunkRenderer>(FindObjectsSortMode.None).Length;
            int meshRendererCount = Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None).Length;
            return "[BrushV1PaintTest] scene=" + ScenePath +
                " screenshot=" + screenshotPath +
                " dimensions=1280x720 counts=" + string.Join(", ", FormatCounts()) +
                " chunkRenderers=" + rendererCount +
                " meshRenderers=" + meshRendererCount;
        }

        private static string[] FormatCounts()
        {
            List<string> values = new List<string>();
            foreach (KeyValuePair<string, int> pair in Counts)
            {
                values.Add(pair.Key + ":" + pair.Value);
            }

            return values.ToArray();
        }
    }
}
#endif
