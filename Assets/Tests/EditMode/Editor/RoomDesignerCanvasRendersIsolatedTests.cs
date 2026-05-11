namespace RIMA.Tests.Editor
{
    using System.Reflection;
    using NUnit.Framework;
    using RIMA.Editor.RoomDesigner;
    using UnityEditor;
    using UnityEngine;

    public sealed class RoomDesignerCanvasRendersIsolatedTests
    {
        [Test]
        public void PreviewCameraRendersOnlyRoomDesignerLayer()
        {
            var window = EditorWindow.GetWindow<RimaRoomDesignerWindow>();

            try
            {
                window.Show();
                window.CreateGUI();

                int roomDesignerLayer = LayerMask.NameToLayer("RoomDesigner");
                Assert.GreaterOrEqual(roomDesignerLayer, 0);

                RoomDesignerCanvas canvas = GetCanvas(window);
                Assert.IsNotNull(canvas);
                Assert.IsNotNull(canvas.StageRoot);

                Assert.AreEqual(roomDesignerLayer, canvas.StageRoot.layer);
                AssertChildrenUseLayer(canvas.StageRoot.transform, roomDesignerLayer);

                Camera previewCamera = canvas.StageRoot.GetComponentInChildren<Camera>(true);
                Assert.IsNotNull(previewCamera);
                Assert.AreEqual(1 << roomDesignerLayer, previewCamera.cullingMask);
                Assert.IsFalse((previewCamera.cullingMask & (1 << 0)) != 0);
            }
            finally
            {
                window.Close();
            }
        }

        private static RoomDesignerCanvas GetCanvas(RimaRoomDesignerWindow window)
        {
            FieldInfo field = typeof(RimaRoomDesignerWindow).GetField(
                "canvas",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field);
            return (RoomDesignerCanvas)field.GetValue(window);
        }

        private static void AssertChildrenUseLayer(Transform root, int expectedLayer)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                Assert.AreEqual(expectedLayer, child.gameObject.layer, child.name);
                AssertChildrenUseLayer(child, expectedLayer);
            }
        }
    }
}
