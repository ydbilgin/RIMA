using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Automation.Editor;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Brush
{
    public sealed class BrushExecutorTests
    {
        private readonly List<UnityEngine.Object> cleanup = new List<UnityEngine.Object>();

        [TearDown]
        public void TearDown()
        {
            WallOverlayPainter[] painters = UnityEngine.Object.FindObjectsByType<WallOverlayPainter>(FindObjectsSortMode.None);
            for (int i = 0; i < painters.Length; i++)
            {
                if (painters[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(painters[i].gameObject);
                }
            }

            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(cleanup[i]);
                }
            }

            cleanup.Clear();
        }

        [Test]
        public void RouterDispatchesToCorrectExecutor()
        {
            var router = new BrushExecutorRouter();
            var fake = new FakeExecutor(PaintMode.GridTile);
            router.Register(fake);

            BrushExecutorResult result = router.Dispatch(
                CreateStroke(new Vector2Int(1, 1), CreateRoomWithWalkableCell(3, 3, new Vector2Int(1, 1))),
                CreateOperation(TargetLayer.L1, false),
                CreatePreset(BrushCategory.Floor, PaintMode.GridTile));

            Assert.IsTrue(result.success);
            Assert.IsTrue(fake.WasCalled);
        }

        [Test]
        public void RouterReturnsErrorForCompositeWithNoOperations()
        {
            var router = new BrushExecutorRouter();
            MapDesignerBrushPresetSO preset = CreatePreset(BrushCategory.Composite, PaintMode.CompositeStroke);
            preset.operations.Clear();

            BrushExecutorResult result = router.Dispatch(
                CreateStroke(new Vector2Int(1, 1), CreateRoomWithWalkableCell(3, 3, new Vector2Int(1, 1))),
                CreateOperation(TargetLayer.L1, false),
                preset);

            Assert.IsFalse(result.success);
            StringAssert.Contains("No operations", result.errorMessage);
        }

        [Test]
        public void RouterSkipsCellWhenWalkableMaskFails()
        {
            var router = new BrushExecutorRouter();
            RoomData room = CreateRoomWithWalkableCell(3, 3, new Vector2Int(0, 0));

            BrushExecutorResult result = router.Dispatch(
                CreateStroke(new Vector2Int(1, 1), room),
                CreateOperation(TargetLayer.L1, true),
                CreatePreset(BrushCategory.Floor, PaintMode.GridTile));

            Assert.IsTrue(result.success);
            Assert.AreEqual(0, result.spawnedCount);
        }

        [Test]
        public void GridTileExecutor_SetsTileOnTilemap()
        {
            Tilemap tilemap = CreateTilemap("L1_GridTileExecutor_Test");
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            cleanup.Add(tile);

            BrushLayerOperation operation = CreateOperation(TargetLayer.L1, false);
            operation.assetPool.tiles.Add(tile);

            var preset = CreatePreset(BrushCategory.Floor, PaintMode.GridTile);
            BrushExecutorResult result = new BrushExecutorRouter().Dispatch(
                CreateStroke(new Vector2Int(2, 1), CreateRoomWithWalkableCell(4, 4, new Vector2Int(2, 1))),
                operation,
                preset);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreSame(tile, tilemap.GetTile(new Vector3Int(2, 1, 0)));
        }

        [Test]
        public void WallStampExecutor_DelegatesToWallOverlayPainter()
        {
            GameObject host = CreateWallHost();
            Sprite sprite = CreateSprite();
            BrushLayerOperation operation = CreateWallOperation(sprite);

            BrushExecutorResult result = new BrushExecutorRouter().Dispatch(
                CreateStroke(new Vector2Int(0, 0), CreateRoomWithWallSegments(false)),
                operation,
                CreatePreset(BrushCategory.Wall, PaintMode.Stamp));

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(1, result.spawnedCount);
            Assert.AreEqual(1, host.GetComponentsInChildren<SpriteRenderer>().Length);
        }

        [Test]
        public void BrushAlongEdges_WalksAllSegments()
        {
            GameObject host = CreateWallHost();
            MapDesignerBrushPresetSO brush = CreateWallBrush(CreateSprite());

            BrushExecutorResult result = BrushAlongEdgesAutomation.Run(brush, CreateRoomWithWallSegments(true), null, 42);

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(7, result.spawnedCount);
            Assert.AreEqual(7, host.GetComponentsInChildren<SpriteRenderer>().Length);
        }

        [Test]
        public void BrushAlongEdges_UndoCollapsesAsOneGroup()
        {
            GameObject host = CreateWallHost();
            MapDesignerBrushPresetSO brush = CreateWallBrush(CreateSprite());

            BrushExecutorResult result = BrushAlongEdgesAutomation.Run(brush, CreateRoomWithWallSegments(true), null, 91);
            Assert.AreEqual(7, result.spawnedCount);
            Assert.AreEqual(7, host.GetComponentsInChildren<SpriteRenderer>().Length);

            Undo.PerformUndo();

            Assert.AreEqual(0, host.GetComponentsInChildren<SpriteRenderer>().Length);
        }

        private GameObject CreateWallHost()
        {
            GameObject host = new GameObject("WallOverlayPainter_ExecutorTest");
            cleanup.Add(host);
            host.AddComponent<WallOverlayPainter>();
            return host;
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

        private static BrushStroke CreateStroke(Vector2Int cell, RoomData room)
        {
            return new BrushStroke
            {
                startCell = cell,
                currentCell = cell,
                room = room,
                seed = 123
            };
        }

        private RoomData CreateRoomWithWalkableCell(int width, int height, Vector2Int walkableCell)
        {
            bool[,] walkable = new bool[width, height];
            if (walkableCell.x >= 0 && walkableCell.y >= 0 && walkableCell.x < width && walkableCell.y < height)
            {
                walkable[walkableCell.x, walkableCell.y] = true;
            }

            return new RoomData
            {
                size = new Vector2Int(width, height),
                walkable = walkable,
                wallEdges = new List<WallSegment>()
            };
        }

        private RoomData CreateRoomWithWallSegments(bool includeDoorway)
        {
            var segments = new List<WallSegment>();
            for (int i = 0; i < 8; i++)
            {
                segments.Add(new WallSegment
                {
                    start = new Vector2Int(i, 0),
                    end = new Vector2Int(i + 1, 0),
                    direction = i % 2 == 0 ? WallDirection.North : WallDirection.East,
                    isCorner = i % 3 == 0,
                    isDoorway = includeDoorway && i == 3
                });
            }

            return new RoomData
            {
                size = new Vector2Int(10, 10),
                walkable = new bool[10, 10],
                wallEdges = segments
            };
        }

        private BrushLayerOperation CreateOperation(TargetLayer layer, bool respectsWalkableMask)
        {
            AssetPoolSO pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            cleanup.Add(pool);
            return new BrushLayerOperation
            {
                targetLayer = layer,
                assetPool = pool,
                respectsWalkableMask = respectsWalkableMask
            };
        }

        private BrushLayerOperation CreateWallOperation(Sprite sprite)
        {
            BrushLayerOperation operation = CreateOperation(TargetLayer.L3, false);
            operation.assetPool.category = AssetCategory.Wall;
            operation.assetPool.sprites.Add(sprite);
            return operation;
        }

        private MapDesignerBrushPresetSO CreatePreset(BrushCategory category, PaintMode mode)
        {
            MapDesignerBrushPresetSO preset = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            cleanup.Add(preset);
            preset.category = category;
            preset.paintMode = mode;
            return preset;
        }

        private MapDesignerBrushPresetSO CreateWallBrush(Sprite sprite)
        {
            MapDesignerBrushPresetSO brush = CreatePreset(BrushCategory.Wall, PaintMode.Stamp);
            brush.operations = new List<BrushLayerOperation> { CreateWallOperation(sprite) };
            return brush;
        }

        private Sprite CreateSprite()
        {
            Texture2D texture = new Texture2D(2, 2);
            cleanup.Add(texture);
            texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white });
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 32f);
            cleanup.Add(sprite);
            return sprite;
        }

        private sealed class FakeExecutor : IBrushExecutor
        {
            public FakeExecutor(PaintMode supportedMode)
            {
                SupportedMode = supportedMode;
            }

            public PaintMode SupportedMode { get; private set; }
            public bool WasCalled { get; private set; }

            public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
            {
                WasCalled = true;
                return new BrushExecutorResult { success = true };
            }
        }
    }
}
