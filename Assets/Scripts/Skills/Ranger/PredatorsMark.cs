using UnityEngine;

namespace RIMA
{
    public class PredatorsMark : RangerSkillBase
    {
        [SerializeField] private float radius = 4f;
        [SerializeField] private float markDuration = 10f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Predator's Mark";
            cooldown = 11f;
            resourceCost = 30;
        }

        protected override void Execute()
        {
            int maxTargets = focus != null && focus.IsDamageBoosted ? 5 : 3;
            int marked = 0;
            foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, radius))
            {
                if (marked >= maxTargets) break;
                Mark(health, markDuration, 2);
                marked++;
            }

            SkillRuntime.SpawnCircleVisual(transform.position, new Color(0.28f, 0.9f, 0.5f, 0.38f), 1.8f, 0.18f, "PredatorsMark_Runtime");
        }
    }
}
