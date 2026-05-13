namespace RIMA.Tests.Editor
{
    using System.Reflection;
    using NUnit.Framework;
    using RIMA.Editor.RoomDesigner;
    using RIMA.Editor.RoomDesigner.Brushes;
    using RIMA.RoomDesigner.Core;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;

    public sealed class RoomDesignerIntegrationTests
    {
        [Test]
        public void StampStrokeAppliesAndUndoClearsTile()
        {
            var window = EditorWindow.GetWindow<RimaRoomDesignerWindow>();
            var testTile = ScriptableObject.CreateInstance<Tile>();
            testTile.name = "IntegrationTestTile";

            try
            {
                window.Show();
                window.CreateGUI();

                BrushController controller = GetBrushController(window);
                Assert.IsNotNull(controller);
                Assert.AreSame(controller, BrushController.Instance);
                Assert.IsNotNull(window.RightPanel.Q<VisualElement>(className: "rd-brush-toolbar"));

                window.ActiveLayer = RoomLayer.Base;
                window.ActiveTile = testTile;

                window.InvokeBrush(0, Vector3Int.zero);
                window.OnBrushRelease();

                Tilemap activeTilemap = window.GetActiveTilemap();
                Assert.AreSame(testTile, activeTilemap.GetTile(Vector3Int.zero));

                Undo.FlushUndoRecordObjects();
                Undo.PerformUndo();

                Assert.IsNull(activeTilemap.GetTile(Vector3Int.zero));
            }
            finally
            {
                window.ActiveTile = null;
                window.Close();
                Object.DestroyImmediate(testTile);
            }
        }

        private static BrushController GetBrushController(RimaRoomDesignerWindow window)
        {
            FieldInfo field = typeof(RimaRoomDesignerWindow).GetField(
                "brushController",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field);
            return (BrushController)field.GetValue(window);
        }
    }
}
