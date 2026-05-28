using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal sealed class PreviewInputHandler
    {
        private bool _isPanning;
        private Vector2 _lastMousePosition;

        public bool Handle(Rect area, ref int zoom, ref Vector2 pan, ref float rotationOverride)
        {
            Event e = Event.current;
            bool changed = false;

            if (e.type == EventType.ScrollWheel && area.Contains(e.mousePosition))
            {
                int nextZoom = Mathf.Clamp(zoom + (e.delta.y < 0f ? 1 : -1), 0, 8);
                if (nextZoom != zoom)
                {
                    zoom = nextZoom;
                    changed = true;
                }

                e.Use();
            }

            bool middleMouseDown = e.type == EventType.MouseDown && e.button == 2; // Mouse.button == 2
            if (middleMouseDown && area.Contains(e.mousePosition))
            {
                _isPanning = true;
                _lastMousePosition = e.mousePosition;
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && _isPanning)
            {
                pan += e.mousePosition - _lastMousePosition;
                _lastMousePosition = e.mousePosition;
                changed = true;
                e.Use();
            }
            else if (e.type == EventType.MouseUp && e.button == 2 && _isPanning)
            {
                _isPanning = false;
                e.Use();
            }

            if (e.type == EventType.KeyDown && area.Contains(e.mousePosition))
            {
                if (e.keyCode == KeyCode.R)
                {
                    rotationOverride = Mathf.Repeat(rotationOverride + 90f, 360f);
                    changed = true;
                    e.Use();
                }
                else if (e.keyCode == KeyCode.Alpha0 || e.keyCode == KeyCode.Keypad0)
                {
                    zoom = 0;
                    pan = Vector2.zero;
                    rotationOverride = 0f;
                    changed = true;
                    e.Use();
                }
            }

            return changed;
        }
    }
}
