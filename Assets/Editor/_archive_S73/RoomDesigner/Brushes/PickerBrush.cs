using RIMA.Editor.RoomDesigner;
using RIMA.RoomDesigner.Core;
using UnityEngine;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class PickerBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Picker;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            var tilemap = ctx.GetActiveTilemap();
            if (tilemap == null) return;
            ctx.ActiveTile = tilemap.GetTile(cell);
            if (BrushController.Instance != null)
            {
                BrushController.Instance.SetBrush(ctx, BrushMode.Stamp);
            }
            else
            {
                ctx.ActiveBrush = BrushMode.Stamp;
            }
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell) { }
        public void OnStrokeEnd(IRoomDesignerContext ctx) { }
    }
}
