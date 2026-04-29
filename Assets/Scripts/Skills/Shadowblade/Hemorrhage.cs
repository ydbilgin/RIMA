using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 2 — Hemorrhage
    /// Bleed DoT 8s + 2 CP. Öldürünce yakına yayılır.
    /// </summary>
    public class Hemorrhage : SkillBase
    {
        [Header("Hemorrhage")]
        [SerializeField] private int   damage     = 20;
        [SerializeField] private float bleedDuration = 8f;
        [SerializeField] private float spreadRadius  = 3f;
        [SerializeField] private float attackRange   = 1.5f;

        private ComboPointSystem combo;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Hemorrhage";
            cooldown = 3f;
            resourceCost = 0;
            combo = GetComponentInParent<ComboPointSystem>();
        }

        protected override void Execute()
        {
            var target = FindNearestEnemy(transform.position, attackRange);
            if (target == null) return;

            var hp = target.GetComponent<Health>();
            if (hp == null || hp.IsDead) return;

            hp.TakeDamage(damage);
            target.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Poison, bleedDuration);
            combo?.Add(2);

            hp.OnDeath.AddListener(() =>
            {
                var nearby = Physics2D.OverlapCircleAll(target.transform.position, spreadRadius);
                foreach (var h in nearby)
                {
                    if (h.gameObject == target.gameObject) continue;
                    if (h.CompareTag("Player")) continue;
                    h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Poison, bleedDuration);
                }
            });
        }

        public static Collider2D FindNearestEnemy(Vector2 from, float range)
        {
            var hits = Physics2D.OverlapCircleAll(from, range);
            float minD = float.MaxValue;
            Collider2D best = null;
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                if (h.GetComponent<Health>() == null) continue;
                float d = Vector2.Distance(from, h.transform.position);
                if (d < minD) { minD = d; best = h; }
            }
            return best;
        }
    }
}
