using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.Background;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RIMA.Tests.Background
{
    public sealed class PersistentBackgroundControllerTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
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
        public void BuildIfEnabled_Disabled_DoesNotCreateChildren()
        {
            PersistentBackgroundController controller = CreateController();

            controller.BuildIfEnabled();

            Assert.AreEqual(0, controller.transform.childCount);
        }

        [Test]
        public void BuildIfEnabled_WithSingleFrameLayers_CreatesParallaxLayersOnce()
        {
            PersistentBackgroundController controller = CreateController();
            Sprite sprite = CreateSprite();
            SetPrivate(controller, "enablePersistentBackground", true);
            SetPrivate(controller, "far", CreateLayer(new Vector2(0.02f, 0.015f), -200, sprite));
            SetPrivate(controller, "mid", CreateLayer(new Vector2(0.05f, 0.04f), -150, sprite));
            SetPrivate(controller, "front", CreateLayer(new Vector2(0.10f, 0.06f), -100, sprite));

            controller.BuildIfEnabled();
            controller.BuildIfEnabled();

            Transform root = controller.transform.Find("PersistentBackground");
            Assert.IsNotNull(root);
            Assert.AreEqual(3, root.childCount);
            AssertLayer(root.GetChild(0), new Vector2(0.02f, 0.015f), -200);
            AssertLayer(root.GetChild(1), new Vector2(0.05f, 0.04f), -150);
            AssertLayer(root.GetChild(2), new Vector2(0.10f, 0.06f), -100);
            Assert.AreEqual(3, root.GetComponentsInChildren<SpriteRenderer>().Length);
        }

        [Test]
        public void DefaultSortingOrders_StayInBackgroundReservedRange()
        {
            PersistentBackgroundController controller = CreateController();

            AssertSortingInRange(GetPrivate<PersistentBackgroundController.BgLayerDef>(controller, "far"));
            AssertSortingInRange(GetPrivate<PersistentBackgroundController.BgLayerDef>(controller, "mid"));
            AssertSortingInRange(GetPrivate<PersistentBackgroundController.BgLayerDef>(controller, "front"));
        }

        private PersistentBackgroundController CreateController()
        {
            GameObject go = new GameObject("PersistentBackgroundController_Test");
            created.Add(go);
            return go.AddComponent<PersistentBackgroundController>();
        }

        private Sprite CreateSprite()
        {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            created.Add(texture);

            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 64f);
            sprite.hideFlags = HideFlags.HideAndDontSave;
            created.Add(sprite);
            return sprite;
        }

        private static PersistentBackgroundController.BgLayerDef CreateLayer(Vector2 factor, int sortingOrder, Sprite sprite)
        {
            return new PersistentBackgroundController.BgLayerDef(sortingOrder, factor)
            {
                frames = new[] { sprite }
            };
        }

        private static void AssertLayer(Transform layer, Vector2 expectedFactor, int expectedSortingOrder)
        {
            SpriteRenderer renderer = layer.GetComponent<SpriteRenderer>();
            Assert.IsNotNull(renderer);
            Assert.AreEqual(expectedSortingOrder, renderer.sortingOrder);

            ParallaxLayer parallax = layer.GetComponent<ParallaxLayer>();
            Assert.IsNotNull(parallax);
            Assert.AreEqual(expectedFactor, parallax.factor);
            Assert.IsTrue(parallax.snapToPixel);
            Assert.AreEqual(64, parallax.pixelsPerUnit);
        }

        private static void AssertSortingInRange(PersistentBackgroundController.BgLayerDef layer)
        {
            Assert.GreaterOrEqual(layer.sortingOrder, -200);
            Assert.LessOrEqual(layer.sortingOrder, -50);
        }

        private static T GetPrivate<T>(object target, string fieldName)
        {
            return (T)target.GetType()
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(target);
        }

        private static void SetPrivate(object target, string fieldName, object value)
        {
            target.GetType()
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(target, value);
        }
    }
}
