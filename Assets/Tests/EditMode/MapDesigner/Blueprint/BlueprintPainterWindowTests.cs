using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class BlueprintPainterWindowTests
    {
        private readonly List<Object> createdObjects = new List<Object>();

        [SetUp]
        public void SetUp()
        {
            EditorPrefs.DeleteKey(BlueprintPainterWindow.ActiveRoomRootPrefsKey);
        }

        [TearDown]
        public void TearDown()
        {
            EditorPrefs.DeleteKey(BlueprintPainterWindow.ActiveRoomRootPrefsKey);
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                if (createdObjects[i] != null)
                {
                    Object.DestroyImmediate(createdObjects[i]);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void RandomSeed_ProducesDifferentSeedValue()
        {
            BlueprintPainterWindow window = CreateWindow();
            window.SeedForTesting = 1337;

            window.RandomizeSeedForTesting();

            Assert.AreNotEqual(1337, window.SeedForTesting);
        }

        [Test]
        public void LayerVisibility_AllOn_NoCellsDimmed()
        {
            BlueprintPainterWindow window = CreateWindow();

            Assert.AreEqual(1f, window.GetZonePaintAlphaForTesting("path"));
        }

        [Test]
        public void LayerVisibility_PathOff_PathCellsDimmed()
        {
            BlueprintPainterWindow window = CreateWindow();

            window.SetLayerVisibleForTesting("path", false);

            Assert.AreEqual(0.2f, window.GetZonePaintAlphaForTesting("path"));
            Assert.AreEqual(1f, window.GetZonePaintAlphaForTesting("grass"));
        }

        [Test]
        public void ActiveRoomRoot_PersistsAcrossWindowReopen()
        {
            Transform root = new GameObject("PersistedRoot").transform;
            createdObjects.Add(root.gameObject);
            BlueprintPainterWindow firstWindow = CreateWindow();

            firstWindow.SetActiveRoomRootForTesting(root);
            BlueprintPainterWindow secondWindow = CreateWindow();
            secondWindow.InvokeOnEnableForTesting();

            Assert.AreEqual(root.GetInstanceID(), EditorPrefs.GetInt(BlueprintPainterWindow.ActiveRoomRootPrefsKey, 0));
            Assert.AreSame(root, secondWindow.ActiveRoomRootForTesting);
        }

        [Test]
        public void ActiveRoomRoot_HandlesDestroyedTransformGracefully()
        {
            GameObject rootObject = new GameObject("DestroyedRoot");
            int instanceId = rootObject.transform.GetInstanceID();
            EditorPrefs.SetInt(BlueprintPainterWindow.ActiveRoomRootPrefsKey, instanceId);
            Object.DestroyImmediate(rootObject);
            BlueprintPainterWindow window = CreateWindow();

            window.InvokeOnEnableForTesting();

            Assert.IsFalse(EditorPrefs.HasKey(BlueprintPainterWindow.ActiveRoomRootPrefsKey));
            Assert.IsNull(window.ActiveRoomRootForTesting);
            StringAssert.Contains("Previous Active Room Root no longer exists", window.StatusTextForTesting);
        }

        private BlueprintPainterWindow CreateWindow()
        {
            BlueprintPainterWindow window = ScriptableObject.CreateInstance<BlueprintPainterWindow>();
            createdObjects.Add(window);
            return window;
        }
    }
}
