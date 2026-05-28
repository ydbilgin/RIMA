#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using LaurethStudio.PainterSuite.Editor.Colliders;
using LaurethStudio.PainterSuite.Editor.Core;

namespace LaurethStudio.PainterSuite.Editor.Hotkeys
{
    /// <summary>
    /// Context-aware shortcuts for PainterSuiteWindow.
    /// All bindings use Shift+ modifier to avoid collision with RIMA's Brush shortcuts
    /// (B/E/[/]) and Unity defaults (Q/W/E/R/T gizmo modes).
    /// Context filter (typeof(PainterSuiteWindow)) ensures shortcuts only fire when
    /// painter window is focused -- no global key hijack.
    /// </summary>
    /// <remarks>
    /// User can rebind any of these via Unity Edit > Shortcuts > LaurethStudio category.
    /// </remarks>
    public static class PainterShortcuts
    {
        [Shortcut("LaurethStudio/Painter/Mode/Box", typeof(PainterSuiteWindow), KeyCode.B, ShortcutModifiers.Shift)]
        private static void SetBoxMode(ShortcutArguments args)
        {
            var w = args.context as PainterSuiteWindow;
            if (w != null) w.SetShapeMode(ShapeMode.Box);
        }

        [Shortcut("LaurethStudio/Painter/Mode/Circle", typeof(PainterSuiteWindow), KeyCode.C, ShortcutModifiers.Shift)]
        private static void SetCircleMode(ShortcutArguments args)
        {
            var w = args.context as PainterSuiteWindow;
            if (w != null) w.SetShapeMode(ShapeMode.Circle);
        }

        [Shortcut("LaurethStudio/Painter/Mode/Polygon", typeof(PainterSuiteWindow), KeyCode.P, ShortcutModifiers.Shift)]
        private static void SetPolygonMode(ShortcutArguments args)
        {
            var w = args.context as PainterSuiteWindow;
            if (w != null) w.SetShapeMode(ShapeMode.Polygon);
        }

        [Shortcut("LaurethStudio/Painter/Mode/Edge", typeof(PainterSuiteWindow), KeyCode.E, ShortcutModifiers.Shift)]
        private static void SetEdgeMode(ShortcutArguments args)
        {
            var w = args.context as PainterSuiteWindow;
            if (w != null) w.SetShapeMode(ShapeMode.Edge);
        }

        [Shortcut("LaurethStudio/Painter/Cancel In-Progress", typeof(PainterSuiteWindow), KeyCode.Escape)]
        private static void CancelInProgress(ShortcutArguments args)
        {
            var w = args.context as PainterSuiteWindow;
            if (w != null) w.CancelInProgressShape();
        }

        [Shortcut("LaurethStudio/Painter/Delete Selected Collider", typeof(PainterSuiteWindow), KeyCode.Delete)]
        private static void DeleteSelectedCollider(ShortcutArguments args)
        {
            var w = args.context as PainterSuiteWindow;
            if (w != null) w.DeleteSelectedCollider();
        }
    }
}
#endif
