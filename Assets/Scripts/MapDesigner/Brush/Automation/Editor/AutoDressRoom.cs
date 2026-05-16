#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Executors;
using RIMA.MapDesigner.Brush.Executors.Editor;
using RIMA.MapDesigner.Brush.Stroke;

namespace RIMA.MapDesigner.Brush.Automation.Editor
{
    public static class AutoDressRoom
    {
        public static BrushExecutorResult Run(BrushPackSO pack, RoomData room, BiomeSkinSO skin, int seed)
        {
            if (pack == null || pack.brushes == null || pack.brushes.Count == 0)
            {
                return new BrushExecutorResult { success = false, errorMessage = "BrushPack is null or empty" };
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
            Undo.SetCurrentGroupName("Auto-Dress: " + (pack.packName ?? "Unnamed"));

            try
            {
                // 1. Wall pass via BrushAlongEdges
                var wallBrush = pack.brushes.FirstOrDefault(b => b != null && b.category == BrushCategory.Wall);
                if (wallBrush != null)
                {
                    var wallResult = BrushAlongEdgesAutomation.Run(wallBrush, room, skin, seed);
                    Merge(result, wallResult);
                }

                // 2. Transition pass (L4)
                foreach (var brush in pack.brushes.Where(b => b != null && b.category == BrushCategory.Transition))
                {
                    ScatterAcrossRoom(brush, room, skin, seed, router, result);
                }

                // 3. Detail pass (L5)
                foreach (var brush in pack.brushes.Where(b => b != null && b.category == BrushCategory.Detail))
                {
                    ScatterAcrossRoom(brush, room, skin, seed, router, result);
                }

                // 4. Accent pass (L6) — sparse
                foreach (var brush in pack.brushes.Where(b => b != null && b.category == BrushCategory.RiftAccent))
                {
                    ScatterAcrossRoom(brush, room, skin, seed, router, result);
                }
            }
            finally
            {
                Undo.CollapseUndoOperations(group);
            }

            return result;
        }

        private static void ScatterAcrossRoom(MapDesignerBrushPresetSO brush, RoomData room, BiomeSkinSO skin,
                                              int seed, BrushExecutorRouter router, BrushExecutorResult acc)
        {
            if (brush.operations == null || brush.operations.Count == 0) return;
            int width = room.size.x > 0 ? room.size.x : (room.walkable != null ? room.walkable.GetLength(0) : 0);
            int height = room.size.y > 0 ? room.size.y : (room.walkable != null ? room.walkable.GetLength(1) : 0);
            if (width <= 0 || height <= 0) return;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = new Vector2Int(x, y);
                    foreach (var op in brush.operations)
                    {
                        if (op == null) continue;
                        Vector2 worldPos = new Vector2(x + 0.5f, y + 0.5f);
                        float density = Karar143Enforcement.EffectiveDensity(cell, worldPos, room, op);
                        if (density <= 0f) continue;

                        float h = Hash01(seed, cell.x, cell.y, (int)op.targetLayer);
                        if (h > density) continue;

                        var stroke = new BrushStroke
                        {
                            currentCell = cell,
                            currentPositionWorld = worldPos,
                            room = room,
                            biomeSkin = skin,
                            seed = seed ^ (cell.x * 73) ^ (cell.y * 17)
                        };
                        var sub = router.Dispatch(stroke, op, brush);
                        Merge(acc, sub);
                    }
                }
            }
        }

        private static void Merge(BrushExecutorResult acc, BrushExecutorResult sub)
        {
            if (sub.spawnedObjects != null) acc.spawnedObjects.AddRange(sub.spawnedObjects);
            if (sub.modifiedAssets != null) acc.modifiedAssets.AddRange(sub.modifiedAssets);
            acc.spawnedCount += sub.spawnedCount;
        }

        private static float Hash01(int seed, int x, int y, int layer)
        {
            unchecked
            {
                uint hash = 2166136261u;
                hash = (hash ^ (uint)seed) * 16777619u;
                hash = (hash ^ (uint)x) * 16777619u;
                hash = (hash ^ (uint)y) * 16777619u;
                hash = (hash ^ (uint)layer) * 16777619u;
                hash ^= hash >> 13;
                hash *= 1274126177u;
                return (hash & 0x00FFFFFF) / 16777215f;
            }
        }
    }
}
#endif
