using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Stroke
{
    [Serializable]
    public struct BrushStroke
    {
        public Vector2 startPositionWorld;
        public Vector2 currentPositionWorld;
        public Vector2Int startCell;
        public Vector2Int currentCell;
        public bool isDrag;
        public int seed;
        public RoomData room;
        public BiomeSkinSO biomeSkin;
        public List<Vector2> strokePath;
    }
}
