using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Runtime;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEngine;
using BrushPatchAtlasSO = RIMA.MapDesigner.SO.PatchAtlasSO;

namespace RIMA.Tests.Brush
{
    public sealed class BrushDataFirstExecutorTests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            DestroyAll<TransitionBrushPainter>();
            DestroyAll<DetailDecalPainter>();
            DestroyAll<AccentPainter>();
            DestroyNamedRoot("TransitionBrushLayer");
            DestroyNamedRoot("DetailDecalLayer");
            DestroyNamedRoot("AccentLayer");

            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null)
                {
                    Object.DestroyImmediate(cleanup[i]);
                }
            }

            cleanup.Clear();
        }

        [Test]
        public void FreeformDecalData_Appends_Placement_NotGameObject()
        {
            Sprite sprite = CreateSprite();
            RoomDecalDataSO data = CreateData();
            BrushPipelineConfigSO config = CreateConfig(true, false);
            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L4, sprite, data, config, 1f);
            MapDesignerBrushPresetSO preset = CreatePreset(PaintMode.FreeformDecal, config);
            int spriteRenderersBefore = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Length;

            BrushExecutorResult result = new BrushExecutorRouter().Dispatch(
                CreateStroke(new Vector2(5f, 3f), CreateRoom(8, 8, true), 777),
                op,
                preset);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(1, result.spawnedCount);
            Assert.AreEqual(1, data.placements.Count);
            Assert.IsNull(result.spawnedObjects);
            Assert.AreEqual(spriteRenderersBefore, Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Length);
        }

        [Test]
        public void ScatterAlongStrokeData_Samples_Along_Path()
        {
            Sprite sprite = CreateSprite();
            RoomDecalDataSO data = CreateData();
            BrushPipelineConfigSO config = CreateConfig(false, true);
            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L5, sprite, data, config, 1f);
            op.minDistance = 2f;
            BrushStroke stroke = CreateStroke(new Vector2(10f, 0f), CreateRoom(12, 2, true), 991);
            stroke.startPositionWorld = Vector2.zero;
            stroke.startCell = Vector2Int.zero;
            stroke.strokePath = new List<Vector2> { Vector2.zero, new Vector2(10f, 0f) };
            MapDesignerBrushPresetSO preset = CreatePreset(PaintMode.ScatterAlongStroke, config);

            BrushExecutorResult result = new BrushExecutorRouter().Dispatch(stroke, op, preset);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.That(data.placements.Count, Is.InRange(3, 5));
            Assert.IsNull(result.spawnedObjects);
            Assert.AreEqual(0, Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Length);
        }

        [Test]
        public void Seed_Determinism_Same_Seed_Same_Placements()
        {
            Sprite sprite = CreateSprite();
            BrushPipelineConfigSO config = CreateConfig(true, false);
            RoomDecalDataSO first = CreateData();
            RoomDecalDataSO second = CreateData();
            BrushStroke stroke = CreateStroke(new Vector2(4f, 4f), CreateRoom(8, 8, true), 12345);

            BrushLayerOperation firstOp = CreateDecorativeOperation(TargetLayer.L6, sprite, first, config, 1f);
            BrushLayerOperation secondOp = CreateDecorativeOperation(TargetLayer.L6, sprite, second, config, 1f);
            MapDesignerBrushPresetSO preset = CreatePreset(PaintMode.FreeformDecal, config);

            new BrushExecutorRouter().Dispatch(stroke, firstOp, preset);
            new BrushExecutorRouter().Dispatch(stroke, secondOp, preset);

            Assert.AreEqual(1, first.placements.Count);
            Assert.AreEqual(1, second.placements.Count);
            Assert.AreEqual(first.placements[0].worldPos, second.placements[0].worldPos);
            Assert.AreEqual(first.placements[0].spriteId, second.placements[0].spriteId);
            Assert.AreEqual(first.placements[0].rotationStep, second.placements[0].rotationStep);
            Assert.AreEqual(first.placements[0].flags, second.placements[0].flags);
        }

        [Test]
        public void Flag_Off_Uses_Legacy_GameObject_Path()
        {
            Sprite sprite = CreateSprite();
            RoomDecalDataSO data = CreateData();
            BrushPipelineConfigSO config = CreateConfig(false, false);
            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L4, sprite, data, config, 1f);
            MapDesignerBrushPresetSO preset = CreatePreset(PaintMode.FreeformDecal, config);

            BrushExecutorResult result = new BrushExecutorRouter().Dispatch(
                CreateStroke(new Vector2(2f, 2f), CreateRoom(4, 4, true), 777),
                op,
                preset);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(1, result.spawnedCount);
            Assert.AreEqual(0, data.placements.Count);
            Assert.AreEqual(1, CountSpritesUnder("TransitionBrushLayer"));
        }

        [Test]
        public void Chunk_Renderer_Builds_Mesh_From_SO()
        {
            Sprite sprite = CreateSprite();
            RoomDecalDataSO data = CreateData();
            BrushPatchAtlasSO atlas = CreatePatchAtlas(sprite);
            for (int i = 0; i < 10; i++)
            {
                data.placements.Add(new DecalPlacement
                {
                    worldPos = new Vector2(i, i % 3),
                    spriteId = 0,
                    layer = (byte)(4 + i % 3),
                    rotationStep = (byte)(i % 4)
                });
            }

            GameObject host = new GameObject("RoomDecalChunkRenderer_Test");
            cleanup.Add(host);
            RoomDecalChunkRenderer renderer = host.AddComponent<RoomDecalChunkRenderer>();
            renderer.DecalData = data;
            renderer.PatchAtlas = atlas;
            renderer.Build();

            MeshRenderer[] meshRenderers = host.GetComponentsInChildren<MeshRenderer>(true);
            MeshFilter[] meshFilters = host.GetComponentsInChildren<MeshFilter>(true);

            int vertexCount = 0;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                vertexCount += meshFilters[i].sharedMesh != null ? meshFilters[i].sharedMesh.vertexCount : 0;
            }

            Assert.AreEqual(3, meshRenderers.Length);
            Assert.AreEqual(40, vertexCount);
        }

        private BrushLayerOperation CreateDecorativeOperation(
            TargetLayer targetLayer,
            Sprite sprite,
            RoomDecalDataSO data,
            BrushPipelineConfigSO config,
            float density)
        {
            AssetPoolSO pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            cleanup.Add(pool);
            pool.sprites.Add(sprite);
            return new BrushLayerOperation
            {
                targetLayer = targetLayer,
                assetPool = pool,
                density = density,
                minDistance = 1f,
                respectsWalkableMask = true,
                wallProximityCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f),
                useNativeBucketVariantPath = false,
                roomDecalData = data,
                patchAtlas = CreatePatchAtlas(sprite),
                pipelineConfig = config,
                allowRotation = true,
                allowFlipX = true,
                allowFlipY = true
            };
        }

        private RoomDecalDataSO CreateData()
        {
            RoomDecalDataSO data = ScriptableObject.CreateInstance<RoomDecalDataSO>();
            cleanup.Add(data);
            data.roomId = "test_room";
            return data;
        }

        private BrushPipelineConfigSO CreateConfig(bool useDataFirstDecals, bool useDataFirstScatter)
        {
            BrushPipelineConfigSO config = ScriptableObject.CreateInstance<BrushPipelineConfigSO>();
            cleanup.Add(config);
            config.useDataFirstDecals = useDataFirstDecals;
            config.useDataFirstScatter = useDataFirstScatter;
            return config;
        }

        private MapDesignerBrushPresetSO CreatePreset(PaintMode mode, BrushPipelineConfigSO config)
        {
            MapDesignerBrushPresetSO preset = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            cleanup.Add(preset);
            preset.paintMode = mode;
            preset.pipelineConfig = config;
            return preset;
        }

        private BrushPatchAtlasSO CreatePatchAtlas(Sprite sprite)
        {
            BrushPatchAtlasSO atlas = ScriptableObject.CreateInstance<BrushPatchAtlasSO>();
            cleanup.Add(atlas);
            atlas.atlasId = "test_patch_atlas";
            atlas.variants = new[] { sprite };
            atlas.allowedTransforms.flipX = true;
            atlas.allowedTransforms.flipY = true;
            atlas.allowedTransforms.rotate90 = true;
            atlas.allowedTransforms.rotate180 = true;
            atlas.allowedTransforms.rotate270 = true;
            atlas.allowedTransforms.scaleRange = Vector2.one;
            return atlas;
        }

        private BrushStroke CreateStroke(Vector2 worldPos, RoomData room, int seed)
        {
            Vector2Int cell = DecorativeExecutorUtility.WorldToCell(worldPos);
            return new BrushStroke
            {
                startPositionWorld = worldPos,
                currentPositionWorld = worldPos,
                startCell = cell,
                currentCell = cell,
                room = room,
                seed = seed
            };
        }

        private static RoomData CreateRoom(int width, int height, bool walkableValue)
        {
            bool[,] walkable = new bool[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    walkable[x, y] = walkableValue;
                }
            }

            return new RoomData
            {
                size = new Vector2Int(width, height),
                walkable = walkable,
                wallEdges = new List<WallSegment>()
            };
        }

        private Sprite CreateSprite()
        {
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            cleanup.Add(texture);
            texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white });
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 32f);
            cleanup.Add(sprite);
            return sprite;
        }

        private static int CountSpritesUnder(string rootName)
        {
            GameObject root = GameObject.Find(rootName);
            return root != null ? root.GetComponentsInChildren<SpriteRenderer>().Length : 0;
        }

        private static void DestroyAll<T>() where T : Component
        {
            T[] instances = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
            for (int i = 0; i < instances.Length; i++)
            {
                if (instances[i] != null)
                {
                    Object.DestroyImmediate(instances[i].gameObject);
                }
            }
        }

        private static void DestroyNamedRoot(string rootName)
        {
            GameObject root = GameObject.Find(rootName);
            if (root != null)
            {
                Object.DestroyImmediate(root);
            }
        }
    }
}
