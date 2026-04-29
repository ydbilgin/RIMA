using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    /// <summary>
    /// RageSystem mantığı testleri.
    /// Decay, TryConsume, OnDealDamage gibi saf mantık fonksiyonları.
    /// </summary>
    public class RageSystemTests
    {
        private RageSystem rage;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject("Player_Test");
            rage = go.AddComponent<RageSystem>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(rage.gameObject);
        }

        [Test]
        public void InitialRage_IsZero()
        {
            Assert.AreEqual(0, rage.CurrentRage, "Başlangıç rage 0 olmalı.");
        }

        [Test]
        public void AddRage_IncreasesRage()
        {
            rage.AddRage(30);
            Assert.AreEqual(30, rage.CurrentRage);
        }

        [Test]
        public void AddRage_CapsAtMaxRage()
        {
            rage.AddRage(999);
            Assert.AreEqual(rage.MaxRage, rage.CurrentRage, "Rage max değeri aşmamalı.");
        }

        [Test]
        public void TryConsume_SucceedsWhenEnough()
        {
            rage.AddRage(100);
            bool result = rage.TryConsume(100);
            Assert.IsTrue(result, "100 rage ile TryConsume(100) başarılı olmalı.");
            Assert.AreEqual(0, rage.CurrentRage, "Tüketim sonrası rage 0 olmalı.");
        }

        [Test]
        public void TryConsume_FailsWhenInsufficient()
        {
            rage.AddRage(50);
            bool result = rage.TryConsume(100);
            Assert.IsFalse(result, "50 rage ile TryConsume(100) başarısız olmalı.");
            Assert.AreEqual(50, rage.CurrentRage, "Başarısız tüketim sonrası rage değişmemeli.");
        }

        [Test]
        public void HasRage_ReturnsTrueWhenEnough()
        {
            rage.AddRage(80);
            Assert.IsTrue(rage.HasRage(80));
            Assert.IsTrue(rage.HasRage(50));
            Assert.IsFalse(rage.HasRage(100));
        }

        [Test]
        public void RagePercent_CalculatesCorrectly()
        {
            rage.AddRage(50);
            Assert.AreEqual(0.5f, rage.RagePercent, 0.01f, "50/100 = %50 olmalı.");
        }

        [Test]
        public void IsFury_SetAtThreshold()
        {
            rage.AddRage(49);
            Assert.IsFalse(rage.IsFury, "49 rage ile Fury olmamalı.");
            rage.AddRage(1); // 50
            Assert.IsTrue(rage.IsFury, "50 rage ile Fury aktif olmalı.");
        }

        [Test]
        public void IsBloodrage_SetAt80()
        {
            rage.AddRage(79);
            Assert.IsFalse(rage.IsBloodrage);
            rage.AddRage(1); // 80
            Assert.IsTrue(rage.IsBloodrage);
        }

        [Test]
        public void ResetRage_SetsToZero()
        {
            rage.AddRage(75);
            rage.ResetRage();
            Assert.AreEqual(0, rage.CurrentRage);
        }
    }
}
