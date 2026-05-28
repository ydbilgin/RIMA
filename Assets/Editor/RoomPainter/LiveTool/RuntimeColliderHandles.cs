using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RIMA.Live;
using RIMA.RoomPainter;
using RIMA.RoomPainter.Editor;   // ColliderShapeSwapper

namespace RIMA.Editor.RoomPainter.LiveTool
{
    /// <summary>
    /// C7 — RuntimeColliderHandles (F4).
    /// Inline IMGUI collider-editing panel for the Live Tool palette window.
    ///
    /// Replaces SceneView Handles.FreeMoveHandle (RoomPainterColliderEditor) with an
    /// IMGUI-rect version that works inside an EditorWindow without a SceneView open.
    ///
    /// Responsibilities:
    ///   - Read SelectedEntry from LiveToolPaletteWindow.Palette (F3 dependency).
    ///   - Draw a scaled 2-D outline of the prefab's Collider2D in a fixed preview rect.
    ///   - Place draggable dot handles at corners/edges; mouse drag → update collider size/offset.
    ///   - Collider type selector (Box/Circle/Capsule) → delegates to ColliderShapeSwapper.
    ///   - Undo stack: Stack&lt;ColliderState&gt; depth 32. Ctrl+Z pops and restores.
    ///   - Exposes Draw(Rect canvasRect) so callers embed this in any IMGUI layout.
    ///
    /// Integration pattern (embed in a docked EditorWindow):
    /// <code>
    ///   RuntimeColliderHandles handles = new RuntimeColliderHandles();
    ///   // In OnGUI:
    ///   Rect r = GUILayoutUtility.GetRect(280, 240);
    ///   handles.Draw(r);
    /// </code>
    ///
    /// Relationship to ColliderShapeSwapper:
    ///   ColliderShapeSwapper handles component type-swap (Box↔Circle↔Capsule) with Undo.
    ///   RuntimeColliderHandles handles drag-resize of size/offset, and calls ColliderShapeSwapper
    ///   when the user changes shape — no duplicate logic.
    /// </summary>
    public sealed class RuntimeColliderHandles
    {
        // ── Constants ──────────────────────────────────────────────────────────

        private const int   UndoDepth     = 32;
        private const float HandleRadius  = 7f;   // screen-space px hit zone
        private const float HandleDotSize = 5f;   // draw radius px
        private const float MarginPx      = 16f;  // canvas padding

        private static readonly Color OutlineColor        = new Color(0.30f, 0.95f, 0.60f, 1f);
        private static readonly Color OutlineTriggerColor = new Color(1f, 0.85f, 0.25f, 1f);
        private static readonly Color HandleColor         = new Color(0.95f, 0.95f, 0.20f, 1f);
        private static readonly Color HandleHoverColor    = new Color(1f, 1f, 0.55f, 1f);
        private static readonly Color BgColor             = new Color(0.12f, 0.12f, 0.13f, 1f);

        // ── Undo ───────────────────────────────────────────────────────────────

        private struct ColliderState
        {
            public ColliderShape shape;
            public Vector2       size;
            public Vector2       offset;
            public bool          isTrigger;
        }

        private readonly Stack<ColliderState> _undoStack = new Stack<ColliderState>(UndoDepth);

        // ── Drag state ─────────────────────────────────────────────────────────

        private int   _draggingHandle = -1; // -1 = none
        private bool  _wasDragging;

        // ── Cached entry / collider ────────────────────────────────────────────

        private RegistryEntry _cachedEntry;
        private Collider2D    _cachedCollider;

        // ── Public entry point ─────────────────────────────────────────────────

