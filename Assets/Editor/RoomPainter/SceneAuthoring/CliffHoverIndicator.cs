using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomPainter.Editor
{
    /// <summary>
    /// D5: SceneView hover indicator for Cliff mode.
    /// Draws a cell outline whose colour depends on cell state:
    ///   red   = in ManualOverrideCells (erased)
    ///   green = in ManualPaintedCells (painted)
    ///   grey  = auto-placed by CliffAutoPlacer
    ///   none  = empty cell (no indicator)
    /// </summary>
    [InitializeOnLoad]
    internal static class CliffHoverIndicator
    {
        private static readonly Color ColorErased  = new Color(0.90f, 0.20f, 0.20f, 0.80f);
        private static readonly Color ColorPainted = new Color(0.25f, 0.85f, 0.35f, 0.80f);
        private static readonly Color ColorAuto    = new Color(0.60f, 0.60f, 0.60f, 0.70f);

        // Enabled only when a RimaRoomPainterWindow is open and in Cliff mode.
        internal static bool Active { get; set; }

        static CliffHoverIndicator()
        {
            SceneView.duringSceneGui += OnSceneGui;
        }

        private static void OnSceneGui(SceneView sceneView)
        {
            if (!Active) return;

            Event e = Event.current;
            if (e == null) return;

            // Find CliffAutoPlacer in scene
            RIMA.Environment.CliffAutoPlacer placer =
                Object.FindAnyObjectByType<RIMA.Environment.CliffAutoPlacer>();
            if (placer == null || placer.cliffTilemap == null) return;

            Tilemap cliff = placer.cliffTilemap;
            Grid grid = cliff.layoutGrid;
            if (grid == null) return;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Vector3 worldPos = ray.origin;
            worldPos.z = 0f;
            Vector3Int cell = grid.WorldToCell(worldPos);

            bool isErased  = placer.ManualOverrideCells.Contains(cell);
            bool isPainted = placer.ManualPaintedCells.Contains(cell);
            bool hasAuto   = cliff.HasTile(cell) && !isErased && !isPainted;

            Color outlineColor;
            if (isErased)        outlineColor = ColorErased;
            else if (isPainted)  outlineColor = ColorPainted;
            else if (hasAuto)    outlineColor = ColorAuto;
            else                 return; // empty cell — no indicator

            DrawCellOutline(grid, cell, outlineColor);
            sceneView.Repaint();
        }

        private static void DrawCellOutline(Grid grid, Vector3Int cell, Color color)
        {
            // Use grid cell size to build a rectangular outline centred on the cell.
            Vector3 center = grid.GetCellCenterWorld(cell);
            Vector3 size = grid.cellSize;
            float hw = size.x * 0.5f;
            float hh = size.y * 0.5f;

            Vector3 p0 = center + new Vector3(-hw,  0f, 0f);  // left
            Vector3 p1 = center + new Vector3(  0f, hh, 0f);  // top
            Vector3 p2 = center + new Vector3( hw,  0f, 0f);  // right
            Vector3 p3 = center + new Vector3(  0f,-hh, 0f);  // bottom

            Color prevColor = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(3f, p0, p1, p2, p3, p0);
            Handles.color = prevColor;
        }
    }
}
