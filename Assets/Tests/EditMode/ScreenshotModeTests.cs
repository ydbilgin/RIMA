#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.DebugTools;
using UnityEngine;

namespace RIMA.Tests
{
    public sealed class ScreenshotModeTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            ScreenshotMode.SetEnabled(false);
            for (int i = created.Count - 1; i >= 0; i--)
            {
                if (created[i] != null)
                {
                    Object.DestroyImmediate(created[i]);
                }
            }
            created.Clear();
        }

        [Test]
        public void Registry_ToggleHidesAndRestoresOriginalActiveState()
        {
            GameObject surface = new GameObject("DebugSurface");
            created.Add(surface);

            ScreenshotMode.Register(surface, "test");
            ScreenshotMode.SetEnabled(true);
            Assert.IsFalse(surface.activeSelf);

            ScreenshotMode.SetEnabled(false);
            Assert.IsTrue(surface.activeSelf);
        }
    }
}
#endif