        /// <summary>
        /// Draw the collider-handles panel into <paramref name="canvasRect"/>.
        /// Call from EditorWindow.OnGUI. Consumes mouse events inside the rect.
        /// Returns true when a collider value was changed this frame (caller should
        /// call EditorUtility.SetDirty + AssetDatabase.SaveAssetIfDirty).
        /// </summary>
        public bool Draw(Rect canvasRect)
        {
            RegistryEntry entry = LiveToolPaletteWindow.Palette?.SelectedEntry;
            bool changed = false;

            // Background
            EditorGUI.DrawRect(canvasRect, BgColor);

            if (entry == null || entry.prefab == null)
            {
                DrawNoSelectionLabel(canvasRect);
                return false;
            }

            // If selection changed, clear drag + undo stack
            if (entry != _cachedEntry)
            {
                _cachedEntry    = entry;
                _cachedCollider = entry.prefab.GetComponent<Collider2D>();
                _draggingHandle = -1;
                _undoStack.Clear();
            }
            else
            {
                // Refresh collider reference (may have changed via shape swap)
                _cachedCollider = entry.prefab.GetComponent<Collider2D>();
            }

            // Shape selector strip (top of canvas)
            Rect shapeStripRect = new Rect(canvasRect.x, canvasRect.y, canvasRect.width, 22f);
            changed |= DrawShapeSelector(shapeStripRect, entry.prefab);

            // Preview area (remainder)
            Rect previewRect = new Rect(canvasRect.x, canvasRect.y + 24f,
                                        canvasRect.width, canvasRect.height - 46f);
            changed |= DrawHandleCanvas(previewRect);

            // Bottom info strip
            Rect infoRect = new Rect(canvasRect.x, canvasRect.yMax - 20f, canvasRect.width, 20f);
            DrawInfoStrip(infoRect);

            // Global Ctrl+Z undo
            if (HandleUndoKeyboard())
            {
                changed = true;
            }

            return changed;
        }

        // ── Shape selector ─────────────────────────────────────────────────────

