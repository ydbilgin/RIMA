using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Death Blow (Master)
    /// Canon: yalnızca Broken/Sundered düşmana kullanılabilir (generic HP&lt;%X execute YASAK,
    /// R3 lock). %400 hasar, Rage boşaltır. Chain: Crippling Blow aktifken → %600 hasar.
    /// Heal/refund Death Blow'da DEĞİL — Battle Surge'ün Rage-harca → +%5 HP loop'undan gelir.
    /// </summary>
    public class DeathBlow : SkillBase
    {
        [Header("Death Blow")]
        [SerializeField] private float range = 2f;
        [SerializeField] private float damageMultiplier = 4f; // %400
        [SerializeField] private float chainMultiplier = 6f;  // %600 chain bonus
        [SerializeField] private int baseDamage = 40;

        [Header("Execute gate (canon = state-only)")]
        [Tooltip("CANON: keep OFF. Generic HP<threshold execute is banned (R3 lock); execute " +
                 "requires Broken/Sundered. Exposed only as a debug/tuning escape hatch.")]
        [SerializeField] private bool allowLowHpExecute = false;
        [SerializeField] private float hpThreshold = 0.30f;  // only used if allowLowHpExecute

        private ChainWindowTracker chain;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Death Blow";
            cooldown = 12f;
            rageCost = 0;
            chain = ChainWindowTracker.For(this);
        }

        protected override void Execute()
        {
            var target = FindExecuteTarget(range);
            if (target == null)
            {
                // Hedef yoksa cooldown başlatma
                cooldownTimer = 0f;
                return;
            }

            // A4: ×6 chain when Crippling Blow's follow-up window is open (was
            // `cripplingBlow.CooldownPercent > 0.5f`). Consume — a single Crippling Blow grants one
            // empowered Death Blow.
            if (chain == null) chain = ChainWindowTracker.For(this);
            bool chained = chain != null && chain.Consume(ChainWindowTracker.CripplingExecute);
            float mult = chained ? chainMultiplier : damageMultiplier;

            int damage = Mathf.RoundToInt(baseDamage * mult);
            SkillRuntime.DealDamage(target, damage, this);

            // Consume the gating state so a survivor can't be re-executed for free
            // (prevents an infinite execute loop). Sundered is the escalated state, prefer it.
            var state = SkillRuntime.State(target);
            if (state != null)
            {
                if (state.Has(SkillStateTracker.Sundered)) state.Consume(SkillStateTracker.Sundered);
                else if (state.Has(SkillStateTracker.Broken)) state.Consume(SkillStateTracker.Broken);
            }

            // Canon: empty the Rage bar. This single spend event is the heal trigger —
            // Battle Surge (if active) heals +%5 HP per spend (canon loop). No heal/refund here.
            if (rage != null)
                rage.TrySpend(rage.CurrentRage);

            // A4: open the follow-up window read by Sunder Mark (was Sunder Mark reading
            // `deathBlow.CooldownPercent > 0.5f`). Generous window preserves the old loose feel.
            chain?.OpenWindow(ChainWindowTracker.SunderExecute, 6f);
        }

        private Health FindExecuteTarget(float radius)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            float best = float.MaxValue;
            Health bestHp = null;
            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                var state = SkillRuntime.State(hp);
                bool markedForExecution = state != null &&
                    (state.Has(SkillStateTracker.Sundered) || state.Has(SkillStateTracker.Broken));
                // CANON: no generic HP<threshold execute (R3 lock). The lowHp path is gated OFF
                // by default and only exists as a debug/tuning escape hatch (allowLowHpExecute).
                bool lowHp = allowLowHpExecute && (float)hp.CurrentHP / hp.MaxHP <= hpThreshold;
                if (!markedForExecution && !lowHp) continue;

                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < best) { best = d; bestHp = hp; }
            }
            return bestHp;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.9f, 0f, 0f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
