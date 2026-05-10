namespace RIMA.Editor.RoomDesigner
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;

    public interface IRoomDesignerContext
    {
        Tilemap FloorTilemap { get; }
        Tilemap WallsTilemap { get; }
        Tilemap DecalsTilemap { get; }
        Tilemap GetActiveTilemap();
        RoomLayer ActiveLayer { get; set; }

        TileBase ActiveTile { get; set; }
        BrushMode ActiveBrush { get; set; }

        Vector3Int HoveredCell { get; set; }
        bool IsCanvasHovered { get; }

        void InvokeBrush(int mouseButton, Vector3Int cell);

        VisualElement LeftPanel { get; }
        VisualElement RightPanel { get; }

        void MarkDirty();
    }

    public enum RoomLayer
    {
        Floor,
        Walls,
        Decals
    }

    public enum BrushMode
    {
        Stamp,
        Eraser,
        Picker,
        Bucket
    }
}
