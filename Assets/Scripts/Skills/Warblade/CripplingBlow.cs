using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Crippling Blow
    /// Yakın düşmana hasar ver + 6s iyileşme -%50.
    /// Chain: Iron Charge sonrası → iyileşme -%100.
    /// </summary>
    public class CripplingBlow : SkillBase
    {
        [Header("Crippling Blow")]
        [SerializeField] private float range = 1.8f;
        [SerializeField] private int baseDamage = 45;
        [SerializeField] private float healReduction = 0.5f;   // normal: %50 iyileşme
        [SerializeField] private float healReductionDuration = 6f;

        private IronCharge ironCharge;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Crippling Blow";
            cooldown = 7f;
            rageCost = 0;
            ironCharge = GetComponentInParent<IronCharge>();
        }

        protected override void Execute()
        {
            var target = FindNearest(range);
            if (target == null) return;

            target.TakeDamage(baseDamage);
            rage?.AddRage(10);

            bool chained = ironCharge != null && ironCharge.CooldownPercent > 0.85f;
            float reduction = chained ? 0f : healReduction;

            StartCoroutine(ApplyHealReduction(target, reduction, healReductionDuration));
        }

        private IEnumerator ApplyHealReduction(Health hp, float multiplier, float duration)
        {
            float previous = hp.healMultiplier;
            hp.healMultiplier = Mathf.Min(hp.healMultiplier, multiplier);
            yield return new WaitForSeconds(duration);
            if (hp != null) hp.healMultiplier = previous;
        }

        private Health FindNearest(float radius)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Default"));
            float best = float.MaxValue;
            Health bestHp = null;
            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < best) { best = d; bestHp = hp; }
            }
            return bestHp;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.8f, 0.1f, 0.1f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
