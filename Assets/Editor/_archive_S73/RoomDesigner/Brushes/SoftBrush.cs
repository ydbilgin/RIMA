using System.Collections.Generic;
using RIMA.RoomDesigner.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class SoftBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Soft;
        private List<CellEdit> _buffer;
        private HashSet<Vector3Int> _visited;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            _buffer = new List<CellEdit>();
            _visited = new HashSet<Vector3Int>();
            OnStrokeContinue(ctx, cell);
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell)
        {
            if (_buffer == null || _visited.Contains(cell)) return;

            int radius = ctx.BrushRadius;
            float maxDist = radius;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector3Int currentCell = cell + new Vector3Int(x, y, 0);
                    if (!_visited.Contains(currentCell))
                    {
                        float dist = Mathf.Sqrt(x * x + y * y);
                        if (dist <= maxDist)
                        {
                            _visited.Add(currentCell);

                            float normalizedDist = maxDist > 0 ? dist / maxDist : 0;
                            float probability = 1f - (normalizedDist * ctx.BrushFalloff);

                            if (Random.value <= probability)
                            {
                                Tilemap target = ctx.GetActiveTilemap();
                                if (target == null) return;
                                _buffer.Add(new CellEdit(target, currentCell, ctx.ActiveTile));

                                if (ctx.ActiveLayer == RoomLayer.Base && ctx.AutoCliff)
                                {
                                    Vector3Int cliffCell = currentCell + new Vector3Int(0, -1, 0);
                                    if (ctx.WallsTilemap != null)
                                    {
                                        if (ctx.ActiveTile == null)
                                        {
                                            _buffer.Add(new CellEdit(ctx.WallsTilemap, cliffCell, null));
                                        }
                                        else if (ctx.CliffTile != null)
                                        {
                                            _buffer.Add(new CellEdit(ctx.WallsTilemap, cliffCell, ctx.CliffTile));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ctx.MarkDirty();
        }

        public void OnStrokeEnd(IRoomDesignerContext ctx)
        {
            if (_buffer == null) return;
            BrushController.Instance.ApplyStroke(ctx, _buffer, "Soft Brush");
            _buffer = null;
            _visited = null;
        }
    }
}
