using UnityEngine;

namespace RIMA
{
    public class SweepVolley : RangerSkillBase
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private float halfAngle = 45f;
        [SerializeField] private int damage = 28;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Sweep Volley";
            cooldown = 8f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            foreach (var health in SkillRuntime.EnemiesInCone(transform.position, dir, radius, halfAngle))
            {
                SkillRuntime.DealDamage(health, FocusDamage(damage));
                Mark(health, 6f);
            }

            SkillRuntime.SpawnCircleVisual((Vector2)transform.position + dir * 2f, new Color(0.36f, 0.8f, 0.36f, 0.45f), 1.4f, 0.18f, "SweepVolley_Runtime");
        }
    }
}
