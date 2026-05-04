using UnityEngine;

namespace RIMA
{
    public class BoneTrap : RangerSkillBase
    {
        [SerializeField] private float radius = 2.2f;
        [SerializeField] private float duration = 8f;
        [SerializeField] private int damage = 18;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Bone Trap";
            cooldown = 9f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 center = (Vector2)transform.position + dir.normalized * 3f;
            SkillRuntime.SpawnCircleVisual(center, new Color(0.78f, 0.92f, 0.58f, 0.45f), 1.25f, duration, "BoneTrap_Runtime");

            foreach (var health in SkillRuntime.EnemiesInCircle(center, radius))
            {
                SkillRuntime.DealDamage(health, FocusDamage(damage));
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 1.25f);
                Trap(health, duration);
                Mark(health, duration);
            }
        }
    }
}