        private bool DrawShapeSelector(Rect strip, GameObject prefab)
        {
            bool changed = false;
            Collider2D col = _cachedCollider; // already refreshed this frame in Draw()
            ColliderShape current = ResolveShape(col);

            GUILayout.BeginArea(strip);
            EditorGUILayout.BeginHorizontal(GUILayout.Height(20f));
            EditorGUILayout.LabelField("Shape:", GUILayout.Width(46f));

            EditorGUI.BeginChangeCheck();
            ColliderShape next = (ColliderShape)EditorGUILayout.EnumPopup(current, GUILayout.Width(78f));
            if (EditorGUI.EndChangeCheck() && next != current)
            {
                if (next == ColliderShape.Polygon)
                {
                    Debug.LogWarning("[RuntimeColliderHandles] Polygon collider deferred — shape not changed.");
                }
                else
                {
                    PushUndo(col);
                    ColliderShapeSwapper.SwapShape(prefab, next);
                    AssetDatabase.SaveAssetIfDirty(prefab);
                    _cachedCollider = prefab.GetComponent<Collider2D>();
                    changed = true;
                }
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
            return changed;
        }

        // ── Handle canvas ──────────────────────────────────────────────────────

        private bool DrawHandleCanvas(Rect canvas)
        {
            if (_cachedCollider == null)
            {
                DrawHelpLabel(canvas, "No Collider2D on prefab root.\nAdd one via Shape selector above.");
                return false;
            }

            EditorGUI.DrawRect(canvas, BgColor);
            DrawBorder(canvas, new Color(0.25f, 0.25f, 0.27f, 1f));

            // Compute scale: fit collider + 20% margin into canvas
            Rect inner = Inflate(canvas, -MarginPx);
            float unitScale = ComputeUnitScale(inner);

            Vector2 canvasCenter = canvas.center;

            if (_cachedCollider is BoxCollider2D box)
                return DrawBoxHandles(box, canvasCenter, unitScale);

            if (_cachedCollider is CircleCollider2D circle)
                return DrawCircleHandles(circle, canvasCenter, unitScale);

            if (_cachedCollider is CapsuleCollider2D capsule)
                return DrawCapsuleHandles(capsule, canvasCenter, unitScale);

            DrawHelpLabel(canvas, "Unsupported collider type.");
            return false;
        }

        // ── Box handles ────────────────────────────────────────────────────────

        // Handle layout (8 total):
        //   Corners: 0=BL 1=BR 2=TR 3=TL
        //   Edges:   4=B  5=R  6=T  7=L

        private static readonly Vector2[] BoxCornerSigns =
        {
            new Vector2(-1f, -1f), new Vector2(+1f, -1f),
            new Vector2(+1f, +1f), new Vector2(-1f, +1f),
        };

        private static readonly Vector2[] BoxEdgeSigns =
        {
            new Vector2(0f, -1f), new Vector2(+1f, 0f),
            new Vector2(0f, +1f), new Vector2(-1f,  0f),
        };

        private bool DrawBoxHandles(BoxCollider2D box, Vector2 center, float scale)
        {
            bool changed = false;
            Color outline = box.isTrigger ? OutlineTriggerColor : OutlineColor;

            Vector2 half    = box.size * 0.5f * scale;
            Vector2 offsetS = box.offset * scale;   // offset in screen-space

            // Draw outline rect
            Rect outlineRect = new Rect(
                center.x + offsetS.x - half.x,
                center.y - offsetS.y - half.y,   // flip y: Unity +y = up, GUI +y = down
                half.x * 2f, half.y * 2f);
            DrawRectOutline(outlineRect, outline);

            // Collect 8 handle positions (screen-space)
            Vector2[] handlePos = new Vector2[8];
            for (int i = 0; i < 4; i++)
            {
                Vector2 s = BoxCornerSigns[i];
                handlePos[i] = new Vector2(
                    center.x + offsetS.x + s.x * half.x,
                    center.y - offsetS.y - s.y * half.y);
            }

            for (int i = 0; i < 4; i++)
            {
                Vector2 s = BoxEdgeSigns[i];
                handlePos[4 + i] = new Vector2(
                    center.x + offsetS.x + s.x * half.x,
                    center.y - offsetS.y - s.y * half.y);
            }

            // Process mouse interactions
            Event e = Event.current;
            changed |= ProcessBoxDrag(box, handlePos, center, scale, e);

            // Draw handles
            for (int i = 0; i < 8; i++)
            {
                bool hovering = (e.type != EventType.Layout) &&
                                Vector2.Distance(handlePos[i], e.mousePosition) < HandleRadius;
                DrawHandleDot(handlePos[i], hovering || _draggingHandle == i
                    ? HandleHoverColor : HandleColor);
            }

            return changed;
        }

        private bool ProcessBoxDrag(BoxCollider2D box, Vector2[] handlePos,
                                    Vector2 center, float scale, Event e)
        {
            bool changed = false;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                for (int i = 0; i < handlePos.Length; i++)
                {
                    if (Vector2.Distance(handlePos[i], e.mousePosition) < HandleRadius)
                    {
                        PushUndo(box);
                        _draggingHandle = i;
                        _wasDragging    = true;
                        e.Use();
                        break;
                    }
                }
            }
            else if (e.type == EventType.MouseDrag && _draggingHandle >= 0 && _wasDragging)
            {
                // delta in world-units
                float dx =  e.delta.x / scale;
                float dy = -e.delta.y / scale;   // flip y

                Vector2 size   = box.size;
                Vector2 offset = box.offset;

                if (_draggingHandle < 4)
                {
                    // Corner: adjust both axes
                    Vector2 sign = BoxCornerSigns[_draggingHandle];
                    ApplyAxisDelta(ref size, ref offset, Vector2.right * sign.x, dx);
                    ApplyAxisDelta(ref size, ref offset, Vector2.up    * sign.y, dy);
                }
                else
                {
                    // Edge: adjust constrained axis
                    Vector2 sign = BoxEdgeSigns[_draggingHandle - 4];
                    if (Mathf.Abs(sign.x) > 0.5f)
                        ApplyAxisDelta(ref size, ref offset, Vector2.right * sign.x, dx);
                    else
                        ApplyAxisDelta(ref size, ref offset, Vector2.up    * sign.y, dy);
                }

                box.size   = size;
                box.offset = offset;
                EditorUtility.SetDirty(box);
                changed = true;
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                if (_wasDragging && _draggingHandle >= 0)
                {
                    AssetDatabase.SaveAssetIfDirty(box.gameObject);
                }

                _draggingHandle = -1;
                _wasDragging    = false;
            }

            return changed;
        }

        // ── Circle handles ─────────────────────────────────────────────────────

