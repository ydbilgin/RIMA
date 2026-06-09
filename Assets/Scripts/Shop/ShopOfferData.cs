using System;
using UnityEngine;

namespace RIMA.Shop
{
    /// <summary>
    /// Identifies the three fixed shop offers in the Merchant room.
    /// </summary>
    public enum ShopOfferId
    {
        Heal,
        DamageBoost,
        MaxHPBoost
    }

    /// <summary>
    /// Plain data for a single shop offer: display name, cost, and the effect to apply.
    /// No MonoBehaviour — created by ShopRoomController and consumed by ShopStand.
    /// </summary>
    public sealed class ShopOfferData
    {
        public readonly ShopOfferId id;
        public readonly string displayName;
        public readonly string description;
        public readonly int cost;

        private readonly Action<GameObject> applyEffect;

        public ShopOfferData(ShopOfferId id, string displayName, string description, int cost, Action<GameObject> applyEffect)
        {
            this.id          = id;
            this.displayName = displayName;
            this.description = description;
            this.cost        = cost;
            this.applyEffect = applyEffect ?? throw new ArgumentNullException(nameof(applyEffect));
        }

        public void Apply(GameObject player)
        {
            if (player == null)
            {
                Debug.LogWarning($"[ShopOffer] Cannot apply '{displayName}': player is null.");
                return;
            }

            applyEffect(player);
        }

        // ── Factory: the three demo offers ─────────────────────────────────────────

        /// <summary>
        /// Offer 1 — Heal: 20 Echo → restore 30% of MaxHP.
        /// Uses Health.Heal(int) (rounds, clamped to maxHP).
        /// </summary>
        public static ShopOfferData CreateHeal()
        {
            return new ShopOfferData(
                ShopOfferId.Heal,
                "Restorative Shard",
                "Restore 30% Max HP",
                20,
                player =>
                {
                    Health health = player.GetComponent<Health>();
                    if (health == null)
                    {
                        Debug.LogWarning("[ShopOffer] Heal: player has no Health component.");
                        return;
                    }

                    int amount = Mathf.RoundToInt(health.MaxHP * 0.30f);
                    health.Heal(Mathf.Max(1, amount));
                    Debug.Log($"[ShopOffer] Healed player for {amount} HP (30% of MaxHP={health.MaxHP}).");
                });
        }

        /// <summary>
        /// Offer 2 — DamageBoost: 35 Echo → +12% outgoing damage this run.
        /// Multiplies PlayerAttack.outgoingDamageMultiplier by 1.12.
        /// </summary>
        public static ShopOfferData CreateDamageBoost()
        {
            return new ShopOfferData(
                ShopOfferId.DamageBoost,
                "Rift-Forged Edge",
                "+12% Damage this run",
                35,
                player =>
                {
                    PlayerAttack attack = player.GetComponent<PlayerAttack>();
                    if (attack == null)
                    {
                        Debug.LogWarning("[ShopOffer] DamageBoost: player has no PlayerAttack component.");
                        return;
                    }

                    attack.outgoingDamageMultiplier *= 1.12f;
                    Debug.Log($"[ShopOffer] DamageBoost applied. outgoingDamageMultiplier={attack.outgoingDamageMultiplier:F3}");
                });
        }

        /// <summary>
        /// Offer 3 — MaxHPBoost: 35 Echo → +20 Max HP.
        /// NOTE: Substituted for cooldown reduction (no global cooldown multiplier hook exists in SkillBase).
        /// Health.AddMaxHP(int) is a clean API that also adds to current HP.
        /// </summary>
        public static ShopOfferData CreateMaxHPBoost()
        {
            return new ShopOfferData(
                ShopOfferId.MaxHPBoost,
                "Vitality Crystal",
                "+20 Max HP",
                35,
                player =>
                {
                    Health health = player.GetComponent<Health>();
                    if (health == null)
                    {
                        Debug.LogWarning("[ShopOffer] MaxHPBoost: player has no Health component.");
                        return;
                    }

                    health.AddMaxHP(20);
                    Debug.Log($"[ShopOffer] MaxHPBoost applied. New MaxHP={health.MaxHP} CurrentHP={health.CurrentHP}");
                });
        }
    }
}
