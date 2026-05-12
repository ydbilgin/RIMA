using NUnit.Framework;
using RIMA.Combat.Skills;
using UnityEngine;

namespace RIMA.Tests
{
    public class SkillEffectDefTests
    {
        private SkillEffectDef def;

        [SetUp]
        public void SetUp()
        {
            def = ScriptableObject.CreateInstance<SkillEffectDef>();
            def.effectId = "test_effect";
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(def);
        }

        [Test]
        public void DefaultDef_IsValid()
        {
            Assert.IsTrue(def.Validate(out string error), error);
        }

        [Test]
        public void ProjectileDirectional8_RequiresEightDirectionalSprites()
        {
            def.angleMode = SkillEffectAngleMode.ProjectileDirectional8;

            Assert.IsTrue(def.RequiresDirectionalSprites());
            Assert.IsFalse(def.UsesRuntimeRotation());
        }

        [Test]
        public void ConeDirectional8_RequiresEightDirectionalSprites()
        {
            def.angleMode = SkillEffectAngleMode.Cone;
            def.coneRenderMode = SkillEffectConeRenderMode.Directional8;

            Assert.IsTrue(def.RequiresDirectionalSprites());
            Assert.IsFalse(def.UsesRuntimeRotation());
        }

        [Test]
        public void BeamRotated_UsesRuntimeRotation()
        {
            def.angleMode = SkillEffectAngleMode.BeamRotated;

            Assert.IsFalse(def.RequiresDirectionalSprites());
            Assert.IsTrue(def.UsesRuntimeRotation());
        }

        [Test]
        public void Radial_DoesNotUseDirectionalSpritesOrRuntimeRotation()
        {
            def.angleMode = SkillEffectAngleMode.Radial;

            Assert.IsFalse(def.RequiresDirectionalSprites());
            Assert.IsFalse(def.UsesRuntimeRotation());
        }

        [Test]
        public void Validate_FailsWithoutEffectId()
        {
            def.effectId = "";

            Assert.IsFalse(def.Validate(out string error));
            Assert.AreEqual("effectId is required", error);
        }
    }
}
