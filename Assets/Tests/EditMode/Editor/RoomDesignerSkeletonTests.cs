namespace RIMA.Tests.Editor
{
    using System.IO;
    using NUnit.Framework;
    using RIMA.Editor.RoomDesigner;
    using UnityEditor;
    using UnityEngine;

    public sealed class RoomDesignerSkeletonTests
    {
        [Test]
        public void WindowCanBeOpened()
        {
            var window = EditorWindow.GetWindow<RimaRoomDesignerWindow>();
            Assert.IsNotNull(window);
            window.Close();
        }

        [Test]
        public void ContextDefaultsAreValid()
        {
            var window = EditorWindow.GetWindow<RimaRoomDesignerWindow>();
            try
            {
                Assert.AreEqual(BrushMode.Stamp, window.ActiveBrush);
                Assert.AreEqual(RoomLayer.Floor, window.ActiveLayer);
                Assert.AreEqual(Vector3Int.zero, window.HoveredCell);
            }
            finally
            {
                window.Close();
            }
        }

        [Test]
        public void RoomSaverSaveCreatesPrefabAndBlueprint()
        {
            const string biome = "_TEST";
            const string roomId = "test_room";
            const string dir = "Assets/_Generated/Rooms/_TEST";
            const string prefabPath = "Assets/_Generated/Rooms/_TEST/test_room.prefab";
            const string blueprintPath = "Assets/_Generated/Rooms/_TEST/test_room.asset";

            var roomRoot = new GameObject("TempRoomRoot");
            try
            {
                var saved = RoomSaver.Save(roomRoot, roomId, biome);

                Assert.AreEqual(prefabPath, saved.prefabPath);
                Assert.AreEqual(blueprintPath, saved.blueprintPath);
                Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
                Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<RoomBlueprint>(blueprintPath));
            }
            finally
            {
                Object.DestroyImmediate(roomRoot);
                AssetDatabase.DeleteAsset(prefabPath);
                AssetDatabase.DeleteAsset(blueprintPath);
                AssetDatabase.DeleteAsset(dir);

                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }

                AssetDatabase.Refresh();
            }
        }

        [Test]
        public void RoomSaverSaveRollsBackBlueprintWhenPrefabSaveFails()
        {
            const string biome = "_TEST_ROLLBACK";
            const string roomId = "bad_room";
            const string dir = "Assets/_Generated/Rooms/_TEST_ROLLBACK";
            const string prefabPath = "Assets/_Generated/Rooms/_TEST_ROLLBACK/bad_room.prefab";
            const string blueprintPath = "Assets/_Generated/Rooms/_TEST_ROLLBACK/bad_room.asset";

            var roomRoot = new GameObject("TempRollbackRoomRoot");
            try
            {
                Directory.CreateDirectory(prefabPath);
                AssetDatabase.Refresh();

                Assert.Catch<System.Exception>(() => RoomSaver.Save(roomRoot, roomId, biome));
                Assert.IsNull(AssetDatabase.LoadAssetAtPath<RoomBlueprint>(blueprintPath));
                Assert.IsFalse(File.Exists(blueprintPath));
            }
            finally
            {
                Object.DestroyImmediate(roomRoot);
                AssetDatabase.DeleteAsset(prefabPath);
                AssetDatabase.DeleteAsset(blueprintPath);
                AssetDatabase.DeleteAsset(dir);

                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }

                AssetDatabase.Refresh();
            }
        }
    }
}
