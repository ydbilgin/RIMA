namespace RIMA.Editor.RoomDesigner
{
    using RIMA.RoomDesigner.Core;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;

    public interface IRoomDesignerContext
    {
        Tilemap BaseTilemap { get; }
        Tilemap DecalTilemap { get; }
        Tilemap WallFrontTilemap { get; }
        Tilemap WallTopTilemap { get; }
        Transform PropContainer { get; }
        Tilemap FloorTilemap { get; }
        Tilemap WallsTilemap { get; }
        Tilemap DecalsTilemap { get; }
        Tilemap GetActiveTilemap();
        RoomLayer ActiveLayer { get; set; }

        TileBase ActiveTile { get; set; }
        BrushMode ActiveBrush { get; set; }

        int BrushRadius { get; set; }
        float BrushFalloff { get; set; }
        bool AutoCliff { get; set; }
        TileBase CliffTile { get; set; }

        Vector3Int HoveredCell { get; set; }
        bool IsCanvasHovered { get; }

        RIMA.Runtime.Rooms.RoomBlueprint ActiveBlueprint { get; }
        bool IsWallOverrideMode { get; set; }

        void InvokeBrush(int mouseButton, Vector3Int cell);

        VisualElement LeftPanel { get; }
        VisualElement RightPanel { get; }

        void MarkDirty();
    }
}