        private bool DrawCircleHandles(CircleCollider2D circle, Vector2 center, float scale)
        {
            bool changed = false;
            Color outline = circle.isTrigger ? OutlineTriggerColor : OutlineColor;

            Vector2 offsetS = circle.offset * scale;
            float   radiusS = circle.radius * scale;
            Vector2 c       = center + new Vector2(offsetS.x, -offsetS.y);

            // Draw wire circle approximation via polyline
            DrawWireCircle(c, radiusS, outline);

            // Two handles: right (radius) + top (radius)
            Vector2 rightHandle = c + Vector2.right * radiusS;
            Vector2 topHandle   = c + Vector2.up    * radiusS;

            Event e = Event.current;
            changed |= ProcessCircleDrag(circle, c, rightHandle, topHandle, scale, e);

            bool hoverR = Vector2.Distance(rightHandle, e.mousePosition) < HandleRadius;
            bool hoverT = Vector2.Distance(topHandle,   e.mousePosition) < HandleRadius;
            DrawHandleDot(rightHandle, (hoverR || _draggingHandle == 0) ? HandleHoverColor : HandleColor);
            DrawHandleDot(topHandle,   (hoverT || _draggingHandle == 1) ? HandleHoverColor : HandleColor);

            return changed;
        }

        private bool ProcessCircleDrag(CircleCollider2D circle, Vector2 circleCenterS,
                                       Vector2 rightHandle, Vector2 topHandle,
                                       float scale, Event e)
        {
            bool changed = false;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (Vector2.Distance(rightHandle, e.mousePosition) < HandleRadius)
                {
                    PushUndo(circle);
                    _draggingHandle = 0;
                    _wasDragging    = true;
                    e.Use();
                }
                else if (Vector2.Distance(topHandle, e.mousePosition) < HandleRadius)
                {
                    PushUndo(circle);
                    _draggingHandle = 1;
                    _wasDragging    = true;
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseDrag && _wasDragging && _draggingHandle >= 0)
            {
                // New radius = distance from center to dragged mouse
                float newRadiusS = Vector2.Distance(circleCenterS, e.mousePosition);
                circle.radius    = Mathf.Max(0.01f, newRadiusS / scale);
                EditorUtility.SetDirty(circle);
                changed = true;
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                if (_wasDragging && _draggingHandle >= 0)
                    AssetDatabase.SaveAssetIfDirty(circle.gameObject);

                _draggingHandle = -1;
                _wasDragging    = false;
            }

            return changed;
        }

        // ── Capsule handles ────────────────────────────────────────────────────

        private bool DrawCapsuleHandles(CapsuleCollider2D capsule, Vector2 center, float scale)
        {
            bool changed = false;
            Color outline = capsule.isTrigger ? OutlineTriggerColor : OutlineColor;

            Vector2 half    = capsule.size * 0.5f * scale;
            Vector2 offsetS = capsule.offset * scale;
            Vector2 c       = center + new Vector2(offsetS.x, -offsetS.y);

            // Draw bounding rect outline (approximation; capsule caps deferred)
            Rect outlineRect = new Rect(c.x - half.x, c.y - half.y, half.x * 2f, half.y * 2f);
            DrawRectOutline(outlineRect, outline);

            // Two handles: right edge + top edge
            Vector2 rightHandle = c + Vector2.right * half.x;
            Vector2 topHandle   = c - Vector2.up    * half.y;

            Event e = Event.current;
            changed |= ProcessCapsuleDrag(capsule, c, rightHandle, topHandle, scale, e);

            bool hoverR = Vector2.Distance(rightHandle, e.mousePosition) < HandleRadius;
            bool hoverT = Vector2.Distance(topHandle,   e.mousePosition) < HandleRadius;
            DrawHandleDot(rightHandle, (hoverR || _draggingHandle == 0) ? HandleHoverColor : HandleColor);
            DrawHandleDot(topHandle,   (hoverT || _draggingHandle == 1) ? HandleHoverColor : HandleColor);

            return changed;
        }

