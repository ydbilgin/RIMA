using UnityEngine;
using System.Collections.Generic;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 9 — Chain Lightning
    /// 5 hedefe sekiyor (yavaşlamış hedef → 7 seki).
    /// </summary>
    public class ChainLightning : SkillBase
    {
        [Header("Chain Lightning")]
        [SerializeField] private int   damage    = 40;
        [SerializeField] private int   baseChain = 5;
        [SerializeField] private float jumpRange = 6f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Chain Lightning";
            cooldown = 7f;
            resourceCost = 40;
        }

        protected override void Execute()
        {
            var first = FindNearestEnemy(transform.position, null);
            if (first == null) return;

            var hit = new HashSet<Collider2D>();
            Collider2D current = first;

            var status = first.GetComponent<StatusEffectSystem>();
            int maxChain = (status != null && status.GetStacks(StatusEffectType.Chill) > 0) ? 7 : baseChain;

            for (int i = 0; i < maxChain; i++)
            {
                if (current == null) break;
                var hp = current.GetComponent<Health>();
                if (hp != null && !hp.IsDead) hp.TakeDamage(damage);
                hit.Add(current);

                current = FindNearestEnemy(current.transform.position, hit);
            }
        }

        private Collider2D FindNearestEnemy(Vector2 from, HashSet<Collider2D> exclude)
        {
            var cols = Physics2D.OverlapCircleAll(from, jumpRange);
            float minD = float.MaxValue;
            Collider2D best = null;
            foreach (var c in cols)
            {
                if (c.CompareTag("Player")) continue;
                if (exclude != null && exclude.Contains(c)) continue;
                if (c.GetComponent<Health>() == null) continue;
                float d = Vector2.Distance(from, c.transform.position);
                if (d < minD) { minD = d; best = c; }
            }
            return best;
        }
    }
}
