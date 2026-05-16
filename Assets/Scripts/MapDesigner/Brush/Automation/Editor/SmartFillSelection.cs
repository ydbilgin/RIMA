#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;

namespace RIMA.MapDesigner.Brush.Automation.Editor
{
    public static class SmartFillSelection
    {
        public static BrushExecutorResult Run(RectInt selection, MapDesignerBrushPresetSO brush,
                                              RoomData room, BiomeSkinSO skin, int seed)
        {
            if (brush == null)
            {
                return new BrushExecutorResult { success = false, errorMessage = "Brush is null" };
            }
            if (brush.operations == null || brush.operations.Count == 0)
            {
                return new BrushExecutorResult { success = false, errorMessage = "Brush has no operations" };
            }

            var router = new BrushExecutorRouter();
            var result = new BrushExecutorResult
            {
                success = true,
                spawnedObjects = new List<GameObject>(),
                modifiedAssets = new List<UnityEngine.Object>()
            };

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Smart Fill: " + (brush.brushName ?? "Brush"));

            try
            {
                int width = room.size.x > 0 ? room.size.x : (room.walkable != null ? room.walkable.GetLength(0) : 0);
                int height = room.size.y > 0 ? room.size.y : (room.walkable != null ? room.walkable.GetLength(1) : 0);

                int xMin = Mathf.Max(selection.xMin, 0);
                int yMin = Mathf.Max(selection.yMin, 0);
                int xMax = Mathf.Min(selection.xMax, width);
                int yMax = Mathf.Min(selection.yMax, height);

                for (int x = xMin; x < xMax; x++)
                {
                    for (int y = yMin; y < yMax; y++)
                    {
                        var cell = new Vector2Int(x, y);
                        Vector2 worldPos = new Vector2(x + 0.5f, y + 0.5f);
                        var stroke = new BrushStroke
                        {
                            currentCell = cell,
                            currentPositionWorld = worldPos,
                            room = room,
                            biomeSkin = skin,
                            seed = seed ^ (cell.x * 73) ^ (cell.y * 17)
                        };

                        foreach (var op in brush.operations)
                        {
                            if (op == null) continue;
                            float density = Karar143Enforcement.EffectiveDensity(cell, worldPos, room, op);
                            if (density <= 0f) continue;

                            float h = Hash01(stroke.seed, (int)op.targetLayer);
                            if (h > density) continue;

                            var sub = router.Dispatch(stroke, op, brush);
                            if (sub.spawnedObjects != null) result.spawnedObjects.AddRange(sub.spawnedObjects);
                            if (sub.modifiedAssets != null) result.modifiedAssets.AddRange(sub.modifiedAssets);
                            result.spawnedCount += sub.spawnedCount;
                        }
                    }
                }
            }
            finally
            {
                Undo.CollapseUndoOperations(group);
            }

            return result;
        }

        private static float Hash01(int seed, int layer)
        {
            unchecked
            {
                uint hash = 2166136261u;
                hash = (hash ^ (uint)seed) * 16777619u;
                hash = (hash ^ (uint)layer) * 16777619u;
                hash ^= hash >> 13;
                hash *= 1274126177u;
                return (hash & 0x00FFFFFF) / 16777215f;
            }
        }
    }
}
#endif
