using NUnit.Framework;
using UnityEngine;
using RIMA;
using RIMA.Shop;

namespace RIMA.Tests.Shop
{
    /// <summary>
    /// EditMode tests for ShopOfferData effects and EchoWallet integration.
    /// Validates: TrySpend reduces balance, offer effects mutate stats correctly.
    /// </summary>
    public class ShopOfferTests
    {
        private GameObject playerGO;
        private Health health;
        private PlayerAttack attack;

        [SetUp]
        public void SetUp()
        {
            // Reset EchoWallet to a known state.
            PlayerPrefs.SetInt(EchoWallet.EchoBalancePrefsKey, 200);
            PlayerPrefs.Save();

            // Build a minimal player GameObject.
            playerGO = new GameObject("TestPlayer");
            playerGO.tag = "Player";

            // Health requires no dependencies.
            health = playerGO.AddComponent<Health>();

            // PlayerAttack requires PlayerController.
            playerGO.AddComponent<Rigidbody2D>();
            playerGO.AddComponent<PlayerController>();
            attack = playerGO.AddComponent<PlayerAttack>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(playerGO);
            // Reset wallet.
            PlayerPrefs.DeleteKey(EchoWallet.EchoBalancePrefsKey);
        }

        // ── EchoWallet ─────────────────────────────────────────────────────────────

        [Test]
        public void TrySpend_ReducesBalanceByAmount()
        {
            int before = EchoWallet.Balance;
            bool result = EchoWallet.TrySpend(20);

            Assert.IsTrue(result, "TrySpend should succeed when balance is sufficient.");
            Assert.AreEqual(before - 20, EchoWallet.Balance, "Balance should decrease by the spend amount.");
        }

        [Test]
        public void TrySpend_ReturnsFalse_WhenInsufficientBalance()
        {
            PlayerPrefs.SetInt(EchoWallet.EchoBalancePrefsKey, 10);

            bool result = EchoWallet.TrySpend(20);

            Assert.IsFalse(result, "TrySpend should fail when balance < cost.");
            Assert.AreEqual(10, EchoWallet.Balance, "Balance should be unchanged after failed spend.");
        }

        // ── Heal offer ─────────────────────────────────────────────────────────────

        [Test]
        public void HealOffer_RestoresThirtyPercentOfMaxHP()
        {
            // EditMode: Awake is not invoked — use SetMaxHP to initialize currentHP,
            // then TakeDamage so there is room to heal.
            health.SetMaxHP(100);         // sets maxHP=100, currentHP=100
            health.TakeDamage(50);        // currentHP → 50
            int maxHP     = health.MaxHP; // 100
            int hpBefore  = health.CurrentHP; // 50

            ShopOfferData offer = ShopOfferData.CreateHeal();
            offer.Apply(playerGO);

            int expected = Mathf.Min(maxHP, hpBefore + Mathf.RoundToInt(maxHP * 0.30f));
            Assert.AreEqual(expected, health.CurrentHP,
                "Heal offer should restore 30% of MaxHP.");
        }

        [Test]
        public void HealOffer_CostIs20Echo()
        {
            Assert.AreEqual(20, ShopOfferData.CreateHeal().cost);
        }

        // ── DamageBoost offer ──────────────────────────────────────────────────────

        [Test]
        public void DamageBoostOffer_IncreasesOutgoingDamageMultiplier()
        {
            float before = attack.outgoingDamageMultiplier;
            ShopOfferData offer = ShopOfferData.CreateDamageBoost();
            offer.Apply(playerGO);

            Assert.AreEqual(before * 1.12f, attack.outgoingDamageMultiplier, 0.0001f,
                "DamageBoost should multiply outgoingDamageMultiplier by 1.12.");
        }

        [Test]
        public void DamageBoostOffer_CostIs35Echo()
        {
            Assert.AreEqual(35, ShopOfferData.CreateDamageBoost().cost);
        }

        // ── MaxHPBoost offer ───────────────────────────────────────────────────────

        [Test]
        public void MaxHPBoostOffer_AddsMaxHP()
        {
            int maxBefore = health.MaxHP;
            ShopOfferData offer = ShopOfferData.CreateMaxHPBoost();
            offer.Apply(playerGO);

            Assert.AreEqual(maxBefore + 20, health.MaxHP,
                "MaxHPBoost should add 20 to MaxHP.");
        }

        [Test]
        public void MaxHPBoostOffer_CostIs35Echo()
        {
            Assert.AreEqual(35, ShopOfferData.CreateMaxHPBoost().cost);
        }
    }
}
