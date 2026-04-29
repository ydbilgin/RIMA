using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 10 — Toxic Eruption (Advanced)
    /// 5 CP: hedefteki tüm debuffları patlatır. Her zehir/kanama stack başına %150 hasar.
    /// Hemorrhage aktif hedefe → debufflar tüketilmez + hasar +%50.
    /// </summary>
    public class ToxicEruption : SkillBase
    {
        [Header("Toxic Eruption")]
        [SerializeField] private int   damagePerStack  = 25;
        [SerializeField] private float spreadRadius    = 2.5f;
        [SerializeField] private float attackRange     = 2f;

        private ComboPointSystem combo;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Toxic Eruption";
            cooldown = 10f;
            resourceCost = 0;
            combo = GetComponentInParent<ComboPointSystem>();
        }

        protected override void Execute()
        {
            if (combo == null) return;
            int pts = combo.PeekAndSpend();
            if (pts < 1) return;

            var target = Hemorrhage.FindNearestEnemy(transform.position, attackRange);
            if (target == null) return;

            var status = target.GetComponent<StatusEffectSystem>();
            int stacks = 0;
            if (status != null)
                stacks = status.GetStacks(StatusEffectType.Poison) + status.GetStacks(StatusEffectType.Burning);

            bool hasHemorrhage = status != null && status.GetStacks(StatusEffectType.Poison) > 0;
            int dmg = Mathf.Max(stacks, 1) * damagePerStack;
            if (!hasHemorrhage) dmg = Mathf.RoundToInt(dmg * 1.5f);

            target.GetComponent<Health>()?.TakeDamage(dmg);

            // Yakına yayıl
            var nearby = Physics2D.OverlapCircleAll(target.transform.position, spreadRadius);
            foreach (var h in nearby)
            {
                if (h.gameObject == target.gameObject) continue;
                if (h.CompareTag("Player")) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                hp.TakeDamage(Mathf.RoundToInt(dmg * 0.5f));
                h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Poison, 4f);
            }
        }
    }
}
