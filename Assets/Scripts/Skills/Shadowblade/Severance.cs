using UnityEngine;

namespace RIMA
{
    public class Severance : ShadowbladeSkillBase
    {
        [SerializeField] private float radius = 9f;
        [SerializeField] private int damagePerScar = 26;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Severance";
            cooldown = 8f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            int collapsed = 0;
            foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, radius))
            {
                var state = SkillRuntime.State(health);
                int scarCount = state != null ? state.GetStacks(SkillStateTracker.RiftScar) : 0;
                if (scarCount < 1) continue;

                SkillRuntime.DealDamage(health, damagePerScar * scarCount);
                state.Remove(SkillStateTracker.RiftScar);
                shadow?.AddSever(50 * scarCount);
                collapsed += scarCount;
            }

            if (collapsed == 0)
                cooldownTimer = 0f;
        }
    }
}
