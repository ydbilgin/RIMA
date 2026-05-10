using RIMA.Editor.RoomDesigner;
using UnityEngine;

namespace RIMA.Editor.RoomDesigner.Brushes
{
    public class PickerBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Picker;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            ctx.ActiveTile = ctx.GetActiveTilemap().GetTile(cell);
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
