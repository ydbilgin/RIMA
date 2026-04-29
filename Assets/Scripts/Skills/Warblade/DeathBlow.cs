using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Death Blow (Master)
    /// Sadece HP &lt;%30 düşmana kullanılabilir: %400 hasar, Rage boşaltır.
    /// Chain: Crippling Blow aktifken → %600 hasar.
    /// </summary>
    public class DeathBlow : SkillBase
    {
        [Header("Death Blow")]
        [SerializeField] private float range = 2f;
        [SerializeField] private float hpThreshold = 0.30f;  // %30 altı
        [SerializeField] private float damageMultiplier = 4f; // %400
        [SerializeField] private float chainMultiplier = 6f;  // %600 chain bonus
        [SerializeField] private int baseDamage = 40;

        private CripplingBlow cripplingBlow;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Death Blow";
            cooldown = 12f;
            rageCost = 0;
            cripplingBlow = GetComponentInParent<CripplingBlow>();
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

            bool chained = cripplingBlow != null && cripplingBlow.CooldownPercent > 0.5f;
            float mult = chained ? chainMultiplier : damageMultiplier;

            int damage = Mathf.RoundToInt(baseDamage * mult);
            target.TakeDamage(damage);

            // Rage boşalt
            if (rage != null)
                rage.TrySpend(rage.CurrentRage);
        }

        private Health FindExecuteTarget(float radius)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Default"));
            float best = float.MaxValue;
            Health bestHp = null;
            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                if ((float)hp.CurrentHP / hp.MaxHP > hpThreshold) continue; // sadece < %30

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
