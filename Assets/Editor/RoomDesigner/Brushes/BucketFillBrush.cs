using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public class BucketFillBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Bucket;
        private const int MaxCells = 10000;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            var tilemap = ctx.GetActiveTilemap();
            var targetTile = tilemap.GetTile(cell);
            if (targetTile == ctx.ActiveTile) return;

            var queue = new Queue<Vector3Int>();
            var visited = new HashSet<Vector3Int>();
            var edits = new List<CellEdit>();

            queue.Enqueue(cell);
            visited.Add(cell);

            while (queue.Count > 0)
            {
                if (edits.Count >= MaxCells)
                {
                    EditorUtility.DisplayDialog("Flood Fill",
                        "Selection exceeds 10000 cells. Operation aborted.", "OK");
                    return;
                }
                var cur = queue.Dequeue();
                edits.Add(new CellEdit(tilemap, cur, ctx.ActiveTile));
                TryEnqueue(tilemap, cur + Vector3Int.right,  targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.left,   targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.up,     targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.down,   targetTile, visited, queue);
            }

            BrushController.Instance.ApplyStroke(ctx, edits, "Bucket Fill");
        }

        private static void TryEnqueue(Tilemap tm, Vector3Int n, UnityEngine.Tilemaps.TileBase target,
            HashSet<Vector3Int> visited, Queue<Vector3Int> queue)
        {
            if (!visited.Contains(n) && tm.GetTile(n) == target)
            {
                visited.Add(n);
                queue.Enqueue(n);
            }
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell) { }
        public void OnStrokeEnd(IRoomDesignerContext ctx) { }
    }
}
