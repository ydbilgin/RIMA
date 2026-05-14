using System.Collections.Generic;
using RIMA.Editor.RoomDesigner;
using RIMA.RoomDesigner.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class EraserBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Eraser;
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
            Tilemap target = ctx.GetActiveTilemap();
            if (target == null) return;
            _buffer.Add(new CellEdit(target, cell, null));
            ctx.MarkDirty();
        }

        public void OnStrokeEnd(IRoomDesignerContext ctx)
        {
            if (_buffer == null) return;
            BrushController.Instance.ApplyStroke(ctx, _buffer, "Erase");
            if (ctx.ActiveLayer == RoomLayer.Wall && ctx.WallsTilemap != null)
            {
                WallAutoConnect.RefreshNeighborhood(ctx.WallsTilemap, _strokeCells, null, ctx.ActiveBlueprint);
                ctx.MarkDirty();
            }

            _buffer = null;
            _visited = null;
        }
    }
}
