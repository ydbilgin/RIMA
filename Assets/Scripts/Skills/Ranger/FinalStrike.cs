using UnityEngine;

namespace RIMA
{
    public class FinalStrike : RangerSkillBase
    {
        [SerializeField] private float range = 10f;
        [SerializeField] private int damage = 160;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Final Strike";
            cooldown = 12f;
            resourceCost = 40;
        }

        protected override void Execute()
        {
            Health target = null;
            float best = range * range;
            foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, range))
            {
                var state = SkillRuntime.State(health);
                if (state == null || !state.Has(SkillStateTracker.RangerMarked) || !state.Has(SkillStateTracker.Trapped))
                    continue;

                float dist = ((Vector2)health.transform.position - (Vector2)transform.position).sqrMagnitude;
                if (dist < best)
                {
                    best = dist;
                    target = health;
                }
            }

            if (target == null)
            {
                cooldownTimer = 0f;
                return;
            }

            SkillRuntime.DealDamage(target, FocusDamage(damage));
            var targetState = SkillRuntime.State(target);
            targetState?.Remove(SkillStateTracker.RangerMarked);
            targetState?.Remove(SkillStateTracker.Trapped);
            focus?.Add(20);
        }
    }
}
