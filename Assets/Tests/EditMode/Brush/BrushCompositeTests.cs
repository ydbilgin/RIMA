using System.Collections.Generic;
using NUnit.Framework;
using RIMA.Data;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.Brush
{
    public sealed class BrushCompositeTests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
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
        public void Composite_FiresAllOperations()
        {
            var router = new BrushExecutorRouter();
            var grid = new CountingExecutor(PaintMode.GridTile);
            var stamp = new CountingExecutor(PaintMode.Stamp);
            var decal = new CountingExecutor(PaintMode.FreeformDecal);
            router.Register(grid);
            router.Register(stamp);
            router.Register(decal);

            BrushExecutorResult result = router.Dispatch(
                CreateStroke(),
                CreateOperation(TargetLayer.L1, 1f),
                CreateCompositePreset(
                    CreateOperation(TargetLayer.L1, 1f),
                    CreateOperation(TargetLayer.L3, 1f),
                    CreateOperation(TargetLayer.L4, 1f)));

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(3, grid.CallCount + stamp.CallCount + decal.CallCount);
        }

        [Test]
        public void Composite_RespectsProbabilityGate()
        {
            var router = new BrushExecutorRouter();
            var grid = new CountingExecutor(PaintMode.GridTile);
            var stamp = new CountingExecutor(PaintMode.Stamp);
            router.Register(grid);
            router.Register(stamp);

            BrushExecutorResult result = router.Dispatch(
                CreateStroke(),
                CreateOperation(TargetLayer.L1, 0f),
                CreateCompositePreset(
                    CreateOperation(TargetLayer.L1, 0f),
                    CreateOperation(TargetLayer.L3, 1f)));

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(0, grid.CallCount);
            Assert.AreEqual(1, stamp.CallCount);
        }

        [Test]
        public void Composite_SingleUndoGroup()
        {
            var router = new BrushExecutorRouter();
            var grid = new SpawningExecutor(PaintMode.GridTile, cleanup);
            var stamp = new SpawningExecutor(PaintMode.Stamp, cleanup);
            var decal = new SpawningExecutor(PaintMode.FreeformDecal, cleanup);
            router.Register(grid);
            router.Register(stamp);
            router.Register(decal);

            BrushExecutorResult result = router.Dispatch(
                CreateStroke(),
                CreateOperation(TargetLayer.L1, 1f),
                CreateCompositePreset(
                    CreateOperation(TargetLayer.L1, 1f),
                    CreateOperation(TargetLayer.L3, 1f),
                    CreateOperation(TargetLayer.L4, 1f)));

            Assert.IsTrue(result.success, result.errorMessage);
            Assert.AreEqual(3, result.spawnedCount);
            Assert.AreEqual(3, CountExisting(result.spawnedObjects));

            Undo.PerformUndo();

            Assert.AreEqual(0, CountExisting(result.spawnedObjects));
        }

        [Test]
        public void DefaultPack_AllBrushesValid()
        {
            BrushPackSO pack = AssetDatabase.LoadAssetAtPath<BrushPackSO>(
                "Assets/Data/Brush/Default/BrushPack_ShatteredKeep_Default.asset");

            Assert.IsNotNull(pack);
            Assert.GreaterOrEqual(pack.brushes.Count, 8);
            Assert.LessOrEqual(pack.brushes.Count, 12);

            foreach (MapDesignerBrushPresetSO brush in pack.brushes)
            {
                Assert.IsNotNull(brush);
                Assert.IsNotNull(brush.operations, brush != null ? brush.brushName : "null brush");
                Assert.GreaterOrEqual(brush.operations.Count, 1, brush.brushName);

                foreach (BrushLayerOperation op in brush.operations)
                {
                    Assert.IsNotNull(op.assetPool, brush.brushName + " has null AssetPool");
                }
            }
        }

        [Test]
        public void DefaultPack_HotkeyUniqueness()
        {
            BrushPackSO pack = AssetDatabase.LoadAssetAtPath<BrushPackSO>(
                "Assets/Data/Brush/Default/BrushPack_ShatteredKeep_Default.asset");
            Assert.IsNotNull(pack);

            var seen = new HashSet<int>();
            foreach (MapDesignerBrushPresetSO brush in pack.brushes)
            {
                if (brush.hotkeyIndex == -1)
                {
                    continue;
                }

                Assert.IsTrue(seen.Add(brush.hotkeyIndex), "Duplicate hotkey " + brush.hotkeyIndex);
            }
        }

        private BrushStroke CreateStroke()
        {
            bool[,] walkable = new bool[3, 3];
            walkable[1, 1] = true;
            return new BrushStroke
            {
                currentCell = new Vector2Int(1, 1),
                currentPositionWorld = new Vector2(1f, 1f),
                room = new RoomData
                {
                    size = new Vector2Int(3, 3),
                    walkable = walkable,
                    wallEdges = new List<WallSegment>()
                },
                seed = 123
            };
        }

        private BrushLayerOperation CreateOperation(TargetLayer layer, float probability)
        {
            AssetPoolSO pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            cleanup.Add(pool);
            return new BrushLayerOperation
            {
                targetLayer = layer,
                assetPool = pool,
                probability = probability,
                respectsWalkableMask = false
            };
        }

        private MapDesignerBrushPresetSO CreateCompositePreset(params BrushLayerOperation[] operations)
        {
            MapDesignerBrushPresetSO preset = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            cleanup.Add(preset);
            preset.brushName = "Composite Test";
            preset.category = BrushCategory.Composite;
            preset.paintMode = PaintMode.CompositeStroke;
            preset.operations = new List<BrushLayerOperation>(operations);
            return preset;
        }

        private static int CountExisting(List<GameObject> spawnedObjects)
        {
            int count = 0;
            foreach (GameObject go in spawnedObjects)
            {
                if (go != null)
                {
                    count++;
                }
            }

            return count;
        }

        private sealed class CountingExecutor : IBrushExecutor
        {
            public CountingExecutor(PaintMode supportedMode)
            {
                SupportedMode = supportedMode;
            }

            public PaintMode SupportedMode { get; private set; }
            public int CallCount { get; private set; }

            public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
            {
                CallCount++;
                return new BrushExecutorResult { success = true };
            }
        }

        private sealed class SpawningExecutor : IBrushExecutor
        {
            private readonly List<Object> cleanup;

            public SpawningExecutor(PaintMode supportedMode, List<Object> cleanup)
            {
                SupportedMode = supportedMode;
                this.cleanup = cleanup;
            }

            public PaintMode SupportedMode { get; private set; }

            public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
            {
                GameObject go = new GameObject("CompositeUndoSpawn");
                cleanup.Add(go);
                Undo.RegisterCreatedObjectUndo(go, "Composite Test Spawn");
                return new BrushExecutorResult
                {
                    success = true,
                    spawnedCount = 1,
                    spawnedObjects = new List<GameObject> { go }
                };
            }
        }
    }
}
