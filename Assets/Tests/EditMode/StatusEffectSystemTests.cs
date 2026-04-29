using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class StatusEffectSystemTests
    {
        private GameObject target;
        private Health health;
        private StatusEffectSystem status;

        [SetUp]
        public void SetUp()
        {
            target = new GameObject("StatusEffect_Test");
            health = target.AddComponent<Health>();
            health.SetMaxHP(100);
            status = target.AddComponent<StatusEffectSystem>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(target);
        }

        [Test]
        public void Poison_AccumulatesFractionalDamage()
        {
            status.ApplyEffect(StatusEffectType.Poison, 4f, 1);

            status.Tick(1f);
            Assert.AreEqual(100, health.CurrentHP, "0.5 damage should not become one damage every tick.");

            status.Tick(1f);
            Assert.AreEqual(99, health.CurrentHP, "Two seconds at 0.5 DPS should deal exactly one damage.");
        }

        [Test]
        public void Burning_ThreeStacks_UpgradesToScorch()
        {
            status.ApplyEffect(StatusEffectType.Burning, 3f, 1);
            status.ApplyEffect(StatusEffectType.Burning, 3f, 1);
            status.ApplyEffect(StatusEffectType.Burning, 3f, 1);

            Assert.IsFalse(status.HasEffect(StatusEffectType.Burning));
            Assert.IsTrue(status.HasEffect(StatusEffectType.Scorch));
            Assert.AreEqual(1.25f, status.damageMultiplierIncoming, 0.001f);
        }

        [Test]
        public void Chill_ThreeStacks_UpgradesToFrozen()
        {
            status.ApplyEffect(StatusEffectType.Chill, 3f, 1);
            status.ApplyEffect(StatusEffectType.Chill, 3f, 1);
            status.ApplyEffect(StatusEffectType.Chill, 3f, 1);

            Assert.IsFalse(status.HasEffect(StatusEffectType.Chill));
            Assert.IsTrue(status.HasEffect(StatusEffectType.Frozen));
            Assert.AreEqual(0f, status.moveSpeedMultiplier, 0.001f);
        }
    }
}
