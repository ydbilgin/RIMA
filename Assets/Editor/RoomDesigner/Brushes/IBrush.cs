using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public interface IBrush
    {
        BrushMode Mode { get; }
        void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton);
        void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell);
        void OnStrokeEnd(IRoomDesignerContext ctx);
    }

    public readonly struct CellEdit
    {
        public readonly Tilemap Target;
        public readonly Vector3Int Cell;
        public readonly TileBase Tile;
        public CellEdit(Tilemap t, Vector3Int c, TileBase tile) { Target = t; Cell = c; Tile = tile; }
    }
}
