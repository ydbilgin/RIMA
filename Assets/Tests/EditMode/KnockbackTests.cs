using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    /// <summary>
    /// KnockbackReceiver/Component EditMode testleri.
    /// ApplyKnockback Coroutine kullanıyor → bileşen varlık testleri.
    /// </summary>
    public class KnockbackTests
    {
        private GameObject target;
        private Rigidbody2D rb;

        [SetUp]
        public void SetUp()
        {
            target = new GameObject("Knockback_Test");
            rb = target.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(target);
        }

        [Test]
        public void Receiver_CanBeAdded()
        {
            var receiver = target.AddComponent<KnockbackReceiver>();
            Assert.IsNotNull(receiver, "KnockbackReceiver eklenebilmeli.");
        }

        [Test]
        public void Receiver_HasRigidbody()
        {
            target.AddComponent<KnockbackReceiver>();
            Assert.IsNotNull(target.GetComponent<Rigidbody2D>(), "Rigidbody2D olmalı.");
        }

        [Test]
        public void KnockbackComponent_CanBeAdded()
        {
            var attacker = new GameObject("Attacker");
            var comp = attacker.AddComponent<KnockbackComponent>();
            Assert.IsNotNull(comp, "KnockbackComponent eklenebilmeli.");
            Assert.IsNotNull(attacker.GetComponent<KnockbackReceiver>(), "Legacy adapter KnockbackReceiver eklemeli.");
            Object.DestroyImmediate(attacker);
        }

        [Test]
        public void HitImpulse_OnlyKnockdownWhenBrokenOrSundered()
        {
            var impulse = new HitImpulse(Vector2.right, 8f, 0.18f, canKnockdown: true);

            Assert.IsFalse(impulse.ShouldKnockdown(target), "State yokken knockdown olmamalı.");

            var tracker = target.AddComponent<SkillStateTracker>();
            tracker.Apply(SkillStateTracker.Broken, 1f);

            Assert.IsTrue(impulse.ShouldKnockdown(target), "Broken hedef knockdown tetiklemeli.");
        }

        [Test]
        public void BasicAttackProfile_FinalLegacyStepBuildsHeavyImpulse()
        {
            var profile = ScriptableObject.CreateInstance<BasicAttackProfile>();
            profile.comboLength = 3;
            profile.knockbackForce = new[] { 4f, 5f, 8f };
            profile.knockbackDuration = new[] { 0.10f, 0.12f, 0.18f };

            HitImpulse light = profile.GetImpulseForStep(0, Vector2.left);
            HitImpulse heavy = profile.GetImpulseForStep(2, Vector2.left);

            Assert.IsFalse(light.canKnockdown);
            Assert.IsTrue(heavy.canKnockdown);
            Assert.AreEqual(8f, heavy.force);
            Assert.AreEqual(0.18f, heavy.duration);

            Object.DestroyImmediate(profile);
        }

        [Test]
        public void KnockdownProfile_ClampsUnsafeRuntimeValues()
        {
            var profile = ScriptableObject.CreateInstance<KnockdownProfile>();
            profile.launchDuration = -1f;
            profile.landingSquashY = 0.05f;
            profile.bounceCount = 99;

            Assert.Greater(profile.LaunchDuration, 0f);
            Assert.AreEqual(0.2f, profile.LandingSquashY);
            Assert.AreEqual(2, profile.BounceCount);

            Object.DestroyImmediate(profile);
        }
    }
}
