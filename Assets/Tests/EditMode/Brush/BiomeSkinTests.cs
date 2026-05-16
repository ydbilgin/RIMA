#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Render.Editor;

namespace RIMA.Tests.Brush
{
    public sealed class BiomeSkinTests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null) Object.DestroyImmediate(cleanup[i]);
            }
            cleanup.Clear();
        }

        [Test]
        public void ApplySkin_NullSkin_ReturnsZero()
        {
            int touched = BiomeSkinApplier.Apply(null);
            Assert.AreEqual(0, touched);
        }

        [Test]
        public void ApplySkin_EmptyRules_ReturnsZero()
        {
            var skin = ScriptableObject.CreateInstance<BiomeSkinSO>();
            cleanup.Add(skin);
            skin.layerRenderRules = new List<LayerRenderRule>();
            int touched = BiomeSkinApplier.Apply(skin);
            Assert.AreEqual(0, touched);
        }

        [Test]
        public void ApplySkin_TouchesSpriteRenderers()
        {
            var container = new GameObject("Layer_" + TargetLayer.L4);
            cleanup.Add(container);
            var child = new GameObject("Decal");
            child.transform.SetParent(container.transform);
            var sr = child.AddComponent<SpriteRenderer>();
            cleanup.Add(child);

            var skin = ScriptableObject.CreateInstance<BiomeSkinSO>();
            cleanup.Add(skin);
            skin.layerRenderRules = new List<LayerRenderRule>
            {
                new LayerRenderRule
                {
                    layer = TargetLayer.L4,
                    alphaMode = AlphaMode.Hard,
                    tint = new Color(0.5f, 0.5f, 0.5f, 1f),
                    sortingOrder = 200
                }
            };
            skin.globalTint = Color.white;

            int touched = BiomeSkinApplier.Apply(skin);
            Assert.GreaterOrEqual(touched, 1);
            Assert.AreEqual(200, sr.sortingOrder);
            Assert.AreEqual(0.5f, sr.color.r, 0.001f);
        }

        [Test]
        public void ApplySkin_RemembersLastApplied()
        {
            var skin = ScriptableObject.CreateInstance<BiomeSkinSO>();
            cleanup.Add(skin);
            skin.skinName = "TestSkin";
            skin.layerRenderRules = new List<LayerRenderRule>();
            BiomeSkinApplier.Apply(skin);
            Assert.AreEqual(skin, BiomeSkinApplier.LastApplied);
        }

        [Test]
        public void ApplySkin_LoadsAllFourDefaultSkins()
        {
            string[] paths = new[]
            {
                "Assets/Data/Brush/Default/BiomeSkin_HadesNet.asset",
                "Assets/Data/Brush/Default/BiomeSkin_GrimdarkMix.asset",
                "Assets/Data/Brush/Default/BiomeSkin_SoftPainter.asset",
                "Assets/Data/Brush/Default/BiomeSkin_BoldGraphic.asset"
            };
            foreach (var path in paths)
            {
                var skin = AssetDatabase.LoadAssetAtPath<BiomeSkinSO>(path);
                Assert.IsNotNull(skin, "Failed to load " + path);
                Assert.IsNotEmpty(skin.skinName);
                Assert.AreEqual(6, skin.layerRenderRules.Count, path + " should have 6 layer rules");
            }
        }
    }
}
#endif
