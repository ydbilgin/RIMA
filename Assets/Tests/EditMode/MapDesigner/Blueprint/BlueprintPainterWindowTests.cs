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
        public void LayerVisibility_AllOn_NoLayersDimmed()
        {
            BlueprintPainterWindow window = CreateWindow();

            Assert.AreEqual(1f, window.GetLayerAlphaForTesting(1));
            Assert.AreEqual(1f, window.GetLayerAlphaForTesting(8));
        }

        [Test]
        public void LayerVisibility_L3Off_OnlyL3Dimmed()
        {
            BlueprintPainterWindow window = CreateWindow();

            window.SetLayerVisibleForTesting(3, false);

            Assert.AreEqual(0.2f, window.GetLayerAlphaForTesting(3));
            Assert.AreEqual(1f, window.GetLayerAlphaForTesting(4));
        }

        [Test]
        public void LayerVisibility_L1Off_DimsMacroLayer()
        {
            BlueprintPainterWindow window = CreateWindow();
            SpriteRenderer renderer = CreatePlacedLayerRenderer(1);
            window.SetActiveRoomRootForTesting(renderer.transform.parent);

            window.SetLayerVisibleForTesting(1, false);

            Assert.AreEqual(0.2f, renderer.color.a);
        }

        [Test]
        public void LayerVisibility_L8Off_DimsAtmospheric()
        {
            BlueprintPainterWindow window = CreateWindow();
            SpriteRenderer renderer = CreatePlacedLayerRenderer(8);
            window.SetActiveRoomRootForTesting(renderer.transform.parent);

            window.SetLayerVisibleForTesting(8, false);

            Assert.AreEqual(0.2f, renderer.color.a);
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

        private SpriteRenderer CreatePlacedLayerRenderer(int layer)
        {
            Transform root = new GameObject("LayerVisibilityRoot").transform;
            createdObjects.Add(root.gameObject);

            GameObject placed = new GameObject($"_BlueprintPlaced_L{layer}_path_0_0");
            placed.transform.SetParent(root);
            return placed.AddComponent<SpriteRenderer>();
        }
    }
}
