using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Sunder Mark
    /// Hedefe 8s işaret: zırh -%40 (Weakened × 1.6 yaklaşımı).
    /// StatusEffectSystem.Weakened zaten +25% incoming var; bunu 2 stack uyguluyoruz.
    /// Chain: Death Blow aktifken → zırh -%60 (3 stack).
    /// </summary>
    public class SunderMark : SkillBase
    {
        [Header("Sunder Mark")]
        [SerializeField] private float range = 10f;  // ranged — fırlatma
        [SerializeField] private float duration = 8f;
        [SerializeField] private int weakenStacks = 2;   // ~%40 zırh kırma

        private ChainWindowTracker chain;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Sunder Mark";
            cooldown = 14f;
            rageCost = 0;
            chain = ChainWindowTracker.For(this);
        }

        // FIX-2 wiring: read-only mirror of Execute's primary no-op gate (`FindNearest(range) == null`).
        // Same OverlapCircleAll(range, "Enemy") query, no side effects — rejects the throw before
        // cost/cooldown when nothing is in range. (Execute also returns if the found target lacks a
        // StatusEffectSystem, but that secondary guard is not mirrored to avoid blocking a valid cast.)
        protected override bool CanExecute()
        {
            return FindNearest(range) != null;
        }

        protected override void Execute()
        {
            var target = FindNearest(range);
            if (target == null) return;

            var status = target.GetComponent<StatusEffectSystem>();
            if (status == null) return;

            // A4: 3-stack chain when Death Blow's follow-up window is open (was
            // `deathBlow.CooldownPercent > 0.5f`). Consume — one Death Blow empowers one Sunder Mark.
            if (chain == null) chain = ChainWindowTracker.For(this);
            bool chained = chain != null && chain.Consume(ChainWindowTracker.SunderExecute);
            int stacks = chained ? 3 : weakenStacks;

            status.ApplyEffect(StatusEffectType.Weakened, duration, stacks);
            SkillRuntime.State(target)?.Apply(SkillStateTracker.Sundered, duration, stacks, 3);
            rage?.AddRage(5);
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
            Gizmos.color = new Color(0.6f, 0.1f, 0.9f, 0.2f);
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
