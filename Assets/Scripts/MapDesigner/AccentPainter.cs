using RIMA.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public sealed class AccentPainter : TransitionBrushPainter
    {
        [SerializeField] private string accentSortingLayerName = "Accent";
        [SerializeField] private int accentSortingOrder = 4;

        public void PaintAccents(Tilemap baseTilemap, RoomData room, PatchAtlasSO atlas, int seed)
        {
            PaintAtlas("AccentLayer", accentSortingLayerName, accentSortingOrder, baseTilemap, room, atlas, seed + 907, 0.25f);
        }
    }
}
