using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomPainter.Editor
{
    /// <summary>
    /// D5.5: Free-form Decor Cliff painter.
    /// Activated in Cliff mode while Shift is held.
    /// Paints to DecorCliffTilemap (no floor-neighbor check — free placement).
    /// Shift+Click  → paint selected cliff tile
    /// Shift+Alt+Click → erase
    /// </summary>
    internal static class DecorCliffPainter
    {
        private const string DecorTilemapName = "DecorCliffTilemap";

        // Counter exposed to statusbar
        internal static int DecorPaintedThisSession { get; private set; }

        internal static void ResetSessionCounter() { DecorPaintedThisSession = 0; }

        /// <summary>
        /// Called from RimaRoomPainterWindow.OnSceneGui when Cliff mode is active.
        /// Returns true if the event was consumed.
        /// </summary>
        internal static bool OnSceneGui(SceneView sceneView, TileBase cliffTile)
        {
            Event e = Event.current;
            if (e == null) return false;

            // Only act on Shift+LMB (paint) or Shift+Alt+LMB (erase) mouse-down / mouse-drag
            bool isShift = e.shift;
            if (!isShift) return false;

            if (e.type != EventType.MouseDown && e.type != EventType.MouseDrag) return false;
            if (e.button != 0) return false;

            Tilemap decorTilemap = FindOrWarnDecorTilemap();
            if (decorTilemap == null) return false;

            // World position under cursor
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Vector3 worldPos = ray.origin;
            worldPos.z = 0f;

            Grid grid = decorTilemap.layoutGrid;
            if (grid == null) return false;

            Vector3Int cell = grid.WorldToCell(worldPos);

            bool erase = e.alt;
            if (erase)
            {
                decorTilemap.SetTile(cell, null);
            }
            else
            {
                if (cliffTile != null)
                {
                    decorTilemap.SetTile(cell, cliffTile);
                    DecorPaintedThisSession++;
                }
                else
                {
                    Debug.LogWarning("[DecorCliffPainter] No cliff tile assigned on CliffAutoPlacer — cannot paint.");
                }
            }

            EditorUtility.SetDirty(decorTilemap.gameObject);
            e.Use();
            sceneView.Repaint();
            return true;
        }

        private static Tilemap FindOrWarnDecorTilemap()
        {
            var go = GameObject.Find(DecorTilemapName);
            if (go == null)
            {
                Debug.LogWarning("[DecorCliffPainter] '" + DecorTilemapName + "' not found in scene. Add it via the Inspector 'Create DecorCliffTilemap' button.");
                return null;
            }

            var tilemap = go.GetComponent<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogWarning("[DecorCliffPainter] '" + DecorTilemapName + "' has no Tilemap component.");
                return null;
            }

            return tilemap;
        }
    }
}
