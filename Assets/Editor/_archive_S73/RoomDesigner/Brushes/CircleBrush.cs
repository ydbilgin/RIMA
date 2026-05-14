using System.Collections.Generic;
using RIMA.RoomDesigner.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class CircleBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Circle;
        private List<CellEdit> _buffer;
        private HashSet<Vector3Int> _visited;
        private readonly List<Vector3Int> _strokeCells = new List<Vector3Int>();

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            _buffer = new List<CellEdit>();
            _visited = new HashSet<Vector3Int>();
            _strokeCells.Clear();
            OnStrokeContinue(ctx, cell);
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell)
        {
            if (_buffer == null || _visited.Contains(cell)) return;

            int radius = ctx.BrushRadius;
            float radiusSq = radius * radius;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (x * x + y * y <= radiusSq)
                    {
                        Vector3Int currentCell = cell + new Vector3Int(x, y, 0);
                        if (!_visited.Contains(currentCell))
                        {
                            _visited.Add(currentCell);
                            _strokeCells.Add(currentCell);
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
            ctx.MarkDirty();
        }

        public void OnStrokeEnd(IRoomDesignerContext ctx)
        {
            if (_buffer == null) return;
            BrushController.Instance.ApplyStroke(ctx, _buffer, "Circle Brush");

            if (ctx.ActiveLayer == RoomLayer.Wall && ctx.WallsTilemap != null)
            {
                if (ctx.IsWallOverrideMode && ctx.ActiveBlueprint != null)
                {
                    var bp = ctx.ActiveBlueprint;
                    int total = bp.roomWidth * bp.roomHeight;
                    if (bp.overrideVariantIndex == null || bp.overrideVariantIndex.Length != total)
                        bp.overrideVariantIndex = new bool[total];
                    foreach (var c in _strokeCells)
                    {
                        int idx = (c.y - bp.roomOrigin.y) * bp.roomWidth + (c.x - bp.roomOrigin.x);
                        if (idx >= 0 && idx < total) bp.overrideVariantIndex[idx] = true;
                    }
                }
                else
                {
                    WallAutoConnect.RefreshNeighborhood(ctx.WallsTilemap, _strokeCells, null, ctx.ActiveBlueprint);
                }
                ctx.MarkDirty();
            }

            _buffer = null;
            _visited = null;
        }
    }
}
