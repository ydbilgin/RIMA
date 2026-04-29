using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Iron Crush
    /// 6s: temel saldırı hasarı +%30.
    /// Chain: Burst window aktifken katlanır (Faz 2).
    /// </summary>
    public class IronCrush : SkillBase
    {
        [Header("Iron Crush")]
        [SerializeField] private float duration = 6f;
        [SerializeField] private float damageBonus = 0.3f; // +%30

        private PlayerAttack playerAttack;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Iron Crush";
            cooldown = 12f;
            rageCost = 30;
            playerAttack = GetComponentInParent<PlayerAttack>();
        }

        protected override void Execute()
        {
            StartCoroutine(ApplyBuff());
        }

        private IEnumerator ApplyBuff()
        {
            if (playerAttack == null) yield break;
            playerAttack.outgoingDamageMultiplier += damageBonus;
            yield return new WaitForSeconds(duration);
            if (playerAttack != null)
                playerAttack.outgoingDamageMultiplier -= damageBonus;
        }
    }
}
