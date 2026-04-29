using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    /// <summary>
    /// PlayerEconomy (gold) sistemi EditMode testleri.
    /// AddGold, TrySpend, ResetGold.
    /// </summary>
    public class PlayerEconomyTests
    {
        private PlayerEconomy sut;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject("Economy_Test");
            sut = go.AddComponent<PlayerEconomy>();
            // Awake singleton set eder, startingGold=0 default
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(sut.gameObject);
        }

        // ── Başlangıç ────────────────────────────────────

        [Test]
        public void InitialGold_IsZero()
        {
            Assert.AreEqual(0, sut.Gold, "Başlangıç gold 0 olmalı.");
        }

        // ── AddGold ──────────────────────────────────────

        [Test]
        public void AddGold_IncreasesTotal()
        {
            sut.AddGold(50);
            Assert.AreEqual(50, sut.Gold, "50 gold eklenmeli.");
        }

        [Test]
        public void AddGold_Accumulates()
        {
            sut.AddGold(10);
            sut.AddGold(20);
            Assert.AreEqual(30, sut.Gold, "10+20 = 30 gold.");
        }

        [Test]
        public void AddGold_NegativeClamps()
        {
            sut.AddGold(10);
            sut.AddGold(-20);
            Assert.AreEqual(0, sut.Gold, "Negatif ekleme 0'dan düşmemeli.");
        }

        // ── TrySpend ─────────────────────────────────────

        [Test]
        public void TrySpend_SucceedsWhenSufficient()
        {
            sut.AddGold(100);
            bool result = sut.TrySpend(60);
            Assert.IsTrue(result, "100 gold ile 60 harcanabilir.");
            Assert.AreEqual(40, sut.Gold, "100-60 = 40 kalmalı.");
        }

        [Test]
        public void TrySpend_FailsWhenInsufficient()
        {
            sut.AddGold(30);
            bool result = sut.TrySpend(50);
            Assert.IsFalse(result, "30 gold ile 50 harcanamaz.");
            Assert.AreEqual(30, sut.Gold, "Başarısız TrySpend gold değiştirmemeli.");
        }

        [Test]
        public void TrySpend_ExactAmount_Succeeds()
        {
            sut.AddGold(25);
            bool result = sut.TrySpend(25);
            Assert.IsTrue(result, "25=25 tam harcama başarılı olmalı.");
            Assert.AreEqual(0, sut.Gold, "Kalan 0 olmalı.");
        }

        // ── ResetGold ────────────────────────────────────

        [Test]
        public void ResetGold_SetsToStarting()
        {
            sut.AddGold(999);
            sut.ResetGold();
            Assert.AreEqual(0, sut.Gold, "Reset sonrası gold startingGold (0) olmalı.");
        }

        // ── Event ────────────────────────────────────────

        [Test]
        public void OnGoldChanged_FiresOnAdd()
        {
            int callbackValue = -1;
            sut.OnGoldChanged.AddListener(val => callbackValue = val);
            sut.AddGold(42);
            Assert.AreEqual(42, callbackValue, "OnGoldChanged doğru toplam vermeli.");
        }

        [Test]
        public void OnGoldChanged_FiresOnSpend()
        {
            sut.AddGold(100);
            int callbackValue = -1;
            sut.OnGoldChanged.AddListener(val => callbackValue = val);
            sut.TrySpend(30);
            Assert.AreEqual(70, callbackValue, "Harcama sonrası callback 70 vermeli.");
        }
    }
}
