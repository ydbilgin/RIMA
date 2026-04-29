using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    /// <summary>
    /// Health sistemi EditMode testleri.
    /// EditMode'da Awake() çağrılmaz → SetMaxHP ile init ediyoruz.
    /// </summary>
    public class HealthTests
    {
        private Health sut;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject("Health_Test");
            sut = go.AddComponent<Health>();
            sut.SetMaxHP(100);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(sut.gameObject);
        }

        [Test]
        public void InitialHP_EqualsMaxHP()
        {
            Assert.AreEqual(100, sut.CurrentHP, "SetMaxHP sonrası HP=100 olmalı.");
            Assert.AreEqual(100, sut.MaxHP);
        }

        [Test]
        public void InitialState_IsNotDead()
        {
            Assert.IsFalse(sut.IsDead, "Başlangıçta IsDead=false olmalı.");
        }

        [Test]
        public void InitialState_IsNotImmune()
        {
            Assert.IsFalse(sut.IsImmune, "Başlangıçta IsImmune=false olmalı.");
        }

        [Test]
        public void TakeDamage_ReducesHP()
        {
            sut.TakeDamage(10);
            Assert.AreEqual(90, sut.CurrentHP, "100 - 10 = 90.");
        }

        [Test]
        public void TakeDamage_CannotGoBelowZero()
        {
            sut.TakeDamage(9999);
            Assert.AreEqual(0, sut.CurrentHP, "HP negatife düşmemeli.");
        }

        [Test]
        public void TakeDamage_ZeroDamage_StillDeals1()
        {
            sut.TakeDamage(0);
            Assert.AreEqual(99, sut.CurrentHP, "0 hasar bile minimum 1 etken hasar verir.");
        }

        [Test]
        public void TakeDamage_KillsAtZeroHP()
        {
            sut.SetMaxHP(10);
            sut.TakeDamage(10);
            Assert.IsTrue(sut.IsDead, "HP=0 iken IsDead=true olmalı.");
        }

        [Test]
        public void TakeDamage_IgnoredWhenDead()
        {
            sut.SetMaxHP(5);
            sut.TakeDamage(5);
            Assert.IsTrue(sut.IsDead);
            sut.TakeDamage(100);
            Assert.AreEqual(0, sut.CurrentHP, "Ölü iken hasar almaz.");
        }

        [Test]
        public void TakeDamage_IgnoredWhenImmune()
        {
            sut.SetImmune(true);
            sut.TakeDamage(50);
            Assert.AreEqual(100, sut.CurrentHP, "Immune iken hasar almamalı.");
        }

        [Test]
        public void IncomingDamageMultiplier_ReducesDamage()
        {
            sut.incomingDamageMultiplier = 0.5f;
            sut.TakeDamage(20);
            Assert.AreEqual(90, sut.CurrentHP, "0.5x multiplier ile 20 hasar → 10 etken.");
        }

        [Test]
        public void Heal_IncreasesHP()
        {
            sut.TakeDamage(30);
            sut.Heal(10);
            Assert.AreEqual(80, sut.CurrentHP, "70 + 10 = 80.");
        }

        [Test]
        public void Heal_CannotExceedMaxHP()
        {
            sut.TakeDamage(5);
            sut.Heal(9999);
            Assert.AreEqual(100, sut.CurrentHP, "Heal MaxHP'yi geçemez.");
        }

        [Test]
        public void Heal_IgnoredWhenDead()
        {
            sut.SetMaxHP(5);
            sut.TakeDamage(5);
            Assert.IsTrue(sut.IsDead);
            sut.Heal(100);
            Assert.AreEqual(0, sut.CurrentHP, "Ölü iken Heal çalışmaz.");
        }

        [Test]
        public void HealMultiplier_ReducesHealing()
        {
            sut.TakeDamage(50);
            sut.healMultiplier = 0f;
            sut.Heal(30);
            Assert.AreEqual(50, sut.CurrentHP, "healMultiplier=0 iken iyileşme yok.");
        }

        [Test]
        public void SetMaxHP_SetsCurrentToMax()
        {
            sut.SetMaxHP(200);
            Assert.AreEqual(200, sut.MaxHP);
            Assert.AreEqual(200, sut.CurrentHP);
        }

        [Test]
        public void ScaleMaxHP_MultipliesAndResets()
        {
            sut.SetMaxHP(50);
            sut.ScaleMaxHP(3);
            Assert.AreEqual(150, sut.MaxHP, "50 * 3 = 150.");
            Assert.AreEqual(150, sut.CurrentHP);
        }

        [Test]
        public void AddMaxHP_IncreasesMaxAndCurrent()
        {
            sut.TakeDamage(20);
            sut.AddMaxHP(20);
            Assert.AreEqual(120, sut.MaxHP);
            Assert.AreEqual(100, sut.CurrentHP);
        }

        [Test]
        public void RestoreToFull_SetsCurrentToMax()
        {
            sut.TakeDamage(50);
            sut.RestoreToFull();
            Assert.AreEqual(100, sut.CurrentHP);
        }

        [Test]
        public void SetImmune_TogglesState()
        {
            sut.SetImmune(true);
            Assert.IsTrue(sut.IsImmune);
            sut.SetImmune(false);
            Assert.IsFalse(sut.IsImmune);
        }

        [Test]
        public void OnDeath_FiresOnKill()
        {
            bool deathFired = false;
            sut.OnDeath ??= new UnityEngine.Events.UnityEvent();
            sut.OnDeath.AddListener(() => deathFired = true);
            sut.SetMaxHP(1);
            sut.TakeDamage(1);
            Assert.IsTrue(deathFired, "HP=0 olunca OnDeath tetiklenmeli.");
        }

        [Test]
        public void OnHealthChanged_FiresOnDamage()
        {
            int callbackCurrent = -1;
            sut.OnHealthChanged ??= new UnityEngine.Events.UnityEvent<int, int>();
            sut.OnHealthChanged.AddListener((cur, max) => callbackCurrent = cur);
            sut.TakeDamage(10);
            Assert.AreEqual(90, callbackCurrent, "OnHealthChanged 90 vermeli.");
        }
    }
}
