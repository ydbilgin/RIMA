#if UNITY_EDITOR
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Editor.UI;

namespace RIMA.MapDesigner.Brush.Tests
{
    public class BrushWindowTests
    {
        private MapDesignerBrushWindow window;

        [SetUp]
        public void SetUp()
        {
            window = EditorWindow.GetWindow<MapDesignerBrushWindow>(false, "TestBrushWindow", false);
        }

        [TearDown]
        public void TearDown()
        {
            if (window != null)
            {
                window.Close();
            }
        }

        [Test]
        public void WindowOpens_NoExceptions()
        {
            Assert.IsNotNull(window);
            Assert.AreEqual(BrushToolMode.Brush, window.ToolMode);
        }

        [Test]
        public void SetMode_ChangesToolMode()
        {
            window.SetMode(BrushToolMode.Erase);
            Assert.AreEqual(BrushToolMode.Erase, window.ToolMode);
            window.SetMode(BrushToolMode.SmartFill);
            Assert.AreEqual(BrushToolMode.SmartFill, window.ToolMode);
        }

        [Test]
        public void AdjustBrushSize_ChangesSize()
        {
            float before = window.BrushSize;
            window.AdjustBrushSize(8f);
            Assert.AreEqual(before + 8f, window.BrushSize, 0.001f);
            window.AdjustBrushSize(-8f);
            Assert.AreEqual(before, window.BrushSize, 0.001f);
        }

        [Test]
        public void AdjustBrushSize_Clamped()
        {
            window.AdjustBrushSize(-10000f);
            Assert.GreaterOrEqual(window.BrushSize, 8f);
            window.AdjustBrushSize(+10000f);
            Assert.LessOrEqual(window.BrushSize, 512f);
        }

        [Test]
        public void SetBrush_UpdatesSelection()
        {
            var brush = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brush.brushName = "TestBrush";
            brush.category = BrushCategory.Floor;
            window.SetBrush(brush);
            Assert.AreEqual(brush, window.SelectedBrush);
            Object.DestroyImmediate(brush);
        }

        [Test]
        public void SelectBrushBySlot_FindsByHotkeyIndex()
        {
            var pack = ScriptableObject.CreateInstance<BrushPackSO>();
            var brushA = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brushA.brushName = "A";
            brushA.hotkeyIndex = 1;
            var brushB = ScriptableObject.CreateInstance<MapDesignerBrushPresetSO>();
            brushB.brushName = "B";
            brushB.hotkeyIndex = 3;
            pack.brushes.Add(brushA);
            pack.brushes.Add(brushB);

            var field = typeof(MapDesignerBrushWindow).GetField("activePack",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(window, pack);

            window.SelectBrushBySlot(3);
            Assert.AreEqual(brushB, window.SelectedBrush);
            window.SelectBrushBySlot(1);
            Assert.AreEqual(brushA, window.SelectedBrush);

            Object.DestroyImmediate(brushA);
            Object.DestroyImmediate(brushB);
            Object.DestroyImmediate(pack);
        }

        [Test]
        public void GetCurrent_ReturnsActiveWindow()
        {
            window.Focus();
            var current = MapDesignerBrushWindow.GetCurrent();
            Assert.IsNotNull(current);
        }
    }
}
#endif
