#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using RIMA.MapDesigner.Brush.Executors.Editor;

namespace RIMA.MapDesigner.Brush.Editor.UI.Panels
{
    public class BrushSceneTooling
    {
        private readonly MapDesignerBrushWindow window;
        private BrushExecutorRouter router;
        private bool dragActive;
        private Vector2 lastDragWorld;
        private int activeControlId;
        private int undoGroupAtPress;

        public BrushSceneTooling(MapDesignerBrushWindow w)
        {
            window = w;
            router = new BrushExecutorRouter();
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (window == null) return;
            if (window.ToolMode != BrushToolMode.Brush && window.ToolMode != BrushToolMode.Erase) return;

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlID);

            Event e = Event.current;
            Vector2 worldPos = GetMouseWorldPos(e);
            Vector2Int cell = WorldToCell(worldPos);

            DrawGhost(worldPos);

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        GUIUtility.hotControl = controlID;
                        activeControlId = controlID;
                        dragActive = true;
                        lastDragWorld = worldPos;
                        BeginStrokeUndoGroup();
                        ApplyStroke(worldPos, cell, isDrag: false);
                        e.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID && dragActive)
                    {
                        ApplyStroke(worldPos, cell, isDrag: true);
                        lastDragWorld = worldPos;
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID && dragActive)
                    {
                        EndStrokeUndoGroup();
                        dragActive = false;
                        GUIUtility.hotControl = 0;
                        e.Use();
                    }
                    break;
                case EventType.Repaint:
                    SceneView.RepaintAll();
                    break;
            }
        }

        private Vector2 GetMouseWorldPos(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            return new Vector2(ray.origin.x, ray.origin.y);
        }

        private static Vector2Int WorldToCell(Vector2 world)
        {
            return new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));
        }

        private void DrawGhost(Vector2 worldPos)
        {
            if (window.SelectedBrush == null) return;
            Handles.color = new Color(0.5f, 0.85f, 1f, 0.5f);
            float radius = Mathf.Max(0.25f, window.BrushSize / 64f);
            Handles.DrawWireDisc(new Vector3(worldPos.x, worldPos.y, 0f), Vector3.forward, radius);
        }

        private void BeginStrokeUndoGroup()
        {
            Undo.IncrementCurrentGroup();
            undoGroupAtPress = Undo.GetCurrentGroup();
            string label = window.SelectedBrush != null ? $"RIMA Brush: {window.SelectedBrush.brushName}" : "RIMA Brush Stroke";
            Undo.SetCurrentGroupName(label);
        }

        private void EndStrokeUndoGroup()
        {
            Undo.CollapseUndoOperations(undoGroupAtPress);
        }

        private void ApplyStroke(Vector2 worldPos, Vector2Int cell, bool isDrag)
        {
            var brush = window.SelectedBrush;
            if (brush == null || brush.operations == null || brush.operations.Count == 0) return;
            if (router == null) router = new BrushExecutorRouter();

            var stroke = new BrushStroke
            {
                startPositionWorld = isDrag ? lastDragWorld : worldPos,
                currentPositionWorld = worldPos,
                startCell = isDrag ? WorldToCell(lastDragWorld) : cell,
                currentCell = cell,
                isDrag = isDrag,
                seed = window.ActiveSeed,
                biomeSkin = window.ActiveSkin
            };

            foreach (var op in brush.operations)
            {
                if (op == null) continue;
                router.Dispatch(stroke, op, brush);
            }
        }
    }
}
#endif
