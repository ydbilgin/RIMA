using UnityEngine;

namespace RIMA.Editor
{
    public class BrushInputHandler
    {
        public Vector2Int GetCellAtMouse(Vector2 mousePos, float cellSize, float canvasPadding, int roomH)
        {
            float roundedCellSize = Mathf.Max(1f, Mathf.Round(cellSize));
            int x = Mathf.FloorToInt((mousePos.x - canvasPadding) / roundedCellSize);
            int invertedY = Mathf.FloorToInt((mousePos.y - canvasPadding) / roundedCellSize);
            int y = roomH - invertedY - 1;
            return new Vector2Int(x, y);
        }

        public bool IsValidCell(Vector2Int cell, int w, int h)
        {
            return cell.x >= 0 && cell.y >= 0 && cell.x < w && cell.y < h;
        }
    }
}
