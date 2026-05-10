using System.Collections.Generic;
using RIMA.Editor.RoomDesigner;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class BucketFillBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Bucket;

        // Bounded box around click point: +/- MaxRadius in x and y.
        // Caps unbounded flood fill on empty tilemaps (Tilemap has no inherent bounds).
        // Hits bound -> silent stop, commit what filled. Re-click to extend.
        private const int MaxRadius = 50;
        private const int MaxCells = (2 * MaxRadius + 1) * (2 * MaxRadius + 1);

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            var tilemap = ctx.GetActiveTilemap();
            var targetTile = tilemap.GetTile(cell);
            if (targetTile == ctx.ActiveTile) return;

            var origin = cell;
            var queue = new Queue<Vector3Int>();
            var visited = new HashSet<Vector3Int>();
            var edits = new List<CellEdit>();

            queue.Enqueue(cell);
            visited.Add(cell);

            while (queue.Count > 0 && edits.Count < MaxCells)
            {
                var cur = queue.Dequeue();
                edits.Add(new CellEdit(tilemap, cur, ctx.ActiveTile));
                TryEnqueue(tilemap, cur + Vector3Int.right, origin, targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.left,  origin, targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.up,    origin, targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.down,  origin, targetTile, visited, queue);
            }

            if (edits.Count == 0) return;
            BrushController.Instance.ApplyStroke(ctx, edits, "Bucket Fill");
        }

        private static void TryEnqueue(Tilemap tm, Vector3Int n, Vector3Int origin,
            UnityEngine.Tilemaps.TileBase target, HashSet<Vector3Int> visited, Queue<Vector3Int> queue)
        {
            if (Mathf.Abs(n.x - origin.x) > MaxRadius || Mathf.Abs(n.y - origin.y) > MaxRadius) return;
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
