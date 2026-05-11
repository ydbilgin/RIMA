using System.Collections.Generic;
using RIMA.Editor.RoomDesigner;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class StampBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Stamp;
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
            _visited.Add(cell);
            _strokeCells.Add(cell);
            _buffer.Add(new CellEdit(ctx.GetActiveTilemap(), cell, ctx.ActiveTile));
            ctx.MarkDirty();
        }

        public void OnStrokeEnd(IRoomDesignerContext ctx)
        {
            if (_buffer == null) return;
            BrushController.Instance.ApplyStroke(ctx, _buffer, "Stamp");
            if (ctx.ActiveLayer == RoomLayer.Walls && ctx.WallsTilemap != null)
            {
                if (ctx.IsWallOverrideMode && ctx.ActiveBlueprint != null)
                {
                    var bp = ctx.ActiveBlueprint;
                    int total = bp.roomWidth * bp.roomHeight;
                    if (bp.overrideVariantIndex == null || bp.overrideVariantIndex.Length != total)
                        bp.overrideVariantIndex = new bool[total];
                    foreach (var cell in _strokeCells)
                    {
                        int idx = (cell.y - bp.roomOrigin.y) * bp.roomWidth + (cell.x - bp.roomOrigin.x);
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
