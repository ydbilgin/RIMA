using UnityEngine;

namespace RIMA
{
    public class MarkedDetonate : RangerSkillBase
    {
        [SerializeField] private float radius = 9f;
        [SerializeField] private int damagePerStack = 22;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Marked Detonate";
            cooldown = 7f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            int detonated = 0;
            foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, radius))
            {
                var state = SkillRuntime.State(health);
                int stacks = state != null ? state.GetStacks(SkillStateTracker.RangerMarked) : 0;
                if (stacks <= 0) continue;

                SkillRuntime.DealDamage(health, FocusDamage(damagePerStack * stacks));
                state.Remove(SkillStateTracker.RangerMarked);
                detonated++;
            }

            if (detonated == 0)
                cooldownTimer = 0f;
        }
    }
}
