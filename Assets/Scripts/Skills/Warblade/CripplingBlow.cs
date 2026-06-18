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

        private ChainWindowTracker chain;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Crippling Blow";
            cooldown = 7f;
            rageCost = 0;
            chain = ChainWindowTracker.For(this);
        }

        // FIX-2 wiring: read-only mirror of Execute's no-op gate (`FindNearest(range) == null`).
        // Same OverlapCircleAll(range, "Enemy") query, no side effects — rejects the cast before
        // cost/cooldown when no enemy is in melee range.
        protected override bool CanExecute()
        {
            return FindNearest(range) != null;
        }

        protected override void Execute()
        {
            var target = FindNearest(range);
            if (target == null) return;

            SkillRuntime.DealDamage(target, baseDamage, this);
            rage?.AddRage(10);

            // A4: chained when Iron Charge's follow-up window is open (was
            // `ironCharge.CooldownPercent > 0.85f`). Read-only (IsOpen) — the original proxy let any
            // follow-up skill chain off one charge, so Gravity Cleave can also benefit; don't consume.
            if (chain == null) chain = ChainWindowTracker.For(this);
            bool chained = chain != null && chain.IsOpen(ChainWindowTracker.IronChargeNextHit);
            float reduction = chained ? 0f : healReduction;

            StartCoroutine(ApplyHealReduction(target, reduction, healReductionDuration));

            // A4: open the follow-up window read by Death Blow (was Death Blow reading
            // `cripplingBlow.CooldownPercent > 0.5f`). Generous window preserves the old loose feel.
            chain?.OpenWindow(ChainWindowTracker.CripplingExecute, 5f);
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
            var hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
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
