#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Editor.UI.Hotkeys
{
    public static class BrushHotkeyHandler
    {
        [Shortcut("RIMA/Brush/Mode/Brush", KeyCode.B)]
        public static void ToBrushMode()
        {
            var w = MapDesignerBrushWindow.GetCurrent();
            if (w != null) w.SetMode(BrushToolMode.Brush);
        }

        [Shortcut("RIMA/Brush/Mode/Erase", KeyCode.E)]
        public static void ToEraseMode()
        {
            var w = MapDesignerBrushWindow.GetCurrent();
            if (w != null) w.SetMode(BrushToolMode.Erase);
        }

        [Shortcut("RIMA/Brush/Size/Decrease", KeyCode.LeftBracket)]
        public static void DecreaseSize()
        {
            var w = MapDesignerBrushWindow.GetCurrent();
            if (w != null) w.AdjustBrushSize(-4f);
        }

        [Shortcut("RIMA/Brush/Size/Increase", KeyCode.RightBracket)]
        public static void IncreaseSize()
        {
            var w = MapDesignerBrushWindow.GetCurrent();
            if (w != null) w.AdjustBrushSize(+4f);
        }

        [Shortcut("RIMA/Brush/Slot/1", KeyCode.Alpha1, ShortcutModifiers.Alt)]
        public static void Slot1() { Select(1); }
        [Shortcut("RIMA/Brush/Slot/2", KeyCode.Alpha2, ShortcutModifiers.Alt)]
        public static void Slot2() { Select(2); }
        [Shortcut("RIMA/Brush/Slot/3", KeyCode.Alpha3, ShortcutModifiers.Alt)]
        public static void Slot3() { Select(3); }
        [Shortcut("RIMA/Brush/Slot/4", KeyCode.Alpha4, ShortcutModifiers.Alt)]
        public static void Slot4() { Select(4); }
        [Shortcut("RIMA/Brush/Slot/5", KeyCode.Alpha5, ShortcutModifiers.Alt)]
        public static void Slot5() { Select(5); }
        [Shortcut("RIMA/Brush/Slot/6", KeyCode.Alpha6, ShortcutModifiers.Alt)]
        public static void Slot6() { Select(6); }
        [Shortcut("RIMA/Brush/Slot/7", KeyCode.Alpha7, ShortcutModifiers.Alt)]
        public static void Slot7() { Select(7); }
        [Shortcut("RIMA/Brush/Slot/8", KeyCode.Alpha8, ShortcutModifiers.Alt)]
        public static void Slot8() { Select(8); }
        [Shortcut("RIMA/Brush/Slot/9", KeyCode.Alpha9, ShortcutModifiers.Alt)]
        public static void Slot9() { Select(9); }

        private static void Select(int slot)
        {
            var w = MapDesignerBrushWindow.GetCurrent();
            if (w != null) w.SelectBrushBySlot(slot);
        }
    }
}
#endif
