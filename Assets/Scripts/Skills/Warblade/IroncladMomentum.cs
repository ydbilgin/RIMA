using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Ironclad Momentum
    /// 6s: alınan hasar %30 azalır.
    /// Chain: War Stomp sonrası → savunma %50'ye çıkar.
    /// </summary>
    public class IroncladMomentum : SkillBase
    {
        [Header("Ironclad Momentum")]
        [SerializeField] private float duration = 6f;
        [SerializeField] private float damageReduction = 0.7f;       // %30 azaltma
        [SerializeField] private float chainDamageReduction = 0.5f;  // %50 azaltma (War Stomp chain)

        private Health playerHealth;
        private ChainWindowTracker chain;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Ironclad Momentum";
            cooldown = 14f;
            rageCost = 0;
            playerHealth = GetComponentInParent<Health>();
            chain = ChainWindowTracker.For(this);
        }

        protected override void Execute()
        {
            // A4: chained when War Stomp's follow-up window is open (was
            // `warStomp.CooldownPercent > 0.7f`). Consume — one stomp empowers one momentum cast.
            if (chain == null) chain = ChainWindowTracker.For(this);
            bool chained = chain != null && chain.Consume(ChainWindowTracker.WarStompFollowup);
            float mult = chained ? chainDamageReduction : damageReduction;
            StartCoroutine(ApplyBuff(mult));
        }

        private IEnumerator ApplyBuff(float multiplier)
        {
            if (playerHealth == null) yield break;
            float prev = playerHealth.incomingDamageMultiplier;
            playerHealth.incomingDamageMultiplier = Mathf.Min(prev, multiplier);
            yield return new WaitForSeconds(duration);
            if (playerHealth != null)
                playerHealth.incomingDamageMultiplier = prev;
        }
    }
}
