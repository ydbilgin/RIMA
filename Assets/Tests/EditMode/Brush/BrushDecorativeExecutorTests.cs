using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Brush
{
    public sealed class BrushDecorativeExecutorTests
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
            DestroyNamedRoot("WallOverlay");

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
        public void FreeformDecal_PlacesOneSprite()
        {
            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L4, CreateSprite(), 1f);
            BrushExecutorResult result = new FreeformDecalExecutor().Apply(
                CreateStroke(new Vector2(1.5f, 1.5f), CreateRoom(4, 4, true)),
                op);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(1, result.spawnedCount);
            Assert.AreEqual(1, CountSpritesUnder("TransitionBrushLayer"));
        }

        [Test]
        public void FreeformDecal_SkipsNonWalkableCell()
        {
            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L4, CreateSprite(), 1f);
            BrushExecutorResult result = new FreeformDecalExecutor().Apply(
                CreateStroke(new Vector2(1.5f, 1.5f), CreateRoom(4, 4, false)),
                op);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(0, result.spawnedCount);
            Assert.AreEqual(0, CountSpritesUnder("TransitionBrushLayer"));
        }

        [Test]
        public void ScatterAlongStroke_RespectsMinDistance()
        {
            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L5, CreateSprite(), 1f);
            op.minDistance = 32f;
            BrushStroke stroke = CreateStroke(new Vector2(0f, 1f), CreateRoom(160, 4, true));
            stroke.strokePath = new List<Vector2> { new Vector2(0f, 1f), new Vector2(128f, 1f) };
            stroke.currentPositionWorld = new Vector2(128f, 1f);

            BrushExecutorResult result = new ScatterAlongStrokeExecutor().Apply(stroke, op);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.GreaterOrEqual(result.spawnedCount, 2);
            for (int a = 0; a < result.spawnedObjects.Count; a++)
            {
                for (int b = a + 1; b < result.spawnedObjects.Count; b++)
                {
                    float distance = Vector2.Distance(
                        result.spawnedObjects[a].transform.position,
                        result.spawnedObjects[b].transform.position);
                    Assert.GreaterOrEqual(distance + 0.001f, 32f);
                }
            }
        }

        [Test]
        public void Karar143E_EdgeBiasDensityMultiplier()
        {
            RoomData room = CreateRoom(4, 4, true);
            room.walkable[0, 1] = false;
            AnimationCurve curve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(1f, 0.6f),
                new Keyframe(2f, 0.3f),
                new Keyframe(3f, 0.1f));

            float multiplier = Karar143Enforcement.ComputeWallProximityMultiplier(new Vector2Int(1, 1), room, curve);

            Assert.AreEqual(0.6f, multiplier, 0.001f);
        }

        [Test]
        public void Karar143K_FeatureMaskMultiplier()
        {
            FeatureMaskSO mask = ScriptableObject.CreateInstance<FeatureMaskSO>();
            cleanup.Add(mask);
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            cleanup.Add(texture);
            texture.SetPixels(new[]
            {
                new Color(1f, 1f, 1f, 0.5f),
                new Color(1f, 1f, 1f, 0.5f),
                new Color(1f, 1f, 1f, 0.5f),
                new Color(1f, 1f, 1f, 0.5f)
            });
            texture.Apply();
            mask.alphaMask = texture;
            mask.remap = AnimationCurve.Linear(0f, 0f, 1f, 1f);

            BrushLayerOperation op = CreateDecorativeOperation(TargetLayer.L4, CreateSprite(), 1f);
            op.featureMaskMultiplier = mask;
            float density = Karar143Enforcement.EffectiveDensity(
                new Vector2Int(1, 1),
                new Vector2(1f, 1f),
                CreateRoom(2, 2, true),
                op);

            Assert.AreEqual(0.5f, density, 0.01f);
        }

        [Test]
        public void EraseByLayer_RemovesOnlyTargetLayer()
        {
            CreateRootChild("TransitionBrushLayer", Vector3.zero);
            CreateRootChild("DetailDecalLayer", Vector3.zero);
            BrushLayerOperation op = new BrushLayerOperation { targetLayer = TargetLayer.L4, minDistance = 16f };

            BrushExecutorResult result = new EraseByLayerExecutor().Apply(
                CreateStroke(Vector2.zero, CreateRoom(2, 2, true)),
                op);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(1, result.spawnedCount);
            Assert.AreEqual(0, CountChildren("TransitionBrushLayer"));
            Assert.AreEqual(1, CountChildren("DetailDecalLayer"));
        }

        [Test]
        public void EraseAllDecorative_PreservesL1L2()
        {
            Tilemap l1 = CreateTilemap("L1_Tilemap");
            Tilemap l2 = CreateTilemap("L2_Tilemap");
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            cleanup.Add(tile);
            l1.SetTile(Vector3Int.zero, tile);
            l2.SetTile(Vector3Int.zero, tile);
            CreateRootChild("WallOverlay", Vector3.zero);
            CreateRootChild("TransitionBrushLayer", Vector3.zero);
            CreateRootChild("DetailDecalLayer", Vector3.zero);
            CreateRootChild("AccentLayer", Vector3.zero);

            BrushExecutorResult result = new EraseAllDecorativeExecutor().Apply(
                CreateStroke(Vector2.zero, CreateRoom(2, 2, true)),
                new BrushLayerOperation());

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(4, result.spawnedCount);
            Assert.AreSame(tile, l1.GetTile(Vector3Int.zero));
            Assert.AreSame(tile, l2.GetTile(Vector3Int.zero));
        }

        [Test]
        public void WeightedAssetPick_RespectsWeights()
        {
            AssetPoolSO pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            cleanup.Add(pool);
            Sprite first = CreateSprite();
            Sprite second = CreateSprite();
            Sprite third = CreateSprite();
            pool.sprites.Add(first);
            pool.sprites.Add(second);
            pool.sprites.Add(third);
            pool.spriteWeights = new List<float> { 0f, 1f, 0f };

            for (int i = 0; i < 100; i++)
            {
                Assert.AreSame(second, DecorativeExecutorUtility.PickSprite(pool, 1234, i));
            }
        }

        private BrushLayerOperation CreateDecorativeOperation(TargetLayer targetLayer, Sprite sprite, float density)
        {
            AssetPoolSO pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            cleanup.Add(pool);
            pool.sprites.Add(sprite);
            return new BrushLayerOperation
            {
                targetLayer = targetLayer,
                assetPool = pool,
                density = density,
                respectsWalkableMask = true,
                wallProximityCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f)
            };
        }

        private BrushStroke CreateStroke(Vector2 worldPos, RoomData room)
        {
            Vector2Int cell = DecorativeExecutorUtility.WorldToCell(worldPos);
            return new BrushStroke
            {
                startPositionWorld = worldPos,
                currentPositionWorld = worldPos,
                startCell = cell,
                currentCell = cell,
                room = room,
                seed = 777
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

        private Tilemap CreateTilemap(string name)
        {
            GameObject gridObject = new GameObject(name + "_Grid");
            cleanup.Add(gridObject);
            gridObject.AddComponent<Grid>();

            GameObject tilemapObject = new GameObject(name);
            cleanup.Add(tilemapObject);
            tilemapObject.transform.SetParent(gridObject.transform, false);
            Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
            tilemapObject.AddComponent<TilemapRenderer>();
            return tilemap;
        }

        private void CreateRootChild(string rootName, Vector3 position)
        {
            GameObject root = GameObject.Find(rootName);
            if (root == null)
            {
                root = new GameObject(rootName);
                cleanup.Add(root);
            }

            GameObject child = new GameObject(rootName + "_Child");
            cleanup.Add(child);
            child.transform.SetParent(root.transform, false);
            child.transform.position = position;
        }

        private static int CountSpritesUnder(string rootName)
        {
            GameObject root = GameObject.Find(rootName);
            return root != null ? root.GetComponentsInChildren<SpriteRenderer>().Length : 0;
        }

        private static int CountChildren(string rootName)
        {
            GameObject root = GameObject.Find(rootName);
            return root != null ? root.transform.childCount : 0;
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
