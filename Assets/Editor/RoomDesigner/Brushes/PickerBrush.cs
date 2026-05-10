using UnityEngine;

namespace RIMA.Editor.RoomDesigner
{
    public class PickerBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Picker;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            ctx.ActiveTile = ctx.GetActiveTilemap().GetTile(cell);
            ctx.ActiveBrush = BrushMode.Stamp;
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell) { }
        public void OnStrokeEnd(IRoomDesignerContext ctx) { }
    }
}
