#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RIMA.MapDesigner;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using RIMA.MapDesigner.Brush.Automation.Editor;

namespace RIMA.Tests.Brush
{
    public sealed class BrushAutomationTests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null) Object.DestroyImmediate(cleanup[i]);
            }
            cleanup.Clear();
        }

        [Test]
        public void AutoDress_NullPack_ReturnsFailure()
        {
            var result = AutoDressRoom.Run(null, MakeRoom(3, 3), null, 42);
            Assert.IsFalse(result.success);
        }

        [Test]
        public void AutoDress_EmptyPack_ReturnsFailure()
        {
            var pack = ScriptableObject.CreateInstance<BrushPackSO>();
            cleanup.Add(pack);
            var result = AutoDressRoom.Run(pack, MakeRoom(3, 3), null, 42);
            Assert.IsFalse(result.success);
        }

        [Test]
        public void RegenerateDecorative_NullPack_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
            {
                RegenerateDecorativeLayers.Run(null, MakeRoom(3, 3), null, 99);
            });
        }

        [Test]
        public void RegenerateDecorative_ClearLayerContainers_NoExceptionWhenAbsent()
        {
            Assert.DoesNotThrow(() => RegenerateDecorativeLayers.ClearLayerContainers());
        }

        [Test]
        public void SmartFill_NullBrush_ReturnsFailure()
        {
            var result = SmartFillSelection.Run(new RectInt(0, 0, 3, 3), null, MakeRoom(3, 3), null, 42);
            Assert.IsFalse(result.success);
        }

        [Test]
        public void SmartFill_BrushWithoutOperations_ReturnsFailure()
        {
            var brush = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            cleanup.Add(brush);
            brush.operations = new List<BrushLayerOperation>();
            var result = SmartFillSelection.Run(new RectInt(0, 0, 3, 3), brush, MakeRoom(3, 3), null, 42);
            Assert.IsFalse(result.success);
        }

        [Test]
        public void SmartFill_ClampsSelectionToRoomBounds()
        {
            // Selection larger than room should still succeed with no errors
            var brush = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            cleanup.Add(brush);
            var pool = ScriptableObject.CreateInstance<AssetPoolSO>();
            cleanup.Add(pool);
            brush.brushName = "Test";
            brush.paintMode = PaintMode.FreeformDecal;
            brush.operations = new List<BrushLayerOperation>
            {
                new BrushLayerOperation
                {
                    targetLayer = TargetLayer.L4,
                    assetPool = pool,
                    density = 0f,
                    respectsWalkableMask = false
                }
            };

            var result = SmartFillSelection.Run(
                new RectInt(-100, -100, 1000, 1000),
                brush,
                MakeRoom(3, 3),
                null,
                42);

            Assert.IsTrue(result.success);
            Assert.AreEqual(0, result.spawnedCount);  // density=0 → no spawns
        }

        private RoomData MakeRoom(int w, int h)
        {
            bool[,] walkable = new bool[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    walkable[x, y] = true;
            return new RoomData
            {
                size = new Vector2Int(w, h),
                walkable = walkable,
                wallEdges = new List<WallSegment>()
            };
        }
    }
}
#endif
