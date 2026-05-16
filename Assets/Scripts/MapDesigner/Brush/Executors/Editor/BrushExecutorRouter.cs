#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

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
            RegisterIfAvailable("RIMA.MapDesigner.Brush.Executors.Editor.CompositeStrokeExecutor");
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
                preset.paintMode != PaintMode.CompositeStroke &&
                !IsCellWalkable(stroke.currentCell, stroke.room))
            {
                return new BrushExecutorResult { success = true, spawnedCount = 0 };
            }

            IBrushExecutor exec;
            if (!registry.TryGetValue(preset.paintMode, out exec))
            {
                return Error("No executor for " + preset.paintMode);
            }

            if (preset.paintMode == PaintMode.CompositeStroke)
            {
                return InvokeComposite(exec, stroke, preset);
            }

            return exec.Apply(stroke, op);
        }

        public BrushExecutorResult DispatchWithMode(BrushStroke stroke, BrushLayerOperation op, PaintMode mode)
        {
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
            if (!registry.TryGetValue(mode, out exec))
            {
                return Error("No executor for " + mode);
            }

            return exec.Apply(stroke, op);
        }

        private void RegisterIfAvailable(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    type = assemblies[i].GetType(typeName);
                    if (type != null)
                    {
                        break;
                    }
                }
            }

            if (type == null || !typeof(IBrushExecutor).IsAssignableFrom(type))
            {
                return;
            }

            IBrushExecutor executor = Activator.CreateInstance(type) as IBrushExecutor;
            Register(executor);
        }

        private BrushExecutorResult InvokeComposite(
            IBrushExecutor exec,
            BrushStroke stroke,
            MapDesignerBrushPresetSO preset)
        {
            System.Reflection.MethodInfo applyComposite = exec.GetType().GetMethod(
                "ApplyComposite",
                new[] { typeof(BrushStroke), typeof(MapDesignerBrushPresetSO), typeof(BrushExecutorRouter) });

            if (applyComposite == null)
            {
                return Error("Composite executor does not expose ApplyComposite");
            }

            return (BrushExecutorResult)applyComposite.Invoke(exec, new object[] { stroke, preset, this });
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

    public sealed partial class CompositeStrokeExecutor : IBrushExecutor
    {
        public PaintMode SupportedMode
        {
            get { return PaintMode.CompositeStroke; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            return new BrushExecutorResult
            {
                success = false,
                errorMessage = "CompositeStroke requires ApplyComposite"
            };
        }

        public BrushExecutorResult ApplyComposite(
            BrushStroke stroke,
            MapDesignerBrushPresetSO preset,
            BrushExecutorRouter router)
        {
            if (preset == null)
            {
                return Error("Preset is null");
            }

            if (preset.paintMode != PaintMode.CompositeStroke)
            {
                return Error("Brush is not CompositeStroke");
            }

            if (preset.operations == null || preset.operations.Count == 0)
            {
                return Error("No operations defined");
            }

            if (router == null)
            {
                return Error("Router is null");
            }

            var result = new BrushExecutorResult
            {
                success = true,
                spawnedObjects = new List<GameObject>(),
                modifiedAssets = new List<UnityEngine.Object>()
            };

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Composite Brush: " + preset.brushName);

            try
            {
                for (int i = 0; i < preset.operations.Count; i++)
                {
                    BrushLayerOperation op = preset.operations[i];
                    if (op == null || !PassesProbability(stroke.seed, op, i))
                    {
                        continue;
                    }

                    PaintMode subMode = MapOpToSubMode(op);
                    BrushExecutorResult subResult = router.DispatchWithMode(stroke, op, subMode);
                    if (!subResult.success)
                    {
                        result.success = false;
                        result.errorMessage = subResult.errorMessage;
                        continue;
                    }

                    result.spawnedCount += subResult.spawnedCount;

                    if (subResult.spawnedObjects != null)
                    {
                        result.spawnedObjects.AddRange(subResult.spawnedObjects);
                    }

                    if (subResult.modifiedAssets != null)
                    {
                        result.modifiedAssets.AddRange(subResult.modifiedAssets);
                    }
                }
            }
            finally
            {
                Undo.CollapseUndoOperations(group);
            }

            return result;
        }

        private static PaintMode MapOpToSubMode(BrushLayerOperation op)
        {
            switch (op.targetLayer)
            {
                case TargetLayer.L1:
                    return PaintMode.GridTile;
                case TargetLayer.L2:
                    return PaintMode.GridTileRandom;
                case TargetLayer.L3:
                    return PaintMode.Stamp;
                case TargetLayer.L4:
                case TargetLayer.L5:
                case TargetLayer.L6:
                    return PaintMode.FreeformDecal;
                default:
                    return PaintMode.FreeformDecal;
            }
        }

        private static bool PassesProbability(int seed, BrushLayerOperation op, int index)
        {
            float probability = Mathf.Clamp01(op.probability);
            if (probability <= 0f)
            {
                return false;
            }

            if (probability >= 1f)
            {
                return true;
            }

            return Hash01(seed, (int)op.targetLayer, index) <= probability;
        }

        private static float Hash01(int seed, int layer, int index)
        {
            unchecked
            {
                uint hash = 2166136261u;
                hash = (hash ^ (uint)seed) * 16777619u;
                hash = (hash ^ (uint)layer) * 16777619u;
                hash = (hash ^ (uint)index) * 16777619u;
                hash ^= hash >> 13;
                hash *= 1274126177u;
                return (hash & 0x00FFFFFF) / 16777215f;
            }
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