        private bool ProcessCapsuleDrag(CapsuleCollider2D capsule, Vector2 capsuleCenterS,
                                        Vector2 rightHandle, Vector2 topHandle,
                                        float scale, Event e)
        {
            bool changed = false;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (Vector2.Distance(rightHandle, e.mousePosition) < HandleRadius)
                {
                    PushUndo(capsule);
                    _draggingHandle = 0;
                    _wasDragging    = true;
                    e.Use();
                }
                else if (Vector2.Distance(topHandle, e.mousePosition) < HandleRadius)
                {
                    PushUndo(capsule);
                    _draggingHandle = 1;
                    _wasDragging    = true;
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseDrag && _wasDragging && _draggingHandle >= 0)
            {
                Vector2 size = capsule.size;

                if (_draggingHandle == 0)
                {
                    float halfX = Mathf.Max(0.01f,
                        Mathf.Abs(e.mousePosition.x - capsuleCenterS.x) / scale);
                    size.x = halfX * 2f;
                }
                else
                {
                    float halfY = Mathf.Max(0.01f,
                        Mathf.Abs(e.mousePosition.y - capsuleCenterS.y) / scale);
                    size.y = halfY * 2f;
                }

                capsule.size = size;
                EditorUtility.SetDirty(capsule);
                changed = true;
                e.Use();
            }
            else if (e.type == EventType.MouseUp)
            {
                if (_wasDragging && _draggingHandle >= 0)
                    AssetDatabase.SaveAssetIfDirty(capsule.gameObject);

                _draggingHandle = -1;
                _wasDragging    = false;
            }

            return changed;
        }

        // ── Undo ───────────────────────────────────────────────────────────────

        private void PushUndo(Collider2D col)
        {
            if (col == null) return;
            if (_undoStack.Count >= UndoDepth) return; // cap depth (oldest lost — acceptable)

            ColliderState state;
            state.isTrigger = col.isTrigger;
            state.offset    = col.offset;
            state.shape     = ResolveShape(col);

            if (col is BoxCollider2D b)        state.size = b.size;
            else if (col is CircleCollider2D c) state.size = new Vector2(c.radius, c.radius);
            else if (col is CapsuleCollider2D p) state.size = p.size;
            else state.size = Vector2.one;

            _undoStack.Push(state);
        }

        private bool HandleUndoKeyboard()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z &&
                (e.control || e.command) && _undoStack.Count > 0 &&
                _cachedEntry?.prefab != null)
            {
                ColliderState prev = _undoStack.Pop();
                ApplyColliderState(_cachedEntry.prefab, prev);
                e.Use();
                return true;
            }

