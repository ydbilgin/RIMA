#if UNITY_EDITOR
using System;
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
            RegisterIfAvailable("RIMA.MapDesigner.Brush.Executors.Editor.FreeformDecalExecutor");
            RegisterIfAvailable("RIMA.MapDesigner.Brush.Executors.Editor.ScatterAlongStrokeExecutor");
            RegisterIfAvailable("RIMA.MapDesigner.Brush.Executors.Editor.StampExecutor");
            RegisterIfAvailable("RIMA.MapDesigner.Brush.Executors.Editor.EraseByLayerExecutor");
            RegisterIfAvailable("RIMA.MapDesigner.Brush.Executors.Editor.EraseAllDecorativeExecutor");
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

            bool requiresAssetPool = preset.paintMode != PaintMode.EraseByLayer &&
                preset.paintMode != PaintMode.EraseAllDecorative;
            if (requiresAssetPool && op.assetPool == null)
            {
                return Error("AssetPool is null");
            }

            if (op.respectsWalkableMask &&
                preset.paintMode != PaintMode.EraseByLayer &&
                preset.paintMode != PaintMode.EraseAllDecorative &&
                !IsCellWalkable(stroke.currentCell, stroke.room))
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

        private void RegisterIfAvailable(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type == null || !typeof(IBrushExecutor).IsAssignableFrom(type))
            {
                return;
            }

            IBrushExecutor executor = Activator.CreateInstance(type) as IBrushExecutor;
            Register(executor);
        }

        private static bool IsCellWalkable(UnityEngine.Vector2Int cell, RoomData room)
        {
            if (room.walkable == null)
            {
                return false;
            }

            int width = room.walkable.GetLength(0);
            int height = room.walkable.GetLength(1);
            return cell.x >= 0 &&
                cell.y >= 0 &&
                cell.x < width &&
                cell.y < height &&
                room.walkable[cell.x, cell.y];
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
