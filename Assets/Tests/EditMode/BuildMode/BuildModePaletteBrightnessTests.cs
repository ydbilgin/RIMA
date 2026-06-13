#if UNITY_EDITOR
using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace RIMA.Tests.BuildMode
{
    /// <summary>
    /// Cheap regression guard against re-darkening the Build Mode palette (consolidation item 6 —
    /// user: "too dark"). BuildModeUiStyle is internal, so the tokens are read by reflection; the
    /// test only asserts a luminance FLOOR (it does not pin exact hex), so future tuning is free as
    /// long as the panel/button surfaces stay legible.
    /// </summary>
    public class BuildModePaletteBrightnessTests
    {
        private static Type StyleType()
        {
            // Same assembly as the controllers (RIMA.Runtime); resolve by full name.
            Type t = Type.GetType("RIMA.UI.BuildMode.BuildModeUiStyle, RIMA.Runtime");
            if (t != null) return t;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = asm.GetType("RIMA.UI.BuildMode.BuildModeUiStyle");
                if (t != null) return t;
            }
            return null;
        }

        private static Color ReadColor(Type t, string field)
        {
            FieldInfo fi = t.GetField(field, BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(fi, $"BuildModeUiStyle.{field} must exist.");
            return (Color)fi.GetValue(null);
        }

        // Rec.601-ish perceived luminance, ignoring alpha.
        private static float Luminance(Color c) => 0.299f * c.r + 0.587f * c.g + 0.114f * c.b;

        [Test]
        public void StyleType_Resolves()
        {
            Assert.IsNotNull(StyleType(), "BuildModeUiStyle type must be resolvable in the runtime assembly.");
        }

        [Test]
        public void PanelBg_AboveDarknessFloor()
        {
            Type t = StyleType();
            // PanelBg was 0x16181C (lum ~0.094); brightened to 0x202329 (lum ~0.13). Floor a touch
            // under the new value so the test guards the lift without pinning the exact hex.
            Assert.GreaterOrEqual(Luminance(ReadColor(t, "PanelBg")), 0.11f,
                "PanelBg must stay brighter than the old near-black base.");
        }

        [Test]
        public void ButtonIdle_AboveDarknessFloor()
        {
            Type t = StyleType();
            // ButtonIdle was 0x2A2D32 (lum ~0.17); brightened to 0x363A41 (lum ~0.22).
            Assert.GreaterOrEqual(Luminance(ReadColor(t, "ButtonIdle")), 0.19f,
                "Idle buttons must read as raised, not flat black.");
        }

        [Test]
        public void Borders_AreVisibleAgainstPanel()
        {
            Type t = StyleType();
            Color panel = ReadColor(t, "PanelBg");
            Color border = ReadColor(t, "PanelBorder");
            Assert.Greater(Luminance(border), Luminance(panel),
                "PanelBorder must be lighter than PanelBg so the hairline edge is visible.");
        }

        [Test]
        public void Ember_Unchanged()
        {
            Type t = StyleType();
            Color ember = ReadColor(t, "Ember");
            // 0xE89020 — must NOT have been touched by the brightness pass (selected-state contrast).
            Assert.AreEqual(0xE8 / 255f, ember.r, 0.01f);
            Assert.AreEqual(0x90 / 255f, ember.g, 0.01f);
            Assert.AreEqual(0x20 / 255f, ember.b, 0.01f);
        }
    }
}
#endif
