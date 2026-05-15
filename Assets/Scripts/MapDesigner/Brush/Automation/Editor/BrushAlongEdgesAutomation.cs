#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.Data;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Automation.Editor
{
    public static class BrushAlongEdgesAutomation
    {
        public static BrushExecutorResult Run(MapDesignerBrushPresetSO wallBrush, RoomData room, BiomeSkinSO skin, int seed)
        {
            if (wallBrush == null)
            {
                return Error("Brush is null");
            }

            if (wallBrush.category != BrushCategory.Wall)
            {
                return Error("Brush is not Wall category");
            }

            var result = new BrushExecutorResult
            {
                success = true,
                spawnedObjects = new List<GameObject>(),
                modifiedAssets = new List<UnityEngine.Object>()
            };
            var router = new BrushExecutorRouter();

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Brush Along Edges");

            if (room.wallEdges != null && wallBrush.operations != null)
            {
                foreach (WallSegment seg in room.wallEdges)
                {
                    if (seg.isDoorway)
                    {
                        continue;
                    }

                    var stroke = new BrushStroke
                    {
                        currentCell = seg.start,
                        room = room,
                        biomeSkin = skin,
                        seed = seed + seg.start.x * 73 + seg.start.y * 17
                    };

                    foreach (BrushLayerOperation op in wallBrush.operations)
                    {
                        BrushExecutorResult subResult = router.Dispatch(stroke, op, wallBrush);
                        if (!subResult.success)
                        {
                            result.success = false;
                            result.errorMessage = subResult.errorMessage;
                            continue;
                        }

                        if (subResult.spawnedObjects != null)
                        {
                            result.spawnedObjects.AddRange(subResult.spawnedObjects);
                        }

                        if (subResult.modifiedAssets != null)
                        {
                            result.modifiedAssets.AddRange(subResult.modifiedAssets);
                        }

                        result.spawnedCount += subResult.spawnedCount;
                    }
                }
            }

            Undo.CollapseUndoOperations(group);
            return result;
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
