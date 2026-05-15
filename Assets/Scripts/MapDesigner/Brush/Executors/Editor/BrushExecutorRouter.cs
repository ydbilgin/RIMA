#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public class BrushExecutorRouter
    {
        private readonly Dictionary<PaintMode, IBrushExecutor> registry = new Dictionary<PaintMode, IBrushExecutor>();

        public BrushExecutorRouter()
        {
            Register(new GridTileExecutor(PaintMode.GridTile));
            Register(new GridTileExecutor(PaintMode.GridTileRandom));
            Register(new WallStampExecutor());
        }

        public void Register(IBrushExecutor exec)
        {
            if (exec == null)
            {
                return;
            }

            registry[exec.SupportedMode] = exec;
        }

        public BrushExecutorResult Dispatch(BrushStroke stroke, BrushLayerOperation op, MapDesignerBrushPresetSO preset)
        {
            if (preset == null)
            {
                return Error("Preset is null");
            }

            if (op == null)
            {
                return Error("BrushLayerOperation is null");
            }

            if (op.assetPool == null)
            {
                return Error("AssetPool is null");
            }

            if (op.respectsWalkableMask && !IsCellWalkable(stroke.currentCell, stroke.room))
            {
                return new BrushExecutorResult { success = true, spawnedCount = 0 };
            }

            IBrushExecutor exec;
            if (!registry.TryGetValue(preset.paintMode, out exec))
            {
                return Error("No executor for " + preset.paintMode);
            }

            return exec.Apply(stroke, op);
        }

        private static bool IsCellWalkable(UnityEngine.Vector2Int cell, RoomData room)
        {
            if (room.walkable == null || cell.x < 0 || cell.y < 0 || cell.x >= room.size.x || cell.y >= room.size.y)
            {
                return false;
            }

            return room.walkable[cell.x, cell.y];
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
