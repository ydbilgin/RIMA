using RIMA.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    [ExecuteAlways]
    public sealed class DetailDecalPainter : TransitionBrushPainter
    {
        [SerializeField] private string detailSortingLayerName = "Detail";
        [SerializeField] private int detailSortingOrder = 3;

        public void PaintDetails(Tilemap baseTilemap, RoomData room, PatchAtlasSO atlas, int seed)
        {
            PaintAtlas("DetailDecalLayer", detailSortingLayerName, detailSortingOrder, baseTilemap, room, atlas, seed, 0.42f);
        }

        public static float DensityForCell(Vector2Int cell, RoomData room, float baseDensity)
        {
            return TransitionBrushPainter.DensityForCell(cell, room, baseDensity);
        }

        public static float DensityForCell(Vector2Int cell, RoomData room, float baseDensity, FeatureMaskSO featureMask, float featureMaskWeight)
        {
            return TransitionBrushPainter.DensityForCell(cell, room, baseDensity, null, featureMask, featureMaskWeight);
        }
    }
}
