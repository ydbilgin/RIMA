using NUnit.Framework;
using RIMA.MapDesigner.Brush.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Tests
{
    public class BrushRadiusProfileTests
    {
        [Test]
        public void Radius1_AllMicro()
        {
            var p = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            p.PopulateDefaultSoftOverlap();
            var weights = p.ResolveWeights(1);
            Assert.That(weights.Count, Is.EqualTo(1));
            Assert.That(weights.ContainsKey(SizeBucket.Micro), Is.True);
            Assert.That(weights[SizeBucket.Micro], Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void Radius4_SmallMedium_5050()
        {
            var p = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            p.PopulateDefaultSoftOverlap();
            var weights = p.ResolveWeights(4);
            Assert.That(weights.Count, Is.EqualTo(2));
            Assert.That(weights[SizeBucket.Small], Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(weights[SizeBucket.Medium], Is.EqualTo(0.5f).Within(0.001f));
        }

        [Test]
        public void Radius8_LargeHero_6040()
        {
            var p = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            p.PopulateDefaultSoftOverlap();
            var weights = p.ResolveWeights(8);
            Assert.That(weights[SizeBucket.Large], Is.EqualTo(0.6f).Within(0.001f));
            Assert.That(weights[SizeBucket.Hero], Is.EqualTo(0.4f).Within(0.001f));
        }

        [Test]
        public void Radius10_AllHero()
        {
            var p = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            p.PopulateDefaultSoftOverlap();
            var weights = p.ResolveWeights(10);
            Assert.That(weights.Count, Is.EqualTo(1));
            Assert.That(weights.ContainsKey(SizeBucket.Hero), Is.True);
            Assert.That(weights[SizeBucket.Hero], Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void Radius_OutOfRange_Empty()
        {
            var p = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            p.PopulateDefaultSoftOverlap();
            var weights = p.ResolveWeights(99);
            Assert.That(weights.Count, Is.EqualTo(0));
        }
    }
}