            return false;
        }

        private static void ApplyColliderState(GameObject prefab, ColliderState state)
        {
            if (state.shape != ResolveShape(prefab.GetComponent<Collider2D>()))
            {
                ColliderShapeSwapper.SwapShape(prefab, state.shape);
            }

            Collider2D col = prefab.GetComponent<Collider2D>();
            if (col == null) return;

            col.isTrigger = state.isTrigger;
            col.offset    = state.offset;

            if (col is BoxCollider2D b)         b.size = state.size;
            else if (col is CircleCollider2D c)  c.radius = state.size.x;
            else if (col is CapsuleCollider2D p) p.size = state.size;

            EditorUtility.SetDirty(col);
            AssetDatabase.SaveAssetIfDirty(prefab);
        }

        // ── Info strip ─────────────────────────────────────────────────────────

        private void DrawInfoStrip(Rect strip)
        {
            if (_cachedCollider == null) return;

            string label;
            if (_cachedCollider is BoxCollider2D b)
                label = string.Format("Box  {0:0.00} x {1:0.00}  offset ({2:0.00}, {3:0.00})",
                    b.size.x, b.size.y, b.offset.x, b.offset.y);
            else if (_cachedCollider is CircleCollider2D c)
                label = string.Format("Circle  r={0:0.00}  offset ({1:0.00}, {2:0.00})",
                    c.radius, c.offset.x, c.offset.y);
            else if (_cachedCollider is CapsuleCollider2D p)
                label = string.Format("Capsule  {0:0.00} x {1:0.00}  offset ({2:0.00}, {3:0.00})",
                    p.size.x, p.size.y, p.offset.x, p.offset.y);
            else
                label = _cachedCollider.GetType().Name;

            GUIStyle mini = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal    = { textColor = new Color(0.65f, 0.65f, 0.65f, 1f) }
            };
            GUI.Label(strip, label + "  [Ctrl+Z undo]", mini);
        }

        // ── Draw helpers ───────────────────────────────────────────────────────

        private static void DrawNoSelectionLabel(Rect canvas)
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                wordWrap  = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize  = 10,
            };
            GUI.Label(canvas, "Select a prefab asset in the palette\nto edit its collider handles.", style);
        }

        private static void DrawHelpLabel(Rect canvas, string msg)
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                wordWrap  = true,
                alignment = TextAnchor.MiddleCenter,
            };
            GUI.Label(canvas, msg, style);
        }

        private static void DrawRectOutline(Rect r, Color color)
        {
            float t = 1.5f;
            EditorGUI.DrawRect(new Rect(r.x,        r.y,        r.width, t),       color);
            EditorGUI.DrawRect(new Rect(r.x,        r.yMax - t, r.width, t),       color);
            EditorGUI.DrawRect(new Rect(r.x,        r.y,        t,       r.height), color);
            EditorGUI.DrawRect(new Rect(r.xMax - t, r.y,        t,       r.height), color);
        }

        private static void DrawBorder(Rect r, Color color)
        {
            DrawRectOutline(r, color);
        }

        private static void DrawHandleDot(Vector2 pos, Color color)
        {
            float d = HandleDotSize;
            Rect dot = new Rect(pos.x - d, pos.y - d, d * 2f, d * 2f);
            EditorGUI.DrawRect(dot, color);
        }

        private static void DrawWireCircle(Vector2 center, float radius, Color color)
        {
            // 32-segment polyline approximation
            const int segments = 32;
            Vector3[] pts = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                pts[i] = new Vector3(
                    center.x + Mathf.Cos(angle) * radius,
                    center.y + Mathf.Sin(angle) * radius, 0f);
            }

            Handles.BeginGUI();
            Color prev = Handles.color;
            Handles.color = color;
            Handles.DrawAAPolyLine(1.5f, pts);
            Handles.color = prev;
            Handles.EndGUI();
        }

        // ── Math helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Pixels per Unity unit that fits the collider into <paramref name="inner"/> with 20% margin.
        /// </summary>
        private float ComputeUnitScale(Rect inner)
        {
            if (_cachedCollider == null) return 60f;

            float wu = 1f, hu = 1f;

            if (_cachedCollider is BoxCollider2D b)
            {
                wu = Mathf.Max(0.01f, b.size.x);
                hu = Mathf.Max(0.01f, b.size.y);
            }
            else if (_cachedCollider is CircleCollider2D c)
            {
                wu = hu = Mathf.Max(0.01f, c.radius * 2f);
            }
            else if (_cachedCollider is CapsuleCollider2D p)
            {
                wu = Mathf.Max(0.01f, p.size.x);
                hu = Mathf.Max(0.01f, p.size.y);
            }

            float scaleX = inner.width  * 0.8f / wu;
            float scaleY = inner.height * 0.8f / hu;
            return Mathf.Min(scaleX, scaleY);
        }

        /// <summary>
        /// Apply a drag delta along one signed axis (size + offset update).
        /// </summary>
        private static void ApplyAxisDelta(ref Vector2 size, ref Vector2 offset,
                                           Vector2 signedAxis, float delta)
        {
            // signedAxis.x or .y is ±1 for the active axis
            if (Mathf.Abs(signedAxis.x) > 0.5f)
            {
                float opp = offset.x - signedAxis.x * (size.x * 0.5f);
                float drag = offset.x + signedAxis.x * (size.x * 0.5f) + signedAxis.x * delta;
                offset.x = (opp + drag) * 0.5f;
                size.x   = Mathf.Max(0.01f, Mathf.Abs(drag - opp));
            }
            else
            {
                float opp = offset.y - signedAxis.y * (size.y * 0.5f);
                float drag = offset.y + signedAxis.y * (size.y * 0.5f) + signedAxis.y * delta;
                offset.y = (opp + drag) * 0.5f;
                size.y   = Mathf.Max(0.01f, Mathf.Abs(drag - opp));
            }
        }

        private static Rect Inflate(Rect r, float amount)
        {
            return new Rect(r.x - amount, r.y - amount,
                            r.width + amount * 2f, r.height + amount * 2f);
        }

        private static ColliderShape ResolveShape(Collider2D col)
        {
            if (col is CircleCollider2D)  return ColliderShape.Circle;
            if (col is CapsuleCollider2D) return ColliderShape.Capsule;
            return ColliderShape.Box;
        }
    }
}
